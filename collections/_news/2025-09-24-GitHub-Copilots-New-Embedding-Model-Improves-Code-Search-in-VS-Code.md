---
layout: post
title: GitHub Copilot’s New Embedding Model Improves Code Search in VS Code
author: Shengyu Fu
canonical_url: https://github.blog/news-insights/product-news/copilot-new-embedding-model-vs-code/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-09-24 20:24:18 +00:00
permalink: /github-copilot/news/GitHub-Copilots-New-Embedding-Model-Improves-Code-Search-in-VS-Code
tags:
- Agent Mode
- Agentic Search
- AI Coding Assistant
- AI Models
- Code Retrieval
- Code Search
- Contrastive Learning
- Copilot Chat
- Developer Productivity
- Embedding
- Embedding Model
- Generative AI
- Hard Negative Mining
- Machine Learning
- News & Insights
- Product
- Programming Languages
- RAG
- Semantic Search
- VS Code
section_names:
- ai
- coding
- github-copilot
---
Shengyu Fu details how the new GitHub Copilot embedding model enhances code search in VS Code by boosting retrieval quality, improving efficiency, and leveraging advanced AI training techniques.<!--excerpt_end-->

# GitHub Copilot’s New Embedding Model: Smarter Code Search in VS Code

**Author: Shengyu Fu**

Finding and reusing the right code is a critical part of everyday programming. GitHub Copilot has introduced a new embedding model aimed at making code search in Visual Studio Code both faster and more accurate, while consuming less memory. This article explains the improvements, how the model was trained, and the impact for developers.

## Key Improvements

- **Retrieval Quality:** A 37.6% increase in relative retrieval quality across multiple benchmarks, including a 110.7% improvement in code acceptance ratios for C# developers and 113.1% for Java developers.
- **Efficiency:** The embedding model achieves approximately 2x higher throughput, 8x smaller index size, and lower memory usage. This results in faster responses and improved scaling both on client and server.
- **Enhanced Code Context:** By leveraging embeddings (vector representations of code and documentation), the model retrieves semantically relevant snippets, not just those with matching keywords.

## Role of Embeddings

Embeddings allow GitHub Copilot to understand and match developer intent even if the words used in code or documentation differ. The new model improves the relevance of search results within Copilot chat and agentic workflows, delivering more accurate code suggestions and documentation retrieval.

## Model Training Overview

- **Contrastive Learning & Hard Negatives:** The model was trained using contrastive learning with InfoNCE loss and Matryoshka Representation Learning, enabling it to better differentiate between nearly identical code snippets. 'Hard negatives'—tricky, almost-correct code samples—were incorporated into training sets to further boost retrieval accuracy.
- **Training Data:** Data was drawn from both public GitHub and Microsoft/GitHub internal repositories, covering a diverse array of languages: Python (36.7%), Java (19%), C++ (13.8%), JavaScript/TypeScript (8.9%), C# (4.6%), and others (17%).

## Evaluation Suite

The evaluation focuses on:

- **Natural language (NL) to code:** Matching queries to code snippets.
- **Code to NL:** Generating natural language summaries from code.
- **Code to code:** Finding similar or refactored snippets.
- **Problems to code:** Suggesting code fixes for given issues.

## Real-World Scenarios

Developers benefit in situations such as:

- Searching for test functions in large repositories.
- Finding helper methods distributed across files.
- Debugging: e.g., locating where a specific error string is handled.

## Technical Approach Example

When prompted, “Which method is invoked to find a single namespace by its name within the project?” the new embedding model accurately retrieved the `findOne` function (the correct context), outperforming the prior model, which returned the similar but incorrect `find` function.

## Future Directions

GitHub plans to expand training and evaluation to more languages, enhance negative sampling techniques, and leverage efficiency gains to develop even larger, more accurate models.

---

**Try out GitHub Copilot’s improved search experiences within VS Code:** [GitHub Copilot Features](https://github.com/features/copilot)

---

**Acknowledgments**
Thanks to the engineers and researchers at GitHub and Microsoft who contributed to the model’s training pipeline, evaluation, and deployment.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/product-news/copilot-new-embedding-model-vs-code/)
