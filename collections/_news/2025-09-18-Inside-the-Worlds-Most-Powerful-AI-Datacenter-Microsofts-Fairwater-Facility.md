---
external_url: https://blogs.microsoft.com/blog/2025/09/18/inside-the-worlds-most-powerful-ai-datacenter/
title: 'Inside the World’s Most Powerful AI Datacenter: Microsoft’s Fairwater Facility'
author: Scott Guthrie
feed_name: The Azure Blog
date: 2025-09-18 15:45:10 +00:00
tags:
- AI Infrastructure
- AI WAN
- Azure Blob Storage
- Data Center Architecture
- Distributed Systems
- Exabyte Storage
- Fairwater Datacenter
- Frontier AI
- Green Cooling Technologies
- High Performance Computing
- Hyperscale
- Liquid Cooling
- Microsoft Cloud
- NVIDIA GPUs
- NVLink
- Storage
- Supercomputer
- AI
- Azure
- News
section_names:
- ai
- azure
primary_section: ai
---
Scott Guthrie introduces Microsoft’s new Fairwater AI datacenter, highlighting the scale, engineering innovation, and cloud infrastructure powering next-generation AI workloads across Azure’s global network.<!--excerpt_end-->

# Inside the World’s Most Powerful AI Datacenter: Microsoft’s Fairwater Facility

Microsoft has introduced Fairwater, its newest and most sophisticated AI datacenter located in Wisconsin, USA, marking a significant investment in the infrastructure necessary for advanced artificial intelligence workloads. This is part of a wave of new purpose-built datacenters designed to power the global adoption of AI and cloud services.

## Fairwater and Global Expansion

- **Fairwater Wisconsin** is the largest AI datacenter built by Microsoft, covering 315 acres and consisting of three buildings with a combined 1.2 million square feet.
- Additional Fairwater datacenters are under development across the US, with further projects in Narvik, Norway, and Loughton, UK, showing Microsoft’s expansion of hyperscale AI cloud deployments.
- These represent tens of billions in investments and house hundreds of thousands of cutting-edge NVIDIA GPUs.

## What is an AI Datacenter?

Unlike traditional cloud datacenters aimed at smaller, independent workloads, Microsoft’s AI datacenters are engineered as massive, unified supercomputers. Each is optimized for training and running large AI models, supporting workloads across OpenAI, Microsoft AI, Copilot capabilities, and more.

- Composed of dedicated AI accelerators (such as NVIDIA GB200 & future GB300), high-bandwidth compute clusters, exabyte-class storage, and top-of-the-line networking.
- A single datacenter can orchestrate training on hundreds of thousands of GPUs in parallel, providing 10x the performance of leading supercomputers.

## Technical Innovations

### High-Density Compute & Networking

- Each rack contains 72 NVIDIA Blackwell GPUs, tied with NVLink for 1.8 TB of bandwidth, sharing 14 TB pooled memory.
- Racks are laid out in a multi-story configuration to reduce network latency, with a fat-tree network using InfiniBand and Ethernet at 800Gbps.
- Multiple pods and racks interconnect to form a single, low-latency global supercomputer.

### Storage for AI Velocity

- Separate datacenter-scale infrastructure for storage and compute is essential for high-velocity AI workflows.
- Azure Blob Storage accounts can sustain over 2 million operations per second, and capacity can elastically scale to exabyte levels.
- Innovations like BlobFuse2 deliver low-latency, high-throughput data access for AI model training.

### Cooling & Sustainability

- Traditional air cooling is not sufficient; the facility uses closed-loop liquid cooling systems that require only a single water fill, reusing water without evaporation.
- The Fairwater site includes one of the world’s largest chiller plants and features 172 giant fans for heat exchange, driving efficiency at peak loads.

## Distributed AI: Azure WAN

- Datacenters are connected by a global Wide Area Network (AI WAN), allowing datacenter resources to operate as one distributed AI supercomputer.
- This model provides scalability, resiliency, and flexibility for enterprise AI workloads.

## Impact and Vision

- Microsoft’s design harmonizes innovation across silicon, hardware, networks, and software to meet the challenges posed by next-generation AI models.
- The Fairwater facility and its global counterparts will anchor the continued democratization of AI services, setting new standards for secure, adaptive cloud-powered intelligence.

For further details and a virtual tour, view [datacenters.microsoft.com](https://datacenters.microsoft.com).

*Author: Scott Guthrie*

This post appeared first on "The Azure Blog". [Read the entire article here](https://blogs.microsoft.com/blog/2025/09/18/inside-the-worlds-most-powerful-ai-datacenter/)
