---
layout: "post"
title: "Orchestrating AI Agents in Blazor Using Microsoft Semantic Kernel"
description: "This video offers a practical, step-by-step walkthrough of implementing concurrent AI agent orchestration in a Blazor application using Microsoft's Semantic Kernel Agent Framework. Viewers will see how to set up Semantic Kernel, register and coordinate multiple agents, and leverage dependency injection to build interactive AI-powered Blazor frontends. The demo covers both code structure and real-world applications like chatbots and recommendation systems, focusing on parallel execution and clean orchestration patterns tailored for .NET developers."
author: "Learn Microsoft AI"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=Cf-bZUnlBHc"
viewing_mode: "internal"
feed_name: "Learn Microsoft AI Youtube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCQf_yRJpsfyEiWWpt1MZ6vA"
date: 2025-06-30 03:47:16 +00:00
permalink: "/videos/2025-06-30-Orchestrating-AI-Agents-in-Blazor-Using-Microsoft-Semantic-Kernel.html"
categories: ["AI", "Coding"]
tags: [".NET", "Agent Resolver", "AgentOrchestration", "AI", "AI Agent Orchestration", "AI in Blazor", "AIinBlazor", "Blazor", "ChatCompletionAgents", "Coding", "Concurrent Orchestration", "ConcurrentOrchestration", "Dependency Injection", "DotNetAI", "Microsoft AI", "MicrosoftAI", "Multi Agent Systems", "MultiAgent", "Orchestration Service", "ProcessRuntime", "Semantic Kernel", "Videos"]
tags_normalized: ["dotnet", "agent resolver", "agentorchestration", "ai", "ai agent orchestration", "ai in blazor", "aiinblazor", "blazor", "chatcompletionagents", "coding", "concurrent orchestration", "concurrentorchestration", "dependency injection", "dotnetai", "microsoft ai", "microsoftai", "multi agent systems", "multiagent", "orchestration service", "processruntime", "semantic kernel", "videos"]
---

Learn Microsoft AI guides you through orchestrating multiple AI agents in a Blazor app using the Semantic Kernel framework, sharing code examples and orchestration strategies tailored for .NET developers.<!--excerpt_end-->

{% youtube Cf-bZUnlBHc %}

# Orchestrating AI Agents in Blazor Using Microsoft Semantic Kernel

**Author:** Learn Microsoft AI

## Overview

Discover how to implement concurrent orchestration of intelligent AI agents using the Semantic Kernel Agent Framework in a Blazor application. This guide provides a step-by-step walkthrough, from initial setup to a hands-on demo, detailing code structure and design patterns for scalable agent management.

## Table of Contents

- Overview of Implementation Plan
- Adding Semantic Kernel to Dependency Injection (DI)
- Registering ChatCompletionAgents in DI
- Setting Up the Orchestration Service
- Creating the Agent Resolver
- Implementing the Transformer Handler
- Registering ProcessRuntime & Invoking Agents
- Injecting Orchestration into a Blazor Page
- Agent Orchestration Demo
- Page-Based Orchestration Explained
- Grouping and Injecting Agents Per Page
- Concurrent Orchestration Logic
- Optimization and Code Improvement

## Semantic Kernel in Blazor

- Add **Semantic Kernel** to your Blazor app via Dependency Injection for centralized management.

## Registering Agents

- Register specialized agents (e.g., Movies Agent, Food Agent) as services.
- Use `ChatCompletionAgents` for AI-driven dialogue and recommendations.

## Orchestration Service

- Set up a **ConcurrentOrchestrationService** to coordinate multiple agents in parallel.
- Demonstrate parallel execution and concurrent orchestration for increased responsiveness.

## Dependency Injection & Agent Invocation

- Inject orchestration service into desired Blazor pages for dynamic agent management.
- Use an *Agent Resolver* for runtime selection of relevant agents based on user actions or page context.

## Demo Highlights

- Page-based orchestration allows agents to be grouped and invoked based on UI context.
- ProcessRuntime enables seamless and efficient triggering of multiple agents.
- Example: running Movies Agent and Food Agent in the same workflow.

## Use Cases

- Smart chatbots with distinct knowledge domains
- Recommendation systems leveraging multiple specialized agents
- Interactive, responsive AI features in .NET web frontends

## Optimization Areas

- Code organization for scalability
- Enhancing agent resolution strategies
- Improving parallelism and error handling

## Takeaways

- Strong fit for .NET and Blazor developers aiming to build multi-agent, intelligent applications.
- Demonstrates practical orchestration patterns with Microsoft's Semantic Kernel.

**Links:**

- [Learn Microsoft AI YouTube](https://www.youtube.com/channel/UCQf_yRJpsfyEiWWpt1MZ6vA)
- [LinkedIn profile](https://www.linkedin.com/in/rvinothrajendran/)
- [GitHub repository](https://github.com/rvinothrajendran)
