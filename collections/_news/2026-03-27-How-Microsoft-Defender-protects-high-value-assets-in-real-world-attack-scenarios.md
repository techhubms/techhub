---
author: Microsoft Defender Security Research Team
date: 2026-03-27 19:53:53 +00:00
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/27/microsoft-defender-protects-high-value-assets/
section_names:
- security
feed_name: Microsoft Security Blog
title: How Microsoft Defender protects high-value assets in real-world attack scenarios
primary_section: security
tags:
- Active Directory
- AMSI
- Attack Path Analysis
- Automatic Attack Disruption
- Cloud Delivered Protection
- Credential Dumping
- Credential Theft
- Domain Controllers
- EWS
- Exchange Server
- Exposure Graph
- High Value Assets
- HVA Protection
- Identity Infrastructure
- IIS
- Incident Response
- Lateral Movement
- Microsoft Defender
- Microsoft Entra ID
- Microsoft Security Exposure Management
- News
- NTDS.DIT
- Ntdsutil.exe
- NTLM Relay
- Reverse SSH Tunnel
- Scheduled Tasks
- Security
- SharePoint Server
- Threat Detection
- Tier 0 Systems
- Vulnerability Triage
- Webshell
---

The Microsoft Defender Security Research Team explains how Microsoft Defender uses high-value asset (HVA) context and Microsoft Security Exposure Management to improve detection and prevention, illustrated with real-world scenarios like domain controller credential theft and Exchange/IIS webshell remediation.<!--excerpt_end-->

# How Microsoft Defender protects high-value assets in real-world attack scenarios

High-Value Assets (HVAs)—like domain controllers, internet-facing web servers, and identity infrastructure—are frequent targets in sophisticated intrusions. This article explains how Microsoft Defender applies asset-aware protections using Microsoft Security Exposure Management to add context (device role, criticality, attack paths) to detections, and shows how that changes prevention outcomes in real attack scenarios.

## Why HVAs need different protection

Not all systems carry the same risk. Compromise of certain assets can have outsized blast radius (for example, domain controllers managing authentication/authorization).

Examples of HVAs called out in the article:

- Domain controllers (Active Directory)
- Web servers hosting business-critical apps (including Exchange and SharePoint)
- Identity systems spanning on-premises and cloud
- Certificate authorities
- Internet-facing services that provide access to corporate apps

Microsoft notes that in more than **78%** of analyzed human-operated attacks, adversaries successfully compromise an HVA (such as a domain controller), enabling elevated access.

## Using asset context to strengthen detection

Traditional endpoint detections often rely on signals like:

- Process execution
- Command-line activity
- File operations

Those signals can be ambiguous because legitimate admin activity can resemble attacker tradecraft.

The key idea here is context: the same activity may be routine on a general-purpose server but high-risk on a Tier-0 system (like a domain controller).

Defender enriches detection using a **critical asset framework** powered by **Microsoft Security Exposure Management**, which provides:

- Critical asset identification and tagging
- Attack path context
- Cross-workload relationships

This enables deeper, role-aware detections and stronger prevention decisions for HVAs.

## How high-value asset protection works

1. **Asset classification**
   - Security Exposure Management builds an inventory and **exposure graph** across devices, identities, cloud resources, and external attack surface.
   - It enriches assets with role-based classifications and criticality levels.
   - It can automatically identify and tag HVAs across on-prem, hybrid, and cloud.

2. **Real-time differentiated intelligence from cloud**
   - HVA-aware anomaly detection extends cloud-delivered protection.
   - It learns baselines of “normal” activity for critical assets.
   - It evaluates behavior based on role, sensitivity, and expected operational patterns (instead of one-size-fits-all thresholds).

3. **Endpoint-delivered protections**
   - Prioritizes and blocks high-impact TTPs on HVAs.
   - Uses asset role and critical asset intelligence to elevate weak signals into high-confidence prevention on Tier-0 systems.

## Real-world high-value asset protection scenarios

### Focused protection for domain controllers

Domain controllers (Active Directory) are targeted to gain elevated privileges. A common technique is extracting credentials from **NTDS.DIT** (the AD database containing password hashes and account information).

In the described incident:

- A threat actor compromises an internet-exposed server (**Machine 0**) and establishes persistence.
- They move laterally to another internal server (**Machine 1**).
- They establish a **reverse SSH tunnel**, bypass inbound firewall restrictions, and set up an **NTLM relay trap**.
- Authentication activity from **Machine 2** (with **Domain Admin** privileges) interacts with the relay setup.
- The actor uses the captured NTLM exchange to authenticate with elevated privileges.
- With Domain Admin access, they authenticate to **Machine 3** (a **domain controller**) and attempt to dump NTDS.DIT using **`ntdsutil.exe`**.

Defender’s HVA protections:

- Block the command-line attempt before database access.
- Trigger **automated disruption**, disabling the Domain Admin account to stop progression.

A second domain-controller technique described:

- The adversary remotely creates a scheduled task on a domain controller.
- The task runs as **SYSTEM**, executes `ntdsutil.exe` to generate an AD database backup, then is deleted to reduce forensic visibility.

Individually, “remote scheduled task creation” and `ntdsutil.exe` execution can occur legitimately—but combined and evaluated with historical environment activity and HVA context, Defender treats it as a high-confidence credential theft preparation signal and blocks it.

![Figure 1 showing Defender blocking domain controller credential theft using critical asset context](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/image-63.webp)

### Early detection of webshells and IIS compromise

When Defender identifies an HVA running the **IIS** role, it applies targeted inspection in common attacker foothold locations:

- Web-accessible directories
- Application paths that are frequently abused

In investigations involving **SharePoint** and **Exchange** servers, this approach surfaced highly targeted webshells (including cases where malicious logic was inserted into legitimate application files).

The article also references **AMSI** protections for Exchange/SharePoint that help block malicious code and exploitation attempts, but notes that if an attacker already has elevated internal access, they can still target internet-facing HVAs directly.

Scenario described:

- From another compromised system, an attacker remotely drops a customized, previously unseen webshell into the **EWS directory** of an Exchange Server.
- The webshell supports upload/download and in-memory code execution.

Because the server is identified as an internet-facing Exchange workload (higher risk role), Defender immediately remediates the file on creation.

![Figure 2 showing Defender remediating a webshell dropped onto an Exchange server based on role and critical asset context](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/image-64.webp)

### Expanded protection from remote credential dumping

The article highlights that HVAs hold the most sensitive credentials, making them a target for remote credential access attempts via:

- Administrative protocols
- Directory replication methods
- Identity synchronization systems such as **Microsoft Entra Connect**

Indicators include staging or moving artifacts like:

- Active Directory database files
- Registry hives
- Identity sync data

Role context helps Defender apply stronger protections (especially for domain controllers and identity infrastructure) and prevent credential exfiltration by correlating process chains and access patterns.

## Protecting your HVAs (customer actions)

1. **Ensure coverage across all critical assets**
   - Verify that all true HVAs are identified, including non-obvious ones (for example, servers running privileged services or holding sensitive credentials).

2. **Prioritize posture improvements and alert response for HVAs**
   - Apply security posture recommendations to HVAs first.
   - Prioritize monitoring and rapid response for alerts originating from HVAs to reduce blast radius.

3. **Triage vulnerabilities with HVA context**
   - Remediate vulnerabilities on HVAs before lower-impact assets.
   - A moderate issue on an HVA can represent greater risk than a severe issue on a non-critical endpoint.

## Learn more

- Automatic attack disruption context: https://www.microsoft.com/en-us/security/blog/2025/04/09/how-cyberattackers-exploit-domain-controllers-using-ransomware/
- Microsoft Defender products and services (Microsoft Learn): https://learn.microsoft.com/en-us/defender/
- Video on attack disruption for domain controllers: https://www.youtube.com/watch?v=BUGoxeoSffs
- Microsoft Security Exposure Management overview: https://www.microsoft.com/en-us/security/business/siem-and-xdr/microsoft-security-exposure-management
- AMSI for SharePoint Server: https://learn.microsoft.com/en-us/sharepoint/security-for-sharepoint-server/configure-amsi-integration
- AMSI integration with Exchange Server: https://learn.microsoft.com/en-us/exchange/antispam-and-antimalware/amsi-integration-with-exchange?view=exchserver-2019#overview
- Microsoft Threat Intelligence (LinkedIn): https://www.linkedin.com/showcase/microsoft-threat-intelligence
- Microsoft Security Intelligence on X: https://x.com/MsftSecIntel
- threatintel.microsoft.com on Bluesky: https://bsky.app/profile/threatintel.microsoft.com
- Microsoft Threat Intelligence podcast: https://thecyberwire.com/podcasts/microsoft-threat-intelligence


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/27/microsoft-defender-protects-high-value-assets/)

