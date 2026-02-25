---
layout: "post"
title: "Hosting OpenClaw on Azure App Service: Full Guide"
description: "This in-depth tutorial walks through deploying the open-source personal AI assistant OpenClaw to Microsoft Azure App Service, leveraging Azure OpenAI, managed identity, persistent storage, and enterprise security features. Readers learn how to replace local hosting with a scalable, always-on, secure, and maintainable cloud architecture using Azure-native tools and best practices."
author: "jordanselig"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/you-can-host-openclaw-on-azure-app-service-here-s-how/ba-p/4496563"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-23 21:40:44 +00:00
permalink: "/2026-02-23-Hosting-OpenClaw-on-Azure-App-Service-Full-Guide.html"
categories: ["AI", "Azure"]
tags: ["AI", "App Service Deployment", "App Service Plans", "Azure", "Azure App Service", "Azure CLI", "Azure Developer CLI", "Azure Files", "Azure OpenAI", "Bicep Templates", "CI/CD", "Cloud Hosting", "Community", "Container Registry", "Discord Bot", "GPT 4o", "Log Analytics", "Managed Identity", "OpenClaw", "Persistent Storage", "Security Best Practices", "Telegram Bot", "WebSockets"]
tags_normalized: ["ai", "app service deployment", "app service plans", "azure", "azure app service", "azure cli", "azure developer cli", "azure files", "azure openai", "bicep templates", "cislashcd", "cloud hosting", "community", "container registry", "discord bot", "gpt 4o", "log analytics", "managed identity", "openclaw", "persistent storage", "security best practices", "telegram bot", "websockets"]
---

jordanselig details a practical approach to running the open-source OpenClaw AI assistant 24/7 on Azure App Service. The guide covers architecture, deployment steps, security, persistent storage, cost factors, and how Azure OpenAI and managed identity streamline and secure the setup.<!--excerpt_end-->

# Hosting OpenClaw on Azure App Service: A Comprehensive Guide

OpenClaw is an open-source personal AI assistant that can run persistently, connected to Discord, Telegram, and other platforms. Traditionally self-hosted on local hardware, OpenClaw (and similar always-on AI tools) can be challenging to maintain. This guide demonstrates how to host OpenClaw entirely in Azure using App Service, taking advantage of Azure's managed infrastructure, built-in security, persistent storage, and seamless integration with Azure OpenAI.

## Why Host OpenClaw in Azure Cloud?

Local hosting (such as leaving a Mac Mini running) ties up personal hardware, increases costs, is vulnerable to outages, and raises security concerns (e.g., ports exposure, lack of TLS, plaintext secrets). Azure App Service allows a 24/7, reliable, and secure alternative:

- Always-on operation with no local machine dependency
- Managed TLS, authentication, virtual networking, and endpoint controls
- Centralized logging, monitoring, and cost predictability
- Persistent storage using Azure Files
- Azure OpenAI integration for native LLM access

## Azure App Service vs. Container Apps

Both services host containers well. App Service is well-suited for single-container, always-on bots with features like built-in scaling, SSH access, deployment slots, WebSockets, and integrated security. Container Apps targets microservices with KEDA-based event-driven scaling. Choose the one fitting your needs; this article covers App Service.

| Feature              | App Service           | Container Apps            |
|----------------------|----------------------|---------------------------|
| Container model      | Single container     | Multi-container/sidecars  |
| Scaling              | Auto-scale rules     | KEDA event-driven         |
| Always On            | Yes, default         | Configure min replicas    |
| SSH access           | Yes                  | No                        |
| Deployment slots     | Yes                  | Revisions                 |
| Use case             | Web apps, bots       | Microservices, events     |

## Architecture Overview

- **Azure Container Registry (ACR):** Stores the custom OpenClaw Docker image
- **Azure App Service (Web App for Containers):** Hosts and runs the OpenClaw container
- **Azure OpenAI (GPT-4o):** Provides LLM API, provisioned alongside other resources
- **Azure Files:** Mounted for persistent conversation history and agent data
- **Log Analytics & Azure Monitor:** Consolidated logs, health monitoring, and alerting
- **Managed Identity:** A secure mechanism for ACR image pulls (no passwords in config)

### Diagram Highlights

- Bot connects outbound to Discord/Telegram APIs (no inbound exposure)
- Enable VNet integration, private endpoints, IP restrictions, and Easy Auth (Entra ID) for layered security
- Web chat (Control UI) access is protected by tokens and/or IP restrictions

## Step-by-Step Deployment

### Prerequisites

- Azure subscription
- Azure CLI and Azure Developer CLI (azd) installed
- Discord and/or Telegram bot credentials

### 1. Clone and Configure

```bash
git clone https://github.com/seligj95/openclaw-azure-appservice.git
cd openclaw-azure-appservice
azd auth login
az login
azd init -e dev
```

### 2. Set Secrets

```bash
# Discord

a zd env set DISCORD_BOT_TOKEN "your-discord-bot-token"
azd env set DISCORD_ALLOWED_USERS "123456789,987654321"

# Telegram (optional)

azd env set TELEGRAM_BOT_TOKEN "your-telegram-bot-token"
azd env set TELEGRAM_ALLOWED_USER_ID "your-user-id"
```

### 3. Deploy

```bash
azd up
```

This creates all necessary resources (App Service, ACR, Azure Files, Azure OpenAI, etc.), builds the Docker image, and deploys everything with one command.

### 4. Launch and Validate

- Wait a few minutes for the container to deploy and pass health checks
- Check health endpoint: `curl https://<your-app>.azurewebsites.net/health`
- Tail logs: `az webapp log tail --name <name> --resource-group <rg>`
- Interact via Discord/Telegram

## Storage, Logging, and Updates

- Data persistency via Azure Files mounted at `/mnt/openclaw-workspace`
- Centralized logs collected by Log Analytics (errors, usage, console output)
- Update OpenClaw by rebuilding the Docker image (`az acr build ...`) and redeploy/restart
- Remove resources: `azd down --purge --force`

## Security Practices

- No passwords in config—ACR pulls use managed identity
- HTTPS enforced; minimum TLS 1.2
- Private endpoints for storage and OpenAI
- Secrets stored as Azure App Service settings, encrypted at rest
- Recommend IP restrictions and/or Easy Auth for web UI

## Cost Estimates

- App Service: ~$77/mo (P0v4; cheaper basic plans available)
- Azure OpenAI: pay-per-usage
- Container Registry: $5/mo (Basic)
- Storage/Logs: minimal (~$5/mo total)
- Cheaper plan possible (~$20–25/mo) with reduced isolation/features

## Key Takeaways

- Azure App Service is a robust, secure way to host always-on containers like OpenClaw—no desktop or specialized hardware required
- End-to-end deployment and maintenance with standard Azure tools and templates
- Security, monitoring, cost management, and persistent storage are built into the solution

## Resources

- [Full source code on GitHub](https://github.com/seligj95/openclaw-azure-appservice)
- [Dheeraj's original Container Apps deployment guide](https://www.agent-lair.com/deploy-clawdbot-azure-container-apps)
- [Azure App Service Documentation](https://learn.microsoft.com/azure/app-service/)
- [Azure OpenAI Documentation](https://learn.microsoft.com/azure/ai-services/openai/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/you-can-host-openclaw-on-azure-app-service-here-s-how/ba-p/4496563)
