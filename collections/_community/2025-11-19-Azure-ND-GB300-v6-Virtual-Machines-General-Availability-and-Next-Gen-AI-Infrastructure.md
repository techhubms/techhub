---
layout: "post"
title: "Azure ND GB300 v6 Virtual Machines: General Availability and Next-Gen AI Infrastructure"
description: "This announcement details the general availability of Azure ND GB300 v6 virtual machines, which leverage NVIDIA GB300 NVL72 GPUs and next-generation networking to deliver exceptional performance for large-scale AI and machine learning workloads. The blog provides benchmark results, architecture specifics, and guidance on utilizing Azure services like CycleCloud, Batch, and Kubernetes for high-performance AI training and inference at scale."
author: "Nitin_Nagarkatte"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-nd-gb300-v6-now-generally-available-hyper-optimized-for/ba-p/4469475"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-19 01:13:53 +00:00
permalink: "/2025-11-19-Azure-ND-GB300-v6-Virtual-Machines-General-Availability-and-Next-Gen-AI-Infrastructure.html"
categories: ["AI", "Azure", "ML"]
tags: ["Agentic AI", "AI", "AI Infrastructure", "AKS", "Azure", "Azure Batch", "Azure CycleCloud", "Azure ND GB300 V6", "Benchmarking", "Cloud AI", "Community", "FP4 Tensor Core", "GPU Virtual Machines", "High Bandwidth", "HPC", "InfiniBand", "Large Model Training", "Llama 2", "ML", "Multimodal AI", "NVIDIA GB300 NVL72", "NVLINK", "Slurm"]
tags_normalized: ["agentic ai", "ai", "ai infrastructure", "aks", "azure", "azure batch", "azure cyclecloud", "azure nd gb300 v6", "benchmarking", "cloud ai", "community", "fp4 tensor core", "gpu virtual machines", "high bandwidth", "hpc", "infiniband", "large model training", "llama 2", "ml", "multimodal ai", "nvidia gb300 nvl72", "nvlink", "slurm"]
---

Nitin_Nagarkatte announces the general availability of Azure ND GB300 v6 virtual machines, highlighting their next-generation AI infrastructure powered by NVIDIA GPUs for advanced model training and large-scale inference.<!--excerpt_end-->

# Azure ND GB300 v6 Virtual Machines: General Availability and Next-Gen AI Infrastructure

Azure has launched the ND GB300 v6 VM series, marking a major advance in cloud-based AI and machine learning infrastructure. These VMs rely on NVIDIA GB300 NVL72 GPUs and advanced InfiniBand networks, designed to enable frontier model training, large-scale inference, multimodal reasoning, and agentic AI workloads.

## Key Highlights

- **Scale**: Tens of thousands of GB300 GPUs deployed for production, with plans for further expansion.
- **Performance**: Trillion-parameter models supported; 1.1 million tokens/sec achieved for Llama 2 70B inference (27% faster than previous gen).
- **Rack Design**: Each rack contains 18 VMs (72 GPUs), using NVLINK for high-speed interconnects. Each VM: 2 NVIDIA Grace CPUs, 4 Blackwell Ultra GPUs.
- **Bandwidth & Memory**:
  - 800 Gb/s per GPU cross-rack InfiniBand
  - 130 TB/sec NVLINK within rack
  - 37TB fast memory per rack (~20TB HBM3e, ~17TB LPDDR)
  - Up to 1,440 PFLOPS FP4 Tensor Core performance
- **Unified Infrastructure**: NVLINK and InfiniBand enable efficient memory pooling and ultra-low latency compute.

## Azure AI Platform Integration

Azure supports end-to-end AI workflows on GB300 VMs, with services like:

- **Azure CycleCloud**: For managing HPC and AI cluster setup at scale, including Slurm integration and dynamic resource administration.
- **Azure Batch**: Handles millions of parallel tasks cost-effectively.
- **Azure Kubernetes Service (AKS)**: Enables rapid deployment and management of containerized AI workloads with platform-specific optimizations.

## Use Cases

- Training of trillion-parameter models
- Long-context/multimodal AI inference
- Agentic model deployment and research
- Large-scale, cloud-native AI and ML workflows

## Benchmarks & Technical Resources

- [GA announcement & technical details](https://azure.microsoft.com/en-us/blog/microsoft-azure-delivers-the-first-large-scale-cluster-with-nvidia-gb300-nvl72-for-openai-workloads/)
- [Performance benchmarks: Million-token barrier](https://techcommunity.microsoft.com/blog/azurehighperformancecomputingblog/breaking-the-million-token-barrier-the-technical-achievement-of-azure-nd-gb300-v/4466080)
- [NVIDIA blog: Azure’s GB300 NVL72 cluster](https://blogs.nvidia.com/blog/microsoft-azure-worlds-first-gb300-nvl72-supercomputing-cluster-openai/)
- [Azure VM sizes overview](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/gpu-accelerated/nd-series)

## Getting Started

Organizations can leverage Azure's suite of services for hassle-free scaling and management of AI and ML workloads, whether using Kubernetes or custom cluster setups. Azure's platform supports deep learning, agentic, and multimodal use cases, all on hyper-optimized infrastructure.

---

*Author: Nitin_Nagarkatte*

> Azure ND GB300 v6 stands as Microsoft's most powerful AI VM offering to date, opening possibilities for unprecedented throughput in advanced AI and ML workflows.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-nd-gb300-v6-now-generally-available-hyper-optimized-for/ba-p/4469475)
