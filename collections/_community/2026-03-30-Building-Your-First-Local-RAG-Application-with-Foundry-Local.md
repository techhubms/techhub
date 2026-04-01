---
date: 2026-03-30 07:00:00 +00:00
tags:
- AI
- Better Sqlite3
- CAG
- Chunking
- Community
- Context Augmented Generation
- Cosine Similarity
- Express.js
- Foundry Local
- Foundry Local SDK
- Inverted Index
- JavaScript
- Local LLM
- Node.js
- Offline AI
- On Device AI
- Phi 3.5 Mini
- Prompt Engineering
- RAG
- Retrieval Augmented Generation
- Server Sent Events
- Small Language Models
- Source Attribution
- SQLite
- SSE
- System Prompt
- TF IDF
- Vector Store
- Winget
author: Lee_Stott
section_names:
- ai
title: Building Your First Local RAG Application with Foundry Local
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-your-first-local-rag-application-with-foundry-local/ba-p/4501968
feed_name: Microsoft Tech Community
primary_section: ai
---

Lee_Stott walks through building a fully offline, browser-based RAG support agent using Microsoft Foundry Local and the Foundry Local SDK, with a simple Node.js/Express backend, SQLite storage, and a TF-IDF retrieval pipeline that streams grounded answers (with sources) to desktop and mobile browsers.<!--excerpt_end-->

# Building Your First Local RAG Application with Foundry Local

This guide shows how to build a **fully offline RAG-powered support agent** that runs entirely on your machine: no cloud, no API keys, and no outbound network calls (after the initial model download). It’s designed for scenarios like field work where connectivity is unavailable, but you still need reliable answers grounded in local procedures.

![Screenshot of the Gas Field Support Agent landing page, showing a dark-themed chat interface with quick-action buttons for common questions](https://raw.githubusercontent.com/leestott/local-rag/main/screenshots/01-landing-page.png)

## What is Retrieval-Augmented Generation (RAG)?

**Retrieval-Augmented Generation (RAG)** grounds model responses in *your own documents*:

1. **Retrieve** relevant chunks from your documents using a vector store
2. **Augment** the model prompt with those chunks as context
3. **Generate** a response based on the provided context

Benefits called out in the post:

- Fewer hallucinations
- Traceable answers with **source attribution**
- Works with your domain content (internal tools, support bots, field manuals, knowledge bases)

## RAG vs CAG (Context-Augmented Generation)

Both RAG and CAG aim to ground answers in your content.

### RAG (Retrieval-Augmented Generation)

**How it works**: split documents into chunks, store vectors in a database, retrieve relevant chunks at query time.

**Strengths**:

- Scales to large doc sets (hundreds → millions)
- Chunk-level retrieval and source attribution
- Docs can be added/updated dynamically (including runtime upload)
- Token-efficient: only relevant chunks are sent

**Limitations**:

- More moving parts (vector store + chunking strategy)
- Retrieval quality depends on chunking/scoring
- Retrieval step can miss content

### CAG (Context-Augmented Generation)

**How it works**: load all docs at startup, select relevant docs per query using keyword scoring.

**Strengths**:

- Much simpler (no vector DB/embeddings)
- Minimal dependencies
- Near-instant document selection

**Limitations**:

- Constrained by context window
- Best for small, curated sets (tens of docs)
- Adding docs requires restart

> **Want to compare hands-on?** The post links a CAG-based version of the same scenario: https://github.com/leestott/local-cag

### When to choose which

| Consideration | Choose RAG | Choose CAG |
| --- | --- | --- |
| Document count | Hundreds or thousands | Tens of documents |
| Document updates | Frequent or dynamic (runtime upload) | Infrequent (restart to reload) |
| Source attribution | Per-chunk with relevance scores | Per-document |
| Setup complexity | Moderate (needs ingestion) | Minimal |
| Query precision | Better for large/diverse collections | Good for keyword-matchable content |
| Infrastructure | SQLite vector store (single file) | None beyond the runtime |

For the sample app (20 documents + runtime upload), the post recommends **RAG**.

## Foundry Local: on-device AI runtime

[Foundry Local](https://foundrylocal.ai) is described as a Microsoft lightweight runtime that downloads, manages, and serves language models locally on your device.

Key points highlighted:

- **No GPU required** (CPU or NPU supported)
- **Native SDK bindings** via `foundry-local-sdk` (in-process inference; no HTTP hop to a local server)
- Automatic model download/cache/load
- Hardware-optimised model variant selection
- Progress callbacks for download/init UI

### Minimal integration example

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Create a manager and discover models via the catalogue
const manager = FoundryLocalManager.create({ appName: "gas-field-local-rag" });
const model = await manager.catalog.getModel("phi-3.5-mini");

// Download if not cached, then load into memory
if (!model.isCached) {
  await model.download((progress) => {
    console.log(`Download: ${Math.round(progress * 100)}%`);
  });
}
await model.load();

// Create a chat client for direct in-process inference
const chatClient = model.createChatClient();
const response = await chatClient.completeChat([
  { role: "system", content: "You are a helpful assistant." },
  { role: "user", content: "How do I detect a gas leak?" }
]);
```

## Technology stack

The sample intentionally avoids frameworks/build steps/Docker.

| Layer | Technology | Purpose |
| --- | --- | --- |
| AI model | Foundry Local + Phi-3.5 Mini | Local inference via native SDK bindings |
| Backend | Node.js + Express | Serves UI + SSE status/chat endpoints |
| Vector store | SQLite via `better-sqlite3` | Single-file local storage |
| Retrieval | TF-IDF + cosine similarity | Offline retrieval without embeddings |
| Frontend | Single HTML file with inline CSS | No build step; mobile responsive |

Dependencies: `express`, `foundry-local-sdk`, `better-sqlite3`.

## Architecture overview

![Architecture diagram showing five layers: Client (HTML/CSS/JS), Server (Express.js), RAG Pipeline (chunker, TF-IDF, chat engine), Data Layer (SQLite), and AI Layer (Foundry Local)](https://raw.githubusercontent.com/leestott/local-rag/main/screenshots/07-architecture-diagram.png)

Five layers on one machine:

- **Client**: single HTML file (responsive chat UI + quick actions)
- **Server**: Express serves UI + status/chat endpoints (SSE)
- **RAG pipeline**: chunker + TF-IDF vectorisation + chat engine + prompts module
- **Data**: SQLite stores chunks + TF-IDF vectors; source docs live as `.md` in `docs/`
- **AI**: Foundry Local runs Phi-3.5 Mini via SDK (CPU/NPU)

## Build it step by step

### Prerequisites

1. Node.js 20+ (https://nodejs.org/)
2. Foundry Local installed via winget:

```powershell
winget install Microsoft.FoundryLocal
```

Note: the first run downloads the Phi-3.5 Mini model (~2 GB).

### Run the sample

```bash
# Clone the repository
git clone https://github.com/leestott/local-rag.git
cd local-rag

# Install dependencies
npm install

# Ingest the documents into the vector store
npm run ingest

# Start the server
npm start
```

Open `http://127.0.0.1:3000`. While the model loads, the UI shows status; when ready it changes to **"Offline Ready"**.

![Desktop view of the application showing the chat interface with quick-action buttons](https://raw.githubusercontent.com/leestott/local-rag/main/screenshots/01-landing-page.png)

![Mobile view of the application showing the responsive layout on a smaller screen](https://raw.githubusercontent.com/leestott/local-rag/main/screenshots/02-mobile-view.png)

## How the RAG pipeline works

Example user question: **"How do I detect a gas leak?"**

![Sequence diagram showing the RAG query flow: user sends a question, the server retrieves relevant chunks from SQLite, constructs a prompt, sends it to Foundry Local, and streams the response back](https://raw.githubusercontent.com/leestott/local-rag/main/screenshots/08-rag-flow-sequence.png)

1. **Ingest + index docs** (`npm run ingest`)
   - Read `.md` files in `docs/`
   - Optional YAML front-matter (title/category/ID)
   - Split into ~200-token overlapping chunks
   - Store in SQLite with TF-IDF vectors
2. **Load the model**
   - SDK discovers model in local catalog
   - Downloads if missing; progress streamed to browser via SSE
3. **Question arrives**
   - Express receives question
   - Convert question to TF-IDF vector
   - Inverted index finds candidate chunks
   - Cosine similarity ranks; top 3 chunks returned in <1 ms
4. **Construct prompt**
   - System prompt (safety-first)
   - Retrieved chunks as context
   - Conversation history
   - User question
5. **Generate response + stream**
   - Send to Foundry Local chat client
   - Stream tokens back via SSE
   - Include source references with relevance scores

![Chat response showing safety warnings followed by step-by-step gas leak detection guidance](https://raw.githubusercontent.com/leestott/local-rag/main/screenshots/03-chat-response.png)

![Sources panel showing the specific document chunks referenced in the response with relevance scores](https://raw.githubusercontent.com/leestott/local-rag/main/screenshots/04-sources-panel.png)

## Key code walkthrough

### Vector store search (TF-IDF + SQLite)

The store persists chunks + TF-IDF vectors in SQLite. At query time it uses an inverted index to narrow candidates and cosine similarity to rank.

```javascript
// src/vectorStore.js
search(query, topK = 5) {
  const queryTf = termFrequency(query);
  this._ensureCache(); // Build in-memory cache on first access

  // Use inverted index to find candidates sharing at least one term
  const candidateIndices = new Set();
  for (const term of queryTf.keys()) {
    const indices = this._invertedIndex.get(term);
    if (indices) {
      for (const idx of indices) candidateIndices.add(idx);
    }
  }

  // Score only candidates, not all rows
  const scored = [];
  for (const idx of candidateIndices) {
    const row = this._rowCache[idx];
    const score = cosineSimilarity(queryTf, row.tf);
    if (score > 0) scored.push({ ...row, score });
  }

  scored.sort((a, b) => b.score - a.score);
  return scored.slice(0, topK);
}
```

The post notes that the inverted index + in-memory cache + prepared SQL statements enable **sub-millisecond** retrieval for typical loads.

### Why TF-IDF instead of embeddings?

Reasons given:

- Fully offline (no embedding model to download/run)
- Low latency (word-frequency math)
- “Good enough” for ~20 domain docs
- More transparent than neural embeddings

It suggests swapping to embeddings for larger collections or when semantic similarity matters more.

### System prompt (safety-first)

```javascript
// src/prompts.js
export const SYSTEM_PROMPT = `You are a local, offline support agent for gas field inspection and maintenance engineers.

Behaviour Rules:
- Always prioritise safety. If a procedure involves risk,
explicitly call it out.
- Do not hallucinate procedures, measurements, or tolerances.
- If the answer is not in the provided context, say:
"This information is not available in the local knowledge base."

Response Format:
- Summary (1-2 lines)
- Safety Warnings (if applicable)
- Step-by-step Guidance
- Reference (document name + section)`;
```

The post points out this structure transfers well to other safety-critical domains.

## Runtime document upload

RAG supports adding documents without restarting. The UI upload accepts `.md` or `.txt` files, then chunks/vectorises/indexes them immediately.

![Upload document modal showing a file drop zone and a list of indexed documents with chunk counts](https://raw.githubusercontent.com/leestott/local-rag/main/screenshots/05-upload-document.png)

## Adapt it for your own domain

### 1) Replace documents

Swap out `docs/` with your own markdown files. Optional YAML front-matter is supported:

```markdown
---
title: Troubleshooting Widget Errors
category: Support
id: KB-001
---

# Troubleshooting Widget Errors
...your content here...
```

### 2) Edit the system prompt

Update `src/prompts.js`, keeping the structured response format.

### 3) Tune retrieval

In `src/config.js`:

- `chunkSize: 200` (smaller chunks = more precise retrieval, less context per chunk)
- `chunkOverlap: 25` (reduces “falling between chunks”)
- `topK: 3` (more chunks = more context but slower generation)

### 4) Swap the model

Change `config.model` in `src/config.js` to another model in the Foundry Local catalogue.

## Field-ready UI considerations

The frontend is a single HTML file with inline CSS to keep deployment simple.

Design choices mentioned:

- Dark, high-contrast theme + 18px base font size
- Touch targets >= 44px
- Quick-action buttons that wrap on mobile
- Responsive layout (320px to 1920px+)
- Streaming responses via SSE

![Mobile view of the chat interface showing a conversation with the AI agent on a small screen](https://raw.githubusercontent.com/leestott/local-rag/main/screenshots/06-mobile-chat.png)

## Testing

Uses the built-in Node.js test runner:

```bash
npm test
```

Tests cover chunker, vector store, configuration, and server endpoints.

## Ideas to extend

- Embedding-based retrieval (local embedding model)
- Persisted conversation memory
- Multi-modal support (image queries)
- PWA packaging
- Hybrid retrieval (TF-IDF + embeddings)
- Compare to the CAG sample: https://github.com/leestott/local-cag

## Links

- RAG sample: https://github.com/leestott/local-rag
- CAG sample: https://github.com/leestott/local-cag
- Foundry Local: https://foundrylocal.ai


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-your-first-local-rag-application-with-foundry-local/ba-p/4501968)

