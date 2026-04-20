---
tags:
- Agentic Workflows
- AI
- API Version 04 01 Preview
- Azure
- Azure AI Foundry
- Azure OpenAI
- Deterministic Graders
- Evaluation
- Fine Tuning
- Function Calling
- Global Training
- GPT 4.1
- GPT 4.1 Mini
- GPT 4.1 Nano
- MCP
- Microsoft Foundry
- Model Graders
- News
- O4 Mini
- Reinforcement Fine Tuning
- REST API
- Reward Hacking
- RFT
- String Match Grading
external_url: https://devblogs.microsoft.com/foundry/whats-new-in-foundry-finetune-april-2026/
feed_name: Microsoft AI Foundry Blog
title: What’s New in Microsoft Foundry Fine-Tuning | April 2026
date: 2026-04-16 19:34:46 +00:00
section_names:
- ai
- azure
author: Blanca Li
primary_section: ai
---

Blanca Li summarizes April 2026 updates to Microsoft Foundry Reinforcement Fine-Tuning (RFT): global training for o4-mini across 13+ Azure regions, new GPT-4.1 family model graders, and practical best practices plus pitfalls to help teams design reliable graders and scale fine-tuning safely.<!--excerpt_end-->

# What’s New in Microsoft Foundry Fine-Tuning | April 2026

This month Microsoft Foundry ships three updates aimed at making Reinforcement Fine-Tuning (RFT) easier to use and easier to get right:

1. **Global Training for o4-mini** — train from 13+ Azure regions at lower per-token rates.
2. **New model graders** — **GPT-4.1**, **GPT-4.1-mini**, and **GPT-4.1-nano** can now be used as graders.
3. **RFT best practices** — guidance on grader design, data prep, and avoiding common pitfalls.

## Global Training for o4-mini

Global Training expands model customization while keeping pricing aligned with other Global offerings.

What changes:

- **Train from anywhere** — start o4-mini fine-tuning jobs from **13** Azure regions now (with expansion planned).
- **Save on training costs** — **lower per-token** training rates vs Standard training.
- **Same quality, broader reach** — identical infrastructure and model quality regardless of the region you start from.

**Currently available regions:** East US 2, North Central US, West US 3, Australia East, France Central, Germany West Central, Switzerland North, Norway East, Poland Central, Spain Central, Italy North, Switzerland West, Sweden Central.

o4-mini is positioned as a popular choice for **reasoning-intensive** and **agentic** workloads; Global Training makes it more cost-effective for geographically distributed teams.

![o4-mini Global training](https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2026/04/ft-o4mini-global-training.webp)

### Create an o4-mini Global Training job via REST API

```bash
curl -X POST "https://<your-resource>.openai.azure.com/openai/fine_tuning/jobs?api-version=2025-04-01-preview" \
  -H "Content-Type: application/json" \
  -H "api-key: $AZURE_OPENAI_API_KEY" \
  -d '{
    "model": "o4-mini",
    "training_file": "<your-training-file-id>",
    "method": {
      "type": "reinforcement",
      "reinforcement": {
        "grader": {
          "type": "string_check",
          "name": "answer-check",
          "input": "{{sample.output_text}}",
          "reference": "{{item.reference_answer}}",
          "operation": "eq"
        }
      }
    },
    "hyperparameters": {
      "n_epochs": 2,
      "compute_multiplier": 1.0
    },
    "trainingType": "globalstandard"
  }'
```

Learn more: https://learn.microsoft.com/azure/foundry/openai/how-to/fine-tuning?tabs=oai-sdk&pivots=programming-language-studio

## New model graders: GPT-4.1, GPT-4.1-mini, and GPT-4.1-nano

Graders define the **reward signal** in RFT. This update adds three models that can be used for **model-based grading**:

- **GPT-4.1**
- **GPT-4.1-mini**
- **GPT-4.1-nano**

### When to use model graders

Use deterministic graders (string-match, Python, endpoint-based) by default because they’re **faster, cheaper, and more reproducible**. Model graders are useful when:

- Outputs are **open-ended or subjective** (summarization quality, tone adherence, reasoning coherence).
- You need **partial credit across dimensions** (factual accuracy, completeness, safety) in one pass.
- You’re building an **agentic workflow** where tool-call correctness depends on semantic context.

### Choosing the right model grader

- Start with **GPT-4.1-nano** for cheap iteration and faster feedback.
- Move to **GPT-4.1-mini** once the rubric is stable and you need higher fidelity.
- Use **GPT-4.1** for production grading or complex rubrics.

> **Tip:** You can mix grader types in a single RFT job (e.g., string-match for correctness plus a GPT-4.1-mini grader for reasoning quality).

## Reinforcement Fine-Tuning best practices

### When to use RFT

RFT helps most when outputs can be **clearly evaluated and scored**, especially for:

- **Tool-calling accuracy** (right tool, right parameters)
- **Policy or rubric enforcement** (business rules the grader can validate)
- **Structured data extraction** (unambiguous correctness)

> **Not a fit for style or tone.** Prefer prompt engineering, structured outputs, or supervised fine-tuning (SFT) for formatting/voice/stylistic changes.

### Step 1: Define the objective

Clearly define success, then design a grader that tracks task quality. The grader is the main driver of RFT success.

### Step 2: Establish a baseline

Run baseline evaluation on **10–100 samples** to measure improvement. Try to improve performance with prompts before fine-tuning.

Foundry Evaluation: https://learn.microsoft.com/en-us/azure/foundry/how-to/evaluate-generative-ai-app

### Step 3: Design effective graders

- Use the **simplest grader that works** (string-check when exact match is possible).
- Prefer **deterministic checks** (string/code/endpoint) over model grading for reliability.
- Aim for **well-distributed rewards** (avoid rewards that are too sparse or too uniform).
- Validate graders on **diverse, real-world inputs**, not only synthetic data.

### Step 4: Start small and iterate

A practical workflow:

1. Start with **o4-mini RFT** to validate setup and grader behavior.
2. Move to larger models after the reward signal looks healthy.
3. Change **one variable at a time** to attribute improvements/regressions.

### Step 5: Tune training parameters

The post highlights **epoch count** and **compute_multiplier** as high-impact knobs. Adjust one at a time and monitor reward trends/variance.

## RFT data format

RFT uses a different schema than SFT.

Key requirement: the final message in each row must be a **User** or **Developer** role (not Assistant).

**SFT format** (answer in the assistant message):

```json
{
  "messages": [
    {
      "role": "system",
      "content": "Reply to the user's question as accurately as possible."
    },
    {
      "role": "user",
      "content": "Question: What is the capital of France?"
    },
    {
      "role": "assistant",
      "content": "Paris"
    }
  ]
}
```

**RFT format** (answer moved to a top-level key for the grader):

```json
{
  "messages": [
    {
      "role": "developer",
      "content": "Reply to the user's question as accurately as possible."
    },
    {
      "role": "user",
      "content": "Question: What is the capital of France?"
    }
  ],
  "reference_answer": "Paris"
}
```

The top-level key (for example, `reference_answer`) can be referenced by graders as `item.reference_answer`.

## Common pitfalls

### Data and grader mismatch

Every key referenced in the grader must exist in **all rows**.

Example mismatch (grader references `item.capital` but the data uses `reference_answer`):

```json
{
  "type": "string_check",
  "name": "answer-check",
  "input": "{{sample.output_text}}",
  "reference": "{{item.capital}}",
  "operation": "eq"
}
```

Fix: update the grader to reference `{{item.reference_answer}}`.

### Missing response format

If your grader references `sample.output_json`, you must supply a response format in the job definition. Otherwise, the model emits free-form text and JSON references will fail.

```json
{
  "type": "json_schema",
  "json_schema": {
    "name": "response",
    "strict": true,
    "schema": {
      "properties": {
        "capital": {
          "title": "Capital",
          "type": "string"
        },
        "population": {
          "title": "Population",
          "type": "string"
        }
      },
      "title": "CapitalData",
      "type": "object",
      "additionalProperties": false
    }
  }
}
```

## Advanced: agentic RFT scenarios

### Tool design

Treat tools as part of the environment. Design tools to represent the **full decision cycle** (e.g., check recipient availability before escalation). Plan for training-scale load: timeouts, rate limits, tracing, and retries.

### MCP server integration

While RFT supports tool use via function-calling, the guidance suggests **MCP** as preferred for production agentic systems.

Approach:

- Implement each tool once.
- Expose it via:
  - an **MCP interface** (for MCP-native clients)
  - a **function-calling-compatible interface** (for fine-tuning)

Samples: https://github.com/microsoft-foundry/fine-tuning/tree/main/Demos/Agentic_RFT_PrivatePreview

### Monitor for reward hacking

Use the Foundry fine-tuning job **Metrics tab** to inspect outputs and metrics during training.

Signs:

- Eval scores rise while visible output quality degrades.
- Outputs “match” the grader without intended behavior.

Mitigations:

- Use **held-out evaluation sets** with diverse real-world inputs.
- Grade multiple dimensions (outcome, tool use, safety) with partial credit.
- Require critical intermediate steps (lookups before writes).
- Keep grading deterministic to reduce grader noise.

## What’s next

- RFT Best Practices guide: https://github.com/microsoft-foundry/fine-tuning/blob/main/Demos/Agentic_RFT_PrivatePreview/RFT_Best_Practice.md
- Fine-tuning samples: https://github.com/microsoft-foundry/fine-tuning/Demos
- Reinforcement Fine-Tuning how-to: https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/reinforcement-fine-tuning
- Community: https://aka.ms/foundrydevs

## Conclusion

The April updates combine:

- **Global Training for o4-mini** to reduce cost across regions,
- **new GPT-4.1-family model graders** to improve reward design flexibility,
- and a **best practices guide** to help teams avoid common RFT pitfalls and scale safely.


[Read the entire article](https://devblogs.microsoft.com/foundry/whats-new-in-foundry-finetune-april-2026/)

