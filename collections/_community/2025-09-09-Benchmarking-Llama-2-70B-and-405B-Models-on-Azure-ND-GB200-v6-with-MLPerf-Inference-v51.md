---
layout: post
title: Benchmarking Llama 2 70B and 405B Models on Azure ND GB200 v6 with MLPerf Inference v5.1
author: Mark_Gitau
canonical_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/a-quick-guide-to-benchmarking-ai-models-on-azure-llama-405b-and/ba-p/4452192
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-09 15:00:00 +00:00
permalink: /ai/community/Benchmarking-Llama-2-70B-and-405B-Models-on-Azure-ND-GB200-v6-with-MLPerf-Inference-v51
tags:
- AI Benchmarking
- Azure HPC
- Azure ND GB200 V6
- Azure Virtual Machines
- Containerization
- GPU Acceleration
- Large Language Models
- Llama 2 70B
- Llama 3.1 405B
- ML Workflow
- MLCommons
- MLPerf Inference
- NVIDIA Blackwell B200 GPU
- NVIDIA Grace CPU
- Performance Testing
section_names:
- ai
- azure
- ml
---
Mark Gitau explains how to benchmark Llama 2 70B and 405B models using MLPerf Inference v5.1 on Azure ND GB200 v6 VMs, detailing setup, performance highlights, and step-by-step replication guidance.<!--excerpt_end-->

# Benchmarking Llama 2 70B and 405B Models on Azure ND GB200 v6 with MLPerf Inference v5.1

**Author:** Mark Gitau (Software Engineer)

## Introduction

This guide covers Azure's MLPerf Inference v5.1 benchmarking results for the latest ND GB200 v6 virtual machines. Powered by two NVIDIA Grace CPUs and four Blackwell B200 GPUs, these VMs deliver high-performance environments for testing and deploying large language models like Llama 2 70B and Llama 3.1 405B.

### Highlights

- **Llama 2 70B**: Azure achieved 52,000 tokens/s offline on a single ND GB200 v6 VM (an 8% performance increase from previous records), scaling up to approximately 937,098 tokens/s with a full NVL72 rack.
- **Llama 3.1 405B**: Azure results matched the top global submitters with 847 tokens/s, showing parity with both leading cloud and on-premises systems.

## Step-by-Step Benchmark Replication on Azure

### Prerequisites

- An Azure account and access to ND GB200 v6-series (single node) VMs.

### Environment Setup

1. **Deploy VM**: Launch a new ND GB200 v6 VM from the [Azure portal](https://ms.portal.azure.com/).
2. **Directory Preparation**: Create a working directory on an NVMe mount.

   ```bash
   export MLPERF_SCRATCH_PATH=/mnt/nvme/mlperf
   mkdir -p $MLPERF_SCRATCH_PATH/data $MLPERF_SCRATCH_PATH/models $MLPERF_SCRATCH_PATH/preprocessed_data
   ```

3. **Clone MLPerf Repo**:

   ```bash
git clone https://github.com/mlcommons/submissions_inference_v5.1 $MLPERF_SCRATCH_PATH
   ```

### Downloading Models and Datasets

- **Models:**
    - [Llama 2 70B](https://github.com/mlcommons/inference/tree/master/language/llama2-70b#get-model)
    - [Llama 3.1 405B](https://github.com/mlcommons/inference/tree/master/language/llama3.1-405b#get-model)
- **Datasets:**
    - [Llama 2 70B datasets](https://github.com/mlcommons/inference/tree/master/language/llama2-70b#get-dataset)
    - [Llama 3.1 405B datasets](https://github.com/mlcommons/inference/tree/master/language/llama3.1-405b#get-dataset)
    - [Preparing Llama 2 70B datasets](https://github.com/mlcommons/submissions_inference_v5.1/tree/main/closed/NVIDIA/code/llama2-70b/tensorrt#download-and-prepare-data)
    - [Preparing Llama 3.1 405B datasets](https://github.com/mlcommons/submissions_inference_v5.1/tree/main/closed/NVIDIA/code/llama3.1-405b/tensorrt#download-and-prepare-data)

### Build and Run Benchmarks

1. **Export variables:**

   ```bash
   export SUBMITTER=Azure SYSTEM_NAME=ND_GB200_v6
   ```

2. **Build MLPerf Container:**
   - Navigate to the closed/Azure directory and execute:

     ```bash
     make prebuild
     make build
     ```

3. **Build Engines and Run Benchmarks:**
   - Build engines for Llama models:

     ```bash
     make generate_engines RUN_ARGS="--benchmarks=llama2-70b,llama3.1-405b --scenarios=offline,server"
     ```

   - Execute benchmarks:

     ```bash
     make run_harness RUN_ARGS="--benchmarks=llama2-70b,llama3.1-405b --scenarios=offline,server"
     ```

## About MLPerf and MLCommons

MLPerf is a set of industry benchmarks from MLCommons, an open engineering consortium, designed to deliver unbiased evaluations of both training and inference performance for AI hardware, software, and cloud services. These benchmarks simulate realistic compute-intensive AI workloads for informed technology assessment and selection.

---

For more details, visit official guides and MLPerf documentation or Azure's HPC blog.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/a-quick-guide-to-benchmarking-ai-models-on-azure-llama-405b-and/ba-p/4452192)
