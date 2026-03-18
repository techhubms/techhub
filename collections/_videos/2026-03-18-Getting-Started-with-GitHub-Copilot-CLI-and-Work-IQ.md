---
section_names:
- ai
- devops
- github-copilot
external_url: https://www.youtube.com/watch?v=tQlNq8bH674
primary_section: github-copilot
tags:
- Agentic CLI
- AI
- Azure
- Azure Cloud
- CLI
- Command Line Interface
- Copilot
- Copilot Plugins
- DevOps
- EULA
- GitHub
- GitHub Authentication
- GitHub Copilot
- GitHub Copilot CLI
- Inference Models
- Johnsavillstechnicaltraining
- M365
- M365 Copilot
- M365copilot
- Microsoft
- Microsoft Azure
- Node.js
- Non Interactive CLI Usage
- Onboard to Azure
- Plugin Installation
- PowerShell 7
- Videos
- Windows Terminal
- Winget
- Work IQ
author: John Savill's Technical Training
title: Getting Started with GitHub Copilot CLI and Work IQ
feed_name: John Savill's Technical Training
date: 2026-03-18 09:57:04 +00:00
---

John Savill's Technical Training walks through installing and using GitHub Copilot CLI plus the Work IQ plugin, including prerequisites (PowerShell 7, Windows Terminal, Node.js), login/auth steps, and examples of interactive and non-interactive agentic CLI usage.<!--excerpt_end-->

# Getting Started with GitHub Copilot CLI and Work IQ

John Savill's Technical Training demonstrates how to set up **GitHub Copilot CLI** and add the **Work IQ** plugin, with a practical, step-by-step install and first-use walkthrough.

## What this video covers

- Why use CLIs
- Installing prerequisites and GitHub Copilot CLI
- Installing and enabling the Work IQ plugin
- First “agentic” usage in the CLI
- Authentication flows (including Microsoft 365 authentication mentioned in the chapter list)
- Interactive vs non-interactive usage
- Selecting different inferencing models (as referenced in the chapter list)

## Step-by-step setup (Windows)

### 1) Create / access a GitHub account

- Sign up: https://github.com/signup

### 2) Install prerequisites

> Note: the description calls out these commands as case sensitive.

Install **PowerShell 7**:

```powershell
winget install -e --id Microsoft.PowerShell
```

Install **Windows Terminal**:

```powershell
winget install -e --id Microsoft.WindowsTerminal
```

Install **Node.js**:

```powershell
winget install -e --id OpenJS.NodeJS
```

### 3) Install GitHub Copilot CLI

```powershell
winget install -e --id GitHub.Copilot
```

### 4) Install the Work IQ plugin

```powershell
/plugin install workiq@copilot-plugins
```

### 5) Accept the Work IQ EULA

```powershell
workiq accept-eula
```

## Key reference link

- GitHub Copilot CLI getting started: https://docs.github.com/en/copilot/how-tos/copilot-cli/cli-getting-started

## Chapters (from the video description)

- 00:00 - Introduction
- 00:11 - Why CLIs
- 01:16 - GitHub Copilot CLI
- 02:29 - Work IQ
- 05:19 - Organization onboarding
- 06:22 - Things I need (accounts)
- 08:03 - Things to install on my machine
- 09:05 - Core components
- 11:20 - Non Windows installation
- 11:48 - Using Terminal
- 13:40 - Launching the GitHub Copilot CLI
- 14:18 - Logging into GitHub
- 15:41 - My first CLI agentic use
- 16:06 - Adding Work IQ plug-in
- 17:23 - Accepting the EULA
- 18:01 - New Work IQ skill
- 18:52 - Using Work IQ
- 19:22 - Authenticating to M365
- 20:31 - Other types of interaction
- 23:22 - Using other inferencing models
- 23:49 - Closing and resuming
- 24:21 - Non-interactive use


