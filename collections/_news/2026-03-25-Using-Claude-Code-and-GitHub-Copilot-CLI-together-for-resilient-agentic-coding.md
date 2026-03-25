---
external_url: https://devblogs.microsoft.com/all-things-azure/your-entire-engineering-floor-just-stopped-coding/
section_names:
- ai
- azure
- devops
- github-copilot
title: Using Claude Code and GitHub Copilot CLI together for resilient agentic coding
primary_section: github-copilot
tags:
- Agentic Coding
- Agents
- AGENTS.md
- AI
- AKS
- All Things Azure
- Autonomous Agents
- Azure
- Azure AI Foundry
- Azure AI Search
- Azure App Service
- Azure Cosmos DB
- Azure Key Vault
- Azure MCP
- Bicep
- Claude Code
- CLAUDE.md
- Codespaces
- Custom Agents
- Dev Containers
- Developer Productivity
- DevOps
- GitHub Copilot
- GitHub Copilot CLI
- Hooks
- MCP
- MCP (model Context Protocol)
- MCP Servers
- Model Routing
- Multi Model
- News
- Opinion
- Pre Commit Review
- Skills Directory
- Subagents
- Thought Leadership
author: Desi Villanueva
date: 2026-03-25 03:38:23 +00:00
feed_name: Microsoft All Things Azure Blog
---

Desi Villanueva argues for running Claude Code and GitHub Copilot CLI side-by-side so agentic workflows keep moving during model-provider slowdowns, and shows how to share instructions, skills, agents, MCP servers, and hook logic across both tools with minimal duplication.<!--excerpt_end-->

# Using Claude Code and GitHub Copilot CLI together for resilient agentic coding

## Why run two agent tools?
Model-provider degradation is part of day-to-day operations for autonomous agent workflows: you can get slow responses and elevated API errors that don’t show up as a full “red” outage on a status page.

The article’s core idea is to run **two different paths to models**:

- **Claude Code** connects directly to **Anthropic’s API**, fanning out across different inference backends.
- **GitHub Copilot CLI** connects through **GitHub’s infrastructure**, and can switch models via `/model`.

Because the tools keep **separate session state** but can share a lot of **repo-level configuration**, you can switch quickly when one path is degraded.

## They’re not competitors: what each tool is good at

### Claude Code excels at

- **Deep autonomous coding sessions** (multi-hour plan/build/test/iterate loops)
- **Subagent orchestration** (specialized agents like Explore/Plan/custom with isolated context)
- **Custom hooks and guardrails**, including programmatic pre/post tool-use control (JS/Python handlers)
- **Flexible model routing**, including routing via **Azure AI Foundry** or using GitHub as a proxy

### Copilot CLI excels at

- **Model-provider freedom via `/model`** (single subscription and switching across model families)
- **Native GitHub integration**, including a built-in GitHub MCP server (issues, PRs, code search, labels, Copilot Spaces)
- **Interactive plan mode** (`Shift+Tab` and `ask_user`)
- **Pre-commit review** via `/review` on staged changes
- **Plugin ecosystem** via `/plugin install owner/repo`
- **Codespaces integration** (included in default image; available as a Dev Container Feature)

## What you can share across both tools
The article claims the overlap is large enough that you’re mostly maintaining **one setup**.

| Feature | Status | What this means |
| --- | --- | --- |
| `CLAUDE.md` instructions | ✅ Native | Copilot CLI reads `CLAUDE.md` at repo root. |
| `AGENTS.md` | 🔗 Symlink | Symlink `AGENTS.md` → `CLAUDE.md` to keep one source of truth. |
| Skills (`.claude/skills/`) | ✅ Native | Both tools auto-load skills. |
| MCP servers | ↔️ Parallel config | Same server process, different config files. |
| Subagents | ↔️ Parallel definitions | Different file formats; share the prompt body. |
| Hooks (pre/post tool-use) | ↔️ Parallel plumbing | Call the same classifier/binary; register differently. |
| Memory | ➖ Separate | Independent systems; no conflicts. |
| Session state | ➖ Separate | `~/.claude/` vs `~/.copilot/` are isolated. |

## The five-minute setup (as described)

## Step 1: Symlink your instructions
Make `CLAUDE.md` the source of truth and symlink `AGENTS.md`:

```bash
ln -s CLAUDE.md AGENTS.md
```

Claude-specific syntax (like `@import` or subagent mentions) is described as being ignored by Copilot CLI.

## Step 2: Use the shared skills directory
If you have `.claude/skills/`, Copilot CLI will read it too. The article calls this the strongest interop point.

## Step 3: Mirror your agents
- Claude Code subagents: `.claude/agents/sentinel.md`
- Copilot custom agents: `.github/agents/sentinel.agent.md`

The YAML frontmatter differs, but the prompt body can be the same. A sample sync script is provided:

```bash
#!/bin/bash
# sync-agents.sh — keep agent definitions in sync
for agent in .claude/agents/*.md; do
  name=$(basename "$agent" .md)
  body=$(sed '1,/^---$/d' "$agent" | sed '1,/^---$/d')
  desc=$(grep 'description:' "$agent" | head -1 | sed 's/description: //')

  cat > ".github/agents/${name}.agent.md" << EOF
---
name: ${name}
description: ${desc}
---
${body}
EOF

done
```

The suggestion is to run this from a pre-commit hook or CI step.

## Step 4: Configure MCP in both places
Both tools connect to the same MCP server process, but registration differs:

- Claude Code: `.claude/mcp.json` using `"type": "stdio"`
- Copilot CLI: `.devcontainer/devcontainer.json` using `"type": "local"`

## Step 5: Share hook logic
If you run safety hooks (the author mentions a PostToolUse classifier chain with **ModernBERT**), extract the logic into a standalone CLI so both tools can call it via stdin/stdout.

## Example repo structure

```text
your-repo/
├── CLAUDE.md                     # [shared] Source of truth
├── AGENTS.md -> CLAUDE.md         # [shared] Symlink
│
├── .claude/
│   ├── mcp.json                   # [Claude Code] MCP config
│   ├── settings.json              # [Claude Code] Hooks + settings
│   ├── agents/                    # [Claude Code] Subagents
│   │   ├── sentinel.md
│   │   └── code-reviewer.md
│   └── skills/                    # [shared] Both tools read
│       ├── sec-analysis/SKILL.md
│       └── deployment/SKILL.md
│
├── .github/
│   ├── copilot-instructions.md    # [Copilot] Extra instructions
│   └── agents/                    # [Copilot] Custom agents
│       ├── sentinel.agent.md
│       └── code-reviewer.agent.md
│
├── .devcontainer/
│   └── devcontainer.json          # [Copilot] MCP config
│
├── mcp-servers/                   # [shared] Server source
│   └── guardrail/
│       ├── index.js
│       └── classifier.js
│
└── scripts/
    └── sync-agents.sh             # [shared] Agent sync
```

## “Bigger picture”: Azure-oriented multi-agent orchestration
The article frames **Copilot CLI** as especially strong for Azure work due to “native Azure MCP tools” and calls out:

- Azure Cosmos DB
- Azure App Service
- AKS (Azure Kubernetes Service)
- Azure Key Vault
- Bicep schemas
- deployment planning

It also suggests a possible next step: a **shared memory layer** via an MCP server using an **Azure AI Search memory layer**, bridging Claude Code’s `MEMORY.md` and Copilot’s memory.

## References

- GitHub Copilot CLI — Getting started: https://docs.github.com/en/copilot/how-tos/copilot-cli
- GitHub Copilot CLI — Custom agents: https://docs.github.com/en/copilot/how-tos/copilot-cli/customize-copilot/create-custom-agents-for-cli
- GitHub Copilot CLI — Custom instructions: https://docs.github.com/en/copilot/how-tos/copilot-cli/customize-copilot/add-custom-instructions
- Copilot CLI GA announcement: https://github.blog/changelog/2026-02-25-github-copilot-cli-is-now-generally-available/
- GitHub Copilot CLI — Available models: https://docs.github.com/en/copilot/how-tos/copilot-cli/using-copilot-cli/change-the-ai-model
- Azure AI Foundry — Platform overview: https://learn.microsoft.com/en-us/azure/ai-foundry/
- Microsoft Agent Framework (MAF): https://devblogs.microsoft.com/agent-framework
- AGENTS.md specification (Linux Foundation): https://github.com/agentsmd/agents.md
- Model Context Protocol (MCP): https://modelcontextprotocol.io/
- Claude Code — Subagents documentation: https://code.claude.com/docs/en/sub-agents


[Read the entire article](https://devblogs.microsoft.com/all-things-azure/your-entire-engineering-floor-just-stopped-coding/)

