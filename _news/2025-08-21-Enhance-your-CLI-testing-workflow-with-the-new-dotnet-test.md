---
layout: "post"
title: "Enhance your CLI testing workflow with the new dotnet test"
description: "This article explores improvements in .NET 10 for CLI-based testing, focusing on the native integration of Microsoft.Testing.Platform (MTP) within the dotnet test command. It covers key differences between VSTest and MTP, performance upgrades, enhanced diagnostics, simplified configuration, practical migration tips, and command-line options for managing and running tests across platforms and environments."
author: "Mariam Abdullah"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/dotnet-test-with-mtp/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-08-21 17:05:00 +00:00
permalink: "/2025-08-21-Enhance-your-CLI-testing-workflow-with-the-new-dotnet-test.html"
categories: ["Coding", "DevOps"]
tags: [".NET", ".NET 10", ".NET Test", "C#", "CLI Testing", "Coding", "Continuous Integration", "DevOps", "F#", "Microsoft.Testing.Platform", "MSBuild", "MTP", "News", "Performance Optimization", "Test Automation", "Test Diagnostics", "Test Runner", "Testing", "Testing Frameworks", "Visual Basic", "VSTest"]
tags_normalized: ["dotnet", "dotnet 10", "dotnet test", "csharp", "cli testing", "coding", "continuous integration", "devops", "fsharp", "microsoftdottestingdotplatform", "msbuild", "mtp", "news", "performance optimization", "test automation", "test diagnostics", "test runner", "testing", "testing frameworks", "visual basic", "vstest"]
---

Mariam Abdullah introduces the new features of dotnet test in .NET 10, highlighting Microsoft.Testing.Platform (MTP) integration for faster performance and improved CLI diagnostics.<!--excerpt_end-->

# Enhance your CLI Testing Workflow with the New dotnet test

**Author:** Mariam Abdullah

The new .NET 10 update brings significant enhancements to the `dotnet test` command, targeting developers who need reliable test execution directly from the CLI. With the native integration of Microsoft.Testing.Platform (MTP), developers and DevOps engineers benefit from faster, more robust, and easier-to-diagnose test runs.

## Key Improvements in .NET 10

- **Native MTP Integration:**
  - MTP is now directly supported in `dotnet test`, replacing earlier reliance on VSTest.
  - Built-in configuration via a solution- or repository-level `dotnet.config` file.

- **Performance & Diagnostics:**
  - Faster test startup and discovery, especially noticeable in large solutions.
  - Structured logging and detailed diagnostics for troubleshooting.
  - Improved output formatting using ANSI terminal standards.

- **Streamlined Migration:**
  - Clear migration path from VSTest to MTP.
  - Legacy MSBuild properties for test configuration are deprecated.

## Usage Patterns and Examples

- **Basic Test Execution:**

  ```bash
  dotnet test
  ```

- **Target Specific Project or Solution:**

  ```bash
  dotnet test --project ./Path/To/Project.csproj
  dotnet test --solution ./Path/To/Solution.sln
  ```

- **Advanced Filtering:**
  - Leverage `--test-modules` for granular assembly selection and globbing patterns.

- **Parallel Execution:**

  ```bash
  dotnet test --max-parallel-test-modules 4
  ```

- **Output Control:**

  ```bash
  dotnet test --no-ansi
  dotnet test --no-progress
  dotnet test --output Detailed
  ```

- **MSBuild Integration Examples:**

  ```bash
  dotnet test -p:DefineConstants="DEV"
  dotnet test --property:Configuration=Release
  ```

## Migration & Compatibility Notes

- All test projects in a solution must migrate to MTP if enabling it via `dotnet.config`—mixed VSTest and MTP environments are not supported.
- Remove obsolete properties like `TestingPlatformDotnetTestSupport` and `TestingPlatformShowTestsFailure` during migration.

## Benefits for Devs and DevOps Teams

- Unified, future-oriented architecture for .NET testing
- Portable solution for local, CI pipeline, and editor-based workflows
- Better performance and enhanced diagnosis in continuous integration scenarios

For complete details, setup guidance, and community discussions, see the official [dotnet test documentation](https://learn.microsoft.com/dotnet/core/tools/dotnet-test?tabs=dotnet-test-with-mtp) and the [Microsoft.Testing.Platform overview](https://aka.ms/mtp-overview).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-test-with-mtp/)
