---
layout: "post"
title: ".NET 10.0.5 Out-of-Band Release: Debugger Crash Fix for macOS with Visual Studio Code"
description: "This article announces the .NET 10.0.5 out-of-band update, which addresses a critical debugger crash affecting macOS developers using Visual Studio Code. The issue, introduced in .NET 10.0.4, caused debug sessions to crash, particularly on ARM64 Macs. Guidance is provided for affected developers, along with relevant links to release notes and downloads."
author: "Rahul Bhandari (MSFT)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/dotnet-10-0-5-oob-release-macos-debugger-fix/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2026-03-12 17:00:00 +00:00
permalink: "/2026-03-12-NET-1005-Out-of-Band-Release-Debugger-Crash-Fix-for-macOS-with-Visual-Studio-Code.html"
categories: ["Coding"]
tags: [".NET", ".NET 10", ".NET 10.0.5", ".NET Maintenance", ".NET SDK", "Apple Silicon", "ARM64", "Bug Fix", "Coding", "Cross Platform Development", "Debugger", "Debugging", "Developer Tools", "Macos", "Maintenance & Updates", "News", "OOB", "Out Of Band Update", "Release Notes", "VS Code"]
tags_normalized: ["dotnet", "dotnet 10", "dotnet 10dot0dot5", "dotnet maintenance", "dotnet sdk", "apple silicon", "arm64", "bug fix", "coding", "cross platform development", "debugger", "debugging", "developer tools", "macos", "maintenance and updates", "news", "oob", "out of band update", "release notes", "vs code"]
---

Rahul Bhandari (MSFT) details the .NET 10.0.5 out-of-band update, which fixes a debugger regression for macOS developers using Visual Studio Code.<!--excerpt_end-->

# .NET 10.0.5 Out-of-Band Release – macOS Debugger Fix

## Overview

Microsoft has released .NET 10.0.5 as an out-of-band (OOB) update to resolve a critical regression impacting macOS users. This bug, introduced in .NET 10.0.4, causes the debugger to crash when developers use Visual Studio Code to debug .NET applications, particularly on Apple Silicon (ARM64) Macs.

## Who is Affected?

- Developers running **macOS** (especially ARM64/Apple Silicon)
- Those using **Visual Studio Code** for .NET development and debugging
- Users who have installed **.NET SDK 10.0.104, 10.0.200, or .NET 10.0.4 runtime**

Other platforms like Windows and Linux are not affected by this issue.

## How to Fix the Issue

**For macOS users with Visual Studio Code:**

1. [Download and install the .NET 10.0.5 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
2. Restart Visual Studio Code
3. Run `dotnet --version` in your terminal to verify the installation

**For Other Platforms:**

- You can continue to use .NET 10.0.4 if desired. The 10.0.5 update is specifically for this macOS-related regression and does not include additional fixes.

## Additional Resources

- [Release Notes – 10.0.5](https://github.com/dotnet/core/blob/main/release-notes/10.0/10.0.5/10.0.5.md)
- [Installers and Binaries](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Container Images](https://mcr.microsoft.com/catalog?search=dotnet/)
- [Linux Packages](https://github.com/dotnet/core/blob/main/release-notes/10.0/install-linux.md)
- [Known Issues](https://github.com/dotnet/core/blob/main/release-notes/10.0/known-issues.md)

## Feedback

If you encounter issues after this update, report them in the [Release feedback issue](https://github.com/dotnet/core/issues/10292).

Thank you to the .NET developer community for your patience as Microsoft resolved this problem.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-10-0-5-oob-release-macos-debugger-fix/)
