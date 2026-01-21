---
external_url: https://github.blog/changelog/2025-11-20-linter-integration-with-copilot-code-review-now-in-public-preview
title: Linter Integration Arrives in Copilot Code Review Public Preview
author: Allison
feed_name: The GitHub Blog
date: 2025-11-20 19:29:29 +00:00
tags:
- Automation
- Code Quality
- CodeQL
- Configuration
- Copilot
- Copilot Code Review
- Enterprise
- ESLint
- GitHub
- Linters
- PMD
- Pull Requests
- Repository Rulesets
- Static Analysis
section_names:
- ai
- devops
- github-copilot
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

_Disclaimer: UI and available features in public preview may change before general release._

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-20-linter-integration-with-copilot-code-review-now-in-public-preview)
