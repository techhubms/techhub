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

    .PARAMETER WithoutClean
        Skip clean build step. By default, Run does a clean build.

    .PARAMETER WithoutTests
        Skip all tests and start servers directly.

    .PARAMETER Rebuild
        Do a clean rebuild only, then exit (don't run tests or start servers).

    .PARAMETER TestProject
        Scope tests to a specific project (e.g., "TechHub.Web.Tests", "TechHub.Api.Tests").
        Can be combined with -TestName to further filter tests.

    .PARAMETER TestName
        Scope tests by name pattern (e.g., "SectionCard", "Repository").
        Uses dotnet test --filter with FullyQualifiedName~pattern.
        Can be combined with -TestProject to scope to specific project.

    .EXAMPLE
        Run
        Default: Clean build, run all tests (PowerShell + .NET), then start servers.

    .EXAMPLE
        Run -WithoutClean
        Build without cleaning, run all tests, then start servers.

    .EXAMPLE
        Run -WithoutTests
        Clean build and start servers without running tests.

    .EXAMPLE
        Run -Rebuild
        Clean rebuild only, then exit (for fixing build errors).

    .EXAMPLE
        Run -TestProject TechHub.Web.Tests
        Clean build, run only Web component tests, then start servers.

    .EXAMPLE
        Run -TestName SectionCard
        Clean build, run all tests matching "SectionCard" pattern, then start servers.

    .EXAMPLE
        Run -TestProject TechHub.E2E.Tests -TestName Navigation
        Clean build, run E2E tests matching "Navigation" pattern, then start servers.

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
        [switch]$WithoutClean,

        [Parameter(Mandatory = $false)]
        [switch]$WithoutTests,

        [Parameter(Mandatory = $false)]
        [switch]$Rebuild,

        [Parameter(Mandatory = $false)]
        [string]$TestProject,

        [Parameter(Mandatory = $false)]
        [string]$TestName
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
        Write-Host "  -WithoutClean  Skip clean build step (faster)" -ForegroundColor White
        Write-Host "  -WithoutTests  Skip all tests, start servers directly" -ForegroundColor White
        Write-Host "  -Rebuild       Clean rebuild only, then exit" -ForegroundColor White
        Write-Host "  -TestProject   Scope tests to specific project (e.g., TechHub.Web.Tests)" -ForegroundColor White
        Write-Host "  -TestName      Scope tests by name pattern (e.g., SectionCard)`n" -ForegroundColor White
        
        Write-Host "EXAMPLES:" -ForegroundColor Yellow
        Write-Host "  Run                          Clean build + all tests + servers (default)" -ForegroundColor Gray
        Write-Host "  Run -WithoutClean            Build + all tests + servers (faster)" -ForegroundColor Gray
        Write-Host "  Run -WithoutTests            Clean build + servers (no tests)" -ForegroundColor Gray
        Write-Host "  Run -Rebuild                 Clean rebuild only" -ForegroundColor Gray
        Write-Host "  Run -TestProject powershell  Run only PowerShell/Pester tests" -ForegroundColor Gray
        Write-Host "  Run -TestProject Web.Tests   Run only Web component tests" -ForegroundColor Gray
        Write-Host "  Run -TestName SectionCard    Run tests matching 'SectionCard'" -ForegroundColor Gray
        Write-Host "  Run -TestProject E2E -TestName Nav  Run E2E navigation tests`n" -ForegroundColor Gray
        
        Write-Host "COMMON WORKFLOWS:" -ForegroundColor Yellow
        Write-Host "  Automated testing:     Run" -ForegroundColor Gray
        Write-Host "  Interactive debugging: Run -WithoutTests" -ForegroundColor Gray
        Write-Host "  Fix build errors:      Run -Rebuild`n" -ForegroundColor Gray
        
        Write-Host "SERVICES:" -ForegroundColor Yellow
        Write-Host "  API:       https://localhost:7153 (Swagger: /swagger)" -ForegroundColor Gray
        Write-Host "  Web:       https://localhost:7190" -ForegroundColor Gray
        Write-Host "  Dashboard: https://localhost:17101 (Aspire Dashboard)`n" -ForegroundColor Gray
        
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
    $script:serversAlreadyRunning = $false

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
            exit 1
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
            exit 1
        }
        
        Write-Success "Prerequisites validated"
    }

    # Clean build artifacts
    function Invoke-Clean {
        Write-Step "Cleaning build artifacts"
        
        dotnet clean $solutionPath --configuration $configuration --verbosity $verbosityLevel
        if ($LASTEXITCODE -ne 0) {
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ CLEAN FAILED - Cannot continue                            ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            Write-Host "  Clean failed with exit code $LASTEXITCODE" -ForegroundColor Yellow
            Write-Host "  This is unusual - check file permissions or disk space" -ForegroundColor Yellow
            Write-Host ""
            exit 1
        }
        
        Write-Success "Clean completed"
    }

    # Build solution
    function Invoke-Build {
        Write-Step "Building solution ($configuration)"
        
        dotnet build $solutionPath --configuration $configuration --verbosity $verbosityLevel
        if ($LASTEXITCODE -ne 0) {
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ BUILD FAILED - Cannot continue                            ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            Write-Host "  Build failed with exit code $LASTEXITCODE" -ForegroundColor Yellow
            Write-Host "  Fix the compilation errors above and try again" -ForegroundColor Yellow
            Write-Host ""
            exit 1
        }
        
        Write-Success "Build completed"
    }

    # Run PowerShell/Pester tests
    function Invoke-PowerShellTests {
        param(
            [string]$TestName
        )
        
        Write-Step "Running PowerShell/Pester tests"
        Write-Host ""
        
        # Call the existing PowerShell test runner script
        $pwshTestScript = Join-Path $workspaceRoot "scripts/run-powershell-tests.ps1"
        
        if (-not (Test-Path $pwshTestScript)) {
            Write-Warning "PowerShell test script not found: $pwshTestScript"
            return
        }
        
        try {
            if ($TestName) {
                Write-Info "Filtering PowerShell tests by name: $TestName"
                & $pwshTestScript -TestName $TestName
            }
            else {
                & $pwshTestScript
            }
            $pwshExitCode = $LASTEXITCODE
        }
        catch {
            Write-Error "PowerShell test execution failed: $_"
            $pwshExitCode = 1
        }
        
        Write-Host ""
        
        if ($pwshExitCode -ne 0) {
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ POWERSHELL TESTS FAILED - Cannot continue                 ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            Write-Host "  PowerShell tests failed with exit code $pwshExitCode" -ForegroundColor Yellow
            Write-Host "  Fix the failing tests above and try again" -ForegroundColor Yellow
            Write-Host ""
            exit 1
        }
        
        Write-Success "PowerShell tests passed"
    }

    # Run tests
    function Invoke-Tests {
        param(
            [string]$TestProject,
            [string]$TestName
        )
        
        # PHASE 0: Run PowerShell tests FIRST (independent of .NET)
        if (-not $TestProject -or $TestProject -match "powershell|pester|scripts") {
            Invoke-PowerShellTests -TestName $TestName
            Write-Host ""
        }
        
        # Determine which .NET tests to run based on TestProject parameter
        $runUnitTests = $true
        $runE2ETests = $true
        
        if ($TestProject) {
            # If TestProject specified, only run that project
            if ($TestProject -notmatch "E2E") {
                $runE2ETests = $false
            }
            if ($TestProject -match "E2E") {
                $runUnitTests = $false
            }
        }
        
        # PHASE 1: Run all non-E2E tests (fast, no server needed)
        if ($runUnitTests) {
            Write-Step "Running unit and integration tests"
            Write-Host ""
            
            # Build filter expression
            $filterParts = @("FullyQualifiedName!~E2E")
            
            if ($TestProject) {
                # Add project filter
                $filterParts += "FullyQualifiedName~$TestProject"
            }
            
            if ($TestName) {
                # Add name filter
                $filterParts += "FullyQualifiedName~$TestName"
            }
            
            # Combine filters with & (AND logic)
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
            
            & dotnet @testArgs
            
            if ($LASTEXITCODE -ne 0) {
                Write-Host ""
                Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
                Write-Host "║                                                              ║" -ForegroundColor Red
                Write-Host "║  ✗ UNIT/INTEGRATION TESTS FAILED - Cannot continue           ║" -ForegroundColor Red
                Write-Host "║                                                              ║" -ForegroundColor Red
                Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
                Write-Host ""
                Write-Host "  Tests failed with exit code $LASTEXITCODE" -ForegroundColor Yellow
                Write-Host "  Fix the failing tests above and try again" -ForegroundColor Yellow
                Write-Host ""
                exit 1
            }
            
            Write-Host ""
            Write-Success "Unit and integration tests passed"
        }
        
        # PHASE 2: Run E2E tests (requires servers)
        if ($runE2ETests) {
            # Start AppHost for E2E tests
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
                    Write-Host "    3. Port conflicts (check ports 5029, 5184, 7153, 7190)" -ForegroundColor Gray
                    Write-Host ""
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
                    Write-Host "    Expected: https://localhost:7153/health" -ForegroundColor Gray
                }
                if (-not $webReady) {
                    Write-Host "  Web health check failed: $lastWebError" -ForegroundColor Yellow
                    Write-Host "    Expected: https://localhost:7190/health" -ForegroundColor Gray
                }
                Write-Host ""
                Write-Host "  Troubleshooting:" -ForegroundColor Cyan
                Write-Host "    1. Check Aspire Dashboard logs (URL shown during startup)" -ForegroundColor Gray
                Write-Host "    2. Try manually: curl -k https://localhost:7153/health" -ForegroundColor Gray
                Write-Host "    3. Check if ports are already in use" -ForegroundColor Gray
                Write-Host ""
                
                # Clean up on startup failure
                if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                    try {
                        $script:appHostProcess.Kill($false)
                    }
                    catch { }
                    finally {
                        $script:appHostProcess.Dispose()
                    }
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
            
            # Add name filter if specified
            if ($TestName) {
                $e2eTestArgs += "--filter"
                $e2eTestArgs += "FullyQualifiedName~$TestName"
            }
            
            # Run E2E tests with explicit error action to prevent terminal crash
            & dotnet @e2eTestArgs
            $e2eExitCode = $LASTEXITCODE
            
            Write-Host ""
            Write-Host "════════════════════════════════════════" -ForegroundColor Cyan
            Write-Host ""
            
            if ($e2eExitCode -ne 0) {
                Write-Host ""
                Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
                Write-Host "║                                                              ║" -ForegroundColor Red
                Write-Host "║  ✗ E2E TESTS FAILED                                          ║" -ForegroundColor Red
                Write-Host "║                                                              ║" -ForegroundColor Red
                Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
                Write-Host ""
                Write-Host "  Tests failed with exit code: $e2eExitCode" -ForegroundColor Yellow
                Write-Host "  Run with 'Run -WithoutTests' to start servers for debugging" -ForegroundColor Cyan
                Write-Host ""
                
                # Stop AppHost on failure
                Write-Info "Stopping AppHost..."
                if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                    try {
                        $script:appHostProcess.Kill($false)
                    }
                    catch { }
                    finally {
                        $script:appHostProcess.Dispose()
                    }
                }
                Stop-ExistingProcesses
                
                exit 1
            }
            
            Write-Success "E2E tests passed - servers will remain running for development"
            
            # Mark that E2E tests already started the servers
            # so we don't restart them later
            $script:serversAlreadyRunning = $true
        }
    }

    # Kill existing processes on ports
    function Stop-ExistingProcesses {
        param(
            [switch]$Silent
        )
        
        $ports = @(5029, 5184)
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
        
        # Clean up log files
        $logPath = Join-Path $workspaceRoot ".tmp/logs/*.log"
        if (Test-Path $logPath) {
            Remove-Item $logPath -Force -ErrorAction SilentlyContinue
            if (-not $Silent) {
                Write-Info "  Removed old log files"
            }
        }
        
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
        
        # Validate prerequisites
        Test-Prerequisites
        
        # ALWAYS clean up orphaned processes at start (ports + browsers + test runners)
        Invoke-FullCleanup
        
        # Clean build artifacts by default (skip if -WithoutClean specified)
        if (-not $WithoutClean) {
            Invoke-Clean
        }
        
        # Always build
        Invoke-Build
        
        # Rebuild mode: Clean rebuild only, then EXIT
        if ($Rebuild) {
            Write-Success "`nBuild completed successfully!"
            return
        }
        
        # Test by default unless -WithoutTests specified
        if (-not $WithoutTests) {
            Invoke-Tests -TestProject $TestProject -TestName $TestName
            
            # If servers are already running from E2E tests, we're done
            if ($script:serversAlreadyRunning) {
                Write-Host ""
                Write-Success "All tests passed! Servers are running and ready for development.`n"
                Write-Info "Services:"
                Write-Info "  API: http://localhost:5029 (Swagger: http://localhost:5029/swagger)"
                Write-Info "  Web: http://localhost:5184"
                Write-Info "  Dashboard: https://localhost:17101 (Aspire Dashboard)"
                Write-Host ""
                Write-Info "Press Ctrl+C to stop"
                Write-Host ""
                
                # Wait for user to stop (Ctrl+C)
                # The AppHost process is already running, just wait for it to exit
                try {
                    if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                        $script:appHostProcess.WaitForExit()
                    }
                }
                finally {
                    Stop-ExistingProcesses
                    Write-Success "All projects stopped"
                }
                
                return
            }
            
            Write-Success "`nAll tests passed! Starting servers...`n"
        }
        
        # Run projects via Aspire AppHost (only if not already running from E2E tests)
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
