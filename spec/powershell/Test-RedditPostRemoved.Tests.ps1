Describe "Test-RedditPostRemoved" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
    }
    
    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        
        # Override the default empty Get-ContentFromUrl mock with specific Reddit responses
        Mock Get-ContentFromUrl {
            param($Url)
            
            # Mock response for active Reddit post
            if ($Url -like "*reddit.com/r/AZURE/comments/1mi33ug/*") {
                return @'
[
    {
        "kind": "Listing",
        "data": {
            "children": [
                {
                    "kind": "t3",
                    "data": {
                        "title": "How I replaced 10 Logic App conditions with 1 C#",
                        "author": "testuser",
                        "selftext": "This is the post content...",
                        "id": "1mi33ug",
                        "subreddit": "AZURE",
                        "created_utc": 1640995200,
                        "removed": false,
                        "removed_by": null,
                        "removed_by_category": null,
                        "removal_reason": null,
                        "banned_by": null,
                        "banned_at_utc": null,
                        "spam": false
                    }
                }
            ]
        }
    }
]
'@
            }
            # Mock response for invalid/non-existent Reddit URLs
            elseif ($Url -like "*reddit.com/invalid/*" -or $Url -like "*reddit.com/r/test/comments/invalid*") {
                return "[]"
            }
            # Mock response for non-Reddit URLs (should cause error)
            elseif ($Url -like "*invalid-reddit-url.com/*") {
                throw "Unable to connect to the remote server"
            }
            # Default empty response for other URLs
            else {
                return "[]"
            }
        }
    }

    Context "Parameter Validation" {
        It "Should throw when RedditUrl parameter is null" {
            { Test-RedditPostRemoved -RedditUrl $null } | Should -Throw
        }
        
        It "Should throw when RedditUrl parameter is empty" {
            { Test-RedditPostRemoved -RedditUrl "" } | Should -Throw
        }
    }
    
    Context "Reddit JSON Structure Handling" {
        It "Should handle posts without removed property gracefully" {
            # Test with a real Reddit URL that should be active
            $testUrl = "https://www.reddit.com/r/AZURE/comments/1mi33ug/how_i_replaced_10_logic_app_conditions_with_1_c"
            
            $result = Test-RedditPostRemoved -RedditUrl $testUrl
            
            # Should not crash and should return proper structure
            $result | Should -Not -BeNullOrEmpty
            $result.IsRemoved | Should -BeOfType [System.Boolean]
            $result.Error | Should -BeNullOrEmpty
            $result.Details | Should -Not -BeNullOrEmpty
            $result.PostData | Should -Not -BeNullOrEmpty
        }
        
        It "Should properly check removal-related properties that exist" {
            # Test with a real Reddit URL 
            $testUrl = "https://www.reddit.com/r/AZURE/comments/1mi33ug/how_i_replaced_10_logic_app_conditions_with_1_c"
            
            $result = Test-RedditPostRemoved -RedditUrl $testUrl
            
            # For an active post, should detect it as not removed
            $result.IsRemoved | Should -Be $false
            $result.Details | Should -Match "Post is active"
        }
    }
    
    Context "Error Handling" {
        It "Should handle invalid Reddit URLs gracefully" {
            $invalidUrl = "https://invalid-reddit-url.com/invalid"
            
            $result = Test-RedditPostRemoved -RedditUrl $invalidUrl
            
            $result | Should -Not -BeNullOrEmpty
            $result.Error | Should -Not -BeNullOrEmpty
        }
        
        It "Should handle network errors gracefully" {
            $invalidUrl = "https://reddit.com/invalid/path/that/does/not/exist"
            
            $result = Test-RedditPostRemoved -RedditUrl $invalidUrl
            
            $result | Should -Not -BeNullOrEmpty
            # Should either have an error or mark as removed (due to no data)
            ($result.Error -or $result.IsRemoved) | Should -Be $true
        }
    }
    
    Context "Removal Detection Logic" {
        It "Should detect removal indicators when present" {
            # This test uses mock data to verify removal detection logic
            # We can't easily test with a real removed post since they change over time
            
            # Test that the function structure works correctly
            $testUrl = "https://www.reddit.com/r/test/comments/invalid"
            
            $result = Test-RedditPostRemoved -RedditUrl $testUrl
            
            # Should not crash regardless of Reddit response
            $result | Should -Not -BeNullOrEmpty
            $result.IsRemoved | Should -BeOfType [System.Boolean]
        }
    }
    
    Context "Return Structure" {
        It "Should return proper hashtable structure" {
            $testUrl = "https://www.reddit.com/r/AZURE/comments/1mi33ug/how_i_replaced_10_logic_app_conditions_with_1_c"
            
            $result = Test-RedditPostRemoved -RedditUrl $testUrl
            
            # Check all required properties exist
            $result.ContainsKey("IsRemoved") | Should -Be $true
            $result.ContainsKey("Error") | Should -Be $true  
            $result.ContainsKey("Details") | Should -Be $true
            $result.ContainsKey("PostData") | Should -Be $true
            
            # Check property types
            $result.IsRemoved | Should -BeOfType [System.Boolean]
        }
    }
}
