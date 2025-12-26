---
layout: "post"
title: "Microsoft Fabric AI Functions: Enhanced Features Now Generally Available"
description: "This news article summarizes the latest enhancements to Microsoft Fabric AI functions, including new features, additional configuration options, support for advanced models (like GPT-5), and improved concurrency for faster data processing. Developers and data scientists can now apply advanced parameters, use new embedding capabilities, and benefit from broader model integration — including Azure OpenAI and AI Foundry resources — across PySpark and pandas workflows."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/29826/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-19 01:30:00 +00:00
permalink: "/news/2025-11-19-Microsoft-Fabric-AI-Functions-Enhanced-Features-Now-Generally-Available.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "AI Functions", "Ai.analyze Sentiment", "Ai.classify", "Ai.embed", "Ai.extract", "Ai.fix Grammar", "Ai.generate Response", "Ai.similarity", "Ai.summarize", "Ai.translate", "Azure", "Azure AI Foundry", "Azure OpenAI", "Claude", "Concurrency", "Data Science", "GPT 5", "LLaMA", "Machine Learning", "Microsoft Fabric", "ML", "Model Configuration", "News", "Pandas", "PySpark", "Vector Embeddings"]
tags_normalized: ["ai", "ai functions", "aidotanalyze sentiment", "aidotclassify", "aidotembed", "aidotextract", "aidotfix grammar", "aidotgenerate response", "aidotsimilarity", "aidotsummarize", "aidottranslate", "azure", "azure ai foundry", "azure openai", "claude", "concurrency", "data science", "gpt 5", "llama", "machine learning", "microsoft fabric", "ml", "model configuration", "news", "pandas", "pyspark", "vector embeddings"]
---

Microsoft Fabric Blog details the latest generally available enhancements to Fabric AI functions, including deeper configurability and new features for developers, data scientists, and analysts.<!--excerpt_end-->

# Microsoft Fabric AI Functions: Enhanced Features Now Generally Available

Fabric AI functions have received significant updates, offering users improved flexibility and power when transforming data with AI. Key highlights from this release include:

## New and Enhanced Functions

- **ai.analyze_sentiment()** – Detect emotional state, now with configurable labels ('positive', 'negative', 'neutral', 'mixed') or custom options.
- **ai.classify()** – Categorize data based on user-defined labels.
- **ai.embed() (New!)** – Generate vector embeddings from text, enabling semantic comparison, grouping, and search.
- **ai.extract()** – Extract specific data types using advanced parameters:
  - *label*: Custom column names
  - *description*: Extra context/instructions for extraction
  - *max_items*: Limit extraction quantity
  - *type*: Data type (string, number, integer, boolean, object, array)
  - *properties*: JSON schema elements for complex types
- **ai.fix_grammar()** – Automated correction of spelling, grammar, and punctuation.
- **ai.generate_response()** – Custom prompt-based generation, now supporting output formatting (text, JSON, pydantic model schema).
- **ai.similarity()** – Compare semantic meaning across text values.
- **ai.summarize()** – Summarize content, now with instructions for controlled output length.
- **ai.translate()** – Translate text to other languages.

## Major Enhancements

- **New Optional Parameters** for deeper control across functions
- **Support for Advanced Models:**
  - Use GPT-5 with configurable *reasoning_effort* and *verbosity*
  - Choose from Fabric-supported models, Azure OpenAI resources, or AI Foundry (access models beyond OpenAI, e.g. Claude, LLaMA)
- **Faster, Parallel Execution:**
  - Default concurrency raised to 200 for improved processing speed with asynchronous requests

## Developer Experience

- *PySpark* and *pandas* workflows fully supported
- Detailed documentation provided:
  - [AI functions overview](https://learn.microsoft.com/fabric/data-science/ai-functions/overview)
  - [Custom configurations](https://learn.microsoft.com/fabric/data-science/ai-functions/pandas/configuration)
- Community feedback encouraged: [Fabric Ideas](https://aka.ms/FabricBlog/ideas) | [Fabric Community](https://aka.ms/FabricBlog/Community)

## Example Use Cases

- Applying sentiment analysis with domain-specific labels
- Extracting structured information from text with custom JSON schema
- Using embeddings for intelligent search and grouping
- Summarizing data and customizing output length
- Accelerating processing with increased concurrency
- Expanding AI and ML modeling beyond OpenAI models

## Availability

- Updates are generally available in all geographies in the coming weeks.
- Full details and guides: [AI functions documentation](https://learn.microsoft.com/fabric/data-science/ai-functions/overview)

---

These enhancements enable more powerful, flexible, and scalable AI-driven data science and ML workflows within Microsoft Fabric, leveraging both built-in and bring-your-own-model approaches.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/29826/)
