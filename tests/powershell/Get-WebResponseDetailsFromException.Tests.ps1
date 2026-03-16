Describe "Get-WebResponseDetailsFromException" {
    BeforeAll {
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }
    
    Context "Parameter Validation" {
        It "Should throw when Exception parameter is null" {
            { Get-WebResponseDetailsFromException -Exception $null } | Should -Throw "*Cannot validate argument*"
        }
    }
    
    Context "HttpWebResponse Processing" {
        It "Should extract status code from HttpWebResponse" {
            # Arrange - Create a mock HttpWebResponse
            $mockResponse = [PSCustomObject]@{
                StatusCode = 429
                StatusDescription = "Too Many Requests"
                Headers = @{
                    "retry-after" = "300"
                    "x-ratelimit-remaining" = "0"
                }
                PSTypeName = "System.Net.HttpWebResponse"
            }
            
            $mockException = [PSCustomObject]@{
                Response = $mockResponse
                PSTypeName = "System.Net.WebException"
            }
            
            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException
            
            # Assert
            $result.StatusCode | Should -Be 429
            $result.StatusDescription | Should -Be "Too Many Requests"
            $result.Headers | Should -Not -BeNullOrEmpty
        }
        
        It "Should handle missing response in exception" {
            # Arrange
            $mockException = [PSCustomObject]@{
                Response = $null
                Message = "Network error occurred"
                PSTypeName = "System.Net.WebException"
            }
            
            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException
            
            # Assert
            $result.StatusCode | Should -BeNullOrEmpty
            $result.StatusDescription | Should -BeNullOrEmpty
            $result.Headers | Should -BeNullOrEmpty
            $result.ResponseContent | Should -BeNullOrEmpty
        }
    }
    
    Context "HttpRequestException Processing" {
        It "Should extract status code from HttpRequestException message" {
            # Arrange
            $mockException = [PSCustomObject]@{
                Message = "Response status code does not indicate success: 401 (Unauthorized)."
                PSTypeName = "System.Net.Http.HttpRequestException"
            }
            
            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException
            
            # Assert
            $result.StatusCode | Should -Be 401
            $result.StatusDescription | Should -Be "Unauthorized"
        }
        
        It "Should handle HttpRequestException without status code" {
            # Arrange
            $mockException = [PSCustomObject]@{
                Message = "Network connection failed"
                PSTypeName = "System.Net.Http.HttpRequestException"
            }
            
            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException
            
            # Assert
            $result.StatusCode | Should -BeNullOrEmpty
            $result.StatusDescription | Should -BeNullOrEmpty
        }
    }
    
    Context "Response Content Extraction" {
        It "Should extract content from stream response" {
            # Arrange
            $mockStream = [PSCustomObject]@{}
            $mockStream | Add-Member -MemberType ScriptMethod -Name "ReadToEnd" -Value {
                return '{"error": "rate limit exceeded"}'
            }
            
            $mockResponse = [PSCustomObject]@{
                StatusCode = 429
                StatusDescription = "Too Many Requests"
                Headers = @{}
                PSTypeName = "System.Net.HttpWebResponse"
            }
            
            # Add the GetResponseStream method using Add-Member
            $mockResponse | Add-Member -MemberType ScriptMethod -Name "GetResponseStream" -Value {
                return $mockStream
            }
            
            $mockException = [PSCustomObject]@{
                Response = $mockResponse
                PSTypeName = "System.Net.WebException"
            }
            
            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException
            
            # Assert
            $result.ResponseContent | Should -Be '{"error": "rate limit exceeded"}'
        }
        
        It "Should handle errors during content extraction" {
            # Arrange
            $mockResponse = [PSCustomObject]@{
                StatusCode = 500
                StatusDescription = "Internal Server Error"
                Headers = @{}
                GetResponseStream = { throw "Stream read error" }
                PSTypeName = "System.Net.HttpWebResponse"
            }
            
            $mockException = [PSCustomObject]@{
                Response = $mockResponse
                PSTypeName = "System.Net.WebException"
            }
            
            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException
            
            # Assert
            $result.StatusCode | Should -Be 500
            $result.StatusDescription | Should -Be "Internal Server Error"
            $result.ResponseContent | Should -BeNullOrEmpty
        }
    }
    
    Context "Edge Cases" {
        It "Should handle exceptions without expected properties" {
            # Arrange
            $mockException = [PSCustomObject]@{
                Message = "Unknown error type"
                PSTypeName = "System.Exception"
            }
            
            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException
            
            # Assert
            $result.StatusCode | Should -BeNullOrEmpty
            $result.StatusDescription | Should -BeNullOrEmpty
            $result.Headers | Should -BeNullOrEmpty
            $result.ResponseContent | Should -BeNullOrEmpty
        }
        
        It "Should handle null response stream" {
            # Arrange
            $mockResponse = [PSCustomObject]@{
                StatusCode = 404
                StatusDescription = "Not Found"
                Headers = @{}
                GetResponseStream = { return $null }
                PSTypeName = "System.Net.HttpWebResponse"
            }
            
            $mockException = [PSCustomObject]@{
                Response = $mockResponse
                PSTypeName = "System.Net.WebException"
            }
            
            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException
            
            # Assert
            $result.StatusCode | Should -Be 404
            $result.ResponseContent | Should -BeNullOrEmpty
        }
    }

    Context "ErrorRecord Fallback for Response Content" {
        It "Should extract response content from ErrorRecord.ErrorDetails.Message when content extraction fails" {
            # Arrange - Simulates PowerShell 7 Invoke-WebRequest failure where
            # the response body is available via ErrorRecord.ErrorDetails.Message
            # but NOT via the exception's Response.Content (stream already consumed)
            $mockResponse = [PSCustomObject]@{
                StatusCode = 400
                StatusDescription = "Bad Request"
                Headers = @{}
                Content = "System.Net.Http.HttpConnectionResponseContent"
                PSTypeName = "System.Net.Http.HttpResponseMessage"
            }

            $mockException = [PSCustomObject]@{
                Response = $mockResponse
                Message = "Response status code does not indicate success: 400 (Bad Request)."
                PSTypeName = "Microsoft.PowerShell.Commands.HttpResponseException"
            }

            $mockErrorDetails = [PSCustomObject]@{
                Message = '{"error":{"message":"The request was invalid.","type":"invalid_request_error","code":"invalid_request"}}'
            }

            $mockErrorRecord = [PSCustomObject]@{
                Exception = $mockException
                ErrorDetails = $mockErrorDetails
            }

            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException -ErrorRecord $mockErrorRecord

            # Assert
            $result.StatusCode | Should -Be 400
            $result.ResponseContent | Should -Be '{"error":{"message":"The request was invalid.","type":"invalid_request_error","code":"invalid_request"}}'
        }

        It "Should not override valid response content with ErrorRecord fallback" {
            # Arrange - Response.Content has real content, ErrorRecord also has content
            $mockResponse = [PSCustomObject]@{
                StatusCode = 400
                StatusDescription = "Bad Request"
                Headers = @{}
                Content = '{"error":"from response content"}'
                PSTypeName = "System.Net.Http.HttpResponseMessage"
            }

            $mockException = [PSCustomObject]@{
                Response = $mockResponse
                Message = "Response status code does not indicate success: 400 (Bad Request)."
                PSTypeName = "Microsoft.PowerShell.Commands.HttpResponseException"
            }

            $mockErrorDetails = [PSCustomObject]@{
                Message = '{"error":"from error details"}'
            }

            $mockErrorRecord = [PSCustomObject]@{
                Exception = $mockException
                ErrorDetails = $mockErrorDetails
            }

            # Act
            $result = Get-WebResponseDetailsFromException -Exception $mockException -ErrorRecord $mockErrorRecord

            # Assert - Should use the content from the response, not the ErrorRecord fallback
            $result.ResponseContent | Should -Be '{"error":"from response content"}'
        }

        It "Should work without ErrorRecord parameter (backward compatible)" {
            # Arrange
            $mockException = [PSCustomObject]@{
                Message = "Response status code does not indicate success: 400 (Bad Request)."
                PSTypeName = "Microsoft.PowerShell.Commands.HttpResponseException"
            }

            # Act - No ErrorRecord parameter passed
            $result = Get-WebResponseDetailsFromException -Exception $mockException

            # Assert
            $result.StatusCode | Should -Be 400
            $result.StatusDescription | Should -Be "Bad Request"
            $result.ResponseContent | Should -BeNullOrEmpty
        }

        It "Should use ErrorRecord fallback when content is a .NET type name string" {
            # Arrange - This is the exact bug scenario: Content.ToString() returns the type name
            $mockResponse = [PSCustomObject]@{
                StatusCode = 400
                StatusDescription = "Bad Request"
                Headers = @{}
                Content = "System.Net.Http.HttpConnectionResponseContent"
                PSTypeName = "System.Net.Http.HttpResponseMessage"
            }

            $mockException = [PSCustomObject]@{
                Response = $mockResponse
                Message = "Response status code does not indicate success: 400 (Bad Request)."
                PSTypeName = "Microsoft.PowerShell.Commands.HttpResponseException"
            }

            $mockErrorRecord = [PSCustomObject]@{
                Exception = $mockException
                ErrorDetails = $null
            }

            # Act - ErrorRecord has no ErrorDetails
            $result = Get-WebResponseDetailsFromException -Exception $mockException -ErrorRecord $mockErrorRecord

            # Assert - Should detect the type name string is not useful content
            $result.ResponseContent | Should -BeNullOrEmpty
        }
    }
}
