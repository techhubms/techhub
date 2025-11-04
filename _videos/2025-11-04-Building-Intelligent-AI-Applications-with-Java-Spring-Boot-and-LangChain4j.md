---
layout: "post"
title: "Building Intelligent AI Applications with Java, Spring Boot, and LangChain4j"
description: "This video, hosted by Ayan Gupta with guest Julien Dubois from Microsoft's Java Developer Advocacy team, introduces building intelligent AI apps in Java using LangChain4j, Spring Boot, and Azure AI Foundry. Viewers learn to integrate LangChain4j with OpenAI’s Java SDK, transform a Spring Boot project into an AI-powered poem generator, and understand dependency management with Maven. The session also covers configuring chat models, using GitHub Copilot to add features, and leveraging multi-LLM support through unified interfaces. The video focuses on adaptable, production-grade AI integration and showcases practical code examples targeting Java developers exploring Microsoft’s AI and cloud ecosystem."
author: "Microsoft Developer"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=_cqJzFxLorE"
viewing_mode: "internal"
feed_name: "Microsoft Developer YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g"
date: 2025-11-04 01:00:50 +00:00
permalink: "/2025-11-04-Building-Intelligent-AI-Applications-with-Java-Spring-Boot-and-LangChain4j.html"
categories: ["AI", "Azure", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Integration", "AIApplications", "AIIntegration", "API Integration", "Application Development", "Azure", "Azure AI", "Azure AI Foundry", "Chat Models", "ChatModels", "Coding", "Dependency Management", "GitHub Copilot", "GPT 4o Mini", "IntelligentApps", "Java", "Java 24", "Java24", "JavaAI", "JavaDevelopment", "LangChain4j", "LLM Providers", "Maven", "Microsoft", "OpenAI", "OpenAI Java SDK", "Spring Boot", "SpringBoot", "Videos"]
tags_normalized: ["ai", "ai integration", "aiapplications", "aiintegration", "api integration", "application development", "azure", "azure ai", "azure ai foundry", "chat models", "chatmodels", "coding", "dependency management", "github copilot", "gpt 4o mini", "intelligentapps", "java", "java 24", "java24", "javaai", "javadevelopment", "langchain4j", "llm providers", "maven", "microsoft", "openai", "openai java sdk", "spring boot", "springboot", "videos"]
---

In this video, Microsoft Developer's Ayan Gupta and Julien Dubois guide Java developers through building intelligent AI-powered applications using LangChain4j, Spring Boot, GitHub Copilot, and Azure AI Foundry.<!--excerpt_end-->

{% youtube _cqJzFxLorE %}

# Building Intelligent AI Applications with Java, Spring Boot, and LangChain4j

**Presenters:** Ayan Gupta (Microsoft) and Julien Dubois (Microsoft Java Developer Advocacy, LangChain4j core contributor)

## Introduction

Modern applications increasingly require adaptive intelligence, similar to how a smart mug reacts to temperature changes. This video introduces Java developers to the process of building AI-powered apps using LangChain4j, Spring Boot, GitHub Copilot, and Azure services.

## Key Learning Points

- **LangChain4j Overview:** Learn why LangChain4j is a preferred framework for Java-based AI integration, offering a unified interface to various LLM providers.
- **Getting Started:** Project setup via [start.spring.io](https://start.spring.io/), configuration with Maven (Java 24), and running a basic Spring Boot application.
- **Feature Addition:** Use GitHub Copilot to quickly add a `CommandLineRunner` for command-line interactivity.
- **Manual User Input Testing:** Understand how to test non-AI functionality before integration.

## Integrating AI Capabilities

- **Dependency Management:** Utilize Maven's Bill of Materials (BOM) for cleaner management of LangChain4j dependencies across modules.
- **OpenAI SDK Integration:** Choose and add the official OpenAI Java SDK as an LLM backend with LangChain4j.
- **Configuring Chat Models:** Connect your application to Azure AI Foundry and GPT-4o Mini, specifying the base URL, API key, and model name for the chat model.
- **Unified LLM Interface:** Demonstrated usage of the ChatModel interface, which can be pointed at various LLM implementations (OpenAI, Mistral, Llama, or GitHub Models) without code rewrites.

## Practical Demonstration

- **Step-by-Step Demo:**
  - Add required dependencies with Maven.
  - Configure chat model for Azure AI Foundry integration.
  - Retrieve and set up API keys and endpoints.
  - Craft and test an AI-powered poem generator in Spring Boot.
  - Recap session takeaways and next steps for further exploration.

## Resource

- [Java & AI for Beginners Resources](https://aka.ms/JavaAndAIForBeginners)

## Timeline Highlights

- 0:00 – Introduction to intelligent applications
- 2:32 – Maven setup (Java 24)
- 2:52 – Adding features with GitHub Copilot
- 4:28 – Maven BOM for LangChain4j dependencies
- 6:27 – Connecting to Azure AI Foundry
- 8:47 – Live AI prompt creation
- 10:30 – Recap and future roadmap

## Summary

Participants learn to architect and develop AI-driven apps with Java, leveraging Microsoft’s Azure AI services, LangChain4j’s flexible framework, and Copilot-driven productivity to streamline modern development workflows.
