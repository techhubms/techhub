---
external_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/azure-automation-general-availability-of-powershell-7-4-python-3/ba-p/4437732
title: Azure Automation Introduces PowerShell 7.4, Python 3.10, and New Runtime Environment
author: Nikita_Bajaj
feed_name: Microsoft Tech Community
date: 2025-07-29 11:34:41 +00:00
tags:
- Azure Automation
- Azure CLI
- Cross Platform Orchestration
- Infrastructure Management
- Module Version Management
- Powershell
- PowerShell 7.4
- Python 3.10
- Rollback Capability
- Runbooks
- Runtime Environment
- Script Modernization
section_names:
- azure
- coding
- security
---
Nikita_Bajaj announces major enhancements to Azure Automation, including support for PowerShell 7.4, Python 3.10, and a new Runtime Environment. The update boosts security, script modernization, control, and operational flexibility for enterprise automation.<!--excerpt_end-->

## Overview

Azure Automation has rolled out significant updates to help enterprises meet evolving operational needs. The platform now generally supports PowerShell 7.4 and Python 3.10 runtime versions, a new Runtime Environment, streamlined upgrades, expanded Azure CLI integration, and improved organizational capabilities.

## Key Announcements & Features

### General Availability of PowerShell 7.4 & Python 3.10

Automation runbooks can now leverage the latest stable releases of both PowerShell and Python, ensuring updated security, performance, and compatibility across the Azure ecosystem.

### Runtime Environment

This feature helps modernize previously written scripts without requiring full rewrites. It facilitates quick script testing, validation, and safe upgrades by providing a controlled environment for updates.

### Azure CLI Support in PowerShell Runbooks

Users can now embed Azure CLI commands directly within PowerShell runbooks, broadening automation scenarios and reducing the reliance on separate tooling.

## Benefits to Users

- **Stay Current:** Continuously updated runtime environments enhance security and maintain compatibility with Azure’s newest features.
- **Faster Upgrades:** Minimize downtime and de-risk upgrades with the Runtime Environment, by validating scripts prior to full deployment.
- **Rollback Capability:** Easily revert runbooks to previous language versions if issues arise, ensuring business continuity.
- **Granular Control:** Fine-tune script execution settings including language versions and module dependencies per runbook.
- **Efficient Module Management:** Avoid the complexity of multiple Automation accounts for conflicting module requirements, simplifying governance.
- **Expanded Automation:** Integrate Azure CLI and PowerShell seamlessly to automate complex resource management tasks across Azure and multi-cloud environments.
- **Cross-Platform Management:** Orchestrate automation tasks on-premises, within Azure, and with other public clouds, reinforcing the adaptive, “cloud-first” vision.

## Upgrade Recommendations

Legacy environments such as PowerShell 7.1/7.2 and Python 2.7/3.8 are retiring. Users are strongly urged to update runbooks to newer, supported versions using the Runtime Environment for continued support and security.

## Resources

- [Azure Automation runbook types](https://learn.microsoft.com/en-us/azure/automation/automation-runbook-types)
- [Upgrade runbook to latest language version](https://learn.microsoft.com/en-us/azure/automation/quickstart-update-runbook-in-runtime-environment)
- [PowerShell runbooks with Azure CLI](https://learn.microsoft.com/en-us/azure/automation/quickstart-cli-support-powershell-runbook-runtime-environment)

For additional information, feedback, or questions, readers are encouraged to contact <askazureautomation@microsoft.com>.

This post appeared first on Microsoft Tech Community. [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/azure-automation-general-availability-of-powershell-7-4-python-3/ba-p/4437732)
