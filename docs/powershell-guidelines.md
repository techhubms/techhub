# PowerShell Development Guidelines

This document provides comprehensive reference material for PowerShell development in the Tech Hub project. For action-oriented guidance and critical rules, see [scripts/AGENTS.md](../scripts/AGENTS.md).

## Overview

PowerShell scripts in this project handle RSS feed processing, content transformation, AI integration, infrastructure deployment, and test automation. All scripts are located in the `scripts/` directory.

## Project-Specific Patterns

### Workspace Directory Handling

Scripts must support two execution contexts: local development and GitHub Actions. This pattern is used throughout the codebase:

```powershell
param(
    [Parameter(Mandatory = $false)]
    [string]$WorkspaceDirectory = $PSScriptRoot
)

# Determine correct paths based on execution context
$functionsPath = if ($WorkspaceDirectory -eq $PSScriptRoot) {
    Join-Path $PSScriptRoot "functions"
} else {
    Join-Path $WorkspaceDirectory "scripts/functions"
}
```

This allows scripts to find their dependencies whether run from the script directory or from the workspace root.

## Key Scripts

### RSS Processing Scripts

- **download-rss-feeds.ps1**: Downloads and caches RSS feed data
- **process-rss-to-markdown.ps1**: Converts RSS data to markdown content files with AI assistance
- **fix-markdown-files.ps1**: Repairs markdown formatting issues
- **remove-deleted-reddit-posts.ps1**: Cleans up deleted Reddit content

### Infrastructure and Testing

- **Deploy-Infrastructure.ps1**: Azure infrastructure deployment via Bicep
- **run-*-tests.ps1**: Test execution scripts for each framework (Pester, Jest, RSpec, Playwright)
- **run-all-tests.ps1**: Orchestrates complete test suite

### Helper Functions

Located in `scripts/functions/`, these provide reusable functionality:
- **Get-SourceRoot.ps1**: Repository root detection
- **Invoke-AiApiCall.ps1**: AI API interaction with retry logic
- **Convert-RssToMarkdown.ps1**: RSS to markdown transformation
- **Feed.ps1**: PowerShell class for RSS feed operations

## PowerShell Testing with Pester

For detailed testing patterns and examples, see [spec/AGENTS.md](../spec/AGENTS.md).

### Test Organization

Tests use a standardized initialization pattern with `Initialize-BeforeAll.ps1` and `Initialize-BeforeEach.ps1` scripts that provide:

- Consistent function loading
- Common variable initialization
- Test isolation between runs
- Mock setup infrastructure

### Test Execution

```powershell
# All PowerShell tests
./scripts/run-powershell-tests.ps1

# Specific test file
./scripts/run-powershell-tests.ps1 -TestPath "spec/powershell/Get-FilteredTags.Tests.ps1"

# With coverage
./scripts/run-powershell-tests.ps1 -Coverage
```

## Resources

- [scripts/AGENTS.md](../scripts/AGENTS.md) - Action-oriented PowerShell development guidance
- [spec/AGENTS.md](../spec/AGENTS.md) - Testing strategy and patterns
- [rss-feeds.md](rss-feeds.md) - RSS processing documentation
