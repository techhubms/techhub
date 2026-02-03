---
external_url: https://devblogs.microsoft.com/dotnet/dotnet-ai-essentials-the-core-building-blocks-explained/
title: '.NET AI Essentials: Unified Building Blocks for Intelligent Apps'
author: Jeremy Likness
primary_section: ai
feed_name: Microsoft .NET Blog
date: 2026-01-28 17:30:00 +00:00
tags:
- .NET
- AI
- AI Integration
- Aspire
- Azure OpenAI
- C#
- ChatClient
- Coding
- Dependency Injection
- Generative AI
- Large Language Models
- Llm
- MCP
- Microsoft.Extensions.AI
- Middleware
- Multi Modal Models
- News
- OllamaSharp
- OpenAI
- OpenTelemetry
- Semantic Kernel
- Structured Output
- Telemetry
- Vector Embeddings
section_names:
- ai
- coding
---
Jeremy Likness explains core AI building blocks in .NET for developers, highlighting how Microsoft.Extensions.AI provides unified LLM access, structured outputs, and practical middleware for robust intelligent app development.<!--excerpt_end-->

# .NET AI Essentials: The Core Building Blocks Explained

*Author: Jeremy Likness*

Artificial Intelligence (AI) is rapidly transforming application development. The .NET team is committed to equipping developers with robust tools, libraries, and patterns to seamlessly integrate AI into .NET apps. This article introduces the core building blocks for bringing generative AI to .NET, focusing on unified APIs, extensibility, and practical code examples.

## Key Components

- **Microsoft.Extensions.AI**: Provides a unified API layer for interacting with various large language model (LLM) providers, such as OpenAI, Azure OpenAI, and OllamaSharp.
- **Microsoft.Extensions.VectorData**: Enables semantic search and persistent vector embeddings.
- **Microsoft Agent Framework**: Supports agentic workflows for intelligent applications.
- **Model Context Protocol (MCP)**: Standardizes interoperability across models and tools.

This article, the first of a four-part series, covers foundational concepts and practical usage of Microsoft.Extensions.AI.

## Unified API for Multiple Providers

Instead of working with multiple SDKs, .NET developers can use a single abstraction with Microsoft.Extensions.AI for LLMs ("MEAI"). It standardizes feature access and integrates well-known patterns like dependency injection and middleware familiar from ASP.NET, minimal APIs, and Blazor.

Example: Using **OllamaSharp** to send a prompt:

```csharp
var uri = new Uri("http://localhost:11434");
var ollama = new OllamaApiClient(uri) { SelectedModel = "mistral:latest" };
await foreach (var stream in ollama.GenerateAsync("How are you today?")) {
    Console.Write(stream.Response);
}
```

Example: Using **OpenAI** with a custom client:

```csharp
OpenAIResponseClient client = new("o3-mini", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
OpenAIResponse response = await client.CreateResponseAsync(
    [ResponseItem.CreateUserMessageItem("How are you today?")]
);
foreach (ResponseItem outputItem in response.OutputItems) {
    if (outputItem is MessageResponseItem message) {
        Console.WriteLine($"{message.Content.FirstOrDefault()?.Text}");
    }
}
```

The universal API enables you to use the same request logic, regardless of provider. Adapters exist for those not implementing the core interface:

```csharp
IChatClient client = new OpenAIClient(key).GetChatClient("o3-mini").AsIChatClient();
```

Now, consistent calls can be made for any supported provider:

```csharp
await foreach (ChatResponseUpdate update in client.GetStreamingResponseAsync("How are you today?")) {
    Console.Write(update);
}
```

## Value Beyond Convenience

Microsoft.Extensions.AI handles provider delegation, retries, token limits, integrates with DI, and supports middleware.

### Structured Output Made Simple

With structured output support, developers can define schemas for responses, allowing more reliable parsing and validation. Example with OpenAI SDK:

```csharp
class Family {
    public List<Person> Parents { get; set; }
    public List<Person>? Children { get; set; }
}
class Person {
    public string Name { get; set; }
    public int Age { get; set; }
}
ChatCompletionOptions options = new() {
    ResponseFormat = StructuredOutputsExtensions.CreateJsonSchemaFormat<Family>("family", jsonSchemaIsStrict: true),
    MaxOutputTokenCount = 4096,
    Temperature = 0.1f,
    TopP = 0.1f
};
List<ChatMessage> messages = [
    new SystemChatMessage("You are an AI assistant that creates families."),
    new UserChatMessage("Create a family with 2 parents and 2 children.")
];
ParsedChatCompletion<Family?> completion = chatClient.CompleteChat(messages, options);
Family? family = completion.Parsed;
```

The extensions provide typed methods for simplified and robust handling:

```csharp
var family = await client.GetResponseAsync<Family>([
    new ChatMessage(ChatRole.System, "You are an AI assistant that creates families."),
    new ChatMessage(ChatRole.User, "Create a family with 2 parents and 2 children.")
]);
```

### Standardized Requests/Responses

Model parameters (like **temperature**, max tokens) can be set through a common interface (`ChatOptions`). Responses include usage details such as token count for resource tracking.

### Middleware Support

With middleware, developers can intercept and modify AI prompts and responses for:

- Filtering or blocking malicious content
- Throttling or controlling requests
- Integrating telemetry and tracing

Example: Adding logging and OpenTelemetry support to any chat client:

```csharp
public IChatClient BuildEnhancedChatClient(IChatClient innerClient, ILoggerFactory? loggerFactory = null){
    var builder = new ChatClientBuilder(innerClient);
    if (loggerFactory is not null) {
        builder.UseLogging(loggerFactory);
    }
    var sensitiveData = false; // Enable for debugging
    builder.UseOpenTelemetry(options => options.EnableSensitiveData = sensitiveData);
    return builder.Build();
}
```

Telemetry can be sent to cloud services like Application Insights, or monitored with Aspire's dashboard, which visualizes LLM interactions.

## DataContent and Multi-Modal Models

The AI ecosystem is moving beyond text. With the `DataContent` type, you can include non-text media (e.g., images) in prompts and receive detailed analysis or tags.

Example: Passing a photo to a model for analysis:

```csharp
var instructions = "You are a photo analyst able to extract the utmost detail ...";
var prompt = new TextContent("What's this photo all about? ...");
var image = new DataContent(File.ReadAllBytes(@"c:\photo.jpg"), "image/jpeg");
var messages = new List<ChatMessage> {
    new(ChatRole.System, instructions),
    new(ChatRole.User, [prompt, image])
};
record ImageAnalysis(string Description, string[] tags);
var analysis = await chatClient.GetResponseAsync<ImageAnalysis>(messages);
```

## Other Highlights

Other features include cancellation tokens, error handling, resilience, vector/embedding management, and image generation support.

### Learn More

- [MEAI repo](https://github.com/dotnet/extensions/blob/main/src/Libraries/Microsoft.Extensions.AI.Abstractions/)
- [MEAI samples](https://github.com/dotnet/ai-samples/tree/main/src/microsoft-extensions-ai)
- [Microsoft Extensions for AI docs](https://learn.microsoft.com/dotnet/ai/microsoft-extensions-ai)
- [Build an AI chat app tutorial](https://learn.microsoft.com/dotnet/ai/quickstarts/build-chat-app?pivots=openai)
- [Create intelligent .NET apps with AI templates](https://learn.microsoft.com/dotnet/ai/quickstarts/ai-templates?tabs=visual-studio%2Cconfigure-visual-studio%2Cconfigure-visual-studio-aspire&pivots=azure-openai)
- [AI building blocks video](https://youtu.be/qcp6ufe_XYo)
- [Building intelligent apps with .NET video](https://youtu.be/N0DzWMkEnzk)

---

This post establishes the foundation for building robust, modern .NET applications empowered by AI. Stay tuned for deep dives on vector extensions, agents, and interoperability.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-ai-essentials-the-core-building-blocks-explained/)
