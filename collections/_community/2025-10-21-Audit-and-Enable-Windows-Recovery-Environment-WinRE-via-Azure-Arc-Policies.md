---
layout: "post"
title: "Audit and Enable Windows Recovery Environment (WinRE) via Azure Arc Policies"
description: "This article explains the Public Preview of Azure Policies designed to audit and enable Windows Recovery Environment (WinRE) on Azure Arc-enabled Windows Servers. IT administrators can leverage these policies, delivered through the Machine Configuration component of the Azure Connected Machine agent, to centrally manage and automate WinRE compliance across hybrid and multicloud server fleets. The content covers policy scope, supported licensing, deployment details, and highlights Azure Arc's role in maintaining resilience in cross-environment Windows Server deployments."
author: "Aurnov_Chattopadhyay"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-audit-and-enable-windows-recovery-environment/ba-p/4462939"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-21 00:53:23 +00:00
permalink: "/2025-10-21-Audit-and-Enable-Windows-Recovery-Environment-WinRE-via-Azure-Arc-Policies.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Arc", "Azure Policy", "Community", "Compliance", "Configuration Management", "Connected Machine Agent", "Defender For Servers", "ESU", "Hybrid Cloud", "Machine Configuration", "Policy Enforcement", "Security", "Server Resiliency", "Windows Recovery Environment", "Windows Server", "WinRE"]
tags_normalized: ["azure", "azure arc", "azure policy", "community", "compliance", "configuration management", "connected machine agent", "defender for servers", "esu", "hybrid cloud", "machine configuration", "policy enforcement", "security", "server resiliency", "windows recovery environment", "windows server", "winre"]
---

Aurnov_Chattopadhyay details how Azure Arc's new Public Preview policies enable IT admins to audit and configure Windows Recovery Environment (WinRE) settings across Arc-enabled Windows Servers, providing resilient configuration management for hybrid environments.<!--excerpt_end-->

# Audit and Enable Windows Recovery Environment (WinRE) for Azure Arc-enabled Servers

**Author:** Aurnov_Chattopadhyay  
**Published:** Oct 21, 2025

## Overview

Windows Recovery Environment (WinRE) is a dedicated partition in Windows that provides critical diagnostics and repair capabilities, helping IT administrators recover from failures such as blue screen errors. For enterprises running mission-critical workloads, ensuring WinRE is enabled and healthy is central to operational resilience.

With this Public Preview, Azure Arc now offers Azure Policies to audit and enable WinRE across any fleet of Arc-enabled Windows Servers. These policies use the Machine Configuration component of the Azure Connected Machine agent, delivering secure, policy-based configuration at scale.

## Key Features

- **Audit WinRE Configuration**: Azure Policy checks WinRE status and health across all targeted servers.
- **Enable WinRE Remotely**: If WinRE is not enabled (but partitioned), Azure Policy can turn it on as part of a compliance remediation process.
- **Integrated with Machine Configuration**: Enforcement and audit leverage the Azure Connected Machine agent for secure, automated compliance.
- **Flexible Licensing Support**: Policies are available at no extra cost for:
  - Windows Server 2012 Extended Security Updates (ESUs)
  - Microsoft Defender for Servers Plan 2
  - Windows Server Software Assurance attestation
  - Windows Server Pay-as-you-Go licensing
- **Charges for Other Servers**: Licensing not covered above will incur Azure Machine Configuration charges.

## How It Works

1. **Deploy Azure Policy**: Assign policies to Arc-enabled Windows Servers in your Azure Subscription.
2. **Audit**: The Connected Machine agent reports WinRE configuration and health.
3. **Remediate**: If needed, use Policy to enable WinRE (if partition is provisioned).

### Policy Resources

- [Audit Windows machines that do not have Windows Recovery Environment (WinRE) enabled](https://portal.azure.com/#blade/Microsoft_Azure_Policy/PolicyDetailBlade/definitionId/%2Fproviders%2FMicrosoft.Authorization%2FpolicyDefinitions%2Fea13dcd3-9b62-4fa0-9462-1695a76d0e59)
- [Configure Windows Recovery Environment (WinRE) on Windows machines](https://portal.azure.com/#blade/Microsoft_Azure_Policy/PolicyDetailBlade/definitionId/%2Fproviders%2FMicrosoft.Authorization%2FpolicyDefinitions%2Fdd970030-1eb5-4d03-b72e-e2c59dcbb202)

## Use Cases

- Improve recovery and security posture across hybrid, edge, or multicloud Windows Server deployments
- Central administrative control of critical OS health features
- Automated reporting and remediation for compliance

## Conclusion

Auditing and enabling WinRE through Azure Arc policies enhances resilience and manageability for hybrid Windows Server environments, leveraging native Azure security, automation, and compliance tooling.

---

For hands-on deployment, follow the policy links provided above.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-audit-and-enable-windows-recovery-environment/ba-p/4462939)
