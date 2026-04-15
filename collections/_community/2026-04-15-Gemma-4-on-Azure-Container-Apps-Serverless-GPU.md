---
tags:
- AI
- Azd
- Azure
- Azure CLI
- Azure Container Apps
- Azure Developer CLI
- Basic Authentication
- Chat Completions API
- Community
- Gemma 4
- GPU Workload Profiles
- HIPAA
- HTTPS Endpoint
- Nginx
- NVIDIA A100
- NVIDIA T4
- Ollama
- OpenAI Compatible API
- OpenCode
- Q4 K M
- Quantization
- Regulated Industries
- Reverse Proxy
- Scale To Zero
- Security
- Self Hosted LLM
- Serverless GPU
- SOC 2
- Terminal Coding Agent
- Tokens Per Second
- TTFT
author: simonjj
feed_name: Microsoft Tech Community
title: Gemma 4 on Azure Container Apps Serverless GPU
primary_section: ai
date: 2026-04-15 16:20:22 +00:00
section_names:
- ai
- azure
- security
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/gemma-4-on-azure-container-apps-serverless-gpu/ba-p/4511671
---

simonjj shares an Azure Developer CLI template that deploys Google’s Gemma 4 (via Ollama) onto Azure Container Apps serverless GPU with an OpenAI-compatible endpoint, protected by an Nginx basic-auth proxy, plus steps to verify the API and wire it into the OpenCode terminal coding agent for private, in-subscription prompting.<!--excerpt_end-->

## Secure, private and performant OpenCode agent configuration — batteries included

Every prompt you send to a hosted AI service leaves your tenant. Your code, your architecture decisions, your proprietary logic — all of it crosses a network boundary you don't control. For teams building in regulated industries or handling sensitive IP, that's not a philosophical concern. It's a compliance blocker.

This template aims to let you spin up a fully private AI coding agent — running on your own GPU, in your own Azure subscription — with a single command.

**One `azd up`, ~15 minutes, and you have Google’s Gemma 4 running on Azure Container Apps serverless GPU with an OpenAI-compatible API, protected by auth, and ready to power OpenCode as a terminal-based coding agent.**

## Why self-hosted AI on Azure Container Apps (ACA)?

Azure Container Apps serverless GPU is positioned as on-demand GPU compute without managing VMs, Kubernetes clusters, or GPU drivers. You deploy a container and get a GPU-backed HTTPS endpoint.

Compared to calling a hosted model API, the post highlights:

- **Complete data privacy**: prompts/code stay inside your Azure subscription.
- **Predictable costs**: pay for GPU compute time rather than per-token.
- **No rate limits**: you control the GPU capacity.
- **Model flexibility**: switch between model sizes depending on needs.

> **This isn't a tradeoff between convenience and privacy.** ACA serverless GPU is presented as making self-hosted AI nearly as simple to deploy as SaaS, while keeping the data boundary under your control.

## What you’re building

The template deploys two containers into an Azure Container Apps environment:

1. **Ollama + Gemma 4** — runs on a serverless GPU (NVIDIA T4 or A100) and serves an **OpenAI-compatible API**.
2. **Nginx auth proxy** — a lightweight reverse proxy that adds **basic authentication** and exposes the endpoint over **HTTPS**.

Notes:

- The Ollama container pulls the Gemma 4 model on first start (no pre-build/upload).
- The Nginx proxy runs on the free Consumption profile; the Ollama container is the one that needs the GPU.

After deployment, you get a single HTTPS endpoint usable via `curl`, OpenAI-compatible SDKs, or **OpenCode** (a terminal-based coding agent, described as a private GitHub Copilot alternative).

## Step 1: Deploy with `azd up`

Prerequisites:

- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Azure Developer CLI (azd)](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/)

Commands:

```bash
git clone https://github.com/simonjj/gemma4-on-aca.git
cd gemma4-on-aca
azd up
```

During setup you choose:

- **GPU selection**: T4 (16 GB VRAM) or A100 (80 GB VRAM)
- **Model selection**: defaults tuned for a quality/speed balance per GPU tier
- **Proxy password**: used for basic auth

Region note (GPU serverless availability varies). The post points to the supported regions list:

- [Serverless GPU supported regions](https://learn.microsoft.com/en-us/azure/container-apps/gpu-serverless-overview#supported-regions)

Examples listed include: `australiaeast`, `brazilsouth`, `canadacentral`, `eastus`, `italynorth`, `swedencentral`, `uksouth`, `westus`, `westus3`.

Provisioning is described as taking ~10 minutes, mostly for ACA environment creation and model download.

## Choose your model

Gemma 4 sizes covered:

| Model | Params | Architecture | Context | Modalities | Disk size |
| --- | --- | --- | --- | --- | --- |
| `gemma4:e2b` | ~2B | Dense | 128K | Text, Image, Audio | ~7 GB |
| `gemma4:e4b` | ~4B | Dense | 128K | Text, Image, Audio | ~10 GB |
| `gemma4:26b` | 26B | MoE (4B active) | 256K | Text, Image | ~18 GB |
| `gemma4:31b` | 31B | Dense | 256K | Text, Image | ~20 GB |

### Real-world performance on ACA

Benchmark details in the post:

- Ollama v0.20
- Q4_K_M quantization
- 32K context
- Sweden Central

| Model | GPU | Tokens/sec | TTFT | Notes |
| --- | --- | --- | --- | --- |
| `gemma4:e2b` | T4 | ~81 | ~15ms | Fastest on T4 |
| `gemma4:e4b` | T4 | ~51 | ~17ms | Default T4 choice (quality/speed) |
| `gemma4:e2b` | A100 | ~184 | ~9ms | Ultra-fast |
| `gemma4:e4b` | A100 | ~129 | ~12ms | Good for lighter workloads |
| `gemma4:26b` | A100 | ~113 | ~14ms | Default A100 choice (reasoning) |
| `gemma4:31b` | A100 | ~40 | ~30ms | Highest quality, slower |

The post calls out:

- **~51 tokens/sec on T4 with the 4B model** as interactive enough for coding.
- **~113 tokens/sec on A100 with the 26B model** with better reasoning for harder tasks.

Constraint:

> The 26B and 31B models require A100 (they don’t fit in T4’s 16 GB VRAM).

## Step 2: Verify your endpoint

After `azd up`, the post-provision hook prints the endpoint URL.

Example `curl` (use your actual endpoint and password):

```bash
curl -u admin:<YOUR_PASSWORD> \
  https://<YOUR_PROXY_ENDPOINT>/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{
    "model": "gemma4:e4b",
    "messages": [{"role": "user", "content": "Hello!"}]
  }'
```

Expected result: a JSON response from Gemma 4. The endpoint is intended to be compatible with tools/SDKs that speak the OpenAI API format.

## Step 3: Connect OpenCode

[OpenCode](https://opencode.ai) is described as a terminal-based AI coding agent ("think GitHub Copilot, but running in your terminal") and can point at an OpenAI-compatible backend.

The `azd up` post-provision hook generates an `opencode.json` with endpoint + credentials. If you create it manually, the post gives a config like:

```json
{
  "$schema": "https://opencode.ai/config.json",
  "provider": {
    "gemma4-aca": {
      "npm": "@ai-sdk/openai-compatible",
      "name": "Gemma 4 on ACA",
      "options": {
        "baseURL": "https://<YOUR_PROXY_ENDPOINT>/v1",
        "headers": {
          "Authorization": "Basic <BASE64_OF_admin:YOUR_PASSWORD>"
        }
      },
      "models": {
        "gemma4:e4b": {
          "name": "Gemma 4 e4b (4B)"
        }
      }
    }
  }
}
```

Generate the Base64 auth value:

```bash
echo -n "admin:YOUR_PASSWORD" | base64
```

Run a one-off prompt:

```bash
opencode run -m "gemma4-aca/gemma4:e4b" "Write a binary search in Rust"
```

For an interactive session, start the TUI:

```bash
opencode
```

Then use `/models` to select the configured model.

## The privacy case

The post frames this as most useful when you can’t send code to external APIs, giving examples:

- HIPAA-regulated healthcare apps
- Financial services
- Defense/government
- Startups with sensitive IP

Core claim: with ACA serverless GPU you get a managed container with a GPU, avoiding VM/Kubernetes management while keeping prompts/code inside your subscription boundary.

## Clean up

Tear down resources:

```bash
azd down
```

The post also notes you can **scale to zero replicas** to pause costs without destroying the environment (since billing is tied to containers running).

## Get started / references

- [gemma4-on-aca on GitHub](https://github.com/simonjj/gemma4-on-aca)
- [OpenCode](https://opencode.ai)
- [Gemma 4 docs](https://ai.google.dev/gemma/docs/core)
- [ACA serverless GPU workload profiles](https://learn.microsoft.com/en-us/azure/container-apps/workload-profiles-overview#gpu-workload-profiles)

Updated Apr 15, 2026 — Version 2.0


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/gemma-4-on-azure-container-apps-serverless-gpu/ba-p/4511671)

