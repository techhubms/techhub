---
layout: "post"
title: "Building MCP Clients: Java Integration and GitHub Copilot Use Cases"
description: "This episode explores building clients for the Model Context Protocol (MCP), focusing on real-world implementations. It covers testing clients using MCP Inspector, integrating MCP into Visual Studio Code with GitHub Copilot, and developing a Java chat application with LangChain4j and local AI models like Llama. Learn how GitHub Copilot and custom MCP tools enhance developer workflows and how to implement real-time AI-assisted client applications in Java."
author: "Microsoft Developer"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=HQQavvdrAA0"
viewing_mode: "internal"
feed_name: "Microsoft Developer YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g"
date: 2025-10-31 00:00:22 +00:00
permalink: "/videos/2025-10-31-Building-MCP-Clients-Java-Integration-and-GitHub-Copilot-Use-Cases.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["Agent Mode", "AI", "AI Integration", "AIIntegration", "AITools", "Chat Applications", "ChatApplications", "Client Server", "Coding", "Copilot Integration", "Dev Tools", "GitHub Copilot", "Java", "Java Development", "JavaClient", "JavaDevelopment", "LangChain4j", "Llama", "LlamaAI", "Local LLM", "LocalLLM", "MCP", "MCP Inspector", "Videos", "VS Code"]
tags_normalized: ["agent mode", "ai", "ai integration", "aiintegration", "aitools", "chat applications", "chatapplications", "client server", "coding", "copilot integration", "dev tools", "github copilot", "java", "java development", "javaclient", "javadevelopment", "langchain4j", "llama", "llamaai", "local llm", "localllm", "mcp", "mcp inspector", "videos", "vs code"]
---

Microsoft Developer presents a hands-on walkthrough featuring Ayan Gupta, Bruno Borges, and Sandra Ahlgrimm on building MCP clients, integrating GitHub Copilot, and developing Java chat applications with AI.<!--excerpt_end-->

{% youtube HQQavvdrAA0 %}

# Building MCP Clients: Java Integration and GitHub Copilot Use Cases

## Overview

In this episode, Ayan Gupta joins Bruno Borges and Sandra Ahlgrimm to explore the client side of the Model Context Protocol (MCP). Building on the prior session where an MCP server was created, this walkthrough demonstrates three approaches to consuming the MCP server from client applications:

- **Testing with the MCP Inspector tool**
- **Integrating MCP into Visual Studio Code with GitHub Copilot**
- **Building a full Java chat application using LangChain4j and local AI models like Llama**

## Key Topics Covered

### 1. MCP Client Implementation

- The client acts as the consumer in a client-server architecture.
- Demonstrates translating user needs into precise requests via MCP.

### 2. Testing MCP with Inspector Tool

- Shows how to validate the MCP server using the MCP Inspector tool.

### 3. VS Code and GitHub Copilot Integration

- Guides through configuring Visual Studio Code for MCP server usage.
- Explains how GitHub Copilot accesses custom MCP tools during development.
- Discusses enabling Agent Mode to allow the AI to call MCP server functions for accurate, domain-specific results within the development environment.

### 4. Building a Java Chat Application

- Walkthrough of creating a Java client application using [LangChain4j](https://github.com/langchain4j/langchain4j).
- Steps include configuring chat models (with local models like Llama), setting up chat memory, and registering MCP tools.
- Application enables real-time integration where the AI model can fetch data from custom MCP tools (demonstrated with domain-specific questions about monkey species).
- Patterns showcased are adaptable to other domains.

### 5. Real-Time AI-Assisted Workflow

- Demonstrates queries for known and fictional species, showing the flexibility of the AI/middleware integration.
- Shows agent-driven development, enhancing workflow with smart AI automation.

## Resources

- [Java and AI for Beginners](https://aka.ms/JavaAndAIForBeginners)

---

## Table of Contents (by session timing)

- 0:00 Introduction: Clients Drive the Interaction
- 0:57 Recap: The MCP Server We Built
- 1:36 Testing with MCP Inspector Tool
- 2:21 Configuring MCP in Visual Studio Code
- 3:00 Using MCP Resources in Copilot
- 3:42 Enabling Agent Mode for MCP
- 4:42 Querying the MCP Server via VS Code
- 5:40 Getting Accurate Data from MCP Tools
- 6:23 Building a Java Client Application
- 7:01 Integrating LangChain4j with MCP
- 7:42 Running the Chat Application
- 8:27 Querying for Monkey Species
- 8:51 Testing with Fictional Species
- 9:29 Using Other AI CLI Tools with MCP
- 10:05 Session Recap and Wrap-Up

---

## Further Reading & Tools

- Model Context Protocol (MCP) documentation
- LangChain4j project
- GitHub Copilot documentation
- Local LLMs such as Llama and their integration strategies
- Visual Studio Code extensions for AI development

## About the Authors

This episode features Ayan Gupta, Bruno Borges, and Sandra Ahlgrimm, delivering practical demonstrations of the MCP ecosystem and client-building techniques with Java and GitHub Copilot.
