---
layout: "post"
title: ".NET 8 Windows Service Fails to Start Due to .NETCore.App Version Mismatch"
description: "This community post analyzes an issue where a .NET 8 Windows service fails to start because the required Microsoft.NETCore.App version is missing, despite the presence of the WindowsDesktop.App runtime. The discussion centers around version resolution, runtime dependencies, and explains why the desktop runtime does not satisfy the Microsoft.NETCore.App version required by the application."
author: "JY_2013"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/net-runtime/net-runtime-issues-application-not-starting-up/m-p/4466585#M773"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=dotnet"
date: 2025-11-03 11:28:20 +00:00
permalink: "/community/2025-11-03-NET-8-Windows-Service-Fails-to-Start-Due-to-NETCoreApp-Version-Mismatch.html"
categories: ["Coding"]
tags: [".NET", ".NET 8", "Application Failure", "Coding", "Community", "Dependency Resolution", "Event Viewer", "Framework Dependency", "Microsoft.NETCore.App", "Microsoft.WindowsDesktop.App", "Publishing", "Runtime Installation", "Runtimeconfig.json", "Version Mismatch", "Windows Runtime", "Windows Service"]
tags_normalized: ["dotnet", "dotnet 8", "application failure", "coding", "community", "dependency resolution", "event viewer", "framework dependency", "microsoftdotnetcoredotapp", "microsoftdotwindowsdesktopdotapp", "publishing", "runtime installation", "runtimeconfigdotjson", "version mismatch", "windows runtime", "windows service"]
---

JY_2013 describes a .NET 8 application failing to start as a Windows service due to a missing Microsoft.NETCore.App version, despite WindowsDesktop.App being present. The post explores runtime dependencies and versioning.<!--excerpt_end-->

# .NET 8 Windows Service Fails to Start Due to .NETCore.App Version Mismatch

**Issue Summary**

A .NET 8 Windows service fails to start, logging the following error in the Windows Event Viewer:

> You must install or update .NET to run this application. ... Framework: 'Microsoft.NETCore.App', version '8.0.21' (x64) ... The following frameworks were found: 8.0.20 at [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]

## Environment and Details

- **Application Target:** .NET 8.0 (runtimeconfig.json specifies `net8.0` with dependencies on Microsoft.NETCore.App and Microsoft.WindowsDesktop.App)
- **System Runtimes Installed:**
  - Microsoft.AspNetCore.App 8.0.21
  - Microsoft.NETCore.App 8.0.20
  - Microsoft.WindowsDesktop.App 8.0.20
- **Observed Problem:**
  - The application demands Microsoft.NETCore.App 8.0.21 but only 8.0.20 is available, resulting in failure to start.
  - User expected WindowsDesktop.App 8.0.20 to include sufficient runtime support.

## Why This Happens

- **.NET Runtime Structure:**
  - `Microsoft.WindowsDesktop.App` _depends on_ `Microsoft.NETCore.App` (i.e., Desktop runtime is an extension but does _not_ supersede or satisfy Core runtime).
  - The version of Microsoft.NETCore.App required by your app (8.0.21) was not satisfied by the 8.0.20 on your system.
  - Even if WindowsDesktop.App runtime is present, the corresponding Core runtime version _must_ be installed for proper execution.
  - Desktop runtime versions and Core runtime versions are released in parallel but not bundled together for upgrades.

- **Version Matching:**
  - If your app was built with or requires .NETCore.App 8.0.21, having only 8.0.20 installed will not satisfy the requirement. .NET follows strict runtime version resolution (especially for patch/security versions above minor releases).
  - WindowsDesktop.App 8.0.20 contains only its own managed DLLs and dependencies. You still need Microsoft.NETCore.App 8.0.21 present _specifically_.

- **Learn More:**
  - [Microsoft Docs: .NET and Desktop Runtime dependencies](https://learn.microsoft.com/en-us/dotnet/core/install/windows)

## How to Fix

1. **Install Microsoft.NETCore.App 8.0.21:**
   - Download from: [Official .NET Download Page](https://aka.ms/dotnet-core-applaunch?framework=Microsoft.NETCore.App&framework_version=8.0.21&arch=x64&rid=win-x64&os=win10)
   - Running `dotnet --list-runtimes` after installation should show Microsoft.NETCore.App 8.0.21 is present.

2. **General Recommendations:**
   - Ensure all required runtimes listed in your app's runtimeconfig.json are present at the required version.
   - Consider self-contained deployment if targeting environments with unknown runtime versions.

3. **Additional Troubleshooting:**
   - If issues persist, verify the application's published artifacts and confirm the runtimeconfig.json matches your expected target frameworks and versions.

## Key Takeaways

- **Microsoft.WindowsDesktop.App _requires_ a matching Microsoft.NETCore.App.**
- **Application will not start with only the Desktop runtime present if the specific Core version is absent.**
- **Always install the exact or higher patch version of each required runtime listed in runtimeconfig.json.**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/net-runtime/net-runtime-issues-application-not-starting-up/m-p/4466585#M773)
