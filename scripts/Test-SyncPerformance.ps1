#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Quick test script to measure database sync performance

.DESCRIPTION
    Deletes the database and runs only the API to trigger a fresh sync.
    Monitors logs to track performance and identify bottlenecks.
    Automatically terminates once sync completes.
#>

param(
    [switch]$KeepRunning  # If set, keeps API running after sync
)

$ErrorActionPreference = "Stop"

Write-Host "`n╔════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║   Database Sync Performance Test      ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════╝`n" -ForegroundColor Cyan

# Navigate to workspace root
Push-Location $PSScriptRoot/..

try {
    # Step 1: Clean database
    Write-Host "→ Deleting existing database..." -ForegroundColor Yellow
    Remove-Item -Path "techhub.db*" -Force -ErrorAction SilentlyContinue
    Write-Host "✓ Database deleted`n" -ForegroundColor Green

    # Step 2: Clear logs
    Write-Host "→ Clearing logs..." -ForegroundColor Yellow
    Remove-Item -Path ".tmp/logs/*" -Force -Recurse -ErrorAction SilentlyContinue
    New-Item -ItemType Directory -Path ".tmp/logs" -Force | Out-Null
    Write-Host "✓ Logs cleared`n" -ForegroundColor Green

    # Step 3: Build solution
    Write-Host "→ Building solution..." -ForegroundColor Yellow
    dotnet build --nologo --verbosity quiet
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed"
    }
    Write-Host "✓ Build succeeded`n" -ForegroundColor Green

    # Step 4: Start API in background
    Write-Host "→ Starting API..." -ForegroundColor Yellow
    $apiProcess = Start-Process -FilePath "dotnet" -ArgumentList "run", "--project", "src/TechHub.Api/TechHub.Api.csproj", "--no-build" -PassThru -RedirectStandardOutput ".tmp/logs/api-sync-test.log" -RedirectStandardError ".tmp/logs/api-sync-error.log"
    Write-Host "✓ API started (PID: $($apiProcess.Id))`n" -ForegroundColor Green

    # Step 5: Monitor logs
    Write-Host "→ Monitoring sync progress...`n" -ForegroundColor Yellow
    $logFile = ".tmp/logs/api-sync-test.log"
    $syncStarted = $false
    $syncComplete = $false
    $startTime = Get-Date
    $timeout = 180  # 3 minutes

    while (-not $syncComplete -and ((Get-Date) - $startTime).TotalSeconds -lt $timeout) {
        Start-Sleep -Milliseconds 500

        if (Test-Path $logFile) {
            $content = Get-Content $logFile -Raw -ErrorAction SilentlyContinue
            
            if (-not $syncStarted -and $content -match "Starting FULL content sync") {
                $syncStarted = $true
                Write-Host "  Sync started..." -ForegroundColor Cyan
            }

            # Show progress
            if ($content -match "Progress: (\d+)/(\d+) items written") {
                $current = $matches[1]
                $total = $matches[2]
                Write-Host "`r  Progress: $current/$total items written" -NoNewline -ForegroundColor Gray
            }

            # Check for completion
            if ($content -match "Indexes and triggers re-created") {
                $syncComplete = $true
                Write-Host "`n"
            }
        }
    }

    if ($syncComplete) {
        Write-Host "✓ Sync completed!`n" -ForegroundColor Green

        # Extract performance metrics
        Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Cyan
        Write-Host "║        Performance Metrics             ║" -ForegroundColor Cyan
        Write-Host "╚════════════════════════════════════════╝`n" -ForegroundColor Cyan

        $logs = Get-Content $logFile -Raw

        # Extract timing information
        if ($logs -match "⏱️ DisableIndexesAndTriggersAsync took (\d+)ms") {
            Write-Host "  Disable indexes/triggers: $($matches[1])ms" -ForegroundColor White
        }
        if ($logs -match "⏱️ GetAllMarkdownFiles took (\d+)ms") {
            Write-Host "  File discovery: $($matches[1])ms" -ForegroundColor White
        }
        if ($logs -match "⏱️ Parallel parsing took (\d+)ms for (\d+) files \(([0-9.]+)ms per file\)") {
            Write-Host "  Parsing $($matches[2]) files: $($matches[1])ms ($($matches[3])ms/file)" -ForegroundColor White
        }
        if ($logs -match "Database writes complete: (\d+) items") {
            Write-Host "  Total items written: $($matches[1])" -ForegroundColor White
        }
        if ($logs -match "Full sync complete: (\d+) added, ([0-9,]+)ms") {
            Write-Host "  Write phase: $($matches[2])ms" -ForegroundColor White
        }
        if ($logs -match "⏱️ EnableIndexesAndTriggersAsync total: (\d+)ms") {
            Write-Host "  Enable indexes/triggers: $($matches[1])ms" -ForegroundColor White
        }

        # Show batch timing details
        Write-Host "`n  Batch timing details:" -ForegroundColor Yellow
        $logs | Select-String "⏱️ Batch \d+:" | ForEach-Object {
            Write-Host "    $_" -ForegroundColor Gray
        }

        # Show slow operations
        $slowOps = $logs | Select-String "⚠️ Slow"
        if ($slowOps) {
            Write-Host "`n  ⚠️  Slow operations detected:" -ForegroundColor Red
            $slowOps | ForEach-Object {
                Write-Host "    $_" -ForegroundColor Yellow
            }
        }

    } else {
        Write-Host "✗ Sync did not complete within timeout!`n" -ForegroundColor Red
    }

    # Step 6: Cleanup
    if (-not $KeepRunning) {
        Write-Host "`n→ Stopping API..." -ForegroundColor Yellow
        Stop-Process -Id $apiProcess.Id -Force -ErrorAction SilentlyContinue
        Write-Host "✓ API stopped`n" -ForegroundColor Green
    } else {
        Write-Host "`n→ API still running (PID: $($apiProcess.Id))" -ForegroundColor Cyan
        Write-Host "  Press Ctrl+C to stop`n" -ForegroundColor Gray
    }

} catch {
    Write-Host "`n✗ Error: $_`n" -ForegroundColor Red
    throw
} finally {
    Pop-Location
}
