---
layout: "post"
title: "Deploying GPT-OSS-20B as a Sidecar AI Model on Azure App Service"
description: "This guide explains how to deploy the open-weight GPT-OSS-20B language model as a sidecar container alongside a Python Flask application on Azure App Service. It covers architecture, containerization, real-time chat streaming, infrastructure-as-code with Bicep, CI/CD setup, and how to use Azure features to host and scale efficient AI-powered applications for developers."
author: "TulikaC"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-lightweight-ai-apps-on-azure-app-service-with-gpt-oss-20b/ba-p/4442885"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-13 13:06:16 +00:00
permalink: "/2025-08-13-Deploying-GPT-OSS-20B-as-a-Sidecar-AI-Model-on-Azure-App-Service.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "Autoscaling", "Azure", "Azure App Service", "Azure Container Registry", "Bicep", "CI/CD Integration", "Cloud Architecture", "Coding", "Community", "Containerization", "Enterprise Security", "Flask", "GPT OSS", "IaC", "Model Deployment", "Ollama", "OpenAI", "Premium V4", "Python", "Real Time AI", "Sidecar Container", "Streaming Response"]
tags_normalized: ["ai", "autoscaling", "azure", "azure app service", "azure container registry", "bicep", "ci slash cd integration", "cloud architecture", "coding", "community", "containerization", "enterprise security", "flask", "gpt oss", "iac", "model deployment", "ollama", "openai", "premium v4", "python", "real time ai", "sidecar container", "streaming response"]
---

TulikaC demonstrates deploying the GPT-OSS-20B open-weight language model as a sidecar container with a Python Flask app on Azure App Service, offering developers a scalable and secure platform for AI-backed applications.<!--excerpt_end-->

# Deploying GPT-OSS-20B as a Sidecar AI Model on Azure App Service

**Author:** TulikaC

## Overview

OpenAI's GPT-OSS-20B is an open, high-performing language model available under the Apache 2.0 license. This guide describes how to deploy it as a sidecar container with a Python Flask web application on Azure App Service, leveraging Azure's managed capabilities for scale, security, and DevOps.

## Key Benefits

- **Efficient Model Hosting**: Run GPT-OSS-20B with your own application instance, optimizing for cost, privacy, and control.
- **Enterprise Azure Features**: Use built-in autoscaling, robust security, VNet integration, and seamless CI/CD in Azure App Service.
- **Developer-Friendly Deployment**: Rely on containers, Bicep templates, and Azure Container Registry for maintainability.

---

## Solution Architecture

- **Application**: Python Flask web app, served via Azure App Service (code-based).
- **AI Model**: GPT-OSS-20B, running as a sidecar container using Ollama within the same App Service instance.
- **Communication**: The Flask app sends requests to the sidecar model container using `localhost:11434` for inference.
- **Infrastructure as Code**: Bicep scripts provision all Azure resources (Web App, ACR, networking, etc.).

![Architecture diagram omitted]

### 1. Containerizing GPT-OSS-20B

The model is pre-packaged in a Docker image (using Ollama as a runtime). Example Dockerfile:

```dockerfile
FROM ollama/ollama
EXPOSE 11434
COPY startup.sh /
RUN chmod +x /startup.sh
ENTRYPOINT ["./startup.sh"]
```

**`startup.sh` script:**

```bash
# Start background model server

ollama serve & sleep 5

# Download the gpt-oss:20b model

ollama pull gpt-oss:20b

# Restart Ollama in foreground to serve the model

pkill -f "ollama"
ollama serve
```

### 2. Flask Application Integration

The app connects to the GPT-OSS-20B model running in the sidecar using local HTTP requests. Key code sample:

```python
OLLAMA_HOST = "http://localhost:11434"
MODEL_NAME = "gpt-oss:20b"

@app.route("/chat", methods=["POST"])
def chat():
    data = request.get_json()
    prompt = data.get("prompt", "")
    payload = {
        "model": MODEL_NAME,
        "messages": [{"role": "user", "content": prompt}],
        "stream": True
    }

    def generate():
        with requests.post(f"{OLLAMA_HOST}/api/chat", json=payload, stream=True) as r:
            for line in r.iter_lines(decode_unicode=True):
                if line:
                    event = json.loads(line)
                    if "message" in event:
                        yield event["message"]["content"]
    return Response(generate(), mimetype="text/plain")
```

- **Real-Time Streaming**: The endpoint streams model responses as they're generated, suitable for chat UIs.

### 3. Azure Deployment Workflow

Resources are provisioned using Bicep templates:

- **Azure Container Registry (ACR)**: Stores the Ollama/GPT-OSS-20B container image.
- **Azure App Service (Premium V4)**: Hosts the Flask app and attaches the model container as a sidecar.

#### Deployment Steps

1. Build and push the GPT-OSS-20B sidecar image to ACR.
2. Deploy the Flask app via VS Code, CLI, GitHub Actions, or Bicep (see template).
    - Example:

      ```bash
      azd init
      azd up
      ```

3. In the Azure Portal, attach the sidecar image to your Web App using port 11434 ([sidecar docs](https://learn.microsoft.com/azure/app-service/configure-sidecar)).
4. On initial run, the sidecar downloads the model—subsequent restarts are faster.

### Additional Azure Features

- **Autoscaling** to match demand
- **VNet and security integration** for compliance
- **CI/CD support** via GitHub Actions or Azure Pipelines
- **Monitoring and observability** via App Insights

---

## Conclusion

Running GPT-OSS-20B as a sidecar on Azure App Service combines the speed and freedom of open-source models with the scalability, security, and ease of use of the Azure managed platform. This architecture is ideal for:

- Lightweight enterprise chatbots
- AI-powered feature prototypes
- Experimenting with self-hosted models, including future model swaps or domain-specific tuning

You can easily adapt this approach for other open AI models, container runtimes, or frameworks.

---

## Resources & Next Steps

- [Sample Repository (all Bicep/code)](https://github.com/Azure-Samples/appservice-ai-samples/tree/main/gpt-oss-20b-sample)
- [GPT-OSS announcement (OpenAI)](https://openai.com/index/introducing-gpt-oss/)
- [Azure App Service Sidecars](https://learn.microsoft.com/azure/app-service/configure-sidecar)
- [Azure App Service Premium V4](https://techcommunity.microsoft.com/blog/appsonazureblog/azure-app-service-premium-v4-plan-is-now-in-public-preview/4413461)
- [Pushing Images to Azure Container Registry](https://learn.microsoft.com/azure/container-registry/container-registry-get-started-docker-cli?tabs=azure-cli)
- [Advanced AI architectures: RAG on Azure](https://learn.microsoft.com/azure/search/retrieval-augmented-generation-overview?tabs=docs)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-lightweight-ai-apps-on-azure-app-service-with-gpt-oss-20b/ba-p/4442885)
