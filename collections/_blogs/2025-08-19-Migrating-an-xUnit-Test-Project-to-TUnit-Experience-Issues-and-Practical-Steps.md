---
external_url: https://andrewlock.net/converting-an-xunit-project-to-tunit/
title: 'Migrating an xUnit Test Project to TUnit: Experience, Issues, and Practical Steps'
author: Andrew Lock
feed_name: Andrew Lock's Blog
date: 2025-08-19 10:00:00 +00:00
tags:
- .NET
- .NET Core
- .NET Standard
- CI/CD
- FluentAssertions
- IDE Support
- Microsoft.Testing.Platform
- MSTest
- NativeAOT
- Nuke Build System
- NUnit
- Snapshot Testing
- Source Generators
- Test Automation
- Test Migration
- Testing
- Testing Frameworks
- TRX Test Reports
- TUnit
- Verify
- xUnit
section_names:
- coding
---
Andrew Lock shares his experience porting a .NET library to TUnit, a next-gen C# testing framework, explaining benefits versus xUnit, migration steps, and practical challenges faced.<!--excerpt_end-->

# Migrating an xUnit Test Project to TUnit: Experience, Issues, and Practical Steps

**Author:** Andrew Lock

In this post, I discuss the new [TUnit testing framework](https://github.com/thomhurst/TUnit), the motivation for switching from xUnit, and some related migration challenges.

## What is TUnit?

TUnit is a modern C# testing framework leveraging source-generated tests, parallel execution by default, and Native AOT support. Built atop Microsoft's Testing Platform, it promises fast test runs, enhanced developer experience, and flexible architecture.

Notably, TUnit’s API is mostly stable (as of early 2024), and its documentation offers ample guides for onboarding and migration. IDEs like Visual Studio, VS Code, and Rider support TUnit, with some requiring explicit enablement.

One of TUnit’s key selling points is speed: source-generation and NativeAOT make for much quicker test discovery and execution. Benchmarks in TUnit’s README reveal substantial speedups over traditional frameworks—sometimes nearly an order of magnitude faster.

## Why Change Test Frameworks?

Switching test frameworks isn’t a decision to make lightly. Differences between frameworks are often superficial, and migration can be tedious without much gain. However, troublesome compatibility issues (e.g., xUnit tests not being discovered on new .NET SDKs) and the limited framework support in xUnit v3 prompted me to reconsider using xUnit for future projects. xUnit v2 is effectively deprecated and stuck on older platforms, while xUnit v3 only supports .NET 8+—not enough for projects supporting a wider matrix.

## How is TUnit Different from xUnit?

- **Async Tests:** TUnit improves parallelization and provides direct control over concurrency.
- **Setup & Tear Down:** Offers `[BeforeTest]` and `[AfterTest]` attributes for async setup/teardown.
- **Assembly Hooks:** TUnit’s `[BeforeAssembly]` and `[AfterAssembly]` for assembly-level setup.
- **TestContext:** Injected context and static properties make tracking per-test state easier.
- **Assertions:** TUnit uses a fluent style familiar from FluentAssertions and Shouldly.

Other notable features:

- **Source generation:** Faster, compile-time test discovery.
- **NativeAOT support:** Ability to publish test projects via AOT.
- **Test ordering:** `[DependsOn]` attribute for flexible test dependencies.
- **Console output capture:** Automatic correlation of output/logs with tests.

Importantly, TUnit supports .NET Standard 2.0 (broader than xUnit v3).

## Step-by-Step: Migrating from xUnit to TUnit

1. **Add TUnit Packages**

   ```bash
   dotnet add package TUnit
   ```

   Include alongside xUnit during initial migration.

2. **Remove Global Usings**
   Disable implicit TUnit usings to avoid namespace clashes:

   ```xml
   <PropertyGroup>
     <TUnitImplicitUsings>false</TUnitImplicitUsings>
     <TUnitAssertionsImplicitUsings>false</TUnitAssertionsImplicitUsings>
   </PropertyGroup>
   ```

3. **Convert xUnit Usages**
   Use TUnit’s analyzer for automated migration:

   ```bash
   dotnet format analyzers --severity info --diagnostics TUXU0001
   ```

   Most facts and attributes are converted; manual cleanup might be needed for complex asserts.

4. **Reinstate Global Usings**
   Re-enable TUnit usings once conversion is complete.

5. **Remove Unneeded Packages**
   Uninstall xUnit and Microsoft.NET.Test.Sdk packages; TUnit runs atop Microsoft.Testing.Platform.

## Real-World Issues

- **Assert Conversion:** Some asserts (e.g., `Assert.Throws`) weren’t handled automatically; resolved using FluentAssertions.
- **TRX Reporting:** Needed special arguments for report generation with the new platform and Nuke build system:

   ```diff
   DotNetTest(s => s
     .SetProjectFile(Solution)
     // ...
   - .SetLoggers("trx")
   - .SetResultsDirectory(TestResultsDirectory)
   + .SetProcessArgumentConfigurator(x=>x
   + .Add("--")
   + .Add("--report-trx")
   + .Add("--results-directory")
   + .Add(TestResultsDirectory)));
   ```

- **Snapshot Testing:** Verify.TUnit only supports .NET 8+, so I forked Verify at an earlier version to support older projects.

## Summary

Migrating from xUnit to TUnit was smooth, with helpful migration tools and guides, and most issues were surmountable. TUnit’s support for a wider range of frameworks, source generation, and speed optimizations make it an appealing alternative. I’ll be keeping a close eye on further development and adoption across my projects.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/converting-an-xunit-project-to-tunit/)
