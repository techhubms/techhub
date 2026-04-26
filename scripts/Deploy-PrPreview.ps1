#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys or tears down a fully ephemeral PR preview environment.

.DESCRIPTION
    Creates/updates or deletes a fully isolated PR preview environment in the staging Azure
    Container Apps Environment. Each PR gets its own:
    - PostgreSQL Flexible Server (created via PITR from production)
    - Container Apps (ca-techhub-api-pr-{number} and ca-techhub-web-pr-{number})

    The PR apps share the staging Container Apps Environment, VNet, and monitoring —
    but get their own isolated database.

    On deploy, a PR-specific Postgres instance is provisioned using Azure Point-in-Time
    Restore (PITR) from the production server. This creates an independent copy with
    realistic production data — no dump files, no firewall rules, no DB credentials needed.

    On teardown, the PR-specific Postgres instance and its private endpoint are deleted
    along with the Container Apps.

.PARAMETER PrNumber
    Pull request number. Used to derive unique resource names.

.PARAMETER Action
    Action to perform: 'deploy' to create/update the PR environment, 'teardown' to remove it.

.PARAMETER Tag
    Docker image tag to deploy. Required for 'deploy' action.

.PARAMETER RegistryName
    Azure Container Registry name (without .azurecr.io). Defaults to 'crtechhubms'.

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
    [string]$RegistryName = 'crtechhubms'
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# CONFIGURATION
# ============================================================================

# Staging infrastructure (shared with PR preview environments)
$stagingRG = 'rg-techhub-staging'
$stagingEnvName = 'cae-techhub-staging'
$stagingIdentityName = 'id-techhub-staging'
$stagingAppInsightsName = 'appi-techhub-staging'

# Production server (source for PITR database clone)
$prodRG = 'rg-techhub-prod'
$prodPostgresServer = 'psql-techhub-prod'

# PR-specific resource names
$prPostgresServer = "psql-techhub-pr-$PrNumber"
$prPostgresDb = 'techhub'
$prPostgresUser = 'techhubadmin'
$prPrivateEndpointName = "pe-psql-techhub-pr-$PrNumber"

# PR-specific Container App names
$apiAppName = "ca-techhub-api-pr-$PrNumber"
$webAppName = "ca-techhub-web-pr-$PrNumber"

$registryServer = "$RegistryName.azurecr.io"
$apiImage = "$registryServer/techhub-api:$Tag"
$webImage = "$registryServer/techhub-web:$Tag"

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
    $result = az containerapp show --name $Name --resource-group $ResourceGroup --query name -o tsv 2>$null
    return ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($result))
}

function Get-PostgresServerExists {
    param([string]$Name, [string]$ResourceGroup)
    $result = az postgres flexible-server show --name $Name --resource-group $ResourceGroup --query name -o tsv 2>$null
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
            # runningStateDetails is only present on some revisions (e.g., when ActivationFailed).
            # With Set-StrictMode -Version Latest, accessing a missing property throws —
            # use PSObject.Properties to safely check.
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
Write-Host "  Resource RG : $stagingRG" -ForegroundColor Gray
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

    if (-not $env:POSTGRES_ADMIN_PASSWORD) {
        Write-Fail "Environment variable POSTGRES_ADMIN_PASSWORD is not set. The PITR-restored server retains the production admin password."
        exit 1
    }
    Write-Ok "POSTGRES_ADMIN_PASSWORD is set"
}

# ============================================================================
# TEARDOWN
# ============================================================================

if ($Action -eq 'teardown') {
    Write-Step "Removing PR preview environment for PR #$PrNumber"

    $deletedAny = $false

    # Delete Web Container App first (it calls the API)
    if (Get-ContainerAppExists -Name $webAppName -ResourceGroup $stagingRG) {
        Write-Detail "Deleting $webAppName..."
        az containerapp delete `
            --name $webAppName `
            --resource-group $stagingRG `
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
    if (Get-ContainerAppExists -Name $apiAppName -ResourceGroup $stagingRG) {
        Write-Detail "Deleting $apiAppName..."
        az containerapp delete `
            --name $apiAppName `
            --resource-group $stagingRG `
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

    # Delete PR-specific PostgreSQL private endpoint
    Write-Step "Deleting PR PostgreSQL private endpoint: $prPrivateEndpointName"
    $peExists = az network private-endpoint show `
        --name $prPrivateEndpointName `
        --resource-group $stagingRG `
        --query name -o tsv 2>$null
    if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($peExists)) {
        az network private-endpoint delete `
            --name $prPrivateEndpointName `
            --resource-group $stagingRG
        if ($LASTEXITCODE -ne 0) {
            Write-Warn "Failed to delete private endpoint $prPrivateEndpointName — continuing"
        }
        else {
            Write-Ok "Deleted private endpoint $prPrivateEndpointName"
            $deletedAny = $true
        }
    }
    else {
        Write-Warn "$prPrivateEndpointName not found — already removed or never deployed"
    }

    # Delete PR-specific PostgreSQL server
    Write-Step "Deleting PR PostgreSQL server: $prPostgresServer"

    # Clean up DNS A record from the shared private DNS zone (safety net).
    # The dns-zone-group on the PE should auto-remove the record when the PE is deleted,
    # but we clean up explicitly in case the PE was created without a zone group.
    Write-Detail "Removing DNS A record for $prPostgresServer (safety net)..."
    az network private-dns record-set a delete `
        --resource-group 'rg-techhub-shared' `
        --zone-name 'privatelink.postgres.database.azure.com' `
        --name $prPostgresServer `
        --yes 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Ok "Deleted DNS A record for $prPostgresServer"
    }
    else {
        Write-Warn "DNS A record for $prPostgresServer not found or could not be deleted — continuing"
    }

    if (Get-PostgresServerExists -Name $prPostgresServer -ResourceGroup $stagingRG) {
        Write-Detail "Deleting $prPostgresServer (this may take a few minutes)..."
        az postgres flexible-server delete `
            --name $prPostgresServer `
            --resource-group $stagingRG `
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

    # Clean up Docker images tagged for this PR from ACR
    Write-Step "Cleaning up Docker images for PR #$PrNumber from ACR"

    $prTagFilter = "pr-$PrNumber-"
    $deleteJobs = @()

    foreach ($repo in @('techhub-api', 'techhub-web')) {
        Write-Detail "Checking $repo for tags matching '$prTagFilter*'..."

        $tags = az acr repository show-tags `
            --name $RegistryName `
            --repository $repo `
            --query "[?starts_with(@, '$prTagFilter')]" `
            -o tsv 2>$null

        if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($tags)) {
            Write-Warn "No matching tags found for $repo — skipping"
            continue
        }

        foreach ($tag in $tags -split "`n") {
            $tag = $tag.Trim()
            if ([string]::IsNullOrWhiteSpace($tag)) { continue }
            Write-Detail "Deleting ${repo}:$tag..."
            $deleteJobs += Start-Job -ScriptBlock {
                param($rn, $r, $t)
                az acr repository delete --name $rn --image "${r}:${t}" --yes 2>$null
                $LASTEXITCODE
            } -ArgumentList $RegistryName, $repo, $tag
        }
    }

    if ($deleteJobs.Count -gt 0) {
        $results = $deleteJobs | Wait-Job | Receive-Job
        $deleteJobs | Remove-Job
        # @(...) forces array context so .Count is always valid under Set-StrictMode -Version Latest.
        # Without it, Where-Object returning zero results yields $null, and $null.Count throws.
        $succeeded = @($results | Where-Object { $_ -eq 0 }).Count
        $failed    = @($results | Where-Object { $_ -ne 0 }).Count
        Write-Ok "Deleted $succeeded ACR image(s)$(if ($failed -gt 0) { "; $failed failed (non-fatal)" })"
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

$pgExists = Get-PostgresServerExists -Name $prPostgresServer -ResourceGroup $stagingRG

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

    # Use a restore time of 03:00 UTC today for a stable backup point (avoids mid-migration state).
    # If it's before 04:00 UTC, use yesterday's 03:00 UTC to ensure the backup window has completed.
    $nowUtc = [DateTime]::UtcNow
    $restoreDate = if ($nowUtc.Hour -lt 4) { $nowUtc.AddDays(-1).Date.AddHours(3) } else { $nowUtc.Date.AddHours(3) }
    $restoreTime = $restoreDate.ToString('yyyy-MM-ddTHH:mm:ssZ')
    Write-Detail "PITR restore time: $restoreTime"

    Write-Detail "Creating $prPostgresServer via PITR from production (this takes 5-8 minutes)..."
    az postgres flexible-server restore `
        --resource-group $stagingRG `
        --name $prPostgresServer `
        --source-server $prodServerId `
        --restore-time $restoreTime
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to create PR PostgreSQL server via PITR"
        exit 1
    }
    Write-Ok "PR PostgreSQL server created: $prPostgresServer"

    # The PITR-restored server inherits the production admin password.
    # Reset it to match the POSTGRES_ADMIN_PASSWORD env var (the staging environment secret)
    # so the connection string built later uses the correct credentials.
    Write-Detail "Resetting admin password on $prPostgresServer to match staging secret..."
    az postgres flexible-server update `
        --resource-group $stagingRG `
        --name $prPostgresServer `
        --admin-password $env:POSTGRES_ADMIN_PASSWORD
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to reset admin password on $prPostgresServer"
        exit 1
    }
    Write-Ok "Admin password reset on $prPostgresServer"
}

# Create private endpoint for the PR Postgres server (if not already present)
Write-Step "Ensuring private endpoint for PR PostgreSQL: $prPrivateEndpointName"

$peExists = az network private-endpoint show `
    --name $prPrivateEndpointName `
    --resource-group $stagingRG `
    --query name -o tsv 2>$null

if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($peExists)) {
    Write-Ok "Private endpoint already exists: $prPrivateEndpointName"
}
else {
    # Get the PR Postgres server resource ID
    $prServerId = az postgres flexible-server show `
        --name $prPostgresServer `
        --resource-group $stagingRG `
        --query id -o tsv 2>$null

    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($prServerId)) {
        Write-Fail "Could not retrieve PR PostgreSQL server ID"
        exit 1
    }

    Write-Detail "Creating private endpoint $prPrivateEndpointName..."
    az network private-endpoint create `
        --name $prPrivateEndpointName `
        --resource-group $stagingRG `
        --vnet-name 'vnet-techhub-staging' `
        --subnet 'snet-private-endpoints' `
        --private-connection-resource-id $prServerId `
        --group-ids postgresqlServer `
        --connection-name "$($prPrivateEndpointName)-conn"
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to create private endpoint for PR PostgreSQL"
        exit 1
    }

    Write-Ok "Private endpoint created: $prPrivateEndpointName"
}

# Register the private endpoint in the shared PostgreSQL private DNS zone via a DNS zone group.
# This is equivalent to the Bicep 'privateDnsZoneGroups' resource used by prod/staging PEs —
# Azure automatically manages the A record (create on attach, update on IP change, delete on
# PE teardown). No manual record-set management needed.
Write-Step "Ensuring DNS zone group for PR PostgreSQL private endpoint"

$privateDnsZoneName = 'privatelink.postgres.database.azure.com'
$sharedRG = 'rg-techhub-shared'

$dnsZoneId = az network private-dns zone show `
    --resource-group $sharedRG `
    --name $privateDnsZoneName `
    --query id -o tsv 2>$null

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($dnsZoneId)) {
    Write-Fail "Could not retrieve private DNS zone ID for '$privateDnsZoneName' in '$sharedRG'"
    exit 1
}

# Create or update the DNS zone group on the PE (idempotent).
az network private-endpoint dns-zone-group create `
    --resource-group $stagingRG `
    --endpoint-name $prPrivateEndpointName `
    --name 'default' `
    --private-dns-zone $dnsZoneId `
    --zone-name 'privatelink-postgres-database-azure-com'
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to create DNS zone group on '$prPrivateEndpointName'"
    exit 1
}

Write-Ok "DNS zone group configured — A record auto-managed for $prPostgresServer"

# ============================================================================
# DEPLOY — Query staging infrastructure
# ============================================================================

Write-Step "Querying staging infrastructure"

# Get Container Apps Environment ID
$envId = az containerapp env show `
    --name $stagingEnvName `
    --resource-group $stagingRG `
    --query id -o tsv 2>$null

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($envId)) {
    Write-Fail "Could not find staging Container Apps Environment '$stagingEnvName' in '$stagingRG'"
    exit 1
}
Write-Ok "Container Apps Environment: $envId"

# Get managed identity ID (used for ACR pull)
$identityId = az identity show `
    --name $stagingIdentityName `
    --resource-group $stagingRG `
    --query id -o tsv 2>$null

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($identityId)) {
    Write-Fail "Could not find managed identity '$stagingIdentityName' in '$stagingRG'"
    exit 1
}
Write-Ok "Managed Identity: $identityId"

# Get Application Insights connection string
$appInsightsConnString = az monitor app-insights component show `
    --app $stagingAppInsightsName `
    --resource-group $stagingRG `
    --query connectionString -o tsv 2>$null

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($appInsightsConnString)) {
    Write-Warn "Could not retrieve AppInsights connection string — proceeding without telemetry"
    $appInsightsConnString = ""
}
else {
    Write-Ok "Application Insights: retrieved connection string"
}

# Compute the PR postgres FQDN and build the connection string.
# After PITR restore, the admin password is reset to match POSTGRES_ADMIN_PASSWORD.
# The connection string uses the private endpoint FQDN for VNet-internal access.
$prPostgresFqdn = "$prPostgresServer.postgres.database.azure.com"
$dbConnectionString = "Host=$prPostgresFqdn;Database=$prPostgresDb;Username=$prPostgresUser;Password=$($env:POSTGRES_ADMIN_PASSWORD);SSL Mode=Require"
Write-Ok "PostgreSQL: $prPostgresFqdn"

# Get the Container Apps Environment default domain (to compute expected FQDNs)
$envDefaultDomain = az containerapp env show `
    --name $stagingEnvName `
    --resource-group $stagingRG `
    --query properties.defaultDomain -o tsv 2>$null

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($envDefaultDomain)) {
    Write-Fail "Could not retrieve Container Apps Environment default domain"
    exit 1
}
Write-Ok "Environment default domain: $envDefaultDomain"

# Pre-compute expected FQDNs (used for CORS and ApiBaseUrl)
# Internal apps use the "internal." prefix in their FQDN within the environment
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
    "AppSettings__Content__CollectionsPath=/app/collections",
    "AppSettings__BaseUrl=https://$webPrFqdn",
    "TECHHUB_TMP=/tmp/techhub",
    "Cors__AllowedOrigins__0=https://$webPrFqdn"
)

$webEnvVars = @(
    "ASPNETCORE_ENVIRONMENT=Staging",
    "APPLICATIONINSIGHTS_CONNECTION_STRING=$appInsightsConnString",
    "OTEL_SERVICE_NAME=techhub-web",
    "ApiBaseUrl=https://$apiPrFqdn",
    "TECHHUB_TMP=/tmp/techhub"
)

# ============================================================================
# DEPLOY API CONTAINER APP
# ============================================================================

Write-Step "Deploying API Container App: $apiAppName"

$apiExists = Get-ContainerAppExists -Name $apiAppName -ResourceGroup $stagingRG

if ($apiExists) {
    Write-Detail "Updating existing $apiAppName (image + env vars + secrets)..."

    # Always refresh the DB connection string secret to ensure it matches the current
    # POSTGRES_ADMIN_PASSWORD. The create path sets the secret initially, but subsequent
    # deploys must update it in case the password rotated or the PITR server was recreated.
    az containerapp secret set `
        --name $apiAppName `
        --resource-group $stagingRG `
        --secrets "db-connection-string=$dbConnectionString" 2>$null | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to update API Container App secrets"
        exit 1
    }

    az containerapp update `
        --name $apiAppName `
        --resource-group $stagingRG `
        --image $apiImage `
        --replace-env-vars @apiEnvVars
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to update API Container App"
        exit 1
    }
}
else {
    Write-Detail "Creating new $apiAppName..."

    az containerapp create `
        --name $apiAppName `
        --resource-group $stagingRG `
        --environment $envId `
        --image $apiImage `
        --ingress internal `
        --target-port 8080 `
        --transport http `
        --registry-server $registryServer `
        --registry-identity $identityId `
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

# Set the Container Apps-level ingress CORS policy (separate from the ASP.NET CORS middleware
# configured via env vars). Both layers are needed to allow cross-origin requests.
Write-Detail "Updating API ingress CORS policy..."
az containerapp ingress cors update `
    --name $apiAppName `
    --resource-group $stagingRG `
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
    --resource-group $stagingRG `
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

$webExists = Get-ContainerAppExists -Name $webAppName -ResourceGroup $stagingRG

if ($webExists) {
    Write-Detail "Updating existing $webAppName (image + env vars)..."

    az containerapp update `
        --name $webAppName `
        --resource-group $stagingRG `
        --image $webImage `
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
        --resource-group $stagingRG `
        --environment $envId `
        --image $webImage `
        --ingress external `
        --target-port 8080 `
        --transport http `
        --registry-server $registryServer `
        --registry-identity $identityId `
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

# Enable sticky sessions for Blazor Server — required for SignalR circuit to work correctly.
# Without session affinity, WebSocket connections may route to a different container instance
# than the one that rendered the SSR HTML, breaking the Blazor Server interactive circuit.
# az containerapp create does not support --sticky-sessions, so we always set it via ingress update.
Write-Detail "Enabling sticky sessions for $webAppName..."
az containerapp ingress sticky-sessions set `
    --name $webAppName `
    --resource-group $stagingRG `
    --affinity sticky
if ($LASTEXITCODE -ne 0) {
    Write-Warn "Could not enable sticky sessions — Blazor SignalR may be unreliable with multiple replicas"
}
else {
    Write-Ok "Sticky sessions enabled (required for Blazor Server)"
}

# Get the actual web FQDN
$webFqdn = az containerapp show `
    --name $webAppName `
    --resource-group $stagingRG `
    --query properties.configuration.ingress.fqdn -o tsv 2>$null

if ([string]::IsNullOrWhiteSpace($webFqdn)) {
    Write-Fail "Could not retrieve Web Container App FQDN"
    exit 1
}

Write-Ok "Web FQDN: $webFqdn"

# Write output for GitHub Actions
if ($env:GITHUB_OUTPUT) {
    "web-url=https://$webFqdn" | Out-File -Append -FilePath $env:GITHUB_OUTPUT
    Write-Ok "Written web-url to GITHUB_OUTPUT"
}

# ============================================================================
# WARMUP — HTTP requests to trigger scaling and verify health
# ============================================================================

# Both apps are deployed with minReplicas=0 (scale to zero). They stay at ScaledToZero
# until actual HTTP traffic arrives — management-plane polling (revision runningState)
# can't trigger scaling. We send real HTTP requests to the Web app, which triggers both:
# 1. Web scaling (direct HTTP request)
# 2. API scaling (Web makes server-side calls to the internal API on page load)

# Phase 1: Wait for the Web container to start responding (/alive liveness check).
# This triggers the Web to scale from zero but doesn't hit the API yet.
Write-Step "Warming up Web Container App at https://$webFqdn"
$aliveUrl = "https://$webFqdn/alive"
$maxAttempts = 60  # 60 × 5s = 5 minutes max
$attempt = 0
$webAlive = $false
while ($attempt -lt $maxAttempts) {
    $attempt++
    try {
        $response = Invoke-WebRequest -Uri $aliveUrl -Method GET -TimeoutSec 10 -UseBasicParsing -ErrorAction Stop
        if ($response.StatusCode -lt 500) {
            Write-Ok "Web is alive (HTTP $($response.StatusCode)) after $($attempt * 5)s"
            $webAlive = $true
            break
        }
    }
    catch {
        # Connection refused, timeout, or 5xx — keep waiting
    }
    Write-Detail "Not yet responding (attempt $attempt/$maxAttempts) — waiting 5s..."
    Start-Sleep -Seconds 5
}
if (-not $webAlive) {
    Write-Fail "Web did not respond to /alive within $($maxAttempts * 5)s — failing deploy"
    Write-ContainerAppDiagnostics -AppName $webAppName -ResourceGroup $stagingRG
    Write-ContainerAppDiagnostics -AppName $apiAppName -ResourceGroup $stagingRG
    exit 1
}

# Phase 2: Hit the homepage to trigger the API to scale up. The Web's SSR page load
# makes server-side calls to the internal API, which triggers it to scale from zero.
# This also validates the full Web → API → DB chain before E2E tests run.
Write-Step "Warming up API via Web homepage (triggers Web → API → DB)"
$homepageUrl = "https://$webFqdn/"
$apiMaxAttempts = 36  # 36 × 5s = 3 minutes max
$apiAttempt = 0
$apiReady = $false
while ($apiAttempt -lt $apiMaxAttempts) {
    $apiAttempt++
    try {
        $response = Invoke-WebRequest -Uri $homepageUrl -Method GET -TimeoutSec 15 -UseBasicParsing -ErrorAction Stop
        if ($response.StatusCode -eq 200) {
            Write-Ok "Homepage loaded successfully (HTTP 200) after $($apiAttempt * 5)s — API is responding"
            $apiReady = $true
            break
        }
        Write-Detail "Homepage returned HTTP $($response.StatusCode) (attempt $apiAttempt/$apiMaxAttempts) — waiting 5s..."
    }
    catch {
        Write-Detail "Homepage not ready (attempt $apiAttempt/$apiMaxAttempts) — waiting 5s..."
    }
    Start-Sleep -Seconds 5
}
if (-not $apiReady) {
    # The Web is alive but the homepage fails — likely an API or DB connectivity issue.
    # Dump diagnostics for both apps to aid triage.
    Write-Fail "Homepage did not return HTTP 200 within $($apiMaxAttempts * 5)s — API may be unhealthy"
    Write-ContainerAppDiagnostics -AppName $apiAppName -ResourceGroup $stagingRG
    Write-ContainerAppDiagnostics -AppName $webAppName -ResourceGroup $stagingRG
    exit 1
}

# New revision is healthy — clean up old ACR images for this PR (keep only the tag just deployed).
# Done AFTER warmup so the old revision's image is still pullable if the new one fails to start.
Write-Step "Cleaning up old ACR images for PR #$PrNumber (keeping: $Tag)"
$prTagFilter = "pr-$PrNumber-"
foreach ($repo in @('techhub-api', 'techhub-web')) {
    $oldTags = az acr repository show-tags `
        --name $RegistryName `
        --repository $repo `
        --query "[?starts_with(@, '$prTagFilter') && @ != '$Tag']" `
        -o tsv 2>$null

    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($oldTags)) {
        Write-Detail "No old tags to clean up for $repo"
        continue
    }

    foreach ($oldTag in $oldTags -split "`n") {
        $oldTag = $oldTag.Trim()
        if ([string]::IsNullOrWhiteSpace($oldTag)) { continue }
        Write-Detail "Deleting old ${repo}:$oldTag..."
        az acr repository delete `
            --name $RegistryName `
            --image "${repo}:$oldTag" `
            --yes 2>$null | Out-Null
        if ($LASTEXITCODE -eq 0) {
            Write-Ok "Deleted old ${repo}:$oldTag"
        }
        else {
            Write-Warn "Could not delete ${repo}:$oldTag — skipping"
        }
    }
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
