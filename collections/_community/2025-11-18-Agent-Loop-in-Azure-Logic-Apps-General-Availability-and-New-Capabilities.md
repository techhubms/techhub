---
layout: "post"
title: "Agent Loop in Azure Logic Apps: General Availability and New Capabilities"
description: "This community announcement details the general availability of Agent Loop in Azure Logic Apps Standard. It covers how Agent Loop enables intelligent, multi-agent AI automation, highlights real-world customer scenarios, explains new features, security and identity controls, and describes deployment, integration, and extensibility across the Azure ecosystem. Readers will learn how businesses use Agent Loop to streamline IT operations, developer tasks, and compliance, and how to get started with workshops and demos."
author: "DivSwa"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-general-availability-of-agent-loop-in-azure-logic/ba-p/4470739"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 18:40:38 +00:00
permalink: "/2025-11-18-Agent-Loop-in-Azure-Logic-Apps-General-Availability-and-New-Capabilities.html"
categories: ["AI", "Azure", "DevOps", "Security"]
tags: ["Access Control Lists (acl)", "Agent Loop", "AI", "AI Automation", "AI Gateway", "Azure", "Azure AI Search", "Azure Logic Apps", "Community", "Conversational Agents", "Data Residency", "DevOps", "DevOps Automation", "Enterprise Security", "Incident Management", "M365 Integration", "MCP", "Microsoft Entra ID", "Multi Agent Workflows", "OAuth 2.0", "On Behalf Of Authentication", "Public Preview Features", "Python Integration", "Security", "Teams Integration", "VNET Integration", "Workflow Orchestration"]
tags_normalized: ["access control lists acl", "agent loop", "ai", "ai automation", "ai gateway", "azure", "azure ai search", "azure logic apps", "community", "conversational agents", "data residency", "devops", "devops automation", "enterprise security", "incident management", "m365 integration", "mcp", "microsoft entra id", "multi agent workflows", "oauth 2dot0", "on behalf of authentication", "public preview features", "python integration", "security", "teams integration", "vnet integration", "workflow orchestration"]
---

DivSwa announces the general availability of Agent Loop in Azure Logic Apps, providing a comprehensive overview of its AI automation features, enterprise security, and extensibility for developers and IT teams.<!--excerpt_end-->

# Agent Loop in Azure Logic Apps: General Availability and New Capabilities

Agent Loop is now generally available in Azure Logic Apps Standard, transforming Microsoft's Logic Apps platform into a scalable, intelligent multi-agent automation system. Organizations can build AI-powered agents that collaborate with automated workflows and humans, all deployed and managed using existing CI/CD pipelines and protected by enterprise-grade identity, access controls, and compliance features.

## Key Highlights

- **AI-Powered Automation:** Agent Loop enables the creation and orchestration of AI agents within business-critical workflows. Agents can act autonomously or in conversational scenarios, streamlining both IT operations and business processes.
- **Scalability and Adoption:** The platform has seen rapid adoption, with thousands of customers—including global enterprises—building agents that process billions of tokens monthly. Real customer use cases show accelerated investigation cycles, reduced manual effort, and unified access to information and approvals.

## Real-World Use Cases

### Cyderes: Security Automation

- Leveraged Agent Loop for automating security alert triage.
- Achieved 5X faster investigation cycles and substantial cost reductions.
- Enabled analysts to orchestrate their own AI agents with low-code tools to keep up with evolving cyber threats.

### Vertex Pharmaceuticals: Accelerated Knowledge Access

- Used Logic Apps and Agent Loop (VAIDA) to orchestrate multiple AI agents for searching, summarization, and analysis.
- Enabled rapid information retrieval across disparate systems, routed approvals through Teams/Outlook, and maintained compliance and multilingual support.

## Deployment Scenarios

- **Developer Productivity:** Automate code generation, testing, workflow creation, and CI/CD.
- **IT Operations:** Incident management, resource optimization, policy enforcement, ticket handling.
- **Business Processes:** Sales workflow automation, retail approvals, healthcare scheduling.
- **Stakeholder Support:** Automated planning, content generation, communications, customer service workflows.

Agent Loop is powering both customer and Microsoft internal automation teams for deployment, incident management, analytics, and operational insights.

## Core Agent Loop Features (GA)

- Support for Autonomous and Conversational agentic workflows.
- On-Behalf-Of (OBO) authentication using OAuth 2.0 for secure, per-user actions on connectors.
- Python code interpreter for dynamic data analysis and computation.
- Nested agent tooling for sophisticated workflow orchestration.
- User ACLs for fine-grained knowledge access.

### New Features (Public Preview)

- Redesigned workflow designer.
- Support for agent deployment in Logic Apps Consumption tier.
- Model Context Protocol (MCP) integration for standardized ecosystem tool access.
- Azure AI Gateway source integration for unified model governance and monitoring.
- Direct deployment in Teams and Microsoft 365; Okta integration for identity provider flexibility.

[Read more in the Announcement Blog](https://aka.ms/agentloop/igniteupdate)

## Platform Security and Compliance

- Built-in per-user actions and delegated permissions (OBO/OAuth 2.0).
- Document-level security with Microsoft Entra-based access control and Azure AI Search integration for retrieval augmentation.
- Easy Auth and Entra ID for securing chat entry and conversational endpoints.
- End-to-end identity and access control for agents, workflows, data, and endpoints.

## Extensibility and Integration

- 1,400+ connectors for SaaS, on-premises, custom API integration.
- MCP server support and A2A protocol for agent-to-agent communication.
- Flexibility to use Azure OpenAI, Azure AI Foundry, or bring-your-own models via AI Gateway.
- Comprehensive SLAs, VNET and hybrid integration, existing governance and pipeline support.

## Getting Started

- **Logic Apps Labs:** [https://aka.ms/LALabs](https://aka.ms/LALabs)
- **Agent Loop Workshop:** [https://aka.ms/la-agent-in-a-day](https://aka.ms/la-agent-in-a-day)
- **Demo Videos:** [https://aka.ms/agentloopdemos](https://aka.ms/agentloopdemos)

## Conclusion

Agent Loop in Azure Logic Apps Standard delivers advanced, enterprise-grade AI automation, extensibility, and security for business and IT workflows. Organizations can now confidently deploy agentic automation at scale across cloud, on-premises, and hybrid environments.

*For more information, read the full community announcement and check out workshops and demos to get started with Agent Loop.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-general-availability-of-agent-loop-in-azure-logic/ba-p/4470739)
