---
layout: "post"
title: "Linter Integration Arrives in Copilot Code Review Public Preview"
description: "Discover how the latest Copilot code review update integrates static analysis tools such as ESLint, PMD, and CodeQL, providing actionable feedback directly in your pull requests. This guide explains how to customize which linters run via repository rulesets and walks you through getting started with configuration for enhanced code quality in GitHub projects."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-11-20-linter-integration-with-copilot-code-review-now-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-11-20 19:29:29 +00:00
permalink: "/news/2025-11-20-Linter-Integration-Arrives-in-Copilot-Code-Review-Public-Preview.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["AI", "Automation", "Code Quality", "CodeQL", "Configuration", "Copilot", "Copilot Code Review", "DevOps", "Enterprise", "ESLint", "GitHub", "GitHub Copilot", "Linters", "News", "PMD", "Pull Requests", "Repository Rulesets", "Static Analysis"]
tags_normalized: ["ai", "automation", "code quality", "codeql", "configuration", "copilot", "copilot code review", "devops", "enterprise", "eslint", "github", "github copilot", "linters", "news", "pmd", "pull requests", "repository rulesets", "static analysis"]
---

Allison details the new GitHub Copilot code review integration with linters, showing how static analysis tools like ESLint, PMD, and CodeQL provide actionable feedback and explaining repository-level configuration.<!--excerpt_end-->

# Linter Integration with Copilot Code Review: Public Preview

GitHub Copilot code review now enhances pull request feedback by integrating industry-standard linters, making it easier to catch minor issues automatically. This public preview allows paid GitHub users to receive actionable suggestions for code improvements from linters and to control which static analysis tools are active through repository rulesets.

## New Static Analysis Tools Supported

Alongside the default CodeQL integration (announced at GitHub Universe 2025), Copilot code review now supports:

- **ESLint:** Catches syntax, style, and programming issues in JavaScript and TypeScript projects.
- **PMD:** Scans for problems in Java, Apex, and other supported languages.

These tools complement CodeQL's security and quality checks, offering a more comprehensive review workflow.

## Customizing Linters via Repository Rulesets

A new repository-level rule, "Manage static analysis tools in Copilot code review," lets administrators select which linters to enable:

- **Enable/disable CodeQL, ESLint, or PMD** as needed.
- Apply settings at various scopes: Enterprise, Organization, Team, or Repository level.
- Fine-tune coverage to target specific branches and repositories.

### Steps to Configure Linters

1. **Navigate to Settings:** In your repository, go to **Settings > Rules > Rulesets**.
2. **Edit or Add a Ruleset:** Select the "Manage static analysis tools in Copilot code review" rule.
3. **Choose Tools:** Specify whether to activate ESLint, PMD, or leave CodeQL enabled by default (note: CodeQL can now be disabled for the first time).
4. **Save and Test:** Save the ruleset, then open a new pull request to experience the updated Copilot code review, complete with custom linter feedback.

_For more information, see the [GitHub documentation](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/request-a-code-review/use-code-review)._

Join the community conversation on [Copilot Conversations](https://github.com/orgs/community/discussions/categories/copilot-conversations) to share your experience or ask questions.

*Disclaimer: UI and available features in public preview may change before general release.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-20-linter-integration-with-copilot-code-review-now-in-public-preview)
