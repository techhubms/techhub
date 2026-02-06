---
external_url: https://www.reddit.com/r/dotnet/comments/1mfnu3z/net80_maui_size_of_my_applications_window/
title: '.NET 8.0 MAUI: How to Set Minimum Window Size for Your Application'
author: Vor__texx
feed_name: Reddit DotNet
date: 2025-08-02 11:04:25 +00:00
tags:
- .NET
- .NET 8
- App Development
- Cross Platform
- MainWindow.xaml.cs
- MAUI
- Minimum Window Size
- UI Constraints
- Windows
- WindowSubClassHelper.cs
- XAML
- Community
section_names:
- dotnet
primary_section: dotnet
---
Vor__texx shares their struggle to set a minimum window size in a .NET 8.0 MAUI application, detailing the approaches tried and asking the community for help.<!--excerpt_end-->

## Summary

Vor__texx describes challenges faced when trying to restrict the minimum window size of a .NET 8.0 MAUI application running on Windows. The author has attempted several solutions, including searching online and consulting ChatGPT, but has yet to find a definitive method.

### Steps Attempted

- **Created `MainWindow.xaml.cs`** in the `Project/Platforms/Windows` directory.
- **Implemented a `WindowSubClassHelper.cs`** to try to enforce window size constraints.

The user shares screenshots (linked images) showing the issue, where the window can be resized too much in various directions, resulting in unwantedly small visual states.

### Problem Statement

Despite the implemented approaches, the application window can be minimized in size by the user more than desired. The author seeks a simple, reliable solution to prevent this behavior.

### Call for Help

The post ends with a thank you and an open request for anyone with experience in MAUI on .NET 8.0 to share possible solutions or workarounds for enforcing minimum window sizes, particularly through code or project setup.

---

#### Key Points

- The author provides partial solutions using C# code and Windows-specific project paths.
- Screenshots illustrate the undesired ability to overly reduce the app's window.
- The post reflects a common challenge with cross-platform UI toolkits and platform-specific requirements.

Anyone able to address minimum window sizing in .NET 8.0 MAUI, especially for Windows desktop apps, is encouraged to contribute.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mfnu3z/net80_maui_size_of_my_applications_window/)
