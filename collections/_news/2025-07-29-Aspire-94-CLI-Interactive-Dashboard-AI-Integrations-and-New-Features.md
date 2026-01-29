---
external_url: https://devblogs.microsoft.com/dotnet/announcing-aspire-9-4/
title: 'Aspire 9.4: CLI, Interactive Dashboard, AI Integrations, and New Features'
author: Maddy Montaquila
feed_name: Microsoft .NET Blog
date: 2025-07-29 18:05:00 +00:00
tags:
- .NET
- .NET Aspire
- AOT Compilation
- Apphost
- Aspire
- Aspire 9.4
- Azure AI Foundry
- CLI
- Custom Resource
- Dashboard
- Developer Productivity
- External Service
- Featured
- GitHub Models
- Interaction Service
- OpenTelemetry
- YARP Integration
- AI
- Azure
- Coding
- News
section_names:
- ai
- azure
- coding
primary_section: ai
---
Maddy Montaquila introduces the Aspire 9.4 release, showcasing new CLI capabilities, interactive dashboard features, and first-class AI service integrations. Discover what's new for developing modern .NET distributed and AI-enabled applications with enhanced productivity.<!--excerpt_end-->

## Aspire 9.4 Release Overview

Today, .NET Aspire 9.4 is released, described as the biggest release so far for the platform. It introduces a suite of new features including a general-availability (GA) command-line interface (CLI), interactive dashboard elements, and integrations for AI-based workflows. The release comes alongside the publication of Aspire's first roadmap, outlining upcoming enhancements.

### Key Highlights

- **Aspire CLI (GA):**
  - Provides a fast, scriptable, and consistent interface for scaffolding, running, and configuring apps.
  - Core commands:
    - `aspire new`: Select templates to start new projects.
    - `aspire add`: Add Aspire hosting integrations from anywhere in a repo.
    - `aspire run`: Run the entire application stack from any terminal or editor.
    - `aspire config`: Manage CLI configurations and feature flags at local or global scope.
  - Updated `aspire publish` (preview) and beta commands `exec` (run CLI tools) and `deploy` (deploy full stack).
  - Native AOT-compiled for performance.
  - Can be installed via Bash or PowerShell scripts:
    - Bash: `curl -sSL https://aspire.dev/install.sh | bash`
    - PowerShell: `iex "& { $(irm https://aspire.dev/install.ps1) }"`

- **Interactive Dashboard Features:**
  - New eventing APIs and an extensibility point via the Interaction Service.
  - Custom UX allows obtaining user input during app runtime, presenting notifications, and confirmations.
  - Supports input types: Text, SecretText (masked), Number, Choice (dropdown), Boolean (checkbox).
  - Interaction service supports both dashboard and CLI inputs (e.g., during `publish` and `deploy`).
  - Feature is in preview with ongoing refinement based on community feedback.

- **Built-in Prompting for Parameters:**
  - Automatically collects missing parameter values through dashboard prompts.
  - Reduces the need for per-developer config files.
  - Supports rich markdown in prompts and user secrets for secure storage.

### AI and Cloud-Native Integrations

- **Easy AI Development:**
  - Aspire 9.4 introduces integrations for:
    - [GitHub Models (Preview)](https://learn.microsoft.com/dotnet/aspire/github/github-models-integration)
    - [Azure AI Foundry (Preview)](https://learn.microsoft.com/dotnet/aspire/azureai/azureai-foundry-integration)
  - Define AI models in the apphost and run or deploy models directly for development and debugging.
  - Seamless setup with [Azure AI Inference (Preview)](https://learn.microsoft.com/dotnet/aspire/azureai/azureai-inference-integration).
  - Example code to define and deploy an AI model:

    ```csharp
    // AppHost.cs
    var ai = builder.AddAzureAIFoundry("ai");
    var embedding = ai.AddDeployment(
        name: "text-embedding",
        modelName: "text-embedding-3-small",
        modelVersion: "1",
        format: "OpenAI")
        .WithProperties(d => { d.SkuCapacity = 20; });
    ```

- **External Service and YARP Integration:**
  - The new `AddExternalService()` API models any URL or endpoint as a first-class resource, allowing health monitoring, configuration, and orchestration like internal services.

    ```csharp
    var externalApi = builder.AddExternalService("resource-name", "https://api.example.com");
    var frontend = builder.AddNpmApp("frontend", "../MyJSCodeDirectory")
        .WithReference(externalApi);
    ```

  - YARP (Yet Another Reverse Proxy) integration enhanced with fluent APIs to manage configuration programmatically in C#.

### Getting Started with Aspire 9.4

- Review the complete feature list in the [What’s New](https://learn.microsoft.com/dotnet/aspire/whats-new/dotnet-aspire-9.4) documentation.
- Upgrade by updating your `AppHost.csproj` SDK and NuGet package references:

  ```xml
  <!-- SDK version -->
  <Sdk Name="Aspire.AppHost.Sdk" Version="9.4.0" />

  <!-- NuGet package references -->
  <PackageReference Include="Aspire.Hosting.AppHost" Version="9.4.0" />
  ```

- Community contributors helped drive updates and feedback is welcomed via the [Aspire GitHub](https://github.com/dotnet/aspire) and [Discord server](https://aka.ms/aspire-discord).

---

*This article includes AI-assisted content and was reviewed and revised by the author. For more details, refer to the principles for AI-generated content.*

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/announcing-aspire-9-4/)
