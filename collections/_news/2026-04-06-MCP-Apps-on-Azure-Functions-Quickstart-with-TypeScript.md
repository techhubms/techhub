---
date: 2026-04-06 15:01:58 +00:00
section_names:
- ai
- azure
tags:
- AI
- Azd
- Azure
- Azure Developer CLI
- Azure Functions
- Azure SDK
- Bicep
- Deployment
- HTTPS
- IaC
- JavaScript
- Local Development
- MCP
- MCP Apps
- MCP Inspector
- MCP Resources
- MCP Tools
- News
- OAuth
- Open Meteo API
- Python
- Serverless
- Typescript
- VS Code
- Weather App
author: Swapnil Nagar
title: 'MCP Apps on Azure Functions: Quickstart with TypeScript'
external_url: https://devblogs.microsoft.com/azure-sdk/mcp-apps-on-azure-functions-quickstart-with-typescript/
primary_section: ai
feed_name: Microsoft Azure SDK Blog
---

Swapnil Nagar shows how to build and deploy a Model Context Protocol (MCP) app on Azure Functions using TypeScript, including MCP tools/resources, local testing, and azd+Bicep deployment, demonstrated with a practical weather widget example.<!--excerpt_end-->

# MCP Apps on Azure Functions: Quickstart with TypeScript

Azure Functions can host MCP apps end-to-end: build locally, expose a secure endpoint, and deploy quickly using Azure Developer CLI (`azd`). This guide walks through the approach using a classic “weather app” example.

## What are MCP Apps?

[MCP Apps](https://modelcontextprotocol.io/extensions/apps/overview) let MCP servers return **interactive HTML interfaces** (visualizations, forms, dashboards) that render inside MCP-compatible hosts (for example: Visual Studio Code Copilot, Claude, ChatGPT). See also the [official MCP Apps documentation](https://apps.extensions.modelcontextprotocol.io/api/).

Interactive UI helps when your scenario needs:

- **Interactive data**: clickable maps/charts instead of static lists
- **Complex setup**: one-page forms instead of long back-and-forth prompts
- **Rich media**: embedded viewers for 3D models and documents
- **Live updates**: real-time dashboards without new prompts
- **Workflow management**: multi-step tasks (approvals, navigation, persistent state)

## Hosting MCP Apps on Azure Functions

Azure Functions provides an abstraction for building MCP servers without having to implement the protocol details directly. Hosting an MCP App on Functions provides:

- **MCP tools** (server logic): handle requests, call backend services, return structured data (Functions manages MCP protocol details)
- **MCP resources** (UI payloads): serve interactive HTML, JSON documents, and formatted content
- **Secure HTTPS access**:
  - Function keys authentication
  - Built-in MCP auth with OAuth support: https://learn.microsoft.com/azure/azure-functions/functions-mcp-tutorial?tabs=mcp-extension&pivots=programming-language-python#enable-built-in-server-authorization-and-authentication
- **Easy deployment** with Bicep and `azd` (IaC)
- **Local development**: test/debug before deploying
- **Auto-scaling**: scaling, retries, and monitoring handled by the platform

The weather app is an example use case, not the only one.

## Architecture overview

![Architecture Diagram showing MCP Apps hosted on Azure Functions](https://devblogs.microsoft.com/azure-sdk/wp-content/uploads/sites/58/2026/04/04-01-mcp-apps-on-functions-ts-architecture.png)

## Example: the classic weather app

The sample includes:

- A **GetWeather MCP tool** that fetches weather by location (calls Open-Meteo geocoding and forecast APIs)
- A **Weather Widget MCP resource** that serves interactive HTML/JS (runs in the client; fetches data via the GetWeather tool)
- A TypeScript service layer abstracting API calls and data transformation (runs on the server)
- Local and remote testing flow for MCP clients (MCP Inspector, Visual Studio Code, or custom clients)

### How UI rendering works in MCP Apps

In the sample:

- Azure Functions serves `getWeatherWidget` as a **resource** → returns `weather-app.ts` compiled to HTML/JS
- The client renders the Weather Widget UI
- The user interacts with the widget (or it issues internal requests)
- The widget calls the `getWeather` **tool** → server returns weather data
- The widget renders the weather data client-side

This keeps UI **responsive locally** while pulling **server-side data/logic** as needed.

## Quickstart

Repository:

- https://github.com/Azure-Samples/remote-mcp-functions-typescript

### Run locally

```bash
npm install
npm run build
func start
```

Local endpoint:

```text
http://0.0.0.0:7071/runtime/webhooks/mcp
```

### Deploy to Azure

```bash
azd provision
azd deploy
```

Remote endpoint:

```text
https://<function-app-name>.azurewebsites.net/runtime/webhooks/mcp
```

![Animated demo of MCP Apps architecture and weather widget flow](https://devblogs.microsoft.com/azure-sdk/wp-content/uploads/sites/58/2026/04/04-01-mcp-apps-on-functions-ts-architecture.gif)

## TypeScript snippets

### MCP tool (Get Weather service)

In Azure Functions, define MCP tools with `app.mcpTool()`. `toolName` and `description` are client-facing metadata, `toolProperties` defines arguments, and `handler` points to your request handler.

```typescript
app.mcpTool("getWeather", {
  toolName: "GetWeather",
  description: "Returns current weather for a location via Open-Meteo.",
  toolProperties: {
    location: arg.string().describe("City name to check weather for")
  },
  handler: getWeather,
});
```

### MCP resource trigger (Weather App hook)

Define MCP resources with `app.mcpResource()`. `uri` is the resource identifier clients use, `mimeType` describes the payload, and `handler` returns the content (for example HTML for a widget).

```typescript
app.mcpResource("getWeatherWidget", {
  uri: "ui://weather/index.html",
  resourceName: "Weather Widget",
  description: "Interactive weather display for MCP Apps",
  mimeType: "text/html;profile=mcp-app",
  handler: getWeatherWidget,
});
```

## Sample repos and references

- TypeScript sample: https://github.com/Azure-Samples/remote-mcp-functions-typescript
- Azure Functions MCP bindings (TypeScript): https://learn.microsoft.com/azure/azure-functions/functions-bindings-mcp?pivots=programming-language-typescript
- Java sample: https://github.com/Azure-Samples/remote-mcp-functions-java
- .NET sample: https://github.com/Azure-Samples/remote-mcp-functions-dotnet
- Python sample: https://github.com/Azure-Samples/remote-mcp-functions-python
- MCP Inspector: https://github.com/modelcontextprotocol/inspector

## Final takeaway

MCP Apps are MCP servers that embed interactive UIs in compatible hosts, shifting experiences from purely text chat to functional interfaces.

Azure Functions helps you build, secure, scale, and deploy MCP apps quickly with a serverless model—so you can focus on the business logic rather than protocol and hosting details.


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/mcp-apps-on-azure-functions-quickstart-with-typescript/)

