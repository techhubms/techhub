# Research: Tech Hub .NET Migration

**Date**: 2026-01-02  
**Purpose**: Validate technology stack and resolve any remaining clarifications

## Research Summary

All major technology decisions are well-defined in the feature specification. This research validates those choices and provides additional implementation guidance.

## Technology Validations

### .NET 10 + Blazor Architecture

**Decision**: Use .NET 10 (latest LTS) with Blazor SSR + WASM hybrid rendering  
**Rationale**:

- .NET 10 is the latest LTS release with long-term support (until 2027+)
- Blazor unified rendering model in .NET 8+ allows SSR for initial load (SEO, performance) and auto-upgrade to WebAssembly for client-side interactivity
- Single codebase for server and client code reduces complexity
- Built-in support for progressive enhancement (works without JavaScript)

**Alternatives Considered**:

- **React + .NET API**: Rejected - requires separate frontend tooling, no C# code sharing, more complex deployment
- **Blazor Server only**: Rejected - requires persistent SignalR connections, not suitable for public website with variable traffic
- **Static Site Generator (Jekyll, Hugo)**: Rejected - limited client-side interactivity, awkward filtering implementation

**Implementation Notes**:

- Use `@rendermode InteractiveAuto` for components requiring client-side interactivity (filters, infinite scroll)
- Use SSR-only rendering for static content (articles, section headers, footers)
- Share models and validation logic between API and Web projects via TechHub.Core

---

### Separate API + Frontend Architecture

**Decision**: Split into TechHub.Api (backend) and TechHub.Web (frontend) with typed HTTP client  
**Rationale**:

- Clear separation of concerns (data access vs. presentation)
- API can be consumed by other clients (mobile apps, CLI tools, future enhancements)
- Easier to scale independently (API can handle more instances than Web)
- Better testability (API integration tests, component tests separate)

**Alternatives Considered**:

- **Monolithic Blazor app with direct data access**: Rejected - tighter coupling, harder to scale, API required for future use cases
- **Blazor WASM calling API directly**: Rejected - loses SSR benefits for SEO and initial load performance

**Implementation Notes**:

- Use IHttpClientFactory for typed API client in TechHub.Web
- Share DTOs between projects via TechHub.Core
- Implement Polly retry policies in API client for resilience

---

### File-Based Storage with Repository Pattern

**Decision**: Start with file-based markdown storage, use repository pattern for future database migration  
**Rationale**:

- Current content volume (~1000 items) works efficiently in memory
- Repository pattern provides abstraction - can swap to database when needed (estimated at ~1000+ items or when query complexity demands it)
- Maintains Git-based content workflow familiar to current editors
- Avoids premature optimization (database infrastructure not needed yet)

**Alternatives Considered**:

- **Database from start**: Rejected - adds complexity, hosting cost, migration overhead without current benefit
- **Direct file access without repository pattern**: Rejected - makes future database migration harder

**Implementation Notes**:

- Load all content into IMemoryCache at startup
- Use sliding expiration (30min) for cache entries
- Clear cache on container restart (acceptable for Git-based updates)
- Repository interface ready for future IContentRepository implementations (SQL, CosmosDB, etc.)

---

### In-Memory Caching Strategy

**Decision**: Use IMemoryCache for content caching with sliding/absolute expiration  
**Rationale**:

- Eliminates database queries for every request
- Fast lookups for filtering and search operations
- Acceptable invalidation strategy (cache clears on container restart after Git push)
- No distributed cache needed for current scale

**Alternatives Considered**:

- **No caching**: Rejected - file I/O on every request unacceptable for performance
- **Redis distributed cache**: Rejected - adds infrastructure complexity, not needed for current traffic (<10k concurrent users)
- **Output caching only**: Rejected - insufficient for dynamic filtering scenarios

**Implementation Notes**:

- Cache entire content collection at startup
- Absolute expiration: 1 hour for API responses
- Sliding expiration: 30 minutes for content items
- Output caching for static pages (home, about)

---

### Client-Side Filtering Implementation

**Decision**: Load all filtered content server-side, filter client-side with JavaScript  
**Rationale**:

- Instant filtering without server roundtrips
- URL state management for shareable filtered views
- Works with browser back/forward navigation
- Supports complex multi-filter scenarios (tags + date + search)

**Alternatives Considered**:

- **Server-side filtering with page reloads**: Rejected - slow UX, breaks flow
- **Server-side filtering with AJAX**: Rejected - still requires roundtrips, adds latency

**Implementation Notes**:

- Use Blazor WASM for filter state management
- Debounce text search input (300ms)
- Sync filter state to URL query parameters
- Restore state from URL on page load

---

### Testing Stack

**Decision**: xUnit (unit), WebApplicationFactory (integration), bUnit (component), Playwright (E2E)  
**Rationale**:

- xUnit is standard .NET testing framework with excellent tooling
- WebApplicationFactory provides in-memory API testing without external dependencies
- bUnit designed specifically for Blazor component testing
- Playwright provides cross-browser E2E testing with excellent developer experience

**Alternatives Considered**:

- **NUnit/MSTest**: Rejected - xUnit is more modern, better async support
- **Selenium**: Rejected - Playwright has better API, faster, more reliable
- **Manual testing only**: Rejected - spec requires 80% code coverage

**Implementation Notes**:

- Organize tests by project (TechHub.Core.Tests, TechHub.Api.Tests, etc.)
- Use test fixtures for shared setup (content loading, test data)
- Run E2E tests in CI/CD pipeline before deployment

---

### Azure Container Apps Deployment

**Decision**: Deploy to Azure Container Apps with auto-scaling (1-10 instances)  
**Rationale**:

- Managed container platform with auto-scaling
- Built-in HTTPS with managed certificates
- Pay-per-use pricing suitable for variable traffic
- Integrates with Application Insights for monitoring

**Alternatives Considered**:

- **Azure App Service**: Rejected - more expensive for variable traffic, less flexible scaling
- **Azure Kubernetes Service (AKS)**: Rejected - over-engineered for current needs, higher management overhead
- **Static Web Apps**: Rejected - not suitable for Blazor SSR + API architecture

**Implementation Notes**:

- Use .NET Aspire for local development orchestration
- Scale on HTTP requests + CPU (80% threshold)
- Min 1 instance (always-on), max 10 instances
- Use Bicep for infrastructure as code

---

### OpenTelemetry + Application Insights

**Decision**: Use OpenTelemetry for distributed tracing with Application Insights backend  
**Rationale**:

- Industry-standard observability (vendor-neutral)
- Traces HTTP requests, repository operations, cache hits/misses
- Adaptive sampling (100% errors, 10% success) balances observability with cost
- Application Insights provides rich Azure integration

**Alternatives Considered**:

- **Application Insights SDK only**: Rejected - locks into Azure, OpenTelemetry is standard
- **No tracing**: Rejected - spec requires monitoring and alerting

**Implementation Notes**:

- Configure OpenTelemetry in Program.cs
- Add custom instrumentation for repository operations
- Set up alerts for error rate >0.1%, response time >200ms

---

## Unknowns Resolved

All technical unknowns from the spec have been resolved:

1. ✅ **Blazor deployment model** - SSR + WASM hybrid using .NET 8+ unified rendering
2. ✅ **Data persistence** - File-based with repository pattern for future database migration
3. ✅ **Caching strategy** - IMemoryCache with sliding/absolute expiration, cleared on restart
4. ✅ **Auto-scaling** - Azure Container Apps 1-10 instances, HTTP + CPU triggers
5. ✅ **Tracing scope** - OpenTelemetry with Application Insights, adaptive sampling

## Best Practices Applied

### .NET/Blazor Best Practices

- Use minimal APIs for lightweight RESTful endpoints
- Implement health checks for container orchestration
- Use IOptions pattern for configuration management
- Follow dependency injection best practices
- Use async/await throughout for I/O operations
- Implement proper logging with ILogger<T>

### Performance Best Practices

- Enable response compression (Brotli/Gzip)
- Implement output caching for static content
- Use lazy loading for images
- Minimize JavaScript bundle size
- Enable HTTP/2 and HTTP/3 support
- Use CDN for static assets (future enhancement)

### Security Best Practices

- Use Azure Key Vault for secrets (API keys, connection strings)
- Implement Content Security Policy headers
- Enable HSTS (HTTP Strict Transport Security)
- Configure CORS properly (whitelist only)
- Sanitize markdown rendering to prevent XSS
- Use parameterized queries (no SQL injection risk)

### Accessibility Best Practices

- Use semantic HTML5 elements
- Implement skip-to-content links
- Ensure 4.5:1 color contrast ratios
- Provide alt text for all images
- Support keyboard navigation
- Test with screen readers (NVDA, JAWS)

---

## Implementation Sequence

Based on spec dependencies, implement in this order:

### Phase 1: Foundation (Specs 001-003, 011-013)

1. 001-solution-structure - Create .NET solution and projects
2. 002-configuration-management - Set up appsettings.json and environment config
3. 003-resilience-error-handling - Configure Polly, logging, error handling
4. 011-domain-models - Define DTOs and domain entities
5. 012-repository-pattern - Implement file-based repositories
6. 013-api-endpoints - Create REST API with minimal endpoints

### Phase 2: Testing Infrastructure (Specs 004-007)

1. 004-unit-testing - Set up xUnit for domain/services
2. 005-integration-testing - Set up WebApplicationFactory for API
3. 006-component-testing - Set up bUnit for Blazor components
4. 007-e2e-testing - Set up Playwright for end-to-end tests

### Phase 3: Core Frontend (Specs 008-012)

1. 008-api-client - Create typed HttpClient for Blazor
2. 009-url-routing - Define routing and URL structure
3. 010-section-system - Implement section/collection architecture
4. 014-blazor-components - Build reusable UI components
5. 017-page-components - Create page-level components

### Phase 4: Content & Features (Specs 013-020)

1. 015-nlweb-semantic-html - Implement semantic markup
2. 016-visual-design-system - Apply design tokens and styling
3. 018-content-rendering - Markdown parsing and rendering
4. 019-filtering-system - Client-side filtering logic
5. 020-infinite-scroll - Progressive loading implementation

### Phase 5: Supporting Features (Specs 021-024)

1. 021-rss-feeds - RSS feed generation
2. 022-search - Text search implementation
3. 023-seo - SEO optimization
4. 024-google-analytics - GA4 integration

### Phase 6: Infrastructure & Deployment (Specs 025-026)

1. 025-azure-resources - Provision Azure resources
2. 026-ci-cd-pipeline - Set up GitHub Actions

---

## Next Steps

1. Proceed to **Phase 1: Design & Contracts**
   - Generate data-model.md with entity definitions
   - Create API contracts in /contracts/ directory
   - Generate quickstart.md for developer onboarding
   - Update agent context with new technology decisions

2. After Phase 1 completes:
   - Re-validate Constitution Check with detailed design
   - Generate tasks.md with implementation checklist
   - Begin implementation following spec-driven development

**Status**: ✅ Research complete - All technology choices validated and documented
