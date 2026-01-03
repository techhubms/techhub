# Jekyll Helper Functions
# Shared functions for Jekyll process management across all Jekyll scripts

# Get paths for Jekyll files
function Get-JekyllPaths {
    $scriptDir = Split-Path -Parent $PSScriptRoot
    if (-not $scriptDir) {
        $scriptDir = Split-Path -Parent (Get-Location)
    }
    
    return @{
        RootDir = $scriptDir
        TmpDir = Join-Path $scriptDir ".tmp"
        LogFile = Join-Path $scriptDir ".tmp" "jekyll-log.txt"
        PidFile = Join-Path $scriptDir ".tmp" "jekyll-pid.txt"
    }
}

# Check if Jekyll is running using multiple methods
# Returns: @{ IsRunning = $true/$false; Pid = $pid or $null; Method = "description" }
function Test-JekyllRunning {
    param(
        [switch]$Cleanup
    )
    
    $paths = Get-JekyllPaths
    $result = @{
        IsRunning = $false
        Pid = $null
        Method = "none"
    }
    
    # Method 1: Check PID file
    if (Test-Path $paths.PidFile) {
        $jekyllPid = Get-Content $paths.PidFile -Raw -ErrorAction SilentlyContinue | ForEach-Object { $_.Trim() }
        
        if ($jekyllPid) {
            $process = Get-Process -Id $jekyllPid -ErrorAction SilentlyContinue
            if ($process) {
                # Process exists - verify it's actually Jekyll
                $result.IsRunning = $true
                $result.Pid = $jekyllPid
                $result.Method = "PID file"
                return $result
            } else {
                # Process is dead but PID file exists
                if ($Cleanup) {
                    Write-Host "⚠️  Found stale PID file (process $jekyllPid no longer running), cleaning up..." -ForegroundColor Yellow
                    Remove-Item $paths.PidFile -Force -ErrorAction SilentlyContinue
                }
            }
        }
    }
    
    # Method 2: Check via HTTP
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:4000" -TimeoutSec 2 -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            # Server is responding but we don't have a PID file - check netstat
            $result.IsRunning = $true
            $result.Method = "HTTP (no PID file)"
            # Try to find PID via netstat
            $netstatPid = Get-JekyllPidFromPort
            if ($netstatPid) {
                $result.Pid = $netstatPid
                $result.Method = "HTTP + netstat"
            }
            return $result
        }
    }
    catch {
        # Not accessible via HTTP
    }
    
    # Method 3: Check port 4000 using netstat (fallback)
    $netstatPid = Get-JekyllPidFromPort
    if ($netstatPid) {
        $result.IsRunning = $true
        $result.Pid = $netstatPid
        $result.Method = "netstat (port 4000)"
        return $result
    }
    
    return $result
}

# Get PID of process listening on port 4000 using netstat
function Get-JekyllPidFromPort {
    try {
        $netstatOutput = netstat -ano 2>$null | Where-Object { $_ -match ':4000\s' }
        
        if ($netstatOutput) {
            # Parse netstat output to get PID
            foreach ($line in $netstatOutput) {
                if ($line -match '\s+(\d+)$') {
                    $foundPid = $matches[1]
                    # Verify process exists
                    $process = Get-Process -Id $foundPid -ErrorAction SilentlyContinue
                    if ($process) {
                        return $foundPid
                    }
                }
            }
        }
    }
    catch {
        # netstat failed or not available
    }
    
    return $null
}

# Clean Jekyll files (logs and PID)
function Clear-JekyllFiles {
    param(
        [switch]$LogOnly,
        [switch]$PidOnly
    )
    
    $paths = Get-JekyllPaths
    
    # Ensure .tmp directory exists
    if (-not (Test-Path $paths.TmpDir)) {
        New-Item -ItemType Directory -Path $paths.TmpDir -Force | Out-Null
    }
    
    if (-not $PidOnly) {
        # Remove log file
        if (Test-Path $paths.LogFile) {
            Remove-Item $paths.LogFile -Force -ErrorAction SilentlyContinue
        }
    }
    
    if (-not $LogOnly) {
        # Remove PID file
        if (Test-Path $paths.PidFile) {
            Remove-Item $paths.PidFile -Force -ErrorAction SilentlyContinue
        }
    }
}

# Stop Jekyll process
function Stop-Jekyll {
    param(
        [int]$ProcessId
    )
    
    if (-not $ProcessId) {
        return $false
    }
    
    try {
        $process = Get-Process -Id $ProcessId -ErrorAction SilentlyContinue
        if ($process) {
            Write-Host "Stopping Jekyll process (PID: $ProcessId)..." -ForegroundColor Yellow
            Stop-Process -Id $ProcessId -Force -ErrorAction SilentlyContinue
            Start-Sleep -Seconds 2
            
            # Verify it's stopped
            $stillRunning = Get-Process -Id $ProcessId -ErrorAction SilentlyContinue
            if (-not $stillRunning) {
                Write-Host "✅ Jekyll process stopped" -ForegroundColor Green
                return $true
            }
        }
    }
    catch {
        Write-Host "Could not stop process $ProcessId : $($_.Exception.Message)" -ForegroundColor Yellow
    }
    
    return $false
}
