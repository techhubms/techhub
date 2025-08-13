---
layout: "post"
title: "Build-Time OpenAPI Documentation in .NET 9: A OneDevQuestion with Mike Kistler"
description: "Mike Kistler discusses .NET 9's build-time OpenAPI documentation feature, highlighting its benefits for developers. He explains how generating docs at build-time enhances local linting, code generation, and tool integration, eliminating the need to serve documentation from applications."
author: "dotnet"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=xBTrFRtBj_0"
viewing_mode: "internal"
feed_name: "DotNet YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCvtT19MZW8dq5Wwfu6B0oxw"
date: 2025-07-16 15:20:20 +00:00
permalink: "/2025-07-16-Build-Time-OpenAPI-Documentation-in-NET-9-A-OneDevQuestion-with-Mike-Kistler.html"
categories: ["Coding"]
tags: [".NET", ".NET 9", "API Development", "Build Time Generation", "Code Generation", "Coding", "Documentation", "Linting", "OneDevQuestion", "OpenAPI", "Spectral", "Videos", "Web API"]
tags_normalized: ["net", "net 9", "api development", "build time generation", "code generation", "coding", "documentation", "linting", "onedevquestion", "openapi", "spectral", "videos", "web api"]
---

In this video, Mike Kistler shares his top OpenAPI feature in .NET 9—build-time generation—offering insights on how it streamlines API development for .NET professionals.<!--excerpt_end-->

{% youtube xBTrFRtBj_0 %}

## Build-Time OpenAPI Documentation in .NET 9: Insights from Mike Kistler

In a recent episode of #OneDevQuestion, Mike Kistler highlights his favorite OpenAPI feature introduced in .NET 9: build-time generation of OpenAPI documentation.

### Key Points Discussed

- **Build-Time Generation**: .NET 9 now supports generating OpenAPI documentation as part of the build process. This eliminates the need for your application to serve the documentation at runtime.
- **Local Usage**: Developers can utilize the generated OpenAPI docs locally, facilitating tasks such as:
  - Linting the API definition to ensure it conforms to best practices
  - Code generation from the OpenAPI specification for clients or servers
  - Applying tools like Spectral to enforce API design conventions

### Benefits

- **Performance and Security**: By moving documentation serving out of the application, runtime overhead and potential exposure are reduced.
- **Increased Flexibility**: Local documentation files offer greater integration with CI/CD pipelines, linting, and external tools—improving the developer experience and code quality.

### Tools Highlighted

- **Spectral**: A popular tool for linting OpenAPI (and other API formats) to enforce consistency and catch issues early.

### References and Further Reading

- Microsoft resource: [https://msft.it/6056s6ib2](https://msft.it/6056s6ib2)
- Hashtags: #dotnet #OpenAPI #webapi

---

*Author: dotnet*
