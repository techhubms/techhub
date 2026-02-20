---
external_url: https://devblogs.microsoft.com/foundry/dpo-fine-tuning-using-microsoft-foundry-sdk/
title: DPO Fine-Tuning Using Microsoft Foundry SDK
author: Jayesh Tanna
primary_section: ai
feed_name: Microsoft AI Foundry Blog
date: 2026-02-13 23:13:44 +00:00
tags:
- AI
- AI Development
- AI Evaluation
- AI Safety
- Azure AI Projects
- Content Moderation
- Direct Preference Optimization
- Fine Tuning
- Foundry SDK
- GPT 4
- Hyperparameters
- LLMs
- Microsoft Foundry
- ML
- Model Alignment
- News
- Preference Pairs
- Python
- RLHF Alternatives
section_names:
- ai
- ml
---
Jayesh Tanna presents a comprehensive walkthrough of how to apply Direct Preference Optimization (DPO) for large language models using the Microsoft Foundry SDK. The article covers practical Python examples, data formats, and ideal scenarios for developers.<!--excerpt_end-->

# DPO Fine-Tuning Using Microsoft Foundry SDK

## Introduction

Direct Preference Optimization (DPO) offers a streamlined approach to aligning large language model outputs with human preferences. This guide overviews what DPO is, why it matters, and provides code examples for developers using the Microsoft Foundry SDK to fine-tune LLMs such as GPT-4.

## What is Direct Preference Optimization?

- **DPO** is a training method for LLMs that uses comparative examples instead of explicit rewards.
- Move beyond traditional RLHF by providing preferred and non-preferred responses to guide model behavior.
- Helps improve output quality, safety, helpfulness, and alignment with desired style or tone.

### Best Use Cases for DPO

- Enhancing response accuracy and helpfulness
- Reducing harmful or unsafe outputs
- Enforcing specific tone or brand voice
- Optimizing for various user preferences

## How DPO Works

1. **Collect data pairs**: For each prompt, supply a preferred (high-quality) and non-preferred (lower-quality) output.
2. **Model updates**: The model is trained to prefer outputs marked as better during fine-tuning.

### Example Dataset Format

```json
{
  "input": {
    "messages": [
      {"role": "system", "content": "You are a helpful assistant."},
      {"role": "user", "content": "What is the capital of France?"}
    ]
  },
  "preferred_output": [
    {"role": "assistant", "content": "The capital of France is Paris."}
  ],
  "non_preferred_output": [
    {"role": "assistant", "content": "I think it's London."}
  ]
}
```

## Fine-Tuning with Microsoft Foundry SDK

- The Microsoft Foundry SDK streamlines DPO setup and training using Python.

### Example Code Snippet

```python
import os
from dotenv import load_dotenv
from azure.identity import DefaultAzureCredential
from azure.ai.projects import AIProjectClient

# Load environment variables

load_dotenv()
endpoint = os.environ.get('AZURE_AI_PROJECT_ENDPOINT')
model_name = os.environ.get('MODEL_NAME')

# Paths to datasets

training_file_path = 'training.jsonl'
validation_file_path = 'validation.jsonl'

credential = DefaultAzureCredential()
project_client = AIProjectClient(endpoint=endpoint, credential=credential)
openai_client = project_client.get_openai_client()

# Upload training files

with open(training_file_path, 'rb') as f:
    train_file = openai_client.files.create(file=f, purpose='fine-tune')
with open(validation_file_path, 'rb') as f:
    validation_file = openai_client.files.create(file=f, purpose='fine-tune')

openai_client.files.wait_for_processing(train_file.id)
openai_client.files.wait_for_processing(validation_file.id)

# Create DPO fine-tuning job

fine_tuning_job = openai_client.fine_tuning.jobs.create(
    training_file=train_file.id,
    validation_file=validation_file.id,
    model=model_name,
    method={
        "type": "dpo",
        "dpo": {
            "hyperparameters": {
                "n_epochs": 3,
                "batch_size": 1,
                "learning_rate_multiplier": 1.0
            }
        }
    },
    extra_body={"trainingType": "GlobalStandard"}
)
```

### Testing the Fine-Tuned Model

```python
print(f"Testing fine-tuned model via deployment: {deployment_name}")
response = openai_client.responses.create(
    model=deployment_name,
    input=[{"role": "user", "content": "Explain machine learning in simple terms."}]
)
print(f"Model response: {response.output_text}")
```

#### Example Output

> Machine learning is like teaching a computer to learn from experience... For example, if you show a machine learning system lots of pictures of cats and dogs, it will learn to recognize which is which by itself.

## DPO vs Other Methods

| Aspect | DPO | SFT | RFT |
| --- | --- | --- | --- |
| Learning signal | Comparative preferences | Input-output pairs | Graded exploration |
| Data requirement | Preference pairs | Example demonstrations | Problems + grader |
| Best for | Quality alignment | Task learning | Complex reasoning |
| Computational cost | Moderate | Low | High |

## Additional Resources

- [DPO Fine-Tuning Using Microsoft Foundry SDK (Blog)](https://devblogs.microsoft.com/foundry/dpo-fine-tuning-using-microsoft-foundry-sdk/)
- [Microsoft Foundry Projects SDK](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/develop/sdk-overview?view=foundry)
- [DPO Paper](https://arxiv.org/abs/2305.18290)
- [Model Mondays | Microsoft Reactor](https://developer.microsoft.com/en-us/reactor/series/S-1485/)
- [Fine-tune models with Microsoft Foundry – Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/fine-tuning-overview?view=foundry-classic)

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/dpo-fine-tuning-using-microsoft-foundry-sdk/)
