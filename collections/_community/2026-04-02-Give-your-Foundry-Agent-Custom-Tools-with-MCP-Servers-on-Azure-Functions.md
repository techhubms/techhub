---
tags:
- Agent Builder Playground
- Agentic AI
- AI
- Authentication
- Authorization
- Azure
- Azure AI Foundry
- Azure Functions
- Azure Functions MCP Extension
- Azure Samples
- Community
- Endpoint URL
- Foundry Agents
- Identity Passthrough
- Managed Identity
- MCP
- MCP Tools
- Microsoft Entra ID
- OAuth
- Security
- Serverless
- Tool Calling
author: lily-ma
date: 2026-04-02 21:50:08 +00:00
primary_section: ai
section_names:
- ai
- azure
- security
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/give-your-foundry-agent-custom-tools-with-mcp-servers-on-azure/ba-p/4507828
feed_name: Microsoft Tech Community
title: Give your Foundry Agent Custom Tools with MCP Servers on Azure Functions
---

lily-ma explains how to connect an MCP server hosted on Azure Functions to an Azure AI Foundry agent, covering why you’d do it, the main authentication options (keys, Entra ID/managed identity, OAuth passthrough), and the high-level steps to register the MCP endpoint as a tool and test tool-calling in the agent playground.<!--excerpt_end-->

## Connect your MCP server on Azure Functions to a Foundry agent

Azure Functions is positioned here as a host for remote MCP servers (scalable infra, built-in auth, serverless billing). The article’s main goal is to show how to take an existing MCP server and make it usable by **Azure AI Foundry agents** so an agent can discover and call your tools.

## Why connect MCP servers to Foundry agents?

- If you already built an MCP server for clients like VS Code, Visual Studio, Cursor, or other MCP clients, you can **reuse the same server** with a Foundry agent.
- This lets an enterprise agent call your custom tools (examples given):
  - Query a database
  - Call an API
  - Run business logic
- The agent “reasons, plans, and takes actions” by deciding when to call tools and using the results in its response (agentic AI tool-use pattern).

## Prerequisites

1. **An MCP server deployed to Azure Functions**.
   - Sample implementations:
     - Python: https://github.com/Azure-Samples/remote-mcp-functions-python
     - TypeScript: https://github.com/Azure-Samples/remote-mcp-functions-typescript
     - .NET: https://github.com/Azure-Samples/remote-mcp-functions-dotnet

2. **An Azure AI Foundry project with a deployed model** and a **Foundry agent**.
   - Foundry resources quickstart: https://learn.microsoft.com/en-us/azure/foundry/tutorials/quickstart-create-foundry-resources?view=foundry&tabs=portal
   - Foundry agent quickstart: https://learn.microsoft.com/en-us/azure/foundry/quickstarts/get-started-code?view=foundry&tabs=portal

## Authentication options

The post outlines several choices depending on environment and requirements:

| Method | When to use |
| --- | --- |
| Key-based | Development, or when Entra auth isn’t required |
| Microsoft Entra | Production, using project managed identity |
| OAuth identity passthrough | Production, when each user must authenticate individually |
| Unauthenticated | Development only, or tools that access only public information |

For detailed, step-by-step setup per auth method, the post links to:
https://learn.microsoft.com/en-us/azure/azure-functions/functions-mcp-foundry-tools?tabs=entra%2Cmcp-extension%2Cfoundry

## Connect your MCP server to your Foundry agent (high-level)

1. **Enable built-in MCP authentication** in Azure Functions.
   - Key-based auth is described as the default when deploying a server to Azure Functions.
   - You may need to disable key-based auth and enable built-in MCP auth.
   - Reference: https://learn.microsoft.com/en-us/azure/azure-functions/functions-mcp-tutorial?tabs=mcp-extension&pivots=programming-language-python#enable-built-in-server-authorization-and-authentication

2. **Get your MCP server endpoint URL**.
   - For MCP extension-based servers:

```text
https://<FUNCTION_APP_NAME>.azurewebsites.net/runtime/webhooks/mcp
```

3. **Get credentials** for your chosen authentication method.
   - Examples mentioned: managed identity configuration, OAuth credentials.

4. **Add the MCP server as a tool** in the Foundry portal.
   - Navigate to your agent.
   - Add a new MCP tool.
   - Provide the MCP endpoint URL and credentials.

5. **Test tool calling**.
   - Use the Agent Builder playground.
   - Send a prompt that triggers one of your MCP tools.

## Closing thoughts

The post’s core takeaway is composability: build an MCP server once and reuse it across different clients, including Foundry agents. It also highlights Azure Functions as a way to host MCP servers “at scale and with security.”

## What’s next

A follow-up post will go deeper into additional MCP topics and new MCP features and developments in Azure Functions.


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/give-your-foundry-agent-custom-tools-with-mcp-servers-on-azure/ba-p/4507828)

