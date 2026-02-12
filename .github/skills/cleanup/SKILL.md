---
name: cleanup
description: Comprehensive code cleanup and quality assurance for .NET solutions. Compiles, formats, lints, removes dead code, and synchronizes documentation with code. Use this skill whenever you are asked to cleanup the solution, repository, branch, project, etc.
---

> **Path Convention**: All paths in this file are relative to this file's location (`.github/skills/cleanup/`).  
> Therefore, `../../../` refers to the workspace root (`/workspaces/techhub/`).

# Code Cleanup Skill

Maintain code quality, consistency, and documentation accuracy for Tech Hub.

## 🎯 Core Workflow

Each step:

1. **Run script** (link provided)
2. **Read template if exists** (link provided)
3. **Present findings in chat** (don't write to file unless specifically useful)
4. **Wait for user input** (template guides options)

Scripts output raw data. YOU analyze and format using templates.

## Cleanup Steps

Execute in order. Don't skip or proceed if a step fails.

### Step 1: Build & Test

**Run**: `Run -Clean` with `IsBackground: false` to verify a clean build and test pass.

**Requirements**: 0 errors, all tests pass.

**If tests fail**: Read [docs/running-and-testing.md](../../../docs/running-and-testing.md) to understand proper test execution, debugging workflows, and using `Run` parameters. Fix all issues and retry before Step 2.

---

### Step 2: Code Formatting

**Run**: [format-code.ps1](scripts/format-code.ps1)

Applies `dotnet format` per `.editorconfig` rules.

---

### Step 2a: CSS Token Analysis

**Script 1**: [Count-TokenUsage.ps1](scripts/Count-TokenUsage.ps1)  
**Script 2**: [Find-UndefinedTokens.ps1](scripts/Find-UndefinedTokens.ps1)  
**Template**: [css-token-analysis-template.md](templates/css-token-analysis-template.md)

- Script 1 counts usage of each CSS color token defined in design-tokens.css
- Script 2 finds CSS color references that don't exist in design-tokens.css
- Analyze the data to identify:
  - Unused tokens (candidates for removal)
  - Single-use tokens (consolidation opportunities)
  - Undefined references (bugs - must fix)
- Present summary in chat with your recommendations using template
- Wait for user input

**Color Token Categories**:
- **Multi-use (2+)**: Core design tokens, keep these
- **Single-use (1)**: Review if they should be consolidated with existing tokens
- **Unused (0)**: Safe to remove unless reserved for future use
- **Undefined**: BUGS - references that must be fixed or removed

---

### Step 3: Code Quality Analysis

**Script**: [analyze-code-quality.ps1](scripts/analyze-code-quality.ps1)  
**Template**: [overview-template.md](templates/overview-template.md)

- Script outputs warnings/errors by priority (High/Medium/Low)
- Analyze the data and provide smart recommendations
- Present summary in chat with your recommendations using template
- Wait for user input

---

### Step 4: Fix or Suppress

Apply Step 3 decisions. If there was nothing to improve immediately go to step 5.

---

### Step 5: Dead Code Detection

**Script**: [find-dead-code.ps1](scripts/find-dead-code.ps1)  
**Template**: [dead-code-report-template.md](templates/dead-code-report-template.md)

- Script finds unused members, commented code, unused CSS
- Analyze findings, identify false positives
- Present summary in chat with your recommendations using template
- Wait for user approval

**Tech Hub Architecture Patterns** (script auto-filters these):

1. **Minimal API Endpoint Handlers** (`src/TechHub.Api/Endpoints/*.cs`):
   - Methods registered via `MapGet()`, `MapPost()`, etc. appear "unused"
   - Static analysis can't detect delegate method group references
   - **Script behavior**: Automatically skips these files

2. **Blazor Event Handlers** (`*.razor.cs`):
   - Methods bound via `@onclick`, `@onchange`, etc. appear "unused"
   - Static analysis can't detect Blazor event binding expressions
   - **Script behavior**: Automatically skips these files

3. **Unused Parameters** (common in these scenarios):
   - **Minimal API handlers**: Injected dependencies for future use
   - **Middleware constructors**: Standard ASP.NET Core parameters (next, logger, environment)
   - **ContentFixer utility**: Placeholder parameters for planned features
   - **Script behavior**: Automatically skips Endpoints/, Middleware, ContentFixer files

4. **Build Artifacts**:
   - Compiler-generated code in `bin/`, `obj/`, `.tmp/` directories
   - **Script behavior**: Excluded via `Get-SourceFiles` helper function

**If you see false positives**, these patterns explain why. Verify the file location matches one of the above before reporting as dead code.

**Roslyn Analyzer Configuration**:

The project uses **built-in .NET analyzers** for build-time dead code detection configured in `.editorconfig`:

- `IDE0051`: Unused private member detection
- `IDE0052`: Unread private member detection  
- `IDE0060`: Remove unused parameter
- **File-Specific Suppressions**: `src/TechHub.Api/Endpoints/*.cs` suppresses unused parameter warnings (intentional injected dependencies)

**Analysis Settings**: Configured in `Directory.Build.props`:
- `AnalysisLevel`: latest-all
- `AnalysisMode`: All  
- Documentation generation enabled for comprehensive IntelliSense

---

### Step 5a: Project Analysis

**No script** - YOU analyze.  
**Template**: [project-analysis-template.md](templates/project-analysis-template.md)

1. Analyze implementation (endpoints, components, services, models)
2. Analyze documentation (API spec, AGENTS.md, functional docs)
3. Analyze tests (unit, integration, component, E2E)
4. Identify gaps (undocumented, untested, stale)
5. Present summary in chat with your recommendations using template
6. Wait for user decision

---

### Step 6: Documentation Review

**Step 6a: Generate Documentation Index**

**Run**: `../../../scripts/Generate-DocumentationIndex.ps1`

This creates [docs/documentation-index.md](../../../docs/documentation-index.md) containing:
- All documentation files (AGENTS.md, README.md, docs/*.md)
- All H1/H2/H3 headers from each file
- Relative links to each file

**Step 6b: Run Documentation Quality Checks**

**Run**: [verify-documentation.ps1](scripts/verify-documentation.ps1)

This performs quality checks:
- **Expected Files**: Verifies presence of key documentation files
- **Broken Links**: Detects broken internal markdown links
- **Missing AGENTS.md**: Checks if code directories lack AGENTS.md files

**Step 6c: Analyze Documentation**

**No script** - YOU analyze using both outputs.  
**Read**: [docs/documentation-index.md](../../../docs/documentation-index.md)

Use the index + verification results to:
- Verify all features are documented (compare with Step 5a)
- Check documentation is in the right location
- Identify duplicate or overlapping documentation
- Fix any broken links found by verify script
- Ensure missing AGENTS.md files are created if needed
- Ensure cross-references are complete

Present summary in chat with your recommendations.

---

### Step 7: Test Review

**No script** - YOU analyze.  
**Template**: [test-review-template.md](templates/test-review-template.md)  
**Read**: [tests/AGENTS.md](../../../tests/AGENTS.md)

- Scan tests for violations (AAA, naming, positioning, coverage)
- Analyze quality and assign grade (A-F)
- Present summary in chat with your recommendations using template
- Wait for user approval

---

### Step 8: Best Practices

**No script** - YOU use `grep_search`.  
**Template**: [best-practices-template.md](templates/best-practices-template.md)

- `grep_search` for anti-patterns
- Read files to verify context
- Analyze findings and categorize by severity
- Present summary in chat with your recommendations using template
- Wait for user approval

**Search**: `.Count(?!\(\))`, `.Result|.Wait\(\)`, `async void`, `catch.*\{\s*\}`

---

### Step 9: Final Validation

**Run to verify nothing broke**: Execute `Run -Clean`

**Checklist**:

- [ ] 0 errors
- [ ] Reduced warnings
- [ ] All tests pass
- [ ] No new diagnostics

Report success or issues.

---

## Rules

**Don't Remove**:

- `// Intentional:` comments
- Documented `.editorconfig` suppressions
- Test helpers (xUnit discovery)
- Blazor parameters
- Minimal API handlers

**Ask First**: Public APIs, debug code, component parameters
