---
layout: post
title: Get Started with Microsoft Desired State Configuration v3.0.0
author: Jason Helmick
canonical_url: https://devblogs.microsoft.com/powershell/get-started-with-dsc-v3/
viewing_mode: external
feed_name: Microsoft PowerShell Blog
feed_url: https://devblogs.microsoft.com/powershell/feed/
date: 2025-03-12 17:59:51 +00:00
permalink: /coding/news/Get-Started-with-Microsoft-Desired-State-Configuration-v300
tags:
- Automation
- CLI
- Configuration Management
- Cross Platform
- Desired State Configuration
- DSC
- Dsc Command
- IaC
- IIS
- PowerShell
- PowerShell DSC
- PSDSC
- Resource Adapters
- System Configuration
- V3.0.0
- Windows Server
- YAML
section_names:
- coding
- devops
---
Jason Helmick walks through the installation and basics of Microsoft Desired State Configuration v3.0.0, helping IT professionals and developers automate infrastructure management using PowerShell.<!--excerpt_end-->

# Get Started with Microsoft Desired State Configuration v3.0.0

*By Jason Helmick*

Microsoft Desired State Configuration (DSC) v3.0.0 is a modern, cross-platform configuration management framework used by administrators and developers to declaratively define and enforce system states. DSC simplifies automation for both infrastructure and application deployment using configuration as code principles.

## Key Terminology

- **DSC**: Desired State Configuration v3.0.0
- **PSDSC**: PowerShell Desired State Configuration (earlier versions v1.1, v2)

## Installing DSC

- **On Windows**: Install DSC from the Microsoft Store using `winget`, which enables automatic updates.

  ```powershell
  winget search DesiredStateConfiguration
  winget install --id <insert-package-id> --source msstore
  ```

- **On Linux/macOS**:
  1. Download the latest release from [PowerShell/DSC releases](https://github.com/PowerShell/DSC/releases).
  2. Expand the release archive.
  3. Add the expanded archive folder to your `PATH` environment variable.

## Getting Started with the `dsc` Command

- Display command help:

  ```powershell
  dsc --help
  ```

- Key options include tracing and output format settings, plus subcommands for managing configurations and resources (e.g., `config`, `resource`, `schema`).
- Check version:

  ```powershell
  dsc --version
  # Output: dsc 3.0.0
  ```

- [dsc command reference documentation](https://learn.microsoft.com/powershell/dsc/reference/cli/dsc?view=dsc-3.0&preserveView=true)

## Accessing DSC Resources

- List all installed DSC resources:

  ```powershell
  dsc resource list
  ```

- Use adapters (e.g., classic PowerShell resources via `Microsoft.Windows/WindowsPowerShell`):

  ```powershell
  dsc resource list --adapter Microsoft.Windows/WindowsPowerShell
  ```

- [dsc resource command reference documentation](https://learn.microsoft.com/powershell/dsc/reference/cli/resource/command?view=dsc-3.0&preserveView=true)

## Managing Basic Configurations

- Example YAML configuration to install IIS on Windows Server (using classic PowerShell resource `WindowsFeature`):

  ```yaml
  $schema: https://raw.githubusercontent.com/PowerShell/DSC/main/schemas/2024/04/config/document.json
  resources:
  - name: Use Windows PowerShell resources
    type: Microsoft.Windows/WindowsPowerShell
    properties:
      resources:
        - name: Web server install
          type: PSDesiredStateConfiguration/WindowsFeature
          properties:
            Name: Web-Server
            Ensure: Present
  ```

- Apply configuration with:

  ```powershell
  dsc config get --file ./web.comfig.dsc.yaml
  ```

- [dsc config command reference documentation](https://learn.microsoft.com/powershell/dsc/reference/cli/config/command?view=dsc-3.0&preserveView=true)

## Next Steps

- Learn more about authoring and enhancements in DSC v3 through [enhanced authoring documentation](https://devblogs.microsoft.com/powershell/enhanced-authoring-with-dsc-v3/)
- [Official DSCv3 documentation](https://learn.microsoft.com/powershell/dsc/overview?view=dsc-3.0&preserveView=true)
- Participate by providing feedback on the [PowerShell DSC GitHub repository](https://github.com/PowerShell/DSC)

---

*Sr. Product Manager, PowerShell*

Jason Helmick

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/get-started-with-dsc-v3/)
