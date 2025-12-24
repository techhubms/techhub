# Tech Hub Development Guide

> **For Jekyll-specific development**: Use the `@jekyll` custom agent or see [.github/agents/jekyll.md](.github/agents/jekyll.md)

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

**Benefits**: Prevents date/time bugs, ensures consistent behavior across all systems, simplifies date comparisons.

### Tool Priority Hierarchy

**CRITICAL**: Always prefer higher-level abstractions over lower-level alternatives.

**Priority Order**:

1. **MCP tools** (highest priority) - Use Model Context Protocol server tools first
2. **Built-in tools** - Use IDE/agent built-in tools if no MCP tool exists  
3. **Command-line tools** (last resort) - Use CLI only when no higher-level tool available

**Rationale**: Higher-level tools provide better error handling, validation, and integration.

## Repository Structure

### Site Architecture Overview

**Main Index** (`index.md`):
- Entry point displaying sections grid, roundups, and latest content
- Uses `home` layout with dynamic section population from configuration

**Section System**:
- Organized into multiple topic areas (AI, GitHub Copilot, Azure, ML, .NET, DevOps, Security)
- Each section contains index page, RSS feed, collections, and optional custom pages
- Configuration-driven via `_data/sections.json` (single source of truth)
- Auto-generated pages via build plugins for consistency

**Data Flow**:
1. Content added to collection directories (`_news/`, `_videos/`, etc.)
2. `_data/sections.json` defines which collections appear in which sections  
3. Build system reads configuration and creates pages
4. Generated pages use `site.data.sections` for navigation and display

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
  - `_events/` - Official events and community meetups
  - `_posts/` - Blog posts and technical articles
  - `_roundups/` - Curated weekly content summaries

**Code & Templates** (framework-specific):
- **`_includes/`** - Reusable template components
- **`_layouts/`** - Page layout templates
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
- **`_config.yml`** - Build system configuration (framework-specific)

### Domain-Specific AGENTS.md Files

**CRITICAL**: Read the domain-specific `AGENTS.md` file for the area you're modifying:

- **`_plugins/AGENTS.md`** - Build plugins and extensions (Ruby for Jekyll)
- **`assets/js/AGENTS.md`** - Client-side JavaScript development
- **`_sass/AGENTS.md`** - Styling and CSS architecture
- **`scripts/AGENTS.md`** - Automation and build scripts (PowerShell)
- **`rss/AGENTS.md`** - Feed management and RSS generation
- **`docs/AGENTS.md`** - Documentation maintenance guidelines
- **`collections/AGENTS.md`** - Content creation and management
- **`spec/AGENTS.md`** - Testing strategies and frameworks

### Documentation Location

- **Functional documentation**: See `docs/` directory for framework-agnostic system documentation
- **Development guidance**: See domain-specific `AGENTS.md` files for code patterns and rules
- **Framework-specific**: See `.github/agents/` for framework development guides

## Development Workflow

### Before Making Changes

### AI Assistant Workflow

**Required Steps**:

1. **Create a step-by-step plan** before making changes
2. **Explain the plan** to validate approach before proceeding
3. **Read domain AGENTS.md files** before modifying code
4. **Follow tool priority hierarchy** (MCP → Built-in → CLI)
5. **Continue working** until the task is complete before ending your turn

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

#### PowerShell Script Requirements

- Always use backticks (`) to escape special characters in PowerShell
- Never use backslashes (\\) for escaping in PowerShell
- Store any temporary files in `/tmp/` directory

#### Temporary File Locations

- **ALWAYS** place temporary scripts in `/tmp/`
- Never create temporary files in repository root or working directories

#### Cleanup Procedures

- Clean up temporary scripts after use
- Remove temporary files when no longer needed

### AI Workflow Principles

Core workflow principles for effective development.

#### Development Workflow Principles

- Always create a step-by-step plan before doing anything and explain it before proceeding
- Always use the latest version of code and libraries
- Always tell the user if code can be optimized extensively, especially for significant performance improvements
- Always ask users for required details if not provided (title, description, author, categories, tags, etc.)
- Always implement or fix everything requested, not just most things
- Always ensure every file is complete and never left broken or unfinished
- Always read documentation, code, and content files to understand implementation before making assumptions
- Always continue working until the task is complete before ending your turn
- Apply edits directly using appropriate tools unless you need to ask clarifying questions

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

- Test frameworks: RSpec (Ruby), Jest (JavaScript), Pester (PowerShell), Playwright (E2E)
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

## Custom Agents

Domain-specific custom agents provide specialized guidance:

- **Jekyll Development**: Use `@jekyll` agent for templating, build process, and server management

## Site Terminology

### Core Content Organization

**Sections**: Top-level organizational units that group related content by topic or domain.

- **Purpose**: Provide thematic organization (e.g., AI, GitHub Copilot, Azure)
- **Configuration**: Defined in `_data/sections.json` as single source of truth
- **Properties**: Each section includes display title, description, URL path, associated category, background image, and collections
- **Key Features**: Dynamic and configuration-driven - new sections added without code changes, each has own index page and navigation

**Collections**: Content types that represent different formats within sections.

- **Purpose**: Organize content by format and purpose (news, videos, community, events, blogs, roundups)
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
- **Videos**: Educational and informational video content
- **Community**: Community-sourced content and discussions
- **Events**: Official events and meetups
- **Blogs**: Blog posts and articles
- **Roundups**: Curated weekly content summaries

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
