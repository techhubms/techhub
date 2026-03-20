---
section_names:
- ai
- devops
- github-copilot
- security
date: 2026-03-18 19:46:44 +00:00
feed_name: The GitHub Blog
primary_section: github-copilot
external_url: https://github.blog/changelog/2026-03-18-configure-copilot-coding-agents-validation-tools
tags:
- Agentic Coding
- AI
- Code Scanning
- CodeQL
- Copilot
- Copilot Code Review
- Copilot Coding Agent
- DevOps
- GitHub Advanced Security
- GitHub Advisory Database
- GitHub Copilot
- Improvement
- Linter
- News
- Quality Gates
- Repository Settings
- Secret Scanning
- Security
- Security Checks
- Tests
- Validation Tools
title: Configure Copilot coding agent’s validation tools
author: Allison
---

Allison explains how GitHub Copilot’s coding agent runs automated validation (tests, linting, and security checks like CodeQL and secret scanning) and how repo admins can configure which checks the agent executes in repository settings.<!--excerpt_end-->

# Configure Copilot coding agent’s validation tools

When [Copilot coding agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent) writes code, it automatically runs your project’s tests and linter. It also runs GitHub’s security and quality validation tools, including:

- [CodeQL](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql)
- The [GitHub Advisory Database](https://docs.github.com/code-security/security-advisories/working-with-global-security-advisories-from-the-github-advisory-database/about-the-github-advisory-database)
- [Secret scanning](https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning)
- [Copilot code review](https://docs.github.com/copilot/using-github-copilot/code-review/using-copilot-code-review)

If any problems are found, Copilot attempts to resolve them before stopping work and requesting review.

## Defaults, licensing, and why you might change it

These validation tools are:

- Free of charge
- Enabled by default
- Not dependent on a GitHub Advanced Security license

However, teams may still want to disable specific checks in some scenarios (for example, if CodeQL analysis takes too long for a particular repository).

## How to configure the checks

Repository admins can now configure which validation tools the coding agent runs via repository settings:

- Go to **Copilot -> Coding agent** in the repository settings.


[Read the entire article](https://github.blog/changelog/2026-03-18-configure-copilot-coding-agents-validation-tools)

