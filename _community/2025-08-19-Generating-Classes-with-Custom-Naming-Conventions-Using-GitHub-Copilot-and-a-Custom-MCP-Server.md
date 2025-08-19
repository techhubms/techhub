---
layout: "post"
title: "Generating Classes with Custom Naming Conventions Using GitHub Copilot and a Custom MCP Server"
description: "This community post by daisami provides a step-by-step guide for building a custom Model Context Protocol (MCP) server with ASP.NET Core and integrating it with GitHub Copilot Agent. It covers how to set up MCP server features to enforce business-specific naming conventions, explains the relevant C# code implementation, and details configuration for enterprise environments, including secure Azure deployment. Practical insights include using Copilot Agent tools, working with config files, and how this enables developer productivity and compliance in .NET projects."
author: "daisami"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/generating-classes-with-custom-naming-conventions-using-github/ba-p/4444837"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-19 16:26:20 +00:00
permalink: "/2025-08-19-Generating-Classes-with-Custom-Naming-Conventions-Using-GitHub-Copilot-and-a-Custom-MCP-Server.html"
categories: ["AI", "Azure", "Coding", "GitHub Copilot"]
tags: [".NET", "AI", "ASP.NET Core", "Azure", "Azure App Service", "Business Logic", "C#", "Class Generation", "Coding", "Community", "Copilot Agent", "Dependency Injection", "Enterprise Compliance", "ExpressRoute", "GitHub Copilot", "GitHub Spark", "MCP Server", "Model Context Protocol", "Naming Convention", "NuGet", "Virtual Network", "Visual Studio", "Visual Studio Code", "Workspace Settings"]
tags_normalized: ["net", "ai", "asp dot net core", "azure", "azure app service", "business logic", "c", "class generation", "coding", "community", "copilot agent", "dependency injection", "enterprise compliance", "expressroute", "github copilot", "github spark", "mcp server", "model context protocol", "naming convention", "nuget", "virtual network", "visual studio", "visual studio code", "workspace settings"]
---

daisami explains how to set up a custom MCP server with ASP.NET Core and integrate it with GitHub Copilot Agent, enabling developers to automate naming conventions and customize agent behavior for enterprise .NET projects.<!--excerpt_end-->

# Generating Classes with Custom Naming Conventions Using GitHub Copilot and a Custom MCP Server

GitHub Spark and GitHub Copilot are powerful developer tools that boost productivity, especially when extended to enterprise environments. Many organizations require development support that meets compliance standards or internal rules, which default Copilot capabilities do not fully address. This guide by daisami demonstrates how to build a custom MCP (Model Context Protocol) server in ASP.NET Core, integrate it with the Copilot Agent, and automate naming conventions for your .NET projects.

## Why Use a Custom MCP Server?

- **Custom compliance enforcement:** Standard models, like GPT-4o, are selectable in Copilot but cannot be fine-tuned at present. Enterprises often need tailored code generation to maintain naming standards and regulatory alignment.
- **Agent integration:** By connecting Copilot Agent to your MCP server, you can inject business logic and custom rules to automate routine tasks.

## Architecture Overview

- Build an MCP server using ASP.NET Core Web API in Visual Studio.
- Host the MCP server on a private endpoint (e.g., Azure Virtual Network) with secure access using ExpressRoute in production.
- Use dependency injection to register custom tools for Copilot integration.

## Building the MCP Server: Key Steps

1. **Set up a new ASP.NET Core Web API project.**
2. **Install necessary NuGet packages:**
   - ModelContextProtocol
   - ModelContextProtocol.AspNetCore (include preview versions if needed)
3. **Configure the server (Program.cs):**

```csharp
using MCPServerLab01.Tools;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options => {
    options.ListenAnyIP(8888); // Adjust port as necessary
});
builder.Logging.AddConsole(opts => {
    opts.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<NamingConventionManagerTool>();
var app = builder.Build();
app.MapMcp();
app.Run();
```

1. **Create and decorate the NamingConventionManagerTool class:**
   - Provides endpoints for naming rule retrieval, class naming based on business logic, and business category determination.

```csharp
[McpServerToolType]
[Description()]
public class NamingConventionManagerTool {
    static int _counter = 0;

    [McpServerTool, Description("Provides Normalian Project rules that must be followed.")]
    public string GetNamingRules() { ... }
    [McpServerTool, Description("Retrieves naming convention for a category.")]
    public ClassNamingConvention GenerateClassNamingConvention(BusinessCategory businessCategory) { ... }
    [McpServerTool, Description("Determines category from class name.")]
    public BusinessCategory DetermineBusinessCategory(string className) { ... }
}
```

1. **Run and note your endpoint:** Start the project and reference the displayed HTTP endpoint.

## Connecting to Copilot Agent

- In Visual Studio Code, select Agent mode for Copilot, click the wrench, and add the MCP server endpoint (e.g., http://localhost:8888/sse).
- Configure in workspace or user settings; for team sharing use workspace settings.
- VS Code will save `.vscode/mcp.json` like:

```json
{
  "servers": {
    "mine-mcp-server": { "type": "sse", "url": "http://localhost:8888/sse" }
  },
  "inputs": []
}
```

- Start the server and access available tools directly from the Copilot Agent window.

## Using Copilot Agent with Your MCP Server

- Make requests in your .NET console app project through Copilot Agent to enforce naming rules.
- Agent consults the MCP tooling for naming decision logic, class generation, and categorization tasks.
- Folders and classes are automatically generated following your set conventions.

## Enterprise Deployment Considerations

- Run the MCP server on Azure with private endpoints to meet security and compliance.
- Enhance with authentication and additional business-specific tools for production.

## Conclusion

By building and connecting a custom MCP server to GitHub Copilot Agent, your development team can automate naming conventions, enforce organizational rules, and streamline .NET project setup. This configuration introduces greater control and alignment with enterprise demands, paving the way for more robust development workflows.

For more details and starter guides, see:

- [Microsoft .NET Devblog on MCP Server](https://devblogs.microsoft.com/dotnet/build-a-model-context-protocol-mcp-server-in-csharp/)
- [MCP Server Quickstart Docs](https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-mcp-server)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/generating-classes-with-custom-naming-conventions-using-github/ba-p/4444837)
