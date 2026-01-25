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
    $script:apiProcess = $null

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
        $buildOutput = @()
        dotnet build $solutionPath --configuration $configuration --verbosity $verbosityLevel 2>&1 | Tee-Object -Variable buildOutput | Write-Host
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
    function Start-AppHost {
        param(
            [string]$Environment
        )
        
        # Check if servers are already running and healthy
        if (Test-ServersHealthy) {
            # Adopt existing processes so we can manage them (e.g., stop on Ctrl+C)
            Adopt-OrphanedServerProcesses
            Write-Success "Servers already running and healthy"
            return $true
        }
        
        # Servers not healthy - clean up and restart
        # This handles crashed servers, unhealthy servers, or blocked ports
        Write-Info "  Servers not healthy - restarting..."
        Stop-Servers -Silent
        
        Write-Step "Starting services ($Environment mode)"
        
        # Log environment configuration for debugging
        Write-Info "  Configuring environment: $Environment"
        
        # Start AppHost in background
        $script:appHostProcess = $null
        
        # Production/Staging mode: Run from published DLLs (real deployment simulation)
        if ($Environment -eq "Production" -or $Environment -eq "Staging") {
            # Verify publish artifacts exist
            $apiDll = Join-Path $publishApiDir "TechHub.Api.dll"
            $webDll = Join-Path $publishWebDir "TechHub.Web.dll"
            
            if (-not (Test-Path $apiDll) -or -not (Test-Path $webDll)) {
                Write-Error "Published artifacts not found. This should not happen - publish step should have run."
                return $false
            }
            
            Write-Info "  Running from published artifacts (simulating production deployment)"
            Write-Info "    API: $publishApiDir"
            Write-Info "    Web: $publishWebDir"
            
            # Start API directly from published DLL
            $apiStartInfo = New-Object System.Diagnostics.ProcessStartInfo
            $apiStartInfo.FileName = "dotnet"
            $apiStartInfo.Arguments = "exec `"$apiDll`""
            $apiStartInfo.WorkingDirectory = $publishApiDir
            $apiStartInfo.UseShellExecute = $false
            $apiStartInfo.RedirectStandardOutput = $false
            $apiStartInfo.RedirectStandardError = $false
            $apiStartInfo.CreateNoWindow = $false
            $apiStartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = $Environment
            $apiStartInfo.EnvironmentVariables["ASPNETCORE_URLS"] = "https://localhost:5001"
            $apiStartInfo.EnvironmentVariables["Logging__Console__LogLevel__Microsoft"] = "Warning"
            $apiStartInfo.EnvironmentVariables["Logging__Console__LogLevel__Microsoft.AspNetCore"] = "Warning"
            
            # File logging configuration for Production mode
            $apiLogPath = Join-Path $workspaceRoot ".tmp/logs/api-prod.log"
            $apiLogDir = Split-Path $apiLogPath -Parent
            if (-not (Test-Path $apiLogDir)) {
                New-Item -Path $apiLogDir -ItemType Directory -Force | Out-Null
            }
            $apiStartInfo.EnvironmentVariables["Logging__File__Path"] = $apiLogPath
            
            $apiProcess = [System.Diagnostics.Process]::Start($apiStartInfo)
            Write-Info "  API started from published DLL (PID: $($apiProcess.Id))"
            
            # Start Web directly from published DLL
            $webStartInfo = New-Object System.Diagnostics.ProcessStartInfo
            $webStartInfo.FileName = "dotnet"
            $webStartInfo.Arguments = "exec `"$webDll`""
            $webStartInfo.WorkingDirectory = $publishWebDir
            $webStartInfo.UseShellExecute = $false
            $webStartInfo.RedirectStandardOutput = $false
            $webStartInfo.RedirectStandardError = $false
            $webStartInfo.CreateNoWindow = $false
            $webStartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = $Environment
            $webStartInfo.EnvironmentVariables["ASPNETCORE_URLS"] = "https://localhost:5003"
            $webStartInfo.EnvironmentVariables["ApiBaseUrl"] = "https://localhost:5001"
            $webStartInfo.EnvironmentVariables["Logging__Console__LogLevel__Microsoft"] = "Warning"
            $webStartInfo.EnvironmentVariables["Logging__Console__LogLevel__Microsoft.AspNetCore"] = "Warning"
            
            # File logging configuration for Production mode
            $webLogPath = Join-Path $workspaceRoot ".tmp/logs/web-prod.log"
            $webStartInfo.EnvironmentVariables["Logging__File__Path"] = $webLogPath
            
            $webProcess = [System.Diagnostics.Process]::Start($webStartInfo)
            Write-Info "  Web started from published DLL (PID: $($webProcess.Id))"
            
            # Store both processes for cleanup (use Web process as primary reference)
            $script:appHostProcess = $webProcess
            $script:apiProcess = $apiProcess
        }
        else {
            # Development/Staging: Use Aspire AppHost orchestration
            $appHostStartInfo = New-Object System.Diagnostics.ProcessStartInfo
            $appHostStartInfo.FileName = "dotnet"
            
            # Use 'run' for Staging (stable, no hot reload needed)
            # Use 'watch' for Development (enables hot reload for faster development)
            if ($Environment -eq "Staging") {
                $appHostStartInfo.Arguments = "run --project `"$appHostProjectPath`" --no-build --launch-profile $Environment --configuration $configuration"
            }
            else {
                $appHostStartInfo.Arguments = "watch --project `"$appHostProjectPath`" --no-build --launch-profile $Environment --configuration $configuration"
            }
            
            $appHostStartInfo.WorkingDirectory = Split-Path $appHostProjectPath -Parent
            $appHostStartInfo.UseShellExecute = $false
            $appHostStartInfo.RedirectStandardOutput = $false
            $appHostStartInfo.RedirectStandardError = $false
            $appHostStartInfo.CreateNoWindow = $false
            # Suppress noisy Microsoft.AspNetCore.* framework logs
            $appHostStartInfo.EnvironmentVariables["Logging__Console__LogLevel__Microsoft"] = "Warning"
            $appHostStartInfo.EnvironmentVariables["Logging__Console__LogLevel__Microsoft.AspNetCore"] = "Warning"
            
            $script:appHostProcess = [System.Diagnostics.Process]::Start($appHostStartInfo)
            
            Write-Info "  AppHost started (PID: $($script:appHostProcess.Id))"
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
                Write-Info "  Still waiting... ($attempt seconds elapsed)"
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
                
            # Check if both servers are healthy
            $serversReady = Test-ServersHealthy
            
            if ($serversReady) {
                Write-Info "  API ready ✓"
                Write-Info "  Web ready ✓"
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

    # Stop all servers and clean up ports
    function Stop-Servers {
        <#
        .SYNOPSIS
            Stops the Aspire AppHost process and cleans up all port usage.
        
        .DESCRIPTION
            Unified server shutdown function that:
            1. Stops AppHost process (SIGTERM, then SIGKILL if needed)
            2. Cleans up any processes still using ports 5001, 5003
            
            Always safe to call - handles null checks and already-stopped processes gracefully.
            
            When -CleanupOnly is specified, only disposes process references without killing
            the servers themselves. Use this when adopting orphaned servers that should keep running.
        #>
        param(
            [switch]$Silent,
            [switch]$CleanupOnly
        )
        
        # PART 1: Stop AppHost/API processes
        # In Production mode, we have separate API and Web processes
        if ($null -ne $script:apiProcess) {
            if (-not $script:apiProcess.HasExited) {
                if ($CleanupOnly) {
                    # Just cleaning up references - don't actually kill the server
                    if (-not $Silent) {
                        Write-Info "  Releasing API process reference (PID: $($script:apiProcess.Id)) - server remains running"
                    }
                }
                else {
                    # Actually stopping the server
                    try {
                        if (-not $Silent) {
                            Write-Info "  Stopping API process..."
                        }
                        
                        # Send SIGTERM for graceful shutdown
                        kill -TERM $script:apiProcess.Id 2>$null
                        
                        # Wait up to 5 seconds for graceful shutdown
                        $waited = 0
                        while (-not $script:apiProcess.HasExited -and $waited -lt 5000) {
                            Start-Sleep -Milliseconds 100
                            $waited += 100
                        }
                        
                        # If still running, force kill
                        if (-not $script:apiProcess.HasExited) {
                            if (-not $Silent) {
                                Write-Info "  API didn't stop gracefully, forcing termination..."
                            }
                            $script:apiProcess.Kill($true)
                        }
                    }
                    catch {
                        # Process might have already exited - that's OK
                    }
                }
            }
            
            # Always dispose and clear reference
            $script:apiProcess.Dispose()
            $script:apiProcess = $null
        }
        
        if ($null -ne $script:appHostProcess) {
            if (-not $script:appHostProcess.HasExited) {
                if ($CleanupOnly) {
                    # Just cleaning up references - don't actually kill the servers
                    if (-not $Silent) {
                        Write-Info "  Releasing AppHost process reference (PID: $($script:appHostProcess.Id)) - servers remain running"
                    }
                }
                else {
                    # Actually stopping the servers
                    try {
                        if (-not $Silent) {
                            Write-Info "  Sending graceful shutdown signal to Aspire AppHost (API + Web)..."
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
                            Write-Info "  Aspire AppHost (API + Web) stopped gracefully"
                        }
                    }
                    catch {
                        # Process might have already exited - that's OK
                        if (-not $Silent) {
                            Write-Info "  AppHost process already exited"
                        }
                    }
                }
            }
            
            # Always dispose and clear reference
            $script:appHostProcess.Dispose()
            $script:appHostProcess = $null
        }
        
        # PART 2: Clean up any processes still using ports
        # Skip this in CleanupOnly mode - servers are still running so ports are still in use
        if ($CleanupOnly) {
            return
        }
        
        $ports = @(5001, 5003)
        $stoppedAny = $false
        $failedKills = @()
        
        foreach ($port in $ports) {
            try {
                $portArg = ":" + $port
                $processIds = lsof -ti $portArg 2>$null
                if ($processIds) {
                    if (-not $stoppedAny -and -not $Silent) {
                        Write-Info "  Cleaning up processes on ports..."
                        $stoppedAny = $true
                    }
                    
                    # Get detailed process information for each PID
                    $processIds | ForEach-Object {
                        $processId = $_
                        try {
                            $processInfo = Get-Process -Id $processId -ErrorAction SilentlyContinue
                            if ($processInfo) {
                                if (-not $Silent) {
                                    Write-Info ("    Port {0} - Killing PID {1}: {2} (Path: {3})" -f $port, $processId, $processInfo.ProcessName, $processInfo.Path)
                                }
                            }
                            else {
                                if (-not $Silent) {
                                    Write-Info ("    Port {0} - PID {1}: (process already exited)" -f $port, $processId)
                                }
                            }
                        }
                        catch {
                            if (-not $Silent) {
                                Write-Info ("    Port {0} - PID {1}: (unable to get process details)" -f $port, $processId)
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

    # Clean up ALL other pwsh terminals (not just competing Run instances)
    # Goal: Ensure ONLY ONE active terminal (current) + PowerShell Extension
    # Uses bottom-up approach: Kill dotnet children first, then pwsh parent
    # Preserves: current terminal, PowerShell Extension, VS Code processes
    function Stop-AllOtherPwshTerminals {
        param(
            [switch]$Silent
        )
        
        if (-not $Silent) {
            Write-Step "Ensuring single active terminal (cleaning up all others)"
        }
        
        $currentPid = $PID
        $pwshProcesses = Get-Process -Name pwsh -ErrorAction SilentlyContinue
        $killedAny = $false
        
        # First, check if servers are healthy and find which pwsh owns them
        $serversHealthy = Test-ServersHealthy
        $pwshWithServers = $null
        
        if ($serversHealthy) {
            # Find which pwsh terminal owns the healthy servers
            # Look for the AppHost dotnet watch process (not port 5001, as that's served by orphaned dcp)
            $appHostLine = ps aux 2>$null | grep -E 'dotnet.*watch.*AppHost' | grep -v grep | head -1
            if ($appHostLine) {
                $appHostPidString = ($appHostLine -split '\s+')[1]
                if ($appHostPidString) {
                    $currentPid_check = [int]$appHostPidString
                    $depth = 0
                    while ($depth -lt 10) {
                        $parentPidString = ps -o ppid= -p $currentPid_check 2>$null | ForEach-Object { $_.Trim() }
                        if (-not $parentPidString -or $parentPidString -eq "0" -or $parentPidString -eq "1") { 
                            break  # Reached init/systemd or no parent
                        }
                        
                        $parentPid = [int]$parentPidString
                        $parent = Get-Process -Id $parentPid -ErrorAction SilentlyContinue
                        if ($parent -and $parent.ProcessName -eq "pwsh") {
                            $pwshWithServers = $parentPid
                            break
                        }
                        
                        $currentPid_check = $parentPid
                        $depth++
                    }
                }
            }
        }
        
        foreach ($pwsh in $pwshProcesses) {
            # Skip current terminal
            if ($pwsh.Id -eq $currentPid) {
                continue
            }
            
            # Skip PowerShell Extension (has -EncodedCommand)
            $cmdLine = cat "/proc/$($pwsh.Id)/cmdline" 2>$null | tr '\0' ' '
            if ($cmdLine -match "EncodedCommand" -or $cmdLine -match "PowerShellEditorServices") {
                if (-not $Silent) {
                    Write-Info "  KEEP PID $($pwsh.Id): PowerShell Extension"
                }
                continue
            }
            
            # Skip the pwsh that owns healthy servers
            if ($pwshWithServers -and $pwsh.Id -eq $pwshWithServers) {
                if (-not $Silent) {
                    $tty = ps -p $pwsh.Id -o tty= 2>$null | ForEach-Object { $_.Trim() }
                    Write-Info "  KEEP PID $($pwsh.Id) (TTY: $tty): Owns healthy servers"
                }
                continue
            }
            
            # Kill all other pwsh terminals
            $tty = ps -p $pwsh.Id -o tty= 2>$null | ForEach-Object { $_.Trim() }
            
            # Check if it has dotnet children
            $dotnetChildren = ps --ppid $pwsh.Id --no-headers -o pid, comm 2>$null | Select-String "dotnet"
            
            if ($dotnetChildren) {
                if (-not $Silent) {
                    Write-Info "  Killing terminal PID $($pwsh.Id) (TTY: $tty) with dotnet children"
                }
                
                # Kill dotnet children first (bottom-up)
                $childPids = ps --ppid $pwsh.Id --no-headers -o pid 2>$null
                foreach ($childPid in $childPids) {
                    try {
                        $child = Get-Process -Id $childPid -ErrorAction SilentlyContinue
                        if ($child -and $child.ProcessName -eq "dotnet") {
                            kill -TERM $childPid 2>$null
                            Start-Sleep -Milliseconds 100
                            $stillRunning = Get-Process -Id $childPid -ErrorAction SilentlyContinue
                            if ($stillRunning -and -not $stillRunning.HasExited) {
                                kill -9 $childPid 2>$null
                            }
                        }
                    }
                    catch { }
                }
            }
            else {
                if (-not $Silent) {
                    Write-Info "  Killing idle terminal PID $($pwsh.Id) (TTY: $tty)"
                }
            }
            
            # Kill the pwsh terminal
            kill -9 $pwsh.Id 2>$null
            $killedAny = $true
        }
        
        if ($killedAny) {
            # Wait for processes to fully terminate
            Start-Sleep -Milliseconds 500
            if (-not $Silent) {
                if ($null -ne $pwshWithServers) {
                    Write-Success "Unused terminals cleaned up - servers remain running in terminal PID $pwshWithServers"
                }
                else {
                    Write-Success "Unused terminals cleaned up"
                }
            }
        }
        else {
            if (-not $Silent) {
                Write-Info "  No other terminals found - this is already the only active terminal"
            }
        }
        
        return $killedAny
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
        
        # Clean up log files
        $logPath = Join-Path $workspaceRoot ".tmp/logs/*.log"
        if (Test-Path $logPath) {
            Remove-Item $logPath -Force -ErrorAction SilentlyContinue
            if (-not $Silent) {
                Write-Info "  Removed old log files"
            }
        }
        
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
                Write-Info "  No orphaned processes found"
            }
        }
    }

    # Adopt orphaned SERVERS (Aspire AppHost process) after killing competing Run instances
    # SERVERS = Aspire AppHost + its child processes (API, Web)
    # This allows the new Run instance to properly manage and cleanup servers on Ctrl+C
    function Adopt-OrphanedServerProcesses {
        Write-Step "Adopting existing Aspire AppHost process"
        
        # Strategy: Find AppHost dotnet watch process by pattern
        # Note: Cannot use port 5001 because that's served by orphaned dcp (parent = PID 1)
        try {
            # Find the AppHost dotnet watch process
            $appHostLine = ps aux 2>$null | grep -E 'dotnet.*watch.*AppHost' | grep -v grep | head -1
            
            if ($appHostLine) {
                $appHostPidString = ($appHostLine -split '\s+')[1]
                if ($appHostPidString) {
                    $appHostPid = [int]$appHostPidString
                    $appHostProcess = Get-Process -Id $appHostPid -ErrorAction SilentlyContinue
                    
                    if ($appHostProcess) {
                        # Adopt this process
                        $script:appHostProcess = $appHostProcess
                        Write-Info "  Adopted Aspire AppHost (PID: $appHostPid)"
                        Write-Info "    └─ dotnet watch running AppHost orchestration"
                        Write-Success "Aspire AppHost adopted - will be managed by this instance"
                        return $true
                    }
                }
            }
        }
        catch {
            Write-Warning "Failed to adopt AppHost: $($_.Exception.Message)"
        }
        
        Write-Info "  Could not find Aspire AppHost to adopt"
        Write-Info "  Servers will be restarted"
        return $false
    }

    # Stop competing Run function instances in other terminals
    # This prevents multiple Run instances from interfering with each other
    # If servers are healthy, kills pwsh without triggering cleanup (servers keep running)
    # If servers are unhealthy, triggers cleanup (servers will be restarted)
    function Stop-CompetingRunInstances {
        Write-Step "Checking for competing Run instances"
        
        $currentPid = $PID
        $killedAny = $false
        
        # Check if servers are healthy FIRST (determines cleanup strategy)
        $serversHealthy = Test-ServersHealthy
        
        # Find dotnet processes on our ports (5001, 5003)
        $ports = @(5001, 5003)
        $competingPwshPids = @()
        
        foreach ($port in $ports) {
            try {
                $portArg = ":" + $port
                $dotnetPids = lsof -ti $portArg 2>$null
                
                if ($dotnetPids) {
                    foreach ($dotnetPid in $dotnetPids) {
                        # Get the parent process of this dotnet process
                        $parentPid = ps -o ppid= -p $dotnetPid 2>$null
                        if ($parentPid) {
                            $parentPid = $parentPid.Trim()
                            
                            # Check if parent is a pwsh process (and not us)
                            if ($parentPid -ne $currentPid) {
                                $parentProcess = Get-Process -Id $parentPid -ErrorAction SilentlyContinue
                                if ($parentProcess -and $parentProcess.ProcessName -eq 'pwsh') {
                                    # This is a competing pwsh instance running dotnet on our ports
                                    if ($competingPwshPids -notcontains $parentPid) {
                                        $competingPwshPids += $parentPid
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch {
                # Ignore errors - port might not be in use
            }
        }
        
        # Kill competing pwsh instances
        if ($competingPwshPids.Count -gt 0) {
            if ($serversHealthy) {
                Write-Info "  Found $($competingPwshPids.Count) competing Run instance(s) with healthy servers"
                Write-Info "  Detaching competing instances (servers will keep running)..."
            }
            else {
                Write-Info "  Found $($competingPwshPids.Count) competing Run instance(s) with unhealthy servers"
                Write-Info "  Stopping competing instances (servers will be cleaned up)..."
            }
            
            foreach ($pwshPid in $competingPwshPids) {
                try {
                    $pwshProcess = Get-Process -Id $pwshPid -ErrorAction SilentlyContinue
                    if ($pwshProcess) {
                        if ($serversHealthy) {
                            # Servers are healthy - we want to reuse them
                            # Strategy: Clean up our references, kill competing pwsh, then adopt the servers
                            Write-Info "    Detaching instance (PID: $pwshPid) - servers will be adopted"
                            
                            # Clean up our process references without killing servers
                            Stop-Servers -Silent -CleanupOnly
                            
                            # SIGKILL the competing pwsh (prevents its finally block from stopping servers)
                            # Note: Terminal will stay open in VS Code but process is terminated (exit code 137)
                            kill -9 $pwshPid 2>$null
                        }
                        else {
                            # Servers are unhealthy - need to stop them cleanly and restart
                            Write-Info "    Stopping instance (PID: $pwshPid) and its servers"
                            
                            # SIGTERM first to allow graceful cleanup
                            kill -TERM $pwshPid 2>$null
                            
                            # Wait up to 3 seconds for graceful shutdown
                            $waited = 0
                            while (-not $pwshProcess.HasExited -and $waited -lt 3000) {
                                Start-Sleep -Milliseconds 100
                                $waited += 100
                            }
                            
                            # If still running after SIGTERM, manually stop servers before SIGKILL
                            # This prevents orphaning the AppHost process
                            if (-not $pwshProcess.HasExited) {
                                Write-Info "    Instance didn't exit gracefully, stopping servers manually..."
                                # Adopt the AppHost so we can stop it
                                if (Adopt-OrphanedServerProcesses) {
                                    # Successfully adopted - now stop the servers
                                    Stop-Servers -Silent
                                }
                                else {
                                    # Couldn't adopt - kill by ports as last resort
                                    Write-Info "    Couldn't adopt servers, cleaning up by port..."
                                    $ports = @(5001, 5003)
                                    foreach ($port in $ports) {
                                        $portPids = lsof -ti ":$port" 2>$null
                                        if ($portPids) {
                                            $portPids | ForEach-Object { kill -9 $_ 2>$null }
                                        }
                                    }
                                }
                                # Now safe to force kill the pwsh instance
                                kill -9 $pwshPid 2>$null
                            }
                        }
                        
                        $killedAny = $true
                    }
                }
                catch {
                    # Process might have already exited - that's OK
                }
            }
            
            # Give cleanup time to complete if servers were unhealthy
            if ($killedAny -and -not $serversHealthy) {
                Write-Info "  Waiting for cleanup to complete..."
                Start-Sleep -Seconds 2
            }
        }
        
        if ($killedAny) {
            if ($serversHealthy) {
                Write-Success "Competing instances detached - reusing healthy servers"
                # Adopt the orphaned server processes so we can manage them
                Adopt-OrphanedServerProcesses
            }
            else {
                Write-Success "Competing instances stopped - servers will be restarted"
            }
        }
        else {
            Write-Info "  No competing instances found"
        }
    }

    # Main execution
    try {
        # Initialize script-level variable to track server ownership
        # CRITICAL: Must be set before any early returns to prevent errors in finally block
        $script:weOwnTheServers = $false
        
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
        
        # COMPREHENSIVE CLEANUP: Ensure ONLY ONE active terminal (this one)
        # Strategy: Kill ALL other pwsh terminals using bottom-up approach
        #   1. Kill dotnet children first (servers, build workers), then pwsh parent
        #   2. This prevents orphaning processes when killing pwsh terminals
        # Preserves: current terminal, PowerShell Extension, VS Code processes
        $null = Stop-AllOtherPwshTerminals
        
        # Clean up any remaining orphaned test/build processes
        # This catches orphaned browsers, test runners, MSBuild workers that survived
        $null = Stop-OrphanedTestProcesses
        
        # Try to adopt any surviving server processes (if servers are healthy)
        # This allows reusing existing healthy servers without restart
        # If adoption succeeds, $script:appHostProcess will be set
        # If adoption fails, servers will be restarted later by smart restart logic
        $null = Adopt-OrphanedServerProcesses
        
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
        
        # Track ownership: if we restart/start servers, we own them
        # If we reuse healthy servers, another terminal owns them
        $script:weOwnTheServers = $needsRestart
        
        if ($needsRestart) {
            Write-Step "Restart decision"
            if ($buildResult.SrcRebuilt) {
                Write-Info "  Source changes detected - binaries were rebuilt"
                Write-Info "  Restarting servers to load new code..."
            }
            elseif (-not $serversHealthy) {
                Write-Info "  Servers are unhealthy or not running"
                Write-Info "  Restarting servers..."
            }
            
            # Stop existing servers and clean up ports
            Stop-Servers
        }
        else {
            Write-Step "Restart decision"
            Write-Info "  No source changes detected - binaries unchanged"
            Write-Info "  Servers are healthy - no restart needed"
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
            $serversStarted = Start-AppHost -Environment $Environment
            if ($serversStarted -ne $true) {
                # Server startup failed - ALWAYS clean up (can't leave non-running servers)
                Stop-Servers
                return
            }
        }
    
        # Servers are running - show info and handle based on ownership
        # (Either started with -WithoutTests, or E2E tests ran without -StopServers)
        if ($WithoutTests -or ($runE2E -and -not $StopServers)) {
            Write-Host ""
            Write-Info "Services:"
            Write-Info "  API: https://localhost:5001 (Swagger: https://localhost:5001/swagger)"
            Write-Info "  Web: https://localhost:5003"
            Write-Info "  Dashboard: https://localhost:18888 (Aspire Dashboard)"
            Write-Host ""
            
            # Check if we own the servers or they're running in another terminal
            if ($script:weOwnTheServers) {
                # We started the servers - this terminal manages them
                Write-Info "Press Ctrl+C to stop servers"
                Write-Host ""
                Write-Host "CRITICAL: Do not execute new commands in this terminal as this will also act as Ctrl-C and stop the servers" -ForegroundColor Yellow
            
                # Wait for user interrupt (Ctrl+C)
                # Cleanup happens in main finally block
                if ($null -ne $script:appHostProcess -and -not $script:appHostProcess.HasExited) {
                    $script:appHostProcess.WaitForExit()
                }
            }
            else {
                # Servers are running in another terminal - we're just a visitor
                Write-Success "Servers already running in another terminal"
                Write-Info "This terminal exiting - servers remain running"
                Write-Host ""
                Write-Host "CRITICAL: This terminal is now free to execute new commands in" -ForegroundColor Green
                # Don't wait, just exit cleanly (no cleanup in finally block)
                return
            }
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
        # Only cleanup if we own the servers
        # If we're reusing servers from another terminal, don't touch them!
        # Check variable exists first (handles early returns before variable is set)
        if ((Get-Variable -Name 'weOwnTheServers' -Scope Script -ErrorAction SilentlyContinue) -and $script:weOwnTheServers) {
            # Gracefully stop AppHost process and clean up ports
            Stop-Servers -Silent
            
            # Clean up orphaned test processes (browsers, testhost, vstest)
            # This runs on Ctrl+C, errors, or normal exit
            Stop-OrphanedTestProcesses -Silent
        }
        
        # Always return to workspace root
        Set-Location $workspaceRoot
    }
}

# Export the Run function
Export-ModuleMember -Function Run
