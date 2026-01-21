---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-service-ai-scenarios-complete-sample-with-ai-foundry/ba-p/4473142
title: 'Azure App Service AI Scenarios: Complete Sample with AI Foundry Integration'
author: Nitesh_Jain
feed_name: Microsoft Tech Community
date: 2025-11-28 10:42:08 +00:00
tags:
- API Integration
- Audio Transcription
- Azure AI Foundry
- Azure App Service
- Azure CLI
- Azure Cognitive Services
- Azure Developer CLI
- Conversational AI
- Enterprise Chat
- Flask
- Gpt 35 Turbo
- Gpt 4o Mini
- Image Analysis
- Managed Identity
- Microsoft Defender For Cloud
- Model Deployment
- Multimodal AI
- OpenAI
- Python
- RBAC
- Reasoning Models
- Security Best Practices
section_names:
- ai
- azure
- coding
- security
---
Nitesh_Jain presents a comprehensive sample demonstrating how to add enterprise-grade Azure AI scenarios to Flask apps, including conversational, reasoning, and multimodal AI—all with secure App Service integration and automated deployment.<!--excerpt_end-->

# Azure App Service AI Scenarios: Complete Sample with AI Foundry Integration

This guide shows developers how to quickly add advanced AI capabilities to web apps using Azure App Service and Azure AI Foundry. The sample code and instructions focus on integrating with Flask but can be adapted for similar Python-based web projects.

## Key AI Scenarios Implemented

- **Conversational AI:** Natural language processing with context and session management
- **Reasoning Models:** Step-by-step problem-solving features
- **Structured Output:** JSON responses for integrations and schema validation
- **Multimodal Processing:** Run image analysis and audio transcription using vision and audio models
- **Enterprise Chat:** Prebuilt assistant with retail scenarios and business intelligence features

## Quick Start—Automated Azure Deployment

**Prerequisites:**

- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Azure Developer CLI (azd)](https://docs.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd)
- Azure subscription (contributor access)
- Existing Azure AI Foundry model deployments (optional)

**Deployment Models:** Supports both new and existing endpoints. Recommended models: `gpt-4o-mini`, `gpt-35-turbo`. Some advanced models may be region-limited.

**Install and Deploy:**

```bash
git clone https://github.com/Azure-Samples/azure-app-service-ai-scenarios-integrated-sample.git
cd azure-app-service-ai-scenarios-integrated-sample
azd up
```

This installs, configures, and deploys all dependencies, setting up managed identities and provisioning required resources.

**Deployment Prompts:**

- Choose resource group and location
- Specify or create Azure AI Foundry endpoints
- Assign managed identity and select model names

## What Gets Deployed

- Azure App Service (Basic B2, Python 3.11)
- Azure AI Foundry resources: AI project workspace, storage account, model deployments if creating new
- Managed identity for secure authentication, role-based access
- Cognitive Services roles assigned to App Service
- Environment variables and permissions automatically set

## Local Development Setup

- **Python 3.8+** required
- Install requirements:

```bash
pip install -r requirements.txt
python app.py
```

- Use <http://localhost:5000> or /settings for configuration
- Provide Azure AI Foundry endpoint and API key; for production, managed identity is used
- Review [Setup Guide](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/create-projects) for deploying Azure AI Foundry models

## Testing Core Features

- Access floating chat popup on homepage
- Test conversational AI, product inquiries, and customer service flows
- Upload test images and audio files for multimodal analysis
- Validate structured reasoning by entering complex business questions
- Each response should be relevant and demonstrate selected scenario capabilities

## Integration with Existing Flask Applications

### Required Azure Resources

- Azure AI Foundry endpoint with deployed models

### Integration Steps

1. **Enable Managed Identity**
2. **Assign roles for Cognitive Services and Azure AI Developer**
3. **Copy AIPlaygroundCode folder to your app**
4. **Merge dependencies in requirements.txt** (e.g. flask, azure-identity, openai, pillow, pydub)
5. **Add and configure environment variables for inference endpoints and model names**
6. **Add /settings route and templates for AI configuration**
7. **Integrate chat interface HTML, CSS, and JavaScript into your app templates**
8. **Test that the chat popup, file uploads, and all AI scenarios work end-to-end**

## Cleaning Up Resources

- Use `azd down` to remove deployed resources when finished
- Or delete resource groups directly from Azure Portal
- Resource deletion may take up to 10 minutes

## Cost and Security Guidance

- Azure usage incurs consumption-based charges; use [Azure pricing calculator](https://azure.microsoft.com/pricing/calculator) for details
- Managed Identity eliminates manual key/config management, improves security
- It is highly recommended to enable [Microsoft Defender for Cloud](https://learn.microsoft.com/azure/defender-for-cloud/), configure VNETs, and Web Application Firewall
- Use [GitHub secret scanning](https://docs.github.com/code-security/secret-scanning/about-secret-scanning) to safeguard sensitive information

## Reference Resources

- [GitHub Repository](https://github.com/Azure-Samples/azure-app-service-ai-scenarios-integrated-sample)
- [Project Structure](https://github.com/Azure-Samples/azure-app-service-ai-scenarios-integrated-sample/blob/main/docs/PROJECT_STRUCTURE.md)
- [Configuration Guide](https://github.com/Azure-Samples/azure-app-service-ai-scenarios-integrated-sample/blob/main/docs/CONFIGURATION_GUIDE.md)
- [Testing Guide](https://github.com/Azure-Samples/azure-app-service-ai-scenarios-integrated-sample/blob/main/docs/TESTING_GUIDE.md)
- [FAQ & Troubleshooting](https://github.com/Azure-Samples/azure-app-service-ai-scenarios-integrated-sample/blob/main/docs/FAQ_and_troubleshooting.md)
- [Chat Completions (Azure AI Foundry)](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-models/how-to/use-chat-completions?tabs=python)
- [Reasoning Model Docs](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-models/how-to/use-chat-reasoning?tabs=openai&pivots=programming-language-python)
- [Multimodal AI](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-models/how-to/use-chat-multi-modal?pivots=programming-language-python)
- [Structured Outputs](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-models/how-to/use-structured-outputs?pivots=programming-language-python)

## Support

- File issues and feature requests via GitHub Issues or Discussions

## Important Notices

- This is a developer sample. Consider enabling additional security and monitoring before using in production.

---
Author: Nitesh_Jain

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-service-ai-scenarios-complete-sample-with-ai-foundry/ba-p/4473142)
