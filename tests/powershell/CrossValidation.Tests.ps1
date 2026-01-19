# CrossValidation.Tests.ps1
# Validates that PowerShell ConvertTo-YamlFrontMatter produces identical output to .NET ContentFixer

Describe "PowerShell and .NET ContentFixer Cross-Validation" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
        
        # Create a temporary test file with old-style frontmatter
        $script:testDir = Join-Path $TestDrive "cross-validation"
        New-Item -ItemType Directory -Path $script:testDir -Force | Out-Null
        
        $script:testFile = Join-Path $script:testDir "test-article.md"
        
        # Old-style content (what RSS pipeline produces before ContentFixer)
        $oldContent = @"
---
title: "GitHub Copilot: Advanced Features and Best Practices"
description: "A comprehensive guide to GitHub Copilot's advanced features"
author: "Jane Doe"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://example.com/copilot-guide"
viewing_mode: "external"
feed_name: "Tech Blog"
feed_url: "https://example.com/feed"
date: 2026-01-16 10:00:00 +00:00
permalink: "/2026-01-16-GitHub-Copilot-Advanced-Features.html"
categories: ["AI", "GitHub Copilot", "Coding"]
tags: ["AI", "GitHub Copilot", "Best Practices", "Coding"]
tags_normalized: ["ai", "github copilot", "best practices", "coding"]
---

This is a comprehensive guide to GitHub Copilot's advanced features.<!--excerpt_end-->

# Main Content

Here's the full article content.
"@
        
        Set-Content -Path $script:testFile -Value $oldContent -Encoding UTF8
    }

    Context "ContentFixer Output Format" {
        It "Should run ContentFixer on test file successfully" {
            $contentFixerProject = "/workspaces/techhub/src/TechHub.ContentFixer/TechHub.ContentFixer.csproj"
            
            if (-not (Test-Path $contentFixerProject)) {
                Set-ItResult -Skipped -Because "ContentFixer project not found"
                return
            }
            
            # Run ContentFixer
            $result = dotnet run --project $contentFixerProject -- --file $script:testFile 2>&1
            
            $LASTEXITCODE | Should -Be 0 -Because "ContentFixer should run successfully"
        }
        
        It "Should produce YAML frontmatter with expected format" {
            $fixedContent = Get-Content -Path $script:testFile -Raw
            
            # Should have YAML delimiters
            $fixedContent | Should -Match "^---\n"
            
            # Should have section_names instead of categories
            $fixedContent | Should -Match "section_names:"
            $fixedContent | Should -Not -Match "categories:"
            
            # Should not have removed fields
            $fixedContent | Should -Not -Match "tags_normalized:"
            $fixedContent | Should -Not -Match "excerpt_separator:"
            $fixedContent | Should -Not -Match "description:"
            $fixedContent | Should -Not -Match "permalink:"
            $fixedContent | Should -Not -Match "canonical_url:"
            $fixedContent | Should -Not -Match "feed_url:"
            $fixedContent | Should -Match "feed_name:"
            $fixedContent | Should -Not -Match "alt_collection:"
            
            # Should have new required fields
            $fixedContent | Should -Match "external_url:"
            $fixedContent | Should -Not -Match "collection:"
        }
    }

    Context "PowerShell ConvertTo-YamlFrontMatter Matching" {
        It "Should produce identical frontmatter structure to ContentFixer" {
            # Get ContentFixer output
            $fixedContent = Get-Content -Path $script:testFile -Raw
            
            # Extract frontmatter from ContentFixer output
            if ($fixedContent -match '(?s)^---\n(.+?)\n---') {
                $contentFixerYaml = $matches[1]
            }
            else {
                throw "Could not extract frontmatter from ContentFixer output"
            }
            
            # Create equivalent frontmatter using PowerShell function
            $frontMatter = @{
                title         = "GitHub Copilot: Advanced Features and Best Practices"
                author        = "Jane Doe"
                external_url  = "https://example.com/copilot-guide"
                viewing_mode  = "external"
                feed_name     = "Tech Blog"
                date          = "2026-01-16 10:00:00 +00:00"
                tags          = @("AI", "GitHub Copilot", "Best Practices", "Coding")
                section_names = @("ai", "github-copilot", "coding")
            }
            
            $powershellYaml = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            # Extract just the YAML part (without delimiters) for comparison
            if ($powershellYaml -match '(?s)^---\n(.+?)\n---') {
                $powershellYamlContent = $matches[1]
            }
            else {
                throw "Could not extract frontmatter from PowerShell output"
            }
            
            # Both should have the same fields
            $contentFixerLines = $contentFixerYaml -split "\n" | Where-Object { $_ -match "^\w+:" }
            $powershellLines = $powershellYamlContent -split "\n" | Where-Object { $_ -match "^\w+:" }
            
            $contentFixerFields = $contentFixerLines | ForEach-Object { ($_ -split ":")[0].Trim() } | Sort-Object
            $powershellFields = $powershellLines | ForEach-Object { ($_ -split ":")[0].Trim() } | Sort-Object
            
            # Should have same field names
            $powershellFields | Should -Be $contentFixerFields
        }
        
        It "Should use same quoting rules as ContentFixer" {
            # Test various quoting scenarios
            $testCases = @(
                @{
                    Field           = "title"
                    Value           = "Test: Article"
                    ExpectedPattern = "title: 'Test: Article'"  # Quotes for colons
                },
                @{
                    Field           = "author"
                    Value           = "John Doe"
                    ExpectedPattern = "author: John Doe"  # No quotes for simple names
                },
                @{
                    Field           = "section_names"
                    Value           = @("ai", "github_copilot")
                    ExpectedPattern = "section_names:"  # Array field
                }
            )
            
            foreach ($testCase in $testCases) {
                $frontMatter = @{
                    $testCase.Field = $testCase.Value
                }
                
                $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
                
                $result | Should -Match $testCase.ExpectedPattern -Because "Field '$($testCase.Field)' with value '$($testCase.Value)' should match pattern '$($testCase.ExpectedPattern)'"
            }
        }
    }

    Context "End-to-End Validation" {
        It "Should produce files that don't need ContentFixer when using PowerShell pipeline" {
            # Create a new file using PowerShell function
            $frontMatter = @{
                layout        = "post"
                title         = "New Article from RSS"
                author        = "RSS Author"
                external_url  = "https://example.com/new-article"
                viewing_mode  = "external"
                tags          = @("AI", "Machine Learning")
                section_names = @("ai")
            }
            
            $yamlFrontMatter = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            $content = "Article excerpt here.<!--excerpt_end-->`n`n# Article Content"
            
            $newFile = Join-Path $script:testDir "powershell-generated.md"
            Set-Content -Path $newFile -Value "$yamlFrontMatter$content" -Encoding UTF8
            
            # Run ContentFixer on it
            $contentFixerProject = "/workspaces/techhub/src/TechHub.ContentFixer/TechHub.ContentFixer.csproj"
            $result = dotnet run --project $contentFixerProject -- --file $newFile 2>&1 | Out-String
            
            # Should report "Skipped" or "already fixed" - not "Processed"
            $result | Should -Match "Skipped|already fixed|Already has section_names"
        }
    }
}
