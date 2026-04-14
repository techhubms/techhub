---
section_names:
- ai
date: 2026-04-14 17:18:25 +00:00
author: stclarke
feed_name: Microsoft News
tags:
- 1024x1024
- AI
- Batch Pipelines
- Company News
- Cost Optimization
- GPU Efficiency
- Image Generation
- Latency
- MAI Image 2
- MAI Image 2 Efficient
- MAI Playground
- Microsoft Foundry
- Model Card
- News
- NVIDIA H100
- Production Workflows
- Prompt Fidelity
- Real Time Rendering
- Text To Image
- Throughput
- Token Pricing
title: 'MAI-Image-2-Efficient in Microsoft Foundry: flagship-quality text-to-image with lower cost and faster latency'
primary_section: ai
external_url: https://microsoft.ai/news/mai-image-2-efficient/
---

stclarke shares a Microsoft AI announcement introducing MAI-Image-2-Efficient, a production-oriented text-to-image model available in Microsoft Foundry and MAI Playground, positioned as faster and cheaper than MAI-Image-2 while maintaining “flagship” quality.<!--excerpt_end-->

## Summary

Microsoft’s MAI Superintelligence Team announces **MAI-Image-2-Efficient**, a new **text-to-image** model positioned as a faster and lower-cost option compared to **MAI-Image-2**, while keeping production-ready quality.

- Available now in **Microsoft Foundry** and **MAI Playground**
  - Microsoft Foundry: https://aka.ms/mai-image-2e-foundryblog
  - MAI Playground: https://playground.microsoft.ai/chat
- Pricing (as stated): **$5 per 1M text input tokens**, **$19.50 per 1M image output tokens**
- Performance claims (as stated):
  - **22% faster and 4x more efficient** (compared to MAI-Image-2 under their test normalization)
  - **~41% lower cost**
  - **40% faster on average** than other leading text-to-image models

## What’s being announced

### MAI-Image-2-Efficient

A “production workhorse” model intended for:

- High-volume generation
- Speed and tight cost control
- Real-time/interactive workflows

Example use cases mentioned:

- Product shots
- Marketing creatives
- UI mockups
- Branded assets
- Batch pipelines

The post also claims it handles **short-form in-image text** (headlines/labels) cleanly.

### MAI-Image-2

Positioned as the “precision tool” for:

- Highest fidelity needs
- Portraits and photorealistic scenes
- Stylized looks (anime/illustration)
- Longer/more complex in-image text

## Availability and rollout notes

- **No waitlist, no preview** (per the post); “plug it in and go” via Foundry/Playground.
- The model is also described as **rolling out across Copilot and Bing**, with “more surfaces like PowerPoint coming soon.”

## External feedback quoted

Shutterstock (Vanessa Salvo, Principal Product Manager) is quoted highlighting:

- Progress in **prompt fidelity** and creative usability
- The importance of **consistent, production-ready outputs** when moving from experimentation to real-world use

## References and benchmarking details (as stated)

- Tests dated **April 13, 2026**.
- For the MAI-Image-2 vs MAI-Image-2e comparison:
  - Normalized by latency and GPU usage
  - Throughput per GPU measured on **NVIDIA H100** at **1024×1024**
  - Results vary based on batch size, concurrency, and latency constraints
- Competitor latency comparison mentions:
  - Gemini 3.1 Flash (high reasoning), Gemini 3.1 Flash Image, Gemini 3 Pro Image measured via AI Studio API
  - MAI-Image-2, MAI-Image-2e, GPT-Image-1.5-High measured via Foundry API

## Further reading

- Model card (PDF): https://microsoft.ai/pdf/MAI-Image-2e-Model-Card.pdf

[Read the entire article](https://microsoft.ai/news/mai-image-2-efficient/)

