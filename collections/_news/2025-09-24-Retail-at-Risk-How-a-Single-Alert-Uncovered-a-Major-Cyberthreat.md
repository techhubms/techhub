---
external_url: https://www.microsoft.com/en-us/security/blog/2025/09/24/retail-at-risk-how-one-alert-uncovered-a-persistent-cyberthreat/
title: 'Retail at Risk: How a Single Alert Uncovered a Major Cyberthreat'
author: Microsoft Incident Response
feed_name: Microsoft Security Blog
date: 2025-09-24 17:00:00 +00:00
tags:
- Attack Detection
- Azure Virtual Desktop
- Cloud Security
- Credential Security
- Cybersecurity
- DART
- Forensics
- Identity Management
- Incident Response
- MFA
- Microsoft Defender
- Microsoft Entra ID
- Microsoft Incident Response
- Microsoft Sentinel
- Remote Desktop Protocol
- Retail Security
- SharePoint Vulnerabilities
- Threat Intelligence
- Zero Trust
section_names:
- azure
- security
---
Microsoft Incident Response’s expert team details how a single alert in a retail environment led to the discovery of a persistent threat. This report highlights actionable forensic insights and security guidance for practitioners.<!--excerpt_end-->

# Retail at Risk: How a Single Alert Uncovered a Major Cyberthreat

## Overview

Microsoft Incident Response – Detection and Response Team (DART) investigates key incidents targeting retail sectors, focusing on operational disruptions caused by unpatched SharePoint vulnerabilities and compromised identities. This report presents case studies, forensic findings, and actionable defense strategies.

## What Happened?

- Initial Alert: A “Possible web shell installation” was detected by Microsoft Defender Experts on a SharePoint server. Investigation found a malicious ASPX file exploiting CVE-2025-49706 and CVE-2025-49704.
- Attack Methods: Attackers exploited unpatched SharePoint, performed identity spoofing, escalated via Microsoft Entra ID and Graph API, and used tools like Azure Virtual Desktop, RDP, PsExec, and Azure CLI.
- Persistence: Attackers maintained control by abusing self-service password resets and registering devices for MFA. Directory exploration and credential manipulation were confirmed through Entra ID risk events.

## Microsoft's Response

- DART isolated and reclaimed both on-premises and cloud identity systems (Active Directory, Entra ID), deprivileged compromised accounts, and revoked access tokens.
- Malicious web shells were promptly removed, and custom forensic tools analyzed attacker activity.
- Zero Trust recommendations were applied: MFA enforcement, patching vulnerable systems, mass password resets, and evidence preservation.
- Threat intelligence combined Microsoft Defender and Sentinel insights to inform actions.

## Key Insights and Recommendations

- **Identity Management is Critical:** Lack of separation between privileged and standard accounts increased exposure to lateral movement. Nine of 20 tested accounts had inappropriate elevated access.
- **Speed of Attacks:** Attackers acted within moments of compromise, with hands-on-keyboard behaviors demanding real-time monitoring and response.
- **Behavioral Analytics:** Custom web shells bypassed basic detections, stressing the need for advanced behavioral tools.
- **Defense in Depth:** Customers should regularly patch environments, enforce MFA everywhere, conduct vulnerability scans, monitor for behavioral anomalies, and have a robust response plan.
- **Zero Trust Principles:** Apply least privilege, enforce strong authentication, and centralize logging with threat intelligence.

## Actionable Steps for Practitioners

- Patch all known vulnerabilities proactively—especially in externally accessible portals like SharePoint.
- Segregate standard and privileged identities, audit for over-privileged accounts.
- Deploy endpoint detection and response (EDR) and enable advanced threat protection with Microsoft Defender and Sentinel.
- Preserve forensic evidence on detection of major alerts; engage expert response if persistent attacks are suspected.
- Apply Zero Trust strategies including MFA, centralized logging, identity isolation, and regular password rotations.

## Resources

- [Download the full Cyberattack report](https://go.microsoft.com/fwlink/?linkid=2336417)
- [Microsoft Security Blog](https://www.microsoft.com/en-us/security/blog)
- [Microsoft Incident Response](https://www.microsoft.com/security/business/microsoft-incident-response)
- [Zero Trust Overview](https://learn.microsoft.com/en-us/security/zero-trust/zero-trust-overview)

## About the Cyberattack Series

The Cyberattack Series from Microsoft Incident Response uncovers how DART investigates and mitigates sophisticated cyberthreats, offering transparency and technical insight for security practitioners globally.

## About the Author

Microsoft Incident Response is comprised of security engineers, investigators, and researchers dedicated to helping organizations prevent, investigate, and recover from cybersecurity incidents.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/09/24/retail-at-risk-how-one-alert-uncovered-a-persistent-cyberthreat/)
