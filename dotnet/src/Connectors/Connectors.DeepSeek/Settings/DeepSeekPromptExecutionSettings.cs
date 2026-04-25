// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;


namespace Microsoft.SemanticKernel.Connectors.DeepSeek;

/// <summary>
/// DeepSeek prompt execution settings.
/// </summary>
public class DeepSeekPromptExecutionSettings : PromptExecutionSettings
{
    /// <summary>
    /// Gets or sets the maximum number of tokens to generate.
    /// </summary>
    public int? MaxTokens { get; set; }

    /// <summary>
    /// Gets or sets the temperature to use for sampling.
    /// </summary>
    public double? Temperature { get; set; }

    /// <summary>
    /// Gets or sets the top-p value to use for sampling.
    /// </summary>
    public double? TopP { get; set; }

    /// <summary>
    /// Gets or sets the reasoning effort level.
    /// </summary>
    public string? ReasoningEffort { get; set; }

    /// <summary>
    /// Gets or sets the tools available to the model.
    /// </summary>
    public List<DeepSeekTool>? Tools { get; set; }

    /// <inheritdoc/>
    public override PromptExecutionSettings Clone()
    {
        var clone = new DeepSeekPromptExecutionSettings
        {
            MaxTokens = this.MaxTokens,
            Temperature = this.Temperature,
            TopP = this.TopP,
            ReasoningEffort = this.ReasoningEffort,
            Tools = this.Tools?.Count > 0 ? new List<DeepSeekTool>(this.Tools) : null
        };

        this.CopyTo(clone);
        return clone;
    }
}
