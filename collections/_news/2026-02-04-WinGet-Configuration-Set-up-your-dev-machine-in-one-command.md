---
layout: "post"
title: "WinGet Configuration: Set up your dev machine in one command"
description: "This article by Kayla Cinnamon outlines how WinGet Configuration simplifies Windows development machine setup, making the process fast, repeatable, and customizable. It compares WinGet Configuration to import/export features, demonstrates YAML-based setup, explains idempotency, covers settings management, and details how to use GitHub Copilot CLI for generating and managing these configurations. The post provides example configuration files, practical tips, and highlights for individual and team-based setups."
author: "Kayla Cinnamon"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/blog/winget-configuration-set-up-your-dev-machine-in-one-command"
viewing_mode: "external"
feed_name: "Microsoft Blog"
feed_url: "https://devblogs.microsoft.com/feed"
date: 2026-02-04 18:00:13 +00:00
permalink: "/2026-02-04-WinGet-Configuration-Set-up-your-dev-machine-in-one-command.html"
categories: ["Coding", "DevOps", "GitHub Copilot"]
tags: [".NET", "Automation", "Azure Developer CLI", "Coding", "Desired State Configuration", "Developer Environment", "Developer Productivity", "DevOps", "Environment Setup", "Git", "GitHub Copilot", "GitHub Copilot CLI", "Microsoft For Developers", "News", "Node.js", "Package Management", "PowerShell", "VS", "VS Code", "VS Workloads", "Windows Package Manager", "Windows Terminal", "WinGet", "WinGet Configuration", "YAML"]
tags_normalized: ["dotnet", "automation", "azure developer cli", "coding", "desired state configuration", "developer environment", "developer productivity", "devops", "environment setup", "git", "github copilot", "github copilot cli", "microsoft for developers", "news", "nodedotjs", "package management", "powershell", "vs", "vs code", "vs workloads", "windows package manager", "windows terminal", "winget", "winget configuration", "yaml"]
---

Kayla Cinnamon explains how WinGet Configuration and GitHub Copilot CLI can streamline and automate the setup of Windows development environments, providing practical steps for developers to get started quickly and consistently.<!--excerpt_end-->

# WinGet Configuration: Set up your dev machine in one command

Kayla Cinnamon shares her experience setting up numerous development machines and introduces WinGet Configuration as a solution for automating and streamlining the process.

## What is WinGet Configuration?

WinGet Configuration allows developers to define their ideal development environments in YAML files. With a single command, all necessary tools and settings are installed and applied, eliminating manual steps.

- **Idempotent operations**: Running the config multiple times only applies missing items
- **Automated consent**: Use `--accept-configuration-agreements` for unattended setups

## Getting Started

1. **Install WinGet DSC module**:

   ```powershell
   Install-Module Microsoft.WinGet.DSC -Force
   ```

2. **Run a configuration**:

   ```powershell
   winget configure -f configuration.winget
   ```

## Import/Export vs Configure

While `winget import/export` handles app installation in bulk by exporting/importing lists of packages, `winget configure` extends to Windows settings, developer modes, VS workloads, environment variables, dependencies, and OS version checks.

| Feature                   | import/export | configure |
|---------------------------|:------------:|:---------:|
| Install packages          |      ✅      |     ✅    |
| Windows settings          |      ❌      |     ✅    |
| Enable Developer Mode     |      ❌      |     ✅    |
| Install VS workloads      |      ❌      |     ✅    |
| Set environment variables |      ❌      |     ✅    |
| Define dependencies       |      ❌      |     ✅    |
| Check OS requirements     |      ❌      |     ✅    |
| Run PowerShell DSC        |      ❌      |     ✅    |

`winget import` is for simple package syncing; `winget configure` is for full environment provisioning.

## Sample Configuration File

Example YAML for a basic setup that includes VS Code, Git, Node.js, and Windows Terminal Preview:

```yaml
# yaml-language-server: $schema=https://aka.ms/configuration-dsc-schema/0.2

properties:
  configurationVersion: 0.2.0
resources:
- resource: Microsoft.WinGet.DSC/WinGetPackage
  directives:
    description: Install Visual Studio Code Insiders
    securityContext: elevated
  settings:
    id: Microsoft.VisualStudioCode.Insiders
    source: winget
- resource: Microsoft.WinGet.DSC/WinGetPackage
  directives:
    description: Install Git
    securityContext: elevated
  settings:
    id: Git.Git
    source: winget
- resource: Microsoft.WinGet.DSC/WinGetPackage
  directives:
    description: Install Node.js LTS
    securityContext: elevated
  settings:
    id: OpenJS.NodeJS.LTS
    source: winget
- resource: Microsoft.WinGet.DSC/WinGetPackage
  directives:
    description: Install Windows Terminal Preview
  settings:
    id: Microsoft.WindowsTerminal.Preview
    source: winget
```

To apply:

```powershell
winget configure -f dev-setup.winget
```

## Adding Windows Settings

You can configure Windows itself, like enabling Developer Mode and dark mode:

```yaml
- resource: Microsoft.Windows.Settings/WindowsSettings
  directives:
    description: Enable Developer Mode
    allowPrerelease: true
    securityContext: elevated
  settings:
    DeveloperMode: true
- resource: Microsoft.Windows.Developer/EnableDarkMode
  directives:
    description: Enable dark mode
    allowPrerelease: true
  settings:
    Ensure: Present
    RestartExplorer: true
```

## Validating Requirements and Dependencies

- Use **assertions** to require certain OS versions before running your setup
- Use **dependsOn** to order installations (e.g., ensure Visual Studio installs before workloads)

## Using GitHub Copilot CLI for Config Generation

Install GitHub Copilot CLI as part of your configuration. Once installed, it can:

- Generate new WinGet configuration files based on natural language prompts
- Retrieve package IDs
- Convert traditional scripts to config files
- Explain what a configuration file does

Example prompt:
> "Create a winget configuration file for a Python data science developer including Python 3.12, VS Code, Git, and Anaconda."

Copilot CLI accelerates building and maintaining environment configs.

## Exporting Current Setup

You can export your computer's current app/config state for backups or onboarding:

```powershell
winget configure export -o my-machine.winget --all
winget configure export -o vscode.winget --package-id Microsoft.VisualStudioCode
```

## Version Control and Team Setups

Store configs in your source repositories at `.config/configuration.winget`. New contributors can instantly match your environment:

```powershell
winget configure -f .config/configuration.winget
```

## Example: Personal Dev Setup

Kayla’s own config installs essentials like PowerShell 7, Oh My Posh, Visual Studio, .NET SDK, Azure Developer CLI, Git, Node.js, and GitHub Copilot CLI, among others, plus enables Developer Mode and dark mode.

## Conclusion

WinGet Configuration, combined with GitHub Copilot CLI, makes setting up Windows development machines faster, consistent, and reproducible. The configuration-as-code approach benefits both solo developers and teams.

For further discussion, configuration sharing, and questions, connect with Kayla on Bluesky or X.

This post appeared first on "Microsoft Blog". [Read the entire article here](https://devblogs.microsoft.com/blog/winget-configuration-set-up-your-dev-machine-in-one-command)
