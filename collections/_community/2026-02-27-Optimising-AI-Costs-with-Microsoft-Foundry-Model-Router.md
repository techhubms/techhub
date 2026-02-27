---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/optimising-ai-costs-with-microsoft-foundry-model-router/ba-p/4494776
title: Optimising AI Costs with Microsoft Foundry Model Router
author: Lee_Stott
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-27 08:00:00 +00:00
tags:
- AI
- AI Cost Optimization
- API Integration
- Azure
- Azure OpenAI Service
- Benchmarking
- Community
- Cost Quality Tradeoffs
- LLM
- Managed Identity
- Microsoft Foundry Model Router
- Model Routing
- Operational Flexibility
- Production Deployment
- Prompt Complexity Analysis
- React
- Recharts
- Routing Strategy
- Tailwind CSS
- TypeScript
- Vite
section_names:
- ai
- azure
---
Lee_Stott demonstrates how the Microsoft Foundry Model Router autonomously selects Azure OpenAI models to optimise costs and performance, sharing a hands-on demo, benchmark results, and practical setup guidance.<!--excerpt_end-->

# Optimising AI Costs with Microsoft Foundry Model Router

**Author: Lee_Stott**

## Overview

The Microsoft Foundry Model Router provides a solution for efficiently managing AI inference costs by routing each prompt to the most suitable Azure OpenAI model based on its complexity. This article showcases a hands-on demo, discusses technical implementation, reviews benchmarking data, and offers practical advice for integrating the Model Router into real-world solutions.

## The Challenge: One Model Doesn't Fit All

Typical AI model deployments force you into a single model choice—either fast and cheap, but limited, or powerful and expensive. For workloads featuring both simple and complex tasks, this means unnecessary overspending.

| Strategy              | Benefit             | Drawback                        |
|----------------------|--------------------|---------------------------------|
| Use a small model    | Fast, cheap        | Can't handle complex queries    |
| Use a large model    | Powerful, flexible | Overpay for simple tasks        |
| Build your own router| Total control      | High maintenance, hard to scale |

## The Solution: Model Router

Model Router acts as a meta-AI that analyses each incoming prompt for complexity, task type, and context length. It then dynamically routes the request to the optimal underlying model from a pool you configure—all through a single Azure endpoint.

**Key Functions:**

1. Analyses prompt properties
2. Selects the best-fit model from the routing pool
3. Forwards, then returns the model's response
4. Exposes the choice via the `response.model` field

Developers interact with just one deployment—no extra routing logic required in their own code.

### Routing Modes

- **Balanced (default):** Cost-quality trade-off
- **Cost:** Minimise spend, may use simpler models often
- **Quality:** Maximise accuracy, uses premium models for complex queries

Modes are configurable in the Azure Foundry Portal with no code changes.

## Hands-On Demo: Interactive Benchmarking

The [router-demo-app](https://github.com/leestott/router-demo-app/) is a React + TypeScript application showcasing Model Router in action. It compares:

- Selected model for each prompt
- Latency in milliseconds
- Token usage (prompt and completion)
- Estimated cost based on model pricing

**Features:**

- 10 diverse pre-built prompts (simple to highly complex)
- Custom prompt input for benchmarking real workloads
- Three routing modes (Balanced, Cost, Quality)
- Batch benchmarking for aggregate analysis

### API Integration Example

```javascript
const response = await fetch(
  `${endpoint}/openai/deployments/model-router/chat/completions?api-version=2024-10-21`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', 'api-key': apiKey },
    body: JSON.stringify({ messages: [{ role: 'user', content: prompt }], max_completion_tokens: 1024 }),
  }
);
const data = await response.json();
const selectedModel = data.model; // e.g., "gpt-5-nano-2025-08-07"
```

This simple deployment naming allows backend cost tracking and analysis.

## Benchmark Results and Analysis

The authors ran all 10 sample prompts in both Balanced mode and a static deployment:

| Metric                   | Router (Balanced) | Static (GPT-5-nano) |
|--------------------------|------------------|---------------------|
| Avg Latency              | ~7,800 ms        | ~7,700 ms           |
| Total Cost (10 prompts)  | ~{{CONTENT}}.029          | ~{{CONTENT}}.030             |
| Cost Savings             | ~4.5%            | —                   |
| Models Used              | 4                | 1                   |

**Model Router Distribution:**

- gpt-5-nano: 50%
- gpt-5-mini: 20%
- gpt-oss-120b: 20%
- gpt-4.1-mini: 10%

**Mode Comparison:**

- **Balanced:** Best cost/quality mix
- **Cost-Optimised:** Maximises use of cheap models—4.7% savings
- **Quality-Optimised:** Highest accuracy, even greater (~14.2%) cost savings due to smarter routing

**Key Insights:**

- The router smartly chooses models per prompt type, e.g., nano for classification, mini for FAQ, OSS for long-context, and 4.1-mini for complex reasoning.
- Latency is usually comparable; quality mode can even be faster for some workloads.
- Cost savings grow with more workload complexity/diversity.
- Application code remains simple, with all routing logic handled via Azure infrastructure.

## Custom Prompt Testing Workflow

You can benchmark your actual prompts:

1. Select 'Custom' in the app
2. Enter your prompt
3. Run both Model Router and a fixed model baseline
4. Compare model choice, latency, token use, and cost
5. Adjust routing mode and re-benchmark as needed

## When to Use Model Router

**Best for:**

- Mixed-complexity workloads (chatbots, content processing, etc.)
- Cost-sensitive production use
- Teams seeking simplified architecture
- Rapid experimentation without code changes

**Use caution if:**

- Sub-second latency is critical
- One model is clearly optimal for all requests
- You need exact, deterministic model selection per request

## Best Practices

1. Start with Balanced mode and gather real results.
2. Test with your production prompts using the demo's custom input.
3. Continuously monitor model distribution and cost trends.
4. Keep a baseline static deployment for comparison.
5. Re-evaluate as new models appear in the pool.

## Technical Stack and Security

- **React 19 + TypeScript 5.9** for UI and type safety
- **Vite 7, Tailwind CSS 4** for build and styling
- **Recharts 3** for analytics
- **Azure OpenAI API (2024-10-21)** for chat completion and routing

Security features include:

- React `ErrorBoundary` for crash resilience
- Sanitised API error messages
- `AbortController` request timeouts
- Length validation and secure API credential handling

**Note:** The demo calls Azure OpenAI directly from the browser—use a backend proxy with [Managed Identity](https://learn.microsoft.com/azure/ai-services/openai/how-to/managed-identity) in production.

## Setup Quick Start

```sh
git clone https://github.com/leestott/router-demo-app/
cd router-demo-app

# Windows setup

echo ".\setup.ps1 -StartDev"

# macOS/Linux setup

chmod +x setup.sh && ./setup.sh --start-dev

# Manual option

npm install
cp .env.example .env.local

# Edit .env.local with your Azure credentials

npm run dev
```

Open [http://localhost:5173](http://localhost:5173), select a prompt, and click Run Both.

## Resources

- [Sample Demo App](https://github.com/leestott/router-demo-app/)
- [Model Router Concepts](https://learn.microsoft.com/azure/ai-foundry/openai/concepts/model-router)
- [How-To Guide](https://learn.microsoft.com/azure/ai-foundry/openai/how-to/model-router)
- [Microsoft Foundry Portal](https://ai.azure.com)
- [Managed Identity Guidance](https://learn.microsoft.com/azure/ai-services/openai/how-to/managed-identity)

## Conclusion

By intelligently routing AI requests to the right underlying model, the Model Router delivers measurable cost savings, operational simplicity, and technical flexibility to Azure AI users. As AI workloads diversify, solutions like this will be key to balancing efficiency, accuracy, and maintainability.

---

*Author: Lee_Stott*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/optimising-ai-costs-with-microsoft-foundry-model-router/ba-p/4494776)
