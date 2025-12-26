---
layout: "post"
title: "How to Host ChatGPT Apps on Azure Functions: Developer Guide"
description: "This post introduces developers to ChatGPT apps, explains how MCP servers underpin these apps, and provides a step-by-step walkthrough for hosting a ChatGPT app using Azure Functions. It also covers code examples in Python, deployment instructions using Azure Developer CLI, UI integration, and testing in ChatGPT's developer mode. Guidance for app directory submission and upcoming authentication topics is included."
author: "lily-ma"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-chatgpt-apps-on-azure-functions/ba-p/4480696"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-12-23 19:14:38 +00:00
permalink: "/community/2025-12-23-How-to-Host-ChatGPT-Apps-on-Azure-Functions-Developer-Guide.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "App Deployment", "Authentication", "Azure", "Azure Developer CLI", "Azure Functions", "ChatGPT Apps", "Cloud Hosting", "Coding", "Community", "Developer Tools", "FastMCP", "MCP Server", "OpenAI", "Python", "REST API", "Serverless Infrastructure", "Streamable HTTP"]
tags_normalized: ["ai", "app deployment", "authentication", "azure", "azure developer cli", "azure functions", "chatgpt apps", "cloud hosting", "coding", "community", "developer tools", "fastmcp", "mcp server", "openai", "python", "rest api", "serverless infrastructure", "streamable http"]
---

Lily Ma presents a comprehensive developer guide for building and hosting ChatGPT apps on Azure Functions, covering architecture, integration, and deployment tips.<!--excerpt_end-->

# Host ChatGPT Apps on Azure Functions: Developer Guide

This guide explains how developers can build, host, and test ChatGPT apps using Azure Functions—a scalable, cloud-based serverless platform from Microsoft.

## Overview of ChatGPT Apps

OpenAI's ChatGPT apps enable users to interact with conversational bots that take actions, fetch data, and present custom UI elements directly in chat. For developers, these apps can reach millions of users instantly, without needing custom frontends.

### Key Benefits

- **Native Integration with ChatGPT**: Users launch apps via mentions or receive contextual app suggestions.
- **Contextual Actions**: Apps do more than chat—they can trigger data fetches, execute tasks, and present custom UI.
- **Massive Distribution**: Apps submitted to the ChatGPT Directory get instant visibility to ChatGPT's large user base.

## Architecture: MCP Servers as the Foundation

ChatGPT apps operate as remote MCP (Message Control Protocol) servers, exposing tools that:

- Specify interactive UI elements via metadata.
- Return results rendered in the chat as iFrames, supporting dynamic actions (buttons, maps, etc).

### Example: Weather App Workflow

- Users chat with `@WeatherApp` and request weather info.
- ChatGPT calls the MCP server tool (`get_current_weather`) via streamable HTTP transport.
- The server returns both readable text and machine data for UI rendering.
- The specified output template embeds results in a customizable widget.

## Hosting MCP Servers using Azure Functions

Azure Functions offer:

- **Scalable, burst-handling infrastructure** (Flex Consumption plan)
- **Built-in authentication and authorization** for secure hosting
- **Serverless billing** (pay-per-use)

This makes Azure Functions ideal for hosting MCP servers powering ChatGPT apps.

## Prerequisites

To follow the deployment steps, ensure you have:

- An [Azure subscription](https://azure.microsoft.com/pricing/purchase-options/azure-account?cid=msft_learn)
- [Azure Developer CLI](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd)
- [ChatGPT Plus subscription](https://chatgpt.com/pricing/) (for developer mode testing)

## Deploying MCP Server to Azure Functions

1. **Clone the MCP server repository**:

   ```bash
   git clone https://github.com/Azure-Samples/chatgpt-app-azure-function-mcp
   ```

2. **Authenticate with Azure**:

   ```bash
   azd auth login
   ```

3. **Deploy the application**:

   ```bash
   azd up
   ```

   - Enter environment name, select subscription, choose location
   - After deployment, copy your app's URL: `https://<your-app>.azurewebsites.net`

## Code Walkthrough (Python FastMCP Example)

Key components in `main.py`:

- `get_weather_widget`: UI resource for rendering weather data in ChatGPT
- `get_current_weather`: Tool fetching weather using Open-Meteo API
- Returns both content and structured data for widget
- Metadata `outputTemplate` links UI with data

```python
@resource("ui://widget/current-weather.html", mime_type="text/html+skybridge")
def get_weather_widget() -> str:
    """Interactive HTML widget to display current weather data in ChatGPT."""
    # ...code...

@mcp.tool(
    annotations={"title": "Get Current Weather", "readOnlyHint": True, "openWorldHint": True},
    meta={
        "openai/outputTemplate": "ui://widget/current-weather.html",
        "openai/toolInvocation/invoking": "Fetching weather data",
        "openai/toolInvocation/invoked": "Weather data retrieved"
    }
)
def get_current_weather(latitude: float, longitude: float) -> ToolResult:
    # ...fetch weather...
    return ToolResult(content=content_text, structured_content=data)
```

On ChatGPT invocation, the tool returns data for the widget, which is then rendered in an iFrame in the chat.

## Testing in ChatGPT Developer Mode

1. Enable Developer mode in ChatGPT (Settings → Connectors → Advanced)
2. Add a new source: click **+ → More → Add sources**
3. Fill out the form: Name (`WeatherApp`), MCP Server URL (`https://<your-app>.azurewebsites.net/mcp`), select **No Auth**
4. After setup, invoke the app by mentioning `@WeatherApp` in a chat (e.g., “@WeatherApp what’s the temperature in NYC today?”)

## Submit Your App to ChatGPT App Directory

- Review [submission guidelines](https://developers.openai.com/apps-sdk/app-submission-guidelines)
- Submit the app for public listing: [App Directory](https://chatgpt.com/apps)

## What’s Next

This post covered app hosting and integration. The next post will explain authentication and authorization for Azure-hosted MCP servers. Azure Functions MCP extension support for MCP Resources is coming soon.

## Final Thoughts

ChatGPT apps bring rich interactivity and distribution to developers through the ChatGPT client. Hosting with Azure Functions provides a reliable, scalable backend for these new app experiences.

## Resources and Further Reading

- [Remote MCP servers on Azure Functions](https://learn.microsoft.com/azure/azure-functions/self-hosted-mcp-servers?pivots=programming-language-python)
- [Sample MCP server repo](https://github.com/Azure-Samples/chatgpt-app-azure-function-mcp)
- [Azure Developer CLI](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd)
- [ChatGPT App Submission Guidelines](https://developers.openai.com/apps-sdk/app-submission-guidelines)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-chatgpt-apps-on-azure-functions/ba-p/4480696)
