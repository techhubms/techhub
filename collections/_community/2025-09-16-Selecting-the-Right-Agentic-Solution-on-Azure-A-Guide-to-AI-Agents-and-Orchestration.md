---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/selecting-the-right-agentic-solution-on-azure/ba-p/4453955
title: 'Selecting the Right Agentic Solution on Azure: A Guide to AI Agents and Orchestration'
author: pranabpaul
feed_name: Microsoft Tech Community
date: 2025-09-16 00:07:04 +00:00
tags:
- Agent Infrastructure
- Agent Orchestration
- AI Workflow
- AKS
- App Service
- AutoGen
- Azure AI Agent Service
- Azure AI Foundry
- Azure Logic Apps
- Azure OpenAI Service
- C#
- CI/CD
- Java
- Knowledge Base Integration
- MS Entra ID
- Python
- Role Based Access Control
- Semantic Kernel
- AI
- Azure
- Community
section_names:
- ai
- azure
primary_section: ai
---
pranabpaul explores Azure-focused approaches for building, deploying, and orchestrating AI agentic solutions, comparing Azure AI Agent Service, Logic Apps, and developer-oriented agent orchestration frameworks.<!--excerpt_end-->

# Selecting the Right Agentic Solution on Azure: A Guide to AI Agents and Orchestration

**Author:** pranabpaul

## Introduction

Azure has become a leading platform for building, deploying, and orchestrating agentic (AI-driven) solutions. With the rise of Generative AI, organizations are looking beyond traditional APIs toward intelligent agents—capable of reasoning, context, and complex workflows. This guide outlines the options available as of September 2025, focusing on Azure-native and compatible tools, and provides clarity on when to use each.

## Key Azure Native Agentic Solutions

### 1. Azure OpenAI Assistants API (Deprecated)

- Enabled developers to build conversational agents with memory and tool usage using OpenAI models like GPT-3.5 and GPT-4.
- **Caveat:** Microsoft has deprecated v1 (and v2 is in preview); migration to Azure AI Agent Service is strongly recommended. OpenAI is retiring this API in favor of the “Response” API ([migration details](https://platform.openai.com/docs/assistants/migration)).

### 2. Azure Logic Apps with AI Agents (Preview)

- Workflow integration with agentic models, currently in public preview.
- **Strengths:** Declarative, no-code/low-code workflows that can integrate AI agents for decision-making and routing (e.g., automating visa application processes).
- Limitations include restricted model/regional availability and dependency on Logic Apps Standard.
- CI/CD supported via VS Code templates.

### 3. Azure AI Agent Service (Recommended)

- Part of Azure AI Foundry; allows for declarative provisioning and management of AI agents via UI or SDKs (Python, C#, Java).
- **Features:**
  - Connects with OpenAI and—soon—other models.
  - Integrates with Azure AI Search, SharePoint, Microsoft Fabric.
  - Supports hierarchical agent dependencies and complex workflows.
  - Offers RBAC, MS Entra ID, custom storage, and Azure Key Vault integration.
  - Agents run in isolated, Microsoft-managed containers.
- **Positioning:** Microsoft’s recommended destination for new agentic solutions; active development and investment.

### 4. Agent Orchestrators (Advanced/Developer)

- Tools: LlamaIndex, LangGraph, LangChain, and Microsoft’s own Semantic Kernel and AutoGen.
- **Who should use:** Developers needing granular control over agent creation, hosting, and orchestration without UI constraints.
- Host and run agents on Azure Kubernetes Service (AKS), App Service, etc.
- Supports complex, multi-layered agent structures.

## Decision Guidance

- **Customer Need**: Workflows requiring agent-driven automation favor Logic Apps; custom applications benefit from Agent Service or orchestrators.
- **No-Code vs Code**: Logic Apps is ideal for no-code solutions. Agent Service provides a balance, while orchestrators demand developer expertise.
- **Hosting & Maintenance**: Use Agent Service if you prefer not to maintain hosting; otherwise, orchestrators offer control at the expense of complexity.
- **Orchestration Complexity**: Agent Service for straightforward scenarios; orchestrators for advanced, custom pipelines.
- **Versioning**: Orchestrators currently offer better versioning and CI/CD clarity than Agent Service.

## Summary Table

| Solution Type                    | Best For                           | Notes                                                  |
|----------------------------------|------------------------------------|--------------------------------------------------------|
| Azure AI Agent Service           | Most scenarios                     | Declarative, managed, ongoing Microsoft investment     |
| Azure Logic Apps + Agents        | Workflow-driven, no-code           | Preview; limited models; best for process automation   |
| Agent Orchestrators (SDK-based)  | Advanced orchestration, custom CI/CD| Developer-focused; more control, but more complexity   |

## Best Practices

- **Align tool choice with project needs:** Simpler, managed services (Agent Service, Logic Apps) for most; orchestrators for expert teams.
- **Stay updated:** Azure capabilities and recommendations are evolving rapidly.
- **Consider migration:** If using deprecated APIs (e.g., Assistants API), plan migration to recommended services.
- **Security & Compliance:** Use MS Entra ID, RBAC, and Key Vault for compliant agent deployments.

## References

- [Assistants API Migration](https://platform.openai.com/docs/assistants/migration)
- Azure AI Foundry Documentation
- Microsoft Semantic Kernel, AutoGen SDKs

## Conclusion

When designing agentic solutions on Azure, stay informed of the latest guidance and invest in platforms receiving active Microsoft development—especially Azure AI Agent Service. Use Logic Apps with agentic integrations for workflow automation, and orchestrators for advanced custom scenarios. Assess your requirements in terms of code/no-code, hosting, scalability, and orchestration complexity to make the optimal choice.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/selecting-the-right-agentic-solution-on-azure/ba-p/4453955)
