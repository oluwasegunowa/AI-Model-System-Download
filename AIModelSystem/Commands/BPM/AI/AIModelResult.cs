using System.Collections.Generic;

namespace AIModelSystem.Commands.BPM.AI
{
    /// <summary>
    /// Represents the result of an AI model execution
    /// </summary>
    public class AIModelResult
    {
        /// <summary>
        /// The unique identifier of the AI model
        /// </summary>
        public string ModelId { get; set; }

        /// <summary>
        /// The capability type executed (e.g., "text-generation", "image-classification")
        /// </summary>
        public string Capability { get; set; }

        /// <summary>
        /// The provider name (e.g., "OpenAI", "Anthropic", "Hugging Face")
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// The output data from the AI model
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Confidence score (0-1) if applicable
        /// </summary>
        public double? Confidence { get; set; }

        /// <summary>
        /// The version of the model used
        /// </summary>
        public string ModelVersion { get; set; }

        /// <summary>
        /// Time taken to process the request in milliseconds
        /// </summary>
        public long ProcessingTimeMs { get; set; }

        /// <summary>
        /// Optional explanation of the result
        /// </summary>
        public string Explanation { get; set; }

        /// <summary>
        /// Additional metadata about the execution
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// Converts the result to a dictionary response object
        /// </summary>
        /// <returns>Dictionary containing all result properties</returns>
        public Dictionary<string, object> ToResponseObject()
        {
            var response = new Dictionary<string, object>
            {
                ["modelId"] = ModelId,
                ["capability"] = Capability,
                ["provider"] = Provider,
                ["data"] = Data,
                ["processingTimeMs"] = ProcessingTimeMs
            };

            if (Confidence.HasValue)
            {
                response["confidence"] = Confidence.Value;
            }

            if (!string.IsNullOrEmpty(ModelVersion))
            {
                response["modelVersion"] = ModelVersion;
            }

            if (!string.IsNullOrEmpty(Explanation))
            {
                response["explanation"] = Explanation;
            }

            if (Metadata != null && Metadata.Count > 0)
            {
                response["metadata"] = Metadata;
            }

            return response;
        }
    }
}
