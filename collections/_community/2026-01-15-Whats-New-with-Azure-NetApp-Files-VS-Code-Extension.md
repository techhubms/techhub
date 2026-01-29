---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/what-s-new-with-azure-netapp-files-vs-code-extension/ba-p/4485989
title: What's New with Azure NetApp Files VS Code Extension
author: GeertVanTeylingen
feed_name: Microsoft Tech Community
date: 2026-01-15 21:04:04 +00:00
tags:
- AI Powered Commands
- ARM Templates
- Azure API
- Azure NetApp Files
- C#
- Cloud Native Applications
- Cloud Storage
- DevOps Tools
- JavaScript
- Microsoft Entra ID
- Mount Code Generation
- Multi Tenant
- NFSv3
- NFSv4.1
- Python
- Resource Management
- TypeScript
- VS Code
- VS Code Extension
- YAML
- AI
- Azure
- Coding
- DevOps
- Community
section_names:
- ai
- azure
- coding
- devops
primary_section: ai
---
GeertVanTeylingen explores the new capabilities in the Azure NetApp Files VS Code Extension, focusing on AI-driven productivity boosts, multi-tenant Azure management, and streamlined storage workflows for developers.<!--excerpt_end-->

# What's New with Azure NetApp Files VS Code Extension

The Azure NetApp Files (ANF) VS Code Extension v1.1.0 delivers feature-rich enhancements for developers and DevOps engineers working in cloud-native environments. This release introduces multi-tenant Azure management, context-aware mount code generation, and AI-powered automation, all accessible directly within Visual Studio Code.

## Key Highlights

- **Multi-tenant support:** Seamlessly manage Azure NetApp Files resources across multiple Azure tenants and subscriptions without frequent signouts or context switching. Analyze, provision, and inspect storage resources from a unified VS Code workspace, ensuring alignment with security and compliance requirements.
- **AI-powered commands:** Use AI-assisted workflows to generate ARM templates, receive optimization recommendations, and automate storage operations. These intelligent features boost productivity by embedding guidance and best practices into VS Code processes.
- **Context-aware mount code generation:** Integrates right-click code generation for mounting Azure NetApp Files volumes. The extension auto-detects the file type (e.g., Python, JavaScript, TypeScript, C#, Java, YAML) and inserts ready-to-run mount code according to the active language and chosen protocol (NFSv3/NFSv4.1).

## Multi-Tenant Management

Managing enterprise-grade cloud storage often involves multiple Azure tenants and subscriptions. With the upgraded extension:

- Work across all tenants and subscriptions from a single session
- Centralize analysis and management of NetApp volumes for performance, data protection, and compliance
- View and optimize cost, usage, and service tiers (Standard/Premium/Ultra) across environments

## Context-Aware Code Generation

Accelerate app development with language-aware workflows:

- Right-click an Azure NetApp Files volume in the VS Code explorer to insert a mount command tailored to your active file’s language
- Supported formats include Python (.py), JavaScript (.js), TypeScript (.ts), C# (.cs), Java (.java), and YAML
- Protocol options (NFSv3, NFSv4.1) are auto-detected and incorporated in the generated code
- Avoid context-switching: code is pasted inline and language-specific, letting you continue developing without interruptions

## Workflow Overview

1. **Authentication** with Microsoft Entra ID for secure, seamless integration with Azure tenants and subscriptions
2. **Resource selection**: Pick NetApp accounts, capacity pools, and volumes through intuitive quick-pick lists
3. **Protocol & language matching**: Choose mounting protocol and auto-generate code matching your application's language
4. **Guided insertion**: The snippet is inserted at your cursor for immediate use; unsupported file types yield clear guidance

## Getting Started with v1.1.0

- Install or update the Azure NetApp Files VS Code Extension from the [Visual Studio Code Marketplace](https://marketplace.visualstudio.com/items?itemName=NetApp.anf-vscode-extension)
- Sign in using Microsoft Entra ID and connect your Azure tenants and subscriptions
- Explore resources in the Azure NetApp Files Explorer
- Open any supported language file and use "Insert mount command" for instant mount code generation

## Resources and Further Reading

- [Quick Start Guide & Documentation](https://github.com/NetApp/anf-vscode-extension/blob/main/ANF-Extension-Quick-Start-Guide.pdf)
- [Azure NetApp Files Storage Templates](https://github.com/NetApp/azure-netapp-files-storage)
- [Accelerating Cloud-Native Development with AI-powered Azure NetApp Files VS Code](https://techcommunity.microsoft.com/blog/azurearchitectureblog/accelerating-cloud-native-development-with-ai-powered-azure-netapp-files-vs-code/4464852)
- For questions: [1P_ProductGrowth@netapp.com](mailto:1P_ProductGrowth@netapp.com)

## About the Authors

**GeertVanTeylingen** and co-authors Prabu Arjunan, Sagar Gupta, and Nitya Gupta contributed to this release and its documentation, providing valuable insights into the technical implementation and developer workflow optimizations for Azure NetApp Files.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/what-s-new-with-azure-netapp-files-vs-code-extension/ba-p/4485989)
