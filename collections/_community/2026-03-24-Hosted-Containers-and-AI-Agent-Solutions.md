---
author: Lee_Stott
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/hosted-containers-and-ai-agent-solutions/ba-p/4500627
primary_section: ai
feed_name: Microsoft Tech Community
tags:
- AI
- Azd
- Azure
- Azure AI Foundry
- Azure Container Apps
- Azure Container Registry
- Azure Cosmos DB
- Azure Developer CLI
- Azure OpenAI
- Bicep
- Community
- Containerized Deployment
- Containers
- DevOps
- Docker
- Dockerfile
- FastAPI
- Git
- GitHub Repositories
- IaC
- Microsoft Agent Framework
- Microsoft Foundry Hosted Agents
- Multi Agent Systems
- Python
- RAG
- Scale To Zero
- Server Sent Events
- SSE
- Uvicorn
title: Hosted Containers and AI Agent Solutions
date: 2026-03-24 07:00:00 +00:00
section_names:
- ai
- azure
- devops
---

Lee_Stott explains why containerizing AI agents is often the fastest path from a laptop prototype to a production-ready service, using Azure Container Apps and a real multi-agent code verification example (Opustest) to show Docker, IaC, scaling, and deployment with azd.<!--excerpt_end-->

## Overview

If you’ve built an AI agent prototype locally, the jump to “other people can use this” often stalls on environment setup, long-running workloads, and the lack of a stable endpoint. This post argues that containers—specifically on managed platforms like **Azure Container Apps**—are a practical way to close that gap.

## The problem with “it works on my machine”

Local agent prototypes (often a Python script + API key) become hard to share because:

- Teammates need the exact same Python version, dependencies, and environment variables.
- Long-running pipelines tie up a developer machine.
- There’s no reliable URL for others to try the system.

**Containers** address this by packaging runtime, dependencies, and startup behavior into a single image that runs consistently across machines and cloud environments.

## Why containers fit AI agents well

### Long, unpredictable execution times

Agent pipelines can take minutes (for example: retrieve context, import a repo, run multiple agents, generate a report). Managed container platforms can support longer-running requests with configurable timeouts and keep-alive behavior, while many serverless offerings have stricter execution limits.

### Heavy, specialized dependencies

Agent apps often require large ML/LLM SDKs, database drivers, and tools like Git. Bundling them into the image avoids dependency resolution at runtime and reduces environment drift.

### Stateless by design

Many pipelines are stateless request/response flows, which aligns with scaling a container app horizontally (each instance can handle requests independently).

### Reproducible environments

If a pipeline fails in production, containers make it easier to reproduce locally because production and local can run the same image.

## Example: multi-agent code verification with Opustest

A concrete example is **Opustest**, an open-source project using **Microsoft Agent Framework** with **Azure OpenAI** to analyze Python codebases.

### Pipeline stages

1. **Code Example Retrieval Agent** queries **Azure Cosmos DB** for curated examples of good/bad Python code.
2. **Codebase Import Agent** reads Python files from a Git repository cloned on the server.
3. **Four Verification Agents** score:
   - coding standards
   - functional correctness
   - known error handling
   - unknown error handling
   
   Scores range from 0 to 5.
4. **Report Generation Agent** produces an HTML report with fix prompts (exportable for use in a coding assistant).

### App architecture

- **Backend**: FastAPI
- **Progress updates**: Server-Sent Events (SSE) streamed to the browser
- **User flow**: paste a Git URL → watch pipeline stages → receive report

### Screenshots (from the project)

- Landing page

![Landing page showing Git URL input mode](https://raw.githubusercontent.com/leestott/opustest/main/screenshots/01-landing-page.png)

- Local Path mode

![Local Path input mode](https://raw.githubusercontent.com/leestott/opustest/main/screenshots/02-local-path-mode.png)

- Repository URL entered

![Repository URL entered in the input field](https://raw.githubusercontent.com/leestott/opustest/main/screenshots/03-url-entered.png)

- Stage 1: retrieval from Cosmos DB

![Stage 1 code example retrieval in progress](https://raw.githubusercontent.com/leestott/opustest/main/screenshots/04-stage1-retrieval.png)

- Stage 3: verification agents running

![Stage 3 verification agents running](https://raw.githubusercontent.com/leestott/opustest/main/screenshots/05-stage3-verification.png)

- Stage 4: report generation

![Stage 4 report generation](https://raw.githubusercontent.com/leestott/opustest/main/screenshots/06-stage4-report-generation.png)

- Verification complete

![Verification complete with success banner](https://raw.githubusercontent.com/leestott/opustest/main/screenshots/07-verification-complete.png)

- Report detail

![Report showing scores and error table](https://raw.githubusercontent.com/leestott/opustest/main/screenshots/08-report-detail.png)

## Dockerfile example

The post highlights a small Dockerfile to package the backend/frontend plus Git support:

```dockerfile
FROM python:3.12-slim

RUN apt-get update && apt-get install -y --no-install-recommends git \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY backend/ backend/
COPY frontend/ frontend/

RUN adduser --disabled-password --gecos "" appuser
USER appuser

EXPOSE 8000

CMD ["uvicorn", "backend.app:app", "--host", "0.0.0.0", "--port", "8000"]
```

A specific security callout is that the container runs as a **non-root user**.

## Deploying with Azure Developer CLI (azd)

The post describes deploying to **Azure Container Apps** using one command:

```bash
azd up
```

What happens behind the scenes:

- `azd` reads an `azure.yaml` describing the project.
- Infrastructure is provisioned with **Bicep templates**:
  - Container Apps environment
  - Azure Container Registry (ACR)
  - Cosmos DB account
- The Docker image is built and pushed to ACR.
- The container app is deployed.
- Sample data is seeded via a post-provision hook.

Result: a public URL with HTTPS, scaling, and minimal manual infra work.

## Microsoft Hosted Agents (Foundry) vs Azure Container Apps

The post contrasts two approaches:

### Microsoft Foundry Hosted Agent Service

- Fully managed execution (no infra to manage)
- Declarative configuration (prompts/config over custom code)
- Built-in tool ecosystem
- Opinionated runtime/environment

Best fit: prompt-driven agents that align with platform conventions and don’t need custom server logic or bespoke frontends.

### Azure Container Apps

- You package the full application (agents + backend + frontend) as a Docker image
- Full HTTP/control over networking (REST, SSE, WebSockets)
- Any dependencies can be included (system packages, Python libs, Git)
- Portability between local/CI/prod

Best fit: when your agent system is really a full web app and needs custom orchestration, UI, and server-side processing.

### Why Opustest uses Container Apps

The post lists requirements like:

- Custom web UI with real-time progress
- Multi-agent pipeline with arbitrary orchestration logic
- Server-side Git cloning
- SSE streaming
- Custom HTML report generation
- Export button for “Copilot prompts”
- RAG retrieval from Cosmos DB with direct query control

## Five practical benefits of hosted containers for agents

1. **Consistent deployments** (local, CI, and cloud behave the same)
2. **Scaling without re-architecture** (including scale-to-zero)
3. **Isolation between services** (split into multiple containers as needed)
4. **Built-in observability** (logs/metrics/health checks)
5. **Infrastructure as code** (Bicep/Terraform/Pulumi; repeatable deployments)

## Common concerns

- **“Containers add complexity”**: after a prototype grows beyond trivial dependencies, a Dockerfile can be simpler than long setup docs.
- **“Serverless is simpler”**: serverless is good for short tasks; agent pipelines often need minutes and streaming connections.
- **“I don’t want to learn Docker”**: basic Dockerfiles are small and pay off quickly.
- **Cost**: Container Apps supports **scale-to-zero** (pay nothing when idle).

## Getting started checklist

1. **Write a Dockerfile** (include system deps like Git if needed; run as non-root).
2. **Test locally**:

```bash
docker build -t my-agent-app .
docker run -p 8000:8000 --env-file .env my-agent-app
```

3. **Define infrastructure** (Bicep/Terraform or `azd`), including backing services (databases, key vaults, AI endpoints).
4. **Deploy** (via `azd` or CI/CD).
5. **Iterate** (Docker layer caching keeps rebuilds fast).

## Resources

- [Azure Container Apps documentation](https://learn.microsoft.com/azure/container-apps/)
- [Microsoft Foundry Hosted Agents](https://learn.microsoft.com/en-us/azure/foundry/agents/quickstarts/quickstart-hosted-agent)
- [Azure Developer CLI (azd)](https://learn.microsoft.com/azure/developer/azure-developer-cli/)
- [Microsoft Agent Framework](https://github.com/microsoft/agent-framework)
- [Docker getting started guide](https://docs.docker.com/get-started/)
- [Opustest: AI-powered code verification (source code)](https://github.com/leestott/opustest)


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/hosted-containers-and-ai-agent-solutions/ba-p/4500627)

