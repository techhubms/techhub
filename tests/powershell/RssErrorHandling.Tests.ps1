# Integration tests for RSS error handling functionality
# This test file validates the error handling improvements work end-to-end

Describe "RSS Error Handling Integration" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"

        $script:TestScriptsPath = Join-Path $script:TempPath "scripts"
        $script:TestDataPath = Join-Path $script:TempPath "_data"
        $script:TestSkippedEntriesPath = Join-Path $script:TestDataPath "skipped-entries.json"
        
        # Create test directories
        New-Item -Path $script:TestScriptsPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestDataPath -ItemType Directory -Force | Out-Null
        
        # Create empty tracking files
        Set-Content -Path $script:TestSkippedEntriesPath -Value "[]"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        
        # Recreate test directories and files after cleanup
        New-Item -Path $script:TestScriptsPath -ItemType Directory -Force | Out-Null
        New-Item -Path $script:TestDataPath -ItemType Directory -Force | Out-Null
        Set-Content -Path $script:TestSkippedEntriesPath -Value "[]"
    }
    
    Context "Add-TrackingEntry Function for Skipped Entries" {
        It "Should add content filter errors to skipped entries" {
            # Test adding a content filter error
            Add-TrackingEntry -EntriesPath $script:TestSkippedEntriesPath -CanonicalUrl "https://example.com/filtered-content" -Reason "Content blocked by safety filters" -Collection "news"
            
            # Verify entry was added
            $skippedEntries = Get-SkippedEntries -SkippedEntriesPath $script:TestSkippedEntriesPath
            $skippedEntries | Should -HaveCount 1
            $skippedEntries[0].canonical_url | Should -Be "https://example.com/filtered-content"
            $skippedEntries[0].reason | Should -Be "Content blocked by safety filters"
            $skippedEntries[0].collection | Should -Be "news"
            $skippedEntries[0].timestamp | Should -Not -BeNullOrEmpty
        }
        
        It "Should add API call failures to skipped entries" {
            # Test adding an API failure error
            Add-TrackingEntry -EntriesPath $script:TestSkippedEntriesPath -CanonicalUrl "https://example.com/api-failure" -Reason "API call failed: Network timeout" -Collection "community"
            
            # Verify entry was added (should have 1 entry in this isolated test)
            $skippedEntries = Get-SkippedEntries -SkippedEntriesPath $script:TestSkippedEntriesPath
            $skippedEntries | Should -HaveCount 1
            
            $apiFailureEntry = $skippedEntries | Where-Object { $_.canonical_url -eq "https://example.com/api-failure" }
            $apiFailureEntry | Should -Not -BeNullOrEmpty
            $apiFailureEntry.reason | Should -Be "API call failed: Network timeout"
        }
        
        It "Should handle multiple entries correctly" {
            # Add several more entries to test file management
            $testEntries = @(
                @{ url = "https://example.com/error1"; reason = "Content filtered"; collection = "news" },
                @{ url = "https://example.com/error2"; reason = "Rate limit exceeded"; collection = "videos" },
                @{ url = "https://example.com/error3"; reason = "API timeout"; collection = "community" }
            )
            
            foreach ($entry in $testEntries) {
                Add-TrackingEntry -EntriesPath $script:TestSkippedEntriesPath -CanonicalUrl $entry.url -Reason $entry.reason -Collection $entry.collection
            }
            
            # Verify all entries were added (should now have 3 total)
            $skippedEntries = Get-SkippedEntries -SkippedEntriesPath $script:TestSkippedEntriesPath
            $skippedEntries | Should -HaveCount 3
            
            # Verify specific entries exist
            foreach ($entry in $testEntries) {
                $foundEntry = $skippedEntries | Where-Object { $_.canonical_url -eq $entry.url }
                $foundEntry | Should -Not -BeNullOrEmpty
                $foundEntry.reason | Should -Be $entry.reason
                $foundEntry.collection | Should -Be $entry.collection
            }
        }
    }
    
    Context "Skipped Entries File Format" {
        It "Should maintain valid JSON format" {
            # Verify the skipped entries file is valid JSON
            $content = Get-Content $script:TestSkippedEntriesPath -Raw
            { $content | ConvertFrom-Json } | Should -Not -Throw
        }
        
        It "Should have proper timestamp format" {
            $skippedEntries = Get-SkippedEntries -SkippedEntriesPath $script:TestSkippedEntriesPath
            
            foreach ($entry in $skippedEntries) {
                # Verify timestamp can be parsed as DateTime
                { [DateTime]::Parse($entry.timestamp) } | Should -Not -Throw
                
                # Verify timestamp includes timezone information
                $entry.timestamp | Should -Match '\+\d{2}:\d{2}$'
            }
        }
        
        It "Should contain required fields" {
            $skippedEntries = Get-SkippedEntries -SkippedEntriesPath $script:TestSkippedEntriesPath
            
            foreach ($entry in $skippedEntries) {
                $entry.canonical_url | Should -Not -BeNullOrEmpty
                $entry.reason | Should -Not -BeNullOrEmpty
                $entry.timestamp | Should -Not -BeNullOrEmpty
            }
        }
    }
    
    Context "Error Handling Patterns" {
        It "Should recognize content filter error patterns" {
            $contentFilterPatterns = @(
                "Content blocked by safety filters",
                "Content filtered by safety filters",
                "Inappropriate content detected",
                "Content policy violation"
            )
            
            foreach ($pattern in $contentFilterPatterns) {
                $testUrl = "https://example.com/test-$(Get-Random)"
                Add-TrackingEntry -EntriesPath $script:TestSkippedEntriesPath -CanonicalUrl $testUrl -Reason $pattern -Collection "news"
                
                $skippedEntries = Get-SkippedEntries -SkippedEntriesPath $script:TestSkippedEntriesPath
                $addedEntry = $skippedEntries | Where-Object { $_.canonical_url -eq $testUrl }
                $addedEntry | Should -Not -BeNullOrEmpty
                $addedEntry.reason | Should -Be $pattern
            }
        }
        
        It "Should recognize API error patterns" {
            $apiErrorPatterns = @(
                "API call failed: Network timeout",
                "API call failed: Connection refused", 
                "API call failed: Service unavailable",
                "API call failed: Internal server error"
            )
            
            foreach ($pattern in $apiErrorPatterns) {
                $testUrl = "https://example.com/api-test-$(Get-Random)"
                Add-TrackingEntry -EntriesPath $script:TestSkippedEntriesPath -CanonicalUrl $testUrl -Reason $pattern -Collection "community"
                
                $skippedEntries = Get-SkippedEntries -SkippedEntriesPath $script:TestSkippedEntriesPath
                $addedEntry = $skippedEntries | Where-Object { $_.canonical_url -eq $testUrl }
                $addedEntry | Should -Not -BeNullOrEmpty
                $addedEntry.reason | Should -Be $pattern
            }
        }
    }
}