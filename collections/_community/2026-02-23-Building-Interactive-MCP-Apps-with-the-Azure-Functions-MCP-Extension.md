---
layout: "post"
title: "Building Interactive MCP Apps with the Azure Functions MCP Extension"
description: "This comprehensive community post by lily-ma introduces the new MCP Apps support in the Azure Functions Model Context Protocol (MCP) extension. It explains how developers can use Python, TypeScript, or .NET to create richer, interactive AI agent experiences by connecting MCP tools to resources that provide HTML interfaces. The guide includes example scenarios, code snippets, and secure deployment advice, enabling developers to build MCP Apps for data exploration, configuration wizards, and real-time monitoring with Azure Functions’ serverless capabilities."
author: "lily-ma"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-mcp-apps-with-azure-functions-mcp-extension/ba-p/4496536"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-23 20:13:39 +00:00
permalink: "/2026-02-23-Building-Interactive-MCP-Apps-with-the-Azure-Functions-MCP-Extension.html"
categories: ["AI", "Azure", "Coding"]
tags: [".NET", "Agent Tools", "AI", "Authentication", "Azure", "Azure Functions", "C#", "Coding", "Community", "Entra ID", "Interactive UI", "LLM Integration", "MCP", "MCP Apps", "MCP Extension", "Metadata", "Python", "Resource Trigger", "Serverless", "TypeScript"]
tags_normalized: ["dotnet", "agent tools", "ai", "authentication", "azure", "azure functions", "csharp", "coding", "community", "entra id", "interactive ui", "llm integration", "mcp", "mcp apps", "mcp extension", "metadata", "python", "resource trigger", "serverless", "typescript"]
---

lily-ma explains how to leverage the new MCP Apps support in Azure Functions MCP Extension to build interactive AI agent tools. The post covers fundamentals, example code, scenarios, and security practices.<!--excerpt_end-->

# Building Interactive MCP Apps with Azure Functions MCP Extension

**Author: lily-ma**

## Introduction

Microsoft has announced the release of MCP App support in the Azure Functions Model Context Protocol (MCP) extension. Developers can now build rich, interactive agentic applications using Python, TypeScript, and .NET, leveraging serverless compute and seamless integration with AI agents.

## What Are MCP Apps?

MCP was initially designed to let AI agents interact with data and external tools by exchanging text responses. However, text-based interactions have limitations for usability and complex input scenarios. MCP Apps extend this model by allowing MCP servers to return interactive HTML interfaces rendered directly in the conversation, greatly improving the user experience.

### Example Use Cases

- **Data exploration:** Interactive dashboards for analytics tools so users can filter, drill down, and export data without leaving the chat.
- **Configuration wizards:** Dynamic forms that reveal new options based on user input, streamlining deployment or setup workflows.
- **Real-time monitoring:** Live metrics widgets that update continuously, enabling instant system health checks.

## How It Works: Overview

### Azure Functions as an MCP Server Host

Azure Functions is a strong fit for hosting MCP servers because it offers:

- Built-in authentication (including Entra ID integration)
- Event-driven scaling (from zero to many instances)
- Cost-efficient serverless billing

### MCP App Architecture

Building an MCP App typically involves:

- **Tools:** Executable functions (e.g., querying databases, sending emails) the LLM can invoke.
- **Resources:** Read-only entities (e.g., log files, API docs, HTML UI) that provide data or interfaces to the LLM or user without side effects.

Tools and resources are connected via tool metadata.

## Code Example: Defining Tools and Resources

### 1. Tool Definition with UI Metadata (C# Example)

```csharp
private const string ToolMetadata = @"{ \"ui\": { \"resourceUri\": \"ui://weather/index.html\" } }";

[Function(nameof(GetWeather))]
public async Task<object> GetWeather(
    [McpToolTrigger(nameof(GetWeather), "Returns current weather for a location via Open-Meteo.")]
    [McpMetadata(ToolMetadata)] ToolInvocationContext context,
    [McpToolProperty("location", "City name to check weather for (e.g., Seattle, New York, Miami)")] string location)
{
    var result = await _weatherService.GetCurrentWeatherAsync(location);
    return result;
}
```

### 2. Resource Definition Serving the UI (C# Example)

```csharp
private const string ResourceMetadata = @"{ \"ui\": { \"prefersBorder\": true } }";

[Function(nameof(GetWeatherWidget))]
public string GetWeatherWidget(
    [McpResourceTrigger(
      "ui://weather/index.html",
      "Weather Widget",
      MimeType = "text/html;profile=mcp-app",
      Description = "Interactive weather display for MCP Apps")]
    [McpMetadata(ResourceMetadata)] ResourceInvocationContext context)
{
    var file = Path.Combine(AppContext.BaseDirectory, "app", "dist", "index.html");
    return File.ReadAllText(file);
}
```

### 3. User Flow

1. The user asks: "What’s the weather in Seattle?"
2. The agent invokes the `GetWeather` tool.
3. The tool returns weather data and UI metadata (`ui.resourceUri`).
4. The client retrieves the HTML UI from the specified resource URI.
5. The tool’s result is passed to the interface.
6. The result is presented in an interactive widget.

## Getting Started

Explore official samples:

- [Python quickstart](https://learn.microsoft.com/azure/azure-functions/scenario-mcp-apps?tabs=bash,linux&pivots=programming-language-python)
- [TypeScript quickstart](https://learn.microsoft.com/azure/azure-functions/scenario-mcp-apps?tabs=bash,linux&pivots=programming-language-typescript)
- [.NET quickstart](https://aka.ms/mcp-app-net-qs)

Documentation:

- [Azure Functions MCP extension](https://aka.ms/functions-mcp-extension)
- [MCP Apps Overview](https://modelcontextprotocol.io/extensions/apps/overview)

## Security

To secure MCP Apps, use access keys or integrate with Microsoft Entra for built-in server authorization and authentication. [Learn more about securing MCP Apps](https://learn.microsoft.com/azure/azure-functions/functions-mcp-tutorial?tabs=mcp-extension&pivots=programming-language-python#enable-built-in-server-authorization-and-authentication).

## Conclusion

MCP Apps with Azure Functions enable developers to create rich, interactive, and secure experiences for AI agent tools using popular programming languages and Microsoft’s serverless platform.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-mcp-apps-with-azure-functions-mcp-extension/ba-p/4496536)
