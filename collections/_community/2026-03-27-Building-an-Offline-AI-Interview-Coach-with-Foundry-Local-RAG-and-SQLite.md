---
tags:
- AI
- Azure AI Foundry
- CLI
- Community
- Cosine Similarity
- Document Chunking
- Express.js
- Foundry Local
- Homebrew
- JavaScript
- Local LLM
- Microsoft Foundry Local
- Node.js
- Offline AI
- On Device AI
- OpenAI Compatible API
- Phi 3.5 Mini
- Prompt Engineering
- RAG
- Retrieval Augmented Generation
- Server Sent Events
- Sql.js
- SQLite
- SSE
- Streaming Chat Completions
- TF IDF
- Winget
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-an-offline-ai-interview-coach-with-foundry-local-rag/ba-p/4500614
author: Lee_Stott
section_names:
- ai
primary_section: ai
date: 2026-03-27 07:00:00 +00:00
feed_name: Microsoft Tech Community
title: Building an Offline AI Interview Coach with Foundry Local, RAG, and SQLite
---

Lee_Stott walks through building “Interview Doctor”, a 100% offline interview-prep assistant that runs locally using Microsoft Foundry Local plus a lightweight RAG pipeline backed by SQLite and TF‑IDF, with both a web UI and a CLI.<!--excerpt_end-->

## Overview

This post describes how **Interview Doctor** works: an **offline** AI interview preparation tool that ingests your CV and a job description, retrieves relevant snippets, and generates tailored interview questions—without sending any data to the cloud.

It’s built as a local-first JavaScript app using:

- **Foundry Local** (Microsoft’s on-device AI runtime)
- **RAG (Retrieval-Augmented Generation)**
- **SQLite** (via `sql.js`) to store chunks and TF-IDF vectors
- **Node.js + Express.js** for the server
- **Node.js built-in test runner** for tests (no Jest/Mocha)

![Interview Doctor - Landing Page](https://github.com/leestott/interview-doctor-js/raw/main/screenshots/01-landing-page.png)

## Why RAG (Retrieval-Augmented Generation)

RAG improves usefulness for domain-specific tasks by:

1. **Retrieving** relevant chunks from your documents
2. **Augmenting** the prompt with those chunks
3. **Generating** grounded output based on your data

For Interview Doctor, that means questions tied to *your CV* and the *specific job posting*, rather than generic interview prompts.

### Why offline RAG

Key benefits called out in the post:

- Privacy (CV and job applications stay on-device)
- No API costs
- No rate limits
- Works without internet
- No API latency/cold starts

## Architecture

The app has **two interfaces**—a **CLI** and a **Web UI**—sharing a single core engine.

![Interview Doctor Architecture Diagram](https://github.com/leestott/interview-doctor-js/raw/main/screenshots/architecture.png)

Core flow:

1. **Document ingestion**: PDFs and markdown are chunked and indexed
2. **Vector store**: SQLite stores text chunks + TF-IDF vectors
3. **Retrieval**: cosine similarity finds the best chunk matches
4. **Generation**: retrieved chunks are injected into the prompt sent to the local model

## Step 1: Set up Foundry Local

Install Foundry Local:

```bash
# Windows
winget install Microsoft.FoundryLocal

# macOS
brew install microsoft/foundrylocal/foundrylocal
```

Use the JavaScript SDK to start the service, download the model, and connect.

```js
import { FoundryLocalManager } from "foundry-local-sdk";
import { OpenAI } from "openai";

const manager = new FoundryLocalManager();
const modelInfo = await manager.init("phi-3.5-mini");

// Foundry Local exposes an OpenAI-compatible API
const openai = new OpenAI({
  baseURL: manager.endpoint, // Dynamic port, discovered by SDK
  apiKey: manager.apiKey,
});
```

### Key insight: dynamic port

Foundry Local uses a **dynamic port**, so the post warns not to hardcode something like `localhost:5272`. Use `manager.endpoint` discovered at runtime.

## Step 2: Build the RAG pipeline

### Document chunking

Documents are chunked into ~200-token chunks with ~25-token overlap.

```js
export function chunkText(text, maxTokens = 200, overlapTokens = 25) {
  const words = text.split(/\s+/).filter(Boolean);
  if (words.length <= maxTokens) return [text.trim()];

  const chunks = [];
  let start = 0;
  while (start < words.length) {
    const end = Math.min(start + maxTokens, words.length);
    chunks.push(words.slice(start, end).join(" "));
    if (end >= words.length) break;
    start = end - overlapTokens;
  }
  return chunks;
}
```

Rationale given:

- Smaller chunks keep retrieved context compact
- Overlap reduces boundary information loss
- Uses simple string operations (no extra dependencies)

### TF-IDF vectors (instead of embeddings)

To avoid the memory cost of running an additional embedding model alongside the LLM, the post uses **TF-IDF-style term frequency vectors** plus **cosine similarity**.

```js
export function termFrequency(text) {
  const tf = new Map();
  const tokens = text
    .toLowerCase()
    .replace(/[^a-z0-9\-']/g, " ")
    .split(/\s+/)
    .filter((t) => t.length > 1);

  for (const t of tokens) {
    tf.set(t, (tf.get(t) || 0) + 1);
  }
  return tf;
}

export function cosineSimilarity(a, b) {
  let dot = 0,
    normA = 0,
    normB = 0;

  for (const [term, freq] of a) {
    normA += freq * freq;
    if (b.has(term)) dot += freq * b.get(term);
  }

  for (const [, freq] of b) normB += freq * freq;

  if (normA === 0 || normB === 0) return 0;
  return dot / (Math.sqrt(normA) * Math.sqrt(normB));
}
```

At query time, a query term-frequency vector is computed and scored against stored chunk vectors.

### SQLite as the vector store

Chunks and their vectors are stored in SQLite using `sql.js`.

```js
export class VectorStore {
  // Created via: const store = await VectorStore.create(dbPath)

  insert(docId, title, category, chunkIndex, content) {
    const tf = termFrequency(content);
    const tfJson = JSON.stringify([...tf]);

    this.db.run(
      "INSERT INTO chunks (...) VALUES (?, ?, ?, ?, ?, ?)",
      [docId, title, category, chunkIndex, content, tfJson]
    );

    this.save();
  }

  search(query, topK = 5) {
    const queryTf = termFrequency(query);
    // Score each chunk by cosine similarity, return top-K
  }
}
```

Reasoning: for small document sets (CV + a few job descriptions), brute-force scoring over SQLite rows is effectively instant (the post cites ~1ms), avoiding the need for dedicated vector DBs.

## Step 3: RAG chat engine (retrieve + generate)

The chat engine:

- retrieves relevant chunks
- builds a prompt with retrieved context
- streams output from the local model

```js
async *queryStream(userMessage, history = []) {
  // 1. Retrieve relevant CV/JD chunks
  const chunks = this.retrieve(userMessage);
  const context = this._buildContext(chunks);

  // 2. Build the prompt with retrieved context
  const messages = [
    { role: "system", content: SYSTEM_PROMPT },
    { role: "system", content: `Retrieved context:\n\n${context}` },
    ...history,
    { role: "user", content: userMessage },
  ];

  // 3. Stream from the local model
  const stream = await this.openai.chat.completions.create({
    model: this.modelId,
    messages,
    temperature: 0.3,
    stream: true,
  });

  // 4. Yield chunks as they arrive
  for await (const chunk of stream) {
    const content = chunk.choices[0]?.delta?.content;
    if (content) yield { type: "text", data: content };
  }
}
```

The post notes using `temperature: 0.3` to keep responses focused and consistent for interview prep.

## Step 4: Two interfaces (Web + CLI)

### Web UI

- A **single HTML file** with inline CSS/JS
- No build step, no React/Vue
- Talks to the backend using REST + SSE

Features mentioned:

- File upload via `multipart/form-data`
- Streaming responses via **Server-Sent Events (SSE)**
- Quick actions (coaching tips, gap analysis, mock interview)

![Interview Doctor - Form filled with job details](https://github.com/leestott/interview-doctor-js/raw/main/screenshots/02-form-filled.png)

### CLI

Run:

```bash
npm run cli
```

The CLI walks through uploading a CV, entering job details, and then supports interactive follow-up questions. Both UI layers share the same `ChatEngine`.

### Edge mode

There’s an “Edge mode” for constrained devices that uses a smaller system prompt.

![Interview Doctor - Edge Mode enabled](https://github.com/leestott/interview-doctor-js/raw/main/screenshots/03-edge-mode.png)

## Step 5: Testing

Tests use Node’s built-in test runner.

```js
import { describe, it } from "node:test";
import assert from "node:assert/strict";

describe("chunkText", () => {
  it("returns single chunk for short text", () => {
    const chunks = chunkText("short text", 200, 25);
    assert.equal(chunks.length, 1);
  });

  it("maintains overlap between chunks", () => {
    // Verifies overlapping tokens between consecutive chunks
  });
});
```

Run:

```bash
npm test
```

The post states tests cover chunking, vector store, config, prompts, and the server API contract, without requiring Foundry Local to be running.

## How to adapt the pattern

Interview Doctor is presented as a reusable pattern:

| What to change | How |
| --- | --- |
| Domain documents | Replace files in `docs/` with your content |
| System prompt | Edit `src/prompts.js` |
| Chunk sizes | Adjust `config.chunkSize` and `config.chunkOverlap` |
| Model | Change `config.model` (use `foundry model list`) |
| UI | Modify `public/index.html` (single file) |

Ideas suggested:

- Customer support bot
- Code review assistant
- Study guide
- Compliance checker
- Onboarding assistant

## Lessons learned (from the post)

1. Offline AI can be production-ready for focused tasks (Foundry Local + small models like Phi-3.5 Mini).
2. For small doc sets, SQLite + TF-IDF can replace a vector DB.
3. Chunking choices have a big effect on RAG quality.
4. An OpenAI-compatible API makes cloud-to-local swaps mostly a `baseURL` change.
5. Sharing a single engine makes a Web UI and CLI straightforward.

### Performance notes

On a typical laptop (no GPU):

- ingestion: under 1 second for ~20 documents
- retrieval: ~1ms
- first token: 2–5 seconds

Foundry Local automatically selects the best variant for available hardware (CUDA GPU, NPU, or CPU).

## Getting started

```bash
git clone https://github.com/leestott/interview-doctor-js.git
cd interview-doctor-js
npm install
npm run ingest
npm start

# Web UI at http://127.0.0.1:3000
# or
npm run cli
```

## Resources

- [Foundry Local](https://foundrylocal.ai/)
- [Foundry Local SDK (npm)](https://www.npmjs.com/package/foundry-local-sdk)
- [Foundry Local GitHub](https://github.com/microsoft/Foundry-Local)
- [Local RAG Reference](https://github.com/leestott/local-rag)
- [Interview Doctor (JavaScript)](https://github.com/leestott/interview-doctor-js)


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-an-offline-ai-interview-coach-with-foundry-local-rag/ba-p/4500614)

