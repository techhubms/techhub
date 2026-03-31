---
date: 2026-03-31 03:31:29 +00:00
primary_section: github-copilot
section_names:
- ai
- azure
- devops
- github-copilot
author: PuiChee (PC) Chan
tags:
- .NET
- ACR
- AI
- AI Agents
- Application Insights
- ARM
- Azd
- Azure
- Azure AI Foundry
- Azure Container Apps
- Azure Container Registry
- Azure Developer CLI
- Azure Functions
- Azure Key Vault
- Azure Resource Manager
- Azure SDK
- Azure.yaml
- Bicep
- CI/CD
- Codespaces
- Container App Jobs
- Dev Containers
- DevOps
- Docker
- GitHub Actions
- GitHub Copilot
- Go 1.26
- gRPC
- Java
- JavaScript
- Kubernetes
- MCP
- News
- Pnpm
- Podman
- Pyproject.toml
- Python
- RBAC
- Typescript
- VS Code
- Yarn
feed_name: Microsoft Azure SDK Blog
title: 'Azure Developer CLI (azd) – March 2026: Run and Debug AI Agents Locally, GitHub Copilot Integration, & Container App Jobs'
external_url: https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-march-2026/
---

PuiChee (PC) Chan summarizes seven Azure Developer CLI (azd) releases from March 2026, covering a new AI agent extension for local run/debug and monitoring, GitHub Copilot-assisted scaffolding in `azd init`, and deployment improvements like Container App Jobs, preflight validation, and configurable timeouts.<!--excerpt_end-->

## Overview

The Azure Developer CLI (`azd`) shipped seven releases in March 2026 (1.23.7 through 1.23.13). Key updates include:

- An AI agent extension to run, invoke, monitor, and deploy agents from the terminal (local or to Microsoft Foundry)
- GitHub Copilot integration in `azd init` for AI-assisted project scaffolding and troubleshooting
- Direct deployment support for Azure Container App Jobs
- Preflight validation to catch infrastructure issues before deploying
- Better project setup and automation (package manager detection, no-prompt defaults, improved error guidance)

Release notes:

- 1.23.7: https://github.com/Azure/azure-dev/releases/tag/azure-dev-cli_1.23.7
- 1.23.8: https://github.com/Azure/azure-dev/releases/tag/azure-dev-cli_1.23.8
- 1.23.9: https://github.com/Azure/azure-dev/releases/tag/azure-dev-cli_1.23.9
- 1.23.10: https://github.com/Azure/azure-dev/releases/tag/azure-dev-cli_1.23.10
- 1.23.11: https://github.com/Azure/azure-dev/releases/tag/azure-dev-cli_1.23.11
- 1.23.12: https://github.com/Azure/azure-dev/releases/tag/azure-dev-cli_1.23.12
- 1.23.13: https://github.com/Azure/azure-dev/releases/tag/azure-dev-cli_1.23.13

Release discussion:

- March release discussion on GitHub: https://github.com/Azure/azure-dev/discussions/7347

## Highlights

- **AI agent extension**: run, invoke, monitor, and deploy agents end-to-end from your terminal to Microsoft Foundry
- **GitHub Copilot integration** in `azd init` for AI-assisted project setup
- **Deploy Container App Jobs** directly with `azd`
- **Local preflight validation** for infrastructure before deploying to Azure
- **Automatic pnpm and yarn detection** for JavaScript/TypeScript projects
- **Per-service deployment timeouts** to reduce long-running deploy failures

## New features

## AI agent extension

The `azure.ai.agents` extension adds commands for running, testing, and monitoring AI agents locally or deployed to Microsoft Foundry.

### Run and invoke

- Run an agent locally:

```bash
azd ai agent run
```

- Send messages to an agent (local or deployed):

```bash
azd ai agent invoke
```

Related posts:

- Run and test AI agents locally with azd: https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-run-invoke/

### Show and monitor

- Display container status and health:

```bash
azd ai agent show
```

- Stream container logs in real time:

```bash
azd ai agent monitor
```

Related posts:

- Debug hosted AI agents from your terminal: https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-logs-status/
- End-to-end walkthrough: From code to cloud: Deploy an AI agent to Microsoft Foundry in minutes with azd: https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-end-to-end/

## GitHub Copilot integration

GitHub Copilot plugs into `azd init` for scaffolding and troubleshooting.

### Set up with GitHub Copilot (Preview)

`azd init` now offers a **“Set up with GitHub Copilot (Preview)”** option using a Copilot agent to scaffold your project.

Behavior notes:

- Checks for a dirty working directory before modifying files
- Prompts for Model Context Protocol (MCP) server tool consent up front

Implementation references:

- https://github.com/Azure/azure-dev/pull/7162
- https://github.com/Azure/azure-dev/pull/7194

### AI-assisted error troubleshooting

When a command fails, `azd` can guide troubleshooting in multiple steps:

- Pick a category: explain, guidance, troubleshoot, or skip
- Optionally let the agent apply a fix
- Retry the failed command without leaving the terminal

Implementation reference:

- https://github.com/Azure/azure-dev/pull/7216

## Deployment and infrastructure

### Container App Jobs support

`azd` can now deploy **Azure Container App Jobs** (`Microsoft.App/jobs`) via the existing `host: containerapp` configuration.

- The Bicep template decides whether the target is a Container App or Container App Job
- No extra `azd` configuration is required

Contributed by @jongio.

Reference:

- https://github.com/Azure/azure-dev/pull/7001

### Configurable deployment timeouts

A new `--timeout` flag on `azd deploy` and `deployTimeout` in `azure.yaml` allow per-service timeouts.

Precedence order:

1. CLI `--timeout`
2. `azure.yaml` `deployTimeout`
3. Default timeout: 1200 seconds

Reference:

- https://github.com/Azure/azure-dev/pull/7045

### Remote build fallback

If `remoteBuild: true` is configured and the remote ACR build fails, `azd` falls back automatically to a local **Docker** or **Podman** build.

Reference:

- https://github.com/Azure/azure-dev/pull/7041

### Local preflight validation

`azd` now validates Bicep parameters and configuration locally before deploying to Azure, catching issues like:

- Missing parameters
- Mismatched types

Reference:

- https://github.com/Azure/azure-dev/pull/7053

### Preflight configuration

To skip ARM preflight validation:

```bash
azd config set provision.preflight off
```

Reference:

- https://github.com/Azure/azure-dev/pull/6852

### Azure SRE Agent resource type

Provisioning output now correctly displays `Microsoft.App/agents` resources.

Contributed by @dm-chelupati.

Reference:

- https://github.com/Azure/azure-dev/pull/6968

## Developer experience

### Automatic package manager detection

`azd` detects **pnpm** and **yarn** for JavaScript and TypeScript services.

Override with:

- `config.packageManager` in `azure.yaml`

Contributed by @jongio.

Reference:

- https://github.com/Azure/azure-dev/pull/6894

### pyproject.toml support (Python)

Python projects using `pyproject.toml` are now detected and built with:

```bash
pip install .
```

Contributed by @jongio.

Reference:

- https://github.com/Azure/azure-dev/pull/6848

### Local template directories

`azd init --template` accepts a local filesystem path.

Reference:

- https://github.com/Azure/azure-dev/pull/6826

### Sensible no-prompt defaults

In `--no-prompt` mode:

- `azd env new` and `azd init` auto-generate an environment name from the working directory
- Subscription auto-selected if only one is available

Reference:

- https://github.com/Azure/azure-dev/pull/7016

### Improved no-prompt error guidance

When required inputs are missing in `--no-prompt` mode, `azd init` and `azd provision` now:

- Report all missing values at once
- Provide actionable resolution commands
- Include environment variable mappings

Reference:

- https://github.com/Azure/azure-dev/pull/6962

### Build environment variables

`azd` environment variables are now available across build subprocesses including:

- Node.js
- .NET
- Java
- Python
- Static Web Apps

This enables build-time environment variable injection.

Contributed by @jongio.

Reference:

- https://github.com/Azure/azure-dev/pull/6905

### YAML-driven error handling

A new error pipeline matches deployment errors against known patterns and surfaces:

- Actionable messages
- Suggestions
- Reference links

Coverage includes (examples):

- Container App secrets
- Image pulls
- Template parameter failures
- RBAC permission/policy errors

References:

- https://github.com/Azure/azure-dev/pull/6827
- https://github.com/Azure/azure-dev/pull/6845
- https://github.com/Azure/azure-dev/pull/6846

### Remote build suggestion

When Docker isn’t installed or isn’t running, `azd` suggests setting `remoteBuild: true` for:

- Container Apps
- Azure Kubernetes Service services

Contributed by @spboyer.

Reference:

- https://github.com/Azure/azure-dev/pull/7247

### Improved auth status output

`azd auth status --output json` now:

- Exits non-zero when unauthenticated
- Includes an `expiresOn` field

Contributed by @spboyer.

Reference:

- https://github.com/Azure/azure-dev/pull/7236

## Extensions

### Extension SDK improvements

New SDK primitives for extension authoring include:

- Command scaffolding
- Typed argument parsing
- Resilient HTTP client
- Network security policy
- MCP server utilities
- Token provider, scope detection, pagination helpers

Contributed by @jongio.

References:

- https://github.com/Azure/azure-dev/pull/6856
- https://github.com/Azure/azure-dev/pull/6954

### Extension source validation

New command to validate extension registry sources:

```bash
azd extension source validate
```

Checks include required fields, version format, capabilities, and checksums.

Contributed by @jongio.

Reference:

- https://github.com/Azure/azure-dev/pull/6906

### Extension website field

Extensions can declare a `website` field that appears in `azd extension show` output.

Contributed by @jongio.

Reference:

- https://github.com/Azure/azure-dev/pull/6904

### CopilotService for extensions

A new `CopilotService` gRPC service allows extensions to interact with GitHub Copilot agent capabilities:

- Initialize sessions
- Send messages
- Retrieve usage metrics

Reference:

- https://github.com/Azure/azure-dev/pull/7172

### Improved startup failure warnings

Extension startup failures now show categorized, actionable messages and include a `--debug` hint.

Reference:

- https://github.com/Azure/azure-dev/pull/7018

## Bugs fixed

- Fixed `azd deploy` removing externally configured Dapr settings during Container App updates (now preserved when not present in the deployment YAML). https://github.com/Azure/azure-dev/pull/7062
- Reverted env-flag change from v1.23.11 to fix `-e` conflict with extension commands. https://github.com/Azure/azure-dev/pull/7274
- Fixed `.funcignore` parsing for flex-consumption function apps (UTF-8 BOM and negation patterns). https://github.com/Azure/azure-dev/pull/7223 and https://github.com/Azure/azure-dev/pull/7311
- Fixed Windows extension startup failures caused by IPv4/IPv6 mismatch in gRPC server address. https://github.com/Azure/azure-dev/pull/7346
- Hardened extension/template security (input validation, origin checks, token handling, data redaction)
- Strengthened MCP extension network security policy

## Other changes

- Faster provisioning polling via concurrent nested deployment traversal + terminal-operation cache. https://github.com/Azure/azure-dev/pull/7019
- Improved Container Apps deployment performance by reducing ARM API round-trips (up to three calls saved per deployment). https://github.com/Azure/azure-dev/pull/6888
- Improved storage blob client performance by verifying container existence once per session. https://github.com/Azure/azure-dev/pull/6912
- Improved `--no-prompt` resource-group deployments by defaulting prompt to `AZURE_RESOURCE_GROUP`. https://github.com/Azure/azure-dev/pull/7044
- Normalized CLI output to consistent lowercase `azd` branding. https://github.com/Azure/azure-dev/pull/6768
- Updated `azd` core to Go 1.26. https://github.com/Azure/azure-dev/pull/7017
- Updated Bicep minimum required version to 0.41.2. https://github.com/Azure/azure-dev/pull/6953
- Added `azure.yaml` schema metadata for IDE schema association (SchemaStore). https://github.com/Azure/azure-dev/pull/7330

## New docs

- Operate and troubleshoot agents with azd (Azure AI Foundry extension docs): https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/azure-ai-foundry-extension
- Dev container extension auto-install guidance: https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/overview
- Publishing workflows (`azd publish`): https://learn.microsoft.com/azure/developer/azure-developer-cli/publishing-workflows#scenario-build-once-deploy-everywhere
- App Service deployment slots: https://learn.microsoft.com/azure/developer/azure-developer-cli/app-service-slots
- Environment secrets (Azure Key Vault-backed values in `azd` environments): https://learn.microsoft.com/azure/developer/azure-developer-cli/environment-secrets
- Container Apps and Jobs workflows: https://learn.microsoft.com/azure/developer/azure-developer-cli/container-apps-workflows?tabs=avm-module
- Authentication and tenant guidance (including `--tenant-id`, MFA and troubleshooting): https://learn.microsoft.com/azure/developer/azure-developer-cli/provisioning-deployment-faq
- JavaScript/TypeScript packaging and package manager config: https://learn.microsoft.com/azure/developer/azure-developer-cli/azd-schema#services-properties
- Troubleshooting improvements (GitHub Actions and Key Vault permissions): https://learn.microsoft.com/azure/developer/azure-developer-cli/provisioning-deployment-faq
- Reference docs refresh: https://learn.microsoft.com/azure/developer/azure-developer-cli/reference

## New templates

- Copilot SDK Service—Chat API with React UI on Azure Container Apps (Jon Gallant): https://github.com/Azure-Samples/copilot-sdk-service
- Track Availability in Application Insights using Standard Test, Azure Function and Logic App Workflow (Ronald Bosma): https://github.com/ronaldbosma/track-availability-in-app-insights
- Copilot Studio with Azure AI Search using Azure Developer CLI (ISE Scale): https://github.com/Azure-Samples/Copilot-Studio-with-Azure-AI-Search

## New to azd?

`azd` is an open-source CLI for moving from local development to Azure using workflow-friendly commands.

- Install azd: https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd
- Template galleries:
  - Awesome azd template gallery: https://azure.github.io/awesome-azd/
  - AI App Templates: https://aka.ms/ai-gallery
- Official docs: https://learn.microsoft.com/azure/developer/azure-developer-cli/
- Troubleshooting guide: https://learn.microsoft.com/azure/developer/azure-developer-cli/troubleshoot
- GitHub repo: https://github.com/Azure/azure-dev
- User research signup: https://aka.ms/azd-user-research-signup


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-march-2026/)

