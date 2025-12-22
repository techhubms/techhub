#!/usr/bin/env pwsh

# Ruby Plugin Test Runner Script for Tech Hub
# This script ensures dependencies are installed and executes the RSpec test suite

param(
    [string]$SpecFile = "",
    [string]$Line = "",
    [switch]$Documentation,
    [switch]$Progress,
    [switch]$Verbose
)

$ErrorActionPreference = "Stop"
$ProgressPreference = 'SilentlyContinue'
Set-StrictMode -Version Latest

# Colors for output
$Red = "`e[31m"
$Green = "`e[32m"
$Yellow = "`e[33m"
$Blue = "`e[34m"
$Reset = "`e[0m"

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

function Test-BundlerInstalled {
    try {
        # Check if bundler command is available in PATH
        $result = Get-Command bundler -ErrorAction SilentlyContinue
        if ($null -ne $result) {
            bundler --version 2>$null | Out-Null
            if ($LASTEXITCODE -eq 0) {
                return $true
            }
        }
        
        # Check if bundle command is available in PATH
        $result = Get-Command bundle -ErrorAction SilentlyContinue
        if ($null -ne $result) {
            bundle --version 2>$null | Out-Null
            if ($LASTEXITCODE -eq 0) {
                return $true
            }
        }
        
        # Check for user-local bundler installation
        $userGemBin = "$env:HOME/.local/share/gem/ruby/3.2.0/bin"
        if (Test-Path "$userGemBin/bundle") {
            # Add to PATH for this session
            $env:PATH = "$userGemBin" + [System.IO.Path]::PathSeparator + $env:PATH
            Write-ColoredOutput "✅ Found user-local bundler, added to PATH" $Green
            return $true
        }
        
        return $false
    }
    catch {
        return $false
    }
}

function Test-GemsInstalled {
    try {
        # Check if Gemfile exists
        if (-not (Test-Path "Gemfile")) {
            Write-ColoredOutput "❌ Gemfile not found in project root" $Red
            return $false
        }
        
        # Check if bundler is installed first
        if (-not (Test-BundlerInstalled)) {
            return $false
        }
        
        # Run bundle check to see if gems are installed and up to date
        bundle check 2>$null | Out-Null
        return $LASTEXITCODE -eq 0
    }
    catch {
        return $false
    }
}

function Invoke-RSpecTests {
    param([string[]]$RSpecArgs)
    
    Write-ColoredOutput "Running RSpec tests..." $Blue
    Write-ColoredOutput "Project directory: $PSScriptRoot" $Yellow
    
    # Show test files being discovered
    Write-ColoredOutput "Discovering Ruby test files..." $Yellow
    $specFiles = Get-ChildItem -Path "spec/plugins/*_spec.rb" -Name
    if ($specFiles.Count -gt 0) {
        Write-ColoredOutput "Found Ruby test files:" $Green
        foreach ($file in $specFiles) {
            Write-ColoredOutput "  - $file" $Green
        }
    } else {
        Write-ColoredOutput "No Ruby test files found in spec/plugins/ directory" $Red
        return 1
    }
    
    # Execute the tests directly with proper argument handling
    $env:FORCE_COLOR = "true"  # Ensure colored output in CI/container environments

    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "Starting Ruby test execution..." $Blue
    Write-ColoredOutput "Command: bundle exec rspec $($RSpecArgs -join ' ')" $Yellow
    Write-ColoredOutput "================================" $Blue
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "🚀 Test execution starting..." $Blue
    Write-ColoredOutput "" $Reset
    
    # Use Start-Process with proper output streaming for complete visibility
    $bundleArgs = @("exec", "rspec") + $RSpecArgs
    
    $processInfo = New-Object System.Diagnostics.ProcessStartInfo
    $processInfo.FileName = "bundle"
    $processInfo.Arguments = ($bundleArgs -join " ")
    $processInfo.UseShellExecute = $false
    $processInfo.RedirectStandardOutput = $false
    $processInfo.RedirectStandardError = $false
    
    $process = [System.Diagnostics.Process]::Start($processInfo)
    $process.WaitForExit()
    $exitCode = $process.ExitCode
    
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "================================" $Blue
    
    if ($exitCode -eq 0) {
        Write-ColoredOutput "✅ All Ruby tests passed!" $Green
    } else {
        Write-ColoredOutput "❌ Some Ruby tests failed. Exit code: $exitCode" $Red
        Write-ColoredOutput "" $Reset
        Write-ColoredOutput "To debug failed tests, you can run:" $Yellow
        Write-ColoredOutput "  run-plugin-tests.ps1 -Progress" $Yellow
        Write-ColoredOutput "  run-plugin-tests.ps1 -Verbose" $Yellow
        Write-ColoredOutput "  run-plugin-tests.ps1 -SpecFile 'spec/plugins/filename_spec.rb'" $Yellow
    }
    
    return $exitCode
}

function Test-RSpecInstalled {
    try {
        # Check if rspec is available via bundler
        bundle exec rspec --version 2>$null
        return $LASTEXITCODE -eq 0
    }
    catch {
        return $false
    }
}

function Invoke-PluginTestsRunner {
    # Main execution
    try {
        Write-ColoredOutput "Tech Hub Ruby Plugin Test Runner" $Blue
        Write-ColoredOutput "======================================" $Blue
        Write-ColoredOutput "" $Reset
        
        # Check and install dependencies if needed
        Write-ColoredOutput "🔍 Checking Ruby dependencies..." $Yellow
        
        # First check if bundler is available
        if (-not (Test-BundlerInstalled)) {
            Write-ColoredOutput "❌ Bundler not found." $Red
            exit 1
        }
        
        # Then check if gems are installed
        if (-not (Test-GemsInstalled)) {
            Write-ColoredOutput "❌ Ruby gems not found or outdated." $Red
            exit 1
        } else {
            Write-ColoredOutput "✅ Ruby dependencies already installed and up to date" $Green
        }
        
        # Verify RSpec is available
        Write-ColoredOutput "🔍 Verifying RSpec availability..." $Yellow
        if (-not (Test-RSpecInstalled)) {
            Write-ColoredOutput "❌ RSpec not found in bundle. Please check Gemfile." $Red
            exit 1
        } else {
            Write-ColoredOutput "✅ RSpec is available" $Green
        }
        
        # Build test arguments
        $testArgs = @()
        
        # Handle specific spec file
        if ($SpecFile) {
            if ($Line) {
                $testArgs += "$SpecFile`:$Line"
                Write-ColoredOutput "🎯 Running specific test: $SpecFile at line $Line" $Yellow
            } else {
                $testArgs += $SpecFile
                Write-ColoredOutput "🎯 Running specific file: $SpecFile" $Yellow
            }
        } else {
            # Default to spec directory
            $testArgs += "spec/"
            Write-ColoredOutput "🧪 Running all Ruby tests in spec/ directory" $Yellow
        }
        
        $format = "documentation"
        if($Progress) {
            $format = "progress"
            Write-ColoredOutput "⚡ Using progress format for live test execution updates" $Yellow
        } else {
            Write-ColoredOutput "📋 Using failures format (default) for detailed output" $Yellow
        }
        $testArgs += "--format"
        $testArgs += $format

        if($Verbose) {
            $testArgs += "--backtrace"
            Write-ColoredOutput "🔍 Using verbose format with backtraces" $Yellow
        } 
        
        # Add color output
        $testArgs += "--color"
        
        # Add fail-fast option to continue on failures but show detailed failure information
        $testArgs += "--no-fail-fast"
        $testArgs += "--failure-exit-code=1"
        
        Write-ColoredOutput "" $Reset
        
        # Run the tests
        $result = Invoke-RSpecTests $testArgs
        
        # Exit with the result code (success/failure messages already printed by Invoke-RSpecTests)
        exit $result
        
    } catch {
        Write-ColoredOutput "💥 An error occurred: $($_.Exception.Message)" $Red
        Write-ColoredOutput "Stack trace: $($_.Exception.StackTrace)" $Red
        exit 1
    }
}

$location = Get-Location
try {
    Invoke-PluginTestsRunner
}
finally {
    Set-Location $location
}