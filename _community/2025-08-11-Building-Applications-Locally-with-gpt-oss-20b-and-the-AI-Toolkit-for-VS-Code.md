---
layout: "post"
title: "Building Applications Locally with gpt-oss-20b and the AI Toolkit for VS Code"
description: "This article offers a detailed walkthrough for developers and enterprises on deploying and building applications with OpenAI's open-source models gpt-oss-20b and gpt-oss-120b using the AI Toolkit for Visual Studio Code. It emphasizes local deployment, model testing, intelligent agent creation, and integration with frameworks like Ollama. Azure AI Foundry integration and best practices for effective development and experimentation are highlighted throughout."
author: "kinfey"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/educator-developer-blog/building-application-with-gpt-oss-20b-with-ai-toolkit/ba-p/4441486"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-11 18:12:15 +00:00
permalink: "/2025-08-11-Building-Applications-Locally-with-gpt-oss-20b-and-the-AI-Toolkit-for-VS-Code.html"
categories: ["AI"]
tags: ["Agent Builder", "AI", "AI Agents", "AI Toolkit", "Azure AI Foundry", "Community", "Edge AI", "Gpt Oss 120b", "Gpt Oss 20b", "Hugging Face", "Inference Frameworks", "Local AI Deployment", "MCP (model Control Protocol)", "Mixture Of Experts", "Model Testing", "MXFP4 Quantization", "Ollama", "OpenAI", "Transformers", "Visual Studio Code", "Vllm", "VS Code Extension"]
tags_normalized: ["agent builder", "ai", "ai agents", "ai toolkit", "azure ai foundry", "community", "edge ai", "gpt oss 120b", "gpt oss 20b", "hugging face", "inference frameworks", "local ai deployment", "mcp model control protocol", "mixture of experts", "model testing", "mxfp4 quantization", "ollama", "openai", "transformers", "visual studio code", "vllm", "vs code extension"]
---

kinfey provides a hands-on guide to deploying OpenAI’s gpt-oss-20b and building intelligent agent applications locally using the AI Toolkit for Visual Studio Code, highlighting practical integration strategies for developers.<!--excerpt_end-->

# Building Applications Locally with gpt-oss-20b and the AI Toolkit for VS Code

OpenAI has introduced the open-source gpt-oss-20b and gpt-oss-120b models, giving enterprises and developers the ability to deploy sophisticated language models on local and edge environments.

## Understanding gpt-oss Models

- **gpt-oss-120b**: 117B parameters, MoE architecture, 80GB GPU requirement, competitive with o4-mini.
- **gpt-oss-20b**: 21B parameters, lower hardware requirement (16GB VRAM), ideal for local deployment, edge, and consumer hardware.
- Both models: 128k context, chain-of-thought reasoning, structured outputs, free commercial use (Apache 2.0), and compatibility with frameworks like vLLM, Ollama, Transformers, Azure AI Foundry, and Hugging Face.

## Prerequisites

- GPU-enabled workstation (16GB+ VRAM for gpt-oss-20b)
- Visual Studio Code with the **AI Toolkit Extension**

## Deployment Workflows

### A. Deploying gpt-oss-20b via AI Toolkit

1. **Access Model Catalog**: In VS Code, after installing AI Toolkit, open Model Catalog using Cmd/Ctrl+Shift+P.
2. **Add Model**: Find gpt-oss-20b and select 'Add Model'.
3. **Deployment**: Toolkit downloads and sets up the model locally. Time: ~15-30 minutes (network dependent).
4. **Verify**: Use the model management interface to confirm operational status.
- Note: Current release requires GPU; CPU support slated for future updates.

### B. Deployment with Ollama Integration

1. **Install Ollama**: Follow OS-specific steps to install Ollama.
2. **Run Model Locally**: Use `ollama run gpt-oss`
3. **Register in AI Toolkit**: Add Ollama deployment as a resource in the Toolkit for streamlined integration.

## Testing and Experimentation

- Use the **AI Toolkit Playground** for side-by-side model tests.
- Compare gpt-oss-20b with other local models like Qwen3-Coder on code generation tasks (e.g., generating an HTML5 Tetris app).
- Run model comparison experiments and evaluate output quality for specific programming prompts.

## Intelligent Agent Construction

- Harness AI Toolkit's **Agent Builder** (GUI) to create and prototype agent applications powered by gpt-oss-20b.
- Combine with MCP (Model Control Protocol) to develop sophisticated, orchestrated AI agent solutions, enabling rapid iteration in local or edge scenarios.

## Security and Compliance

- Models have been evaluated for safety across multiple high-risk domains (e.g., chemical/biological security).
- Released under Apache 2.0: allows unrestricted commercial modification, deployment, and integration for enterprise solutions.

## Resources

- [AI Toolkit](https://aka.ms/aitoolkit)
- [OpenAI’s gpt-oss introduction](https://openai.com/index/introducing-gpt-oss/)
- [Azure AI Foundry integration blog](https://azure.microsoft.com/en-us/blog/openais-open%E2%80%91source-model-gpt%E2%80%91oss-on-azure-ai-foundry-and-windows-ai-foundry/)

_Authored by kinfey (Microsoft Tech Community)_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-application-with-gpt-oss-20b-with-ai-toolkit/ba-p/4441486)
