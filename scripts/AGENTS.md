# PowerShell Development Agent

> **AI CONTEXT**: This is a **LEAF** context file for the `scripts/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Critical PowerShell Rules

### ✅ Always Do

- **Use backticks for escaping** (`` ` ``), NEVER backslashes
- **Use subexpressions `$(...)` for complex interpolations** (dotted notation, array access, type casting)
- **Test ALL script changes** - Run `scripts/run-powershell-tests.ps1` after modifications
- **Set error handling** - `$ErrorActionPreference = "Stop"` at script start
- **Use strict mode** - `Set-StrictMode -Version Latest`
- **Handle two execution contexts** - Script directory vs workspace root
- **Import error handling first** - Before other functions
- **Document parameters** with `[Parameter()]` attributes

### ⚠️ Ask First

- **Adding new PowerShell modules** or dependencies
- **Changing RSS feed structure** or processing logic
- **Modifying AI integration** patterns

### 🚫 Never Do

- **Never use backslashes for escaping** - Use backticks (`` ` ``)
- **Never escape dollar signs** - Use subexpressions `$(...)` instead
- **Never use dotted notation without subexpression** - `"$object.property"` is WRONG, use `"$($object.property)"`
- **Never replicate production logic in tests** - Test real functions
- **Never install dependencies in scripts** - Use `.devcontainer/post-create.sh`
- **Never add functions only for tests** - Test real implementation
- **Never leave scripts without error handling** - Always use try/catch
- **Never assume execution context** - Support both script dir and workspace root
- **Never skip testing after changes** - Run `scripts/run-powershell-tests.ps1`

## Overview

You are a PowerShell development specialist working with the Tech Hub's automation scripts. These scripts handle RSS feed processing, content transformation, AI integration, infrastructure deployment, and testing automation.

⚠️ **CRITICAL TESTING RULE**: After making ANY changes to PowerShell scripts with tests, run the PowerShell test suite in `scripts/run-powershell-tests.ps1` to validate your changes.

## When to Use This Guide

**Read this file when**:

- Writing or modifying PowerShell scripts in `scripts/` directory
- Creating automation for content processing
- Working with RSS feeds or AI integration
- Implementing build or deployment automation
- Debugging PowerShell-based workflows

**Related Documentation**:

- Testing PowerShell scripts → [tests/powershell/AGENTS.md](../tests/powershell/AGENTS.md)
- Content management → [collections/AGENTS.md](../collections/AGENTS.md)
- Azure infrastructure → [infra/main.bicep](../infra/main.bicep)

## Tech Stack

- **PowerShell**: 7+ (cross-platform)
- **Testing Framework**: Pester v5
- **Key Modules**: HtmlToMarkdown, Az (Azure)
- **AI Integration**: Azure AI Foundry

## Directory Structure

```text
scripts/
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

#### download-rss-feeds.ps1

- Downloads RSS feeds from configured sources
- Saves structured data to `scripts/data/rss-cache/`
- Tracks processed entries to avoid duplicates

#### process-rss-to-markdown.ps1

- Converts RSS data to markdown content files
- AI-powered content transformation and summarization
- Collection-aware prioritization
- Rate limit handling for AI APIs
- **Generates .NET frontmatter** - Uses `section_names` (normalized identifiers) instead of `categories` (display names)
- **Template-based output** - Uses [templates/template-generic.md](content-processing/templates/template-generic.md) and [templates/template-videos.md](content-processing/templates/template-videos.md)
- **Field mapping**: AI returns categories (display names) → converted to section_names (lowercase identifiers)
  - Example: `"GitHub Copilot"` → `"github-copilot"`, `"AI"` → `"ai"`, `".NET"` → `"dotnet"`

For complete frontmatter schema, see [collections/frontmatter-schema.md](../collections/frontmatter-schema.md).

#### fix-markdown-files.ps1

- **Purpose**: Fixes AI-generated markdown formatting issues ONLY
- **What it does**: Repairs markdown formatting (missing blank lines, heading spacing, list formatting, etc.)
- **What it does NOT do**: Modify frontmatter (templates already generate correct .NET format)
- **When to use**: After AI processes content that has markdown formatting issues
- **Note**: New content from RSS pipeline already has correct frontmatter structure (section_names)

#### iterative-roundup-generation.ps1

- AI-powered weekly roundup generation
- Multi-step iteration with validation
- Combines content from multiple sources

#### detect-repository-content-issues.ps1

- Validates markdown content across collections
- Checks for duplicates and similarity
- Verifies frontmatter structure

### Infrastructure

#### infra/Deploy-Infrastructure.ps1

- Azure infrastructure deployment via Bicep
- Three modes: validate, whatif, deploy
- Environment-specific parameters
- GitHub Actions compatible

### Testing

**PowerShell Script Testing**:

For PowerShell script testing (Pester v5), see:

- Content processing scripts → [tests/powershell/AGENTS.md](../tests/powershell/AGENTS.md)
- .NET testing → [tests/](../tests/) directory

**Running Tests**:

```bash
# All PowerShell tests for content processing scripts

./scripts/run-powershell-tests.ps1

# Specific test file

./scripts/run-powershell-tests.ps1 -TestFile "tests/powershell/Convert-RssToMarkdown.Tests.ps1"

# With coverage

./scripts/run-powershell-tests.ps1 -Coverage
```

## PowerShell Testing Standards

Use **Pester v5** for all PowerShell testing. For complete testing patterns, test organization, and critical testing rules, see [tests/powershell/AGENTS.md](../tests/powershell/AGENTS.md).

### Test File Location

```text
tests/powershell/
├── [ScriptName].Tests.ps1
├── Initialize-BeforeAll.ps1    # Standard setup for all tests
├── Initialize-BeforeEach.ps1   # Standard cleanup between tests
└── test-data/                  # Test data files
```

### Running Pester Tests

```bash
# All PowerShell tests (Content Processing Scripts)

./scripts/run-powershell-tests.ps1

# Specific test file

./scripts/run-powershell-tests.ps1 -TestFile "tests/powershell/Convert-RssToMarkdown.Tests.ps1"

# With coverage

./scripts/run-powershell-tests.ps1 -Coverage
```

## Common Functions

### Get-SourceRoot.ps1

Detects repository root directory, handling multiple execution contexts (devcontainer, GitHub Actions, local).

### Invoke-AiApiCall.ps1

Makes AI API calls with retry logic, rate limit handling, and error recovery. Supports both Azure OpenAI and GitHub Models.

### Convert-RssToMarkdown.ps1

Transforms RSS feed items into markdown files with .NET Tech Hub frontmatter structure.

**Key Features**:

- Generates `section_names` from AI-provided categories (normalized to lowercase identifiers)
- Removes deprecated Jekyll fields (`categories`, `tags_normalized`, `description`, `excerpt_separator`)
- Uses proper field ordering per [frontmatter-schema.md](../collections/frontmatter-schema.md)
- Applies markdown formatting repairs (NOT Jekyll repairs - new files already have correct frontmatter)
- Tracks processed/skipped entries to avoid reprocessing

**Section Name Normalization**:

```powershell
# AI returns display names, we convert to identifiers:
"AI" → "ai"
"GitHub Copilot" → "github-copilot"
".NET" → "dotnet"
"Azure" → "azure"
"DevOps" → "devops"
"Security" → "security"
"Coding" → "coding"
"Cloud" → "cloud"
```

**Template Variables** (both template-generic.md and template-videos.md):

- `{{TITLE}}` - Content title
- `{{AUTHOR}}` - Author name
- `{{CANONICAL_URL_FORMATTED}}` - Original URL (quoted)
- `{{FEEDNAME}}` - RSS feed name
- `{{FEEDURL}}` - RSS feed URL
- `{{DATE}}` - Publication date with timezone
- `{{PERMALINK}}` - URL path
- `{{TAGS}}` - Topic tags array
- `{{SECTION_NAMES}}` - Normalized section identifiers array
- `{{CONTENT}}` - Main content
- `{{EXCERPT}}` - Summary excerpt
- `{{YOUTUBE_ID}}` - YouTube video ID (videos only)

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
