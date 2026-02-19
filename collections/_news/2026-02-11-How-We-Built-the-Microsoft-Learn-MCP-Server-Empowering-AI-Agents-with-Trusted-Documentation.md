---
layout: "post"
title: "How We Built the Microsoft Learn MCP Server: Empowering AI Agents with Trusted Documentation"
description: "This article provides an in-depth look at the design, architecture, and operational lessons learned from building the Microsoft Learn Model Context Protocol (MCP) Server. The server supports AI agents such as GitHub Copilot and VS Code extensions by exposing trusted Microsoft Learn content via an agent-friendly protocol, offering streamlined access to documentation and code samples for coding and troubleshooting tasks."
author: "Tianqi Zhang, Eric Imasogie, Pieter de Bruin"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/engineering-at-microsoft/how-we-built-the-microsoft-learn-mcp-server/"
viewing_mode: "external"
feed_name: "Microsoft Engineering Blog"
feed_url: "https://devblogs.microsoft.com/engineering-at-microsoft/feed/"
date: 2026-02-11 19:23:31 +00:00
permalink: "/2026-02-11-How-We-Built-the-Microsoft-Learn-MCP-Server-Empowering-AI-Agents-with-Trusted-Documentation.html"
categories: ["AI", "Azure", "Coding"]
tags: ["Agent Tooling", "AI", "AI Agents", "Azure", "Azure App Service", "C#", "Code Sample Retrieval", "Coding", "Distributed Systems", "Engineering@Microsoft", "Knowledge Service", "MCP", "MCP Server", "Microsoft Learn", "News", "OpenAI", "SDK", "Semantic Search", "Streamable HTTP Transport", "Tool Design", "Vector Store"]
tags_normalized: ["agent tooling", "ai", "ai agents", "azure", "azure app service", "csharp", "code sample retrieval", "coding", "distributed systems", "engineeringatmicrosoft", "knowledge service", "mcp", "mcp server", "microsoft learn", "news", "openai", "sdk", "semantic search", "streamable http transport", "tool design", "vector store"]
---

Tianqi Zhang, Eric Imasogie, and Pieter de Bruin share insights on building the Microsoft Learn MCP Server, a solution empowering AI agents like GitHub Copilot to access and integrate up-to-date Microsoft documentation for enhanced coding assistance.<!--excerpt_end-->

# How We Built the Microsoft Learn MCP Server

**Authors:** Tianqi Zhang, Eric Imasogie, Pieter de Bruin

## Introduction

In June 2025, Microsoft launched the Learn Model Context Protocol (MCP) Server with the goal of enabling AI agents—like GitHub Copilot—to access trusted, up-to-date Microsoft Learn documentation effortlessly. This server makes it easier for agents to ground their responses using authoritative technical content, offering a more reliable developer experience.

## Why MCP and the Learn MCP Server?

Modern AI agents benefit from the Model Context Protocol (MCP), which allows dynamic tool discovery, runtime capability negotiation, result streaming, and adaptation to evolving tools. Instead of manual searching or embedding maintenance, agents connect to the Learn MCP Server, retrieve available tools, and invoke them as needed. Key clients like VS Code GitHub Copilot deliver more accurate answers for Microsoft technologies, firmly anchored in official Learn resources.

## Server-Exposed Tools

- **microsoft_docs_search**: Retrieves titles, content sections, and URLs from source articles.
- **microsoft_docs_fetch**: Fetches the full content of an article for detailed context.
- **microsoft_code_sample_search**: Enables fast retrieval of language-specific code snippets from Microsoft Learn documentation.

## Architectural Overview

- The remote MCP server is deployed in front of the Learn knowledge service and accessed via Streamable HTTP Transport.
- The official C# SDK for MCP handles session management, running on Azure App Service.
- Content is backed by a vector store (the same powering Ask Learn) for high relevance and freshness.
- The architecture leverages semantic search, Azure AI Search, OpenAI, and blob storage, ensuring up-to-date document access and robust scaling across regions.

![Schematic: AI agents invoke MCP Server to query Microsoft Learn documentation, routed through Azure, AI Search, OpenAI, and blob storage.](https://devblogs.microsoft.com/engineering-at-microsoft/wp-content/uploads/sites/72/2026/02/Gemini_Generated_Image_cout2lcout2lcout-300x164.webp)

## Key Lessons Learned

### 1. "Your API is not an MCP tool"

Design tool interfaces aligned with agent workflows, not just by exposing underlying APIs. Tool operations were distilled to align with natural "search-and-read" patterns expected by LLM agents, explicitly separating complex retrieval logic from the agent contract.

### 2. Remote Servers Behave Like Distributed Systems

Operating the MCP server brought challenges with cross-region deployment, scaling, CORS management, stateless design, and data protection. Collaborating with the MCP C# SDK team was key to adopting best practices.

### 3. Tool Descriptions Define Agent Experience

Agent behavior heavily depends on the clarity of tool descriptions. Iterative improvements to description text, based on automated evaluation, significantly influenced how and when agents invoke tools.

### 4. Tool Composition for Better Results

Chaining tools—for instance, searching first, then fetching details—yields better-grounded answers and higher-quality citations. Clients were guided to use tools in tandem for optimal results.

### 5. Defending Against Hardcoded Clients

Despite dynamic discovery, some agents hardcoded tool schemas. Renaming parameters (e.g., from 'question' to 'query') caused request failures. The team adopted defensive evolution and community guidance to reduce breakage and provided resources like MCP Interviewer for preemptive issue detection.

### 6. Data-Driven Iteration

Data revealed most usage revolved around coding and troubleshooting tasks. Changes to tool instructions and retrieval strategies were prioritized based on observed agent behavior to increase developer impact.

## Developer Impact and Next Steps

Where engineers once manually searched documentation, now AI agents can integrate MCP Server tools directly, automating the retrieval and application of relevant technical knowledge into the development workflow. The [Learn MCP Server repository](https://aka.ms/learnmcpblog/repo) provides further resources for agent configuration and ongoing improvements.

## Conclusion

The Microsoft Learn MCP Server demonstrates the power of tailored agent tool design, distributed system resilience, and data-driven continual improvement for bringing scalable, authenticated technical resources directly into AI-driven development tools.

This post appeared first on "Microsoft Engineering Blog". [Read the entire article here](https://devblogs.microsoft.com/engineering-at-microsoft/how-we-built-the-microsoft-learn-mcp-server/)
