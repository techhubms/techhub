Describe "ConvertTo-SafeFilename" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }

    Context "Parameter Validation" {
        It "Should throw when Title parameter is null" {
            { ConvertTo-SafeFilename -Title $null } | Should -Throw "*argument is null or empty*"
        }
        
        It "Should throw when Title parameter is empty" {
            { ConvertTo-SafeFilename -Title "" } | Should -Throw "*argument is null or empty*"
        }
        
        It "Should throw when Title parameter is whitespace only" {
            { ConvertTo-SafeFilename -Title "   " } | Should -Throw "*Title parameter cannot be null, empty, or whitespace*"
        }
        
        It "Should throw when MaxLength is zero" {
            { ConvertTo-SafeFilename -Title "Test" -MaxLength 0 } | Should -Throw "*less than the minimum allowed range of 1*"
        }
        
        It "Should throw when MaxLength is negative" {
            { ConvertTo-SafeFilename -Title "Test" -MaxLength -5 } | Should -Throw "*less than the minimum allowed range of 1*"
        }
    }
    
    Context "International Character Conversion" {
        It "Should convert accented vowels to ASCII equivalents" {
            $result = ConvertTo-SafeFilename -Title "Café Résumé Naïve"
            $result | Should -Be "Cafe-Resume-Naive"
        }
        
        It "Should convert various a-variants correctly" {
            $result = ConvertTo-SafeFilename -Title "àáâãäåāăą"
            $result | Should -Be "aaaaaaaaa"
        }
        
        It "Should convert various e-variants correctly" {
            $result = ConvertTo-SafeFilename -Title "èéêëēĕėęě"
            $result | Should -Be "eeeeeeeee"
        }
        
        It "Should convert various o-variants correctly" {
            $result = ConvertTo-SafeFilename -Title "òóôõöøōŏő"
            $result | Should -Be "ooooooooo"
        }
        
        It "Should convert consonants with diacritics" {
            $result = ConvertTo-SafeFilename -Title "ñ ç ł ž š"
            $result | Should -Be "n-c-l-z-s"
        }
        
        It "Should handle uppercase international characters" {
            $result = ConvertTo-SafeFilename -Title "ÀÁÂÃ ÈÉÊË ÌÍÎÏ"
            $result | Should -Be "AAAA-EEEE-IIII"
        }
        
        It "Should convert special European characters" {
            $result = ConvertTo-SafeFilename -Title "ß ĳ æ ð þ"
            $result | Should -Be "ss-ij-ae-dh-th"
        }
    }
    
    Context "Filesystem Safety" {
        It "Should remove Windows problematic characters" {
            $result = ConvertTo-SafeFilename -Title 'File<>:"/\|?*Name'
            $result | Should -Be "FileName"
        }
        
        It "Should remove special punctuation characters" {
            $result = ConvertTo-SafeFilename -Title "File[]{}();,!@#$%^&+=`~Name"
            $result | Should -Be "FileandName"
        }
        
        It "Should convert ampersand to 'and'" {
            $result = ConvertTo-SafeFilename -Title "AI & ML Guide"
            $result | Should -Be "AI-and-ML-Guide"
        }
        
        It "Should handle multiple ampersands with spacing" {
            $result = ConvertTo-SafeFilename -Title "A&B & C&D"
            $result | Should -Be "AandB-and-CandD"
        }
        
        It "Should remove dots from the end (Windows compatibility)" {
            $result = ConvertTo-SafeFilename -Title "filename..."
            $result | Should -Be "filename"
        }
    }
    
    Context "Slug Formatting" {
        It "Should convert spaces to hyphens" {
            $result = ConvertTo-SafeFilename -Title "Multiple Word Title"
            $result | Should -Be "Multiple-Word-Title"
        }
        
        It "Should normalize multiple spaces to single hyphens" {
            $result = ConvertTo-SafeFilename -Title "Multiple    Spaces     Here"
            $result | Should -Be "Multiple-Spaces-Here"
        }
        
        It "Should normalize multiple hyphens to single hyphens" {
            $result = ConvertTo-SafeFilename -Title "Multiple---Hyphens--Here"
            $result | Should -Be "Multiple-Hyphens-Here"
        }
        
        It "Should trim leading and trailing hyphens" {
            $result = ConvertTo-SafeFilename -Title "---Leading and Trailing---"
            $result | Should -Be "Leading-and-Trailing"
        }
        
        It "Should trim leading and trailing spaces" {
            $result = ConvertTo-SafeFilename -Title "   Spaced Title   "
            $result | Should -Be "Spaced-Title"
        }
    }
    
    Context "Length Management" {
        It "Should respect default MaxLength of 200 characters" {
            $longTitle = "A" * 250
            $result = ConvertTo-SafeFilename -Title $longTitle
            $result.Length | Should -BeLessOrEqual 200
        }
        
        It "Should respect custom MaxLength parameter" {
            $longTitle = "Very Long Title That Exceeds Limit"
            $result = ConvertTo-SafeFilename -Title $longTitle -MaxLength 10
            $result.Length | Should -BeLessOrEqual 10
        }
        
        It "Should trim hyphens after length truncation" {
            $title = "Word-After-Word-After-Word"
            $result = ConvertTo-SafeFilename -Title $title -MaxLength 10
            $result | Should -Not -Match '-$'
        }
    }
    
    Context "Edge Cases" {
        It "Should throw when no valid filename characters remain" {
            { ConvertTo-SafeFilename -Title "!@#$%^*()" } | Should -Throw "*Title contains no valid filename characters after processing*"
        }
        
        It "Should handle only international characters" {
            $result = ConvertTo-SafeFilename -Title "àáâãäå"
            $result | Should -Be "aaaaaa"
        }
        
        It "Should handle mixed case properly" {
            $result = ConvertTo-SafeFilename -Title "MiXeD CaSe TiTlE"
            $result | Should -Be "MiXeD-CaSe-TiTlE"
        }
        
        It "Should handle numbers and preserve them" {
            $result = ConvertTo-SafeFilename -Title "Article 123 Version 2.0"
            $result | Should -Be "Article-123-Version-20"
        }
        
        It "Should handle single character input" {
            $result = ConvertTo-SafeFilename -Title "A"
            $result | Should -Be "A"
        }
        
        It "Should handle Unicode quotes correctly" {
            $title = 'Microsoft''s "New" Feature'
            $result = ConvertTo-SafeFilename -Title $title
            $result | Should -Be "Microsofts-New-Feature"
        }
    }
    
    Context "Real-World Examples" {
        It "Should handle typical blog post title" {
            $result = ConvertTo-SafeFilename -Title "How to Use GitHub Copilot: A Complete Guide"
            $result | Should -Be "How-to-Use-GitHub-Copilot-A-Complete-Guide"
        }
        
        It "Should handle title with programming languages" {
            $result = ConvertTo-SafeFilename -Title "C# vs. C++: Performance Comparison"
            $result | Should -Be "C-vs-C-Performance-Comparison"
        }
        
        It "Should handle title with version numbers" {
            $result = ConvertTo-SafeFilename -Title "VS Code v1.85.0 Release Notes"
            $result | Should -Be "VS-Code-v1850-Release-Notes"
        }
        
        It "Should handle title with company names and products" {
            $result = ConvertTo-SafeFilename -Title "Microsoft's Azure AI: The Future of Cloud Computing"
            $result | Should -Be "Microsofts-Azure-AI-The-Future-of-Cloud-Computing"
        }
        
        It "Should handle scientific/technical articles" {
            $result = ConvertTo-SafeFilename -Title "AI/ML: Research & Development in 2025"
            $result | Should -Be "AIML-Research-and-Development-in-2025"
        }
    }
    
    Context "Pipeline Support" {
        It "Should work with pipeline input" {
            $result = "Test Title" | ConvertTo-SafeFilename
            $result | Should -Be "Test-Title"
        }
        
        It "Should handle multiple pipeline inputs" {
            $titles = @("First Title", "Second Title", "Third Title")
            $results = $titles | ConvertTo-SafeFilename
            $results | Should -HaveCount 3
            $results[0] | Should -Be "First-Title"
            $results[1] | Should -Be "Second-Title"
            $results[2] | Should -Be "Third-Title"
        }
    }
    
    Context "Performance" {
        It "Should process titles efficiently" {
            $title = "A reasonably long title with international characters like café and résumé"
            
            $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
            $result = ConvertTo-SafeFilename -Title $title
            $stopwatch.Stop()
            
            $result | Should -Not -BeNullOrEmpty
            $stopwatch.ElapsedMilliseconds | Should -BeLessThan 100 # Should complete within 100ms
        }
    }
}