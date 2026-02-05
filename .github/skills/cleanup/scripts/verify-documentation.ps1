[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Find the repository root
$root = $PSScriptRoot
while ($root -and -not (Test-Path (Join-Path $root "TechHub.slnx"))) {
    $root = Split-Path $root -Parent
}

if (-not $root) {
    throw "Could not find repository root (TechHub.slnx)"
}

Write-Host "Documentation Review Files" -ForegroundColor Cyan
Write-Host "=========================`n" -ForegroundColor Cyan

$allDocs = @()

# Repository root documentation
$rootDocs = @("README.md", "AGENTS.md")
foreach ($doc in $rootDocs) {
    $path = Join-Path $root $doc
    if (Test-Path $path) {
        $allDocs += $path
        Write-Host "✓ $doc" -ForegroundColor Green
    }
    else {
        Write-Host "✗ $doc (MISSING)" -ForegroundColor Red
    }
}

Write-Host "`nFunctional Documentation:" -ForegroundColor Cyan
$functionalDocs = @(
    "docs/AGENTS.md",
    "docs/content-api.md",
    "docs/content-processing.md",
    "docs/custom-pages.md",
    "docs/rss-feeds.md",
    "docs/toc-component.md",
    "docs/filtering.md",
    "docs/database.md",
    "docs/health-checks.md",
    "docs/architecture.md"
)
foreach ($doc in $functionalDocs) {
    $path = Join-Path $root $doc
    if (Test-Path $path) {
        $allDocs += $path
        Write-Host "✓ $doc" -ForegroundColor Green
    }
    else {
        Write-Host "✗ $doc (MISSING)" -ForegroundColor Red
    }
}

Write-Host "`nDomain-Specific AGENTS.md:" -ForegroundColor Cyan
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
        $allDocs += $path
        Write-Host "✓ $doc" -ForegroundColor Green
    }
    else {
        Write-Host "✗ $doc (MISSING)" -ForegroundColor Red
    }
}

Write-Host "`nContent Guidelines:" -ForegroundColor Cyan
$contentDocs = @("docs/writing-style-guidelines.md")
foreach ($doc in $contentDocs) {
    $path = Join-Path $root $doc
    if (Test-Path $path) {
        $allDocs += $path
        Write-Host "✓ $doc" -ForegroundColor Green
    }
    else {
        Write-Host "✗ $doc (MISSING)" -ForegroundColor Red
    }
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Total documentation files: $($allDocs.Count)" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# Check for broken internal markdown links
Write-Host "`n📎 Checking for Broken Links:" -ForegroundColor Cyan
$brokenLinks = @()

foreach ($docPath in $allDocs) {
    $content = Get-Content $docPath -Raw
    $relativePath = $docPath -replace [regex]::Escape($root), '' -replace '^[\\/]', ''
    
    # Find markdown links [text](path)
    $linkPattern = '\[([^\]]+)\]\(([^)]+)\)'
    $matches = [regex]::Matches($content, $linkPattern)
    
    foreach ($match in $matches) {
        $linkText = $match.Groups[1].Value
        $linkPath = $match.Groups[2].Value
        
        # Skip external links (http/https)
        if ($linkPath -match '^https?://') {
            continue
        }
        
        # Skip anchors only (#section)
        if ($linkPath -match '^#') {
            continue
        }
        
        # Remove anchor from path
        $cleanPath = $linkPath -replace '#.*$', ''
        
        # Resolve relative path
        $docDir = Split-Path $docPath -Parent
        $targetPath = Join-Path $docDir $cleanPath
        $targetPath = [System.IO.Path]::GetFullPath($targetPath)
        
        # Check if target exists
        if (-not (Test-Path $targetPath)) {
            $brokenLinks += @{
                SourceFile   = $relativePath
                LinkText     = $linkText
                LinkPath     = $linkPath
                ResolvedPath = $targetPath -replace [regex]::Escape($root), '' -replace '^[\\/]', ''
            }
        }
    }
}

if ($brokenLinks.Count -gt 0) {
    Write-Host "  Found $($brokenLinks.Count) potentially broken link(s):" -ForegroundColor Yellow
    $brokenLinks | ForEach-Object {
        Write-Host "    ❌ $($_.SourceFile): [$($_.LinkText)]($($_.LinkPath))" -ForegroundColor Red
        Write-Host "       → Target not found: $($_.ResolvedPath)" -ForegroundColor Gray
    }
}
else {
    Write-Host "  ✓ All internal links appear valid" -ForegroundColor Green
}

# Check for missing AGENTS.md in code directories
Write-Host "`n📁 Checking for Missing AGENTS.md:" -ForegroundColor Cyan
$missingAgents = @()

$codeDirs = @(
    "src",
    "src/TechHub.Api",
    "src/TechHub.Web",
    "src/TechHub.Core",
    "src/TechHub.Infrastructure",
    "tests",
    "tests/TechHub.Api.Tests",
    "tests/TechHub.Web.Tests",
    "tests/TechHub.Core.Tests",
    "tests/TechHub.Infrastructure.Tests",
    "tests/TechHub.E2E.Tests",
    "tests/powershell",
    "scripts",
    "collections",
    "docs"
)

foreach ($dir in $codeDirs) {
    $dirPath = Join-Path $root $dir
    $agentsPath = Join-Path $dirPath "AGENTS.md"
    
    if ((Test-Path $dirPath) -and -not (Test-Path $agentsPath)) {
        $missingAgents += $dir
    }
}

if ($missingAgents.Count -gt 0) {
    Write-Host "  Found $($missingAgents.Count) director(ies) missing AGENTS.md:" -ForegroundColor Yellow
    $missingAgents | ForEach-Object {
        Write-Host "    ❌ $_/AGENTS.md" -ForegroundColor Red
    }
}
else {
    Write-Host "  ✓ All code directories have AGENTS.md" -ForegroundColor Green
}

exit 0
