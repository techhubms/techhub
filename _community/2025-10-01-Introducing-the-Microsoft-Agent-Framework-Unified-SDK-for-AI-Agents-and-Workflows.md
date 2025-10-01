---
layout: "post"
title: "Introducing the Microsoft Agent Framework: Unified SDK for AI Agents and Workflows"
description: "This article presents the Microsoft Agent Framework, an open-source SDK from Microsoft designed for building intelligent, multi-agent systems in .NET or Python. It merges innovations from Semantic Kernel and AutoGen with new capabilities such as graph-based workflows, checkpointing, and human-in-the-loop support. The framework emphasizes agent orchestration, extensibility, and integration with Azure OpenAI and Microsoft AI services, and offers migration guidance for existing users. The content details developer experience, installation steps, compatibility, and best practices for building scalable AI agent solutions."
author: "Lee_Stott"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-the-micrsoft-agent-framework/ba-p/4458377"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-01 14:02:09 +00:00
permalink: "/2025-10-01-Introducing-the-Microsoft-Agent-Framework-Unified-SDK-for-AI-Agents-and-Workflows.html"
categories: ["AI", "Azure", "Coding"]
tags: [".NET", "A2A SDK", "Agent Orchestration", "Agent SDK", "Agent Threads", "Agent Workflows", "AI", "AI Agents", "AutoGen", "Azure", "Azure AI", "Azure OpenAI", "Coding", "Community", "Graph Workflows", "Human in The Loop", "LLM", "MCP SDK", "Microsoft Agent Framework", "Migration Guide", "Multimodal AI", "Open Source", "Python", "Semantic Kernel"]
tags_normalized: ["dotnet", "a2a sdk", "agent orchestration", "agent sdk", "agent threads", "agent workflows", "ai", "ai agents", "autogen", "azure", "azure ai", "azure openai", "coding", "community", "graph workflows", "human in the loop", "llm", "mcp sdk", "microsoft agent framework", "migration guide", "multimodal ai", "open source", "python", "semantic kernel"]
---

Lee Stott introduces the Microsoft Agent Framework, an SDK empowering developers to build robust, multi-agent AI solutions in Python or .NET, with features for orchestration, extensibility, and workflow design.<!--excerpt_end-->

# Introducing the Microsoft Agent Framework: A Unified Foundation for AI Agents and Workflows

The Microsoft Agent Framework is an open-source SDK intended for developers building advanced AI agent systems using .NET or Python. It unifies the capabilities of Semantic Kernel and AutoGen into a single, extensible toolkit, introducing new features like graph-based workflow orchestration, checkpointing, and human-in-the-loop support.

## Why Choose the Microsoft Agent Framework?

- **Unified Platform**: Merges simplicity from AutoGen with robust features from Semantic Kernel
- **Cross-Language Support**: Use either .NET or Python when developing agent systems
- **Open Source**: Available via [GitHub](https://github.com/microsoft/agent-framework) and welcomes contributions

## Core Capabilities

### AI Agents

- Build intelligent, autonomous agents driven by large language models (LLMs) such as Azure OpenAI and Azure AI
- Supports state management through agent threads, context providers, and middleware for advanced use-cases
- Integrates easily with MCP servers and third-party tools
- Typical applications: customer support, automated workflows, code generation, research assistants, etc.

### Workflows

- Enables complex, graph-based orchestration of multi-agent systems
- Supports type-based routing, conditional logic, checkpointing, and human-in-the-loop design
- Scales from prototype use to enterprise-grade production systems

## Developer Experience

- **Installation:**
  - Python: `pip install agent-framework`
  - .NET: `dotnet add package Microsoft.Agents.AI`
- **Integration:** Works with Foundry SDK, MCP SDK, A2A SDK, and supports interop with M365 Copilot Agents
- **Learning Resources:**
  - [Microsoft Learn modules](https://learn.microsoft.com/training/paths/develop-ai-agents-on-azure/)
  - [AI Agents for Beginners](https://github.com/microsoft/ai-agents-for-beginners)
  - [AI Show demos and community](https://aka.ms/AIShow/NewAgentFramework)
  - [Azure AI Foundry Discord](https://aka.ms/foundry/discord)

## Migration and Compatibility

- Migration guides for Semantic Kernel and AutoGen users
- Backwards compatibility wherever possible
- Community involvement and support for future enhancements via open GitHub issues and PRs

## Important Considerations

- The framework is currently in public preview—community feedback welcome
- Review data sharing and compliance practices when integrating with external agents or services
- Designed for safety, scalability, and modularity

## Getting Started

- [Download the Microsoft Agent Framework](https://aka.ms/AgentFramework)
- Explore official [documentation](https://aka.ms/AgentFramework/Docs)
- Connect with the community to shape the future of AI agent development

---
*Author: Lee Stott*

*Published October 1, 2025 — Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-the-micrsoft-agent-framework/ba-p/4458377)
