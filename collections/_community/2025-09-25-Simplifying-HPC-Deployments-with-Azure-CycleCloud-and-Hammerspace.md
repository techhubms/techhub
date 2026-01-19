---
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/cyclecloud-hammerspace/ba-p/4457043
title: Simplifying HPC Deployments with Azure CycleCloud and Hammerspace
author: anhoward
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-09-25 19:01:20 +00:00
tags:
- Automation
- Azure CycleCloud
- Azure Marketplace
- Cloud Infrastructure
- Cluster Deployment
- Data Platform
- File System
- Hammerspace
- High Performance Computing
- HPC
- Job Scheduling
- Linux
- NFS
- Operational Efficiency
- Scheduled Events
- SLURM
section_names:
- azure
- coding
- devops
---
anhoward demonstrates how to simplify high performance computing on Azure by integrating CycleCloud, SLURM, and Hammerspace. The post guides through cluster setup, data management, and automated cleanup for practitioners seeking operational efficiency.<!--excerpt_end-->

# Simplifying HPC Deployments with Azure CycleCloud and Hammerspace

## Overview

Today's high performance computing (HPC) users face an overwhelming array of schedulers, cloud infrastructures, and data management options. This guide focuses on practical simplicity: using Azure CycleCloud to deploy a SLURM cluster in combination with the Hammerspace Data Platform, all leveraging familiar NFS protocols for quick, scalable integration.

## Why Simplicity Matters in HPC

Deploying, managing, and scaling HPC environments can be complex and resource-intensive. CycleCloud offers a standardized template approach, allowing direct deployment of SLURM clusters from the Azure Marketplace with minimal manual configuration:

- **Cluster in minutes:** Skip manual installation; deploy a working cluster in 15–20 minutes
- **Best practices built-in:** Preconfigured security rules, partitions (GPU, HTC spot), and node setups
- **Automatic cost control:** Nodes spin up on job submission and shut down post-completion, supporting elastic resource allocation

## Hammerspace: Seamless Data Platform Integration

Hammerspace provides a global, software-defined file system that operates natively within the Linux kernel. This allows all compute nodes in a CycleCloud cluster to access and share files via standard NFS protocols (v3, v4, pNFS) without agent installation or custom scripts.

**Benefits of Native NFS with Hammerspace:**

- POSIX-compliant, high-performance access without code changes
- No need for data copying or application refactoring
- Fast, seamless NFS mounts made during CycleCloud deployment—data is instantly available to SLURM jobs

## Step-by-Step: Adding NFS Storage

1. In the Azure Marketplace template or directly from the SLURM scheduler, configure external NFS mounts by specifying the Hammerspace Anvil Metadata server address and relevant mount options
2. Specify mount points for directories such as `/sched` and `/data`
3. Once nodes are provisioned, all are automatically mounted and available for job execution

## Data Management and Policy Automation

Hammerspace simplifies on-demand data placement and ensures immediate data availability—no more scripting or manual tier management. Its policy-driven automation moves data to the right performance or cost tier as needed, removing operational bottlenecks.

## CycleCloud Scheduled Events: Resource Cleanup

A key feature in newer CycleCloud versions is **Scheduled Events**, allowing scripts to run automatically during node termination. This enables:

- Clean unmounting of NFS shares upon VM shutdown, mitigating issues with stale or hanging mounts
- Cost savings and operational efficiency by ensuring cloud resources (e.g., IPs, NICs, disks) aren't left behind

*Relevant Azure documentation:* [CycleCloud Scheduled Events](https://learn.microsoft.com/en-us/azure/cyclecloud/how-to/scheduled-events?view=cyclecloud-8)

## Conclusion

By combining Azure CycleCloud, SLURM, and Hammerspace, organizations can build robust, high-performing, and easy-to-manage HPC clusters. The described solutions minimize administrative overhead, accelerate deployment, lower operational costs, and free up engineering time for solving core computational challenges.

For more background and advanced configuration guides, see:

- [Azure CycleCloud & SLURM: A Beginner’s Guide](https://techcommunity.microsoft.com/blog/azurehighperformancecomputingblog/azure-cyclecloud--slurm-a-beginner%E2%80%99s-guide-to-job-submission/4413711)
- [Hammerspace Whitepapers](https://hammerspace.com/category/white-paper/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/cyclecloud-hammerspace/ba-p/4457043)
