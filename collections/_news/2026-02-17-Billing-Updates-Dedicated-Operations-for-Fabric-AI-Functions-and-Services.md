---
layout: "post"
title: "Billing Updates: Dedicated Operations for Fabric AI Functions and Services"
description: "This news highlights Microsoft Fabric's latest billing reporting changes, introducing clear, dedicated operations for tracking AI Functions and AI Services usage. The update separates Fabric AI functions and Azure AI Services billing from other workloads, offering improved transparency and easier tracking of AI-related consumption within Fabric, especially for operations using Azure OpenAI Service and Azure Cognitive Services."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/billing-updates-new-operations-for-fabric-ai-functions-and-ai-services/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-02-17 12:00:00 +00:00
permalink: "/2026-02-17-Billing-Updates-Dedicated-Operations-for-Fabric-AI-Functions-and-Services.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "AI Functions", "AI Services", "Azure", "Azure AI Translator", "Azure Cognitive Services", "Azure OpenAI Service", "Billing", "Capacity Metrics", "Copilot Capacity", "Dataflows Gen2", "Microsoft Fabric", "ML", "News", "Notebook", "REST API", "Synapse ML", "Text Analytics"]
tags_normalized: ["ai", "ai functions", "ai services", "azure", "azure ai translator", "azure cognitive services", "azure openai service", "billing", "capacity metrics", "copilot capacity", "dataflows gen2", "microsoft fabric", "ml", "news", "notebook", "rest api", "synapse ml", "text analytics"]
---

Microsoft Fabric Blog details upcoming billing reporting updates by introducing new dedicated operations for Fabric AI Functions and AI Services. This aims to clarify and separate AI-related consumption for users and organizations.<!--excerpt_end-->

# Billing Updates: Dedicated Operations for Fabric AI Functions and Services

Microsoft Fabric is introducing new billing reporting updates to improve transparency and tracking for AI-related usage. Previously, usage of Fabric AI functions was reported under broader operational categories such as Spark or Dataflows Gen2 operations. With this update, Microsoft is establishing separate, dedicated operations for both **AI Functions** and **AI Services**, allowing for clearer visibility into AI-specific consumption.

## New AI Functions Operation

- AI Functions usage, whether initiated from Notebooks or Dataflows Gen2, will now be reported under a specific "AI Functions" operation.
- This includes activities such as calls to Azure OpenAI Service via REST API, Python SDK, or Synapse ML.
- The goal is to make it easier for users to understand and attribute compute costs directly related to Fabric's built-in AI capabilities.

## New AI Services Operation

- Text Analytics and Azure AI Translator (part of Azure AI Services in Fabric, previously Azure Cognitive Services) will have a dedicated "AI Services" operation.
- This operation helps distinguish and track consumption of these AI features separately from other workloads.

## Billing and Rollout Details

- Both new operations—AI Functions and AI Services—will be billed under the **Copilot and AI Capacity Usage CU** billing meter.
- Users will start to see these changes reflected in the Microsoft Fabric Capacity Metrics application beginning March 17, 2025.
- The update does not change the underlying consumption rates; it is a reporting change aimed at easier allocation and management of AI costs.

| Operation     | Description                                                                  | Item           | Azure Billing Meter                |
|--------------|------------------------------------------------------------------------------|----------------|-------------------------------------|
| AI Functions | Compute cost for Fabric AI functions and Azure OpenAI Service                | Notebook       | Copilot and AI Capacity Usage CU    |
| AI Functions | Compute cost for Fabric AI functions                                         | Dataflow Gen2  | Copilot and AI Capacity Usage CU    |
| AI Services  | Compute cost for Azure AI Services in Fabric (Text Analytics, Translator)     | Notebook       | Copilot and AI Capacity Usage CU    |

These changes improve cost transparency, making it easier to understand, allocate, and manage AI-related consumption within your Microsoft Fabric environment.

## Next Steps and Resources

- For full details on Microsoft Fabric operations and billing, refer to [Fabric operations documentation](https://learn.microsoft.com/fabric/enterprise/fabric-operations).
- To explore AI functions in detail, visit the [AI functions documentation](https://learn.microsoft.com/fabric/data-science/ai-functions/overview).
- Feedback can be provided via [Fabric ideas](https://aka.ms/FabricBlog/ideas).

> **Note:** These updates are focused on reporting and do not affect the actual rates of AI capacity consumption.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/billing-updates-new-operations-for-fabric-ai-functions-and-ai-services/)
