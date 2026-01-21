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

function Get-SourceFiles {
    param(
        [string]$Path,
        [string]$Filter
    )
    
    # Get all files, then filter out build artifacts and generated code
    $excludeDirs = @('bin', 'obj', '.tmp', 'node_modules', '.vs', '.vscode')
    
    Get-ChildItem -Path $Path -Filter $Filter -Recurse | Where-Object {
        $file = $_
        $shouldExclude = $false
        
        # Check if file is in any excluded directory
        foreach ($excludeDir in $excludeDirs) {
            if ($file.FullName -match "[\\/]$excludeDir[\\/]") {
                $shouldExclude = $true
                break
            }
        }
        
        -not $shouldExclude
    }
}

# Find-UnusedUsings function removed - unused usings are already handled by dotnet format in Step 2

function Find-UnusedPrivateMembers {
    param([string]$SourceRoot)
    
    $results = @()
    $csFiles = @(Get-SourceFiles -Path (Join-Path $SourceRoot "src") -Filter "*.cs")
    
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName -Raw
        
        # Skip Minimal API endpoint files - methods are registered via MapGet/MapPost/etc
        if ($file.FullName -match '[\\/]Endpoints[\\/].*\.cs$') {
            continue
        }
        
        # Skip Blazor code-behind files - methods bound via @onclick, @onchange, etc
        if ($file.FullName -match '\.razor\.cs$') {
            continue
        }
        
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

function Find-EmptyClassesAndInterfaces {
    param([string]$SourceRoot)
    
    $results = @()
    $csFiles = @(Get-SourceFiles -Path (Join-Path $SourceRoot "src") -Filter "*.cs")
    
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName -Raw
        
        # Find empty classes (class with only opening/closing braces)
        $emptyClassPattern = '(?:public|internal|private|protected)\s+(?:sealed\s+|static\s+|abstract\s+)?class\s+(\w+)[^{]*\{\s*\}'
        $classMatches = [regex]::Matches($content, $emptyClassPattern)
        
        foreach ($match in $classMatches) {
            $className = $match.Groups[1].Value
            $results += [PSCustomObject]@{
                Category   = "Empty Class"
                File       = $file.Name
                FullPath   = $file.FullName
                Member     = $className
                Confidence = "High"
                Note       = "Class has no members"
            }
        }
        
        # Find empty interfaces
        $emptyInterfacePattern = '(?:public|internal)\s+interface\s+(I\w+)[^{]*\{\s*\}'
        $interfaceMatches = [regex]::Matches($content, $emptyInterfacePattern)
        
        foreach ($match in $interfaceMatches) {
            $interfaceName = $match.Groups[1].Value
            $results += [PSCustomObject]@{
                Category   = "Empty Interface"
                File       = $file.Name
                FullPath   = $file.FullName
                Member     = $interfaceName
                Confidence = "High"
                Note       = "Interface has no members"
            }
        }
    }
    
    return $results
}

function Find-CommentedCode {
    param([string]$SourceRoot)
    
    $results = @()
    $csFiles = @(Get-SourceFiles -Path (Join-Path $SourceRoot "src") -Filter "*.cs")
    
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
            # Skip XML documentation comments (///)
            if ($line -match '^\s*///') {
                continue
            }
            
            # Skip common explanatory comment patterns
            if ($line -match '^\s*//\s*(Return|For now|Handle|Filter|Check|Verify|Get|Set|Skip|Find|Load|Create|Update|Delete|Add|Remove)') {
                continue
            }
            
            # Only flag actual code patterns (assignments, method calls with semicolons, control flow with braces)
            $codePatterns = @(
                '//\s*(public|private|protected|internal)\s+\w+\s+\w+\s*[({;]',  # Method/property declarations
                '//\s*(var|int|string|bool|void)\s+\w+\s*=.*[;)]',              # Variable assignments with semicolons
                '//\s*\w+\s*\.\s*\w+\s*\([^)]*\)\s*;',                          # Method calls with semicolons
                '//\s*(if|for|foreach|while|switch)\s*\([^)]+\)\s*\{'           # Control flow with opening brace
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

function Find-UnusedParameters {
    param([string]$SourceRoot)
    
    $results = @()
    $csFiles = @(Get-SourceFiles -Path (Join-Path $SourceRoot "src") -Filter "*.cs")
    
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName -Raw
        
        # Skip certain file types with expected unused parameters
        $skipFile = $false
        
        # Minimal API endpoint files often have injected parameters for future use
        if ($file.FullName -match '[\\/]Endpoints[\\/].*\.cs$') {
            $skipFile = $true
        }
        
        # Middleware constructors have standard ASP.NET Core parameters
        if ($file.FullName -match 'Middleware\.cs$') {
            $skipFile = $true
        }
        
        # ContentFixer is a utility tool with many placeholder parameters
        if ($file.FullName -match '[\\/]TechHub\.ContentFixer[\\/]') {
            $skipFile = $true
        }
        
        if ($skipFile) {
            continue
        }
        
        # Find method signatures with parameters
        $methodPattern = '(?:public|private|protected|internal)\s+(?:static\s+)?(?:async\s+)?[\w<>\[\],\s]+\s+(\w+)\s*\(([^)]*)\)\s*\{([^}]+)\}'
        $methodMatches = [regex]::Matches($content, $methodPattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)
        
        foreach ($match in $methodMatches) {
            $methodName = $match.Groups[1].Value
            $parameters = $match.Groups[2].Value
            $methodBody = $match.Groups[3].Value
            
            # Skip empty parameter lists
            if ([string]::IsNullOrWhiteSpace($parameters)) {
                continue
            }
            
            # Parse parameters
            $paramList = $parameters -split ','
            foreach ($param in $paramList) {
                # Extract parameter name (last word before = or end)
                if ($param -match '\s+(\w+)\s*(?:=|$)') {
                    $paramName = $Matches[1]
                    
                    # Skip common parameter names that are typically used
                    if ($paramName -in @('sender', 'e', 'args', 'cancellationToken')) {
                        continue
                    }
                    
                    # Check if parameter is used in method body
                    if ($methodBody -notmatch "\b$([regex]::Escape($paramName))\b") {
                        $results += [PSCustomObject]@{
                            Category   = "Unused Parameter"
                            File       = $file.Name
                            FullPath   = $file.FullName
                            Method     = $methodName
                            Parameter  = $paramName
                            Confidence = "Medium"
                            Note       = "Parameter not referenced in method body"
                        }
                    }
                }
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
        $classMatches = [regex]::Matches($content, $classPattern)
        
        foreach ($match in $classMatches) {
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
    UnusedMembers    = @()
    EmptyClasses     = @()
    CommentedCode    = @()
    UnusedParameters = @()
    UnusedCss        = @()
    Summary          = @{
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

# Find empty classes and interfaces
if ($Category -eq 'All') {
    Write-Host "`nChecking for empty classes and interfaces..." -ForegroundColor Yellow
    try {
        $emptyClasses = @(Find-EmptyClassesAndInterfaces -SourceRoot $root)
        $allResults.EmptyClasses = $emptyClasses
        $count = if ($emptyClasses) { $emptyClasses.Count } else { 0 }
        Write-Host "  Found $count empty classes/interfaces" -ForegroundColor $(if ($count -gt 0) { "Yellow" } else { "Green" })
    }
    catch {
        Write-Warning "  Could not check empty classes: $($_.Exception.Message)"
        Write-Host "  Skipping empty class check" -ForegroundColor Gray
    }
}

# Find commented code
if ($Category -eq 'All') {
    Write-Host "`nChecking for commented code..." -ForegroundColor Yellow
    try {
        $commentedCode = @(Find-CommentedCode -SourceRoot $root)
        $allResults.CommentedCode = $commentedCode
        $count = if ($commentedCode) { $commentedCode.Count } else { 0 }
        Write-Host "  Found $count potential commented code blocks" -ForegroundColor $(if ($count -gt 0) { "Yellow" } else { "Green" })
    }
    catch {
        Write-Warning "  Could not check commented code: $($_.Exception.Message)"
        Write-Host "  Skipping commented code check" -ForegroundColor Gray
    }
}

# Find unused parameters
if ($Category -eq 'All') {
    Write-Host "`nChecking for unused method parameters..." -ForegroundColor Yellow
    try {
        $allResults.UnusedParameters = Find-UnusedParameters -SourceRoot $root
        Write-Host "  Found $($allResults.UnusedParameters.Count) potentially unused parameters" -ForegroundColor $(if ($allResults.UnusedParameters.Count -gt 0) { "Yellow" } else { "Green" })
    }
    catch {
        Write-Warning "  Could not check unused parameters: $($_.Exception.Message)"
        Write-Host "  Skipping unused parameter check" -ForegroundColor Gray
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
if ($allResults.EmptyClasses) { $allIssues += @($allResults.EmptyClasses) }
if ($allResults.CommentedCode) { $allIssues += @($allResults.CommentedCode) }
if ($allResults.UnusedParameters) { $allIssues += @($allResults.UnusedParameters) }
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
        
        $emptyClassesCount = @($allResults.EmptyClasses).Count
        if ($emptyClassesCount -gt 0) {
            Write-Host "`nEmpty Classes/Interfaces:" -ForegroundColor Yellow
            foreach ($item in $allResults.EmptyClasses) {
                Write-Host "  🔴 $($item.File): $($item.Member) ($($item.Category))" -ForegroundColor Yellow
            }
        }
        
        $unusedParametersCount = @($allResults.UnusedParameters).Count
        if ($unusedParametersCount -gt 0) {
            Write-Host "`nUnused Method Parameters:" -ForegroundColor Yellow
            @($allResults.UnusedParameters) | Group-Object File | ForEach-Object {
                Write-Host "  📁 $($_.Name): $($_.Count) unused parameter(s)" -ForegroundColor Yellow
                $_.Group | Select-Object -First 3 | ForEach-Object {
                    Write-Host "    🟡 $($_.Method): parameter '$($_.Parameter)'" -ForegroundColor Yellow
                }
                if ($_.Count -gt 3) {
                    Write-Host "    ...and $($_.Count - 3) more"
                }
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
