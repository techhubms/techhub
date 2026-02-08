---
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-of-llama-3-1-8b-ai-inference-using-vllm-on-nd-h100/ba-p/4448355
title: Benchmarking Llama 3.1 8B AI Inference on Azure ND-H100-v5 with vLLM
author: CormacGarvey
feed_name: Microsoft Tech Community
date: 2025-08-26 17:02:48 +00:00
tags:
- AI Inference
- Azure ND H100 V5
- Dynamo Inference Framework
- FP8 Quantization
- GPU Deployment
- HuggingFace
- KV Cache
- Large Language Models
- Llama 3.1 8B
- PagedAttention
- Performance Benchmarking
- Python
- Tensor Parallelism
- Token Throughput
- Vllm
- AI
- Azure
- ML
- Community
section_names:
- ai
- azure
- ml
primary_section: ai
---
Cormac Garvey offers an in-depth benchmarking study of Llama 3.1 8B model inference using vLLM on Azure ND-H100-v5 GPUs. The article explains AI inference stages, optimization techniques, and reports on throughput, latency, and deployment tips for enterprise AI practitioners.<!--excerpt_end-->

# Benchmarking Llama 3.1 8B AI Inference on Azure ND-H100-v5 with vLLM

## Introduction

The rapid advancement of large language models (LLMs) like Llama 3.1 8B has brought new challenges and opportunities to the field of AI. Deployment at scale requires significant compute resources, but smaller, efficient models are becoming more practical for enterprises. This article benchmarks Llama 3.1 8B’s inference performance on Azure’s ND-H100 v5 GPU infrastructure using vLLM and analyzes techniques for maximizing throughput and minimizing latency.

## AI Inference Architecture

Inference in transformer-based LLMs consists of two main stages:

- **Prefill (Compute-Bound Initialization):** Tokenizes and processes input prompts, filling the key-value (KV) cache using GPU compute throughput.
- **Decode (Memory-Bound Token Generation):** Generates tokens one at a time, relying on fast KV cache access. Cache efficiency is critical for low latency and high throughput.

### Optimization Techniques

- **KV Cache Management:** Use of vLLM’s PagedAttention for dynamic HBM memory allocation and high cache hit rates.
- **Batching/Scheduling:** Parameters like `MAX_NUM_SEQS` and `MAX_NUM_BATCHED_TOKENS` control how requests are grouped for efficient GPU utilization.
- **Weight/Activation Quantization:** FP8 quantization enables more memory usage for models or larger KV cache, offering performance gains via higher FLOPS units.
- **Parallelism:** Tensor, pipeline, expert, and data parallel strategies split models across GPUs to maximize resource use.
- **Speculative Decoding:** Predicts future tokens to reduce compute passes.
- **Prefill/Decode Decoupling:** Assigns separate resources to each phase for parallel processing, as seen with vLLM and NVIDIA Dynamo.

## Benchmarking Environment

### Setup

- **Models Evaluated:** meta-llama/Llama-3.1-8B-Instruct (FP16), neuralmagic/Meta-Llama-3.1-8B-Instruct-FP8, nvidia/Llama-3.1-8B-Instruct-FP8.
- **Azure Hardware:** ND-H100-v5 (8x H100 GPUs).
- **vLLM/Dynamo Parameters:** Configured for high utilization (`gpu_memory_utilization`=0.9, `max_num_seqs`=1024, `max_num_batched_tokens`=8192/16384).

### Inference Profiles

- **Chat:** Q&A format.
- **Classification:** Document summarization.
- **Code Generation:** Automated code writing and function creation.

## Results

- **Throughput (tokens/sec):** Varied by profile and configuration; code generation profile is more compute-bound in prefill, less cache-efficient in decode.
- **KV Cache Usage:** Code generation uses the KV cache least, leading to higher decode latency.
- **Parallelism Effect:** Loading full model per GPU provides higher throughput over tensor parallelism on this hardware/model size.
- **FP8 Quantization:** Improves code-generation throughput by ~38% on a single GPU when memory headroom exists.
- **Dynamo vs vLLM:** For modest model sizes on single-node clusters, vLLM's traditional aggregated approach was faster than distributed Dynamo due to overhead tradeoffs.

## Key Takeaways for AI Engineers

- Understand your LLM’s bottleneck: prefill (compute) or decode (memory). Tailor GPU assignment and KV cache policies accordingly.
- For models like Llama 3.1 8B that fit on a single GPU, avoid over-parallelizing across devices.
- Profile different inference workloads—chat, classification, and code generation each stress pipelines differently.
- FP8 quantization boosts throughput for compute-limited scenarios when memory/caching isn’t a bottleneck.
- Modern inference engines like vLLM and Dynamo enable architectural flexibility but may add overhead for small to medium setups.

## Installation and Configuration

### Tools/Dependencies

- Rust (for inference-benchmarker)
- Python 3.10, uv (Python environment manager)
- vLLM (`pip install vllm`), FlashInfer, NVIDIA Dynamo

### Deployment Scripts

Detailed bash and Python scripts are provided for:

- Setting up vLLM and its dependencies
- Launching inference servers on specified models and GPUs
- Running Hugging Face inference benchmarks
- Building and operating NVIDIA Dynamo and its supporting services

## Conclusion

Efficient LLM inference on Azure requires understanding both hardware and software specs. Llama 3.1 8B is highly performant on ND-H100-v5 with optimal vLLM configuration—in particular, maximizing throughput by careful batch/parallel settings and leveraging quantization. As model and infrastructure sizes scale, trade-offs between architectural complexity and performance gains should be reassessed.

**References:**

- vLLM Documentation: <https://docs.vllm.ai/en/v0.8.1/index.html>
- NVIDIA Tech Blog on LLM Optimization: <https://developer.nvidia.com/blog/mastering-llm-techniques-inference-optimization/>
- HuggingFace Inference Benchmarker: <https://github.com/huggingface/inference-benchmarker/>
- Meta Llama Model Utilities: <https://github.com/meta-llama/llama-models>
- Azure ND-H100-v5 series: <https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/gpu-accelerated/ndh100v5-series?tabs=sizebasic>

## Appendix: Setup Guides

Setup instructions for Rust, Python, vLLM, FlashInfer, Dynamo, and supporting services are included in the article for immediate reproducibility.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-of-llama-3-1-8b-ai-inference-using-vllm-on-nd-h100/ba-p/4448355)
