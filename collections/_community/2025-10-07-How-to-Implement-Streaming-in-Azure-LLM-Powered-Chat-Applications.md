---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/the-importance-of-streaming-for-llm-powered-chat-applications/ba-p/4459574
title: How to Implement Streaming in Azure LLM-Powered Chat Applications
author: Pamela_Fox
feed_name: Microsoft Tech Community
date: 2025-10-07 05:57:30 +00:00
tags:
- API Integration
- Azure AI Search
- Azure OpenAI Service
- Backend Development
- Chat Applications
- FastAPI
- Frontend Development
- JavaScript
- LLM
- NDJSON
- OpenAI Python SDK
- Python
- Quart
- RAG
- React
- Readable Streams
- Server Sent Events
- Streaming
- WebSockets
- AI
- Azure
- Coding
- Community
section_names:
- ai
- azure
- coding
primary_section: ai
---
Pamela Fox demonstrates the value and implementation of streaming for LLM-powered chat applications on Azure, guiding developers through backend and frontend techniques, complete with sample repositories and actionable tips.<!--excerpt_end-->

# How to Implement Streaming in Azure LLM-Powered Chat Applications

Pamela Fox details why **streaming responses** have become a cornerstone for modern chat-based user experiences, especially in applications powered by large language models (LLMs) like those provided by Azure OpenAI Service.

## Why Streaming Matters

As conversational interfaces such as ChatGPT and GitHub Copilot gain traction, users expect answers to appear in real time. Streaming allows the frontend to render answers word-by-word (or token-by-token), reducing perceived latency (lower "time to first token") and improving engagement.

## Template Repositories Supporting Streaming

Microsoft and contributors have open-sourced multiple Azure-based chat application templates with streaming support, including:

- [Simple Azure OpenAI chat app](https://github.com/Azure-Samples/openai-chat-app-quickstart)
- [RAG (Retrieval-Augmented Generation) chat app](https://github.com/Azure-Samples/azure-search-openai-demo/)
- [rag-postgres-openai-python](https://github.com/Azure-Samples/rag-postgres-openai-python/)
- [openai-chat-backend-fastapi](https://github.com/Azure-Samples/openai-chat-backend-fastapi)
- [deepseek-python](https://github.com/Azure-Samples/deepseek-python)

All repositories provide out-of-the-box streaming features for both frontend and backend.

## Streaming From APIs

Most LLM APIs and client libraries support streaming via a flag or method. For example, with the OpenAI Python SDK:

```python
completion_stream = openai_client.chat.completions.create(
    model="gpt-5-mini",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "What does a product manager do?"}
    ],
    stream=True,
)

for chunk in await completion_stream:
    content = event.choices[0].delta.content
```

## Streaming From Backend to Frontend

Regular HTTP responses close the connection after sending all data, so alternatives are needed for progressive delivery:

- **WebSockets**: Bi-directional, useful for real-time apps
- **Server-sent events (SSE)**: Server pushes events over HTTP
- **Readable streams**: Chunked HTTP responses

Pamela's preferred approach is **readable streams** for their simplicity and compatibility with POST requests, using NDJSON to partition streamed data.

- Example guides:
  - [Streaming ChatGPT with SSE](http://blog.pamelafox.org/2023/05/streaming-chatgpt-with-server-sent.html)
  - [Fetching JSON Over Streaming HTTP](http://blog.pamelafox.org/2023/08/fetching-json-over-streaming-http.html)

## Achieving Smooth Word-by-Word Display

Browser optimizations can cause entire sentences to appear at once, even if tokens are streamed individually. To mimic true word-by-word rendering:

- Use `window.setTimeout()` with a short delay (e.g., 33ms) for each update
- See [React implementation details](https://github.com/Azure-Samples/azure-search-openai-demo/pull/659)

## Streaming the Full Application Process

In RAG architectures, the pipeline often involves multiple stages (querying databases, generating embeddings, LLM calls). Developers can stream progress events (e.g., search query generated, database results found, answer generation started) to keep users engaged and informed during longer tasks.

Example progress messages:

- *Processing your question...*
- *Generated search query...*
- *Found related results...*
- *Generating answer...*
- *Here's a recipe...*

## Making Streaming Optional

Always provide a non-streaming option in both frontend and backend for:

- Accessibility requirements
- Compatibility with automated test tools (e.g., curl, requests)
- User preference

## Conclusion

Streaming isn't just a nice-to-have for LLM-powered chat apps; it's quickly becoming the baseline for great user experience. Microsoft’s open-source sample apps provide a strong foundation for implementing robust and flexible streaming in your own Azure cloud projects. Experiment with approaches and tailor streaming options to fit your stack and users' needs.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/the-importance-of-streaming-for-llm-powered-chat-applications/ba-p/4459574)
