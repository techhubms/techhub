---
external_url: https://devblogs.microsoft.com/dotnet/dotnet-framework-3-5-moves-to-standalone-deployment-in-new-versions-of-windows/
title: .NET Framework 3.5 Now Requires Standalone Deployment on New Windows Versions
author: Tara Overfield, Brett Lopez
primary_section: dotnet
feed_name: Microsoft .NET Blog
date: 2026-02-05 18:00:00 +00:00
tags:
- .NET
- .NET Framework
- .NET Framework 3.5
- Compatibility
- End Of Support
- Installer
- Legacy Applications
- Microsoft Learn
- Migration Planning
- News
- Standalone Deployment
- Support Lifecycle
- Windows 11
- Windows Deployment
- Windows Insider Preview
section_names:
- dotnet
---
Tara Overfield and Brett Lopez announce that .NET Framework 3.5 will be distributed as a standalone installer in new versions of Windows, sharing implications for developers and migration recommendations.<!--excerpt_end-->

# .NET Framework 3.5 Moves to Standalone Deployment in New Versions of Windows

## Overview

Microsoft has announced a change to how .NET Framework 3.5 is deployed on upcoming Windows releases. Starting with Windows 11 Insider Preview Build 27965, .NET Framework 3.5 is no longer included as an optional Windows component. Moving forward, it must be obtained separately using a standalone installer.

## Key Details

- **Affected Platforms:** This change affects Windows 11 Insider Preview Build 27965 and all future Windows platform releases.
- **Legacy Support:** .NET Framework 3.5 remains available as a standalone installer for legacy applications that require it.
- **No Impact for Existing Systems:** Windows 10 and earlier Windows 11 releases (through 25H2) are not affected by this change.
- **Lifecycle Note:** .NET Framework 3.5 is approaching end of support on January 9, 2029.
- **Migration Guidance:** Developers are advised to start planning migration of applications to newer, supported versions of .NET.

## What Developers Need to Do

- **Obtain the Standalone Installer**: For applications needing .NET Framework 3.5 on new Windows versions, download and redistribute the standalone installer.
- **Plan for Migration:** Review your application dependencies and begin upgrading to modern .NET versions before .NET Framework 3.5 support ends.
- **Consult Official Guidance:** Microsoft's [official documentation](https://go.microsoft.com/fwlink/?linkid=2348700) on Microsoft Learn provides detailed instructions, compatibility notes, and recommended migration paths.

## Additional Resources

- [Microsoft Learn migration and deployment guidance](https://go.microsoft.com/fwlink/?linkid=2348700)
- [Official .NET Blog announcement](https://devblogs.microsoft.com/dotnet/dotnet-framework-3-5-moves-to-standalone-deployment-in-new-versions-of-windows/)

## Summary

This change in deployment policy is part of .NET Framework 3.5 approaching its end of support, and Microsoft recommends that all developers and IT professionals plan accordingly to ensure continued application compatibility.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-framework-3-5-moves-to-standalone-deployment-in-new-versions-of-windows/)
