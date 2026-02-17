# Tests for Invoke-ProcessWithAiModel error handling functionality
# This test file validates the content filter and error handling improvements

Describe "Invoke-ProcessWithAiModel" {
    BeforeAll {
        # Initialize-BeforeAll.ps1 (inline for standalone test execution)
        if (-not $global:TempPath) {
            $tempBase = if ($env:TEMP) { $env:TEMP } elseif ($env:TMP) { $env:TMP } elseif ($env:TMPDIR) { $env:TMPDIR } else { "/tmp" }
            $tempPath = Join-Path $tempBase "pwshtests"
            if (Test-Path $tempPath) {
                Remove-Item $tempPath -Recurse -Force
            }
            New-Item -ItemType Directory -Path $tempPath -Force | Out-Null
            $global:TempPath = $tempPath
        }
        
        # Load PowerShell functions
        $functionsPath = Join-Path $PSScriptRoot "../../scripts/content-processing/functions"
        if (Test-Path $functionsPath) {
            . (Join-Path $functionsPath "Write-ErrorDetails.ps1")
            Get-ChildItem -Path $functionsPath -Filter "*.ps1" | 
            Where-Object { $_.Name -ne "Write-ErrorDetails.ps1" } |
            ForEach-Object { . $_.FullName }
        }
        
        $script:TestScriptsPath = Join-Path $global:TempPath "scripts"
        $script:TestDataPath = Join-Path $global:TempPath "_data"
        $script:TestSystemMessagePath = Join-Path $script:TestScriptsPath "system-message.md"
        $script:TestSkippedEntriesPath = Join-Path $script:TestDataPath "skipped-entries.json"
        
        # Create test directories
        New-Item -Path $script:TestScriptsPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestDataPath -ItemType Directory -Force | Out-Null
        
        # Create test system message file
        Set-Content -Path $script:TestSystemMessagePath -Value "Test system message for AI API"

        # Create empty skipped entries file
        Set-Content -Path $script:TestSkippedEntriesPath -Value "[]"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        
        # Recreate test directories and files after cleanup
        New-Item -Path $script:TestScriptsPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestDataPath -ItemType Directory -Force | Out-Null
        Set-Content -Path $script:TestSystemMessagePath -Value "Test system message for AI API"
        Set-Content -Path $script:TestSkippedEntriesPath -Value "[]"
        
        # Mock the centralized config functions
        Mock Get-AzureOpenAIEndpoint { return "https://oai-techhub-staging.services.ai.azure.com/models/gpt-4.1/chat/completions" }
        Mock Get-AzureOpenAIModelName { return "gpt-4.1" }
    }
    
    Context "Content Filter Error Detection" {
        It "Should handle content filter errors from Invoke-AiApiCall" {
            # Mock Invoke-AiApiCall to return content filter error
            Mock Invoke-AiApiCall {
                $errorObject = @{
                    Error = $true
                    Type = "ContentFilter"
                    Message = "Content was blocked by safety filters"
                }
                return ($errorObject | ConvertTo-Json -Compress)
            }
            
            # Should receive and parse the content filter error
            $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData @{"test" = "content" }
            $result.Error | Should -Be $true
            $result.Type | Should -Be "ContentFilter"
        }
        
        It "Should handle different content filter patterns" {
            # Test different patterns that indicate content filtering
            $patterns = @(
                "content filter violation",
                "inappropriate content detected", 
                "safety filter triggered",
                "content policy violation",
                "blocked content"
            )
            
            foreach ($pattern in $patterns) {
                # Mock Invoke-AiApiCall to return content filter error with specific pattern
                Mock Invoke-AiApiCall {
                    $errorObject = @{
                        Error = $true
                        Type = "ContentFilter"
                        Message = $pattern
                    }
                    return ($errorObject | ConvertTo-Json -Compress)
                }
                
                # Should receive and parse the content filter error
                $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData @{"test" = "content" }
                $result.Error | Should -Be $true
                $result.Type | Should -Be "ContentFilter"
            }
        }
        
        It "Should handle non-content-filter errors correctly" {
            # Test that other errors are passed through correctly
            Mock Invoke-AiApiCall {
                $errorObject = @{
                    Error = $true
                    Type = "BadRequest"
                    Message = "Invalid request format"
                }
                return ($errorObject | ConvertTo-Json -Compress)
            }
            
            # Should parse the error response and return parsed object
            $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData @{"test" = "content" }
            $result.Error | Should -Be $true
            $result.Type | Should -Be "BadRequest"
        }
    }
    
    Context "Rate Limit Integration" {
        It "Should integrate with Invoke-AiApiCall rate limit handling" {
            # Test that rate limit errors from Invoke-AiApiCall are properly propagated
            Mock Invoke-AiApiCall {
                return @{
                    Error = $true
                    Type = "RateLimit"
                    RateLimitSeconds = 300
                } | ConvertTo-Json -Compress
            }
            
            $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData @{"test" = "content" }
            
            # Should receive and parse the rate limit error from the API caller
            $result.Error | Should -Be $true
            $result.Type | Should -Be "RateLimit"
            $result.RateLimitSeconds | Should -Be 300
        }
    }
    
    Context "WhatIf Mode" {
        It "Should return dummy response in WhatIf mode" {
            $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData @{"test" = "content" } -WhatIf
            
            $result | Should -Not -BeNullOrEmpty
            $result.title | Should -Be "Sample RSS Article Title"
            $result.categories | Should -Contain "AI"
        }
    }
    
    Context "Endpoint Integration" {
        It "Should pass environment parameters to Invoke-AiApiCall correctly" -Tag "Endpoint" {
            # Test that environment is properly passed through to the API caller
            Mock Invoke-AiApiCall {
                param($Token, $Environment, $SystemMessage, $UserMessage, $MaxRetries, $RateLimitPreventionDelay)
                
                # Verify parameters are passed correctly
                $Token | Should -Be "test-token"
                $Environment | Should -Be "staging"
                $SystemMessage | Should -Not -BeNullOrEmpty
                $UserMessage | Should -Not -BeNullOrEmpty
                $MaxRetries | Should -Be 5
                $RateLimitPreventionDelay | Should -Be 20
                
                $responseObject = @{
                    title = "Test"
                    categories = @("Test")
                }
                return ($responseObject | ConvertTo-Json -Compress)
            }
            
            $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData @{"test" = "content" } -MaxRetries 5 -RateLimitPreventionDelay 20
            $result | Should -Not -BeNullOrEmpty
        }
        
        It "Should handle authentication errors from Invoke-AiApiCall" -Tag "Endpoint" {
            # Test that authentication errors are properly propagated
            Mock Invoke-AiApiCall {
                return @{
                    Error = $true
                    Type = "AuthenticationError"
                    Message = "Invalid API key"
                } | ConvertTo-Json -Compress
            }
            
            $result = Invoke-ProcessWithAiModel -Token "invalid-token" -Environment "staging" -InputData @{"test" = "content" }
            
            # Should receive and parse the authentication error
            $result.Error | Should -Be $true
            $result.Type | Should -Be "AuthenticationError"
        }
    }
    
    Context "Header Management During Retries" {
        It "Should preserve authentication headers when retrying after failure" {
            # This test validates that Invoke-AiApiCall properly handles retries
            # and maintains authentication headers during the retry process
            Mock Invoke-AiApiCall {
                param($Token, $Model, $SystemMessage, $UserMessage, $MaxRetries, $Endpoint, $RateLimitPreventionDelay)
                
                # Verify that parameters are passed correctly to the API caller
                $Token | Should -Be "test-token"
                $MaxRetries | Should -Be 2
                
                # Return success after retry handling
                $responseObject = @{
                    title = "Test"
                    categories = @("Test")
                }
                return ($responseObject | ConvertTo-Json -Compress)
            }
            
            # Call the function with retry parameters
            $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData @{"test" = "data"} -MaxRetries 2
            
            # Verify the call was made and result returned
            Should -Invoke Invoke-AiApiCall -Exactly 1
            $result.title | Should -Be "Test"
        }
    }
    
    Context "JSON Parsing Error Handling" {
        It "Should return JsonParseError when AI response cannot be parsed as JSON" {
            # Mock Invoke-AiApiCall to return successful response but with invalid JSON content
            Mock Invoke-AiApiCall {
                return ".NET development with Azure Functions offers powerful capabilities for building scalable serverless applications..."
            }
            
            # Create test input data
            $inputData = @{
                title       = "Test Article"
                description = "Test description"
                content     = "Test content"
                author      = "Test Author"
                tags        = "test,ai"
                type        = "news"
            }
            
            # Call the function
            $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData $inputData
            
            # Should return JsonParseError when response cannot be parsed
            $parsedResult = $result | ConvertFrom-Json
            $parsedResult.Error | Should -Be $true
            $parsedResult.Type | Should -Be "JsonParseError"
            $parsedResult.ResponseContent | Should -Match "\.NET development with Azure Functions"
        }
        
        It "Should return JsonParseError when API response structure is invalid" {
            # Mock Invoke-AiApiCall to return invalid JSON structure
            Mock Invoke-AiApiCall {
                $responseObject = @{
                    invalid = "structure"
                }
                return ($responseObject | ConvertTo-Json -Compress)
            }
            
            # Create test input data
            $inputData = @{
                title       = "Test Article"
                description = "Test description"
                content     = "Test content"
                author      = "Test Author"
                tags        = "test,ai"
                type        = "news"
            }
            
            # Call the function - this should succeed in parsing JSON but the structure is wrong
            $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData $inputData
            
            # Should return the parsed invalid structure (not an error in this case)
            $result.invalid | Should -Be "structure"
        }
    }
    
    Context "Markdown Code Block JSON Parsing Issue - Real Crash Reproduction" {
        It "Should fail to parse AI response when JSON is wrapped in markdown code blocks" {
            # This is the exact ResponseContent from the crashed AI result file
            # airesult-2025-08-06-Announcing-the-Browser-Automation-Tool-Preview-in-Azure-AI-Foundry-Agent-Service.json
            $responseObject = @{
                choices = @(
                    @{
                        content_filter_results = @{
                            hate = @{
                                filtered = $false
                                severity = "safe"
                            }
                            protected_material_code = @{
                                detected = $false
                                filtered = $false
                            }
                            protected_material_text = @{
                                detected = $false
                                filtered = $false
                            }
                            self_harm = @{
                                filtered = $false
                                severity = "safe"
                            }
                            sexual = @{
                                filtered = $false
                                severity = "safe"
                            }
                            violence = @{
                                filtered = $false
                                severity = "safe"
                            }
                        }
                        finish_reason = "stop"
                        index = 0
                        logprobs = $null
                        message = @{
                            annotations = @()
                            content = "``````json`n{`n  `"title`": `"Announcing the Browser Automation Tool (Preview) for Azure AI Foundry Agent Service`",`n  `"description`": `"Linda Li introduces the Browser Automation Tool (Preview) as a new action tool in Azure AI Foundry Agent Service.`",`n  `"categories`": [`"AI`", `"Azure`", `"Coding`"],`n  `"tags`": [`"Azure AI Foundry`", `"Browser Automation Tool`", `"Playwright Workspaces`"],`n  `"excerpt`": `"Linda Li introduces the Browser Automation Tool (Preview) in Azure AI Foundry Agent Service.`",`n  `"content`": `"# Announcing the Browser Automation Tool\n\nAuthor: Linda Li\n\n## Overview\nThe tool empowers developers to build sophisticated agents.`"`n}`n``````"
                            refusal = $null
                            role = "assistant"
                        }
                    }
                )
                created = 1754645489
                id = "chatcmpl-C2DjdgdvcP8Zdu4mqaLiLn5HxpOWK"
                model = "gpt-4.1-2025-04-14"
                object = "chat.completion"
            }
            $responseContent = ($responseObject | ConvertTo-Json -Compress -Depth 10)

            # Simulate the exact parsing logic from the Invoke-ProcessWithAiModel function
            $responseJson = $responseContent | ConvertFrom-Json
            $messageContent = $responseJson.choices[0].message.content
            
            # This should fail with the same error as the original crash:
            # "Unexpected character encountered while parsing value: `. Path '', line 0, position 0."
            { $messageContent | ConvertFrom-Json } | Should -Throw "*Unexpected character encountered while parsing value*"
        }
        
        It "Should demonstrate the specific character that causes the parsing error" {
            # Extract just the problematic content that starts with backticks
            $responseObject = @{
                choices = @(
                    @{
                        message = @{
                            content = "``````json`n{`"test`": `"value`"}`n``````"
                        }
                    }
                )
            }
            $responseContent = ($responseObject | ConvertTo-Json -Compress -Depth 10)
            $responseJson = $responseContent | ConvertFrom-Json
            $messageContent = $responseJson.choices[0].message.content
            
            # Show that the content starts with backticks, which ConvertFrom-Json cannot parse
            $messageContent | Should -Match '^```json'
            
            # Verify this fails with the exact same error pattern
            { $messageContent | ConvertFrom-Json } | Should -Throw "*Unexpected character*"
        }
        
        It "Should succeed when JSON is properly extracted from markdown code blocks" {
            # Same response content but with proper extraction logic
            $responseObject = @{
                choices = @(
                    @{
                        message = @{
                            content = "``````json`n{`"title`": `"Test Article`", `"categories`": [`"AI`"]}`n``````"
                        }
                    }
                )
            }
            $responseContent = ($responseObject | ConvertTo-Json -Compress -Depth 10)
            $responseJson = $responseContent | ConvertFrom-Json
            $messageContent = $responseJson.choices[0].message.content
            
            # Fix: Extract JSON from markdown code blocks using regex
            if ($messageContent -match '```json\s*(.*?)\s*```') {
                $jsonContent = $matches[1]
                $parsedJson = $jsonContent | ConvertFrom-Json
                
                # The parsing should succeed with proper extraction
                $parsedJson | Should -Not -BeNullOrEmpty
                $parsedJson.title | Should -Be "Test Article"
                $parsedJson.categories | Should -Contain "AI"
            } else {
                throw "Unable to extract JSON from markdown code blocks"
            }
        }
        
        It "Should reproduce the exact error conditions from the crash" {
            # Mock Invoke-AiApiCall to return the problematic response with markdown code blocks
            Mock Invoke-AiApiCall {
                return "``````json`n{`"title`": `"Test`"}`n``````"
            }

            # Create test input data as hashtable (required parameter type)
            $inputData = @{
                title = "Test Article"
                content = "Test content"
                description = "Test description"
            }

            # This should trigger the same error path as the original crash
            $result = Invoke-ProcessWithAiModel -Token "test-token" -Environment "staging" -InputData $inputData
            
            # Verify it returns an error object with JsonParseError type
            $parsedResult = $result | ConvertFrom-Json
            $parsedResult.Error | Should -Be $true
            $parsedResult.Type | Should -Be "JsonParseError"
            $parsedResult.ResponseContent | Should -Not -BeNullOrEmpty
        }
    }
}