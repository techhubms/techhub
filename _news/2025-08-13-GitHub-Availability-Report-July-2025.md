---
layout: "post"
title: "GitHub Availability Report: July 2025"
description: "This report covers a July 2025 incident impacting GitHub Enterprise Importer (GEI), detailing causes, remediation, changes to IP allow lists, and improvements in infrastructure recovery and validation. The update provides essential information for teams managing GitHub migrations, especially those integrating with Azure DevOps, Azure Blob Storage, and Bitbucket platforms."
author: "Jakub Oleksy"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/news-insights/company-news/github-availability-report-july-2025/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-08-13 21:00:00 +00:00
permalink: "/2025-08-13-GitHub-Availability-Report-July-2025.html"
categories: ["DevOps"]
tags: ["Azure DevOps", "Bitbucket", "Cloud Migration", "Company News", "DevOps", "GitHub", "GitHub Availability", "GitHub Availability Report", "GitHub Enterprise Importer", "Incident Report", "Incident Response", "Infrastructure Recovery", "IP Allow List", "Migration", "News", "News & Insights", "Unit Testing", "Validation"]
tags_normalized: ["azure devops", "bitbucket", "cloud migration", "company news", "devops", "github", "github availability", "github availability report", "github enterprise importer", "incident report", "incident response", "infrastructure recovery", "ip allow list", "migration", "news", "news insights", "unit testing", "validation"]
---

Jakub Oleksy shares the July 2025 GitHub Availability Report, analyzing a migration outage affecting GitHub Enterprise Importer and outlining important steps for DevOps teams, including updating IP allow lists.<!--excerpt_end-->

# GitHub Availability Report: July 2025

**Author:** Jakub Oleksy

In July 2025, GitHub experienced an incident affecting the GitHub Enterprise Importer (GEI) service, which resulted in degraded performance and prevented migration processing for several hours. This report details the timeline, cause, and remediation steps, along with guidance for users managing migrations and relevant integrations.

## Incident Timeline

- **Start:** July 28, 2025, 21:41 UTC
- **End:** July 29, 2025, 03:15 UTC
- **Duration:** 5 hours, 34 minutes

During this period, GEI was unable to process migrations due to an infrastructure component being improperly taken out of service during an internal improvement process. The previous configuration could not be restored, requiring the provisioning of new resources to resolve the outage.

## Impacted Areas

- Migrations via GitHub Enterprise Importer (GEI) were not processed
- Affected migrations sourced from github.com, GitHub Enterprise Server, Bitbucket Server, Bitbucket Data Center, and Azure DevOps

## Root Cause and Mitigation

- A key infrastructure component was inadvertently retired before proper validation
- Recovery required provisioning new resources
- Improvements have been made in infrastructure recovery, unit testing, and validation workflows

## Required Actions: Update IP Allow Lists

If your organization uses GEI for migrations, update your IP allow lists as follows:

**Add these IP ranges:**

- `20.99.172.64/28`
- `135.234.59.224/28`

**Remove these old IP ranges:**

- `40.71.233.224/28`
- `20.125.12.8/29`

**Where to update:**

- Your destination github.com organization’s IP allow list
- Your source github.com organization’s IP allow list (if running migrations from github.com)
- Azure Blob Storage or Amazon S3 allow lists (if migrating from GitHub Enterprise Server, Bitbucket Server, or Bitbucket Data Center)
- Azure DevOps organization’s allow list (if migrating from Azure DevOps)

Users running migrations in the past 90 days have been notified by email.

## Ongoing Improvements

- Enhanced internal unit testing and infrastructure validation
- Better recovery mechanisms for GEI infrastructure

## Additional Resources

- [GitHub Status Page](https://www.githubstatus.com/) for live status updates and post-incident summaries
- [GitHub Engineering Blog](https://github.blog/category/engineering/) for more technical insights and future improvements

For migration-related support, contact [GitHub Customer Support](https://support.github.com/).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/company-news/github-availability-report-july-2025/)
