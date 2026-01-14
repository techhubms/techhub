<#
.SYNOPSIS
    Verifies documentation is synchronized with code.

.DESCRIPTION
    Checks that documentation files accurately reflect the current state of the codebase.
    Identifies outdated references, missing documentation, and inconsistencies.

.PARAMETER SourceRoot
    Root directory of the repository. Defaults to auto-detection.

.PARAMETER OutputFormat
    Format for output: 'Console', 'Markdown', or 'Json'. Defaults to 'Console'.

.PARAMETER IncludeDetails
    Include detailed information about each issue found.

.EXAMPLE
    ./verify-documentation.ps1
    
.EXAMPLE
    ./verify-documentation.ps1 -OutputFormat Markdown -IncludeDetails
#>

[CmdletBinding()]
param(
    [Parameter()]
    [string]$SourceRoot,
    
    [Parameter()]
    [ValidateSet('Console', 'Markdown', 'Json')]
    [string]$OutputFormat = 'Console',
    
    [Parameter()]
    [switch]$IncludeDetails
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

#region Helper Functions

function Get-SourceRoot {
    if ($SourceRoot) {
        return $SourceRoot
    }
    
    # Try to find the repository root
    $current = $PSScriptRoot
    while ($current -and -not (Test-Path (Join-Path $current "TechHub.slnx"))) {
        $current = Split-Path $current -Parent
    }
    
    if (-not $current) {
        throw "Could not find repository root. Please specify -SourceRoot parameter."
    }
    
    return $current
}

function Test-FileExists {
    param([string]$Path, [string]$Description)
    
    $exists = Test-Path $Path
    return [PSCustomObject]@{
        Check   = "File exists: $Description"
        Path    = $Path
        Status  = if ($exists) { "Pass" } else { "Fail" }
        Message = if ($exists) { "File found" } else { "File missing" }
    }
}

function Test-MarkdownLinks {
    param([string]$FilePath)
    
    $issues = @()
    $content = Get-Content $FilePath -Raw
    
    # Find markdown links [text](path)
    $linkPattern = '\[([^\]]+)\]\(([^)]+)\)'
    # Note: Calling [regex]::Matches() method is safe, not assigning to $Matches variable
    $foundLinks = [regex]::Matches($content, $linkPattern)
    
    foreach ($match in $foundLinks) {
        $linkText = $match.Groups[1].Value
        $linkPath = $match.Groups[2].Value
        
        # Skip external URLs and anchors
        if ($linkPath -match '^(https?://|#|mailto:)') {
            continue
        }
        
        # Resolve relative path
        $basePath = Split-Path $FilePath -Parent
        $fullPath = Join-Path $basePath $linkPath
        
        # Remove anchor from path
        $fullPath = $fullPath -replace '#.*$', ''
        
        if (-not (Test-Path $fullPath)) {
            $issues += [PSCustomObject]@{
                File  = $FilePath
                Link  = $linkPath
                Text  = $linkText
                Issue = "Broken link - target does not exist"
            }
        }
    }
    
    return $issues
}

function Test-ApiEndpointsDocumented {
    param([string]$SourceRoot)
    
    $issues = @()
    $apiSpec = Join-Path $SourceRoot "docs/api-specification.md"
    
    if (-not (Test-Path $apiSpec)) {
        $issues += [PSCustomObject]@{
            Check = "API Documentation"
            Issue = "API specification file not found"
            Path  = $apiSpec
        }
        return $issues
    }
    
    $specContent = Get-Content $apiSpec -Raw
    
    # Find all endpoint definitions in the API project
    $apiFiles = Get-ChildItem -Path (Join-Path $SourceRoot "src/TechHub.Api") -Filter "*.cs" -Recurse
    
    foreach ($file in $apiFiles) {
        $content = Get-Content $file.FullName -Raw
        
        # Look for MapGet, MapPost, etc.
        $endpointPattern = 'Map(Get|Post|Put|Delete|Patch)\s*\(\s*"([^"]+)"'
        $endpointMatches = [regex]::Matches($content, $endpointPattern)
        
        foreach ($match in $endpointMatches) {
            $method = $match.Groups[1].Value.ToUpper()
            $route = $match.Groups[2].Value
            
            # Check if this endpoint is documented
            if ($specContent -notmatch [regex]::Escape($route)) {
                $issues += [PSCustomObject]@{
                    Check  = "Endpoint Documentation"
                    Method = $method
                    Route  = $route
                    File   = $file.Name
                    Issue  = "Endpoint not found in API specification"
                }
            }
        }
    }
    
    return $issues
}

function Test-AgentsFilesExist {
    param([string]$SourceRoot)
    
    $results = @()
    
    # Expected AGENTS.md locations
    $expectedLocations = @(
        "AGENTS.md",
        "src/AGENTS.md",
        "src/TechHub.Api/AGENTS.md",
        "src/TechHub.Web/AGENTS.md",
        "src/TechHub.Core/AGENTS.md",
        "src/TechHub.Infrastructure/AGENTS.md",
        "tests/AGENTS.md",
        "scripts/AGENTS.md",
        "docs/AGENTS.md",
        "collections/AGENTS.md"
    )
    
    foreach ($location in $expectedLocations) {
        $fullPath = Join-Path $SourceRoot $location
        $results += Test-FileExists -Path $fullPath -Description $location
    }
    
    return $results
}

#endregion

#region Main Execution

$root = Get-SourceRoot
Write-Host "Verifying documentation in: $root" -ForegroundColor Cyan

$allResults = @{
    FileChecks  = @()
    BrokenLinks = @()
    ApiIssues   = @()
    Summary     = @{
        TotalChecks = 0
        Passed      = 0
        Failed      = 0
        Warnings    = 0
    }
}

# Check AGENTS.md files exist
Write-Host "`nChecking AGENTS.md files..." -ForegroundColor Yellow
$agentsResults = Test-AgentsFilesExist -SourceRoot $root
$allResults.FileChecks += $agentsResults

# Check essential documentation files
Write-Host "Checking essential documentation..." -ForegroundColor Yellow
$essentialDocs = @(
    @{ Path = "README.md"; Description = "Repository README" },
    @{ Path = "docs/api-specification.md"; Description = "API Specification" },
    @{ Path = "docs/filtering-system.md"; Description = "Filtering System" },
    @{ Path = "docs/content-management.md"; Description = "Content Management" },
    @{ Path = "collections/markdown-guidelines.md"; Description = "Markdown Guidelines" },
    @{ Path = "collections/writing-style-guidelines.md"; Description = "Writing Style Guidelines" }
)

foreach ($doc in $essentialDocs) {
    $fullPath = Join-Path $root $doc.Path
    $allResults.FileChecks += Test-FileExists -Path $fullPath -Description $doc.Description
}

# Check for broken links in key documentation
Write-Host "Checking for broken links..." -ForegroundColor Yellow
$docsToCheck = @(
    "README.md",
    "AGENTS.md",
    "docs/api-specification.md"
)

foreach ($doc in $docsToCheck) {
    $fullPath = Join-Path $root $doc
    if (Test-Path $fullPath) {
        $linkIssues = Test-MarkdownLinks -FilePath $fullPath
        $allResults.BrokenLinks += $linkIssues
    }
}

# Check API endpoints are documented
Write-Host "Checking API endpoint documentation..." -ForegroundColor Yellow
$apiIssues = Test-ApiEndpointsDocumented -SourceRoot $root
$allResults.ApiIssues += $apiIssues

# Calculate summary
$allResults.Summary.TotalChecks = $allResults.FileChecks.Count
$passedChecks = @($allResults.FileChecks | Where-Object { $_.Status -eq "Pass" })
$failedChecks = @($allResults.FileChecks | Where-Object { $_.Status -eq "Fail" })
$allResults.Summary.Passed = $passedChecks.Count
$allResults.Summary.Failed = $failedChecks.Count
$allResults.Summary.Warnings = $allResults.BrokenLinks.Count + $allResults.ApiIssues.Count

#endregion

#region Output

switch ($OutputFormat) {
    'Console' {
        Write-Host "`n===== Documentation Verification Results =====" -ForegroundColor Cyan
        
        Write-Host "`nFile Checks:" -ForegroundColor Yellow
        foreach ($check in $allResults.FileChecks) {
            $color = if ($check.Status -eq "Pass") { "Green" } else { "Red" }
            $symbol = if ($check.Status -eq "Pass") { "✅" } else { "❌" }
            Write-Host "  $symbol $($check.Check)" -ForegroundColor $color
        }
        
        if ($allResults.BrokenLinks.Count -gt 0) {
            Write-Host "`nBroken Links:" -ForegroundColor Yellow
            foreach ($link in $allResults.BrokenLinks) {
                Write-Host "  ❌ [$($link.Text)]($($link.Link)) in $($link.File)" -ForegroundColor Red
            }
        }
        
        if ($allResults.ApiIssues.Count -gt 0) {
            Write-Host "`nAPI Documentation Issues:" -ForegroundColor Yellow
            foreach ($issue in $allResults.ApiIssues) {
                Write-Host "  ⚠️  $($issue.Method) $($issue.Route) - $($issue.Issue)" -ForegroundColor Yellow
            }
        }
        
        Write-Host "`n===== Summary =====" -ForegroundColor Cyan
        Write-Host "  Total Checks: $($allResults.Summary.TotalChecks)"
        Write-Host "  Passed: $($allResults.Summary.Passed)" -ForegroundColor Green
        Write-Host "  Failed: $($allResults.Summary.Failed)" -ForegroundColor $(if ($allResults.Summary.Failed -gt 0) { "Red" } else { "Green" })
        Write-Host "  Warnings: $($allResults.Summary.Warnings)" -ForegroundColor $(if ($allResults.Summary.Warnings -gt 0) { "Yellow" } else { "Green" })
    }
    
    'Markdown' {
        $output = @"
## Documentation Verification Results

### File Checks

| Status | Check | Path |
|--------|-------|------|
"@
        foreach ($check in $allResults.FileChecks) {
            $symbol = if ($check.Status -eq "Pass") { "✅" } else { "❌" }
            $output += "`n| $symbol | $($check.Check) | ``$($check.Path)`` |"
        }
        
        if ($allResults.BrokenLinks.Count -gt 0) {
            $output += "`n`n### Broken Links`n"
            foreach ($link in $allResults.BrokenLinks) {
                $output += "`n- ❌ ``$($link.Link)`` in ``$($link.File)``"
            }
        }
        
        if ($allResults.ApiIssues.Count -gt 0) {
            $output += "`n`n### API Documentation Issues`n"
            foreach ($issue in $allResults.ApiIssues) {
                $output += "`n- ⚠️ ``$($issue.Method) $($issue.Route)`` - $($issue.Issue)"
            }
        }
        
        $output += @"

### Summary

- **Total Checks**: $($allResults.Summary.TotalChecks)
- **Passed**: $($allResults.Summary.Passed) ✅
- **Failed**: $($allResults.Summary.Failed) $(if ($allResults.Summary.Failed -gt 0) { "❌" } else { "✅" })
- **Warnings**: $($allResults.Summary.Warnings) $(if ($allResults.Summary.Warnings -gt 0) { "⚠️" } else { "✅" })
"@
        
        Write-Output $output
    }
    
    'Json' {
        $allResults | ConvertTo-Json -Depth 5
    }
}

# Exit with appropriate code
if ($allResults.Summary.Failed -gt 0) {
    exit 1
}
exit 0

#endregion
