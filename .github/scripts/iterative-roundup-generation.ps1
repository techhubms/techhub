#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Generates a weekly roundup using a structured 9-step AI process for comprehensive content creation.

.DESCRIPTION
    This script creates weekly roundups using a structured 9-step process:
    1. Collect articles from the specified date range
    2. Analyze each article for summary and section categorization via AI
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
    GitHub Personal Access Token or Azure API Key for AI model access

.PARAMETER Model
    AI model to use (default: openai/gpt-4o)

.PARAMETER Endpoint
    AI API endpoint (default: GitHub Models)

.PARAMETER ResumeFromBackup
    Path to a backup file to resume from instead of starting from scratch

.PARAMETER StartFromStep
    Step number to start from when resuming (1-9, default: 1)

.PARAMETER ResumeFromSection
    Section name to resume from in Step 3 or 4 (e.g., "AI", "Azure"). Useful if Step 3 or 4 fails partway through processing sections.

.EXAMPLE
    ./iterative-roundup-generation.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "ghp_..."

.EXAMPLE
    ./iterative-roundup-generation.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "api_key..." -Model "my-gpt4-deployment" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions"

.EXAMPLE
    ./iterative-roundup-generation.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "ghp_..." -ResumeFromBackup ".tmp/roundup-debug/2025-07-21-to-2025-07-27-Step4-OngoingNarrative-20250812-1430.txt" -StartFromStep 5

.EXAMPLE
    ./iterative-roundup-generation.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "ghp_..." -StartFromStep 3 -ResumeFromSection "Azure"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$StartDate,
    
    [Parameter(Mandatory = $true)]
    [string]$EndDate,
    
    [Parameter(Mandatory = $true)]
    [string]$Token,
    
    [Parameter(Mandatory = $false)]
    [string]$Model = "openai/gpt-4o",
    
    [Parameter(Mandatory = $false)]
    [string]$Endpoint = "https://models.github.ai/inference/chat/completions",

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
            IsValid = $false
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

# Import functions
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
        Write-Error "StartFromStep must be between 1 and 9"
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

    # Step 2: Analyze Each Article for Summary and Categorization (skip if resuming from later step)
    if ($StartFromStep -le 2) {
        Write-Host "🤖 Step 2: Analyzing articles for summaries and categorization..."

        $step2SystemMessage = @"
ABSOLUTE CRITICAL REQUIREMENT: You must provide a complete, comprehensive response. Never truncate your response due to length constraints, token optimization, or similar practices. Always provide the full enhanced content requested!

ROLE: You are an expert technical content analyst creating detailed summaries for a comprehensive weekly tech roundup. Your analysis will be used to create narrative sections that tell the story of the week's developments.

RESPONSE FORMAT: Return ONLY a JSON object with this exact structure:
{
  "section": "Suggested Section Name",
  "summary": "Comprehensive Summary",
  "relevance_score": 1-10,
  "primary_topic": "Main product/technology/concept",
  "topic_type": "announcement|tutorial|update|guide|analysis|feature|troubleshooting|case-study|news|preview|ga-release|deprecation|migration|integration|comparison",
  "target_audience": "developers|administrators|business|researchers|data-scientists|devops-engineers",
  "original_tags": ["tag1", "tag2", "tag3"],
  "ai_suggested_tags": ["additional-tag1", "additional-tag2"],
  "impact_level": "high|medium|low",
  "time_sensitivity": "immediate|this-week|this-month|long-term"
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

DECISION EXAMPLES FOR COMMON SCENARIOS:
- "GitHub Copilot for C# development" → "GitHub Copilot" (Copilot is primary focus)
- "C# async patterns and best practices" → "Coding" (no AI/Copilot focus)
- "Azure OpenAI integration tutorial" → "AI" (AI service usage, not technical ML)
- "Custom ML model training on Azure" → "ML" (technical ML development)
- "Building REST APIs in ASP.NET Core" → "Coding" (development focus)
- "Deploying .NET apps with GitHub Actions" → "DevOps" (deployment process focus)
- "Azure Functions serverless architecture" → "Azure" (Azure service focus)
- "Zero Trust security architecture" → "Security" (security focus)

WHEN CONTENT SPANS MULTIPLE AREAS:
- Choose the section that represents the PRIMARY purpose or main audience
- Ask: "What is this content mainly trying to teach or announce?"
- If it's about using a tool/service, categorize by the tool/service being used
- If it's about implementing something, categorize by the implementation domain

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

RELEVANCE SCORING (1-10) - FOR PRE-FILTERED TECH CONTENT:
Since content is already filtered for tech relevance, score based on IMPACT and IMPORTANCE within the tech ecosystem:
- 9-10: Breaking news, major product launches, industry-changing announcements, critical security issues
- 7-8: Significant feature releases, important updates that affect many developers, notable capability expansions
- 5-6: Useful updates, incremental improvements, helpful tutorials for common scenarios
- 3-4: Minor releases, niche features, specialized use cases, maintenance updates
- 1-2: Very minor updates, extremely niche content, or edge case scenarios

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
- ORIGINAL_TAGS: Extract all tags from the article's frontmatter "tags" field exactly as they appear
- AI_SUGGESTED_TAGS: Add 3-5 additional relevant tags that would help with grouping and discovery
- Focus AI suggested tags on: product families, technology stacks, use cases, developer roles
"@

        # Save Step 2 input for debugging
        $step2Input = @"
STEP 2 INPUT: Article Analysis Configuration
AI Model: $Model
Endpoint: $Endpoint
Date Range: $StartDate to $EndDate
Total Articles to Process: $($articles.Count)

SYSTEM MESSAGE:
$step2SystemMessage
"@
        Save-StepBackup -StepName "Step2-Input" -Content $step2Input -StartDate $StartDate -EndDate $EndDate

        # Process each article for analysis
        for ($i = 0; $i -lt $articles.Count; $i++) {
            $articleFilePath = $articles[$i]
            $articleNum = $i + 1
            $articleName = Split-Path $articleFilePath -Leaf
        
            Write-Host "🔄 Analyzing article $articleNum of $($articles.Count): $($articleName)"
        
            # Check if file exists
            if (-not (Test-Path $articleFilePath)) {
                throw "Article file not found: $articleFilePath"
            }
        
            # Read the article content
            $articleContent = Get-Content $articleFilePath -Raw
            
            # Extract original tags for AI analysis
            $originalTags = Get-FrontMatterValue -Content $articleContent -Key "tags"
            $tagsForAI = if ($originalTags) { 
                ($originalTags | ConvertTo-Json -Compress) 
            } else { 
                "[]" 
            }

            $step2UserMessage = @"
ARTICLE CONTENT:
$articleContent

ORIGINAL ARTICLE TAGS: $tagsForAI
"@
            
            # Save individual article input for debugging
            $individualInput = @"
STEP 2 INDIVIDUAL ARTICLE INPUT ($articleNum of $($articles.Count))
Article: $articleName
File Path: $articleFilePath

USER MESSAGE:
$step2UserMessage
"@
            Save-StepBackup -StepName "Step2-Article$($articleNum)-$($articleName.Replace('.md', ''))-Input" -Content $individualInput -StartDate $StartDate -EndDate $EndDate
        
            # Call the AI model
            Write-Host "🤖 Calling AI model for analysis..."
            $response = Invoke-AiApiCall `
                -Token $Token `
                -Model $Model `
                -SystemMessage $step2SystemMessage `
                -UserMessage $step2UserMessage `
                -Endpoint $Endpoint `
                -RateLimitPreventionDelay $RateLimitPreventionDelay

            # Check for errors with robust error handling
            $rewriteResult = Test-AiResponseFormat -Response $response -StepName "Step 2 (AI analysis for $articleName)"
            if (-not $rewriteResult.IsValid) {
                # Save failed response for debugging
                $failedResponse = @"
STEP 2 INDIVIDUAL ARTICLE FAILED RESPONSE ($articleNum of $($articles.Count))
Article: $articleName
File Path: $articleFilePath
Error: $($rewriteResult.ErrorMessage)

AI RESPONSE:
$response
"@
                Save-StepBackup -StepName "Step2-Article$($articleNum)-$($articleName.Replace('.md', ''))-FAILED" -Content $failedResponse -StartDate $StartDate -EndDate $EndDate
                throw $rewriteResult.ErrorMessage
            }

            # Save successful individual article response for debugging
            $individualOutput = @"
STEP 2 INDIVIDUAL ARTICLE OUTPUT ($articleNum of $($articles.Count))
Article: $articleName
File Path: $articleFilePath

AI RESPONSE:
$response
"@
            Save-StepBackup -StepName "Step2-Article$($articleNum)-$($articleName.Replace('.md', ''))-Output" -Content $individualOutput -StartDate $StartDate -EndDate $EndDate

            # Parse AI response
            try {
                $analysisResult = $response | ConvertFrom-Json
                
                # Validate required fields
                if (-not $analysisResult.section -or -not $analysisResult.summary -or -not $analysisResult.relevance_score) {
                    throw "Invalid response for $($articleName): Missing required fields"
                }
                
                # Extract original title and link from frontmatter using existing function
                $canonicalUrl = Get-FrontMatterValue -Content $articleContent -Key "canonical_url"
                $title = Get-FrontMatterValue -Content $articleContent -Key "title"
                $viewingMode = Get-FrontMatterValue -Content $articleContent -Key "viewing_mode"
                $permalink = Get-FrontMatterValue -Content $articleContent -Key "permalink"
                $originalTags = Get-FrontMatterValue -Content $articleContent -Key "tags"
                
                # Ensure original_tags is populated from frontmatter if not provided by AI
                if (-not $analysisResult.original_tags -and $originalTags) {
                    $analysisResult | Add-Member -NotePropertyName "original_tags" -NotePropertyValue $originalTags
                }
                
                $analysisResult | Add-Member -NotePropertyName "canonical_url" -NotePropertyValue $canonicalUrl
                $analysisResult | Add-Member -NotePropertyName "title" -NotePropertyValue $title
                $analysisResult | Add-Member -NotePropertyName "viewing_mode" -NotePropertyValue $viewingMode
                $analysisResult | Add-Member -NotePropertyName "permalink" -NotePropertyValue $permalink
                $analysisResult | Add-Member -NotePropertyName "filename" -NotePropertyValue $articleFilePath
                
                # Add to appropriate section
                $section = $analysisResult.section
                if (-not $articleSummaries.ContainsKey($section)) {
                    $articleSummaries[$section] = @()
                }
                $articleSummaries[$section] += $analysisResult
                Write-Host "  ✅ Added to $section section (score: $($analysisResult.relevance_score), impact: $($analysisResult.impact_level), timing: $($analysisResult.time_sensitivity))"
            }
            catch {
                $parseError = @"
STEP 2 INDIVIDUAL ARTICLE PARSE ERROR ($articleNum of $($articles.Count))
Article: $articleName
File Path: $articleFilePath
Parse Error: $($_.Exception.Message)

AI RESPONSE:
$($response.Substring(0, [Math]::Min(500, $response.Length)))
"@
                Save-StepBackup -StepName "Step2-Article$($articleNum)-$($articleName.Replace('.md', ''))-PARSE-ERROR" -Content $parseError -StartDate $StartDate -EndDate $EndDate
                throw "Failed to parse AI response for $($articleName): $($_.Exception.Message). Response was: $($response.Substring(0, [Math]::Min(200, $response.Length)))"
            }
        
            Write-Host "✅ Article $articleNum analyzed successfully"
        }

        Write-Host "✅ Step 2 complete - All articles analyzed"
        
        # Create backup of analysis results
        $analysisBackup = $articleSummaries | ConvertTo-Json -Depth 10
        $step2FinalOutput = @"
STEP 2 FINAL OUTPUT: Complete Analysis Results
Total Articles Processed: $($articles.Count)
Sections Created: $($articleSummaries.Keys.Count)
Sections: $($articleSummaries.Keys -join ', ')

ANALYSIS RESULTS JSON:
$analysisBackup
"@
        Save-StepBackup -StepName "Step2-FinalOutput" -Content $step2FinalOutput -StartDate $StartDate -EndDate $EndDate
        Save-StepBackup -StepName "Step2-Analysis" -Content $analysisBackup -StartDate $StartDate -EndDate $EndDate
    }
    else {
        Write-Host "⏭️ Skipping Step 2 (article analysis) - resuming from backup"
        # Note: articleSummaries will be reconstructed from the next step's input if needed
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
- Comprehensive narrative section introduction
- Topic subsection headers (###)  
- Detailed narrative content incorporating article information, ending with links
- Article links in list format at the END of each topic section
- Do NOT add a summary. Stop after the last link.

The section should follow this pattern:

```markdown
## [Section Name]

[Comprehensive section introduction that flows as an ongoing narrative, explaining the major themes and developments in this area this week]

### [Topic Theme 1]

[Detailed narrative content about the developments, incorporating key details from the articles and explaining their significance. Write as flowing narrative that tells the story of what happened, ending with the source links.]

- [Theme 1 Article 1](link)
- [Theme 1 Article 2](link)
- [Theme 1 Article 3](link)
- [Theme 1 Article 4](link)

### [Topic Theme 2]

[Another detailed narrative section that flows directly into content, ending with links]

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
- Create flowing narrative introductions that tell the story of the week's developments
- Explain what's new or changed this week in clear, direct language as an ongoing story
- Connect multiple topics within the section when relevant
- Use narrative phrases like "This week brought...", "Several developments emerged...", "The community saw..."
- Reference progression when you can identify it from the articles
- Focus on practical impact and create a sense of continuity
- Provide comprehensive context without length restrictions

CRITICAL TOPIC CONTENT GUIDELINES:
- Write detailed narrative content that incorporates key information from the article summaries
- Tell the story of what happened and why it matters, flowing directly into content
- Include specific technical details, version numbers, and practical implications
- End each topic section with the relevant article links
- No separate introductory paragraph - dive straight into the narrative content
- Focus on concrete benefits and real-world applications

CRITICAL TOPIC GROUPING STRATEGY:

Use the enhanced metadata to make smarter grouping decisions:
- IMPACT LEVEL: Prioritize high-impact articles for major topics
- TIME SENSITIVITY: Group immediate/this-week items for timely themes
- TOPIC TYPE: Group similar types (announcements, tutorials, updates)
- ORIGINAL_TAGS + AI_TAGS: Use for identifying related technologies and themes

PRIMARY GROUPING RULES:
- Create Major Topic Groups: 3+ articles on closely related themes (no upper limit - group as many closely related articles as exist)
- Create Minor Topic Groups: 2-3 articles on similar themes when they're strongly related
- Reserve "Other News": Single articles or 2+ articles that only have weak connections

ENHANCED HIERARCHICAL GROUPING APPROACH:
1. FIRST PASS - Identify Major Themes (3+ articles):
   - Same product announcements/updates (check PRIMARY_TOPIC and tags)
   - Related feature releases (check ORIGINAL_TAGS and AI_TAGS for technology overlap)
   - Common workflow improvements (check TARGET_AUDIENCE and IMPACT LEVEL)
   - Shared technology stack (check tags for framework/platform commonalities)
   - Time-sensitive clusters (multiple IMMEDIATE or THIS-WEEK items on related topics)

2. SECOND PASS - Group Remaining Articles:
   - Pair articles with matching TOPIC_TYPE and similar PRIMARY_TOPIC
   - Group by TARGET_AUDIENCE when content serves the same developer role
   - Use tag overlap to identify technology relationships
   - Examples: "Security Tool Updates", "Database Migration Improvements"

3. FINAL PASS - Other News Section:
   - Group very broadly by TOPIC_TYPE: tutorials, troubleshooting, announcements
   - Consider IMPACT LEVEL for prioritization within groups
   - Keep single articles that don't fit elsewhere
   - Avoid forcing weak connections

ENHANCED GROUPING QUALITY CRITERIA:
- Strong Connection: Same PRIMARY_TOPIC + significant tag overlap + matching IMPACT_LEVEL
- Medium Connection: Related technologies in tags + similar TARGET_AUDIENCE + compatible TOPIC_TYPE
- Weak Connection: Same general section but different purposes/audiences/impact levels
- Only group articles with Strong or Medium connections into named sections

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
                
                # Analyze grouping potential for better AI guidance
                $groupingAnalysis = @()
                if ($sectionArticles.Count -gt 1) {
                    # Enhanced grouping analysis using new metadata
                    $topicGroups = @($sectionArticles | Group-Object { $_.primary_topic } | Where-Object { $_.Count -gt 1 })
                    $typeGroups = @($sectionArticles | Group-Object { $_.topic_type } | Where-Object { $_.Count -gt 1 })
                    $impactGroups = @($sectionArticles | Group-Object { $_.impact_level } | Where-Object { $_.Count -gt 1 })
                    $timingGroups = @($sectionArticles | Group-Object { $_.time_sensitivity } | Where-Object { $_.Count -gt 1 })
                    $audienceGroups = @($sectionArticles | Group-Object { $_.target_audience } | Where-Object { $_.Count -gt 1 })
                    
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
                
                foreach ($article in $sectionArticles) {
                    $sectionInput += "ARTICLE: $($article.title)`n"
                    $sectionInput += "SUMMARY: $($article.summary)`n"
                    $sectionInput += "RELEVANCE: $($article.relevance_score)`n"
                    
                    # Add enhanced metadata for better grouping
                    if ($article.primary_topic) {
                        $sectionInput += "PRIMARY_TOPIC: $($article.primary_topic)`n"
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
                    if ($article.original_tags) {
                        $sectionInput += "ORIGINAL_TAGS: $($article.original_tags -join ', ')`n"
                    }
                    if ($article.ai_suggested_tags) {
                        $sectionInput += "AI_TAGS: $($article.ai_suggested_tags -join ', ')`n"
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
        
        if (Test-Path "_roundups") {
            $roundupFiles = @(Get-ChildItem "_roundups" -Filter "*.md" | Sort-Object Name -Descending)
        
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
            Write-Host "ℹ️ No _roundups directory found" -ForegroundColor Yellow
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
CRITICAL: Return a JSON object with these exact fields: title, tags, description, introduction
CRITICAL: These are what the 4 fields should contain:

- Title: Create an engaging, informative title that reflects the week's main themes. Do NOT include the date in the title.
- Tags: Array of 10-15 relevant technology tags from the content
- Description: Write a 2-3 sentence summary of the week's key developments
- Introduction: Create a compelling 2-3 paragraph introduction that:
  * Welcomes readers to the roundup
  * Highlights the week's most significant developments
  * Provides context for the stories that follow
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

        # Check for errors with robust error handling
        $rewriteResult = Test-AiResponseFormat -Response $step7Response -StepName "Step 7"
        if (-not $rewriteResult.IsValid) {
            Write-Error $rewriteResult.ErrorMessage
            exit 1
        }

        # Additional validation for Step 7 - ensure it's valid JSON
        try {
            $testParse = $step7Response | ConvertFrom-Json
            if (-not $testParse.title -or -not $testParse.description -or -not $testParse.tags) {
                Write-Host "❌ Step 7 response missing required metadata fields (title, description, tags)" -ForegroundColor Red
                Write-Host "Response: $step7Response" -ForegroundColor Red
                Write-Error "Step 7 metadata generation failed: missing required fields"
                exit 1
            }
        }
        catch {
            Write-Host "❌ Step 7 response is not valid JSON" -ForegroundColor Red
            Write-Host "Response: $step7Response" -ForegroundColor Red
            Write-Error "Step 7 metadata generation failed: invalid JSON format"
            exit 1
        }

        Save-StepBackup -StepName "Step7-Metadata" -Content $step7Response -StartDate $StartDate -EndDate $EndDate
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
                    $anchor = $sectionTitle.ToLower() -replace '[^a-z0-9\s-]', '' -replace '\s+', '-'
                    $tocLines += "- [$sectionTitle](#$anchor)"
                }
                elseif ($line -match '^### (.+)$') {
                    $subsectionTitle = $matches[1].Trim()
                    $anchor = $subsectionTitle.ToLower() -replace '[^a-z0-9\s-]', '' -replace '\s+', '-'
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
    
    # Read the writing style guidelines
    $guidelinesPath = "/workspaces/techhub/docs/writing-style-guidelines.md"
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
- Improve clarity and readability while preserving meaning
- Ensure tone matches the guidelines (down-to-earth, professional)
- Fix language issues while keeping technical accuracy
- Make content more actionable and specific
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
    $OutputFile = "_roundups/$filename.md"

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
        $fixScriptPath = Join-Path (Split-Path $PSScriptRoot -Parent) "scripts" "fix-markdown-files.ps1"
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