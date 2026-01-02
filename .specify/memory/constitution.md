<!--
SYNC IMPACT REPORT - Constitution Amendment (2026-01-02)
=========================================================

VERSION CHANGE: 1.1.0 → 1.2.0 (MINOR bump)

REASON FOR MINOR BUMP:
- Removed backwards compatibility constraints
- Clarified this is a greenfield-style optimal solution
- Removed URL preservation and RSS format requirements
- Removed Jekyll visual replication requirement
- No principles removed, but constraints significantly relaxed for optimal design

MODIFIED SECTIONS:
- Constraints section → Removed backwards compatibility requirements:
  - REMOVED: "Must support RSS feed URLs (content can differ)"
  - REMOVED: "Must modernize visual design (not replicate Jekyll)"
  - REMOVED: "Must support URL redirects from old Jekyll URLs if structure changes"
  - ADDED: "No backwards compatibility required - design optimal modern solution as if greenfield"
  - ADDED: "No requirement to preserve Jekyll URL structure or RSS feed formats"
  - ADDED: "No requirement to replicate Jekyll visual design - create modern optimal UX"

TEMPLATE CONSISTENCY STATUS:
✅ plan-template.md: No template changes needed
✅ spec-template.md: Specs should focus on optimal design, not Jekyll compatibility
✅ tasks-template.md: Quality standards enhanced by freedom to optimize
⚠️ Migration plan should be updated to remove URL redirect requirements

FOLLOW-UP ACTIONS:
✅ Constitution updated with greenfield approach
✅ Version bumped to 1.2.0 (MINOR)
✅ Last Amended date updated to 2026-01-02
⚠️ TODO: Update migration plan to remove backwards compatibility tasks
⚠️ TODO: Review all feature specs to remove Jekyll compatibility constraints

LAST REVIEW: 2026-01-02
NEXT AMENDMENT: When additional principles or constraints need definition
-->

# Tech Hub .NET Migration Constitution

## Project Identity

- **Name**: Tech Hub .NET
- **Purpose**: Modern .NET implementation of Tech Hub content platform
- **Repository**: techhubms/techhub
- **Branch**: dotnet-migration

## Core Principles

1. **Modern UX First**: State-of-the-art user experience using latest .NET web patterns
2. **Configuration-Driven**: sections.json remains single source of truth
3. **Hybrid Rendering**: SSR for SEO + interactive Blazor for dynamic features
4. **Semantic HTML**: Accessible markup with Schema.org structured data
5. **Performance**: Aggressive caching, infinite scroll, lazy loading, minimal JavaScript
6. **Accessibility**: WCAG 2.1 Level AA compliance with modern interaction patterns

## Technology Stack

- **Runtime**: .NET 10 (latest LTS)
- **Web Framework**: Blazor (SSR + WASM for interactivity)
- **Orchestration**: .NET Aspire
- **Markdown**: Markdig with custom extensions
- **Caching**: Output caching, distributed cache (Redis)
- **Search**: Full-text search (Azure AI Search or Elasticsearch)
- **Testing**: xUnit, bUnit, Playwright
- **Infrastructure**: Azure Container Apps, Bicep
- **Monitoring**: OpenTelemetry, Application Insights

## Constraints

- Must not modify Jekyll source (documentation reference only)
- Must use repository pattern (file-based initially, database-ready)
- Must implement progressive enhancement (works without JavaScript)
- No backwards compatibility required - design optimal modern solution as if greenfield
- No requirement to preserve Jekyll URL structure or RSS feed formats
- No requirement to replicate Jekyll visual design - create modern optimal UX

### Directory Structure Requirements

**All .NET implementation code MUST reside in the `dotnet/` directory**, including:

- .NET solution and project files
- Source code (`src/`, `apps/`, etc.)
- .NET-specific tests
- .NET-specific DevContainer configuration (`.devcontainer/`)
- .NET-specific README.md
- .NET-specific AGENTS.md documentation

**Repository-level infrastructure MUST remain in repository root**, including:

- `.github/` - GitHub workflows, actions, and agents
- `.specify/` - Spec-kit templates and memory
- `.vscode/` - VS Code workspace settings
- Other dotfile configurations (`.gitignore`, `.editorconfig`, etc.)
- Root-level AGENTS.md (framework-agnostic principles)

## Development Methodology

This project follows **Spec-Driven Development (SDD)** using the spec-kit methodology:

1. **Constitution** → Define project principles and constraints
2. **Specification** → Document exact behavior before implementation
3. **Planning** → Break work into small, testable tasks
4. **Implementation** → Write code that matches specifications
5. **Verification** → Validate implementation against specs

## Quality Standards

- All code must have unit tests
- All Blazor components must have bUnit tests
- All API endpoints must have integration tests
- All user flows must have E2E tests
- Code coverage minimum: 80%
- All tests must pass before deployment

## Documentation Requirements

- AGENTS.md files for all major directories
- XML documentation comments for public APIs
- README.md for each project
- Inline comments for complex logic only

## Security Requirements

- No secrets in source code or configuration
- All external inputs must be validated
- Use parameterized queries (no SQL injection risk)
- HTTPS only in production
- Content Security Policy headers
- CORS configured properly

## Performance Requirements

- Initial page load (SSR): < 1 second
- Time to interactive: < 2 seconds
- Largest Contentful Paint: < 2.5 seconds
- First Input Delay: < 100ms
- Cumulative Layout Shift: < 0.1
- Lighthouse score: > 95 (all categories)
- API response time: < 50ms (p95, cached)
- Infinite scroll load time: < 500ms per batch
- Build time: < 5 minutes

## Governance

- This constitution supersedes all other practices
- Amendments require documentation and migration plan
- All code reviews must verify constitution compliance
- Specs in `/specs/features/` define authoritative behavior

**Version**: 1.2.0 | **Ratified**: 2026-01-01 | **Last Amended**: 2026-01-02

