---
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/ai-inferencing-in-air-gapped-environments/ba-p/4498594
title: Deploying AI Inferencing in Isolated Azure Kubernetes Service (AKS) Clusters
author: damocelj
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-03-09 09:18:31 +00:00
tags:
- AI
- Air Gapped
- AKS
- Artifact Cache
- Azure
- Azure Container Registry
- Community
- Container Deployment
- Data Exfiltration
- DevOps
- GPU Node Pool
- Hugging Face
- Kubernetes
- Large Language Models
- LLM
- ML
- Model Weights
- Network Isolation
- NFS
- NVIDIA GPU Operator
- NVIDIA NIM
- Private Cluster
- Security
- Vllm
section_names:
- ai
- azure
- devops
- ml
- security
---
damocelj offers a practical walkthrough on securely deploying LLM inferencing with vLLM and NVIDIA NIM microservices in air-gapped Azure Kubernetes Service clusters, tackling network isolation, GPU configuration, and model artifact challenges.<!--excerpt_end-->

# Deploying AI Inferencing in Isolated Azure Kubernetes Service (AKS) Clusters

**Author:** damocelj

This article explores deploying large language model (LLM) inferencing solutions—using vLLM and NVIDIA NIM—in highly secure, air-gapped Azure Kubernetes Service (AKS) clusters. The focus is on enabling secure, scalable, and high-throughput AI inferencing while maintaining strict network isolation, a requirement for many regulated or high-security environments.

## Key Trends: AI and Cybersecurity

Generative AI and cybersecurity are converging fields, especially as AI workloads require additional security to protect valuable data from increasingly sophisticated cyber threats. Organizations must architect environments where both AI innovations and robust security controls go hand in hand.

## Kubernetes for AI Workloads

Kubernetes has become the standard for deploying AI workloads due to its portability and orchestration capabilities. Azure Kubernetes Service (AKS) simplifies running managed Kubernetes clusters in Azure, offering features such as private clusters and network isolation to help meet stringent security standards.

### Cluster Types in AKS

- **Public AKS clusters:** API and node endpoints accessible via public IPs.
- **Private clusters:** All control plane and node communication is private, with Azure Firewall regulating egress traffic.
- **Network-isolated (air-gapped) clusters:** No outbound traffic allowed, preventing accidental or malicious data exfiltration (see [AKS network isolation concepts](https://learn.microsoft.com/azure/aks/concepts-network-isolated)).

## Design Considerations for Air-Gapped AKS

To deploy AI workloads in an air-gapped cluster, the following must be ensured:

- No public load balancer, NAT gateway, or public IPs on the node subnet.
- Subnet configured for no default outbound access.
- Critical Azure resources like Container Registry, Storage, or Key Vault use Private Link for in-VNet consumption.
- Use Azure Container Registry's "artifact cache" to locally retrieve images needed at cluster creation.

## The LLM Model Weights Challenge

LLM container images (from Hugging Face, NVIDIA, etc.) typically include runtimes (like vLLM) but not the actual model weights, which are downloaded at startup. In an air-gapped environment, dynamic download is not feasible, requiring alternative strategies to ensure the required model artifacts are locally available:

### Two Solution Strategies

1. **Bake Model Weights into Container Images:**
   - Pull the target Hugging Face model and bake into a custom container image.
   - Use `az acr build` to build and push the image to Azure Container Registry accessible by AKS.
   - Deploy as a pod within the isolated cluster.
2. **Pre-Download Model Artifacts to Private Storage:**
   - Download model weights/artifacts on a VM with external connectivity.
   - Copy artifacts to a shared file system (e.g. Azure Files with NFS) accessible from the air-gapped cluster.
   - Mount the share in your model-serving pods.

## GPU Node Pool Provisioning

For high-performance AI inferencing, GPU-enabled node pools are needed. Traditionally, this required manual installation and management of NVIDIA drivers and plugins—particularly tricky in air-gapped settings. Microsoft now offers a managed GPU node pool preview in AKS, with all NVIDIA prerequisites pre-installed and lifecycle-managed.

- Use Azure CLI (see [Managed GPU Node Pools](https://learn.microsoft.com/en-us/azure/aks/aks-managed-gpu-nodes?tabs=add-ubuntu-gpu-node-pool)) to deploy GPU nodes.
- For full driver/version control, follow NVIDIA's guidance to deploy GPU Operator in air-gapped mode.

## Implementation Example

See [aks-air-gap-vllm-deployment](https://github.com/mocelj/aks-air-gap-vllm-deployment) GitHub repository for references and detailed implementation scripts.

### Baking Model Weights (Example Command)

```bash
az acr build \
  --registry <ACR_NAME> \
  --image llama3-vllm-fat:8b-instruct \
  --build-arg HF_TOKEN=$HF_TOKEN \
  .
```

### Pre-Downloading Artifacts to NFS Share

- Download containers/images and model artifacts using a jump box VM.
- Copy downloaded weights to a shared NFS via `rsync`.
- When deploying, point NVIDIA NIM microservice to local folder.

### Testing the Deployment

- Retrieve internal service IP using `kubectl get svc ...`.
- Test endpoint via curl from within the private network or using a VPN connection.
- Example curl command:

```bash
curl -X POST "http://${svc_ip}:8000/v1/chat/completions" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{ "messages": [...], "model": "meta/llama3-8b-instruct", ... }'
```

## Security Considerations

- Outbound access is strictly disabled in air-gapped clusters, mitigating exfiltration risks.
- Artifact caching and NFS shares ensure compliance with data residency and isolation requirements without sacrificing model or runtime flexibility.

## Conclusion

Deploying advanced AI solutions in strictly isolated Azure environments requires careful orchestration of network, storage, and deployment strategies. By following the detailed approaches demonstrated here, organizations with regulatory or security needs (e.g., finance, public sector) can confidently run powerful LLM inferencing workloads in Azure while minimizing attack surfaces.

---
For full step-by-step examples and scripts, refer to the author's GitHub: [https://github.com/mocelj/aks-air-gap-vllm-deployment](https://github.com/mocelj/aks-air-gap-vllm-deployment)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/ai-inferencing-in-air-gapped-environments/ba-p/4498594)
