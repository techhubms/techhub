#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys or tears down a fully ephemeral PR preview environment.

.DESCRIPTION
    Creates/updates or deletes a fully isolated PR preview environment in the production Azure
    Container Apps Environment. Each PR gets its own:
    - PostgreSQL Flexible Server (created via PITR from production)
    - Container Apps (ca-techhub-api-pr-{number} and ca-techhub-web-pr-{number})

    The PR apps share the production Container Apps Environment, VNet, and monitoring —
    but get their own isolated database.

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
    that owns the 'techhub-github-registry-token' PAT stored in Key Vault. Defaults to GithubRegistryUsername.

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
$prodAppInsightsName = 'appi-techhub-prod'
$prodKeyVaultName = 'kv-techhub-prod'

# Production server (source for PITR database clone)
$prodPostgresServer = 'psql-techhub-prod'

# Container Apps subnet range (used for PostgreSQL firewall rule).
# These must match the containerAppsSubnetStartIp/EndIp values in infra/parameters/prod.bicepparam.
# Update both places if the Container Apps subnet CIDR ever changes.
$containerAppsSubnetStartIp = '10.2.0.0'
$containerAppsSubnetEndIp = '10.2.1.255'

# PR-specific resource names
$prPostgresServer = "psql-techhub-pr-$PrNumber"
$prPostgresDb = 'techhub'
$prManagedIdentityName = "id-techhub-pr-$PrNumber"

# Seconds to wait after creating the per-PR managed identity for its service principal
# to propagate through Entra ID before registering it as a PostgreSQL Entra admin.
$entraIdPropagationDelaySecs = 15

# PR-specific Container App names
$apiAppName = "ca-techhub-api-pr-$PrNumber"
$webAppName = "ca-techhub-web-pr-$PrNumber"

$registryServer = "ghcr.io"
$apiImage = "$registryServer/$GithubRegistryUsername/techhub-api:$Tag"
$webImage = "$registryServer/$GithubRegistryUsername/techhub-web:$Tag"

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

function Set-KeyVaultSecretFromValue {
    param(
        [Parameter(Mandatory = $true)][string]$VaultName,
        [Parameter(Mandatory = $true)][string]$SecretName,
        [Parameter(Mandatory = $true)][string]$Value
    )

    $tmpFile = [System.IO.Path]::GetTempFileName()
    try {
        [System.IO.File]::WriteAllText($tmpFile, $Value)
        az keyvault secret set `
            --vault-name $VaultName `
            --name $SecretName `
            --file $tmpFile `
            --output none
        return $LASTEXITCODE -eq 0
    }
    finally {
        Remove-Item $tmpFile -Force -ErrorAction SilentlyContinue
    }
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

function Set-WebStartupProbe {
    <#
    .SYNOPSIS
        Configures a startup probe on the Web Container App via ARM REST PATCH.

        The Web's Program.cs blocks Kestrel startup while pre-loading the section cache
        from the API. Without an explicit startup probe, Container Apps injects a default
        TCP liveness probe (failureThreshold=3, periodSeconds=10) that kills the container
        after ~30s — before the API cold-starts (~15-30s) and responds.

        Defining a startup probe also disables the auto-injected TCP liveness probe for the
        duration of startup. This allows both apps to stay at minReplicas=0 without the Web
        being killed while it waits for the API to scale from zero.

        Probes are configured via az rest PATCH (partial ARM update) because az containerapp
        create/update have no individual probe CLI flags — only --yaml, which requires the
        full container spec and YAML serialisation tooling.

        ACA probe limits: failureThreshold 1-10, periodSeconds 1-240, initialDelaySeconds 1-60.
        initialDelaySeconds=5 + failureThreshold=10 × periodSeconds=20 → 205s ≈ 3.4 min.
    #>
    param([string]$AppName, [string]$ResourceGroup)

    $webApp = az containerapp show --name $AppName --resource-group $ResourceGroup -o json 2>$null |
        ConvertFrom-Json -ErrorAction SilentlyContinue
    if (-not $webApp) {
        Write-Fail "Could not retrieve Web Container App spec — cannot configure startup probe"
        exit 1
    }

    $subId = az account show --query id -o tsv 2>$null
    $container = $webApp.properties.template.containers[0]

    # Preserve all existing non-Startup probes, then add ours.
    $existingProbes = @()
    if ($container.PSObject.Properties['probes'] -ne $null) {
        $existingProbes = @($container.probes | Where-Object { $_.type -ne 'Startup' })
    }

    $startupProbe = [PSCustomObject]@{
        type                = 'Startup'
        httpGet             = [PSCustomObject]@{ path = '/alive'; port = 8080 }
        initialDelaySeconds = 5
        periodSeconds       = 20
        failureThreshold    = 10   # ACA max is 10; 5 + 10×20 = 205s ≈ 3.4-min tolerance
    }

    $container | Add-Member -NotePropertyName 'probes' -NotePropertyValue (@($existingProbes) + @($startupProbe)) -Force

    $patchBody = [PSCustomObject]@{
        properties = [PSCustomObject]@{
            template = [PSCustomObject]@{
                containers = @($container)
            }
        }
    } | ConvertTo-Json -Depth 20 -Compress

    $patchFile = Join-Path ([System.IO.Path]::GetTempPath()) "web-probe-patch-$AppName.json"
    $patchBody | Set-Content $patchFile -Encoding utf8

    $apiUrl = "https://management.azure.com/subscriptions/$subId/resourceGroups/$ResourceGroup" +
              "/providers/Microsoft.App/containerApps/$($AppName)?api-version=2024-03-01"
    az rest --method PATCH --url $apiUrl --body "@$patchFile" --output none

    Remove-Item $patchFile -ErrorAction SilentlyContinue

    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Startup probe REST PATCH failed (exit code $LASTEXITCODE)"
        exit 1
    }

    Write-Ok "Startup probe configured (initialDelaySeconds=5, failureThreshold=10, periodSeconds=20 → ~3.4 min tolerance)"
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

    # Delete the per-PR managed identity
    $prIdentityExists = az identity list `
        --resource-group $prodRG `
        --query "[?name=='$prManagedIdentityName'].id | [0]" `
        --output tsv 2>$null
    if (-not [string]::IsNullOrWhiteSpace($prIdentityExists)) {
        Write-Detail "Deleting per-PR managed identity $prManagedIdentityName..."
        az identity delete `
            --name $prManagedIdentityName `
            --resource-group $prodRG
        if ($LASTEXITCODE -ne 0) {
            Write-Warn "Failed to delete managed identity $prManagedIdentityName — continuing"
        }
        else {
            Write-Ok "Deleted managed identity $prManagedIdentityName"
            $deletedAny = $true
        }
    }
    else {
        Write-Warn "$prManagedIdentityName not found — already removed or never created"
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
# CREATE PER-PR MANAGED IDENTITY AND CONFIGURE ENTRA AUTH ON PITR SERVER
# ============================================================================

Write-Step "Provisioning per-PR managed identity: $prManagedIdentityName"

# Check whether the identity already exists — redeploys for the same PR should be idempotent.
$prIdentityJsonText = az identity list `
    --resource-group $prodRG `
    --query "[?name=='$prManagedIdentityName'] | [0]" `
    --output json 2>$null
$prIdentityJson = $null
if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($prIdentityJsonText) -and $prIdentityJsonText -ne 'null') {
    $prIdentityJson = $prIdentityJsonText | ConvertFrom-Json
}

if ($LASTEXITCODE -eq 0 -and $null -ne $prIdentityJson -and $prIdentityJson.id) {
    Write-Ok "Managed identity already exists: $prManagedIdentityName (reusing)"
}
else {
    $prIdentityJson = az identity create `
        --name $prManagedIdentityName `
        --resource-group $prodRG `
        --output json | ConvertFrom-Json
    if ($LASTEXITCODE -ne 0 -or $null -eq $prIdentityJson) {
        Write-Fail "Failed to create managed identity '$prManagedIdentityName'"
        exit 1
    }
    Write-Ok "Managed identity created: $prManagedIdentityName"
}
$prIdentityId = $prIdentityJson.id
$prIdentityPrincipalId = $prIdentityJson.principalId
Write-Ok "Managed identity ready: $prManagedIdentityName (principal: $prIdentityPrincipalId)"

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

# Register the PR managed identity as an Entra administrator on the PITR server.
# This grants the identity the azure_pg_admin role needed to connect and run migrations.
# Azure requires a short propagation delay before the identity principal is resolvable.
Write-Detail "Waiting for managed identity to propagate in Entra ID..."
Start-Sleep -Seconds $entraIdPropagationDelaySecs
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
# Container Apps subnet: allow all Container Apps in the prod environment to reach the PR database.
Write-Step "Configuring PostgreSQL firewall rules for $prPostgresServer"

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

# Container Apps subnet rule (idempotent)
$containerAppsSubnetRuleName = 'allow-container-apps-subnet'
$existingContainerAppsSubnetRuleJson = az postgres flexible-server firewall-rule list `
    --resource-group $prodRG `
    --name $prPostgresServer `
    --query "[?name=='$containerAppsSubnetRuleName'] | [0]" `
    --output json 2>$null
$existingContainerAppsSubnetRule = $null
if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($existingContainerAppsSubnetRuleJson) -and $existingContainerAppsSubnetRuleJson -ne 'null') {
    $existingContainerAppsSubnetRule = $existingContainerAppsSubnetRuleJson | ConvertFrom-Json
}

if ($existingContainerAppsSubnetRule -and
    $existingContainerAppsSubnetRule.startIpAddress -eq $containerAppsSubnetStartIp -and
    $existingContainerAppsSubnetRule.endIpAddress -eq $containerAppsSubnetEndIp) {
    Write-Ok "Container Apps subnet firewall rule already configured"
}
else {
    if ($existingContainerAppsSubnetRule) {
        Write-Detail "Replacing stale Container Apps subnet firewall rule..."
        az postgres flexible-server firewall-rule delete `
            --resource-group $prodRG `
            --name $prPostgresServer `
            --rule-name $containerAppsSubnetRuleName `
            --yes 2>$null | Out-Null
        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to remove existing Container Apps subnet firewall rule"
            exit 1
        }
    }

    Write-Detail "Adding Container Apps subnet firewall rule ($containerAppsSubnetStartIp - $containerAppsSubnetEndIp)..."
    az postgres flexible-server firewall-rule create `
        --resource-group $prodRG `
        --name $prPostgresServer `
        --rule-name $containerAppsSubnetRuleName `
        --start-ip-address $containerAppsSubnetStartIp `
        --end-ip-address $containerAppsSubnetEndIp
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to add Container Apps subnet firewall rule"
        exit 1
    }
    Write-Ok "Container Apps subnet firewall rule added"
}

# ============================================================================
# DEPLOY — Query production infrastructure
# ============================================================================

Write-Step "Querying production infrastructure"

# Get Container Apps Environment ID
$envId = az containerapp env show `
    --name $prodEnvName `
    --resource-group $prodRG `
    --query id -o tsv 2>$null

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($envId)) {
    Write-Fail "Could not find production Container Apps Environment '$prodEnvName' in '$prodRG'"
    exit 1
}
Write-Ok "Container Apps Environment: $envId"

# Get Application Insights connection string
$appInsightsConnString = az monitor app-insights component show `
    --app $prodAppInsightsName `
    --resource-group $prodRG `
    --query connectionString -o tsv 2>$null

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($appInsightsConnString)) {
    Write-Warn "Could not retrieve AppInsights connection string — proceeding without telemetry"
    $appInsightsConnString = ""
}
else {
    Write-Ok "Application Insights: retrieved connection string"
}

# Resolve GitHub registry token for Container Apps image pulls.
# Preferred source is Key Vault secret; fallback supports first-run bootstrap before that secret exists.
Write-Detail "Reading GitHub registry token from Key Vault..."
$ghcrToken = az keyvault secret show `
    --vault-name $prodKeyVaultName `
    --name 'techhub-github-registry-token' `
    --query value -o tsv 2>$null
if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($ghcrToken)) {
    $allowGhcrPatFallback = -not [string]::IsNullOrWhiteSpace($env:ALLOW_GHCR_PAT_FALLBACK) -and
        $env:ALLOW_GHCR_PAT_FALLBACK.Trim().ToLowerInvariant() -eq 'true'
    if ($allowGhcrPatFallback -and -not [string]::IsNullOrWhiteSpace($env:GHCR_PAT)) {
        $ghcrToken = $env:GHCR_PAT
        Write-Warn "Key Vault secret 'techhub-github-registry-token' not available; using GHCR_PAT fallback for this deploy."
        if (Set-KeyVaultSecretFromValue -VaultName $prodKeyVaultName -SecretName 'techhub-github-registry-token' -Value $ghcrToken) {
            Write-Ok "Key Vault secret 'techhub-github-registry-token' updated from GHCR_PAT fallback"
        }
        else {
            Write-Warn "Could not update Key Vault secret 'techhub-github-registry-token' from GHCR_PAT fallback"
        }
    }
    else {
        Write-Fail "Could not resolve GitHub registry token for ghcr.io image pulls."
        Write-Detail "Preferred: set Key Vault secret 'techhub-github-registry-token' in '$prodKeyVaultName'."
        Write-Detail "Bootstrap fallback: set ALLOW_GHCR_PAT_FALLBACK=true and GHCR_PAT (read:packages scope)."
        exit 1
    }
}
Write-Ok "GitHub registry token resolved"

# Build a passwordless PostgreSQL connection string.
# The username is the managed identity display name as registered in the Entra admin.
# The app fetches an Azure AD token at runtime via ManagedIdentityCredential.
$prPostgresFqdn = "$prPostgresServer.postgres.database.azure.com"
$dbConnectionString = "Host=$prPostgresFqdn;Database=$prPostgresDb;Username=$prManagedIdentityName;SSL Mode=Require"
Write-Ok "PostgreSQL: $prPostgresFqdn"

# Get the Container Apps Environment default domain (to compute expected FQDNs)
$envDefaultDomain = az containerapp env show `
    --name $prodEnvName `
    --resource-group $prodRG `
    --query properties.defaultDomain -o tsv 2>$null

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($envDefaultDomain)) {
    Write-Fail "Could not retrieve Container Apps Environment default domain"
    exit 1
}
Write-Ok "Environment default domain: $envDefaultDomain"

# Pre-compute expected FQDNs (used for CORS and ApiBaseUrl)
$apiPrFqdn = "$apiAppName.internal.$envDefaultDomain"
$webPrFqdn = "$webAppName.$envDefaultDomain"

Write-Detail "Expected API FQDN (internal): $apiPrFqdn"
Write-Detail "Expected Web FQDN:             $webPrFqdn"

# Shared environment variable lists (used in both create and update paths)
$apiEnvVars = @(
    "ASPNETCORE_ENVIRONMENT=Staging",
    "APPLICATIONINSIGHTS_CONNECTION_STRING=$appInsightsConnString",
    "OTEL_SERVICE_NAME=techhub-api",
    "Database__Provider=PostgreSQL",
    "Database__ConnectionString=secretref:db-connection-string",
    "Database__UseEntraAuth=true",
    "AppSettings__BaseUrl=https://$webPrFqdn",
    "TECHHUB_TMP=/tmp/techhub",
    "Cors__AllowedOrigins__0=https://$webPrFqdn",
    # Disable scheduled background jobs in PR environments.
    "ContentProcessor__Enabled=false",
    "RoundupGenerator__Enabled=false"
)

$webEnvVars = @(
    "ASPNETCORE_ENVIRONMENT=Staging",
    "APPLICATIONINSIGHTS_CONNECTION_STRING=$appInsightsConnString",
    "OTEL_SERVICE_NAME=techhub-web",
    "ApiBaseUrl=https://$apiPrFqdn",
    "TECHHUB_TMP=/tmp/techhub",
    "DEPLOY_IMAGE_TAG=$Tag"
)

# ============================================================================
# DEPLOY API CONTAINER APP
# ============================================================================

Write-Step "Deploying API Container App: $apiAppName"

$apiExists = Get-ContainerAppExists -Name $apiAppName -ResourceGroup $prodRG

if ($apiExists) {
    Write-Detail "Updating existing $apiAppName (image + env vars + secrets)..."

    az containerapp secret set `
        --name $apiAppName `
        --resource-group $prodRG `
        --secrets "db-connection-string=$dbConnectionString" 2>$null | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to update API Container App secrets"
        exit 1
    }

    az containerapp update `
        --name $apiAppName `
        --resource-group $prodRG `
        --image $apiImage `
        --min-replicas 0 `
        --max-replicas 1 `
        --replace-env-vars @apiEnvVars
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to update API Container App"
        exit 1
    }

    # Ensure this app only has the PR identity to avoid ambiguous DefaultAzureCredential selection.
    $apiIdentityStateJson = az containerapp identity show `
        --name $apiAppName `
        --resource-group $prodRG `
        --output json 2>$null
    if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($apiIdentityStateJson)) {
        $apiIdentityState = $apiIdentityStateJson | ConvertFrom-Json
        $assignedIdentityIds = @()
        if ($apiIdentityState.PSObject.Properties.Name -contains 'userAssignedIdentities' -and $apiIdentityState.userAssignedIdentities) {
            $assignedIdentityIds = @($apiIdentityState.userAssignedIdentities.PSObject.Properties.Name)
        }

        foreach ($assignedIdentityId in $assignedIdentityIds) {
            if ($assignedIdentityId -ne $prIdentityId) {
                Write-Detail "Removing stale managed identity from ${apiAppName}: $assignedIdentityId"
                az containerapp identity remove `
                    --name $apiAppName `
                    --resource-group $prodRG `
                    --user-assigned $assignedIdentityId | Out-Null
                if ($LASTEXITCODE -ne 0) {
                    Write-Fail "Failed to remove stale managed identity from $apiAppName"
                    exit 1
                }
            }
        }
    }

    # Ensure the PR identity is assigned (idempotent — safe to re-run)
    az containerapp identity assign `
        --name $apiAppName `
        --resource-group $prodRG `
        --user-assigned $prIdentityId | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to assign PR managed identity to $apiAppName"
        exit 1
    }
}
else {
    Write-Detail "Creating new $apiAppName..."

    az containerapp create `
        --name $apiAppName `
        --resource-group $prodRG `
        --environment $envId `
        --image $apiImage `
        --ingress internal `
        --target-port 8080 `
        --transport http `
        --registry-server $registryServer `
        --registry-username $GithubRegistryAuthUsername `
        --registry-password $ghcrToken `
        --user-assigned $prIdentityId `
        --cpu 0.5 `
        --memory 1Gi `
        --min-replicas 0 `
        --max-replicas 1 `
        --secrets "db-connection-string=$dbConnectionString" `
        --env-vars @apiEnvVars
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to create API Container App"
        exit 1
    }
}

Write-Ok "API Container App deployed: $apiAppName"

# Set the Container Apps-level ingress CORS policy
Write-Detail "Updating API ingress CORS policy..."
az containerapp ingress cors update `
    --name $apiAppName `
    --resource-group $prodRG `
    --allowed-origins "https://$webPrFqdn" "https://*.azurecontainerapps.io" `
    --allowed-methods GET POST PUT DELETE OPTIONS `
    --allowed-headers "*" `
    --allow-credentials false
if ($LASTEXITCODE -ne 0) {
    Write-Warn "CORS ingress update failed — web app may have cross-origin issues"
}
else {
    Write-Ok "CORS ingress policy updated"
}

# Retrieve the actual API FQDN (may differ from computed)
$actualApiPrFqdn = az containerapp show `
    --name $apiAppName `
    --resource-group $prodRG `
    --query properties.configuration.ingress.fqdn -o tsv 2>$null
if (-not [string]::IsNullOrWhiteSpace($actualApiPrFqdn)) {
    $apiPrFqdn = $actualApiPrFqdn
    $webEnvVars = $webEnvVars | ForEach-Object { $_ -replace "^ApiBaseUrl=.*", "ApiBaseUrl=https://$apiPrFqdn" }
    Write-Detail "Actual API FQDN: $apiPrFqdn"
}

# ============================================================================
# DEPLOY WEB CONTAINER APP
# ============================================================================

Write-Step "Deploying Web Container App: $webAppName"

$webExists = Get-ContainerAppExists -Name $webAppName -ResourceGroup $prodRG

if ($webExists) {
    Write-Detail "Updating existing $webAppName (image + env vars)..."

    az containerapp update `
        --name $webAppName `
        --resource-group $prodRG `
        --image $webImage `
        --min-replicas 0 `
        --max-replicas 1 `
        --replace-env-vars @webEnvVars
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to update Web Container App"
        exit 1
    }
}
else {
    Write-Detail "Creating new $webAppName..."

    az containerapp create `
        --name $webAppName `
        --resource-group $prodRG `
        --environment $envId `
        --image $webImage `
        --ingress external `
        --target-port 8080 `
        --transport auto `
        --registry-server $registryServer `
        --registry-username $GithubRegistryAuthUsername `
        --registry-password $ghcrToken `
        --cpu 0.5 `
        --memory 1Gi `
        --min-replicas 0 `
        --max-replicas 1 `
        --env-vars @webEnvVars
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to create Web Container App"
        exit 1
    }
}

Write-Ok "Web Container App deployed: $webAppName"

# Configure a startup probe on the Web so it is not killed while waiting for the API to
# cold-start. The Web's Program.cs blocks Kestrel startup on an API call (section cache
# pre-load). Without a startup probe, Container Apps' default TCP liveness probe kills the
# container after ~30s. The startup probe gives ~3.4 min tolerance to pass /alive.
Write-Step "Configuring startup probe on Web Container App"
Set-WebStartupProbe -AppName $webAppName -ResourceGroup $prodRG

# Enable sticky sessions for Blazor Server — required for SignalR circuit to work correctly.
Write-Step "Enabling sticky sessions on Web Container App"
$stickySetTimeoutSeconds = 90
$stickySetRetryDelaySeconds = 5
$stickySetDeadline = (Get-Date).AddSeconds($stickySetTimeoutSeconds)
$stickySessionsEnabled = $false
while ((Get-Date) -lt $stickySetDeadline) {
    $stickySetOutput = az containerapp ingress sticky-sessions set `
        --name $webAppName `
        --resource-group $prodRG `
        --affinity sticky 2>&1
    if ($LASTEXITCODE -eq 0) {
        $stickySessionsEnabled = $true
        break
    }

    $stickySetOutputText = ($stickySetOutput | Out-String).Trim()
    if ($stickySetOutputText -match 'ContainerAppOperationInProgress') {
        Write-Warn "Container App operation still in progress while enabling sticky sessions — retrying in $($stickySetRetryDelaySeconds)s"
        Start-Sleep -Seconds $stickySetRetryDelaySeconds
        continue
    }

    Write-Fail "Failed to enable sticky sessions — Blazor SignalR will not work correctly (exit code $LASTEXITCODE)"
    if (-not [string]::IsNullOrWhiteSpace($stickySetOutputText)) {
        Write-Detail $stickySetOutputText
    }
    exit 1
}
if (-not $stickySessionsEnabled) {
    Write-Fail "Failed to enable sticky sessions within $($stickySetTimeoutSeconds)s because the Container App stayed busy"
    exit 1
}
Write-Ok "Sticky sessions set — waiting for propagation..."

# Poll until the ingress affinity is confirmed as 'sticky'.
$stickyDeadline = (Get-Date).AddSeconds(60)
$stickyConfirmed = $false
while ((Get-Date) -lt $stickyDeadline) {
    $affinity = az containerapp show `
        --name $webAppName `
        --resource-group $prodRG `
        --query "properties.configuration.ingress.stickySessions.affinity" `
        -o tsv 2>$null
    if ($affinity -eq 'sticky') {
        $stickyConfirmed = $true
        break
    }
    Start-Sleep -Seconds 5
}
if (-not $stickyConfirmed) {
    Write-Fail "Sticky sessions did not propagate within 60s — Blazor SignalR will not work correctly"
    exit 1
}
Write-Ok "Sticky sessions confirmed active (affinity=sticky)"

# Get the actual web FQDN
$webFqdn = az containerapp show `
    --name $webAppName `
    --resource-group $prodRG `
    --query properties.configuration.ingress.fqdn -o tsv 2>$null

if ([string]::IsNullOrWhiteSpace($webFqdn)) {
    Write-Fail "Could not retrieve Web Container App FQDN"
    exit 1
}

Write-Ok "Web FQDN: $webFqdn"

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
