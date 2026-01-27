---
external_url: https://techcommunity.microsoft.com/t5/azure/azure-automation-feature-improvements-and-bugs/m-p/4456195#M22242
title: 'Azure Automation: User Requests, Feature Suggestions, and Bug Reports'
author: mynster
feed_name: Microsoft Tech Community
date: 2025-09-23 10:12:56 +00:00
tags:
- Automation Account
- Azure Automation
- Bug Reports
- Community Feedback
- Feature Requests
- GitHub Integration
- Hybrid Workers
- Job Scheduling
- Logging
- PowerShell
- RBAC
- Runbooks
- Source Control
- UI Improvements
- Webhooks
section_names:
- azure
- coding
- devops
primary_section: coding
---
In this comprehensive community post, mynster outlines several bugs and feature suggestions for Azure Automation Accounts, aiming to improve reliability, developer experience, and usability for automation engineers.<!--excerpt_end-->

# Azure Automation: User Requests, Feature Suggestions, and Bug Reports

Author: mynster

This post collects detailed feedback on Microsoft Azure Automation Accounts, with first-hand observations and recommendations for improving workflow automation and integration with modern development practices.

## Source Control (GitHub)

### Bugs

- Tags are being overwritten or removed during both full and incremental syncs with source control (reported as case #2508010040002105).

### Feature Requests

- When runbooks are deleted in source control, they should also be deleted in the Automation Account to maintain consistency.
- Support for different sync types beyond PowerShell 5.1 (requests for newer versions before enabling source control are present).
- Enable full repository syncing (recursion) rather than restricting to specific folders, improving repository organization.
- Reduce redundancy in setting up multiple sources, especially as source control integration expires after one year regardless of PAT token settings.
- Sync synopsis/description fields from PowerShell scripts directly to Automation Account descriptions, ideally by parsing script help output (e.g. the output of `get-help .\ScriptName.ps1`).

## Logging

### Bugs

- Duplicate log entries appear after loading initial logs; subsequent scrolling repeats these entries (likely a JavaScript-related UI bug). This often happens during active runbook execution.
- PowerShell 7+ outputs sometimes include unescaped ASCII characters, causing log splitting and decreased readability (reported as now fixed).

### Feature Requests

- Ability to search for a specific job ID in the general job list (currently only possible through workarounds).
- Multi-line formatting support in log outputs for more readable outputs (e.g. `Write-Output "New\r\nLine"`).

## Runbook Page

### Bugs

- Searching for runbook names may produce inconsistent or incorrect results, such as non-matching entries, or failing to return to the full list after clearing a search.

### Feature Requests

- Option to rerun or restart a previous job with identical parameters, similar to restarting a GitHub Actions run.

## Scheduling

### Feature Requests

- Define schedules for runbooks directly within code parameters. Current workaround involves scanning runbooks for parameters, constructing schedules programmatically, and tagging them with schedule information.

## Hybrid Workers

### Feature Requests

- Ability to pause a hybrid worker within a worker group for maintenance or troubleshooting, without removal and extension reinstall (mitigates issues during patch windows or module updates).
- Maintenance mode for hybrid workers, allowing completion of current jobs followed by suspension for updates or debugging.
- Link to related Hybrid Worker troubleshooting guidance: [Microsoft Docs Scenario](https://learn.microsoft.com/en-us/azure/automation/troubleshoot/extension-based-hybrid-runbook-worker#scenario-runbooks-go-into-a-suspended-state-on-a-hybrid-runbook-worker-when-using-a-custom-account-on-a-server-with-user-account-control-uac-enabled)

## Additional Feature Requests

- An end-user-friendly front end for Azure Automation (self-service portal), allowing selected users to run specific runbooks with tailored UI limitations. Example/inspiration: [Self-Service Frontend on GitHub](https://github.com/Mynster9361/Self-Service-Frontend-Azure-Automation).
- Runbook-level RBAC permissions, possibly extended beyond what CLI currently supports, to enhance security for individual workflows.
- Centralized management blade for webhooks, giving a unified view of all webhook-to-runbook associations, their status, expiration, and last triggered time. This improves security and simplifies management over scattered per-runbook webhooks.

## Highest Priority Wishes

1. Revamp source control integration:
   - Support for more scripting languages and PowerShell versions
   - Accurate tag synchronization
   - Reflect deletions from source control in Automation Account automatically
2. Robust hybrid worker maintenance mode:
   - Permit safe updates and troubleshooting
   - Prevent jobs from stalling or suspending during maintenance windows

---

The above points reflect practical needs from real-world Azure Automation administrators, with actionable suggestions for Microsoft to improve developer and operations experience.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/azure-automation-feature-improvements-and-bugs/m-p/4456195#M22242)
