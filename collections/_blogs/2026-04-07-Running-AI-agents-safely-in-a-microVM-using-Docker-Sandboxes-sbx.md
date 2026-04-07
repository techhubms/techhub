---
feed_name: Andrew Lock's Blog
section_names:
- devops
title: Running AI agents safely in a microVM using Docker Sandboxes (sbx)
tags:
- 1Password
- AI
- Allowlist
- Blogs
- Branch Mode
- Commit Signing
- Core.excludesfile
- Developer Workflow
- DevOps
- Direct Mode
- Docker
- Docker Sandboxes
- Getting Started
- Git
- Git Worktree
- Gitignore
- HypervisorPlatform
- Installation
- Isolated Development Environment
- Local Development Security
- Microvm
- Network Policy
- PowerShell
- Proxy
- Sandboxing
- Sbx
- SSH Agent
- TUI Dashboard
- Windows 11
- WinGet
primary_section: devops
external_url: https://andrewlock.net/running-ai-agents-safely-in-a-microvm-using-docker-sandbox/
date: 2026-04-07 10:00:00 +00:00
author: Andrew Lock
---

Andrew Lock explains how to run AI coding agents in Docker Sandboxes using the sbx tool, so you can use “dangerous”/YOLO-style agent modes while keeping your host machine isolated, with practical setup steps, network policy notes, and git workflow tips.<!--excerpt_end-->

## Overview

This post describes a practical workflow for running AI coding agents locally in a safer way by using **Docker Sandboxes** (microVM-based isolation) and the **`sbx`** CLI.

The core goal: get the productivity of “dangerous/YOLO” agent modes (no constant tool permission prompts) while reducing the blast radius if the agent runs destructive commands.

## Why “dangerous” agent modes are tempting (and risky)

Many coding agents require frequent confirmations for tool usage (e.g., running `grep`, `sed`, changing directories). This can become a constant interruption.

Claude Code supports bypassing these prompts via permission flags:

- `--allow-dangerously-skip-permissions` enables the bypass mode option
- `--permission-mode bypassPermissions` or `--dangerously-skip-permissions` starts directly in bypass

Example:

```bash
claude --allow-dangerously-skip-permissions
```

The downside is obvious: once you bypass tool permissions, the agent can run destructive commands without asking.

## Docker Sandboxes: microVM isolation (not just containers)

Docker Sandboxes are built on **isolated microVM sandboxes** (not standard containers). The post highlights security properties from Docker’s documentation:

- Each sandbox has its **own kernel** (unlike containers which share the host kernel)
- The microVM runs a **separate Docker engine inside the VM**, avoiding mounting the host Docker socket
- The sandbox network is **isolated**; outbound traffic goes through a **host-side proxy** that:
  - blocks access to the host’s localhost
  - can enforce allow/deny rules
  - can inject auth headers so the sandbox itself doesn’t get direct access to them

> Note: Docker Sandboxes are described as experimental, and Docker has shifted from Docker Desktop integration to a dedicated `sbx` tool.

## Installing `sbx` on Windows

### 1) Enable HypervisorPlatform

Run this in an elevated PowerShell:

```powershell
Enable-WindowsOptionalFeature -Online -FeatureName HypervisorPlatform -All
```

### 2) Install `sbx`

Install with WinGet:

```powershell
winget install -h Docker.sbx
```

Or download the MSI from the releases page:

- https://github.com/docker/sbx-releases/releases

Open a new terminal after installation so `sbx` is on your PATH.

## Sign in and choose a default network policy

Sign in:

```powershell
sbx login
```

During setup, you choose a default network policy:

1. **Open** — allow all outbound traffic
2. **Balanced** — default deny + allow common developer/AI sites
3. **Locked Down** — block all unless explicitly allowed

The author notes that “Balanced” can be restrictive in practice (e.g., documentation sites such as Microsoft Learn may be blocked), and they switched back to **Open** using:

```powershell
sbx policy reset
```

## Create and run a sandboxed agent

From your project directory, run an agent in a sandbox:

```powershell
cd .\NetEscapades.EnumGenerators
sbx run claude
```

What happens:

- `sbx` downloads the relevant agent template image
- spins up a microVM sandbox
- mounts your working directory into the sandbox
- runs the agent in a “dangerous/bypass permissions” style mode so it can operate without constant prompts

The author’s key safety point: the agent can affect the mounted workspace (for example, it could delete your repo), but it’s isolated from the rest of the host machine.

Docker Sandboxes (at the time of writing) support multiple agents, including:

- Claude Code, Codex, Copilot, Gemini, Kiro, OpenCode, Docker Agent

## Git workflows: direct mode vs branch mode

### Direct mode

- The agent edits files directly in your working directory
- It can commit directly to the repo
- Trade-off: it has access to the full `.git` data, so in theory it could damage repository state

### Branch mode (git worktree)

Start branch mode with `--branch`:

```bash
# Creates a worktree at .sbx/<sandbox-name>-worktrees/my-feature
sbx run claude --branch my-feature

# Auto-generates the branch name
sbx run claude --branch auto
```

This creates a worktree under `.sbx/` in the repo. That typically shows up as untracked files, so you need to ignore it.

#### Ignore `.sbx/` globally

The post provides a PowerShell script to add `.sbx/` to your global git ignore file (via `core.excludesFile`, or defaulting to `$HOME/.config/git/ignore`):

```powershell
# Get the path to the default ignore file
$path = git config --global core.excludesFile
if (-not $path) { $path = "$HOME/.config/git/ignore" }

# Create the parent directory
New-Item -ItemType Directory -Force -Path (Split-Path $path) | Out-Null

# Add .sbx/ to the file
Add-Content -Path $path -Value ".sbx/"
```

A noted gotcha: creating your own worktree manually and running a sandbox inside it may break committing, because the agent may not be able to access the “parent” git repo.

## Managing sandboxes and using the dashboard

`sbx` includes commands for listing/managing sandboxes (`ls`, `rm`, `stop`, `exec`, etc.).

It also includes a TUI “dashboard” view when you run `sbx` without arguments after logging in, showing:

- running sandboxes
- resource usage (memory, uptime)
- network requests
- global network rules

## Limitations and trade-offs called out

- **Resource limits**: older Docker Desktop-based sandboxes had a hard 4GB RAM limit; `sbx` supports `--memory` and defaults to 50% of host memory.
- **Commit signing**: the author couldn’t get 1Password-backed commit signing working inside the sandbox and uses a workaround (unsigned commits in sandbox, then rebase/sign on host).
- **Network policies**: useful for reducing blast radius, but may be too restrictive for day-to-day agent use if you need many documentation sites.
- **Performance**: microVM overhead can be significant and was a deal-breaker for the author in some cases.

## Summary

The post walks through using **Docker Sandboxes + `sbx`** to run coding agents locally in a more isolated environment so you can use bypass/YOLO-style modes without continuously approving tool calls. It covers installation on Windows, network policy choices, running a sandboxed agent, git workflows (direct vs branch mode), and practical limitations like performance and commit signing.

[Read the entire article](https://andrewlock.net/running-ai-agents-safely-in-a-microvm-using-docker-sandbox/)

