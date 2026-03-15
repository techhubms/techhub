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
}
