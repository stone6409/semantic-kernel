// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;

namespace Microsoft.SemanticKernel.Connectors.DeepSeek;

/// <summary>
/// Represents a function tool call made by the DeepSeek model.
/// </summary>
public sealed class DeepSeekFunctionToolCall
{
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
    /// Initializes a new instance of the <see cref="DeepSeekFunctionToolCall"/> class.
    /// </summary>
    /// <param name="toolCall">The tool call to convert.</param>
    public DeepSeekFunctionToolCall(DeepSeekToolCall toolCall)
    {
        this.Id = toolCall.Id;
        this.FunctionName = toolCall.FunctionName;
        this.FunctionArguments = toolCall.FunctionArguments;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeepSeekFunctionToolCall"/> class.
    /// </summary>
    /// <param name="id">The ID of the tool call.</param>
    /// <param name="functionName">The name of the function to call.</param>
    /// <param name="functionArguments">The arguments to pass to the function.</param>
    public DeepSeekFunctionToolCall(string id, string functionName, IReadOnlyDictionary<string, object> functionArguments)
    {
        this.Id = id;
        this.FunctionName = functionName;
        this.FunctionArguments = functionArguments;
    }
}
