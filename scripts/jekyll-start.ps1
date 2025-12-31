# Jekyll Development Server Script

[CmdletBinding()]
param(
    [switch]$ForceStop,
    [switch]$ForceClean,
    [switch]$BuildInsteadOfServe,
    [switch]$VerboseOutput
)

$ErrorActionPreference = "Stop"
$ProgressPreference = 'SilentlyContinue'
Set-StrictMode -Version Latest

# Load shared Jekyll helper functions
. (Join-Path $PSScriptRoot "jekyll-helpers.ps1")

# Get the root directory of the repository (parent of scripts directory)
$script:rootDir = Split-Path -Parent $PSScriptRoot

# Validate that no unknown parameters were passed
$boundParameters = $PSBoundParameters.Keys
$validParameters = @('ForceStop', 'ForceClean', 'BuildInsteadOfServe', 'VerboseOutput')
$unknownParameters = $boundParameters | Where-Object { $_ -notin $validParameters }

if ($unknownParameters) {
    Write-Host "Error: Unknown parameter(s) specified: $($unknownParameters -join ', ')" -ForegroundColor Red
    Write-Host "Valid parameters are: $($validParameters -join ', ')" -ForegroundColor Yellow
    exit 1
}

try {
    # Check if Jekyll is already running
    $jekyllStatus = Test-JekyllRunning -Cleanup
    
    if ($jekyllStatus.IsRunning) {
        if (-not $ForceStop) {
            # Jekyll is running and we're not forcing a restart - just exit
            Write-Host "✅ Jekyll is already running (Method: $($jekyllStatus.Method))" -ForegroundColor Green
            if ($jekyllStatus.Pid) {
                Write-Host "   PID: $($jekyllStatus.Pid)" -ForegroundColor Gray
            }
            Write-Host "💡 Use -ForceStop to restart Jekyll" -ForegroundColor Yellow
            exit 0
        }
        else {
            # Jekyll is running and we want to restart it
            Write-Host "Jekyll is running, restarting..." -ForegroundColor Yellow
        }
    }
    
    # Clean Jekyll files (log and PID) for fresh start - only if we're actually restarting
    if ($ForceStop) {
        Clear-JekyllFiles
    }
    
    if ($ForceStop) {
        # Stop existing Jekyll processes using separate script
        & "./scripts/jekyll-stop.ps1"
    }
    
    if ($ForceClean) {
        # Clean Jekyll
        Write-Host "Cleaning Jekyll cache..." -ForegroundColor Yellow
        & bundle exec jekyll clean
        if ($LASTEXITCODE -ne 0) {
            throw "Jekyll clean failed with exit code $LASTEXITCODE"
        }
        Write-Host "Jekyll cache cleaned successfully" -ForegroundColor Green
    }

    $jekyllArgs = @()
    if ($BuildInsteadOfServe) {
        # Build site only
        Write-Host "Only building Jekyll site..." -ForegroundColor Cyan
        $jekyllArgs = @('build')
    }
    else {
        # Start Jekyll server
        Write-Host "Building and serving Jekyll site..." -ForegroundColor Cyan
        $jekyllArgs = @('serve', "--host", "0.0.0.0", "--watch", "--force_polling", "--incremental")
    }

    # Build Jekyll command with optional verbose flag
    if ($VerboseOutput) {
        $jekyllArgs += "--verbose"
    }
    
    # Run Jekyll
    if ($BuildInsteadOfServe) {
        # For build-only mode, run in foreground
        & bundle exec jekyll @jekyllArgs
    }
    else {
        # Get paths from helper
        $paths = Get-JekyllPaths
        $logFile = $paths.LogFile
        $pidFile = $paths.PidFile
        
        # Ensure .tmp directory exists
        if (-not (Test-Path $paths.TmpDir)) {
            New-Item -ItemType Directory -Path $paths.TmpDir -Force | Out-Null
        }
        
        # Start Jekyll server in background with output redirected to log file
        Write-Host "Starting Jekyll server in background..." -ForegroundColor Cyan
        Write-Host "Log file: $logFile" -ForegroundColor Gray
        Write-Host "PID file: $pidFile" -ForegroundColor Gray
        
        # Build the command string
        $allArgs = $jekyllArgs -join " "
        $command = "bundle exec jekyll $allArgs"
        
        # Ensure parent directories exist for log and PID files
        $logDir = Split-Path $logFile -Parent
        if (-not (Test-Path $logDir)) {
            New-Item -ItemType Directory -Path $logDir -Force | Out-Null
        }
        
        # Start Jekyll as a detached process using bash, redirect output and save PID
        $bashCommand = "nohup $command > '$logFile' 2>&1 & echo `$! > '$pidFile'"
        bash -c $bashCommand
        
        # Give it a moment to start and read PID
        Start-Sleep -Seconds 2
        
        # PID file MUST exist - if not, something went wrong
        if (-not (Test-Path $pidFile)) {
            throw "Failed to create PID file at $pidFile - Jekyll may not have started correctly"
        }
        
        $jekyllPid = Get-Content $pidFile -Raw | ForEach-Object { $_.Trim() }
        if (-not $jekyllPid) {
            throw "PID file exists but is empty - Jekyll may not have started correctly"
        }
        
        # Verify the process is actually running
        $process = Get-Process -Id $jekyllPid -ErrorAction SilentlyContinue
        if (-not $process) {
            throw "Jekyll process (PID: $jekyllPid) is not running - check $logFile for errors"
        }
        
        Write-Host "✅ Jekyll server starting in background (PID: $jekyllPid)" -ForegroundColor Green
        Write-Host "⏱️  Waiting for Jekyll to become ready (timeout: 180s)..." -ForegroundColor Yellow
        
        # Poll for up to 3 minutes (180 seconds)
        $timeout = 180
        $elapsed = 0
        $checkInterval = 2
        $jekyllUrl = "http://localhost:4000"
        
        while ($elapsed -lt $timeout) {
            # First check if process is still alive
            $process = Get-Process -Id $jekyllPid -ErrorAction SilentlyContinue
            if (-not $process) {
                Write-Host "" # New line after dots
                Write-Host "❌ Jekyll process (PID: $jekyllPid) has terminated unexpectedly" -ForegroundColor Red
                Write-Host "💡 Check log file for details: $logFile" -ForegroundColor Yellow
                exit 1
            }
            
            # Check if Jekyll is responding
            try {
                $response = Invoke-WebRequest -Uri $jekyllUrl -TimeoutSec 2 -ErrorAction SilentlyContinue
                if ($response.StatusCode -eq 200) {
                    Write-Host "" # New line after dots
                    Write-Host "✅ Jekyll server is ready and accessible!" -ForegroundColor Green
                    Write-Host "🌐 Server URL: $jekyllUrl" -ForegroundColor Cyan
                    Write-Host "📋 Log file: $logFile" -ForegroundColor Gray
                    exit 0
                }
            }
            catch {
                # Not ready yet, continue waiting
            }
            
            # Show progress
            Write-Host "." -NoNewline
            
            Start-Sleep -Seconds $checkInterval
            $elapsed += $checkInterval
            
            # Show elapsed time every 10 seconds
            if ($elapsed % 10 -eq 0 -and $elapsed -lt $timeout) {
                Write-Host "" # New line
                Write-Host "Still waiting... ($elapsed/${timeout}s)" -ForegroundColor Yellow
            }
        }
        
        # Timeout reached
        Write-Host "" # New line after dots
        Write-Host "❌ Jekyll did not become ready within $timeout seconds" -ForegroundColor Red
        Write-Host "💡 Check log file for details: $logFile" -ForegroundColor Yellow
        Write-Host "💡 Jekyll process (PID: $jekyllPid) may still be building..." -ForegroundColor Yellow
        exit 1
    }
}
catch {
    Write-Host "Jekyll script error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}