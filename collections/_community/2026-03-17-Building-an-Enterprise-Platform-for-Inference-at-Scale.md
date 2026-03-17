---
author: bobmital
tags:
- AI
- AKS
- Anyscale On Azure
- Azure
- Azure Arc
- Azure Blob Storage
- Azure CNI Powered By Cilium
- Azure Firewall
- Azure GPU SKUs
- Azure Key Vault
- Azure Private Link
- Community
- Continuous Batching
- Data Parallelism
- Disaggregated Inference
- Edge Inference
- GPU Parallelism
- GPU Utilization
- Hybrid Cloud
- Kubernetes RBAC
- KV Cache
- LLM Inference
- Managed Identities
- Micro Segmentation
- Microsoft Entra ID
- MIG Partitioning
- Network Security Groups
- NVLink
- P95 Latency
- Pipeline Parallelism
- Placement Groups
- Private Clusters
- Quantization
- Ray
- Security
- Tensor Parallelism
- Tokens Per GPU Hour
title: Building an Enterprise Platform for Inference at Scale
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-an-enterprise-platform-for-inference-at-scale/ba-p/4498820
date: 2026-03-17 18:28:24 +00:00
feed_name: Microsoft Tech Community
section_names:
- ai
- azure
- security
primary_section: ai
---

bobmital explains how to run large-scale LLM inference on Azure Kubernetes Service (AKS), covering GPU parallelism choices, cloud/edge/hybrid deployment topology, and the security and governance controls (private clusters, Entra ID, Key Vault) needed to make inference production-safe.<!--excerpt_end-->

## Overview

This post (part 3 of a series) focuses on the architectural and platform decisions that make LLM inference optimizations production-safe: how to distribute compute across GPUs/nodes on Azure Kubernetes Service (AKS), where to run deployments (cloud/edge/hybrid), how to meet enterprise security/governance requirements, and which metrics best capture inference economics.

## Architecture decisions

### GPU parallelism strategy on AKS

| Strategy | How it works | When to use | Tradeoff |
| --- | --- | --- | --- |
| Tensor Parallelism | Splits weight matrices within each layer across GPUs (intra-layer sharding); all GPUs participate in every forward pass. | Model exceeds single-GPU memory (e.g., 70B on A100 GPUs once weights, KV cache, runtime overhead are included) | Inter-GPU communication overhead; requires fast interconnects (NVLink on ND-series) — costly to scale beyond a single node without them |
| Pipeline Parallelism | Distributes layers sequentially across nodes, with each stage processing part of the model | Model exceeds single-node GPU memory — typically unquantized deployments beyond ~70–100B depending on node GPU count and memory | Pipeline “bubbles” reduce utilization. Pipeline parallelism is unfriendly to small batches |
| Data Parallelism | Replicates full model across GPUs | Scaling throughput / QPS on AKS node pools | Memory-inefficient (full copy per replica); only strategy that scales throughput linearly |
| Combined | Tensor within node + Pipeline across nodes + Data for throughput scaling | Production at scale on AKS — for any model requiring multi-node deployment, combine TP within each node and PP across nodes | Complexity; standard for large deployments |

Key guidance:

- If quality permits, **quantize first** to fit a single GPU or single node.
  - Avoiding cross-node communication typically gives the best latency and cost profile.

### Orchestrating combined parallelism with Ray (Anyscale on Azure)

Combined parallelism needs careful shard placement, inter-GPU communication management, and scaling that doesn’t break shard assignments.

- The post calls out **Anyscale on Azure** as providing orchestration via **Ray** scheduling primitives.
- Specifically, it uses **placement groups** to:
  - Co-locate tensor-parallel shards within a node
  - Scale data-parallel replicas independently across AKS node pools

Reference links:

- Anyscale on Azure: Powering distributed AI/ML at scale with Azure and Anyscale: https://devblogs.microsoft.com/all-things-azure/powering-distributed-aiml-at-scale-with-azure-and-anyscale/
- Ray: https://www.ray.io/

## Deployment topology

Parallelism decides how GPUs are used inside a deployment; topology decides where the deployment runs.

### Cloud (AKS)

- Emphasizes elastic scaling across **Azure GPU SKUs**:
  - ND GB200-v6, ND H100 v5, NC A100 v4
- Notes that Anyscale on Azure can run managed Ray clusters inside the customer’s AKS environment, with:
  - Azure billing integration
  - Microsoft Entra ID integration
  - Connectivity to Azure storage services

Azure GPU SKU reference: https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/gpu-accelerated/nc-family

### Edge

- Benefits:
  - Ultra-low latency
  - Avoids per-query cloud inference cost
  - Local data residency (e.g., manufacturing, healthcare, retail)

### Hybrid

- Described as the “pragmatic default”:
  - Sensitive data stays local with small quantized models
  - Complex analysis routes to AKS
- Uses **Azure Arc** to extend governance across hybrid deployments.

Azure Arc link (as provided): https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account/search?icid=hybrid-cloud&ef_id=_k_Cj0KCQiA2bTNBhDjARIsAK89wlGUxF3168y9g-Qd2SIKTUceXy3pAjy3nCBgQalDr_rmGhM55mrQtf0aAtYSEALw_wcB_k_&OCID=AIDcmm83ywnuwb_SEM_k_Cj0KCQiA2bTNBhDjARIsAK89wlGUxF3168y9g-Qd2SIKTUceXy3pAjy3nCBgQalDr_rmGhM55mrQtf0aAtYSEALw_wcB&utm_source=google&utm_medium=cpc&utm_campaign=23555152752&utm_adgroup=196337722074&utm_term=kwd-1434042116848&utm_content=797239507669&gad_source=1&gad_campaignid=23555152752&gbraid=0AAAABCZrPIT2I9sJC9M47x8zy9OyhVDzX&gclid=Cj0KCQiA2bTNBhDjARIsAK89wlGUxF3168y9g-Qd2SIKTUceXy3pAjy3nCBgQalDr_rmGhM55mrQtf0aAtYSEALw_wcB

## Enterprise platform: security, compliance, and governance on AKS

The post argues that performance optimizations (quantization, continuous batching, disaggregated inference, MIG partitioning) only matter if the platform meets enterprise security and governance requirements.

### Why self-host inference on AKS

- Keeping inference on AKS can keep prompts, output tokens, KV cache, model weights, and fine-tuning data **inside the customer’s Azure subscription and virtual network**.
- It frames this as addressing data residency/sovereignty concerns that hosted API services can’t handle “by design.”

### Network isolation and access control

- **AKS private clusters**: Kubernetes API server exposed via **Azure Private Link** instead of a public endpoint.
  - AKS security concepts: https://learn.microsoft.com/en-us/azure/aks/concepts-security
- **Traffic restriction and micro-segmentation**:
  - Network Security Groups (NSGs): https://learn.microsoft.com/en-us/azure/aks/operator-best-practices-cluster-isolation
  - Azure Firewall
  - Kubernetes network policies enforced through **Azure CNI powered by Cilium**
- **Identity and secret management**:
  - Microsoft Entra ID integration with Kubernetes RBAC for SSO and group-based roles: https://learn.microsoft.com/en-us/azure/aks/operator-best-practices-cluster-security
  - Managed identities to avoid credentials in application code
  - Azure Key Vault for secrets/certificates/API keys: https://learn.microsoft.com/en-us/azure/aks/concepts-security

### Anyscale on Azure inherits the AKS security stack

- Workloads run inside the customer AKS cluster.
- Calls out inherited controls/integrations:
  - Entra ID authentication
  - Azure Blob Storage connectivity via private endpoints
  - Unified Azure billing
- States there’s no separate Anyscale-controlled infrastructure to audit or secure.

## Metrics that determine profitability

| Metric | What it measures | Why it matters |
| --- | --- | --- |
| Tokens/second/GPU | Raw hardware throughput | Capacity planning on AKS GPU node pools |
| Tokens/GPU-hour | Unit economics | Tokens generated per Azure VM billing hour |
| P95 / P99 latency | Tail latency | Tail experience matters more than averages in production |
| GPU utilization % | Paid vs. used Azure GPU capacity | Low utilization means expensive GPUs sitting idle/underused |
| Output-to-input token ratio | Generation cost ratio | Higher output ratios increase generation time and reduce throughput |
| KV cache hit rate | Context reuse efficiency | Low hit rates increase recomputation, latency, and cost |

Additional point:

- Product design affects inference economics: defaulting to verbose responses increases GPU cycles per request, reducing requests served per hour.

## Conclusion

- “Base model intelligence is increasingly commoditized. Inference efficiency compounds.”
- Teams that manage the accuracy–latency–cost tradeoff deliberately and track tokens per GPU-hour can deploy cheaper, scale faster, and protect margins.

## Series links

- Strategic partnership: Powering Distributed AI/ML at Scale with Azure and Anyscale (All things Azure): https://devblogs.microsoft.com/all-things-azure/powering-distributed-aiml-at-scale-with-azure-and-anyscale/
- Part 1: Inference at Enterprise Scale: Why LLM Inference Is a Capital Allocation Problem: https://techcommunity.microsoft.com/blog/appsonazureblog/inference-at-enterprise-scale-why-llm-inference-is-a-capital-allocation-problem/4498754
- Part 2: The LLM Inference Optimization Stack: A Prioritized Playbook for Enterprise Teams: https://techcommunity.microsoft.com/blog/appsonazureblog/the-llm-inference-optimization-stack-a-prioritized-playbook-for-enterprise-teams/4498818
- Part 3: This post


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-an-enterprise-platform-for-inference-at-scale/ba-p/4498820)

