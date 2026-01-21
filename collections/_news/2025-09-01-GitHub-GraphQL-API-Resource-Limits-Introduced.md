---
external_url: https://github.blog/changelog/2025-09-01-graphql-api-resource-limits
title: GitHub GraphQL API Resource Limits Introduced
author: Allison
feed_name: The GitHub Blog
date: 2025-09-01 16:00:30 +00:00
tags:
- API Best Practices
- API Design
- API Resource Limits
- Developer Integrations
- Error Handling
- GitHub
- GraphQL API
- Partial Responses
- Query Optimization
- Rate Limiting
section_names:
- devops
---
Allison details GitHub's introduction of execution resource limits in the GraphQL API, highlighting implications for developer integrations and guidance on avoiding new query restrictions.<!--excerpt_end-->

# GitHub GraphQL API Resource Limits Introduced

GitHub is rolling out new safeguards for its GraphQL API that cap the execution resources a single query can use. Unlike traditional rate limiters, these resource limits focus on how much server-side computation a query consumes, not the number or frequency of requests.

## What are the new limits?

- **Execution resources, not request count:** Capped per-query resources rather than counting requests.
- **Potential triggers:**
  - Requesting very large numbers of objects or deeply nested relationships.
  - Using large `first` or `last` arguments on multiple connections simultaneously.
  - Fetching exhaustive details, such as all comments, reactions, and related issues for repositories.

## Impact on Developers

- **For most usage:** No changes are expected if queries are efficient.
- **Integration caveats:** Expensive or inefficient queries may start returning partial results with errors indicating resource limits have been exceeded.
- **Recommended Action:** Review your current usages and query patterns, especially if fetching large data sets or deep object relationships, and adjust to fit within the new constraints.

## Further Information

- Learn more at GitHub’s official documentation: [GraphQL API’s rate and query limits](https://docs.github.com/graphql/overview/rate-limits-and-query-limits-for-the-graphql-api)

By introducing these measures, GitHub aims to maintain reliability for all users and encourage efficient API consumption.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-01-graphql-api-resource-limits)
