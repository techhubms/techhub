---
external_url: https://devblogs.microsoft.com/dotnet/explore-text-to-image-dotnet/
title: Exploring Text-to-Image Capabilities in .NET with Microsoft Extensions for AI
author: Jeremy Likness
feed_name: Microsoft .NET Blog
date: 2025-09-24 23:30:00 +00:00
tags:
- .NET
- Abstractions
- Agent Framework
- API
- Azure AI Foundry
- C#
- Copilot Studio
- Developer Tools
- Extensions
- IImageGenerator
- Image Editing
- Image Generation
- LLM
- MEAI
- Microsoft.Extensions.AI
- Multimodal AI
- OpenAI
- SDK
- Text To Image
- AI
- Coding
- News
section_names:
- ai
- coding
primary_section: ai
---
Jeremy Likness introduces developers to text-to-image features in .NET using Microsoft's AI abstractions, covering practical usage examples and best practices for leveraging MEAI's consistent API approach.<!--excerpt_end-->

# Exploring Text-to-Image Capabilities in .NET with Microsoft Extensions for AI

*Author: Jeremy Likness*

## Overview

This article demonstrates how Microsoft's Extensions for AI (MEAI) in .NET provides a developer-friendly, consistent abstraction for working with advanced AI services like text-to-image generation. Jeremy Likness discusses both the motivation for MEAI and hands-on examples relevant for .NET developers looking to implement multimodal AI scenarios.

## Why Text-to-Image?

Text-to-image generation unlocks a variety of use cases, such as:

- **Marketing:** Automatic creation of campaign visuals
- **Education:** Generating diagrams or illustrations from textual descriptions
- **Accessibility:** Turning text into visual content
- **Prototyping:** Quickly visualizing UI concepts or product ideas
- **Fine-tuning:** Editing and personalizing images iteratively

## Abstractions and MEAI Benefits

MEAI provides a unified API to access modalities like text, image, speech, or video across different providers (Azure AI, OpenAI, ONNX Runtime) and adapters. This abstraction enables:

- **Plug-and-Play Modality Support:** Swap providers without rewriting logic
- **Provider-Agnostic Interoperability:** Standard interfaces across platforms
- **Ecosystem Growth:** Easily adopt new modalities and tools
- **Accelerated Innovation:** Focus on building experiences, not plumbing

MEAI is foundational to related efforts in Teams AI, Azure AI Foundry, and Agent Framework. Feedback is actively requested from developers to improve and adapt these abstractions further ([feedback form](https://aka.ms/ai-modalities)).

## Getting Started with Text-to-Image in .NET

The latest preview of [`Microsoft.Extensions.AI`](https://www.nuget.org/packages/Microsoft.Extensions.AI) includes text-to-image functionality and sample applications. The [`Text2ImageSample`](https://github.com/JeremyLikness/Text2ImageSample) demonstrates generating and modifying images using:

- Natural language prompts
- Existing images
- A provider-agnostic API

**Workflow Example:**

1. Query genres using a chat model
2. Randomly select a genre
3. Use the chat model to produce a detailed scene description
4. Pass the description to the image generator
5. Load a user-supplied character image
6. Describe and modify the character image based on the scene
7. Render modified versions as sketches

### Code Snippets

To access the universal image generator:

```csharp
IImageGenerator openAiGenerator = openAi.GetImageClient("my-image-model").AsIImageGenerator();
```

Generate an image from a prompt:

```csharp
DataContent imageBytes = (await imageGenerator.GenerateImagesAsync("Draw me something so amazing it can't be described.")).ContentsOfType<DataContent>().First();
```

Save the result based on content type:

```csharp
private string SaveImage(DataContent content, string name) {
    var extension = content.MediaType.Split(@"/")[1];
    var path = Path.Combine(baseDirectory, $"{name}.{extension}");
    File.WriteAllBytes(path, content.Data.ToArray());
    return Path.GetFileName(path);
}
```

Edit an existing image by providing an image and descriptive prompt:

```csharp
DataContent variation = (await myImageGenerator.EditImageAsync(
    Context.Character!,
    $"The source image should contain a person. Create a new image that transforms the person into a character in a {Context.Genre} novel. They should be doing something interesting/productive in this location: {Context.SceneDescription}",
    cancellationToken: ct)).Contents.OfType<DataContent>().First();
```

## What’s Next for MEAI?

Work is ongoing to expand beyond text-to-image into:

- **Image-to-image:** Style transfer, enhancement
- **Image-to-video:** Animation, synthesis
- **Text-to-speech/Speech-to-text:** Via Azure Speech or other providers

## Feedback and Participation

Microsoft is actively seeking developer input through surveys and collaboration to better adapt abstractions and modalities for .NET AI development.

For details, sample code, and the latest updates, review the linked documentation and repositories.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/explore-text-to-image-dotnet/)
