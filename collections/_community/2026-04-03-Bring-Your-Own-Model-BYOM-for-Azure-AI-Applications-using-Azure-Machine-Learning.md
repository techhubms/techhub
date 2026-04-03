---
feed_name: Microsoft Tech Community
title: Bring Your Own Model (BYOM) for Azure AI Applications using Azure Machine Learning
author: vaibhavpandey
primary_section: ai
section_names:
- ai
- azure
- ml
- security
tags:
- Agent Orchestration
- AI
- Azure
- Azure Machine Learning
- Azure Machine Learning Inference Server HTTP
- Azure ML Managed Online Endpoints
- Azure ML Studio
- Azure ML Workspace
- Azure.ai.ml
- Bring Your Own Model
- BYOM
- Community
- Conda Environment
- DefaultAzureCredential
- Hugging Face Transformers
- Inference Environment
- Managed Identity
- Microsoft Entra ID
- Microsoft Foundry
- ML
- MLClient
- Model Registry
- Model Versioning
- OpenAPI
- Private Endpoints
- Python SDK V2
- PyTorch
- RBAC
- REST API Inference
- Security
- SmolLM 135M
- Text Generation
- VNET Isolation
- Zero Trust
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bring-your-own-model-byom-for-azure-ai-applications-using-azure/ba-p/4508211
date: 2026-04-03 02:00:40 +00:00
---

Vaibhav Pandey shares a production-oriented “Bring Your Own Model” (BYOM) pattern for Azure AI applications, showing how to package, register, and deploy a custom model on Azure Machine Learning with secure identity, networking, and scalable managed endpoints.<!--excerpt_end-->

# Bring Your Own Model (BYOM) for Azure AI Applications using Azure Machine Learning

Modern AI-powered applications on Azure often need flexibility beyond managed model catalogs, for example to:

- Host open-source or fine-tuned models
- Deploy domain-specific or regulated models inside a tenant boundary
- Control runtime environments and versions
- Integrate inference into existing application architectures

**Bring Your Own Model (BYOM)** is positioned here as an architectural capability: **apps orchestrate, Azure ML executes the model**.

## Reference architecture

| Azure layer | Responsibility |
| --- | --- |
| Azure Application layer | API, app logic, orchestration, agent logic |
| Azure Machine Learning | Model registration, environments, scalable inference |
| Azure Identity & Networking | Authentication, RBAC, private endpoints |

**Key principle:** Applications orchestrate. Azure ML executes the model.

## BYOM workflow overview

1. Provision Azure Machine Learning
2. Create Azure ML compute
3. Author code in an Azure ML notebook
4. Download and package the model
5. Register the model
6. Define a reproducible inference environment
7. Implement scoring logic
8. Deploy a managed online endpoint
9. Use the endpoint from Microsoft Foundry

## Step 1: Provision Azure Machine Learning

An Azure ML workspace acts as the governance boundary for BYOM:

- Model versioning and lineage
- Environment definitions
- Secure endpoint hosting
- Auditability

Also consider region choices for latency, data residency, and networking.

## Step 2: Create Azure ML compute (Compute Instance)

Create a **Compute Instance** in Azure ML Studio.

Why it matters:

- Managed Jupyter environment
- Identity integrated (no secrets in notebooks)
- Useful for model packaging and testing

Operational tips:

- Enable auto-shutdown for cost control
- CPU is sufficient for many development workflows

## Step 3: Create an Azure ML notebook

- Open **Azure ML Studio** → **Notebooks**
- Create a new Python notebook
- Select the **Python SDK v2** kernel

## Step 4: Connect to the Azure ML workspace (Entra ID auth)

```python
# Import Azure ML SDK client
from azure.ai.ml import MLClient

# Import identity library for secure authentication
from azure.identity import DefaultAzureCredential

# Define workspace details
subscription_id = "<SUBSCRIPTION_ID>"
resource_group = "<RESOURCE_GROUP>"
workspace_name = "<WORKSPACE_NAME>"

# Create MLClient using Microsoft Entra ID
# No keys or secrets are embedded in code
ml_client = MLClient(
    DefaultAzureCredential(),
    subscription_id,
    resource_group,
    workspace_name,
)
```

The post calls out this approach as aligned with **zero-trust** practices (no embedded keys/secrets).

## Step 5: Download and package model artifacts (example: SmolLM-135M)

```python
from transformers import AutoModelForCausalLM, AutoTokenizer
import os

# Hugging Face model identifier
model_id = "HuggingFaceTB/SmolLM-135M"

# Local directory where model artifacts will be stored
model_dir = "smollm_135m"
os.makedirs(model_dir, exist_ok=True)

# Download model weights
model = AutoModelForCausalLM.from_pretrained(model_id)

# Download tokenizer
tokenizer = AutoTokenizer.from_pretrained(model_id)

# Save artifacts locally
model.save_pretrained(model_dir)
tokenizer.save_pretrained(model_dir)
```

Notes from the post:

- Open-source and proprietary models follow the same packaging pattern
- Azure ML treats all registered models identically

## Step 6: Register the model in Azure ML

Register packaged artifacts as a custom model asset to support:

- Version tracking
- Rolling upgrades
- CI/CD integration

```python
from azure.ai.ml.entities import Model

# Create a model asset in Azure ML
registered_model = Model(
    path=model_dir,
    name="SmolLM-135M",
    description="BYOM model for Microsoft Foundry extensibility",
    type="custom_model",
)

# Register (or update) the model
ml_client.models.create_or_update(registered_model)
```

## Step 7: Define a reproducible inference environment

The post emphasizes environment management as a major ongoing challenge and recommends treating environment changes like code changes.

```yaml
name: dev-hf-base
channels:
  - conda-forge
dependencies:
  - python=3.12
  - numpy=2.3.1
  - pip=25.1.1
  - scipy=1.16.1
  - pip:
      - azureml-inference-server-http==1.4.1
      - inference-schema[numpy-support]
      - accelerate==1.10.0
      - einops==0.8.1
      - torch==2.0.0
      - transformers==4.55.2
```

## BYOM inference patterns

The same model can expose multiple behaviors.

### Pattern 1: Text generation endpoint

Characteristics:

- REST-based text generation
- Stateless inference
- Horizontal scaling via Azure ML **managed endpoints**

Use cases mentioned:

- Copilots
- Chat APIs
- Summarization/content generation services

**Scoring script (score.py)**

```python
import os
import json
import torch
from transformers import AutoTokenizer, AutoModelForCausalLM


def init():
    """Called once when the container starts. Loads the model and tokenizer into memory."""
    global model, tokenizer

    # Azure ML injects model path at runtime
    model_dir = os.getenv("AZUREML_MODEL_DIR")

    tokenizer = AutoTokenizer.from_pretrained(model_dir)
    model = AutoModelForCausalLM.from_pretrained(model_dir)
    model.eval()


def run(raw_data):
    """Called for each inference request. Expects JSON input with a 'prompt' field."""
    data = json.loads(raw_data)
    prompt = data.get("prompt", "")

    # Tokenize input text
    inputs = tokenizer(prompt, return_tensors="pt")

    # Generate text without tracking gradients
    with torch.no_grad():
        outputs = model.generate(**inputs, max_new_tokens=100)

    # Decode output tokens into text
    response_text = tokenizer.decode(outputs[0], skip_special_tokens=True)

    return {"response": response_text}
```

Example request/response:

```json
{ "prompt": "Summarize the BYOM pattern in one sentence." }
```

```json
{
  "response": "Bring Your Own Model (BYOM) allows organizations to extend Microsoft Foundry with custom models hosted on Azure Machine Learning while maintaining enterprise governance and scalability."
}
```

### Pattern 2: Predictive / token rank analysis

Non-generative behaviors described:

- Token likelihood analysis
- Ranking/scoring
- Model introspection services

Example implementation:

```python
import torch
from transformers import AutoModelForCausalLM, AutoTokenizer


class PredictiveAnalysisModel:
    """Computes the rank of each token based on the model's next-token probability distribution."""

    def init(self, model, tokenizer):
        self.model = model
        self.tokenizer = tokenizer
        self.model.eval()

    def analyze(self, text):
        tokens = self.tokenizer.tokenize(text)
        token_ids = self.tokenizer.convert_tokens_to_ids(tokens)

        # Start with BOS token
        input_sequence = [self.tokenizer.bos_token_id, *token_ids]
        results = []

        for i in range(len(token_ids)):
            context = input_sequence[: i + 1]
            model_input = torch.tensor([context])

            with torch.no_grad():
                outputs = self.model(model_input)

            logits = outputs.logits[0, -1]
            sorted_indices = torch.argsort(logits, descending=True)

            actual_token = token_ids[i]
            rank = (sorted_indices == actual_token).nonzero(as_tuple=True)[0].item()

            results.append({"token": tokens[i], "rank": rank})

        return results

    @classmethod
    def from_disk(cls, model_path):
        model = AutoModelForCausalLM.from_pretrained(model_path)
        tokenizer = AutoTokenizer.from_pretrained(model_path)
        return cls(model, tokenizer)
```

And a corresponding scoring entrypoint:

```python
import os
from predictive_analysis import PredictiveAnalysisModel


def init():
    """Loads predictive analysis model from disk."""
    global model
    model_dir = os.getenv("AZUREML_MODEL_DIR")
    model = PredictiveAnalysisModel.from_disk(model_dir)


def run(text: str):
    """Accepts raw text input and returns token ranks."""
    return {"token_ranks": model.analyze(text)}
```

Example request/response:

```json
{ "text": "This is a test." }
```

```json
{
  "token_ranks": [
    { "token": "This", "rank": 518 },
    { "token": " is", "rank": 2 },
    { "token": " a", "rank": 0 },
    { "token": " test", "rank": 33 },
    { "token": ".", "rank": 77 }
  ]
}
```

## Consuming the BYOM endpoint from Azure applications

Azure ML endpoints are described as external inference services consumed by apps.

### Option A: Application-controlled invocation

Characteristics:

- App calls Azure ML endpoint directly
- IAM/networking/retries controlled by the app
- Recommended for most production systems

```python
import requests
import os

AML_ENDPOINT = os.environ["AML_ENDPOINT"]
AML_KEY = os.environ["AML_KEY"]

headers = {
    "Authorization": f"Bearer {AML_KEY}",
    "Content-Type": "application/json",
}

payload = {"prompt": "Summarize BYOM in one sentence."}

response = requests.post(AML_ENDPOINT, json=payload, headers=headers)
print(response.json())
```

### Option B: Tool-based invocation

- Expose the ML endpoint as an OpenAPI interface
- Allow orchestration layers (such as agents) to invoke it dynamically

The post notes these patterns integrate with **Azure App Service**, **Azure Container Apps**, **Azure Functions**, and Kubernetes-based apps.

## Operational considerations

- Dependency management is ongoing work
- Model upgrades require redeployment
- Private networking must be planned early
- Use managed Foundry models where possible
- Use BYOM when business or regulatory needs require it

## Security and governance by default

Security controls highlighted:

- Entra ID & managed identity
- RBAC-based permissions
- Private networking and VNET isolation
- Centralized logging and diagnostics

## When should you use BYOM?

BYOM is presented as a good fit when:

- You need model choice independence
- You want to deploy open-source or proprietary LLMs
- You require enterprise-grade controls
- You are building AI APIs, agents, or copilots at scale

For experimentation, higher-level tooling may be faster; for production, BYOM provides more control and durability.

## Conclusion

Azure ML serves as the execution layer for model hosting and inference, while Azure applications handle orchestration. The pattern aims to support mixing managed and custom models, enforce security/compliance, scale reliably, and reduce vendor lock-in.


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bring-your-own-model-byom-for-azure-ai-applications-using-azure/ba-p/4508211)

