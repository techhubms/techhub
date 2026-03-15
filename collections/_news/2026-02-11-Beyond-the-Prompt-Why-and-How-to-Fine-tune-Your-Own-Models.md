---
external_url: https://devblogs.microsoft.com/foundry/beyond-the-prompt-why-and-how-to-fine-tune-your-own-models/
title: Beyond the Prompt – Why and How to Fine-tune Your Own Models
author: Radhika Bollineni
primary_section: ai
feed_name: Microsoft AI Foundry Blog
date: 2026-02-11 17:29:15 +00:00
tags:
- AI
- AI Compliance
- AI Deployment
- Azure
- Azure AI
- Azure AI Projects
- Domain Adaptation
- Fine Tuning
- GPT 4
- LLM
- Microsoft Foundry
- ML
- Model Training
- News
- OpenAI
- Prompt Engineering
- PubMed Dataset
- Python SDK
- Reinforcement Learning
- Retrieval Augmented Generation
- SFT
- Supervised Learning
section_names:
- ai
- azure
- ml
---
Radhika Bollineni discusses fine-tuning large language models using Microsoft Foundry. The article covers methods, code examples, and practical tips for training enterprise-ready, reliable AI using Azure's tools and infrastructure.<!--excerpt_end-->

# Beyond the Prompt – Why and How to Fine-tune Your Own Models

*By Radhika Bollineni*

Large Language Models (LLMs) are increasingly effective, but ensuring their reliable and policy-compliant behavior at scale is a challenge for enterprise AI. Prompt engineering and Retrieval-Augmented Generation (RAG) help, but do not fundamentally change core model behavior. Fine-tuning addresses this by adapting a pre-trained AI model with additional, domain-specific training for improved accuracy, compliance, and alignment with business needs.

## What is Microsoft Foundry Fine-Tuning?

[Microsoft Foundry fine-tuning](https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/fine-tuning-overview?view=foundry-classic) enables users to customize pre-trained foundation models (OpenAI as well as open models) using task-specific datasets. This results in specialized models that behave predictably for your use case, while maintaining Azure's enterprise-grade security, governance, and observability.

## Key Benefits and Top Use Cases for Fine-tuning

- **Domain Specialization**: Adapt models for medicine, finance, law, or other technical domains to improve understanding and relevance.
- **Task Performance**: Optimize for sentiment analysis, code generation, translation, or summarization for higher performance.
- **Style and Tone Adaptation**: Ensure outputs match desired communication style or formatting requirements.
- **Instruction Following**: Improve adherence to structured instructions, formatting, or complex workflows.
- **Compliance and Safety**: Train models to follow regulatory, policy, or safety requirements.
- **Language or Culture Customization**: Adapt to specific languages, dialects, or cultural contexts, avoiding high costs of training from scratch.

## Supported Fine-tuning Methods

- **Supervised Fine-Tuning (SFT)**
- **Direct Preference Optimization**
- **Reinforcement Fine-Tuning**

### Supervised Fine-Tuning (SFT)

The model is trained on task-specific input-output pairs to deliver accurate and consistent responses for, e.g., summarization, code generation, question answering, or knowledge extraction.

**Best Use Cases for SFT:**

1. Text classification & labeling
2. Question answering & knowledge extraction
3. Text summarization
4. Code generation & analysis
5. Structured output & formatting
6. Domain-specific language/style adaptation
7. Multi-agent or tool-calling workflows

**How SFT Works:**
You provide example input/output pairs; the model learns the mapping. This "learn by example" approach lets the base model inherit new task-specific skills while maintaining foundational knowledge.

### Example: Supervised Fine-tuning with Microsoft Foundry SDK

```python
import os
from dotenv import load_dotenv
from azure.identity import DefaultAzureCredential
from azure.ai.projects import AIProjectClient

load_dotenv()
endpoint = os.environ.get("AZURE_AI_PROJECT_ENDPOINT")
model_name = os.environ.get("MODEL_NAME")

# Define dataset file paths

training_file_path = "training.jsonl"
validation_file_path = "validation.jsonl"
credential = DefaultAzureCredential()
project_client = AIProjectClient(endpoint=endpoint, credential=credential)
openai_client = project_client.get_openai_client()

with open(validation_file_path, "rb") as f:
    validation_file = openai_client.files.create(file=f, purpose="fine-tune")
openai_client.files.wait_for_processing(validation_file.id)

with open(training_file_path, "rb") as f:
    train_file = openai_client.files.create(file=f, purpose="fine-tune")
openai_client.files.wait_for_processing(train_file.id)

fine_tune_job = openai_client.fine_tuning.jobs.create(
    model=model_name,
    training_file=train_file.id,
    validation_file=validation_file.id,
    method={
        "type": "supervised",
        "supervised": {"hyperparameters": {"n_epochs": 3, "batch_size": 1, "learning_rate_multiplier": 1.0}},
    },
    suffix="pubmed-summarization"
)
```

### Data Format Example

Each example should be in JSONL format. Minimum 10 examples are recommended.

```json
{
  "messages": [
    { "role": "system", "content": "You are a medical research summarization assistant. Create concise, accurate abstracts of medical research articles that capture the key findings and methodology." },
    { "role": "user", "content": "Summarize this medical research article:\n\n[full article text]" },
    { "role": "assistant", "content": "[generated abstract]" }
  ]
}
```

### Cookbook Example

[Fine-tuning/Demos/SFT_PubMed_Summarization at microsoft-foundry/fine-tuning (GitHub)](https://github.com/microsoft-foundry/fine-tuning/tree/main/Demos/SFT_PubMed_Summarization) demonstrates fine-tuning with the PubMed Summarization dataset on Azure AI. After running the cookbook, monitor job progress in the Foundry Portal.

## How to View and Deploy Fine-tuning Jobs

Navigate to [Microsoft Foundry Portal](https://ai.azure.com) and access the fine-tuning section to view job status and outputs. You can optionally deploy your fine-tuned model to a serverless endpoint for inference.

## Results

| Metric           | Base Model | Fine-Tuned Model |
|------------------|------------|------------------|
| Task Accuracy    | 70–80%     | 88–95%           |
| Prompt Length    | 800–1200   | 200–400 tokens   |
| Inference Cost   | Baseline   | 0.5–0.7x         |

## Key Highlights

1. GPT-4.1 was used as the foundational model in the sample scenario.
2. The sample uses the PubMed Article Summarization dataset from Kaggle.
3. Requirements: Azure subscription, Microsoft Foundry project, AI User role, and dataset in JSONL format.
4. After set up, configure hyperparameters and launch job in Foundry.
5. Deploy the finished model for production use and perform inferences directly.

## References

- [Microsoft Foundry Fine-Tuning Documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/fine-tuning-overview?view=foundry-classic)
- [Foundry Portal](https://ai.azure.com)
- [Sample Cookbook (GitHub)](https://github.com/microsoft-foundry/fine-tuning/tree/main/Demos/SFT_PubMed_Summarization)
- [PubMed Summarization Dataset (Kaggle)](https://www.kaggle.com/datasets/thedevastator/pubmed-article-summarization-dataset)

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/beyond-the-prompt-why-and-how-to-fine-tune-your-own-models/)
