---
layout: post
title: 'Running GenAI in Containers: Dynamic Sessions with Azure Container Apps and LangChain4j'
author: Microsoft Developer
canonical_url: https://www.youtube.com/watch?v=oCNqsEXKnoA
viewing_mode: internal
feed_name: Microsoft Developer YouTube
feed_url: https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g
date: 2025-11-05 17:01:32 +00:00
permalink: /ai/videos/Running-GenAI-in-Containers-Dynamic-Sessions-with-Azure-Container-Apps-and-LangChain4j
tags:
- AI Integration
- AI Workloads
- AIIntegration
- AIWorkloads
- Azure Container Apps
- AzureContainerApps
- Cloud Native
- CloudNative
- Code Execution
- CodeExecution
- Containerized AI
- ContainerizedAI
- Dynamic Sessions
- DynamicSessions
- Java
- Java Development
- JavaAI
- JavaDevelopment
- LangChain4j
- OpenAI
- Persistent Environment
- Python Integration
- Session Pools
- Stateful Containers
- StatefulContainers
section_names:
- ai
- azure
- coding
---
Microsoft Developer, joined by Brian Benz, guides Java developers through using Azure Container Apps Dynamic Sessions and LangChain4j to create persistent, interactive AI code execution environments powered by OpenAI.<!--excerpt_end-->

{% youtube oCNqsEXKnoA %}

# Running GenAI in Containers: Dynamic Sessions with Azure Container Apps and LangChain4j

**Presented by: Microsoft Developer & Brian Benz**

## Introduction

This video concludes the Java and AI for Beginners series by demonstrating how you can run generative AI workloads using stateful containers through Azure Container Apps Dynamic Sessions. Unlike traditional ephemeral containers, dynamic sessions allow your code execution environment to persist, maintaining the context between requests for a smoother, more interactive AI experience.

## What Are Azure Container Apps Dynamic Sessions?

Azure Container Apps Dynamic Sessions provide a mechanism to keep containers running across multiple requests. This enables stateful code execution: for example, AI agents can preserve context, maintain files, and persist data between interactions.

## Integration with LangChain4j

Brian Benz describes his contribution to the LangChain4j ecosystem—a tool that enables seamless integration between Java applications and Azure Container Apps Dynamic Sessions. This allows developers to add persistent, interactive AI capabilities to their Java projects with less complexity.

- **LangChain4j**: A Java-first library for building AI-powered applications with language models.
- **Demo Example**: The session features a practical demonstration, using a question about the volume of a pizza ("If a pizza has radius Z and depth A, what's its volume?") that highlights the system's ability to:
  - Generate and execute Python code within the session
  - Maintain the code execution environment and results across user interactions
  - Upload and download files generated during the session

## How It Works: Code Execution Engine

- **Code execution engine** combines LangChain4j, Azure Container Apps Dynamic Sessions, and OpenAI APIs.
- Developers can initiate a question, receive code from the AI model, send it for execution in a persistent container, and get the results back—without losing context across requests.
- The system supports uploading files, generating code, and context-aware interactions.
- Persistent session pools managed in the Azure Portal support scalable, cloud-native workloads.

## Benefits for Developers

- Persistent execution environment for long-running, interactive AI workloads
- Simplified integration for Java developers using familiar tools
- Access to powerful AI models via OpenAI
- Enhanced user experiences for cloud-native applications

## Topics Covered

- Overview and benefits of dynamic sessions
- LangChain4j repository exploration
- Demo: Running a code execution scenario
- Practical uses for persistent container environments
- Insights into code, architecture, and Azure Portal integration

## Resources

- [Java and AI for Beginners](https://aka.ms/JavaAndAIForBeginners)

## Conclusion

This episode demonstrates modern patterns for running AI workloads in containers, giving Java developers practical tools and strategies for building stateful, real-world AI applications in the cloud.
