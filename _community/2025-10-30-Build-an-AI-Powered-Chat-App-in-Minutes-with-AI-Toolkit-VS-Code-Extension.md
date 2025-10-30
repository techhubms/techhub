---
layout: "post"
title: "Build an AI-Powered Chat App in Minutes with AI Toolkit VS Code Extension"
description: "This post guides developers through using the AI Toolkit VS Code extension to rapidly turn AI experiments into fully runnable chat applications. It introduces a project scaffold feature for generating a real-time, multi-room AI chat app powered by GitHub LLMs and optionally scalable with Azure Web PubSub. Readers learn how to start locally and scale seamlessly to Azure using managed cloud infrastructure, automation tools, and secure real-time messaging services."
author: "kevinguo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-an-ai-powered-chat-app-in-minutes-with-ai-toolkit/ba-p/4464967"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-30 04:56:45 +00:00
permalink: "/2025-10-30-Build-an-AI-Powered-Chat-App-in-Minutes-with-AI-Toolkit-VS-Code-Extension.html"
categories: ["AI", "Azure", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Toolkit", "Azure", "Azure App Service", "Azure Developer CLI", "Azure Storage", "Azure Web PubSub", "Backend", "Chat Application", "Cloud Deployment", "Coding", "Community", "Frontend", "GitHub Copilot", "GitHub LLM", "Project Scaffold", "Python", "React", "Real Time Messaging", "Token Authentication", "VS Code Extension", "WebSockets"]
tags_normalized: ["ai", "ai toolkit", "azure", "azure app service", "azure developer cli", "azure storage", "azure web pubsub", "backend", "chat application", "cloud deployment", "coding", "community", "frontend", "github copilot", "github llm", "project scaffold", "python", "react", "real time messaging", "token authentication", "vs code extension", "websockets"]
---

kevinguo explains how developers can use the AI Toolkit VS Code extension to scaffold and deploy real-time AI chat apps, leveraging GitHub models and Azure Web PubSub for scalable, production-ready solutions.<!--excerpt_end-->

# Build an AI-Powered Chat App in Minutes with AI Toolkit VS Code Extension

The AI Toolkit VS Code extension offers developers a quick path from AI experimentation to functional applications.

## Project Scaffold: Rapid App Creation

With the latest AI Toolkit update, you can instantly scaffold a full-featured chat application powered by AI in just a few clicks:

- Start in Model Playground to test models and prompts
- Click **View Code**, then choose the new scaffold option for a chat app using the OpenAI SDK (additional SDK support coming)
- Select a local folder, and the toolkit generates frontend and backend code

## Chat App Features

The generated project, **AI Chat**, includes:

- Multi-room, real-time chat
- AI bot replies via GitHub LLM models (using your GitHub token)
- Unified React frontend and Python backend (runs locally or cloud)
- Optional Azure Web PubSub integration for scalable, broadcast messaging

## Local Development

Get started quickly:

- Prerequisites: Python 3.12+, Node.js 18+, GitHub Personal Access Token with model read permissions
- Setup Steps:

  ```
  pip install -r requirements.txt
  export GITHUB_TOKEN=<your_pat>
  python start_dev.py
  ```

- Open [http://localhost:5173](http://localhost:5173) to chat and see real-time streaming
- Experiment with features using browser split-screen or multiple windows

## Scaling with Azure (No Code Changes Needed)

When ready for production or larger audiences, deploy to Azure without modifying any code:

- One-command cloud deployment: `azd up`
- Managed services provisioned automatically: Azure App Service, Azure Web PubSub, Azure Storage
- Benefits:
  - Scalability for thousands of concurrent users
  - Managed infrastructure: no server maintenance
  - Security via Azure identity integration
  - Automated provisioning with Azure Developer CLI

## Real-Time Architecture: Azure Web PubSub

Azure Web PubSub manages real-time, bi-directional communication:

- Message broadcasting across rooms
- Room/group management
- Flexible event handling with CloudEvents
- Secure client negotiation using tokens

This enables scalable global chat without infrastructure overhead.

## Next Steps

1. Try the AI Toolkit project scaffold in VS Code
2. Explore advanced configuration in [docs/ADVANCED.md](http://docs/ADVANCED.md)
3. Deploy to Azure using `azd up`
4. Learn more about [Azure Web PubSub](https://learn.microsoft.com/azure/azure-web-pubsub/)

---

**Start experimenting today. The new AI Toolkit and Azure Web PubSub workflow helps you build real-time, intelligent applications with minimal setup.**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-an-ai-powered-chat-app-in-minutes-with-ai-toolkit/ba-p/4464967)
