---
title: Stop juggling package managers—just run `azd update`
external_url: https://devblogs.microsoft.com/azure-sdk/azd-update/
author: Kristen Womack
feed_name: Microsoft Azure SDK Blog
date: 2026-04-14 19:24:08 +00:00
section_names:
- azure
- devops
primary_section: azure
tags:
- Azd
- Azd Update
- Azd Version
- Azure
- Azure Developer CLI
- Azure SDK
- Chocolatey
- CLI Tooling
- Daily Channel
- Developer Tooling
- DevOps
- GitHub Issues
- GitHub Pull Requests
- Homebrew
- Insiders Builds
- Linux
- Macos
- News
- Release Channels
- Stable Channel
- Windows
- Winget
---

Kristen Womack explains how `azd update` simplifies keeping the Azure Developer CLI current across Windows, macOS, and Linux, including how to switch between stable and daily release channels.<!--excerpt_end-->

## Stop juggling package managers—just run `azd update`

Updating `azd` used to mean remembering which package manager you installed it with. Now one command handles it on every platform.

## What’s new?

`azd update` updates the Azure Developer CLI (azd) to the latest version.

- Works on **Windows, macOS, and Linux**
- Works regardless of how `azd` was originally installed:
  - winget
  - Chocolatey
  - Homebrew
  - install script

## Why it matters

When the “A new version of azd is available” prompt appears, it’s easy to postpone updating and forget—then later hit a bug that was already fixed in a newer release.

If you don’t remember whether you installed via winget, Homebrew, or a curl script, upgrading becomes a time-wasting detour. `azd update` removes that friction by making updates consistent across platforms.

## How to use it

### Update to the latest stable release

```bash
azd update
```

### Switch release channels

Use `--channel` to move between stable and daily (insiders) builds.

```bash
azd update --channel daily
```

```bash
azd update --channel stable
```

## Try it out

- `azd update` is available starting with **azd 1.23.x**.
- Check your installed version:

```bash
azd version
```

If you’re on an older version:

1. Do one last manual update using your original installation method.
2. After that, use `azd update` going forward.

Fresh install instructions: Install azd (https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd)

For a deeper dive, see Jon Gallant’s post: azd update—Stop Juggling Package Managers (https://blog.jongallant.com/2026/04/azd-update)

## Feedback

- File an issue or start a discussion on GitHub: https://github.com/Azure/azure-dev
- Sign up for user research: https://aka.ms/azd-user-research-signup

This feature was introduced in PR #6942 (https://github.com/Azure/azure-dev/pull/6942), based on Issue #6673 (https://github.com/Azure/azure-dev/issues/6673).


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/azd-update/)

