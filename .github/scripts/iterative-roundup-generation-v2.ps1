#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Generates a weekly roundup using a new iterative AI process with improved article grouping (v2).

.DESCRIPTION
    This script creates weekly roundups using a new iterative approach with enhanced output organization:
    1. Collect articles from the specified date range  
    2. Build roundup iteratively by integrating one article at a time
    3. Create ongoing narrative by comparing with the previous roundup
    4. Condense content as much as possible while maintaining narrative quality
    5. Generate metadata (title, tags, description, introduction)
    6. Create final markdown file with table of contents
    7. Rewrite content against writing style guidelines

    V2 IMPROVEMENTS:
    - All logging includes timestamps for better tracking
    - AI prompts focus on practical developer benefits
    - All output files saved in organized timestamped directories
    - Step files are numbered for easy resumption (e.g., step2-article-001-input.md)
    - All markdown files have .md extensions
    - Final roundup saved to output directory alongside step files
    
    RESUME FUNCTIONALITY:
    Use StartFromStep=2 and ResumeFromArticle=N to resume processing from article N.

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
    Step number to start from when resuming (1-7, default: 1)

.PARAMETER ResumeFromArticle
    Article number to resume from in Step 2 (useful if Step 2 fails partway through processing articles)

.EXAMPLE
    ./iterative-roundup-generation-v2.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "ghp_..."

.EXAMPLE
    ./iterative-roundup-generation-v2.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "api_key..." -Model "my-gpt4-deployment" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions"

.EXAMPLE
    ./iterative-roundup-generation-v2.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "ghp_..." -ResumeFromBackup ".tmp/roundup-debug/2025-07-21-to-2025-07-27-Step2-Iteration-15-20250812-1430.txt" -StartFromStep 2 -ResumeFromArticle 15

.EXAMPLE
    ./iterative-roundup-generation-v2.ps1 -StartDate "2025-07-21" -EndDate "2025-07-27" -Token "ghp_..." -ResumeFromBackup ".tmp/roundup-debug/2025-07-21-to-2025-07-27-Step4-OngoingNarrative-20250812-1430.txt" -StartFromStep 4
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
    [int]$ResumeFromArticle = 1,

    [Parameter(Mandatory = $false)]
    [int]$ResumeFromSection = 1
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Set timezone to Europe/Amsterdam for all timestamps
$env:TZ = "Europe/Amsterdam"

# Function to validate AI response format
function Test-AiResponseFormat {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Response,
        [Parameter(Mandatory = $true)]
        [string]$StepName
    )
    
    # Check if response indicates an error
    if ($Response -like "*`"Error`": true*") {
        try {
            $errorObj = $Response | ConvertFrom-Json
            Write-Host "[$(Get-Date -Format 'HH:mm:ss')] ❌ $StepName failed with error type: $($errorObj.Type)" -ForegroundColor Red
            if ($errorObj.ResponseContent) {
                Write-Host "[$(Get-Date -Format 'HH:mm:ss')] Response content: $($errorObj.ResponseContent)" -ForegroundColor Red
            }
            return @{ IsValid = $false; ErrorType = $errorObj.Type; ErrorMessage = "$StepName failed: $($errorObj.Type)" }
        }
        catch {
            Write-Host "[$(Get-Date -Format 'HH:mm:ss')] ❌ $StepName failed but could not parse error response" -ForegroundColor Red
            Write-Host "[$(Get-Date -Format 'HH:mm:ss')] Raw response: $Response" -ForegroundColor Red
            return @{ IsValid = $false; ErrorType = "UnparseableError"; ErrorMessage = "$StepName failed with unparseable error response" }
        }
    }
    
    # Check if response is empty or too short to be useful
    if ([string]::IsNullOrWhiteSpace($Response) -or $Response.Length -lt 10) {
        Write-Host "[$(Get-Date -Format 'HH:mm:ss')] ❌ $StepName returned empty or too short response" -ForegroundColor Red
        Write-Host "[$(Get-Date -Format 'HH:mm:ss')] Response length: $($Response.Length)" -ForegroundColor Red
        return @{ IsValid = $false; ErrorType = "EmptyResponse"; ErrorMessage = "$StepName returned empty or too short response" }
    }
    
    # Response appears to be successful
    return @{ IsValid = $true; ErrorType = $null; ErrorMessage = $null }
}

# Function to create backup files for debugging
function Save-StepBackup {
    param(
        [string]$StepName,
        [string]$Content,
        [string]$StartDate,
        [string]$EndDate,
        [string]$OutputDirectory = $null
    )
    
    if ($OutputDirectory) {
        $backupDir = $OutputDirectory
    } else {
        $backupDir = ".tmp/roundup-debug"
    }
    
    if (-not (Test-Path $backupDir)) {
        New-Item -ItemType Directory -Path $backupDir -Force | Out-Null
    }
    
    $timestamp = Get-Date -Format "HH:mm:ss"
    if ($OutputDirectory) {
        # For output directory saves, use simple filename without dates/times
        $extension = if ($StepName -match "\.md$") { "" } else { ".md" }
        $backupFile = Join-Path $backupDir "$StepName$extension"
    } else {
        # For debug saves, use original format
        $backupFile = Join-Path $backupDir "$StartDate-to-$EndDate-$StepName-$(Get-Date -Format 'yyyyMMdd-HHmm').txt"
    }
    
    $Content | Out-File -FilePath $backupFile -Encoding UTF8 -Force
    Write-Host "[$timestamp] 💾 Backup saved: $backupFile" -ForegroundColor DarkGray
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
    if ($ResumeFromBackup -and (Test-Path $ResumeFromBackup)) {
        Write-Host "[$(Get-Date -Format 'HH:mm:ss')] 📂 Resuming from backup: $ResumeFromBackup" -ForegroundColor Cyan
        $resumeContent = Get-Content -Path $ResumeFromBackup -Raw
        Write-Host "[$(Get-Date -Format 'HH:mm:ss')] ✅ Backup content loaded, starting from step $StartFromStep" -ForegroundColor Green
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

    # Create output directory with current date/time for this run
    $runTimestamp = Get-Date -Format "yyyyMMdd-HHmmss"
    $outputDirectory = ".tmp/roundup-output-$runTimestamp"
    if (-not (Test-Path $outputDirectory)) {
        New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null
    }
    $timestamp = Get-Date -Format "HH:mm:ss"
    Write-Host "[$timestamp] 📁 Created output directory: $outputDirectory" -ForegroundColor Cyan

    # Skip early steps if resuming
    if ($StartFromStep -gt 1) {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] ⏭️ Skipping steps 1-$($StartFromStep - 1) due to resume/restart parameter" -ForegroundColor Yellow
    }

    # Step 1: Collect all articles that need processing (same as v1)
    $articles = @()
    if ($StartFromStep -le 1) {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] 📋 Step 1: Scanning collections for content between $StartDate and $EndDate..." -ForegroundColor Green

        # Get collection priority order and collect all files (exclude roundups)
        $collectionPriorityOrder = Get-CollectionPriorityOrder

        foreach ($collectionDir in $collectionPriorityOrder.Keys) {
            # Skip the roundups collection to avoid including previous roundups
            if ($collectionDir -eq "_roundups") {
                continue
            }
            
            if (Test-Path $collectionDir) {
                $timestamp = Get-Date -Format "HH:mm:ss"
                Write-Host "[$timestamp] Scanning $collectionDir..." -ForegroundColor Blue
                
                $files = Get-ChildItem $collectionDir -Filter "*.md" -Recurse
                
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

        # Sort articles by filename to ensure consistent order
        $articles = $articles | Sort-Object
        
        if ($articles.Count -eq 0) {
            throw "No articles found in the specified date range: $StartDate to $EndDate"
        }
        
        # Create numbered breakdown for step 2 resume capability
        $numberedBreakdown = ""
        for ($i = 0; $i -lt $articles.Count; $i++) {
            $articleNumber = $i + 1
            $articlePath = $articles[$i]
            $fileName = [System.IO.Path]::GetFileName($articlePath)
            $numberedBreakdown += "$articleNumber. $fileName`n"
        }
        
        # Save numbered breakdown
        $breakdownPath = "$outputDirectory/step1-numbered-breakdown.md"
        Set-Content -Path $breakdownPath -Value $numberedBreakdown -Encoding UTF8
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] 💾 Saved numbered breakdown to: $breakdownPath" -ForegroundColor Cyan
        Write-Host "[$timestamp] 📊 Processing $($articles.Count) articles" -ForegroundColor Green
    }
    else {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] ⏭️ Skipping Step 1 (collection scanning) - resuming from backup" -ForegroundColor Yellow
    }

    # Step 2: Build roundup iteratively by integrating one article at a time
    $iterativeRoundup = ""
    if ($StartFromStep -le 2) {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] 🔄 Step 2: Building roundup iteratively by integrating articles one by one..." -ForegroundColor Green

        # If resuming from backup in Step 2, load the existing roundup content
        if ($ResumeFromBackup -and $StartFromStep -eq 2) {
            $iterativeRoundup = $resumeContent
            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] 📂 Loaded existing roundup content from backup" -ForegroundColor Cyan
        }

        # Define the system message for iterative roundup building with focus on developer value
        $iterativeSystemMessage = @"
IMPORTANT: Please provide a complete and comprehensive response that includes all requested content.

ROLE: You are an expert content editor creating a practical weekly tech roundup for developers. Your task is to analyze articles and create natural, engaging narratives that highlight developments, changes, and practical benefits for developers.

CONTENT FOCUS REQUIREMENTS:
- Focus on practical implications for developers
- Naturally incorporate what's new or changed in narrative flow
- Weave in how developments help or impact developers throughout the narrative
- Avoid including all article details and links
- Create engaging, readable narratives (NOT bullet-pointed lists)
- Skip marketing language and focus on technical value
- Write like a tech journalist, not an automated process

PROCESSING APPROACH:
- For the FIRST article: Create an initial roundup structure based on the article's content
- For SUBSEQUENT articles: Intelligently integrate the new article into the existing roundup

ROUNDUP STRUCTURE:
Use this flexible structure that groups content by natural themes and topics:

```markdown
## [Major Theme 1]

[Comprehensive introduction explaining the developments in this theme area]

### [Specific Topic 1.1]

[Detailed narrative content incorporating article details, ending with source links]

- [Article 1](link)
- [Article 2](link)

### [Specific Topic 1.2]

[Another detailed narrative section, ending with links]

- [Article 3](link)

## [Major Theme 2]

[Introduction for this theme area]

### [Specific Topic 2.1]

[Narrative content ending with links]

- [Article 4](link)
- [Article 5](link)

## Other Notable Developments

[Brief coverage of articles that don't fit major themes, grouped broadly when possible]

- [Article 6](link)
- [Article 7](link)
```

NARRATIVE STYLE EXAMPLES:

✅ GOOD (Natural, journalistic narrative):
"Developers can now deploy OpenAI's latest gpt-oss models directly in VS Code using the AI Toolkit's new catalog integration. This development enables completely offline LLM experimentation, addressing privacy concerns and cost constraints that have limited AI adoption in enterprise environments. The toolkit simplifies what was previously a complex multi-step process, offering one-click deployment alongside tools like Ollama and Azure AI Foundry."

❌ BAD (Obvious AI structure):
"- **What's New:** AI Toolkit now integrates catalog-driven local install of gpt-oss models
- **What Happened:** Step-by-step deployment instructions detail toolkit setup
- **Why it Helps Developers:** Opens up offline LLM development"

✅ GOOD (Flowing integration of benefits):
"The integration streamlines knowledge graph analytics through GraphRAG, making complex reporting instantly accessible in Teams and Copilot. Python-based MCP servers deployed on Azure connect seamlessly to Copilot Studio, transforming how developers expose custom analytics and insights to business users without extensive integration work."

❌ BAD (Mechanical listing):
"- **What's New:** Uses Python-based MCP servers deployed on Azure
- **What Happened:** Complete walkthrough from endpoint authoring to containerized deployment
- **How it Helps Developers:** Empowers developers to make custom analytics available"

THEME AND TOPIC IDENTIFICATION:
- Identify natural themes based on:
  * Product families (GitHub Copilot, Azure AI, .NET ecosystem, etc.)
  * Technology areas (AI/ML, cloud infrastructure, development tools, etc.)  
  * Development domains (web development, data science, DevOps, security, etc.)
  * Problem areas being solved (productivity, automation, collaboration, etc.)
- Create topic subsections for specific developments within each theme
- Group 2 or more related articles under topic headings when possible
- Use an "Other Notable Developments" section for standalone articles

IMPORTANCE-BASED SORTING REQUIREMENT:
- Please sort the roundup from most important news to least important
- Determine importance based on:
  * Frequency of topics discussed during the week (more mentions = higher importance)
  * Content significance and impact on developers (major releases, breaking changes, etc.)
  * Breadth of developer audience affected (widespread tools vs niche features)
  * Strategic significance for Microsoft ecosystem development
- Place most important themes and topics at the top of the roundup
- Within each theme, order topics by importance as well
- Use your judgment to assess article content significance when topic frequency is similar

INTEGRATION STRATEGY FOR NEW ARTICLES:
1. ANALYZE the new article for its primary theme and specific topic
2. CHECK if this theme already exists in the current roundup
3. IF THEME EXISTS:
   - Check if the specific topic already exists
   - If topic exists: Add article to existing topic with updated narrative
   - If topic is new: Create new topic subsection within the theme
4. IF THEME IS NEW:
   - Create new major theme section
   - Create appropriate topic subsection
   - Add comprehensive theme introduction
5. IF ARTICLE DOESN'T FIT MAJOR THEMES:
   - Add to "Other Notable Developments" section
   - Group with similar content when possible

CONTENT WRITING GUIDELINES:
- Write engaging narrative introductions for each theme (2-3 sentences max)
- Create natural, flowing narrative content focused on developer impact
- Include specific technical details that matter to developers seamlessly within narratives
- End each topic section with relevant article links
- Write like a tech journalist covering exciting developments for developers
- Avoid structured bullet points with "What's New:", "What Happened:", "Why it Helps:" headers
- Create readable, engaging content that flows naturally
- Keep narratives actionable and developer-focused

RESPONSE REQUIREMENTS:
- Please return the complete updated roundup content
- Include all previously integrated articles plus the new one
- Maintain all existing content while adding the new article
- Preserve existing sections and their content
- Keep all article links and titles exactly as provided
- Update narrative content to reflect the addition of new information

$WritingStyleGuidelines
"@

        # Process articles iteratively, starting from ResumeFromArticle if resuming
        $startArticleIndex = if ($ResumeFromBackup -and $StartFromStep -eq 2) { $ResumeFromArticle - 1 } else { 0 }
        
        for ($i = $startArticleIndex; $i -lt $articles.Count; $i++) {
            $articleFilePath = $articles[$i]
            $articleNum = $i + 1
            $articleName = Split-Path $articleFilePath -Leaf
            
            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] 🔄 Integrating article $articleNum of $($articles.Count): $($articleName)" -ForegroundColor Blue
            
            # Check if file exists
            if (-not (Test-Path $articleFilePath)) {
                throw "Article file not found: $articleFilePath"
            }
            
            # Read the article content
            $articleContent = Get-Content $articleFilePath -Raw
            
            # Extract metadata from frontmatter
            $canonicalUrl = Get-FrontMatterValue -Content $articleContent -Key "canonical_url"
            $title = Get-FrontMatterValue -Content $articleContent -Key "title"
            $viewingMode = Get-FrontMatterValue -Content $articleContent -Key "viewing_mode"
            $permalink = Get-FrontMatterValue -Content $articleContent -Key "permalink"
            
            # Determine the appropriate link format
            $articleLink = if ($viewingMode -eq "internal") {
                "[$title]({{ `"$permalink`" | relative_url }})"
            } else {
                "[$title]($canonicalUrl)"
            }

            # Create user message for this iteration
            if ($i -eq 0) {
                # First article - create initial structure
                $iterativeUserMessage = @"
CREATE INITIAL ROUNDUP STRUCTURE FOR THE FIRST ARTICLE:

ARTICLE TITLE: $title
ARTICLE LINK: $articleLink

ARTICLE CONTENT:
$articleContent

Create the initial roundup structure based on this first article. Write natural, engaging narrative content that seamlessly incorporates new developments, changes, and practical benefits for developers. Avoid structured bullet points or obvious AI formatting.
"@
            }
            else {
                # Subsequent articles - integrate into existing roundup
                $iterativeUserMessage = @"
INTEGRATE NEW ARTICLE INTO EXISTING ROUNDUP:

CURRENT ROUNDUP CONTENT:
$iterativeRoundup

---

NEW ARTICLE TO INTEGRATE:
ARTICLE TITLE: $title  
ARTICLE LINK: $articleLink

ARTICLE CONTENT:
$articleContent

Intelligently integrate this new article into the existing roundup structure. Write natural, engaging narrative content that seamlessly incorporates new developments, changes, and practical benefits for developers. Avoid structured bullet points or obvious AI formatting.
"@
            }

            # Save step 2 request/response files with numbered format and -input/-output suffixes
            $inputFileName = "step2-article-$($articleNum.ToString('000'))-input"
            $outputFileName = "step2-article-$($articleNum.ToString('000'))-output"
            
            Save-StepBackup -StepName $inputFileName -Content $iterativeUserMessage -OutputDirectory $outputDirectory
            
            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] 🤖 Calling AI model to integrate article $articleNum..." -ForegroundColor Blue
            $response = Invoke-AiApiCall `
                -Token $Token `
                -Model $Model `
                -SystemMessage $iterativeSystemMessage `
                -UserMessage $iterativeUserMessage `
                -Endpoint $Endpoint `
                -RateLimitPreventionDelay $RateLimitPreventionDelay

            # Check for errors with robust error handling
            $responseValidation = Test-AiResponseFormat -Response $response -StepName "Step 2 - Article $articleNum Integration"
            if (-not $responseValidation.IsValid) {
                $timestamp = Get-Date -Format "HH:mm:ss"
                Write-Host "[$timestamp] ❌ Step 2 failed for article $articleNum ($articleName): $($responseValidation.ErrorMessage)" -ForegroundColor Red
                Write-Host ""
                Write-Host "💡 To resume from this article, use: -StartFromStep 2 -ResumeFromArticle $articleNum" -ForegroundColor Cyan
                exit 1
            }

            # Update the iterative roundup with the new content
            $iterativeRoundup = $response
            
            # Save response output
            Save-StepBackup -StepName $outputFileName -Content $iterativeRoundup -OutputDirectory $outputDirectory
            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] ✅ Article $articleNum integrated successfully" -ForegroundColor Green
            
            # Small delay to prevent overwhelming the API
            Start-Sleep -Seconds 1
        }

        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] ✅ Step 2 complete - All $($articles.Count) articles integrated iteratively" -ForegroundColor Green
        Save-StepBackup -StepName "Step2-Final" -Content $iterativeRoundup -OutputDirectory $outputDirectory
    }
    else {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] ⏭️ Skipping Step 2 (iterative integration) - resuming from backup" -ForegroundColor Yellow
        if ($ResumeFromBackup) {
            $iterativeRoundup = $resumeContent
        }
    }

    # Step 3: Create Ongoing Narrative by Comparing with Previous Roundup (keeping from v1 but applied to Step 2 result)
    $step3Response = $null
    if ($StartFromStep -le 3) {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] 🔄 Step 3: Creating ongoing narrative by comparing with previous roundup..." -ForegroundColor Green

        # Find the most recent previous roundup
        $previousRoundup = $null
        $previousRoundupContent = ""
        
        if (Test-Path "_roundups") {
            $roundupFiles = Get-ChildItem "_roundups" -Filter "*.md" | Sort-Object Name -Descending
        
            # Find the most recent roundup that's older than our start date
            foreach ($file in $roundupFiles) {
                if ($file.Name -match '^(\d{4}-\d{2}-\d{2})-') {
                    $roundupDate = [DateTime]::ParseExact($matches[1], "yyyy-MM-dd", $null)
                    if ($roundupDate -le $startDateTime) {
                        $previousRoundup = $file.FullName
                        $previousRoundupContent = Get-Content $previousRoundup -Raw
                        break
                    }
                }
            }
        }
        else {
            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] ℹ️ No _roundups directory found" -ForegroundColor Yellow
        }

        if ($previousRoundup -and $previousRoundupContent) {
            $step3SystemMessage = @"
IMPORTANT: Please provide a complete and comprehensive response that includes all requested content.

ROLE: You are an expert content editor creating ongoing narrative connections between weekly tech roundups. Your task is to enhance the current week's roundup by identifying themes, products, or developments that continue from the previous week.

RESPONSE FORMAT:
Return the enhanced content with the EXACT same structure as provided (all topics and links preserved), but with added narrative connections where relevant.

ONGOING NARRATIVE ENHANCEMENT STRATEGY:
1. Identify products, features, or themes mentioned in BOTH the previous and current roundup
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

IMPORTANT: If no meaningful connections exist between the previous and current roundup, return the content unchanged. Only enhance where natural, relevant connections can be made.
"@

            $step3UserMessage = @"
PREVIOUS WEEK'S ROUNDUP CONTENT:
$previousRoundupContent

---

CURRENT WEEK'S ROUNDUP CONTENT TO ENHANCE:
$iterativeRoundup
"@

            Save-StepBackup -StepName "Step3-Request" -Content $step3UserMessage -OutputDirectory $outputDirectory

            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] 🤖 Calling AI model to create ongoing narrative connections..." -ForegroundColor Blue
            $step3Response = Invoke-AiApiCall `
                -Token $Token `
                -Model $Model `
                -SystemMessage $step3SystemMessage `
                -UserMessage $step3UserMessage `
                -Endpoint $Endpoint `
                -RateLimitPreventionDelay $RateLimitPreventionDelay

            # Check for errors with robust error handling  
            $responseValidation = Test-AiResponseFormat -Response $step3Response -StepName "Step 3 (Ongoing Narrative)"
            if (-not $responseValidation.IsValid) {
                $timestamp = Get-Date -Format "HH:mm:ss"
                Write-Host "[$timestamp] ⚠️ Warning: Step 3 ongoing narrative failed: $($responseValidation.ErrorMessage)" -ForegroundColor Yellow
                Write-Host "[$timestamp] Using original content..." -ForegroundColor Yellow
                $step3Response = $iterativeRoundup
            }
            else {
                Save-StepBackup -StepName "Step3-Response" -Content $step3Response -OutputDirectory $outputDirectory
                $timestamp = Get-Date -Format "HH:mm:ss"
                Write-Host "[$timestamp] ✅ Step 3 complete - Ongoing narrative connections added" -ForegroundColor Green
            }
        }
        else {
            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] ℹ️ No previous roundup found or empty content - skipping ongoing narrative step" -ForegroundColor Yellow
            $step3Response = $iterativeRoundup
        }
    }
    else {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] ⏭️ Skipping Step 3 (ongoing narrative) - resuming from backup" -ForegroundColor Yellow
        if ($ResumeFromBackup) {
            $step3Response = $resumeContent
        } else {
            $step3Response = $iterativeRoundup
        }
    }

    # Step 4: Condense Content (adapted from v1)
    $step4Response = $null
    if ($StartFromStep -le 4) {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] 📝 Step 4: Condensing content while maintaining narrative quality..." -ForegroundColor Green
        
        # If we're resuming from step 4 backup, use the backup content
        if ($ResumeFromBackup -and $StartFromStep -eq 4) {
            $step4Response = $resumeContent
            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] ✅ Step 4 complete - Resumed from backup" -ForegroundColor Green
        }
        else {
            $step4SystemMessage = @"
IMPORTANT: Please provide a complete response that includes all content sections.

CONTENT CONDENSATION TASK: Make every paragraph shorter and more concise while keeping all paragraphs, sections, structure, and content intact.

CONTENT PRESERVATION GUIDELINES:
- Every paragraph in the input should have a corresponding paragraph in the output
- Every section should remain in its current position
- Every article link should be preserved exactly as provided
- Every grouping of content should remain intact
- Every header structure should be maintained exactly

✅ WHAT YOU SHOULD DO - CONDENSE, DON'T REMOVE:
- Make sentences shorter and more direct
- Remove unnecessary words and phrases
- Combine related ideas into fewer words
- Eliminate redundant language
- Keep all technical details and specifics
- Maintain the same meaning with fewer words
- Preserve all article references and links

WHAT TO AVOID:
- Removing any paragraph, no matter how short
- Removing any article link or reference
- Removing any section or topic header
- Merging separate groups of articles into one
- Skipping any content with placeholder phrases
- Removing introductory text that precedes groups of article links
- Changing the fundamental meaning of any content

PROCESSING APPROACH:
For each paragraph you encounter:
1. Read the paragraph for unnecessary words
2. Rewrite it to be more concise while keeping the same meaning
3. Ensure all technical details are preserved
4. Move to the next paragraph and repeat

📋 STRUCTURE PRESERVATION:
- Keep ALL section headers (##) exactly as provided
- Keep ALL topic headers (###) exactly as provided  
- Maintain the exact same organization and flow
- Preserve all article groupings
- Keep all article links in their original positions

RESPONSE COMPLETION REQUIREMENT:
Please provide the complete condensed version of all content provided. Your response should end with the last article link from the last section, not with any summary statements.

$WritingStyleGuidelines

RESPONSE FORMAT: Return the condensed content with the exact same structure but with shorter, more concise paragraphs.
"@

            $step4UserMessage = @"
CONTENT TO CONDENSE:

$step3Response
"@

            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] 🤖 Calling AI model to condense content..." -ForegroundColor Blue
            $step4Response = Invoke-AiApiCall `
                -Token $Token `
                -Model $Model `
                -SystemMessage $step4SystemMessage `
                -UserMessage $step4UserMessage `
                -Endpoint $Endpoint `
                -RateLimitPreventionDelay $RateLimitPreventionDelay

            # Check for errors with robust error handling
            $responseValidation = Test-AiResponseFormat -Response $step4Response -StepName "Step 4 (Content Condensation)"
            if (-not $responseValidation.IsValid) {
                $timestamp = Get-Date -Format "HH:mm:ss"
                Write-Host "[$timestamp] ⚠️ Warning: Step 4 content condensation failed: $($responseValidation.ErrorMessage)" -ForegroundColor Yellow
                Write-Host "[$timestamp] Using original content..." -ForegroundColor Yellow
                $step4Response = $step3Response
            }
            else {
                Save-StepBackup -StepName "Step4-Condensed" -Content $step4Response -OutputDirectory $outputDirectory
                $timestamp = Get-Date -Format "HH:mm:ss"
                Write-Host "[$timestamp] ✅ Step 4 complete - Content condensed successfully" -ForegroundColor Green
            }
        }
    }
    else {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] ⏭️ Skipping Step 4 (content condensation) - resuming from backup" -ForegroundColor Yellow
        if ($ResumeFromBackup) {
            $step4Response = $resumeContent
        } else {
            $step4Response = $step3Response
        }
    }

    # Step 5: Generate Metadata (same as v1)
    $step5Response = $null
    if ($StartFromStep -le 5) {
        $timestamp = Get-Date -Format "HH:mm:ss"
        Write-Host "[$timestamp] 📊 Step 5: Generating metadata (title, tags, description, introduction)..." -ForegroundColor Green
        
        # If we're resuming from step 5 backup, use the backup content
        if ($ResumeFromBackup -and $StartFromStep -eq 5) {
            $step5Response = $resumeContent
            $timestamp = Get-Date -Format "HH:mm:ss"
            Write-Host "[$timestamp] ✅ Step 5 complete - Resumed from backup" -ForegroundColor Green
        }
        else {
            $step5SystemMessage = @"
IMPORTANT: Please provide a complete and comprehensive response that includes all requested metadata.

ROLE: You are an expert content strategist creating compelling metadata for a weekly tech roundup. Your task is to generate an engaging title, comprehensive tags, a clear description, and an informative introduction based on the roundup content.

RESPONSE FORMAT: Return ONLY a JSON object with this exact structure:
{
  "title": "Engaging Weekly Roundup Title",
  "description": "Clear, informative description (100-150 words max)",
  "tags": ["tag1", "tag2", "tag3", "tag4", "tag5", "tag6", "tag7", "tag8", "tag9", "tag10"],
  "introduction": "Comprehensive introduction paragraph (150-200 words)"
}

TITLE REQUIREMENTS:
- Create an engaging, specific title that captures the week's main themes
- Include the date range for the week (e.g., "July 21-27, 2025" format)
- Avoid generic phrases like "Weekly Roundup" alone
- Focus on the most significant developments or themes
- Examples: "AI Development Tools Advance: July 21-27, 2025 Tech Roundup"
- Keep it under 80 characters for SEO and readability

DESCRIPTION REQUIREMENTS (100-150 words):
- Provide a clear, informative summary of the week's key developments
- Mention the main themes and most significant announcements
- Focus on practical benefits and real-world impact
- Avoid starting with "In this roundup" or similar phrases
- Write in a professional, down-to-earth tone
- Include specific products, technologies, or developments mentioned

TAGS REQUIREMENTS (exactly 10 tags):
- Include all major technologies, products, and themes covered
- Use specific, searchable terms (e.g., "GitHub Copilot", "Azure AI", ".NET", "Visual Studio")
- Include both broad categories and specific products
- Prioritize tags that accurately represent the content
- Use proper capitalization and formatting
- Examples: ["GitHub Copilot", "Azure AI", "Machine Learning", "Visual Studio", ".NET", "DevOps", "Cloud Computing", "Developer Tools", "AI Models", "Security"]

INTRODUCTION REQUIREMENTS (150-200 words):
- Create a compelling opening paragraph that draws readers in
- Summarize the week's major themes and developments
- Provide context about why these developments matter
- Write in a flowing, narrative style that leads into the detailed content
- Reference the specific week dates naturally
- Focus on the practical impact for developers and tech professionals
- End with a transition that leads into the detailed roundup content

CONTENT ANALYSIS APPROACH:
1. Read through all sections to identify major themes
2. Note the most significant products, announcements, and developments
3. Identify target audience and practical benefits
4. Create metadata that accurately represents the content scope
5. Ensure all metadata elements work together cohesively

$WritingStyleGuidelines
"@

            $step5UserMessage = @"
ROUNDUP CONTENT FOR METADATA GENERATION:

$step4Response

Generate appropriate metadata (title, description, tags, introduction) for this weekly tech roundup covering $WeekDescription.
"@

            Write-Host "[$(Get-Date -Format 'HH:mm:ss')] 🤖 Calling AI model to generate metadata..."
            $step5Response = Invoke-AiApiCall `
                -Token $Token `
                -Model $Model `
                -SystemMessage $step5SystemMessage `
                -UserMessage $step5UserMessage `
                -Endpoint $Endpoint `
                -RateLimitPreventionDelay $RateLimitPreventionDelay

            # Check for errors with robust error handling
            $responseValidation = Test-AiResponseFormat -Response $step5Response -StepName "Step 5 (Metadata Generation)"
            if (-not $responseValidation.IsValid) {
                throw $responseValidation.ErrorMessage
            }

            Save-StepBackup -StepName "Step5-Metadata" -Content $step5Response -OutputDirectory $outputDirectory
            Write-Host "[$(Get-Date -Format 'HH:mm:ss')] ✅ Step 5 complete - Metadata generated successfully"
        }
    }
    else {
        Write-Host "⏭️ Skipping Step 5 (metadata generation) - resuming from backup"
        if ($ResumeFromBackup) {
            $step5Response = $resumeContent
        } else {
            Write-Error "Step 5 requires generated metadata. Please run from Step 5 or provide a backup."
            exit 1
        }
    }

    # Step 6: Create Final Markdown File and Table of Contents (same as v1)
    if ($StartFromStep -le 6) {
        Write-Host "📄 Step 6: Creating final markdown file with table of contents..."
        
        # If we're resuming from step 6 backup, the backup content should be the final content
        if ($ResumeFromBackup -and $StartFromStep -eq 6) {
            $finalContent = $resumeContent
            Write-Host "✅ Step 6 complete - Resumed from backup"
        }
        else {
            # Parse the metadata JSON from step 5
            try {
                $metadata = $step5Response | ConvertFrom-Json
            }
            catch {
                Write-Host "[$(Get-Date -Format 'HH:mm:ss')] ❌ Failed to parse metadata JSON from Step 5" -ForegroundColor Red
                Write-Host "[$(Get-Date -Format 'HH:mm:ss')] Step 5 response: $step5Response" -ForegroundColor Red
                Write-Host "[$(Get-Date -Format 'HH:mm:ss')] Parse error: $($_.Exception.Message)" -ForegroundColor Red
                Write-Error "Failed to parse metadata JSON from Step 5: $($_.Exception.Message)"
                exit 1
            }
            
            # Create table of contents by parsing the content from step 4
            $tableOfContents = ""
            $contentLines = $step4Response -split "`n"
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

$step4Response
"@
            
            Save-StepBackup -StepName "Step6-IncludingFrontmatter" -Content $finalContent -OutputDirectory $outputDirectory
            Write-Host "✅ Step 6 complete - Markdown file including frontmatter created"
        }
    }
    else {
        Write-Host "⏭️ Skipping Step 6 (markdown + frontmatter file creation) - resuming from backup"
        # If a backup file is explicitly provided, always use that
        if ($ResumeFromBackup) {
            $finalContent = $resumeContent
            Write-Host "📂 Using provided backup file" -ForegroundColor Cyan
        }
        else {
            Write-Error "Step 6 requires a backup file to continue. Please provide -ResumeFromBackup parameter."
            exit 1
        }
    }

    # Step 7: Rewrite Content Against Writing Style Guidelines (same as v1)
    # If we're resuming from step 7 backup, the backup content should be the final content
    if ($ResumeFromBackup -and $StartFromStep -eq 7) {
        $finalContent = $resumeContent
        Write-Host "✏️ Step 7: Rewriting content against writing style guidelines... - resuming from backup"
    }
    else {    
        Write-Host "[$(Get-Date -Format 'HH:mm:ss')] ✏️ Step 7: Rewriting content against writing style guidelines..."
    }
    
    # Read the writing style guidelines
    $guidelinesPath = "/workspaces/techhub/docs/writing-style-guidelines.md"
    if (-not (Test-Path $guidelinesPath)) {
        throw "⚠️ Writing style guidelines not found at $guidelinesPath"
    }

    try {
        $guidelines = Get-Content $guidelinesPath -Raw -Encoding UTF8

        $rewriteSystemMessage = @"
IMPORTANT: Please provide a complete response that includes all content sections with improved writing style.

CONTENT REWRITING TASK: Rewrite every paragraph and the frontmatter title and description to ensure writing style guideline compliance while keeping all paragraphs, sections, structure, and content intact.

CONTENT PRESERVATION GUIDELINES:
- Every paragraph in the input should have a corresponding paragraph in the output
- Every section should remain in its current position
- Every article link should be preserved exactly as provided
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

WHAT TO AVOID:
- Removing any paragraph, no matter how short or seemingly unimportant
- Removing any article link or reference
- Removing any section or topic header
- Merging separate groups of articles into one large list
- Skipping any content with placeholder phrases
- Removing introductory text that precedes groups of article links (these provide readability structure)
- Deleting references to previous roundups or ongoing narratives
- Changing the fundamental meaning or technical accuracy of content
- Removing or modifying any frontmatter fields except title and description
- Changing dates, permalinks, categories, tags, or any other frontmatter fields

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

RESPONSE COMPLETION REQUIREMENT:
Please provide the complete rewritten version of all content provided. Your response should end with the last article link from the last section, not with any summary statements or notes about continuation.

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

        Write-Host "[$(Get-Date -Format 'HH:mm:ss')] 🤖 Calling AI model to rewrite content for writing style compliance..."
        $finalContent = Invoke-AiApiCall `
            -Token $Token `
            -Model $Model `
            -SystemMessage $rewriteSystemMessage `
            -UserMessage $rewriteUserMessage `
            -Endpoint $Endpoint `
            -RateLimitPreventionDelay $RateLimitPreventionDelay

        # Check for errors with robust error handling
        $rewriteResult = Test-AiResponseFormat -Response $finalContent -StepName "Step 7 (Content Rewriting)"
        if (-not $rewriteResult.IsValid) {
            Write-Host "[$(Get-Date -Format 'HH:mm:ss')] ⚠️ Warning: Step 7 content rewriting failed: $($rewriteResult.ErrorMessage)" -ForegroundColor Yellow
            Write-Host "[$(Get-Date -Format 'HH:mm:ss')] Using original content..." -ForegroundColor Yellow
            # finalContent already contains the original content, so no change needed
        }
        else {
            # Save successful rewrite results
            Save-StepBackup -StepName "Step7-RewriteResult" -Content $finalContent -OutputDirectory $outputDirectory
            Write-Host "✅ Step 7 complete - Content rewritten for writing style compliance"
        }
       
    }
    catch {
        Write-Host "[$(Get-Date -Format 'HH:mm:ss')] ⚠️ Warning: Error during writing style rewriting: $($_.Exception.Message)" -ForegroundColor Yellow
        Write-Host "[$(Get-Date -Format 'HH:mm:ss')] Continuing with file creation using original content..." -ForegroundColor Yellow
    }

    # Generate the OutputFile in the output directory instead of _roundups/
    if ($outputDirectory) {
        $OutputFile = Join-Path $outputDirectory "$filename.md"
    } else {
        $OutputFile = "_roundups/$filename.md"
    }

    # Ensure output directory exists
    $outputDir = Split-Path $OutputFile -Parent
    if ($outputDir -and -not (Test-Path $outputDir)) {
        New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
    }

    # Write final roundup to file
    $finalContent | Out-File -FilePath $OutputFile -Encoding UTF8 -Force

    # Format the created file using the fix-markdown-files script
    Write-Host "[$(Get-Date -Format 'HH:mm:ss')] 🔧 Formatting the created roundup file..."
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

    Write-Host "[$(Get-Date -Format 'HH:mm:ss')] 🎉 Roundup generation complete!"
    Write-Host "[$(Get-Date -Format 'HH:mm:ss')] 📄 Final roundup saved to: $OutputFile"
    Write-Host "[$(Get-Date -Format 'HH:mm:ss')] 📊 Processed $($articles.Count) articles in iterative approach"
}
catch {
    Write-ErrorDetails -ErrorRecord $_ -Context "Roundup generation v2"
    throw
}
