---
layout: "post"
title: "Deploying OpenAI’s GPT-OSS-20B on Azure AKS with KAITO and vLLM"
description: "This technical tutorial details the end-to-end process of deploying OpenAI’s first open-source large language model, GPT-OSS-20B, on Azure Kubernetes Service (AKS) using KAITO and vLLM. The guide covers Azure resource setup, Kubernetes cluster provisioning, GPU quota management, YAML configuration for KAITO, public endpoint exposure, API testing, and benchmarking. Readers will learn how to build a scalable, high-performance AI inference endpoint on Azure with real-world load testing and optimization advice."
author: "maljazaery"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/ai-machine-learning-blog/deploying-openai-s-first-open-source-model-on-azure-aks-with/ba-p/4444234"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 17:08:39 +00:00
permalink: "/community/2025-08-15-Deploying-OpenAIs-GPT-OSS-20B-on-Azure-AKS-with-KAITO-and-vLLM.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "AI Endpoint", "AKS", "API Deployment", "Azure", "Azure CLI", "Chat Completions API", "Community", "GPT OSS 20B", "GPU Inference", "KAITO", "Kubectl", "Kubernetes", "LLM", "Load Testing", "ML", "NVIDIA A10", "OpenAI", "Performance Benchmarking", "Token Throughput", "Vllm", "Workspace Configuration", "YAML"]
tags_normalized: ["ai", "ai endpoint", "aks", "api deployment", "azure", "azure cli", "chat completions api", "community", "gpt oss 20b", "gpu inference", "kaito", "kubectl", "kubernetes", "llm", "load testing", "ml", "nvidia a10", "openai", "performance benchmarking", "token throughput", "vllm", "workspace configuration", "yaml"]
---

maljazaery presents a practical step-by-step tutorial on deploying OpenAI’s GPT-OSS-20B model on Azure’s AKS using KAITO and vLLM. The guide covers the full technical workflow, including cluster creation, GPU setup, inference optimization, public exposure, endpoint testing, and load benchmarking.<!--excerpt_end-->

# Deploying OpenAI’s GPT-OSS-20B on Azure AKS with KAITO and vLLM

Deploy and optimize OpenAI’s first open-source large language model, GPT-OSS-20B, on Microsoft Azure’s cloud GPU infrastructure using the Kubernetes AI Toolchain Operator (KAITO) and vLLM for high-performance inference.

## Introduction

OpenAI’s GPT-OSS-20B is a powerful open-source LLM. Deploying such a model for real-time inference at scale requires modern GPU hardware and efficient orchestration. Azure’s Standard_NV36ads_A10_v5 GPU instances alongside AKS and KAITO streamline resource provisioning, management, and AI workload deployment.

### Prerequisites

- Active Azure subscription with permission to create resource groups and clusters
- Approved quota for GPUs (Standard_NVads_A10_v5 or similar)
- Familiarity with Kubernetes, kubectl, and Azure CLI

## Step 1: Set Up Environment Variables

Define shell variables for resource uniqueness and region selection:

```sh
export RANDOM_ID="33000"
export REGION="swedencentral"
export AZURE_RESOURCE_GROUP="myKaitoResourceGroup$RANDOM_ID"
export CLUSTER_NAME="myClusterName$RANDOM_ID"
```

## Step 2: Create Azure Resource Group

```sh
az group create \
  --name $AZURE_RESOURCE_GROUP \
  --location $REGION
```

## Step 3: Enable AKS Preview Features

Install the aks-preview extension and register AI Toolchain features.

```sh
az extension add --name aks-preview
az extension update --name aks-preview
az feature register \
  --namespace "Microsoft.ContainerService" \
  --name "AIToolchainOperatorPreview"
```

Check registration status with `az feature list`.

## Step 4: Create AKS Cluster With AI Toolchain Operator

```sh
az aks create \
  --location $REGION \
  --resource-group $AZURE_RESOURCE_GROUP \
  --name $CLUSTER_NAME \
  --node-count 1 \
  --enable-ai-toolchain-operator \
  --enable-oidc-issuer \
  --generate-ssh-keys
```

## Step 5: Connect kubectl to Cluster

```sh
az aks get-credentials \
  --resource-group ${AZURE_RESOURCE_GROUP} \
  --name ${CLUSTER_NAME}
```

## Step 6: Create KAITO Workspace YAML

Prepare a KAITO workspace specification file (`workspace-gptoss.yaml`):

```yaml
apiVersion: kaito.sh/v1alpha1
kind: Workspace
metadata:
  name: workspace-gpt-oss-vllm-nv-a10
resource:
  instanceType: "Standard_NV36ads_A10_v5"
  count: 1
labelSelector:
  matchLabels:
    app: gpt-oss-20b-vllm
inference:
  template:
    spec:
      containers:
      - name: vllm-openai
        image: vllm/vllm-openai:gptoss
        imagePullPolicy: IfNotPresent
        args:
          - --model
          - openai/gpt-oss-20b
          - --swap-space
          - "4"
          - --gpu-memory-utilization
          - "0.85"
          - --port
          - "5000"
        ports:
          - name: http
            containerPort: 5000
        resources:
          limits:
            nvidia.com/gpu: 1
            cpu: "36"
            memory: "440Gi"
          requests:
            nvidia.com/gpu: 1
            cpu: "18"
            memory: "220Gi"
        readinessProbe:
          httpGet:
            path: /health
            port: 5000
          initialDelaySeconds: 30
          periodSeconds: 10
        livenessProbe:
          httpGet:
            path: /health
            port: 5000
          initialDelaySeconds: 600
          periodSeconds: 20
        env:
          - name: VLLM_ATTENTION_BACKEND
            value: "TRITON_ATTN_VLLM_V1"
          - name: VLLM_DISABLE_SINKS
            value: "1"
```

Apply it:

```sh
kubectl apply -f workspace-gptoss.yaml
```

## Step 7: Expose the Inference Service Publicly

Expose the deployment via LoadBalancer:

```sh
kubectl expose deployment workspace-gpt-oss-vllm-nv-a10 \
  --type=LoadBalancer \
  --name=workspace-gpt-oss-vllm-nv-a10-pub
kubectl get svc workspace-gpt-oss-vllm-nv-a10-pub
```

## Step 8: Test the Endpoint

Use curl or the OpenAI Python SDK to send API-format requests:

**Shell Test:**

```sh
export CLUSTERIP=<public_ip>
kubectl run -it --rm --restart=Never curl \
  --image=curlimages/curl -- \
  curl -X POST http://$CLUSTERIP/v1/chat/completions \
    -H "Content-Type: application/json" \
    -d '{"model": "openai/gpt-oss-20b", "messages": [{"role": "user", "content": "What is Kubernetes?"}], "max_tokens": 50, "temperature": 0}'
```

**Python SDK:**

```python
from openai import OpenAI
client = OpenAI(base_url="http://<ip>:5000/v1/", api_key="EMPTY")
result = client.chat.completions.create(
    model="openai/gpt-oss-20b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain what MXFP4 quantization is."}
    ]
)
print(result.choices[0].message)
```

## Step 9: Load Testing & Performance Benchmarking

The [llm-load-test-azure](https://github.com/maljazaery/llm-load-test-azure) tool can stress test inference endpoints. Example settings:

- Input Tokens per Request: 250
- Output Tokens per Request: ~1,500
- Test Duration: ~10 minutes
- Concurrency: 10

**Key Metrics:**
| Metric                  | Value     | Description                                |
|-------------------------|-----------|--------------------------------------------|
| TT_ACK                  | 0.60 s    | Time to acknowledge the request            |
| TTFT                    | 0.77 s    | Time to first token                        |
| ITL                     | 0.038 s   | Idle time per request                      |
| TPOT                    | 0.039 s   | Time per output token                      |
| Avg. Response Time      | 57.77 s   | Total latency per request                  |
| Output Tokens Throughput| 257.77/s  | Average output tokens per second           |

**Concurrency Tradeoff:**

- Lowering concurrency reduces per-request latency but may decrease throughput.
- Example: 10 vs 5 concurrency decreased average response time by ~21%; higher concurrency increases request throughput.

## Conclusion

You have successfully:

- Provisioned an AKS cluster with GPU support
- Installed KAITO and deployed GPT-OSS-20B with vLLM
- Set up a public inference endpoint
- Validated with API testing and performance load tests

Scale your setup by adjusting GPU count, vLLM configuration, or integrating with production applications. This architecture gives you a flexible, open-source, cloud-native LLM deployment on Azure for advanced inference workloads.

---

_Special thanks to Andrew Thomas, Kurt Niebuhr, and Sachi Desai for support during this deployment._

_Author: maljazaery — Updated August 15, 2025_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/ai-machine-learning-blog/deploying-openai-s-first-open-source-model-on-azure-aks-with/ba-p/4444234)
