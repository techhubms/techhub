# Tech Hub

Welcome to the Tech Hub ([tech.hub.ms](https://tech.hub.ms)) - an automated content aggregation and curation platform for Microsoft and technology sources.

## About This Project

The Tech Hub is an automated content aggregation and curation platform that helps you stay current with Microsoft and technology content.

### Key Features

- **Automated Content Aggregation**: Hourly RSS feed processing with AI-powered categorization using GitHub Models or Azure AI Foundry
- **Dynamic Sections**: Eight specialized technology hubs:
  - Everything (all content)
  - AI (artificial intelligence)
  - GitHub Copilot (AI-powered coding)
  - ML (machine learning)
  - Azure (cloud platform)
  - Coding (.NET ecosystem)
  - DevOps (automation and lifecycle)
  - Security (Microsoft security)
- **Advanced Filtering**: Real-time tag-based and date-based content filtering with no page reloads
- **Multiple Content Types**: News, blogs, videos, community discussions, events, and weekly roundups
- **Performance Optimized**: Server-side content limiting with "20 + Same-Day" rule for fast loading
- **RSS Feeds**: Section-specific and collection-specific RSS feeds for all content types

## Quick Start

Get the Tech Hub running in just a few minutes using containers.

### Prerequisites

#### Dev Container (Recommended)

- Docker: For running the dev container
- VS Code: With the Dev Containers extension
- Git: For version control

#### GitHub Codespaces

- GitHub Account: With access to the repository

### Setup Options

#### Option 1: Dev Container (Recommended)

1. **Open in VS Code**:

   ```bash
   code /path/to/tech
   ```

2. **Reopen in Container**:

   - VS Code will prompt to reopen in dev container
   - Or use Command Palette: "Dev Containers: Reopen in Container"

3. **Start the Site**:

   ```powershell
   pwsh /workspaces/techhub/jekyll-start.ps1
   ```

4. **Access the Site**: Open browser to `http://localhost:4000`

#### Option 2: GitHub Codespaces

1. **Create Codespace**: Go to [GitHub repository](https://github.com/techhubms/techhub) → "Code" → "Codespaces" → "Create codespace on main"
2. **Start the Site**:

   ```powershell
   pwsh /workspaces/techhub/jekyll-start.ps1
   ```

3. **Access the Site**: Codespace will provide a URL, usually accessible through the "Ports" panel

## Architecture

Tech Hub is built as a modern static site with intelligent automation:

### Technology Stack

- **Jekyll 4.4.1**: Static site generator with Liquid templating
- **Ruby 3.2**: Plugin development and server-side processing
- **Node.js 20+**: JavaScript tooling and testing
- **.NET 9.0**: API services and infrastructure components
- **PowerShell**: Automation scripts and RSS processing
- **GitHub Actions**: CI/CD and automated workflows

### Testing Framework

The project uses a comprehensive four-tier testing approach:

- **PowerShell/Pester**: Unit tests for preprocessing scripts
- **JavaScript/Jest**: Unit tests for client-side logic
- **Ruby/RSpec**: Integration tests for Jekyll plugins
- **Playwright**: End-to-end browser testing

Run all tests with:

```powershell
pwsh /workspaces/techhub/run-all-tests.ps1
```

Or run specific test suites:

```powershell
pwsh /workspaces/techhub/run-plugin-tests.ps1      # Ruby/RSpec tests
pwsh /workspaces/techhub/run-javascript-tests.ps1  # JavaScript/Jest tests
pwsh /workspaces/techhub/run-powershell-tests.ps1  # PowerShell/Pester tests
pwsh /workspaces/techhub/run-e2e-tests.ps1         # Playwright E2E tests
```

## Project Structure

```text
techhub/
├── _config.yml              # Jekyll configuration
├── _data/                   # Data files (sections.json)
├── _includes/               # Reusable template components
├── _layouts/                # Page layout templates
├── _plugins/                # Custom Jekyll plugins
├── _sass/                   # Stylesheet organization
├── assets/                  # Static assets (images, CSS, JS)
├── _community/              # Community content collection
├── _events/                 # Events collection
├── _news/                   # News articles collection
├── _posts/                  # Blog posts collection
├── _roundups/               # Weekly roundups collection
├── _videos/                 # Video content collection
├── ai/                      # AI section custom pages
├── github-copilot/          # GitHub Copilot section pages
├── docs/                    # Technical documentation
├── spec/                    # Test files (Ruby, JS, PowerShell, E2E)
├── src/                     # .NET API services
├── infra/                   # Azure infrastructure code
└── .github/                 # GitHub Actions workflows
```

## Content Collections

Tech Hub organizes content into collections:

- **News** (`_news/`): Official announcements and product updates - **830+ items**
- **Posts** (`_posts/`): Blog posts and technical articles - **473+ items**
- **Videos** (`_videos/`): YouTube content and tutorials - **397+ items**
- **Community** (`_community/`): Community-sourced content - **556+ items**
- **Events** (`_events/`): Conferences and meetups - **5+ items**
- **Roundups** (`_roundups/`): Weekly curated summaries - **16+ items**

Each piece of content is automatically categorized using AI and organized by tags for easy filtering.

## Automation

### RSS Feed Processing

The site automatically processes RSS feeds from Microsoft and technology sources:

- **Hourly Processing**: GitHub Actions check for new content every hour
- **AI Categorization**: Content is automatically categorized and tagged using GitHub Models or Azure AI Foundry
- **Per-Entry Enrichment**: Full content is fetched and analyzed for each RSS entry
- **Automatic Publishing**: New content appears without manual intervention

Configure feeds in `.github/scripts/rss-feeds.json`.

### GitHub Actions Workflows

- **RSS Processing**: Hourly content updates from configured feeds
- **PR Validation**: Automated testing on pull requests (Ruby, JavaScript, PowerShell, Playwright)
- **Static Web App Deployment**: Automatic deployment to Azure Static Web Apps
- **Weekly Roundups**: Automated generation of content summaries
- **Infrastructure Deployment**: Azure infrastructure as code with Bicep

## Deployment

The site is deployed to Azure Static Web Apps and automatically updates when changes are pushed to the main branch. The deployment includes:

- **Static Site**: Jekyll-generated static HTML, CSS, and JavaScript
- **API Services**: .NET 9.0 API endpoints for dynamic functionality
- **CDN**: Global content delivery for optimal performance
- **Custom Domain**: Accessible at [tech.hub.ms](https://tech.hub.ms)

Infrastructure is managed as code in the `infra/` directory using Azure Bicep templates.

## Development Workflow

### Starting the Jekyll Server

```powershell
pwsh /workspaces/techhub/jekyll-start.ps1
```

Access the site at `http://localhost:4000`

### Stopping the Jekyll Server

```powershell
pwsh /workspaces/techhub/jekyll-stop.ps1
```

### Creating Content with GitHub Copilot

Use the built-in commands in VS Code with GitHub Copilot:

```text
/new-article        # Create a new article from a URL
/new-rss-feeds      # Add new RSS feeds
/pushall            # Stage, commit, and push changes
```

### Running Tests

```powershell
# Run all tests
pwsh /workspaces/techhub/run-all-tests.ps1

# Run specific test suites
pwsh /workspaces/techhub/run-plugin-tests.ps1
pwsh /workspaces/techhub/run-javascript-tests.ps1
pwsh /workspaces/techhub/run-powershell-tests.ps1
pwsh /workspaces/techhub/run-e2e-tests.ps1
```

## Documentation

The `docs/` directory contains comprehensive documentation organized by topic:

### Getting Started

- [Content Management](docs/content-management.md) - How to add and manage content
- [Site Overview](docs/site-overview.md) - Architecture and structure
- [Terminology](docs/terminology.md) - Key concepts and vocabulary

### Development Guides

- [Jekyll Development](docs/jekyll-development.md) - Jekyll-specific patterns
- [JavaScript Guidelines](docs/javascript-guidelines.md) - Client-side development
- [Ruby Guidelines](docs/ruby-guidelines.md) - Plugin development
- [PowerShell Guidelines](docs/powershell-guidelines.md) - Script standards
- [Markdown Guidelines](docs/markdown-guidelines.md) - Content formatting
- [Testing Guidelines](docs/testing-guidelines.md) - Testing strategy

### Advanced Topics

- [Filtering System](docs/filtering-system.md) - Tag and date filtering implementation
- [RSS Feeds](docs/rss-feeds.md) - Feed configuration and processing
- [Plugins](docs/plugins.md) - Custom plugin development
- [Performance Guidelines](docs/performance-guidelines.md) - Optimization strategies
- [DateTime Processing](docs/datetime-processing.md) - Date handling and timezones

### Reference

- [Documentation Guidelines](docs/documentation-guidelines.md) - Documentation structure
- [Writing Style Guidelines](docs/writing-style-guidelines.md) - Content standards
- [Frontend Guidelines](docs/frontend-guidelines.md) - UI/UX development
- [GitHub Token Setup](docs/github-token-setup.md) - Authentication configuration

### For AI Agents

- [Copilot Instructions](.github/copilot-instructions.md) - Instructions for AI agents working on this repository

## Contributing

We welcome contributions! Please feel free to:

- Submit pull requests
- Report issues
- Suggest improvements
- Fork the repository for your own projects

## Commercial Use & Partnerships

This project is licensed under the MIT License, which allows commercial use. If you're using this project commercially, we'd love to hear from you! Consider:

- **Sponsoring the project** to support ongoing development
- **Partnering with us** for feature development or customization
- **Contributing back** improvements that benefit the community

For commercial partnerships or sponsorship opportunities, please contact [Reinier van Maanen](mailto:reinier.vanmaanen@xebia.com).

## Support

For questions or issues:

1. Check the relevant documentation in the [docs/](docs/) folder
2. Look at existing content files for examples
3. Report problems via GitHub Issues
4. Contact Reinier van Maanen or Rob Bos

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

Copyright (c) 2025 Reinier van Maanen
