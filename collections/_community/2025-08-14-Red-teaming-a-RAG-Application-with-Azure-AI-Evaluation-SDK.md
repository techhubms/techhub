---
layout: "post"
title: "Red-teaming a RAG Application with Azure AI Evaluation SDK"
description: "This article by Pamela Fox explores how developers can use Microsoft's Azure AI Evaluation SDK to automate red-teaming for Retrieval-Augmented Generation (RAG) apps. It covers the risks of deploying LLM-powered applications, the process of simulating adversarial attacks with Microsoft's tooling, analyzing results across multiple models, and strategies for safeguarding AI-driven apps in production environments."
author: "Pamela_Fox"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/red-teaming-a-rag-app-with-the-azure-ai-evaluation-sdk/ba-p/4442682"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-14 07:00:00 +00:00
permalink: "/2025-08-14-Red-teaming-a-RAG-Application-with-Azure-AI-Evaluation-SDK.html"
categories: ["AI", "Azure", "Security"]
tags: ["Adversarial Testing", "AI", "AI Content Safety API", "Automated Red Teaming", "Azure", "Azure AI Evaluation SDK", "Azure AI Foundry", "Azure OpenAI Service", "Community", "Content Safety", "GPT 4o", "Hermes3", "LLM Security", "Microsoft", "Model Evaluation", "Model Hosting", "Ollama", "Prompt Engineering", "Pyrit", "Python", "RAG App", "Red Teaming", "Retrieval Augmented Generation", "RLHF", "Security"]
tags_normalized: ["adversarial testing", "ai", "ai content safety api", "automated red teaming", "azure", "azure ai evaluation sdk", "azure ai foundry", "azure openai service", "community", "content safety", "gpt 4o", "hermes3", "llm security", "microsoft", "model evaluation", "model hosting", "ollama", "prompt engineering", "pyrit", "python", "rag app", "red teaming", "retrieval augmented generation", "rlhf", "security"]
---

Pamela Fox investigates how the Azure AI Evaluation SDK enables automated red-teaming for LLM-powered RAG apps, examining real attack scenarios and showing practical ways to evaluate and strengthen AI security.<!--excerpt_end-->

# Red-teaming a RAG Application with Azure AI Evaluation SDK

**By Pamela Fox**

Deploying user-facing applications powered by large language models (LLMs) introduces safety risks, such as inadvertently producing harmful outputs or responding to adversarial prompts. This article reviews how to automate the red-teaming (adversarial testing) process for Retrieval-Augmented Generation (RAG) apps using Microsoft's Azure AI Evaluation SDK.

## Why Red-teaming?

Manual red-teaming brings in experts to probe apps with malicious queries, but it's expensive and impractical to repeat for every model, prompt, or app iteration. Automated tools can help spot vulnerabilities earlier and more efficiently.

## Azure AI Evaluation SDK and Automated Red Teaming

Microsoft's [automated Red Teaming agent](https://learn.microsoft.com/azure/ai-foundry/how-to/develop/run-scans-ai-red-teaming-agent) (within the [azure-ai-evaluations Python package](https://pypi.org/project/azure-ai-evaluation/)) spins up adversarial LLM agents inside Azure AI Foundry projects. These agents generate a battery of unsafe questions, applying transformations (base-64 encoding, Caesar cipher, URL encoding, etc.) via [pyrit](https://pypi.org/project/pyrit/) to craft complex attacks. The responses from your apps are then systematically evaluated for unsafe outputs.

## RAG-on-PostgreSQL: A Practical Experiment

The author's [sample RAG application](https://github.com/Azure-Samples/rag-postgres-openai-python/) uses user queries to fetch product info from a fictional retail database, inserting search results into an LLM prompt. The default model is Azure OpenAI's gpt-4o-mini, but the setup is flexible: models from Azure, GitHub, or Ollama can be swapped in.

### Screenshot

![RAG app UI](https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEi-xXXq7QXYg4uxK5S7Mc44_rOl6RJ1yQIrIz7j2L5Z3BUV38Rb_PRXTjNPyhEzF3DF7eVGDk2ZucRpQMjl7ukNn_iTLbrw5M0YG1jc1c3FHZUbSl-0oF3vBRvjhen-QLGHDvJtcrIeVGjyksWueOQGvXSQk8MzK9CSbchR86I9jeHtq_TiMIsi1rKR2A/s1600/Screenshot%202025-08-04%20at%209.48.43%E2%80%AFAM.png)

## Testing Results: Azure vs Open Models

Red-teaming the RAG app highlighted distinct differences in model robustness:

| Model      | Host           | Attack Success Rate |
|------------|----------------|--------------------|
| gpt-4o-mini| Azure OpenAI   | 0%                 |
| llama3.1:8b| Ollama         | 2%                 |
| hermes3:3b | Ollama         | 12.5%              |

**gpt-4o-mini (Azure OpenAI):**

- Benefits from Azure OpenAI's [content safety filters](https://learn.microsoft.com/azure/ai-foundry/openai/concepts/content-filter).
- Further protected by rigorous RLHF (reinforcement learning from human feedback) processes.

**llama3.1:8b (Ollama) and hermes3:3b:**

- Despite open model status, llama also scored well due to Meta's documented RLHF.
- Hermes was selected for its more 'neutrally-aligned' approach and proved most vulnerable, especially to 'self-harm' category attacks.

## Attack Strategies and Categories

Red-teaming attacks were broken down by type:

- **Attack category:** hate/unfairness, self-harm, sexual, violence
- **Complexity:** easy (string transforms), moderate (hypothetical/tense rephrasing by LLM), difficult (compositions)

#### Example Attacks

- Tense strategy prompts LLMs to answer as though explaining in a different timeframe or scenario (e.g., building an explosive device in a hypothetical society).
- Combined strategies such as tense + URL encoding were also tested.

## Lessons Learned and Next Steps

- Azure OpenAI's content safety and RLHF provide strong practical defense.
- More open models demand additional manual guardrails (e.g., [Azure AI Content Safety API](https://learn.microsoft.com/azure/ai-services/content-safety/overview)), and repeated automated testing.
- Prompt engineering alone is insufficient to block sophisticated or compound attacks.
- Before production, run comprehensive automated red teaming using diverse and complex strategies.

## References & Resources

- [Azure AI Foundry Documentation](https://learn.microsoft.com/azure/ai-foundry)
- [azure-ai-evaluations PyPI](https://pypi.org/project/azure-ai-evaluation/)
- [Pyrit Project](https://pypi.org/project/pyrit/)
- [Sample RAG app on GitHub](https://github.com/Azure-Samples/rag-postgres-openai-python/)

---

Pamela Fox, [Microsoft](https://techcommunity.microsoft.com/t5/s/gxcuf89792/images/dS0xNjA0MDc4LTQxODI4MWk5MjkyQjFBMEVGOUE5NkM5?image-dimensions=50x50)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/red-teaming-a-rag-app-with-the-azure-ai-evaluation-sdk/ba-p/4442682)
