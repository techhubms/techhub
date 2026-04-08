---
primary_section: github-copilot
title: 'Spec-Kit Extensions: Making spec-driven development your own'
tags:
- Agentic Coding
- AI
- AI Assisted Development
- Autonomous Implementation
- Azure DevOps Integration
- Blogs
- CI/CD
- CLI
- Code Review Automation
- Confluence Integration
- Copilot
- Copilot Agent
- Development
- DevOps
- Extensions
- GitHub
- GitHub Copilot
- Iteration Loop
- Jira Integration
- Progress.md
- Ralph Loop
- Slash Commands
- Spec Driven Development
- Spec Kit
- Specification Driven Development
- Tasks.md
- Workflow Orchestration
external_url: https://hiddedesmet.com/speckit-extensions
feed_name: Hidde de Smet's Blog
author: Hidde de Smet
date: 2026-04-08 00:00:00 +00:00
section_names:
- ai
- devops
- github-copilot
---

Hidde de Smet explains how Spec-Kit’s extension system works, highlights useful community extensions, and walks through the Ralph Loop extension, which runs a GitHub Copilot agent in iterations to implement tasks from `tasks.md`, commit changes, and track context in `progress.md`.<!--excerpt_end-->

## Overview

Spec-Kit’s core workflow (constitution → specify → plan → tasks → implement) is meant to be extended. This post explains how Spec-Kit extensions and presets differ, where extensions live, how to install them, and which community extensions are worth knowing. It then dives into **Ralph Loop**, an extension that can autonomously implement tasks in a loop using a Copilot-based agent process.

## What are Spec-Kit extensions?

Extensions add new capabilities to Spec-Kit. Presets customize existing behavior.

| Mechanism | Purpose | Example |
| --- | --- | --- |
| **Extension** | Add new commands or workflows | Jira sync, code review, autonomous loops |
| **Preset** | Customize specs, plans, and task formats | Pirate speak, compliance templates, TOC navigation |

### Extension structure

Extensions live under `extensions/` and typically include:

- `extension.yml` (manifest)
- command templates
- optional scripts
- configuration

### Installing and managing extensions

```bash
# Search what's available
specify extension search

# Install an extension
specify extension add <extension-name>

# List installed extensions
specify extension list
```

After installation, extension commands show up as **slash commands** in your AI agent session, similar to built-in Spec-Kit commands.

## Extensions worth knowing about

The community catalog is at: https://speckit-community.github.io/extensions/

It groups extensions into **docs**, **code**, **process**, **integration**, and **visibility**.

### Docs extensions

| Extension | What it does |
| --- | --- |
| **Iterate** | Refine specs mid-implementation and return directly to building |
| **Reconcile** | Update feature artifacts when implementation drifts from the spec |
| **Spec Critique** | Critical review from product strategy and engineering-risk angles |

### Code extensions

| Extension | What it does |
| --- | --- |
| **Review** | Post-implementation code review focusing on quality, tests, error handling, simplification |
| **Staff Review** | Staff-level review: validates against spec, checks security and performance |
| **Fix Findings** | Analyze → fix → reanalyze loop until findings are resolved |
| **Cleanup** | Quality gate: fixes small issues, creates tasks for medium ones |
| **Verify** | Validates implemented code against specification artifacts |

### Process extensions

| Extension | What it does |
| --- | --- |
| **Fleet Orchestrator** | Full feature lifecycle orchestration with human-in-the-loop gates |
| **MAQA** | Coordinator → feature → QA workflow with parallel worktree-based implementation |
| **Conduct** | Delegates phases to sub-agents to reduce context pollution |
| **Product Forge** | End-to-end lifecycle: research → product spec → SpecKit → implement → verify → test |

### Integration extensions

| Extension | What it does |
| --- | --- |
| **Jira Integration** | Creates Jira Epics, Stories, and Issues from specs/tasks |
| **Azure DevOps** | Syncs user stories and tasks to Azure DevOps work items |
| **Confluence** | Creates a Confluence doc summarizing specs and planning files |

### Visibility extensions

| Extension | What it does |
| --- | --- |
| **Project Health Check** | Reports health issues across structure, agents, features, scripts, extensions, and git |
| **Project Status** | Shows current SDD progress: active feature, artifact status, task completion, extensions summary |

### Read-only vs Read+Write

Extensions declare whether they are:

- **Read-only**: reports without modifying files
- **Read+Write**: modifies files / creates artifacts

Check this label before installing an extension that can write to your repo.

## Ralph Loop: autonomous implementation in practice

Ralph Loop repository: https://github.com/Rubiss/spec-kit-ralph

Ralph Loop takes your `tasks.md` and implements tasks autonomously, one work unit per iteration.

### How it works

For each iteration:

1. The orchestrator starts a loop and validates prerequisites / loads config.
2. If tasks remain, it spawns a **fresh agent process**.
3. The agent reads `tasks.md` (and `progress.md`), selects the first incomplete work unit, implements it, checks off tasks, and commits.
4. The orchestrator checks termination conditions and loops.

The process is designed to be resumable: interrupting with Ctrl+C allows you to resume later because task completion is tracked via checkboxes in `tasks.md`.

The post describes the loop as:

```text
┌─────────────────────────────────────────┐
│ ralph-loop starts                       │
│ validate prerequisites, load config      │
└──────────────────┬──────────────────────┘
                   ▼
           ┌────────────────┐
           │ Any tasks left? │──No──▶ exit 0 (COMPLETED)
           └───────┬────────┘
                   │ Yes
                   ▼
┌────────────────────────────────────────────┐
│ Spawn fresh agent process                   │
│ copilot --agent speckit.ralph               │
└───────────────────┬────────────────────────┘
                    ▼
┌────────────────────────────────────────────┐
│ Agent reads tasks.md + progress.md,         │
│ implements ONE work unit, commits           │
└───────────────────┬────────────────────────┘
                    ▼
          ┌──────────────────────┐
          │ Check termination     │
          │ conditions            │
          └─────────┬────────────┘
                    ▼
            back to "Any tasks left?"
```

### What it looks like in the terminal

Ralph prints an iteration banner and streams agent output, including the tasks it’s completing and the commit message.

```text
============================================================
Ralph Loop - 001-my-feature Iteration 1 of 10
============================================================

[09:12:03] * Running - Starting iteration

--- Copilot Agent Output ---
Reading tasks.md... found 12 tasks across 3 user stories.
Working on US-001: Initialize project structure
[x] T001: Create directory layout
[x] T002: Add configuration files
[x] T003: Set up dependency management
Committing: feat(001-my-feature): US-001 Initialize project structure
--- End Agent Output ---

[09:14:27] * Success - Iteration completed
9 task(s) remaining
```

### What `progress.md` looks like

After each iteration, Ralph appends to `progress.md`:

- iteration timestamp
- user story worked on
- tasks completed
- remaining tasks in the story
- commit hash
- files changed
- a **Learnings** section

Example (abridged):

```markdown
---
## Iteration 1 - 2026-04-08 09:14:27
**User Story**: US-001 Initialize project structure

**Tasks Completed**:
- [x] T001: Create directory layout
- [x] T002: Add configuration files
- [x] T003: Set up dependency management

**Commit**: a1b2c3d

**Files Changed**:
- src/config/app.yml
- src/config/database.yml
- package.json

**Learnings**:
- Project uses ESM modules, not CommonJS
- Prettier config already exists in repo root
```

The “Learnings” section is intended to reduce repeated rediscovery when each iteration starts a new agent process.

## Installing and running Ralph

### Install

```bash
# Install from the community catalog
specify extension add ralph

# Or install directly from the repository
specify extension add ralph --from \
  https://github.com/Rubiss/spec-kit-ralph/archive/refs/tags/v1.0.0.zip

# Verify
specify extension list
```

### Run inside an agent session

```text
/speckit.ralph.run
```

With options:

```text
/speckit.ralph.run --max-iterations 5 --model gpt-5.1
```

### Run from the terminal (debugging / CI)

```bash
.specify/extensions/ralph/scripts/bash/ralph-loop.sh \
  --feature-name "001-my-feature" \
  --tasks-path "specs/001-my-feature/tasks.md" \
  --spec-dir "specs/001-my-feature" \
  --max-iterations 10 \
  --model "claude-sonnet-4.6"
```

## Configuration

Project defaults live at:

- `.specify/extensions/ralph/ralph-config.yml`

Example:

```yaml
# AI model for agent iterations
model: "claude-sonnet-4.6"

# Maximum loop iterations before stopping
max_iterations: 10

# Path or name of the agent CLI binary
agent_cli: "copilot"
```

### Config precedence

| Priority | Source |
| --- | --- |
| 1 (lowest) | Defaults in `extension.yml` |
| 2 | Project config `.specify/extensions/ralph/ralph-config.yml` |
| 3 | Local overrides `.specify/extensions/ralph/ralph-config.local.yml` (gitignored) |
| 4 | Environment variables (e.g., `SPECKIT_RALPH_MODEL`) |
| 5 (highest) | CLI parameters (e.g., `--model`, `--max-iterations`) |

This lets individual developers override models locally without committing changes.

## Termination conditions

Ralph stops for these reasons:

| Condition | Exit code | Meaning |
| --- | --- | --- |
| All tasks marked `[x]` | 0 | Done |
| Agent outputs `COMPLETE` | 0 | Agent confirmed completion |
| Max iterations reached | 1 | Safety limit |
| 3 consecutive failures | 1 | Circuit breaker (agent is stuck) |
| Ctrl+C | 130 | User interrupted |

## When Ralph makes sense

Ralph is positioned as most effective when:

- `tasks.md` is well-structured with clear, independent work units
- tasks are granular enough for a single iteration each
- spec and plan are already validated
- the team is comfortable reviewing commits after the fact

## Building your own extension

Basic extension layout:

```text
my-extension/
├── extension.yml          # Manifest: name, version, commands, hooks
├── commands/
│   └── my-command.md      # Command template (becomes /speckit.ext.my-command)
├── scripts/
│   └── bash/
│       └── my-script.sh   # Optional automation scripts
├── README.md
└── LICENSE
```

Publishing reference:

- Extension Publishing Guide: https://github.com/github/spec-kit/blob/main/extensions/EXTENSION-PUBLISHING-GUIDE.md

## Where to start

- Browse the community catalog: https://speckit-community.github.io/extensions/
- Install 1–2 extensions that cover gaps in your current workflow
- If nothing fits, build your own extension (the format is intended to be lightweight)


[Read the entire article](https://hiddedesmet.com/speckit-extensions)

