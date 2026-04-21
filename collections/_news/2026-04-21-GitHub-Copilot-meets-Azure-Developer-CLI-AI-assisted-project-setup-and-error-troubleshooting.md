---
author: Kristen Womack
section_names:
- ai
- azure
- devops
- github-copilot
date: 2026-04-21 00:38:49 +00:00
external_url: https://devblogs.microsoft.com/azure-sdk/azd-copilot-integration/
tags:
- AI
- Azd
- Azd Init
- Azd Provision
- Azd Up
- Azure
- Azure Container Apps
- Azure Database For PostgreSQL
- Azure Deployment
- Azure Developer CLI
- Azure SDK
- Azure.yaml
- Bicep
- Copilot Agent
- DevOps
- GitHub CLI
- GitHub Copilot
- IaC
- MCP
- MCP Server Consent
- Microsoft.App Resource Provider
- MissingSubscriptionRegistration
- News
- OperationNotAllowed
- Quota Limits
- SkuNotAvailable
- StorageAccountAlreadyTaken
- Troubleshooting
primary_section: github-copilot
title: 'GitHub Copilot meets Azure Developer CLI: AI-assisted project setup and error troubleshooting'
feed_name: Microsoft Azure SDK Blog
---

Kristen Womack announces new Azure Developer CLI (azd) integrations with GitHub Copilot: a Copilot-powered “azd init” flow that scaffolds azure.yaml and infrastructure templates, plus in-terminal AI troubleshooting that can explain, guide, diagnose, and (with approval) fix common Azure deployment errors.<!--excerpt_end-->

# GitHub Copilot meets Azure Developer CLI: AI-assisted project setup and error troubleshooting

The Azure Developer CLI (`azd`) now integrates with GitHub Copilot in two ways:

- **AI-assisted project scaffolding** during `azd init`
- **Intelligent error troubleshooting** when `azd` commands fail

Both experiences run **in the terminal**, using Copilot to reduce manual setup and troubleshooting work.

## Prerequisites

To use these features, you need:

- **azd 1.23.11 or later**
  - Check: `azd version`
  - Update: `azd update`
- **GitHub Copilot access**
  - An active GitHub Copilot subscription (Individual, Business, or Enterprise)
- **GitHub CLI (`gh`)**
  - `azd` checks and prompts for login if needed

## Copilot-powered project setup with `azd init`

Running `azd init` now presents a **“Set up with GitHub Copilot (Preview)”** option.

When selected, Copilot analyzes your codebase and scaffolds a project setup by generating:

- `azure.yaml`
- Infrastructure templates
- Deployment configuration

The scaffolding is based on your code’s language, framework, and dependencies.

### Works for new or existing apps

- **Starting fresh**: Copilot can help create a project from scratch with the right Azure infrastructure.
- **Bringing an existing app to Azure**: Copilot can analyze the codebase and generate configuration to deploy with `azd` without rewriting or restructuring the project.

### Preflight checks and consent

Before changes are made, the flow:

- Verifies your **git working directory is clean** (to protect uncommitted work)
- Prompts for **Model Context Protocol (MCP) server tool consent** up front, so you know what Copilot can access before it starts

### How to use Copilot-powered init

```bash
azd init
# Select: "Set up with GitHub Copilot (Preview)"
```

The Copilot agent examines your project structure, proposes an `azure.yaml` configuration, and generates the necessary infrastructure files. **You review and approve** changes before anything is written to disk.

### Example: scaffolding a Node.js app

Example project:

- Express API with a `package.json`
- `src/` directory
- PostgreSQL dependency

Without Copilot, you’d typically need to:

1. Determine the right `host` type (Container App vs App Service vs Function)
2. Write an `azure.yaml` with correct `language`, `host`, and `build` settings
3. Author Bicep modules for the app, the database, and any networking

With Copilot-powered init, Copilot detects Express + PostgreSQL and generates:

- `azure.yaml`
- A Bicep module for **Azure Container Apps**
- A Bicep module for **Azure Database for PostgreSQL**

All ready for you to review.

## AI-assisted error troubleshooting

When `azd provision` or `azd up` fails, `azd` can offer an interactive Copilot-powered troubleshooting flow.

### Troubleshooting with and without Copilot

Without Copilot, you might:

1. Copy the error from the terminal
2. Search docs / Stack Overflow
3. Find the relevant fix
4. Run one or more `az` CLI commands
5. Rerun `azd` and hope it works

With Copilot, `azd` can keep that workflow in your terminal and use context about:

- Your project configuration
- The command that failed
- The error details

You can choose from:

- **Explain** — Plain-language explanation
- **Guidance** — Step-by-step instructions
- **Diagnose and Guide** — What happened, why, how to fix; optionally let Copilot apply a fix (with approval) and retry
- **Skip** — Dismiss and handle manually

### Set a default error handling behavior

You can skip the interactive prompt by setting a default:

```bash
azd config set copilot.errorHandling.category troubleshoot
```

Available values for `copilot.errorHandling.category`:

| Value | Behavior |
| --- | --- |
| `explain` | Automatically get a plain-language explanation |
| `guidance` | Automatically get step-by-step fix instructions |
| `troubleshoot` | Automatically diagnose and guide |
| `fix` | Automatically apply a fix |
| `skip` | Always skip Copilot troubleshooting |

You can also enable auto-fix and retry:

```bash
azd config set copilot.errorHandling.fix allow
```

Reset to the default interactive prompt:

```bash
azd config unset copilot.errorHandling.category
```

### Common Azure deployment errors where Copilot helps

#### `MissingSubscriptionRegistration` — resource provider not registered

Example error:

```text
ERROR: deployment failed: MissingSubscriptionRegistration: The subscription is not registered to use namespace 'Microsoft.App'.
```

Explanation: Azure requires resource providers to be registered before certain resource types can be created. If a **Container App** deployment is the first in a subscription, `Microsoft.App` may not be registered.

Copilot’s **Troubleshoot** option can register the provider and run the deployment automatically.

#### `SkuNotAvailable` / `OperationNotAllowed` — SKU or quota limits

SKU capacity error:

```text
ERROR: deployment failed: SkuNotAvailable: The requested VM size 'Standard_D2s_v3' is not available in location 'westus'.
```

Quota error variant:

```text
ERROR: deployment failed: OperationNotAllowed: Operation results in exceeding quota limits of Core. Maximum allowed: 4, Current in use: 4, Additional requested: 2.
```

Copilot can:

- Explain which SKU/quota is blocked
- Suggest alternative regions or VM sizes
- Show how to request a quota increase

#### `StorageAccountAlreadyTaken` — globally unique name collision

Example error:

```text
ERROR: deployment failed: StorageAccountAlreadyTaken: The storage account named 'myappstorage' is already taken.
```

Explanation: Storage account names must be unique across all of Azure.

Copilot can suggest updating a Bicep parameter or `azure.yaml` environment variable with a unique name (for example, appending an environment name or random suffix) and then retrying the deployment.

## What’s next

The `azd` team is working on deeper agent capabilities, including:

- Copilot-assisted infrastructure customization
- Smarter multi-service orchestration

To influence what’s built next:

- Sign up for user research: https://aka.ms/azd-user-research-signup
- File an issue or start a discussion in the repo

## Try it out

Check version:

```bash
azd version
```

Update:

```bash
azd update
```

Install instructions:

- Install azd: https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd

More details:

- Full release blog post (March 2026): https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-march-2026/

## Feedback

- Repo: https://github.com/Azure/azure-dev
- User research signup: https://aka.ms/azd-user-research-signup


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/azd-copilot-integration/)

