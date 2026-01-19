---
external_url: https://devblogs.microsoft.com/dotnet/announcing-dotnet-10/
title: 'Announcing .NET 10: A Major Release for Modern, Productive, and AI-Powered Development'
author: .NET Team
viewing_mode: external
feed_name: Microsoft .NET Blog
date: 2025-11-11 15:38:00 +00:00
tags:
- .NET
- .NET 10
- AI Integration
- AppHost
- ASP.NET Core
- Azure SQL
- Blazor
- C#
- C# 14
- Cosmos DB
- Entity Framework Core 10
- F#
- F# 10
- Featured
- Featured Post
- MAUI
- MCP
- Microsoft Agent Framework
- Minimal APIs
- NativeAOT
- NuGet
- OpenAPI
- Performance
- Post Quantum Cryptography
- SLNX
- VS
- VS Code
- WinForms
- WPF
section_names:
- ai
- azure
- coding
- devops
---
.NET Team announces .NET 10, highlighting key advancements in AI, coding, and development tooling. This release provides developers with improved performance, enhanced language features, and new AI integration options across the Microsoft stack.<!--excerpt_end-->

# Announcing .NET 10: A Major Release for Modern, Productive, and AI-Powered Development

**Author:** .NET Team

## Overview

.NET 10 is a landmark Long Term Support (LTS) release, bringing together another year of collaboration from the global Microsoft developer community. It delivers thousands of improvements across the .NET stack, emphasizing productivity, security, performance, and integrated AI features.

- **LTS support:** Supported through November 2028
- **Major frameworks and tools updated:** Includes ASP.NET Core, Blazor, C# 14, F# 10, MAUI, Entity Framework Core 10, and Visual Studio 2026
- **AI Integration:** Native support for building intelligent applications

## Performance and Security Enhancements

- Improved JIT compiler (better inlining, method devirtualization, struct argument handling)
- Hardware acceleration including AVX10.2, Arm64 SVE
- Enhanced NativeAOT for smaller, faster compiled apps
- Cryptography improvements (post-quantum cryptography support, AES KeyWrap with Padding)
- Optimized networking, JSON handling, and more

## Language Highlights

### C# 14

- Field-backed properties
- Extension properties and methods
- First-class Span<T> conversions
- Partial properties and constructors

### F# 10

- Scoped warning suppression
- ValueOption struct-based option parameters
- Parallel compilation and performance improvements

## AI in .NET 10

AI is central to .NET 10, with features for both simple integrations and advanced, multi-agent systems.

- **Microsoft Agent Framework:** Build intelligent, agentic AI workflows using Semantic Kernel and AutoGen foundations
- **Microsoft.Extensions.AI:** Unified abstractions for using AI across providers (Azure OpenAI, GitHub, etc.)
- **Model Context Protocol (MCP):** Standardizes agent interaction with external data and toolchains

Sample code and project templates are provided for fast starts with AI agents, with support for rich UI through AG-UI and C# client frameworks (including MAUI and Blazor).

## Framework and Tooling Advancements

### ASP.NET Core and Blazor

- Security, OpenAPI 3.1, performance, diagnostics
- Passkey (WebAuthn/FIDO2) authentication support
- Enhanced Blazor component state, forms and validation
- Observability improvements for diagnostics

### .NET MAUI

- Support for latest Android and iOS updates
- Improved XAML performance and developer experience
- Enhanced controls, media features, and platform integration

### Aspire

- Orchestrate distributed, observable, production-ready apps with integrated telemetry and resource management features
- Supports polyglot scenarios and simplified deployment to cloud and on-prem

### Entity Framework Core 10

- AI-ready features (vector search, hybrid search for AI/RAG workloads)
- Advanced JSON and complex type handling
- Improved query and data access performance
- Better Azure SQL and Cosmos DB integration

### Developer Tools

- **Visual Studio 2026:** Integrated Copilot agents for code generation, debugging, and performance analysis
- **C# Dev Kit for Visual Studio Code:** Modern C# editing, solution-less mode, improved Razor support, integrated test coverage
- **.NET SDK:** CLI enhancements for productivity, better container support, SLNX simplified solution format
- **NuGet:** Improved security audit, MCP server publishing, and vulnerability remediation tools

## Azure Integration

- First-class support for Azure SQL, Cosmos DB, AppHost orchestration, and cloud-aware deployment workflows
- Vector search and hybrid search to enable AI/ML scenarios directly on Azure data backends

## Getting Started

- Download .NET 10 from [get.dot.net/10](https://get.dot.net/10)
- Access tutorials, documentation, and videos showcasing new features and migration guidance

For in-depth details, explore the official [What’s new in .NET 10](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview) and related documentation.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/announcing-dotnet-10/)
