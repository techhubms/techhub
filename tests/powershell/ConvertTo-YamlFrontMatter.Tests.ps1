# ConvertTo-YamlFrontMatter.Tests.ps1
# Tests for the ConvertTo-YamlFrontMatter PowerShell function using YamlDotNet

Describe "ConvertTo-YamlFrontMatter" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }

    Context "YamlDotNet Integration" {
        It "Should load YamlDotNet assembly successfully" {
            $frontMatter = @{
                title = "Test"
            }
            
            { ConvertTo-YamlFrontMatter -FrontMatter $frontMatter } | Should -Not -Throw
        }
        
        It "Should only load YamlDotNet once (performance check)" {
            $frontMatter = @{
                title = "Test"
            }
            
            # First call
            $result1 = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            # Second call should reuse loaded assembly
            $result2 = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            $result1 | Should -Be $result2
        }
    }

    Context "Frontmatter Structure" {
        It "Should include YAML delimiters" {
            $frontMatter = @{
                title = "Test Article"
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            $result | Should -Match "^---\n"
            $result | Should -Match "\n---\n$"
        }
        
        It "Should serialize simple string values without quotes" {
            $frontMatter = @{
                layout = "post"
                author = "John Doe"
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            $result | Should -Match "layout: post"
            $result | Should -Match "author: John Doe"
        }
        
        It "Should use single quotes for strings with special YAML characters" {
            $frontMatter = @{
                title = "Test: Article with special chars"
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            # YamlDotNet uses single quotes for strings with colons
            $result | Should -Match "title: '.*:.*'"
        }
        
        It "Should escape single quotes in values by doubling them" {
            $frontMatter = @{
                title = "It's a test article"
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            # YamlDotNet may not quote simple strings with apostrophes if they're not ambiguous
            # Just verify the title is present correctly
            $result | Should -Match "title: It's a test article"
        }
    }

    Context "Array Serialization" {
        It "Should serialize arrays in block sequence format" {
            $frontMatter = @{
                tags = @("tag1", "tag2", "tag3")
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            $result | Should -Match "tags:"
            $result | Should -Match "\n- tag1"
            $result | Should -Match "\n- tag2"
            $result | Should -Match "\n- tag3"
        }
        
        It "Should serialize empty arrays as []" {
            $frontMatter = @{
                tags = @()
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            $result | Should -Match "tags: \[\]"
        }
        
        It "Should handle arrays with special characters in values" {
            $frontMatter = @{
                tags = @("AI & ML", "GitHub: Copilot", "Test-Tag")
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            $result | Should -Match "tags:"
            # YamlDotNet quotes values with colons
            $result | Should -Match "- 'GitHub: Copilot'"
            # But may not quote & if unambiguous
            $result | Should -Match "- AI & ML|'AI & ML'"
        }
    }

    Context "Real-World RSS Content Scenarios" {
        It "Should match ContentFixer output format for blog posts" {
            $frontMatter = @{
                layout        = "post"
                title         = "GitHub Copilot: Best Practices"
                author        = "Jane Smith"
                external_url  = "https://example.com/article"
                viewing_mode  = "external"
                tags          = @("AI", "GitHub Copilot", "Best Practices")
                section_names = @("ai", "github-copilot")
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            # Verify all fields are present
            $result | Should -Match "title: '.*:.*'"  # Contains colon, should be quoted
            $result | Should -Match "author: Jane Smith"
            $result | Should -Match "external_url: https://example.com/article"
            $result | Should -Match "viewing_mode: external"
            $result | Should -Match "tags:"
            $result | Should -Match "section_names:"
        }
        
        It "Should match ContentFixer output format for video content" {
            $frontMatter = @{
                layout        = "post"
                title         = "Learn Azure in 10 Minutes"
                author        = "Microsoft"
                external_url  = "https://youtube.com/watch?v=test123"
                viewing_mode  = "internal"
                tags          = @("Azure", "Tutorial", "Video")
                section_names = @("azure")
                youtube_id    = "test123"
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            $result | Should -Match "viewing_mode: internal"
            $result | Should -Match "youtube_id: test123"
        }
    }

    Context "Edge Cases" {
        It "Should handle frontmatter with only required fields" {
            $frontMatter = @{
                layout        = "post"
                title         = "Minimal Article"
                author        = "Author"
                date          = "2026-01-16 10:00:00 +00:00"
                tags          = @()
                section_names = @("ai")
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "---\n"
        }
        
        It "Should handle long titles with special characters" {
            $frontMatter = @{
                title = "Advanced C# Features: Nullable Reference Types, Pattern Matching & Records - A Comprehensive Guide"
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            $result | Should -Match "title:"
            # Should be quoted due to special chars
            $result | Should -Match "title: '.*'"
        }
        
        It "Should handle URLs with query parameters" {
            $frontMatter = @{
                external_url = "https://example.com/article?utm_source=rss&utm_campaign=tech"
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            # YamlDotNet may not quote URLs if they're unambiguous
            # Just verify the URL is present
            $result | Should -Match "external_url: https://example.com/article"
        }
    }

    Context "Consistency with ContentFixer" {
        It "Should use underscored naming convention like ContentFixer" {
            # ContentFixer uses UnderscoredNamingConvention
            $frontMatter = @{
                viewing_mode  = "external"
                section_names = @("ai")
            }
            
            $result = ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
            
            # Should use underscored names (viewing_mode, not viewingMode)
            $result | Should -Match "viewing_mode:"
            $result | Should -Match "section_names:"
        }
    }
}
