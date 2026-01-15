---
layout: post
title: 'How to Host Remote MCP Servers on Azure Functions: Extension and SDK Approaches'
author: lily-ma
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-remote-mcp-servers-on-azure-functions/ba-p/4471047
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 18:15:00 +00:00
permalink: /ai/community/How-to-Host-Remote-MCP-Servers-on-Azure-Functions-Extension-and-SDK-Approaches
tags:
- .NET
- AI
- API Center
- Authentication
- Authorization
- Azure
- Azure Foundry
- Azure Functions
- C#
- Coding
- Community
- Custom Handler
- Flex Consumption
- Host.json
- Identity Providers
- Java
- JavaScript
- Maven Build Plugin
- MCP
- MCP Extension
- MCP Server
- MCP Tool Trigger
- Microsoft Entra ID
- OAuth
- OBO Auth
- Python
- SDK
- Security
- Server Sent Events
- Serverless
- Stateless Server
- Streamable HTTP
- TypeScript
section_names:
- ai
- azure
- coding
- security
---
lily-ma presents a comprehensive overview of hosting remote Model Context Protocol (MCP) servers on Azure Functions, helping developers deploy secure, scalable agent tools using extension or SDK hosting options.<!--excerpt_end-->

# How to Host Remote MCP Servers on Azure Functions

**Author:** lily-ma

Model Context Protocol (MCP) servers enable AI agents to interact securely with external tools, services, and data sources. Azure Functions provides a robust serverless platform for hosting these MCP servers, ensuring scalability, reliability, and security. This guide reviews two main approaches for deployment: the MCP extension (generally available) and self-hosted SDK-based servers (public preview).

## 1. Hosting Options

### Azure Functions MCP Extension (GA)

- **Programming Languages Supported:** .NET, Java, JavaScript, Python, TypeScript.
- **Programming Model:** Utilizes triggers and bindings, such as the MCP tool trigger, allowing focus on tool logic without handling lower-level protocol logistics.
- **Key Features:**
  - Support for [streamable-http transport](https://learn.microsoft.com/en-us/azure/azure-functions/flex-consumption-plan) (recommended for most scenarios) and Server-Sent Events (SSE) endpoints.
  - Define server configuration and metadata in the `host.json` file, including instructions, name, version, encryption, message options, and webhook authorization settings.
  - Built-in authentication and authorization via [Microsoft Entra ID](https://learn.microsoft.com/en-us/azure/app-service/configure-authentication-mcp), OAuth, and support for OBO (on-behalf-of) flows.
  - **Maven Build Plugin for Java:** Automatically parses/validates MCP tool annotations, auto-generates extension configs, and optimizes cold starts.
- **Quickstart Links:**
  - [C# (.NET)](https://github.com/Azure-Samples/remote-mcp-functions-dotnet)
  - [Python](https://github.com/Azure-Samples/remote-mcp-functions-python)
  - [TypeScript (Node.js)](https://github.com/Azure-Samples/remote-mcp-functions-typescript)
  - [Java](https://github.com/Azure-Samples/remote-mcp-functions-java)
- **Documentation:**
  - [Extension Overview](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-mcp?pivots=programming-language-csharp)
  - [Tool Trigger Details](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-mcp-trigger?tabs=attribute&pivots=programming-language-csharp)

#### Example host.json for MCP Extension

```json
{
  "version": "2.0",
  "extensions": {
    "mcp": {
      "instructions": "Some test instructions on how to use the server",
      "serverName": "TestServer",
      "serverVersion": "2.0.0",
      "encryptClientState": true,
      "messageOptions": {
        "useAbsoluteUriForEndpoint": false
      },
      "system": {
        "webhookAuthorizationLevel": "System"
      }
    }
  }
}
```

### Self-Hosted MCP Servers (SDK)

- **SDK Hosting:** Deploy servers built with official MCP SDKs for Python, TypeScript, C#, Java (Java coming soon) using Azure Functions custom handlers.
- **Supported Features:** Streamable-http transport for stateless servers, built-in authentication/authorization as with MCP extension.
- **Deployment:**
  - **Custom Handler Configuration:** Place a host.json in the project root pointing to the main executable/script.
  - Example for Python:

    ```json
    {
      "version": "2.0",
      "configurationProfile": "mcp-custom-handler",
      "customHandler": {
        "description": {
          "defaultExecutablePath": "python",
          "arguments": ["hello.py"]
        },
        "port": "8000"
      }
    }
    ```

- **Quickstart SDK Hosting:**
  - [C# (.NET)](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-dotnet)
  - [Python](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-python)
  - [Node.js/TypeScript](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-node)
  - Java (coming soon)
- **Documentation:** [Self-hosted MCP Servers](https://aka.ms/self-hosted-mcp-docs)

## 2. Platform Features

- **Scalability:** Utilize [Flex Consumption](https://learn.microsoft.com/en-us/azure/azure-functions/flex-consumption-plan) serverless model for cost-effective scaling.
- **Security:** Robust authentication and authorization via Microsoft Entra ID and OAuth; support for configurable authorization protocols and metadata.
- **Integration:** Compatible with Azure Foundry and API Center for advanced service integrations.

## 3. Getting Started and Best Practices

- Choose the **MCP extension** for a Functions-native experience or use **SDK hosting** for minimal code changes if you have an existing MCP server.
- Always configure server info in `host.json` and leverage built-in auth for secure deployments.
- Favor streamable-http transport over SSE except for legacy clients.
- Review relevant quickstarts for your preferred language/stack.

---

## Useful Links

- [Azure Functions Flex Consumption Plan](https://learn.microsoft.com/en-us/azure/azure-functions/flex-consumption-plan)
- [Azure Functions MCP Extension Documentation](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-mcp?pivots=programming-language-csharp)
- [MCP SDK Official Repo](https://github.com/modelcontextprotocol/servers)
- [Self-hosted MCP Docs](https://aka.ms/self-hosted-mcp-docs)

## Feedback

Have suggestions or feature requests for hosting MCP servers on Azure Functions? Share your needs and priorities with the Azure team to help shape future improvements.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-remote-mcp-servers-on-azure-functions/ba-p/4471047)
