﻿// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel;

namespace Microsoft.SemanticKernel.Plugins.Core;

/// <summary>
/// MathPlugin provides a set of functions to make Math calculations.
/// </summary>
/// <example>
/// Usage: kernel.ImportFunctions(new MathPlugin(), "math");
/// Examples:
/// {{math.Add}}         => Returns the sum of FirstNumber and SecondNumber (provided in the SKContext)
/// </example>
public sealed class MathPlugin
{
    /// <summary>
    /// Returns the Addition result of initial and amount values provided.
    /// </summary>
    /// <param name="value">Initial value to which to add the specified amount</param>
    /// <param name="amount">The amount to add as a string.</param>
    /// <returns>The resulting sum as a string.</returns>
    [KernelFunction, Description("Adds an amount to a value")]
    [return: Description("The sum")]
    public int Add(
        [Description("The value to add")] int value,
        [Description("Amount to add")] int amount) =>
        value + amount;

    /// <summary>
    /// Returns the Sum of two provided numbers.
    /// </summary>
    /// <param name="value">Initial value from which to subtract the specified amount</param>
    /// <param name="amount">The amount to subtract as a string.</param>
    /// <returns>The resulting subtraction as a string.</returns>
    [KernelFunction, Description("Subtracts an amount from a value")]
    [return: Description("The difference")]
    public int Subtract(
        [Description("The value to subtract")] int value,
        [Description("Amount to subtract")] int amount) =>
        value - amount;
}
