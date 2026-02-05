# Repository Structure

## Source Code (`src/`)

- `TechHub.Api/` - REST API backend (Minimal API, OpenAPI/Swagger)
- `TechHub.Web/` - Blazor frontend (SSR + WebAssembly)
- `TechHub.Core/` - Domain models, interfaces
- `TechHub.Infrastructure/` - Repository implementations, services
- `TechHub.AppHost/` - .NET Aspire orchestration
- `TechHub.ServiceDefaults/` - Shared .NET Aspire service configuration
- `TechHub.ContentFixer/` - CLI utility for content maintenance and frontmatter updates

## Content (`collections/`)

- `_news/` - Official announcements and product updates
- `_videos/` - Video content and tutorials
- `_community/` - Microsoft Tech Community posts
- `_custom/` - Custom manually created pages
- `_blogs/` - Technical articles and blogs
- `_roundups/` - Curated weekly summaries

## Tests (`tests/`)

- `TechHub.Core.Tests/` - Unit tests for domain logic
- `TechHub.Api.Tests/` - Integration tests for API endpoints
- `TechHub.Web.Tests/` - bUnit component tests
- `TechHub.E2E.Tests/` - Playwright end-to-end tests
- `powershell/` - PowerShell script tests

## Configuration & Documentation

- `appsettings.json` - Application configuration (sections, collections, service settings)
- `docs/` - Functional documentation
- `scripts/` - Automation and utility scripts (PowerShell)
- `infra/` - Azure infrastructure (Bicep templates)
- `specs/` - Feature specifications
