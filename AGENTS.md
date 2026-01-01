# Tech Hub Documentation Guide

**AI CONTEXT**: This is the **ROOT** context file. It defines global architecture, principles, and rules. When working in a specific domain (e.g., `scripts/`, `_plugins/`), **ALSO** read the `AGENTS.md` file in that directory.

## Index

- [AI Assistant Workflow](#ai-assistant-workflow)
  - [0. Core Rules & Boundaries](#0-core-rules--boundaries)
  - [1. Gather Context](#1-gather-context)
  - [2. Create a Plan](#2-create-a-plan)
  - [3. Research & Validate](#3-research--validate)
  - [4. Verify Behavior (Optional)](#4-verify-behavior-optional)
  - [5. Implement Changes](#5-implement-changes)
  - [6. Test & Validate](#6-test--validate)
  - [7. Update Documentation](#7-update-documentation)
  - [8. Report Completion](#8-report-completion)
- [Project Knowledge](#project-knowledge)
- [Documentation Structure](#documentation-structure)
  - [Complete Documentation Map](#complete-documentation-map)
  - [Quick Reference Guide](#quick-reference-guide)
- [Core Development Principles](#core-development-principles)
  - [Performance Architecture](#performance-architecture)
  - [Accessibility Standards](#accessibility-standards)
  - [Configuration-Driven Development](#configuration-driven-development)
  - [Timezone & Date Handling](#timezone--date-handling)
- [Repository Structure](#repository-structure)
  - [Site Architecture Overview](#site-architecture-overview)
  - [Core Directories](#core-directories)
- [Site Terminology](#site-terminology)

## AI Assistant Workflow

**🚨 ABSOLUTELY CRITICAL**: This section defines the **required step-by-step process** for all development tasks. Follow these steps in order for every request.

### 0. Core Rules & Boundaries

These are the **non-negotiable rules** that apply to ALL development tasks. ALWAYS follow these rules without exception.

#### ✅ Always Do

- **ALWAYS follow the 8-step workflow**: Complete all steps in order for every request
- **Prefer higher-level tools**: ALWAYS use MCP tools > Built-in tools > CLI commands
  - **MCP tools** (highest priority): Playwright MCP (web testing), GitHub MCP (GitHub operations), context7 MCP (documentation)
  - **Built-in tools**: `replace_string_in_file` (with 5-10 lines context), `read_file`, `grep_search`, `file_search`
  - **CLI** (lowest priority): Only for complex multi-step operations not supported by tools
- **Use `@fullstack` agent for development tasks**: Server management, templating, plugins, build system, coding, testing. See [.github/agents/fullstack.md](.github/agents/fullstack.md)
- **Check for errors after editing files**: Use `get_errors` tool on modified files to check VS Code diagnostics (markdown linting, ESLint, RuboCop, etc.) and fix all issues
- **Run tests after modifying code**: CRITICAL - After ANY code changes (Ruby, JavaScript, PowerShell, templates), run appropriate test suites. Documentation-only changes do not require testing
- **Fix linter issues**: Always resolve all linting errors and warnings, EXCEPT intentional bad examples in documentation
- **Read domain-specific AGENTS.md files**: Before editing any code in that domain
- **Store temp files in `.tmp/`**: ALL temporary/one-off scripts in `.tmp/` (e.g., quick tests, debugging scripts), permanent/reusable automation goes in `scripts/`
- **Use PowerShell for scripts**: If script is required, it MUST be `.ps1` file in `.tmp/` directory, then execute it
- **Follow timezone standards**: `Europe/Brussels` for all date operations
- **Use configuration-driven design**: Update `_data/sections.json`, not hardcoded values
- **Server-side render all content**: Initial page load must show complete content
- **Add tests for new functionality**: According to `spec/AGENTS.md`
- **Follow markdown guidelines**: See `collections/markdown-guidelines.md` and `collections/writing-style-guidelines.md`
- **ALWAYS be direct and concise**: Avoid exaggerated language
- **ALWAYS maintain professional yet approachable tone**: Clear and authoritative without being overly formal
- **ALWAYS avoid filler phrases**: Don't use "Sure!" or "You're right!"

#### ⚠️ Ask First

- **Website configuration changes**: Consult `@fullstack` agent before modifying build system or configuration (e.g., `_config.yml`, `package.json`, `Gemfile`)
- **Breaking changes to public APIs**: Changes that affect existing functionality (e.g., modifying filter signatures, changing data structure)
- **Adding new dependencies**: To dependency management files or any config (e.g., new npm packages, Ruby gems, PowerShell modules)
- **Cross-domain changes**: Modifications affecting multiple areas (e.g., Ruby plugins + JavaScript + Liquid templates, or content structure + build system)
- **Significant refactoring**: That touches many files or core architecture (e.g., restructuring `_data/sections.json`, changing collection types)

#### 🚫 Never Do

- **Never skip the 8-step workflow**: All steps are required for quality work
- **Never use lower-level tools unnecessarily**: Don't use CLI when MCP or built-in tools are available
- **Never dissect GitHub URLs**: Always extract IDs from URLs before passing to GitHub MCP tools
- **Never paste scripts into terminal**: Always save as `.ps1` file in `.tmp/` and execute
- **Never use `pwsh -Command`**: Save script to file and execute it
- **Never use backslashes for escaping in PowerShell**: Always use backticks (`)
- **Never work around missing tools**: Tell user if needed tools are unavailable instead of using alternatives
- **Never commit secrets or API keys**: Check all files before committing
- **Never modify generated directories**: `_site/`, `node_modules/`, `.tmp/` (except for temp files)
- **Never create content without frontmatter**: All markdown must have proper YAML front matter
- **Never hardcode section/collection data**: Always use `_data/sections.json`
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

### 1. Gather Context

**Before touching any code**, understand what you're working on:

**Read Documentation First**:

- Start with this file (Root AGENTS.md) for architecture and principles
- See [Complete Documentation Map](#complete-documentation-map) for navigation guide to all documentation files
- Read domain-specific AGENTS.md file for the area you'll modify (e.g., [_plugins/AGENTS.md](_plugins/AGENTS.md), [assets/js/AGENTS.md](assets/js/AGENTS.md), [scripts/AGENTS.md](scripts/AGENTS.md))
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
- Follow existing code patterns and conventions
- Read documentation, code, and content files to understand implementation
- Use the latest version of code and libraries
- Use context7 MCP tool to fetch latest documentation when working with external libraries or frameworks

### 2. Create a Plan

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

### 3. Research & Validate

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

### 4. Verify Behavior (Optional)

**If needed**, understand current behavior BEFORE making changes:

**Use Playwright MCP Server**:

- Reproduce bugs by interacting with live site
- Verify expected behavior on <https://tech.hub.ms>
- Understand how features currently work
- Capture screenshots or console output for debugging

**When to Verify**:

- Reproducing user-reported bugs
- Understanding complex UI interactions
- Investigating unexpected behavior
- Documenting current state before modifications

**Key Rules**:

- Use Playwright MCP for browser testing and validation
- Test on live site when appropriate
- Document unexpected behaviors you discover
- Report any discrepancies between observed behavior and documentation

### 5. Implement Changes

**Deep dive into the code** and make necessary modifications:

**Implementation Guidelines**:

- Follow tool calling strategy: MCP → Built-in → CLI
- Use `replace_string_in_file` or `multi_replace_string_in_file` for code edits
- Use `get_errors` on modified files to check for VS Code diagnostics
- Fix all linting and compilation errors immediately
- Follow existing patterns and conventions in each language
- Use meaningful variable names and proper error handling
- Prefer clarity over cleverness
- Make parameters required unless they can be null
- Test real implementation, never duplicate logic in tests

**Critical Requirements**:

- **Ask for clarification** on ANYTHING unclear - NEVER assume
- **For bigger changes**: ALWAYS ask before proceeding
- **Check for errors** after every file modification
- **Fix all issues** before moving to next step
- **DevContainer Dependencies**: Always install in `/workspaces/techhub/.devcontainer/post-create.sh`, never in PowerShell or other scripts

**Files to Update**:

- Code files (Ruby, JavaScript, PowerShell, templates, etc.)
- Configuration files if needed
- Any related includes, layouts, or components

**Key Rules**:

- Follow existing patterns and conventions
- See [Core Rules & Boundaries](#0-core-rules--boundaries) for complete rule list

### 6. Test & Validate

**Ensure your changes work correctly**:

**Start Jekyll (If Needed)**:

- Use `./scripts/jekyll-start.ps1` to start server
- Wait for "✅ Jekyll server is ready and accessible!" message
- Monitor rebuild progress with `tail -n 100 ./.tmp/jekyll-log.txt`
- Wait for "...done in X seconds" in log file before testing

**Run Automated Tests**:

- **CRITICAL**: Run tests after ANY code changes (Ruby, JavaScript, PowerShell, templates)
- **Test-first workflow**: When fixing bugs, write a regression test FIRST that reproduces the issue
- Use test scripts: `run-all-tests.ps1`, `run-plugin-tests.ps1`, `run-javascript-tests.ps1`, `run-powershell-tests.ps1`, `run-e2e-tests.ps1`
- Fix any regression issues immediately
- See [spec/AGENTS.md](spec/AGENTS.md) for comprehensive testing strategy

**Extend Tests**:

- **Add new tests** for new functionality BEFORE or DURING implementation
- **Update existing tests** if behavior changed
- Ensure edge cases are covered
- Test real implementation, not mocks
- Use appropriate testing frameworks for each language (see [spec/AGENTS.md](spec/AGENTS.md))
- Never add wrapper methods in production code just for tests

**Test Command Examples**:

```powershell
# Run all tests (all layers in sequence)
./scripts/run-all-tests.ps1

# Run specific test suites
./scripts/run-powershell-tests.ps1
./scripts/run-javascript-tests.ps1
./scripts/run-plugin-tests.ps1
./scripts/run-e2e-tests.ps1

# Run specific test file (layer-specific parameters)
./scripts/run-powershell-tests.ps1 -TestFile "spec/powershell/Get-MarkdownFiles.Tests.ps1"
./scripts/run-javascript-tests.ps1 -TestFile "spec/javascript/sections.test.js"
./scripts/run-plugin-tests.ps1 -SpecFile "spec/plugins/date_filters_spec.rb"
./scripts/run-e2e-tests.ps1 -TestFile "spec/e2e/tests/filtering-core.spec.js"

# Run with additional options
./scripts/run-powershell-tests.ps1 -Coverage
./scripts/run-javascript-tests.ps1 -Watch
./scripts/run-plugin-tests.ps1 -Documentation
./scripts/run-e2e-tests.ps1 -UI -Debug
```

**Key Rules**:

- Documentation-only changes do not require testing
- All code changes MUST be tested
- Write regression tests for bugs BEFORE fixing them
- Fix all test failures before proceeding
- Never skip tests to save time

### 7. Update Documentation

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
- See [Documentation Structure](#documentation-structure) for complete doc map

**Key Rules**:

- This is NOT optional - always update docs
- Check for linting errors in markdown files
- Fix any MD032 or other markdown issues
- Documentation is part of "task complete"

### 8. Report Completion

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

## Project Knowledge

The Tech Hub is a static technical content hub with configuration-driven section and collection management. All framework-specific implementation details are handled by the `@fullstack` agent.

**Core Directories**:

- `collections/` - Content files (`_news/`, `_videos/`, `_community/`, `_blogs/`, `_roundups/`)
- `_data/sections.json` - Single source of truth for site structure
- `docs/` - Framework-agnostic functional documentation
- `.github/agents/` - Framework-specific development agents
- Domain-specific directories - See Documentation Structure below for complete map

## Documentation Structure

### Overview

The Tech Hub uses a three-tier documentation system:

1. **This file (AGENTS.md)**: High-level principles, architecture, and navigation
2. **Domain-Specific AGENTS.md**: Development patterns for specific code areas
3. **Custom Agents**: Framework-specific implementation guides
4. **Functional Docs**: What the system does (not how to code it)

### Documentation Placement Strategy

**CRITICAL**: Understanding where information belongs is essential for maintaining clean, maintainable documentation that remains stable across technology changes.

**Why Framework-Specific Details Are NOT in Root AGENTS.md:**

This is a **permanent architectural principle**, not just a temporary measure. By separating framework-specific implementation from generic principles, we ensure documentation stability across all future technology migrations (currently Jekyll → .NET/Blazor, but applicable to any future changes):

- **Root AGENTS.md** (this file): Generic principles that apply to ANY tech stack (timezone handling, performance architecture, configuration-driven design, terminology)
- **Framework Agents** (`.github/agents/`): Complete framework-specific guidance that will be REPLACED during migration
  - `fullstack.md` (current): Jekyll, Ruby, Liquid, testing commands
  - `dotnet.md` (future): .NET, Blazor, C# patterns
- **Domain AGENTS.md**: Domain-specific patterns that MAY need updates but maintain focus
  - `assets/js/AGENTS.md`: JavaScript patterns (likely unchanged)
  - `_plugins/AGENTS.md`: Build plugins (Ruby → C#, same purpose)
  - `_sass/AGENTS.md`: Styling (SCSS → CSS-in-JS, same purpose)
- **Functional Docs** (`docs/`): Framework-agnostic descriptions of WHAT the system does
  - Only 2 files: `filtering-system.md`, `content-management.md`

**Complete Placement Hierarchy**: See [docs/AGENTS.md](docs/AGENTS.md) for detailed guidance on where to place new documentation.

**Why This Matters**: This separation ensures that when technology stacks change (Jekyll → .NET/Blazor, or any future migrations), only framework agents need replacement. All other documentation—principles, domain patterns, and functional specs—remain stable and relevant. This architecture makes the codebase resilient to technology evolution while preserving institutional knowledge.

**Framework Mentions in Documentation**: Functional documentation may reference specific frameworks (Jekyll, .NET, etc.) when contextually relevant to the system behavior being described. These mentions serve as indicators for areas that may need review during migrations. For example, "Restart Jekyll after tag changes" is appropriate in filtering documentation, while "Jekyll uses Liquid templating" is generic framework information that belongs in framework agents. See [docs/AGENTS.md - Framework Mentions](docs/AGENTS.md#framework-mentions-in-functional-documentation) for complete guidelines.

### Complete Documentation Map

**Custom Agents** (in `.github/agents/`):

- **[fullstack.md](.github/agents/fullstack.md)** - `@fullstack` agent for Jekyll, Liquid, Ruby plugins, JavaScript, PowerShell, testing, server management

**Domain-Specific AGENTS.md Files**:

- **[_plugins/AGENTS.md](_plugins/AGENTS.md)** - Jekyll plugins and extensions
- **[_includes/AGENTS.md](_includes/AGENTS.md)** - Liquid templates and includes
- **[assets/js/AGENTS.md](assets/js/AGENTS.md)** - JavaScript client-side development patterns
- **[_sass/AGENTS.md](_sass/AGENTS.md)** - SCSS styling and CSS architecture
- **[scripts/AGENTS.md](scripts/AGENTS.md)** - PowerShell automation scripts
- **[rss/AGENTS.md](rss/AGENTS.md)** - RSS feed generation
- **[docs/AGENTS.md](docs/AGENTS.md)** - Documentation maintenance guidelines
- **[collections/AGENTS.md](collections/AGENTS.md)** - Content creation and management
- **[spec/AGENTS.md](spec/AGENTS.md)** - Testing strategies across all frameworks

**Functional Documentation** (in `docs/`):

- **[filtering-system.md](docs/filtering-system.md)** - How tag and date filtering works
- **[content-management.md](docs/content-management.md)** - Content workflows and RSS processing

**Content Guidelines** (in `collections/`):

- **[markdown-guidelines.md](collections/markdown-guidelines.md)** - Markdown formatting rules
- **[writing-style-guidelines.md](collections/writing-style-guidelines.md)** - Writing tone and style

### When to Use Each Documentation Type

**Use This File (Root AGENTS.md)** for:

- High-level architecture and principles
- Performance and timezone standards
- Repository structure overview
- Cross-cutting concerns
- Navigation to other docs

**Use `@fullstack` Agent** for:

- Server management and deployment
- Local development and testing
- Templating and view patterns
- Jekyll plugin development
- JavaScript implementation
- PowerShell scripting
- Testing commands and strategies
- Framework-specific how-tos

**Use Domain-Specific AGENTS.md** for:

- Code patterns in specific areas
- Before editing files in that domain
- Language-specific best practices
- Domain-specific rules and examples

**Use Functional Docs** for:

- Understanding system behavior
- Feature specifications
- Business logic documentation

### Quick Reference Guide

**Working on JavaScript?**

1. Read [assets/js/AGENTS.md](assets/js/AGENTS.md)
2. See `@fullstack` agent for testing

**Working on Jekyll plugins?**

1. Read [_plugins/AGENTS.md](_plugins/AGENTS.md)
2. See `@fullstack` agent for framework specifics

**Working on PowerShell scripts?**

1. Read [scripts/AGENTS.md](scripts/AGENTS.md)
2. See `@fullstack` agent for testing

**Working on content?**

1. Read [collections/AGENTS.md](collections/AGENTS.md)
2. Follow [markdown-guidelines.md](collections/markdown-guidelines.md)
3. Follow [writing-style-guidelines.md](collections/writing-style-guidelines.md)

**Working on templates?**

1. Use `@fullstack` agent
2. Read [_plugins/AGENTS.md](_plugins/AGENTS.md) for plugin patterns

**Working on includes/layouts?**

1. Read [_includes/AGENTS.md](_includes/AGENTS.md)
2. Use `@fullstack` agent for framework specifics

**Working on styles?**

1. Read [_sass/AGENTS.md](_sass/AGENTS.md)

**Working on tests?**

1. Read [spec/AGENTS.md](spec/AGENTS.md)
2. See `@fullstack` agent for test commands

**Understanding system behavior?**

1. Read [docs/filtering-system.md](docs/filtering-system.md) for filtering
2. Read [docs/content-management.md](docs/content-management.md) for content workflows

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

- Server-side: See [.github/agents/fullstack.md](.github/agents/fullstack.md) for server-side patterns
- Jekyll plugins: See [_plugins/AGENTS.md](_plugins/AGENTS.md) for plugin-specific handling
- JavaScript: See [assets/js/AGENTS.md](assets/js/AGENTS.md) for client-side patterns

**Benefits**: Prevents date/time bugs, ensures consistent behavior across all systems, simplifies date comparisons.

## Repository Structure

### Site Architecture Overview

**Main Index** (`index.md`):

- Entry point displaying sections grid, roundups, and latest content
- Uses `home` layout with dynamic section population from configuration

**Section System**:

- Organized into multiple topic areas (AI, GitHub Copilot, Azure, ML, .NET, DevOps, Security)
- Each section contains index page, RSS feed, collections, and optional custom pages
- Configuration-driven via `_data/sections.json` (single source of truth)
- Auto-generated pages via Jekyll plugins for consistency

> **See [_includes/AGENTS.md](_includes/AGENTS.md) for detailed section subnavigation behavior documentation**

**Data Flow**:

1. Content added to collection directories (`_news/`, `_videos/`, etc.)
2. `_data/sections.json` defines which collections appear in which sections  
3. Build system reads configuration and creates pages
4. Generated pages use configuration data for navigation and display

**Key Architectural Principles**:

- **Configuration-Driven**: All sections and collections defined in data files
- **Dynamic Generation**: Pages created automatically from configuration
- **Modular Design**: New sections added by updating `sections.json`
- **Consistent Structure**: Build system ensures uniform page generation
- **Content Separation**: Clear distinction between generated and custom pages

### Core Directories

**Content Directories**:

- **`collections/`** - Content organization with one directory per collection:
  - `_news/` - Official announcements and product updates
  - `_videos/` - Video content and tutorials
    - `_videos/ghc-features/` - GitHub Copilot feature demos (special frontmatter required)
  - `_community/` - Microsoft Tech Community posts and community-sourced content
  - `_blogs/` - Blogs and technical articles
  - `_roundups/` - Curated weekly content summaries

**Code & Templates** (framework-specific - see [.github/agents/fullstack.md](.github/agents/fullstack.md) for details):

- **`_includes/`** - Reusable template components (see [_includes/AGENTS.md](_includes/AGENTS.md))
- **`_layouts/`** - Page layout templates (see [_includes/AGENTS.md](_includes/AGENTS.md))
- **`_plugins/`** - Build system plugins and extensions
- **`_sass/`** - Stylesheet organization
- **`assets/`** - Static assets (images, CSS, JS)
  - `assets/js/` - Client-side JavaScript code
  - `assets/section-backgrounds/` - Section header images

**Section Directories** (custom pages):

- **`ai/`** - AI section-specific files (e.g., `ai-to-z.md`)
- **`github-copilot/`** - GitHub Copilot section files (e.g., `features.md`, `levels-of-enlightenment.md`)
- Other sections auto-generated by build plugins

**Development & Build**:

- **`docs/`** - Framework-agnostic functional documentation
- **`.github/agents/`** - Framework-specific development agents
- **`.devcontainer/`** - Dev container configuration and setup scripts
- **`scripts/`** - Automation and utility scripts (PowerShell)
- **`rss/`** - RSS feed generation and management
- **`spec/`** - Testing frameworks and test files
- **`_site/`** - Generated site output (ignored in git)
- **`.tmp/`** - Temporary directory for development scripts

**Configuration**:

- **`_data/sections.json`** - Single source of truth for sections and collections
- Build system configuration file - See [.github/agents/fullstack.md](.github/agents/fullstack.md) for framework-specific details

> **See "Documentation Structure" section above for complete documentation map and navigation guide**

## Site Terminology

### Core Content Organization

**Sections**: Top-level organizational units that group related content by topic or domain.

- **Purpose**: Provide thematic organization (e.g., AI, GitHub Copilot, Azure)
- **Configuration**: Defined in `_data/sections.json` as single source of truth
- **Properties**: Each section includes display title, description, URL path, associated category, background image, and collections
- **Key Features**: Dynamic and configuration-driven - new sections added without code changes, each has own index page and navigation

**Collections**: Content types that represent different formats within sections.

- **Purpose**: Organize content by format and purpose (news, videos, community, blogs, roundups)
- **Configuration**: Defined in framework configuration (e.g., Jekyll's `_config.yml`), associated with sections via `_data/sections.json`
- **Technical**: Each collection has its own directory, can be marked as custom (manually created) or auto-generated
- **Properties**: Collections with output enabled generate individual pages for each item

**Items**: Individual pieces of content within collections.

- **Definition**: Actual content users consume (articles, videos, announcements, Blogs)
- **Terminology Note**: "Item" is the preferred term, but "Article" and "Post" are also used in code/documentation to refer to content (note: "Post" in variables does NOT specifically mean blogs from `_blogs/`)
- **Structure**: Markdown files with YAML front matter containing metadata (title, date, author, categories, tags) and content body
- **Processing**: Items are processed by the build system and can be listed on collection pages, filtered by date/tags/categories, displayed on section index pages, and included in RSS feeds

### Relationship Between Concepts

1. **Sections** contain multiple **Collections**
2. **Collections** contain multiple **Items**
3. **Items** have metadata (dates, tags) used by filtering systems
4. Build-time processing prepares all data for client-side consumption
5. Client-side filtering provides interactive content discovery

### Content Types

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
