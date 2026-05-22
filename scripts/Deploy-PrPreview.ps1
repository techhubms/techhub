#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys or tears down a fully ephemeral PR preview environment.

.DESCRIPTION
    Creates/updates or deletes a fully isolated PR preview environment in the production Azure
    Container Apps Environment. Each PR gets its own:
    - PostgreSQL Flexible Server (created via PITR from production)
    - Container Apps (ca-techhub-api-pr-{number} and ca-techhub-web-pr-{number})

    The PR apps share the production Container Apps Environment and VNet,
    but get their own isolated database. Application telemetry export is disabled
    for PR previews to avoid polluting production monitoring data.

    On deploy, a PR-specific Postgres instance is provisioned using Azure Point-in-Time
    Restore (PITR) from the production server. This creates an independent copy with
    realistic production data. PostgreSQL is accessible over the public internet with
    firewall rules for admin IPs and the Container Apps subnet.

    On teardown, the PR-specific Postgres instance is deleted along with the Container Apps.

.PARAMETER PrNumber
    Pull request number. Used to derive unique resource names.

.PARAMETER Action
    Action to perform: 'deploy' to create/update the PR environment, 'teardown' to remove it.

.PARAMETER Tag
    Docker image tag to deploy. Required for 'deploy' action.

.PARAMETER GithubRegistryUsername
    GitHub organization username for ghcr.io. Defaults to 'techhubms'.

.PARAMETER GithubRegistryAuthUsername
    GitHub username of the PAT owner used to authenticate with ghcr.io. Must match the account
    that owns the GHCR_PAT secret configured in GitHub. Defaults to the value of
    GithubRegistryUsername.

.EXAMPLE
    ./scripts/Deploy-PrPreview.ps1 -PrNumber 42 -Action deploy -Tag "pr-42-20250101120000"
    Deploy PR #42 preview environment.

.EXAMPLE
    ./scripts/Deploy-PrPreview.ps1 -PrNumber 42 -Action teardown
    Remove the PR #42 preview environment.
#>

param(
    [Parameter(Mandatory = $true)]
    [int]$PrNumber,

    [Parameter(Mandatory = $true)]
    [ValidateSet('deploy', 'teardown')]
    [string]$Action,

    [Parameter(Mandatory = $false)]
    [string]$Tag,

    [Parameter(Mandatory = $false)]
    [string]$GithubRegistryUsername = 'techhubms',

    [Parameter(Mandatory = $false)]
    [string]$GithubRegistryAuthUsername = $GithubRegistryUsername
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# CONFIGURATION
# ============================================================================

# Production resource group (PR previews run here alongside production)
$prodRG = 'rg-techhub-prod'
$prodEnvName = 'cae-techhub-prod'
$prodIdentityName = 'id-techhub-prod'

# Production server (source for PITR database clone)
$prodPostgresServer = 'psql-techhub-prod'

# NAT Gateway public IP resource name — outbound traffic from VNet-integrated Container Apps
# routes through the NAT Gateway on the Container Apps subnet, NOT through the CAE staticIp
# (which is the inbound LB frontend). Name follows infra/modules/network.bicep:
# replace(vnetName, 'vnet-', 'pip-nat-').
$natGatewayPublicIpName = 'pip-nat-techhub-prod'

# PR-specific resource names
$prPostgresServer = "psql-techhub-pr-$PrNumber"
$prPostgresDb = 'techhub'
# Shared managed identity — created once by infrastructure.bicep, reused by all PR environments.
$prManagedIdentityName = 'id-techhub-pr'

# PR-specific Container App names
$apiAppName = "ca-techhub-api-pr-$PrNumber"
$webAppName = "ca-techhub-web-pr-$PrNumber"

# ============================================================================
# HELPERS
# ============================================================================

function Write-Step {
    param([string]$Message)
    Write-Host ""
    Write-Host "=> $Message" -ForegroundColor Cyan
}

function Write-Ok {
    param([string]$Message)
    Write-Host "   [OK] $Message" -ForegroundColor Green
}

function Write-Warn {
    param([string]$Message)
    Write-Host "   [WARN] $Message" -ForegroundColor Yellow
}

function Write-Fail {
    param([string]$Message)
    Write-Host "   [FAIL] $Message" -ForegroundColor Red
}

function Write-Detail {
    param([string]$Message)
    Write-Host "   $Message" -ForegroundColor Gray
}

function Get-ContainerAppExists {
    param([string]$Name, [string]$ResourceGroup)
    $result = az containerapp list --resource-group $ResourceGroup --query "[?name=='$Name'].name | [0]" -o tsv 2>$null
    return ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($result))
}

function Get-PostgresServerExists {
    param([string]$Name, [string]$ResourceGroup)
    $result = az postgres flexible-server list --resource-group $ResourceGroup --query "[?name=='$Name'].name | [0]" -o tsv 2>$null
    return ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($result))
}

function Write-ContainerAppDiagnostics {
    <#
    .SYNOPSIS
        Dumps the latest revision status and system logs for a Container App.
        Used when warmup fails to help triage ActivationFailed / crash-loop / image-pull errors.
    #>
    param([string]$AppName, [string]$ResourceGroup)

    try {
        $latestRevision = az containerapp revision list `
            -n $AppName -g $ResourceGroup `
            --query "sort_by([], &properties.createdTime) | [-1]" -o json 2>$null |
            ConvertFrom-Json -ErrorAction SilentlyContinue
        if ($latestRevision) {
            Write-Host ""
            Write-Host "Latest revision status for ${AppName}:" -ForegroundColor Yellow
            Write-Host "  Name            : $($latestRevision.name)" -ForegroundColor Gray
            Write-Host "  Image           : $($latestRevision.properties.template.containers[0].image)" -ForegroundColor Gray
            Write-Host "  Replicas        : $($latestRevision.properties.replicas)" -ForegroundColor Gray
            Write-Host "  HealthState     : $($latestRevision.properties.healthState)" -ForegroundColor Gray
            Write-Host "  RunningState    : $($latestRevision.properties.runningState)" -ForegroundColor Gray
            if ($latestRevision.properties.PSObject.Properties['runningStateDetails']) {
                Write-Host "  Details         : $($latestRevision.properties.runningStateDetails)" -ForegroundColor Gray
            }
        }
    }
    catch {
        Write-Detail "Could not fetch revision status for ${AppName}: $_"
    }

    try {
        Write-Host ""
        Write-Host "Recent system events for ${AppName}:" -ForegroundColor Yellow
        az containerapp logs show -n $AppName -g $ResourceGroup --type system --tail 30 2>&1 |
            ForEach-Object { Write-Host $_ }
    }
    catch {
        Write-Detail "Could not fetch system logs for ${AppName}: $_"
    }
}

# ============================================================================
# BANNER
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  TechHub PR Preview Environment" -ForegroundColor White
Write-Host "  PR Number   : #$PrNumber" -ForegroundColor Gray
Write-Host "  Action      : $Action" -ForegroundColor Gray
if ($Tag) {
    Write-Host "  Tag         : $Tag" -ForegroundColor Gray
}
Write-Host "  API App     : $apiAppName" -ForegroundColor Gray
Write-Host "  Web App     : $webAppName" -ForegroundColor Gray
Write-Host "  PostgreSQL  : $prPostgresServer" -ForegroundColor Gray
Write-Host "  Resource RG : $prodRG" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PREREQUISITES
# ============================================================================

Write-Step "Validating prerequisites"

# Check Azure CLI login
$account = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
    exit 1
}
$accountInfo = $account | ConvertFrom-Json
Write-Ok "Azure CLI authenticated (subscription: $($accountInfo.name))"

if ($Action -eq 'deploy') {
    if (-not $Tag) {
        Write-Fail "Tag is required for the deploy action."
        exit 1
    }

    if (-not $env:ADMIN_IP_ADDRESSES) {
        Write-Fail "Environment variable ADMIN_IP_ADDRESSES is not set. Required for PR PostgreSQL firewall rules."
        exit 1
    }
    Write-Ok "ADMIN_IP_ADDRESSES is set"
}

# ============================================================================
# TEARDOWN
# ============================================================================

if ($Action -eq 'teardown') {
    Write-Step "Removing PR preview environment for PR #$PrNumber"

    $deletedAny = $false

    # Delete Web Container App first (it calls the API)
    if (Get-ContainerAppExists -Name $webAppName -ResourceGroup $prodRG) {
        Write-Detail "Deleting $webAppName..."
        az containerapp delete `
            --name $webAppName `
            --resource-group $prodRG `
            --yes
        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to delete $webAppName"
            exit 1
        }
        Write-Ok "Deleted $webAppName"
        $deletedAny = $true
    }
    else {
        Write-Warn "$webAppName not found — already removed or never deployed"
    }

    # Delete API Container App
    if (Get-ContainerAppExists -Name $apiAppName -ResourceGroup $prodRG) {
        Write-Detail "Deleting $apiAppName..."
        az containerapp delete `
            --name $apiAppName `
            --resource-group $prodRG `
            --yes
        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to delete $apiAppName"
            exit 1
        }
        Write-Ok "Deleted $apiAppName"
        $deletedAny = $true
    }
    else {
        Write-Warn "$apiAppName not found — already removed or never deployed"
    }

    # Delete PR-specific PostgreSQL server
    Write-Step "Deleting PR PostgreSQL server: $prPostgresServer"

    if (Get-PostgresServerExists -Name $prPostgresServer -ResourceGroup $prodRG) {
        Write-Detail "Deleting $prPostgresServer (this may take a few minutes)..."
        az postgres flexible-server delete `
            --name $prPostgresServer `
            --resource-group $prodRG `
            --yes
        if ($LASTEXITCODE -ne 0) {
            Write-Warn "Failed to delete $prPostgresServer — continuing"
        }
        else {
            Write-Ok "Deleted $prPostgresServer"
            $deletedAny = $true
        }
    }
    else {
        Write-Warn "$prPostgresServer not found — already removed or never deployed"
    }

    if ($deletedAny) {
        Write-Ok "PR preview environment for PR #$PrNumber removed successfully"
    }
    else {
        Write-Ok "Nothing to remove for PR #$PrNumber"
    }

    exit 0
}

# ============================================================================
# DEPLOY — Provision PR-specific PostgreSQL via PITR from production
# ============================================================================

Write-Step "Provisioning PR PostgreSQL server: $prPostgresServer"

$pgExists = Get-PostgresServerExists -Name $prPostgresServer -ResourceGroup $prodRG

if ($pgExists) {
    Write-Ok "PR PostgreSQL server already exists — reusing $prPostgresServer"
}
else {
    # Get the production server resource ID for PITR source
    $prodServerId = az postgres flexible-server show `
        --name $prodPostgresServer `
        --resource-group $prodRG `
        --query id -o tsv 2>$null

    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($prodServerId)) {
        Write-Fail "Could not find production PostgreSQL server '$prodPostgresServer' in '$prodRG'"
        exit 1
    }
    Write-Ok "Production server ID: $prodServerId"

    # Use a restore time of 03:00 UTC today for a stable backup point.
    $nowUtc = [DateTime]::UtcNow
    $restoreDate = if ($nowUtc.Hour -lt 4) { $nowUtc.AddDays(-1).Date.AddHours(3) } else { $nowUtc.Date.AddHours(3) }
    $restoreTime = $restoreDate.ToString('yyyy-MM-ddTHH:mm:ssZ')
    Write-Detail "PITR restore time: $restoreTime"

    Write-Detail "Creating $prPostgresServer via PITR from production (this takes 5-8 minutes)..."
    az postgres flexible-server restore `
        --resource-group $prodRG `
        --name $prPostgresServer `
        --source-server $prodServerId `
        --restore-time $restoreTime
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to create PR PostgreSQL server via PITR"
        exit 1
    }
    Write-Ok "PR PostgreSQL server created: $prPostgresServer"
}

# ============================================================================
# CONFIGURE ENTRA AUTH ON PITR SERVER USING SHARED PR IDENTITY
# ============================================================================

Write-Step "Configuring Entra auth on $prPostgresServer"

# Resolve the shared PR identity's principal ID (created once by infrastructure.bicep)
$prIdentityPrincipalId = az identity show `
    --name $prManagedIdentityName `
    --resource-group $prodRG `
    --query principalId -o tsv 2>$null
if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($prIdentityPrincipalId)) {
    Write-Fail "Shared PR managed identity '$prManagedIdentityName' not found in '$prodRG'. Ensure infrastructure.bicep has been deployed."
    exit 1
}
Write-Ok "Shared PR managed identity: $prManagedIdentityName (principal: $prIdentityPrincipalId)"

# Enable Entra-only authentication on the PR PostgreSQL server so no shared password is needed.
# The PITR-restored server inherits production's password auth; switch it to Entra auth.
Write-Detail "Enabling Entra ID authentication on $prPostgresServer..."
az postgres flexible-server update `
    --resource-group $prodRG `
    --name $prPostgresServer `
    --microsoft-entra-auth Enabled `
    --password-auth Disabled
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to enable Entra auth on $prPostgresServer"
    exit 1
}
Write-Ok "Entra auth enabled on $prPostgresServer (password auth disabled)"

# Register the shared PR managed identity as Entra admin on the PITR server.
# No propagation delay needed — the identity already exists in Entra ID.
Write-Detail "Setting PR managed identity as Entra admin on $prPostgresServer..."

# Check if the admin is already registered — redeploys for the same PR should be idempotent.
# List all admins and look for one with our principal ID to avoid a double-create failure.
$existingAdminsJson = az postgres flexible-server microsoft-entra-admin list `
    --server-name $prPostgresServer `
    --resource-group $prodRG `
    --output json 2>$null
$existingAdmins = @()
if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($existingAdminsJson) -and $existingAdminsJson -ne 'null') {
    $existingAdmins = @($existingAdminsJson | ConvertFrom-Json)
}

$adminAlreadyRegistered = $LASTEXITCODE -eq 0 -and
    $existingAdmins -and
    (@($existingAdmins | Where-Object { $_.objectId -eq $prIdentityPrincipalId }).Count -gt 0)

if ($adminAlreadyRegistered) {
    Write-Ok "Entra admin already registered on $prPostgresServer (reusing)"
}
else {
    az postgres flexible-server microsoft-entra-admin create `
        --server-name $prPostgresServer `
        --resource-group $prodRG `
        --display-name $prManagedIdentityName `
        --object-id $prIdentityPrincipalId `
        --type ServicePrincipal
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to set Entra admin on $prPostgresServer"
        exit 1
    }
    Write-Ok "PR managed identity registered as Entra admin on $prPostgresServer"
}

# Add firewall rules for the PR PostgreSQL server.
# Admin IPs: allow explicit admin access.
# NAT Gateway IP: all outbound connections from VNet-integrated Container Apps egress through
# the NAT Gateway attached to the Container Apps subnet — this is the SNAT IP seen by PostgreSQL.
Write-Step "Configuring PostgreSQL firewall rules for $prPostgresServer"

# Fetch the NAT Gateway's stable public IP.
# Do NOT use properties.staticIp from the Container Apps Environment — that is the INBOUND
# load balancer frontend IP, not the outbound SNAT IP for pods.
$natGatewayIp = az network public-ip show `
    --name $natGatewayPublicIpName `
    --resource-group $prodRG `
    --query properties.ipAddress -o tsv 2>$null
if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($natGatewayIp)) {
    Write-Fail "Could not retrieve NAT Gateway public IP from '$natGatewayPublicIpName' in '$prodRG'"
    exit 1
}
Write-Ok "NAT Gateway outbound IP: $natGatewayIp"

# Parse admin IPs from env var
$adminIps = @($env:ADMIN_IP_ADDRESSES -split ',' | ForEach-Object { $_.Trim() } | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })

# Reconcile admin IP firewall rules: remove any stale allow-admin-ip-* rules from a previous
# deploy so that IPs removed from ADMIN_IP_ADDRESSES do not remain permitted indefinitely.
$existingAdminRuleNames = az postgres flexible-server firewall-rule list `
    --resource-group $prodRG `
    --name $prPostgresServer `
    --query "[?starts_with(name, 'allow-admin-ip-')].name" `
    --output tsv 2>$null
if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($existingAdminRuleNames)) {
    foreach ($existingRuleName in ($existingAdminRuleNames -split "`n" | ForEach-Object { $_.Trim() } | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })) {
        az postgres flexible-server firewall-rule delete `
            --resource-group $prodRG `
            --name $prPostgresServer `
            --rule-name $existingRuleName `
            --yes 2>$null | Out-Null
        Write-Detail "Removed stale admin IP firewall rule: $existingRuleName"
    }
}

$ruleIndex = 0
foreach ($ip in $adminIps) {
    Write-Detail "Adding admin IP firewall rule for $ip..."
    az postgres flexible-server firewall-rule create `
        --resource-group $prodRG `
        --name $prPostgresServer `
        --rule-name "allow-admin-ip-$ruleIndex" `
        --start-ip-address $ip `
        --end-ip-address $ip 2>$null | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-Ok "Added admin IP firewall rule: $ip"
    }
    else {
        Write-Warn "Failed to add admin IP rule for $ip — continuing"
    }
    $ruleIndex++
}

# NAT Gateway firewall rule (idempotent)
$natGatewayRuleName = 'allow-nat-gateway'

# Clean up stale rules from previous Bicep iterations (incremental deployments leave orphans).
$staleRuleNames = @('allow-container-apps-subnet', 'allow-container-apps-static-ip')
foreach ($staleRule in $staleRuleNames) {
    $staleRuleJson = az postgres flexible-server firewall-rule list `
        --resource-group $prodRG `
        --name $prPostgresServer `
        --query "[?name=='$staleRule'] | [0]" `
        --output json 2>$null
    if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($staleRuleJson) -and $staleRuleJson -ne 'null') {
        Write-Detail "Removing stale firewall rule: $staleRule"
        az postgres flexible-server firewall-rule delete `
            --resource-group $prodRG `
            --name $prPostgresServer `
            --rule-name $staleRule `
            --yes 2>$null | Out-Null
    }
}

$existingNatRuleJson = az postgres flexible-server firewall-rule list `
    --resource-group $prodRG `
    --name $prPostgresServer `
    --query "[?name=='$natGatewayRuleName'] | [0]" `
    --output json 2>$null
$existingNatRule = $null
if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($existingNatRuleJson) -and $existingNatRuleJson -ne 'null') {
    $existingNatRule = $existingNatRuleJson | ConvertFrom-Json
}

if ($existingNatRule -and
    $existingNatRule.startIpAddress -eq $natGatewayIp -and
    $existingNatRule.endIpAddress -eq $natGatewayIp) {
    Write-Ok "NAT Gateway firewall rule already configured"
}
else {
    if ($existingNatRule) {
        Write-Detail "Replacing stale NAT Gateway firewall rule..."
        az postgres flexible-server firewall-rule delete `
            --resource-group $prodRG `
            --name $prPostgresServer `
            --rule-name $natGatewayRuleName `
            --yes 2>$null | Out-Null
        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to remove existing NAT Gateway firewall rule"
            exit 1
        }
    }

    Write-Detail "Adding NAT Gateway firewall rule ($natGatewayIp)..."
    az postgres flexible-server firewall-rule create `
        --resource-group $prodRG `
        --name $prPostgresServer `
        --rule-name $natGatewayRuleName `
        --start-ip-address $natGatewayIp `
        --end-ip-address $natGatewayIp
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to add NAT Gateway firewall rule"
        exit 1
    }
    Write-Ok "NAT Gateway firewall rule added"
}

# ============================================================================
# DEPLOY — Container Apps via Bicep
# ============================================================================

Write-Step "Deploying Container Apps via Bicep"

# Get the Container Apps Environment default domain for GITHUB_OUTPUT.
$envDefaultDomain = az containerapp env show `
    --name $prodEnvName `
    --resource-group $prodRG `
    --query properties.defaultDomain -o tsv 2>$null

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($envDefaultDomain)) {
    Write-Fail "Could not retrieve Container Apps Environment default domain"
    exit 1
}
Write-Ok "Environment default domain: $envDefaultDomain"

$templateFile = Join-Path $PSScriptRoot '../infra/pr-applications.bicep'
$deploymentOutput = az deployment group create `
    --resource-group $prodRG `
    --template-file $templateFile `
    --parameters `
        prNumber=$PrNumber `
        imageTag=$Tag `
        githubRegistryUsername=$GithubRegistryUsername `
        githubRegistryAuthUsername=$GithubRegistryAuthUsername `
    --output json

if ($LASTEXITCODE -ne 0) {
    Write-Fail "Bicep deployment failed (exit code $LASTEXITCODE)"
    Write-Host $deploymentOutput
    exit 1
}

$deploymentResult = $deploymentOutput | ConvertFrom-Json
$webFqdn = $deploymentResult.properties.outputs.webFqdn.value

if ([string]::IsNullOrWhiteSpace($webFqdn)) {
    Write-Fail "Could not read webFqdn from deployment output"
    exit 1
}

Write-Ok "API Container App : $apiAppName"
Write-Ok "Web Container App : $webAppName"
Write-Ok "Web FQDN          : $webFqdn"

# Write output for GitHub Actions
if ($env:GITHUB_OUTPUT) {
    "cae-default-domain=$envDefaultDomain" | Out-File -Append -FilePath $env:GITHUB_OUTPUT
    Write-Ok "Written cae-default-domain to GITHUB_OUTPUT"
}

# ============================================================================
# VERSION WAIT + SMOKE TESTS
# ============================================================================

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
& (Join-Path $scriptDir 'Wait-ForLiveVersion.ps1') -WebFqdn $webFqdn -Tag $Tag
if ($LASTEXITCODE -ne 0) {
    Write-ContainerAppDiagnostics -AppName $webAppName -ResourceGroup $prodRG
    Write-ContainerAppDiagnostics -AppName $apiAppName -ResourceGroup $prodRG
    exit 1
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  PR Preview Environment Ready" -ForegroundColor Green
Write-Host "  PR Number   : #$PrNumber" -ForegroundColor Gray
Write-Host "  Tag         : $Tag" -ForegroundColor Gray
Write-Host "  API App     : $apiAppName" -ForegroundColor Gray
Write-Host "  Web App     : $webAppName" -ForegroundColor Gray
Write-Host "  PostgreSQL  : $prPostgresServer" -ForegroundColor Gray
Write-Host "  Web URL     : https://$webFqdn" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""

# Exit explicitly to prevent false-negative GitHub Actions failures on successful deploys.
exit 0
