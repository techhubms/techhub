---
external_url: https://devblogs.microsoft.com/visualstudio/improving-codebase-awareness-in-visual-studio-chat/
title: Enhancing Visual Studio Copilot Chat with Semantic Code Search
author: Pablo Gonzalez
feed_name: Microsoft VisualStudio Blog
date: 2025-08-14 12:00:59 +00:00
tags:
- AI Models
- Azure DevOps
- BM25
- Code Search
- Codebase Awareness
- Contextual Search
- Copilot Chat
- Developer Productivity
- GitHub Copilot Chat
- GitHub Integration
- Microsoft
- Release 17.14.11
- Remote Semantic Search
- Semantic Search
- Vector Embeddings
- VS
section_names:
- ai
- coding
- github-copilot
---
Pablo Gonzalez details how Visual Studio Copilot Chat now leverages semantic AI search, improving codebase comprehension and search relevance for developers through advanced contextual understanding.<!--excerpt_end-->

# Enhancing Visual Studio Copilot Chat with Semantic Code Search

**Author:** Pablo Gonzalez

## Introduction

Visual Studio Copilot Chat has integrated Remote Semantic Search in its 17.14.11 release, offering a smarter way to search and understand code by moving beyond pure keyword matching. This enhancement uses AI-driven semantic models, yielding significantly more relevant results within your codebase hosted on Azure DevOps or GitHub.

## From BM25 Keyword Search to Semantic AI

Previously, Visual Studio’s code search relied on the BM25 algorithm, which ranks results based on keyword frequency and document length. While effective for simple searches, BM25 doesn’t recognize synonyms, intent, or deeper code meanings.

- **Limitations of BM25**:
  - Purely keyword-based; lacks understanding of concepts
  - Misses relevant code with different naming (e.g., does not link “get user authentication token” with “RetrieveOAuthCredential”)

## The Upgrade: Introducing Semantic Search

Semantic Search transforms queries and code into high-dimensional vectors for meaningful comparison.

- **Benefits:**
  - Matches concepts and intent, not just words
  - Understands related phrases (e.g., “fetch user credentials” ≈ “get authentication token”)
  - Recognizes intent and context in code and comments

## How to Use in Visual Studio

Remote Semantic Search is now enabled for Azure DevOps and GitHub repos that have been indexed. To access the feature:

1. Open the Copilot Chat window in Visual Studio
2. Use the `#solution` feature with natural language queries (e.g., “#solution Where are the API requests?” or “Where is Authentication Handled? #solution”)
3. Semantic Search interprets whole sentences and developer intent

For more details about GitHub integration, visit [GitHub Code Search Documentation](https://docs.github.com/en/search-github/github-code-search/about-github-code-search).

## Comparative Example: Classic vs. Semantic Search

- **BM25 Example:** Returns files containing literal keywords, sometimes returning less relevant or test files.
- **Semantic Search Example:** Returns code matching the actual intent, such as correlating “deleting not synchronous modifiers” with “removing Async modifiers”, regardless of phrasing differences.

| BM25 Results | Semantic Results |
|--------------|-----------------|
| Fuzzy keyword matches, can include unrelated files like tests | Intent-based, precise matches, ignoring surface-level keyword noise |

## Conclusion

Remote Semantic Search in Visual Studio Copilot represents a major step in codebase awareness. By merging keyword and AI contextual search, developers can now find code faster and more intuitively, saving time and reducing frustration when working across large or unfamiliar projects.

**Give it a try** to make your codebase searches smarter and more productive.

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/improving-codebase-awareness-in-visual-studio-chat/)
