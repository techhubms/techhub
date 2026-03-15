---
title: 'Coming Soon: Revolutionary AI Feature'
author: Test Author
date: 2026-02-01
primary_section: github-copilot
section_names:
- ai
- github-copilot
tags:
- copilot
- draft
- AI
- GitHub Copilot
external_url: https://example.com/draft-feature
feed_name: Test Feed
draft: true
---
This is a draft article used for testing the draft filtering functionality.

<!--excerpt_end-->

## Draft Content

This content should NEVER be returned by the API unless the frontend explicitly requests content with `includeDraft=true`.

Draft content is typically used for:

- Unreleased features
- Upcoming announcements
- Content in review
- Placeholder items

## Testing Requirements

API endpoints must:

1. Filter out draft items by default
2. Only include drafts when explicitly requested
3. Apply this rule consistently across all endpoints
