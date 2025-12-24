---
layout: "post"
title: "Do Epic Shit Chat Mode: Beast Mode for GitHub Copilot"
description: "This post by Harald Binkle delves into 'Do Epic Shit' chat mode, an enhanced operational configuration for GitHub Copilot Chat that pushes the tool into 'Beast Mode.' The article explains the motivation, conceptual framework, implementation details, and practical differences compared to standard Copilot modes, focusing on rigorous automation, strict TODO enforcement, autonomy, and research depth for developers in VS Code."
author: "Harald Binkle"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://harrybin.de/posts/do-epic-shit-chat-mode/"
viewing_mode: "external"
feed_name: "Harald Binkle's blog"
feed_url: "https://harrybin.de/rss.xml"
date: 2025-08-15 10:30:00 +00:00
permalink: "/posts/2025-08-15-Do-Epic-Shit-Chat-Mode-Beast-Mode-for-GitHub-Copilot.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Development Tools", "Automation", "Autonomous Agents", "Beast Mode", "Chat Configuration", "Coding", "Copilot Chat Mode", "Developer Productivity", "Engineering Agents", "GitHub Copilot", "GPT 4.1", "Markdown Checklist", "Posts", "Refactoring", "Software Testing", "Todo Loop", "VS Code"]
tags_normalized: ["ai", "ai development tools", "automation", "autonomous agents", "beast mode", "chat configuration", "coding", "copilot chat mode", "developer productivity", "engineering agents", "github copilot", "gpt 4dot1", "markdown checklist", "posts", "refactoring", "software testing", "todo loop", "vs code"]
---

Harald Binkle introduces the 'Do Epic Shit' chat mode for GitHub Copilot, outlining its strict autonomy and research loops to maximize developer productivity in VS Code.<!--excerpt_end-->

# Do Epic Shit Chat Mode (Beast Mode for GitHub Copilot)

**Author:** Harald Binkle

## Overview

This post explores the 'Do Epic Shit' chat mode—a rigorously structured operational pattern for GitHub Copilot Chat, designed to maximize automation, autonomy, and quality assurance in real development workflows.

## What is “Beast Mode”?

'Beast Mode' is a conceptual setup for GitHub Copilot Chat that:  

- Grants expansive access to tools (codebase, search, terminals, etc.)  
- Enforces strict, iterative TODO loop completion  
- Demands fresh research rather than relying on model memory  
- Supports recursive validation and post-action verification (e.g., running tests, linting)

It suits non-trivial engineering tasks such as major refactors, exploratory debugging, and multi-file feature development.

## Motivation for Rigorous Chat Modes

Community feedback highlighted issues like premature agent exit, shallow research, partial edits, and ambiguous step planning. Beast Mode—and later the 'Do Epic Shit' variant—arose to enforce repeatability, discipline, and traceability:

- Prevents incomplete tasks with strict checklist contracts
- Enforces revision and live research instead of trusting static responses
- Requires verification steps with test and lint tooling
- Creates auditable logs through markdown checklists and versioning

## The Do Epic Shit Mode: Implementation Details

Inspired by the broader Beast Mode pattern, 'Do Epic Shit Mode' is a hardened realization:

- **Frontmatter description and versioning**—for traceability
- **Explicit, wide tool declaration**—including diagnostics and automation tasks
- **Model pinning**—e.g., GPT-4.1 for expected output quality
- **Strict, markdown-only checklist format**—for reliable automation and parsing
- **Autonomy contract**—no early turn yield until all steps are verified and complete
- **Recursive research discipline**—refresh and validate information on every loop

**Code Example for Template:**

```markdown
---
description: "Beast Mode v0.1"
tools: ["codebase", "search", "runTests", "runTasks", "terminal", "editFiles"]
model: GPT-4.1
---
You are an autonomous engineering agent. Follow the checklist until all items are completed & verified.

Checklist format:
- [ ] Step 1: ...
- [ ] Step 2: ...
```

## Comparison: Beast Mode vs Do Epic Shit Mode

| Capability         | Beast Mode (Pattern)    | Do Epic Shit Mode         | Advantage                        |
|-------------------|-------------------------|---------------------------|----------------------------------|
| Tool Scope        | Broad, conceptual       | Exhaustively enumerated   | Concrete reproducibility         |
| Iteration Loop    | Encouraged              | Hard requirement          | Guarantees completion discipline |
| Research Mandate  | Often implied           | Explicit, recursive       | Reduces stale assumptions        |
| Versioning        | Optional                | Embedded/versioned        | Traceability/evolution           |
| Verification      | Recommended             | Mandatory                 | High-confidence output           |
| Tool Governance   | Loose                   | Curated/strict            | Predictable environment          |

## New VS Code Chat Settings

Recent VS Code updates added settings to boost Copilot Chat functionality:

- `chat.todoListTool.enabled`: Activates native TODO extraction and management, dovetailing with strict checklist enforcement.
- `github.copilot.chat.alternateGptPrompt.enabled`: Uses an alternative, more reasoning-focused prompt, stacking with custom modes for better planning.

**Difference:**
Custom mode requirements override tooling defaults—deviations in checklist format can invalidate the mode contract.

## Designing Your Own Mode

- Keep the checklist contract strict and machine-parsable
- Enumerate all tools up front for intentional operation
- Include version notes for traceability
- Require explicit reflection after each checklist step
- Only allow agent exit once the list is empty, tests pass, and lint checks are clean

## Conclusion & Takeaways

- Codify autonomy—don’t rely on default agent behavior
- Rely on explicit, deterministic checklist grammar
- Treat research as an ongoing loop
- Use versioning for audits and improvements
- Verification gates ensure actual completeness

For more on prompt tuning or building your own discipline-driven modes, see Harald’s recommended reading on further improving Copilot results.

This post appeared first on "Harald Binkle's blog". [Read the entire article here](https://harrybin.de/posts/do-epic-shit-chat-mode/)
