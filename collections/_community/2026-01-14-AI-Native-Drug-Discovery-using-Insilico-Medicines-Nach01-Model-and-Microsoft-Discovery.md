---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-native-drug-discovery-using-insilico-medicine-s-nach01-model/ba-p/4484497
title: AI-Native Drug Discovery using Insilico Medicine’s Nach01 Model and Microsoft Discovery
author: JohnGruszczyk
feed_name: Microsoft Tech Community
date: 2026-01-14 14:00:00 +00:00
tags:
- ADMET Prediction
- AI Workflows
- Azure ML Workspace
- Biotechnology
- Chemical Design
- Cloud Orchestration
- Cognition Engine
- Drug Discovery
- Foundation Models
- Generative AI
- GraphRAG
- Insilico Medicine
- Microsoft Discovery
- Molecular Property Prediction
- Multimodal AI
- Nach01
- Reproducible Research
- Scalable Compute
- AI
- Azure
- ML
- Community
- Machine Learning
section_names:
- ai
- azure
- ml
primary_section: ai
---
JohnGruszczyk discusses how Insilico Medicine’s Nach01 model integrates with Microsoft Discovery on Azure, enabling AI-driven, reproducible, and scalable drug discovery workflows for researchers.<!--excerpt_end-->

# AI-Native Drug Discovery using Insilico Medicine’s Nach01 Model and Microsoft Discovery

Drug discovery has traditionally faced challenges around workflow fragmentation, isolated tools, and reproducibility. In this post, JohnGruszczyk presents an in-depth look at a collaborative demonstration between Insilico Medicine and Microsoft, showcasing how the Nach01 foundation model can be deployed on Microsoft Discovery—a science-focused, agentic AI platform built on Azure—for accelerated R&D.

## Tackling Workflow Fragmentation in Drug Discovery

Fragmented scientific tasks such as hypothesis generation, molecular design, and data analysis are often spread across disparate tools, impacting innovation speed and reproducibility. To overcome these hurdles, Insilico Medicine partnered with Microsoft to integrate the Nach01 model with Microsoft Discovery.

Nach01 is a multimodal foundation model capable of handling hundreds of molecular design and prediction tasks by combining language models and molecular point cloud encoders. It facilitates:

- Molecular property prediction
- Compound design
- 3D spatial representation learning

## Microsoft Discovery: An Enterprise Agentic AI Platform

[Microsoft Discovery](https://azure.microsoft.com/en-us/blog/transforming-rd-with-agentic-ai-introducing-microsoft-discovery/) is designed to accelerate research and development through the following capabilities:

- **Cognition engine** for orchestrating complex, multi-step investigations
- **Knowledge generation** via internal documentation and graph-based retrieval ([GraphRAG](https://www.microsoft.com/en-us/research/project/graphrag/))
- **Scalable compute orchestration** using Azure ML Workspace for elastic model deployment and inference
- **Seamless data connectivity** leveraging Microsoft Entra ID for access management

## Integrating Nach01 into Microsoft Discovery

Researchers can extend Microsoft Discovery by integrating the Nach01 model into AI-driven workflows, enabling:

- End-to-end computational designs (e.g., structure-based design, property prediction, and data analysis, all orchestrated in one workflow)
- Dynamic scaling of compute resources according to workload demands
- Traceability and reproducibility of investigative processes (investigative blueprints and configurations are preserved for reruns and handoff)

### Example Workflow Steps

1. **Target Identification**: Use knowledge graphs or literature mining to confirm biological targets
2. **Hit Generation with Nach01**: Generate novel chemical structures likely to bind the target protein
3. **Molecule Evaluation**: Predict properties like solubility and toxicity (ADMET) via Nach01 modules
4. **Additional Scoring**: Utilize third-party QSAR models or docking tools within the workflow
5. **Automated Reporting**: Compile, visualize, and review the top candidate molecules

All steps are fully orchestrated on Azure through Microsoft Discovery, allowing scientists to quickly iterate, refine, and reproduce results.

## Conclusion

By enabling the integration of specialized generative models such as Nach01 with robust cloud infrastructure, Microsoft Discovery empowers scientists to focus on scientific logic while automating technical logistics. This approach paves the way for rapid, reproducible, and collaborative research in drug discovery. As AI continues to advance, such platforms will drive innovation from hypothesis to life-saving medicines more efficiently than ever before.

> “While Nach01 is a small, specialized model, its successful launch demonstrates the immense power and scalability of the Microsoft platform. We are now applying these learnings to develop large, highly multimodal models aimed at delivering SOTA and SOTA+ results in every aspect of drug discovery—a critical step toward scientific superintelligence.” — Alex Zhavoronkov, PhD (Insilico Founder and CEO)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-native-drug-discovery-using-insilico-medicine-s-nach01-model/ba-p/4484497)
