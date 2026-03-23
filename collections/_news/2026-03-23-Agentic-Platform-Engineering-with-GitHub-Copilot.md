---
external_url: https://devblogs.microsoft.com/all-things-azure/agentic-platform-engineering-with-github-copilot/
tags:
- Agentic DevOps
- Agentic Platform Engineering
- AgenticDevOps
- AI
- AKS
- AKS MCP Server
- All Things Azure
- Argo CD
- Argo CD Notifications
- Azure
- Azure Service Operator (aso)
- AzureGlobalBlackBelts
- Bicep
- Cluster API For Azure (capz)
- Containers
- Developer Productivity
- DeveloperExperience
- DevOps
- Ebpf
- GitHub Actions
- GitHub Copilot
- GitHub Copilot CLI
- GitOps
- IaC
- Incident Response Automation
- Inspektor Gadget
- Internal Developer Portal
- Kubectl
- Kubernetes
- Model Context Protocol (mcp)
- News
- Platform Engineering
- PlatformEngineering
- Repository Dispatch
- SRE
- Terraform
- Workload Identity Federation
title: Agentic Platform Engineering with GitHub Copilot
date: 2026-03-23 19:26:27 +00:00
author: Diego Casati, Ray Kao
section_names:
- ai
- azure
- devops
- github-copilot
primary_section: github-copilot
feed_name: Microsoft All Things Azure Blog
---

Diego Casati and Ray Kao outline a practical “agentic platform engineering” approach using GitHub Copilot: start by capturing tribal knowledge in repo-grounded prompts, then enforce standards in GitHub Actions, and finally run a Kubernetes “Cluster Doctor” agent that can diagnose AKS issues and propose fixes via pull requests with humans approving changes.<!--excerpt_end-->

# Agentic Platform Engineering with GitHub Copilot

We’ve talked about the [human scale problem](https://devblogs.microsoft.com/all-things-azure/the-human-scale-problem-in-platform-engineering/) and what happens [when infrastructure scales but understanding doesn’t](https://devblogs.microsoft.com/all-things-azure/when-infrastructure-scales-but-understanding-doesnt/). The thesis: our tools have outpaced our ability to operate them, and platform engineering is how we’re fighting back.

The missing piece is that we’ve been encoding knowledge into runbooks that go stale, documentation that drifts, and tribal expertise that disappears when people leave. The authors’ proposal is **agentic platform engineering**: don’t replace humans—give the platform the ability to observe, reason, and act, with humans still in the pilot seat.

Companion repo you can run:

- https://github.com/microsoftgbb/agentic-platform-engineering

A companion walkthrough is referenced on YouTube (link not provided in the input), and the repo is organized into three “acts” with agent definitions, GitHub Actions workflows, MCP configs, and sample Argo CD manifests.

## The paradox of choice in platform engineering

Platform engineering has a huge tool landscape (“the eye chart”). The point isn’t that tools exist—it’s that no single person can hold the details of how to use each tool and how to compose them.

A common, repeatable pattern the authors see with customers:

- Developer self-service via an internal developer portal
- A management cluster running:
  - Cluster API for Azure (CAPZ)
  - Azure Service Operator (ASO)
- GitOps with Argo CD syncing desired state from config repos
- GitHub Actions for CI/CD

A concrete reference architecture is called out:

- [AKS platform engineering lab for CAPZ and ASO](https://azure-samples.github.io/aks-labs/docs/platform-engineering/aks-capz-aso/)

The hard part isn’t just building it—it’s “day two” operations.

## A story in three acts

The authors frame agentic platform engineering in three stages that also map to how GitHub Copilot is evolving (autocomplete → contextual enforcement → autonomous agents that take meaningful action).

## Act one: the platform grows faster than the team

Problem: **knowledge lives in people, and people don’t scale**.

Symptoms:

- Tribal knowledge scattered across engineers
- Docs in random places and drifting over time
- Platform team becomes a bottleneck for onboarding and “how do I…?” questions

Shift:

- Encode knowledge into the platform and repos so Copilot can act like an always-available experienced colleague.

Examples described:

- Copilot helps explain deployment pipelines and conventions by reading repo context.
- Copilot helps assemble infrastructure using a vetted Terraform module catalog.
- For “brownfield” environments (built manually in the portal), an AI assistant can examine resources and generate Terraform or Bicep templates you should have had.

Hands-on resources:

- [Act 1 workshop](https://github.com/microsoftgbb/agentic-platform-engineering/tree/main/Act-1)
- [Starter prompt template](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/Act-1/starter-prompt.md)
- Example IaC-aware agents:
  - [IaC Module Catalog Agent](https://github.com/ricardocovo/iac-module-catalog)
  - [Infrastructure Reverse Engineer Agent](https://github.com/ricardocovo/ghcp-infra-reverse-engineer)

## Act two: standards exist, but they’re not enforced

Problem: standards and compliance rules are easy to forget and hard to apply consistently.

Pattern:

- Every push triggers a GitHub Action.
- That workflow runs GitHub Copilot “in the background” with a standardized prompt.
- Checks can include:
  - Did documentation get updated?
  - Were unit tests generated?
  - Does IaC comply with security policies?

Why this differs from static lint rules:

- The “guardrails” can be updated by changing instructions in markdown (instead of changing pipeline code).
- New requirements can be added without rewriting CI/CD logic.

The post also calls out **Microsoft Foundry** as a way to host custom models and connect to organizational data sources (SharePoint, databases, external compliance providers). Copilot can pull that information in via MCP servers so the rules can live outside the repo—so long as they’re reachable.

Hands-on resources:

- [Act 2 workshop](https://github.com/microsoftgbb/agentic-platform-engineering/tree/main/Act-2)

Repo implementation details called out:

- Reusable team prompts stored as `.prompt.md` under `.github/prompts/`
- Examples for AKS ops:
  - [aks-check-pods.prompt.md](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/.github/prompts/aks-check-pods.prompt.md)
  - [aks-check-nodes.prompt.md](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/.github/prompts/aks-check-nodes.prompt.md)
  - [aks-remediation.prompt.md](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/.github/prompts/aks-remediation.prompt.md)
- Wiring into CI/CD with a docs workflow:
  - [copilot.generate-docs.yml](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/.github/workflows/copilot.generate-docs.yml)

## Act three: Kubernetes operations don’t scale linearly

Problem: day-two operations create constant firefighting, and diagnosis expertise doesn’t scale. Runbooks are static and don’t cover every scenario.

The proposed solution is an ops agent that can **observe, diagnose, and propose remediation**.

### The “Cluster Doctor” agent

A custom GitHub Copilot agent configured with SRE-style diagnostic knowledge.

- Workshop: [Act 3 workshop](https://github.com/microsoftgbb/agentic-platform-engineering/tree/main/Act-3)
- Agent definition file:
  - [cluster-doctor.agent.md](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/.github/agents/cluster-doctor.agent.md)

The agent definition encodes:

- Persona: senior Kubernetes administrator / SRE
- Workflow: collect → verify → diagnose → triage → remediate
- Safety constraints:
  - Never attempt destructive changes without authorization
  - Verify cluster identity before any write action

### Crawl / walk / run adoption model

**Crawl** (capture the expertise):

- Engineers write down diagnostic steps and `kubectl` commands.
- Store as repo-based markdown: agent definitions, instructions, prompts.

**Walk** (wire into operations):

- Argo CD monitors app health.
- When a deployment degrades, Argo triggers a webhook to GitHub Actions.
- GitHub Actions creates a GitHub issue with context (cluster name, resource group, telemetry).
- A human applies a label like `cluster-doctor`.
- The agent runs:
  - Authenticates to Azure via Workload Identity Federation
  - Runs `kubectl` against the cluster
  - Queries the [AKS MCP server](https://github.com/Azure/aks-mcp) for deeper telemetry
  - Uses eBPF tooling via [Inspektor Gadget](https://inspektor-gadget.io/) for harder diagnostics (e.g., latency / CoreDNS)
- The agent opens a PR with the proposed fix.

**Run** (reduce manual triggering):

- Apply the label automatically when issues are created.
- Agent starts immediately, performs investigation, and produces a remediation PR plus root-cause summary.
- Human reviews and approves.

### The wiring: Argo CD → GitHub → GitHub Actions → agent

The event pipeline:

- Argo CD detects failure/unhealthy deployment.
- Argo CD Notifications shapes a custom message/payload.
- Sends to GitHub using `repository_dispatch`.
- A workflow creates/updates a GitHub issue with labels, troubleshooting context, and metadata.

Two workflows are highlighted:

1. **[argocd-deployment-failure.yml](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/.github/workflows/argocd-deployment-failure.yml)**
   - Receives `repository_dispatch`
   - Parses payload
   - Creates a structured GitHub issue
   - Deduplicates by app (comment instead of new issue)
2. **[copilot.trigger-cluster-doctor.yml](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/.github/workflows/copilot.trigger-cluster-doctor.yml)**
   - Runs when the `cluster-doctor` label is applied
   - Checks out repo
   - Installs GitHub Copilot CLI
   - Authenticates to Azure via Workload Identity Federation
   - Invokes the Cluster Doctor agent pointing at the issue

Argo CD notification setup guide:

- [ArgoCD GitHub Issue Creation Setup Guide](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/Act-3/SETUP.md)

### Setting up permissions and tokens

Repo setup requirements described:

- Configure a GitHub environment named **`copilot`**
- Add secrets from the AKS setup:
  - `ARM_CLIENT_ID`
  - `ARM_SUBSCRIPTION_ID`
  - `ARM_TENANT_ID`
- Configure a **PAT** token for actions against the repo

The post references an AKS setup guide:

- [Deploy MCP Server on AKS with Workload Identity](https://blog.aks.azure.com/2025/10/22/deploy-mcp-server-aks-workload-identity)

### Using MCP with GitHub Copilot

The repo uses an MCP config file to let Copilot reach:

- A GitHub MCP server (read issues, create PRs)
- An AKS MCP server inside the cluster (run `kubectl` and fetch deeper diagnostics)

Link:

- [MCP configuration file](https://github.com/microsoftgbb/agentic-platform-engineering/blob/main/.copilot/mcp-config.json)

The cluster embeds metadata (resource group, cluster name, region) into an Argo CD config map so the agent can route itself to the right target when it picks up an issue.

## From reactive to adaptive

The shift isn’t “automation vs no automation”—it’s **static/brittle processes** vs **adaptive, reasoning-capable systems**.

| Area | Before | After |
| --- | --- | --- |
| Knowledge | Tribal, in people’s heads | Encoded in repos, accessible via conversation |
| Standards | Manual enforcement | Automatically applied on every push |
| Incident response | Reactive, expert-dependent | Agent-initiated, human-approved |
| Runbooks | Static docs | Context-adapting agents |
| Onboarding | Weeks | Ask the platform |

## What this means for your platform

A suggested adoption path:

1. **Start with awareness**: give GitHub Copilot access to repos, service catalogs, infra definitions.
2. **Add enforcement**: run Copilot-powered checks on pushes (documentation generation is suggested as a low-risk starting point).
3. **Enable agent operations**: connect Argo CD (or another monitoring tool) to create GitHub issues on failures; build an agent that can authenticate, diagnose, and propose fixes with humans approving.

Resources mentioned:

- Repo: https://github.com/microsoftgbb/agentic-platform-engineering
- YouTube: [Platform Engineering: Creating Scalable and Resilient Systems | BRK188](https://www.youtube.com/watch?v=mGq442iwAF0)
- Related blog post: [Platform Engineering for the Agentic AI era | All things Azure](https://devblogs.microsoft.com/all-things-azure/platform-engineering-for-the-agentic-ai-era/)


[Read the entire article](https://devblogs.microsoft.com/all-things-azure/agentic-platform-engineering-with-github-copilot/)

