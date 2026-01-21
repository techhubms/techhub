---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/reimagining-ai-at-scale-nvidia-gb300-nvl72-on-azure/ba-p/4464556
title: 'Reimagining AI at Scale: Deploying NVIDIA GB300 NVL72 on Azure'
author: gwaqar
feed_name: Microsoft Tech Community
date: 2025-10-28 18:30:02 +00:00
tags:
- AI Cluster
- AI Infrastructure
- Azure Boost
- Blackwell Ultra GPU
- Datacenter
- Fleet Health
- Grace CPU
- Hardware Security Module
- High Performance Computing
- Infrastructure Innovation
- Liquid Cooling
- NVIDIA GB300
- NVL72
- Rack Management
- Supercomputing
section_names:
- ai
- azure
---
Gohar Waqar examines how Microsoft engineered the Azure platform to support NVIDIA GB300 NVL72 racks, focusing on AI infrastructure, advanced cooling, hardware integration, and secure, scalable deployment.<!--excerpt_end-->

# Reimagining AI at Scale: Deploying NVIDIA GB300 NVL72 on Azure

**Author:** Gohar Waqar, CVP of Cloud Hardware Infrastructure Engineering, Microsoft

## Overview

Microsoft is the first hyperscaler to deploy the NVIDIA GB300 NVL72 infrastructure at scale, enabling high-density, purpose-built AI supercomputing within a single Azure rack. This article outlines the technical details, design choices, and operational innovations behind the deployment.

## Key Platform Highlights

- **Ultra-dense GPU Cluster:** Each NVL72 rack consolidates 72 NVIDIA Blackwell Ultra GPUs (288 GB HBM3e per GPU) with 36 NVIDIA Grace CPUs. This delivers supercomputer-class performance within a standard rack footprint, making it suitable for multitrillion-parameter model training and high-throughput inference.

- **Next-generation Liquid Cooling:** A custom liquid cooling heat exchanger unit (HXU) system enables racks to dissipate up to ~136 kW of heat. Solutions support both air-cooled and facility-water environments, using closed coolant loops and leak detection systems to maximize efficiency and reliability.

- **Smart Rack Management:** Embedded management controllers and rack control modules provide real-time telemetry, automatic fault isolation, leak detection, and power/cooling adjustment. "Single pane of glass" fleet management streamlines diagnostics and maximizes uptime.

- **Integrated Security:** Each rack features Azure Integrated Hardware Security Module (HSM) chips, Azure Boost I/O and security accelerators, and a hardware root of trust via Datacenter-Secure Control Module (DC-SCM), further hardening datacenter operations.

- **Rapid Deployment:** Preassembled compute trays, NVLink fabrics, cooling, and power shelves allow for fast datacenter integration. The racks self-regulate power and cooling upon installation, minimizing manual intervention.

## Architecture & Innovation

- **Purpose-built for Scale:** Compared to previous generations, GB300 NVL72 racks increase GPU performance (from ~1.2kW to ~1.4kW per GPU with more memory), boost NVFP4 throughput by 50%, and improve power/cooling systems for 20% higher thermal loads. Cold plates and leak detection assemblies enhance safety and reliability for dense deployment.

- **Operational Resilience:** Management modules monitor power, temperature, flow, and fault scenarios, auto-throttling or shutting down components if needed. Fleet-wide health and diagnostics increase visibility and responsiveness for large-scale AI infrastructure.

- **Efficiency & Serviceability:** Azure’s deployment approach pairs high performance with operational excellence. Automation reduces manual overhead, ensuring consistent uptime and robust management as fleets of racks are deployed worldwide.

## Conclusion

The Azure–NVIDIA GB300 NVL72 partnership marks a significant step for large-scale AI infrastructure, providing the compute density, reliability, and secure operations required for advanced AI workloads. Innovations in cooling, management, and integration help organizations scale with confidence, supporting both current and next-generation AI demands.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/reimagining-ai-at-scale-nvidia-gb300-nvl72-on-azure/ba-p/4464556)
