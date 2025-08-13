---
layout: "post"
title: "Transitioning to Microsoft.Extensions.AI’s IEmbeddingGenerator in Semantic Kernel"
description: "This article by Roger Barreto details Semantic Kernel’s migration from its experimental embeddings interfaces to the standardized Microsoft.Extensions.AI IEmbeddingGenerator interface. It covers the motivations, key differences, migration guidance, connector support, and benefits for .NET developers integrating AI services."
author: "Roger Barreto"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/semantic-kernel/transitioning-to-new-iembeddinggenerator-interface/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2025-05-21 14:46:46 +00:00
permalink: "/2025-05-21-Transitioning-to-MicrosoftExtensionsAIs-IEmbeddingGenerator-in-Semantic-Kernel.html"
categories: ["AI", "Coding"]
tags: [".NET", "AI", "AI Services", "Azure OpenAI", "C#", "Coding", "Connector Support", "Ecosystem", "Embeddings", "IEmbeddingGenerator", "Microsoft", "Microsoft Semantic Kernel", "Microsoft.Extensions.AI", "Migration Guide", "News", "OpenAI", "RAG", "Samples", "Semantic Kernel", "Type Safety", "Vector Database"]
tags_normalized: ["net", "ai", "ai services", "azure openai", "c", "coding", "connector support", "ecosystem", "embeddings", "iembeddinggenerator", "microsoft", "microsoft semantic kernel", "microsoft dot extensions dot ai", "migration guide", "news", "openai", "rag", "samples", "semantic kernel", "type safety", "vector database"]
---

Roger Barreto examines the transition of Semantic Kernel to the Microsoft.Extensions.AI IEmbeddingGenerator interface, highlighting migration steps, key benefits, and standardized AI service integration in the .NET ecosystem.<!--excerpt_end-->

# Transitioning to Microsoft.Extensions.AI’s IEmbeddingGenerator in Semantic Kernel

**Author:** Roger Barreto

---

Semantic Kernel is evolving its foundational abstractions in line with the broader .NET ecosystem by adopting Microsoft.Extensions.AI, a step that signals the end-of-life for its experimental embeddings interfaces. This article explains the shift to the new standardized `IEmbeddingGenerator` interface, its benefits, and provides practical migration guidance for developers.

## The Evolution of Embedding Generation in Semantic Kernel

Semantic Kernel’s goal has always been to provide a unified way to work with AI services such as embedding generation. The original approach utilized the `ITextEmbeddingGenerationService` interface, serving well during the project's experimental phase. However, as AI development tools matured, Semantic Kernel moved towards more robust abstractions.

With Microsoft.Extensions.AI now generally available, Semantic Kernel will standardize on the `IEmbeddingGenerator<string, Embedding<float>>` interface, offering improved consistency, type safety, and integration capability for developers working across the .NET AI service landscape.

## Why Make the Change?

Transitioning to the new interface brings significant advantages:

1. **Standardization**: Aligns with Microsoft.Extensions conventions and best practices in the .NET ecosystem.
2. **Type Safety**: Enhanced through generic return types such as `Embedding<float>`.
3. **Flexibility**: Supports various input types and embedding formats.
4. **Consistency**: Delivers a uniform approach for different AI service providers.
5. **Integration**: Ensures seamless interaction with other Microsoft.Extensions libraries.

## Code Comparison: Before and After

### Before: Using ITextEmbeddingGenerationService

```csharp
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;

// Create the kernel builder
var builder = Kernel.CreateBuilder();

// Add OpenAI embedding service
builder.Services.AddOpenAITextEmbeddingGeneration(
  modelId: "text-embedding-ada-002",
  apiKey: "your-api-key");

// Build the kernel
var kernel = builder.Build();

// Get the embedding service
var embeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

// Generate embeddings
var text = "Semantic Kernel is a lightweight SDK that integrates Large Language Models (LLMs) with conventional programming languages.";
var embedding = await embeddingService.GenerateEmbeddingAsync(text);

// Work with the embedding vector
Console.WriteLine($"Generated embedding with {embedding.Length} dimensions");
```

### After: Using IEmbeddingGenerator

```csharp
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;

// Create the kernel builder
var builder = Kernel.CreateBuilder();

// Add OpenAI embedding generator
builder.Services.AddOpenAIEmbeddingGenerator(
  modelId: "text-embedding-ada-002",
  apiKey: "your-api-key");

// Build the kernel
var kernel = builder.Build();

// Get the embedding generator
var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

// Generate embeddings
var text = "Semantic Kernel is a lightweight SDK that integrates Large Language Models (LLMs) with conventional programming languages.";
var embedding = await embeddingGenerator.GenerateAsync(text);

// Work with the embedding vector
Console.WriteLine($"Generated embedding with {embedding.Vector.Length} dimensions");
```

## Key Differences

- **Method Names**: `GenerateEmbeddingsAsync` is now `GenerateAsync`.
- **Return Type**: The new approach returns `GeneratedEmbeddings<Embedding<float>>` instead of `IList<ReadOnlyMemory<float>>`.
- **Options**: The new interface accepts optional `EmbeddingGenerationOptions` for greater control.
- **Dimensions**: Dimensions are configured through options rather than attributes.

## Migrating Your Code

To migrate from `ITextEmbeddingGenerationService` to `IEmbeddingGenerator<string, Embedding<float>>`, follow these steps:

1. Update your Semantic Kernel package dependencies to the latest version.
2. Replace references to `ITextEmbeddingGenerationService` with the new interface.
3. Adjust service registration to use new embedding generator classes (e.g., `OpenAIEmbeddingGenerator`).
4. Update method calls from `GenerateEmbeddingsAsync` to `GenerateAsync`.
5. Access embedding vectors through the `.Vector` property.

## Transitional Support

Semantic Kernel provides extension methods to help convert between the old and new interfaces during your transition:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;

// Create a kernel with both old and new embedding services
var builder = Kernel.CreateBuilder();

// Add old OpenAI embedding service
builder.Services.AddOpenAITextEmbeddingGeneration(
  modelId: "text-embedding-ada-002",
  apiKey: "your-api-key");

// Build the kernel
var kernel = builder.Build();

// Get the old embedding service
var oldEmbeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

// Convert to new interface with extension method
IEmbeddingGenerator<string, Embedding<float>> newGenerator = oldEmbeddingService.AsEmbeddingGenerator();

// Use new generator
var newEmbedding = await newGenerator.GenerateAsync("Converting from old to new");
Console.WriteLine($"Generated embedding with {newEmbedding.Vector.Length} dimensions");
```

## Connector Support

All official connectors have been updated to support the new interface, including:

- OpenAI and Azure OpenAI
- Google AI and Vertex AI
- Amazon Bedrock
- Hugging Face
- MistralAI
- And more

Connectors now provide both legacy (marked obsolete) and new generator implementations.

## Conclusion

Migrating to `Microsoft.Extensions.AI` and the `IEmbeddingGenerator` interface is a key step in the maturation of Semantic Kernel. This transition brings consistency, improved type safety, and easier integration for all developers working with AI in .NET. All users are encouraged to migrate, as the legacy interface is now obsolete and will be removed in a future release.

**Further Reading:**
For more information, visit the [official Microsoft.Extensions.AI announcement](https://devblogs.microsoft.com/semantic-kernel/microsoft-extensions-ai-simplifying-ai-integration-for-net-partners/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/transitioning-to-new-iembeddinggenerator-interface/)
