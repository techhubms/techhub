---
layout: "post"
title: "Red-Teaming a RAG App with the Azure AI Evaluation SDK"
description: "This article by Pamela Fox explores automated red-teaming of Retrieval-Augmented Generation (RAG) applications using the Azure AI Evaluation SDK, focusing on techniques to assess and improve the safety of LLM-powered user applications. The author describes the risks posed by unsafe LLM outputs, the value of automated red-teaming agents provided by Microsoft, shares quantitative results from running red-teaming scans against multiple models (including Azure OpenAI gpt-4o-mini, Meta's Llama, and Hermes), and discusses practical safety measures for production-ready AI apps."
author: "Pamela_Fox"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/red-teaming-a-rag-app-with-the-azure-ai-evaluation-sdk/ba-p/4442682"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-14 07:00:00 +00:00
permalink: "/2025-08-14-Red-Teaming-a-RAG-App-with-the-Azure-AI-Evaluation-SDK.html"
categories: ["AI", "Azure", "Security"]
tags: ["AI", "Azure", "Azure AI Evaluation SDK", "Azure AI Evaluations", "Azure AI Foundry", "Azure OpenAI Service", "Community", "Content Filters", "Content Safety", "Gpt 4o Mini", "Guardrails", "Hermes", "Llama3", "LLM", "Ollama", "Prompt Engineering", "Pyrit", "Python", "RAG", "Red Teaming", "Retrieval Augmented Generation", "RLHF", "Safety Evaluation", "Security", "Security Assessment"]
tags_normalized: ["ai", "azure", "azure ai evaluation sdk", "azure ai evaluations", "azure ai foundry", "azure openai service", "community", "content filters", "content safety", "gpt 4o mini", "guardrails", "hermes", "llama3", "llm", "ollama", "prompt engineering", "pyrit", "python", "rag", "red teaming", "retrieval augmented generation", "rlhf", "safety evaluation", "security", "security assessment"]
---

Pamela Fox demonstrates how to use the Azure AI Evaluation SDK to automate red-teaming of a RAG application, analyzing risks when deploying LLMs and showing security outcomes across different AI models.<!--excerpt_end-->

# Red-Teaming a RAG App with the Azure AI Evaluation SDK

Author: Pamela Fox

## Introduction

Deploying user-facing applications powered by large language models (LLMs) carries the risk of producing unsafe outputs—such as content that encourages violence, hate speech, or self-harm. Manual testing is only a partial solution, since malicious users may craft highly creative inputs that bypass superficial filters.

## The Challenge of Red-Teaming LLM Applications

Red-teaming is the process of rigorously probing a system for vulnerabilities, often with experts designing malicious prompts to assess weaknesses. Traditional red-teaming is resource-intensive and not practical for every iteration of an LLM-powered app.

## The Automated Red Teaming Agent from Microsoft

Microsoft addresses this challenge with its [automated Red Teaming agent](https://learn.microsoft.com/azure/ai-foundry/how-to/develop/run-scans-ai-red-teaming-agent), delivered via the [`azure-ai-evaluations`](https://pypi.org/project/azure-ai-evaluation/) Python package. This tool:

- Uses an adversarial LLM, safely sandboxed within Azure AI Foundry
- Automatically generates unsafe query prompts across different risk categories
- Applies known transformation and obfuscation attacks (using the [pyrit package](https://pypi.org/project/pyrit/): base64, URL encoding, ciphers, etc.)
- Evaluates both original and transformed queries against your app
- Assesses if your app leaks answers to unsafe queries

## Testing a Retrieval-Augmented Generation (RAG) Application

Pamela tested this process on her [RAG-on-PostgreSQL sample application](https://github.com/Azure-Samples/rag-postgres-openai-python/), which uses RAG techniques to answer product queries from a sample outdoors store database. The app retrieves top product details based on user queries and sends them, along with a customer service prompt, to an LLM.

![Example App Screenshot](https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEi-xXXq7QXYg4uxK5S7Mc44_rOl6RJ1yQIrIz7j2L5Z3BUV38Rb_PRXTjNPyhEzF3DF7eVGDk2ZucRpQMjl7ukNn_iTLbrw5M0YG1jc1c3FHZUbSl-0oF3vBRvjhen-QLGHDvJtcrIeVGjyksWueOQGvXSQk8MzK9CSbchR86I9jeHtq_TiMIsi1rKR2A/s1600/Screenshot%202025-08-04%20at%209.48.43%E2%80%AFAM.png)

## Red-Teaming Results Across Models

Pamela ran the agent against multiple backend models: Azure OpenAI gpt-4o-mini, Meta's Llama3.1:8b via Ollama, and Hermes3:3b via Ollama. Results included:

| Model         | Host         | Attack Success Rate  |
|-------------- |------------- |---------------------|
| gpt-4o-mini   | Azure OpenAI | 0%   🥳             |
| llama3.1:8b   | Ollama       | 2%                  |
| hermes3:3b    | Ollama       | 12.5% 😭            |

- **gpt-4o-mini (Azure OpenAI)**: 0% attack success, attributed to robust [content safety filters](https://learn.microsoft.com/azure/ai-foundry/openai/concepts/content-filter) and reinforcement learning from human feedback (RLHF).
- **llama3.1:8b (Ollama)**: Low (2%) success, indicating effective RLHF even on local models.
- **hermes3:3b (Ollama)**: Higher (12.5%) success, with self-harm prompts being most successful (31.25% in that category), likely reflecting less training in filtering such content.

Analysis included breakdowns by attack category and complexity, and example attacks highlighted subtle failures—particularly when prompt context accidentally lent legitimacy to unsafe queries.

## Lessons and Mitigations

- **Azure AI Content Safety API**: For models with higher attack rates, layering Microsoft's safety APIs and rerunning the red-teaming process is recommended.
- **Prompt Engineering**: It helps but may not suffice against sophisticated attacks.
- **Comprehensive Testing**: A robust, multi-faceted red-teaming scan is vital before deploying to production, especially for models lacking integrated guardrails.

## Conclusion

Using tools like the Azure AI Evaluation SDK makes security evaluation scalable, repeatable, and more accessible to typical development teams. Pamela's hands-on results reveal concrete risks and best practices for deploying LLM-powered apps in production—underscoring the need for layered safety controls and ongoing automated testing.

---

References:

- [Azure AI Red Teaming Agent Documentation](https://learn.microsoft.com/azure/ai-foundry/how-to/develop/run-scans-ai-red-teaming-agent)
- [azure-ai-evaluations Python Package](https://pypi.org/project/azure-ai-evaluation/)
- [Azure AI Content Safety Overview](https://learn.microsoft.com/azure/ai-services/content-safety/overview)
- [RAG-on-PostgreSQL Sample Application](https://github.com/Azure-Samples/rag-postgres-openai-python/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/red-teaming-a-rag-app-with-the-azure-ai-evaluation-sdk/ba-p/4442682)
