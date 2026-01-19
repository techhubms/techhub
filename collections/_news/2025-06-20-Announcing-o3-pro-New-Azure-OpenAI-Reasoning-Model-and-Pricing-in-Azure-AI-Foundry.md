---
external_url: https://devblogs.microsoft.com/foundry/azure-openai-o3-pro-ai-foundry/
title: 'Announcing o3-pro: New Azure OpenAI Reasoning Model and Pricing in Azure AI Foundry'
author: Sanjeev Jagtap, Anthony Mocny
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-06-20 16:06:00 +00:00
tags:
- API Versioning
- Azure AI Foundry
- Azure OpenAI
- Enterprise AI
- File Search
- Foundry
- Model Pricing
- Multimodal Input
- O3 Pro
- OpenAI Library
- Reasoning Model
- Responses API
- Responsible AI
- Structured Output
section_names:
- ai
- azure
---
Authored by Sanjeev Jagtap and Anthony Mocny, this detailed overview introduces o3-pro, Azure AI Foundry's new reasoning model, outlining its features, access process, and pricing.<!--excerpt_end-->

# o-series Updates: New o3 pricing and o3-pro in Azure AI Foundry

**Authors:** Sanjeev Jagtap, Anthony Mocny

## Overview

o3-pro, the newest model in the Azure AI Foundry via Azure OpenAI, is now available. This advanced reasoning model is designed to deliver deeper, more consistent answers for complex enterprise tasks, leveraging advanced compute capabilities, long context handling, and multimodal input support.

## Key Features

- **Highest reasoning performance:** Utilizes increased compute resources for improved, consistent results on sophisticated tasks.
- **Multimodal input:** Accepts both text and image inputs to broaden use case applicability.
- **Structured outputs:** Supports structured data generation, including function calling.
- **Tool integration:** Compatible with File Search tool, with additional tools to be supported in the future.
- **Enterprise-ready:** Supported by Microsoft’s enterprise compliance promises and Responsible AI best practices.

## Accessing o3-pro

Customers can [request access to o3-pro](https://aka.ms/oai/o3proaccess). The model is currently available in East US2 and Sweden Central Azure regions and is deployable in Global Standard environments. To leverage o3-pro and other latest features, use the new [v1 preview API](https://learn.microsoft.com/en-us/azure/ai-services/openai/api-version-lifecycle?tabs=key#next-generation-api).

## Getting Started

1. **Upgrade the OpenAI Library:**

   ```bash
   pip install --upgrade openai
   ```

2. **Submit a Test Prompt via REST API:**

   ```pwsh
   curl -X POST https://YOUR-RESOURCE-NAME.openai.azure.com/openai/v1/responses?api-version=preview \
        -H "Content-Type: application/json" \
        -H "api-key: $AZURE_OPENAI_API_KEY" \
        -d '{ "model": "o3-pro", "input": "This is a test" }'
   ```

3. **Generate a Response Using Python SDK:**

   ```python
   from openai import AzureOpenAI
   from azure.identity import DefaultAzureCredential, get_bearer_token_provider

   token_provider = get_bearer_token_provider(
       DefaultAzureCredential(),
       "https://cognitiveservices.azure.com/.default"
   )

   client = AzureOpenAI(
       base_url = "https://YOUR-RESOURCE-NAME.openai.azure.com/openai/v1/",
       azure_ad_token_provider=token_provider,
       api_version="preview"
   )

   response = client.responses.create(
       model="o3-pro",
       input="This is a test"
   )

   print(response.output_text)
   ```

## Availability & Pricing

o3-pro is available via the Responses API starting June 19, 2025. The pricing is as follows:

| Model   | Input $/million tokens | Output $/million tokens |
|---------|-----------------------|------------------------|
| o3-pro  | $20                   | $80                    |
| o3*     | $2                    | $8                     |

*Starting June 1, 2025, Azure OpenAI adjusts the o3 model to $2/$8 per million input/output tokens, reflected in July invoices.*

## Why o3-pro?

o3-pro brings advanced reasoning and multimodal capacity to enterprise AI, enabling organizations to address their most complex challenges confidently, at scale. It’s part of the expanding Foundry model lineup in Azure OpenAI Service, supporting intelligent application development with enterprise compliance.

## Next Steps & Resources

- [Azure OpenAI Overview](https://learn.microsoft.com/en-us/azure/ai-services/openai/overview)
- [Responses API QuickStart](https://learn.microsoft.com/en-us/azure/ai-services/openai/how-to/responses?tabs=python-secure)
- [Using Reasoning Models in Responses API](https://learn.microsoft.com/en-us/azure/ai-services/openai/how-to/reasoning?tabs=python-secure%2Cpy)
- [Azure AI Foundry Developer Forum](https://aka.ms/azureaifoundry/forum)
- [Sign up for o3-pro on Azure AI Foundry](https://ai.azure.com/catalog/models/o3-pro)

## Conclusion

o3-pro offers deeper reasoning, multimodal support, structured outputs, and expanded integration options—empowering enterprises to build sophisticated AI-driven solutions. Interested customers are encouraged to request access and begin exploring o3-pro in Azure AI Foundry.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/foundry/azure-openai-o3-pro-ai-foundry/)
