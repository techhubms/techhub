# IMPORTANT: These tests intentionally do NOT use -WhatIf parameter because they must verify
# actual file operations (creating markdown files, writing to tracking files). The tests are
# safe because they use isolated test directories and proper BeforeEach/AfterAll cleanup.

Describe "Convert-RssToMarkdown" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"

        $script:TestScriptsPath = Join-Path $script:TempPath ".github/scripts"
        $script:TestOutputDir = Join-Path $script:TempPath "_posts"
        $script:TestCommunityOutputDir = Join-Path $script:TempPath "_community"
        $script:TestAiResultsDir = Join-Path $script:TestScriptsPath "ai-results"
        $script:TestTemplatesPath = Join-Path $script:TestScriptsPath "templates"
        $script:TestSkippedEntriesPath = Join-Path $script:TestScriptsPath "skipped-entries.json"
        $script:TestProcessedEntriesPath = Join-Path $script:TestScriptsPath "processed-entries.json"
        
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
        
        $script:postsTestItems = @(
            [PSCustomObject]@{
                Title           = "Posts Article"
                Link            = "https://example.com/posts-article"
                PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                Description     = "This is a posts article. " + ("Lorem ipsum dolor sit amet, consectetur adipiscing elit. " * 10)
                Author          = "Posts Author"
                Tags            = @("Posts", "Testing")
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
---
title: {{TITLE}}
date: {{DATE}}
categories: {{CATEGORIES}}
tags: {{TAGS}}
tags_normalized: {{TAGS_NORMALIZED}}
author: {{AUTHOR}}
description: {{DESCRIPTION}}
canonical_url: {{CANONICAL_URL}}
---

{{CONTENT}}
'@
        Set-Content -Path (Join-Path $script:TemplateSourcePath "template-generic.md") -Value $genericTemplate
        
        $videoTemplate = @'
---
title: {{TITLE}}
date: {{DATE}}
categories: {{CATEGORIES}}
tags: {{TAGS}}
tags_normalized: {{TAGS_NORMALIZED}}
author: {{AUTHOR}}
description: {{DESCRIPTION}}
canonical_url: {{CANONICAL_URL}}
youtube_id: {{YOUTUBE_ID}}
---

{{CONTENT}}
'@
        Set-Content -Path (Join-Path $script:TemplateSourcePath "template-videos.md") -Value $videoTemplate

        # Recreate directory structure (Initialize-BeforeEach.ps1 removes TestSourceRoot)
        New-Item -Path $script:TempPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestScriptsPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestOutputDir -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestAiResultsDir -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestTemplatesPath -ItemType Directory -Force | Out-Null
        
        # Copy template files from BeforeAll setup
        Copy-Item -Path (Join-Path $script:TemplateSourcePath "*") -Destination $script:TestTemplatesPath -Force
        
        # Reset tracking files for test isolation
        Set-Content -Path $script:TestSkippedEntriesPath -Value "[]"
        Set-Content -Path $script:TestProcessedEntriesPath -Value "[]"
        
        # Create collection directories and clear any existing files
        $collectionDirs = @("_posts", "_community", "_news", "_videos", "_events", "_magazines", "_roundups")
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
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
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
            $result = Convert-RssToMarkdown -Items $script:testItems -Token "test-token" -Model "test-model"
            
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
            { Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" } |
            Should -Throw "*Unknown error type: APIError*"
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
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
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
            
            # Mock Save-GitHubModelsResult to verify it's called for debugging
            Mock Save-GitHubModelsResult {}
            
            # Run the conversion (NOTE: -WhatIf is intentionally NOT used here because this test must 
            # verify that entries are actually written to the skipped entries file.)
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
            # Should return 0 new files (since JSON parsing failed)
            $result | Should -Be 0
            
            # Check that the item was added to skipped entries
            $skippedEntries = Get-Content $script:TestSkippedEntriesPath | ConvertFrom-Json
            $skippedEntries | Should -HaveCount 1
            $skippedEntries[0].canonical_url | Should -Be "https://example.com/test-article"
            $skippedEntries[0].reason | Should -Be "AI model response could not be parsed as JSON"
            
            # Verify that Save-GitHubModelsResult was called for debugging purposes
            Should -Invoke Save-GitHubModelsResult -Times 1 -ParameterFilter {
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
categories: [AI]
tags: [Old, Tags]
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
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
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
categories: [AI]
tags: [Test]
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
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
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
categories: [AI]
tags: [Old, Tags]
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
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
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
categories: [AI]
tags: [Different]
---

This is a different article.
'@
            $existingContent2 = @'
---
title: "Target Article"
date: 2024-12-01 11:00:00 +00:00
canonical_url: "https://example.com/test-article"
author: "Target Author"
categories: [AI]
tags: [Target]
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
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
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
            { Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" } | Should -Not -Throw
            
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
categories: [AI]
tags: [Quoted]
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
            Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
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
categories: [AI]
tags: [Unquoted]
---

This has an unquoted canonical URL.
'@
            $unquotedFile = Join-Path $script:TestOutputDir "2024-12-01-Unquoted.md"
            Set-Content -Path $unquotedFile -Value $unquotedContent -Encoding UTF8
            
            # Create a feed with a different URL for the unquoted test
            $testFeedDataUnquoted = [PSCustomObject]@{
                FeedName        = "Test Feed"
                OutputDir       = "_posts"
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
                        OutputDir   = "_posts"
                    }
                )
            }
            $testFeedJsonPathUnquoted = Join-Path $script:TestScriptsPath "test-feed-unquoted.json"
            $testFeedDataUnquoted | ConvertTo-Json -Depth 10 | Set-Content -Path $testFeedJsonPathUnquoted
            # Feed object creation removed - using direct items array
            
            # Run conversion again with unquoted feed
            Convert-RssToMarkdown -Items $unquotedTestItems -Token "test-token" -Model "test-model"
            
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
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
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
            { Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" } | Should -Not -Throw
            
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
            { Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" } | Should -Not -Throw
            
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
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model" -WhatIf
            
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
                    OutputDir       = Join-Path $script:TempPath "_posts"
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
            $result = Convert-RssToMarkdown -Items $shortTestItems -Token "test-token" -Model "test-model"

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
            $result = Convert-RssToMarkdown -Items $youTubeTestItems -Token "test-token" -Model "test-model"

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
            $result = Convert-RssToMarkdown -Items $redditTestItems -Token "test-token" -Model "test-model"

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
                    OutputDir       = Join-Path $script:TempPath "_posts"
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
            { Convert-RssToMarkdown -Items $errorTestItems -Token "test-token" -Model "test-model" } | Should -Throw "*AI service error: Unable to process content*"

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

    Context "Reddit Cross-Post Detection" {
        BeforeEach {
            # Clear any existing files
            Get-ChildItem -Path $script:TestCommunityOutputDir -Filter "*.md" -ErrorAction SilentlyContinue | Remove-Item -Force
        }
        
        It "Should detect and remove Reddit cross-posts with identical title in URL" {
            # Create existing Reddit post
            $existingContent = @'
---
title: "Original Reddit Post"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://www.reddit.com/r/programming/comments/abc123/cross_posted_article"
author: "Original Author"
categories: ["AI"]
tags: ["Discussion"]
---

Original Reddit post content.
'@
            $existingFilename = "2024-12-01-Original-Reddit-Post.md"
            $existingFilePath = Join-Path $script:TestCommunityOutputDir $existingFilename
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Create test items with cross-post (same title in URL but different subreddit and post ID)
            $communityTestItems = @(
                [PSCustomObject]@{
                    Title           = "Cross Posted Article"
                    Link            = "https://www.reddit.com/r/dotnet/comments/xyz456/cross_posted_article"  # Same title: cross_posted_article
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "This is a cross-posted article. " + ("Lorem ipsum " * 50)  # Long enough to avoid HTTP fetch
                    Author          = "Cross Post Author"
                    Tags            = @("AI", "Discussion")
                    OutputDir       = Join-Path $script:TempPath "_community"
                    FeedName        = "Community Test Feed"
                    FeedUrl         = "https://example.com/community-feed.xml"
                    EnhancedContent = "This is a comprehensive discussion about cross-posted articles and content sharing strategies across different platforms. The post discusses various aspects of content distribution, including effective cross-posting techniques, community engagement best practices, and strategies for maximizing reach without creating spam. It covers practical applications in community management and provides detailed examples of successful cross-posting implementations. The content explores best practices for content curation, common pitfalls to avoid when sharing across multiple platforms, and recommendations for maintaining authentic engagement. Additionally, it discusses the importance of adapting content for different audiences, timing considerations for cross-posting, and techniques for measuring cross-post effectiveness. The post also covers ethical considerations in content sharing and the importance of respecting community guidelines across different platforms. Furthermore, it includes detailed analysis of cross-platform engagement metrics, advanced strategies for content optimization across different communities, and comprehensive guidelines for maintaining consistent brand voice while adapting to diverse audience expectations and platform-specific requirements."
                }
            )
            
            # Mock successful AI response that would create categories
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Cross Posted Article"
                    description = "This is a cross-posted article"
                    content     = "# Cross Posted Article\n\nThis is cross-posted content."
                    excerpt     = "Cross-post excerpt"
                    categories  = @("AI")  # This will trigger the cross-post check
                    tags        = @("Discussion", "Community")
                }
            }
            
            # Verify existing file exists before test
            Test-Path $existingFilePath | Should -Be $true
            
            # Run the conversion
            $result = Convert-RssToMarkdown -Items $communityTestItems -Token "test-token" -Model "test-model"
            
            # Should return 1 new file
            $result | Should -Be 1
            
            # Original file should be removed due to cross-post detection
            Test-Path $existingFilePath | Should -Be $false
            
            # New file should be created
            $newFiles = Get-ChildItem -Path $script:TestCommunityOutputDir -Filter "*.md"
            $newFiles | Should -HaveCount 1
            $newFiles[0].Name | Should -Match "^2025-01-01-.*\.md$"
        }
        
        It "Should not remove Reddit posts with different title in URL" {
            # Create existing Reddit post with different title
            $existingContent = @'
---
title: "Different Reddit Post"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://www.reddit.com/r/programming/comments/xyz789/different_post"
author: "Different Author"
categories: ["AI"]
tags: ["Discussion"]
---

Different Reddit post content.
'@
            $existingFilename = "2024-12-01-Different-Reddit-Post.md"
            $existingFilePath = Join-Path $script:TestCommunityOutputDir $existingFilename
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Create test items for community processing
            $communityTestItems = @(
                [PSCustomObject]@{
                    Title           = "New Reddit Article"
                    Link            = "https://www.reddit.com/r/dotnet/comments/abc123/new_reddit_article"  # Different title: new_reddit_article vs different_post
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "This is a new Reddit article that provides comprehensive coverage of development topics."
                    Author          = "New Author"
                    Tags            = @("AI", "Discussion")
                    OutputDir       = Join-Path $script:TempPath "_community"
                    FeedName        = "Community Test Feed"
                    FeedUrl         = "https://example.com/community-feed.xml"
                    EnhancedContent = "This is a comprehensive discussion about new Reddit articles and their impact on the developer community. The post discusses various aspects of community engagement, including effective communication strategies, best practices for technical discussions, and methods for fostering inclusive conversations. It covers practical applications in online community management and provides detailed examples of successful community building initiatives. The content explores best practices for content creation, common pitfalls to avoid when engaging with technical communities, and recommendations for maintaining productive discourse. Additionally, it discusses the importance of knowledge sharing, mentorship opportunities within developer communities, and techniques for encouraging participation from diverse backgrounds. The post also covers ethical considerations in community management and the importance of maintaining respectful environments for technical learning and growth. Furthermore, it includes detailed analysis of community metrics, advanced strategies for content moderation, and comprehensive guidelines for building sustainable online developer communities."
                }
            )
            
            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "New Reddit Article"
                    description = "This is a new Reddit article"
                    content     = "# New Reddit Article\n\nThis is new content."
                    excerpt     = "New excerpt"
                    categories  = @("AI", "GitHub Copilot")
                    tags        = @("Discussion", "Community")
                }
            }
            
            # Mock network calls
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"New Reddit Article","description":"This is a new Reddit article","content":"# New Reddit Article\\n\\nThis is new content.","excerpt":"New excerpt","categories":["AI","GitHub Copilot"],"tags":["Discussion","Community"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Verify existing file exists before test
            Test-Path $existingFilePath | Should -Be $true
            
            # Run the conversion
            $result = Convert-RssToMarkdown -Items $communityTestItems -Token "test-token" -Model "test-model"
            
            # Should return 1 new file
            ($result | Measure-Object).Count | Should -Be 1
            
            # Original file should NOT be removed (different URL endings)
            Test-Path $existingFilePath | Should -Be $true
            
            # Both files should exist
            $allFiles = Get-ChildItem -Path $script:TestCommunityOutputDir -Filter "*.md"
            $allFiles | Should -HaveCount 2
        }
        
        It "Should only perform cross-post detection for community collection" {
            # Create existing Reddit post in regular posts directory (not community)
            $existingContent = @'
---
title: "Regular Post"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://www.reddit.com/r/programming/comments/abc123/regular_post"
author: "Regular Author"
categories: ["AI"]
tags: ["Discussion"]
---

Regular post content.
'@
            $existingFilename = "2024-12-01-Regular-Post.md"
            $existingFilePath = Join-Path $script:TestOutputDir $existingFilename  # Note: TestOutputDir is _posts, not _community
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Create test feed with same URL ending but different collection (posts, not community)
            $testFeedDataPosts = [PSCustomObject]@{
                FeedName        = "Posts Test Feed"
                OutputDir       = "_posts"
                URL             = "https://example.com/posts-feed.xml"
                FeedLevelAuthor = "Posts Feed Author"
                Items           = @(
                    [PSCustomObject]@{
                        Title       = "New Post With Same Ending"
                        Link        = "https://www.reddit.com/r/dotnet/comments/abc123/new_post_same_ending"  # Same ending: abc123
                        PubDate     = "2025-01-01T12:00:00Z"
                        Description = "This is a new post with same URL ending. " + ("Lorem ipsum " * 50)
                        Author      = "New Author"
                        Tags        = @("AI", "Discussion")
                        OutputDir   = "_posts"
                    }
                )
            }
            $testPostsFeedJsonPath = Join-Path $script:TestScriptsPath "test-posts-feed.json"
            $testFeedDataPosts | ConvertTo-Json -Depth 10 | Set-Content -Path $testPostsFeedJsonPath
            # Feed object creation removed - using direct items array
            
            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "New Post With Same Ending"
                    description = "This is a new post with same URL ending"
                    content     = "# New Post\n\nThis is new content."
                    excerpt     = "New excerpt"
                    categories  = @("AI", "GitHub Copilot")
                    tags        = @("Discussion", "Blog")
                }
            }
            
            # Mock network calls
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"New Post With Same Ending","description":"This is a new post with same URL ending","content":"# New Post\\n\\nThis is new content.","excerpt":"New excerpt","categories":["AI","GitHub Copilot"],"tags":["Discussion","Blog"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Verify existing file exists before test
            Test-Path $existingFilePath | Should -Be $true
            
            # Run the conversion on posts feed (not community)
            $result = Convert-RssToMarkdown -Items $postsTestItems -Token "test-token" -Model "test-model"
            
            # Should return 1 new file
            ($result | Measure-Object).Count | Should -Be 1
            
            # Original file should NOT be removed (cross-post detection only for community collection)
            Test-Path $existingFilePath | Should -Be $true
            
            # Both files should exist
            $allFiles = Get-ChildItem -Path $script:TestOutputDir -Filter "*.md"
            $allFiles | Should -HaveCount 2
        }
        
        It "Should only check Reddit URLs for cross-post detection" {
            # Create existing non-Reddit post in community directory
            $existingContent = @'
---
title: "Non-Reddit Post"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://example.com/blog/abc123/non_reddit_post"
author: "Blog Author"
categories: ["AI"]
tags: ["Discussion"]
---

Non-Reddit post content.
'@
            $existingFilename = "2024-12-01-Non-Reddit-Post.md"
            $existingFilePath = Join-Path $script:TestCommunityOutputDir $existingFilename
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Create test items for community processing
            $communityTestItems = @(
                [PSCustomObject]@{
                    Title           = "New Non-Reddit Article"
                    Link            = "https://different-site.com/articles/abc123/new_article"  # Same ending: abc123, but not Reddit
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "This is a new non-Reddit article that provides comprehensive coverage of development topics."
                    Author          = "New Author"
                    Tags            = @("AI", "Discussion")
                    OutputDir       = Join-Path $script:TempPath "_community"
                    FeedName        = "Community Test Feed"
                    FeedUrl         = "https://example.com/community-feed.xml"
                    EnhancedContent = "This is a comprehensive discussion about non-Reddit article sharing and content distribution strategies across different platforms. The post discusses various aspects of content curation, including effective cross-platform sharing techniques, community engagement best practices, and strategies for maximizing reach across diverse audiences. It covers practical applications in content management and provides detailed examples of successful content distribution implementations. The content explores best practices for article promotion, common pitfalls to avoid when sharing content across platforms, and recommendations for maintaining authentic engagement. Additionally, it discusses the importance of adapting content for different community cultures, timing considerations for content sharing, and techniques for measuring article effectiveness across various platforms. The post also covers ethical considerations in content distribution and the importance of respecting community guidelines while maintaining content quality and relevance."
                }
            )
            
            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "New Non-Reddit Article"
                    description = "This is a new non-Reddit article"
                    content     = "# New Non-Reddit Article\n\nThis is new content."
                    excerpt     = "New excerpt"
                    categories  = @("AI", "GitHub Copilot")
                    tags        = @("Discussion", "Community")
                }
            }
            
            # Mock network calls
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"New Non-Reddit Article","description":"This is a new non-Reddit article","content":"# New Non-Reddit Article\\n\\nThis is new content.","excerpt":"New excerpt","categories":["AI","GitHub Copilot"],"tags":["Discussion","Community"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Verify existing file exists before test
            Test-Path $existingFilePath | Should -Be $true
            
            # Run the conversion
            $result = Convert-RssToMarkdown -Items $communityTestItems -Token "test-token" -Model "test-model"
            
            # Should return 1 new file
            ($result | Measure-Object).Count | Should -Be 1
            
            # Original file should NOT be removed (not Reddit URLs)
            Test-Path $existingFilePath | Should -Be $true
            
            # Both files should exist
            $allFiles = Get-ChildItem -Path $script:TestCommunityOutputDir -Filter "*.md"
            $allFiles | Should -HaveCount 2
        }
        
        It "Should handle empty URL endings gracefully" {
            # Create existing Reddit post with trailing slash (empty ending)
            $existingContent = @'
---
title: "Reddit Post With Slash"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://www.reddit.com/r/programming/comments/"
author: "Slash Author"
categories: ["AI"]
tags: ["Discussion"]
---

Reddit post with trailing slash.
'@
            $existingFilename = "2024-12-01-Reddit-Post-With-Slash.md"
            $existingFilePath = Join-Path $script:TestCommunityOutputDir $existingFilename
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Create test items for community processing
            $communityTestItems = @(
                [PSCustomObject]@{
                    Title           = "New Reddit Article With Slash"
                    Link            = "https://www.reddit.com/r/dotnet/comments/"  # Also empty ending
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "This is a new Reddit article with slash that provides comprehensive coverage of development topics."
                    Author          = "New Author"
                    Tags            = @("AI", "Discussion")
                    OutputDir       = Join-Path $script:TempPath "_community"
                    FeedName        = "Community Test Feed"
                    FeedUrl         = "https://example.com/community-feed.xml"
                    EnhancedContent = "This is a comprehensive discussion about Reddit articles with trailing slashes and URL handling in content management systems. The post discusses various aspects of URL processing, including effective URL parsing techniques, edge case handling for malformed URLs, and strategies for robust URL validation in content management workflows. It covers practical applications in web scraping and content aggregation, providing detailed examples of successful URL processing implementations. The content explores best practices for URL normalization, common pitfalls to avoid when handling URLs with special characters, and recommendations for maintaining URL consistency across different platforms. Additionally, it discusses the importance of URL sanitization, security considerations when processing external URLs, and techniques for handling various URL formats and edge cases. The post also covers performance considerations in URL processing and the importance of maintaining system reliability when dealing with diverse URL structures and formatting variations."
                }
            )
            
            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "New Reddit Article With Slash"
                    description = "This is a new Reddit article with slash"
                    content     = "# New Reddit Article\n\nThis is new content."
                    excerpt     = "New excerpt"
                    categories  = @("AI", "GitHub Copilot")
                    tags        = @("Discussion", "Community")
                }
            }
            
            # Mock network calls
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"New Reddit Article With Slash","description":"This is a new Reddit article with slash","content":"# New Reddit Article\\n\\nThis is new content.","excerpt":"New excerpt","categories":["AI","GitHub Copilot"],"tags":["Discussion","Community"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Verify existing file exists before test
            Test-Path $existingFilePath | Should -Be $true
            
            # Run the conversion
            $result = Convert-RssToMarkdown -Items $communityTestItems -Token "test-token" -Model "test-model"
            
            # Should return 1 new file
            ($result | Measure-Object).Count | Should -Be 1
            
            # Original file should NOT be removed (empty endings should not match)
            Test-Path $existingFilePath | Should -Be $true
            
            # Both files should exist
            $allFiles = Get-ChildItem -Path $script:TestCommunityOutputDir -Filter "*.md"
            $allFiles | Should -HaveCount 2
        }
        
        It "Should only trigger cross-post detection after successful AI categorization" {
            # Create existing Reddit post
            $existingContent = @'
---
title: "Existing Reddit Post"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "https://www.reddit.com/r/programming/comments/abc123/existing_post"
author: "Existing Author"
categories: ["AI"]
tags: ["Discussion"]
---

Existing Reddit post content.
'@
            $existingFilename = "2024-12-01-Existing-Reddit-Post.md"
            $existingFilePath = Join-Path $script:TestCommunityOutputDir $existingFilename
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Create test items with cross-post
            $communityTestItems = @(
                [PSCustomObject]@{
                    Title           = "Cross Posted Article"
                    Link            = "https://www.reddit.com/r/dotnet/comments/abc123/cross_posted_article"  # Same ending: abc123
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "This is a cross-posted article. " + ("Lorem ipsum " * 50)
                    Author          = "Cross Post Author"
                    Tags            = @("AI", "Discussion")
                    OutputDir       = Join-Path $script:TempPath "_community"
                    FeedName        = "Community Test Feed"
                    FeedUrl         = "https://example.com/community-feed.xml"
                    EnhancedContent = "Test enhanced content for skipped cross-post processing"
                }
            )
            
            # Mock AI response that returns NO categories (will skip item)
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Cross Posted Article"
                    description = "This is a cross-posted article"
                    content     = "# Cross Posted Article\n\nThis is cross-posted content."
                    excerpt     = "Cross-post excerpt"
                    categories  = @()  # Empty categories will cause item to be skipped
                    tags        = @("Discussion", "Community")
                }
            }
            
            # Verify existing file exists before test
            Test-Path $existingFilePath | Should -Be $true
            
            # Run the conversion
            $result = Convert-RssToMarkdown -Items $communityTestItems -Token "test-token" -Model "test-model"
            
            # Should return 0 new files (item skipped due to insufficient content length)
            $result | Should -Be 0
            
            # Original file should NOT be removed (cross-post detection never ran because item was skipped)
            Test-Path $existingFilePath | Should -Be $true
            
            # Only original file should exist
            $allFiles = Get-ChildItem -Path $script:TestCommunityOutputDir -Filter "*.md"
            $allFiles | Should -HaveCount 1
            
            # Item should be added to skipped entries
            $skippedEntries = Get-Content $script:TestSkippedEntriesPath | ConvertFrom-Json
            $skippedEntries | Should -HaveCount 1
            $skippedEntries[0].canonical_url | Should -Be "https://www.reddit.com/r/dotnet/comments/abc123/cross_posted_article"
            $skippedEntries[0].reason | Should -Be "Insufficient content length"
        }
        
        It "Should handle malformed canonical URLs gracefully" {
            # Create existing file with malformed canonical URL
            $existingContent = @'
---
title: "Malformed URL Post"
date: 2024-12-01 10:00:00 +00:00
canonical_url: "not-a-valid-url-format"
author: "Test Author"
categories: ["AI"]
tags: ["Discussion"]
---

Post with malformed URL.
'@
            $existingFilename = "2024-12-01-Malformed-URL-Post.md"
            $existingFilePath = Join-Path $script:TestCommunityOutputDir $existingFilename
            Set-Content -Path $existingFilePath -Value $existingContent -Encoding UTF8
            
            # Create test items for community processing
            $communityTestItems = @(
                [PSCustomObject]@{
                    Title           = "Valid Reddit Article"
                    Link            = "https://www.reddit.com/r/dotnet/comments/abc123/valid_reddit_article"
                    PubDate         = [DateTime]::Parse("2025-01-01T12:00:00Z")
                    Description     = "This is a valid Reddit article that provides comprehensive coverage of development topics."
                    Author          = "Valid Author"
                    Tags            = @("AI", "Discussion")
                    OutputDir       = Join-Path $script:TempPath "_community"
                    FeedName        = "Community Test Feed"
                    FeedUrl         = "https://example.com/community-feed.xml"
                    EnhancedContent = "This is a comprehensive discussion about handling malformed URLs in content management systems and their impact on cross-post detection algorithms. The post discusses various aspects of URL validation, including effective error handling techniques, graceful degradation strategies when processing malformed URLs, and methods for maintaining system stability during URL parsing operations. It covers practical applications in web content processing and provides detailed examples of robust URL handling implementations. The content explores best practices for URL validation, common pitfalls to avoid when processing URLs with various formats, and recommendations for maintaining content processing workflows even with invalid input. Additionally, it discusses the importance of error logging, fallback mechanisms for URL processing failures, and techniques for maintaining data integrity when encountering malformed URLs. The post also covers performance considerations in URL validation and the importance of maintaining system reliability when dealing with diverse and potentially invalid URL formats in content feeds."
                }
            )
            
            # Mock successful AI response
            Mock Invoke-ProcessWithAiModel {
                return [PSCustomObject]@{
                    title       = "Valid Reddit Article"
                    description = "This is a valid Reddit article"
                    content     = "# Valid Reddit Article\n\nThis is valid content."
                    excerpt     = "Valid excerpt"
                    categories  = @("AI", "GitHub Copilot")
                    tags        = @("Discussion", "Community")
                }
            }
            
            # Mock network calls
            Mock Invoke-RestMethod {
                if ($Uri -like "*api.openai.com*" -or $Uri -like "*api.azure.com*") {
                    return [PSCustomObject]@{
                        choices = @(
                            [PSCustomObject]@{
                                message = [PSCustomObject]@{
                                    content = '{"title":"Valid Reddit Article","description":"This is a valid Reddit article","content":"# Valid Reddit Article\\n\\nThis is valid content.","excerpt":"Valid excerpt","categories":["AI","GitHub Copilot"],"tags":["Discussion","Community"]}'
                                }
                            }
                        )
                    }
                }
                throw "Unexpected REST call to: $Uri"
            }
            
            # Verify existing file exists before test
            Test-Path $existingFilePath | Should -Be $true
            
            # Run the conversion - should not crash on malformed URL
            $result = Convert-RssToMarkdown -Items $communityTestItems -Token "test-token" -Model "test-model"
            
            # Should return 1 new file
            ($result | Measure-Object).Count | Should -Be 1
            
            # Original file should NOT be removed (malformed URL doesn't match anything)
            Test-Path $existingFilePath | Should -Be $true
            
            # Both files should exist
            $allFiles = Get-ChildItem -Path $script:TestCommunityOutputDir -Filter "*.md"
            $allFiles | Should -HaveCount 2
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
            $result = Convert-RssToMarkdown -Items $script:TestItems -Token "test-token" -Model "test-model"
            
            # Should return 1 new file
            ($result | Measure-Object).Count | Should -Be 1
            
            # Should not add anything to skipped entries
            $skippedEntries = Get-Content $script:TestSkippedEntriesPath | ConvertFrom-Json
            $skippedEntries | Should -HaveCount 0
        }
    }
}
