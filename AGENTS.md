# Tech Hub Development Guide

**AI CONTEXT**: This is the **ROOT** development guide. It defines repository-wide principles, architecture, and workflow. When working in a specific domain (e.g., `src/`, `scripts/`, `tests/`), **ALSO** read the domain-specific `AGENTS.md` file in that directory.

**🚨 ABSOLUTELY CRITICAL**: This section defines a **required 10-step process** for all development tasks in [AI Assistant Workflow](#ai-assistant-workflow). Always follow these steps in order for every request.
**🚨 ABSOLUTELY CRITICAL**: Always read the root [README.md](/README.md) before starting any work to understand the context of this repository better.**

## Index

- [Project Overview](#project-overview)
  - [Repository Organization](#repository-organization)
- [AI Assistant Workflow](#ai-assistant-workflow)
  - [1. Core Rules & Boundaries](#1-core-rules--boundaries)
  - [2. Gather Context](#2-gather-context)
  - [3. Create a Plan](#3-create-a-plan)
  - [4. Research & Validate](#4-research--validate)
  - [5. Verify Current Behavior (Optional)](#5-verify-current-behavior-optional)
  - [6. Write Tests First (TDD)](#6-write-tests-first-tdd)
  - [7. Implement Changes](#7-implement-changes)
  - [8. Validate & Fix](#8-validate--fix)
  - [9. Update Documentation](#9-update-documentation)
  - [10. Report Completion](#10-report-completion)
- [Starting & Stopping the Website](#starting--stopping-the-website)
  - [Starting the Website](#starting-the-website)
  - [Testing the Running Website](#testing-the-running-website)
  - [If CLI Tools Are Absolutely Required](#if-cli-tools-are-absolutely-required)
  - [Stopping the Website](#stopping-the-website)
  - [Run Function Parameters (for AI Agents)](#run-function-parameters-for-ai-agents)
  - [Building/Testing Individual Projects](#buildingtesting-individual-projects)
- [Documentation Architecture](#documentation-architecture)
  - [Documentation Hierarchy](#documentation-hierarchy)
  - [Documentation Placement Strategy](#documentation-placement-strategy)
  - [Complete Documentation Map](#complete-documentation-map)
  - [Quick Reference Guide](#quick-reference-guide)
- [Core Development Principles](#core-development-principles)
  - [Tech Stack](#tech-stack)
  - [Architectural Principles](#architectural-principles)
  - [Performance Architecture](#performance-architecture)
  - [Accessibility Standards](#accessibility-standards)
  - [Configuration-Driven Development](#configuration-driven-development)
  - [Timezone & Date Handling](#timezone--date-handling)
- [Site Terminology](#site-terminology)
  - [Core Concepts](#core-concepts)
  - [Content Organization](#content-organization)
  - [Filtering Systems](#filtering-systems)
  - [Content Structure](#content-structure)
  - [RSS Feeds](#rss-feeds)
- [.NET Migration Status](#net-migration-status)
  - [Implementation Progress](#implementation-progress)

## Project Overview

The Tech Hub is a **modern .NET web application** built with Blazor that serves as a technical content hub. We're creating a fast, responsive, accessible platform for showcasing Microsoft technical content across AI, Azure, GitHub Copilot, .NET, DevOps, and Security topics.

**What We're Building**:

A production-quality web application featuring:

- **Modern responsive design** - Mobile-first, accessible UI with Tech Hub visual identity
- **Server-side rendering** - Blazor SSR for optimal SEO and performance
- **Progressive enhancement** - WebAssembly for rich client-side interactions
- **RESTful architecture** - Decoupled API backend and Blazor frontend
- **Configuration-driven** - Content structure defined in data files, not code
- **Resilient by design** - Built-in retry policies, error handling, graceful degradation

**Implementation Details**:

For .NET development patterns, component architecture, API design, and all code examples, see **[src/AGENTS.md](src/AGENTS.md)**.

### Repository Organization

**Source Code** (`src/`):

- `TechHub.Api/` - REST API backend (Minimal API, OpenAPI/Swagger)
- `TechHub.Web/` - Blazor frontend (SSR + WebAssembly)
- `TechHub.Core/` - Domain models, DTOs, interfaces
- `TechHub.Infrastructure/` - Repository implementations, services
- `TechHub.AppHost/` - .NET Aspire orchestration

**Content** (`collections/`):

- `_news/` - Official announcements and product updates
- `_videos/` - Video content and tutorials
- `_community/` - Microsoft Tech Community posts
- `_blogs/` - Technical articles and blogs
- `_roundups/` - Curated weekly summaries

**Tests** (`tests/`):

- `TechHub.Core.Tests/` - Unit tests for domain logic
- `TechHub.Api.Tests/` - Integration tests for API endpoints
- `TechHub.Web.Tests/` - bUnit component tests
- `TechHub.E2E.Tests/` - Playwright end-to-end tests
- `powershell/` - PowerShell script tests

**Configuration & Documentation**:

- `_data/sections.json` - Single source of truth for content structure
- `appsettings.json` - Application configuration (sections, collections)
- `docs/` - Functional documentation (API spec, filtering, content management)
- `scripts/` - Automation and utility scripts (PowerShell)
- `infra/` - Azure infrastructure (Bicep templates)
- `specs/` - Feature specifications (planning docs, may be outdated)

**Navigation**:

- See [Documentation Architecture](#documentation-architecture) for complete documentation map
- See [.NET Migration Status](#net-migration-status) for current implementation progress
- See [README.md](README.md) for quick start guide
- See [MIGRATIONSTATUS.md](MIGRATIONSTATUS.md) for detailed migration status

## AI Assistant Workflow

### 1. Core Rules & Boundaries

**CRITICAL**: These are the **non-negotiable rules** that apply to ALL development tasks. **ALWAYS** follow these rules without exception and not only the always do rules but also the ask first and never do rules.

#### ✅ Always Do

- **Always follow the 10-step workflow**: Complete all steps in order for every request
- **Always write tests BEFORE implementation**: Test-Driven Development (TDD) is mandatory
- **Always prefer higher-level tools**: ALWAYS use MCP tools > Built-in tools > CLI commands
  - **MCP tools** (highest priority): Playwright MCP (web testing), GitHub MCP (GitHub operations), context7 MCP (documentation)
  - **Built-in tools**: `replace_string_in_file` (with 5-10 lines context), `read_file`, `grep_search`, `file_search`
  - **CLI** (lowest priority): Only for complex multi-step operations not supported by tools
- **Always check for errors after editing files**: Use `get_errors` tool on modified files to check VS Code diagnostics (markdown linting, ESLint, RuboCop, etc.) and fix all issues
- **Always run tests after modifying code**: CRITICAL - After ANY code changes (C#, JavaScript, PowerShell, templates), run appropriate test suites. Documentation-only changes do not require testing
- **Always fix linter issues**: Always resolve all linting errors and warnings, EXCEPT intentional bad examples in documentation. Do NOT do this for temporary markdown files in `.tmp/` unless you intend to keep them later.
- **Always read domain-specific AGENTS.md files**: Before editing any code in that domain
- **Always store temp files in `.tmp/`**: ALL temporary/one-off scripts in `.tmp/` (e.g., quick tests, debugging scripts), permanent/reusable automation goes in `scripts/`
- **Always use PowerShell for scripts**: If script is required, it MUST be `.ps1` file in `.tmp/` directory, then execute it
- **Always follow timezone standards**: `Europe/Brussels` for all date operations
- **Always use configuration-driven design**: Update configuration in `appsettings.json`, not hardcoded values
- **Always server-side render all content**: Initial page load must show complete content
- **Always add tests for new functionality**: According to domain-specific AGENTS.md files
- **Always follow markdown guidelines**: See [collections/markdown-guidelines.md](collections/markdown-guidelines.md) and [collections/writing-style-guidelines.md](collections/writing-style-guidelines.md)
- **Always be direct and concise**: Avoid exaggerated language
- **Always maintain professional yet approachable tone**: Clear and authoritative without being overly formal
- **Always avoid filler phrases**: Don't use "Sure!" or "You're right!"
- **Always install dependencies in and via the DevContainer configuration or installation scripts**: This means `.devcontainer/post-create.sh` script or `.devcontainer/devcontainer.json` file. NEVER install dependencies in PowerShell or other scripts
- **Always install Playwright via devcontainer**: Playwright browsers are installed in `.devcontainer/post-create.sh`, NOT via terminal commands during development

#### ⚠️ Ask First

- **Ask first before making configuration changes**: Consult domain agents before modifying build system or configuration (e.g., `package.json`, `TechHub.slnx`, `.csproj` files)
- **Ask first before making breaking changes to public APIs**: Changes that affect existing functionality (e.g., modifying endpoint signatures, changing data structures)
- **Ask first before making adding new dependencies**: To dependency management files or any config (e.g., new NuGet packages, npm packages, PowerShell modules)
- **Ask first before making cross-domain changes**: Modifications affecting multiple areas (e.g., API + Blazor + Infrastructure, or content structure + build system)
- **Ask first before making a significant refactoring**: That touches many files or core architecture (e.g., modifying section configuration in `appsettings.json`, changing domain models)

#### 🚫 Never Do

- **Never skip the 10-step workflow**: All steps are required for quality work
- **Never write implementation before tests**: TDD is mandatory for code changes
- **Never skip E2E tests for UI changes**: E2E tests are MANDATORY for frontend, not optional
- **Never use lower-level tools unnecessarily**: Don't use CLI when MCP or built-in tools are available
- **Never use `| head` or `| tail` or `Select-Object -Last` or similar in CLI commands to limit the amount of lines**: They block critical output and often require the user to press control-c before you can continue!
- **Never filter test output with Select-String, Select-Object, grep, or similar pattern filtering**: In addition to the previous rule, this blocks critical output of test results, failures, errors, and context
- **Never paste scripts into terminal**: Always save as `.ps1` file in `.tmp/` and execute
- **Never use `cat` and/or `EOF` constructions to create scripts**: Always save script to file and execute it
- **Never use `pwsh -Command` with `EOF` or other large text demarcations**: Always save script to file and execute it
- **Never use backslashes for escaping in PowerShell**: Always use backticks (`)
- **Never work around missing tools**: Tell user if needed tools are unavailable instead of using alternatives
- **Never commit secrets or API keys**: Check all files before committing
- **Never modify generated directories**: Build outputs (e.g., `bin/`, `obj/`, `node_modules/`, `.tmp/`)
- **Never create content without frontmatter**: All markdown must have proper YAML front matter
- **Never hardcode section/collection data**: Always use configuration in `appsettings.json`
- **Never let JavaScript create initial content**: Server-side render everything, JS only enhances
- **Never add wrapper methods just for tests**: Test real implementation
- **Never make code backwards compatible unless requested**: Prefer clean, modern code
- **Never add comments describing what changed**: Code should be self-documenting
- **Never start responses with "Sure!" or "You're right!"**: Be direct and professional
- **Never use emojis unless explicitly requested**: Keep communication professional
- **Never leave files incomplete or broken**: Ensure all changes are complete
- **Never assume UTC**: Always use configured timezone (`Europe/Brussels`)
- **Never include Windows support**: This project is Linux-only (DevContainer environment)
- **Never suppress errors for resiliency**: Prevent errors by checking conditions first, don't hide failures with error suppression (e.g., `2>$null`)
- **Never swallow exceptions without logging**: ALWAYS log exceptions in catch blocks with full context (exception details, relevant parameters, file paths). Silent failures make debugging impossible. Use `_logger.LogError(ex, "Context message", parameters)` for all caught exceptions.
- **Never rename identifiers without checking ALL occurrences**: When renaming parameters, variables, properties, methods, or classes, use `grep_search` to find and update ALL occurrences in code, documentation, tests, and specifications

#### 🚨 Naming Consistency Rule

When renaming ANY identifier, you **MUST** verify and update ALL occurrences across:

1. **Implementation code**: Source files, interfaces, classes
2. **Documentation**: Comments, markdown files, specifications
3. **Tests**: Test files, test method names, assertions
4. **Configuration**: JSON files, YAML files, environment variables
5. **API contracts**: Endpoint routes, DTOs, OpenAPI docs

**Process**: Before renaming, run `grep_search` with the old name to find all occurrences, then systematically update each location. This prevents inconsistencies that break builds, confuse developers, and create maintenance issues.

### 2. Gather Context

**Before touching any code**, understand what you're working on:

**Read Documentation First**:

- Start with this file (Root AGENTS.md) for architecture and principles
- See [Complete Documentation Map](#complete-documentation-map) for navigation guide to all documentation files
- See [Quick Reference Guide](#quick-reference-guide) for quick navigation by task type

**Scan the Code**:

- Use `read_file` to examine relevant files
- Use `grep_search` or `semantic_search` to find related code patterns
- Use `list_dir` to understand directory structure
- Check existing tests in `tests/` to understand expected behavior

**Key Rules**:

- Never assume - always read before modifying
- Understand the existing architecture before proposing changes
- Follow existing code patterns and conventions when proposing changes
- Use the latest version of code and libraries when proposing changes
- Always use the context7 MCP tool to fetch latest documentation when proposing changes or if you don't understand something
- Read documentation, code, and content files to understand implementation

### 3. Create a Plan

**Always create a step-by-step plan** before making any changes:

**Planning Requirements**:

- Break down the task into logical, actionable steps
- Identify all files that need modification
- Determine if tests need to be added or updated
- Identify documentation that will need updates
- Consider edge cases and potential issues

**Communicate the Plan**:

- Explain your plan to the user clearly
- Wait for confirmation if changes are significant or complex
- Use `manage_todo_list` tool for complex multi-step work
- For simple single-step operations, skip formal task tracking

**Key Rules**:

- Never start coding without a plan
- Break complex tasks into smaller, manageable steps
- Always explain what you're about to do before doing it
- Wait for confirmation if changes are significant or complex

### 4. Research & Validate

**Find additional information** to ensure correct implementation:

**Use Online Resources**:

- **context7 MCP tool**: Get up-to-date documentation for any framework/library (.NET, Blazor, JavaScript libraries, etc.)
- **Web searches**: Research best practices, patterns, or solutions
- **Follow referenced links**: Check official documentation and guides
- **Verify information**: Cross-reference multiple sources when uncertain

**When to Research**:

- Working with external frameworks or libraries
- Implementing new features or patterns
- Encountering errors or unexpected behavior
- Need to verify best practices or modern approaches

**Key Rules**:

- Always use context7 for framework-specific questions
- Never fabricate or invent information
- Prefer official documentation over Stack Overflow

### 5. Verify Current Behavior (Optional)

**If needed**, understand current behavior BEFORE writing tests or making changes:

**How to Run and Test**:

See [Starting & Stopping the Website](#starting--stopping-the-website) for complete instructions on:

- How to start the website with the `Run` function
- Using Playwright MCP tools for testing (CRITICAL: works directly in GitHub Copilot Chat)
- When to use `-OnlyTests` vs `-SkipTests` parameters
- How to safely interact with the running website

**When to Verify**:

- Reproducing user-reported bugs
- Understanding complex UI interactions
- Investigating unexpected behavior
- Documenting current state before modifications

**Key Rules**:

- Use Playwright MCP for browser testing and validation
- Test local site (<http://localhost:5184>) for development
- Test live site (<https://tech.hub.ms>) when appropriate
- Document unexpected behaviors you discover
- Report any discrepancies between observed behavior and documentation

### 6. Write Tests First (TDD)

**CRITICAL**: Write tests BEFORE implementing changes to ensure proper validation:

**ALWAYS Start with Clean Slate**:

- **BEFORE any new feature, change, or bugfix**: Run ALL tests first
- **Fix ALL broken tests**: Ensure 100% pass rate before starting new work
- **Clean slate principle**: Never build on top of failing tests
- **Prevents cascading failures**: Broken tests mask new issues

**Test-First Development**:

- **For bug fixes**: Write a failing test that reproduces the bug FIRST
- **For new features**: Write tests that define expected behavior BEFORE implementation
- **For refactoring**: Ensure existing tests pass, add tests for edge cases if missing
- **Run tests**: Verify tests fail for the right reasons (proving they test the right thing)

**Test Coverage Requirements**:

- **Unit tests**: Test individual components in isolation (Core, Infrastructure layers)
- **Integration tests**: Test API endpoints and data access (API, Repository layers)
- **Component tests**: Test Blazor components with bUnit (Web layer)
- **E2E tests**: Test complete user workflows with Playwright - **MANDATORY for ALL UI changes**
- See [tests/AGENTS.md](tests/AGENTS.md) for comprehensive testing strategies

**🚨 CRITICAL E2E RULE**: NEVER skip E2E tests for UI/frontend changes:

- ALL URL routing changes → E2E tests REQUIRED
- ALL Blazor component changes → E2E tests REQUIRED
- ALL button/interaction changes → E2E tests REQUIRED
- ALL navigation changes → E2E tests REQUIRED
- See [tests/TechHub.E2E.Tests/AGENTS.md](tests/TechHub.E2E.Tests/AGENTS.md) for patterns

**When to Write Tests**:

- **ALWAYS** for bug fixes - reproduce the bug first (all layers including E2E)
- **ALWAYS** for new features - define expected behavior first (all layers including E2E)
- **ALWAYS** for API changes - test contracts and responses (integration + E2E)
- **ALWAYS** for UI/frontend changes - E2E tests are MANDATORY, not optional
- **ALWAYS** for URL routing changes - E2E tests verify navigation flows
- **ALWAYS** for component interactivity - E2E tests verify button clicks, forms, etc.
- **SKIP** for documentation-only changes (no code impact)
- **SKIP** for backend-only changes that don't affect user workflows (but keep integration tests)

**Key Benefits of Test-First**:

- Forces clear thinking about requirements and edge cases
- Prevents writing tests that just validate what you coded (confirmation bias)
- Ensures tests actually catch bugs (proven by initial failure)
- Makes refactoring safer with confidence
- Documents intended behavior clearly

**Example Workflow**:

1. **Understand the requirement**: Read specs, understand expected behavior
2. **Write failing test**: Test should fail because feature doesn't exist yet
3. **Run test**: Verify it fails for the right reason (not syntax errors)
4. **Implement**: Write minimal code to make test pass
5. **Run tests**: Verify test now passes
6. **Refactor**: Improve code quality while keeping tests green
7. **Repeat**: For next scenario/edge case

**Test Organization**:

- Place tests in corresponding test projects (TechHub.Core.Tests, TechHub.Api.Tests, etc.)
- Use descriptive test names: `MethodName_Scenario_ExpectedResult`
- Group related tests in same test class
- Use `[Theory]` for testing multiple similar scenarios
- See domain-specific AGENTS.md for test patterns

### 7. Implement Changes

**NOW implement** to make your tests pass:

**Implementation Guidelines**:

- **Write MINIMAL code** to make tests pass - avoid over-engineering
- Follow tool calling strategy: MCP → Built-in → CLI
- Use `replace_string_in_file` or `multi_replace_string_in_file` for code edits
- Use `get_errors` on modified files to check for VS Code diagnostics
- Fix all linting and compilation errors immediately
- Follow existing patterns and conventions in each language
- Use meaningful variable names and proper error handling
- Prefer clarity over cleverness
- Make parameters required unless they can be null
- Follow existing code patterns and conventions
- Use the latest version of code and libraries
- Always use the context7 MCP tool to fetch latest documentation when modifying code
- **Run tests frequently** during implementation to catch issues early

**Running and Testing**:

- See [Starting & Stopping the Website](#starting--stopping-the-website) for complete instructions
- Quick reference: Use `Run -OnlyTests` for automated testing, `Run -SkipTests` for interactive debugging

**Critical Requirements**:

- **Ask for clarification** on ANYTHING unclear - NEVER assume
- **For bigger changes**: ALWAYS ask before proceeding
- **Check for errors** after every file modification
- **Run tests** after each logical change to verify progress
- **Fix all issues** before moving to next step
- **DevContainer Dependencies**: Always install in `/workspaces/techhub/.devcontainer/post-create.sh`, never in PowerShell or other scripts

**Implementation Strategy**:

1. **Red**: Tests fail (Step 5 verified this)
2. **Green**: Write minimal code to make tests pass
3. **Refactor**: Improve code quality while keeping tests green
4. **Repeat**: For each test case / scenario

**Files to Update**:

- Code files (C#, JavaScript, PowerShell, Razor, etc.)
- Configuration files if needed
- Any related includes, layouts, or components

**Cleanup and Removal**:

- **ALWAYS clean up after changes** - Remove outdated files, code, and dependencies
- **No backwards compatibility unless requested** - Don't keep old implementations "just in case"
- **Remove unused files immediately** - Don't leave lingering code that's no longer referenced
- **Check documentation before removing critical files** - Update all references (e.g., when removing `sections.json`, update all docs mentioning it)
- **Delete deprecated code** - Remove old implementations after migration is complete
- **Clean up test code** - Remove obsolete test helpers, fixtures, or data files
- **Update configuration** - Remove unused settings, dependencies, and build artifacts

**Key Rules**:

- Write production code ONLY to make tests pass
- Don't add features not covered by tests
- Follow existing patterns and conventions
- Clean up outdated code and files immediately
- See [Core Rules & Boundaries](#1-core-rules--boundaries) for complete rule list

### 8. Validate & Fix

**Ensure all tests pass and code quality is high**:

**Run Full Test Suite**:

- **For AI agents**: Use `Run -OnlyTests` to run all tests (unit, integration, component, E2E) and exit
- **For humans**: Use `Run` to run tests and keep servers running for interactive debugging
- Run ALL affected tests (unit, integration, component, E2E as appropriate)
- **E2E tests are MANDATORY** for any UI/frontend changes - NO EXCEPTIONS
- Tests should NOW PASS (they failed in step 5, you fixed in step 6)
- Use test scripts appropriate for the domain you're working in
- See [tests/AGENTS.md](tests/AGENTS.md) for comprehensive testing strategy

**Check for Regressions**:

- Run full test suite to ensure no existing functionality broke
- Pay special attention to integration tests
- Verify E2E tests pass for critical user workflows
- All tests MUST pass before proceeding

**Code Quality Checks**:

- Use `get_errors` to check for linting/compilation errors
- Fix all warnings and errors
- Verify code follows project conventions
- Check for proper error handling
- Ensure logging is appropriate

**Performance Validation** (if applicable):

- Check that changes don't introduce performance regressions
- Verify caching still works correctly
- Test with realistic data volumes

**Key Rules**:

- ALL tests must pass before moving forward
- Zero tolerance for failing tests
- Fix issues immediately, don't postpone
- If tests fail, return to step 6 and fix implementation
- Never commit code with failing tests

### 9. Update Documentation

**🚨 MANDATORY**: Documentation MUST be updated whenever code behavior changes (Constitution Rule #7).

**Keep documentation in sync with code changes**:

**What to Update**:

- Domain-specific AGENTS.md files if behavior changed
- Functional documentation in `docs/` if features changed
- Code comments if complex logic was added
- README.md if user-facing changes occurred

**How to Find Documentation**:

- Use `grep_search` to find relevant docs mentioning modified features
- Check domain-specific AGENTS.md in directories you modified
- Review functional docs if feature behavior changed
- Update this file if architectural changes were made

**Documentation Placement Strategy**:

**🚨 CRITICAL**: See [docs/AGENTS.md](docs/AGENTS.md) for complete instructions on:

- **Where to place functional documentation** (docs/ directory)
- **Where to place technical documentation** (AGENTS.md files)
- **Where to place content guidelines** (collections/ directory)
- **What belongs in each file type**
- **How to avoid duplication**
- **Complete documentation placement examples**

**Quick Reference**:

- ✅ **Functional documentation** (WHAT the system does) → `docs/` directory
- ✅ **Technical documentation** (HOW to implement) → Domain-specific AGENTS.md files
- ✅ **Content guidelines** (writing standards) → `collections/` directory
- 🚫 **Never skip documentation** when behavior changes
- 🚫 **Never duplicate content** - link to existing docs instead
- 🚫 **Never mix functional and technical** - keep them separate

**Documentation Rules**:

- **ALWAYS update documentation** when code behavior changes (NOT optional)
- Search for existing docs before assuming none exist
- Keep docs accurate and up-to-date
- Follow markdown formatting guidelines (wrap symbol names in backticks, use KaTeX for equations)
- Follow [`collections/markdown-guidelines.md`](collections/markdown-guidelines.md) and [`collections/writing-style-guidelines.md`](collections/writing-style-guidelines.md)
- See [Documentation Architecture](#documentation-architecture) for complete doc map

**Key Rules**:

- This is NOT optional - always update docs
- Check for linting errors in markdown files
- Fix any MD032 or other markdown issues
- Documentation is part of "task complete"
- Work is NOT done until documentation is updated

### 10. Report Completion

**Tell the user you're done** with a clear summary:

**What to Report**:

- Concise summary of what was changed
- Links to modified files (use proper markdown link format below)
- Any important notes or caveats
- Confirmation that tests pass (if applicable)
- Confirmation that documentation is updated

**File Link Formatting Rules**:

- **NO BACKTICKS**: Never wrap file names, paths, or links in backticks
- **Required Formats**: File references: `[path/file.ts](path/file.ts)`, line references: `[file.ts:10](file.ts#L10)`, range references: `[file.ts:10-12](file.ts#L10-L12)`
- **Path Rules**: Use `/` only, encode spaces in target (`My%20File.md`), no `file://` or `vscode://` schemes
- **Forbidden**: Inline code (`file.ts`), plain text file names, bare line citations ("Line 86")

**Key Rules**:

- Only report completion when task is 100% done
- Never stop working until task is complete
- Include all relevant details in completion report

## Starting & Stopping the Website

**🚨 CRITICAL FOR AI AGENTS**: This section defines how to properly start, interact with, and stop the running website without breaking it.

### Starting the Website

**ALWAYS use the Run function** (automatically loaded in PowerShell):

```powershell
# Start both API and Web (browser never opens in DevContainer)
# Default behavior: runs tests, then keeps servers running
Run

# For automated testing (AI agents verifying changes)
Run -OnlyTests

# For interactive debugging with Playwright MCP (AI agents AND humans)
Run -SkipTests
```

**⚠️ CRITICAL E2E TEST WARNING**:

🚫 **NEVER** run `dotnet test tests/TechHub.E2E.Tests` directly - it **WILL FAIL** without servers running!  
✅ **ALWAYS** use `Run -OnlyTests` which handles server startup, testing, and shutdown automatically.

**CRITICAL RULES**:

✅ **DO**: Start website with `Run` in a dedicated terminal  
✅ **DO**: Let it run in the background - NEVER touch that terminal again  
✅ **DO**: Use Playwright MCP tools from GitHub Copilot Chat for all website testing  
✅ **DO**: Open NEW terminals for ANY other commands while website is running  
✅ **DO**: Use `Run -OnlyTests` for automated testing (run all tests, verify changes, exit)
✅ **DO**: Use `Run -SkipTests` for interactive debugging (AI agents AND humans using Playwright MCP)

🚫 **NEVER**: Type ANY command in the terminal running the website  
🚫 **NEVER**: Use curl, wget, or CLI tools in the website terminal  
🚫 **NEVER**: Run dotnet commands in the website terminal  
🚫 **NEVER**: Execute ANY operation that interacts with the website terminal  

**Why This Matters**: ANY interaction with the terminal running the website (typing a command, pressing Enter, Ctrl+C accidentally) will **IMMEDIATELY SHUTDOWN** the website and cause the command to fail.

### Testing the Running Website

**ALWAYS use Playwright MCP tools directly in GitHub Copilot Chat** - NO terminal commands needed:

```plaintext
# Navigate to page
mcp_playwright_browser_navigate(url: "http://localhost:5184")

# Take snapshots to see page structure
mcp_playwright_browser_snapshot()

# Take screenshots for visual verification
mcp_playwright_browser_take_screenshot()

# Interact with elements
mcp_playwright_browser_click(element: "button description")
mcp_playwright_browser_type(element: "input field", text: "test query")

# Verify behavior
# - Reproduce bugs
# - Verify expected behavior  
# - Understand features
# - Debug issues
```

**Benefits of Playwright MCP**:

- Works directly from GitHub Copilot Chat - no terminal needed
- Never risks shutting down the website
- Provides rich snapshots and screenshots
- Allows complex interactions (click, type, navigate)
- Can verify page state and behavior

### If CLI Tools Are Absolutely Required

**ONLY if Playwright MCP cannot accomplish the task**, and you MUST use curl/wget/other CLI tools:

```powershell
# Open a NEW terminal (NEVER use the website terminal)
# Then run your command
curl http://localhost:5184/api/sections
```

**Terminal Safety Checklist**:

- [ ] Website is running in Terminal 1 (DO NOT TOUCH)
- [ ] Opened NEW Terminal 2 for commands
- [ ] Verified I'm typing in Terminal 2, NOT Terminal 1
- [ ] Command does not interact with Terminal 1 in any way

### Stopping the Website

**Only stop when task is complete or restart is needed**:

1. Switch to the terminal running `Run`
2. Press `Ctrl+C` to gracefully stop both API and Web servers
3. Wait for "Cleanup complete" message
4. Terminal is now safe to use for other commands

**The Run function handles**:

- Aspire AppHost orchestration (starts both API and Web)
- Health checks before declaring ready (up to 60 seconds for Aspire startup)
- Graceful shutdown of all processes
- Port cleanup on exit
- Clean console output (warnings/errors only, info logs suppressed)

### Run Function Parameters (for AI Agents)

**Common Options**:

- **Default** (no args) - Run tests, then keep servers running
- `-OnlyTests` - Run all tests, then exit (for automated testing and verification)
- `-SkipTests` - Skip tests, start servers directly (for interactive debugging with Playwright MCP)
  - **AI agents**: Use this when investigating bugs, testing UI, or exploring behavior interactively
  - **Humans**: Use this when manually testing or using Playwright MCP tools
  - **Why**: Playwright MCP is faster than writing tests for exploration and debugging
- `-Clean` - Clean all build artifacts before building
- `-Build` - Build only, don't run servers

**Examples**:

```powershell
# Automated testing - verify all changes work
Run -OnlyTests

# Interactive debugging - AI agents OR humans using Playwright MCP
Run -SkipTests

# Default - run tests first, then keep servers running
Run

# Clean build and test first
Run -Clean

# Build only, don't run
Run -Build
```

**Built-in Features**:

- **Port Cleanup**: Automatically kills processes using required ports before starting
- **Ctrl+C Handling**: Properly stops all processes and cleans up ports when interrupted
- **Conflict Prevention**: Safe to run even if ports are already in use
- **Health Checks**: Verifies services are responding before declaring ready

### Building/Testing Individual Projects

**ALWAYS prefer the Run function for build and test operations**:

```powershell
# Build and test everything (recommended)
Run -OnlyTests

# Build everything with clean slate
Run -Clean -OnlyTests

# Build only
Run -Build
```

**Only use low-level dotnet commands when the Run function doesn't support the operation**:

```powershell
# Restore NuGet packages (not in Run function)
dotnet restore

# Clean build artifacts only (without rebuild)
dotnet clean TechHub.slnx

# Watch mode for development (not in Run function)
dotnet watch --project src/TechHub.Api/TechHub.Api.csproj
dotnet watch --project src/TechHub.Web/TechHub.Web.csproj

# Code coverage (not in Run function)
dotnet test --collect:"XPlat Code Coverage"

# Entity Framework migrations (future, not in Run function)
dotnet ef migrations add MigrationName --project src/TechHub.Infrastructure
dotnet ef database update --project src/TechHub.Api

# Global tools management (not in Run function)
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

**Why prefer the Run function**:

- Handles server startup/shutdown correctly
- Runs E2E tests with proper infrastructure
- Cleans up ports automatically
- Provides consistent experience across operations
- Prevents common errors (like running E2E tests without servers)

## Documentation Architecture

The Tech Hub uses a **multi-tier documentation system** organized by scope and domain. Since we're committed to .NET as our permanent tech stack, documentation is separated by **scope** (repository-wide vs domain-specific), not by framework.

### Documentation Hierarchy

**1. Root AGENTS.md** (this file):

- Repository-wide development principles and standards
- AI Assistant Workflow (10-step process for all development)
- Core rules and boundaries
- .NET tech stack, development commands, and common patterns
- Performance architecture, accessibility standards, timezone handling
- Configuration-driven development principles
- Repository structure and site terminology
- **Scope**: Applies to ALL work across the entire repository

**2. Domain-Specific AGENTS.md Files**:

- Development patterns for specific code domains (src/, scripts/, tests/, collections/, etc.)
- Located in each major directory
- Domain-specific rules, patterns, and examples
- **Scope**: Applies only when working in that specific domain

**3. Functional Documentation** (`docs/`):

- WHAT the system does (behavior, contracts, rules)
- Minimal set: filtering, content management, API specification
- Describes features and capabilities, not implementation
- **Scope**: Understanding system behavior and architecture

**4. Content Guidelines** (`collections/`):

- Writing standards and markdown formatting rules
- Content creation and management workflows
- **Scope**: Creating and maintaining content

### Documentation Placement Strategy

**Where to Place Information**:

**Root AGENTS.md** (this file):

- Development workflow and process (10 steps, TDD, etc.)
- Core rules and boundaries (always/ask/never rules)
- .NET tech stack overview (what we use: .NET 10, Blazor, etc.)
- .NET development commands (dotnet build, test, run, etc.)
- Repository-wide principles (performance, accessibility, timezone, configuration-driven design)
- Site terminology and concepts

**src/AGENTS.md** (ALL .NET implementation patterns):

- Minimal API endpoint patterns
- Blazor component patterns (code-behind, etc.)
- Repository pattern implementation
- Dependency injection patterns and service lifetimes
- Domain models and DTOs
- Markdown frontmatter mapping
- HttpClient configuration
- ALL other .NET code patterns

**tests/AGENTS.md** (Testing strategies):

- Testing strategies across all frameworks (unit, integration, component, E2E)
- When to write tests and what to test
- Test organization and naming
- References src/AGENTS.md for implementation patterns when writing test code

**Domain-specific AGENTS.md** (src/TechHub.Web/, src/TechHub.Api/, etc.):

- Patterns specific to that project/domain
- Project-specific configuration
- Specialized tooling for that domain

**Functional Docs** (docs/):

- Feature descriptions (WHAT the system does)
- API contracts and endpoint specifications
- Business rules and behavior
- System architecture diagrams

**Content Guidelines** (collections/):

- Markdown formatting standards
- Writing style and tone
- Content workflow and RSS processing

**Key Principle**: Place information at the **highest applicable level**:

- If it's a .NET pattern used across projects → src/AGENTS.md
- If it's specific to one project → Domain AGENTS.md (e.g., src/TechHub.Web/AGENTS.md)
- If it's repository-wide workflow/rules → Root AGENTS.md
- If it describes behavior → Functional docs
- If it's about content → Content guidelines

### Complete Documentation Map

**Project Overview**:

- **[README.md](README.md)** - Project overview, quick start guide, architecture summary, current implementation status, and navigation to all other documentation

**Domain-Specific AGENTS.md Files**:

- **[src/AGENTS.md](src/AGENTS.md)** - .NET development patterns across all projects
- **[src/TechHub.Api/AGENTS.md](src/TechHub.Api/AGENTS.md)** - API development patterns (Minimal APIs, endpoints)
- **[src/TechHub.Web/AGENTS.md](src/TechHub.Web/AGENTS.md)** - Blazor component patterns (SSR, interactivity)
- **[src/TechHub.Core/AGENTS.md](src/TechHub.Core/AGENTS.md)** - Domain model design (records, DTOs)
- **[src/TechHub.Infrastructure/AGENTS.md](src/TechHub.Infrastructure/AGENTS.md)** - Data access patterns (repositories, caching)
- **[scripts/AGENTS.md](scripts/AGENTS.md)** - PowerShell automation scripts
- **[docs/AGENTS.md](docs/AGENTS.md)** - Documentation maintenance guidelines
- **[collections/AGENTS.md](collections/AGENTS.md)** - Content creation and management
- **[tests/AGENTS.md](tests/AGENTS.md)** - Testing strategies across all frameworks
- **[tests/TechHub.E2E.Tests/AGENTS.md](tests/TechHub.E2E.Tests/AGENTS.md)** - E2E test architecture and performance optimizations

**Functional Documentation** (in `docs/`):

- **[filtering-system.md](docs/filtering-system.md)** - How tag and date filtering works
- **[content-management.md](docs/content-management.md)** - Content workflows and RSS processing
- **[api-specification.md](docs/api-specification.md)** - REST API contracts and endpoints

**Content Guidelines** (in `collections/`):

- **[markdown-guidelines.md](collections/markdown-guidelines.md)** - Markdown formatting rules
- **[writing-style-guidelines.md](collections/writing-style-guidelines.md)** - Writing tone and style

### Quick Reference Guide

**New to the project?**

1. Start with [README.md](README.md) for overview and quick start
2. Read this file (AGENTS.md) for development workflow
3. Review domain-specific AGENTS.md before coding

**Working on .NET/Blazor?**

1. Review .NET sections in this file (Tech Stack, Commands, Patterns)
2. Read [src/AGENTS.md](src/AGENTS.md) for general .NET patterns
3. Read specific domain AGENTS.md (API, Web, Core, Infrastructure)

**Working on PowerShell scripts?**

1. Read [scripts/AGENTS.md](scripts/AGENTS.md)
2. Review [Starting & Stopping the Website](#starting--stopping-the-website) for build/test commands

**Working on content?**

1. Read [collections/AGENTS.md](collections/AGENTS.md)
2. Follow [markdown-guidelines.md](collections/markdown-guidelines.md)
3. Follow [writing-style-guidelines.md](collections/writing-style-guidelines.md)

**Working on tests?**

1. Read [tests/AGENTS.md](tests/AGENTS.md) for testing strategies
2. Read specific test domain AGENTS.md:
   - [tests/TechHub.E2E.Tests/AGENTS.md](tests/TechHub.E2E.Tests/AGENTS.md) - E2E test patterns
   - [tests/TechHub.Api.Tests/AGENTS.md](tests/TechHub.Api.Tests/AGENTS.md) - API integration test patterns
   - [tests/TechHub.Web.Tests/AGENTS.md](tests/TechHub.Web.Tests/AGENTS.md) - Blazor component test patterns
   - [tests/TechHub.Core.Tests/AGENTS.md](tests/TechHub.Core.Tests/AGENTS.md) - Unit test patterns
   - [tests/TechHub.Infrastructure.Tests/AGENTS.md](tests/TechHub.Infrastructure.Tests/AGENTS.md) - Infrastructure test patterns
   - [tests/powershell/AGENTS.md](tests/powershell/AGENTS.md) - PowerShell test patterns
3. Review [Starting & Stopping the Website](#starting--stopping-the-website) for build/test commands

**Understanding system behavior?**

1. Read [docs/filtering-system.md](docs/filtering-system.md) for filtering
2. Read [docs/content-management.md](docs/content-management.md) for content workflows
3. Read [docs/api-specification.md](docs/api-specification.md) for API contracts

## Core Development Principles

### Tech Stack

**Runtime & Core Frameworks**:

- .NET 10 (latest LTS - November 2025)
- C# 13 with nullable reference types
- ASP.NET Core Minimal API (backend)
- Blazor SSR + WebAssembly (frontend)
- .NET Aspire (orchestration, observability, service discovery)

**Frontend Technologies**:

- HTML5 semantic markup
- CSS3 with modern features (Grid, Flexbox, Custom Properties)
- Vanilla JavaScript (ES2024+) for progressive enhancement
- No JavaScript frameworks - pure web standards

**Testing & Quality**:

- xUnit (unit and integration tests)
- bUnit (Blazor component tests)
- Moq (mocking framework)
- Playwright (E2E tests)
- PowerShell Pester (script tests)

**Infrastructure & Deployment**:

- Azure Container Apps
- Bicep Infrastructure as Code
- OpenTelemetry + Application Insights
- GitHub Actions (CI/CD)

**Development Tools**:

- PowerShell 7+ (automation scripts)
- Git (version control)
- VS Code DevContainers (consistent development environment)
- Markdown (documentation)

**.NET Aspire**:

Tech Hub uses Aspire for orchestration and observability:

- **AppHost** (`src/TechHub.AppHost/`) - Orchestrates API + Web services
- **ServiceDefaults** (`src/TechHub.ServiceDefaults/`) - Shared configuration for OpenTelemetry, health checks, resilience
- **Service Discovery** - Web finds API via `https+http://api` URL scheme
- **Aspire Dashboard** - Real-time traces, metrics, and logs visualization

**Running with Aspire**:

```powershell
# Default - uses Aspire AppHost with built-in dashboard
Run

# Dashboard URL: https://localhost:17101 (URL with token shown in startup output)
```

**Implementation Guidance**:

For ALL .NET code patterns, examples, and best practices, see **[src/AGENTS.md](src/AGENTS.md)**:

- Minimal API endpoint patterns
- Blazor component patterns (code-behind, SSR)
- Repository pattern implementation
- Dependency injection and service lifetimes
- Domain models and DTOs
- HttpClient configuration and resilience
- Documentation resources (context7 MCP queries)
- All other .NET development patterns

**When writing .NET code** (including tests), ALWAYS read [src/AGENTS.md](src/AGENTS.md) for implementation patterns.

### Architectural Principles

**Configuration-Driven Design**:

- All sections and collections defined in `_data/sections.json`
- Content structure managed through data files, not code
- New sections added by updating configuration
- Single source of truth ensures consistency

**Performance & User Experience**:

- Server-side rendering for fast initial page loads
- Client-side enhancement for responsive interactions
- Pre-computation during build for optimal runtime performance
- Resilience policies (retry, circuit breaker, timeout)

**Quality & Maintainability**:

- Test-driven development at all layers
- Comprehensive test coverage (unit, integration, component, E2E)
- Clean architecture with separation of concerns
- Zero-warning policy for code quality

### Performance Architecture

**Server-Side First, Client-Side Performance**:

- **Server-side rendering is preferred** - Pre-compute and render all content during build time
- **Client-side performance is critical** - After initial load, all interactions must be responsive
- **Pre-load during application start** - Calculate once at build time for optimal server AND client performance
- **Never sacrifice functionality for performance** - Use intelligent caching and pre-computation instead
- **JavaScript enhances, never creates** - Initial content must never depend on JavaScript execution

**Core Performance Principles**:

1. **Server-side first** - All content must be rendered server-side for initial page load
2. **Client-side performance is paramount** - After initial render, all interactions must be responsive
3. **Never sacrifice functionality** - Don't reduce content amount or visual elements for performance
4. **Server-side can be slower** - Build-time processing can take longer if it benefits the client
5. **JavaScript enhances, never creates** - Initial content must never depend on JavaScript

**The Server-Side First Rule**:

All visible content must be fully rendered server-side by the build system. Users must see complete, functional content immediately upon page load. This ensures optimal SEO, performance, and accessibility.

**The ONLY exception**: `assets/js/sections.js` is allowed to modify section collections state on page load based on URL parameters. All other JavaScript must wait for user interaction.

**Progressive Enhancement**:

- JavaScript enhances server-rendered content rather than creating it
- Core functionality works without JavaScript
- Load core functionality first, then enhance progressively
- Monitor memory usage for leaks in long-running sessions

**Image and Asset Performance**:

- Use lazy loading for large images to improve initial page performance
- Implement responsive images with proper sizing attributes
- Use modern image formats when supported
- Optimize asset bundling to reduce HTTP requests

**Mobile and Cross-Browser Performance**:

- Test loading speeds on mobile networks and various connection speeds
- Ensure touch interactions are responsive
- Test across different browsers and versions
- Use profiling tools to identify and fix bottlenecks
- Verify performance standards are met across all target devices

### Accessibility Standards

**Critical Accessibility Requirements**:

All user interface components and interactions must be accessible to users with disabilities following WCAG 2.1 Level AA standards.

**Keyboard Navigation**:

- All interactive elements must be keyboard accessible
- Implement proper focus states with visible indicators
- Support standard keyboard shortcuts and navigation patterns
- Ensure logical tab order through interactive content

**Visual Accessibility**:

- Maintain color contrast ratios of at least 4.5:1 for normal text (WCAG AA)
- Maintain color contrast ratios of at least 3:1 for large text and UI components
- Never rely solely on color to convey information
- Provide text alternatives for images and non-text content

**Screen Reader Support**:

- Use semantic HTML elements appropriately
- Provide descriptive ARIA labels where needed
- Ensure all interactive controls have accessible names
- Test with screen readers to verify accessibility

**Testing Requirements**:

- Test with keyboard-only navigation
- Verify screen reader compatibility
- Check color contrast with automated tools
- Ensure responsive design works with text scaling and zoom

### Configuration-Driven Development

**Never Hardcode Values**:

- Always derive sections, collections from configuration files
- Single source of truth for all structural data
- New features should work by updating configuration, not code
- All components must stay synchronized automatically through configuration

**Benefits**: Changes to site structure require only configuration updates, no code modifications needed.

### Timezone & Date Handling

**Consistent Timezone**:

- **Define once**: `Europe/Brussels` timezone for all date operations
- **Apply everywhere**: Build process, templates, client code must all use same timezone
- **Store universally**: Use Unix epoch timestamps as primary storage format
- **Display locally**: Convert to timezone only when rendering for users

**Date Format Standards**:

- **Input**: `YYYY-MM-DD` (ISO 8601) or `YYYY-MM-DD HH:MM:SS +ZONE`
- **Storage**: Unix epoch timestamps (integers, timezone-agnostic)
- **Display**: Generated dynamically at runtime from epoch
- **URL Parameters**: ISO date format for consistency

**Critical Rules**:

- All date calculations must respect the configured timezone
- Normalize dates to midnight in configured timezone for comparisons
- Server-side and client-side date handling must be synchronized
- Never assume UTC - always use configured timezone

**Implementation Details**:

- Server-side: See [Tech Stack](#tech-stack) section for patterns reference
- Client-side: See domain-specific AGENTS.md files for client-side patterns

**Benefits**: Prevents date/time bugs, ensures consistent behavior across all systems, simplifies date comparisons.

## Site Terminology

### Core Concepts

**Sections**: Top-level organizational units that group related content by topic or domain.

- **Purpose**: Provide thematic organization (e.g., AI, GitHub Copilot, Azure)
- **Configuration**: Defined in `appsettings.json` as single source of truth
- **Properties**: Each section includes display title, description, URL path, background image, and collections
- **Key Features**: Dynamic and configuration-driven - new sections added without code changes, each has own index page and navigation

**Collections**: Content types that represent different formats within sections.

- **Purpose**: Organize content by format and purpose (news, videos, community, blogs, roundups)
- **Configuration**: Defined in `appsettings.json`, associated with sections via section configuration
- **Technical**: Each collection has its own directory, can be marked as custom (manually created) or auto-generated
- **Properties**: Collections generate individual pages for each item via Blazor routing

**Items**: Individual pieces of content within collections. Also referred to as content or content items.

- **Definition**: Actual content users consume (articles, videos, announcements, blogs)
- **Terminology Note**: "Item" is the preferred term, but "Article" and "Post" are also used in code/documentation to refer to content (note: "Post" in variables does NOT specifically mean blogs from `_blogs/`)
- **Structure**: Markdown files with YAML front matter containing metadata (title, date, author, sections, tags) and content body
- **Section Names Frontmatter Field**: The `section_names` field in frontmatter contains section names (e.g., "ai", "gitHub_copilot") that determine which sections this content appears in.
- **Processing**: Items are processed by the build system and can be listed on collection pages, filtered by date/tags/sections, displayed on section index pages, and included in RSS feeds

### Content Organization

**Relationship Between Concepts**:

1. **Sections** contain multiple **Collections**
2. **Collections** contain multiple **Items** (also called content or content items)
3. **Items** have metadata (dates, tags) used by filtering systems
4. Build-time processing prepares all data for client-side consumption
5. Client-side filtering provides interactive content discovery

**Collections**:

- **News**: Official product updates and announcements
- **Videos**: Educational and informational video content (may include special subfolders with `alt-collection` frontmatter)
- **Community**: Community-sourced content and discussions
- **Blogs**: Blogs
- **Roundups**: Curated weekly content summaries

**Alt-Collection**: Optional frontmatter field for content organized in subfolders (e.g., `_videos/ghc-features/`, `_videos/vscode-updates/`) that need special categorization beyond their parent collection.

### Filtering Systems

**Date Filters**: Client-side filtering by publication date ranges (e.g., "Last 30 days").

**Tag Filters**: Client-side filtering by normalized tags for content discovery.

**Text Search**: Real-time search across titles, descriptions, and tags with debounced input.

### Content Structure

**Excerpt**: Introduction that summarizes main points (max 200 words, followed by `<!--excerpt_end-->`).

**Content**: Main detailed markdown content following the excerpt.

### RSS Feeds

The site provides RSS feeds for all sections and collections.

**For complete RSS feed documentation**, see [docs/rss-feeds.md](docs/rss-feeds.md).

**Quick Reference**:

- **Everything**: `/api/rss/all` - All content across all sections
- **Section Feeds**: `/api/rss/{sectionName}` - Content for a specific section
- **Collection Feeds**: `/api/rss/collection/{collectionName}` - Content for a specific collection type
