---
section_names:
- ai
- security
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/21/detection-strategies-cloud-identities-against-infiltrating-it-workers/
title: Detection strategies across cloud and identities against infiltrating IT workers
tags:
- Advanced Hunting
- AI
- API Telemetry
- Cisco Webex
- CloudAppEvents
- DocuSign
- External User Monitoring
- Generative AI Tradecraft
- Identity Verification
- Impossible Travel Alerts
- Jasper Sleet
- Kusto Query Language (kql)
- Microsoft Defender For Cloud Apps
- Microsoft Defender XDR
- Microsoft Teams
- Microsoft Threat Intelligence
- News
- North Korea Threat Actor
- OAuth
- OAuth Tokens
- Recruitment Workflow Security
- Remote IT Worker Fraud
- SaaS Security
- Security
- Sleet
- Threat Hunting
- Workday
- Workday Recruiting Web Service
- Zoom
feed_name: Microsoft Security Blog
date: 2026-04-21 16:03:09 +00:00
author: Microsoft Defender Security Research Team and Microsoft Threat Intelligence
primary_section: ai
---

The Microsoft Defender Security Research Team and Microsoft Threat Intelligence describe how the Jasper Sleet threat actor abuses remote hiring workflows (using AI-assisted deception) and share practical Defender for Cloud Apps and Defender XDR hunting/detection guidance focused on Workday recruiting and onboarding telemetry.<!--excerpt_end-->

# Detection strategies across cloud and identities against infiltrating IT workers

The shift to remote and hybrid work expanded global hiring and accelerated digital onboarding, increasing reliance on online identity verification and remote access. Threat actors such as **Jasper Sleet** (North Korea-aligned) exploit this model by posing as legitimate hires using stolen or fabricated identities and **AI-assisted deception** to gain trusted access, generate revenue, and in some cases enable data theft, extortion, or follow-on compromise.

In the initial job-discovery phase, these fraudulent applicants survey organization career sites and external hiring portals to identify active technical roles and recruitment workflows. A related Microsoft Threat Intelligence blog notes how actors use **generative AI at scale** to analyze job postings, extract role-specific language/skills/certifications/tooling expectations, and then build tailored fake personas and convincing applications.

This article uses **Workday** as a concrete example (due to widespread adoption and available event logs) and explains how customers can use **Microsoft Defender for Cloud Apps** and **Microsoft Defender XDR** to monitor/detect fraudulent activity across:

- Pre-recruitment (job discovery/application)
- Recruiting (communications and document workflows)
- Post-recruitment (onboarding, sign-ins, and data access)

## Attack chain overview

Threat actors leverage routine HR workflows like external-facing career sites with open job postings. After they are contacted, interviewed, and hired, they complete new-hire onboarding steps (for example, payroll setup) through the HR SaaS platform.

![Jasper Sleet attack chain timeline](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Jasper-Sleet-attack-chain.webp)

## Activities in the pre-recruitment phase (Workday)

Microsoft observed Jasper Sleet accessing **Workday Recruiting Web Service** endpoints exposed via external career sites, from known actor infrastructure and email accounts.

Workday can expose internal/non-public APIs (like Recruiting Web Service) to allow programmatic access for browsing and applying for jobs. This typically involves:

- Setting up **OAuth clients** and associated **OAuth tokens**
- Exposing APIs for use by external career sites

Microsoft observed API call events in Workday telemetry to `hrrecruiting/*` endpoints that access job posting and application data and submit applications/questionnaires.

Examples of API calls seen:

- `hrrecruiting/accounts/*`
- `hrrecruiting/jobApplicationPackages/*`
- `hrrecruiting/validateJobApplication/*`
- `hrrecruiting/resumes/*`

![Sample Workday hrrecruiting API activity view](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-54.webp)

### Why this stands out from normal applicant behavior

These API calls can be legitimate. The suspicious signal Microsoft calls out is repeated, consistent access patterns across **multiple external accounts** from known Jasper Sleet infrastructure.

### Defender for Cloud Apps: Workday connector

Defender for Cloud Apps’ Workday connector can help:

- View/track API activity to `*/hrrecruiting*` endpoints
- Identify external accounts and associated infrastructure metadata
- Match indicators against threat intel to flag fraudulent applications early

Workday connector documentation:

- [How Defender for Cloud Apps helps protect your Workday environment](https://learn.microsoft.com/en-us/defender-cloud-apps/protect-workday)

## Activities in the recruiting phase (communications and documents)

During recruiting, additional signals may exist outside Workday:

- Email and meeting coordination using **Microsoft Teams**, **Zoom**, or **Cisco Webex**
- Hiring documentation workflows such as **DocuSign**

Microsoft suggests using **advanced hunting** to track suspicious communications (for example, external accounts originating from suspicious IPs or emails potentially tied to the actor).

Connector documentation:

- [How Defender for Cloud Apps helps protect your Zoom environment](https://learn.microsoft.com/en-us/defender-cloud-apps/protect-zoom)
- [How Defender for Cloud Apps helps protect your Cisco Webex environment](https://learn.microsoft.com/en-us/defender-cloud-apps/protect-webex)
- [How Defender for Cloud Apps helps protect your DocuSign environment](https://learn.microsoft.com/en-us/defender-cloud-apps/protect-docusign)

## Activities in the post-recruitment phase (onboarding and internal access)

Once hired, a legitimate account is created as part of onboarding. In Workday onboarding scenarios, Microsoft observed:

- Sign-ins to newly created Workday profiles
- Payroll setup/changes originating from known Jasper Sleet infrastructure

![Sample payroll change event](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-55.webp)

After onboarding, the actor may access internal SaaS apps including **Teams, SharePoint, OneDrive, and Exchange Online**. Microsoft recommends investigating alerts associated with new-hire accounts, especially:

- Location anomalies
- Anonymous proxy usage
- Search/download spikes against Microsoft 365 or other SaaS apps

Microsoft notes spikes in **impossible travel** alerts for these new hires in early months.

![Impossible travel alerts example](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-56.webp)

## Mitigation and protection guidance

Microsoft recommends correlating telemetry across multiple sources and monitoring behavioral anomalies in recruitment/onboarding, using threat intelligence when available.

Key actions:

- Enable Defender for Cloud Apps connectors to gain visibility into activity from external user accounts and newly hired internal users originating from malicious infrastructure.
- Train employees to recognize social engineering patterns during hiring and when interacting with new hires.

Reference:

- [Jasper Sleet: North Korean remote IT workers’ evolving tactics to infiltrate organizations](https://www.microsoft.com/en-us/security/blog/2025/06/30/jasper-sleet-north-korean-remote-it-workers-evolving-tactics-to-infiltrate-organizations/)

## Microsoft Defender XDR detections

Microsoft lists detections that may apply across the recruitment/onboarding attack chain, including Defender for Cloud Apps coverage for Workday-related suspicious activity and Defender XDR coverage such as **impossible travel**.

Microsoft also notes that customers with provisioned access can use **Microsoft Security Copilot in Microsoft Defender** to investigate and respond to incidents.

## Threat intelligence reports

Defender XDR customers can review the actor profile in the Defender portal:

- [Actor profile: Jasper Sleet](https://security.microsoft.com/threatanalytics3/3ed6ce54-7d04-4819-aa9a-7f3085ea4a49/overview)

Microsoft Security Copilot integration references:

- [Turn on the security copilot integration in Defender TI](https://learn.microsoft.com/defender/threat-intelligence/security-copilot-and-defender-threat-intelligence?bc=%2Fsecurity-copilot%2Fbreadcrumb%2Ftoc.json&toc=%2Fsecurity-copilot%2Ftoc.json#turn-on-the-security-copilot-integration-in-defender-ti)
- [Using Copilot in the embedded Defender portal experience](https://learn.microsoft.com/defender/threat-intelligence/using-copilot-threat-intelligence-defender-xdr)

## Hunting queries (examples)

### Access to Workday Recruiting Web Service API by external users

```kusto
let api_endpoint_regex = 'hrrecruiting/*';
CloudAppEvents
| where Application == 'Workday'
| where IsExternalUser
| where ActionType matches regex api_endpoint_regex
| where IPAddress in () or AccountId in ()
| summarize make_set(ActionType) by AccountId, IPAddress, bin(Timestamp, 1d)
```

### Emails and Teams communications related to interviews

```kusto
//Email communications
EmailEvents
| where SenderMailFromAddress == "" or RecipientEmailAddress == ""
| where Subject has "Interview"
| project Timestamp, SenderMailFromAddress, SenderDisplayName, SenderIPv4, SenderIPv6, RecipientEmailAddress, Subject, DeliveryAction, DeliveryLocation;

EmailEvents
| where SenderIPv4 == "" or SenderIPv6 == ""
| where Subject has "Interview"
| project Timestamp, SenderMailFromAddress, SenderDisplayName, SenderIPv4, SenderIPv6, RecipientEmailAddress, Subject, DeliveryAction, DeliveryLocation;

//Microsoft Teams communications
CloudAppEvents
| where Application == "Microsoft Teams"
| where IsExternalUser
| where AccountId == "" or AccountDisplayName == ""
| summarize make_set(ActionType) by IPAddress, AccountId, bin(Timestamp, 1d);

CloudAppEvents
| where Application == "Microsoft Teams"
| where IsExternalUser
| where IPAddress == ""
| summarize make_set(ActionType) by IPAddress, AccountId, bin(Timestamp, 1d);

//Zoom or Cisco Webex communication events after enabling the Microsoft Defender for Cloud apps connectors
CloudAppEvents
| where Application == "Zoom"
| where IsExternalUser
| where IPAddress == ""
| summarize make_set(ActionType) by IPAddress, AccountId, bin(Timestamp, 1d);

CloudAppEvents
| where Application == "Cisco Webex"
| where IsExternalUser
| where IPAddress == ""
| summarize make_set(ActionType) by IPAddress, AccountId, bin(Timestamp, 1d)
```

### Hiring phase involving signing agreements through DocuSign

```kusto
CloudAppEvents
| where Application == "DocuSign"
| where IsExternalUser
| where ActionType == "ENVELOPE SIGNED"
| where IPAddress in ("") or AccountId == ""
```

### New hire onboarding/payroll activities from known Jasper Sleet infrastructure

```kusto
CloudAppEvents
| where Application == "Workday"
| where AccountId == ""
| where ActionType has_any ("Add", "Change", "Assign", "Create", "Modify")
    and ActionType has_any ("Account", "Bank", "Payment", "Tax")
| where IPAddress in ("")
| summarize make_set(ActionType) by IPAddress, bin(Timestamp, 1d)
```

## References

- [Detection strategies across cloud and identities against infiltrating IT workers](https://www.microsoft.com/en-us/security/blog/2026/04/21/detection-strategies-cloud-identities-against-infiltrating-it-workers/)
- [Jasper Sleet: North Korean remote IT workers’ evolving tactics to infiltrate organizations](https://www.microsoft.com/en-us/security/blog/2025/06/30/jasper-sleet-north-korean-remote-it-workers-evolving-tactics-to-infiltrate-organizations/)
- [AI as tradecraft: How threat actors operationalize AI](https://www.microsoft.com/en-us/security/blog/2026/03/06/ai-as-tradecraft-how-threat-actors-operationalize-ai/)
- [Microsoft Threat Intelligence Blog](https://aka.ms/threatintelblog)

## Learn more

- [Securing Copilot Studio agents with Microsoft Defender](https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection)
- [Zero Trust for AI workshop](https://microsoft.github.io/zerotrustassessment/)
- [Protect your agents in real-time during runtime (Preview)](https://learn.microsoft.com/en-us/defender-cloud-apps/real-time-agent-protection-during-runtime)

## Notes on source and attribution

This research is provided by **Microsoft Defender Security Research** with contributions from members of **Microsoft Threat Intelligence**.


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/21/detection-strategies-cloud-identities-against-infiltrating-it-workers/)

