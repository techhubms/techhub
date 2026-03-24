---
feed_name: Microsoft Tech Community
title: Automating Azure OpenAI Cost Control Using Budgets, Action Groups, and Automation Runbooks
primary_section: ai
tags:
- AI
- Alerting
- Auditing
- Automation Runbooks
- Az PowerShell
- Azure
- Azure Action Groups
- Azure Automation
- Azure Budgets
- Azure Cost Management
- Azure OpenAI
- Azure OpenAI Service
- Budget Thresholds
- Community
- Connect AzAccount
- Cost Governance
- Cost Overruns
- DevOps
- DisableLocalAuth
- Managed Identity
- Manual Approval Workflow
- Operational Guardrails
- PowerShell
- Resource Group Scope
- Set AzCognitiveServicesAccount
- Set AzContext
author: trisha1302
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/automating-azure-openai-cost-control-using-budgets-action-groups/ba-p/4505164
section_names:
- ai
- azure
- devops
date: 2026-03-24 13:28:40 +00:00
---

trisha1302 outlines an automation pattern to prevent Azure OpenAI spend overruns by wiring Azure Budgets to Action Groups that trigger Azure Automation runbooks, automatically disabling API access at a budget threshold and requiring a controlled manual runbook to re-enable it.<!--excerpt_end-->

## Overview

As organizations adopt **Azure OpenAI** for generative AI, analytics, and experimentation, costs can scale quickly. This pattern uses native Azure services to enforce budget limits automatically by disabling Azure OpenAI API access when a budget threshold is breached, while keeping a manual, auditable recovery path to re-enable access.

## Solution overview

This pattern combines three Azure capabilities:

- **Azure Budgets**: monitors spend and detects threshold breaches
- **Action Groups**: routes the budget alert to automation and notifications
- **Azure Automation Runbooks**: executes enforcement (disable) and recovery (enable)

When the threshold is reached, an Automation Runbook disables Azure OpenAI API access to stop token consumption and further cost growth. Re-enabling is done via a separate manual runbook after review/approval.

## Key objectives

- Prevent unplanned Azure OpenAI cost overruns
- Enforce usage limits without manual intervention
- Provide a controlled, auditable recovery workflow
- Enable safer experimentation with financial guardrails

## Architecture components

### 1. Azure Budget (resource group scope)

Configure an **Azure Budget** at the **resource group** level that contains the Azure OpenAI resource.

- Tracks actual costs continuously
- Triggers alerts at thresholds (for example: **80%**, **90%**, **100%**)

Why resource group scope?

- Clear ownership and accountability
- Isolated cost boundaries
- Easier governance and rollback

### 2. Action Group integration

Connect the budget alert to an **Azure Action Group**, acting as the bridge between cost detection and enforcement.

Action Group responsibilities:

- Trigger Azure Automation runbooks
- Notify stakeholders via email
- Centralize alert handling

### 3. Azure Automation account and runbooks

An **Azure Automation Account** hosts two PowerShell runbooks:

| Runbook name | Purpose |
| --- | --- |
| `alert-budget-runbook-disable` | Automatically disables Azure OpenAI API access |
| `alert-budget-runbook-enable` | Manually re-enables Azure OpenAI API access |

The disable runbook is invoked by the Action Group automatically. The enable runbook is started manually after review/approval.

## Automated enforcement flow

### Step 1: Budget threshold breach

- Azure Budget monitors cost consumption
- When the defined threshold is crossed, a budget alert fires

### Step 2: Action Group triggers automation

- The Action Group invokes the disable runbook
- No manual intervention is required
- Execution is logged for audit purposes

Example disable runbook logic:

```powershell
Connect-AzAccount -Identity
Set-AzContext -subscription xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx

Set-AzCognitiveServicesAccount -ResourceGroupName "test-rg" -Name "name of openai api resource" -DisableLocalAuth $true
Set-AzCognitiveServicesAccount -ResourceGroupName "test-rg" -Name "name of openai api resource" -DisableLocalAuth $true
```

### Step 3: Azure OpenAI API is disabled

- The runbook disables Azure OpenAI API access
- API calls/token consumption stop
- Further cost accumulation is prevented

Verification:

- In the Azure portal: **Azure OpenAI → Keys and Endpoint**
- You should see an error indicating the service is disabled

## Manual recovery: re-enabling Azure OpenAI

After the breach is reviewed and approved, re-enable API access by manually running `alert-budget-runbook-enable`.

Example enable runbook logic:

```powershell
Connect-AzAccount -Identity
Set-AzContext -subscription xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx

Set-AzCognitiveServicesAccount -ResourceGroupName "test-rg" -Name "name of openai api resource" -DisableLocalAuth $false
Set-AzCognitiveServicesAccount -ResourceGroupName "test-rg" -Name "name of openai api resource" -DisableLocalAuth $false
```

## Enable runbook: user guide

1. Navigate to Runbooks
   - Azure Portal → Automation Account → Runbooks
2. Select the enable runbook
   - Choose `alert-budget-runbook-enable`
3. Start the runbook
   - Click **Start**
   - Optionally review the PowerShell code
4. Monitor execution
   - Track status in the **Jobs** blade
   - Confirm completion and validate API access

This manual recovery step enforces review, approval, and intentional reactivation.

## Budget configuration: step-by-step

1. Navigate to Budgets
   - Azure Portal → Resource Group → Cost Management → Budgets
2. Create a new budget
   - Set a budget name
   - Define the budget amount
   - Keep default time range unless required
3. Configure alert conditions
   - Alert type: **Actual cost**
   - Thresholds: **70%**, **80%**, **90%**, **100%** (as needed)
   - Action Group: link the automation action group
   - Enable email notifications

## Benefits

- **Automated cost protection**: prevents runaway costs without manual monitoring
- **Governance-first design**: detection (Budgets), enforcement (Runbooks), recovery (manual enable)
- **Auditability and control**: actions logged; re-enable requires deliberate approval
- **Scalable and reusable**: extend the pattern to other Azure services/environments

## Best practices and recommendations

- Use early warning thresholds (70–80%) for proactive visibility
- Limit permissions for the enable runbook to approved operators
- Log all runbook executions for auditing
- Periodically review budget values as usage grows
- Combine with Azure Monitor alerts for richer insights

## References

- [Manage runbooks in Azure Automation | Microsoft Learn](https://learn.microsoft.com/en-us/azure/automation/manage-runbooks)
- [Tutorial - Create and manage budgets - Microsoft Cost Management | Microsoft Learn](https://learn.microsoft.com/en-us/azure/cost-management-billing/costs/tutorial-acm-create-budgets?tabs=psbudget)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/automating-azure-openai-cost-control-using-budgets-action-groups/ba-p/4505164)

