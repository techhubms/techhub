---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/on-device-ai-with-windows-ai-foundry/ba-p/4466236
title: 'On-Device AI with Windows AI Foundry: Local Inference for Fast, Private Apps'
author: Nandhini_Elango
feed_name: Microsoft Tech Community
date: 2025-11-04 08:00:00 +00:00
tags:
- AI Inference
- Application Workflow
- Azure AI Services
- Azure Cognitive Services
- Developer Toolkit
- DirectML
- Edge AI
- Foundry Local
- Hybrid AI
- Local Models
- NPU Acceleration
- Offline AI
- On Device AI
- ONNX Runtime
- Privacy
- Python
- Windows AI Foundry
section_names:
- ai
- coding
---
Nandhini_Elango explains how Windows AI Foundry enables developers to run AI models locally on Windows devices, enhancing privacy, speed, and offline capabilities. The article covers hybrid design patterns, integration guidance, and code approaches for practical implementation.<!--excerpt_end-->

# On-Device AI with Windows AI Foundry: Local Inference for Fast, Private Apps

## Introduction

On-device AI is transforming user experiences by enabling instant, private, and reliable intelligence—right where your users are. With Windows AI Foundry, developers can integrate and run AI models locally on Windows devices, using ONNX Runtime and hardware acceleration (CPU, GPU via DirectML, or NPU) without handling low-level details.

## Why On‑Device AI?

- **Speed:** Reduce perceived latency and deliver instant responses to users.
- **Privacy:** Keep user and app data on the device, avoiding unnecessary cloud transmission.
- **Reliability:** Enable AI-powered features even when offline or when network is poor.
- **Cost:** Cut costs for high-volume inference by minimizing cloud compute and data usage.

This is especially useful in regulated environments, field tools, and places where data sensitivity or low latency are priorities.

## Windows AI Foundry Components

### Windows AI Foundry

A Microsoft developer toolkit simplifying local AI inference.

- Uses **ONNX Runtime** as the backend engine.
- Supports CPU, GPU (DirectML), and NPU acceleration.
- Abstracts hardware management so developers can focus on app logic.

### Foundry Local

The local inference engine—integrate it for fast, private AI experiences directly in your Windows apps.

- Keeps both model and data on-device.
- Only accesses the cloud when explicitly configured by the developer (or user consent).

## Hybrid Model for Real-World Apps

You don’t have to choose just local or just cloud:

- Use **on-device inference** (Foundry Local) for fast, private, disconnected workloads (e.g., summarization, local search).
- **Cloud as extension**: Enhance with large models or up-to-date data only when necessary.

**Hybrid workflow:**

- Prefer Foundry Local for inference.
- Cloud assists with model refresh, telemetry, analytics, or heavy compute tasks.

## Sample Application Workflows and Code

1. **On-Device Only**: Attempts Foundry Local first, then falls back to local ONNX model.

```python
if foundry_runtime.check_foundry_available():
    # Use on-device Foundry Local models
    try:
        answer = foundry_runtime.run_inference(question, context)
        return answer, "Foundry Local (On-Device)"
    except Exception as e:
        logger.warning(f"Foundry failed: {e}, trying ONNX...")

if onnx_model.is_loaded():
    # Fallback to local ONNX model
    try:
        answer = bert_model.get_answer(question, context)
        return answer, "BERT ONNX (On-Device)"
    except Exception as e:
        logger.warning(f"ONNX failed: {e}")

return "Error: No local AI available"
```

1. **Hybrid On-Device + Cloud**: Prioritizes local, falls back to cloud API as last option.

```python
def get_answer(question, context):
    # Priority order:
    # 1. Foundry Local (best: advanced + private)
    # 2. ONNX Runtime (good: fast + private)
    # 3. Cloud API (fallback: requires internet, less private)

    if foundry_runtime.check_foundry_available():
        try:
            answer = foundry_runtime.run_inference(question, context)
            return answer, "Foundry Local (On-Device)"
        except Exception as e:
            logger.warning(f"Foundry failed: {e}, trying ONNX...")

    if onnx_model.is_loaded():
        try:
            answer = bert_model.get_answer(question, context)
            return answer, "BERT ONNX (On-Device)"
        except Exception as e:
            logger.warning(f"ONNX failed: {e}, trying cloud...")

    if network_available():
        try:
            import requests
            response = requests.post(
                '{BASE_URL_AI_CHAT_COMPLETION}',
                headers={'Authorization': f'Bearer {API_KEY}'},
                json={'model': '{MODEL_NAME}', 'messages': [{'role': 'user', 'content': f'Context: {context}\n\nQuestion: {question}'}]},
                timeout=10,
            )
            answer = response.json()['choices'][0]['message']['content']
            return answer, "Cloud API (Online)"
        except Exception as e:
            return "Error: No AI runtime available", "Failed"
    else:
        return "Error: No internet and no local AI available", "Offline"
```

## Demo Project

- **Foundry Local answering context-based questions offline:**
  - Ran the Phi-4-mini model offline to retrieve answers.
  - Indicated clearly when answers were not found in the local context.

## Practical Use Cases

- **Privacy-First Assistants:** Summarize documents or answer user questions locally.
- **Healthcare Apps:** Analyze sensitive medical data on-device for compliance.
- **Finance:** Local risk scoring; personal finance tools that never leave the device.
- **IoT & Edge:** Enable AI-powered monitoring, detection, classification offline.

## Conclusion

On-device AI using Windows AI Foundry lets developers build responsive, secure, and offline-ready apps—combining local inference with cloud capabilities for scalability. You get best-of-both-worlds intelligence, while protecting user data and delivering great UX.

## References

- [Get started with Foundry Local - Foundry Local | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-local/get-started)
- [What is Windows AI Foundry? | Microsoft Learn](https://learn.microsoft.com/en-us/windows/ai/overview)
- [Unlocking Instant On-device AI with Foundry Local (DevBlogs)](https://devblogs.microsoft.com/foundry/unlock-instant-on-device-ai-with-foundry-local/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/on-device-ai-with-windows-ai-foundry/ba-p/4466236)
