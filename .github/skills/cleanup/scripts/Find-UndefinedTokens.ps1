<#
.SYNOPSIS
    Finds CSS color references that don't exist in design-tokens.css
.DESCRIPTION
    Scans all CSS files for var(--color-*) references and checks if each
    token is actually defined in design-tokens.css
#>

$srcPath = "/workspaces/techhub/src/TechHub.Web"
$designTokensPath = "$srcPath/wwwroot/css/design-tokens.css"

Write-Host "=== Find Undefined Color References ===" -ForegroundColor Cyan

# Read design-tokens.css and extract all defined color tokens
$tokensContent = Get-Content $designTokensPath -Raw
$definedPattern = '(--color-[a-z\-]+)\s*:'
$definedMatches = [regex]::Matches($tokensContent, $definedPattern)
$definedTokens = $definedMatches | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique

Write-Host "Found $($definedTokens.Count) defined color tokens in design-tokens.css`n" -ForegroundColor DarkGray

# Find all CSS files (excluding design-tokens.css and obj/)
$cssFiles = Get-ChildItem -Path $srcPath -Filter '*.css' -Recurse | 
Where-Object { 
    $_.FullName -notmatch '[\\/]obj[\\/]' -and 
    $_.Name -ne 'design-tokens.css' 
}

Write-Host "Scanning $($cssFiles.Count) CSS files for references...`n" -ForegroundColor DarkGray

# Find all var(--color-*) references
$undefinedRefs = @()

foreach ($file in $cssFiles) {
    $content = Get-Content $file.FullName -Raw
    $relativePath = $file.FullName.Replace($srcPath, '').TrimStart('/\')
    
    # Find all var(--color-*) references
    $refPattern = 'var\((--color-[a-z\-]+)\)'
    $refMatches = [regex]::Matches($content, $refPattern)
    
    foreach ($match in $refMatches) {
        $token = $match.Groups[1].Value
        
        if ($token -notin $definedTokens) {
            # Find line number
            $beforeMatch = $content.Substring(0, $match.Index)
            $lineNumber = ($beforeMatch -split "`n").Count
            
            $undefinedRefs += [PSCustomObject]@{
                Token = $token
                File  = $relativePath
                Line  = $lineNumber
            }
        }
    }
}

# Group and display results
if ($undefinedRefs.Count -eq 0) {
    Write-Host "✓ All color references are defined! No orphaned tokens found." -ForegroundColor Green
}
else {
    Write-Host "=== UNDEFINED COLOR REFERENCES ===" -ForegroundColor Red
    Write-Host "These tokens are used but NOT defined in design-tokens.css:`n" -ForegroundColor Yellow
    
    $grouped = $undefinedRefs | Group-Object Token | Sort-Object Name
    
    foreach ($group in $grouped) {
        Write-Host "$($group.Name)" -ForegroundColor Red
        foreach ($ref in $group.Group) {
            Write-Host "  └─ $($ref.File):$($ref.Line)" -ForegroundColor DarkGray
        }
    }
    
    Write-Host "`n=== SUMMARY ===" -ForegroundColor Cyan
    Write-Host "Undefined tokens: $($grouped.Count)" -ForegroundColor Red
    Write-Host "Total references: $($undefinedRefs.Count)" -ForegroundColor Yellow
}
