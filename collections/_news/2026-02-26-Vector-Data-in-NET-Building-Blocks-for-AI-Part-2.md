---
external_url: https://devblogs.microsoft.com/dotnet/vector-data-in-dotnet-building-blocks-for-ai-part-2/
title: Vector Data in .NET – Building Blocks for AI Part 2
author: Jeremy Likness
primary_section: ai
feed_name: Microsoft .NET Blog
date: 2026-02-26 18:00:00 +00:00
tags:
- .NET
- Abstraction Layer
- AI
- AI Development
- Azure AI Search
- C#
- Cosmos DB
- Data Model
- Document Store
- Embeddings
- LINQ
- Microsoft.Extensions.VectorData
- News
- OpenAI
- PostgreSQL
- Qdrant
- Rag
- Redis
- Retrieval Augmented Generation
- Semantic Search
- SQL Server
- Vector Database
- Vector Search
- VectorStore
section_names:
- ai
- dotnet
---
Jeremy Likness explores unified vector database access in .NET using Microsoft.Extensions.VectorData, showing how to implement semantic search and RAG with embeddings in C# applications.<!--excerpt_end-->

# Vector Data in .NET – Building Blocks for AI Part 2

**Author: Jeremy Likness**

## Introduction

This article continues the series on building blocks for AI in .NET, focusing on the Microsoft.Extensions.VectorData library. It introduces how unified vector database access enables efficient semantic search, embeddings, and Retrieval-Augmented Generation (RAG) workflows across a variety of backend vector stores.

## Why Vector Data Matters

Most AI-powered apps need more than basic keyword or text search; they leverage semantic search by encoding text as embeddings—numerical representations of meaning. Embeddings are stored in specialized databases (vector stores), such as Qdrant, Azure AI Search, Redis, SQL Server, and Cosmos DB.

Traditional search retrieves matching documents by text; embeddings allow for context-aware matching using vector similarity. This pattern is crucial for intelligent applications like document search, chatbots with long-term memory, and RAG.

## Microsoft.Extensions.VectorData Overview

- **Unified Abstractions:** The library provides a platform-agnostic interface for interacting with various vector databases. Developers implement business logic once and switch backend vector stores by swapping the implementation.
- **Provider Examples:** Qdrant, Redis, SQL Server, Azure Cosmos DB, Azure AI Search, PostgreSQL, SQLite, and in-memory storage (useful for development/testing).

## Key Concepts

### Semantic Search Workflow

Typical pipeline for LLM-powered search or RAG:

1. **Convert documents to embeddings** using a model (e.g., OpenAI).
2. **Store embeddings in a vector database** alongside original content.
3. **Convert user queries to embeddings** (same model).
4. **Perform similarity search** via the vector database.
5. **Provide relevant context** to the language model to generate grounded answers.

### Provider-Agnostic Coding

Direct client SDK usage is replaced by universal abstractions. For example, vector storage and searching functions can use the same code regardless of backend:

```csharp
var embeddingGenerator = new OpenAIClient(apiKey)
    .GetEmbeddingClient("text-embedding-3-small")
    .AsIEmbeddingGenerator();

var vectorStore = new QdrantVectorStore(
    new QdrantClient("localhost"),
    ownsClient: true,
    new QdrantVectorStoreOptions { EmbeddingGenerator = embeddingGenerator });

var collection = vectorStore.GetCollection<string, DocumentRecord>("my_collection");
await collection.EnsureCollectionExistsAsync();

var record = new DocumentRecord {
    Key = Guid.NewGuid().ToString(),
    Text = "Sample document text",
    Category = "documentation"
};

await collection.UpsertAsync(record);
var searchResults = collection.SearchAsync("find documents about sample topics", top: 5);
```

Swapping providers only changes the vectorStore implementation, not your app logic.

### Data Model Mapping

The library uses attributes to map C# classes to vector database schemas:

```csharp
public class DocumentRecord {
    [VectorStoreKey]
    public string Key { get; set; }
    [VectorStoreData]
    public string Text { get; set; }
    [VectorStoreData(IsIndexed = true)]
    public string Category { get; set; }
    [VectorStoreData(IsIndexed = true)]
    public DateTimeOffset Timestamp { get; set; }
    [VectorStoreVector(1536, DistanceFunction.CosineSimilarity)]
    public string Embedding => this.Text;
}
```

- **VectorStoreKey:** Uniquely identifies each record
- **VectorStoreData:** Metadata fields
- **VectorStoreVector:** Embedding vector configuration

### Semantic Search and Filtering

- Use `SearchAsync` to trigger embedding generation and query vector similarity.
- Supports passing pre-computed embeddings as well.
- Combine vector similarity with metadata filters using standard LINQ expressions (e.g., date range, category equality).

#### Example: Filtering

```csharp
var searchOptions = new VectorSearchOptions<DocumentRecord> {
    Filter = r => r.Category == "documentation" && r.Timestamp > DateTimeOffset.UtcNow.AddDays(-30)
};
var results = collection.SearchAsync("find relevant documentation", top: 10, searchOptions);
```

### RAG Pattern Implementation in C#

Combining the above with language models (e.g., Microsoft.Extensions.AI):

```csharp
public async Task<string> AskQuestionAsync(string question) {
    var contextParts = new List<string>();
    await foreach (var result in collection.SearchAsync(question, top: 3)) {
        contextParts.Add(result.Record.Text);
    }
    var context = string.Join("\n\n", contextParts);
    var messages = new List<ChatMessage> {
        new(ChatRole.System, "Answer questions based on the provided context. If the context doesn't contain relevant information, say so."),
        new(ChatRole.User, $"Context:\n{context}\n\nQuestion: {question}")
    };
    var response = await chatClient.GetResponseAsync(messages);
    return response.Message.Text;
}
```

## Supported Vector Stores

- Azure AI Search
- Qdrant
- Redis
- PostgreSQL
- Azure Cosmos DB (NoSQL)
- SQL Server
- SQLite
- In-Memory

Full list and setup guidance: [Vector store connectors documentation](https://learn.microsoft.com/semantic-kernel/concepts/vector-store-connectors/out-of-the-box-connectors/?pivots=programming-language-csharp).

## Separation from Core AI Extensions

Vector data support is decoupled from Microsoft.Extensions.AI so that users only add vector functionality when needed, keeping the core library lightweight for scenarios such as chatbots, content generation, or classification.

## Summary

Microsoft.Extensions.VectorData makes vector database integration as portable and developer-friendly as LLM development in .NET. It provides a common interface for semantic search, RAG, and other AI features, letting you focus on your application rather than database intricacies.

## Resources

- [AI samples repository](https://github.com/dotnet/ai-samples)
- [.NET AI documentation](https://learn.microsoft.com/dotnet/ai/)
- [AI building blocks video](https://youtu.be/qcp6ufe_XYo)
- [Building intelligent apps with .NET video](https://youtu.be/N0DzWMkEnzk)

---

*Happy coding!*

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/vector-data-in-dotnet-building-blocks-for-ai-part-2/)
