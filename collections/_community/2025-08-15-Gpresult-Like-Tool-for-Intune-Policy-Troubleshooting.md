---
external_url: https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/gpresult-like-tool-for-intune/ba-p/4437008
title: Gpresult-Like Tool for Intune Policy Troubleshooting
author: jonasoh
feed_name: Microsoft Tech Community
date: 2025-08-15 07:50:53 +00:00
tags:
- Administrative Scripts
- Defender
- Device Management
- EnterpriseDesktopAppManagement
- Get MDMPolicyReport
- HTML Reporting
- Intune Management Extension
- IntuneDebug
- LAPS
- MDM
- MDMDiagnosticsTool
- MDMDiagReport
- Microsoft Intune
- Policy Troubleshooting
- PolicyScope
- PowerShell
- Registry
- Win32Apps
- Windows
section_names:
- azure
- coding
- security
primary_section: coding
---
Jonas Ohmsen presents a PowerShell module called IntuneDebug aimed at simplifying Microsoft Intune policy troubleshooting for Windows devices, with step-by-step usage details and reporting techniques.<!--excerpt_end-->

# Gpresult-Like Tool for Intune Policy Troubleshooting

**Author: Jonas Ohmsen**

Intune administrators often need to understand which device and user policies are active and how they are applied. Jonas Ohmsen introduces the **IntuneDebug** PowerShell module, which provides a familiar HTML report experience (similar to `GPresult /h` from the Group Policy world) but tailored for Microsoft Intune and modern management.

## Module Overview

The IntuneDebug module allows admins to:

- Generate an HTML report summarizing Intune MDM policies for any Windows device
- Parse policy data from local or remote MDM diagnostic reports
- View both device and user policy details, Win32Apps installations, script deployments, and more

## Installation

Install from the PowerShell Gallery:

```
Install-Module -Name IntuneDebug
```

Alternatively, download or contribute at [the module repository](https://aka.ms/IntuneDebug).

## Usage Scenarios

- **Basic Report**: Run `Get-MDMPolicyReport` locally (no admin needed) to generate a readable HTML overview of all policies applied to the device and signed-in users.
- **Advanced Report**: Run with administrative rights for extra detail (e.g., precise Win32Apps installation statuses) or load a previously captured report with the `-MDMDiagReportPath` parameter.
- **Remote Data**: Capture MDM diagnostics on any Windows device, copy results, and analyze them on another machine with the module installed.

## Report Sections

- **DeviceInfo**: General device metadata and Intune sync status
- **PolicyScope: Device**: All settings applied at the device scope, grouped by solution area/product (e.g., Defender, Delivery Optimization)
- **PolicyScope: <User>**: Per-user settings by SID and UPN
- **EnterpriseDesktopAppManagement**: List of MSI-based application deployments from Intune
- **Resources**: Payload-based policies like certificates and firewall rules
- **Win32Apps**: Win32 application policies (requires admin for installation status)
- **Intune Scripts**: State of remediation/detection scripts
- **Local Admin Password Solution (LAPS)**: LAPS-related settings and local status

## Data Collection Details

- Built-in tool `MdmDiagnosticsTool.exe` exports required MDM data (
  - Data collected to `C:\Users\PUBLIC\Documents\MDMDiagnostics`
  - Main parsed artifacts: `MDMDiagReport.html` and `MDMDiagReport.xml`
- Extra data (e.g., Win32App status, Intune Scripts) pulled from Intune Management Extension logs and the registry when admin rights are available
- Data folders can be auto-cleaned after a day (customizable via `-CleanUpDays`)

## Running Against Existing Diagnostics

To analyze a captured report from another machine:

1. Generate data on the source device with `mdmdiagnosticstool.exe -area "DeviceEnrollment;DeviceProvisioning;Autopilot" -zip C:\temp\MDMDiagnosticsData.zip`.
2. Unpack data and run `Get-MDMPolicyReport -MDMDiagReportPath <path>` on your analysis machine.

## Limitations & Notes

- The tool cannot remotely gather live data; it operates on local or pre-captured diagnostics
- Some information (e.g., Win32App status, UPN for users) only available when run locally and as admin
- Script is provided AS IS—no Microsoft support guarantee

## Troubleshooting & Contribution

If you encounter issues or have suggestions, contribute on GitHub via [https://aka.ms/IntuneDebug](https://aka.ms/IntuneDebug).

> *Stay safe! — Jonas Ohmsen*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/gpresult-like-tool-for-intune/ba-p/4437008)
