---
external_url: https://techcommunity.microsoft.com/t5/microsoft-security-community/bitunlocker-leveraging-windows-recovery-to-extract-bitlocker/ba-p/4442806
title: 'BitUnlocker: Leveraging Windows Recovery to Extract BitLocker Secrets'
author: alon-leviev
feed_name: Microsoft Tech Community
date: 2025-08-13 17:04:05 +00:00
tags:
- BCD Store
- BitLocker
- Boot.sdi
- CVE 48003
- CVE 48800
- CVE 48804
- CVE 48818
- Exploit
- Microsoft STORM
- Offline Scanning
- Patch Tuesday
- ReAgent.xml
- Recovery OS
- REVISE Mitigation
- Security Testing
- SetupPlatform.exe
- TPM+PIN
- Trusted WIM Boot
- Vulnerability Research
- Windows Recovery Environment
- WinRE
section_names:
- security
---
Alon Leviev, along with Netanel Ben Simon from Microsoft's STORM research team, details their investigation into newly discovered attack surfaces in Windows Recovery Environment impacting BitLocker. The post guides readers through research findings, exploitation techniques, and recommended mitigations.<!--excerpt_end-->

# BitUnlocker: Leveraging Windows Recovery to Extract BitLocker Secrets

## Table of Contents

1. Introduction
2. BitLocker Overview
3. WinRE Overview
4. Attacking Boot.sdi Parsing
5. Attacking ReAgent.xml Parsing
6. Attacking Boot Configuration Data (BCD) Parsing
7. Vulnerability Fixes
8. BitLocker Countermeasures
9. About the STORM Researchers

---

## Introduction

BitLocker provides full disk encryption on Windows, designed to secure sensitive data even if the device falls into adversarial hands. Recent design changes in the Windows Recovery Environment (WinRE) to support BitLocker recovery inadvertently introduced new attack surfaces. This post, based on research presented at Black Hat USA 2025 and DEF CON 33, explores how attackers can leverage WinRE to bypass BitLocker, details discovered vulnerabilities, exploitation methodologies, and the resulting fixes.

## BitLocker Overview

BitLocker encrypts disk volumes, focusing on defense against attackers with physical access but lacking credentials. It is especially notable for explicitly including physical attacker scenarios in its threat model. The core challenge is to ensure recovery mechanisms (such as WinRE) do not inadvertently allow sensitive data access by adversaries.

## WinRE Overview

WinRE is a stripped-down Windows OS (the Recovery OS), compressed into WinRE.wim, which mounts into RAM for system recovery operations. Recent architectural shifts in BitLocker support caused WinRE.wim to move from the OS volume to an unencrypted recovery volume, enabled Trusted WIM Boot for integrity checking, and introduced a 'volumes re-lock' mechanism for sensitive recovery tools.

## Attacking Boot.sdi Parsing

**Vulnerability (CVE-2025-48804):**

- WinRE's RAM disk is assembled using a Boot.sdi file (with blob tables) and the WinRE.wim image. WinRE trusts the on-disk hash of WinRE.wim, but blindly boots from a WIM location specified in the SDI blobs.
- **Exploit:** By appending a malicious WIM to the SDI file and setting the offset, attackers can boot with an untrusted WIM in auto-unlocked mode, sidestepping integrity checks and extracting data.
- **Impact:** Practical bypass of BitLocker, enabling access to encrypted data.

## Attacking ReAgent.xml Parsing

### Scheduled Operations Primitive (CVE-2025-48800, CVE-2025-48003)

- ReAgent.xml's ScheduledOperation field can auto-trigger certain recovery actions. Limitations on allowed offline scanning apps and WinRE apps are enforced via signatures and hardcoded lists, but two significant bypasses were found:
  - **tttracer.exe as Proxy**: This legitimate Microsoft-signed debug tool, when invoked as a scheduled scanner, can trace arbitrary applications (e.g., cmd.exe) without triggering auto-lock. Exploiting this, attackers can access command prompt in an unlocked state and extract secrets.
  - **SetupPlatform.exe Hotkey Abuse:** Registered as trusted during upgrades, this app contains logic to launch cmd.exe via Shift+F10. With correct initialization sequence (INI file on recovery volume), attackers can create an 'infinite window' to exploit this and run cmd.exe post-upgrade in auto-unlocked mode.

## Attacking Boot Configuration Data (BCD) Parsing

### BCD Store Volume Scan Flaw (CVE-2025-48818)

- WinRE iterates over all volumes looking for a BCD store, starting with the recovery volume before the EFI volume. By creating a crafted BCD store on the recovery volume, attackers can impersonate the OS device target, causing WinRE to load recovery configuration from attacker-controlled storage.
- Combining this primitive with Push Button Reset's DecryptVolume directive, scheduled via ReAgent.xml, the attacker can trick WinRE into decrypting a BitLocker-protected volume, fully exposing the secrets.

## Vulnerability Fixes

All four critical vulnerabilities were patched as part of July 2025 Patch Tuesday:

- CVE-2025-48800
- CVE-2025-48003
- CVE-2025-48804
- CVE-2025-48818

## BitLocker Countermeasures

- **Enable TPM+PIN:** Restricts BitLocker unlocking to authorized PIN input, reducing attack surface.
- **REVISE Mitigation:** (see [Microsoft Learn](https://support.microsoft.com/en-us/topic/how-to-manage-the-windows-boot-manager-revocations-for-secure-boot-changes-associated-with-cve-2023-24932-41a975df-beb2-40c1-99a3-b3ff139f832d)) Enforces secure boot versioning, protecting against version downgrade attacks.
- **Stay Updated:** Regularly apply Windows security updates, particularly those relevant to WinRE and BitLocker.

## About the STORM Researchers

This research was conducted by Netanel Ben Simon and Alon Leviev with Microsoft Security Testing & Offensive Research (STORM). The team’s mission is to elevate engineering security standards and ensure Microsoft cloud platform resilience.

---

**References:**

- [BitLocker Countermeasures | Microsoft Learn](https://learn.microsoft.com/en-us/windows/security/operating-system-security/data-protection/bitlocker/countermeasures)
- CVE Breakdown:
  - CVE-2025-48800 (tttracer.exe proxy execution)
  - CVE-2025-48003 (SetupPlatform.exe Shift+F10 bypass)
  - CVE-2025-48804 (Boot.sdi/SDI boot bypass)
  - CVE-2025-48818 (BCD store volume enumeration exploit)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-security-community/bitunlocker-leveraging-windows-recovery-to-extract-bitlocker/ba-p/4442806)
