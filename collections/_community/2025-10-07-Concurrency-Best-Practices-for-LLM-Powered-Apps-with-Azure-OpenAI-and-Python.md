---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/why-your-llm-powered-app-needs-concurrency/ba-p/4459584
title: Concurrency Best Practices for LLM-Powered Apps with Azure OpenAI and Python
author: Pamela_Fox
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-10-07 16:05:02 +00:00
tags:
- AI Application
- API Integration
- Asynchronous
- AsyncIO
- Azure AI Search
- Azure OpenAI Service
- Backend Development
- FastAPI
- Gunicorn
- LLM
- Microsoft
- OpenAI Python SDK
- Python
- Quart
- RAG
- Uvicorn
section_names:
- ai
- azure
- coding
---
Pamela Fox shares actionable best practices for building LLM-powered apps on Azure, emphasizing asynchronous Python frameworks to ensure speed and reliability when integrating Azure OpenAI Service and Azure AI Search.<!--excerpt_end-->

# Concurrency Best Practices for LLM-Powered Apps with Azure OpenAI and Python

**Author: Pamela Fox**

## Introduction

Through her work on the Python advocacy team at Microsoft, Pamela Fox has maintained several open-source AI sample applications, most notably the [RAG chat demo](https://github.com/Azure-Samples/azure-search-openai-demo/). She discusses critical lessons in making LLM-powered apps both fast and reliable—chief among them: using an asynchronous backend framework.

## Why Concurrency Matters for LLM Apps

- LLM applications (e.g., apps powered by Azure OpenAI Service) frequently handle multiple API calls and database queries concurrently.
- Synchronous backend frameworks like Flask can block worker threads during long-running requests (e.g., to OpenAI APIs), reducing throughput and wasting resources.
- Asynchronous frameworks allow the Python runtime to pause coroutines waiting for I/O so other requests can be processed in parallel, maximizing efficiency.

### Example

Running a Flask app on Gunicorn can result in each worker being tied up during slow API calls to Azure OpenAI, forcing serial processing. Switching to an async framework keeps all workers busy by interleaving waiting requests.

#### Diagrams

- Synchronous: Each worker handles one request at a time, waiting during API calls.
- Asynchronous: Workers can process other requests while waiting for I/O.

## Asynchronous Python Backends

Several Python frameworks support asynchronous patterns:

- [Quart](https://quart.palletsprojects.com/): An async version of Flask.
- [FastAPI](https://github.com/pamelafox/chatgpt-backend-fastapi): Async-first, built on Starlette.
- [Litestar](https://docs.litestar.dev/): Batteries-included async framework.
- [Django](https://www.djangoproject.com/): Offers async view support.

Pamela describes the decision-making process for choosing among these frameworks and shares detailed considerations [in a separate blog post](https://blog.pamelafox.org/2024/07/should-you-use-quart-or-fastapi-for-ai.html).

## Porting Flask to Quart: Asynchronous Handlers

Converting from Flask to Quart involves prefixing handlers with `async`, enabling them to return coroutines. This permits concurrent processing of requests.

```python
async def chat_handler():
    request_message = (await request.get_json())["message"]
```

For deployment:

- Continue using **Gunicorn** for production, but run it with the **Uvicorn** worker (ASGI compatible).
- Alternatively, run **Uvicorn** or **Hypercorn** directly.

## Asynchronous API Calls with Azure OpenAI

To fully leverage async frameworks, API calls themselves must be asynchronous. Using the OpenAI Python SDK in async mode:

```python
openai_client = openai.AsyncOpenAI(
    base_url=os.environ["AZURE_OPENAI_ENDPOINT"] + "/openai/v1",
    api_key=token_provider
)

chat_coroutine = await openai_client.chat.completions.create(
    deployment_id=os.environ["AZURE_OPENAI_CHAT_DEPLOYMENT"],
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": request_message}
    ],
    stream=True,
)
```

For Azure services like AI Search, use their async clients:

```python
from azure.identity.aio import DefaultAzureCredential
from azure.search.documents.aio import SearchClient

r = await self.search_client.search(query_text)
```

With all outbound calls as asynchronous coroutines, your app can efficiently handle multiple user sessions without idle worker time.

## Real-World Sample Applications

Here’s a list of open-source, async-enabled sample applications suitable for various tech stacks:

| Repository                                                                                 | App Purpose                 | Backend           | Frontend |
|--------------------------------------------------------------------------------------------|-----------------------------|-------------------|----------|
| [azure-search-openai-demo](https://github.com/Azure-Samples/azure-search-openai-demo)      | RAG with AI Search          | Python + Quart    | React    |
| [rag-postgres-openai-python](https://github.com/Azure-Samples/rag-postgres-openai-python/) | RAG with PostgreSQL         | Python + FastAPI  | React    |
| [openai-chat-app-quickstart](https://github.com/Azure-Samples/openai-chat-app-quickstart)  | Simple chat (OpenAI models) | Python + Quart    | JS       |
| [openai-chat-backend-fastapi](https://github.com/Azure-Samples/openai-chat-backend-fastapi)| Simple chat (OpenAI models) | Python + FastAPI  | JS       |
| [deepseek-python](https://github.com/Azure-Samples/deepseek-python)                        | Chat (AI Foundry models)    | Python + Quart    | JS       |

## Conclusion

To build robust, responsive LLM-powered apps with Azure’s AI services, always leverage asynchronous backend frameworks and async API clients. See the above samples for production-grade architectures.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/why-your-llm-powered-app-needs-concurrency/ba-p/4459584)
