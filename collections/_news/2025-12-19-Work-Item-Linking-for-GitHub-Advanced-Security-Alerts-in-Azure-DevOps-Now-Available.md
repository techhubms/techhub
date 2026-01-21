---
external_url: https://devblogs.microsoft.com/devops/work-item-linking-for-advanced-security-alerts-now-available/
title: Work Item Linking for GitHub Advanced Security Alerts in Azure DevOps Now Available
author: Laura Jiang
feed_name: Microsoft DevOps Blog
date: 2025-12-19 18:24:14 +00:00
tags:
- '#azure'
- '#devops'
- Advanced Security Hub
- Azure & Cloud
- Azure DevOps
- Dependency Scanning
- DevOps Boards
- GitHub Advanced Security
- Permissions
- Security Alerts
- Security Workflow
- Sprint Planning
- Vulnerability Management
- Work Item Linking
section_names:
- azure
- devops
- security
---
Laura Jiang announces the general availability of work item linking for GitHub Advanced Security alerts in Azure DevOps, simplifying security tracking and resolution in engineering teams.<!--excerpt_end-->

# Work Item Linking for GitHub Advanced Security Alerts in Azure DevOps Now Available

**Author:** Laura Jiang

Security vulnerabilities require active management—from tracking and prioritization to actually shipping fixes. Historically, engineers have faced friction handling security alerts alongside everyday sprint work, toggling between separate tabs for vulnerability alerts and project boards.

To address this, GitHub Advanced Security for Azure DevOps now supports **work item linking**: you can directly associate Azure DevOps board work items with Advanced Security alerts, creating a seamless connection between security findings and actionable engineering tasks.

## Problem: Siloed Security and Sprint Planning

- Security alerts live in the Advanced Security hub, while sprint planning remains in DevOps Boards.
- Teams often lose context on alert ownership and struggle with visibility: Who's responsible for a specific vulnerability? Is it being actively addressed?
- Bridging this gap helps both security and engineering teams stay aligned.

## How Work Item Linking Works

- **Bidirectional linking:** From an alert, add a related work item; from a work item, link it to a security alert.
- **UI enhancements:** You'll see which alerts have linked work items directly in the repository's Advanced Security tab.
- **Seamless navigation:** Quickly switch between an alert and its corresponding work item for better context and faster action.
- **Permissions respected:** Only users with proper access can create links between alerts and work items.

### Steps to Link

1. Open a security alert in the Advanced Security hub and click “Add” next to the **Related Work** section.
2. Alternatively, within a work item in Boards, add a link and select **Advanced Security Alert** as the link type.
3. Once linked, you can jump between the two with a click.

## Additional Security Workflow Improvements

- Recent updates include:
  - [One-click enablement for dependency scanning](https://learn.microsoft.com/en-us/azure/devops/release-notes/2025/sprint-264-update#one-click-enablement-for-dependency-scanning-generally-available)
  - [Granular project and organization-level enablement settings](https://learn.microsoft.com/en-us/azure/devops/release-notes/2025/sprint-262-update#granular-enablement-panels-now-available-for-project-and-organization-level-enablement)
- These updates continue to make security workflows more native and robust within Azure DevOps.

## Get Started

- **Try it:** [Link work items to Advanced Security alerts](https://learn.microsoft.com/en-us/azure/devops/boards/backlogs/add-link?view=azure-devops&tabs=browser#link-work-items-to-advanced-security-alerts)
- **Learn more:** [GitHub Advanced Security for Azure DevOps Documentation](https://learn.microsoft.com/en-us/azure/devops/repos/security/configure-github-advanced-security-features)
- **Feedback:** [Let the team know what you think](https://aka.ms/ghazdo-feedback)

This enhancement helps engineering and security teams integrate workflows, reduce context-switching, and increase confidence that every critical security issue is tracked and resolved.

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/work-item-linking-for-advanced-security-alerts-now-available/)
