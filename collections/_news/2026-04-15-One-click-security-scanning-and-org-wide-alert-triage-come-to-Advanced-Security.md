---
title: One-click security scanning and org-wide alert triage come to Advanced Security
external_url: https://devblogs.microsoft.com/devops/one-click-security-scanning-and-org-wide-alert-triage-come-to-advanced-security/
author: Laura Jiang
feed_name: Microsoft DevOps Blog
primary_section: azure
tags:
- Agent Pools
- Alert Triage
- Azure
- Azure & Cloud
- Azure DevOps
- Azure DevOps Advanced Security
- Azure Pipelines
- CI/CD
- Code Scanning
- CodeQL
- Combined Alerts
- CVE Remediation
- Default Setup
- Dependency Scanning
- DevOps
- News
- Organization Settings
- Repository Security
- Secret Scanning
- Security
- Security Campaigns
- Security Overview
section_names:
- azure
- devops
- security
date: 2026-04-15 15:06:53 +00:00
---

Laura Jiang announces two Azure DevOps Advanced Security updates: CodeQL default setup to enable org-wide code scanning without per-repo pipeline configuration, and a combined alerts experience (with security campaigns) to triage and coordinate remediation across all repositories.<!--excerpt_end-->

# One-click security scanning and org-wide alert triage come to Advanced Security

We’re shipping two major capabilities that change how security teams enable and act on application security in Azure DevOps:

- **CodeQL default setup** enables code scanning across your organization without configuring a pipeline per repository.
- A **new combined alerts experience in Security Overview** provides a single place for administrators to search, filter, and coordinate remediation across every repository.

In tandem with **dependency scanning default setup** and **automatic secret scanning**, scanning is now the default. Delegating work is built into the product with **security campaigns** powered by the combined alerts experience.

## CodeQL default setup (public preview)

Previously, enabling CodeQL code scanning on Azure DevOps required manual work per repository:

- Configure a pipeline
- Install the CodeQL task
- Set up build steps
- Maintain the pipeline over time

For organizations with hundreds of repositories, this created a significant adoption barrier.

**CodeQL default setup** removes that friction. With one click, you can enable code scanning:

- For a repository
- Across a project
- Across the entire organization

Advanced Security then **automatically runs CodeQL scans using Azure Pipelines by default**, without additional configuration.

![Screenshot showing CodeQL default setup enablement in Advanced Security](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/04/advanced-security-codeql-default-setup-enablement-repo-unbundled.webp)

### Key capabilities

- **One-click enablement** at repository, project, or organization level
- **Automatic scanning** (no YAML editing, no task installation)
- **Runs on Azure Pipelines** out of the box
- **Configurable agent pool** via organization-level repository settings (control where scans run)

### Getting started

1. Navigate to your repository, project, or organization settings
2. Enable the Code Security plan for your repository
3. Enable CodeQL default setup
4. Scans run on a schedule (schedule can be changed at the organization level)

For more details: https://aka.ms/ghazdo/codeql-default-setup

## Combined alerts view and security campaigns

Security administrators requested an org-wide view rather than checking alerts repo by repo. The new **combined alerts experience** in **Security Overview** is intended to address that.

### See everything in one place

The **Security Overview alerts tab** surfaces alerts from the default branch of **all repositories** in your organization in a single unified view.

From that screen you can:

- Search across all repositories
- Sort and filter across the estate
- Review posture without drilling into individual repos

![Animated screenshot of filtering in the Security Overview alerts view](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/04/security-overview-alerts-page.gif)

### Security campaigns: coordinate remediation at scale

**Security campaigns** let you create and share filtered alert views with your team as a way to coordinate remediation.

- Filters are **live**: new vulnerabilities that match the criteria will appear automatically.

Use campaigns to:

- Track remediation of a specific **CVE** across all affected repositories
- Create a “critical secrets” campaign for weekly triage
- Share a filtered view with a development team so they only see what’s relevant to them

![Animated screenshot showing security campaigns in the alerts experience](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/04/security-overview-alerts-campaigns.gif)

## What’s next

- **CodeQL default setup** and the **combined alerts dashboard** will roll out to organizations over the next **two to three weeks**.
- To try **CodeQL default setup**, enable it from repository or organization settings.
- To view the **combined alerts dashboard**, go to **Organization Settings > Security overview**.

Have feedback? Reach out via the Azure DevOps Developer Community or contact your Microsoft account team.

[Read the entire article](https://devblogs.microsoft.com/devops/one-click-security-scanning-and-org-wide-alert-triage-come-to-advanced-security/)

