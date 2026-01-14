---
name: cleanup
description: Comprehensive code cleanup and quality assurance skill. Compiles, formats, lints, and synchronizes code with documentation. Use when asked to clean the entire solution or repository.
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

## Cleanup Process Overview

Execute cleanup tasks in this order for best results:

1. **Build & Test Verification** - Ensure code compiles and all tests pass
2. **Code Formatting** - Apply consistent formatting
3. **Generate Quality Overview** - Analyze warnings/errors and create overview
4. **Fix or Suppress Issues** - Address issues based on overview recommendations
5. **Dead Code Removal** - Remove unused code
6. **Documentation Sync** - Update docs to match code
7. **Final Validation** - Verify all checks pass

## Detailed Instructions

Follow these steps in order. Do not skip steps or proceed if a step fails.

### Step 1: Build and Test Verification

**Execute**: [`./run.ps1 -Clean -OnlyTests`](../../../../run.ps1)

**Purpose**: Ensure the solution compiles without errors and all tests pass before making any changes.

**Requirements**:

- ✅ Build must succeed with 0 errors
- ✅ All tests must pass

**If this step fails**:

1. Fix all compilation errors first
2. Fix all failing tests
3. Re-run this step until it passes
4. Do NOT proceed to step 2 until this step succeeds

---

### Step 2: Code Formatting

**Execute**: [`./scripts/format-code.ps1`](./scripts/format-code.ps1)

**Purpose**: Apply consistent code formatting to all C# files using `dotnet format`.

**What it does**:

- Formats all C# files according to `.editorconfig` rules
- Applies formatting from `Directory.Build.props`
- Ensures consistent style across the codebase

**Requirements**:

- ✅ Formatting completes successfully

---

### Step 3: Generate Quality Overview

**Execute**: [`./scripts/analyze-code-quality.ps1`](./scripts/analyze-code-quality.ps1)

**Purpose**: Build the solution and analyze all warnings, errors, and code quality issues.

**Output**: Creates a detailed overview file using the [overview template](./templates/overview-template.md).

**What it reports**:

- Compilation errors and warnings categorized by priority
- Analyzer issues (CA*, IDE*, RZ*, etc.)
- Recommendations for fix vs suppress decisions
- File locations with line numbers

**Requirements**:

- ✅ Overview file is generated successfully
- 📋 Review the overview before proceeding

**Action Required**:

- Read the generated overview
- Decide which issues to fix and which to suppress
- Discuss with user if needed

---

### Step 4: Fix or Suppress Issues

**Based on the overview from Step 3**, make code changes:

**For High Priority Issues**:

- Fix immediately - these are typically bugs or critical quality issues
- Use appropriate tools (`replace_string_in_file`, `multi_replace_string_in_file`)
- Fix compilation errors, null reference issues, disposable patterns, etc.

**For Medium Priority Issues**:

- Fix if straightforward
- Suppress if false positive or intentional design choice

**For Low Priority Issues**:

- Suppress via `.editorconfig` to reduce noise
- Add comments explaining why suppression is appropriate

**After making changes**:

- Run [`./run.ps1 -OnlyTests`](../../../../run.ps1) to verify changes
- Use `get_errors` tool to check for new VS Code diagnostics
- Fix any new issues introduced by changes

---

### Step 5: Dead Code Removal

**Execute**: [`./scripts/find-dead-code.ps1`](./scripts/find-dead-code.ps1)

**Purpose**: Identify unused code, imports, members, and CSS classes.

**What it finds**:

- Unused using statements (IDE0005)
- Unused private members (methods, fields, properties)
- Commented-out code blocks
- Unused CSS classes

**Review output carefully**:

- Verify findings before removing code
- Check for reflection usage or dynamic code
- Use `grep_search` to confirm code is truly unused
- Look for false positives (see [Common False Positives](#common-false-positives))

**To remove dead code after verification**:

1. Review the findings from the script output
2. Verify suspicious items manually using `grep_search` or `read_file`
3. Execute [`./scripts/find-dead-code.ps1 -Fix`](./scripts/find-dead-code.ps1) to automatically remove confirmed dead code

**After removing**:

- Run [`./run.ps1 -OnlyTests`](../../../../run.ps1) to verify nothing broke
- Use `get_errors` to check for new compilation issues

---

### Step 6: Documentation Synchronization

**Execute**: [`./scripts/verify-documentation.ps1`](./scripts/verify-documentation.ps1)

**Purpose**: Ensure documentation matches current code implementation.

**What it checks**:

- API documentation matches actual endpoints
- Code comments are accurate
- AGENTS.md patterns match current code
- README.md instructions work

**Update as needed**:

- Sync outdated documentation
- Add missing documentation for new features
- Remove documentation for deleted code

---

### Step 7: Final Validation

**Execute**: [`./run.ps1 -Clean -OnlyTests`](../../../../run.ps1)

**Purpose**: Final verification that all changes are correct and nothing is broken.

**Requirements**:

- ✅ Build succeeds with 0 errors
- ✅ Warnings are reduced or only intentional suppressions remain
- ✅ All tests pass
- ✅ No new VS Code diagnostics introduced

**Validation Checklist**:

- [ ] Solution builds without errors
- [ ] Warnings significantly reduced (or properly suppressed)
- [ ] All tests pass (same or better than Step 1)
- [ ] Code formatting verified: [`./scripts/format-code.ps1 -Verify`](./scripts/format-code.ps1)
- [ ] PowerShell tests pass: [`../../scripts/run-powershell-tests.ps1`](../../../../scripts/run-powershell-tests.ps1)
- [ ] No new VS Code diagnostics (`get_errors` tool)

---

## Output Format

After completing all steps, provide a summary report:

```markdown
## Cleanup Complete

**Date**: {Date}  
**Branch**: {Branch}

### Results

| Metric | Before | After | Change |
|--------|--------|-------|--------|
| Compilation Errors | {N} | 0 | ✅ -{N} |
| Warnings | {N} | {N} | ✅ -{N} / ⚠️ {N} suppressed |
| Test Failures | {N} | 0 | ✅ -{N} |
| Dead Code Items | {N} | 0 | ✅ Removed {N} |

### Changes Made

- Fixed {N} high priority issues
- Fixed {N} medium priority issues  
- Suppressed {N} low priority warnings
- Removed {N} dead code items
- Updated {N} documentation files

### Quality Overview

See detailed analysis: [Code Quality Overview](.tmp/code-quality-overview.md)

### Validation

- ✅ Build succeeds
- ✅ All tests pass
- ✅ Formatting verified
- ✅ Documentation synchronized
```

---

## Important Rules

### Do NOT Remove

- Code marked with `// Intentional:` comments
- Suppressed warnings with documented reasons in `.editorconfig`
- Test helpers that appear unused (used via reflection/xUnit discovery)
- Reflection-based code (check for attributes)
- Configuration-driven code paths

### Ask Before Removing

- Public API members (may be used by external consumers)
- Code in `#if DEBUG` or `#if RELEASE` blocks
- Blazor component parameters (used in Razor syntax)
- Minimal API endpoint handlers (registered at startup)

### Common False Positives

- xUnit test methods appear unused but are discovered by reflection
- Blazor component parameters appear unused but are set via Razor
- Minimal API handlers appear unused but are registered in `Program.cs`
- Private fields only used in `Dispose()` methods

---

## Related Resources

**Scripts**:

- [format-code.ps1](./scripts/format-code.ps1) - Code formatting
- [analyze-code-quality.ps1](./scripts/analyze-code-quality.ps1) - Quality analysis and overview generation
- [find-dead-code.ps1](./scripts/find-dead-code.ps1) - Dead code detection
- [verify-documentation.ps1](./scripts/verify-documentation.ps1) - Documentation verification

**Templates**:

- [overview-template.md](./templates/overview-template.md) - Standard format for quality overviews

**Project Scripts**:

- [run.ps1](../../../../run.ps1) - Build, test, and run the solution
- [run-powershell-tests.ps1](../../../../scripts/run-powershell-tests.ps1) - PowerShell script validation
