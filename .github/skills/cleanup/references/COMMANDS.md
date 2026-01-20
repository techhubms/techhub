# Cleanup Commands Quick Reference

Quick reference for all cleanup-related commands in the Tech Hub project.

## Build Commands

```bash
# Build entire solution
dotnet build TechHub.slnx

# Clean build (remove all artifacts first)
dotnet clean TechHub.slnx && dotnet build TechHub.slnx

# Build with warnings as errors
dotnet build TechHub.slnx -warnaserror

# Build specific project
dotnet build src/TechHub.Api/TechHub.Api.csproj
dotnet build src/TechHub.Web/TechHub.Web.csproj
```

## Formatting Commands

```bash
# Format entire solution
dotnet format TechHub.slnx

# Format specific project
dotnet format src/TechHub.Api/TechHub.Api.csproj

# Verify formatting (dry-run, exits with error if changes needed)
dotnet format TechHub.slnx --verify-no-changes

# Format with detailed output
dotnet format TechHub.slnx --verbosity detailed

# Fix specific diagnostic
dotnet format TechHub.slnx --diagnostics IDE0005  # Unused usings
dotnet format TechHub.slnx --diagnostics IDE0060  # Unused parameters
```

## Analyzer Commands

```bash
# Run analyzers during build
dotnet build TechHub.slnx /p:RunAnalyzersDuringBuild=true

# Build with specific analyzer severity
dotnet build TechHub.slnx /p:AnalysisLevel=latest-all

# Get analyzer output as SARIF
dotnet build TechHub.slnx /p:ErrorLog=analysis.sarif
```

## Test Commands

**Use the Run function** (see [AGENTS.md - Using the Run Function](../../../AGENTS.md#using-the-run-function) for complete documentation).
The module is auto-loaded in devcontainer terminals. Only use `Import-Module` in CI or after making changes to the module.

```powershell
# Common workflows
Run                              # Build + all tests + servers
Run -WithoutTests                # Build + servers (no tests)
Run -WithoutClean                # Build + all tests + servers (faster)
Run -Rebuild                     # Clean rebuild only

# Testing workflows (run tests, then exit)
Run -OnlyTests                                   # All tests, then exit
Run -OnlyTests -TestProject powershell           # PowerShell tests only (fast - no .NET build)
Run -OnlyTests -TestProject Web.Tests            # Web component tests only
Run -OnlyTests -TestProject E2E.Tests            # E2E tests only
Run -OnlyTests -TestName SectionCard             # Tests matching 'SectionCard'
Run -OnlyTests -TestProject E2E.Tests -TestName Navigation  # E2E navigation tests
```

**Low-level dotnet test commands** (for reference only - use Run instead):

```bash
# ⚠️ WARNING: Use 'Run' function for normal workflows.
# E2E tests WILL FAIL if you run them directly without servers.

# Run all tests
dotnet test TechHub.slnx

# Run with verbose output
dotnet test TechHub.slnx --verbosity normal

# Run specific test project  
dotnet test tests/TechHub.Core.Tests/TechHub.Core.Tests.csproj

# Run tests matching a filter
dotnet test TechHub.slnx --filter "FullyQualifiedName~ContentItem"
```

## PowerShell Script Tests

```powershell
# Run PowerShell Pester tests via Run function (module is auto-loaded)
Run -OnlyTests -TestProject powershell

# Run with test name filter
Run -OnlyTests -TestProject powershell -TestName "FrontMatter"
```

## Cleanup Skill Scripts

```bash
# Verify documentation
pwsh -File .github/skills/cleanup/scripts/verify-documentation.ps1

# Output as Markdown
pwsh -File .github/skills/cleanup/scripts/verify-documentation.ps1 -OutputFormat Markdown

# Find dead code
pwsh -File .github/skills/cleanup/scripts/find-dead-code.ps1

# Find only unused usings
pwsh -File .github/skills/cleanup/scripts/find-dead-code.ps1 -Category Usings
```

## Common Diagnostic IDs

| ID | Description | Command to Fix |
| -- | ----------- | -------------- |
| IDE0005 | Remove unnecessary using | `dotnet format --diagnostics IDE0005` |
| IDE0051 | Remove unused private member | Manual review required |
| IDE0052 | Remove unread private member | Manual review required |
| IDE0059 | Unnecessary assignment | `dotnet format --diagnostics IDE0059` |
| IDE0060 | Remove unused parameter | Manual review required |
| CA1822 | Mark member as static | `dotnet format --diagnostics CA1822` |
| CA2007 | ConfigureAwait | `dotnet format --diagnostics CA2007` |

## Full Cleanup Workflow

```bash
# 1. Clean and rebuild
dotnet clean TechHub.slnx
dotnet build TechHub.slnx

# 2. Format code
dotnet format TechHub.slnx

# 3. Fix unused usings
dotnet format TechHub.slnx --diagnostics IDE0005

# 4. Run all tests
pwsh -Command "Run"

# 5. Verify documentation
pwsh -File .github/skills/cleanup/scripts/verify-documentation.ps1

# 6. Final verification
dotnet format TechHub.slnx --verify-no-changes
dotnet build TechHub.slnx -warnaserror
```

## Git Commands for Cleanup

```bash
# Stage all changes
git add -A

# Interactive staging (review changes)
git add -p

# Check what would be committed
git status

# Commit cleanup changes
git commit -m "chore: code cleanup and formatting"

# Amend if needed
git commit --amend
```
