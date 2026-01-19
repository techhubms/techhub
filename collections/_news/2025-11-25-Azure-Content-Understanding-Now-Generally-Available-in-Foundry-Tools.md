---
external_url: https://devblogs.microsoft.com/foundry/azure-content-understanding-is-now-generally-available/
title: Azure Content Understanding Now Generally Available in Foundry Tools
author: Joe Filcik
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
date: 2025-11-25 16:00:24 +00:00
tags:
- Agent Ecosystem
- Audit Automation
- Azure AI Search
- Azure AI Services
- Azure Content Understanding
- Content Extraction
- Document Intelligence
- Domain Specific Analyzers
- Field Extraction
- Finance Workflows
- Foundry Tools
- Generative AI
- Layout Analysis
- Microsoft Foundry
- OCR
- Prebuilt Analyzers
- Production Workloads
- RAG
- Retrieval Augmented Generation
- Unstructured Data
section_names:
- ai
- azure
---
Joe Filcik announces the general availability of Azure Content Understanding in Microsoft Foundry Tools, detailing enhancements in model deployment, document analysis, generative AI integration, and practical customer applications across industries.<!--excerpt_end-->

# Azure Content Understanding Now Generally Available in Foundry Tools

Azure Content Understanding has reached general availability (GA) within Microsoft Foundry Tools, introducing industry-grade, production-ready features for intelligent document analysis and retrieval-augmented generation (RAG) solutions.

## What's New

- **Direct Connection to Foundry Models:**
  - Deploy Content Understanding with any Microsoft Foundry model.
  - Configure for quality, cost, and latency to match your use cases.
  - Supports pay-as-you-go and Provisioned Throughput Units (PTUs) with expanded global availability.

- **Integration with Foundry IQ and Azure AI Search:**
  - Seamlessly connect content extraction with enterprise search and agentic workflows, streamlining ingestion and metadata enrichment.

- **Model Flexibility and Architecture:**
  - Combine specialized models (for OCR, layout, and transcription) with generative AI (for field extraction, segmentation, figure analysis).
  - Enable predictable costs and high-quality outputs for various applications.

- **Expanded Region Availability:**
  - Now available in 12 regions, offering improved data residency and lower latencies.

## Prebuilt and Domain-Specific Analyzers

- **Prebuilt Analyzer Library:**
  - Ready-to-use analyzers for top use cases including RAG ingestion, finance, procurement, and more, extending Azure Document Intelligence with foundation models.

- **RAG-Specific Tools:**
  - Extract tables, paragraphs, sections, and figures for richer structured outputs essential for retrieval-augmented generation.

- **Multimodal Support:**
  - Prebuilt analyzers for video, audio, and images enable structured outputs and context-aware data for agentic or search solutions.

- **Layout-Aware Enrichment:**
  - Extracts structured information like tables (even spanning pages), figures, diagrams, and charts, outputting formats like Chart.js and Mermaid.js for downstream use.

- **Domain-Specific Accelerators:**
  - Analyzers for finance (statements, tax forms), contracts & procurement, ID verification, mortgage and lending, and more for fast, accurate field extraction.

## Intelligent Document Processing and Workflow Upgrades

- **Segmentation and Classification:**
  - Automatically split complex documents (e.g., visa applications) into their components, routing each part to appropriate analyzers.

- **Granular Control and Cost Reduction:**
  - Enhanced support for confidence scores, grounding, complex schemas, and field-level extraction choices, now including mini and nano models for cost-efficient processing.

- **Operational Improvements:**
  - Automatic method selection (extract/generate), support for complex object types, and consistency across modalities.

## Integration with Agents and Multi-Agent Systems

- Standardized file representation and schema for developers building agent workflows.
- Upcoming Content Understanding MCP server for advanced agentic integrations.

## Customer Success Stories

- **KPMG:** Uses Content Understanding within KPMG Clara AI to transform unstructured audit data into structured, actionable insights, streamlining complex audit engagements.

- **DataSnipper:** Empowers finance teams to extract and validate data within Excel using Azure Content Understanding, linking extracted values to original sources for full auditability and efficiency.

- **SJR:** Embeds Content Understanding in their GX Manager, achieving personalization at scale and paradigm shifts in digital marketing with structured, AI-powered content.

## Getting Started

- **Target impactful use cases:** Start with high-friction workflows (contracts, forms, application-document pipelines) to automate and structure processes.
- **Explore the new Content Understanding UI:** [Content Understanding in Microsoft Foundry](https://aka.ms/foundry-content-understanding)
- **Documentation and Demos:**
  - [Overview](https://learn.microsoft.com/en-us/azure/ai-services/content-understanding/overview)
  - [Prebuilt Analyzers](https://learn.microsoft.com/en-us/azure/ai-services/content-understanding/concepts/prebuilt-analyzers#retrieval-augmented-generation-rag-analyzers)
  - [Azure AI Search Integration](https://learn.microsoft.com/en-us/azure/search/cognitive-search-skill-content-understanding)
  - [GitHub Samples](https://aka.ms/cu-samples)
- **Related sessions:**
  - [Innovation Session: Build & Manage AI Apps with Your Agent Factory](https://ignite.microsoft.com/en-US/sessions/BRK1706?source=sessions)
  - [Introducing Microsoft Foundry Tools](https://ignite.microsoft.com/en-US/sessions/BRK192)

## Conclusion

Azure Content Understanding, now generally available, provides a flexible and robust foundation for building intelligent document and content processing solutions with native integration across the Microsoft ecosystem. This release brings together advanced AI, multimodal support, prebuilt and domain-specific analyzers, and improved developer tooling, enabling organizations to translate content complexity into actionable insights at enterprise scale.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/azure-content-understanding-is-now-generally-available/)
