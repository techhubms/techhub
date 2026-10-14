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
  - Contains all entity definitions (ContentItem, Collection, Section)
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

See [src/AGENTS.md](../src/AGENTS.md) for general .NET development patterns and architecture guidelines.

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
- **`javascript/`** - Vitest tests for client-side JavaScript
  - Tests all JS files in `src/TechHub.Web/wwwroot/js/`
  - See [tests/javascript/AGENTS.md](../tests/javascript/AGENTS.md)

### Test Utilities & Test Collections

- **`TechHub.TestUtilities/`** - Shared test infrastructure
  - **`TestCollections/`** - Sample file-based fixtures for test seeding:
    - `_blogs/` - Sample blog posts
    - `_community/` - Sample community posts
    - `_custom/` - Sample custom pages
    - `_news/` - Sample news items
    - `_roundups/` - Sample roundup posts
    - `_videos/` - Sample video content
  - `TestCollectionsSeeder.cs` - Seeds test database from TestCollections
  - See [tests/TechHub.TestUtilities/AGENTS.md](../tests/TechHub.TestUtilities/AGENTS.md)

See [tests/AGENTS.md](../tests/AGENTS.md) for testing strategies, TDD workflow, and Testing Diamond pattern.

## Configuration & Documentation

### Critical Configuration Files

- **`AGENTS.md`** - Root AI assistant workflow (8-step development process) - **READ THIS FIRST**
- **`appsettings*.json`** - Configuration lives per project, with environment variants (Development, Production, and so on):
  - `src/TechHub.Api/` - Section and collection definitions, database, content processing, and other API settings
  - `src/TechHub.Web/` - MCP, subdomain shortcuts, primary hosts, and `AzureAd` settings
  - `src/TechHub.AppHost/` - Aspire orchestration settings
- **`Directory.Build.props`** - MSBuild properties for all projects
- **`Directory.Packages.props`** - Central NuGet package version management
- **`global.json`** - Pins the .NET SDK version and selects the Microsoft.Testing.Platform test runner
- **`TechHub.slnx`** - Solution file (XML-based .NET 10+ format)
- **`docker-compose.yml`** - Local PostgreSQL, API, Web, and Aspire Dashboard containers
- **`package.json`** - Node.js tooling: Vitest and jsdom (JavaScript tests), Husky (git hooks), markdownlint-cli2, and `@playwright/test`
- **`vitest.config.js`** - Vitest configuration for the JavaScript tests
- **`.husky/pre-commit`** - Git hook that verifies `dotnet format` on staged C# files and runs markdownlint on staged Markdown files
- **`.editorconfig`** - Code style and formatting rules
- **`.markdownlint-cli2.jsonc`** - Markdown lint rules
- **`.gitattributes`** - Git line ending and attribute rules
- **`CODEOWNERS`** - Code review ownership

### Documentation (`docs/`)

Comprehensive functional and technical documentation.

- **`architecture.md`** - System architecture overview
- **`technology-stack.md`** - Technologies used and rationale
- **`repository-structure.md`** - This file
- **`running-and-testing.md`** - How to build, run, and test the application
- **`testing-strategy.md`** - Testing approach and patterns
- **Additional docs:** caching, content-api, database, design-system, filtering, health-checks, render-modes, RSS feeds, SEO, and more

See [docs/AGENTS.md](AGENTS.md) for documentation maintenance guidelines.

### Automation & Scripts (`scripts/`)

PowerShell automation scripts for development, deployment, and maintenance. All content processing lives in the C# background services, not in scripts.

#### Development and local data

- **`TechHubRunner.psm1`** - **PRIMARY BUILD/RUN/TEST MODULE** - exports `Run`, `Stop-Servers`, `Stop-OrphanedTestProcesses`, and `Start-McpInspector`
  - `Run` - Build, test, and start servers (use this for all build/test operations)
  - `Run -WithoutTests` - Start servers without running tests
  - `Run -TestProject <name>` - Run specific test project
- **`Generate-DevCertificate.ps1`** - Creates HTTPS dev certificates
- **`Normalize-Images.ps1`** - Optimizes and normalizes images
- **`Restore-Database.ps1`** - Dumps the production PostgreSQL database and restores it locally
  - Target: `local` (restore into Docker Compose PostgreSQL) or `disk` (dump to `.tmp/` only)
  - Populates data in non-production environments
  - Requires PostgreSQL client tools and network access to the production database
- **`Invoke-Sql.ps1`** - Runs a single SQL query against the local PostgreSQL container
- **`Setup-UserSecrets.ps1`** - Fills .NET user secrets for local development from Azure and the localhost Entra ID app registration
- **`Fetch-VideoTranscripts.ps1`** - Backfills video transcripts with yt-dlp through the admin API

#### Build and deploy

- **`Build-Images.ps1`** - Builds the API and Web Docker images and pushes them to `ghcr.io`
- **`Deploy-Infrastructure.ps1`** - Phase 1 Azure infrastructure deployment (Bicep)
  - Deploys networking, identity, Key Vault, PostgreSQL, AI Foundry, ACS, monitoring, and governance
  - Modes: validate, whatif, deploy. `-LinkEmailDomain` links the verified ACS email domain
  - Resolves `ADMIN_IP_ADDRESSES` from the GitHub variable and `POSTGRES_ADMIN_PASSWORD` from Key Vault when not set
- **`Deploy-Applications.ps1`** - Phase 2 App Service deployment (Bicep, fully standalone)
  - Requires `-ImageTag`; no infrastructure output dependencies
  - Modes: validate, whatif, deploy
- **`Deploy-Application.ps1`** - Fast image-only update via `az webapp config set`
  - Smoke tests and automatic rollback
  - Requires `-Tag`; used for code-only deploys (skips full Bicep evaluation)
- **`Deploy-PrPreview.ps1`** - Deploys or tears down a PR preview environment (`deploy` or `teardown`)
- **`Teardown-PrEnvironments.ps1`** - Tears down all active PR preview environments (used by the nightly workflow)
- **`Wait-ForLiveVersion.ps1`** - Post-deploy gate that polls the Web `/version` endpoint, then runs smoke tests
- **`Add-AppInsightsAnnotation.ps1`** - Creates an Application Insights release annotation after a production deploy

#### Identity, secrets, and certificates

- **`Manage-EntraId.ps1`** - Creates or rotates the Entra ID app registration for localhost, staging, or production
- **`Assign-AdminUsers.ps1`** - Assigns admin users to the Entra ID enterprise app
- **`Setup-OidcAuthentication.ps1`** - Configures GitHub Actions OIDC federated credentials and repository variables
- **`Sync-KeyVaultSecrets.ps1`** - Writes application secrets from environment variables to Key Vault
- **`Renew-WildcardCertificates.ps1`** - Renews the Let's Encrypt wildcard certificates and imports them into Key Vault and App Service
- **`Migrate-KeyVaultSecrets.ps1`** - Legacy one-off copy from `kv-techhub-shared` to `kv-techhub-prod`

See [scripts/AGENTS.md](../scripts/AGENTS.md) for scripting guidelines and patterns.

### Infrastructure (`infra/`)

Azure infrastructure as code (Bicep templates) for Azure App Service deployment:

- **`infrastructure.bicep`** - Phase 1: base infrastructure (identity, networking, KV, PostgreSQL, AI Foundry, ACS, monitoring, governance, and the production App Service Plan)
- **`applications.bicep`** - Phase 2: production API + Web App Service site deployments (Web Apps for Containers)
- **`pr-applications.bicep`** - PR preview API + Web App Service site deployments (shared PR App Service Plan, telemetry disabled)
- **`parameters/`** - Environment-specific parameter files:
  - `prod-infrastructure.bicepparam` - Production infrastructure parameters
  - `prod-applications.bicepparam` - Production application deployment parameters
- **`modules/`** - Reusable Bicep modules:
  - `appServicePlan.bicep` - App Service Plan (Basic B1, shared by API + Web sites)
  - `api.bicep` - TechHub API App Service site (Web App for Containers, VNet-restricted, not publicly accessible)
  - `web.bicep` - TechHub Web App Service site (Web App for Containers)
  - `monitoring.bicep` - Application Insights + Log Analytics workspace (the Log Analytics workspace is defined here)
  - `postgres.bicep` - PostgreSQL Flexible Server
  - `keyVault.bicep` - Key Vault
  - `keyVaultSecret.bicep` - Writes a single Key Vault secret
  - `identity.bicep` - User-assigned managed identity
  - `network.bicep` - VNet, subnets, private DNS zones
  - `openai.bicep` - AI Foundry (OpenAI) account, model deployment, and private endpoint
  - `communication.bicep` - Azure Communication Services and the custom email domain used for newsletters
  - `acsDataContributorRole.bicep`, `kvSecretsUserRole.bicep`, `openAiUserRole.bicep` - RBAC role assignments for managed identities
  - `acmeDnsZone.bicep` - Azure DNS zone used for ACME DNS-01 challenge delegation
  - `actionGroup.bicep` and `alerts.bicep` - Alert notification target and alert rules
  - `budget.bicep` and `policy.bicep` - Cost budget and Azure Policy assignments
  - `wildcardCert.bicep` - Imports a wildcard TLS certificate from Key Vault into an App Service Plan (deployed standalone by `Renew-WildcardCertificates.ps1`)
  - `logAnalytics.bicep` - Not referenced by any template (orphaned; `monitoring.bicep` creates its own workspace)

Compiled ARM templates (`infra/**/*.json`) are gitignored build output.

Deployment is managed via PowerShell scripts (`Deploy-Infrastructure.ps1`, `Deploy-Applications.ps1`, `Deploy-Application.ps1`, and `Deploy-PrPreview.ps1`) which are called by GitHub Actions workflows. Scripts can also be run locally for testing. See [infra/AGENTS.md](../infra/AGENTS.md) for the deployment rules.

## Development Environment & Working Directories

### `.devcontainer/`

Dev container configuration for VS Code and GitHub Codespaces:

- **`devcontainer.json`** - Container configuration, extensions, settings
- **`devcontainer-lock.json`** - Pinned versions of dev container features
- **`post-create.sh`** - Post-creation setup script. Add ALL dependencies needed for the devcontainer here (including certbot)
- **`oh-my-posh.json`** - Shell prompt theme

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

- **`.github/`** - GitHub Actions workflows, dependency automation, and AI assistant configuration
  - `workflows/ci.yml` - CI pipeline: quality checks run on pull requests, PR preview build/deploy/E2E run on manual dispatch only, and preview teardown runs automatically when a PR closes
  - `workflows/cd.yml` - CD pipeline: runs on pushes to `main` (CI + production deployment)
  - `workflows/dependency-review.yml` - Dependency review on pull requests
  - `workflows/pr-env-nightly-teardown.yml` - Nightly teardown of leftover PR preview environments
  - `dependabot.yml` - Weekly dependency updates
  - `codeql/codeql-config.yml` - CodeQL code scanning configuration
  - `copilot-instructions.md`, `prompts/`, and `skills/` (`cleanup`, `pushall`) - AI assistant instructions, prompts, and skills
  - See [ci-cd-pipeline.md](ci-cd-pipeline.md) for complete CI/CD documentation
- **`.husky/`** - Git hooks (pre-commit formatting and Markdown lint checks)
- **`.vscode/`** - VS Code workspace configuration
  - `launch.json` - Launch configurations: "Tech Hub (Development)", "Tech Hub (Attach to running)", and "Tech Hub (Production)"
  - `tasks.json` - Build, test, and run tasks
  - `settings.json` - Workspace settings
  - `mcp.json` (gitignored, copy from `mcp.example.json`) and `playwright-mcp-config.json` - MCP server configuration
- **`.git/`** - Git repository metadata
- **`.playwright-mcp/`** - Playwright MCP output (console logs, page snapshots, screenshots). Gitignored
- **`.dockerignore`** - Files to exclude from Docker builds
