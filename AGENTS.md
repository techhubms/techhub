# Tech Hub Development Guide

**AI CONTEXT**: This is the **ROOT** development guide. It defines repository-wide principles, architecture, and workflow. When working in a specific domain (e.g., `src/`, `scripts/`, `tests/`), **ALSO** read the domain-specific `AGENTS.md` file in that directory.

**🚨 ABSOLUTELY CRITICAL**: This section defines a **required 9-step process** for all development tasks in [AI Assistant Workflow](#ai-assistant-workflow). Always follow these steps in order for every request.
**🚨 ABSOLUTELY CRITICAL**: Always read the root [README.md](/README.md) before starting any work to understand the context of this repository better.**

## Index

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
- [.NET Development Commands](#net-development-commands)
- [.NET Migration Status](#net-migration-status)
- [.NET Tech Stack](#net-tech-stack)
- [.NET Patterns & Examples](#net-patterns--examples)
  - [Minimal API Endpoints](#minimal-api-endpoints)
  - [Blazor Components with Code-Behind](#blazor-components-with-code-behind)
  - [Repository Pattern Implementation](#repository-pattern-implementation)
  - [Dependency Injection Service Lifetimes](#dependency-injection-service-lifetimes)
  - [Domain Models with Records](#domain-models-with-records)
  - [Markdown Frontmatter Mapping](#markdown-frontmatter-mapping)
  - [Dependency Injection Configuration](#dependency-injection-configuration)
- [Project Overview](#project-overview)
- [Documentation Architecture](#documentation-architecture)
  - [Documentation Hierarchy](#documentation-hierarchy)
  - [Documentation Placement Strategy](#documentation-placement-strategy)
  - [Complete Documentation Map](#complete-documentation-map)
  - [Quick Reference Guide](#quick-reference-guide)
- [Core Development Principles](#core-development-principles)
  - [Performance Architecture](#performance-architecture)
  - [Accessibility Standards](#accessibility-standards)
  - [Configuration-Driven Development](#configuration-driven-development)
  - [Timezone & Date Handling](#timezone--date-handling)
- [Repository Organization](#repository-organization)
  - [Core Directories](#core-directories)
  - [Project Structure](#project-structure)
- [Site Terminology](#site-terminology)
  - [Core Concepts](#core-concepts)
  - [Content Organization](#content-organization)
  - [Filtering Systems](#filtering-systems)
  - [RSS Feeds](#rss-feeds)

## AI Assistant Workflow

### 1. Core Rules & Boundaries

These are the **non-negotiable rules** that apply to ALL development tasks. ALWAYS follow these rules without exception.

#### ✅ Always Do

- **Always follow the 9-step workflow**: Complete all steps in order for every request
- **Always write tests BEFORE implementation**: Test-Driven Development (TDD) is mandatory
- **Always prefer higher-level tools**: ALWAYS use MCP tools > Built-in tools > CLI commands
  - **MCP tools** (highest priority): Playwright MCP (web testing), GitHub MCP (GitHub operations), context7 MCP (documentation)
  - **Built-in tools**: `replace_string_in_file` (with 5-10 lines context), `read_file`, `grep_search`, `file_search`
  - **CLI** (lowest priority): Only for complex multi-step operations not supported by tools
- **Always check for errors after editing files**: Use `get_errors` tool on modified files to check VS Code diagnostics (markdown linting, ESLint, RuboCop, etc.) and fix all issues
- **Always run tests after modifying code**: CRITICAL - After ANY code changes (C#, JavaScript, PowerShell, templates), run appropriate test suites. Documentation-only changes do not require testing
- **Always fix linter issues**: Always resolve all linting errors and warnings, EXCEPT intentional bad examples in documentation
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

- **Never skip the 9-step workflow**: All steps are required for quality work
- **Never write implementation before tests**: TDD is mandatory for code changes
- **Never skip E2E tests for UI changes**: E2E tests are MANDATORY for frontend, not optional
- **Never use lower-level tools unnecessarily**: Don't use CLI when MCP or built-in tools are available
- **Never paste scripts into terminal**: Always save as `.ps1` file in `.tmp/` and execute
- **Never use `pwsh -Command` with EOF or other large text demarcations**: Save script to file and execute it
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
- Read domain-specific AGENTS.md file for the area you'll modify (e.g., [jekyll/_plugins/AGENTS.md](jekyll/_plugins/AGENTS.md), [jekyll/assets/js/AGENTS.md](jekyll/assets/js/AGENTS.md), [scripts/AGENTS.md](scripts/AGENTS.md))
- Review functional documentation in `docs/` if working on features (e.g., [docs/filtering-system.md](docs/filtering-system.md))
- Check [collections/markdown-guidelines.md](collections/markdown-guidelines.md) and [collections/writing-style-guidelines.md](collections/writing-style-guidelines.md) for content work

**Scan the Code**:

- Use `read_file` to examine relevant files
- Use `grep_search` or `semantic_search` to find related code patterns
- Use `list_dir` to understand directory structure
- Check existing tests in `spec/` to understand expected behavior

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

- **context7 MCP tool**: Get up-to-date documentation for any framework/library (REQUIRED for Jekyll, JavaScript libraries, etc.)
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

**Running the Website Locally**:

- **ALWAYS use `./run.ps1` to start the website** - it handles both API and Web projects correctly
- **Start in background**: Use `./run.ps1` and let it run in the background (browser never opens in DevContainer)
- **Reuse terminal**: Once started, NEVER touch that terminal window again
- **New operations**: Use a NEW terminal for any other commands while website is running
- **To test**: Use Playwright MCP tools directly in GitHub Copilot Chat (no terminal commands needed)
- **To stop**: Only touch the terminal to stop the server (Ctrl+C)
- **For building/testing individual projects**: Use specific dotnet commands (e.g., `dotnet build src/TechHub.Api/TechHub.Api.csproj`)
- **The run.ps1 script ensures**: Proper startup order, health checks, and graceful shutdown for the entire website

**Use Playwright MCP Server for Testing**:

- **CRITICAL**: Playwright MCP tools work DIRECTLY in GitHub Copilot Chat - no terminal commands needed
- **Navigate to page**: `mcp_playwright_browser_navigate` to <http://localhost:5184>
- **Take snapshots**: `mcp_playwright_browser_snapshot` to capture page state
- **Take screenshots**: `mcp_playwright_browser_take_screenshot` for visual verification
- **Interact with elements**: `mcp_playwright_browser_click`, `mcp_playwright_browser_type`, etc.
- **Verify behavior**: Reproduce bugs, verify expected behavior, understand features
- **Debug issues**: Capture screenshots or console output for debugging

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
- NEVER run terminal commands for Playwright testing - use MCP tools directly

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

**Starting/Stopping the Application**:

- See [Starting & Stopping the Website](#starting--stopping-the-website) for complete instructions on running and testing the website
- **For automated testing**: Use `./run.ps1 -OnlyTests` (runs all tests, exits - for verifying changes)
- **For interactive debugging**: Use `./run.ps1 -SkipTests` (AI agents AND humans using Playwright MCP tools)
- **Default behavior**: Use `./run.ps1` (runs tests first, then keeps servers running)
- **IMPORTANT**: AI agents should use Playwright MCP for interactive debugging AND write tests that reproduce the debugged issues so they don't happen again

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

- **For AI agents**: Use `./run.ps1 -OnlyTests` to run all tests (unit, integration, component, E2E) and exit
- **For humans**: Use `./run.ps1` to run tests and keep servers running for interactive debugging
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

**Documentation Rules**:

- **ALWAYS update documentation** when code behavior changes
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
- **Required Formats**: `[path/file.ts](path/file.ts)` (file), `[file.ts](file.ts#L10)` (line), `[file.ts](file.ts#L10-L12)` (range)
- **Path Rules**: Use `/` only, encode spaces in target (`My%20File.md`), no `file://` or `vscode://` schemes
- **Forbidden**: Inline code (`file.ts`), plain text file names, bare line citations ("Line 86")

**Key Rules**:

- Only report completion when task is 100% done
- Never stop working until task is complete
- Include all relevant details in completion report

## Starting & Stopping the Website

**🚨 CRITICAL FOR AI AGENTS**: This section defines how to properly start, interact with, and stop the running website without breaking it.

### Starting the Website

**ALWAYS use the run.ps1 script**:

```powershell
# Start both API and Web (browser never opens in DevContainer)
# Default behavior: runs tests, then keeps servers running
./run.ps1

# For automated testing (AI agents verifying changes)
./run.ps1 -OnlyTests

# For interactive debugging with Playwright MCP (AI agents AND humans)
./run.ps1 -SkipTests
```

**⚠️ CRITICAL E2E TEST WARNING**:

🚫 **NEVER** run `dotnet test tests/TechHub.E2E.Tests` directly - it **WILL FAIL** without servers running!  
✅ **ALWAYS** use `./run.ps1 -OnlyTests` which handles server startup, testing, and shutdown automatically.

**CRITICAL RULES**:

✅ **DO**: Start website with `./run.ps1` in a dedicated terminal  
✅ **DO**: Let it run in the background - NEVER touch that terminal again  
✅ **DO**: Use Playwright MCP tools from GitHub Copilot Chat for all website testing  
✅ **DO**: Open NEW terminals for ANY other commands while website is running  
✅ **DO**: Use `./run.ps1 -OnlyTests` for automated testing (run all tests, verify changes, exit)
✅ **DO**: Use `./run.ps1 -SkipTests` for interactive debugging (AI agents AND humans using Playwright MCP)

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

1. Switch to the terminal running `./run.ps1`
2. Press `Ctrl+C` to gracefully stop both API and Web servers
3. Wait for "Cleanup complete" message
4. Terminal is now safe to use for other commands

**The run.ps1 script handles**:

- Proper startup order (API first, then Web)
- Health checks before declaring ready
- Graceful shutdown of both processes
- Port cleanup on exit

### run.ps1 Script Parameters (for AI Agents)

**Common Options**:

- **Default** (no args) - Run tests, then keep servers running
- `-OnlyTests` - Run all tests, then exit (for automated testing and verification)
- `-SkipTests` - Skip tests, start servers directly (for interactive debugging with Playwright MCP)
  - **AI agents**: Use this when investigating bugs, testing UI, or exploring behavior interactively
  - **Humans**: Use this when manually testing or using Playwright MCP tools
  - **Why**: Playwright MCP is faster than writing tests for exploration and debugging
- `-Clean` - Clean all build artifacts before building
- `-SkipBuild` - Skip build, use existing binaries
- `-ApiOnly` - Only run the API project
- `-WebOnly` - Only run the Web project
- `-Release` - Build in Release mode
- `-VerboseOutput` - Show verbose output for debugging

**Examples**:

```powershell
# Automated testing - verify all changes work
./run.ps1 -OnlyTests

# Interactive debugging - AI agents OR humans using Playwright MCP
./run.ps1 -SkipTests

# Default - run tests first, then keep servers running
./run.ps1

# Clean build and test first
./run.ps1 -Clean

# Only API for backend testing
./run.ps1 -ApiOnly

# Only Web for frontend testing
./run.ps1 -WebOnly
```

**Script Built-in Features**:

- **Port Cleanup**: Automatically kills processes using required ports before starting
- **Ctrl+C Handling**: Properly stops all processes and cleans up ports when interrupted
- **Conflict Prevention**: Safe to run even if ports are already in use
- **Health Checks**: Verifies services are responding before declaring ready

### Building/Testing Individual Projects

**For building or testing specific projects WITHOUT running the website**, use dotnet commands directly in ANY terminal:

```powershell
# Build specific project
dotnet build src/TechHub.Api/TechHub.Api.csproj
dotnet build src/TechHub.Web/TechHub.Web.csproj

# Run tests
dotnet test
dotnet test tests/TechHub.Core.Tests

# Build entire solution
dotnet build TechHub.slnx
```

These commands are safe to run anytime because they don't start the website.

## .NET Development Commands

**Build Commands**:

```powershell
# Build all projects
dotnet build TechHub.slnx

# Build specific project
dotnet build src/TechHub.Api/TechHub.Api.csproj
dotnet build src/TechHub.Web/TechHub.Web.csproj

# Clean build artifacts
dotnet clean TechHub.slnx

# Restore NuGet packages
dotnet restore
```

**Test Commands**:

```powershell
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/TechHub.Core.Tests
dotnet test tests/TechHub.Api.Tests
dotnet test tests/TechHub.Web.Tests
dotnet test tests/TechHub.E2E.Tests

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

**Watch Mode** (auto-rebuild on file changes):

```powershell
# Watch API project
dotnet watch --project src/TechHub.Api/TechHub.Api.csproj

# Watch Web project
dotnet watch --project src/TechHub.Web/TechHub.Web.csproj
```

**Entity Framework** (future):

```powershell
# Add migration
dotnet ef migrations add MigrationName --project src/TechHub.Infrastructure

# Update database
dotnet ef database update --project src/TechHub.Api

# Remove last migration
dotnet ef migrations remove --project src/TechHub.Infrastructure
```

**Global Tools**:

```powershell
# Install global tools
dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspire

# Update global tools
dotnet tool update --global dotnet-ef
```

## .NET Migration Status

**Project Status**: 🚧 Currently migrating from Jekyll to .NET/Blazor architecture with separate API and frontend.

**Current Phase**: Phase 3 - User Story 1 MVP (API Implementation) ✅ Partially Complete

### Implementation Progress

Following the migration plan phases defined in [specs/dotnet-migration/](specs/dotnet-migration/):

- **Phase 1: Foundation** (36/36 tasks) ✅ Complete
  - All projects, domain models, DTOs, interfaces, extensions
- **Phase 2: Data Access** (8/17 tasks) 🔄 In Progress
  - ✅ FrontMatterParser (11 tests passing)
  - ✅ MarkdownService (19 tests passing)
  - ✅ FileBasedSectionRepository (7 tests passing)
  - ✅ FileBasedContentRepository (15 tests passing)
  - ⏳ RssService, Caching, Entity tests (not started)
- **Phase 3: API Endpoints** (5/70 tasks) 🔄 In Progress
  - ✅ All section endpoints (6 endpoints, 8 tests)
  - ✅ Advanced filtering (2 endpoints, 6 tests)
  - ⏳ Blazor components, pages, client (not started)

**Test Results**: 52/52 tests passing (100% pass rate)

**What's Working Now**:

✅ **RESTful API** with 14 endpoints, all tested and working  
✅ **Frontend** - Home page with 8 sections, responsive grid, design system  
✅ **Components** - SectionCard, ContentItemCard with Tech Hub styling  
✅ **HTTP Client** - TechHubApiClient with resilience policies  
✅ **Visual Design** - Complete color system from Jekyll _sass  
✅ **Images** - All 8 section background images  

**Access Points**:

- Web UI: <http://localhost:5184>
- API: <http://localhost:5029/api/sections>
- Swagger: <http://localhost:5029/swagger>

**Next Steps**:

1. Complete Phase 2: RssService, Caching, Entity tests (T045-T051)
2. Continue Phase 3: Section/content detail pages, filtering, accessibility (T062-T087)
3. Begin Phase 4: Features implementation (filtering, search, infinite scroll)

See [specs/dotnet-migration/tasks.md](specs/dotnet-migration/tasks.md) for complete task breakdown and [README.md](README.md) for user-facing quick start guide.

## .NET Tech Stack

**.NET Runtime & Framework**:

- .NET 10 (latest LTS - November 2025)
- C# 13 with nullable reference types enabled
- File-scoped namespaces

**Frontend (Blazor)**:

- Blazor Server-Side Rendering (SSR) for SEO
- Blazor WebAssembly for enhanced interactivity
- Typed HttpClient for API communication
- Resilience policies (retry, circuit breaker)

**Backend (REST API)**:

- ASP.NET Core Minimal API
- OpenAPI/Swagger documentation
- Repository pattern for data access
- File-based content storage (database-ready design)

**Infrastructure**:

- .NET Aspire for orchestration
- OpenTelemetry + Application Insights
- Azure Container Apps deployment
- Bicep Infrastructure as Code

**Testing Frameworks**:

- xUnit (unit and integration tests)
- bUnit (Blazor component tests)
- Moq (mocking framework)
- Playwright (E2E tests)

**Key Directories**:

- `src/TechHub.Api/` - REST API backend
- `src/TechHub.Web/` - Blazor frontend
- `src/TechHub.Core/` - Domain models and interfaces
- `src/TechHub.Infrastructure/` - Data access implementations
- `src/TechHub.AppHost/` - .NET Aspire orchestration
- `tests/` - All test projects
- `infra/` - Bicep infrastructure
- `scripts/` - PowerShell automation

**.NET-Specific Development Patterns**:

✅ **File-scoped namespaces**: Use in all C# files  
✅ **Nullable reference types**: Enabled in all projects  
✅ **Records for DTOs**: Prefer `record` over `class` for immutable data  
✅ **Minimal APIs**: Use static methods for endpoint handlers  
✅ **Async/await**: All I/O operations must be asynchronous  
✅ **Dependency injection**: Constructor injection for all dependencies  
✅ **Service lifetimes**: Singleton (stateless/cached), Scoped (per-request), Transient (lightweight)  
✅ **Options pattern**: Use `IOptions<T>` for configuration, never direct access  
✅ **Typed HttpClient**: Register with `AddHttpClient<TInterface, TImplementation>`  

**Architecture Decisions**:

✅ **Separate frontend/backend**: TechHub.Web calls TechHub.Api via HttpClient  
✅ **MCP-ready design**: API follows resource-oriented patterns for future MCP support  
✅ **Auth-ready design**: Architecture supports future IdentityServer/Duende integration  
✅ **Multi-location URLs**: Content accessible from multiple section contexts  

**Documentation Resources** (use context7 MCP tool):

When working on .NET features, ALWAYS use the context7 MCP tool to fetch current documentation:

```plaintext
# .NET Runtime and Libraries
mcp_context7_resolve-library-id(libraryName: "dotnet")
mcp_context7_query-docs(context7CompatibleLibraryID: "/dotnet/docs", query: "your topic")

# ASP.NET Core
mcp_context7_resolve-library-id(libraryName: "aspnetcore")
mcp_context7_query-docs(context7CompatibleLibraryID: "/dotnet/aspnetcore", query: "minimal apis")

# Blazor
mcp_context7_query-docs(context7CompatibleLibraryID: "/dotnet/aspnetcore", query: "blazor server-side rendering")

# .NET Aspire
mcp_context7_resolve-library-id(libraryName: "aspire")
mcp_context7_query-docs(context7CompatibleLibraryID: "/dotnet/aspire", query: "service discovery")

# xUnit Testing
mcp_context7_resolve-library-id(libraryName: "xunit")
mcp_context7_query-docs(context7CompatibleLibraryID: "/xunit/xunit", query: "theories and data-driven tests")

# bUnit (Blazor Testing)
mcp_context7_resolve-library-id(libraryName: "bunit")
mcp_context7_query-docs(context7CompatibleLibraryID: "/bunit/bunit", query: "component testing")
```

## .NET Patterns & Examples

### Minimal API Endpoints

Use static methods for endpoint handlers following clean architecture:

```csharp
// Endpoints/SectionEndpoints.cs
namespace TechHub.Api.Endpoints;

public static class SectionEndpoints
{
    public static void MapSectionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/sections")
            .WithTags("Sections")
            .WithOpenApi();
        
        group.MapGet("/", GetAllSections)
            .WithName("GetAllSections")
            .WithSummary("Get all sections");
        
        group.MapGet("/{url}", GetSectionByUrl)
            .WithName("GetSectionByUrl")
            .WithSummary("Get section by URL slug");
    }
    
    private static async Task<IResult> GetAllSections(
        ISectionRepository repository, 
        CancellationToken ct)
    {
        var sections = await repository.GetAllSectionsAsync(ct);
        return Results.Ok(sections);
    }
    
    private static async Task<IResult> GetSectionByUrl(
        string url,
        ISectionRepository repository, 
        CancellationToken ct)
    {
        var section = await repository.GetSectionByUrlAsync(url, ct);
        return section is not null 
            ? Results.Ok(section) 
            : Results.NotFound();
    }
}
```

### Blazor Components with Code-Behind

Separate complex component logic using code-behind pattern:

```razor
@* Components/Pages/SectionIndex.razor *@
@page "/{SectionUrl}"
@inherits SectionIndexBase

<PageTitle>@Section?.Title | Tech Hub</PageTitle>

<HeadContent>
    <meta name="description" content="@Section?.Description" />
    <link rel="canonical" href="https://tech.hub.ms/@SectionUrl" />
</HeadContent>

@if (Section is not null)
{
    <SectionHeader Section="@Section" />
    <SectionNav Section="@Section" ActiveCollection="@null" />
    
    <main id="content" role="main">
        <FilterControls @bind-FilteredItems="FilteredItems" AllItems="@AllItems" />
        <ContentList Items="@FilteredItems" />
    </main>
}
```

```csharp
// Components/Pages/SectionIndex.razor.cs
namespace TechHub.Web.Components.Pages;

public class SectionIndexBase : ComponentBase
{
    [Parameter] public required string SectionUrl { get; set; }
    
    [Inject] protected ITechHubApiClient ApiClient { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    
    protected SectionDto? Section { get; set; }
    protected IReadOnlyList<ContentItemDto> AllItems { get; set; } = [];
    protected IReadOnlyList<ContentItemDto> FilteredItems { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        Section = await ApiClient.GetSectionAsync(SectionUrl);
        if (Section is null)
        {
            Navigation.NavigateTo("/404");
            return;
        }
        
        AllItems = await ApiClient.GetContentAsync(SectionUrl);
        FilteredItems = AllItems;
    }
}
```

### Repository Pattern Implementation

File-based repository with caching:

```csharp
// Infrastructure/Repositories/FileSectionRepository.cs
namespace TechHub.Infrastructure.Repositories;

public class FileSectionRepository : ISectionRepository
{
    private readonly string _sectionsJsonPath;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FileSectionRepository> _logger;
    
    public FileSectionRepository(
        IOptions<ContentOptions> options,
        IMemoryCache cache,
        ILogger<FileSectionRepository> logger)
    {
        _sectionsJsonPath = options.Value.SectionsJsonPath;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<IReadOnlyList<Section>> GetAllSectionsAsync(
        CancellationToken ct = default)
    {
        const string cacheKey = "all_sections";
        
        if (_cache.TryGetValue<IReadOnlyList<Section>>(cacheKey, out var cached))
        {
            return cached!;
        }
        
        var json = await File.ReadAllTextAsync(_sectionsJsonPath, ct);
        var sections = JsonSerializer.Deserialize<List<Section>>(json) 
            ?? throw new InvalidOperationException("Failed to parse sections.json");
        
        _cache.Set(cacheKey, sections, TimeSpan.FromHours(1));
        
        _logger.LogInformation("Loaded {Count} sections from {Path}", 
            sections.Count, _sectionsJsonPath);
        
        return sections;
    }
    
    // ... other methods
}
```

**CRITICAL**: All `IContentRepository` methods **MUST** return content sorted by `DateEpoch` in **descending order** (newest first). This sorting is applied:

- At the repository layer (not in controllers/endpoints)
- To all methods: `GetAllAsync()`, `GetByCollectionAsync()`, `GetByCategoryAsync()`, `SearchAsync()`
- Before caching (cached results are pre-sorted)

**Implementation Example**:

```csharp
public async Task<IReadOnlyList<ContentItem>> GetAllAsync(CancellationToken ct = default)
{
    var items = await LoadItemsFromDisk(ct);
    return items
        .OrderByDescending(x => x.DateEpoch)
        .ToList();
}
```

**Rationale**: Consistent sorting across all endpoints, reduces client-side burden, matches user expectations.

### Dependency Injection Service Lifetimes

**Singleton** - Service has no state or state is shared across all requests:

- `ISectionRepository` (FileSectionRepository with caching)
- `IContentRepository` (FileContentRepository with caching)
- `IMarkdownProcessor` (stateless)
- `IMemoryCache`, `TimeProvider` (built-in)

**Scoped** - Service lifetime matches HTTP request:

- `IRssGenerator` (generates per-request)
- `IStructuredDataService` (generates per-request)
- `ITechHubApiClient` (typed HttpClient)

**Transient** - Lightweight, stateless, new instance each time:

- Rarely needed (most services fit Singleton or Scoped)

**Options Pattern for Configuration**:

```csharp
// Configuration class
public class ContentOptions
{
    public required string SectionsJsonPath { get; init; }
    public required string CollectionsRootPath { get; init; }
    public string Timezone { get; init; } = "Europe/Brussels";
}

// Registration in Program.cs
builder.Services.Configure<ContentOptions>(
    builder.Configuration.GetSection("Content"));

// Injection in service
public class FileSectionRepository : ISectionRepository
{
    private readonly ContentOptions _options;
    
    public FileSectionRepository(IOptions<ContentOptions> options)
    {
        _options = options.Value;
    }
}
```

**Typed HttpClient Pattern**:

```csharp
// Interface
public interface ITechHubApiClient
{
    Task<IReadOnlyList<SectionDto>> GetAllSectionsAsync(CancellationToken ct = default);
}

// Registration with resilience
builder.Services.AddHttpClient<ITechHubApiClient, TechHubApiClient>(client =>
{
    client.BaseAddress = new Uri("https+http://api"); // Aspire service discovery
})
.AddStandardResilienceHandler(); // Retry + Circuit Breaker

// Implementation
public class TechHubApiClient : ITechHubApiClient
{
    private readonly HttpClient _httpClient;
    
    public TechHubApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IReadOnlyList<SectionDto>> GetAllSectionsAsync(
        CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync("/api/sections", ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<SectionDto>>(ct) ?? [];
    }
}
```

**Common DI Pitfalls**:

❌ **WRONG**: Singleton with scoped dependency (e.g., HttpContext)  
❌ **WRONG**: Transient for heavy objects (creates too many instances)  
❌ **WRONG**: Direct configuration access (`builder.Configuration["Key"]`)  

✅ **CORRECT**: Match lifetime to dependency requirements  
✅ **CORRECT**: Singleton for stateless services  
✅ **CORRECT**: Use Options pattern for configuration

### Domain Models with Records

Use records for immutable domain models:

```csharp
// Core/Models/ContentItem.cs
namespace TechHub.Core.Models;

/// <summary>
/// Represents a content item (news, blog, video, etc.)
/// </summary>
public class ContentItem
{
    /// <summary>
    /// URL-friendly slug derived from filename (e.g., "2025-01-15-product-launch")
    /// </summary>
    public required string Slug { get; init; }
    
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? Author { get; init; }
    
    /// <summary>
    /// Publication date as Unix epoch timestamp
    /// </summary>
    public required long DateEpoch { get; init; }
    
    /// <summary>
    /// Primary collection (news, blogs, videos, community, roundups)
    /// </summary>
    public required string CollectionName { get; init; }
    
    /// <summary>
    /// Optional alt-collection for content organized in subfolders
    /// (e.g., "ghc-features" for _videos/ghc-features/)
    /// </summary>
    public string? AltCollection { get; init; }
    
    /// <summary>
    /// All categories this content belongs to (e.g., ["ai", "github-copilot"])
    /// Supports multi-location content access
    /// </summary>
    public required IReadOnlyList<string> Categories { get; init; }
    
    public required IReadOnlyList<string> Tags { get; init; }
    public required string RenderedHtml { get; init; }
    public required string Excerpt { get; init; }
    
    /// <summary>
    /// External link URL (mapped from frontmatter canonical_url field)
    /// </summary>
    public string? ExternalUrl { get; init; }
    
    /// <summary>
    /// YouTube video ID for video content
    /// </summary>
    public string? VideoId { get; init; }
    
    /// <summary>
    /// Viewing mode for content ("internal" or "external", default: "external")
    /// Maps from frontmatter viewing_mode field
    /// </summary>
    public string? ViewingMode { get; init; }
    
    /// <summary>
    /// Generate URL for this content in a specific section context.
    /// Example: /ai/videos/vs-code-107.html
    /// All URLs are lowercase for consistency
    /// </summary>
    public string GetUrlInSection(string sectionUrl)
    {
        var normalizedSection = sectionUrl.StartsWith('/') ? sectionUrl : $"/{sectionUrl}";
        return $"{normalizedSection.ToLowerInvariant()}/{CollectionName.ToLowerInvariant()}/{Slug.ToLowerInvariant()}.html";
    }
}
```

### Markdown Frontmatter Mapping

**Critical**: Understanding how markdown frontmatter maps to domain model properties:

```markdown
---
title: "Example Article Title"
author: "Author Name"
date: 2026-01-02
categories: [ai, github-copilot]
tags: [machine-learning, azure-openai]
canonical_url: "https://example.com/article"  # Maps to ExternalUrl property
viewing_mode: "external"                      # "internal" or "external" (default: "external")
video_id: "dQw4w9WgXcQ"                       # YouTube video ID (not extracted from URLs)
alt_collection: "ghc-features"                # For subfolder organization (_videos/ghc-features/)
---

This is the excerpt that appears in list views.

<!--excerpt_end-->

# Full Article Content

The rest of the markdown content rendered to HTML...
```

**Property Mappings**:

- `title` → `Title`
- `author` → `Author`
- `date` → `DateEpoch` (converted to Unix timestamp)
- `categories` → `Categories` (array)
- `tags` → `Tags` (normalized to lowercase, hyphen-separated)
- `canonical_url` → `ExternalUrl` (original source URL)
- `viewing_mode` → `ViewingMode` ("internal" or "external")
- `video_id` → `VideoId` (YouTube video identifier)
- `alt_collection` → `AltCollection` (subfolder categorization)
- Filename → `Slug` (e.g., `2025-01-15-article.md` → `2025-01-15-article`)
- Content before `<!--excerpt_end-->` → `Excerpt`
- Full markdown → `RenderedHtml` (rendered with Markdig)

### Dependency Injection Configuration

```csharp
// Api/Program.cs - Dependency Registration
var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, health checks, resilience)
builder.AddServiceDefaults();

// Configuration
builder.Services.Configure<ContentOptions>(
    builder.Configuration.GetSection("Content"));

// Infrastructure services
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(TimeProvider.System);

// Repositories (file-based)
builder.Services.AddSingleton<ISectionRepository, FileSectionRepository>();
builder.Services.AddSingleton<IContentRepository, FileContentRepository>();

// Services
builder.Services.AddSingleton<IMarkdownProcessor, MarkdownProcessor>();
builder.Services.AddScoped<IRssGenerator, RssGenerator>();
builder.Services.AddScoped<IStructuredDataService, StructuredDataService>();

// API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// CORS for Blazor frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(builder.Configuration["AllowedOrigins"] ?? "https://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader());
});
```

## Project Overview

The Tech Hub is a technical content hub with configuration-driven section and collection management. Content is organized by sections (Everything, AI, GitHub Copilot, Azure, ML, .NET, DevOps, Security) and collections (news, videos, community, blogs, roundups).

**Core Architecture**:

- **Configuration-driven**: Single source of truth in `_data/sections.json`
- **RESTful API**: Backend provides content via REST endpoints
- **Modern frontend**: Blazor SSR with progressive enhancement
- **Test-driven**: Comprehensive test coverage at all layers
- **Performance-first**: Server-side rendering with client-side enhancements

**.NET Projects** (separate frontend and backend):

- **TechHub.Api** - REST API backend (ASP.NET Core Minimal API)
- **TechHub.Web** - Blazor frontend (SSR + WebAssembly)
- **TechHub.Core** - Domain models and interfaces
- **TechHub.Infrastructure** - Data access implementations
- **TechHub.AppHost** - .NET Aspire orchestration

**Resilience & Reliability**:

- **HTTP Resilience Policies** - Built-in retry (3 attempts with exponential backoff), circuit breaker (50% failure ratio), and timeout (60s)
- **Graceful Error Handling** - User-friendly error messages with functional retry buttons
- **Automatic State Management** - UI automatically updates during loading and retry operations

**Content & Configuration**:

- **Content Collections**: `collections/` directory with markdown files
- **Data Configuration**: `appsettings.json` for site structure (sections and collections)
- **Tests**: `tests/` directory with unit, integration, component, and E2E tests
- **Infrastructure**: `infra/` directory with Bicep templates

See [.NET Migration Status](#net-migration-status) section above for current implementation progress, [README.md](README.md) for quick start guide, and [specs/](specs/) for detailed feature specifications.

## Documentation Architecture

The Tech Hub uses a **multi-tier documentation system** designed to separate generic principles from implementation details. This architecture ensures documentation stability across technology migrations.

### Documentation Hierarchy

**1. Root AGENTS.md** (this file):

- Generic development principles that apply to ANY tech stack
- Timezone handling, performance architecture, configuration-driven design
- Repository structure and organization
- Site terminology and concepts
- **NOT for**: Framework-specific implementation details

**2. Framework-Specific Agents** (`.github/agents/`):

- Complete framework-specific development guidance
- Server management, build processes, testing commands
- **Current**: `dotnet.md` for .NET/Blazor development
- **Will be REPLACED** during technology migrations

**3. Domain-Specific AGENTS.md Files**:

- Development patterns for specific code areas (not frameworks)
- Located in each major directory (`src/`, `scripts/`, `tests/`, etc.)
- **MAY need updates** during migrations but maintain domain focus
- Examples: API patterns, component patterns, scripting patterns

**4. Functional Documentation** (`docs/`):

- Framework-agnostic descriptions of WHAT the system does
- Minimal set - only 3 files: filtering, content management, API spec
- Survive technology changes with minimal updates

**5. Content Guidelines** (`collections/`):

- Writing standards and markdown formatting rules
- Content creation and management workflows

### Documentation Placement Strategy

**CRITICAL**: Understanding where information belongs is essential for maintaining clean, maintainable documentation that remains stable across technology changes.

**Why Framework-Specific Details Are NOT in Root AGENTS.md:**

This is a **permanent architectural principle**, not just a temporary measure. By separating framework-specific implementation from generic principles, we ensure documentation stability across all future technology migrations (currently Jekyll → .NET/Blazor, but applicable to any future changes):

- **Root AGENTS.md** (this file): Generic principles that apply to ANY tech stack (timezone handling, performance architecture, configuration-driven design, terminology)
- **Framework Agents** (`.github/agents/`): Complete framework-specific guidance that will be REPLACED during migration
  - `dotnet.md` (current): .NET, Blazor, C# patterns
- **Domain AGENTS.md**: Domain-specific patterns that MAY need updates but maintain focus
  - `src/AGENTS.md`: .NET development across all projects
  - `src/TechHub.Api/AGENTS.md`: API development patterns
  - `src/TechHub.Web/AGENTS.md`: Blazor component patterns
  - `scripts/AGENTS.md`: PowerShell automation patterns
  - `tests/AGENTS.md`: Testing strategies
- **Functional Docs** (`docs/`): Framework-agnostic descriptions of WHAT the system does
  - Only 3 files: `filtering-system.md`, `content-management.md`, `api-specification.md`

**Complete Placement Hierarchy**: See [docs/AGENTS.md](docs/AGENTS.md) for detailed guidance on where to place new documentation.

**Why This Matters**: This separation ensures that when technology stacks change (Jekyll → .NET/Blazor, or any future migrations), only framework agents need replacement. All other documentation—principles, domain patterns, and functional specs—remain stable and relevant. This architecture makes the codebase resilient to technology evolution while preserving institutional knowledge.

**Framework Mentions in Documentation**: Functional documentation may reference specific implementations (API endpoints, service names, etc.) when essential to understanding the system behavior. These mentions describe WHAT the system does (behavior, contracts, rules), not HOW to build it (code patterns, commands, frameworks). See [docs/AGENTS.md - Implementation Mentions](docs/AGENTS.md#implementation-mentions-in-functional-documentation) for complete guidelines.

### Complete Documentation Map

**Project Overview**:

- **[README.md](README.md)** - Project overview, quick start guide, architecture summary, current implementation status, and navigation to all other documentation

**Custom Agents** (in `.github/agents/`):

- **[dotnet.md](.github/agents/dotnet.md)** - `@dotnet` agent for .NET/Blazor development, C# patterns, ASP.NET Core, Aspire orchestration

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

1. Use `@dotnet` agent for framework specifics
2. Read [src/AGENTS.md](src/AGENTS.md) for general .NET patterns
3. Read specific domain AGENTS.md (API, Web, Core, Infrastructure)

**Working on PowerShell scripts?**

1. Read [scripts/AGENTS.md](scripts/AGENTS.md)
2. See `@dotnet` agent for testing

**Working on content?**

1. Read [collections/AGENTS.md](collections/AGENTS.md)
2. Follow [markdown-guidelines.md](collections/markdown-guidelines.md)
3. Follow [writing-style-guidelines.md](collections/writing-style-guidelines.md)

**Working on tests?**

1. Read [tests/AGENTS.md](tests/AGENTS.md)
2. See `@dotnet` agent for test commands

**Understanding system behavior?**

1. Read [docs/filtering-system.md](docs/filtering-system.md) for filtering
2. Read [docs/content-management.md](docs/content-management.md) for content workflows
3. Read [docs/api-specification.md](docs/api-specification.md) for API contracts

## Core Development Principles

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

- Always derive sections, collections, categories from configuration files
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

- Server-side: See [.github/agents/dotnet.md](.github/agents/dotnet.md) for server-side patterns
- Client-side: See domain-specific AGENTS.md files for client-side patterns

**Benefits**: Prevents date/time bugs, ensures consistent behavior across all systems, simplifies date comparisons.

## Repository Organization

### Core Directories

**Content Directories**:

- **`collections/`** - Content organization with one directory per collection:
  - `_news/` - Official announcements and product updates
  - `_videos/` - Video content and tutorials
    - `_videos/ghc-features/` - GitHub Copilot feature demos (special frontmatter required)
  - `_community/` - Microsoft Tech Community posts and community-sourced content
  - `_blogs/` - Blogs and technical articles
  - `_roundups/` - Curated weekly content summaries

**.NET Code & Projects** (in repository root):

- **`src/`** - .NET source code projects
  - `src/TechHub.Api/` - ASP.NET Core REST API
  - `src/TechHub.Web/` - Blazor frontend
  - `src/TechHub.Core/` - Domain models and interfaces
  - `src/TechHub.Infrastructure/` - Repository implementations
  - `src/TechHub.AppHost/` - .NET Aspire orchestration
- **`tests/`** - .NET test projects
  - `tests/TechHub.Core.Tests/` - Unit tests
  - `tests/TechHub.Api.Tests/` - API integration tests
  - `tests/TechHub.Web.Tests/` - bUnit component tests
  - `tests/TechHub.E2E.Tests/` - Playwright E2E tests

**Development & Build**:

- **`docs/`** - Framework-agnostic functional documentation
- **`.github/agents/`** - Framework-specific development agents
- **`scripts/`** - Automation and utility scripts (PowerShell)
- **`.tmp/`** - Temporary directory for development scripts

**Configuration**:

- **`_data/sections.json`** - Single source of truth for sections and collections (shared)
- **`TechHub.slnx`** - .NET solution file

> **See "Documentation Architecture" section above for complete documentation map and navigation guide**

### Project Structure

The Tech Hub follows a **configuration-driven architecture** where all sections, collections, and content organization is defined in `_data/sections.json`. This single source of truth ensures consistency across all parts of the application.

**Key Architectural Principles**:

- **Configuration-Driven**: All sections and collections defined in data files
- **Dynamic Generation**: Pages created automatically from configuration
- **Modular Design**: New sections added by updating `sections.json`
- **Consistent Structure**: Build system ensures uniform page generation
- **Content Separation**: Clear distinction between generated and custom pages

## Site Terminology

### Core Concepts

**Sections**: Top-level organizational units that group related content by topic or domain.

- **Purpose**: Provide thematic organization (e.g., AI, GitHub Copilot, Azure)
- **Configuration**: Defined in `_data/sections.json` as single source of truth
- **Properties**: Each section includes display title, description, URL path, associated category, background image, and collections
- **Key Features**: Dynamic and configuration-driven - new sections added without code changes, each has own index page and navigation

**Collections**: Content types that represent different formats within sections.

- **Purpose**: Organize content by format and purpose (news, videos, community, blogs, roundups)
- **Configuration**: Defined in framework configuration (e.g., Jekyll's `jekyll/_config.yml`), associated with sections via `_data/sections.json`
- **Technical**: Each collection has its own directory, can be marked as custom (manually created) or auto-generated
- **Properties**: Collections with output enabled generate individual pages for each item

**Items**: Individual pieces of content within collections.

- **Definition**: Actual content users consume (articles, videos, announcements, Blogs)
- **Terminology Note**: "Item" is the preferred term, but "Article" and "Post" are also used in code/documentation to refer to content (note: "Post" in variables does NOT specifically mean blogs from `_blogs/`)
- **Structure**: Markdown files with YAML front matter containing metadata (title, date, author, categories, tags) and content body
- **Processing**: Items are processed by the build system and can be listed on collection pages, filtered by date/tags/categories, displayed on section index pages, and included in RSS feeds

### Content Organization

**Relationship Between Concepts**:

1. **Sections** contain multiple **Collections**
2. **Collections** contain multiple **Items**
3. **Items** have metadata (dates, tags) used by filtering systems
4. Build-time processing prepares all data for client-side consumption
5. Client-side filtering provides interactive content discovery

**Content Types**:

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

The site provides RSS feeds for all sections and collections:

**Section RSS Feeds** (all content with that category):

- Everything: `/feed.xml` - All content across all sections
- AI: `/ai/feed.xml` - AI-related content
- GitHub Copilot: `/github-copilot/feed.xml` - GitHub Copilot content
- ML: `/ml/feed.xml` - Machine learning content
- Azure: `/azure/feed.xml` - Azure cloud platform content
- .NET: `/coding/feed.xml` - .NET and coding content
- DevOps: `/devops/feed.xml` - DevOps and automation content
- Security: `/security/feed.xml` - Security content

**Collection RSS Feeds**:

- Roundups: `/roundups/feed.xml` - Weekly content roundups

**Access**:

- Section pages include RSS link in header area
- Footer "Subscribe via RSS" links to everything feed
