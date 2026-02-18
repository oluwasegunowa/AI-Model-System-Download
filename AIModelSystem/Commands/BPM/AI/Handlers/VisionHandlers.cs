using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AIModelSystem.Commands.BPM.AI.Handlers
{
    [Capability("object-detection")]
    public class ObjectDetectionHandler : ICapabilityHandler
    {
        public async Task<AIModelResult> ExecuteAsync(AIModelConfig config, object input, int timeoutMs, CancellationToken cancellationToken)
        {
            var inputDict = input as Dictionary<string, object>;
            var imageUrl = inputDict?.ContainsKey("imageUrl") == true ? inputDict["imageUrl"]?.ToString() : null;

            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("ImageUrl is required");

            await Task.Delay(180, cancellationToken);

            return new AIModelResult
            {
                ModelId = config.ModelId,
                Capability = config.Capability,
                Provider = config.Provider,
                ModelVersion = config.Model,
                Data = new Dictionary<string, object>
                {
                    ["objects"] = new[]
                    {
                        new { label = "person", confidence = 0.96, boundingBox = new { x = 100, y = 100, width = 200, height = 300 } },
                        new { label = "car", confidence = 0.94, boundingBox = new { x = 400, y = 200, width = 250, height = 150 } }
                    }
                },
                Confidence = 0.95
            };
        }
    }

    [Capability("face-detection")]
    public class FaceDetectionHandler : ICapabilityHandler
    {
        public async Task<AIModelResult> ExecuteAsync(AIModelConfig config, object input, int timeoutMs, CancellationToken cancellationToken)
        {
            var inputDict = input as Dictionary<string, object>;
            var imageUrl = inputDict?.ContainsKey("imageUrl") == true ? inputDict["imageUrl"]?.ToString() : null;

            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("ImageUrl is required");

            await Task.Delay(140, cancellationToken);

            return new AIModelResult
            {
                ModelId = config.ModelId,
                Capability = config.Capability,
                Provider = config.Provider,
                ModelVersion = config.Model,
                Data = new Dictionary<string, object>
                {
                    ["faces"] = new[]
                    {
                        new { 
                            confidence = 0.98, 
                            boundingBox = new { x = 150, y = 80, width = 120, height = 140 },
                            landmarks = new { leftEye = new { x = 180, y = 120 }, rightEye = new { x = 220, y = 120 } }
                        }
                    }
                },
                Confidence = 0.98
            };
        }
    }

    [Capability("pose-estimation")]
    public class PoseEstimationHandler : ICapabilityHandler
    {
        public async Task<AIModelResult> ExecuteAsync(AIModelConfig config, object input, int timeoutMs, CancellationToken cancellationToken)
        {
            var inputDict = input as Dictionary<string, object>;
            var imageUrl = inputDict?.ContainsKey("imageUrl") == true ? inputDict["imageUrl"]?.ToString() : null;

            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("ImageUrl is required");

            await Task.Delay(200, cancellationToken);

            return new AIModelResult
            {
                ModelId = config.ModelId,
                Capability = config.Capability,
                Provider = config.Provider,
                ModelVersion = config.Model,
                Data = new Dictionary<string, object>
                {
                    ["poses"] = new[]
                    {
                        new { 
                            confidence = 0.92,
                            keypoints = new[]
                            {
                                new { name = "nose", x = 250, y = 100, confidence = 0.95 },
                                new { name = "leftShoulder", x = 200, y = 150, confidence = 0.93 }
                            }
                        }
                    }
                },
                Confidence = 0.92
            };
        }
    }

    [Capability("optical-character-recognition")]
    public class OCRHandler : ICapabilityHandler
    {
        public async Task<AIModelResult> ExecuteAsync(AIModelConfig config, object input, int timeoutMs, CancellationToken cancellationToken)
        {
            var inputDict = input as Dictionary<string, object>;
            var imageUrl = inputDict?.ContainsKey("imageUrl") == true ? inputDict["imageUrl"]?.ToString() : null;

            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("ImageUrl is required");

            await Task.Delay(160, cancellationToken);

            return new AIModelResult
            {
                ModelId = config.ModelId,
                Capability = config.Capability,
                Provider = config.Provider,
                ModelVersion = config.Model,
                Data = new Dictionary<string, object>
                {
                    ["text"] = "Extracted text from image",
                    ["lines"] = new[]
                    {
                        new { text = "Line 1", confidence = 0.97, boundingBox = new { x = 10, y = 10, width = 200, height = 30 } },
                        new { text = "Line 2", confidence = 0.95, boundingBox = new { x = 10, y = 45, width = 220, height = 30 } }
                    }
                },
                Confidence = 0.96
            };
        }
    }

    [Capability("video-classification")]
    public class VideoClassificationHandler : ICapabilityHandler
    {
        public async Task<AIModelResult> ExecuteAsync(AIModelConfig config, object input, int timeoutMs, CancellationToken cancellationToken)
        {
            var inputDict = input as Dictionary<string, object>;
            var videoUrl = inputDict?.ContainsKey("videoUrl") == true ? inputDict["videoUrl"]?.ToString() : null;

            if (string.IsNullOrWhiteSpace(videoUrl))
                throw new ArgumentException("VideoUrl is required");

            await Task.Delay(400, cancellationToken);

            return new AIModelResult
            {
                ModelId = config.ModelId,
                Capability = config.Capability,
                Provider = config.Provider,
                ModelVersion = config.Model,
                Data = new Dictionary<string, object>
                {
                    ["category"] = "sports",
                    ["categories"] = new[]
                    {
                        new { name = "sports", confidence = 0.89 },
                        new { name = "outdoor", confidence = 0.76 }
                    }
                },
                Confidence = 0.89
            };
        }
    }
}
