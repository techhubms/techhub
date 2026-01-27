---
external_url: https://www.microsoft.com/en-us/research/blog/mmctagent-enabling-multimodal-reasoning-over-large-video-and-image-collections/
title: 'MMCTAgent: Microsoft’s Multimodal Critical Thinking Agent for Image and Video Reasoning'
author: stclarke
feed_name: Microsoft News
date: 2025-11-12 16:23:01 +00:00
tags:
- AutoGen
- Azure AI Foundry Labs
- Azure AI Search
- Company News
- GPT 4V
- GPT4o
- Image Reasoning
- ImageAgent
- Knowledge Base
- Microsoft Research
- MMCTAgent
- Modular Agents
- Multimodal AI
- Object Detection
- OCR
- Open Source Tools
- Performance Evaluation
- Planner–Critic Architecture
- Semantic Retrieval
- Video Reasoning
- VideoAgent
- Visual Question Answering
section_names:
- ai
- azure
primary_section: ai
---
stclarke explores MMCTAgent, Microsoft Research’s agentic, multimodal AI framework for scalable reasoning over videos and images, describing its innovative architecture and Azure integration.<!--excerpt_end-->

# MMCTAgent: Microsoft’s Multimodal Critical Thinking Agent for Image and Video Reasoning

Modern multimodal AI models excel at short-clip analysis and object recognition, but real-world reasoning often requires handling long-form video and integrating massive libraries of images, videos, and transcripts. MMCTAgent (Multi-modal Critical Thinking Agent) is Microsoft’s open-source response to these challenges, providing structured reasoning workflows for complex visual data.

## MMCTAgent Overview

Built on AutoGen, MMCTAgent adopts a Planner–Critic architecture, separating planning and self-evaluation for dynamic, tool-enabled multimodal reasoning. It offers modality-specific agents—ImageAgent and VideoAgent—that utilize dedicated tools for image and video analysis (e.g., object detection, OCR, visual question answering).

Key features:

- **Agent-based design:** Separate agents for image and video processing
- **Extensible toolchain:** Easily integrate new domain- or modality-specific tools
- **Iterative reasoning:** Planner generates solutions, Critic reviews and refines them
- **Azure integration:** Deploy on Azure AI Foundry Labs, index visual and textual metadata with Azure AI Search

## How MMCTAgent Works

### Planner–Critic Workflow

- **Planner Agent:** Decomposes queries, chooses relevant reasoning tools, drafts answers
- **Critic Agent:** Reviews evidence, aligns facts, refines outputs for accuracy/coherence

Modality-specific agents:

- **ImageAgent:** Uses ViT/VLM models, scene recognition, object detection, OCR; performs image-centric question answering and inspection
- **VideoAgent:** Handles long-form video ingestion (transcription, key-frame extraction, semantic chunking, multimodal embedding). At query time, retrieves/reasons across indexed content using planner and critic tools for temporal/contextual analysis.

### Toolset Highlights

- get_video_analysis: Summarizes videos and detected objects
- get_context: Fetches chapters and context from Azure AI Search index
- get_relevant_frames, query_frame: Focused visual reasoning on key frames
- object_detection_tool, ocr_tool: Deep visual analysis for static images

## Evaluation Results

Experiments show MMCTAgent substantially enhances accuracy of base large models (GPT-4V, GPT4o, GPT-5) on benchmark image/video datasets (MM-Vet, MMMU, VideoMME). Tool integration (object detection, OCR, critic validation) led to 10–15% improvement over raw LLMs in multimodal VQA and video analysis tasks. Full evaluation details are available [on GitHub](https://github.com/microsoft/MMCTAgent/blob/main/EVALUATIONS.md).

## Extensibility and Azure-Native Deployment

Developers can add new tools and model integrations, making MMCTAgent suitable for specialized tasks such as medical imaging and industrial inspection. All metadata is indexed in the Multimodal Knowledgebase using Azure AI Search for scalable semantic retrieval.

MMCTAgent is featured on Azure AI Foundry Labs, which hosts experimental Microsoft Research technologies for advanced AI applications. Future directions include improving reasoning workflow efficiency and expanding to new real-world domains through projects like Project Gecko.

## References and Links

- [MMCTAgent Research Publication](https://www.microsoft.com/en-us/research/publication/mmctagent-multi-modal-critical-thinking-agent-framework-for-complex-visual-reasoning/?msockid=153992cb7df169482b9487167c0968e9)
- [Azure AI Foundry Labs](https://labs.ai.azure.com/projects/mmct-agent/)
- [AutoGen Project](https://www.microsoft.com/en-us/research/project/autogen)
- [MMCTAgent GitHub Repository](https://github.com/microsoft/MMCTAgent)
- [Deep Video Discovery](https://www.microsoft.com/en-us/research/publication/deep-video-discovery-agentic-search-with-tool-use-for-long-form-video-understanding/)
- [Azure AI Search](https://learn.microsoft.com/en-us/azure/search/search-what-is-azure-search)

## Acknowledgements

Team members: Aman Patkar, Ogbemi Ekwejunor-Etchie, Somnath Kumar, Soumya De, and Yash Gadhia.

## Key Takeaways

- Scalable, Azure-native multimodal agent architecture for reasoning over images and video
- Modular, tool-driven approach enables extensibility and domain adaptation
- Empirically validated improvements over base LLM performance
- Open-source and available for developer experimentation on Azure

For more in-depth benchmarking details and documentation, visit the [MMCTAgent GitHub page](https://github.com/microsoft/MMCTAgent).

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/research/blog/mmctagent-enabling-multimodal-reasoning-over-large-video-and-image-collections/)
