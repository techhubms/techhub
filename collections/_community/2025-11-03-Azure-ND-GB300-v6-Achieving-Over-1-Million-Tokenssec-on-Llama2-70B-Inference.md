---
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/breaking-the-million-token-barrier-the-technical-achievement-of/ba-p/4466080
title: 'Azure ND GB300 v6: Achieving Over 1 Million Tokens/sec on Llama2 70B Inference'
author: HugoAffaticati
feed_name: Microsoft Tech Community
date: 2025-11-03 17:00:00 +00:00
tags:
- AI Inference
- AI Infrastructure
- Azure ND GB300 V6
- Azure Virtual Machines
- Benchmarking
- Cloud AI
- Containerization
- FP4 Quantization
- GEMM TFLOPS
- GPU Performance
- High Bandwidth Memory
- Llama2 70B
- ML Engineering
- ML Pipeline
- MLCommons
- MLPerf Inference
- NCCL Communication
- NVIDIA Blackwell
- NVLink
- Offline Inference
- Python
- TensorRT LLM
section_names:
- ai
- azure
- ml
---
Hugo Affaticati and Mark Gitau detail Azure ND GB300 v6 VMs' record-breaking throughput for Llama2 70B inference, sharing technical benchmarks and a step-by-step Azure deployment guide.<!--excerpt_end-->

# Azure ND GB300 v6: Achieving Over 1 Million Tokens/sec on Llama2 70B Inference

Authors: Mark Gitau (Software Engineer), Hugo Affaticati (Senior Cloud Infrastructure Engineer)

## Overview

Microsoft Azure's ND GB300 v6 virtual machines, based on NVIDIA's Blackwell architecture, deliver a breakthrough in AI inference throughput. In formal benchmarking, the system achieved a record 1,100,000 tokens/sec on Llama2 70B using MLPerf Inference v5.1, outpacing previous Azure ND GB200 v6 results by 27%.

## Key Hardware and Benchmark Details

- **Cloud Platform:** Microsoft Azure
- **VM Instance SKU:** ND_GB300_v6
- **System Configuration:** 18 x NDv6 VM instances, one NVL72 rack
- **GPU:** 4 x NVIDIA GB300 per VM (72 total)
- **GPU Memory:** 189,471 MiB per GPU
- **GPU Power Limit:** 1,400 Watts
- **Storage:** 14 TB Local NVMe RAID per VM
- **Inference Engine:** NVIDIA TensorRT-LLM
- **Benchmark Harness:** MLCommons MLPerf Inference v5.1 (Offline scenario)
- **Model:** Llama2-70B (FP4 Precision Quantization)

### Performance Metrics (from Table 1)

| Metric                      | Performance (Tokens/Second) |
|-----------------------------|-----------------------------|
| Total Aggregated Throughput | 1,100,948.3                 |
| Maximum Single-Node         | 62,803.9                    |
| Minimum Single-Node         | 57,599.1                    |
| Average Single-Node         | 61,163.8                    |
| Median Single-Node          | 61,759.1                    |

Compared to previous generations:

- **Azure ND GB200 v6:** 865,000 tokens/sec
- **NVIDIA DGX H100:** ~3,066 tokens/sec per GPU
- **ND H100 v5 VM:** 5× lower throughput than ND GB300 v6

## Technical Highlights

- **GEMM TFLOPS:** ND GB300 v6 achieves 2.5x more GEMM TFLOPS per GPU than ND H100 v5
- **Memory Bandwidth:** 7.37 TB/s HBM bandwidth at 92% efficiency
- **NVLink C2C:** 4x faster CPU-to-GPU transfer speeds
- **NCCL Communication:** Improved GPU interconnect performance
- **FP4 Precision:** Leveraging quantization for fast, accurate inference
- **Software Stack:** NVIDIA TensorRT-LLM, MLPerf 5.1, containerized workflows

## Detailed Benchmark Replication Guide

### Prerequisites

- Azure ND GB300 v6 VM access
- Basic familiarity with Docker, Python, ML benchmarking

### Step 1: Clone Benchmarking Guide

```bash
 git clone https://github.com/Azure/AI-benchmarking-guide.git && cd AI-benchmarking-guide/Azure_Results
```

### Step 2: Download Models & Datasets

- Create directories for models, data, and preprocessed_data
- Download Llama2 70B model and datasets as described in repo instructions

### Step 3: Setup and Build Container

```bash
mkdir build && cd build
 git clone https://github.com/NVIDIA/TensorRT-LLM.git TRTLLM
 cd TRTLLM

# Edit Makefile lines 135, 136 to set SOURCE_DIR and CODE_DIR paths

 make -C docker build
 make -C docker run
```

### Step 4: Install TensorRT-LLM and MLPerf Dependencies

```bash
 cd 1M_ND_GB300_v6_Inference/build/TRTLLM
 python3 ./scripts/build_wheel.py --trt_root /usr/local/tensorrt --benchmarks --cuda_architectures "103-real" --no-venv --clean
 pip install build/tensorrt_llm-1.1.0rc6-cp312-cp312-linux_aarch64.whl
 make clone_loadgen && make build_loadgen
 git clone https://github.com/NVIDIA/mitten.git ./build/mitten && pip install build/mitten
 pip install -r docker/common/requirements/requirements.llm.txt
```

### Step 5: Link Datasets and Run Benchmark

```bash
export MLPERF_SCRATCH_PATH=/work
export SYSTEM_NAME=ND_GB300_v6
make link_dirs
make run_llm_server RUN_ARGS="--core_type=trtllm_endpoint --benchmarks=llama2-70b --scenarios=Offline"
make run_harness RUN_ARGS="--core_type=trtllm_endpoint --benchmarks=llama2-70b --scenarios=Offline"
```

- Log files for all 18 runs: [Benchmark Results](https://github.com/Azure/AI-benchmarking-guide/tree/main/Azure_Results/1M_ND_GB300_v6_Inference/results)

### Notes on Results

- [1] Unverified MLPerf® v5.1 result; see MLCommons on validation protocols
- [2] Comparison ID for verified NVIDIA DGX H100 run: 4.1-0043

## Summary

Azure ND GB300 v6 enables new scales of enterprise AI inference, thanks to technical advances in hardware and ML infrastructure. By following the documented guide, practitioners can replicate benchmark performance using the open-source MLPerf and TensorRT-LLM stack on Azure VMs.

For questions, analysis, or further details, refer to the original benchmarking guide or reach out to Hugo Affaticati via the Azure HPC Blog.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/breaking-the-million-token-barrier-the-technical-achievement-of/ba-p/4466080)
