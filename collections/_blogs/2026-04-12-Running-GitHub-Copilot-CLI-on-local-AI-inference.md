---
title: Running GitHub Copilot CLI on local AI inference
external_url: https://devopsjournal.io/blog/2026/04/12/Running-GitHub-Copilot-CLI-on-local-AI
feed_name: Rob Bos' Blog
primary_section: github-copilot
date: 2026-04-12 00:00:00 +00:00
tags:
- AI
- AWQ
- Blogs
- BYOK
- Context Window
- CUDA
- Docker
- FlashAttention
- GGUF
- GitHub Copilot
- GitHub Copilot CLI
- Hugging Face
- Intel AI Boost NPU
- Intel Arc
- KV Cache
- LM Studio
- Local Inference
- Microsoft Foundry Local
- NVIDIA RTX PRO 500
- Ollama
- OpenAI Compatible API
- OpenVINO
- PowerShell
- Safetensors
- Text Generation Inference
- Tool Calling
- Vllm
- Vulkan
- Windows
- WSL2
author: Rob Bos
section_names:
- ai
- github-copilot
---

Rob Bos walks through running GitHub Copilot CLI against local OpenAI-compatible inference servers (Ollama, LM Studio, Foundry Local, vLLM/TGI), focusing on the practical constraints (32k context, tool calling, VRAM/KV-cache) and sharing concrete Windows/PowerShell setup and throughput numbers.<!--excerpt_end-->

# Running GitHub Copilot CLI on local AI inference

Rob Bos has been using the **GitHub Copilot CLI** as a terminal assistant, but wanted a setup where prompts and context **don’t leave the machine** (privacy) and where local hardware does the compute instead of cloud-hosted models.

Copilot CLI now supports **BYOK** (bring your own key) mode: you can point it at **any OpenAI-compatible API** endpoint. In practice, to make it usable you need to solve for:

- **Huge system prompt size** (Copilot CLI tool definitions are ~21,000 tokens)
- **Tool calling compatibility** (Copilot CLI relies on tool calls)
- **Context window** (practically **32k** minimum)
- **VRAM pressure from KV cache** at large context sizes
- **Windows networking quirks** (IPv4 vs IPv6 localhost)

## Hardware tested

Machine: **Dell Pro Max 14 MC14250 (Q4 2025)**

- Intel Core Ultra 7 265H
- 32 GB RAM, 1 TB SSD
- AI-capable processors:
  - **Intel Arc 140T (GPU 0)** — 16 GB VRAM (integrated)
  - **NVIDIA RTX PRO 500 Blackwell (GPU 1)** — 6.1 GB VRAM (GDDR)
  - **Intel AI Boost NPU**

## How Copilot CLI BYOK is configured

Copilot CLI reads these environment variables to redirect requests to a local server:

```powershell
$env:COPILOT_PROVIDER_BASE_URL = "http://localhost:11434/v1"
$env:COPILOT_MODEL = "qwen2.5:7b-instruct-32k"
$env:COPILOT_PROVIDER_MAX_PROMPT_TOKENS = "32768"
$env:COPILOT_PROVIDER_MAX_OUTPUT_TOKENS = "8192"
```

Notes:

- `COPILOT_PROVIDER_TYPE` isn’t required (defaults to `openai`).
- No API key is needed for local inference.
- To go back to cloud models, remove the variables.

### Critical constraint: the context window

The Copilot CLI system prompt + tool definitions are roughly **21,000 tokens** before you type anything. If your model has less than **32k context**, Copilot CLI can behave unpredictably:

- prompt truncation
- missing tools mid-chat
- incoherent responses

## What was tested

## Ollama (OpenAI-compatible server) — works with gotchas

Ollama runs an OpenAI-compatible API on port `11434`.

Key gotchas:

- **Base URL must include `/v1`**: `http://localhost:11434/v1` (otherwise 404).
- **Default context windows are too small** (often 2k–4k). Needs 32k.

To force a 32k context window, create a custom `Modelfile`:

```dockerfile
FROM qwen2.5:7b-instruct
PARAMETER num_ctx 32768
```

Create the model:

- `ollama create qwen2.5:7b-instruct-32k -f Modelfile`

Performance settings for large context:

```powershell
[System.Environment]::SetEnvironmentVariable("OLLAMA_FLASH_ATTENTION", "1", "User")
[System.Environment]::SetEnvironmentVariable("OLLAMA_KV_CACHE_TYPE", "q8_0", "User")
```

Restart Ollama after setting these.

Tool calling compatibility:

- Fails (no tool calls): `gemma3`, `mistral:7b`, `qwen2.5-coder:1.5b-base`
- Works: `qwen2.5:7b-instruct`
- Llama 3.1 8B works but quality is described as mediocre

**Verdict:** Works. `qwen2.5:7b-instruct-32k` on the NVIDIA GPU is functional (reported ~13 tok/s during use; later table shows ~11.25 tok/s).

## Microsoft Foundry Local (OpenVINO) — mostly blocked by driver/hardware limits

Installed via:

```powershell
winget install Microsoft.FoundryLocal
```

Foundry Local can target:

- OpenVINO GPU (Intel Arc)
- NPU (Intel AI Boost)
- CUDA (NVIDIA)

But each path hit a major blocker.

### NPU (Intel AI Boost)

- NPU-optimized models cap at `maxInputTokens=3696`.
- Copilot CLI needs ~21k tokens.

This is described as an architectural limitation for this use case.

### OpenVINO GPU (Arc 140T)

- Errors like: `EPContext node not found`.
- Root cause suspected: **driver mismatch**.
  - Installed Arc driver: `32.0.101.8509` (Dell OEM, Nov 2025)
  - Needed by OpenVINO runtime: `32.0.101.8629` (Apr 2026)

### CUDA (NVIDIA)

- 7B models OOM because 6 GB VRAM can’t hold weights + large KV cache (28k+ context).
- 1.5B model fits but is not capable enough for agentic tasks.

**Verdict:** Blocked (driver updates needed for Arc; NPU context too small; NVIDIA VRAM too small for 7B + big context).

## LM Studio (llama.cpp / OpenAI server) — works with careful configuration

Install:

```powershell
winget install ElementLabs.LMStudio --scope user
```

Important configuration points:

### Context window

Defaults to 4096; needs to be raised to **32768**.

### URL (IPv4 vs IPv6)

Use:

- `http://127.0.0.1:1234/v1`

Avoid:

- `http://localhost:1234/v1`

Because LM Studio binds IPv4 only, and on some Windows setups `localhost` resolves to IPv6 `::1`.

### GPU backend selection (Vulkan vs CUDA)

LM Studio prefers CUDA if an NVIDIA GPU exists. To run on Arc via Vulkan, edit:

- `%USERPROFILE%\.lmstudio\.internal\backend-preferences-v1.json`

```json
[{"model_format":"gguf","name":"llama.cpp-win-x86_64-vulkan-avx2","version":"2.13.0"}]
```

But even under Vulkan, LM Studio may prefer the NVIDIA device because it’s “Discrete”. The workaround:

- Settings → Hardware → disable RTX PRO 500

That writes to `hardware-config.json`:

```json
{"json":[["llama.cpp-win-x86_64-vulkan-avx2",{"fields":[{"key":"load.gpuSplitConfig","value":{"strategy":"evenly","disabledGpus":[1],"priority":[],"customRatio":[]}}]}]],"meta":{"values":["map"]}}
```

### Arc-only Vulkan performance issue

Arc 140T has 16 GB but is integrated memory (shared LPDDR5x, ~68 GB/s). Running 7B models saturated bandwidth and froze the system.

Working approach:

- **Offload ~25 layers to NVIDIA GDDR, rest on CPU**.

### Model selection notes

- `qwen2.5.1-coder-7b-instruct` works but is less effective for agentic behavior.
- `qwen2.5-7b-instruct@q5_k_m` gave better results.
- `@q5_k_m` is the quantization level.
- `gemma-4-e4b` (128k context + tool calling) fits context easily, but produced malformed JSON schemas for tool calls.

### The `lms load` “:2” trap

If you run `lms load` while a model is already loaded, LM Studio can create a second instance with a `:2` suffix. Copilot CLI may keep talking to the original instance.

Unload first:

```powershell
lms unload --all
lms load qwen2.5-7b-instruct@q4_k_s --gpu 0.78 --context-length 32768
```

### The `--gpu` trap

`--gpu` is a **0–1 fraction of layers**, not “max”. `--gpu max` can silently fall back to CPU.

Observed heuristic:

- ~1852 MiB GPU memory => CPU mode
- ~4914 MiB GPU memory => proper GPU split

Recommended:

```powershell
lms load qwen2.5-7b-instruct@q4_k_s --gpu 0.78 --context-length 32768
```

Quant choice:

- `@q4_k_s` (~4.46 GB) slightly faster than `@q5_k_m` (~4.78 GB), likely due to more KV cache headroom.

**Verdict:** Works; fastest 7B setup on this machine was `qwen2.5-7b-instruct@q4_k_s` with `--gpu 0.78`.

## NPU situation summary

- NPU is accessible (in this set of tools) only through **OpenVINO**, i.e., via Foundry Local.
- NPU models are capped at ~3,696 input tokens.
- That’s far below the ~21k Copilot CLI system prompt.

Conclusion: NPU is not viable for this Copilot CLI use case today.

## vLLM and TGI (Hugging Face inference stack)

Motivation: PyTorch runtimes (FlashAttention v2, AWQ-Marlin) should be faster than llama.cpp in theory.

### GGUF vs safetensors

- Ollama / LM Studio → llama.cpp → **GGUF**
- vLLM / TGI → PyTorch → **safetensors**

You can’t mix these formats.

For 7B on 6 GB VRAM, the chosen model:

- `Qwen/Qwen2.5-7B-Instruct-AWQ` (4-bit AWQ), ~4.5 GB on disk

### vLLM via Docker

Command used:

```bash
docker run -d --runtime nvidia --gpus all --name vllm-server `
  -v hf-cache:/root/.cache/huggingface `
  -p 127.0.0.1:8000:8000 --shm-size 2g `
  vllm/vllm-openai:latest `
  --model Qwen/Qwen2.5-7B-Instruct-AWQ --quantization awq_marlin `
  --max-model-len 32768 --gpu-memory-utilization 0.81 `
  --kv-cache-dtype fp8 --max-num-seqs 1 `
  --cpu-offload-gb 2 --enforce-eager `
  --dtype auto --host 0.0.0.0 --port 8000
```

Key constraints called out:

- After CUDA init, only ~4.89 GiB is free on a 5.97 GiB card.
- AWQ weights alone would need ~5.2 GiB on GPU.
- `--cpu-offload-gb 2` reduces on-GPU weights to ~3.17 GiB.
- `--kv-cache-dtype fp8` is needed to make 32k context feasible.
- Use a Docker volume for HF cache rather than Windows filesystem mounts.
- Bind to `127.0.0.1:8000`.

Outcome:

- Works, FlashAttention v2 + AWQ-Marlin load.
- But throughput: **~2.6 tok/s** (CPU offload over PCIe is the bottleneck).

**Verdict:** Works but impractical for interactive Copilot CLI use on 6 GB.

### TGI (Text Generation Inference)

- Needs whole model in VRAM.
- No CPU offload flag like vLLM.
- OOM when allocating KV cache.

Also mentioned: a WSL2 Triton linker issue needing:

- `-e LIBRARY_PATH=/usr/local/cuda-12.4/compat`

…but even with that, the VRAM issue remains.

**Verdict:** OOM on 6 GB for the target model/context.

## PowerShell profile setup to switch providers

The author created a profile setup with:

- `set-copilot-local` — interactive menu to select provider/model and set environment variables
- `online-copilot` — clears env vars to go back to the default online models

It includes helpers:

- `Start-LMStudio`
- `Set-LMStudioArcGPU` (writes `hardware-config.json`)

A gist with the throughput benchmarking script is linked:

- `Measure-LocalAIThroughput.ps1`: https://gist.github.com/rajbos/8c9a5bfb832469db52482082f88aae06

## Summary: what worked (April 2026)

The article’s summary table (condensed):

- Online Copilot (GitHub-hosted Claude/GPT): ✅ best
- LM Studio `qwen2.5-7b-instruct@q4_k_s` + `--gpu 0.78`: ✅ works, **17.2 tok/s**
- LM Studio `qwen2.5-7b-instruct@q5_k_m`: ✅ works, **13.6 tok/s**
- Ollama `qwen2.5:7b-instruct-32k`: ✅ works, **11.25 tok/s**
- vLLM `Qwen2.5-7B-Instruct-AWQ` + CPU offload: ⚠️ works but too slow, **2.6 tok/s**
- TGI with same model: ❌ OOM
- Foundry Local on Arc (OpenVINO GPU): ❌ blocked by driver version
- Foundry Local on NPU: ❌ context too small (3696 tokens)
- Foundry Local CUDA 7B on NVIDIA: ❌ OOM
- LM Studio on Arc-only Vulkan: ❌ too slow / system freezes (memory bandwidth)

## Throughput benchmark methodology

- Task: generate a ~250-token PowerShell function
- Settings: `temperature=0.2`, `max_tokens=400`
- Process: 1 warm-up run, then 5 measured runs averaged
- Measures **output throughput** (decode speed), not time-to-first-token.

## Fastest local setup on this machine

Best practical configuration found:

- **LM Studio**
- Model: `qwen2.5-7b-instruct@q4_k_s`
- Load: `--gpu 0.78 --context-length 32768`
- Result: **17.2 tok/s**

Steps:

1. Install LM Studio:

   ```powershell
   winget install ElementLabs.LMStudio --scope user
   ```

2. Download GGUF model:

   ```powershell
   lms get bartowski/Qwen2.5-7B-Instruct-GGUF:Q4_K_S
   ```

3. Load with GPU split and 32k context:

   ```powershell
   lms load qwen2.5-7b-instruct@q4_k_s --gpu 0.78 --context-length 32768
   ```

4. Point Copilot CLI at the local endpoint using the profile function (`set-copilot-local`).

## What the author is waiting for

Most promising future path:

- **Foundry Local** using **OpenVINO** on **Intel Arc 140T**

But it’s blocked until Dell ships Arc driver **32.0.101.8629** for the MC14250.


[Read the entire article](https://devopsjournal.io/blog/2026/04/12/Running-GitHub-Copilot-CLI-on-local-AI)

