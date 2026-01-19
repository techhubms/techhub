---
layout: post
title: Introducing Data Ingestion Building Blocks in .NET for AI Applications
author: Luis Quintanilla, Adam Sitnik
canonical_url: https://devblogs.microsoft.com/dotnet/introducing-data-ingestion-building-blocks-preview/
viewing_mode: external
feed_name: Microsoft .NET Blog
feed_url: https://devblogs.microsoft.com/dotnet/feed/
date: 2025-12-03 18:05:00 +00:00
permalink: /ai/news/Introducing-Data-Ingestion-Building-Blocks-in-NET-for-AI-Applications
tags:
- .NET
- AI Applications
- Aspire
- CosmosDB
- Data
- Data Ingestion
- Dataingestion
- Document Chunking
- ElasticSearch
- Embedding Generation
- ETL
- MarkDownReader
- Microsoft.Extensions.DataIngestion
- Microsoft.ML.Tokenizers
- MongoDB
- Observability
- OpenAI
- OpenTelemetry
- Qdrant
- Rag
- Semantic Kernel
- SummaryEnricher
- Vector Databases
section_names:
- ai
- coding
- ml
---
Luis Quintanilla and Adam Sitnik announce a preview of modular .NET building blocks for data ingestion, detailing how developers can build scalable, flexible AI pipelines for intelligent applications.<!--excerpt_end-->

# Introducing Data Ingestion Building Blocks in .NET for AI Applications

**Authors:** Luis Quintanilla, Adam Sitnik

The .NET team has announced a preview release of open and modular data ingestion building blocks designed to empower developers in the .NET ecosystem to create scalable AI and ML data pipelines, especially for Retrieval-Augmented Generation (RAG) scenarios.

## Why Context Engineering Matters for AI

AI models have a knowledge cutoff and do not access personal or organizational data by default. To enable high-quality answers in AI apps, developers need robust context engineering: providing AI models with relevant, processed data at the right time.

### Data Ingestion Challenges for .NET Developers

- Efficiently ingest, transform, and retrieve data for AI and context-aware experiences
- Move and manipulate data in ETL workflows (Extract, Transform, Load) for quality, structure, and usefulness
- Make data usable for AI/ML: chunking, enriching, embedding, chunk management

## Preview: Data Ingestion Building Blocks for .NET

The new data ingestion library provides:

- **Unified document representation**: Support for diverse file types (PDF, Word, images, etc.) with a consistent structure for large language models
- **Flexible ingestion**: Readers for both cloud and local data sources
- **AI-powered enrichment**: Automatic enhancements (summaries, sentiment analysis, keyword extraction, classification)
- **Customizable chunking**: Token-based, section-based, semantic-aware chunkers
- **Persistent storage**: Vector databases and document stores with embedding support
- **Composable pipelines**: Chain together readers, processors, chunkers, and writers via the `IngestionPipeline` API
- **Scalability**: Components built for large-scale, enterprise-grade data processing and integration
- **Extensibility**: Designed for plugin capability, letting developers add logic, connectors, and integrations as their needs evolve

### Key Library Dependencies

- **Microsoft.ML.Tokenizers**: Tokenizer support for document chunking
- **Microsoft.Extensions.AI**: AI enrichments like summarization, sentiment, embedding
- **Microsoft.Extensions.VectorData**: Unified interface for vector store backends (Qdrant, SQL Server, CosmosDB, MongoDB, ElasticSearch, SQLite, etc.)

## Building a Data Ingestion Pipeline: Step-by-Step Example

### 1. Set Up a Project & Install Packages

```console
ni DataIngestion.cs # Powershell

touch DataIngestion.cs # Bash

# Add key packages

Microsoft.Extensions.DataIngestion@10.0.1-preview.1.25571.5
Microsoft.Extensions.DataIngestion.Markdig@10.0.1-preview.1.25571.5
Microsoft.Extensions.AI.OpenAI@10.0.1-preview.1.25571.5
Microsoft.Extensions.Logging.Console@10.0.0
Microsoft.ML.Tokenizers.Data.Cl100kBase@2.0.0
Microsoft.SemanticKernel.Connectors.SqliteVec@1.67.1-preview
```

### 2. Document Reading

```csharp
IngestionDocumentReader reader = new MarkdownReader();
```

### 3. Document Processing/Enrichment

Use AI-based enrichment (e.g., add alt text for images):

```csharp
ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddSimpleConsole());
OpenAIClient openAIClient = new(...);
IChatClient chatClient = openAIClient.GetChatClient("gpt-4.1").AsIChatClient();
IngestionDocumentProcessor imageAlternativeTextEnricher = new ImageAlternativeTextEnricher(new EnricherOptions(chatClient) { LoggerFactory = loggerFactory });
```

### 4. Chunking Documents

```csharp
IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = openAIClient.GetEmbeddingClient("text-embedding-3-small").AsIEmbeddingGenerator();
IngestionChunker<string> chunker = new SemanticSimilarityChunker(embeddingGenerator, new IngestionChunkerOptions(tokenizer) { MaxTokensPerChunk = 2000 });
```

### 5. Chunk Processing/Enrichment

Generate summaries for each chunk:

```csharp
IngestionChunkProcessor<string> summaryEnricher = new SummaryEnricher(enricherOptions);
```

### 6. Store Processed Chunks

```csharp
SqliteVectorStore vectorStore = new(...);
VectorStoreWriter<string> writer = new(vectorStore, dimensionCount: 1536);
```

### 7. Compose Pipeline & Run Processing

```csharp
IngestionPipeline<string> pipeline = new(reader, chunker, writer, loggerFactory: loggerFactory)
{
  DocumentProcessors = { imageAlternativeTextEnricher },
  ChunkProcessors = { summaryEnricher }
};
await foreach (var result in pipeline.ProcessAsync(new DirectoryInfo("."), searchPattern: "*.md")) {
  Console.WriteLine($"Completed processing '{result.DocumentId}'. Succeeded: '{result.Succeeded}'.");
}
```

### 8. Vector Search on Stored Chunks

```csharp
var collection = writer.VectorStoreCollection;
while (true) {
  Console.Write("Enter your question (or 'exit' to quit): ");
  string? searchValue = Console.ReadLine();
  if (string.IsNullOrEmpty(searchValue) || searchValue == "exit") break;
  Console.WriteLine("Searching...\n");
  await foreach (var result in collection.SearchAsync(searchValue, top: 3)) {
    Console.WriteLine($"Score: {result.Score}\n\tContent: {result.Record[\"content\"]}");
  }
}
```

## End-to-End Sample: AI Chat Web Template

Try the official AI Web Chat Template for a guided experience integrating MarkItDown for parsing, Qdrant for chunk storage, and distributed orchestration with .NET Aspire. Observability is handled via OpenTelemetry tracing.

## Observability with Aspire

Enable distributed tracing for data ingestion workflows:

```csharp
builder.Services.AddOpenTelemetry()
  .WithTracing(tracing => {
    tracing.AddSource("Experimental.Microsoft.Extensions.AI");
    tracing.AddSource("Experimental.Microsoft.Extensions.DataIngestion");
  });
```

## Get Started

- Install the AI Web Chat Template: [Quickstart](https://learn.microsoft.com/dotnet/ai/quickstarts/ai-templates)
- Sample code: [DataIngestion repo](https://github.com/luisquintanilla/DataIngestion)
- Extend abstractions: [NuGet Data Ingestion Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.DataIngestion.Abstractions/)

Developers and ecosystem authors can extend, integrate, and compose these building blocks for custom data scenarios in AI and ML.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/introducing-data-ingestion-building-blocks-preview/)
