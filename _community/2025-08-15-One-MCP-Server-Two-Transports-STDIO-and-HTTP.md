---
layout: "post"
title: "One MCP Server, Two Transports: STDIO and HTTP"
description: "This post explains how to implement a flexible MCP (Model Context Protocol) server in .NET that supports both STDIO and HTTP transports. The approach reduces code duplication and management overhead by allowing the transport to be selected at runtime via a command-line switch, leveraging the .NET builder pattern."
author: "justinyoo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 08:02:40 +00:00
permalink: "/2025-08-15-One-MCP-Server-Two-Transports-STDIO-and-HTTP.html"
categories: ["Azure", "Coding"]
tags: [".NET", "ASP.NET Core", "Azure", "Builder Pattern", "C#", "Coding", "Command Line Switches", "Community", "Console Applications", "Copilot Studio", "Dependency Injection", "HostApplicationBuilder", "HTTP Transport", "MCP Server", "Microsoft Azure", "Runtime Configuration", "STDIO Transport", "Web Hosting", "WebApplicationBuilder"]
tags_normalized: ["net", "asp dot net core", "azure", "builder pattern", "c", "coding", "command line switches", "community", "console applications", "copilot studio", "dependency injection", "hostapplicationbuilder", "http transport", "mcp server", "microsoft azure", "runtime configuration", "stdio transport", "web hosting", "webapplicationbuilder"]
---

justinyoo details how to build a .NET MCP server capable of handling both STDIO and HTTP transports using the builder pattern and a runtime switch, streamlining server management and flexibility.<!--excerpt_end-->

# One MCP Server, Two Transports: STDIO and HTTP

In this tutorial, justinyoo demonstrates how to build a single MCP (Model Context Protocol) server in .NET that supports both STDIO and HTTP transports, allowing the hosting mode to be switched at runtime with a simple `--http` flag. This reduces the management overhead of maintaining separate codebases or hosts for each transport.

## When to Use MCP Servers

- Most MCP servers run locally (directly or in containers).
- Integration scenarios like [Copilot Studio](https://learn.microsoft.com/microsoft-copilot-studio/agent-extend-action-mcp), or secure environments, often require remote HTTP-based servers.

## Design Goal

Support both STDIO and HTTP with a single server instance, switching modes with a `--http` command-line argument rather than duplicating code.

## Using the .NET Builder Pattern

- Start with either a console app builder or a web app builder, depending on the hosting mode.
- Interface in common: `IHostApplicationBuilder`.

**Console Application:**

```csharp
var builder = Host.CreateApplicationBuilder(args);
```

**Web Application:**

```csharp
var builder = WebApplication.CreateBuilder(args);
```

## Determining Hosting Mode

A command-line switch, `--http`, determines which builder to use:

**Example:**

```bash
dotnet run --project MyMcpServer -- --http
```

**Function to select transport:**

```csharp
public static bool UseStreamableHttp(IDictionary env, string[] args) {
    var useHttp = env.Contains("UseHttp") && bool.TryParse(env["UseHttp"]?.ToString()?.ToLowerInvariant(), out var result) && result;
    if (args.Length == 0) {
        return useHttp;
    }
    useHttp = args.Contains("--http", StringComparer.InvariantCultureIgnoreCase);
    return useHttp;
}
```

**Using the selector:**

```csharp
var useStreamableHttp = UseStreamableHttp(Environment.GetEnvironmentVariables(), args);
IHostApplicationBuilder builder = useStreamableHttp ? WebApplication.CreateBuilder(args) : Host.CreateApplicationBuilder(args);
```

## Configuring the MCP Server

Add dependencies to the builder, import prompts, resources, and tools:

```csharp
var mcpServerBuilder = builder.Services.AddMcpServer()
    .WithPromptsFromAssembly()
    .WithResourcesFromAssembly()
    .WithToolsFromAssembly();
```

Select the transport based on the switch:

```csharp
if (useStreamableHttp) {
    mcpServerBuilder.WithHttpTransport(o => o.Stateless = true);
} else {
    mcpServerBuilder.WithStdioServerTransport();
}
```

## Running the Server

Cast to the appropriate host type and run the server:

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

## Sample Apps and Resources

- [MCP Samples in .NET](https://aka.ms/mcp/dotnet/samples)
- [Let's Learn MCP](https://aka.ms/letslearnmcp)
- [MCP Workshop in .NET](https://aka.ms/mcp-workshop/dotnet)
- [MCP Samples on GitHub](https://github.com/modelcontextprotocol/csharp-sdk/tree/main/samples)
- [MCP for Beginners](https://aka.ms/mcp-for-beginners)

By following this approach, developers can manage a single, versatile MCP server project in .NET, simplifying deployment and integration with various platforms.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915)
