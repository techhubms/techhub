---
layout: "post"
title: "AI Toolkit for VS Code: February 2026 Major Update (v0.30.0)"
description: "This community release by junjieli details the major February 2026 update for the AI Toolkit extension in Visual Studio Code (v0.30.0). It introduces several new features for agent development, including a centralized Tool Catalog, enhanced debugging via Agent Inspector, treating agent evaluations as first-class tests, and deeper integration with Microsoft Foundry. Improvements span agent building, model management (with OpenAI Response API support), Copilot workflow orchestration, and conversion/profiling. The update aims to streamline creation, debugging, and deployment of AI agents directly in the developer environment."
author: "junjieli"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-toolkit-for-vs-code-february-2026-update/ba-p/4493673"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-13 08:00:00 +00:00
permalink: "/2026-02-13-AI-Toolkit-for-VS-Code-February-2026-Major-Update-v0300.html"
categories: ["AI", "Azure", "Coding"]
tags: ["Agent Builder", "Agent Development", "Agent Inspector", "AI", "AI Toolkit", "Azure", "Coding", "Community", "Copilot", "Data Wrangler", "Debugging", "Eval Runner SDK", "Integration", "Microsoft Foundry", "Model Playground", "OpenAI Response API", "Phi Silica", "Prompt Agents", "Pytest", "Python", "Qualcomm GPU", "Tool Catalog", "VS Code", "Workflow Orchestration"]
tags_normalized: ["agent builder", "agent development", "agent inspector", "ai", "ai toolkit", "azure", "coding", "community", "copilot", "data wrangler", "debugging", "eval runner sdk", "integration", "microsoft foundry", "model playground", "openai response api", "phi silica", "prompt agents", "pytest", "python", "qualcomm gpu", "tool catalog", "vs code", "workflow orchestration"]
---

junjieli announces the AI Toolkit v0.30.0 February 2026 update for VS Code, highlighting new features such as a centralized Tool Catalog, improved debugging with Agent Inspector, advanced agent evaluation workflows, and Microsoft Foundry integration.<!--excerpt_end-->

# AI Toolkit for VS Code — February 2026 Update

**Author:** junjieli

## Major Milestone: v0.30.0

The February 2026 release of AI Toolkit for Visual Studio Code delivers significant new features, making AI agent development more discoverable, debuggable, and production-ready. The update introduces a brand-new Tool Catalog, end-to-end Agent Inspector, and treating evaluations as full-fledged tests.

---

## 🔧 New in v0.30.0

### 🧰 Tool Catalog: Centralized Agent Tool Management

- Unified hub for discovering and managing agent tools.
- Browse, search, and filter tools from the public Foundry catalog and local stdio MCP servers.
- Configure connection settings for each tool directly in VS Code.
- Add tools to agents via the Agent Builder interface.
- Manage tool lifecycles (add, update, remove) within the editor.

### 🕵️ Agent Inspector: Enhanced Debugging

- Debug agents inside VS Code with one-click F5 support.
- Set breakpoints, inspect variables, and step through agent execution.
- Copilot auto-configuration scaffolds agent code, endpoints, and debugging setup.
- Generate production-ready code using the Hosted Agent SDK for Microsoft Foundry.
- Real-time visualization of responses, tool calls, and agent workflows.
- Rapid navigation—double-click workflow nodes to jump to the corresponding source code.
- Unified view combining workflow, chat, and visualization.

### 🧪 Evaluation as Tests: Developer-Friendly Quality Checks

- Define agent evaluations as test cases using familiar pytest syntax and Eval Runner SDK annotations.
- Run evaluations directly from the VS Code Test Explorer.
- Analyze evaluation results in a table view with Data Wrangler integration.
- Submit and run evaluation definitions at scale within Microsoft Foundry.

---

## 🔄 Improvements Across the Toolkit

### 🧱 Agent Builder

- Refreshed layout and navigation for building/managing agents.
- Switch easily between different agents.
- Support for creating, running, and saving Foundry prompt agents.
- Integrate tools into prompt agents from the Tool Catalog or built-in selections.
- New "Inspire Me" feature for agent instruction drafting.

### 🤖 Model Catalog

- Added support for OpenAI Response API models, such as gpt-5.2-codex.
- General improvements for reliability and performance.

### 🧠 Build Agent with GitHub Copilot

- New workflow entry point for generating multi-agent workflows with Copilot.
- Orchestrate workflows by selecting prompt agents from Microsoft Foundry.

### 🔁 Conversion & Profiling

- Generate interactive playgrounds for history models.
- Added Qualcomm GPU recipes.
- View resource usage for Phi Silica models in the Model Playground.

---

## ✨ Wrapping Up

AI Toolkit v0.30.0 represents a leap in building AI agents directly in VS Code, closely aligning agent workflows with mainstream software engineering practices. With enhanced discoverability, debugging, structured evaluation, and seamless Foundry integration, developers can more efficiently build and deploy production-grade AI agents.

---

*For more updates, feedback is welcome from the community. Happy agent building!*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-toolkit-for-vs-code-february-2026-update/ba-p/4493673)
