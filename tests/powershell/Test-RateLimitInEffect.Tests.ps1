# Test-RateLimitInEffect.Tests.ps1
# Tests for the Test-RateLimitInEffect PowerShell function

Describe "Test-RateLimitInEffect" {
    BeforeAll {
        $script:TestScriptsPath = Join-Path $global:TempPath "scripts"
    }
    
    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        
        # Recreate test directories after cleanup
        New-Item -Path $script:TestScriptsPath -ItemType Directory -Force | Out-Null

        Mock Get-SourceRoot { return $global:TempPath } 
    }
    
    Context "No Rate Limit File" {
        It "Should return false when no rate limit file exists" {
            $result = Test-RateLimitInEffect
            $result | Should -Be $false
        }
    }
    
    Context "Expired Rate Limit" {
        It "Should remove expired rate limit file and return false" {
            # Create expired rate limit file (1 hour ago)
            $pastTime = (Get-Date).AddHours(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            $rateLimitData = @{ endDate = $pastTime } | ConvertTo-Json
            $rateLimitFile = Join-Path $script:TestScriptsPath "rate-limit-enddate.json"
            $rateLimitData | Out-File -FilePath $rateLimitFile -Encoding utf8
            
            $result = Test-RateLimitInEffect
            
            $result | Should -Be $false
            Test-Path $rateLimitFile | Should -Be $false
        }
    }
    
    Context "Active Rate Limit - Short Duration (≤60 seconds)" {
        It "Should wait 60 seconds and remove file for 30-second rate limit" {
            # Create rate limit file with 30 seconds from now
            $futureTime = (Get-Date).AddSeconds(30).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            $rateLimitData = @{ endDate = $futureTime } | ConvertTo-Json
            $rateLimitFile = Join-Path $script:TestScriptsPath "rate-limit-enddate.json"
            $rateLimitData | Out-File -FilePath $rateLimitFile -Encoding utf8
            
            $startTime = Get-Date
            
            # Mock Start-Sleep to avoid actually waiting in tests
            Mock Start-Sleep { 
                param($Seconds)
                # Verify it's trying to sleep for 60 seconds
                $Seconds | Should -Be 60
            }
            
            $result = Test-RateLimitInEffect
            
            $result | Should -Be $false
            Test-Path $rateLimitFile | Should -Be $false
            
            # Verify Start-Sleep was called
            Should -Invoke Start-Sleep -Exactly 1 -ParameterFilter { $Seconds -eq 60 }
        }
        
        It "Should wait 60 seconds and remove file for exactly 60-second rate limit" {
            # Create rate limit file with exactly 60 seconds from now
            $futureTime = (Get-Date).AddSeconds(60).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            $rateLimitData = @{ endDate = $futureTime } | ConvertTo-Json
            $rateLimitFile = Join-Path $script:TestScriptsPath "rate-limit-enddate.json"
            $rateLimitData | Out-File -FilePath $rateLimitFile -Encoding utf8
            
            # Mock Start-Sleep
            Mock Start-Sleep { 
                param($Seconds)
                $Seconds | Should -Be 60
            }
            
            $result = Test-RateLimitInEffect
            
            $result | Should -Be $false
            Test-Path $rateLimitFile | Should -Be $false
            
            # Verify Start-Sleep was called
            Should -Invoke Start-Sleep -Exactly 1 -ParameterFilter { $Seconds -eq 60 }
        }
    }
    
    Context "Active Rate Limit - Long Duration (>60 seconds)" {
        It "Should return true and keep file for 120-second rate limit" {
            # Create rate limit file with 120 seconds from now
            $futureTime = (Get-Date).AddSeconds(120).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            $rateLimitData = @{ endDate = $futureTime } | ConvertTo-Json
            $rateLimitFile = Join-Path $script:TestScriptsPath "rate-limit-enddate.json"
            $rateLimitData | Out-File -FilePath $rateLimitFile -Encoding utf8
            
            $result = Test-RateLimitInEffect
            
            $result | Should -Be $true
            Test-Path $rateLimitFile | Should -Be $true
        }
        
        It "Should return true and keep file for 300-second rate limit" {
            # Create rate limit file with 300 seconds from now
            $futureTime = (Get-Date).AddSeconds(300).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            $rateLimitData = @{ endDate = $futureTime } | ConvertTo-Json
            $rateLimitFile = Join-Path $script:TestScriptsPath "rate-limit-enddate.json"
            $rateLimitData | Out-File -FilePath $rateLimitFile -Encoding utf8
            
            $result = Test-RateLimitInEffect
            
            $result | Should -Be $true
            Test-Path $rateLimitFile | Should -Be $true
        }
    }
    
    Context "Malformed Rate Limit File" {
        It "Should remove invalid JSON file and return false" {
            # Create invalid JSON file
            $rateLimitFile = Join-Path $script:TestScriptsPath "rate-limit-enddate.json"
            "invalid json content" | Out-File -FilePath $rateLimitFile -Encoding utf8
            
            $result = Test-RateLimitInEffect
            
            $result | Should -Be $false
            Test-Path $rateLimitFile | Should -Be $false
        }
        
        It "Should remove file without endDate property and return false" {
            # Create JSON without endDate
            $rateLimitData = @{ otherProperty = "value" } | ConvertTo-Json
            $rateLimitFile = Join-Path $script:TestScriptsPath "rate-limit-enddate.json"
            $rateLimitData | Out-File -FilePath $rateLimitFile -Encoding utf8
            
            $result = Test-RateLimitInEffect
            
            $result | Should -Be $false
            Test-Path $rateLimitFile | Should -Be $false
        }
    }
    
    Context "Timezone Handling" {
        It "Should handle timezone conversions correctly" {
            # Create rate limit file with UTC time
            $utcTime = (Get-Date).ToUniversalTime().AddSeconds(30).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            $rateLimitData = @{ endDate = $utcTime } | ConvertTo-Json
            $rateLimitFile = Join-Path $script:TestScriptsPath "rate-limit-enddate.json"
            $rateLimitData | Out-File -FilePath $rateLimitFile -Encoding utf8
            
            # Mock Start-Sleep
            Mock Start-Sleep { 
                param($Seconds)
                $Seconds | Should -Be 60
            }
            
            $result = Test-RateLimitInEffect
            
            $result | Should -Be $false
            Test-Path $rateLimitFile | Should -Be $false
        }
    }
}