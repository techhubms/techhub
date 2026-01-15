---
layout: post
title: Azure Arc Monthly Forum Recap – November 2025
author: yunishussein
canonical_url: https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-arc-monthly-forum-recap-november-2025/ba-p/4478127
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-17 17:55:17 +00:00
permalink: /azure/community/Azure-Arc-Monthly-Forum-Recap-November-2025
tags:
- Auto Agent Upgrade
- AZCM Agent
- Azure
- Azure Arc
- Azure Monitor
- Azure Policy
- Change Tracking
- CIS Baseline Compliance
- Community
- Essential Machine Management
- Hybrid Cloud
- Inventory
- Linux Support
- Log Analytics
- Machine Configuration
- Operations Center
- Preview
- Security
- Security Baseline
- Windows Server
section_names:
- azure
- security
---
yunishussein summarizes the key discussions from the November 2025 Azure Arc Monthly Forum, with highlights on Auto Agent Upgrade, Essential Machine Management, CIS Baseline Compliance, and answers to common Azure management and security questions.<!--excerpt_end-->

# Azure Arc Monthly Forum Recap – November 2025

Missed the live session? All forum sessions are recorded and uploaded to [the Azure Arc Server Forum YouTube channel](https://www.youtube.com/@AzureArcServerForum) within 2-3 weeks for convenient access.

## Key Highlights

### Auto Agent Upgrade

- **Status:** Public Preview
- **Capability:** Automatically updates the Azure Connected Machine (AZCM) Agent for simplified lifecycle management.
- **Feedback/Support:** arcautoupgradefeedback@microsoft.com

### Essential Machine Management (EMM)

- **Status:** Private Preview
- **Capability:** Delivers a simple, unified experience for managing machines across environments with the new Operations Center interface.
- **Lab Access:** [Operations Center Lab](https://aka.ms/operationsCenterLab)
- **Feedback/Support:** machineEnrollmentSupport@microsoft.com

### Machine Configuration – CIS Baseline Compliance

- **Status:** Public Preview
- **Capability:** Filter, search, exclude, and modify CIS baseline settings through Azure Policy for enhanced compliance.
- **Insiders Program:** aka.ms/machine-config-insiders
- **Feedback/Support:** machineconfig@microsoft.com

## FAQs from November 2025

### Essential Machine Management (EMM)

- **Azure Local Support:** EMM includes Azure Local machines.
- **Optimizations vs. Recommendations:** Recommendations are provided by Azure Advisor (focus on security, observability, configs), while optimizations currently address cost and emissions.
- **Rebranding:** Arc is not being rebranded. Operations Center is a new, unified management experience.
- **Training:** [Official Operations Center documentation](https://learn.microsoft.com/en-us/azure/operations/overview).
- **Azure Monitor & Log Analytics Costs:** Azure Monitor Workspace is free for EMM metrics. Log Analytics Workspace logs may incur charges; as of now, only Change Tracking and Inventory logs are configured.

### Machine Configuration – CIS Baseline Compliance

- **More Baselines:** Additional baselines will be added in the future.
- **Security Baseline Remediation:** Audit-only policies are available; remediation support is in development.
- **Windows Security Baseline:** Planned for Windows Server 2025 (WS2025).
- **Local GPO Policies:** Currently audit-only; no override support yet.

### Machine Configuration – OS Settings Inventory Platform

- **Custom Classes:** No plans for custom data ingestion classes at present.
- **Linux Support:** Linux support is planned for Guest Configuration resources soon.

---

For more details and updates, visit the [Azure Arc Blog](https://techcommunity.microsoft.com/t5/s/gxcuf89792/m_assets/avatars/default/avatar-4.svg?image-dimensions=50x50) or join future community forums for direct engagement.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-arc-monthly-forum-recap-november-2025/ba-p/4478127)
