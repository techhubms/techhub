---
layout: "post"
title: "The new Dependabot NuGet updater: 65% faster with native .NET"
description: "This article details the major update to Dependabot’s NuGet updater, now leveraging native .NET tooling for improved performance, reliability, and accuracy. It explains key architectural changes, enhanced dependency analysis, better support for .NET project features, and benefits for developer productivity and security."
author: "Jamie Magee, Brett Forsgren"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/the-new-dependabot-nuget-updater/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-08-04 15:00:00 +00:00
permalink: "/2025-08-04-The-new-Dependabot-NuGet-updater-65-faster-with-native-NET.html"
categories: ["Coding", "DevOps", "Security"]
tags: [".NET", "Azure Artifacts", "Central Package Management", "Coding", "Conflict Resolution", "Dependabot", "Dependency Update", "DevOps", "GitHub Packages", "Global.json", "MSBuild", "News", "NuGet", "Package Management", "Performance", "Security", "Transitive Dependencies"]
tags_normalized: ["dotnet", "azure artifacts", "central package management", "coding", "conflict resolution", "dependabot", "dependency update", "devops", "github packages", "globaldotjson", "msbuild", "news", "nuget", "package management", "performance", "security", "transitive dependencies"]
---

Jamie Magee and Brett Forsgren share how Dependabot’s revamped NuGet updater uses .NET’s own tooling to bring faster, more reliable, and more secure dependency updates to developers.<!--excerpt_end-->

# The new Dependabot NuGet updater: 65% faster with native .NET

*By Jamie Magee, Brett Forsgren*

If you’ve ever waited impatiently for Dependabot to update your .NET dependencies—or worse, watched it fail with cryptic errors—this article brings good news. The Dependabot team has refactored the NuGet updater, yielding significant performance, reliability, and developer experience improvements.

## From Hybrid to Native Tooling

The previous updater relied on a hybrid solution, using manual XML parsing and string replacements in Ruby. This was functional for simple cases, but fell short in more complex .NET project scenarios. The new updater fully embraces native .NET tooling by using:

- [NuGet client libraries](https://learn.microsoft.com/nuget/reference/nuget-client-sdk) for package operations
- [MSBuild APIs](https://learn.microsoft.com/visualstudio/msbuild/msbuild-api) for project evaluation and dependency resolution
- [.NET CLI](https://learn.microsoft.com/dotnet/core/tools/) for restores

By aligning with the official .NET toolchain, the updater now behaves like the tools developers already use, eliminating discrepancies between CI updates and local workflows.

## Performance and Reliability Improvements

- **Speed**: Automated test suites that once took 26 minutes now complete in 9 minutes—a 65% reduction in runtime.
- **Success Rate**: Update success rates have improved from 82% to 94%.

This translates to quicker update cycles and fewer manual interventions for developers.

## Improved Dependency Detection with MSBuild

Previously, dependency detection used fragile XML parsing, missing many advanced scenarios. The new updater uses MSBuild’s project evaluation engine, handling:

- Conditional package references (e.g., based on TargetFramework or build configuration)
- [`Directory.Build.props` and `Directory.Build.targets`](https://learn.microsoft.com/visualstudio/msbuild/customize-by-directory)
- MSBuild variables and hierarchical property evaluation
- Complex package reference patterns

**Example of improved detection:**

```xml
<ItemGroup Condition="'$(TargetFramework)' == 'net8.0'">
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.0" />
</ItemGroup>
```

These advanced patterns were often missed or misinterpreted by the old updater.

## Sophisticated Dependency Resolution

The updated engine takes a comprehensive approach to dependency conflicts and security vulnerabilities:

### Transitive Dependency Updates

When a project is affected by a vulnerable transitive dependency, the updater finds a resolution path—either by updating a parent package or explicitly adding a direct reference (using NuGet’s [direct dependency wins](https://learn.microsoft.com/nuget/concepts/dependency-resolution#direct-dependency-wins) rule):

```plaintext
YourApp
└── PackageA v1.0.0
    └── TransitivePackage v2.0.0 (CVE-2024-12345)
```

Resolution steps:

1. Attempt to update `PackageA` to a version with fixed transitive dependencies.
2. If unavailable, add a direct reference to a secure version of `TransitivePackage`.

```xml
<PackageReference Include="PackageA" Version="1.0.0" />
<PackageReference Include="TransitivePackage" Version="3.0.0" />
```

### Related Package Updates

When updating one package could cause version mismatches in a package family (e.g., `Microsoft.Extensions.*`), the updater now automatically updates all related packages for compatibility.

## Respecting `global.json`

The updater now respects `.NET SDK` versions specified in [`global.json`](https://learn.microsoft.com/dotnet/core/tools/global-json), ensuring updates are evaluated against the same SDK as the developer and CI environments. This works with recent Dependabot features that update SDK versions as needed.

## Full Support for Central Package Management (CPM)

[Central Package Management (CPM)](https://learn.microsoft.com/nuget/consume-packages/central-package-management) is fully supported:

- Detection and updates of `Directory.Packages.props` files
- Support for package overrides and transitive dependencies

This means CPM-based .NET solutions benefit from the same automation and reliability improvements.

## Universal NuGet Feed Support

Dependabot can now update from any [NuGet v2 or v3-compliant feed](https://learn.microsoft.com/nuget/api/overview)—including nuget.org, Azure Artifacts, and GitHub Packages—while respecting authentication, feed-specific behaviors, and [package source mapping](https://learn.microsoft.com/nuget/consume-packages/package-source-mapping).

## What This Means for Developers

If you’re using Dependabot for .NET projects:

- Updates run faster.
- Fewer failed updates.
- More accurate handling of complex dependency graphs.
- Clearer and more actionable error messages.
- No changes are required to your existing [`dependabot.yml`](https://docs.github.com/code-security/dependabot/dependabot-version-updates/configuration-options-for-the-dependabot.yml-file) configuration.

## Looking Forward

The new architecture, written in C# with native .NET APIs, is also a platform for future improvements—integration with new .NET features, better enterprise support, clearer diagnostic tooling, and easier community contributions.

## Getting Started

The new updater is live for all .NET repositories on GitHub. No action is needed to benefit, but here’s a minimal sample config if you’re starting fresh:

```yaml
version: 2
updates:
  - package-ecosystem: "nuget"
    directory: "/"
    schedule:
      interval: "weekly"
```

If you’re already using Dependabot, you should already see the speed, reliability, and clarity improvements in your dependency update PRs.

---

*The development of this native updater demonstrates how modern dependency management can be fast, robust, and seamlessly integrated with the .NET developer experience.*

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/the-new-dependabot-nuget-updater/)
