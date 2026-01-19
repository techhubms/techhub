---
external_url: https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-driving-security-in-the-era-of-ai-innovation/ba-p/4471034
title: 'Azure Linux: Security-Focused Cloud Innovation at Ignite 2025'
author: Sudhanva
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-18 16:49:02 +00:00
tags:
- AKS
- Azure Linux
- Azure Red Hat OpenShift
- Cloud Security
- Container Security
- Dm Verity
- FedRAMP
- FIPS Compliance
- Immutable Host
- Kata Containers
- Linux On Azure
- Microsoft Ignite
- OS Guard
- Pod Sandboxing
- SELinux
- Trusted Launch
- Upstream Transparency
- VM Level Isolation
- Vtpm
- Workload Isolation
section_names:
- azure
- security
---
Sudhanva details Azure Linux security innovations introduced at Ignite 2025, including OS Guard and pod sandboxing in AKS, offering cloud practitioners stronger workload isolation and compliance capabilities.<!--excerpt_end-->

# Azure Linux: Security-Focused Cloud Innovation at Ignite 2025

## Overview

Microsoft continues advancing cloud and AI innovation with a sharp focus on security, quality, and responsible development. At Ignite 2025, Azure Linux stands out, powering critical services and serving as a hub for ongoing security innovation.

## Key Announcements

### OS Guard Public Preview

- **Azure Linux OS Guard** (public preview):
  - Hardened, immutable container host based on Azure Linux’s FedRAMP-certified base image
  - Streamlined footprint with ~100 fewer packages than standard image, reducing attack surface and improving performance
  - **FIPS mode enforced by default** for out-of-the-box compliance in regulated workloads
  - **Security features** include:
    - *dm-verity* for filesystem immutability
    - Trusted Launch (vTPM-secured keys)
    - Integration with AKS for streamlined container workloads
    - Upstream transparency and ongoing Microsoft contributions
  - Operational simplicity for secure, containerized app deployment
- **Audit Mode for Code Integrity and SELinux**:
  - Enabled during preview to allow customers policy validation and enforcement prep

### General Availability: Pod Sandboxing in AKS

- **Pod Sandboxing for AKS** (GA):
  - Stronger workload isolation in multi-tenant/regulatory environments
  - **Kata Containers project** enables VM-level isolation for each pod, utilizing lightweight VMs for security boundaries

## Sessions & Resources at Ignite

- Breakout and theater sessions cover performance, deployment, and security for Azure Linux and hybrid workloads:
  - [BRK 143: Optimizing performance, deployments, and security for Linux on Azure](https://ignite.microsoft.com/en-US/sessions/BRK143)
  - [BRK 144: Build, modernize, and secure AKS workloads with Azure Linux](https://ignite.microsoft.com/en-US/sessions/BRK144)
  - [BRK 104: From VMs and containers to AI apps with Azure Red Hat OpenShift](https://ignite.microsoft.com/en-US/sessions/BRK104)
  - [TRH 712: Hybrid workload compliance from policy to practice on Azure](https://ignite.microsoft.com/en-US/sessions/THR712)
  - [THR 701: Building Minimal-CVE Solutions with Azure Linux](https://ignite.microsoft.com/en-US/sessions/THR701)
  - [Lab 505: Fast track your Linux and PostgreSQL migration with Azure Migrate](https://ignite.microsoft.com/en-US/sessions/LAB505)

## Resources

- [Azure Linux OS Guard Overview & QuickStart](https://aka.ms/osguard)
- [Pod Sandboxing Overview & QuickStart](https://aka.ms/podsandboxing)
- [Azure Linux Documentation](https://learn.microsoft.com/en-us/azure/azure-linux/)

## Connect & Learn

- Meet the Azure Linux team at Ignite, join live demos, ask questions, and participate in deep dive sessions focused on security and operational excellence.

## Author

Sudhanva

_Last updated: Nov 18, 2025_

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-driving-security-in-the-era-of-ai-innovation/ba-p/4471034)
