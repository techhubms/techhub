---
layout: post
title: General Availability of Code Scanning Alert Assignees in GitHub
author: Allison
canonical_url: https://github.blog/changelog/2025-12-16-code-scanning-alert-assignees-are-now-generally-available
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-12-16 09:40:33 +00:00
permalink: /github-copilot/news/General-Availability-of-Code-Scanning-Alert-Assignees-in-GitHub
tags:
- AI
- Alert Assignment
- Alert Notifications
- Application Security
- Code Scanning
- Copilot Autofix
- DevOps
- DevOps Workflows
- GitHub Advanced Security
- GitHub Code Security
- GitHub Copilot
- GitHub Enterprise Server
- News
- Repository Management
- REST API
- Security
- Security Automation
- Security Workflows
- Vulnerability Remediation
- Webhooks
section_names:
- ai
- devops
- github-copilot
- security
---
Allison introduces the general availability of code scanning alert assignee features in GitHub, including assignment to Copilot, automated notifications, and REST API integration for enhanced security vulnerability management.<!--excerpt_end-->

# General Availability of Code Scanning Alert Assignees in GitHub

GitHub has released code scanning alert assignees as generally available, helping development teams manage, track, and remediate security vulnerabilities more effectively by assigning clear ownership directly within the GitHub workflow.

## What's New in General Availability?

The update brings several new features since public preview:

- **Assignment to Copilot**: Developers can delegate fixes for vulnerabilities to GitHub Copilot coding agent, automating remediation and reducing time spent on manual fixes. [Learn more](https://github.blog/changelog/2025-10-28-assign-code-scanning-alerts-to-copilot-for-automated-fixes-in-public-preview).
- **Notifications**: Assigned users now get email notifications based on repository watching settings when alerts are assigned.
- **Webhooks**: Webhook events trigger for changes in assignee status, allowing integration of alert assignment into custom workflows and security automation.
- **REST API Support**: Teams can programmatically view, assign, and unassign users to code scanning alerts, enabling bulk operations and custom integrations.

> ![Code scanning alert with Copilot Autofix](https://github.com/user-attachments/assets/ca39933d-7cf5-4248-b06d-94a1cf621734)

Above: An example alert shows how Copilot Autofix can be assigned to remediate vulnerabilities through suggested fixes.

## Using Alert Assignees

From the alert detail page, any code scanning alert can be assigned to users with `write` access to the relevant repository. This brings vulnerability remediation right into the issue and pull request workflow developers already use, making security work highly actionable.

Benefits of alert assignees include:

- **Clear Ownership**: Teams can explicitly assign responsibility for specific vulnerabilities.
- **Tracking Work**: Remediation progress is tracked inside GitHub.
- **Accelerated Fixes**: Assignment makes responsibility for security issues visible and actionable, speeding up response times.

## Availability

Alert assignees are available for customers with GitHub Code Security or GitHub Advanced Security on github.com. Support for GitHub Enterprise Server will arrive with version 3.20.

## Resources

- [Managing code scanning alerts](https://docs.github.com/code-security/code-scanning/managing-code-scanning-alerts/managing-code-scanning-alerts-for-your-repository)
- [Assigning code scanning alerts](https://docs.github.com/code-security/code-scanning/managing-code-scanning-alerts/about-code-scanning-alerts#about-alert-assignment)

## Summary

With these enhancements, GitHub further integrates security automation and ownership into developer workflows, leveraging AI and automation to simplify and accelerate vulnerability response.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-16-code-scanning-alert-assignees-are-now-generally-available)
