#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deletes untagged (dangling) manifests from the shared Tech Hub Container Registry.

.DESCRIPTION
    Every CI build pushes a new image with a unique tag (commit SHA or PR timestamp),
    which over time leaves many untagged manifests from replaced :latest / :pr-N tags.
    Because the registry is the 'Standard' SKU, the built-in retention policy
    feature is not available (Premium only). This script performs the same cleanup
    by enumerating manifests with no tags that are older than a cutoff and deleting them.

    Safe to run repeatedly — deletion is idempotent. Run from an admin machine
    that can reach ACR (the registry has public network access enabled).

.PARAMETER RegistryName
    Container Registry name (without the .azurecr.io suffix). Defaults to 'crtechhubms'.

.PARAMETER Repository
    Specific repository to clean, or 'all' (default) to clean every repository in the registry.

.PARAMETER AgeDays
    Minimum age (in days) of an untagged manifest before it is deleted. Defaults to 7.

.PARAMETER DryRun
    Only list manifests that would be deleted, do not actually delete.

.EXAMPLE
    # Preview what would be deleted across all repos, older than 7 days
    ./scripts/Cleanup-ContainerRegistry.ps1 -DryRun

.EXAMPLE
    # Actually delete untagged manifests older than 14 days from the techhub-api repo
    ./scripts/Cleanup-ContainerRegistry.ps1 -Repository techhub-api -AgeDays 14
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$RegistryName = 'crtechhubms',

    [Parameter(Mandatory = $false)]
    [string]$Repository = 'all',

    [Parameter(Mandatory = $false)]
    [int]$AgeDays = 7,

    [Parameter(Mandatory = $false)]
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$cutoff = (Get-Date -AsUTC).AddDays(-$AgeDays)
Write-Host "Cleanup target: $($RegistryName) | repository='$($Repository)' | age>$($AgeDays)d | dryRun=$($DryRun)" -ForegroundColor Cyan
Write-Host "Cutoff (UTC): $($cutoff.ToString('o'))" -ForegroundColor Cyan

# Determine repositories to scan
if ($Repository -eq 'all') {
    $repoListJson = az acr repository list --name $RegistryName --output json
    if ($LASTEXITCODE -ne 0) { throw "Failed to list repositories on '$($RegistryName)'." }
    $repositories = $repoListJson | ConvertFrom-Json
}
else {
    $repositories = @($Repository)
}

$totalDeleted = 0
foreach ($repo in $repositories) {
    Write-Host ""
    Write-Host "Scanning repository: $($repo)" -ForegroundColor Yellow

    $manifestsJson = az acr manifest list-metadata `
        --registry $RegistryName `
        --name $repo `
        --orderby time_asc `
        --output json

    if ($LASTEXITCODE -ne 0) {
        Write-Warning "Failed to enumerate manifests for '$($repo)'. Skipping."
        continue
    }

    $manifests = $manifestsJson | ConvertFrom-Json
    $candidates = @($manifests | Where-Object {
            $isUntagged = $null -eq $_.tags -or $_.tags.Count -eq 0
            $isOlderThanCutoff = [datetime]::Parse($_.lastUpdateTime).ToUniversalTime() -lt $cutoff
            $isUntagged -and $isOlderThanCutoff
        })

    Write-Host "  Found $($candidates.Count) untagged manifest(s) older than cutoff." -ForegroundColor Gray

    foreach ($manifest in $candidates) {
        $reference = "$($repo)@$($manifest.digest)"
        if ($DryRun) {
            Write-Host "  [dry-run] Would delete $($reference) (updated $($manifest.lastUpdateTime))" -ForegroundColor DarkGray
        }
        else {
            Write-Host "  Deleting $($reference)..." -ForegroundColor Red
            az acr repository delete `
                --name $RegistryName `
                --image $reference `
                --yes `
                --output none

            if ($LASTEXITCODE -eq 0) {
                $totalDeleted++
            }
            else {
                Write-Warning "  Failed to delete $($reference)."
            }
        }
    }
}

Write-Host ""
if ($DryRun) {
    Write-Host "Dry run complete. Re-run without -DryRun to actually delete." -ForegroundColor Yellow
}
else {
    Write-Host "Cleanup complete. Deleted $($totalDeleted) manifest(s)." -ForegroundColor Green
}
