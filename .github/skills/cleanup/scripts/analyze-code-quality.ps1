#!/usr/bin/env pwsh

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Get paths
$ScriptRoot = Split-Path -Parent (Split-Path -Parent (Split-Path -Parent (Split-Path -Parent $PSScriptRoot)))
$SolutionFile = Join-Path $ScriptRoot "TechHub.slnx"

Write-Host "Code Quality Analysis" -ForegroundColor Cyan
Write-Host "=====================`n" -ForegroundColor Cyan

# Build solution and capture output
Write-Host "Building solution to collect warnings and errors...`n" -ForegroundColor Yellow
$buildOutput = dotnet build $SolutionFile --no-incremental 2>&1

# Parse build results
$buildSuccess = $LASTEXITCODE -eq 0
$errors = @($buildOutput | Select-String -Pattern ": error ")
$warnings = @($buildOutput | Select-String -Pattern ": warning ")

Write-Host "Build Status" -ForegroundColor Cyan
Write-Host "------------" -ForegroundColor Cyan
Write-Host "Result: $(if ($buildSuccess) { 'Success' } else { 'Failed' })"
Write-Host "Errors: $($errors.Count)"
Write-Host "Warnings: $($warnings.Count)"

# Categorize issues
$issues = @{
    High   = @()
    Medium = @()
    Low    = @()
}

# Parse warnings and categorize
foreach ($warning in $warnings) {
    $line = $warning.Line
    
    # Extract rule ID (e.g., CA1062, RZ2012, IDE0005)
    if ($line -match "warning ((?:CA|IDE|RZ|CS)\d+)") {
        $ruleId = $Matches[1]
        
        # Extract file path and line number
        $filePath = ""
        $lineNumber = ""
        if ($line -match "([^(]+)\((\d+),\d+\): warning") {
            $filePath = $Matches[1]
            $lineNumber = $Matches[2]
        }
        
        # Extract message
        $message = $line -replace "^.*: warning [^:]+:\s*", ""
        
        # Categorize by priority
        $priority = switch -Regex ($ruleId) {
            '^RZ' { 'High' }   # Blazor/Razor issues
            '^CA1001$|^CA1062$|^CA1304$|^CA1305$|^CA1307$' { 'High' }  # Critical code analysis
            '^CA' { 'Medium' } # Other code analysis
            '^IDE' { 'Low' }    # IDE suggestions
            '^CS15' { 'Low' }    # Documentation warnings
            default { 'Medium' }
        }
        
        $issues[$priority] += @{
            RuleId  = $ruleId
            File    = $filePath
            Line    = $lineNumber
            Message = $message
        }
    }
}

# Group by rule ID
$groupedIssues = @{
    High   = $issues.High | Group-Object -Property RuleId
    Medium = $issues.Medium | Group-Object -Property RuleId
    Low    = $issues.Low | Group-Object -Property RuleId
}

# Get git info
$branch = git rev-parse --abbrev-ref HEAD 2>$null
$commitSha = git rev-parse --short HEAD 2>$null
$date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

Write-Host "`nGit Information" -ForegroundColor Cyan
Write-Host "---------------" -ForegroundColor Cyan
Write-Host "Branch: $($branch ?? 'unknown')"
Write-Host "Commit: $($commitSha ?? 'unknown')"
Write-Host "Date: $date"

# Output categorized issues
Write-Host "`n🔴 High Priority Issues" -ForegroundColor Red
Write-Host "----------------------" -ForegroundColor Red
Write-Host "Count: $($issues.High.Count)"
if ($groupedIssues.High) {
    foreach ($group in $groupedIssues.High) {
        Write-Host "`n  $($group.Name): $($group.Count) occurrence(s)" -ForegroundColor Yellow
        Write-Host "  Message: $($group.Group[0].Message -replace ' \(https://.*\)', '')"
        Write-Host "  Files:"
        foreach ($issue in $group.Group | Select-Object -First 5) {
            $relativePath = $issue.File -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
            Write-Host "    - $relativePath (Line $($issue.Line))"
        }
        if ($group.Count -gt 5) {
            Write-Host "    ...and $($group.Count - 5) more"
        }
    }
}

Write-Host "`n🟡 Medium Priority Issues" -ForegroundColor Yellow
Write-Host "------------------------" -ForegroundColor Yellow
Write-Host "Count: $($issues.Medium.Count)"
if ($groupedIssues.Medium) {
    foreach ($group in $groupedIssues.Medium) {
        Write-Host "`n  $($group.Name): $($group.Count) occurrence(s)" -ForegroundColor Yellow
        Write-Host "  Message: $($group.Group[0].Message -replace ' \(https://.*\)', '')"
        Write-Host "  Files:"
        foreach ($issue in $group.Group | Select-Object -First 5) {
            $relativePath = $issue.File -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
            Write-Host "    - $relativePath (Line $($issue.Line))"
        }
        if ($group.Count -gt 5) {
            Write-Host "    ...and $($group.Count - 5) more"
        }
    }
}

Write-Host "`n🟢 Low Priority Issues" -ForegroundColor Green
Write-Host "---------------------" -ForegroundColor Green
Write-Host "Count: $($issues.Low.Count)"
if ($groupedIssues.Low) {
    foreach ($group in $groupedIssues.Low) {
        Write-Host "`n  $($group.Name): $($group.Count) occurrence(s)" -ForegroundColor Yellow
        Write-Host "  Message: $($group.Group[0].Message -replace ' \(https://.*\)', '')"
        Write-Host "  Files:"
        foreach ($issue in $group.Group | Select-Object -First 5) {
            $relativePath = $issue.File -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
            Write-Host "    - $relativePath (Line $($issue.Line))"
        }
        if ($group.Count -gt 5) {
            Write-Host "    ...and $($group.Count - 5) more"
        }
    }
}

# Additional code quality checks
Write-Host "`n📋 Additional Quality Checks" -ForegroundColor Cyan
Write-Host "---------------------------" -ForegroundColor Cyan

$additionalIssues = @{
    TechnicalDebt = @()
    DebugCode     = @()
    DisabledTests = @()
    LargeFiles    = @()
}

# Check for TODO/FIXME/HACK comments
Write-Host "`nScanning for technical debt markers (TODO, FIXME, HACK)..." -ForegroundColor Yellow
$csFiles = @(Get-ChildItem -Path (Join-Path $ScriptRoot "src") -Filter "*.cs" -Recurse)
foreach ($file in $csFiles) {
    $lineNumber = 0
    foreach ($line in Get-Content $file.FullName) {
        $lineNumber++
        if ($line -match '//\s*(TODO|FIXME|HACK)\s*:?\s*(.*)') {
            $marker = $Matches[1]
            $message = $Matches[2].Trim()
            $additionalIssues.TechnicalDebt += @{
                File    = $file.FullName -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
                Line    = $lineNumber
                Marker  = $marker
                Message = if ($message) { $message } else { "(no description)" }
            }
        }
    }
}
Write-Host "  Found $($additionalIssues.TechnicalDebt.Count) technical debt markers"

# Check for Console.WriteLine in production code
Write-Host "`nScanning for Console.WriteLine in production code..." -ForegroundColor Yellow
foreach ($file in $csFiles) {
    # Skip test files
    if ($file.FullName -match '[\\/]tests[\\/]' -or $file.FullName -match 'Tests\.cs$') {
        continue
    }
    
    $lineNumber = 0
    foreach ($line in Get-Content $file.FullName) {
        $lineNumber++
        if ($line -match 'Console\.WriteLine' -and $line -notmatch '^\s*//' -and $line -notmatch '^\s*\*') {
            $additionalIssues.DebugCode += @{
                File = $file.FullName -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
                Line = $lineNumber
                Code = $line.Trim()
            }
        }
    }
}
Write-Host "  Found $($additionalIssues.DebugCode.Count) Console.WriteLine statements"

# Check for disabled tests
Write-Host "`nScanning for disabled tests..." -ForegroundColor Yellow
$testFiles = @(Get-ChildItem -Path (Join-Path $ScriptRoot "tests") -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue)
foreach ($file in $testFiles) {
    $lineNumber = 0
    foreach ($line in Get-Content $file.FullName) {
        $lineNumber++
        if ($line -match '\[(?:Fact|Theory)\(Skip\s*=\s*"([^"]+)"\)') {
            $reason = $Matches[1]
            $additionalIssues.DisabledTests += @{
                File   = $file.FullName -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
                Line   = $lineNumber
                Reason = $reason
            }
        }
    }
}
Write-Host "  Found $($additionalIssues.DisabledTests.Count) disabled tests"

# Check for large files (>500 lines)
Write-Host "`nScanning for large files (>500 lines)..." -ForegroundColor Yellow
foreach ($file in $csFiles) {
    $lineCount = (Get-Content $file.FullName).Count
    if ($lineCount -gt 500) {
        $additionalIssues.LargeFiles += @{
            File       = $file.FullName -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
            Lines      = $lineCount
            Complexity = if ($lineCount -gt 1000) { "Very High" } elseif ($lineCount -gt 750) { "High" } else { "Medium" }
        }
    }
}
Write-Host "  Found $($additionalIssues.LargeFiles.Count) large files"

# Output additional issues
if ($additionalIssues.TechnicalDebt.Count -gt 0) {
    Write-Host "`n📝 Technical Debt Markers:" -ForegroundColor Yellow
    $additionalIssues.TechnicalDebt | Group-Object Marker | ForEach-Object {
        Write-Host "  $($_.Name): $($_.Count) occurrence(s)" -ForegroundColor Yellow
        $_.Group | Select-Object -First 3 | ForEach-Object {
            Write-Host "    - $($_.File):$($_.Line) - $($_.Message)"
        }
        if ($_.Count -gt 3) {
            Write-Host "    ...and $($_.Count - 3) more"
        }
    }
}

if ($additionalIssues.DebugCode.Count -gt 0) {
    Write-Host "`n🐛 Debug Code in Production:" -ForegroundColor Yellow
    $additionalIssues.DebugCode | Select-Object -First 5 | ForEach-Object {
        Write-Host "  - $($_.File):$($_.Line)" -ForegroundColor Yellow
    }
    if ($additionalIssues.DebugCode.Count -gt 5) {
        Write-Host "  ...and $($additionalIssues.DebugCode.Count - 5) more"
    }
}

if ($additionalIssues.DisabledTests.Count -gt 0) {
    Write-Host "`n⏭️  Disabled Tests:" -ForegroundColor Yellow
    $additionalIssues.DisabledTests | ForEach-Object {
        Write-Host "  - $($_.File):$($_.Line) - $($_.Reason)" -ForegroundColor Yellow
    }
}

if ($additionalIssues.LargeFiles.Count -gt 0) {
    Write-Host "`n📏 Large Files:" -ForegroundColor Yellow
    $additionalIssues.LargeFiles | Sort-Object Lines -Descending | Select-Object -First 10 | ForEach-Object {
        Write-Host "  - $($_.File): $($_.Lines) lines ($($_.Complexity) complexity)" -ForegroundColor Yellow
    }
    if ($additionalIssues.LargeFiles.Count -gt 10) {
        Write-Host "  ...and $($additionalIssues.LargeFiles.Count - 10) more"
    }
}



# Output structured data as JSON for easier parsing if needed
$structuredOutput = @{
    Date         = $date
    Branch       = $branch ?? 'unknown'
    Commit       = $commitSha ?? 'unknown'
    BuildSuccess = $buildSuccess
    ErrorCount   = $errors.Count
    WarningCount = $warnings.Count
    Issues       = @{
        High   = $groupedIssues.High | ForEach-Object {
            @{
                RuleId  = $_.Name
                Count   = $_.Count
                Message = $_.Group[0].Message -replace ' \(https://.*\)', ''
                Files   = $_.Group | ForEach-Object {
                    @{
                        File = $_.File -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
                        Line = $_.Line
                    }
                }
            }
        }
        Medium = $groupedIssues.Medium | ForEach-Object {
            @{
                RuleId  = $_.Name
                Count   = $_.Count
                Message = $_.Group[0].Message -replace ' \(https://.*\)', ''
                Files   = $_.Group | ForEach-Object {
                    @{
                        File = $_.File -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
                        Line = $_.Line
                    }
                }
            }
        }
        Low    = $groupedIssues.Low | ForEach-Object {
            @{
                RuleId  = $_.Name
                Count   = $_.Count
                Message = $_.Group[0].Message -replace ' \(https://.*\)', ''
                Files   = $_.Group | ForEach-Object {
                    @{
                        File = $_.File -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
                        Line = $_.Line
                    }
                }
            }
        }
    }
}

# Add additional issues to structured output
$structuredOutput.AdditionalIssues = @{
    TechnicalDebt = $additionalIssues.TechnicalDebt
    DebugCode     = $additionalIssues.DebugCode
    DisabledTests = $additionalIssues.DisabledTests
    LargeFiles    = $additionalIssues.LargeFiles
}

# Save JSON output for programmatic access
$jsonOutput = $structuredOutput | ConvertTo-Json -Depth 10
$jsonOutput | Out-File -FilePath (Join-Path $ScriptRoot ".tmp/code-quality-data.json") -Encoding utf8

exit 0
