---
layout: "post"
title: "Microsoft’s Agentic Frameworks: AutoGen and Semantic Kernel"
description: "This article discusses the collaboration between Microsoft's Semantic Kernel and AutoGen frameworks for agentic AI applications, detailing their alignment, differences, and future convergence. Guidance is provided for selecting the right tool, understanding current and upcoming features, and available resources to help developers leverage these technologies."
author: "Friederike Niedtner"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/autogen/microsofts-agentic-frameworks-autogen-and-semantic-kernel/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/autogen/feed/"
date: 2024-11-18 15:50:54 +00:00
permalink: "/2024-11-18-Microsofts-Agentic-Frameworks-AutoGen-and-Semantic-Kernel.html"
categories: ["AI", "Coding"]
tags: ["Agentic Frameworks", "AI", "AI Agents", "AutoGen", "C#", "Coding", "Dapr", "Enterprise AI", "Event Driven Architecture", "GenAI", "Java", "LLMs", "Microsoft", "Microsoft Orleans", "Multi Agent Applications", "News", "Python", "Semantic Kernel"]
tags_normalized: ["agentic frameworks", "ai", "ai agents", "autogen", "c", "coding", "dapr", "enterprise ai", "event driven architecture", "genai", "java", "llms", "microsoft", "microsoft orleans", "multi agent applications", "news", "python", "semantic kernel"]
---

Authored by Friederike Niedtner, this article explores the collaboration between Microsoft's Semantic Kernel and AutoGen agentic AI frameworks, detailing their capabilities, integration roadmap, and resources to help developers choose and leverage these powerful tools.<!--excerpt_end-->

# Microsoft’s Agentic Frameworks: AutoGen and Semantic Kernel

*By Friederike Niedtner*

Microsoft’s agentic AI frameworks—Semantic Kernel and AutoGen—are now engaged in a deep collaboration to deliver a leading developer experience for agent-based solutions. This initiative brings together the robust, enterprise-ready features of Semantic Kernel and the innovative, multi-agent runtime from AutoGen (autogen-core), aimed at providing customers with a unified and flexible path for building advanced agentic applications.

## Current State and Collaboration Roadmap

- **Semantic Kernel** offers enterprise-ready AI capabilities for agent application development and is already available and supported for building such solutions.
- **AutoGen** has developed a powerful multi-agent runtime (autogen-core), and efforts are underway to align this with Semantic Kernel, forthcoming in early 2025.

Together, the teams are building towards an integrated multi-agent runtime, enabling users to easily transition from experimentation to production-grade environments.

### Key Announcements

- *Customers can already use Semantic Kernel for developing agentic applications with enterprise support.*
- *AutoGen allows ideation and experimentation with multi-agent patterns, making it ideal for cutting-edge development.*
- *Future alignment will enable a seamless transition for customers starting in AutoGen to scale production solutions with Semantic Kernel’s enterprise features.*

## Recent Releases and Features

### Semantic Kernel Process Framework

- Introduces stateful, long-running workflows and human-in-the-loop processes.
- Enables business process modeling, scalable deployment with **Dapr**, and future integration with **Microsoft Orleans**.
- [Read more about the Semantic Kernel Process Framework](https://devblogs.microsoft.com/semantic-kernel/integrating-ai-into-business-processes-with-the-process-framework/)

### AutoGen 0.4 Preview

- Major redesign with a distributed, event-driven architecture using Microsoft Orleans.
- Composable, flexible, observable, and scalable; supports multiple programming languages.
- Features [Magentic-One](https://aka.ms/magentic-one-blog), a generalist multi-agent team achieving state-of-the-art benchmarks.
- [Learn more about AutoGen 0.4 Preview](https://microsoft.github.io/autogen/0.2/blog/2024/10/02/new-autogen-architecture-preview/)

## Converging Design Principles

Both frameworks are migrating towards similar principles for orchestrating multiple agents, leading to planned collaboration and a unified runtime. This will:

- Allow experimentation with advanced agentic patterns.
- Enable seamless scaling and transition to enterprise support.

## Frequently Asked Questions

### What are the primary functions of Semantic Kernel and AutoGen?

- **AutoGen:**
  - Open-source framework by Microsoft Research’s AI Frontiers Lab.
  - Builds AI agent systems for orchestrating event-driven, distributed multi-agent applications.
  - Supports orchestrating multiple LLMs, tools, and advanced design patterns.
  - Ideal for long-running, autonomous collaborative agents with variable human involvement.
  - Current language support: C# and Python.

- **Semantic Kernel:**
  - Production-ready SDK for integrating LLMs and data stores into applications, enabling GenAI solutions.
  - Multi-language support: C#, Python, Java.
  - Features Agent and Process Frameworks (in preview) for developing single and multi-agent solutions.

### Which framework is more appropriate for production?

- **Semantic Kernel** has reached v1.0 (.NET, Python, Java) and is enterprise-ready, emphasizing stability and non-breaking updates. Supported via Microsoft Unified Customer Support.
- **AutoGen** is maintained by Microsoft Research as a community-backed, cutting-edge framework for rapid innovation in agentic systems. Provides a pathway for the latest AI research integration.
- Efforts are ongoing to allow seamless transition from AutoGen’s experimental phase to Semantic Kernel’s enterprise stability.

### What’s the path for product support if I use AutoGen?

- *In 2025*, customers using Autogen’s multi-agent runtime will be able to transition smoothly to Semantic Kernel for an enterprise-ready, supported environment.
- Until then, continued use of AutoGen is possible with community support, especially for complex patterns not yet available elsewhere.

### Are there resources for getting started?

**Semantic Kernel:**

- [GitHub Repository](https://aka.ms/sk/repo)
- [Blog](https://aka.ms/sk/blog)
- [Microsoft Learn: Semantic Kernel Agent Framework](https://aka.ms/sk/agents)

**AutoGen:**

- [GitHub Repository](https://github.com/microsoft/autogen)
- [Documentation](https://microsoft.github.io/autogen/dev/)
- [AutoGen Architecture Preview](https://microsoft.github.io/autogen/0.2/blog/2024/10/02/new-autogen-architecture-preview/)

## Summary

Combining Semantic Kernel’s production reliability and AutoGen’s rapid agentic innovation gives developers multiple paths for building state-of-the-art AI solutions. The planned convergence will bridge experimentation and enterprise-grade stability, underpinned by increasingly rich support resources from Microsoft. Stay tuned for further updates as the two frameworks build a unified agentic runtime and enable broader adoption across the developer ecosystem.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/autogen/microsofts-agentic-frameworks-autogen-and-semantic-kernel/)
