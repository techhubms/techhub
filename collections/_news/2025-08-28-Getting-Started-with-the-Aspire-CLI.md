---
layout: "post"
title: "Getting Started with the Aspire CLI"
description: "This guide explores the Aspire CLI—a cross-platform tool for streamlining .NET application development and deployment. It covers installation, project creation using built-in templates, adding Azure integrations and packages, running, and publishing distributed microservices applications. The article includes practical examples and details for .NET professionals aiming to modernize their workflows."
author: "Jeffrey Fritz"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/getting-started-with-the-aspire-cli/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-08-28 17:05:00 +00:00
permalink: "/news/2025-08-28-Getting-Started-with-the-Aspire-CLI.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", ".NET Aspire", "AppHost", "Application Templates", "Aspire", "Aspire CLI", "Automation", "Azure", "Azure Integration", "CLI", "Cloud Native", "Coding", "Continuous Deployment", "Cross Platform Tools", "DevOps", "Distributed Applications", "Integration Tests", "Microservices", "News", "NuGet Packages", "Project Orchestration", "Service Defaults"]
tags_normalized: ["dotnet", "dotnet aspire", "apphost", "application templates", "aspire", "aspire cli", "automation", "azure", "azure integration", "cli", "cloud native", "coding", "continuous deployment", "cross platform tools", "devops", "distributed applications", "integration tests", "microservices", "news", "nuget packages", "project orchestration", "service defaults"]
---

Jeffrey Fritz presents an in-depth walkthrough of the Aspire CLI, illustrating its use for .NET application development—from installation to running, integrating Azure services, and publishing distributed systems.<!--excerpt_end-->

# Getting Started with the Aspire CLI

The Aspire CLI is a powerful, cross-platform tool designed to streamline the development, management, and deployment of .NET application systems. It's suitable for building anything from simple websites to enterprise-scale distributed applications leveraging microservices, queues, and databases.

## 1. Installing the Aspire CLI

Before you start, ensure you have the [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) installed.

### a. Installation as a Native Executable

- **Windows**: Run in PowerShell

  ```powershell
  Invoke-Expression "& { $(Invoke-RestMethod https://aspire.dev/install.ps1) }"
  ```

  Installs Aspire CLI to `%USERPROFILE%\.aspire\bin` (custom paths and versions are optional).

- **Unix (Linux/macOS)**: Run in terminal

  ```bash
  curl -sSL https://aspire.dev/install.sh | bash
  ```

  Installs to `$HOME/.aspire/bin` by default.

For custom options and troubleshooting, consult the [official documentation](https://learn.microsoft.com/dotnet/aspire/cli/install?tabs=windows).

### b. Installation as a .NET Global Tool

This works cross-platform. Run:

```bash
dotnet tool install -g Aspire.Cli --prerelease
```

Verify installation:

```bash
aspire --version
```

If you see a version number, installation succeeded.

## 2. Creating a New Project with Templates

To create a new project, run:

```powershell
aspire new
```

This launches an interactive setup to select templates, name your project, and specify an output folder.

**Available Templates:**

- **Starter** – Multi-project system demonstrating basics
- **AppHost & service defaults** – Orchestrated solution with defaults for distributed apps
- **AppHost** – Entry-point orchestrator only
- **Service defaults** – Projects with recommended .NET practices for microservices/websites
- **Integration tests** – Foundation for distributed application integration testing

The CLI fetches the latest templates and generates the solution structure.

![Demo Video](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2025/08/aspirecli-new.webm)

## 3. Adding Integrations and Packages

To add official integration packages (e.g., databases, messaging), use:

```powershell
aspire add
```

You can provide a package/NuGet ID directly or browse the interactive list. Example integrations:

- Azure CosmosDB
- Azure Event Hubs
- Azure Functions
- Azure Key Vault
- Azure PostgreSQL
- Azure Redis
- Azure Service Bus
- Azure SignalR

This workflow simplifies dependency management directly from your terminal.

![Integration Demo](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2025/08/aspirecli-add.webm)

## 4. Running and Debugging Your Application

To launch in development mode, use:

```powershell
aspire run
```

This command:

- Sets up Aspire environment
- Builds/starts all AppHost resources
- Launches the Aspire dashboard (view logs, metrics, diagrams)
- Displays endpoint info

Example output:

```
Dashboard: https://localhost:17178/login?t=...
Logs: .../.aspire/cli/logs/apphost-...log
```

Logs persist to disk for review.

![Run Demo](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2025/08/aspirecli-run.webm)

## 5. Publishing Your Application

The publishing feature is in preview. Use:

```powershell
aspire publish
```

This serializes resources for deployment—generating assets for Azure, Docker Compose, or Kubernetes depending on integrations.

A **deploy** command exists but is not yet functional.

## Conclusion

The Aspire CLI is designed as a one-stop workflow tool for .NET application professionals, providing streamlined processes from project creation to deployment. Source code is available [on GitHub](https://github.com/dotnet/aspire/tree/main/src/Aspire.Cli).

Install Aspire CLI today to facilitate your next .NET distributed application project.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/getting-started-with-the-aspire-cli/)
