using System.Collections.Generic;

namespace AIModelSystem.Commands.BPM.AI
{
    /// <summary>
    /// Configuration for an AI model
    /// </summary>
    public class AIModelConfig
    {
        /// <summary>
        /// Unique identifier for the model
        /// </summary>
        public string ModelId { get; set; }

        /// <summary>
        /// The capability type (e.g., "text-generation", "image-classification")
        /// </summary>
        public string Capability { get; set; }

        /// <summary>
        /// The provider name (e.g., "OpenAI", "Anthropic", "Hugging Face")
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// The specific model name/version at the provider
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// API endpoint URL
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// API key for authentication
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Additional provider-specific parameters
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Default timeout in milliseconds
        /// </summary>
        public int TimeoutMs { get; set; } = 30000;
    }
}
