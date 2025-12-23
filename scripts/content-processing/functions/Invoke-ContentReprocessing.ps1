function Invoke-ContentReprocessing {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Token,
        
        [Parameter(Mandatory = $false)]
        [string[]]$Categories = @(),
        
        [Parameter(Mandatory = $false)]
        [string[]]$Collections = @(),
        
        [Parameter(Mandatory = $true)]
        [string]$Model,
        
        [Parameter(Mandatory = $true)]
        [string]$Endpoint,
        
        [Parameter(Mandatory = $false)]
        [int]$MaxFiles = 0,  # 0 = no limit
        
        [Parameter(Mandatory = $false)]
        [switch]$DryRun,
        
        [Parameter(Mandatory = $false)]
        [int]$RateLimitDelay = 15,
        
        [Parameter(Mandatory = $false)]
        [string]$RestartFromFile = ""
    )

    # Load required functions
    $scriptRoot = $PSScriptRoot
    $functionsToLoad = @(
        "Invoke-AiApiCall.ps1",
        "Get-FrontMatterValue.ps1",
        "Set-FrontMatterValue.ps1"
    )
    
    foreach ($functionFile in $functionsToLoad) {
        $functionPath = Join-Path $scriptRoot $functionFile
        if (!(Test-Path $functionPath)) {
            Write-Error "Function file not found: $functionPath"
            return
        }
        . $functionPath
    }

    # Load system message
    $SystemMessagePath = "$scriptRoot/../system-message-for-reprocessing.md"
    if (!(Test-Path $SystemMessagePath)) {
        Write-Error "System message file not found: $SystemMessagePath"
        return
    }

    $SystemMessage = Get-Content $SystemMessagePath -Raw

    # Check for existing rate limit before starting processing
    $rateLimitInEffect = Test-RateLimitInEffect
    if ($rateLimitInEffect) {
        Write-Host "⚠️ Rate limit in effect, stopping content reprocessing" -ForegroundColor Yellow
        return @{
            TotalScanned = 0
            Skipped      = 0
            Processed    = 0
            Updated      = 0
            Removed      = 0
            Errors       = 1
            DryRun       = $DryRun.IsPresent
            Message      = "Rate limit in effect"
        }
    }

    Write-Host "🔍 Starting content reprocessing..." -ForegroundColor Green
    Write-Host "   Categories filter: $($Categories -join ', ')" -ForegroundColor Cyan
    Write-Host "   Collections filter: $($Collections -join ', ')" -ForegroundColor Cyan
    Write-Host "   Model: $Model" -ForegroundColor Cyan
    Write-Host "   Dry run: $DryRun" -ForegroundColor Cyan
    if ($RestartFromFile) {
        Write-Host "   Restart from file: $RestartFromFile" -ForegroundColor Cyan
    }

    # Get workspace root
    $WorkspaceRoot = Split-Path (Split-Path (Split-Path $scriptRoot -Parent) -Parent) -Parent

    # Initialize counters
    $ProcessedCount = 0
    $UpdatedCount = 0
    $RemovedCount = 0
    $ErrorCount = 0
    $TotalFileCount = 0
    $SkippedCount = 0

    # Initialize restart flag
    $ShouldStartProcessing = if ($RestartFromFile) { $false } else { $true }

    # Get all collection directories
    $collectionDirs = Get-ChildItem -Path $WorkspaceRoot -Directory -Filter "_*" | Where-Object { $_.Name -notin @('_site', '_sass') }
    Write-Host "📁 Scanning collection directories: $($collectionDirs.Name -join ', ')" -ForegroundColor Cyan

    # Loop 1: Iterate over directories
    foreach ($dir in $collectionDirs) {
        # Skip if filtering by collections and this directory is not included
        if ($Collections -and $Collections.Count -gt 0 -and $Collections -notcontains $dir.Name) {
            continue
        }
        
        Write-Host "📂 Processing directory: $($dir.Name)" -ForegroundColor Yellow
        
        # Loop 2: Iterate over files in this directory and process immediately
        $files = Get-ChildItem -Path $dir.FullName -Filter "*.md" -Recurse
        $TotalFileCount += $files.Count
        
        foreach ($file in $files) {
            # Check if we need to start processing from a specific file
            if ($RestartFromFile -and -not $ShouldStartProcessing) {
                if ($file.Name -eq $RestartFromFile) {
                    $ShouldStartProcessing = $true
                    Write-Host "📍 Found restart file: $($file.Name) - Starting processing from here" -ForegroundColor Green
                }
                else {
                    $SkippedCount++
                    Write-Host "⏭️  Skipping file: $($file.Name)" -ForegroundColor DarkGray
                    continue
                }
            }
            
            # Read file content once
            $content = Get-Content -Path $file.FullName -Raw -ErrorAction SilentlyContinue
            if (-not $content) { continue }
            
            # Parse frontmatter using the existing function
            $existingCategories = Get-FrontMatterValue -Content $content -Key "categories"
            if (-not $existingCategories) {
                continue  # Skip files without categories
            }
            
            # Check category filter if specified
            $shouldProcess = $true
            if ($Categories -and $Categories.Count -gt 0) {
                # Check if any specified category matches
                $hasMatchingCategory = $false
                foreach ($category in $Categories) {
                    if ($existingCategories -contains $category) {
                        $hasMatchingCategory = $true
                        break
                    }
                }
                $shouldProcess = $hasMatchingCategory
            }
            
            # If file matches criteria, process it immediately
            if ($shouldProcess) {
                $ProcessedCount++
                Write-Host ""
                Write-Host "📝 Processing file ${ProcessedCount}: $($file.Name)" -ForegroundColor White
                
                # Check MaxFiles limit
                if ($MaxFiles -gt 0 -and $ProcessedCount -gt $MaxFiles) {
                    Write-Host "🛑 Reached MaxFiles limit of $MaxFiles" -ForegroundColor Yellow
                    break
                }
                
                try {
                    # Extract all required fields using existing functions
                    $title = Get-FrontMatterValue -Content $content -Key "title"
                    $description = Get-FrontMatterValue -Content $content -Key "description"
                    $author = Get-FrontMatterValue -Content $content -Key "author"
                    $tags = Get-FrontMatterValue -Content $content -Key "tags"
                    $categories = Get-FrontMatterValue -Content $content -Key "categories"
                    
                    # Extract collection type from directory name
                    $type = $file.Directory.Name -replace "^_", ""
                    
                    # Extract markdown content (after frontmatter)
                    $markdownContent = ""
                    if ($content -match '(?s)^---\s*\n.*?\n---\s*\n(.*)$') {
                        $markdownContent = $matches[1]
                    }
                    
                    # Create input object for AI
                    $inputObject = @{
                        title       = $title
                        description = $description
                        content     = $markdownContent
                        author      = $author
                        tags        = if ($tags) { $tags } else { @() }
                        categories  = if ($categories) { $categories } else { @() }
                        type        = $type
                    }
                    
                    $userMessage = $inputObject | ConvertTo-Json -Depth 10
                    
                    Write-Host "   🤖 Calling AI for re-categorization..." -ForegroundColor Cyan
                    
                    # Call AI API
                    $aiResponse = Invoke-AiApiCall -Token $Token -Model $Model -SystemMessage $SystemMessage -UserMessage $userMessage -Endpoint $Endpoint -RateLimitPreventionDelay $RateLimitDelay
                    
                    # Check for rate limit after API call
                    $rateLimitInEffect = Test-RateLimitInEffect
                    if ($rateLimitInEffect) {
                        Write-Host "⚠️ Rate limit detected after API call, stopping content reprocessing" -ForegroundColor Yellow
                        Write-Host ""
                        Write-Host "🎯 Processing stopped due to rate limit!" -ForegroundColor Red
                        Write-Host "   📊 Total files scanned: $TotalFileCount" -ForegroundColor Cyan
                        if ($SkippedCount -gt 0) {
                            Write-Host "   ⏭️  Files skipped (before restart): $SkippedCount" -ForegroundColor Yellow
                        }
                        Write-Host "   📝 Files processed: $ProcessedCount" -ForegroundColor Cyan
                        Write-Host "   ✅ Files updated: $UpdatedCount" -ForegroundColor Green
                        Write-Host "   🗑️ Files removed: $RemovedCount" -ForegroundColor Red
                        Write-Host "   ❌ Errors: $ErrorCount" -ForegroundColor Red
                        Write-Host "   🛑 Stopped due to rate limit" -ForegroundColor Yellow
                        
                        return @{
                            TotalScanned = $TotalFileCount
                            Skipped      = $SkippedCount
                            Processed    = $ProcessedCount
                            Updated      = $UpdatedCount
                            Removed      = $RemovedCount
                            Errors       = $ErrorCount
                            DryRun       = $DryRun.IsPresent
                            Message      = "Stopped due to rate limit"
                        }
                    }
                    
                    if ($aiResponse) {
                        # Check if the response is an error response
                        $isErrorResponse = $false
                        try {
                            $response = $aiResponse | ConvertFrom-Json -ErrorAction SilentlyContinue
                            if ($response -and ($response.PSObject.Properties.Name -Contains 'Error' -and $response.Error -eq $true)) {
                                $isErrorResponse = $true
                                Write-Host "   ⚠️  AI API returned error response: Type=$($response.Type)" -ForegroundColor Yellow
                                if ($response.Type -eq "RateLimit") {
                                    Write-Host "      Rate limit duration: $($response.RateLimitSeconds) seconds" -ForegroundColor Yellow
                                }
                                $ErrorCount++
                            }
                        }
                        catch {
                            # If we can't parse as JSON, it might be a regular content response
                            $isErrorResponse = $false
                        }
                        
                        if (-not $isErrorResponse) {
                            try {
                                $result = $aiResponse | ConvertFrom-Json
                            
                                if ($result.categories -and $result.categories.Count -gt 0) {
                                    # Content has categories - check if they changed
                                    $newCategories = $result.categories
                                    $originalCategories = if ($categories) { $categories } else { @() }
                                
                                    $categoriesChanged = $false
                                    if ($newCategories.Count -ne $originalCategories.Count) {
                                        $categoriesChanged = $true
                                    }
                                    else {
                                        foreach ($cat in $newCategories) {
                                            if ($originalCategories -notcontains $cat) {
                                                $categoriesChanged = $true
                                                break
                                            }
                                        }
                                        foreach ($cat in $originalCategories) {
                                            if ($newCategories -notcontains $cat) {
                                                $categoriesChanged = $true
                                                break
                                            }
                                        }
                                    }
                                
                                    if ($categoriesChanged) {
                                        Write-Host "   📋 Categories changed:" -ForegroundColor Yellow
                                        Write-Host "      Old: [$($originalCategories -join ', ')]" -ForegroundColor Red
                                        Write-Host "      New: [$($newCategories -join ', ')]" -ForegroundColor Green
                                        Write-Host "   💭 Explanation: $($result.explanation)" -ForegroundColor Magenta
                                    
                                        if (!$DryRun) {
                                            # Update the categories using the existing function
                                            $newContent = Set-FrontMatterValue -Content $content -Key "categories" -Value $newCategories
                                        
                                            # Write back to file
                                            Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
                                            Write-Host "   ✅ File updated" -ForegroundColor Green
                                        }
                                        else {
                                            Write-Host "   🔍 DRY RUN - File not updated" -ForegroundColor Yellow
                                        }
                                    
                                        $UpdatedCount++
                                    }
                                    else {
                                        Write-Host "   ✓ Categories unchanged: [$($originalCategories -join ', ')]" -ForegroundColor Gray
                                    }
                                }
                                else {
                                    # Content should be excluded - remove the file
                                    Write-Host "   ❌ Content should be excluded" -ForegroundColor Red
                                    Write-Host "      Original categories: [$($originalCategories -join ', ')]" -ForegroundColor Red
                                    Write-Host "   💭 Explanation: $($result.explanation)" -ForegroundColor Magenta
                                
                                    if (!$DryRun) {
                                        # Remove the file completely
                                        Remove-Item -Path $file.FullName -Force
                                        Write-Host "   🗑️ File removed" -ForegroundColor Green
                                    }
                                    else {
                                        Write-Host "   🔍 DRY RUN - File not removed" -ForegroundColor Yellow
                                    }
                                
                                    $RemovedCount++
                                }
                            
                            }
                            catch {
                                Write-Warning "   ⚠️  Failed to parse AI response as JSON: $_"
                                Write-Host "   📄 Raw response: $aiResponse" -ForegroundColor DarkGray
                                $ErrorCount++
                            }
                        }
                        else {
                            # AI returned an error response - skip file modifications
                            Write-Host "   🚫 Skipping file modifications due to AI error" -ForegroundColor Yellow
                        }
                    }
                    else {
                        Write-Warning "   ⚠️  No valid response from AI API"
                        $ErrorCount++
                    }
                }
                catch {
                    Write-Error "   ❌ Error processing $($file.Name): $_"
                    $ErrorCount++
                }
            }
        }
        
        # Check MaxFiles limit at directory level too
        if ($MaxFiles -gt 0 -and $ProcessedCount -gt $MaxFiles) {
            break
        }
    }

    Write-Host ""
    Write-Host "🎯 Reprocessing completed!" -ForegroundColor Green
    Write-Host "   📊 Total files scanned: $TotalFileCount" -ForegroundColor Cyan
    if ($SkippedCount -gt 0) {
        Write-Host "   ⏭️  Files skipped (before restart): $SkippedCount" -ForegroundColor Yellow
    }
    Write-Host "   📝 Files processed: $ProcessedCount" -ForegroundColor Cyan
    Write-Host "   ✅ Files updated: $UpdatedCount" -ForegroundColor Green
    Write-Host "   🗑️ Files removed: $RemovedCount" -ForegroundColor Red
    Write-Host "   ❌ Errors: $ErrorCount" -ForegroundColor Red

    if ($DryRun) {
        Write-Host ""
        Write-Host "🔍 This was a DRY RUN - no files were actually modified" -ForegroundColor Yellow
        Write-Host "   Run without -DryRun to apply changes" -ForegroundColor Yellow
    }

    # Return summary object
    return @{
        TotalScanned = $TotalFileCount
        Skipped      = $SkippedCount
        Processed    = $ProcessedCount
        Updated      = $UpdatedCount
        Removed      = $RemovedCount
        Errors       = $ErrorCount
        DryRun       = $DryRun.IsPresent
    }
}
