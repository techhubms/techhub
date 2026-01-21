---
external_url: https://devblogs.microsoft.com/openapi/welcome-post/
title: 'Welcome to the OpenAPI Blog: Microsoft’s Hub for API Development, Tools, and Best Practices'
author: Darrel Miller
feed_name: Microsoft DevBlog
date: 2024-10-01 18:48:35 +00:00
tags:
- .NET
- API Center
- API Design
- API Development
- ASP.NET Core
- Azure API Management
- Client Code Generation
- Copilot Agents
- Hidi
- Kiota
- Microsoft
- OpenAPI
- Power Platform
- Semantic Kernel
- TypeSpec
section_names:
- ai
- azure
- coding
- github-copilot
---
Darrel Miller launches the Microsoft OpenAPI Blog, detailing what developers can expect on topics including API design, OpenAPI tools, Azure integrations, AI plugins, Copilot agents, and more.<!--excerpt_end-->

# Welcome to the OpenAPI Blog

*By Darrel Miller*

![OpenApiIntro.jpeg](https://devblogs.microsoft.com/openapi/wp-content/uploads/sites/82/2024/10/OpenAPIIntro.jpeg)

### New Home for an Old Friend

Although this is the inaugural post on this new platform, Microsoft has a longstanding relationship with the [OpenAPI specification](https://spec.openapis.org/). As a founding member and key contributor to the [OpenAPI Initiative](https://www.openapis.org/), Microsoft uses OpenAPI internally across a wide range of products and supports developers with tools that utilize OpenAPI descriptions throughout the API development lifecycle.

### What You Will Find Here

The OpenAPI Blog will be a hub for sharing:  

- Experiences working with OpenAPI  
- Best practices, tips, and tricks for API development  
- News and updates about the OpenAPI Specification and related Microsoft tools  
- Insights that complement the [Microsoft OpenAPI documentation portal](https://learn.microsoft.com/openapi/)

### .NET Ecosystem Libraries

Many .NET tools that interact with OpenAPI leverage the [Microsoft.OpenApi.NET](https://www.nuget.org/packages/Microsoft.OpenApi) library for parsing, creating, and validating OpenAPI descriptions. The team is preparing for a major release introducing performance improvements, API enhancements, and support for the OpenAPI v3.1 specification.

### Client Code Generation

Developers seeking to generate API client code from OpenAPI descriptions can use [Kiota](https://aka.ms/kiota/docs). Kiota currently supports client generation in C#, Go, Java, PHP, and Python, with TypeScript support in preview and ongoing community efforts for Dart.

### OpenAPI Description Generation from Code

With .NET 9, [ASP.NET Core](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview4/aspnetcore.md#built-in-support-for-openapi-document-generation) introduces built-in support for generating OpenAPI descriptions directly from C# code, making it easier for developers to manage and share API documentation.

### Design-First API Development

For those who prefer upfront API design, [TypeSpec](https://typespec.io) offers a robust language and tooling for describing APIs and exporting them as OpenAPI definitions. TypeSpec enhances team collaboration in API design before development begins.

### Azure API Management and API Center

[Azure API Management](https://learn.microsoft.com/en-us/azure/api-management/) acts as Microsoft's enterprise API gateway, supporting OpenAPI descriptions and enabling management, security, and publishing of APIs. The newer [Azure API Center](https://learn.microsoft.com/en-us/azure/api-center/overview) helps manage APIs across the entire lifecycle, with built-in OpenAPI support.

### Power Platform Connectors

Developers can use OpenAPI descriptions to create custom connectors for Power Platform, enabling integration with PowerApps and Power Automate. Example connectors and community resources are available in the [PowerPlatform Connectors GitHub repository](https://github.com/microsoft/PowerPlatformConnectors/).

### OpenAPI Transformation Tools

The [Hidi](https://aka.ms/hidi) tool transforms OpenAPI descriptions between YAML and JSON formats or between specification versions (v2/v3). It also converts OData CSDL descriptions to OpenAPI and provides graphical API visualization.

### Integrating OpenAPI with AI: Semantic Kernel

The [Semantic Kernel](https://learn.microsoft.com/en-us/semantic-kernel/) platform allows AI applications to use [OpenAPI-based plugins](https://learn.microsoft.com/en-us/semantic-kernel/concepts/plugins/adding-openapi-plugins?pivots=programming-language-csharp) for real-time API integrations. API Manifest Plugins can help import subsets of large OpenAPI descriptions into your AI workflows.

### Copilot Agents and Extensibility

Copilot Agents open new possibilities for extending [Microsoft 365 Copilot](https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/overview-api-plugins) via OpenAPI-described APIs, allowing deeper business process integration and automation.

### Join the Journey

Microsoft supports a wide variety of developer workflows and tools for OpenAPI, extending from API code generation through to integration with Azure, Power Platform, AI, and Copilot extensibility. The OpenAPI Blog's purpose is to share knowledge and updates, answer your questions, and provide resources to help you maximize the value of OpenAPI throughout your development journey.

Feel free to leave comments with your questions or topics you’d like to see covered.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/openapi/welcome-post/)
