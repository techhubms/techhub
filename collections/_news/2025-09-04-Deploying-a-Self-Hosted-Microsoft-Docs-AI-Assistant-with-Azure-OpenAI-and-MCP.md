---
external_url: https://devblogs.microsoft.com/all-things-azure/build-your-own-microsoft-docs-ai-assistant-with-azure-container-apps-and-azure-openai/
title: Deploying a Self-Hosted Microsoft Docs AI Assistant with Azure OpenAI and MCP
author: Ricardo Macedo Martins
feed_name: Microsoft All Things Azure Blog
date: 2025-09-04 22:09:45 +00:00
tags:
- AI Apps
- All Things Azure
- Azure AI Foundry
- Azure CLI
- Azure Container Apps
- Azure Container Registry
- Azure OpenAI Service
- Cloud Architecture
- Developer Tools
- Docker
- GPT 35 Turbo
- GPT 4
- GPT 4.1
- MCP
- Microsoft Learn
- Mslearn Mcp Chat
- Next.js
- Node.js
- RAG Pipeline
- Secure Deployment
- Serverless Hosting
section_names:
- ai
- azure
- coding
- devops
---
Ricardo Macedo Martins walks developers through deploying a custom AI assistant using Azure OpenAI, Model Context Protocol, and Azure Container Apps, leveraging Microsoft Learn content in a secure, production-ready stack.<!--excerpt_end-->

# Deploying a Self-Hosted Microsoft Docs AI Assistant with Azure OpenAI and MCP

This tutorial outlines the process for building, deploying, and operating a self-hosted AI assistant that provides context-grounded answers from Microsoft Learn using the Model Context Protocol (MCP) and Microsoft's Azure OpenAI platform. The solution leverages scalable, serverless Azure Container Apps and integrates developer-focused workflows for real-world use cases.

## Prerequisites

- Active Azure subscription
- Azure OpenAI resource provisioned with a deployed model (such as GPT-4, GPT-4.1, or GPT-35-Turbo)
- Familiarity with Azure CLI, Docker, and basic cloud architecture

## Solution Overview

- **Model Context Protocol (MCP)** provides structured, trusted content grounding for LLMs
- **Azure OpenAI (GPT-4.1 Mini deployment)** delivers robust natural language synthesis
- **Azure Container Apps** enables secure, scalable, serverless hosting
- **Docker & Azure Container Registry (ACR)** facilitates packaging and image delivery

## Architecture Walkthrough

The solution centers on an agent that pulls verified context from Microsoft Learn/Docs via MCP. After user prompts like "What are best practices for Azure Bicep deployments?", it returns precise, summary answers using GPT-4, all grounded on real documentation.

## Deployment Steps

### 1. Clone the Project

```bash
git clone https://github.com/passadis/mslearn-mcp-chat.git
cd mslearn-mcp-chat
```

### 2. Auto-Discover Azure OpenAI Configuration

Script automatically populates `.env.local`:

```bash
RESOURCE_GROUP=$(az cognitiveservices account list --query "[?kind=='OpenAI'].resourceGroup" -o tsv | head -n1)
AOAI_RESOURCE_NAME=$(az cognitiveservices account list --query "[?kind=='OpenAI'].name" -o tsv | head -n1)
AOAI_ENDPOINT=$(az cognitiveservices account show --name "$AOAI_RESOURCE_NAME" --resource-group "$RESOURCE_GROUP" --query "properties.endpoint" -o tsv)
AOAI_KEY=$(az cognitiveservices account keys list --name "$AOAI_RESOURCE_NAME" --resource-group "$RESOURCE_GROUP" --query "key1" -o tsv)
DEPLOYMENT_NAME=$(az cognitiveservices account deployment list --name "$AOAI_RESOURCE_NAME" --resource-group "$RESOURCE_GROUP" --query "[?contains(properties.model.name, 'gpt-4')].name" -o tsv | head -n1)
cat <<EOF > .env.local
AZURE_OPENAI_KEY=$AOAI_KEY
AZURE_OPENAI_ENDPOINT=$AOAI_ENDPOINT
AZURE_OPENAI_DEPLOYMENT_NAME=$DEPLOYMENT_NAME
EOF
echo ".env.local created"
cat .env.local
```

### 3. Export Variables for CLI Usage

```bash
sed 's/^/export /' .env.local > .env.exported
source .env.exported
```

### 4. Create Azure Resources

```bash
az group create --name rg-mcp-chat --location eastus
az acr create --name acrmcpchat --resource-group rg-mcp-chat --sku Basic --admin-enabled true
```

### 5. Build, Dockerize, and Push

**Dockerfile example:**

```dockerfile
FROM node:20
WORKDIR /app
COPY . .
RUN npm install && npm run build
EXPOSE 3000
CMD ["npm", "start"]
```

**Build & push:**

```bash
docker build -t acrmcpchat.azurecr.io/mcp-chat:latest .
az acr login --name acrmcpchat
docker push acrmcpchat.azurecr.io/mcp-chat:latest
```

### 6. Create Container App Environment

```bash
az containerapp env create --name env-mcp-chat --resource-group rg-mcp-chat --location eastus
```

### 7. Deploy Container App

```bash
az containerapp create \
  --name mcp-chat-app \
  --resource-group rg-mcp-chat \
  --environment env-mcp-chat \
  --image acrmcpchat.azurecr.io/mcp-chat:latest \
  --registry-server acrmcpchat.azurecr.io \
  --cpu 1.0 --memory 2.0Gi \
  --target-port 3000 \
  --ingress external \
  --env-vars \
    AZURE_OPENAI_KEY=$AZURE_OPENAI_KEY \
    AZURE_OPENAI_ENDPOINT=$AZURE_OPENAI_ENDPOINT \
    AZURE_OPENAI_DEPLOYMENT_NAME=$AZURE_OPENAI_DEPLOYMENT_NAME
```

### 8. Retrieve App Public URL

```bash
az containerapp show --name mcp-chat-app --resource-group rg-mcp-chat --query properties.configuration.ingress.fqdn --output tsv
```

Open the provided URL to interact with your AI assistant.

### Example Questions

- "What's the best way to deploy Azure Functions using Bicep?"
- "How does Azure Policy work with management groups?"
- "What’s the difference between vCore and DTU in Azure SQL?"

## Why Model Context Protocol (MCP)?

MCP helps AI assistants anchor their responses to official Microsoft sources, improving reliability by returning context fragments for Retrieval-Augmented Generation (RAG) workflows and summary prompts.

**Sample MCP payload:**

```json
{
  "jsonrpc": "2.0",
  "id": "chat-123",
  "method": "tools/call",
  "params": {
    "name": "microsoft_docs_search",
    "arguments": {
      "question": "How do I deploy AKS with Bicep?"
    }
  }
}
```

## Key Features

- **MCP Integration**: Grounded responses from Microsoft Learn/Docs
- **Azure OpenAI Security**: Backend-only key usage
- **Container Apps**: Scalable and serverless
- **Dev-Focused Stack**: Next.js, Node.js, Docker, Azure CLI, ACR

## References

- [Microsoft Model Context Protocol (MCP)](https://learn.microsoft.com/api/mcp)
- [Azure OpenAI Overview](https://learn.microsoft.com/en-us/azure/ai-foundry/what-is-azure-ai-foundry)
- [Azure Container Apps](https://learn.microsoft.com/en-us/azure/container-apps/overview)
- [GitHub Project – mslearn-mcp-chat](https://github.com/passadis/mslearn-mcp-chat)

Whether you're building an internal developer assistant, an Azure learning tool, or experimenting with custom copilots, the MCP + Azure OpenAI combination is flexible, secure, and quick to deploy with just a few CLI commands.

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/build-your-own-microsoft-docs-ai-assistant-with-azure-container-apps-and-azure-openai/)
