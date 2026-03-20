---
title: Heroku Entered Maintenance Mode — Migrating a Node.js + Redis App to Azure Container Apps
author: simonjj
section_names:
- ai
- azure
- devops
- github-copilot
date: 2026-03-20 14:27:30 +00:00
primary_section: github-copilot
feed_name: Microsoft Tech Community
tags:
- ACR Build
- AI
- Az Containerapp
- Azure
- Azure AI Foundry
- Azure Cache For Redis
- Azure CLI
- Azure Container Apps
- Azure Container Registry
- Azure Monitor
- Community
- Container Apps Jobs
- Dapr
- DevOps
- DNS Cutover
- Dockerfile
- Dynamic Sessions
- Environment Variables
- Express
- GitHub Actions
- GitHub Copilot
- Heroku Migration
- KEDA Autoscaling
- Log Analytics
- Microsoft.App Resource Provider
- Node.js
- Redis
- Revisions
- Scale To Zero
- Secrets Management
- Serverless GPU
- Traffic Splitting
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/heroku-entered-maintenance-mode-here-s-your-next-move/ba-p/4504021
---

simonjj walks through migrating a real Node.js + Redis Heroku app to Azure Container Apps in about 90 minutes, including service mapping, containerization, CLI-driven deployment, and common gotchas. The post also highlights how GitHub Copilot and Azure AI Foundry can support migration and future AI-native workloads.<!--excerpt_end-->

## Overview

Heroku has moved into sustaining engineering (no new features and no new enterprise contracts). If you have production workloads on Heroku, this post argues that **Azure Container Apps (ACA)** is a practical next step—especially for apps with workers, background jobs, or variable traffic.

The author pressure-tested the experience by migrating a **real Node.js + Redis todo app** (Express + Redis, single web process) to Container Apps.

## Why Azure Container Apps maps well to Heroku

A big part of Heroku’s appeal is simple deploys (e.g., `git push`). The equivalent “get something running quickly” path in Container Apps is:

```sh
az containerapp up --name my-app --source . --environment my-env
```

If you have a Dockerfile, it can build and deploy directly without requiring local Docker or a manual registry push.

### Concept mapping

| Heroku | Azure Container Apps |
| --- | --- |
| Dyno | Container App replica |
| Procfile process types | Separate Container Apps |
| Heroku add-ons | Azure managed services |
| Config vars | Environment variables + secrets |
| `heroku run` one-off dynos | Container Apps Jobs |
| Heroku Pipelines | GitHub Actions |
| Heroku Scheduler | Scheduled Container Apps Jobs |

### Built-in platform capabilities called out

- **KEDA-powered autoscaling** (event-driven)
- **Dapr** for service-to-service communication
- **Traffic splitting across revisions** for safer rollouts
- **Scale to zero** to stop paying when idle

> **Simplest path?** If your app is just a straightforward web server and you don’t want containers, the post notes that **Azure App Service** (`az webapp up`) can also be an option. But for many Heroku-style workloads, the author recommends Container Apps.

## A real migration: Node.js + Redis in ~90 minutes

The migration took about **90 minutes end-to-end**, with most of that time waiting for Redis to provision.

### Step 1: Export what you have (Heroku)

Export environment variables, metadata, and add-ons:

```sh
heroku config --json --app my-heroku-app > heroku-config.json
heroku apps:info --app my-heroku-app
heroku addons --app my-heroku-app
```

The config export is the key artifact because it includes the secrets/connection strings the app needs.

### Step 2: Create the Azure backing services

Create Azure equivalents for common Heroku add-ons:

| Heroku add-on | Azure service | CLI command |
| --- | --- | --- |
| Heroku Postgres | Azure Database for PostgreSQL | `az postgres flexible-server create` |
| Heroku Redis | Azure Cache for Redis | `az redis create` |
| Heroku Scheduler | Container Apps Jobs | `az containerapp job create` |
| SendGrid | SendGrid via Marketplace | (Portal) |
| Papertrail / LogDNA | Azure Monitor + Log Analytics | (Enabled by default) |

Example Redis creation (and a timing note):

```sh
az redis create \
  --name my-redis \
  --resource-group my-rg \
  --location swedencentral \
  --sku Basic --vm-size c0
```

Provisioning time callout:

- **Azure Cache for Redis**: ~10–20 minutes
- **Heroku Redis add-on**: ~2 minutes

### Step 3: Containerize (Dockerfile)

If you don’t have a Dockerfile, write one. Example Node Dockerfile:

```dockerfile
FROM node:20-slim
WORKDIR /app
COPY package*.json ./
RUN npm ci --omit=dev
COPY . .
EXPOSE 8080
CMD ["node", "server.js"]
```

If you need help generating a Dockerfile for common stacks, the post suggests using GitHub Copilot with the provided migration repo:

- https://github.com/simonjj/heroku-to-aca

### Step 4: Build, push, deploy (ACR + ACA)

The author uses **Azure Container Registry** (no local Docker required):

```sh
az acr create --name myacr --resource-group my-rg --sku Basic
az acr build --registry myacr --image my-app:v1 .
```

Create the Container App:

```sh
az containerapp create \
  --name my-app \
  --resource-group my-rg \
  --environment my-env \
  --image myacr.azurecr.io/my-app:v1 \
  --registry-server myacr.azurecr.io \
  --target-port 8080 \
  --ingress external \
  --min-replicas 1
```

### Step 5: Wire up config (secrets + env vars)

Gotcha highlighted: **set secrets before referencing them in environment variables**.

```sh
# Set the secret first
az containerapp secret set \
  --name my-app \
  --resource-group my-rg \
  --secrets redis-url="rediss://:ACCESS_KEY@my-redis.redis.cache.windows.net:6380"

# Then reference it
az containerapp update \
  --name my-app \
  --resource-group my-rg \
  --set-env-vars "REDIS_URL=secretref:redis-url"
```

### Step 6: Verify and cut over

- Test the Azure URL, validate routes and data
- Update DNS **CNAME** to point to the **Container Apps FQDN**

## Lessons from the field (common time-wasters)

### Register resource providers first

If you’ve never used Container Apps in the subscription, register providers:

```sh
az provider register --namespace Microsoft.App --wait
az provider register --namespace Microsoft.OperationalInsights --wait
```

### Secrets first, env vars second

The CLI may fail a deployment if you reference a secret that hasn’t been created yet.

### Budget provisioning time

Redis (and other managed services) can take longer to provision than their Heroku counterparts; plan to do backing services in parallel when possible.

## “AI-native” capabilities mentioned after migration

The post calls out that once on Container Apps, you can enable platform features useful for AI workloads:

- **Serverless GPU** (inference workloads):
  - https://learn.microsoft.com/en-us/azure/container-apps/gpu-serverless-overview
- **Dynamic Sessions** (isolated/sandboxed execution environments):
  - https://learn.microsoft.com/en-us/azure/container-apps/sessions

It also mentions pairing Container Apps with **Azure AI Foundry** for accessing models, prompt management, evaluations, and deploying endpoints:

- https://ai.azure.com

## Migration resources linked

- Official Azure guidance:
  - https://learn.microsoft.com/en-us/azure/container-apps/migrate-heroku-overview
- Agent-assisted migration repository (designed to work with GitHub Copilot and other assistants):
  - https://github.com/simonjj/heroku-to-aca
- Azure Container Apps overview:
  - https://learn.microsoft.com/en-us/azure/container-apps/overview

## Cost notes included

The post compares example monthly costs and highlights Container Apps’ **consumption model** and free grant:

- Free grant includes **180,000 vCPU-seconds** and **2 million requests**.
- Emphasis: largest savings are for apps that idle (because of scale-to-zero).

## Suggested “Get started” checklist

1. Inventory: `heroku apps` and `heroku addons`
2. Pick a pilot (non-critical app)
3. Migrate via the official guide or use the Copilot-friendly repo alongside your app


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/heroku-entered-maintenance-mode-here-s-your-next-move/ba-p/4504021)

