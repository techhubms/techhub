---
layout: "post"
title: "GitHub Enterprise Importer Incident and IP Range Update: July 2025 Availability Report"
description: "A detailed incident report outlining the July 2025 service degradation for GitHub Enterprise Importer (GEI), the root cause analysis, recovery steps, infrastructure improvements, and the new required IP ranges for migrations, including Azure DevOps and Azure Storage integration details."
author: "Jakub Oleksy"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/news-insights/company-news/github-availability-report-july-2025/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-08-13 21:00:00 +00:00
permalink: "/2025-08-13-GitHub-Enterprise-Importer-Incident-and-IP-Range-Update-July-2025-Availability-Report.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Availability Report", "AWS S3", "Azure", "Azure Blob Storage", "Azure DevOps", "Bitbucket", "Change Management", "Cloud Storage", "Company News", "DevOps", "GitHub Availability Report", "GitHub Enterprise Importer", "Incident Response", "Infrastructure Recovery", "IP Allow List", "Migration", "Network Security", "News", "News & Insights", "Security", "Service Incident", "Unit Testing", "Validation"]
tags_normalized: ["availability report", "aws s3", "azure", "azure blob storage", "azure devops", "bitbucket", "change management", "cloud storage", "company news", "devops", "github availability report", "github enterprise importer", "incident response", "infrastructure recovery", "ip allow list", "migration", "network security", "news", "news and insights", "security", "service incident", "unit testing", "validation"]
---

Jakub Oleksy reports on the July 2025 service disruption affecting GitHub Enterprise Importer, focusing on incident details, infrastructure changes, updated IP allow list requirements for Azure and other cloud migrations, and security considerations.<!--excerpt_end-->

# GitHub Enterprise Importer Incident and IP Range Update: July 2025 Availability Report

**Author:** Jakub Oleksy  
**Published:** July 2025

In July 2025, GitHub services experienced a significant incident impacting the GitHub Enterprise Importer (GEI). This post outlines the incident timeline, its root cause, the remediation steps taken, and important security updates for organizations using GEI in migration scenarios, particularly those integrating with Azure DevOps, Azure Blob Storage, AWS S3, or Bitbucket.

## Incident Timeline and Impact

- **Start:** July 28, 2025, 21:41 UTC  
- **End:** July 29, 2025, 03:15 UTC  
- **Duration:** 5 hours and 34 minutes  
- **Impact:** Migrations via GitHub Enterprise Importer were unavailable. Customers could not process migrations, affecting integrations with Microsoft Azure and other platforms.

## Root Cause

A key infrastructure component of GEI was inadvertently removed during routine internal improvements. Recovery was not possible; new resources had to be provisioned, resulting in downtime.

## Remediation and Improvements

- Improved infrastructure recovery mechanisms
- Enhanced unit testing
- Stricter validation using test data

## Action Required: IP Allow List Update

Customers must update firewalls and allow lists with new GEI IP ranges to restore migration functionality:

**Add these IP ranges to your applicable allow lists:**

- `20.99.172.64/28`
- `135.234.59.224/28`

**Remove these old IP ranges:**

- `40.71.233.224/28`
- `20.125.12.8/29`

**Where to update:**

- GitHub.com destination organization or enterprise IP allow list
- Source GitHub.com organization or enterprise (if applicable)
- Azure Blob Storage or Amazon S3 storage account allow list (for migrations from external servers)
- Azure DevOps organization allow list (for migrations from Azure DevOps)

## Notifications and Support

- Users with migrations in the last 90 days received email alerts.
- For questions or problems, contact GitHub Customer Support.
- Follow the [GitHub Status Page](https://www.githubstatus.com/) for updates.
- More technical details and ongoing engineering efforts: [GitHub Engineering Blog](https://github.blog/category/engineering/).

## Security and Compliance Considerations

- Review and update firewall policies and allow lists promptly to ensure migration operations continue securely.
- Verify allow list changes across all integrated services, including Azure DevOps, Azure Blob Storage, and any third-party platforms.

---

For further details, visit the original blog post: [GitHub Availability Report: July 2025](https://github.blog/news-insights/company-news/github-availability-report-july-2025/).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/company-news/github-availability-report-july-2025/)
