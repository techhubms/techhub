---
external_url: https://github.blog/changelog/2026-04-02-improved-search-for-github-issues-is-now-generally-available
primary_section: devops
section_names:
- devops
date: 2026-04-02 13:11:31 +00:00
title: Improved search for GitHub Issues is now generally available
feed_name: The GitHub Blog
tags:
- /search/issues Endpoint
- DevOps
- GitHub Issues
- GraphQL API
- Hybrid Search
- Improvement
- Issue Indexing
- Issue Templates
- Issues Dashboard
- Lexical Search
- Mermaid Diagrams
- Natural Language Search
- News
- Org Qualifier
- Projects & Issues
- Rate Limiting
- Repo Qualifier
- REST API
- Search Qualifiers
- Search Relevance
- Semantic Search
- User Qualifier
author: Allison
---

Allison announces general availability of improved GitHub Issues search, including semantic and hybrid search modes, best-match sorting, and new REST/GraphQL API access for integrating issue search into developer tools and workflows.<!--excerpt_end-->

# Improved search for GitHub Issues is now generally available

Finding the right issue just got easier. First introduced in public preview in January and expanded to the Issues dashboard in February, improved search for GitHub Issues is now generally available (GA).

This release indexes issue titles and bodies so you can find issues by meaning (semantic intent), not just keywords.

## What’s included in this release

- **Natural language search across issues**: Describe what you’re looking for in plain language and GitHub returns conceptually related results, even when the wording doesn’t match.
- **Issues index and dashboard**: Semantic search works both within a single repository and across your repositories on the [issues dashboard](https://github.com/issues).
- **Hybrid search**: When you search with natural language, GitHub combines semantic and keyword matching in the same query.
  - Searches using only filters or quotation marks use traditional lexical search for precision.
- **Best match sorting**: Results are ordered by relevance by default.
- **API access**: Semantic search is now available through the REST and GraphQL APIs, so you can integrate it into your own tools and workflows.

## Observed results since preview

- When users search successfully, the desired result is in the top three issues shown **75% of the time**, compared to **66%** with traditional search.

## API details

### REST API

Use the existing [`/search/issues`](https://docs.github.com/rest/search/search) endpoint with:

- `search_type=semantic`, or
- `search_type=hybrid`

Notes:

- The response indicates which search was performed.
- If there was a fallback to lexical search, the response indicates **why**.
- If you don’t specify a search type, GitHub performs a **lexical** search by default.

You can scope queries using qualifiers such as:

- `org:`
- `user:`
- `repo:`

Rate limits:

- Semantic and hybrid queries: **10 requests per minute**
- Standard lexical searches: retain existing rate limits

### GraphQL API

Use the `searchType` argument on the search query:

- `SEMANTIC`
- `HYBRID`

See documentation:

- [REST](https://docs.github.com/rest/search/search)
- [GraphQL](https://docs.github.com/graphql/reference/queries#search)

## Other improvements to GitHub Issues

- The issue template editor now preserves the `Type` field when editing templates from the UI.
- When using filters in issues search, tagging assignees with `@` in front of their handle now works as expected.
- Comma-separated `repo:`, `org:`, and `user:` qualifiers no longer return errors on the Issues dashboard.
- Mermaid diagrams inside collapsed blocks now render correctly.

## Feedback

Share your feedback in the [GitHub Community](https://github.com/orgs/community/discussions/190865).


[Read the entire article](https://github.blog/changelog/2026-04-02-improved-search-for-github-issues-is-now-generally-available)

