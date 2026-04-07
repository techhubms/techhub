---
author: Nick McKenna
date: 2026-04-06 21:53:49 +00:00
external_url: https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-combines-model-families-for-a-second-opinion/
tags:
- Agentic Coding
- AI
- AI & ML
- AI Agents
- Claude Opus
- Claude Sonnet
- Coding Agents
- Cross Model Review
- Experimental Mode
- GitHub Copilot
- GitHub Copilot CLI
- GPT 5.4
- Long Running Tasks
- Model Families
- Multi File Tasks
- News
- Planning Checkpoint
- Rubber Duck
- Slash Commands
- SWE Bench Pro
- Test Coverage
feed_name: The GitHub Blog
primary_section: github-copilot
title: GitHub Copilot CLI adds Rubber Duck for cross-model plan and code review (experimental)
section_names:
- ai
- github-copilot
---

Nick McKenna introduces Rubber Duck in GitHub Copilot CLI experimental mode, a second-model reviewer that critiques an agent’s plan and work at key checkpoints (planning, implementation, and tests) to help prevent confident mistakes from compounding.<!--excerpt_end-->

# GitHub Copilot CLI combines model families for a second opinion

When you ask a coding agent to build something complex (like a data pipeline), it can confidently choose a suboptimal structure early on. Those early decisions then compound as the agent implements, tests, and iterates.

GitHub Copilot CLI now includes **Rubber Duck (experimental)**: a focused review agent that brings a **second AI model from a different model family** to critique the primary agent’s plan and work at high-impact moments.

## The problem: Confident mistakes can compound

Typical coding-agent workflow:

1. Assess the task
2. Draft a plan
3. Implement
4. Test
5. Iterate

This loop works well, but it has a blind spot: **bad assumptions and inefficiencies introduced during planning can become dependencies**, making later fixes more expensive.

Self-review helps, but a model reviewing its own work is constrained by its own training biases and blind spots.

## Rubber Duck adds a second perspective

**Rubber Duck** is designed to act as an **independent reviewer** for your Copilot session, surfacing a short list of high-value concerns:

- Details the primary agent may have missed
- Assumptions worth questioning
- Edge cases to consider

### How the model pairing works (current experiment)

- If you select a **Claude** model as the orchestrator in the model picker, **Rubber Duck will be GPT-5.4**.
- GitHub notes they are experimenting with other model family pairings over time.

## When does cross-family review help?

GitHub evaluated Rubber Duck using **SWE-Bench Pro**, a benchmark of difficult, real-world coding tasks drawn from open-source repositories.

Key result reported:

- **Claude Sonnet 4.6 + Rubber Duck (GPT-5.4)** achieved a resolution rate approaching **Claude Opus 4.6 alone**, closing **74.7% of the performance gap** between Sonnet and Opus.

Observed where it helps most:

- Harder tasks spanning **3+ files**
- Tasks that normally take **70+ steps**

Reported deltas:

- **+3.8%** over the Sonnet baseline on these harder tasks
- **+4.8%** on the hardest problems identified across three trials

Examples of issues Rubber Duck caught:

- **Architectural catch (OpenLibrary/async scheduler)**: proposed scheduler would start and immediately exit (zero jobs); even fixed, a scheduled task was an infinite loop.
- **One-liner bug, big impact (OpenLibrary/Solr)**: a loop overwrote the same `dict` key each iteration, dropping 3 of 4 Solr facet categories silently.
- **Cross-file conflict (NodeBB/email confirmation)**: multiple files read a Redis key that new code stopped writing, silently breaking confirmation UI and cleanup paths on deploy.

## When does Rubber Duck activate?

Copilot can invoke Rubber Duck in three ways:

- **Proactively** at key checkpoints
- **Reactively** if the agent is stuck
- **On demand** when the user requests critique

### Proactive checkpoints

1. **After drafting a plan** (largest expected payoff)
2. **After a complex implementation** (edge-case catching)
3. **After writing tests, before executing them** (test coverage and assertion quality)

### Design choice: invoke sparingly

GitHub states Rubber Duck is called only at the moments where feedback has the highest signal, to avoid getting in the way. Technically, Rubber Duck is invoked through Copilot’s existing **task tool** infrastructure (the same used for other subagents).

## Getting started

Rubber Duck is available in **experimental mode**:

- Experimental mode docs: https://github.com/github/copilot-cli?tab=readme-ov-file#experimental-mode

Steps:

1. Install **GitHub Copilot CLI**: https://github.com/features/copilot/cli?utm_source=blog-cross-model-cta&utm_medium=blog&utm_campaign=copilot-cli-cross-model-march-2026
2. Run the `/experimental` slash command
3. Select any **Claude** model from the model picker
4. Ensure you have access enabled to **GPT-5.4** (required for Rubber Duck in this pairing)

How critiques appear:

- **Automatically** (when Copilot decides a checkpoint warrants a second opinion)
- **On demand** (ask Copilot to critique its work; it will invoke Rubber Duck, incorporate feedback, and show what changed and why)

Where it’s positioned to help most:

- Complex refactors and architectural changes
- High-stakes tasks
- Improving test coverage
- Getting a second opinion on a plan before committing

Feedback link (GitHub discussion): https://github.com/orgs/community/discussions/191734

Source post: https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-combines-model-families-for-a-second-opinion/

[Read the entire article](https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-combines-model-families-for-a-second-opinion/)

