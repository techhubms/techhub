---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-vibe-coding-to-working-app-how-sre-agent-completes-the/ba-p/4482000
title: 'How SRE Agent Closes the Developer Loop: Debugging and Fixing Azure Cloud App Failures with AI'
author: dchelupati
feed_name: Microsoft Tech Community
date: 2025-12-30 16:22:54 +00:00
tags:
- .NET 8
- Application Debugging
- Azure Networking
- Azure SQL Database
- Azure SRE Agent
- Bicep
- Claude Opus
- Developer Workflow
- DevOps Automation
- DNS Zone
- Entra ID
- GitHub
- GitHub Coding Agent
- GitHub MCP
- IaC
- Managed Identity
- Private Endpoint
- Resource Mapping
- Virtual Network
- VS Code Copilot
- AI
- Azure
- Coding
- DevOps
- Security
- Community
section_names:
- ai
- azure
- coding
- devops
- security
primary_section: ai
---
dchelupati demonstrates how Azure SRE Agent, in combination with Copilot and GitHub Coding Agent, enables developers to debug, document, and automatically fix complex infrastructure bugs in Azure app deployments using AI-driven workflows.<!--excerpt_end-->

# From Vibe Coding to Working App: How SRE Agent Completes the Developer Loop

**Author: dchelupati**

## The Developer’s Cloud Debugging Challenge

Deploying cloud-native applications on Azure often succeeds at the resource level but can fail at runtime with cryptic errors like `Login failed for user ''`. Identifying the true cause—spanning web apps, SQL servers, VNets, private endpoints, DNS zones, and managed identities—is challenging because these components individually appear fine, but their interactions cause integration failures invisible in the Azure portal.

## Modern AI-Driven Debug and Repair Flow

The post presents an integrated workflow utilizing:

- **VS Code Copilot Agent Mode + Claude Opus**: For writing and deploying infrastructure and application code (Bicep, .NET 8).
- **Azure SRE Agent**: Azure’s diagnostic tool that builds a knowledge graph of resources, links runtime errors to resource misconfigurations, and automates issue creation.
- **GitHub Coding Agent**: Consumes the issues identified by SRE Agent, prepares code and Bicep fix PRs, and streamlines developer review and deployment.

## Step-by-Step Breakdown

### 1. Build & Deploy

- Application: .NET 8 Web App connecting to Azure SQL via a private endpoint
- Features: Private networking (no public endpoints), Entra-only authentication, and managed identity (no credentials)
- Deploy: `azd up` for automated resource provisioning and Bicep deployment

### 2. Error Surfaces

- Health check fails: Runtime endpoint returns `{ "status": "unhealthy", "error": "Login failed for user ''", "errorType":"SqlException" }`

### 3. SRE Agent Troubleshooting

- **SRE Agent** (created in Azure): Scoped with read access to the resource group.
- *Knowledge Graph*: Maps inter-resource dependencies, visualizing logical connections between the app, SQL database, VNet, private endpoint, DNS, and managed identity.

### 4. GitHub MCP Integration

- Connects SRE Agent to the source code repository
- Enables SRE Agent to open issues with diagnostic context

### 5. Source Code Sub-Agent

- Extends SRE Agent to analyze both Bicep and app code
- Cross-references resource configuration with IaC and live app errors

### 6. Automated Analysis

- Traces error back via source, Bicep, and resource graph
- Identifies multi-layer issues through logical elimination
  - Examples:
        1. Missing private DNS zone link to VNet
        2. Managed identity not provisioned as SQL user
- SRE Agent documents root causes and suggested fixes as a GitHub issue

### 7. Fix & Redeploy

- Assign the GitHub issue to Coding Agent
- Coding Agent generates a PR with Bicep and SQL fixes:
  - Adds `virtualNetworkLinks` resource to connect DNS zone and VNet
  - Appends SQL script to add managed identity as SQL user with roles
- Review and merge PR
- Deploy the updated code & infra; application health check succeeds

## Why This Scenario Matters

- **Developers focus on app logic, not networking minutiae**: SRE Agent bridges the gap between resource state and runtime error causality
- **AI closes the loop**: From infrastructure generation to runtime troubleshooting to automated repair, the entire cycle is versioned and repeatable
- **Workflow stays in code, not the portal**: All fixes remain under source control and CI/CD, minimizing risky manual interventions

## Challenge & Community Call

Readers are encouraged to try “vibe coding” a similar app, intentionally omitting a key infrastructure link, and using SRE Agent to observe the debugging, issue-creation, and auto-remediation sequence.

## Learn More

- [Azure SRE Agent documentation](https://aka.ms/sreagent/docs)
- [Azure SRE Agent blogs](http://aka.ms/sreagent/blogs)
- [Azure SRE Agent community](https://aka.ms/sreagent/discussions)
- [Azure SRE Agent home page](http://www.azure.com/sreagent)
- [Azure SRE Agent pricing](http://aka.ms/sreagent/pricing)

---
*This workflow demonstrates a practical, code-centric path toward AI-assisted DevOps, incident response, and infrastructure lifecycle management for engineers building on Azure.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-vibe-coding-to-working-app-how-sre-agent-completes-the/ba-p/4482000)
