---
layout: post
title: 'Customizable Security Baselines in Azure Machine Configuration: Public Preview'
author: mutemwamasheke
canonical_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/public-preview-introducing-customizable-security-baseline/ba-p/4469678
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-13 17:20:06 +00:00
permalink: /azure/community/Customizable-Security-Baselines-in-Azure-Machine-Configuration-Public-Preview
tags:
- ARM Templates
- Audit Policy
- Azure Arc
- Azure CLI
- Azure Machine Configuration
- Azure Policy
- Bicep
- CI/CD
- CIS Benchmark
- Compliance
- Configuration Management
- Governance
- Linux Server
- Policy as Code
- Security Baseline
- Windows Server
section_names:
- azure
- devops
- security
---
mutemwamasheke presents a detailed overview of customizable security baselines in Azure Machine Configuration, explaining how to tailor industry security standards and automate server compliance workflows.<!--excerpt_end-->

# Customizable Security Baselines in Azure Machine Configuration

## Overview

Azure Machine Configuration (formerly Azure Policy Guest Configuration) allows organizations to enforce and audit security and configuration policies across Azure and hybrid environments, including Azure Arc-enabled servers. With the Public Preview of customizable security baselines, users can now tailor these standards to meet their organization’s specific requirements.

## Key Features

- **Custom Security Baselines**: Adapt industry standards like Center for Internet Security (CIS) benchmarks and Microsoft Azure Compute Security Baselines for both Windows Server and Linux.
- **Policy-as-Code Integration**: Export and manage baseline configurations as JSON artifacts, allowing version control and CI/CD integration.
- **Real-Time Compliance Visibility**: Assign baseline audit policies via Azure Policy and monitor results in Azure Policy, Azure Resource Graph, and Guest Assignments.
- **Automation Support**: Integrate baseline deployment into DevOps pipelines using Azure CLI, ARM templates, Bicep, and other common tools.

## Implementation Steps

### Prerequisites

- Deploy the [Azure Machine Configuration prerequisite policy initiative](https://ms.portal.azure.com/#view/Microsoft_Azure_Policy/InitiativeDetail.ReactView/id/%2Fproviders%2FMicrosoft.Authorization%2FpolicySetDefinitions%2F12794019-7a00-42cf-95c2-882eed337cc8) to install necessary extensions.
- Ensure you have supported Windows or Linux VMs in your Azure subscription or management group.
- Grant Owner or Resource Policy Contributor permissions to create and assign policies.

### How to Use

1. **Select a Baseline**: In Azure Policy’s Machine Configuration tab, choose a relevant standard (CIS, Microsoft Baseline).
2. **Customize Settings**: Use the Modify Settings wizard to enable, exclude, or parameterize rules, matching internal compliance needs.
3. **Export JSON Configuration**: Download your custom baseline for repeatable deployments and integration.
4. **Policy Assignment**: Assign the baseline policy through the Azure portal, CLI, or CI/CD.
5. **Monitor Compliance**: Review near real-time compliance status and findings across Azure Policy, Resource Graph, and Guest Assignments.

## Supported Standards

- **CIS Linux Benchmarks**: Official standards for Azure-endorsed Linux distributions.
- **Azure Compute Security Baseline for Windows**: Security controls for Windows Server 2022 and 2025.
- **Azure Compute Security Baseline for Linux**: Consistent controls for recommended Linux setups.

## DevOps and Automation Integration

Custom baseline configurations can be integrated and automated through:

- **Azure CLI**
- **ARM templates**
- **Bicep**
- **CI/CD pipelines**

This approach ensures all compliance requirements are deployed, audited, and tracked programmatically at scale.

## Availability

Customizable security baselines are available in all public Azure regions. Support for Azure Government and Sovereign Clouds will be added in future releases.

## Learn More

- [Azure Machine Configuration security baselines documentation](https://learn.microsoft.com/en-us/azure/governance/machine-configuration/how-to/assign-security-baselines/overview-page)
- [CIS Benchmark for Linux documentation](https://learn.microsoft.com/en-us/azure/governance/policy/samples/guest-configuration-baseline-cis-linux)
- [Azure Windows Baseline](https://learn.microsoft.com/en-us/azure/governance/policy/samples/guest-configuration-baseline-windows-server-2025)
- [Azure Linux Baseline](https://learn.microsoft.com/en-us/azure/governance/policy/samples/guest-configuration-baseline-linux)

> *Note: Using Azure Machine Configuration on Azure Arc-enabled servers incurs a charge.*

---

_Post by mutemwamasheke_

Version 2.0 · Updated Nov 13, 2025

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/public-preview-introducing-customizable-security-baseline/ba-p/4469678)
