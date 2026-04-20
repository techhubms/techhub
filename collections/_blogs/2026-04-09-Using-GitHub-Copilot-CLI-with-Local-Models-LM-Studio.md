---
section_names:
- ai
- github-copilot
primary_section: github-copilot
title: Using GitHub Copilot CLI with Local Models (LM Studio)
feed_name: Emanuele Bartolesi's Blog
external_url: https://dev.to/playfulprogramming/using-github-copilot-cli-with-local-models-lm-studio-5e3b
author: Emanuele Bartolesi
date: 2026-04-09 11:36:59 +00:00
tags:
- AI
- Blogs
- BYOK Models
- COPILOT MODEL
- COPILOT OFFLINE
- COPILOT PROVIDER BASE URL
- Copilot Quota
- Developer Workflow
- Environment Variables
- Gemma
- GitHub
- GitHub Copilot
- GitHub Copilot CLI
- LM Studio
- Local LLM
- Localhost
- Model Selection
- NVIDIA Nemotron
- Offline AI
- OpenAI Compatible API
- PowerShell
- Privacy
- Qwen
- VS Code Extension
---

Emanuele Bartolesi explains how to run GitHub Copilot CLI against a local LLM via LM Studio’s OpenAI-compatible API, including the exact PowerShell environment variables needed to avoid cloud fallback and when this offline setup is (and isn’t) worth using.<!--excerpt_end-->

# Using GitHub Copilot CLI with Local Models (LM Studio)

Local AI is getting attention for one simple reason: control.

Cloud models are strong and fast, but for many companies and developers—especially when experimenting or working with sensitive code—that is not ideal.

This is where local models come in.

Tools like **LM Studio** let you run LLMs directly on your machine:

- No external calls
- No data leaving your environment or your network

Instead of sending prompts to cloud models, you can point **GitHub Copilot CLI** to a **local model running in LM Studio**.

This setup is not perfect and not officially seamless, but it works well enough for learning, experimentation, and some real workflows.

![Animated demo of Copilot CLI working with a local model in LM Studio](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fotbwrhflq8hriglrvff3.gif)

## What you need

This is not plug-and-play. There are a few moving parts and assumptions.

## GitHub Copilot CLI

You need **GitHub Copilot CLI** installed and working.

Run it with:

```sh
copilot
```

Or, to see the banner in the terminal:

```sh
copilot --banner
```

By default, the CLI uses GitHub-managed models, but you can override that.

> Note: You must be authenticated with GitHub and have access to Copilot.

## LM Studio

**LM Studio** is a simple way to run local LLMs without dealing with raw model tooling.

It provides:

- A UI to download and run models
- A local server that exposes an OpenAI-compatible API
- No need to manually manage inference engines

Once running, it exposes an endpoint like:

```text
http://localhost:1234/v1
```

That endpoint is what Copilot CLI will talk to instead of the cloud.

## A local model (be realistic)

Not all models are equal, and your hardware matters.

A model like:

```text
qwen/qwen3-coder-30b
```

can be enough to get started, but understand the trade-offs:

- Small models → fast, but weaker reasoning
- Large models → better output, but slow or unusable on most laptops

Rough laptop guidance:

- 1B–3B models → OK
- 7B+ models → borderline
- 13B+ → usually not practical without a GPU

The author notes using **nemotron-3-nano-4b** on a laptop, and larger models (like Qwen3 Code) on a more powerful PC.

> Tip: Start small. The goal is understanding the workflow, not benchmarking models.

## Connecting Copilot CLI to LM Studio

This is the part most people get wrong.

Copilot CLI is not designed primarily for local models. You’re using a **BYOK (Bring Your Own Key/Model)** path that is still evolving.

It works, but you need to be precise.

Reference:

- https://docs.github.com/en/copilot/how-tos/copilot-cli/customize-copilot/use-byok-models

## 1) Set the environment variables

You must override the default Copilot provider.

In PowerShell:

```powershell
$env:COPILOT_PROVIDER_BASE_URL="http://localhost:1234/v1" $env:COPILOT_MODEL="google/gemma-3-1b" $env:COPILOT_OFFLINE="true"
```

What they do:

- `COPILOT_PROVIDER_BASE_URL` → points to your local LM Studio server
- `COPILOT_MODEL` → defines which model to use
- `COPILOT_OFFLINE` → prevents fallback to cloud models

If `COPILOT_OFFLINE` is not set, Copilot may silently use cloud models.

![Copilot CLI and LM Studio running together](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fje4fn2f7yla7hryhx611.png)

## 2) Run a simple test

In LM Studio:

1. Open the **Developer** tab.
2. Toggle the **Status** switch to start the server.

![LM Studio Developer settings showing server status](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fgekat46qalagdgh9l6qz.png)

Then:

1. Click **Load Models** and load a model you’ve downloaded.

![LM Studio Load Models screen](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fz1olalk0255pe25xae4j.png)

In a project folder, run:

```sh
copilot --banner
```

Try a simple task, for example:

- "Give me the list of all the files larger than 2MB"

If everything is configured correctly:

- The response comes from your local model
- No external API calls are made

Expect it to be slower than cloud models—it can take seconds, or even minutes, depending on your hardware.

## Reality check

This is not a first-class integration:

- No guarantee of full compatibility
- No optimization for local inference
- No smart routing like GitHub-hosted Copilot

Still, for simple CLI workflows, it’s often “good enough”.

## When this setup makes sense

Use it where it actually gives you an advantage.

### 1) Privacy-sensitive environments

If code cannot leave your machine, this setup is useful:

- Internal tools
- Proprietary scripts
- Regulated environments

You avoid sending prompts and contexts to external services.

### 2) Offline workflows

Useful when you don’t have a stable connection (or any connection at all, like during a flight). Slower, but available.

### 3) Learning and understanding AI

Running locally makes LLM behavior more obvious. You learn:

- Prompt sensitivity
- Model limitations
- Output variability

## When it does NOT make sense

Avoid this setup if you need:

- High accuracy
- Large context handling
- Production-grade reliability

In those cases, cloud models are still the better option.

## Appendix: GitHub Copilot quota visibility in VS Code

The author mentions a small VS Code extension called **Copilot Insights** to show Copilot plan and quota status in VS Code (no usage analytics or productivity scoring).

VS Code Marketplace:

- https://marketplace.visualstudio.com/items?itemName=emanuelebartolesi.vscode-copilot-insights


[Read the entire article](https://dev.to/playfulprogramming/using-github-copilot-cli-with-local-models-lm-studio-5e3b)

