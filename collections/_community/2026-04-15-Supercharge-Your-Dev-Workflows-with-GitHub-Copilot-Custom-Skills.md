---
feed_name: Microsoft Tech Community
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/supercharge-your-dev-workflows-with-github-copilot-custom-skills/ba-p/4510012
primary_section: github-copilot
date: 2026-04-15 05:00:00 +00:00
title: Supercharge Your Dev Workflows with GitHub Copilot Custom Skills
section_names:
- ai
- devops
- github-copilot
author: sachoudhury
tags:
- .NET CLI
- Agent Mode
- AI
- Automation Scripts
- Azure CLI
- CLI Automation
- Community
- Copilot Chat
- Copilot Custom Skills
- DevOps
- GitHub Copilot
- GitHub Copilot CLI
- GitHub Copilot Coding Agent
- HTML Reports
- Prompt Engineering
- Python Scripts
- Reusable Templates
- Runbooks
- SKILL.md
- Slash Commands
- VS Code
- Workflow Orchestration
- YAML Frontmatter
---

sachoudhury explains GitHub Copilot Custom Skills: repo- or user-scoped SKILL.md “runbooks” that Copilot can discover and execute in agent mode to automate multi-step developer workflows (commands, scripts, and report generation).<!--excerpt_end-->

# Supercharge Your Dev Workflows with GitHub Copilot Custom Skills

Turn repetitive multi-step tasks into one-command AI workflows — no extensions, no plugins, just markdown and scripts.

## The Problem
Teams often repeat the same multi-step workflows:

- Run a sequence of CLI commands, parse output, and generate a report
- Query multiple APIs, correlate data, and summarize findings
- Execute test suites, analyze failures, and produce actionable insights

Even if these workflows are documented in a wiki or runbook, they often still require manual copy/paste, parameter tweaks, and stitching results together.

The goal: trigger the whole workflow with a single natural-language request via **GitHub Copilot Custom Skills**.

## What Are Custom Skills?
A **skill** is a folder containing:

- A required **`SKILL.md`** file (instructions for the AI)
- Optional scripts, templates, and reference docs

When you ask Copilot something that matches a skill description, Copilot loads the instructions and can execute the workflow autonomously.

Conceptually: a runbook Copilot can *execute*, not just read.

| Without Skills | With Skills |
| --- | --- |
| Read the wiki for the procedure | Copilot loads the procedure automatically |
| Copy-paste 5 CLI commands | Copilot runs the full pipeline |
| Manually parse JSON output | Script generates a formatted HTML report |
| 15–30 minutes of manual work | One natural language request, ~2 minutes |

## How It Works
The key idea: **the skill file is the contract between you and the AI**.

- You describe **what** to do and **how** to do it
- Copilot handles the orchestration

## Prerequisites

| Requirement | Details |
| --- | --- |
| VS Code | Latest stable release |
| GitHub Copilot | Active subscription (Individual, Business, or Enterprise) |
| Agent mode | Select **Agent** mode in the Copilot Chat panel |
| Runtime tools | Whatever your scripts need — Python, Node.js, **.NET CLI**, **az CLI**, etc. |

Agent Skills follow an open standard: [agentskills.io](https://agentskills.io). The post states they work across VS Code, GitHub Copilot CLI, and GitHub Copilot coding agent.

## Anatomy of a Skill
Example structure:

```text
.github/skills/my-skill/
├── SKILL.md                # Instructions (required)
└── references/
    ├── resources/
    │   ├── run.py          # Automation script
    │   ├── query-template.sql
    │   └── config.yaml
    └── reports/
        └── report_template.html
```

## The `SKILL.md` File
Each skill uses a consistent format with YAML frontmatter.

### Frontmatter example

```yaml
---
name: my-skill
description: 'What this does and when to use it. Include trigger phrases so Copilot knows when to load it. USE FOR: specific task A, task B. Trigger phrases: "keyword1", "keyword2".'
argument-hint: 'What inputs the user should provide.'
---
```

### Suggested body sections

- **When to Use**
- **Quick Start** (exact commands)
- **What It Does** (often as a table of steps)
- **Output**

Quick start example (as written in the post):

```powershell
cd .github/skills/my-skill/references/resources
py run.py <arg1> <arg2>
```

## Key Design Principles
1. **Description is discovery**: the `description` field is what Copilot uses to decide whether to load the skill, so include trigger phrases and keywords.
2. **Progressive loading**: Copilot reads only `name` + `description` (~100 tokens) for all skills, then loads full skill content only for matches; reference files load when referenced.
3. **Self-contained procedures**: include exact commands, parameter formats, and paths.
4. **Scripts do the heavy lifting**: the AI orchestrates; scripts execute for determinism and reproducibility.

## Example: Deployment Health Check Skill
A worked example builds a skill to query endpoints, compare to baselines, and generate an HTML report.

### Step 1 — Folder structure

```text
.github/skills/deployment-health/
├── SKILL.md
└── references/
    └── resources/
        ├── check_health.py
        └── endpoints.yaml
```

### Step 2 — `SKILL.md` frontmatter

```yaml
---
name: deployment-health
description: 'Check deployment health across environments. Queries health endpoints, compares response times against baselines, and flags degraded services. USE FOR: deployment validation, health check, post-deploy verification, service status. Trigger phrases: "check deployment health", "is the deployment healthy", "post-deploy check", "service health".'
argument-hint: 'Provide the environment name (e.g., staging, production).'
---
```

### Step 2 — What it does (outlined)
- Load endpoint definitions from `endpoints.yaml`
- Call each endpoint and record status + latency
- Compare against baseline thresholds
- Generate an HTML report

Output example:

- `references/reports/health_<environment>_<date>.html`

### Step 3 — Script (as shown, abridged)

```python
# check_health.py
import sys, yaml, requests, time, json
from datetime import datetime

def main():
    env = sys.argv[1]
    with open("endpoints.yaml") as f:
        config = yaml.safe_load(f)

    results = []
    for ep in config["endpoints"]:
        url = ep["url"].replace("{env}", env)
        start = time.time()
        resp = requests.get(url, timeout=10)
        elapsed = time.time() - start
        results.append({
            "service": ep["name"],
            "status": resp.status_code,
            "latency_ms": round(elapsed * 1000),
            "threshold_ms": ep["threshold_ms"],
            "healthy": resp.status_code == 200 and elapsed * 1000 < ep["threshold_ms"],
        })

    healthy = sum(1 for r in results if r["healthy"])
    print(f"Health check: {healthy}/{len(results)} services healthy")
    # ... generate HTML report ...

if __name__ == "__main__":
    main()
```

### Step 4 — Use it from Copilot
Ask Copilot in agent mode:

> Check deployment health for staging

Expected Copilot behavior:
1. Match against the skill description
2. Load `SKILL.md`
3. Run `python check_health.py staging`
4. Open the generated report
5. Summarize findings in chat

## More Skill Ideas (patterns)
Examples suggested in the post:

- Test Regression Analyzer
- API Contract Checker
- Security Scan Reporter
- Cost Analysis
- Release Notes Generator
- Infrastructure Drift Detector
- Log Pattern Analyzer
- Performance Benchmarker
- Dependency Auditor

The repeated pattern: **instructions (`SKILL.md`) + automation script + output template**.

## Tips for Writing Effective Skills

### Do
- Front-load the description with keywords
- Include exact commands (`cd ... && python ... <args>`)
- Document input/output clearly
- Use tables for multi-step procedures
- Include time zone conversion notes when timestamps matter
- Bundle HTML report templates

### Don’t
- Don’t use vague descriptions
- Don’t assume context (document paths/env vars)
- Don’t put everything in `SKILL.md` (use `references/` for large files)
- Don’t hardcode secrets (use environment variables or Azure Key Vault)
- Don’t skip error guidance

## Skill Locations
Skills can be stored at project or personal scope:

| Location | Scope | Shared with team? |
| --- | --- | --- |
| `.github/skills/<name>/` | Project | Yes |
| `.agents/skills/<name>/` | Project | Yes |
| `.claude/skills/<name>/` | Project | Yes |
| `~/.copilot/skills/<name>/` | Personal | No |
| `~/.agents/skills/<name>/` | Personal | No |
| `~/.claude/skills/<name>/` | Personal | No |

VS Code can be configured with additional locations using the `chat.skillsLocations` setting.

## Where Skills Fit in Copilot Customization
The post positions skills among other customization mechanisms:

| Primitive | Use When |
| --- | --- |
| Workspace Instructions (`.github/copilot-instructions.md`) | Always-on rules (standards, naming, architecture) |
| File Instructions (`.github/instructions/*.instructions.md`) | Rules scoped to file patterns |
| Prompts (`.github/prompts/*.prompt.md`) | Single-shot tasks with parameters |
| Skills (`.github/skills/<name>/SKILL.md`) | Multi-step workflows with scripts/templates |
| Custom Agents (`.github/agents/*.agent.md`) | Isolated subagents / restricted tool access / multi-stage pipelines |
| Hooks (`.github/hooks/*.json`) | Deterministic shell commands at lifecycle events |
| Plugins | Installable bundles (see [awesome-copilot](https://github.com/github/awesome-copilot)) |

## Slash Commands & Quick Creation
- Skills show up as **slash commands** in chat (type `/` to see them)
- You can pass context after the command (examples from the post):

```text
/deployment-health staging
/webapp-testing for the login page
```

Creation shortcuts:
- Type `/create-skill` and describe what you want; Copilot generates the `SKILL.md` and structure.
- After solving an issue in chat, ask to “create a skill” from the conversation to capture the procedure.

## Controlling When Skills Load
Frontmatter properties control discoverability and invocation:

| Configuration | Slash command? | Auto-loaded? | Use case |
| --- | --- | --- | --- |
| Default (both omitted) | Yes | Yes | General-purpose skills |
| `user-invocable: false` | No | Yes | Background knowledge loaded when relevant |
| `disable-model-invocation: true` | Yes | No | Run only on demand |
| Both set | No | No | Disabled skills |

## The Open Standard
Agent Skills are described as portable across:

- GitHub Copilot in VS Code
- GitHub Copilot CLI
- GitHub Copilot coding agent
- Claude Code, Gemini CLI (via `.claude/skills/` and `.agents/skills/`)

## Further Reading
- [Official Agent Skills docs](https://code.visualstudio.com/docs/copilot/customization/agent-skills)
- [Community skills & plugins (awesome-copilot)](https://github.com/github/awesome-copilot)
- [Anthropic reference skills](https://github.com/anthropics/skills)


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/supercharge-your-dev-workflows-with-github-copilot-custom-skills/ba-p/4510012)

