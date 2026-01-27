# PowerShell Test Suite

> **AI CONTEXT**: This is a **LEAF** context file for PowerShell tests in the `tests/powershell/` directory. It complements the [scripts/AGENTS.md](../../scripts/AGENTS.md) where the implementation lives.
> **RULE**: Follow the 10-step workflow in Root [AGENTS.md](../../AGENTS.md). Project principles are in [README.md](../../README.md). Follow **BOTH**.

## Overview

This directory contains **Pester v5** tests for PowerShell content processing scripts used by the Tech Hub. These tests validate RSS feed processing, markdown conversion, AI integration, and content transformation logic.

## What This Directory Contains

**Test Files**: Pester v5 test files (`.Tests.ps1`) that validate the behavior of PowerShell functions used in content processing.

**Helper Files**:

- `Initialize-BeforeAll.ps1` - Standard setup run before all tests (module imports, test data loading)
- `Initialize-BeforeEach.ps1` - Standard cleanup between individual tests

**Test Data**: Sample data files used by tests (if applicable)

## Where the Implementation Lives

**CRITICAL**: The actual PowerShell functions being tested are located in:

```text
scripts/content-processing/functions/
├── Add-TrackingEntry.ps1
├── Convert-RssToMarkdown.ps1
├── ConvertTo-SafeFilename.ps1
├── Feed.ps1
├── Format-FrontMatterValue.ps1
├── Get-BalancedHtmlContent.ps1
├── Get-FilteredTags.ps1
├── Get-FrontMatterValue.ps1
├── Get-MainContentFromHtml.ps1
├── Get-MarkdownFiles.ps1
├── Get-MarkdownFromHtml.ps1
├── Get-SourceRoot.ps1
├── Get-WebResponseDetailsFromException.ps1
├── Get-YouTubeVideoId.ps1
├── Invoke-AiApiCall.ps1
├── Invoke-ProcessWithAiModel.ps1
├── Repair-JsonResponse.ps1
├── Set-FrontMatterValue.ps1
├── Test-RateLimitInEffect.ps1
└── Write-ErrorDetails.ps1
```

For complete implementation details, patterns, and best practices, see [scripts/AGENTS.md](../../scripts/AGENTS.md).

## Running Tests

**🚨 ALWAYS prefer the Run function over direct script execution.**

See [README.md - Starting, Stopping and Testing](../../README.md#starting-stopping-and-testing-the-website) for complete documentation.

### Using Run Function (Recommended)

```powershell
# All PowerShell tests only (fast - no .NET build)
Run -TestProject powershell

# PowerShell tests by name pattern
Run -TestProject powershell -TestName "RSS"
Run -TestProject powershell -TestName "FrontMatter"

# All tests (PowerShell + .NET), then start servers
Run

# Skip all tests, start servers directly
Run -WithoutTests
```

**Aliases**: The following are equivalent:

- `Run -TestProject powershell`
- `Run -TestProject pester`
- `Run -TestProject scripts`

### Test Filtering Options

```powershell
# Run tests matching a name pattern
Run -TestProject powershell -TestName "RSS"
Run -TestProject powershell -TestName "FrontMatter"
Run -TestProject powershell -TestName "should convert basic"
```

### From VS Code

Use the integrated terminal and run the Run function commands above. The test runner will automatically detect the DevContainer environment.

## Testing Framework

**Framework**: Pester v5  
**Installation**: Managed via `.devcontainer/post-create.sh`  
**Version**: 5.x or higher

## Test File Naming Convention

- Test files must end with `.Tests.ps1`
- Test file name should match the function being tested
- Example: `Convert-RssToMarkdown.ps1` → `Convert-RssToMarkdown.Tests.ps1`

## Test Structure

All tests follow the Pester v5 pattern with `Describe`, `Context`, and `It` blocks:

```powershell
BeforeAll {
    # Load test initialization
    . (Join-Path $PSScriptRoot "Initialize-BeforeAll.ps1")
    
    # Load the function being tested
    . (Join-Path $script:FunctionsPath "Convert-RssToMarkdown.ps1")
}

Describe "Convert-RssToMarkdown" {
    Context "When converting valid RSS data" {
        It "should create markdown file with correct frontmatter" {
            # Arrange
            $rssItem = @{
                Title = "Test Article"
                Link = "https://example.com/article"
                # ... other properties
            }
            
            # Act
            $result = Convert-RssToMarkdown -RssItem $rssItem
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "title: Test Article"
        }
    }
    
    Context "When handling edge cases" {
        It "should handle missing description gracefully" {
            # Test logic
        }
    }
}
```

## Testing Best Practices

### Do's

✅ **Test real implementation** - Never duplicate production logic in tests  
✅ **Use descriptive test names** - Clearly state what behavior is being validated  
✅ **Test edge cases** - Empty strings, null values, special characters  
✅ **Use BeforeAll/BeforeEach** - Load shared setup code efficiently  
✅ **Mock external dependencies** - Use `Mock` for HTTP calls, file system operations  
✅ **Test error handling** - Verify functions handle errors gracefully  

### Don'ts

❌ **Don't test implementation details** - Test behavior, not internal state  
❌ **Don't add wrapper functions for tests** - Test the actual production code  
❌ **Don't skip cleanup** - Use `BeforeEach` to ensure clean state  
❌ **Don't hardcode paths** - Use `$PSScriptRoot` and path joining  
❌ **Don't suppress real errors** - Tests should fail loudly  

## Common Test Patterns

### Testing Function Output

```powershell
It "should return expected format" {
    $result = Get-FilteredTags -Tags @("ai", "github-copilot", "ml")
    $result | Should -HaveCount 3
    $result | Should -Contain "ai"
}
```

### Testing Error Handling

```powershell
It "should throw on invalid input" {
    { ConvertTo-SafeFilename -Filename $null } | Should -Throw
}
```

### Mocking External Calls

```powershell
BeforeAll {
    Mock Invoke-WebRequest {
        return @{
            StatusCode = 200
            Content = "<html><body>Test</body></html>"
        }
    }
}

It "should handle web response" {
    $result = Get-ContentFromUrl -Url "https://example.com"
    $result | Should -Not -BeNullOrEmpty
    Should -Invoke Invoke-WebRequest -Times 1
}
```

## Test Data Management

For tests requiring sample data:

1. Create sample files in `tests/powershell/test-data/` (if needed)
2. Load data in `BeforeAll` block
3. Clean up in `AfterAll` if files are created during tests

## Coverage Requirements

- **Minimum coverage**: 80% for all functions with tests
- **Focus on**: Business logic, error paths, edge cases
- **Less critical**: Simple getters/setters, trivial wrappers

## Troubleshooting

### Tests Not Found

Ensure test files:

- Are named `*.Tests.ps1`
- Are located in `tests/powershell/`
- Follow Pester v5 syntax

### Function Not Found

Check that:

- Function path is correct in test file
- `Initialize-BeforeAll.ps1` is loading correctly
- Function file exists in `scripts/content-processing/functions/`

### Import Errors

Verify:

- Pester v5 is installed: `Get-Module -Name Pester -ListAvailable`
- PowerShell version is 7+: `$PSVersionTable.PSVersion`
- Running from correct directory (use test runner script)

## Related Documentation

- **Implementation**: [scripts/AGENTS.md](../../scripts/AGENTS.md) - PowerShell development patterns
- **Content Processing**: [scripts/content-processing/](../../scripts/content-processing/) - Main scripts
- **Root Architecture**: [AGENTS.md](../../AGENTS.md) - Global principles and workflow

## Key Testing Rules

1. **ALWAYS run tests after modifying PowerShell functions** - Use `Run -TestProject powershell`
2. **Write tests FIRST for bug fixes** - Reproduce the issue before fixing
3. **Test real code, not mocks** - Don't replicate production logic in tests
4. **Use meaningful assertions** - Make test failures easy to diagnose
5. **Keep tests focused** - One logical assertion per `It` block

---

**Remember**: These tests validate the content processing automation that powers the Tech Hub. Ensure all changes maintain backward compatibility and don't break existing RSS feed processing workflows.
