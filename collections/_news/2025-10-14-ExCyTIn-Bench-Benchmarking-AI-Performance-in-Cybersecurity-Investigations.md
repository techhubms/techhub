---
layout: "post"
title: "ExCyTIn-Bench: Benchmarking AI Performance in Cybersecurity Investigations"
description: "This news piece introduces ExCyTIn-Bench, an open-source benchmarking tool from Microsoft that evaluates how well AI models perform in realistic cybersecurity scenarios. The tool leverages data-rich, multistage investigations within Azure-based SOC environments, using Microsoft Sentinel log tables and integration with security products like Defender and Security Copilot. ExCyTIn-Bench provides actionable, fine-grained metrics beyond traditional benchmarks to drive better model selection and ongoing innovation for security teams."
author: "Anand Mudgerikar"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/security/blog/2025/10/14/microsoft-raises-the-bar-a-smarter-way-to-measure-ai-for-cybersecurity/"
viewing_mode: "external"
feed_name: "Microsoft Security Blog"
feed_url: "https://www.microsoft.com/en-us/security/blog/feed/"
date: 2025-10-14 16:00:00 +00:00
permalink: "/news/2025-10-14-ExCyTIn-Bench-Benchmarking-AI-Performance-in-Cybersecurity-Investigations.html"
categories: ["AI", "Azure", "Security"]
tags: ["AI", "AI Benchmarking", "AI Security", "Azure", "Cybersecurity", "Defender", "ExCyTIn Bench", "Incident Graphs", "LLM Evaluation", "Microsoft", "Microsoft Sentinel", "News", "Open Source", "Security", "Security Copilot", "Security Operations Center", "SOC", "Threat Investigation"]
tags_normalized: ["ai", "ai benchmarking", "ai security", "azure", "cybersecurity", "defender", "excytin bench", "incident graphs", "llm evaluation", "microsoft", "microsoft sentinel", "news", "open source", "security", "security copilot", "security operations center", "soc", "threat investigation"]
---

Anand Mudgerikar presents ExCyTIn-Bench, Microsoft's open-source framework for evaluating AI systems in complex cybersecurity investigations, combining Azure SOC simulations and Sentinel logs for advanced, actionable model benchmarking.<!--excerpt_end-->

# ExCyTIn-Bench: Benchmarking AI for Real-World Cybersecurity Investigations

**Author:** Anand Mudgerikar

ExCyTIn-Bench is Microsoft’s recently released open-source toolkit to evaluate the performance of AI systems in realistic cybersecurity investigations. Unlike previous benchmarks that rely on static quizzes and trivia, ExCyTIn-Bench simulates actual attacks, demanding the kind of complex, multistage analysis you would find in a real Security Operations Center (SOC). Built around Microsoft Sentinel data and designed to operate directly within Azure, it brings industry-level rigor to AI security evaluation.

## Key Features of ExCyTIn-Bench

- **Holistic Scenario Simulation:** Leverages 57 log tables from Microsoft Sentinel plus related services to create realistic, noisy, and complex cyber incident scenarios.
- **Designed for Real Workflows:** Evaluates not just answers, but the reasoning process, as AI models interact with live data sources and plan investigations step-by-step — reflecting actual SOC analyst workflows in Azure.
- **Grounded in Incident Graphs:** Human analysts use incident graphs (alerts, entities, and relationships) as 'ground truth' for constructing explainable Q&A pairs, allowing for transparent and objective scoring.
- **Transparent, Actionable Metrics:** Provides detailed reward signals for every investigative step, enabling organizations to understand not just 'if' but 'how' a model solves a problem.
- **Open-Source and Collaborative:** Available via [GitHub](https://github.com/microsoft/SecRL), the framework invites researchers and security vendors to contribute, benchmark, and improve new AI models.

## Integrations and Impact

ExCyTIn-Bench isn’t just an external tool — it is used internally by Microsoft to improve its own AI-driven security products. Models are tested in-depth to identify weaknesses in detection, reasoning, and tool usage. The framework is tightly integrated with Microsoft Security Copilot, Microsoft Sentinel, and Defender, offering a unified way to monitor the cost and performance impact of different language models used in defense workflows.

## Insights from Latest Evaluations

- **Advanced Reasoning is Key:** The latest models (e.g., GPT-5 with high reasoning settings) significantly outperform others, confirming the importance of step-by-step reasoning for sophisticated cyber investigations.
- **Smaller Models Can Compete:** Efficient smaller models with robust chain-of-thought techniques nearly match larger models, providing more accessible options for automation.
- **Open-Source Models Improving:** Open solutions are closing the gap with proprietary systems, making quality AI security tools more obtainable.
- **Metrics Show What Matters:** Fine-grained metrics highlight the value of explicit reasoning over just final answers, guiding security teams to make better choices.

## Get Involved and Next Steps

- ExCyTIn-Bench is free and open for anyone to use, contribute, and share results.
- Personalized tenant-specific benchmarks are in development, promising even more relevant evaluations.
- For questions, contributions, or partnership opportunities, contact the team at [msecaimrbenchmarking@microsoft.com](mailto:msecaimrbenchmarking@microsoft.com).
- Stay up to date with Microsoft Security by visiting their [blog](https://www.microsoft.com/security/blog/) or following on [LinkedIn](https://www.linkedin.com/showcase/microsoft-security/) and [X](https://twitter.com/@MSFTSecurity).

## Resources

- [ExCyTIn-Bench GitHub Repository](https://github.com/microsoft/SecRL)
- [Benchmarking LLM agents on Cyber Threat Investigation](https://github.com/microsoft/SecRL)
- [ExCyTIn-Bench: Evaluating LLM agents on Cyber Threat Investigation (arXiv)](https://arxiv.org/abs/2507.14201)
- [Upcoming Microsoft Security Events](https://ignite.microsoft.com/en-US/home?wt.mc_ID=Ignite2025_marx_corp_bl_oo_bl_Security_2_1)
- [Microsoft Sentinel](https://www.microsoft.com/en-us/security/business/siem-and-xdr/microsoft-sentinel/?msockid=27b7b3bc5be566bc06c9a5a05a7a679d)

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/10/14/microsoft-raises-the-bar-a-smarter-way-to-measure-ai-for-cybersecurity/)
