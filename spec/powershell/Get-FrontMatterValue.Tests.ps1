# Get-FrontMatterValue.Tests.ps1
# Tests for the Get-FrontMatterValue PowerShell function

Describe "Get-FrontMatterValue" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }
    Context "Frontmatter Value Reading" {
        It "Should read title from frontmatter" {
            $content = @"
---
title: Test Article
description: Test description
---
Content here
"@
            $result = Get-FrontMatterValue -Content $content -Key "title"
            $result | Should -Be "Test Article"
        }
        
        It "Should read description from frontmatter" {
            $content = @"
---
title: Test Article
description: Test description
---
Content here
"@
            $result = Get-FrontMatterValue -Content $content -Key "description"
            $result | Should -Be "Test description"
        }
        
        It "Should read array values from frontmatter" {
            $content = @"
---
title: Test Article
tags: [AI, Azure, GitHub Copilot]
categories: [AI, Azure]
---
Content here
"@
            $result = Get-FrontMatterValue -Content $content -Key "tags"
            $result | Should -Contain "AI"
            $result | Should -Contain "Azure"
            $result | Should -Contain "GitHub Copilot"
        }
        
        It "Should return empty array for non-existent key" {
            $content = @"
---
title: Test Article
---
Content here
"@
            $result = Get-FrontMatterValue -Content $content -Key "nonexistent"
            $result | Should -Be @()
        }
        
        It "Should handle content without frontmatter" {
            $content = "Just plain content without frontmatter"
            $result = Get-FrontMatterValue -Content $content -Key "title"
            $result | Should -Be @()
        }
    }
    
    Context "Edge Cases" {
        It "Should handle empty content" {
            { Get-FrontMatterValue -Content "" -Key "title" } | Should -Not -Throw
        }
        
        It "Should handle empty key" {
            $content = @"
---
title: Test Article
-}}
Content here
"@
            { Get-FrontMatterValue -Content $content -Key "" } | Should -Not -Throw
        }
    }
}