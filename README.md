# 🎯 AI Model Integration System

A comprehensive, production-ready AI integration system with support for 26+ providers and 32+ capability handlers.

## 📦 Features

- **26+ AI Provider Support**: OpenAI, Anthropic, Meta, Hugging Face, Stability AI, and more
- **32+ Capability Handlers**: Text generation, image classification, sentiment analysis, code generation, and more
- **MediatR Integration**: Clean CQRS pattern with command handlers
- **Configuration-Driven**: Load model configurations from appsettings.json
- **Standardized Responses**: Consistent result format across all capabilities
- **Error Handling**: Graceful error handling with detailed error messages
- **Performance Metrics**: Built-in processing time measurement
- **Flexible Input**: Support for both JObject and Dictionary input formats
- **Timeout Management**: Configurable timeouts per model or request

## 🏗️ Architecture

### Core Components

#### 1. **ExecuteAIModelCommand** (`Commands/BPM/AI/ExecuteAIModelCommand.cs`)
Main MediatR command handler that:
- Routes requests to capability handlers based on capability type
- Loads configuration from appsettings.json (`AI:Models:{modelId}`)
- Measures processing time
- Returns standardized responses
- Handles errors gracefully
- Registers all capability handlers in constructor
- Supports both JObject and Dictionary input formats

#### 2. **ICapabilityHandler** (`Commands/BPM/AI/ICapabilityHandler.cs`)
Interface for implementing AI capability handlers:
```csharp
public interface ICapabilityHandler
{
    Task<AIModelResult> ExecuteAsync(AIModelConfig config, object input, int timeoutMs, CancellationToken cancellationToken);
}
```

#### 3. **AIModelResult** (`Commands/BPM/AI/AIModelResult.cs`)
Result model with properties:
- `ModelId`: Unique identifier of the AI model
- `Capability`: The capability type executed
- `Provider`: The provider name
- `Data`: Output data from the AI model
- `Confidence`: Confidence score (0-1) if applicable
- `ModelVersion`: The version of the model used
- `ProcessingTimeMs`: Time taken to process
- `Explanation`: Optional explanation of the result
- `Metadata`: Additional metadata
- `ToResponseObject()`: Method that returns Dictionary<string, object>

#### 4. **AIModelConfig** (`Commands/BPM/AI/AIModelConfig.cs`)
Configuration model for AI models with provider-specific parameters

## 🎨 Supported Capabilities

### Text & NLP (13 handlers)
- `text-generation` - Generate text using LLMs (GPT, Claude, LLaMA)
- `text-classification` - Classify text into categories
- `sentiment-analysis` - Analyze sentiment (positive/negative/neutral)
- `named-entity-recognition` - Extract entities (people, places, organizations)
- `text-summarization` - Summarize long text
- `question-answering` - Answer questions based on context
- `translation` - Translate between languages
- `embeddings` - Generate text embeddings
- `semantic-search` - Semantic similarity search
- `zero-shot-classification` - Classify without training data
- `paraphrase-generation` - Generate alternative phrasings
- `grammar-correction` - Fix grammar errors
- `few-shot-learning` - Learn from few examples

### Vision (8 handlers)
- `image-classification` - Classify images
- `image-generation` - Generate images from text (DALL-E, Stable Diffusion)
- `image-segmentation` - Segment objects in images
- `object-detection` - Detect and locate objects
- `face-detection` - Detect faces and landmarks
- `pose-estimation` - Estimate human poses
- `optical-character-recognition` - Extract text from images
- `video-classification` - Classify video content

### Audio (4 handlers)
- `speech-to-text` - Transcribe audio to text
- `text-to-speech` - Generate speech from text
- `audio-classification` - Classify audio content
- `music-generation` - Generate music

### Code (4 handlers)
- `code-generation` - Generate code from descriptions
- `code-completion` - Complete code snippets
- `code-review` - Review code for issues
- `bug-detection` - Detect potential bugs

### Machine Learning (4 handlers)
- `recommendation` - Generate recommendations
- `anomaly-detection` - Detect anomalies in data
- `forecasting` - Time series forecasting
- `clustering` - Cluster data points
- `regression` - Regression predictions

### Content Moderation (2 handlers)
- `moderation` - Content moderation (OpenAI)
- `toxicity-detection` - Detect toxic content

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- NuGet packages (automatically restored):
  - MediatR 12.4.0
  - Microsoft.Extensions.Configuration.Abstractions 9.0.0
  - Newtonsoft.Json 13.0.3

### Building the Project
```bash
dotnet build AIModelSystem/AIModelSystem.csproj
```

### Configuration

Add your AI model configurations to `appsettings.json`:

```json
{
  "AI": {
    "Models": {
      "gpt-4": {
        "Capability": "text-generation",
        "Provider": "OpenAI",
        "Model": "gpt-4-turbo-preview",
        "Endpoint": "https://api.openai.com/v1/chat/completions",
        "ApiKey": "your-openai-api-key",
        "TimeoutMs": 30000,
        "Parameters": {
          "temperature": 0.7,
          "max_tokens": 2000
        }
      }
    }
  }
}
```

### Usage Example

```csharp
// Create a request
var request = new ExecuteAIModelRequest
{
    ModelId = "gpt-4",
    Input = new Dictionary<string, object>
    {
        ["prompt"] = "Write a short story about AI"
    },
    TimeoutMs = 30000 // Optional timeout override
};

// Execute via MediatR
var result = await mediator.Send(request);

// Access the response
var text = result["data"]["text"];
var processingTime = result["processingTimeMs"];
```

## 📋 Capability Handler Implementation

To add a new capability handler:

1. Create a new class implementing `ICapabilityHandler`
2. Add the `[Capability("capability-name")]` attribute
3. Implement the `ExecuteAsync` method

```csharp
[Capability("my-capability")]
public class MyCapabilityHandler : ICapabilityHandler
{
    public async Task<AIModelResult> ExecuteAsync(
        AIModelConfig config, 
        object input, 
        int timeoutMs, 
        CancellationToken cancellationToken)
    {
        // Your implementation here
        
        return new AIModelResult
        {
            ModelId = config.ModelId,
            Capability = config.Capability,
            Provider = config.Provider,
            Data = new Dictionary<string, object>
            {
                ["result"] = "your result"
            },
            Confidence = 0.95
        };
    }
}
```

## 🔧 Response Format

All responses follow a standardized format:

### Success Response
```json
{
  "modelId": "gpt-4",
  "capability": "text-generation",
  "provider": "OpenAI",
  "data": {
    "text": "Generated text...",
    "completionTokens": 150,
    "promptTokens": 20
  },
  "confidence": 0.95,
  "modelVersion": "gpt-4-turbo-preview",
  "processingTimeMs": 1234,
  "metadata": {
    "model": "gpt-4-turbo-preview",
    "temperature": 0.7
  }
}
```

### Error Response
```json
{
  "success": false,
  "error": "Error message",
  "errorType": "InvalidOperationException",
  "processingTimeMs": 123,
  "modelId": "model-id"
}
```

## 📊 Supported Providers

- **OpenAI**: GPT-4, GPT-3.5, DALL-E, Whisper, Moderation
- **Anthropic**: Claude 3 (Opus, Sonnet, Haiku)
- **Meta**: LLaMA 2, NLLB Translation, MusicGen
- **Hugging Face**: 100+ models (BERT, RoBERTa, ResNet, etc.)
- **Stability AI**: Stable Diffusion
- **Google**: FLAN-T5
- **Microsoft**: ResNet, TrOCR
- **Ultralytics**: YOLOv8
- **Intel**: DPT
- **Facebook**: DETR, Wav2Vec2
- **Custom**: Your own API endpoints

## 🛡️ Error Handling

The system includes comprehensive error handling:
- Configuration validation
- Input validation
- Timeout management
- Provider-specific error handling
- Graceful degradation
- Detailed error messages

## 📈 Performance

- Built-in processing time measurement
- Configurable timeouts per model
- Async/await throughout
- Cancellation token support
- Efficient handler registration

## 🔒 Security

- API keys stored in configuration (use secure configuration providers in production)
- Input validation
- Timeout protection
- Provider-agnostic error messages

## 📝 Project Structure

```
AIModelSystem/
├── Commands/
│   └── BPM/
│       └── AI/
│           ├── ExecuteAIModelCommand.cs    # Main command handler
│           ├── ICapabilityHandler.cs       # Handler interface
│           ├── AIModelResult.cs            # Result model
│           ├── AIModelConfig.cs            # Configuration model
│           └── Handlers/                   # Capability handlers
│               ├── TextGenerationHandler.cs
│               ├── ImageClassificationHandler.cs
│               ├── TextAnalysisHandlers.cs
│               ├── MultiModalHandlers.cs
│               ├── VisionHandlers.cs
│               ├── CodeHandlers.cs
│               ├── MLHandlers.cs
│               └── AdvancedNLPHandlers.cs
├── appsettings.json                        # Configuration file
└── AIModelSystem.csproj                    # Project file
```

## 🤝 Contributing

To add a new provider or capability:
1. Implement `ICapabilityHandler`
2. Add configuration to appsettings.json
3. Register in DI container (if using one)
4. Add documentation

## 📄 License

This project is open source and available under the MIT License.

## 🙏 Acknowledgments

Built with:
- MediatR for CQRS pattern
- Microsoft.Extensions.Configuration for configuration management
- Newtonsoft.Json for JSON handling

## 📞 Support

For questions or issues, please open an issue on GitHub.