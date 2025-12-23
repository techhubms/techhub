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
- **AI Integration**: Azure OpenAI / GitHub Models
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

### Code Quality

- Always test real implementation, never duplicate logic in tests
- Add tests when creating or changing functionality
- Follow existing patterns and conventions in each language
- Use meaningful variable names and proper error handling
- Prefer clarity over cleverness

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

## Resources

### Domain-Specific AGENTS.md Files

- [scripts/AGENTS.md](scripts/AGENTS.md) - PowerShell development
- [_plugins/AGENTS.md](_plugins/AGENTS.md) - Jekyll Ruby plugins
- [assets/js/AGENTS.md](assets/js/AGENTS.md) - Client-side JavaScript
- [_sass/AGENTS.md](_sass/AGENTS.md) - SCSS styling
- [rss/AGENTS.md](rss/AGENTS.md) - RSS feed management
- [docs/AGENTS.md](docs/AGENTS.md) - Documentation guidelines
- [spec/AGENTS.md](spec/AGENTS.md) - Testing strategies

### Documentation

- [site-overview.md](docs/site-overview.md) - Site architecture and structure
- [content-management.md](docs/content-management.md) - Content creation and RSS processing
- [jekyll-development.md](docs/jekyll-development.md) - Jekyll-specific patterns
- [filtering-system.md](docs/filtering-system.md) - Filtering implementation
- [datetime-processing.md](docs/datetime-processing.md) - Date and timezone handling
- [terminology.md](docs/terminology.md) - Project vocabulary
- [writing-style-guidelines.md](docs/writing-style-guidelines.md) - Content writing standards
- [markdown-guidelines.md](docs/markdown-guidelines.md) - Markdown formatting
- [performance-guidelines.md](docs/performance-guidelines.md) - Performance optimization
- [documentation-guidelines.md](docs/documentation-guidelines.md) - Documentation structure

## Never Do

- Never invent or fabricate information
- Never use unverifiable content
- Never leave documentation out of sync with code
- Never add wrapper methods in production code just for tests
- Never make code backwards compatible unless explicitly requested
- Never add comments describing what you changed
- Never start responses with "Sure!" or "You're right!"
