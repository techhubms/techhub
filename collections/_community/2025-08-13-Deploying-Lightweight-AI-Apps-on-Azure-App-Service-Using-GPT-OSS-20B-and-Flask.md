---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-lightweight-ai-apps-on-azure-app-service-with-gpt-oss-20b/ba-p/4442885
title: Deploying Lightweight AI Apps on Azure App Service Using GPT-OSS-20B and Flask
author: TulikaC
feed_name: Microsoft Tech Community
date: 2025-08-13 13:06:16 +00:00
tags:
- Azure App Service
- Azure Container Registry
- Azure Deployment
- Bicep
- CI/CD Integration
- Container Sidecar
- Enterprise Security
- Flask
- GPT OSS 20B
- Model Containerization
- Ollama
- OpenAI
- Premium V4 Plan
- Python
- Real Time Streaming
section_names:
- ai
- azure
- coding
primary_section: ai
---
TulikaC outlines how to combine GPT-OSS-20B and a Flask web app as containers within Azure App Service, giving developers a robust pattern for running open-weight language models with cloud scalability and minimal infrastructure overhead.<!--excerpt_end-->

# Deploying Lightweight AI Apps on Azure App Service Using GPT-OSS-20B and Flask

**Author: TulikaC**

OpenAI's GPT-OSS-20B is an open-weight language model optimized for both performance and cost under the Apache 2.0 license. By running this model on Azure App Service as a sidecar container alongside a Python Flask app, you gain performance, enterprise security, and scalability without the burden of managing underlying infrastructure.

## Architecture Overview

- **Flask App**: Runs on Azure App Service, handling frontend/API routes.
- **Sidecar Model Container**: GPT-OSS-20B packaged with Ollama, running locally and accessible via `localhost:11434`.
- **Infrastructure as Code**: Provisioned with Bicep templates, Azure Container Registry (ACR), and deployed with tools like `azd`, VS Code, or GitHub Actions.

## 1. Model Containerization with Ollama

**Dockerfile Example:**

```dockerfile
FROM ollama/ollama
EXPOSE 11434
COPY startup.sh /
RUN chmod +x /startup.sh
ENTRYPOINT ["./startup.sh"]
```

**startup.sh Example:**

```bash
# Start Ollama in the background

ollama serve & sleep 5

# Pull and run gpt-oss:20b

ollama pull gpt-oss:20b

# Restart ollama and run it in the foreground

pkill -f "ollama"
ollama serve
```

This wraps the GPT-OSS-20B model for quick and repeatable deployment as a sidecar.

## 2. Flask Application Highlights

- Connects to GPT-OSS-20B via HTTP on the local network namespace.
- Streams responses so users see real-time chat output.

**Core Python Snippet:**

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

This produces a browser-based chat UI backed by the locally running model.

## 3. Deploying to Azure App Service

Deploy using any of these options:

- Visual Studio Code or GitHub Actions
- `az webapp up`
- **Bicep templates** (provided in the sample repo)

**Deployment Steps:**

1. Provision ACR and the Web App (Premium V4 preferred for performance)
2. Push model container to ACR
3. Attach the container as a sidecar within the Azure Portal ([configuration guide](https://learn.microsoft.com/azure/app-service/configure-sidecar))
4. Set sidecar port to 11434
5. Note: First startup involves model download, so expect a cold start; reboots are faster after model layers are cached.

## Next Steps & Resources

- [Sample Code & Templates (GitHub)](https://github.com/Azure-Samples/appservice-ai-samples/tree/main/gpt-oss-20b-sample)
- [Introducing GPT-OSS (OpenAI blog)](https://openai.com/index/introducing-gpt-oss/)
- [Configure Sidecars in Azure App Service](https://learn.microsoft.com/azure/app-service/configure-sidecar)
- [Azure App Service Premium V4 Public Preview](https://techcommunity.microsoft.com/blog/appsonazureblog/azure-app-service-premium-v4-plan-is-now-in-public-preview/4413461)
- [Push and Pull Images to ACR](https://learn.microsoft.com/azure/container-registry/container-registry-get-started-docker-cli?tabs=azure-cli)
- [Build RAG Solutions with Azure AI Search](https://learn.microsoft.com/azure/search/retrieval-augmented-generation-overview?tabs=docs)

## Conclusion

This approach lets you harness both the flexibility of open-source AI models and the reliability, security, and scaling of Azure's managed cloud services. Whether you're rapidly prototyping, scaling AI-powered features, or experimenting with custom ML, deploying GPT-OSS-20B as a sidecar in Azure App Service streamlines adoption and management for technical teams.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-lightweight-ai-apps-on-azure-app-service-with-gpt-oss-20b/ba-p/4442885)
