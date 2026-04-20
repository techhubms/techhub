---
author: Allison
tags:
- AI
- Automated Remediation
- Code Scanning
- CodeQL
- Copilot
- Copilot Cloud Agent
- Copilot Code Review
- Developer Workflow
- DevOps
- GitHub Advisory Database
- GitHub Copilot
- GitHub Copilot Coding Agent
- GitHub Security Tools
- Improvement
- News
- Parallel Validation
- Quality Checks
- Repository Settings
- Secret Scanning
- Security
- Security Validation
primary_section: github-copilot
date: 2026-04-10 12:19:17 +00:00
title: Copilot cloud agent’s validation tools are now 20% faster
external_url: https://github.blog/changelog/2026-04-10-copilot-cloud-agents-validation-tools-are-now-20-faster
feed_name: The GitHub Blog
section_names:
- ai
- devops
- github-copilot
- security
---

Allison shares a GitHub update: Copilot cloud agent now runs its built-in security and quality validation tools in parallel, cutting validation time by about 20%, while keeping the same checks (CodeQL, secret scanning, Advisory Database, and Copilot code review).<!--excerpt_end-->

# Copilot cloud agent’s validation tools are now 20% faster

When [Copilot cloud agent](https://docs.github.com/copilot/concepts/agents/cloud-agent/about-cloud-agent) writes code, it automatically runs GitHub’s security and quality validation tools, including:

- [CodeQL](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql)
- [GitHub Advisory Database](https://docs.github.com/code-security/security-advisories/working-with-global-security-advisories-from-the-github-advisory-database/about-the-github-advisory-database)
- [Secret scanning](https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning)
- [Copilot code review](https://docs.github.com/copilot/using-github-copilot/code-review/using-copilot-code-review)

If any problems are found, Copilot attempts to resolve them before finishing work and requesting review.

## What changed

These validation tools now run **in parallel** rather than **sequentially**, which reduces validation time by **20%**.

## How to configure validation tools

You can configure which validation tools the cloud agent runs in your repository settings:

- Go to **Copilot → Cloud agent**
- Adjust the validation tools the agent should run

Configuration details: [Configuring agent settings](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/configuring-agent-settings)

## Learn more

- [Copilot cloud agent documentation](https://docs.github.com/copilot/concepts/agents/cloud-agent/about-cloud-agent)


[Read the entire article](https://github.blog/changelog/2026-04-10-copilot-cloud-agents-validation-tools-are-now-20-faster)

