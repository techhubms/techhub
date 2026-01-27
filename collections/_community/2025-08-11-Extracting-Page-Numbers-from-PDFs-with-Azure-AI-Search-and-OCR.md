---
external_url: https://techcommunity.microsoft.com/t5/azure-paas-blog/finding-the-right-page-number-in-pdfs-with-ai-search/ba-p/4440758
title: Extracting Page Numbers from PDFs with Azure AI Search and OCR
author: samsarka
feed_name: Microsoft Tech Community
date: 2025-08-11 09:10:39 +00:00
tags:
- AI Search
- Azure AI Search
- Azure Blob Storage
- Azure Cognitive Search
- Document Cracking
- Index Definition
- Index Projection
- Indexer
- Knowledge Mining
- Managed Identity
- Microsoft Azure
- OCR Skill
- Page Number Extraction
- Parent Child Indexing
- PDF Indexing
- RBAC
- REST API Integration
- Search Pipeline
- Skillset
- Text Merger
section_names:
- ai
- azure
primary_section: ai
---
samsarka describes how to use Azure AI Search and OCR to extract and index PDF page numbers, enabling fast and context-aware knowledge discovery in large document repositories.<!--excerpt_end-->

# Extracting Page Numbers from PDFs with Azure AI Search and OCR

In document-heavy environments—contracts, manuals, or other large PDFs—being able to map search results to precise page numbers helps users navigate directly to relevant content. This tutorial outlines how to set up Azure AI Search (formerly Cognitive Search) to provide page-level granularity for knowledge mining solutions.

## Why Page Numbers Matter in AI Search

- **Contextual navigation**: Users can jump straight to relevant sections in large documents.
- **Precise citations**: Better support for knowledge bases and chatbots, allowing exact references.
- **Increased user trust**: Showing where the answer came from increases confidence in AI responses.

## Prerequisites: Azure Blob Storage & AI Search Setup

1. **Azure Blob Storage**: Prepare a container to store your PDFs.
2. **Permissions**: The AI Search service requires 'Storage Blob Data Reader' access. If you're using Azure RBAC, assign the managed identity accordingly.

    - [How to assign blob permissions](https://learn.microsoft.com/en-us/azure/search/search-blob-indexer-role-based-access)
    - [How to index Azure Blobs](https://learn.microsoft.com/en-us/azure/search/search-howto-indexing-azure-blobs)

## Technical Implementation

### 1. Skillsets: Document Cracking, OCR, and Index Projection

- **Document Cracking** splits a PDF into pages and separates text/image content.
- **OCR Skill** (#Microsoft.Skills.Vision.OcrSkill) processes images per page, extracting machine-readable text. This is crucial for PDFs that mix text and embedded images.
- **Index Projection**: Map each page's content, file name, URL, and page number into a search index. Parent-child indexing enables querying at the page level.

Example skillset configuration snippet:

```json
{
  "name": "pagenumskillset",
  "skills": [
    {
      "@odata.type": "#Microsoft.Skills.Vision.OcrSkill",
      "context": "/document/normalized_images/*",
      "outputs": [
        { "name": "text", "targetName": "text" },
        { "name": "layoutText", "targetName": "layoutText" }
      ]
    }
  ],
  "indexProjections": {
    "selectors": [
      {
        "targetIndexName": "pagenumidx",
        "parentKeyFieldName": "ParentKey",
        "sourceContext": "/document/normalized_images/*",
        "mappings": [
          { "name": "DocText", "source": "/document/normalized_images/*/text" },
          { "name": "DocName", "source": "/document/metadata_storage_name" },
          { "name": "DocURL", "source": "/document/metadata_storage_path" },
          { "name": "PageNum", "source": "/document/normalized_images/*/pageNumber" }
        ]
      }
    ]
  }
}
```

### 2. Index Definition

Define an index schema that includes fields for document text, name, page number, and URLs. The index enables fast lookup and faceted search across documents and their pages.

Example fields:

- **ID** (string, key)
- **DocText** (string, searchable text for the page)
- **DocName** (string, file name)
- **PageNum** (int, page number)
- **DocURL** (string, PDF link)
- **ParentKey** (string, reference for parent-child relationship)

### 3. Indexer Configuration: Using OCR and Image Actions

Configure the indexer to render each PDF page as an image, then pass it to the OCR skill for text extraction. Map outputs according to the index definition.

Example indexer snippet:

```json
{
  "name": "indexer-pagenum",
  "dataSourceName": "azureblob-...-datasource",
  "skillsetName": "pagenumskillset",
  "targetIndexName": "pagenumidx",
  "parameters": {
    "configuration": {
      "dataToExtract": "contentAndMetadata",
      "imageAction": "generateNormalizedImagePerPage"
    }
  }
}
```

### 4. Validation: Explore the Results

Use Azure AI Search's Search Explorer to query your index and confirm that search results include page numbers:

```json
{
  "@search.score": 1,
  "ID": "...",
  "DocText": "...PDF content...",
  "DocName": "docname.pdf",
  "PageNum": 33,
  "DocURL": "https://storageaccount.blob.core.windows.net/containername/docname.pdf",
  "ParentKey": "..."
}
```

## Conclusion

With this setup, search and AI-driven applications can return not just relevant snippets, but exactly where in a PDF the answer resides—supporting granular, reliable, and user-friendly document discovery.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-paas-blog/finding-the-right-page-number-in-pdfs-with-ai-search/ba-p/4440758)
