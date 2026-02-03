---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/github-copilot-sdk-and-hybrid-ai-in-practice-automating-readme/ba-p/4489694
title: 'Hybrid AI with GitHub Copilot SDK: Automating README to PowerPoint Generation Using Microsoft Foundry Local'
author: kinfey
primary_section: github-copilot
feed_name: Microsoft Tech Community
date: 2026-01-28 09:10:46 +00:00
tags:
- Agent Development
- AI
- AI Orchestration
- Automation
- Claude Sonnet 4.5
- Coding
- Community
- Copilot CLI
- Copilot Skills
- Cost Optimization
- Edge AI
- GitHub Copilot
- GitHub Copilot SDK
- Hybrid AI
- Large Language Models
- LLM
- Microsoft Foundry Local
- Natural Language Engineering
- PowerPoint Generation
- Privacy
- Python
- Qwen 2.5 7B
- README Processing
- SLM
- Small Language Models
section_names:
- ai
- coding
- github-copilot
---
kinfey delivers a practical exploration of hybrid AI system design, highlighting how Microsoft Foundry Local and GitHub Copilot SDK automate the conversion of GitHub README files into professional PowerPoint presentations. This article demystifies local and cloud AI model orchestration for real developer scenarios.<!--excerpt_end-->

# Hybrid AI with GitHub Copilot SDK: Automating README to PowerPoint Generation Using Microsoft Foundry Local

## Introduction

In today’s AI landscape, developers can leverage both cloud-based Large Language Models (LLMs) and efficient, on-device Small Language Models (SLMs) to build powerful, privacy-respecting solutions. Hybrid models combine the best of both worlds—retaining security and reducing costs while delivering advanced capabilities. This article explores these architectures using the GenGitHubRepoPPT project, which automates professional PowerPoint creation from GitHub README files by combining Microsoft Foundry Local, GitHub Copilot SDK, and modern AI techniques.

## 1. Hybrid Model Scenarios and Value

### What Are Hybrid AI Models?

Hybrid AI models integrate locally running SLMs and cloud-based LLMs within a single application, assigning tasks based on privacy, complexity, and cost constraints. Key principles include:

- **Local Processing for Sensitive Data**: Handling private or regulated data on-device
- **Cloud-Powered Creativity**: Using LLMs for demanding generation and reasoning
- **Smart Cost/Performance Choices**: Running frequent simple tasks locally

### Typical Use Cases

- **Intelligent Document Processing**: Local extraction, cloud refinement
- **Code Development Assistants**: Local syntax checks, cloud for complex refactoring
- **Customer Service**: Local intent detection, cloud for complex queries
- **Content Creation Platforms**: Local outlines, cloud for advanced writing

### Why Choose Hybrid Models?

- **Privacy**: Data stays on-device for sensitive steps
- **Cost Optimization**: Reduces API usage expenses
- **Reliability & Speed**: Handles some tasks even offline, avoids latency

## 2. Core Technology Analysis

### Large Language Models (LLMs)

LLMs, like GPT-5.2, Claude Sonnet 4.5, and Gemini, excel at:

- Text understanding & generation
- Code synthesis and refactorings
- Translation and creative writing

### Small Language Models (SLMs) & Microsoft Foundry Local

SLMs (1B–7B parameters), such as the Microsoft Phi family and Qwen models, allow fast, low-cost, on-device inference. **Microsoft Foundry Local** provides:

- OpenAI-compatible APIs for local model serving
- Hardware acceleration (CPU, GPU, NPU, Apple Silicon)
- Cross-platform (Windows/macOS) with ONNX Runtime
- Easy management of models and inference processes

Example usage:

```python
from openai import OpenAI
from foundry_local import FoundryLocalManager
manager = FoundryLocalManager("qwen2.5-7b-instruct")
client = OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
```

### GitHub Copilot SDK: Rapid Agent & Automation Development

Released in preview (Jan 2026), the GitHub Copilot SDK enables direct development of production-grade AI agents. It offers:

- Out-of-the-box context management, tool orchestration, and model switching
- Skill system for reusable task logic
- Reliable multi-step planning and execution
- Seamless connection to filesystems, APIs, and business code

Example: Instead of hand-writing complex PowerPoint logic, developers just define the desired business outcome using Copilot Skills and let the SDK handle orchestration and code generation.

#### Copilot Skills Example

```markdown
# PowerPoint Generation Expert Skill

## Expertise

You are skilled at turning technical outlines into engaging PPTs.
- Analyze structure (titles, bullets)
- Select layouts, apply colors
- Adapt for multiple languages
- Generate .pptx files with uniform style
```

## 3. GenGitHubRepoPPT Case Study

### Overview

GenGitHubRepoPPT automatically generates PowerPoint presentations from GitHub repository READMEs in minutes by leveraging a hybrid model:

#### 1. Local SLM (Foundry Local + Qwen-2.5-7B)

- Analyzes the README for sensitive info
- Generates a structured outline locally for privacy
- Zero API cost for frequent processing

#### 2. Cloud LLM (Claude Sonnet 4.5) + Copilot SDK

- Converts outlines to high-quality PowerPoint using Copilot SDK’s agent core
- Automates hundreds of lines of logic (layout, style, error handling)
- Handles multilingual and professional formatting
- Dramatically reduces dev time from days to hours

### Technical Architecture Flow

1. Extract & structure content with SLM (on device)
2. Pass outline to Copilot SDK (cloud), declare intent and skills
3. Agent executes file generation, outputting .pptx

### Benefits Demonstrated

- **Privacy**: Data remains under user control
- **Cost Savings**: Expensive API calls only for high-value tasks
- **Developer Productivity**: 95% less code (skills vs. hand-written logic)
- **Scalable Automation**: New features via skill files, not infrastructure changes

## 4. Key Takeaways and Technology Trends

- **Intent-Driven Development**: Copilot SDK lets developers focus on *what* they want, not *how* to code it.
- **Democratization**: Reusable skills empower less-experienced devs and domain experts.
- **Edge + Cloud AI**: Combining local and remote models is the future for secure, flexible, and efficient AI apps.

## References

- [GenGitHubRepoPPT Repository](https://github.com/kinfey/GenGitHubRepoPPT)
- [Microsoft Foundry Local](https://github.com/microsoft/Foundry-Local)
- [GitHub Copilot SDK](https://github.com/github/copilot-sdk)
- [Copilot SDK Getting Started](https://github.com/github/copilot-sdk/blob/main/docs/getting-started.md)
- [Edge AI for Beginners](https://github.com/microsoft/edgeai-for-beginners)
- [Azure AI Foundry Documentation](https://learn.microsoft.com/azure/ai-foundry/)

---
*Article by kinfey*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/github-copilot-sdk-and-hybrid-ai-in-practice-automating-readme/ba-p/4489694)
