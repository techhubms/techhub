---
layout: "post"
title: "Configuring Microsoft.Extensions.AI with Multiple Providers: OpenAI, Azure OpenAI, and Ollama"
description: "Rick Strahl provides a thorough guide to configuring the Microsoft.Extensions.AI library with multiple AI providers, including OpenAI, Azure OpenAI, and Ollama. The post covers library setup, provider-specific client instantiation, and a practical example of streaming completions in a .NET desktop application."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/May/30/Configuring-MicrosoftAIExtension-with-multiple-providers"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2025-05-31 08:56:33 +00:00
permalink: "/2025-05-31-Configuring-MicrosoftExtensionsAI-with-Multiple-Providers-OpenAI-Azure-OpenAI-and-Ollama.html"
categories: ["AI", "Coding", "Azure"]
tags: [".NET", "AI", "Azure", "Azure OpenAI", "Blogs", "C#", "Chat Clients", "ChatOptions", "Coding", "Dependency Injection", "IChatClient", "Microsoft.Extensions.AI", "NuGet", "Ollama", "OpenAI", "Streaming Completions", "WPF"]
tags_normalized: ["dotnet", "ai", "azure", "azure openai", "blogs", "csharp", "chat clients", "chatoptions", "coding", "dependency injection", "ichatclient", "microsoftdotextensionsdotai", "nuget", "ollama", "openai", "streaming completions", "wpf"]
---

In this comprehensive guide, Rick Strahl explores how to configure Microsoft.Extensions.AI with multiple providers such as OpenAI, Azure OpenAI, and Ollama, detailing the setup process and sharing practical code examples for streaming completions in .NET applications.<!--excerpt_end-->

# Configuring Microsoft.Extensions.AI with Multiple Providers: OpenAI, Azure OpenAI, and Ollama

*By Rick Strahl*

---

## Introduction

In this post, Rick Strahl delves into configuring the `Microsoft.Extensions.AI` library for creating AI clients in an abstracted, provider-agnostic way. He shares his personal journey, the challenges faced, and concrete solutions to instantiate AI chat clients from major providers—OpenAI, Azure OpenAI, and Ollama—culminating with a live streaming completions example in a desktop .NET application.

---

## Why Use Microsoft.Extensions.AI?

Rick originally used a homebrew HTTP-based OpenAI interface for lightweight completions and DALL-E image generation. However, as Microsoft introduced more powerful libraries (`Microsoft.AI.Extensions`), supporting broader functionality and more AI providers, he decided to migrate, particularly to enable streaming completions, and to allow end users to select their own providers and API keys.

He notes:

- The extension abstracts AI operations (chat, completions, images, etc.) under a unified interface.
- Initial provider setup is not trivial and varies by provider.

---

## NuGet Dependencies

To support OpenAI, Azure OpenAI, and Ollama, you'll need several NuGet packages. Example list:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Extensions.AI" Version="9.5.0" />
  <PackageReference Include="Microsoft.Extensions.AI.OpenAI" Version="9.5.0-preview.1.25265.7" />
  <PackageReference Include="Microsoft.Extensions.AI.Ollama" Version="9.5.0-preview.1.25265.7" />
  <PackageReference Include="Microsoft.Extensions.AI.AzureAIInference" Version="9.5.0-preview.1.25265.7" />
  <PackageReference Include="Azure.AI.OpenAI" Version="2.2.0-beta.4" />
  <PackageReference Include="Azure.Identity" Version="1.14.0" />
  <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.5" />
  <PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.5" />
  <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.5" />
  <PackageReference Include="Microsoft.Extensions.Logging" Version="9.0.5" />
</ItemGroup>
```

**Note:** `Azure.Identity` brings many dependencies, even if you're using API keys exclusively.

---

## Instantiating a Chat Client with Providers

The library enables you to set up multiple providers, each using their own unique configuration process. Regardless of provider, the end goal is to obtain an `IChatClient` instance.

### 1. **OpenAI Provider**

```csharp
var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
_chatClient = new OpenAIClient(apiKey)
    .GetChatClient("gpt-4o-mini")
    .AsIChatClient();
```

- Only requires an API key and model name.
- Documentation for syntax and changes is best referenced from NuGet ReadMe files (not always from blog/docs or LLMs).

### 2. **Azure OpenAI Provider**

Azure OpenAI requires the deployment of custom models in your own Azure environment—increasing the number of steps and required libraries.

```csharp
// site example: https://youraisite.openai.azure.com/
var site = Environment.GetEnvironmentVariable("AZURE_OPENAI_SITE");
var apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY");
var deployment = "gpt-4o-mini"; // deployment name
_chatClient = new AzureOpenAIClient(
    new Uri(site),
    new ApiKeyCredential(apiKey)
  )
  .GetChatClient(deployment)
  .AsIChatClient();
```

- Requires base URL, deployment name, and API key.
- Additional Azure-specific NuGet packages are needed.

### 3. **Ollama Provider**

Ollama provides local inference for models, accessible in two ways:

#### a. **Via OpenAI Provider**

Since Ollama uses the OpenAI protocol, you can point the OpenAI provider at the Ollama endpoint. The API key field is required but ignored.

```csharp
_chatClient = new OpenAIClient(
    new ApiKeyCredential("ignored"),
    new OpenAIClientOptions {
      Endpoint = new Uri("http://localhost:11434/v1")
    }
  )
  .GetChatClient("llama3.1")
  .AsIChatClient();
```

#### b. **Explicit Ollama Provider**

```csharp
_chatClient = new OllamaChatClient("http://localhost:11434", "llama3.1");
```

- This is more straightforward compared to other providers.
- Useful if you only need Ollama-specific features.
- For general use, either approach suffices.

#### **Notes on Local Models**

Local inference with Ollama is practical for lightweight workloads or local chat, but Rick finds remote models generally outperform in speed and utility.

### 4. **Other Providers**

There are additional providers, like Onyx (for local file models), which can offer better performance at the cost of higher resource consumption within the application.

---

## Streaming Completions Example

A practical reason for this migration was streaming completions—yielding results as the model outputs them, rather than after the response is complete. Here's a sample for streaming to a WPF application's textbox:

```csharp
private async Task GetAutocompletionSuggestions(string inputText) {
    _currentRequestCts = new CancellationTokenSource();
    try {
        Status.ShowProgress(" Generating suggestions...");
        SuggestionsTextBox.Text = "";
        var messages = new List<ChatMessage> {
            new(ChatRole.System, "You are an AI writing assistant..."),
            new(ChatRole.User, $"Please continue this text: \"{inputText}\"")
        };
        var options = new ChatOptions { MaxOutputTokens = 150, Temperature = 0.7f };
        var suggestionBuilder = new StringBuilder();
        await foreach (var update in _chatClient.GetStreamingResponseAsync(messages, options, _currentRequestCts.Token)) {
            if (_currentRequestCts.Token.IsCancellationRequested) break;
            var content = update.Text;
            suggestionBuilder.Append(content);
            if (!string.IsNullOrEmpty(content)) {
                Dispatcher.Invoke(() => {
                    SuggestionsTextBox.Text = suggestionBuilder.ToString();
                    SuggestionsTextBox.ScrollToEnd();
                });
            }
        }
        Dispatcher.Invoke(() => {
            SuggestionsTextBox.Text = suggestionBuilder.ToString();
            Status.ShowSuccess(" Suggestions complete");
        });
    }
    catch (OperationCanceledException) {
        Dispatcher.Invoke(() => { Status.ShowError(" Cancelled"); });
    }
    catch (Exception ex) {
        Dispatcher.Invoke(() => {
            SuggestionsTextBox.Text = $"Error: {ex.Message}";
            Status.ShowError("Error occurred");
        });
    }
}
```

The async enumerable from the chat client yields updates for real-time consumption, and fits naturally into the UI.

---

## Summary

- `Microsoft.Extensions.AI` aims to provide a stable cross-provider foundation for AI functionality in .NET.
- Provider setup (OpenAI, Azure, Ollama, etc.) is varied in complexity and documentation clarity.
- Streaming completions are straightforward once setup is complete.
- The AI library landscape is evolving rapidly; always consult up-to-date documentation (NuGet, official docs).

---

## Additional Resources

- [Adding minimal OWIN Identity Authentication to an Existing ASP.NET MVC Application](https://weblog.west-wind.com/posts/2015/Apr/29/Adding-minimal-OWIN-Identity-Authentication-to-an-Existing-ASPNET-MVC-Application)
- [Keeping Content Out of the Publish Folder for WebDeploy](https://weblog.west-wind.com/posts/2022/Aug/24/Keeping-Content-Out-of-the-Publish-Folder-for-WebDeploy)
- [Using SQL Server on Windows ARM](https://weblog.west-wind.com/posts/2024/Oct/24/Using-Sql-Server-on-Windows-ARM)
- [Map Physical Paths with an HttpContext.MapPath() Extension Method in ASP.NET](https://weblog.west-wind.com/posts/2023/Aug/15/Map-Physical-Paths-with-an-HttpContextMapPath-Extension-Method-in-ASPNET)

---

*Find this post useful? Consider making a donation to support Rick's content.*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/May/30/Configuring-MicrosoftAIExtension-with-multiple-providers)
