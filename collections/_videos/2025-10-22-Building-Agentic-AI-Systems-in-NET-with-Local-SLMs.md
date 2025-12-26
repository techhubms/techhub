---
layout: "post"
title: "Building Agentic AI Systems in .NET with Local SLMs"
description: "This session explores agentic AI architecture within .NET workflows using Semantic Kernel and local-first Small Language Models (SLMs). Developers learn how to set up agents for autonomous or human-in-the-loop automation, utilizing in-process and server-hosted SLMs, integrating Retrieval-Augmented Generation, output evaluation, and detailed observability through Microsoft tools."
author: "dotnet"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=mwwAz0F4r1E"
viewing_mode: "internal"
feed_name: "DotNet YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCvtT19MZW8dq5Wwfu6B0oxw"
date: 2025-10-22 23:08:25 +00:00
permalink: "/videos/2025-10-22-Building-Agentic-AI-Systems-in-NET-with-Local-SLMs.html"
categories: ["AI", "Coding"]
tags: [".NET", "Agentic AI", "Agents", "AI", "Autonomous Agents", "C#", "Coding", "Demo", "Developer", "Developer Community", "Developer Tools", "Dotnet10", "Dotnetdeveloper", "Edge Deployment", "Embeddings", "Human in The Loop", "in Memory Vector Store", "LLamaSharp", "Microsoft", "Microsoft.Extensions.AI.Evaluation", "Observability", "OllamaSharp", "On Prem Deployment", "Retrieval Augmented Generation", "Semantic Kernel", "SLM", "Small Language Models", "Smalllanguagemodels", "Software Developer", "State Management", "Videos"]
tags_normalized: ["dotnet", "agentic ai", "agents", "ai", "autonomous agents", "csharp", "coding", "demo", "developer", "developer community", "developer tools", "dotnet10", "dotnetdeveloper", "edge deployment", "embeddings", "human in the loop", "in memory vector store", "llamasharp", "microsoft", "microsoftdotextensionsdotaidotevaluation", "observability", "ollamasharp", "on prem deployment", "retrieval augmented generation", "semantic kernel", "slm", "small language models", "smalllanguagemodels", "software developer", "state management", "videos"]
---

In this .NET Live session, Daniel Costea, Cam Soper, Frank Boucher, and Maria Wenzel guide developers through building agentic AI systems with local SLMs, enhanced by Semantic Kernel, in the Microsoft ecosystem.<!--excerpt_end-->

{% youtube mwwAz0F4r1E %}

# Building Agentic AI Systems in .NET with Local SLMs

## Overview

Agentic AI enables the creation of .NET-based agents that can reason, act autonomously, and collaborate with developers—streamlining enterprise automation with trusted decision flows. This hands-on session demonstrates building agentic systems using local-first Small Language Models (SLMs), integrated with Microsoft Semantic Kernel and key libraries like OllamaSharp and LLamaSharp.

## Key Technologies and Concepts

- **Small Language Models (SLMs):** Utilized locally for lowest latency, data control, and reduced operational costs; deployed on edge or on-premises.
- **Microsoft Semantic Kernel:** Central to coordinating agent workflows, controlling tool usage, and managing state.
- **OllamaSharp & LLamaSharp:** Enable integration with SLMs (via Ollama server or in-process), giving flexibility for server-side or fully local deployments.
- **Retrieval-Augmented Generation (RAG):** Uses in-memory vector stores and embeddings for grounded, accurate responses in business scenarios.
- **Microsoft.Extensions.AI.Evaluation:** Provides output evaluation and supports observability/auditing through Semantic Kernel filters.

## Demo Scenarios

- **Autonomous Agents:** Execute tasks independently and can recover from errors using safe recovery loops and state management.
- **Human-in-the-Loop:** Escalation and approval processes allow agents to defer key decisions to human oversight.

## Architecture Patterns

- **Local-first SLM deployment:** Low latency, strong data control, and cost efficiency for developer and enterprise use.
- **Edge and On-Premises:** Agents can be deployed where data sovereignty and quick response are critical.
- **Observability and Auditing:** Deep integration allows for tracking agent decisions and ensuring compliance.

## Practical Guidance

Attendees walk away with:

- Working C# code samples for agent development with SLMs
- Patterns for integrating agentic AI into existing .NET solutions
- Strategies for evaluating agent output and maintaining production-readiness

## Speakers

Daniel Costea, Cam Soper, Frank Boucher, and Maria Wenzel share real-world expertise, live demos, and actionable solutions.
