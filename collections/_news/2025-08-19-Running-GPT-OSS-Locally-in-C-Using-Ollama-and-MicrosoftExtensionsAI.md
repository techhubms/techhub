---
external_url: https://devblogs.microsoft.com/dotnet/gpt-oss-csharp-ollama/
title: Running GPT-OSS Locally in C# Using Ollama and Microsoft.Extensions.AI
author: Bruno Capuano
viewing_mode: external
feed_name: Microsoft .NET Blog
date: 2025-08-19 17:05:00 +00:00
tags:
- .NET
- .NET 8
- Agentic Apps
- AI Chatbot
- C#
- Console Application
- Function Calling
- Gpt
- Gpt Oss
- Local LLM
- Microsoft.Extensions.AI
- Offline AI
- Ollama
- OllamaSharp
- OpenAI
- Private AI
- Streaming Response
section_names:
- ai
- coding
---
Bruno Capuano demonstrates how developers can run GPT-OSS locally using C#, Ollama, and Microsoft.Extensions.AI libraries to create fast, private, offline-capable AI features.<!--excerpt_end-->

# Running GPT-OSS Locally in C# Using Ollama and Microsoft.Extensions.AI

Bruno Capuano presents a developer-focused guide for setting up and running OpenAI's GPT-OSS model locally with C#, utilizing the Microsoft.Extensions.AI abstraction layer and Ollama for local inference. This empowers developers to build private, offline-capable AI applications without cloud dependencies.

## Why GPT-OSS Matters

- **Open-weight model**: Powerful, open-source LLM options directly available for developers
- **Local execution**: Models like gpt-oss-20b run comfortably on systems with 16GB RAM—no cloud required
- **Versatility**: Supports coding, math, and tool use scenarios
- **Privacy & cost**: Keeps all data on your machine, reducing privacy risks and cloud costs

## Prerequisites

- PC or Mac with at least 16GB RAM and suitable GPU (Apple Silicon supported)
- .NET 8 SDK (or higher)
- Ollama installed and running
- GPT-OSS:20b model pulled via `ollama pull gpt-oss:20b`

## Unified AI with Microsoft.Extensions.AI

Microsoft.Extensions.AI libraries unify AI access for .NET—whether using Ollama, Azure AI, or OpenAI—so you can switch providers without rewriting your core logic. You'll use these abstractions with OllamaSharp for local GPT-OSS inference.

## Step-by-Step: Build a Local C# Chatbot with GPT-OSS

### 1. Create a New Console App

```bash
 dotnet new console -n OllamaGPTOSS
 cd OllamaGPTOSS
```

### 2. Add NuGet Packages

```bash
 dotnet add package Microsoft.Extensions.AI
 dotnet add package OllamaSharp
```

*Note: `Microsoft.Extensions.AI.Ollama` is deprecated; use `OllamaSharp` instead.*

### 3. Implement Rolling Chat in C#

Replace `Program.cs` with the following (simplified for clarity):

```csharp
using Microsoft.Extensions.AI;
using OllamaSharp;

IChatClient chatClient = new OllamaApiClient(new Uri("http://localhost:11434/"), "gpt-oss:20b");
List<ChatMessage> chatHistory = new();

Console.WriteLine("GPT-OSS Chat - Type 'exit' to quit");
while (true) {
    Console.Write("You: ");
    var userInput = Console.ReadLine();
    if (userInput?.ToLower() == "exit") break;
    if (string.IsNullOrWhiteSpace(userInput)) continue;
    chatHistory.Add(new ChatMessage(ChatRole.User, userInput));
    Console.Write("Assistant: ");
    var assistantResponse = "";
    await foreach (var update in chatClient.GetStreamingResponseAsync(chatHistory)) {
        Console.Write(update.Text);
        assistantResponse += update.Text;
    }
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, assistantResponse));
    Console.WriteLine();
}
```

### 4. Run Your Application

Ensure Ollama is running:  

```bash
 dotnet run
```

Your console app will stream responses from GPT-OSS locally.

[Watch a demo here.](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2025/08/gpt-oss-ollama-demo.webm)

## Beyond Chat: Build Agentic Apps

- Use AIFunction calling—to let your GPT-OSS model call C# methods and APIs for tool use
- Next steps: build document summarizers, code generation assistants, or enrich local RAG patterns
- All data and processing remains private and offline

## Upcoming: Foundry Local Integration

A follow-up article will guide developers through setting up GPT-OSS with Foundry Local for Windows-native GPU acceleration. This will include specific configuration tips and example code mirroring the stream/chat pattern shown here. [See Foundry Local details in the Windows Developer Blog](https://blogs.windows.com/windowsdeveloper/2025/08/05/available-today-gpt-oss-20b-model-on-windows-with-gpu-acceleration-further-pushing-the-boundaries-on-the-edge/).

## Summary and Recommendations

- Set up a local LLM using .NET and Ollama with GPT-OSS-20b
- Utilize Microsoft.Extensions.AI for portable, provider-agnostic AI architecture
- Leverage function calling and agentic patterns—all running privately
- Extend this base to build richer, offline, intelligent .NET applications

*Bruno Capuano encourages you to get started, explore advanced features, and contribute to the growing ecosystem of decentralized AI tools for .NET.*

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/gpt-oss-csharp-ollama/)
