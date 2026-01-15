#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Tech Hub PowerShell Module - Provides the Run function for easy development workflow.

.DESCRIPTION
    This module exports the 'Run' function which provides a convenient way to build, test,
    and run the Tech Hub .NET application. It wraps the functionality into
    a reusable PowerShell function that auto-loads in terminal sessions.

.NOTES
    Author: Tech Hub Team
    Requires: .NET 10 SDK, PowerShell 7+
#>

function Run {
    <#
    .SYNOPSIS
        Runs the Tech Hub application (API + Web) with various options.

    .DESCRIPTION
        This function provides a convenient way to build, test, and run the Tech Hub .NET application
        from the command line. It supports cleaning, building, testing, and running both the API
        and Web projects. Uses .NET Aspire for orchestration, service discovery, and observability.

    .PARAMETER Clean
        Clean all build artifacts before building.

    .PARAMETER Build
        Only build the solution without running the application.

    .PARAMETER Test
        Run all tests before starting the application (servers keep running after tests).

    .PARAMETER SkipTests
        Skip tests and start servers directly (for interactive debugging with Playwright MCP).
        
        Use this for:
        - AI agents doing interactive debugging with Playwright MCP tools
        - Humans manually testing or using Playwright MCP
        - Fast startup when you don't need to verify all tests first

    .PARAMETER OnlyTests
        Run all tests and exit without starting servers (for CI/automated testing).
        
        Use this for:
        - Automated verification that all changes work correctly
        - CI/CD pipelines
        - Final verification before committing changes

    .EXAMPLE
        Run
        Default: Builds, runs all tests, then starts both API and Web projects.

    .EXAMPLE
        Run -SkipTests
        Builds and starts servers without tests (for interactive debugging).
        AI agents: Use this with Playwright MCP for fast interactive testing!
        Humans: Use this for manual testing or Playwright MCP exploration.

    .EXAMPLE
        Run -OnlyTests
        Builds, runs all tests, then exits (for automated verification).
        Use this when you want to verify all changes work correctly.

    .EXAMPLE
        Run -Clean -Test
        Cleans, builds, runs all tests, then starts both projects.

    .EXAMPLE
        Run -Build
        Only builds the solution without running.

    .NOTES
        Author: Tech Hub Team
        Requires: .NET 10 SDK, PowerShell 7+
        Optional: Docker (for Aspire Dashboard)
    #>
    
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false)]
        [switch]$Help,

        [Parameter(Mandatory = $false)]
        [switch]$Clean,

        [Parameter(Mandatory = $false)]
        [switch]$Build,

        [Parameter(Mandatory = $false)]
        [switch]$Test,

        [Parameter(Mandatory = $false)]
        [switch]$SkipTests,

        [Parameter(Mandatory = $false)]
        [switch]$OnlyTests
    )

    # Show help if requested
    if ($Help) {
        Write-Host "`n╔════════════════════════════════════════╗" -ForegroundColor Cyan
        Write-Host "║      Tech Hub Development Runner       ║" -ForegroundColor Cyan
        Write-Host "╚════════════════════════════════════════╝`n" -ForegroundColor Cyan
        
        Write-Host "USAGE:" -ForegroundColor Yellow
        Write-Host "  Run [options]`n" -ForegroundColor White
        
        Write-Host "OPTIONS:" -ForegroundColor Yellow
        Write-Host "  -Help          Show this help message" -ForegroundColor White
        Write-Host "  -Build         Build only, don't run servers" -ForegroundColor White
        Write-Host "  -Clean         Clean build artifacts before building" -ForegroundColor White
        Write-Host "  -Test          Run all tests before starting servers" -ForegroundColor White
        Write-Host "  -SkipTests     Skip tests, start servers directly" -ForegroundColor White
        Write-Host "  -OnlyTests     Run tests only, then exit`n" -ForegroundColor White
        
        Write-Host "EXAMPLES:" -ForegroundColor Yellow
        Write-Host "  Run                    Build, test, and start servers (default)" -ForegroundColor Gray
        Write-Host "  Run -OnlyTests         Run all tests, then exit (for verification)" -ForegroundColor Gray
        Write-Host "  Run -SkipTests         Skip tests, start servers (for debugging)" -ForegroundColor Gray
        Write-Host "  Run -Clean             Clean build before starting" -ForegroundColor Gray
        Write-Host "  Run -Build             Build only, don't run servers`n" -ForegroundColor Gray
        
        Write-Host "COMMON WORKFLOWS:" -ForegroundColor Yellow
        Write-Host "  Automated testing:     Run -OnlyTests" -ForegroundColor Gray
        Write-Host "  Interactive debugging: Run -SkipTests" -ForegroundColor Gray
        Write-Host "  Full clean build:      Run -Clean -OnlyTests`n" -ForegroundColor Gray
        
        Write-Host "SERVICES:" -ForegroundColor Yellow
        Write-Host "  API:       http://localhost:5029 (Swagger: /swagger)" -ForegroundColor Gray
        Write-Host "  Web:       http://localhost:5184" -ForegroundColor Gray
        Write-Host "  Dashboard: https://localhost:17101 (Aspire Dashboard)`n" -ForegroundColor Gray
        
        Write-Host "For detailed help: " -NoNewline -ForegroundColor White
        Write-Host "Get-Help Run -Full`n" -ForegroundColor Cyan
        
        return
    }

    $ErrorActionPreference = "Stop"
    Set-StrictMode -Version Latest

    # Script-level variables for process management
    $script:appHostProcess = $null

    # Determine workspace root - navigate up from scripts directory
    $workspaceRoot = Split-Path $PSScriptRoot -Parent

    # Solution file path
    $solutionPath = Join-Path $workspaceRoot "TechHub.slnx"

    # Project paths - for running/watching specific projects
    $appHostProjectPath = Join-Path $workspaceRoot "src/TechHub.AppHost/TechHub.AppHost.csproj"

    # Test project path for E2E (only needed separately)
    $e2eTestProjectPath = Join-Path $workspaceRoot "tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj"

    # Build configuration (always Debug for development)
    $configuration = "Debug"

    # Verbosity level (always minimal for clean output)
    $verbosityLevel = "minimal"

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
        
        # Check that solution file exists
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
            [switch]$OnlyTests
        )
        
        # PHASE 1: Run all non-E2E tests first (fast, no server needed)
        Write-Step "Running unit and integration tests"
        Write-Host ""
        
        # Run all tests except E2E using solution-level test with filter
        # Integration tests use WebApplicationFactory which manages its own environment
        $testArgs = @(
            "test",
            $solutionPath,
            "--configuration", $configuration,
            "--no-build",
            "--filter", "FullyQualifiedName!~E2E",
            "--settings", (Join-Path $workspaceRoot ".runsettings"),
            "--blame-hang-timeout", "1m"
        )
        
        & dotnet @testArgs
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host ""
            Write-Error "Unit/integration tests failed with exit code $LASTEXITCODE"
            exit 1
        }
        
        Write-Host ""
        Write-Success "Unit and integration tests passed"
        
        # PHASE 2: Start AppHost for E2E tests
        Write-Step "Starting Aspire AppHost for E2E tests"
        
        # Start AppHost in background with output capture
        $script:appHostProcess = $null
        $script:dashboardUrl = $null
        
        $appHostStartInfo = New-Object System.Diagnostics.ProcessStartInfo
        $appHostStartInfo.FileName = "dotnet"
        $appHostStartInfo.Arguments = "run --project `"$appHostProjectPath`" --no-build --configuration $configuration"
        $appHostStartInfo.WorkingDirectory = Split-Path $appHostProjectPath -Parent
        $appHostStartInfo.UseShellExecute = $false
        $appHostStartInfo.RedirectStandardOutput = $false
        $appHostStartInfo.RedirectStandardError = $false
        $appHostStartInfo.CreateNoWindow = $false
        # Use Test environment for E2E tests - API/Web will load appsettings.Test.json
        # This provides cleaner console output (Error only) and proper test log files
        $appHostStartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = "Test"
        # Keep Aspire orchestration logs (useful for debugging startup issues)
        # but suppress noisy Microsoft.AspNetCore.* framework logs
        $appHostStartInfo.EnvironmentVariables["Logging__Console__LogLevel__Microsoft"] = "Warning"
        $appHostStartInfo.EnvironmentVariables["Logging__Console__LogLevel__Microsoft.AspNetCore"] = "Warning"
        # Disable Aspire dashboard authentication for local development (no login token required)
        $appHostStartInfo.EnvironmentVariables["DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS"] = "true"
        # Disable automatic browser launch in DevContainer (prevents DBus errors)
        $appHostStartInfo.EnvironmentVariables["DOTNET_DASHBOARD_OPEN_BROWSER"] = "false"
        
        $script:appHostProcess = [System.Diagnostics.Process]::Start($appHostStartInfo)
        
        Write-Info "  AppHost started (PID: $($script:appHostProcess.Id))"
        
        # Wait for services to be ready (Aspire orchestration is slower than direct dotnet watch)
        Write-Info "Waiting for services to start (Aspire orchestration can take 30-60 seconds)..."
        $maxAttempts = 60
        $attempt = 0
        $apiReady = $false
        $webReady = $false
        $lastApiError = ""
        $lastWebError = ""
        
        while ($attempt -lt $maxAttempts -and (-not $apiReady -or -not $webReady)) {
            Start-Sleep -Seconds 1
            $attempt++
            
            # Show progress every 10 seconds with diagnostic info
            if ($attempt % 10 -eq 0) {
                Write-Info "  Still waiting... ($attempt seconds elapsed)"
                if (-not $apiReady -and $lastApiError) {
                    Write-Info "    API: $lastApiError"
                }
                if (-not $webReady -and $lastWebError) {
                    Write-Info "    Web: $lastWebError"
                }
            }
            
            # Check if AppHost is still alive
            if ($script:appHostProcess.HasExited) {
                Write-Error "AppHost process exited unexpectedly (exit code: $($script:appHostProcess.ExitCode))"
                Stop-ExistingProcesses
                exit 1
            }
            
            if (-not $apiReady) {
                # Use HTTPS endpoint (Aspire configures HTTPS by default)
                # -k ignores self-signed certificate validation for local dev
                $curlOutput = curl -s -k -m 2 -w "\n%{http_code}" "https://localhost:7153/health" 2>&1
                $exitCode = $LASTEXITCODE
                
                if ($exitCode -eq 0 -and $curlOutput -match "200$") {
                    $apiReady = $true
                    Write-Info "  API ready ✓"
                }
                elseif ($exitCode -ne 0) {
                    $lastApiError = "Connection failed (exit code: $exitCode)"
                }
                else {
                    $httpCode = ($curlOutput -split "`n")[-1]
                    $lastApiError = "HTTP $httpCode"
                }
            }
            
            if (-not $webReady) {
                # Use HTTPS endpoint (Aspire configures HTTPS by default)
                # -k ignores self-signed certificate validation for local dev
                $curlOutput = curl -s -k -m 2 -w "\n%{http_code}" "https://localhost:7190/health" 2>&1
                $exitCode = $LASTEXITCODE
                
                if ($exitCode -eq 0 -and $curlOutput -match "200$") {
                    $webReady = $true
                    Write-Info "  Web ready ✓"
                }
                elseif ($exitCode -ne 0) {
                    $lastWebError = "Connection failed (exit code: $exitCode)"
                }
                else {
                    $httpCode = ($curlOutput -split "`n")[-1]
                    $lastWebError = "HTTP $httpCode"
                }
            }
        }
        
        if (-not $apiReady -or -not $webReady) {
            Write-Error "Services failed to start within $maxAttempts seconds"
            if (-not $apiReady) {
                Write-Info "  API health check failed at https://localhost:7153/health ($lastApiError)"
            }
            if (-not $webReady) {
                Write-Info "  Web health check failed at https://localhost:7190/health ($lastWebError)"
            }
            Write-Info "  Check Aspire Dashboard logs at the URL shown above"
            Write-Info "  Try manually: curl -k https://localhost:7153/health"
            
            # Clean up on startup failure
            if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                $script:appHostProcess.Kill($true)
                $script:appHostProcess.Dispose()
            }
            Stop-ExistingProcesses
            exit 1
        }
        
        Write-Success "Services ready"
        
        # PHASE 3: Run E2E tests
        Write-Step "Running E2E tests"
        Write-Host ""
        
        # Optimize thread count based on environment
        if ($env:CI) {
            Write-Info "CI environment detected - using default 4 threads from xunit.runner.json"
        }
        else {
            Write-Info "Local environment - using 8 threads for faster execution"
            $env:XUNIT_MAX_PARALLEL_THREADS = 8
        }
        
        $e2eTestArgs = @(
            "test",
            $e2eTestProjectPath,
            "--configuration", $configuration,
            "--no-build",
            "--settings", (Join-Path $workspaceRoot ".runsettings"),
            "--logger", "console;verbosity=detailed",
            "--blame-hang-timeout", "1m"
        )
        
        & dotnet @e2eTestArgs
        
        $e2eExitCode = $LASTEXITCODE
        
        Write-Host ""
        Write-Host "════════════════════════════════════════" -ForegroundColor Cyan
        Write-Host ""
        
        if ($e2eExitCode -ne 0) {
            # E2E tests failed - clean up AppHost
            Write-Error "E2E tests failed with exit code $e2eExitCode"
            
            Write-Info "Stopping AppHost..."
            
            if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                $script:appHostProcess.Kill($true)
                $script:appHostProcess.WaitForExit(2000)
                $script:appHostProcess.Dispose()
            }
            
            Stop-ExistingProcesses
            exit 1
        }
        
        Write-Success "All tests passed!"
        
        # OnlyTests mode: Stop AppHost and exit
        if ($OnlyTests) {
            Write-Info "Stopping AppHost..."
            
            if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                $script:appHostProcess.Kill($true)
                $script:appHostProcess.WaitForExit(2000)
                $script:appHostProcess.Dispose()
            }
            
            Stop-ExistingProcesses
            return
        }
        
        # Default mode: Keep AppHost running, show URLs
        Write-Host ""
        Write-Info "AppHost is running:"
        Write-Info "  API: https://localhost:7153 (Swagger: https://localhost:7153/swagger)"
        Write-Info "  Web: https://localhost:7190"
        Write-Info "  Dashboard: https://localhost:17101 (no login required)"
        Write-Host ""
        Write-Host "Press Ctrl+C to stop" -ForegroundColor Yellow
        
        # Wait for user to stop (Ctrl+C)
        try {
            while ($true) {
                if ($script:appHostProcess.HasExited) {
                    Write-Error "`nAppHost stopped unexpectedly"
                    break
                }
                Start-Sleep -Milliseconds 500
            }
        }
        finally {
            # Clean up when user presses Ctrl+C
            Write-Info "`nStopping AppHost..."
            
            if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                $script:appHostProcess.Kill($true)
                $script:appHostProcess.WaitForExit(2000)
                $script:appHostProcess.Dispose()
            }
            
            Stop-ExistingProcesses
            Write-Success "AppHost stopped"
        }
    }

    # Kill existing processes on ports
    function Stop-ExistingProcesses {
        param(
            [switch]$Silent
        )
        
        $ports = @(5029, 5184)
        $stoppedAny = $false
        foreach ($port in $ports) {
            try {
                $portArg = ":" + $port
                $processIds = lsof -ti $portArg 2>$null
                if ($processIds) {
                    if (-not $stoppedAny -and -not $Silent) {
                        Write-Info "Cleaning up processes on ports..."
                        $stoppedAny = $true
                    }
                    
                    # Get detailed process information for each PID
                    $processIds | ForEach-Object {
                        $processId = $_
                        try {
                            $processInfo = Get-Process -Id $processId -ErrorAction SilentlyContinue
                            if ($processInfo) {
                                if (-not $Silent) {
                                    Write-Info ("  Port {0} - Killing PID {1}: {2} (Path: {3})" -f $port, $processId, $processInfo.ProcessName, $processInfo.Path)
                                }
                            }
                            else {
                                if (-not $Silent) {
                                    Write-Info ("  Port {0} - PID {1}: (process already exited)" -f $port, $processId)
                                }
                            }
                        }
                        catch {
                            if (-not $Silent) {
                                Write-Info ("  Port {0} - PID {1}: (unable to get process details)" -f $port, $processId)
                            }
                        }
                        
                        # Use SIGKILL for immediate termination
                        kill -9 $processId 2>$null
                    }
                    
                    # Brief wait to ensure ports are released
                    Start-Sleep -Milliseconds 200
                }
            }
            catch {
                # Ignore errors if no process found
            }
        }
        
        if ($stoppedAny -and -not $Silent) {
            Write-Success "Port cleanup completed"
        }
    }

    # Kill ALL potentially orphaned processes (browsers, test runners, dotnet processes)
    function Stop-AllOrphanedProcesses {
        param(
            [switch]$Silent
        )
        
        $cleanedAny = $false
        
        # Kill orphaned Playwright/Chromium processes
        $playwrightProcesses = Get-Process -ErrorAction SilentlyContinue | Where-Object { 
            $_.ProcessName -match "chromium|chrome|headless_shell" -and 
            $_.CommandLine -match "playwright|--remote-debugging"
        }
        if ($playwrightProcesses) {
            if (-not $Silent) {
                Write-Info "  Killing orphaned browser processes:"
            }
            $playwrightProcesses | ForEach-Object { 
                if (-not $Silent) {
                    Write-Info ("    - PID {0}: {1} (Path: {2})" -f $_.Id, $_.ProcessName, $_.Path)
                }
                try { $_.Kill() } catch { }
            }
            $cleanedAny = $true
        }
        
        # Kill orphaned vstest/testhost processes
        $testProcesses = Get-Process -ErrorAction SilentlyContinue | Where-Object { 
            $_.ProcessName -match "testhost|vstest" -or
            ($_.ProcessName -eq "dotnet" -and $_.CommandLine -match "vstest\.console|testhost")
        }
        if ($testProcesses) {
            if (-not $Silent) {
                Write-Info "  Killing orphaned test processes:"
            }
            $testProcesses | ForEach-Object { 
                if (-not $Silent) {
                    Write-Info ("    - PID {0}: {1} (Path: {2})" -f $_.Id, $_.ProcessName, $_.Path)
                }
                try { $_.Kill() } catch { }
            }
            $cleanedAny = $true
        }
        
        # Kill orphaned TechHub dotnet processes (but NOT VS Code language servers)
        $dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Where-Object { 
            $_.CommandLine -match "TechHub\.(Api|Web)" -and
            $_.CommandLine -notmatch "LanguageServer|csdevkit|csharp"
        }
        if ($dotnetProcesses) {
            if (-not $Silent) {
                Write-Info "  Killing orphaned TechHub processes:"
            }
            $dotnetProcesses | ForEach-Object { 
                if (-not $Silent) {
                    Write-Info ("    - PID {0}: {1} (CommandLine: {2})" -f $_.Id, $_.ProcessName, ($_.CommandLine -replace '.*TechHub\.(Api|Web)[^/]*', 'TechHub.$1'))
                }
                try { $_.Kill() } catch { }
            }
            $cleanedAny = $true
        }
        
        return $cleanedAny
    }

    # Comprehensive cleanup function - call at start AND end
    function Invoke-FullCleanup {
        param(
            [switch]$Silent
        )
        
        if (-not $Silent) {
            Write-Step "Cleaning up any orphaned processes"
        }
        
        $cleanedAny = $false
        
        # First: Kill processes by port
        $ports = @(5029, 5184)
        $portsCleaned = $false
        foreach ($port in $ports) {
            try {
                # Get PIDs using lsof (outputs one PID per line)
                $lsofOutput = lsof -ti ":$port" 2>&1
                
                # Parse output - lsof returns PIDs separated by newlines
                $processIds = @()
                if ($lsofOutput -and $LASTEXITCODE -eq 0) {
                    # Split by newlines and filter out empty strings - ensure result is array
                    $processIds = @($lsofOutput -split "`n" | Where-Object { $_ -match '^\d+$' })
                }
                
                if ($processIds -and $processIds.Count -gt 0) {
                    foreach ($pidString in $processIds) {
                        $processId = [int]$pidString
                        try {
                            $processInfo = Get-Process -Id $processId -ErrorAction SilentlyContinue
                            if ($processInfo) {
                                if (-not $Silent) {
                                    Write-Info ("  Port {0} - Killing PID {1}: {2} (Path: {3})" -f $port, $processId, $processInfo.ProcessName, $processInfo.Path)
                                }
                                # Use Stop-Process with -Force for reliability
                                Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
                            }
                            else {
                                if (-not $Silent) {
                                    Write-Info ("  Port {0} - PID {1} already exited" -f $port, $processId)
                                }
                            }
                        }
                        catch {
                            if (-not $Silent) {
                                Write-Warning ("  Port {0} - Failed to kill PID {1}: {2}" -f $port, $processId, $_.Exception.Message)
                            }
                        }
                    }
                    $portsCleaned = $true
                    # Wait longer for ports to be released by OS
                    Start-Sleep -Milliseconds 500
                }
            }
            catch {
                if (-not $Silent) {
                    Write-Warning ("  Failed to clean up port {0}: {1}" -f $port, $_.Exception.Message)
                }
            }
        }
        if ($portsCleaned) { $cleanedAny = $true }
        
        # Second: Kill orphaned processes by name/command
        if (Stop-AllOrphanedProcesses -Silent:$Silent) {
            $cleanedAny = $true
        }
        
        # Brief pause to let OS release resources
        if ($cleanedAny) {
            Start-Sleep -Milliseconds 300
        }
        
        if (-not $Silent) {
            if ($cleanedAny) {
                Write-Success "Cleanup completed"
            }
            else {
                Write-Info "  No orphaned processes found"
            }
        }
    }

    # Run both projects using Aspire AppHost
    function Start-BothProjects {
        Write-Step "Starting Tech Hub via Aspire AppHost"
        
        Write-Host ""
        Write-Info "Services:"
        Write-Info "  API: http://localhost:5029 (Swagger: http://localhost:5029/swagger)"
        Write-Info "  Web: http://localhost:5184"
        Write-Info "  Dashboard: https://localhost:17101 (URL with token shown below)"
        Write-Host ""
        Write-Info "Press Ctrl+C to stop"
        
        try {
            Push-Location (Join-Path $workspaceRoot "src/TechHub.AppHost")
            
            $env:ASPNETCORE_ENVIRONMENT = "Development"
            # Disable automatic browser launch in DevContainer (prevents DBus errors)
            $env:DOTNET_DASHBOARD_OPEN_BROWSER = "false"
            # Disable Aspire dashboard authentication for local development (no login token required)
            $env:DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS = "true"
            
            # Run the AppHost with hot reload which orchestrates both API and Web
            # AppHost includes a built-in Aspire Dashboard
            dotnet watch --project $appHostProjectPath --no-build --configuration $configuration
        }
        finally {
            Pop-Location
            
            # Final cleanup - kill any remaining processes on the ports
            Stop-ExistingProcesses
            
            Write-Success "All projects stopped"
        }
    }

    # Main execution
    try {
        # Ensure we're in the workspace root
        Set-Location $workspaceRoot
        
        Write-Host "`n╔════════════════════════════════════════╗" -ForegroundColor Cyan
        Write-Host "║      Tech Hub Development Runner       ║" -ForegroundColor Cyan
        Write-Host "╚════════════════════════════════════════╝`n" -ForegroundColor Cyan
        
        # Validate parameter combinations
        if ($SkipTests -and $OnlyTests) {
            Write-Error "Cannot use both -SkipTests and -OnlyTests"
            exit 1
        }
        
        if ($Test -and ($SkipTests -or $OnlyTests)) {
            Write-Error "Cannot use -Test with -SkipTests or -OnlyTests"
            exit 1
        }
        
        # Default behavior: Run tests unless explicitly skipped or -Build specified
        if (-not $Test -and -not $SkipTests -and -not $OnlyTests -and -not $Build) {
            $Test = $true
            Write-Info "Running tests by default (use -SkipTests to skip, -OnlyTests to run tests and exit)"
            Write-Host ""
        }
        
        # Validate prerequisites
        Test-Prerequisites
        
        # ALWAYS clean up orphaned processes at start (ports + browsers + test runners)
        Invoke-FullCleanup
        
        # Clean build artifacts if requested
        if ($Clean) {
            Invoke-Clean
        }
        
        # Always build (use -Build to build-only)
        Invoke-Build
        
        # Test if requested - run tests FIRST before starting servers
        if ($Test) {
            Invoke-Tests
            # If tests pass, continue to start servers normally (unless Build-only mode)
            Write-Success "`nAll tests passed! Starting servers...`n"
        }
        
        # OnlyTests mode: Run tests then EXIT (don't start servers)
        if ($OnlyTests) {
            Invoke-Tests -OnlyTests
            Write-Success "`nAll tests passed!"
            return
        }
        
        # If only build was requested, exit here
        if ($Build) {
            Write-Success "`nBuild completed successfully!"
            return
        }
        
        # Run projects via Aspire AppHost
        Start-BothProjects
    }
    catch {
        Write-Error "`nFunction failed: $($_.Exception.Message)"
        Write-Host $_.ScriptStackTrace -ForegroundColor DarkGray
        
        # Clean up on error
        Write-Info "`nCleaning up after error..."
        Invoke-FullCleanup -Silent
        
        throw
    }
    finally {
        # ALWAYS clean up at the end (ports, browsers, test runners)
        Invoke-FullCleanup -Silent
        
        # Always return to workspace root
        Set-Location $workspaceRoot
    }
}

# Export the Run function
Export-ModuleMember -Function Run
