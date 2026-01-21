---
external_url: https://techcommunity.microsoft.com/t5/azure-arc-blog/aks-enabled-by-azure-arc-powering-ai-applications-from-cloud-to/ba-p/4471511
title: 'AKS Enabled by Azure Arc: Powering Hybrid AI Applications from Cloud to Edge'
author: SchumannGE
feed_name: Microsoft Tech Community
date: 2025-11-20 00:36:19 +00:00
tags:
- AI Foundry Local
- AI Model Deployment
- AKS
- AKS Container Apps
- Azure Arc
- Azure Container Storage
- Azure Monitor Pipeline
- Bare Metal Kubernetes
- Cluster High Availability
- Data Sovereignty
- Disconnected Operations
- Edge Computing
- GPU Acceleration
- Hybrid Cloud
- KAITO
- Key Vault Extension
- KMS V2
- Kubernetes Security
- RAG
- Retrieval Augmented Generation
- Telecom
- Workload Identity Federation
section_names:
- ai
- azure
- devops
- ml
- security
---
SchumannGE details the latest AKS Arc developments from Ignite 2025, focusing on hybrid Kubernetes, AI model deployment, enhanced edge and disconnected support, and new security features for the AI-enabled enterprise.<!--excerpt_end-->

# AKS Enabled by Azure Arc: Powering Hybrid AI Applications from Cloud to Edge

**Author: SchumannGE**

Microsoft Ignite 2025 showcased significant updates to Azure Kubernetes Service (AKS) enabled by Azure Arc, marking a leap forward in hybrid, edge, and AI workload capabilities. This summary distills the key announcements, technical improvements, and their implications for organizations building and scaling AI-driven solutions across diverse environments.

## Key Announcements at Ignite 2025

- **AKS on Azure Local Disconnected Operations (Public Preview):** Run AKS clusters fully offline in sovereign or regulated environments, with asynchronous synchronization for updates.
- **AKS Local Small Form Factor Bare-Metal (Private Preview):** Deploy AKS to compact edge hardware for industrial, retail, and factory scenarios, supporting optional GPU acceleration.
- **AKS on Windows Server & Azure Local Medium Improvements:** Enhanced reliability, expanded hardware support, improved image management, certificate handling, and error detection.
- **2-Node High Availability:** Makes robust, production-grade AKS deployments feasible in space- and budget-constrained edge sites.
- **AI Foundry Local Integration:** Provides offline AI model catalog access, local fine-tuning (including LoRA/QLoRA), and optimized execution for GPUs, NPUs, and CPUs directly in customer environments.
- **KAITO for Model Serving:** Automates packaging, optimization, and deployment of AI models (ONNX, Hugging Face, or custom) for multi-modal inferencing across cloud, datacenter, and edge.
- **Edge RAG (Retrieval-Augmented Generation):** Enables compliant, low-latency generative AI grounded in on-premises data via Azure Arc extensions.
- **AKS Container Apps Public Preview:** Edge-native microservices and event-driven AI endpoints graduating to production-readiness on Azure Local.
- **Arc-enabled Azure Monitor Pipeline:** Enhanced telemetry for hybrid/disconnected sites with local caching and later synchronization.
- **Security Improvements:** KMS v2 for secrets encryption at rest, Workload Identity Federation for secret-less Edge identity, integrated Key Vault extension for secure storage.
- **Expanded GPU and Storage Support:** NVIDIA RTX 6000 Ada, L-series GPU previews, GPU partitioning, and persistent storage enhancements for AI/ML/data workloads.

## Technical Deep Dive

### Disconnected and Hybrid Operations

Enterprises can now deploy, manage, and upgrade AKS clusters without continuous Azure connectivity. This benefits regulated and air-gapped scenarios, supporting critical workloads in isolated manufacturing, defense, and telecom environments. Temporary sync enables updates without internet persistency.

### AI Development at the Edge

AKS Arc establishes itself as Azure's platform for distributed hybrid AI workloads:

- **AI Foundry Local** enables local training and fine-tuning of foundation models, delivering a complete offline AI dev loop. Models can be packaged for AKS clusters, even in air-gapped environments.
- **KAITO** offers streamlined deployment for ONNX, Hugging Face, and custom models, optimized for various hardware.
- **Edge RAG** supports end-to-end generative AI using proprietary on-premises data, maintaining data sovereignty and reducing cloud latency.
- **Expanded GPU Support** helps organizations run resource-intensive AI/ML inferencing and training where data lives, leveraging new GPU types and partitioning techniques.

### Application and DevOps Enhancements

- **AKS Container Apps** simplify microservices and event-driven deployments at the edge, using existing Azure development models.
- **Arc Gateway** streamlines hybrid cluster connectivity and onboarding, reducing firewall configuration complexity.
- **Operational improvements**: Enhanced monitoring, secret management, and governance tools for distributed fleets.

### Security Improvements

- **KMS v2** delivers better at-rest encryption for Kubernetes secrets, aligning with Azure's native practices.
- **Identity Federation & Key Vault Extensibility** improve cluster workload security without explicit secrets. Workloads can use federated identities and integrated Key Vault storage, ideal for complex regulatory contexts.

### Observability and Storage at Scale

- **Azure Monitor Pipeline** supports disconnected operations, capturing and syncing telemetry efficiently.
- **Azure Container Storage** supports AI/ML workloads' storage needs, with performance improvements for vector stores and embedding caches.

## Conclusion

Microsoft's 2025 Ignite announcements cement AKS enabled by Azure Arc as the most versatile and secure platform for running modern—especially AI-powered—applications across cloud, on-premises, and edge. The platform now supports offline development, robust security, GPU-powered inferencing, and easy, consistent management, enabling organizations to build, deploy, and operate next-generation AI and cloud-native solutions wherever needed.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/aks-enabled-by-azure-arc-powering-ai-applications-from-cloud-to/ba-p/4471511)
