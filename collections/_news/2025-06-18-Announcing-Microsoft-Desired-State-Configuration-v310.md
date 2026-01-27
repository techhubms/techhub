---
external_url: https://devblogs.microsoft.com/powershell/announcing-dsc-v3-1-0/
title: Announcing Microsoft Desired State Configuration v3.1.0
author: Jason Helmick
feed_name: Microsoft PowerShell Blog
date: 2025-06-18 14:12:40 +00:00
tags:
- Automation
- Configuration Management
- Cross Platform
- Desired State Configuration
- DSC
- IaC
- Linux
- PowerShell
- PowerShell DSC
- Resource Authoring
- Windows
- WinGet
section_names:
- coding
- devops
primary_section: coding
---
Authored by Jason Helmick, this post announces the general availability of Microsoft Desired State Configuration (DSC) v3.1.0, outlining its new features, improvements, and installation instructions for different platforms.<!--excerpt_end-->

## Announcing Microsoft Desired State Configuration v3.1.0

**Author:** Jason Helmick

Microsoft is pleased to announce the General Availability of Desired State Configuration (DSC) version 3.1.0. This release marks a significant milestone in providing cloud-native configuration management for cross-platform environments. DSC is a declarative configuration and orchestration platform designed to define and maintain application and service settings reliably across Windows, Linux, and macOS.

### Overview of DSC v3.1.0

DSC v3.1.0 is the result of close collaboration with partners and the DSC community, with notable contributions and support from the Windows Package Manager (WinGet) team. The platform standardizes configuration management and expands integration scenarios for IT professionals and developers.

#### References to Previous Releases

- [DSC v3.0.0 Announcement](https://devblogs.microsoft.com/powershell/announcing-dsc-v3/)
- [DSC v3.0.0 Get Started](https://devblogs.microsoft.com/powershell/get-started-with-dsc-v3/)
- [DSC v3.0.0 Enhanced Authoring](https://devblogs.microsoft.com/powershell/enhanced-authoring-with-dsc-v3/)

### What's New in DSC v3.1.0

This release focuses on delivering practical features, stability improvements, and new capabilities, responding directly to feedback from real-world users, partners, and community contributors. Noteworthy improvements include:

#### WinGet and Partner-Driven Enhancements

- Enhanced core infrastructure to enable seamless DSC-based management with WinGet.
- Expanded resource invocation APIs, supporting deeper integration by third-party tools.
- Better flexibility for configuration refresh intervals and reporting, as requested by partners.

#### Resource Authoring Improvements

- Improved validation and handling for resource schema files, including clearer, actionable error messages.
- Resolved module loading and path resolution issues for PSDSC resources.
- Stronger support for resources with both required and optional properties.

#### Cross-Platform Reliability and Bug Fixes

- Resolved several Linux-specific bugs related to resource execution, state detection, and error reporting.
- Improved compatibility for Windows, especially in heterogeneous (mixed-OS) environments.
- Addressed inconsistencies in handling **ensure** properties and desired state evaluations.

#### Performance and Quality

- Optimized detection of configuration drift, resulting in faster, more reliable test operations.
- Reduced likelihood of configurations remaining in unclear or failed states.
- Enhanced error handling for edge cases in `set`, `test`, and `get` operations.

#### Diagnostics and Usability

- Expanded logging and diagnostics features, simplifying resource troubleshooting and configuration activity tracking.
- Improved clarity and relevance of error and warning messages on all supported platforms.
- More consistent reporting of results for both interactive and automated operations.

A comprehensive list of changes is available in the [DSC v3.1 changelog](https://github.com/PowerShell/DSC/blob/main/CHANGELOG.md).

### How to Install DSC v3.1.0

#### On Windows

You can install DSC from the Microsoft Store using `winget`, which provides automatic updates.

- Search for DSC:

  ```powershell
  winget search DesiredStateConfiguration --source msstore
  ```

- Install the latest stable release:

  ```powershell
  winget install --id 9NVTPZWRC6KQ --source msstore
  ```

- Install the latest preview release (optional):

  ```powershell
  winget install --id 9PCX3HX4HZ0Z --source msstore
  ```

#### On Linux and macOS

1. Download the latest DSC release from the [PowerShell/DSC GitHub repository](https://github.com/PowerShell/DSC/releases).
2. Extract the release archive.
3. Add the extracted folder to your `PATH` environment variable.

### Support Lifecycle

- DSC uses [semantic versioning](https://semver.org/), with stable releases (e.g., 3.1.0) receiving critical bug and security patches for three months after the next stable version's release.
- Users are encouraged to stay current by updating to the latest patch version when possible.

### Additional Resources and Feedback

- Refer to the [official DSC documentation](https://learn.microsoft.com/powershell/dsc/overview?view=dsc-3.0) for more details on usage and features.
- The DSC team welcomes feedback; visit the [PowerShell/DSC GitHub repository](https://github.com/PowerShell/DSC) to report issues or contribute.

---
Jason Helmick
Sr. Product Manager, PowerShell

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/announcing-dsc-v3-1-0/)
