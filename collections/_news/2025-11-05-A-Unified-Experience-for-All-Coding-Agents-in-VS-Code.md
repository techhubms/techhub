---
layout: "post"
title: "A Unified Experience for All Coding Agents in VS Code"
description: "This article by Burke Holland outlines the evolution of coding agents within Visual Studio Code as of 2025, highlighting the integrations of GitHub Copilot, Copilot coding agent, Copilot CLI, OpenAI Codex, and Anthropic Claude. It introduces 'Agent Sessions,' a new VS Code feature for orchestrating multiple coding agents, details on custom agents such as the Plan agent, and innovations such as subagents and context management tools for developers."
author: "Burke Holland"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/blogs/2025/11/03/unified-agent-experience"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2025-11-05 00:00:00 +00:00
permalink: "/news/2025-11-05-A-Unified-Experience-for-All-Coding-Agents-in-VS-Code.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["Agent Sessions", "AI", "Anthropic Claude", "Code Orchestration", "Coding", "Coding Agents", "Context Engineering", "Copilot CLI", "Custom Agents", "Developers", "GitHub Copilot", "Model Integration", "News", "OpenAI Codex", "Plan Agent", "Pro+ Subscription", "Prompt Engineering", "Subagents", "VS Code"]
tags_normalized: ["agent sessions", "ai", "anthropic claude", "code orchestration", "coding", "coding agents", "context engineering", "copilot cli", "custom agents", "developers", "github copilot", "model integration", "news", "openai codex", "plan agent", "proplus subscription", "prompt engineering", "subagents", "vs code"]
---

Burke Holland discusses the 2025 surge in coding agents within VS Code, including unified orchestration for GitHub Copilot, OpenAI Codex, and custom agents, offering developers more flexibility and control.<!--excerpt_end-->

# A Unified Experience for All Coding Agents in VS Code

**Author:** Burke Holland

Agents have taken center stage in Visual Studio Code (VS Code) development workflows throughout 2025. This article details the integration of multiple coding agents—most prominently GitHub Copilot, OpenAI Codex, and the new Copilot CLI—into a unified experience for developers.

## Agent Diversity and Integration

2025 marked a year of rapid expansion for coding agents. OpenAI released GPT-5 and GPT-5 Codex, making these advanced models accessible via both CLI tools and VS Code extensions. Concurrently, the VS Code team introduced "agent mode" and expanded support for the Copilot coding agent (cloud), the GitHub Copilot CLI, and OpenAI Codex, among others. These integrations mean developers can use several agents—from Microsoft, OpenAI, and Anthropic—within the same development environment.

## The Multi-Agent Dilemma

The influx of agent choices has led to more flexibility but also to some fragmentation. Developers often face "subscription hopping," tool juggling, and fear of missing out on the latest innovations.

## Solution: Unified Agent Experience

Unveiled at GitHub Universe, the new "Agent Sessions" feature in VS Code allows developers to manage all agents—local or cloud-based—via a dedicated sidebar. Agent Sessions display the status of all running agents, allow jumping between sessions, and support a tabbed "chat editors" experience for real-time agent interactions. Developers can course-correct agents mid-run or delegate tasks to any available agent directly from the Chat view.

## OpenAI Codex Integration With Copilot

OpenAI Codex now integrates natively with GitHub Copilot Pro+ subscriptions, eliminating the need for separate OpenAI accounts. With a simple extension installation, developers get seamless access to both Copilot and Codex features—like code generation and explanation—under shared rate limits.

## Building and Customizing Agents

The concept of "agents" in VS Code has evolved to encompass custom modes and extensions. The new built-in "Plan" agent helps developers break down vague prompts into actionable steps, asks clarifying questions, and provides recommended libraries with comparative reasoning. This feature enhances prompt engineering and serves as a baseline for building custom agents.

Custom agents can be delegated tasks and work alongside default agents. A rich repository of community-generated prompts and agent templates is available via the [awesome-copilot GitHub repo](https://github.com/github/awesome-copilot).

## Context Management and Subagents

With increasing agent activity comes the challenge of context management—too much context can lead to confusion. The specialized "runSubagent" tool allows the creation of subagents that operate with isolated context, preventing context bleed and ensuring focused assistance. These subagents run independently and only return their final results to the main chat thread.

## Looking Forward

This shift to a mission control-style experience within VS Code empowers developers to work with multiple agents, fine-tune context management, and create or customize their own agents as needed. With continual innovation, the multi-agent workflow is now a central part of coding in VS Code.

For more detailed documentation, visit [VS Code's blog](https://code.visualstudio.com/blogs/2025/11/03/unified-agent-experience) or check out the [GitHub Universe announcements](https://github.com/events/universe).

> Happy Coding! 💙

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/11/03/unified-agent-experience)
