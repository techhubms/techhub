# Tech Hub

> **Status**: 🚧 In Development

Tech Hub is a **modern .NET web application** built with Blazor that serves as a technical content hub. We're creating a fast, responsive, accessible platform for showcasing Microsoft technical content across AI, Azure, GitHub Copilot, .NET, DevOps, and Security topics.

## Quick Start

**Easiest Way - F5 in VS Code**:

1. Open the project in VS Code
2. Press **F5** (or click **Run > Start Debugging**)
3. Select **"Tech Hub (API + Web)"** from the dropdown
4. Web UI opens at <https://localhost:5003> (accept dev certificate warning)
5. API available at <https://localhost:5001> (Swagger: <https://localhost:5001/swagger>)
6. Aspire Dashboard at <https://localhost:18888> (token shown in console)

> **Note**: You'll see browser warnings about self-signed certificates - this is normal for local development. Click "Advanced" → "Proceed" to continue.

**Alternative - PowerShell Function**:

```powershell
# Build, test, and run servers in background
Run

# Build and start servers (no tests)
Run -WithoutTests
```

Servers run in background and keep running. When using a subsequent `Run` command the script automatically detects if binaries changed and if servers need to be restarted because of that. Server logs are in `.tmp/logs/`.

## .NET Aspire

Tech Hub uses **.NET Aspire** for orchestration, observability, and service discovery. Aspire provides:

- **Service Discovery**: Web frontend automatically discovers the API via `https+http://api`
- **OpenTelemetry**: Distributed tracing, metrics, and structured logging
- **Resilience**: Built-in retry policies and circuit breakers for HTTP clients
- **Health Checks**: `/health` and `/alive` endpoints on all services

### Running with Aspire

#### Via Run Function (Recommended)

```powershell
# Uses Aspire AppHost to orchestrate API + Web
Run
```

#### Direct AppHost

```powershell
dotnet run --project src/TechHub.AppHost/TechHub.AppHost.csproj
```

#### VS Code Task

Press `Ctrl+Shift+P` → "Tasks: Run Task" → "run-apphost"

### Aspire Dashboard

The Aspire Dashboard provides real-time visualization of:

- **Traces**: Distributed request tracing across services
- **Metrics**: Performance counters, request rates, error rates
- **Logs**: Structured logs from all services
- **Resources**: Service health and status

To start the dashboard:

```powershell
# Start with dashboard (runs as Docker container)
Run -Dashboard

# Dashboard URL: https://localhost:18888
# Note: Copy the login token from the terminal output
```

Manual dashboard start (if needed separately):

```bash
docker run --rm -it -d \
    -p 18888:18888 \
    -p 4317:18889 \
    --name aspire-dashboard \
    mcr.microsoft.com/dotnet/aspire-dashboard:9.5
```

### Architecture

```text
┌─────────────────────────────────────────────────────────────┐
│                    Aspire AppHost                           │
│  (Orchestrates services, configures service discovery)      │
└─────────────────────────────────────────────────────────────┘
                              │
              ┌───────────────┼───────────────┐
              ▼               ▼               ▼
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│   TechHub.Api   │  │   TechHub.Web   │  │ Aspire Dashboard│
│   (Port 5029)   │◄─┤   (Port 5184)   │  │  (Port 18888)   │
│                 │  │                 │  │                 │
│ - REST API      │  │ - Blazor SSR    │  │ - Traces        │
│ - Swagger UI    │  │ - Service       │  │ - Metrics       │
│ - Health checks │  │   Discovery     │  │ - Logs          │
└─────────────────┘  └─────────────────┘  └─────────────────┘
         │                    │                    ▲
         │                    │                    │
         └────────────────────┴──── OTLP ─────────┘
                        (OpenTelemetry)
```

### ServiceDefaults

The `TechHub.ServiceDefaults` project provides shared Aspire configuration:

```csharp
// In API and Web Program.cs
builder.AddServiceDefaults();  // Adds OpenTelemetry, health checks, resilience
// ...
app.MapDefaultEndpoints();     // Maps /health and /alive endpoints
```

## DevContainer Setup

1. In VS Code, open Command Palette (`Ctrl+Shift+P`)
2. Select **"Dev Containers: Reopen in Container"**
3. Wait for container to build and initialize
4. Use F5 or run script as described above

## Repository Organization

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

- `appsettings.json` - Application configuration (sections, collections, service settings)
- `docs/` - Functional documentation (API spec, filtering, content management)
- `scripts/` - Automation and utility scripts (PowerShell)
- `infra/` - Azure infrastructure (Bicep templates)
- `specs/` - Feature specifications (planning docs, may be outdated)

## Tech Stack

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

## Core Development Principles

### Configuration-Driven Design

- All sections and collections defined in `appsettings.json`
- Content structure managed through configuration, not code
- New sections added by updating configuration per environment
- Single source of truth ensures consistency across environments

### Performance & User Experience

- Server-side rendering for fast initial page loads
- Client-side enhancement for responsive interactions
- Pre-computation during build for optimal runtime performance
- Resilience policies (retry, circuit breaker, timeout)

### Quality & Maintainability

- Test-driven development at all layers
- Comprehensive test coverage (unit, integration, component, E2E)
- Clean architecture with separation of concerns
- Zero-warning policy for code quality

### Accessibility Standards

All user interface components and interactions must be accessible to users with disabilities following WCAG 2.1 Level AA standards.

**Key requirements**:

- Keyboard navigation for all interactive elements
- Color contrast ratios of at least 4.5:1 for normal text
- Screen reader support with semantic HTML and ARIA labels
- Test with keyboard-only navigation and screen readers

### Timezone & Date Handling

- **Define once**: `Europe/Brussels` timezone for all date operations
- **Apply everywhere**: Build process, templates, client code must all use same timezone
- **Store universally**: Use Unix epoch timestamps as primary storage format
- **Display locally**: Convert to timezone only when rendering for users

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

**Linking Strategy**: Collections have different link behaviors based on their content type:

- **External Linking** (news, blogs, community): Always redirect users to the original source URL when clicked. These collections represent content that should be consumed at the original publisher's site.
- **Internal Linking** (videos, roundups, custom): Link to pages within the Tech Hub site. These collections represent content that can be fully presented on our site, such as embedded videos or curated summaries.

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

## Starting, Stopping and Testing the Website

### Starting the Website

**ALWAYS use the Run function directly** (automatically loaded in PowerShell):

The `Run` function builds, tests, and starts servers **in background**. After `Run` completes, the terminal is immediately free to use for other commands. Servers continue running in background with output redirected to `.tmp/logs/`.

**Standard Run Commands**:

```powershell
# Default development workflow - build, test, start servers in background
Run

# Scoped testing - only runs E2E tests
Run -TestProject E2E.Tests

# Start servers for debugging (no tests)
Run -WithoutTests

# Run tests and stop servers after (CI/CD mode)
Run -StopServers
```

**Log Files**:

- **Console output**: `.tmp/logs/console.txt` (Development) or `api-console.txt`/`web-console.txt` (Production)
- **API logs**: `.tmp/logs/api-dev.log` (-prod for Production mode)
- **Web logs**: `.tmp/logs/web-dev.log` (-prod for Production mode)

### Testing Workflows

**Simple decision tree for running tests**:

```text
┌─────────────────────────────────────────────────────────────┐
│ What are you doing?                                         │
└─────────────────────────────────────────────────────────────┘
                        │
        ┌───────────────┼───────────────┐
        ▼               ▼               ▼
  
  📝 Changed        🐛 Fixing         🔍 Exploring/
  any code?         a bug?            Debugging?
        │               │                   │
        ▼               ▼                   ▼
  Run               Run -WithoutTests  Run -WithoutTests
  (verify all       (debug first,      (explore with
   tests pass)       write test,        Playwright MCP
                     then fix)          interactively)
```

**Quick Reference**:

- **Run** - Build, all tests, servers stay running (default development workflow)
- **Run -WithoutTests** - Build, servers, skip tests (debugging/exploration)
- **Run -TestProject Web.Tests** - Run only Web component tests
- **Run -TestProject E2E.Tests** - Run only E2E tests
- **Run -TestName SectionCard** - Run tests matching 'SectionCard'
- **Run -StopServers** - CI/CD mode (stop servers after tests)

**For detailed testing patterns**, see [tests/AGENTS.md](tests/AGENTS.md).

### Stopping the Website

Servers run in background and keep running. To stop them:

```powershell
Run -StopServers
```

Or force kill:

```powershell
Get-Process dotnet | Stop-Process -Force
```

## Documentation Architecture

The Tech Hub uses a **multi-tier documentation system** organized by scope and domain:

### Documentation Hierarchy

**1. [AGENTS.md](AGENTS.md)** - AI Assistant Workflow

- **Purpose**: The required 10-step development workflow for AI coding agents
- **Contains**: Development process with checklists, core rules and boundaries
- **Scope**: Process and methodology for ALL development tasks

**2. This file (README.md)** - General Project Information

- **Purpose**: Project overview, tech stack, running/testing, terminology
- **Contains**: Quick start, architecture, development principles, site concepts
- **Scope**: Understanding the project and getting started

**3. Domain-Specific AGENTS.md Files** - Technical Implementation Details

- **Purpose**: Development patterns for specific code domains
- **Contains**: Code patterns, framework-specific guidance, domain rules
- **Examples**:
  - [src/AGENTS.md](src/AGENTS.md) - .NET development patterns
  - [src/TechHub.Api/AGENTS.md](src/TechHub.Api/AGENTS.md) - API endpoint patterns
  - [src/TechHub.Web/AGENTS.md](src/TechHub.Web/AGENTS.md) - Blazor component patterns
  - [tests/AGENTS.md](tests/AGENTS.md) - Testing strategies
  - [scripts/AGENTS.md](scripts/AGENTS.md) - PowerShell automation

**4. Functional Documentation** (`docs/`)

- **Purpose**: WHAT the system does (behavior, contracts, rules)
- **Contains**: API specifications, content management workflows, feature descriptions
- **Files**:
  - [content-management.md](docs/content-management.md) - Content workflows and RSS processing
  - [api-specification.md](docs/api-specification.md) - REST API contracts
  - [rss-feeds.md](docs/rss-feeds.md) - RSS feed system
  - [toc-component.md](docs/toc-component.md) - Table of contents architecture

**5. Content Guidelines** (`collections/`)

- **Purpose**: Writing standards and content creation workflows
- **Files**:
  - [AGENTS.md](collections/AGENTS.md) - Content management with frontmatter schema
  - [writing-style-guidelines.md](collections/writing-style-guidelines.md) - Writing tone and style

### Quick Reference Guide

**New to the project?**

1. Start with this file (README.md) for overview and quick start
2. Read [AGENTS.md](AGENTS.md) for the required development workflow
3. Review domain-specific AGENTS.md before coding

**Working on .NET/Blazor?**

1. Read [AGENTS.md](AGENTS.md) for the 10-step workflow
2. Read [src/AGENTS.md](src/AGENTS.md) for general .NET patterns
3. Read specific domain AGENTS.md (API, Web, Core, Infrastructure)

**Working on PowerShell scripts?**

1. Read [AGENTS.md](AGENTS.md) for the 10-step workflow
2. Read [scripts/AGENTS.md](scripts/AGENTS.md)

**Working on content?**

1. Read [collections/AGENTS.md](collections/AGENTS.md)
2. Follow [writing-style-guidelines.md](collections/writing-style-guidelines.md)
3. Use `npx markdownlint-cli2 --fix <file-path>` to fix formatting

**Working on tests?**

1. Read [AGENTS.md](AGENTS.md) for the 10-step workflow
2. Read [tests/AGENTS.md](tests/AGENTS.md) for testing strategies
3. Read specific test domain AGENTS.md files

**Understanding system behavior?**

1. Read [docs/content-management.md](docs/content-management.md) for content workflows
2. Read [docs/api-specification.md](docs/api-specification.md) for API contracts
3. Read [docs/rss-feeds.md](docs/rss-feeds.md) for RSS feed system

## Contributing

All development must follow the guidelines in [AGENTS.md](AGENTS.md):

- **[AI Assistant Workflow](AGENTS.md#ai-assistant-workflow)** - Required 10-step process with checklists
- **[Core Rules & Boundaries](AGENTS.md#core-rules--boundaries)** - Non-negotiable rules
- **[Feature Specifications](specs/)** - Use spec-kit methodology for all features
