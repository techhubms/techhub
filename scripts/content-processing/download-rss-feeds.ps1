<#
.SYNOPSIS
Downloads RSS feeds and saves them to structured data directories with content fetching.

.DESCRIPTION
Downloads RSS feeds from configured sources and saves them in a structured format:
- One directory per output type (e.g., _news, _posts)
- One subdirectory per feed within each output type
- One JSON file per feed item
Each item includes enhanced content fetched using Playwright (for Reddit) or direct HTTP (for others).

.PARAMETER WorkspaceDirectory
Optional. Path to the workspace directory. Defaults to the script's directory.
When running in GitHub Actions, this should be set to the GitHub workspace path.

.EXAMPLE
./download-rss-feeds.ps1

.EXAMPLE
# Run from GitHub Actions with workspace directory
./download-rss-feeds.ps1 -WorkspaceDirectory ${{ github.workspace }}
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$WorkspaceDirectory = $PSScriptRoot
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Add required assemblies
Add-Type -AssemblyName System.Web

# Determine the correct functions path
$functionsPath = if ($WorkspaceDirectory -eq $PSScriptRoot) {
    # Running from the script's directory
    Join-Path $PSScriptRoot "functions"
}
else {
    # Running from workspace root
    Join-Path $WorkspaceDirectory "scripts/functions"
}

. (Join-Path $functionsPath "Write-ErrorDetails.ps1")

try {
    Get-ChildItem -Path $functionsPath -Filter "*.ps1" | 
    Where-Object { $_.Name -ne "Write-ErrorDetails.ps1" } |
    ForEach-Object { . $_.FullName }

    Write-Host "📡 Starting RSS feed download..."
    
    $sourceRoot = Get-SourceRoot
    $feedsPath = Join-Path $sourceRoot "scripts/data/rss-feeds.json"
    $dataDir = Join-Path $sourceRoot "scripts/data/rss-cache"

    if (-not (Test-Path $feedsPath)) {
        throw "Feeds file not found: $feedsPath"
    }

    # Ensure data directory exists
    if (-not (Test-Path $dataDir)) {
        New-Item -ItemType Directory -Path $dataDir -Force | Out-Null
        Write-Host "📁 Created data directory"
    }

    # Load feed configurations
    $feedConfigs = Get-Content $feedsPath | ConvertFrom-Json

    if (-not $feedConfigs -or $feedConfigs.Count -eq 0) {
        Write-Host "⚠️ No feed configurations found"
        return
    }

    # Sort feeds by collection priority
    $collectionOrder = Get-CollectionPriorityOrder

    # Sort feeds by collection order, fallback to name for unknown collections
    $feedConfigs = $feedConfigs | Sort-Object {
        $order = $collectionOrder[$_.outputDir]
        if ($order) { $order } else { 60 }
    }

    # Track all current feed items to identify files for removal
    $allCurrentItems = @{}
    $outputTypes = @{}
    $successfulFeeds = @{}  # Track feeds that were successfully processed

    $successCount = 0
    $totalCount = $feedConfigs.Count

    foreach ($feedConfig in $feedConfigs) {
        try {
            Write-Host "Downloading: $($feedConfig.name)"
            
            $feed = [Feed]::new($feedConfig.name, $feedConfig.outputDir, $feedConfig.url)

            # Check if feed has items in the last 365 days
            $cutoffDate = (Get-Date).AddDays(-365)
            $recentItems = @($feed.Items | Where-Object { $_.PubDate -gt $cutoffDate })
            
            if ($recentItems.Count -eq 0) {
                Write-Host "⚠️ Warning: $($feedConfig.name) has no items in the last 365 days" -ForegroundColor Yellow
                continue
            }

            # Create output type directory
            $outputTypeDir = Join-Path $dataDir $feedConfig.outputDir
            if (-not (Test-Path $outputTypeDir)) {
                New-Item -ItemType Directory -Path $outputTypeDir -Force | Out-Null
            }
            
            # Track this output type
            $outputTypes[$feedConfig.outputDir] = $true

            # Create feed directory (sanitize feed name for filesystem)
            $feedDirName = $feedConfig.name -replace '[^A-Za-z0-9_\-\s]', '_' -replace '\s+', '_'
            $feedDir = Join-Path $outputTypeDir $feedDirName
            if (-not (Test-Path $feedDir)) {
                New-Item -ItemType Directory -Path $feedDir -Force | Out-Null
            }

            Write-Host "📝 Processing $($recentItems.Count) items for $($feedConfig.name)..."
            
            $processedCount = 0
            $enhancedCount = 0
            $errorCount = 0
            $newItemsCount = 0
            $skippedItemsCount = 0

            # Phase 1: Determine which files need to be created (no content downloading yet)
            $itemsToProcess = @()
            $otherUrlsToFetch = @()
            
            foreach ($item in $recentItems) {
                # Create unique filename based on link or title
                $baseFilename = if ($item.Link) { 
                    $item.Link
                }
                else {
                    $item.Title
                }
                
                $itemId = ConvertTo-SafeFilename -Title $baseFilename -MaxLength 100
                $itemFileName = "$itemId.json"
                $itemFilePath = Join-Path $feedDir $itemFileName
                
                # Track this item as current
                $allCurrentItems[$itemFilePath] = $true

                # Check if file needs processing
                $needsProcessing = $false
                if (-not (Test-Path $itemFilePath)) {
                    $needsProcessing = $true
                }
                else {
                    try {
                        $existingContent = Get-Content $itemFilePath -Raw | ConvertFrom-Json
                        if (-not ($existingContent -and $existingContent.Title)) {
                            $needsProcessing = $true
                        }
                    }
                    catch {
                        # If we can't parse existing file, we'll recreate it
                        Write-Host "    ⚠️ Corrupted file detected, will recreate: $($item.Title.Substring(0, [Math]::Min(50, $item.Title.Length)))..."
                        $needsProcessing = $true
                    }
                }

                if ($needsProcessing) {
                    # Create processing item with metadata
                    $processingItem = @{
                        Item       = $item
                        FilePath   = $itemFilePath
                        IsYouTube  = $item.Link -match 'youtube\.com|youtu\.be'
                        ContentUrl = $item.Link
                    }
                    
                    $itemsToProcess += $processingItem
                    $newItemsCount++
                    
                    # Categorize URLs for processing
                    if (-not $processingItem.IsYouTube) {
                        $otherUrlsToFetch += $item.Link
                    }
                }
                else {
                    $skippedItemsCount++
                }
            }

            Write-Host "    📊 Found $newItemsCount new items to process and $skippedItemsCount already processed"

            # Phase 2: Download content for all new items
            $contentMap = @{}
            
            # Fetch URLs individually (with rate limiting)
            if ($otherUrlsToFetch.Count -gt 0) {
                Write-Host "    🌐 Fetching $($otherUrlsToFetch.Count) URLs individually..."
                foreach ($url in $otherUrlsToFetch) {
                    try {
                        $htmlContent = Get-ContentFromUrl -Url $url
                        if ($htmlContent -and -not [string]::IsNullOrWhiteSpace($htmlContent)) {
                            $contentMap[$url] = $htmlContent
                            Write-Host "      ✅ Retrieved content for URL: $url"
                        }
                        else {
                            Write-Host "      ⚠️ No content retrieved for URL: $url" -ForegroundColor Yellow
                        }
                        
                        # Add delay to prevent rate limiting
                        if ($otherUrlsToFetch.IndexOf($url) -lt ($otherUrlsToFetch.Count - 1)) {
                            Start-Sleep -Seconds 10
                        }
                    }
                    catch {
                        Write-Host "      ⚠️ Failed to fetch content from $url`: $($_.ToString())" -ForegroundColor Yellow
                    }
                }
                Write-Host "      ✅ Retrieved content for $($contentMap.Count) URLs"
            }

            # Phase 3: Process and save all items to disk
            foreach ($processingItem in $itemsToProcess) {
                try {
                    $item = $processingItem.Item
                    $itemFilePath = $processingItem.FilePath
                    
                    # Create item object with all feed properties and new properties
                    $itemData = @{
                        # Item properties
                        Title           = $item.Title
                        Link            = $item.Link
                        PubDate         = $item.PubDate
                        Description     = $item.Description
                        Author          = $item.Author
                        Tags            = $item.Tags
                        OutputDir       = $item.OutputDir
                        
                        # Feed properties
                        FeedName        = $feed.FeedName
                        FeedUrl         = $feed.URL
                        FeedLevelAuthor = $feed.FeedLevelAuthor
                        
                        # Enhancement properties
                        EnhancedContent = $null
                        ProcessedDate   = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
                    }

                    # Enhanced content processing
                    $contentProcessingSuccessful = $false
                    try {
                        if ($processingItem.IsYouTube) {
                            Write-Host "    📺 Processing YouTube content: $($item.Title.Substring(0, [Math]::Min(50, $item.Title.Length)))..."
                            $contentProcessingSuccessful = $true
                        }
                        elseif ($contentMap.ContainsKey($item.Link)) {
                            Write-Host "    🔍 Processing regular content: $($item.Title.Substring(0, [Math]::Min(50, $item.Title.Length)))..."
                            
                            $htmlContent = $contentMap[$item.Link]
                            $mainContent = Get-MainContentFromHtml -InputHtml $htmlContent -SourceUrl $item.Link
                            $markdownContent = Get-MarkdownFromHtml -HtmlContent $mainContent

                            if ($markdownContent -and -not [string]::IsNullOrWhiteSpace($markdownContent)) {
                                $itemData.EnhancedContent = $markdownContent
                                $enhancedCount++
                                Write-Host "      ✅ Enhanced content retrieved ($($markdownContent.Length) chars)"
                                $contentProcessingSuccessful = $true
                            }
                        }
                        else {
                            Write-Host "    ⚠️ No content available for: $($item.Title.Substring(0, [Math]::Min(50, $item.Title.Length)))..." -ForegroundColor Yellow
                        }
                    }
                    catch {
                        Write-Host "      ⚠️ Failed to process enhanced content: $($_.ToString())" -ForegroundColor Yellow
                        $errorCount++
                    }

                    # Save item only if content processing was successful
                    if ($contentProcessingSuccessful) {
                        try {
                            $itemData | ConvertTo-Json -Depth 10 | Set-Content -Path $itemFilePath -Encoding UTF8 -Force
                            $processedCount++
                            Write-Host "    💾 Saved: $($item.Title.Substring(0, [Math]::Min(50, $item.Title.Length)))..."
                        }
                        catch {
                            Write-Host "      ❌ Failed to save item to disk: $($_.ToString())" -ForegroundColor Red
                            $errorCount++
                        }
                    }
                    else {
                        Write-Host "    ❌ Skipping save due to content processing failure: $($item.Title.Substring(0, [Math]::Min(50, $item.Title.Length)))..." -ForegroundColor Red
                    }
                    
                    Write-Host "    📊 Progress: $processedCount/$($itemsToProcess.Count) items processed..."
                }
                catch {
                    $errorCount++
                    Write-Host "    ❌ Error processing item '$($processingItem.Item.Title)': $($_.ToString())" -ForegroundColor Red
                    continue
                }
            }

            Write-Host "✅ Success: $($feed.FeedName) - $($recentItems.Count) items processed"
            Write-Host "   📊 New: $newItemsCount | Skipped: $skippedItemsCount | Enhanced: $enhancedCount | Errors: $errorCount"
            $successCount++
            
            # Track this feed as successfully processed for cleanup
            $feedDirName = $feedConfig.name -replace '[^A-Za-z0-9_\-\s]', '_' -replace '\s+', '_'
            $successfulFeeds["$($feedConfig.outputDir)/$feedDirName"] = $true
        }
        catch {
            Write-Host "❌ Error processing feed '$($feedConfig.name)': $($_.Exception.Message)" -ForegroundColor Red
            Write-Host "📊 Progress after failure: $successCount/$totalCount feeds downloaded" -ForegroundColor Yellow
            # Continue with next feed instead of throwing
            continue
        }
    }

    # Clean up old files that are no longer in any feed - only for successfully processed feeds
    if ($successfulFeeds.Count -gt 0) {
        Write-Host "🧹 Cleaning up old files for $($successfulFeeds.Count) successfully processed feeds..."
        $cleanupCount = 0
        $emptyDirsRemoved = 0
        
        foreach ($outputType in $outputTypes.Keys) {
            $outputTypeDir = Join-Path $dataDir $outputType
            if (Test-Path $outputTypeDir) {
                Write-Host "  📁 Checking output type: $outputType..."
                
                # Get all feed directories in this output type
                $feedDirs = @(Get-ChildItem -Path $outputTypeDir -Directory)
                foreach ($feedDirInfo in $feedDirs) {
                    $feedKey = "$outputType/$($feedDirInfo.Name)"
                    
                    # Only process cleanup for feeds that were successfully processed
                    if ($successfulFeeds.ContainsKey($feedKey)) {
                        Write-Host "    🔍 Cleaning up successful feed: $($feedDirInfo.Name)"
                        
                        $feedDir = $feedDirInfo.FullName
                        $jsonFiles = @(Get-ChildItem -Path $feedDir -Filter "*.json")
                        
                        foreach ($jsonFile in $jsonFiles) {
                            if (-not $allCurrentItems.ContainsKey($jsonFile.FullName)) {
                                Remove-Item $jsonFile.FullName -Force
                                $cleanupCount++
                                Write-Host "      🗑️ Removed old file: $($jsonFile.Name)"
                            }
                        }
                        
                        # Remove empty feed directory if no JSON files remain
                        $remainingJsonFiles = @(Get-ChildItem -Path $feedDir -Filter "*.json")
                        if ($remainingJsonFiles.Count -eq 0) {
                            Remove-Item $feedDir -Force -Recurse
                            $emptyDirsRemoved++
                            Write-Host "      📁 Removed empty feed directory: $($feedDirInfo.Name)"
                        }
                    }
                    else {
                        Write-Host "    ⏩ Skipping cleanup for unsuccessful/unprocessed feed: $($feedDirInfo.Name)" -ForegroundColor Yellow
                    }
                }
            }
        }
        
        if ($cleanupCount -gt 0 -or $emptyDirsRemoved -gt 0) {
            Write-Host "🧹 Cleanup completed: $cleanupCount files and $emptyDirsRemoved directories removed"
        }
        else {
            Write-Host "🧹 No cleanup needed - all files are current"
        }
    } else {
        Write-Host "⏩ Skipping cleanup - no feeds were successfully processed" -ForegroundColor Yellow
    }

    Write-Host "✅ RSS feed download completed: $successCount/$totalCount feeds successful"
}
catch {
    Write-ErrorDetails -ErrorRecord $_ -Context "RSS feed download"
    throw
}
