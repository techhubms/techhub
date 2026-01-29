---
external_url: https://devopsjournal.io/blog/2024/09/29/GitHub-Models-API
title: Configuring Azure Inference AI SDK for Azure OpenAI Endpoints in Python
author: Rob Bos
feed_name: Rob Bos' Blog
date: 2024-09-29 00:00:00 +00:00
tags:
- API Integration
- Authentication
- Azure Inference AI SDK
- Azure OpenAI
- AzureKeyCredential
- ChatCompletionsClient
- Cloud Development
- Environment Variables
- GitHub Models
- GPT 4o
- Python
- AI
- Azure
- Coding
- Blogs
section_names:
- ai
- azure
- coding
primary_section: ai
---
Rob Bos provides a helpful guide on using the Azure Inference AI SDK in Python to connect to Azure OpenAI endpoints, including code examples for credential configuration.<!--excerpt_end-->

# Configuring the Azure Inference AI SDK for Azure OpenAI Endpoints in Python

**Author:** Rob Bos  
**Date posted:** 29 Sep 2024 (Estimated 1 min read)

If you need to use the Azure Inference AI SDK in Python to connect specifically to an Azure OpenAI endpoint (rather than the default GitHub Models endpoint), this tip explains how to correctly configure authentication. The process involves setting the right credentials depending on which endpoint you want to target.

## Problem

The default examples for the Azure Inference AI SDK often demonstrate usage against GitHub’s Models endpoint. If you want to connect to a custom Azure OpenAI endpoint, you need to adjust the way credentials are handled in your code.

## Solution: Credential Configuration

Below is the relevant Python code, showcasing how to switch between running on GitHub’s endpoint and your own Azure OpenAI endpoint. The difference lies in how you configure your credentials and which environment variables you use for authentication.

```python
import os
from azure.ai.inference import ChatCompletionsClient
from azure.ai.inference.models import SystemMessage, UserMessage
from azure.core.credentials import AzureKeyCredential

# Set the runtime -- use "GITHUB" for GitHub endpoint, else for Azure

runtime = "AZURE"
client = None

if runtime == "GITHUB":
    print("Running in GitHub")
    token = os.environ["GITHUB_TOKEN"]
    ENDPOINT = "https://models.inference.ai.azure.com"
    client = ChatCompletionsClient(
        endpoint=ENDPOINT,
        credential=AzureKeyCredential(token)
    )
else:
    print("Running in Azure")
    token = os.environ["AI_TOKEN"]
    ENDPOINT = "https://xms-openai.openai.azure.com/openai/deployments/gpt-4o"
    client = ChatCompletionsClient(
        endpoint=ENDPOINT,
        credential=AzureKeyCredential("") ,  # Pass an empty value here!
        headers={"api-key": token}            # Use your Azure OpenAI key
        # api_version="2024-06-01"           # API version optional
    )
```

### Key Points

- When targeting Azure OpenAI, use your Azure API key from environment variables and include it in the headers. Pass an empty value to `AzureKeyCredential`.
- For GitHub’s Models endpoint, the credential is passed directly.
- Adjust the endpoint URL based on where you are running the code.

This setup enables smooth switching between endpoints without code duplication.

## References

- [Azure Inference AI SDK Documentation](https://learn.microsoft.com/en-us/azure/?product=azure-inference)
- [Azure OpenAI Documentation](https://learn.microsoft.com/en-us/azure/ai-services/openai/)

---

*Post originally by Rob Bos. [Original post and code](https://devopsjournal.io/blog/2024/09/29/GitHub-Models-API).*

This post appeared first on "Rob Bos' Blog". [Read the entire article here](https://devopsjournal.io/blog/2024/09/29/GitHub-Models-API)
