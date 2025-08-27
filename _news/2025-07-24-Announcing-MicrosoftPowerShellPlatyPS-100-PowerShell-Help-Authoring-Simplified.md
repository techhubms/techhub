---
layout: "post"
title: "Announcing Microsoft.PowerShell.PlatyPS 1.0.0: PowerShell Help Authoring Simplified"
description: "Microsoft announces the GA release of Microsoft.PowerShell.PlatyPS 1.0.0, a major update for creating and maintaining PowerShell help files using Markdown. The new version features a C# rewrite, improved performance, enhanced cmdlet support, and cross-platform compatibility on PowerShell 7+."
author: "Jason Helmick, Sean Wheeler"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/powershell/announcing-platyps-100/"
viewing_mode: "external"
feed_name: "Microsoft PowerShell Blog"
feed_url: "https://devblogs.microsoft.com/powershell/feed/"
date: 2025-07-24 21:35:14 +00:00
permalink: "/2025-07-24-Announcing-MicrosoftPowerShellPlatyPS-100-PowerShell-Help-Authoring-Simplified.html"
categories: ["Coding"]
tags: ["C#", "Cmdlets", "Coding", "Cross Platform", "Documentation", "MAML", "Markdown", "Microsoft Learn", "News", "PlatyPS", "PowerShell", "PowerShell Gallery", "PowerShell Help", "PowerShell PlatyPS", "VS Code"]
tags_normalized: ["csharp", "cmdlets", "coding", "cross platform", "documentation", "maml", "markdown", "microsoft learn", "news", "platyps", "powershell", "powershell gallery", "powershell help", "powershell platyps", "vs code"]
---

Authored by Jason Helmick and Sean Wheeler, this post introduces Microsoft.PowerShell.PlatyPS 1.0.0, detailing the new capabilities and improvements for PowerShell help file authoring.<!--excerpt_end-->

## Announcing Microsoft.PowerShell.PlatyPS 1.0.0

**Authors**: Jason Helmick, Sean Wheeler

We’re pleased to announce the general availability (GA) release of [Microsoft.PowerShell.PlatyPS v1.0.0](https://www.powershellgallery.com/packages/Microsoft.PowerShell.PlatyPS).

### What is PlatyPS?

**PlatyPS** is a tool that Microsoft utilizes to generate PowerShell content available from `Get-Help` and to build documentation published on [Microsoft Learn](https://learn.microsoft.com/powershell/scripting). Traditionally, PowerShell help files are stored using [Microsoft Assistance Markup Language (MAML)](https://wikipedia.org/wiki/Microsoft_Assistance_Markup_Language), an XML-based format. PlatyPS streamlines the authoring process by enabling users to write help files in [Markdown](https://wikipedia.org/wiki/Markdown), a format widely adopted across the software industry with support in numerous editors, including **Visual Studio Code**. Markdown files can be easily converted to MAML for help file consumption.

### Key Improvements in 1.0.0

- **Major Re-write in C#**: Leverages [markdig](https://github.com/xoofx/markdig) for Markdown parsing (the same library used by Microsoft Learn).
- **More Accurate Cmdlet Descriptions**: Provides richer details of PowerShell cmdlets and their parameters, including previously unavailable information.
- **Object Model for Help Files**: Enables manipulation and cmdlet chaining for complex documentation operations.
- **Improved Performance**: Processes thousands of Markdown files in seconds.
- **Cross-Platform Support**: Runs on Windows PowerShell 5.1+ and PowerShell 7+ across Windows, Linux, and macOS.

### Transition from Previous Versions

With this release, **Microsoft.PowerShell.PlatyPS** is the officially supported tool. Older versions, such as **platyPS v0.14.2**, are no longer supported. Users are strongly encouraged to upgrade. Note: Any scripts using the old version must be updated for compatibility with the new cmdlets.

### Getting Started

To install **Microsoft.PowerShell.PlatyPS 1.0.0**, use the following command from PSGallery:

```powershell
Install-PSResource -Name Microsoft.PowerShell.PlatyPS
```

#### Documentation and References

- [Microsoft.PowerShell.PlatyPS Cmdlet Reference](https://learn.microsoft.com/powershell/module/microsoft.powershell.platyps)
- Example usage: [New-MarkdownCommandHelp Example #1](https://learn.microsoft.com/powershell/module/microsoft.powershell.platyps/new-markdowncommandhelp#example-1-create-markdown-help-files-for-a-module)

### Community Contribution

Microsoft welcomes community feedback and contributions:

- Report issues or submit ideas at the [PlatyPS GitHub repository](https://github.com/PowerShell/platyPS).
- Interested in contributing? Follow the guidelines in the [GitHub Readme](https://github.com/PowerShell/platyPS/blob/main/README.md).

---

*Sr. Product Manager, PowerShell*

Jason Helmick

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/announcing-platyps-100/)
