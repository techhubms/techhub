---
section_names:
- ai
- azure
- dotnet
primary_section: ai
title: Foundry Local is now Generally Available
feed_name: Microsoft AI Foundry Blog
external_url: https://devblogs.microsoft.com/foundry/foundry-local-ga/
author: samkemp
date: 2026-04-09 19:00:00 +00:00
tags:
- .NET
- AI
- Audio Transcription
- Azure
- Azure Local
- C#
- Chat Completions
- Edge AI
- Foundry
- Foundry Local
- GPU
- Hardware Acceleration
- JavaScript SDK
- Local AI
- Metal
- Microsoft Foundry
- Model Catalog
- News
- NPU
- Offline AI
- On Device
- On Device Inference
- ONNX Runtime
- OpenAI API Compatibility
- Python SDK
- Rust SDK
- Tool Calling
- Windows ML
- WinML
---

samkemp announces the GA of Foundry Local, a cross-platform on-device AI runtime and SDK that bundles ONNX Runtime and lets developers ship chat and audio inference inside apps with offline operation, hardware acceleration, and OpenAI-format APIs.<!--excerpt_end-->

## Foundry Local is now Generally Available

Today Microsoft announced the **General Availability (GA) of Foundry Local**, a cross-platform local AI solution for shipping **on-device inference** (including **chat** and **audio**) inside applications with **no cloud dependency**, **no network latency**, and **no per-token costs**.

Example scenarios mentioned include desktop assistants, healthcare decision support, private coding companions, and offline-capable edge apps.

## What is Foundry Local?

Microsoft positions **Microsoft Foundry** as spanning cloud to edge:

- **Microsoft Foundry (cloud):** frontier models, agents, and fine-tuning
- **Foundry Local for on-premises/distributed deployments:** validated on **Azure Local**
- **Foundry Local on devices:** runs natively across **Windows, macOS, Android**, and other devices (phones, laptops, desktops)

Foundry Local is intended to be small enough to bundle into an app installer without meaningfully increasing download size, and it aims to keep CI/CD artifacts lean by having **zero external dependencies**.

## How Foundry Local works

![Foundry Local architecture diagram](https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2026/04/foundry-local-architecture.webp)

At a high level:

1. You install a Foundry Local SDK in your app (thin language wrapper).
2. During build, **Foundry Local Core** and **ONNX Runtime** binaries are automatically downloaded and bundled as dependencies.
3. On first run, Foundry Local uses **Foundry Catalog** to download a model optimized for the user’s hardware.
4. On subsequent runs, the model loads from a **local device cache**.

### SDK installation

```bash
npm install foundry-local-sdk          # JavaScript
pip install foundry-local-sdk          # Python
dotnet add package Microsoft.AI.Foundry.Local  # C#
cargo add foundry-local-sdk            # Rust
```

### Runtime and platform integration details

- **Foundry Local Core** is the native runtime that manages model lifecycle:
  - download
  - load into device memory
  - inference management
  - unload
- **Windows:** integrates with **Windows ML (WinML)** to run inference and acquire hardware-matched execution provider plugins via the OS/Windows Update (aiming to simplify driver compatibility/version negotiation).
- **macOS:** runs on Apple Silicon GPU via **Metal**.
- **Cross-platform support:** Windows, Linux, macOS.

### API compatibility and serving options

- Inference APIs support:
  - **OpenAI request/response format** for chat completions and audio transcription
  - **Open Responses API** format (https://www.openresponses.org/)
- Goal: allow switching between cloud and on-device inference without spinning up a local HTTP server.
- Optional: configure an **OpenAI-compatible HTTP endpoint** (local webserver) on initialization if a REST endpoint is required.

## Foundry Local capabilities

- Ship AI features with **zero user setup** (self-contained; no CLI or third-party installs).
- Unified SDK for **speech-to-text**, **tool calling**, and **chat**.
- **Automatic hardware acceleration** (GPU/NPU, CPU fallback) without hardware detection code.
- **Token-by-token streaming** for real-time UX.
- **Offline** execution (user data stays on device; no network latency).
- Multi-language SDKs: **C#**, **Python**, **JavaScript**, **Rust**.
- **Resumable model downloads**.
- Curated optimized local models via Foundry Model Catalog, including:
  - GPT OSS
  - Qwen family
  - Whisper
  - Deepseek
  - Mistral
  - Phi
- Cross-platform support: **Windows**, **macOS (Apple Silicon)**, **Linux x64**.
- Optional OpenAI-compatible HTTP endpoint.

## What’s next

Planned/preview items called out:

- **Foundry Local powered by Azure Local** (preview): bringing models and agentic AI (including RAG and chat) to customer-owned distributed infrastructure.
- Expanded model catalog, including community contributions.
- Real-time microphone audio transcription for live captioning scenarios.
- Broader NPU/GPU support.
- Enhanced shared cache so models can be shared between applications.

## Get started

Install the Python SDK (other languages are available):

```bash
# Windows (recommended for hardware acceleration)
pip install foundry-local-sdk-winml

# macOS/Linux
pip install foundry-local-sdk
```

Then run:

```python
from foundry_local_sdk import Configuration, FoundryLocalManager

# Initialize Foundry Local
config = Configuration(app_name="foundry_local_samples")
FoundryLocalManager.initialize(config)
manager = FoundryLocalManager.instance

# Download and load a model from the catalog
model = manager.catalog.get_model("qwen2.5-0.5b")
model.download()
model.load()

# Get a chat client
client = model.get_chat_client()

# Create and send message in OpenAI format
messages = [{"role": "user", "content": "What is the golden ratio?"}]
response = client.complete_chat(messages)

# Response in OpenAI format
print(f"Response: {response.choices[0].message.content}")

# Unload the model from memory
model.unload()
```

## Learn more

- Foundry Local GitHub repo (samples for each language): https://github.com/microsoft/Foundry-Local
- Foundry Local docs: https://learn.microsoft.com/en-us/azure/foundry-local/


[Read the entire article](https://devblogs.microsoft.com/foundry/foundry-local-ga/)

