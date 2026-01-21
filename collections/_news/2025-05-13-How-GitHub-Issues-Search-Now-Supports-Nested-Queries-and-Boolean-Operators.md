---
external_url: https://github.blog/developer-skills/application-development/github-issues-search-now-supports-nested-queries-and-boolean-operators-heres-how-we-rebuilt-it/
title: How GitHub Issues Search Now Supports Nested Queries and Boolean Operators
author: Deborah Digges
feed_name: GitHub Engineering Blog
date: 2025-05-13 16:00:00 +00:00
tags:
- Advanced Search
- API
- Application Architecture
- Application Development
- Architecture & Optimization
- Boolean Operators
- Compatibility
- Developer Skills
- Developer Tools
- Elasticsearch
- Engineering
- Engineering Principles
- GitHub Issues
- Issues Search
- Nested Queries
- Parsing
- Performance
- Search
- User Experience
section_names:
- devops
---
In this article, Deborah Digges explains the major overhaul to GitHub Issues search, introducing nested queries and boolean logic. The post details architectural changes, implementation steps, and risk mitigation for this widely used developer feature.<!--excerpt_end-->

## GitHub Issues Search: Supporting Nested Queries and Boolean Operators

### Introduction

GitHub Issues search has evolved from supporting only flat, simple queries to now allowing developers to construct advanced searches using logical AND/OR operators and nested parentheses. This upgrade provides fine-grained control over issue queries and responds to nearly a decade of community requests.

### New Advanced Search Capabilities

With the introduction of advanced search syntax, users can now:

- Use logical AND/OR operators across all issue fields
- Nest query terms within parentheses for precise searches

Example:

```
is:issue state:open author:rileybroughten (type:Bug OR type:Epic)
```

This finds all open issues authored by 'rileybroughten' that are either bugs or epics.

![Screenshot of an Issues search query using OR](https://github.blog/wp-content/uploads/2025/05/3_issues_search_screenshot.png?resize=1024%2C411)

### From Simple to Sophisticated

Previously, issues search only supported a flat list joined by implicit AND operators (e.g., `assignee:@me label:support new-project`). Over the years, users requested greater flexibility, notably the ability to use OR logic beyond label searches. In 2021, a step forward allowed comma-separated label values, but full boolean queries and support across all fields was still lacking.

### Architectural Overhaul

![Issues Search Architecture](https://github.blog/wp-content/uploads/2025/05/1_architecture.png?resize=1024%2C486)

The core technical change involved replacing the original `IssuesQuery` module with a new `ConditionalIssuesQuery` module. This new module handles recursive, nested queries, maintains backward compatibility, and continues to parse queries into Elasticsearch requests.

![Search Module Architecture](https://github.blog/wp-content/uploads/2025/05/2_search_architecture.png?resize=1024%2C376)

#### Three Main Stages

1. **Parse**: Convert the user's search string into a structured format (AST).
2. **Query**: Translate the AST into an Elasticsearch query.
3. **Normalize**: Map results into Ruby objects (unchanged in the overhaul).

#### Parsing Improvements

The old parser generated a flat list of terms and filters. The new parser, based on the [parslet](https://github.com/kschiess/parslet) library, creates an Abstract Syntax Tree (AST) according to a Parsing Expression Grammar (PEG). This AST supports nested and mixed boolean operators.

Example user search:

```
is:issue AND (author:deborah-digges OR author:monalisa)
```

Sample AST structure (simplified):

```
{
  "root": {
    "and": {
      "left": { ... },
      "right": {
        "or": { ... }
      }
    }
  }
}
```

#### Query Generation & ElasticSearch Integration

Previously, each filter term was mapped linearly using filter classes. The new method recurses through the AST, mapping boolean logic directly to Elasticsearch's bool queries (*must*, *should*, *must_not*).

Example query document snippet:

```
{
  "query": {
    "bool": {
      "must": [
        /* nested logic for AND and OR */
      ]
    }
  }
}
```

### Considerations and Challenges

- **Backward Compatibility:** Extensive testing of both systems, dark-shipping (running new and old systems in parallel for 1% of queries), and bug fixing based on differences ensured a seamless upgrade.
  - "Differences" were initially defined as divergent result counts within a short period.
- **Performance:** More complex queries were anticipated to consume more backend resources. The team compared performance on both platforms using GitHub’s [scientist](https://github.com/github/scientist) library.
- **User Experience:** To avoid overwhelming users, nesting depth was limited to five levels. UI cues highlight logical operators, and familiar autocomplete features remain for queries.
- **Minimizing Risk:** The rollout was gradual—first to the GraphQL API and repository Issues UI tab, then to the Issues dashboard and REST API. Internal and early partner testing guarded against major regressions.

### Try It Out & Further Information

For more details and to try the new functionality, consult the official documentation:

- [Using boolean operators in Issues search](https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/filtering-and-searching-issues-and-pull-requests#using-boolean-operators)
- [Using parentheses for complex filters](https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/filtering-and-searching-issues-and-pull-requests#using-parentheses-for-more-complicated-filters)

Feedback is encouraged via the [community discussions](https://github.com/orgs/community/discussions/categories/announcements).

### Acknowledgements

Thanks to AJ Schuster, Riley Broughten, Stephanie Goldstein, Eric Jorgensen, Mike Melanson, and Laura Lindeman for their feedback and contributions to this development and write-up.

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/developer-skills/application-development/github-issues-search-now-supports-nested-queries-and-boolean-operators-heres-how-we-rebuilt-it/)
