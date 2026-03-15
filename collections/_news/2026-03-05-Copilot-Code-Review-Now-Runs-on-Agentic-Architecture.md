---
external_url: https://github.blog/changelog/2026-03-05-copilot-code-review-now-runs-on-an-agentic-architecture
title: Copilot Code Review Now Runs on Agentic Architecture
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-05 19:36:13 +00:00
tags:
- Agentic Architecture
- AI
- Code Review Automation
- Copilot
- Copilot Code Review
- Developer Productivity
- DevOps
- GitHub Actions
- GitHub Copilot
- News
- Pro Code Review
- Pull Request Automation
- Self Hosted Runners
- Tool Calling Architecture
section_names:
- ai
- devops
- github-copilot
---
Allison highlights the launch of Copilot Code Review's agentic architecture, now available across Copilot Pro, Business, and Enterprise. This update brings actionable, context-aware code review feedback through GitHub Actions and supports both GitHub-hosted and self-hosted runners.<!--excerpt_end-->

# Copilot Code Review Now Runs on Agentic Architecture

Copilot Code Review has introduced an agentic tool-calling architecture, now generally available for all Copilot Pro, Copilot Pro+, Copilot Business, and Copilot Enterprise users.

## What’s New?

With agentic tool-calling, Copilot Code Review can:

- Gather deeper repository context, such as relevant code, directory structure, and references.
- Deliver higher-quality review findings that prioritize correctness and architectural soundness.
- Reduce noise, ensuring review comments surface meaningful and actionable insights.
- Provide more actionable guidance for resolving issues quickly and confidently.

This agentic architecture lets Copilot's reviews reflect how your changes fit into the larger system, giving targeted, high-signal comments for cleaner and faster merges.

For more background:

- [Previous public preview announcement](https://github.blog/changelog/2025-10-28-new-public-preview-features-in-copilot-code-review-ai-reviews-that-see-the-full-picture/)
- [Agentic architecture overview](https://github.blog/ai-and-ml/github-copilot/60-million-copilot-code-reviews-and-counting/)

## Setup for Self-Hosted Runners

Agentic Copilot Code Review runs on GitHub Actions. If you use self-hosted runners (not the ones hosted by GitHub), a [one-time setup process is required](https://docs.github.com/copilot/how-tos/use-copilot-agents/request-a-code-review/configure-self-hosted-runners) to enable agentic reviews for pull requests. Otherwise, for those using GitHub-hosted runners—including [larger runners](https://docs.github.com/actions/concepts/runners/larger-runners#about-larger-runners)—no additional configuration is needed.

Review the official [Copilot Code Review documentation](https://docs.github.com/copilot/how-tos/use-copilot-agents/request-a-code-review/use-code-review?tool=webui) to get started.

## Share Your Feedback

Join the broader discussion and share experiences within the [GitHub Community](https://github.com/orgs/community/discussions/186303).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-05-copilot-code-review-now-runs-on-an-agentic-architecture)
