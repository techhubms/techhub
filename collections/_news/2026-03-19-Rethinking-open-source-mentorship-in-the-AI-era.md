---
external_url: https://github.blog/open-source/maintainers/rethinking-open-source-mentorship-in-the-ai-era/
author: Abigail Cabunoc Mayes
primary_section: ai
section_names:
- ai
- devops
date: 2026-03-19 18:00:00 +00:00
title: Rethinking open source mentorship in the AI era
tags:
- AGENTS.md
- AI
- AI Assisted Coding
- AI Disclosure Policy
- Code Review
- Community
- Contribution Guidelines
- Contributor Onboarding
- DevOps
- GitHub Community
- Governance
- LLMs
- Maintainer Burnout
- Maintainers
- Mentoring
- Mentorship
- News
- Open Source
- Open Source Maintainers
- Pull Requests
- Review Workflow
- RFC
feed_name: The GitHub Blog
---

Abigail Cabunoc Mayes (GitHub Blog) explains how AI-assisted contributions are changing the signals maintainers use to decide where to spend review and mentorship time, and proposes the “3 Cs” framework—Comprehension, Context, and Continuity—to mentor more strategically without burning out.<!--excerpt_end-->

# Rethinking open source mentorship in the AI era

Abigail Cabunoc Mayes describes a common maintainer experience: a polished pull request (PR) arrives, the maintainer spends significant time reviewing and asking clarifying questions, and then either the contributor disappears or it becomes clear they can’t explain the change—often because an LLM helped generate something that looked plausible.

The core problem is:

- **The cost to create has dropped** (AI can generate plausible code quickly).
- **The cost to review hasn’t** (maintainers still pay the full review/debugging cost).

This creates a strain similar to an open source “Eternal September”: constant newcomer volume that overwhelms the social systems communities rely on for trust and mentorship.

## The signals have changed

Historically, maintainers could treat certain signals as evidence that a contributor had invested in understanding the project:

- Clean code
- Fast turnaround
- Handling complexity

AI can now produce outputs that mimic those signals without the underlying understanding.

Examples cited in the post:

- tldraw closed their pull requests: <https://github.com/tldraw/tldraw/issues/7695>
- Fastify shut down their HackerOne program due to unmanageable volume: <https://github.com/fastify/fastify/issues/6468>
- The Octoverse 2025 report notes nearly **45 million PRs merged per month in 2025** (up 23% YoY): <https://github.blog/news-insights/octoverse/octoverse-a-new-developer-joins-github-every-second-as-ai-leads-typescript-to-1/>

GitHub is also exploring longer-term platform solutions and published an RFC for feedback:

- GitHub RFC discussion: <https://github.com/orgs/community/discussions/185387>

But even with platform changes, maintainers still need practical strategies for mentorship decisions.

## Why this is urgent

The post argues mentorship is how open source communities scale:

- A good mentor is often how contributors get started.
- Mentoring effectively creates a “multiplier effect” (mentees go on to onboard others).

A key risk is **maintainer burnout** from trying to mentor everyone. If mentorship collapses, communities lose the multiplier effect.

## The 3 Cs: a framework for strategic mentorship at scale

To decide where to invest mentorship energy when traditional signals are unreliable, Mayes proposes three filters: **Comprehension, Context, and Continuity**.

### 1) Comprehension

**Question:** *Do they understand the problem well enough to propose this change?*

Patterns the author is seeing:

- Some projects require contributors to **open an issue and get approval before submitting a PR**, using the issue discussion as a comprehension check.
  - Examples mentioned: OpenAI Codex and Google Gemini CLI guidelines.
- In-person code sprints and hackathons can help maintainers assess comprehension and interest through real-time conversation.

The bar isn’t “understand the whole project,” but “don’t commit code above your own comprehension level.”

### 2) Context

**Question:** *Do they give me what I need to review this well?*

Comprehension is about the contributor’s understanding; **Context** is about the reviewer’s ability to review efficiently and accurately.

Signals of good context include:

- Linking to the issue
- Explaining trade-offs
- Disclosing AI use

AI disclosure examples referenced:

- ROOST AI-assisted coding policy: <https://github.com/roostorg/.github/blob/main/CONTRIBUTING.md#ai-assisted-coding-policy>
- Fedora AI contribution policy: <https://docs.fedoraproject.org/en-US/council/policy/ai-contribution-policy>

The author also points to **`AGENTS.md`** as a way to provide instructions to AI coding agents (compared to robots.txt), shifting some of the “provide context” burden onto contributors and their tools.

Examples mentioned:

- Processing `AGENTS.md`: <https://github.com/processing/processing4/blob/main/AGENTS.md>
- scikit-learn PR template reference: <https://github.com/scikit-learn/scikit-learn/blob/main/.github/PULL_REQUEST_TEMPLATE.md>
- Goose `HOWTOAI.md`: <https://github.com/block/goose/blob/main/HOWTOAI.md>

### 3) Continuity

**Question:** *Do they keep coming back?*

This is positioned as the primary mentorship filter:

- Drive-by contributions can still be useful.
- Deep mentorship investment should go to contributors who repeatedly engage thoughtfully.

The post describes a progression:

- Great first PR conversation → make review a teachable moment
- They keep coming back → offer pairing, suggest harder tasks
- Continued engagement → invite to events or consider commit access

## The takeaway

> Comprehension and Context get you reviewed. Continuity gets you mentored.

A suggested decision flow:

```text
PR Lands → Follows Guidelines?
  NO  → Close. Guilt-free.
  YES → Review → They Come Back?
            YES → Consider Mentorship
```

The author argues this approach:

- Protects maintainer time (and reduces burnout)
- Preserves the mentorship multiplier effect
- Reduces bias by using explicit criteria instead of “vibes”

## Getting started (practical implementations)

Pick one “C” to implement first:

- **Comprehension**
  - Require issue before pull request
  - Host an in-person code sprint for live discussions
- **Context**
  - Add AI disclosure expectations
  - Add `AGENTS.md`
- **Continuity**
  - Watch who comes back

The post emphasizes the goal isn’t to ban AI-assisted contributions, but to add guardrails that keep human mentorship viable.

## Resources (from the post)

- GitHub RFC for platform-level solutions: <https://github.com/orgs/community/discussions/185387>
- OpenAI Codex contribution policy: <https://github.com/openai/codex/discussions/9956>
- Google Gemini CLI contribution policy: <https://github.com/google-gemini/gemini-cli/discussions/16706>
- ROOST AI policy: <https://github.com/roostorg/.github/blob/main/CONTRIBUTING.md#ai-assisted-coding-policy>
- Fedora AI contribution policy: <https://docs.fedoraproject.org/en-US/council/policy/ai-contribution-policy>
- Processing `AGENTS.md`: <https://github.com/processing/processing4/blob/main/AGENTS.md>
- scikit-learn pull request template: <https://github.com/scikit-learn/scikit-learn/blob/main/.github/PULL_REQUEST_TEMPLATE.md>
- Goose `HOWTOAI.md`: <https://github.com/block/goose/blob/main/HOWTOAI.md>


[Read the entire article](https://github.blog/open-source/maintainers/rethinking-open-source-mentorship-in-the-ai-era/)

