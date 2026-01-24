#!/usr/bin/env pwsh

# Analyze markdown linting errors and provide summary
Write-Host "Running markdownlint on all markdown files..." -ForegroundColor Cyan
Write-Host ""

# Run markdownlint and capture output
$output = npx markdownlint-cli2 "**/*.md" 2>&1 | Out-String

# Parse the output to extract errors
$lines = $output -split "`n" | Where-Object { $_ -match "error MD\d+" }

# Group by error type
$errorGroups = $lines | ForEach-Object {
    if ($_ -match "(MD\d+)/([^\s]+)") {
        [PSCustomObject]@{
            Code     = $matches[1]
            Name     = $matches[2]
            FullLine = $_
        }
    }
} | Group-Object Code | Sort-Object Count -Descending

# Display summary
Write-Host "=== Markdown Linting Error Summary ===" -ForegroundColor Yellow
Write-Host ""

if ($errorGroups) {
    $totalErrors = ($errorGroups | Measure-Object -Property Count -Sum).Sum
    
    Write-Host "Total Errors: $totalErrors" -ForegroundColor Red
    Write-Host ""
    
    Write-Host "Errors by Type:" -ForegroundColor Cyan
    foreach ($group in $errorGroups) {
        $code = $group.Name
        $count = $group.Count
        $sample = $group.Group[0]
        
        # Extract rule name from sample
        if ($sample.FullLine -match "$code/([^\s]+)\s+(.+?)\s+\[") {
            $ruleName = $matches[1]
            $description = $matches[2]
        }
        else {
            $ruleName = "unknown"
            $description = "No description"
        }
        
        Write-Host "  $code ($ruleName): $count occurrences" -ForegroundColor White
        Write-Host "    Description: $description" -ForegroundColor Gray
        Write-Host ""
    }
    
    # Show top 5 most common errors with examples
    Write-Host "=== Top 5 Most Common Errors with Examples ===" -ForegroundColor Yellow
    Write-Host ""
    
    $top5 = $errorGroups | Select-Object -First 5
    foreach ($group in $top5) {
        $code = $group.Name
        $count = $group.Count
        $examples = $group.Group | Select-Object -First 3
        
        Write-Host "$code - $count occurrences:" -ForegroundColor Cyan
        foreach ($example in $examples) {
            Write-Host "  $($example.FullLine)" -ForegroundColor Gray
        }
        Write-Host ""
    }
}
else {
    Write-Host "No errors found! 🎉" -ForegroundColor Green
}
