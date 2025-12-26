---
layout: "post"
title: "Deep Dive into Microsoft Agent Framework for AutoGen Users"
description: "This video provides an in-depth look at the Microsoft Agent Framework (MAF) designed for AutoGen users. Eric Zhu guides viewers through the migration from AutoGen to MAF, detailing key similarities and changes, critical features like model client setup, single and multi-agent orchestration, MCP support, agent-as-a-tool, and practical code comparisons. The session highlights architectural patterns and showcases how MAF converges Semantic Kernel and AutoGen into a single SDK for building advanced multi-agent AI systems on Microsoft technologies."
author: "Microsoft Developer"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=JlzteydCK_Q"
viewing_mode: "internal"
feed_name: "Microsoft Developer YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g"
date: 2025-10-30 16:00:39 +00:00
permalink: "/videos/2025-10-30-Deep-Dive-into-Microsoft-Agent-Framework-for-AutoGen-Users.html"
categories: ["AI", "Azure", "Coding"]
tags: ["Agent as A Tool", "Agent Orchestration", "AI", "AI Agents", "AutoGen", "Azure", "Cloud Computing", "Coding", "Debug Tools", "Dev", "Development", "Event Driven Runtime", "GraphFlow", "LLMs", "MCP Support", "Microsoft", "Microsoft Agent Framework", "Model Clients", "Multi Agent Systems", "Open Source", "Orchestration", "Pro Code Development", "SDK", "Semantic Kernel", "Tech", "Technology", "Videos", "Workflow"]
tags_normalized: ["agent as a tool", "agent orchestration", "ai", "ai agents", "autogen", "azure", "cloud computing", "coding", "debug tools", "dev", "development", "event driven runtime", "graphflow", "llms", "mcp support", "microsoft", "microsoft agent framework", "model clients", "multi agent systems", "open source", "orchestration", "pro code development", "sdk", "semantic kernel", "tech", "technology", "videos", "workflow"]
---

Eric Zhu from Microsoft Developer presents a thorough guide for AutoGen users transitioning to the Microsoft Agent Framework, exploring migration strategies and new multi-agent architecture capabilities.<!--excerpt_end-->

{% youtube JlzteydCK_Q %}

# Deep Dive into Microsoft Agent Framework for AutoGen Users

**Presented by Eric Zhu (Microsoft Developer)**

This comprehensive video explores the Microsoft Agent Framework (MAF)—a new open-source project that unifies Semantic Kernel and AutoGen into a single pro-code SDK for building advanced multi-agent AI systems on Azure.

## Overview

- **Migration Path:** Stepwise guidance for AutoGen users migrating to MAF, highlighting what remains consistent and what changes.
- **Key Topics:**
  - Model client setup
  - Single-agent features
  - MCP support
  - Agent-as-a-tool
  - Multi-agent orchestration (with side-by-side code comparisons)
  - Comprehensive discussion of core differences (conversation state, memory, message types, middleware)

## What is Microsoft Agent Framework?

- **Unified SDK:** MAF combines Semantic Kernel and AutoGen into one platform for developing composable, programmable multi-agent solutions.
- **Roots:** AutoGen started at Microsoft Research and became known for GroupChat and event-driven orchestration.
- **Community Driven:** Contributions from internal teams and the open-source community have enhanced critical features and extensibility.

## Key Feature Demonstrations

### 1. Model Clients

- Overview of setting up and managing model clients in MAF.

### 2. Single-Agent Features

- Core interface and capabilities for single-agent scenarios.
- Handling tool usage and streaming responses.

### 3. Multi-Agent Orchestration

- Implementation of sequential, concurrent, and advanced orchestration patterns.
- Side-by-side code demonstrations contrasting AutoGen and MAF approaches.

### 4. MCP Support & Agent-as-a-Tool

- Integrating with the Multi-Channel Platform (MCP) and enabling agents to function as callable tools.

### 5. Workflow Patterns and Differences

- In-depth comparison of 'GraphFlow' vs. traditional workflow models
- Message types, conversation memory/state, and the new middleware architecture.
- Advanced orchestration logic including conditionals, fan-out/join, and targeted routing.

### 6. Dev Tools and UI

- Differences between the MAF Developer UI, Debug UI, and the legacy AutoGen Studio.

## Resources and Next Steps

- [MAF Docs & Getting Started](https://aka.ms/AgentFramework/Docs)
- [AutoGen to MAF Migration Guide](https://learn.microsoft.com/en-us/agent-framework/user-guide/workflows/core-concepts/overview)
- [Sessions On-Demand](https://aka.ms/AgentFramework/AIShow)
- [Victor Dibia's Blog Series on Multi-Agent Systems](https://www.linkedin.com/posts/dibiavictor_multiagentsystems-multiagentbook-activity-7384289105616146433-LuFQ?utm_source=share&utm_medium=member_desktop&rcm=ACoAAAGUAuIBcAPq6XKXTQDQsQcUj49UEXhye40)

## Contributors

- Eric Zhu, Victor Dibia, Jorge Arteiro, Elijah Straight

## How to Get Involved

- [Submit OSS Project](https://aka.ms/OpenAtMsCFP)
- [Open at Microsoft Playlist](https://aka.ms/OpenAtMicrosoftPlaylist)
- Connect with contributors via LinkedIn (links in video description)

## Summary

This session equips AutoGen users and AI developers with foundational and advanced knowledge for migrating to Microsoft Agent Framework, leveraging new orchestration patterns, memory models, and tool-building strategies. The talk emphasizes concrete migration paths and practical demonstrations, ideal for technical teams intent on deploying sophisticated AI agent solutions on the Microsoft stack.
