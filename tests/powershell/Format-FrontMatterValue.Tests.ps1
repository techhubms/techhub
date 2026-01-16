# Format-FrontMatterValue.Tests.ps1
# Tests for the Format-FrontMatterValue PowerShell function

Describe "Format-FrontMatterValue" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }

    Context "Parameter Validation" {
        It "Should handle null input" {
            $result = Format-FrontMatterValue -Value $null
            $result | Should -Be $null
        }
        
        It "Should handle empty string input" {
            $result = Format-FrontMatterValue -Value ""
            $result | Should -Be '""'
        }
        
        It "Should handle whitespace-only input" {
            $result = Format-FrontMatterValue -Value "   "
            $result | Should -Be '""'
        }
    }
    
    Context "Single Value Processing" {
        It "Should clean and quote simple text" {
            $result = Format-FrontMatterValue -Value "Simple text"
            $result | Should -Be '"Simple text"'
        }
        
        It "Should trim leading and trailing whitespace" {
            $result = Format-FrontMatterValue -Value "  test value  "
            $result | Should -Be '"test value"'
        }
        
        It "Should replace multiple spaces with single space" {
            $result = Format-FrontMatterValue -Value "test    multiple     spaces"
            $result | Should -Be '"test multiple spaces"'
        }
        
        It "Should escape double quotes properly for YAML" {
            $result = Format-FrontMatterValue -Value 'Text with "quoted" content'
            $result | Should -Be '"Text with \"quoted\" content"'
        }
        
        It "Should handle line breaks by converting to spaces" {
            $result = Format-FrontMatterValue -Value "Line one`r`nLine two"
            $result | Should -Be '"Line one Line two"'
        }
    }
    
    Context "HTML Tag Removal" {
        It "Should remove HTML tags" {
            $testCases = @(
                @{ Input = "<p>Hello World</p>"; Expected = '"Hello World"' },
                @{ Input = "<div class='test'>Content</div>"; Expected = '"Content"' },
                @{ Input = "Text with <strong>bold</strong> and <em>italic</em>"; Expected = '"Text with bold and italic"' },
                @{ Input = "<a href='link'>Link text</a>"; Expected = '"Link text"' },
                @{ Input = "No tags here"; Expected = '"No tags here"' }
            )
            
            foreach ($testCase in $testCases) {
                $result = Format-FrontMatterValue -Value $testCase.Input
                $result | Should -Be $testCase.Expected -Because "Input '$($testCase.Input)' should become '$($testCase.Expected)'"
            }
        }
        
        It "Should handle nested HTML tags" {
            $inputText = "<div><p>Nested <strong>tags</strong> content</p></div>"
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Nested tags content"'
        }
        
        It "Should handle self-closing HTML tags" {
            $inputText = "Text with <br/> line break and <img src='test.jpg'/> image"
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Text with line break and image"'
        }
    }
    
    Context "Array Processing" {
        It "Should format simple string array" {
            $inputArray = @("First", "Second", "Third")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "First"
  - "Second"
  - "Third"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle empty array" {
            $inputArray = @()
            $result = Format-FrontMatterValue -Value $inputArray
            $result | Should -Be '[]'
        }
        
        It "Should handle single-item array" {
            $inputArray = @("Single item")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "Single item"
"@
            $result | Should -Be $expected
        }
        
        It "Should escape quotes in array items" {
            $inputArray = @('Item with "quotes"', "Another 'item'")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "Item with `"quotes`""
  - "Another 'item'"
"@
            $result | Should -Be $expected
        }
        
        It "Should clean HTML from array items" {
            $inputArray = @("<p>First item</p>", "<div>Second item</div>")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "First item"
  - "Second item"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle array with null or empty items" {
            $inputArray = @("Valid", "", $null, "Another valid")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "Valid"
  - ""
  - "Another valid"
"@
            $result | Should -Be $expected
        }
    }
    
    Context "Real-World Content Examples" {
        It "Should handle typical RSS feed content as single value" {
            $inputText = @"
<p>Learn about the latest GitHub Copilot features and how they can improve your development workflow.</p>
<p>Visit <a href="https://github.com/features/copilot">GitHub Copilot</a> for more information.</p>
"@
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Learn about the latest GitHub Copilot features and how they can improve your development workflow. Visit GitHub Copilot for more information."'
        }
        
        It "Should handle blog tags as array" {
            $inputArray = @("GitHub Copilot", "AI", "Development Tools", 'Feature "Update"')
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "GitHub Copilot"
  - "AI"
  - "Development Tools"
  - "Feature `"Update`""
"@
            $result | Should -Be $expected
        }
        
        It "Should handle categories as array" {
            $inputArray = @("AI", "GitHub Copilot")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "AI"
  - "GitHub Copilot"
"@
            $result | Should -Be $expected
        }
    }
    
    Context "Double-Escaping Prevention" {
        It "Should not double-escape already escaped quotes" {
            $inputText = 'Test Post with \"Quotes\"'
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Test Post with \"Quotes\""'
        }
        
        It "Should handle pre-escaped backslashes correctly" {
            $inputText = 'Path\\with\\backslashes'
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Path\\with\\backslashes"'
        }
        
        It "Should properly unescape and re-escape mixed content" {
            $inputText = 'Mixed \"quotes\" and \\backslashes\\'
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Mixed \"quotes\" and \\backslashes\\"'
        }
        
        It "Should handle strings with existing surrounding quotes" {
            $inputText = '"Already quoted string"'
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Already quoted string"'
        }
        
        It "Should handle single quotes and convert to double quotes" {
            $inputText = "'Single quoted string'"
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Single quoted string"'
        }
        
        It "Should handle backticks and convert to double quotes" {
            $inputText = '`Backtick quoted string`'
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Backtick quoted string"'
        }
        
        It "Should handle complex already-escaped JSON-like content" {
            $inputText = '{"key": "value with \"nested\" quotes"}'
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"{\"key\": \"value with \"nested\" quotes\"}"'
        }
        
        It "Should handle Windows file paths with escaped backslashes" {
            $inputText = 'C:\\Program Files\\\"My App\"\\file.exe'
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"C:\\Program Files\\\"My App\"\\file.exe"'
        }
        
        It "Should handle regex patterns with escaped characters" {
            $inputText = '\\d+\\.\\d+\\s*\"[^\"]*\"'
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"\\d+\\.\\d+\\s*\"[^\"]*\""'
        }
        
        It "Should handle arrays with pre-escaped content" {
            $inputArray = @('Item with \"quotes\"', 'Another\\with\\backslashes', '"Pre-quoted item"')
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "Item with `"quotes`""
  - "Another\with\backslashes"
  - "Pre-quoted item"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle empty strings after unescaping" {
            $inputText = '""'
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '""'
        }
        
        It "Should handle strings that become empty after quote removal" {
            $inputText = "`"'"
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '""'
        }
    }
    
    Context "Real-World Tag Examples" {
        It "Should handle C# tag correctly" {
            $result = Format-FrontMatterValue -Value "C#"
            $result | Should -Be '"C#"'
        }
        
        It "Should handle file path tags" {
            $result = Format-FrontMatterValue -Value "/a/b/c/"
            $result | Should -Be '"/a/b/c/"'
        }
        
        It "Should handle Windows path with backslashes" {
            $result = Format-FrontMatterValue -Value "\a\b\c"
            $result | Should -Be '"\\a\\b\\c"'
        }
        
        It "Should handle special character patterns" {
            $result = Format-FrontMatterValue -Value ".?"
            $result | Should -Be '".?"'
        }
        
        It "Should handle bracket notation" {
            $result = Format-FrontMatterValue -Value "[.]"
            $result | Should -Be '"[.]"'
        }
        
        It "Should handle mixed special characters" {
            $result = Format-FrontMatterValue -Value "C:\Program Files\App"
            $result | Should -Be '"C:\\Program Files\\App"'
        }
    }

    Context "Minimal YAML Escaping Behavior" {
        It "Should not escape a backslash that is an escape character" {
            # Single backslash should not be escaped
            $result = Format-FrontMatterValue -Value 'This article explains how to implement a \"build once, deploy everywhere\" approach using Azure Developer CLI (azd), covering Bicep conditional deployments, environment variable management, and automated CI/CD pipelines for streamlined promotion from development to production environments.'
            $result | Should -Be '"This article explains how to implement a \"build once, deploy everywhere\" approach using Azure Developer CLI (azd), covering Bicep conditional deployments, environment variable management, and automated CI/CD pipelines for streamlined promotion from development to production environments."'
        }

        It "Should add minimal escaping for YAML validity" {
            # Single backslash becomes double backslash
            $result = Format-FrontMatterValue -Value "test\value"
            $result | Should -Be '"test\\value"'
        }
        
        It "Should preserve already escaped content without over-escaping" {
            # Already escaped backslash stays as is
            $result = Format-FrontMatterValue -Value "test\\value"
            $result | Should -Be '"test\\value"'
        }
        
        It "Should escape unescaped quotes" {
            $result = Format-FrontMatterValue -Value 'say "hello"'
            $result | Should -Be '"say \"hello\""'
        }
        
        It "Should preserve already escaped quotes" {
            $result = Format-FrontMatterValue -Value 'say \"hello\"'
            $result | Should -Be '"say \"hello\""'
        }
        
        It "Should handle mix of escaped and unescaped content" {
            $result = Format-FrontMatterValue -Value 'path\to"file"'
            $result | Should -Be '"path\\to\"file\""'
        }
    }

    Context "YAML Special Character Handling" {
        It "Should escape backslashes according to YAML spec" {
            $result = Format-FrontMatterValue -Value "Path with \ backslash"
            $result | Should -Be '"Path with \\ backslash"'
        }
        
        It "Should escape backslashes before double quotes" {
            $result = Format-FrontMatterValue -Value 'Path like C:\"Program Files"'
            $result | Should -Be '"Path like C:\"Program Files\""'
        }
        
        It "Should handle YAML indicator characters safely" {
            $testCases = @(
                @{ Input = "Text with : colon"; Expected = '"Text with : colon"' },
                @{ Input = "Text with - dash"; Expected = '"Text with - dash"' },
                @{ Input = "Text with ? question"; Expected = '"Text with ? question"' },
                @{ Input = "Text with # hash"; Expected = '"Text with # hash"' }
            )
            
            foreach ($testCase in $testCases) {
                $result = Format-FrontMatterValue -Value $testCase.Input
                $result | Should -Be $testCase.Expected
            }
        }
        
        It "Should handle YAML flow collection indicators" {
            $testCases = @(
                @{ Input = "Text with [brackets]"; Expected = '"Text with [brackets]"' },
                @{ Input = "Text with {braces}"; Expected = '"Text with {braces}"' },
                @{ Input = "Text with , comma"; Expected = '"Text with , comma"' }
            )
            
            foreach ($testCase in $testCases) {
                $result = Format-FrontMatterValue -Value $testCase.Input
                $result | Should -Be $testCase.Expected
            }
        }
        
        It "Should handle YAML reserved words and literals" {
            $testCases = @(
                @{ Input = "true"; Expected = '"true"' },
                @{ Input = "false"; Expected = '"false"' },
                @{ Input = "null"; Expected = '"null"' },
                @{ Input = "yes"; Expected = '"yes"' },
                @{ Input = "no"; Expected = '"no"' },
                @{ Input = "~"; Expected = '"~"' }
            )
            
            foreach ($testCase in $testCases) {
                $result = Format-FrontMatterValue -Value $testCase.Input
                $result | Should -Be $testCase.Expected
            }
        }
        
        It "Should handle YAML document markers" {
            $testCases = @(
                @{ Input = "---"; Expected = '"---"' },
                @{ Input = "..."; Expected = '"..."' },
                @{ Input = "Text with --- in middle"; Expected = '"Text with --- in middle"' }
            )
            
            foreach ($testCase in $testCases) {
                $result = Format-FrontMatterValue -Value $testCase.Input
                $result | Should -Be $testCase.Expected
            }
        }
    }
    
    Context "Arrays with Special Characters and Brackets" {
        It "Should handle arrays with bracket characters" {
            $inputArray = @("GitHub [Copilot]", "Azure [Functions]", "Visual Studio [Code]")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "GitHub [Copilot]"
  - "Azure [Functions]"
  - "Visual Studio [Code]"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle arrays with nested brackets" {
            $inputArray = @("Config [[nested]]", "Data [with [inner] brackets]")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "Config [[nested]]"
  - "Data [with [inner] brackets]"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle arrays with braces" {
            $inputArray = @("Config {settings}", "Object {properties}", "JSON {key: value}")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "Config {settings}"
  - "Object {properties}"
  - "JSON {key: value}"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle arrays with mixed bracket types" {
            $inputArray = @("Mixed [square] and {curly}", "Combined (parentheses) and [brackets]")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "Mixed [square] and {curly}"
  - "Combined (parentheses) and [brackets]"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle arrays with commas and special characters" {
            $inputArray = @("List, item with comma", "Another, item: with colon")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "List, item with comma"
  - "Another, item: with colon"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle arrays with backslashes and quotes" {
            $inputArray = @("C:\Program Files\", 'Quote: "Hello World"', "Path\\with\\backslashes")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "C:\Program Files\"
  - "Quote: `"Hello World`""
  - "Path\with\backslashes"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle arrays with YAML indicator characters" {
            $inputArray = @("Key: value", "List - item", "Question? mark", "Comment # here")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "Key: value"
  - "List - item"
  - "Question? mark"
  - "Comment # here"
"@
            $result | Should -Be $expected
        }
    }
    
    Context "Complex Real-World Scenarios" {
        It "Should handle programming languages and frameworks with brackets" {
            $inputArray = @("C#", "F#", "C++", "ASP.NET [Core]", "Entity Framework [Core]")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "C#"
  - "F#"
  - "C++"
  - "ASP.NET [Core]"
  - "Entity Framework [Core]"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle file paths with various separators" {
            $inputArray = @("/usr/bin/bash", "C:\Windows\System32", "\\network\share", "./relative/path")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "/usr/bin/bash"
  - "C:\Windows\System32"
  - "\network\share"
  - "./relative/path"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle URLs and URIs with special characters" {
            $inputArray = @("https://example.com/path?query=value&other=test", "mailto:user@domain.com", "ftp://user:pass@host.com")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "https://example.com/path?query=value&other=test"
  - "mailto:user@domain.com"
  - "ftp://user:pass@host.com"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle code snippets and technical content" {
            $inputArray = @(
                'function test() { return "Hello"; }',
                "SELECT * FROM table WHERE id = 'value'",
                "<div class=""container"">Content</div>"
            )
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "function test() { return `"Hello`"; }"
  - "SELECT * FROM table WHERE id = 'value'"
  - "Content"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle mathematical and scientific notation" {
            $inputArray = @("E=mc²", "Temperature: -40°C", "Probability: 0.95 (95%)")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "E=mc²"
  - "Temperature: -40°C"
  - "Probability: 0.95 (95%)"
"@
            $result | Should -Be $expected
        }
        
        It "Should handle version numbers and semantic versioning" {
            $inputArray = @("v1.0.0", "2.3.4-beta.1", "1.0.0-rc.1+build.123")
            $result = Format-FrontMatterValue -Value $inputArray
            $expected = @"

  - "v1.0.0"
  - "2.3.4-beta.1"
  - "1.0.0-rc.1+build.123"
"@
            $result | Should -Be $expected
        }
    }
    
    Context "YAML Compliance and Edge Cases" {
        It "Should produce valid YAML array syntax" {
            $result = Format-FrontMatterValue -Value @("item1", "item2")
            # Verify block sequence format (starts with newline, has hyphens)
            $result | Should -Match '^\n'
            $result | Should -Match '  - '
        }
        
        It "Should produce valid YAML string syntax" {
            $result = Format-FrontMatterValue -Value "test string"
            # Verify it's wrapped in double quotes
            $result | Should -Match '^".*"$'
        }
        
        It "Should handle multiple quote types safely" {
            $inputText = "Text with 'single' and `"double`" quotes"
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Text with ''single'' and \"double\" quotes"'
        }
        
        It "Should maintain character order and content integrity" {
            $originalText = "Important order: first, second, third!"
            $result = Format-FrontMatterValue -Value $originalText
            # Remove quotes and verify content is preserved (accounting for space normalization)
            $unquoted = $result.Substring(1, $result.Length - 2)
            $unquoted | Should -Be $originalText
        }
        
        It "Should handle extremely long strings without issues" {
            $longString = "A" * 2000
            $result = Format-FrontMatterValue -Value $longString
            $result | Should -Be "`"$longString`""
            $result.Length | Should -Be 2002  # Original length + 2 quotes
        }
        
        It "Should handle empty arrays correctly" {
            $result = Format-FrontMatterValue -Value @()
            $result | Should -Be '[]'
        }
    }
    
    Context "Edge Cases" {
        It "Should handle content with only HTML tags" {
            $result = Format-FrontMatterValue -Value "<div><p></p></div>"
            $result | Should -Be '""'
        }
        
        It "Should handle very long content" {
            $longContent = "Word " * 100 + "End"
            $result = Format-FrontMatterValue -Value $longContent
            $expected = '"' + (("Word " * 100 + "End").Trim()) + '"'
            $result | Should -Be $expected
        }
        
        It "Should handle content with special Unicode characters" {
            $inputText = "Content with émojis 🚀 and spëcial characters"
            $result = Format-FrontMatterValue -Value $inputText
            $result | Should -Be '"Content with émojis 🚀 and spëcial characters"'
        }
        
        It "Should handle mixed types in collection" {
            $inputCollection = @("String", 123, $true)
            $result = Format-FrontMatterValue -Value $inputCollection
            $expected = @"

  - "String"
  - "123"
  - "True"
"@
            $result | Should -Be $expected
        }
    }
} 