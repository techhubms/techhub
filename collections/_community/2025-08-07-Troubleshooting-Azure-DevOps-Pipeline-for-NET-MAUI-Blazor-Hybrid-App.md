---
layout: "post"
title: "Troubleshooting Azure DevOps Pipeline for .NET MAUI Blazor Hybrid App"
description: "This community post details a developer's experience and troubleshooting process while setting up an Azure DevOps pipeline for building a .NET MAUI Blazor Hybrid project targeting Windows. It highlights pipeline YAML configuration, issues with missing runtime packages, and feedback on DevOps task selection."
author: "Background_March7229"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/azuredevops/comments/1mjui7y/dotnet_maui_pipeline/"
viewing_mode: "external"
feed_name: "Reddit Azure DevOps"
feed_url: "https://www.reddit.com/r/azuredevops/.rss"
date: 2025-08-07 08:26:59 +00:00
permalink: "/community/2025-08-07-Troubleshooting-Azure-DevOps-Pipeline-for-NET-MAUI-Blazor-Hybrid-App.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET 9", ".NET Workload", "Azure", "Azure DevOps", "Blazor Hybrid", "Build Automation", "CI/CD", "Coding", "Community", "DevOps", "DotNetCoreCLI", "MAUI", "Microsoft.NETCore.App.Runtime.Mono.win X64", "NuGet", "Pipeline", "Pipeline Troubleshooting", "SDK Installation", "Windows Desktop", "YAML"]
tags_normalized: ["dotnet 9", "dotnet workload", "azure", "azure devops", "blazor hybrid", "build automation", "cislashcd", "coding", "community", "devops", "dotnetcorecli", "maui", "microsoftdotnetcoredotappdotruntimedotmonodotwin x64", "nuget", "pipeline", "pipeline troubleshooting", "sdk installation", "windows desktop", "yaml"]
---

Background_March7229 seeks help configuring an Azure DevOps pipeline for a .NET MAUI Blazor Hybrid app, describing errors encountered with runtime packages and pipeline tasks.<!--excerpt_end-->

# Troubleshooting Azure DevOps Pipeline for .NET MAUI Blazor Hybrid App

**Author:** Background_March7229

## Scenario

The author attempts to build a simple .NET MAUI Blazor Hybrid project for Windows using Azure DevOps. The project is new with no modifications and is pushed directly to Azure DevOps.

## Issue

During pipeline execution, the following error is encountered:

> Unable to find package Microsoft.NETCore.App.Runtime.Mono.win-x64 with version

This occurs during the restore or publish phase when using .NET 9 and targeting `win-x64`.

## Pipeline Configuration (YAML)

```yaml
trigger:
  branches:
    include:
      - main

pool:
  vmImage: 'windows-latest'

variables:
  buildConfiguration: 'Release'
  runtime: 'win-x64'
  dotnetVersion: '9.0.101'
  projectPath: 'MyApp/MyApp.csproj'
  targetFramework: 'net9.0-windows10.0.19041.0'
  outputDir: '$(Build.ArtifactStagingDirectory)/publish'

steps:
  - task: UseDotNet@2
    displayName: 'Install .NET 9.0.303 SDK'
    inputs:
      packageType: 'sdk'
      version: '$(dotnetVersion)'
      includePreviewVersions: true

  - task: NuGetToolInstaller@1

  - task: PowerShell@2
    displayName: 'Install .NET MAUI Workload'
    inputs:
      targetType: 'inline'
      script: |
        dotnet workload install maui

  - task: NuGetCommand@2
    inputs:
      restoreSolution: 'MyApp/MyApp.sln'

  - task: DotNetCoreCLI@2
    displayName: 'Publish MAUI App (.NET 9)'
    inputs:
      command: 'publish'
      projects: '$(projectPath)'
      arguments: >
        -c $(buildConfiguration) -f $(targetFramework) -r $(runtime) --self-contained true -p:PublishSingleFile=true -p:WindowsPackageType=None -o $(outputDir)
```

### Additional Feedback

- A community member suggests using `DotNetCoreCLI@2` with a restore command instead of NuGet restore for .NET projects.
- The author reports receiving the same error after updating the pipeline to use `DotNetCoreCLI@2` for restore.

## Observations

- The error suggests the .NET MAUI workload or required runtime packs for the targeted platform/version might be unavailable or missing from the feed used by the pipeline.
- Using preview SDKs with MAUI requires careful matching of workloads, SDK versions, and platform support.
- The build process may require additional setup to enable preview features or install specific runtime packs.

## Suggestions for Similar Scenarios

- Ensure the specified `dotnetVersion` and the targeted framework are properly supported on the chosen build agent.
- Confirm that the MAUI workload installs the correct version and all platform-specific packs.
- If using a preview SDK, add steps/flags to enable preview features or consider pinning to a stable release if available.
- Use `DotNetCoreCLI@2` for both restore and build/publish over `NuGetCommand@2` to ensure best compatibility with modern .NET workloads.
- Check that `Microsoft.NETCore.App.Runtime.Mono.win-x64` is available for the selected SDK/runtime versions or adjust the runtime/platform target as needed.

## References

- [Azure DevOps: Build and publish a .NET MAUI app](https://learn.microsoft.com/dotnet/maui/windows/deployment/publish-cli)
- [.NET MAUI workload installation](https://learn.microsoft.com/dotnet/maui/fundamentals/workloads)

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1mjui7y/dotnet_maui_pipeline/)
