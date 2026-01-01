---
layout: "post"
title: "Host Your Node.js MCP Server on Azure Functions in 3 Simple Steps"
description: "This guide demonstrates how to deploy a Node.js Model Context Protocol (MCP) server using the official Anthropic MCP SDK on Azure Functions. It covers serverless hosting benefits, step-by-step configuration, testing, automated setup with GitHub Copilot, local development tips, Infrastructure as Code deployment with Azure Developer CLI, and key limitations. Learn how to build scalable, reliable AI agent backends by leveraging Azure's serverless compute platform and modern open protocols."
author: "Yohan Lasorsa"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/blog/host-your-node-js-mcp-server-on-azure-functions-in-3-simple-steps"
viewing_mode: "external"
feed_name: "Microsoft Blog"
feed_url: "https://devblogs.microsoft.com/feed"
date: 2025-12-08 18:00:54 +00:00
permalink: "/2025-12-08-Host-Your-Nodejs-MCP-Server-on-Azure-Functions-in-3-Simple-Steps.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "Anthropic MCP SDK", "Azure", "Azure Developer CLI", "Azure Functions", "Coding", "Custom Handler", "Express", "HTTP Streaming", "IaC", "LangChain.js", "MCP", "News", "Node.js", "Serverless", "TypeScript"]
tags_normalized: ["ai", "anthropic mcp sdk", "azure", "azure developer cli", "azure functions", "coding", "custom handler", "express", "http streaming", "iac", "langchaindotjs", "mcp", "news", "nodedotjs", "serverless", "typescript"]
---

Yohan Lasorsa explains how to deploy a Node.js MCP server to Azure Functions using the official Anthropic MCP SDK, including practical advice for automation, infrastructure setup, and cost management.<!--excerpt_end-->

# Host Your Node.js MCP Server on Azure Functions in 3 Simple Steps

Author: Yohan Lasorsa

Deploying AI agents and MCP (Model Context Protocol) servers to production just got easier thanks to Azure Functions’ serverless hosting capabilities. This tutorial explains how to host your Node.js MCP server with the Anthropic MCP SDK on Azure Functions, enabling scalable, cost-effective deployment with minimal code changes.

## What is MCP?

Model Context Protocol (MCP) is an open standard that lets AI models interact with external tools and data sources securely. Instead of hardcoding tool integrations, you build an MCP server that exposes capabilities—such as database queries or placing orders—so any MCP-compatible AI agent can discover and use them. MCP is model-agnostic and supports major AI models including OpenAI and Anthropic.

## Why use Azure Functions?

Azure Functions provides a serverless compute platform for:

- **Zero infrastructure management**
- **Automatic scaling**
- **Pay-per-use pricing**
- **Built-in monitoring** (Application Insights)
- **Global distribution**

Recent updates enable hosting Node.js servers as custom handlers, making Azure Functions a great fit for MCP servers and similar APIs.

## Prerequisites

- Node.js 22+
- Azure account ([get free credits](https://azure.microsoft.com/free))
- Azure Developer CLI ([install guide](https://aka.ms/azure-dev/install))
- (Optional) GitHub account for Codespaces or Copilot

## Quick Start: Three Simple Steps

1. **Create `host.json` configuration**

    ```json
    {
      "version": "2.0",
      "extensions": { "http": { "routePrefix": "" } },
      "customHandler": {
        "description": {
          "defaultExecutablePath": "node",
          "workingDirectory": "",
          "arguments": ["lib/server.js"]
        },
        "enableForwardingHttpRequest": true,
        "enableHttpProxyingRequest": true
      }
    }
    ```

   Adjust the `arguments` path as needed for your build output.

2. **Port Configuration in Server Code**

    ```typescript
    const PORT = process.env.FUNCTIONS_CUSTOMHANDLER_PORT || process.env.PORT || 3000;
    app.listen(PORT, () => {
      console.log(`MCP server listening on port ${PORT}`);
    });
    ```

   This ensures compatibility with Azure Functions' environment.

3. **Create a `function.json` file in `handler/` directory**

    ```json
    {
      "bindings": [
        {
          "authLevel": "anonymous",
          "type": "httpTrigger",
          "direction": "in",
          "name": "req",
          "methods": ["get", "post", "put", "delete", "patch", "head", "options"],
          "route": "{*route}"
        },
        {
          "type": "http",
          "direction": "out",
          "name": "res"
        }
      ]
    }
    ```

   Routes all HTTP requests to your MCP server handler.

## Automate with GitHub Copilot

You can automate the entire setup using [GitHub Copilot](https://github.com/anthonychu/create-functions-mcp-server)'s prompt helper. This adds all required config files, updates code, and prepares Infrastructure as Code.

## Example Project: Burger MCP Server

- Implements AI-powered burger ordering via a set of MCP tools
- Built on Express using official MCP SDK and exposes endpoints like `get_burgers`, `place_order`.
- Integrates with test and production environments
- [Full sample repo](https://github.com/Azure-Samples/mcp-agent-langchainjs/tree/main/packages/burger-mcp)

### MCP Tool Example

```typescript
import { McpServer } from '@modelcontextprotocol/sdk/server/mcp.js';

export function getMcpServer() {
  const server = new McpServer({
    name: 'burger-mcp',
    version: '1.0.0',
  });

  server.registerTool(
    'get_burgers',
    { description: 'Get a list of all burgers in the menu' },
    async () => {
      const response = await fetch(`${burgerApiUrl}/burgers`);
      const burgers = await response.json();
      return { content: [{ type: 'text', text: JSON.stringify(burgers, null, 2) }] };
    }
  );
  // ...more tools
  return server;
}
```

## Testing Locally

- Use [Codespaces](https://codespaces.new/Azure-Samples/mcp-agent-langchainjs?hide_repo_select=true&ref=main&quickstart=true)
- Clone and run:

    ```sh
    npm install
    npm start
    ```

- Use `@modelcontextprotocol/inspector` for testing the API:

    ```sh
    npx -y @modelcontextprotocol/inspector
    ```

- Connect with GitHub Copilot by updating `.vscode/mcp.json` for real-world testing

## Deployment with Azure Developer CLI

- Define infrastructure in `azure.yaml` and `infra/`
- Deploy with:

    ```sh
    azd auth login
    azd up
    ```

- Choose a region and wait for deployment
- You get a ready-to-use, globally available MCP server endpoint

## Costs

- Free tier: 1 million requests and 400,000 GB-s/month
- Scale down to zero when idle
- Pay only for execution time

## Current Limitations & Tips

- Only HTTP Streaming protocol is supported (stateless servers)
- Stateful protocols like SSE are not supported due to serverless connection model
- Most use cases should migrate to HTTP Streaming for scalability

## Resources

- [Model Context Protocol](https://modelcontextprotocol.io/)
- [Anthropic MCP SDK](https://github.com/modelcontextprotocol/typescript-sdk)
- [Azure Functions documentation](https://learn.microsoft.com/azure/azure-functions/functions-overview)
- [Burger MCP example](https://github.com/Azure-Samples/mcp-agent-langchainjs)
- [Serverless starter template](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-node)

> For community help, join the [Azure AI community on Discord](https://aka.ms/foundry/discord).

---

This guide enables you to rapidly bring AI agent integration scenarios to Azure cloud, reducing operational friction and focusing on delivering value through open protocols and managed serverless technology.

This post appeared first on "Microsoft Blog". [Read the entire article here](https://devblogs.microsoft.com/blog/host-your-node-js-mcp-server-on-azure-functions-in-3-simple-steps)
