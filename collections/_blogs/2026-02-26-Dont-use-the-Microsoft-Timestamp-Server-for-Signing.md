---
layout: "post"
title: "Don't use the Microsoft Timestamp Server for Signing"
description: "This blog post by Rick Strahl discusses reliability issues with the default Microsoft timestamp server used for code signing, as recommended in Microsoft's Trusted Signing documentation. The author shares his personal experience with signing failures and offers a practical solution: switching to more reliable alternative timestamp servers, such as DigiCert, Sectigo, and others. The post includes code examples, troubleshooting tips, and a curated list of alternative servers."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2026/Feb/26/Dont-use-the-Microsoft-Timestamp-Server-for-Signing"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2026-02-26 22:41:03 +00:00
permalink: "/2026-02-26-Dont-use-the-Microsoft-Timestamp-Server-for-Signing.html"
categories: ["Security"]
tags: ["Azure.CodeSigning.Dlib.dll", "Blogs", "Certificate Authority", "Certum", "Code Signing", "DigiCert", "Entrust", "GlobalSign", "Microsoft Trusted Signing", "PowerShell", "Sectigo", "Security", "SHA256", "Signing Errors", "Signing Tools", "Signtool", "SSL.com", "Timestamp Server", "Trusted Signing Client Tools", "Windows", "Windows Security"]
tags_normalized: ["azuredotcodesigningdotdlibdotdll", "blogs", "certificate authority", "certum", "code signing", "digicert", "entrust", "globalsign", "microsoft trusted signing", "powershell", "sectigo", "security", "sha256", "signing errors", "signing tools", "signtool", "ssldotcom", "timestamp server", "trusted signing client tools", "windows", "windows security"]
---

Rick Strahl shares his experience with failures using the Microsoft timestamp server for code signing and recommends alternative, more reliable servers. The article provides practical advice and example code for secure code signing workflows.<!--excerpt_end-->

# Don't use the Microsoft Timestamp Server for Signing

**Author: Rick Strahl**

If you use Microsoft Trusted Signing for code signing Windows binaries, you may encounter repeated failures when using the default Microsoft-provided timestamp server (`http://timestamp.acs.microsoft.com`).

![Signing Failed Banner](https://weblog.west-wind.com/images/2026/Dont-use-the-Microsoft-Timestamp-Server-for-Signing/SigningFailedBanner.png)

## The Problem with Microsoft's Timestamp Server

When signing code—binaries or installers—via PowerShell, Microsoft's documentation suggests this server:

```powershell
$timeServer = "http://timestamp.acs.microsoft.com"
```

A typical PowerShell signing script:

```powershell
$timeServer = "http://timestamp.acs.microsoft.com"
$args = @( "sign", "/v", "/debug", "/fd", "SHA256", "/tr", "$timeServer", "/td", "SHA256", "/dlib", "$env:LOCALAPPDATA\Microsoft\MicrosoftTrustedSigningClientTools\Azure.CodeSigning.Dlib.dll", "/dmdf", ".\SignfileMetadata.json" )

# Add non-empty file arguments

foreach ($f in @($file, $file1, $file2, $file3, $file4, $file5, $file6, $file7)) {
  if (![string]::IsNullOrWhiteSpace($f)) {
    $args += $f
  }
}

# Run signtool and capture the exit code

.\signtool.exe $args
```

**Issue**: The Microsoft timestamp server works only about 80% of the time. Frequent issues include failures while signing multiple files for a release, especially with larger installers. This leads to unreliable workflows and deployment headaches.

![Signing Failed With Microsoft Time Service](https://weblog.west-wind.com/images/2026/Dont-use-the-Microsoft-Timestamp-Server-for-Signing/SigningFailedWithMicrosoftTimeService.png)

> It's frustrating that something as basic as a timestamp service fails so often in production signing workflows.

## Solution: Use a Reliable Timestamp Server

Rick recommends switching to more reliable, industry-standard timestamp servers. For example, DigiCert's works consistently:

```powershell
$timeServer = "http://timestamp.digicert.com"
```

Just update your signing script to use this endpoint. Subsequent signing operations should avoid failures traced to unresponsive timestamp servers.

## List of Alternative Timestamp Servers

- **DigiCert**:  
  - `http://timestamp.digicert.com`  
  - `http://timestamp.digicert.com/ts` (alt endpoint)
- **Sectigo/Comodo**:  
  - `http://timestamp.sectigo.com`  
  - `http://timestamp.comodoca.com/rfc3161`
- **GlobalSign**:  
  - `http://timestamp.globalsign.com/?signature=sha2`
- **SSL.com**:  
  - `http://ts.ssl.com`  
  - `http://ts.ssl.com/rfc3161`
- **Entrust**:  
  - `http://timestamp.entrust.net/TSS/RFC3161sha2TS`
- **Certum (Asseco)**:  
  - `http://time.certum.pl`

## Additional Resources

- [Fighting through Setting up Microsoft Trusted Signing](https://weblog.west-wind.com/posts/2025/Jul/20/Fighting-through-Setting-up-Microsoft-Trusted-Signing)

---

**Takeaway:**

- Avoid using `http://timestamp.acs.microsoft.com` in code signing workflows
- Switch to a reputable timestamp authority, such as DigiCert or Sectigo, for reliable results
- Update your signing scripts and CI/CD pipelines accordingly

*Content by Rick Strahl, West Wind Technologies*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2026/Feb/26/Dont-use-the-Microsoft-Timestamp-Server-for-Signing)
