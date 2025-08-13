<#
.SYNOPSIS
Processes RSS data from the data directory into markdown files.

.DESCRIPTION
Processes downloaded RSS data stored in collection directories within the data folder 
and converts it to markdown content. Collections are processed in priority order.
Does not handle any git operations - purely RSS data to markdown conversion.

.PARAMETER Repository
Repository in owner/repo format (e.g., "techhubms/techhub")

.PARAMETER Token
GitHub token for API operations

.PARAMETER WorkspaceDirectory
Optional. Path to the workspace directory. Defaults to the script's directory.
When running in GitHub Actions, this should be set to the GitHub workspace path.

.PARAMETER Endpoint
Optional. AI service endpoint URL. Defaults to GitHub Models.
- GitHub Models: "https://models.github.ai/inference/chat/completions"
- Azure AI Foundry: "https://<resource>.services.ai.azure.com/models/chat/completions"

.PARAMETER Model
Optional. AI model to use for content processing. Defaults to "openai/gpt-4.1".
- GitHub Models: Use format like "openai/gpt-4.1", "microsoft/phi-3-5-mini-instruct"
- Azure AI Foundry: Use the deployment name configured in your Azure resource

.PARAMETER RateLimitPreventionDelay
Optional. Delay in seconds between AI API calls to prevent rate limiting. Defaults to 15.

.EXAMPLE
./process-rss-to-markdown.ps1 "owner/repo" "ghp_token123"

.EXAMPLE
# Run from GitHub Actions with workspace directory
./process-rss-to-markdown.ps1 "owner/repo" "ghp_token123" -WorkspaceDirectory ${{ github.workspace }}

.EXAMPLE
# Use Azure AI Foundry endpoint
./process-rss-to-markdown.ps1 "owner/repo" "api_key123" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions"

.EXAMPLE
# Use different model
./process-rss-to-markdown.ps1 "owner/repo" "ghp_token123" -Model "microsoft/phi-3-5-mini-instruct"

.EXAMPLE
# Use Azure AI Foundry with specific deployment
./process-rss-to-markdown.ps1 "owner/repo" "api_key123" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions" -Model "my-gpt4-deployment"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Repository,
    
    [Parameter(Mandatory = $true)]
    [string]$Token,
    
    [Parameter(Mandatory = $false)]
    [string]$WorkspaceDirectory = $PSScriptRoot,
    
    [Parameter(Mandatory = $false)]
    [string]$Endpoint = "https://models.github.ai/inference/chat/completions",
    
    [Parameter(Mandatory = $false)]
    [string]$Model = "openai/gpt-4.1",
    
    [Parameter(Mandatory = $false)]
    [int]$RateLimitPreventionDelay = 15
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

    Write-Host "🔄 Starting RSS data processing..."
    
    $sourceRoot = Get-SourceRoot
    $dataDir = Join-Path $sourceRoot ".github/scripts/data"
    
    if (-not (Test-Path $dataDir)) {
        Write-Host "⚠️ Data directory not found: $dataDir"
        return
    }
    
    # Check rate limit first
    $rateLimitInEffect = Test-RateLimitInEffect
    if ($rateLimitInEffect) {
        Write-Host "⚠️ Rate limit in effect, skipping RSS processing"
        return
    }

    # Get collection priority order
    $collectionOrder = Get-CollectionPriorityOrder
    
    # Get all collection directories and sort by priority
    $collectionDirs = @(Get-ChildItem -Path $dataDir -Directory) | Where-Object { 
        $collectionOrder.ContainsKey($_.Name) 
    } | Sort-Object { $collectionOrder[$_.Name] }
    
    if (-not $collectionDirs -or $collectionDirs.Count -eq 0) {
        Write-Host "⚠️ No collection directories found to process"
        return
    }

    $totalNewMarkdownFiles = 0
    $successCount = 0
    $totalWebsiteCount = 0
    
    # Count total websites across all collections for progress reporting
    foreach ($collectionDir in $collectionDirs) {
        $websiteDirs = @(Get-ChildItem -Path $collectionDir.FullName -Directory)
        $totalWebsiteCount += $websiteDirs.Count
    }

    Write-Host "📊 Found $($collectionDirs.Count) collections with $totalWebsiteCount websites to process"

    foreach ($collectionDir in $collectionDirs) {
        Write-Host "📁 Processing collection: $($collectionDir.Name)" -ForegroundColor Cyan
        
        # Get all website directories within this collection
        $websiteDirs = Get-ChildItem -Path $collectionDir.FullName -Directory
        
        if (-not $websiteDirs -or $websiteDirs.Count -eq 0) {
            Write-Host "  ⚠️ No website directories found in $($collectionDir.Name)"
            continue
        }
        
        Write-Host "  📈 Processing $($websiteDirs.Count) websites in $($collectionDir.Name)"
        
        foreach ($websiteDir in $websiteDirs) {
            try {
                Write-Host "  🌐 Processing website: $($websiteDir.Name)" -ForegroundColor Blue
                
                # Get all JSON items from this website directory
                $jsonFiles = @(Get-ChildItem -Path $websiteDir.FullName -Filter "*.json")
                
                if (-not $jsonFiles -or $jsonFiles.Count -eq 0) {
                    Write-Host "    ⚠️ No JSON files found in $($websiteDir.Name)"
                    continue
                }
                
                # Load all items from JSON files
                $items = @()
                foreach ($jsonFile in $jsonFiles) {
                    try {
                        $item = Get-Content $jsonFile.FullName -Raw | ConvertFrom-Json
                        $items += $item
                    }
                    catch {
                        Write-Host "    ⚠️ Failed to parse JSON file $($jsonFile.Name): $($_.ToString())" -ForegroundColor Yellow
                        continue
                    }
                }
                
                if ($items.Count -eq 0) {
                    Write-Host "    ⚠️ No valid items found in $($websiteDir.Name)"
                    continue
                }
                
                Write-Host "    📝 Processing $($items.Count) items from $($websiteDir.Name)"
                
                $newFilesCount = Convert-RssToMarkdown `
                    -Items $items `
                    -Token $Token `
                    -Model $Model `
                    -Endpoint $Endpoint `
                    -RateLimitPreventionDelay $RateLimitPreventionDelay
                
                # Check rate limit after successful call
                $rateLimitInEffect = Test-RateLimitInEffect
                if ($rateLimitInEffect) {
                    Write-Host "⚠️ Rate limit in effect after processing website, aborting further RSS processing"
                    break
                }
                
                $totalNewMarkdownFiles += $newFilesCount
                $successCount++
                Write-Host "    ✅ Created $newFilesCount new markdown files from $($websiteDir.Name)"
            }
            catch {
                Write-Host "📊 Progress before failure: $successCount/$totalWebsiteCount websites processed, $totalNewMarkdownFiles markdown files created" -ForegroundColor Yellow
                throw
            }
        }
        
        # Check rate limit between collections
        $rateLimitInEffect = Test-RateLimitInEffect
        if ($rateLimitInEffect) {
            Write-Host "⚠️ Rate limit in effect after processing collection, aborting further RSS processing"
            break
        }
    }
    
    Write-Host "✅ RSS data processing completed: $successCount/$totalWebsiteCount websites processed"
    Write-Host "📄 Total new markdown files created: $totalNewMarkdownFiles"
}
catch {
    Write-ErrorDetails -ErrorRecord $_ -Context "RSS data processing"
    throw
}