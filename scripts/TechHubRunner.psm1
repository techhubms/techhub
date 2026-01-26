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

    .PARAMETER Rebuild
        Do a clean rebuild only, then exit (don't run tests or start servers).

    .PARAMETER TestProject
        Scope tests to a specific project (e.g., "TechHub.Web.Tests", "TechHub.Api.Tests", "E2E.Tests", "powershell").
        Can be combined with -TestName to further filter tests.

    .PARAMETER TestName
        Scope tests by name pattern (e.g., "SectionCard", "Repository").
        Uses dotnet test --filter with FullyQualifiedName~pattern.
        Can be combined with -TestProject to scope to specific project.

    .PARAMETER Environment
        The ASP.NET Core environment to use (Development, Production, Staging).
        Defaults to Development.
        
        Production/Staging mode:
        - Runs 'dotnet publish' to create optimized deployment artifacts
        - Executes from published DLLs (same as real production deployment)
        - Tests bundled/minified CSS, optimized assemblies, trimmed code
        - Published output stored in .tmp/publish/ (excluded from git)
        - Mimics actual Azure Container Apps deployment

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

    .EXAMPLE
        Run -Environment Production -WithoutTests
        Publish and run in Production mode (tests real deployment artifacts).
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
        [string]$TestProject,

        [Parameter(Mandatory = $false)]
        [string]$TestName,

        [Parameter(Mandatory = $false)]
        [ValidateSet("Development", "Production", "Staging")]
        [string]$Environment = "Development"
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
        Write-Host "  -Rebuild       Clean rebuild only, then exit" -ForegroundColor White
        Write-Host "  -TestProject   Scope tests to specific project (e.g., TechHub.Web.Tests, E2E.Tests, powershell)" -ForegroundColor White
        Write-Host "  -TestName      Scope tests by name pattern (e.g., SectionCard)`n" -ForegroundColor White
        
        Write-Host "EXAMPLES:" -ForegroundColor Yellow
        Write-Host "  Run                                  Build + all tests + servers (default)" -ForegroundColor Gray
        Write-Host "  Run -Clean                           Clean build + all tests + servers" -ForegroundColor Gray
        Write-Host "  Run -StopServers                     Build + all tests, stop servers (CI/CD)" -ForegroundColor Gray
        Write-Host "  Run -WithoutTests                    Build + servers (no tests, for debugging)" -ForegroundColor Gray
        Write-Host "  Run -Rebuild                         Clean rebuild only" -ForegroundColor Gray
        Write-Host "  Run -TestProject powershell          Run only PowerShell tests, keep servers running" -ForegroundColor Gray
        Write-Host "  Run -TestProject Web.Tests           Run only Web tests, keep servers running" -ForegroundColor Gray
        Write-Host "  Run -TestName SectionCard            Run tests matching 'SectionCard', keep servers running" -ForegroundColor Gray
        Write-Host "  Run -StopServers -TestProject E2E -TestName Nav  CI/CD: Run E2E navigation tests, then stop`n" -ForegroundColor Gray
        
        Write-Host "COMMON WORKFLOWS:" -ForegroundColor Yellow
        Write-Host "  CI/CD (test + stop):       Run -StopServers" -ForegroundColor Gray
        Write-Host "  TDD (test-driven dev):     Run (auto-detects changes)" -ForegroundColor Gray
        Write-Host "  Development mode:          Run" -ForegroundColor Gray
        Write-Host "  Debug/explore:             Run -WithoutTests" -ForegroundColor Gray
        Write-Host "  Fix dependencies:          Run -Clean`n" -ForegroundColor Gray
        
        Write-Host "SERVICES:" -ForegroundColor Yellow
        Write-Host "  API:       https://localhost:5001 (Swagger: /swagger)" -ForegroundColor Gray
        Write-Host "  Web:       https://localhost:5003" -ForegroundColor Gray
        Write-Host "  Dashboard: https://localhost:18888 (Aspire Dashboard)`n" -ForegroundColor Gray
        
        Write-Host "LOG FILES:" -ForegroundColor Yellow
        Write-Host "  Console:      .tmp/logs/console.txt (Development - combined Aspire output)" -ForegroundColor Gray
        Write-Host "  API console:  .tmp/logs/api-console.txt (Production/Staging mode)" -ForegroundColor Gray
        Write-Host "  Web console:  .tmp/logs/web-console.txt (Production/Staging mode)" -ForegroundColor Gray
        Write-Host "  API logs:     .tmp/logs/api-dev.log / api-prod.log (FileLoggerProvider)" -ForegroundColor Gray
        Write-Host "  Web logs:     .tmp/logs/web-dev.log / web-prod.log (FileLoggerProvider)`n" -ForegroundColor Gray
        
        Write-Host "For detailed help: " -NoNewline -ForegroundColor White
        Write-Host "Get-Help Run -Full`n" -ForegroundColor Cyan
        
        return
    }

    # Use Continue to allow error handling to work properly
    # Stop causes the entire script to terminate on any error (including test failures)
    # which kills the terminal with exit code -1
    $ErrorActionPreference = "Continue"
    Set-StrictMode -Version Latest

    # Determine workspace root - navigate up from scripts directory
    $workspaceRoot = Split-Path $PSScriptRoot -Parent

    # Solution file path
    $solutionPath = Join-Path $workspaceRoot "TechHub.slnx"

    # Project paths - for running/watching specific projects
    $appHostProjectPath = Join-Path $workspaceRoot "src/TechHub.AppHost/TechHub.AppHost.csproj"

    # Test project path for E2E (only needed separately)
    $e2eTestProjectPath = Join-Path $workspaceRoot "tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj"

    # Publish directories for Production mode
    $publishBaseDir = Join-Path $workspaceRoot ".tmp/publish"
    $publishApiDir = Join-Path $publishBaseDir "api"
    $publishWebDir = Join-Path $publishBaseDir "web"

    # Log directories and files
    $logDir = Join-Path $workspaceRoot ".tmp/logs"
    $consoleLogPath = Join-Path $logDir "console.txt"

    # Build configuration - Release for Production, Debug otherwise
    $configuration = if ($Environment -eq "Production") { "Release" } else { "Debug" }

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
    # Returns: @{ Success = $true/$false; SrcRebuilt = $true/$false }
    function Invoke-Build {
        Write-Step "Building solution ($configuration)"
        
        # Capture DLL timestamps BEFORE build to detect actual recompilation
        $srcProjects = @('Api', 'Web', 'Core', 'Infrastructure', 'ServiceDefaults')
        $timestampsBefore = @{}
        foreach ($project in $srcProjects) {
            $dllPath = Join-Path $workspaceRoot "src/TechHub.$project/bin/$configuration/net10.0/TechHub.$project.dll"
            if (Test-Path $dllPath) {
                $timestampsBefore[$project] = (Get-Item $dllPath).LastWriteTime
            }
        }
        
        # Run build and stream output in real-time (no Out-String buffering)
        dotnet build $solutionPath --configuration $configuration --verbosity $verbosityLevel 2>&1 | Out-Host
        $success = $LASTEXITCODE -eq 0
        
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
            Write-Info "  Cleaning up processes in case files are locked..."
            Stop-Servers -Silent
            return @{ Success = $false; SrcRebuilt = $false }
        }
        
        # Check which src projects were ACTUALLY rebuilt by comparing DLL timestamps
        $rebuiltProjects = @()
        foreach ($project in $srcProjects) {
            $dllPath = Join-Path $workspaceRoot "src/TechHub.$project/bin/$configuration/net10.0/TechHub.$project.dll"
            if (Test-Path $dllPath) {
                $timestampAfter = (Get-Item $dllPath).LastWriteTime
                $timestampBefore = $timestampsBefore[$project]
                
                # DLL timestamp changed = actual recompilation occurred
                if (-not $timestampBefore -or $timestampAfter -gt $timestampBefore) {
                    $rebuiltProjects += $project
                }
            }
        }
        
        # Determine if src projects were rebuilt (for restart logic)
        $srcRebuilt = $rebuiltProjects.Count -gt 0
        
        Write-Success "Build completed"
        return @{ Success = $true; SrcRebuilt = $srcRebuilt }
    }

    # Publish solution for Production deployment
    function Invoke-Publish {
        Write-Step "Publishing solution for Production deployment"
        
        # Clean publish directory
        if (Test-Path $publishBaseDir) {
            Remove-Item -Path $publishBaseDir -Recurse -Force
        }
        New-Item -Path $publishBaseDir -ItemType Directory -Force | Out-Null
        
        # Publish API
        Write-Info "Publishing API..."
        $apiProjectPath = Join-Path $workspaceRoot "src/TechHub.Api/TechHub.Api.csproj"
        $publishApiArgs = @(
            "publish",
            $apiProjectPath,
            "--configuration", "Release",
            "--output", $publishApiDir,
            "--verbosity", $verbosityLevel
        )
        
        $apiSuccess = Invoke-ExternalCommand "dotnet" $publishApiArgs
        if (-not $apiSuccess) {
            Write-Error "API publish failed"
            return $false
        }
        
        # Publish Web
        Write-Info "Publishing Web..."
        $webProjectPath = Join-Path $workspaceRoot "src/TechHub.Web/TechHub.Web.csproj"
        $publishWebArgs = @(
            "publish",
            $webProjectPath,
            "--configuration", "Release",
            "--output", $publishWebDir,
            "--verbosity", $verbosityLevel
        )
        
        $webSuccess = Invoke-ExternalCommand "dotnet" $publishWebArgs
        if (-not $webSuccess) {
            Write-Error "Web publish failed"
            return $false
        }
        
        Write-Success "Publish completed"
        Write-Info "Published to: $publishBaseDir"
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
        
        # Pre-load PowerShell functions BEFORE Pester runs (avoids Discovery phase overhead)
        $functionsPath = Join-Path $testDirectory "../../scripts/content-processing/functions"
        if (Test-Path $functionsPath) {
            . (Join-Path $functionsPath "Write-ErrorDetails.ps1")
            Get-ChildItem -Path $functionsPath -Filter "*.ps1" | 
            Where-Object { $_.Name -ne "Write-ErrorDetails.ps1" } |
            ForEach-Object { . $_.FullName }
        }
        
        # Set up global temp directory for tests
        $tempBase = if ($env:TEMP) { $env:TEMP } elseif ($env:TMP) { $env:TMP } elseif ($env:TMPDIR) { $env:TMPDIR } else { "/tmp" }
        $tempPath = Join-Path $tempBase "pwshtests"
        if (Test-Path $tempPath) {
            Remove-Item $tempPath -Recurse -Force
        }
        New-Item -ItemType Directory -Path $tempPath -Force | Out-Null
        $global:TempPath = $tempPath
        
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
    
    # Test if servers are healthy
    function Test-ServersHealthy {
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
        
        return ($apiHealthy -and $webHealthy)
    }

    # Start Aspire AppHost (orchestrates API + Web servers)
    # Servers run in background with output redirected to .tmp/logs/
    function Start-AppHost {
        param(
            [string]$Environment
        )
        
        # Check if servers are already running and healthy - just reuse them
        if (Test-ServersHealthy) {
            return $true
        }
        
        # Servers not healthy - clean up and restart
        # This handles crashed servers, unhealthy servers, or blocked ports
        Write-Info "Servers not healthy - restarting..."
        Stop-Servers -Silent
        
        Write-Step "Starting services in background ($Environment mode)"
        
        # Clear ALL previous log files before starting new servers
        if (Test-Path $logDir) {
            Remove-Item "$logDir/*" -Force -Recurse -ErrorAction SilentlyContinue
        }
        
        # Ensure log directory exists
        if (-not (Test-Path $logDir)) {
            New-Item -Path $logDir -ItemType Directory -Force | Out-Null
        }
        
        # Log environment configuration for debugging
        Write-Info "Configuring environment: $Environment"
        
        # Production/Staging mode: Run from published DLLs (real deployment simulation)
        if ($Environment -eq "Production" -or $Environment -eq "Staging") {
            # Verify publish artifacts exist
            $apiDll = Join-Path $publishApiDir "TechHub.Api.dll"
            $webDll = Join-Path $publishWebDir "TechHub.Web.dll"
            
            if (-not (Test-Path $apiDll) -or -not (Test-Path $webDll)) {
                Write-Error "Published artifacts not found. This should not happen - publish step should have run."
                return $false
            }
            
            Write-Info "Running from published artifacts (simulating production deployment)"
            Write-Info "  API: $publishApiDir"
            Write-Info "  Web: $publishWebDir"
            
            # Console output files for Production mode (separate from FileLoggerProvider logs)
            $apiConsolePath = Join-Path $logDir "api-console.txt"
            $webConsolePath = Join-Path $logDir "web-console.txt"
            
            # Start API in background using PowerShell job
            Start-Job -ScriptBlock {
                param($dir, $dll, $env, $logPath)
                Set-Location $dir
                $env:ASPNETCORE_ENVIRONMENT = $env
                $env:ASPNETCORE_URLS = "https://localhost:5001"
                & dotnet exec $dll *> $logPath
            } -ArgumentList $publishApiDir, (Join-Path $publishApiDir "TechHub.Api.dll"), $Environment, $apiConsolePath | Out-Null
            Write-Info "API starting in background..."
            
            # Start Web in background using PowerShell job
            Start-Job -ScriptBlock {
                param($dir, $dll, $env, $logPath)
                Set-Location $dir
                $env:ASPNETCORE_ENVIRONMENT = $env
                $env:ASPNETCORE_URLS = "https://localhost:5003"
                $env:ApiBaseUrl = "https://localhost:5001"
                & dotnet exec $dll *> $logPath
            } -ArgumentList $publishWebDir, (Join-Path $publishWebDir "TechHub.Web.dll"), $Environment, $webConsolePath | Out-Null
            Write-Info "Web starting in background..."
        }
        else {
            # Development: Use Aspire AppHost orchestration in background
            $appHostDir = Split-Path $appHostProjectPath -Parent
            
            # Start dotnet watch in background using PowerShell job
            Start-Job -ScriptBlock {
                param($dir, $project, $launchProfile, $config, $logPath)
                Set-Location $dir
                & dotnet watch --project $project --no-build --launch-profile $launchProfile --configuration $config *> $logPath
            } -ArgumentList $appHostDir, $appHostProjectPath, $Environment, $configuration, $consoleLogPath | Out-Null
            Write-Info "AppHost starting in background..."
        }
        
        # Wait for services to be ready (Aspire orchestration can take 30-60 seconds)
        Write-Info "Waiting for services to start (Aspire orchestration can take 30-60 seconds)..."
        $maxAttempts = 60
        $attempt = 0
        $serversReady = $false
        
        while ($attempt -lt $maxAttempts -and -not $serversReady) {
            Start-Sleep -Seconds 1
            $attempt++
                
            # Show progress every 10 seconds
            if ($attempt % 10 -eq 0) {
                Write-Info "Still waiting... ($attempt seconds elapsed)"
            }
                
            # Check if both servers are healthy
            $serversReady = Test-ServersHealthy
            
            if ($serversReady) {
                Write-Info "API ready ✓"
                Write-Info "Web ready ✓"
            }
        }
            
        if (-not $serversReady) {
            Write-Host ""
            Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "║  ✗ SERVERS FAILED TO START - Cannot continue                 ║" -ForegroundColor Red
            Write-Host "║                                                              ║" -ForegroundColor Red
            Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Red
            Write-Host ""
            Write-Host "  Services failed to start within $maxAttempts seconds" -ForegroundColor Yellow
            Write-Host "  Health check failed for one or both services" -ForegroundColor Yellow
            Write-Host "    Expected: https://localhost:5001/health" -ForegroundColor Gray
            Write-Host "    Expected: https://localhost:5003/health" -ForegroundColor Gray
            Write-Host ""
            Write-Host "  Troubleshooting:" -ForegroundColor Cyan
            Write-Host "    1. Check logs: .tmp/logs/ (console.txt or api/web-console.txt)" -ForegroundColor Gray
            Write-Host "    2. Try manually: curl -k https://localhost:5001/health" -ForegroundColor Gray
            Write-Host "    3. Check if ports are already in use" -ForegroundColor Gray
            Write-Host ""
            return $false
        }
            
        Write-Success "Services ready (running in background)"
            
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

    # Stop all servers and clean up ports
    function Stop-Servers {
        <#
        .SYNOPSIS
            Stops all processes using the server ports (5001, 5003).
        
        .DESCRIPTION
            Port-based cleanup function that kills any process using our ports.
            No process tracking needed - just kills whatever is on the ports.
            Always safe to call.
        #>
        param(
            [switch]$Silent
        )
        
        # Kill any processes on our ports
        $ports = @(5001, 5003)
        $stoppedAny = $false
        
        foreach ($port in $ports) {
            $processIds = lsof -ti ":$port" 2>$null
            if ($processIds) {
                $stoppedAny = $true
                foreach ($pidToKill in $processIds) {
                    if (-not $Silent) {
                        Write-Info "  Killing process on port $port (PID: $pidToKill)"
                    }
                    kill -9 $pidToKill 2>$null
                }
            }
        }
        
        if ($stoppedAny) {
            # Brief wait to ensure ports are released
            Start-Sleep -Milliseconds 300
            if (-not $Silent) {
                Write-Success "Port cleanup completed"
            }
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
        
        # Kill orphaned TEST processes only (vstest, testhost in command line)
        # DO NOT kill MSBuild workers - they might be from active dotnet watch/build!
        # SKIP VS Code extension processes (parent is ServiceHost/Controller/LanguageServer)
        $dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue
        if ($dotnetProcesses) {
            foreach ($proc in $dotnetProcesses) {
                try {
                    # Get command line to check if it's a test process
                    $cmdLine = cat "/proc/$($proc.Id)/cmdline" 2>$null | tr '\0' ' '
                    
                    # Skip VS Code processes
                    if ($cmdLine -match "vscode-server|csdevkit|csharp") {
                        continue
                    }
                    
                    # ONLY kill if it's a test-related process (vstest, testhost, dotnet test)
                    # DO NOT kill MSBuild workers, build processes, or watch processes
                    if ($cmdLine -match "vstest|testhost|dotnet.*test") {
                        if (-not $Silent) {
                            $parentPid = ps -o ppid= -p $proc.Id 2>$null | ForEach-Object { $_.Trim() }
                            Write-Info ("  Killing orphaned test process: PID {0} (Parent: {1})" -f $proc.Id, $parentPid)
                        }
                        kill -9 $proc.Id 2>$null
                        $cleanedAny = $true
                    }
                }
                catch {
                    # Process might have already exited - that's OK
                }
            }
        }
        
        return $cleanedAny
    }
    
    # Stop orphaned test processes (browsers, testhost, vstest)
    # Call at start to clean up from previous runs, and in finally block for cleanup
    # Does NOT touch server processes - those are managed by Stop-Servers
    function Stop-OrphanedTestProcesses {
        param(
            [switch]$Silent
        )
        
        if (-not $Silent) {
            Write-Step "Cleaning up orphaned test processes"
        }
        
        $cleanedAny = $false
        
        # Note: Log files are NOT cleaned up here
        # They are only removed when servers are actually (re)started in Start-AppHost
        
        # Kill orphaned processes by name/command (browsers, test runners)
        # DO NOT kill server processes - smart restart logic handles that
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
                Write-Info "No orphaned processes found"
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
        
        # Check if this is PowerShell-only test run (no .NET build/tests needed)
        $powerShellOnly = $TestProject -match "^(powershell|pester|scripts)$"
        
        if ($powerShellOnly) {
            # PowerShell-only mode: Skip all .NET build/test/server operations
            Write-Step "Running PowerShell/Pester tests"
            $pwshSuccess = Invoke-PowerShellTests -TestName $TestName
            if ($pwshSuccess -ne $true) {
                return
            }
            Write-Host ""
            return
        }
        
        # Regular mode: .NET build/test/server workflow
        
        # Validate prerequisites
        $prereqsSucceeded = Test-Prerequisites
        if ($prereqsSucceeded -eq $false) {
            return
        }
        
        # Clean up any remaining orphaned test/build processes
        # This catches orphaned browsers, test runners, MSBuild workers that survived
        $null = Stop-OrphanedTestProcesses
        
        # Clean build artifacts only if -Clean is specified
        if ($Clean) {
            $cleanSucceeded = Invoke-Clean
            if ($cleanSucceeded -eq $false) {
                return
            }
        }
        
        # Build OR Publish depending on environment
        if ($Environment -eq "Production" -or $Environment -eq "Staging") {
            # Production/Staging: Publish (which builds implicitly)
            $publishSucceeded = Invoke-Publish
            if ($publishSucceeded -eq $false) {
                return
            }
            # Set buildResult for restart logic (publish always rebuilds)
            $buildResult = @{ Success = $true; SrcRebuilt = $true }
        }
        else {
            # Development: Build only
            $buildResult = Invoke-Build
            if ($buildResult.Success -eq $false) {
                return
            }
        }
        
        # Check if we need to restart servers:
        # 1. Source projects were rebuilt (binaries changed)
        # 2. Servers are unhealthy (not responding to health checks)
        $serversHealthy = Test-ServersHealthy
        $needsRestart = $buildResult.SrcRebuilt -or (-not $serversHealthy)
        
        if ($needsRestart) {
            Write-Step "Restart decision"
            if ($buildResult.SrcRebuilt) {
                Write-Info "Source changes detected - binaries were rebuilt"
                Write-Info "Restarting servers to load new code..."
            }
            elseif (-not $serversHealthy) {
                Write-Info "Servers are unhealthy or not running"
                Write-Info "Restarting servers..."
            }
            
            # Stop existing servers and clean up ports
            Stop-Servers
        }
        else {
            Write-Step "Restart decision"
            Write-Info "No source changes detected - binaries unchanged"
            Write-Info "Servers are healthy - no restart needed"
            Write-Success "Reusing existing servers"
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
                # No TestProject specified - run ALL tests (PowerShell first, then .NET)
                $runPowerShell = $true
                $runUnitIntegration = $true
                $runE2E = $true
            }
            elseif ($TestProject -match "E2E") {
                # E2E tests only
                $runE2E = $true
            }
            else {
                # Specific unit/integration test project
                $runUnitIntegration = $true
            }
            
            # PHASE 1: PowerShell tests (fast, independent, no build needed)
            # Run first because they're fast and don't require servers
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
                $serversStarted = Start-AppHost -Environment $Environment
                if ($serversStarted -ne $true) {
                    # Server startup failed - ALWAYS clean up (can't leave non-running servers)
                    Stop-Servers
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
                Stop-Servers
                Write-Success "Servers stopped"
            }
            
            # Show appropriate success message - servers run in background now
            if ($runE2E -and -not $StopServers) {
                Write-Host ""
                if ($e2eSuccess) {
                    Write-Success "All tests passed! Servers are running in background."
                }
                else {
                    Write-Error "E2E tests failed. Servers are still running for debugging."
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
            $serversStarted = Start-AppHost -Environment $Environment
            if ($serversStarted -ne $true) {
                # Server startup failed - ALWAYS clean up (can't leave non-running servers)
                Stop-Servers
                return
            }
        }
    
        # Servers are running in background - show info
        # (Either started with -WithoutTests, or E2E tests ran without -StopServers)
        if ($WithoutTests -or ($runE2E -and -not $StopServers)) {
            Write-Step "Services (running in background)"
            Write-Info "API: https://localhost:5001 (Swagger: https://localhost:5001/swagger)"
            Write-Info "Web: https://localhost:5003"
            Write-Info "Dashboard: https://localhost:18888 (Aspire Dashboard)"
            Write-Step "Log files"
            Write-Host "  Console:      .tmp/logs/console.txt (Development - combined Aspire output)" -ForegroundColor Gray
            Write-Host "  API console:  .tmp/logs/api-console.txt (Production/Staging mode)" -ForegroundColor Gray
            Write-Host "  Web console:  .tmp/logs/web-console.txt (Production/Staging mode)" -ForegroundColor Gray
            Write-Host "  API logs:     .tmp/logs/api-dev.log / api-prod.log (FileLoggerProvider)" -ForegroundColor Gray
            Write-Host "  Web logs:     .tmp/logs/web-dev.log / web-prod.log (FileLoggerProvider)" -ForegroundColor Gray
            Write-Host ""
        }
    }
    catch {
        Write-Error "`nFunction failed: $($_.Exception.Message)"
        Write-Host $_.ScriptStackTrace -ForegroundColor DarkGray
        
        # Clean up on error
        Write-Info "`nCleaning up after error..."
        Stop-OrphanedTestProcesses -Silent
        
        throw
    }
    finally {
        # Always return to workspace root
        Set-Location $workspaceRoot
        Write-Host "Back at workspace root: $workspaceRoot`nThis terminal is now free to use"
    }
}

# Export the Run function
Export-ModuleMember -Function Run
