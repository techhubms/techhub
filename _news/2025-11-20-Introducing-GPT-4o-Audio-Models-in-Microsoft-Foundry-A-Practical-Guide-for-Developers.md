---
layout: "post"
title: "Introducing GPT-4o Audio Models in Microsoft Foundry: A Practical Guide for Developers"
description: "This comprehensive guide introduces developers to the new GPT-4o audio models—GPT-4o-Transcribe, GPT-4o-Mini-Transcribe, and GPT-4o-Mini-TTS—now available in Microsoft Azure OpenAI via Foundry. It details technical innovations, explains targeted audio pretraining and advanced distillation, and provides step-by-step instructions for integrating speech-to-text and text-to-speech functions in real-world applications using Azure. Developers will learn best practices for environment setup, authentication, configuration, and hands-on deployment with Gradio, enabling the creation of advanced, voice-driven applications."
author: "Allan Carranza"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/azure-openai-gpt4o-audio-models-developer-guide/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2025-11-20 16:00:21 +00:00
permalink: "/2025-11-20-Introducing-GPT-4o-Audio-Models-in-Microsoft-Foundry-A-Practical-Guide-for-Developers.html"
categories: ["AI", "Azure"]
tags: ["AI", "API Integration", "Audio Models", "Authentication", "Azure", "Azure AI", "Azure AI Foundry", "Azure CLI", "Azure OpenAI", "Azure OpenAI Service", "Developer Guide", "GPT 4o", "Gradio", "Microsoft Foundry", "News", "OpenAI", "Speech To Text", "Text To Speech", "Transcription", "TTS"]
tags_normalized: ["ai", "api integration", "audio models", "authentication", "azure", "azure ai", "azure ai foundry", "azure cli", "azure openai", "azure openai service", "developer guide", "gpt 4o", "gradio", "microsoft foundry", "news", "openai", "speech to text", "text to speech", "transcription", "tts"]
---

Allan Carranza presents a step-by-step developer guide to using the latest GPT-4o audio models on Azure OpenAI via Microsoft Foundry, with practical examples for speech-to-text and TTS integration.<!--excerpt_end-->

# Introducing GPT-4o Audio Models in Microsoft Foundry: A Practical Guide for Developers

Author: Allan Carranza

## Overview

This guide explores the latest additions to Azure OpenAI—GPT-4o-Transcribe, GPT-4o-Mini-Transcribe, and GPT-4o-Mini-TTS—offering cutting-edge audio capabilities for transcription and text-to-speech (TTS) use cases.

## What's New in OpenAI's Audio Models?

- **GPT-4o-Transcribe & GPT-4o-Mini-Transcribe:** Advanced speech-to-text models that surpass previous benchmarks for accuracy and speed.
- **GPT-4o-Mini-TTS:** A flexible text-to-speech model supporting custom speech instructions for interactive and accessible applications.

| Feature              | GPT-4o-Transcribe | GPT-4o-Mini-Transcribe | GPT-4o-Mini-TTS   |
|----------------------|-------------------|------------------------|-------------------|
| Performance          | Best Quality      | Great Quality          | Best Quality      |
| Speed                | Fast              | Fastest                | Fastest           |
| Input                | Text, Audio       | Text, Audio            | Text              |
| Output               | Text              | Text                   | Audio             |
| Streaming            | ✅                | ✅                     | ✅                |
| Ideal Use Cases      | Accurate transcription for complex audio (e.g., call centers, meetings) | Live captioning, rapid response, budget scenarios | Interactive voice outputs for bots, assistants, accessibility, edu apps |

## Technical Innovations

- **Targeted Audio Pretraining:** Utilizes large-scale, specialized datasets for improved speech understanding.
- **Advanced Distillation:** Preserves high model performance while reducing size for efficiency.
- **Reinforcement Learning:** Boosts transcription accuracy, minimizing misrecognition in complex environments.

## Getting Started Guide

### 1. Set Up Azure OpenAI Environment

- Obtain your Azure OpenAI endpoint and API key.
- Authenticate via Azure CLI:

```bash
az login
```

### 2. Configure Project Environment

- Create a `.env` file with your credentials:

```env
AZURE_OPENAI_ENDPOINT="your-endpoint-url"
AZURE_OPENAI_API_KEY="your-api-key"
AZURE_OPENAI_API_VERSION="2025-04-14"
```

### 3. Install Dependencies

- Configure your Python virtual environment and install essentials:

```bash
uv venv
source .venv/bin/activate        # macOS/Linux
.venv\Scripts\activate           # Windows
uv add azure-ai-openai python-dotenv gradio aiohttp
```

### 4. Deploy and Test Using Gradio

- Launch your Gradio app for audio streaming:

```bash
python your_gradio_app.py
```

## Developer Impact

By integrating GPT-4o audio models, developers can:

- Easily add transcription and TTS capabilities to apps
- Build responsive, accessible, and voice-driven user experiences
- Leverage customizable voice features for unique interfaces

## Additional Resources

- [GPT-4o Audio Models Overview (Nick.FM)](https://nick.fm)
- [Azure OpenAI Documentation](https://azure.microsoft.com/services/openai)
- [Azure AI Foundry Quickstart](https://github.com/azure-ai-foundry)

We encourage developers to explore these new models and share results and feedback to keep improving the platform.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/azure-openai-gpt4o-audio-models-developer-guide/)
