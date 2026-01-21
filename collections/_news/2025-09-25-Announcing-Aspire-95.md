---
external_url: https://devblogs.microsoft.com/dotnet/announcing-dotnet-aspire-95/
title: Announcing Aspire 9.5
author: Jeffrey Fritz
viewing_mode: external
feed_name: Microsoft .NET Blog
date: 2025-09-25 17:25:00 +00:00
tags:
- .NET
- .NET 10
- .NET Aspire
- AI Integrations
- AppHost
- Aspire
- Aspire CLI
- Azure AI Foundry
- CLI
- Cloud Native
- Containers
- Dashboards
- DevTunnels
- Distributed Applications
- GitHub Models
- OpenAI
- VS
- YARP
section_names:
- ai
- coding
- devops
---
Jeffrey Fritz announces Aspire 9.5, highlighting rich new features for .NET distributed application developers, including enhanced AI integrations, streamlined DevOps workflows, and powerful CLI and dashboard improvements.<!--excerpt_end-->

# Announcing Aspire 9.5

**Author: Jeffrey Fritz**

Aspire 9.5 introduces major enhancements for developers building distributed applications with .NET Aspire. This post outlines new features, shows upgrade steps, and gives practical examples for integrating novel capabilities like AI, DevTunnels, and more.

## Key Features in Aspire 9.5

### 1. Preview: `aspire update` Command

- Simplifies upgrading Aspire installations and projects
- Automatically handles SDK/AppHost package updates and validates compatibility
- Supports multiple release channels and asks for user confirmation before making changes
- Introduced as a preview feature; feedback encouraged via [GitHub issues](https://github.com/dotnet/aspire/issues)

### 2. Enhanced CLI and AppHost Experience

- Improved Aspire CLI for a better developer workflow
- **Single-File AppHost (Preview):**
  - Supports defining distributed applications in a single `apphost.cs` file
  - Reduces startup complexity—no project file needed
  - Requires .NET SDK 10.0.100 RC1 or later and enabling feature flags
- Example:

    ```bash
    aspire config set features.singlefileAppHostEnabled true
    aspire new
    ```

    ```csharp
    var builder = DistributedApplication.CreateBuilder(args);
    builder.Build().Run();
    ```

### 3. Dashboard Usability and Telemetry Improvements

- Multi-resource logs: View all service logs together with clear color coding
- **GenAI Visualizer:** Explore and debug LLM interactions, view prompts and responses directly from the dashboard

### 4. New Integrations and AI/Productivity Enhancements

- **OpenAI Integration:** First-class, code-centric support for OpenAI and model endpoints

    ```csharp
    var apiKey = builder.AddParameter("openai-api-key", secret: true);
    var openai = builder.AddOpenAI("openai").WithApiKey(apiKey).WithEndpoint("https://api.openai.com");
    var chatModel = openai.AddModel("chat", "gpt-4o-mini");
    ```

- **Azure AI Foundry and GitHub Model Catalogs:** Strong typing and Intellisense for the latest model deployments

    ```csharp
    var gpt4 = builder.AddAzureAIFoundry("foundryresource").AddDeployment("gpt4", AIFoundryModel.OpenAI.Gpt4);
    var mistral = builder.AddGitHubModel("mistral", GitHubModel.MistralAI.MistralLarge2411);
    ```

- **Dev Tunnels Integration:** Securely expose local applications using Azure Dev Tunnels, simplified for web and mobile development scenarios

### 5. YARP Static Files Support

- Serve static files efficiently alongside API proxying
- Flexible source management for assets, reliable in both development and production
- Example routing for combining API and static file delivery via YARP

### 6. Visual Studio 2026: Distributed Call Stack Insights

- Visual Studio's new Call Stack Window integration for Aspire apps lets you trace logic and debug cross-process calls in distributed .NET systems

## Getting Started and Upgrading

- Install the Aspire CLI with Bash or PowerShell from [aspire.dev](https://aspire.dev)
- Use `aspire update` to upgrade existing projects
- Review [the upgrade guide](https://learn.microsoft.com/dotnet/aspire/get-started/upgrade-to-aspire-9) for help moving from Aspire 8.x
- Explore more with [Aspire documentation](https://learn.microsoft.com/dotnet/aspire/) and connect with the team on [GitHub](https://github.com/dotnet/aspire) or [Discord](https://aka.ms/aspire-discord)

## Summary

Aspire 9.5 delivers significant quality-of-life improvements for .NET distributed application developers. AI integrations, enhanced DevOps tooling, richer dashboard telemetry, and the flexibility of single-file configurations provide a modern, streamlined experience. Give Aspire 9.5 a try and leverage these features in your next project.

For more details and a complete changelog, read the [official Aspire 9.5 release notes](https://learn.microsoft.com/dotnet/aspire/whats-new/dotnet-aspire-9-5).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/announcing-dotnet-aspire-95/)
