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
    Category of dead code to look for: 'All', 'Usings', 'Methods', 'Properties', 'Fields'.
    Defaults to 'All'.

.EXAMPLE
    ./find-dead-code.ps1
    
.EXAMPLE
    ./find-dead-code.ps1 -Category Usings -OutputFormat Markdown
#>

[CmdletBinding()]
param(
    [Parameter()]
    [string]$SourceRoot,
    
    [Parameter()]
    [ValidateSet('Console', 'Markdown', 'Json')]
    [string]$OutputFormat = 'Console',
    
    [Parameter()]
    [ValidateSet('All', 'Usings', 'Methods', 'Properties', 'Fields')]
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

function Find-UnusedUsings {
    param([string]$SourceRoot)
    
    $results = @()
    
    # Run dotnet format with IDE0005 (Remove unnecessary usings)
    $output = & dotnet format (Join-Path $SourceRoot "TechHub.slnx") `
        --diagnostics IDE0005 `
        --verify-no-changes `
        --verbosity diagnostic 2>&1
    
    # Parse output for files with unused usings
    $filePattern = '^\s*(.+\.cs)\(\d+,\d+\):'
    foreach ($line in $output) {
        if ($line -match $filePattern) {
            $results += [PSCustomObject]@{
                Category = "Unused Using"
                File     = $Matches[1]
                Details  = $line
            }
        }
    }
    
    return $results
}

function Find-UnusedPrivateMembers {
    param([string]$SourceRoot)
    
    $results = @()
    $csFiles = Get-ChildItem -Path (Join-Path $SourceRoot "src") -Filter "*.cs" -Recurse
    
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
    $csFiles = Get-ChildItem -Path (Join-Path $SourceRoot "src") -Filter "*.cs" -Recurse
    
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
    
    # Get all CSS files
    $cssFiles = Get-ChildItem -Path $webRoot -Filter "*.css" -Recurse
    $allCssClasses = @{}
    
    # Extract all CSS class definitions
    foreach ($cssFile in $cssFiles) {
        $content = Get-Content $cssFile.FullName -Raw
        
        # Match class selectors (e.g., .class-name, .class-name:hover, etc.)
        $classPattern = '\.([a-zA-Z0-9_-]+)(?:[:\[,\s>+~]|$)'
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
    
    # Get all Razor and HTML files to check usage
    $razorFiles = Get-ChildItem -Path $webRoot -Include "*.razor", "*.cshtml", "*.html" -Recurse
    
    foreach ($className in $allCssClasses.Keys) {
        foreach ($razorFile in $razorFiles) {
            $content = Get-Content $razorFile.FullName -Raw
            
            # Check if class is used in class="..." or class='...'
            if ($content -match "class\s*=\s*[`"']([^`"']*\b$([regex]::Escape($className))\b[^`"']*)[`"']") {
                $allCssClasses[$className].Used = $true
                break
            }
        }
    }
    
    # Report unused classes
    foreach ($className in $allCssClasses.Keys) {
        if (-not $allCssClasses[$className].Used) {
            $results += [PSCustomObject]@{
                Category   = "Unused CSS Class"
                Class      = $className
                DefinedIn  = $allCssClasses[$className].DefinedIn
                Confidence = "Medium"
                Note       = "Class not found in Razor/HTML files - may be added dynamically via JavaScript"
            }
        }
    }
    
    return $results
}

#endregion

#region Main Execution

$root = Get-SourceRoot
Write-Host "Scanning for dead code in: $root" -ForegroundColor Cyan

$allResults = @{
    UnusedUsings  = @()
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

# Find unused usings
if ($Category -in @('All', 'Usings')) {
    Write-Host "`nChecking for unused usings..." -ForegroundColor Yellow
    try {
        $allResults.UnusedUsings = Find-UnusedUsings -SourceRoot $root
        Write-Host "  Found $($allResults.UnusedUsings.Count) potential unused usings" -ForegroundColor $(if ($allResults.UnusedUsings.Count -gt 0) { "Yellow" } else { "Green" })
    }
    catch {
        Write-Host "  Could not check usings: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

# Find unused private members
if ($Category -in @('All', 'Methods', 'Properties', 'Fields')) {
    Write-Host "Checking for unused private members..." -ForegroundColor Yellow
    $allResults.UnusedMembers = Find-UnusedPrivateMembers -SourceRoot $root
    Write-Host "  Found $($allResults.UnusedMembers.Count) potentially unused members" -ForegroundColor $(if ($allResults.UnusedMembers.Count -gt 0) { "Yellow" } else { "Green" })
}

# Find commented code
if ($Category -eq 'All') {
    Write-Host "Checking for commented code..." -ForegroundColor Yellow
    $allResults.CommentedCode = Find-CommentedCode -SourceRoot $root
    Write-Host "  Found $($allResults.CommentedCode.Count) potential commented code blocks" -ForegroundColor $(if ($allResults.CommentedCode.Count -gt 0) { "Yellow" } else { "Green" })
}

# Find unused CSS classes
if ($Category -eq 'All') {
    Write-Host "Checking for unused CSS classes..." -ForegroundColor Yellow
    $allResults.UnusedCss = Find-UnusedCssClasses -SourceRoot $root
    Write-Host "  Found $($allResults.UnusedCss.Count) potentially unused CSS classes" -ForegroundColor $(if ($allResults.UnusedCss.Count -gt 0) { "Yellow" } else { "Green" })
}

# Calculate summary
$allIssues = @($allResults.UnusedUsings) + @($allResults.UnusedMembers) + @($allResults.CommentedCode) + @($allResults.UnusedCss)
$allResults.Summary.TotalIssues = $allIssues.Count
$allResults.Summary.HighConfidence = ($allIssues | Where-Object { $_.Confidence -eq "High" }).Count
$allResults.Summary.MediumConfidence = ($allIssues | Where-Object { $_.Confidence -eq "Medium" }).Count
$allResults.Summary.LowConfidence = ($allIssues | Where-Object { $_.Confidence -eq "Low" }).Count

#endregion

#region Output

switch ($OutputFormat) {
    'Console' {
        Write-Host "`n===== Dead Code Detection Results =====" -ForegroundColor Cyan
        
        if ($allResults.UnusedUsings.Count -gt 0) {
            Write-Host "`nUnused Usings:" -ForegroundColor Yellow
            $allResults.UnusedUsings | Group-Object File | ForEach-Object {
                Write-Host "  📁 $($_.Name): $($_.Count) unused using(s)" -ForegroundColor Yellow
            }
        }
        
        if ($allResults.UnusedMembers.Count -gt 0) {
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
        
        if ($allResults.CommentedCode.Count -gt 0) {
            Write-Host "`nCommented Code:" -ForegroundColor Yellow
            $allResults.CommentedCode | Group-Object File | ForEach-Object {
                Write-Host "  📁 $($_.Name): $($_.Count) instance(s)" -ForegroundColor Yellow
            }
        }
        
        if ($allResults.UnusedCss.Count -gt 0) {
            Write-Host "`nPotentially Unused CSS Classes:" -ForegroundColor Yellow
            $allResults.UnusedCss | Group-Object DefinedIn | ForEach-Object {
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
        
        if ($allResults.UnusedMembers.Count -gt 0) {
            $output += "`n### Potentially Unused Members`n`n"
            $output += "| Confidence | File | Member | Category |`n"
            $output += "|------------|------|--------|----------|`n"
            foreach ($member in $allResults.UnusedMembers) {
                $emoji = switch ($member.Confidence) { "High" { "🔴" } "Medium" { "🟡" } "Low" { "🟢" } }
                $output += "| $emoji $($member.Confidence) | ``$($member.File)`` | ``$($member.Member)`` | $($member.Category) |`n"
            }
        }
        
        if ($allResults.CommentedCode.Count -gt 0) {
            $output += "`n### Commented Code`n`n"
            $grouped = $allResults.CommentedCode | Group-Object File
            foreach ($group in $grouped) {
                $output += "- **$($group.Name)**: $($group.Count) instance(s)`n"
            }
        }
        
        if ($allResults.UnusedCss.Count -gt 0) {
            $output += "`n### Potentially Unused CSS Classes`n`n"
            $grouped = $allResults.UnusedCss | Group-Object DefinedIn
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
