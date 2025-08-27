---
layout: "post"
title: "OpenAPI Specification Generation in Fabric User Data Functions"
description: "This news article explains how Microsoft Fabric now enables automatic generation of OpenAPI specifications for User Data Functions using the Functions portal. It covers the integration possibilities with client code generation, API management platforms like Azure API Management, and AI agents via Azure AI Foundry. Step-by-step instructions for updating the fabric-user-data-functions library and generating specifications in both JSON and YAML formats are included, plus tips for using Python docstrings to enrich API documentation."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/openapi-specification-code-generation-now-available-in-fabric-user-data-functions/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-19 10:00:00 +00:00
permalink: "/2025-08-19-OpenAPI-Specification-Generation-in-Fabric-User-Data-Functions.html"
categories: ["AI", "Azure", "ML"]
tags: ["Agentic Use Cases", "AI", "AI Agents", "API Gateway", "API Integration", "API Management", "Azure", "Azure AI Foundry", "Azure API Management", "Client Code", "Code Generation", "Data Engineering", "Develop Mode", "Docstring", "Fabric User Data Functions", "JSON", "Microsoft Fabric", "ML", "News", "OpenAPI Specification", "Python", "REST API", "Swagger", "User Data Functions", "YAML"]
tags_normalized: ["agentic use cases", "ai", "ai agents", "api gateway", "api integration", "api management", "azure", "azure ai foundry", "azure api management", "client code", "code generation", "data engineering", "develop mode", "docstring", "fabric user data functions", "json", "microsoft fabric", "ml", "news", "openapi specification", "python", "rest api", "swagger", "user data functions", "yaml"]
---

Microsoft Fabric Blog details how developers can use the Functions portal to automatically generate OpenAPI specifications for User Data Functions, supporting API integrations, client code generation, and AI agent capabilities.<!--excerpt_end-->

# OpenAPI Specification Generation in Fabric User Data Functions

Microsoft Fabric has introduced the ability to automatically generate OpenAPI specifications (formerly Swagger Specification) for User Data Functions directly in the Functions portal. OpenAPI is a language-agnostic standard for describing REST APIs, enabling both human and machine users to understand service capabilities, which is critical for integration, code generation, and AI scenarios.

## Key Use Cases

1. **Client Code Generation**
   - Generate REST API client code for your Fabric functions using tools like Swagger Codegen.
   - Automate invocation from external applications based on OpenAPI specs.
2. **API Gateway and Management Integration**
   - Import OpenAPI specs into API management platforms such as Azure API Management.
   - Facilitate public exposure and management of User Data Functions endpoints.
3. **Integration with AI Agents**
   - Use platforms like Azure AI Foundry to configure tools and models that interact with your APIs.
   - Unlock agentic AI scenarios by giving models structured access to function endpoints.

## How to Generate OpenAPI Specifications

- Ensure your project uses the latest `fabric-user-data-functions` library version. Update it via the Functions portal.
- Navigate to your User Data Functions item.
- Click **Generate invocation code** in the toolbar.
- Select **Open API specification** from the dropdown.
- Choose JSON or YAML format in the popup and copy your specification.

### Python Docstrings for Documentation

Provide meaningful [docstrings](https://peps.python.org/pep-0257/) in your Python functions to automatically include descriptive metadata in the generated OpenAPI spec.

## Updating the Library

- Go to the Functions portal's Library Management section.
- Find and edit the `fabric-user-data-functions` library.
- Select the latest version from the dropdown; publishing your functions is not required after the update.
- Keeping the library up to date is important for compatibility with new portal features.

## Resources

- For questions, email [FabricUserDataFunctionsPreview@service.microsoft.com](mailto:FabricUserDataFunctionsPreview@service.microsoft.com).
- [Develop mode in the Microsoft Fabric docs](https://go.microsoft.com/fwlink/?linkid=2330551)
- [Fabric User Data Functions overview](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/user-data-functions-overview)

---

*Images and walkthroughs illustrating the process are available in the original post.*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/openapi-specification-code-generation-now-available-in-fabric-user-data-functions/)
