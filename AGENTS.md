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

- Update documentation when changing behavior
- Keep documentation in sync with code
- Follow [writing-style-guidelines](docs/writing-style-guidelines.md) for all text
- See [documentation-guidelines](docs/documentation-guidelines.md) for structure

### Testing

- Test frameworks: RSpec (Ruby), Jest (JavaScript), Pester (PowerShell), Playwright (E2E)
- Run tests before committing changes
- See [testing-guidelines](docs/testing-guidelines.md) for comprehensive strategy

### Dependencies

- Install workspace dependencies in `.devcontainer/post-create.sh`
- Check `.github/workflows/` when updating post-create script
- Never install dependencies in PowerShell scripts or other runtime contexts

## Key Concepts

### Content Structure

- **Collections**: `_posts`, `_news`, `_videos`, `_community`, `_events`, `_roundups`
- **Sections**: AI, Azure, Coding, DevOps, GitHub Copilot, ML, Security
- **Tags**: Primary filtering mechanism, pre-calculated relationships
- See [site-overview](docs/site-overview.md) and [terminology](docs/terminology.md)

### Filtering System

- Tag-based modular system across sections, collections, and content
- Pre-calculated relationships for performance
- Client-side real-time filtering
- See [filtering-system](docs/filtering-system.md)

### RSS Processing

- Automated hourly downloads from configured feeds
- AI-powered content transformation to markdown
- Collection-aware prioritization
- See [rss-feeds](docs/rss-feeds.md)

## Common Commands

### Testing

```bash
# All tests
./scripts/run-all-tests.ps1

# Specific test suites
./scripts/run-powershell-tests.ps1
./scripts/run-javascript-tests.ps1
./scripts/run-plugin-tests.ps1
./scripts/run-e2e-tests.ps1
```

### Jekyll Development

```bash
# Start Jekyll server
./scripts/jekyll-start.ps1

# Stop Jekyll server
./scripts/jekyll-stop.ps1
```

### RSS Processing

```bash
# Download RSS feeds
./scripts/download-rss-feeds.ps1 -WorkspaceDirectory .

# Process RSS to markdown
./scripts/process-rss-to-markdown.ps1 "owner/repo" "token" -WorkspaceDirectory .
```

## Domain-Specific Guidelines

For specialized development in specific areas:

- **PowerShell Development**: See [scripts/AGENTS.md](scripts/AGENTS.md)
- **Ruby Plugin Development**: See [_plugins/AGENTS.md](_plugins/AGENTS.md)
- **JavaScript Development**: See [assets/js/AGENTS.md](assets/js/AGENTS.md)
- **Styling (SCSS)**: See [_sass/AGENTS.md](_sass/AGENTS.md)
- **RSS Feed Management**: See [rss/AGENTS.md](rss/AGENTS.md)
- **Documentation**: See [docs/AGENTS.md](docs/AGENTS.md)
- **Testing**: See [spec/AGENTS.md](spec/AGENTS.md)

## Resources

### Essential Documentation

Read these before making changes:

- [README.md](README.md) - Project introduction and quick start
- [content-management.md](docs/content-management.md) - Content creation and organization
- [jekyll-development.md](docs/jekyll-development.md) - Jekyll-specific patterns
- [performance-guidelines.md](docs/performance-guidelines.md) - Performance optimization

### Full Documentation Index

- [datetime-processing.md](docs/datetime-processing.md) - Date handling and timezone configuration
- [filtering-system.md](docs/filtering-system.md) - Complete filtering implementation
- [markdown-guidelines.md](docs/markdown-guidelines.md) - Markdown structure for AI models
- [site-overview.md](docs/site-overview.md) - Comprehensive site structure
- [terminology.md](docs/terminology.md) - Project vocabulary and concepts
- [writing-style-guidelines.md](docs/writing-style-guidelines.md) - Content writing standards

## Never Do

- Never invent or fabricate information
- Never use unverifiable content
- Never leave documentation out of sync with code
- Never add wrapper methods in production code just for tests
- Never make code backwards compatible unless explicitly requested
- Never add comments describing what you changed
- Never start responses with "Sure!" or "You're right!"
