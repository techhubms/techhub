---
layout: "post"
title: "How to Use Azure Developer CLI AI Agent Extension with Microsoft Foundry"
description: "This post explains how to streamline AI agent development and deployment using the new Azure Developer CLI (azd) AI agent extension. Readers will learn to initialize projects, provision resources, and publish agents to Microsoft Foundry from their local development environment, leveraging Infrastructure as Code, secure authentication, and automated workflows. The walkthrough includes practical steps for setup, configuration, deployment, and testing, plus advanced customization options and links to documentation and sample repositories."
author: "Kristen Womack, Jeff Omhover"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-foundry-agent-extension/"
viewing_mode: "external"
feed_name: "Microsoft Azure SDK Blog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-11-19 17:22:31 +00:00
permalink: "/2025-11-19-How-to-Use-Azure-Developer-CLI-AI-Agent-Extension-with-Microsoft-Foundry.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: [".NET", "Agent Initialization", "Agents", "AI", "AI Agent", "Azd", "Azure", "Azure Developer CLI", "Azure Resource Manager", "Azure SDK", "Bicep", "CI/CD", "Codespaces", "Coding", "Container Registry", "Declarative Configuration", "DevOps", "Docker", "Environment Variables", "GitHub", "IaC", "Java", "JavaScript", "Kubernetes", "Managed Identity", "Microsoft Foundry", "News", "Project Scaffolding", "Python", "Typescript", "VS Code"]
tags_normalized: ["dotnet", "agent initialization", "agents", "ai", "ai agent", "azd", "azure", "azure developer cli", "azure resource manager", "azure sdk", "bicep", "cislashcd", "codespaces", "coding", "container registry", "declarative configuration", "devops", "docker", "environment variables", "github", "iac", "java", "javascript", "kubernetes", "managed identity", "microsoft foundry", "news", "project scaffolding", "python", "typescript", "vs code"]
---

Kristen Womack and Jeff Omhover guide you through using the new Azure Developer CLI AI agent extension for rapid development, provisioning, and deployment of Microsoft Foundry agents with secure, automated workflows.<!--excerpt_end-->

# How to Use Azure Developer CLI AI Agent Extension with Microsoft Foundry

Authors: Kristen Womack, Jeff Omhover

This guide walks through developing and deploying AI agents using the new Azure Developer CLI (azd) AI agent extension, designed to simplify workflows for building, provisioning, and publishing agents to Microsoft Foundry.

## Overview

The Azure Developer CLI (`azd`) AI agent extension lets you:

- Scaffold new agent projects
- Provision cloud resources and deploy models automatically
- Package, publish, and manage AI agents with integrated security features

It bridges local development and cloud deployment with minimal manual configuration.

## Core Features

1. **Project Initialization**: Start with templates including Infrastructure as Code (IaC) files and manifest-based agent definitions.
2. **Declarative Configuration**: Use a single `azure.yaml` to define services, resources, and model deployments—making infrastructure version-controlled and repeatable.
3. **Provisioning and Deployment**: Automatic container builds, registry pushes, and resource deployment with `azd up`.
4. **Agent Definition Management**: Fetch agent definitions from GitHub or local paths, update project configuration, and environment variables.
5. **Integrated Security**: Managed identity is set up by default for secure resource access; Azure AD authentication is used for endpoints.

## Step-by-Step Workflow

### Prerequisites

- Install [Azure Developer CLI (azd)](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd) v1.21.3+
- Authenticate via `azd auth login`
- Install `azd ai agent` extension (`azd extension install azure.ai.agents`)
- Have an Azure subscription and Azure CLI

### 1. Initialize a Project

```bash
azd init -t Azure-Samples/azd-ai-starter-basic
```

This clones the template, sets up the directory structure (`infra/` for IaC, `src/` for agent code), and generates an environment-specific `.env`.

### 2. Configure Your Agent

- Edit `azure.yaml` for declarative service/resource configuration
- Add your agent definition with:

```bash
azd ai agent init -m <agent-definition-url>
```

For example:

```bash
azd ai agent init -m https://github.com/azure-ai-foundry/foundry-samples/blob/main/samples/microsoft/python/getting-started-agents/hosted-agents/calculator-agent/agent.yaml
```

The CLI updates both `azure.yaml` and `.env` for your agent and environment variable values.

### 3. Provision and Deploy

Trigger the full workflow with:

```bash
azd up
```

This automates these steps:

- Creates the Foundry project and required Azure resources
- Deploys models specified in your configuration
- Packages agent code into a Docker image and pushes to Azure Container Registry
- Publishes the agent as a callable endpoint in Foundry

Links to endpoints and the agent playground portal are provided after deployment.

### 4. Test Your Agent

- Use the Foundry portal to find your deployed agent and send test queries via the playground interface.
- Example query: “Summarize your capabilities.”

## Under the Hood

- **IaC Files (Bicep)**: Define all resources and model deployments
- **Managed Identity and Role Assignments**: Secure access and resource authorization
- **Declarative Agent Manifest**: Specify agent logic, supported protocols, sample queries, and environment variables
- **Environment Configuration**: Customize variables for different environments (`dev`, `test`, `prod`)

## Advanced Scenarios

- Multiple agents and models provisioned from one configuration (scale, roll back, version easily)
- Custom model deployments and environment variable management
- CI/CD pipeline integration using `azd provision` and `azd deploy`

## Use Cases

- Conversational AI assistants (multi-model reasoning, integrated evaluations)
- Data analysis/insight agents (access databases, generate reports)
- Multi-agent systems (agent orchestration, dependency management)
- Enterprise agent blueprints (standardization for large teams)

## Community & Resources

- [Sample Python agents](https://aka.ms/hosted-agent-samples/python)
- [Sample .NET agents](https://aka.ms/hosted-agent-samples/csharp)
- [Agent Framework repository](https://github.com/microsoft/agent-framework)
- [Foundry Documentation](https://learn.microsoft.com/azure/ai-foundry/)
- [Azure Developer CLI GitHub discussions](https://github.com/Azure/azure-dev/discussions)

## Conclusion

The Azure Developer CLI AI agent extension and Microsoft Foundry together provide a streamlined path to developing, deploying, and managing production-grade AI agents—letting you focus on building solutions while security, provisioning, and deployment are automated.

Happy building!

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-foundry-agent-extension/)
