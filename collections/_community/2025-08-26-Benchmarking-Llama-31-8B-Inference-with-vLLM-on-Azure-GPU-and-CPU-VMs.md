---
layout: post
title: Benchmarking Llama 3.1 8B Inference with vLLM on Azure GPU and CPU VMs
author: CormacGarvey
canonical_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/inference-performance-of-llama-3-1-8b-using-vllm-across-various/ba-p/4448420
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-08-26 22:22:59 +00:00
permalink: /ai/community/Benchmarking-Llama-31-8B-Inference-with-vLLM-on-Azure-GPU-and-CPU-VMs
tags:
- A100
- AI
- AI Infrastructure
- AVX512
- Azure
- Azure ND Series
- Benchmarking
- Community
- Cost Efficiency
- CPU VM
- CUDA
- Hugging Face
- Inference Benchmark
- KV Cache
- Llama 3.1 8B
- NVIDIA H100
- NVIDIA H200
- Prompt Latency
- PyTorch
- Token Throughput
- Vllm
section_names:
- ai
- azure
---
Cormac Garvey evaluates the inference performance and cost-efficiency of Llama 3.1 8B using vLLM across Azure GPU and CPU virtual machines, offering actionable benchmarks and deployment strategies for enterprise AI workloads.<!--excerpt_end-->

# Benchmarking Llama 3.1 8B Inference with vLLM on Azure GPU and CPU VMs

## Introduction

This report presents a comparative analysis of the inference performance of the Llama 3.1 8B large language model using vLLM, evaluated across a range of Azure ND-series GPU and CPU virtual machines. It expands on previous work by benchmarking not only throughput and latency but also the cost-efficiency associated with deploying large language models (LLMs) in enterprise settings.

Key focus areas include:

- Performance across chat, document classification, and code generation AI workloads
- Resource utilization (particularly KV cache effectiveness)
- Cost per token metrics, based on Azure's region-specific pricing

## Benchmark Environment

**Benchmarks Used:**

- *Tool:* Hugging Face Inference Benchmarker
- *Profiles:* Chat (share_gpt_turns.json), Classification (classification.json), Code Generation (github_code.json)
- *Model:* meta-llama/Llama-3.1-8B-Instruct (FP16 precision, 14.9 GiB)
- *Inference Engine:* vLLM with tuned parameters (e.g., gpu_memory_utilization=0.9, max_num_seqs=1024, chunked prefill and prefix caching enabled)

**Hardware:**

- GPUs: ND-H100-v5, ND-H200-v5, HD-A100-v4 (both 80GB and 40GB) on HPC Ubuntu 22.04, PyTorch 2.7.0 + CUDA 12.8
- CPUs: HPC-class Ubuntu 22.02 VMs

## Results Summary

### Throughput & Latency

- **H200 GPU:** Top performance across all workloads (highest prompt/generation throughput).
- **H100 GPU:** Close second, especially strong on classification and code generation.
- **A100/A100_40G:** Lagging, especially in classification (↓ throughput due to memory and cache constraints).

### KV Cache Analysis

- **H200/H100:** High hit rates (up to 99% in classification), efficient cache use, minimal request queuing except for code generation (where hit rates drop).
- **A100_40G:** High cache usage, very low hit rates for classification and code gen; higher server queuing observed.

### Cost Efficiency

- **Chat workloads:** A100 40G best value.
- **Classification:** H200 shines.
- **Code Generation:** H100 delivers optimum efficiency.

### CPU vs. GPU

- CPUs struggle to deliver acceptable throughput or latency for Llama 3.1 8B. Even advanced CPU VMs (HB176-96_v4) are an order of magnitude slower than GPUs. Only small models (≤1B parameters) may be feasible on CPU for light workloads.

## Optimization Tips

- Enable AVX512 support on CPU VMs if available
- Fit model on a single socket if possible, or use tensor parallelism for split inference
- Use CPU core/thread pinning to optimize vLLM independent thread performance
- Allocate sufficient CPU memory for KVCache (important for larger models)

## Conclusion

Hardware choice on Azure directly impacts both AI inference speed and cost. The H200 GPU is the overall top performer, followed by the H100, while the A100/A100_40G provide budget alternatives mainly for chat-type tasks. CPU VMs are currently not viable for high-throughput LLM inference. Practitioners planning enterprise AI deployments on Azure should match GPU choice to their workload's performance and cost needs.

## References and Resources

- Hugging Face Inference Benchmarker: https://github.com/huggingface/inference-benchmarker
- Llama 3.1 8B Model: https://huggingface.co/meta-llama/Llama-3.1-8B-Instruct
- vLLM Engine: https://github.com/vllm-project/vllm
- Azure ND-Series GPU Docs: https://learn.microsoft.com/en-us/azure/virtual-machines/nd-series
- Azure Pricing Calculator: https://azure.microsoft.com/en-us/pricing/calculator
- CPU vLLM Install: https://docs.vllm.ai/en/latest/getting_started/installation/cpu.html

## Appendix: vLLM on CPU VMs

- Clone: `git clone https://github.com/vllm-project/vllm.git vllm_source`
- Adjust Dockerfiles as needed (see content for ENTRYPOINT details)
- Build with AVX512 flags and required Docker targets
- Launch vLLM serve/benchmark with proper environment variables (kv cache, thread binding, Hugging Face token, etc.)
- See full Docker commands and environment variables within the appendix details.

---

For deeper graphs, benchmarks datasets, and full tuning parameters, refer to the links above or the original evaluation content.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/inference-performance-of-llama-3-1-8b-using-vllm-across-various/ba-p/4448420)
