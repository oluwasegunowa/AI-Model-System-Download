using System.Threading;
using System.Threading.Tasks;

namespace AIModelSystem.Commands.BPM.AI
{
    /// <summary>
    /// Interface for handling specific AI capability executions
    /// </summary>
    public interface ICapabilityHandler
    {
        /// <summary>
        /// Executes the AI model with the specified configuration and input
        /// </summary>
        /// <param name="config">The AI model configuration</param>
        /// <param name="input">Input data for the model (can be JObject or Dictionary)</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result of the AI model execution</returns>
        Task<AIModelResult> ExecuteAsync(AIModelConfig config, object input, int timeoutMs, CancellationToken cancellationToken);
    }
}
