---
layout: "post"
title: "Work Item Linking for GitHub Advanced Security Alerts in Azure DevOps Now Available"
description: "This announcement details the new feature that allows users to directly link work items in Azure DevOps Boards to security alerts surfaced by GitHub Advanced Security. It streamlines tracking, prioritizing, and fixing vulnerabilities by bridging the gap between security alerting and regular sprint/project management within Azure DevOps. The update enhances visibility and coordination for security and engineering teams, and is part of ongoing efforts to integrate security workflows natively into Azure DevOps."
author: "Laura Jiang"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/work-item-linking-for-advanced-security-alerts-now-available/"
viewing_mode: "external"
feed_name: "Microsoft DevOps Blog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2025-12-19 18:24:14 +00:00
permalink: "/2025-12-19-Work-Item-Linking-for-GitHub-Advanced-Security-Alerts-in-Azure-DevOps-Now-Available.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["#azure", "#devops", "Advanced Security Hub", "Azure", "Azure & Cloud", "Azure DevOps", "Dependency Scanning", "DevOps", "DevOps Boards", "GitHub Advanced Security", "News", "Permissions", "Security", "Security Alerts", "Security Workflow", "Sprint Planning", "Vulnerability Management", "Work Item Linking"]
tags_normalized: ["sharpazure", "sharpdevops", "advanced security hub", "azure", "azure and cloud", "azure devops", "dependency scanning", "devops", "devops boards", "github advanced security", "news", "permissions", "security", "security alerts", "security workflow", "sprint planning", "vulnerability management", "work item linking"]
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
