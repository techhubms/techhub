---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-interactive-agent-uis-with-ag-ui-and-microsoft-agent/ba-p/4488249
title: Building Interactive Agent UIs with AG-UI and Microsoft Agent Framework
author: pratikpanda
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-01-29 08:00:00 +00:00
tags:
- .NET
- AG UI
- Agent Architectures
- Agent Observability
- AI
- AI Protocols
- Async Programming
- Azure
- Azure AI Foundry
- Azure OpenAI
- Coding
- Community
- CrewAI
- Event Driven Design
- FastAPI
- LangGraph
- LLM
- Microsoft Agent Framework
- Python
- Real Time UI
- Server Sent Events
- Streaming APIs
- Tool Invocation
section_names:
- ai
- azure
- coding
---
pratikpanda presents a hands-on guide to building interactive UIs for AI agents using AG-UI and Microsoft Agent Framework. This post explains the motivation, protocol design, architecture, and provides working code for integrating Azure OpenAI with Python FastAPI.<!--excerpt_end-->

# Building Interactive Agent UIs with AG-UI and Microsoft Agent Framework

AG-UI is an open, event-based protocol designed to allow AI agents and user interfaces to communicate in real time. In this guide, we demonstrate its use with Microsoft Agent Framework (MAF) using Python, providing step-by-step instructions for building live, observable, and maintainable agent applications.

## Why AG-UI?

Traditional agent applications leave users staring at loading spinners while backend processing happens out of sight. AG-UI solves this by providing:

- **Streaming Updates & Observability:** Real-time feedback on what the agent is doing (tool calling, partial results, errors, etc.).
- **Framework-Agnostic Operation:** The protocol works across Microsoft Agent Framework, LangGraph, CrewAI, and other platforms.
- **Standard Vocabulary:** A shared set of structured events (e.g., TOOL_CALL_START, TEXT_MESSAGE_CONTENT, RUN_FINISHED).
- **Protocol-Managed State:** No need to manually manage conversation history across sessions or clients.

## Key Concepts

- **Server-Sent Events (SSE):** Communication from server to client using standard HTTP.
- **Protocol-managed Threads:** Conversation state is tracked server-side, supporting multi-client scenarios and reliability.
- **Event Types:** The protocol defines standardized event types such as RUN_STARTED, TEXT_MESSAGE_CONTENT, TOOL_CALL_START, TOOL_CALL_RESULT, and RUN_FINISHED, letting UIs react intelligently during agent execution.

## Architecture

The AG-UI integration with Microsoft Agent Framework consists of several layers:

- **FastAPI Endpoint:** Handles HTTP requests, manages SSE connections.
- **AgentFrameworkAgent Wrapper:** Bridges AG-UI protocol with Agent Framework internals.
- **Orchestrators:** Coordinate execution, manage tool calls, handle state transitions.
- **ChatAgent:** The core agent logic and tool configuration.
- **ChatClient:** Connects to language models (Azure OpenAI, OpenAI, etc.).

## Hands-on: Building Your Own AG-UI App

### Prerequisites

- Python 3.10+
- Azure CLI (with 'az login' completed)
- Azure OpenAI endpoint and deployed model ([setup guide](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/create-resource))
- Role: Cognitive Services OpenAI Contributor
- Install dependencies:

  ```bash
  pip install agent-framework-ag-ui --pre
  ```

### Example Server (FastAPI, MAF, Azure OpenAI)

```python
import os
from typing import Annotated
from dotenv import load_dotenv
from fastapi import FastAPI
from pydantic import Field
from agent_framework import ChatAgent, ai_function
from agent_framework.azure import AzureOpenAIChatClient
from agent_framework_ag_ui import add_agent_framework_fastapi_endpoint
from azure.identity import DefaultAzureCredential

load_dotenv()
openai_endpoint = os.getenv("AZURE_OPENAI_ENDPOINT")
model_deployment = os.getenv("AZURE_OPENAI_DEPLOYMENT_NAME")

if not openai_endpoint or not model_deployment:
    raise RuntimeError("Azure OpenAI credentials are missing.")

@ai_function
def get_order_status(order_id: Annotated[str, Field(description="Order ID (e.g., ORD-001)")]) -> dict:
    # Simulated lookup
    orders = {"ORD-001": {"status": "shipped", "tracking": "1Z999AA1", "eta": "Jan 25, 2026"}}
    return orders.get(order_id, {"status": "not_found", "message": "Order not found"})

chat_client = AzureOpenAIChatClient(
    credential=DefaultAzureCredential(), endpoint=openai_endpoint, deployment_name=model_deployment
)
agent = ChatAgent(
    name="CustomerSupportAgent",
    instructions="You are a helpful support agent. Use get_order_status tool as needed.",
    chat_client=chat_client, tools=[get_order_status]
)

app = FastAPI(title="AG-UI Customer Support Server")
add_agent_framework_fastapi_endpoint(app, agent, path="/chat")

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
```

### Example Client

```python
import asyncio
import os
from dotenv import load_dotenv
from agent_framework import ChatAgent, FunctionCallContent, FunctionResultContent
from agent_framework_ag_ui import AGUIChatClient

load_dotenv()
base_url = os.getenv("AGUI_SERVER_URL", "http://localhost:8000/chat")

async def interactive_chat():
    client = AGUIChatClient(endpoint=base_url)
    agent = ChatAgent(chat_client=client)
    thread = agent.get_new_thread()
    print(f"Connected to {base_url}\nType 'exit' to quit.\n")
    try:
        while True:
            msg = input("You: ")
            if msg.lower() in ["exit", "quit"]:
                print("Goodbye!")
                break
            print("Agent: ", end="", flush=True)
            seen_tools = set()
            async for update in agent.run_stream(msg, thread=thread):
                if update.text:
                    print(update.text, end="", flush=True)
                for c in update.contents:
                    if isinstance(c, FunctionCallContent) and c.call_id not in seen_tools:
                        seen_tools.add(c.call_id)
                        print(f"\n[Calling tool: {c.name}]", flush=True)
                    if isinstance(c, FunctionResultContent):
                        rid = f"result_{c.call_id}"
                        if rid not in seen_tools:
                            seen_tools.add(rid)
                            print(f"[Tool result: {c.result}]", flush=True)
            print("\n")
    except KeyboardInterrupt:
        print("\nInterrupted.")

if __name__ == "__main__":
    asyncio.run(interactive_chat())
```

### Key Takeaways

- **Event-based model**: AG-UI emits explicit events for each stage: e.g., RUN_STARTED, TOOL_CALL_START, TOOL_CALL_RESULT, RUN_FINISHED.
- **Observability**: UIs can show current agent activity, error states, multi-tool flows, and progress bars.
- **Reusable UI**: The same UI logic works across different agent backends and frameworks.

## Protocol Resources

- [Getting Started with AG-UI (Microsoft Learn)](https://learn.microsoft.com/en-us/agent-framework/integrations/ag-ui/getting-started?pivots=programming-language-python)
- [AG-UI Protocol Specification](https://docs.ag-ui.com/introduction)
- [Microsoft Agent Framework Documentation](https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview)
- [Live Demos](https://dojo.ag-ui.com/microsoft-agent-framework-dotnet)
- [Azure AI Foundry Docs](https://learn.microsoft.com/azure/ai-foundry/)

## When Should You Use AG-UI?

**Use AG-UI if:**

- You need live agent progress or transparency for users
- You're building new agent UIs and want future-proof, framework-agnostic code
- Multiple frameworks are in play (e.g., MAF, LangGraph)

**Stick with older patterns if:**

- Your current approach is stable and sufficient
- You don’t need observable, tool-rich agents
- Compatibility with legacy code is your main constraint

AG-UI is a forward-looking way to build interactive, maintainable agent user interfaces, particularly in the Microsoft ecosystem. As the protocol matures, expect even broader adoption and richer tooling.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-interactive-agent-uis-with-ag-ui-and-microsoft-agent/ba-p/4488249)
