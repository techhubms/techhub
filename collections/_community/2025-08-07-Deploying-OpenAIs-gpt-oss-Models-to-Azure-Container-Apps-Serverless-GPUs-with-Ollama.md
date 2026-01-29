---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/open-ai-s-gpt-oss-models-on-azure-container-apps-serverless-gpus/ba-p/4440836
title: Deploying OpenAI's gpt-oss Models to Azure Container Apps Serverless GPUs with Ollama
author: Cary_Chai
feed_name: Microsoft Tech Community
date: 2025-08-07 20:23:51 +00:00
tags:
- A100 GPU
- API Integration
- Autoscaling
- Azure Container Apps
- Cloud AI
- Containerization
- Cost Optimization
- Docker
- Gpt Oss 120b
- Gpt Oss 20b
- Inference
- LLM Deployment
- Microservices
- Ollama
- OpenAI
- Public Models
- Scalable AI
- Serverless GPU
- T4 GPU
- AI
- Azure
- Community
section_names:
- ai
- azure
primary_section: ai
---
Cary_Chai provides a clear walkthrough on deploying OpenAI's gpt-oss-120b and gpt-oss-20b models in Azure Container Apps with serverless GPU support via Ollama, outlining deployment steps, technical considerations, and tips for scalable AI workloads in the cloud.<!--excerpt_end-->

# Deploying OpenAI's gpt-oss Models to Azure Container Apps Serverless GPUs with Ollama

**Author:** Cary_Chai

OpenAI recently introduced the open-weight gpt-oss-120b and gpt-oss-20b models, making it easier for developers to self-host powerful language models. This guide demonstrates how to deploy these models using Ollama containers on Azure Container Apps serverless GPUs, taking advantage of the platform's autoscaling, managed identity, and enterprise features.

## Why Use Azure Container Apps Serverless GPUs?

Azure Container Apps offers a serverless environment for running containerized applications. With GPU backing (A100 or T4), developers can run AI models at scale without managing infrastructure. Key benefits include:

- **Autoscaling:** Scales to zero when idle, scales out automatically
- **Pay-per-second billing:** Cost optimization by paying only for actual usage
- **Ease of Deployment:** Simple container onboarding, managed networking, and identity
- **Enterprise Features:** VNET, private endpoints, compliance, and data governance

## Choosing the Right gpt-oss Model

- **gpt-oss-120b:** Powerful, designed for high-performance inference, comparable to gpt-o4-mini. Requires A100 GPUs.
- **gpt-oss-20b:** Lightweight, ideal for smaller workloads, similar to gpt-o3-mini. Runs efficiently on T4 GPUs (or A100).

| Region           | A100 | T4  |
|------------------|------|-----|
| West US          | ✅    |     |
| West US 3        | ✅    | ✅  |
| Sweden Central   | ✅    | ✅  |
| Australia East   | ✅    | ✅  |
| West Europe      |      | ✅  |

## Step-by-Step Deployment Guide

### 1. Create Azure Container App

- Navigate to the [Azure Portal](https://portal.azure.com/)
- Create a new resource > Search for 'Azure Container Apps' > Create
- On the **Basics** tab, select the appropriate region (A100 or T4 available)

### 2. Configure the Container

- **Image Source:** Docker Hub (or another registry)
- **Image Name:** `ollama/ollama:latest`
- **Workload Profile:** Consumption
- **GPU:** Enable GPU, select A100 for 120b or T4/A100 for 20b
- If quota is insufficient, [request more here](https://learn.microsoft.com/en-us/azure/container-apps/gpu-serverless-overview#request-serverless-gpu-quota)

### 3. Configure Ingress

- **Ingress Enabled:** Yes
- **Traffic:** Accept from anywhere
- **Target Port:** 11434

### 4. Review and Create

- Click 'Review + Create', then 'Create' to deploy the resource.

## Running the gpt-oss Model on Azure Container Apps

1. Access the deployed resource and use the Application URL to launch your container app.
2. For hands-on interaction, use the Azure Portal's **Monitoring > Console**:
   - Start Ollama:

     ```bash
     ollama serve
     ```

   - Pull the desired model:

     ```bash
     ollama pull gpt-oss:120b
     # or for 20b:
     ollama pull gpt-oss:20b
     ```

   - Run the model:

     ```bash
     ollama run gpt-oss:120b
     ```

   - Enter prompts to interact with the LLM.

3. (Optional) To keep containers running for extended sessions, adjust replica or cooldown settings under 'Scaling'.

## Calling the Ollama gpt-oss API from Your Local Machine

1. Set the `OLLAMA_URL` to your Application URL:

   ```bash
   export OLLAMA_URL="{Your application URL}"
   ```

2. Make an API call using curl:

   ```bash
   curl -X POST "$OLLAMA_URL/api/generate" -H "Content-Type: application/json" \
     -d '{"model":"gpt-oss:120b", "prompt":"Can you explain LLMs and recent developments in AI the last few years?", "stream":false}'
   ```

## Persisting State and Data

Azure Container Apps are ephemeral. To persist data (e.g., model state or logs), [add a volume mount](https://learn.microsoft.com/azure/container-apps/storage-mounts) following official documentation.

## Additional Resources

- [OpenAI gpt-oss Models](https://openai.com/index/introducing-gpt-oss/)
- [Ollama Container](https://hub.docker.com/r/ollama/ollama)
- [Azure Container Apps Serverless GPUs Documentation](https://learn.microsoft.com/azure/container-apps/gpu-serverless-overview)
- [Ollama Model Library](https://ollama.com/search)

## Summary

By following this guide, you can self-host high-performance language models like gpt-oss-120b and gpt-oss-20b on Azure's fully managed, scalable, and cost-efficient serverless GPU infrastructure. The process covers best practices for deployment, operation, and integration for advanced AI workloads.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/open-ai-s-gpt-oss-models-on-azure-container-apps-serverless-gpus/ba-p/4440836)
