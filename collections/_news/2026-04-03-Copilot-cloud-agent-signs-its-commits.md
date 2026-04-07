---
primary_section: github-copilot
date: 2026-04-03 12:05:23 +00:00
author: Allison
section_names:
- ai
- devops
- github-copilot
feed_name: The GitHub Blog
title: Copilot cloud agent signs its commits
external_url: https://github.blog/changelog/2026-04-03-copilot-cloud-agent-signs-its-commits
tags:
- Agentic Coding
- AI
- Branch Protection Rules
- Commit Signing
- Copilot
- Copilot Cloud Agent
- Copilot Coding Agent
- DevOps
- Git Commit Integrity
- GitHub Copilot
- GitHub Documentation
- Improvement
- News
- Repository Security
- Require Signed Commits
- Rulesets
- Signed Commits
- Software Supply Chain Security
- Verified Badge
---

Allison announces that GitHub Copilot cloud agent now signs every commit it creates, so commits show as Verified and the agent can work in repositories that enforce “Require signed commits” branch protection rules.<!--excerpt_end-->

# Copilot cloud agent signs its commits

[Copilot cloud agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent) now signs every commit it makes.

## What changed

- Every commit created by Copilot cloud agent is now **signed**.
- On GitHub, these signed commits show as **`Verified`**, which helps confirm the commit genuinely came from the agent and hasn’t been tampered with.

## Why it matters

This update means Copilot cloud agent can now be used in repositories that enforce signed-commit policies, including:

- The **“Require signed commits”** branch protection rule or ruleset
  - Docs: [Require signed commits](https://docs.github.com/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/available-rules-for-rulesets#require-signed-commits)

Previously, if a repository required signed commits, Copilot cloud agent couldn’t comply, which **blocked it from being used** in those repositories.

## Learn more

- GitHub Docs: [About Copilot cloud agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent)


[Read the entire article](https://github.blog/changelog/2026-04-03-copilot-cloud-agent-signs-its-commits)

