---
external_url: https://www.microsoft.com/en-us/security/blog/2026/02/05/clickfix-variant-crashfix-deploying-python-rat-trojan/
title: CrashFix ClickFix Variant Deploys Python RAT via Browser Extension and Living-off-the-Land Tactics
author: Microsoft Defender Security Research Team
primary_section: security
feed_name: Microsoft Security Blog
date: 2026-02-05 18:51:39 +00:00
tags:
- Browser Extension Attack
- C2 Beaconing
- ClickFix
- CrashFix
- Detection Queries
- Domain Joined Systems
- Finger.exe
- Indicators Of Compromise
- Living Off The Land Binaries
- Malware Analysis
- Microsoft Defender
- Microsoft Sentinel
- ModeloRAT
- News
- Persistence Mechanisms
- PowerShell Obfuscation
- Python RAT
- Registry Persistence
- Remote Access Trojan
- Scheduled Task
- Security
- WinPython
section_names:
- security
---
The Microsoft Defender Security Research Team dissects the CrashFix variant of ClickFix, revealing how it combines malicious browser extensions, PowerShell obfuscation, and a portable Python-based RAT to compromise and persist on high-value Windows systems.<!--excerpt_end-->

# CrashFix ClickFix Variant: Technical Dissection and Defense Guidance

## Overview

In January 2026, Microsoft Defender Experts uncovered a new variant in the ClickFix campaign, dubbed **CrashFix**. This attack strategy targets browser users with social engineering and technical evasion—culminating in the deployment of a Python Remote Access Trojan (RAT) via a malicious Chrome extension, living-off-the-land OS binaries, and portable Python. The campaign especially targets domain-joined, high-value Windows systems.

## Attack Lifecycle

1. **Lure Phase:** User searches for popular ad blockers, gets diverted to a fraudulent Chrome Web Store listing for a fake extension imitating uBlock Origin Lite.
2. **Initial Compromise & Social Engineering:** Installers create delayed browser disruption, masking the extension's involvement. A fabricated "CrashFix" popup drives users to run an attacker-placed malicious command.
3. **Living-off-the-Land Execution:** The native Windows utility `finger.exe` is covertly copied, renamed to `ct.exe`, and used to fetch an obfuscated PowerShell payload from a remote attack server.
4. **Payload Download Chain:** The PowerShell script, saved to AppData\Roaming, utilizes obfuscation and anti-analysis (detects debuggers, security tools, and domain-join status) to tailor next-stage deployment. It sends system details to an attacker C2 endpoint.
5. **Selective RAT Deployment:** On domain-joined endpoints, CrashFix downloads a **WinPython** portable environment and the core Python RAT logic (`modes.py`, aka **ModeloRAT**), ensuring cross-system compatibility without requiring full Python installation.
6. **Persistence:** The RAT ensures startup via a `Run` registry key (using `pythonw.exe`) and, in some cases, via a scheduled task masquerading as legitimate software protection.
7. **Recon & Control:** ModeloRAT executes Windows enumeration commands, establishes C2 beaconing via HTTP, and can download/run further Python payloads via Dropbox or ZIP archives.

## Key Technical Details

- **Malware Tactics:** Delayed execution, credential/data theft, post-exploitation reconnaissance.
- **Persistence Mechanisms:** Registry Run key, deceptive scheduled tasks.
- **Living-off-the-Land:** Fileless techniques leveraging renamed system binaries (`finger.exe` as `ct.exe`), obfuscated PowerShell, Windows scheduled tasks and registry startup locations.
- **Payloads:** PowerShell scripts, Python RAT (ModeloRAT), additional payloads (e.g., `udp.pyw`, `extentions.py`) fetched from remote servers, executed in memory or via portable Python.
- **Communication:** Beaconing to attacker C2 servers, use of typosquatted domains, data exfiltration and remote command execution.

## Detection & Hunting

### Defender XDR Queries

- **Malicious Extension Download:**

  ```
  DeviceFileEvents | where FileName has "cpcdkmjddocikjdkbbeiaafnpdbdafmi"
  ```

- **Suspicious Process Execution:**

  ```
  DeviceProcessEvents | where InitiatingProcessCommandLine has_all ("cmd.exe","start","finger.exe","ct.exe")
  ```

- **Python RAT Beaconing:**

  ```
  DeviceNetworkEvents | where InitiatingProcessCommandLine has_all ("pythonw.exe","modes.py"), RemoteIP !in ("", "127.0.0.1")
  ```

- **Registry Persistence:**

  ```
  DeviceRegistryEvents | where InitiatingProcessCommandLine has_all ("pythonw.exe","modes.py")
  ```

- **Scheduled Task Creation:**

  ```
  DeviceEvents | where ActionType == "ScheduledTaskCreated" | where InitiatingProcessCommandLine has_all ("run.exe", "udp.pyw")
  ```

### Indicators of Compromise (IOCs)

- **Malicious Domains:** nexsnield[.]com
- **Malicious IPs:** 69.67.173.30, 144.31.221.197, 199.217.98.108, 158.247.252.178, 170.168.103.208
- **Payload Hashes:** c76c0146407069fd4c271d6e1e03448c481f0970ddbe7042b31f552e37b55817 (PowerShell), see post for additional extension/package hashes

## Mitigation Recommendations

- Enable [cloud-delivered protection](https://learn.microsoft.com/defender-endpoint/configure-block-at-first-sight-microsoft-defender-antivirus) and [EDR in block mode](https://learn.microsoft.com/defender-endpoint/edr-in-block-mode) in Microsoft Defender for Endpoint
- Enforce network egress filtering, particularly blocking rarely-used legacy protocols such as those used by `finger.exe`
- Harden registry and scheduled task settings against untrusted modifications
- Turn on web and network protection, and require MFA everywhere
- Regularly remind users not to store enterprise creds in browsers, and disable browser password sync where possible on managed devices
- Apply Microsoft Defender [attack surface reduction rules](https://learn.microsoft.com/defender-endpoint/attack-surface-reduction)
- Use SmartScreen-enabled browsers for phishing and malware defense

## Microsoft Security Product Integration

- **Defender for Endpoint & XDR:** Coverage for detection, blocking, and hunting of the described behaviors and payloads.
- **Microsoft Sentinel:** Use provided TI Mapping analytics or deploy Threat Intelligence solution for automated IOC matching.
- **Security Copilot Integration:** Utilize Security Copilot capabilities in Defender XDR for incident investigation and threat hunting.

---

### References and Additional Reading

- [Microsoft Security Blog Original Post](https://www.microsoft.com/en-us/security/blog/2026/02/05/clickfix-variant-crashfix-deploying-python-rat-trojan/)
- [Huntress Blog: Malicious CrashFix Browser Extension](https://www.huntress.com/blog/malicious-browser-extention-crashfix-kongtuke)
- [Microsoft Learn: Configure Block at First Sight](https://learn.microsoft.com/defender-endpoint/configure-block-at-first-sight-microsoft-defender-antivirus)

*Research by Microsoft Defender Security Research Team with Sai Chakri Kandalai and Kaustubh Mangalwedhekar.*

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/02/05/clickfix-variant-crashfix-deploying-python-rat-trojan/)
