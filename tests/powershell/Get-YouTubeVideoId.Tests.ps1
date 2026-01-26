# Get-YouTubeVideoId.Tests.ps1
# Tests for the Get-YouTubeVideoId PowerShell function

Describe "Get-YouTubeVideoId" {
    BeforeAll {
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }
    
    Context "Standard YouTube URLs" {
        It "Should extract video ID from standard watch URL" {
            # Arrange
            $url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract video ID from standard watch URL with additional parameters" {
            # Arrange
            $url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&t=30s&list=PLrAXtmRdnEQy8Q"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract video ID from URL without www" {
            # Arrange
            $url = "https://youtube.com/watch?v=dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
    }
    
    Context "Shortened YouTube URLs" {
        It "Should extract video ID from youtu.be URL" {
            # Arrange
            $url = "https://youtu.be/dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract video ID from youtu.be URL with parameters" {
            # Arrange
            $url = "https://youtu.be/dQw4w9WgXcQ?t=30"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract video ID from youtu.be URL without https" {
            # Arrange
            $url = "http://youtu.be/dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
    }
    
    Context "Embed YouTube URLs" {
        It "Should extract video ID from embed URL" {
            # Arrange
            $url = "https://www.youtube.com/embed/dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract video ID from embed URL with parameters" {
            # Arrange
            $url = "https://www.youtube.com/embed/dQw4w9WgXcQ?start=30&autoplay=1"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract video ID from embed URL without www" {
            # Arrange
            $url = "https://youtube.com/embed/dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
    }
    
    Context "Older Embed YouTube URLs" {
        It "Should extract video ID from older embed URL" {
            # Arrange
            $url = "https://www.youtube.com/v/dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract video ID from older embed URL with parameters" {
            # Arrange
            $url = "https://www.youtube.com/v/dQw4w9WgXcQ?version=3&hl=en_US"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
    }
    
    Context "YouTube Shorts URLs" {
        It "Should extract video ID from Shorts URL" {
            # Arrange
            $url = "https://www.youtube.com/shorts/dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract video ID from Shorts URL with parameters" {
            # Arrange
            $url = "https://www.youtube.com/shorts/dQw4w9WgXcQ?feature=share"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract video ID from Shorts URL without www" {
            # Arrange
            $url = "https://youtube.com/shorts/dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
    }
    
    Context "Parameter Validation" {
        It "Should throw error when URL parameter is null" {
            { Get-YouTubeVideoId -Url $null } | Should -Throw "*URL parameter cannot be null or empty*"
        }
        
        It "Should throw error when URL parameter is empty string" {
            { Get-YouTubeVideoId -Url "" } | Should -Throw "*URL parameter cannot be null or empty*"
        }
        
        It "Should throw error when URL parameter is whitespace" {
            { Get-YouTubeVideoId -Url "   " } | Should -Throw "*URL parameter cannot be null or empty*"
        }
        
        It "Should throw error for invalid URL format" {
            { Get-YouTubeVideoId -Url "not-a-valid-url" } | Should -Throw "*Invalid URL format*"
        }
    }
    
    Context "Edge Cases and Invalid URLs" {
        It "Should return empty string for non-YouTube URL" {
            # Arrange
            $url = "https://www.google.com/search?q=youtube"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be ""
        }
        
        It "Should return empty string for YouTube URL without video ID" {
            # Arrange
            $url = "https://www.youtube.com/"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be ""
        }
        
        It "Should return empty string for YouTube watch URL without v parameter" {
            # Arrange
            $url = "https://www.youtube.com/watch?list=PLrAXtmRdnEQy8Q"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be ""
        }
        
        It "Should return empty string for invalid video ID length" {
            # Arrange
            $url = "https://www.youtube.com/watch?v=shortid"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be ""
        }
        
        It "Should return empty string for video ID with invalid characters" {
            # Arrange
            $url = "https://www.youtube.com/watch?v=dQw4w9WgXc@"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be ""
        }
    }
    
    Context "Video ID Format Validation" {
        It "Should accept video ID with letters, numbers, hyphens, and underscores" {
            # Arrange
            $url = "https://www.youtube.com/watch?v=aB3-De_4FgH"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "aB3-De_4FgH"
        }
        
        It "Should accept video ID with underscores" {
            # Arrange
            $url = "https://www.youtube.com/watch?v=aB3_De_4FgH"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "aB3_De_4FgH"
        }
        
        It "Should handle mixed case video IDs correctly" {
            # Arrange
            $url = "https://www.youtube.com/watch?v=AbCdEfGhIjK"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "AbCdEfGhIjK"
        }
    }
    
    Context "Real-World Examples" {
        It "Should extract ID from real Rick Roll video URL" {
            # Arrange
            $url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
        }
        
        It "Should extract ID from real tech tutorial URL" {
            # Arrange
            $url = "https://youtu.be/8JJ101D3knE"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "8JJ101D3knE"
        }
        
        It "Should extract ID from real Shorts URL" {
            # Arrange
            $url = "https://www.youtube.com/shorts/2g811Eo7K8U"
            
            # Act
            $result = Get-YouTubeVideoId -Url $url
            
            # Assert
            $result | Should -Be "2g811Eo7K8U"
        }
    }
    
    Context "Performance" {
        It "Should process URL efficiently" {
            # Arrange
            $url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
            
            # Act
            $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
            $result = Get-YouTubeVideoId -Url $url
            $stopwatch.Stop()
            
            # Assert
            $result | Should -Be "dQw4w9WgXcQ"
            $stopwatch.ElapsedMilliseconds | Should -BeLessThan 100
        }
    }
}
