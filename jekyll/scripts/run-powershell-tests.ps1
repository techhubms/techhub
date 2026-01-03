#!/usr/bin/env pwsh

# PowerShell Test Runner Script for Tech Hub
# This script ensures Pester is installed and executes the PowerShell test suite
# Cross-platform compatible for Windows, Linux, and devcontainers

param(
    [Alias('TestPath')]
    [string]$TestFile = "",
    [string]$TestName = "",
    [switch]$Coverage,
    [switch]$Detailed,
    [switch]$Verbose
)

$ErrorActionPreference = "Stop"
$ProgressPreference = 'SilentlyContinue'
Set-StrictMode -Version Latest

# Colors for output (compatible with both Windows and Linux terminals)
if ($IsWindows -or ($PSVersionTable.Platform -eq "Win32NT")) {
    # Windows PowerShell or PowerShell Core on Windows
    $Red = "`e[31m"
    $Green = "`e[32m"
    $Yellow = "`e[33m"
    $Blue = "`e[34m"
    $Reset = "`e[0m"
} else {
    # Linux/macOS PowerShell Core
    $Red = "`e[31m"
    $Green = "`e[32m"
    $Yellow = "`e[33m"
    $Blue = "`e[34m"
    $Reset = "`e[0m"
}

# Detect environment
function Get-Environment {
    if ($env:GITHUB_ACTIONS -eq "true") {
        return "GitHubActions"
    } elseif ($env:CODESPACES -eq "true") {
        return "GitHubCodespaces"
    } elseif ((Test-Path "/.devcontainer.json") -or $env:REMOTE_CONTAINERS -eq "true") {
        return "DevContainer"
    } elseif ($IsWindows -or ($PSVersionTable.Platform -eq "Win32NT")) {
        return "Windows"
    } elseif ($IsLinux) {
        return "Linux"
    } elseif ($IsMacOS) {
        return "MacOS"
    } else {
        return "Unknown"
    }
}

function Get-ProjectRoot {
    # Load Get-SourceRoot function if available
    $sourceRootPath = Join-Path $PSScriptRoot "functions/Get-SourceRoot.ps1"
    if (Test-Path $sourceRootPath) {
        . $sourceRootPath
        return Get-SourceRoot
    }
    
    # Fallback logic
    if ($env:GITHUB_WORKSPACE) {
        return $env:GITHUB_WORKSPACE
    }
    
    # Search upward for repository indicators
    $currentPath = $PSScriptRoot
    while ($currentPath -and $currentPath -ne [System.IO.Path]::GetPathRoot($currentPath)) {
        if (Test-Path (Join-Path $currentPath ".git")) {
            return $currentPath
        }
        if (Test-Path (Join-Path $currentPath "_config.yml")) {
            return $currentPath
        }
        $currentPath = Split-Path $currentPath -Parent
    }
    
    # Ultimate fallback
    return $PSScriptRoot
}

function Invoke-WithRetry {
    param(
        [ScriptBlock]$Command,
        [int]$MaxAttempts = 3,
        [int]$DelaySeconds = 5,
        [string]$OperationName = "Operation"
    )
    
    for ($attempt = 1; $attempt -le $MaxAttempts; $attempt++) {
        try {
            Write-ColoredOutput "🔄 $OperationName (attempt ${attempt}/${MaxAttempts})..." $Yellow
            
            $result = & $Command
            
            if ($LASTEXITCODE -eq 0 -or $result) {
                Write-ColoredOutput "✅ $OperationName succeeded on attempt ${attempt}" $Green
                return $true
            }
            
            if ($attempt -lt $MaxAttempts) {
                Write-ColoredOutput "⚠️  $OperationName failed on attempt ${attempt}, retrying in ${DelaySeconds}s..." $Yellow
                Start-Sleep -Seconds $DelaySeconds
            }
        }
        catch {
            if ($attempt -lt $MaxAttempts) {
                Write-ColoredOutput "⚠️  $OperationName error on attempt ${attempt}: $($_.Exception.Message)" $Yellow
                Write-ColoredOutput "🔄 Retrying in ${DelaySeconds}s..." $Yellow
                Start-Sleep -Seconds $DelaySeconds
            } else {
                Write-ColoredOutput "❌ $OperationName failed after $MaxAttempts attempts: $($_.Exception.Message)" $Red
                throw
            }
        }
    }
    
    Write-ColoredOutput "❌ $OperationName failed after $MaxAttempts attempts" $Red
    return $false
}

function Write-ColoredOutput {
    param($Message, $Color)
    Write-Host "$Color$Message$Reset"
}

function Test-PesterInstalled {
    try {
        $pesterModule = Get-Module -Name Pester -ListAvailable | Where-Object { $_.Version -ge [Version]"5.0.0" }
        return $null -ne $pesterModule
    }
    catch {
        return $false
    }
}

function Invoke-PowerShellTests {
    param($TestArgs)
    
    $projectRoot = Get-ProjectRoot
    $environment = Get-Environment
    
    Write-ColoredOutput "Environment: $environment" $Yellow
    Write-ColoredOutput "Project root: $projectRoot" $Yellow
    Write-ColoredOutput "Running PowerShell tests..." $Blue
    
    # Determine test directory path
    $testDirectory = Join-Path $projectRoot "spec/powershell"
    
    Write-ColoredOutput "Test directory: $testDirectory" $Yellow
    
    # Check if test directory exists
    if (-not (Test-Path $testDirectory)) {
        Write-ColoredOutput "❌ PowerShell test directory not found: $testDirectory" $Red
        Write-ColoredOutput "💡 Expected directory structure: spec/powershell/*.Tests.ps1" $Yellow
        return 1
    }
    
    # Show test files being discovered
    Write-ColoredOutput "Discovering PowerShell test files..." $Yellow
    $testFiles = Get-ChildItem -Path (Join-Path $testDirectory "*.Tests.ps1") -ErrorAction SilentlyContinue
    
    if ($testFiles.Count -gt 0) {
        Write-ColoredOutput "Found PowerShell test files:" $Green
        foreach ($file in $testFiles) {
            Write-ColoredOutput "  - $($file.Name)" $Green
        }
    } else {
        Write-ColoredOutput "❌ No PowerShell test files found in $testDirectory" $Red
        Write-ColoredOutput "💡 Looking for files matching pattern: *.Tests.ps1" $Yellow
        
        # Show what files are actually there
        $allFiles = Get-ChildItem -Path $testDirectory -ErrorAction SilentlyContinue
        if ($allFiles.Count -gt 0) {
            Write-ColoredOutput "Files found in directory:" $Yellow
            foreach ($file in $allFiles) {
                Write-ColoredOutput "  - $($file.Name)" $Yellow
            }
        }
        return 1
    }
    
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "Starting PowerShell test execution..." $Blue
    Write-ColoredOutput "Command: Invoke-Pester -Configuration <PesterConfiguration>" $Yellow
    Write-ColoredOutput "================================" $Blue
    Write-ColoredOutput "" $Reset
    
    # Set working directory to project root
    Set-Location $projectRoot
    
    # Import Pester module explicitly
    try {
        Import-Module Pester -Force -ErrorAction Stop
        Write-ColoredOutput "✅ Pester module imported successfully" $Green
    }
    catch {
        Write-ColoredOutput "❌ Failed to import Pester module: $($_.Exception.Message)" $Red
        return 1
    }
    
    # Use Pester 5.x configuration syntax
    if ($TestArgs["Path"]) {
        $pesterPath = $TestArgs["Path"]
    } else {
        $pesterPath = $testDirectory
    }
    
    # Ensure path exists before running tests
    if (-not (Test-Path $pesterPath)) {
        Write-ColoredOutput "❌ Test path does not exist: $pesterPath" $Red
        return 1
    }
    
    # Build Pester 5.x configuration object
    $pesterConfig = [PesterConfiguration]::Default
    
    # Set basic configuration
    $pesterConfig.Run.Path = @($pesterPath)
    $pesterConfig.Run.PassThru = $true
    
    # Configure output
    if ($TestArgs["Output"] -eq "Detailed") {
        $pesterConfig.Output.Verbosity = 'Detailed'
    } else {
        $pesterConfig.Output.Verbosity = 'Normal'
    }
    
    # Configure code coverage if requested
    if ($TestArgs["CodeCoverage"]) {
        # Make code coverage path relative to project root
        $coveragePath = Join-Path $projectRoot "scripts/content-processing/functions/*.ps1"
        if (Test-Path (Split-Path $coveragePath -Parent)) {
            $pesterConfig.CodeCoverage.Enabled = $true
            $pesterConfig.CodeCoverage.Path = @($coveragePath)
        } else {
            Write-ColoredOutput "⚠️  Code coverage path not found: $coveragePath" $Yellow
        }
    }
    
    # Handle TestName filtering using Pester 5.x Filter configuration
    if ($TestArgs["TestName"]) {
        Write-ColoredOutput "🔍 Applying test name filter: $($TestArgs["TestName"])" $Yellow
        $pesterConfig.Filter.FullName = $TestArgs["TestName"]
    }
    
    Write-ColoredOutput "Pester configuration:" $Yellow
    Write-ColoredOutput "  Path: $($pesterConfig.Run.Path.Value)" $Yellow
    Write-ColoredOutput "  Verbosity: $($pesterConfig.Output.Verbosity.Value)" $Yellow
    Write-ColoredOutput "  PassThru: $($pesterConfig.Run.PassThru.Value)" $Yellow
    if ($pesterConfig.CodeCoverage.Enabled.Value) {
        Write-ColoredOutput "  CodeCoverage: $($pesterConfig.CodeCoverage.Path.Value)" $Yellow
    }
    if ($pesterConfig.Filter.FullName.Value) {
        Write-ColoredOutput "  Filter: $($pesterConfig.Filter.FullName.Value)" $Yellow
    }
    Write-ColoredOutput "" $Reset
    
    # Execute the tests using Pester 5.x configuration
    try {
        $result = Invoke-Pester -Configuration $pesterConfig
    }
    catch {
        Write-ColoredOutput "❌ Test execution failed: $($_.Exception.Message)" $Red
        Write-ColoredOutput "Stack trace: $($_.Exception.StackTrace)" $Red
        return 1
    }
    
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "================================" $Blue
    
    # Check for successful test execution - tests pass if no failures occurred and tests ran
    if ($result -and $result.TotalCount -gt 0) {
        $passedAllTests = ($result.FailedCount -eq 0)
        
        if ($passedAllTests) {
            Write-ColoredOutput "✅ All PowerShell tests passed! ($($result.PassedCount)/$($result.TotalCount))" $Green
            return 0
        } else {
            Write-ColoredOutput "❌ Some PowerShell tests failed. Failed: $($result.FailedCount), Total: $($result.TotalCount)" $Red
            Write-ColoredOutput "" $Reset
            Write-ColoredOutput "To debug failed tests, you can run:" $Yellow
            Write-ColoredOutput "  $($MyInvocation.MyCommand.Name) -Detailed" $Yellow
            Write-ColoredOutput "  $($MyInvocation.MyCommand.Name) -Verbose" $Yellow
            Write-ColoredOutput "  $($MyInvocation.MyCommand.Name) -TestFile 'spec/powershell/Get-FilteredTags.Tests.ps1'" $Yellow
            return 1
        }
    } else {
        Write-ColoredOutput "❌ No tests were executed or result is invalid" $Red
        if ($result) {
            Write-ColoredOutput "Result details: TotalCount=$($result.TotalCount), PassedCount=$($result.PassedCount), FailedCount=$($result.FailedCount)" $Yellow
        } else {
            Write-ColoredOutput "Result object is null or invalid" $Yellow
        }
        return 1
    }
}

function Invoke-PowerShellTestsRunner() {
    # Main execution
    try {
        $environment = Get-Environment
        $projectRoot = Get-ProjectRoot
        
        Write-ColoredOutput "Tech Hub PowerShell Test Runner" $Blue
        Write-ColoredOutput "======================================" $Blue
        Write-ColoredOutput "Environment: $environment" $Yellow
        Write-ColoredOutput "Project Root: $projectRoot" $Yellow
        Write-ColoredOutput "PowerShell Version: $($PSVersionTable.PSVersion)" $Yellow
        Write-ColoredOutput "" $Reset
        
        # Check and install Pester if needed
        Write-ColoredOutput "🔍 Checking Pester framework..." $Yellow
        
        if (-not (Test-PesterInstalled)) {
            Write-ColoredOutput "❌ Pester 5.x not found" $Red
            Write-ColoredOutput "💡 Please ensure Pester is installed via post-create.sh or CI setup" $Yellow
            exit 1
        } else {
            Write-ColoredOutput "✅ Pester framework already installed" $Green
        }
        
        # Build test arguments
        $testArgs = @{}
        
        # Handle specific test file
        if ($TestFile) {
            # Make test file path relative to project root if needed
            if ([System.IO.Path]::IsPathRooted($TestFile)) {
                $testArgs.Path = $TestFile
            } else {
                $testArgs.Path = Join-Path $projectRoot $TestFile
            }
            Write-ColoredOutput "🎯 Running specific file: $($testArgs.Path)" $Yellow
        } else {
            # Default to spec/powershell directory
            $testArgs.Path = Join-Path $projectRoot "spec/powershell"
            Write-ColoredOutput "🧪 Running all PowerShell tests in spec/powershell/ directory" $Yellow
        }
        
        # Handle test name pattern
        if ($TestName) {
            $testArgs.TestName = "*$TestName*"
            Write-ColoredOutput "🔍 Filtering tests by name: $TestName" $Yellow
        }
        
        # Add output format
        if ($Detailed -or $Verbose) {
            $testArgs.Output = "Detailed"
            Write-ColoredOutput "📋 Using detailed output format" $Yellow
        } else {
            $testArgs.Output = "Normal"
            Write-ColoredOutput "📋 Using normal output format" $Yellow
        }
        
        # Add code coverage if requested
        if ($Coverage) {
            $coveragePath = Join-Path $projectRoot "scripts/content-processing/functions/*.ps1"
            $testArgs.CodeCoverage = $coveragePath
            Write-ColoredOutput "📊 Code coverage analysis enabled: $coveragePath" $Yellow
        }
        
        Write-ColoredOutput "" $Reset
        
        # Run the tests
        $result = Invoke-PowerShellTests $testArgs
        
        Write-ColoredOutput "" $Reset
        if ($result -eq 0) {
            Write-ColoredOutput "🎉 PowerShell test execution completed successfully!" $Green
        } else {
            Write-ColoredOutput "💥 PowerShell test execution completed with failures!" $Red
        }
        
        exit $result
        
    } catch {
        Write-ColoredOutput "💥 An error occurred: $($_.Exception.Message)" $Red
        Write-ColoredOutput "Stack trace: $($_.Exception.StackTrace)" $Red
        exit 1
    }
}

$originalLocation = Get-Location
try {    
    Invoke-PowerShellTestsRunner
}
finally {
    Set-Location $originalLocation
}