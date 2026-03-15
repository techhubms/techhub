---
external_url: https://techcommunity.microsoft.com/t5/azure-migration-and/azure-migrate-physical-server-discovery-serverdiscoveryservice/m-p/4490238#M733
title: ServerDiscoveryService.exe Crash Bug in Azure Migrate Physical Server Discovery
author: cbrnit
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-01-29 02:24:12 +00:00
tags:
- .NET 8
- Appliance Crash
- Azure
- Azure Migrate
- Azure Migration
- Bug Report
- CIM
- Community
- Discovery Agent
- Microsoft.Management.Infrastructure
- NullReferenceException
- Physical Server Discovery
- PowerShell Remoting
- ServerDiscoveryService.exe
- Windows Server
- WinRM
- WSMan
section_names:
- azure
---
Community member cbrnit delivers a thorough technical breakdown of a reproducible crash issue affecting Azure Migrate’s ServerDiscoveryService.exe in physical server discovery workflows.<!--excerpt_end-->

# Azure Migrate Physical Server Discovery - ServerDiscoveryService.exe Crash Bug

## Summary

An issue is observed in the Azure Migrate appliance where the ServerDiscoveryService.exe process crashes during physical server discovery. While the service connects to target servers successfully, it consistently fails before collecting any discovery data due to a code defect.

## Environment Details

- **Appliance OS:** Windows Server 2022 Standard Evaluation (Build 20348)
- **Appliance Type:** Physical server discovery, script-based installation
- **ServerDiscoveryService.exe Version:** 2.0.3300.663
- **.NET Version:** 8.0.22 (CoreCLR 8.0.2225.52707)
- **Target Servers:** Mixed Windows Server and Linux (on-premises)
- **Discovery Agent Version:** 2.0.03300.663
- **Appliance Configuration Manager Version:** 6.1.294.1847

## Symptom Timeline

1. Target server validation is successful in the Appliance Configuration Manager
2. CIM sessions connect successfully (logs: "TestConnection succeeded for CIM Session with HTTP protocol")
3. Immediately, connections are disposed with messages about process shutdown
4. No discovery data is collected at any point
5. Azure portal displays error 60001 and references "Could not load file or assembly 'Microsoft.Management.Infrastructure'"
6. Discovery status persists as "Discovery Incomplete" for all Windows servers

## Root Cause Analysis

The ServerDiscoveryService.exe process exhibits repeated, unhandled `NullReferenceException` errors within the WSMan transport finalizer. From the Windows Application Event Log:

> Application: ServerDiscoveryService.exe CoreCLR Version: 8.0.2225.52707 .NET Version: 8.0.22 Description: The process was terminated due to an unhandled exception. Exception Info: System.NullReferenceException: Object reference not set to an instance of an object. ...

A related access violation is logged:

> Faulting application name: ServerDiscoveryService.exe, version: 2.0.3300.663 Exception code: 0xc0000005

This crash cycle repeats every ten minutes, preventing completion of any discovery operation.

## Troubleshooting Performed

- **PowerShell Remoting:** Verified successful connectivity with `Invoke-Command` and `New-CimSession`
- **WinRM Configuration:** Confirmed listeners and `LocalAccountTokenFilterPolicy` set appropriately
- **Assembly Verification:** Checked that `Microsoft.Management.Infrastructure.dll` exists in GAC on both appliance and targets
- **Network:** Tested both FQDNs and IP addresses (same result)
- **Credentials:** Tested both domain and local accounts
- **NTP:** Verified appliance and target clocks align
- **Updates:** Confirmed all components are current
- **Fresh Installation:** Reproduced on both OVA and scripted install, on clean Server 2022

## Log Locations

- `C:\ProgramData\Microsoft Azure\Logs\ConfigManager\ClientOperations\*` — Shows CIM connections/disposals
- `C:\ProgramData\Microsoft Azure\Logs\ConfigManager\ApplianceOnboarding-Portal-*` — Contains error 60000 and "UnhandledException"
- **Windows Event Log** — Includes stack traces from application crashes

## Conclusion

This issue stems from a programming defect within ServerDiscoveryService.exe. The unhandled `NullReferenceException` in a finalizer suggests a code-quality error, rather than misconfiguration. The appliance validates connectivity and environment but fails just before or at the point of data gathering due to this crash.

## Request for Microsoft

_Community user **cbrnit** requests escalation to the Azure Migrate engineering team to address this reproducible crash in version 2.0.3300.663 of ServerDiscoveryService.exe._

---

**If you are experiencing similar issues, document your environment details and logs before escalating to Microsoft support.**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-migration-and/azure-migrate-physical-server-discovery-serverdiscoveryservice/m-p/4490238#M733)
