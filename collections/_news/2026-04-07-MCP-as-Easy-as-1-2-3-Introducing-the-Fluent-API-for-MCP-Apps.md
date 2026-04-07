---
feed_name: Microsoft Azure SDK Blog
tags:
- .NET
- .NET Isolated Worker
- AI
- Apps
- Azure
- Azure Functions
- Azure Functions MCP Extension
- Azure SDK
- C#
- Clipboard Permissions
- Content Security Policy
- CSP
- Fluent API
- Functions
- HTML Views
- IMcpAppBuilder
- Least Privilege
- MCP
- MCP Apps
- Mcpapps
- McpToolTrigger
- McpViewSource
- Microsoft.Azure.Functions.Worker.Extensions.Mcp
- MIME Type
- News
- NuGet Package
- Permissions
- Preview Release
- Security
- Static Assets
- Tool Metadata
- Ui:// URI
section_names:
- ai
- azure
- dotnet
- security
primary_section: ai
author: Lilian Kasem (she/her)
title: 'MCP as Easy as 1-2-3: Introducing the Fluent API for MCP Apps'
external_url: https://devblogs.microsoft.com/azure-sdk/mcp-as-easy-as-1-2-3-introducing-the-fluent-api-for-mcp-apps/
date: 2026-04-07 21:16:11 +00:00
---

Lilian Kasem (she/her) introduces a new Fluent API for building MCP Apps with Azure Functions (.NET isolated worker), showing how to turn an MCP tool into a UI-capable app and configure views, permissions, and CSP security policies with a small amount of code.<!--excerpt_end-->

# MCP as Easy as 1-2-3: Introducing the Fluent API for MCP Apps

Earlier this year, Microsoft introduced **MCP Apps** in the **Azure Functions MCP extension**. MCP Apps are tools that go beyond text: they can render full UI experiences, serve static assets, and integrate with AI agents.

If you’re new to this, start with the MCP Apps quickstart:

- MCP Apps quickstart: https://learn.microsoft.com/azure/azure-functions/scenario-mcp-apps?tabs=bash%2Clinux&pivots=programming-language-csharp

This post introduces a **fluent configuration API** for the **.NET isolated worker** that lets you promote any MCP tool into a full MCP App (views, permissions, and security policies) with a few lines of code.

## What are MCP Apps?

MCP Apps extend the Model Context Protocol (MCP) tool model by letting individual tools be configured as apps: tools with UI views, static assets, and fine-grained security controls.

Model Context Protocol intro:

- https://modelcontextprotocol.io/introduction

With MCP Apps, you can:

- Attach **HTML views** to tools (rendered by the MCP client)
- Serve **static assets** (HTML, CSS, JavaScript, images)
- Control **permissions** like clipboard access
- Define **Content Security Policies (CSP)** for what the app can load/connect to

## Why a Fluent API?

In the MCP protocol, wiring a tool to a UI view requires coordinating several details:

- A resource endpoint at a `ui://` URI
- A special mime type: `text/html;profile=mcp-app`
- `_meta.ui` metadata on both tool and resource
  - Tool metadata includes details like `resourceUri` and `visibility`
  - Resource response includes CSP, permissions, and rendering hints

The new Fluent API abstracts those MCP protocol details. Calling `AsMcpApp` automatically:

- Generates the synthetic resource function
- Sets the correct mime type
- Injects metadata connecting the tool to its view

## Get started

The Fluent API for MCP Apps is available as a **preview** in the `Microsoft.Azure.Functions.Worker.Extensions.Mcp` NuGet package.

Install/upgrade:

```bash
 dotnet add package Microsoft.Azure.Functions.Worker.Extensions.Mcp --version 1.5.0-preview.1 
```

Because it’s a preview, APIs may change based on feedback before stable release.

## The Fluent API (Hello App example)

## Step 1: Define your function

Start with a standard Azure Functions MCP tool. The `[McpToolTrigger]` attribute wires the function up as an MCP tool.

```csharp
[Function(nameof(HelloApp))]
public string HelloApp(
    [McpToolTrigger("HelloApp", "A simple MCP App that says hello.")] ToolInvocationContext context)
{
    return "Hello from app";
}
```

## Step 2: Configure it as an MCP App

In startup, use the Fluent API to promote the tool into a full MCP App:

```csharp
builder
    .ConfigureMcpTool("HelloApp")
    .AsMcpApp(app => app
        .WithView("assets/hello-app.html")
        .WithTitle("Hello App")
        .WithPermissions(McpAppPermissions.ClipboardWrite | McpAppPermissions.ClipboardRead)
        .WithCsp(csp =>
        {
            csp.AllowBaseUri("https://www.microsoft.com")
               .ConnectTo("https://www.microsoft.com");
        }));
```

## Step 3: Add your view

Create an HTML file at `assets/hello-app.html`. MCP clients render this UI when the tool is invoked. You control the markup, styling, and client-side behavior.

## Breaking down the API

### `ConfigureMcpTool("HelloApp")`

Selects the MCP tool to configure. The name must match the function name registered with `[McpToolTrigger]`.

### `.AsMcpApp(app => ...)`

Promotes the tool to an MCP App. Inside the lambda, you configure app behavior via `IMcpAppBuilder`.

### `.WithView(...)`

Sets the view for the app. You can use a file path or `McpViewSource` factories:

```csharp
// File on disk (relative to output directory)
app.WithView("assets/hello-app.html");

// Explicit file source
app.WithView(McpViewSource.FromFile("assets/hello-app.html"));

// Embedded resource from an assembly
app.WithView(McpViewSource.FromEmbeddedResource("MyApp.Resources.view.html"));
```

`McpViewSource` lets you choose whether HTML is deployed as a file alongside the function or embedded into the assembly for self-contained deployment.

### `.WithTitle("Hello App")`

Sets a human-readable title displayed by MCP clients.

### `.WithBorder()`

Hints how the MCP client should render a border around the view:

```csharp
.WithBorder() // prefer border
.WithBorder(false) // prefer no border
```

### `.WithDomain("myapp.example.com")`

Sets a domain hint used by the host to scope cookies and storage.

### `.WithPermissions(...)`

Permissions are opt-in flags on `McpAppPermissions`:

| Permission | Description |
| --- | --- |
| `ClipboardRead` | Allows the app to read from the clipboard |
| `ClipboardWrite` | Allows the app to write to the clipboard |

### `.WithCsp(csp => ...)`

Defines the Content Security Policy for the app’s view. The CSP builder (`IMcpCspBuilder`) maps methods to CSP directives:

| Method | CSP Directive | Purpose |
| --- | --- | --- |
| `ConnectTo(origin)` | `connect-src` | Network requests (fetch, XHR, WebSocket) |
| `LoadResourcesFrom(origin)` | `img-src`, `script-src`, `style-src`, `font-src`, `media-src` | Static resources |
| `AllowFrame(origin)` | `frame-src` | Nested iframes |
| `AllowBaseUri(origin)` | `base-uri` | Base URI for the document |

Example:

```csharp
.WithCsp(csp =>
{
    csp.ConnectTo("https://api.example.com")
       .LoadResourcesFrom("https://cdn.example.com")
       .AllowFrame("https://youtube.com")
       .AllowBaseUri("https://www.microsoft.com");
})
```

You can call `WithCsp` multiple times; origins accumulate across calls. The default CSP is restrictive, and you allowlist origins explicitly (least privilege).

### `.WithVisibility(...)`

Controls who can see the tool via `McpVisibility` flags:

| Visibility | Description |
| --- | --- |
| `Model` | Visible to the LLM during tool selection |
| `App` | Visible to the host UI for rendering |

Default is `Model | App`. You can make a UI-only tool hidden from the model:

```csharp
.ConfigureApp()
.WithVisibility(McpVisibility.App)
```

### `.WithStaticAssets(...)`

Serves static assets from a directory:

```csharp
.ConfigureApp()
.WithStaticAssets("assets/dist");

// Or with options
.WithStaticAssets("assets/dist", options =>
{
    options.IncludeSourceMaps = true; // default: false
});
```

By default, `.map` files are excluded to avoid leaking internal paths and implementation details.

### Builder navigation

There are three builder levels (tool, app, view) and you can move between them:

```csharp
builder
    .ConfigureMcpTool("Dashboard")
    .AsMcpApp(app => app
        .WithView("ui/dashboard.html") // returns IMcpViewBuilder
        .WithTitle("Dashboard")
        .WithPermissions(McpAppPermissions.ClipboardRead)
        .ConfigureApp() // back to IMcpAppBuilder
        .WithStaticAssets("ui/dist")
        .WithVisibility(McpVisibility.Model | McpVisibility.App))
    .WithProperty("dataset", McpToolPropertyType.String, "The dataset to display"); // back to tool builder
```

## Summary

The Fluent API for MCP Apps in the .NET isolated worker aims to make MCP Apps straightforward to build:

- Attach a UI view
- Configure permissions
- Apply CSP security controls

The extension handles protocol wiring like synthetic resource generation, metadata, mime type management, and secure asset serving.

## Useful links

- Building MCP Apps with Azure Functions MCP Extension: https://techcommunity.microsoft.com/blog/appsonazureblog/building-mcp-apps-with-azure-functions-mcp-extension/4496536
- MCP Apps quickstart: https://learn.microsoft.com/azure/azure-functions/scenario-mcp-apps?tabs=bash%2Clinux&pivots=programming-language-csharp
- Azure Functions MCP extension documentation: https://learn.microsoft.com/azure/azure-functions/functions-bindings-mcp


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/mcp-as-easy-as-1-2-3-introducing-the-fluent-api-for-mcp-apps/)

