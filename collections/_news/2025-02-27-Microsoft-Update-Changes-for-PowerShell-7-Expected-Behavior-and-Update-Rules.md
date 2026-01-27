---
external_url: https://devblogs.microsoft.com/powershell/microsoft-update-changes-for-powershell-7/
title: 'Microsoft Update Changes for PowerShell 7: Expected Behavior and Update Rules'
author: Steven Bucher
feed_name: Microsoft PowerShell Blog
date: 2025-02-27 22:28:17 +00:00
tags:
- Automation
- LTS
- Microsoft Update
- PowerShell
- PowerShell Installation
- Preview Releases
- SCCM
- Stable Releases
- Update Process
- Versioning
- Windows Server Update Services
- Windows Updates
- WSUS
section_names:
- coding
primary_section: coding
---
In this blog post, Steven Bucher from the PowerShell Team explains important changes to how Microsoft Update handles PowerShell 7 updates, including expected behaviors for LTS, stable, and preview versions.<!--excerpt_end-->

# Microsoft Update (MU) Changes for PowerShell 7

**Author:** Steven Bucher, PowerShell Team

## Introduction

Steven Bucher discusses recent changes and clarifies the behaviors of Microsoft Update (MU) as it relates to PowerShell 7 installations. This post aims to help users understand how updates are managed for different PowerShell release channels (LTS, stable, preview), and offers guidance on configuration and further resources.

## About Microsoft Update

Microsoft Update (MU) provides automatic update services for supported Microsoft products. Starting from PowerShell 7.2, MU can deliver updates for PowerShell 7 to ensure consistent, timely rollout across environments. This method allows IT admins to:

- Control the update schedule.
- Test updates in staging environments.
- Manage enterprise-scale rollouts efficiently.

### Enabling MU During Installation

When installing PowerShell via MSI, two key checkboxes configure its update behavior:

1. **Enable updates for PowerShell through Microsoft Update or WSUS (recommended):**
   - Allows delivery of PowerShell 7 updates via Microsoft Update, Windows Server Update Services (WSUS), or System Center Configuration Manager (SCCM).

2. **Enable Microsoft Update when I check for updates (recommended):**
   - Enables the system to receive updates for all supported Microsoft software (not just Windows).

For best results, it's recommended to select both options. For command-line installations, refer to the [PowerShell documentation](https://learn.microsoft.com/powershell/scripting/install/installing-powershell-on-windows#install-the-msi-package-from-the-command-line) for specific instructions.

### Availability and Timing

- Updates can take up to two weeks to become available through MU after a new version releases on GitHub.
- Although updates are targeted to be published within two weeks post-GitHub release, delays can occur.
- For immediate needs, direct downloads are available at the [PowerShell Releases GitHub page](https://github.com/PowerShell/PowerShell/releases/).

## Update Rules and Expected Behavior

MU update rules are designed to keep users on their preferred release type (LTS, stable, preview). Upgrade scenarios include:

- **LTS (e.g., 7.4):**
  - Receives updates only for the same major LTS version (e.g., updated within 7.4.x branch).
- **Stable (e.g., 7.5):**
  - Receives updates only for the same stable version stream (e.g., updates to new 7.5.x releases).
- **Preview or RC versions:**
  - Receive updates only to the next available preview version, not to stable releases.

### Special Considerations

- LTS versions will **not** upgrade to newer stable versions (e.g., 7.4 will not automatically update to 7.5).
- Stable releases can automatically update to a higher LTS release if support ends for the stable version.
- LTS versions are only upgraded to a different LTS (e.g., from 7.4 to 7.6) **after** the old LTS goes out of support.
- Preview versions are updated only to newer preview releases—even when new stable versions are available.

**Example:**

- If you have 7.5-rc.1, you will be offered 7.6-preview.2 (not 7.5 stable).

### Upcoming Change

- Beginning **March 14, 2025**, users on PowerShell 7.2 will be automatically updated to 7.4.

## Helpful Links

- [Microsoft Update FAQ](https://learn.microsoft.com/powershell/scripting/install/microsoft-update-faq)
- [PowerShell GitHub Releases](https://github.com/PowerShell/PowerShell/releases/)
- [PowerShell Support Lifecycle](https://learn.microsoft.com/powershell/scripting/install/powershell-support-lifecycle)

## Feedback and Contact

Feedback is welcome! To share feedback or suggestions, use [GitHub Issues](https://github.com/PowerShell/PowerShell/issues/).

*Thank you from Steven Bucher, PM on the PowerShell Team.*

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/microsoft-update-changes-for-powershell-7/)
