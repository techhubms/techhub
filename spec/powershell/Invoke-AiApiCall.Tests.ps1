Describe "Invoke-AiApiCall" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
        
        # Test constants
        $script:TestToken = "test-token-12345"
        $script:TestModel = "gpt-4"
        $script:TestSystemMessage = "You are a helpful assistant."
        $script:TestUserMessage = "Hello, world!"
        $script:TestEndpoint = "https://models.github.ai/inference/chat/completions"
        $script:AzureEndpoint = "https://test.openai.azure.com/openai/deployments/gpt-4/chat/completions"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        
        # Reset mocks for each test
        Mock Get-WebResponseDetailsFromException { 
            return [PSCustomObject]@{
                StatusCode = $null
                StatusDescription = $null
                Headers = $null
                ResponseContent = $null
            }
        }
        
        Mock Get-SourceRoot { return "/workspaces/techhub" }
        Mock Out-File { }
        Mock Start-Sleep { }
    }
    
    Context "Parameter Validation" {
        It "Should have mandatory Token parameter" {
            (Get-Command Invoke-AiApiCall).Parameters['Token'].Attributes | Where-Object { $_.GetType().Name -eq 'ParameterAttribute' } | ForEach-Object { $_.Mandatory } | Should -Contain $true
        }
        
        It "Should have mandatory Model parameter" {
            (Get-Command Invoke-AiApiCall).Parameters['Model'].Attributes | Where-Object { $_.GetType().Name -eq 'ParameterAttribute' } | ForEach-Object { $_.Mandatory } | Should -Contain $true
        }
        
        It "Should have mandatory SystemMessage parameter" {
            (Get-Command Invoke-AiApiCall).Parameters['SystemMessage'].Attributes | Where-Object { $_.GetType().Name -eq 'ParameterAttribute' } | ForEach-Object { $_.Mandatory } | Should -Contain $true
        }
        
        It "Should have mandatory UserMessage parameter" {
            (Get-Command Invoke-AiApiCall).Parameters['UserMessage'].Attributes | Where-Object { $_.GetType().Name -eq 'ParameterAttribute' } | ForEach-Object { $_.Mandatory } | Should -Contain $true
        }
        
        It "Should use default values for optional parameters" {
            Mock Invoke-WebRequest { 
                return [PSCustomObject]@{
                    Content = '{"choices":[{"message":{"content":"Test response"}}]}'
                }
            }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage
            
            $result | Should -Be "Test response"
        }
    }
    
    Context "Authentication and Headers" {
        It "Should use Bearer token for GitHub Models endpoint" {
            Mock Invoke-WebRequest { 
                param($Headers)
                $Headers["Authorization"] | Should -Be "Bearer $TestToken"
                return [PSCustomObject]@{
                    Content = '{"choices":[{"message":{"content":"Success"}}]}'
                }
            }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage
            
            $result | Should -Be "Success"
        }
        
        It "Should use api-key for Azure endpoints" {
            Mock Invoke-WebRequest { 
                param($Headers, $Uri)
                $Headers["api-key"] | Should -Be $TestToken
                $Uri | Should -Match "api-version="
                return [PSCustomObject]@{
                    Content = '{"choices":[{"message":{"content":"Azure success"}}]}'
                }
            }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage -Endpoint $AzureEndpoint
            
            $result | Should -Be "Azure success"
        }
        
        It "Should add API version to Azure endpoint if missing" {
            Mock Invoke-WebRequest { 
                param($Uri)
                $Uri | Should -Match "api-version=2024-05-01-preview"
                return [PSCustomObject]@{
                    Content = '{"choices":[{"message":{"content":"Azure with version"}}]}'
                }
            }
            
            Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage -Endpoint $AzureEndpoint
        }
        
        It "Should throw for unsupported endpoints" {
            { Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage -Endpoint "https://unsupported.com/api" } | Should -Throw "*Unsupported endpoint*"
        }
    }
    
    Context "Successful API Calls" {
        It "Should return content from successful response" {
            Mock Invoke-WebRequest { 
                return [PSCustomObject]@{
                    Content = '{"choices":[{"message":{"content":"Hello from AI!"}}]}'
                }
            }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage
            
            $result | Should -Be "Hello from AI!"
        }
        
        It "Should build correct request body" {
            Mock Invoke-WebRequest { 
                param($Body)
                $bodyObject = $Body | ConvertFrom-Json
                $bodyObject.model | Should -Be $TestModel
                $bodyObject.messages.Count | Should -Be 2
                $bodyObject.messages[0].role | Should -Be "system"
                $bodyObject.messages[0].content | Should -Be $TestSystemMessage
                $bodyObject.messages[1].role | Should -Be "user"
                $bodyObject.messages[1].content | Should -Be $TestUserMessage
                
                return [PSCustomObject]@{
                    Content = '{"choices":[{"message":{"content":"Validated"}}]}'
                }
            }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage
            
            $result | Should -Be "Validated"
        }
    }
    
    Context "Error Handling" {
        It "Should return RequestEntityTooLarge error for 413 status" {
            Mock Get-WebResponseDetailsFromException { 
                return [PSCustomObject]@{
                    StatusCode = 413
                    StatusDescription = 'Request Entity Too Large'
                    Headers = @{}
                    ResponseContent = $null
                }
            }
            
            Mock Invoke-WebRequest { throw "Request too large" }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage
            
            $errorObject = $result | ConvertFrom-Json
            $errorObject.Error | Should -Be $true
            $errorObject.Type | Should -Be "RequestEntityTooLarge"
        }
        
        It "Should return RateLimit error and save rate limit file" {
            Mock Get-WebResponseDetailsFromException { 
                return [PSCustomObject]@{
                    StatusCode = 429
                    StatusDescription = 'Too Many Requests'
                    Headers = @{
                        "x-ratelimit-timeremaining" = "300"
                    }
                    ResponseContent = $null
                }
            }
            
            Mock Invoke-WebRequest { throw "Rate limited" }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage
            
            $errorObject = $result | ConvertFrom-Json
            $errorObject.Error | Should -Be $true
            $errorObject.Type | Should -Be "RateLimit"
            $errorObject.RateLimitSeconds | Should -Be "300"
            
            # Verify rate limit file was created
            Should -Invoke Out-File -ParameterFilter { $FilePath -like "*rate-limit-enddate.json" }
        }
        
        It "Should return ContentFilter error for content policy violations" {
            Mock Get-WebResponseDetailsFromException { 
                return [PSCustomObject]@{
                    StatusCode = 400
                    StatusDescription = 'Bad Request'
                    Headers = @{}
                    ResponseContent = '{"error": "content filter blocked this request"}'
                }
            }
            
            Mock Invoke-WebRequest { throw "Content filtered" }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage
            
            $errorObject = $result | ConvertFrom-Json
            $errorObject.Error | Should -Be $true
            $errorObject.Type | Should -Be "ContentFilter"
        }
        
        It "Should return ResponseParseError for invalid JSON response" {
            Mock Invoke-WebRequest { 
                return [PSCustomObject]@{
                    Content = 'invalid json response'
                }
            }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage
            
            $errorObject = $result | ConvertFrom-Json
            $errorObject.Error | Should -Be $true
            $errorObject.Type | Should -Be "ResponseParseError"
            $errorObject.ResponseContent | Should -Be 'invalid json response'
        }
    }
    
    Context "Retry Logic" {
        It "Should retry on failure with exponential backoff" {
            $script:callCount = 0
            Mock Invoke-WebRequest { 
                $script:callCount++
                if ($script:callCount -le 2) {
                    throw "Temporary failure"
                }
                return [PSCustomObject]@{
                    Content = '{"choices":[{"message":{"content":"Success after retries"}}]}'
                }
            }
            
            Mock Start-Sleep { param($Seconds) }
            
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage
            
            $result | Should -Be "Success after retries"
            $script:callCount | Should -Be 3
            
            # Verify exponential backoff delays were called
            Should -Invoke Start-Sleep -Times 5 # 2 failures + rate limit prevention sleeps
        }
        
        It "Should respect MaxRetries parameter" {
            Mock Invoke-WebRequest { throw "Always fails" }
            
            { Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage -MaxRetries 1 } | Should -Throw
        }
    }
    
    Context "WhatIf Support" {
        It "Should return dummy response in WhatIf mode" {
            $result = Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage -WhatIf
            
            $result | Should -Match "Sample Article Title"
            
            # Should not make actual HTTP calls
            Should -Not -Invoke Invoke-WebRequest
        }
        
        It "Should display WhatIf information" {
            Mock Write-Host { }
            
            Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage -WhatIf
            
            Should -Invoke Write-Host -ParameterFilter { $Object -like "*What if*" }
            Should -Invoke Write-Host -ParameterFilter { $Object -like "*Model: $TestModel*" }
        }
    }
    
    Context "Rate Limit Prevention" {
        It "Should wait after successful API call" {
            Mock Invoke-WebRequest { 
                return [PSCustomObject]@{
                    Content = '{"choices":[{"message":{"content":"Success"}}]}'
                }
            }
            
            Mock Start-Sleep { param($Seconds) }
            
            Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage -RateLimitPreventionDelay 10
            
            Should -Invoke Start-Sleep -ParameterFilter { $Seconds -eq 10 }
        }
        
        It "Should wait after failed API call" {
            Mock Invoke-WebRequest { throw "API failure" }
            Mock Start-Sleep { param($Seconds) }
            
            { Invoke-AiApiCall -Token $TestToken -Model $TestModel -SystemMessage $TestSystemMessage -UserMessage $TestUserMessage -MaxRetries 0 -RateLimitPreventionDelay 5 } | Should -Throw
            
            Should -Invoke Start-Sleep -ParameterFilter { $Seconds -eq 5 }
        }
    }
}
