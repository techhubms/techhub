---
section_names:
- ai
- azure
- github-copilot
tags:
- Agent Handoff Pattern
- AgentBaker
- Agents
- AI
- AI Agents
- AKS
- All Things Azure
- ARO
- Azure
- Azure Architecture Center
- Azure Red Hat OpenShift
- Cloud Provider Azure
- Customer Managed Keys
- Developer Productivity
- Fact Checking
- GitHub Copilot
- GitHub Copilot CLI
- Grounded AI
- KMS Encryption
- Kubernetes
- MCP
- MCP Server
- Mermaid Diagrams
- Multi Agent Systems
- News
- Operations
- Project Nighthawk
- Source Code Analysis
- Technical Research
- Terraform
- VS Code
feed_name: Microsoft All Things Azure Blog
date: 2026-04-01 04:24:38 +00:00
external_url: https://devblogs.microsoft.com/all-things-azure/project-nighthawk-a-research-agent-built-for-field-engineering/
title: 'Project Nighthawk: A Research Agent Built for Field Engineering'
primary_section: github-copilot
author: Diego Casati, Ray Kao
---

Diego Casati and Ray Kao introduce Project Nighthawk, a VS Code + GitHub Copilot-based multi-agent research workflow that produces source-cited, fact-checked markdown reports for deep AKS/ARO questions by grounding answers in local repos, Microsoft Learn, and release notes.<!--excerpt_end-->

# Project Nighthawk: A Research Agent Built for Field Engineering

If you work in field engineering, you’ve likely seen the same pattern: a customer deploys **AKS** in a regulated environment, hits an issue (often during **node bootstrapping**), and needs a precise, version-accurate explanation of what happens when a node joins a cluster. That answer is scattered across source code, documentation, release notes, and tribal knowledge—and assembling it repeatedly doesn’t scale.

**Project Nighthawk** is built to close that gap: a targeted research pipeline for AKS and **Azure Red Hat OpenShift (ARO)** that retrieves, verifies, and writes up findings as a reusable technical report.

## What Nighthawk Is

Nighthawk is a **multi-agent research system** built inside **VS Code** using **GitHub Copilot**. The goal is to automate the *research and verification* so the engineer can focus on communication and judgment.

You ask a technical question about AKS or ARO, and Nighthawk produces a **fact-checked, source-cited technical report in markdown**—based on an investigation against real sources rather than model recall.

Example entry point:

```text
/Nighthawk How does AKS implement KMS encryption with customer-managed keys?
```

Behind this is a pipeline that:

- classifies the question
- researches against live source code and official docs
- synthesizes a structured report
- validates every claim
- writes the report into `notes/`

## The Problem with Asking AI to Research

The authors argue that asking an LLM directly is unreliable for Azure engineering work because:

- Azure behavior changes frequently
- code paths get refactored between releases
- a model can confidently describe outdated implementation details

Nighthawk’s answer is **grounding**: make agents consult current source code and official docs, correlate across sources, and surface conflicts.

## Grounding via Local Repositories

A core design choice: researchers work against **locally cloned repositories**.

One-time setup example:

```bash
git clone --depth=1 https://github.com/Azure/AgentBaker.git repos/AgentBaker
git clone --depth=1 https://github.com/Azure/AKS.git repos/AKS
git clone --depth=1 https://github.com/kubernetes-sigs/cloud-provider-azure.git repos/cloud-provider-azure
```

Before each run, update the repo (example):

```bash
git -C repos/AgentBaker pull --ff-only
```

This ensures the model is working against the **current state of the codebase**.

## Six Agents, One Pipeline

Nighthawk implements the **Agent Handoff Pattern** as described in the Azure Architecture Center guide:

- Azure Architecture Center AI Agent Design Patterns: [Agent handoff pattern example](https://learn.microsoft.com/en-us/azure/architecture/ai-ml/guide/ai-agent-design-patterns#agent-handoff-pattern-example)

Pipeline sketch from the post:

```text
/Nighthawk question
  |
  v
[Orchestrator]  <-- coordinates the workflow
  |
  v
[Classifier]    <-- AKS or ARO? what question type?
  |
  v
[Researcher]    <-- searches local repos + Microsoft Learn
  |
  v
[Synthesizer]   <-- writes the structured report
  |
  v
[FactChecker]   <-- validates every claim against sources
  |
  v
notes/Nighthawk-<date>-<topic>.md
```

### Agent responsibilities

- **Orchestrator**: coordinates the workflow and stage handoffs.
- **Classifier**: determines service (AKS/ARO), question type (architecture/bug/guidance), and extracts keywords.
- **Researchers**: search local repos and read files in-depth; pull official docs via the **Microsoft Learn MCP server**. Uses tools like `grep_search` and `read_file`.
- **Synthesizer**: converts research notes into a structured report using predefined templates; generates **Mermaid** diagrams when helpful.
- **FactChecker**: validates each factual claim against cited sources; flags what can’t be verified and summarizes verified vs unverified counts.

## What a Report Looks Like

For a question like *“What are the required permissions for Terraform-based AKS deployment?”*, the post describes output sections such as:

- **TL;DR** (direct answer)
- **Recommendations table** (roles mapped to scope and reason)
- **Terraform examples** (HCL patterns)
- **Feature-specific guidance** (BYO VNet, private DNS, ACR, workload identity)
- **Mermaid diagrams** (flows/relationships/decision trees)
- **Reference table** (Microsoft Learn links + GitHub file paths)
- **Fact-check summary** (verified/unverified claims)

The post points to an example report in the `notes/` directory:

- `../notes/Nighthawk-2026-03-31-AKS-Terraform-Permissions.md`

## Skills: Encoding Expertise as Reusable Instructions

Nighthawk uses **VS Code agent skills** (markdown files) to codify operational context and report structure.

- `Nighthawk-LocalRepos`: tells researchers which repos exist locally, what each repo is for, and why `git pull` is mandatory.
- `Nighthawk-ReportTemplates`: defines report formats (architecture/bug/guidance), required sections (e.g., TL;DR, Technical Deep Dive, Key Findings, References), and Mermaid conventions.

The authors’ point: keep agent definitions focused and load durable domain/process knowledge on demand via skills.

## What Nighthawk Is Not

Nighthawk is deliberately scoped:

- not a general-purpose Azure assistant
- does not browse the web
- does not query live Azure APIs
- focuses on AKS and ARO to maintain quality through constraints

Adding a new Azure service would mean creating a new researcher agent that follows the same contract.

## Getting Started (As Described)

Prerequisites: **VS Code with GitHub Copilot** and access to the repo.

Example command:

```text
/Nighthawk What are the networking options for ARO private clusters?
```

Setup guide reference:

- `../USAGE.md`

Architecture rationale reference:

- `../ARCHITECTURE-DECISION-FRAMEWORK.md`

Repository link as written in the post:

- https://github.com/your-org/project-nighthawk

## Key takeaway

The design thesis is that **quality comes from constraints**:

- restricting researchers to known repos improves accuracy
- separating synthesis from fact-checking catches more errors
- separation of concerns applies to agent systems just like software


[Read the entire article](https://devblogs.microsoft.com/all-things-azure/project-nighthawk-a-research-agent-built-for-field-engineering/)

