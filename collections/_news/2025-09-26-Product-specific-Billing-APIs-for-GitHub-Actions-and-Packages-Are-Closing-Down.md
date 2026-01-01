---
layout: "post"
title: "Product-specific Billing APIs for GitHub Actions and Packages Are Closing Down"
description: "GitHub is retiring its product-specific billing APIs for Actions, Packages, and shared storage. Users and organizations are being moved to a single, unified usage reporting endpoint that consolidates billing data across all metered products. This update impacts developers automating billing and usage reporting workflows, requiring migration to the new API endpoints."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-25-product-specific-billing-apis-are-closing-down"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-26 12:10:33 +00:00
permalink: "/2025-09-26-Product-specific-Billing-APIs-for-GitHub-Actions-and-Packages-Are-Closing-Down.html"
categories: ["DevOps"]
tags: ["Account Management", "API Deprecation", "API Migration", "Billing API", "Change Management", "CI/CD", "Developer Workflows", "DevOps", "GitHub", "GitHub Actions", "News", "Package Management", "Retired", "Usage Reporting"]
tags_normalized: ["account management", "api deprecation", "api migration", "billing api", "change management", "cislashcd", "developer workflows", "devops", "github", "github actions", "news", "package management", "retired", "usage reporting"]
---

Allison summarizes the closure of GitHub’s product-specific billing APIs for Actions, Packages, and shared storage, guiding users to the new consolidated usage endpoint.<!--excerpt_end-->

# Product-specific Billing APIs for GitHub Actions and Packages Are Closing Down

GitHub has officially closed the remaining product-specific billing API endpoints for Actions (`/settings/billing/actions`), Packages (`/settings/billing/packages`), and shared storage (`/settings/billing/shared-storage`) for enterprises, organizations, and individual users.

## What’s Changing

- These endpoints are no longer supported or accessible.
- All users must now switch to a consolidated [usage endpoint](https://docs.github.com/enterprise-cloud@latest/rest/enterprise-admin/billing?apiVersion=2022-11-28#get-billing-usage-report-for-an-enterprise) for accessing metered product usage and billing data.

## Migration Guidance

- Reference [Migrating from the endpoints used for the previous billing platform](https://docs.github.com/enterprise-cloud@latest/billing/tutorials/automate-usage-reporting#migrating-from-the-endpoints-used-for-the-previous-billing-platform) for details on moving automated reporting workflows to the new API.

## Impact

- Developers who automated usage reporting or billing queries with the retiring endpoints will need to update their scripts and tools.
- The migration provides a single API endpoint for all usage details, simplifying future billing integrations.

## Next Steps

- Update any code or workflow automation to use the unified endpoint.
- Review GitHub documentation for best practices on [usage data automation](https://docs.github.com/enterprise-cloud@latest/billing/tutorials/automate-usage-reporting).

---

For more details, visit the [GitHub Blog announcement](https://github.blog/changelog/2025-09-25-product-specific-billing-apis-are-closing-down).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-25-product-specific-billing-apis-are-closing-down)
