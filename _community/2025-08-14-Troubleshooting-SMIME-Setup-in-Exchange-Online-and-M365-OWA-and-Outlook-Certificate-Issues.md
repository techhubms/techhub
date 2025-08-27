---
layout: "post"
title: "Troubleshooting S/MIME Setup in Exchange Online and M365: OWA and Outlook Certificate Issues"
description: "This detailed troubleshooting post by JChristiansen covers challenges encountered during the deployment and configuration of S/MIME on Microsoft 365 Exchange Online, specifically addressing cross-platform certificate publishing and related errors in Outlook (Windows and Mac) as well as OWA. It outlines steps taken for certificate enrollment, Intune deployment, Exchange Online configuration, and current working scenarios, along with cases where certificate trust errors block encrypted email functionality."
author: "JChristiansen"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/exchange/smime-not-working-in-owa/m-p/4443230#M16650"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-14 09:58:31 +00:00
permalink: "/2025-08-14-Troubleshooting-SMIME-Setup-in-Exchange-Online-and-M365-OWA-and-Outlook-Certificate-Issues.html"
categories: ["Security"]
tags: ["Certificate Enrollment", "Certificate Trust", "Community", "Connect ExchangeOnline", "Email Security", "Encryption", "Exchange Online", "GAL", "Intermediate Certificate", "Intune", "Macos", "Microsoft 365", "Outlook", "OWA", "PowerShell", "Root Certificate", "S/MIME", "Security", "SMIMECertificateIssuingCA", "Windows"]
tags_normalized: ["certificate enrollment", "certificate trust", "community", "connect exchangeonline", "email security", "encryption", "exchange online", "gal", "intermediate certificate", "intune", "macos", "microsoft 365", "outlook", "owa", "powershell", "root certificate", "sslashmime", "security", "smimecertificateissuingca", "windows"]
---

JChristiansen provides a hands-on report documenting their S/MIME deployment experience with Exchange Online, Intune, and mixed Windows/macOS endpoints, focusing on certificate publishing and troubleshooting Outlook/OWA trust issues.<!--excerpt_end-->

# Troubleshooting S/MIME Setup with Exchange Online and Microsoft 365

**Author: JChristiansen**

## Overview

This post documents the hands-on experience and troubleshooting steps involved in setting up S/MIME on Microsoft 365 with Exchange Online, supporting both Windows and macOS environments. The author details certificate deployment methods, Outlook-related publishing steps, and analyzes scenarios where S/MIME encryption is working as expected versus where certificate trust issues persist, especially in web (OWA) and new Outlook interfaces.

## Setup Steps Taken

- **Certificate Installation:**
    - Installed user .pfx certificates on Windows and macOS devices using the provided password.
- **Root/Intermediate Certificate Deployment:**
    - Deployed the root and intermediate CA certificates to endpoints using Microsoft Intune for both platforms.
- **Publishing to Exchange Online:**
    - Exported the root and intermediate CA certificates from Windows (using certmgr.msc).
    - Uploaded the CA certificates into Exchange Online via PowerShell:

    ```powershell
    Connect-ExchangeOnline
    Set-SmimeConfig -SMIMECertificateIssuingCA ([IO.File]::ReadAllBytes('C:\Temp\certificate\_CA.sst'))
    ```

- **GAL Publication:**
    - Published the S/MIME public certificate for each user into the Global Address List (GAL) via classic Outlook manually (Windows users only).

## Current Functionality

### Working Scenarios

- Encrypted emails can be sent from signed replies using old/classic Outlook.
- New encrypted email is successful in old Outlook after GAL publication (and when recipient signature saved to contact for external users).
- Sending encrypted mail from Outlook for Mac to Windows users who published their certificate via GAL also works.

### Not Working / Issues Observed

- **New Outlook (Windows):** Sending a new encrypted email results in the error: *Certificate is not trusted by this organization.*
- **OWA on Edge (Windows):** Same error as above when sending encrypted mail.
- **Cross-platform Issue:** Sending encrypted mail from old Outlook (Windows) to macOS users fails when Mac certificates have not been published to GAL.

## Observations / Next Steps

- Certificate trust errors in the New Outlook and OWA suggest an issue with CA publishing or propagation in Exchange Online or certificate chain trust visibility within these clients.
- Publishing via GAL appears critical for trust establishment, particularly for non-Windows clients and scenarios.
- Intune deployment to endpoints ensures local trust but does not solve visibility/trust requirements within Exchange Online and cloud endpoints.

## Key PowerShell Command Reference

```powershell
Connect-ExchangeOnline
Set-SmimeConfig -SMIMECertificateIssuingCA ([IO.File]::ReadAllBytes('C:\Temp\certificate\_CA.sst'))
```

## Guidance / Community Request

If you've encountered similar OWA or New Outlook certificate trust issues, especially post-root/intermediate CA publishing, share your resolutions or troubleshooting experiences, particularly regarding cloud trust propagation for S/MIME in hybrid or cross-platform environments.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/exchange/smime-not-working-in-owa/m-p/4443230#M16650)
