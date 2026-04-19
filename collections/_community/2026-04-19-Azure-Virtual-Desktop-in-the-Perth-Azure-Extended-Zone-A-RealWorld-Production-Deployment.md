---
section_names:
- azure
- devops
- security
feed_name: Microsoft Tech Community
author: akashmishra
date: 2026-04-19 10:03:32 +00:00
title: 'Azure Virtual Desktop in the Perth Azure Extended Zone: A Real‑World Production Deployment'
primary_section: azure
tags:
- Active Directory Domain Join
- ARM Templates
- Automation Webhook
- Azure
- Azure Automation
- Azure CLI
- Azure Compute Gallery
- Azure Extended Zones
- Azure Image Builder
- Azure Instance Metadata Service
- Azure REST API
- Azure Virtual Desktop
- Bicep
- Community
- Custom DNS Forwarders
- Desktop Virtualization Power On Contributor
- DevOps
- GitHub Actions
- GPO Logoff Script
- GPU Virtual Machines
- Hub And Spoke Network
- Hybrid Identity
- IaC
- Image Replication
- IMDS
- Key Vault
- Managed Identity
- Microsoft Entra ID
- NVadsA10 V5
- Personal Host Pools
- Perth Extended Zone
- Postman
- Private Endpoints
- Private Link
- RBAC
- Security
- Shared Image Gallery
- Start VM On Connect
- User Assigned Managed Identity
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/azure-virtual-desktop-in-the-perth-azure-extended-zone-a-real/ba-p/4508550
---

akashmishra walks through a production Azure Virtual Desktop deployment in the Perth Azure Extended Zone, covering the practical patterns needed for low-latency GPU workloads: image engineering with Azure Image Builder + Azure Compute Gallery replication, private-only networking, GitHub Actions automation, and user-driven VM deallocation via IMDS and Azure Automation.<!--excerpt_end-->

# Azure Virtual Desktop in the Perth Azure Extended Zone: A Real‑World Production Deployment

Azure Extended Zones bring Azure services closer to users to reduce latency for edge-adjacent workloads. In Azure Virtual Desktop (AVD), only the session host VMs are placed in the Extended Zone, while the AVD control plane/metadata remains in the parent region.

The Perth Extended Zone was announced in Dec 2024, entered Public Preview in mid‑2025, and reached General Availability (GA) in Dec 2025. This article documents the first production AVD deployment on the Perth Extended Zone, with an “beyond the docs” focus on the engineering required for an enterprise-grade setup: image engineering via Azure Image Builder + Azure Compute Gallery, Extended Zone replication using Managed Identity + REST, a private-only hub-and-spoke with custom DNS forwarders, and user-driven cost control without Azure portal access.

## Introduction

Perth is one of the most geographically isolated major cities globally. For performance‑sensitive, graphics‑heavy engineering workloads, user experience can degrade significantly when desktops are hosted far from the user.

For workloads such as subsurface modelling and GPU‑intensive analysis, reducing latency is critical not just for user experience but also for productivity. This makes Perth an ideal candidate for Azure Extended Zone deployments, where compute resources can operate closer to the workload users while still integrating with Azure’s regional services.

## Architecture at a Glance

- **Host pool type:** Personal (persistent, one VM per user, "Automatic" assignment)
- **Session hosts:** **NVadsA10 v5** VMs in the **Perth Extended Zone** (GPU-backed)
- **Identity:** Hybrid (domain-joined to on-premises Active Directory; Entra ID synced)
- **Access model:** Private-only (no public IPs; AVD Private Link + Private Endpoints across PaaS)
- **Network topology:** Hub-and-spoke with **custom DNS forwarders** for FQDN resolution
- **Image lifecycle:** AIB → Azure Compute Gallery (Australia East) → replication to **Perth Extended Zone**
- **Deployment automation:** GitHub Actions for VM provisioning, domain join, and host pool registration
- **Cost control:** User-triggered VM deallocation via a **"Stop My VDI"** desktop shortcut, using IMDS + Azure Automation
- **Role assignments:** Governed via **Saviynt** identity governance workflows (RBAC requests/approvals)

## Why Personal Host Pools — and Why No FSLogix

- GPU-accelerated subsurface apps are often per-seat licensed, stateful, and latency sensitive.
- Pooled host pools can add variability (contention, session density, app behavior).
- Personal host pools avoid that by giving each engineer a dedicated VM.

FSLogix was intentionally excluded: with persistent personal VMs, profile containers were seen as added complexity without meaningful benefit for these workload patterns.

## Image Engineering

Early on, a snapshot-based workflow was used temporarily while validating the Extended Zone environment and pipeline. After stabilization, the deployment aligned to an **Azure Image Builder (AIB)** based process.

- Images are produced via AIB.
- Published to **Azure Compute Gallery** in **Australia East**.
- Replicated to the **Perth Extended Zone** for session host deployment.

## Azure Image Builder template (example)

Templates are maintained as infrastructure as code (Bicep/ARM). They define:

- Base platform image
- Image customizations (PowerShell or file operations)
- Target Azure Compute Gallery for publishing

Simplified skeleton:

```bicep
param imageTemplateName string
param location string = 'australiaeast'
param subnetId string
param uamiId string
param galleryImageId string
param artifactsBaseUri string // e.g., https://<storage>.blob.core.windows.net/<container>

resource imageTemplate 'Microsoft.VirtualMachineImages/imageTemplates@2022-02-14' = {
  name: imageTemplateName
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${uamiId}': {}
    }
  }
  properties: {
    vmProfile: {
      vmSize: 'Standard_D4s_v5'
      vnetConfig: {
        subnetId: subnetId
      }
      // Optional: identities available inside the build VM
      userAssignedIdentities: [
        uamiId
      ]
    }

    source: {
      type: 'PlatformImage'
      publisher: 'MicrosoftWindowsDesktop'
      offer: 'windows-11'
      sku: 'win11-24h2-ent'
      version: 'latest'
    }

    customize: [
      // 1) Baseline prerequisites / org compliance
      {
        type: 'PowerShell'
        name: 'BaselinePrereqs'
        runElevated: true
        runAsSystem: true
        inline: [
          'Write-Output "Apply baseline compliance settings and prerequisites"'
          'Enable-WindowsOptionalFeature -Online -FeatureName NetFx3 -All -NoRestart'
          'Disable-WindowsOptionalFeature -Online -FeatureName SMB1Protocol -NoRestart'
        ]
      }

      // 2) Remove/deprovision built-in AppX packages
      {
        type: 'File'
        name: 'DownloadAppxRemovalScript'
        sourceUri: '${artifactsBaseUri}/scripts/Remove-Appx_Packages.zip'
        destination: 'C:\\Temp\\Remove-Appx_Packages.zip'
      }
      {
        type: 'PowerShell'
        name: 'RemoveAppxPackages'
        runElevated: true
        runAsSystem: true
        inline: [
          'Expand-Archive C:\\Temp\\Remove-Appx_Packages.zip -DestinationPath C:\\Temp\\RemoveAppx -Force'
          'PowerShell.exe -ExecutionPolicy Bypass -File C:\\Temp\\RemoveAppx\\RemoveAppx.ps1'
        ]
      }

      // 3) Install core enterprise apps (representative)
      {
        type: 'PowerShell'
        name: 'InstallCoreApps'
        runElevated: true
        runAsSystem: true
        inline: [
          'Write-Output "Install core enterprise applications (Office, Teams, etc.)"'
          // In practice this calls your internal packaging method
        ]
      }

      // 4) Install workload-specific apps
      {
        type: 'PowerShell'
        name: 'InstallWorkloadApps'
        runElevated: true
        runAsSystem: true
        inline: [
          'Write-Output "Install subsurface workload applications and configurations"'
        ]
      }

      // 5) Cleanup / finalize
      {
        type: 'PowerShell'
        name: 'Cleanup'
        runElevated: true
        runAsSystem: true
        inline: [
          'Write-Output "Cleanup temp files and finalize image"'
          'Remove-Item C:\\Temp\\* -Recurse -Force -ErrorAction SilentlyContinue'
        ]
      }
    ]

    distribute: [
      {
        type: 'SharedImage'
        runOutputName: 'sigOutput'
        galleryImageId: galleryImageId
        replicationRegions: [
          'australiaeast'
        ]
      }
    ]
  }
}
```

## Build execution

Template deployment and build trigger via Azure PowerShell:

```powershell
New-AzResourceGroupDeployment `
  -ResourceGroupName <imageBuilderResourceGroup> `
  -TemplateFile <imageTemplateFile> `
  -ImageTemplateName <templateName> `
  -Location australiaeast

Start-AzImageBuilderTemplate `
  -ResourceGroupName <imageBuilderResourceGroup> `
  -Name <templateName> `
  -NoWait
```

After success, a new image version exists in the gallery, ready for consumption/replication.

## Key Extended Zone constraint: image builds occur in the parent region

Supported pattern for Perth Extended Zone AVD:

**Build + publish image in Australia East → replicate to Perth Extended Zone → deploy session host VMs in Perth**

This aligns with AVD Extended Zone design: control plane in parent region; execution layer (session hosts) in the Extended Zone.

## Replicating to Perth Extended Zone via Managed Identity + REST

To replicate an image version from Azure Compute Gallery in Australia East to Perth Extended Zone, the gallery needs a **one-time** prerequisite: associate a **User-Assigned Managed Identity** with the gallery.

Because this can’t currently be done through the Azure portal, it’s performed via Azure REST APIs (validated with Postman).

### Step 1 — Setup Managed Identity

```bash
az identity create \
  --name "mi-gallery-perth-replication" \
  --resource-group "<RG_NAME>" \
  --location "australiaeast"

az role assignment create \
  --assignee <IDENTITY_PRINCIPAL_ID> \
  --role "Reader" \
  --scope "/subscriptions/<Subscription_ID>"
```

### Step 2 — Generate Access Token

```bash
az account get-access-token --resource https://management.azure.com/
```

Copy `accessToken` from the output.

### Step 3 — Configure Postman Request

**Method:** `PUT`

**URL:**

```text
https://australiaeast.management.azure.com/subscriptions/<Subscription_ID>/resourceGroups/<RG_NAME>/providers/Microsoft.Compute/galleries/<Gallery_name>?api-version=2023-07-03
```

**Authorization — Option A (OAuth 2.0):**

| Field | Value |
| --- | --- |
| Auth Type | OAuth 2.0 |
| Grant Type | Authorization Code |
| Auth URL | https://login.microsoftonline.com/<tenant-id>/oauth2/v2.0/authorize |
| Access Token URL | https://login.microsoftonline.com/<tenant-id>/oauth2/v2.0/token |
| Client ID | Your Azure App Registration Client ID |
| Client Secret | Your Azure App Registration Client Secret |
| Scope | https://management.azure.com/.default |
| Header Prefix | Bearer |

**Authorization — Option B (Bearer token):**

- Auth Type: Bearer Token
- Token: the `accessToken` from CLI

**Headers:**

| Key | Value |
| --- | --- |
| Content-Type | application/json |
| Authorization | Bearer {access_token} *(auto-added if using OAuth 2.0)* |

**Request Body:**

```json
{
  "location": "australiaeast",
  "identity": {
    "type": "UserAssigned",
    "userAssignedIdentities": {
      "/subscriptions/<Subscription_ID>/resourceGroups/<RG_NAME>/providers/Microsoft.ManagedIdentity/userAssignedIdentities/<managed-identity-name>": {}
    }
  }
}
```

### Step 4 — Execute and Verify

- Postman should return **200 OK** or **201 Created** with `provisioningState: Succeeded`.
- Azure Portal: Azure Compute Gallery → Identity.
- CLI:

```bash
az sig show --resource-group <RG_NAME> --gallery-name <Gallery_name> --query identity
```

## Cost Control: User-initiated VM Deallocation via Desktop Shortcut

In a personal host pool without autoscale, idle GPU VMs are a cost risk (NVadsA10 v5 isn’t cheap). Users are domain specialists and aren’t expected to manage Azure resources.

Solution: place a desktop shortcut called **"Stop My VDI"** on each session host. Clicking it triggers VM deallocation automatically, without Azure portal access.

### How it works

1. User clicks **"Stop My VDI"**.
2. Shortcut runs a PowerShell script.
3. Script queries **IMDS** to find the VM name and resource group.
4. Script calls an **Azure Automation webhook** with that payload.
5. Runbook uses its **managed identity** to deallocate the VM.
6. Next connect triggers **Start VM on Connect** to power it back on.

### Script on VDI to initiate deallocation

```powershell
$WebhookURI = "WEBHOOK_URL_PLACEHOLDER"

# Pull VM name and RG directly from Azure Instance Metadata Service (IMDS)
$metadata = Invoke-RestMethod -Uri "http://169.254.169.254/metadata/instance?api-version=2021-02-01" -Headers @{Metadata="true"}

$vmName = $metadata.compute.name
$rgName = $metadata.compute.resourceGroupName

$body = @{ VMName = $vmName; ResourceGroup = $rgName } | ConvertTo-Json -Compress

try {
  Invoke-RestMethod -Uri $WebhookURI -Method Post -Body $body -ContentType "application/json"
  Write-Host "Deallocate request submitted for $vmName. You will be disconnected shortly."
} catch {
  Write-Host "Webhook call failed: $($_.Exception.Message)"
  if ($_.Exception.Response -and $_.Exception.Response.StatusCode) {
    Write-Host "HTTP Status: $($_.Exception.Response.StatusCode.value__)"
  }
}
```

Notes:

- The IMDS endpoint `169.254.169.254` is link-local and accessible only within the VM (helpful for private-only architectures).
- The webhook URI is stored as a secret in Key Vault and retrieved during deployment.

### Automation Account runbook

```powershell
param (
  [object] $WebhookData
)

$body = ConvertFrom-Json $WebhookData.RequestBody
$vmName = $body.VMName
$rgName = $body.ResourceGroup

Connect-AzAccount -Identity

Write-Output "Deallocating VM: $vmName in RG: $rgName"

Stop-AzVM -Name $vmName -ResourceGroupName $rgName -Force
```

- `Connect-AzAccount -Identity` uses the Automation Account’s system-assigned managed identity.
- For **Start VM on Connect**, the service principal named **"Azure Virtual Desktop"** must be granted **Desktop Virtualization Power On Contributor**.

### Alternative: Logoff-based trigger

If a desktop shortcut isn’t suitable, the same script can run as a **GPO logoff script**.

High-level steps:

- Save the script (e.g., `Deallocate-MyVM.ps1`) to a share or local path accessible by session hosts.
- In **Group Policy Management Console (GPMC)**, create/edit a GPO linked to the OU containing session host computer objects.
- Configure:
  - Computer Configuration → Windows Settings → Scripts → Shutdown, or
  - User Configuration → Windows Settings → Scripts → Logoff
- Add the PowerShell script.
- Ensure the GPO targets the correct OU and test.

## Closing thoughts

Perth’s Extended Zone is a strong fit for latency-sensitive workloads. The practical patterns covered here—AIB + Compute Gallery publishing, replication prerequisites via Managed Identity + REST, private-only networking, and user-driven cost control—show what it takes to move from “supported” to “production-ready”.

## References

- [Azure Extended Zones documentation | Microsoft Learn](https://learn.microsoft.com/en-us/azure/extended-zones/)
- [Azure Instance Metadata Service for virtual machines - Azure Virtual Machines | Microsoft Learn](https://learn.microsoft.com/en-us/azure/virtual-machines/instance-metadata-service?tabs=windows)
- [Deallocate VM on user logoff | Microsoft Community Hub](https://techcommunity.microsoft.com/discussions/azurevirtualdesktopforum/deallocate-vm-on-user-logoff/2280211)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/azure-virtual-desktop-in-the-perth-azure-extended-zone-a-real/ba-p/4508550)

