Describe "Test-AiResponseFormat" {
    BeforeAll {
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }

    Context "Valid Responses" {
        It "Should return IsValid true for valid JSON response" {
            # Arrange
            $response = '{"skip_article": false, "section": "AI", "summary": "Test summary", "relevance_score": 8}'

            # Act
            $result = Test-AiResponseFormat -Response $response -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $true
            $result.ErrorType | Should -BeNullOrEmpty
            $result.ErrorMessage | Should -BeNullOrEmpty
        }

        It "Should return IsValid true for valid skip_article response" {
            # Arrange
            $response = '{"skip_article": true, "reasoning": "Not relevant for developers"}'

            # Act
            $result = Test-AiResponseFormat -Response $response -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $true
        }
    }

    Context "Error JSON Detection" {
        It "Should detect compressed error JSON from ConvertTo-Json -Compress" {
            # Arrange - This is the exact format Invoke-AiApiCall produces
            $errorObj = [PSCustomObject]@{ Error = $true; Type = "ContentFilter"; Pattern = "content.*filter"; Message = "Content filter violation" } | ConvertTo-Json -Compress

            # Act
            $result = Test-AiResponseFormat -Response $errorObj -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorType | Should -Be "ContentFilter"
            $result.ErrorMessage | Should -BeLike "*ContentFilter*"
        }

        It "Should detect non-compressed error JSON with spaces" {
            # Arrange - Non-compressed JSON format
            $errorObj = [PSCustomObject]@{ Error = $true; Type = "RateLimit"; Message = "Rate limit exceeded" } | ConvertTo-Json

            # Act
            $result = Test-AiResponseFormat -Response $errorObj -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorType | Should -Be "RateLimit"
        }

        It "Should detect ContentFilter error with ResponseContent" {
            # Arrange - Full error object as produced by Invoke-AiApiCall for content filter
            $errorObj = [PSCustomObject]@{
                Error = $true
                Type = "ContentFilter"
                Pattern = "content.*filter"
                Message = "Content filter violation: content.*filter"
                ResponseContent = '{"error":{"code":"content_filter","message":"filtered"}}'
            } | ConvertTo-Json -Compress

            # Act
            $result = Test-AiResponseFormat -Response $errorObj -StepName "Step 2 (Filter and analysis)"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorType | Should -Be "ContentFilter"
            $result.ErrorMessage | Should -BeLike "*ContentFilter*"
        }

        It "Should detect RequestEntityTooLarge error" {
            # Arrange
            $errorObj = [PSCustomObject]@{ Error = $true; Type = "RequestEntityTooLarge"; Message = "Request entity too large" } | ConvertTo-Json -Compress

            # Act
            $result = Test-AiResponseFormat -Response $errorObj -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorType | Should -Be "RequestEntityTooLarge"
        }

        It "Should return UnparseableError for malformed error JSON" {
            # Arrange - Contains the error marker but is not valid JSON
            $response = 'Something "Error":true but not valid json {'

            # Act
            $result = Test-AiResponseFormat -Response $response -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorType | Should -Be "UnparseableError"
        }
    }

    Context "AI Refusal Detection" {
        It "Should detect 'I cannot' refusal" {
            # Arrange
            $response = "I cannot process this content because it contains inappropriate material."

            # Act
            $result = Test-AiResponseFormat -Response $response -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorMessage | Should -BeLike "*AI indicated it cannot complete*"
        }

        It "Should detect 'I'm unable' refusal" {
            # Arrange
            $response = "I'm unable to analyze this article due to content restrictions."

            # Act
            $result = Test-AiResponseFormat -Response $response -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorMessage | Should -BeLike "*AI indicated it cannot complete*"
        }

        It "Should detect 'I don't have' refusal" {
            # Arrange
            $response = "I don't have enough information to process this request."

            # Act
            $result = Test-AiResponseFormat -Response $response -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorMessage | Should -BeLike "*AI indicated it cannot complete*"
        }
    }

    Context "Empty and Short Responses" {
        It "Should reject empty response" {
            # Arrange & Act - Mandatory [string] parameter rejects empty strings
            { Test-AiResponseFormat -Response "" -StepName "Test Step" } | Should -Throw
        }

        It "Should reject whitespace-only response" {
            # Arrange & Act
            $result = Test-AiResponseFormat -Response "   " -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorType | Should -Be "EmptyResponse"
        }

        It "Should reject response shorter than 10 characters" {
            # Arrange & Act
            $result = Test-AiResponseFormat -Response "short" -StepName "Test Step"

            # Assert
            $result.IsValid | Should -Be $false
            $result.ErrorType | Should -Be "EmptyResponse"
        }
    }

    Context "Step Name in Messages" {
        It "Should include step name in error message for error JSON" {
            # Arrange
            $errorObj = [PSCustomObject]@{ Error = $true; Type = "ContentFilter"; Message = "test" } | ConvertTo-Json -Compress

            # Act
            $result = Test-AiResponseFormat -Response $errorObj -StepName "Step 2 (Filter article.md)"

            # Assert
            $result.ErrorMessage | Should -BeLike "*Step 2 (Filter article.md)*"
        }
    }
}
