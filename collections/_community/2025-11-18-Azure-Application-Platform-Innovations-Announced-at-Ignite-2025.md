---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-platform-at-ignite-2025-new-innovations-for-all-your/ba-p/4470759
title: Azure Application Platform Innovations Announced at Ignite 2025
author: NagaSurendran
feed_name: Microsoft Tech Community
date: 2025-11-18 16:13:48 +00:00
tags:
- .NET Modernization
- AI Gateway
- AKS
- AKS Automatic
- API Management
- App Modernization
- Azure Application Platform
- Azure Functions
- Azure Red Hat OpenShift
- Confidential Containers
- Docker Compose
- Durable Functions
- Logic Apps
- MCP
- Microsoft Foundry
- Multi Agent Orchestration
- Semantic Kernel
section_names:
- ai
- azure
- devops
- github-copilot
- security
primary_section: github-copilot
---
NagaSurendran introduces the latest Azure app platform features announced at Microsoft Ignite 2025, showcasing how organizations can leverage unified AI and app modernization capabilities to accelerate innovation while ensuring security and scalability.<!--excerpt_end-->

# Azure Application Platform Innovations Announced at Ignite 2025

## Overview

At Microsoft Ignite 2025, Azure unveiled a range of new features and services that support organizations operating across multiple generations of application architectures—from mission-critical legacy workloads to AI-driven agentic apps. The platform's enhancements focus on unifying the AI application lifecycle, streamlining modernization, scaling globally, and securing operations.

---

## Building and Operationalizing AI Apps and Agents

### Microsoft Foundry Unification

- Microsoft Foundry now consolidates the entire AI app and agent lifecycle in one location.
- Supports model evaluation, safety, observability, deployment, and integration via MCP (Model Context Protocol).
- Enables developers to design and build agents that reason, act, and connect to diverse business systems.

### Expanded Tools Layer

- Over 1,400 business systems (SAP, Salesforce, ServiceNow, Workday) available as MCP tools through Logic Apps connectors (public preview).
- Internal REST APIs and custom tools exposed via API Management and API Center (public preview).

### Platform-Wide MCP Support

- Azure Functions can host MCP servers, providing real-time streaming.
- Policy-driven governance for MCP endpoints via Azure API Management.
- Azure API Center now functions as an enterprise MCP registry for secure publishing and reuse.

### Multi-Agent Orchestration

- Enhanced Durable Functions support multi-agent patterns and integrate with OpenAI SDK.
- Azure Container Apps enable multi-container agent deployments, simplifying dev and ops via Docker Compose.
- Logic Apps agent loop allows orchestration of iterative, goal-driven agent business processes without custom code.
- AI Gateway in API Management secures model access and enables policy controls, with agent-to-agent support.

**Customer Example:**

- AT&T built a scalable, secure agent platform with AKS and Microsoft Foundry, powering over 70 generative AI solutions for 100,000+ employees.

---

## Modernizing Architectures

### GitHub Copilot for Modernization

- Assesses legacy .NET and Java codebases, recommends modernization, generates code and IaC templates for Azure.
- New capabilities include .NET Framework-to-.NET upgrades, code-to-cloud enhancements, Java modernization, containerization assist, and SQL/PostgreSQL migration on Azure.

### Flexible Modernization Paths

- Adopting containers (AKS, Azure Container Apps), event-driven/serverless extensions, and API Management for backend modernization.
- Managed Instance on Azure App Service allows legacy dependencies in a PaaS environment.
- Azure Red Hat OpenShift with virtualization unifies VMs and containers on Kubernetes.

**Customer Example:**

- Banco Bradesco modernized with Azure Red Hat OpenShift and Microsoft Foundry, accelerating AI adoption and supporting 200+ enterprise AI initiatives.

---

## Scaling Globally for High Performance

### AKS and Global App Operations

- Zone-redundancy, global load balancing, self-healing, and multi-region orchestration via Azure Kubernetes Fleet Manager.
- API Management Premium v2 delivers low latency, unlimited included calls, and strong security for large-scale APIs.

**Customer Example:**

- Ahold Delhaize USA uses AKS for efficient global app deployment and rapid feature shipping through containerization.

---

## Security, Governance, and Automated Operations

### Platform Advancements

- AKS Automatic and managed system node pools transfer operational responsibilities to Azure, increasing reliability.
- Pod readiness SLA (99.9th percentile, <5 min) enhances app readiness.
- Azure SRE Agent brings no-code automation, diagnostics, prescriptive insights, and event-driven incident response.

### Security Enhancements

- Defender for Cloud now integrates with GitHub Advanced Security, offering threat mapping from runtime to code and AI-generated fixes through Copilot Autofix and GitHub Copilot.
- Confidential container support in Azure Red Hat OpenShift and Azure Container Apps adds hardware-level encryption.
- Centralized, policy-driven governance for every AI model interaction via API Management, Semantic Kernel, and Azure AI Search.

**Customer Example:**

- The Access Group used Azure API Management with Semantic Kernel and AI Search for secure, compliant, and scalable AI product deployment, achieving ISO 42001 certification.

---

## Getting Started

- Join Azure platform and Microsoft Foundry sessions at Ignite.
- Try hands-on labs and review new architecture guides and documentation for practical insights.

**Helpful Resources:**

- [Azure application platform](https://aka.ms/app-platform)
- [Microsoft Foundry](http://ai.azure.com/)
- [GitHub Copilot app modernization](https://learn.microsoft.com/en-us/azure/developer/github-copilot-app-modernization/overview)

## Selected Ignite Sessions Covering These Innovations

| Session | Link |
|---|---|
| BRK113: Agentic apps with Microsoft Foundry | [BRK113](https://ignite.microsoft.com/en-US/sessions/BRK113) |
| BRK103: GitHub Copilot-driven app modernization | [BRK103](https://ignite.microsoft.com/en-US/sessions/BRK103) |
| BRK104: Modernizing with Azure Red Hat OpenShift | [BRK104](https://ignite.microsoft.com/en-US/sessions/BRK104) |
| BRK110: Building AI apps fast with GitHub and Foundry | [BRK110](https://ignite.microsoft.com/en-US/sessions/BRK110) |
| BRK116: Apps, agents, and MCP | [BRK116](https://ignite.microsoft.com/en-US/sessions/BRK116) |
| BRK150: Modernizing .NET apps on Azure | [BRK150](https://ignite.microsoft.com/en-US/sessions/BRK150) |
| BRK120: Scaling Kubernetes securely | [BRK120](https://ignite.microsoft.com/en-US/sessions/BRK120) |
| BRK121: Kubernetes with AKS Automatic | [BRK121](https://ignite.microsoft.com/en-US/sessions/BRK121) |
| BRK119: Securing AI agents with API Management | [BRK119](https://ignite.microsoft.com/en-US/sessions/BRK119) |
| BRK118: Agentic integration | [BRK118](https://ignite.microsoft.com/en-US/sessions/BRK118) |
| BRK115: AI transformation across the software lifecycle | [BRK115](https://ignite.microsoft.com/en-US/sessions/BRK115) |
| THR702: Azure SRE Agent for reliability | [THR702](https://ignite.microsoft.com/en-US/sessions/THR702) |

---

For more customer stories and in-depth technical guides, explore the Azure Tech Community and Microsoft Ignite site.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-platform-at-ignite-2025-new-innovations-for-all-your/ba-p/4470759)
