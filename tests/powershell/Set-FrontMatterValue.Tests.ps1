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

            $result | Should -Match 'categories:\s+- "AI"\s+- "Azure"'
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

            $result | Should -Match 'tags:\s+- "Azure"\s+- "Cloud"'
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

            $result | Should -Match 'category:\s+- "Azure"'
        }
        
        It "Should handle multiple item array" {
            $content = @"
---
title: Test Article
---
Content here
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "tags" -Value @("Azure", "Cloud", "AI")

            $result | Should -Match 'tags:\s+- "Azure"\s+- "Cloud"\s+- "AI"'
        }
        
        It "Should replace existing block sequence array" {
            $content = @"
---
title: Test Article
tags:
  - "Old Tag 1"
  - "Old Tag 2"
  - "Old Tag 3"
---
Content here
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "tags" -Value @("New Tag 1", "New Tag 2")

            $result | Should -Match 'tags:\s+- "New Tag 1"\s+- "New Tag 2"'
            $result | Should -Not -Match "Old Tag"
            $result | Should -Match "Content here"
        }
        
        It "Should replace block sequence array preserving other fields" {
            $content = @"
---
title: Test Article
section_names:
  - "ai"
  - "azure"
author: Original Author
---
Content here
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "section_names" -Value @("github-copilot", "dotnet")

            $result | Should -Match "title: Test Article"
            $result | Should -Match 'section_names:\s+- "github-copilot"\s+- "dotnet"'
            $result | Should -Match "author: Original Author"
            $result | Should -Not -Match '"ai"'
            $result | Should -Not -Match '"azure"'
        }
        
        It "Should replace flow sequence array with block sequence" {
            $content = @"
---
title: Test Article
tags: ["old1", "old2"]
---
Content here
"@
            
            $result = Set-FrontMatterValue -Content $content -Key "tags" -Value @("new1", "new2")

            $result | Should -Match 'tags:\s+- "new1"\s+- "new2"'
            $result | Should -Not -Match '\["old1"'
        }
    }
}
