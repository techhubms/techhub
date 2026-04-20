---
section_names:
- dotnet
title: Microsoft calls time on ASP.NET Core 2.3 on .NET Framework
tags:
- .NET
- .NET 10
- .NET Framework
- Arm64
- ASP.NET Core
- ASP.NET Core 2.1
- ASP.NET Core 2.2
- ASP.NET Core 2.3
- Blogs
- End Of Support
- LTS
- Migration
- NuGet
- Security Patches
- Semantic Versioning
- SemVer
- Servicing
- Support Lifecycle
- Windows Forms
- Windows Only Runtime
- Windows Server
- WPF
external_url: https://www.devclass.com/devops/2026/04/13/microsoft-calls-time-on-aspnet-core-23-on-net-framework/5216962
feed_name: DevClass
primary_section: dotnet
date: 2026-04-13 16:40:49 +00:00
author: DevClass.com
---

DevClass.com reports that Microsoft will end support for ASP.NET Core 2.3 on April 7, 2027, leaving it without security patches or fixes and pushing teams running on .NET Framework toward migrating to modern ASP.NET on .NET 10.<!--excerpt_end-->

# Microsoft calls time on ASP.NET Core 2.3 on .NET Framework

Microsoft has set an end-of-support date of **April 7, 2027**, for **ASP.NET Core 2.3** — described as **the only supported version of ASP.NET Core that runs on .NET Framework** — even though **.NET Framework (and original ASP.NET)** will continue to be supported.

> "After that date, Microsoft will no longer provide security patches, bug fixes, or technical support for ASP.NET Core 2.3,"
> said principal product manager Daniel Roth.

Source: Daniel Roth’s post on the .NET blog: https://devblogs.microsoft.com/dotnet/aspnet-core-2-3-end-of-support/

## What Microsoft is telling customers to do

- Microsoft’s guidance is to **upgrade to modern ASP.NET on .NET 10**.

## .NET Framework context

- **.NET Framework** is the **Windows-only** version of .NET.
- It is still maintained, but mainly for **stability** rather than major new features.
- **4.8.1** is cited as the latest release, adding:
  - **Native Arm64 support**
  - **Accessibility improvements** for:
    - **Windows Forms**
    - **WPF (Windows Presentation Foundation)**
- .NET Framework is treated as a **component of Windows** and follows the **Windows support lifecycle**.

## Why ASP.NET Core on .NET Framework is a confusing corner

- ASP.NET Core shipped alongside **.NET Core** (cross-platform .NET).
- Early ASP.NET Core (2016) could also run on .NET Framework.
- That ability was dropped with **ASP.NET Core 3.0 (2019)**.

### Versions and support pressure

- The last ASP.NET Core version that ran on .NET Framework was **2.2** (end of 2018).
- The last **LTS** release was **2.1** (earlier in 2018).

This left some teams in a bind: apps depending on **2.2 features** but running on .NET Framework had to either:

- revert to **2.1** and address compatibility issues, or
- migrate to run on **.NET Core**.

## Why ASP.NET Core 2.3 existed at all

Microsoft addressed this by releasing **ASP.NET Core 2.3** in early 2025, but it was effectively a **re-release of 2.1** (despite the version number).

- Microsoft’s stated reasoning (as described in the article):
  - **2.2** had breaking changes from **2.1** and became unsupported earlier.
  - **2.3** reverted to the most recently supported code line.

Reference: https://devblogs.microsoft.com/dotnet/servicing-release-advisory-aspnetcore-23/

### The SemVer problem

The article notes this approach is not how **semantic versioning (SemVer)** is typically expected to work, and that it caused real-world issues.

A user reported that moving from **2.1 to 2.3** removed code that had been added in **2.2**, which they described as an unexpected breaking change for a minor-version upgrade:

- GitHub issue comment: https://github.com/dotnet/aspnetcore/issues/58598#issuecomment-2596133585

## Why support is ending so soon

ASP.NET Core 2.3 will be supported for **fewer than two years**.

Microsoft’s justification (as described in the article) relies on lifecycle language that treats the framework as a **"tool"**. A Microsoft support lifecycle document says that for a tool:

- the only lifecycle requirement is a **minimum of 12 months notification** before support ends.

Reference: https://learn.microsoft.com/en-us/troubleshoot/developer/webapps/aspnet/development/support-lifecycle-web-stack

## Evidence that 2.2 and 2.3 are still used

The article points to **NuGet statistics** indicating both **2.2** and **2.3** are still frequently installed:

- NuGet stats: https://www.nuget.org/stats/packages/Microsoft.AspNetCore?groupby=Version

## Microsoft’s rationale and migration messaging

Roth is quoted as saying support is ending due to the ongoing cost of:

- maintenance
- compliance

…which pulls resources away from investment in the modern .NET platform.

The article also says Microsoft’s original reason for supporting ASP.NET Core on .NET Framework was to make it easier to migrate from **ASP.NET** to **ASP.NET Core**, but that **2.3 is now so out of date** that Microsoft no longer recommends it as a long-term migration strategy.

It also reports Roth suggesting users migrating from ASP.NET should now use **AI tools** to assist migration.

## Reaction and the practical trade-offs

Some users view the "tooling" lifecycle rationale as a loophole enabling premature end-of-support.

The article notes that moving to **.NET 10 and modern ASP.NET** has benefits, including the ability to run on **Linux** as well as Windows, but that some organizations with legacy, on-prem apps primarily want continued stability.

---

**Original article context/title:** Microsoft calls time on ASP.NET Core 2.3 on .NET Framework


[Read the entire article](https://www.devclass.com/devops/2026/04/13/microsoft-calls-time-on-aspnet-core-23-on-net-framework/5216962)

