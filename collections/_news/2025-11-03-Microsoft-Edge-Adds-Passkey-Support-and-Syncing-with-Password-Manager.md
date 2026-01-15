---
layout: post
title: Microsoft Edge Adds Passkey Support and Syncing with Password Manager
author: stclarke
canonical_url: https://blogs.windows.com/msedgedev/2025/11/03/microsoft-edge-introduces-passkey-saving-and-syncing-with-microsoft-password-manager/
viewing_mode: external
feed_name: Microsoft News
feed_url: https://news.microsoft.com/source/feed/
date: 2025-11-03 21:32:08 +00:00
permalink: /azure/news/Microsoft-Edge-Adds-Passkey-Support-and-Syncing-with-Password-Manager
tags:
- Azure
- Azure Confidential Ledger
- Biometric Security
- Cloud Security
- Company News
- Cryptography
- Device Authentication
- FIDO2
- Microsoft Account
- Microsoft Edge
- Microsoft Password Manager
- News
- Passkey
- Passwordless Authentication
- PIN Authentication
- Security
- Windows 10
- Windows 11
section_names:
- azure
- security
---
stclarke details how Microsoft Edge introduces passkey saving and syncing with Microsoft Password Manager, providing improved passwordless security backed by Azure confidential ledger.<!--excerpt_end-->

# Microsoft Edge Adds Passkey Support and Syncing with Password Manager

**Author:** stclarke  
**Published:** November 3, 2025

Microsoft Edge now makes it possible to securely save and sync passkeys using Microsoft Password Manager, providing a new passwordless way to sign in to supported websites and applications. This update works on Windows devices (10 and above) with Edge version 142 or newer and a Microsoft account.

## What Are Passkeys?

Passkeys are a modern authentication method based on the FIDO2 standard, leveraging public-key cryptography. When a passkey is created, your device generates a unique private key (stored securely and never leaves your device) and a public key (shared with the website). Authentication relies on biometrics (fingerprint, face recognition), or a device PIN, reducing exposure to credential theft.

- **Cannot be guessed or reused like passwords**
- **Resistant to phishing and credential attacks**
- **No need to memorize or type complex passwords**

## How it Works in Microsoft Edge

- You can save passkeys in Microsoft Password Manager when signing in to supported websites.
- Passkeys sync securely with your Microsoft account and protected by a dedicated Microsoft Password Manager PIN set up during initial passkey creation.
- To use passkeys on another device, you verify yourself with the Manager PIN.
- Passkeys are managed from Edge (Settings > Passwords and autofill > Microsoft Password Manager).

## Security & Privacy Details

- Passkeys are encrypted in the cloud.
- All PIN unlock and reset attempts are logged and integrity-protected in the Azure confidential ledger for transparency.
- Biometric data is always processed locally, not shared with websites or Microsoft.
- If you forget your Manager PIN, it can be reset from any device with active passkey access.

## Prerequisites

- Windows device running version 10 or above
- Microsoft Edge version 142+
- Microsoft account

## Additional Notes

- Passkey sync is not yet available on mobile or for Microsoft Entra accounts.
- Microsoft Password Manager plugin will extend passkeys to other apps and browsers on Windows in future updates.
- Saved passwords remain untouched; passkeys are an added option for supported sites.
- More technical details and documentation are available for [Azure confidential ledger](https://aka.ms/acl-docs).

## Frequently Asked Questions

- **Does this impact my existing passwords?** No, you can continue using them or upgrade to passkeys for supported sites.
- **What if my device is lost or stolen?** Biometric/PIN security prevents unauthorized access to passkeys.
- **Can passkeys be used outside Edge?** Support for other browsers and apps is coming soon via plugin.
- **Is business/school account support included?** Not yet; currently limited to personal Microsoft Accounts on Windows.

For more details, visit the [original Microsoft Edge Blog post](https://blogs.windows.com/msedgedev/2025/11/03/microsoft-edge-introduces-passkey-saving-and-syncing-with-microsoft-password-manager/).

This post appeared first on "Microsoft News". [Read the entire article here](https://blogs.windows.com/msedgedev/2025/11/03/microsoft-edge-introduces-passkey-saving-and-syncing-with-microsoft-password-manager/)
