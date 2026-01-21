---
external_url: https://www.microsoft.com/en-us/research/blog/when-industry-knowledge-meets-pike-rag-the-innovation-behind-signifys-customer-service-boost/
title: Signify Boosts Customer Service Accuracy with Microsoft PIKE-RAG on Azure
author: stclarke
feed_name: Microsoft News
date: 2025-11-06 17:14:37 +00:00
tags:
- Algorithm Optimization
- Company News
- Customer Service AI
- Document Intelligence
- Domain Adaptation
- Enterprise AI
- Industrial AI
- Knowledge Management
- LED Lighting
- Microsoft Azure OpenAI
- Microsoft Research Asia
- Multimodal AI
- PIKE RAG
- Retrieval Augmented Generation
- Signify
- Technology
section_names:
- ai
- azure
---
stclarke explores how Signify enhanced its customer service knowledge management system using Microsoft Azure and PIKE-RAG, collaborating with Microsoft Research Asia to achieve greater answer accuracy and efficiency.<!--excerpt_end-->

# Signify Boosts Customer Service Accuracy with Microsoft PIKE-RAG on Azure

*Author: stclarke*

## Overview

Signify (formerly Philips Lighting) partnered with Microsoft Research Asia to improve its customer service response accuracy. By leveraging PIKE-RAG (Prompt, Indexed Knowledge, and Reasoning-Augmented Generation) technology, integrated with Microsoft Azure, Signify's knowledge management system achieved a 12% improvement in answer accuracy.

## Background & Challenges

Signify serves a diverse clientele, from general consumers to professionals requiring detailed technical support on thousands of products. Their challenge was efficiently answering complex queries about specifications and compatibility across vast, multimodal documentation (text, tables, diagrams).

Retrieval-Augmented Generation (RAG) offered a solution, but standard implementations struggled with nonstandard tables and complex reasoning. This led Signify to collaborate on integrating PIKE-RAG—a system designed for domain adaptability and multimodal understanding.

## PIKE-RAG Solution

PIKE-RAG, developed by Microsoft Research Asia, extends traditional RAG by:

- Efficiently parsing multimodal documents—including charts, tables, and diagrams—using Document Intelligence and Azure OpenAI models.
- Understanding domain-specific reasoning, crucial for engineering and technical queries.
- Establishing reliable citation relationships across multiple data sources for more accurate reasoning and eliminating reliance on potentially outdated or erroneous sources.
- Supporting advanced capabilities like dynamic task decomposition and multi-hop reasoning, allowing the system to answer complex, multi-part customer queries.

### Key Benefits in the Signify Use Case

- **Multimodal Document Analysis:** Accurately interprets tables and diagrams where traditional systems fail.
- **Automated Reasoning:** Breaks down complex queries and synthesizes answers from multiple documents and knowledge types.
- **End-to-End Knowledge Loop:** Integrates and validates information across disparate sources to ensure trustworthy outputs.

## Results

- **12% increase** in answer accuracy over the previous system, achieved through algorithmic improvement with no question-specific customization.
- Enhanced system learning and continual improvement as it ingests more proprietary Signify data.
- Positive feedback from Signify, noting both technical advancement and Microsoft Research Asia’s rigorous methodology.

## Broader Industry Impact

PIKE-RAG has demonstrated its versatility by generalizing across industries like manufacturing and pharmaceuticals, as well as lighting. Key features supporting this adaptability include:

- **Self-Evolution/Continuous Learning:** The system learns from error cases to optimize knowledge extraction strategies and adapts to new document types automatically.
- **Modular Architecture:** Components for document parsing, retrieval, reasoning, and organization can be flexibly combined and adjusted to the needs of each use case.
- **Domain-Specific Reasoning:** Supports injection of custom domain logic (like specific electrical engineering standards) to tailor Q&A to professional contexts.

## Technical Highlights

- Azure hosts both Document Intelligence and OpenAI model components, providing scalable, cloud-based AI services.
- PIKE-RAG dynamically decomposes and orchestrates reasoning tasks, calling on specialized tools (e.g., for Text2SQL, visual parsing) for optimal information access.

## Conclusion

Through the collaboration, Signify demonstrated measurable improvements in customer service efficiency and accuracy. PIKE-RAG, integrated via Microsoft Azure, enables scalable advancement in knowledge management and could serve as a model for other enterprises facing complex Q&A and documentation challenges.

For enterprise use cases aiming to move beyond generic chatbots and towards professional, domain-aligned knowledge reasoning, PIKE-RAG and the Microsoft Azure platform offer a robust, future-ready foundation.

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/research/blog/when-industry-knowledge-meets-pike-rag-the-innovation-behind-signifys-customer-service-boost/)
