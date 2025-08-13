---
layout: "post"
title: "Exploring the .NET AI Chat Web App Template: Setup, Features, and Architecture"
description: "Andrew Lock explores the new .NET AI Chat Web App template (preview), guiding readers through installation, LLM provider setup (including GitHub Models), vector embedding options, and the architecture using Aspire and Blazor. The post details how the template ingests PDFs, manages embeddings, and sets up a chat flow powered by AI."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/exploring-the-new-ai-chat-template/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-05-06 09:00:00 +00:00
permalink: "/2025-05-06-Exploring-the-NET-AI-Chat-Web-App-Template-Setup-Features-and-Architecture.html"
categories: ["AI", "Coding"]
tags: [".NET", ".NET Core", "AI", "AI Chat Web App", "ASP.NET Core", "Aspire", "Azure OpenAI Service", "Blazor", "C#", "Chat Applications", "Coding", "Embeddings", "Entity Framework Core", "GitHub Models", "LLM", "Microsoft.Extensions.AI", "PDF Ingestion", "Posts", "Vector Store"]
tags_normalized: ["net", "net core", "ai", "ai chat web app", "asp dot net core", "aspire", "azure openai service", "blazor", "c", "chat applications", "coding", "embeddings", "entity framework core", "github models", "llm", "microsoft dot extensions dot ai", "pdf ingestion", "posts", "vector store"]
---

In this blog post, Andrew Lock introduces the new .NET AI Chat Web App template, discussing its setup, integration with GitHub Models and Azure OpenAI, and the technical components that enable document-powered chat applications.<!--excerpt_end-->

# Exploring the .NET AI Chat Web App Template

**Author:** Andrew Lock  

In this post, Andrew Lock delves into the capabilities and architecture of the new **.NET AI Chat Web App template** (currently in preview), demonstrating how to create a conversational app powered by large language models (LLMs). The article also previews adjustments to the template for ingesting website content in an upcoming post.

---

## Getting Started with the .NET AI Chat Web App Template

The .NET AI Chat Web App provides a starting point for developing chat applications that utilize LLMs for conversation and question answering. While chatbots are just one method for introducing AI into applications, the template streamlines getting started with this popular approach.

### Template Installation

Install the template via the .NET CLI:

```bash
dotnet new install Microsoft.Extensions.AI.Templates
```

Once installed, you can create a new AI chat project using:

```bash
dotnet new aichatweb \
  --output ModernDotNetShowChat \
  --provider githubmodels \
  --vector-store local \
  --aspire true
```

This setup creates a solution with the following projects:

- **Web project**: Contains the Blazor-based chat application.
- **Service Defaults project**: For Aspire application best practices.
- **App Host project**: Wires dependencies and runs the application.

A solution file (*.sln*) and configuration README are included.

---

## Template Configuration: Providers and Vector Stores

The template’s flexibility is one of its strengths. You can select:

- **LLM Provider:**
  - *GitHub Models*: Free, easy for developers (demonstrated in this post).
  - *OpenAI*: Uses the OpenAI API.
  - *Azure OpenAI*: Employs Azure’s OpenAI Service.
  - *Ollama*: Runs models locally.

- **Vector Embedding Store:**
  - *Local*: JSON file—suitable for prototyping.
  - *Azure AI Search*: Automates ingestion and search via Azure.
  - *Qdrant*: Local vector DB (dockerized).

#### Concept Summary

- **LLM Provider**: Supplies the language model for chat.
- **Vector Embedding**: Converts ingested data (PDFs, etc.) into vectors for similarity search and grounding model responses.

The post proceeds using GitHub Models as the LLM provider with local vector store storage for a fast, developer-friendly setup.

---

## Using GitHub Models as an LLM Provider

GitHub Models provides a convenient way to experiment with LLMs, requiring only a GitHub account. It offers access to various state-of-the-art models *without* signing up for third-party AI services, but is governed by strict usage and content limits, intended for prototyping.

#### Setup Process

1. Go to [github.com/marketplace/models](https://github.com/marketplace/models) and select a model.
2. Retrieve a developer key (personal access token) from GitHub’s settings.
3. As of May 15, 2025, this token must have the `model:read` permission.
4. Add the token to your application’s secrets as a connection string in the Aspire AppHost project:

```bash
cd ModernDotNetShowChat.AppHost

dotnet user-secrets set ConnectionStrings:openai "Endpoint=https://models.inference.ai.azure.com;Key=YOUR-API-KEY"
```

GitHub Models functions by leveraging the Azure OpenAI Service in the background.

---

## Running and Exploring the Default App

Run the app via the Aspire AppHost project. The web app then ingests two PDF files (included in the template), making their contents searchable via chat.

The user interface is similar to ChatGPT or GitHub Copilot Chat—ask questions about the ingested documents, and the app provides answers, including citations and file references.

### RAG (Retrieval-Augmented Generation)

The template applies the RAG paradigm: it grounds answers with specific data from given documents by converting text into embeddings and searching for similarities on each query.

---

## Technical Architecture

### Aspire App Host

This component orchestrates the application, connecting:

- The **OpenAI (GitHub Models) connection**
- The **Blazor web app**
- The **SQLite database** for caching text embeddings

**Configuration Sketch (Program.cs):**

```csharp
var builder = DistributedApplication.CreateBuilder(args);
var openai = builder.AddConnectionString("openai");
var ingestionCache = builder.AddSqlite("ingestionCache");
var webApp = builder.AddProject<Projects.ModernDotNetShowChat_Web>("aichatweb-app");
webApp.WithReference(openai);
webApp.WithReference(ingestionCache).WaitFor(ingestionCache);
builder.Build().Run();
```

When running the AppHost, Aspire ensures dependencies like the SQLite database are initialized.

---

### Web Application (Blazor Server)

Main responsibilities:

- **GitHub Models/OpenAI chat client**: Connects using the OpenAI NuGet package and Microsoft.Extensions.AI abstractions. Registers embedding generators for converting text to vectors.
- **Vector store**: Persists embeddings as a JSON file.
- **Ingestion cache**: Uses EF Core to track which documents are ingested.

**Service Registration Excerpt (Program.cs):**

```csharp
// Add the OpenAI chat client
var openai = builder.AddAzureOpenAIClient("openai");
openai.AddChatClient("gpt-4o-mini").UseFunctionInvocation();
openai.AddEmbeddingGenerator("text-embedding-3-small");

// Embeddings storage
var vectorStore = new JsonVectorStore(Path.Combine(AppContext.BaseDirectory, "vector-store"));
builder.Services.AddSingleton<IVectorStore>(vectorStore);
builder.Services.AddScoped<DataIngestor>();
builder.Services.AddSingleton<SemanticSearch>();

// Ingestion tracking
builder.AddSqliteDbContext<IngestionCacheDbContext>("ingestionCache");
```

At app start, the ingestion logic processes PDFs and stores embeddings for chat retrieval.

---

## Data Ingestion and Embedding Generation

The `DataIngestor` manages reading source documents, generating embeddings with the specified generator, and storing results in the vector store and SQLite cache.

**Key Steps:**

1. Retrieve documents already ingested.
2. Identify deleted or modified files, removing/updating embeddings and records as needed.
3. For new or changed documents, generate new embeddings and update storage.

**DataIngestor Class Pseudocode:**

```csharp
public async Task IngestDataAsync(IIngestionSource source) {
   // Create collection in vector store (if not exists)
   // Remove embeddings for deleted files
   // Generate and store embeddings for new/changed files
   // Save changes in SQLite cache
}
```

The `PDFDirectorySource` component specifically reads PDFs, checks for modifications, uses **PdfPig** to extract text, and generates one embedding per paragraph.

---

## Chat Flow and Embedding Search

The core chat logic resides in the `Chat.razor` Blazor component. It configures the LLM with:

- A **system prompt** that restricts the assistant to answers grounded in the provided documents, with required citation tags.
- A **search tool** (function invocation) to find relevant embeddings.

**Simplified Prompt (example):**
> You are an assistant who answers questions about information you retrieve. ... Use the search tool to find relevant information. When you do this, end your reply with citations...

The `SearchAsync()` function is invoked by the LLM to find matching data among local embeddings. This architecture ensures answers are factual and directly attributable to source documents.

---

## Summary

Andrew provides a comprehensive introduction to the .NET AI Chat Web App template—covering installation, configuration, the technical breakdown of its components, and practical use of LLMs such as GitHub Models and Azure OpenAI. The template combines modern .NET stacks (Blazor, Aspire, EF Core) with AI capabilities, making it easy to adapt or extend for custom conversational use cases. The next post in the series will cover adapting the template to ingest data from websites, further expanding its utility.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-the-new-ai-chat-template/)
