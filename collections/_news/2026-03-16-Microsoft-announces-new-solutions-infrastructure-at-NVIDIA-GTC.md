---
author: stclarke
section_names:
- ai
- azure
- ml
- security
primary_section: ai
external_url: https://blogs.microsoft.com/blog/2026/03/16/microsoft-at-nvidia-gtc-new-solutions-for-microsoft-foundry-azure-ai-infrastructure-and-physical-ai/
title: Microsoft announces new solutions, infrastructure at NVIDIA GTC
feed_name: Microsoft News
date: 2026-03-16 20:34:48 +00:00
tags:
- Agentic AI
- AI
- AI Agents
- Azure
- Azure AI Infrastructure
- Azure Arc
- Azure Local
- Company News
- Digital Twins
- Edge Deployment
- Foundry Agent Service
- Foundry Control Plane
- Foundry Local
- Foundry Observability
- GitHub Repository
- Grace Blackwell GPUs
- Inference Workloads
- Liquid Cooled Datacenters
- Microsoft Fabric
- Microsoft Foundry
- ML
- Model Fine Tuning
- Multimodal Agents
- News
- NVIDIA Nemotron
- NVIDIA Omniverse
- NVIDIA Vera Rubin NVL72
- Observability
- Open Weight Models
- Palo Alto Prisma AIRS
- Physical AI
- Reasoning Models
- Regulated Environments
- Robotics Workflows
- Runtime Security
- Security
- Sovereign Cloud
- Voice Live API
- Zenity
---

In this NVIDIA GTC update, stclarke outlines Microsoft’s announcements across Microsoft Foundry and Azure: Foundry Agent Service GA with control-plane observability, voice agent preview capabilities, expanded model access (including NVIDIA Nemotron), and new Azure AI infrastructure plus “Physical AI” tooling that connects simulation, data, and real-world operations.<!--excerpt_end-->

![An employee standing between server racks at a Microsoft AI datacenter.](https://blogs.microsoft.com/wp-content/uploads/2026/03/OMB-Hero-3_16_26.png)

## Overview
Microsoft shared a set of announcements at NVIDIA GTC focused on:

- **Microsoft Foundry** capabilities for building and operating **production AI agents**
- **Azure AI infrastructure** optimized for inference-heavy, reasoning workloads (including upcoming NVIDIA platforms)
- A joint push into **Physical AI** (simulation + real-world operations) with integrations across Azure services, Microsoft Fabric, and NVIDIA tooling

## What’s new at NVIDIA GTC
- Expanded **Microsoft Foundry** capabilities to build, deploy, and operate production-ready AI agents on **NVIDIA accelerators** and open **NVIDIA Nemotron** models
- New **Azure AI infrastructure** optimized for inference-heavy, reasoning-based workloads, including being the first hyperscale cloud to power on next-generation **NVIDIA Vera Rubin NVL72** systems
- Deeper integration across **Microsoft Foundry**, **Microsoft Fabric**, and **NVIDIA Omniverse** libraries/frameworks to support **Physical AI** systems from simulation to real‑world operations

## From frontier models to production-ready agents
Microsoft positions **Microsoft Foundry** as an “operating system” for building, deploying, and operating AI at enterprise scale, built on **Azure** and bringing together:

- Models
- Tools
- Data
- Observability

### Foundry Agent Service + Foundry control plane (GA)
The following are now generally available:

- [Foundry Agent Service and Observability in Foundry Control Plane](https://aka.ms/FoundryAgentsGA-blog)

Key points described:

- Teams can build agents that **reason, plan, and act** across tools, data, and workflows.
- **Foundry Control Plane** provides end-to-end visibility into agent behavior, framed as improving developer productivity and enterprise trust.
- Example customer: [Corvus Energy](https://aka.ms/CorvusEnergy) using Foundry to replace manual inspection workflows with agent-driven operational intelligence.

### Voice-first agent experiences (public preview)
Microsoft also announced:

- [Voice Live API integration with Foundry Agent Service](https://aka.ms/VoiceAgent-preview-blog) (public preview)

This is described as enabling developers to build **voice-first, multimodal, real-time** agentic experiences.

### Additional portal, integrations, and security
The post mentions:

- A refreshed **Microsoft Foundry portal** (GA)
- Expanded integrations for **Palo Alto Networks’ Prisma AIRS** and **Zenity**, intended to provide deeper builder experiences and **runtime security across the agent lifecycle**

### NVIDIA Nemotron models in Foundry
Microsoft announced Nemotron availability and partnerships for model options and inference:

- [NVIDIA Nemotron models now available through Microsoft Foundry](https://aka.ms/gtc2026foundrymodelsblog)
- Prior partnership: [Fireworks AI on Microsoft Foundry](https://azure.microsoft.com/en-us/blog/introducing-fireworks-ai-on-microsoft-foundry-bringing-high-performance-low-latency-open-model-inference-to-azure/)

The post highlights fine-tuning **open-weight models** (like Nemotron) into low-latency assets that can be distributed to the edge.

## Scaling AI infrastructure for demanding inference workloads
Microsoft frames inference and reasoning workloads as changing cost/performance requirements and driving demand for infrastructure that can be:

- Deployed and operated consistently across **global** and **regulated** environments
- Purpose-built for **inference-heavy, reasoning-based** workloads

### Datacenter and NVIDIA platform rollout
Key infrastructure points:

- Azure datacenters engineered for power/cooling/networking and fast upgrades.
- Link: [Microsoft’s strategic AI datacenter planning… NVIDIA Rubin deployments](https://azure.microsoft.com/en-us/blog/microsofts-strategic-ai-datacenter-planning-enables-seamless-large-scale-nvidia-rubin-deployments)
- Deployed “hundreds of thousands” of **liquid-cooled Grace Blackwell GPUs**.
- Microsoft claims to be the first hyperscale cloud to power on **NVIDIA Vera Rubin NVL72** in labs, with rollout planned into liquid-cooled Azure datacenters.
  - Reference: [Satya Nadella post](https://x.com/satyanadella/status/2032515189086761005)

### Sovereign/regulated environments: Azure Local + Arc + Foundry Local
Microsoft describes extending accelerated AI to customer-controlled environments:

- [Foundry Local support for modern infrastructure and large AI models](https://blogs.microsoft.com/blog/2026/02/24/microsoft-sovereign-cloud-adds-governance-productivity-and-support-for-large-ai-models-securely-running-even-when-completely-disconnected/)
- Initial support for the **NVIDIA Vera Rubin platform on Azure Local**: http://aka.ms/GTC26SovAI

The post also calls out **Azure-consistent operations, governance, and security** using a unified software layer with **Azure Arc** and **Foundry Local**.

## Bringing AI into the physical world (Physical AI)
Microsoft and NVIDIA describe collaboration around **Physical AI**, including an NVIDIA blueprint and Azure-based tooling.

### Physical AI Data Factory Blueprint + Azure
Microsoft references the NVIDIA Physical AI Data Factory Blueprint, with Foundry as a platform to host and operate Physical AI systems on Azure.

### Azure Physical AI Toolchain (GitHub)
Microsoft introduced a public repo:

- [Azure Physical AI Toolchain GitHub repository](https://github.com/microsoft/physical-ai-toolchain)

The post says it is integrated with the NVIDIA Physical AI Data Factory and core Azure services.

### Fabric + Omniverse for real-time operational data + digital twins
The post also highlights deeper integration between:

- **Microsoft Fabric**
- **NVIDIA Omniverse** libraries

The goal described is connecting live operational data with physically accurate digital twins/simulation to support real-time understanding and AI-driven decisions and actions in operations/manufacturing scenarios.

## From innovation to impact
Microsoft summarizes the overall direction as delivering production-scale AI by combining:

- Global AI infrastructure
- Platforms (Foundry, Fabric)
- Real-world systems

The post reiterates themes of performance, security, and governance for regulated industries, and faster paths from prototypes to deployed systems.

## Author note
The post ends with a brief bio noting that **Yina Arenas** leads product strategy and execution for Microsoft Foundry, including infrastructure, developer experiences, and model integration across multiple model providers.


[Read the entire article](https://blogs.microsoft.com/blog/2026/03/16/microsoft-at-nvidia-gtc-new-solutions-for-microsoft-foundry-azure-ai-infrastructure-and-physical-ai/)

