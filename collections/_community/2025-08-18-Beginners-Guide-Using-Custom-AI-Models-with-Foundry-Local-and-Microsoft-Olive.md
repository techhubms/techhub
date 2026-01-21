---
external_url: https://techcommunity.microsoft.com/t5/educator-developer-blog/how-to-use-custom-models-with-foundry-local-a-beginner-s-guide/ba-p/4428857
title: 'Beginner’s Guide: Using Custom AI Models with Foundry Local and Microsoft Olive'
author: kinfey
feed_name: Microsoft Tech Community
date: 2025-08-18 07:00:00 +00:00
tags:
- AI Chatbot
- CPU
- DeepSeek Models
- Foundry Local
- GGUF
- GPU
- Hugging Face
- INT4
- Local Inference
- Mac
- Microsoft Olive
- Model Compression
- Model Conversion
- ONNX
- Phi Models
- Python
- PyTorch
- Qwen Models
- Qwen3 0.6B
- Transformers
- Windows
section_names:
- ai
---
kinfey explains how to set up and run your own AI language models locally using Foundry Local and Microsoft Olive, covering installation, model format conversion, file organization, and troubleshooting for beginners.<!--excerpt_end-->

# Beginner’s Guide: Using Custom AI Models with Foundry Local and Microsoft Olive

**Author:** kinfey

## Overview

This guide shows you how to run your own small AI language models on your Windows or Mac computer using Foundry Local. It covers converting models (like Qwen3-0.6B) for local use with Microsoft Olive, along with detailed steps for setup, conversion, and troubleshooting.

## What is Foundry Local?

Foundry Local lets you run AI models locally, providing privacy and performance benefits. It works with model families such as:

- **Phi models** (by Microsoft)
- **Qwen models** (by Alibaba)
- **DeepSeek models**

## Why Model Conversion?

Models are usually distributed in **PyTorch** format for training/portability but should be converted for efficient local inference. Two main formats:

- **GGUF:** Simple, memory-efficient, CPU-only, best for basic or older setups
- **ONNX:** Professional, hardware-accelerated, supports GPU/NPU, industry standard

For beginners, ONNX is recommended for better performance and future flexibility.

## Microsoft Olive: Your Model Conversion Helper

Microsoft Olive is a tool that automates conversion and compression (supports formats like INT4, INT8, FP16). It works on all major hardware and streamlines the conversion process.

### Key Benefits

- Handles conversion options and hardware compatibility for you
- Automates complex command-line steps
- Works with Hugging Face model zoo and other sources

## Step-by-Step Conversion Guide

### 1. Install Required Tools

Open your terminal/command prompt and run:

```bash
pip install transformers -U
pip install git+https://github.com/microsoft/Olive.git
git clone https://github.com/microsoft/onnxruntime-genai
cd onnxruntime-genai && python build.py --config Release
pip install {Your build release path}/onnxruntime_genai-0.9.0.dev0-cp311-cp311-linux_x86_64.whl
```

- Ensure **CMake >= 3.31** is installed ([cmake.org](https://cmake.org/download/))

### 2. Convert the Model the Easy Way

```
olive auto-opt \
  --model_name_or_path {Your Qwen3-0.6B Path} \
  --device cpu \
  --provider CPUExecutionProvider \
  --use_model_builder \
  --precision int4 \
  --output_path models/Qwen3-0.6B/onnx \
  --log_level 1
```

- `--precision int4` reduces size for efficiency
- Swap `cpu` for `cuda` if you want GPU acceleration

### 3. Advanced Configuration (Optional)

Create `conversion_config.json`:

```json
{
  "input_model": {
    "type": "HfModel",
    "model_path": "Qwen/Qwen3-0.6B",
    "task": "text-generation"
  },
  "systems": {
    "local_system": {
      "type": "LocalSystem",
      "accelerators": [
        { "execution_providers": ["CPUExecutionProvider"] }
      ]
    }
  },
  "passes": {
    "builder": { "type": "ModelBuilder", "config": { "precision": "int4" } }
  },
  "host": "local_system",
  "target": "local_system",
  "cache_dir": "cache",
  "output_dir": "model/output/Qwen3-0.6B-ONNX"
}
```

Run:

```
olive run --config ./conversion_config.json
```

You'll need to log in to Hugging Face for downloading models:

```
huggingface-cli login
```

## Setting Up the Converted Model in Foundry Local

1. Install Foundry Local and convert your model as above
2. Navigate to your models folder:
   - `foundry cache cd ./models/`
3. Create a chat template `inference_model.json`:

```json
{
  "Name": "Qwen3-0.6b-cpu",
  "PromptTemplate": {
    "system": "<|im_start|>system\n{Content}<|im_end|>",
    "user": "<|im_start|>user\n/think{Content}<|im_end|>",
    "assistant": "<|im_start|>assistant\n{Content}<|im_end|>",
    "prompt": "<|im_start|>user\n/think{Content}<|im_end|>\n<|im_start|>assistant"
  }
}
```

1. Organize your files:
   - `mkdir -p ./models/qwen/Qwen3-0.6B`
   - Place ONNX files and `inference_model.json` here
2. Verify with `foundry cache ls`
3. Launch your model:
   - `foundry model run Qwen3-0.6b-cpu`

## Troubleshooting

- **Model not found:** Ensure files are placed in the correct directory
- **Weird responses:** Double-check chat template formatting
- **Slow performance:** Close unused programs, try a smaller model or enable GPU use
- **Command failures:** Confirm Python ≥ 3.8 and internet connectivity

## Summary

You now know how to:

- Convert AI models from PyTorch to ONNX
- Use Microsoft Olive for conversion
- Set up models in Foundry Local
- Troubleshoot common installation and runtime issues

Further support is available via [AI Discord](https://aka.ms/ai/discord) or the [Foundry Local GitHub repo](https://github.com/microsoft/foundry-Local).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/educator-developer-blog/how-to-use-custom-models-with-foundry-local-a-beginner-s-guide/ba-p/4428857)
