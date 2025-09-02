---
layout: "post"
title: "Build an AI Image-Caption Generator on Azure App Service with Streamlit and GPT-4o-mini"
description: "This detailed guide walks through building a Python-based AI app that generates natural-language image captions using Azure AI Vision for image analysis and Azure OpenAI (GPT-4o-mini) for language generation. Deployment uses Azure App Service and Managed Identity for secure, cloud-native operations, with infrastructure as code options and extension ideas."
author: "TulikaC"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-an-ai-image-caption-generator-on-azure-app-service-with/ba-p/4450313"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-02 10:08:03 +00:00
permalink: "/2025-09-02-Build-an-AI-Image-Caption-Generator-on-Azure-App-Service-with-Streamlit-and-GPT-4o-mini.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "App Service Samples", "Azd", "Azure", "Azure AI Vision", "Azure App Service", "Azure OpenAI", "Bicep", "Cloud Deployment", "Coding", "Community", "Computer Vision", "GPT 4o Mini", "IaC", "Image Captioning", "Managed Identity", "Natural Language Generation", "Python", "RBAC", "REST API", "Streamlit"]
tags_normalized: ["ai", "app service samples", "azd", "azure", "azure ai vision", "azure app service", "azure openai", "bicep", "cloud deployment", "coding", "community", "computer vision", "gpt 4o mini", "iac", "image captioning", "managed identity", "natural language generation", "python", "rbac", "rest api", "streamlit"]
---

TulikaC walks through building a Python app with Azure AI Vision and GPT-4o-mini to generate image captions, deployed securely to Azure App Service using Streamlit for the UI.<!--excerpt_end-->

# Build an AI Image-Caption Generator on Azure App Service with Streamlit and GPT-4o-mini

This guide demonstrates how to create a cloud-native application that takes any image upload and instantly produces a natural one-line caption using a combination of Microsoft-powered AI services and Python technologies.

## Overview

- **Image upload:** User submits an image via Streamlit UI.
- **Azure AI Vision:** Extracts descriptive tags with confidence scores from the image.
- **Azure OpenAI (GPT-4o-mini):** Receives tags and generates a fluent image caption.
- **Streamlit:** Provides a simple, Python-based frontend perfect for fast iteration and sharing.

[Sample Code and Infra Templates](https://github.com/Azure-Samples/appservice-ai-samples/tree/main/image_caption_app)

## What are these components?

- **Streamlit:** Open-source Python framework for rapid development of data/AI apps with an intuitive interface.
- **Azure AI Vision (Vision API):** Delivers thorough image analysis, returning tags and signals for further processing.

## How the App Works

1. **Image upload:** Streamlit UI lets users upload any photo.
2. **Tag extraction:** App sends the image to Azure AI Vision and receives a high-confidence tag list.
3. **Caption generation:** Tags are sent to Azure OpenAI's GPT-4o-mini, which creates a natural-sounding one-line caption.
4. **Results:** Caption is displayed in the Streamlit browser app.

## Prerequisites

- Azure subscription ([Sign Up](https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account))
- Azure CLI ([Install guide](https://learn.microsoft.com/azure/cli/azure/install-azure-cli-linux))
- Azure Developer CLI (`azd`) ([Install guide](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd))
- Python 3.10+
- Visual Studio Code (optional)
- Streamlit (for local runs)
- Managed Identity enabled on App Service ([Overview](https://learn.microsoft.com/azure/app-service/overview-managed-identity))

## Resources to Deploy

You can provision resources either manually or using the provided azd template.

### Deployed Components

- Azure App Service (Linux, Python)
- Azure AI Foundry/OpenAI with a GPT-4o-mini deployment
- Azure AI Vision (Computer Vision API)
- Managed Identity for secure service authentication

### Quick Deploy with Azure Developer CLI

```
git clone https://github.com/Azure-Samples/appservice-ai-samples
cd appservice-ai-samples/image_caption_app
azd auth login
azd up
```

All required resources and app deployment are automated with these commands.

### Manual Setup Steps

1. Create Azure AI Vision resource (note the endpoint).
2. Deploy OpenAI resource and set up GPT-4o-mini deployment.
3. Deploy App Service and enable system-assigned Managed Identity.
4. Assign correct RBAC roles:
   - *Cognitive Services OpenAI User* (OpenAI)
   - *Cognitive Services User* (Vision)
5. Add application settings (endpoints/deployment names) and deploy code.
6. Configure startup command (manual path):

   ```
   streamlit run app.py --server.port 8000 --server.address 0.0.0.0
   ```

## Core Code Flow Walkthrough

### Top-level (app.py)

```python
tags = extract_tags(image_bytes)
caption = generate_caption(tags)
```

### Vision API Call (utils/vision.py)

```python
response = requests.post(VISION_API_URL, headers=headers, params=PARAMS, data=image_bytes, timeout=30)
response.raise_for_status()
analysis = response.json()
tags = [t.get('name') for t in analysis.get('tags', []) if t.get('name') and t.get('confidence', 0) > 0.6]
```

### Caption Generation (utils/openai_caption.py)

```python
tag_text = ", ".join(tags)
prompt = f"""
You are an assistant that generates vivid, natural-sounding captions for images. Create a one-line caption for an image that contains the following: {tag_text}.
"""
response = client.chat.completions.create(
    model=DEPLOYMENT_NAME,
    messages=[{"role": "system", "content": "You are a helpful AI assistant."}, {"role": "user", "content": prompt.strip()}],
    max_tokens=60,
    temperature=0.7
)
return response.choices[0].message.content.strip()
```

## Security and Authentication

- By default, **Managed Identity** is enabled, so the app can securely authenticate to Azure resources via Microsoft Entra ID, with no secrets in config.
- For local tests without Managed Identity, key-based authentication is possible by supplying credentials as environment variables.

## Running Locally (optional)

```bash
python -m venv .venv && source .venv/bin/activate  # Windows: .venv\Scripts\activate
pip install -r requirements.txt

# Set environment variables for endpoints and deployments (plus keys if not using MI)

streamlit run app.py
```

## Repository Structure

- App code & Streamlit UI: `/image_caption_app/`
- Infrastructure as code (Bicep): `/image_caption_app/infra/`

## Extension Ideas

- Add object detection, OCR, or brand detection to enhance prompts for captioning.
- Save images and metadata to Blob Storage and Cosmos DB; build a gallery feature.
- Implement performance optimizations (caching, token usage tracking).
- Hook up Application Insights for observability and monitoring.

## Further Learning

- [App Service AI Samples main repo](https://github.com/Azure-Samples/appservice-ai-samples/tree/main)
- [Azure App Service AI Integration Docs](https://learn.microsoft.com/en-us/azure/app-service/overview-ai-integration)

---
**Author:** TulikaC

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-an-ai-image-caption-generator-on-azure-app-service-with/ba-p/4450313)
