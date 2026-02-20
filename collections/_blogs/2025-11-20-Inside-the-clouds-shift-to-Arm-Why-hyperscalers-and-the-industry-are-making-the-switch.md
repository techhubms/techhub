---
external_url: https://www.devclass.com/vms/2025/11/20/inside-the-clouds-shift-to-arm-why-hyperscalers-and-the-industry-are-making-the-switch/1731935
title: 'Inside the cloud’s shift to Arm: Why hyperscalers and the industry are making the switch'
author: DevClass.com
primary_section: azure
feed_name: DevClass
date: 2025-11-20 09:00:00 +00:00
tags:
- ARM Neoverse
- Arm64
- Azure
- Azure Cobalt 100
- Azure Cobalt 200
- Blogs
- Cloud Infrastructure
- Cloud Migration
- Developer Onboarding
- Energy Efficiency
- GitHub Integration
- Hyperscalers
- Kubernetes
- Microsoft Azure
- Price Performance
- Virtual Machines
section_names:
- azure
---
DevClass.com examines the rapid adoption of Arm architecture among leading hyperscalers, highlighting Microsoft Azure’s transition to Arm-based Cobalt processors. The article covers ecosystem maturity and practical migration steps for developers.<!--excerpt_end-->

# Inside the cloud’s shift to Arm: Why hyperscalers and the industry are making the switch

*By DevClass.com*

## Overview

Arm, once known mainly for powering mobile devices, is now a central architecture for major cloud providers. With energy efficiency, strong performance, and cost benefits, hyperscalers like Microsoft Azure, Google Cloud, and AWS are pivoting large segments of their infrastructure to Arm-based platforms, notably the Arm Neoverse line.

## Arm in Modern Cloud Infrastructure

- **Arm Neoverse** is now at the heart of cloud infrastructure, with estimates that half of all compute shipped to top hyperscalers in 2025 will be Arm-based.
- Arm’s architecture is optimizing both general-purpose computing and AI/data-intensive workloads, lowering total cost of ownership across the board.

## Case Studies and Performance Benchmarks

- **Google Cloud**:
  - Has ported over 30,000 applications to Arm, with another 70,000 in progress.
  - Google Axion CPUs deliver up to 65% better price-performance and up to 60% greater energy efficiency than x86 instances.
  - N4A virtual machines claim up to 2x better price-performance and 80% better performance-per-watt compared to x86 VMs.

- **AWS**:
  - Graviton4 processors outperform x86-based EC2 offerings.
  - XgBoost ML training on Graviton4 is up to 53% faster with 64% better price-performance.
  - Redis operations per second are up to 93% higher than x86 options.

- **Microsoft Azure**:
  - Arm Neoverse-based **Cobalt 100** processors deliver up to 48% higher performance and 91% better price-performance in DB workloads, plus significant gains in networking and financial modeling.
  - For AI and large language models with ONNX Runtime, Cobalt 100 provides up to 1.9x the performance and 2.8x the price-performance over x86.
  - **Cobalt 200**, introduced at Microsoft Ignite, promises another 50% performance lift over Cobalt 100, advancing efficiency, security, and storage/networking technology.

## Ecosystem Adoption

- Cloud-native firms like Databricks and Snowflake leverage Cobalt 100 for footprint and performance gains.
- Spotify, Paramount Global, and Pinterest cite substantial improvements in performance metrics and cost efficiency after adopting Arm-based VM offerings on various clouds.

## Migration Paths and Tools for Developers

- Migration to Arm is now more mature, with robust toolchains, Arm64 framework support, and onboarding guides.
- Developers are advised to start with bounded workloads (e.g., stateless microservices or API backends), compile for Arm64, and test in production via A/B traffic routing.
- Key migration strategies include monitoring p95 latency, cost-per-request, and energy consumption per request.
- Arm’s ecosystem offers a cloud migration program and integration with tools like Kubernetes and GitHub (including Copilot extensions) for smoother transitions.

## Summary

- The cloud industry’s shift to Arm is well underway, driven by tangible gains in performance, efficiency, and cost-control.
- Microsoft Azure is a leader in this transformation with Cobalt processors advancing real-world workloads.
- Developers and organizations are encouraged to consider phased migrations, leveraging mature toolchains and ecosystem partnerships to optimize for the next era of cloud computing.

This post appeared first on "DevClass". [Read the entire article here](https://www.devclass.com/vms/2025/11/20/inside-the-clouds-shift-to-arm-why-hyperscalers-and-the-industry-are-making-the-switch/1731935)
