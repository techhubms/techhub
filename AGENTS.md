# Tech Hub Documentation Guide

> **AI CONTEXT**: This is the **ROOT** context file. It defines global architecture, principles, and rules. When working in a specific domain (e.g., `scripts/`, `_plugins/`), **ALSO** read the `AGENTS.md` file in that directory.

> **CRITICAL**: For ALL framework-specific development (Jekyll, Ruby, JavaScript, PowerShell, testing), use the **`@fullstack` agent** - see [.github/agents/fullstack.md](.github/agents/fullstack.md)

## Project Knowledge

The Tech Hub is a static technical content hub with configuration-driven section and collection management. All framework-specific implementation details are handled by the `@fullstack` agent.

**Core Directories**:

- `collections/` - Content files (`_news/`, `_videos/`, `_community/`, `_posts/`, `_roundups/`)
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

## Boundaries

### ✅ Always Do

- **Use `@fullstack` agent for development tasks**: Server management, templating, plugins, build system, coding, testing
- **Check for errors after editing files**: Use `get_errors` tool on files you modified to check for VS Code diagnostics (markdown linting, ESLint, RuboCop, etc.) and fix all issues
- **Run tests after modifying code**: CRITICAL - After ANY code changes (Ruby, JavaScript, PowerShell, templates), you MUST run the appropriate test suites. See the AGENTS.md files in each subdirectory for specific testing commands (e.g., `_plugins/AGENTS.md`, `assets/js/AGENTS.md`, `scripts/AGENTS.md`). Documentation-only changes do not require testing.
- **Fix linter issues**: Always resolve all linting errors and warnings, EXCEPT when the issue is in an intentional bad example in documentation (e.g., fenced code blocks without language specifiers showing anti-patterns)
- **Use MCP tools over CLI**: Prefer Playwright MCP, GitHub MCP, context7 MCP
- **Read domain-specific AGENTS.md files**: Before editing any code in that domain
- **Store temp files in `.tmp/`**: ALL temporary or one-time use scripts MUST go in `.tmp/` directory. ONLY place scripts in `scripts/` directory if they are permanent, reusable tools that are part of the project infrastructure (e.g., build scripts, deployment scripts, test runners). If you're creating a script to solve an immediate task or perform a one-time operation, it belongs in `.tmp/`. Never place temporary files in repository root, working directories, or permanent project directories like `scripts/`.
- **Follow timezone standards**: `Europe/Brussels` for all date operations
- **Use configuration-driven design**: Update `_data/sections.json`, not hardcoded values
- **Server-side render all content**: Initial page load must show complete content
- **Add tests for new functionality**: Use according to `spec/AGENTS.md`
- **Follow markdown guidelines**: See `collections/markdown-guidelines.md` and `collections/writing-style-guidelines.md`

### ⚠️ Ask First

- **Website configuration changes**: Consult `@fullstack` agent before modifying build system or configuration
- **Breaking changes to public APIs**: Changes that affect existing functionality
- **Adding new dependencies**: To dependency management files or any config
- **Cross-domain changes**: Modifications affecting multiple areas
- **Significant refactoring**: That touches many files or core architecture
- **Cross-domain changes**: Modifications affecting multiple areas (Ruby + JS + content)
- **Significant refactoring**: That touches many files or core architecture

### 🚫 Never Do

- **Never commit secrets or API keys**: Check all files before committing
- **Never modify generated directories**: `_site/`, `node_modules/`, `.tmp/` (except for temp files)
- **Never use backslashes for escaping in PowerShell**: Always use backticks (`)
- **Never create content without frontmatter**: All markdown must have proper YAML front matter
- **Never hardcode section/collection data**: Always use `_data/sections.json`
- **Never let JavaScript create initial content**: Server-side render everything, JS only enhances
- **Never invent or fabricate information**: Only use verified, accurate content
- **Never add wrapper methods just for tests**: Test real implementation
- **Never make code backwards compatible unless requested**: Prefer clean, modern code
- **Never add comments describing what changed**: Code should be self-documenting
- **Never start responses with "Sure!" or "You're right!"**: Be direct and professional
- **Never use emojis unless explicitly requested**: Keep communication professional
- **Never leave files incomplete or broken**: Ensure all changes are complete
- **Never assume UTC**: Always use configured timezone (`Europe/Brussels`)

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

### Tool Usage

**CRITICAL**: Always prefer higher-level tools (MCP > Built-in > CLI). See complete tool calling strategy in [AI Assistant Guidelines](#ai-assistant-guidelines) section below.

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
  - `_posts/` - Blog posts and technical articles
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
- **`scripts/`** - Automation and utility scripts (PowerShell)
- **`rss/`** - RSS feed generation and management
- **`spec/`** - Testing frameworks and test files
- **`_site/`** - Generated site output (ignored in git)
- **`.tmp/`** - Temporary directory for development scripts

**Configuration**:

- **`_data/sections.json`** - Single source of truth for sections and collections
- Build system configuration file - See [.github/agents/fullstack.md](.github/agents/fullstack.md) for framework-specific details

> **See "Documentation Structure" section above for complete documentation map and navigation guide**

## Development Workflow

### Before Making Changes

### AI Assistant Workflow

**Required Steps**:

1. **Create a step-by-step plan** before making changes
2. **Explain the plan** to validate approach before proceeding
3. **Read domain AGENTS.md files** before modifying code
4. **Follow tool calling strategy** (see [Tool Calling Strategy](#tool-calling-strategy) below: MCP → Built-in → CLI)
5. **After making changes**: Use `get_errors` tool on modified files to check for VS Code diagnostics and fix all issues
6. **Continue working** until the task is complete before ending your turn

## AI Assistant Guidelines

Complete workflow requirements, tool priorities, and communication standards for AI assistants.

### Tool Calling Strategy

Always prefer higher-level tools over lower-level alternatives to ensure maintainable, consistent code.

#### MCP Tools vs Built-in Tools vs CLI

**MCP Tools** (highest priority):

- **Playwright MCP server**: Use to interact with or look at websites for debugging or coding. If you need raw HTML or documentation, use curl or built-in tools
- **GitHub MCP tools**: Use for all GitHub interactions (listing pull requests, workflows, triggering actions). **Never** use `gh` or `fetch`
- **context7 MCP tool**: Use to get documentation for any coding tasks

**Built-in Tools**:

- **`replace_string_in_file`**: Always include 5-10 lines of context before and after target code. Verify edit location matches file structure (proper indentation, braces, etc.)
- **`read_file`**: Preferred over command-line `cat`, `Get-Content`
- **`grep_search`, `file_search`**: Preferred over command-line `grep`, `find`

**Command-Line Tools** (lowest priority):

- Only acceptable for complex multi-step operations requiring many tool calls
- Only when operations not supported by MCP or built-in tools

#### Tool Availability & Fallbacks

**When Tools Are Not Available**:

- Always tell the user if you want to use certain tools but they are not available
- Never work around missing tools with alternative methods
- Especially important for `replace_string_in_file` - if you can't edit directly, tell the user instead of using `cat` or `Out-File`

Tools can be disabled by the user. Be careful to only use currently available tools.

#### GitHub MCP Tools

**URL Handling**: Always dissect GitHub URLs to extract IDs rather than using URLs directly.

**Example**: From `https://github.com/techhubms/techhub/actions/runs/16540183811/job/46780259738`:

- Extract run ID: `16540183811`
- Extract job ID: `46780259738`

**Never pass URLs directly to GitHub MCP tools** - extract parameters first.

### Scripts & Temporary Files

Use scripts only for complex logic that cannot be expressed with tool calls.

#### Scripts as Last Resort

- Only use scripts for genuinely complicated logic
- Simple operations (text replacement, file reading, searches) should NEVER use scripts
- If a script IS required, it MUST be a PowerShell script (`.ps1`)
- Always place scripts in a file in the `.tmp` directory and then execute them
- Do NOT paste scripts into the terminal and do NOT call `pwsh -Command`

#### PowerShell Script Requirements

- Always use backticks (`) to escape special characters in PowerShell
- Never use backslashes (\\) for escaping in PowerShell
- Store any temporary files in `.tmp/` directory (or `/tmp/` directory)

#### Temporary File Locations

- **ALWAYS** place temporary scripts in `.tmp/` directory
- **ALWAYS** store any temporary files you create in the `.tmp/` directory
- Never create temporary files in repository root or working directories

#### Cleanup Procedures

- Clean up temporary scripts after use
- Remove temporary files when no longer needed

### AI Workflow Principles

Core workflow principles for effective development.

#### Development Workflow Principles

- Always create a step-by-step plan before doing anything and explain it before proceeding
- Always use the latest version of code and libraries
- Always use context7 MCP tool to fetch the latest documentation when doing any development work with external libraries or frameworks
- Always tell the user if code can be optimized extensively, especially for significant performance improvements
- Always ask users for required details if not provided (title, description, author, categories, tags, etc.)
- Always implement or fix everything requested, not just most things
- Always ensure every file is complete and never left broken or unfinished
- Always read documentation, code, and content files to understand implementation before making assumptions
- Always add tests when creating or changing functionality
- Always add documentation whenever you make code changes that change existing or introduce new behavior
- Always follow [`collections/markdown-guidelines.md`](collections/markdown-guidelines.md) and [`collections/writing-style-guidelines.md`](collections/writing-style-guidelines.md) when creating or editing any markdown files
- Always continue working until the task is complete before ending your turn
- Apply edits directly using appropriate tools unless you need to ask clarifying questions

**DevContainer Dependencies**:

- Always install any dependencies needed by this workspace in `/workspaces/techhub/.devcontainer/post-create.sh`
- Never install dependencies inside PowerShell or other scripts
- Always check `/workspaces/techhub/.github/workflows` when updating `/workspaces/techhub/.devcontainer/post-create.sh` to apply similar updates in workflows

#### Creating Plans Before Action

Before making changes:

1. Create a comprehensive step-by-step plan
2. Explain the plan to the user
3. Wait for confirmation if the changes are significant
4. Proceed systematically through the plan

#### Task Tracking & Todo Lists

For complex multi-step work, use the `manage_todo_list` tool extensively:

- Break complex work into logical, actionable steps
- Mark tasks as in-progress when you begin
- Mark tasks as completed immediately after finishing each one
- Do not batch completions

**Skip task tracking for simple, single-step operations.**

#### Continuing Until Complete

- Never stop working until the task is complete
- Don't hand back to the user when you encounter uncertainty
- Research or deduce the most reasonable approach and continue
- Only terminate your turn when you are certain the task is complete

### Communication Style

Maintain clarity and directness in all responses.

#### Clarity & Directness

- **Down-to-earth and authentic**: Avoid exaggerated language
- **Professional yet approachable**: Clear and authoritative without being overly formal
- **Direct and honest**: Get to the point without embellishment
- **Concise and actionable**: Every sentence should serve a purpose

#### Response Length Guidelines

- **For straightforward queries**: Keep answers brief (typically a few lines excluding code or tool invocations)
- **For complex work**: Expand detail as appropriate
- **Optimize for conciseness**: Maintain helpfulness and accuracy
- **Target 1-3 sentences for simple answers** when possible

**Examples**:

- User: "what's the square root of 144?" → Assistant: "12"
- User: "which directory has the server code?" → Assistant: [searches] "backend/"
- User: "what files are in src/utils/?" → Assistant: [lists] "helpers.ts, validators.ts, constants.ts"

#### What to Avoid in Communication

- Never start responses with "Sure!" or "You're right!"
- Never say the name of a tool to a user (say "I'll run the command in a terminal" not "I'll use run_in_terminal")
- Never use emojis unless explicitly requested
- Never provide information that was not explicitly requested
- Never add generic or filler text
- Never repeat instructions unnecessarily

#### Output Formatting Standards

When executing non-trivial commands, explain their purpose and impact so users understand what's happening, particularly for system-modifying operations.

### Output Formatting

Standards for formatting output in markdown and code.

#### Markdown Formatting Rules

Use proper Markdown formatting:

- Wrap symbol names in backticks: `MyClass`, `handleClick()`
- Use KaTeX for math equations (inline: `$equation$`, block: `$$equation$$`)
- Follow file linkification rules (see below)

#### File Link Formatting

**CRITICAL**: When mentioning files or line numbers, convert to markdown links using workspace-relative paths.

**NO BACKTICKS ANYWHERE**:

- Never wrap file names, paths, or links in backticks
- Never use inline-code formatting for file references

**Required Formats**:

- File: `[path/file.ts](path/file.ts)`
- Line: `[file.ts](file.ts#L10)`
- Range: `[file.ts](file.ts#L10-L12)`

**Path Rules**:

- Without line numbers: Display text must match target path
- With line numbers: Display text can be path or descriptive text
- Use `/` only; strip drive letters and external folders
- Do not use `file://` or `vscode://` URI schemes
- Encode spaces only in target: `My File.md` → `My%20File.md`
- Non-contiguous lines require separate links (never use comma-separated)

**Valid formats**: `[file.ts](file.ts#L10)` or `[file.ts#L10]`
**Invalid**: `([file.ts#L10])` or `[file.ts](file.ts)#L10`

**Examples**:

- With path as display: The handler is in [src/handler.ts](src/handler.ts#L10).
- With descriptive text: The [widget initialization](src/widget.ts#L321) runs on startup.
- File only: See [src/config.ts](src/config.ts) for settings.

**Forbidden (NEVER OUTPUT)**:

- Inline code: `file.ts`, `src/file.ts`, `L86`
- Plain text file names without links
- References without links when mentioning specific locations
- Specific line citations without links ("Line 86", "at line 86")
- Multiple line references in one link: `[file.ts#L10-L12, L20]`

### Code Quality Standards

Quality requirements across all code.

#### General Principles

- Test real implementation, never duplicate logic in tests
- Add tests when creating or changing functionality
- Follow existing patterns and conventions in each language
- Use meaningful variable names and proper error handling
- Prefer clarity over cleverness
- Make parameters required unless they can be null; prefer a failing build over unnecessary optional parameters

#### Testing Requirements

- Use appropriate testing frameworks for each language (see [spec/AGENTS.md](spec/AGENTS.md) for current frameworks)
- Run tests before committing changes
- See [spec/AGENTS.md](spec/AGENTS.md) for comprehensive testing strategy
- Add tests when creating or changing functionality

#### Never Do

- Never invent or fabricate information
- Never use unverifiable content
- Never leave documentation out of sync with code
- Never add wrapper methods in production code just for tests
- Never make code backwards compatible unless explicitly requested
- Never add comments describing what you changed
- Never start responses with "Sure!" or "You're right!"
- Never prevent code from failing if something goes wrong, unless explicitly instructed to add fallbacks or default values
- Never provide information that was not explicitly requested
- Never make changes unless specifically requested
- Never add comments unless they provide clear, actionable information
- Never add generic or filler text
- Never repeat instructions unnecessarily
- Never leave comments that describe what changed (e.g., 'All filtering is now tag-based')

## Custom Agents

Domain-specific custom agents provide specialized guidance:

- **Full-Stack Development**: Use `@fullstack` agent for ALL Jekyll, Liquid templating, server management, plugins, build system, testing, and framework-specific development - see [.github/agents/fullstack.md](.github/agents/fullstack.md)

## Site Terminology

### Core Content Organization

**Sections**: Top-level organizational units that group related content by topic or domain.

- **Purpose**: Provide thematic organization (e.g., AI, GitHub Copilot, Azure)
- **Configuration**: Defined in `_data/sections.json` as single source of truth
- **Properties**: Each section includes display title, description, URL path, associated category, background image, and collections
- **Key Features**: Dynamic and configuration-driven - new sections added without code changes, each has own index page and navigation

**Collections**: Content types that represent different formats within sections.

- **Purpose**: Organize content by format and purpose (news, videos, community, blogs, roundups)
- **Configuration**: Defined in build system configuration, associated with sections via `_data/sections.json`
- **Technical**: Each collection has its own directory, can be marked as custom (manually created) or auto-generated
- **Properties**: Collections with output enabled generate individual pages for each item
- **Configuration**: Defined in framework configuration (e.g., Jekyll's `_config.yml`), associated with sections via `_data/sections.json`
- **Technical**: Each collection has its own directory, can be marked as custom (manually created) or auto-generated
- **Properties**: Collections with output enabled generate individual pages for each item

**Items**: Individual pieces of content within collections.

- **Definition**: Actual content users consume (articles, videos, announcements, blog posts)
- **Terminology Note**: "Item" is the preferred term, but "Article" and "Post" are also used in code/documentation to refer to content (note: "Post" in variables does NOT specifically mean blogs from `_posts/`)
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
- **Blogs**: Blog posts and articles
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
- AI: `/ai.xml` - AI-related content
- GitHub Copilot: `/github-copilot.xml` - GitHub Copilot content
- ML: `/ml.xml` - Machine learning content
- Azure: `/azure.xml` - Azure cloud platform content
- Coding: `/coding.xml` - .NET and coding content
- DevOps: `/devops.xml` - DevOps and automation content
- Security: `/security.xml` - Security content

**Collection RSS Feeds**:

- Roundups: `/roundups.xml` - Weekly content roundups

**Access**:

- Section pages include RSS link in header area
- Footer "Subscribe via RSS" links to everything feed
