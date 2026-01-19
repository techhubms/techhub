---
external_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/introducing-customizable-security-baselines-in-azure-policy-and/ba-p/4469678
title: Customizable Security Baselines Now in Preview for Azure Policy and Machine Configuration
author: mutemwamasheke
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-13 16:44:41 +00:00
tags:
- ARM Templates
- Azure Arc
- Azure CLI
- Azure Policy
- Bicep
- CIS Benchmarks
- Compliance Automation
- Configuration Management
- Continuous Compliance
- DevOps Integration
- Linux
- Machine Configuration
- Policy as Code
- Security Baselines
- Windows Server
section_names:
- azure
- devops
- security
---
mutemwamasheke presents an overview of customizable security baselines within Azure Policy and Machine Configuration, offering practical guidance for compliance automation and integration with DevOps workflows.<!--excerpt_end-->

# Customizable Security Baselines Now in Preview for Azure Policy and Machine Configuration

Azure Policy and Machine Configuration have launched public preview support for fully customizable security baselines—enabling organizations to tailor compliance controls and automate governance across Windows and Linux workloads, both in Azure and hybrid (Arc-enabled) environments.

## Background

Azure Machine Configuration (formerly Azure Policy Guest Configuration) gives you built-in and custom configuration-as-code for auditing and enforcement of OS, app, and workload-level settings at scale. This works for machines in Azure as well as hybrid Azure Arc-enabled servers.

## What's New?

- **Customizable Security Baselines**: Modify and assign industry-standard security benchmarks (including CIS and Microsoft Azure Compute Security Baselines) to align with your organization's internal policies.
- **Parameterization and Automation**: Adjust rules and settings, apply organization-specific parameters, and export configurations as JSON artifacts for policy-as-code workflows.
- **Continuous Compliance Visibility**: Automated assessment and reporting of compliance status across environments using Azure Policy, Azure Resource Graph, and Guest Assignments.

## Key Scenarios

### Baseline Customization

- Modify settings from built-in baselines (enable, exclude, adjust rules)
- Apply organization-specific controls/parameters
- Export JSON configuration files for version control and repeatable deployments

### Audit Policy Assignment

- Assign customized baselines using Azure Policy
- Real-time evaluation of configuration compliance
- Track status in Azure Policy, Resource Graph, and Guest Assignments

### Integration and Automation

- Integrate baselines with DevOps pipelines, CI/CD systems, and configuration management workflows
- Deploy configurations via Azure CLI, ARM templates, Bicep, and automation scripts
- Ensure reproducible, traceable compliance across environments

## Supported Standards

- **CIS Linux Benchmarks**: Official CIS controls for Azure-endorsed Linux distributions
- **Azure Compute Security Baselines for Windows**: Controls for Windows Server 2022/2025 in Azure
- **Azure Compute Security Baselines for Linux**: Azure-specific security guidance for Linux workloads

## Availability

- Public preview available in all Azure regions
- Coming soon to Azure Government and Sovereign Clouds

## Getting Started

### Prerequisites

- Deploy the Azure Machine Configuration prerequisite policy initiative (installs Guest Configuration extension)
- Ensure supported Windows/Linux VMs in subscription or management group
- Required permissions: Owner or Resource Policy Contributor

### Step-by-Step Guidance

1. Select a baseline in Azure Policy > Machine Configuration
2. Modify settings as needed
3. Download JSON for repeatable automation
4. Assign policy via Azure Portal, CLI, or CI/CD workflow
5. Review compliance status in Azure Policy and supporting views

## Learn More

- [Official documentation: Azure Machine Configuration security baselines](https://learn.microsoft.com/en-us/azure/governance/machine-configuration/how-to/assign-security-baselines/overview-page)
- [CIS Linux Benchmark](https://learn.microsoft.com/en-us/azure/governance/policy/samples/guest-configuration-baseline-cis-linux)
- [Azure Windows Baseline](https://learn.microsoft.com/en-us/azure/governance/policy/samples/guest-configuration-baseline-windows-server-2025)
- [Azure Linux Baseline](https://learn.microsoft.com/en-us/azure/governance/policy/samples/guest-configuration-baseline-linux)

*Note: Azure Arc-enabled servers incur additional charges with Machine Configuration.*

---

For questions and guidance, review the linked documentation or reach out on the Azure Governance and Management Blog.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/introducing-customizable-security-baselines-in-azure-policy-and/ba-p/4469678)
