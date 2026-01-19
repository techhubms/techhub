---
external_url: https://andrewlock.net/exploring-the-dotnet-boot-process-via-host-tracing/
title: Exploring the .NET Boot Process via Host Tracing
author: Andrew Lock
viewing_mode: external
feed_name: Andrew Lock's Blog
date: 2025-11-25 10:00:00 +00:00
tags:
- .NET
- .NET 10
- .NET CLI
- .NET Core
- .NET Muxer
- Apphost
- BCL
- Context Switches
- CoreCLR
- Dependency Resolution
- Deps.json
- Diagnostics
- Environment Variables
- Global.json
- Hostfxr
- Hostpolicy.dll
- Logging
- Runtimeconfig.json
- SDK
- TPA
- Trusted Platform Assemblies
section_names:
- coding
---
Andrew Lock provides a technical walkthrough exploring .NET's boot process using host tracing, offering developers step-by-step insights into muxer, hostfxr, and hostpolicy.dll components.<!--excerpt_end-->

# Exploring the .NET Boot Process via Host Tracing

Enabling host tracing is a powerful way to diagnose issues with .NET application startup. Andrew Lock explains how to activate this feature and leverage detailed logs to understand the chain of processes—from running a simple console app, to execution via the dotnet muxer, to hostfxr's runtime selection, and hostpolicy.dll's role in loading assemblies and launching CoreCLR.

## 1. Enabling Host Tracing

Set the following environment variables to enable tracing:

- `COREHOST_TRACE=1` (activates tracing and logs to stderr by default)
- `COREHOST_TRACEFILE=<file_path>` (redirects logs to a file)
- For .NET 10+, you can also point to a directory; the trace will be saved by exe and PID.
- Adjust verbosity using `COREHOST_TRACE_VERBOSITY=<level>` (1 = errors, 4 = verbose)

Example PowerShell:

```powershell
$env:COREHOST_TRACE=1
$env:COREHOST_TRACEFILE="host_trace.log"
dotnet bin\Debug\net9.0\MyApp.dll
```

## 2. .NET Boot Process Components

### dotnet muxer

- Entry point for .NET commands (build, publish, run).
- Located at `C:\Program Files\dotnet\dotnet.exe`.
- Determines if you're running an SDK command or an app.
- Loads the latest `hostfxr` version based on SemVer from `.../host/fxr` folder.

### hostfxr

- Parses arguments to distinguish between SDK commands and app execution.
- Locates SDK via `global.json` and roll-forward policy, or resolves app runtime from `runtimeconfig.json`.
- Finds and loads `hostpolicy.dll` for chosen runtime and framework.

### hostpolicy

- Compiles the list of Trusted Platform Assemblies (TPA) from framework and app deps.json files.
- Sets up process context (environment properties, probing directories, config properties).
- Loads and launches the .NET runtime via `coreclr.dll`.

## 3. Reading the Host Tracing Logs

Tracing logs show how components select versions, validate configs, and load libraries. Example traces help diagnose issues like missing runtime versions, incorrect SDK selection, or asset loading errors.

Sample log excerpt (boot chain):

```
Invoked dotnet [version: 10.0.0-rc.2...]
Considering fxr version=[10.0.0-rc.2...]
Resolved fxr [hostfxr.dll]... Loaded library...
Invoking fx resolver...
Probing for global.json... Resolving SDK...
App runtimeconfig.json found, version established...
Roll forward enabled...
Chose FX version [Microsoft.NETCore.App\9.0.10]
Loaded library from [hostpolicy.dll]
Processing package...
Adding tpa entry: myapp.dll, System.Private.CoreLib.dll...
Loaded library from [coreclr.dll]
Launch host: dotnet.exe, app: myapp.dll
```

## 4. Diagnosing Boot Issues

- Use tracing output to see why an app is loading an unexpected .NET version.
- Validate existence and correctness of runtimeconfig.json and deps.json.
- Check roll-forward policies to troubleshoot version mismatches.
- Confirm that all required assemblies and native assets are present and loaded.

## 5. Summary

Host tracing demystifies the .NET app launch sequence, exposing hidden details about runtime and SDK selection, dependency loading, and execution context setup. This approach is invaluable for diagnosing startup problems and understanding the internals of the .NET host.

---

*Author: Andrew Lock*

For further reading about internals, see Steve Gordon's posts on the [dotnet muxer](https://www.stevejgordon.co.uk/a-brief-introduction-to-the-dotnet-muxer) and [hostfxr library](https://www.stevejgordon.co.uk/how-dotnet-muxer-resolves-and-loads-the-hostfxr-library).

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-the-dotnet-boot-process-via-host-tracing/)
