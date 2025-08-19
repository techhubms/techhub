function Get-MarkdownFiles {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Root,
        
        [Parameter(Mandatory = $false)]
        [string[]]$IncludeDirectoryPatterns = @(),
        
        [Parameter(Mandatory = $false)]
        [string[]]$ExcludeDirectoryPatterns = @('**/node_modules/*', '.git/*', 'spec/*', 'vendor/*', '_site/*')
    )

    $allFiles = Get-ChildItem -Path $Root -Recurse -File -Filter '*.md'
    
    # Apply include filtering first if patterns are provided
    if ($IncludeDirectoryPatterns.Count -gt 0) {
        $filteredFiles = $allFiles | Where-Object { 
            $shouldInclude = $false
            $relativePath = $_.FullName.Substring($Root.Length).TrimStart('\', '/')
            foreach ($pattern in $IncludeDirectoryPatterns) {
                if ($relativePath -like $pattern) {
                    $shouldInclude = $true
                    break
                }
            }
            return $shouldInclude
        }
    } else {
        $filteredFiles = $allFiles
    }
    
    # Apply exclude filtering if patterns are provided
    if ($ExcludeDirectoryPatterns.Count -gt 0) {
        $filteredFiles = $filteredFiles | Where-Object { 
            $shouldExclude = $false
            $relativePath = $_.FullName.Substring($Root.Length).TrimStart('\', '/')
            
            foreach ($pattern in $ExcludeDirectoryPatterns) {
                if ($relativePath -like $pattern) {
                    $shouldExclude = $true
                    break
                }
            }
            return -not $shouldExclude
        }
    }
    
    # Log directory breakdown after filtering
    Write-Host "📊 Directory breakdown of $($filteredFiles.Count) total markdown files:"
    $directoryGroups = $filteredFiles | Group-Object { $_.Directory.Name } | Sort-Object Count -Descending
    foreach ($group in $directoryGroups) {
        $samplePath = ($group.Group | Select-Object -First 1).FullName
        Write-Host "  📁 '$($group.Name)': $($group.Count) files (e.g., $samplePath)"
    }
    
    # Build status message
    $statusParts = @()
    if ($IncludeDirectoryPatterns.Count -gt 0) {
        $statusParts += "including directories matching: $($IncludeDirectoryPatterns -join ', ')"
    }
    if ($ExcludeDirectoryPatterns.Count -gt 0) {
        $statusParts += "excluding directories matching: $($ExcludeDirectoryPatterns -join ', ')"
    }
    
    if ($statusParts.Count -gt 0) {
        Write-Host "Found $($filteredFiles.Count) markdown files after $($statusParts -join ' and ')."
    } else {
        Write-Host "Found $($filteredFiles.Count) markdown files (no directory filtering applied)."
    }

    return $filteredFiles
}
