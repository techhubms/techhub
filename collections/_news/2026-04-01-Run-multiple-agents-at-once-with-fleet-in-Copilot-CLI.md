---
author: Matt Nigh
date: 2026-04-01 15:00:00 +00:00
primary_section: github-copilot
feed_name: The GitHub Blog
title: Run multiple agents at once with /fleet in Copilot CLI
external_url: https://github.blog/ai-and-ml/github-copilot/run-multiple-agents-at-once-with-fleet-in-copilot-cli/
section_names:
- ai
- devops
- github-copilot
tags:
- '  No Ask User'
- .github/agents
- /fleet
- /tasks
- Agent Orchestration
- AI
- AI & ML
- Context Window
- Custom Agents
- Dependencies
- DevOps
- GitHub Copilot
- GitHub Copilot CLI
- Linting
- Multi Agent
- News
- Non Interactive Mode
- Parallel Execution
- Prompt Engineering
- Slash Commands
- Sub Agents
- Task Decomposition
- Tests
- Type Checking
- Validation Criteria
---

Matt Nigh explains how the `/fleet` command in GitHub Copilot CLI orchestrates multiple parallel sub-agents across your codebase, with practical prompt patterns for splitting deliverables, declaring dependencies, setting boundaries, and avoiding file-collision pitfalls.<!--excerpt_end-->

## Overview

`/fleet` is a slash command in **GitHub Copilot CLI** that lets Copilot work on multiple parts of your codebase at the same time by dispatching multiple background **sub-agents** in parallel. Instead of completing tasks one-by-one, an orchestrator breaks your objective into independent work items, runs what it can concurrently, and then synthesizes the final output.

## How `/fleet` works

When you run `/fleet` with a prompt, Copilot’s orchestrator:

1. Decomposes the objective into discrete work items (with dependencies).
2. Determines what can run in parallel vs. what must wait.
3. Dispatches independent work items as background sub-agents.
4. Polls for completion and dispatches additional waves as needed.
5. Verifies outputs and synthesizes final artifacts.

Key behavior details:

- Each sub-agent has its own context window.
- Sub-agents share the same filesystem.
- Sub-agents can’t talk to each other directly—only the orchestrator coordinates.

## Getting started

### Interactive usage

Start fleet mode with:

```text
/fleet <YOUR OBJECTIVE PROMPT>
```

Example:

```text
/fleet Refactor the auth module, update tests, and fix the related docs in the folder docs/auth/
```

### Non-interactive usage

You can run `/fleet` non-interactively:

```bash
copilot -p "/fleet <YOUR TASK>" --no-ask-user
```

- `--no-ask-user` is required in non-interactive mode because there’s no way to respond to prompts.

## Write prompts that parallelize well

The more clearly you define structure and deliverables, the easier it is for the orchestrator to split work into parallel tracks.

### Prefer concrete deliverables

Vague prompts tend to serialize work because Copilot can’t identify independent pieces.

Instead of:

```text
/fleet Build the documentation
```

Use artifact-driven deliverables:

```text
/fleet Create docs for the API module:

- docs/authentication.md covering token flow and examples
- docs/endpoints.md with request/response schemas for all REST endpoints
- docs/errors.md with error codes and troubleshooting steps
- docs/index.md linking to all three pages (depends on the others finishing first)
```

This creates four distinct deliverables; three can run in parallel, and one depends on the others.

## Set explicit boundaries

Sub-agents work best when scope is explicit. Include:

- File/module boundaries (directories/files each track owns)
- Constraints (what not to touch)
- Validation criteria (lint/typecheck/tests that must pass)

Example:

```text
/fleet Implement feature flags in three tracks:

1. API layer: add flag evaluation to src/api/middleware/ and include unit tests that look for successful flag evaluation and tests API endpoints

2. UI: wire toggle components in src/components/flags/ and introduce no new dependencies

3. Config: add flag definitions to config/features.yaml and validate against schema

Run independent tracks in parallel. No changes outside assigned directories.
```

## Declare dependencies when they exist

If one task depends on another, say so. The orchestrator can serialize the dependency chain and still parallelize the rest.

Example:

```text
/fleet Migrate the database layer:

1. Write new schema in migrations/005_users.sql

2. Update the ORM models in src/models/user.ts (depends on 1)

3. Update API handlers in src/api/users.ts (depends on 2)

4. Write integration tests in tests/users.test.ts (depends on 2)

Items 3 and 4 can run in parallel after item 2 completes.
```

## Use custom agents for different jobs

You can define specialized agents under `.github/agents/` and reference them in your `/fleet` prompt. Agents can specify their own model, tools, and instructions.

Example agent definition:

```markdown
# .github/agents/technical-writer.md

---

name: technical-writer

description: Documentation specialist

model: claude-sonnet-4

tools: ["bash", "create", "edit", "view"]

---

You write clear, concise technical documentation. Follow the project style guide in /docs/styleguide.md.
```

Then reference it:

```text
/fleet Use @technical-writer.md as the agent for all docs tasks and the default agent for code changes.
```

## How to verify sub-agents are running

Checklist:

- **Decomposition appears**: review Copilot’s plan to confirm multiple tracks vs. a single linear plan.
- **Background task UI confirms activity**: run `/tasks` to inspect running background tasks.
- **Parallel progress appears**: updates should reference separate tracks moving at the same time.

If it doesn’t parallelize, stop and request explicit decomposition:

```text
Decompose this into independent tracks first, then execute tracks in parallel. Report each track separately with status and blockers.
```

## Avoiding common pitfalls

### Partition your files

Because sub-agents share a filesystem **without file locking**, two agents writing to the same file can cause silent overwrites (last write wins).

Mitigations:

- Assign each agent distinct files.
- If multiple agents must contribute to one file:
  - have each write to a temporary path and let the orchestrator merge, or
  - set an explicit order.

### Keep prompts self-contained

Sub-agents don’t have access to the orchestrator’s conversation history. Ensure your `/fleet` prompt includes required context or references files the agents can read.

### Steering a fleet in progress

You can guide the orchestrator after dispatching, for example:

- `Prioritize failing tests first, then complete remaining tasks.`
- `List active sub-agents and what each is currently doing.`
- `Mark done only when lint, type check, and all tests pass.`

## When to use `/fleet` (and when not to)

Best fit:

- Refactoring across multiple files.
- Generating documentation for several components.
- Features spanning API, UI, and tests.
- Independent code modifications that don’t share state.

Less suitable:

- Strictly linear, single-file work (regular Copilot CLI prompts are simpler; fleet adds coordination overhead).

## Source

- The GitHub Blog: Run multiple agents at once with /fleet in Copilot CLI (https://github.blog/ai-and-ml/github-copilot/run-multiple-agents-at-once-with-fleet-in-copilot-cli/)

[Read the entire article](https://github.blog/ai-and-ml/github-copilot/run-multiple-agents-at-once-with-fleet-in-copilot-cli/)

