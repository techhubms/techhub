---
layout: "post"
title: ".NET and .NET Framework March 2026 Servicing Updates: Security and Release Roundup"
description: "This news update provides a detailed overview of the March 2026 servicing releases for .NET and .NET Framework. Key highlights include the resolution of multiple security vulnerabilities across .NET 10.0, 9.0, and 8.0, release notes, installer and container image links, and changelogs for ASP.NET Core, Entity Framework Core, Runtime, and WPF. The update also notes that no new .NET Framework patches were released this month."
author: "Rahul Bhandari (MSFT), Tara Overfield"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-march-2026-servicing-updates/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2026-03-10 20:33:31 +00:00
permalink: "/2026-03-10-NET-and-NET-Framework-March-2026-Servicing-Updates-Security-and-Release-Roundup.html"
categories: ["Coding", "Security"]
tags: [".NET", ".NET 10.0", ".NET 8.0", ".NET 9.0", ".NET Framework", "ASP.NET Core", "Coding", "CVE 26127", "CVE 26130", "CVE 26131", "EF Core", "Maintenance", "Maintenance & Updates", "Microsoft", "News", "Patch Tuesday", "Runtime", "Security", "Security Update", "Servicing Release", "WPF"]
tags_normalized: ["dotnet", "dotnet 10dot0", "dotnet 8dot0", "dotnet 9dot0", "dotnet framework", "aspdotnet core", "coding", "cve 26127", "cve 26130", "cve 26131", "ef core", "maintenance", "maintenance and updates", "microsoft", "news", "patch tuesday", "runtime", "security", "security update", "servicing release", "wpf"]
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
