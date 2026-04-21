---
section_names:
- dotnet
- security
tags:
- .NET
- .NET   Info
- .NET 10
- .NET 10.0.7
- ASP.NET Core
- ASP.NET Core Data Protection
- Authenticated Encryption
- Container Images
- CVE 40372
- Decryption Regression
- Elevation Of Privilege
- GitHub Issues
- HMAC
- Maintenance & Updates
- MCR
- Microsoft.AspNetCore.DataProtection
- News
- NuGet
- OOB
- Out Of Band Update
- Patch Tuesday
- Release Notes
- Runtime
- SDK
- Security
- Security Update
primary_section: dotnet
author: Rahul Bhandari (MSFT)
feed_name: Microsoft .NET Blog
date: 2026-04-21 18:48:34 +00:00
title: .NET 10.0.7 Out-of-Band Security Update
external_url: https://devblogs.microsoft.com/dotnet/dotnet-10-0-7-oob-security-update/
---

Rahul Bhandari (MSFT) announces .NET 10.0.7 as an out-of-band update that fixes a decryption regression and addresses CVE-2026-40372 in the Microsoft.AspNetCore.DataProtection package used by ASP.NET Core Data Protection.<!--excerpt_end-->

# .NET 10.0.7 Out-of-Band Security Update

We are releasing **.NET 10.0.7** as an **out-of-band (OOB)** update to address a security issue introduced in the **Microsoft.AspNetCore.DataProtection** NuGet package.

## Security update details

This release includes a fix for **CVE-2026-40372**:

- CVE: [CVE-2026-40372](https://github.com/dotnet/announcements/issues/395)

After the Patch Tuesday **10.0.6** release, some customers reported that **decryption was failing** in their applications. This behavior was reported in:

- Issue: [aspnetcore issue #66335](https://github.com/dotnet/aspnetcore/issues/66335)

While investigating those reports, Microsoft determined that the regression also **exposed a vulnerability**.

### What was vulnerable

In versions **10.0.0 through 10.0.6** of the **Microsoft.AspNetCore.DataProtection** package:

- The **managed authenticated encryptor** could compute its **HMAC validation tag** over the **wrong bytes** of the payload.
- It could then **discard the computed hash**.
- This could result in an **elevation of privilege** vulnerability.

## Update required

If your application uses **ASP.NET Core Data Protection**, update the **Microsoft.AspNetCore.DataProtection** package to **10.0.7** as soon as possible to address:

- the decryption regression
- the security vulnerability

## Download .NET 10.0.7

- Release Notes: [10.0 release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/README.md)
- Installers and binaries: [10.0.7](https://dotnet.microsoft.com/download/dotnet/10.0)
- Container Images: [images](https://mcr.microsoft.com/catalog?search=dotnet/)
- Linux packages: [10.0](https://github.com/dotnet/core/blob/main/release-notes/10.0/install-linux.md)
- Known Issues: [10.0](https://github.com/dotnet/core/blob/main/release-notes/10.0/known-issues.md)

## Installation guidance

1. Download and install the [.NET 10.0.7 SDK or Runtime](https://dotnet.microsoft.com/download/dotnet/10.0).
2. Verify installation by running:

```bash
dotnet --info
```

3. Confirm you are on **10.0.7**.
4. Rebuild and redeploy your applications using updated images or packages.

## Share your feedback

If you experience any issues after installing this update, share feedback here:

- [.NET release feedback issues](https://github.com/dotnet/core/issues)


[Read the entire article](https://devblogs.microsoft.com/dotnet/dotnet-10-0-7-oob-security-update/)

