#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Downloads and restores the production PostgreSQL database to the local development environment.

.DESCRIPTION
    Performs a logical dump of the production TechHub PostgreSQL database (content_items and
    related tables only) and restores it to the target environment. This replaces the need to
    run ContentSyncService locally — developers get a production data snapshot
    instead of processing markdown files. ContentSync has been removed from production; the
    database is now the single source of truth.

    Prerequisites:
    - pg_dump and psql (or pg_restore) installed locally (PostgreSQL client tools)
    - VPN connection to the production environment (production DB is behind a private endpoint)
    - Azure CLI authenticated (for fetching connection parameters if not provided manually)

.PARAMETER Target
    Target environment to restore the database into. Options:
    - local:   Restores to the local Docker Compose PostgreSQL instance.

.PARAMETER ProductionConnectionString
    Optional. Full PostgreSQL connection string for the production database.
    Format: Host=<host>;Database=techhub;Username=<user>;Password=<pass>;SSL Mode=Require
    When omitted, the script attempts to read it from environment variables or Azure secrets.

.PARAMETER TargetConnectionString
    Optional. Full PostgreSQL connection string for the target database.
    When omitted, defaults to the local Docker Compose connection string.

.PARAMETER OutputPath
    Optional. Path where the database dump file is saved before restoring.
    Defaults to .tmp/db-restore-<timestamp>.dump

.PARAMETER SkipDump
    Skip the dump step and use the file at OutputPath (must already exist).

.PARAMETER SkipRestore
    Perform the dump but skip the restore step (dump only).

.PARAMETER TablesOnly
    Only dump and restore the content tables (content_items, content_tags_expanded, _migrations).
    Recommended to avoid restoring users, permissions, or infrastructure tables.

.EXAMPLE
    # Restore production data to local development environment
    ./scripts/Restore-Database.ps1 -Target local

.EXAMPLE
    # Dump production data to file only (no restore)
    ./scripts/Restore-Database.ps1 -Target local -SkipRestore

.EXAMPLE
    # Restore a previously downloaded dump to local
    ./scripts/Restore-Database.ps1 -Target local -SkipDump -OutputPath .tmp/db-restore-20260101.dump
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('local')]
    [string]$Target,

    [Parameter(Mandatory = $false)]
    [string]$ProductionConnectionString,

    [Parameter(Mandatory = $false)]
    [string]$TargetConnectionString,

    [Parameter(Mandatory = $false)]
    [string]$OutputPath,

    [switch]$SkipDump,
    [switch]$SkipRestore,

    [switch]$TablesOnly
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# CONFIGURATION
# ============================================================================

$workspaceRoot = if (Test-Path (Join-Path $PSScriptRoot "../src")) {
    (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
} else {
    $PSScriptRoot
}

$tmpDir = Join-Path $workspaceRoot ".tmp"
if (-not (Test-Path $tmpDir)) {
    New-Item -ItemType Directory -Path $tmpDir -Force | Out-Null
}

$timestamp = Get-Date -Format 'yyyyMMddHHmmss'
if (-not $OutputPath) {
    $OutputPath = Join-Path $tmpDir "db-restore-$timestamp.dump"
}

# Default local connection string (matches docker-compose.yml)
$localConnectionString = "Host=localhost;Port=5432;Database=techhub;Username=techhub;Password=localdev"

# Tables to include when -TablesOnly is specified
$contentTables = @(
    "content_items",
    "content_tags_expanded",
    "_migrations",
    "content_processing_jobs",
    "rss_feed_configs",
    "processed_urls"
)

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

function Write-Fail {
    param([string]$Message)
    Write-Host "   [FAIL] $Message" -ForegroundColor Red
}

function Write-Detail {
    param([string]$Message)
    Write-Host "   $Message" -ForegroundColor Gray
}

function Parse-ConnectionString {
    param([string]$ConnectionString)

    $parts = @{}
    foreach ($part in $ConnectionString.Split(';')) {
        $kv = $part.Split('=', 2)
        if ($kv.Count -eq 2) {
            $parts[$kv[0].Trim()] = $kv[1].Trim()
        }
    }
    return $parts
}

function Get-PgEnv {
    param([hashtable]$Params)
    return @{
        PGHOST     = $Params['Host']
        PGPORT     = if ($Params.ContainsKey('Port')) { $Params['Port'] } else { '5432' }
        PGDATABASE = $Params['Database']
        PGUSER     = $Params['Username']
        PGPASSWORD = $Params['Password']
        PGSSLMODE  = if ($Params.ContainsKey('SSL Mode')) { $Params['SSL Mode'].ToLower() -replace ' ', '' } else { 'prefer' }
    }
}

function Invoke-PgDump {
    param(
        [hashtable]$PgEnv,
        [string]$OutputFile,
        [string[]]$Tables
    )

    Write-Detail "Running pg_dump..."

    $env:PGHOST = $PgEnv.PGHOST
    $env:PGPORT = $PgEnv.PGPORT
    $env:PGDATABASE = $PgEnv.PGDATABASE
    $env:PGUSER = $PgEnv.PGUSER
    $env:PGPASSWORD = $PgEnv.PGPASSWORD
    $env:PGSSLMODE = $PgEnv.PGSSLMODE

    $args = @(
        "--format=custom",
        "--no-owner",
        "--no-acl",
        "--file=$OutputFile"
    )

    if ($Tables.Count -gt 0) {
        foreach ($table in $Tables) {
            $args += "--table=$table"
        }
    }

    & pg_dump @args

    $env:PGPASSWORD = $null
    $env:PGSSLMODE = $null

    if ($LASTEXITCODE -ne 0) {
        Write-Fail "pg_dump failed with exit code $LASTEXITCODE"
        exit 1
    }
}

function Invoke-PgRestore {
    param(
        [hashtable]$PgEnv,
        [string]$InputFile,
        [switch]$DropAndRecreate
    )

    $env:PGHOST = $PgEnv.PGHOST
    $env:PGPORT = $PgEnv.PGPORT
    $env:PGDATABASE = $PgEnv.PGDATABASE
    $env:PGUSER = $PgEnv.PGUSER
    $env:PGPASSWORD = $PgEnv.PGPASSWORD
    $env:PGSSLMODE = $PgEnv.PGSSLMODE

    if ($DropAndRecreate) {
        # Drop and recreate the database from scratch so no stale tables remain.
        # Connect to the 'postgres' maintenance DB to issue DROP/CREATE.
        Write-Detail "Dropping and recreating database '$($PgEnv.PGDATABASE)'..."
        $resetSql = @"
SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '$($PgEnv.PGDATABASE)' AND pid <> pg_backend_pid();
DROP DATABASE IF EXISTS "$($PgEnv.PGDATABASE)";
CREATE DATABASE "$($PgEnv.PGDATABASE)" OWNER "$($PgEnv.PGUSER)";
"@
        $resetSql | & psql `
            --host $PgEnv.PGHOST `
            --port $PgEnv.PGPORT `
            --username $PgEnv.PGUSER `
            --dbname postgres `
            --no-password `
            -f - 2>&1 | Out-Null

        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to drop/recreate database."
            exit 1
        }
        Write-Detail "Database recreated."
    }

    Write-Detail "Running pg_restore..."

    $args = @(
        "--no-owner",
        "--no-acl",
        "--single-transaction",
        "--dbname=postgresql://$($PgEnv.PGUSER)@$($PgEnv.PGHOST):$($PgEnv.PGPORT)/$($PgEnv.PGDATABASE)",
        $InputFile
    )

    & pg_restore @args

    $env:PGPASSWORD = $null
    $env:PGSSLMODE = $null

    if ($LASTEXITCODE -ne 0) {
        # pg_restore exits with 1 even on partial success (warnings); only fail on exit code > 1
        if ($LASTEXITCODE -gt 1) {
            Write-Fail "pg_restore failed with exit code $LASTEXITCODE"
            exit 1
        }
        Write-Host "   [WARN] pg_restore completed with warnings (exit code 1 - usually safe to ignore)" -ForegroundColor Yellow
    }
}

# ============================================================================
# BANNER
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  TechHub Database Restore" -ForegroundColor White
Write-Host "  Target      : $Target" -ForegroundColor Gray
Write-Host "  Tables only : $($TablesOnly.IsPresent)" -ForegroundColor Gray
Write-Host "  Dump file   : $OutputPath" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# VALIDATE PREREQUISITES
# ============================================================================

Write-Step "Validating prerequisites"

# Check for pg_dump / pg_restore
$pgDump = Get-Command pg_dump -ErrorAction SilentlyContinue
if (-not $pgDump) {
    Write-Fail "pg_dump not found. Install PostgreSQL client tools (e.g. 'apt install postgresql-client' or install PostgreSQL)."
    exit 1
}
Write-Ok "pg_dump found: $($pgDump.Source)"

$pgRestore = Get-Command pg_restore -ErrorAction SilentlyContinue
if (-not $pgRestore) {
    Write-Fail "pg_restore not found. Install PostgreSQL client tools."
    exit 1
}
Write-Ok "pg_restore found: $($pgRestore.Source)"

# ============================================================================
# RESOLVE CONNECTION STRINGS
# ============================================================================

Write-Step "Resolving connection strings"

# Production connection string — only needed when actually dumping
if (-not $SkipDump) {
    if (-not $ProductionConnectionString) {
        $ProductionConnectionString = $env:TECHHUB_PROD_DB_CONNECTION_STRING
    }
}

if (-not $SkipDump -and -not $ProductionConnectionString) {
    # Try to fetch from Azure (requires az CLI login and VPN access)
    Write-Detail "Attempting to read production connection string from Azure..."
    try {
        $prodRg = "rg-techhub-prod"
        $prodServer = "psql-techhub-prod"
        $prodDb = "techhub"
        $adminUser = "techhubadmin"

        # Get PostgreSQL server FQDN
        $serverFqdn = az postgres flexible-server show `
            --name $prodServer `
            --resource-group $prodRg `
            --query fullyQualifiedDomainName `
            -o tsv 2>$null

        if ($serverFqdn -and $LASTEXITCODE -eq 0) {
            Write-Host "   Production server: $serverFqdn" -ForegroundColor Yellow

            # Try to fetch password from GitHub secret POSTGRES_PROD_PW
            $adminPassword = $null
            $ghPw = gh secret view POSTGRES_PROD_PW --json value -q ".value" 2>$null
            if ($ghPw -and $LASTEXITCODE -eq 0) {
                $adminPassword = $ghPw.Trim()
                Write-Ok "Retrieved production password from GitHub secret POSTGRES_PROD_PW"
            }

            if (-not $adminPassword) {
                # Fall back to interactive prompt
                $securePassword = Read-Host -Prompt "   Enter production database password for '$adminUser'" -AsSecureString
                $adminPassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
                    [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
                )
            }

            if (-not $adminPassword) {
                Write-Fail "Password is required to connect to the production database."
                exit 1
            }
            $ProductionConnectionString = "Host=$serverFqdn;Database=$prodDb;Username=$adminUser;Password=$adminPassword;SSL Mode=Require"
            Write-Ok "Retrieved production server FQDN: $serverFqdn"
        }
        else {
            Write-Fail "Could not retrieve production PostgreSQL FQDN. Ensure you are logged in to Azure CLI and connected via VPN."
            Write-Fail "Alternatively, provide -ProductionConnectionString parameter."
            exit 1
        }
    }
    catch {
        Write-Fail "Failed to retrieve production connection string: $($_.Exception.Message)"
        exit 1
    }
}
if (-not $SkipDump) {
    Write-Ok "Production connection string resolved"
}

# Target connection string
if (-not $TargetConnectionString) {
    if ($Target -eq 'local') {
        $TargetConnectionString = $localConnectionString
        Write-Ok "Target: local Docker Compose PostgreSQL"
    }
}

# ============================================================================
# DUMP
# ============================================================================

if (-not $SkipDump) {
    Write-Step "Dumping production database"

    $prodParams = Parse-ConnectionString -ConnectionString $ProductionConnectionString
    $prodPgEnv = Get-PgEnv -Params $prodParams

    [string[]]$tablesToDump = @(if ($TablesOnly) { $contentTables })

    if ($tablesToDump.Count -gt 0) {
        Write-Detail "Dumping tables: $($tablesToDump -join ', ')"
    }
    else {
        Write-Detail "Dumping entire database"
    }

    Invoke-PgDump -PgEnv $prodPgEnv -OutputFile $OutputPath -Tables $tablesToDump

    $dumpSizeMb = [math]::Round((Get-Item $OutputPath).Length / 1MB, 1)
    Write-Ok "Dump complete: $OutputPath ($dumpSizeMb MB)"
}
else {
    Write-Step "Skipping dump (using existing file)"
    if (-not (Test-Path $OutputPath)) {
        Write-Fail "Dump file not found: $OutputPath"
        exit 1
    }
    Write-Ok "Using existing dump: $OutputPath"
}

# ============================================================================
# RESTORE
# ============================================================================

if (-not $SkipRestore) {
    Write-Step "Restoring to $Target"

    $targetParams = Parse-ConnectionString -ConnectionString $TargetConnectionString
    $targetPgEnv = Get-PgEnv -Params $targetParams

    Write-Detail "Target: $($targetPgEnv.PGHOST):$($targetPgEnv.PGPORT)/$($targetPgEnv.PGDATABASE)"

    # Stop app servers first so they don't hold DB connections that would race
    # with DROP DATABASE, then drop and recreate for a clean slate.
    if ($Target -eq 'local') {
        Write-Step "Stopping local app servers"
        docker compose stop api web 2>&1 | Out-Null
        docker compose rm -f api web 2>&1 | Out-Null
        foreach ($port in @(5001, 5003)) {
            $pids = lsof -ti ":$port" 2>$null
            if ($pids) {
                foreach ($p in $pids) { kill -9 $p 2>$null }
            }
        }
        Write-Ok "Local app servers stopped"
    }


    $dropAndRecreate = $true
    Invoke-PgRestore -PgEnv $targetPgEnv -InputFile $OutputPath -DropAndRecreate:$dropAndRecreate

    Write-Ok "Restore complete"



    # Reset the local database user password to match the local connection string,
    # so the app can connect without changing connection strings.
    if ($Target -eq 'local') {
        Write-Step "Resetting local database password"

        $localTargetParams = Parse-ConnectionString -ConnectionString $TargetConnectionString
        $localUser = $localTargetParams['Username']
        $localPassword = $localTargetParams['Password']

        Write-Detail "Setting password for role '$localUser' to match local connection string"

        $env:PGHOST = $targetPgEnv.PGHOST
        $env:PGPORT = $targetPgEnv.PGPORT
        $env:PGDATABASE = $targetPgEnv.PGDATABASE
        $env:PGUSER = $targetPgEnv.PGUSER
        $env:PGPASSWORD = $targetPgEnv.PGPASSWORD

        & psql -c "ALTER ROLE $localUser WITH PASSWORD '$localPassword';"

        $env:PGPASSWORD = $null

        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to reset local password for role '$localUser'"
            exit 1
        }
        Write-Ok "Local password reset for role '$localUser'"
    }
}
else {
    Write-Step "Skipping restore"
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Database Restore Complete" -ForegroundColor Green
Write-Host "  Target : $Target" -ForegroundColor Gray
if (-not $SkipDump) {
    Write-Host "  Dump   : $OutputPath" -ForegroundColor Gray
}
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
