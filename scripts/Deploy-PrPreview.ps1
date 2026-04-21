#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys or tears down a PR preview environment in the staging Azure Container Apps environment.

.DESCRIPTION
    Creates/updates or deletes PR-specific Container Apps in the existing staging Container Apps
    Environment. Each PR gets its own Container Apps (ca-techhub-api-pr-{number} and
    ca-techhub-web-pr-{number}) running in the shared staging environment, without affecting
    the permanent staging application.

    The PR apps share the staging PostgreSQL database, monitoring, and networking — only the
    Container Apps themselves are PR-specific.

.PARAMETER PrNumber
    Pull request number. Used to derive unique Container App names.

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
$stagingPostgresServer = 'psql-techhub-staging'
$stagingPostgresDb = 'techhub'
$stagingPostgresUser = 'techhubadmin'

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
        Write-Fail "Environment variable POSTGRES_ADMIN_PASSWORD is not set."
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
        $succeeded = ($results | Where-Object { $_ -eq 0 }).Count
        $failed    = ($results | Where-Object { $_ -ne 0 }).Count
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

# Compute the postgres FQDN and build the connection string
$postgresFqdn = "$stagingPostgresServer.postgres.database.azure.com"
$dbConnectionString = "Host=$postgresFqdn;Database=$stagingPostgresDb;Username=$stagingPostgresUser;Password=$($env:POSTGRES_ADMIN_PASSWORD);SSL Mode=Require"
Write-Ok "PostgreSQL: $postgresFqdn"

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
    Write-Detail "Updating existing $apiAppName (image + env vars)..."

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

# Clean up old images for this PR from ACR (keep only the tag just deployed)
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

# Warmup: wait for the first successful HTTP response before returning.
# Container Apps can take 30-90s after deployment before the container is reachable
# (image pull, startup probe, cold-start). Waiting here means the caller (CI) gets
# a URL that is already responding — no extra sleep needed in the workflow.
Write-Step "Waiting for Web to respond at https://$webFqdn"
$warmupUrl = "https://$webFqdn/alive"
$maxAttempts = 60  # 60 × 5s = 5 minutes max
$attempt = 0
$ready = $false
while ($attempt -lt $maxAttempts) {
    $attempt++
    try {
        $response = Invoke-WebRequest -Uri $warmupUrl -Method GET -TimeoutSec 10 -UseBasicParsing -ErrorAction Stop
        if ($response.StatusCode -lt 500) {
            Write-Ok "Web is responding (HTTP $($response.StatusCode)) after $($attempt * 5)s"
            $ready = $true
            break
        }
    }
    catch {
        # Connection refused, timeout, or 5xx — keep waiting
    }
    Write-Detail "Not yet responding (attempt $attempt/$maxAttempts) — waiting 5s..."
    Start-Sleep -Seconds 5
}
if (-not $ready) {
    # Hard fail: if the web app hasn't responded to /alive within the warmup
    # window, something is wrong with the revision (failed activation,
    # ImagePullUnauthorized, crash loop, etc.). Running E2E tests against a
    # dead site just wastes 60s per test and obscures the real failure.
    # Dump recent system log events for the latest revision to aid triage.
    Write-Err "Web did not respond within $($maxAttempts * 5)s — failing deploy"
    try {
        $latestRevision = az containerapp revision list `
            -n $webAppName -g $ResourceGroup `
            --query "sort_by([], &properties.createdTime) | [-1]" -o json 2>$null |
            ConvertFrom-Json -ErrorAction SilentlyContinue
        if ($latestRevision) {
            Write-Host ""
            Write-Host "Latest revision status:" -ForegroundColor Yellow
            Write-Host "  Name            : $($latestRevision.name)" -ForegroundColor Gray
            Write-Host "  Image           : $($latestRevision.properties.template.containers[0].image)" -ForegroundColor Gray
            Write-Host "  Replicas        : $($latestRevision.properties.replicas)" -ForegroundColor Gray
            Write-Host "  HealthState     : $($latestRevision.properties.healthState)" -ForegroundColor Gray
            Write-Host "  RunningState    : $($latestRevision.properties.runningState)" -ForegroundColor Gray
            if ($latestRevision.properties.runningStateDetails) {
                Write-Host "  Details         : $($latestRevision.properties.runningStateDetails)" -ForegroundColor Gray
            }
        }
        Write-Host ""
        Write-Host "Recent system events for $webAppName (last 30):" -ForegroundColor Yellow
        az containerapp logs show -n $webAppName -g $ResourceGroup --type system --tail 30 2>&1 |
            Select-Object -Last 30
    }
    catch {
        Write-Detail "Could not fetch diagnostic info: $_"
    }
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
Write-Host "  Web URL     : https://$webFqdn" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
