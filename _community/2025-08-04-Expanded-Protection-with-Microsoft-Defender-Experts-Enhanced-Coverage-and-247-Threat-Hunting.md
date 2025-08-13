---
layout: "post"
title: "Expanded Protection with Microsoft Defender Experts: Enhanced Coverage and 24/7 Threat Hunting"
description: "This article announces and details expanded coverage for Microsoft Defender Experts for XDR and Hunting, introducing 24/7 protection and proactive threat hunting for hybrid and multicloud servers. Third-party network signals integration, flexible per-server pricing, and new requirements for third-party enrichment are highlighted. Implementation guidance and documentation links are provided."
author: "henryyan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/elevate-your-protection-with-expanded-microsoft-defender-experts/ba-p/4439134"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-04 16:00:00 +00:00
permalink: "/2025-08-04-Expanded-Protection-with-Microsoft-Defender-Experts-Enhanced-Coverage-and-247-Threat-Hunting.html"
categories: ["AI", "Azure", "Security"]
tags: ["AI", "Azure", "Cloud Security", "Community", "Defender For Cloud", "Fortinet", "Hybrid Cloud", "Incident Enrichment", "Managed Detection And Response", "Microsoft Defender Experts", "Microsoft Sentinel", "Multicloud Security", "Palo Alto Networks", "Security", "SOC", "Third Party Integration", "Threat Hunting", "Virtual Machines", "XDR", "Zscaler"]
tags_normalized: ["ai", "azure", "cloud security", "community", "defender for cloud", "fortinet", "hybrid cloud", "incident enrichment", "managed detection and response", "microsoft defender experts", "microsoft sentinel", "multicloud security", "palo alto networks", "security", "soc", "third party integration", "threat hunting", "virtual machines", "xdr", "zscaler"]
---

Authored by henryyan, this article covers the latest enhancements to Microsoft Defender Experts, focusing on 24/7 protection, threat hunting, and enriched detection capabilities for hybrid and multicloud environments.<!--excerpt_end-->

## Overview

Security Operations Centers (SOCs) are experiencing substantial pressure due to increasingly sophisticated and frequent cyberattacks, many of which are AI-driven, combined with persistent skills shortages in the cybersecurity talent pool. To help organizations rise to these challenges, Microsoft Defender Experts for XDR (Extended Detection and Response) and Defender Experts for Hunting provide managed services to bolster defenses and ensure readiness against evolving threats.

## Expanded Coverage and Capabilities

### General Availability of Expanded Coverage

Microsoft announces the general availability of expanded Microsoft Defender Experts coverage, bringing 24/7, expert-led protection and proactive threat hunting to hybrid and multicloud servers—starting with servers managed through Microsoft Defender for Cloud. The update enables organizations to:

- Receive round-the-clock monitoring by experienced analysts for both on-premises and cloud virtual machines (VMs) across Azure, AWS, and Google Cloud Platform.
- Benefit from continuous, proactive threat hunting that now extends beyond endpoints, identities, email, and apps to include cloud workloads.

### Third-Party Network Signals for Incident Enrichment

Incident response within Defender Experts for XDR is now enhanced by integrating third-party network signals from:

- Palo Alto Networks (PAN-OS Firewall)
- Zscaler (Internet & Private Access)
- Fortinet (FortiGate Next-Generation Firewall)

This expanded context supports:

- Improved detection accuracy by correlating data across domains and vendors.
- Accelerated response to emerging threats, thanks to deeper insight into attack paths and patterns.

#### Hypothetical Scenario Illustration

1. **Detection**: An "Atypical Travel" alert flags suspicious user activity, with logins from disparate geographic regions on different devices.
2. **Correlation**: Third-party logs reveal unauthorized access attempts and encrypted traffic targeting legacy infrastructure.
3. **Investigation**: Analysts expose a bypass of conditional access due to configuration issues, confirming lateral movement and potential data exfiltration.
4. **Response**: Immediate remedial actions are triggered—revoking sessions, isolating endpoints, and enforcing stricter mobile access protocols.

## Flexible, Cost-Effective Pricing

- Billing is based on the cumulative server hours protected each month (per-server, per-month model), allowing organizations to pay only for actual Defender Experts utilization.
- There are no additional charges for third-party signal enrichment, beyond the standard data ingestion fees in Microsoft Sentinel.

## Deployment and Requirements

### To enable expanded coverage

- You need either a Defender Experts for XDR or Defender Experts for Hunting license.
- Defender for Servers Plan 1 or Plan 2 in Defender for Cloud is required.
- Only one license is needed for organization-wide server coverage.

### For third-party enrichment

- Deploy Microsoft Sentinel and onboard it with Microsoft Defender.
- At least one supported network signal must be ingested (from Palo Alto, Zscaler, or Fortinet) via Sentinel built-in connectors.

## Getting Started

- Defender Experts coverage of servers is available as an add-on to Defender Experts for XDR or Defender Experts for Hunting.
- Contact your Microsoft account representative or complete the online interest form for purchasing.
- Existing Defender Experts for XDR customers should reach out to their Service Delivery Manager to enable third-party enrichment features.

## Additional Resources

- [Microsoft Defender Experts for XDR Documentation](https://learn.microsoft.com/defender-xdr/dex-xdr-overview)
- [Microsoft Defender Experts for Hunting](https://learn.microsoft.com/defender-xdr/defender-experts-for-hunting)
- [Third-party network signals for enrichment](https://learn.microsoft.com/defender-xdr/third-party-enrichment-defender-experts)
- [Plan Defender for Servers deployment](https://learn.microsoft.com/en-us/azure/defender-for-cloud/plan-defender-for-servers)
- [Defender Experts Ninja Training](https://techcommunity.microsoft.com/blog/microsoftsecurityexperts/welcome-to-the-microsoft-defender-experts-ninja-hub/4061509)

## Conclusion

Microsoft is responding to modern security needs with expanded, expertly managed protection through Defender Experts for XDR and Hunting—delivering 24/7 defense, deeper context for incident response, and cost-effective, scalable solutions for multicloud and hybrid environments.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/elevate-your-protection-with-expanded-microsoft-defender-experts/ba-p/4439134)
