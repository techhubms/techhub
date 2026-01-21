---
external_url: https://devblogs.microsoft.com/foundry/announcing-model-context-protocol-support-preview-in-azure-ai-foundry-agent-service/
title: Announcing Model Context Protocol Support in Azure AI Foundry Agent Service (Preview)
author: Linda Li
feed_name: Microsoft DevBlog
date: 2025-06-27 22:30:03 +00:00
tags:
- Agent Integration
- AI Agent
- AI Agent Tools
- Anthropic
- Azure AI Foundry
- Generative AI Agents
- JSON RPC
- MCP
- Microsoft Build
- Open By Design
- Python Code Samples
- VS Code
section_names:
- ai
- azure
- coding
---
In this announcement, Linda Li details the preview of Model Context Protocol (MCP) support in Azure AI Foundry Agent Service, highlighting easier AI agent integration, open standards, and practical steps for developers.<!--excerpt_end-->

# Announcing Model Context Protocol Support (Preview) in Azure AI Foundry Agent Service

_By Linda Li_

Generative-AI agents only become useful when they can **do** things—such as querying systems of record, triggering workflows, or accessing specialized knowledge. Traditionally, achieving this integration required custom solutions: hand-rolling Azure Functions, managing OpenAPI specifications, or creating proprietary plugins for each backend system.

## Introducing MCP: Streamlining AI Agent Integration

The **Model Context Protocol (MCP)**, an open, JSON-RPC–based protocol originally proposed by Anthropic, changes this paradigm. MCP allows a “server” to publish _tools_ (i.e., functions) and _resources_ (i.e., context) once. Any compliant “client,” such as your AI agent runtime, can then discover and call these tools and resources automatically—think of it as the "USB-C for AI integrations."

With the preview release, **Azure AI Foundry Agent Service** has become a first-class MCP client. This means you can bring any remote MCP server—whether self-hosted or SaaS—and Azure AI Foundry will import its capabilities in seconds, keep them updated, and seamlessly route calls through the service's enterprise envelope.

> **Model Context Protocol (MCP)** enables:
>
> - Developers, organizations, and service providers to host services and APIs on MCP servers.
> - Easy exposure and connection of tools to MCP-compatible clients (like Foundry Agent Service).
> - Automatic addition and update of actions and knowledge in agents as MCP server functionality evolves.

As a result, the process of building and maintaining powerful agents is significantly streamlined.

## Key Benefits with MCP Support in Foundry Agent Service

- **Easy integration with services and APIs**: Connect both internal and external services/APIs to Foundry Agent Service with MCP, removing the need to write or manage custom functions.
- **Enterprise-ready features**: Access capabilities such as Bring Your Own thread storage and other advanced options through the enterprise features built into Foundry Agent Service.

## Practical Example: Code Samples

The following hands-on Python code sample illustrates the integration process using the Azure AI Foundry SDK.

### Step 1: Import Required Packages

Use the latest versions for [azure-ai-projects](https://pypi.org/project/azure-ai-projects/1.0.0b12/) and [azure-ai-agents](https://pypi.org/project/azure-ai-agents/1.1.0b2/):

```python
import time
import json
from azure.ai.agents.models import MessageTextContent, ListSortOrder, McpTool
from azure.ai.projects import AIProjectClient
from azure.identity import DefaultAzureCredential
```

### Step 2: Create an AI Project Client and Foundry Agent

Assign a unique `server_label` for the MCP server; `server_url` points to the MCP endpoint. `allowed_tools` can be specified as needed.

```python
mcp_tool = McpTool(
    server_label=mcp_server_label,
    server_url=mcp_server_url,
    allowed_tools=[],  # Optional
)

project_client = AIProjectClient(
    endpoint=PROJECT_ENDPOINT,
    credential=DefaultAzureCredential()
)

with project_client:
    agent = project_client.agents.create_agent(
        model=MODEL_DEPLOYMENT_NAME,
        name="my-mcp-agent",
        instructions=(
            "You are a helpful assistant. Use the tools provided to answer the user's "
            "questions. Be sure to cite your sources."
        ),
        tools=mcp_tool.definitions
    )
    print(f"Created agent, agent ID: {agent.id}")
```

### Step 3: Create a Thread, Message, and Run

Within the run, you can pass custom headers and use your `server_label` to map to a specific MCP server.

```python
thread = project_client.agents.threads.create()
print(f"Created thread, thread ID: {thread.id}")

message = project_client.agents.messages.create(
    thread_id=thread.id,
    role="user",
    content="<a question for your MCP server>"
)
print(f"Created message, message ID: {message.id}")
```

To provide headers:

```python
mcp_tool.update_headers("SuperSecret", "123456")
```

To create a run:

```python
run = project_client.agents.runs.create(
    thread_id=thread.id,
    agent_id=agent.id
)
```

### Step 4: Execute the Run and Retrieve a Message

Use `run_step` for details on tool inputs/outputs. Tool calls to an MCP server require approval by default.

```python
while run.status in ["queued", "in_progress", "requires_action"]:
    time.sleep(1)
    run = agents_client.runs.get(thread_id=thread.id, run_id=run.id)

    if run.status == "requires_action" and isinstance(run.required_action, SubmitToolApprovalAction):
        tool_calls = run.required_action.submit_tool_approval.tool_calls
        if not tool_calls:
            print("No tool calls provided - cancelling run")
            agents_client.runs.cancel(thread_id=thread.id, run_id=run.id)
            break

        tool_approvals = []
        for tool_call in tool_calls:
            if isinstance(tool_call, RequiredMcpToolCall):
                try:
                    print(f"Approving tool call: {tool_call}")
                    tool_approvals.append(
                        ToolApproval(
                            tool_call_id=tool_call.id,
                            approve=True,
                            headers=mcp_tool.headers,
                        )
                    )
                except Exception as e:
                    print(f"Error approving tool_call {tool_call.id}: {e}")

        print(f"tool_approvals: {tool_approvals}")
        if tool_approvals:
            agents_client.runs.submit_tool_outputs(
                thread_id=thread.id,
                run_id=run.id,
                tool_approvals=tool_approvals
            )

    print(f"Current run status: {run.status}")

# Retrieve the generated response

messages = agents_client.messages.list(thread_id=thread.id)
print("\nConversation:")
print("-" * 50)
for msg in messages:
    if msg.text_messages:
        last_text = msg.text_messages[-1]
        print(f"{msg.role.upper()}: {last_text.text.value}")
print("-" * 50)
```

### Step 5: Clean Up

```python
project_client.agents.delete_agent(agent.id)
print(f"Deleted agent, agent ID: {agent.id}")
```

## MCP and Open-by-Design AI: Microsoft Build 2025 Announcement

At **Microsoft Build 2025**, Satya Nadella shared Microsoft's vision for an _open-by-design_ AI ecosystem, announcing a partnership with **Anthropic** to make MCP a first-class standard across Windows 11, GitHub, Copilot Studio, and Azure AI Foundry.

Preview support for MCP in Azure AI Foundry Agent Service is a major step, enabling “connect once, integrate anywhere” for cloud-hosted agents. Now, plugging an MCP server into Foundry can be accomplished without custom code.

## Get Started with Azure AI Foundry

- Try [Azure AI Foundry](https://ai.azure.com/) and jump into [Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=TeamsDevApp.vscode-ai-foundry).
- Download the [Azure AI Foundry SDK](https://aka.ms/aifoundrysdk).
- Read the [documentation](https://aka.ms/FoundryAgentMCPDoc) for more details.
- Explore [Azure AI Foundry Learn courses](https://aka.ms/CreateAgenticAISolutions).
- Join the conversation on [GitHub](https://aka.ms/azureaifoundry/forum) or [Discord](https://aka.ms/azureaifoundry/discord).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/foundry/announcing-model-context-protocol-support-preview-in-azure-ai-foundry-agent-service/)
