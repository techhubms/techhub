---
layout: "post"
title: "Integrating Foundry Local with GitHub Copilot in Visual Studio Code"
description: "This article provides an in-depth guide to using Foundry Local for AI-assisted development directly within Visual Studio Code. It details how to set up local models that prioritize privacy, outlines supported AI models, and explains integration steps with the AI Toolkit and GitHub Copilot. The focus is on enabling developers to maintain control and flexibility in their workflow while leveraging advanced Microsoft AI features."
author: "Maanav Dalal"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/ai-assisted-development-powered-by-local-models/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2025-09-18 19:00:50 +00:00
permalink: "/news/2025-09-18-Integrating-Foundry-Local-with-GitHub-Copilot-in-Visual-Studio-Code.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Development", "AI Toolkit", "Azure AI Foundry", "Code Generation", "Coding", "Development Tools", "Foundry Local", "FoundryLocal", "GitHub Copilot", "Local AI Models", "Microsoft Phi", "Model Management", "News", "Offline Development", "OpenAI GPT", "Privacy", "Qwen Models", "VS Code", "VS Code Extensions"]
tags_normalized: ["ai", "ai development", "ai toolkit", "azure ai foundry", "code generation", "coding", "development tools", "foundry local", "foundrylocal", "github copilot", "local ai models", "microsoft phi", "model management", "news", "offline development", "openai gpt", "privacy", "qwen models", "vs code", "vs code extensions"]
---

Maanav Dalal introduces a practical guide to integrating Foundry Local with GitHub Copilot in Visual Studio Code, highlighting privacy, local model flexibility, and setup benefits for developers.<!--excerpt_end-->

# Integrating Foundry Local with GitHub Copilot in Visual Studio Code

*Author: Maanav Dalal*

## Introduction

Developers in regulated environments or those with heightened privacy demands often encounter challenges with cloud-based AI tools due to concerns about data privacy, cloud dependencies, and limited customizability. Foundry Local answers these needs by empowering developers to utilize local AI models natively within Visual Studio Code (VS Code) alongside GitHub Copilot, providing enhanced privacy, flexibility, and offline capabilities.

## What is Foundry Local?

Foundry Local is a Microsoft solution that enables seamless integration of local AI models within your development environment. It works with the AI Toolkit extension for VS Code, allowing users to manage and switch between local models easily, without compromising on capability or privacy.

Find more information in the [official documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-local/).

## Video Demonstrations

- [AI Toolkit Integration Overview](https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2025/09/1-104_AIToolkit_updated.mp4)
- [Foundry Local Demo](https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2025/09/AITK-FL-Demo_updated.mp4)

## Setting Up Foundry Local and GitHub Copilot in VS Code

To get started with local AI model development, follow these steps:

1. **Install Visual Studio Code**
   - Download from [code.visualstudio.com](https://code.visualstudio.com/download).
2. **Add GitHub Copilot Extension**
   - Install the [GitHub Copilot extension](https://marketplace.visualstudio.com/items?itemName=GitHub.copilot) from the VS Code Marketplace.
3. **Add AI Toolkit Extension**
   - Install the [AI Toolkit extension](https://marketplace.visualstudio.com/items?itemName=ms-windows-ai-studio.windows-ai-studio).
4. **Open GitHub Copilot Chat**
   - Use the model picker to add a new model and select "Foundry Local via AI Toolkit" as your provider.
5. **Download Local Models**
   - If a chosen model is not already available on your device, AI Toolkit will prompt you to download it.

This setup process is user-friendly and designed for easy switching between different models within your workflow.

## Supported and Recommended Local Models

Foundry Local currently supports a set of performant models ideal for code-related and natural language tasks:

- **Qwen Models**: Multilingual, supporting both code generation and general language tasks.
- **Microsoft Phi**: Optimized for reasoning, code generation, and natural language understanding in a small, efficient package.
- **OpenAI GPT Series**: Advanced models with broad compatibility and strong language capabilities.

## Key Benefits of Foundry Local Integration

- **Privacy-Preserving Development**: Keeps your queries and coding data on your local machine.
- **Cost Efficiency**: Avoid cloud compute and transfer costs; choose local or cloud models as needed.
- **Model Flexibility and Management**: Add, update, or switch between models—use ones provided or bring your own, all managed through a single interface.
- **Seamless VS Code Integration**: Works directly alongside GitHub Copilot for a unified, AI-enhanced developer experience.
- **User-Friendly Workflow**: Intuitive design minimizes setup complexity and streamlines coding tasks with minimal overhead.

## Enhancing Your Coding Workflow

With local models enabled, developers benefit from responsive, privacy-focused AI assistance directly within VS Code. The AI Toolkit supports experimentation and model tweaking, making it easier to adapt to new AI features and optimize development workflows.

- For more on VS Code releases and features, see [the latest release notes](https://code.visualstudio.com/updates/v1_104#_language-model-chat-provider-api).
- To start, download the AI Toolkit from [aka.ms/aitoolkit](https://aka.ms/aitoolkit.).

## Conclusion

The integration of Foundry Local with GitHub Copilot represents a significant advancement for developers who need control over their AI development environments. By allowing local execution of AI models, it supports privacy, customizability, and cost-effective operation while delivering powerful coding assistance tools.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/ai-assisted-development-powered-by-local-models/)
