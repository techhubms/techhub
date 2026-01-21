---
external_url: https://www.youtube.com/watch?v=AEbJzTF03F0
title: .NET Framework 4.8 to .NET 9 Step by Step Migration Guide
author: dotnet
feed_name: DotNet YouTube
date: 2025-11-14 03:30:06 +00:00
tags:
- .NET
- .NET 9
- .NET Conf
- .NET Framework 4.8
- .NET Standard
- AI
- AI Assistance
- ASP.NET
- Dual Compilation
- Legacy Application
- Microsoft.Extensions
- Migration
- Modernization
- MVC
- Upgrade Process
- Web API
- Windows Service
section_names:
- coding
---
dotnet explains how their team successfully migrated a 12-year-old .NET Framework application to .NET 9, outlining each critical step and the technical considerations involved.<!--excerpt_end-->

{% youtube AEbJzTF03F0 %}

# .NET Framework 4.8 to .NET 9 Step by Step Migration Guide

This session presents a comprehensive strategy for upgrading an aging .NET Framework ASP.NET and Windows Service application (over 12 years in production) directly to .NET 9 without a complete rewrite. The process maintains the ability to ship software throughout the transition, balancing risk and modernization goals.

## Key Migration Steps

1. **Pre-Upgrade Modernization**
    - Use **.NET Standard-compatible Microsoft.Extensions.\*** packages in the .NET Framework application to enable gradual migration.
2. **Library Conversion**
    - Convert existing supporting libraries to **.NET Standard** where feasible.
    - For legacy dependencies, create a consolidated library that remains .NET Framework-specific.
3. **Greenfield Projects in .NET 8+**
    - Any new projects or major new features are authored in .NET 8 or later, provided dependencies support .NET Standard.
4. **Dual Compilation Techniques**
    - Employ dual compilation for components that must support both .NET Framework and .NET Standard 2.1, handling edge cases effectively.
5. **Modernizing ASP.NET Admin Site**
    - The admin site (based on MVC/WebAPI) is converted—a process that benefited from AI coding assistance tools for faster, more reliable migration.
6. **Final Project Conversion**
    - Remaining projects are iteratively upgraded, with careful planning to mitigate risk and avoid downtime.

## Lessons Learned

- Continuous shipping during modernization reduces business risk.
- Incremental upgrades using .NET Standard can ease migration of large, complex codebases.
- AI-assisted refactoring (for example, during the admin site update) can accelerate transition while maintaining quality.

## Additional Resources

- Session video: [Watch on YouTube](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXtIlvq1tuORUtZqVG-HdCt)
- Explore more .NET Conf 2025 sessions for further migration and modernization insights.

---
