# Get-MarkdownFiles.Tests.ps1
# Tests for the Get-MarkdownFiles PowerShell function

Describe "Get-MarkdownFiles" {
    BeforeAll {
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        
        # Create a test directory structure with markdown files (after cleanup in Initialize-BeforeEach)
        $script:testRoot = Join-Path $global:TempPath "markdown-test"
        New-Item -ItemType Directory -Path $script:testRoot -Force | Out-Null
        
        # Create .gitignore to enable default exclusion patterns
        $gitignoreContent = @"
node_modules/
.git/
spec/
_site/
**/AGENTS.md
"@
        Set-Content -Path (Join-Path $script:testRoot ".gitignore") -Value $gitignoreContent -Force
        
        # Create test directory structure
        $dirs = @(
            "collections/_news",
            "collections/_videos",
            "collections/_blogs",
            "docs",
            "node_modules/some-package",
            ".git/objects",
            "spec/test",
            "_site/generated"
        )
        
        foreach ($dir in $dirs) {
            New-Item -ItemType Directory -Path (Join-Path $script:testRoot $dir) -Force | Out-Null
        }
        
        # Create test markdown files
        $files = @(
            # Valid content files
            "collections/_news/2024-01-01-article.md",
            "collections/_videos/2024-01-02-video.md",
            "collections/_blogs/2024-01-03-blog.md",
            
            # AGENTS.md files (should be excluded by default)
            "AGENTS.md",
            "collections/AGENTS.md",
            "docs/AGENTS.md",
            
            # Guidelines files (should be excluded by default)
            "collections/writing-style-guidelines.md",
            
            # Regular docs files
            "docs/feature-spec.md",
            "docs/architecture.md",
            
            # Files in excluded directories
            "node_modules/some-package/readme.md",
            ".git/objects/info.md",
            "spec/test/sample.md",
            "_site/generated/page.md"
        )
        
        foreach ($file in $files) {
            $filePath = Join-Path $script:testRoot $file
            # Ensure parent directory exists
            $parentDir = Split-Path -Parent $filePath
            if (-not (Test-Path $parentDir)) {
                New-Item -ItemType Directory -Path $parentDir -Force | Out-Null
            }
            "# Test Content" | Set-Content -Path $filePath -Force
        }
    }
    
    Context "Parameter Validation" {
        It "Should throw when Root parameter is missing" {
            { Get-MarkdownFiles -Root $null } | Should -Throw
        }
        
        It "Should return empty array when no markdown files exist" {
            $emptyDir = Join-Path $global:TempPath "empty-test"
            New-Item -ItemType Directory -Path $emptyDir -Force | Out-Null
            
            $result = Get-MarkdownFiles -Root $emptyDir
            
            $result | Should -BeNullOrEmpty
        }
    }
    
    Context "Default Behavior (No Filters)" {
        It "Should find all markdown files when no filters are specified" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot)
            
            # Should find some files (exact count depends on exclusions)
            $result.Count | Should -BeGreaterThan 0
        }
        
        It "Should exclude default directory patterns" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot)
            
            # Should NOT include files from excluded directories
            $result.FullName | Should -Not -Match "node_modules"
            $result.FullName | Should -Not -Match "\.git"
            $result.FullName | Should -Not -Match "\\spec\\"
            $result.FullName | Should -Not -Match "_site"
        }
        
        It "Should exclude AGENTS.md files by default" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot)
            
            # Should NOT include any AGENTS.md files
            $agentsFiles = @($result | Where-Object { $_.Name -eq "AGENTS.md" })
            $agentsFiles.Count | Should -Be 0 -Because "AGENTS.md files should be excluded by default"
        }
    }
    
    Context "Include Directory Patterns" {
        It "Should only include files matching include pattern" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot -IncludeDirectoryPatterns @('collections/*'))
            
            # All results should be from collections directory
            foreach ($file in $result) {
                $relativePath = $file.FullName.Substring($script:testRoot.Length).TrimStart('\', '/')
                $relativePath | Should -Match "^collections/"
            }
        }
        
        It "Should support multiple include patterns" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot -IncludeDirectoryPatterns @('collections/_news/*', 'docs/*'))
            
            # Should include files from both patterns
            $newsFiles = @($result | Where-Object { $_.FullName -match "collections[\\/]_news" })
            $docFiles = @($result | Where-Object { $_.FullName -match "docs" })
            
            $newsFiles.Count | Should -BeGreaterThan 0
            $docFiles.Count | Should -BeGreaterThan 0
        }
    }
    
    Context "Exclude File Patterns" {
        It "Should exclude AGENTS.md in root directory" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot -ExcludeFilePatterns @('AGENTS.md'))
            
            # Should NOT find root AGENTS.md
            $rootAgents = @($result | Where-Object { $_.FullName -eq (Join-Path $script:testRoot "AGENTS.md") })
            $rootAgents | Should -BeNullOrEmpty
        }
        
        It "Should exclude AGENTS.md in subdirectories with wildcard pattern" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot -ExcludeFilePatterns @('*/AGENTS.md', 'AGENTS.md'))
            
            # Should NOT find ANY AGENTS.md files
            $agentsFiles = @($result | Where-Object { $_.Name -eq "AGENTS.md" })
            $agentsFiles.Count | Should -Be 0 -Because "All AGENTS.md files should be excluded"
        }
        
        It "Should exclude guidelines files with wildcard pattern" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot -ExcludeFilePatterns @('*-guidelines.md'))
            
            # Should NOT find any guidelines files
            $guidelineFiles = @($result | Where-Object { $_.Name -like "*-guidelines.md" })
            $guidelineFiles.Count | Should -Be 0 -Because "All guidelines files should be excluded"
        }
        
        It "Should support multiple exclude file patterns" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot -ExcludeFilePatterns @('*/AGENTS.md', 'AGENTS.md', '*-guidelines.md'))
            
            # Should exclude both AGENTS.md and guidelines
            $agentsFiles = @($result | Where-Object { $_.Name -eq "AGENTS.md" })
            $guidelineFiles = @($result | Where-Object { $_.Name -like "*-guidelines.md" })
            
            $agentsFiles.Count | Should -Be 0
            $guidelineFiles.Count | Should -Be 0
        }
    }
    
    Context "Combined Include and Exclude Patterns" {
        It "Should apply include first, then exclude" {
            # Include collections/*, but exclude AGENTS.md and guidelines
            $result = @(Get-MarkdownFiles -Root $script:testRoot `
                    -IncludeDirectoryPatterns @('collections/*') `
                    -ExcludeFilePatterns @('*/AGENTS.md', 'AGENTS.md', '*-guidelines.md'))
            
            # All files should be from collections
            foreach ($file in $result) {
                $relativePath = $file.FullName.Substring($script:testRoot.Length).TrimStart('\', '/')
                $relativePath | Should -Match "^collections/"
            }
            
            # But no AGENTS.md or guidelines
            $agentsFiles = @($result | Where-Object { $_.Name -eq "AGENTS.md" })
            $guidelineFiles = @($result | Where-Object { $_.Name -like "*-guidelines.md" })
            
            $agentsFiles.Count | Should -Be 0
            $guidelineFiles.Count | Should -Be 0
        }
        
        It "Should handle collections/* pattern correctly for content processing" {
            # Pattern used for processing all collection content
            $result = @(Get-MarkdownFiles -Root $script:testRoot `
                    -IncludeDirectoryPatterns @('collections/*') `
                    -ExcludeFilePatterns @('*/AGENTS.md', 'AGENTS.md', '*-guidelines.md'))
            
            # Should find content files
            $newsFiles = @($result | Where-Object { $_.FullName -match "_news" })
            $videoFiles = @($result | Where-Object { $_.FullName -match "_videos" })
            $blogFiles = @($result | Where-Object { $_.FullName -match "_blogs" })
            
            $newsFiles.Count | Should -Be 1
            $videoFiles.Count | Should -Be 1
            $blogFiles.Count | Should -Be 1
            
            # Should NOT find AGENTS.md or guidelines in collections/
            $excludedFiles = @($result | Where-Object { 
                    $_.Name -eq "AGENTS.md" -or $_.Name -like "*-guidelines.md" 
                })
            $excludedFiles.Count | Should -Be 0
        }
    }
    
    Context "Edge Cases" {
        It "Should handle paths with spaces" {
            $spaceDir = Join-Path $script:testRoot "dir with spaces"
            New-Item -ItemType Directory -Path $spaceDir -Force | Out-Null
            "# Content" | Set-Content -Path (Join-Path $spaceDir "file.md") -Force
            
            $result = @(Get-MarkdownFiles -Root $script:testRoot)
            
            $spaceFile = @($result | Where-Object { $_.Directory.Name -eq "dir with spaces" })
            $spaceFile | Should -Not -BeNullOrEmpty
        }
        
        It "Should be case-insensitive for .md extension" {
            $upperFile = Join-Path $script:testRoot "UPPERCASE.MD"
            $upperDir = Split-Path -Parent $upperFile
            if (-not (Test-Path $upperDir)) {
                New-Item -ItemType Directory -Path $upperDir -Force | Out-Null
            }
            "# Content" | Set-Content -Path $upperFile -Force
            
            $result = @(Get-MarkdownFiles -Root $script:testRoot)
            
            $found = @($result | Where-Object { $_.Name -eq "UPPERCASE.MD" })
            $found | Should -Not -BeNullOrEmpty
        }
        
        It "Should handle empty exclude patterns gracefully" {
            $result = @(Get-MarkdownFiles -Root $script:testRoot -ExcludeFilePatterns @())
            
            # Should still work and find files
            $result.Count | Should -BeGreaterThan 0
        }
    }
    
    Context "Regression Tests" {
        It "Should exclude collections/AGENTS.md to prevent errors" {
            # This test specifically validates the bug fix
            $result = @(Get-MarkdownFiles -Root $script:testRoot `
                    -IncludeDirectoryPatterns @('collections/*') `
                    -ExcludeFilePatterns @('*/AGENTS.md', 'AGENTS.md', '*-guidelines.md'))
            
            # The specific file that was causing the error should NOT be included
            $collectionsAgents = @($result | Where-Object { 
                    $_.FullName -match "collections[\\/]AGENTS\.md$" 
                })
            
            $collectionsAgents | Should -BeNullOrEmpty -Because "collections/AGENTS.md must be excluded to prevent errors"
        }
    }
}
