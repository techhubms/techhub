---
layout: "post"
title: "Investigating Microsoft 365 Copilot Activity with Sentinel, Defender XDR, and Purview DSPM for AI Security"
description: "This in-depth community article by Safeena Begum Lepakshi explores how organizations can monitor and secure Microsoft 365 Copilot AI activities using Microsoft Sentinel, Defender XDR, and Purview's Data Security Posture Management (DSPM) for AI. The guide covers ingesting Copilot activity into CloudAppEvents, advanced hunting practices with KQL, gaps in telemetry, and recommended approaches for compliance auditing and incident investigation in AI-powered enterprise environments."
author: "Safeena Begum Lepakshi"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-security-community/investigating-m365-copilot-activity-with-sentinel-defender-xdr/ba-p/4442641"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-14 18:58:58 +00:00
permalink: "/2025-08-14-Investigating-Microsoft-365-Copilot-Activity-with-Sentinel-Defender-XDR-and-Purview-DSPM-for-AI-Security.html"
categories: ["AI", "Security"]
tags: ["Advanced Hunting", "AI", "AI Risk Management", "Audit Log", "CloudAppEvents", "Community", "Compliance", "Data Security Posture Management", "DSPM For AI", "Incident Investigation", "KQL", "M365 Activity Monitoring", "Microsoft 365 Copilot", "Microsoft Defender XDR", "Microsoft Purview", "Microsoft Sentinel", "Security", "Security For AI", "Security Operations", "Sensitive Data", "SOC", "Threat Hunting"]
tags_normalized: ["advanced hunting", "ai", "ai risk management", "audit log", "cloudappevents", "community", "compliance", "data security posture management", "dspm for ai", "incident investigation", "kql", "m365 activity monitoring", "microsoft 365 copilot", "microsoft defender xdr", "microsoft purview", "microsoft sentinel", "security", "security for ai", "security operations", "sensitive data", "soc", "threat hunting"]
---

Safeena Begum Lepakshi details how to leverage Microsoft Sentinel, Defender XDR, and Purview's DSPM for AI to investigate and secure Microsoft 365 Copilot activities, providing actionable steps for security operations teams.<!--excerpt_end-->

# Investigating Microsoft 365 Copilot Activity with Sentinel, Defender XDR, and Purview DSPM for AI Security

**Author:** Safeena Begum Lepakshi

As organizations adopt AI-powered tools such as Microsoft 365 Copilot, ChatGPT, and other generative assistants, ensuring data security becomes critical. AI tools process sensitive enterprise data, making robust monitoring and compliance essential.

## Data Security Posture Management (DSPM) for AI

- DSPM for AI is a Microsoft Purview capability that helps organizations discover, secure, and enforce compliance controls on AI usage.
- It provides personalized recommendations and policy enforcement to mitigate oversharing risks and unauthorized data access in AI interactions.
- Key questions addressed by DSPM:
  - Where is sensitive data stored? Who can access it?
  - Are we protecting data from oversharing or unauthorized access by AI tools?
  - How to maintain compliance and governance for data accessed via AI?

Learn more about [DSPM for AI](https://learn.microsoft.com/en-us/purview/dspm-for-ai).

## Operationalizing DSPM with Sentinel & Defender XDR

Security teams can operationalize DSPM for AI through Microsoft Sentinel and Defender XDR. The process involves capturing telemetry from Copilot interactions, building investigations, and responding to security incidents.

### 1. Ingesting Copilot Activity: CloudAppEvents Table

- **CloudAppEvents** is an advanced hunting table populated by Microsoft Defender for Cloud Apps.
- To use this feature, organizations must enable Microsoft 365 activities within Defender XDR.
- Follow [detailed deployment steps](https://learn.microsoft.com/en-us/defender-xdr/advanced-hunting-cloudappevents-table) to ensure CloudAppEvents is properly populated.
- The table contains enriched logs from all SaaS apps connected via Microsoft Defender for Cloud Apps.

### 2. Activity Explorer in DSPM for AI

- The Activity Explorer hub in Purview DSPM for AI surfaces granular telemetry about every AI interaction, including:
  - Prompts and responses (if permissions/policies allow)
  - User identities
  - Sensitivity information types (SITs)
  - Application and agent details
- Provides visibility for risk assessment and compliance.

### 3. Unified Audit and Data Flow

- All AI interactions (from Copilot, Fabric, third-party GenAI apps, etc.) generate events in **Microsoft 365 Unified Audit Log**.
- These are ingested into the **CloudAppEvents** table within Defender XDR if configured.
- The same CloudAppEvents are exposed in Microsoft Sentinel, allowing:
  - Threat hunting
  - Incident investigation
  - Compliance correlation
- No additional connectors are required.

## Practical Use Cases

- **Advanced Hunting:** Use KQL to query for AI interactions by sensitivity labels, app types, or user risk.
- **Incident Investigation:** Correlate AI activity with Office 365 alerts.
- **Compliance Audits:** Track activity for audit trails.
- **Custom Dashboards:** Visualize AI usage with Power BI or Sentinel dashboards.

### Example KQL Query

```kql
CloudAppEvents
| where Application in ("Microsoft 365", "Microsoft 365 Copilot Chat")
| where ActivityType == "Interactwithcopilot"
```

## Known Gaps and Recommendations

- **CloudAppEvents** provides metadata (timestamp, user, app/agent names, action type) but does not include prompt/response contents or DSPM enrichment (SITs, policy hits).
- For detailed investigations, pivot to **Purview DSPM Activity Explorer** to inspect full AI interactions with sensitivity context.

## Resources and Further Reading

- [DSPM for AI](https://learn.microsoft.com/en-us/purview/dspm-for-ai)
- [CloudAppEvents Table Docs](https://learn.microsoft.com/en-us/defender-xdr/advanced-hunting-cloudappevents-table)
- [Deploy Supported Services for Defender XDR](https://learn.microsoft.com/en-us/defender-xdr/deploy-supported-services)

---

**Acknowledgements:** Special thanks to Martin Gagné, Principal Group Engineering Manager, for reviewing and providing feedback on this work.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-security-community/investigating-m365-copilot-activity-with-sentinel-defender-xdr/ba-p/4442641)
