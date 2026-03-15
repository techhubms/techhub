---
external_url: https://techcommunity.microsoft.com/t5/net-runtime/do-i-need-to-upgrade-microsoft-aspnetcore-nuget-packages-after/m-p/4431436#M752
title: 'Upgrading NuGet Packages vs. .NET Runtime: Addressing SCA Vulnerabilities in Microsoft.AspNetCore.*'
author: MahaauD
feed_name: Microsoft Tech Community
date: 2025-07-09 05:01:49 +00:00
tags:
- .NET
- .NET 8
- .NET Runtime
- Authorization
- Best Practices
- Components
- Dependency Management
- Microsoft.AspNetCore
- NuGet Packages
- Package Upgrade
- SCA
- SignalR
- Software Composition Analysis
- Vulnerability Management
- Community
section_names:
- dotnet
primary_section: dotnet
---
MahaauD asks whether upgrading the .NET Runtime alone resolves SCA-detected vulnerabilities for Microsoft.AspNetCore.* NuGet packages, or if manual package updates are also necessary.<!--excerpt_end-->

# Upgrading NuGet Packages vs. .NET Runtime: Resolving SCA Vulnerabilities

MahaauD asks whether upgrading only the .NET Runtime is enough to address vulnerabilities flagged by a Software Composition Analysis (SCA) tool in Microsoft.AspNetCore.* NuGet packages, or if these packages must also be upgraded explicitly.

## Scenario

- **Issue:** SCA scan reports known vulnerabilities in the following NuGet packages at version `8.0.0`:
  - Microsoft.AspNetCore.Authorization
  - Microsoft.AspNetCore.Components
  - Microsoft.AspNetCore.Http.Connections.Client
  - Microsoft.AspNetCore.SignalR.Client
- **Scanner Recommendation:** Upgrade these packages to `8.0.15`.
- **Attempted Solution:** .NET Runtime on the environment was upgraded to `8.0.15`.
- **Result:** Vulnerabilities still reported; package versions untouched.

## Clarifying the Relationship

In .NET, there's a distinction between the **runtime** your application targets and the **NuGet packages** you reference in your project:

- **Upgrading the .NET Runtime** installs a newer version of the runtime **on your machine or server**, but does **not** affect NuGet package versions in your project files (`.csproj`).
- **Referenced NuGet Packages** are included at the versions defined in your project; unless you explicitly update versions with NuGet, these do **not change** even when you upgrade the runtime globally.

## Why Do SCA Scans Still Flag Vulnerabilities?

Your project dependencies (including `Microsoft.AspNetCore.*`) remain at `8.0.0` unless **manually updated**. SCA tools analyze the packages **declared in your project** and their versions, not just your machine's installed runtime. Updating only the runtime does not resolve vulnerabilities in project-scoped package dependencies.

## **What You Should Do**

1. **Explicitly upgrade the NuGet package references in your project** to the recommended secure versions (e.g., `8.0.15`).
   - In Visual Studio, use the NuGet Package Manager UI or run:

     ```sh
     dotnet add package Microsoft.AspNetCore.Authorization --version 8.0.15
     dotnet add package Microsoft.AspNetCore.Components --version 8.0.15
     ...and so on for each flagged package
     ```

2. **Verify** that your `csproj` no longer references the vulnerable versions.
3. **Rebuild and redeploy** your application after updating package references.
4. **Rerun your SCA scan** to confirm vulnerabilities are resolved.

## Summary Table

| Action                       | Effect on NuGet References |
|------------------------------|----------------------------|
| Upgrade .NET Runtime only    | No effect, packages unchanged |
| Upgrade NuGet package version| References updated (fixes vulnerabilities) |

## Takeaways

- Always update both the runtime **and** vulnerable NuGet packages.
- The SCA tool validates package versions used in your build, not just your deployment environment.

---
**References**

- [Microsoft Docs: How package versioning works](https://learn.microsoft.com/en-us/nuget/concepts/package-versioning)
- [NuGet Package Manager CLI Reference](https://learn.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference)

---
If you have further questions about dependency management or vulnerability scanning, feel free to ask for more details.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/net-runtime/do-i-need-to-upgrade-microsoft-aspnetcore-nuget-packages-after/m-p/4431436#M752)
