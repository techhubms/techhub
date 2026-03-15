# Repository Structure

## Source Code (`src/`)

### Core Projects

- **`TechHub.Api/`** - REST API backend built with ASP.NET Core Minimal API
  - Exposes all content operations via OpenAPI-documented endpoints
  - Handles content retrieval, filtering, search, and RSS feed generation
  - See [src/TechHub.Api/AGENTS.md](../src/TechHub.Api/AGENTS.md) for API development guidelines
- **`TechHub.Web/`** - Blazor InteractiveServer frontend with prerendering
  - Server-side rendering for optimal SEO, followed by SignalR-based interactivity
  - Global InteractiveServer render mode (no per-component render mode decisions)
  - Implements the design system and all UI components
  - See [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) for frontend development guidelines
- **`TechHub.Core/`** - Domain models, interfaces, and business logic
  - Contains all entity definitions (ContentItem, Collection, Section, Tag)
  - Defines repository interfaces and core abstractions
  - Framework-agnostic, reusable across all projects
  - See [src/TechHub.Core/AGENTS.md](../src/TechHub.Core/AGENTS.md) for domain modeling guidelines

### Infrastructure & Services

- **`TechHub.Infrastructure/`** - Data access and service implementations
  - Dapper-based repository implementations for PostgreSQL
  - Content processing services (markdown parsing, frontmatter extraction)
  - Caching, logging, and cross-cutting concerns
  - See [src/TechHub.Infrastructure/AGENTS.md](../src/TechHub.Infrastructure/AGENTS.md) for infrastructure patterns
- **`TechHub.AppHost/`** - .NET Aspire orchestration and local development
  - Coordinates API and Web projects during development
  - Provides Aspire Dashboard for monitoring and diagnostics
  - Configures service discovery and container orchestration
- **`TechHub.ServiceDefaults/`** - Shared .NET Aspire service configuration
  - Common observability, health checks, and service defaults
  - Reusable across all Aspire-managed services

### Utilities

- **`TechHub.ContentFixer/`** - CLI utility for content maintenance
  - Bulk updates to frontmatter across collections
  - Content validation and normalization
  - Run from command line or as part of CI/CD pipeline

See [src/AGENTS.md](../src/AGENTS.md) for general .NET development patterns and architecture guidelines.

## Content (`collections/`)

Markdown content organized by content type. Each collection is synced to the database during application startup or via the ContentSyncService.

- **`_news/`** - Official GitHub Copilot announcements and product updates
- **`_videos/`** - Video content and tutorials (YouTube embeds, tutorials)
- **`_community/`** - Microsoft Tech Community posts and community contributions
- **`_custom/`** - Custom manually created pages (e.g., About, Documentation)
- **`_blogs/`** - Technical articles and blogs from various Microsoft sources
- **`_roundups/`** - Curated weekly summaries (e.g., This Week in GitHub Copilot)

All collections follow the [frontmatter schema](frontmatter.md) and are processed according to [content-processing.md](content-processing.md).

See [collections/AGENTS.md](../collections/AGENTS.md) for content creation and maintenance guidelines.

## Tests (`tests/`)

### Test Projects

- **`TechHub.Core.Tests/`** - Unit tests for domain logic
  - Focus on edge cases and business rules
  - Fast, isolated tests with no external dependencies
  - See [tests/TechHub.Core.Tests/AGENTS.md](../tests/TechHub.Core.Tests/AGENTS.md)
- **`TechHub.Api.Tests/`** - Integration tests for API endpoints (**PRIMARY TEST LAYER**)
  - Tests all API functionality via HTTP requests
  - Uses PostgreSQL Testcontainers database
  - Mandatory for all API changes - follows Testing Diamond pattern
  - See [tests/TechHub.Api.Tests/AGENTS.md](../tests/TechHub.Api.Tests/AGENTS.md)
- **`TechHub.Web.Tests/`** - bUnit component tests for Blazor UI
  - Tests individual components in isolation
  - Validates rendering, user interactions, and component state
  - See [tests/TechHub.Web.Tests/AGENTS.md](../tests/TechHub.Web.Tests/AGENTS.md)
- **`TechHub.E2E.Tests/`** - Playwright end-to-end tests (**MANDATORY FOR UI CHANGES**)
  - Tests complete user journeys across API + Web
  - Browser automation for realistic user scenarios
  - Organized into xUnit test collections per feature area
  - See [tests/TechHub.E2E.Tests/AGENTS.md](../tests/TechHub.E2E.Tests/AGENTS.md)
- **`TechHub.Infrastructure.Tests/`** - Infrastructure layer tests
  - Tests repository implementations, caching, and services
  - Database integration tests
  - See [tests/TechHub.Infrastructure.Tests/AGENTS.md](../tests/TechHub.Infrastructure.Tests/AGENTS.md)
- **`powershell/`** - Pester tests for PowerShell scripts
  - Tests automation scripts in `scripts/`
  - See [tests/powershell/AGENTS.md](../tests/powershell/AGENTS.md)

### Test Utilities & Test Collections

- **`TechHub.TestUtilities/`** - Shared test infrastructure
  - **`TestCollections/`** - Sample markdown files for testing (mirrors `collections/` structure):
    - `_blogs/` - Sample blog posts
    - `_community/` - Sample community posts
    - `_custom/` - Sample custom pages
    - `_news/` - Sample news items
    - `_roundups/` - Sample roundup posts
    - `_videos/` - Sample video content
  - `DatabaseFixture.cs` - In-memory database setup for integration tests
  - `TechHubApiFactory.cs` - WebApplicationFactory for API testing
  - `TestCollectionsSeeder.cs` - Seeds test database from TestCollections
  - See [tests/TechHub.TestUtilities/AGENTS.md](../tests/TechHub.TestUtilities/AGENTS.md)

See [tests/AGENTS.md](../tests/AGENTS.md) for testing strategies, TDD workflow, and Testing Diamond pattern.

## Configuration & Documentation

### Critical Configuration Files

- **`AGENTS.md`** - Root AI assistant workflow (8-step development process) - **READ THIS FIRST**
- **`appsettings.json`** - Application configuration (API, Web projects)
  - Section and collection definitions
  - Database connection strings
  - Service settings and feature flags
- **`Directory.Build.props`** - MSBuild properties for all projects
- **`TechHub.slnx`** - Solution file (XML-based .NET 9+ format)
- **`docker-compose.yml`** - Multi-container Docker configuration
- **`package.json`** - Node.js dependencies (markdownlint, etc.)
- **`.editorconfig`** - Code style and formatting rules
- **`.runsettings`** - Test execution configuration

### Documentation (`docs/`)

Comprehensive functional and technical documentation. **Always consult [docs/documentation-index.md](documentation-index.md) to find relevant documentation.**

- **`documentation-index.md`** - **INDEX OF ALL DOCUMENTATION** - use this to find what you need
- **`architecture.md`** - System architecture overview
- **`technology-stack.md`** - Technologies used and rationale
- **`repository-structure.md`** - This file
- **`running-and-testing.md`** - How to build, run, and test the application
- **`testing-strategy.md`** - Testing approach and patterns
- **Additional docs:** caching, content-api, database, design-system, filtering, frontmatter, health-checks, render-modes, RSS feeds, SEO, and more

See [docs/AGENTS.md](AGENTS.md) for documentation maintenance guidelines.

### Automation & Scripts (`scripts/`)

PowerShell automation scripts for development and maintenance tasks:

- **`TechHubRunner.psm1`** - **PRIMARY BUILD/RUN/TEST MODULE** - exports `Run` function
  - `Run` - Build, test, and start servers (use this for all build/test operations)
  - `Run -WithoutTests` - Start servers without running tests
  - `Run -TestProject <name>` - Run specific test project

- **`Generate-DocumentationIndex.ps1`** - Generates `docs/documentation-index.md` from doc headings
- **`Generate-DevCertificate.ps1`** - Creates HTTPS dev certificates
- **`Normalize-Images.ps1`** - Optimizes and normalizes images
- **`Deploy-Infrastructure.ps1`** - Azure infrastructure deployment (Bicep)
  - Supports shared, staging, and production environments
  - Modes: validate, whatif, deploy
  - Pre-flight checks (soft-deleted AI purge, ACR pull roles)
- **`Deploy-Application.ps1`** - Container image build, push, and deployment
  - Builds and pushes Docker images to ACR
  - Deploys to Azure Container Apps
  - Default 'dev' tag for local builds, git SHA in CI
  - Smoke tests and automatic production rollback
- **`analyze-markdown-errors.ps1`** - Linting and markdown analysis
- **`content-processing/`** - Content import and transformation scripts
- **`data/`** - Data files for scripts

See [scripts/AGENTS.md](../scripts/AGENTS.md) for scripting guidelines and patterns.

### Infrastructure (`infra/`)

Azure infrastructure as code (Bicep templates) for Azure Container Apps deployment:

- **`main.bicep`** - Main infrastructure orchestration template
- **`parameters/`** - Environment-specific parameter files:
  - `staging.bicepparam` - Staging environment
  - `prod.bicepparam` - Production environment
- **`modules/`** - Reusable Bicep modules:
  - `containerApps.bicep` - Container Apps Environment
  - `api.bicep` - TechHub API Container App
  - `web.bicep` - TechHub Web Container App
  - `monitoring.bicep` - Application Insights + Log Analytics
  - `registry.bicep` - Azure Container Registry

Deployment is managed via PowerShell scripts (`scripts/Deploy-Infrastructure.ps1` and `scripts/Deploy-Application.ps1`) which are called by GitHub Actions workflows. Scripts can also be run locally for testing.

See [specs/008-azure-infrastructure/spec.md](../specs/008-azure-infrastructure/spec.md) for architecture details.

### Feature Specifications (`specs/`)

Detailed specifications for major features that still need to be built (following SpecKit structure):

- `001a-tag-counting/` - Dynamic tag counts feature
- `001b-date-range-slider/` - Date range filtering
- `001c-tag-dropdown-filter/` - Excel-style tag dropdown
- `004-custom-pages/` - Custom markdown pages
- `005-mobile-navigation/` - Mobile navigation
- `006-seo/` - SEO optimization
- `007-google-analytics/` - Analytics integration
- `008-azure-infrastructure/` - Azure deployment
- `009-ci-cd-pipeline/` - CI/CD automation

Each spec folder contains `spec.md`, `plan.md`, and `tasks.md` files.

## Development Environment & Working Directories

### `.devcontainer/`

Dev container configuration for VS Code and GitHub Codespaces:

- **`devcontainer.json`** - Container configuration, extensions, settings
- **`post-create.sh`** - Post-creation setup script. Add ALL dependencies needed for the devcontainer here

### `.databases/postgres/**`

Local database storage (gitignored) when running with PostgreSQL.

Delete the entire postgres directory if you need a fresh database. It will automatically get recreated based on our source collection files.

### `.tmp/`

Temporary working directory for AI assistants and development tasks (gitignored):

- PowerShell analysis scripts and output
- Benchmark results and performance data
- Backup files and test artifacts
- Experimental code and migration plans
- **Always store temporary files here** - never commit to repo

### Other Hidden Directories

- **`.github/`** - GitHub Actions workflows and CI/CD automation
  - `workflows/ci-cd.yml` - Unified CI/CD pipeline: CI (build, test, lint, security) runs on all PRs and pushes; deployment to staging (automatic) and production (manual approval) runs after quality gate passes (PRs skip deployment)
  - See [ci-cd-pipeline.md](ci-cd-pipeline.md) for complete CI/CD documentation
- **`.vscode/`** - VS Code workspace settings and launch configurations
- **`.git/`** - Git repository metadata
- **`.specify/`** - SpecKit configuration and templates
- **`.playwright-mcp/`** - Playwright MCP (Model Context Protocol) configuration
- **`.dockerignore`** - Files to exclude from Docker builds
