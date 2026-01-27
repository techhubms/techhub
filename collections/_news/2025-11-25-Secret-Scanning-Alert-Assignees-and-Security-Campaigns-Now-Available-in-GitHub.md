---
external_url: https://github.blog/changelog/2025-11-25-secret-scanning-alert-assignees-security-campaigns-are-generally-available
title: Secret Scanning Alert Assignees and Security Campaigns Now Available in GitHub
author: Allison
feed_name: The GitHub Blog
date: 2025-11-25 17:51:00 +00:00
tags:
- Alert Assignees
- Application Security
- Bulk Actions
- CI/CD
- GitHub
- Remediation
- Repository Security
- REST API
- Secret Scanning
- Security Alerts
- Security Automation
- Security Campaigns
- Security Dashboard
- Webhooks
section_names:
- devops
- security
primary_section: devops
---
Allison reports on GitHub's latest features for secret scanning alert assignees and security campaigns, highlighting improvements in alert notifications, remediation workflows, and security automation for development teams.<!--excerpt_end-->

# Secret Scanning Alert Assignees and Security Campaigns Now Available in GitHub

GitHub has announced the general availability of two major security workflow features: secret scanning alert assignees and security campaigns. These enhancements aim to streamline how organizations and teams track, manage, and remediate secret scanning alerts in their repositories.

## What's New?

- **Notifications:**
  - Alert assignees now receive email notifications if subscribed to `participating and @mentions`, not just the broader `All events` or `Security Alert events`. This makes it easier for developers and security engineers to respond to security alerts directly affecting them.

- **Campaign List Views:**
  - The campaign list views now mirror existing alert lists, supporting filters and bulk actions.
  - Repository-level campaign views are available to anyone with access to the repository’s alert list, aiding visibility across teams.

- **REST API and Webhooks:**
  - The REST API now supports actions for viewing and updating secret scanning security campaigns, and for managing alert assignments.
  - Webhooks for alert assignees have been introduced, functioning similarly to those for pull requests and issue assignments, enabling automated integrations and workflow triggers.

## Security Campaigns Explained

Security campaigns in secret scanning allow organizations to:

- **Target specific alerts** for focused remediation efforts.
- **Set remediation deadlines** to ensure timely resolution.
- **Notify admins and security managers** to coordinate responses.
- Campaigns are created and published from the security overview dashboard.

## Alert Assignees Explained

Assigning alerts makes it easier to track individual responsibility for remediation:

- Alerts can be delegated to anyone with `write` access.
- Assignees get permissions to view and edit alerts if they lacked access before assignment.
- Permissions are revoked if the user is unassigned from the alert.
- Secret scanning alert assignees will be available for GitHub Enterprise Server starting with version 3.20.

## Resources and Next Steps

- [Learn more about security campaigns](https://docs.github.com/enterprise-cloud@latest/code-security/securing-your-organization/fixing-security-alerts-at-scale/creating-managing-security-campaigns)
- [Alert assignment for secret scanning alerts](https://docs.github.com/code-security/secret-scanning/managing-alerts-from-secret-scanning/viewing-alerts#filtering-alerts)

These new features help development and security teams better collaborate and automate alert management, promoting stronger security practices and quicker remediation throughout the software development lifecycle.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-25-secret-scanning-alert-assignees-security-campaigns-are-generally-available)
