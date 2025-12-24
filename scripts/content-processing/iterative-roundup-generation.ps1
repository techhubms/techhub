#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Generates a weekly roundup using a structured 9-step AI process for comprehensive content creation.

.DESCRIPTION
    This script creates weekly roundups using a structured 9-step process:
    1. Collect articles from the specified date range
    2A. Filter articles for developer relevance (exclude corporate content)
    2B. Analyze developer-relevant articles for summary and section categorization via AI
    3. Create well-organized stories with intelligent grouping (processes each section individually to avoid token limits)
    4. Create ongoing narrative by comparing with the previous roundup (processes each section individually)
    5. Merge all section responses and prepare for condensation
    6. Condense content as much as possible while maintaining narrative quality
    7. Generate metadata (title, tags, description, introduction)
    8. Create final markdown file with table of contents
    9. Rewrite content against writing style guidelines

.PARAMETER StartDate
    Start date for the roundup range (format: yyyy-MM-dd)

.PARAMETER EndDate
    End date for the roundup range (format: yyyy-MM-dd)

.PARAMETER Token
    Azure API Key for AI model access

.PARAMETER Model
    Required. The deployment name configured in your Azure AI Foundry resource.

.PARAMETER Endpoint
    Required. Azure AI Foundry endpoint URL.
    Example: "https://<resource>.services.ai.azure.com/models/chat/completions"

.PARAMETER ResumeFromBackup
    Path to a backup file to resume from instead of starting from scratch

.PARAMETER StartFromStep
    Step number to start from when resuming (1-9, default: 1)

.PARAMETER ResumeFromSection
    Section name to resume from in Step 3 or 4 (e.g., "AI", "Azure"). Useful if Step 3 or 4 fails partway through processing sections.

.EXAMPLE
    ./iterative-roundup-generation.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "api_key..." -Model "gpt-4.1" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions"

.EXAMPLE
    ./iterative-roundup-generation.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "api_key..." -Model "gpt-4.1" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions" -ResumeFromBackup ".tmp/roundup-debug/2025-07-21-to-2025-07-27-Step4-OngoingNarrative-20250812-1430.txt" -StartFromStep 5

.EXAMPLE
    ./iterative-roundup-generation.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "api_key..." -Model "gpt-4.1" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions" -StartFromStep 3 -ResumeFromSection "Azure"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$StartDate,
    
    [Parameter(Mandatory = $true)]
    [string]$EndDate,
    
    [Parameter(Mandatory = $true)]
    [string]$Token,
    
    [Parameter(Mandatory = $true)]
    [string]$Model,
    
    [Parameter(Mandatory = $true)]
    [string]$Endpoint,

    [Parameter(Mandatory = $false)]
    [int]$RateLimitPreventionDelay = 15,

    [Parameter(Mandatory = $false)]
    [string]$WorkspaceDirectory = $PSScriptRoot,

    [Parameter(Mandatory = $false)]
    [string]$ResumeFromBackup,

    [Parameter(Mandatory = $false)]
    [int]$StartFromStep = 1,

    [Parameter(Mandatory = $false)]
    [string]$ResumeFromSection
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Function to validate AI response format
function Test-AiResponseFormat {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Response,
        [Parameter(Mandatory = $true)]
        [string]$StepName
    )
    
    # Check if response indicates an error
    if ($Response -like "*I cannot*" -or $Response -like "*I'm unable*" -or $Response -like "*I don't have*") {
        return @{
            IsValid      = $false
            ErrorMessage = "AI indicated it cannot complete the request: $($Response.Substring(0, [Math]::Min(200, $Response.Length)))"
        }
    }
    
    # Check if response contains error JSON
    if ($Response -like "*`"Error`": true*") {
        try {
            $errorObj = $Response | ConvertFrom-Json
            Write-Host "❌ $StepName failed with error type: $($errorObj.Type)" -ForegroundColor Red
            if ($errorObj.ResponseContent) {
                Write-Host "Response content: $($errorObj.ResponseContent)" -ForegroundColor Red
            }
            return @{ IsValid = $false; ErrorType = $errorObj.Type; ErrorMessage = "$StepName failed: $($errorObj.Type)" }
        }
        catch {
            Write-Host "❌ $StepName failed but could not parse error response" -ForegroundColor Red
            Write-Host "Raw response: $Response" -ForegroundColor Red
            return @{ IsValid = $false; ErrorType = "UnparseableError"; ErrorMessage = "$StepName failed with unparseable error response" }
        }
    }
    
    # Check if response is empty or too short to be useful
    if ([string]::IsNullOrWhiteSpace($Response) -or $Response.Length -lt 10) {
        Write-Host "❌ $StepName returned empty or too short response" -ForegroundColor Red
        Write-Host "Response length: $($Response.Length)" -ForegroundColor Red
        return @{ IsValid = $false; ErrorType = "EmptyResponse"; ErrorMessage = "$StepName returned empty or too short response" }
    }
    
    # Response appears to be successful
    return @{ IsValid = $true; ErrorType = $null; ErrorMessage = $null }
}

# Function to analyze grouping quality in generated content
function Test-GroupingQuality {
    param(
        [Parameter(Mandatory = $true)]
        [string]$GeneratedContent,
        [Parameter(Mandatory = $true)]
        [string]$SectionName
    )
    
    $warnings = @()
    
    # Count topic sections (### headers)
    $topicSectionsArray = @($GeneratedContent -split "`n" | Where-Object { $_ -match "^### " })
    $totalLinksArray = @($GeneratedContent -split "`n" | Where-Object { $_ -match "^- \[.*\]\(" })
    $topicSections = $topicSectionsArray.Length
    $totalLinks = $totalLinksArray.Length
    
    if ($topicSections -gt 0) {
        $avgArticlesPerTopic = $totalLinks / $topicSections
        
        # Check for too many small groups
        if ($topicSections -gt 3 -and $avgArticlesPerTopic -lt 2.5) {
            $warnings += "⚠️ Section '$SectionName' has many small groups ($topicSections topics, avg $([Math]::Round($avgArticlesPerTopic, 1)) articles each). Consider consolidating."
        }
        
        # Check for unbalanced distribution
        if ($topicSections -eq 1 -and $totalLinks -gt 8) {
            $warnings += "⚠️ Section '$SectionName' has only 1 topic group with $totalLinks articles. Consider splitting into multiple themes."
        }
    }
    
    return $warnings
}

# Function to create backup files for debugging
function Save-StepBackup {
    param(
        [string]$StepName,
        [string]$Content,
        [string]$StartDate,
        [string]$EndDate
    )
    
    $backupDir = ".tmp/roundup-debug"
    if (-not (Test-Path $backupDir)) {
        New-Item -ItemType Directory -Path $backupDir -Force | Out-Null
    }
    
    $timestamp = Get-Date -Format "yyyyMMdd-HHmm"
    $backupFile = Join-Path $backupDir "$StartDate-to-$EndDate-$StepName-$timestamp.txt"
    $Content | Out-File -FilePath $backupFile -Encoding UTF8 -Force
    Write-Host "💾 Backup saved: $backupFile" -ForegroundColor DarkGray
}

# Function to generate consistent filename and publish date
function Get-RoundupFileInfo {
    param(
        [DateTime]$EndDateTime
    )
    
    $publishDateFormatted = $EndDateTime.AddDays(1).ToString("yyyy-MM-dd")
    $filename = "$publishDateFormatted-Weekly-AI-and-Tech-News-Roundup"
    $publishDate = $EndDateTime.AddDays(1).ToString("yyyy-MM-dd 09:00:00 +00:00")
    
    return @{
        PublishDateFormatted = $publishDateFormatted
        Filename             = $filename
        PublishDate          = $publishDate
    }
}

# Establish base paths for consistent usage throughout script
$isRunningFromScriptDirectory = ($WorkspaceDirectory -eq $PSScriptRoot)

if ($isRunningFromScriptDirectory) {
    # Running from the script's directory (scripts/)
    $WorkspaceRoot = Split-Path $PSScriptRoot -Parent
}
else {
    # Running from workspace root
    $WorkspaceRoot = $WorkspaceDirectory
}

$contentProcessingDirectory = Join-Path $WorkspaceRoot "scripts/content-processing"
$functionsPath = Join-Path $contentProcessingDirectory "functions"

# Import Write-ErrorDetails first (for error handling), then all others sorted alphabetically
. (Join-Path $functionsPath "Write-ErrorDetails.ps1")

try {
    Get-ChildItem -Path $functionsPath -Filter "*.ps1" | 
    Where-Object { $_.Name -ne "Write-ErrorDetails.ps1" } |
    ForEach-Object { . $_.FullName }

    # Validate date format and convert to DateTime objects
    try {
        $startDateTime = [DateTime]::ParseExact($StartDate, "yyyy-MM-dd", $null)
        $endDateTime = [DateTime]::ParseExact($EndDate, "yyyy-MM-dd", $null)
    }
    catch {
        Write-Error "Invalid date format. Please use yyyy-MM-dd format."
        exit 1
    }

    if ($startDateTime -gt $endDateTime) {
        Write-Error "Start date must be before or equal to end date."
        exit 1
    }

    # Create week description for AI prompts
    $WeekDescription = "the week of $($startDateTime.ToString('MMMM d')) to $($endDateTime.ToString('MMMM d, yyyy'))"

    # Define consistent writing style guidelines for all AI prompts
    $WritingStyleGuidelines = @"
- Maintain all technical accuracy and specificity
- Use clear, direct professional tone - avoid marketing buzzwords and hyperbole
- Focus on practical benefits and real-world context
- Be authentic and down-to-earth rather than sensational
- Create flowing narrative content that tells the story of developments
- Preserve narrative flow and readability
- Keep professional, down-to-earth tone throughout
- Reference progression and ongoing storylines when relevant
- Focus on concrete benefits and real-world applications
- Eliminate repetitive phrases and unnecessary elaboration
- Preserve references to previous roundups and ongoing narratives (these make content feel human-written)
- Never use horizontal separators `---`, always use proper headings. The separators will be added automatically with CSS.

🚨 CRITICAL: AVOID FLASHY/OVERSELLING LANGUAGE:
- NEVER use: "pivotal", "major breakthrough", "revolutionary", "game-changing", "transformative"
- NEVER use impact qualifiers: "significant", "major", "substantial", "dramatic", "massive"
- NEVER use intensity words: "incredible", "amazing", "groundbreaking", "cutting-edge", "paradigm-shifting"
- NEVER use marketing superlatives: "ultimate", "best-in-class", "industry-leading", "world-class"
- INSTEAD use neutral descriptors: "new", "updated", "improved", "enhanced", "additional", "latest"
- INSTEAD focus on specific capabilities: "enables X", "provides Y", "supports Z", "includes A and B"
- Be factual and specific rather than emotional and grandiose
"@

    # Generate consistent file information that will be used throughout the script
    $fileInfo = Get-RoundupFileInfo -EndDateTime $endDateTime
    $publishDateFormatted = $fileInfo.PublishDateFormatted
    $filename = $fileInfo.Filename
    $publishDate = $fileInfo.PublishDate

    # Handle resume from backup functionality
    $resumeContent = $null
    $articleSummaries = @{}  # Initialize the articleSummaries hashtable
    if ($ResumeFromBackup -and (Test-Path $ResumeFromBackup)) {
        Write-Host "📂 Resuming from backup: $ResumeFromBackup" -ForegroundColor Cyan
        $resumeContent = Get-Content -Path $ResumeFromBackup -Raw
        Write-Host "✅ Backup content loaded, starting from step $StartFromStep" -ForegroundColor Green
        
        # If starting from step 3 or later, parse the JSON backup to reconstruct articleSummaries
        if ($StartFromStep -eq 3) {
            try {
                $backupData = $resumeContent | ConvertFrom-Json
                Write-Host "📊 Reconstructing article summaries from backup..."
                
                # Convert JSON structure back to the articleSummaries hashtable
                foreach ($property in $backupData.PSObject.Properties) {
                    $sectionName = $property.Name
                    $articles = $property.Value
                    $articleSummaries[$($sectionName)] = @()
                    
                    foreach ($article in $articles) {
                        $articleSummaries[$($sectionName)] += $article
                    }
                }
                Write-Host "✅ Reconstructed $($articleSummaries.Keys.Count) sections with article summaries"
            }
            catch {
                Write-Error "Failed to parse backup file JSON: $($_.Exception.Message)"
                exit 1
            }
        }
    }
    elseif ($ResumeFromBackup) {
        Write-Error "Backup file not found: $ResumeFromBackup"
        exit 1
    }

    # Validate StartFromStep
    if ($StartFromStep -lt 1 -or $StartFromStep -gt 9) {
        Write-Error "StartFromStep must be between 1 and 9."
        exit 1
    }

    if ($ResumeFromBackup -and $StartFromStep -eq 1) {
        Write-Error "Cannot resume from backup and start from step 1. Use StartFromStep 2-9 when resuming."
        exit 1
    }

    # Skip early steps if resuming
    if ($StartFromStep -gt 1) {
        Write-Host "⏭️ Skipping steps 1-$($StartFromStep - 1) due to resume/restart parameter" -ForegroundColor Yellow
    }

    # Step 1: Collect all articles that need processing (skip if resuming)
    $articles = @()
    if ($StartFromStep -le 1) {
        Write-Host "📋 Step 1: Scanning collections for content between $StartDate and $EndDate..."

        # Get collection priority order and collect all files (exclude roundups)
        $collectionPriorityOrder = Get-CollectionPriorityOrder

        foreach ($collectionDir in $collectionPriorityOrder.Keys) {
            # Skip the roundups collection to avoid including previous roundups
            if ($collectionDir -eq "_roundups") {
                continue
            }
            
            if (Test-Path $collectionDir) {
                Write-Host "Scanning $collectionDir..."
                
                $files = @(Get-ChildItem $collectionDir -Filter "*.md" -Recurse)
                
                foreach ($file in $files) {
                    # Extract date from filename (format: YYYY-MM-DD-title.md)
                    $currentFileName = $file.Name
                    if ($currentFileName -match '^(\d{4}-\d{2}-\d{2})-') {
                        $fileDate = [DateTime]::ParseExact($matches[1], "yyyy-MM-dd", $null)
                        
                        # Check if file date is within our range (inclusive)
                        if ($fileDate -ge $startDateTime -and $fileDate -le $endDateTime) {
                            $articles += $file.FullName
                        }
                    }
                    else {
                        throw "File with invalid date format found: $currentFileName. Expected format: YYYY-MM-DD-title.md"
                    }
                }
            }
        }

        # Sort articles by filename
        if ($articles.Count -eq 0) {
            throw "No articles found in the specified date range: $StartDate to $EndDate"
        }
        
        # Create numbered list of full file paths and save to tmp
        $numberedArticleList = @()
        for ($i = 0; $i -lt $articles.Count; $i++) {
            $numberedArticleList += "$($i + 1). $($articles[$i])"
        }
        
        $step1Output = $numberedArticleList -join "`n"
        
        Save-StepBackup -StepName "Step1-ArticleList" -Content $step1Output -StartDate $StartDate -EndDate $EndDate
        Write-Host "📊 Processing $($articles.Count) articles"
    }
    else {
        Write-Host "⏭️ Skipping Step 1 (collection scanning) - resuming from backup"
    }

    # Step 2: Filter and Analyze Articles for Developer Relevance (skip if resuming from later step)
    if ($StartFromStep -le 2) {
        Write-Host "🔍 Step 2: Filtering and analyzing articles for developer relevance..."

        $step2SystemMessage = @"
ABSOLUTE CRITICAL REQUIREMENT: You must provide a complete, comprehensive response. Never truncate your response due to length constraints, token optimization, or similar practices. Always provide the full enhanced content requested!

ROLE: You are an expert technical content analyst who filters and analyzes articles for a DEVELOPER-FOCUSED tech roundup. You must first determine if content is relevant for developers, then provide detailed analysis for relevant articles.

🎯 DEVELOPER-FOCUSED CONTENT FILTER:
This roundup is specifically for developers, so you must filter out corporate/business content that isn't relevant to developers.

EXCLUDE (mark as skip_article: true):
- Corporate partnerships and business announcements (e.g., "NFL and Microsoft Expand Partnership")
- Industry reports without technical actionables (e.g., "AI in Education Report: Key Insights")
- Analyst reports and market positioning (e.g., "Microsoft Named Leader in Gartner Magic Quadrant")
- Executive interviews and corporate strategy discussions
- Business case studies without technical implementation details
- General industry trend pieces without specific developer tools/features
- Marketing announcements without concrete developer benefits
- Pure business/financial announcements
- Company acquisition news without technical implications
- Executive appointments and organizational changes

INCLUDE AND ANALYZE (mark as skip_article: false):
- New developer tools, features, and capabilities
- Technical tutorials, guides, and how-to content
- Product updates that affect developer workflows
- API announcements and SDK releases
- Technical deep-dives and implementation guidance
- Developer-focused case studies with code examples
- Technical previews and beta program announcements
- Bug fixes and technical improvements
- Migration guides and technical documentation updates
- Open source project updates and releases

🚨 CRITICAL JSON FORMATTING RULES:
- Return ONLY valid JSON - no other text before or after
- Escape ALL special characters properly in JSON strings:
  * Use \" for quotation marks within text
  * Use \\ for backslashes within text  
  * Use \n for line breaks within text
  * Use \t for tabs within text
- Keep all text content on single lines within JSON strings
- Test your JSON response mentally before providing it

CRITICAL RESPONSE FORMAT: Return ONLY a JSON object with this exact structure:

For articles to SKIP (corporate/non-developer content):
{
  "skip_article": true,
  "reasoning": "Brief explanation of why this content isn't relevant for developers",
  "title": "Article title from frontmatter"
}

For articles to INCLUDE (developer-relevant content):
{
  "skip_article": false,
  "section": "Suggested Section Name",
  "summary": "Comprehensive Summary",
  "relevance_score": 1-10,
  "developer_relevance": "high|medium|low",
  "primary_topic": "Main product/technology/concept",
  "technology_stack": "Specific technology/framework/platform involved",
  "topic_type": "announcement|tutorial|update|guide|analysis|feature|troubleshooting|case-study|news|preview|ga-release|deprecation|migration|integration|comparison",
  "target_audience": "developers|administrators|business|researchers|data-scientists|devops-engineers",
  "tags": ["tag1", "tag2", "tag3", "tag4", "tag5"],
  "impact_level": "high|medium|low",
  "time_sensitivity": "immediate|this-week|this-month|long-term",
  "reasoning": "Brief explanation of why this content is relevant for developers"
}

SECTION CATEGORIZATION - CHOOSE THE SINGLE BEST MATCH:
Choose ONE section that best represents the primary focus of the content. Use this priority order when content could fit multiple sections:

1. "GitHub Copilot": Content specifically about GitHub Copilot features, usage, integrations, or announcements
   - GitHub Copilot Chat, code completion, enterprise features
   - GitHub Copilot extensions and integrations
   - Takes priority over "AI" or "Coding" when Copilot is the main focus

2. "ML": Machine learning engineering, data science workflows, model development, AI research
   - Azure ML, data science platforms, model training and deployment
   - Advanced analytics, business intelligence development
   - Custom ML implementations, algorithm development
   - Takes priority over "AI" when technical ML development is the main focus

3. "AI": General AI services, tools, and platforms (excluding GitHub Copilot and technical ML)
   - Azure AI Foundry, Azure OpenAI, AI services and APIs, Copilot Studio
   - AI integration and usage (not technical ML development)
   - Prompt engineering, AI-powered applications
   - Only use when content is not primarily about GitHub Copilot or technical ML

4. "Azure": Microsoft cloud services, infrastructure, and platform services
   - Azure services, cloud architecture, deployment
   - ARM templates, Bicep, Terraform (if Azure) cloud management
   - Use when Azure services are the primary focus, not just mentioned

5. "Coding": .NET ecosystem, programming languages, development frameworks
   - C#, F#, .NET, ASP.NET Core, development patterns
   - Programming languages, frameworks, development tools, NuGet package manager
   - Only use when development is the main focus and not about GitHub Copilot

6. "DevOps": Development processes, CI/CD, automation, team collaboration
   - GitHub Actions, Azure DevOps, deployment pipelines
   - Development workflows, team practices, automation
   - Version control, release management

7. "Security": Cybersecurity, threats, security tools, compliance
   - Security tools, vulnerability management, compliance
   - Identity management, security architecture
   - Threat detection, incident response

SUMMARY REQUIREMENTS:
- Provide comprehensive detailed context covering all key aspects of the article
- Include specific version numbers, feature names, and technical details
- Explain what changed from previous versions or states when clear from the article
- Focus on practical implications for developers and their daily workflows
- Keep technical accuracy and specificity
- Cover the full scope of the development or announcement
- Integrate developer impact naturally within the summary, including:
  * Specific workflow improvements and practical benefits
  * Concrete benefits like time savings, new capabilities, or simplified processes
  * Reference to specific tools, IDEs, or development scenarios affected
  * Both immediate and longer-term implications for developers

RELEVANCE SCORING (1-10) - FOR DEVELOPER-FOCUSED TECH CONTENT:
Rate based on DEVELOPER IMPACT and relevance to developer workflows:
- 9-10: Critical developer tools, major SDK releases, breaking changes, essential security fixes for developers
- 7-8: Significant new features affecting developer productivity, important API updates, useful new capabilities
- 5-6: Incremental improvements, helpful tutorials, minor feature additions, maintenance updates with developer impact
- 3-4: Niche developer tools, specialized use cases, minor updates with limited developer relevance

🚨 DEVELOPER RELEVANCE ASSESSMENT:
- HIGH: Direct impact on developer workflows, new coding capabilities, essential tools and features
- MEDIUM: Useful for specific developer scenarios, incremental improvements, educational content
- LOW: Limited developer application, niche use cases, requires significant business context

IMPACT LEVEL ASSESSMENT:
- HIGH: Affects large developer populations, changes workflows significantly, major announcements
- MEDIUM: Affects specific developer segments, incremental improvements, useful capabilities
- LOW: Niche use cases, minor updates, specialized scenarios

TIME SENSITIVITY CLASSIFICATION:
- IMMEDIATE: Breaking news, critical security fixes, urgent action items
- THIS-WEEK: New releases, important updates, timely announcements
- THIS-MONTH: General improvements, educational content, planning items
- LONG-TERM: Strategic content, future roadmaps, educational materials

TOPIC TYPE EXPANDED DEFINITIONS:
- announcement: Official product or feature announcements
- tutorial: Step-by-step educational content
- update: Product updates, new versions, improvements
- guide: Best practices, how-to documentation
- analysis: Technical analysis, comparisons, insights
- feature: New feature highlights and explanations
- troubleshooting: Problem-solving and debugging content
- case-study: Real-world implementation examples
- news: Industry news, company updates, general tech news
- preview: Beta features, previews, early access content
- ga-release: General availability releases, official launches
- deprecation: End-of-life announcements, migration notices
- migration: Migration guides, upgrade instructions
- integration: Integration tutorials, ecosystem connections
- comparison: Product comparisons, technology evaluations

TAGS REQUIREMENTS:
- Provide a comprehensive list of all relevant tags (combining original tags from frontmatter plus additional relevant tags)
- Focus on: product families, technology stacks, use cases, developer roles, specific features mentioned
- Include 10-20 tags total that would help with content grouping and discovery

CRITICAL WRITING STYLE GUIDELINES:
$WritingStyleGuidelines
"@

        # Save Step 2 input for debugging
        $step2Input = @"
STEP 2 INPUT: Combined Filter and Analysis Configuration
AI Model: $Model
Endpoint: $Endpoint
Date Range: $StartDate to $EndDate
Total Articles to Process: $($articles.Count)

SYSTEM MESSAGE:
$step2SystemMessage
"@
        Save-StepBackup -StepName "Step2-Input" -Content $step2Input -StartDate $StartDate -EndDate $EndDate

        # Process each article for filtering and analysis
        $includedCount = 0
        $excludedCount = 0
        for ($i = 0; $i -lt $articles.Count; $i++) {
            $articleFilePath = $articles[$i]
            $articleNum = $i + 1
            $articleName = Split-Path $articleFilePath -Leaf
        
            Write-Host "🔄 Processing article $articleNum of $($articles.Count): $($articleName)"
        
            # Check if file exists
            if (-not (Test-Path $articleFilePath)) {
                throw "Article file not found: $articleFilePath"
            }
        
            # Read the article content
            $articleContent = Get-Content $articleFilePath -Raw

            $step2UserMessage = @"
ARTICLE CONTENT:
$articleContent
"@
            
            # Call the AI model for filtering and analysis
            Write-Host "🤖 Calling AI model for filtering and analysis..."
            $response = Invoke-AiApiCall `
                -Token $Token `
                -Model $Model `
                -SystemMessage $step2SystemMessage `
                -UserMessage $step2UserMessage `
                -Endpoint $Endpoint `
                -RateLimitPreventionDelay $RateLimitPreventionDelay

            # Check for errors
            $analysisResult = Test-AiResponseFormat -Response $response -StepName "Step 2 (Filter and analysis for $articleName)"
            if (-not $analysisResult.IsValid) {
                throw $analysisResult.ErrorMessage
            }

            # Parse AI response
            try {
                $result = $response | ConvertFrom-Json
                
                # Validate required fields
                if ($null -eq $result.skip_article -or -not $result.reasoning) {
                    throw "Invalid response for $($articleName): Missing required fields (skip_article, reasoning)"
                }
                
                # Check if article should be skipped
                if ($result.skip_article -eq $true) {
                    $excludedCount++
                    Write-Host "  ❌ EXCLUDED - Corporate/non-developer content" -ForegroundColor Yellow
                    Write-Host "  📝 Reasoning: $($result.reasoning)" -ForegroundColor Yellow
                }
                else {
                    # Validate analysis fields for included articles
                    if (-not $result.section -or -not $result.summary -or -not $result.relevance_score) {
                        throw "Invalid response for $($articleName): Missing required analysis fields (section, summary, relevance_score)"
                    }
                    
                    $includedCount++
                    Write-Host "  ✅ INCLUDED - Relevance: $($result.developer_relevance)"
                    Write-Host "  📝 Reasoning: $($result.reasoning)"
                    
                    # Extract original title and link from frontmatter using existing function
                    $canonicalUrl = Get-FrontMatterValue -Content $articleContent -Key "canonical_url"
                    $title = Get-FrontMatterValue -Content $articleContent -Key "title"
                    $viewingMode = Get-FrontMatterValue -Content $articleContent -Key "viewing_mode"
                    $permalink = Get-FrontMatterValue -Content $articleContent -Key "permalink"
                    
                    $result | Add-Member -NotePropertyName "canonical_url" -NotePropertyValue $canonicalUrl
                    $result | Add-Member -NotePropertyName "title" -NotePropertyValue $title
                    $result | Add-Member -NotePropertyName "viewing_mode" -NotePropertyValue $viewingMode
                    $result | Add-Member -NotePropertyName "permalink" -NotePropertyValue $permalink
                    $result | Add-Member -NotePropertyName "filename" -NotePropertyValue $articleFilePath
                    
                    # Add to appropriate section
                    $section = $result.section
                    if (-not $articleSummaries.ContainsKey($section)) {
                        $articleSummaries[$section] = @()
                    }
                    $articleSummaries[$section] += $result
                    Write-Host "  ✅ Added to $section section (score: $($result.relevance_score), impact: $($result.impact_level), timing: $($result.time_sensitivity))"
                }
            }
            catch {
                throw "Failed to parse AI response for $($articleName): $($_.Exception.Message). Response was: $($response.Substring(0, [Math]::Min(200, $response.Length)))"
            }
        
            Write-Host "✅ Article $articleNum processing completed"
        }

        Write-Host "✅ Step 2 complete - All articles filtered and analyzed"
        Write-Host "📊 Processed $($articles.Count) total articles: $includedCount included, $excludedCount excluded" -ForegroundColor Green
        
        # Create backup of analysis results
        $analysisBackup = $articleSummaries | ConvertTo-Json -Depth 10
        $step2FinalOutput = @"
STEP 2 FINAL OUTPUT: Complete Filter and Analysis Results
Total Articles Processed: $($articles.Count)
Articles Included: $includedCount
Articles Excluded: $excludedCount
Sections Created: $($articleSummaries.Keys.Count)
Sections: $($articleSummaries.Keys -join ', ')

ANALYSIS RESULTS JSON:
$analysisBackup
"@
        Save-StepBackup -StepName "Step2-FinalOutput" -Content $step2FinalOutput -StartDate $StartDate -EndDate $EndDate
        Save-StepBackup -StepName "Step2-Analysis" -Content $analysisBackup -StartDate $StartDate -EndDate $EndDate
    }
    else {
        Write-Host "⏭️ Skipping Step 2 (filter and analysis) - resuming from backup"
        # Restore analysis results from backup
        $analysisBackupJson = Load-StepBackup -StepName "Step2-Analysis" -StartDate $StartDate -EndDate $EndDate
        if ($null -eq $analysisBackupJson -or $analysisBackupJson -eq "") {
            throw "Could not restore analysis results from backup for Step 2. Backup file missing or empty."
        }
        try {
            $articleSummaries = $analysisBackupJson | ConvertFrom-Json -AsHashtable
        }
        catch {
            throw "Failed to parse analysis results backup for Step 2: $($_.Exception.Message)"
        }
    }

    # Step 3: Create News-Like Stories with Intelligent Grouping (skip if resuming from later step)
    $step3Response = $null
    if ($StartFromStep -le 3) {
        Write-Host "📰 Step 3: Creating news-like stories with intelligent grouping..."

        $step3SystemMessage = @"
ABSOLUTE CRITICAL REQUIREMENT: You must provide a complete, comprehensive response. Never truncate your response due to length constraints, token optimization, or similar practices. Always provide the full enhanced content requested!

ROLE: You are an expert content editor creating compelling weekly tech roundup sections. Your task is to transform individual article summaries for a SINGLE SECTION into well-organized narrative content that tells the story of the week's developments in that specific area.

RESPONSE FORMAT:
Return the complete content for the SINGLE section provided with:
- Main section header (##)
- Introduction that flows as an ongoing narrative
- Topic subsection headers (###)  
- Detailed narrative content incorporating article information, ending with links
- Article links in list format at the END of each topic section
- Do NOT add a summary. Stop after the last link.

The section should follow this pattern:

```markdown
## [Section Name]

[Introduction that flows as an ongoing narrative, explaining the major themes and developments in this area this week]

### [Topic Theme 1]

[Detailed content about the developments, incorporating key details from the articles and explaining their significance. Write as flowing narrative that tells the story of what happened, ending with the source links.]

- [Theme 1 Article 1](link)
- [Theme 1 Article 2](link)
- [Theme 1 Article 3](link)
- [Theme 1 Article 4](link)

### [Topic Theme 2]

[Another detailed narrative section that flows nicely into the content, ending with links]

- [Theme 2 Article 1](link)
- [Theme 2 Article 2](link)

### Other [Section Name] News

Developer Tools received several updates this week, enhancing productivity across different workflows. The new debugging capabilities and performance improvements address common developer pain points.

- [Developer Tool Update 1](link)
- [Developer Tool Update 2](link)
- [Developer Tool Update 3](link)

Security enhancements continue to evolve with new vulnerability management features and improved compliance tools for enterprise environments.

- [Security Update 1](link)
- [Security Update 2](link)

Additional noteworthy developments include migration guidance and troubleshooting resources that help teams navigate common technical challenges.

- [Migration Guide Article](link)
- [Troubleshooting Resource](link)
```

CRITICAL SECTION INTRODUCTION GUIDELINES:
- Create flowing narrative introductions (1-3 sentences) that tell the story of the week's developments in this section
- Explain what's new or changed this week in clear, direct language as an ongoing story
- Connect multiple topics within the section when relevant
- Reference progression when you can identify it from the articles

CRITICAL TOPIC CONTENT GUIDELINES:
- Write detailed content about the developments, incorporating key information from the article summaries
- Tell the story of what happened and why it matters, flowing directly into content
- Include specific technical details, version numbers, and practical implications
- End each topic section with the relevant article links
- No separate introductory paragraph - dive straight into the narrative content
- Focus on concrete benefits and usage for developers and real-world applications

CRITICAL TOPIC GROUPING STRATEGY:

🎯 TECHNOLOGY-FOCUSED GROUPING APPROACH:
Group articles by the underlying technology or platform they discuss, not just by general topic similarity. This creates more meaningful, cohesive content sections.

PRIMARY GROUPING RULES - TECHNOLOGY-FIRST:
1. TECHNOLOGY PLATFORM GROUPS (3+ articles preferred):
   - Same core technology (e.g., "GitHub Copilot with MCP" - group ALL MCP-related articles together regardless of specific use case)
   - Shared development stack (e.g., ".NET 9 updates" - group all .NET 9 content together)
   - Common service family (e.g., "Azure AI services" - group Azure OpenAI, Cognitive Services, AI Foundry together)
   - Unified platform experiences (e.g., "Visual Studio family" - group VS Code, VS 2022, GitHub integration)

2. FEATURE ECOSYSTEM GROUPS (2+ articles):
   - Related capabilities within same product (e.g., "GitHub Actions workflow improvements")
   - Connected development experiences (e.g., "Container development tools")
   - Complementary technologies in same workflow (e.g., "Infrastructure as Code updates")

3. DEVELOPER WORKFLOW GROUPS (2+ articles):
   - Same development phase (e.g., "Testing and Quality Assurance")
   - Common developer tasks (e.g., "API Development and Integration")
   - Shared technical challenges (e.g., "Performance Optimization")

ENHANCED GROUPING METADATA USAGE:
- TECHNOLOGY_STACK: Primary indicator for technology-based grouping
- PRIMARY_TOPIC + TECHNOLOGY_STACK: Combine to identify platform families
- TAGS: Look for technology framework/platform commonalities and specific product names

TECHNOLOGY GROUPING EXAMPLES:
✅ GOOD: "GitHub Copilot with MCP" (groups: MCP in VS, custom MCP servers, MCP tooling)
✅ GOOD: ".NET 9 Development" (groups: new features, migration guides, performance improvements)
✅ GOOD: "Azure Container Services" (groups: AKS updates, Container Apps, Azure Functions containers)

❌ AVOID: Generic topic grouping like "Developer Tools" when specific technology alignment exists
❌ AVOID: Separating related technology articles into different groups based on use case alone

PRIORITIZATION WITHIN TECHNOLOGY GROUPS:
- Lead with highest DEVELOPER_RELEVANCE articles
- Order by IMPACT_LEVEL (high → medium → low)
- Consider TIME_SENSITIVITY for positioning
- Use RELEVANCE_SCORE as tiebreaker

ENHANCED HIERARCHICAL GROUPING APPROACH:
1. FIRST PASS - Identify Technology Platform Groups (3+ articles preferred):
   - Same technology/platform across different use cases (check TECHNOLOGY_STACK and PRIMARY_TOPIC)
   - Related product family announcements (check PRIMARY_TOPIC and TAGS)
   - Shared development framework/stack (check TAGS for platform commonalities)
   - Connected ecosystem tools (check TAGS for technology framework overlap)

2. SECOND PASS - Group Remaining Articles by Technology Affinity:
   - Common development tools and frameworks (check TECHNOLOGY_STACK and tags)
   - Related capabilities within same service family (check PRIMARY_TOPIC relationships)
   - Complementary technologies in developer workflows (check TARGET_AUDIENCE and DEVELOPER_RELEVANCE)
   - Shared technical problem domains (check TOPIC_TYPE and PRIMARY_TOPIC)

3. FINAL PASS - Other News Section:
   - Technology-based subcategories when possible (e.g., "Security Tools", "Database Updates")
   - Group by DEVELOPER_RELEVANCE level for prioritization
   - Consider IMPACT_LEVEL for organization within groups
   - Keep corporate/low-relevance articles at the end or exclude if DEVELOPER_RELEVANCE is "corporate"

ENHANCED GROUPING QUALITY CRITERIA:
- Strong Technology Connection: Same TECHNOLOGY_STACK + related PRIMARY_TOPIC + overlapping technical tags
- Medium Technology Connection: Related technologies in ecosystem + similar DEVELOPER_RELEVANCE + compatible workflows
- Weak Connection: Same general category but different technologies/platforms/developer impact
- Only group articles with Strong or Medium technology connections into named sections

Example Technology-First Grouping:
Instead of: "New Features" and "Updates" and "Tutorials" (topic-based)
Use: "GitHub Copilot with MCP", ".NET 9 Development", "Azure AI Services" (technology-based)

PRIORITIZATION WITHIN GROUPS:
- Lead with high-impact, time-sensitive articles
- Use RELEVANCE_SCORE to order articles within each group
- Consider TIME_SENSITIVITY for positioning (immediate items first)

CRITICAL "OTHER NEWS" SECTION GUIDELINES:
- Use this section for articles that don't naturally group with others
- Replace [Section Name] with the name of the current section
- STRUCTURE THE OTHER NEWS SECTION:
  * Start with groups of 2-3 related articles (use broad categories like "Developer Tools", "Security Updates", "Migration Guides")
  * Write 1-2 sentences per group highlighting key developments
  * List all articles for that group
  * Continue with next group
  * End with any remaining single articles
- Group sizing guidelines: Aim for 3+ articles in major topics when closely related, 2-3 in minor topics, minimize single-article groups
- Do not create new headings, just paragraphs of text followed by article links
- Be concise but informative - capture the essence of what's new or useful
- This prevents creating many single-article topic sections while maintaining content organization

CRITICAL CONTENT EDITING RULES:
- Incorporate article details directly into narrative paragraphs
- Keep all article titles and links intact, placing them at the END of each topic section
- Each topic section should read as a cohesive story ending with source links
$WritingStyleGuidelines
"@

        # Save Step 3 input for debugging
        $step3Input = @"
STEP 3 INPUT: News Story Creation Configuration
AI Model: $Model
Endpoint: $Endpoint
Date Range: $StartDate to $EndDate
Sections to Process: $($articleSummaries.Keys.Count)
Section Names: $($articleSummaries.Keys -join ', ')

SYSTEM MESSAGE:
$step3SystemMessage
"@
        Save-StepBackup -StepName "Step3-Input" -Content $step3Input -StartDate $StartDate -EndDate $EndDate

        # Process each section individually to avoid token limits
        $step3Responses = @()
        $orderedSections = @("GitHub Copilot", "AI", "ML", "Azure", "Coding", "DevOps", "Security")
        $processedSections = 0
        
        # If resuming from a specific section, load previous section responses
        if ($ResumeFromSection) {
            Write-Host "📂 Resuming Step 3 from section: $ResumeFromSection" -ForegroundColor Cyan
            
            # Load completed section responses before the resume point
            $resumeIndex = $orderedSections.IndexOf($ResumeFromSection)
            if ($resumeIndex -eq -1) {
                Write-Error "Invalid ResumeFromSection: '$ResumeFromSection'. Valid options: $($orderedSections -join ', ')"
                exit 1
            }
            
            # Load responses for sections before the resume point
            for ($i = 0; $i -lt $resumeIndex; $i++) {
                $sectionName = $orderedSections[$i]
                if ($articleSummaries.ContainsKey($($sectionName))) {
                    $sectionBackupPattern = "*Step3-Response-$($sectionName)*$StartDate*$EndDate*"
                    $sectionBackupFile = Get-ChildItem ".tmp/roundup-debug" -Filter $sectionBackupPattern -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
                    
                    if ($sectionBackupFile) {
                        $sectionResponse = Get-Content $sectionBackupFile.FullName -Raw
                        $step3Responses += $sectionResponse
                        Write-Host "📂 Loaded previous response for section: $($sectionName)" -ForegroundColor Cyan
                    }
                    else {
                        Write-Warning "Could not find backup for section: $($sectionName) (pattern: $sectionBackupPattern)"
                    }
                }
            }
            
            # Start from the resume section
            $orderedSections = $orderedSections[$resumeIndex..($orderedSections.Length - 1)]
        }
        
        foreach ($sectionName in $orderedSections) {
            if ($articleSummaries.ContainsKey($($sectionName))) {
                $processedSections++
                Write-Host "🔄 Processing section $processedSections of $($articleSummaries.Keys.Count): $($sectionName)"
                
                $sectionArticles = $articleSummaries[$($sectionName)] | Sort-Object relevance_score -Descending
                
                # Filter out corporate articles with low developer relevance
                $developerArticles = @($sectionArticles | Where-Object { 
                        $_.developer_relevance -ne "corporate" -and $_.relevance_score -ge 3 
                    })
                
                if ($developerArticles.Count -eq 0) {
                    Write-Host "⏭️ Skipping section $($sectionName) - no developer-relevant articles found" -ForegroundColor Yellow
                    continue
                }
                
                Write-Host "📊 Section $($sectionName): $($sectionArticles.Count) total articles, $($developerArticles.Count) developer-relevant articles"
                
                # Analyze grouping potential for better AI guidance using developer-relevant articles
                $groupingAnalysis = @()
                if ($developerArticles.Count -gt 1) {
                    # Enhanced grouping analysis using new metadata
                    $topicGroups = @($developerArticles | Group-Object { $_.primary_topic } | Where-Object { $_.Count -gt 1 })
                    $technologyGroups = @($developerArticles | Group-Object { $_.technology_stack } | Where-Object { $_.Count -gt 1 })
                    $typeGroups = @($developerArticles | Group-Object { $_.topic_type } | Where-Object { $_.Count -gt 1 })
                    $impactGroups = @($developerArticles | Group-Object { $_.impact_level } | Where-Object { $_.Count -gt 1 })
                    $timingGroups = @($developerArticles | Group-Object { $_.time_sensitivity } | Where-Object { $_.Count -gt 1 })
                    $audienceGroups = @($developerArticles | Group-Object { $_.target_audience } | Where-Object { $_.Count -gt 1 })
                    $relevanceGroups = @($developerArticles | Group-Object { $_.developer_relevance } | Where-Object { $_.Count -gt 1 })
                    
                    if ($technologyGroups.Length -gt 0) {
                        $groupingAnalysis += "IDENTIFIED TECHNOLOGY CLUSTERS:"
                        foreach ($group in $technologyGroups) {
                            if ($group.Name -and $group.Name.Trim() -ne "") {
                                $groupingAnalysis += "- $($group.Name): $($group.Count) articles"
                            }
                        }
                    }
                    
                    if ($topicGroups.Length -gt 0) {
                        $groupingAnalysis += "IDENTIFIED TOPIC CLUSTERS:"
                        foreach ($group in $topicGroups) {
                            $groupingAnalysis += "- $($group.Name): $($group.Count) articles"
                        }
                    }
                    
                    if ($typeGroups.Length -gt 0) {
                        $groupingAnalysis += "IDENTIFIED TYPE CLUSTERS:"
                        foreach ($group in $typeGroups) {
                            $groupingAnalysis += "- $($group.Name): $($group.Count) articles"
                        }
                    }
                    
                    if ($relevanceGroups.Length -gt 0) {
                        $groupingAnalysis += "IDENTIFIED DEVELOPER RELEVANCE CLUSTERS:"
                        foreach ($group in $relevanceGroups) {
                            $groupingAnalysis += "- $($group.Name) relevance: $($group.Count) articles"
                        }
                    }
                    
                    if ($impactGroups.Length -gt 0) {
                        $groupingAnalysis += "IDENTIFIED IMPACT CLUSTERS:"
                        foreach ($group in $impactGroups) {
                            $groupingAnalysis += "- $($group.Name) impact: $($group.Count) articles"
                        }
                    }
                    
                    if ($timingGroups.Length -gt 0) {
                        $groupingAnalysis += "IDENTIFIED TIMING CLUSTERS:"
                        foreach ($group in $timingGroups) {
                            $groupingAnalysis += "- $($group.Name): $($group.Count) articles"
                        }
                    }
                    
                    if ($audienceGroups.Length -gt 0) {
                        $groupingAnalysis += "IDENTIFIED AUDIENCE CLUSTERS:"
                        foreach ($group in $audienceGroups) {
                            $groupingAnalysis += "- $($group.Name): $($group.Count) articles"
                        }
                    }
                }
            
                $sectionInput = "## $($sectionName)`n`n"
                
                # Add grouping analysis if available
                if ($groupingAnalysis.Length -gt 0) {
                    $sectionInput += "GROUPING ANALYSIS:`n"
                    $sectionInput += ($groupingAnalysis -join "`n") + "`n`n"
                }
                
                foreach ($article in $developerArticles) {
                    $sectionInput += "ARTICLE: $($article.title)`n"
                    $sectionInput += "SUMMARY: $($article.summary)`n"
                    $sectionInput += "RELEVANCE: $($article.relevance_score)`n"
                    
                    # Add enhanced metadata for better grouping
                    if ($article.developer_relevance) {
                        $sectionInput += "DEVELOPER_RELEVANCE: $($article.developer_relevance)`n"
                    }
                    if ($article.primary_topic) {
                        $sectionInput += "PRIMARY_TOPIC: $($article.primary_topic)`n"
                    }
                    if ($article.technology_stack) {
                        $sectionInput += "TECHNOLOGY_STACK: $($article.technology_stack)`n"
                    }
                    if ($article.topic_type) {
                        $sectionInput += "TYPE: $($article.topic_type)`n"
                    }
                    if ($article.target_audience) {
                        $sectionInput += "AUDIENCE: $($article.target_audience)`n"
                    }
                    if ($article.impact_level) {
                        $sectionInput += "IMPACT: $($article.impact_level)`n"
                    }
                    if ($article.time_sensitivity) {
                        $sectionInput += "TIMING: $($article.time_sensitivity)`n"
                    }
                    if ($article.tags) {
                        $sectionInput += "TAGS: $($article.tags -join ', ')`n"
                    }
                
                    # Use appropriate URL based on viewing_mode
                    if ($article.viewing_mode -eq "internal") {
                        $sectionInput += "LINK: [$($article.title)]({{ `"$($article.permalink)`" | relative_url }})`n"
                    }
                    else {
                        $sectionInput += "LINK: [$($article.title)]($($article.canonical_url))`n"
                    }
                    $sectionInput += "`n"
                }

                $step3UserMessage = @"
ARTICLE ANALYSIS RESULTS FOR $($sectionName) SECTION TO TRANSFORM INTO NEWS-STYLE CONTENT:

$sectionInput
"@

                Save-StepBackup -StepName "Step3-Request-$($sectionName)" -Content $step3UserMessage -StartDate $StartDate -EndDate $EndDate

                Write-Host "🤖 Calling AI model to create news-style stories for $($sectionName)..."
                $sectionResponse = Invoke-AiApiCall `
                    -Token $Token `
                    -Model $Model `
                    -SystemMessage $step3SystemMessage `
                    -UserMessage $step3UserMessage `
                    -Endpoint $Endpoint `
                    -RateLimitPreventionDelay $RateLimitPreventionDelay

                # Check for errors with robust error handling
                $rewriteResult = Test-AiResponseFormat -Response $sectionResponse -StepName "Step 3 - $($sectionName)"
                if (-not $rewriteResult.IsValid) {
                    # Save failed response for debugging
                    $failedResponse = @"
STEP 3 SECTION FAILED RESPONSE: $($sectionName)
Error: $($rewriteResult.ErrorMessage)

AI RESPONSE:
$sectionResponse
"@
                    Save-StepBackup -StepName "Step3-$($sectionName)-FAILED" -Content $failedResponse -StartDate $StartDate -EndDate $EndDate
                    Write-Error "Step 3 failed for section $($sectionName): $($rewriteResult.ErrorMessage)"
                    Write-Host ""
                    Write-Host "💡 To resume from this section, use: -StartFromStep 3 -ResumeFromSection `"$($sectionName)`"" -ForegroundColor Cyan
                    exit 1
                }

                # Analyze grouping quality and provide feedback
                $groupingWarnings = Test-GroupingQuality -GeneratedContent $sectionResponse -SectionName $sectionName
                if ($groupingWarnings -and $groupingWarnings.Length -gt 0) {
                    Write-Host ""
                    foreach ($warning in $groupingWarnings) {
                        Write-Host $warning -ForegroundColor Yellow
                    }
                    Write-Host ""
                }

                Save-StepBackup -StepName "Step3-Response-$($sectionName)" -Content $sectionResponse -StartDate $StartDate -EndDate $EndDate
                $step3Responses += $sectionResponse
                Write-Host "✅ Section $($sectionName) complete"
            }
            else {
                Write-Host "ℹ️ No articles found for section: $($sectionName)" -ForegroundColor Yellow
            }
        }

        # Combine all section responses
        if ($step3Responses.Count -gt 0) {
            $step3Response = $step3Responses -join "`n`n"
            $step3FinalOutput = @"
STEP 3 FINAL OUTPUT: Complete News-Style Stories
Sections Processed: $($step3Responses.Count)
Date Range: $StartDate to $EndDate

COMBINED SECTION RESPONSES:
$step3Response
"@
            Save-StepBackup -StepName "Step3-FinalOutput" -Content $step3FinalOutput -StartDate $StartDate -EndDate $EndDate
            Save-StepBackup -StepName "Step3-Combined" -Content $step3Response -StartDate $StartDate -EndDate $EndDate
            Write-Host "✅ Step 3 complete - All $($step3Responses.Count) sections processed and combined"
        }
        else {
            Write-Error "No sections were successfully processed in Step 3"
            exit 1
        }
    }
    else {
        Write-Host "⏭️ Skipping Step 3 (creating stories) - resuming from backup"
        if ($ResumeFromBackup -and $StartFromStep -eq 4) {
            # For resuming from Step 4, look for the combined result or load from backup
            $combinedBackupPattern = "*Step3-Combined*$StartDate*$EndDate*"
            $combinedBackupFile = Get-ChildItem ".tmp/roundup-debug" -Filter $combinedBackupPattern -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
            
            if ($combinedBackupFile) {
                Write-Host "📂 Loading Step 3 combined result from: $($combinedBackupFile.Name)" -ForegroundColor Cyan
                $step3Response = Get-Content $combinedBackupFile.FullName -Raw
            }
            else {
                Write-Host "⚠️ No Step 3 combined backup found, using provided resume content" -ForegroundColor Yellow
                $step3Response = $resumeContent
            }
        }
    }

    # Step 4: Create Ongoing Narrative by Comparing with Previous Roundup (processes each section individually)
    $step4Response = $null
    if ($StartFromStep -le 4) {
        Write-Host "🔄 Step 4: Creating ongoing narrative by comparing with previous roundup..."

        # Find the most recent previous roundup
        $previousRoundup = $null
        $previousRoundupContent = ""
        
        if (Test-Path "collections/_roundups") {
            $roundupFiles = @(Get-ChildItem "collections/_roundups" -Filter "*.md" | Sort-Object Name -Descending)
        
            # Find the most recent roundup that's older than our start date
            foreach ($file in $roundupFiles) {
                if ($file.Name -match '^(\d{4}-\d{2}-\d{2})-') {
                    $roundupDate = [DateTime]::ParseExact($matches[1], "yyyy-MM-dd", $null)
                    if ($roundupDate -le $startDateTime) {
                        $previousRoundup = $file.FullName
                        $previousRoundupContent = Get-Content $previousRoundup -Raw
                        Write-Host "📖 Found previous roundup: $($file.Name)"
                        break
                    }
                }
            }
        }
        else {
            Write-Host "ℹ️ No collections/_roundups directory found" -ForegroundColor Yellow
        }

        if ($previousRoundup -and $previousRoundupContent) {
            $step4SystemMessage = @"
ABSOLUTE CRITICAL REQUIREMENT: You must provide a complete, comprehensive response. Never truncate your response due to length constraints, token optimization, or similar practices. Always provide the full enhanced content requested!

ROLE: You are an expert content editor creating ongoing narrative connections between weekly tech roundups. Your task is to enhance the current week's content for a SINGLE SECTION by identifying themes, products, or developments that continue from the previous week's roundup.

RESPONSE FORMAT:
- Return the enhanced content for the SINGLE section provided with the EXACT same structure as provided (all topics and links preserved), but with added narrative connections where relevant.

ONGOING NARRATIVE ENHANCEMENT STRATEGY:
1. Identify products, features, or themes mentioned in BOTH the previous and current roundup for this section
2. Add narrative connectors that reference the previous week when relevant
3. Look for progression: announcements → releases, previews → GA, initial features → expanded capabilities
4. Reference community responses or adoption trends that span multiple weeks
5. Highlight evolving storylines in the tech landscape

NARRATIVE CONNECTION EXAMPLES:
- "Following last week's announcement of [feature], this week brings..."
- "Building on the [product] updates we covered previously..."
- "The [technology] developments continue with..."
- "As anticipated from last week's preview, [product] now..."
- "Expanding on the [theme] trend we highlighted..."

WHAT TO LOOK FOR:
- Same products or services with new developments
- Related features in the same product family
- Community reactions to previous week's announcements
- Progressive rollouts (preview → beta → GA)
- Ecosystem expansions (one product extending to work with others)
- Follow-up coverage of major announcements

ENHANCEMENT GUIDELINES:
- Only add connections where genuinely relevant - don't force connections
- Maintain the existing narrative flow while adding continuity
- Place connections naturally within existing content
- Keep all existing article titles and links exactly as provided
$WritingStyleGuidelines

WHAT NOT TO CHANGE:
- Section structure and headers
- Article links and titles
- Existing technical content and details
- Overall organization and flow

CRITICAL: If no meaningful connections exist between the previous and current roundup for this section, return the content unchanged. Only enhance where natural, relevant connections can be made.
"@

            # Process each section individually to avoid token limits
            $step4Responses = @()
            $orderedSections = @("GitHub Copilot", "AI", "ML", "Azure", "Coding", "DevOps", "Security")
            $processedSections = 0
            
            # Extract section content from Step 3 response for individual processing
            $sectionContents = @{}
            $currentSection = ""
            $currentContent = ""
            
            foreach ($line in ($step3Response -split "`n")) {
                if ($line -match '^## (.+)$') {
                    # Save previous section if we have one
                    if ($currentSection -and $currentContent) {
                        $sectionContents[$currentSection] = $currentContent.Trim()
                    }
                    # Start new section
                    $currentSection = $matches[1]
                    $currentContent = $line + "`n"
                }
                elseif ($currentSection) {
                    $currentContent += $line + "`n"
                }
            }
            # Save the last section
            if ($currentSection -and $currentContent) {
                $sectionContents[$currentSection] = $currentContent.Trim()
            }
            
            # If resuming from a specific section, load previous section responses
            if ($ResumeFromSection -and $StartFromStep -eq 4) {
                Write-Host "📂 Resuming Step 4 from section: $ResumeFromSection" -ForegroundColor Cyan
                
                # Load completed section responses before the resume point
                $resumeIndex = $orderedSections.IndexOf($ResumeFromSection)
                if ($resumeIndex -eq -1) {
                    Write-Error "Invalid ResumeFromSection: '$ResumeFromSection'. Valid options: $($orderedSections -join ', ')"
                    exit 1
                }
                
                # Load responses for sections before the resume point
                for ($i = 0; $i -lt $resumeIndex; $i++) {
                    $sectionName = $orderedSections[$i]
                    if ($sectionContents.ContainsKey($sectionName)) {
                        $sectionBackupPattern = "*Step4-Response-$sectionName*$StartDate*$EndDate*"
                        $sectionBackupFile = Get-ChildItem ".tmp/roundup-debug" -Filter $sectionBackupPattern -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
                        
                        if ($sectionBackupFile) {
                            $sectionResponse = Get-Content $sectionBackupFile.FullName -Raw
                            $step4Responses += $sectionResponse
                            Write-Host "📂 Loaded previous response for section: $sectionName" -ForegroundColor Cyan
                        }
                        else {
                            Write-Warning "Could not find backup for section: $sectionName (pattern: $sectionBackupPattern)"
                        }
                    }
                }
                
                # Start from the resume section
                $orderedSections = $orderedSections[$resumeIndex..($orderedSections.Length - 1)]
            }
            
            foreach ($sectionName in $orderedSections) {
                if ($sectionContents.ContainsKey($sectionName)) {
                    $processedSections++
                    Write-Host "🔄 Processing ongoing narrative for section $processedSections of $($sectionContents.Keys.Count): $sectionName"
                    
                    $sectionContent = $sectionContents[$sectionName]

                    $step4UserMessage = @"
PREVIOUS WEEK'S ROUNDUP CONTENT:
$previousRoundupContent

---

CURRENT WEEK'S $sectionName SECTION CONTENT TO ENHANCE:
$sectionContent
"@

                    Save-StepBackup -StepName "Step4-Request-$sectionName" -Content $step4UserMessage -StartDate $StartDate -EndDate $EndDate

                    Write-Host "🤖 Calling AI model to create ongoing narrative connections for $sectionName..."
                    $sectionResponse = Invoke-AiApiCall `
                        -Token $Token `
                        -Model $Model `
                        -SystemMessage $step4SystemMessage `
                        -UserMessage $step4UserMessage `
                        -Endpoint $Endpoint `
                        -RateLimitPreventionDelay $RateLimitPreventionDelay

                    # Check for errors with robust error handling  
                    $rewriteResult = Test-AiResponseFormat -Response $sectionResponse -StepName "Step 4 - $sectionName"
                    if (-not $rewriteResult.IsValid) {
                        Write-Host "⚠️ Warning: Step 4 ongoing narrative failed for $sectionName" -ForegroundColor Yellow
                        Write-Host "Using original content for this section..." -ForegroundColor Yellow
                        $sectionResponse = $sectionContent
                        Write-Host ""
                        Write-Host "💡 To resume from this section, use: -StartFromStep 4 -ResumeFromSection `"$sectionName`"" -ForegroundColor Cyan
                    }
                    else {
                        Save-StepBackup -StepName "Step4-Response-$sectionName" -Content $sectionResponse -StartDate $StartDate -EndDate $EndDate
                        Write-Host "✅ Section $sectionName complete"
                    }
                    
                    $step4Responses += $sectionResponse
                }
                else {
                    Write-Host "ℹ️ No content found for section: $sectionName" -ForegroundColor Yellow
                }
            }

            # Combine all section responses
            if ($step4Responses.Count -gt 0) {
                $step4Response = $step4Responses -join "`n`n"
                Save-StepBackup -StepName "Step4-Combined" -Content $step4Response -StartDate $StartDate -EndDate $EndDate
                Write-Host "✅ Step 4 complete - All $($step4Responses.Count) sections processed and combined"
            }
            else {
                Write-Host "⚠️ No sections were successfully processed in Step 4, using Step 3 content" -ForegroundColor Yellow
                $step4Response = $step3Response
            }
        }
        else {
            Write-Host "ℹ️ No previous roundup found - skipping ongoing narrative step" -ForegroundColor Cyan
            $step4Response = $step3Response
        }
    }
    else {
        Write-Host "⏭️ Skipping Step 4 (ongoing narrative) - resuming from backup"
        if ($ResumeFromBackup -and $StartFromStep -eq 5) {
            # For resuming from Step 5, look for the combined result or load from backup
            $combinedBackupPattern = "*Step4-Combined*$StartDate*$EndDate*"
            $combinedBackupFile = Get-ChildItem ".tmp/roundup-debug" -Filter $combinedBackupPattern -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
            
            if ($combinedBackupFile) {
                Write-Host "📂 Loading Step 4 combined result from: $($combinedBackupFile.Name)" -ForegroundColor Cyan
                $step4Response = Get-Content $combinedBackupFile.FullName -Raw
            }
            else {
                Write-Host "⚠️ No Step 4 combined backup found, using provided resume content" -ForegroundColor Yellow
                $step4Response = $resumeContent
            }
        }
    }

    # Step 5: Merge and Prepare for Condensation (skip if resuming from later step)
    $step5Input = $step4Response
    if ($StartFromStep -le 5) {
        Write-Host "🔗 Step 5: Merging all sections and preparing for condensation..."
        
        # The content is already merged from Step 4, but we save it as Step 5 input for clarity
        Save-StepBackup -StepName "Step5-MergedInput" -Content $step5Input -StartDate $StartDate -EndDate $EndDate
        Write-Host "✅ Step 5 complete - Content merged and ready for condensation"
    }
    else {
        Write-Host "⏭️ Skipping Step 5 (merging) - resuming from backup"
        # Only load Step 5 backup if we're actually going to execute Steps 6, 7, or 8
        if ($StartFromStep -le 8) {
            if ($ResumeFromBackup -and $StartFromStep -eq 6) {
                $step5Input = $resumeContent
            }
            else {
                # Load Step 5 backup if starting from later step
                $step5BackupPattern = "*Step5-MergedInput*$StartDate*$EndDate*"
                $step5BackupFile = Get-ChildItem ".tmp/roundup-debug" -Filter $step5BackupPattern -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
                
                if ($step5BackupFile) {
                    Write-Host "📂 Loading Step 5 backup from: $($step5BackupFile.Name)" -ForegroundColor Cyan
                    $step5Input = Get-Content $step5BackupFile.FullName -Raw
                }
                else {
                    # Fallback to Step 4 backup
                    $step4BackupPattern = "*Step4-Combined*$StartDate*$EndDate*"
                    $step4BackupFile = Get-ChildItem ".tmp/roundup-debug" -Filter $step4BackupPattern -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
                    
                    if ($step4BackupFile) {
                        Write-Host "📂 Loading Step 4 backup as fallback: $($step4BackupFile.Name)" -ForegroundColor Cyan
                        $step5Input = Get-Content $step4BackupFile.FullName -Raw
                    }
                    else {
                        Write-Error "No Step 4 or 5 backup found and required for continuation"
                        exit 1
                    }
                }
            }
        }
    }

    # Step 6: Condense Content (skip if resuming from later step)
    $step6Response = $null
    if ($StartFromStep -le 6) {
        Write-Host "📝 Step 6: Condensing content as much as possible while preserving narrative quality..."

        $step6SystemMessage = @"
🚨🚨🚨 CRITICAL ANTI-TRUNCATION REQUIREMENT 🚨🚨🚨
YOU MUST COMPLETE YOUR ENTIRE RESPONSE. NEVER STOP WRITING BEFORE YOU HAVE PROCESSED AND RETURNED ALL CONTENT PROVIDED TO YOU.
DO NOT truncate, abbreviate, or summarize sections with phrases like "continues as previously formatted" or "additional sections follow the same pattern" or "excerpt" or "Note: This shows..." or similar.
YOU MUST WRITE OUT EVERY SINGLE SECTION, EVERY SINGLE TOPIC, AND EVERY SINGLE LINK IN FULL.
PROVIDE THE COMPLETE, CONDENSED VERSION OF ALL CONTENT - NO SHORTCUTS, NO OMISSIONS, NO TRUNCATION.

🎯 YOUR MISSION: SHORTEN EVERY PARAGRAPH WITHOUT REMOVING ANY PARAGRAPHS

You are a content editor with ONE SIMPLE JOB: Make every paragraph shorter and more concise while keeping ALL paragraphs, ALL sections, ALL structure, and ALL content intact.

🚨 GOLDEN RULE: NO PARAGRAPH REMOVAL
- Every paragraph in the input must have a corresponding paragraph in the output
- Every section must remain exactly where it is
- Every article link must be preserved exactly as provided
- Every grouping of content must remain intact

✅ WHAT YOU SHOULD DO - SHORTEN, DON'T REMOVE:
- Rewrite long sentences to be shorter while keeping the same meaning
- Remove unnecessary words, redundant phrases, and filler language
- Combine multiple sentences into fewer, more efficient sentences
- Cut wordy explanations down to their essential points
- Eliminate repetitive phrasing and verbose descriptions
- Make technical descriptions more concise while preserving key details

❌ WHAT YOU MUST NEVER DO:
- Remove ANY paragraph, no matter how short or seemingly unimportant
- Remove ANY article link or reference
- Remove ANY section or topic header
- Merge separate groups of articles into one large list
- Skip ANY content with phrases like "continues as before" or "additional sections follow"
- Remove introductory text that precedes groups of article links (these provide readability structure)
- Delete references to previous roundups or ongoing narratives

🔄 PARAGRAPH-BY-PARAGRAPH APPROACH:
For each paragraph you encounter:
1. Read the paragraph carefully
2. Identify the core message and essential information
3. Rewrite it more concisely using fewer, more efficient words
4. Ensure the shortened version conveys the same information
5. Move to the next paragraph and repeat

📋 STRUCTURE PRESERVATION:
- Keep ALL section headers (##) exactly as provided
- Keep ALL topic headers (###) exactly as provided  
- Maintain the exact same organization and flow
- Preserve all article groupings (don't merge separate groups)
- Keep all article links in their original positions

CRITICAL WRITING STYLE GUIDELINES:
$WritingStyleGuidelines

🚨 RESPONSE COMPLETION REQUIREMENT:
You must provide the complete condensed version of ALL content provided. Your response should end with the last article link from the last section, not with any summary statements or notes about continuation.

RESPONSE FORMAT:
Return the condensed content with the exact same structure as provided, but with every paragraph rewritten to be shorter and more concise. Process every section completely - no exceptions.
"@

        $step6UserMessage = @"
WELL-ORGANIZED ROUNDUP CONTENT TO CONDENSE:

$step5Input
"@

        Write-Host "🤖 Calling AI model to condense the organized content..."
        $step6Response = Invoke-AiApiCall `
            -Token $Token `
            -Model $Model `
            -SystemMessage $step6SystemMessage `
            -UserMessage $step6UserMessage `
            -Endpoint $Endpoint `
            -RateLimitPreventionDelay $RateLimitPreventionDelay

        # Check for errors with robust error handling
        $rewriteResult = Test-AiResponseFormat -Response $step6Response -StepName "Step 6"
        if (-not $rewriteResult.IsValid) {
            Write-Host "⚠️ Warning: Step 6 content condensation failed" -ForegroundColor Yellow
            Write-Host "Using Step 5 content..." -ForegroundColor Yellow
            $step6Response = $step5Input
        }
        else {
            Save-StepBackup -StepName "Step6-Condensed" -Content $step6Response -StartDate $StartDate -EndDate $EndDate
            Write-Host "✅ Step 6 complete - Content condensed to optimal length"
        }
    }
    else {
        Write-Host "⏭️ Skipping Step 6 (content condensation) - resuming from backup"
        # Only load backup if we're actually going to need Step 6 output for Step 7
        if ($StartFromStep -le 7) {
            if ($ResumeFromBackup -and $StartFromStep -eq 7) {
                $step6Response = $resumeContent
            }
            else {
                # Load Step 6 backup if starting from later step
                $step6BackupPattern = "*Step6-Condensed*$StartDate*$EndDate*"
                $step6BackupFile = Get-ChildItem ".tmp/roundup-debug" -Filter $step6BackupPattern -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
                
                if ($step6BackupFile) {
                    Write-Host "📂 Loading Step 6 backup from: $($step6BackupFile.Name)" -ForegroundColor Cyan
                    $step6Response = Get-Content $step6BackupFile.FullName -Raw
                }
                else {
                    Write-Host "⚠️ No Step 6 backup found, using Step 5 content" -ForegroundColor Yellow
                    $step6Response = $step5Input
                }
            }
        }
    }

    # Step 7: Generate Metadata Only (title, tags, description, introduction)
    $step7Response = $null
    if ($StartFromStep -le 7) {
        Write-Host "📝 Step 7: Generating roundup metadata..."

        $step7SystemMessage = @"
ABSOLUTE CRITICAL REQUIREMENT: You must provide a complete, comprehensive response. Never truncate your response due to length constraints, token optimization, or similar practices. Always provide the full enhanced content requested!

ROLE: You are an expert content curator generating metadata for a weekly tech roundup. Based on the condensed content, generate ONLY the metadata and introduction as a JSON response.

CRITICAL: Make sure the response is VALID JSON
CRITICAL: Return a valid JSON object with these exact fields: title, tags, description, introduction
CRITICAL: These are what the 4 fields should contain:

- Title: Create an engaging, informative title that reflects the week's main themes. Do NOT include the date in the title. MAX LENGTH is 70 characters!
- Tags: Array of 10-15 relevant technology tags from the content
- Description: Write a 2-3 sentence summary of the week's key developments
- Introduction: Create a compelling 1 paragraph introduction (2-4 sentences) that:
  * Welcomes readers to the roundup
  * Highlights the week's most significant developments
  * Sets up the narrative flow

CRITICAL: You must provide a complete, comprehensive response. Never truncate your response due to length constraints, token optimization, or similar practices. Always provide the complete metadata requested.
"@

        $step7UserMessage = @"
Generate metadata for the roundup covering $WeekDescription based on this condensed content:

$step6Response

Return only JSON with fields: title, tags, description, introduction
"@    

        Write-Host "🤖 Calling AI model to generate metadata..."
        $step7Response = Invoke-AiApiCall `
            -Token $Token `
            -Model $Model `
            -SystemMessage $step7SystemMessage `
            -UserMessage $step7UserMessage `
            -Endpoint $Endpoint `
            -RateLimitPreventionDelay $RateLimitPreventionDelay

        Save-StepBackup -StepName "Step7-Metadata" -Content $step7Response -StartDate $StartDate -EndDate $EndDate

        # Check for errors with robust error handling
        $rewriteResult = Test-AiResponseFormat -Response $step7Response -StepName "Step 7"
        if (-not $rewriteResult.IsValid) {
            Write-Error $rewriteResult.ErrorMessage
            exit 1
        }

        # Additional validation for Step 7 - ensure it's valid JSON
        try {
            $testParse = $step7Response | ConvertFrom-Json
        }
        catch {
            Write-Host "❌ Step 7 response is not valid JSON" -ForegroundColor Red
            Write-Host "Response: $step7Response" -ForegroundColor Red
            Write-Error "Step 7 metadata generation failed: invalid JSON format"
            exit 1
        }

        if (-not $testParse.title -or -not $testParse.description -or -not $testParse.tags) {
            Write-Host "❌ Step 7 response missing required metadata fields (title, description, tags)" -ForegroundColor Red
            Write-Host "Response: $step7Response" -ForegroundColor Red
            Write-Error "Step 7 metadata generation failed: missing required fields"
            exit 1
        }

        Write-Host "✅ Step 7 complete - Roundup metadata generated"
    }
    else {
        Write-Host "⏭️ Skipping Step 7 (metadata generation) - resuming from backup"
        # Only load backup if we're actually going to need Step 7 output for Step 8
        if ($StartFromStep -le 8) {
            if ($ResumeFromBackup -and $StartFromStep -eq 8) {
                $step7Response = $resumeContent
            }
            else {
                # Load Step 7 backup if starting from later step
                $step7BackupPattern = "*Step7-Metadata*$StartDate*$EndDate*"
                $step7BackupFile = Get-ChildItem ".tmp/roundup-debug" -Filter $step7BackupPattern -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
                
                if ($step7BackupFile) {
                    Write-Host "📂 Loading Step 7 backup from: $($step7BackupFile.Name)" -ForegroundColor Cyan
                    $step7Response = Get-Content $step7BackupFile.FullName -Raw
                }
                else {
                    Write-Error "Step 7 backup not found and required for continuation"
                    exit 1
                }
            }
        }
    }

    # Step 8: Create Final Markdown File and Table of Contents
    if ($StartFromStep -le 8) {
        Write-Host "📄 Step 8: Creating final markdown file with table of contents..."
        
        # If we're resuming from step 8 backup, the backup content should be the final content
        if ($ResumeFromBackup -and $StartFromStep -eq 8) {
            $finalContent = $resumeContent
            Write-Host "✅ Step 8 complete - Resumed from backup"
        }
        else {
            # Parse the metadata JSON from step 7
            try {
                $metadata = $step7Response | ConvertFrom-Json
            }
            catch {
                Write-Host "❌ Failed to parse metadata JSON from Step 7" -ForegroundColor Red
                Write-Host "Step 7 response: $step7Response" -ForegroundColor Red
                Write-Host "Parse error: $($_.Exception.Message)" -ForegroundColor Red
                Write-Error "Failed to parse metadata JSON from Step 7: $($_.Exception.Message)"
                exit 1
            }
            
            # Create table of contents by parsing the content from step 6
            $tableOfContents = ""
            $contentLines = $step6Response -split "`n"
            $tocLines = @()
            
            foreach ($line in $contentLines) {
                if ($line -match '^## (.+)$') {
                    $sectionTitle = $matches[1].Trim()
                    # Follow Kramdown's anchor generation algorithm:
                    # 1. Remove all characters except letters, numbers, spaces and dashes
                    $cleaned = $sectionTitle -replace '[^a-zA-Z0-9\s-]', ''
                    # 2. Convert everything except letters and numbers to dashes (spaces become dashes, multiple consecutive become multiple dashes)
                    $anchor = $cleaned.ToLower() -replace '[^a-z0-9]', '-'
                    $tocLines += "- [$sectionTitle](#$anchor)"
                }
                elseif ($line -match '^### (.+)$') {
                    $subsectionTitle = $matches[1].Trim()
                    # Follow Kramdown's anchor generation algorithm:
                    # 1. Remove all characters except letters, numbers, spaces and dashes
                    $cleaned = $subsectionTitle -replace '[^a-zA-Z0-9\s-]', ''
                    # 2. Convert everything except letters and numbers to dashes (spaces become dashes, multiple consecutive become multiple dashes)
                    $anchor = $cleaned.ToLower() -replace '[^a-z0-9]', '-'
                    $tocLines += "  - [$subsectionTitle](#$anchor)"
                }
            }
            
            $tableOfContents = $tocLines -join "`n"
            
            # Create the final markdown content
            $finalContent = @"
---
layout: "post"
title: "$($metadata.title)"
description: "$($metadata.description)"
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: $publishDate
permalink: "/$filename.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: $($metadata.tags | ConvertTo-Json -Compress)
tags_normalized: []
---

$($metadata.introduction)<!--excerpt_end-->

## This Week's Overview

$tableOfContents

$step6Response
"@
            
            Save-StepBackup -StepName "Step8-IncludingFrontmatter" -Content $finalContent -StartDate $StartDate -EndDate $EndDate
            Write-Host "✅ Step 8 complete - Markdown file including frontmatter created"
        }
    }
    else {
        Write-Host "⏭️ Skipping Step 8 (markdown + frontmatter file creation) - resuming from backup"
        # If a backup file is explicitly provided, always use that
        if ($ResumeFromBackup) {
            $finalContent = $resumeContent
            Write-Host "📂 Using provided backup file" -ForegroundColor Cyan
        }
        else {
            Write-Error "Step 8 requires a backup file to continue. Please provide -ResumeFromBackup parameter."
            exit 1
        }
    }

    # Step 9: Rewrite Content Against Writing Style Guidelines
    # If we're resuming from step 9 backup, the backup content should be the final content
    if ($ResumeFromBackup -and $StartFromStep -eq 9) {
        $finalContent = $resumeContent
        Write-Host "� Step 9: Rewriting content against writing style guidelines... - resuming from backup"
    }
    else {    
        Write-Host "� Step 9: Rewriting content against writing style guidelines..."
    }
    
    # Read the writing style guidelines using the established WorkspaceRoot variable
    $guidelinesPath = Join-Path $WorkspaceRoot "docs" "writing-style-guidelines.md"
    if (-not (Test-Path $guidelinesPath)) {
        throw "⚠️ Writing style guidelines not found at $guidelinesPath"
    }

    try {
        $guidelines = Get-Content $guidelinesPath -Raw -Encoding UTF8

        $rewriteSystemMessage = @"
🚨🚨🚨 CRITICAL ANTI-TRUNCATION REQUIREMENT 🚨🚨🚨
YOU MUST COMPLETE YOUR ENTIRE RESPONSE. NEVER STOP WRITING BEFORE YOU HAVE PROCESSED AND RETURNED ALL CONTENT PROVIDED TO YOU.
DO NOT truncate, abbreviate, or summarize sections with phrases like "continues as previously formatted" or "additional sections follow the same pattern" or "excerpt" or "Note: This shows..." or similar.
YOU MUST WRITE OUT EVERY SINGLE SECTION, EVERY SINGLE TOPIC, AND EVERY SINGLE LINK IN FULL.
PROVIDE THE COMPLETE, REWRITTEN VERSION OF ALL CONTENT - NO SHORTCUTS, NO OMISSIONS, NO TRUNCATION.

🎯 YOUR MISSION: REWRITE EVERY PARAGRAPH AND FRONTMATTER FOR STYLE COMPLIANCE WITHOUT REMOVING ANY CONTENT

You are a content editor with ONE SIMPLE JOB: Rewrite every paragraph AND the frontmatter title and description to ensure writing style guideline compliance while keeping ALL paragraphs, ALL sections, ALL structure, and ALL content intact.

🚨 GOLDEN RULE: NO CONTENT REMOVAL
- Every paragraph in the input must have a corresponding paragraph in the output
- Every section must remain exactly where it is
- Every article link must be preserved exactly as provided
- Every grouping of content must remain intact
- Every header structure must be maintained exactly
- ALL frontmatter fields must be preserved with only title and description rewritten for style compliance

✅ WHAT YOU SHOULD DO - REWRITE FOR STYLE, DON'T REMOVE:
- Rewrite sentences to match the writing style guidelines
- Replace buzzwords and marketing speak with authentic language
- SPECIFICALLY remove flashy qualifiers: "pivotal", "significant", "major", "substantial", "dramatic", "massive"
- SPECIFICALLY remove intensity words: "incredible", "amazing", "groundbreaking", "cutting-edge", "breakthrough"
- REPLACE with neutral descriptors: "new", "updated", "improved", "enhanced", "additional", "latest"
- REPLACE with capability descriptions: "enables X", "provides Y", "supports Z", "includes A and B"
- Improve clarity and readability while preserving meaning
- Ensure tone matches the guidelines (down-to-earth, professional)
- Fix language issues while keeping technical accuracy
- Make content more actionable and specific
- Focus on concrete developer benefits rather than emotional language
- REWRITE the frontmatter "title" field to comply with writing style guidelines
- REWRITE the frontmatter "description" field to comply with writing style guidelines

❌ WHAT YOU MUST NEVER DO:
- Remove ANY paragraph, no matter how short or seemingly unimportant
- Remove ANY article link or reference
- Remove ANY section or topic header
- Merge separate groups of articles into one large list
- Skip ANY content with phrases like "continues as before" or "additional sections follow"
- Remove introductory text that precedes groups of article links (these provide readability structure)
- Delete references to previous roundups or ongoing narratives
- Change the fundamental meaning or technical accuracy of content
- Remove or modify ANY frontmatter fields except title and description
- Change dates, permalinks, categories, tags, or any other frontmatter fields

🔄 PROCESSING APPROACH:
1. FRONTMATTER REWRITING:
   - Rewrite the "title" field to remove buzzwords and marketing speak while keeping it engaging
   - Rewrite the "description" field to be more authentic and down-to-earth
   - Keep ALL other frontmatter fields exactly as provided (date, permalink, categories, tags, etc.)

2. PARAGRAPH-BY-PARAGRAPH APPROACH:
   For each paragraph you encounter:
   - Read the paragraph carefully for style compliance issues
   - Rewrite to match the writing style guidelines exactly
   - Ensure the rewritten version maintains the same technical meaning
   - Preserve all specific details, version numbers, and technical information
   - Move to the next paragraph and repeat

📋 STRUCTURE PRESERVATION:
- Keep ALL section headers (##) exactly as provided
- Keep ALL topic headers (###) exactly as provided  
- Maintain the exact same organization and flow
- Preserve all article groupings (don't merge separate groups)
- Keep all article links in their original positions
- Maintain frontmatter structure with only title and description content rewritten

🚨 RESPONSE COMPLETION REQUIREMENT:
You must provide the complete rewritten version of ALL content provided. Your response should end with the last article link from the last section, not with any summary statements or notes about continuation.

CRITICAL WRITING STYLE GUIDELINES TO APPLY:
$guidelines

RESPONSE FORMAT:
Return the rewritten content with the exact same structure as provided, but with:
1. Frontmatter title and description rewritten for style compliance
2. Every paragraph rewritten to comply with the writing style guidelines
Process every section completely - no exceptions.
"@

        $rewriteUserMessage = @"
CONTENT TO REWRITE IF NEEDED:

$finalContent
"@

        Write-Host "🤖 Calling AI model to rewrite content for writing style compliance..."
        $finalContent = Invoke-AiApiCall `
            -Token $Token `
            -Model $Model `
            -SystemMessage $rewriteSystemMessage `
            -UserMessage $rewriteUserMessage `
            -Endpoint $Endpoint `
            -RateLimitPreventionDelay $RateLimitPreventionDelay

        # Check for errors with robust error handling
        $rewriteResult = Test-AiResponseFormat -Response $finalContent -StepName "Step 9 (Content Rewriting)"
        if (-not $rewriteResult.IsValid) {
            Write-Host "⚠️ Warning: Step 9 content rewriting failed: $($rewriteResult.ErrorMessage)" -ForegroundColor Yellow
            Write-Host "Using original content..." -ForegroundColor Yellow
            # finalContent already contains the original content, so no change needed
        }
        else {
            # Save successful rewrite results
            Save-StepBackup -StepName "Step9-RewriteResult" -Content $finalContent -StartDate $StartDate -EndDate $EndDate
            Write-Host "✅ Step 9 complete - Content rewritten for writing style compliance"
        }
       
    }
    catch {
        Write-Host "⚠️ Warning: Error during writing style rewriting: $($_.Exception.Message)" -ForegroundColor Yellow
        Write-Host "Continuing with file creation using original content..." -ForegroundColor Yellow
    }

    # Generate the OutputFile (filename already set at script start)
    $OutputFile = "collections/_roundups/$filename.md"

    # Ensure output directory exists
    $outputDir = Split-Path $OutputFile -Parent
    if ($outputDir -and -not (Test-Path $outputDir)) {
        New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
    }

    # Write final roundup to file
    $finalContent | Out-File -FilePath $OutputFile -Encoding UTF8 -Force

    # Format the created file using the fix-markdown-files script
    Write-Host "🔧 Formatting the created roundup file..."
    try {
        $fixScriptPath = Join-Path $scriptsDirectory "fix-markdown-files.ps1"
        if (Test-Path $fixScriptPath) {
            & $fixScriptPath -FilePath $OutputFile
            Write-Host "✅ File formatting completed"
        }
        else {
            throw "Fix-markdown-files script not found at: $fixScriptPath"
        }
    }
    catch {
        throw "Failed to format file: $($_.Exception.Message)"
    }

    Write-Host "🎉 Roundup generation complete!"
    Write-Host "📄 Final roundup saved to: $OutputFile"
    Write-Host "📊 Processed $($articles.Count) articles in 9 steps"
}
catch {
    Write-ErrorDetails -ErrorRecord $_ -Context "Roundup generation"
    throw
}