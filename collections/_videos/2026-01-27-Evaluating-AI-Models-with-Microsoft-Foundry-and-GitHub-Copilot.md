---
external_url: https://www.youtube.com/watch?v=o5o_pmMXJUs
title: Evaluating AI Models with Microsoft Foundry and GitHub Copilot
author: Microsoft Developer
primary_section: github-copilot
feed_name: Microsoft Developer YouTube
date: 2026-01-27 19:23:14 +00:00
tags:
- Agent Development
- AI
- AI Model Evaluation
- Azure
- Azure AI
- Claude Sonnet 4.5
- Cloud Computing
- Dataset Versioning
- Dev
- Developer Workflow
- Development
- Evaluations SDK
- F1 Score
- GitHub Copilot
- GPT 5
- Grok 4
- Jupyter Notebooks
- METEOR
- Microsoft
- Microsoft Foundry
- ML
- Prompt Engineering
- Python SDK
- PyTorch
- Tech
- Technology
- Videos
section_names:
- ai
- azure
- github-copilot
- ml
---
Microsoft MVP Veronika Kolesnikova, joined by Justin Garrett, provides a hands-on walkthrough of evaluating and comparing AI models using Microsoft Foundry, with practical tips for developers. Generated datasets and workflows with GitHub Copilot are also showcased.<!--excerpt_end-->

{% youtube o5o_pmMXJUs %}

# Evaluating AI Models with Microsoft Foundry and GitHub Copilot

**Presenter:** Veronika Kolesnikova (Microsoft MVP, Principal AI Engineer)
**Host:** Justin Garrett (Principal PM, Developer Relations, Microsoft)

## Overview

This MVP Unplugged episode features a detailed exploration of how to systematically evaluate and select AI models for developer tasks using Microsoft Foundry. Topics include:

- Building custom evaluation datasets leveraging GitHub Copilot
- Comparing outputs from models like GPT-5, Claude Sonnet 4.5, and Grok-4
- Running evaluations with the Microsoft Foundry Python SDK
- Applying performance metrics (F1, METEOR, similarity scores)
- Storing and versioning evaluation datasets in Microsoft Foundry
- Debugging, iteration strategies, and practical developer tips

## Key Highlights

### 1. Creating Datasets with GitHub Copilot

- Use Copilot to generate sample queries and contexts around domains like hiking and software engineering
- Build real, domain-relevant datasets for robust model evaluation

### 2. Microsoft Foundry Evaluation Process

- **Set Up:** Connect to Azure AI projects through the Foundry portal or SDK
- **Structure:** Datasets include query, context, and ground truth for reliable evaluation
- **Evaluation:** Run tests programmatically using the new SDK or inside Jupyter Notebooks
- **Measure:** Use metrics such as F1, METEOR, and similarity thresholds to assess accuracy and reliability
- **Analyze Outputs:** Compare detailed results and behaviors across models (GPT-5, Claude, Grok-4)

### 3. Practical Model Selection Guidance

- See how to choose the best model for your use case based on systematic analysis
- Use Foundry results to iterate, refine, and debug AI workflows
- Strategies for storing, versioning datasets, and ensuring reproducibility

### 4. Additional Resources

- Free Microsoft Foundry Trial: [https://aka.ms/devrelft](https://aka.ms/devrelft)
- Observability and monitoring guidance: [Docs](https://learn.microsoft.com/azure/ai-foundry/concepts/observability)
- Model Leaderboard: [Azure AI Model Leaderboard](https://ai.azure.com/explore/models/leaderboard)
- Foundry Evaluations: [Docs](https://learn.microsoft.com/azure/ai-services/foundry/evaluations)
- GitHub Copilot: [https://github.com/features/copilot](https://github.com/features/copilot)

## Developer Takeaways

- **Repeatable AI Evaluation:** Leverage Foundry's SDK and UI for end-to-end evaluation.
- **Integrated Workflows:** Connect datasets, models, and evaluation results with Azure AI projects.
- **Model Comparison:** Detailed analysis across multiple LLMs improves AI solution quality.
- **Real-World Advice:** Debugging and iterative improvements are essential for reliable deployment.

---
*Episode presented by Veronika Kolesnikova and Justin Garrett. For continued learning, see the [MVP Unplugged Playlist](https://youtube.com/playlist?list=PLlrxD0HtieHhclud3yVB88znZPKCZYX_8&si=4HoycKJyUcl1qwV-).*
