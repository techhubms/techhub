---
layout: "post"
title: "Performance and Scalability of Azure HBv5-series Virtual Machines"
description: "This article provides a detailed exploration of Azure HBv5-series virtual machines, focusing on their technical features, benchmark results, cost-performance optimization, and use cases for high performance computing (HPC). It compares HBv5 with previous generations (HBv4, HBv3, HBv2), outlines key improvements in memory bandwidth, CPU architecture, networking, and disk IO, and discusses practical performance, cost, and fleet consolidation benefits for workloads like computational fluid dynamics, weather simulation, geoscience, and molecular dynamics. The article also covers microbenchmark methodologies, scalability analysis (weak and strong scaling), and offers actionable recommendations for HPC practitioners considering HBv5 adoption."
author: "jvenkatesh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-and-scalability-of-azure-hbv5-series-virtual/ba-p/4467230"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-05 06:42:40 +00:00
permalink: "/community/2025-11-05-Performance-and-Scalability-of-Azure-HBv5-series-Virtual-Machines.html"
categories: ["Azure"]
tags: ["AMD EPYC", "Ansys Fluent", "Azure", "Azure HBv5", "Community", "Cost Optimization", "CPU Benchmarking", "Devito", "High Performance Computing", "HPC", "InfiniBand", "Memory Bandwidth", "NAMD", "NVIDIA Quantum 2", "OpenFOAM", "Scalability", "Siemens Star CCM+", "STREAM Benchmark", "Strong Scaling", "Virtual Machines", "VM Fleet Consolidation", "Weak Scaling", "WRF"]
tags_normalized: ["amd epyc", "ansys fluent", "azure", "azure hbv5", "community", "cost optimization", "cpu benchmarking", "devito", "high performance computing", "hpc", "infiniband", "memory bandwidth", "namd", "nvidia quantum 2", "openfoam", "scalability", "siemens star ccmplus", "stream benchmark", "strong scaling", "virtual machines", "vm fleet consolidation", "weak scaling", "wrf"]
---

jvenkatesh and colleagues present an in-depth technical overview and benchmarks of Azure HBv5-series Virtual Machines, highlighting improvements for HPC workloads and practical guidance for Azure users.<!--excerpt_end-->

# Performance and Scalability of Azure HBv5-series Virtual Machines

## Overview

Azure HBv5-series virtual machines (VMs) are the latest CPU-based high performance computing (HPC) offering from Microsoft Azure, now generally available. This article, contributed by Amirreza Rastegari and colleagues, provides a technical deep-dive into HBv5 architecture, performance, cost implications, and real-world HPC application benchmarks.

HBv5 VMs offer significant advances for memory bandwidth-bound workloads, including computational fluid dynamics (CFD), weather and geoscience simulations, and finite element analysis. Compared to the previous HBv4 generation, HBv5 VMs deliver up to:

- 5x higher performance for CFD workloads with 43% lower costs
- 3.2x higher performance for weather simulation with 16% lower costs
- 2.8x higher performance for geoscience workloads at the same costs

## Technical Highlights

**Key features of HBv5-series VMs:**

- Up to 6.6 TB/s memory bandwidth (STREAM TRIAD) and 432 GB RAM per VM
- Up to 368 physical cores (user configurable) using custom AMD EPYC Zen4 CPUs (SMT disabled)
- Base clock 3.5 GHz, boost to 4 GHz across all cores
- 800 Gb/s NVIDIA Quantum-2 InfiniBand (4 x 200 Gb/s CX-7)
- 180 Gb/s Azure Accelerated Networking
- 15 TB local NVMe SSD, 50 GB/s read, 30 GB/s write bandwidth
- High-bandwidth memory (HBM), with ~9x memory bandwidth vs. dual-socket EPYC Genoa and ~7x vs. EPYC Turin
- Multiple constrained core configurations for ISV licensing and core-optimized scenarios

*See [official documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/high-performance-compute/hbv5-series) for full specs.*

## Microbenchmark Results

### Memory & Compute Performance

Industry benchmarks were used:

- STREAM – Memory bandwidth
- HPCG – Sparse linear algebra performance
- HPL – Dense linear algebra performance

Example command for STREAM:

```bash
OMP_NUM_THREADS=368 OMP_PROC_BIND=true OMP_PLACES=cores ./amd_zen_stream STREAM data size: 2621440000 bytes
```

### InfiniBand Networking Performance

- Four NVIDIA Quantum-2 NICs per VM, each at 200 Gb/s, aggregate 800 Gb/s per VM
- [IB perftests](https://github.com/linux-rdma/perftest) and OSU [benchmarks](https://mvapich.cse.ohio-state.edu/benchmarks/) show 99% of theoretical bandwidth achieved
- Latency as low as 1.25 microseconds (dependent on message size)

## Application Benchmarks: Performance, Cost, & Consolidation

Benchmarks were performed with real HPC workloads, comparing HBv5 with prior Azure VM generations (HBv4, HBv3, HBv2, HX series):

### CFD

- **OpenFOAM 2306:** 4.8x performance vs. HBv4, 57% cost.
- **Palabos 1.01:** 4.4x performance vs. HBv4, 62% cost.
- **Ansys Fluent 2025 R2:** 3.4x performance vs. HBv4, 81% cost.
- **Siemens Star-CCM+ 17.04:** 3.4x performance vs. HBv4, 81% cost.

### Weather Modeling

- **WRF 4.2.2:** 3.27x performance vs. HBv4, 84% cost.

### Energy Research

- **Devito 4.8.7:** 3.27x performance vs. HBv4, equivalent cost.

### Molecular Dynamics

- **NAMD 2.15a2:** 2.18x performance vs. HBv4, 26% higher cost (compute-bound, less memory-bound advantage).

### Insights

- HBv5 VMs consistently allow for ~2-5x consolidation of VM fleets for memory-bound applications.
- For compute-bound workloads (e.g. NAMD), HBv5 is fastest on Azure but cost efficiency may favor other VM families or GPUs.

## Scalability Analysis

### Weak Scaling

- Demonstrated using Palabos 3D Cavity model
- HBv5 shows linear scaling as workload and core count increase together

### Strong Scaling

- Demonstrated with NAMD 20M and 210M cell cases
- For moderate problem sizes, scaling is efficient up to a point (communication time limits gains)
- For large cases (210M cells), HBv5 scales linearly even at >11,000 cores (32 VMs)

**Recommendations:**

- Run scaling tests with your specific application to identify the sweet spot for time-to-solution and job cost
- For communication-bound cases, consider fewer MPI processes or redesigns to overlap communication and compute

## Conclusion

Azure HBv5-series VMs provide a major leap in memory bandwidth and CPU performance for Azure's HPC offerings, enabling substantial performance and cost advantages for memory-bound workloads across various scientific and engineering domains. For detailed configurations, benchmarks, and scalability practices, consult the official Azure documentation and consider application-specific scaling tests to optimize your deployment.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-and-scalability-of-azure-hbv5-series-virtual/ba-p/4467230)
