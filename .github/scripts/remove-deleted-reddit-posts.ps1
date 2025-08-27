#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Removes markdown files that reference deleted/removed Reddit posts.

.DESCRIPTION
    This script checks all markdown files with Reddit canonical URLs to see if the 
    posts have been deleted, removed, or banned on Reddit. It uses Reddit's JSON API
    to efficiently check post status and removes the corresponding markdown files.

.PARAMETER DryRun
    Show what would be removed without actually deleting any files.

.PARAMETER DaysBack
    Only check posts from the last N days. Default is 7 days.

.EXAMPLE
    .\remove-deleted-reddit-posts.ps1 -DryRun
    Shows what would be removed without deleting anything.

.EXAMPLE
    .\remove-deleted-reddit-posts.ps1 -DaysBack 14
    Remove deleted posts, checking last 14 days.
#>

param(
    [switch]$DryRun,
    [int]$DaysBack = 7,
    [string]$WorkspaceDirectory = $PSScriptRoot
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Determine the correct functions path
$functionsPath = if ($WorkspaceDirectory -eq $PSScriptRoot) {
    # Running from the script's directory
    Join-Path $PSScriptRoot "functions"
}
else {
    # Running from workspace root
    Join-Path $WorkspaceDirectory ".github/scripts/functions"
}

# Import Write-ErrorDetails first (for error handling), then all others sorted alphabetically
. (Join-Path $functionsPath "Write-ErrorDetails.ps1")

try {
    Get-ChildItem -Path $functionsPath -Filter "*.ps1" | 
    Where-Object { $_.Name -ne "Write-ErrorDetails.ps1" } |
    ForEach-Object { . $_.FullName }

    if ($DryRun) {
        Write-Host "🔍 DRY RUN MODE - No files will be deleted" -ForegroundColor Yellow
        Write-Host ""
    }

    # Get all markdown files from content directories
    Write-Host "📁 Scanning content directories..." -ForegroundColor White

    $allFiles = @()
    $totalFileCount = 0

    $dir = "_community"
    if (Test-Path $dir) {
        $dirFiles = Get-ChildItem $dir -Filter "*.md" -File -ErrorAction SilentlyContinue
        $totalFileCount += $dirFiles.Count
        
        $allFiles += $dirFiles | ForEach-Object { 
            @{
                Path      = $_.FullName.Replace((Get-Location).Path + "\", "").Replace("\", "/")
                Directory = $dir
                Name      = $_.Name
                FullPath  = $_.FullName
            }
        }
        
        Write-Host "   📂 $dir`: $($dirFiles.Count) files" -ForegroundColor Gray
    }
    else {
        Write-Host "   ⚠️  Directory not found: $dir" -ForegroundColor Yellow
    }

    Write-Host "   📊 Total: $totalFileCount markdown files" -ForegroundColor White
    Write-Host ""

    # Find files with Reddit URLs
    Write-Host "🔍 Finding files with Reddit canonical URLs..." -ForegroundColor White

    $redditFiles = @()
    $processedCount = 0

    foreach ($file in $allFiles) {
        $processedCount++
        if ($processedCount % 50 -eq 0) {
            Write-Host "   Progress: $processedCount/$($allFiles.Count) files scanned..." -ForegroundColor Gray
        }
    
        try {
            $content = Get-Content $file.FullPath -Raw -ErrorAction Stop
            $canonicalUrl = Get-CanonicalUrl $content
        
            if ($canonicalUrl -and $canonicalUrl -match 'reddit\.com') {
                $postDate = Get-FrontmatterDate $content
            
                $redditFiles += @{
                    File    = $file
                    Url     = $canonicalUrl
                    Content = $content
                    Date    = $postDate
                }
            }
        }
        catch {
            Write-Host "   ⚠️  Could not read file: $($file.Path) - $($_.ToString())" -ForegroundColor Yellow
        }
    }

    Write-Host "   🎯 Found $($redditFiles.Count) files with Reddit URLs" -ForegroundColor White
    Write-Host ""

    if ($redditFiles.Count -eq 0) {
        Write-Host "✅ No Reddit URLs found. Nothing to check." -ForegroundColor Green
        exit 0
    }

    # Filter to only include posts from the last N days
    $cutoffDate = (Get-Date).AddDays(-$DaysBack)
    Write-Host "📅 Filtering posts to last $DaysBack days (since $($cutoffDate.ToString('yyyy-MM-dd')))..." -ForegroundColor White

    $originalCount = $redditFiles.Count
    $redditFiles = $redditFiles | Where-Object { 
        $_.Date -ne [DateTime]::MinValue -and $_.Date -ge $cutoffDate 
    }

    Write-Host "   📊 Filtered from $originalCount to $($redditFiles.Count) posts" -ForegroundColor Gray

    if ($redditFiles.Count -eq 0) {
        Write-Host "✅ No Reddit posts found from the last $DaysBack days. Nothing to check." -ForegroundColor Green
        exit 0
    }

    # Sort Reddit files by date (latest first)
    $redditFiles = $redditFiles | Sort-Object { $_.Date } -Descending

    Write-Host "   📊 Date range: " -ForegroundColor Gray -NoNewline
    $validDates = $redditFiles | Where-Object { $_.Date -ne [DateTime]::MinValue }
    if ($validDates.Count -gt 0) {
        $latestDate = ($validDates | Select-Object -First 1).Date
        $oldestDate = ($validDates | Select-Object -Last 1).Date
        Write-Host "$($latestDate.ToString('yyyy-MM-dd')) to $($oldestDate.ToString('yyyy-MM-dd'))" -ForegroundColor Gray
    }
    else {
        Write-Host "No valid dates found" -ForegroundColor Yellow
    }
    Write-Host ""

    # Check Reddit posts for removal status
    Write-Host "🔍 Checking Reddit posts for removal status..." -ForegroundColor White
    Write-Host "   (This may take a while for many URLs)" -ForegroundColor Gray
    Write-Host ""

    $removedPosts = @()
    $activePosts = @()
    $errorPosts = @()

    $checkedCount = 0
    foreach ($redditFile in $redditFiles) {
        $checkedCount++
    
        $dateInfo = ""
        if ($redditFile.Date -ne [DateTime]::MinValue) {
            $dateInfo = " ($($redditFile.Date.ToString('yyyy-MM-dd')))"
        }
    
        Write-Host "[$checkedCount/$($redditFiles.Count)] Checking: $($redditFile.File.Path)$dateInfo" -ForegroundColor White
    
        $checkResult = Test-RedditPostRemoved -RedditUrl $redditFile.Url -FilePath $redditFile.File.Path
    
        if ($checkResult.Error) {
            $errorPosts += @{
                File  = $redditFile.File
                Url   = $redditFile.Url
                Error = $checkResult.Error
            }
            Write-Host "   ❌ Error: $($checkResult.Error)" -ForegroundColor Red
        }
        elseif ($checkResult.IsRemoved) {
            $removedPosts += @{
                File     = $redditFile.File
                Url      = $redditFile.Url
                Details  = $checkResult.Details
                PostData = $checkResult.PostData
            }
            Write-Host "   🗑️  REMOVED: $($checkResult.Details)" -ForegroundColor Red
        }
        else {
            $activePosts += @{
                File    = $redditFile.File
                Url     = $redditFile.Url
                Details = $checkResult.Details
            }
            Write-Host "   ✅ Active" -ForegroundColor Green
        }
    
        # Add delay to be respectful to Reddit's servers
        # Increase delay if we've hit rate limits recently
        if ($checkResult.Error -and $checkResult.Error -match "429") {
            Write-Host "   ⏱️  Rate limited. Waiting 10 seconds..." -ForegroundColor Yellow
            Start-Sleep -Seconds 10
        }
        else {
            Start-Sleep -Seconds 5
        }
    }

    Write-Host ""

    # Report results
    Write-Host "📊 RESULTS SUMMARY:" -ForegroundColor Cyan
    Write-Host "   ✅ Active posts: $($activePosts.Count)" -ForegroundColor Green
    Write-Host "   🗑️  Removed posts: $($removedPosts.Count)" -ForegroundColor Red
    Write-Host "   ❌ Errors: $($errorPosts.Count)" -ForegroundColor Yellow
    Write-Host ""

    # Handle errors
    if ($errorPosts.Count -gt 0) {
        Write-Host "❌ POSTS WITH ERRORS:" -ForegroundColor Red
        foreach ($errorPost in $errorPosts) {
            Write-Host "   📁 $($errorPost.File.Path)" -ForegroundColor Yellow
            Write-Host "      URL: $($errorPost.Url)" -ForegroundColor White
            Write-Host "      Error: $($errorPost.Error)" -ForegroundColor Gray
        }
        Write-Host ""
    }

    # Handle removed posts
    if ($removedPosts.Count -gt 0) {
        Write-Host "🗑️  REMOVED POSTS TO DELETE:" -ForegroundColor Red
        foreach ($removedPost in $removedPosts) {
            Write-Host "   📁 $($removedPost.File.Path)" -ForegroundColor Yellow
            Write-Host "      URL: $($removedPost.Url)" -ForegroundColor White
            Write-Host "      Reason: $($removedPost.Details)" -ForegroundColor Gray
        
            if ($removedPost.PostData) {
                $postData = $removedPost.PostData
                if ($postData.title) {
                    Write-Host "      Title: $($postData.title)" -ForegroundColor Gray
                }
                if ($postData.subreddit) {
                    Write-Host "      Subreddit: r/$($postData.subreddit)" -ForegroundColor Gray
                }
            }
        }
        Write-Host ""
    
        if ($DryRun) {
            Write-Host "🔍 DRY RUN: Would delete $($removedPosts.Count) files" -ForegroundColor Yellow
            Write-Host "   Run without -DryRun to actually delete these files." -ForegroundColor Gray
        }
        else {
            # Confirm deletion
            Write-Host "⚠️  CONFIRMATION REQUIRED" -ForegroundColor Yellow
            Write-Host "Are you sure you want to delete $($removedPosts.Count) markdown files?" -ForegroundColor White
            Write-Host "Type 'YES' to confirm, anything else to cancel: " -ForegroundColor Yellow -NoNewline
        
            $confirmation = Read-Host
        
            if ($confirmation -eq 'YES') {
                Write-Host ""
                Write-Host "🗑️  Deleting removed Reddit posts..." -ForegroundColor Red
            
                $deletedCount = 0
                $failedDeletes = @()
            
            foreach ($removedPost in $removedPosts) {
                try {
                    Remove-Item -Path $removedPost.File.FullPath -Force -ErrorAction Stop
                    $deletedCount++
                    Write-Host "   ✅ Deleted: $($removedPost.File.Path)" -ForegroundColor Green
                }
                catch {
                    $failedDeletes += @{
                        File  = $removedPost.File.Path
                        Error = $_.ToString()
                    }
                    Write-Host "   ❌ Failed to delete: $($removedPost.File.Path) - $($_.ToString())" -ForegroundColor Red
                }
            
                Write-Host ""
                Write-Host "✅ DELETION COMPLETE:" -ForegroundColor Green
                Write-Host "   🗑️  Successfully deleted: $deletedCount files" -ForegroundColor Green
                if ($failedDeletes.Count -gt 0) {
                    Write-Host "   ❌ Failed to delete: $($failedDeletes.Count) files" -ForegroundColor Red
                }
            
                if ($failedDeletes.Count -gt 0) {
                    Write-Host ""
                    Write-Host "❌ DELETION FAILURES:" -ForegroundColor Red
                    foreach ($failed in $failedDeletes) {
                        Write-Host "   📁 $($failed.File)" -ForegroundColor Yellow
                        Write-Host "      Error: $($failed.Error)" -ForegroundColor Gray
                    }
                }
            }
            else {
                Write-Host ""
                Write-Host "🚫 Deletion cancelled by user." -ForegroundColor Yellow
            }
        }
    }
    else {
        Write-Host "✅ No removed Reddit posts found. All Reddit links are active!" -ForegroundColor Green
    }

    Write-Host ""
    Write-Host "🔚 Script completed." -ForegroundColor Cyan
}
catch {
    Write-ErrorDetails -ErrorRecord $_ -Context "Removing deleted Reddit posts"
    throw
}