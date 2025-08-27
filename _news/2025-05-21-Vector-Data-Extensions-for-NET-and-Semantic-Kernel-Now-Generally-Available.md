---
layout: "post"
title: "Vector Data Extensions for .NET and Semantic Kernel Now Generally Available"
description: "Microsoft has released Microsoft.Extensions.VectorData.Abstractions, a foundational library for managing vector data in AI-powered applications. Developed jointly by the .NET and Semantic Kernel teams, this GA release provides robust abstractions for vector store interoperability, extensibility, and consistency across AI workloads in the Microsoft ecosystem."
author: "Wes Steyn, Shay Rojansky"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/semantic-kernel/vector-data-extensions-are-now-generally-available-ga/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2025-05-21 14:41:49 +00:00
permalink: "/2025-05-21-Vector-Data-Extensions-for-NET-and-Semantic-Kernel-Now-Generally-Available.html"
categories: ["AI", "Coding"]
tags: [".NET", "Abstractions", "AI", "AI Powered Applications", "Application Development", "Azure AI Search", "Coding", "Embedding Generation", "GA Release", "Interoperability", "Library Development", "Microsoft.Extensions.AI.Abstractions", "Microsoft.Extensions.VectorData.Abstractions", "News", "Semantic Kernel", "Vector Database", "Vector Store Connectors"]
tags_normalized: ["dotnet", "abstractions", "ai", "ai powered applications", "application development", "azure ai search", "coding", "embedding generation", "ga release", "interoperability", "library development", "microsoftdotextensionsdotaidotabstractions", "microsoftdotextensionsdotvectordatadotabstractions", "news", "semantic kernel", "vector database", "vector store connectors"]
---

Written by Wes Steyn and Shay Rojansky, this article introduces the GA release of the Microsoft.Extensions.VectorData.Abstractions library. It focuses on shared abstractions for vector data, integration with AI tooling, and practical usage with Semantic Kernel and .NET.<!--excerpt_end-->

# Vector Data Extensions are now Generally Available (GA)

**Authors:** Wes Steyn, Shay Rojansky

---

## Introduction

Microsoft announces the general availability (GA) of **Microsoft.Extensions.VectorData.Abstractions**, a foundational library designed to standardize exchange types and integrations for vector stores in AI-powered applications. This library results from collaborative work between the Semantic Kernel and .NET teams, aiming to provide a robust, extensible, and consistent development experience.

## What is Microsoft.Extensions.VectorData.Abstractions?

The **Microsoft.Extensions.VectorData.Abstractions** library offers shared abstractions and utilities to facilitate scalable, maintainable, and interoperable AI solutions leveraging vector data. Now officially GA, it forms the backbone for integrating various vector databases and stores into AI workflows.

### Key Features

- **Interoperability:** Libraries integrate seamlessly by targeting common abstractions.
- **Extensibility:** Developers can build on shared types for new capabilities.
- **Consistency:** Unified APIs simplify development and reduce integration challenges.

## Integration with Microsoft.Extensions.AI.Abstractions

The component is designed to work hand-in-hand with **Microsoft.Extensions.AI.Abstractions** for embedding generation. Types such as [`IEmbeddingGenerator`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai.iembeddinggenerator) and [`Embedding`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai.embedding-1) are fully compatible, and the Microsoft.Extensions.AI packages are themselves now GA.

The **Semantic Kernel** framework and **Microsoft.Extensions.AI** provide implementation modules to connect with embedding generation services. For more information, see the [documentation on Semantic Kernel and Microsoft.Extensions.AI integration](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-extensions-ai-better-together-part-1/).

## Why Target These Abstractions?

- **Library Authors:** Remaining agnostic regarding specific AI or vector database providers avoids vendor lock-in and improves library compatibility across the ecosystem.
- **Application Developers:** Freedom to choose concrete implementations while benefiting from a unified, stable API.
- **Providers:** Can implement the abstractions to ensure smooth integration and ecosystem compatibility.

### Practical Benefits

- Libraries and connectors can interoperate smoothly.
- Developers retain flexibility in switching or combining providers without major code changes.

## Available Implementations

Several vector store connectors implementing the new abstractions are available, making it easy to connect AI applications to popular databases such as:

- Azure AI Search
- Qdrant
- PostgreSQL
- More options listed in the [Semantic Kernel vector store connectors documentation](https://learn.microsoft.com/semantic-kernel/concepts/vector-store-connectors/out-of-the-box-connectors)

## Usage Example

### Step 1: Add Required Packages

```bash
dotnet add package Azure.Identity
dotnet add package Azure.AI.OpenAI --prerelease
dotnet add package Microsoft.Extensions.AI.OpenAI --prerelease
dotnet add package Microsoft.SemanticKernel.Connectors.SqliteVec --prerelease
```

### Step 2: Implement Embedding and Vector Store Usage

```csharp
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.SqliteVec;

// Create embedding generator
IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
    new AzureOpenAIClient(new Uri(Settings.EmbeddingEndpoint), new DefaultAzureCredential())
    .GetEmbeddingClient("text-embedding-3-large")
    .AsIEmbeddingGenerator(1536);

// Create vector store collection
VectorStoreCollection<int, Product> collection = new SqliteCollection<int, Product>(
    connectionString: "Data Source=products.db",
    name: "products",
    new SqliteCollectionOptions {
        // Pass embedding generator
        EmbeddingGenerator = embeddingGenerator,
    });

// Ensure the collection exists
await collection.EnsureCollectionExistsAsync();

// Upsert a record
await collection.UpsertAsync(new Product {
    Id = 1,
    Name = "Kettle",
    TenantId = 5,
    Description = "This kettle is great for making tea, it heats up quickly and has a large capacity."
});

// Conduct a vector search
var query = "Find me kettles that can hold a lot of water";
await foreach (var result in collection.SearchAsync(query, top: 5, new() { Filter = r => r.TenantId == 5 }))
{ Console.WriteLine(result.Record.Description); }

class Product {
    [VectorStoreKey]
    public int Id { get; set; }
    [VectorStoreData]
    public string Name { get; set; }
    [VectorStoreData]
    public int TenantId { get; set; }
    [VectorStoreData]
    public string Description { get; set; }
    // Automatically convert Description to a vector on upsert
    [VectorStoreVector(Dimensions: 1536)]
    public string? Embedding => this.Description;
}
```

## Current Status and Roadmap

- The core **abstractions** are now **generally available**.
- **Semantic Kernel** implementations are currently in **preview** and will reach GA in the coming weeks. Some connectors depend on drivers or SDKs still being finalized, delaying their GA launch.
- Breaking changes to implementations may still occur; the focus is on quality and stability for the future.

## Get Started

1. Review the [documentation](https://learn.microsoft.com/semantic-kernel/concepts/vector-store-connectors) for guidance and examples.
2. Explore the [Semantic Kernel GitHub repository](https://github.com/microsoft/semantic-kernel) for connectors and sample projects.
3. See the [.NET blog post](https://aka.ms/dotnet/ai/extensions/ga) for more details about the releases.

---

Stay tuned for updates as more connectors become generally available. Microsoft encourages developers to use these new abstractions for building maintainable and production-grade AI solutions with the Semantic Kernel and .NET.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/vector-data-extensions-are-now-generally-available-ga/)
