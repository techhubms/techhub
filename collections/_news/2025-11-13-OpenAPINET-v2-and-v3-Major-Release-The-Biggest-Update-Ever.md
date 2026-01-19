---
layout: post
title: 'OpenAPI.NET v2 & v3 Major Release: The Biggest Update Ever'
author: Darrel Miller, Rachit Kumar Malik
canonical_url: https://devblogs.microsoft.com/openapi/openapi-net-release-announcements/
viewing_mode: external
feed_name: Microsoft OpenAPI Blog
feed_url: https://devblogs.microsoft.com/openapi/feed/
date: 2025-11-13 20:53:01 +00:00
permalink: /coding/news/OpenAPINET-v2-and-v3-Major-Release-The-Biggest-Update-Ever
tags:
- .NET
- API Documentation
- ASP.NET Core
- Microsoft
- NSwag
- Open Source
- OpenAPI
- OpenAPI Specification
- OpenAPI V3.1
- OpenAPI V3.2
- OpenAPI.NET
- Performance Optimization
- Semantic Kernel
- Serialization
- Swashbuckle
- System.Text.Json
section_names:
- coding
---
Darrel Miller and Rachit Kumar Malik announce the largest update ever to OpenAPI.NET, bringing major performance, compatibility, and feature enhancements to help .NET developers keep pace with modern API standards.<!--excerpt_end-->

# OpenAPI.NET v2 & v3 Major Release: The Biggest Update Ever

**Authors:** Darrel Miller, Rachit Kumar Malik

OpenAPI.NET—the core library underpinning OpenAPI support in the .NET ecosystem—has just received its most important update since launch in 2018. With the simultaneous release of **OpenAPI.NET v2** and **v3**, developers have access to:

- Latest OpenAPI Specification support (v3.1.0 in v2, v3.2.0 in v3)
- Major performance improvements (up to 50% faster parsing and 35% less memory usage)
- Richer model properties, new serialization options, and numerous API surface enhancements

## What’s New in OpenAPI.NET v2

- **Full OpenAPI v3.1 Support:** Complete serialization and model compatibility with the latest spec
- **Performance:** Uses `System.Text.Json` for faster JSON parsing while continuing YAML support. 50% reduction in document processing time, 35% memory savings
- **Improved API Surface:** Native JSON node API use for properties of type “json”
- **Lazy Reference Resolution:** Faster load times for large documents using heavy `$ref` references
- **Reduced Dependencies:** Reads/writes JSON without extra packages

[Upgrade Guide for v2](https://github.com/microsoft/OpenAPI.NET/blob/main/docs/upgrade-guide-2.md)

## What’s New in OpenAPI.NET v3

- **Full OpenAPI v3.2 Support:** Serialization and model parity with the latest spec
- **Enhanced Media Type Support:** More robust encoding and schema handling
- **Hierarchical Tags:** Organizational improvements for tags, including kind, summary, and parent relationships
- **Security Updates:** Deprecated flag handling and device authorization flow support
- **Example Improvements:** New properties for example data values and serialization
- **Extended Parameter Support:** More locations and styles for API parameters

[Upgrade Guide for v3](https://github.com/microsoft/OpenAPI.NET/blob/main/docs/upgrade-guide-3.md)

## Impact on .NET Ecosystem

OpenAPI.NET is foundational for many widely-used .NET tools—including Swashbuckle, Semantic Kernel, NSwag, and direct .NET integrations. The updates make it easier to adopt modern API standards, improve reliability for large-scale systems, and future-proof ASP.NET Core projects (with .NET 10 moving toward native OpenAPI support).

## For Developers

- Start with [v2 upgrade guide](https://github.com/microsoft/OpenAPI.NET/blob/main/docs/upgrade-guide-2.md), then proceed to [v3 guide](https://github.com/microsoft/OpenAPI.NET/blob/main/docs/upgrade-guide-3.md)
- Performance, documentation, and reliability are all enhanced for API-centric .NET development
- The project is open source and welcomes contributions ([see contributing guide](https://github.com/microsoft/OpenAPI.NET/blob/main/CONTRIBUTING.md))

## Key Contributors

This milestone involved numerous community and internal contributors, including: Vincent Biret, Matthieu Costabello, Maggie Kimani, Adrian Obando, Darrel Miller, Irvine Sunday, Martin Costello, Safia Abdalla, Mike Kistler, and Rachit Malik.

## Feedback & Getting Involved

Contributions and issues are welcomed via [GitHub](https://github.com/microsoft/OpenAPI.NET/issues). Full details, guides, and the full changelog are available on the project's documentation.

This post appeared first on "Microsoft OpenAPI Blog". [Read the entire article here](https://devblogs.microsoft.com/openapi/openapi-net-release-announcements/)
