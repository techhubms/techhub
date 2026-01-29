---
external_url: https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-october-2025-servicing-updates/
title: '.NET and .NET Framework October 2025 Servicing Updates: Security Fixes and Release Details'
author: Tara Overfield, Victor Israel-Bolarinwa
feed_name: Microsoft .NET Blog
date: 2025-10-14 22:50:29 +00:00
tags:
- .NET
- .NET 8.0
- .NET 9.0
- .NET Framework
- Arcade
- ASP.NET Core
- CVE
- Installer
- Maintenance & Updates
- Microsoft
- Patching
- Release Notes
- Runtime
- SDK
- Security Update
- Servicing
- Vulnerability Management
- Coding
- Security
- News
section_names:
- coding
- security
primary_section: coding
---
Tara Overfield and Victor Israel-Bolarinwa provide a thorough overview of the October 2025 .NET and .NET Framework servicing updates, highlighting key security patches and release details.<!--excerpt_end-->

# .NET and .NET Framework October 2025 Servicing Updates

By Tara Overfield and Victor Israel-Bolarinwa

This post presents an overview of the most recent servicing updates for .NET and .NET Framework released in October 2025, focusing on both security and non-security improvements.

## Security Improvements

The October 14, 2025 update addresses several vulnerabilities:

| CVE # | Title | Applies to |
| --- | --- | --- |
| [CVE-2025-55248](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2025-55248) | .NET Information Disclosure Vulnerability | .NET 9.0, .NET 8.0 |
| [CVE-2025-55315](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2025-55315) | .NET Security Feature Bypass Vulnerability | .NET 9.0, .NET 8.0 |
| [CVE-2025-55247](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2025-55247) | .NET Denial of Service Vulnerability | .NET 9.0, .NET 8.0 |
| [CVE-2025-21176](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2025-21176) | .NET Remote Code Execution Vulnerability | .NET Framework 3.5, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1 |

These vulnerabilities include information disclosure, security feature bypass, denial of service, and remote code execution affecting multiple supported versions. All recommended updates include both security and non-security fixes.

## Quick Access Links

- **Release Notes:** [8.0.21](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.21/8.0.21.md) | [9.0.10](https://github.com/dotnet/core/blob/main/release-notes/9.0/9.0.10/9.0.10.md)
- **Installers:** [.NET 8.0.21](https://dotnet.microsoft.com/download/dotnet/8.0) | [.NET 9.0.10](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Container Images:** [Browse .NET images](https://mcr.microsoft.com/catalog?search=dotnet/)
- **Linux Packages:** [8.0.21](https://github.com/dotnet/core/blob/main/release-notes/8.0/install-linux.md) | [9.0.10](https://github.com/dotnet/core/blob/main/release-notes/9.0/install-linux.md)
- **Known Issues:** [8.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/known-issues.md) | [9.0](https://github.com/dotnet/core/blob/main/release-notes/9.0/known-issues.md)

### Related Changelogs

- **Runtime:** [8.0.21](https://github.com/dotnet/runtime/issues?q=milestone%3A8.0.21%20is%3Aclosed%20label%3Aservicing-approved) | [9.0.10](https://github.com/dotnet/runtime/issues?q=milestone%3A9.0.10%20is%3Aclosed%20label%3Aservicing-approved)
- **AspNetCore:** [8.0.21](https://github.com/dotnet/aspnetcore/issues?q=milestone%3A8.0.21%20is%3Aclosed%20label%3Aservicing-approved) | [9.0.10](https://github.com/dotnet/aspnetcore/issues?q=milestone%3A9.0.10%20is%3Aclosed%20label%3Aservicing-approved)
- **SDK:** [8.0.21](https://github.com/dotnet/sdk/issues?q=milestone%3A8.0.21%20is%3Aclosed%20label%3Aservicing-approved) | [9.0.10](https://github.com/dotnet/sdk/issues?q=milestone%3A9.0.10%20is%3Aclosed%20label%3Aservicing-approved)
- **Installer:** [8.0.21](https://github.com/dotnet/installer/issues?q=milestone%3A8.0.21%20is%3Aclosed%20label%3Aservicing-approved)
- **Arcade:** [8.0.21](https://github.com/dotnet/arcade/issues?q=milestone%3A8.0.21%20is%3Aclosed%20label%3Aservicing-approved) | [9.0.10](https://github.com/dotnet/arcade/issues?q=milestone%3A9.0.10%20is%3Aclosed%20label%3Aservicing-approved)

Feedback on this release can be provided in the [Release feedback issue](https://github.com/dotnet/core/issues/10120).

## .NET Framework October 2025 Updates

New security and non-security updates have been made available for .NET Framework. See the latest [release notes for .NET Framework](https://learn.microsoft.com/dotnet/framework/release-notes/release-notes) for details.

## Recommendation

All developers and administrators should update to the latest servicing releases to maintain secure, supported environments. Regularly reviewing and applying these updates is critical for ongoing security and support.

---

For more information and continuing updates, visit the official [.NET Blog](https://devblogs.microsoft.com/dotnet).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-october-2025-servicing-updates/)
