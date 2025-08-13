---
layout: "post"
title: "Azure MCP Server – May 2025 Release: New Integrations and Features"
description: "This post by Rohit Ganguly details the May 2025 updates to the Azure MCP Server, highlighting new integrations with Azure AI Search, PostgreSQL, Key Vault, Data Explorer, and Service Bus. It also introduces fresh documentation, developer tools, and a best practices tool in collaboration with GitHub Copilot for Azure."
author: "Rohit Ganguly"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-mcp-server-may-2025-release/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-05-20 19:40:20 +00:00
permalink: "/2025-05-20-Azure-MCP-Server-May-2025-Release-New-Integrations-and-Features.html"
categories: ["AI", "Azure", "Coding", "ML", "DevOps", "GitHub Copilot", "Security"]
tags: [".NET", "Agents", "AI", "Azure", "Azure AI Search", "Azure Data Explorer", "Azure Key Vault", "Azure MCP Server", "Azure SDK", "Azure Service Bus", "Cloud", "Coding", "Data Analytics", "Developer Best Practices", "DevOps", "Documentation", "GitHub Copilot", "GitHub Copilot For Azure", "Integrations", "Kusto", "MCP", "ML", "News", "PostgreSQL", "Python", "Security", "Visual Studio Code"]
tags_normalized: ["net", "agents", "ai", "azure", "azure ai search", "azure data explorer", "azure key vault", "azure mcp server", "azure sdk", "azure service bus", "cloud", "coding", "data analytics", "developer best practices", "devops", "documentation", "github copilot", "github copilot for azure", "integrations", "kusto", "mcp", "ml", "news", "postgresql", "python", "security", "visual studio code"]
---

Rohit Ganguly presents the May 2025 release of Azure MCP Server, focusing on new service integrations, improved documentation, and tools for developers, including collaboration with GitHub Copilot for Azure.<!--excerpt_end-->

## Azure MCP Server – May 2025 Release

**Author:** Rohit Ganguly

### Overview

The Azure MCP (Model Context Protocol) Server continues to evolve with the May 2025 release, introducing integrations across various Azure services and new tools for developers. This update builds on the momentum from public preview, expanding the server’s capabilities and documentation for easier adoption.

### New Microsoft Learn Documentation

A comprehensive [documentation hub](https://learn.microsoft.com/azure/developer/azure-mcp-server/) is now live on Microsoft Learn. It covers:

- Getting started guides for Visual Studio Code
- Tutorials for .NET and Python MCP libraries
- Ongoing updates and new walkthroughs to support the Azure MCP Server ecosystem

### Major Service Integrations

#### Azure AI Search

- Semantic search, NLP, and vector similarity for intelligent apps
- Capabilities:
  - List AI Search Services
  - List, describe, query, and search indexes

#### Azure Database for PostgreSQL

- Managed PostgreSQL with high availability and enterprise security
- Capabilities:
  - List flexible servers per resource group
  - Retrieve server configurations (version, compute, storage, backup)
  - Access specific server parameters
  - List databases and tables
  - Access table schema info
  - Execute read queries
  
[See more details in this related blog post.](https://aka.ms/azure-mcp-postgresql)

#### Azure Key Vault – Keys

- Secure management of cloud secrets, keys, and certificates
- Capabilities:
  - List Keys
  - Get Keys
  - Create Keys

#### Azure Data Explorer (Kusto)

- Managed service for large-scale data analytics
- Capabilities:
  - List Kusto clusters
  - Access cluster, database, and table details
  - Retrieve schemas
  - Run Kusto Query Language (KQL) queries
  - Sample table data

#### Azure Service Bus

- Reliable managed messaging between distributed apps
- Capabilities:
  - Peek messages in queues and topic subscriptions
  - Get runtime details for queues, topics, and subscriptions

### Developer Tools and Best Practices

- Bug fixes and tooling improvements enhance stability and usability
- New Azure Developer Best Practices Tool (in collaboration with GitHub Copilot for Azure):
  - Provides agent guidance for more efficient code generation
  - Freely available via [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azure-github-copilot)

### Summary

The Azure MCP Server continues to broaden its reach with stronger service integrations, improved documentation, and collaboration across Microsoft Azure teams.

- Access the [Azure MCP Server on GitHub](https://github.com/Azure/azure-mcp/)
- Explore the [Microsoft Learn documentation](https://learn.microsoft.com/azure/developer/azure-mcp-server/)

Stay tuned for further updates as the platform advances to support more intelligent agent workloads across Azure.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-mcp-server-may-2025-release/)
