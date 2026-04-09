---
date: 2026-04-08 07:07:55 +00:00
author: Evan Mattson
primary_section: ai
feed_name: Microsoft Semantic Kernel Blog
tags:
- AG UI
- Agent Framework
- Agent Handoffs
- Agent Orchestration
- AI
- Approval Modal
- Customer Support Automation
- Demo
- FastAPI
- HandoffBuilder
- Human in The Loop
- Interrupts
- Microsoft Agent Framework
- Multi Agent Systems
- News
- Python
- React
- Server Sent Events
- SSE
- Streaming Events
- Tool Approval
- Vite
- Workflow State
external_url: https://devblogs.microsoft.com/agent-framework/ag-ui-multi-agent-workflow-demo/
section_names:
- ai
title: Building a Real-Time Multi-Agent UI with AG-UI and Microsoft Agent Framework Workflows
---

Evan Mattson walks through building a real-time multi-agent customer support UI by combining Microsoft Agent Framework handoffs with the AG-UI protocol over SSE, including human-in-the-loop tool approvals and user-input interrupts.<!--excerpt_end-->

## Overview

Multi-agent workflows are easy to demo in a terminal, but once you have multiple agents handing off to each other, waiting for approvals, and asking follow-up questions, a basic chat UI becomes hard to trust:

- Which agent is currently active?
- Why is the system waiting?
- What is it about to do on the user’s behalf?

This article builds a concrete solution: a customer support workflow using **Microsoft Agent Framework (MAF)** for orchestration and **AG-UI** (an open protocol) to stream agent execution events to a frontend over **Server-Sent Events (SSE)**.

## What you will build

A customer support workflow with three specialized agents:

- **Triage Agent**: analyzes the request and routes to the right specialist
- **Refund Agent**: looks up order details, gathers context, and submits refund requests
- **Order Agent**: handles replacements and shipping preferences

## Defining the workflow with `HandoffBuilder`

The orchestration layer uses MAF’s `HandoffBuilder`, which lets you declare agents, their tools, and an explicit *handoff topology* (a routing graph enforced by the framework).

### Tools with human approval (HITL)

The key integration point is marking tools with `approval_mode="always_require"`. When an agent invokes one of these tools:

- the workflow pauses
- an interrupt event is emitted
- the frontend renders an approval prompt
- execution resumes only after approval/rejection

```python
from agent_framework import Agent, tool
from agent_framework.orchestrations import HandoffBuilder

@tool(approval_mode="always_require")
def submit_refund(
    refund_description: str,
    amount: str,
    order_id: str,
) -> str:
    """Capture a refund request for manual review before processing."""
    return f"refund recorded for order {order_id} (amount: {amount})"

@tool(approval_mode="always_require")
def submit_replacement(
    order_id: str,
    shipping_preference: str,
    replacement_note: str,
) -> str:
    """Capture a replacement request for manual review before processing."""
    return (
        f"replacement recorded for order {order_id} "
        f"(shipping: {shipping_preference})"
    )

triage = Agent(
    id="triage_agent",
    name="triage_agent",
    instructions="...",
    client=client,
    require_per_service_call_history_persistence=True,
)

refund = Agent(
    id="refund_agent",
    name="refund_agent",
    instructions="...",
    client=client,
    tools=[submit_refund],
    require_per_service_call_history_persistence=True,
)

order = Agent(
    id="order_agent",
    name="order_agent",
    instructions="...",
    client=client,
    tools=[submit_replacement],
    require_per_service_call_history_persistence=True,
)
```

### Declaring the routing graph

Each `add_handoff` call creates a directed edge (with a natural-language description). The framework uses those descriptions to generate handoff tools, grounding routing decisions in the topology.

```python
builder = HandoffBuilder(
    name="ag_ui_handoff_workflow_demo",
    participants=[triage, refund, order],
    termination_condition=termination_condition,
)

(
    builder
    .add_handoff(
        triage,
        [refund],
        description=(
            "Refunds, damaged-item claims, "
            "refund status updates."
        ),
    )
    .add_handoff(
        triage,
        [order],
        description=(
            "Replacement, exchange, "
            "shipping preference changes."
        ),
    )
    .add_handoff(
        refund,
        [order],
        description="Replacement logistics needed after refund.",
    )
    .add_handoff(
        refund,
        [triage],
        description=(
            "Final case closure when refund-only work is complete."
        ),
    )
    .add_handoff(
        order,
        [triage],
        description="After replacement/shipping tasks complete.",
    )
    .add_handoff(
        order,
        [refund],
        description=(
            "User pivots from replacement to refund processing."
        ),
    )
)

workflow = builder.with_start_agent(triage).build()
```

## Connecting MAF to AG-UI (SSE streaming)

AG-UI streams agent execution events over SSE. MAF provides a bridge in `agent_framework.ag_ui` to expose any workflow as an AG-UI-compatible endpoint.

```python
from agent_framework.ag_ui import (
    AgentFrameworkWorkflow,
    add_agent_framework_fastapi_endpoint,
)
from fastapi import FastAPI

app = FastAPI()

demo_workflow = AgentFrameworkWorkflow(
    workflow_factory=lambda _thread_id: create_handoff_workflow(),
    name="ag_ui_handoff_workflow_demo",
)

add_agent_framework_fastapi_endpoint(
    app=app,
    agent=demo_workflow,
    path="/handoff_demo",
)
```

The `workflow_factory` creates a fresh workflow instance per thread to keep per-conversation state isolated. The endpoint streams events such as:

- `RUN_STARTED`
- `STEP_STARTED`
- `TEXT_MESSAGE_*`
- `TOOL_CALL_*`
- `RUN_FINISHED`

## Two types of interrupts

### 1) Tool approval interrupts

Triggered when an agent calls a tool marked with `approval_mode="always_require"`.

- workflow pauses
- frontend shows tool name + arguments
- operator approves or rejects
- workflow resumes

This is aimed at sensitive actions like issuing refunds or submitting replacement orders.

### 2) Information request interrupts

Used when an agent needs more info from the user (order ID, shipping preference, etc.). Under the hood this uses `HandoffAgentUserRequest`.

When no handoff is requested, the workflow emits an interrupt and waits:

```python
from agent_framework.orchestrations import HandoffAgentUserRequest

# Inside the handoff executor, when no handoff is requested:
# The workflow pauses and waits for user input.
await ctx.request_info(
    HandoffAgentUserRequest(agent_response),
    list[Message],
)

# When the user responds, the handler resumes the workflow:
@response_handler
async def handle_response(
    self,
    original_request: HandoffAgentUserRequest,
    response: list[Message],
    ctx: WorkflowContext,
) -> None:
    if not response:
        # Empty response signals termination
        await ctx.yield_output(self._full_conversation)
        return

    await self._broadcast_messages(response, ctx)
    self._cache.extend(response)
    await self._run_agent_and_emit(ctx)
```

Both interrupt types use AG-UI’s `resume.interrupts` mechanism: the frontend sends a resume payload containing the interrupt ID and response value, and the workflow continues from the paused point.

## The frontend experience

A React frontend consumes the SSE stream and renders workflow state in real time, including:

- active agent indicator
- a case snapshot card (order details, refund amount, shipping preferences)
- chat panel with streaming messages
- approval modal for tool calls

![The HITL approval modal showing the submit_refund tool call with refund details and Approve/Reject buttons](https://devblogs.microsoft.com/agent-framework/wp-content/uploads/sites/78/2026/04/ag-ui-handoff-1.webp)

The frontend keeps a queue of pending interrupts so multiple approvals/info-requests can be handled in sequence without losing state.

## Running the demo

Backend: FastAPI. Frontend: Vite + React.

```bash
# Backend (from python/ directory)
uv sync
uv run python samples/05-end-to-end/ag_ui_workflow_handoff/backend/server.py

# Frontend (in a separate terminal)
cd samples/05-end-to-end/ag_ui_workflow_handoff/frontend
npm install
npm run dev
```

Open `http://127.0.0.1:5173` and try: “I need a refund for order 987654.”

## What this demonstrates

- Exposing MAF workflows (including `HandoffBuilder`) as AG-UI backends with minimal glue
- Dynamic, non-linear routing enforced by a declared handoff graph
- Human-in-the-loop approval at the tool-call level
- Thread-scoped state (one workflow instance per conversation)
- Real-time UI updates streaming tokens, tool calls, and state changes

## Learn more

- [AG-UI Protocol specification](https://github.com/ag-ui-protocol/ag-ui)
- [Microsoft Agent Framework documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/agent-framework/)
- [Full sample code on GitHub](https://github.com/microsoft/agent-framework/tree/main/python/samples/05-end-to-end/ag_ui_workflow_handoff)
- [Microsoft Agent Framework discussion board](https://github.com/microsoft/agent-framework/discussions)


[Read the entire article](https://devblogs.microsoft.com/agent-framework/ag-ui-multi-agent-workflow-demo/)

