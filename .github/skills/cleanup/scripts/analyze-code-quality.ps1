#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Analyzes code quality and generates a comprehensive overview.

.DESCRIPTION
    Builds the solution, analyzes all warnings and errors, categorizes them by
    priority, and generates a detailed overview using the standard template.

.PARAMETER OutputPath
    Path where the overview markdown file should be saved.
    Defaults to .tmp/code-quality-overview.md

.EXAMPLE
    .\analyze-code-quality.ps1
    Generates overview in default location (.tmp/code-quality-overview.md)

.EXAMPLE
    .\analyze-code-quality.ps1 -OutputPath "docs/quality-report.md"
    Generates overview in custom location
#>

param(
    [string]$OutputPath = ".tmp/code-quality-overview.md"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Get paths
$ScriptRoot = Split-Path -Parent (Split-Path -Parent (Split-Path -Parent (Split-Path -Parent $PSScriptRoot)))
$SolutionFile = Join-Path $ScriptRoot "TechHub.slnx"
$TemplateFile = Join-Path (Split-Path -Parent $PSScriptRoot) "templates/overview-template.md"
$OutputFile = Join-Path $ScriptRoot $OutputPath

Write-Host "Code Quality Analysis" -ForegroundColor Cyan
Write-Host "=====================`n" -ForegroundColor Cyan

# Ensure output directory exists
$OutputDir = Split-Path -Parent $OutputFile
if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

# Build solution and capture output
Write-Host "Building solution to collect warnings and errors..." -ForegroundColor Yellow
$buildOutput = dotnet build $SolutionFile --no-incremental 2>&1

# Parse build results
$buildSuccess = $LASTEXITCODE -eq 0
$errors = @($buildOutput | Select-String -Pattern ": error ")
$warnings = @($buildOutput | Select-String -Pattern ": warning ")

Write-Host "`nBuild Result: $(if ($buildSuccess) { '✅ Success' } else { '❌ Failed' })" -ForegroundColor $(if ($buildSuccess) { 'Green' } else { 'Red' })
Write-Host "Errors: $($errors.Count)" -ForegroundColor $(if ($errors.Count -gt 0) { 'Red' } else { 'Green' })
Write-Host "Warnings: $($warnings.Count)" -ForegroundColor $(if ($warnings.Count -gt 0) { 'Yellow' } else { 'Green' })

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

# Read template
if (-not (Test-Path $TemplateFile)) {
    Write-Host "`n❌ Template file not found: $TemplateFile" -ForegroundColor Red
    exit 1
}

$template = Get-Content $TemplateFile -Raw

# Get git info
$branch = git rev-parse --abbrev-ref HEAD 2>$null
$commitSha = git rev-parse --short HEAD 2>$null
$date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

# Build detailed sections for each priority
function Build-IssueSection {
    param($GroupedIssues, $Priority)
    
    if (-not $GroupedIssues -or $GroupedIssues.Count -eq 0) {
        return "_No $Priority priority issues found._`n"
    }
    
    $section = ""
    foreach ($group in $GroupedIssues) {
        $ruleId = $group.Name
        $count = $group.Count
        $firstIssue = $group.Group[0]
        
        # Clean message
        $message = $firstIssue.Message -replace ' \(https://.*\)', ''
        
        # Determine recommendation
        $recommendation = switch ($Priority) {
            'High' { '**Fix immediately** - Critical issue' }
            'Medium' { '**Fix or suppress** - Review and decide' }
            'Low' { '**Suppress recommended** - Style/formatting' }
        }
        
        $section += @"

### $ruleId

**Count**: $count occurrence(s)  
**Recommendation**: $recommendation

**Description**: $message

**Affected Files**:

"@
        
        foreach ($issue in $group.Group | Select-Object -First 10) {
            $relativePath = $issue.File -replace [regex]::Escape($ScriptRoot), '' -replace '^[\\/]', ''
            $section += "- [$relativePath]($relativePath#L$($issue.Line))`n"
        }
        
        if ($count -gt 10) {
            $section += "`n_...and $($count - 10) more occurrence(s)_`n"
        }
        
        $section += "`n---`n"
    }
    
    return $section
}

$highPriorityDetails = Build-IssueSection -GroupedIssues $groupedIssues.High -Priority 'High'
$mediumPriorityDetails = Build-IssueSection -GroupedIssues $groupedIssues.Medium -Priority 'Medium'
$lowPriorityDetails = Build-IssueSection -GroupedIssues $groupedIssues.Low -Priority 'Low'

# Build suppression examples
$suppressionExamples = ""
if ($groupedIssues.Low -and $groupedIssues.Low.Count -gt 0) {
    foreach ($group in $groupedIssues.Low | Select-Object -First 5) {
        $ruleId = $group.Name
        $firstIssue = $group.Group[0]
        $message = $firstIssue.Message -replace ' \(https://.*\)', '' | Select-Object -First 1
        $suppressionExamples += "dotnet_diagnostic.$ruleId.severity = none  # $($message -split '\.' | Select-Object -First 1)`n"
    }
}
else {
    $suppressionExamples = "# No suppressions needed"
}

# Calculate totals
$totalIssues = $issues.High.Count + $issues.Medium.Count + $issues.Low.Count

# Replace placeholders in template
$output = $template
$output = $output -replace '\{DATE\}', $date
$output = $output -replace '\{BRANCH\}', ($branch ?? 'unknown')
$output = $output -replace '\{COMMIT_SHA\}', ($commitSha ?? 'unknown')
$output = $output -replace '\{BUILD_RESULT\}', $(if ($buildSuccess) { '✅ Success' } else { '❌ Failed' })
$output = $output -replace '\{ERROR_COUNT\}', $errors.Count
$output = $output -replace '\{WARNING_COUNT\}', $warnings.Count
$output = $output -replace '\{HIGH_PRIORITY_COUNT\}', $issues.High.Count
$output = $output -replace '\{MEDIUM_PRIORITY_COUNT\}', $issues.Medium.Count
$output = $output -replace '\{LOW_PRIORITY_COUNT\}', $issues.Low.Count
$output = $output -replace '\{TOTAL_ISSUE_COUNT\}', $totalIssues
$output = $output -replace '\{HIGH_PRIORITY_DETAILS\}', $highPriorityDetails
$output = $output -replace '\{MEDIUM_PRIORITY_DETAILS\}', $mediumPriorityDetails
$output = $output -replace '\{LOW_PRIORITY_DETAILS\}', $lowPriorityDetails
$output = $output -replace '\{SUPPRESSION_EXAMPLES\}', $suppressionExamples

# Save output
$output | Out-File -FilePath $OutputFile -Encoding utf8

Write-Host "`n✅ Code quality overview generated: $OutputFile" -ForegroundColor Green
Write-Host "`nSummary:" -ForegroundColor Cyan
Write-Host "  High Priority: $($issues.High.Count) issues" -ForegroundColor $(if ($issues.High.Count -gt 0) { 'Red' } else { 'Green' })
Write-Host "  Medium Priority: $($issues.Medium.Count) issues" -ForegroundColor $(if ($issues.Medium.Count -gt 0) { 'Yellow' } else { 'Green' })
Write-Host "  Low Priority: $($issues.Low.Count) issues" -ForegroundColor $(if ($issues.Low.Count -gt 0) { 'Yellow' } else { 'Green' })
