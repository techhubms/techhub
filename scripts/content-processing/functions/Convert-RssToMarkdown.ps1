function Convert-RssToMarkdown {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory = $true)]
        [array]$Items,
        [Parameter(Mandatory = $true)]
        [string]$Token,
        [Parameter(Mandatory = $true)]
        [string]$Model,
        [Parameter(Mandatory = $true)]
        [string]$Endpoint,
        [Parameter(Mandatory = $false)]
        [int]$RateLimitPreventionDelay = 15,
        [Parameter(Mandatory = $false)]
        [ref]$FailedArticleCount = ([ref]0)
    )

    $sourceRoot = Get-SourceRoot

    # Determine template based on output directory
    $scriptsPath = Join-Path $sourceRoot "scripts" "content-processing"
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
    $dataPath = Join-Path $sourceRoot "scripts/data"
    $skippedEntriesPath = Join-Path $dataPath "skipped-entries.json"
    $processedEntriesPath = Join-Path $dataPath "processed-entries.json"
    $skippedEntries = Get-SkippedEntries -SkippedEntriesPath $skippedEntriesPath
    $processedEntries = Get-ProcessedEntries -ProcessedEntriesPath $processedEntriesPath
    
    # Filter items to get processing summary
    $totalItems = $Items.Count
    $itemsToSkip = @($Items | Where-Object { 
            $currentLink = $_.Link
            $skippedEntries | Where-Object { $_.external_url -eq $currentLink }
        })
    $itemsAlreadyProcessed = @($Items | Where-Object { 
            $currentLink = $_.Link
            $processedEntries | Where-Object { $_.external_url -eq $currentLink }
        })
    $itemsToProcess = @($Items | Where-Object { 
            $currentLink = $_.Link
            -not ($skippedEntries | Where-Object { $_.external_url -eq $currentLink }) -and
            -not ($processedEntries | Where-Object { $_.external_url -eq $currentLink })
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

            # Remove old file if it already exists based on external_url, so we can update markdown files by removing their entries from the processed and/or skipped entries files
            $existingFiles = Get-ChildItem -Path $item.OutputDir -Filter "*.md" -ErrorAction SilentlyContinue
            foreach ($existingFile in $existingFiles) {
                try {
                    $existingContent = Get-Content -Path $existingFile.FullName -Raw -ErrorAction SilentlyContinue
                    if ($existingContent -and $existingContent -match 'external_url:\s*"?([^"\s]+)"?') {
                        $existingExternalUrl = $matches[1].Trim('"')
                        if ($existingExternalUrl -eq $item.Link) {
                            if ($PSCmdlet.ShouldProcess($existingFile.FullName, "Remove existing markdown file")) {
                                Remove-Item -Path $existingFile.FullName -Force
                                Write-Host "Removing existing file with same external_url: $($existingFile.FullName)"
                            }
                            else {
                                Write-Host "What if: Would remove existing file with same external_url: $($existingFile.FullName)"
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
                
                # Write external URL to skipped-entries.json to avoid reprocessing
                Add-TrackingEntry -EntriesPath $skippedEntriesPath -ExternalUrl $item.Link -Reason "Insufficient content length" -Collection $collection_value
                
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
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -ExternalUrl $item.Link -Reason "Content blocked by safety filters" -Collection $collection_value
                    
                    continue
                }
                elseif ($response.Type -eq "RequestEntityTooLarge") {
                    Write-Host "Too many tokens for GPT 4.1" -ForegroundColor Yellow
                    # Add to skipped entries and continue to next item
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -ExternalUrl $item.Link -Reason "Too many tokens in the request" -Collection $collection_value
                    
                    continue
                }
                elseif ($response.Type -eq "JsonParseError") {
                    Write-Host "AI model response could not be parsed as JSON" -ForegroundColor Yellow
                
                    # Save the AI result for debugging purposes
                    Save-AiApiResult -InputData $inputData -Response $response -Url $item.Link -Model $Model -PubDate $item.PubDate -AiResultsPath (Join-Path $scriptsPath "airesults")
                
                    # Add to skipped entries and continue to next item
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -ExternalUrl $item.Link -Reason "AI model response could not be parsed as JSON" -Collection $collection_value
                    
                    continue
                }
                elseif ($response.Type -eq "ResponseParseError") {
                    Write-Host "AI API response could not be parsed" -ForegroundColor Yellow
                
                    # Add to skipped entries and continue to next item
                    $errorMessage = if ($response.PSObject.Properties.Name -contains 'Message') { $response.Message } else { "Response parse error" }
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -ExternalUrl $item.Link -Reason "AI API response parse error: $errorMessage" -Collection $collection_value
                    
                    continue
                }
                elseif ($response.Type -eq "AllRetriesFailed") {
                    Write-Host "All API call retries failed" -ForegroundColor Yellow
                
                    # Add to skipped entries and continue to next item
                    $errorMessage = if ($response.PSObject.Properties.Name -contains 'Message') { $response.Message } else { "All retries failed" }
                    Add-TrackingEntry -EntriesPath $skippedEntriesPath -ExternalUrl $item.Link -Reason "API call retries failed: $errorMessage" -Collection $collection_value
                    
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

                # Write external URL to skipped-entries.json to avoid reprocessing
                $explanation = if ($response.PSObject.Properties.Name -contains 'explanation') { $response.explanation } else { "" }
                $finalReason = if ($explanation -and $explanation.Trim() -ne "") {
                    "No categories found: $explanation"
                }
                else {
                    "No categories found"
                }
            
                Add-TrackingEntry -EntriesPath $skippedEntriesPath -ExternalUrl $item.Link -Reason $finalReason -Collection $collection_value

                continue;
            }
            else {
                Write-Host "Categories found: $($categories -join ', ')" -ForegroundColor Green
            }        

            # Convert categories (display names) to section_names (normalized identifiers)
            # Mapping: 'AI' → 'ai', 'GitHub Copilot' → 'github-copilot', '.NET' → 'dotnet', etc.
            $sectionNameMapping = @{
                'AI'             = 'ai'
                'Azure'          = 'azure'
                'GitHub Copilot' = 'github-copilot'
                '.NET'           = 'dotnet'
                'DevOps'         = 'devops'
                'Security'       = 'security'
                'Coding'         = 'coding'
                'Cloud'          = 'cloud'
            }
            
            $section_names = @()
            foreach ($category in $categories) {
                if ($sectionNameMapping.ContainsKey($category)) {
                    $section_names += $sectionNameMapping[$category]
                }
                else {
                    # Fallback: convert to lowercase and replace spaces with hyphens
                    $normalizedName = $category.ToLower() -replace '\s+', '-' -replace '[^a-z0-9-]', ''
                    $section_names += $normalizedName
                    Write-Host "Warning: Unknown category '$category', normalized to '$normalizedName'" -ForegroundColor Yellow
                }
            }
            $section_names = @($section_names | Sort-Object -Unique)

            $tags = $item.Tags
            if ($response.tags -and $response.tags.Count -gt 0) {
                $tags += $response.tags
            }
        
            $tags = Get-FilteredTags -Tags $tags -Categories $categories -Collection $collection_value
            # tags_normalized is deprecated - normalization happens in .NET code, not stored in frontmatter

            # Select template based on output directory
            $templatePath = $genericTemplatePath
        
            if ($item.OutputDir -eq '_videos') {
                $templatePath = $videoTemplatePath
            }

            # Format date and fix timezone format from +0000 to +00:00
            $dateFormatted = $item.PubDate.ToString("yyyy-MM-dd HH:mm:ss zzz")
            $dateFormatted = $dateFormatted -replace '(\+|-)(\d{2})(\d{2})(?!:)', '$1$2:$3'
        
            # Generate proper filename with date and .md extension
            $fileNamePubDate = $item.PubDate.ToString("yyyy-MM-dd")
            $fileNameTitle = (ConvertTo-SafeFilename -Title $response.title -MaxLength 200)
            $filename = "$fileNamePubDate-$fileNameTitle.md"

            # Build frontmatter hashtable
            $frontMatter = @{
                title         = $response.title
                author        = $item.Author
                external_url  = $item.Link
                viewing_mode  = if ($collection_value -eq "videos") { "internal" } else { "external" }
                feed_name     = $item.FeedName
                date          = $dateFormatted
                tags          = @($tags)
                section_names = @($section_names)
            }
            
            # Convert frontmatter to YAML using YamlDotNet (same library as ContentFixer)
            $yamlFrontMatter = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter

            # Load template and replace placeholders
            $markdownContent = Get-Content $templatePath -Raw
            $markdownContent = $markdownContent -replace '{{FRONTMATTER}}', $yamlFrontMatter
            $markdownContent = $markdownContent -replace '{{EXTERNAL_URL}}', $item.Link
            $markdownContent = $markdownContent -replace '{{CONTENT}}', $response.content
            $markdownContent = $markdownContent -replace '{{EXCERPT}}', $response.excerpt

            # Create the file
            $filePath = Join-Path $item.OutputDir $filename
            
            # Ensure OutputDir is relative to source root (convert _news to collections/_news)
            if ($item.OutputDir -match '^_(.+)$') {
                $collectionName = $matches[1]
                $fullOutputDir = Join-Path $sourceRoot "collections" "_$collectionName"
                $filePath = Join-Path $fullOutputDir $filename
            }
            
            if ($PSCmdlet.ShouldProcess($filePath, "Save markdown file")) {
                # Save content
                Set-Content -Path $filePath -Value $markdownContent -Encoding UTF8 -Force
                Write-Host "✅ Created file: $filePath" -ForegroundColor Green

                # Fix markdown formatting only - new files already have correct .NET frontmatter
                Repair-MarkdownFormatting -FilePath $filePath
                
                # Verify file was created successfully before adding to processed entries
                if (Test-Path $filePath) {
                    # Add to processed entries ONLY after successful file creation and repair
                    $explanation = if ($response.PSObject.Properties.Name -contains 'explanation') { $response.explanation } else { "" }
                    $finalReason = if ($explanation -and $explanation.Trim() -ne "") {
                        "Succesfully added: $explanation"
                    }
                    else {
                        "Succesfully added"
                    }
                    Add-TrackingEntry -EntriesPath $processedEntriesPath -ExternalUrl $item.Link -Collection $collection_value -Reason $finalReason
                }
                else {
                    Write-Host "⚠️  File creation verification failed, not adding to processed entries" -ForegroundColor Yellow
                    throw "File was not created at expected path: $filePath"
                }
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
