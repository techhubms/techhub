---
external_url: https://devblogs.microsoft.com/openapi/kiota-dart-support/
title: Announcing Public Preview of Kiota Dart Support
author: Maisa Rissi
feed_name: Microsoft DevBlog
date: 2025-02-04 16:12:50 +00:00
tags:
- .NET Tools
- API Automation
- API Client Generation
- API Integration
- Community Contribution
- Dart
- Declarative Agents
- Kiota
- Microsoft
- Microsoft 365 Copilot
- OAuth
- OpenAPI
- Strong Typing
- VS Code Extension
section_names:
- coding
---
Maisa Rissi introduces the public preview of Dart support in Kiota, detailing how developers can generate Dart API clients from OpenAPI specs and highlighting community-driven development.<!--excerpt_end-->

# Announcing Public Preview of Kiota Dart Support

**Author: Maisa Rissi**

## Overview

Kiota, Microsoft's open-source generator for API client libraries from OpenAPI descriptions, now supports Dart -- made possible by significant community contributions. This new feature, available in public preview, allows developers to effortlessly create type-safe Dart API clients for any OpenAPI-described API.

## Community-Driven Development

The Dart support milestone in Kiota was driven largely by the open-source community. Special thanks go to contributors [Kees-Schotanus](https://github.com/Kees-Schotanus), [Joanne ter Maat](https://github.com/joanne-ter-maat), [Ricardo Boss](https://github.com/ricardoboss), [Andrea Peruffo](https://github.com/andreaTP), and [Emond Papegaaij](https://github.com/papegaaij) for their efforts.

## What is Kiota?

- **Purpose:** Kiota generates API client libraries for multiple languages, now including Dart, from OpenAPI descriptions.
- **Use Cases:** Quickly create robust, strongly-typed clients for any RESTful API, and automate creating plugins for declarative agents in Microsoft 365 Copilot.
- **Benefits:** Saves developer time and reduces errors by eliminating manual client coding.

## Why Use Kiota?

- **Automation:** Kiota eliminates boilerplate code for API interactions.
- **Type Safety:** Generated clients catch errors at compile time.
- **Efficiency:** Tailor clients to your app needs, optimizing performance.
- **Community Collaboration:** The Dart implementation showcases the open-source spirit and Microsoft’s readiness to evolve the tool with community input.

## Getting Started with Kiota and Dart

### 1. Install Kiota

- Options: Use as a [.NET tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools), with Docker, or other supported methods.
- See [Kiota documentation](https://aka.ms/kiota/docs) for full installation instructions.

### 2. Generate the API Client

- Open the Kiota extension in your development environment (e.g., VS Code via the Active Bar icon).
- Provide a local or remote OpenAPI description, or choose one from Kiota’s public catalog.
- Select desired API paths and click Generate.
- Specify output language as Dart.
- Full quickstart guide: [Build API clients for Dart](https://learn.microsoft.com/en-us/openapi/kiota/quickstarts/dart).

### 3. Use the Generated Client

- Integrate the generated Dart code into your application.
- Example: Use API Key for authentication, or implement OAuth for more secure scenarios.

## Next Steps

- **Try Dart Support:** Test out Dart client generation and provide feedback at [aka.ms/kiota](https://aka.ms/kiota).
- **Authentication:** Implement OAuth for secure, authenticated API calls.
- **Contribute:** Report bugs, suggest features, or submit pull requests to improve Kiota further.
- **Follow the Project:** Monitor development as Dart support moves toward General Availability, continuing to share user feedback.

## Resources

- [Kiota Official Documentation](https://aka.ms/kiota/docs)
- [Build API clients for Dart Quickstart](https://learn.microsoft.com/en-us/openapi/kiota/quickstarts/dart)
- [OpenAPI at Microsoft Blog](https://devblogs.microsoft.com/openapi)

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/openapi/kiota-dart-support/)
