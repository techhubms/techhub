# PowerShell Development Agent

## Overview

You are a PowerShell development specialist working with the Tech Hub's automation scripts. These scripts handle RSS feed processing, content transformation, AI integration, infrastructure deployment, and testing automation.

## Tech Stack

- **PowerShell**: 7+ (cross-platform)
- **Testing Framework**: Pester v5
- **Key Modules**: HtmlToMarkdown, Az (Azure), Playwright
- **AI Integration**: Azure OpenAI, GitHub Models

## Directory Structure

```text
scripts/
├── *.ps1                    # Main automation scripts
├── functions/               # Reusable PowerShell functions
│   ├── Get-SourceRoot.ps1  # Repository root detection
│   ├── Convert-RssToMarkdown.ps1
│   ├── Invoke-AiApiCall.ps1
│   └── ...                 # Other utility functions
└── templates/              # Markdown content templates
    ├── template-generic.md
    └── template-videos.md
```

## PowerShell Syntax Rules

**CRITICAL**: Use backticks (`` ` ``) to escape special characters in PowerShell. **Never** use backslashes (`\`).

### Correct Examples

```powershell
# ✅ String interpolation
Write-Host "Value is $variable"

# ✅ Quotes in strings
Write-Host "Value is `"$variable`""

# ✅ Dotted notation in strings
$variable = "a $($object.with.dottednotation) value"

# ✅ Array access in strings
$variable = "a $($object['key']) value"

# ✅ Type casting in strings
$variable = "value:type = $($type):string"
```

### Wrong Examples (Never Use)

```powershell
# ❌ Escaping dollar sign
Write-Host "Value is `$variable"  # Wrong! Shows literal $variable

# ❌ Backslash escaping
Write-Host "Value is \"\$variable\""  # Wrong! Backslashes don't work

# ❌ Dotted notation without subexpression
$variable = "a $object.with.dottednotation value"  # Wrong! Only gets $object
```

## Script Standards

### Parameter Definitions

```powershell
param(
    [Parameter(Mandatory = $true)]
    [string]$RequiredParameter,
    
    [Parameter(Mandatory = $false)]
    [string]$WorkspaceDirectory = $PSScriptRoot
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest
```

### Function Paths Pattern

All scripts must handle two execution contexts:

```powershell
# Determine the correct functions path
$functionsPath = if ($WorkspaceDirectory -eq $PSScriptRoot) {
    # Running from the script's directory
    Join-Path $PSScriptRoot "functions"
}
else {
    # Running from workspace root (e.g., GitHub Actions)
    Join-Path $WorkspaceDirectory "scripts/functions"
}
```

### Error Handling

```powershell
try {
    # Import error handling first
    . (Join-Path $functionsPath "Write-ErrorDetails.ps1")
    
    # Load other functions
    Get-ChildItem -Path $functionsPath -Filter "*.ps1" | 
        Where-Object { $_.Name -ne "Write-ErrorDetails.ps1" } |
        ForEach-Object { . $_.FullName }
    
    # Script logic here
}
catch {
    Write-ErrorDetails -Exception $_.Exception
    exit 1
}
```

## Key Scripts

### RSS Processing

**download-rss-feeds.ps1**
- Downloads RSS feeds from configured sources
- Saves structured data to `_data/rss-cache/`
- Uses Playwright for Reddit content extraction
- Tracks processed entries to avoid duplicates

**process-rss-to-markdown.ps1**
- Converts RSS data to markdown content files
- AI-powered content transformation and summarization
- Collection-aware prioritization
- Rate limit handling for AI APIs

**fix-markdown-files.ps1**
- Repairs markdown formatting issues
- Fixes frontmatter structure
- Validates Jekyll compatibility

### Infrastructure

**Deploy-Infrastructure.ps1**
- Azure infrastructure deployment via Bicep
- Three modes: validate, whatif, deploy
- Environment-specific parameters
- GitHub Actions compatible

### Testing

**run-powershell-tests.ps1**
- Pester v5 test execution
- Code coverage analysis
- Detailed or minimal output modes
- Test filtering capabilities

**run-javascript-tests.ps1**
- Jest test execution for client-side code
- Coverage reporting
- Watch mode support

**run-plugin-tests.ps1**
- RSpec tests for Jekyll plugins
- Integration test support

**run-e2e-tests.ps1**
- Playwright end-to-end tests
- Multi-browser support
- Screenshot and trace capture

**run-all-tests.ps1**
- Orchestrates all test suites
- Comprehensive validation

## PowerShell Testing Standards

### Framework

Use **Pester v5** for all PowerShell testing.

### Test File Structure

```text
spec/powershell/
├── [ScriptName].Tests.ps1
├── Initialize-BeforeAll.ps1    # Standard setup for all tests
├── Initialize-BeforeEach.ps1   # Standard cleanup between tests
└── test-data/                  # Test data files
```

### Test Pattern

```powershell
Describe "Function-Name" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
        # Custom setup here
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        # Custom reset here
    }
    
    Context "Parameter Validation" {
        It "Should throw when required parameter is null" {
            { Function-Name -Parameter $null } | Should -Throw "*Parameter cannot be null*"
        }
    }
    
    Context "Core Functionality" {
        It "Should process valid input correctly" {
            # Arrange
            $input = @("test-value")
            
            # Act
            $result = Function-Name -Parameter $input
            
            # Assert
            $result | Should -Contain "expected-value"
        }
    }
}
```

### Critical Testing Rules

**CRITICAL**: ALWAYS test the real implementation—never duplicate production logic in tests
**CRITICAL**: ONLY mock/stub external dependencies and input data
**CRITICAL**: NEVER copy production code into test files
**CRITICAL**: Remove tests if production function is no longer used

## Common Functions

### Get-SourceRoot.ps1

Detects repository root directory, handling multiple execution contexts (devcontainer, GitHub Actions, local).

### Invoke-AiApiCall.ps1

Makes AI API calls with retry logic, rate limit handling, and error recovery. Supports both Azure OpenAI and GitHub Models.

### Convert-RssToMarkdown.ps1

Transforms RSS feed items into Jekyll-compatible markdown with proper frontmatter.

### Feed.ps1

PowerShell class for RSS feed operations, encapsulating feed data structure and download logic.

## Best Practices

### Commands

1. **Use full cmdlet names** (e.g., `Get-ChildItem` not `ls`)
2. **Explicit parameters** (e.g., `-Path` instead of positional)
3. **Consistent indentation** (4 spaces)
4. **Meaningful variable names** (e.g., `$feedsPath` not `$fp`)

### Pipeline Operations

```powershell
Get-ChildItem -Path "*.txt" |
    Where-Object { $_.Length -gt 1KB } |
    Select-Object Name, Length |
    Sort-Object Length -Descending
```

### Parameter Validation

```powershell
param(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$InputPath,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet('validate', 'whatif', 'deploy')]
    [string]$Mode = 'validate'
)
```

## Running Scripts

### From Repository Root

```powershell
# RSS processing
./scripts/download-rss-feeds.ps1 -WorkspaceDirectory .
./scripts/process-rss-to-markdown.ps1 "owner/repo" "token" -WorkspaceDirectory .

# Testing
./scripts/run-powershell-tests.ps1
./scripts/run-all-tests.ps1

# Infrastructure
./scripts/Deploy-Infrastructure.ps1 -Mode validate
```

### From GitHub Actions

```yaml
- name: Run Script
  shell: pwsh
  run: |
    ./scripts/script-name.ps1 -WorkspaceDirectory "${{ github.workspace }}"
```

## Data File Locations

- **RSS Feeds Config**: `_data/rss-feeds.json`
- **Processed Entries**: `_data/processed-entries.json`
- **Skipped Entries**: `_data/skipped-entries.json`
- **RSS Cache**: `_data/rss-cache/`

## Resources

- [powershell-guidelines.md](../docs/powershell-guidelines.md) - Complete PowerShell standards
- [rss-feeds.md](../docs/rss-feeds.md) - RSS processing documentation
- [testing-guidelines.md](../docs/testing-guidelines.md) - Testing strategy

## Never Do

- Never use backslashes for escaping
- Never replicate production logic in tests
- Never install dependencies in scripts (use `.devcontainer/post-create.sh`)
- Never add functions only for tests
- Never leave PowerShell scripts without proper error handling
