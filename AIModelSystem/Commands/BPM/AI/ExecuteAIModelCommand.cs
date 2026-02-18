using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AIModelSystem.Commands.BPM.AI
{
    /// <summary>
    /// Request to execute an AI model
    /// </summary>
    public class ExecuteAIModelRequest : IRequest<Dictionary<string, object>>
    {
        /// <summary>
        /// The model identifier to execute
        /// </summary>
        public string ModelId { get; set; }

        /// <summary>
        /// Input data for the model (supports JObject or Dictionary)
        /// </summary>
        public object Input { get; set; }

        /// <summary>
        /// Optional timeout override in milliseconds
        /// </summary>
        public int? TimeoutMs { get; set; }
    }

    /// <summary>
    /// Command handler for executing AI models
    /// Routes requests to appropriate capability handlers based on configuration
    /// </summary>
    public class ExecuteAIModelCommand : IRequestHandler<ExecuteAIModelRequest, Dictionary<string, object>>
    {
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, ICapabilityHandler> _capabilityHandlers;

        /// <summary>
        /// Initializes the command handler with configuration and capability handlers
        /// </summary>
        /// <param name="configuration">Configuration provider</param>
        /// <param name="capabilityHandlers">Collection of capability handlers</param>
        public ExecuteAIModelCommand(
            IConfiguration configuration,
            IEnumerable<ICapabilityHandler> capabilityHandlers)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            // Register all capability handlers
            _capabilityHandlers = new Dictionary<string, ICapabilityHandler>(StringComparer.OrdinalIgnoreCase);
            
            if (capabilityHandlers != null)
            {
                foreach (var handler in capabilityHandlers)
                {
                    var handlerType = handler.GetType();
                    var capabilityAttr = handlerType.GetCustomAttributes(typeof(CapabilityAttribute), false)
                        .FirstOrDefault() as CapabilityAttribute;
                    
                    if (capabilityAttr != null)
                    {
                        _capabilityHandlers[capabilityAttr.CapabilityName] = handler;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the execution request
        /// </summary>
        public async Task<Dictionary<string, object>> Handle(ExecuteAIModelRequest request, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Validate request
                if (string.IsNullOrWhiteSpace(request.ModelId))
                {
                    throw new ArgumentException("ModelId is required", nameof(request.ModelId));
                }

                // Load configuration from appsettings.json (AI:Models:{modelId})
                var config = LoadModelConfiguration(request.ModelId);
                if (config == null)
                {
                    throw new InvalidOperationException($"Model configuration not found for ModelId: {request.ModelId}");
                }

                // Validate capability
                if (string.IsNullOrWhiteSpace(config.Capability))
                {
                    throw new InvalidOperationException($"Capability not specified for model: {request.ModelId}");
                }

                // Find the appropriate capability handler
                if (!_capabilityHandlers.TryGetValue(config.Capability, out var handler))
                {
                    throw new InvalidOperationException($"No handler registered for capability: {config.Capability}");
                }

                // Normalize input (support both JObject and Dictionary formats)
                var normalizedInput = NormalizeInput(request.Input);

                // Determine timeout
                int timeoutMs = request.TimeoutMs ?? config.TimeoutMs;

                // Execute the capability handler
                var result = await handler.ExecuteAsync(config, normalizedInput, timeoutMs, cancellationToken);

                // Set processing time
                stopwatch.Stop();
                result.ProcessingTimeMs = stopwatch.ElapsedMilliseconds;

                // Return standardized response
                return result.ToResponseObject();
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                // Return error response
                return new Dictionary<string, object>
                {
                    ["success"] = false,
                    ["error"] = ex.Message,
                    ["errorType"] = ex.GetType().Name,
                    ["processingTimeMs"] = stopwatch.ElapsedMilliseconds,
                    ["modelId"] = request.ModelId
                };
            }
        }

        /// <summary>
        /// Loads model configuration from appsettings.json
        /// </summary>
        private AIModelConfig LoadModelConfiguration(string modelId)
        {
            var configSection = _configuration.GetSection($"AI:Models:{modelId}");
            
            if (!configSection.Exists())
            {
                return null;
            }

            var config = new AIModelConfig
            {
                ModelId = modelId,
                Capability = configSection["Capability"],
                Provider = configSection["Provider"],
                Model = configSection["Model"],
                Endpoint = configSection["Endpoint"],
                ApiKey = configSection["ApiKey"]
            };

            // Parse timeout if specified
            if (int.TryParse(configSection["TimeoutMs"], out int timeoutMs))
            {
                config.TimeoutMs = timeoutMs;
            }

            // Load additional parameters
            var parametersSection = configSection.GetSection("Parameters");
            if (parametersSection.Exists())
            {
                config.Parameters = new Dictionary<string, object>();
                foreach (var param in parametersSection.GetChildren())
                {
                    config.Parameters[param.Key] = param.Value;
                }
            }

            return config;
        }

        /// <summary>
        /// Normalizes input to support both JObject and Dictionary formats
        /// </summary>
        private object NormalizeInput(object input)
        {
            if (input == null)
            {
                return new Dictionary<string, object>();
            }

            // If it's already a Dictionary, return it
            if (input is Dictionary<string, object> dict)
            {
                return dict;
            }

            // If it's a JObject, convert to Dictionary
            if (input is JObject jobj)
            {
                return jobj.ToObject<Dictionary<string, object>>();
            }

            // For other types, wrap in a dictionary
            return new Dictionary<string, object>
            {
                ["value"] = input
            };
        }
    }

    /// <summary>
    /// Attribute to mark capability handlers with their capability name
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CapabilityAttribute : Attribute
    {
        public string CapabilityName { get; }

        public CapabilityAttribute(string capabilityName)
        {
            CapabilityName = capabilityName ?? throw new ArgumentNullException(nameof(capabilityName));
        }
    }
}
