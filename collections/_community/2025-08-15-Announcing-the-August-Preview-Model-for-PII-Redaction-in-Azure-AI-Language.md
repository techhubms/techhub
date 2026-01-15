---
layout: post
title: Announcing the August Preview Model for PII Redaction in Azure AI Language
author: renaliu
canonical_url: https://techcommunity.microsoft.com/t5/azure-ai-foundry-blog/announcing-the-text-pii-august-preview-model-release-in-azure-ai/ba-p/4441705
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-15 07:34:14 +00:00
permalink: /ai/community/Announcing-the-August-Preview-Model-for-PII-Redaction-in-Azure-AI-Language
tags:
- AI
- AI Model Update
- Azure
- Azure AI Foundry
- Azure AI Language
- Azure Cognitive Services
- Community
- Data Privacy
- Entity Recognition
- Financial Data
- Language Support
- LicensePlateNumber
- Machine Learning
- Multilingual AI
- Natural Language Processing
- PII Redaction
- Preview Release
- SortCode
section_names:
- ai
- azure
---
renaliu introduces the August preview of the Azure AI Language PII Redaction service, highlighting expanded multilingual and entity coverage, plus improved AI detection for regulatory scenarios.<!--excerpt_end-->

# Announcing the August Preview Model for PII Redaction in Azure AI Language

Azure AI Language has released a new preview model for its PII (Personally Identifiable Information) redaction service. This release—version *2025-08-01-preview*—broadens the service's capabilities to address real-world demands for international use, regulatory coverage, and enhanced precision in sensitive data processing.

## What's New

- **Expanded Language Support for DateOfBirth Entities:**
  - Previously supported in English only, the DateOfBirth entity now recognizes and redacts dates in all Tier 1 languages: French, German, Italian, Spanish, Portuguese, Brazilian Portuguese, and Dutch.
- **Additional Entity Types:**
  - *SortCode*: Financial codes used in the UK and Ireland for identifying banks and branches.
  - *LicensePlateNumber*: Detection of alphanumeric vehicle registration codes (with current limitations for plates containing only letters).
- **AI Quality Enhancements:**
  - Improved false positive/negative rates, especially for financial entities, leading to more reliable service performance.

## Benefits and Use Cases

These improvements meet customer feedback by broadening both the scenarios and the international reach of the PII Redaction service. This is key for sectors such as finance and criminal justice, as well as any organization processing sensitive data across multiple languages and regulatory environments.

## Resources

- [Service Feature Documentation](https://learn.microsoft.com/en-us/azure/ai-services/language-service/personally-identifiable-information/overview?tabs=text-pii)
- [What's New in Azure AI Language](https://learn.microsoft.com/en-us/azure/ai-services/language-service/whats-new?tabs=csharp)
- [Azure AI Language Overview](https://learn.microsoft.com/en-us/azure/ai-services/language-service/overview)
- [Pricing Information](https://azure.microsoft.com/en-us/pricing/details/cognitive-services/language-service/)
- [List of Supported PII Entities](https://learn.microsoft.com/en-us/azure/ai-services/language-service/personally-identifiable-information/concepts/conversations-entity-categories)
- [Try Azure AI Foundry](https://ai.azure.com/build/playground/language?wsid=/subscriptions/a9216f37-b90e-4db2-b844-b171e5394fc1/resourceGroups/xiaoy_1rv2/providers/Microsoft.MachineLearningServices/workspaces/renaliu-0105&flight=nav21,aistudiohome,ShowNotificationPanel,VisionDemonstrations,AIServicesConnection,aacs,safetymsg,UnifiedIndexCreation&tid=72f988bf-86f1-41af-91ab-2d7cd011db47&connectionName=Default_AzureAIContentSafety#convpii) – experiment with a code-free PII redaction experience.

## Feedback and Future

The Azure AI Language team encourages users to explore the new features and provide feedback to shape future releases as the service continues to evolve.

*Updated Aug 15, 2025 by renaliu*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-ai-foundry-blog/announcing-the-text-pii-august-preview-model-release-in-azure-ai/ba-p/4441705)
