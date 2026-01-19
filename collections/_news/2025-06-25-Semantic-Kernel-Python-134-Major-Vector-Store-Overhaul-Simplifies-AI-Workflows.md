---
layout: post
title: 'Semantic Kernel Python 1.34: Major Vector Store Overhaul Simplifies AI Workflows'
author: Eduard van Valkenburg
canonical_url: https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-python-gets-a-major-vector-store-upgrade/
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/semantic-kernel/feed/
date: 2025-06-25 00:40:21 +00:00
permalink: /ai/news/Semantic-Kernel-Python-134-Major-Vector-Store-Overhaul-Simplifies-AI-Workflows
tags:
- API
- Automatic Embedding
- Dataclass
- Developer Experience
- Embedding
- Filtering
- Hybrid Search
- Memory Collection
- Migration
- OpenAITextEmbedding
- Python
- Semantic Kernel
- Vector Store
section_names:
- ai
- coding
---
Authored by Eduard van Valkenburg, this detailed news highlights the major enhancements in Semantic Kernel Python’s vector store with version 1.34, focusing on improved developer experience, unified APIs, and streamlined vector data workflows for AI projects.<!--excerpt_end-->

# Semantic Kernel Python Gets a Major Vector Store Upgrade (v1.34)

### By Eduard van Valkenburg

Semantic Kernel Python’s vector store implementation has received a comprehensive overhaul in version 1.34, greatly simplifying and enriching the AI development experience. This update introduces a consolidated API, automatic embedding generation, improved filtering, streamlined data operations, and better connector experience—all aimed at empowering efficient, modern AI workflows.

---

## What Makes This Release Special?

The new vector store architecture consolidates all functionality under `semantic_kernel.data.vector` and introduces three core improvements:

1. **Simplified API:** A single unified field model—`VectorStoreField`—replaces multiple complex field types, making configuration clearer and more maintainable.
2. **Integrated Embeddings:** Embeddings can now be generated automatically wherever needed, eliminating manual steps.
3. **Enhanced Features:** Support for advanced filtering, hybrid search, and consistent, streamlined operations.

---

## Unified Field Model – Simplified Configuration

- **Old Approach:** Multiple field classes (e.g., `VectorStoreRecordKeyField`, `VectorStoreRecordDataField`, `VectorStoreRecordVectorField`) required verbose and error-prone configuration.
- **New Approach:** The versatile `VectorStoreField` class covers all field types, resulting in cleaner code and better IDE support.

```python
# Old Way

from semantic_kernel.data import (
  VectorStoreRecordKeyField, VectorStoreRecordDataField, VectorStoreRecordVectorField
)
fields = [
  VectorStoreRecordKeyField(name="id"),
  VectorStoreRecordDataField(name="text", is_filterable=True, is_full_text_searchable=True),
  VectorStoreRecordVectorField(name="vector", dimensions=1536, distance_function="cosine")
]

# New Way

from semantic_kernel.data.vector import VectorStoreField
from semantic_kernel.connectors.ai.open_ai import OpenAITextEmbedding
embedding_service = OpenAITextEmbedding(ai_model_id="text-embedding-3-small")
fields = [
  VectorStoreField("key", name="id"),
  VectorStoreField("data", name="text", is_indexed=True, is_full_text_indexed=True),
  VectorStoreField("vector", name="vector", dimensions=1536, distance_function="cosine", embedding_generator=embedding_service)
]
```

---

## Integrated Embeddings – Automatic Generation

With the new architecture, embedding generation is defined directly in your field declarations for data classes, leading to:

- Automatic, consistent embedding creation
- Ability to combine multiple fields for rich vector representations

```python
from semantic_kernel.data.vector import VectorStoreField, vectorstoremodel
from semantic_kernel.connectors.ai.open_ai import OpenAITextEmbedding
from typing import Annotated
from dataclasses import dataclass

@vectorstoremodel
@dataclass
class MyRecord:
    content: Annotated[str, VectorStoreField('data', is_indexed=True, is_full_text_indexed=True)]
    title: Annotated[str, VectorStoreField('data', is_indexed=True, is_full_text_indexed=True)]
    id: Annotated[str, VectorStoreField('key')]
    vector: Annotated[
        list[float] | str | None,
        VectorStoreField(
            'vector',
            dimensions=1536,
            distance_function="cosine",
            embedding_generator=OpenAITextEmbedding(ai_model_id="text-embedding-3-small"),
        ),
    ] = None

    def __post_init__(self):
        if self.vector is None:
            # Combine fields for richer embeddings
            self.vector = f"Title: {self.title}, Content: {self.content}"
```

---

## Lambda-Powered Filtering – Type-Safe and Expressive

Filtering functionality has moved away from string-based constructs to type-safe lambda expressions. This greatly improves readability, safety, and IDE support.

```python
# Old (string-based)

from semantic_kernel.data.text_search import SearchFilter
text_filter = SearchFilter()
text_filter.equal_to("category", "AI")
text_filter.equal_to("status", "active")

# New (lambda-based)

results = await collection.search(
    "query text",
    filter=lambda record: record.category == "AI" and record.status == "active"
)

# Complex filtering

results = await collection.search(
    "machine learning concepts",
    filter=lambda record: (
        record.category == "AI"
        and record.score > 0.8
        and "important" in record.tags
        and 0.5 <= record.confidence_score <= 0.9
    )
)
```

---

## Streamlined Operations – Consistent Interface

A unified API surface means the same methods are used for both single and batch operations, as well as flexible retrieval and search:

```python
from semantic_kernel.connectors.in_memory import InMemoryCollection

collection = InMemoryCollection(
    record_type=MyRecord,
    embedding_generator=OpenAITextEmbedding(ai_model_id="text-embedding-3-small")
)

# Upsert records

await collection.upsert(single_record)
await collection.upsert([record1, record2, record3])

# Retrieval

await collection.get(["id1", "id2"])
await collection.get(top=10, skip=0, order_by='title')

# Search

results = await collection.search("find AI articles", top=10)
results = await collection.hybrid_search("machine learning", top=10)
```

---

## Instant Search Functions – Simplified Creation

Creating and registering search functions in the kernel can now be accomplished directly on the collection object:

```python
# Old

from semantic_kernel.data import VectorStoreTextSearch
collection = InMemoryCollection(collection_name='collection', record_type=MyRecord)
search = VectorStoreTextSearch.from_vectorized_search(
    vectorized_search=collection,
    embedding_generator=OpenAITextEmbedding(ai_model_id="text-embedding-3-small")
)
search_function = search.create_search(function_name='search')

# New

search_function = collection.create_search_function(
    function_name="search",
    search_type="vector", # or "keyword_hybrid"
    top=10,
    vector_property_name="vector"
)
kernel.add_function(plugin_name="memory", function=search_function)
```

---

## Enhanced Data Model Expressiveness

Data models are more capable than before, supporting:

- Rich metadata fields
- Multiple vectors for different embedding strategies

```python
@vectorstoremodel(collection_name="documents")
@dataclass
class DocumentRecord:
    id: Annotated[str, VectorStoreField('key')]
    title: Annotated[str, VectorStoreField('data', is_indexed=True, is_full_text_indexed=True)]
    content: Annotated[str, VectorStoreField('data', is_full_text_indexed=True)]
    category: Annotated[str, VectorStoreField('data', is_indexed=True)]
    tags: Annotated[list[str], VectorStoreField('data', is_indexed=True)]
    created_date: Annotated[datetime, VectorStoreField('data', is_indexed=True)]
    confidence_score: Annotated[float, VectorStoreField('data', is_indexed=True)]

    # Multiple vectors for different purposes
    content_vector: Annotated[
        list[float] | str | None,
        VectorStoreField(
            'vector',
            dimensions=1536,
            storage_name="content_embedding",
            embedding_generator=OpenAITextEmbedding(ai_model_id="text-embedding-3-small")
        )
    ] = None
    title_vector: Annotated[
        list[float] | str | None,
        VectorStoreField(
            'vector',
            dimensions=1536,
            storage_name="title_embedding",
            embedding_generator=OpenAITextEmbedding(ai_model_id="text-embedding-3-small")
        )
    ] = None
    def __post_init__(self):
        if self.content_vector is None:
            self.content_vector = self.content
        if self.title_vector is None:
            self.title_vector = self.title
```

---

## Better Connector Experience

Connectors and imports are logically reorganized for clarity and convenience. Connector stores include Azure AI Search, Chroma, Pinecone, and Qdrant. Lazy loading is supported for easy imports:

```python
from semantic_kernel.connectors.azure_ai_search import AzureAISearchStore
from semantic_kernel.connectors.chroma import ChromaVectorStore
from semantic_kernel.connectors.pinecone import PineconeVectorStore
from semantic_kernel.connectors.qdrant import QdrantVectorStore

# Or lazy-load all

from semantic_kernel.connectors.memory import (
    AzureAISearchStore,
    ChromaVectorStore,
    PineconeVectorStore,
    QdrantVectorStore
)
```

---

## Real-World Example: Complete Implementation

A clear, end-to-end example using the new design pattern:

```python
from semantic_kernel.data.vector import VectorStoreField, vectorstoremodel
from semantic_kernel.connectors.in_memory import InMemoryCollection
from semantic_kernel.connectors.ai.open_ai import OpenAITextEmbedding
from typing import Annotated
from dataclasses import dataclass

@vectorstoremodel(collection_name="knowledge_base")
@dataclass
class KnowledgeBase:
    id: Annotated[str, VectorStoreField('key')]
    content: Annotated[str, VectorStoreField('data', is_full_text_indexed=True)]
    category: Annotated[str, VectorStoreField('data', is_indexed=True)]
    vector: Annotated[
        list[float] | str | None,
        VectorStoreField(
            'vector',
            dimensions=1536,
            embedding_generator=OpenAITextEmbedding(ai_model_id="text-embedding-3-small")
        )
    ] = None
    def __post_init__(self):
        if self.vector is None:
            self.vector = self.content

# Usage with automatic embedding

docs = [
    KnowledgeBase(id="1", content="Semantic Kernel is awesome", category="general"),
    KnowledgeBase(id="2", content="Python makes AI development easy", category="programming"),
]
async with InMemoryCollection(record_type=KnowledgeBase) as collection:
    await collection.ensure_collection_exists()
    await collection.upsert(docs)
    results = await collection.search(
        "AI development", top=5, filter=lambda doc: doc.category == "programming"
    )
    search_func = collection.create_search_function("knowledge_search", search_type="vector")
    kernel.add_function(plugin_name="kb", function=search_func)
```

---

## What This Means for Your Projects

- **Faster Development:** Eliminate boilerplate and focus on AI logic
- **Better Maintainability:** Concise, understandable, modifiable code
- **Enhanced Performance:** Built-in optimizations and batch processing
- **Future-Proof:** Aligned with .NET SDK for consistent, cross-platform development
- **Richer Functionality:** Hybrid search, advanced filtering, integrated embeddings

---

## Migration and Deprecations

- Deprecated: `MemoryStore` abstractions, implementations, `Semantic Text Memory`, and the `TextMemoryPlugin`
- Deprecated connectors now moved to `semantic_kernel.connectors.memory_stores`, with full removal planned for August
- Migration guide: [Learn more](https://learn.microsoft.com/en-us/semantic-kernel/support/migration/vectorstore-python-june-2025)

---

## Conclusion

Semantic Kernel Python 1.34 marks a substantial advancement for AI application development. The revamped vector store system unifies the experience, delivers efficient and expressive APIs, enhances maintainability, and prepares developers for future growth. Developers are encouraged to upgrade, consult the migration guide, and experiment with the new, streamlined workflows.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-python-gets-a-major-vector-store-upgrade/)
