---
layout: "post"
title: "How GitHub Copilot Uses Embedding-Guided Tool Routing in VS Code"
description: "This article explores how GitHub Copilot Chat in VS Code leverages embedding-guided tool routing, adaptive tool clustering, and a streamlined tool set via the Model Context Protocol (MCP) to improve agent efficiency and response times. It covers technical details on tool selection, clustering algorithms, and benchmark results, providing actionable insights for developers interested in AI-driven developer tooling and the evolution of intelligent coding assistants."
author: "Anisha Agarwal"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/ai-and-ml/github-copilot/how-were-making-github-copilot-smarter-with-fewer-tools/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-11-19 20:00:00 +00:00
permalink: "/2025-11-19-How-GitHub-Copilot-Uses-Embedding-Guided-Tool-Routing-in-VS-Code.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["Adaptive Tool Clustering", "Agentic Workflows", "AI", "AI & ML", "AI Driven Tooling", "Codebase Analysis", "Coding", "Copilot Embedding Model", "Developer Tools", "Embedding Guided Routing", "GitHub Copilot", "LLM", "MCP", "News", "Performance Optimization", "Semantic Similarity", "SWE Lancer", "SWEbench Verified", "Tool Selection", "Virtual Tools", "VS Code"]
tags_normalized: ["adaptive tool clustering", "agentic workflows", "ai", "ai and ml", "ai driven tooling", "codebase analysis", "coding", "copilot embedding model", "developer tools", "embedding guided routing", "github copilot", "llm", "mcp", "news", "performance optimization", "semantic similarity", "swe lancer", "swebench verified", "tool selection", "virtual tools", "vs code"]
---

Anisha Agarwal delves into engineering advances in GitHub Copilot Chat for VS Code, showing how embedding-guided tool routing and adaptive clustering improve tool selection and efficiency for developers.<!--excerpt_end-->

# How GitHub Copilot Uses Embedding-Guided Tool Routing in VS Code

GitHub Copilot Chat in Visual Studio Code (VS Code) now accesses hundreds of tools through the Model Context Protocol (MCP), enabling everything from codebase analysis to Azure-specific utilities. But simply adding more tools doesn't guarantee smarter AI agents; in fact, it can slow them down and increase response latency.

## Why Too Many Tools Can Slow Down the Agent

A large toolset (e.g., 40 built-in tools by default, up to hundreds with MCP integrations) can exceed model API limits and cause response slowdowns, as evidenced by frequent 'optimizing tool selection' spinners. The engineering team recognized that more isn't always better, and excessive options can hinder both LLM reasoning and user experience.

## New Approaches: Embedding-Guided Routing & Adaptive Tool Clustering

Two systems address this challenge:

- **Embedding-guided tool routing**: Leverages semantic similarity embeddings to route queries to the most relevant tool clusters or 'virtual tools.'
- **Adaptive tool clustering**: Uses an internal embedding model to group tools by functionality, forming clusters (like folders) so the model handles fewer tool names at once.

### Details of Adaptive Clustering

Instead of feeding every tool directly to the LLM (which is costly and slow), embeddings are calculated for each tool. Cosine similarity groups tools functionally. Summary LLM calls are then made per cluster, improving speed and reproducibility. Tool embeddings and summaries are cached locally for efficiency.

### Context-Guided Tool Selection

With virtual tool groups made, the next issue was helping the model choose the right group efficiently. Embedding-guided routing compares the user's query to tool/cluster vectors, preselecting the most semantically relevant candidates—eliminating excessive exploratory calls and reducing latency/failure.

Example: For "Fix this bug and merge it into the dev branch," the router brings the proper *merge tool* to the forefront, skipping irrelevant clusters.

## Technical Benchmarks

- **Success rates**: On tests like SWE-Lancer and SWEbench-Verified with GPT-5 and Sonnet 4.5, the streamlined system increases success rates by 2-5 percentage points.
- **Latency**: Average response latency drops by about 400ms; time to first token (TTFT) and time to complete response both improve.
- **Coverage**: Embedding-based selection achieved 94.5% 'Tool Use Coverage,' outperforming both LLM-based (87.5%) and default static lists (69.0%).
  - Online: Pre-expanding the correct tool group rose from 19% (old method) to 72% (embedding-based).

## The Reduced Toolset

Performance data identified a core set of 13 essential tools (for repo parsing, file editing, context search, terminal usage). Remaining built-in tools group into four virtual categories: Jupyter Notebook Tools, Web Interaction Tools, VS Code Workspace Tools, and Testing Tools. Only these clusters are expanded if needed, significantly improving response times and efficiency.

## Looking Ahead: Long-Context Reasoning

As MCP systems mature, future work includes enabling agents to:

- Remember past tool usage
- Infer intent from history
- Plan multi-step actions using context across long sessions
- Combine embeddings, memory, and reinforcement to make Copilot more context-aware and intelligent

## Try It and Contribute

To see Copilot's MCP tooling in action, [try GitHub Copilot](https://github.com/features/copilot/?utm_source=blog-vs-code-updates&utm_medium=blog&utm_campaign=universe25post) in VS Code. Feedback from the developer community, as well as active research and engineering, continue to improve these features.

---

*Acknowledgments: Thanks to Zijian Jin and the broader Copilot team for contributions to this work. Authored by Anisha Agarwal.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/how-were-making-github-copilot-smarter-with-fewer-tools/)
