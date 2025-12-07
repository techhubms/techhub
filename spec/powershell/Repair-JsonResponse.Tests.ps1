# Repair-JsonResponse.Tests.ps1
# Tests for JSON response sanitization to handle AI model escape sequence issues

Describe "JSON Escape Sequence Issue Reproduction" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }

    Context "Reproducing the \s escape sequence error from workflow" {
        It "Should fail with the exact error when AI returns invalid \s escape sequence" {
            # This simulates what the AI model returned in the failing workflow
            # The AI wrote "keyboard\shortcuts" where \s is not a valid JSON escape
            # 
            # From the error log:
            # "Bad JSON escape sequence: \s. Path 'summary', line 4, position 472"
            #
            # Valid JSON escape sequences are: \" \\ \/ \b \f \n \r \t \uXXXX
            # The AI model incorrectly used \s (probably thinking it's regex whitespace)
            
            $aiResponseWithBadEscape = @'
{
  "skip_article": false,
  "section": "GitHub Copilot",
  "summary": "This practical guide focuses on actionable strategies and shortcuts for developers leveraging GitHub Copilot within Visual Studio Code.",
  "relevance_score": 85
}
'@
            # Inject the bad escape sequence that the AI generated
            # The AI wrote something like "keyboard\shortcuts" 
            $aiResponseWithBadEscape = $aiResponseWithBadEscape.Replace("shortcuts", "\shortcuts")

            # This demonstrates the EXACT error from the production workflow
            { $aiResponseWithBadEscape | ConvertFrom-Json } | Should -Throw "*Bad JSON escape sequence*"
        }

        It "Should fail with \d escape sequence (AI thinking regex digit)" {
            $jsonWithInvalidEscape = '{"text": "Match digits with \d pattern"}'
            { $jsonWithInvalidEscape | ConvertFrom-Json } | Should -Throw
        }

        It "Should fail with \w escape sequence (AI thinking regex word)" {
            $jsonWithInvalidEscape = '{"text": "Match words with \w pattern"}'
            { $jsonWithInvalidEscape | ConvertFrom-Json } | Should -Throw
        }
    }

    Context "Valid JSON escape sequences should still work" {
        It "Should correctly parse JSON with all valid escape sequences" {
            # These are the ONLY valid JSON escape sequences:
            # \" \\ \/ \b \f \n \r \t \uXXXX
            $validJson = @'
{
  "newline": "Line 1\nLine 2",
  "tab": "Col1\tCol2",
  "quote": "He said \"Hello\"",
  "backslash": "C:\\Users\\Name",
  "carriage_return": "Line1\rLine2",
  "unicode": "\u0041\u0042\u0043"
}
'@
            $result = $validJson | ConvertFrom-Json
            
            $result.newline | Should -Be "Line 1`nLine 2"
            $result.tab | Should -Be "Col1`tCol2"
            $result.quote | Should -Be 'He said "Hello"'
            $result.backslash | Should -Be "C:\Users\Name"
            $result.unicode | Should -Be "ABC"
        }
    }

    Context "Repair-JsonResponse function should fix invalid escapes" {
        It "Should have a Repair-JsonResponse function available" {
            # The iterative-roundup-generation.ps1 script needs this function
            # to sanitize AI responses before parsing with ConvertFrom-Json
            $functionExists = Get-Command -Name "Repair-JsonResponse" -ErrorAction SilentlyContinue
            
            $functionExists | Should -Not -BeNullOrEmpty -Because "AI models generate invalid JSON escape sequences that must be sanitized before parsing"
        }

        It "Should fix \s escape sequences" {
            $functionExists = Get-Command -Name "Repair-JsonResponse" -ErrorAction SilentlyContinue
            if (-not $functionExists) {
                Set-ItResult -Skipped -Because "Repair-JsonResponse function not yet implemented"
                return
            }

            $brokenJson = '{"summary": "keyboard\shortcuts for developers"}'
            $fixedJson = Repair-JsonResponse -JsonString $brokenJson
            
            { $fixedJson | ConvertFrom-Json } | Should -Not -Throw
        }

        It "Should fix \d, \w, \a, \e and other invalid single-letter escapes" {
            $functionExists = Get-Command -Name "Repair-JsonResponse" -ErrorAction SilentlyContinue
            if (-not $functionExists) {
                Set-ItResult -Skipped -Because "Repair-JsonResponse function not yet implemented"
                return
            }

            $brokenJson = '{"text": "regex \d digits \w words \a alert \e escape"}'
            $fixedJson = Repair-JsonResponse -JsonString $brokenJson
            
            { $fixedJson | ConvertFrom-Json } | Should -Not -Throw
        }

        It "Should preserve valid escape sequences while fixing invalid ones" {
            $functionExists = Get-Command -Name "Repair-JsonResponse" -ErrorAction SilentlyContinue
            if (-not $functionExists) {
                Set-ItResult -Skipped -Because "Repair-JsonResponse function not yet implemented"
                return
            }

            # Mix of valid (\n, \\) and invalid (\s) escapes
            $mixedJson = '{"path": "C:\\Users\\Name", "broken": "keyboard\shortcuts", "newline": "line1\nline2"}'
            
            $fixedJson = Repair-JsonResponse -JsonString $mixedJson
            $result = $fixedJson | ConvertFrom-Json
            
            # Valid escapes must be preserved exactly
            $result.path | Should -Be "C:\Users\Name"
            $result.newline | Should -Be "line1`nline2"
            # Invalid escape should be fixed (backslash escaped to \\)
            $result.broken | Should -Match "shortcuts"
        }
    }
}
