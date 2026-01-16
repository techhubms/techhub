<#
.SYNOPSIS
    Fix markdown formatting issues in Tech Hub content files
.DESCRIPTION
    This script processes markdown files to fix AI-generated markdown formatting issues.
    It fixes common problems like missing blank lines, heading spacing, list formatting, etc.
    It does NOT modify frontmatter - templates already generate correct .NET format.
.PARAMETER FilePath
    Optional. Path to a specific markdown file to process (relative to repository root).
    When specified, only this file will be processed instead of all markdown files.
.PARAMETER WorkspaceDirectory
    Optional. Path to the workspace directory. Defaults to the script's directory.
    When running in GitHub Actions, this should be set to the GitHub workspace path.
.EXAMPLE
    # Process all markdown files in the repository
    pwsh scripts/content-processing/fix-markdown-files.ps1
.EXAMPLE
    # Process a specific file
    pwsh scripts/content-processing/fix-markdown-files.ps1 -FilePath "docs/site-overview.md"
.EXAMPLE
    # Process a file with full path
    pwsh scripts/content-processing/fix-markdown-files.ps1 -FilePath "collections/_blogs/2025-01-01-example-item.md"
.EXAMPLE
    # Run from GitHub Actions with workspace directory
    pwsh scripts/content-processing/fix-markdown-files.ps1 -WorkspaceDirectory ${{ github.workspace }}
#>
param(
    [Parameter(Mandatory = $false)]
    [string]$FilePath,
    
    [Parameter(Mandatory = $false)]
    [string]$WorkspaceDirectory = $PSScriptRoot
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# Determine the correct functions path
$functionsPath = if ($WorkspaceDirectory -eq $PSScriptRoot) {
    # Running from the script's directory
    Join-Path $PSScriptRoot "functions"
}
else {
    # Running from workspace root
    Join-Path $WorkspaceDirectory "scripts/content-processing/functions"
}

. (Join-Path $functionsPath "Write-ErrorDetails.ps1")

try {
    Get-ChildItem -Path $functionsPath -Filter "*.ps1" | 
    Where-Object { $_.Name -ne "Write-ErrorDetails.ps1" } |
    ForEach-Object { . $_.FullName }

    $sourceRoot = Get-SourceRoot

    if ($FilePath) {
        # Convert relative path to absolute path
        $absoluteFilePath = Join-Path $sourceRoot $FilePath
        
        if (-not (Test-Path $absoluteFilePath)) {
            Write-Host "❌ File not found: $absoluteFilePath" -ForegroundColor Red
            exit 1
        }
        
        Write-Host "🔍 Processing single file: $FilePath"
        Write-Host "🔧 Fixing AI-generated markdown formatting issues..."
        Repair-MarkdownFormatting -FilePath $absoluteFilePath
        
        Write-Host "✅ Fixed: $FilePath"
    }
    else {
        Write-Host "🔍 Source root detected: $sourceRoot"
        Write-Host "🔧 Fixing AI-generated markdown formatting issues..."
        Repair-MarkdownFormatting -Path $sourceRoot
        Write-Host "✅ Markdown formatting complete"
    }
}
catch {
    Write-ErrorDetails -ErrorRecord $_ -Context "markdown processing"
    throw
}
