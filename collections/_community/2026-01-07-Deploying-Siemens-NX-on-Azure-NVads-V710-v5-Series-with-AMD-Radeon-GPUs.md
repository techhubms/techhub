---
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-v710-v5-series-amd-radeon-gpu-validation-of-siemens-cad-nx/ba-p/4483791
title: Deploying Siemens NX on Azure NVads V710 v5-Series with AMD Radeon GPUs
author: Sunita_AZ0708
feed_name: Microsoft Tech Community
date: 2026-01-07 16:38:08 +00:00
tags:
- Accelerated Networking
- AMD Radeon Pro V710
- Azure NVads V710
- Azure Storage
- Azure Virtual Machines
- CAM
- Cloud Architecture
- GPU Acceleration
- High Performance Computing
- OpenGL
- Remote Engineering
- Siemens NX
- Virtual Desktop
section_names:
- azure
primary_section: azure
---
Sunita_AZ0708 explores how Siemens NX runs on Azure NVads V710 v5 VMs with AMD Radeon GPUs, covering architecture details, performance benchmarks, and benefits for cloud-based engineering teams.<!--excerpt_end-->

# Siemens NX on Azure NVads V710 v5-Series: Technical Overview

## Overview of Siemens NX

Siemens NX is a next-generation integrated CAD/CAM/CAE platform widely used in aerospace, automotive, energy, robotics, and other manufacturing industries. It supports complex 3D modeling, massive assemblies, CAM/machining simulation, and integrated multiphysics with Simcenter/NX Nastran. Performance and graphics acceleration are critical for optimal user experience.

### Why GPU Acceleration for NX?

- **OpenGL acceleration**
- **Shader-based rendering**
- **Real-time shading, hidden line removal**
- **Ray-Traced Studio for photorealistic output**

NX's graphical interface, quick mode switching, smooth zoom, and sectioning rely on GPU power and stable frame pacing.

## Azure NVads V710 v5-Series VM Features

Azure NVads V710 v5-series VMs are designed for GPU-accelerated workloads and virtual desktops. Key specs:

- **GPU**: AMD Radeon™ Pro V710 (up to 24 GiB, fractional allocation supported)
- **CPU**: AMD EPYC™ 9V64 F (up to 4.3 GHz)
- **Memory**: 16 GiB – 160 GiB
- **VM Sizes**: From Standard_NV4ads_V710_v5 (4 vCPUs, 1/6 GPU) to Standard_NV28adms_V710_v5 (28 vCPUs, full GPU)
- **Storage**: NVMe-based ephemeral storage, premium storage support
- **OS**: Both Windows and Linux
- **Other**: Accelerated networking, no extra GPU licensing needed

AMD Radeon PRO drivers are ISV-validated, delivering reliable OpenGL performance and stability with large assemblies.

## Business Scenarios

- **Distributed engineering teams** can access powerful workstations from anywhere.
- **Supplier/partner collaboration** without local high-end hardware.
- **IP protection:** Data stays in Azure; files don't leave the cloud.
- **Pay-as-you-go scale** for burst design/test cycles.

## Architecture for Siemens NX on Azure

- Deploy Azure NVads_v710_24 VM
- Install AMD V710 GPU drivers from Azure
- Attach Azure File-based storage for assemblies, metadata, simulation data
- Set up VNET with accelerated networking
- Install Siemens NX, NX licenses, and test suites (NXCP & ATS)

## Performance and Benchmark Results

Siemens-approved tests validate rendering performance, stability, and correctness:

- Complex assemblies remain smooth and interactive (rotation, zoom, selection) during concurrent sessions
- ATS Non-Interactive/Interactive tests: Passed on all standard functions, including basic OpenGL features, fog, lighting, environment mapping, etc.
- NXCP Gdat tests demonstrated deterministic rendering and visual consistency
- Execution times confirm stable, repeatable performance under various loads

## Key Benefits

- **Workstation-class drivers baked-in**: ISV-certified
- **Excellent CAD performance** for broader user base
- **Remote engineering enablement** for global teams
- **Elastic scale**: Spin up GPU power when needed, scale down when idle
- **Fractional GPU allocation**: Right-size VMs for user needs, manage costs efficiently
- **CPU-GPU architectural balance** avoids idle hardware and boosts utilization

## Conclusion

Running Siemens NX on Azure NVads V710 v5 with AMD Radeon GPUs delivers a robust, scalable, and cost-efficient platform for enterprise CAD/CAM/CAE workloads. It combines workstation-grade graphics, secure remote access, and flexible cost models—enabling distributed innovation and digital product development without traditional hardware constraints.

---
**Author:** Sunita_AZ0708

**Published:** Jan 07, 2026

**Azure High Performance Computing (HPC) Blog**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-v710-v5-series-amd-radeon-gpu-validation-of-siemens-cad-nx/ba-p/4483791)
