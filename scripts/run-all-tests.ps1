#!/usr/bin/env pwsh

# All Tests Runner Script for Tech Hub
# This script runs all test suites in logical order for comprehensive validation

param(
    [switch]$SkipPowerShell,
    [switch]$SkipJavaScript,
    [switch]$SkipRuby,
    [switch]$SkipE2E,
    [switch]$StopOnFailure,
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
$Magenta = "`e[35m"
$Cyan = "`e[36m"
$Reset = "`e[0m"

function Write-ColoredOutput {
    param($Message, $Color)
    Write-Host "$Color$Message$Reset"
}

function Write-SectionHeader {
    param($Title, $Phase)
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "═══════════════════════════════════════════════════════════════" $Cyan
    Write-ColoredOutput "  $Title" $Cyan
    Write-ColoredOutput "  Phase $Phase of the Tech Hub Filtering System" $Cyan
    Write-ColoredOutput "═══════════════════════════════════════════════════════════════" $Cyan
    Write-ColoredOutput "" $Reset
}

function Invoke-TestSuite {
    param(
        [string]$SuiteName,
        [string]$ScriptPath,
        [string[]]$Arguments = @(),
        [string]$Phase,
        [string]$Description
    )
    
    Write-SectionHeader $SuiteName $Phase
    Write-ColoredOutput "🎯 $Description" $Yellow
    Write-ColoredOutput "📍 Executing: $ScriptPath $($Arguments -join ' ')" $Blue
    Write-ColoredOutput "" $Reset
    
    $startTime = Get-Date
    
    try {
        # Build the full command with arguments
        $allArgs = @($ScriptPath) + $Arguments
        
        # Execute the test script
        Invoke-Expression -Command @allArgs
        $exitCode = $LASTEXITCODE
        
        $endTime = Get-Date
        $duration = $endTime - $startTime
        $durationText = "{0:N1} seconds" -f $duration.TotalSeconds
        
        if ($exitCode -eq 0) {
            Write-ColoredOutput "" $Reset
            Write-ColoredOutput "✅ $SuiteName completed successfully in $durationText" $Green
            return $true
        } else {
            Write-ColoredOutput "" $Reset
            Write-ColoredOutput "❌ $SuiteName failed with exit code $exitCode after $durationText" $Red
            return $false
        }
    }
    catch {
        $endTime = Get-Date
        $duration = $endTime - $startTime
        $durationText = "{0:N1} seconds" -f $duration.TotalSeconds
        
        Write-ColoredOutput "" $Reset
        Write-ColoredOutput "💥 $SuiteName encountered an error after $durationText" $Red
        Write-ColoredOutput "Error: $($_.Exception.Message)" $Red
        return $false
    }
}

# Main execution
try {
    Write-ColoredOutput "Tech Hub Comprehensive Test Runner" $Magenta
    Write-ColoredOutput "=====================================" $Magenta
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "🧪 Running all test suites in logical order based on the testing pyramid" $Yellow
    Write-ColoredOutput "⚡ Fast unit tests first, slower integration tests last" $Yellow
    Write-ColoredOutput "" $Reset
    
    $overallStartTime = Get-Date
    $testResults = @()
    $failedSuites = @()
    
    # Phase 2: PowerShell Unit Tests (Fast preprocessing validation)
    if (-not $SkipPowerShell) {
        $args = @()
        if ($Verbose) { $args += "-Verbose" }
        
        $result = Invoke-TestSuite -SuiteName "PowerShell Unit Tests" -ScriptPath "./run-powershell-tests.ps1" -Arguments $args -Phase "2" -Description "Testing PowerShell preprocessing scripts (tag normalization, content processing)"
        $testResults += @{ Suite = "PowerShell"; Success = $result }
        
        if (-not $result) {
            $failedSuites += "PowerShell Unit Tests"
            if ($StopOnFailure) {
                Write-ColoredOutput "🛑 Stopping execution due to PowerShell test failure (StopOnFailure enabled)" $Red
                exit 1
            }
        }
    } else {
        Write-ColoredOutput "⏭️  Skipping PowerShell Unit Tests (Phase 2)" $Yellow
    }
    
    # Phase 4: JavaScript Unit Tests (Fast client-side logic validation)
    if (-not $SkipJavaScript) {
        $args = @()
        if ($Verbose) { $args += "-Verbose" }
        
        $result = Invoke-TestSuite -SuiteName "JavaScript Unit Tests" -ScriptPath "./run-javascript-tests.ps1" -Arguments $args -Phase "4" -Description "Testing JavaScript filtering logic (client-side tag/date filtering, state management)"
        $testResults += @{ Suite = "JavaScript"; Success = $result }
        
        if (-not $result) {
            $failedSuites += "JavaScript Unit Tests"
            if ($StopOnFailure) {
                Write-ColoredOutput "🛑 Stopping execution due to JavaScript test failure (StopOnFailure enabled)" $Red
                exit 1
            }
        }
    } else {
        Write-ColoredOutput "⏭️  Skipping JavaScript Unit Tests (Phase 4)" $Yellow
    }
    
    # Phase 3: Ruby Plugin Tests (Medium - Jekyll integration validation)
    if (-not $SkipRuby) {
        $args = @()
        if ($Verbose) { $args += "-Verbose" }
        
        $result = Invoke-TestSuite -SuiteName "Ruby Plugin Tests" -ScriptPath "./run-plugin-tests.ps1" -Arguments $args -Phase "3" -Description "Testing Jekyll plugins and Ruby components (server-side data generation, Liquid filters)"
        $testResults += @{ Suite = "Ruby"; Success = $result }
        
        if (-not $result) {
            $failedSuites += "Ruby Plugin Tests"
            if ($StopOnFailure) {
                Write-ColoredOutput "🛑 Stopping execution due to Ruby test failure (StopOnFailure enabled)" $Red
                exit 1
            }
        }
    } else {
        Write-ColoredOutput "⏭️  Skipping Ruby Plugin Tests (Phase 3)" $Yellow
    }
    
    # Phase 5: End-to-End Tests (Slow - Full browser integration validation)
    if (-not $SkipE2E) {
        $args = @()
        if ($Verbose) { $args += "-Verbose" }
        
        $result = Invoke-TestSuite -SuiteName "End-to-End Browser Tests" -ScriptPath "./run-e2e-tests.ps1" -Arguments $args -Phase "5" -Description "Testing complete user workflows and browser interactions (full system integration)"
        $testResults += @{ Suite = "E2E"; Success = $result }
        
        if (-not $result) {
            $failedSuites += "End-to-End Browser Tests"
            if ($StopOnFailure) {
                Write-ColoredOutput "🛑 Stopping execution due to E2E test failure (StopOnFailure enabled)" $Red
                exit 1
            }
        }
    } else {
        Write-ColoredOutput "⏭️  Skipping End-to-End Browser Tests (Phase 5)" $Yellow
    }
    
    # Summary Report
    $overallEndTime = Get-Date
    $totalDuration = $overallEndTime - $overallStartTime
    $totalDurationText = "{0:N1} seconds" -f $totalDuration.TotalSeconds
    
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "═══════════════════════════════════════════════════════════════" $Magenta
    Write-ColoredOutput "  TEST EXECUTION SUMMARY" $Magenta
    Write-ColoredOutput "═══════════════════════════════════════════════════════════════" $Magenta
    Write-ColoredOutput "" $Reset
    
    $successCount = ($testResults | Where-Object { $_.Success }).Count
    $totalCount = $testResults.Count
    
    Write-ColoredOutput "📊 Overall Results:" $Blue
    Write-ColoredOutput "   Total Test Suites: $totalCount" $Blue
    Write-ColoredOutput "   Successful: $successCount" $Green
    Write-ColoredOutput "   Failed: $($totalCount - $successCount)" $Red
    Write-ColoredOutput "   Total Duration: $totalDurationText" $Blue
    Write-ColoredOutput "" $Reset
    
    # Detailed Results
    Write-ColoredOutput "📋 Detailed Results:" $Blue
    foreach ($result in $testResults) {
        $status = if ($result.Success) { "✅ PASSED" } else { "❌ FAILED" }
        $color = if ($result.Success) { $Green } else { $Red }
        Write-ColoredOutput "   $($result.Suite): $status" $color
    }
    
    Write-ColoredOutput "" $Reset
    
    # Final Status
    if ($failedSuites.Count -eq 0) {
        Write-ColoredOutput "🎉 ALL TEST SUITES PASSED! The Tech Hub filtering system is validated." $Green
        Write-ColoredOutput "✨ All 5 phases of the filtering system are working correctly." $Green
        exit 0
    } else {
        Write-ColoredOutput "💥 SOME TEST SUITES FAILED!" $Red
        Write-ColoredOutput "❌ Failed suites: $($failedSuites -join ', ')" $Red
        Write-ColoredOutput "" $Reset
        Write-ColoredOutput "🔍 To debug specific failures, run individual test scripts:" $Yellow
        if ($failedSuites -contains "PowerShell Unit Tests") {
            Write-ColoredOutput "   pwsh run-powershell-tests.ps1 -Verbose" $Yellow
        }
        if ($failedSuites -contains "JavaScript Unit Tests") {
            Write-ColoredOutput "   pwsh run-javascript-tests.ps1 -Verbose" $Yellow
        }
        if ($failedSuites -contains "Ruby Plugin Tests") {
            Write-ColoredOutput "   pwsh run-plugin-tests.ps1 -Verbose" $Yellow
        }
        if ($failedSuites -contains "End-to-End Browser Tests") {
            Write-ColoredOutput "   pwsh run-e2e-tests.ps1 -Verbose" $Yellow
        }
        exit 1
    }
    
} catch {
    Write-ColoredOutput "💥 An error occurred during test execution: $($_.Exception.Message)" $Red
    Write-ColoredOutput "Stack trace: $($_.Exception.StackTrace)" $Red
    exit 1
}