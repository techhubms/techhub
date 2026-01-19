---
external_url: https://www.reddit.com/r/VisualStudio/comments/1mcle8w/widget_creation_with_xbox_game_bar_sdk_missing/
title: 'Widget Creation with Xbox Game Bar SDK: UWP Workload Missing in Visual Studio 2022'
author: Separate_Detective_9
viewing_mode: external
feed_name: Reddit Visual Studio
date: 2025-07-29 19:44:01 +00:00
tags:
- C#
- Game Development
- Project Templates
- SDK
- Universal Windows Platform
- UWP
- VS
- Windows 10
- Workload Installation
- Xbox Game Bar
section_names:
- coding
---
Separate_Detective_9 describes challenges building an Xbox Game Bar widget in C# due to the missing Universal Windows Platform (UWP) workload in Visual Studio 2022, and requests advice or alternatives.<!--excerpt_end-->

## Problem Overview

Separate_Detective_9 is attempting to create an Xbox Game Bar widget in C# using the Universal Windows Platform (UWP) SDK with Visual Studio Community 2022 (version 17.14.9, July 2025) on Windows 10 Home (build 19045). However, the crucial "Universal Windows Platform development" workload is missing from the Visual Studio Installer. This results in the absence of UWP project templates and incomplete Xbox Game Bar SDK integration.

## Steps Tried

1. **Visual Studio Installer Workloads Tab**
    - The "Universal Windows Platform development" workload does not appear under "Desktop & Mobile" workloads.
    - Only options such as .NET Desktop, WinUI, and C++ Desktop/Mobile are visible—UWP is absent.
2. **Checking Individual Components**
    - Searching for keywords like "Universal" and "UWP" only turns up low-level C++ UWP support and SDK/CRT components. The full UWP workload or its templates are missing.
3. **Command-Line Installation**
    - The author attempted to invoke the installer from the command line with:

      ```
      vs_installer.exe modify --installPath "C:\Program Files\Microsoft Visual Studio\2022\Community" --add Microsoft.VisualStudio.Workload.Universal
      ```

    - This only launches the installer GUI, where the UWP workload remains unavailable.
4. **Other Verifications**
    - Confirmed not using Windows Server or an enterprise environment.
    - Both Windows and Visual Studio installations are up to date and legitimate.

## Issues Faced

- All attempted methods failed to make the "Universal Windows Platform development" workload appear.
- The lack of this workload blocks access to UWP project templates and Xbox Game Bar widget development features.

## Requests

- The author is seeking help or advice to:
  - Understand why the UWP workload is missing from their installer.
  - Confirm if the UWP workload should still be available and necessary for Xbox Game Bar widget development.
  - Find any workaround for developing Game Bar widgets without the UWP workload.

## Context

- The problem was posted to Reddit’s VisualStudio community.
- The author included software versions, troubleshooting steps, and a detailed description of the issue.

---

### Remarks

As of the last few years, UWP’s position within the Microsoft ecosystem has shifted, and some workloads may become deprecated or hidden based on Windows versions, Visual Studio versions, or Microsoft’s product strategy. It is important to confirm with official Microsoft documentation or forums if UWP workload availability has officially changed for new Visual Studio 2022 builds or for certain Windows SKUs like Windows 10 Home.

For Xbox Game Bar widget development, UWP has traditionally been required. Alternative solutions may involve checking for WinUI or newer development approaches if UWP is permanently deprecated in some environments.

This post appeared first on "Reddit Visual Studio". [Read the entire article here](https://www.reddit.com/r/VisualStudio/comments/1mcle8w/widget_creation_with_xbox_game_bar_sdk_missing/)
