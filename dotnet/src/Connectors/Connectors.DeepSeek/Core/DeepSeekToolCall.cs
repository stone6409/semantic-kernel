// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;

namespace Microsoft.SemanticKernel.Connectors.DeepSeek;

/// <summary>
/// Represents the kind of tool call.
/// </summary>
public enum DeepSeekToolCallKind
{
    /// <summary>
    /// A function call.
    /// </summary>
    Function,
}

/// <summary>
/// Represents a tool call made by the model.
/// </summary>
public sealed class DeepSeekToolCall
{
    /// <summary>
    /// Gets the kind of tool call.
    /// </summary>
    public DeepSeekToolCallKind Kind { get; }

    /// <summary>
    /// Gets the ID of the tool call.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the name of the function to call.
    /// </summary>
    public string FunctionName { get; }

    /// <summary>
    /// Gets the arguments to pass to the function.
    /// </summary>
    public IReadOnlyDictionary<string, object> FunctionArguments { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeepSeekToolCall"/> class.
    /// </summary>
    /// <param name="id">The ID of the tool call.</param>
    /// <param name="functionName">The name of the function to call.</param>
    /// <param name="functionArguments">The arguments to pass to the function.</param>
    public DeepSeekToolCall(string id, string functionName, IReadOnlyDictionary<string, object> functionArguments)
    {
        this.Kind = DeepSeekToolCallKind.Function;
        this.Id = id;
        this.FunctionName = functionName;
        this.FunctionArguments = functionArguments;
    }
}
