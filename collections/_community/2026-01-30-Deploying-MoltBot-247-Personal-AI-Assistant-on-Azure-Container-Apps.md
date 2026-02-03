---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploy-moltbot-to-azure-container-apps-your-24-7-ai-assistant-in/ba-p/4490611
title: 'Deploying MoltBot: 24/7 Personal AI Assistant on Azure Container Apps'
author: dbandaru
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-01-30 00:06:37 +00:00
tags:
- AI
- AI Assistant
- API Integration
- Azure
- Azure CLI
- Azure Container Apps
- Azure Developer CLI
- Azure Log Analytics
- Community
- Container Security
- Deployment Automation
- DevOps
- Discord Bot
- Managed Identity
- MoltBot
- Node.js
- OpenRouter
- Persistent Memory
- RBAC
- Secrets Management
- TypeScript
- Virtual Network
section_names:
- ai
- azure
- devops
---
dbandaru provides a comprehensive tutorial on deploying the open-source AI assistant MoltBot to Azure Container Apps, highlighting security, automation with DevOps tools, and multi-platform integrations for persistent, scalable AI workflows.<!--excerpt_end-->

# Deploying MoltBot: 24/7 Personal AI Assistant on Azure Container Apps

MoltBot is an open-source AI-powered assistant designed to operate continuously, providing seamless communication across platforms such as Discord, Telegram, WhatsApp, Slack, and more. This guide explores how to deploy MoltBot securely and efficiently on Azure Container Apps, highlighting the automation, security, and operational excellence provided by Azure.

## Why Choose Azure Container Apps for MoltBot?

- **Built-in Security Features**: Azure provides managed identity, secrets management, VNet integration, private endpoints, automatic HTTPS (TLS), SOC2/ISO/HIPAA certifications, Defender for Cloud, and RBAC.
- **Zero Maintenance**: No VMs or Kubernetes clusters to manage; Container Apps handle underlying infrastructure.
- **Autoscaling**: Scales down to zero when idle, up automatically based on demand.
- **Integrated Monitoring**: All logs and metrics are available in Azure Log Analytics.
- **Cost-effective**: You pay for what you use; typical costs range from $40-60/month for a 24/7 deployment.

## MoltBot Overview

MoltBot offers:

- **Persistent multi-channel AI assistant**—always on, available via Discord, Telegram, WhatsApp, etc.
- **Memory and skill automation**—remembers preferences, can be taught new automation skills.
- **Execution capabilities**—manages tasks like deploying code, managing files, or fetching information.
- **User privacy and security**—stores credentials securely, only whitelisted users can access via DMs.

## Deployment Steps: Azure Native Approach

### Prerequisites

- Azure Subscription (free tier is sufficient for testing)
- Azure CLI: [Install Guide](https://docs.microsoft.com/cli/azure/install-azure-cli)
- Azure Developer CLI (azd): [Install Guide](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd)
- OpenRouter API Key: [Get It Here](https://openrouter.ai/keys)
- Discord Account (for bot creation)

### Step 1: Set Up Your Discord Bot

- Register your application/bot in [Discord Developer Portal](https://discord.com/developers/applications)
- Enable required intents and permissions; copy the bot token securely
- Generate OAuth2 URL and invite the bot to your server
- Enable Developer Mode in Discord and copy your User ID (for the DM allowlist)

### Step 2: Clone the Deployment Template

```sh
git clone https://github.com/BandaruDheeraj/moltbot-azure-container-apps
cd moltbot-azure-container-apps
```

### Step 3: Azure Infrastructure Provisioning

```sh
azd provision
```

- Prompts for environment name, subscription, and region (suggested: eastus2)
- Sets up Resource Group, Container Registry, Azure Storage, Container Apps Environment, and Log Analytics

### Step 4: Build the MoltBot Container Image

```sh
ACR_NAME=$(az acr list --resource-group rg-MoltBot-prod --query "[0].name" -o tsv)
az acr build --registry $ACR_NAME --image "MoltBot:latest" --file src/MoltBot/Dockerfile src/MoltBot/
```

- Dockerfile pulls MoltBot source, installs dependencies, builds app and UI, and prepares the runtime entrypoint

### Step 5: Configure Secrets

```sh
azd env set OPENROUTER_API_KEY "sk-or-v1-your-key-here"
azd env set DISCORD_BOT_TOKEN "your-discord-bot-token"
azd env set DISCORD_ALLOWED_USERS "your-discord-user-id"
```

- Additional settings for custom AI model, persona name, allowed IPs, and alerting are available

### Step 6: Deploy the App

```sh
azd deploy
```

- Deploys the Container App, injects all secrets and environment variables

### Step 7: Invite Bot & Start Chatting

- Use your previously generated OAuth2 URL to invite the bot
- DMs work only if you and the bot share a server and your User ID is in the allowlist
- Test by messaging "Hello!" to MoltBot

## Security Model: Azure Best Practices

- **Managed Identity** for Azure resources—no plaintext secrets in configs
- **Secrets Management** via Azure Container Apps and environment variables; supports easy rotation with `az containerapp secret set`
- **Network Isolation** with VNet, private endpoints, and IP allowlists; supports internal-only deployments
- **Observability & Alerts** with Log Analytics and preconfigured security notifications (auth errors, restarts, abnormal traffic)
- **RBAC** and audit logging for granular access tracking

## Troubleshooting

- Common issues like missing images, secret application errors, and permission misconfigurations are covered, with specific Azure CLI commands provided for resolutions.
- Model ID errors and Discord permissions are also addressed for easy correction.

## Operational Benefits

- Real-world automation—MoltBot can execute shell commands, connect to email/calendar/integrations, manage files, and perform skill-based automation.
- Supports skill creation via natural language; persistent memory for user context and automation tasks.
- Full extensibility with custom skills and integrations, including support for scheduled tasks and voice integration via ElevenLabs.

## Advanced Azure Operations

- *Log viewing*: Real-time and historical CLI commands for fetching logs
- *Secret rotation*: CLI-driven updates and container restarts—no need for full redeployment
- *Rate limiting*: Protected by messaging platform and API limits; additional throttling possible with API Management

## Clean Up

To remove all resources after testing:

```sh
azd down --purge
```

## Resources

- [MoltBot Documentation](https://docs.molt.bot)
- [Sample Azure Repository](https://github.com/BandaruDheeraj/moltbot-azure-container-apps)
- [Azure Container Apps Documentation](https://learn.microsoft.com/azure/container-apps)
- [MoltBot Discord](https://discord.gg/molt)

---

For questions or feedback, join the [MoltBot Discord](https://discord.gg/molt) or open an issue on the [GitHub repository](https://github.com/BandaruDheeraj/moltbot-azure-container-apps).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploy-moltbot-to-azure-container-apps-your-24-7-ai-assistant-in/ba-p/4490611)
