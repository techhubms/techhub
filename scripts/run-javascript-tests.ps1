#!/usr/bin/env pwsh

# JavaScript Test Runner Script for Tech Hub
# This script ensures Node.js dependencies are installed and executes the Jest test suite

param(
    [Alias('TestPath')]
    [string]$TestFile = "",
    [string]$TestName = "",
    [switch]$Coverage,
    [switch]$Watch,
    [switch]$Verbose
)

$ErrorActionPreference = "Stop"
$ProgressPreference = 'SilentlyContinue'
Set-StrictMode -Version Latest

# Define script-level variables
$script:scriptDir = $PSScriptRoot
$script:rootDir = Split-Path -Parent $script:scriptDir
$script:jsTestDir = Join-Path $script:rootDir "spec/javascript"

# Colors for output
$Red = "`e[31m"
$Green = "`e[32m"
$Yellow = "`e[33m"
$Blue = "`e[34m"
$Reset = "`e[0m"

function Write-ColoredOutput {
    param($Message, $Color)
    Write-Host "$Color$Message$Reset"
}

function Test-NodeInstalled {
    try {
        $result = Get-Command node -ErrorAction SilentlyContinue
        if ($null -eq $result) {
            return $false
        }
        
        # Verify node is actually working
        node --version 2>$null | Out-Null
        return $LASTEXITCODE -eq 0
    }
    catch {
        return $false
    }
}

function Test-DependenciesInstalled {
    # Check if node_modules exists and jest is installed
    $nodeModules = Join-Path $script:jsTestDir "node_modules"
    $jestBinary = Join-Path $nodeModules ".bin/jest"
    
    if (-not (Test-Path $nodeModules)) {
        return $false
    }
    
    if (-not (Test-Path $jestBinary)) {
        return $false
    }
    
    # Dependencies are installed if we reach here
    return $true
}

function Invoke-JavaScriptTests {
    param($TestArgs)
    
    Set-Location $script:jsTestDir
    
    Write-ColoredOutput "Running JavaScript tests..." $Blue
    Write-ColoredOutput "Test directory: $script:jsTestDir" $Yellow
    
    # Show test files being discovered
    Write-ColoredOutput "Discovering JavaScript test files..." $Yellow
    $testFiles = Get-ChildItem -Path "*.test.js" -Name
    if ($testFiles.Count -gt 0) {
        Write-ColoredOutput "Found JavaScript test files:" $Green
        foreach ($file in $testFiles) {
            Write-ColoredOutput "  - $file" $Green
        }
    } else {
        Write-ColoredOutput "No JavaScript test files found in spec/javascript/ directory" $Red
        return 1
    }
    
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "Starting JavaScript test execution..." $Blue
    Write-ColoredOutput "Command: npm test $($TestArgs -join ' ')" $Yellow
    Write-ColoredOutput "================================" $Blue
    Write-ColoredOutput "" $Reset
    
    # Build the npm test command arguments
    $npmArgs = @("test")
    
    if ($TestArgs) {
        $npmArgs += "--"
        $npmArgs += $TestArgs
    }
    
    # Execute the tests using Start-Process to ensure output is visible
    $arguments = $npmArgs -join " "
    $process = Start-Process -FilePath "npm" -ArgumentList $npmArgs -NoNewWindow -Wait -PassThru
    $exitCode = $process.ExitCode
    
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "================================" $Blue
    
    if ($exitCode -eq 0) {
        Write-ColoredOutput "✅ All JavaScript tests passed!" $Green
    } else {
        Write-ColoredOutput "❌ Some JavaScript tests failed. Exit code: $exitCode" $Red
        Write-ColoredOutput "" $Reset
        Write-ColoredOutput "To debug failed tests, you can run:" $Yellow
        Write-ColoredOutput "  run-javascript-tests.ps1 -Verbose" $Yellow
        Write-ColoredOutput "  run-javascript-tests.ps1 -TestFile 'filters.test.js'" $Yellow
        Write-ColoredOutput "  run-javascript-tests.ps1 -Watch" $Yellow
    }
    
    return $exitCode
}

function Test-JestInstalled {
    Set-Location $script:jsTestDir
    
    try {
        # Check if jest is available via npm
        npm list jest 2>$null | Out-Null
        return $LASTEXITCODE -eq 0
    }
    catch {
        return $false
    }
}

function Invoke-JavascriptTestsRunner {
    # Main execution
    try {
        Write-ColoredOutput "Tech Hub JavaScript Test Runner" $Blue
        Write-ColoredOutput "====================================" $Blue
        Write-ColoredOutput "" $Reset
        
        # Check if Node.js is installed
        Write-ColoredOutput "🔍 Checking Node.js installation..." $Yellow
        if (-not (Test-NodeInstalled)) {
            Write-ColoredOutput "❌ Node.js not found. Please install Node.js and npm first." $Red
            Write-ColoredOutput "💡 Visit: https://nodejs.org/en/download/" $Yellow
            exit 1
        } else {
            $nodeVersion = node --version
            Write-ColoredOutput "✅ Node.js is installed (version: $nodeVersion)" $Green
        }
        
        # Check and install dependencies if needed
        Write-ColoredOutput "🔍 Checking JavaScript dependencies..." $Yellow
        
        # Ensure we're in the right directory and package.json exists
        if (-not (Test-Path (Join-Path $script:jsTestDir "package.json"))) {
            Write-ColoredOutput "❌ package.json not found in spec/javascript directory. Exiting." $Red
            exit 1
        }
        
        if (-not (Test-DependenciesInstalled)) {
            Write-ColoredOutput "❌  Dependencies not found." $Red
            exit 1
        }
        
        # Verify Jest is available
        Write-ColoredOutput "🔍 Verifying Jest availability..." $Yellow
        if (-not (Test-JestInstalled)) {
            Write-ColoredOutput "❌ Jest not found in dependencies. Please check package.json." $Red
            exit 1
        } else {
            Write-ColoredOutput "✅ Jest is available" $Green
        }
        
        # Build test arguments
        $testArgs = @()
        
        # Handle specific test file
        if ($TestFile) {
            $testArgs += $TestFile
            Write-ColoredOutput "🎯 Running specific file: $TestFile" $Yellow
        } else {
            Write-ColoredOutput "🧪 Running all JavaScript tests" $Yellow
        }
        
        # Handle test name pattern
        if ($TestName) {
            $testArgs += "--testNamePattern=`"$TestName`""
            Write-ColoredOutput "🔍 Filtering tests by name: $TestName" $Yellow
        }
        
        # Add coverage if requested
        if ($Coverage) {
            $testArgs += "--coverage"
            Write-ColoredOutput "📊 Code coverage analysis enabled" $Yellow
        }
        
        # Add watch mode if requested
        if ($Watch) {
            $testArgs += "--watch"
            Write-ColoredOutput "👀 Watch mode enabled for development" $Yellow
        }
        
        # Add verbose output if requested
        if ($Verbose) {
            $testArgs += "--verbose"
            Write-ColoredOutput "📋 Verbose output enabled" $Yellow
        }
        
        Write-ColoredOutput "" $Reset
        
        # Run the tests
        $result = Invoke-JavaScriptTests $testArgs
        
        Write-ColoredOutput "" $Reset
        if ($result -eq 0) {
            Write-ColoredOutput "🎉 JavaScript test execution completed successfully!" $Green
        } else {
            Write-ColoredOutput "💥 JavaScript test execution completed with failures!" $Red
        }
        
        exit $result
        
    } catch {
        Write-ColoredOutput "💥 An error occurred: $($_.Exception.Message)" $Red
        Write-ColoredOutput "Stack trace: $($_.Exception.StackTrace)" $Red
        exit 1
    }
}

$location = Get-Location
try {
    Invoke-JavascriptTestsRunner
}
finally {
    Set-Location $location
}