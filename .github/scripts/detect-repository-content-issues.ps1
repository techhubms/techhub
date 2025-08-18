#!/usr/bin/env pwsh

param(
    [switch]$QuickCheck,
    [int]$SimilarityThreshold = 40,
    [string[]]$ExcludeDirectories = @('_videos')
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   FIXED VALIDATION SCRIPT              " -ForegroundColor Cyan  
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Get all markdown files from content directories
$contentDirs = @("_news", "_posts", "_videos", "_community", "_magazines", "_events", "_roundups")

# Filter out excluded directories
if ($ExcludeDirectories.Count -gt 0) {
    $contentDirs = $contentDirs | Where-Object { $_ -notin $ExcludeDirectories }
    Write-Host "Excluding directories: $($ExcludeDirectories -join ', ')" -ForegroundColor Gray
}

$allFiles = @()
$nonMarkdownFiles = @()

foreach ($dir in $contentDirs) {
    if (Test-Path $dir) {
        # Get all files (not just .md)
        $allDirFiles = Get-ChildItem $dir -File -ErrorAction SilentlyContinue
        
        # Separate markdown and non-markdown files
        $mdFiles = $allDirFiles | Where-Object { $_.Extension -eq '.md' }
        $nonMdFiles = $allDirFiles | Where-Object { $_.Extension -ne '.md' }
        
        # Add markdown files to main collection
        $allFiles += $mdFiles | ForEach-Object { 
            @{
                Path      = $_.FullName.Replace((Get-Location).Path + "\", "").Replace("\", "/")
                Directory = $dir
                Name      = $_.Name
                FullPath  = $_.FullName
            }
        }
        
        # Track non-markdown files
        $nonMarkdownFiles += $nonMdFiles | ForEach-Object {
            @{
                Path      = $_.FullName.Replace((Get-Location).Path + "\", "").Replace("\", "/")
                Directory = $dir
                Name      = $_.Name
                Extension = $_.Extension
            }
        }
    }
}

Write-Host "📊 Found $($allFiles.Count) markdown files across $($contentDirs.Count) directories" -ForegroundColor White
if ($nonMarkdownFiles.Count -gt 0) {
    Write-Host "⚠️  Found $($nonMarkdownFiles.Count) non-markdown files in content directories" -ForegroundColor Yellow
}
Write-Host ""

# Helper function to check if two dates are significantly different (ignoring 7-day publishing differences)
function Test-DateSignificantlyDifferent {
    param([string]$date1, [string]$date2)
    
    if ([string]::IsNullOrWhiteSpace($date1) -or [string]::IsNullOrWhiteSpace($date2)) {
        return $false
    }
    
    try {
        $d1 = [DateTime]::Parse($date1)
        $d2 = [DateTime]::Parse($date2)
        
        $daysDifference = [Math]::Abs(($d1 - $d2).TotalDays)
        
        # Ignore differences of 7 days or less (timezone issues and publishing delays)
        return $daysDifference -gt 7
    }
    catch {
        # If we can't parse the dates, consider them different
        return $date1 -ne $date2
    }
}

# Helper function to extract canonical URL from frontmatter
function Get-CanonicalUrl {
    param([string]$content)
    
    if ($content -match 'canonical_url:\s*"([^"]+)"') {
        return $matches[1]
    }
    return $null
}

# Helper function to extract date from URL using various formats
function Get-DateFromUrl {
    param([string]$url)
    
    if ([string]::IsNullOrWhiteSpace($url)) {
        return $null
    }
    
    # Pre-filter to avoid false positives with version numbers
    # Skip patterns that look like version numbers (claude 3.7, version 2.5, etc.)
    if ($url -match '(claude|version|v\d|gpt|model)\s*[\d\.]+' -or 
        $url -match '\d+\.\d+' -and $url -notmatch '\d{4}') {
        # This looks like a version number context, be more restrictive
        # Only match explicit date patterns with years
        if (-not ($url -match '\d{4}[/-]\d{1,2}[/-]\d{1,2}' -or $url -match '\d{1,2}[/-]\d{1,2}[/-]\d{4}')) {
            return $null
        }
    }
    
    # Common date patterns in URLs
    $datePatterns = @(
        # ISO format: 2025/07/28 or 2025-07-28 (most reliable)
        '(\d{4})[/-](\d{1,2})[/-](\d{1,2})',
        # US format: 07/28/2025 or 07-28-2025 (reliable with year)
        '(\d{1,2})[/-](\d{1,2})[/-](\d{4})',
        # Month names with year: july-28-2025, july-28, 28-july-2025
        '(\w+)[/-](\d{1,2})[/-](\d{4})',
        '(\d{1,2})[/-](\w+)[/-](\d{4})',
        # Month names without year (less reliable, only if clear month names)
        '\b(january|february|march|april|may|june|july|august|september|october|november|december)[/-](\d{1,2})\b',
        '\b(\d{1,2})[/-](january|february|march|april|may|june|july|august|september|october|november|december)\b'
    )
    
    foreach ($pattern in $datePatterns) {
        if ($url -match $pattern) {
            try {
                $match = $matches
                
                # Try to parse the matched components
                if ($match.Count -eq 4) {
                    # Three components - try different arrangements
                    $part1, $part2, $part3 = $match[1], $match[2], $match[3]
                    
                    # Check if it's year-month-day
                    if ($part1 -match '^\d{4}$' -and $part2 -match '^\d{1,2}$' -and $part3 -match '^\d{1,2}$') {
                        $year, $month, $day = $part1, $part2, $part3
                    }
                    # Check if it's month-day-year (US format)
                    elseif ($part3 -match '^\d{4}$' -and $part1 -match '^\d{1,2}$' -and $part2 -match '^\d{1,2}$') {
                        $year, $month, $day = $part3, $part1, $part2
                    }
                    # Check if it's day-month-year
                    elseif ($part3 -match '^\d{4}$' -and $part2 -match '^\d{1,2}$' -and $part1 -match '^\d{1,2}$') {
                        $year, $month, $day = $part3, $part2, $part1
                    }
                    # Month name formats
                    elseif ($part1 -match '^(january|february|march|april|may|june|july|august|september|october|november|december|januari|februari|maart|april|mei|juni|juli|augustus|september|oktober|november)$') {
                        $monthNames = @{
                            'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                            'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12;
                            'januari' = 1; 'februari' = 2; 'maart' = 3; 'mei' = 5; 'juni' = 6; 'juli' = 7; 
                            'augustus' = 8; 'oktober' = 10
                        }
                        $monthNum = $monthNames[$part1.ToLower()]
                        if ($monthNum -and $part2 -match '^\d{1,2}$') {
                            $month = $monthNum
                            $day = $part2
                            $year = if ($part3 -match '^\d{4}$') { $part3 } else { (Get-Date).Year }
                        }
                    }
                    elseif ($part2 -match '^(january|february|march|april|may|june|july|august|september|october|november|december|januari|februari|maart|april|mei|juni|juli|augustus|september|oktober|november)$') {
                        $monthNames = @{
                            'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                            'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12;
                            'januari' = 1; 'februari' = 2; 'maart' = 3; 'mei' = 5; 'juni' = 6; 'juli' = 7; 
                            'augustus' = 8; 'oktober' = 10
                        }
                        $monthNum = $monthNames[$part2.ToLower()]
                        if ($monthNum -and $part1 -match '^\d{1,2}$') {
                            $month = $monthNum
                            $day = $part1
                            $year = if ($part3 -match '^\d{4}$') { $part3 } else { (Get-Date).Year }
                        }
                    }
                    
                    # Validate and format date
                    if ($year -and $month -and $day) {
                        $month = [int]$month
                        $day = [int]$day
                        $year = [int]$year
                        
                        if ($month -ge 1 -and $month -le 12 -and $day -ge 1 -and $day -le 31 -and $year -ge 2020 -and $year -le 2030) {
                            return "$year-$($month.ToString('00'))-$($day.ToString('00'))"
                        }
                    }
                }
                elseif ($match.Count -eq 3) {
                    # Two components - only proceed if we have clear month names
                    $part1, $part2 = $match[1], $match[2]
                    $year = (Get-Date).Year
                    
                    # Handle month name formats (more restrictive)
                    if ($part1 -match '^(january|february|march|april|may|june|july|august|september|october|november|december)$') {
                        $monthNames = @{
                            'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                            'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12
                        }
                        $monthNum = $monthNames[$part1.ToLower()]
                        if ($monthNum -and $part2 -match '^\d{1,2}$') {
                            $month = $monthNum
                            $day = [int]$part2
                            if ($day -ge 1 -and $day -le 31) {
                                return "$year-$($month.ToString('00'))-$($day.ToString('00'))"
                            }
                        }
                    }
                    elseif ($part2 -match '^(january|february|march|april|may|june|july|august|september|october|november|december)$') {
                        $monthNames = @{
                            'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                            'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12
                        }
                        $monthNum = $monthNames[$part2.ToLower()]
                        if ($monthNum -and $part1 -match '^\d{1,2}$') {
                            $month = $monthNum
                            $day = [int]$part1
                            if ($day -ge 1 -and $day -le 31) {
                                return "$year-$($month.ToString('00'))-$($day.ToString('00'))"
                            }
                        }
                    }
                    # Skip numeric-only patterns without year to avoid false positives
                }
            }
            catch {
                # Continue to next pattern
            }
        }
    }
    
    return $null
}

# Helper function to extract date from HTML content
function Get-DateFromHtml {
    param([string]$html, [string]$url)
    
    if ([string]::IsNullOrWhiteSpace($html)) {
        return $null
    }
    
    # Special handling for Reddit URLs using JSON API
    if ($url -and $url -match 'reddit\.com') {
        try {
            # Convert to JSON API URL
            $jsonUrl = $url.TrimEnd('/') + '.json'
            
            $headers = @{
                'User-Agent' = 'Mozilla/5.0 (compatible; PowerShell script)'
            }
            
            $response = Invoke-WebRequest -Uri $jsonUrl -Headers $headers -ErrorAction Stop -TimeoutSec 10
            $jsonData = $response.Content | ConvertFrom-Json
            
            # Extract post data
            $postData = $jsonData[0].data.children[0].data
            
            # Check if post is removed
            $isRemoved = $false
            if ($postData.removed -eq $true -or 
                ($postData.removed_by -and $null -ne $postData.removed_by -and $postData.removed_by -ne "") -or
                ($postData.banned_by -and $null -ne $postData.banned_by -and $postData.banned_by -ne "") -or
                $postData.author -eq "[deleted]" -or
                $postData.selftext -eq "[removed]" -or
                $postData.spam -eq $true) {
                $isRemoved = $true
            }
            
            # Return null date for removed posts to trigger removal detection
            if ($isRemoved) {
                return $null
            }
            
            # Convert Unix timestamp to date
            if ($postData.created_utc) {
                $unixTime = [int64]$postData.created_utc
                $dateTime = [DateTime]::UnixEpoch.AddSeconds($unixTime)
                if ($dateTime.Year -ge 2020 -and $dateTime.Year -le 2030) {
                    return $dateTime.ToString('yyyy-MM-dd')
                }
            }
        }
        catch {
            # Fall back to HTML parsing if JSON API fails
            Write-Verbose "Reddit JSON API failed for $url, falling back to HTML parsing: $($_.ToString())"
        }
    }
    
    # Look for various date patterns in HTML
    $datePatterns = @(
        # ISO format dates
        '(\d{4}-\d{2}-\d{2})',
        # Date attributes and meta tags
        'datetime="([^"]*\d{4}-\d{2}-\d{2}[^"]*)"',
        'content="([^"]*\d{4}-\d{2}-\d{2}[^"]*)"',
        # Common date formats in content
        '(\w+\s+\d{1,2},?\s+\d{4})',  # July 28, 2025
        '(\d{1,2}\s+\w+\s+\d{4})',    # 28 July 2025
        '(\d{1,2}/\d{1,2}/\d{4})',    # 7/28/2025
        '(\d{4}/\d{1,2}/\d{1,2})'     # 2025/7/28
    )
    
    foreach ($pattern in $datePatterns) {
        if ($html -match $pattern) {
            $dateStr = $matches[1]
            try {
                # Try to parse the date string
                $parsedDate = [DateTime]::Parse($dateStr)
                if ($parsedDate.Year -ge 2020 -and $parsedDate.Year -le 2030) {
                    return $parsedDate.ToString('yyyy-MM-dd')
                }
            }
            catch {
                # Try alternative parsing for month names
                try {
                    $monthNames = @{
                        'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                        'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12;
                        'jan' = 1; 'feb' = 2; 'mar' = 3; 'apr' = 4; 'jun' = 6;
                        'jul' = 7; 'aug' = 8; 'sep' = 9; 'oct' = 10; 'nov' = 11; 'dec' = 12
                    }
                    
                    if ($dateStr -match '(\w+)\s+(\d{1,2}),?\s+(\d{4})') {
                        $monthName = $matches[1].ToLower()
                        $day = [int]$matches[2]
                        $year = [int]$matches[3]
                        
                        $monthNum = $monthNames[$monthName]
                        if ($monthNum -and $day -ge 1 -and $day -le 31 -and $year -ge 2020 -and $year -le 2030) {
                            return "$year-$($monthNum.ToString('00'))-$($day.ToString('00'))"
                        }
                    }
                }
                catch {
                    # Continue to next pattern
                }
            }
        }
    }
    
    return $null
}

# Helper function to extract content body (remove frontmatter)
function Get-ContentBody {
    param([string]$content)
    
    if ($content -match '^---[\s\S]*?---\s*(.*)$') {
        return $matches[1].Trim()
    }
    return $content
}

# Helper function to calculate Jaccard similarity
function Get-JaccardSimilarity {
    param(
        [string[]]$words1,
        [string[]]$words2
    )
    
    if ($words1.Count -eq 0 -and $words2.Count -eq 0) {
        return 100
    }
    
    if ($words1.Count -eq 0 -or $words2.Count -eq 0) {
        return 0
    }
    
    $set1 = $words1 | Sort-Object -Unique
    $set2 = $words2 | Sort-Object -Unique
    
    $intersection = Compare-Object $set1 $set2 -IncludeEqual -ExcludeDifferent | Measure-Object | Select-Object -ExpandProperty Count
    $union = ($set1 + $set2 | Sort-Object -Unique | Measure-Object).Count
    
    if ($union -eq 0) {
        return 0
    }
    
    return [math]::Round(($intersection / $union) * 100, 1)
}

# Helper function to get normalized words from text
function Get-NormalizedWords {
    param([string]$text)
    
    if ([string]::IsNullOrWhiteSpace($text)) {
        return @()
    }
    
    # Remove special characters and split into words
    $cleanText = $text -replace '[^\w\s-]', ' '
    $words = $cleanText -split '\s+' | Where-Object { $_.Length -gt 2 } | ForEach-Object { $_.ToLower().Trim() }
    
    return $words | Where-Object { $_ -ne '' }
}

Write-Host ""

# 0. CHECK NON-MARKDOWN FILES
if ($nonMarkdownFiles.Count -gt 0) {
    Write-Host "🔴 CHECKING NON-MARKDOWN FILES..." -ForegroundColor Red
    Write-Host "❌ Found $($nonMarkdownFiles.Count) non-markdown files in content directories:" -ForegroundColor Red
    
    $groupedByDir = $nonMarkdownFiles | Group-Object -Property Directory
    foreach ($group in $groupedByDir) {
        Write-Host "   📁 $($group.Name):" -ForegroundColor Yellow
        foreach ($file in $group.Group) {
            $extensionInfo = if ([string]::IsNullOrEmpty($file.Extension)) { "(no extension)" } else { $file.Extension }
            Write-Host "      📄 $($file.Name) $extensionInfo" -ForegroundColor White
        }
    }
    Write-Host ""
}

# 1. CHECK DATE CONSISTENCY
Write-Host "🔴 CHECKING DATE CONSISTENCY..." -ForegroundColor Red
$dateMismatches = @()

foreach ($file in $allFiles) {
    if ($file.Path -match "(\d{4}-\d{2}-\d{2})") {
        $filenameDate = $matches[1]
        
        try {
            # Read the frontmatter section to find date and permalink
            $content = Get-Content $file.Path -Raw -ErrorAction Stop
            
            # Extract frontmatter date
            $frontmatterDate = $null
            if ($content -match 'date:\s*(\d{4}-\d{2}-\d{2})') {
                $frontmatterDate = $matches[1]
            }
            
            # Extract permalink date
            $permalinkDate = $null
            if ($content -match 'permalink:\s*["\s]*/?(\d{4}-\d{2}-\d{2})') {
                $permalinkDate = $matches[1]
            }
            
            # Check for mismatches
            $issues = @()
            
            if ($frontmatterDate -and $filenameDate -ne $frontmatterDate) {
                $issues += "Filename ($filenameDate) ≠ Frontmatter ($frontmatterDate)"
            }
            
            if ($permalinkDate -and $filenameDate -ne $permalinkDate) {
                $issues += "Filename ($filenameDate) ≠ Permalink ($permalinkDate)"
            }
            
            if ($frontmatterDate -and $permalinkDate -and $frontmatterDate -ne $permalinkDate) {
                $issues += "Frontmatter ($frontmatterDate) ≠ Permalink ($permalinkDate)"
            }
            
            if ($issues.Count -gt 0) {
                $dateMismatches += @{
                    File            = $file.Path
                    FilenameDate    = $filenameDate
                    FrontmatterDate = $frontmatterDate
                    PermalinkDate   = $permalinkDate
                    Issues          = $issues
                }
            }
        }
        catch {
            Write-Host "   ⚠️  Could not read file: $($file.Path) - $($_.ToString())" -ForegroundColor Yellow
        }
    }
}

if ($dateMismatches.Count -gt 0) {
    Write-Host "❌ Found $($dateMismatches.Count) date mismatches:" -ForegroundColor Red
    foreach ($mismatch in $dateMismatches) {
        Write-Host "   📁 $($mismatch.File)" -ForegroundColor Yellow
        Write-Host "      Filename: $($mismatch.FilenameDate) | Frontmatter: $($mismatch.FrontmatterDate) | Permalink: $($mismatch.PermalinkDate)" -ForegroundColor White
        foreach ($issue in $mismatch.Issues) {
            Write-Host "      Issue: $issue" -ForegroundColor Red
        }
    }
}
else {
    Write-Host "✅ All dates are consistent!" -ForegroundColor Green
}
Write-Host ""

# 2. CHECK PERMALINK CONSISTENCY
Write-Host "🔴 CHECKING PERMALINK CONSISTENCY..." -ForegroundColor Red
$permalinkMismatches = @()

foreach ($file in $allFiles) {
    if ($file.Name -match "^(\d{4}-\d{2}-\d{2}-.+)\.md$") {
        $expectedPermalink = "/$($matches[1]).html"
        
        try {
            # Read only the first 30 lines to find permalink
            $lines = Get-Content $file.Path -TotalCount 30 -ErrorAction Stop
            $permalinkLine = ($lines | Select-String 'permalink:\s*"([^"]+)"' | Select-Object -First 1)
            
            if ($permalinkLine) {
                $actualPermalink = $permalinkLine.Matches[0].Groups[1].Value
                
                if ($expectedPermalink -ne $actualPermalink) {
                    $permalinkMismatches += @{
                        File     = $file.Path
                        Expected = $expectedPermalink
                        Actual   = $actualPermalink
                    }
                }
            }
        }
        catch {
            Write-Host "   ⚠️  Could not read file: $($file.Path) - $($_.ToString())" -ForegroundColor Yellow
        }
    }
}

if ($permalinkMismatches.Count -gt 0) {
    Write-Host "❌ Found $($permalinkMismatches.Count) permalink mismatches:" -ForegroundColor Red
    foreach ($mismatch in $permalinkMismatches) {
        Write-Host "   📁 $($mismatch.File)" -ForegroundColor Yellow
        Write-Host "      Expected: $($mismatch.Expected)" -ForegroundColor White
        Write-Host "      Actual:   $($mismatch.Actual)" -ForegroundColor Gray
    }
}
else {
    Write-Host "✅ All permalinks are consistent!" -ForegroundColor Green
}
Write-Host ""

# 3. CHECK CANONICAL URL VALIDITY
Write-Host "🔴 CHECKING CANONICAL URL VALIDITY..." -ForegroundColor Red
$urlIssues = @()
$urlDateMismatches = @()
$removedPosts = @()

# Create script block for parallel URL testing
$urlTestScriptBlock = {
    param($file, $canonicalUrl, $content)
    
    $result = @{
        File            = $file.Path
        Url             = $canonicalUrl
        Success         = $false
        Error           = $null
        HtmlDate        = $null
        UrlDate         = $null
        FrontmatterDate = $null
        IsRemoved       = $false
    }
    
    try {
        # Extract frontmatter date for comparison
        if ($content -match 'date:\s*(\d{4}-\d{2}-\d{2})') {
            $result.FrontmatterDate = $matches[1]
        }
        
        # Test URL accessibility
        $response = Invoke-WebRequest -Uri $canonicalUrl -TimeoutSec 15 -UseBasicParsing -ErrorAction Stop
        $result.Success = $true
        
        # Check for removed posts in community content
        if ($file.Directory -eq '_community' -and $response.Content -match 'Sorry, this post has been removed') {
            $result.IsRemoved = $true
        }
        
        # Extract date from content (prefer JSON API for Reddit)
        if ($canonicalUrl -and $canonicalUrl -match 'reddit\.com') {
            try {
                # Try JSON API first for Reddit
                $jsonUrl = $canonicalUrl.TrimEnd('/') + '.json'
                $headers = @{
                    'User-Agent' = 'Mozilla/5.0 (compatible; PowerShell script)'
                }
                
                $jsonResponse = Invoke-WebRequest -Uri $jsonUrl -Headers $headers -ErrorAction Stop -TimeoutSec 10
                $jsonData = $jsonResponse.Content | ConvertFrom-Json
                
                # Extract post data
                $postData = $jsonData[0].data.children[0].data
                
                # Check if post is removed
                if ($postData.removed -eq $true -or 
                    ($postData.removed_by -and $null -ne $postData.removed_by -and $postData.removed_by -ne "") -or
                    ($postData.banned_by -and $null -ne $postData.banned_by -and $postData.banned_by -ne "") -or
                    $postData.author -eq "[deleted]" -or
                    $postData.selftext -eq "[removed]" -or
                    $postData.spam -eq $true) {
                    $result.IsRemoved = $true
                }
                
                # Convert Unix timestamp to date if not removed
                if (-not $result.IsRemoved -and $postData.created_utc) {
                    $unixTime = [int64]$postData.created_utc
                    $dateTime = [DateTime]::UnixEpoch.AddSeconds($unixTime)
                    if ($dateTime.Year -ge 2020 -and $dateTime.Year -le 2030) {
                        $result.HtmlDate = $dateTime.ToString('yyyy-MM-dd')
                    }
                }
            }
            catch {
                # Fall back to HTML parsing if JSON API fails
                Write-Verbose "Reddit JSON API failed for $canonicalUrl, falling back to HTML parsing: $($_.ToString())"
                
                # HTML fallback - try new Reddit timestamp format first
                if ($response.Content -match 'created-timestamp="([^"]*)"') {
                    try {
                        $timestampStr = $matches[1]
                        $redditDate = [DateTime]::Parse($timestampStr)
                        if ($redditDate.Year -ge 2020 -and $redditDate.Year -le 2030) {
                            $result.HtmlDate = $redditDate.ToString('yyyy-MM-dd')
                        }
                    }
                    catch {
                        # Fall through to old format
                    }
                }
                
                # Try old Reddit timestamp format if new format not found
                if (-not $result.HtmlDate -and $response.Content -match '<faceplate-timeago[^>]+ts="(\d+)"') {
                    try {
                        $timestamp = [long]$matches[1]
                        $unixEpoch = [DateTime]::new(1970, 1, 1, 0, 0, 0, [DateTimeKind]::Utc)
                        $redditDate = $unixEpoch.AddSeconds($timestamp)
                        if ($redditDate.Year -ge 2020 -and $redditDate.Year -le 2030) {
                            $result.HtmlDate = $redditDate.ToString('yyyy-MM-dd')
                        }
                    }
                    catch {
                        # Continue to regular HTML parsing
                    }
                }
            }
        }
        
        # If no Reddit date found, try intelligent HTML parsing
        if (-not $result.HtmlDate) {
            # Step 1: Look for date-related keywords and extract surrounding context
            $dateKeywords = @('published', 'created', 'date', 'posted', 'time')
            $contextSegments = @()
            
            # Check if HTML is essentially one line (no meaningful line breaks)
            $htmlLines = $response.Content -split "`n"
            $isOneLine = ($htmlLines.Count -le 3) -or ($htmlLines | Where-Object { $_.Trim().Length -gt 100 }).Count -le 1
            
            if ($isOneLine) {
                # For single-line HTML, extract context around date keywords
                foreach ($keyword in $dateKeywords) {
                    if ($response.Content -match "(.{0,200}$keyword.{0,200})" -and $matches[1]) {
                        $contextSegments += $matches[1]
                    }
                }
                # If no keywords found, fall back to entire content
                if ($contextSegments.Count -eq 0) {
                    $contextSegments += $response.Content
                }
            }
            else {
                # For multi-line HTML, find lines containing date keywords and surrounding lines
                for ($i = 0; $i -lt $htmlLines.Count; $i++) {
                    $line = $htmlLines[$i]
                    foreach ($keyword in $dateKeywords) {
                        if ($line -match $keyword) {
                            # Extract this line plus 2 lines before and after for context
                            $startIndex = [Math]::Max(0, $i - 2)
                            $endIndex = [Math]::Min($htmlLines.Count - 1, $i + 2)
                            $contextLines = $htmlLines[$startIndex..$endIndex] -join " "
                            $contextSegments += $contextLines
                            break  # Found keyword in this line, move to next line
                        }
                    }
                }
                # If no keywords found, fall back to entire content
                if ($contextSegments.Count -eq 0) {
                    $contextSegments += $response.Content
                }
            }
            
            # Step 2: Look for date patterns in the extracted context segments
            $datePatterns = @(
                # High priority: structured date attributes (most reliable)
                'datetime="([^"]*(\d{4}-\d{2}-\d{2})[^"]*)"',
                'content="([^"]*(\d{4}-\d{2}-\d{2})[^"]*)"',
                'value="([^"]*(\d{4}-\d{2}-\d{2})[^"]*)"',
                
                # Medium priority: ISO format dates
                '(\d{4}-\d{2}-\d{2})',
                
                # Medium priority: structured formats with separators
                '(\d{4}/\d{1,2}/\d{1,2})',
                '(\d{1,2}/\d{1,2}/\d{4})',
                
                # Lower priority: natural language dates
                '(\w+\s+\d{1,2},?\s+\d{4})',  # July 28, 2025
                '(\d{1,2}\s+\w+\s+\d{4})'     # 28 July 2025
            )
            
            foreach ($context in $contextSegments) {
                foreach ($pattern in $datePatterns) {
                    if ($context -match $pattern) {
                        $dateStr = $matches[1]
                        
                        # For structured attributes, extract the actual date part
                        if ($matches.Count -gt 2 -and $matches[2]) {
                            $dateStr = $matches[2]  # Use the captured date part from structured attributes
                        }
                        
                        try {
                            $parsedDate = [DateTime]::Parse($dateStr)
                            if ($parsedDate.Year -ge 2020 -and $parsedDate.Year -le 2030) {
                                $result.HtmlDate = $parsedDate.ToString('yyyy-MM-dd')
                                break
                            }
                        }
                        catch {
                            # Try alternative parsing for month names
                            try {
                                $monthNames = @{
                                    'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                                    'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12;
                                    'jan' = 1; 'feb' = 2; 'mar' = 3; 'apr' = 4; 'jun' = 6;
                                    'jul' = 7; 'aug' = 8; 'sep' = 9; 'oct' = 10; 'nov' = 11; 'dec' = 12
                                }
                                
                                if ($dateStr -match '(\w+)\s+(\d{1,2}),?\s+(\d{4})') {
                                    $monthName = $matches[1].ToLower()
                                    $day = [int]$matches[2]
                                    $year = [int]$matches[3]
                                    
                                    $monthNum = $monthNames[$monthName]
                                    if ($monthNum -and $day -ge 1 -and $day -le 31 -and $year -ge 2020 -and $year -le 2030) {
                                        $result.HtmlDate = "$year-$($monthNum.ToString('00'))-$($day.ToString('00'))"
                                        break
                                    }
                                }
                            }
                            catch {
                                # Continue to next pattern
                            }
                        }
                    }
                }
                
                # If we found a date, break out of context loop
                if ($result.HtmlDate) {
                    break
                }
            }
        }
        
        # Extract date from URL itself
        # Pre-filter to avoid false positives with version numbers
        if (-not ($canonicalUrl -match '(claude|version|v\d|gpt|model)\s*[\d\.]+' -or 
                ($canonicalUrl -match '\d+\.\d+' -and $canonicalUrl -notmatch '\d{4}'))) {
            
            $datePatterns = @(
                '(\d{4})[/-](\d{1,2})[/-](\d{1,2})',
                '(\d{1,2})[/-](\d{1,2})[/-](\d{4})',
                '(\w+)[/-](\d{1,2})[/-](\d{4})',
                '(\d{1,2})[/-](\w+)[/-](\d{4})',
                '\b(january|february|march|april|may|june|july|august|september|october|november|december)[/-](\d{1,2})\b',
                '\b(\d{1,2})[/-](january|february|march|april|may|june|july|august|september|october|november|december)\b'
            )
            
            foreach ($pattern in $datePatterns) {
                if ($canonicalUrl -match $pattern) {
                    try {
                        $match = $matches
                        $year = $null; $month = $null; $day = $null
                        
                        if ($match.Count -eq 4) {
                            $part1, $part2, $part3 = $match[1], $match[2], $match[3]
                            
                            if ($part1 -match '^\d{4}$' -and $part2 -match '^\d{1,2}$' -and $part3 -match '^\d{1,2}$') {
                                $year, $month, $day = $part1, $part2, $part3
                            }
                            elseif ($part3 -match '^\d{4}$' -and $part1 -match '^\d{1,2}$' -and $part2 -match '^\d{1,2}$') {
                                $year, $month, $day = $part3, $part1, $part2
                            }
                            elseif ($part3 -match '^\d{4}$' -and $part2 -match '^\d{1,2}$' -and $part1 -match '^\d{1,2}$') {
                                $year, $month, $day = $part3, $part2, $part1
                            }
                            elseif ($part1 -match '^(january|february|march|april|may|june|july|august|september|october|november|december)$') {
                                $monthNames = @{
                                    'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                                    'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12
                                }
                                $monthNum = $monthNames[$part1.ToLower()]
                                if ($monthNum -and $part2 -match '^\d{1,2}$') {
                                    $month = $monthNum
                                    $day = $part2
                                    $year = if ($part3 -match '^\d{4}$') { $part3 } else { (Get-Date).Year }
                                }
                            }
                            elseif ($part2 -match '^(january|february|march|april|may|june|july|august|september|october|november|december)$') {
                                $monthNames = @{
                                    'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                                    'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12
                                }
                                $monthNum = $monthNames[$part2.ToLower()]
                                if ($monthNum -and $part1 -match '^\d{1,2}$') {
                                    $month = $monthNum
                                    $day = $part1
                                    $year = if ($part3 -match '^\d{4}$') { $part3 } else { (Get-Date).Year }
                                }
                            }
                        }
                        elseif ($match.Count -eq 3) {
                            $part1, $part2 = $match[1], $match[2]
                            $year = (Get-Date).Year
                            
                            if ($part1 -match '^(january|february|march|april|may|june|july|august|september|october|november|december)$') {
                                $monthNames = @{
                                    'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                                    'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12
                                }
                                $monthNum = $monthNames[$part1.ToLower()]
                                if ($monthNum -and $part2 -match '^\d{1,2}$') {
                                    $month = $monthNum
                                    $day = [int]$part2
                                }
                            }
                            elseif ($part2 -match '^(january|february|march|april|may|june|july|august|september|october|november|december)$') {
                                $monthNames = @{
                                    'january' = 1; 'february' = 2; 'march' = 3; 'april' = 4; 'may' = 5; 'june' = 6;
                                    'july' = 7; 'august' = 8; 'september' = 9; 'october' = 10; 'november' = 11; 'december' = 12
                                }
                                $monthNum = $monthNames[$part2.ToLower()]
                                if ($monthNum -and $part1 -match '^\d{1,2}$') {
                                    $month = $monthNum
                                    $day = [int]$part1
                                }
                            }
                        }
                        
                        if ($year -and $month -and $day) {
                            $month = [int]$month
                            $day = [int]$day
                            $year = [int]$year
                            
                            if ($month -ge 1 -and $month -le 12 -and $day -ge 1 -and $day -le 31 -and $year -ge 2020 -and $year -le 2030) {
                                $result.UrlDate = "$year-$($month.ToString('00'))-$($day.ToString('00'))"
                                break
                            }
                        }
                    }
                    catch {
                        # Continue to next pattern
                    }
                }
            }
        }
        
    }
    catch {
        $result.Error = $_.ToString()
    }
    
    return $result
}

# Collect files with canonical URLs for parallel processing
$urlTestJobs = @()
$filesWithUrls = @()

foreach ($file in $allFiles) {
    try {
        $content = Get-Content $file.Path -Raw -ErrorAction Stop
        $canonicalUrl = Get-CanonicalUrl $content
        
        if ($canonicalUrl) {
            $filesWithUrls += @{
                File    = $file
                Url     = $canonicalUrl
                Content = $content
            }
        }
    }
    catch {
        Write-Host "   ⚠️  Could not read file: $($file.Path) - $($_.ToString())" -ForegroundColor Yellow
    }
}

Write-Host "   🔍 Testing $($filesWithUrls.Count) URLs in parallel (batch size: 10)..." -ForegroundColor Gray

# Process URLs in parallel batches
$batchSize = 10
$completedCount = 0
$results = @()

# for ($i = 0; $i -lt $filesWithUrls.Count; $i += $batchSize) {
#     $batch = $filesWithUrls[$i..([Math]::Min($i + $batchSize - 1, $filesWithUrls.Count - 1))]
    
#     # Start parallel jobs for this batch
#     $jobs = @()
#     foreach ($item in $batch) {
#         $job = Start-Job -ScriptBlock $urlTestScriptBlock -ArgumentList $item.File, $item.Url, $item.Content
#         $jobs += $job
#     }
    
#     # Wait for all jobs in this batch to complete
#     $batchResults = Receive-Job -Job $jobs -Wait
#     $results += $batchResults
    
#     # Clean up jobs
#     Remove-Job -Job $jobs -Force
    
#     $completedCount += $batch.Count
#     Write-Host "   Progress: $completedCount/$($filesWithUrls.Count) URLs tested..." -ForegroundColor Gray
# }

# Process results
# foreach ($result in $results) {
#     if (-not $result.Success) {
#         $urlIssues += @{
#             File  = $result.File
#             Url   = $result.Url
#             Error = $result.Error
#         }
#     }
    
#     if ($result.IsRemoved) {
#         $removedPosts += @{
#             File   = $result.File
#             Url    = $result.Url
#             Status = 'Post removed'
#         }
#     }
    
#     # Check date mismatches with timezone tolerance
#     if ($result.HtmlDate -and $result.FrontmatterDate) {
#         if (Test-DateSignificantlyDifferent $result.HtmlDate $result.FrontmatterDate) {
#             $urlDateMismatches += @{
#                 File            = $result.File
#                 Url             = $result.Url
#                 HtmlDate        = $result.HtmlDate
#                 FrontmatterDate = $result.FrontmatterDate
#                 Source          = 'HTML'
#             }
#         }
#     }
    
#     if ($result.UrlDate -and $result.FrontmatterDate) {
#         if (Test-DateSignificantlyDifferent $result.UrlDate $result.FrontmatterDate) {
#             $urlDateMismatches += @{
#                 File            = $result.File
#                 Url             = $result.Url
#                 UrlDate         = $result.UrlDate
#                 FrontmatterDate = $result.FrontmatterDate
#                 Source          = 'URL'
#             }
#         }
#     }
    
#     # Also compare URL date with filename date
#     if ($result.UrlDate -and $result.File.Path -match "(\d{4}-\d{2}-\d{2})") {
#         $filenameDate = $matches[1]
#         if (Test-DateSignificantlyDifferent $result.UrlDate $filenameDate) {
#             $urlDateMismatches += @{
#                 File         = $result.File
#                 Url          = $result.Url
#                 UrlDate      = $result.UrlDate
#                 FilenameDate = $filenameDate
#                 Source       = 'URL vs Filename'
#             }
#         }
#     }
# }

# if ($urlIssues.Count -gt 0) {
#     Write-Host "❌ Found $($urlIssues.Count) URL accessibility issues:" -ForegroundColor Red
#     foreach ($issue in $urlIssues) {
#         Write-Host "   📁 $($issue.File)" -ForegroundColor Yellow
#         Write-Host "      URL: $($issue.Url)" -ForegroundColor White
#         Write-Host "      Error: $($issue.Error)" -ForegroundColor Gray
#     }
# }
# else {
#     Write-Host "✅ All canonical URLs are accessible!" -ForegroundColor Green
# }

# if ($removedPosts.Count -gt 0) {
#     Write-Host "❌ Found $($removedPosts.Count) removed posts in community content:" -ForegroundColor Red
#     foreach ($removed in $removedPosts) {
#         Write-Host "   📁 $($removed.File)" -ForegroundColor Yellow
#         Write-Host "      URL: $($removed.Url)" -ForegroundColor White
#         Write-Host "      Status: $($removed.Status)" -ForegroundColor Gray
#     }
# }

# if ($urlDateMismatches.Count -gt 0) {
#     # Remove duplicates by creating a unique key for each mismatch
#     $uniqueMismatches = @{}
#     foreach ($mismatch in $urlDateMismatches) {
#         $key = "$($mismatch.File)|$($mismatch.Source)"
#         if (-not $uniqueMismatches.ContainsKey($key)) {
#             $uniqueMismatches[$key] = $mismatch
#         }
#     }
#     $urlDateMismatches = $uniqueMismatches.Values
    
#     Write-Host "❌ Found $($urlDateMismatches.Count) significant date mismatches from URLs/HTML (ignoring 7-day publishing differences):" -ForegroundColor Red
#     foreach ($mismatch in $urlDateMismatches) {
#         Write-Host "   📁 $($mismatch.File)" -ForegroundColor Yellow
#         Write-Host "      URL: $($mismatch.Url)" -ForegroundColor White
#         if ($mismatch.HtmlDate) {
#             Write-Host "      HTML Date: $($mismatch.HtmlDate) | Frontmatter: $($mismatch.FrontmatterDate)" -ForegroundColor Gray
#         }
#         if ($mismatch.UrlDate) {
#             $comparison = if ($mismatch.Source -eq 'URL vs Filename') { "Filename: $($mismatch.FilenameDate)" } else { "Frontmatter: $($mismatch.FrontmatterDate)" }
#             Write-Host "      URL Date: $($mismatch.UrlDate) | $comparison" -ForegroundColor Gray
#         }
#     }
# }
# else {
#     Write-Host "✅ All URL/HTML dates match frontmatter dates (within 1-day tolerance)!" -ForegroundColor Green
# }
# Write-Host ""

# 4. CANONICAL URL DUPLICATION CHECK
Write-Host "🔴 CHECKING CANONICAL URL DUPLICATES..." -ForegroundColor Red
$canonicalUrlGroups = @{}

foreach ($file in $allFiles) {
    try {
        $content = Get-Content $file.Path -Raw -ErrorAction Stop
        
        # Extract canonical URL from frontmatter
        if ($content -match 'canonical_url:\s*["\s]*([^"\s\r\n]+)') {
            $canonicalUrl = $matches[1].Trim()
            
            if (-not $canonicalUrlGroups.ContainsKey($canonicalUrl)) {
                $canonicalUrlGroups[$canonicalUrl] = @()
            }
            $canonicalUrlGroups[$canonicalUrl] += $file.Path
        }
    }
    catch {
        Write-Host "   ⚠️  Could not read file: $($file.Path) - $($_.ToString())" -ForegroundColor Yellow
    }
}

# Find duplicates
$duplicateCanonicalUrls = $canonicalUrlGroups.GetEnumerator() | Where-Object { $_.Value.Count -gt 1 }

if ($duplicateCanonicalUrls.Count -gt 0) {
    Write-Host "❌ Found $($duplicateCanonicalUrls.Count) canonical URLs used by multiple files:" -ForegroundColor Red
    foreach ($urlGroup in $duplicateCanonicalUrls) {
        Write-Host "   🔗 URL: $($urlGroup.Key)" -ForegroundColor Yellow
        Write-Host "   📁 Used by $($urlGroup.Value.Count) files:" -ForegroundColor White
        foreach ($filePath in $urlGroup.Value) {
            Write-Host "      - $filePath" -ForegroundColor Gray
        }
        Write-Host ""
    }
}
else {
    Write-Host "✅ All canonical URLs are unique!" -ForegroundColor Green
}
Write-Host ""

# 5. MATHEMATICAL APPROACH TO DUPLICATE DETECTION
if (-not $QuickCheck) {
    Write-Host "🔴 CHECKING FOR DUPLICATES (MATHEMATICAL INDEX APPROACH)..." -ForegroundColor Red
    Write-Host "   Using inverted index and set theory for optimal performance" -ForegroundColor Gray
    
    $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
    
    # Step 1: Build content profiles and inverted word index
    Write-Host "   📝 Building content profiles and word index..." -ForegroundColor Yellow
    $fileProfiles = @()
    $wordIndex = @{}  # word -> list of file indices that contain it
    $processedCount = 0
    
    foreach ($file in $allFiles) {
        try {
            $content = Get-Content $file.Path -Raw -ErrorAction Stop
            
            if ([string]::IsNullOrWhiteSpace($content)) {
                Write-Host "   ⚠️  Empty file: $($file.Path)" -ForegroundColor Yellow
                continue
            }
            
            # Remove frontmatter for comparison
            $contentBody = Get-ContentBody $content
            
            # Extract title from frontmatter
            $titleMatch = $content | Select-String 'title:\s*"([^"]+)"'
            $title = if ($titleMatch) { $titleMatch.Matches[0].Groups[1].Value } else { $file.Name }
            
            # Get normalized words from content
            $contentWords = Get-NormalizedWords $contentBody
            if ($contentWords.Count -eq 0) {
                Write-Host "   ⚠️  No content words found: $($file.Path)" -ForegroundColor Yellow
                continue
            }
            
            # Create file profile
            $fileIndex = $fileProfiles.Count
            $uniqueWords = $contentWords | Sort-Object -Unique
            
            # Create content hash for exact duplicate detection
            $contentHash = [System.Security.Cryptography.SHA256]::Create().ComputeHash([System.Text.Encoding]::UTF8.GetBytes($contentBody))
            $contentHashString = [System.BitConverter]::ToString($contentHash) -replace '-', ''
            
            $fileProfile = @{
                Index           = $fileIndex
                File            = $file
                Title           = $title
                ContentWords    = $contentWords
                UniqueWords     = $uniqueWords
                WordCount       = $contentWords.Count
                UniqueWordCount = $uniqueWords.Count
                ContentHash     = $contentHashString
                ContentBody     = $contentBody.Substring(0, [Math]::Min(100, $contentBody.Length))
            }
            
            $fileProfiles += $fileProfile
            
            # Build inverted index: for each unique word, track which files contain it
            foreach ($word in $uniqueWords) {
                if (-not $wordIndex.ContainsKey($word)) {
                    $wordIndex[$word] = @()
                }
                $wordIndex[$word] += $fileIndex
            }
            
            $processedCount++
        }
        catch {
            Write-Host "   ⚠️  Error processing file: $($file.Path) - $($_.ToString())" -ForegroundColor Yellow
        }
        
        # Progress indicator every 50 files
        if ($processedCount % 50 -eq 0) {
            Write-Host "   Progress: $processedCount files indexed..." -ForegroundColor Gray
        }
    }
    
    Write-Host "   ✅ Successfully indexed $($fileProfiles.Count) files with $($wordIndex.Keys.Count) unique words" -ForegroundColor Green
    
    # Step 2: Find exact duplicates by content hash (super fast)
    Write-Host "   🔍 Finding exact duplicates using content hashes..." -ForegroundColor Yellow
    $exactDuplicates = @()
    $hashGroups = $fileProfiles | Group-Object -Property ContentHash | Where-Object { $_.Count -gt 1 }
    
    foreach ($group in $hashGroups) {
        $files = $group.Group
        for ($i = 0; $i -lt $files.Count; $i++) {
            for ($j = $i + 1; $j -lt $files.Count; $j++) {
                $exactDuplicates += @{
                    File1          = $files[$i].File.Path
                    File2          = $files[$j].File.Path
                    Similarity     = 100.0
                    Type           = "Exact Content Match"
                    Directory1     = $files[$i].File.Directory
                    Directory2     = $files[$j].File.Directory
                    ContentSample1 = $files[$i].ContentBody
                    ContentSample2 = $files[$j].ContentBody
                }
            }
        }
    }
    
    Write-Host "   📊 Found $($exactDuplicates.Count) exact duplicate pairs" -ForegroundColor Green
    
    # Step 3: Use mathematical approach to find similar pairs efficiently
    Write-Host "   🧮 Using mathematical similarity detection (threshold: $SimilarityThreshold%)..." -ForegroundColor Yellow
    
    $candidatePairs = @()
    $totalComparisons = 0
    $skippedComparisons = 0
    
    # For each file, use inverted index to find potential matches
    for ($i = 0; $i -lt $fileProfiles.Count; $i++) {
        $profile1 = $fileProfiles[$i]
        
        # Skip if this file is already in an exact duplicate pair
        $isInExactDuplicate = $exactDuplicates | Where-Object { 
            $_.File1 -eq $profile1.File.Path -or $_.File2 -eq $profile1.File.Path 
        }
        if ($isInExactDuplicate) {
            continue
        }
        
        # Use inverted index to find files that share words with this file
        $potentialMatches = @{}
        foreach ($word in $profile1.UniqueWords) {
            if ($wordIndex.ContainsKey($word)) {
                foreach ($matchIndex in $wordIndex[$word]) {
                    if ($matchIndex -gt $i) {
                        # Only compare each pair once
                        if (-not $potentialMatches.ContainsKey($matchIndex)) {
                            $potentialMatches[$matchIndex] = 0
                        }
                        $potentialMatches[$matchIndex]++
                    }
                }
            }
        }
        
        # Filter potential matches using mathematical pre-screening
        foreach ($matchIndex in $potentialMatches.Keys) {
            $profile2 = $fileProfiles[$matchIndex]
            $sharedWords = $potentialMatches[$matchIndex]
            
            $totalComparisons++
            
            # Mathematical pre-filter 1: Jaccard upper bound estimation
            # If shared_words / min(words1, words2) < threshold, skip
            $minWords = [Math]::Min($profile1.UniqueWordCount, $profile2.UniqueWordCount)
            $maxPossibleJaccard = if ($minWords -gt 0) { ($sharedWords / $minWords) * 100 } else { 0 }
            
            if ($maxPossibleJaccard -lt $SimilarityThreshold) {
                $skippedComparisons++
                continue
            }
            
            # Mathematical pre-filter 2: Word count ratio filter
            $wordCountRatio = if ($profile2.UniqueWordCount -gt 0) { $profile1.UniqueWordCount / $profile2.UniqueWordCount } else { 999 }
            if ($wordCountRatio -gt 3 -or $wordCountRatio -lt 0.33) {
                $skippedComparisons++
                continue
            }
            
            # Mathematical pre-filter 3: Minimum intersection requirement
            # For Jaccard >= threshold, we need: intersection >= threshold * union / 100
            # Since union <= words1 + words2, we can estimate minimum required intersection
            $maxUnion = $profile1.UniqueWordCount + $profile2.UniqueWordCount
            $minRequiredIntersection = ($SimilarityThreshold * $maxUnion) / (100 + $SimilarityThreshold)
            
            if ($sharedWords -lt $minRequiredIntersection) {
                $skippedComparisons++
                continue
            }
            
            # This pair passes all mathematical pre-filters, add to candidates
            $candidatePairs += @{
                Profile1            = $profile1
                Profile2            = $profile2
                SharedWords         = $sharedWords
                EstimatedSimilarity = $maxPossibleJaccard
            }
        }
        
        # Progress indicator
        if ($i % 20 -eq 0) {
            Write-Host "   Progress: $i/$($fileProfiles.Count) files processed, $($candidatePairs.Count) candidates found..." -ForegroundColor Gray
        }
    }
    
    Write-Host "   📊 Mathematical screening: $totalComparisons total comparisons, $skippedComparisons skipped, $($candidatePairs.Count) similar pairs for detailed analysis" -ForegroundColor Green
    Write-Host "   ⚡ Efficiency: $([Math]::Round(($skippedComparisons / [Math]::Max(1, $totalComparisons)) * 100, 1))% reduction in expensive calculations" -ForegroundColor Green
    
    # Step 4: Calculate precise Jaccard similarity only for promising pairs
    Write-Host "   🎯 Calculating precise similarity for $($candidatePairs.Count) promising pairs..." -ForegroundColor Yellow
    
    # If there are fewer than 10 promising pairs, log each of them
    if ($candidatePairs.Count -lt 10) {
        Write-Host "   📋 Promising pairs for analysis:" -ForegroundColor Cyan
        for ($i = 0; $i -lt $candidatePairs.Count; $i++) {
            $candidate = $candidatePairs[$i]
            Write-Host "      $($i + 1). $($candidate.Profile1.File.Path) ↔ $($candidate.Profile2.File.Path)" -ForegroundColor Gray
        }
    }
    
    $similarDuplicates = @()
    $candidateCount = 0
    
    foreach ($candidate in $candidatePairs) {
        $profile1 = $candidate.Profile1
        $profile2 = $candidate.Profile2
        
        # Calculate precise Jaccard similarity
        $intersection = Compare-Object $profile1.UniqueWords $profile2.UniqueWords -IncludeEqual -ExcludeDifferent | Measure-Object | Select-Object -ExpandProperty Count
        $union = ($profile1.UniqueWords + $profile2.UniqueWords | Sort-Object -Unique | Measure-Object).Count
        
        $jaccardSimilarity = if ($union -gt 0) { [math]::Round(($intersection / $union) * 100, 1) } else { 0 }
        
        if ($jaccardSimilarity -ge $SimilarityThreshold) {
            $similarDuplicates += @{
                File1          = $profile1.File.Path
                File2          = $profile2.File.Path
                Similarity     = $jaccardSimilarity
                Type           = "Content Similarity"
                Directory1     = $profile1.File.Directory
                Directory2     = $profile2.File.Directory
                ContentSample1 = $profile1.ContentBody
                ContentSample2 = $profile2.ContentBody
            }
        }
        
        $candidateCount++
        if ($candidateCount % 100 -eq 0) {
            Write-Host "   Progress: $candidateCount/$($candidatePairs.Count) pairs analyzed..." -ForegroundColor Gray
        }
    }
    
    $stopwatch.Stop()
    
    # Combine all duplicates
    $allDuplicates = $exactDuplicates + $similarDuplicates | Sort-Object -Property Similarity -Descending
    
    # Filter duplicates with intelligent thresholds
    # The user's threshold is the minimum, but we apply additional logic for what's truly concerning
    $concerningDuplicates = $allDuplicates | Where-Object {
        # Always show exact matches
        $_.Similarity -eq 100 -or
        
        # Always show if above user threshold AND meets additional criteria
        ($_.Similarity -ge $SimilarityThreshold -and (
            # Very high similarity is always concerning
            $_.Similarity -gt 80 -or
            
            # High similarity in same directory is concerning (suggests real duplicates)
            ($_.Similarity -gt 60 -and $_.Directory1 -eq $_.Directory2) -or
            
            # Medium similarity with user threshold >= 50 (user specifically looking for loose matches)
            ($SimilarityThreshold -ge 50 -and $_.Similarity -ge [Math]::Max($SimilarityThreshold, 50))
        )) -or
        
        # If user sets very low threshold (< 50), only show top matches to avoid spam
        ($SimilarityThreshold -lt 50 -and $_.Similarity -ge [Math]::Max($SimilarityThreshold, 70))
    }
    
    Write-Host "   ⚡ Analysis completed in $($stopwatch.ElapsedMilliseconds)ms" -ForegroundColor Green
    
    if ($concerningDuplicates.Count -gt 0) {
        # Group duplicates by file to show candidates more clearly
        $fileGroups = @{}
        
        foreach ($duplicate in $concerningDuplicates) {
            $file1 = $duplicate.File1
            $file2 = $duplicate.File2
            
            # Add both files as potential candidates
            if (-not $fileGroups.ContainsKey($file1)) {
                $fileGroups[$file1] = @{
                    FilePath  = $file1
                    Directory = $duplicate.Directory1
                    SimilarTo = @()
                }
            }
            if (-not $fileGroups.ContainsKey($file2)) {
                $fileGroups[$file2] = @{
                    FilePath  = $file2
                    Directory = $duplicate.Directory2
                    SimilarTo = @()
                }
            }
            
            # Add the similarity relationships
            $fileGroups[$file1].SimilarTo += @{
                File       = $file2
                Similarity = $duplicate.Similarity
                Type       = $duplicate.Type
                Directory  = $duplicate.Directory2
            }
            $fileGroups[$file2].SimilarTo += @{
                File       = $file1
                Similarity = $duplicate.Similarity
                Type       = $duplicate.Type
                Directory  = $duplicate.Directory1
            }
        }
        
        # Sort candidates by highest similarity and show results
        $candidateFiles = $fileGroups.Values | Sort-Object { ($_.SimilarTo | Measure-Object -Property Similarity -Maximum).Maximum } -Descending
        
        Write-Host "❌ Found $($candidateFiles.Count) files with potential duplicates:" -ForegroundColor Red
        Write-Host ""
        
        for ($i = 0; $i -lt $candidateFiles.Count; $i++) {
            $candidate = $candidateFiles[$i]
            $fileName = Split-Path $candidate.FilePath -Leaf
            
            Write-Host "   📁 $fileName" -ForegroundColor Yellow
            Write-Host "      Path: $($candidate.FilePath)" -ForegroundColor Gray
            Write-Host "      Directory: $($candidate.Directory)" -ForegroundColor Gray
            Write-Host "      Similar to $($candidate.SimilarTo.Count) other file(s):" -ForegroundColor White
            
            # Sort similarities by percentage and show all matches
            $sortedSimilarities = $candidate.SimilarTo | Sort-Object Similarity -Descending
            
            foreach ($similar in $sortedSimilarities) {
                $similarName = Split-Path $similar.File -Leaf
                $sameDir = if ($similar.Directory -eq $candidate.Directory) { " (same directory)" } else { "" }
                $typeMarker = if ($similar.Type -eq "Exact Content Match") { "🔄" } else { "≈" }
                
                Write-Host "         $typeMarker $($similar.Similarity)% - $similarName$sameDir" -ForegroundColor White
            }
            
            Write-Host ""
        }
        
        Write-Host "💡 These files are candidates for review. Check if they contain duplicate content." -ForegroundColor Cyan
    }
    else {
        Write-Host "✅ No concerning duplicates found at $SimilarityThreshold% threshold!" -ForegroundColor Green
    }
}
else {
    Write-Host "⚡ Skipping duplicate check (use without -QuickCheck for full analysis)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "                SUMMARY                " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "📊 Total files found: $($allFiles.Count)" -ForegroundColor White
if ($nonMarkdownFiles.Count -gt 0) {
    Write-Host "⚠️  Non-markdown files: $($nonMarkdownFiles.Count)" -ForegroundColor Yellow
}
if (-not $QuickCheck) {
    Write-Host "📊 Files successfully processed: $($fileProfiles.Count)" -ForegroundColor White
}
Write-Host "🔴 Date mismatches: $($dateMismatches.Count)" -ForegroundColor Red
Write-Host "🔴 Permalink mismatches: $($permalinkMismatches.Count)" -ForegroundColor Red
Write-Host "🔴 URL accessibility issues: $($urlIssues.Count)" -ForegroundColor Red
Write-Host "🔴 Removed community posts: $($removedPosts.Count)" -ForegroundColor Red
Write-Host "🔴 URL/HTML date mismatches: $($urlDateMismatches.Count)" -ForegroundColor Red
Write-Host "🔴 Duplicate canonical URLs: $($duplicateCanonicalUrls.Count)" -ForegroundColor Red

if (-not $QuickCheck) {
    $duplicateCount = if (Get-Variable -Name "candidateFiles" -ErrorAction SilentlyContinue) { $candidateFiles.Count } else { 0 }
    Write-Host "🔄 Candidate files with duplicates: $duplicateCount" -ForegroundColor Red
    Write-Host "⚡ Analysis time: $($stopwatch.ElapsedMilliseconds)ms" -ForegroundColor Green
}

$duplicateCount = if (Get-Variable -Name "candidateFiles" -ErrorAction SilentlyContinue) { $candidateFiles.Count } else { 0 }
$allIssuesCount = $dateMismatches.Count + $permalinkMismatches.Count + $urlIssues.Count + $removedPosts.Count + $urlDateMismatches.Count + $duplicateCount
if ($allIssuesCount -eq 0 -and $nonMarkdownFiles.Count -eq 0) {
    Write-Host ""
    Write-Host "🎉 ALL VALIDATION CHECKS PASSED!" -ForegroundColor Green
}
else {
    Write-Host ""
    Write-Host "⚠️  Issues found - see details above" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "💡 Usage tips:" -ForegroundColor Cyan
Write-Host "   -QuickCheck        Skip duplicate analysis for faster execution" -ForegroundColor White
Write-Host "   -SimilarityThreshold 80   Set duplicate similarity threshold (default: 40%)" -ForegroundColor White
Write-Host "   -ExcludeDirectories @('_news','_videos')   Exclude specific directories from analysis" -ForegroundColor White
