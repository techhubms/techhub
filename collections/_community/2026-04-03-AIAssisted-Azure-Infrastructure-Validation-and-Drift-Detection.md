---
primary_section: github-copilot
author: ShivaniThadiyan
section_names:
- ai
- azure
- devops
- github-copilot
- security
title: AI‑Assisted Azure Infrastructure Validation and Drift Detection
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-assisted-azure-infrastructure-validation-and-drift-detection/ba-p/4508346
feed_name: Microsoft Tech Community
date: 2026-04-03 08:48:00 +00:00
tags:
- AI
- AI Assisted Review
- Azure
- Azure Policy
- Azure Resource Graph
- Change Analysis
- Checkov
- Community
- Detailed Exitcode
- DevOps
- Drift Detection
- GitHub Copilot
- Governance
- IaC
- IaC Drift
- IaC Security Scanning
- KQL
- Policy Compliance
- Pull Requests
- RBAC
- Security
- Shift Left
- Tag Governance
- Terraform
- Terraform Plan
- Terraform Plan  Refresh Only
- TFLint
- Tfsec
---

ShivaniThadiyan outlines a shift-left approach to Azure infrastructure validation, using GitHub Copilot as an assistive layer to summarize Terraform plans, interpret drift signals, and help prioritize Azure Policy and Azure Resource Graph findings—without removing human approvals or governance.<!--excerpt_end-->

## Overview

Infrastructure as Code (IaC) improves consistency and repeatability for Azure platforms, but **infrastructure drift** still happens (portal edits, emergency fixes, policy exemptions, and environment-specific overrides). When drift goes unnoticed, teams lose confidence in automation and can end up with outages or security gaps.

This guide explains an approach that combines:

- **Shift-left Terraform practices** (catch issues before deployment)
- **Azure-native signals** (Azure Policy + Azure Resource Graph)
- **AI assistance** (summarization and prioritization—*not* autonomous enforcement)

## Why traditional drift detection isn’t enough

Teams often already use:

- Terraform plan reviews
- Azure Policy compliance dashboards
- Azure Resource Graph queries
- Manual scripts and audits

The issue is less about missing data and more about **interpretation at scale**:

- Outputs are verbose/noisy
- Findings are spread across tools
- Prioritization is hard
- Human context is required

## Where AI fits (and where it does not)

AI should **not**:

- Auto-approve infrastructure changes
- Apply remediation directly
- Replace Terraform, Policy, or RBAC

AI **should**:

- Summarize large outputs
- Highlight risky/unexpected changes
- Detect drift patterns
- Assist human decision-making

Goal: **decision support**, not autonomous enforcement.

## Shift-left Terraform lifecycle (step by step)

End-to-end flow:

- Code Commit
- Local Validation
- Static Analysis & Security
- Terraform Plan Review
- Drift Gate
- Approval
- Apply

## Step 1: Local Terraform validation

Run basic validation on the developer workstation:

```bash
terraform init
terraform validate
```

## Step 2: PR-level static validation

Run automated checks on pull requests:

- `terraform fmt`
- Linting (TFLint)
- IaC security scanning (tfsec, Checkov, etc.)

## Step 3: Generate a deterministic Terraform plan

Separate planning from execution:

```bash
terraform plan -out=tfplan
```

This provides visibility with **zero impact** to Azure.

## Step 4: AI-assisted Terraform plan review

Terraform plans can be accurate but tedious to review at scale. GitHub Copilot can help summarize impact.

Example prompt:

```text
Summarize this Terraform plan:
1) Security, network, or identity-impacting changes
2) Potential downtime risks
3) Unexpected changes outside standard modules
Provide a concise approval-ready summary.
```

## Step 5: Drift-only detection gate (critical control)

Before applying changes, validate that Terraform state still matches Azure:

```bash
terraform plan -refresh-only -detailed-exitcode
```

Exit codes:

- `0` → No drift
- `2` → Drift detected
- `1` → Error

This is intended to catch drift sources like:

- Manual portal edits
- Emergency fixes not back-ported to IaC
- External automation interference

## Step 6: Human approval (governance intact)

Approvals still validate:

- Terraform plan
- Drift results
- AI summaries
- Policy implications

## Step 7: Apply exactly what was reviewed

Apply the saved plan (no re-calculation):

```bash
terraform apply tfplan
```

## Azure Resource Graph: drift at scale (actual state)

Terraform shows **intended** state; Azure Resource Graph can show **actual** state across the environment.

### Who changed what? (change analysis)

```kusto
resourcechanges
| extend changeTime = todatetime(properties.changeAttributes.timestamp)
| extend targetResourceId = tostring(properties.targetResourceId)
| extend changeType = tostring(properties.changeType)
| extend changedBy = tostring(properties.changeAttributes.changedBy)
| extend clientType = tostring(properties.changeAttributes.clientType)
| extend operation = tostring(properties.changeAttributes.operation)
| where changeTime > ago(7d)
| project changeTime, targetResourceId, changeType, changedBy, clientType, operation
| sort by changeTime desc
```

Use this to understand:

- Portal vs automation changes
- Actor identity
- Operation type

Then use AI to flag suspicious patterns instead of manually scanning results.

### Detecting tag drift

```kusto
ResourceContainers
| where type =~ 'microsoft.resources/subscriptions/resourcegroups'
| where isnull(tags['Owner']) or isempty(tostring(tags['Owner']))
| project subscriptionId, resourceGroup=name, location, tags
```

Missing tags can be an early signal of governance erosion.

## Azure Policy: from compliance to action

Azure Policy can show what’s non-compliant, but not what to fix first:

```kusto
PolicyResources
| where type =~ 'Microsoft.PolicyInsights/PolicyStates'
| extend complianceState = tostring(properties.complianceState)
| extend policyAssignmentName = tostring(properties.policyAssignmentName)
| summarize count() by policyAssignmentName, complianceState
```

AI can help by grouping violations, ranking risk, and suggesting remediation paths.

## Reusable prompt library (infra-focused)

Standardizing prompts helps avoid ad-hoc prompting and improves consistency.

### Terraform plan review

```text
Summarize this Terraform plan:
- High-risk changes
- Downtime risks
- Unexpected modifications
```

### Drift interpretation

```text
Analyze this terraform plan -refresh-only output.
Explain drift cause and recommend revert, backport, or accept.
```

### Resource Graph drift triage

```text
Group these Azure resource changes by actor and clientType.
Highlight suspicious patterns and suggest guardrails.
```

### Policy compliance prioritization

```text
Group policy violations by root cause.
Rank by risk and suggest remediation approaches.
```

## Key takeaways

- Drift is inevitable; unmanaged drift is optional
- Shift-left Terraform reduces risk before Azure is touched
- AI is for analysis and decision support, not enforcement
- Terraform + KQL + Azure Policy + AI are stronger together
- Governance can become clearer without slowing delivery


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-assisted-azure-infrastructure-validation-and-drift-detection/ba-p/4508346)

