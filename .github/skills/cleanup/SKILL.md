---
name: cleanup
description: Comprehensive code cleanup and quality assurance skill. Compiles, formats, lints, and synchronizes code with documentation. Use when asked to clean up code, fix build warnings, format files, lint code, synchronize documentation with code, remove unused code, or ensure coding standards compliance.
license: MIT
compatibility: Requires .NET 10+, PowerShell 7+. Works with GitHub Copilot coding agent, CLI, and VS Code.
metadata:
  author: techhubms
  version: "1.0"
  category: code-quality
allowed-tools: Bash(dotnet:*) Bash(pwsh:*) Read Write
---

# Code Cleanup Skill

You are a code and documentation cleanup agent for the Tech Hub .NET/Blazor project. This skill helps maintain code quality, consistency, and documentation accuracy across the codebase.

## When to Use This Skill

Activate this skill when the user asks to:

- Clean up code or fix code quality issues
- Format code files
- Fix build warnings or errors
- Lint code (C#, PowerShell, Markdown)
- Synchronize documentation with code
- Remove unused variables, imports, or dead code
- Ensure coding standards compliance
- Run quality checks before committing

## Cleanup Process Overview

Execute cleanup tasks in this order for best results:

1. **Build Verification** - Ensure code compiles without errors
2. **Code Formatting** - Apply consistent formatting
3. **Linting** - Fix analyzer warnings and style issues
4. **Dead Code Removal** - Remove unused code
5. **Documentation Sync** - Update docs to match code
6. **Final Validation** - Verify all checks pass

## Detailed Instructions

### Step 1: Build Verification

First, ensure the solution builds without errors:

```bash
# Build the entire solution
dotnet build TechHub.slnx --no-incremental

# If build fails, fix compilation errors before proceeding
```

**Handle build failures by**:

- Reading error messages carefully
- Fixing syntax errors and missing references
- Resolving type mismatches and null reference issues
- Checking for missing using statements

### Step 2: Code Formatting

Apply consistent formatting using `dotnet format`:

```bash
# Format all C# files in the solution
dotnet format TechHub.slnx

# Format a specific project
dotnet format src/TechHub.Api/TechHub.Api.csproj

# Format with verification (dry-run)
dotnet format TechHub.slnx --verify-no-changes
```

**Formatting Rules** (from Directory.Build.props):

- `EnforceCodeStyleInBuild` is enabled
- `AnalysisLevel` is set to `latest-all`
- `AnalysisMode` is set to `All`

### Step 3: Linting and Analysis

Run analyzers and fix warnings:

```bash
# Build with all analyzers enabled (warnings shown)
dotnet build TechHub.slnx -warnaserror

# Run analyzers on specific project
dotnet build src/TechHub.Web/TechHub.Web.csproj /p:TreatWarningsAsErrors=true
```

**For PowerShell scripts**, use the validation script:

```bash
# Run PowerShell linting via Pester tests
pwsh -File scripts/run-powershell-tests.ps1
```

**For Markdown files**, check VS Code diagnostics using `get_errors` tool.

### Step 4: Dead Code Removal

Identify and remove unused code:

**Find unused usings**:

```bash
dotnet format TechHub.slnx --diagnostics IDE0005
```

**Common dead code patterns to look for**:

- Unused private fields and methods
- Commented-out code blocks
- Unreachable code after return/throw
- Unused parameters (IDE0060)
- Empty catch blocks without logging

**Use `grep_search` to verify before removing**:

- Search for usages of suspicious symbols
- Check if "unused" code is used via reflection or configuration

### Step 5: Documentation Synchronization

Ensure documentation matches code reality:

**Check documentation consistency**:

1. **API documentation** - Verify `docs/api-specification.md` matches actual endpoints
2. **Code comments** - Update XML documentation on public APIs
3. **AGENTS.md files** - Ensure patterns match current implementation
4. **README.md** - Verify quick start instructions work

**Run the documentation check script**:

```bash
pwsh -File scripts/cleanup/verify-documentation.ps1
```

See [references/DOCUMENTATION-CHECKLIST.md](references/DOCUMENTATION-CHECKLIST.md) for the full checklist.

### Step 6: Final Validation

Run all tests to ensure cleanup didn't break anything:

```bash
# Run all tests (unit, integration, component)
dotnet test TechHub.slnx

# Or use the project's run script
pwsh -File run.ps1 -OnlyTests
```

**Validation Checklist**:

- [ ] Solution builds without errors
- [ ] Solution builds without warnings (or only intentional suppressions)
- [ ] All tests pass
- [ ] `dotnet format --verify-no-changes` passes
- [ ] PowerShell tests pass
- [ ] No new VS Code diagnostics

## Project-Specific Patterns

### C# Naming Conventions

- **Interfaces**: `I` prefix (e.g., `IContentRepository`)
- **Private fields**: `_camelCase` with underscore prefix
- **Constants**: `PascalCase`
- **Local variables**: `camelCase`
- **Async methods**: `Async` suffix

### File Organization

```text
src/
├── TechHub.Api/        # Minimal API endpoints
├── TechHub.Web/        # Blazor SSR + WASM components
├── TechHub.Core/       # Domain models and interfaces
├── TechHub.Infrastructure/  # Repository implementations
└── TechHub.AppHost/    # .NET Aspire orchestration
```

### Documentation Locations

| Type | Location |
| ---- | -------- |
| Development patterns | `**/AGENTS.md` files |
| Functional docs | `docs/` directory |
| Content guidelines | `collections/*.md` |
| API specification | `docs/api-specification.md` |

## Output Format

After completing cleanup, report results using this template:

```markdown
## Cleanup Results

### Build Status
- **Compilation**: ✅ Success / ❌ Failed (X errors)
- **Warnings**: X warnings (Y fixed, Z suppressed)

### Formatting
- **Files formatted**: X files
- **Changes made**: Y changes

### Linting
- **Analyzer issues**: X found, Y fixed
- **Remaining**: Z (intentional suppressions)

### Dead Code
- **Unused usings removed**: X
- **Unused code removed**: Y items

### Documentation
- **Files checked**: X
- **Updates made**: Y

### Tests
- **Total**: X tests
- **Passed**: Y ✅
- **Failed**: Z ❌

### Summary
[Brief description of what was cleaned and any remaining issues]
```

## Edge Cases and Warnings

### Do NOT Remove

- Code marked with `// Intentional:` comments
- Suppressed warnings with documented reasons
- Test helpers that appear unused but are used dynamically
- Reflection-based code (check attributes)

### Ask Before Removing

- Public API members (may be used externally)
- Configuration-driven code paths
- Code in `#if DEBUG` blocks

### Common False Positives

- xUnit test methods (appear unused but discovered by reflection)
- Blazor component parameters (used via Razor syntax)
- Minimal API endpoint handlers (registered at startup)

## Related Resources

- [references/DOCUMENTATION-CHECKLIST.md](references/DOCUMENTATION-CHECKLIST.md) - Full documentation sync checklist
- [scripts/verify-documentation.ps1](scripts/verify-documentation.ps1) - Automated documentation verification
- [scripts/find-dead-code.ps1](scripts/find-dead-code.ps1) - Dead code detection helper
