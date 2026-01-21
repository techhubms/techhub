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
5a. **Project Analysis** - Comprehensively analyze implementation, docs, and tests to identify gaps
6. **Documentation Sync** - Update docs to match code
7. **Test Review** - Review tests for correctness, completeness, and proper positioning
8. **Best Practices Review** - Scan for common anti-patterns and code quality issues
9. **Final Validation** - Verify all checks pass

## Detailed Instructions

Follow these steps in order. Do not skip steps or proceed if a step fails.

### Step 1: Build and Test Verification

**Execute**: `Run`

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

**Execute**: [`./.github/skills/cleanup/scripts/format-code.ps1`](/.github/skills/cleanup/scripts/format-code.ps1)

**Purpose**: Apply consistent code formatting to all C# files using `dotnet format`.

**What it does**:

- Formats all C# files according to `.editorconfig` rules
- Applies formatting from `Directory.Build.props`
- Ensures consistent style across the codebase

**Requirements**:

- ✅ Formatting completes successfully

---

### Step 3: Generate Quality Overview

**Execute**: [`./.github/skills/cleanup/scripts/analyze-code-quality.ps1`](/.github/skills/cleanup/scripts/analyze-code-quality.ps1)

**Purpose**: Build the solution and analyze all warnings, errors, and code quality issues from `dotnet build` output.

**Output**: Creates a detailed overview file at `.tmp/code-quality-overview.md` using the [overview template](/.github/skills/cleanup/templates/overview-template.md).

**What the template shows**:

- **Build status**: Success/failure, error count, warning count
- **High priority issues** (🔴): Critical bugs, security issues, Blazor/Razor problems
  - These MUST be fixed before proceeding
  - Examples: CA1062 (null checks), RZ2012 (Blazor component issues)
- **Medium priority issues** (🟡): Code quality and maintainability concerns
  - Should be fixed or explicitly suppressed with reason
  - Examples: Most CA\* analyzers
- **Low priority issues** (🟢): Style, formatting, documentation
  - Can be safely suppressed to reduce noise
  - Examples: IDE\* suggestions, CS15\* documentation warnings
- **Quick response guide**: Template for user to tell you what to do

**When to use this template**:

- ✅ **Always run after Step 2 (Code Formatting)** - Shows all `dotnet build` warnings
- ✅ **When code quality issues need review** - Provides clear priority assessment
- ✅ **Before making fixes** - Gives user visibility to decide fix vs suppress
- ✅ **After major changes** - Verifies no new issues were introduced

**Action Required**:

1. **Read the generated overview** (`.tmp/code-quality-overview.md`)
2. **Present it to the user** - Show summary and ask for decisions
3. **Wait for user response** - User will tell you which issues to fix/suppress/show
4. **Do NOT proceed automatically** - User must review and approve actions

**User Response Format**:

The template includes a "Quick Response Guide" section that helps users respond with:

- **"Fix {RULE_ID}"** - You'll fix all occurrences of that rule
- **"Suppress {RULE_ID}"** - You'll add `.editorconfig` suppression
- **"Show {RULE_ID}"** - You'll show the code for manual review
- **"Suppress all low priority"** - You'll suppress all low-priority warnings

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

- Run `Run` to verify changes
- Use `get_errors` tool to check for new VS Code diagnostics
- Fix any new issues introduced by changes

---

### Step 5: Dead Code Detection

**Execute**: [`./.github/skills/cleanup/scripts/find-dead-code.ps1`](/.github/skills/cleanup/scripts/find-dead-code.ps1)

**Purpose**: Identify unused code, imports, members, and CSS classes.

**What it finds**:

- Unused using statements (IDE0005)
- Unused private members (methods, fields, properties)
- Commented-out code blocks
- Unused CSS classes

**Output**: Console report of all potentially dead code with confidence levels

**Action Required**:

1. **Review the findings** in console output
2. **Verify suspicious items** manually using `grep_search` or `read_file`
3. **Tell me which items to remove**:
   - "Remove all high confidence items"
   - "Remove unused usings only"
   - "Show me [specific item] before removing"
   - "Skip dead code removal" (if all items are false positives)
4. **Do NOT proceed automatically** - User must approve removals

**Common False Positives** to watch for:

- xUnit test methods (discovered by reflection)
- Blazor component parameters (set via Razor)
- Minimal API handlers (registered in `Program.cs`)
- Private fields only used in `Dispose()` methods

**After user approval**:

- Execute `find-dead-code.ps1 -Fix` (if user confirmed)
- Run `Run` to verify nothing broke
- Use `get_errors` to check for new compilation issues

---

### Step 5a: Project Analysis (NEW)

**Purpose**: Perform comprehensive analysis of the entire project to understand what exists BEFORE reviewing documentation and tests. This provides critical context for subsequent reviews.

**Why This Step Matters**:

Without understanding what's actually implemented, documentation and test reviews are superficial. This step systematically discovers:

- What features exist in code (implementation)
- What's documented (documentation)
- What's tested (test coverage)

This analysis provides the foundation for identifying:

- Undocumented features
- Missing test coverage
- Stale documentation
- Gaps between implementation and tests

**Analysis Process** (follow in order):

**1. Implementation Analysis**:

- **API Endpoints**: List all endpoints in `src/TechHub.Api/`
  - Use `grep_search` for `MapGet|MapPost|MapPut|MapDelete` patterns
  - Document endpoint paths, HTTP methods, parameters
  
- **Blazor Components**: List all components in `src/TechHub.Web/Components/`
  - Use `file_search` for `*.razor` files
  - Document component names, parameters, purposes
  
- **Services & Repositories**: List all services in `src/TechHub.Infrastructure/`
  - Use `grep_search` for `public.*Service|public.*Repository` patterns
  - Document service names and primary responsibilities
  
- **Domain Models**: List all models in `src/TechHub.Core/Models/`
  - Use `file_search` for `*.cs` files
  - Document record/class names and key properties
  
- **Configuration**: Review `appsettings.json` for sections, collections, settings
  - Document all configured features

- **Your own judgement**: Based on what you've found so far, look for any other significant features or patterns in the codebase that should be noted. Take as long as you need.

**2. Documentation Analysis**:

- **API Specification**: Read `docs/api-specification.md`
  - List documented endpoints
  - Compare with implementation (Step 1)
  
- **AGENTS.md Files**: Scan all AGENTS.md files
  - List documented patterns and features
  - There are nested AGENTS.md files in many folders
  - Note any references to code/features
  
- **Functional Docs**: Read `docs/*.md` files
  - List documented features (filtering, content management, RSS)
  - Note any implementation details mentioned

**3. Test Coverage Analysis**:

- **Unit Tests**: Scan `tests/TechHub.Core.Tests/` and `tests/TechHub.Infrastructure.Tests/`
  - Use `grep_search` for `public.*void.*Test|\[Fact\]|\[Theory\]` patterns
  - List tested classes and methods
  
- **Integration Tests**: Scan `tests/TechHub.Api.Tests/`
  - List tested API endpoints
  - Compare with implementation endpoints (Step 1)
  
- **Component Tests**: Scan `tests/TechHub.Web.Tests/`
  - List tested components
  - Compare with implementation components (Step 1)
  
- **E2E Tests**: Scan `tests/TechHub.E2E.Tests/`
  - List tested user workflows
  - Identify which features have E2E coverage

**4. Gap Analysis**:

Compare implementation, documentation, and tests:

- **Undocumented Features**: Implementation exists but not documented
- **Untested Features**: Implementation exists but no tests
- **Stale Documentation**: Documentation references removed features
- **Misplaced Tests**: Tests in wrong layer (e.g., E2E tests with mocks)
- **Missing E2E Coverage**: UI changes without E2E tests

**Output**:

Create a summary report at `.tmp/project-analysis.md`:

```markdown
## Project Analysis Summary

### Implementation Inventory
- **API Endpoints**: 14 endpoints (list with paths)
- **Blazor Components**: 8 components (list with names)
- **Services**: 5 services (list with names)
- **Domain Models**: 6 models (list with names)

### Documentation Inventory
- **API Specification**: Documents 14 endpoints (✅ complete)
- **Functional Docs**: 4 files covering filtering, content, RSS
- **AGENTS.md Files**: 17 files covering all domains

### Test Coverage Inventory
- **Unit Tests**: 155 tests across 8 test files
- **Integration Tests**: 100 tests across 6 test files
- **Component Tests**: 0 tests (⚠️ missing)
- **E2E Tests**: 103 tests across 13 test files

### Gap Analysis
- **Undocumented**: (list features)
- **Untested**: (list features)
- **Stale Docs**: (list outdated content)
- **Misplaced Tests**: (list tests in wrong layer)
- **Missing E2E**: (list UI changes without E2E tests)
```

**Action Required**:

1. **Present the analysis report** to user
2. **Highlight critical gaps** (untested features, undocumented endpoints)
3. **Wait for user decisions**:
   - "Proceed with documentation review" (continue to Step 6)
   - "Fix gaps first" (address gaps before continuing)
   - "Skip gap fixing" (note gaps but continue)

**This analysis provides context for**:

- Step 6 (Documentation Review): Know what to look for
- Step 7 (Test Review): Know what should be tested
- Step 8 (Best Practices): Understand feature usage patterns

---

### Step 6: Documentation Review

**Execute**: [`./.github/skills/cleanup/scripts/verify-documentation.ps1`](/.github/skills/cleanup/scripts/verify-documentation.ps1)

**Purpose**: Ensure documentation matches current code and follows [documentation strategy](/workspaces/techhub/docs/AGENTS.md).

**What it checks**:

1. **File existence**: All required AGENTS.md and functional docs exist
2. **Broken links**: Markdown links point to existing files
3. **API coverage**: Endpoints are documented in api-specification.md
4. **Documentation completeness**: Based on docs/AGENTS.md strategy
5. **Content placement**: Docs in correct locations per hierarchy
6. **Staleness**: Documentation reflects current implementation
7. **Duplication**: Content isn't duplicated across files
8. **Consistency**: Terminology matches Site Terminology in root AGENTS.md

**Output**: Verification report with file checks, broken links, and API documentation gaps

**Enhanced Documentation Review**:

After running the basic verification script, perform comprehensive review:

1. **Read [/workspaces/techhub/docs/AGENTS.md](/workspaces/techhub/docs/AGENTS.md)** to understand documentation strategy
2. **Use project analysis from Step 5a** to identify undocumented features
3. **Review existing docs** for:
   - **Completeness**: Missing sections or outdated information
   - **Staleness**: References to removed features or old patterns
   - **Consistency**: Terminology matches root AGENTS.md Site Terminology
   - **Duplication**: Same content in multiple files (should link instead)
   - **Placement**: Content in correct file per docs/AGENTS.md hierarchy
4. **Check AGENTS.md files** against implementation:
   - Patterns still match current code
   - Commands still work
   - Examples are up-to-date

**Action Required**:

1. **Present verification results** to user
2. **Show any documentation gaps** found during implementation scan
3. **List inconsistencies** or stale content discovered
4. **Wait for user decisions**:
   - "Fix broken links"
   - "Update API specification for [endpoint]"
   - "Remove stale references to [feature]"
   - "Document [new feature]"
   - "Skip documentation updates" (if all is current)

**Update as needed**:

- Sync outdated documentation
- Add missing documentation for new features
- Remove documentation for deleted code
- Fix broken links
- Consolidate duplicated content

---

### Step 7: Test Review

**Read First**: [/workspaces/techhub/tests/AGENTS.md](/workspaces/techhub/tests/AGENTS.md)

**Purpose**: Review all tests for correctness, completeness, proper positioning, and adherence to testing standards defined in tests/AGENTS.md.

**What to review**:

1. **Test Correctness**:
   - ✅ Tests follow AAA pattern (Arrange-Act-Assert) with explicit comments
   - ✅ Test names follow `{MethodName}_{Scenario}_{ExpectedOutcome}` convention
   - ✅ **Test names accurately describe what is being tested** (name matches behavior)
   - ✅ Test names are descriptive enough to understand test purpose without reading code
   - ✅ Tests use `async Task` not `async void`
   - ✅ Tests include proper assertions (not missing or trivial)
   - ✅ Tests dispose resources properly
   - ✅ No shared mutable state between tests
   - ❌ No production logic duplicated in tests
   - ❌ No testing of implementation details (test public API only)

2. **Test Completeness**:
   - ✅ Happy path scenarios covered
   - ✅ Edge cases covered (null, empty, boundary values)
   - ✅ Error cases covered (exceptions, validation failures)
   - ✅ Regression tests for known bugs
   - ✅ All public API methods have tests
   - ✅ Critical business logic thoroughly tested

3. **Test Positioning** (correct layer):
   - **Unit Tests** (`TechHub.Core.Tests/`, `TechHub.Infrastructure.Tests/`):
     - Tests domain logic in isolation
     - Mocks external dependencies (file system, HTTP, external APIs)
     - No real file I/O or network calls
   - **Integration Tests** (`TechHub.Api.Tests/`):
     - Tests API endpoints with WebApplicationFactory
     - **Uses mocked repositories/services** (not real file system)
     - Tests request/response contracts
   - **Component Tests** (`TechHub.Web.Tests/`):
     - Tests Blazor components with bUnit
     - Mocks services and dependencies
     - Tests rendering and component logic
   - **E2E Tests** (`TechHub.E2E.Tests/`):
     - Tests complete user workflows
     - **Uses real dependencies** (actual file system, real data)
     - Tests both API (HttpClient) and UI (Playwright)
     - NO mocking - tests real behavior
   - **PowerShell Tests** (`powershell/`):
     - Tests automation scripts with Pester
     - Mocks external commands

4. **Test Type Correctness**:
   - ❌ Unit tests don't access file system or network
   - ❌ Integration tests don't use real file system (should mock repositories)
   - ❌ E2E tests don't use mocks (should use real dependencies)
   - ✅ Tests are in correct test project for what they test
   - ✅ Mocking strategy matches test layer (see tests/AGENTS.md)

5. **Common Anti-Patterns to Flag**:
   - ❌ Duplicating production logic in test files
   - ❌ Copying production code into tests
   - ❌ Testing implementation details instead of public API
   - ❌ Mocking what you're testing (only mock dependencies)
   - ❌ Sharing mutable state between tests
   - ❌ Assuming test execution order
   - ❌ Skipped tests without clear reason
   - ❌ Flaky tests that pass/fail randomly
   - ❌ Using `async void` in tests
   - ❌ **Test names that don't match test behavior** (misleading or inaccurate names)
   - ❌ **Vague test names** (e.g., `Test1`, `TestMethod`, `ShouldWork`)

6. **TDD Compliance**:
   - ✅ Bug fixes have regression tests
   - ✅ New features have tests
   - ✅ Tests verify behavior, not just code coverage
   - ✅ Tests are maintained alongside code

**Review Process**:

1. **Read tests/AGENTS.md** to understand testing standards
2. **Use project analysis from Step 5a** to identify coverage gaps:
   - API endpoints without integration tests
   - Components without component or E2E tests
   - Services without unit tests
   - UI changes without E2E tests
3. **Read project-specific test AGENTS.md files**:
   - `tests/TechHub.Core.Tests/AGENTS.md` - Unit test patterns
   - `tests/TechHub.Infrastructure.Tests/AGENTS.md` - Infrastructure test patterns
   - `tests/TechHub.Api.Tests/AGENTS.md` - Integration test patterns (mocked dependencies)
4  - `tests/TechHub.Web.Tests/AGENTS.md` - Component test patterns
5  - `tests/TechHub.E2E.Tests/AGENTS.md` - E2E test patterns (real dependencies)
   - `tests/powershell/AGENTS.md` - PowerShell test patterns
4. **Scan test files** in each test project using `file_search` and `grep_search`
5. **For each test method, verify**:
   - Read the test code (Arrange, Act, Assert sections)
   - Check if the test name accurately describes what is being tested
6  - Verify the scenario in the name matches the actual test setup
7  - Verify the expected outcome in the name matches the assertions
8  - Flag any mismatch between name and behavior
6. **Check for violations** of testing standards
7. **Identify misplaced tests** (e.g., unit tests in E2E project)
8. **Find missing test coverage** for critical features

**Action Required**:

1. **Present findings** to user with specific examples:
   - "Found 5 tests missing AAA comments in `TechHub.Core.Tests/Models/ContentItemTests.cs`"
   - "Test `GetSections_ReturnsData()` name doesn't match behavior - actually tests filtering, should be `GetSections_WithTagFilter_ReturnsFilteredData()`"
   - "Test `RepositoryTests.GetAll_ReturnsItems()` duplicates production parsing logic"
   - "Integration test `ApiTests.GetSections_ReturnsData()` uses real file system - should mock repository"
   - "Missing edge case tests for null/empty inputs in `MarkdownServiceTests.cs`"
   - "Vague test name `Test1()` in `ContentItemTests.cs` - should describe scenario and outcome"
2. **Categorize issues by severity**:
   - 🔴 **Critical**: Tests that are broken, flaky, or fundamentally wrong
   - 🟡 **Important**: Tests missing coverage, incorrect positioning, anti-patterns
   - 🟢 **Minor**: Naming conventions, missing comments, minor style issues
3. **Wait for user decisions**:
   - "Fix all critical issues"
   - "Move integration tests to use mocked repositories"
   - "Add missing edge case tests for [feature]"
   - "Fix AAA pattern violations"
   - "Skip test review" (if all tests are correct)

**Common Issues to Watch For**:

- **Misplaced E2E Tests**: E2E tests MUST use real file system and real dependencies
  - If test uses mocks → It's NOT E2E, move to integration or unit layer
- **Integration Tests Using Real Files**: API integration tests should mock repositories
  - If test reads real markdown files → Should use mocked repository instead
- **Unit Tests With File I/O**: Unit tests should never touch file system
  - If test creates/reads files → Should mock file system or move to integration layer
- **Missing E2E Coverage**: UI changes MUST have E2E tests
  - Check for URL routing, component interactivity, navigation changes without E2E tests

**Grading Rubric**:

Provide an overall test quality grade based on findings:

- **A (Excellent)**: 0 critical, 0-2 important issues, any minor issues
  - Tests are comprehensive, well-structured, correctly positioned
  - Names accurately describe behavior
  - All edge cases covered
  
- **B (Good)**: 0 critical, 3-5 important issues, any minor issues
  - Tests are solid with room for improvement
  - Most names accurate, minor gaps in coverage
  - Generally follows best practices
  
- **C (Acceptable)**: 0 critical, 6+ important issues OR 1-2 critical issues
  - Tests work but have notable quality problems
  - Some misleading names or missing coverage
  - Needs improvement to meet standards
  
- **D (Poor)**: 3-5 critical issues
  - Tests have serious quality problems
  - Misleading names, incorrect positioning, or broken patterns
  - Requires immediate attention
  
- **F (Failing)**: 6+ critical issues OR tests don't run
  - Tests are fundamentally broken or missing
  - Critical defects prevent reliable testing
  - Major rework required

**NEVER report "good to excellent" if ANY critical issues exist** - critical issues automatically cap grade at C or lower.

**After user approval**:

- Fix test issues using `replace_string_in_file` or `multi_replace_string_in_file`
- Move misplaced tests to correct projects
- Add missing test coverage
- Run `Run` to verify all tests still pass
- Use `get_errors` to check for new issues

---

### Step 8: Best Practices Review

**Purpose**: Review source code for common anti-patterns, performance issues, and coding best practices violations.

**What to review**:

1. **Null Safety Issues**:
   - ❌ `.Count` or `.Length` on potentially null collections without null check
   - ❌ Direct property access without null-conditional operator (`?.`)
   - ❌ Missing null checks on parameters that could be null
   - ✅ Proper use of null-conditional operators (`?.`, `??`)
   - ✅ Nullable reference types used correctly

2. **Collection and LINQ Anti-Patterns**:
   - ❌ `.Count()` on `IEnumerable` that's already a collection (use `.Count` property)
   - ❌ `.Any()` followed by `.First()` (use `.FirstOrDefault()` instead)
   - ❌ Multiple enumerations of same `IEnumerable` (materialize with `.ToList()`)
   - ❌ Unnecessary `.ToList()` when only enumerating once
   - ❌ Using `foreach` with `.Add()` instead of LINQ operations
   - ✅ Efficient LINQ usage with proper materialization

3. **Async/Await Patterns**:
   - ❌ `async void` methods (except event handlers)
   - ❌ `.Result` or `.Wait()` on async operations (causes deadlocks)
   - ❌ Missing `ConfigureAwait(false)` in library code
   - ❌ Async methods not awaited (missing `await`)
   - ✅ Proper async/await throughout call chain
   - ✅ Cancellation tokens passed through async operations

4. **Disposal and Resource Management**:
   - ❌ `IDisposable` objects not disposed (missing `using` statement)
   - ❌ Streams, HttpClient, database connections not properly disposed
   - ❌ Event handlers not unsubscribed (memory leaks)
   - ✅ Proper `using` statements or `using` declarations
   - ✅ Dispose pattern implemented correctly

5. **String and StringBuilder Issues**:
   - ❌ String concatenation in loops (use `StringBuilder`)
   - ❌ `string.Format` when interpolation is clearer (`$"..."`)
   - ❌ Case-sensitive string comparisons without culture specification
   - ✅ `StringBuilder` for complex string building
   - ✅ String interpolation for readability
   - ✅ `StringComparison.OrdinalIgnoreCase` for comparisons

6. **Exception Handling**:
   - ❌ Empty catch blocks (swallowing exceptions)
   - ❌ Catching generic `Exception` without logging
   - ❌ Using exceptions for control flow
   - ❌ Not logging caught exceptions with context
   - ✅ Specific exception types caught
   - ✅ All exceptions logged with full context
   - ✅ Proper error handling and recovery

7. **Performance Issues**:
   - ❌ Synchronous I/O in async methods
   - ❌ Boxing of value types in hot paths
   - ❌ Inefficient string operations in loops
   - ❌ Creating objects unnecessarily in loops
   - ✅ Async I/O for all file/network operations
   - ✅ Value types used efficiently
   - ✅ Object pooling where appropriate

8. **Dependency Injection Issues**:
   - ❌ Using `new` for services (should be injected)
   - ❌ Static dependencies (makes testing hard)
   - ❌ Service locator pattern (anti-pattern)
   - ❌ Incorrect service lifetimes (Singleton vs Scoped vs Transient)
   - ✅ Constructor injection for dependencies
   - ✅ Proper service registration

9. **Magic Values and Configuration**:
   - ❌ Hard-coded values (URLs, file paths, connection strings)
   - ❌ Magic numbers without named constants
   - ❌ Configuration values not in `appsettings.json`
   - ✅ Configuration-driven design
   - ✅ Named constants for magic values
   - ✅ Settings from configuration files

10. **Code Readability and Maintainability**:
    - ❌ Long methods (>50 lines) that do multiple things
    - ❌ Deep nesting (>3 levels)
    - ❌ Unclear variable names (`x`, `temp`, `data`)
    - ❌ Comments explaining "what" instead of "why"
    - ✅ Single Responsibility Principle
    - ✅ Clear, descriptive names
    - ✅ Comments explain "why" when code shows "what"

**Review Process**:

1. **Use `grep_search` to find common patterns**:

   ```powershell
   # Find .Count without null check
   grep_search -query "\.Count(?!\(\))" -isRegexp true -includePattern "src/**/*.cs"
   
   # Find .Result or .Wait() (deadlock risks)
   grep_search -query "\.(Result|Wait\(\))" -isRegexp true -includePattern "src/**/*.cs"
   
   # Find async void methods
   grep_search -query "async void" -isRegexp false -includePattern "src/**/*.cs"
   
   # Find empty catch blocks
   grep_search -query "catch\s*\(\w+\)\s*\{\s*\}" -isRegexp true -includePattern "src/**/*.cs"
   ```

2. **Read source files** to verify context and check for false positives

3. **Categorize findings** by severity:
   - 🔴 **Critical**: Bugs, security issues, potential runtime errors (null refs, deadlocks)
   - 🟡 **Important**: Performance issues, maintainability problems, anti-patterns
   - 🟢 **Minor**: Style improvements, readability enhancements

4. **Present findings** to user with specific examples and recommendations

**Action Required**:

1. **Show findings** with file locations and code snippets
2. **Categorize by severity** (Critical, Important, Minor)
3. **Wait for user decisions**:
   - "Fix all critical issues"
   - "Fix [specific pattern]"
   - "Show me [file/method] for manual review"
   - "Skip best practices review" (if all code is clean)

**After user approval**:

- Fix issues using `replace_string_in_file` or `multi_replace_string_in_file`
- Run `Run` to verify changes
- Use `get_errors` to check for new issues

---

### Step 9: Final Validation

**Execute**: `Run`

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
- [ ] Code formatting verified: [`./.github/skills/cleanup/scripts/format-code.ps1 -Verify`](/.github/skills/cleanup/scripts/format-code.ps1)
- [ ] PowerShell tests pass: `Run -TestProject powershell`
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

- [format-code.ps1](/.github/skills/cleanup/scripts/format-code.ps1) - Code formatting
- [analyze-code-quality.ps1](/.github/skills/cleanup/scripts/analyze-code-quality.ps1) - Quality analysis and overview generation
- [find-dead-code.ps1](/.github/skills/cleanup/scripts/find-dead-code.ps1) - Dead code detection
- [verify-documentation.ps1](/.github/skills/cleanup/scripts/verify-documentation.ps1) - Documentation verification

**Templates**:

- [overview-template.md](/.github/skills/cleanup/templates/overview-template.md) - Standard format for quality overviews

**Project Scripts**:

- Run function (`TechHubRunner.psm1`) - Build, test, and run the solution
  - `Run` - Build + all tests + servers (validation workflow)
  - `Run -TestRerun` - Fast iteration after fixes (rebuild tests only)
  - `Run -TestProject powershell` - PowerShell tests only
  - `Run -TestProject <name>` - Scope tests to specific project
