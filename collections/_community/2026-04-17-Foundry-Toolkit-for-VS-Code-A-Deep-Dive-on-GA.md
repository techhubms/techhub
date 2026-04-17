---
author: junjieli
primary_section: github-copilot
feed_name: Microsoft Tech Community
title: 'Foundry Toolkit for VS Code: A Deep Dive on GA'
section_names:
- ai
- azure
- github-copilot
- ml
date: 2026-04-17 07:06:47 +00:00
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/foundry-toolkit-for-vs-code-a-deep-dive-on-ga/ba-p/4509510
tags:
- Agent Builder
- Agent Inspector
- AI
- AI Agents
- Azure
- Azure Container Apps
- Community
- Data Wrangler
- Debugger (f5)
- Evaluations
- GitHub Copilot
- GitHub Copilot Skill
- Hosted Agents
- LangGraph
- Local Tracing
- LoRA Fine Tuning
- Microsoft Agent Framework
- Microsoft Foundry Agent Service
- Microsoft Foundry Toolkit
- ML
- Model Catalog
- Model Conversion
- Model Playground
- MSIX Packaging
- NVIDIA TensorRT
- ONNX
- OpenVINO
- Phi
- Phi Silica
- Profiling
- Pytest
- Qualcomm QNN
- Quantization
- VS Code
- VS Code Extension
- VS Code Test Explorer
- Windows ML
---

In this community deep dive, junjieli walks through the GA release of Microsoft Foundry Toolkit for Visual Studio Code—covering model experimentation, agent development (no-code and code-first), evaluations, deployment to Microsoft Foundry Agent Service, and workflows for converting, profiling, and fine-tuning local models on Windows.<!--excerpt_end-->

## Overview

Microsoft Foundry Toolkit for Visual Studio Code is now generally available (GA). This post walks through what shipped in GA, including:

- Extension rebrand and consolidation
- Experimenting with models (cloud and local)
- Building and debugging AI agents
- Running evaluations inside VS Code
- Converting/profiling/fine-tuning models for on-device (Windows) AI

Related links:

- GA announcement: https://aka.ms/mftk-ga-blog
- VS Code Marketplace: https://marketplace.visualstudio.com/items?itemName=ms-windows-ai-studio.windows-ai-studio
- Samples: https://github.com/Azure-Samples/Foundry_Toolkit_Samples
- Lab: https://github.com/microsoft-foundry/Foundry_Toolkit_for_VSCode_Lab
- Issues: https://github.com/microsoft/vscode-ai-toolkit
- Community discussions: https://github.com/microsoft-foundry/discussions

## Experimenting with AI models

### Model Catalog

The toolkit includes a Model Catalog with 100+ models:

- Cloud-hosted models from GitHub, Microsoft Foundry, OpenAI, Anthropic, and Google
- Local model options via ONNX, Foundry Local, or Ollama

### Model Playground

The Model Playground supports:

- Side-by-side model comparisons
- Multimodal testing with file attachments
- Optional web search
- System prompt adjustments
- Streaming responses

### View Code

After testing in the Playground, **View Code** can generate “ready-to-use” snippets in:

- Python
- JavaScript
- C#
- Java

The intent is to produce the API call matching what you just tested, translated into your chosen language.

## Building AI agents: prototype to production

Foundry Toolkit supports two paths, plus an export bridge between them.

### Path A: Prototyper (no code)

**Agent Builder** provides a low-code workflow to create and test an agent quickly:

- Prompt Optimizer to analyze and improve instructions
- Tool Catalog integration (Foundry public catalog and local MCP servers)
- MCP tool approval settings (manual approval vs auto-run)
- Quick switching between agents and auto-save for drafts
- One-click save to Foundry to manage agents centrally

### Path B: Professional team (code-first)

For production-grade systems (multi-agent workflows, orchestration, deployments), the toolkit can scaffold code structures for:

- Microsoft Agent Framework
- LangGraph
- Other orchestration frameworks (as referenced)

#### Agent Inspector (debugging)

Agent Inspector adds development-time visibility:

- Launch under VS Code debugger with **F5** (breakpoints, variable inspection, step-through)
- Visualize streaming responses, tool calls, and workflow graphs
- Jump from workflow nodes to source by double-clicking
- Local tracing of execution spans across tool calls/delegation (no external infra required)

#### Deployment to Microsoft Foundry Agent Service

The toolkit supports one-click packaging and deployment to Microsoft Foundry Agent Service as a hosted agent:

- Hosted agent concept: https://learn.microsoft.com/en-us/azure/foundry/agents/concepts/hosted-agents
- Test from the **Hosted Agent Playground** directly in the VS Code sidebar

### Bridge: export prototype to code

When a no-code Agent Builder prototype is ready for engineering work, it can be exported to code. The generated project includes:

- Agent instructions
- Tool configuration
- Project scaffolding (so teams avoid a full rewrite)

### GitHub Copilot integration

The post highlights **GitHub Copilot with the Microsoft Foundry Skill**, positioned to help generate code that matches:

- Agent Framework patterns
- Evaluation APIs
- Foundry deployment model

## Evaluations in the VS Code workflow

The toolkit provides integrated evaluations to measure agent quality:

- Define evaluations using **pytest**-style syntax
- Run them from **VS Code Test Explorer** alongside unit tests
- Analyze results in a table, with **Data Wrangler** integration
- Submit the same evaluation definitions to run at scale in Microsoft Foundry

The goal is for evaluations to be versioned, repeatable, and CI-friendly.

## On-device (Windows) AI workflow

The post argues for local models when you need:

- Privacy/compliance (data stays on-device)
- Cost control (no per-token billing)
- Offline capability
- Better use of modern Windows hardware

Foundry Toolkit aims to provide an end-to-end workflow on Windows for discovering, running, converting, profiling, and fine-tuning models.

### Local model playground

Model Playground supports local models, including Microsoft’s:

- **Phi** open model family
- **Phi Silica**, a local language model optimized for Windows

It also supports trying any model you’ve converted through the toolkit’s conversion workflow by adding it to **My Resources**.

### Model conversion: Hugging Face to ONNX for Windows ML

Conversion pipeline described:

- Hugging Face → Conversion → Quantization → Evaluation → ONNX

Output is optimized for **Windows ML**, described as Microsoft’s unified runtime for local AI on Windows.

Supported hardware targets (via Windows ML execution provider ecosystem):

- MIGraphX (AMD)
- NvTensorRtRtx (NVIDIA)
- OpenVINO (Intel)
- QNN (Qualcomm)
- VitisAI (AMD)

Windows ML is positioned as enabling automatic acquisition/use of hardware-specific execution providers at runtime, avoiding device-specific code.

After conversion, the toolkit provides:

- Tracked benchmark results (accuracy/latency/throughput) in a History Board
- Sample code showing how to load and run inference with Windows ML
- A “quick playground” web demo generated via GitHub Copilot
- Packaging the converted model as **MSIX**

### Profiling

Profiling tools expose CPU/GPU/NPU/memory usage with per-second granularity and a 10-minute rolling window.

Modes:

- Attach at startup
- Connect to a running process
- Profile an ONNX model directly (tool feeds data and measures performance)

The post also mentions:

- Windows ML Event Breakdown to distinguish startup vs per-request performance
- Operator-level tracing to see which nodes/operators run on NPU/CPU/GPU and their timing

### Fine-tuning Phi Silica with LoRA (cloud training, edge runtime)

The toolkit supports training **LoRA (Low-Rank Adaption)** adapters for Phi Silica using your data without standing up ML infrastructure.

Workflow described:

- Configure LoRA parameters
- Submit a cloud job
- Toolkit spins up **Azure Container Apps** in your subscription to train
- Track training/eval loss curves
- Optional cloud inference for validating adapter behavior

After training:

- Download the adapter
- Integrate it into your app at runtime: https://learn.microsoft.com/en-us/windows/ai/apis/phi-silica-lora?tabs=csharp0

## What GA is aiming to cover

The GA release is positioned as supporting a full lifecycle inside VS Code:

- Explore 100+ models
- Prototype agents without code
- Build production agents with debugging and scaffolding
- Deploy to Microsoft Foundry and test without leaving VS Code
- Run evaluations integrated with test workflows
- Convert/profile/fine-tune models for hardware-specific on-device scenarios


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/foundry-toolkit-for-vs-code-a-deep-dive-on-ga/ba-p/4509510)

