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
The module is auto-loaded in devcontainer terminals.

```powershell
# Validation workflows
Run                              # Build + all tests + servers (default validation)
Run -TestRerun                   # Fast: Only rebuild and rerun tests (after fixes)

# Scoped testing (when working on specific areas)
Run -TestProject powershell                      # PowerShell tests only (fast - no .NET build)
Run -TestProject Web.Tests                       # Web component tests only
Run -TestProject E2E.Tests                       # E2E tests only
Run -TestName SectionCard                        # Tests matching 'SectionCard'
Run -TestProject E2E.Tests -TestName Navigation  # E2E navigation tests
```

## PowerShell Script Tests

```powershell
# Run PowerShell Pester tests via Run function (module is auto-loaded)
Run -TestProject powershell

# Run with test name filter
Run -TestProject powershell -TestName "FrontMatter"
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

# 4. Run all tests to validate changes
pwsh -Command "Run"

# 5. After making fixes, rerun tests quickly
pwsh -Command "Run -TestRerun"

# 6. Verify documentation
pwsh -File .github/skills/cleanup/scripts/verify-documentation.ps1

# 7. Final verification
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
