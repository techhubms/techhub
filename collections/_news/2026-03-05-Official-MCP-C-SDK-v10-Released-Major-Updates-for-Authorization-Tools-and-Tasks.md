---
external_url: https://devblogs.microsoft.com/dotnet/release-v10-of-the-official-mcp-csharp-sdk/
title: 'Official MCP C# SDK v1.0 Released: Major Updates for Authorization, Tools, and Tasks'
author: Mike Kistler
primary_section: ai
feed_name: Microsoft .NET Blog
date: 2026-03-05 18:05:00 +00:00
tags:
- .NET
- AI
- ASP.NET Core
- Authorization
- C#
- Client Capabilities
- Distributed Cache
- Durability
- Elicitation
- IChatClient
- JWT
- Long Running Requests
- MCP
- Metadata
- Middleware
- News
- OAuth 2.0
- OpenAI
- Sampling
- SDK
- Tasks
- Tool Calling
section_names:
- ai
- dotnet
---
Mike Kistler presents the v1.0 release of the official MCP C# SDK, highlighting new authorization features, advanced tool integration, support for long-running operations, and experimental durable tasks—all designed for AI and .NET developers.<!--excerpt_end-->

# Official MCP C# SDK v1.0 Released: Major Updates for Authorization, Tools, and Tasks

*Author: Mike Kistler*

The [Model Context Protocol (MCP) C# SDK](https://github.com/modelcontextprotocol/csharp-sdk) has officially reached version 1.0, bringing a wide array of new features and improvements for .NET and AI developers.

## What’s New in v1.0

### 1. Enhanced Authorization Flows

- Full support for [2025-11-25 MCP Specification](https://modelcontextprotocol.io/specification/2025-11-25)
- Servers can now expose Protected Resource Metadata (PRM) documents in three standards-compliant ways
- Easy configuration on both server (using `.AddMcp` extension methods) and client sides
- Automatic handling of discovery sequences for authorization

### 2. Richer Metadata and Icon Support

- [Icons for tools, resources, and prompts](https://csharp.sdk.modelcontextprotocol.io/api/ModelContextProtocol.Server.McpServerToolAttribute.html#ModelContextProtocol_Server_McpServerToolAttribute_IconSource) can now be included and programmatically configured
- Metadata for both resources and implementations (server/client) now supports web URLs and rich icon descriptions (e.g., multiple formats, themes, sizes)
- Visual illustration of icon usage provided in the MCP Inspector

### 3. Incremental Scope Consent and Improved OAuth

- Brings the Principle of Least Privilege to MCP authorization
- Clients incrementally request scopes as required by each operation
- The SDK automates scope extraction and consent workflows on both clients and servers
- Recommended token validation settings outlined for secure deployment

### 4. URL Mode Elicitation

- Secure, out-of-band user interactions (such as gathering API keys or third-party consents) using browser-based, server-defined URLs
- Unified elicitation handler support for both form and URL mode in the SDK
- Emphasis on security (CSRF prevention, session management) during custom form development

### 5. Tool Calling Support in Sampling

- Servers can now make tools available in sampling requests to large language models (LLMs)
- Advanced orchestration: tools invoked by the LLM during a sampling session are handled with repeated exchanges until a final response is achieved
- Integration with Microsoft.Extensions.AI enables simplified creation of custom [SamplingHandler](https://csharp.sdk.modelcontextprotocol.io/api/ModelContextProtocol.Client.McpClientHandlers.html#ModelContextProtocol_Client_McpClientHandlers_SamplingHandler)

### 6. OAuth Client ID Metadata Documents

- Introduction of Client ID Metadata Documents (CIMD) as the new standard for client registration, streamlining dynamic relationships with authorization servers
- The SDK prefers CIMD, falling back to Dynamic Client Registration (DCR) as needed

### 7. Long-running Requests over HTTP

- Robust new handling of long-running operations and polling using [SSE streams](https://csharp.sdk.modelcontextprotocol.io/api/ModelContextProtocol.Server.ISseEventStreamStore.html)
- Improved event-tracking, disconnect/reconnect, and cache management (ensure to clean up event streams and consider time-based retention strategies)

### 8. Experimental Support for Durable Tasks

- Introduction of "tasks" as an experimental API primitive, enabling durable state tracking and deferred result retrieval
- Tools indicate task support via async signatures or explicit `ToolTaskSupport` enums
- The reference [InMemoryMcpTaskStore](https://modelcontextprotocol.github.io/csharp-sdk/api/ModelContextProtocol.InMemoryMcpTaskStore.html) is included; persistent backing stores are encouraged for production
- Rich client capabilities for task lifecycle management, polling, listing, and graceful cancellation

## Example Feature Highlights

### Configuring Authorization and Resource Metadata

```csharp
.AddMcp(options => {
    options.ResourceMetadata = new() {
        ResourceDocumentation = new Uri("https://docs.example.com/api/weather"),
        AuthorizationServers = { new Uri(inMemoryOAuthServerUrl) },
        ScopesSupported = ["mcp:tools"]
    };
});
```

### Adding Tool Icons

```csharp
[McpServerTool(Title = "This is a title", IconSource = "https://example.com/tool-icon.svg")]
public static string ToolWithIcon()
```

### Enabling Incremental Scope Consent (Client-Side)

The client SDK now listens for `401`/`403` responses and requests only the scopes required for each operation.

### Server Middleware for Secure Scope Handling

Authorization is now best handled in ASP.NET Core middleware; ensure the correct interpretation of JWT claims for robust security.

### Long-running Requests with Polling

```csharp
await context.EnablePollingAsync(retryInterval: TimeSpan.FromSeconds(retryIntervalInSeconds));
```

Use a DistributedCache or equivalent for event stream retention and cleanup.

### Durable Tasks API Example

```csharp
[McpServerTool(TaskSupport = ToolTaskSupport.Required)]
[Description("Processes a batch of data records. Always runs as a task.")]
public static async Task<string> ProcessData(
    [Description("Number of records to process")] int recordCount,
    CancellationToken cancellationToken)
{
    await Task.Delay(TimeSpan.FromSeconds(8), cancellationToken);
    return $"Processed {recordCount} records successfully.";
}
```

## Additional Resources

- [MCP Specification 2025-11-25](https://modelcontextprotocol.io/specification/2025-11-25)
- [Full Changelog](https://modelcontextprotocol.io/specification/2025-11-25/changelog)
- [C# SDK Repository](https://github.com/modelcontextprotocol/csharp-sdk)
- [Demo Projects](https://github.com/mikekistler/mcp-whats-new)

## Summary

The MCP C# SDK v1.0 offers a powerful foundation for integrating Model Context Protocol features in .NET, with deep support for modern AI workflows, secure operations, metadata richness, and robust durability for complex scenarios.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/release-v10-of-the-official-mcp-csharp-sdk/)
