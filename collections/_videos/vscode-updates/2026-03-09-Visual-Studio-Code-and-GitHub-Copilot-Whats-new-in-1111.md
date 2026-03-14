---
external_url: https://www.youtube.com/watch?v=Z-psHv_W5Yc
title: Visual Studio Code and GitHub Copilot - What's new in 1.111
author: Fokko Veegens
feed_name: Fokko at Work
date: 2026-03-09 18:00:00 +00:00
tags:
- Agent Hooks
- Autopilot
- Debug Panel
- VS Code
- VS Code 1.111
- AI
- GitHub Copilot
- Videos
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Fokko Veegens covers the highlights of VS Code 1.111, the first weekly release following the team's shift from monthly to weekly cadence. Features include autopilot mode for bypassing all approvals, agent-scoped hooks for custom agents, and debug event snapshots for diagnosing failed sessions.<!--excerpt_end-->

{% youtube Z-psHv_W5Yc %}

## Full summary based on transcript

Visual Studio Code 1.111 marks the beginning of a new weekly release cadence, replacing the previous monthly schedule. This first weekly release brings significant changes to Copilot's autonomy, hook system, and debugging capabilities.

### Autopilot

Agent mode already operated quite autonomously, but VS Code 1.111 takes it a step further with full approval bypassing and an "autopilot" mode.

**Approval levels:**

- **Default approvals** - honors the per-tool approval settings configured in your settings
- **Bypass approvals** - skips all approval prompts (one-time confirmation warning is shown)

**Enabling autopilot:**

1. Go to settings and search for "chat approval"
2. Set the approval mode to "bypass approvals" (triggers a one-time risk acknowledgment)
3. Optionally enable the autopilot checkbox for full autonomous operation (the mode indicator turns yellow as a reminder)

**Slash commands:**

- `/autoapprove` - enable auto-approve mode
- `/yolo` - alias for auto-approve, familiar from other AI assistants

**Caution:** With autopilot enabled, Copilot runs tools without asking for consent. Read the risk warnings carefully before enabling this feature.

### Agent-Scoped Hooks

Building on the hooks feature introduced in VS Code 1.110, hooks can now be scoped to specific custom agents rather than applying globally.

**Prerequisites:**

- Setting: `chat.useCustomAgentHooks` must be enabled (does not work without it)

**How it works:**

- Define hooks directly inside a custom agent definition file
- Hooks configured in an agent only trigger when that specific agent is active
- Global hooks (configured via `/hooks` or in the `.github/hooks/` folder) continue to apply to all agents

**Example - audit logging:**

- A custom "nitpicker" agent configured for code language review
- A `pre-tool-use` hook runs a PowerShell script before each tool call
- The script captures data from standard input (JSON with timestamp, working directory, session info, hook event name, and transcript path) and writes it to an audit log

**Hook input data (via standard input):**

- Timestamp
- Current working directory
- Session information
- Hook event name
- Transcript path
- Note: some fields may be empty depending on the context

**Use cases for agent-scoped hooks:**

- Audit logging for specific agents
- Running linters after code review agents
- Verification steps tied to particular workflows

### Debug Events Snapshot

The agent debug panel introduced in VS Code 1.110 now supports attaching debug event snapshots as context in new chat sessions, making it easier to diagnose failures.

**How it works:**

1. When an agent session fails or takes an unexpected direction, open the agent debug panel
2. Navigate to the "View logs" view
3. Click the sparkle button to start a new chat session with the debug events snapshot attached
4. Alternatively, use `#debugEventsSnapshot` in any chat to attach the most recent session's debug data

**Example:**

- A session using GPT-5 mini failed because the reasoning effort was set to "x-high", which that model does not support (valid values: minimal, low, medium, high)
- Attaching the debug events snapshot to a new session with a different model allowed Copilot to analyze the failure and explain exactly what went wrong
