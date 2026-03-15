---
external_url: https://techcommunity.microsoft.com/t5/azure-virtual-desktop/macos-sso-no-longer-fully-functional-on-avd-win11-25h2/m-p/4494544#M14001
title: SSO Issues on Azure Virtual Desktop for macOS Clients After Windows 11 25H2 Update
author: FT_1
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-12 10:57:27 +00:00
tags:
- AADSTS9002341
- Authentication
- AVD
- Azure
- Azure Virtual Desktop
- Community
- Cross Platform
- IntegratedServicesRegionPolicySet.json
- Macos
- Microsoft Entra ID
- PRT
- Security
- Session Host
- Single Sign On
- SSO
- Teams
- Token Renewal
- Windows 11 25H2
section_names:
- azure
- security
---
FT_1 shares a detailed account of SSO authentication issues for macOS users on Azure Virtual Desktop after updating Windows 11 AVD session hosts to 25H2. Includes logs, symptoms, and a tested workaround.<!--excerpt_end-->

# SSO Issues on Azure Virtual Desktop for macOS Clients After Windows 11 25H2 Update

**Author: FT_1**

After upgrading Azure Virtual Desktop (AVD) session hosts from Windows 11 23H2 to 25H2 (26200.7462), a single sign-on (SSO) issue began affecting _only_ macOS clients using the Windows App. This post outlines the symptoms, technical diagnostics, and a specific workaround that mitigates the issue for affected users.

## Symptoms

- Microsoft Teams identifies users as "Unknown User"
- Chat and collaboration features do not load
- Error: "You need to sign in again. This may be a requirement from your IT department or Teams, or the result of a password update. - Sign in"
- After clicking "Sign in," the user sees only "Continue with sign-in"—no password or MFA prompt
- Other applications work after this step, with no further authentication required

## Technical Details

- **macOS Device:** AppleM4 Pro, macOS Tahoe 26.2
- **Windows App Version:** 11.3.2 (2848)
- `dsregcmd /status`: No errors; PRT (Primary Refresh Token) active and updated for sign-in
- **Entra ID Sign-In Logs:** Error code `9002341`
- **Session Host Event Logs (AAD-Operational):**
  - Event ID 1098 (Error 0xCAA2000C): Request requires user interaction (AADSTS9002341)
  - Event ID 1097 (Error 0xCAA90056): Token renewal via PRT failed

## Observations

- Issue affects both managed and unmanaged macOS devices
- Windows devices using the Windows App are _not_ affected
- If a session started on macOS (with the error) is later resumed on Windows, authentication succeeds

## Workaround

- Editing `C:\Windows\System32\IntegratedServicesRegionPolicySet.json` and **removing the "DE" flag** from "Automatic app sign-in" resolves the issue for macOS clients

## Open Questions

1. Is this a documented issue related to the Windows 11 25H2 update?
2. Has anyone else experienced this with macOS clients?
3. Why does it appear only on macOS, not Windows?
4. Why does the "DE" flag influence macOS behavior, but not Windows clients?

**Request:**
FT_1 seeks insights or confirmation from others encountering similar problems.

---

**If you have experience with this SSO issue or additional troubleshooting steps, please share your findings below.**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/macos-sso-no-longer-fully-functional-on-avd-win11-25h2/m-p/4494544#M14001)
