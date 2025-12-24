# Tech Hub Development Agent

## Overview

Tech Hub is a Jekyll-based static site for Microsoft technology content, featuring automated RSS feed processing, AI-powered content generation, and a sophisticated tag-based filtering system.

## Tech Stack

- **Static Site Generator**: Jekyll 4.3+
- **Template Engine**: Liquid
- **Styling**: SCSS (Sass)
- **Client-Side**: Vanilla JavaScript (ES6+)
- **Backend Processing**: PowerShell 7+
- **Testing**:
  - Ruby: RSpec
  - JavaScript: Jest with jsdom
  - PowerShell: Pester v5
  - E2E: Playwright
- **AI Integration**: Azure OpenAI
- **CI/CD**: GitHub Actions

## Repository Structure

```text
/
├── _plugins/          # Jekyll Ruby plugins (see _plugins/AGENTS.md)
├── assets/js/         # Client-side JavaScript (see assets/js/AGENTS.md)
├── _sass/             # SCSS styling (see _sass/AGENTS.md)
├── scripts/           # PowerShell automation (see scripts/AGENTS.md)
├── rss/               # RSS feed XML files (see rss/AGENTS.md)
├── docs/              # Documentation (see docs/AGENTS.md)
├── spec/              # All tests (see spec/AGENTS.md)
├── _data/             # Jekyll data files and RSS cache
├── _includes/         # Liquid template partials
├── _layouts/          # Liquid page layouts
└── _*/                # Content collections (_posts, _news, _videos, etc.)
```

## Critical Development Rules

### CRITICAL: Mandatory Reading by Domain

**Before making ANY changes, read the domain-specific AGENTS.md file first.** These files contain essential patterns, color palettes, naming conventions, and constraints that MUST be followed.

| Change Type | Read First | Why It's Critical |
| ----------- | ---------- | ----------------- |
| Styling, CSS, SCSS, colors, layout, visual appearance | [_sass/AGENTS.md](_sass/AGENTS.md) | Contains site color palette, spacing variables, component patterns |
| Client-side JavaScript, DOM manipulation, event handling | [assets/js/AGENTS.md](assets/js/AGENTS.md) | Module architecture, initialization patterns, performance rules |
| PowerShell scripts, automation, RSS processing | [scripts/AGENTS.md](scripts/AGENTS.md) | Script patterns, error handling, path conventions |
| Jekyll plugins, Ruby, page generation | [_plugins/AGENTS.md](_plugins/AGENTS.md) | Plugin architecture, Liquid integration patterns |
| RSS feeds, feed configuration | [rss/AGENTS.md](rss/AGENTS.md) | Feed structure, caching rules |
| Tests (any language) | [spec/AGENTS.md](spec/AGENTS.md) | Test organization, framework-specific patterns |
| Documentation | [docs/AGENTS.md](docs/AGENTS.md) | Writing standards, file organization |

**Failure to read domain AGENTS.md files results in:**

- Using wrong colors/styles that don't match the site theme
- Breaking established patterns and conventions
- Creating inconsistent code that requires rework

### CRITICAL: Documentation Reference by Topic

**Before making ANY changes, first consult these docs when working on specific features or areas.** These documents provide in-depth explanations of architecture, functionality, patterns, and best practices.

| Topic | Read This | When To Use |
| ----- | --------- | ----------- |
| Site structure, sections, collections | [site-overview.md](docs/site-overview.md) | Adding or modifying sections, understanding URL patterns, working with collections, changing navigation structure, adding RSS feeds |
| Content creation, RSS feeds, AI processing | [content-management.md](docs/content-management.md) | Creating content manually or via GitHub Copilot commands, configuring RSS sources, understanding the AI transformation pipeline, managing GitHub Copilot features videos |
| Jekyll patterns, Liquid, layouts | [jekyll-development.md](docs/jekyll-development.md) | Writing Liquid templates, working with includes and layouts, debugging Jekyll builds, understanding plugin integration, managing Jekyll server |
| Tag filtering, filter UI, filter counts | [filtering-system.md](docs/filtering-system.md) | Modifying filter behavior, understanding the "20 + same-day" rule, working with tag relationships, debugging filter count issues, implementing text search |
| Dates, timezones, scheduling | [datetime-processing.md](docs/datetime-processing.md) | Any date/time operations, fixing timezone issues, working with epoch timestamps, scheduling content publication |
| Project vocabulary, concept definitions | [terminology.md](docs/terminology.md) | Understanding project-specific terms like sections vs collections, clarifying the difference between "post" and "blog", learning filter terminology |
| Markdown structure, frontmatter | [markdown-guidelines.md](docs/markdown-guidelines.md) | Creating or editing any markdown file, setting up frontmatter fields, understanding content structure rules, fixing formatting issues |
| Performance optimization | [performance-guidelines.md](docs/performance-guidelines.md) | Optimizing page load times, reducing JavaScript bundle size, improving server-side rendering, debugging slow pages |
| Documentation organization | [documentation-guidelines.md](docs/documentation-guidelines.md) | Writing or updating documentation, understanding the docs hierarchy, deciding where new documentation belongs |

### Code Quality

- Always test real implementation, never duplicate logic in tests
- Add tests when creating or changing functionality
- Follow existing patterns and conventions in each language
- Use meaningful variable names and proper error handling
- Prefer clarity over cleverness

### CRITICAL: Writing Style Guidelines

**Always read and follow [writing-style-guidelines.md](docs/writing-style-guidelines.md) for ALL output.** This includes code comments, documentation, chat responses, and generated content. The guidelines define tone, voice, language standards, and character/typography rules that apply universally.

### CRITICAL: Tool Calling Priority

**Always prefer higher-level tools over lower-level alternatives.** Follow this priority order:

1. **MCP tools** (highest priority): Use MCP server tools first if available
2. **Built-in tools**: Use VS Code / agent built-in tools if no MCP tool exists
3. **Command-line tools** (lowest priority): Use CLI tools only as a last resort

**Examples:**

| Task | Prefer | Avoid |
| ---- | ------ | ----- |
| GitHub operations | MCP GitHub tools, built-in GitHub tools | `gh` CLI |
| Text replacement | Built-in `replace_string_in_file` | `sed`, PowerShell string manipulation |
| File search | Built-in `grep_search`, `file_search` | `grep`, `find` |
| Reading files | Built-in `read_file` | `cat`, `Get-Content` |

**When command-line is acceptable:**

- Complex multi-step operations that would require many tool calls
- Operations not supported by MCP or built-in tools

**Scripts are a last resort:**

- Only use scripts for genuinely complicated logic that cannot be expressed with tool calls
- Simple operations like text replacement, file reading, or searches should NEVER use scripts
- If a script IS required, it MUST be a PowerShell script (`.ps1`)
- **ALWAYS** place temporary scripts in `/tmp/`
- Clean up temporary scripts after use
- Never create temporary files in the repository root or working directories

### Path References

- Workspace root in devcontainer: `/workspaces/techhub/`
- Always use absolute paths when working with repository files
- PowerShell scripts reference: `scripts/` directory
- RSS feed files: `rss/` directory
- Data files: `_data/` directory

### Documentation

- **Hierarchy**:
  1. **AGENTS.md** (this file): Primary entry point and agent navigation
  2. **Domain AGENTS.md**: Specific development rules (e.g., `scripts/AGENTS.md`)
  3. **docs/**: Comprehensive architectural and feature documentation
- **Rule**: Prefer referencing `docs/` files over duplicating content in `AGENTS.md` files
- **Maintenance**: Update documentation when changing behavior; keep in sync with code
- **Standards**: Follow [writing-style-guidelines](docs/writing-style-guidelines.md) and [documentation-guidelines](docs/documentation-guidelines.md)

### Testing

- Test frameworks: RSpec (Ruby), Jest (JavaScript), Pester (PowerShell), Playwright (E2E)
- Run tests before committing changes
- See [spec/AGENTS.md](spec/AGENTS.md) for comprehensive testing strategy

### Dependencies

- Install workspace dependencies in `.devcontainer/post-create.sh`
- Check `.github/workflows/` when updating post-create script
- Never install dependencies in PowerShell scripts or other runtime contexts

### Never Do

- Never invent or fabricate information
- Never use unverifiable content
- Never leave documentation out of sync with code
- Never add wrapper methods in production code just for tests
- Never make code backwards compatible unless explicitly requested
- Never add comments describing what you changed
- Never start responses with "Sure!" or "You're right!"

## Key Concepts

### Section Architecture

The site is organized into multiple sections (AI, GitHub Copilot, Azure, etc.), each with auto-generated index pages, RSS feeds, and collection pages.

For complete section details, URLs, and structure, see [site-overview.md](docs/site-overview.md#section-architecture).

### Collections System

Collections (posts, news, videos, community, events, roundups) represent different content types defined in `_config.yml`.

For complete collection details, see [site-overview.md](docs/site-overview.md#collections-system).

### Configuration

**`_data/sections.json`** - Single source of truth for section configuration. Always use `site.data.sections` in Liquid templates.

**Global Standards**:

- **Timezone**: All date operations use `Europe/Brussels` timezone
- **Data Access**: Always use `site.data.sections` in Liquid templates

For configuration details, see [site-overview.md](docs/site-overview.md#_datasectionsjson---source-of-truth).

### Plugin System

Jekyll plugins in `_plugins/` handle page generation, filtering, and content processing. For plugin details, see [_plugins/AGENTS.md](_plugins/AGENTS.md).

### Filtering System

Tag-based modular filtering with pre-calculated relationships and client-side real-time updates. See [filtering-system.md](docs/filtering-system.md).

### RSS Processing

Automated hourly RSS downloads with AI-powered content transformation. See [content-management.md](docs/content-management.md#rss-feed-processing).

## Common Commands

```bash
# Jekyll server
./scripts/jekyll-start.ps1
./scripts/jekyll-stop.ps1

# Run all tests
./scripts/run-all-tests.ps1
```

For domain-specific commands, see the relevant AGENTS.md file.
