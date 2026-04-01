---
primary_section: github-copilot
external_url: https://devblogs.microsoft.com/all-things-azure/building-with-azure-skills/
tags:
- Agent Mode
- Agentic DevOps
- AgenticDevOps
- AI
- AI Gateway
- All Things Azure
- Application Insights
- Azd
- Azure
- Azure AI Search
- Azure API Management
- Azure App Service
- Azure Blob Storage
- Azure Container Apps
- Azure Cosmos DB
- Azure Cost Management
- Azure Data Explorer
- Azure Database For PostgreSQL Flexible Server
- Azure Developer CLI
- Azure Document Intelligence
- Azure Functions
- Azure Key Vault
- Azure MCP
- Azure MCP Server
- Azure Monitor
- Azure Plugin
- Azure Quotas
- Azure Resource Graph
- Azure Service Bus
- Azure Skills Plugin
- Azure Speech Services
- Azure Static Web Apps
- Bicep
- Copilot
- Copilot Chat
- Copilot CLI
- DevOps
- Event Hubs
- GitHub Copilot
- KQL
- Log Analytics
- Managed Identity
- MCP
- Microsoft Entra ID
- Microsoft Foundry
- Microsoft Graph
- MSAL
- News
- RBAC
- Security
- Skills
- Terraform
- Vector Search
author: Chris Harris
date: 2026-03-31 23:52:21 +00:00
section_names:
- ai
- azure
- devops
- github-copilot
- security
title: 'Building with Azure Skills: a prompt cookbook for the Azure plugin (MCP server)'
feed_name: Microsoft All Things Azure Blog
---

Chris Harris shares a practical prompt cookbook for the Azure Skills Plugin (Azure MCP server), with copy/paste prompts you can use in GitHub Copilot Chat (or Copilot CLI) to prepare, validate, deploy, troubleshoot, secure, and optimize real Azure environments.<!--excerpt_end-->

# Building with Azure Skills

*Part 3 of the Azure Skills Plugin series*

Previously: How to Install the Azure Plugin (`02-install-guide.md`)

You’ve installed the Azure Skills Plugin. The Azure MCP server is running.

You have a huge collection of tools and skills at your disposal.

So, now what do you actually *say* to it?

This post is a prompt cookbook. Every example below is a real prompt you can type into Copilot Chat (or Copilot CLI, or Claude Code) with the Azure Plugin installed. Each one triggers a specific skill and produces a concrete, actionable result – not generic advice.

> **A note on scope:** Azure is huge, and the examples below reflect that – there are a lot of them. This list is by no means comprehensive. It’s meant to give you ideas and show the range of what’s possible, not to catalog every skill or scenario. If you think of a prompt that isn’t listed here, try it anyway – you’ll probably be surprised.
>
> **Don’t have the plugin yet?** Install it first: [aka.ms/azure-plugin](https://aka.ms/azure-plugin)

## How to Read This Post

Each section covers a category of Azure work. Within each category you’ll find:

- **The skill** that activates
- **Example prompts** you can copy and use immediately
- **What happens** when you run it – what the skill does behind the scenes

You don’t need to memorize skill names. Just type naturally. The plugin matches your intent to the right skill automatically.

## Creating and Deploying Apps

The three core workflow skills—`azure-prepare`, `azure-validate`, and `azure-deploy`—cover the lifecycle from source code to production.

> **You usually don’t need to call these individually.** Agents like Copilot can automatically string together prepare → validate → deploy when you ask it to deploy your app. The individual prompts are useful when you want fine-grained control.

### Starting a New Project

**Skill:** `azure-prepare`

Example prompts:

```text
"I have a Python FastAPI app. Prepare it for deployment to Azure."
```

```text
"Create the infrastructure for a Node.js Express API on Azure Container Apps"
```

```text
"Set up this React frontend as an Azure Static Web App with a Python API backend"
```

```text
"Generate Terraform for a containerized Go microservice on Azure"
```

```text
"I want to deploy this Django app to Azure App Service - generate the Bicep and Dockerfile"
```

```text
"Prepare this project for Azure using Terraform instead of Bicep"
```

**What happens:** Copilot analyzes your project, recommends Azure services, and generates infrastructure code (Bicep or Terraform), a `Dockerfile`, and an `azure.yaml` manifest.

### Validating Before Deploy

**Skill:** `azure-validate`

Example prompts:

```text
"Validate my Azure configuration before I deploy"
```

```text
"Check if this project is ready for deployment to Azure"
```

```text
"Run preflight checks on my Bicep files"
```

```text
"Verify my Azure Functions configuration is correct"
```

**What happens:** Copilot checks infrastructure code, validates `azure.yaml`, verifies Azure CLI authentication and permissions, and reports issues that would likely break deployment.

### Deploying

**Skill:** `azure-deploy`

Example prompts:

```text
"Deploy this project to Azure"
```

```text
"Run azd up"
```

```text
"Push this to production on Azure"
```

```text
"Ship it"
```

**What happens:** Copilot orchestrates deployment via `azd`, provisions infrastructure, builds and pushes container images, deploys code, and returns the live URL.

### The One-Liner

One prompt can trigger the full pipeline:

```text
"Create and deploy this Python Flask API to Azure Container Apps"
```

Copilot chains `azure-prepare` → `azure-validate` → `azure-deploy`.

## Cost and Resource Management

### Finding Waste

**Skill:** `azure-cost-optimization`

Example prompts:

```text
"Analyze my Azure spending and find savings"
```

```text
"What orphaned resources do I have in my subscription?"
```

```text
"Are any of my VMs oversized?"
```

```text
"Show me my top 10 most expensive resources this month"
```

```text
"How much could I save with reserved instances?"
```

```text
"Find unused public IPs and unattached disks"
```

**What happens:** Copilot scans subscriptions using Cost Management APIs and utilization metrics, identifies waste (orphaned resources, oversized VMs, idle environments), and estimates savings.

### Checking Quotas

**Skill:** `azure-quotas`

Example prompts:

```text
"How many vCPUs do I have available in East US?"
```

```text
"Check my quota for GPU VMs in West US 2"
```

```text
"Will I hit any limits if I deploy 10 D4s_v5 VMs?"
```

```text
"Show my current usage vs. limits for compute in all regions"
```

**What happens:** Copilot queries quota APIs to show usage, limits, and remaining capacity.

## Finding and Exploring Resources

### Listing What You Have

**Skill:** `azure-resource-lookup`

Example prompts:

```text
"List all my Azure resource groups"
```

```text
"What container apps do I have running?"
```

```text
"Show me all storage accounts in the production resource group"
```

```text
"Find resources tagged with environment=staging"
```

```text
"How many VMs do I have across all subscriptions?"
```

```text
"List my Key Vaults"
```

**What happens:** Copilot queries Azure Resource Graph and returns filtered lists.

### Visualizing Architecture

**Skill:** `azure-resource-visualizer`

Example prompts:

```text
"Generate an architecture diagram of my resource group 'prod-rg'"
```

```text
"Visualize the resources in my production environment"
```

```text
"Show me how the resources in 'api-rg' are connected"
```

**What happens:** Copilot analyzes resource relationships and outputs a Mermaid architecture diagram.

## Choosing the Right Compute

**Skill:** `azure-compute`

Example prompts:

```text
"What VM size should I use for a Python ML training job with 64GB RAM and a GPU?"
```

```text
"Recommend the cheapest VM for a small web server"
```

```text
"Compare D-series vs E-series VMs for a memory-intensive database workload"
```

```text
"I need a burstable VM for a dev/test environment - what are my options?"
```

```text
"What's the best VM for running a large PostgreSQL database?"
```

```text
"Should I use a VM Scale Set or individual VMs for my web tier?"
```

**What happens:** Copilot recommends VM sizes, pulls pricing from the Azure Retail Prices API, and explains trade-offs.

## Debugging and Diagnostics

**Skill:** `azure-diagnostics`

Example prompts:

```text
"My container app is returning 503 errors - what's wrong?"
```

```text
"Show me the errors from my function app in the last 24 hours"
```

```text
"Why is my container app failing to pull its image?"
```

```text
"Analyze the health probe failures for my app 'checkout-api'"
```

```text
"My Azure Function keeps timing out - help me find the bottleneck"
```

```text
"What's causing cold start latency on my function app?"
```

**What happens:** Copilot runs KQL queries against Log Analytics, correlates logs/metrics, and suggests concrete fixes.

## Security, Compliance, and Identity

### RBAC and Permissions

**Skill:** `azure-rbac`

Example prompts:

```text
"What's the least-privilege role for reading blobs from a storage account?"
```

```text
"What role should I assign to a managed identity that needs to push images to ACR?"
```

```text
"Generate the CLI command to assign the Contributor role on my resource group"
```

```text
"Create a custom role definition that allows starting/stopping VMs but not deleting them"
```

**What happens:** Copilot recommends least-privilege roles and generates `az role assignment` commands plus Bicep.

### Compliance Scanning

**Skill:** `azure-compliance`

Example prompts:

```text
"Run a compliance scan on my subscription"
```

```text
"Check for expiring secrets and certificates in my Key Vaults"
```

```text
"Are any of my resources misconfigured from a security standpoint?"
```

```text
"Find resources that don't follow Azure best practices"
```

**What happens:** Copilot audits resources against best practices, checks Key Vault expirations, and flags security configuration issues.

### App Registration and Authentication

**Skill:** `entra-app-registration`

Example prompts:

```text
"Create a new Entra ID app registration for my web API"
```

```text
"Set up OAuth 2.0 authentication for my Node.js app"
```

```text
"Generate an MSAL example for a Python console app that calls Microsoft Graph"
```

```text
"Add API permissions for Microsoft Graph User.Read to my app registration"
```

**What happens:** Copilot guides app registration (redirect URIs, permissions) and generates MSAL code.

## AI and Foundry

### Building AI Apps

**Skill:** `azure-ai`

Example prompts:

```text
"Set up Azure AI Search with vector search for my documents"
```

```text
"How do I add speech-to-text to my app using Azure Speech Services?"
```

```text
"Configure hybrid search with semantic ranking on my search index"
```

```text
"Set up Azure Document Intelligence to extract data from invoices"
```

**What happens:** Copilot generates SDK setup, configuration, and working code for Azure AI services.

### Foundry Agent Workflows

**Skill:** `microsoft-foundry`

Example prompts:

```text
"Deploy a GPT-4o model through Microsoft Foundry"
```

```text
"Create a new Foundry agent that can answer questions about my product docs"
```

```text
"Run an evaluation on my agent using a test dataset"
```

```text
"What AI models are available in the Foundry catalog?"
```

**What happens:** Copilot manages model deployment, agent creation, evaluations, and projects using the Foundry MCP Server.

### API Gateway for AI

**Skill:** `azure-aigateway`

Example prompts:

```text
"Set up Azure API Management as an AI Gateway in front of my Azure OpenAI models"
```

```text
"Add semantic caching and token rate limiting to my AI endpoint"
```

```text
"Configure content safety and jailbreak detection on my AI gateway"
```

**What happens:** Copilot configures API Management with AI-oriented policies (caching, token limits, content safety, load balancing).

## Migration

**Skill:** `azure-cloud-migrate`

Example prompts:

```text
"Assess my AWS Lambda functions for migration to Azure Functions"
```

```text
"Migrate this AWS CDK stack to Azure Bicep"
```

```text
"Generate a migration readiness report for moving from AWS to Azure"
```

```text
"Convert this S3 + Lambda architecture to Azure equivalents"
```

**What happens:** Copilot generates migration assessment reports, maps cloud services to Azure equivalents, and converts infrastructure code.

## Data and Observability

### Application Insights

**Skill:** `appinsights-instrumentation`

Example prompts:

```text
"Add Application Insights telemetry to my Express.js API"
```

```text
"Set up distributed tracing for my microservices"
```

```text
"What telemetry should I collect for a Python Flask app?"
```

**What happens:** Copilot generates instrumentation setup and telemetry patterns for Application Insights.

### Kusto / Azure Data Explorer

**Skill:** `azure-kusto`

Example prompts:

```text
"Write a KQL query to find the top 10 slowest API requests in the last hour"
```

```text
"Analyze my IoT telemetry data for anomalies in the last 7 days"
```

```text
"Query my ADX cluster for time-series data on sensor readings"
```

**What happens:** Copilot generates (and can run) KQL queries against Azure Data Explorer.

## Storage and Messaging

### Storage

**Skill:** `azure-storage`

Example prompts:

```text
"Upload files to Azure Blob Storage from my Node.js app"
```

```text
"Set up lifecycle management to move old blobs to cool storage"
```

```text
"Create an Azure File Share for my container app"
```

```text
"What's the difference between hot, cool, and archive storage tiers?"
```

**What happens:** Copilot generates code for storage operations and configures lifecycle/access tiers.

### Messaging

**Skill:** `azure-messaging`

Example prompts:

```text
"My Event Hub consumer is disconnecting - help me troubleshoot"
```

```text
"Set up a Service Bus queue with dead-letter handling in Python"
```

```text
"Why am I getting AMQP errors on my Event Hub connection?"
```

**What happens:** Copilot troubleshoots Azure messaging SDK issues and generates producer/consumer code.

## Databases

### PostgreSQL

**Skill:** `azure-postgres`

Example prompts:

```text
"Create a new Azure PostgreSQL Flexible Server"
```

```text
"Set up passwordless authentication with Entra ID for my Postgres database"
```

```text
"Migrate from password-based auth to managed identity for my PostgreSQL connection"
```

**What happens:** Copilot provisions PostgreSQL Flexible Server and configures passwordless auth using Entra ID managed identities.

### Cosmos DB

**Skill:** `cosmosdb-best-practices`

Example prompts:

```text
"Optimize my Cosmos DB queries - they're running slow"
```

```text
"What's the best partition key for a multi-tenant SaaS application?"
```

```text
"Review my Cosmos DB data model for performance issues"
```

**What happens:** Copilot applies Cosmos DB best practices around partitioning, indexing, SDK usage, and query optimization.

## Infrastructure and DevOps

### Copilot SDK Apps

**Skill:** `azure-hosted-copilot-sdk`

Example prompts:

```text
"Build a GitHub Copilot SDK app and deploy it to Azure"
```

```text
"Set up a Copilot-powered chat app that uses Azure OpenAI as the model backend"
```

```text
"Scaffold a Copilot SDK project with BYOM (bring your own model) support"
```

**What happens:** Copilot scaffolds a Copilot SDK app (session management, model configuration) plus Azure deployment infrastructure.

## Prompting Tips

### Be Specific About Your Stack

More context helps.

| Vague | Better |
| --- | --- |
| “Deploy my app” | “Deploy my Python FastAPI app to Azure Container Apps” |
| “Fix my Azure errors” | “My container app `checkout-api` is returning 503s since 6 AM” |
| “Set up a database” | “Create a PostgreSQL Flexible Server with Entra ID auth for my Node.js API” |

### Name Your Resources

Example:

```text
"Show me errors from the container app 'payments-api' in the last 4 hours"
```

### Chain Prompts for Complex Workflows

```text
1. "What Azure services would I need for this project?"
2. "Prepare it for Azure Container Apps with a PostgreSQL backend"
3. "Validate the configuration"
4. "Deploy it"
5. "Set up Application Insights monitoring"
6. "Run a cost optimization scan"
```

### Ask About Trade-offs

```text
"Should I use Container Apps or App Service for this API?"
```

```text
"What's the trade-off between Bicep and Terraform for this project?"
```

```text
"Is a VM Scale Set or AKS better for my autoscaling needs?"
```

## Quick Reference: Skills × Prompts

| Skill | Trigger Example | Category |
| --- | --- | --- |
| `azure-prepare` | “Prepare this for Azure” | Deploy |
| `azure-validate` | “Validate my config” | Deploy |
| `azure-deploy` | “Deploy to Azure” | Deploy |
| `azure-cost-optimization` | “Find Azure savings” | Cost |
| `azure-quotas` | “Check my vCPU quota” | Cost |
| `azure-resource-lookup` | “List my storage accounts” | Explore |
| `azure-resource-visualizer` | “Diagram my resource group” | Explore |
| `azure-compute` | “Recommend a VM size” | Compute |
| `azure-diagnostics` | “Debug my container app” | Ops |
| `azure-rbac` | “Least-privilege role for blobs” | Security |
| `azure-compliance` | “Run a compliance scan” | Security |
| `entra-app-registration` | “Create an app registration” | Security |
| `azure-ai` | “Set up vector search” | AI |
| `microsoft-foundry` | “Deploy a model in Foundry” | AI |
| `azure-aigateway` | “Add AI gateway policies” | AI |
| `azure-cloud-migrate` | “Migrate from AWS” | Migration |
| `appinsights-instrumentation` | “Add App Insights to my app” | Observability |
| `azure-kusto` | “Write a KQL query” | Data |
| `azure-storage` | “Upload to Blob Storage” | Storage |
| `azure-messaging` | “Troubleshoot Event Hub” | Messaging |
| `azure-postgres` | “Passwordless Postgres auth” | Database |
| `cosmosdb-best-practices` | “Optimize Cosmos DB queries” | Database |
| `azure-hosted-copilot-sdk` | “Build a Copilot SDK app” | Build |

## What’s Next

A future post will dive into how skills and MCP servers work together under the hood.

## Try It Yourself

1. Install the plugin: [aka.ms/azure-plugin](https://aka.ms/azure-plugin)
2. Open Copilot Chat in **Agent mode**
3. Pick any prompt from this post and try it
4. Chain 2–3 prompts together to build a workflow

Plugin repo: [aka.ms/azure-skills](https://aka.ms/azure-skills)


[Read the entire article](https://devblogs.microsoft.com/all-things-azure/building-with-azure-skills/)

