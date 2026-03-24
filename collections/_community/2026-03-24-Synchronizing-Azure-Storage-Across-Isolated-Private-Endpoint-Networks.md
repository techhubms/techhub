---
title: Synchronizing Azure Storage Across Isolated Private Endpoint Networks
date: 2026-03-24 08:27:32 +00:00
author: deepakkumarv
tags:
- 403 Error
- Audit Logging
- Az PowerShell Module
- Az.Storage
- AzCopy
- Azcopy Sync
- Azure
- Azure Blob Storage
- Azure Storage
- Azure Virtual Machine
- CannotVerifyCopySource
- Community
- DevOps
- DNS A Records
- Hub And Spoke Network
- Managed Identity
- Microsoft Entra ID
- Network Isolation
- PowerShell
- Private DNS Zone
- Private Endpoints
- Public Network Access Disabled
- RBAC
- Region To Region Migration
- Repatriation
- Security
- Storage Blob Data Contributor
- Storage Blob Data Reader
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/synchronizing-azure-storage-across-isolated-private-endpoint/ba-p/4504564
section_names:
- azure
- devops
- security
feed_name: Microsoft Tech Community
primary_section: azure
---

deepakkumarv walks through a target-anchored pattern for syncing Azure Blob data between two fully isolated Azure Storage accounts (private endpoints, no shared DNS, no public access), using an Azure VM plus AzCopy with Managed Identity and Entra ID RBAC, including PowerShell scripts for sync and post-sync validation.<!--excerpt_end-->

## Overview

In Azure repatriation (region-to-region or subscription-to-subscription) programs, you may need to migrate large volumes of blob data between a **source** and **target** Azure environment that are **fully isolated**:

- Both Storage Accounts have **public network access disabled**
- Access is only via **Private Endpoints**
- Each environment has its own **Private DNS zone** (no DNS sharing)
- There is **no shared VNet** and **no direct private connectivity** between environments

When both ends are locked down like this, **server-side copy** commonly fails (for example, Azure Storage copy operations that require the destination to verify the source), often with:

```text
403 – CannotVerifyCopySource
```

The requirement is to migrate securely **without**:

- Enabling public network access
- Using SAS tokens or storage account keys
- Re-architecting the source environment
- Relaxing enterprise isolation boundaries

## Typical repatriation architecture

### Source environment (Region A)

- Azure Storage Account (Blob)
- Source subscription
- Public network access disabled
- Private Endpoint in a source hub-spoke network
- Private DNS zone scoped only to the source network

### Target environment (Region B)

- Azure Storage Account (Blob)
- Target subscription
- Public network access disabled
- Private Endpoint in a target hub-spoke network
- Independent Private DNS zone (no sharing with source)

## Recommended pattern: target-anchored data movement VM

**Core principle:** run the transfer from a controlled client in the **target** environment.

Instead of attempting direct storage-to-storage copy across isolated networks, deploy an **Azure VM** in the target subscription that can reach:

- The **target** Storage Account via its private endpoint
- The **source** Storage Account via its private endpoint (with explicit DNS alignment in the target)

### Execution flow

1. Deploy an Azure VM in the target subscription.
2. Place the VM in:
   - The same VNet as the target Storage private endpoint, **or**
   - A peered VNet that has access to the target private endpoint.
3. In the **target private DNS zone**, create DNS **A records** for:
   - `sourceaccount.blob.core.windows.net`
   - `targetaccount.blob.core.windows.net`
4. Map each record to the **private IP** of its corresponding private endpoint.
5. Run **AzCopy** on the VM.
6. Authenticate using **Microsoft Entra ID** via **Managed Identity**.
7. Read from source blobs and write to target blobs over private networking.

## DNS configuration (critical)

Because source and target environments do not share DNS, you must explicitly align DNS resolution **in the target environment**.

In the **target private DNS zone**:

- Create A records for:
  - `sourceaccount.blob.core.windows.net`
  - `targetaccount.blob.core.windows.net`
- Point each record to the private endpoint IP of the matching Storage Account.

This ensures:

- AzCopy resolves both endpoints to **private IPs**
- Public endpoint resolution is avoided
- Transfers stay within isolation and security policies

Without this DNS alignment, AzCopy authentication/transfer can fail even when networking and RBAC are correct.

## Identity & access configuration

Use the VM’s **Managed Identity** for authentication.

Required role assignments:

- On **source** Storage Account:
  - `Storage Blob Data Reader`
- On **target** Storage Account:
  - `Storage Blob Data Contributor`

AzCopy performs data plane operations using **Entra ID RBAC** (no account keys, no SAS).

## PowerShell scripts

The article provides three scripts:

- Script 1: installs prerequisites (Az module, AzCopy)
- Script 2: syncs blobs container-by-container using `azcopy sync`
- Script 3: post-sync validation (folder summaries, counts, bytes, 0-byte object checks) and CSV export

### Script 1: package installations (Az + AzCopy)

```powershell
# Ensure script runs with admin privileges
if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent() ).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
  Write-Error "Please run PowerShell as Administrator."
  exit
}

# Enforce TLS 1.2 (recommended for PSGallery)
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# -------------------------------
# Check and Install Az module
# -------------------------------
if (-not (Get-Module -ListAvailable -Name Az)) {
  Write-Output "Az module not found. Installing Az PowerShell module..."
  Install-Module -Name Az -Repository PSGallery -Force -AllowClobber
} else {
  Write-Output "Az module already installed. Skipping installation."
}

# Verify Az module
Write-Output "Verifying Az module installation..."
Import-Module Az
Get-Module Az -ListAvailable | Select-Object Name, Version

# -------------------------------
# Check & Install AzCopy module
# -------------------------------
Write-Host "Checking AzCopy installation..." -ForegroundColor Cyan

# 1. Check if azcopy is already available in PATH
if (Get-Command azcopy -ErrorAction SilentlyContinue) {
  Write-Host "AzCopy already installed and available in PATH." -ForegroundColor Green
  azcopy --version
  return
}

# 2. Check standard install location
$installPath = Join-Path $env:ProgramFiles "AzCopy"
$targetExe = Join-Path $installPath "azcopy.exe"

if (Test-Path $targetExe) {
  Write-Host "AzCopy found at $targetExe" -ForegroundColor Green
} else {
  Write-Host "AzCopy not found. Downloading and installing..." -ForegroundColor Yellow

  # Download
  $azCopyUrl = "https://aka.ms/downloadazcopy-v10-windows"
  $zipPath = Join-Path $env:TEMP "azcopy.zip"
  $extractPath = Join-Path $env:TEMP "azcopy_extract"

  Invoke-WebRequest -Uri $azCopyUrl -OutFile $zipPath

  if (Test-Path $extractPath) { Remove-Item $extractPath -Recurse -Force }
  Expand-Archive -Path $zipPath -DestinationPath $extractPath -Force

  # Find azcopy.exe inside extracted folder
  $foundExe = Get-ChildItem -Path $extractPath -Recurse -Filter "azcopy.exe" |
    Select-Object -First 1 -ExpandProperty FullName

  if (-not $foundExe) { throw "azcopy.exe not found after extraction." }

  # Copy to Program Files\AzCopy
  New-Item -ItemType Directory -Path $installPath -Force | Out-Null
  Copy-Item -Path $foundExe -Destination $targetExe -Force
}

# 3. Add AzCopy to Machine PATH (only if missing)
Write-Host "Ensuring AzCopy is added to PATH..." -ForegroundColor Cyan

$machinePath = [Environment]::GetEnvironmentVariable("Path", "Machine")
if ($machinePath -notlike "*$installPath*") {
  [Environment]::SetEnvironmentVariable(
    "Path",
    "$machinePath;$installPath",
    [EnvironmentVariableTarget]::Machine
  )
}

# Update current session PATH so azcopy works immediately
$env:Path = "$env:Path;$installPath"

# 4. Verify installation
Write-Host "Verifying AzCopy installation..." -ForegroundColor Cyan
& $targetExe --version
azcopy --version
```

### Script 2: blob synchronization (AzCopy sync)

```powershell
# Login using the VM's System Assigned Identity
Connect-AzAccount -Identity

# Use below command if managed identity is associated with VM
# $clientId = "VMManagedIdentity"
# Connect-AzAccount -Identity -AccountId $clientId

# Define source and destination account pairs with subscription IDs
$storagePairs = @(
  @{
    SourceAccount = "SourceStorageAccountName"; SourceRG = "SourceResourceGroup"; SourceSub = "SourceSubscriptionId";
    DestAccount   = "DestinationStorageAccountName"; DestRG   = "DestinationResourceGroup"; DestSub   = "DestinationSubscriptionId"
  }
  # Add more pairs as needed
)

foreach ($pair in $storagePairs) {
  $sourceAccount = $pair.SourceAccount
  $sourceRG      = $pair.SourceRG
  $sourceSub     = $pair.SourceSub

  $destAccount   = $pair.DestAccount
  $destRG        = $pair.DestRG
  $destSub       = $pair.DestSub

  Write-Host "`n?? Processing pair: $sourceAccount ($sourceSub) ? $destAccount ($destSub)"

  try {
    # Set context to source subscription and get context
    Set-AzContext -SubscriptionId $sourceSub
    $sourceContext = New-AzStorageContext -StorageAccountName $sourceAccount

    # Set context to destination subscription and get context
    Set-AzContext -SubscriptionId $destSub
    $destContext = New-AzStorageContext -StorageAccountName $destAccount

    # Get all containers from the source account
    $containers = Get-AzStorageContainer -Context $sourceContext

    foreach ($container in $containers) {
      $containerName = $container.Name
      Write-Host "`n?? Syncing container: $containerName"

      try {
        # Check if destination container exists
        $destContainer = Get-AzStorageContainer -Name $containerName -Context $destContext -ErrorAction SilentlyContinue

        if (-not $destContainer) {
          New-AzStorageContainer -Name $containerName -Context $destContext | Out-Null
          Write-Host "? Created destination container: $containerName"
        }

        # Build source and destination URLs
        $sourceUrl = "https://$sourceAccount.blob.core.windows.net/$containerName"
        $destUrl   = "https://$destAccount.blob.core.windows.net/$containerName"

        # Run AzCopy sync using identity
        azcopy login --identity

        Write-Host "Login Successful"
        Write-Host "Sync Started"
        azcopy sync $sourceUrl $destUrl --recursive=true --compare-hash=MD5 --include-directory-stub=true

      } catch {
        Write-Error "? Error syncing container '$containerName': $_"
      }
    }

  } catch {
    Write-Error "? Error processing storage pair $sourceAccount ? $destAccount : $_"
  }
}
```

### Script 3: post-sync validation (PowerShell-only)

```powershell
# =====================================================================
# POST-AZSYNC VALIDATION (PowerShell-only, NO Azure CLI)
# - Uses VM Managed Identity (Connect-AzAccount -Identity)
# - Validates ALL containers: folders, blob count, total bytes
# - Includes 0-byte files and 0-byte folder stubs
# - Exports CSV
# =====================================================================

# ------------------ CONFIG ------------------
$SrcStorageAccount = "SourceStorageAccountName"
$DstStorageAccount = "DestinationStorageAccountName"

# Output folder
$OutDir = ".\PostAzSync-Outputfile"
New-Item -ItemType Directory -Path $OutDir -Force | Out-Null

# Optional: restrict to specific container names
$OnlyTheseContainers = @()

# Blob listing page size
$PageSize = 5000

# ------------------ AUTH (VM Managed Identity) ------------------
Connect-AzAccount -Identity | Out-Null

# ------------------ CONTEXTS (OAuth) ------------------
$SrcCtx = New-AzStorageContext -StorageAccountName $SrcStorageAccount -UseConnectedAccount
$DstCtx = New-AzStorageContext -StorageAccountName $DstStorageAccount -UseConnectedAccount

function Get-Containers {
  param([Parameter(Mandatory=$true)]$Ctx)
  return (Get-AzStorageContainer -Context $Ctx | Select-Object -ExpandProperty Name | Sort-Object -Unique)
}

function Get-AllBlobsPaged {
  param(
    [Parameter(Mandatory=$true)][string]$Container,
    [Parameter(Mandatory=$true)]$Ctx,
    [int]$MaxCount = 5000
  )

  $all = @()
  $token = $null

  do {
    $page = Get-AzStorageBlob -Container $Container -Context $Ctx -MaxCount $MaxCount -ContinuationToken $token

    if ($page) {
      $all += $page
      $token = $page[$page.Count - 1].ContinuationToken
    } else {
      $token = $null
    }
  } while ($token -ne $null)

  return $all
}

function Get-AllPrefixesForBlob {
  param([Parameter(Mandatory=$true)][string]$BlobName)

  $parts = $BlobName -split "/"
  if ($parts.Count -le 1) { return @() }

  $prefixes = @()
  for ($i = 1; $i -le ($parts.Count - 1); $i++) {
    $prefixes += (($parts[0..($i-1)] -join "/") + "/")
  }

  return $prefixes
}

function Get-FolderSummary {
  param([Parameter(Mandatory=$true)]$BlobObjects)

  $expanded = foreach ($b in $BlobObjects) {
    $name = [string]$b.Name
    $size = [int64]$b.Length

    if ([string]::IsNullOrWhiteSpace($name)) { continue }

    foreach ($p in (Get-AllPrefixesForBlob -BlobName $name)) {
      $lvl = ($p.TrimEnd("/") -split "/").Count
      [pscustomobject]@{ Level = $lvl; Folder = $p; SizeBytes = $size }
    }
  }

  $summary = $expanded |
    Group-Object Level, Folder |
    ForEach-Object {
      $lvl = $_.Group[0].Level
      $folder = $_.Group[0].Folder
      $total = ($_.Group | Measure-Object SizeBytes -Sum).Sum
      [pscustomobject]@{ Level = $lvl; Folder = $folder; BlobCount = $_.Count; TotalBytes = [int64]$total }
    } |
    Sort-Object Level, Folder

  return $summary
}

function Compare-Summaries {
  param(
    [Parameter(Mandatory=$true)]$SrcSummary,
    [Parameter(Mandatory=$true)]$DstSummary
  )

  $srcIndex = @{}
  foreach ($r in $SrcSummary) { $srcIndex["$($r.Level)|$($r.Folder)"] = $r }

  $dstIndex = @{}
  foreach ($r in $DstSummary) { $dstIndex["$($r.Level)|$($r.Folder)"] = $r }

  $keys = ($srcIndex.Keys + $dstIndex.Keys) | Sort-Object -Unique

  $compare = foreach ($k in $keys) {
    $s = $srcIndex[$k]
    $d = $dstIndex[$k]
    $parts = $k -split "\|", 2

    $srcCount = if ($s) { $s.BlobCount } else { 0 }
    $dstCount = if ($d) { $d.BlobCount } else { 0 }
    $srcBytes = if ($s) { $s.TotalBytes } else { 0 }
    $dstBytes = if ($d) { $d.TotalBytes } else { 0 }

    [pscustomobject]@{
      Level          = [int]$parts[0]
      Folder         = $parts[1]
      SrcBlobCount   = $srcCount
      DstBlobCount   = $dstCount
      SrcTotalBytes  = $srcBytes
      DstTotalBytes  = $dstBytes
      BlobCountDelta = ($dstCount - $srcCount)
      TotalBytesDelta= ($dstBytes - $srcBytes)
    }
  }

  return ($compare | Sort-Object Level, Folder)
}

function Get-ZeroByteObjects {
  param([Parameter(Mandatory=$true)]$BlobObjects)
  if ($null -eq $BlobObjects) { return @() }
  return @(
    $BlobObjects |
      Where-Object { [int64]$_.Length -eq 0 } |
      Select-Object -ExpandProperty Name |
      Sort-Object
  )
}

function Get-ZeroByteFolderStubs {
  param([Parameter(Mandatory=$true)]$BlobObjects)
  if ($null -eq $BlobObjects) { return @() }
  return @(
    $BlobObjects |
      Where-Object { ([int64]$_.Length -eq 0) -and ([string]$_.Name).EndsWith("/") } |
      Select-Object -ExpandProperty Name |
      Sort-Object
  )
}

function Diff-List {
  param(
    [Parameter(Mandatory=$true)]$SrcList,
    [Parameter(Mandatory=$true)]$DstList
  )

  if ($null -eq $SrcList) { $SrcList = @() }
  if ($null -eq $DstList) { $DstList = @() }

  $diff = Compare-Object -ReferenceObject $SrcList -DifferenceObject $DstList
  if (-not $diff) { return @() }

  return @(
    $diff | ForEach-Object {
      $present = if ($_.SideIndicator -eq "<=") { "SOURCE" } elseif ($_.SideIndicator -eq "=>") { "DESTINATION" } else { "UNKNOWN" }
      [pscustomobject]@{ Name = $_.InputObject; PresentOnlyIn = $present }
    }
  )
}

function Export-CsvSafe {
  param(
    [Parameter(Mandatory=$true)]$Data,
    [Parameter(Mandatory=$true)][string]$Path,
    [switch]$NoTypeInformation
  )

  if ($null -eq $Data) { $Data = @() }
  if ($NoTypeInformation) {
    $Data | Export-Csv -Path $Path -NoTypeInformation
  } else {
    $Data | Export-Csv -Path $Path
  }
}

# ------------------ GET CONTAINER SETS ------------------
$srcContainers = Get-Containers -Ctx $SrcCtx
$dstContainers = Get-Containers -Ctx $DstCtx

$allContainers = ($srcContainers + $dstContainers) | Sort-Object -Unique

if ($OnlyTheseContainers.Count -gt 0) {
  $allContainers = $allContainers | Where-Object { $OnlyTheseContainers -contains $_ }
}

if ($allContainers.Count -eq 0) {
  throw "No containers returned. Ensure MI has Storage Blob Data Reader (or higher) on both accounts."
}

# ------------------ PROCESS ALL CONTAINERS ------------------
$master = @()

foreach ($c in $allContainers) {
  Write-Host "`n=============================="
  Write-Host "CONTAINER: $c"
  Write-Host "=============================="

  $srcBlobs = @()
  $dstBlobs = @()

  try {
    $srcBlobs = Get-AllBlobsPaged -Container $c -Ctx $SrcCtx -MaxCount $PageSize -ErrorAction Stop
  } catch {
    Write-Warning ("SRC error on container {0}: {1}" -f $c, $_.Exception.Message)
    $srcBlobs = @()
  }

  try {
    $dstBlobs = Get-AllBlobsPaged -Container $c -Ctx $DstCtx -MaxCount $PageSize -ErrorAction Stop
  } catch {
    Write-Warning ("DST error on container {0}: {1}" -f $c, $_.Exception.Message)
    $dstBlobs = @()
  }

  $srcSummary = Get-FolderSummary -BlobObjects $srcBlobs
  $dstSummary = Get-FolderSummary -BlobObjects $dstBlobs
  $cmp = Compare-Summaries -SrcSummary $srcSummary -DstSummary $dstSummary

  $srcZero = @(Get-ZeroByteObjects -BlobObjects $srcBlobs)
  $dstZero = @(Get-ZeroByteObjects -BlobObjects $dstBlobs)
  $zeroDiff = Diff-List -SrcList $srcZero -DstList $dstZero

  $srcStub = @(Get-ZeroByteFolderStubs -BlobObjects $srcBlobs)
  $dstStub = @(Get-ZeroByteFolderStubs -BlobObjects $dstBlobs)
  $stubDiff = Diff-List -SrcList $srcStub -DstList $dstStub

  $cmp | Format-Table Level, Folder, SrcBlobCount, DstBlobCount, SrcTotalBytes, DstTotalBytes, BlobCountDelta, TotalBytesDelta -AutoSize

  $safe = $c -replace '[^a-zA-Z0-9\-]', '_'
  Export-CsvSafe -Data $srcSummary -Path (Join-Path $OutDir "SRC_${safe}_folder_summary.csv")
  Export-CsvSafe -Data $dstSummary -Path (Join-Path $OutDir "DST_${safe}_folder_summary.csv")
  Export-CsvSafe -Data $cmp        -Path (Join-Path $OutDir "CMP_${safe}_Folder_Diff.csv")
  Export-CsvSafe -Data $zeroDiff   -Path (Join-Path $OutDir "CMP_${safe}_ZeroByteFiles_Diff.csv")
  Export-CsvSafe -Data $stubDiff   -Path (Join-Path $OutDir "CMP_${safe}_ZeroByteFolderStubs_Diff.csv")

  foreach ($row in $cmp) {
    $master += [pscustomobject]@{
      Container       = $c
      Level           = $row.Level
      Folder          = $row.Folder
      SrcBlobCount    = $row.SrcBlobCount
      DstBlobCount    = $row.DstBlobCount
      SrcTotalBytes   = $row.SrcTotalBytes
      DstTotalBytes   = $row.DstTotalBytes
      BlobCountDelta  = $row.BlobCountDelta
      TotalBytesDelta = $row.TotalBytesDelta
    }
  }
}

Export-CsvSafe -Data $master -Path (Join-Path $OutDir "CMP_MASTER_allcontainers_folder.csv") -NoTypeInformation
Write-Host "All Containers Exported to: $OutDir"
```

## Key features (as described)

- No SAS tokens, no storage account keys, and no environment-specific secrets
- Runs from the target VM using Managed Identity
- Uses `azcopy sync` for resumability and large datasets
- Enables logging/audit and troubleshooting
- Handles partial failures (skips failed containers rather than terminating the whole run)

## When to use this pattern

### Recommended

- Azure repatriation / region-to-region migration
- Source environment is locked down and cannot be modified
- Separate hub-spoke architectures per region
- One-time or phased migrations
- Strong compliance/security requirements

After repatriation and decommissioning the source environment, temporary private endpoint/DNS records created in the target can be removed.

### Less suitable

- Continuous or near real-time replication requirements
- Architectures with shared networking between environments
- Disaster recovery scenarios requiring bi-directional sync

## References

- AzCopy overview: [Copy or move data to Azure Storage by using AzCopy v10 | Microsoft Learn](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azcopy-v10)
- Azure Storage PowerShell module: [Az.Storage Module | Microsoft Learn](https://learn.microsoft.com/en-gb/powershell/module/az.storage/?view=azps-15.4.0)
- Network-restricted blob copy guidance: [Use AzCopy to copy blobs between storage accounts with access restriction - Azure | Microsoft Learn](https://learn.microsoft.com/en-us/troubleshoot/azure/azure-storage/blobs/connectivity/copy-blobs-between-storage-accounts-network-restriction)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/synchronizing-azure-storage-across-isolated-private-endpoint/ba-p/4504564)

