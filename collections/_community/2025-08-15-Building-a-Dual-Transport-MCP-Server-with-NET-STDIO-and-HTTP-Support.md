---
layout: "post"
title: "Building a Dual-Transport MCP Server with .NET: STDIO and HTTP Support"
description: "This tutorial by justinyoo demonstrates how to design a single Model Context Protocol (MCP) server in .NET that supports both STDIO (console) and HTTP transports. The solution uses the .NET builder pattern, leveraging command-line arguments or environment variables to determine the hosting mode at runtime. The guide walks through using shared logic, minimizing code duplication, and applying conditional setup to create a versatile, manageable server deployment."
author: "justinyoo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-15 08:02:40 +00:00
permalink: "/community/2025-08-15-Building-a-Dual-Transport-MCP-Server-with-NET-STDIO-and-HTTP-Support.html"
categories: ["Azure", "Coding"]
tags: [".NET", ".NET Builder Pattern", "ASP.NET", "Azure", "C#", "Coding", "Command Line Arguments", "Community", "Copilot Studio", "Dependency Injection", "Environment Variables", "Host", "HostApplicationBuilder", "HTTP Transport", "IHost", "MCP", "Sample Applications", "Server Deployment", "STDIO Transport", "WebApplication", "WebApplicationBuilder"]
tags_normalized: ["dotnet", "dotnet builder pattern", "aspdotnet", "azure", "csharp", "coding", "command line arguments", "community", "copilot studio", "dependency injection", "environment variables", "host", "hostapplicationbuilder", "http transport", "ihost", "mcp", "sample applications", "server deployment", "stdio transport", "webapplication", "webapplicationbuilder"]
---

justinyoo explains how to implement a .NET MCP server with runtime-selectable STDIO and HTTP transports, reducing management overhead and supporting integration with Copilot Studio and other platforms.<!--excerpt_end-->

# Building a Dual-Transport MCP Server with .NET: STDIO and HTTP Support

Author: justinyoo

This guide will show you how to build a single Model Context Protocol (MCP) server that can operate using either STDIO (console) or HTTP transports, selectable with a simple `--http` command-line switch. The approach leverages the .NET builder pattern to avoid code duplication and simplify deployment, supporting integration scenarios such as with Copilot Studio or enterprise solutions where HTTP may be required.

## Why Dual-Transport MCP Servers?

- **Flexibility**: Run MCP servers locally (STDIO) or across networks (HTTP) without modifying core logic.
- **Maintainability**: Minimize code duplication by sharing core logic and abstracting transport selection.
- **Integration**: Easily integrate with tooling like Copilot Studio or enable secure, remote access via HTTP.

## Using the .NET Builder Pattern

There are two primary .NET builder interfaces:

- Console apps use:

  ```csharp
  var builder = Host.CreateApplicationBuilder(args);
  ```

- ASP.NET web apps use:

  ```csharp
  var builder = WebApplication.CreateBuilder(args);
  ```

Both result in a builder implementing `IHostApplicationBuilder`, giving a common ground for adding services and logic prior to selecting the hosting mode.

## Mode Selection at Runtime

Determine the transport mode via argument or environment variable:

```bash
dotnet run --project MyMcpServer -- --http
```

Check for `--http` in the arguments or `UseHttp` in environment variables:

```csharp
public static bool UseStreamableHttp(IDictionary env, string[] args) {
    var useHttp = env.Contains("UseHttp") && bool.TryParse(env["UseHttp"]?.ToString()?.ToLowerInvariant(), out var result) && result;
    if (args.Length == 0) return useHttp;
    useHttp = args.Contains("--http", StringComparer.InvariantCultureIgnoreCase);
    return useHttp;
}
```

Use this selector to choose the builder:

```csharp
IHostApplicationBuilder builder = useStreamableHttp
    ? WebApplication.CreateBuilder(args)
    : Host.CreateApplicationBuilder(args);
```

## Registering the MCP Server

Add your MCP server and related services to the DI container:

```csharp
var mcpServerBuilder = builder.Services.AddMcpServer()
    .WithPromptsFromAssembly()
    .WithResourcesFromAssembly()
    .WithToolsFromAssembly();
```

Select the correct transport based on the runtime check:

```csharp
if (useStreamableHttp) {
    mcpServerBuilder.WithHttpTransport(o => o.Stateless = true);
} else {
    mcpServerBuilder.WithStdioServerTransport();
}
```

## Bootstrapping and Running the Server

Depending on the hosting mode, cast and build appropriately:

```csharp
IHost app;
if (useStreamableHttp) {
    var webApp = (builder as WebApplicationBuilder)!.Build();
    webApp.UseHttpsRedirection();
    webApp.MapMcp("/mcp");
    app = webApp;
} else {
    var consoleApp = (builder as HostApplicationBuilder)!.Build();
    app = consoleApp;
}
await app.RunAsync();
```

## Sample Code and Additional Resources

- [MCP Samples in .NET](https://aka.ms/mcp/dotnet/samples)
- [Let's Learn MCP](https://aka.ms/letslearnmcp)
- [MCP Workshop in .NET](https://aka.ms/mcp-workshop/dotnet)
- [MCP for Beginners](https://aka.ms/mcp-for-beginners)
- [Copilot Studio integration](https://learn.microsoft.com/microsoft-copilot-studio/agent-extend-action-mcp)

This technique enables efficient and maintainable MCP server deployments in .NET for both local development and broader enterprise integration scenarios.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915)
