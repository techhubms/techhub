---
layout: "post"
title: "Semantic Kernel and Microsoft.Extensions.AI: Better Together, Part 2"
description: "This post, authored by Roger Barreto, provides hands-on examples demonstrating how to use the Microsoft.Extensions.AI abstractions with Semantic Kernel in non-agent .NET scenarios, including chat completion, embedding generation, function invocation, and advanced service selection patterns."
author: "Roger Barreto"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-extensions-ai-better-together-part-2/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2025-05-28 13:18:24 +00:00
permalink: "/2025-05-28-Semantic-Kernel-and-MicrosoftExtensionsAI-Better-Together-Part-2.html"
categories: ["AI", "Coding"]
tags: [".NET", ".NET AI Extensions", "Agents", "AI", "AIFunction", "Azure OpenAI", "C#", "Chat Completion", "Coding", "Dependency Injection", "Embeddings", "Extensions AI", "Function Calling", "IChatClient", "IEmbeddingGenerator", "KernelFunction", "MCP", "Microsoft.Extensions.AI", "News", "Plugins", "Prompt Templates", "Samples", "Semantic Kernel", "Tools"]
tags_normalized: ["net", "net ai extensions", "agents", "ai", "aifunction", "azure openai", "c", "chat completion", "coding", "dependency injection", "embeddings", "extensions ai", "function calling", "ichatclient", "iembeddinggenerator", "kernelfunction", "mcp", "microsoft dot extensions dot ai", "news", "plugins", "prompt templates", "samples", "semantic kernel", "tools"]
---

Authored by Roger Barreto, this article explores practical non-agent integration patterns for Microsoft.Extensions.AI and Semantic Kernel within the .NET AI ecosystem. It guides developers through real code samples for chat completion, embeddings, function calls, dependency injection, and service selection.<!--excerpt_end-->

# Semantic Kernel and Microsoft.Extensions.AI: Better Together, Part 2

*By Roger Barreto*

---

This is the second installment of a series focused on integrating [Microsoft.Extensions.AI](https://www.nuget.org/packages/Microsoft.Extensions.AI) with [Semantic Kernel](https://devblogs.microsoft.com/semantic-kernel). After covering complementary relationships in [Part 1](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-extensions-ai-better-together-part-1/), this article provides practical examples to help developers leverage both technologies—especially in non-agent AI scenarios.

## Getting Started with Microsoft.Extensions.AI and Semantic Kernel

Microsoft.Extensions.AI introduces foundational abstractions such as `IChatClient` and `IEmbeddingGenerator<string, Embedding<float>>`. Semantic Kernel builds atop these, enabling features like plugins, prompt templates, and automated workflows. By combining both, you can address a wide range of AI development patterns in .NET.

## 1. Basic Chat Completion with `IChatClient`

Semantic Kernel now natively supports Microsoft.Extensions.AI's `IChatClient` interface. Below are ways to initiate chat completions leveraging the integration:

### Using Kernel Builder

```csharp
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;

// Create a kernel with OpenAI chat client
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatClient("gpt-4o", "your-api-key")
    .Build();

// Simple chat completion
var response = await kernel.InvokePromptAsync("What is the capital of France?");
Console.WriteLine(response);
```

### Using a Chat Client Directly with Azure OpenAI

```csharp
var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatClient(
        deploymentName: "gpt-4o",
        endpoint: "https://your-resource.openai.azure.com/",
        apiKey: "your-api-key")
    .Build();

var client = kernel.GetRequiredService<IChatClient>();
var response = await client.GetResponseAsync([
    new(ChatRole.User, "Hello, AI!")
]);
Console.WriteLine(response.Text);
```

### Using Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;

var services = new ServiceCollection();

// Register the chat client
services.AddOpenAIChatClient("gpt-4o", "your-api-key");

// Register Semantic Kernel
services.AddKernel();

var serviceProvider = services.BuildServiceProvider();
var kernel = serviceProvider.GetRequiredService<Kernel>();

var response = await kernel.InvokePromptAsync("Tell me about artificial intelligence.");
Console.WriteLine(response);
```

### Converting Between `IChatCompletionService` and `IChatClient`

```csharp
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

// Get the chat completion service
var chatService = kernel.GetRequiredService<IChatCompletionService>();

// Convert to IChatClient when needed
IChatClient chatClient = chatService.AsChatClient();

// Or convert back
IChatCompletionService backToService = chatClient.AsChatCompletionService();
```

## 2. Embedding Generation with `IEmbeddingGenerator`

Semantic Kernel has transitioned from its own `ITextEmbeddingGenerationService` to Microsoft.Extensions.AI's generalized `IEmbeddingGenerator<string, Embedding<float>>`.

### Basic Embedding Generation

```csharp
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;

# pragma warning disable SKEXP0010 // Type is for evaluation

var kernel = Kernel.CreateBuilder()
    .AddOpenAIEmbeddingGenerator("text-embedding-ada-002", "your-api-key")
    .Build();

var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

// Generate embeddings
var embeddings = await embeddingGenerator.GenerateAsync([
    "Semantic Kernel is a lightweight, open-source development kit.",
    "Microsoft.Extensions.AI provides foundational AI abstractions."
]);

foreach (var embedding in embeddings)
{
    Console.WriteLine($"Generated embedding with {embedding.Vector.Length} dimensions");
}
```

### Working with Azure OpenAI Embeddings

```csharp
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;

# pragma warning disable SKEXP0010

var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIEmbeddingGenerator(
        deploymentName: "text-embedding-ada-002",
        endpoint: "https://your-resource.openai.azure.com/",
        apiKey: "your-api-key")
    .Build();

var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

// Generate embeddings with custom dimensions (if supported by model)
var embeddings = await embeddingGenerator.GenerateAsync(
    ["Custom text for embedding"],
    new EmbeddingGenerationOptions { Dimensions = 1536 }
);

Console.WriteLine($"Generated {embeddings.Count} embeddings");
```

## 3. Function Calling Integration

Semantic Kernel’s function calling model, built atop Microsoft.Extensions.AI, uses `KernelFunction` (a type of `AIFunction`), resulting in direct integration.

### Creating and Using Kernel Functions

```csharp
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using System.ComponentModel;

var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatClient("gpt-4o", "your-api-key")
    .Build();

// Import the function as a plugin
kernel.ImportPluginFromType<WeatherPlugin>();

// Use function calling
var settings = new PromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

var response = await kernel.InvokePromptAsync(
    "What's the weather like in Seattle and what time is it?",
    new(settings)
);
Console.WriteLine(response);

public class WeatherPlugin
{
    [KernelFunction, Description("Get the current weather for a city")]
    public static string GetWeather([Description("The city name")] string city)
    {
        return $"The weather in {city} is sunny and 72°F";
    }

    [KernelFunction, Description("Get the current time")]
    public static string GetCurrentTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
```

### Working with `KernelFunction` Directly

```csharp
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatClient("gpt-4o", "your-api-key")
    .Build();

// Create a function from a method
var weatherFunction = KernelFunctionFactory.CreateFromMethod(
    () => "Sunny and 75°F", "GetWeather", "Gets the current weather");

// KernelFunction is already an AIFunction, so you can use it directly
var chatOptions = new ChatOptions
{
    Tools = [weatherFunction], // KernelFunction as AITool
    ToolMode = ChatToolMode.Auto
};

var chatClient = kernel.GetRequiredService<IChatClient>();
var messages = new List<ChatMessage> { new(ChatRole.User, "What's the weather like?") };

var response = await chatClient.GetResponseAsync(messages, chatOptions);
Console.WriteLine(response.Text);
```

## 4. Content Type Conversions

Semantic Kernel enables you to directly return Microsoft.Extensions.AI types from prompts.

### Using `InvokeAsync<T>` with Microsoft.Extensions.AI Types

```csharp
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatClient("gpt-4o", "your-api-key")
    .Build();

// Get Microsoft.Extensions.AI ChatResponse directly
var chatResponse = await kernel.InvokeAsync<ChatResponse>(
    kernel.CreateFunctionFromPrompt("Tell me a joke")
);

Console.WriteLine($"Model: {chatResponse.ModelId}");
Console.WriteLine($"Content: {chatResponse.Text}");

// Get List<ChatMessage> for conversation history
var message = await kernel.InvokeAsync<ChatMessage>(
    kernel.CreateFunctionFromPrompt("Start a conversation about AI")
);

Console.WriteLine($"Message Role: {message.Role}");
Console.WriteLine($"Message Content: {message.Text}");

// Get Microsoft.Extensions.AI TextContent directly
var textContent = await kernel.InvokeAsync<Microsoft.Extensions.AI.TextContent>(
    kernel.CreateFunctionFromPrompt("Start a conversation about AI")
);

Console.WriteLine($"Text Content: {textContent.Text}");
```

## 5. Service Selection and Dependency Injection

Semantic Kernel supports advanced dependency injection and service selection patterns, aiding projects with multiple AI providers.

### Multiple Chat Providers

```csharp
var services = new ServiceCollection();

// Register multiple chat clients
services.AddOpenAIChatClient("gpt-4", "openai-key", serviceId: "OpenAI");
services.AddAzureOpenAIChatClient(
    "gpt-4", "https://your-resource.openai.azure.com/", "azure-key", serviceId: "AzureOpenAI");

services.AddKernel();

var serviceProvider = services.BuildServiceProvider();
var kernel = serviceProvider.GetRequiredService<Kernel>();

// Use specific service
var settings = new PromptExecutionSettings { ServiceId = "AzureOpenAI" };
var response = await kernel.InvokePromptAsync<ChatResponse>(
    "Explain machine learning", new(settings)
);

Console.WriteLine("Model: " + response.ModelId);
Console.WriteLine("Content: " + response.Text);
```

## Conclusion

Combining Microsoft.Extensions.AI and Semantic Kernel streamlines the development of .NET AI applications by offering:

- **Flexibility**: Use foundational abstractions for simple scenarios
- **Productivity**: Incorporate plugins, prompt templates, and other advanced Semantic Kernel features
- **Interoperability**: Convert between content types and interfaces with ease
- **Scalability**: Use dependency injection and service selection for complex workflows

These patterns empower developers to implement solutions ranging from chatbots to robust workflow engines. In further parts of this series, more advanced agent-based scenarios will be explored.

## Packages and References

- [Microsoft.Extensions.AI.Abstractions (NuGet)](https://www.nuget.org/packages/Microsoft.Extensions.AI.Abstractions/latest)
- [Microsoft.Extensions.AI (NuGet)](https://www.nuget.org/packages/Microsoft.Extensions.AI/latest)
- [Semantic Kernel Agent Framework (Microsoft Learn)](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/?pivots=programming-language-csharp)
- [Semantic Kernel Process Framework (Microsoft Learn)](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/process/process-framework)
- [Semantic Kernel Samples (GitHub)](https://github.com/microsoft/semantic-kernel/tree/main/dotnet/samples)
- [Vector Data Extensions – Blog Post](https://devblogs.microsoft.com/semantic-kernel/vector-data-extensions-are-now-generally-available-ga)
- [Microsoft.Extensions.AI libraries – .NET | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai)
- [eShop Support with Microsoft.Extensions.AI](https://github.com/dotnet/eShopSupport/tree/main)
- [.NET AI Samples](https://github.com/dotnet/ai-samples)

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-extensions-ai-better-together-part-2/)
