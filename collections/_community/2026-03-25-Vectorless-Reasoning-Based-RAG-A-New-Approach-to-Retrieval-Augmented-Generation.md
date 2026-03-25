---
primary_section: ai
tags:
- AI
- AsyncAzureOpenAI
- Azure
- Azure AI Foundry
- Azure OpenAI Service
- Chat.completions
- Community
- Context Retrieval
- Document Tree Indexing
- Explainability
- Hierarchical Document Navigation
- Hybrid RAG
- LLM
- PageIndex
- PDF Ingestion
- Prompt Engineering
- Python
- RAG
- Reasoning Based Retrieval
- Retrieval Augmented Generation
- Traceability
- Vector Database
- Vectorless RAG
date: 2026-03-25 07:00:00 +00:00
section_names:
- ai
- azure
feed_name: Microsoft Tech Community
author: Rajapriya
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/vectorless-reasoning-based-rag-a-new-approach-to-retrieval/ba-p/4502238
title: 'Vectorless Reasoning-Based RAG: A New Approach to Retrieval-Augmented Generation'
---

Rajapriya explains vectorless reasoning-based RAG, using PageIndex to navigate a document’s natural structure and Azure OpenAI (via Azure AI Foundry) to reason over a page tree to retrieve context and generate answers—without embeddings, chunking, or a vector database.<!--excerpt_end-->

# Vectorless Reasoning-Based RAG: A New Approach to Retrieval-Augmented Generation

## Introduction

Retrieval-Augmented Generation (RAG) is a common architecture for AI applications that combine Large Language Models (LLMs) with external knowledge sources.

Traditional RAG pipelines often use vector embeddings and similarity search to retrieve relevant documents. While effective in many scenarios, this can introduce:

- Document chunking into small segments
- Context being split across chunks
- Extra infrastructure for embedding generation and vector databases

A newer approach, **Vectorless Reasoning-Based RAG**, aims to reduce those issues.

One framework enabling this is **PageIndex**, an open-source document indexing system that:

- Organizes documents into a **hierarchical tree structure**
- Lets an LLM do **reasoning-based retrieval** over that structure

## Vectorless Reasoning-Based RAG

Instead of vectors, this approach relies on structured document navigation:

> User Query -> Document Tree Structure -> LLM Reasoning -> Relevant Nodes Retrieved -> LLM Generates Answer

This mirrors how humans read documents:

1. Look at the table of contents
2. Identify relevant sections
3. Read the relevant content
4. Answer the question

### Core features

- **No Vector Database**: Retrieval is based on document structure + LLM reasoning, not vector similarity search.
- **No Chunking**: Documents aren’t split into artificial chunks; they keep natural structure (pages/sections).
- **Human-like Retrieval**: The system navigates sections and extracts from relevant parts.
- **Better Explainability and Traceability**: You can trace results back to pages/sections, avoiding opaque “vibe retrieval” from approximate vector search.

## When to Use Vectorless RAG

Vectorless RAG works best when:

- Data is structured or semi-structured
- Documents have clear metadata
- Knowledge sources are well organized
- Queries require reasoning more than semantic similarity

Example use cases:

- Enterprise knowledge bases
- Internal documentation systems
- Compliance and policy search
- Healthcare documentation
- Financial reporting

## Implementing Vectorless RAG with Azure AI Foundry

### Step 1: Install and initialize PageIndex

```python
from pageindex import PageIndexClient
import pageindex.utils as utils

# Get your PageIndex API key from https://dash.pageindex.ai/api-keys
PAGEINDEX_API_KEY = "YOUR_PAGEINDEX_API_KEY"
pi_client = PageIndexClient(api_key=PAGEINDEX_API_KEY)
```

### Step 2: Set up your LLM using Azure OpenAI

```python
from openai import AsyncAzureOpenAI

client = AsyncAzureOpenAI(
  api_key=AZURE_OPENAI_API_KEY,
  azure_endpoint=AZURE_OPENAI_ENDPOINT,
  api_version=AZURE_OPENAI_API_VERSION
)

async def call_llm(prompt, temperature=0):
  response = await client.chat.completions.create(
    model=AZURE_DEPLOYMENT_NAME,
    messages=[{"role": "user", "content": prompt}],
    temperature=temperature
  )

  return response.choices[0].message.content.strip()
```

### Step 3: Page tree generation (download PDF + submit)

```python
import os, requests

pdf_url = "https://arxiv.org/pdf/2501.12948.pdf"  # example PDF
pdf_path = os.path.join("../data", pdf_url.split('/')[-1])
os.makedirs(os.path.dirname(pdf_path), exist_ok=True)

response = requests.get(pdf_url)
with open(pdf_path, "wb") as f:
  f.write(response.content)
print(f"Downloaded {pdf_url}")

doc_id = pi_client.submit_document(pdf_path)["doc_id"]
print('Document Submitted:', doc_id)
```

### Step 4: Print the generated PageIndex tree

```python
if pi_client.is_retrieval_ready(doc_id):
  tree = pi_client.get_tree(doc_id, node_summary=True)['result']
  print('Simplified Tree Structure of the Document:')
  utils.print_tree(tree)
else:
  print("Processing document, please try again later...")
```

### Step 5: Use the LLM to search the tree and pick relevant nodes

```python
import json

query = "What are the conclusions in this document?"

tree_without_text = utils.remove_fields(tree.copy(), fields=['text'])

search_prompt = f"""
You are given a question and a tree structure of a document. Each node contains a node id, node title, and a corresponding summary. Your task is to find all nodes that are likely to contain the answer to the question.

Question: {query}

Document tree structure: {json.dumps(tree_without_text, indent=2)}

Please reply in the following JSON format:
{{ "thinking": "<Your thinking process on which nodes are relevant to the question>", "node_list": ["node_id_1", "node_id_2", ..., "node_id_n"] }}
Directly return the final JSON structure. Do not output anything else.
"""

tree_search_result = await call_llm(search_prompt)
```

### Step 6: Print retrieved nodes and the reasoning process

```python
node_map = utils.create_node_mapping(tree)
tree_search_result_json = json.loads(tree_search_result)

print('Reasoning Process:')
utils.print_wrapped(tree_search_result_json['thinking'])

print('\nRetrieved Nodes:')
for node_id in tree_search_result_json["node_list"]:
  node = node_map[node_id]
  print(f"Node ID: {node['node_id']}\t Page: {node['page_index']}\t Title: {node['title']}")
```

### Step 7: Generate the final answer from retrieved node text

```python
node_list = json.loads(tree_search_result)["node_list"]
relevant_content = "\n\n".join(node_map[node_id]["text"] for node_id in node_list)

print('Retrieved Context:\n')
utils.print_wrapped(relevant_content[:1000] + '...')

answer_prompt = f"""
Answer the question based on the context:

Question: {query}
Context: {relevant_content}

Provide a clear, concise answer based only on the context provided.
"""

print('Generated Answer:\n')
answer = await call_llm(answer_prompt)
utils.print_wrapped(answer)
```

## When to Use Each Approach

Both approaches can make sense depending on document shape and retrieval needs.

### When to use vector database–based RAG

Vector retrieval tends to work well for large collections of unrelated or loosely structured documents.

Use vector RAG when:

- Searching across many independent documents
- Semantic similarity is sufficient
- Real-time retrieval is required over very large datasets

Common use cases:

- Customer support knowledge bases
- Conversational chatbots
- Product and content search systems

### When to use vectorless RAG

Vectorless approaches like PageIndex are better for long, structured documents where the logical organization matters.

Use vectorless RAG when:

- Documents have clear hierarchical structure
- Logical reasoning across sections is required
- High retrieval accuracy is critical

Typical examples:

- Financial filings and regulatory reports
- Legal documents and contracts
- Technical manuals and documentation
- Academic and research papers

## Conclusion

Vector databases helped RAG scale semantic search across large datasets, but they’re not always optimal.

Vectorless approaches like PageIndex focus on retrieving content that is **logically relevant**, by reasoning over a document’s structure instead of relying on embeddings. As RAG evolves, hybrid systems may combine vector search for broad recall with reasoning-based navigation for precision.


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/vectorless-reasoning-based-rag-a-new-approach-to-retrieval/ba-p/4502238)

