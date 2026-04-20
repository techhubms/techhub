---
primary_section: ai
external_url: https://www.devclass.com/development/2026/04/16/github-invokes-spirit-of-phabricator-with-preview-of-stacked-prs/5217921
feed_name: DevClass
title: GitHub invokes spirit of Phabricator with preview of Stacked PRs
section_names:
- ai
- devops
tags:
- AI
- AI Agents
- Blogs
- Branching Strategy
- Code Review
- Developer Workflow
- DevOps
- Differential
- Gh Stack
- GitHub
- GitHub CLI
- Merge Order
- Phabricator
- Phorge
- Private Preview
- Pull Requests
- Stacked Diffs
- Stacked PRs
author: DevClass.com
date: 2026-04-16 07:24:15 +00:00
---

DevClass.com reports on GitHub’s private preview of Stacked PRs, a workflow for breaking large changes into smaller, independently reviewable pull requests that can still depend on each other, with an optional gh stack CLI that’s also intended to work well with AI agents.<!--excerpt_end-->

## Overview

GitHub has unveiled **Stacked PRs**, a feature intended to make large pull requests easier to review and to move through delivery pipelines faster.

Stacked PRs are currently in **private preview**: https://github.github.com/gh-stack/

## What “Stacked PRs” are

With Stacked PRs, a pull request can be **based on a previous pull request**, forming a **stack**:

- Each PR in the stack can be **reviewed and merged independently**.
- A PR can be merged only after **all PRs below it** in the stack have been merged.
- It’s also possible to **merge the whole stack together**.

## Why GitHub is doing this

The main goal is to encourage **smaller pull requests**, which are easier to review.

GitHub’s documentation recommends:

> Each branch in a stack should represent a discrete, logical unit of work that can be reviewed independently.

Docs reference: https://github.github.com/gh-stack/introduction/overview/#thinking-about-stack-structure

### The problem with “small PRs” in practice

Developers often want to keep working even when new work depends on earlier changes that aren’t merged yet. Waiting for review/merge after every small chunk can slow work down, so teams often:

- keep building on a separate branch until the feature is complete
- then open a large PR touching many files

Large PRs are harder to review and can become a bottleneck.

## How it’s used (workflow differences)

Using Stacked PRs changes the workflow compared to a single branch + one PR:

- The **bottom PR** in a stack is usually based on the **main branch**, not an isolated feature branch.
- Later PRs build on earlier PRs, rather than stacking commits in one giant review.

A referenced explainer by Jackson Gabbard (ex-Facebook) discusses the workflow in more detail:

- “Stacked diffs versus pull requests”: https://jg.gg/2018/09/29/stacked-diffs-versus-pull-requests/

## Relationship to Phabricator / “stacked diffs”

The idea is not new: other systems have long supported this workflow, often called **stacked diffs**.

- A well-known implementation was **Differential**, created in 2007 by Evan Priestley and Luke Shepard at Facebook.
- Differential became part of **Phabricator**, released as open source in 2011.
- Open source Phabricator ceased development in 2021, but a fork called **Phorge** is actively maintained: https://we.phorge.it/

Project history reference: https://secure.phabricator.com/book/phabflavor/article/project_history/

## Tooling: gh stack CLI (optional)

A Hacker News discussion noted that newer Git features can support similar workflows, questioning the need for a GitHub-specific CLI.

The feature includes an optional GitHub extension:

- **gh stack** (GitHub CLI extension)

GitHub engineer Sameen Karim said the CLI is optional and stacked PRs can be created via the GitHub UI.

## AI angle

GitHub has positioned the feature partly around improving throughput when code review is the bottleneck:

- “The bottleneck is no longer writing code – it's reviewing it. Stacks help solve that,” said Sameen Karim.
- Karim also said the **stack CLI is designed for use by AI agents**.

LinkedIn reference (as cited in the article): https://www.linkedin.com/posts/sameenkarim_excited-to-share-what-ive-been-working-on-activity-7448411477482430465-SYiT

## Image

![Managing stacked PRs in GitHub](https://image.devclass.com/5217923.webp?imageId=5217923&x=0.00&y=0.00&cropw=100.00&croph=100.00&width=960&height=452&format=jpg)

## Source and further reading

- Stacked PRs site: https://github.github.com/gh-stack/
- Docs overview: https://github.github.com/gh-stack/introduction/overview/
- HN discussion (as referenced): https://news.ycombinator.com/item?id=47757495
- Phabricator/stacked diffs explainer: https://jg.gg/2018/09/29/stacked-diffs-versus-pull-requests/
- Phorge fork: https://we.phorge.it/


[Read the entire article](https://www.devclass.com/development/2026/04/16/github-invokes-spirit-of-phabricator-with-preview-of-stacked-prs/5217921)

