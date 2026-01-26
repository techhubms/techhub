<#
.SYNOPSIS
    Counts how many times each color token is used across CSS files
#>

$srcPath = "/workspaces/techhub/src/TechHub.Web"
$designTokensPath = "$srcPath/wwwroot/css/design-tokens.css"

# Read design-tokens.css
$tokensContent = Get-Content $designTokensPath -Raw

# Extract all color tokens
$tokenPattern = '(--color-[a-z\-]+)\s*:'
$tokenMatches = [regex]::Matches($tokensContent, $tokenPattern)
$allTokens = $tokenMatches | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique

Write-Host "=== Color Token Usage Count ===" -ForegroundColor Cyan
Write-Host "Found $($allTokens.Count) color tokens`n" -ForegroundColor DarkGray

# Find all CSS files (excluding design-tokens.css and obj/)
$cssFiles = Get-ChildItem -Path $srcPath -Filter '*.css' -Recurse | 
Where-Object { 
    $_.FullName -notmatch '[\\/]obj[\\/]' -and 
    $_.Name -ne 'design-tokens.css' 
}

# Build combined content
$allCssContent = $cssFiles | ForEach-Object { Get-Content $_.FullName -Raw } | Out-String

# Count usage for each token
$results = @()
foreach ($token in $allTokens) {
    $pattern = "var\($([regex]::Escape($token))[\),]"
    $usageMatches = [regex]::Matches($allCssContent, $pattern)
    $count = $usageMatches.Count
    
    $results += [PSCustomObject]@{
        Token = $token
        Count = $count
    }
}

# Sort by count (descending) then by name
$sorted = $results | Sort-Object -Property @{Expression = "Count"; Descending = $true }, Token

# Display results
Write-Host ("{0,-45} {1,6}" -f "TOKEN", "COUNT") -ForegroundColor White
Write-Host ("{0,-45} {1,6}" -f "-----", "-----") -ForegroundColor DarkGray

foreach ($item in $sorted) {
    $color = if ($item.Count -eq 0) { "Red" } 
    elseif ($item.Count -eq 1) { "Yellow" } 
    else { "Green" }
    
    Write-Host ("{0,-45} {1,6}" -f $item.Token, $item.Count) -ForegroundColor $color
}

# Summary
$unused = ($results | Where-Object { $_.Count -eq 0 }).Count
$singleUse = ($results | Where-Object { $_.Count -eq 1 }).Count
$multiUse = ($results | Where-Object { $_.Count -gt 1 }).Count

Write-Host "`n=== SUMMARY ===" -ForegroundColor Cyan
Write-Host "Multi-use (2+):  $multiUse" -ForegroundColor Green
Write-Host "Single-use (1): $singleUse" -ForegroundColor Yellow
Write-Host "Unused (0):     $unused" -ForegroundColor Red
