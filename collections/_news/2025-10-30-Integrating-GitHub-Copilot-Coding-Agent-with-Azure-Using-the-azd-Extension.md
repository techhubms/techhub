---
layout: post
title: Integrating GitHub Copilot Coding Agent with Azure Using the azd Extension
author: Kristen Womack
canonical_url: https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-copilot-coding-agent-config/
viewing_mode: external
feed_name: Microsoft Azure SDK Blog
feed_url: https://devblogs.microsoft.com/azure-sdk/feed/
date: 2025-10-30 18:29:56 +00:00
permalink: /github-copilot/news/Integrating-GitHub-Copilot-Coding-Agent-with-Azure-Using-the-azd-Extension
tags:
- .NET
- AI
- Azd
- Azure
- Azure App Service
- Azure Cosmos DB
- Azure Developer CLI
- Azure Key Vault
- Azure MCP Server
- Azure SDK
- Azure Subscription
- Azure.coding Agent Extension
- CI/CD
- Codespaces
- Copilot
- DevOps
- Docker
- Federated Credentials
- GitHub Copilot
- GitHub Copilot Coding Agent
- IaC
- Java
- JavaScript
- Kubernetes
- Managed Identity
- MCP Configuration
- News
- OpenID Connect
- Python
- RBAC
- Repository Configuration
- Security
- Typescript
- VS Code
- Workflow Automation
section_names:
- ai
- azure
- devops
- github-copilot
- security
---
Kristen Womack shares a practical walkthrough for setting up GitHub Copilot coding agent integration with Azure via the azd extension, automating secure configuration and agent access.<!--excerpt_end-->

# Integrating GitHub Copilot Coding Agent with Azure Using the azd Extension

This article outlines how developers can set up the GitHub Copilot coding agent to securely interact with Azure cloud resources. Leveraging the Azure Developer CLI (`azd`) and its `azure.coding-agent` extension, manual configuration work is automated for streamlined integration.

## Why the azd Extension for Copilot?

Connecting Copilot coding agents to Azure allows them to:

- Suggest and update infrastructure code
- Query and monitor Azure resources
- Enable AI-powered cloud app development

Manual setup typically involves configuring managed identities, role assignments, federated credentials, and GitHub repository settings. The azd extension simplifies this process.

## Key Features

- **Automated managed identity creation** with configurable Azure RBAC roles.
- **Federated credential setup** for passwordless, secure GitHub Actions authentication.
- **GitHub environment variable configuration** for Azure credentials.
- **MCP Server integration** for Model Context Protocol access.
- **Workflow automation**, generating `.github/workflows/copilot-setup-steps.yml` and creating pull requests with setup instructions.

## Prerequisites

- Install and authenticate Azure Developer CLI (`azd`)
- Azure subscription with resource group and identity creation permissions
- Local clone of the targeted GitHub repository
- GitHub CLI (`gh`) installed and authenticated

## Installation & Setup

Install or upgrade the extension:

```bash
azd extension install azure.coding-agent
azd extension upgrade azure.coding-agent
```

Configure your repository:

```bash
cd <your-github-repository>
azd coding-agent config
```

Follow the interactive prompts to:

1. Authenticate Azure and select a subscription
2. Select the repository
3. Create or select managed identity
4. Assign RBAC roles
5. Generate and push necessary workflow files
6. Copy MCP JSON configuration to Copilot agent settings

## Technical Workflow

**Managed Identity:**

- Create/select identity, specify resource group
- Configure RBAC (default Reader, fully customizable)

**Federated Credentials:**

- Setup for passwordless authentication from GitHub Actions
- OIDC credentials configured under Azure portal -> Managed Identity

**Environment Variables:**

- Set in GitHub environment (`copilot`) for Azure authentication
  - `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`

**Workflow Generation:**

- `.github/workflows/copilot-setup-steps.yml` automates setup and verification

**MCP Server Configuration:**

- Copy JSON block to Copilot’s configuration for Azure MCP server integration

## Advanced Options

- Custom RBAC roles via `--roles` flag
- Use or create custom branches/remotes via CLI flags
- Shortcut for existing managed identities

## Security Best Practices

- Least privilege: default Reader role, resource group scope
- Passwordless auth: OIDC, no GitHub secrets
- Full audit via Azure logs
- Compliance via Azure Policy integration
- Copilot agent access governed by repository write permissions

## Use Cases

- Querying and monitoring Azure resources
- Infrastructure as code generation (Bicep, ARM templates)
- Cost optimization and security insights
- AI-powered workflow automation for cloud development

## Troubleshooting

- Check managed identity roles/scopes for access issues
- Verify Copilot environment variables and federated credential subjects
- Ensure workflow files are merged to main for activation

## How to Get Involved

- Report issues and discuss on [Azure Developer CLI GitHub](https://github.com/Azure/azure-dev)
- Review/contribute to the extension source code
- Share feedback or enhancement requests via GitHub issues

## Summary

The azd extension makes it easy for developers, regardless of project scale, to connect GitHub Copilot coding agents to Azure cloud services securely and efficiently. This means faster, AI-assisted cloud development while maintaining strong identity and security controls.

**Related resources:**

- [GitHub Copilot Documentation](https://docs.github.com/copilot)
- [Model Context Protocol Spec](https://spec.modelcontextprotocol.io/)
- [Azure Managed Identity docs](https://learn.microsoft.com/azure/active-directory/managed-identities-azure-resources/)
- [Azure Developer CLI Extensions](https://github.com/Azure/azure-dev)

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-copilot-coding-agent-config/)
