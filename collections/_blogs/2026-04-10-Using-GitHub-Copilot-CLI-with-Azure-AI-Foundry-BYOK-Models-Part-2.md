---
author: Emanuele Bartolesi
primary_section: github-copilot
tags:
- AI
- API Key
- Azure
- Azure AI Foundry
- Azure OpenAI
- Azure OpenAI Endpoint
- Blogs
- Bring Your Own Model
- BYOK
- Cloud Hosted Models
- Deployment Name
- Endpoint URL
- Environment Variables
- GitHub
- GitHub Copilot
- GitHub Copilot CLI
- Hugging Face Catalog
- Model Deployment
- Model Deployments
- PowerShell
- Programming
- Visual Studio Code Marketplace
- VS Code Extension
section_names:
- ai
- azure
- github-copilot
external_url: https://dev.to/playfulprogramming/using-github-copilot-cli-with-azure-ai-foundry-byok-models-part-2-4e5n
feed_name: Emanuele Bartolesi's Blog
title: Using GitHub Copilot CLI with Azure AI Foundry (BYOK Models) – Part 2
date: 2026-04-10 09:54:00 +00:00
---

Emanuele Bartolesi shows how to point GitHub Copilot CLI at an Azure AI Foundry (Azure OpenAI) deployment using a BYOK-style setup, including how to deploy a model, build the correct endpoint URL, set the required environment variables, and validate the connection.<!--excerpt_end-->

## Context (Part 2)

In [Part 1](https://dev.to/playfulprogramming/using-github-copilot-cli-with-local-models-lm-studio-5e3b), GitHub Copilot CLI was configured to run against a local model using LM Studio.

Local setup trade-offs:

- **Pros**: control, no external calls, no data leaving your machine
- **Cons**: smaller models struggle; output quality drops quickly on non-trivial tasks

This part switches to **Azure AI Foundry** so Copilot CLI can use a cloud-hosted model you control.

## Why Azure AI Foundry for Copilot CLI

Instead of running models locally, you connect Copilot CLI to a model deployed on Azure.

Trade-offs:

- You gain better models and stronger reasoning
- You keep control over the endpoint and deployment
- You accept cost and network dependency

It’s not a replacement for local models—more like the next step if you want privacy *and* more capable models.

## Setting up Azure AI Foundry

This section is the “Azure-heavy” part: get a working endpoint and a deployed model.

### 1) Create or use an existing resource

In the Azure portal, open **Azure AI Foundry**.

You need:

- A resource already created
- Access to it (permissions matter)

If you already use **Azure OpenAI**, you can reuse it.

### 2) Deploy a model

You can’t call a model directly in Azure—you must **deploy it first**.

Steps:

1. Open your resource
2. Go to **Model deployments**
3. Select a model (for example, a GPT-4 class model)
4. Assign a **deployment name**

Example deployment name:

```text
copilot-gpt53codex
```

That **deployment name** is what Copilot CLI uses later.

Notes from the article:

- The model catalog can include options like “GPT 5.3 Codex” and also models from the **HuggingFace catalog**.
- If the Foundry experience is available for your account, you can deploy directly from the model details page via the **Deploy** button.

Images referenced:

- [Azure AI Foundry screenshot](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fiu329t6nsedmxgjgiicq.png)
- [Azure Foundry AI Catalog screenshot](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fxrft73cvv3svfhaq2dmp.png)
- [GPT 5.3 Codex screenshot](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fka6swu8e986uxd0ldsp0.png)
- [Deploy button screenshot](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fvi67ot5cj0v8n55djska.png)

### 3) Get endpoint and API key

From the resource, retrieve:

- **Endpoint URL**
- **API key**

Typical base endpoint format:

```text
https://<your-resource>.openai.azure.com/
```

### 4) Build the final endpoint

Copilot CLI expects a **full path**, not just the base URL.

Format:

```text
https://<your-resource>.openai.azure.com/openai/deployments/<your-deployment>/v1
```

Example:

```text
https://my-ai.openai.azure.com/openai/deployments/copilot-gpt53codex/v1
```

If this path is wrong, the setup won’t work.

### 5) Quick validation

Before using Copilot CLI, validate:

- The deployment exists
- The API key is valid
- The endpoint is reachable

The point: avoid painful debugging later.

## Connecting Copilot CLI to Azure

Same overall pattern as Part 1, but with a different endpoint and more constraints.

### Set the environment variables

You must point Copilot CLI to your Azure deployment.

PowerShell example:

```powershell
$env:COPILOT_PROVIDER_BASE_URL="https://<your-resource>.openai.azure.com/openai/deployments/<your-deployment>/v1"
$env:COPILOT_MODEL="<your-deployment>"
$env:AZURE_OPENAI_API_KEY="<your-api-key>"
```

What matters:

- `COPILOT_PROVIDER_BASE_URL` → full Azure endpoint
- `COPILOT_MODEL` → **deployment name**, not model name
- `AZURE_OPENAI_API_KEY` → required for authentication

No `COPILOT_OFFLINE` here: requests go to Azure.

### Run a simple test

In the project folder you want to test:

```bash
copilot --banner
```

Try a simple task like:

> “Give me the list of all the files larger than 2MB”

If configured correctly:

- The response comes from your Azure AI Foundry model
- The answer should be fast (the author says faster than Part 1)

### Reality check

This is not “Copilot with Azure magic”.

It’s:

- **Copilot CLI** as a thin client
- **Azure** as the model provider

You’re mainly getting better models, not better tooling.

## Appendix: Copilot Insights (VS Code extension)

The author mentions a VS Code extension called **Copilot Insights** to show Copilot plan/quota status inside VS Code (no usage analytics, no productivity scoring).

VS Code Marketplace link:

- https://marketplace.visualstudio.com/items?itemName=emanuelebartolesi.vscode-copilot-insights

Image referenced:

- [Copilot Insights screenshot](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fsqzk5hamyymcmuh515a4.png)

Additional image:

- ![Small icon image](https://media2.dev.to/dynamic/image/width=256,height=,fit=scale-down,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F8j7kvp660rqzt99zui8e.png)


[Read the entire article](https://dev.to/playfulprogramming/using-github-copilot-cli-with-azure-ai-foundry-byok-models-part-2-4e5n)

