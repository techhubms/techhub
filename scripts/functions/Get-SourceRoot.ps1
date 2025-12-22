function Get-SourceRoot {
    if ($env:GITHUB_WORKSPACE) {
        return $env:GITHUB_WORKSPACE
    } else {
        # Start from current directory and work up to find repository root
        $currentPath = (Get-Location).Path
        
        while ($currentPath -and $currentPath -ne [System.IO.Path]::GetPathRoot($currentPath)) {
            # Check for repository indicators - prioritize .git over _config.yml
            $gitDir = Join-Path $currentPath ".git"
            
            if (Test-Path $gitDir) {
                return $currentPath
            }
            
            # Move up one directory
            $currentPath = Split-Path $currentPath -Parent
        }
        
        # If no .git found, look for _config.yml as secondary indicator
        $currentPath = (Get-Location).Path
        while ($currentPath -and $currentPath -ne [System.IO.Path]::GetPathRoot($currentPath)) {
            $configFile = Join-Path $currentPath "_config.yml"
            
            if (Test-Path $configFile) {
                return $currentPath
            }
            
            # Move up one directory
            $currentPath = Split-Path $currentPath -Parent
        }
        
        # Fallback: if we can't find repository root, use current directory
        return (Get-Location).Path
    }
}
