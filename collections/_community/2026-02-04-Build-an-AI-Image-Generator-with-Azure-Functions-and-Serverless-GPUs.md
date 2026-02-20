---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/serverless-gpu-tutorial-build-an-ai-image-generator-with-azure/ba-p/4492228
title: Build an AI Image Generator with Azure Functions and Serverless GPUs
author: Nitesh_Jain
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-04 17:43:54 +00:00
tags:
- AI
- AI Image Generation
- API Development
- Azure
- Azure Container Apps
- Azure Developer CLI
- Azure Functions
- Cloud Deployment
- Community
- Containerization
- Cost Optimization
- Diffusers
- Docker
- GPU Workload
- Monitoring
- Python
- Serverless GPUs
- Stable Diffusion
- .NET
section_names:
- ai
- azure
- dotnet
---
Nitesh_Jain demonstrates how to build an AI image generator powered by Stable Diffusion, leveraging Azure Functions and serverless GPUs on Azure Container Apps. The guide covers development, deployment, optimization, and cost-saving tips.<!--excerpt_end-->

# Build an AI Image Generator with Azure Functions and Serverless GPUs

Learn how to build your own AI-powered image generator API using Azure Functions running on Azure Container Apps with NVIDIA GPU acceleration. This step-by-step guide walks you through the architecture, code, deployment, optimization, and troubleshooting—all with minimal infrastructure management.

## What You'll Build

- **A REST API that turns text descriptions into AI-generated images** using Stable Diffusion running in a containerized Azure Function with on-demand GPU support.
- **Serverless GPU hosting**: You only pay while generating images, and Azure handles scaling, driver installation, and infrastructure.

## Architecture Overview

**Flow:**

- Your App ⟶ Azure Function on Container Apps ⟶ Stable Diffusion (on GPU) ⟶ Image returned as base64

## Prerequisites

- Azure account ([get free one](https://azure.microsoft.com/free/))
- GPU quota approval ([request here](https://aka.ms/aca-gpu-request))
- Azure CLI ([installation guide](https://docs.microsoft.com/cli/azure/install-azure-cli))

**Note:** Request GPU quota early, as approval can take a day or two.

## Step 1: Clone the Sample Code

```bash
git clone https://github.com/Azure-Samples/function-on-aca-gpu.git
cd function-on-aca-gpu
```

Project structure:

- `function_app.py` — Handles HTTP requests and triggers image generation
- `requirements.txt` — Lists required Python packages
- `Dockerfile` — Containerizes the function app for deployment
- `deploy.ps1` & `deploy.sh` — Automates resource creation on Azure

## Step 2: Review the Image Generation Code

**Core steps (in `function_app.py`):**

1. Receive POST with a user prompt (e.g., "a robot painting a sunset").
2. Load the Stable Diffusion pipeline (cached after first load).
3. Generate an image with the prompt using the diffusers library and GPU.
4. Encode image result to base64, return as JSON.

Sample API usage:

```http
POST /api/generate
{
  "prompt": "A friendly robot chef cooking pasta in a cozy kitchen"
}
```

## Step 3: Deploy to Azure

Three deployment options:

### A. Azure Developer CLI (`azd up`)

- **Fastest method**: single command does it all
- Prompts for environment name, Azure region (use `swedencentral` for GPU), and Azure subscription

Steps:

```bash
git clone https://github.com/Azure-Samples/function-on-aca-gpu.git
cd function-on-aca-gpu
azd up
```

Creates and configures all resources, builds image, deploys, and outputs your API endpoint URL.

### B. PowerShell/Bash Scripts

- More control; run `deploy.ps1` (Windows) or `deploy.sh` (Linux/Mac)

### C. Azure Portal

- Manual, visual process—suitable for learning or granular configuration

#### Key Resources Created

- Container Registry
- Container Apps Environment with GPU profile
- Azure Function App (containerized, GPU-enabled)
- Monitoring (Log Analytics, Application Insights)

## Testing & Usage

1. **Health check**

   ```bash
   Invoke-RestMethod -Uri "https://YOUR-FUNCTION-URL/api/health"
   # Should show GPU available: true
   ```

2. **Generate an image**

   ```powershell
   $body = @{ prompt = "A happy corgi astronaut floating in space, digital art"; num_steps = 25 } | ConvertTo-Json
   $response = Invoke-RestMethod -Uri "https://YOUR-FUNCTION-URL/api/generate" -Method POST -ContentType "application/json" -Body $body
   [IO.File]::WriteAllBytes("corgi-astronaut.png", [Convert]::FromBase64String($response.image))
   Start-Process "corgi-astronaut.png"
   ```

   _First request may take 1-2 minutes; after model loads, subsequent requests are fast._

## API Reference

**`POST /api/generate` Parameters:**

| Name             | Type   | Default | Description                                       |
|------------------|--------|---------|---------------------------------------------------|
| prompt           | string | —       | The description of the desired image              |
| negative_prompt  | string | ""      | Things to avoid (e.g., "blurry, ugly")            |
| num_steps        | int    | 25      | Number of inference steps (quality vs. speed)     |
| guidance_scale   | float  | 7.5     | Strictness for matching prompt                    |
| width            | int    | 512     | Output image width                                |
| height           | int    | 512     | Output image height                               |

## Cost Optimization

- Default setup scales GPU to zero when idle (you pay only for use)
- Set `min-replicas` to 0 for dev/testing; raise for production to avoid cold starts
- Use [Azure Pricing Calculator](https://azure.microsoft.com/pricing/calculator/)

## Troubleshooting & Tips

- If model fails to load: Check if the model requires authentication. This setup uses an open model.
- Slow start: First invocation will be slower to download and cache the model.
- GPU not detected: Make sure your quota is approved and Azure region supports GPU in Container Apps.
- For custom models or advanced use (e.g., SDXL), update the `MODEL_ID` environment variable.
- Try different prompts for creative results; tune `num_steps`, `guidance_scale` for quality/performance trade-offs.

## Next Steps

- Build a web UI for easier interaction
- Experiment with different AI models
- Add features (image-to-image, Discord bot integration)

## Resource Cleanup

When finished, you can delete all resources to stop billing:

```bash
az group delete --name gpu-functions-rg --yes
```

## Support & Additional Resources

- Full sample code: [Azure-Samples/function-on-aca-gpu](https://github.com/Azure-Samples/function-on-aca-gpu)
- [Azure Container Apps GPU Documentation](https://learn.microsoft.com/azure/container-apps/gpu-support)
- [Azure Functions on Container Apps](https://learn.microsoft.com/azure/azure-functions/functions-container-apps-hosting)
- [Stable Diffusion with Diffusers](https://huggingface.co/docs/diffusers/using-diffusers/conditional_image_generation)

## Acknowledgments

This tutorial is created by Nitesh_Jain. For support with Azure infrastructure or to contribute, open GitHub issues or use Microsoft Azure support channels.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/serverless-gpu-tutorial-build-an-ai-image-generator-with-azure/ba-p/4492228)
