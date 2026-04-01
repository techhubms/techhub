---
tags:
- AI Infrastructure
- Azure
- Azure CycleCloud
- Azure Monitor Agent
- Azure Monitor Logs
- Centralized Logging
- Cluster Troubleshooting
- Community
- CycleCloud Workspace For Slurm
- Data Collection Rules
- DCR Associations
- DevOps
- Distributed Training
- Dmesg
- HPC
- KQL
- Kusto Query Language
- Linux Syslog
- Log Analytics Workspace
- Managed Identity
- Observability
- Slurm
- Time Series Correlation
- User Assigned Managed Identity
- VM Scale Sets
- VMSS
author: jesselopez
section_names:
- azure
- devops
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/simplify-troubleshooting-at-scale-centralized-log-management-for/ba-p/4470658
primary_section: azure
title: Centralized Log Management for CycleCloud Workspace for Slurm with Azure Monitor Logs
date: 2026-03-31 21:17:04 +00:00
feed_name: Microsoft Tech Community
---

jesselopez shares a turnkey approach for centralizing Slurm, CycleCloud, and OS logs from large Azure CycleCloud clusters into Azure Monitor Logs/Log Analytics, making it faster to troubleshoot failures across thousands of nodes using consistent schemas, DCRs, and KQL queries.<!--excerpt_end-->

## Overview

Training large AI models on hundreds or thousands of nodes makes troubleshooting painful: when a distributed job fails, the root cause is often spread across many machines and many log files. Manually SSH-ing around the cluster slows recovery and reduces cluster utilization.

This post describes a turnkey, customizable log-forwarding architecture for **CycleCloud Workspace for Slurm (CCWS)** that centralizes cluster logs in **Azure Monitor Logs (Log Analytics)** so you can query everything from one place.

## Solution architecture

The solution centralizes logs from a CycleCloud Workspace for Slurm cluster into Azure Monitor Logs:

- Target platform: [CycleCloud Workspace for Slurm](https://learn.microsoft.com/en-us/azure/cyclecloud/overview-ccws?view=cyclecloud-8)
- Central store/query surface: [Azure Monitor Logs / Log Analytics](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/log-analytics-overview?tabs=simple)
- Agent on each node: [Azure Monitor Agent (AMA)](https://learn.microsoft.com/en-us/azure/azure-monitor/agents/azure-monitor-agent-overview)
- Collection configuration: [Data Collection Rules (DCR)](https://learn.microsoft.com/en-us/azure/azure-monitor/data-collection/data-collection-rule-overview)
- Destination: [Log Analytics workspace](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/log-analytics-workspace-overview)

### What gets collected

The turnkey setup captures three core log categories (and is designed to be extended to others):

- **Slurm logs**
  - Includes `slurmctld`, `slurmd`, etc.
  - Also captures archived job artifacts (job submission scripts, environment variables, stdout/stderr) via **prolog/epilog scripts**
- **Infrastructure logs**
  - Includes CycleCloud logs
  - Includes **CycleCloud Healthagent** output (tests hardware health and drains nodes that fail)
- **Operating system logs**
  - `syslog` and `dmesg` for kernel events, network state changes, and hardware issues

### How logs are organized

- Each log source has its **own DCR** and lands in a **dedicated table** with a consistent schema.
- The solution associates:
  - scheduler-specific DCRs with the **Slurm scheduler node**
  - compute-specific DCRs with **compute nodes**
- Dynamic scaling is handled transparently (including VM and **Virtual Machine Scale Set (VMSS)** scenarios).

## Key benefits

- **Time-series correlation**
  - Azure Monitor time-based indexing helps correlate cascading failures quickly (example: a network carrier flap in `syslog` → `slurmd` communication errors → specific job failures).
- **Centralized visibility**
  - Query logs across thousands of nodes without SSH-ing into individual machines; correlate controller decisions with node/system events.
- **Log persistence**
  - Logs survive node deallocations and reimaging, which matters when compute nodes are ephemeral.
- **Powerful querying with KQL**
  - Use **Kusto Query Language (KQL)** to parse raw logs into structured fields, filter across sources, and build dashboards.
  - Example use cases mentioned: repeated job failures, network instability, resource exhaustion.
- **Production-ready scalability**
  - **User-assigned managed identities** propagate automatically to new VMSS instances.
  - DCR associations support thousands of nodes without manual per-node configuration.

## Getting started

The complete solution is published on GitHub: [slurm-log-collection](https://github.com/yosoyjay/slurm-log-collection)

Deployment scripts in the repo:

- Create required Log Analytics tables
- Deploy pre-configured DCRs for Slurm, CycleCloud, and OS logs
- Automatically associate DCRs with scheduler and compute resources

Operational notes:

- After setting environment variables and running the setup scripts, logs should begin flowing and **populate within ~15 minutes**.
- Normal ingestion latency is described as **~30 seconds to 3 minutes**.
- The repo includes **sample KQL queries** for common troubleshooting scenarios, as well as analysis of cluster usage.

## Version

- Updated: Mar 31, 2026
- Version: 1.0


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/simplify-troubleshooting-at-scale-centralized-log-management-for/ba-p/4470658)

