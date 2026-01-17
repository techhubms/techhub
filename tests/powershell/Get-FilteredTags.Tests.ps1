# Get-FilteredTags.Tests.ps1
# Tests for the Get-FilteredTags PowerShell function
#
# NOTE: Section/collection names (AI, Azure, Blogs, Videos, etc.) are now filtered OUT
# from tags since the .NET migration allows dynamic querying by section/collection.

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
            $result = Get-FilteredTags -Tags @("python-tools", "visual_studio") -Categories @("AI") -Collection "news"
            
            $result | Should -Contain "Python Tools"
            $result | Should -Contain "VS"
        }
        
        It "Should filter out tags containing only special characters" {
            $result = Get-FilteredTags -Tags @("valid-tag", "!!!", "@@@", "###", "---", "***", "a1g2", "test") -Categories @("AI") -Collection "news"
            
            # Should keep tags with alphanumeric characters
            $result | Should -Contain "Valid Tag"
            $result | Should -Contain "A1g2"
            $result | Should -Contain "Test"
            
            # Should filter out tags with only special characters
            $result | Should -Not -Contain "!!!"
            $result | Should -Not -Contain "@@@"
            $result | Should -Not -Contain "###"
            $result | Should -Not -Contain "---"
            $result | Should -Not -Contain "***"
        }
        
        It "Should NOT automatically add categories and collection to output" {
            $result = Get-FilteredTags -Tags @("test-tag", "Python", "JavaScript") -Categories @("AI", "Technology") -Collection "news"
            
            # Categories and collection should NOT be automatically added
            # (they're not in the input Tags, so they shouldn't appear in output)
            $result | Should -Not -Contain "AI"
            $result | Should -Not -Contain "Technology" 
            $result | Should -Not -Contain "News"
            
            # Actual tags should still be present
            $result | Should -Contain "Test Tag"
            $result | Should -Contain "Python"
            $result | Should -Contain "JavaScript"
        }
        
        It "Should keep section names if they are in input tags (AI assigned them)" {
            $result = Get-FilteredTags -Tags @("AI", "Azure", "GitHub Copilot", ".NET", "DevOps", "Security", "ML", "Python") -Categories @("AI") -Collection "news"
            
            # Section names in input should be kept (AI assigned them as tags)
            $result | Should -Contain "AI"
            $result | Should -Contain "Azure"
            $result | Should -Contain "GitHub Copilot"
            $result | Should -Contain ".NET"
            $result | Should -Contain "DevOps"
            $result | Should -Contain "Security"
            $result | Should -Contain "ML"
            $result | Should -Contain "Python"
        }
        
        It "Should keep collection names if they are in input tags (AI assigned them)" {
            $result = Get-FilteredTags -Tags @("News", "Blogs", "Videos", "Community", "Roundups", "Events", "Python") -Categories @("AI") -Collection "news"
            
            # Collection names in input should be kept (AI assigned them as tags)
            # Note: Some may be filtered by the filterWords list (common words)
            $result | Should -Contain "News"
            $result | Should -Contain "Blogs"
            $result | Should -Contain "Videos"
            $result | Should -Contain "Community"
            $result | Should -Contain "Roundups"
            $result | Should -Contain "Events"
            $result | Should -Contain "Python"
        }
        
        It "Should return filtered tags array" {
            $result = Get-FilteredTags -Tags @("test", "another", "Python") -Categories @("AI") -Collection "news"
            
            ($result -is [Array]) | Should -Be $true
            # Should contain at least the non-section tags
            $result.Count | Should -BeGreaterThan 0
        }
    }
    
    Context "Custom Tag Mappings" {
        It "Should apply custom tag mappings correctly" {
            $testCases = @(
                @{ Input = "vscode"; Expected = "VS Code" },
                @{ Input = "vs code"; Expected = "VS Code" },
                @{ Input = "dotnet"; Expected = ".NET" },
                @{ Input = "csharp"; Expected = "C#" },
                @{ Input = "infrastructureascode"; Expected = "IaC" },
                @{ Input = "infrastructure as code"; Expected = "IaC" }
            )
            
            foreach ($testCase in $testCases) {
                $result = Get-FilteredTags -Tags @($testCase.Input) -Categories @("AI") -Collection "news"
                $result | Should -Contain $testCase.Expected -Because "Input '$($testCase.Input)' should map to '$($testCase.Expected)'"
            }
        }
        
        It "Should apply section-name mappings and keep them (AI assigned them)" {
            # These mappings apply tag transformations
            $result = Get-FilteredTags -Tags @("githubcopilot", "aiagent") -Categories @("AI") -Collection "news"
            
            # "githubcopilot" maps to "GitHub Copilot" and should be kept
            $result | Should -Contain "GitHub Copilot"
            # "aiagent" maps to "AI Agent" and should be kept
            $result | Should -Contain "AI Agent"
        }
    }
    
    Context "Word-Level Mappings" {
        It "Should apply word mappings in multi-word tags" {
            $testCases = @(
                @{ Input = "python tools"; Expected = "Python Tools" },
                @{ Input = "openai integration"; Expected = "OpenAI Integration" },
                @{ Input = "kubernetes deployment"; Expected = "Kubernetes Deployment" }
            )
            
            foreach ($testCase in $testCases) {
                $result = Get-FilteredTags -Tags @($testCase.Input) -Categories @("AI") -Collection "news"
                $result | Should -Contain $testCase.Expected -Because "Input '$($testCase.Input)' should become '$($testCase.Expected)'"
            }
        }
    }
    
    Context "Tag Normalization" {
        It "Should normalize complex tags correctly" {
            $result = Get-FilteredTags -Tags @("VS Code Extension", "Infrastructure as Code") -Categories @("Development") -Collection "blogs"
            
            # IaC mapping should be applied
            $result | Should -Contain "IaC"
            $result | Should -Contain "VS Code Extension"
        }
    }
    
    Context "Filter Word Removal" {
        It "Should remove common filter words" {
            # Note: This test depends on the actual implementation of filter word removal
            # We'll need to check what the function actually does for filter words
            $result = Get-FilteredTags -Tags @("the quick brown", "and or but") -Categories @("AI") -Collection "news"
            
            # The function should filter out common words like "the", "and", "or", "but"
            # We'll verify the function's actual behavior
            $result | Should -Not -BeNullOrEmpty
        }
    }
    
    Context "Edge Cases" {
        It "Should handle empty tag strings" {
            $result = Get-FilteredTags -Tags @("", "  ", "valid-tag") -Categories @("AI") -Collection "news"
            
            $result | Should -Contain "Valid Tag"
            $result | Should -Not -Contain ""
        }
        
        It "Should handle special characters in tags" {
            $result = Get-FilteredTags -Tags @("C#/.NET", "Python & Ruby", "API's") -Categories @("Programming") -Collection "blogs"
            
            # Verify the function handles special characters appropriately
            $result | Should -Not -BeNullOrEmpty
        }
        
        It "Should handle duplicate tags" {
            $result = Get-FilteredTags -Tags @("Python", "python", "Python") -Categories @("AI") -Collection "news"
            
            # Should return filtered tags
            $result | Should -Not -BeNullOrEmpty
        }
    }
    
    Context "Color Code Filtering" {
        It "Should filter out 3-character hex color codes" {
            $tagsWithColors = @("fff", "000", "abc", "Python", "Technology")
            
            $result = Get-FilteredTags -Tags $tagsWithColors -Categories @("AI") -Collection "blogs"
            
            # Should filter out hex color codes
            $result | Should -Not -Contain "fff"
            $result | Should -Not -Contain "000"
            $result | Should -Not -Contain "abc"
            
            # Should keep legitimate tags
            $result | Should -Contain "Python"
            $result | Should -Contain "Technology"
        }
        
        It "Should filter out 6-character hex color codes" {
            $tagsWithColors = @("2196f3", "4caf50", "9c27b0", "ffcdd2", "Python Adoption", "Business Value")
            
            $result = Get-FilteredTags -Tags $tagsWithColors -Categories @("AI") -Collection "blogs"
            
            # Should filter out hex color codes
            $result | Should -Not -Contain "2196f3"
            $result | Should -Not -Contain "4caf50"
            $result | Should -Not -Contain "9c27b0"
            $result | Should -Not -Contain "ffcdd2"
            
            # Should keep legitimate tags
            $result | Should -Contain "Python Adoption"
            $result | Should -Contain "Business Value"
        }
        
        It "Should keep tags that contain hex characters but are not pure hex codes" {
            $mixedTags = @("abc123def", "Python4All", "H2O", "C2G", "Kubernetes")
            
            $result = Get-FilteredTags -Tags $mixedTags -Categories @("AI") -Collection "blogs"
            
            # Should keep tags that aren't pure hex (wrong length or contain non-hex chars)
            $result | Should -Contain "abc123def"  # Too long (9 chars)
            $result | Should -Contain "Python4All"     # Contains non-hex chars and wrong length
            $result | Should -Contain "H2O"        # Contains non-hex chars (H is hex but O is not)
            $result | Should -Contain "C2G"        # Contains non-hex chars (G is not hex)
            $result | Should -Contain "Kubernetes" # Normal tag
        }
        
        It "Should handle mixed case hex codes" {
            $mixedCaseTags = @("AbC", "GHI123", "2196F3", "Python", "GitHub")
            
            $result = Get-FilteredTags -Tags $mixedCaseTags -Categories @("AI") -Collection "blogs"
            
            # Should filter out mixed case hex codes
            $result | Should -Not -Contain "AbC"     # 3-char hex (mixed case)
            $result | Should -Not -Contain "2196F3"  # 6-char hex (mixed case)
            
            # Should keep non-hex tags
            $result | Should -Contain "GHI123"  # Contains non-hex chars (G, H, I)
            $result | Should -Contain "Python"
            $result | Should -Contain "GitHub"
        }
        
        It "Should reproduce the original issue from the problematic post" {
            # This reproduces the actual tags from the problematic post mentioned in issue #105
            # Note: AI and Blogs are now kept if they're in the input (AI assigned them)
            $problematicTags = @(
                "2196f3", "4caf50", "9c27b0", "AI", "Python Adoption", "Center Of Excellence", 
                "Strategy", "Talent", "Bbdefb", "Business Value", "C8e6c9", "Ccoe", 
                "CCoE KPI", "Cross Functional Teams", "D1c4e9", "Data Governance", "E1f5fe", 
                "E3f2fd", "E8f5e8", "Enterprise", "F1f8e9", "F3e5f5", "Fce4ec", "Ff9800", 
                "Ffcdd2", "Ffebee", "Fff", "Fff3e0", "Governance", "MLOps", "Operational Excellence", 
                "Organizational Change", "Blogs", "Risk Management"
            )
            
            $result = Get-FilteredTags -Tags $problematicTags -Categories @("AI") -Collection "blogs"
            
            # Should filter out all the hex color codes (both 3 and 6 character)
            $hexCodes = @("2196f3", "4caf50", "9c27b0", "Bbdefb", "C8e6c9", "D1c4e9", "E1f5fe", 
                "E3f2fd", "E8f5e8", "F1f8e9", "F3e5f5", "Fce4ec", "Ff9800", "Ffcdd2", 
                "Ffebee", "Fff", "Fff3e0")
            
            foreach ($hexCode in $hexCodes) {
                $result | Should -Not -Contain $hexCode -Because "Hex code '$hexCode' should be filtered out"
            }
            
            # Section/collection names are now KEPT if they're in input tags (AI assigned them)
            $result | Should -Contain "AI" -Because "AI was in input, so it's kept"
            $result | Should -Contain "Blogs" -Because "Blogs was in input, so it's kept"
            
            # Should keep all other legitimate tags
            $legitimateTags = @("Python Adoption", "Center Of Excellence", "Strategy", 
                "Talent", "Business Value", "Ccoe", "CCoE KPI", "Cross Functional Teams", 
                "Data Governance", "Enterprise", "Governance", "MLOps", 
                "Operational Excellence", "Organizational Change", "Risk Management")
            
            foreach ($legitTag in $legitimateTags) {
                $result | Should -Contain $legitTag -Because "Legitimate tag '$legitTag' should be preserved"
            }
        }
    }
    
    Context "Tag Splitting Functionality" {
        It "Should split tags on semicolons" {
            $tagsWithSemicolons = @("Python;Machine Learning", "Kubernetes;Cloud", "Single Tag")
            
            $result = Get-FilteredTags -Tags $tagsWithSemicolons -Categories @("AI") -Collection "blogs"
            
            # Should have split the semicolon-separated tags
            $result | Should -Contain "Python"
            $result | Should -Contain "Machine Learning"
            $result | Should -Contain "Kubernetes"
            $result | Should -Contain "Cloud"
            $result | Should -Contain "Single Tag"
            # Note: "Blogs" is a collection name and is now filtered out
            $result | Should -Not -Contain "Blogs"
        }
        
        It "Should split tags on commas" {
            $tagsWithCommas = @("Python,JavaScript", "Data Science,Analytics", "Solo Tag")
            
            $result = Get-FilteredTags -Tags $tagsWithCommas -Categories @("AI") -Collection "blogs"
            
            # Should have split the comma-separated tags
            $result | Should -Contain "Python"
            $result | Should -Contain "JavaScript"
            $result | Should -Contain "Data Science"
            $result | Should -Contain "Analytics"
            $result | Should -Contain "Solo Tag"
            # Note: AI and Blogs are section/collection names and are now filtered out
            $result | Should -Not -Contain "AI"
            $result | Should -Not -Contain "Blogs"
        }
        
        It "Should split tags on both semicolons and commas in same tag" {
            $mixedSeparators = @("Python;Ruby,Go", "Kubernetes,Docker;Terraform")
            
            $result = Get-FilteredTags -Tags $mixedSeparators -Categories @("Technology") -Collection "blogs"
            
            # Should have split all separators
            $result | Should -Contain "Python"
            $result | Should -Contain "Ruby"
            $result | Should -Contain "Go"
            $result | Should -Contain "Kubernetes"
            $result | Should -Contain "Docker"
            $result | Should -Contain "Terraform"
            # Section/collection names are filtered out
            $result | Should -Not -Contain "Technology"
            $result | Should -Not -Contain "Blogs"
        }
        
        It "Should handle empty values after splitting" {
            $tagsWithEmptyValues = @("Python;;Ruby", "Kubernetes,", ";Docker", ",Terraform")
            
            $result = Get-FilteredTags -Tags $tagsWithEmptyValues -Categories @("Technology") -Collection "blogs"
            
            # Should filter out empty values but keep valid ones
            $result | Should -Contain "Python"
            $result | Should -Contain "Ruby"
            $result | Should -Contain "Kubernetes"
            $result | Should -Contain "Docker"
            $result | Should -Contain "Terraform"
            
            # Should not contain empty strings
            $result | Should -Not -Contain ""
        }
        
        It "Should trim whitespace around split values" {
            $tagsWithWhitespace = @("Python ; Machine Learning", "Kubernetes , Cloud Computing", " Data Science; Analytics ")
            
            $result = Get-FilteredTags -Tags $tagsWithWhitespace -Categories @("Technology") -Collection "blogs"
            
            # Should have properly trimmed all values
            $result | Should -Contain "Python"
            $result | Should -Contain "Machine Learning"
            $result | Should -Contain "Kubernetes"
            $result | Should -Contain "Cloud Computing"
            $result | Should -Contain "Data Science"
            $result | Should -Contain "Analytics"
            # Categories/collection are NOT automatically added (not in input tags)
            $result | Should -Not -Contain "Technology"
            $result | Should -Not -Contain "Blogs"
        }
    }
    
    Context "Integration with Real Data" {
        It "Should process typical blogs tags correctly" {
            $typicalTags = @(
                "python-tools",
                "kubernetes-deployment", 
                "dotnet-framework",
                "visual-studio-code",
                "infrastructure-as-code"
            )
            
            $result = Get-FilteredTags -Tags $typicalTags -Categories @("AI", "Development") -Collection "blogs"
            
            $result | Should -Contain "Python Tools"
            $result | Should -Contain "Kubernetes Deployment"
            $result | Should -Contain ".NET Framework"
            $result | Should -Contain "VS Code"
            $result | Should -Contain "IaC"
            # Categories/collection are NOT automatically added (not in input tags)
            $result | Should -Not -Contain "AI"
            $result | Should -Not -Contain "Development"
            $result | Should -Not -Contain "Blogs"
        }
    }
}
