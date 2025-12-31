---
layout: "post"
title: "GitHub Introduces 'enterprise:' Qualifier for Enhanced Code Search"
description: "This news update highlights the general availability of the new 'enterprise:' qualifier in GitHub code search. The qualifier enables users to search across all organizations within a GitHub enterprise, streamlining previously complex multi-org search queries. The post explores how the new qualifier works, links to relevant documentation, and clarifies that this feature is available for GitHub Enterprise Cloud customers. Technical users and administrators managing codebases at scale will benefit from a more efficient code search experience."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-11-05-enterprise-qualifier-is-now-generally-available-in-github-code-search"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-11-05 19:49:17 +00:00
permalink: "/news/2025-11-05-GitHub-Introduces-enterprise-Qualifier-for-Enhanced-Code-Search.html"
categories: ["DevOps"]
tags: ["Boolean Operators", "Code Search", "Codebase Management", "Collaboration Tools", "DevOps", "Documentation", "Enterprise Qualifier", "GitHub", "GitHub Enterprise Cloud", "Improvement", "News", "Query Operators", "Search Syntax"]
tags_normalized: ["boolean operators", "code search", "codebase management", "collaboration tools", "devops", "documentation", "enterprise qualifier", "github", "github enterprise cloud", "improvement", "news", "query operators", "search syntax"]
---

Allison details the rollout of the 'enterprise:' qualifier in GitHub code search, a new feature making it easier for GitHub Enterprise Cloud customers to search codes across multiple organizations within their enterprise.<!--excerpt_end-->

# GitHub Introduces 'enterprise:' Qualifier for Enhanced Code Search

GitHub has rolled out a new `enterprise:` qualifier in its code search functionality, aimed at simplifying how users search across multiple organizations within a GitHub enterprise. This addition addresses previous challenges where users needed to manually combine several `org:` qualifiers using boolean operators, a time-consuming process especially for enterprises managing hundreds or thousands of organizations.

## How the 'enterprise:' qualifier works

- Add the `enterprise:` qualifier to your GitHub code search queries to automatically include all organizations within your enterprise.
- This streamlines searches that, until now, required complex combinations of multiple `org:` qualifiers.

[See the official documentation for an example.](https://docs.github.com/enterprise-cloud@latest/search-github/github-code-search/understanding-github-code-search-syntax#enterprise-qualifier)

- The new qualifier integrates with existing GitHub code search qualifiers, following their syntax and limitations.
- This functionality is generally available for all GitHub Enterprise Cloud customers.

## Key points

- **Simplifies enterprise-wide code search**: No need to manually list all organizations for broad queries.
- **Subject to existing search limitations**: Behaves as other code search qualifiers do, ensuring consistent usage and expected results.

For more details and examples, consult the [GitHub documentation](https://docs.github.com/enterprise-cloud@latest/search-github/github-code-search/understanding-github-code-search-syntax#enterprise-qualifier).

---

*Author: Allison*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-05-enterprise-qualifier-is-now-generally-available-in-github-code-search)
