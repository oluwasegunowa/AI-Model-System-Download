using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AIModelSystem.Commands.BPM.AI.Handlers
{
    /// <summary>
    /// Handler for text generation capabilities (e.g., GPT, Claude, LLaMA)
    /// </summary>
    [Capability("text-generation")]
    public class TextGenerationHandler : ICapabilityHandler
    {
        public async Task<AIModelResult> ExecuteAsync(AIModelConfig config, object input, int timeoutMs, CancellationToken cancellationToken)
        {
            // Extract prompt from input
            var inputDict = input as Dictionary<string, object>;
            var prompt = inputDict?.ContainsKey("prompt") == true ? inputDict["prompt"]?.ToString() : input?.ToString();

            if (string.IsNullOrWhiteSpace(prompt))
            {
                throw new ArgumentException("Prompt is required for text generation");
            }

            // Simulate API call (in production, this would call the actual provider API)
            await Task.Delay(100, cancellationToken); // Simulate network delay

            // Build response
            var result = new AIModelResult
            {
                ModelId = config.ModelId,
                Capability = config.Capability,
                Provider = config.Provider,
                ModelVersion = config.Model,
                Data = new Dictionary<string, object>
                {
                    ["text"] = $"Generated response for: {prompt.Substring(0, Math.Min(50, prompt.Length))}...",
                    ["completionTokens"] = 150,
                    ["promptTokens"] = 20
                },
                Confidence = 0.95,
                Metadata = new Dictionary<string, object>
                {
                    ["model"] = config.Model,
                    ["temperature"] = config.Parameters?.ContainsKey("temperature") == true ? config.Parameters["temperature"] : 0.7
                }
            };

            return result;
        }
    }
}
