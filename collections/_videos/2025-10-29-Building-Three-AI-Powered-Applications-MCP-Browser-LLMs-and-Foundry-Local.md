---
layout: post
title: 'Building Three AI-Powered Applications: MCP, Browser LLMs, and Foundry Local'
author: Microsoft Developer
canonical_url: https://www.youtube.com/watch?v=2QXzxOLtCHM
viewing_mode: internal
feed_name: Microsoft Developer YouTube
feed_url: https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g
date: 2025-10-29 16:00:05 +00:00
permalink: /ai/videos/Building-Three-AI-Powered-Applications-MCP-Browser-LLMs-and-Foundry-Local
tags:
- Agent Code Generation
- AI Application Development
- AI Toolkit
- AIApplications
- AIToolkit
- Browser LLM
- BuildingWithAI
- Cloud AI
- CloudAI
- Code Integration
- Foundry Local
- FoundryLocal
- GenerativeAI
- GitHub Codespaces
- Java
- JavaApps
- JavaDevelopment
- JavaScript
- Language Models
- LLM
- Local AI
- LocalAI
- MCP
- VS Code
- VS Code Extensions
section_names:
- ai
- coding
---
In this episode, Ayan Gupta and Rory demonstrate hands-on development of three different AI-powered apps, showcasing MCP, browser-based LLMs, and Microsoft's Foundry Local. Their step-by-step approach enables Java developers to learn practical AI integration.<!--excerpt_end-->

{% youtube 2QXzxOLtCHM %}

# Building Three AI-Powered Applications: MCP, Browser LLMs, and Foundry Local

## Introduction

In this episode, Ayan Gupta is joined by Rory to bring theory into practice by building three working AI applications tailored for Java developers. Each app highlights a different architectural approach for utilizing large language models (LLMs) and AI integrations.

---

## Application 1: Calculator Service with MCP (Model Context Protocol)

- **What is MCP?** MCP (Model Context Protocol) allows LLMs to call external tools and functions, extending AI capabilities beyond basic conversation.
- **How it's built:**
  - Set up a new GitHub Codespace to code the project.
  - Use the AI Toolkit extension in Visual Studio Code to automatically generate agent code, simplifying the integration process.
  - Register a calculator tool on the MCP server, making it accessible to AI models for real-world computations.
- **Key architecture:** AI agents interact with registered tools using the MCP server as a bridge, which enables LLMs to perform function calling and interact with external systems.

---

## Application 2: Offline Pet Story Generator Using Browser-Based LLMs

- **Local AI in your browser:** Build a pet story generator app running entirely in the browser using JavaScript's built-in language model interface. This approach keeps all data local, improving privacy and offline capability.
- **Features:**
  - No external server required; runs in-browser for complete data privacy.
  - Processes user input and generates stories locally.
  - Includes an image analysis feature leveraging browser-based AI.

---

## Application 3: Running LLMs Locally with Foundry Local

- **What is Foundry Local?** Microsoft's Foundry Local lets developers run large language models on their own hardware, providing full control over performance, privacy, and data handling.
- **Setup steps:**
  - Install and configure Foundry Local on your development machine.
  - Run and test language models entirely on your own infrastructure, without needing cloud resources.
- **Use cases:** Ideal for scenarios requiring completely private AI inference, development and testing, or strict regulatory constraints.

---

## Key Takeaways and Deployment Comparisons

- **Cloud-Based (MCP + GitHub)**: Best for integration with external tools and when leveraging the scalability of cloud services.
- **Client-Side (Browser LLMs)**: Ideal for privacy, offline scenarios, and lightweight local AI tasks.
- **Local Deployment (Foundry Local)**: Crucial for organizations needing total data control or working within sensitive environments.
- The episode wraps up with practical comparison and analysis of these approaches, empowering developers to choose the right tool and strategy for their use case.

---

## Resources

- [Java & AI for Beginners](https://aka.ms/JavaAndAIForBeginners)
- [GenAI Java documentation](https://aka.ms/genaijava)

---

**Presented by:** Ayan Gupta & Rory

Viewers leave with hands-on experience and architectural insights for AI-powered app development with Java and Microsoft tools.
