---
external_url: https://www.devclass.com/development/2026/01/07/microsoft-open-sources-xaml-studio-amid-developer-discontent-with-visual-studio-designers/4079573
title: Microsoft Open Sources XAML Studio Amid Developer Discontent with Visual Studio Designers
author: DevClass.com
primary_section: dotnet
feed_name: DevClass
date: 2026-01-07 14:58:24 +00:00
tags:
- .NET Foundation
- Arm64
- Blogs
- Designer Tools
- Fluent UI
- MAUI
- Microsoft Garage
- Open Source
- SDK Style Projects
- UI Frameworks
- UWP
- VS
- Windows Development
- Windows Forms
- WinUI 3
- WPF
- XAML Designer
- XAML Studio
- .NET
section_names:
- dotnet
---
DevClass.com reports on Microsoft's decision to open source XAML Studio, as detailed by Michael Hawker, highlighting ongoing frustrations in the Visual Studio developer community regarding UI designer support.<!--excerpt_end-->

# Microsoft Open Sources XAML Studio Amid Developer Discontent with Visual Studio Designers

Microsoft has open sourced XAML Studio, a lightweight tool for designing XAML-based user interfaces. Developed by Michael Hawker, a senior software engineer at Microsoft, XAML Studio was created within Microsoft Garage, the company's experimental projects division. The tool originally evolved from XamlPad and gained recognition for its IntelliSense and live preview features, facilitating rapid UI prototyping.

## Key Highlights

- **Open Source Transition**: XAML Studio has been open sourced with support from the .NET Foundation as a "seed project." After years of being a sideline project, the code for version 2.0 is now available, though only as a pre-beta.
- **Features**: XAML Studio offers live previews, IntelliSense, and quick links to documentation for WinUI elements, but is not a full visual designer. Developers use it to design UIs and then copy-paste code into Visual Studio.
- **Developer Frustrations**: The move comes amid longstanding complaints from developers about the lack of robust visual design tools in Visual Studio for modern Windows frameworks like WinUI 3 and .NET MAUI. Microsoft has clarified that drag-and-drop UI designers for these frameworks are not planned, referring users to features like XAML Hot Reload and Live Preview instead.
- **Framework Fragmentation**: The XAML ecosystem is complex, with distinct differences between UWP, WinUI 3, WinUI 2, and cross-platform .NET MAUI. As a result, many developers interested in a visual design surface for new frameworks still revert to older technologies like Windows Forms or WPF for productivity.
- **Future Direction**: The 2.0 roadmap aims to consolidate support for both (Modern) UWP and WinUI 3 as part of a single SDK-style project, using Microsoft's Fluent UI design language. Community involvement remains limited, but Hawker hopes open sourcing XAML Studio will attract more contributors.

## Technical Insights

- **Compatibility**: The latest version runs in Visual Studio 2026 on Arm64 Windows, but only the x86 build is functional, and even then only in debug mode. The last stable version (1.1) is outdated.
- **Community and Maintenance**: The project remains largely a one-person effort, though open sourcing may stimulate broader participation.
- **Documentation and Alternatives**: Microsoft's official documentation acknowledges the lack of visual XAML designers for WinUI 3 and .NET MAUI, recommending XAML Hot Reload for rapid UI development.

## Further Reading and Resources

- [XAML Studio GitHub Discussions](https://github.com/dotnet/XAMLStudio/discussions/26)
- [Project Roadmap](https://github.com/dotnet/XAMLStudio/issues/34)
- [Official Documentation on XAML Designer](https://learn.microsoft.com/en-us/visualstudio/xaml-tools/creating-a-ui-by-using-xaml-designer-in-visual-studio?view=visualstudio)
- [Visual Studio Developer Community Feedback](https://developercommunity.visualstudio.com/t/Feature-request-Visual-designer-for-Wi/1608476)

## Conclusion

While open sourcing XAML Studio is a positive step for transparency and community-driven development, Visual Studio's limitations with modern visual design tools continue to challenge developers building for Windows. The future of XAML-based desktop development remains uncertain, with developers weighing productivity needs against the available tooling.

This post appeared first on "DevClass". [Read the entire article here](https://www.devclass.com/development/2026/01/07/microsoft-open-sources-xaml-studio-amid-developer-discontent-with-visual-studio-designers/4079573)
