# Set-FrontMatterValue.Tests.ps1
# Tests for the Set-FrontMatterValue PowerShell function

Describe "Set-FrontMatterValue" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
    }
    
    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }

    Context "Setting Frontmatter Values" {
        It "Should set title in frontmatter" {
            $content = @"
---
author: Test Author
---
Content here
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "title" -Value @("New Title")
            
            $result | Should -Match "title: `"New Title`""
            $result | Should -Match "author: Test Author"
            $result | Should -Match "Content here"
        }
        
        It "Should update existing categories array" {
            $content = @"
---
title: Test Article
categories: [AI]
---
Content here
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "categories" -Value @("AI", "Azure")

            $result | Should -Match "categories: \[`"AI`", `"Azure`"\]"
            $result | Should -Match "Content here"
        }
        
        It "Should add new array field to frontmatter" {
            $content = @"
---
title: Test Article
---
Content here
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "tags" -Value @("Azure", "Cloud")

            $result | Should -Match "tags: \[`"Azure`", `"Cloud`"\]"
            $result | Should -Match "Content here"
        }
        
        It "Should preserve existing frontmatter when adding new field" {
            $content = @"
---
title: Original Title
author: Original Author
date: 2025-07-30
---
Original content
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "description" -Value @("New Description")
            
            $result | Should -Match "title: Original Title"
            $result | Should -Match "author: Original Author"
            $result | Should -Match "description: `"New Description`""
            $result | Should -Match "Original content"
        }
    }
    
    Context "Error Handling" {
        It "Should handle empty key" {
            $content = @"
---
title: Test
---
Content
"@
            { Set-FrontMatterValue -Content $content -Key "" -Value @("Test") } | Should -Not -Throw
        }
        
        It "Should handle null content" {
            { Set-FrontMatterValue -Content $null -Key "title" -Value @("Test") } | Should -Not -Throw
        }
    }
    
    Context "Edge Cases" {
        It "Should handle content without existing frontmatter" {
            $content = "Just plain content without frontmatter"
            
            $result = Set-FrontMatterValue -Content $content -Key "title" -Value @("Added Title")
            
            $result | Should -Match "---"
            $result | Should -Match "title: `"Added Title`""
            $result | Should -Match "Just plain content without frontmatter"
        }
        
        It "Should handle single item array" {
            $content = @"
---
title: Test Article
---
Content here
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "category" -Value @("Azure")

            $result | Should -Match "category: \[`"Azure`"\]"
        }
        
        It "Should handle multiple item array" {
            $content = @"
---
title: Test Article
---
Content here
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "tags" -Value @("Azure", "Cloud", "AI")

            $result | Should -Match "tags: \[`"Azure`", `"Cloud`", `"AI`"\]"
        }
    }
}
