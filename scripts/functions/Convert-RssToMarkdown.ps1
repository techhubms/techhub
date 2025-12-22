function Convert-RssToMarkdown {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory = $true)]
        [array]$Items,
        [Parameter(Mandatory = $true)]
        [string]$Token,
        [Parameter(Mandatory = $true)]
        [string]$Model,
        [Parameter(Mandatory = $false)]
        [string]$Endpoint = "https://models.github.ai/inference/chat/completions",
        [Parameter(Mandatory = $false)]
        [int]$RateLimitPreventionDelay = 15,
        [Parameter(Mandatory = $false)]
        [ref]$FailedArticleCount = ([ref]0)
    )

    $sourceRoot = Get-SourceRoot

    # Determine template based on output directory
    $scriptsPath = Join-Path $sourceRoot "scripts"
    $templateDir = Join-Path $scriptsPath "templates"
    $videoTemplatePath = Join-Path $templateDir "template-videos.md"
    $genericTemplatePath = Join-Path $templateDir "template-generic.md"

    if (-not (Test-Path $genericTemplatePath)) {
        throw "Template file not found: $genericTemplatePath"
    }
    if (-not (Test-Path $videoTemplatePath)) {
        throw "Video template file not found: $videoTemplatePath"
    }

    # Initialize counter for new markdown files created
    $newMarkdownFilesCount = 0
    
    # Initialize skipped and processed entries tracking
    $skippedEntriesPath = Join-Path $scriptsPath "skipped-entries.json"
    $processedEntriesPath = Join-Path $scriptsPath "processed-entries.json"
    $skippedEntries = Get-SkippedEntries -SkippedEntriesPath $skippedEntriesPath
    $processedEntries = Get-ProcessedEntries -ProcessedEntriesPath $processedEntriesPath
    
    # Filter items to get processing summary
    $totalItems = $Items.Count
    $itemsToSkip = @($Items | Where-Object { 
            $currentLink = $_.Link
            $skippedEntries | Where-Object { $_.canonical_url -eq $currentLink }
        })
    $itemsAlreadyProcessed = @($Items | Where-Object { 
            $currentLink = $_.Link
            $processedEntries | Where-Object { $_.canonical_url -eq $currentLink }
        })
    $itemsToProcess = @($Items | Where-Object { 
            $currentLink = $_.Link
            -not ($skippedEntries | Where-Object { $_.canonical_url -eq $currentLink }) -and
            -not ($processedEntries | Where-Object { $_.canonical_url -eq $currentLink })
        })
    
    # Display processing summary
    Write-Host "📊 Processing Summary:" -ForegroundColor Blue
    Write-Host "   Total items: $totalItems" -ForegroundColor Cyan
    Write-Host "   Items to process: $($itemsToProcess.Count)" -ForegroundColor Green
    Write-Host "   Items ignored: $($itemsToSkip.Count)" -ForegroundColor Yellow
    Write-Host "   Items already processed: $($itemsAlreadyProcessed.Count)" -ForegroundColor Gray
    Write-Host ""

    foreach ($item in $itemsToProcess) {
        try {
            Write-Host "📄 Processing: $($item.Link)" -ForegroundColor Magenta

            # Determine collection from output directory
            $collection_value = $null
            if ($item.OutputDir -match '_(.+)$') {
                $collection_value = $matches[1].Trim()
            }
            if (-not $collection_value) {
                throw "Collection could not be determined from OutputDir '$($item.OutputDir)'"
            }

            # Remove old file if it already exists based on canonical_url, so we can update markdown files by removing their entries from the processed and/or skipped entries files
            $existingFiles = Get-ChildItem -Path $item.OutputDir -Filter "*.md" -ErrorAction SilentlyContinue
            foreach ($existingFile in $existingFiles) {
                try {
                    $existingContent = Get-Content -Path $existingFile.FullName -Raw -ErrorAction SilentlyContinue
                    if ($existingContent -and $existingContent -match 'canonical_url:\s*"?([^"\s]+)"?') {
                        $existingCanonicalUrl = $matches[1].Trim('"')
                        if ($existingCanonicalUrl -eq $item.Link) {
                            if ($PSCmdlet.ShouldProcess($existingFile.FullName, "Remove existing markdown file")) {
                                Remove-Item -Path $existingFile.FullName -Force
                                Write-Host "Removing existing file with same canonical_url: $($existingFile.FullName)"
                            }
                            else {
                                Write-Host "What if: Would remove existing file with same canonical_url: $($existingFile.FullName)"
                            }
                            break
                        }
                    }
                }
                catch {
                    Write-Host "Warning: Could not check existing file $($existingFile.FullName): $($_.Exception.Message)" -ForegroundColor Yellow
                }
            }

            # Apply content length check only for community data
            $lengthRequirement = if ($collection_value -eq "community") { 1000 } else { 0 }
            if ($item.EnhancedContent -and $item.EnhancedContent.Length -lt $lengthRequirement) {
                Write-Host "Skipping item due to insufficient content length: $($item.Link)" -ForegroundColor Yellow
                
                # Write canonical URL to skipped-entries.json to avoid reprocessing
                Add-TrackingEntry -EntriesPath $skippedEntriesPath -CanonicalUrl $item.Link -Reason "Insufficient content length" -Collection $collection_value
                
                continue
            }

            $description = $item.Description
            $content = $item.EnhancedContent

            if (-not $item.Tags) {
                $item.Tags = @()
            }

            $inputData = @{
                title       = $item.Title
                description = $description
                content     = $content
                author      = $item.Author
                tags        = $item.Tags -join ', '
                type        = $collection_value
            }

            $response = Invoke-ProcessWithAiModel `
                -Token $Token `
                -Model $Model `
                -InputData $inputData `
                -Endpoint $Endpoint `
                -RateLimitPreventionDelay $RateLimitPreventionDelay
        
            # Check for errors that we can return (rate limit or content filter)
            if ($response.PSObject.Properties.Name -contains 'Error' -and $response.Error -eq $true) {
                if ($response.Type -eq "RateLimit") {
                    # Rate limit reached, stop processing
                    Write-Host "Rate limit reached, please wait $($response.RateLimitSeconds) seconds" -ForegroundColor Yellow

                    return $newMarkdownFilesCount
                }
                elseif ($response.Type -eq "ContentFilter") {
                    Write-Host "Content filter error from pattern $($response.Pattern)" -ForegroundColor Yellow
                    # Add to skipped entries and continue to next item
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -CanonicalUrl $item.Link -Reason "Content blocked by safety filters" -Collection $collection_value
                    
                    continue
                }
                elseif ($response.Type -eq "RequestEntityTooLarge") {
                    Write-Host "Too many tokens for GPT 4.1" -ForegroundColor Yellow
                    # Add to skipped entries and continue to next item
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -CanonicalUrl $item.Link -Reason "Too many tokens in the request" -Collection $collection_value
                    
                    continue
                }
                elseif ($response.Type -eq "JsonParseError") {
                    Write-Host "AI model response could not be parsed as JSON" -ForegroundColor Yellow
                
                    # Save the AI result for debugging purposes
                    Save-GitHubModelsResult -InputData $inputData -Response $response -Url $item.Link -Model $Model -PubDate $item.PubDate -AiResultsPath (Join-Path $scriptsPath "airesults")
                
                    # Add to skipped entries and continue to next item
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -CanonicalUrl $item.Link -Reason "AI model response could not be parsed as JSON" -Collection $collection_value
                    
                    continue
                }
                elseif ($response.Type -eq "ResponseParseError") {
                    Write-Host "AI API response could not be parsed" -ForegroundColor Yellow
                
                    # Add to skipped entries and continue to next item
                    $errorMessage = if ($response.PSObject.Properties.Name -contains 'Message') { $response.Message } else { "Response parse error" }
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -CanonicalUrl $item.Link -Reason "AI API response parse error: $errorMessage" -Collection $collection_value
                    
                    continue
                }
                elseif ($response.Type -eq "AllRetriesFailed") {
                    Write-Host "All API call retries failed" -ForegroundColor Yellow
                
                    # Add to skipped entries and continue to next item
                    $errorMessage = if ($response.PSObject.Properties.Name -contains 'Message') { $response.Message } else { "All retries failed" }
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -CanonicalUrl $item.Link -Reason "API call retries failed: $errorMessage" -Collection $collection_value
                    
                    continue
                }
                else {
                    # Create error message using available properties
                    $errorDetails = @()
                    $errorDetails += "Type: $($response.Type)"
                
                    if ($response.PSObject.Properties.Name -contains 'Message') {
                        $errorDetails += "Message: $($response.Message)"
                    }
                    if ($response.PSObject.Properties.Name -contains 'RateLimitSeconds') {
                        $errorDetails += "RateLimitSeconds: $($response.RateLimitSeconds)"
                    }
                    if ($response.PSObject.Properties.Name -contains 'Pattern') {
                        $errorDetails += "Pattern: $($response.Pattern)"
                    }
                
                    $errorMessage = $errorDetails -join ", "
                    throw "Unknown error type: $errorMessage"
                }
            }

            $categories = @()
            if ($response.PSObject.Properties.Name -contains 'categories' -and $response.categories -and $response.categories.Count -gt 0) {
                $categories += $response.categories
            }
            $categories = @($categories | Sort-Object -Unique)

            if (-not $categories -or $categories.Count -eq 0) {
                Write-Host "No categories found, skipping item" -ForegroundColor Yellow

                # Write canonical URL to skipped-entries.json to avoid reprocessing
                $explanation = if ($response.PSObject.Properties.Name -contains 'explanation') { $response.explanation } else { "" }
                $finalReason = if ($explanation -and $explanation.Trim() -ne "") {
                    "No categories found: $explanation"
                }
                else {
                    "No categories found"
                }
            
                Add-TrackingEntry -EntriesPath $skippedEntriesPath -CanonicalUrl $item.Link -Reason $finalReason -Collection $collection_value

                continue;
            }
            else {
                Write-Host "Categories found: $($categories -join ', ')" -ForegroundColor Green
            }        

            $tags = $item.Tags
            if ($response.tags -and $response.tags.Count -gt 0) {
                $tags += $response.tags
            }
        
            $tagResult = Get-FilteredTags -Tags $tags -Categories $categories -Collection $collection_value
            $tags = $tagResult.tags
            $tags_normalized = $tagResult.tags_normalized

            # Select template based on output directory
            $templatePath = $genericTemplatePath
            $youtubeId = ''
        
            if ($item.OutputDir -eq '_videos') {
                $templatePath = $videoTemplatePath

                # Extract YouTube video ID using the comprehensive function
                $youtubeId = Get-YouTubeVideoId -Url $item.Link
            }

            # Format date and fix timezone format from +0000 to +00:00 for Jekyll compatibility
            $dateFormatted = $item.PubDate.ToString("yyyy-MM-dd HH:mm:ss zzz")
            $dateFormatted = $dateFormatted -replace '(\+|-)(\d{2})(\d{2})(?!:)', '$1$2:$3'
        
            # Generate proper filename with date and .md extension
            $fileNamePubDate = $item.PubDate.ToString("yyyy-MM-dd")
            $fileNameTitle = (ConvertTo-SafeFilename -Title $response.title -MaxLength 200)
            $filename = "$fileNamePubDate-$fileNameTitle.md"
            $permalink = "$fileNamePubDate-$fileNameTitle.html"

            $markdownContent = Get-Content $templatePath -Raw
            $markdownContent = $markdownContent -replace '{{TITLE}}', (Format-FrontMatterValue -Value $response.title)
            $markdownContent = $markdownContent -replace '{{AUTHOR}}', (Format-FrontMatterValue -Value $item.Author)
            $markdownContent = $markdownContent -replace '{{DESCRIPTION}}', (Format-FrontMatterValue -Value $response.description)
            $markdownContent = $markdownContent -replace '{{TAGS}}', (Format-FrontMatterValue -Value @($tags))
            $markdownContent = $markdownContent -replace '{{TAGS_NORMALIZED}}', (Format-FrontMatterValue -Value @($tags_normalized))
            $markdownContent = $markdownContent -replace '{{FEEDNAME}}', (Format-FrontMatterValue -Value $item.FeedName)
            $markdownContent = $markdownContent -replace '{{CATEGORIES}}', (Format-FrontMatterValue -Value @($categories))
            $markdownContent = $markdownContent -replace '{{PERMALINK}}', (Format-FrontMatterValue -Value $permalink)
            $markdownContent = $markdownContent -replace '{{CANONICAL_URL_FORMATTED}}', (Format-FrontMatterValue -Value $item.Link)
            $markdownContent = $markdownContent -replace '{{CANONICAL_URL}}', $item.Link
            $markdownContent = $markdownContent -replace '{{FEEDURL}}', $item.FeedUrl
            $markdownContent = $markdownContent -replace '{{CONTENT}}', $response.content
            $markdownContent = $markdownContent -replace '{{EXCERPT}}', $response.excerpt
            $markdownContent = $markdownContent -replace '{{YOUTUBE_ID}}', $youtubeId
            $markdownContent = $markdownContent -replace '{{DATE}}', $dateFormatted

            # For community Reddit posts, check for cross-posts after successful AI categorization and remove all of them.
            # If our prompt is working correct, any cross posts that only link to the original post should not be categorized so we'll only keep the real post.
            # If any crossposts DO get categorized, this code will make sure we still only have 1 post in the end.
            if ($collection_value -eq 'community' -and $item.Link -like '*reddit.com*') {
                $existingFiles = Get-ChildItem -Path $item.OutputDir -Filter "*.md" -ErrorAction SilentlyContinue
                foreach ($existingFile in $existingFiles) {
                    try {
                        $existingContent = Get-Content -Path $existingFile.FullName -Raw -ErrorAction SilentlyContinue
                        if ($existingContent -and $existingContent -match 'canonical_url:\s*"?([^"\s]+)"?') {
                            $existingCanonicalUrl = $matches[1].Trim('"')
                        
                            # Check for Reddit cross-posts (same title in URL)
                            if ($existingCanonicalUrl -like '*reddit.com*') {
                                # Extract the title part from Reddit URLs
                                # Reddit URLs follow pattern: /r/subreddit/comments/postid/title_slug/
                                # We want to compare the title_slug part
                                $normalizedCurrentUrl = $item.Link.TrimEnd('/')
                                $normalizedExistingUrl = $existingCanonicalUrl.TrimEnd('/')
                                $currentUrlParts = $normalizedCurrentUrl -split '/'
                                $existingUrlParts = $normalizedExistingUrl -split '/'
                            
                                # Find the title part in Reddit URLs (usually after comments/postid/)
                                $currentTitle = $null
                                $existingTitle = $null
                            
                                # Look for pattern: .../comments/postid/title_slug
                                for ($i = 0; $i -lt $currentUrlParts.Length - 1; $i++) {
                                    if ($currentUrlParts[$i] -eq 'comments' -and $i + 2 -lt $currentUrlParts.Length) {
                                        $currentTitle = $currentUrlParts[$i + 2]
                                        break
                                    }
                                }
                            
                                for ($i = 0; $i -lt $existingUrlParts.Length - 1; $i++) {
                                    if ($existingUrlParts[$i] -eq 'comments' -and $i + 2 -lt $existingUrlParts.Length) {
                                        $existingTitle = $existingUrlParts[$i + 2]
                                        break
                                    }
                                }
                            
                                # If we found titles in both URLs and they match, this is likely a cross-post
                                if ($currentTitle -and $existingTitle -and $currentTitle -eq $existingTitle) {
                                    Write-Host "Removing existing Reddit cross-post with same title '$currentTitle': $($existingFile.FullName)"
                                    if ($PSCmdlet.ShouldProcess($existingFile.FullName, "Remove existing markdown file")) {
                                        Remove-Item -Path $existingFile.FullName -Force
                                        Write-Host "Removed file: $($existingFile.FullName)"
                                    }
                                    else {
                                        Write-Host "What if: Would remove existing file $($existingFile.FullName)"
                                    }
                                    break
                                }
                            }
                        }
                    }
                    catch {
                        Write-Host "Warning: Could not check existing file $($existingFile.FullName): $($_.Exception.Message)" -ForegroundColor Yellow
                    }
                }
            }

            # Create the file
            $filePath = Join-Path $item.OutputDir $filename
            if ($PSCmdlet.ShouldProcess($filePath, "Save markdown file")) {
                # Save content
                Set-Content -Path $filePath -Value $markdownContent -Encoding UTF8 -Force
                Write-Host "✅ Created file: $filePath" -ForegroundColor Green

                # Add to processed entries
                $explanation = if ($response.PSObject.Properties.Name -contains 'explanation') { $response.explanation } else { "" }
                $finalReason = if ($explanation -and $explanation.Trim() -ne "") {
                    "Succesfully added: $explanation"
                }
                else {
                    "Succesfully added"
                }
                Add-TrackingEntry -EntriesPath $processedEntriesPath -CanonicalUrl $item.Link -Collection $collection_value -Reason $finalReason

                # Fix markdown immediately
                Repair-MarkdownJekyll -FilePath $filePath
                Repair-MarkdownFormatting -FilePath $filePath
            }
            else {
                Write-Host "What if: Would save markdown file to $filePath"
            }

            # Count both real and simulated file creation
            $newMarkdownFilesCount++
        }
        catch {
            # Determine if this is a retryable processing failure or a logic/validation error
            $errorMessage = $_.Exception.Message
            
            # Logic errors and validation errors should still fail the workflow immediately
            if ($errorMessage -like "*Unknown error type*" -or 
                $errorMessage -like "*Collection could not be determined*" -or
                $errorMessage -like "*Template file not found*") {
                # These are logic/configuration errors that should fail fast
                throw
            }
            
            # Network errors, rate limiting, and API failures should be handled gracefully
            if ($errorMessage -like "*429*" -or 
                $errorMessage -like "*Too Many Requests*" -or
                $errorMessage -like "*Response status code does not indicate success*" -or
                $errorMessage -like "*rate limit*" -or
                $errorMessage -like "*network*" -or 
                $errorMessage -like "*timeout*" -or
                $errorMessage -like "*HttpResponseException*" -or
                $errorMessage -like "*HttpRequestException*") {
                
                # Handle individual article processing failures
                $FailedArticleCount.Value++
                
                Write-Host "❌ Failed to process article: $($item.Link)" -ForegroundColor Red
                Write-Host "   Error: $errorMessage" -ForegroundColor Red
                Write-Host "   Failed articles so far: $($FailedArticleCount.Value)" -ForegroundColor Yellow
                
                # Check if we've hit the failure threshold
                if ($FailedArticleCount.Value -ge 3) {
                    Write-Host "💥 Workflow failure threshold reached: $($FailedArticleCount.Value) articles have failed" -ForegroundColor Red
                    Write-Host "   Failing the entire workflow to prevent further issues" -ForegroundColor Red
                    throw "Too many article processing failures ($($FailedArticleCount.Value)). Last failure: $errorMessage"
                }
                
                # Continue processing remaining articles
                Write-Host "   Continuing with next article..." -ForegroundColor Yellow
            }
            else {
                # Unknown error types should still fail immediately for debugging
                throw
            }
        }
    }
    
    return $newMarkdownFilesCount
}
