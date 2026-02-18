using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AIModelSystem.Commands.BPM.AI.Handlers
{
    /// <summary>
    /// Handler for image classification capabilities
    /// </summary>
    [Capability("image-classification")]
    public class ImageClassificationHandler : ICapabilityHandler
    {
        public async Task<AIModelResult> ExecuteAsync(AIModelConfig config, object input, int timeoutMs, CancellationToken cancellationToken)
        {
            var inputDict = input as Dictionary<string, object>;
            var imageUrl = inputDict?.ContainsKey("imageUrl") == true ? inputDict["imageUrl"]?.ToString() : null;
            var imageData = inputDict?.ContainsKey("imageData") == true ? inputDict["imageData"]?.ToString() : null;

            if (string.IsNullOrWhiteSpace(imageUrl) && string.IsNullOrWhiteSpace(imageData))
            {
                throw new ArgumentException("Either imageUrl or imageData is required for image classification");
            }

            // Simulate API call
            await Task.Delay(150, cancellationToken);

            var result = new AIModelResult
            {
                ModelId = config.ModelId,
                Capability = config.Capability,
                Provider = config.Provider,
                ModelVersion = config.Model,
                Data = new Dictionary<string, object>
                {
                    ["labels"] = new[]
                    {
                        new { label = "cat", confidence = 0.92 },
                        new { label = "animal", confidence = 0.89 },
                        new { label = "pet", confidence = 0.85 }
                    },
                    ["topLabel"] = "cat"
                },
                Confidence = 0.92,
                Metadata = new Dictionary<string, object>
                {
                    ["imageSource"] = imageUrl ?? "base64Data"
                }
            };

            return result;
        }
    }
}
