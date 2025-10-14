---
layout: "post"
title: "The Developer’s Guide to Smarter Fine-tuning with Azure AI Foundry"
description: "This guide introduces developers to custom model fine-tuning using Azure AI Foundry. It explains core concepts, shares best practices, highlights new developer-friendly features, and provides actionable resources—from code samples to hands-on videos—empowering you to build and deploy specialized AI agents tailored to real-world business scenarios."
author: "Malena Lopez-Sotelo, Jacques Guibert de Bruet"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/the-developers-guide-to-smarter-fine-tuning/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2025-10-14 19:01:20 +00:00
permalink: "/2025-10-14-The-Developers-Guide-to-Smarter-Fine-tuning-with-Azure-AI-Foundry.html"
categories: ["AI", "Azure"]
tags: ["Agentic AI", "AI", "Azure", "Azure AI Foundry", "Azure OpenAI", "Code Samples", "Developer Tier", "Direct Preference Optimization", "Distillation", "Fine Tuning", "GPT 4o", "LLM", "MicrosoftLearn", "Model Customization", "News", "Python Grader", "Region Deployment", "Reinforcement Fine Tuning", "REST API", "RFT", "Signal Loop", "SkilledByMTT"]
tags_normalized: ["agentic ai", "ai", "azure", "azure ai foundry", "azure openai", "code samples", "developer tier", "direct preference optimization", "distillation", "fine tuning", "gpt 4o", "llm", "microsoftlearn", "model customization", "news", "python grader", "region deployment", "reinforcement fine tuning", "rest api", "rft", "signal loop", "skilledbymtt"]
---

Malena Lopez-Sotelo and Jacques Guibert de Bruet provide developers with a practical guide to fine-tuning large language models in Azure AI Foundry, sharing proven strategies, platform updates, and code samples for launch-ready, business-focused AI.<!--excerpt_end-->

# The Developer’s Guide to Smarter Fine-tuning with Azure AI Foundry

**Authors:** Malena Lopez-Sotelo, Jacques Guibert de Bruet

Azure AI Foundry is designed to make fine-tuning large language models (LLMs) smarter, faster, and more accessible to developers. Whether you’re building reasoning agents, adaptive tools, or scalable workflows, Azure AI Foundry offers a comprehensive toolkit for customizing AI models to solve business-specific challenges.

## What is Fine-tuning?

Fine-tuning takes a pre-trained LLM and customizes it for your task by training it further on your own datasets. This means you can get models tailored to your domain, business language, and nuanced use cases—without building one from scratch. Fine-tuning increases performance, accuracy, and relevance for specific tasks like:

- Legal document analysis
- Domain-specific chatbots and copilots
- Custom recommendation engines

## Why Fine-tune?

- **Precision:** Achieve domain-specific accuracy and reduce generic, unhelpful outputs.
- **Cost and Efficiency:** Lower inference costs by optimizing models for your use-case.
- **Adaptability:** Empower AI to understand unique terminology, tone, or workflows.

### Real-World Use Case: Wealth Advisory Demo

A fine-tuned model can give detailed, actionable advice—for example, recommending travel gear after considering region-specific weather, brand data, and user intent, rather than generic suggestions. This demonstrates how prompt engineering and retrieval-augmented generation (RAG) combine with fine-tuning for context-aware answers.

## Best Practices for Fine-tuning

- **Set Clear Objectives:** Define target tasks, outcomes, and model behaviors up front.
- **Prioritize Data Quality:** Use well-labeled, high-quality training datasets for best results.
- **Iterate Continuously:** Evaluate model outputs, refine data, and retrain as needed.
- **Integrate into Your Workflow:** Treat fine-tuning as a continuous, feedback-driven process.
- **Utilize Azure’s Ecosystem:** Leverage integrated tools, REST APIs, monitoring, and Azure’s training infrastructure.

## What’s New in Azure AI Foundry for Developers

- **Reinforcement Fine-Tuning (RFT):** Now generally available, with full API/Swagger support, letting you optimize models based on reward feedback, ideal for agentic workflows.
- **Expanded Regional Support:** Fine-tune and deploy models globally across 26+ Azure regions, including support for GPT-4o and GPT-4o-mini.
- **Developer Tier:** Cost-effective option for rapid prototyping, including free hosting for 24 hours and pay-per-token testing.
- **Advanced Evaluation Tools:** Auto-Evals, Quick Evals, and Python Grader streamline model assessment and debugging.
- **Granular Model Control:** Easily move models across regions, pause and resume training jobs, and evaluate multiple candidates in parallel.

## Steps to Get Started

1. **Data Preparation:** Collect and curate your domain data.
2. **Model Selection:** Pick the best foundation model (e.g., GPT-4o, o4-mini).
3. **Training:** Use Direct Preference Optimization (DPO), RFT, or distillation approaches to refine your model.
4. **Deployment:** Deploy fine-tuned models with Azure’s scaling and monitoring features.
5. **Iterate:** Keep refining based on real-world performance and user feedback for optimal results.

## Technical Resources & Sample Code

The Azure AI Foundry developer community offers comprehensive, hands-on material:

| Name | Description | Links |
|---|---|---|
| O4-mini RFT Code Sample | Example of fine-tuning a reasoning model (o4-mini) using RFT | [GitHub Repo](https://github.com/azure-ai-foundry/fine-tuning/blob/main/Demos/RFT_Countdown/demo_with_python_grader.ipynb) |
| RAFT Fine-tuning Code Sample | Fine-tune Meta Llama 3.1 405B or GPT-4o on Azure AI | [GitHub Repo](https://github.com/Azure-Samples/azureai-foundry-finetuning-raft) |
| GPT-oss-20B Fine-tuning | Managed Compute fine-tuning example | [GitHub Repo](https://github.com/Azure/azureml-examples/blob/4df413cccaef14bd6f6c7efc6f41fdad0cf0533d/sdk/python/jobs/finetuning/standalone/model-as-a-platform/chat-completion/gpt-oss-20b/gpt-oss-20b-chat-completion.ipynb) |
| Distill DeepSeek V3 into Phi-4-mini | Knowledge distillation on Azure AI Foundry | [GitHub Repo](https://github.com/microsoft/Build25-LAB329) |
| AI Tour 2025: Efficient Model Customization | Distillation, RFT, and RAFT in production | [GitHub Repo](https://github.com/microsoft/Build25-LAB329) · [Slides](https://speakerdeck.com/nitya/aitour-26-efficient-model-customization-with-azure-ai-foundry) |
| Models for Beginners Course | Introductory curriculum (preview) | [GitHub Preview](https://aka.ms/models-for-beginners) |

### Videos

- **Azure AI Show: There’s no reason not to fine-tune** ([YouTube](https://aka.ms/AIShow/FineTuning))
- **Model Mondays: Fine-tuning & Distillation** ([YouTube](https://youtu.be/VSNGzBB20aw))

### Documentation

- [Fine-tune models with Azure AI Foundry](https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/fine-tuning-overview)
- [Reinforcement Fine-Tuning (RFT) Overview](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/reinforcement-fine-tuning)
- [Developer Tier Details](https://azure.microsoft.com/en-us/products/ai-foundry/)
- [Evaluations Enhancements & Auto-Evals](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/evaluate-generative-ai-app)

### Community

- Join discussions on [Discord](https://aka.ms/model-mondays/discord)

---

**About the Authors**

- *Jacques “Jack” Guibert de Bruet*: Microsoft Technical Trainer, specializing in intelligent automation and agentic AI solutions.
- *Malena Lopez-Sotelo*: Product marketing lead for fine-tuning at Microsoft, focused on activating hands-on developer resources.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/the-developers-guide-to-smarter-fine-tuning/)
