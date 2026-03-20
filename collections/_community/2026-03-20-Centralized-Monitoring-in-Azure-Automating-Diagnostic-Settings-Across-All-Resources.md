---
date: 2026-03-20 06:17:50 +00:00
tags:
- Automation
- Azure
- Azure CLI
- Azure Monitor
- Azure Policy
- Bulk Remediation
- Centralized Monitoring
- Community
- DevOps
- Diagnostic Settings
- Log Analytics Workspace
- Logs
- Metrics
- Monitoring Contributor
- Multi Subscription
- Observability
- PowerShell
- Workspace Consolidation
primary_section: azure
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/centralized-monitoring-in-azure/ba-p/4504027
section_names:
- azure
- devops
feed_name: Microsoft Tech Community
author: Shikhaghildiyal
title: 'Centralized Monitoring in Azure: Automating Diagnostic Settings Across All Resources'
---

Shikhaghildiyal walks through an automation approach to standardize Azure Monitor diagnostic settings across many Azure resources, deleting existing configurations and recreating them to route logs and metrics into a centralized Log Analytics Workspace for more consistent observability.<!--excerpt_end-->

# Centralized Monitoring in Azure: Automating Diagnostic Settings Across All Resources

As Azure environments scale, organizations often manage hundreds of resources across multiple subscriptions, making consistent monitoring a challenge.

A key enabler for observability is **diagnostic settings**, which route logs and metrics to destinations like **Log Analytics Workspaces**. In practice, customers frequently encounter:

- Inconsistent diagnostic configurations across resources
- Logs being sent to multiple or outdated workspaces
- A need to centralize monitoring into a single **Log Analytics Workspace (LAW)**
- Difficulty updating existing configurations at scale

Azure does not provide a native way to bulk update diagnostic settings, and it does not support direct migration of historical logs between workspaces.

This post describes a practical automation approach using **PowerShell** and **Azure CLI** to standardize diagnostic settings, redirect logs to a centralized workspace, and improve monitoring visibility.

## Introduction

Observability is foundational in modern cloud operations. As Azure footprints grow into hundreds or thousands of resources, ensuring consistent diagnostic configuration becomes a major operational challenge.

## Problem statement

### Challenges customers face

A common gap in real customer environments:

- Lack of standardized diagnostic settings across resources, especially when customers want to migrate all logging to a new centralized Log Analytics Workspace (LAW) and update existing diagnostic configurations accordingly.

Typical situation:

- Diagnostic settings already exist across many resources
- Teams want to **redirect all logs to a new centralized LAW** (consolidation, cost optimization, or security requirements)
- Updating existing diagnostic settings at scale is difficult

Why it’s hard:

- Azure does **not** offer a native bulk update mechanism for diagnostic settings
- Existing configurations keep pointing at **old workspaces**
- Monitoring becomes fragmented and inconsistent

## Real-world customer scenario

A large enterprise managing **1000+ Azure resources across multiple subscriptions**:

- Multiple Log Analytics Workspaces created over time
- Logs distributed across workspaces, making troubleshooting difficult
- Security team mandated a move to a centralized monitoring workspace

### The problem

- No native way to migrate diagnostic settings in bulk
- Manual reconfiguration would take weeks
- Risk of missing logs during transition

### The need

They needed a solution to:

- Remove existing diagnostic settings
- Reconfigure diagnostic settings across all resources
- Ensure **zero monitoring gaps** (for new data going forward)

## Solution overview

The script automates:

- Discovery of all Azure resources
- Removal of existing diagnostic settings
- Application of a standardized diagnostic setting
- Routing logs and metrics to a **central Log Analytics Workspace**

## How the script works

### Step 1: Define inputs

- New workspace resource ID
- Diagnostic setting name
- Logs and metrics configuration JSON

### Step 2: Fetch all resources

Uses Azure CLI to list all resources:

```powershell
$Resources = az resource list -o json | ConvertFrom-Json
```

### Step 3: Process each resource

For each resource:

- Delete existing diagnostic settings
- Create a new diagnostic setting pointing to the central LAW
- Skip gracefully when diagnostic settings are unsupported for that resource type

## Flow: LAW migration scenario

Before:

- Resource A → LAW-1
- Resource B → LAW-2
- Resource C → LAW-3

After (post script execution):

- All resources → Central LAW

Result:

- Unified logging
- Centralized monitoring
- Simplified operations

## Benefits for customers

- **Centralized monitoring**: single workspace for logs and metrics
- **Faster incident response**: telemetry available in one place
- **Consistency at scale**: standardized configuration across resources
- **Migration enablement**: bulk redirection to a new LAW
- **Reduced operational effort**: no manual per-resource configuration

## Script vs Azure Policy

### Azure Policy limitations

- Primarily effective for **new resources**
- Cannot easily update existing diagnostic settings
- May create duplicate configurations
- Requires remediation cycles

### Why the script is more effective for migration

- Bulk update of existing resources
- Immediate execution
- Cleans up old configurations
- Ensures consistent redirection to the new LAW

### Best practice

- Use this script for **one-time migration/remediation**
- Use Azure Policy for **ongoing governance**

## Key limitations

### No direct migration between workspaces

Azure does not support direct migration of historical logs from one Log Analytics Workspace to another:

- Historical data cannot be moved
- Only new logs can be redirected

Reference: Log Analytics workspace overview - Azure Monitor | Microsoft Learn

https://learn.microsoft.com/en-us/azure/azure-monitor/logs/log-analytics-workspace-overview

### Diagnostic settings are resource-level

- Must be configured per resource
- No native bulk update feature

Reference: Diagnostic settings in Azure Monitor | Microsoft Learn

https://learn.microsoft.com/en-us/azure/azure-monitor/platform/diagnostic-settings?tabs=portal

### Not all resources supported

Some resource types do not support diagnostic settings and will be skipped by the script.

### Azure limits

- Max **5 diagnostic settings per resource**

### Eventual consistency

Deletion and creation may not be instantaneous.

## Permissions required

- Monitoring Contributor
- Or Contributor / Owner

## Full script

```powershell
# ================================
# INPUT VARIABLES (YOUR VALUES)
# ================================

$NEW_LAW_ID = "/subscriptions/91dac1c9-8eb0-4ed4-bf61-16be033f41a2/resourceGroups/<rgname>/providers/Microsoft.OperationalInsights/workspaces/log-central-prod"
$DiagName = "send-to-central-law"

# Correct JSON (DO NOT CHANGE)
$LogsJson = '[{"categoryGroup":"allLogs","enabled":true}]'
$MetricsJson = '[{"category":"AllMetrics","enabled":true}]'

# ================================
# GET ALL RESOURCES
# ================================

Write-Host "Fetching all Azure resources..."

$Resources = az resource list -o json | ConvertFrom-Json

Write-Host "Total resources found: $($Resources.Count)"
Write-Host "------------------------------------------"

# ================================
# LOOP THROUGH EACH RESOURCE
# ================================

foreach ($res in $Resources) {

    $RES_ID = $res.id
    $RES_NAME = $res.name
    $RES_TYPE = $res.type

    Write-Host ""
    Write-Host "Processing: $RES_NAME ($RES_TYPE)"

    # =========================================
    # STEP 1: DELETE EXISTING DIAGNOSTIC SETTINGS
    # =========================================

    try {
        $ExistingDiags = az monitor diagnostic-settings list --resource $RES_ID -o json 2>$null | ConvertFrom-Json

        if ($ExistingDiags -and $ExistingDiags.value -and $ExistingDiags.value.Count -gt 0) {
            foreach ($diag in $ExistingDiags.value) {
                Write-Host " Removing: $($diag.name)"

                az monitor diagnostic-settings delete `
                    --name $diag.name `
                    --resource $RES_ID `
                    --output none 2>$null
            }
        }
    } catch {
        Write-Host " ⚠️ Skip delete (not supported)"
    }

    # =========================================
    # STEP 2: CREATE NEW DIAGNOSTIC SETTING
    # =========================================

    try {
        az monitor diagnostic-settings create `
            --name $DiagName `
            --resource $RES_ID `
            --workspace $NEW_LAW_ID `
            --logs $LogsJson `
            --metrics $MetricsJson `
            --output none 2>$null

        if ($LASTEXITCODE -eq 0) {
            Write-Host " ✅ Applied"
        } else {
            Write-Host " ❌ Failed"
        }
    } catch {
        Write-Host " ⚠️ Not supported / skipped"
    }
}

Write-Host ""
Write-Host "🎉 Completed applying diagnostic settings to all resources!"
```

## Conclusion

Centralized monitoring is a critical pillar of cloud operations, but Azure has gaps around bulk updating diagnostic settings for existing resources.

This automation approach helps by enabling:

- Rapid migration to centralized monitoring
- Consistent configuration across environments
- Improved operational visibility

Combined with Azure Policy, it can support both one-time remediation and ongoing governance.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/centralized-monitoring-in-azure/ba-p/4504027)

