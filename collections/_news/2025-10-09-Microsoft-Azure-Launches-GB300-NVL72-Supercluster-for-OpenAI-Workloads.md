---
external_url: https://azure.microsoft.com/en-us/blog/microsoft-azure-delivers-the-first-large-scale-cluster-with-nvidia-gb300-nvl72-for-openai-workloads/
title: Microsoft Azure Launches GB300 NVL72 Supercluster for OpenAI Workloads
author: Rani Borkar and Nidhi Chappell
feed_name: The Azure Blog
date: 2025-10-09 16:00:00 +00:00
tags:
- AI Datacenter
- AI Infrastructure
- Blackwell Ultra GPUs
- Collective Operations
- Compute
- Data Center Cooling
- Datacenter
- FP4 Performance
- Frontier AI
- Grace CPU
- InfiniBand
- Microsoft Azure
- ND GB300 V6
- NVIDIA GB300
- NVIDIA Quantum X800
- NVL72
- OpenAI
- Performance Benchmarking
- Supercomputing
- Tensor Core
- Virtual Machines
section_names:
- ai
- azure
primary_section: ai
---
Rani Borkar and Nidhi Chappell introduce the Azure GB300 NVL72 cluster, detailing how a partnership with NVIDIA delivers a supercomputing platform that sets new standards for large-scale AI workloads and model training.<!--excerpt_end-->

# Microsoft Azure Launches GB300 NVL72 Supercluster for OpenAI Workloads

Microsoft Azure has deployed the world's first large-scale production cluster comprising more than 4,600 NVIDIA GB300 NVL72 GPUs, utilizing NVIDIA Blackwell Ultra architecture. This milestone enables rapid model training and inference for multitrillion-parameter AI models, supporting demanding workloads such as those required by OpenAI.

## Cluster Overview

- **Hardware**: 4,600+ NVIDIA GB300 NVL72 GPUs, Blackwell Ultra architecture
- **Networking**: Quantum-X800 InfiniBand, up to 800 Gbp/s per GPU
- **Rack Configuration**: Each rack includes 18 virtual machines and 72 GPUs, powered by 36 NVIDIA Grace CPUs, 37TB fast memory, and NVLink bandwidth of 130TB/s
- **Performance**: Up to 1,440 PFLOPS of FP4 Tensor Core compute per rack

## Innovations in Infrastructure

- **Networking**: The cluster uses a fat-tree, non-blocking architecture with Quantum-X800 InfiniBand, enabling scalable training across tens of thousands of GPUs.
- **Memory & Bandwidth**: NVLink and NVSwitch technologies reduce intra-rack bottlenecks, improving throughput and responsiveness for agentic and multimodal AI systems.
- **Cooling & Power**: Advanced heat exchangers and dynamic power distribution maintain reliability in densely packed, high-performance clusters.
- **Software Stack**: Azure's orchestration, scheduling, and storage solutions have been optimized for supercomputing scale, assuring high efficiency and reliability.

## Impact for AI Workloads

Azure customers gain access to infrastructure capable of training and serving models with hundreds of trillions of parameters in weeks instead of months. This accelerates research, development, and production deployment of advanced AI systems, supported by optimized hardware, network, and software collaboration between Microsoft and NVIDIA.

## Looking Ahead

As Microsoft continues global rollout of GB300-based clusters, further performance benchmarks and technical updates will be shared. The ND GB300 v6 VM class sets a new standard in industry AI infrastructure, reinforcing Azure's position in supporting cutting-edge frontier AI.

**For more information:**

- [Microsoft Azure AI Infrastructure Solutions](https://azure.microsoft.com/solutions/high-performance-computing/ai-infrastructure/)
- [NVIDIA Blog: Azure's GB300 NVL72 Supercluster](https://blogs.nvidia.com/blog/microsoft-azure-worlds-first-gb300-nvl72-supercomputing-cluster-openai/)
- [Inside Azure’s AI Datacenters](https://blogs.microsoft.com/blog/2025/09/18/inside-the-worlds-most-powerful-ai-datacenter/)

*Authors: Rani Borkar, Nidhi Chappell*

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/microsoft-azure-delivers-the-first-large-scale-cluster-with-nvidia-gb300-nvl72-for-openai-workloads/)
