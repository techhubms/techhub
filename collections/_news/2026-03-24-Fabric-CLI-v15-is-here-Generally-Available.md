---
date: 2026-03-24 14:00:00 +00:00
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
title: Fabric CLI v1.5 is here (Generally Available)
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-v1-5-is-here-generally-available/
section_names:
- ai
- devops
- ml
tags:
- Agent Skills
- AI
- AI Agents
- Azure DevOps Pipelines
- CI/CD
- Claude
- Cursor
- DevOps
- Fabric Cicd
- Fabric CLI
- Fabric Notebooks
- Federated Credentials
- GitHub Actions
- GitHub OIDC
- JMESPath
- Microsoft Fabric
- ML
- Ms Fabric CLI
- News
- OneLake
- Power BI
- Prompt Templates
- PySpark
- Python 3.13
- REPL
- Report Rebinding
- Semantic Model Refresh
- Service Principal Authentication
- Workspace Deployments
primary_section: ai
---

Microsoft Fabric Blog (co-authored with Jeremy Hoover) announces Fabric CLI v1.5 GA, highlighting one-command CI/CD deployments, expanded Power BI automation, running the CLI inside Fabric notebooks, and new assets that help AI agents reliably execute Fabric operations through the CLI.<!--excerpt_end-->

## Overview

Fabric CLI v1.5 (Generally Available) expands the open-sourced CLI with stronger deployment automation, deeper Power BI support, improved notebook workflows, and features aimed at making the CLI a reliable execution surface for AI agents.

Since the open-source launch, updates include:

- One-command deployments
- First-class Power BI support
- An AI agent execution layer
- Interactive REPL mode
- Python 3.13 support
- Community contributions shipping in released versions (v1.2–v1.5)

## What’s new (in a nutshell)

1. **CI/CD deployments from the CLI**: a new `deploy` command integrates the `fabric-cicd` library to enable full workspace deployments from a single command.
2. **Power BI scenario support**: report rebinding, semantic model refresh, and property management from the CLI.
3. **CLI in Fabric Notebooks**: pre-installed and pre-authenticated in PySpark notebooks, turning notebooks into a remote execution surface for CLI scripts.
4. **AI agent execution layer**: agent instructions, custom agent-skills, REPL mode, and improved error messages/docs to make agent-driven Fabric operations more reliable.

Plus: JMESPath filtering, multi-format import/export, 30+ item types supported, and more.

![Screenshot of the .ai-assets folder in the open-sourced GitHub repo of the Fabric CLI.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-ai-assets-folder-in-the-open-so.png)

## CI/CD: deploy workspaces in one command

### New `deploy` command (v1.5)

Fabric CLI v1.5 adds a `deploy` command that wraps the [`fabric-cicd`](https://github.com/microsoft/fabric-cicd) Python library.

Example:

```bash
fab deploy --config deployment-config.yaml
```

This works:

- In local terminals
- In **GitHub Actions**
- In **Azure DevOps Pipelines**
- Anywhere else the CLI runs

### Authentication options mentioned

- **Service principal authentication**:

```bash
fab auth login -u <client_id> -p <secret> --tenant <tenant_id>
```

- **Federated credentials for GitHub OIDC**: see the docs on federated credentials for GitHub OIDC: https://microsoft.github.io/fabric-cli/authentication/#federated-credentials

### What this enables

- **Git-based promotion**: author in a Git-connected workspace, then deploy to staging/production; also import exported items using parametrization or deployment config.
- **Pipeline-native deployments**: run Fabric deployments alongside application deployments in the same CI/CD YAML with consistent triggers and audit trails.
- **Config reuse**: existing `fabric-cicd` config files can be reused directly.

References:

- `fabric-cicd` docs: https://github.com/microsoft/fabric-cicd
- CLI CI/CD guide: https://microsoft.github.io/fabric-cli/examples/#cicd-integration

## Power BI: first-class scenario support

The CLI expands Power BI support via the API command and enhanced commands.

### Capabilities called out

- **Rebind reports via `set`**: the enhanced `set` command (v1.3.1+) supports any settable property path within an item’s definition/metadata, including rebinding a report to a different semantic model.
- **Manage Power BI properties via `set`**: update semantic model properties, refresh schedules, and other settings.
- **Semantic model refresh**: trigger a semantic model refresh and optionally wait for completion.

Documentation links:

- Semantic model refresh example: https://microsoft.github.io/fabric-cli/examples/refresh_semantic_model_example/
- Jobs command docs: https://microsoft.github.io/fabric-cli/commands/jobs/
- `set` command docs: https://microsoft.github.io/fabric-cli/commands/fs/set/

## CLI in Fabric notebooks

Fabric CLI is now **pre-installed and pre-authenticated** in **PySpark notebooks**, so you can run CLI commands directly from notebook cells.

### Example commands shown

Explore environment:

```bash
!fab ls
```

Run a pipeline (example path):

```bash
!fab job run "My Workspace.Workspace/Daily-ETL.DataPipeline"
```

Combine CLI output with Python logic:

```python
import json, subprocess

result = subprocess.run(
    ["fab", "ls", "-l", "--output_format", "json"],
    capture_output=True,
    text=True
)

output = json.loads(result.stdout)
workspaces = output["result"]["data"]

for ws in workspaces:
    print(f"Workspace: {ws['name']}")
```

### Why it matters

This turns notebooks into a remote execution surface for automation, enabling scenarios like:

- Triggering a downstream pipeline after a Spark job completes
- Scheduled runbooks (nightly backups, weekly permission audits, environment sync)
- Self-contained operational documentation alongside executable commands

## AI agents: the CLI as an execution layer

The post notes that developers are connecting Fabric CLI to AI coding assistants (including **GitHub Copilot**, Claude, Cursor) and using natural language to operate Fabric.

### What was built for agents

- **AI assets folder**: https://github.com/microsoft/fabric-cli/tree/main/.ai-assets
  - Includes skills, context files, prompts, and modes
  - Covers execution patterns (variable extraction, chaining, error handling)
  - Includes guardrails and scenario templates
- **Contributor agent instructions** (`AGENTS.md`): https://github.com/microsoft/fabric-cli/blob/main/AGENTS.md
  - Structured guidance that an AI assistant can consume to generate valid `fab` commands without inventing flags/commands
- **Improved error messages**
  - More actionable failure output to help humans and agents diagnose and self-correct

### Rationale

Using a CLI as the execution layer helps avoid providing agents with large REST API specs (endpoints/auth/payload details). The post references Anthropic’s write-up on tool use via code execution:

- https://www.anthropic.com/engineering/code-execution-with-mcp

## Release highlights (v1.2–v1.5)

Notable items listed:

- `deploy` command (v1.5): workspace deployments with `fabric-cicd`
- REPL mode (v1.4): run `fab` for an interactive, authenticated session
- Notebook export formats (v1.4): `fab export --format ipynb`
- Python 3.13 support (v1.4)
- Version notification on login (v1.4)
- API response data in output (v1.4)
- JMESPath filtering in `ls` (v1.3.1):

```bash
fab ls -q "[?type=='Notebook']"
```

- Enhanced `set` command (v1.3.1)
- `job run-rm` (v1.3.1)

The CLI also expanded item type coverage (examples named in the post include CosmosDB Databases, User Data Functions, Digital Twin Builders, Graph Query Sets, Dataflows, GraphQL APIs, Variable Libraries, Copy Jobs, SQL Databases) with CRUD-style operations for many types.

Full release notes:

- https://microsoft.github.io/fabric-cli/release-notes/

## Built by the community

The post highlights that community pull requests have shipped in production releases since the CLI was open-sourced in October 2025, spanning features, bug fixes, optimizations, and documentation.

It lists contributors whose work shipped in v1.0–v1.5 (with links to their GitHub profiles) and encourages opening issues/discussions/PRs:

- Issues: https://github.com/microsoft/fabric-cli/issues

## Upcoming ideas

Areas mentioned as planned or being explored:

- **Azure Identity support**: authenticate using an existing `az login` session
- **Multi-tenancy support**: switch tenants in a CLI session
- **Soft delete support**: list/restore/permanently delete soft-deleted items
- **Docker image** for consistent CI/CD environments
- **Community scripts library** for reusable operational scripts
- **Remote execution options** (Fabric Cloud Shell / browser terminal / deeper notebook integration)
- **IntelliSense in VS Code** for CLI command autocomplete

## Get started

Install/upgrade:

```bash
pip install ms-fabric-cli --upgrade
```

Start REPL:

```bash
fab
```

Then (inside the shell):

```bash
ls
```

Links:

- Documentation: https://microsoft.github.io/fabric-cli/
- GitHub repo: https://github.com/microsoft/fabric-cli
- Full release notes: https://microsoft.github.io/fabric-cli/release-notes/
- AI assets for agents: https://github.com/microsoft/fabric-cli/tree/main/.ai-assets


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-v1-5-is-here-generally-available/)

