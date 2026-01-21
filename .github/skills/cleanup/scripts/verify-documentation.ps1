<#
.SYNOPSIS
    Lists all documentation files for AI agent review.

.DESCRIPTION
    Outputs all documentation files that should be reviewed to ensure they accurately
    reflect the current state of the codebase. The AI agent should read and review
    each file for accuracy, completeness, and consistency with the code.

.PARAMETER SourceRoot
    Root directory of the repository. Defaults to auto-detection.

.EXAMPLE
    ./verify-documentation.ps1
    
.EXAMPLE
    ./verify-documentation.ps1 -SourceRoot /workspaces/techhub
#>

[CmdletBinding()]
param(
    [Parameter()]
    [string]$SourceRoot
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

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

$root = Get-SourceRoot

Write-Host @"

================================================================================
DOCUMENTATION REVIEW REQUIRED
================================================================================

The following documentation files should be reviewed for accuracy and completeness.
Please read each file and verify it reflects the current state of the codebase.

INSTRUCTIONS FOR AI AGENT:
--------------------------
1. Read each file listed below
2. Compare documentation content with actual implementation
3. Identify any outdated information, broken links, or inconsistencies
4. Report discrepancies and suggest corrections
5. Ensure all new features are documented
6. Verify all code examples are accurate

REPOSITORY ROOT DOCUMENTATION:
"@ -ForegroundColor Cyan

$rootDocs = @(
    "README.md",
    "AGENTS.md"
)

foreach ($doc in $rootDocs) {
    $path = Join-Path $root $doc
    if (Test-Path $path) {
        Write-Host "  ✓ $doc" -ForegroundColor Green
    }
    else {
        Write-Host "  ✗ $doc (MISSING)" -ForegroundColor Red
    }
}

Write-Host "`nFUNCTIONAL DOCUMENTATION (docs/):" -ForegroundColor Cyan

$functionalDocs = @(
    "docs/AGENTS.md",
    "docs/api-specification.md",
    "docs/content-management.md",
    "docs/rss-feeds.md",
    "docs/toc-component.md"
)

foreach ($doc in $functionalDocs) {
    $path = Join-Path $root $doc
    if (Test-Path $path) {
        Write-Host "  ✓ $doc" -ForegroundColor Green
    }
    else {
        Write-Host "  ✗ $doc (MISSING)" -ForegroundColor Red
    }
}

Write-Host "`nDOMAIN-SPECIFIC AGENTS.md FILES:" -ForegroundColor Cyan

$domainDocs = @(
    "src/AGENTS.md",
    "src/TechHub.Api/AGENTS.md",
    "src/TechHub.Web/AGENTS.md",
    "src/TechHub.Core/AGENTS.md",
    "src/TechHub.Infrastructure/AGENTS.md",
    "collections/AGENTS.md",
    "scripts/AGENTS.md",
    "tests/AGENTS.md",
    "tests/TechHub.Api.Tests/AGENTS.md",
    "tests/TechHub.Core.Tests/AGENTS.md",
    "tests/TechHub.Web.Tests/AGENTS.md",
    "tests/TechHub.Infrastructure.Tests/AGENTS.md",
    "tests/TechHub.E2E.Tests/AGENTS.md",
    "tests/powershell/AGENTS.md"
)

foreach ($doc in $domainDocs) {
    $path = Join-Path $root $doc
    if (Test-Path $path) {
        Write-Host "  ✓ $doc" -ForegroundColor Green
    }
    else {
        Write-Host "  ✗ $doc (MISSING)" -ForegroundColor Red
    }
}

Write-Host "`nCONTENT GUIDELINES:" -ForegroundColor Cyan

$contentDocs = @(
    "collections/writing-style-guidelines.md"
)

foreach ($doc in $contentDocs) {
    $path = Join-Path $root $doc
    if (Test-Path $path) {
        Write-Host "  ✓ $doc" -ForegroundColor Green
    }
    else {
        Write-Host "  ✗ $doc (MISSING)" -ForegroundColor Red
    }
}

Write-Host @"

================================================================================
REVIEW CHECKLIST FOR EACH FILE:
================================================================================

For each documentation file above, verify:

1. ACCURACY: Does content match current code implementation?
   - Check code examples are correct
   - Verify API endpoints match actual routes
   - Confirm patterns match current practices

2. COMPLETENESS: Are all features documented?
   - New endpoints have documentation
   - New components are described
   - Configuration changes are explained

3. CONSISTENCY: Does documentation align across files?
   - Terminology is consistent
   - Cross-references are valid
   - No conflicting information

4. LINKS: Are all internal links working?
   - Markdown links point to existing files
   - Anchors are valid
   - No orphaned references

5. EXAMPLES: Are code examples up-to-date?
   - Examples compile and run
   - Examples follow current patterns
   - Examples use latest APIs

================================================================================
AFTER REVIEW:
================================================================================

Report any issues found using this format:

FILE: [filename]
ISSUE: [description of problem]
SUGGESTED FIX: [proposed correction]

Then update the documentation files as needed to ensure accuracy.

================================================================================

"@ -ForegroundColor Yellow

exit 0
