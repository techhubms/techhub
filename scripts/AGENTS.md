# PowerShell Development Agent

> **AI CONTEXT**: This is a **LEAF** context file for the `scripts/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

> ⚠️ **CRITICAL TESTING RULE**: After making ANY changes to files in `scripts/`, you MUST run the PowerShell test suite by executing `./scripts/run-powershell-tests.ps1` to validate your changes.

## Overview

You are a PowerShell development specialist working with the Tech Hub's automation scripts. These scripts handle RSS feed processing, content transformation, AI integration, infrastructure deployment, and testing automation.

## When to Use This Guide

**Read this file when**:

- Writing or modifying PowerShell scripts in `scripts/` directory
- Creating automation for content processing
- Working with RSS feeds or AI integration
- Implementing build or deployment automation
- Debugging PowerShell-based workflows

**Related Documentation**:

- Testing PowerShell scripts → [spec/AGENTS.md](../spec/AGENTS.md)
- Jekyll integration → [.github/agents/fullstack.md](../.github/agents/fullstack.md)
- Content management → [collections/AGENTS.md](../collections/AGENTS.md)

## Tech Stack

- **PowerShell**: 7+ (cross-platform)
- **Testing Framework**: Pester v5
- **Key Modules**: HtmlToMarkdown, Az (Azure), Playwright
- **AI Integration**: Azure AI Foundry
- **Ruby**: 3.2+ (Jekyll dependencies)
- **Node.js**: 22+ (development tooling)

## Directory Structure

```text
scripts/
├── jekyll-start.ps1         # Start Jekyll development server
├── jekyll-stop.ps1          # Stop Jekyll development server
├── run-all-tests.ps1        # Run all test suites
├── run-e2e-tests.ps1        # Playwright end-to-end tests
├── run-javascript-tests.ps1 # Jest JavaScript tests
├── run-plugin-tests.ps1     # RSpec Ruby plugin tests
├── run-powershell-tests.ps1 # Pester PowerShell tests
├── data/                    # Script data files
│   ├── rss-feeds.json       # RSS feed configuration
│   ├── processed-entries.json
│   ├── skipped-entries.json
│   └── rss-cache/           # Downloaded RSS data
└── content-processing/      # Content generation scripts
    ├── download-rss-feeds.ps1
    ├── process-rss-to-markdown.ps1
    ├── fix-markdown-files.ps1
    ├── iterative-roundup-generation.ps1
    ├── detect-repository-content-issues.ps1
    ├── system-message.md
    ├── functions/           # Reusable PowerShell functions
    │   ├── Get-SourceRoot.ps1
    │   ├── Convert-RssToMarkdown.ps1
    │   ├── Invoke-AiApiCall.ps1
    │   └── ...
    └── templates/           # Markdown content templates
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

Content processing scripts must handle two execution contexts:

```powershell
# Determine the correct functions path
$functionsPath = if ($WorkspaceDirectory -eq $PSScriptRoot) {
    # Running from the script's directory
    Join-Path $PSScriptRoot "functions"
}
else {
    # Running from workspace root (e.g., GitHub Actions)
    Join-Path $WorkspaceDirectory "scripts/content-processing/functions"
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

### Content Processing (in `content-processing/`)

**download-rss-feeds.ps1**

- Downloads RSS feeds from configured sources
- Saves structured data to `scripts/data/rss-cache/`
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

**iterative-roundup-generation.ps1**

- AI-powered weekly roundup generation
- Multi-step iteration with validation
- Combines content from multiple sources

**detect-repository-content-issues.ps1**

- Validates markdown content across collections
- Checks for duplicates and similarity
- Verifies frontmatter structure

### Infrastructure

**infra/Deploy-Infrastructure.ps1**

- Azure infrastructure deployment via Bicep
- Three modes: validate, whatif, deploy
- Environment-specific parameters
- GitHub Actions compatible

### Testing

**Script-Specific Testing**:

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

**For complete testing strategy, patterns, and best practices, see [spec/AGENTS.md](../spec/AGENTS.md).**

## PowerShell Testing Standards

Use **Pester v5** for all PowerShell testing. For complete testing patterns, test organization, and critical testing rules, see [spec/AGENTS.md](../spec/AGENTS.md).

### Test File Location

```text
spec/powershell/
├── [ScriptName].Tests.ps1
├── Initialize-BeforeAll.ps1    # Standard setup for all tests
├── Initialize-BeforeEach.ps1   # Standard cleanup between tests
└── test-data/                  # Test data files
```

### Running Pester Tests

```bash
# All PowerShell tests
./scripts/run-powershell-tests.ps1

# Specific test file
./scripts/run-powershell-tests.ps1 -TestPath "spec/powershell/Convert-RssToMarkdown.Tests.ps1"

# With coverage
./scripts/run-powershell-tests.ps1 -Coverage
```

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
# Content processing
./scripts/content-processing/download-rss-feeds.ps1 -WorkspaceDirectory .
./scripts/content-processing/process-rss-to-markdown.ps1 "owner/repo" "token" -WorkspaceDirectory .

# Testing
./scripts/run-powershell-tests.ps1
./scripts/run-all-tests.ps1

# Infrastructure
./infra/Deploy-Infrastructure.ps1 -Mode validate
```

### From GitHub Actions

```yaml
- name: Run Content Processing
  shell: pwsh
  run: |
    ./scripts/content-processing/download-rss-feeds.ps1 -WorkspaceDirectory "${{ github.workspace }}"

- name: Run Tests
  shell: pwsh
  run: |
    ./scripts/run-powershell-tests.ps1
```

## Data File Locations

- **RSS Feeds Config**: `scripts/data/rss-feeds.json`
- **Processed Entries**: `scripts/data/processed-entries.json`
- **Skipped Entries**: `scripts/data/skipped-entries.json`
- **RSS Cache**: `scripts/data/rss-cache/`

## Never Do

- Never use backslashes for escaping
- Never replicate production logic in tests
- Never install dependencies in scripts (use `.devcontainer/post-create.sh`)
- Never add functions only for tests
- Never leave PowerShell scripts without proper error handling
