# AI Assistant Workflow

**🚨 ABSOLUTELY CRITICAL REQUIREMENT 1**: Always use these instructions! Even when you think my instructions are clear or that you are given a simple task. There is critical information in this file that you will always need!

**🚨 ABSOLUTELY CRITICAL REQUIREMENT 2**: NEVER EVER use pattern recognition or "I know what this step should do" thinking. Each step has EXACT instructions - follow them literally, not what you think they should accomplish.

**🚨 ABSOLUTELY CRITICAL REQUIREMENT 3**: Do NOT optimize for tokens, speed, or efficiency. This workflow is intentionally verbose and step-by-step for precision. Follow every sub-instruction within each step.

**🚨 ABSOLUTELY CRITICAL REQUIREMENT 4**: Do NOT improvise, combine, reorder, parallelize or alter instructions in any way.

**🚨 ABSOLUTELY CRITICAL REQUIREMENT 5**: Always complete step checklists before moving to the next step!

## What is This File?

**This file is specifically for YOU - an AI coding agent.** It defines the mandatory workflow you must follow for all development tasks.

**How to use**: First read the [absolutely critical rules](#-absolute-critical-rules). Then follow [the 8-step workflow](#the-8-step-workflow). Each step has a checklist - complete or explicitly skip each item.

---

## 🚨 ABSOLUTE CRITICAL RULES

### ✅ Always Do

- **ALWAYS go back to [2. Gather context from documentation and validate the plan](#2-gather-context-from-documentation-and-validate-the-plan)** when you are about to work on ANYTHING you haven't yet read documentation for!
- **ALWAYS go back to [2. Gather context from documentation and validate the plan](#2-gather-context-from-documentation-and-validate-the-plan)** if you find yourself struggling!
- **Always use `Run` function** for all build/test/run operations, see [docs/running-and-testing.md](docs/running-and-testing.md)** for complete instructions
- **Always prefer tools in this order**: MCP tools → Built-in tools → CLI
- **Always check for errors after editing files** (`get_errors` tool)
- **Always fix all linter issues**
- **Always store temp files in `.tmp/`**
- **Always use PowerShell for scripts** (save as `.ps1`, then execute)
- **Always follow timezone standard**: `Europe/Brussels`
- **Always be direct and concise** - no filler phrases

### ⚠️ Ask First

- **Ask first before making breaking changes to public APIs**
- **Ask first before adding new dependencies**
- **Ask first before making cross-domain changes** (API + Web + Infrastructure)
- **Ask first before making significant refactoring**

### 🚫 Never Do

- **Never use `| head`, `| tail`, `Select-Object -Last`** (blocks output)
- **Never paste scripts into terminal** (save as file first)
- **Never commit secrets or API keys**
- **Never hardcode section/collection data**
- **Never assume UTC** (use Europe/Brussels)
- **Never swallow exceptions without logging**
- **Never import `TechHubRunner.psm1`**, it gets imported automatically. Only do a force reload if you made changes in this file.

---

## The 8-Step Workflow

### 1. Review the codebase and create an initial plan

#### 1.1 Preconditions

- User has requested a task or change
- You have NOT started making any code changes yet

#### 1.2 Actions

1. Explore the code:
   - Use `read_file` to examine relevant code files
   - Use `grep_search` or `semantic_search` to find related patterns
   - Check existing tests to understand expected behavior

2. Create an initial plan:
   - Identify logical steps
   - List files that need modification
   - Determine tests needed
   - Identify documentation updates
   - Use `manage_todo_list` for complex multi-step work

#### 1.3 Checklist

- [ ] Examined related code files
- [ ] Checked existing tests
- [ ] Understand the current architecture
- [ ] Initial plan created with logical steps
- [ ] Files to modify identified
- [ ] Tests to add/update identified

#### 1.4 Completion

You understand the current code and have an initial plan.

---

### 2. Gather context from documentation and validate the plan

#### 2.1 Preconditions

- Step 1 completed (you have an initial plan)
- You need to validate and refine the plan with documentation and best practices

#### 2.2 Why This Step Matters

AI training data becomes outdated. Frameworks and technologies constantly evolve with newer, better ways of solving problems. By consulting up-to-date documentation, you create better solutions instead of relying on potentially obsolete patterns.

#### 2.3 Actions

1. Read repository documentation:
   - Read the functional documentation for the topics you are about to work on. Investigate [docs/documentation-index.md](docs/documentation-index.md) to find out what documentation exists and where to find it. This is really crucial so never skip reading the actual functional docs!
   - Read [docs/repository-structure.md](docs/repository-structure.md) repository structure file so you understand what the structure of this repository is and what files and folders exist
   - Read [docs/running-and-testing.md](docs/running-and-testing.md) to understand how you can run the application or run tests
   - Read the domain AGENTS.md for the area you're working in too. They are nested. Here's an EXAMPLE of how this works when making a change in the API:
      - [src/AGENTS.md](src/AGENTS.md) - The API resides in the src folder, so read this file first
      - [src/TechHub.Api/AGENTS.md](src/TechHub.Api/AGENTS.md) - Additionally read this because you're making API changes
      - [src/TechHub.Web/AGENTS.md](src/TechHub.Web/AGENTS.md) - If the contract of the API changes, you'll need to make changes here too
      - [tests/AGENTS.md](tests/AGENTS.md) - As you make code changes, you'll also need to write/update tests
      - [tests/TechHub.Api.Tests/AGENTS.md](tests/TechHub.Api.Tests/AGENTS.md) - You made changes in the API so you need to read this API tests specific AGENTS.md too
      - [tests/TechHub.Web.Tests/AGENTS.md](tests/TechHub.Web.Tests/AGENTS.md) - The same for this one, if you edited the web project
      - [docs/AGENTS.md](docs/AGENTS.md) - If you changed functionality or implemented certain requirements, make sure to read this to understand where and how you need to document this
      - etc

2. Get latest framework documentation:
   - **MANDATORY for new features and big changes**: Use **context7 MCP tool** for framework/library documentation
   - For bug fixes: Context7 is optional unless you're unsure about the correct approach

3. Validate and refine the plan:
   - Compare your initial plan against documentation and best practices
   - Update the plan if better approaches exist
   - Communicate the validated plan to user (if changes are significant)

#### 2.4 Checklist

- [ ] Read relevant repository documentation
- [ ] Read domain AGENTS.md files for areas being modified
- [ ] Used context7 for latest framework docs (MANDATORY for new features/big changes)
- [ ] Plan validated against best practices
- [ ] Plan communicated (if significant)
- [ ] OR: Simple bug fix where extensive validation wasn't needed

#### 2.5 Completion

Plan is validated against documentation and best practices, OR this is a simple bug fix where validation wasn't needed.

---

### 3. Verify current behavior before making changes (Optional)

#### 3.1 Preconditions

- Step 2 completed
- You need to understand current behavior before changing it
- Reproducing bugs
- Understanding complex interactions
- Investigating unexpected behavior

#### 3.2 Actions

- Start website: `Run -WithoutTests` (runs as background process)
- Use Playwright MCP tools for browser testing
- Document current behavior

**See [docs/running-and-testing.md](docs/running-and-testing.md)** for complete instructions.

#### 3.3 Checklist

- [ ] Current behavior verified
- [ ] OR: Verification not needed for this task

#### 3.4 Completion

Current behavior is documented, OR verification was not needed.

---

### 4. Write tests BEFORE implementing changes (TDD)

#### 4.1 Preconditions

- Steps 1-3 completed (or skipped where applicable)
- You are ready to start coding
- You have NOT written implementation code yet

📖 **MUST READ**: [tests/AGENTS.md](tests/AGENTS.md) for testing strategies, patterns, and requirements.

#### 4.2 Actions

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

#### 4.3 Checklist

- [ ] Ran existing tests first (clean slate)
- [ ] Wrote failing tests for new behavior
- [ ] Verified tests fail for the right reason
- [ ] OR: Documentation-only change (no tests needed)

#### 4.4 Completion

Tests exist that fail for the right reason, OR this is a documentation-only change.

---

### 5. Implement changes to make tests pass

#### 5.1 Preconditions

- Step 4 completed (failing tests exist)
- Tests are failing for the expected reason

📖 **MUST READ**: [src/AGENTS.md](src/AGENTS.md) for .NET patterns, plus the domain-specific AGENTS.md for the area you're modifying.

#### 5.2 Actions

1. Write minimal code:
   - Write ONLY enough code to make tests pass
   - Follow existing patterns and conventions
   - Use `replace_string_in_file` for edits

2. Check quality:
   - Run `get_errors` after each file edit
   - Fix all linting/compilation errors
   - Run tests frequently

3. If you struggle, need to introduce a workaround or hack or are going in circles, ALWAYS go back to [2. Gather context from documentation and validate the plan](#2-gather-context-from-documentation-and-validate-the-plan) and gather more information and re-validate your approach.

**Running the Website**:

See [docs/running-and-testing.md](docs/running-and-testing.md) for:

- `Run` - Build, test, start servers
- `Run -WithoutTests` - Start servers without tests
- Playwright MCP tools for interactive testing

#### 5.3 Checklist

- [ ] Implemented minimal code to pass tests
- [ ] Checked for errors after edits
- [ ] Fixed all linting issues
- [ ] Tests pass

#### 5.4 Completion

Tests pass with minimal implementation code.

---

### 6. Validate all tests pass and code quality is high

#### 6.1 Preconditions

- Step 5 completed (implementation done)
- Tests were passing at end of Step 5

#### 6.2 Actions

1. Run full test suite:
   - `Run` to run all tests
   - Or `Run -TestProject <name>` for scoped testing
   - ALL tests must pass

2. Check quality:
   - Use `get_errors` for linting/compilation
   - Verify code follows conventions
   - Check error handling and logging

#### 6.3 Checklist

- [ ] All tests pass
- [ ] No linting/compilation errors
- [ ] Code follows project conventions
- [ ] No regressions introduced

#### 6.4 Completion

All tests pass, no linting errors, code follows conventions.

---

### 7. Update documentation if you made changes

#### 7.1 Preconditions

- Step 6 completed (all tests pass)
- You made changes that affect functionality or behavior

📖 **MUST READ**: [docs/AGENTS.md](docs/AGENTS.md) for documentation strategies and rules.

#### 7.2 Actions

1. Identify what to update:
   - Domain AGENTS.md if implementation changed significantly
   - [docs/](docs/) if ANY features or behavior changed
   - Code comments for complex logic

2. Find relevant docs:
   - Review the [docs/documentation-index.md](docs/documentation-index.md) to find out which files you need to update.
   - Use `grep_search` to find docs mentioning modified features
   - Check domain AGENTS.md in directories you modified

3. Update and validate:
   - Make documentation changes
   - Fix markdown linting
   - Regenerate documentation index if headings changed

**Introducing New Files**: If you can't find a good place to document something, consider creating a new file!

#### 7.3 Checklist

- [ ] Updated relevant documentation by changing existing files or introducing new ones
- [ ] Fixed markdown linting: `npx markdownlint-cli2 --fix <file> --config /workspaces/techhub/.markdownlint-cli2.jsonc`
- [ ] Ran `/workspaces/techhub/scripts/Generate-DocumentationIndex.ps1` if you changed ANY headings
- [ ] OR: No documentation updates needed

#### 7.4 Completion

Documentation is updated, OR no documentation updates were needed.

---

### 8. Report completion with a clear summary

#### 8.1 Preconditions

- All previous steps completed
- All tests pass
- Documentation is up to date

#### 8.2 Actions

- Provide concise summary of changes
- Link to all modified files
- Confirm that tests pass
- Note any important caveats or follow-up items

**File Link Format**: Use `[src/TechHub.Api/AGENTS.md](src/TechHub.Api/AGENTS.md)` format - NO backticks around file names.

#### 8.3 Checklist

- [ ] Summary provided
- [ ] All files linked
- [ ] Tests confirmed passing
- [ ] Task 100% complete

#### 8.4 Completion

User has been informed of all changes and outcomes.
