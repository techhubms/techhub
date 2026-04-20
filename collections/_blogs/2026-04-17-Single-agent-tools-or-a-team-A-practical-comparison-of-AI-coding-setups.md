---
title: Single-agent, tools, or a team? A practical comparison of AI coding setups
date: 2026-04-17 00:00:00 +00:00
external_url: https://hiddedesmet.com/single-agent-tools-or-a-team
tags:
- .NET
- .NET Aspire
- 429 Too Many Requests
- Agent With Tools
- Agents
- AI
- AI Assisted Development
- Appsettings.json
- ASP.NET Core
- Aspire.Hosting.Testing
- Blogs
- Claude
- Copilot Agent Mode
- Cost And Latency
- Development
- DistributedApplicationTestingBuilder
- GitHub Copilot
- Health Checks
- Integration Tests
- MCP
- Microsoft.AspNetCore.RateLimiting
- Minimal APIs
- Multi Agent
- OpenTelemetry
- Orchestration
- Prompt Engineering
- Rate Limiting
- Redis
- Retry After Header
- Service Discovery
- ServiceDefaults
- Single Agent
- Tool Calling
- xUnit
feed_name: Hidde de Smet's Blog
primary_section: github-copilot
section_names:
- ai
- dotnet
- github-copilot
author: Hidde de Smet
---

Hidde de Smet compares three AI coding setups—single-agent, agent-with-tools, and multi-agent—using a realistic .NET Aspire + ASP.NET Core rate-limiting task to show trade-offs in fit, cost, latency, and common failure modes.<!--excerpt_end-->

# Single-agent, tools, or a team? A practical comparison of AI coding setups

Every week someone asks which agent setup they should use:

- **Single-agent** (just chat with one model)
- **Agent-with-tools** (one model, but with tool access via MCP / function calling)
- **Multi-agent** (a coordinated “team” of specialized agents)

The answer is usually “it depends”, so this post compares the same realistic feature across all three setups, highlights failure modes, and ends with a decision framework.

## The three archetypes

| Setup | What it is | Typical example |
| --- | --- | --- |
| **Single-agent** | One model, one prompt, one context window. No external tools beyond what the IDE provides. | A chat session in Copilot or Claude where you paste code and iterate. |
| **Agent-with-tools** | One model that can call external tools through MCP or function calling. Still one context window, but it can read files, run commands, search docs, hit APIs. | Claude Code with a few MCP servers attached, or Copilot’s agent mode with tool access. |
| **Multi-agent** | Multiple specialized agents coordinated by an orchestrator. Each agent has its own role, prompt, and often its own context window. | A planner agent writes a spec, an implementer agent writes code, a reviewer agent checks it. |

The boundary between **agent-with-tools** and **multi-agent** can blur (for example when a single agent spawns subagents), but here “multi-agent” means at least two agents with distinct roles that hand work to each other.

## The benchmark task

Same task, same model family, same spec—different orchestration.

**Task:** add rate limiting to an **ASP.NET Core Minimal API** service inside a **.NET Aspire** solution.

The solution already has:

- Redis integration wired up in the **AppHost**
- service defaults for **OpenTelemetry** and **health checks** in place
- an existing **xUnit** test project using `Aspire.Hosting.Testing`
- no current rate-limit implementation

**Spec requirements:**

- per-IP and per-API-key limits using built-in `Microsoft.AspNetCore.RateLimiting`
- configurable windows via `appsettings.json`
- proper `429` responses with `Retry-After` headers
- integration tests that boot the AppHost using `DistributedApplicationTestingBuilder` and hit both limiters

**Important scope note:** sharing counters across instances via Redis was explicitly a follow-up and not part of this first pass, because ASP.NET Core’s built-in rate limiter is **in-process**.

**Model setup:** Claude 4.x family, with Haiku used for worker roles in the multi-agent run.

## What each setup gets right (and wrong)

### Single-agent

**Strengths:** fast and cheap.

**Typical failure:** it forgets Aspire conventions. A common symptom is adding rate limiting directly in the API project’s `Program.cs`, rather than extending the shared `ServiceDefaults` extension methods (where health checks and telemetry already live).

Result: the code works, but it doesn’t fit the solution’s structure—especially obvious once you add a second API in the Aspire setup and realize the limiter wasn’t applied.

### Agent-with-tools

**Strengths:** spends time reading the AppHost wiring and the `ServiceDefaults` project before coding, which improves solution fit.

What it tends to do well:

- finds Redis already registered in the AppHost
- wires the connection through **service discovery** instead of hardcoding connection strings
- writes tests that actually boot the distributed app with `DistributedApplicationTestingBuilder`

**Typical failure:** tool-call loops on edge cases (example: API-key header casing), repeatedly trying near-identical fixes rather than stepping back.

### Multi-agent

**Strengths:** cleaner first pass, especially when roles are well-defined.

A common successful pattern:

- a planner agent produces a spec and places rate-limit registration in `ServiceDefaults`
- an implementer codes against that spec
- a reviewer catches missing tests (often around `Retry-After` or the 429 response shape)

**Trade-off:** more hidden cost (orchestration tokens, handoff artifacts) and longer wall-clock time.

## The decision matrix

| Factor | Single-agent | Agent-with-tools | Multi-agent |
| --- | --- | --- | --- |
| Task complexity | Low | Medium | High |
| Codebase size | Small | Medium to large | Medium to large |
| Context needed | Fits in one prompt | Needs file reads, search | Needs isolated reasoning per step |
| Determinism required | Low | Medium | High |
| Cost sensitivity | High | Medium | Low |
| Debuggability | Easy | Medium | Hard |
| Setup effort | None | Low | Medium to high |

Two rules of thumb:

1. If the task fits in a single prompt with full context, don’t reach for multi-agent—you’ll often pay more for less.
2. If the task naturally splits into roles (design, implement, review), multi-agent can win, **but only if the handoff artifacts are good**.

Multi-agent systems live or die on what gets passed between agents: vague specs are worse than a single agent with a clear prompt. Good specs, clear task lists, and structured progress logs make multi-agent viable.

## Cost and latency, without the hype

Multi-agent often looks expensive, but routing helps.

A sensible split:

- orchestrator + planner on **Sonnet**
- implementer on **Sonnet** (coding quality matters)
- reviewer/test-runner/doc-updater on **Haiku**

At current [Anthropic pricing](https://www.anthropic.com/pricing), Haiku 4.5 is roughly a third of Sonnet 4.5’s per-token cost, so offloading bounded roles can cut cost without much quality hit for those roles.

Latency is different: multi-agent is often sequential (planner → implementer → reviewer). Some roles can be parallel (tests and lint; docs after implementation), but the main chain is still serial.

Rule of thumb:

- Running hundreds per day in CI: **cost** matters more than latency.
- Running a handful during an afternoon: **latency** matters more than cost.

## Failure modes

### Single-agent failures

- **Context collapse:** runs out of room, forgets earlier parts, reintroduces fixed bugs.
- **Pattern invention:** without reading the codebase, it guesses conventions and often guesses wrong.
- **Silent scope creep:** expands the task (rate limiting turns into “also a new config system”).

### Agent-with-tools failures

- **Tool-call loops:** retries the same failing call with minor variations; burns tokens.
- **Context bloat:** tool output fills the context window before any code is written.
- **Over-reading:** reads far more files than needed because the prompt rewards thoroughness.

### Multi-agent failures

- **Lying handoffs:** one agent claims “done” when it isn’t; downstream agents build on it.
- **Coordination overhead:** orchestrator spends more tokens on delegation than work.
- **Runaway cost:** without budgets/iteration limits, loops can spiral.
- **Debugging hell:** hard to attribute failures without per-agent logs and artifacts.

## When to pick what

### Pick single-agent when

- the task fits in one prompt with full context
- you want fast feedback and low cost
- you’re exploring/prototyping/small edits
- you don’t need production-ready output without review

### Pick agent-with-tools when

- the task needs codebase awareness (read files, run tests, search docs)
- you want one coherent reasoning thread with external information
- conventions matter in a medium-sized codebase
- you will review the final result

### Pick multi-agent when

- the task has separable roles (plan, implement, review, test)
- you need determinism and repeatability (CI / scheduled runs)
- the context exceeds one window in a single-agent setup
- you have good handoff artifacts (specs, task lists, progress logs)
- you can invest in logging and guardrails up front

Anti-pattern: choosing multi-agent because it sounds sophisticated for tasks a well-prompted single agent would finish in minutes.

## Where this is going

These categories are blending:

- agents spawn subagents mid-task
- MCP servers can wrap what used to be multi-agent systems behind one tool call

The question shifts from “which setup do I pick” to “which shape does this task want right now”. The recommended path:

1. Start in a single-agent chat to feel the problem.
2. Move to agent-with-tools once you need to read files or run commands.
3. Use multi-agent only when roles clearly split, or when the workflow must run repeatedly in CI.

The best AI coding setup is the smallest one that gets the job done—everything beyond that is complexity you’ll debug later.


[Read the entire article](https://hiddedesmet.com/single-agent-tools-or-a-team)

