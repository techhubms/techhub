---
external_url: https://andrewlock.net/exploring-dotnet-10-preview-features-5-running-one-off-dotnet-tools-with-dnx/
title: 'Running One-Off .NET Tools with dnx: Exploring the .NET 10 Preview'
author: Andrew Lock
feed_name: Andrew Lock's Blog
date: 2025-07-29 10:00:00 +00:00
tags:
- .NET 10
- .NET Core
- .NET SDK
- .NET Tool
- ASP.NET Core
- Command Line
- Dnx
- Global Tools
- Local Tools
- NuGet
- Tool Execution
section_names:
- coding
---
In this post, Andrew Lock explores the new 'dnx' command in .NET 10, demonstrating how developers can run .NET tools without installing them. He provides both practical usage examples and an under-the-hood look at the SDK implementation.<!--excerpt_end-->

## Running One-Off .NET Tools with dnx in .NET 10

by Andrew Lock

.NET 10 preview 6 introduces a significant quality-of-life feature: the `dnx` command for executing .NET tools directly, without requiring a permanent installation. Andrew Lock reviews how this command works, its advantages, and the implementation under the hood.

### Why `dnx`?

Previously, .NET developers could install tools globally or locally using `dotnet tool install`. Unlike Node.js's `npx`, the .NET experience required explicit installation. With `dnx`, .NET 10 makes on-the-fly, one-shot execution of tooling possible. This draws on familiar patterns from ecosystems like Node.js and Go, especially the use of version specifiers like `[email protected]`.

#### Command Syntax

The new command can be invoked with:

```bash
dnx --help
```

**Description**: Executes a tool from source without permanently installing it.

**Usage**:

```
dotnet dnx <packageId> [<commandArguments>...] [options]
```

Where `<PACKAGE_ID>` can be `packageId` or `[email protected]` (e.g., `dotnetsay`, `dotnetsay@1.1.0`).

**Options** include specifying a version, accepting all prompts, running interactively, including prerelease packages, configuring sources, and more. The command’s help information details all arguments and options, mirroring much of the existing `dotnet tool` command syntax.

#### Running a Demo Tool

Demonstrating its usage, Andrew Lock runs the demo package `dotnetsay` with `dnx`:

```bash
> dnx dotnetsay
Tool package [email protected] will be downloaded from source https://api.nuget.org/v3/index.json. Proceed? [y/n] (y): y

Welcome to using a .NET Core global tool!
```

For subsequent executions, confirmation is skipped, and the tool runs immediately:

```bash
> dnx dotnetsay
Welcome to using a .NET Core global tool!
```

This streamlines running one-off tools on demand.

### dnx vs dotnet tool install

The classic method of using `.NET tools` involves:

```bash
dotnet tool install -g dotnetsay
```

This installs the tool globally with an executable shim placed on your PATH, and the package managed in the tool store (`~/.dotnet/tools/.store`). You then run `dotnetsay` directly. In contrast, `dnx` installs nothing permanently—tool packages are downloaded to the global NuGet cache and run directly from there. There’s no added executable or persistent configuration: it’s a true “one shot.”

### How Does dnx Work Internally?

Delving under the hood, Andrew describes how `dnx` is implemented. The `dnx` command itself is a simple shell or batch file—on Windows (`dnx.cmd`) or Linux/MacOS (shell script)—that invokes `dotnet dnx` with the provided arguments.

Within the .NET SDK, the [DnxCommandParser](https://github.com/dotnet/sdk/blob/681138b2d3d7255a17ad6cb4812787a0d5edef99/src/Cli/dotnet/Commands/Dnx/DnxCommandParser.cs#L10) parses the command, and the [ToolExecuteCommand](https://github.com/dotnet/sdk/blob/main/src/Cli/dotnet/Commands/Tool/Execute/ToolExecuteCommand.cs#L42) handles the logic:

1. If no version is provided, it checks for the package in a local tools manifest (`dotnet-tools.json`), running it from there if found.
2. Otherwise, it looks up the package (from nuget.org or user-specified sources).
3. If the tool isn’t present, user confirmation is requested before download (unless `--yes` is provided to auto-confirm).
4. After download, the tool runs from the global cache.

This mechanism leverages existing NuGet infrastructure but bypasses persistent installation or shims.

### Summary

The `dnx` command in .NET 10 brings .NET tool experience closer to ecosystems like Node.js, letting developers run tools conveniently without installation overhead. Andrew Lock demonstrates usage, explains the difference from existing approaches, and explores implementation details within the SDK.

---

**Stay updated with Andrew Lock’s posts at .Net Escapades.**

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-dotnet-10-preview-features-5-running-one-off-dotnet-tools-with-dnx/)
