<#
.SYNOPSIS
    Fix markdown formatting issues in Tech Hub content files
.DESCRIPTION
    This script processes markdown files to fix Jekyll-specific and generic formatting issues.
    It can process all files in the repository or a single specific file.
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
    pwsh scripts/content-processing/fix-markdown-files.ps1 -FilePath "_blogs/2025-01-01-example-post.md"
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
} else {
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
        
        # Process Jekyll-specific markdown issues first
        Write-Host "🔧 Processing Jekyll-specific markdown issues..."
        Repair-MarkdownJekyll -FilePath $absoluteFilePath

        # Then apply generic markdown formatting fixes
        Write-Host "� Applying generic markdown formatting fixes..."
        Repair-MarkdownFormatting -FilePath $absoluteFilePath
        
        Write-Host "Fixed: $FilePath"
    }
    else {
        Write-Host "�🔍 Source root detected: $sourceRoot"

        # Process Jekyll-specific markdown issues first
        Write-Host "🔧 Processing Jekyll-specific markdown issues..."
        Repair-MarkdownJekyll -Path $sourceRoot

        # Then apply generic markdown formatting fixes
        Write-Host "🔧 Applying generic markdown formatting fixes..."
        Repair-MarkdownFormatting -Path $sourceRoot
    }
}
catch {
    Write-ErrorDetails -ErrorRecord $_ -Context "markdown processing"
    throw
}
