---
title: PowerShell MSI package deprecation and preview updates
primary_section: dotnet
author: Jason Helmick
date: 2026-04-10 14:46:23 +00:00
section_names:
- dotnet
tags:
- .NET
- Accessibility
- Deployment Tooling
- Differential Updates
- Enterprise Deployment
- MSI
- MSIX
- News
- Packaging
- PowerShell
- PowerShell 7.6
- PowerShell 7.7
- PowerShell 7.7 Preview.1
- PowerShell Release
- PowerShell Remoting
- Screen Readers
- Servicing Model
- Software Installation
- System Level Services
- Task Scheduler
- Windows
- Windows Deployment Tools
feed_name: Microsoft PowerShell Blog
external_url: https://devblogs.microsoft.com/powershell/powershell-msi-deprecation/
---

Jason Helmick announces that starting with PowerShell 7.7-preview.1, PowerShell will move away from MSI installers on Windows and adopt MSIX as the primary install method, outlining the servicing, reliability, enterprise deployment, and accessibility reasons behind the change.<!--excerpt_end-->

# PowerShell MSI package deprecation and preview updates

Beginning with **PowerShell 7.7-preview.1 (April 2026)**, the **MSIX package** will be the **primary installation method** for PowerShell on Windows. The PowerShell team will **no longer ship the MSI installer** for new PowerShell releases.

For **existing releases (including PowerShell 7.6)**, MSI packages will continue to be provided. However, MSI is **not planned** for future releases, including **PowerShell 7.7 GA and beyond**.

## Why the PowerShell team is making this change

The post argues that **MSIX** provides a more modern and predictable model for installation and servicing on Windows:

- **Modern installation/servicing model** supported by **Windows deployment tools**
- **Declarative model** that is described as more predictable and reliable than MSI
  - MSI often relies on **custom actions and scripts**, which can lead to inconsistent behavior
- **Built-in update mechanisms**, including **differential updates**
- Ongoing **Microsoft investment** in improving MSIX

It also describes **MSI** as a legacy approach with multiple drawbacks:

- Servicing MSI installs typically requires **external tooling** and often results in **full reinstalls**
- MSI is described as failing to meet **modern accessibility requirements** (notably for **screen reader** usage)
  - Accessible experiences require predictable tab stops and accurate screen reader announcements
- Accessibility is stated as a **core requirement** for PowerShell

The team frames the change as being driven by **accessibility and long-term practicality**, not packaging “modernization” alone.

## Looking forward: known gaps and current investments

The team states the goal is a **fully accessible, reliable, enterprise-ready** installation experience, but acknowledges current MSIX limitations compared to MSI.

At the time of writing, MSIX doesn’t support all scenarios previously enabled by MSI, including:

- **Remoting**
- Execution by **system-level services** (for example, **Task Scheduler**)

The team says they are actively working to address these gaps, including investments in:

- Improving MSIX support for **system-level** and **enterprise deployment** scenarios
- Ensuring **accessibility requirements** are met across all installation paths
- Providing clearer **guidance and tooling** for deployment at scale

They plan to continue sharing updates as this work progresses.

## Closing

The post notes that environments relying on MSI-based deployment may need to adjust, and emphasizes that the focus is keeping PowerShell installations **accessible, predictable, and practical**.

— The PowerShell Team


[Read the entire article](https://devblogs.microsoft.com/powershell/powershell-msi-deprecation/)

