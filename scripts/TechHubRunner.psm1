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
        Skip all tests and start servers directly (for debugging failing tests).

    .PARAMETER OnlyTests
        Run tests only, then exit. Stops servers after successful test completion.
        Use this for CI/CD or when you only want to verify tests pass.

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
        Run -OnlyTests
        Clean build, run all tests, then exit (servers stop after tests pass).

    .EXAMPLE
        Run -WithoutClean
        Build without cleaning, run all tests, then start servers.

    .EXAMPLE
        Run -WithoutTests
        Clean build and start servers without running tests (for debugging).

    .EXAMPLE
        Run -Rebuild
        Clean rebuild only, then exit (for fixing build errors).

    .EXAMPLE
        Run -OnlyTests -TestProject TechHub.Web.Tests
        Clean build, run only Web component tests, then exit.

    .EXAMPLE
        Run -OnlyTests -TestName SectionCard
        Clean build, run all tests matching "SectionCard" pattern, then exit.

    .EXAMPLE
        Run -OnlyTests -TestProject TechHub.E2E.Tests -TestName Navigation
        Clean build, run E2E tests matching "Navigation" pattern, then exit.

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
        [switch]$OnlyTests,

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
        Write-Host "  -WithoutTests  Skip all tests, start servers directly (for debugging)" -ForegroundColor White
        Write-Host "  -OnlyTests     Run tests only, then exit (stops servers after tests pass)" -ForegroundColor White
        Write-Host "  -Rebuild       Clean rebuild only, then exit" -ForegroundColor White
        Write-Host "  -TestProject   Scope tests to specific project (e.g., TechHub.Web.Tests)" -ForegroundColor White
        Write-Host "  -TestName      Scope tests by name pattern (e.g., SectionCard)`n" -ForegroundColor White
        
        Write-Host "EXAMPLES:" -ForegroundColor Yellow
        Write-Host "  Run                                  Clean build + all tests + servers (default)" -ForegroundColor Gray
        Write-Host "  Run -OnlyTests                       Clean build + all tests, then exit" -ForegroundColor Gray
        Write-Host "  Run -WithoutClean                    Build + all tests + servers (faster)" -ForegroundColor Gray
        Write-Host "  Run -WithoutTests                    Clean build + servers (no tests, for debugging)" -ForegroundColor Gray
        Write-Host "  Run -Rebuild                         Clean rebuild only" -ForegroundColor Gray
        Write-Host "  Run -OnlyTests -TestProject powershell   Run only PowerShell tests, then exit" -ForegroundColor Gray
        Write-Host "  Run -OnlyTests -TestProject Web.Tests    Run only Web tests, then exit" -ForegroundColor Gray
        Write-Host "  Run -OnlyTests -TestName SectionCard     Run tests matching 'SectionCard', then exit" -ForegroundColor Gray
        Write-Host "  Run -OnlyTests -TestProject E2E -TestName Nav  Run E2E navigation tests, then exit`n" -ForegroundColor Gray
        
        Write-Host "COMMON WORKFLOWS:" -ForegroundColor Yellow
        Write-Host "  Run all tests (CI/CD): Run -OnlyTests" -ForegroundColor Gray
        Write-Host "  Development mode:      Run" -ForegroundColor Gray
        Write-Host "  Debug failing tests:   Run -WithoutTests" -ForegroundColor Gray
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
                Write-Host "    3. Port conflicts (check ports 5029, 5184, 7153, 7190)" -ForegroundColor Gray
                Write-Host ""
                return $false
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
            Write-Host "  Run with 'Run -WithoutTests' to start servers for debugging" -ForegroundColor Cyan
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
        
        # ALWAYS clean up orphaned processes at start (ports + browsers + test runners)
        Invoke-FullCleanup
        
        # Clean build artifacts by default (skip if -WithoutClean specified)
        if (-not $WithoutClean) {
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
        
        # Determine if this is a test-only run (explicit -OnlyTests OR any test-scoping parameter)
        $isTestOnlyRun = $OnlyTests -or $TestProject -or $TestName
        
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
            
            # PHASE 3: Start servers (if needed for E2E tests or development mode)
            # Start servers BEFORE E2E tests if:
            # - We're running E2E tests, OR
            # - We're in development mode (not test-only run)
            $needServers = $runE2E -or (-not $isTestOnlyRun)
        
            if ($needServers) {
                $serversStarted = Start-AppHost
                if ($serversStarted -ne $true) {
                    # Server startup failed - clean up and exit
                    Stop-AppHost
                    Stop-ExistingProcesses
                    return
                }
                $script:serversAlreadyRunning = $true
            }
        
            # PHASE 4: E2E tests (servers already running)
            if ($runE2E) {
                $e2eSuccess = Invoke-E2ETests -TestName $TestName
                if ($e2eSuccess -ne $true) {
                    # Tests failed - clean up and exit
                    Write-Info "Stopping servers..."
                    Stop-AppHost
                    Stop-ExistingProcesses
                    return
                }
            }
            
            # All tests passed - decide what to do next
            if ($isTestOnlyRun) {
                Write-Host ""
                Write-Success "All tests passed!`n"
                
                # Stop servers if they were started
                if ($script:serversAlreadyRunning) {
                    Write-Info "Stopping servers..."
                    Stop-AppHost
                    Stop-ExistingProcesses
                    Write-Success "Servers stopped"
                }
                
                return
            }
            
            Write-Success "`nAll tests passed! Servers are running and ready for development.`n"
        }
        else {
            # -WithoutTests: Start servers directly
            $serversStarted = Start-AppHost
            if ($serversStarted -ne $true) {
                # Server startup failed - clean up and exit
                Stop-AppHost
                Stop-ExistingProcesses
                return
            }
            $script:serversAlreadyRunning = $true
        }
    
        # Servers are running - show info and wait for user to stop
        Write-Host ""
        Write-Info "Services:"
        Write-Info "  API: http://localhost:5029 (Swagger: http://localhost:5029/swagger)"
        Write-Info "  Web: http://localhost:5184"
        Write-Info "  Dashboard: https://localhost:17101 (Aspire Dashboard)"
        Write-Host ""
        Write-Info "Press Ctrl+C to stop"
        Write-Host ""
    
        try {
            if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                $script:appHostProcess.WaitForExit()
            }
        }
        finally {
            Stop-AppHost -Silent
            Stop-ExistingProcesses
            Write-Success "All projects stopped"
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
        # ALWAYS clean up at the end (ports, browsers, test runners)
        Invoke-FullCleanup -Silent
        
        # Always return to workspace root
        Set-Location $workspaceRoot
    }
}

# Export the Run function
Export-ModuleMember -Function Run
