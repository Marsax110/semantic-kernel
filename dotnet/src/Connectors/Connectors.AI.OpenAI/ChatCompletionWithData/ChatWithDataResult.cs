﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;

namespace Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletionWithData;

[Experimental("SKEXP0010")]
internal sealed class ChatWithDataResult : IChatResult, ITextResult
{
    public ModelResult ModelResult { get; }

    public ChatWithDataResult(ChatWithDataResponse response, ChatWithDataChoice choice)
    {
        Verify.NotNull(response);
        Verify.NotNull(choice);

        this.ModelResult = new(new ChatWithDataModelResult(response.Id, DateTimeOffset.FromUnixTimeSeconds(response.Created))
        {
            ToolContent = this.GetToolContent(choice)
        });

        this._choice = choice;
    }

    public Task<ChatMessage> GetChatMessageAsync(CancellationToken cancellationToken = default)
    {
        var message = this._choice.Messages
            .FirstOrDefault(message => message.Role.Equals(AuthorRole.Assistant.Label, StringComparison.Ordinal));

        return message is not null ?
            Task.FromResult<ChatMessage>(new AzureOpenAIChatMessage(message.Role, message.Content)) :
            Task.FromException<ChatMessage>(new KernelException("No message found"));
    }

    public async Task<string> GetCompletionAsync(CancellationToken cancellationToken = default)
    {
        var message = await this.GetChatMessageAsync(cancellationToken).ConfigureAwait(false);

        return message.Content;
    }

    #region private ================================================================================

    private readonly ChatWithDataChoice _choice;

    private string? GetToolContent(ChatWithDataChoice choice)
    {
        var message = choice.Messages
            .FirstOrDefault(message => message.Role.Equals(AuthorRole.Tool.Label, StringComparison.Ordinal));

        return message?.Content;
    }

    #endregion
}
