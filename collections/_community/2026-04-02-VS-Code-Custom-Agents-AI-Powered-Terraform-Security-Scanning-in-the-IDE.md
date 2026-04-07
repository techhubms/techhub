---
section_names:
- ai
- azure
- devops
- github-copilot
- security
author: SundarBalajiA
feed_name: Microsoft Tech Community
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/vs-code-custom-agents-ai-powered-terraform-security-scanning-in/ba-p/4507903
title: 'VS Code Custom Agents: AI-Powered Terraform Security Scanning in the IDE'
date: 2026-04-02 09:30:08 +00:00
tags:
- .github/agents
- Agent Mode
- AI
- AKS Security
- Azure
- Azure Infrastructure
- Azure Key Vault
- Azure Monitor
- Azure RBAC
- Azure Security Benchmark V3
- CIS Azure Foundations Benchmark
- Community
- Compliance
- Copilot Chat
- DevOps
- Diagnostic Settings
- GitHub Copilot
- IaC
- IaC Security
- Least Privilege
- Network Security
- NIST 800 53
- PowerShell
- Private Endpoints
- PSScriptAnalyzer
- SARIF
- Security
- Terraform
- Terraform Fmt
- TFLint
- VS Code
- VS Code Copilot Chat
- VS Code Custom Agents
primary_section: github-copilot
---

SundarBalajiA explains how to embed GitHub Copilot custom agents in a repo (via `.github/agents/`) to run Terraform-based Azure infrastructure security checks inside VS Code, including recommended agent metadata, tool permissions, and a structured finding format mapped to CIS, Azure Security Benchmark, and NIST controls.<!--excerpt_end-->

# VS Code Custom Agents: AI-Powered Terraform Security Scanning in the IDE

GitHub Copilot is a strong coding assistant, but it doesn’t automatically understand your repo’s conventions, security requirements, or operational processes. VS Code **custom agents** address that by letting you define specialized assistants that live in your repository and behave consistently for every developer.

This post focuses on **Azure infrastructure teams using Terraform**, and walks through building an **AI-powered security scanner** agent for Terraform IaC modules.

## What are VS Code custom agents?

Starting with **VS Code 1.99+**, GitHub Copilot supports **custom agents** defined as markdown files in your repository:

- Location: `.github/agents/`
- Each agent file defines:
  - **Name and description** (what it is, when to invoke)
  - **Model selection**
  - **Tool permissions** (read/search/execute/edit)
  - **Instructions** (a system-prompt style policy that shapes responses)

When you open a workspace containing these files, the agents appear in the Copilot Chat agent picker. You can invoke them by selecting the agent or typing `@AgentName`.

## How custom agents differ from regular Copilot chat

| Aspect | Regular Copilot Chat | Custom Agent |
| --- | --- | --- |
| Knowledge scope | General programming knowledge | Domain-specific expertise you define |
| Consistency | Varies by prompt phrasing | Consistent behavior across all users |
| Tool access | Context-dependent | Explicitly defined per agent |
| Invocation | Open chat | Named agent with focused scope |
| Portability | Per-user | Shared via repository |
| Constraints | None by default | You define guardrails (e.g., no file edits) |

The goal is repeatable “expert judgment” inside the IDE (for example, consistent security findings) rather than ad-hoc Q&A.

## Anatomy of a custom agent file

A custom agent markdown file has two sections:

### Part 1: YAML frontmatter (metadata)

Frontmatter includes fields like:

- `name`
- `description`
- `model`
- `tools`
- `argument-hint`

### Part 2: Markdown body (instructions)

Everything after the frontmatter is treated like a **system prompt** that defines:

- The agent’s role and expertise
- What it should and should not do
- Output structure
- Domain-specific rules

Because this lives in the repo, it’s version-controlled and shared across the team.

## Frontmatter fields (practical notes)

### Name
- Identifier shown in agent picker and `@mentions`
- Use a clear name without spaces

### Description
- Copilot uses it to decide when to suggest the agent
- Include keywords users will actually type (for example: “security”, “scan”, “review”, “harden”, “validate”)

### Model
Different models have different strengths:

| Model | Best For |
| --- | --- |
| Claude Sonnet 4.5 | Code analysis, security review, structured output |
| GPT-4o | General reasoning, broad knowledge |
| o3-mini | Fast responses, simple tasks |

### Tools
Tool choice is both a capability decision and a security boundary:

| Tool | Capability | Use When |
| --- | --- | --- |
| `read` | Read files in the workspace | Agent needs to analyze code |
| `search` | Search across workspace files | Agent needs to find files by name/content |
| `execute` | Run terminal commands | Agent needs to run scripts/tools |
| `editFiles` | Create/modify files | Agent should write or change code |

Grant only what the agent needs. A read-only reviewer shouldn’t get `editFiles`.

### Argument-hint
Placeholder text shown in chat input to guide users (for example, “scan `resource-groups`” vs “scan all”).

## What can custom agents do?

Common repetitive tasks that fit well:

| Use Case | What the Agent Does |
| --- | --- |
| Code Review | Quality issues, anti-patterns, naming violations with line-level findings |
| Security Scanning | Checks against security baselines (CIS, NIST) with remediation guidance |
| Documentation | Generates API references, runbooks, architecture summaries |
| Onboarding | Answers repo-specific convention/pattern questions |
| Deployment / Ops | Guides deploy/incident steps grounded in repo config |
| Testing | Suggests missing test cases based on changes |
| Release Management | Creates release notes/version decisions from changelogs/git history |

## Prerequisites

| Requirement | Details |
| --- | --- |
| VS Code | 1.99 or later |
| GitHub Copilot | Active subscription (Individual/Business/Enterprise) |
| Copilot Chat extension | Installed and signed in |
| Agent mode enabled | VS Code Settings → search “chat agent” |
| A repository | Agents live in `.github/agents/` (local folder works) |

No extra extensions or infrastructure are required beyond Copilot Chat + a repo.

## Building the IaC security scanner (step-by-step)

### Why do this in the IDE?
Terraform module checks often run only in CI/CD, which creates a slow feedback loop. A custom Copilot agent can surface security feedback while developers write IaC.

Example areas Azure Terraform module teams commonly need to validate:

- Least-privilege **RBAC** (avoid broad Owner/Contributor)
- Network exposure (avoid unrestricted inbound)
- TLS minimums (TLS 1.2+)
- Diagnostic logging for audit trails
- Resource locks for production resources

### Step 1: Create the directory

Create this directory at repo root (if it doesn’t exist):

- `.github/agents/`

### Step 2: Create the agent file

Create a markdown file under `.github/agents/` with frontmatter that looks like this (example from the post):

> **name**: IaCSecurityAgent description: "Scan Terraform and IaC files for security misconfigurations, insecure defaults, and compliance violations. Detects public endpoints, weak IAM, missing encryption, network exposure, and logging gaps. Use when user asks to check security, find misconfigurations, security review, or harden infrastructure"
> **model**: Claude Sonnet 4.5 (copilot)
> **tools**: [read, search, execute]
> **argument-hint**: "Specify directory to scan (e.g., 'resource-groups'), multiple directories (e.g., 'resource-groups, nsg'), or 'all' for entire workspace"

Rationale called out in the post:

- **Why Claude Sonnet 4.5?** Strong code analysis, security context reasoning, consistent structured output.
- **Why `execute`?** The agent saves reports by calling a helper **PowerShell** script.
- **Why not `editFiles`?** Keep the agent advisory unless explicitly asked to make changes.

### Step 3: Open VS Code and test

1. Open Copilot Chat (`Ctrl+Alt+I`)
2. Click the agent picker
3. Select your new agent
4. Try a prompt like: `scan resource-groups`

### Step 4: Iterate like code

Agent instructions are just text in the repo:

- Anyone can update them
- Commit changes
- Behavior updates for everyone

Treat agent instructions like code: review, version, and improve over time.

## What the agent checks (security domains)

The example agent is designed to scan Terraform resources across these areas:

### 1) Identity and Access Management (IAM)
- Overly permissive RBAC roles (Owner/Contributor at broad scope)
- Missing managed identity configuration (using keys instead)
- Hardcoded credentials or secrets
- Missing validation on role assignment variables

### 2) Network Security
- Public endpoints on databases, storage, Key Vaults
- Admin ports (22, 3389) open to `0.0.0.0/0`
- Missing private endpoints for PaaS services
- NSG rules allowing wildcard source addresses

### 3) Data Protection and Encryption
- Encryption at rest disabled
- TLS below 1.2
- HTTPS not enforced
- Secrets stored in plain text in variables

### 4) Logging and Monitoring
- Missing `azurerm_monitor_diagnostic_setting` resources
- Log retention below 90 days
- No audit logging on Key Vault, SQL, or AKS

### 5) Container and Workload Security
- AKS without RBAC enabled
- Local accounts not disabled
- Missing network policy configuration

### 6) Backup and Disaster Recovery
- Key Vault without purge protection
- Missing soft delete configuration
- No geo-redundancy for critical data

## Compliance framework alignment

Findings are mapped to Azure-relevant controls:

- CIS Azure Foundations Benchmark (example: CIS 3.7, CIS 6.1)
- Azure Security Benchmark v3 (example: NS-1, PA-7, DP-4)
- NIST 800-53 (example: SC-7, AC-6)

## Choosing scanning scope

The agent supports:

- Single folder scans
- Multiple folders
- Whole workspace scans (`scan all`)

For “scan all”, it searches for `.tf` files, groups by directory, and scans each group independently.

## Structured finding output

Findings follow a consistent format. Example from the post:

> ###### [MEDIUM] IAM-002: Missing principal_type default recommendation
> - **File:** user-assigned-identity/variables.tf (Line 45)
> - **Resource:** var.rg_role_assignments.principal_type
> - **Issue:** principal_type is optional with null default. In environments with ABAC policies, role assignments may fail if this is not explicitly set.
> - **Impact:** Role assignments could fail silently or be mis-scoped in ABAC-constrained environments.
> - **Compliance:** Azure Security Benchmark PA-7

## Example results from real scans

The post reports running scans across three Azure Terraform modules:

| Module | CRITICAL | HIGH | MEDIUM | LOW | Key Finding |
| --- | --- | --- | --- | --- | --- |
| resource-groups | 0 | 1 | 3 | 2 | Role assignments allow Owner/Contributor |
| nsg | 0 | 1 | 3 | 2 | Wildcard source addresses and ports not blocked |
| user-assigned-identity | 0 | 1 | 3 | 2 | Managed identity lacks role_assignments field — permissions must be set manually post-creation |

The generated report includes exact file paths, line numbers, and Terraform code fixes.

## Companion quality scanner (deterministic tools)

The workspace also includes a second agent (“Super-Linter Scanner”) that runs static analysis tools and generates SARIF + HTML reports:

| Tool | Version | Purpose |
| --- | --- | --- |
| TFLint | v0.53.0 | Naming conventions, unused declarations, provider pinning |
| terraform fmt | v1.9.8 | Formatting validation |
| yamllint | latest | YAML syntax and style |
| PSScriptAnalyzer | latest | PowerShell best practices |

SARIF output can be viewed inline using the SARIF Viewer extension.

## Why agent-based scanning goes beyond traditional tools

Traditional tools (tfsec, Checkov, tflint) match patterns against known rules. The agent adds reasoning, for example:

- Flagging risky designs even without a “bad value” present (for example, missing validation on a role variable)
- Correlating findings across files
- Mapping findings to compliance controls without maintaining a mapping table
- Producing natural-language explanations (not just rule IDs)

This is positioned as complementary to deterministic tools, not a replacement.

## Key takeaways

- Custom VS Code Copilot agents are markdown files in `.github/agents/` (no extension development or deployment required)
- YAML frontmatter controls model choice, tools, and suggestion behavior
- The markdown body is your system prompt—treat it like code
- Tool permissions are a security decision (least privilege)
- Agents are portable across the team via the repo
- Combining AI reasoning with deterministic tools (TFLint, terraform fmt, etc.) improves coverage

## Useful resources

- VS Code Custom Agents documentation: https://code.visualstudio.com/docs/copilot/chat/chat-agent-mode
- GitHub Copilot documentation: https://docs.github.com/en/copilot
- CIS Azure Foundations Benchmark: https://www.cisecurity.org/benchmark/azure
- Azure Security Benchmark: https://learn.microsoft.com/azure/security/benchmarks/
- NIST 800-53 Rev 5: https://csrc.nist.gov/publications/detail/sp/800-53/rev-5/final
- TFLint documentation: https://github.com/terraform-linters/tflint
- SARIF Viewer for VS Code: https://marketplace.visualstudio.com/items?itemName=MS-SarifVSCode.sarif-viewer


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/vs-code-custom-agents-ai-powered-terraform-security-scanning-in/ba-p/4507903)

