---
tags:
- Az PowerShell
- Azure
- Azure Arc
- Azure Extension For SQL Server Deployment
- Azure Governance
- Azure Monitor
- Azure Policy
- Azure Resource Graph
- Azure Subscription Scope
- Community
- Compliance
- ConsentToRecurringPAYG
- DeployIfNotExists
- DevOps
- KQL
- LicenseType
- Managed Identity
- Management Groups
- Microsoft Defender For Cloud
- PAYG
- Policy Remediation
- PowerShell
- RBAC
- Reader Role
- Recurring Billing Consent
- Resource Policy Contributor
- Software Assurance
- SQL Server Enabled By Azure Arc
- System Assigned Managed Identity
primary_section: azure
date: 2026-04-13 06:53:27 +00:00
author: TomClaes
external_url: https://techcommunity.microsoft.com/t5/azure-arc-blog/automating-arc-enabled-sql-server-license-type-configuration/ba-p/4500326
section_names:
- azure
- devops
title: Automating Arc-enabled SQL Server license type configuration with Azure Policy
feed_name: Microsoft Tech Community
---

TomClaes shows how to use Azure Policy to enforce the correct LicenseType on SQL Server enabled by Azure Arc, including PowerShell-based deployment, remediation runs, and the RBAC permissions needed to keep Arc SQL inventory compliant at scale.<!--excerpt_end-->

## Overview

Azure Arc lets you onboard SQL Server instances (Linux or Windows) into Azure—whether they run on-premises, in other clouds, or at the edge—so you can manage them from the Azure portal using services like Azure Monitor, Azure Policy, and Microsoft Defender for Cloud.

A key onboarding step is setting the SQL Server extension **LicenseType** correctly so it matches your Microsoft licensing agreement:

- **Paid**: you have a SQL Server license with Software Assurance or a SQL Server subscription
- **PAYG**: pay-as-you-go billing for SQL Server software via your Azure subscription
- **LicenseOnly**: you have a perpetual SQL Server license

Why it matters:

- **Benefits unlock**: Paid/PAYG can unlock Azure benefits at no extra cost (e.g., Azure Update Manager, Machine Configuration) and Arc SQL capabilities like Best Practices Assessment and Remote Support.
- **Billing correctness**: PAYG enables SQL Server software billing through your Azure subscription when you don’t have Software Assurance.

## Configure license types at scale using Azure Policy

You can set license type manually in the Azure portal, but at scale you’ll want automation. PowerShell is one option (Microsoft Learn explains it), but this article focuses on **Azure Policy** for enforcement and remediation.

Reference materials:

- Microsoft Learn (PowerShell approach): [Configure SQL Server - SQL Server enabled by Azure Arc | Microsoft Learn](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-configuration?view=sql-server-ver17&tabs=azure%2Cazure-portal#modify-sql-server-configuration)
- Azure Policy tutorial: [Tutorial: Build policies to enforce compliance - Azure Policy | Microsoft Learn](https://learn.microsoft.com/en-us/azure/governance/policy/tutorials/create-and-manage#implement-a-new-custom-policy)
- Inspiration for Windows Server licensing: [Automating Windows Server Licensing Benefits with Azure Arc Policy](https://techcommunity.microsoft.com/blog/coreinfrastructureandsecurityblog/automating-windows-server-licensing-benefits-with-azure-arc-policy/4469345)

Source code:

- Microsoft samples: [microsoft/sql-server-samples/.../arc-sql-license-type-compliance](https://github.com/microsoft/sql-server-samples/tree/master/samples/manage/azure-arc-enabled-sql-server/compliance/arc-sql-license-type-compliance)
- Personal repo: [claestom/sql-arc-policy-license-config](https://github.com/claestom/sql-arc-policy-license-config)

### Deployment flow

Deployment has two steps:

1. **Create/update** the Azure Policy definition and assignment
2. Start a **remediation task** so existing Arc-enabled SQL Server extensions get brought into compliance

This article uses **PowerShell** to deploy.

## Deploy the policy (PowerShell)

### Download files

```powershell
# Optional: create and enter a local working directory
mkdir sql-arc-lt-compliance
cd sql-arc-lt-compliance

$baseUrl = "https://raw.githubusercontent.com/microsoft/sql-server-samples/master/samples/manage/azure-arc-enabled-sql-server/compliance/arc-sql-license-type-compliance"

New-Item -ItemType Directory -Path policy, scripts -Force | Out-Null

curl -sLo policy/azurepolicy.json "$baseUrl/policy/azurepolicy.json"
curl -sLo scripts/deployment.ps1 "$baseUrl/scripts/deployment.ps1"
curl -sLo scripts/start-remediation.ps1 "$baseUrl/scripts/start-remediation.ps1"
```

Note on Windows PowerShell 5.1:

- `curl` is an alias for `Invoke-WebRequest`
- Use `curl.exe` instead, or run these commands in PowerShell 7+

### Authenticate

```powershell
Connect-AzAccount
```

### Set variables

Only `TargetLicenseType` is required; the rest are optional.

```powershell
# ── Required ──
$TargetLicenseType = "PAYG" # "Paid" or "PAYG"

# ── Optional (uncomment to override defaults) ──
# $ManagementGroupId = "<management-group-id>" # Default: tenant root management group
# $SubscriptionId = "<subscription-id>" # Default: policy assigned at management group scope
# $ExtensionType = "Both" # "Windows", "Linux", or "Both" (default)
# $LicenseTypesToOverwrite = @("Unspecified","Paid","PAYG","LicenseOnly") # Default: all
```

### Run the deployment script

```powershell
# Minimal — uses defaults for management group, platform, and overwrite targets
.\scripts\deployment.ps1 -TargetLicenseType $TargetLicenseType

# With subscription scope
.\scripts\deployment.ps1 -TargetLicenseType $TargetLicenseType -SubscriptionId $SubscriptionId

# With all options
.\scripts\deployment.ps1 `
  -ManagementGroupId $ManagementGroupId `
  -SubscriptionId $SubscriptionId `
  -ExtensionType $ExtensionType `
  -TargetLicenseType $TargetLicenseType `
  -LicenseTypesToOverwrite $LicenseTypesToOverwrite
```

Parameter notes:

- `ManagementGroupId` (optional): management group where the policy definition is created; defaults to tenant root management group
- `ExtensionType` (optional, default `Both`): `Windows`, `Linux`, or `Both`; `Both` creates one definition/assignment for both platforms
- `SubscriptionId` (optional): if provided, assignment scope is subscription; otherwise it assigns at management group scope
- `TargetLicenseType` (required): `Paid` or `PAYG`
- `LicenseTypesToOverwrite` (optional, default all): controls which current states are eligible for update
  - `Unspecified`: no current LicenseType
  - `Paid`, `PAYG`, `LicenseOnly`: explicit current values

The script also:

- Creates a **system-assigned managed identity** on the policy assignment (`-IdentityType SystemAssigned`)
- Assigns required roles automatically
- Uses retry logic (5 attempts, 10-second delay) to reduce `PolicyAuthorizationFailed` errors due to managed identity replication delays

## Remediate existing resources

After deployment, wait a few minutes for Azure Policy to run a compliance scan (Azure Policy → **Compliance**).

More info: [Get policy compliance data - Azure Policy | Microsoft Learn](https://learn.microsoft.com/en-us/azure/governance/policy/how-to/get-compliance-data#portal)

### Set variables

`TargetLicenseType` must match what you used during deployment.

```powershell
# ── Required ──
$TargetLicenseType = "PAYG" # Must match the deployment target

# ── Optional (uncomment to override defaults) ──
# $ManagementGroupId = "<management-group-id>" # Default: tenant root management group
# $SubscriptionId = "<subscription-id>" # Default: remediation runs at management group scope
# $ExtensionType = "Both" # Must match the platform used for deployment
```

### Start remediation

```powershell
# Minimal — uses defaults for management group and platform
.\scripts\start-remediation.ps1 -TargetLicenseType $TargetLicenseType -GrantMissingPermissions

# With subscription scope
.\scripts\start-remediation.ps1 -TargetLicenseType $TargetLicenseType -SubscriptionId $SubscriptionId -GrantMissingPermissions

# With all options
.\scripts\start-remediation.ps1 `
  -ManagementGroupId $ManagementGroupId `
  -ExtensionType $ExtensionType `
  -SubscriptionId $SubscriptionId `
  -TargetLicenseType $TargetLicenseType `
  -GrantMissingPermissions
```

Parameter notes:

- `ManagementGroupId` (optional): defaults to tenant root management group
- `ExtensionType` (optional, default `Both`): must match the platform used for the assignment
- `SubscriptionId` (optional): run remediation at subscription scope
- `TargetLicenseType` (required): must match the assignment target
- `GrantMissingPermissions` (optional switch): checks/assigns missing roles before starting remediation

Track progress in Azure Policy → **Remediation** → **Remediation tasks**.

## Recurring billing consent for PAYG

If `TargetLicenseType` is `PAYG`, the policy also includes `ConsentToRecurringPAYG` in the extension settings:

- `Consented: true`
- a UTC timestamp

Details: [Move SQL Server license agreement to pay-as-you-go subscription](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-pay-as-you-go-transition?view=sql-server-ver17)

Compliance behavior:

- Resources with `LicenseType: PAYG` but missing `ConsentToRecurringPAYG` are marked non-compliant and remediated.
- This covers backward compatibility for older PAYG extensions created before the consent requirement.

Important constraint:

- Once set, `ConsentToRecurringPAYG` **cannot be removed** (enforced by the Azure resource provider).
- If you transition away from PAYG later, the policy changes `LicenseType` but leaves the consent property in place.

## Required RBAC permissions

The policy assignment’s managed identity needs these roles at (or inherited to) the assignment scope:

- **Azure Extension for SQL Server Deployment**: update Arc SQL extension settings (including `LicenseType`)
- **Reader**: read resource/extension state for policy evaluation
- **Resource Policy Contributor**: required for policy-driven template deployments used by **DeployIfNotExists**

## Brownfield and greenfield scenarios

### Brownfield: existing Arc SQL inventory

Azure Policy helps remediate mixed/incorrect/missing LicenseType values across the estate.

Depending on `targetLicenseType` and `licenseTypesToOverwrite`, you can:

- Standardize all resources to one value
- Set LicenseType only when missing
- Migrate a specific subset (e.g., Paid → PAYG)
- Preserve selected states and remediate only what needs attention

Examples:

- **Standardize everything to Paid**
  - `targetLicenseType: Paid`
  - `licenseTypesToOverwrite: ['Unspecified','Paid','PAYG','LicenseOnly']`
  - Result: all in-scope Arc SQL extensions converge to `LicenseType == Paid`

- **Backfill only missing values**
  - `targetLicenseType: Paid`
  - `licenseTypesToOverwrite: ['Unspecified']`
  - Result: only resources with missing LicenseType are updated

- **Migrate only Paid to PAYG**
  - `targetLicenseType: PAYG`
  - `licenseTypesToOverwrite: ['Paid']`
  - Result: only Paid instances switch to PAYG; consent is set as required for recurring PAYG billing

- **Protect existing PAYG, fix only missing or LicenseOnly**
  - `targetLicenseType: Paid`
  - `licenseTypesToOverwrite: ['Unspecified','LicenseOnly']`
  - Result: only Unspecified/LicenseOnly become Paid; existing PAYG remains untouched

### Greenfield: newly onboarded SQL Servers

Azure Policy provides ongoing enforcement for new Arc-enabled SQL Server resources as they enter the assignment scope.

#### Azure Policy vs tagging

Microsoft’s automatic deployment of the SQL Server extension for Azure has an option to enforce `LicenseType` via tags:

- [Manage Automatic Connection - SQL Server enabled by Azure Arc | Microsoft Learn](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-autodeploy?view=sql-server-ver17&tabs=azure-portal)

Practical split:

- Tagging ensures **initial compliance** for newly connected Arc-enabled SQL servers
- Azure Policy enforces **ongoing compliance** and guards against drift or ad-hoc additions

## Tools: visibility with KQL + workbook

### KQL query

This Azure Resource Graph query helps list and join discovered SQL Server machines with their Arc SQL extension settings (including `LicenseType`).

```kusto
resources
| where type == "microsoft.hybridcompute/machines"
| where properties.detectedProperties.mssqldiscovered == "true"
| extend machineIdHasSQLServerDiscovered = id
| project name, machineIdHasSQLServerDiscovered, resourceGroup, subscriptionId
| join kind=leftouter (
    resources
    | where type == "microsoft.hybridcompute/machines/extensions"
    | where properties.type in ("WindowsAgent.SqlServer","LinuxAgent.SqlServer")
    | extend machineIdHasSQLServerExtensionInstalled = iff(
        id contains "/extensions/WindowsAgent.SqlServer" or id contains "/extensions/LinuxAgent.SqlServer",
        substring(id, 0, indexof(id, "/extensions/")),
        ""
      )
    | project License_Type = properties.settings.LicenseType, machineIdHasSQLServerExtensionInstalled
) on $left.machineIdHasSQLServerDiscovered == $right.machineIdHasSQLServerExtensionInstalled
| where isnotempty(machineIdHasSQLServerExtensionInstalled)
| project-away machineIdHasSQLServerDiscovered, machineIdHasSQLServerExtensionInstalled
```

Source: [Configure SQL Server - list configuration details for each SQL Server instance](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-configuration?view=sql-server-ver17&tabs=azure%2Cazure-portal#list-configuration-details-for-each-sql-server-instance)

### Azure workbook

- [claestom/azure-arc-sa-workbook](https://github.com/claestom/azure-arc-sa-workbook)

## Resources

- [Configure SQL Server - SQL Server enabled by Azure Arc | Microsoft Learn](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-configuration?view=sql-server-ver17&tabs=azure%2Cazure-portal#modify-sql-server-configuration)
- [Azure Policy documentation | Microsoft Learn](https://learn.microsoft.com/en-us/azure/governance/policy/)
- [Recurring billing consent - SQL Server enabled by Azure Arc](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-pay-as-you-go-transition?view=sql-server-ver17#recurring-billing-consent)
- [microsoft/sql-server-samples/.../arc-sql-license-type-compliance](https://github.com/microsoft/sql-server-samples/tree/master/samples/manage/azure-arc-enabled-sql-server/compliance/arc-sql-license-type-compliance)
- [claestom/sql-arc-policy-license-config](https://github.com/claestom/sql-arc-policy-license-config)

Updated Apr 12, 2026 (Version 1.0)

[Read the entire article](https://techcommunity.microsoft.com/t5/azure-arc-blog/automating-arc-enabled-sql-server-license-type-configuration/ba-p/4500326)

