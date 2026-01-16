# IMPORTANT: These tests intentionally do NOT use -WhatIf parameter because they must verify
# actual file operations (creating markdown files, writing to tracking files). The tests are
# safe because they use isolated test directories and proper BeforeEach/AfterAll cleanup.

Describe "Convert-RssToMarkdown" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"

        $script:TestScriptsPath = Join-Path $script:TempPath "scripts"
        $script:TestContentProcessingPath = Join-Path $script:TestScriptsPath "content-processing"
        $script:TestDataPath = Join-Path $script:TestScriptsPath "data"
        $script:TestOutputDir = Join-Path $script:TempPath "_blogs"
        $script:TestCommunityOutputDir = Join-Path $script:TempPath "_community"
        $script:TestAiResultsDir = Join-Path $script:TestScriptsPath "ai-results"
        $script:TestTemplatesPath = Join-Path $script:TestContentProcessingPath "templates"
        $script:TestSkippedEntriesPath = Join-Path $script:TestDataPath "skipped-entries.json"
        $script:TestProcessedEntriesPath = Join-Path $script:TestDataPath "processed-entries.json"
        
        # Create source location for template files that BeforeEach can copy from
        $script:TemplateSourcePath = Join-Path $script:TempPath "templates-source"
       
        # Create test items array directly (no Feed object needed)
        $script:TestItems = @(
            [PSCustomObject]@{
                Title           = "Test Article"
                Link            = "https://example.com/test-article"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "This is a test article description. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " * 5) # Make longer than 500 chars to avoid HTTP fetch
                Author          = "Test Author"
                Tags            = @("AI", "Testing")
                OutputDir       = $script:TestOutputDir
                FeedName        = "Test Feed"
                FeedUrl         = "https://example.com/feed"
                EnhancedContent = "Test enhanced content for processing"
            }
        )
        
        # Create additional test data for specialized tests
        $script:unquotedTestItems = @(
            [PSCustomObject]@{
                Title           = "Unquoted Test Article"
                Link            = "https://example.com/unquoted-test-article"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "This is a test article for unquoted test. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " * 5)
                Author          = "Test Author"
                Tags            = @("AI", "Testing")
                OutputDir       = $script:TestOutputDir
                FeedName        = "Test Feed"
                FeedUrl         = "https://example.com/feed"
                EnhancedContent = "Test enhanced content for processing"
            }
        )
        
        $script:shortTestItems = @(
            [PSCustomObject]@{
                Title           = "Short Description Article"
                Link            = "https://example.com/short-article"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "Short description under 500 chars"
                Author          = "Test Author"
                Tags            = @("AI", "Testing")
                OutputDir       = $script:TestOutputDir
                FeedName        = "Test Feed"
                FeedUrl         = "https://example.com/feed"
                EnhancedContent = "Test enhanced content for processing"
            }
        )
        
        $script:youTubeTestItems = @(
            [PSCustomObject]@{
                Title           = "YouTube Video Article"
                Link            = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "This is a YouTube video description. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit. " * 10)
                Author          = "YouTube Creator"
                Tags            = @("Video", "YouTube")
                OutputDir       = Join-Path $script:TempPath "_videos"
                FeedName        = "Test Feed"
                FeedUrl         = "https://example.com/feed"
                EnhancedContent = "Test enhanced content for processing"
            }
        )
        
        $script:redditTestItems = @(
            [PSCustomObject]@{
                Title           = "Reddit Discussion Article"
                Link            = "https://www.reddit.com/r/programming/comments/test"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "This is a Reddit discussion. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit. " * 10)
                Author          = "Reddit User"
                Tags            = @("Community", "Reddit")
                OutputDir       = Join-Path $script:TempPath "_community"
                FeedName        = "Test Feed"
                FeedUrl         = "https://example.com/feed"
                EnhancedContent = "Test enhanced content for processing"
            }
        )
        
        $script:errorTestItems = @(
            [PSCustomObject]@{
                Title           = "Error Test Article"
                Link            = "https://example.com/error-article"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "This article will cause errors. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit. " * 10)
                Author          = "Error Author"
                Tags            = @("Error", "Testing")
                OutputDir       = $script:TestOutputDir
                FeedName        = "Test Feed"
                FeedUrl         = "https://example.com/feed"
                EnhancedContent = "Test enhanced content for processing"
            }
        )
        
        $script:communityTestItems = @(
            [PSCustomObject]@{
                Title           = "Community Article"
                Link            = "https://example.com/community-article"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "This is a community article. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit. " * 10)
                Author          = "Community Author"
                Tags            = @("Community", "Testing")
                OutputDir       = Join-Path $script:TempPath "_community"
                FeedName        = "Test Feed"
                FeedUrl         = "https://example.com/feed"
                EnhancedContent = "This is a comprehensive community article that provides substantial value to readers interested in technology and software development. The article discusses various aspects of community-driven development, including collaboration strategies, best practices for open source contributions, and effective communication techniques in distributed teams. It covers practical applications of community management principles and provides detailed examples of successful community initiatives. The content explores methodologies for building engaged developer communities, common challenges in community building, and recommendations for fostering inclusive and productive environments. Additionally, it discusses the importance of documentation, mentorship programs, and knowledge sharing platforms in creating thriving technical communities. The article also covers governance models, conflict resolution strategies, and sustainability considerations for long-term community success."
            }
        )
        
        $script:blogsTestItems = @(
            [PSCustomObject]@{
                Title           = "Blogs Article"
                Link            = "https://example.com/blogs-article"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "This is a blogs article. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit. " * 10)
                Author          = "Blogs Author"
                Tags            = @("Blogs", "Testing")
                OutputDir       = $script:TestOutputDir
                FeedName        = "Test Feed"
                FeedUrl         = "https://example.com/feed"
                EnhancedContent = "Test enhanced content for processing"
            }
        )
    }
    
    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"        

        New-Item -Path $script:TemplateSourcePath -ItemType Directory -Force | Out-Null

        # Create template files once in source location
        $genericTemplate = @'
{{FRONTMATTER}}
{{EXCERPT}}<!--excerpt_end-->

{{CONTENT}}

This post appeared first on {{FEEDNAME}}. [Read the entire article here]({{CANONICAL_URL}})
'@
        Set-Content -Path (Join-Path $script:TemplateSourcePath "template-generic.md") -Value $genericTemplate
        
        $videoTemplate = @'
{{FRONTMATTER}}
{{EXCERPT}}<!--excerpt_end-->

{% youtube {{YOUTUBE_ID}} %}

{{CONTENT}}
'@
        Set-Content -Path (Join-Path $script:TemplateSourcePath "template-videos.md") -Value $videoTemplate

        # Recreate directory structure (Initialize-BeforeEach.ps1 removes TestSourceRoot)
        New-Item -Path $script:TempPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestScriptsPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestContentProcessingPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestDataPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestOutputDir -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestAiResultsDir -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestTemplatesPath -ItemType Directory -Force | Out-Null
        
        # Copy template files from BeforeAll setup
        Copy-Item -Path (Join-Path $script:TemplateSourcePath "*") -Destination $script:TestTemplatesPath -Force
        
        # Reset tracking files for test isolation
        Set-Content -Path $script:TestSkippedEntriesPath -Value "[]"
        Set-Content -Path $script:TestProcessedEntriesPath -Value "[]"
        
        # Create collection directories and clear any existing files
        $collectionDirs = @("_blogs", "_community", "_news", "_videos", "_events", "_roundups")
        foreach ($dir in $collectionDirs) {
            $fullPath = Join-Path $script:TempPath $dir
            New-Item -Path $fullPath -ItemType Directory -Force | Out-Null
            Get-ChildItem -Path $fullPath -Filter "*.md" -ErrorAction SilentlyContinue | Remove-Item -Force
        }

        # Reset test items to fresh state for each test
        $script:testItems = @(
            [PSCustomObject]@{
                Title           = "Test Article"
                Link            = "https://example.com/test-article"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "Test article description. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit. " * 10)
                Author          = "Test Author"
                Tags            = @("AI", "Testing")
                OutputDir       = $script:TestOutputDir
                FeedName        = "Test Feed"
                FeedUrl         = "https://example.com/feed"
                EnhancedContent = "Test enhanced content for processing"
            }
        )

        Mock Get-SourceRoot { return $script:TempPath }
    }
    
    AfterAll {
        # Clean up test files
        if (Test-Path $script:TempPath) {
            Remove-Item -Path $script:TempPath -Recurse -Force
        }
    }
    
    Context "Content Filter Error Handling" {
        It "Should add items to skipped entries when content is filtered" {
            # Mock network calls to simulate production behavior
            Mock Invoke-WebRequest {
                if ($Uri -like "https://example.com/test-article*") {
                    # Simulate article content that would normally be extracted
                    $htmlContent = @"
<!DOCTYPE html>
<html>
<head><title>Test Article</title></head>
<body>
<h1>Test Article</h1>
<p>This is test article content that would normally be processed by AI.</p>
<p>Author: Test Author</p>
</body>
</html>
"@
                    return [PSCustomObject]@{
                        StatusCode = 200
                        Content    = $htmlContent
                        Headers    = @{ 'Content-Type' = 'text/html' }
                    }
                }
                throw "Unexpected URL: $Uri"
            }
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate Azure OpenAI content filter response
                    throw [System.Net.WebException]::new("The response was filtered due to the prompt triggering Azure OpenAI's content management policy.")
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Mock Invoke-ProcessWithAiModel to return content filter error object
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    Error   = $true
                    Type    = "ContentFilter"
                    Pattern = "content.*filter"
                }
            }
            
            # Run the conversion (NOTE: -WhatIf is intentionally NOT used here because this test must 
            # verify that entries are actually written to the skipped entries file. The mocked API 
            # call prevents markdown file creation, so only tracking files are modified.)
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 0 new files (since content was filtered)
            $result | Should -Be 0
            
            # Check that the item was added to skipped entries
            $skippedEntries = Get-Content $script:TestSkippedEntriesPath | ConvertFrom-Json
            $skippedEntries | Should -HaveCount 1
            $skippedEntries[0].canonical_url | Should -Be "https://example.com/test-article"
            $skippedEntries[0].reason | Should -Be "Content blocked by safety filters"
        }
        
        It "Should continue processing other items after content filter error" {
            # Mock network calls to simulate production behavior for multiple articles
            Mock Invoke-WebRequest {
                if ($Uri -like "https://example.com/filtered-article*") {
                    # Simulate content that would be filtered
                    $htmlContent = @"
<!DOCTYPE html>
<html>
<head><title>Filtered Article</title></head>
<body>
<h1>Filtered Article</h1>
<p>This content contains material that would trigger content filters.</p>
<p>Author: Test Author</p>
</body>
</html>
"@
                    return [PSCustomObject]@{
                        StatusCode = 200
                        Content    = $htmlContent
                        Headers    = @{ 'Content-Type' = 'text/html' }
                    }
                }
                elseif ($Uri -like "https://example.com/good-article*") {
                    # Simulate good content that would pass filters
                    $htmlContent = @"
<!DOCTYPE html>
<html>
<head><title>Good Article</title></head>
<body>
<h1>Good Article</h1>
<p>This is good article content that would pass all filters and be processed successfully.</p>
<p>Author: Test Author</p>
</body>
</html>
"@
                    return [PSCustomObject]@{
                        StatusCode = 200
                        Content    = $htmlContent
                        Headers    = @{ 'Content-Type' = 'text/html' }
                    }
                }
                throw "Unexpected URL: $Uri"
            }
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # First call fails with content filter, second succeeds
                    if (-not $script:RestCallCount) { $script:RestCallCount = 0 }
                    $script:RestCallCount++
                    if ($script:RestCallCount -eq 1) {
                        throw [System.Net.WebException]::new("The response was filtered due to the prompt triggering Azure OpenAI's content management policy.")
                    }
                    else {
                        # Simulate successful AI response for good article
                        return [PSCustomObject]@{
                            choices = @(
                                [PSCustomObject]@{
                                    message = [PSCustomObject]@{
                                        content = '{"title":"Good Article","description":"Processed description","content":"Processed content","excerpt":"Processed excerpt","categories":["Testing"],"tags":["Testing","Good"]}'
                                    }
                                }
                            )
                        }
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Initialize call counters
            $script:RestCallCount = 0
            
            # Create multiple test items for this specific test
            $script:testItems = @(
                [PSCustomObject]@{
                    Title           = "Filtered Article"
                    Link            = "https://example.com/filtered-article"
                    Description     = "This article will be filtered. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit. " * 10) # Make it longer than 500 chars to avoid HTTP fetch
                    Author          = "Test Author"
                    Tags            = @("AI")
                    PubDate         = [DateTime]::Parse("2025-01-01T13:00:00Z")  # Later date so it processes first
                    OutputDir       = $script:TestOutputDir
                    FeedName        = "Test Feed"
                    FeedUrl         = "https://example.com/feed"
                    EnhancedContent = "Test enhanced content for processing"
                },
                [PSCustomObject]@{
                    Title           = "Good Article"
                    Link            = "https://example.com/good-article" 
                    Description     = "This article will succeed. " + ("Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. " * 8) # Make it longer than 500 chars to avoid HTTP fetch
                    Author          = "Test Author"
                    Tags            = @("Testing")
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")  # Earlier date so it processes second
                    OutputDir       = $script:TestOutputDir
                    FeedName        = "Test Feed"
                    FeedUrl         = "https://example.com/feed"
                    EnhancedContent = "Test enhanced content for processing"
                }
            )
            
            # Mock to fail first item, succeed second
            $script:CallCount = 0
            Mock Invoke-ProcessWithAiModel {
                $script:CallCount++
                if ($script:CallCount -eq 1) {
                    return [PSCustomObject]@{
                        Error   = $true
                        Type    = "ContentFilter"
                        Pattern = "Content was blocked by safety filters"
                    }
                }
                else {
                    return [PSCustomObject]@{
                        title       = "Good Article"
                        description = "Processed description"
                        content     = "Processed content"
                        excerpt     = "Processed excerpt"
                        categories  = @("Testing")
                        tags        = @("Testing", "Good")
                    }
                }
            }
            
            # Run the conversion (NOTE: -WhatIf is intentionally NOT used here because this test must 
            # verify that one file is actually created and one entry is written to skipped entries)
            $result = Convert-RssToMarkdown -Items $script:testItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 1 new file (second item succeeded)
            $result | Should -Be 1
            
            # Check that the first item was added to skipped entries
            $skippedEntries = Get-Content $script:TestSkippedEntriesPath | ConvertFrom-Json
            $filteredEntry = $skippedEntries | Where-Object { $_.canonical_url -eq "https://example.com/filtered-article" }
            $filteredEntry | Should -Not -BeNullOrEmpty
            $filteredEntry.reason | Should -Be "Content blocked by safety filters"
        }
    }
    
    Context "API Error Handling" {
        It "Should throw exception for unknown error types" {
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate API error response
                    throw [System.Net.WebException]::new("Network error occurred")
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Mock Invoke-ProcessWithAiModel to return unknown error type (which will cause exception)
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    Error   = $true
                    Type    = "APIError"
                    Message = "Network error occurred"
                }
            }
            
            # Should throw because APIError is not a recognized error type  
            { Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions" } |
            Should -Throw "*Unknown error type: Type: APIError, Message: Network error occurred*"
        }
        
        It "Should stop processing on rate limit errors" {
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate rate limit response
                    $response = [PSCustomObject]@{
                        StatusCode = 429
                        Headers    = @{
                            'Retry-After' = '300'
                        }
                    }
                    $exception = [System.Net.WebException]::new("Too Many Requests")
                    $exception.Data.Add('Response', $response)
                    throw $exception
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Mock Invoke-ProcessWithAiModel to return rate limit error
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    Error            = $true
                    Type             = "RateLimit"
                    RateLimitSeconds = 300
                }
            }
            
            # Run the conversion
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 0 new files
            $result | Should -Be 0
            
            # Should not add to skipped entries for rate limit (processing should stop)
            $skippedEntries = Get-Content $script:TestSkippedEntriesPath | ConvertFrom-Json
            $skippedEntries | Should -HaveCount 0
        }
        
        It "Should handle JSON parse errors and skip items" {
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate successful API call but invalid JSON response
                    return @{
                        StatusCode = 200
                        Content    = '{"choices":[{"message":{"content":".NET development with Azure Functions offers powerful..."}}]}'
                        Headers    = @{ 'Content-Type' = 'application/json' }
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Mock Invoke-ProcessWithAiModel to return JSON parse error object
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    Error           = $true
                    Type            = "JsonParseError"
                    ResponseContent = '{"choices":[{"message":{"content":".NET development with Azure Functions offers powerful..."}}]}'
                    ContentString   = ".NET development with Azure Functions offers powerful..."
                }
            }
            
            # Mock Save-AiApiResult to verify it's called for debugging
            Mock Save-AiApiResult {}
            
            # Run the conversion (NOTE: -WhatIf is intentionally NOT used here because this test must 
            # verify that entries are actually written to the skipped entries file.)
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 0 new files (since JSON parsing failed)
            $result | Should -Be 0
            
            # Check that the item was added to skipped entries
            $skippedEntries = Get-Content $script:TestSkippedEntriesPath | ConvertFrom-Json
            $skippedEntries | Should -HaveCount 1
            $skippedEntries[0].canonical_url | Should -Be "https://example.com/test-article"
            $skippedEntries[0].reason | Should -Be "AI model response could not be parsed as JSON"
            
            # Verify that Save-AiApiResult was called for debugging purposes
            Should -Invoke Save-AiApiResult -Times 1 -ParameterFilter {
                $PubDate -eq [DateTime]::Parse("2025-01-01T12:00:00Z")
            }
        }
    }
    
    Context "Early File Removal Based on Canonical URL" {
        It "Should remove existing file early in processing before AI call" {
            # Create an existing file with the same canonical URL
            $existingContent = @'
---
title: "Old Title"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://example.com/test-article"
author: "Old Author"
section_names: ["ai"]
tags: ["Old", "Tags"]
---

This is old content that should be replaced.
'@
            $existingFilename = "2024-12-01-Old-Title.md"
            $existingFilePath = Join-Path $script:TestOutputDir $existingFilename
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Verify the existing file was created
            Test-Path $existingFilePath | Should -Be $true
            
            # Mock successful API response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "New Updated Title"
                    description = "This is an updated article"
                    content     = "# Updated Article\n\nThis is updated content."
                    excerpt     = "Updated excerpt"
                    categories  = @("AI", "Testing")
                    tags        = @("GitHub", "PowerShell")
                }
            }
            
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate successful AI response
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"New Updated Title","description":"This is an updated article","content":"# Updated Article\\n\\nThis is updated content.","excerpt":"Updated excerpt","categories":["AI","Testing"],"tags":["GitHub","PowerShell"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Run the conversion - should remove old file early and create new one
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 1 new file
            $result | Should -Be 1
            
            # Old file should be removed
            Test-Path $existingFilePath | Should -Be $false
            
            # New file should be created with updated content
            $newFiles = Get-ChildItem -Path $script:TestOutputDir -Filter "*.md"
            $newFiles | Should -HaveCount 1
            $newFiles[0].Name | Should -Match "^2025-01-01-.*\.md$"
            
            # Verify new file has correct canonical URL
            $newContent = Get-Content $newFiles[0].FullName -Raw
            $newContent | Should -Match 'canonical_url:\s*"?https://example\.com/test-article"?'
            $newContent | Should -Match 'title:\s*"?New Updated Title"?'
        }
        
        It "Should remove existing file even if AI processing fails later" {
            # Create an existing file with the same canonical URL
            $existingContent = @'
---
title: "File That Will Be Removed"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://example.com/test-article"
author: "Test Author"
section_names: ["ai"]
tags: ["Test"]
---

This file should be removed even if AI processing fails.
'@
            $existingFilename = "2024-12-01-File-That-Will-Be-Removed.md"
            $existingFilePath = Join-Path $script:TestOutputDir $existingFilename
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Verify the existing file was created
            Test-Path $existingFilePath | Should -Be $true
            
            # Mock AI processing to fail with content filter
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    Error   = $true
                    Type    = "ContentFilter"
                    Pattern = "content.*filter"
                }
            }
            
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    throw [System.Net.WebException]::new("The response was filtered due to the prompt triggering Azure OpenAI's content management policy.")
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Run the conversion - file should be removed even though AI processing fails
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 0 new files (AI processing failed)
            $result | Should -Be 0
            
            # Old file should still be removed (this happens before AI processing)
            Test-Path $existingFilePath | Should -Be $false
            
            # No new files should be created
            $newFiles = Get-ChildItem -Path $script:TestOutputDir -Filter "*.md"
            $newFiles | Should -HaveCount 0
            
            # Entry should be added to skipped entries
            $skippedEntries = Get-Content $script:TestSkippedEntriesPath | ConvertFrom-Json
            $skippedEntries | Should -HaveCount 1
            $skippedEntries[0].canonical_url | Should -Be "https://example.com/test-article"
        }
        
        It "Should remove existing file with same canonical_url before creating new one" {
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate successful AI response
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"New Updated Title","description":"This is an updated article","content":"# Updated Article\\n\\nThis is updated content.","excerpt":"Updated excerpt","categories":["AI","Testing"],"tags":["GitHub","PowerShell"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Create an existing file with the same canonical URL
            $existingContent = @'
---
title: "Old Title"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://example.com/test-article"
author: "Old Author"
section_names: ["ai"]
tags: ["Old", "Tags"]
---

This is old content that should be replaced.
'@
            $existingFilename = "2024-12-01-Old-Title.md"
            $existingFilePath = Join-Path $script:TestOutputDir $existingFilename
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Verify the existing file was created
            Test-Path $existingFilePath | Should -Be $true
            
            # Mock successful API response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "New Updated Title"
                    description = "This is an updated article"
                    content     = "# Updated Article\n\nThis is updated content."
                    excerpt     = "Updated excerpt"
                    categories  = @("AI", "Testing")
                    tags        = @("GitHub", "PowerShell")
                }
            }
            
            # Run the conversion - should remove old file and create new one
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 1 new file
            $result | Should -Be 1
            
            # Old file should be removed
            Test-Path $existingFilePath | Should -Be $false
            
            # New file should be created with updated content
            $newFiles = Get-ChildItem -Path $script:TestOutputDir -Filter "*.md"
            $newFiles | Should -HaveCount 1
            $newFiles[0].Name | Should -Match "^2025-01-01-.*\.md$"
            
            # Verify new file has correct canonical URL
            $newContent = Get-Content $newFiles[0].FullName -Raw
            $newContent | Should -Match 'canonical_url:\s*"?https://example\.com/test-article"?'
            $newContent | Should -Match 'title:\s*"?New Updated Title"?'
        }
        
        It "Should handle multiple existing files but only remove the one with matching canonical_url" {
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate successful AI response
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"Updated Target Article","description":"This is an updated target article","content":"# Updated Target\\n\\nThis is updated content.","excerpt":"Updated excerpt","categories":["AI","Testing"],"tags":["GitHub","PowerShell"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Create multiple existing files with different canonical URLs
            $existingContent1 = @'
---
title: "Different Article"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://example.com/different-article"
author: "Different Author"
section_names: ["ai"]
tags: ["Different"]
---

This is a different article.
'@
            $existingContent2 = @'
---
title: "Target Article"
date: 2024-12-01 11:00:00 +00:00
canonical_url: "https://example.com/test-article"
author: "Target Author"
section_names: ["ai"]
tags: ["Target"]
---

This is the target article that should be removed.
'@
            
            $existingFile1 = Join-Path $script:TestOutputDir "2024-12-01-Different-Article.md"
            $existingFile2 = Join-Path $script:TestOutputDir "2024-12-01-Target-Article.md"
            
            Set-Content -Path $existingFile1 -Value $existingContent1 -Encoding UTF8
            Set-Content -Path $existingFile2 -Value $existingContent2 -Encoding UTF8
            
            # Verify both files exist
            Test-Path $existingFile1 | Should -Be $true
            Test-Path $existingFile2 | Should -Be $true
            
            # Mock successful API response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Updated Target Article"
                    description = "This is an updated target article"
                    content     = "# Updated Target\n\nThis is updated content."
                    excerpt     = "Updated excerpt"
                    categories  = @("AI", "Testing")
                    tags        = @("GitHub", "PowerShell")
                }
            }
            
            # Run the conversion
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 1 new file
            $result | Should -Be 1
            
            # Different article should still exist
            Test-Path $existingFile1 | Should -Be $true
            
            # Target article should be removed
            Test-Path $existingFile2 | Should -Be $false
            
            # Should have 2 files total (1 untouched + 1 new)
            $allFiles = Get-ChildItem -Path $script:TestOutputDir -Filter "*.md"
            $allFiles | Should -HaveCount 2
        }
        
        It "Should handle malformed frontmatter gracefully when checking canonical_url" {
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate successful AI response
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"New Article","description":"This is a new article","content":"# New Article\\n\\nThis is new content.","excerpt":"New excerpt","categories":["AI","Testing"],"tags":["GitHub","PowerShell"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Create file with malformed frontmatter but valid canonical_url line
            $malformedContent = @'
---
title: "Malformed File"
canonical_url: https://example.com/different-malformed-url
date: 2024-12-01
invalid yaml here!!!
author: "Test"
---

This file has malformed YAML but contains a different URL so should not be removed.
'@
            $malformedFile = Join-Path $script:TestOutputDir "2024-12-01-Malformed.md"
            Set-Content -Path $malformedFile -Value $malformedContent -Encoding UTF8
            
            # Mock successful API response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "New Article"
                    description = "This is a new article"
                    content     = "# New Article\n\nThis is new content."
                    excerpt     = "New excerpt"
                    categories  = @("AI", "Testing")
                    tags        = @("GitHub", "PowerShell")
                } 
            }
            
            # Should not throw an exception even with malformed file
            { Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions" } | Should -Not -Throw
            
            # Malformed file should still exist (not removed due to different URL)
            Test-Path $malformedFile | Should -Be $true
            
            # Should have created the new file as well
            $allFiles = Get-ChildItem -Path $script:TestOutputDir -Filter "*.md"
            $allFiles | Should -HaveCount 2
        }
        
        It "Should support both quoted and unquoted canonical_url formats" {
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate successful AI response
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"New Article","description":"This is a new article","content":"# New Article\\n\\nThis is new content.","excerpt":"New excerpt","categories":["AI","Testing"],"tags":["GitHub","PowerShell"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Test with quoted canonical_url
            $quotedContent = @'
---
title: "Quoted URL Article"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://example.com/test-article"
author: "Quoted Author"
section_names: ["ai"]
tags: ["Quoted"]
---

This has a quoted canonical URL.
'@
            $quotedFile = Join-Path $script:TestOutputDir "2024-12-01-Quoted.md"
            Set-Content -Path $quotedFile -Value $quotedContent -Encoding UTF8
            
            # Mock successful API response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "New Article"
                    description = "This is a new article"
                    content     = "# New Article\n\nThis is new content."
                    excerpt     = "New excerpt"
                    categories  = @("AI", "Testing")
                    tags        = @("GitHub", "PowerShell")
                } 
            }
            
            # Run conversion
            Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Quoted file should be removed
            Test-Path $quotedFile | Should -Be $false
            
            # Clean up for next test
            Get-ChildItem -Path $script:TestOutputDir -Filter "*.md" | Remove-Item -Force
            
            # Test with unquoted canonical_url - create a new feed item for this test
            $unquotedContent = @'
---
title: "Unquoted URL Article"
date: 2024-12-01 10:00:00 +00:00
canonical_url: https://example.com/unquoted-test-article
author: "Unquoted Author"
section_names: ["ai"]
tags: ["Unquoted"]
---

This has an unquoted canonical URL.
'@
            $unquotedFile = Join-Path $script:TestOutputDir "2024-12-01-Unquoted.md"
            Set-Content -Path $unquotedFile -Value $unquotedContent -Encoding UTF8
            
            # Create a feed with a different URL for the unquoted test
            $testFeedDataUnquoted = [PSCustomObject]@{
                FeedName        = "Test Feed"
                OutputDir       = "_blogs"
                URL             = "https://example.com/feed.xml"
                FeedLevelAuthor = "Test Feed Author"
                Items           = @(
                    [PSCustomObject]@{
                        Title       = "Unquoted Test Article"
                        Link        = "https://example.com/unquoted-test-article"
                        PubDate     = "2025-01-01T12:00:00Z"
                        Description = "This is a test article for unquoted test. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " * 5) # Make longer than 500 chars to avoid HTTP fetch
                        Author      = "Test Author"
                        Tags        = @("AI", "Testing")
                        OutputDir   = "_blogs"
                    }
                )
            }
            $testFeedJsonPathUnquoted = Join-Path $script:TestScriptsPath "test-feed-unquoted.json"
            $testFeedDataUnquoted | ConvertTo-Json -Depth 10 | Set-Content -Path $testFeedJsonPathUnquoted
            # Feed object creation removed - using direct items array
            
            # Run conversion again with unquoted feed
            Convert-RssToMarkdown -Items $unquotedTestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Unquoted file should be removed
            Test-Path $unquotedFile | Should -Be $false
        }
    }
    
    Context "Mocked File I/O for File Removal Logic" {
        It "Should handle when Get-ChildItem returns no files" {
            # Mock Get-ChildItem to return empty array (simulating ErrorAction SilentlyContinue behavior)
            Mock Get-ChildItem {
                return @()
            } -ParameterFilter { $Path -eq $script:TestOutputDir -and $Filter -eq "*.md" }
            
            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Test Article"
                    description = "Test description"
                    content     = "Test content"
                    excerpt     = "Test excerpt"
                    categories  = @("AI")
                    tags        = @("Test")
                }
            }
            
            # Mock network calls
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"Test Article","description":"Test description","content":"Test content","excerpt":"Test excerpt","categories":["AI"],"tags":["Test"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Should not throw an exception and should process normally
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 1 (new file created)
            ($result | Measure-Object).Count | Should -Be 1
        }
        
        It "Should continue processing when file content reading fails" {
            # Create a real file that we can make unreadable
            $testFile = Join-Path $script:TestOutputDir "unreadable-file.md"
            Set-Content -Path $testFile -Value "dummy content"
            
            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Test Article"
                    description = "Test description"
                    content     = "Test content"
                    excerpt     = "Test excerpt"
                    categories  = @("AI")
                    tags        = @("Test")
                }
            }
            
            # Mock network calls
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"Test Article","description":"Test description","content":"Test content","excerpt":"Test excerpt","categories":["AI"],"tags":["Test"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # The function should continue processing even when it encounters files it can't read
            { Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions" } | Should -Not -Throw
            
            # Clean up
            if (Test-Path $testFile) { Remove-Item $testFile -Force }
        }
        
        It "Should handle error conditions in file checking loop gracefully" {
            # Create a file with invalid content that will cause regex issues
            $invalidFile = Join-Path $script:TestOutputDir "invalid-frontmatter.md"
            $invalidContent = @'
---
title: "File with Tricky Content"
canonical_url
malformed content here that might cause regex issues
---
Content
'@
            Set-Content -Path $invalidFile -Value $invalidContent -Encoding UTF8
            
            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Test Article"
                    description = "Test description"
                    content     = "Test content"
                    excerpt     = "Test excerpt"
                    categories  = @("AI")
                    tags        = @("Test")
                }
            }
            
            # Mock network calls
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"Test Article","description":"Test description","content":"Test content","excerpt":"Test excerpt","categories":["AI"],"tags":["Test"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Should not throw an exception even with invalid file content
            { Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions" } | Should -Not -Throw
            
            # Clean up
            if (Test-Path $invalidFile) { Remove-Item $invalidFile -Force }
        }
        
        It "Should use ShouldProcess correctly for file removal" {
            # Create a real file with matching canonical URL
            $existingContent = @'
---
title: "File To Remove"
canonical_url: "https://example.com/test-article"
---
Content
'@
            $existingFile = Join-Path $script:TestOutputDir "existing-file.md"
            Set-Content -Path $existingFile -Value $existingContent -Encoding UTF8
            
            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Test Article"
                    description = "Test description"
                    content     = "Test content"
                    excerpt     = "Test excerpt"
                    categories  = @("AI")
                    tags        = @("Test")
                }
            }
            
            # Mock network calls
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"Test Article","description":"Test description","content":"Test content","excerpt":"Test excerpt","categories":["AI"],"tags":["Test"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Run with -WhatIf to test ShouldProcess behavior
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions" -WhatIf
            
            # Should return 1 (simulated file creation)
            ($result | Measure-Object).Count | Should -Be 1
            
            # Original file should still exist (not actually removed due to -WhatIf)
            Test-Path $existingFile | Should -Be $true
            
            # No new markdown file should be created (due to -WhatIf)
            $newFiles = Get-ChildItem -Path $script:TestOutputDir -Filter "*.md"
            $newFiles | Should -HaveCount 1  # Only the original file
            $newFiles[0].Name | Should -Be "existing-file.md"
            
            # Clean up
            Remove-Item $existingFile -Force
        }
    }

    Context "Content Fetching for Short Articles" {
        It "Should process articles with short descriptions using provided EnhancedContent" {
            # Create test items with a short article (< 500 chars)
            $shortTestItems = @(
                [PSCustomObject]@{
                    Title           = "Short Article"
                    Link            = "https://example.com/short-article"
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "This is a short description." # Only 30 chars - but should work fine
                    Author          = "Test Author"
                    Tags            = @("AI", "Testing")
                    OutputDir       = Join-Path $script:TempPath "_blogs"
                    FeedName        = "Test Feed"
                    FeedUrl         = "https://example.com/feed.xml"
                    EnhancedContent = "Test enhanced content for processing"
                }
            )

            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Short Article"
                    description = "This is a short article with enhanced content"
                    content     = "# Short Article\n\nThis is test content from AI processing."
                    excerpt     = "Test excerpt for short article"
                    categories  = @("AI", "Testing")
                    tags        = @("GitHub", "PowerShell")
                } 
            }

            # Run the conversion
            $result = Convert-RssToMarkdown -Items $shortTestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"

            # Should return 1 new file
            $result | Should -Be 1

            # Verify markdown file was created
            $markdownFiles = Get-ChildItem -Path $script:TestOutputDir -Filter "*.md"
            $markdownFiles | Should -HaveCount 1

            # Check that processed entries was updated
            $processedEntries = Get-Content $script:TestProcessedEntriesPath | ConvertFrom-Json
            $processedEntries | Should -HaveCount 1
            $processedEntries[0].canonical_url | Should -Be "https://example.com/short-article"
        }

        It "Should not fetch content for YouTube feeds even if description is short" {
            # Create YouTube test items with a short article
            $youTubeTestItems = @(
                [PSCustomObject]@{
                    Title           = "Short YouTube Video"
                    Link            = "https://www.youtube.com/watch?v=test123"
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "Short video description." # Short but should not trigger fetch for YouTube
                    Author          = "Video Author"
                    Tags            = @("AI", "Video")
                    OutputDir       = Join-Path $script:TempPath "_videos"
                    FeedName        = "YouTube Test Feed"
                    FeedUrl         = "https://www.youtube.com/feeds/videos.xml?channel_id=UCtest"
                    EnhancedContent = "Test enhanced content for processing"
                }
            )

            # Mock that should NOT be called for YouTube feeds
            Mock Get-ContentFromUrl {
                throw "Get-ContentFromUrl should not be called for YouTube feeds"
            }

            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Short YouTube Video"
                    description = "This is a short YouTube video"
                    content     = "# Short YouTube Video\n\nThis is test content."
                    excerpt     = "Test excerpt for YouTube video"
                    categories  = @("AI", "GitHub Copilot")
                    tags        = @("YouTube", "Video")
                } 
            }

            # Run the conversion
            $result = Convert-RssToMarkdown -Items $youTubeTestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"

            # Should return 1 new file
            ($result | Measure-Object).Count | Should -Be 1

            # Verify Get-ContentFromUrl was NOT called
            Should -Invoke Get-ContentFromUrl -Exactly 0
        }

        It "Should process Reddit posts with content and handle cross-post detection" {
            # Create a feed with a Reddit article
            $redditTestItems = @(
                [PSCustomObject]@{
                    Title           = "Reddit Post About AI"
                    Link            = "https://www.reddit.com/r/test/comments/123456/reddit_post_about_ai"
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "Reddit discussion about AI tools and techniques."
                    Author          = "Reddit User"
                    Tags            = @("AI", "Community")
                    OutputDir       = Join-Path $script:TempPath "_community"
                    FeedName        = "Test Feed"
                    FeedUrl         = "https://example.com/feed.xml"
                    EnhancedContent = "This is a comprehensive Reddit discussion about AI tools and techniques that provides substantial value to readers. The post discusses various aspects of artificial intelligence, including natural language processing, computer vision, and deep learning algorithms. It covers practical applications in enterprise environments and provides detailed examples of implementation strategies. The content explores best practices for AI development, common pitfalls to avoid, and recommendations for getting started with AI projects. Additionally, it discusses the importance of data quality, model training techniques, and deployment considerations for production AI systems. The post also covers ethical considerations in AI development and the importance of responsible AI practices in modern software development. This content is long enough to meet the minimum character requirements for community content processing and provides meaningful insights to the developer community. Furthermore, it includes detailed discussions about emerging AI trends, best practices for machine learning model evaluation, and comprehensive strategies for implementing AI solutions in real-world scenarios with proper testing and validation methodologies."
                }
            )

            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Reddit Post About AI"
                    description = "Discussion about AI tools and techniques from Reddit"
                    content     = "# Reddit Post About AI\n\nThis is content from Reddit processing."
                    excerpt     = "Test excerpt for Reddit post"
                    categories  = @("AI")
                    tags        = @("AI", "Community", "Discussion")
                } 
            }

            # Run the conversion
            $result = Convert-RssToMarkdown -Items $redditTestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"

            # Should return 1 new file
            $result | Should -Be 1

            # Verify AI processing was called for Reddit post
            Should -Invoke Invoke-ProcessWithAiModel -Exactly 1

            # Verify markdown file was created
            $markdownFiles = Get-ChildItem -Path $script:TestCommunityOutputDir -Filter "*.md"
            $markdownFiles | Should -HaveCount 1

            # Check that processed entries was updated
            $processedEntries = Get-Content $script:TestProcessedEntriesPath | ConvertFrom-Json
            $processedEntries | Should -HaveCount 1
            $processedEntries[0].canonical_url | Should -Be "https://www.reddit.com/r/test/comments/123456/reddit_post_about_ai"
        }

        It "Should handle errors during AI processing gracefully" {
            # Create test items with valid data
            $errorTestItems = @(
                [PSCustomObject]@{
                    Title           = "Article With AI Error"
                    Link            = "https://example.com/error-article"
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "This is an article that will cause AI processing to fail."
                    Author          = "Test Author"
                    Tags            = @("AI", "Testing")
                    OutputDir       = Join-Path $script:TempPath "_blogs"
                    FeedName        = "Test Feed"
                    FeedUrl         = "https://example.com/feed.xml"
                    EnhancedContent = "Test enhanced content for error processing"
                }
            )

            # Mock AI processing to throw an error
            Mock Invoke-ProcessWithAiModel {
                throw "AI service error: Unable to process content"
            }

            # Run the conversion - should throw when AI processing fails
            { Convert-RssToMarkdown -Items $errorTestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions" } | Should -Throw "*AI service error: Unable to process content*"

            # Verify AI processing was attempted
            Should -Invoke Invoke-ProcessWithAiModel -Exactly 1

            # Verify no markdown files were created due to error
            $markdownFiles = Get-ChildItem -Path $script:TestOutputDir -Filter "*.md"
            $markdownFiles | Should -HaveCount 0

            # Check that processed entries was NOT updated (error handling)
            if (Test-Path $script:TestProcessedEntriesPath) {
                $processedEntries = Get-Content $script:TestProcessedEntriesPath | ConvertFrom-Json
                $processedEntries | Should -HaveCount 0
            }
        }
    }

    Context "Successful Processing" {
        It "Should create markdown files when API calls succeed" {
            # Mock network calls to simulate production behavior
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    # Simulate successful AI response
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"Test Article","description":"This is a test article","content":"# Test Article\\n\\nThis is test content.","excerpt":"Test excerpt","categories":["AI","Testing"],"tags":["GitHub","PowerShell"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Mock successful API response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Test Article"
                    description = "This is a test article"
                    content     = "# Test Article\n\nThis is test content."
                    excerpt     = "Test excerpt"
                    categories  = @("AI", "Testing")
                    tags        = @("GitHub", "PowerShell")
                } 
            }
            
            # Run the conversion
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -Endpoint "https://test.azure.com/openai/chat/completions"
            
            # Should return 1 new file
            ($result | Measure-Object).Count | Should -Be 1
            
            # Should not add anything to skipped entries
            $skippedEntries = Get-Content $script:TestSkippedEntriesPath | ConvertFrom-Json
            $skippedEntries | Should -HaveCount 0
        }
    }
}
