---
layout: "post"
title: "Implementing Siemens Teamcenter Simulation Process Data Management on Azure CycleCloud with Slurm"
description: "This article by Sunita_AZ0708 details the architecture and workflow for running Siemens Teamcenter Simulation Process Data Management (SPDM) on Azure CycleCloud using a Slurm HPC cluster. It covers the PLM core deployment, simulation job orchestration with StarCCM+, secure Azure networking, and benefits like dynamic scaling, hybrid flexibility, integrated security, and traceable digital thread from CAD to simulation results."
author: "Sunita_AZ0708"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-high-performance-computing/teamcenter-simulation-process-data-management-architecture-on/ba-p/4449316"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-28 19:58:12 +00:00
permalink: "/community/2025-08-28-Implementing-Siemens-Teamcenter-Simulation-Process-Data-Management-on-Azure-CycleCloud-with-Slurm.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Application Gateway", "Azure CycleCloud", "Azure Entra ID", "Azure Firewall", "Community", "Cost Governance", "HPC", "Hybrid Cloud", "PLM", "Product Validation", "RBAC", "SAML", "Security", "Simulation Management", "Simulation Workflow", "Slurm", "SPDM", "StarCCM+", "Teamcenter", "Virtual Network", "VNet Peering"]
tags_normalized: ["azure", "azure application gateway", "azure cyclecloud", "azure entra id", "azure firewall", "community", "cost governance", "hpc", "hybrid cloud", "plm", "product validation", "rbac", "saml", "security", "simulation management", "simulation workflow", "slurm", "spdm", "starccmplus", "teamcenter", "virtual network", "vnet peering"]
---

Sunita_AZ0708 explains how to implement Siemens Teamcenter SPDM on Azure CycleCloud with Slurm, covering secure multi-tier PLM architecture, simulation job workflow, and dynamic HPC resource management.<!--excerpt_end-->

# Implementing Siemens Teamcenter Simulation Process Data Management on Azure CycleCloud with Slurm

## Introduction

Many enterprises have fragmented simulation and product data management across multiple Teamcenter-SPDM instances, ISVs, and cloud/on-prem solutions. Consolidating with Azure enables a harmonized PLM experience, improving data access and traceability.

## What is Teamcenter Simulation?

Teamcenter Simulation tightly integrates simulation data, processes, and results into the core PLM environment, providing:

- Centralized management of CAD, models, inputs, and results
- Traceability between design, simulation, and manufacturing
- Seamless support for multi-CAD/CAE (NX Nastran, ANSYS, Abaqus, Star-CCM+)
- Single source of truth linking CAE analysis with original CAD data

## Reference Architecture

### Major Components

1. **Teamcenter PLM Core**
   - Distributed over four tiers (client, web, enterprise, resource) on virtual machines in one VNet
   - Central CAE management function runs in enterprise tier with web/thick-client access
   - Development/test isolation achievable through separate VMs and VNets

2. **StarCCM+ HPC Cluster with Azure CycleCloud and Slurm**
   - Siemens StarCCM simulation jobs launched from Teamcenter UI
   - Azure CycleCloud Provisions and scales HPC nodes as needed
   - Results output generated as `.sim` files and returned to Teamcenter

### Secure Access and Networking

- Users access Teamcenter via HTTPS endpoints, including rich and web clients
- Azure Entra ID (SAML) provides SSO authentication
- Azure Firewall and backbone filter/secure traffic; Application Gateway routes to web VMs
- Hub and Spoke VNets peered for seamless backbone communication
- Role-based access control and isolation for simulation data and compute resources

## Typical Simulation Workflow

1. **Users access Teamcenter via secure endpoint** (Active Workspace/Rich Client); simulation engineers use a lightweight Simulation client.
2. **SSO via Azure Entra ID** with SAML protects application logins.
3. **Traffic is secured** through Azure Firewall, Application Gateway, and Microsoft cybersecurity feeds.
4. **Teamcenter PLM is hosted on Azure**, with separation across core tiers and environments.
5. **Simulation jobs are submitted** from Teamcenter to the StarCCM HPC cluster via CycleCloud/Slurm.

### Simulation Steps

- **Step 1: Manage CAD Data and Product Structures**
  - CAD files from NX, CATIA, SolidWorks are managed directly in Teamcenter
  - Simulation models are linked to Teamcenter product structures for version control

- **Step 2: Build Simulation Model (Pre-processing)**
  - Templates define solvers (FEA, CFD, multiphysics) and required inputs
  - Tools like NX CAE, Simcenter 3D, ANSYS, and Star-CCM+ are integrated
  - Meshes, loads, materials, and boundary conditions mapped to design revisions

- **Step 3: Manage Simulation Data**
  - Input files, scripts, and solver settings are versioned in Teamcenter
  - Metadata for load cases, solver settings captured for reuse/search
  - Automated workflows ensure repeatable simulation processes

- **Step 4: Run Simulation Jobs (with Azure CycleCloud)**
  - Jobs are submitted to cloud HPC via Teamcenter; logs and outputs managed centrally
  - CycleCloud on-demand scales up nodes and tears down after job completion
  - Squeue/Sbatch used for job scheduling/monitoring

- **Step 5: Post-processing and Results Management**
  - Simulation results (stress plots, temperature distributions) imported back into Teamcenter
  - Visualization support through Simcenter 3D, JT format, web viewers
  - Results linked to original CAD, setups, and load cases for a complete digital thread

- **Step 6: Review, Sign-off, Collaboration**
  - Results are reviewed with design/manufacturing/management teams
  - E-signatures, approval workflows, and product validation are integrated into the process

## Azure CycleCloud Key Benefits

- **On-demand scaling** of compute resources to match simulation load
- **Slurm scheduler integration** for smooth job handling
- **Multi-VM and GPU support** for diverse simulation workloads
- **Hybrid cloud bursting** for seamless extension of on-prem resources
- **Cost governance** via built-in controls and reporting
- **Enhanced security**: VNet isolation, Azure Firewall, and RBAC
- **Integration with Azure Storage** for scalable, high-throughput data access

## Conclusion

Deploying Siemens Teamcenter SPDM on Azure CycleCloud with HPC and Slurm empowers enterprises to scale and standardize simulation process data management. Azure's dynamic scaling, network security, hybrid flexibility, and cost controls make it a robust platform for demanding engineering and product validation workloads.

For further details, see the [Teamcenter Architecture on Azure documentation](https://learn.microsoft.com/en-us/azure/architecture/example-scenario/manufacturing/teamcenter-baseline).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/teamcenter-simulation-process-data-management-architecture-on/ba-p/4449316)
