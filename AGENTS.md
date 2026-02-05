# AI Assistant Workflow

🚨 **CRITICAL**: This file defines the **required 10-step development process** for AI coding agents. Follow these steps in order for every task.

## What is This File?

**This file is specifically for YOU - an AI coding agent.** It defines the mandatory workflow you must follow for all development tasks.

**How to use**: Follow the 10 steps below. Each step has a checklist - complete or explicitly skip each item.

---

## The 10-Step Workflow

### Step 1: Core Rules & Boundaries

**CRITICAL**: These rules apply to ALL tasks. Review before starting any work.

#### ✅ Always Do

- **Always follow the 10-step workflow in order**
- **Always complete step checklists before moving to the next step**
- **Always write tests BEFORE implementation** (TDD)
- **Always prefer tools in this order**: MCP tools → Built-in tools → CLI
- **Always check for errors after editing files** (`get_errors` tool)
- **Always run tests after code changes**
- **Always fix all linter issues**
- **Always read domain-specific AGENTS.md before making any changes in that domain**
- **Always store temp files in `.tmp/`**
- **Always use PowerShell for scripts** (save as `.ps1`, then execute)
- **Always follow timezone standard**: `Europe/Brussels`
- **Always be direct and concise** - no filler phrases
- **Always use `Run` function** (from TechHubRunner.psm1) for all build/test/run operations
- **Always monitor `Run` with `get_terminal_output`** repeatedly until "This terminal is now free to use"
- **Always wait for "This terminal is now free to use"** before executing ANY other commands in that terminal

#### ⚠️ Ask First

- **Ask first before making configuration changes** (package.json, .csproj, appsettings.json)
- **Ask first before making breaking changes to public APIs**
- **Ask first before adding new dependencies**
- **Ask first before making cross-domain changes** (API + Web + Infrastructure)
- **Ask first before making significant refactoring**

#### 🚫 Never Do

- **Never skip the 10-step workflow**
- **Never write implementation before tests**
- **Never skip E2E tests for UI changes**
- **Never use `| head`, `| tail`, `Select-Object -Last`** (blocks output)
- **Never paste scripts into terminal** (save as file first)
- **Never commit secrets or API keys**
- **Never hardcode section/collection data**
- **Never assume UTC** (use Europe/Brussels)
- **Never swallow exceptions without logging**
- **Never run `Start-Sleep` or other commands in terminal executing `Run`** before it completes
- **Never execute multiple commands in same terminal before `Run` finishes**
- **Never use `dotnet test` directly for tests**: Use `Run` instead or `Run -TestProject E2E.Tests` if you want to scope to a certain project or even add `-TestName` too
- **Never use `isBackground: true` for test runs**: Tests complete within 60 seconds - use `isBackground: false` to wait synchronously
- **Never create new terminals for each command**: Reuse existing terminals instead of spawning orphaned processes

**Step 1 Checklist**:

- [ ] Reviewed Always/Ask/Never rules
- [ ] Understand which rules apply to this task
- [ ] Ready to proceed

---

### Step 2: Gather Context

**Before touching any code**, understand what you're working with.

**Actions**:

1. MANDATORY & CRITICAL: Read relevant documentation:
   - This file for workflow
   - Important are the domain AGENTS.md for the area you're working in too. They are nested. Here's an EXAMPLE of how this works when making a change in the API:
     - [src/AGENTS.md](src/AGENTS.md) - The API resides in the src folder, so read this file first
     - [src/TechHub.Api/AGENTS.md](src/TechHub.Api/AGENTS.md) - Additionally read this because you're making API changes
     - [src/TechHub.Web/AGENTS.md](src/TechHub.Web/AGENTS.md) - If the contract of the API changes, you'll need to make changes here too
     - [tests/AGENTS.md](tests/AGENTS.md) - As you make code changes, you'll also need to write/update tests
     - [tests/TechHub.Api.Tests/AGENTS.md](tests/TechHub.Api.Tests/AGENTS.md) - You made changes in the API so you need to read this API tests specific AGENTS.md too
     - [tests/TechHub.Web.Tests/AGENTS.md](tests/TechHub.Web.Tests/AGENTS.md) - The same for this one, if you edited the web project
     - [docs/AGENTS.md](docs/AGENTS.md) - If you changed functionality or implemented certain requirements, make sure to read this to understand where and how you need to document this
     - etc
   - Finally review the [docs/documentation-index.md](docs/documentation-index.md) to find out which files you need to read to understand the functionality of the website. This is critical and you need to read the docs that are mentioned to prevent mistakes. The index file is less than 1000 lines long, so read it all to get a clear picture on what additional documentation to read.

2. Scan the code:
   - Use `read_file` to examine relevant files
   - Use `grep_search` or `semantic_search` to find related patterns
   - Check existing tests to understand expected behavior

**Key Rules**:

- Never assume - always read before modifying
- Use context7 MCP tool for framework documentation
- Follow existing code patterns

**Step 2 Checklist**:

- [ ] Read relevant documentation
- [ ] Examined related code files
- [ ] Checked existing tests
- [ ] Understand the current architecture

---

### Step 3: Create a Plan

**Always plan before making changes**.

**Actions**:

1. Break down the task:
   - Identify logical steps
   - List files that need modification
   - Determine tests needed
   - Identify documentation updates

2. Communicate:
   - Explain plan to user
   - Wait for confirmation if changes are significant
   - Use `manage_todo_list` for complex multi-step work

**Step 3 Checklist**:

- [ ] Task broken into logical steps
- [ ] Files to modify identified
- [ ] Tests to add/update identified
- [ ] Documentation updates identified
- [ ] Plan communicated (if significant)

---

### Step 4: Research & Validate

**Find additional information** for correct implementation.

**Actions**:

- Use **context7 MCP tool** for framework/library documentation
- Research best practices if needed
- Verify information from official sources

**When to Research**:

- Working with external frameworks
- Implementing new patterns
- Encountering errors
- Unsure about best practices

**Step 4 Checklist**:

- [ ] Researched framework docs (if applicable)
- [ ] Verified best practices (if applicable)
- [ ] Have enough information to proceed
- [ ] OR: No research needed for this task

---

### Step 5: Verify Current Behavior (Optional)

**If needed**, understand current behavior before changes.

**When to Verify**:

- Reproducing bugs
- Understanding complex interactions
- Investigating unexpected behavior

**How to Verify**:

- Start website: `Run -WithoutTests` (runs as background process)
- Use Playwright MCP tools for browser testing
- Document current behavior

**See [README.md - Starting, Stopping and Testing](README.md#starting-stopping-and-testing-the-website)** for complete instructions.

**Step 5 Checklist**:

- [ ] Current behavior verified
- [ ] OR: Verification not needed for this task

---

### Step 6: Write Tests First (TDD)

**CRITICAL**: Write tests BEFORE implementing changes.

📖 **MUST READ**: [tests/AGENTS.md](tests/AGENTS.md) for testing strategies, patterns, and requirements.

**Actions**:

1. Start with clean slate:
   - Run ALL tests first: `Run`
   - Fix any broken tests before proceeding

2. Write failing tests:
   - For bugs: Write test that reproduces the bug
   - For features: Write tests defining expected behavior
   - Run tests - verify they fail for the right reason

**Test Types Required**:

| Change Type | Required Tests |
|-------------|----------------|
| Bug fix | Failing test reproducing bug (integration preferred if exposed via API) |
| New feature | **Integration (MANDATORY)** + Unit (edge cases) + E2E (critical paths) |
| API changes | **Integration tests (MANDATORY)** |
| UI/frontend changes | Component + **E2E (MANDATORY)** + Integration for API endpoints |
| Backend-only | **Integration (MANDATORY)** + Unit (edge cases) |
| Documentation-only | None |

**Testing Diamond Priority**: Integration tests at the API boundary are the most important layer. All functionality exposed via API must have integration test coverage. Unit tests focus on edge cases and quick feedback. E2E tests validate critical user journeys.

**🚨 E2E tests are MANDATORY for ALL UI changes** - see [tests/TechHub.E2E.Tests/AGENTS.md](tests/TechHub.E2E.Tests/AGENTS.md)

**Step 6 Checklist**:

- [ ] Ran existing tests first (clean slate)
- [ ] Wrote failing tests for new behavior
- [ ] Verified tests fail for the right reason
- [ ] OR: Documentation-only change (no tests needed)

---

### Step 7: Implement Changes

**NOW implement** to make tests pass.

📖 **MUST READ**: [src/AGENTS.md](src/AGENTS.md) for .NET patterns, plus the domain-specific AGENTS.md for the area you're modifying.

**Actions**:

1. Write minimal code:
   - Write ONLY enough code to make tests pass
   - Follow existing patterns and conventions
   - Use `replace_string_in_file` for edits

2. Check quality:
   - Run `get_errors` after each file edit
   - Fix all linting/compilation errors
   - Run tests frequently

**Running the Website**:

See [docs/running-and-testing.md](docs/running-and-testing.md) for:

- `Run` - Build, test, start servers
- `Run -WithoutTests` - Start servers without tests
- Playwright MCP tools for interactive testing

**Step 7 Checklist**:

- [ ] Implemented minimal code to pass tests
- [ ] Checked for errors after edits
- [ ] Fixed all linting issues
- [ ] Tests pass

---

### Step 8: Validate & Fix

**Ensure all tests pass and code quality is high**.

**Actions**:

1. Run full test suite:
   - `Run` to run all tests
   - Or `Run -TestProject <name>` for scoped testing
   - ALL tests must pass

2. Check quality:
   - Use `get_errors` for linting/compilation
   - Verify code follows conventions
   - Check error handling and logging

**Step 8 Checklist**:

- [ ] All tests pass
- [ ] No linting/compilation errors
- [ ] Code follows project conventions
- [ ] No regressions introduced

---

### Step 9: Update Documentation

**🚨 MANDATORY**: Update docs if you made changes.

📖 **MUST READ**: [docs/AGENTS.md](docs/AGENTS.md) for documentation strategies and rules.

**What to Update**:

- Domain AGENTS.md if implementation changed significantly
- [docs/](docs/) if ANY features or behavior changed
- Code comments for complex logic

**How to Find Docs**:

- Review the [docs/documentation-index.md](docs/documentation-index.md) to find out which files you need to update.
- Use `grep_search` to find docs mentioning modified features
- Check domain AGENTS.md in directories you modified

**Introducing New Files**:

If you can't find a good place to document something, consider creating a new file!

**Step 9 Checklist**:

- [ ] Updated relevant documentation by changing existing files or introducing new ones
- [ ] Fixed markdown linting: `npx markdownlint-cli2 --fix <file> --config /workspaces/techhub/.markdownlint-cli2.jsonc`
- [ ] Ran `/workspaces/techhub/scripts/Generate-DocumentationIndex.ps1` if you changed ANY headings
- [ ] OR: No documentation updates needed

---

### Step 10: Report Completion

**Tell the user you're done** with a clear summary.

**What to Report**:

- Concise summary of changes
- Links to modified files
- Confirmation that tests pass
- Any important notes or caveats

**File Link Format**: Use `[path/file.ts](path/file.ts)` - NO backticks around file names.

**Step 10 Checklist**:

- [ ] Summary provided
- [ ] All files linked
- [ ] Tests confirmed passing
- [ ] Task 100% complete