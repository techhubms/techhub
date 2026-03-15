---
external_url: https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-march-2026-servicing-updates/
title: '.NET and .NET Framework March 2026 Servicing Updates: Security and Release Roundup'
author: Rahul Bhandari (MSFT), Tara Overfield
primary_section: dotnet
feed_name: Microsoft .NET Blog
date: 2026-03-10 20:33:31 +00:00
tags:
- .NET
- .NET 10.0
- .NET 8.0
- .NET 9.0
- .NET Framework
- ASP.NET Core
- CVE 26127
- CVE 26130
- CVE 26131
- EF Core
- Maintenance
- Maintenance & Updates
- Microsoft
- News
- Patch Tuesday
- Runtime
- Security
- Security Update
- Servicing Release
- WPF
section_names:
- dotnet
- security
---
Rahul Bhandari (MSFT) and Tara Overfield summarize important security fixes and servicing information for .NET and .NET Framework with the March 2026 update, including CVE resolutions and detailed changelogs.<!--excerpt_end-->

# .NET and .NET Framework March 2026 Servicing Updates

**Authors:** Rahul Bhandari (MSFT), Tara Overfield

Welcome to the recap of the March 2026 servicing updates for .NET and .NET Framework. This update is critical for developers and administrators maintaining Microsoft .NET applications, as it addresses several security vulnerabilities and delivers important fixes.

## Security Improvements

The March 10, 2026 update refreshes both .NET Core (10.0, 9.0, 8.0) and .NET Framework with security and non-security fixes. This month's release addresses these security vulnerabilities:

- [CVE-2026-26130](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2026-26130): .NET Security Feature Bypass Vulnerability (.NET 10.0, 9.0, 8.0)
- [CVE-2026-26127](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2026-26127): .NET Security Feature Bypass Vulnerability (.NET 10.0, 9.0)
- [CVE-2026-26131](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2026-26131): .NET Security Feature Bypass Vulnerability (.NET 10.0)

## Release Resources

| Version | Release Notes | Installers/Binaries | Container Images | Linux Packages | Known Issues |
| ------- | ------------- | ------------------- | --------------- | -------------- | ------------ |
| .NET 10.0 | [10.0.4](https://github.com/dotnet/core/blob/main/release-notes/10.0/10.0.4/10.0.4.md) | [Download](https://dotnet.microsoft.com/download/dotnet/10.0) | [Container Images](https://mcr.microsoft.com/catalog?search=dotnet/) | [Linux](https://github.com/dotnet/core/blob/main/release-notes/10.0/install-linux.md) | [Known Issues](https://github.com/dotnet/core/blob/main/release-notes/10.0/known-issues.md) |
| .NET 9.0 | [9.0.14](https://github.com/dotnet/core/blob/main/release-notes/9.0/9.0.14/9.0.14.md) | [Download](https://dotnet.microsoft.com/download/dotnet/9.0) | [Container Images](https://mcr.microsoft.com/catalog?search=dotnet/) | [Linux](https://github.com/dotnet/core/blob/main/release-notes/9.0/install-linux.md) | [Known Issues](https://github.com/dotnet/core/blob/main/release-notes/9.0/known-issues.md) |
| .NET 8.0 | [8.0.25](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.25/8.0.25.md) | [Download](https://dotnet.microsoft.com/download/dotnet/8.0) | [Container Images](https://mcr.microsoft.com/catalog?search=dotnet/) | [Linux](https://github.com/dotnet/core/blob/main/release-notes/8.0/install-linux.md) | [Known Issues](https://github.com/dotnet/core/blob/main/release-notes/8.0/known-issues.md) |

## Changelogs

- ASP.NET Core: [10.0.4](https://github.com/dotnet/aspnetcore/issues?q=milestone%3A10.0.4%20is%3Aclosed%20label%3Aservicing-approved)
- Entity Framework Core: [10.0.4](https://github.com/dotnet/efcore/issues?q=state%3Aclosed%20label%3AServicing-approved%20milestone%3A10.0.4)
- Runtime: [10.0.4](https://github.com/dotnet/runtime/issues?q=milestone%3A10.0.4%20is%3Aclosed%20label%3Aservicing-approved), [9.0.14](https://github.com/dotnet/runtime/issues?q=milestone%3A9.0.14%20is%3Aclosed%20label%3Aservicing-approved), [8.0.25](https://github.com/dotnet/runtime/issues?q=milestone%3A8.0.25%20is%3Aclosed%20label%3Aservicing-approved)
- WPF: [10.0.4](https://github.com/dotnet/wpf/issues?q=state:closed%20label:servicing-approved%20milestone:10.0.4), [9.0.14](https://github.com/dotnet/wpf/issues?q=state:closed%20label:servicing-approved%20milestone:9.0.14)

Feedback and additional details can be shared in the [Release feedback issue](https://github.com/dotnet/core/issues/10250).

## .NET Framework March 2026 Updates

There are no new security or non-security updates for .NET Framework this month. For previous updates, refer to [the .NET Framework release notes](https://learn.microsoft.com/dotnet/framework/release-notes/release-notes).

---

Stay updated by applying the latest servicing releases and monitoring known issues.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-march-2026-servicing-updates/)
