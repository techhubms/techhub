---
date: 2026-03-18 21:47:31 +00:00
author: Jason Helmick
primary_section: dotnet
tags:
- .NET
- .NET 10
- Argument Completer
- Join Path
- Long Term Support
- LTS
- Native Command Handling
- News
- NO COLOR
- PowerShell
- PowerShell 7.6
- PowerShellStandard.Library
- PSFeedbackProvider
- PSNativeWindowsTildeExpansion
- PSReadLine
- PSRedirectToVariable
- PSResourceGet
- PSSubsystemPluginModel
- Start Process
- Tab Completion
- ThreadJob
- WildcardPattern.Escape
- X509Certificate2
- X509SubjectAlternativeNameExtension
external_url: https://devblogs.microsoft.com/powershell/announcing-powershell-7-6/
section_names:
- dotnet
title: Announcing PowerShell 7.6 (LTS) GA Release
feed_name: Microsoft PowerShell Blog
---

Jason Helmick announces the GA release of PowerShell 7.6 (LTS), highlighting its move to .NET 10 (LTS), reliability improvements, new/updated module features, tab completion enhancements, and a small set of breaking changes relevant to production automation environments.<!--excerpt_end-->

# Announcing PowerShell 7.6 (LTS) GA Release

PowerShell 7.6 is now **generally available** as the next **Long Term Support (LTS)** release. It is built on **.NET 10 (LTS)**, continuing PowerShell’s alignment with the modern .NET platform.

PowerShell 7.6 focuses on reliability improvements across:

- The PowerShell engine
- Core modules
- Interactive shell experience
- Cross-platform behavior consistency

As an LTS release, **PowerShell 7.6 is the recommended version for production automation environments**.

## Highlights

### Core module updates

PowerShell 7.6 includes updates to several core modules:

- **PSReadLine**
- **Microsoft.PowerShell.PSResourceGet**
- **Microsoft.PowerShell.ThreadJob**

### Tab completion improvements

This release includes dozens of improvements to completions, including:

- Improved path completion across providers
- Added value completion for parameters of several cmdlets
- Enabled completions in more contexts and scopes
- Added completion of modules by their shortname

### New and improved command features

- Added `-Delimiter` parameter to `Get-Clipboard`
- Added `Register-ArgumentCompleter -NativeFallback` to support registering a cover-all completer for native commands
- Treat `-Target` as literal in `New-Item`
- Added `-ExcludeModule` parameter to `Get-Command`
- Improved `Start-Process -Wait` polling efficiency

### Engine and platform improvements

- Added `PSForEach()` and `PSWhere()` as aliases for the PowerShell intrinsic methods `ForEach()` and `Where()`
- Make `SystemPolicy` public APIs visible but no-op on Unix platforms so that they can be included in `PowerShellStandard.Library`
- Update `DnsNameList` for `X509Certificate2` to use `X509SubjectAlternativeNameExtension.EnumerateDnsNames()`
- Fixed stderr output of console host to respect the `NO_COLOR` environment variable

### Features promoted to mainstream

The following features have been converted to mainstream features:

- `PSFeedbackProvider`
- `PSNativeWindowsTildeExpansion`
- `PSRedirectToVariable`
- `PSSubsystemPluginModel`

## Breaking changes

PowerShell 7.6 includes a small set of breaking changes aimed at long-term consistency:

- Converted `-ChildPath` parameter to `string[]` for `Join-Path`. This allows passing an array of child paths and avoids extra usage with `-AdditionalChildPath`.
- `WildcardPattern.Escape()` now correctly escapes lone backticks.
- Removed the trailing space from the `GetHelpCommand` trace source name.

## Community contributions

PowerShell is built by a global community of users and contributors. This release includes contributions from many community members (listed in the original announcement), along with thanks to people who filed issues, tested previews, improved docs, and submitted fixes.

## Call to action

Install PowerShell 7.6 and review the detailed docs:

- [Install PowerShell on Windows, Linux, and macOS](https://learn.microsoft.com/powershell/scripting/install/install-powershell)
- [What’s New in PowerShell 7.6](https://learn.microsoft.com/powershell/scripting/whats-new/what-s-new-in-powershell-76)

## Looking ahead

The team notes ongoing work toward PowerShell 7.7 and beyond, and points to:

- [Steve Lee’s recent blog on future plans](https://devblogs.microsoft.com/powershell/powershell-openssh-and-dsc-team-investments-for-2026/)

Preview releases will continue to provide early access to new capabilities and improvements.


[Read the entire article](https://devblogs.microsoft.com/powershell/announcing-powershell-7-6/)

