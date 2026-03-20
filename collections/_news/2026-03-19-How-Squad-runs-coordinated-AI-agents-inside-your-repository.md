---
date: 2026-03-19 16:09:50 +00:00
tags:
- Agentic Development
- AI
- AI & ML
- AI Agents
- CLI Tooling
- Code Review
- Context Replication
- Context Windows
- DevOps
- GitHub Copilot
- Independent Review Protocol
- Markdown Based Memory
- Multi Agent Workflows
- News
- npm
- Pull Requests
- Repository Native Orchestration
- Shared Memory
- Squad
- Squad CLI
- Versioned Decisions
- Workflow Automation
feed_name: The GitHub Blog
external_url: https://github.blog/ai-and-ml/github-copilot/how-squad-runs-coordinated-ai-agents-inside-your-repository/
title: How Squad runs coordinated AI agents inside your repository
author: Brady Gaster
primary_section: github-copilot
section_names:
- ai
- devops
- github-copilot
---

Brady Gaster explains how Squad, a GitHub Copilot-based open source project, coordinates multiple AI agents directly inside a repository using inspectable patterns like versioned decision logs, specialist routing, and PR-based collaboration rather than opaque orchestration layers.<!--excerpt_end-->

# How Squad runs coordinated AI agents inside your repository

If you’ve used AI coding tools before, you’ve probably seen the loop: write a prompt, the model misunderstands, refine the prompt, repeat. As projects grow, the bigger problem becomes coordinating design, implementation, testing, and review without losing context.

Multi-agent systems can help, but they often require significant setup (custom orchestration layers, frameworks, and sometimes vector databases) before they’re usable.

## What Squad is

[Squad](https://github.com/bradygaster/squad) is an open source project built on GitHub Copilot that initializes a preconfigured AI team directly inside your repository. The goal is to make multi-agent development more accessible and inspectable without heavy centralized orchestration infrastructure.

To install and initialize:

- Install once globally:

```bash
npm install -g @bradygaster/squad-cli
```

- Initialize once per repository:

```bash
squad init
```

After initialization, Squad adds a specialized team (for example: lead, frontend developer, backend developer, tester) that works directly against your repo.

## How Squad coordinates work across agents

You describe the work in natural language, and a coordinator agent routes tasks, loads repository context, and spawns specialist agents with task-specific instructions.

Example prompt:

> Team, I need JWT auth—refresh tokens, bcrypt, the works.

What happens next (as described):

- Specialists can spin up in parallel.
  - Backend specialist works on implementation.
  - Tester writes and runs an accompanying test suite.
  - A documentation specialist can open a pull request.
- Files can be written and branches created.
- Specialists can pick up repository conventions and prior decisions by loading shared, committed project history/decision files—rather than relying only on the current prompt.

### Built-in iteration and independent review

Instead of pushing you through repeated manual prompt/test cycles, Squad runs iteration internally:

- A specialist drafts an implementation.
- A tester runs the test suite against it.
- If tests fail, the tester rejects the code.

A key safeguard described in the post is that the original agent can be prevented from revising its own rejected work. A different agent must step in to fix it. The intent is to enforce independent review (a separate context window and fresh perspective) rather than having a single AI “review” its own output.

Squad is not positioned as autopilot:

- Agents may ask clarifying questions.
- Agents may make reasonable but incorrect assumptions.
- You still review and merge every pull request.

## Architectural patterns behind repository-native orchestration

The post lays out patterns learned while building “repository-native orchestration” to move away from black-box behavior and toward workflows that are inspectable and predictable at the repository level.

### 1. The “Drop-box” pattern for shared memory

Rather than relying on real-time chat synchronization or vector database lookups, Squad uses a repository file as shared memory.

- Architectural choices (library decisions, naming conventions, etc.) are appended as structured blocks to a repository file:
  - `versioned decisions.md`
- Benefits called out:
  - persistence
  - legibility
  - audit trail
  - easier recovery after disconnects/restarts

### 2. Context replication over context splitting

To deal with context window limits:

- The coordinator stays a thin router.
- Specialists do the work as separate inference calls.
- Instead of splitting a single context across agents, repository context is replicated across specialist contexts.

The post mentions large context windows on supported models (for example, “up to 200K tokens”).

### 3. Explicit memory in the prompt vs. implicit memory in the weights

Squad emphasizes memory that is versioned and legible in the repository:

- An agent’s identity is built primarily on repository files:
  - a charter (who they are)
  - a history (what they’ve done)
  - shared team decisions
- These live as plain text in a `. squad/` folder (as written in the post).

Result: cloning the repo gives you not just code, but an “onboarded” AI team whose memory is committed alongside the project.

## Lowering the barrier to multi-agent workflows

The post’s overall pitch is reducing ceremony and infrastructure overhead so people can try agentic development without spending hours setting up orchestration layers or learning complex prompt engineering.

To try it:

- Read the original post: https://github.blog/ai-and-ml/github-copilot/how-squad-runs-coordinated-ai-agents-inside-your-repository/
- Explore the project: https://github.com/bradygaster/squad


[Read the entire article](https://github.blog/ai-and-ml/github-copilot/how-squad-runs-coordinated-ai-agents-inside-your-repository/)

