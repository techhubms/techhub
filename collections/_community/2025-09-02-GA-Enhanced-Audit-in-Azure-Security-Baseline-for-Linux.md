---
layout: "post"
title: "GA: Enhanced Audit in Azure Security Baseline for Linux"
description: "This post introduces the General Availability of the Enhanced Azure Security Baseline for Linux, which brings scalable audit capabilities to over 1.6 million Linux devices across Azure and hybrid cloud environments. It explains how organizations can leverage Azure Policy and Azure Machine Configuration to continuously audit Linux machines, align with industry benchmarks, and surface compliance data for large-scale visibility. Key features include broad distribution support, granular reporting, integration with Azure OSConfig, and seamless enterprise reporting through Azure dashboards and Microsoft Defender for Cloud."
author: "AmirB"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-governance-and-management/ga-enhanced-audit-in-azure-security-baseline-for-linux/ba-p/4446170"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-02 16:00:00 +00:00
permalink: "/community/2025-09-02-GA-Enhanced-Audit-in-Azure-Security-Baseline-for-Linux.html"
categories: ["Azure", "Security"]
tags: ["Audit Only", "Azure", "Azure Arc", "Azure Governance", "Azure Machine Configuration", "Azure Policy", "Azure Security Baseline", "CIS Controls", "Cloud Security", "Community", "Compliance", "Configuration Drift", "Enterprise Security", "Hybrid Cloud", "Industry Benchmarks", "Linux", "Microsoft Defender For Cloud", "OSConfig", "Remediation", "Reporting", "Resource Graph Explorer", "Security", "Security Auditing"]
tags_normalized: ["audit only", "azure", "azure arc", "azure governance", "azure machine configuration", "azure policy", "azure security baseline", "cis controls", "cloud security", "community", "compliance", "configuration drift", "enterprise security", "hybrid cloud", "industry benchmarks", "linux", "microsoft defender for cloud", "osconfig", "remediation", "reporting", "resource graph explorer", "security", "security auditing"]
---

AmirB details the GA release of Enhanced Azure Security Baseline for Linux, highlighting audit-focused capabilities that help enterprises monitor Linux security compliance at scale.<!--excerpt_end-->

# GA: Enhanced Audit in Azure Security Baseline for Linux

The Enhanced Azure Security Baseline for Linux is now generally available, delivering robust audit-only security and compliance capabilities for over 1.6 million Linux devices across all Azure regions.

## What Is the Azure Security Baseline for Linux?

This baseline consists of preconfigured security recommendations provided through Azure Policy and Azure Machine Configuration. It empowers organizations to continuously audit Linux VMs and Arc-enabled servers against industry standards (such as CIS) without enforcing changes or enabling auto-remediation.

Key audit features include:

- **Granular insights** on each configuration check
- **Reporting aligned to industry benchmarks**
- **Rule-level evidence and context** for easy decision-making
- **Deployment at scale** across Azure and Arc-enabled machines (on-premises/multicloud)

## Key Features

### Broad Distribution Support

Covers a comprehensive list of Linux distributions. See [Supported Client Types](https://learn.microsoft.com/en-us/azure/governance/machine-configuration/overview#supported-client-types).

### Industry-Aligned Audit

Over 200 security controls per machine are audited, including:

- OS hardening
- Network/firewall configs
- SSH and remote access
- Logging and auditing
- Kernel/system service configurations

Each finding details the configuration state and provides actionable context.

### Coverage Across Hybrid/Multi-Cloud

Applies to Azure VMs, as well as Arc-enabled servers in other clouds or on-prem environments—enabling unified policy and security configuration management.

### Powered by Azure OSConfig

Auditing leverages the [Azure OSConfig](https://github.com/Azure/azure-osconfig/) open-source framework for in-depth, high-scale, and low-impact Linux-native assessments.

### Enterprise-Scale Reporting

Audit results appear in:

- Azure Policy compliance dashboard
- Azure Resource Graph Explorer
- Microsoft Defender for Cloud (Security Recommendations)

This ensures you can manage, track, and export compliance and audit data organization-wide.

### Cost Model

The audit capability does not require any premium SKU or special licenses; only charges apply for Azure Arc managed workloads outside of Azure.

## How to Get Started

1. **Review the Quickstart Guide:** [Quickstart: Audit Azure Security Baseline for Linux](https://learn.microsoft.com/en-us/azure/osconfig/quickstart-sec-baseline-mc?tabs=azure-cli)
2. **Assign the Built-In Policy:** Search Azure Policy for “Linux machines should meet requirements for the Azure compute security baseline” and assign.
3. **Monitor Compliance:** Use Azure Policy/Resource Graph to track compliance and spot non-compliant systems.
4. **Remediation Planning:** While auto-remediation is in limited public preview, detailed audit findings empower organizations to plan fixes manually or via automation.

## Benefits

- **Improved security visibility**: Easily track Linux configuration and drift
- **Benchmark alignment**: Prove compliance with industry standards
- **Streamlined reporting**: Organization-scale dashboards and APIs
- **Risk reduction**: Proactively address security gaps in hybrid/cloud Linux estates

For more on capabilities and onboarding, refer to the official documentation.

---
*Authored by AmirB, August 27, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/ga-enhanced-audit-in-azure-security-baseline-for-linux/ba-p/4446170)
