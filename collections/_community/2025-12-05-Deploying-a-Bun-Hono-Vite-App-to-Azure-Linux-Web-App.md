---
layout: post
title: Deploying a Bun + Hono + Vite App to Azure Linux Web App
author: theringe
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploying-a-bun-hono-vite-app-to-azure-app-service/ba-p/4475356
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-05 00:09:20 +00:00
permalink: /coding/community/Deploying-a-Bun-Hono-Vite-App-to-Azure-Linux-Web-App
tags:
- Azure
- Azure App Service
- Azure PaaS
- Bun
- CI/CD
- Coding
- Community
- Deployment
- DevOps
- Docker
- Full Stack
- Hono
- JavaScript
- Linux Web App
- Node.js
- Serverless
- TypeScript
- Vite
- VS Code
- Web Development
section_names:
- azure
- coding
- devops
---
theringe guides readers through deploying a Bun, Hono, and Vite application to Azure Linux Web App, outlining local development, Azure configuration, and deployment strategies.<!--excerpt_end-->

# Deploying a Bun + Hono + Vite App to Azure Linux Web App

Author: theringe

## Introduction

This guide demonstrates how to deploy a full-stack JavaScript application built with Bun (JavaScript runtime), Hono (API framework), and Vite (front-end tooling) to an Azure Linux Web App. The approach prioritizes speed, efficiency, and alignment with modern AI-accelerated workloads.

## 1. Local Environment Setup

* Use a Docker container for consistent project creation and development.
* Technologies: **Bun**, **Hono**, **Vite**.
* Endpoints to implement:
  - `/` : Root - displays *Hello Bun Hono Vite*
  - `/api/hello` : Static API endpoint
  - `/api/backend` : Dynamic API endpoint returning computed data

### Initialize Project

```bash
docker run --rm -it -v "$PWD":/app -w /app oven/bun:latest bunx create-vite . --template vanilla-ts
```

Follow on-screen prompts to finish creating project files. Once done, stop the dev server with `Ctrl + C`.

### Add Hono Framework

```bash
docker run --rm -it -v "$PWD":/app -w /app oven/bun:latest bun add hono
```

### Key File Operations

- **.vscode/settings.json**: Prevent uploads of `node_modules` and disables in-service build prompts.
- **vite.config.ts**: Configures Vite to use relative base paths.
- **server.ts**: Sets up API routes and static serving via Hono and Bun.
- **startup.sh**: Custom script for Azure startup, handling Bun installation, cleanup, and server launch.

#### Sample server.ts minimal setup

```typescript
import { Hono } from 'hono';
import { serveStatic } from 'hono/bun';
const app = new Hono();
app.get('/api/hello', (c) => c.text('this is api/hello'));
app.get('/api/backend', (c) => {
  const result = 1 + 1;
  return c.json({ message: "this is /api/backend", calc: `1 + 1 = ${result}`, value: result });
});
app.use('/assets/*', serveStatic({ root: './dist' }));
app.use('/*', serveStatic({ root: './dist' }));
app.get('/', serveStatic({ path: './dist/index.html' }));
const port = Number(process.env.PORT ?? 3000);
export default { port, fetch: app.fetch };
```

### Build and Test Locally

Build the Vite frontend:

```bash
docker run --rm -it -v "$PWD":/app -w /app oven/bun:latest bun run build
```

Serve locally:

```bash
docker run --rm -it -v "$PWD":/app -w /app -p 3000:3000 -e PORT=3000 oven/bun:latest bun run start
```

Test in browser at `http://127.0.0.1:3000/`, `/api/hello`, `/api/backend`.

## 2. Deployment to Azure Linux Web App

### Prerequisites

- Azure Linux Web App with **Premium SKU**
- Deploy via VS Code or Azure CLI

### Environment Variables

* `SCM_DO_BUILD_DURING_DEPLOYMENT=false`: Stops build packaging during publish
* `WEBSITE_RUN_FROM_PACKAGE=false`: Prevents running app from package
* `ENABLE_ORYX_BUILD=false`: Disables Azure automatic build, relying on custom script

### Startup Command

Set startup script:

```bash
bash /home/site/wwwroot/startup.sh
```

This script handles:

- Cleaning Oryx-generated artifacts
- Installing Bun if missing
- Performing dependency install and build
- Starting the Bun server

### Deployment Workflow

1. Deploy code to Azure App Service (using VS Code or other method)
2. Wait ~5 minutes for on-server build/startup
3. Access your deployed endpoints at `/`, `/api/hello`, `/api/backend`

## 3. Conclusion

The Bun + Hono + Vite stack is well-suited for high-performance modern JavaScript applications and emerging AI workloads, offering low-latency, scalability, and developer-friendly workflows. Deploying on Azure App Service provides robust managed infrastructure for easy scaling and operational management.

---

**Further Reading**:

- [Azure App Service Documentation](https://learn.microsoft.com/azure/app-service/)
- [Bun.js Documentation](https://bun.sh/docs)
- [Hono Framework](https://hono.dev/)
- [Vite](https://vitejs.dev/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploying-a-bun-hono-vite-app-to-azure-app-service/ba-p/4475356)
