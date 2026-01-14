<#
.SYNOPSIS
    Detects potentially dead or unused code in the solution.

.DESCRIPTION
    Analyzes the codebase to find unused variables, methods, classes, and other
    potentially dead code. Uses Roslyn analyzers and pattern matching.

.PARAMETER SourceRoot
    Root directory of the repository. Defaults to auto-detection.

.PARAMETER OutputFormat
    Format for output: 'Console', 'Markdown', or 'Json'. Defaults to 'Console'.

.PARAMETER Category
    Category of dead code to look for: 'All', 'Methods', 'Properties', 'Fields'.
    Defaults to 'All'.

.EXAMPLE
    ./find-dead-code.ps1
    
.EXAMPLE
    ./find-dead-code.ps1 -Category Methods -OutputFormat Markdown
#>

[CmdletBinding()]
param(
    [Parameter()]
    [string]$SourceRoot,
    
    [Parameter()]
    [ValidateSet('Console', 'Markdown', 'Json')]
    [string]$OutputFormat = 'Console',
    
    [Parameter()]
    [ValidateSet('All', 'Methods', 'Properties', 'Fields')]
    [string]$Category = 'All'
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

#region Helper Functions

function Get-SourceRoot {
    if ($SourceRoot) {
        return $SourceRoot
    }
    
    $current = $PSScriptRoot
    while ($current -and -not (Test-Path (Join-Path $current "TechHub.slnx"))) {
        $current = Split-Path $current -Parent
    }
    
    if (-not $current) {
        throw "Could not find repository root. Please specify -SourceRoot parameter."
    }
    
    return $current
}

# Find-UnusedUsings function removed - unused usings are already handled by dotnet format in Step 2

function Find-UnusedPrivateMembers {
    param([string]$SourceRoot)
    
    $results = @()
    $csFiles = @(Get-ChildItem -Path (Join-Path $SourceRoot "src") -Filter "*.cs" -Recurse)
    
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName -Raw
        
        # Find private fields
        $privateFieldPattern = 'private\s+(?:readonly\s+)?[\w<>\[\],\s]+\s+(_\w+)\s*[;=]'
        $fieldMatches = [regex]::Matches($content, $privateFieldPattern)
        
        foreach ($match in $fieldMatches) {
            $fieldName = $match.Groups[1].Value
            
            # Count occurrences (should be > 1 if used)
            $usageCount = ([regex]::Matches($content, [regex]::Escape($fieldName))).Count
            
            if ($usageCount -le 1) {
                $results += [PSCustomObject]@{
                    Category   = "Potentially Unused Field"
                    File       = $file.Name
                    FullPath   = $file.FullName
                    Member     = $fieldName
                    Confidence = "Medium"
                    Note       = "Only found $usageCount occurrence(s) - verify manually"
                }
            }
        }
        
        # Find private methods
        $privateMethodPattern = 'private\s+(?:static\s+)?(?:async\s+)?[\w<>\[\],\s]+\s+(\w+)\s*\('
        $methodMatches = [regex]::Matches($content, $privateMethodPattern)
        
        foreach ($match in $methodMatches) {
            $methodName = $match.Groups[1].Value
            
            # Skip constructors and common patterns
            if ($methodName -match '^(Dispose|Initialize|Configure)') {
                continue
            }
            
            # Count occurrences
            $usageCount = ([regex]::Matches($content, "\b$([regex]::Escape($methodName))\s*\(")).Count
            
            if ($usageCount -le 1) {
                $results += [PSCustomObject]@{
                    Category   = "Potentially Unused Method"
                    File       = $file.Name
                    FullPath   = $file.FullName
                    Member     = $methodName
                    Confidence = "Low"
                    Note       = "Only found $usageCount call(s) - may be called externally or via reflection"
                }
            }
        }
    }
    
    return $results
}

function Find-CommentedCode {
    param([string]$SourceRoot)
    
    $results = @()
    $csFiles = @(Get-ChildItem -Path (Join-Path $SourceRoot "src") -Filter "*.cs" -Recurse)
    
    foreach ($file in $csFiles) {
        $lines = Get-Content $file.FullName
        $lineNumber = 0
        $inCommentBlock = $false
        $commentBlockStart = 0
        $commentBlockLines = 0
        
        foreach ($line in $lines) {
            $lineNumber++
            
            # Check for multi-line comment start
            if ($line -match '/\*') {
                $inCommentBlock = $true
                $commentBlockStart = $lineNumber
                $commentBlockLines = 0
            }
            
            # Check for commented code patterns
            $codePatterns = @(
                '//\s*(public|private|protected|internal)\s+',
                '//\s*(var|int|string|bool|void)\s+\w+\s*[=;]',
                '//\s*\w+\s*\.\s*\w+\s*\(',
                '//\s*(if|for|foreach|while|switch)\s*\(',
                '//\s*return\s+',
                '//\s*throw\s+'
            )
            
            foreach ($pattern in $codePatterns) {
                if ($line -match $pattern) {
                    $results += [PSCustomObject]@{
                        Category   = "Commented Code"
                        File       = $file.Name
                        FullPath   = $file.FullName
                        Line       = $lineNumber
                        Content    = $line.Trim()
                        Confidence = "Medium"
                    }
                    break
                }
            }
            
            # Check for multi-line comment end
            if ($line -match '\*/') {
                if ($commentBlockLines -gt 5) {
                    $results += [PSCustomObject]@{
                        Category   = "Large Comment Block"
                        File       = $file.Name
                        FullPath   = $file.FullName
                        StartLine  = $commentBlockStart
                        EndLine    = $lineNumber
                        Lines      = $commentBlockLines
                        Confidence = "Low"
                        Note       = "Large comment block - may contain dead code"
                    }
                }
                $inCommentBlock = $false
            }
            
            if ($inCommentBlock) {
                $commentBlockLines++
            }
        }
    }
    
    return $results
}

function Find-UnusedCssClasses {
    param([string]$SourceRoot)
    
    $results = @()
    $webRoot = Join-Path $SourceRoot "src/TechHub.Web"
    
    if (-not (Test-Path $webRoot)) {
        return $results
    }
    
    Write-Host "  Step 1: Collecting all CSS class definitions..." -ForegroundColor Gray
    
    # Step 1: Get all CSS files and extract class definitions
    $cssFiles = @(Get-ChildItem -Path $webRoot -Filter "*.css" -Recurse)
    $allCssClasses = @{}
    
    foreach ($cssFile in $cssFiles) {
        $content = Get-Content $cssFile.FullName -Raw
        
        # Match class selectors (e.g., .class-name, .class-name:hover, etc.)
        $classPattern = '\.([a-zA-Z0-9_-]+)(?:[:\[,\s>+~{]|\s|$)'
        $matches = [regex]::Matches($content, $classPattern)
        
        foreach ($match in $matches) {
            $className = $match.Groups[1].Value
            if (-not $allCssClasses.ContainsKey($className)) {
                $allCssClasses[$className] = @{
                    DefinedIn = $cssFile.Name
                    Used      = $false
                }
            }
        }
    }
    
    Write-Host "  Found $($allCssClasses.Count) unique CSS classes" -ForegroundColor Gray
    Write-Host "  Step 2: Reading all Razor/HTML/CSS files for usage check..." -ForegroundColor Gray
    
    # Step 2: Read ALL content files ONCE and build combined content
    $razorFiles = @(Get-ChildItem -Path $webRoot -Include "*.razor", "*.cshtml", "*.html", "*.css", "*.js" -Recurse)
    $allContent = New-Object System.Text.StringBuilder
    
    foreach ($file in $razorFiles) {
        $content = Get-Content $file.FullName -Raw
        [void]$allContent.AppendLine($content)
    }
    
    $combinedContent = $allContent.ToString()
    
    Write-Host "  Step 3: Checking which CSS classes are used..." -ForegroundColor Gray
    
    # Step 3: Check each CSS class against combined content (single pass)
    foreach ($className in $allCssClasses.Keys) {
        # Check if class appears in class="..." or class='...' or in CSS/JS
        # Using simple contains for maximum performance
        if ($combinedContent -match "\b$([regex]::Escape($className))\b") {
            $allCssClasses[$className].Used = $true
        }
    }
    
    # Step 4: Report unused classes
    $unusedCount = 0
    foreach ($className in $allCssClasses.Keys) {
        if (-not $allCssClasses[$className].Used) {
            $unusedCount++
            $results += [PSCustomObject]@{
                Category   = "Unused CSS Class"
                Class      = $className
                DefinedIn  = $allCssClasses[$className].DefinedIn
                Confidence = "Medium"
                Note       = "Class not found in any file - may be added dynamically"
            }
        }
    }
    
    Write-Host "  Found $unusedCount potentially unused CSS classes" -ForegroundColor $(if ($unusedCount -gt 0) { "Yellow" } else { "Green" })
    
    return $results
}

#endregion

#region Main Execution

$root = Get-SourceRoot
Write-Host "Scanning for dead code in: $root" -ForegroundColor Cyan

$allResults = @{
    UnusedMembers = @()
    CommentedCode = @()
    UnusedCss     = @()
    Summary       = @{
        TotalIssues      = 0
        HighConfidence   = 0
        MediumConfidence = 0
        LowConfidence    = 0
    }
}

# Note: Unused usings are handled by dotnet format in Step 2 of the cleanup process

# Find unused private members
if ($Category -in @('All', 'Methods', 'Properties', 'Fields')) {
    Write-Host "`nChecking for unused private members..." -ForegroundColor Yellow
    try {
        $allResults.UnusedMembers = Find-UnusedPrivateMembers -SourceRoot $root
        Write-Host "  Found $($allResults.UnusedMembers.Count) potentially unused members" -ForegroundColor $(if ($allResults.UnusedMembers.Count -gt 0) { "Yellow" } else { "Green" })
    }
    catch {
        Write-Warning "  Could not check private members: $($_.Exception.Message)"
        Write-Host "  Skipping unused members check" -ForegroundColor Gray
    }
}

# Find commented code
if ($Category -eq 'All') {
    Write-Host "`nChecking for commented code..." -ForegroundColor Yellow
    try {
        $allResults.CommentedCode = Find-CommentedCode -SourceRoot $root
        Write-Host "  Found $($allResults.CommentedCode.Count) potential commented code blocks" -ForegroundColor $(if ($allResults.CommentedCode.Count -gt 0) { "Yellow" } else { "Green" })
    }
    catch {
        Write-Warning "  Could not check commented code: $($_.Exception.Message)"
        Write-Host "  Skipping commented code check" -ForegroundColor Gray
    }
}

# Find unused CSS classes
if ($Category -eq 'All') {
    Write-Host "`nChecking for unused CSS classes..." -ForegroundColor Yellow
    try {
        $allResults.UnusedCss = Find-UnusedCssClasses -SourceRoot $root
    }
    catch {
        Write-Warning "  Could not check CSS classes: $($_.Exception.Message)"
        Write-Host "  Skipping CSS class check" -ForegroundColor Gray
    }
}

# Calculate summary
$allIssues = @()
if ($allResults.UnusedMembers) { $allIssues += @($allResults.UnusedMembers) }
if ($allResults.CommentedCode) { $allIssues += @($allResults.CommentedCode) }
if ($allResults.UnusedCss) { $allIssues += @($allResults.UnusedCss) }

$allResults.Summary.TotalIssues = $allIssues.Count
$allResults.Summary.HighConfidence = @($allIssues | Where-Object { $_.Confidence -eq "High" }).Count
$allResults.Summary.MediumConfidence = @($allIssues | Where-Object { $_.Confidence -eq "Medium" }).Count
$allResults.Summary.LowConfidence = @($allIssues | Where-Object { $_.Confidence -eq "Low" }).Count

#endregion

#region Output

switch ($OutputFormat) {
    'Console' {
        Write-Host "`n===== Dead Code Detection Results =====" -ForegroundColor Cyan
        Write-Host "Note: Unused usings are automatically removed by 'dotnet format' in Step 2" -ForegroundColor Gray
        
        $unusedMembersCount = @($allResults.UnusedMembers).Count
        if ($unusedMembersCount -gt 0) {
            Write-Host "`nPotentially Unused Members:" -ForegroundColor Yellow
            foreach ($member in $allResults.UnusedMembers) {
                $confidence = switch ($member.Confidence) {
                    "High" { "🔴" }
                    "Medium" { "🟡" }
                    "Low" { "🟢" }
                }
                Write-Host "  $confidence $($member.File): $($member.Member) ($($member.Category))" -ForegroundColor Yellow
            }
        }
        
        $commentedCodeCount = @($allResults.CommentedCode).Count
        if ($commentedCodeCount -gt 0) {
            Write-Host "`nCommented Code:" -ForegroundColor Yellow
            @($allResults.CommentedCode) | Group-Object File | ForEach-Object {
                Write-Host "  📁 $($_.Name): $($_.Count) instance(s)" -ForegroundColor Yellow
            }
        }
        
        $unusedCssCount = @($allResults.UnusedCss).Count
        if ($unusedCssCount -gt 0) {
            Write-Host "`nPotentially Unused CSS Classes:" -ForegroundColor Yellow
            @($allResults.UnusedCss) | Group-Object DefinedIn | ForEach-Object {
                Write-Host "  📁 $($_.Name):" -ForegroundColor Yellow
                $_.Group | ForEach-Object {
                    Write-Host "    🟡 .$($_.Class)" -ForegroundColor Yellow
                }
            }
        }
        
        Write-Host "`n===== Summary =====" -ForegroundColor Cyan
        Write-Host "  Total Issues: $($allResults.Summary.TotalIssues)"
        Write-Host "  High Confidence: $($allResults.Summary.HighConfidence)" -ForegroundColor Red
        Write-Host "  Medium Confidence: $($allResults.Summary.MediumConfidence)" -ForegroundColor Yellow
        Write-Host "  Low Confidence: $($allResults.Summary.LowConfidence)" -ForegroundColor Green
        
        Write-Host "`n⚠️  Note: These are potential issues. Always verify manually before removing code." -ForegroundColor Cyan
    }
    
    'Markdown' {
        $output = @"
## Dead Code Detection Results

### Summary

| Metric | Count |
|--------|-------|
| Total Issues | $($allResults.Summary.TotalIssues) |
| High Confidence | $($allResults.Summary.HighConfidence) |
| Medium Confidence | $($allResults.Summary.MediumConfidence) |
| Low Confidence | $($allResults.Summary.LowConfidence) |

"@
        
        $unusedMembersCount = @($allResults.UnusedMembers).Count
        if ($unusedMembersCount -gt 0) {
            $output += "`n### Potentially Unused Members`n`n"
            $output += "| Confidence | File | Member | Category |`n"
            $output += "|------------|------|--------|----------|`n"
            foreach ($member in $allResults.UnusedMembers) {
                $emoji = switch ($member.Confidence) { "High" { "🔴" } "Medium" { "🟡" } "Low" { "🟢" } }
                $output += "| $emoji $($member.Confidence) | ``$($member.File)`` | ``$($member.Member)`` | $($member.Category) |`n"
            }
        }
        
        $commentedCodeCount = @($allResults.CommentedCode).Count
        if ($commentedCodeCount -gt 0) {
            $output += "`n### Commented Code`n`n"
            $grouped = @($allResults.CommentedCode) | Group-Object File
            foreach ($group in $grouped) {
                $output += "- **$($group.Name)**: $($group.Count) instance(s)`n"
            }
        }
        
        $unusedCssCount = @($allResults.UnusedCss).Count
        if ($unusedCssCount -gt 0) {
            $output += "`n### Potentially Unused CSS Classes`n`n"
            $grouped = @($allResults.UnusedCss) | Group-Object DefinedIn
            foreach ($group in $grouped) {
                $output += "- **$($group.Name)**:`n"
                $group.Group | ForEach-Object {
                    $output += "  - ``.$($_.Class)```n"
                }
            }
        }
        
        $output += "`n> ⚠️ **Note**: These are potential issues. Always verify manually before removing code.`n"
        
        Write-Output $output
    }
    
    'Json' {
        $allResults | ConvertTo-Json -Depth 5
    }
}

#endregion
