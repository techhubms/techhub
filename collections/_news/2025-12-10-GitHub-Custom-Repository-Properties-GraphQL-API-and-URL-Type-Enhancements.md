---
layout: "post"
title: "GitHub Custom Repository Properties: GraphQL API and URL Type Enhancements"
description: "This news update highlights two enhancements to GitHub's custom repository properties: programmatic management via the GraphQL API and a new URL property type with validation. These features help organizations better organize and automate repository governance at scale, and support integration with external resources like Azure Kubernetes Fleet portals."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-12-09-repository-custom-properties-graphql-api-and-url-type"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-12-10 05:27:55 +00:00
permalink: "/news/2025-12-10-GitHub-Custom-Repository-Properties-GraphQL-API-and-URL-Type-Enhancements.html"
categories: ["DevOps"]
tags: ["API Integration", "Automation", "Azure Kubernetes Fleet", "DevOps", "Enterprise Management", "Enterprise Management Tools", "GitHub", "GraphQL API", "Improvement", "News", "Platform Governance", "Repository Properties", "URL Property", "Validation", "Workflow Automation"]
tags_normalized: ["api integration", "automation", "azure kubernetes fleet", "devops", "enterprise management", "enterprise management tools", "github", "graphql api", "improvement", "news", "platform governance", "repository properties", "url property", "validation", "workflow automation"]
---

Allison reports on GitHub's new features for repository custom properties, including GraphQL API management and robust URL type validation for improved organizational workflows.<!--excerpt_end-->

# GitHub Custom Repository Properties: GraphQL API and URL Type Enhancements

GitHub has introduced two new capabilities for custom repository properties, designed to make repository management and organization more scalable:

## 1. Manage Custom Properties via GraphQL

GitHub repositories can now have their custom properties created, updated, and deleted programmatically using the GraphQL API. This lets administrators and developers integrate property management into their automation routines and custom workflows.

- **Programmatic Access:** Create, update, and delete repository properties using GraphQL queries and mutations.
- **Workflow Integration:** Seamlessly add repository metadata management to CI/CD pipelines and DevOps automations.
- [Learn more about the GraphQL API for custom properties](https://docs.github.com/en/graphql/reference).
- *Note: Documentation updates for GraphQL may take up to 24 hours to become available.*

## 2. New URL Property Type

There is now a built-in `URL` property type that provides automatic validation to ensure all property values are well-formed URLs. This feature makes it simpler to associate repositories with external resources:

- **Validated URLs:** Ensures uniformity and prevents broken links by checking URL formatting.
- **Attach Resources:** Easily link runbooks, dashboards, or documentation (such as the Microsoft Azure Kubernetes Fleet portal) directly to repositories.
- **Organization Consistency:** Admins can manage property values across all repositories for standard governance.

For more information, check GitHub's [custom repository properties documentation](https://docs.github.com/organizations/managing-organization-settings/managing-custom-properties-for-repositories-in-your-organization).

Join the [community discussion](https://gh.io/props-gql-url-discussion) to provide feedback and ideas.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-09-repository-custom-properties-graphql-api-and-url-type)
