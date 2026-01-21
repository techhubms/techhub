---
name: cleanup
description: Comprehensive code cleanup and quality assurance for .NET solutions. Compiles, formats, lints, removes dead code, and synchronizes documentation with code.
---

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

**Run**: `Run -Clean -StopServers` (use `isBackground=true`, monitor with `get_terminal_output`)

**Requirements**: 0 errors, all tests pass.

**If tests fail**: Read [Starting, Stopping and Testing the Website](/workspaces/techhub/AGENTS.md#starting-stopping-and-testing-the-website) in root AGENTS.md to understand proper test execution, debugging workflows, and using `Run` parameters. Fix all issues and retry before Step 2.

---

### Step 2: Code Formatting

**Run**: [format-code.ps1](.github/skills/cleanup/scripts/format-code.ps1)

Applies `dotnet format` per `.editorconfig` rules.

---

### Step 3: Code Quality Analysis

**Script**: [analyze-code-quality.ps1](.github/skills/cleanup/scripts/analyze-code-quality.ps1)  
**Template**: [overview-template.md](.github/skills/cleanup/templates/overview-template.md)

- Script outputs warnings/errors by priority (High/Medium/Low)
- Analyze the data and provide smart recommendations
- Present summary in chat with your recommendations using template
- Wait for user input

---

### Step 4: Fix or Suppress

Apply Step 3 decisions. If there was nothing to improve immediately go to step 5.

---

### Step 5: Dead Code Detection

**Script**: [find-dead-code.ps1](.github/skills/cleanup/scripts/find-dead-code.ps1)  
**Template**: [dead-code-report-template.md](.github/skills/cleanup/templates/dead-code-report-template.md)

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

The project uses **Roslynator.Analyzers** (4.12.11+) for build-time dead code detection:

- **Package**: Only `Roslynator.Analyzers` in `Directory.Build.props` (removed Microsoft.CodeAnalysis.NetAnalyzers to avoid duplication)
- **Configuration**: `.editorconfig` suppresses all Roslynator rules by default, then enables only dead code detection:
  - `IDE0051`: Unused private member detection
  - `IDE0052`: Unread private member detection  
  - `IDE0060`: Remove unused parameter
  - `RCS1163`: Unused parameter (more comprehensive than IDE0060)
  - `RCS1213`: Unused member declaration (catches public unused methods in sealed classes)
  - `RCS1175`: Unused 'this' parameter
- **File-Specific Suppressions**: `src/TechHub.Api/Endpoints/*.cs` suppresses unused parameter warnings (intentional injected dependencies)
- **Roslynator Config**: `.roslynatorconfig` enables refactorings, compiler fixes, disables formatting (defer to .editorconfig)

**Why Roslynator Only?**

- More comprehensive (500+ rules vs Microsoft's ~300)
- Detects public unused members (RCS1213) that Microsoft analyzers miss
- Single source of truth simplifies configuration and reduces warning duplication
- Proper suppression strategy keeps noise low while maintaining coverage

---

### Step 5a: Project Analysis

**No script** - YOU analyze.  
**Template**: [project-analysis-template.md](.github/skills/cleanup/templates/project-analysis-template.md)

1. Analyze implementation (endpoints, components, services, models)
2. Analyze documentation (API spec, AGENTS.md, functional docs)
3. Analyze tests (unit, integration, component, E2E)
4. Identify gaps (undocumented, untested, stale)
5. Present summary in chat with your recommendations using template
6. Wait for user decision

---

### Step 6: Documentation Review

**Script**: [verify-documentation.ps1](.github/skills/cleanup/scripts/verify-documentation.ps1)

- Script lists all docs
- YOU read each, compare with Step 5a analysis
- Find issues: broken links, outdated examples, missing features, dupes, incomplete cross references
- Present summary in chat with your recommendations

---

### Step 7: Test Review

**No script** - YOU analyze.  
**Template**: [test-review-template.md](.github/skills/cleanup/templates/test-review-template.md)  
**Read**: [tests/AGENTS.md](../../tests/AGENTS.md)

- Scan tests for violations (AAA, naming, positioning, coverage)
- Analyze quality and assign grade (A-F)
- Present summary in chat with your recommendations using template
- Wait for user approval

---

### Step 8: Best Practices

**No script** - YOU use `grep_search`.  
**Template**: [best-practices-template.md](.github/skills/cleanup/templates/best-practices-template.md)

- `grep_search` for anti-patterns
- Read files to verify context
- Analyze findings and categorize by severity
- Present summary in chat with your recommendations using template
- Wait for user approval

**Search**: `.Count(?!\(\))`, `.Result|.Wait\(\)`, `async void`, `catch.*\{\s*\}`

---

### Step 9: Final Validation

**Run to verify nothing broke**: Execute `Run -Clean -StopServers`

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
