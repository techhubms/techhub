---
layout: post
title: 'Semantic Kernel and Microsoft Agent Framework: Evolution and Future Support'
author: Shawn Henry
canonical_url: https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-agent-framework/
viewing_mode: external
feed_name: Microsoft Semantic Kernel Blog
feed_url: https://devblogs.microsoft.com/semantic-kernel/feed/
date: 2025-10-08 06:51:06 +00:00
permalink: /ai/news/Semantic-Kernel-and-Microsoft-Agent-Framework-Evolution-and-Future-Support
tags:
- .NET
- Agentic AI
- AI
- AI Agents
- AI Application Development
- AI Framework
- AutoGen
- Azure
- Azure AI
- C#
- Coding
- Community Support
- Enterprise AI
- Microsoft Agent Framework
- Migration
- News
- Open Source
- Python
- Semantic Kernel
section_names:
- ai
- azure
- coding
---
Shawn Henry outlines the launch of Microsoft Agent Framework as the new foundation for building AI agents, describes its relationship to Semantic Kernel, and provides support and migration guidance for developers.<!--excerpt_end-->

# Semantic Kernel and Microsoft Agent Framework

**Author:** Shawn Henry, Product Lead – Semantic Kernel, AutoGen and Microsoft Agent Framework

## Announcement and Overview

Last week, Microsoft announced the [Microsoft Agent Framework](https://aka.ms/AgentFramework), an open-source engine designed for building agentic AI applications. This framework builds on the team’s past learnings from Semantic Kernel and AutoGen, with a focus on providing a unified, enterprise-ready platform for developing, deploying, and managing sophisticated AI agents.

- [Blog Announcement](https://devblogs.microsoft.com/foundry/introducing-microsoft-agent-framework-the-open-source-engine-for-agentic-ai-apps/)
- [Documentation](https://aka.ms/AgentFramework/Docs)
- [Demos: AI Show and Open at Microsoft](https://aka.ms/AgentFramework/AIShow)
- [Step-by-step Learning Path](https://learn.microsoft.com/en-us/training/paths/develop-ai-agents-on-azure/), [AI Agents for Beginners](https://github.com/microsoft/ai-agents-for-beginners)

## Relationship to Semantic Kernel and AutoGen

Microsoft Agent Framework is positioned as the successor to Semantic Kernel for building AI agents. It aims to provide:

- **Unified development experience** for agentic AI apps
- **Deep integration** with Microsoft and Azure ecosystems
- **Extensibility** for a range of models and tools (not limited to Microsoft)

Semantic Kernel and AutoGen informed the design of Microsoft Agent Framework. The same team is behind all three initiatives.

## Migration and Support Guidance

- Microsoft Agent Framework is considered an evolution—think of it as "Semantic Kernel v2.0."
- **Semantic Kernel v1.x** will continue to receive support for at least one year after Agent Framework reaches General Availability, including bug and security fixes.
- Most new features will target the new framework, but critical updates and some feature GA milestones will still reach Semantic Kernel v1.x.
- [Migration guides](https://github.com/microsoft/agent-framework/tree/main/dotnet/samples/SemanticKernelMigration) are provided for both .NET and Python libraries.

## Platform Support

Microsoft intends to maintain feature parity and ongoing support across both Python and C#/.NET for features at General Availability (GA). However, preview features may first appear in one language or another, depending on team resources.

## Project Recommendations

- **Existing projects**: No immediate need to switch from Semantic Kernel unless features in Agent Framework are required.
- **New projects**: If possible, start with Microsoft Agent Framework, especially if you can wait for General Availability or need features exclusive to the new framework.

## Community and Naming

- Developers from the Semantic Kernel and AutoGen communities are encouraged to transition and contribute feedback.
- Branding experiments (such as "Semantogen") were considered and humorously dismissed.

## Feedback and Community Links

- Engage via [GitHub Discussions](https://devblogs.microsoft.com/foundry/introducing-microsoft-agent-framework-the-open-source-engine-for-agentic-ai-apps/) and [Discord](https://aka.ms/foundry/discord).

**Happy coding!**

---

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-agent-framework/)
