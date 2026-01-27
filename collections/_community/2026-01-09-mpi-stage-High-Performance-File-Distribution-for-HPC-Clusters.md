---
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/mpi-stage-high-performance-file-distribution-for-hpc-clusters/ba-p/4484366
title: 'mpi-stage: High-Performance File Distribution for HPC Clusters'
author: pauledwards
feed_name: Microsoft Tech Community
date: 2026-01-09 09:49:38 +00:00
tags:
- Azure CycleCloud
- Blobfuse2
- Cluster Networking
- Container Images
- File Distribution
- GB300
- High Performance Computing
- HPC
- InfiniBand
- Job Orchestration
- Linux
- MPI
- Mpi Stage
- NVMe
- Parallel Computing
- Sbcast
- Shared Filesystem
- Slurm
- Squashfs
- Srun
section_names:
- azure
- devops
primary_section: azure
---
pauledwards demonstrates how mpi-stage leverages MPI broadcasts to efficiently distribute large files, such as container images, across Azure-based HPC clusters—improving startup times and minimizing shared file system bottlenecks.<!--excerpt_end-->

# Efficiently Staging Large Files Across Hundreds of Nodes Using MPI Broadcasts

Author: pauledwards

## Overview

Distributing large container images across HPC clusters can be a major bottleneck, especially when hundreds of nodes need the same file. This post describes how the `mpi-stage` tool solves this problem by using MPI broadcasts to efficiently distribute files such as Squashfs container images across all nodes in an Azure CycleCloud cluster with GPU nodes.

## Problem: Scalable File Staging

- Standard approaches like running container images from shared storage can overload that storage at scale.
- Copying to each node's local NVMe storage provides predictable startup performance and avoids performance degradation.
- Existing tools (e.g., Slurm's sbcast or shell-script fan-out) are either slow, complex, or unable to fully avoid shared filesystem contention.

## The mpi-stage Solution

`mpi-stage` uses highly optimized MPI collectives to distribute files:

- **Single source read:** Only one node pulls from the shared filesystem (e.g., NFS, Lustre, Azure Blob Storage via blobfuse2).
- **Backend distribution:** Data is shared over high-speed networks like InfiniBand using `MPI_Bcast`.
- **Smart node skipping:** If a file already exists on a node (verified by size or checksum), it skips copy.
- **Chunked, double-buffered transfer:** Overlaps network and storage I/O for best performance.
- **Predictable scalability:** Enforces one MPI rank per node for contention-free writing.

## Workflow Example

1. Stage the image with mpi-stage as a job pre-step:

   ```bash
   #!/bin/bash
   #SBATCH --job-name=my-training-job
   #SBATCH --ntasks-per-node=1
   #SBATCH --exclusive
   
   srun --mpi=pmix ./mpi_stage \
     --source /shared/images/pytorch.sqsh \
     --dest /nvme/images/pytorch.sqsh \
     --pre-validate size \
     --verbose
   
   srun --container-image=/nvme/images/pytorch.sqsh ...
   ```

2. Pre-validation ensures minimal time penalty for already-staged files.

## Real-World Results

- On 156 GPU nodes, effective distribution rate was ~3 GB/s, completing in roughly 5 seconds.
- Cumulative bandwidth scales with node count (e.g., ~468 GB/s of total local write throughput).

## Use Cases

- Distributing container images before training jobs
- Staging datasets or model checkpoints to local storage
- Any scenario with "many nodes, same file" requirement

## Getting Started

```sh
git clone https://github.com/edwardsp/mpi-stage.git
cd mpi-stage
make
```

- See the [README](https://github.com/edwardsp/mpi-stage) for details and usage options.

## When to Use

- Any large-scale, containerized workload on Azure HPC clusters or similar environments
- Especially critical when backend performance, startup predictability, and shared filesystem health are priorities

## Summary

`mpi-stage` streamlines high-throughput file distribution in Azure HPC clusters by combining efficient MPI communication with pragmatic workflow validation. Its approach offers significant improvements for startup reliability and cluster resource utilization versus traditional ad-hoc methods.

Feedback on real-world workflows and potential enhancements is welcome. For issues or contributions, see the [GitHub project](https://github.com/edwardsp/mpi-stage/).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/mpi-stage-high-performance-file-distribution-for-hpc-clusters/ba-p/4484366)
