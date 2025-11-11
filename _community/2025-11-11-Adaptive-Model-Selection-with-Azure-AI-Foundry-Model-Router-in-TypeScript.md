---
layout: "post"
title: "Adaptive Model Selection with Azure AI Foundry Model Router in TypeScript"
description: "This article, authored by Julia Muiruri, introduces the Model Router feature (preview) within Azure AI Foundry. The Model Router enables developers to send a chat request and have Azure select the optimal underlying model (such as gpt-4.1-mini, gpt-5 variants) for each prompt, optimizing cost and performance. The blog discusses use cases, implementation in TypeScript, versioning, monitoring, and practical limitations. It also covers real-world scenarios of dynamic model selection, configuration details, and monitoring strategies using Azure services."
author: "Julia_Muiruri"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/adaptive-model-selection-in-typescript-with-the-model-router/ba-p/4465192"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-11 08:00:00 +00:00
permalink: "/2025-11-11-Adaptive-Model-Selection-with-Azure-AI-Foundry-Model-Router-in-TypeScript.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "API Authentication", "Application Insights", "Azure", "Azure AI Foundry", "Azure Inference SDK", "Azure Monitor", "Chat Models", "Coding", "Community", "Cost Optimization", "Dynamic Model Selection", "GPT 4.1", "GPT 5", "Latency Monitoring", "LLM Routing", "Microsoft Entra ID", "Model Router", "Observability", "Reasoning Models", "TypeScript", "Versioning"]
tags_normalized: ["ai", "api authentication", "application insights", "azure", "azure ai foundry", "azure inference sdk", "azure monitor", "chat models", "coding", "community", "cost optimization", "dynamic model selection", "gpt 4dot1", "gpt 5", "latency monitoring", "llm routing", "microsoft entra id", "model router", "observability", "reasoning models", "typescript", "versioning"]
---

Julia Muiruri shares how to use Azure AI Foundry's Model Router in TypeScript for intelligent model selection per prompt, showing implementation, architecture, and monitoring best practices.<!--excerpt_end-->

# Adaptive Model Selection with Azure AI Foundry Model Router in TypeScript

Author: Julia Muiruri

## Overview

The Model Router feature (preview) in Azure AI Foundry lets you deploy a chat endpoint that intelligently routes each prompt to the most suitable Azure OpenAI model variant (for example, gpt-4.1-mini, gpt-4.1-nano, o4-mini, or gpt-5). This guide explains why and how to use Model Router, including implementation in TypeScript, and strategies for monitoring and version management.

## What Is the Model Router?

Model Router is a special Azure AI Foundry deployment that sits in front of multiple models, taking a prompt and choosing an appropriate backend model based on complexity. Rather than hard-coding model selection in your application, you send all prompts to a single endpoint and let the service optimize for performance, cost, and capability. The JSON response contains the underlying model used for each answer.

**Key architectural points:**

- **Single deployment:** Unified filters and rate limits.
- **Dynamic model choice:** Cheaper models for simpler prompts, upscaled for more complex requests.
- **Future-proof:** Newer models can be picked up automatically with option for auto-update.

## Why Use Model Router?

- **Cost Efficiency:** Reduces overuse of expensive, large reasoning models.
- **Operational Simplicity:** One endpoint, easier logging and config.
- **Performance Balance:** Only escalate to reasoning models when required.
- **Version Agility:** Toggle auto-update as appropriate per environment.
- **Observability:** `model` field in responses lets you split logs and dashboards by specific models.

### Example Scenarios

- **Customer Support Triage:** Short prompts routed to nano/mini models for low cost.
- **Developer Knowledge Assistant:** Escalates to reasoning models (e.g., o4-mini, gpt-5) for code-related questions.
- **Strategic Analytics Q&A:** Deep queries go to high-reasoning models with more compute.

## Implementation in TypeScript

**Pre-requisites:**

- Azure AI Foundry project
- Model Router deployment

**Authentication:** Sample uses API key, but Managed Identity (Microsoft Entra ID) is recommended in production.

```typescript
const client = ModelClient(endpoint, new AzureKeyCredential(key));

const messages = [
  { role: "system", content: "You are a helpful assistant." },
  { role: "user", content: "Give me a concise 5-bullet travel safety list for solo backpacking in South America." }
];

const response = await client.path("/chat/completions").post({
  body: {
    model: "model-router",
    messages,
    max_tokens: 512,
    temperature: 0.7,
    top_p: 0.95
  }
});

console.log("Model chosen by the router:", response.body?.model ?? "(unknown)");
console.log("Model response:", response.body?.choices?.[0]?.message?.content);
console.log("Usage:", response.body?.usage);
```

### Multi-turn Conversation Strategy

Use context trimming to limit history and token costs:

```typescript
function trimHistory(messages: ChatMessage[], maxHistoryTurns: number): ChatMessage[] {
  if (messages.length <= 1) return messages; // keep system message
  const head = messages.slice(0, 1);
  const body = messages.slice(1);
  const keep = maxHistoryTurns * 2; // user + assistant pairs
  return [...head, ...body.slice(-keep)];
}
```

## Versioning and Monitoring

Router deployments map to a fixed set of models. Managing upgrade policies (auto vs manual) impacts cost and latency. Recommended lifecycle:

- **Dev/Test:** Enable auto-update to evaluate improvements fast.
- **Staging:** Compare token usage, routing distribution.
- **Prod:** Scale after regression tests on cost and latency KPIs.

Use Azure Monitor and Application Insights to track router performance by underlying model (filter by deployment name and/or model field). In Cost Analysis, use resource tags to focus on router-related spend.

## Known Limitations

- **Context Window:** Large prompts routed to small-window models may fail. Summarize/truncate inputs as needed.
- **Model Parameters:** Settings like `temperature` and `top_p` may not apply for reasoning models. Use prompt engineering for output control.
- **Modalities:** Only text/image input and text output are supported so far.
- **Latency:** Reasoning models often have higher latency. Consider circuit breakers for time-sensitive cases.

## Resources

- [Model Router Sample Code (Python & TypeScript)](https://github.com/Azure-Samples/insideAIF/tree/main/Samples/Model-Router)
- [Model Router How-To](https://learn.microsoft.com/azure/ai-foundry/openai/how-to/model-router)
- [Model Router Concepts](https://learn.microsoft.com/azure/ai-foundry/openai/concepts/model-router)
- [Working with Models and Upgrade Policies](https://learn.microsoft.com/azure/ai-foundry/openai/how-to/working-with-models)
- [Router Quotas & Limits](https://learn.microsoft.com/azure/ai-foundry/openai/quotas-limits)
- [Models Catalog (Availability & Capabilities)](https://learn.microsoft.com/azure/ai-foundry/openai/concepts/models#model-router)
- [Reasoning Models](https://learn.microsoft.com/azure/ai-foundry/openai/how-to/reasoning)

---

_Last updated: Oct 30, 2025. Version 1.0_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/adaptive-model-selection-in-typescript-with-the-model-router/ba-p/4465192)
