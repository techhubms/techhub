---
external_url: https://devblogs.microsoft.com/dotnet/mcp-csharp-sdk-2025-06-18-update/
title: 'MCP C# SDK Updated: Protocol 2025-06-18 Brings Elicitation, Structured Output, and Enhanced Security'
author: Mike Kistler
viewing_mode: external
feed_name: Microsoft .NET Blog
date: 2025-07-22 17:00:00 +00:00
tags:
- .NET
- Authentication
- C#
- Dependency Injection
- Elicitation
- MCP
- OAuth2
- OpenID Connect
- Resource Links
- Schema
- SDK
- Structured Output
- Tool Metadata
section_names:
- ai
- coding
- security
---
In this post, Mike Kistler presents the major update for the MCP C# SDK, introducing support for protocol version 2025-06-18. The release offers .NET developers new features such as elicitation, structured tool output, enhanced authentication, resource links, and schema improvements for AI application development.<!--excerpt_end-->

# MCP C# SDK Gets Major Update: Protocol Version 2025-06-18 Support

**Author:** Mike Kistler

The Model Context Protocol (MCP) continues to evolve, and the MCP C# SDK now supports the [latest specification version 2025-06-18](https://modelcontextprotocol.io/specification/2025-06-18). This release introduces significant new features for .NET developers building AI applications:

- Improved authentication protocol (better security; OAuth2/OpenID Connect integration)
- Elicitation support (interactive information gathering)
- Structured tool output
- Resource links in tool responses
- Schema and metadata improvements

These new capabilities help .NET AI developers create more robust, secure, and interactive solutions—whether building AI assistants, automation tools, or adding AI to existing applications.

## Key Features Breakdown

### Improved Authentication Protocol

The 2025-06-18 MCP specification adds a new authentication protocol that enhances security and makes integration easier. The authentication server and resource server roles are split, allowing for seamless integration with OAuth2 and OpenID Connect providers.

> **Further Reading:** [OAuth In The MCP C# SDK: Simple, Secure, Standard by Den Delimarsky](https://den.dev/blog/mcp-csharp-sdk-authorization/)

### Elicitation: Interactive User Engagement

Elicitation enables servers to request structured input from users during conversations, creating more dynamic and context-aware AI experiences.

#### Server-Side Elicitation

- Use the [`ElicitAsync`](https://modelcontextprotocol.github.io/csharp-sdk/api/ModelContextProtocol.Server.McpServerExtensions.html#ModelContextProtocol_Server_McpServerExtensions_ElicitAsync_ModelContextProtocol_Server_IMcpServer_ModelContextProtocol_Protocol_ElicitRequestParams_System_Threading_CancellationToken_) extension method on `IMcpServer`
- The SDK registers an `IMcpServer` for dependency injection
- Describe required user input with schema (supports string, number, boolean)
- Request single or multiple inputs with unique names
- Example: Asking if a user wants to play a game

  ```csharp
  [McpServerTool, Description("A simple game where the user has to guess a number between 1 and 10.")]
  public async Task<string> GuessTheNumber(IMcpServer server, CancellationToken token)
  {
      var playSchema = new RequestSchema
      {
          Properties = { ["Answer"] = new BooleanSchema() }
      };
      var playResponse = await server.ElicitAsync(
          new ElicitRequestParams
          {
              Message = "Do you want to play a game?",
              RequestedSchema = playSchema
          }, token);
      if (playResponse.Action != "accept" || playResponse.Content?["Answer"].ValueKind != JsonValueKind.True)
      {
          return "Maybe next time!";
      }
      // continue with the rest...
  }
  ```

#### Client-Side Elicitation

- Clients declare support in the `initialize` request by configuring an `ElicitationHandler` in `McpClientOptions`:

  ```csharp
  McpClientOptions options = new()
  {
      ClientInfo = new() { Name = "ElicitationClient", Version = "1.0.0" },
      Capabilities = new()
      {
          Elicitation = new()
          {
              ElicitationHandler = HandleElicitationAsync
          }
      }
  };
  ```

- `ElicitationHandler` receives server requests, prompts the user, and returns input matching the schema.
- Sample handler for a console application:

  ```csharp
  async ValueTask<ElicitResult> HandleElicitationAsync(ElicitRequestParams? requestParams, CancellationToken token)
  {
      if (requestParams?.RequestedSchema?.Properties == null)
          return new ElicitResult();
      if (requestParams?.Message is not null)
          Console.WriteLine(requestParams.Message);

      var content = new Dictionary<string, JsonElement>();
      foreach (var property in requestParams.RequestedSchema.Properties)
      {
          // Perform appropriate prompting and conversion for BooleanSchema, NumberSchema, or StringSchema
          // Add user input to `content`
      }
      return new ElicitResult { Action = "accept", Content = content };
  }
  ```

### Structured Tool Output

Tools can now return explicitly structured results, allowing better interpretation by hosts/LLMs:

- Use `[McpServerTool(UseStructuredContent = true)]` to enable structured output.
- The SDK generates a JSON schema based on the return value type.
- Example method:

  ```csharp
  [McpServerTool(UseStructuredContent = true), Description("Gets a list of structured product data with detailed information.")]
  public static List<Product> GetProducts(int count = 5)
  ```

- Example of tool metadata returned in `tools/list` response:

  ```json
  {
    "result": {
      "tools": [
        {
          "name": "get_products",
          "description": "Gets a list of structured product data with detailed information.",
          "inputSchema": { "type": "object", "properties": { "count": { "type": "integer", "default": 5 } } },
          "outputSchema": { "type": "object", "properties": { "result": { "type": "array", ... } } }
        }
      ]
    }
  }
  ```

- When invoked, the output of the tool is returned inside `result.structuredContent`.

### Resource Links in Tool Results

Tools may return **resource links** to help clients locate and interact with created or managed resources.

Example:

  ```csharp
  [McpServerTool]
  [Description("Creates a resource with a random value and returns a link to this resource.")]
  public async Task<CallToolResult> MakeAResource()
  {
      int id = new Random().Next(1, 101);
      var resource = ResourceGenerator.CreateResource(id);
      var result = new CallToolResult();
      result.Content.Add(new ResourceLinkBlock() { Uri = resource.Uri, Name = resource.Name });
      return result;
  }
  ```

### Schema Improvements & Metadata

- The `_meta` field is available in more interface types for extensibility.
- Tools, resources, and prompts now have separate `name` and `title` fields.
- Example of specifying a tool title:

  ```csharp
  [McpServerToolType]
  public class EchoTool
  {
      [McpServerTool(Name = "echo", Title = "Echo Tool")]
      [Description("Echoes the message back to the client.")]
      public static string Echo(string message) => $"Echo: {message}";
  }
  ```

The tool's `tools/list` output includes both `name` and `title` fields.

### Getting Started

1. Update your package:

   ```bash
   dotnet add package ModelContextProtocol --prerelease
   ```

2. Review the [documentation](https://modelcontextprotocol.github.io/csharp-sdk/api/ModelContextProtocol.html) and [samples](https://github.com/modelcontextprotocol/csharp-sdk/tree/main/samples)
3. Follow security best practices: use proper OAuth flows, validate user input, use resource indicators for tokens, and review [protocol security guidelines](https://modelcontextprotocol.io/specification/2025-06-18/basic/security_best_practices).

## Additional Links

- [modelcontextprotocol/csharp-sdk GitHub repository](https://github.com/modelcontextprotocol/csharp-sdk)
- [MCP C# SDK documentation](https://modelcontextprotocol.github.io/csharp-sdk/api/ModelContextProtocol.html)
- [MCP C# SDK samples](https://github.com/modelcontextprotocol/csharp-sdk/tree/main/samples)

## Summary

The MCP C# SDK’s support for protocol version 2025-06-18 gives .NET developers powerful new tools for building secure, interactive, and sophisticated AI solutions. Update your SDK and explore the new features to build the next generation of AI-powered .NET applications.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/mcp-csharp-sdk-2025-06-18-update/)
