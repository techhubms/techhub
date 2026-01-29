---
external_url: https://github.blog/changelog/2025-10-28-new-public-preview-features-in-copilot-code-review-ai-reviews-that-see-the-full-picture
title: 'New Public Preview Features in Copilot Code Review: Smarter, AI-Driven Code Reviews'
author: Allison
feed_name: The GitHub Blog
date: 2025-10-28 15:27:44 +00:00
tags:
- AI Code Review
- Automation
- CodeQL
- Copilot
- Copilot Code Review
- Custom Workflows
- Developer Tools
- ESLint
- JetBrains
- LLM
- Pull Requests
- Quality Assurance
- Static Analysis
- Universe25
- VS
- VS Code
- Xcode
- AI
- Coding
- DevOps
- GitHub Copilot
- Security
- News
section_names:
- ai
- coding
- devops
- github-copilot
- security
primary_section: github-copilot
---
Allison introduces major upgrades to GitHub Copilot Code Review, showing how AI-powered and deterministic tools combine for smarter security and quality insights.<!--excerpt_end-->

# New Public Preview Features in Copilot Code Review: AI Reviews that See the Full Picture

**Author:** Allison

GitHub Copilot Code Review (CCR) now brings together LLM-based AI detections and deterministic tools like ESLint and CodeQL. This upgrade provides developers with richer, more actionable feedback, helping teams ship better code faster and with fewer manual steps.

## Rich Context Through Agentic Tool Calling

CCR uses agentic tool calling to gather the full context—code, directory structure, and references—enabling reviews that truly reflect how changes fit into your entire project.

- *Benefit:* More specific feedback, less noise, and a deeper understanding of code changes.

## Deterministic Detections: CodeQL & ESLint

By integrating CodeQL and ESLint, CCR combines semantic AI analysis with classic rule-based checks to catch security and maintainability issues. This fusion means:

- High-signal, consistent findings
- Security vulnerabilities identified and clearly explained
- Code quality issues reliably surfaced

**Unique Advantage:** Only GitHub offers CodeQL-powered insights directly within AI-driven code reviews.

## Seamless Handoff to Copilot Coding Agent

After receiving AI-enhanced recommendations, you can pass them directly to the Copilot coding agent. Mentioning `@copilot` in your pull request creates a stacked PR with fixes applied, ready for your review and merge.

- *Benefit:* Fewer manual changes and review cycles
- Stay focused on high-value engineering work

## Customizable Workflows & Multi-Editor Support

- Define review standards and tone with `instructions.md` or `copilot-instructions.md`
- CCR is available on VS Code, Visual Studio, JetBrains, Xcode, and github.com
- Ensures consistent feedback across your team's tools

## Getting Started

- Tool calling and deterministic detections are in public preview
- Available by default for Copilot Pro and Copilot Pro Plus users
- Copilot Business and Enterprise users can opt in via policy settings

For more dialogue and feedback, join the ongoing [GitHub Community discussion](https://github.com/orgs/community/discussions/177790?utm_source=changelog-community-copilot-code-review&utm_medium=changelog&utm_campaign=universe25).

*Disclaimer: UI and features in public preview may change.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-28-new-public-preview-features-in-copilot-code-review-ai-reviews-that-see-the-full-picture)
