---
layout: "post"
title: ".NET 8.0 MAUI: How to Set Minimum Window Size for Your Application"
description: "The author seeks advice on enforcing a minimum window size in a .NET MAUI app for Windows. Despite searching online and asking ChatGPT, the author could not find a straightforward solution. They share code snippets for customizing MainWindow.xaml.cs and implementing a helper file, hoping for clearer answers or guidance from the community."
author: "Vor__texx"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/dotnet/comments/1mfnu3z/net80_maui_size_of_my_applications_window/"
viewing_mode: "external"
feed_name: "Reddit DotNet"
feed_url: "https://www.reddit.com/r/dotnet/.rss"
date: 2025-08-02 11:04:25 +00:00
permalink: "/2025-08-02-NET-80-MAUI-How-to-Set-Minimum-Window-Size-for-Your-Application.html"
categories: ["Coding"]
tags: [".NET", ".NET 8", ".NET MAUI", "App Development", "Coding", "Community", "Cross Platform", "MainWindow.xaml.cs", "Minimum Window Size", "UI Constraints", "Windows", "WindowSubClassHelper.cs", "XAML"]
tags_normalized: ["net", "net 8", "net maui", "app development", "coding", "community", "cross platform", "mainwindow dot xaml dot cs", "minimum window size", "ui constraints", "windows", "windowsubclasshelper dot cs", "xaml"]
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
