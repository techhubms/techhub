---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-parse-chunk-with-metadata-in-logic-apps-build-context/ba-p/4458438
title: Enhancing Logic Apps with Parse & Chunk with Metadata for AI-Powered Document Processing
author: shahparth
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-10-01 23:25:57 +00:00
tags:
- Agent Loop
- Azure AI Search
- Azure OpenAI
- Blob Storage
- Chunk Text With Metadata
- Conversational AI
- Document Indexing
- Enterprise Contracts
- GPT 4.1
- Logic Apps
- Metadata Extraction
- Natural Language Processing
- Parse Document With Metadata
- RAG (retrieval Augmented Generation)
- Vector Search
section_names:
- ai
- azure
---
shahparth discusses leveraging new AI-enhanced Logic Apps actions to parse and chunk documents with contextual metadata, integrating these capabilities into Azure AI Search and Agent Loops for production-ready document Q&A.<!--excerpt_end-->

# Enhancing Logic Apps with Parse & Chunk with Metadata for AI-Powered Document Processing

**Author: shahparth**

The Logic Apps team has introduced two powerful new actions—**Parse document with metadata** and **Chunk text with metadata**—to improve document handling workflows. These actions go beyond basic text extraction, capturing rich metadata such as `pageNumber`, `totalPages`, and sentence completeness, enabling more accurate citation and efficient processing for downstream AI workflows.

## Why Metadata Matters

Traditional document parsing actions output raw text, often resulting in fragmented sentences and loss of critical context like page locations. The new metadata-enriched actions ensure that each text chunk:

- Is accompanied by its originating page number (`pageNumber`)
- Includes the total number of pages (`totalPages`)
- Ends on a complete sentence (`sentencesAreComplete`)

This enables precise citation, better navigation, and improved processing reliability for tasks such as contract review, compliance, and knowledge retrieval.

## End-to-End Workflow Walkthrough

### Prerequisites

- Azure Blob Storage: For staging your PDF or document files
- Azure AI Search: With an index to hold embedded and chunked document data
- Azure OpenAI deployment: For embeddings and chat-based Q&A (GPT-4.1 recommended)
- Logic Apps (Standard): To orchestrate the workflow using the new AI actions

A [sample GitHub demo](https://github.com/Azure/logicapps/tree/shahparth-lab-patch-1/ws-vscode) is available to accelerate your setup.

### Step 1: Ingestion Flow

**Objective:** Convert uploaded PDFs into sentence-complete chunks with metadata and index them.

Workflow overview:

- Trigger: When a blob (document) is added or updated in Blob Storage
- Read blob content
- Parse the document, extracting text along with metadata
- Chunk the text, maintaining sentence and page structural boundaries
- Generate vector embeddings for each chunk via Azure OpenAI
- Prepare index objects with text, embedding, and metadata fields
- Batch index these objects into Azure AI Search

This approach ensures each indexed item links back to its source page and position, laying the groundwork for highly accurate retrievals.

### Step 2: Agent Flow with Vector Search

**Objective:** Enable users to ask questions about contracts and receive AI-generated answers with reliable citations.

- Set up a Conversational workflow in Logic Apps
- Configure the Agent action with GPT-4.1, instructing it to use only retrieved, indexed content to answer questions
- Pass chat prompts into an Agent Parameter, which feeds user queries into the Vector Search tool
- The tool retrieves contextually relevant chunks from Azure AI Search (using vector similarity)
- The agent forms a response solely based on these chunks, including page number citations

This architecture ensures answers are always grounded in enterprise documents and traceable to their source.

#### Example Scenario

Whenever a new file is added:

- The ingestion workflow parses, chunks, embeds, and indexes it
- When a user asks, “What is the standard payment timeline?”, the conversational agent:
  - Performs a vector search against the indexed content
  - Extracts the correct chunk (with metadata such as page number)
  - Generates an answer citing the specific contract page

### Step 3: Observability and Validation

- Indexing and Q&A runs are easily traceable in Logic Apps run history, confirming that metadata is attached and responses include proper citations.
- The agent flow remains declarative and easy to debug, with each tool action traceable in the Logic Apps portal.

## Conclusion

The new **Parse & Chunk with Metadata** actions, in tandem with Azure AI Search and Agent Loop workflows, provide a robust foundation for building context-aware, production-grade Retrieval-Augmented Generation (RAG) solutions in Microsoft Azure. By bringing metadata into every step, organizations can ensure trustworthy, referenceable answers for enterprise documents with minimal custom code.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-parse-chunk-with-metadata-in-logic-apps-build-context/ba-p/4458438)
