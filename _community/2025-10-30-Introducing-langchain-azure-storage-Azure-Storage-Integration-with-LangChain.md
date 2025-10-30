---
layout: "post"
title: "Introducing langchain-azure-storage: Azure Storage Integration with LangChain"
description: "This post introduces langchain-azure-storage, the official Microsoft package for integrating Azure Storage with LangChain 1.0. It explains how the new AzureBlobStorageLoader simplifies document retrieval for RAG (Retrieval-Augmented Generation) pipelines, details migration from older community loaders, and covers features like secure OAuth authentication and support for custom file parsing in LangChain-based workflows using Python."
author: "kyleknapp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-langchain-azure-storage-azure-storage-integrations/ba-p/4465268"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-30 16:00:00 +00:00
permalink: "/2025-10-30-Introducing-langchain-azure-storage-Azure-Storage-Integration-with-LangChain.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "Authentication", "Azure", "Azure AI Search", "Azure Storage", "AzureBlobStorageLoader", "Blob Storage", "Coding", "Community", "Container Access", "DefaultAzureCredential", "Document Loader", "Langchain", "LLM", "Memory Efficiency", "Microsoft Entra ID", "Migration", "OAuth 2.0", "PyPDFLoader", "Python", "RAG", "UnstructuredLoader", "Vector Store"]
tags_normalized: ["ai", "authentication", "azure", "azure ai search", "azure storage", "azureblobstorageloader", "blob storage", "coding", "community", "container access", "defaultazurecredential", "document loader", "langchain", "llm", "memory efficiency", "microsoft entra id", "migration", "oauth 2dot0", "pypdfloader", "python", "rag", "unstructuredloader", "vector store"]
---

Kyle Knapp presents langchain-azure-storage, detailing how Microsoft’s official Azure Storage integration enhances LangChain RAG pipelines with secure, scalable, and customizable document loading capabilities.<!--excerpt_end-->

# Introducing langchain-azure-storage: Azure Storage Integrations for LangChain

**Author:** Kyle Knapp

Microsoft has released `langchain-azure-storage`, the official package for connecting Azure Storage with [LangChain](https://docs.langchain.com/oss/python/langchain/overview) 1.0. This integration introduces the new [AzureBlobStorageLoader](https://docs.langchain.com/oss/python/integrations/document_loaders/azure_blob_storage), now in public preview, which streamlines and improves document-loading for applications, particularly those using Retrieval-Augmented Generation (RAG) approaches with LLMs.

## Key Features

- **Unified Blob/Container Access:** Load documents by entire container, specific prefix, or individual blob names.
- **Memory Efficiency:** Supports lazy loading, enabling scalable processing even for very large datasets (millions or billions of blobs).
- **Secure Authentication:** Leverages default OAuth 2.0 with [DefaultAzureCredential](https://learn.microsoft.com/en-us/azure/developer/python/sdk/authentication/credential-chains?tabs=dac#defaultazurecredential-overview) and supports custom authentication (Managed Identity, SAS).
- **Pluggable Parsing:** Easily integrate any LangChain-compatible loader—parse file types like PDF, DOCX, and more, using the loader factory interface.

## How it Fits into RAG Workflows

- In a RAG pipeline, documents are often stored on Azure Blob Storage.
- Workflows:
  1. **Collect docs** (PDFs, DOCX, etc.)
  2. **Parse to text and metadata** as LangChain `Document` objects
  3. **Chunk/embed** and store in a vector database (e.g., Azure AI Search)
  4. **Query**: Retrieve relevant documents for context to LLMs
- LangChain loaders make steps 1 and 2 simple and consistent. See [RAG tutorial](https://docs.langchain.com/oss/python/langchain/rag#build-a-rag-agent-with-langchain).

## Using AzureBlobStorageLoader

### Install the Package

```sh
pip install langchain-azure-storage
```

### Load All Blobs From a Container

```python
from langchain_azure_storage.document_loaders import AzureBlobStorageLoader
loader = AzureBlobStorageLoader("https://<your-storage-account>.blob.core.windows.net/", "<your-container-name>")
for doc in loader.lazy_load():
    print(doc.metadata["source"])
    print(doc.page_content)
```

### Load Specific Blobs by Name

```python
from langchain_azure_storage.document_loaders import AzureBlobStorageLoader
loader = AzureBlobStorageLoader(
    "https://<your-storage-account>.blob.core.windows.net/",
    "<your-container-name>",
    ["<blob-name-1>", "<blob-name-2>"]
)
for doc in loader.lazy_load():
    print(doc.metadata["source"])
    print(doc.page_content)
```

### Customize Parsing with Loader Factory

```python
from langchain_azure_storage.document_loaders import AzureBlobStorageLoader
from langchain_community.document_loaders import PyPDFLoader  # install langchain-community and pypdf
loader = AzureBlobStorageLoader(
    "https://<your-storage-account>.blob.core.windows.net/",
    "<your-container-name>",
    prefix="pdfs/",  # only blobs prefixed with 'pdfs/'
    loader_factory=PyPDFLoader
)
for doc in loader.lazy_load():
    print(doc.page_content)
```

- This approach works with any file-path-based loader (e.g., DOCX, Markdown, custom formats).

## Migration Guide: Community Loaders to langchain-azure-storage

If you use the old `AzureBlobStorageContainerLoader` or `AzureBlobStorageFileLoader` (from `langchain-community`), migrate by:

1. Switching to `langchain-azure-storage` as a dependency.
2. Updating imports to `langchain_azure_storage.document_loaders`.
3. Using `AzureBlobStorageLoader` in place of container/file loaders.
4. Passing account URLs instead of connection strings.
5. Adopting Microsoft Entra ID authentication (enable with `az login` or managed identity), moving away from connection strings.

### Code Sample: Before/After

**Before:**

```python
from langchain_community.document_loaders import AzureBlobStorageContainerLoader, AzureBlobStorageFileLoader
container_loader = AzureBlobStorageContainerLoader("DefaultEndpointsProtocol=https;AccountName=...", "container")
file_loader = AzureBlobStorageFileLoader("DefaultEndpointsProtocol=https;AccountName=...", "container", "blob")
```

**After:**

```python
from langchain_azure_storage.document_loaders import AzureBlobStorageLoader
container_loader = AzureBlobStorageLoader("https://<account>.blob.core.windows.net", "container", loader_factory=UnstructuredLoader)
file_loader = AzureBlobStorageLoader("https://<account>.blob.core.windows.net", "container", "blob", loader_factory=UnstructuredLoader)
```

## Resources and Feedback

- [AzureBlobStorageLoader Usage Guide](https://docs.langchain.com/oss/python/integrations/document_loaders/azure_blob_storage)
- [langchain-azure-storage PyPI](https://pypi.org/project/langchain-azure-storage/)
- [GitHub Repository](https://github.com/langchain-ai/langchain-azure)

You can provide feedback or request features by filing an issue on [GitHub](https://github.com/langchain-ai/langchain-azure/issues/new) or emailing the team.

---
*Try out the new loader and help make it better for the entire community!*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-langchain-azure-storage-azure-storage-integrations/ba-p/4465268)
