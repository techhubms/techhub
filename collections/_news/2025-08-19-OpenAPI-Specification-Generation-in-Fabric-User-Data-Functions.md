---
external_url: https://blog.fabric.microsoft.com/en-US/blog/openapi-specification-code-generation-now-available-in-fabric-user-data-functions/
title: OpenAPI Specification Generation in Fabric User Data Functions
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-08-19 10:00:00 +00:00
tags:
- Agentic Use Cases
- AI Agents
- API Gateway
- API Integration
- API Management
- Azure AI Foundry
- Azure API Management
- Client Code
- Code Generation
- Data Engineering
- Develop Mode
- Docstring
- Fabric User Data Functions
- JSON
- Microsoft Fabric
- OpenAPI Specification
- Python
- REST API
- Swagger
- User Data Functions
- YAML
section_names:
- ai
- azure
- ml
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
