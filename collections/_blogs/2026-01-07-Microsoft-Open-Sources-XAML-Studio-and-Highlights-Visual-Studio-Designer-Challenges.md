---
external_url: https://devclass.com/2026/01/07/microsoft-open-sources-xaml-studio-amid-developer-discontent-with-visual-studio-designers/
title: Microsoft Open Sources XAML Studio and Highlights Visual Studio Designer Challenges
author: Tim Anderson
feed_name: DevClass
date: 2026-01-07 14:58:24 +00:00
tags:
- .NET
- Desktop Applications
- Development
- Fluent UI
- IntelliSense
- Microsoft
- Microsoft Garage
- Microsoft Store
- Open Source
- UI Design
- UWP
- VS
- Windows Development
- WinUI 3
- XAML
- XAML Studio
section_names:
- coding
---
Tim Anderson reports on Microsoft open sourcing XAML Studio, offering insights into the tool’s new status, ongoing development, and broader implications for the Windows desktop application developer community.<!--excerpt_end-->

# Microsoft Open Sources XAML Studio and Highlights Visual Studio Designer Challenges

**Author: Tim Anderson**

## Overview

Microsoft has open sourced XAML Studio, a lightweight XAML interface creation tool, as developer frustration continues over the absence of robust visual designers within Visual Studio—particularly for WinUI 3 and .NET MAUI frameworks.

## Background

- **XAML Studio**: Developed by Michael Hawker as a Microsoft Garage project, the tool provides features like IntelliSense, live preview, and links to WinUI documentation. Its primary workflow involves designing UIs in XAML Studio, then copying and pasting the markup into Visual Studio.
- **Version History**: The last official release of XAML Studio (1.1) appeared in late 2019, leading many to believe the project had been abandoned.

## Open Source Transition

- After years of effort, Michael Hawker—supported by the .NET Foundation—has succeeded in open sourcing XAML Studio, which is now a foundational or "seed" project.
- Ongoing work on **XAML Studio 2.0** introduces a refreshed UI based on Microsoft Fluent UI and supports WinUI 3 components. Early pre-beta builds run in Visual Studio 2026 on Windows Arm64 (limited to x86 debugging scenarios).
- The project roadmap aims to unify support for (Modern) UWP and WinUI 3 within a single SDK-style project.

## Current Status and Community

- XAML Studio is currently maintained mostly by one person (Hawker), so long-term progress will require broader community involvement.
- The article points out the difficulty in Microsoft's XAML ecosystem: UWP, WinUI 2, WinUI 3, and .NET MAUI all use variations of XAML but differ significantly in capabilities and support.

## Visual Designers: Developer Sentiment

- Windows application development once relied heavily on drag-and-drop UI designers (e.g., Visual Basic), but Visual Studio currently lacks true visual design support for WinUI 3.
- Documentation explicitly states that the "WinUI 3 / .NET MAUI XAML designer is not supported" and recommends using Hot Reload instead.
- Feature requests for a WinUI 3 designer remain "under review" and prospects for change appear limited.
- Hot Reload and Live Preview have seen improvements, but these do not fully address developer desires for a robust designer.
- For .NET MAUI, Microsoft has confirmed that "a drag-and-drop UI designer is not part of our direction."

## Implications for Developers

- Developers wanting native Windows 11 look and feel are steered toward WinUI 3, but productivity concerns mean many still use legacy frameworks like Windows Forms or WPF, which retain designer support.
- The future success of XAML Studio will rely on further community contributions and its ability to fill the gap left by Visual Studio.

## References

- [XAML Studio GitHub Discussions](https://github.com/dotnet/XAMLStudio/discussions/26)
- [XAML Studio Roadmap](https://github.com/dotnet/XAMLStudio/issues/34)
- [Visual Studio XAML Designer Documentation](https://learn.microsoft.com/en-us/visualstudio/xaml-tools/creating-a-ui-by-using-xaml-designer-in-visual-studio?view=visualstudio)
- [Feature Request: Visual Designer for WinUI 3](https://developercommunity.visualstudio.com/t/Feature-request-Visual-designer-for-Wi/1608476)
- [Live Preview/Hot Reload Update](https://developercommunity.visualstudio.com/t/Updates-to-Live-Preview-Hot-Reload-and/10846679)
- [MAUI Designer Direction](https://developercommunity.visualstudio.com/t/XAML-Designer-for-Net-MAUI/10224319)

## Conclusion

The open sourcing of XAML Studio offers hope for those seeking modern XAML UI design tools, but for now, Visual Studio’s designer limitations remain a significant pain point for many Windows application developers.

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2026/01/07/microsoft-open-sources-xaml-studio-amid-developer-discontent-with-visual-studio-designers/)
