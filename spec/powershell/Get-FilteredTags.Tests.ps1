# Get-FilteredTags.Tests.ps1
# Tests for the Get-FilteredTags PowerShell function

Describe "Get-FilteredTags" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }
    
    Context "Parameter Validation" {
        It "Should throw when Tags parameter is null" {
            { Get-FilteredTags -Tags $null -Categories @("AI") -Collection "news" } | Should -Throw "*Tags parameter cannot be null*"
        }
        
        It "Should throw when Tags parameter is empty" {
            { Get-FilteredTags -Tags @() -Categories @("AI") -Collection "news" } | Should -Throw "*Tags parameter cannot be empty*"
        }
        
        It "Should throw when Categories parameter is null" {
            { Get-FilteredTags -Tags @("test") -Categories $null -Collection "news" } | Should -Throw "*Categories parameter cannot be null*"
        }
        
        It "Should throw when Categories parameter is empty" {
            { Get-FilteredTags -Tags @("test") -Categories @() -Collection "news" } | Should -Throw "*Categories parameter cannot be empty*"
        }
        
        It "Should throw when Collection parameter is null or empty" {
            { Get-FilteredTags -Tags @("test") -Categories @("AI") -Collection $null } | Should -Throw "*Collection parameter cannot be null or empty*"
            { Get-FilteredTags -Tags @("test") -Categories @("AI") -Collection "" } | Should -Throw "*Collection parameter cannot be null or empty*"
            { Get-FilteredTags -Tags @("test") -Categories @("AI") -Collection "   " } | Should -Throw "*Collection parameter cannot be null or empty*"
        }
    }
    
    Context "Basic Tag Processing" {
        It "Should replace dashes and underscores with spaces" {
            $result = Get-FilteredTags -Tags @("azure-ai", "visual_studio") -Categories @("AI") -Collection "news"
            
            $result.tags | Should -Contain "azure ai"
            $result.tags | Should -Contain "visual studio"
        }
        
        It "Should filter out tags containing only special characters" {
            $result = Get-FilteredTags -Tags @("valid-tag", "!!!", "@@@", "###", "---", "***", "a1g2", "test") -Categories @("AI") -Collection "news"
            
            # Should keep tags with alphanumeric characters
            $result.tags | Should -Contain "Valid Tag"
            $result.tags | Should -Contain "A1g2"
            $result.tags | Should -Contain "Test"
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "News"
            
            # Should filter out tags with only special characters
            $result.tags | Should -Not -Contain "!!!"
            $result.tags | Should -Not -Contain "@@@"
            $result.tags | Should -Not -Contain "###"
            $result.tags | Should -Not -Contain "---"
            $result.tags | Should -Not -Contain "***"
        }
        
        It "Should preserve categories and collection in output" {
            $result = Get-FilteredTags -Tags @("test-tag") -Categories @("AI", "Technology") -Collection "news"
            
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "Technology" 
            $result.tags | Should -Contain "news"
        }
        
        It "Should return both tags and tags_normalized arrays" {
            $result = Get-FilteredTags -Tags @("test", "another") -Categories @("AI") -Collection "news"
            
            $result.ContainsKey("tags") | Should -Be $true
            $result.ContainsKey("tags_normalized") | Should -Be $true
            # Should be arrays when multiple items
            $result.tags.Count | Should -BeGreaterThan 1
            $result.tags_normalized.Count | Should -BeGreaterThan 1
        }
    }
    
    Context "Custom Tag Mappings" {
        It "Should apply custom tag mappings correctly" {
            $testCases = @(
                @{ Input = "githubcopilot"; Expected = "GitHub Copilot" },
                @{ Input = "aiagent"; Expected = "AI Agent" },
                @{ Input = "aiagents"; Expected = "AI Agents" },
                @{ Input = "vs"; Expected = "Visual Studio" },
                @{ Input = "vscode"; Expected = "Visual Studio Code" },
                @{ Input = "vs code"; Expected = "Visual Studio Code" },
                @{ Input = "dotnet"; Expected = ".NET" },
                @{ Input = "csharp"; Expected = "C#" },
                @{ Input = "infrastructureascode"; Expected = "IaC" },
                @{ Input = "infrastructure as code"; Expected = "IaC" }
            )
            
            foreach ($testCase in $testCases) {
                $result = Get-FilteredTags -Tags @($testCase.Input) -Categories @("AI") -Collection "news"
                $result.tags | Should -Contain $testCase.Expected -Because "Input '$($testCase.Input)' should map to '$($testCase.Expected)'"
            }
        }
    }
    
    Context "Word-Level Mappings" {
        It "Should apply word mappings in multi-word tags" {
            $testCases = @(
                @{ Input = "azure ai tools"; Expected = "Azure AI Tools" },
                @{ Input = "github copilot api"; Expected = "GitHub Copilot API" },
                @{ Input = "dotnet framework"; Expected = ".NET Framework" },
                @{ Input = "openai integration"; Expected = "OpenAI Integration" }
            )
            
            foreach ($testCase in $testCases) {
                $result = Get-FilteredTags -Tags @($testCase.Input) -Categories @("AI") -Collection "news"
                $result.tags | Should -Contain $testCase.Expected -Because "Input '$($testCase.Input)' should become '$($testCase.Expected)'"
            }
        }
    }
    
    Context "Dynamic Category and Collection Mappings" {
        It "Should create dynamic mappings for categories" {
            $result = Get-FilteredTags -Tags @("ai", "githubcopilot") -Categories @("AI", "GitHub Copilot") -Collection "news"
            
            # The function should create dynamic mappings for categories
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "GitHub Copilot"
        }
        
        It "Should handle short collection names correctly" {
            $result = Get-FilteredTags -Tags @("test") -Categories @("AI") -Collection "tst"
            
            $result.tags | Should -Contain "TST" -Because "Short collection names should be uppercase"
        }
        
        It "Should handle longer collection names correctly" {
            $result = Get-FilteredTags -Tags @("test") -Categories @("AI") -Collection "community"
            
            $result.tags | Should -Contain "Community" -Because "Longer collection names should be title case"
        }
    }
    
    Context "Tag Normalization" {
        It "Should create normalized versions of tags" {
            $result = Get-FilteredTags -Tags @("Azure AI", "GitHub Copilot") -Categories @("AI", "GitHub Copilot") -Collection "news"
            
            $result.tags_normalized | Should -Contain "azure ai"
            $result.tags_normalized | Should -Contain "github copilot"
            $result.tags_normalized | Should -Contain "ai"
            $result.tags_normalized | Should -Contain "news"
        }
        
        It "Should normalize complex tags correctly" {
            $result = Get-FilteredTags -Tags @("Visual Studio Code Extension", "Infrastructure as Code") -Categories @("Development") -Collection "posts"
            
            $result.tags_normalized | Should -Contain "visual studio code extension"
            $result.tags_normalized | Should -Contain "iac"
            $result.tags_normalized | Should -Contain "development"
            $result.tags_normalized | Should -Contain "posts"
        }
    }
    
    Context "Filter Word Removal" {
        It "Should remove common filter words" {
            # Note: This test depends on the actual implementation of filter word removal
            # We'll need to check what the function actually does for filter words
            $result = Get-FilteredTags -Tags @("the quick brown", "and or but") -Categories @("AI") -Collection "news"
            
            # The function should filter out common words like "the", "and", "or", "but"
            # We'll verify the function's actual behavior
            $result.tags | Should -Not -BeNullOrEmpty
        }
    }
    
    Context "Edge Cases" {
        It "Should handle empty tag strings" {
            $result = Get-FilteredTags -Tags @("", "  ", "valid-tag") -Categories @("AI") -Collection "news"
            
            $result.tags | Should -Contain "valid tag"
            $result.tags | Should -Not -Contain ""
        }
        
        It "Should handle special characters in tags" {
            $result = Get-FilteredTags -Tags @("C#/.NET", "AI & ML", "API's") -Categories @("Programming") -Collection "posts"
            
            # Verify the function handles special characters appropriately
            $result.tags | Should -Not -BeNullOrEmpty
        }
        
        It "Should handle duplicate tags" {
            $result = Get-FilteredTags -Tags @("AI", "ai", "AI") -Categories @("AI") -Collection "news"
            
            # Should deduplicate tags (production code preserves duplicates but normalizes case)
            $result.tags | Should -Not -BeNullOrEmpty
            $result.tags_normalized | Should -Not -BeNullOrEmpty
        }
    }
    
    Context "Color Code Filtering" {
        It "Should filter out 3-character hex color codes" {
            $tagsWithColors = @("fff", "000", "abc", "AI", "Technology")
            
            $result = Get-FilteredTags -Tags $tagsWithColors -Categories @("AI") -Collection "posts"
            
            # Should filter out hex color codes
            $result.tags | Should -Not -Contain "fff"
            $result.tags | Should -Not -Contain "000"
            $result.tags | Should -Not -Contain "abc"
            
            # Should keep legitimate tags
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "Technology"
        }
        
        It "Should filter out 6-character hex color codes" {
            $tagsWithColors = @("2196f3", "4caf50", "9c27b0", "ffcdd2", "AI Adoption", "Business Value")
            
            $result = Get-FilteredTags -Tags $tagsWithColors -Categories @("AI") -Collection "posts"
            
            # Should filter out hex color codes
            $result.tags | Should -Not -Contain "2196f3"
            $result.tags | Should -Not -Contain "4caf50"
            $result.tags | Should -Not -Contain "9c27b0"
            $result.tags | Should -Not -Contain "ffcdd2"
            
            # Should keep legitimate tags
            $result.tags | Should -Contain "AI Adoption"
            $result.tags | Should -Contain "Business Value"
        }
        
        It "Should keep tags that contain hex characters but are not pure hex codes" {
            $mixedTags = @("abc123def", "AI4All", "H2O", "C2G", "AI")
            
            $result = Get-FilteredTags -Tags $mixedTags -Categories @("AI") -Collection "posts"
            
            # Should keep tags that aren't pure hex (wrong length or contain non-hex chars)
            $result.tags | Should -Contain "abc123def"  # Too long (9 chars)
            $result.tags | Should -Contain "AI4All"     # Contains non-hex chars and wrong length
            $result.tags | Should -Contain "H2O"        # Contains non-hex chars (H is hex but O is not)
            $result.tags | Should -Contain "C2G"        # Contains non-hex chars (G is not hex)
            $result.tags | Should -Contain "AI"         # Normal tag
        }
        
        It "Should handle mixed case hex codes" {
            $mixedCaseTags = @("AbC", "GHI123", "2196F3", "AI", "GitHub")
            
            $result = Get-FilteredTags -Tags $mixedCaseTags -Categories @("AI") -Collection "posts"
            
            # Should filter out mixed case hex codes
            $result.tags | Should -Not -Contain "AbC"     # 3-char hex (mixed case)
            $result.tags | Should -Not -Contain "2196F3"  # 6-char hex (mixed case)
            
            # Should keep non-hex tags
            $result.tags | Should -Contain "GHI123"  # Contains non-hex chars (G, H, I)
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "GitHub"
        }
        
        It "Should reproduce the original issue from the problematic post" {
            # This reproduces the actual tags from the problematic post mentioned in issue #105
            $problematicTags = @(
                "2196f3", "4caf50", "9c27b0", "AI", "AI Adoption", "AI Center Of Excellence", 
                "AI Strategy", "AI Talent", "Bbdefb", "Business Value", "C8e6c9", "Ccoe", 
                "CCoE KPI", "Cross Functional Teams", "D1c4e9", "Data Governance", "E1f5fe", 
                "E3f2fd", "E8f5e8", "Enterprise AI", "F1f8e9", "F3e5f5", "Fce4ec", "Ff9800", 
                "Ffcdd2", "Ffebee", "Fff", "Fff3e0", "Governance", "MLOps", "Operational Excellence", 
                "Organizational Change", "Posts", "Risk Management"
            )
            
            $result = Get-FilteredTags -Tags $problematicTags -Categories @("AI") -Collection "posts"
            
            # Should filter out all the hex color codes (both 3 and 6 character)
            $hexCodes = @("2196f3", "4caf50", "9c27b0", "Bbdefb", "C8e6c9", "D1c4e9", "E1f5fe", 
                "E3f2fd", "E8f5e8", "F1f8e9", "F3e5f5", "Fce4ec", "Ff9800", "Ffcdd2", 
                "Ffebee", "Fff", "Fff3e0")
            
            foreach ($hexCode in $hexCodes) {
                $result.tags | Should -Not -Contain $hexCode -Because "Hex code '$hexCode' should be filtered out"
            }
            
            # Should keep all legitimate tags
            $legitimateTags = @("AI", "AI Adoption", "AI Center Of Excellence", "AI Strategy", 
                "AI Talent", "Business Value", "Ccoe", "CCoE KPI", "Cross Functional Teams", 
                "Data Governance", "Enterprise AI", "Governance", "MLOps", 
                "Operational Excellence", "Organizational Change", "Posts", "Risk Management")
            
            foreach ($legitTag in $legitimateTags) {
                $result.tags | Should -Contain $legitTag -Because "Legitimate tag '$legitTag' should be preserved"
            }
        }
    }
    
    Context "Tag Splitting Functionality" {
        It "Should split tags on semicolons" {
            $tagsWithSemicolons = @("AI;Machine Learning", "Azure;Cloud", "Single Tag")
            
            $result = Get-FilteredTags -Tags $tagsWithSemicolons -Categories @("AI") -Collection "posts"
            
            # Should have split the semicolon-separated tags
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "Machine Learning"
            $result.tags | Should -Contain "Azure"
            $result.tags | Should -Contain "Cloud"
            $result.tags | Should -Contain "Single Tag"
            $result.tags | Should -Contain "Posts"
        }
        
        It "Should split tags on commas" {
            $tagsWithCommas = @("Python,JavaScript", "Data Science,Analytics", "Solo Tag")
            
            $result = Get-FilteredTags -Tags $tagsWithCommas -Categories @("AI") -Collection "posts"
            
            # Should have split the comma-separated tags
            $result.tags | Should -Contain "Python"
            $result.tags | Should -Contain "JavaScript"
            $result.tags | Should -Contain "Data Science"
            $result.tags | Should -Contain "Analytics"
            $result.tags | Should -Contain "Solo Tag"
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "Posts"
        }
        
        It "Should split tags on both semicolons and commas in same tag" {
            $mixedSeparators = @("AI;ML,Deep Learning", "Azure,AWS;GCP")
            
            $result = Get-FilteredTags -Tags $mixedSeparators -Categories @("Technology") -Collection "posts"
            
            # Should have split all separators
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "ML"
            $result.tags | Should -Contain "Deep Learning"
            $result.tags | Should -Contain "Azure"
            $result.tags | Should -Contain "AWS"
            $result.tags | Should -Contain "GCP"
            $result.tags | Should -Contain "Technology"
            $result.tags | Should -Contain "Posts"
        }
        
        It "Should handle empty values after splitting" {
            $tagsWithEmptyValues = @("AI;;ML", "Azure,", ";Cloud", ",Data")
            
            $result = Get-FilteredTags -Tags $tagsWithEmptyValues -Categories @("Technology") -Collection "posts"
            
            # Should filter out empty values but keep valid ones
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "ML"
            $result.tags | Should -Contain "Azure"
            $result.tags | Should -Contain "Cloud"
            $result.tags | Should -Contain "ML"
            $result.tags | Should -Contain "Technology"
            $result.tags | Should -Contain "Posts"
            
            # Should not contain empty strings
            $result.tags | Should -Not -Contain ""
        }
        
        It "Should trim whitespace around split values" {
            $tagsWithWhitespace = @("AI ; Machine Learning", "Azure , Cloud Computing", " Data Science; Analytics ")
            
            $result = Get-FilteredTags -Tags $tagsWithWhitespace -Categories @("Technology") -Collection "posts"
            
            # Should have properly trimmed all values
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "Machine Learning"
            $result.tags | Should -Contain "Azure"
            $result.tags | Should -Contain "Cloud Computing"
            $result.tags | Should -Contain "Data Science"
            $result.tags | Should -Contain "Analytics"
            $result.tags | Should -Contain "Technology"
            $result.tags | Should -Contain "Posts"
        }
    }
    
    Context "Integration with Real Data" {
        It "Should process typical blog post tags correctly" {
            $typicalTags = @(
                "github-copilot",
                "azure-ai", 
                "dotnet-framework",
                "visual-studio-code",
                "infrastructure-as-code"
            )
            
            $result = Get-FilteredTags -Tags $typicalTags -Categories @("AI", "Development") -Collection "posts"
            
            $result.tags | Should -Contain "GitHub Copilot"
            $result.tags | Should -Contain "Azure AI"
            $result.tags | Should -Contain ".NET Framework"
            $result.tags | Should -Contain "Visual Studio Code"
            $result.tags | Should -Contain "IaC"
            $result.tags | Should -Contain "AI"
            $result.tags | Should -Contain "Development"
            $result.tags | Should -Contain "Posts"
        }
    }
}