---
tags:
- .NET
- .NET 9
- AI
- ASP.NET Core
- Azd Up
- Azure
- Azure App Service
- Azure Application Insights
- Azure Developer CLI
- Azure Functions
- Bicep
- C#
- Community
- Deployment Slots
- DevOps
- Easy Auth
- Entra ID
- GitHub Copilot
- GitHub Copilot Chat
- Iframe Sandbox
- Interactive UI
- MCP
- MCP Apps
- MCP Server
- Open Meteo API
- Postmessage
- Security
- Stateless HTTP Transport
- Streamable HTTP
- TypeScript
- Vite
- VS Code
date: 2026-04-08 18:10:03 +00:00
section_names:
- ai
- azure
- devops
- dotnet
- github-copilot
- security
primary_section: github-copilot
feed_name: Microsoft Tech Community
title: Build and Host MCP Apps on Azure App Service
author: jordanselig
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-and-host-mcp-apps-on-azure-app-service/ba-p/4509705
---

jordanselig walks through building an MCP App (a tool plus a UI resource) with ASP.NET Core, rendering an interactive weather widget inside chat clients like VS Code Copilot, and deploying the MCP server to Azure App Service using azd and Bicep.<!--excerpt_end-->

## Overview

MCP Apps (an extension to the Model Context Protocol) let AI tools return **interactive UIs**—charts, forms, dashboards—that render directly inside MCP-capable chat clients (for example, VS Code Copilot, Claude Desktop, and ChatGPT).

This guide builds a **weather widget MCP App** using **ASP.NET Core** for the MCP server and a **Vite + TypeScript** frontend for the UI, then deploys it to **Azure App Service** using a single command.

## What are MCP Apps?

MCP Apps extend MCP by pairing:

- **Tools**: callable functions exposed to the AI client
- **UI resources**: web UI payloads (HTML/JS/CSS) that the host renders

How it works:

1. A tool declares a `_meta.ui.resourceUri` in its metadata
2. When the tool is invoked, the MCP host fetches that UI resource
3. The UI renders in a sandboxed iframe inside the chat client

Key point: **MCP Apps are just web apps** served via MCP.

## Why host MCP Apps on Azure App Service?

App Service is positioned as a good fit for MCP Apps because it provides:

- **Always On** (no cold starts)
- **Easy Auth** (secure endpoints with **Microsoft Entra ID** without writing code)
- **Custom domains + TLS**
- **Deployment slots** (staged/canary rollouts)
- **Sidecars** (run supporting components alongside your server)
- **Application Insights** telemetry (tool/UI invocation, latency, errors)

The sample focuses on the core hosting pattern first; the production features are optional add-ons.

## Functions vs App Service for MCP Apps

Both can host MCP Apps; the trade-off described is:

- **Azure Functions**: serverless scaling (scale to zero), potential cold starts (depending on plan)
- **Azure App Service**: always-on hosting, useful when you want persistent state / larger web app integration and platform features like Easy Auth and deployment slots

Reference quickstart for Functions:

- Azure Functions MCP Apps quickstart: https://learn.microsoft.com/en-us/azure/azure-functions/scenario-mcp-apps

## Sample project structure

```text
app-service-mcp-app-sample/
├── src/
│   ├── Program.cs              # MCP server setup
│   ├── WeatherTool.cs          # Weather tool with UI metadata
│   ├── WeatherUIResource.cs    # MCP resource serving the UI
│   ├── WeatherService.cs       # Open-Meteo API integration
│   └── app/                    # Vite frontend (weather widget)
│       └── src/
│           └── weather-app.ts  # MCP Apps SDK integration
├── .vscode/
│   └── mcp.json                # VS Code MCP server config
├── azure.yaml                  # Azure Developer CLI config
└── infra/                      # Bicep infrastructure
```

## MCP server implementation (ASP.NET Core)

### Program.cs — MCP server setup

Registers the weather service, configures MCP with HTTP transport, and maps the MCP endpoint:

```csharp
using ModelContextProtocol;

var builder = WebApplication.CreateBuilder(args);

// Register WeatherService
builder.Services.AddSingleton<WeatherService>(sp =>
    new WeatherService(WeatherService.CreateDefaultClient()));

// Add MCP Server with HTTP transport, tools, and resources
builder.Services.AddMcpServer()
    .WithHttpTransport(t => t.Stateless = true)
    .WithTools<WeatherTool>()
    .WithResources<WeatherUIResource>();

var app = builder.Build();

// Map MCP endpoints (no auth required for this sample)
app.MapMcp("/mcp").AllowAnonymous();

app.Run();
```

Notes from the post:

- `AddMcpServer()` sets up the MCP protocol handler
- `WithHttpTransport(...Stateless = true)` enables Streamable HTTP with stateless mode
- `WithTools<WeatherTool>()` registers the tool
- `WithResources<WeatherUIResource>()` registers the UI resource handler
- `MapMcp("/mcp")` exposes the MCP endpoint at `/mcp`

### WeatherTool.cs — tool with UI metadata

Declares a tool that returns weather data, and adds UI metadata via `[McpMeta]`:

```csharp
using System.ComponentModel;
using ModelContextProtocol.Server;

[McpServerToolType]
public class WeatherTool
{
    private readonly WeatherService _weatherService;

    public WeatherTool(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [McpServerTool]
    [Description("Get current weather for a location via Open-Meteo. Returns weather data that displays in an interactive widget.")]
    [McpMeta("ui", JsonValue = """{"resourceUri": "ui://weather/index.html"}""")]
    public async Task<object> GetWeather(
        [Description("City name to check weather for (e.g., Seattle, New York, Miami)")] string location)
    {
        var result = await _weatherService.GetCurrentWeatherAsync(location);
        return result;
    }
}
```

The key behavior:

- `[McpMeta("ui", ...)]` adds `_meta.ui.resourceUri`
- `resourceUri` points to `ui://weather/index.html`
- When invoked, the host fetches that resource and renders it (sandboxed iframe) next to the tool output

### WeatherUIResource.cs — UI resource handler

Serves the bundled HTML using the `ui://` URI scheme and the MCP Apps MIME type `text/html;profile=mcp-app`:

```csharp
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

[McpServerResourceType]
public class WeatherUIResource
{
    [McpServerResource(
        UriTemplate = "ui://weather/index.html",
        Name = "weather_ui",
        MimeType = "text/html;profile=mcp-app")]
    public static ResourceContents GetWeatherUI()
    {
        var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "app",
            "dist",
            "index.html");

        var html = File.ReadAllText(filePath);

        return new TextResourceContents
        {
            Uri = "ui://weather/index.html",
            MimeType = "text/html;profile=mcp-app",
            Text = html
        };
    }
}
```

### WeatherService.cs — Open-Meteo API integration

The service is described as standard HTTP client code that:

- geocodes a city name
- fetches current weather observations

API used:

- Open-Meteo: https://open-meteo.com/

## UI implementation (Vite + TypeScript)

The frontend uses the MCP Apps SDK `@modelcontextprotocol/ext-apps`:

```typescript
import { App } from "@modelcontextprotocol/ext-apps";

const app = new App({ name: "Weather Widget", version: "1.0.0" });

// Handle tool results from the server
app.ontoolresult = (params) => {
  const data = parseToolResultContent(params.content);
  if (data) render(data);
};

// Adapt to host theme (light/dark)
app.onhostcontextchanged = (ctx) => {
  if (ctx.theme) applyTheme(ctx.theme);
};

await app.connect();
```

Packaging notes:

- Bundled into a **single `index.html`** with Vite
- Uses `vite-plugin-singlefile` to inline JS/CSS
- This makes it easy to serve as one MCP UI resource

## Run locally

Prereqs:

- .NET 9 SDK: https://dotnet.microsoft.com/download/dotnet/9.0
- Node.js 18+: https://nodejs.org/

Commands:

```bash
# Clone the repo
git clone https://github.com/seligj95/app-service-mcp-app-sample.git
cd app-service-mcp-app-sample

# Build the frontend
cd src/app
npm install
npm run build

# Run the MCP server
cd ..
dotnet run
```

The server runs at `http://localhost:5000`.

### Connect from VS Code Copilot

Create/use the provided `.vscode/mcp.json` pointing at your local server:

```json
{
  "servers": {
    "local-mcp-appservice": {
      "type": "http",
      "url": "http://localhost:5000/mcp"
    }
  }
}
```

Then:

1. Open GitHub Copilot Chat in VS Code
2. Ask: **“What’s the weather in Seattle?”**

Copilot invokes `GetWeather` and the weather widget renders inline.

## Deploy to Azure (App Service)

The repo includes:

- `azure.yaml` for Azure Developer CLI
- Bicep templates under `infra/`

Deploy with:

```bash
cd app-service-mcp-app-sample
azd auth login
azd up
```

What `azd up` does (as described):

1. Provisions an App Service plan and web app
2. Builds the .NET app and Vite frontend
3. Deploys to App Service
4. Outputs the public MCP endpoint URL

Update `.vscode/mcp.json` to point to the remote server (example URL):

```json
{
  "servers": {
    "remote-weather-app": {
      "type": "http",
      "url": "https://app-abc123.azurewebsites.net/mcp"
    }
  }
}
```

## Next steps

- MCP Apps spec overview: https://modelcontextprotocol.io/extensions/apps/overview
- ext-apps examples: https://github.com/modelcontextprotocol/ext-apps/tree/main/examples
- Azure Functions quickstart: https://learn.microsoft.com/en-us/azure/azure-functions/scenario-mcp-apps
- Hosting remote MCP servers in App Service: https://learn.microsoft.com/en-us/azure/app-service/scenario-ai-model-context-protocol-server?tabs=dotnet
- Sample repo: https://github.com/seligj95/app-service-mcp-app-sample

Published: Apr 08, 2026

Version: 1.0


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-and-host-mcp-apps-on-azure-app-service/ba-p/4509705)

