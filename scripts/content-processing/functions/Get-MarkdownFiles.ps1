function Get-MarkdownFiles {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Root,
        
        [Parameter(Mandatory = $false)]
        [string[]]$IncludeDirectoryPatterns = @(),
        
        [Parameter(Mandatory = $false)]
        [string[]]$ExcludeDirectoryPatterns = @(),
        
        [Parameter(Mandatory = $false)]
        [string[]]$ExcludeFilePatterns = @()
    )

    # Parse .gitignore file from repository root for default exclusions
    $gitignorePath = Join-Path $Root '.gitignore'
    $defaultExclusions = @()
    
    if (Test-Path $gitignorePath) {
        Write-Verbose "📄 Reading .gitignore from: $gitignorePath"
        $gitignoreContent = Get-Content $gitignorePath -ErrorAction SilentlyContinue
        
        foreach ($line in $gitignoreContent) {
            # Skip comments and empty lines
            if ($line -match '^\s*#' -or [string]::IsNullOrWhiteSpace($line)) {
                continue
            }
            
            $pattern = $line.Trim()
            
            # Convert gitignore patterns to PowerShell wildcard patterns
            # Handle directory patterns (ending with /)
            if ($pattern.EndsWith('/')) {
                $baseName = $pattern.TrimEnd('/')
                
                # Handle ** (any depth) - convert to *
                $baseName = $baseName -replace '\*\*/', '*'
                $baseName = $baseName -replace '/\*\*', '/*'
                
                # For directory patterns, create TWO patterns:
                # 1. Root-level: "node_modules/*" matches /node_modules/file.md
                # 2. Nested: "*/node_modules/*" matches /path/node_modules/file.md
                # This ensures we catch both root and nested occurrences
                $defaultExclusions += "$baseName/*"
                if (-not $baseName.StartsWith('*')) {
                    $defaultExclusions += "*/$baseName/*"
                }
            }
            else {
                # Handle file patterns
                $pattern = $pattern -replace '\*\*/', '*'
                $pattern = $pattern -replace '/\*\*', '/*'
                
                # Ensure pattern works for relative paths
                if (-not $pattern.StartsWith('*')) {
                    $pattern = '*/' + $pattern
                }
                
                $defaultExclusions += $pattern
            }
        }
        
        Write-Verbose "✅ Loaded $($defaultExclusions.Count) patterns from .gitignore"
    } else {
        Write-Verbose "⚠️  No .gitignore found at: $gitignorePath"
    }
    
    # Merge default exclusions with user-provided patterns
    $allExcludeDirectoryPatterns = @($defaultExclusions) + @($ExcludeDirectoryPatterns)
    
    $allFiles = @(Get-ChildItem -Path $Root -Recurse -File -Filter '*.md')
    
    # Apply include filtering first if patterns are provided
    if ($IncludeDirectoryPatterns.Count -gt 0) {
        $filteredFiles = @($allFiles | Where-Object { 
            $shouldInclude = $false
            $relativePath = $_.FullName.Substring($Root.Length).TrimStart('\', '/')
            foreach ($pattern in $IncludeDirectoryPatterns) {
                if ($relativePath -like $pattern) {
                    $shouldInclude = $true
                    break
                }
            }
            return $shouldInclude
        })
    } else {
        $filteredFiles = @($allFiles)
    }
    
    # Apply exclude filtering if patterns are provided
    if ($allExcludeDirectoryPatterns.Count -gt 0 -or $ExcludeFilePatterns.Count -gt 0) {
        $filteredFiles = @($filteredFiles | Where-Object { 
            $shouldExclude = $false
            $relativePath = $_.FullName.Substring($Root.Length).TrimStart('\', '/')
            
            # Check directory patterns (from .gitignore and parameters)
            foreach ($pattern in $allExcludeDirectoryPatterns) {
                if ($relativePath -like $pattern) {
                    $shouldExclude = $true
                    break
                }
            }
            
            # Check file patterns if not already excluded
            if (-not $shouldExclude) {
                foreach ($pattern in $ExcludeFilePatterns) {
                    if ($relativePath -like $pattern) {
                        $shouldExclude = $true
                        break
                    }
                }
            }
            
            return -not $shouldExclude
        })
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
    if ($defaultExclusions.Count -gt 0) {
        $statusParts += "using $($defaultExclusions.Count) patterns from .gitignore"
    }
    if ($ExcludeDirectoryPatterns.Count -gt 0) {
        $statusParts += "plus $($ExcludeDirectoryPatterns.Count) additional exclusion patterns"
    }
    if ($ExcludeFilePatterns.Count -gt 0) {
        $statusParts += "excluding files matching: $($ExcludeFilePatterns -join ', ')"
    }
    
    if ($statusParts.Count -gt 0) {
        Write-Host "Found $($filteredFiles.Count) markdown files after $($statusParts -join ' and ')."
    } else {
        Write-Host "Found $($filteredFiles.Count) markdown files (no directory filtering applied)."
    }

    return $filteredFiles
}
