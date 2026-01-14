#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Fix markdown frontmatter and content for migration to .NET architecture

.DESCRIPTION
    This script processes markdown files in collections/ to:
    - Rename 'categories' to 'section_names' and normalize values (e.g., 'GitHub Copilot' → 'github-copilot')
    - Remove 'tags_normalized' frontmatter field
    - Remove 'excerpt_separator' frontmatter field
    - Fix permalinks to include /primarySection/collection/ prefix
    - Remove 'description' field from frontmatter
    - Replace ALL Jekyll variables ({{ page.var }}) with actual values from frontmatter
    - Keep {% youtube VIDEO_ID %} tags intact
    
    The script is idempotent - can be run multiple times safely without breaking already-fixed files.
    Will crash on errors rather than swallowing them.

.PARAMETER Path
    Path to markdown file or directory. Defaults to collections/

.PARAMETER WhatIf
    Show what would be changed without actually modifying files

.EXAMPLE
    ./scripts/Fix-MarkdownFrontmatter.ps1
    
.EXAMPLE
    ./scripts/Fix-MarkdownFrontmatter.ps1 -Path collections/_videos
    
.EXAMPLE
    ./scripts/Fix-MarkdownFrontmatter.ps1 -WhatIf
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    [Parameter()]
    [string]$Path = "collections"
)

$ErrorActionPreference = 'Stop'

# Section name normalization mapping (display name → normalized identifier)
$sectionMapping = @{
    'AI'             = 'ai'
    'Azure'          = 'azure'
    'GitHub Copilot' = 'github-copilot'
    '.NET'           = 'dotnet'
    'DevOps'         = 'devops'
    'Security'       = 'security'
    'Coding'         = 'coding'
    'Cloud'          = 'cloud'
}

# Collection to section priority mapping (for determining primary section)
$sectionPriority = @(
    'github-copilot'
    'ai'
    'dotnet'
    'azure'
    'devops'
    'security'
    'coding'
    'cloud'
)

function Normalize-SectionName {
    param([string]$DisplayName)
    
    # Check mapping first
    if ($sectionMapping.ContainsKey($DisplayName)) {
        return $sectionMapping[$DisplayName]
    }
    
    # Default normalization: lowercase and replace spaces with hyphens
    return $DisplayName.ToLowerInvariant() -replace '\s+', '-'
}

function Get-PrimarySection {
    param(
        [string[]]$SectionNames,
        [string]$Collection
    )
    
    # Ensure SectionNames is an array
    if ($SectionNames -is [string]) {
        $SectionNames = @($SectionNames)
    }
    
    # Special case: 'all' collection always uses 'all' section
    if ($Collection -eq 'all') {
        return 'all'
    }
    
    # Find first section from priority list that exists in content's sections
    foreach ($prioritySection in $sectionPriority) {
        if ($SectionNames -contains $prioritySection) {
            return $prioritySection
        }
    }
    
    # Fallback: use first section
    if ($SectionNames -and $SectionNames.Length -gt 0) {
        return $SectionNames[0]
    }
    
    throw "Content has no sections assigned"
}

function Process-MarkdownFile {
    param([string]$FilePath)
    
    Write-Host "Processing: $FilePath" -ForegroundColor Cyan
    
    # Read file content
    $content = Get-Content -Path $FilePath -Raw -Encoding UTF8
    
    # Split frontmatter and content (handle both LF and CRLF, use dotall mode)
    if ($content -notmatch '(?s)^---[\r\n]+(.*?)[\r\n]+---[\r\n]+(.*)$') {
        Write-Warning "  Skipping - no frontmatter found"
        return
    }
    
    $frontmatterText = $Matches[1]
    $markdownContent = $Matches[2]
    
    # Parse frontmatter as key-value pairs
    $frontmatter = @{}
    $frontmatterLines = $frontmatterText -split '\r?\n'
    
    $currentKey = $null
    $currentValue = @()
    $inList = $false
    
    foreach ($line in $frontmatterLines) {
        # List item
        if ($line -match '^\s*-\s+(.+)$') {
            $inList = $true
            $currentValue += $Matches[1].Trim('"').Trim("'")
        }
        # Key-value pair
        elseif ($line -match '^([^:]+):\s*(.*)$') {
            # Save previous key-value
            if ($currentKey) {
                if ($inList) {
                    $frontmatter[$currentKey] = $currentValue
                }
                else {
                    $frontmatter[$currentKey] = $currentValue[0]
                }
            }
            
            # Start new key
            $currentKey = $Matches[1].Trim()
            $value = $Matches[2].Trim().Trim('"').Trim("'")
            $currentValue = @($value)
            $inList = $false
        }
    }
    
    # Save last key-value
    if ($currentKey) {
        if ($inList -or ($currentValue -and $currentValue.Length -gt 1)) {
            $frontmatter[$currentKey] = $currentValue
        }
        else {
            $frontmatter[$currentKey] = $currentValue[0]
        }
    }
    
    # Determine collection from file path
    $collection = if ($FilePath -match 'collections/_([^/]+)/') { $Matches[1] } else { 'unknown' }
    
    # 1. Rename 'categories' to 'section_names' and normalize
    if ($frontmatter.ContainsKey('categories')) {
        $categories = $frontmatter['categories']
        if ($categories -is [string]) {
            $categories = @($categories)
        }
        
        $sectionNames = @($categories | ForEach-Object { Normalize-SectionName $_ } | Where-Object { $_ })
        $frontmatter['section_names'] = $sectionNames
        $frontmatter.Remove('categories')
        Write-Host "  ✓ Renamed categories → section_names ($($sectionNames.Count) sections)" -ForegroundColor Green
    }
    elseif ($frontmatter.ContainsKey('section_names')) {
        Write-Host "  ℹ Already has section_names" -ForegroundColor Gray
    }
    else {
        throw "No categories or section_names found in frontmatter"
    }
    
    # 2. Remove tags_normalized
    if ($frontmatter.ContainsKey('tags_normalized')) {
        $frontmatter.Remove('tags_normalized')
        Write-Host "  ✓ Removed tags_normalized" -ForegroundColor Green
    }
    
    # 3. Remove excerpt_separator
    if ($frontmatter.ContainsKey('excerpt_separator')) {
        $frontmatter.Remove('excerpt_separator')
        Write-Host "  ✓ Removed excerpt_separator" -ForegroundColor Green
    }
    
    # 4. Fix permalink to include /section/collection/ prefix
    if ($frontmatter.ContainsKey('permalink')) {
        $permalink = $frontmatter['permalink']
        
        # Check if already has section/collection prefix (idempotent check)
        if ($permalink -notmatch '^/[^/]+/[^/]+/') {
            # Extract filename from permalink
            $filename = $permalink -replace '^/', '' -replace '\.html$', ''
            
            # Remove date prefix if present (YYYY-MM-DD-)
            $slug = $filename -replace '^\d{4}-\d{2}-\d{2}-', ''
            
            # Determine primary section
            $sectionNames = $frontmatter['section_names']
            if ($sectionNames -isnot [array]) {
                $sectionNames = @($sectionNames)
            }
            $primarySection = Get-PrimarySection -SectionNames $sectionNames -Collection $collection
            
            # Build new permalink
            $newPermalink = "/$primarySection/$collection/$slug"
            $frontmatter['permalink'] = $newPermalink
            Write-Host "  ✓ Fixed permalink: $permalink → $newPermalink" -ForegroundColor Green
        }
        else {
            Write-Host "  ℹ Permalink already has section/collection prefix" -ForegroundColor Gray
        }
    }
    
    # 5. Save description for variable replacement, then remove from frontmatter
    $description = $frontmatter['description']
    if ($frontmatter.ContainsKey('description')) {
        $frontmatter.Remove('description')
        Write-Host "  ✓ Removed description from frontmatter" -ForegroundColor Green
    }
    
    # 6. Replace Jekyll variables in content (except {% youtube %})
    # Replace {{ page.description }} with actual description
    if ($description) {
        $markdownContent = $markdownContent -replace '\{\{\s*page\.description\s*\}\}', $description
    }
    
    # Replace {{ "/path" | relative_url }} with /path
    $markdownContent = $markdownContent -replace '\{\{\s*"([^"]+)"\s*\|\s*relative_url\s*\}\}', '$1'
    
    # Replace any other {{ page.variable }} with frontmatter values
    $markdownContent = [regex]::Replace($markdownContent, '\{\{\s*page\.(\w+)\s*\}\}', {
            param($match)
            $varName = $match.Groups[1].Value
            if ($frontmatter.ContainsKey($varName)) {
                $value = $frontmatter[$varName]
                if ($value -is [array]) {
                    return $value -join ', '
                }
                return $value
            }
            return $match.Value  # Keep original if not found
        })
    
    # Remove {% raw %} and {% endraw %} tags
    $markdownContent = $markdownContent -replace '\{%\s*(?:raw|endraw)\s*%\}', ''
    
    Write-Host "  ✓ Replaced Jekyll variables" -ForegroundColor Green
    
    # Rebuild frontmatter YAML
    $newFrontmatter = @()
    $newFrontmatter += '---'
    
    foreach ($key in $frontmatter.Keys | Sort-Object) {
        $value = $frontmatter[$key]
        
        if ($value -is [array]) {
            $newFrontmatter += "$key`:"
            foreach ($item in $value) {
                $newFrontmatter += "  - `"$item`""
            }
        }
        else {
            $newFrontmatter += "$key`: `"$value`""
        }
    }
    
    $newFrontmatter += '---'
    
    # Combine frontmatter and content
    $newContent = ($newFrontmatter -join "`n") + "`n" + $markdownContent
    
    # Write back to file
    if ($PSCmdlet.ShouldProcess($FilePath, "Update frontmatter and content")) {
        Set-Content -Path $FilePath -Value $newContent -Encoding UTF8 -NoNewline
        Write-Host "  ✓ Saved changes" -ForegroundColor Green
    }
    else {
        Write-Host "  [WhatIf] Would save changes" -ForegroundColor Yellow
    }
}

# Main execution
Write-Host "==> Fixing markdown frontmatter and content" -ForegroundColor Magenta
Write-Host ""

# Get all markdown files
$files = if (Test-Path $Path -PathType Leaf) {
    @(Get-Item $Path)
}
else {
    @(Get-ChildItem -Path $Path -Filter *.md -Recurse)
}

Write-Host "Found $($files.Count) markdown files" -ForegroundColor Cyan
Write-Host ""

$processedCount = 0
$errorCount = 0

foreach ($file in $files) {
    try {
        Process-MarkdownFile -FilePath $file.FullName
        $processedCount++
        Write-Host ""
    }
    catch {
        $errorCount++
        Write-Host ""
        Write-Host "ERROR processing $($file.FullName):" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
        Write-Host $_.ScriptStackTrace -ForegroundColor Red
        Write-Host ""
        throw  # Re-throw to crash instead of continuing
    }
}

Write-Host ""
Write-Host "==> Summary" -ForegroundColor Magenta
Write-Host "Processed: $processedCount files" -ForegroundColor Green
if ($errorCount -gt 0) {
    Write-Host "Errors: $errorCount files" -ForegroundColor Red
}
