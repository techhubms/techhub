---
section_names:
- ai
- azure
date: 2026-03-18 20:08:51 +00:00
feed_name: Microsoft Tech Community
primary_section: ai
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-ncv6-virtual-machines-enhancements-and-ga-transition/ba-p/4503578
tags:
- AI
- AI Infrastructure
- Azure
- Azure HPC
- Azure Virtual Machines
- CAD/CAE
- Community
- Fractional GPU
- General Availability
- Generative AI Inference
- GPU Virtual Machines
- Intel Xeon 6 Granite Rapids
- NCv6
- NVIDIA RTX PRO 6000 Blackwell Server Edition
- NVMe
- Preview
- Regional Expansion
- SLA
- Southeast Asia
- Temp Disk
- VDI
- West US 2
title: 'Azure NCv6 Virtual Machines: Enhancements and GA Transition'
author: Fernando_Aznar
---

Fernando_Aznar outlines upcoming Azure NCv6 VM updates, including new General Purpose and Compute Optimized sizes, fractional GPU options, a move from Preview to General Availability, and planned region expansion for GPU-backed graphics and generative AI workloads.<!--excerpt_end-->

# Azure NCv6 Virtual Machines: Enhancements and GA Transition

NCv6 Virtual Machines are Azure's next-generation platform for both leading-edge graphics and generative AI compute workloads. They feature:

- NVIDIA RTX PRO 6000 Blackwell Server Edition (BSE) GPUs
- Intel Xeon 6 "Granite Rapids" 6900P series CPUs

NCv6 VMs are available now in **Preview**.

- Join the Preview: https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR9s7orOb3OJJnwABCNj_8JdUMzlLSzJFTTdRRE8yU0UxWFFYQlpYV1hDVy4u

## What’s changing soon

Azure is announcing updates intended to:

- Enhance VM performance and capabilities
- Provide more VM sizes so customers can right-size usage
- Transition NCv6 to production readiness via **General Availability (GA)**
- Expand availability across more Azure regions

## New VM sizes, features, and performance enhancements

Azure plans to introduce:

- **Seven new NCv6-series VM sizes**
- **Two sub-families**: General Purpose and Compute Optimized

Key changes called out:

- **Fractional GPU support**
  - Deploy with as little as **1/2** or **1/4** of an RTX PRO 6000.
  - Fractional GPU sizes also reduce vCPU, memory, SSD, and networking to optimize cost.
- **Increased vCPU per VM size**
  - Example given: **288 vCPU instead of 256**.
  - Intended to better align with the Intel Xeon 6900P triple compute tile architecture and improve performance for high-end VDI workstations.
- **General Purpose vs Compute Optimized**
  - **General Purpose**: more CPU memory for demanding generative AI inference and ISV CAD/CAE simulations.
  - **Compute Optimized**: reduced memory for less memory-intensive workloads to lower cost.

These new sizes will **replace** the existing three Preview VM sizes.

### NCv6 — General Purpose VM sizes

| Size Name | vCPUs | Memory (GB) | Networking (Mb/s) | GPUs | GPU Mem (GB) | Temp Disk | NVMe Disk |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Standard_NC36ds_xl_RTXPro6000_v6 | 36 | 132 | 22500 | 1/4 | 24 | 256 | 1600 |
| Standard_NC72ds_xl_RTXPro6000_v6 | 72 | 264 | 45000 | 1/2 | 48 | 512 | 3200 |
| Standard_NC132ds_xl_RTXPro6000_v6 | 132 | 516 | 90000 | 1 | 96 | 1024 | 6400 |
| Standard_NC144ds_xl_RTXPro6000_v6 | 144 | 516 | 90000 | 1 | 96 | 1024 | 6400 |
| Standard_NC264ds_xl_RTXPro6000_v6 | 264 | 1032 | 180000 | 2 | 192 | 2048 | 12800 |
| Standard_NC288ds_xl_RTXPro6000_v6 | 288 | 1032 | 180000 | 2 | 192 | 2048 | 12800 |
| Standard_NC324ds_xl_RTXPro6000_v6 | 324 | 1284 | 180000 | 2 | 192 | 2048 | 12800 |

### NCv6 — Compute Optimized VM sizes

| Size Name | vCPUs | Memory (GB) | Networking (Mbps) | GPUs | GPU Mem (GB) | Temp Disk | NVMe Disk |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Standard_NC24lds_xl_RTXPro6000_v6 | 24 | 72 | 22500 | 1/4 | 24 | 256 | 1600 |
| Standard_NC36lds_xl_RTXPro6000_v6 | 36 | 72 | 22500 | 1/4 | 24 | 256 | 1600 |
| Standard_NC72lds_xl_RTXPro6000_v6 | 72 | 132 | 45000 | 1/2 | 48 | 512 | 3200 |
| Standard_NC132lds_xl_RTXPro6000_v6 | 132 | 264 | 90000 | 1 | 96 | 1024 | 6400 |
| Standard_NC144lds_xl_RTXPro6000_v6 | 144 | 264 | 90000 | 1 | 96 | 1024 | 6400 |
| Standard_NC264lds_xl_RTXPro6000_v6 | 264 | 516 | 180000 | 2 | 192 | 2048 | 12800 |
| Standard_NC288lds_xl_RTXPro6000_v6 | 288 | 516 | 180000 | 2 | 192 | 2048 | 12800 |
| Standard_NC324lds_xl_RTXPro6000_v6 | 324 | 648 | 180000 | 2 | 192 | 2048 | 12800 |

> Note: Until the new sizes are available, Microsoft Learn resources may still show the currently offered Preview sizes and specs.

## Transition to General Availability (GA)

In the coming weeks, Azure plans to transition NCv6-series from **Preview** to **General Availability (GA)**.

Implications called out:

- NCv6 VMs will be covered by the **Azure Service Level Agreement (SLA)**.
- Intended to support production-grade deployments by customers, partners, and service providers.

Initial GA regions:

- **West US2**
- **Southeast Asia**

## Regional expansion across Azure

NCv6 started in **West US2** and later expanded to **Southeast Asia** (both included in the GA transition).

Planned additional regions (stated as in the months covering **Q3 of 2026**):

- East US
- West Europe
- East US 2
- North Europe
- South Central US
- Germany West Central
- West US
- Korea Central

## Availability and metadata

- Updated: Mar 18, 2026
- Version: 1.0


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-ncv6-virtual-machines-enhancements-and-ga-transition/ba-p/4503578)

