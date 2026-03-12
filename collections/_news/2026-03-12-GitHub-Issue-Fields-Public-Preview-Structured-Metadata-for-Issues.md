---
layout: "post"
title: "GitHub Issue Fields Public Preview: Structured Metadata for Issues"
description: "GitHub has announced that issue fields, providing structured and typed metadata for issues, are now available in public preview for select organizations. This feature allows administrators to configure up to 25 fields, customize their visibility across issue types, enable search and filter functionality, support project integration, and automate workflows via API and webhooks."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-03-12-issue-fields-structured-issue-metadata-is-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-03-12 08:03:06 +00:00
permalink: "/2026-03-12-GitHub-Issue-Fields-Public-Preview-Structured-Metadata-for-Issues.html"
categories: ["DevOps"]
tags: ["Automation", "Custom Fields", "DevOps", "DevOps Tools", "GitHub", "GraphQL API", "Issue Fields", "Issue Management", "News", "Priority Tracking", "Project Management", "Projects & Issues", "Projects Integration", "Release Management", "REST API", "Structured Metadata", "Workflows"]
tags_normalized: ["automation", "custom fields", "devops", "devops tools", "github", "graphql api", "issue fields", "issue management", "news", "priority tracking", "project management", "projects and issues", "projects integration", "release management", "rest api", "structured metadata", "workflows"]
---

Allison reports on GitHub's public preview of issue fields, which introduce structured and customizable metadata for issue tracking, offering enhanced project management for development teams.<!--excerpt_end-->

# GitHub Issue Fields: Structured Issue Metadata in Public Preview

GitHub has introduced issue fields in public preview for select organizations. This new feature replaces label-based workarounds like `priority/p0` with consistent, typed metadata fields for issues across repositories. Benefits include:

- **Four field types**: Single select, text, number, and date, supporting up to 25 fields per organization.
- **Preconfigured fields**: Out of the box, organizations get `Priority`, `Effort`, `Start date`, and `Target date` fields pinned to the appropriate issue types.
- **Customization**: Admins can define which fields appear for each issue type (bug, feature, task, or custom types).
- **Search & filter**: Issues can be found by field value organization-wide, bringing consistency and insight to work tracking.
- **Projects integration**: Issue fields can be columns in private project views, aiding grouping, filtering, and sorting.
- **Timeline events**: Every change to a field is tracked, showing who made the update and when.
- **API and automation**: Full REST and GraphQL API support for managing field settings and values, including new webhook events for `field_added` and `field_removed` compatible with GitHub Actions.

Issue fields are rolling out to a subset of organizations. To request access, participate in the [community discussion](https://github.com/orgs/community/discussions/189141) by sharing your organization's name and use case.

For detailed guidance, check the [official documentation](https://docs.github.com/issues/tracking-your-work-with-issues/using-issues/managing-issue-fields-in-an-organization).

This new capability helps development teams achieve consistent, actionable, and automated issue tracking, improving project and workflow management in GitHub.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-12-issue-fields-structured-issue-metadata-is-in-public-preview)
