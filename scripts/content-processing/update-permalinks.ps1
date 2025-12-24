<#
.SYNOPSIS
    Updates permalinks in collection markdown files to include collection name prefix.

.DESCRIPTION
    This script processes all markdown files in the collections folder and updates their
    permalink frontmatter field to include the collection name as a prefix.
    
    For example:
    - Before: permalink: "/2024-09-05-Article-Title.html"
    - After:  permalink: "/news/2024-09-05-Article-Title.html"

.PARAMETER WorkspaceDirectory
    The root directory of the workspace. Defaults to the script's parent directory.

.PARAMETER WhatIf
    If specified, shows what changes would be made without actually modifying files.

.EXAMPLE
    ./update-permalinks.ps1
    
.EXAMPLE
    ./update-permalinks.ps1 -WhatIf
    
.EXAMPLE
    ./update-permalinks.ps1 -WorkspaceDirectory "/workspaces/techhub"
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$WorkspaceDirectory = (Split-Path -Parent (Split-Path -Parent $PSScriptRoot)),
    
    [Parameter(Mandatory = $false)]
    [switch]$WhatIf
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Get collections directory
$collectionsDir = Join-Path $WorkspaceDirectory "collections"

if (-not (Test-Path $collectionsDir)) {
    Write-Error "Collections directory not found: $collectionsDir"
    exit 1
}

# Get all collection directories
$collectionDirs = Get-ChildItem -Path $collectionsDir -Directory | Where-Object { $_.Name -like "_*" }

$totalFiles = 0
$updatedFiles = 0
$skippedFiles = 0

foreach ($collectionDir in $collectionDirs) {
    # Extract collection name (remove leading underscore)
    $collectionName = $collectionDir.Name -replace '^_', ''
    
    Write-Host "`nProcessing collection: $collectionName" -ForegroundColor Cyan
    
    # Get all markdown files in this collection
    $markdownFiles = Get-ChildItem -Path $collectionDir.FullName -Filter "*.md" -File
    
    foreach ($file in $markdownFiles) {
        $totalFiles++
        $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8
        
        # Check if file has frontmatter
        if ($content -notmatch '^---\s*\r?\n') {
            Write-Warning "  Skipping $($file.Name): No frontmatter found"
            $skippedFiles++
            continue
        }
        
        # Extract frontmatter
        if ($content -match '(?s)^---\s*\r?\n(.*?)\r?\n---') {
            $frontmatter = $matches[1]
            
            # Check if permalink exists and doesn't already include collection name
            if ($frontmatter -match 'permalink:\s*"?/([^/"][^"]*\.html)"?') {
                $currentPermalink = $matches[1]
                
                # Check if permalink already includes collection name
                if ($currentPermalink -like "$collectionName/*") {
                    Write-Host "  ✓ $($file.Name): Permalink already correct" -ForegroundColor Green
                    $skippedFiles++
                    continue
                }
                
                # Create new permalink with collection prefix
                $newPermalink = "/$collectionName/$currentPermalink"
                
                if ($WhatIf) {
                    Write-Host "  [WHATIF] Would update $($file.Name):" -ForegroundColor Yellow
                    Write-Host "    Old: /$currentPermalink" -ForegroundColor Gray
                    Write-Host "    New: $newPermalink" -ForegroundColor Gray
                } else {
                    # Replace the permalink in the content
                    # Match quoted permalinks: permalink: "/filename.html"
                    $content = $content -replace 'permalink:\s*"/[^/][^"]*\.html"', "permalink: `"$newPermalink`""
                    
                    # Match unquoted permalinks: permalink: /filename.html
                    $content = $content -replace 'permalink:\s*/[^/][^\s]*\.html', "permalink: `"$newPermalink`""
                    
                    # Write updated content back to file
                    Set-Content -Path $file.FullName -Value $content -Encoding UTF8 -NoNewline
                    
                    Write-Host "  ✓ Updated $($file.Name)" -ForegroundColor Green
                    Write-Host "    Old: /$currentPermalink" -ForegroundColor Gray
                    Write-Host "    New: $newPermalink" -ForegroundColor Gray
                }
                
                $updatedFiles++
            } else {
                Write-Warning "  Skipping $($file.Name): No permalink found in frontmatter"
                $skippedFiles++
            }
        } else {
            Write-Warning "  Skipping $($file.Name): Could not parse frontmatter"
            $skippedFiles++
        }
    }
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Total files processed: $totalFiles" -ForegroundColor White
Write-Host "  Files updated: $updatedFiles" -ForegroundColor Green
Write-Host "  Files skipped: $skippedFiles" -ForegroundColor Yellow

if ($WhatIf) {
    Write-Host "`n[WHATIF] No changes were made. Run without -WhatIf to apply changes." -ForegroundColor Yellow
}
