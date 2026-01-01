---
layout: "post"
title: "GitHub Retires Product-Specific Billing APIs for Actions, Packages, and Storage"
description: "This news update details the deprecation of GitHub's product-specific billing APIs for Actions, Packages, and shared storage. Users and organizations are directed to a new consolidated usage endpoint that simplifies reporting and monitoring of metered services. The announcement includes endpoint changes, impacts on account management workflows, and migration guidance."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-26-product-specific-billing-apis-are-closing-down"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-26 14:10:33 +00:00
permalink: "/2025-09-26-GitHub-Retires-Product-Specific-Billing-APIs-for-Actions-Packages-and-Storage.html"
categories: ["DevOps"]
tags: ["Account Management", "API Deprecation", "Billing API", "DevOps", "Endpoint Migration", "Enterprise Cloud", "GitHub", "GitHub Actions", "News", "Packages", "REST API", "Retired", "Shared Storage", "Usage Reporting", "Workflow Automation"]
tags_normalized: ["account management", "api deprecation", "billing api", "devops", "endpoint migration", "enterprise cloud", "github", "github actions", "news", "packages", "rest api", "retired", "shared storage", "usage reporting", "workflow automation"]
---

Allison announces the shutdown of GitHub's product-specific billing APIs affecting Actions, Packages, and shared storage, and introduces a consolidated usage endpoint for all users.<!--excerpt_end-->

# Product-Specific Billing APIs Are Closing Down

**Author:** Allison

As previously communicated, GitHub is proceeding with the shutdown of all product-specific billing APIs for Actions, Packages, and shared storage. The following endpoints are now retired for enterprises, organizations, and users:

- `/settings/billing/actions`
- `/settings/billing/packages`
- `/settings/billing/shared-storage`

Instead, a consolidated [usage endpoint](https://docs.github.com/enterprise-cloud@latest/rest/enterprise-admin/billing?apiVersion=2022-11-28#get-billing-usage-report-for-an-enterprise) is available to provide reporting on all metered GitHub products. This unified endpoint simplifies how teams and administrators monitor usage across Actions, Packages, and storage, streamlining billing workflows.

For migration details and implementation guidance, refer to the [Migrating from the endpoints used for the previous billing platform](https://docs.github.com/enterprise-cloud@latest/billing/tutorials/automate-usage-reporting#migrating-from-the-endpoints-used-for-the-previous-billing-platform) tutorial.

## What Has Changed?

- The dedicated billing APIs for GitHub Actions, Packages, and shared storage are no longer supported.
- All GitHub users should use the new consolidated billing usage endpoint to track spending and usage.

## Implications for DevOps and Account Management

- Automation scripts and integrations that previously called the old endpoints will need to update their logic to use the new unified API.
- Monitoring and reporting on usage is more centralized, reducing complexity when working with multiple GitHub services.

## Migration Guidance

- Review the migration guide provided by GitHub to update automated workflows and ensure seamless reporting.
- Test integrations with the new API and verify that desired usage data is available for your organization or enterprise.

## Additional Resources

- [GitHub Billing Usage Endpoint Documentation](https://docs.github.com/enterprise-cloud@latest/rest/enterprise-admin/billing?apiVersion=2022-11-28#get-billing-usage-report-for-an-enterprise)
- [Migration Tutorial](https://docs.github.com/enterprise-cloud@latest/billing/tutorials/automate-usage-reporting#migrating-from-the-endpoints-used-for-the-previous-billing-platform)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-26-product-specific-billing-apis-are-closing-down)
