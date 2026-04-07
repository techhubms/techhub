---
tags:
- AI
- CAG
- Community
- Context Augmented Generation
- Context Window
- Embeddings
- Express
- Foundry Local
- Foundry Local SDK
- FOUNDRY MODEL
- JavaScript
- Keyword Scoring
- Local Inference
- Model Selection
- Node.js
- Node.js 20
- Offline AI
- On Device AI
- Phi 3.5 Mini
- Phi 4
- Prompt Engineering
- RAG
- Retrieval Augmented Generation
- Server Sent Events
- SSE
- Streaming Responses
- System Prompt
- Unit Testing
- Vector Database
- Winget
- YAML Front Matter
feed_name: Microsoft Tech Community
author: Lee_Stott
section_names:
- ai
title: Build a Fully Offline AI App with Foundry Local and CAG
date: 2026-04-02 07:00:00 +00:00
primary_section: ai
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-a-fully-offline-ai-app-with-foundry-local-and-cag/ba-p/4502124
---

Lee_Stott walks through building a fully offline, on-device AI support agent using Microsoft Foundry Local and a Context-Augmented Generation (CAG) approach, including model auto-selection, SSE-based loading status, and a simple Node.js/Express architecture you can fork for your own domain.<!--excerpt_end-->

# Build a Fully Offline AI App with Foundry Local and CAG

This guide shows how to build a **fully offline, on-device AI support agent** using **Microsoft Foundry Local** and a pattern called **Context-Augmented Generation (CAG)**. The sample app is a browser-based chat UI backed by a Node.js/Express server, with local model inference via native SDK bindings.

![Screenshot of the Gas Field Support Agent landing page, showing a dark-themed chat interface with quick-action buttons for common questions](https://raw.githubusercontent.com/leestott/local-cag/main/screenshots/01-landing-page.png)

## Why offline AI?

The usual advice is “just call our API”, but that fails when:

- Users have **no connectivity** (e.g., field engineers)
- You have **strict privacy / data residency** requirements
- You want to avoid **ongoing cloud inference cost**

This project runs locally (after the initial model download) with **no outbound network calls**.

## What is Context-Augmented Generation (CAG)?

**Context-Augmented Generation (CAG)** grounds model responses in your own content by loading your knowledge base into memory and injecting the most relevant documents into the prompt for each question.

Core flow:

1. **Load** documents into memory at app startup.
2. **Inject** the most relevant documents into the prompt (per query).
3. **Generate** a response grounded in those documents.

Key characteristics:

- No retrieval pipeline
- No vector database
- No embeddings
- Document selection uses simple **keyword scoring**

## CAG vs RAG: trade-offs

Both CAG and **Retrieval-Augmented Generation (RAG)** solve the same problem (grounding answers in your content) but with different architectures.

### CAG

**How it works:** Load all documents at startup; select relevant docs per query via keyword scoring; inject them into the prompt.

**Strengths:**

- Much simpler architecture (no vector DB, embeddings, retrieval pipeline)
- Fully offline
- Minimal dependencies (two npm packages in the sample)
- Fast doc selection (no embedding latency)
- Easy to debug and reason about

**Limitations:**

- Limited by the model’s context window
- Best for **small curated sets** (tens of docs, not thousands)
- Keyword scoring is weaker than semantic similarity for ambiguous questions
- Adding docs requires an **app restart**

### RAG

**How it works:** Chunk docs, embed to vectors, store in a database; retrieve semantically similar chunks at query time.

**Strengths:**

- Scales to large document sets
- Semantic matching works better when wording differs
- Docs can be added/updated without restart
- Chunk-level retrieval can be more token-efficient

**Limitations:**

- More moving parts: chunking + embedding model + vector DB
- Quality depends heavily on tuning
- Adds latency
- More infra and dependencies

To compare hands-on, the author also provides a RAG version of the same scenario:

- CAG sample: https://github.com/leestott/local-cag
- RAG sample: https://github.com/leestott/local-rag

## Foundry Local: on-device AI runtime

**Foundry Local** is a lightweight Microsoft runtime that downloads, caches, loads, and serves language models **on your device**.

Notable developer-facing behavior in this sample:

- App uses `foundry-local-sdk` and creates a `FoundryLocalManager`
- App reads the **local model catalogue**
- App applies a **runtime selection policy** (based on available RAM)
- If the model is not cached, the app triggers a download (with progress callbacks)
- Model loads into memory and is used via **in-process inference** (no HTTP calls to a local server)

Reasons for runtime model selection:

- A single fixed model can be too big for low-spec machines or waste capability on high-spec machines.
- The sample targets mixed hardware by choosing the best model that fits a RAM budget.

### Minimal integration pattern

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Create a manager and get the model catalogue
const manager = FoundryLocalManager.create({ appName: "my-app" });

// Auto-select the best model for this device based on available RAM
const models = await manager.catalog.getModels();
const model = selectBestModel(models);

// Download if not cached, then load into memory
if (!model.isCached) {
  await model.download((progress) => {
    console.log(`Download: ${progress.toFixed(0)}%`);
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

## Technology stack (sample)

The sample is intentionally simple (no frameworks, no build steps, no Docker):

| Layer | Technology | Purpose |
| --- | --- | --- |
| AI model | Foundry Local + auto-selected model | Local inference via native SDK bindings |
| Back end | Node.js + Express | HTTP server + API routes |
| Context | Markdown docs pre-loaded at startup | No vector DB, no embeddings |
| Front end | Single HTML file with inline CSS | No build step; mobile-responsive |

Dependencies:

- `express`
- `foundry-local-sdk`

## Architecture overview

![Architecture diagram showing four layers: Client (HTML/CSS/JS), Server (Express.js), CAG Engine (document loading, keyword scoring, prompt construction), and AI Layer (Foundry Local with in-process inference)](https://raw.githubusercontent.com/leestott/local-cag/main/screenshots/07-architecture-diagram.png)

Four layers in a single process:

- **Client layer:** single HTML file served by Express
- **Server layer:** Express serves UI + an SSE status endpoint; routes for chat (streaming and non-streaming), context listing, and health checks
- **CAG engine:** loads docs at startup; keyword-scores docs per query; injects into prompt
- **AI layer:** Foundry Local model runs on CPU/NPU via native bindings (no HTTP round-trips)

## Step-by-step: get it running

### Prerequisites

1. Node.js **20+**: https://nodejs.org/
2. Foundry Local runtime:

```powershell
winget install Microsoft.FoundryLocal
```

Optional override to force a specific model:

- Set `FOUNDRY_MODEL` to a model alias.

### Run the sample

```bash
# Clone the repository
git clone https://github.com/leestott/local-cag.git
cd local-cag

# Install dependencies
npm install

# Start the server
npm start
```

Open `http://127.0.0.1:3000`.

On first run, the UI shows a loading overlay while the model downloads and loads.

![Desktop view of the application showing the chat interface with quick-action buttons](https://raw.githubusercontent.com/leestott/local-cag/main/screenshots/01-landing-page.png)

![Mobile view of the application showing the responsive layout on a smaller screen](https://raw.githubusercontent.com/leestott/local-cag/main/screenshots/02-mobile-view.png)

## How the CAG pipeline works (request flow)

Example user question: **“How do I detect a gas leak?”**

![Sequence diagram showing the CAG query flow: user sends a question, the server selects relevant documents, constructs a prompt, sends it to Foundry Local, and streams the response back](https://raw.githubusercontent.com/leestott/local-cag/main/screenshots/08-rag-flow-sequence.png)

1. **Server starts and loads documents**
   - Reads all `.md` files from `docs/`
   - Parses optional YAML front-matter (title, category, ID)
   - Builds a document index
2. **Model is selected and loaded**
   - Evaluates available RAM
   - Picks best fitting model from catalogue
   - Downloads if missing (progress streamed to browser via SSE)
   - Loads model into memory
3. **User sends a question**
   - Server receives it; CAG engine keyword-scores documents
   - Selects top 3 docs
4. **Prompt is constructed**
   - Includes system prompt, document index, top docs (about 6,000 chars), conversation history, and user question
5. **Model generates a grounded response**
   - Response streams token-by-token back to browser via SSE

![Chat response showing safety warnings followed by step-by-step gas leak detection guidance](https://raw.githubusercontent.com/leestott/local-cag/main/screenshots/03-chat-response.png)

![Sources panel showing the specific documents referenced in the response](https://raw.githubusercontent.com/leestott/local-cag/main/screenshots/04-sources-panel.png)

## Key code walkthrough

### Load documents (context module)

Documents are loaded into memory as plain text (no chunking, vectors, or DB):

```javascript
// src/context.js
export function loadDocuments() {
  const files = fs.readdirSync(config.docsDir)
    .filter(f => f.endsWith(".md"))
    .sort();

  const docs = [];
  for (const file of files) {
    const raw = fs.readFileSync(path.join(config.docsDir, file), "utf-8");
    const { meta, body } = parseFrontMatter(raw);
    docs.push({
      id: meta.id || path.basename(file, ".md"),
      title: meta.title || file,
      category: meta.category || "General",
      content: body.trim(),
    });
  }
  return docs;
}
```

### Dynamic model selection (RAM budget)

The selector uses a RAM budget (example shown uses 60%):

```javascript
// src/modelSelector.js
const totalRamMb = os.totalmem() / (1024 * 1024);
const budgetMb = totalRamMb * 0.6; // Use up to 60% of system RAM

// Filter to models that fit, rank by quality, boost cached models
const candidates = allModels.filter(m =>
  m.task === "chat-completion" && m.fileSizeMb <= budgetMb
);

// Returns the best model: e.g. phi-4 on a 32 GB machine,
// or phi-3.5-mini on a laptop with 8 GB RAM
```

### Safety-focused system prompt

The system prompt is structured to reduce hallucinations and enforce safe responses:

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

## Adapting the project to your domain

1. **Replace documents** in `docs/` with your own markdown files (optional YAML front-matter supported):

```markdown
---
title: Troubleshooting Widget Errors
category: Support
id: KB-001
---

# Troubleshooting Widget Errors
...your content here...
```

2. **Edit the system prompt** in `src/prompts.js` (keep the structure, change the domain language).

3. **Override the model (optional)**:

```bash
# See available models
foundry model list

# Force a smaller, faster model
FOUNDRY_MODEL=phi-3.5-mini npm start

# Or a larger, higher-quality model
FOUNDRY_MODEL=phi-4 npm start
```

## Field-ready UI choices

The UI is a single HTML file (no React/build tooling) and is tuned for field usage:

- Dark, high-contrast theme with 18px base font
- Large touch targets (48px minimum)
- Quick-action buttons for common questions
- Responsive layout (320px to 1920px+)
- Streaming responses via SSE

![Mobile view of the chat interface showing a conversation with the AI agent on a small screen](https://raw.githubusercontent.com/leestott/local-cag/main/screenshots/06-mobile-chat.png)

## Visual startup progress using SSE

The app broadcasts initialization stages to the browser:

1. Loading documents
2. Initializing Foundry Local SDK
3. Selecting model
4. Downloading model (first run)
5. Loading model

Server-side broadcast example:

```javascript
// Server-side: broadcast status to all connected browsers
function broadcastStatus(state) {
  initState = state;
  const payload = `data: ${JSON.stringify(state)}\n\n`;
  for (const client of statusClients) {
    client.write(payload);
  }
}

// During initialisation:
broadcastStatus({ stage: "downloading", message: "Downloading phi-4...", progress: 42 });
```

## Testing

Uses the built-in Node.js test runner:

```bash
npm test
```

Covers configuration, server endpoints, and document loading.

## Extension ideas

- Persisted conversation memory (local storage or lightweight DB)
- Hybrid CAG + RAG for larger doc collections
- Multimodal queries (image input)
- PWA packaging for installable offline usage
- Custom fine-tuning for better domain accuracy

## Summary

- **CAG** is a good fit for small, curated document sets where simplicity and offline capability matter.
- **RAG** scales better for large, frequently changing document collections.
- **Foundry Local** enables practical on-device inference with model management and native SDK bindings.

Resources:

- CAG sample: https://github.com/leestott/local-cag
- RAG sample: https://github.com/leestott/local-rag


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-a-fully-offline-ai-app-with-foundry-local-and-cag/ba-p/4502124)

