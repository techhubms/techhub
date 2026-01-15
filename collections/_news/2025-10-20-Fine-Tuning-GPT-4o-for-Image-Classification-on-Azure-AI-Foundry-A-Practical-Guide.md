---
layout: post
title: 'Fine-Tuning GPT-4o for Image Classification on Azure AI Foundry: A Practical Guide'
author: Alexandre Levret
canonical_url: https://devblogs.microsoft.com/foundry/a-developers-guide-to-fine-tuning-gpt-4o-for-image-classification-on-azure-ai-foundry/
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
feed_url: https://devblogs.microsoft.com/foundry/feed/
date: 2025-10-20 07:11:50 +00:00
permalink: /ai/news/Fine-Tuning-GPT-4o-for-Image-Classification-on-Azure-AI-Foundry-A-Practical-Guide
tags:
- AI
- Azure
- Azure AI Foundry
- Azure Machine Learning
- Azure OpenAI
- Batch API
- CNN
- Cost Analysis
- Fine Tuning
- GPT 4o
- Image Classification
- Inference
- Latency
- ML
- Model Deployment
- Model Evaluation
- News
- Stanford Dogs Dataset
- Supervised Fine Tuning
- Vision Language Models
section_names:
- ai
- azure
- ml
---
Alexandre Levret demonstrates how developers can fine-tune GPT-4o using Azure AI Foundry to enhance image classification accuracy, providing a hands-on comparison against conventional CNNs and exploring practical trade-offs in performance, cost, and development workflow.<!--excerpt_end-->

# Fine-Tuning GPT-4o for Image Classification on Azure AI Foundry

**Author: Alexandre Levret**

This guide shows how to apply the latest Vision-Language Models (VLM) on Azure for image classification, even if you don’t have deep learning expertise. By fine-tuning GPT-4o on Azure OpenAI, you can boost accuracy for tasks like dog breed identification, leveraging modern cloud-based AI.

## Overview

- **Dataset**: Stanford Dogs (120 breeds, thousands of images)
- **Goal**: Compare out-of-the-box GPT-4o (zero-shot), fine-tuned GPT-4o, and a traditional lightweight CNN
- **Workflow**:
  1. Data preparation
  2. Batch inference with Azure OpenAI
  3. Model fine-tuning with Vision Fine-Tuning API
  4. Evaluation of results
  5. Cost, latency, and accuracy analysis

All code, scripts, and templates are available in the [GitHub repository](https://github.com/azure-ai-foundry/fine-tuning/tree/main/Demos/Image_Breed_Classification_FT).

---

## 1. What is Image Classification?

Image classification is a core computer vision task—grouping images into categories, like identifying dog breeds. Traditionally, this used Convolutional Neural Networks (CNNs), but since the advent of Large Language Models (LLMs) with vision capabilities, Vision-Language Models can now deliver powerful classification and reasoning with text and images.

Anyone can access these models via apps or APIs. For example, upload an image and ask, “What is the dog’s breed in the picture?” The model understands both the image and the prompt.

---

## 2. Deploying a Vision-Language Model on Azure

With [Azure AI Foundry](https://ai.azure.com/), you have access to many models, including the latest GPT-4o that supports both batch inference and vision fine-tuning. This enables:

- **Batch API**: Efficient, lower-cost large-scale inference
- **Vision Fine-Tuning API**: Task-specific adaptation of base models

### Batch Inference Example

You format requests in JSONL:

```json
{"model": "gpt-4o-batch", "messages": [ {"role": "system", "content":"Classify the following input image into one of the following categories: [Affenpinscher, Afghan Hound, ... , Yorkshire Terrier]." }, {"role": "user", "content": [ {"type": "image_url", "image_url": {"url": "b64", "detail": "low"}} ]} ]}
```

- Specify model version and task
- Send base64-encoded images
- Omit the correct label to simulate inference

**Batch API** saves 50% compared to synchronous inference, with longer latency (up to 24 hours, not guaranteed).

---

## 3. Fine-Tuning with Vision Fine-Tuning API

Fine-tuning adapts the pre-trained model for your specific task. Azure OpenAI supports various methods, including Supervised Fine-Tuning (SFT).

- You provide new labeled data in JSONL format (image-text pairs)
- Configure hyperparameters (batch size, learning rate, epochs, etc.)
- Example training entry:

```json
{"messages": [ {"role": "system", "content": "Classify the following input image into one of the following categories: [Affenpinscher, Afghan Hound, ... , Yorkshire Terrier]."}, {"role": "user", "content": [{"type": "image_url", "image_url": {"url": "data:image/jpeg;base64,<encoded_image>", "detail": "low"}}]}, {"role": "assistant", "content": "Springer Spaniel"} ]}
```

- Selected hyperparameters used in the fine-tuning run:
  - **Batch size**: 6
  - **Learning rate**: 0.5
  - **Epochs**: 2
  - **Seed**: 42
- More details at [Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/fine-tuning?context=%2Fazure%2Fai-foundry%2Fcontext%2Fcontext&tabs=azure-openai&pivots=programming-language-studio#configure-training-parameters-optional)

The process returns log files and visualizations of training progress (loss per step, etc.) in the Azure AI Foundry portal.

---

## 4. Baseline: Training a Classic CNN

Alongside the VLM approach, the guide also implements a lightweight CNN, trained on the same subset. The CNN achieves a mean accuracy of 61.67%—a useful reference, though it’s not state-of-the-art and requires more setup and maintenance.

- **Mean accuracy**: 61.67%
- **Training time**: Under 30 minutes
- **Deployment**: Local or via Azure Machine Learning

---

## 5. Results: Accuracy, Latency, and Cost

| Aspect                              | Base GPT-4o (Zero-Shot) | Fine-Tuned GPT-4o        | CNN Baseline          |
|--------------------------------------|-------------------------|--------------------------|-----------------------|
| **Mean accuracy**                    | 73.67%                  | 82.67% (+9pp vs base)    | 61.67% (-12pp vs base)|
| **Mean latency**                     | 1665ms                  | 1506ms (-9.6%)           | ~tens of ms           |
| **Cost**                             | Inference only ($)     | Training+Hosting+Inf ($$)| Local or AML ($)      |

**Key observations:**

- Fine-tuned GPT-4o achieves highest accuracy and slightly lower latency relative to zero-shot
- Zero-shot is quickest to production, but less accurate
- CNN is cheapest in inference, but much less accurate and requires engineering work

---

## 6. Next Steps

- Explore other datasets and tasks (OCR, multi-modal prompts)
- Experiment with different parameters
- Integrate the fine-tuned model into your pipeline
- Inspect and modify the codebase at [the GitHub repository](https://github.com/azure-ai-foundry/fine-tuning/tree/main/Demos/Image_Breed_Classification_FT)

Take advantage of [Azure AI Foundry Models](https://azure.microsoft.com/en-us/products/ai-foundry/models/) for model selection and secure enterprise-grade deployments.

---

## References & Further Reading

- [Fine-tuning documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/fine-tuning-overview)
- [Ignite: AI fine-tuning in Azure AI Foundry](https://azure.microsoft.com/en-us/blog/announcing-new-fine-tuning-models-and-techniques-in-azure-ai-foundry/?msockid=198f34c7ab4366b01e4122f1aae467c1)
- [Discord discussion channel](https://aka.ms/model-mondays/discord)

---

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/a-developers-guide-to-fine-tuning-gpt-4o-for-image-classification-on-azure-ai-foundry/)
