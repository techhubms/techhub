---
layout: "post"
title: "Disabling Localized Satellite Assemblies During .NET Publish"
description: "Andrew Lock demonstrates how to reduce .NET app publish size by excluding unnecessary localization satellite assemblies. This guide includes explanations of localization resources, configuring .NET projects to control which cultures are included, and practical troubleshooting tips."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/disabling-localized-satellite-assemblies-during-dotnet-publish/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-02-25 09:00:00 +00:00
permalink: "/2025-02-25-Disabling-Localized-Satellite-Assemblies-During-NET-Publish.html"
categories: ["Coding"]
tags: [".NET", "Coding", "Configuration", "Globalization", "Hosting", "InvariantGlobalization", "Localization", "MSBuild", "Posts", "Publish Output", "Satellite Assemblies", "SatelliteResourceLanguages", "System.CommandLine"]
tags_normalized: ["net", "coding", "configuration", "globalization", "hosting", "invariantglobalization", "localization", "msbuild", "posts", "publish output", "satellite assemblies", "satelliteresourcelanguages", "system dot commandline"]
---

In this post, Andrew Lock explores how .NET developers can prevent unnecessary localization satellite assemblies from being included in publish outputs to reduce app size.<!--excerpt_end-->

# Disabling Localized Satellite Assemblies During .NET Publish

*Author: Andrew Lock*

## Introduction

Publishing .NET applications often results in additional localization satellite assemblies being included in the output. These assemblies, while useful for supporting multiple cultures, can significantly increase the size of the deployment, especially when localization isn’t needed for your intended use-case.

This post explains how .NET satellite assemblies work, discusses scenarios in which you may not need them, and provides concrete steps for excluding them from your published output.

---

## What Are Localization Satellite Assemblies?

Localization enables applications to adapt to diverse cultures by supporting different languages, currencies, number formats, and other culture-specific elements. .NET accomplishes this with satellite resource assemblies: additional DLL files named after the parent assembly with a `.resources` suffix, stored in a folder matching the target culture.

For example, the [System.CommandLine](https://www.nuget.org/packages/System.CommandLine/2.0.0-beta4.22272.1) package contains:

- `System.CommandLine.dll` in the root folder
- `System.CommandLine.resources.dll` in various `culture` subfolders

![The folder structure for System.CommandLine](/content/images/2025/satellites_01.png)

When your project references such a package, all resource assembly folders may be copied into your build output, increasing app size. In System.CommandLine, localization resources can add up to ~260KB while the main DLL is ~205KB. If every dependency had a similar footprint, app size could double.

---

## Do You Need Localized Resources?

While localization improves user experience and meets explicit requirements in some scenarios, it may not be necessary in all cases. For example, web apps targeting a single culture—or the [invariant culture](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-globalization-cultureinfo-invariantculture)—do not need extra resource DLLs.

Including unnecessary satellite assemblies increases your deployment package for no benefit.

---

## Attempt 1: Globalization Invariant Mode

One might assume enabling [globalization invariant mode](https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md) via MSBuild would prevent copying these assemblies, e.g.,

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <InvariantGlobalization>true</InvariantGlobalization>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="System.CommandLine" Version="2.0.0-beta4.22272.1" />
  </ItemGroup>
</Project>
```

However, this only changes runtime behavior; it does **not** affect build or publish output. Satellite assemblies are still copied.

---

## Solution: `<SatelliteResourceLanguages>` Property

The .NET Core 2.1 SDK introduced the `<SatelliteResourceLanguages>` MSBuild property. By setting this in your `.csproj`, you control which cultures' satellite resource assemblies are included during build and publish.

- To only include Spanish (`es`), add:

  ```xml
  <SatelliteResourceLanguages>es</SatelliteResourceLanguages>
  ```

- For multiple cultures (e.g., Spanish and French):

  ```xml
  <SatelliteResourceLanguages>es;fr</SatelliteResourceLanguages>
  ```

- To exclude **all** satellites (useful for invariant culture or English-only):

  ```xml
  <SatelliteResourceLanguages>en</SatelliteResourceLanguages>
  ```

Each of these will ensure only the specified culture(s) are copied, reducing your published size:

![Only es has been copied to the build output](/content/images/2025/satellites_02.png)

![Only es and fr have been copied to the build output](/content/images/2025/satellites_03.png)

![None of the assemblies have been copied to the build output](/content/images/2025/satellites_04.png)

---

## Caveats and Troubleshooting

- **Add to the correct project**: If you use packages with resources in class libraries, the `<SatelliteResourceLanguages>` must be in each library that references such packages—not only in the final application project. Use `Directory.Build.props` to propagate across multiple projects if needed.
- **Exact culture names**: Specify cultures as they appear in the output (`es`, `de`, `pt-BR`), not as broader prefixes (`pt`, `zh`), or else they won't be included. No error is thrown if you misspell or use a culture not present.

---

## Summary

By setting `<SatelliteResourceLanguages>` in your project or build props, you can:

- Drastically reduce your .NET publish size
- Include only specific culture resources required for your application
- Improve performance and efficiency, especially for single-culture or invariant culture deployments

## Reference

- [Satellite Assemblies Documentation](https://learn.microsoft.com/en-us/dotnet/core/dependency-loading/loading-resources#when-are-satellite-assemblies-loaded)
- [Globalization Invariant Mode](https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md)
- [System.CommandLine Package](https://www.nuget.org/packages/System.CommandLine/2.0.0-beta4.22272.1)

---

*Andrew Lock | .Net Escapades*

Stay up to date with the latest posts!

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/disabling-localized-satellite-assemblies-during-dotnet-publish/)
