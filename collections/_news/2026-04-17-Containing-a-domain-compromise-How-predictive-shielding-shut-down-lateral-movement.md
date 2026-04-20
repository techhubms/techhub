---
section_names:
- security
author: Microsoft Defender Security Research Team
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/17/domain-compromise-predictive-shielding-shut-down-lateral-movement/
date: 2026-04-17 14:51:01 +00:00
feed_name: Microsoft Security Blog
primary_section: security
title: 'Containing a domain compromise: How predictive shielding shut down lateral movement'
tags:
- Access Control Lists (acl)
- Active Directory
- Automatic Attack Disruption
- Credential Theft
- Domain Compromise
- Exchange Server
- Group Policy Objects (gpo)
- IIS
- Impacket
- Kerberos
- Krbtgt Rotation
- Lateral Movement
- LSASS Dumping
- Microsoft Defender For Endpoint P2
- Microsoft Defender XDR
- Microsoft Entra Connect
- Mimikatz
- MITRE ATT&CK
- News
- NTDS
- Password Spraying
- Predictive Shielding
- Security
---

The Microsoft Defender Security Research Team walks through a real-world Active Directory domain compromise and shows how Microsoft Defender XDR’s predictive shielding (automatic attack disruption) used exposure-based containment to slow credential abuse and limit lateral movement until the attacker lost momentum.<!--excerpt_end-->

# Containing a domain compromise: How predictive shielding shut down lateral movement

In identity-based attacks, an intrusion can become critical once a threat actor obtains **domain admin** rights. With domain-level control, attackers can change group memberships and **ACLs**, mint **Kerberos tickets**, replicate directory secrets, and push policy via **Group Policy Objects (GPOs)**.

A key problem is speed: in many incidents, **domain credentials are compromised immediately after initial access** and then abused before defenders can scope and remediate. Full recovery can also be slow and disruptive (for example, **[krbtgt rotation](https://learn.microsoft.com/en-us/windows-server/identity/ad-ds/manage/forest-recovery-guide/ad-forest-recovery-reset-the-krbtgt-password)**, **[GPO cleanup](https://learn.microsoft.com/en-us/windows-server/identity/ad-ds/manage/group-policy/group-policy-backup-restore)**, and **[ACL validation](https://learn.microsoft.com/en-us/windows/win32/ad/how-access-control-works-in-active-directory-domain-services)**).

This post explains how **Microsoft Defender’s predictive shielding**, as part of **automatic attack disruption** in **Microsoft Defender XDR**, aims to close that “speed gap” by proactively restricting accounts that were likely exposed.

## Predictive shielding overview

**[Predictive shielding](https://learn.microsoft.com/en-us/defender-xdr/shield-predict-threats)** is a capability in **[automatic attack disruption](https://learn.microsoft.com/en-us/defender-xdr/automatic-attack-disruption)**.

Instead of waiting to observe malicious account behavior, it acts when Defender has **high-confidence signals that credentials were exposed** on a device:

- Defender detects post-breach activity strongly associated with credential exposure on a device.
- It evaluates which high-privilege identities were likely exposed.
- It applies **containment** to reduce an attacker’s ability to pivot and perform high-impact identity operations while investigation and remediation proceed.

It’s described as an out-of-the-box enhancement for **Microsoft Defender for Endpoint P2** customers who meet prerequisites.

## Attack chain overview

In **June 2025**, a public sector organization was targeted. The actor progressed through:

- initial exploitation
- local escalation
- directory reconnaissance
- credential access
- expansion into **Microsoft Exchange** and identity infrastructure

![Attack diagram of the domain compromise](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-30.webp)

### Initial entry: Pre-domain compromise

The campaign began with a file-upload flaw in an internet-facing **IIS** server, used to plant a web shell. The attacker then escalated to **NT AUTHORITY\\SYSTEM** via a Potato-class token impersonation primitive (for example, **BadPotato**).

The attacker attempted to reset passwords of high-impact identities and also deployed **Mimikatz** to dump logon secrets (for example, **MSV**, **LSASS**, and **SAM**) to harvest credentials exposed on the device.

![Example discovery commands screenshot](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-31.webp)

![Mimikatz-related screenshots](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-32.webp)

![Mimikatz-related screenshots](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-33.webp)

**Key takeaway:** At this stage, keep containment host-scoped. Prioritize blocking credential theft and stopping escalation before it reaches identity infrastructure.

### First pivot: Directory credential materialization and Exchange delegation

Within 24 hours, the attacker abused privileged accounts and remotely created a scheduled task on a domain controller. The task initiated **NTDS snapshot** activity and packaged the output using **makecab.exe**, enabling offline access to directory credential material.

![NTDS snapshot activity screenshot](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-34.webp)

The actor then planted a **Godzilla** web shell on **Exchange Server**, enumerated accounts with **ApplicationImpersonation** role assignments, and granted mailbox access via **Add‑MailboxPermission**.

The attack used **Impacket atexec.py** for remote enumeration. This triggered Defender’s attack disruption, which revoked sessions for an admin account and blocked further use.

![Attack disruption screenshot](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-35.webp)

The actor then attempted additional actions such as resetting passwords and dumping credentials from a **Veeam** backup device.

**Key takeaway:** This is a turning point. Once directory credentials and privileged delegation are involved, scope and impact expand quickly. Prioritize protecting domain controllers, privileged identities, and authentication paths.

### Scale and speed: Tool return, spraying, and lateral movement

Weeks later, the actor returned with **Impacket** tooling (for example, **secretsdump** and **PsExec**). Defender repeatedly disrupted the abused accounts, forcing pivots to other compromised identities.

The actor then launched a broad **password spray** from the compromised IIS server, unlocking access to at least **14 servers** through password reuse, and attempted remote credential dumping against domain controllers and an additional IIS server.

**Key takeaway:** Even with fast disruption, prior large-scale credential dumping meant the attacker already had multiple usable credentials—highlighting why exposure-based containment is useful.

### Predictive shielding breaks the chain: Exposure-centric containment

In the second phase, predictive shielding was activated. When exposure signals appeared (for example, credential dumping attempts and replay from compromised hosts), automated containment blocked new sign-ins and interactive pivots for abused accounts and context-linked identities active on the same compromised surfaces.

When a high-tier **Enterprise Admin** or **Schema Admin** credential was exposed, predictive shielding contained it pre-abuse to prevent catastrophic escalation.

### Second pivot: Alternative paths to new credentials

With high-value identities pre-contained, the actor pivoted to exploiting **Apache Tomcat** servers, dropped the Godzilla web shell, and ran PowerShell-based **Invoke-Mimikatz** to harvest credentials. At one point, the attacker operated under **Schema Admin**.

![Schema Admin activity screenshot](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-36.webp)

They then used **Impacket WmiExec** to access **Microsoft Entra Connect** servers and attempt extraction of Entra Connect synchronization credentials. The account used for this pivot was later contained.

### Last attempts and shutdown

Finally, the actor attempted a full **LSASS** dump on a file-sharing server using **comsvcs.dll MiniDump**, followed by additional **NTDS** activity.

![LSASS dump and NTDS activity screenshot](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-37.webp)

Defender repeatedly severed sessions and blocked sign-ins. The campaign reportedly stopped on **July 28, 2025**.

## How predictive shielding changed the outcome

The post argues:

- Before domain compromise, attackers are constrained by host footholds.
- A small set of exposed credentials can remove those constraints quickly.
- Once predictive shielding went live mid-campaign, Defender could act **preemptively** on likely exposure, blocking pivots for abused accounts and context-linked identities.
- The attacker shifted to alternate credential sources, but continued containment reduced lateral reach until they lost momentum.

## MITRE ATT&CK techniques observed

The post maps behaviors to ATT&CK techniques, including:

- **T1190** Exploit Public-Facing Application (IIS file upload vuln)
- **T1505.003** Web Shell
- **T1059.001** PowerShell (Exchange role queries, mailbox permission changes, Invoke-Mimikatz)
- **T1068** Privilege escalation (BadPotato)
- **T1003.001** LSASS memory dumping (Mimikatz, comsvcs.dll MiniDump)
- **T1003.003** NTDS credential dumping workflows (ntdsutil snapshot/IFM)
- **T1053.005** Scheduled task
- **T1087.002** Domain account discovery
- **T1021.002 / T1021.003** Remote services (SMB/Admin Shares, WinRM/WmiExec)
- **T1110.003** Password spraying
- **T1114.002** Remote email collection
- **T1071.001** Web protocols for web shells
- **T1070.004** File deletion/cleanup scripts
- **T1098** Account manipulation
- **T1078** Valid accounts

## Learn more

- Ninja show video: https://youtu.be/u9nXRABIw1k?si=A6X9q1rHFqKPVtQb
- [Automatic attack disruption in Microsoft Defender XDR](https://learn.microsoft.com/en-us/defender-xdr/automatic-attack-disruption)
- [Predictive shielding in Microsoft Defender (Preview)](https://learn.microsoft.com/en-us/defender-xdr/shield-predict-threats)


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/17/domain-compromise-predictive-shielding-shut-down-lateral-movement/)

