---
layout: post
title: 'Performance Analysis: DeepSeek R1 Inference with vLLM on Azure ND-H100-v5'
author: CormacGarvey
canonical_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-analysis-of-deepseek-r1-ai-inference-using-vllm-on/ba-p/4449351
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-08-28 22:13:32 +00:00
permalink: /ai/community/Performance-Analysis-DeepSeek-R1-Inference-with-vLLM-on-Azure-ND-H100-v5
tags:
- AI
- AI Infrastructure
- Azure
- Azure ND H100 V5
- Chain Of Thought
- Community
- Cost Analysis
- DeepSeek R1
- FP8 Precision
- GPU Utilization
- HPC
- Inference Benchmarking
- InfiniBand
- Latency Analysis
- Model Deployment
- NVIDIA H100
- NVLink
- Token Throughput
- Vllm
section_names:
- ai
- azure
---
CormacGarvey examines the deployment and benchmarking of the DeepSeek R1 reasoning model on Azure ND_H100_v5 nodes using vLLM, providing practical insights into infrastructure demands and performance.<!--excerpt_end-->

# Performance Analysis: DeepSeek R1 Inference with vLLM on Azure ND-H100-v5

## Introduction

The DeepSeek R1 model heralds a new era for large-scale reasoning in AI, but its deployment is far from trivial. This article guides practitioners through benchmark results, infrastructure demands, and cost-performance trade-offs for running DeepSeek R1 on Azure ND_H100_v5 hardware.

## Benchmark Environment

- **Hardware:** 2 Azure ND_H100_v5 nodes, each with 8 NVIDIA H100 GPUs (total 16 GPUs)
- **Interconnect:** InfiniBand and NVLink for optimal memory bandwidth and low latency
- **Inference Server:** vLLM used for scalable API-based model serving
- **Benchmarking Tool:** vLLM bench with the AI-MO/aimo-validation-aime dataset from Hugging Face

## Key Results

### Reasoning Model Output

- DeepSeek R1 produces extensive chain-of-thought reasoning, demonstrated by the generation of 1162 completion tokens on a simple numeric comparison prompt, compared to only 37 tokens by Llama 3.1 8B.
- For simple prompts, DeepSeek R1's detailed reasoning is often excessive and incurs higher costs and latency.

### Throughput, Latency, and Cost

- **Throughput:** DeepSeek R1 generates tokens much more slowly—about 54 times slower than Llama 3.1 8B on comparable GPU hardware.
- **Latency:** Both TTFT (Time-To-First-Token) and ITL (Inter-Token Latency) are significantly higher (~6x and ~3x, respectively, over smaller models).
- **Cost:** Token generation cost for DeepSeek R1 is approximately 54x that of Llama 3.1 8B (on 16 H100s), and about 34x higher on 8 H200s.
- **Resource Utilization:** Actual network usage is modest (~14% of InfiniBand bandwidth; <1% of NVLink).

### Infrastructure Setup

To deploy DeepSeek R1 for inference:

- Install vLLM and FlashInfer, with required CUDA and Ninja dependencies.
- Configure environment variables for each node, ensuring vLLM's expert/data parallelism and backend flags are set for low-latency or throughput-optimized runs.
- Scripts are provided for node configuration as well as for benchmarking via vLLM bench.

## Analysis and Recommendations

- DeepSeek R1 is well-suited for applications requiring deep, multi-step logical reasoning where model output quality justifies infrastructure cost.
- For standard inference or cost-constrained scenarios, smaller models such as Llama 3.1 8B are greatly more efficient and economical.
- GPU memory bandwidth and compute FLOPS are the primary bottlenecks, not network interconnect.
- Practitioners should reserve R1 deployments for cases where its advanced reasoning is a must-have feature.

## Example: API Usage

```bash
curl http://localhost:8000/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{
    "model": "deepseek-ai/DeepSeek-R1",
    "messages": [{"role": "user", "content": "9.11 and 9.8, which is greater? Explain your reasoning"}]
}'
```

## References

- [DeepSeek R1 on Hugging Face](https://huggingface.co/deepseek-ai/DeepSeek-R1)
- [vLLM GitHub](https://github.com/vllm-project/vllm)
- [Azure ND H100 v5 Documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/nd-h100-v5-series)
- [FlashInfer GitHub](https://github.com/flashinfer-ai/flashinfer)
- [DeepGEMM GitHub](https://github.com/deepseek-ai/DeepGEMM)
- [AI-MO/aimo-validation-aime Dataset](https://huggingface.co/datasets/AI-MO/aimo-validation-aime)

## Appendix: Installation and Configuration

Step-by-step scripts and command sequences for installing dependencies, preparing the environment, and configuring both nodes are included in the article body above.

## Conclusion

DeepSeek R1 is a powerful reasoning LLM, but its resource and cost requirements limit its practicality to use cases demanding the highest reasoning quality. Azure's ND_H100_v5 series provides the necessary infrastructure, but careful cost-benefit analysis is advised. For most projects, lighter-weight models will offer superior efficiency.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-analysis-of-deepseek-r1-ai-inference-using-vllm-on/ba-p/4449351)
