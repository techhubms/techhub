#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Runs the Tech Hub application (API + Web) with various options.

.DESCRIPTION
    This script provides a convenient way to build, test, and run the Tech Hub .NET application
    from the command line. It supports cleaning, building, testing, and running both the API
    and Web projects.

.PARAMETER Clean
    Clean all build artifacts before building.

.PARAMETER Build
    Only build the solution without running the application.

.PARAMETER Test
    Run all tests before starting the application.

.PARAMETER SkipBuild
    Skip the build step and run the application with existing binaries.

.PARAMETER ApiOnly
    Only run the API project.

.PARAMETER WebOnly
    Only run the Web project.

.PARAMETER ApiPort
    Port for the API server (default: 5029).

.PARAMETER WebPort
    Port for the Web server (default: 5184).

.PARAMETER Release
    Build and run in Release configuration instead of Debug.

.PARAMETER VerboseOutput
    Show verbose output from dotnet commands.

.EXAMPLE
    ./run.ps1
    Builds and runs both API and Web projects.

.EXAMPLE
    ./run.ps1 -Clean -Test
    Cleans, builds, runs all tests, then starts both projects.

.EXAMPLE
    ./run.ps1 -Build
    Only builds the solution without running.

.EXAMPLE
    ./run.ps1 -ApiOnly -ApiPort 8080
    Only runs the API on port 8080.

.EXAMPLE
    ./run.ps1 -SkipBuild
    Runs both projects with existing binaries.

.NOTES
    Author: Tech Hub Team
    Requires: .NET 10 SDK, PowerShell 7+
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [switch]$Clean,

    [Parameter(Mandatory = $false)]
    [switch]$Build,

    [Parameter(Mandatory = $false)]
    [switch]$Test,

    [Parameter(Mandatory = $false)]
    [switch]$SkipBuild,

    [Parameter(Mandatory = $false)]
    [switch]$ApiOnly,

    [Parameter(Mandatory = $false)]
    [switch]$WebOnly,

    [Parameter(Mandatory = $false)]
    [int]$ApiPort = 5029,

    [Parameter(Mandatory = $false)]
    [int]$WebPort = 5184,

    [Parameter(Mandatory = $false)]
    [switch]$Release,

    [Parameter(Mandatory = $false)]
    [switch]$VerboseOutput
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Determine workspace root
$workspaceRoot = $PSScriptRoot

# Project paths
$solutionPath = Join-Path $workspaceRoot "TechHub.slnx"
$apiProjectPath = Join-Path $workspaceRoot "src/TechHub.Api/TechHub.Api.csproj"
$webProjectPath = Join-Path $workspaceRoot "src/TechHub.Web/TechHub.Web.csproj"
$e2eTestProjectPath = Join-Path $workspaceRoot "tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj"

# Build configuration
$configuration = if ($Release) { "Release" } else { "Debug" }

# Verbosity level
$verbosityLevel = if ($VerboseOutput) { "detailed" } else { "minimal" }

# Color output helpers
function Write-Step {
    param([string]$Message)
    Write-Host "`n==> $Message" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Error {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Info {
    param([string]$Message)
    Write-Host "  $Message" -ForegroundColor Gray
}

# Validate prerequisites
function Test-Prerequisites {
    Write-Step "Checking prerequisites"
    
    # Check .NET SDK
    try {
        $dotnetVersion = dotnet --version
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet command failed with exit code $LASTEXITCODE"
        }
        Write-Info ".NET SDK version: $dotnetVersion"
    }
    catch {
        Write-Error ".NET SDK not found. Please install .NET 10 SDK."
        exit 1
    }
    
    # Check solution file
    if (-not (Test-Path $solutionPath)) {
        Write-Error "Solution file not found: $solutionPath"
        exit 1
    }
    
    Write-Success "Prerequisites validated"
}

# Clean build artifacts
function Invoke-Clean {
    Write-Step "Cleaning build artifacts"
    
    dotnet clean $solutionPath --configuration $configuration --verbosity $verbosityLevel
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Clean failed with exit code $LASTEXITCODE"
        exit 1
    }
    Write-Success "Clean completed"
}

# Build solution
function Invoke-Build {
    Write-Step "Building solution ($configuration)"
    
    dotnet build $solutionPath --configuration $configuration --verbosity $verbosityLevel
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build failed with exit code $LASTEXITCODE"
        exit 1
    }
    Write-Success "Build completed"
}

# Run tests
function Invoke-Tests {
    param(
        [int]$ApiPort = 5029,
        [int]$WebPort = 5184
    )
    
    # PHASE 1: Run all non-E2E tests first (fast, no server needed)
    Write-Step "Running unit and integration tests"
    Write-Host ""
    
    $nonE2eTestArgs = @(
        "test",
        $solutionPath,
        "--configuration", $configuration,
        "--no-build",
        "--verbosity", "detailed",
        "--filter", "FullyQualifiedName!~E2E",  # Exclude E2E tests
        "--blame-hang-timeout", "5m"
    )
    
    & dotnet @nonE2eTestArgs
    
    $nonE2eExitCode = $LASTEXITCODE
    Write-Host ""
    
    if ($nonE2eExitCode -ne 0) {
        Write-Error "Unit/Integration tests failed with exit code $nonE2eExitCode"
        exit 1
    }
    
    Write-Success "Unit and integration tests passed"
    
    # PHASE 2: Start servers for E2E tests
    Write-Step "Starting servers for E2E tests"
    
    $apiUrl = "http://localhost:$ApiPort"
    $webUrl = "http://localhost:$WebPort"
    
    # Create logs directory for server output
    $logsDir = Join-Path $PSScriptRoot ".tmp/test-logs"
    if (-not (Test-Path $logsDir)) {
        New-Item -ItemType Directory -Path $logsDir -Force | Out-Null
    }
    
    $apiLogPath = Join-Path $logsDir "api.log"
    $webLogPath = Join-Path $logsDir "web.log"
    
    Write-Info "Server output will be logged to .tmp/test-logs/"
    
    # Track processes for cleanup
    $script:testApiProcess = $null
    $script:testWebProcess = $null
    $apiLogWriter = $null
    $webLogWriter = $null
    
    # Start API process with Test environment (suppresses console logging via appsettings.Test.json)
    $apiStartInfo = New-Object System.Diagnostics.ProcessStartInfo
    $apiStartInfo.FileName = "dotnet"
    $apiStartInfo.Arguments = "run --project `"$apiProjectPath`" --no-build --no-launch-profile --configuration $configuration"
    $apiStartInfo.WorkingDirectory = Split-Path $apiProjectPath -Parent
    $apiStartInfo.UseShellExecute = $false
    $apiStartInfo.CreateNoWindow = $false
    $apiStartInfo.RedirectStandardOutput = $true
    $apiStartInfo.RedirectStandardError = $true
    $apiStartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = "Test"
    $apiStartInfo.EnvironmentVariables["ASPNETCORE_URLS"] = $apiUrl
    
    $script:testApiProcess = [System.Diagnostics.Process]::Start($apiStartInfo)
    
    # Redirect API output (stdout + stderr) to log file
    $apiLogWriter = [System.IO.StreamWriter]::new($apiLogPath, $false)
    $apiLogWriter.AutoFlush = $true
    $script:apiOutputJob = Start-ThreadJob -ScriptBlock {
        param($stdoutReader, $stderrReader, $writer)
        $jobs = @(
            (Start-ThreadJob -ScriptBlock { param($r, $w) while ($null -ne ($line = $r.ReadLine())) { $w.WriteLine($line) } } -ArgumentList $stdoutReader, $writer),
            (Start-ThreadJob -ScriptBlock { param($r, $w) while ($null -ne ($line = $r.ReadLine())) { $w.WriteLine("ERR: $line") } } -ArgumentList $stderrReader, $writer)
        )
        $jobs | Wait-Job | Out-Null
    } -ArgumentList $script:testApiProcess.StandardOutput, $script:testApiProcess.StandardError, $apiLogWriter
    
    Write-Info "  API started (PID: $($script:testApiProcess.Id))"
    
    # Start Web process with Test environment (suppresses console logging via appsettings.Test.json)
    $webStartInfo = New-Object System.Diagnostics.ProcessStartInfo
    $webStartInfo.FileName = "dotnet"
    $webStartInfo.Arguments = "run --project `"$webProjectPath`" --no-build --no-launch-profile --configuration $configuration"
    $webStartInfo.WorkingDirectory = Split-Path $webProjectPath -Parent
    $webStartInfo.UseShellExecute = $false
    $webStartInfo.CreateNoWindow = $false
    $webStartInfo.RedirectStandardOutput = $true
    $webStartInfo.RedirectStandardError = $true
    $webStartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = "Test"
    $webStartInfo.EnvironmentVariables["ASPNETCORE_URLS"] = $webUrl
    
    $script:testWebProcess = [System.Diagnostics.Process]::Start($webStartInfo)
    
    # Redirect Web output (stdout + stderr) to log file
    $webLogWriter = [System.IO.StreamWriter]::new($webLogPath, $false)
    $webLogWriter.AutoFlush = $true
    $script:webOutputJob = Start-ThreadJob -ScriptBlock {
        param($stdoutReader, $stderrReader, $writer)
        $jobs = @(
            (Start-ThreadJob -ScriptBlock { param($r, $w) while ($null -ne ($line = $r.ReadLine())) { $w.WriteLine($line) } } -ArgumentList $stdoutReader, $writer),
            (Start-ThreadJob -ScriptBlock { param($r, $w) while ($null -ne ($line = $r.ReadLine())) { $w.WriteLine("ERR: $line") } } -ArgumentList $stderrReader, $writer)
        )
        $jobs | Wait-Job | Out-Null
    } -ArgumentList $script:testWebProcess.StandardOutput, $script:testWebProcess.StandardError, $webLogWriter
    
    Write-Info "  Web started (PID: $($script:testWebProcess.Id))"
    
    # Wait for servers to be ready
    Write-Info "Waiting for servers to start..."
    $maxAttempts = 5
    $attempt = 0
    $apiReady = $false
    $webReady = $false
    
    while ($attempt -lt $maxAttempts -and (-not $apiReady -or -not $webReady)) {
        Start-Sleep -Seconds 1
        $attempt++
        
        if (-not $apiReady) {
            try {
                $response = Invoke-WebRequest -Uri "$apiUrl/health" -Method Get -TimeoutSec 2 -ErrorAction SilentlyContinue
                if ($response.StatusCode -eq 200) {
                    $apiReady = $true
                    Write-Info "  API ready ✓"
                }
            } catch {
                # Still waiting
            }
        }
        
        if (-not $webReady) {
            try {
                $response = Invoke-WebRequest -Uri $webUrl -Method Get -TimeoutSec 2 -ErrorAction SilentlyContinue
                if ($response.StatusCode -eq 200) {
                    $webReady = $true
                    Write-Info "  Web ready ✓"
                }
            } catch {
                # Still waiting
            }
        }
    }
    
    if (-not $apiReady -or -not $webReady) {
        Write-Error "Servers failed to start within $maxAttempts seconds"
        Write-Info "Check logs for details:"
        Write-Info "  API: $apiLogPath"
        Write-Info "  Web: $webLogPath"
        
        # Clean up on startup failure
        if ($null -ne $script:testApiProcess -and -not $script:testApiProcess.HasExited) {
            $script:testApiProcess.Kill($true)
            $script:testApiProcess.Dispose()
        }
        if ($null -ne $script:testWebProcess -and -not $script:testWebProcess.HasExited) {
            $script:testWebProcess.Kill($true)
            $script:testWebProcess.Dispose()
        }
        Stop-ExistingProcesses -Ports @($ApiPort, $WebPort)
        exit 1
    }
    
    Write-Success "Servers ready"
    
    # PHASE 3: Run E2E tests only
    Write-Step "Running E2E tests"
    Write-Host ""
    
    $e2eTestArgs = @(
        "test",
        $e2eTestProjectPath,
        "--configuration", $configuration,
        "--no-build",
        "--verbosity", "detailed",
        "--blame-hang-timeout", "5m"
    )
    
    & dotnet @e2eTestArgs
    
    $e2eExitCode = $LASTEXITCODE
    
    Write-Host ""
    Write-Host "════════════════════════════════════════" -ForegroundColor Cyan
    
    Write-Info "Server logs:"
    Write-Info "  API: $apiLogPath"
    Write-Info "  Web: $webLogPath"
    Write-Host ""
    
    if ($e2eExitCode -ne 0) {
        # E2E tests failed - clean up servers
        Write-Error "E2E tests failed with exit code $e2eExitCode"
        
        # Stop servers on failure
        Write-Info "Stopping test servers..."
        
        if ($null -ne $script:testApiProcess -and -not $script:testApiProcess.HasExited) {
            $script:testApiProcess.Kill($true)
            $script:testApiProcess.WaitForExit(2000)
            $script:testApiProcess.Dispose()
        }
        
        if ($null -ne $script:testWebProcess -and -not $script:testWebProcess.HasExited) {
            $script:testWebProcess.Kill($true)
            $script:testWebProcess.WaitForExit(2000)
            $script:testWebProcess.Dispose()
        }
        
        Stop-ExistingProcesses -Ports @($ApiPort, $WebPort)
        exit 1
    }
    
    # All tests passed! Leave servers running
    Write-Success "All tests passed! Servers are running:"
    Write-Host ""
    Write-Info "  API: $apiUrl (Swagger: $apiUrl/swagger)"
    Write-Info "  Web: $webUrl"
    Write-Host ""
    Write-Info "Server logs:"
    Write-Info "  API: $apiLogPath"
    Write-Info "  Web: $webLogPath"
    Write-Host ""
    Write-Host "Press Ctrl+C to stop servers when done" -ForegroundColor Yellow
    
    # Wait for user to stop (Ctrl+C)
    try {
        while ($true) {
            if ($script:testApiProcess.HasExited -or $script:testWebProcess.HasExited) {
                Write-Error "`nOne or more servers stopped unexpectedly"
                break
            }
            Start-Sleep -Milliseconds 500
        }
    }
    finally {
        # Clean up when user presses Ctrl+C or process exits
        Write-Info "`nStopping servers..."
        
        if ($null -ne $script:testApiProcess -and -not $script:testApiProcess.HasExited) {
            $script:testApiProcess.Kill($true)
            $script:testApiProcess.WaitForExit(2000)
            $script:testApiProcess.Dispose()
        }
        
        if ($null -ne $script:testWebProcess -and -not $script:testWebProcess.HasExited) {
            $script:testWebProcess.Kill($true)
            $script:testWebProcess.WaitForExit(2000)
            $script:testWebProcess.Dispose()
        }
        
        Stop-ExistingProcesses -Ports @($ApiPort, $WebPort)
        Write-Success "Servers stopped"
    }
}

# Kill existing processes on ports
function Stop-ExistingProcesses {
    param(
        [int[]]$Ports
    )
    
    $stoppedAny = $false
    foreach ($port in $Ports) {
        try {
            $processIds = lsof -ti ":$port" 2>$null
            if ($processIds) {
                if (-not $stoppedAny) {
                    Write-Step "Cleaning up existing processes"
                    $stoppedAny = $true
                }
                Write-Info "Port $port is in use, killing process(es): $($processIds -join ', ')"
                # Use SIGKILL for immediate termination
                $processIds | ForEach-Object { 
                    kill -9 $_ 2>$null
                }
                # Brief wait to ensure ports are released
                Start-Sleep -Milliseconds 200
            }
        }
        catch {
            # Ignore errors if no process found
        }
    }
    
    if ($stoppedAny) {
        Write-Success "Port cleanup completed"
    }
}

# Run API project
function Start-ApiProject {
    param([int]$Port)
    
    Write-Step "Starting API (http://localhost:$Port)"
    
    $apiUrl = "http://localhost:$Port"
    
    try {
        # Navigate to API directory and run
        Push-Location (Join-Path $workspaceRoot "src/TechHub.Api")
        
        $env:ASPNETCORE_ENVIRONMENT = "Development"
        $env:ASPNETCORE_URLS = $apiUrl
        
        Write-Info "API running at: $apiUrl"
        Write-Info "Swagger UI: $apiUrl/swagger"
        Write-Info "Hot reload enabled - changes will auto-reload"
        Write-Info "Press Ctrl+C to stop"
        
        dotnet watch --project $apiProjectPath --configuration $configuration
    }
    finally {
        Pop-Location
    }
}

# Run Web project
function Start-WebProject {
    param([int]$Port)
    
    Write-Step "Starting Web (http://localhost:$Port)"
    
    $webUrl = "http://localhost:$Port"
    
    try {
        # Navigate to Web directory and run
        Push-Location (Join-Path $workspaceRoot "src/TechHub.Web")
        
        $env:ASPNETCORE_ENVIRONMENT = "Development"
        
        Write-Info "Web running at: $webUrl"
        Write-Info "Hot reload enabled - changes will auto-reload"
        Write-Info "Press Ctrl+C to stop"
        
        dotnet watch --project $webProjectPath --configuration $configuration
    }
    finally {
        Pop-Location
    }
}

# Run both projects
function Start-BothProjects {
    param(
        [int]$ApiPort,
        [int]$WebPort
    )
    
    Write-Step "Starting both API and Web projects"
    
    $apiUrl = "http://localhost:$ApiPort"
    $webUrl = "http://localhost:$WebPort"
    
    Write-Info "API: $apiUrl (Swagger: $apiUrl/swagger)"
    Write-Info "Web: $webUrl"
    Write-Info "Press Ctrl+C to stop all"
    
    # Track processes for cleanup
    $script:apiProcess = $null
    $script:webProcess = $null
    
    try {
        # Start API process
        $apiStartInfo = New-Object System.Diagnostics.ProcessStartInfo
        $apiStartInfo.FileName = "dotnet"
        $apiStartInfo.Arguments = "run --project `"$apiProjectPath`" --no-build --configuration $configuration"
        $apiStartInfo.WorkingDirectory = Split-Path $apiProjectPath -Parent
        $apiStartInfo.UseShellExecute = $false
        $apiStartInfo.CreateNoWindow = $false
        $apiStartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = "Development"
        $apiStartInfo.EnvironmentVariables["ASPNETCORE_URLS"] = $apiUrl
        
        $script:apiProcess = [System.Diagnostics.Process]::Start($apiStartInfo)
        Write-Info "API process started (PID: $($script:apiProcess.Id))"
        
        # Wait for API to start
        Write-Info "Waiting for API to start..."
        Start-Sleep -Seconds 3
        
        # Start Web process
        $webStartInfo = New-Object System.Diagnostics.ProcessStartInfo
        $webStartInfo.FileName = "dotnet"
        $webStartInfo.Arguments = "run --project `"$webProjectPath`" --no-build --configuration $configuration"
        $webStartInfo.WorkingDirectory = Split-Path $webProjectPath -Parent
        $webStartInfo.UseShellExecute = $false
        $webStartInfo.CreateNoWindow = $false
        $webStartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = "Development"
        
        $script:webProcess = [System.Diagnostics.Process]::Start($webStartInfo)
        Write-Info "Web process started (PID: $($script:webProcess.Id))"
        
        # Wait for Web to start
        Write-Info "Waiting for Web to start..."
        Start-Sleep -Seconds 3
        
        Write-Success "Both projects started"
        Write-Info "`nPress Ctrl+C to stop all processes"
        
        # Wait for processes to exit
        while ($true) {
            if ($script:apiProcess.HasExited -or $script:webProcess.HasExited) {
                Write-Error "`nOne or more projects stopped unexpectedly"
                break
            }
            Start-Sleep -Milliseconds 500
        }
    }
    finally {
        # Clean up processes
        Write-Info "`nStopping projects..."
        
        # Kill processes immediately
        if ($null -ne $script:apiProcess -and -not $script:apiProcess.HasExited) {
            Write-Info "Stopping API (PID: $($script:apiProcess.Id))..."
            $script:apiProcess.Kill($true)  # Kill entire process tree
            $script:apiProcess.WaitForExit(2000)  # Wait max 2 seconds
            $script:apiProcess.Dispose()
        }
        
        if ($null -ne $script:webProcess -and -not $script:webProcess.HasExited) {
            Write-Info "Stopping Web (PID: $($script:webProcess.Id))..."
            $script:webProcess.Kill($true)  # Kill entire process tree
            $script:webProcess.WaitForExit(2000)  # Wait max 2 seconds
            $script:webProcess.Dispose()
        }
        
        # Final cleanup - kill any remaining processes on the ports
        Stop-ExistingProcesses -Ports @($ApiPort, $WebPort)
        
        Write-Success "All projects stopped"
    }
}

# Main execution
try {
    # Ensure we're in the workspace root
    Set-Location $workspaceRoot
    
    Write-Host "`n╔════════════════════════════════════════╗" -ForegroundColor Cyan
    Write-Host "║      Tech Hub Development Runner      ║" -ForegroundColor Cyan
    Write-Host "╚════════════════════════════════════════╝`n" -ForegroundColor Cyan
    
    # Validate prerequisites
    Test-Prerequisites
    
    # Stop existing processes
    $portsToCheck = @()
    if (-not $WebOnly) { $portsToCheck += $ApiPort }
    if (-not $ApiOnly) { $portsToCheck += $WebPort }
    
    if ($portsToCheck.Count -gt 0) {
        Stop-ExistingProcesses -Ports $portsToCheck
    }
    
    # Clean if requested
    if ($Clean) {
        Invoke-Clean
    }
    
    # Build if needed
    if (-not $SkipBuild) {
        Invoke-Build
    }
    
    # Test if requested - run tests FIRST before starting servers
    if ($Test) {
        Invoke-Tests -ApiPort $ApiPort -WebPort $WebPort
        # If tests pass, continue to start servers normally (unless Build-only mode)
        Write-Success "`nAll tests passed! Starting servers...`n"
    }
    
    # If only build was requested, exit here
    if ($Build) {
        Write-Success "`nBuild completed successfully!"
        exit 0
    }
    
    # Run projects
    if ($ApiOnly) {
        Start-ApiProject -Port $ApiPort
    }
    elseif ($WebOnly) {
        Start-WebProject -Port $WebPort
    }
    else {
        Start-BothProjects -ApiPort $ApiPort -WebPort $WebPort
    }
}
catch {
    Write-Error "`nScript failed: $($_.Exception.Message)"
    Write-Host $_.ScriptStackTrace -ForegroundColor DarkGray
    exit 1
}
finally {
    # Always return to workspace root
    Set-Location $workspaceRoot
}
