#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Configures OIDC (Workload Identity Federation) for GitHub Actions CI/CD authentication.

.DESCRIPTION
    Sets up all federated credentials on the TechHub service principal (sp-techhubms) and
    stores the Azure identity values as GitHub repository variables so that all GitHub Actions
    workflows can authenticate to Azure without any stored secrets.

    The following federated credentials are created (idempotent - skips existing ones):
      - techhub-environment-staging    repo:...:environment:staging
      - techhub-environment-production repo:...:environment:production
      - techhub-ref-main               repo:...:ref:refs/heads/main
      - techhub-pull-request           repo:...:pull_request

    The pull_request subject covers PR preview jobs that have no GitHub environment assigned
    (pr-preview-build-and-push, pr-preview-teardown), including Dependabot PRs.

    After running this script, ensure no AZURE_CREDENTIALS secret exists in GitHub - it is not used.

    Requires:
      - Azure CLI (az) authenticated with permission to manage app registrations
      - GitHub CLI (gh) authenticated with write access to the repository

.PARAMETER AppId
    Application (client) ID of the service principal. Defaults to sp-techhubms.

.PARAMETER GitHubRepo
    GitHub repository in owner/name format. Defaults to techhubms/techhub.

.EXAMPLE
    ./scripts/Setup-OidcAuthentication.ps1
    Configure OIDC for the default service principal and repository.

.EXAMPLE
    ./scripts/Setup-OidcAuthentication.ps1 -AppId 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
    Configure OIDC for a specific service principal.
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$AppId = '91fe5f39-bee3-48b1-b976-e8ba4e41d84e',

    [Parameter(Mandatory = $false)]
    [string]$GitHubRepo = 'techhubms/techhub'
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# PRE-FLIGHT: Verify tooling is available and authenticated
# ============================================================================

Write-Host ""
Write-Host "TechHub OIDC Setup" -ForegroundColor Cyan
Write-Host "==================" -ForegroundColor Cyan

Write-Host ""
Write-Host "Checking prerequisites..." -ForegroundColor Cyan

# Verify Azure CLI session
$accountOutput = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "  [FAIL] Not logged in to Azure. Run: az login" -ForegroundColor Red
    exit 1
}
$account = $accountOutput | ConvertFrom-Json
$tenantId = $($account.tenantId)
$subscriptionId = $($account.id)
$subscriptionName = $($account.name)

Write-Host "  [OK] Azure CLI authenticated" -ForegroundColor Green
Write-Host "       Tenant:       $tenantId" -ForegroundColor Gray
Write-Host "       Subscription: $subscriptionName ($subscriptionId)" -ForegroundColor Gray

# Verify GitHub CLI session
$ghStatusOutput = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "  [FAIL] Not logged in to GitHub. Run: gh auth login" -ForegroundColor Red
    exit 1
}
Write-Host "  [OK] GitHub CLI authenticated" -ForegroundColor Green

# ============================================================================
# STEP 1: Verify the app registration exists
# ============================================================================

Write-Host ""
Write-Host "Verifying app registration $AppId..." -ForegroundColor Cyan

$appOutput = az ad app show --id $AppId --query "{displayName:displayName, appId:appId}" -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "  [FAIL] App registration '$AppId' not found." -ForegroundColor Red
    Write-Host "         Ensure you are logged in to the correct tenant." -ForegroundColor Red
    exit 1
}
$app = $appOutput | ConvertFrom-Json
Write-Host "  [OK] Found: $($app.displayName) ($($app.appId))" -ForegroundColor Green

# ============================================================================
# STEP 2: Create federated credentials (idempotent)
# ============================================================================

Write-Host ""
Write-Host "Configuring federated credentials..." -ForegroundColor Cyan

$credentials = @(
    @{
        name        = 'techhub-environment-staging'
        subject     = "repo:$GitHubRepo`:environment:staging"
        description = 'Staging environment deployments (deploy-staging-infra, pr-preview-deploy, redeploy-pr-env)'
    },
    @{
        name        = 'techhub-environment-production'
        subject     = "repo:$GitHubRepo`:environment:production"
        description = 'Production environment deployments (deploy-production)'
    },
    @{
        name        = 'techhub-ref-main'
        subject     = "repo:$GitHubRepo`:ref:refs/heads/main"
        description = 'CD pipeline jobs on main branch without a GitHub environment (deploy-shared-infra, build-and-push, teardown-all-pr-envs)'
    },
    @{
        name        = 'techhub-pull-request'
        subject     = "repo:$GitHubRepo`:pull_request"
        description = 'PR preview jobs without a GitHub environment assignment, including Dependabot PRs (pr-preview-build-and-push, pr-preview-teardown)'
    }
)

# Fetch existing credentials once to avoid redundant API calls
$existingOutput = az ad app federated-credential list --id $AppId -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "  [FAIL] Failed to list federated credentials." -ForegroundColor Red
    Write-Host $existingOutput -ForegroundColor Red
    exit 1
}
$existingCredentials = $existingOutput | ConvertFrom-Json
$existingNames = @($existingCredentials | ForEach-Object { $_.name })

$tmpFile = $null
try {
    $tmpFile = New-TemporaryFile

    foreach ($cred in $credentials) {
        if ($existingNames -contains $cred.name) {
            Write-Host "  [SKIP] $($cred.name) — already exists" -ForegroundColor Gray
            continue
        }

        $params = [ordered]@{
            name        = $cred.name
            issuer      = 'https://token.actions.githubusercontent.com'
            subject     = $cred.subject
            description = $cred.description
            audiences   = @('api://AzureADTokenExchange')
        }
        $params | ConvertTo-Json | Set-Content -Path $tmpFile.FullName -Encoding UTF8

        $createOutput = az ad app federated-credential create `
            --id $AppId `
            --parameters $tmpFile.FullName `
            -o none 2>&1

        if ($LASTEXITCODE -ne 0) {
            Write-Host "  [FAIL] Failed to create '$($cred.name)'" -ForegroundColor Red
            Write-Host $createOutput -ForegroundColor Red
            exit 1
        }

        Write-Host "  [OK]   $($cred.name)" -ForegroundColor Green
        Write-Host "         Subject: $($cred.subject)" -ForegroundColor Gray
    }
}
finally {
    if ($null -ne $tmpFile -and (Test-Path $tmpFile.FullName)) {
        Remove-Item $tmpFile.FullName -Force
    }
}

# ============================================================================
# STEP 3: Set GitHub repository variables
# ============================================================================

Write-Host ""
Write-Host "Setting GitHub repository variables..." -ForegroundColor Cyan

$variables = @{
    AZURE_CLIENT_ID       = $AppId
    AZURE_TENANT_ID       = $tenantId
    AZURE_SUBSCRIPTION_ID = $subscriptionId
}

foreach ($varName in $variables.Keys) {
    $varValue = $variables[$varName]
    $setOutput = gh variable set $varName `
        --body $varValue `
        --repo $GitHubRepo 2>&1

    if ($LASTEXITCODE -ne 0) {
        Write-Host "  [FAIL] Failed to set $varName" -ForegroundColor Red
        Write-Host $setOutput -ForegroundColor Red
        exit 1
    }

    Write-Host "  [OK] $varName = $varValue" -ForegroundColor Green
}

# ============================================================================
# DONE
# ============================================================================

Write-Host ""
Write-Host "OIDC setup complete." -ForegroundColor Green
Write-Host ""
Write-Host "OIDC is the only authentication method — no AZURE_CREDENTIALS secret is needed." -ForegroundColor Green
Write-Host ""
