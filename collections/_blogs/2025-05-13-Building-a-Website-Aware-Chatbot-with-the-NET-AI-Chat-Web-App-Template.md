---
external_url: https://andrewlock.net/using-the-new-ai-template-to-create-a-chatbot-about-a-website/
title: Building a Website-Aware Chatbot with the .NET AI Chat Web App Template
author: Andrew Lock
viewing_mode: external
feed_name: Andrew Lock's Blog
date: 2025-05-13 09:00:00 +00:00
tags:
- .NET
- .NET Core
- AngleSharp
- ASP.NET Core
- Aspire
- Chatbot
- Cite Answers
- IIngestionSource
- LLM
- Microsoft.Extensions.AI.Templates
- OpenAI
- Retrieval Augmented Generation
- Semantic Kernel
- Textify
- Vector Embeddings
- Web Scraping
section_names:
- ai
- coding
---
Andrew Lock guides readers through extending the .NET AI Chat Web App template to build a chatbot that understands website content and answers with citations. The post includes code examples, insights, and performance observations.<!--excerpt_end-->

# Building a Website-Aware Chatbot with the .NET AI Chat Web App Template

**Author:** Andrew Lock

## Introduction

In this post, Andrew Lock walks through the process of leveraging the .NET AI Chat Web App template (currently in preview) to build a chatbot that ingests and understands the content of a website. The chatbot can then answer user questions about the site with links and citations to the original sources, using retrieval-augmented generation (RAG) techniques.

## Background and Motivation

The inspiration for this work came from a conversation at the MVP Summit with Jamie Taylor, who wanted a chatbot interface for his podcast, The Modern .NET Show. Since complete transcripts are available online, this provided an ideal use case for the AI template's capabilities.

## Getting Started with the .NET AI Chat Web App Template

Andrew first covers installing and running the new template:

```bash
dotnet new install Microsoft.Extensions.AI.Templates
dotnet new aichatweb \
  --output ModernDotNetShowChat \
  --provider githubmodels \
  --vector-store local \
  --aspire true
```

The template generates a solution with a README.md that explains setup steps, such as configuring a connection string for GitHub Models (enabling access to LLMs like those from OpenAI).

```bash
cd ModernDotNetShowChat.AppHost
dotnet user-secrets set ConnectionStrings:openai "Endpoint=https://models.inference.ai.azure.com;Key=YOUR-API-KEY"
```

Running the app (via Aspire’s AppHost project) starts both the web app and an ingestion process using default content (two PDFs about watches).

The chat interface allows users to ask questions about ingested documents and provides answers with citations to the original content.

## RAG Approach

The chatbot uses retrieval-augmented generation (RAG), which means the LLM consults an indexed vector store of ingested documents to answer questions—ensuring responses are grounded in external source data, not just the model's internal training.

## Customizing for Website Data

Since the target is The Modern .NET Show website, the main change is to use web pages as the data source rather than PDFs. This is done by customizing the ingestion source interface:

### Implementing `IIngestionSource`

Andrew defines a new class `WebIngestionSource` that implements the template’s `IIngestionSource` interface, providing methods for:

- Identifying new or updated documents (`GetNewOrModifiedDocumentsAsync`)
- Ingesting and processing content (`CreateRecordsForDocumentAsync`)
- Handling deletions (`GetDeletedDocumentsAsync`)

#### Example: Creating the Ingestion Source Class

```csharp
public class WebIngestionSource : IIngestionSource {
    private readonly HttpClient _httpClient;
    public WebIngestionSource(string url) {
        SourceId = $"{nameof(WebIngestionSource)}:{url}";
        _httpClient = new HttpClient { BaseAddress = new Uri(url) };
    }
    public string SourceId { get; }
    // ...
}
```

### Discovering Website Pages

To identify all pages in the site, Andrew parses the site's `sitemap.xml` (if available). This is more efficient than crawling. He demonstrates using `XmlSerializer` to load a list of URLs and their last-modified dates:

```csharp
private async Task<Sitemap> GetSitemap() {
    var serializer = new XmlSerializer(typeof(Sitemap));
    await using var stream = await _httpClient.GetStreamAsync("sitemap.xml");
    var sitemap = serializer.Deserialize(stream) as Sitemap;
    if (sitemap is null) throw new Exception("Unable to read sitemap");
    return sitemap;
}
```

Pages are returned and their modification date compared to previously ingested data, ensuring only new or changed content is re-processed.

### Ingesting and Processing Web Pages

The main ingestion function performs these steps for each discovered page:

1. Download page HTML with `HttpClient`
2. Parse the HTML via AngleSharp
3. Convert HTML to markdown using the Textify library
4. Split text into paragraphs with Semantic Kernel utilities
5. Generate vector-embeddings for each paragraph

This prepares the data for fast, citation-capable retrieval during chat interactions.

#### Example: Paragraph Splitting and Embedding

```csharp
List<(int IndexOnPage, string Text)> paragraphs = TextChunker.SplitPlainTextParagraphs([pageText], maxTokensPerParagraph: 200)
    .Select((text, index) => (index, text))
    .ToList();
var embeddings = await embeddingGenerator.GenerateAsync(paragraphs.Select(c => c.Text));
```

### Handling Deleted Content

The implementation also detects pages that have been removed from the sitemap and schedules them for deletion from the vector store.

## Integrating the New Ingestion Source

To enable site-based ingestion, Andrew updates the web app’s Program.cs to register `WebIngestionSource`:

```csharp
await DataIngestor.IngestDataAsync(app.Services, new WebIngestionSource("https://dotnetcore.show"));
app.Run();
```

## Observations: Performance and Usability

- **Ingestion Performance:** Processing a page (including embeddings) takes about 10 seconds each—large sites with many pages can take a long time.
- **Resilience:** The default ingestion process saves results only after all documents are processed—Andrew modifies the logic to save incrementally every 20 documents, improving reliability.
- **Cache Persistence:** Setting a static file path for the SQLite cache prevents unnecessary re-ingestion each app start.

```csharp
var ingestionCache = builder.AddSqlite(
    "ingestionCache",
    databasePath: @"D:\\repos",
    databaseFileName: "modern_dotnetshow_embeddings.db"
);
```

## Results and Conclusion

The proof-of-concept chatbot now enables users to pose questions about The Modern .NET Show website, with the model responding by citing specific URLs and quotations from the original web pages. Andrew notes room for additional optimizations and tweaks, but the system works as a solid demonstration of ingesting and querying website content in a .NET AI-powered chat application.

You can find the full source code for this demo on [GitHub](https://github.com/andrewlock/ModernDotNetShowChat).

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/using-the-new-ai-template-to-create-a-chatbot-about-a-website/)
