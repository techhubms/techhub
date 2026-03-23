---
tags:
- Active Directory
- AD Explorer
- Attack Disruption
- Defense Evasion
- Entra Connect
- GPO Hardening
- Group Policy Objects
- Human Operated Ransomware
- Kerberoasting
- Microsoft Defender
- Microsoft Defender For Endpoint
- Microsoft Defender XDR
- Microsoft Entra ID
- MITRE ATT&CK
- NETLOGON
- News
- NTDS.dit
- Predictive Shielding
- Ransomware
- Scheduled Tasks
- Security
- SMB
- SYSVOL
- T1021.002
- T1053.005
- T1484.001
- T1486
- T1562.001
- Tamper Protection
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/23/case-study-predictive-shielding-defender-stopped-gpo-based-ransomware-before-started/
author: Microsoft Defender Security Research Team
date: 2026-03-23 16:00:00 +00:00
title: 'Case study: How predictive shielding in Defender stopped GPO-based ransomware before it started'
feed_name: Microsoft Security Blog
primary_section: security
section_names:
- security
---

The Microsoft Defender Security Research Team walks through a real human-operated ransomware case where attackers abused Group Policy Objects (GPOs) to disable defenses and distribute payloads, and shows how Defender predictive shielding (GPO hardening + attack disruption) proactively blocked the GPO-based encryption path across ~700 devices.<!--excerpt_end-->

## Summary

- Microsoft Defender disrupted a human-operated ransomware incident at a large educational institution with **a couple of thousand devices**.
- The attacker attempted to abuse **Group Policy Objects (GPOs)** to:
  - Tamper with Defender protections
  - Distribute ransomware broadly via **scheduled tasks**
- Defender **predictive shielding** detected precursors and **proactively hardened** against malicious GPO propagation across **~700 devices**.
- Defender blocked **~97%** of attempted encryption overall, and **zero machines were encrypted via the GPO path**.

## The growing threat: GPO abuse in ransomware operations

Ransomware operators increasingly abuse trusted enterprise administration mechanisms to disable defenses and distribute ransomware at scale.

**Why GPOs are attractive to attackers:**

- GPOs are a built-in, trusted way to push changes across domain-joined devices.
- Attackers can abuse GPOs to:
  - Push configurations that disable security tools
  - Deploy scheduled tasks to distribute and execute ransomware
  - Achieve large-scale impact without manual per-device access

This case study covers a real incident where an attacker weaponized GPOs, and how Defender predictive shielding stopped the attack before ransomware was deployed via GPO.

![Two diagrams illustrating GPO abuse and Defender response](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/image-59.webp)

![Additional diagram illustrating the case study flow](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/image-58.webp)

## The incident

The target environment:

- Large educational institution
- **33 servers**
- **11 domain controllers**
- **2 Entra Connect servers**
- Devices onboarded to Microsoft Defender with the full Defender suite deployed

### Attack chain overview

#### Initial access and privilege escalation

- Attacker began from an **unmanaged device**.
- A **Domain Admin** account was already compromised.
- Initial access vector and the exact method of obtaining Domain Admin privileges were **unknown** due to limited visibility.

#### Day 1: Reconnaissance

- Attacker used **AD Explorer** to enumerate Active Directory.
- Brute force techniques were used to map the environment.
- Defender generated alerts for these activities.

#### Day 2: Credential access and lateral movement

- Attacker obtained credentials for multiple high-privilege accounts.
- Observed activity included:
  - **Kerberoasting**
  - **NTDS dump** activity
- Attacker created multiple **local accounts** on compromised systems for persistence.
- Attacker moved laterally using acquired credentials.

**Defender response:**

- Defender initiated **attack disruption** against **five compromised accounts**.
- Outcome:
  - Lateral movement attempts blocked at scale
  - Thousands of blocked authentication/access attempts
  - Noticeable slowdown of the attack

#### Day 5: Defense evasion and impact

- Attacker still had access to additional privileged accounts.
- Attacker moved to impact phase using **Group Policy** as the primary distribution mechanism:
  - First, used GPO to push a tampering policy to disable Defender protections.
  - Then, distributed ransomware via GPO.
  - In parallel, executed additional remote ransomware deployment over **SMB** using multiple compromised accounts.

**Defender response:**

- Defender initiated a second round of **attack disruption**, alongside **predictive shielding**.
- More than a dozen compromised entities were disrupted.
- Combined with GPO hardening, Defender prevented further progress.

![Deep dive diagram: how GPO was weaponized and stopped](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/image-61.webp)

## Deep dive: How the attacker weaponized GPO and how predictive shielding stopped the attack

### Step 1: Tampering with security controls

- Attacker created a malicious GPO designed to tamper with endpoint security controls.
- The policy disabled key Defender protections, including:
  - Behavioral monitoring
  - Real-time protection

**Defender response:**

- Defender raised a tampering alert.
- Predictive shielding activated **GPO hardening**:
  - Temporarily paused propagation of new GPO policies
  - Across all MDE onboarded devices reachable from the attacker’s standpoint
  - Resulted in protection of **~85% of devices** against the tampering policy

### Step 2: Ransomware distribution via scheduled tasks

~10 minutes after the tampering GPO, the attacker proceeded to ransomware distribution.

**Attacker actions:**

- Placed three malicious files in the **SYSVOL** share:
  - `run.bat`
  - `run.exe`
  - `run.dll`
- Created a second malicious GPO that configured a **scheduled task** on targeted devices.
- Scheduled task execution chain:

```text
cmd /c start run.bat → cmd /c c:\users\…\run.exe → rundll32 c:\users\…\run.dll Encryptor
```

**Why this works operationally:**

- Each device pulls the payload via the scheduled task.
- The attacker sets the policy once; devices perform the distribution and execution (“self-service distribution model”).

**Why it failed in this case:**

- GPO hardening had already been applied during the tampering stage.
- By the time the ransomware GPO was created, the environment was already hardened.
- Predictive shielding treated GPO tampering as a precursor to ransomware distribution and acted before the ransomware appeared.

## Results

- **Zero** machines encrypted via the **GPO path**.
- Defender protected roughly **97%** of devices targeted for encryption.
- Some devices experienced encryption due to concurrent ransomware delivery over **SMB**, but:
  - Attack disruption contained the incident
  - Further impact and propagation were stopped
- **700 devices** applied the predictive shielding GPO hardening policy.
- Malicious policy propagation was blocked within approximately **3 hours**.

## The hardening dilemma: Why attackers target operational mechanisms

Operational tools such as **GPO**, **scheduled tasks**, and remote management are:

- Highly privileged
- Widely trusted
- Essential for normal IT operations

This creates an asymmetry:

- Defenders can’t permanently lock these mechanisms down without breaking operations.
- Detection-only approaches can be too late (ransomware may already be distributed).
- Manual SOC response can’t match attacker speed.

Predictive shielding is positioned as a way to close this gap.

## Predictive shielding: Contextual, just-in-time hardening

Predictive shielding is described as having two pillars:

1. **Prediction**
   - Correlates activity signals, threat intelligence, and exposure topology
   - Infers likely next attacker actions and reachable assets
2. **Enforcement**
   - Applies targeted, temporary controls
   - Disrupts predicted attack paths in real time

Key characteristics highlighted:

- Adaptive and risk-conditioned enforcement
- Controls are scoped to blast radius
- Temporary and contextual
- Focuses on predicted intent rather than waiting for impact

## Closing the gap

In this incident, predictive shielding detected the attacker at the GPO tampering stage and prevented ransomware propagation through malicious GPOs:

- **700 devices** avoided GPO-based encryption
- **97% protection rate** overall
- Remaining encryptions came via rapid SMB-based remote deployment, which was then contained via attack disruption

## MITRE ATT&CK techniques observed

| Tactic(s) | Technique ID | Technique name | Observed details |
| --- | --- | --- | --- |
| Discovery | T1087.002 | Account Discovery: Domain Account | AD Explorer used to enumerate AD objects and accounts. |
| Credential Access | T1110 | Brute Force | Brute force techniques used during early recon. |
| Credential Access | T1558.003 | Steal or Forge Kerberos Tickets: Kerberoasting | Kerberoasting observed before acquiring high-privilege credentials. |
| Credential Access | T1003.003 | OS Credential Dumping: NTDS | NTDS dump activity observed during credential harvesting. |
| Persistence | T1136.001 | Create Account: Local Account | Multiple local accounts created for persistence. |
| Lateral Movement | T1021.002 | Remote Services: SMB/Windows Admin Shares | Lateral movement via SMB/admin shares using stolen creds. |
| Persistence | T1484.001 | Domain Policy Modification: Group Policy Modification | Malicious GPOs created to modify security config and deploy ransomware. |
| Defense Evasion | T1562.001 | Impair Defenses: Disable or Modify Tools | GPO used to disable Defender protections. |
| Execution | T1053.005 | Scheduled Task/Job: Scheduled Task | GPO created scheduled tasks to copy/execute payload from SYSVOL. |
| Execution | T1059.003 | Command and Scripting Interpreter: Windows Command Shell | `cmd /c` used in execution chain. |
| Execution | T1218.011 | System Binary Proxy Execution: Rundll32 | `rundll32.exe` used to execute DLL payload. |
| Impact | T1486 | Data Encrypted for Impact | Ransomware attempted via GPO and via remote operations. |

## References

- Blog post: Microsoft Defender now prevents threats on endpoints during an attack: https://aka.ms/MDEIgniteNews2025
- Predictive shielding in Microsoft Defender: https://learn.microsoft.com/en-us/defender-xdr/shield-predict-threats
- GPO hardening in Microsoft Defender for Endpoint: https://learn.microsoft.com/en-us/defender-endpoint/respond-machine-alerts#gpo-hardening-preview

## Attribution

This research is provided by Microsoft Defender Security Research with contributions from Tal Tzhori and Aviv Sharon.

## Learn more

- Protect your agents in real-time during runtime (Preview) – Microsoft Defender for Cloud Apps: https://learn.microsoft.com/en-us/defender-cloud-apps/real-time-agent-protection-during-runtime
- How to build and customize agents with Copilot Studio Agent Builder: https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/copilot-studio-agent-builder
- Microsoft 365 Copilot AI security documentation: https://learn.microsoft.com/en-us/copilot/microsoft-365/microsoft-365-copilot-ai-security
- How Microsoft discovers and mitigates evolving attacks against AI guardrails: https://www.microsoft.com/en-us/security/blog/2024/04/11/how-microsoft-discovers-and-mitigates-evolving-attacks-against-ai-guardrails/
- Securing Copilot Studio agents with Microsoft Defender: https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection

[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/23/case-study-predictive-shielding-defender-stopped-gpo-based-ransomware-before-started/)

