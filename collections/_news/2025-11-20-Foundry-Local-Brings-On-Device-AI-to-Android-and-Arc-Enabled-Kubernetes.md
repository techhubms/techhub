---
layout: "post"
title: "Foundry Local Brings On-Device AI to Android and Arc-Enabled Kubernetes"
description: "This announcement introduces Foundry Local for Android, enabling developers to deploy Microsoft AI models directly on mobile devices and on-premises infrastructure. Key new features include an SDK for easy integration, on-device speech-to-text powered by Whisper, OpenAI-compatible APIs, and seamless support for hybrid and disconnected environments via Azure Arc-enabled Kubernetes. The update underscores performance, privacy, and flexibility, offering insights and code samples for developers looking to leverage on-device AI with Microsoft Foundry Local."
author: "Raji Rajagopalan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/foundry-local-comes-to-android/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2025-11-20 16:30:12 +00:00
permalink: "/2025-11-20-Foundry-Local-Brings-On-Device-AI-to-Android-and-Arc-Enabled-Kubernetes.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "AI Development", "AI Tools", "Android Development", "AnythingLLM", "Azure", "Azure AI Foundry", "Azure Arc", "Coding", "Container Deployment", "Deep Learning Models", "Edge AI", "Foundry Local", "FoundryLocal", "GPT OSS", "Hybrid Cloud", "Kubernetes", "LangChain Integration", "Mistral", "MSIgnite", "News", "On Device AI", "OpenAI API", "Phi", "Qwen", "SDK", "Speech API", "Whisper Speech To Text", "Windows ML"]
tags_normalized: ["ai", "ai development", "ai tools", "android development", "anythingllm", "azure", "azure ai foundry", "azure arc", "coding", "container deployment", "deep learning models", "edge ai", "foundry local", "foundrylocal", "gpt oss", "hybrid cloud", "kubernetes", "langchain integration", "mistral", "msignite", "news", "on device ai", "openai api", "phi", "qwen", "sdk", "speech api", "whisper speech to text", "windows ml"]
---

Raji Rajagopalan introduces Foundry Local's expansion to Android and on-prem with Arc-enabled Kubernetes, covering new SDK capabilities and on-device AI powered by Microsoft’s open-source models.<!--excerpt_end-->

# Foundry Local Brings On-Device AI to Android and Arc-Enabled Kubernetes

Microsoft Foundry Local has extended its AI capabilities to Android devices and Arc-enabled Kubernetes environments. This release enables developers to integrate high-performance, privacy-enhanced AI models directly into their mobile and edge applications, eliminating the need for constant cloud connectivity.

## Key Features

- **On-Device AI for Android:**
  - Deploy optimized open-source AI models locally—no internet or cloud required.
  - Improved privacy, lower latency, and suitability for offline scenarios (e.g., healthcare, finance).
  - Preview partners such as PhonePe are running local AI models for payment platform experiences.

- **Speech-to-Text API (Whisper):**
  - Low-latency speech recognition performed entirely on device.
  - Ensures no audio data leaves the device by default, enhancing privacy.
  - Example use cases include form-filling and note-taking in low-connectivity environments.

#### Code Example: Transcribing Audio Using Whisper Model

```csharp
var model = await catalog.GetModelAsync("whisper-tiny");
await model.DownloadAsync(progress => { Console.Write($"\rDownloading model: {progress:F2}%"); if (progress >= 100f) { Console.WriteLine(); } });
await model.LoadAsync();
var audioClient = await model.GetAudioClientAsync();
var response = audioClient.TranscribeAudioStreamingAsync("Recording.mp3", ct);
await foreach (var chunk in response) { Console.Write(chunk.Text); Console.Out.Flush(); }
Console.WriteLine();
```

See API documentation: [https://aka.ms/foundrylocal-audiodocs](https://aka.ms/foundrylocal-audiodocs)

- **Foundry Local SDK Highlights:**
  - Self-contained packaging; no external executable needed for model serving.
  - Small runtime footprint and simple APIs for model management (download, load, serve).
  - Chat completion and speech transcription APIs compatible with OpenAI protocols.
  - Optional OpenAI-compliant web server, supporting integrations (LangChain, OpenAI SDK, Web UI).
  - Integration with [Windows ML](https://learn.microsoft.com/en-us/windows/ai/new-windows-ml/overview) to optimize hardware selection.

#### Code Example: Running Chat Completions

```csharp
var model = await catalog.GetModelAsync("qwen2.5-0.5b");
await model.DownloadAsync(progress => { Console.Write($"\rDownloading model: {progress:F2}%"); if (progress >= 100f) { Console.WriteLine(); } });
await model.LoadAsync();
var chatClient = await model.GetChatClientAsync();
List<ChatMessage> messages = new() { new ChatMessage { Role = "user", Content = "Why is the sky blue?" } };
var streamingResponse = chatClient.CompleteChatStreamingAsync(messages, ct);
await foreach (var chunk in streamingResponse) { Console.Write(chunk.Choices[0].Message.Content); Console.Out.Flush(); }
Console.WriteLine();
await model.UnloadAsync();
```

SDK docs and samples: [https://aka.ms/foundrylocalSDK](https://aka.ms/foundrylocalSDK)

- **On-Premises & Edge Support via Arc-Enabled Kubernetes:**
  - Foundry Local can be deployed in containers orchestrated by Azure Arc-enabled Kubernetes.
  - Designed for manufacturing, industrial, and sovereign scenarios with limited connectivity.
  - Edge deployments validated in preview with enterprise partners; join preview: [https://aka.ms/FL-K8s-Preview-Signup](https://aka.ms/FL-K8s-Preview-Signup)

## Customer Stories

- **PhonePe:** Integrated Foundry Local to enable secure, on-device AI for millions of mobile users.
- **Dell:** Partners with Microsoft to expand model access and support for AI PCs.
- **AnythingLLM:** Leverages Foundry Local to support running deep learning models on local hardware with high performance and flexibility.

*Video and customer testimonials available from Microsoft Foundry blog.*

## What’s Next

Microsoft continues expanding Foundry Local across platforms and scenarios. The roadmap includes:

- General availability release
- Enhanced multi-modality and tool calling
- Linux support
- Expanded on-prem capabilities
- Ongoing partnership with major enterprise and edge computing platforms

Get started:

- [Foundry Local Official Site](https://aka.ms/foundrylocal)
- [Ignite Edge AI Session](https://ignite.microsoft.com/en-US/sessions/BRK199?source=sessions)
- [Demo on MS Mechanics](https://aka.ms/FL_IGNITE_MSMechanics)

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/foundry-local-comes-to-android/)
