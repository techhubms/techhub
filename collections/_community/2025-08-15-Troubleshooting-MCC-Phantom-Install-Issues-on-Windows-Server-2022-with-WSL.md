---
external_url: https://techcommunity.microsoft.com/t5/microsoft-connected-cache-for/mcc-phantom-install/m-p/4444201#M108
title: Troubleshooting MCC Phantom Install Issues on Windows Server 2022 with WSL
author: jjmehren
feed_name: Microsoft Tech Community
date: 2025-08-15 13:49:08 +00:00
tags:
- Azure Container Registry
- Container Deployment
- Gmsa
- Install Troubleshooting
- Log Diagnostics
- MCC
- Microsoft Connected Cache
- Port Proxy
- PowerShell
- Symlink Configuration
- TLS Infrastructure
- Ubuntu 24.04
- VHDX
- Windows Server
- Windows Subsystem For Linux
- WSL
section_names:
- azure
- devops
---
jjmehren shares a step-by-step installation log and troubleshooting notes for deploying Microsoft Connected Cache on Windows Server 2022 using WSL and Ubuntu, highlighting success indicators and points of confusion.<!--excerpt_end-->

# MCC Phantom Install on Windows Server 2022 with WSL

**Author:** jjmehren

## Overview

This post documents the process and associated logs for installing Microsoft Connected Cache (MCC) on a Windows Server 2022 host, utilizing WSL (Windows Subsystem for Linux) and Ubuntu 24.04. Despite reaching a "success" state according to installation tasks and logs, running `wsl --list` returns no installed distributions. The post provides stepwise details for each major installation milestone, hoping to isolate causes of the apparent phantom install.

## Environment

- **Host OS:** Windows Server 2022
- **Linux Distro:** Ubuntu 24.04 (imported as VHDX)
- **WSL Status:** Fully updated (WSL, kernel)

## Installation Steps and Diagnostic Output

### 1. Initialization

- MCC installer invoked via scheduled task.
- VHDX file existence/size verified: `c:\mccwsl02\Ubuntu-24.04-Mcc-Converted.vhdx`
- VHDX accessible for import: **size** 132,074,438,656 bytes.

### 2. Image Import

- Image import claimed successful: MCC Ubuntu image imported to WSL.
- Validation step shows WSL Ubuntu image detected and running, with IP recorded as `System.Object[]` (suggests serialization or logging issue).

### 3. Port Proxy Setup

- Port 80 forwarding to WSL instance completed with `netsh interface portproxy`.

### 4. Command Execution & Container Management

- Install commands run inside Ubuntu WSL Distro
- MCC container (pulled from Azure Container Registry):
  - `msconnectedcacheprod1.azurecr.io/mcc/linux/iot/mcc-ubuntu-iot-amd64:2.0.0.2112_e`
  - Container startup detected, confirmed with status code 200.

### 5. Directory, Symlink, and gMSA Account Handling

- Certificates and log directories created at `c:\mccwsl02\Certificates` and subfolders.
- Symlinks set inside the WSL instance: `windowsCerts -> /mnt/c/mccwsl02/Certificates`
- Config file verification successful. All steps reported as OK.
- WSL distribution checks reference:
  - `Ubuntu-24.04-Mcc-Base`
  - `Ubuntu-24.04-Mcc`
- All properties for TLS and symlink infrastructure report as True/Success.

### 6. Finalization & Cleanup

- Last validation steps and PowerShell transcript confirm successful install, no error messages.
- **However:** `wsl --list` returns no distributions, and the original issue remains unresolved.

## Additional Diagnostics

- The `wslip.txt` file only outputs: `System.Object[] COMPLETED` (not human-readable).
- All other logs appear clean, suggesting either a registration or visibility issue inside WSL's distribution table.

## Recommendations

- Re-run `wsl.exe --list --verbose` to confirm current WSL state.
- Check `%LOCALAPPDATA%\Packages` for the Ubuntu/MCC distro, and review logon context.
- Try manual import via `wsl --import` if necessary.
- Validate that the gMSA account used has sufficient permissions and is properly mapped.
- Investigate custom installation parameters or scheduled task constraints.

## Useful Resources

- [Microsoft Connected Cache Documentation](https://learn.microsoft.com/en-us/microsoft-365/solutions/microsoft-connected-cache-overview)
- [WSL Troubleshooting Guide](https://learn.microsoft.com/en-us/windows/wsl/troubleshooting)

---

By tracing each phase from VHDX verification through symlink and container setup, this log aids troubleshooting situations where the installer claims success but `wsl --list` yields no evident distributions. Double-check import logs, permission boundaries, and installation contexts for further resolution.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-connected-cache-for/mcc-phantom-install/m-p/4444201#M108)
