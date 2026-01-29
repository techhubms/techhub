---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploy-langchain-applications-to-azure-app-service/ba-p/4440640
title: Deploying LangChain Applications with Azure OpenAI on Azure App Service
author: TulikaC
feed_name: Microsoft Tech Community
date: 2025-08-06 11:42:32 +00:00
tags:
- AI Application Deployment
- App Service
- Authentication
- Azd
- Azure AI Foundry
- Azure Developer CLI
- Azure OpenAI Service
- Deployment Automation
- FastAPI
- GPT 4o
- LangChain
- Managed Identity
- Prompt Engineering
- Python
- Streaming Response
- Summarization
- Token Management
- AI
- Azure
- Coding
- Community
section_names:
- ai
- azure
- coding
primary_section: ai
---
TulikaC guides readers through deploying a LangChain-powered conversational AI app on Azure App Service, highlighting secure authentication, streaming GPT-4o responses, and rapid deployment techniques.<!--excerpt_end-->

# Deploying LangChain Applications with Azure OpenAI on Azure App Service

Author: TulikaC

## Overview

This post details how to create and deploy a robust conversational AI web application leveraging LangChain, FastAPI, and Azure OpenAI Foundry models on Azure App Service. The guide walks through key features like secure authentication, real-time streaming responses, intelligent summarization, and app deployment using the Azure Developer CLI (azd).

## Key Features of the Sample Application

- **Real-time streaming responses:** Leverages Azure OpenAI GPT-4o for instant user feedback.
- **Automatic response summarization:** Uses LangChain's summarize chain to generate concise answers.
- **Secure authentication:** Employs Azure Managed Identity, avoiding API key exposure.
- **Modern UI:** FastAPI backend with a responsive front-end chat interface.
- **Easy deployment:** Automated with Azure Developer CLI.

## Technical Walkthrough

### 1. **Secure Azure OpenAI Integration Using Managed Identity**

- Uses `DefaultAzureCredential` from `azure.identity` to request tokens at runtime.
- Removes the risk of API key leaks and supports Azure RBAC and compliance.

```python
from azure.identity import DefaultAzureCredential
credential = DefaultAzureCredential()
token = credential.get_token("https://cognitiveservices.azure.com/.default")

llm_long = AzureChatOpenAI(
    azure_endpoint=endpoint,
    openai_api_version="2025-01-01-preview",
    deployment_name=deployment,
    temperature=0.5,
    streaming=True,
    max_tokens=600,
    azure_ad_token=token.token)
```

### 2. **Intelligent Chaining with LangChain**

Configure two LLM instances:

- One for detailed (long) responses.
- One for concise summaries (temperature=0 for deterministic output).

```python
llm_summary = AzureChatOpenAI(temperature=0, max_tokens=200)
summarize_chain = load_summarize_chain(llm_summary, chain_type="stuff")
```

### 3. **Real-Time Streaming Responses**

- Implements async streaming to return tokens as they are generated, mimicking the ChatGPT user experience.
- After full answer streaming, triggers the LangChain summarization and streams the summary.

```python
async def streamer():
    long_answer = ""
    for chunk in llm_long.stream(messages):
        long_answer += chunk.content
        yield chunk.content
        await asyncio.sleep(0)
    docs = [Document(page_content=long_answer)]
    summary = await loop.run_in_executor(None, summarize_chain.run, docs)
    yield "__SUMMARY__" + summary

return StreamingResponse(streamer(), media_type="text/plain")
```

### 4. **Token and Temperature Management**

- Carefully manage tokens to avoid hitting Azure OpenAI limits and control costs.
- Lower temperatures yield consistent outputs (used for summaries), while higher temperatures generate more creative language.
- Configuration values are adjustable for both quotas and user experience.

### 5. **Automated Deployment Using Azure Developer CLI (azd)**

#### Prerequisites

- Azure Developer CLI (`azd`)
- Azure Subscription with OpenAI and App Service
- Python 3.10 or higher

#### Deployment Steps

```sh
git clone https://github.com/Azure-Samples/appservice-ai-samples.git
cd langchain-fastapi-chat
azd init
azd up
```

- `azd up` provisions resources, deploys the FastAPI app, configures managed identity and role assignments, and wires up environment variables for you.

### 6. **Customization Tips**

- **Switch AI models:** Change the `aiFoundryModelName` parameter in the ARM/Bicep template.
- **Adjust token limits:** Set `max_tokens` in your app's configuration.
- **Use API keys (optional):** Can fall back to traditional authentication if desired.

### 7. **Potential Extensions**

- Add conversation memory for richer chat sessions.
- Enable document upload and analysis.
- Support multiple AI models.
- Add app user personalization.
- Apply advanced prompt engineering techniques.

## Resources and Links

- [Sample Application on GitHub](https://github.com/Azure-Samples/appservice-ai-samples/tree/main/langchain-fastapi-chat)
- [Azure App Service AI Integration Docs](https://learn.microsoft.com/en-us/azure/app-service/overview-ai-integration)

Ready to get started? Clone the sample repository and run `azd up` to launch your own AI chat solution on Azure App Service in minutes.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploy-langchain-applications-to-azure-app-service/ba-p/4440640)
