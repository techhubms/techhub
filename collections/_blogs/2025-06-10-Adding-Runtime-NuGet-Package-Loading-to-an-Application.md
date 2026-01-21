---
external_url: https://weblog.west-wind.com/posts/2025/Jun/09/Adding-Runtime-NuGet-Package-Loading-to-an-Application
title: Adding Runtime NuGet Package Loading to an Application
author: Rick Strahl
feed_name: Rick Strahl's Blog
date: 2025-06-10 04:40:37 +00:00
tags:
- .NET
- .NET ASP.NET
- ASP.NET
- Assembly Loading
- C#
- Dependency Management
- Dynamic Code
- LiveReloadServer
- NuGet
- Razor Pages
- Runtime Extensibility
section_names:
- coding
---
In this article, Rick Strahl discusses how to enable runtime extensibility in .NET apps using NuGet package loading and dynamic assembly integration, with practical code samples and insights from his work on LiveReloadServer.<!--excerpt_end-->

# Adding Runtime NuGet Package Loading to an Application

*Author: Rick Strahl*

## Overview

Rick Strahl provides an in-depth look at adding dynamic code loading to .NET applications, with a focus on integrating NuGet package loading and external assembly support. The use case is exemplified by his [LiveReloadServer](https://github.com/RickStrahl/LiveReloadServer) tool, a local web server that benefits from extensibility through runtime code injection.

## Introduction

Developers occasionally need to add external code to applications without recompiling or redeploying. Traditionally, this meant direct assembly loading or copying DLLs into a known folder. Modern .NET applications, however, can leverage NuGet packages for a more robust and familiar approach. This post covers both assembly loading and dynamic NuGet package integration within LiveReloadServer, especially to extend Razor and Markdown scripting.

## LiveReloadServer: The Runtime Extensible Web Server

LiveReloadServer is a .NET-powered local web server capable of serving static content, supporting Razor Pages (`.cshtml`), and rendering themed Markdown files. It enables quick local development and live reloading, making it suitable for:

- Local client web application development
- Static site authoring and iterative preview
- Hosting several sites from a single server instance (no publish step necessary)

A standout feature is **dynamic Razor Page support**, enabling 'static pages with benefits.' Pages can include Razor code, compiled and executed at runtime, eliminating the need for traditional build-and-publish cycles. This model is reminiscent of classic ASP or simpler dynamic platforms like PHP.

## The Need for Runtime Extensibility

While static and basic dynamic features suffice, there are scenarios where more advanced capabilities are required. Manually adding assemblies to a designated folder was the initial approach in LiveReloadServer. As requirements grew, supporting the loading of external NuGet packages at runtime became essential for flexibility and code reuse.

## Assembly Loading: The Traditional Route

Early versions allowed for loading compiled assemblies from a `PrivateBin` folder. The following code (simplified) illustrates how assemblies are loaded and added to the Razor engine context:

```csharp
private void LoadPrivateBinAssemblies(IMvcBuilder mvcBuilder)
{
    var binPath = Path.Combine(ServerConfig.WebRoot, "privatebin");
    if (Directory.Exists(binPath))
    {
        var files = Directory.GetFiles(binPath);
        foreach (var file in files)
        {
            if (!file.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase)) continue;
            try
            {
                var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                mvcBuilder.AddApplicationPart(asm);
            }
            catch (Exception) { /* error handling omitted */ }
        }
    }
}
```

This approach allows for quick extension by simply dropping DLLs into a known directory and restarting the server as needed.

## NuGet Package Loading: A Familiar and Scalable Solution

Loading assemblies works well for code you compile yourself, but NuGet packages are industry standard for sharing and consuming dependencies. Until recently, integrating NuGet package management into running .NET applications was complex. With .NET 8.0 and the `Nuget.Protocol` library, the process is now manageable:

```ps
dotnet add package Nuget.Protocol
```

### Why NuGet?

- Encapsulates dependencies
- Handles versioning
- Aligns with modern development practices

## Implementing a NuGet Package Loader

### Configuration

Package references are stored in a configuration file (`NuGetPackages.json`):

```json
{
  "Packages": [
    { "packageId": "Westwind.Ai", "version": "0.2.7" },
    { "packageId": "Humanizer.Core", "version": "2.14.1" }
  ],
  "Sources": [ "https://api.nuget.org/v3/index.json" ]
}
```

### Package Loader Logic

At application startup, the loader processes each package definition and downloads both the primary and dependency packages:

```csharp
Task.Run(async () =>
{
    foreach (var package in packageConfiguration.Packages)
    {
        try
        {
            await nuget.LoadPackageAsync(
                package.PackageId, package.Version, mvcBuilder,
                packageConfiguration.NugetSources,
                LoadedNugetPackages, FailedNugetPackages);
        }
        catch { FailedNugetPackages.Add(package.PackageId); }
    }
}).FireAndForget();
```

Code is run asynchronously, suitable even for classic console apps—though refactoring to use full async would be preferable long-term.

#### Core Loader Implementation

The loader uses `Nuget.Protocol` to fetch package streams, resolves dependencies, determines the best target framework, and extracts the relevant assemblies:

```csharp
public async Task LoadPackageAsync(string packageId, string version, IMvcBuilder builder, ...)
{
    // Set up NuGet source repository and cache
    // Retrieve the desired package and its dependencies
    // Extract assemblies for the relevant .NET framework
    // Load assemblies into the application's context
    // Handle errors and logging
}
```

Full code and details are available on [GitHub](https://github.com/RickStrahl/LiveReloadServer/blob/fa1fc7339a0584f8b6980b28e4a25110c9616b54/LiveReloadServer/Startup.cs#L483).

### Directory Layout

Downloaded packages are stored using a hierarchy that organizes by package, version, and framework target. Only the required framework version is extracted to conserve space.

## Considerations and Security

Runtime code loading provides powerful extensibility but introduces potential security implications. LiveReloadServer includes options to disable dynamic loading as needed.

## Benefits Realized

With the inclusion of NuGet package support, any LiveReloadServer instance can now:

- Dynamically integrate third-party or custom-developed packages
- Keep deployments lean (only update/extend as needed)
- Manage dependencies and updates in a familiar way

## Conclusion

Dynamically loading code (via assemblies or NuGet packages) isn't common in typical .NET apps. However, for generic execution tools or scenarios requiring on-the-fly extensibility, it opens new possibilities. The evolution of Microsoft’s NuGet libraries has simplified what previously felt unapproachable. This approach is now practical for tools needing runtime extensibility using industry-standard dependency management.

## Resources

- [LiveReloadServer Source Code on GitHub](https://github.com/RickStrahl/LiveReloadServer)
- [NuGet.Protocol on NuGet](https://www.nuget.org/packages/NuGet.Protocol)

---

*If you found this helpful, consider making a small donation to support the author.*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Jun/09/Adding-Runtime-NuGet-Package-Loading-to-an-Application)
