# Jekyll Stop Script
# Gracefully stops existing Jekyll processes

[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"
$ProgressPreference = 'SilentlyContinue'
Set-StrictMode -Version Latest

# Load shared Jekyll helper functions
. (Join-Path $PSScriptRoot "jekyll-helpers.ps1")

# Validate that no unknown parameters were passed
$boundParameters = $PSBoundParameters.Keys
$validParameters = @()  # No parameters currently supported
$unknownParameters = $boundParameters | Where-Object { $_ -notin $validParameters }

if ($unknownParameters) {
    Write-Host "Error: Unknown parameter(s) specified: $($unknownParameters -join ', ')" -ForegroundColor Red
    Write-Host "This script does not accept any parameters." -ForegroundColor Yellow
    exit 1
}

Write-Host "Stopping existing Jekyll processes..." -ForegroundColor Yellow

# Check if Jekyll is running using shared detection
$jekyllStatus = Test-JekyllRunning -Cleanup

if ($jekyllStatus.IsRunning) {
    Write-Host "Found Jekyll running (Method: $($jekyllStatus.Method))" -ForegroundColor Cyan
    
    if ($jekyllStatus.Pid) {
        # Stop using the PID we found
        $stopped = Stop-Jekyll -ProcessId $jekyllStatus.Pid
        if ($stopped) {
            # Clean up PID file
            Clear-JekyllFiles -PidOnly
            Write-Host "✅ Jekyll server stopped successfully" -ForegroundColor Green
            exit 0
        } else {
            Write-Host "⚠️  Could not stop Jekyll via PID" -ForegroundColor Yellow
        }
    }
}

# Fallback: Try to find Jekyll processes by command line pattern
Write-Host "Checking for Jekyll processes..." -ForegroundColor Yellow

$ProcessesFound = $false

# Search for jekyll pattern in process list
$jekyllProcesses = Get-Process | Where-Object { $_.CommandLine -like '*jekyll serve*' } | Select-Object -Property ProcessName, Id

if ($jekyllProcesses) {
    Write-Host "Found $($jekyllProcesses.Count) Jekyll process(es), stopping them..." -ForegroundColor Cyan
    $ProcessesFound = $true

    foreach ($Process in $jekyllProcesses) {
        try {
            # Try graceful shutdown first
            Stop-Process -Id $Process.Id -PassThru -ErrorAction Stop | Out-Null
            Start-Sleep -Seconds 1
            
            # Check if process still exists
            if (Get-Process -Id $Process.Id -ErrorAction SilentlyContinue) {
                Stop-Process -Id $Process.Id -PassThru -Force -ErrorAction Stop | Out-Null
            }
            
            Write-Host "Stopped $($Process.ProcessName) (PID: $($Process.Id))" -ForegroundColor Green
        }
        catch {
            Write-Host "Error stopping $($Process.ProcessName) (PID: $($Process.Id)): $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# If no Jekyll processes found, check port 4000
Write-Host "Checking for processes on port 4000..." -ForegroundColor Yellow

# Get processes listening on port 4000 using cross-platform PowerShell
try {
    $NetstatOutput = netstat -ano 2>$null | Where-Object { $_ -match ':4000\s' }
    $JekyllProcessIds = @()

    if ($NetstatOutput) {
        # Parse netstat output to get PIDs
        $NetstatOutput | ForEach-Object {
            if ($_ -match '\s+(\d+)$') {
                $JekyllProcessIds += $matches[1]
            }
        }
            
        $JekyllProcessIds = $JekyllProcessIds | Select-Object -Unique
            
        if ($JekyllProcessIds.Count -gt 0) {
            Write-Host "Found $($JekyllProcessIds.Count) process(es) on port 4000, stopping them..." -ForegroundColor Cyan
            $ProcessesFound = $true
                
            foreach ($ProcessId in $JekyllProcessIds) {
                try {
                    Stop-Process -Id $ProcessId -ErrorAction Stop | Out-Null
                    Write-Host "Stopped process (PID: $ProcessId)" -ForegroundColor Green
                }
                catch {
                    Write-Host "Error stopping process (PID: $ProcessId): $($_.Exception.Message)" -ForegroundColor Red
                }
            }
        }
    }
}
catch {
    Write-Host "Could not check port 4000 (netstat failed)" -ForegroundColor Yellow
}

if (-not $ProcessesFound) {
    Write-Host "No Jekyll processes found - already stopped" -ForegroundColor Green
}