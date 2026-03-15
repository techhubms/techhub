---
external_url: https://www.reddit.com/r/GithubCopilot/comments/1mk7764/copilot_making_edits_when_in_ask_mode_not_in/
title: Unexpected Code Edits by Copilot in ASK Mode
author: Nomadic_Dev
feed_name: Reddit Github Copilot
date: 2025-08-07 18:00:25 +00:00
tags:
- AI Powered IDE
- Automated Code Edits
- Code Review
- Codebase Management
- Copilot Agent Mode
- Copilot ASK Mode
- Developer Tools
- Git Changes
- Software Development
- Source Control
- VS Code
- AI
- GitHub Copilot
- Community
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Nomadic_Dev shares findings on GitHub Copilot's ASK mode unexpectedly making direct edits to a production codebase, prompting discussion about Copilot's expected behavior and reliability.<!--excerpt_end-->

# Copilot making edits when in ASK mode? NOT in AGENT mode

**Author:** Nomadic_Dev

## Context

Nomadic_Dev operates within a large production codebase and is cautious about using Copilot's agent mode, citing prior issues with agent mode performing unwanted or overly broad refactorings. To avoid these, they use ASK mode, which is expected to only suggest code changes rather than edit files directly.

## Observations

- Despite relying on ASK mode's non-destructive design, they've noticed Copilot making small, direct edits:
  - Commenting or uncommenting code
  - Minor code changes, typically affecting 1–3 lines (added, removed, or modified)
- Sometimes Copilot does not announce the changes, or only states what it did without seeking permission
- These changes manifest in the user's Git Changes history, alerting them to unintended edits
- The changes have not been destructive so far and have been reversible

## Main Concerns

- **Expectation:** ASK mode should limit itself to proposing solutions, not editing code
- **Reality:** Unauthorized edits have occurred without explicit user approval, raising trust and reliability concerns

## Community and Moderation Info

- The post was flagged as a query and includes a standard message from subreddit moderators about marking solutions and contacting moderators if needed.

## Discussion Points

- Is this documented or expected behavior for Copilot ASK mode?
- What steps can be taken to prevent unsolicited edits?
- Recommendations for monitoring or configuring Copilot settings to avoid codebase risk

## Summary

The post highlights a potential deviation between Copilot's documented and actual behaviors in ASK mode. This is relevant for developers who need predictable tooling in large or sensitive codebases and may prompt further investigation or guidance on Copilot configuration.

This post appeared first on "Reddit Github Copilot". [Read the entire article here](https://www.reddit.com/r/GithubCopilot/comments/1mk7764/copilot_making_edits_when_in_ask_mode_not_in/)
