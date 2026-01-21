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
        Explicitly clean build artifacts (bin/obj directories) before building.
        By default, Run does NOT clean - only use when dependencies change.

    .PARAMETER WithoutTests
        Skip all tests and start servers directly (for debugging or interactive development).

    .PARAMETER StopServers
        Stop servers after all tests complete. Use this for CI/CD pipelines where servers
        should exit after test validation. For development, omit this to keep servers running.

    .PARAMETER TestRerun
        Fast test iteration mode: Skip clean and src build, only build and run test projects.
        Assumes servers are already running (started with 'Run -WithoutTests' or 'Run').
        Combine with -TestProject for maximum speed (only rebuilds that specific test project).

    .PARAMETER Rebuild
        Do a clean rebuild only, then exit (don't run tests or start servers).

    .PARAMETER TestProject
        Scope tests to a specific project (e.g., "TechHub.Web.Tests", "TechHub.Api.Tests", "E2E.Tests", "powershell").
        Can be combined with -TestName to further filter tests.
        With -TestRerun, only rebuilds this test project for maximum speed.

    .PARAMETER TestName
        Scope tests by name pattern (e.g., "SectionCard", "Repository").
        Uses dotnet test --filter with FullyQualifiedName~pattern.
        Can be combined with -TestProject to scope to specific project.

    .EXAMPLE
        Run
        Default: Build (no clean), run all tests, then start servers for development.

    .EXAMPLE
        Run -Clean
        Clean build: Remove bin/obj, build, run all tests, then start servers.

    .EXAMPLE
        Run -WithoutTests
        Build and start servers without running tests (for debugging or quick start).

    .EXAMPLE
        Run -TestRerun -TestProject E2E.Tests
        Fast test iteration: Only rebuild E2E tests and run them (assumes servers already running).

    .EXAMPLE
        Run -Rebuild
        Clean rebuild only, then exit (for fixing build errors).

    .EXAMPLE
        Run -TestProject TechHub.Web.Tests
        Build, run only Web component tests, then keep servers running.

    .EXAMPLE
        Run -TestName SectionCard
        Build, run all tests matching "SectionCard" pattern, then keep servers running.

    .EXAMPLE
        Run -TestProject TechHub.E2E.Tests -TestName Navigation
        Build, run E2E tests matching "Navigation" pattern, then keep servers running.
    #>
    
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false)]
        [switch]$Help,

        [Parameter(Mandatory = $false)]
        [switch]$Clean,
    
        [Parameter(Mandatory = $false)]
        [switch]$WithoutTests,
    
        [Parameter(Mandatory = $false)]
        [switch]$StopServers,
    
        [Parameter(Mandatory = $false)]
        [switch]$Rebuild,
    
        [Parameter(Mandatory = $false)]
        [switch]$TestRerun,

        [Parameter(Mandatory = $false)]
        [string]$TestProject,

        [Parameter(Mandatory = $false)]
        [string]$TestName
    )

    # Disable progress bars for performance (prevents slowdown from file operations)
    $ProgressPreference = 'SilentlyContinue'

    # Show help if requested
    if ($Help) {
        Write-Host "`n╔════════════════════════════════════════╗" -ForegroundColor Cyan
        Write-Host "║      Tech Hub Development Runner       ║" -ForegroundColor Cyan
        Write-Host "╚════════════════════════════════════════╝`n" -ForegroundColor Cyan
        
        Write-Host "USAGE:" -ForegroundColor Yellow
        Write-Host "  Run [options]`n" -ForegroundColor White
        
        Write-Host "OPTIONS:" -ForegroundColor Yellow
        Write-Host "  -Help          Show this help message" -ForegroundColor White
        Write-Host "  -Clean         Clean build artifacts before building (use when dependencies change)" -ForegroundColor White
        Write-Host "  -WithoutTests  Skip all tests, start servers directly (for debugging)" -ForegroundColor White
        Write-Host "  -StopServers   Stop servers after tests complete (for CI/CD pipelines)" -ForegroundColor White
        Write-Host "  -TestRerun     Fast test iteration: Only rebuild and run test projects (assumes servers running)" -ForegroundColor White
        Write-Host "  -Rebuild       Clean rebuild only, then exit" -ForegroundColor White
        Write-Host "  -TestProject   Scope tests to specific project (e.g., TechHub.Web.Tests, E2E.Tests, powershell)" -ForegroundColor White
        Write-Host "  -TestName      Scope tests by name pattern (e.g., SectionCard)`n" -ForegroundColor White
        
        Write-Host "EXAMPLES:" -ForegroundColor Yellow
        Write-Host "  Run                                  Build + all tests + servers (default)" -ForegroundColor Gray
        Write-Host "  Run -Clean                           Clean build + all tests + servers" -ForegroundColor Gray
        Write-Host "  Run -StopServers                     Build + all tests, stop servers (CI/CD)" -ForegroundColor Gray
        Write-Host "  Run -WithoutTests                    Build + servers (no tests, for debugging)" -ForegroundColor Gray
        Write-Host "  Run -TestRerun -TestProject E2E.Tests    Fast: Only rebuild E2E tests and run (~5 sec)" -ForegroundColor Gray
        Write-Host "  Run -Rebuild                         Clean rebuild only" -ForegroundColor Gray
        Write-Host "  Run -TestProject powershell          Run only PowerShell tests, keep servers running" -ForegroundColor Gray
        Write-Host "  Run -TestProject Web.Tests           Run only Web tests, keep servers running" -ForegroundColor Gray
        Write-Host "  Run -TestName SectionCard            Run tests matching 'SectionCard', keep servers running" -ForegroundColor Gray
        Write-Host "  Run -StopServers -TestProject E2E -TestName Nav  CI/CD: Run E2E navigation tests, then stop`n" -ForegroundColor Gray
        
        Write-Host "COMMON WORKFLOWS:" -ForegroundColor Yellow
        Write-Host "  CI/CD (test + stop):       Run -StopServers" -ForegroundColor Gray
        Write-Host "  TDD (test-driven dev):     Run, then Run -TestRerun when fixing tests" -ForegroundColor Gray
        Write-Host "  Development mode:          Run" -ForegroundColor Gray
        Write-Host "  Debug/explore:             Run -WithoutTests" -ForegroundColor Gray
        Write-Host "  Fix dependencies:          Run -Clean`n" -ForegroundColor Gray
        
        Write-Host "SERVICES:" -ForegroundColor Yellow
        Write-Host "  API:       https://localhost:5001 (Swagger: /swagger)" -ForegroundColor Gray
        Write-Host "  Web:       https://localhost:5003" -ForegroundColor Gray
        Write-Host "  Dashboard: https://localhost:18888 (Aspire Dashboard)`n" -ForegroundColor Gray
        
        Write-Host "For detailed help: " -NoNewline -ForegroundColor White
        Write-Host "Get-Help Run -Full`n" -ForegroundColor Cyan
        
        return
    }

    # Use Continue to allow error handling to work properly
    # Stop causes the entire script to terminate on any error (including test failures)
    # which kills the terminal with exit code -1
    $ErrorActionPreference = "Continue"
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

    # Verbosity level - "minimal" for clean output
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

    # Run external command, display output, return $true if exit code is 0
    function Invoke-ExternalCommand {
        param(
            [string]$Command,
            [string[]]$Arguments
        )
        & $Command @Arguments | Out-Host
        return $LASTEXITCODE -eq 0
    }

    # Validate prerequisites
    function Test-Prerequisites {
        Write-Step "Checking prerequisites"
        
        # Check .NET SDK
        try {
            $dotnetVersion = dotnet --version 2>&1
            if ($LASTEXITCODE -ne 0) {
                throw "dotnet command failed with exit code $LASTEXITCODE"
            }
            Write-Info ".NET SDK version: $dotnetVersion"
        }
        catch {
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ PREREQUISITES CHECK FAILED - Cannot continue              ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            Write-Host "  .NET SDK not found or not working properly" -ForegroundColor Yellow
            Write-Host "  Please install .NET 10 SDK" -ForegroundColor Yellow
            Write-Host ""
            return $false
        }
        
        # Check that solution file exists
        if (-not (Test-Path $solutionPath)) {
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ SOLUTION FILE NOT FOUND - Cannot continue                 ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            Write-Host "  Expected: $solutionPath" -ForegroundColor Yellow
            Write-Host "  Make sure you're running from the workspace root" -ForegroundColor Yellow
            Write-Host ""
            return $false
        }
        
        Write-Success "Prerequisites validated"
        return $true
    }

    # Clean build artifacts
    function Invoke-Clean {
        Write-Step "Cleaning build artifacts"
        
        # Fast recursive removal of bin/obj directories using native Linux find (faster than dotnet clean)
        bash -c "find '$workspaceRoot' -type d \( -name bin -o -name obj \) -exec rm -rf {} + 2>/dev/null"
        
        Write-Success "Clean completed"
        return $true
    }

    # Build solution
    function Invoke-Build {
        Write-Step "Building solution ($configuration)"
        
        $success = Invoke-ExternalCommand "dotnet" @("build", $solutionPath, "--configuration", $configuration, "--verbosity", $verbosityLevel)
        
        if (-not $success) {
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ BUILD FAILED - Cannot continue                            ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            Write-Host "  Build failed - fix the compilation errors above" -ForegroundColor Yellow
            Write-Host ""
            return $false
        }
        
        Write-Success "Build completed"
        return $true
    }

    # Run PowerShell/Pester tests
    function Invoke-PowerShellTests {
        param(
            [string]$TestName
        )
        
        Write-Step "Running PowerShell/Pester tests"
        Write-Host ""
        
        $testDirectory = Join-Path $workspaceRoot "tests/powershell"
        
        # Check if test directory exists
        if (-not (Test-Path $testDirectory)) {
            Write-Host "  PowerShell test directory not found: $testDirectory" -ForegroundColor Red
            return $false
        }
        
        # Ensure Pester is available
        try {
            Import-Module Pester -Force -ErrorAction Stop
        }
        catch {
            Write-Host "  Failed to import Pester module: $($_.Exception.Message)" -ForegroundColor Red
            return $false
        }
        
        # Build Pester configuration
        $pesterConfig = [PesterConfiguration]::Default
        $pesterConfig.Run.Path = @($testDirectory)
        $pesterConfig.Run.PassThru = $true
        $pesterConfig.Output.Verbosity = 'Normal'
        
        # Apply test name filter if specified
        if ($TestName) {
            Write-Info "Filtering tests by name: *$TestName*"
            $pesterConfig.Filter.FullName = "*$TestName*"
        }
        
        # Run tests
        try {
            $result = Invoke-Pester -Configuration $pesterConfig
        }
        catch {
            Write-Host "  Test execution failed: $($_.Exception.Message)" -ForegroundColor Red
            return $false
        }
        
        # Check results
        if ($result -and $result.TotalCount -gt 0 -and $result.FailedCount -eq 0) {
            Write-Host ""
            Write-Success "PowerShell tests passed"
            return $true
        }
        
        Write-Host ""
        Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
        Write-Host "║                                                              ║" -ForegroundColor Red
        Write-Host "║  ✗ POWERSHELL TESTS FAILED - Cannot continue                 ║" -ForegroundColor Red
        Write-Host "║                                                              ║" -ForegroundColor Red
        Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
        Write-Host ""
        if ($result) {
            Write-Host "  Failed: $($result.FailedCount), Passed: $($result.PassedCount), Total: $($result.TotalCount)" -ForegroundColor Yellow
        }
        Write-Host ""
        return $false
    }

    # Run unit and integration tests (no servers needed)
    function Invoke-UnitAndIntegrationTests {
        param(
            [string]$TestProject,
            [string]$TestName
        )
        
        Write-Step "Running unit and integration tests"
        Write-Host ""
        
        # Build filter expression
        $filterParts = @("FullyQualifiedName!~E2E")
        
        if ($TestProject) {
            $filterParts += "FullyQualifiedName~$TestProject"
        }
        
        if ($TestName) {
            $filterParts += "FullyQualifiedName~$TestName"
        }
        
        $filter = $filterParts -join "&"
        
        # Run all tests except E2E using solution-level test with filter
        # Integration tests use WebApplicationFactory which manages its own environment
        $testArgs = @(
            "test",
            $solutionPath,
            "--configuration", $configuration,
            "--no-build",
            "--filter", $filter,
            "--settings", (Join-Path $workspaceRoot ".runsettings"),
            "--blame-hang-timeout", "1m"
        )
        
        $success = Invoke-ExternalCommand "dotnet" $testArgs
        
        if (-not $success) {
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ UNIT/INTEGRATION TESTS FAILED - Cannot continue           ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            Write-Host "  Tests failed - fix the failing tests above" -ForegroundColor Yellow
            Write-Host ""
            return $false
        }
        
        Write-Host ""
        Write-Success "Unit and integration tests passed"
        return $true
    }
    
    # Start Aspire AppHost (orchestrates API + Web servers)
    function Start-AppHost {
        param(
            [string]$Environment = "Development"
        )
        
        # Check if servers are already running
        $apiHealthy = $false
        $webHealthy = $false
        
        $curlOutput = curl -s -k -m 2 -w "\n%{http_code}" "https://localhost:5001/health" 2>&1
        if ($LASTEXITCODE -eq 0 -and $curlOutput -match "200$") {
            $apiHealthy = $true
        }
        
        $curlOutput = curl -s -k -m 2 -w "\n%{http_code}" "https://localhost:5003/health" 2>&1
        if ($LASTEXITCODE -eq 0 -and $curlOutput -match "200$") {
            $webHealthy = $true
        }
        
        if ($apiHealthy -and $webHealthy) {
            Write-Success "Servers already running and healthy"
            return $true
        }
        
        Write-Step "Starting Aspire AppHost"
        
        # Start AppHost in background
        $script:appHostProcess = $null
        
        $appHostStartInfo = New-Object System.Diagnostics.ProcessStartInfo
        $appHostStartInfo.FileName = "dotnet"
        $appHostStartInfo.Arguments = "watch --project `"$appHostProjectPath`" --no-build --configuration $configuration"
        $appHostStartInfo.WorkingDirectory = Split-Path $appHostProjectPath -Parent
        $appHostStartInfo.UseShellExecute = $false
        $appHostStartInfo.RedirectStandardOutput = $false
        $appHostStartInfo.RedirectStandardError = $false
        $appHostStartInfo.CreateNoWindow = $false
        # Use specified environment (defaults to Development for both E2E tests and normal dev)
        $appHostStartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = $Environment
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
        
        # Wait for services to be ready (Aspire orchestration can take 30-60 seconds)
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
                Write-Host ""
                Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
                Write-Host "║                                                              ║" -ForegroundColor Red
                Write-Host "║  ✗ APPHOST CRASHED DURING STARTUP - Cannot continue          ║" -ForegroundColor Red
                Write-Host "║                                                              ║" -ForegroundColor Red
                Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
                Write-Host ""
                Write-Host "  AppHost process exited unexpectedly" -ForegroundColor Yellow
                Write-Host "  Exit code: $($script:appHostProcess.ExitCode)" -ForegroundColor Yellow
                Write-Host ""
                Write-Host "  This usually means:" -ForegroundColor Cyan
                Write-Host "    1. A configuration error in appsettings" -ForegroundColor Gray
                Write-Host "    2. Missing dependencies or packages" -ForegroundColor Gray
                Write-Host "    3. Port conflicts (check ports 5001, 5003)" -ForegroundColor Gray
                Write-Host ""
                return $false
            }
                
            if (-not $apiReady) {
                # Use HTTPS endpoint (Aspire configures HTTPS by default)
                # -k ignores self-signed certificate validation for local dev
                $curlOutput = curl -s -k -m 2 -w "\n%{http_code}" "https://localhost:5001/health" 2>&1
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
                $curlOutput = curl -s -k -m 2 -w "\n%{http_code}" "https://localhost:5003/health" 2>&1
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
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ SERVERS FAILED TO START - Cannot continue                 ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            Write-Host "  Services failed to start within $maxAttempts seconds" -ForegroundColor Yellow
            if (-not $apiReady) {
                Write-Host "  API health check failed: $lastApiError" -ForegroundColor Yellow
                Write-Host "    Expected: https://localhost:5001/health" -ForegroundColor Gray
            }
            if (-not $webReady) {
                Write-Host "  Web health check failed: $lastWebError" -ForegroundColor Yellow
                Write-Host "    Expected: https://localhost:5003/health" -ForegroundColor Gray
            }
            Write-Host ""
            Write-Host "  Troubleshooting:" -ForegroundColor Cyan
            Write-Host "    1. Check Aspire Dashboard logs (URL shown during startup)" -ForegroundColor Gray
            Write-Host "    2. Try manually: curl -k https://localhost:5001/health" -ForegroundColor Gray
            Write-Host "    3. Check if ports are already in use" -ForegroundColor Gray
            Write-Host ""
            return $false
        }
            
        Write-Success "Services ready"
            
        return $true
    }

    # Run E2E tests (assumes servers are already running)
    function Invoke-E2ETests {
        param(
            [string]$TestName
        )
        
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
            
        # Add name filter if specified
        if ($TestName) {
            $e2eTestArgs += "--filter"
            $e2eTestArgs += "FullyQualifiedName~$TestName"
        }
            
        # Run E2E tests
        $e2eSuccess = Invoke-ExternalCommand "dotnet" $e2eTestArgs
            
        if (-not $e2eSuccess) {
            Write-Host ""
            Write-Host "════════════════════════════════════════" -ForegroundColor Red
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ E2E TESTS FAILED                                          ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            return $false
        }
            
        Write-Host ""
        Write-Host "════════════════════════════════════════" -ForegroundColor Green
        Write-Host ""
        Write-Success "E2E tests passed"
        return $true
    }

    # Kill existing processes on ports
    function Stop-AppHost {
        <#
        .SYNOPSIS
            Gracefully stops the AppHost process and disposes resources.
        
        .DESCRIPTION
            Sends SIGTERM for graceful shutdown, waits up to 5 seconds,
            then forcefully kills if still running. Always disposes the process object.
        #>
        param(
            [switch]$Silent
        )
        
        if ($null -eq $script:appHostProcess) {
            return
        }
        
        if ($script:appHostProcess.HasExited) {
            $script:appHostProcess.Dispose()
            $script:appHostProcess = $null
            return
        }
        
        try {
            if (-not $Silent) {
                Write-Info "  Sending graceful shutdown signal to AppHost..."
            }
            
            # Send SIGTERM for graceful shutdown
            kill -TERM $script:appHostProcess.Id 2>$null
            
            # Wait up to 5 seconds for graceful shutdown
            $waited = 0
            while (-not $script:appHostProcess.HasExited -and $waited -lt 5000) {
                Start-Sleep -Milliseconds 100
                $waited += 100
            }
            
            # If still running, force kill
            if (-not $script:appHostProcess.HasExited) {
                if (-not $Silent) {
                    Write-Info "  AppHost didn't stop gracefully, forcing termination..."
                }
                $script:appHostProcess.Kill($true)
            }
            elseif (-not $Silent) {
                Write-Info "  AppHost stopped gracefully"
            }
        }
        catch {
            # Process might have already exited - that's OK
            if (-not $Silent) {
                Write-Info "  AppHost process already exited"
            }
        }
        finally {
            if ($null -ne $script:appHostProcess) {
                $script:appHostProcess.Dispose()
                $script:appHostProcess = $null
            }
        }
    }

    function Stop-ExistingProcesses {
        param(
            [switch]$Silent
        )
        
        $ports = @(5001, 5003)
        $stoppedAny = $false
        $failedKills = @()
        
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
                        if ($LASTEXITCODE -ne 0) {
                            $failedKills += "PID $processId on port $port"
                        }
                    }
                    
                    # Brief wait to ensure ports are released
                    Start-Sleep -Milliseconds 200
                }
            }
            catch {
                if (-not $Silent) {
                    Write-Warning "Failed to clean up port ${port}: $($_.Exception.Message)"
                }
            }
        }
        
        if ($failedKills.Count -gt 0 -and -not $Silent) {
            Write-Warning "Failed to kill some processes: $($failedKills -join ', ')"
            Write-Warning "You may need to manually kill these processes or restart your environment"
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
        # Use simpler name-based matching to avoid CommandLine hangs
        $playwrightProcesses = Get-Process -Name "chromium", "chrome", "headless_shell" -ErrorAction SilentlyContinue
        if ($playwrightProcesses) {
            if (-not $Silent) {
                Write-Info "  Killing orphaned browser processes:"
            }
            $playwrightProcesses | ForEach-Object { 
                if (-not $Silent) {
                    Write-Info ("    - PID {0}: {1}" -f $_.Id, $_.ProcessName)
                }
                try { Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue } catch { }
            }
            $cleanedAny = $true
        }
        
        # Kill orphaned vstest/testhost processes
        $testProcesses = Get-Process -Name "testhost", "vstest" -ErrorAction SilentlyContinue
        if ($testProcesses) {
            if (-not $Silent) {
                Write-Info "  Killing orphaned test processes:"
            }
            $testProcesses | ForEach-Object { 
                if (-not $Silent) {
                    Write-Info ("    - PID {0}: {1}" -f $_.Id, $_.ProcessName)
                }
                try { Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue } catch { }
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
        
        # Clean up log files
        $logPath = Join-Path $workspaceRoot ".tmp/logs/*.log"
        if (Test-Path $logPath) {
            Remove-Item $logPath -Force -ErrorAction SilentlyContinue
            if (-not $Silent) {
                Write-Info "  Removed old log files"
            }
        }
        
        # First: Kill processes by port
        $ports = @(5001, 5003)
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

    # Main execution
    try {
        # Ensure we're in the workspace root
        Set-Location $workspaceRoot
        
        Write-Host "`n╔════════════════════════════════════════╗" -ForegroundColor Cyan
        Write-Host "║      Tech Hub Development Runner       ║" -ForegroundColor Cyan
        Write-Host "╚════════════════════════════════════════╝`n" -ForegroundColor Cyan
        
        # Validate prerequisites
        $prereqsSucceeded = Test-Prerequisites
        if ($prereqsSucceeded -eq $false) {
            return
        }
        
        # For TestRerun, skip cleanup (servers should be running)
        if (-not $TestRerun) {
            # ALWAYS clean up orphaned processes at start (ports + browsers + test runners)
            Invoke-FullCleanup
        }
        
        # Handle TestRerun fast-path: Skip clean and main build, only build/run tests
        if ($TestRerun) {
            Write-Step "Fast test rerun mode - skipping clean and src build"
            
            # Determine which tests to run
            $runPowerShell = $false
            $runUnitIntegration = $false
            $runE2E = $false
            
            if (-not $TestProject) {
                # No TestProject specified - rebuild and run ALL tests
                Write-Info "  Rebuilding all test projects..."
                $runPowerShell = $true
                $runUnitIntegration = $true
                $runE2E = $true
            }
            elseif ($TestProject -match "^(powershell|pester|scripts)$") {
                Write-Info "  Rebuilding PowerShell tests..."
                $runPowerShell = $true
            }
            elseif ($TestProject -match "E2E") {
                Write-Info "  Rebuilding E2E test project..."
                # Build only E2E test project
                $e2eTestProject = Join-Path $workspaceRoot "tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj"
                $buildResult = Invoke-ExternalCommand -Command "dotnet" -Arguments @("build", $e2eTestProject, "--verbosity", $verbosityLevel)
                if ($buildResult -ne $true) {
                    return
                }
                $runE2E = $true
            }
            else {
                Write-Info "  Rebuilding test project: $TestProject..."
                # Build specific test project
                $testProjectPath = Join-Path $workspaceRoot "tests/$TestProject/$TestProject.csproj"
                if (-not (Test-Path $testProjectPath)) {
                    Write-Error "Test project not found: $testProjectPath"
                    return
                }
                $buildResult = Invoke-ExternalCommand -Command "dotnet" -Arguments @("build", $testProjectPath, "--verbosity", $verbosityLevel)
                if ($buildResult -ne $true) {
                    return
                }
                $runUnitIntegration = $true
            }
            
            Write-Host ""
            
            # Run tests (assume servers already running for E2E)
            if ($runPowerShell) {
                $pwshSuccess = Invoke-PowerShellTests -TestName $TestName
                if ($pwshSuccess -ne $true) {
                    return
                }
                Write-Host ""
            }
            
            if ($runUnitIntegration) {
                $unitSuccess = Invoke-UnitAndIntegrationTests -TestProject $TestProject -TestName $TestName
                if ($unitSuccess -ne $true) {
                    return
                }
            }
            
            if ($runE2E) {
                Write-Info "Assuming servers are already running..."
                $e2eSuccess = Invoke-E2ETests -TestName $TestName
                if ($e2eSuccess -ne $true) {
                    Write-Error "E2E tests failed. Servers are still running."
                    return
                }
            }
            
            Write-Host ""
            Write-Success "Tests completed! Servers are still running."
            Write-Info "To stop servers, press Ctrl+C in the terminal running 'Run -WithoutTests'"
            return
        }
        
        # Clean build artifacts only if -Clean is specified
        if ($Clean) {
            $cleanSucceeded = Invoke-Clean
            if ($cleanSucceeded -eq $false) {
                return
            }
        }
        
        # Always build
        $buildSucceeded = Invoke-Build
        if ($buildSucceeded -eq $false) {
            return
        }
        
        # Rebuild mode: Clean rebuild only, then EXIT
        if ($Rebuild) {
            Write-Success "`nBuild completed successfully!"
            return
        }
        
        # Test by default unless -WithoutTests specified
        if (-not $WithoutTests) {
            # Determine which tests to run based on TestProject parameter
            $runPowerShell = $false
            $runUnitIntegration = $false
            $runE2E = $false
            
            if (-not $TestProject) {
                # No TestProject specified - run ALL tests
                $runPowerShell = $true
                $runUnitIntegration = $true
                $runE2E = $true
            }
            elseif ($TestProject -match "^(powershell|pester|scripts)$") {
                # PowerShell tests only
                $runPowerShell = $true
            }
            elseif ($TestProject -match "E2E") {
                # E2E tests only
                $runE2E = $true
            }
            else {
                # Specific unit/integration test project
                $runUnitIntegration = $true
            }
            
            # PHASE 1: PowerShell tests (fast, independent)
            if ($runPowerShell) {
                $pwshSuccess = Invoke-PowerShellTests -TestName $TestName
                if ($pwshSuccess -ne $true) {
                    return
                }
                Write-Host ""
            }
            
            # PHASE 2: Unit and integration tests (fast, no servers)
            if ($runUnitIntegration) {
                $unitSuccess = Invoke-UnitAndIntegrationTests -TestProject $TestProject -TestName $TestName
                if ($unitSuccess -ne $true) {
                    return
                }
            }
            
            # PHASE 3: Start servers if needed for E2E tests
            if ($runE2E) {
                $serversStarted = Start-AppHost
                if ($serversStarted -ne $true) {
                    # Server startup failed - ALWAYS clean up (can't leave non-running servers)
                    Stop-AppHost
                    Stop-ExistingProcesses
                    return
                }
            }
        
            # PHASE 4: E2E tests (servers already running)
            if ($runE2E) {
                $e2eSuccess = Invoke-E2ETests -TestName $TestName
            }
            
            # All tests passed - stop servers if requested, otherwise keep running
            if ($StopServers -and $runE2E) {
                Write-Host ""
                Write-Info "Stopping servers..."
                Stop-AppHost
                Stop-ExistingProcesses
                Write-Success "Servers stopped"
            }
            
            # Show appropriate success message and exit if servers not running
            if ($runE2E -and -not $StopServers) {
                Write-Host ""
                if ($e2eSuccess) {
                    Write-Success "All tests passed! Servers are running and ready for development."
                }
                else {
                    Write-Error "E2E tests failed. Servers are still running for debugging."
                    Write-Info "To stop servers, press Ctrl+C"
                }
                Write-Host ""
            }
            else {
                Write-Host ""
                Write-Success "All tests passed!"
                Write-Host ""
                return
            }
        }
        else {
            # -WithoutTests: Start servers directly
            $serversStarted = Start-AppHost
            if ($serversStarted -ne $true) {
                # Server startup failed - ALWAYS clean up (can't leave non-running servers)
                Stop-AppHost
                Stop-ExistingProcesses
                return
            }
        }
    
        # Servers are running - show info and wait for user to stop
        # (Either started with -WithoutTests, or E2E tests ran without -StopServers)
        if ($WithoutTests -or ($runE2E -and -not $StopServers)) {
            Write-Host ""
            Write-Info "Services:"
            Write-Info "  API: https://localhost:5001 (Swagger: https://localhost:5001/swagger)"
            Write-Info "  Web: https://localhost:5003"
            Write-Info "  Dashboard: https://localhost:18888 (Aspire Dashboard)"
            Write-Host ""
            Write-Info "Press Ctrl+C to stop"
            Write-Host ""
        
            # Wait for user interrupt (Ctrl+C)
            # Cleanup happens in main finally block
            if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                $script:appHostProcess.WaitForExit()
            }
        }
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
        # Gracefully stop AppHost process if still running
        Stop-AppHost -Silent
        
        # ALWAYS clean up at the end (ports, browsers, test runners)
        Invoke-FullCleanup -Silent
        
        # Always return to workspace root
        Set-Location $workspaceRoot
    }
}

# Export the Run function
Export-ModuleMember -Function Run
