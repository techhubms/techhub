---
layout: "post"
title: "Ask Copilot Coding Agent to Make Changes in Any Pull Request with @copilot"
description: "This news update announces a new feature for GitHub Copilot: users can now ask Copilot to make changes to existing pull requests by mentioning @copilot in a comment. The Copilot coding agent works asynchronously, creating a new pull request on top of the existing branch and requests review, while users maintain full control over their original branch. The feature is available to Copilot Pro, Pro+, Business, and Enterprise subscribers, with additional admin setup required for Business and Enterprise users."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-10-28-ask-copilot-coding-agent-to-make-changes-in-any-pull-request-with-copilot"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-10-28 15:20:57 +00:00
permalink: "/news/2025-10-28-Ask-Copilot-Coding-Agent-to-Make-Changes-in-Any-Pull-Request-with-copilot.html"
categories: ["AI", "GitHub Copilot"]
tags: ["@copilot", "AI", "Automation", "Code Review", "Copilot", "Copilot Business", "Copilot Coding Agent", "Feature Update", "GitHub", "GitHub Copilot", "GitHub Enterprise", "GitHub Pro", "Improvement", "News", "Pull Requests", "Software Development", "Universe25", "Version Control"]
tags_normalized: ["atcopilot", "ai", "automation", "code review", "copilot", "copilot business", "copilot coding agent", "feature update", "github", "github copilot", "github enterprise", "github pro", "improvement", "news", "pull requests", "software development", "universe25", "version control"]
---

Allison shares the latest GitHub Copilot feature, allowing users to delegate changes to existing pull requests by simply mentioning @copilot, making code collaboration more efficient.<!--excerpt_end-->

# Ask Copilot Coding Agent to Make Changes in Any Pull Request with @copilot

GitHub Copilot has introduced an enhancement that lets you ask the Copilot coding agent to make changes to any active pull request (PR) by mentioning `@copilot` in a comment. This capability builds on Copilot's existing support for opening new PRs and now extends its assistance into modifying ongoing code reviews.

## How It Works

- **Delegate a task:** Mention `@copilot` in a comment on an existing pull request.
- **Autonomous editing:** Copilot, acting as an asynchronous background agent, will create a new pull request on top of your active PR, using the same branch as the base.
- **Review cycle:** After completing the requested changes, Copilot asks for your review by opening the new PR, making it clear that the changes merge into your ongoing work.
- **Full control:** Your original PR remains untouched until you choose to merge Copilot’s proposed updates into your branch. Pull requests merging into another pull request are now visually marked as such for clarity.

![Screenshot of a pull request with the 'Parent' tag, showing that it merges into another pull request's branch](https://github.com/user-attachments/assets/121e3d3f-0df5-44ea-9626-a28cd54bc227)

## Availability and Setup

- The Copilot coding agent feature is available to Copilot Pro, Copilot Pro+, Copilot Business, and Copilot Enterprise subscribers.
- For Copilot Business and Enterprise users, an administrator must enable Copilot coding agent from the [Policies page](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent/enabling-copilot-coding-agent?utm_source=changelog-docs-mention-copilot-in-pr&utm_medium=changelog&utm_campaign=universe25) prior to use.

## Learn More

- Full documentation on the Copilot coding agent is available [here](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent?utm_source=changelog-docs-mention-copilot-in-pr&utm_medium=changelog&utm_campaign=universe25).

---

**Author:** Allison

With these improvements, Copilot increases developer productivity and makes pull request workflows even more streamlined on GitHub.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-28-ask-copilot-coding-agent-to-make-changes-in-any-pull-request-with-copilot)
