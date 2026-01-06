# Tasks: Tech Hub .NET Migration

**Input**: Design documents from `/specs/dotnet-migration/`
**Prerequisites**: [plan.md](plan.md), [spec.md](spec.md), [research.md](research.md), [data-model.md](data-model.md), [contracts/](contracts/)

**Tests**: Comprehensive testing strategy across all layers (unit, integration, component, E2E)

**Organization**: Tasks are grouped by implementation phase following the 6-phase sequence from [research.md](research.md#implementation-sequence)

---

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3, US4, US5, US6, US7)
- Include exact file paths in descriptions

## Path Conventions

All .NET code resides in repository root per constitution requirements:

- Projects: `src/`
- Tests: `tests/`
- Jekyll files: `jekyll/`
- Content files: `collections/` (shared)
- Configuration: `_data/sections.json` (shared)

---

## Phase 1: Foundation (Setup & Core Infrastructure)

**Purpose**: Project initialization, basic structure, and foundational services that block ALL user stories

**CRITICAL**: This phase MUST complete before any user story implementation begins

### Project Setup

- [X] T001 Create .NET solution structure per [research.md](research.md#implementation-sequence) Phase 1
- [X] T002 Initialize TechHub.Core class library in src/TechHub.Core/TechHub.Core.csproj
- [X] T003 [P] Initialize TechHub.Infrastructure class library in src/TechHub.Infrastructure/TechHub.Infrastructure.csproj
- [X] T004 [P] Initialize TechHub.Api web API project in src/TechHub.Api/TechHub.Api.csproj
- [X] T005 [P] Initialize TechHub.Web Blazor project in src/TechHub.Web/TechHub.Web.csproj
- [X] T006 [P] Initialize TechHub.AppHost .NET Aspire project in src/TechHub.AppHost/TechHub.AppHost.csproj

### Testing Infrastructure

- [X] T007 [P] Initialize TechHub.Core.Tests xUnit project in tests/TechHub.Core.Tests/TechHub.Core.Tests.csproj
- [X] T008 [P] Initialize TechHub.Infrastructure.Tests xUnit project in tests/TechHub.Infrastructure.Tests/TechHub.Infrastructure.Tests.csproj
- [X] T009 [P] Initialize TechHub.Api.Tests xUnit project in tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj
- [X] T010 [P] Initialize TechHub.Web.Tests bUnit project in tests/TechHub.Web.Tests/TechHub.Web.Tests.csproj
- [X] T011 [P] Initialize TechHub.E2E.Tests Playwright project in tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj

### Configuration Management

- [X] T012 Add configuration classes in src/TechHub.Core/Configuration/AppSettings.cs
- [X] T013 [P] Create appsettings.json for TechHub.Api in src/TechHub.Api/appsettings.json
- [X] T014 [P] Create appsettings.Development.json for TechHub.Api in src/TechHub.Api/appsettings.Development.json
- [X] T015 [P] Create appsettings.json for TechHub.Web in src/TechHub.Web/appsettings.json
- [X] T016 [P] Create appsettings.Development.json for TechHub.Web in src/TechHub.Web/appsettings.Development.json

### Resilience & Error Handling

- [X] T017 Configure Polly retry policies in src/TechHub.Infrastructure/Resilience/PollyPolicies.cs
- [X] T018 [P] Implement global exception handler middleware in src/TechHub.Api/Middleware/ExceptionHandlerMiddleware.cs
- [X] T019 [P] ~~Configure structured logging with Serilog~~ SKIPPED - Using ASP.NET Core default logging (see docs/decisions/no-serilog.md)
- [X] T020 [P] Configure OpenTelemetry tracing in src/TechHub.Api/Extensions/TelemetryExtensions.cs

### Domain Models & Interfaces

- [X] T021 [P] Create Section entity in src/TechHub.Core/Models/Section.cs
- [X] T022 [P] Create CollectionReference entity in src/TechHub.Core/Models/CollectionReference.cs
- [X] T023 [P] Create ContentItem entity in src/TechHub.Core/Models/ContentItem.cs
- [X] T024 [P] Create FilterState entity in src/TechHub.Core/Models/FilterState.cs
- [X] T025 [P] Create SectionDto in src/TechHub.Core/DTOs/SectionDto.cs
- [X] T026 [P] Create CollectionReferenceDto in src/TechHub.Core/DTOs/CollectionReferenceDto.cs
- [X] T027 [P] Create ContentItemDto in src/TechHub.Core/DTOs/ContentItemDto.cs
- [X] T028 [P] Create ContentItemDetailDto in src/TechHub.Core/DTOs/ContentItemDetailDto.cs
- [X] T029 [P] Create PagedResultDto in src/TechHub.Core/DTOs/PagedResultDto.cs
- [X] T030 [P] Create RssChannelDto and RssItemDto in src/TechHub.Core/DTOs/RssDtos.cs

### Repository Pattern Interfaces

- [X] T031 Define IContentRepository interface in src/TechHub.Core/Interfaces/IContentRepository.cs
- [X] T032 [P] Define ISectionRepository interface in src/TechHub.Core/Interfaces/ISectionRepository.cs
- [X] T033 [P] Define IRssService interface in src/TechHub.Core/Interfaces/IRssService.cs
- [X] T034 [P] Define IMarkdownService interface in src/TechHub.Core/Interfaces/IMarkdownService.cs

### Extension Methods

- [X] T035 [P] Implement ContentItem extension methods (ToDto, ToDetailDto) in src/TechHub.Core/Extensions/ContentItemExtensions.cs
- [X] T036 [P] Implement Section extension methods (ToDto) in src/TechHub.Core/Extensions/SectionExtensions.cs

**Checkpoint**: Foundation ready - domain models, DTOs, interfaces, configuration, and testing infrastructure complete

---

## Phase 2: Data Access & Core Services

**Purpose**: Implement repositories, markdown parsing, RSS generation, and caching

### File-Based Repositories

- [X] T037 Implement FileBasedContentRepository in src/TechHub.Infrastructure/Repositories/FileBasedContentRepository.cs
- [X] T038 Implement FileBasedSectionRepository in src/TechHub.Infrastructure/Repositories/FileBasedSectionRepository.cs
- [X] T039 Add YAML frontmatter parser in src/TechHub.Infrastructure/Services/FrontMatterParser.cs

### Markdown Processing

- [X] T040 Implement MarkdownService with Markdig in src/TechHub.Infrastructure/Services/MarkdownService.cs
- [X] T041 [P] Add excerpt extraction logic (<!--excerpt_end--> marker) in MarkdownService
- [X] T042 [P] Add YouTube embed processing in MarkdownService

### RSS Feed Generation

- [X] T043 Implement RssService for feed generation in src/TechHub.Infrastructure/Services/RssService.cs
- [X] T044 ~~Add RSS XML serialization helpers in src/TechHub.Infrastructure/Services/RssXmlHelpers.cs~~ Integrated into RssService.SerializeToXml()

### Caching Strategy

- [ ] T045 Configure IMemoryCache in src/TechHub.Infrastructure/Caching/CacheConfiguration.cs
- [ ] T046 [P] Implement cache keys and expiration policies in src/TechHub.Infrastructure/Caching/CacheKeys.cs
- [ ] T047 [P] Add cache warming on application startup in src/TechHub.Api/HostedServices/CacheWarmingService.cs

### Unit Tests for Core Services

- [X] T048 [P] Unit tests for Section entity validation in tests/TechHub.Core.Tests/Models/SectionTests.cs (already existed)
- [X] T049 [P] Unit tests for ContentItem entity validation in tests/TechHub.Core.Tests/Models/ContentItemTests.cs (already existed)
- [X] T050 [P] Unit tests for extension methods in tests/TechHub.Core.Tests/Extensions/ContentItemExtensionsTests.cs (already existed)
- [X] T051 [P] Unit tests for MarkdownService in tests/TechHub.Infrastructure.Tests/Services/MarkdownServiceTests.cs (already existed)
- [X] T052 [P] Unit tests for RssService in tests/TechHub.Infrastructure.Tests/Services/RssServiceTests.cs
- [X] T053 [P] Unit tests for FileBasedContentRepository in tests/TechHub.Infrastructure.Tests/Repositories/FileBasedContentRepositoryTests.cs (already existed)

**Checkpoint**: Data access layer complete - repositories can load content from markdown files, parse YAML, render markdown, and generate RSS feeds

---

## Phase 3: User Story 1 - Content Consumer Experience (P1) 🎯 MVP

**Goal**: Users can browse sections, view collections, and read individual content items with responsive design and accessibility

**Independent Test**: Navigate to home page, click a section (e.g., /github-copilot), select a collection (e.g., News), open an article, verify responsive layout and screen reader support

### API Endpoints for User Story 1

- [X] T054 [P] [US1] Implement GET /api/sections endpoint in src/TechHub.Api/Endpoints/SectionsEndpoints.cs
- [X] T055 [P] [US1] Implement GET /api/sections/{sectionName} endpoint in src/TechHub.Api/Endpoints/SectionsEndpoints.cs
- [X] T056 [P] [US1] Implement GET /api/content (filter) endpoint in src/TechHub.Api/Endpoints/ContentEndpoints.cs
- [X] T057 [US1] Configure dependency injection for repositories and services in src/TechHub.Api/Program.cs
- [X] T058 [US1] Configure CORS policy for TechHub.Web in src/TechHub.Api/Program.cs

### Blazor Components for User Story 1

- [X] T060 [P] [US1] Create SectionCard component in src/TechHub.Web/Components/SectionCard.razor
- [X] T060 [P] [US1] Create CollectionList component in src/TechHub.Web/Components/CollectionList.razor
- [X] T061 [P] [US1] Create ContentItemCard component in src/TechHub.Web/Components/ContentItemCard.razor
- [X] T062 [P] [US1] Create ContentDetail component in src/TechHub.Web/Components/ContentDetail.razor
- [X] T063 [P] [US1] Create YouTubeEmbed component in src/TechHub.Web/Components/YouTubeEmbed.razor

### Pages for User Story 1

- [X] T064 [US1] Create Home page in src/TechHub.Web/Pages/Home.razor (with @rendermode InteractiveServer for retry button)
- [X] T065 [US1] Create Section page (collections + items) in src/TechHub.Web/Pages/Section.razor (with @rendermode InteractiveServer and URL routing)
  - Dual route support: /{sectionName} and /{sectionName}/{collectionName}
  - SelectCollection method navigates to /section/collection URLs
  - OnParametersSetAsync detects URL changes and reloads content
  - "All" button in collection sidebar
  - Support for /all/all (everything), /section/all (all from section), /all/collection (collection across sections)
- [X] T066 [US1] Create Content detail page in src/TechHub.Web/Pages/Content.razor

### API Client for User Story 1

- [X] T067 [US1] Create typed HttpClient for API in src/TechHub.Web/Services/TechHubApiClient.cs
- [ ] T068 [US1] Add Polly retry policies to API client in src/TechHub.Web/Services/TechHubApiClient.cs
- [X] T069 [US1] Configure dependency injection for API client in src/TechHub.Web/Program.cs

### Semantic HTML & Accessibility for User Story 1

- [ ] T070 [P] [US1] Implement semantic HTML5 elements (article, section, nav, header, footer, time) in all components
- [ ] T071 [P] [US1] Add ARIA labels and roles for accessibility in all components
- [ ] T072 [P] [US1] Implement keyboard navigation support in all components
- [ ] T073 [US1] Add Schema.org Article markup in ContentDetail component

### Responsive Design for User Story 1

- [ ] T074 [P] [US1] Create responsive layout CSS in src/TechHub.Web/wwwroot/styles.css
  - [X] Import Google Fonts and base styles (completed)
  - [X] Define color variables from Jekyll design system (completed)
  - [X] Implement responsive grid system (completed)
  - [ ] **NEW VISUAL REQUIREMENTS** (see T088-T099 for details):
    - [ ] Add muted purple colors (#7f56d9, #6b4fb8) - FR-047, T088
    - [ ] Add soft text colors (#333 on #fafafa) - FR-048, T089
    - [ ] Add 800px max-width for article content - FR-041, T095
    - [ ] Add two-column layout styles - FR-042, T096
- [ ] T075 [P] [US1] Add mobile breakpoints and grid layout in src/TechHub.Web/wwwroot/css/responsive.css
- [ ] T076 [P] [US1] Implement responsive images with lazy loading in src/TechHub.Web/Components/ContentItemCard.razor

### Integration Tests for User Story 1

- [ ] T077 [P] [US1] Integration test for GET /api/sections in tests/TechHub.Api.Tests/Endpoints/SectionsEndpointsTests.cs
- [ ] T078 [P] [US1] Integration test for GET /api/sections/{sectionName} in tests/TechHub.Api.Tests/Endpoints/SectionsEndpointsTests.cs
- [ ] T079 [P] [US1] Integration test for GET /api/content/{sectionName}/{collection}/{itemId} in tests/TechHub.Api.Tests/Endpoints/ContentEndpointsTests.cs

### Component Tests for User Story 1

- [ ] T080 [P] [US1] bUnit tests for SectionCard component in tests/TechHub.Web.Tests/Components/SectionCardTests.cs
- [ ] T081 [P] [US1] bUnit tests for ContentItemCard component in tests/TechHub.Web.Tests/Components/ContentItemCardTests.cs
- [ ] T082 [P] [US1] bUnit tests for ContentDetail component in tests/TechHub.Web.Tests/Components/ContentDetailTests.cs

### E2E Tests for User Story 1

- [X] T083 [P] [US1] Playwright test for home page navigation in tests/TechHub.E2E.Tests/Tests/NavigationImprovementsTests.cs
- [X] T084 [P] [US1] Playwright test for section browsing in tests/TechHub.E2E.Tests/Tests/NavigationImprovementsTests.cs
- [X] T084.1 [P] [US1] **NEW** Comprehensive URL routing and "all" collection tests in tests/TechHub.E2E.Tests/Tests/UrlRoutingAndNavigationTests.cs
  - Tests URL patterns: /section, /section/collection, /section/all, /all/all, /all/collection
  - Tests collection button clicks update URL correctly
  - Tests browser back/forward navigation
  - Tests "all" collection shows all content from section
  - Tests collection badges display logic (shown on "all", hidden on specific collection)
  - Tests retry button functionality
  - Tests active button states
  - Tests URL sharing and bookmarking
  - 20 comprehensive test cases documenting expected behavior
- [X] T085 [P] [US1] Playwright test for content detail viewing in tests/TechHub.E2E.Tests/Web/ContentDetailTests.cs
  - 11 E2E tests created for content detail page functionality
  - Tests PrimarySection URL routing (e.g., /github-copilot/videos/item)
  - Tests sidebar display, navigation buttons, button spacing
  - 3/11 passing, 8/11 skipped pending internal content (roundups, custom pages in US4)
  - Tests require ViewingMode="internal" content which will be implemented in User Story 4
- [ ] T086 [P] [US1] Playwright test for mobile responsive layout in tests/TechHub.E2E.Tests/Tests/ResponsiveDesignTests.cs
- [ ] T087 [P] [US1] Playwright test for accessibility (keyboard navigation, screen reader) in tests/TechHub.E2E.Tests/Tests/AccessibilityTests.cs

### Visual Design Polish for User Story 1

- [X] T088 [P] [US1] Update styles.css with muted purple color variables (--muted-purple: #6b4fb8, --darker-purple: #7f56d9) in src/TechHub.Web/wwwroot/styles.css
- [X] T089 [P] [US1] Update styles.css with soft text colors (--text-primary: #333, --background-primary: #fafafa) in src/TechHub.Web/wwwroot/styles.css
- [X] T090 [P] [US1] Reduce bright purple backgrounds (<20% of background usage, use muted variants) in src/TechHub.Web/wwwroot/styles.css
- [ ] T091 [P] [US1] Verify WCAG AA color contrast ratios (4.5:1 normal text, 3:1 large/UI) for new color palette using contrast checker tool
- [ ] T092 [P] [US1] Create ArticleSidebar component with 5-priority structure (quick nav, author, metadata, related, share) in src/TechHub.Web/Components/ArticleSidebar.razor
- [ ] T093 [P] [US1] Implement table of contents generation from article headings in ArticleSidebar component
- [ ] T094 [P] [US1] Implement smooth scroll with header offset for TOC links in ArticleSidebar component
- [X] T095 [P] [US1] Create article container with 800px max-width constraint in src/TechHub.Web/wwwroot/styles.css (Implemented in .content-page)
- [ ] T096 [P] [US1] Implement two-column article layout (25-30% sidebar, 70-75% content) in src/TechHub.Web/Components/Pages/Content.razor
- [ ] T097 [P] [US1] Implement responsive mobile sidebar layout (essentials above, supplementary below) in src/TechHub.Web/wwwroot/css/responsive.css
- [X] T098 [P] [US1] Update section card background images with hover effects (brightness increase, smooth transition) in src/TechHub.Web/Components/SectionCard.razor (Already implemented with box-shadow and transform)
- [X] T099 [P] [US1] Verify full-width grid layout for section/collection listing pages (no max-width constraint) in src/TechHub.Web/wwwroot/styles.css (Verified: .grid has no max-width)

**Checkpoint**: User Story 1 complete - users can browse and consume content with full accessibility, responsive design, and polished visual design matching Jekyll quality

---

## Phase 4: User Story 2 - Content Discovery via Filtering (P1)

**Goal**: Users can filter content by date range, tags, and search text with instant client-side filtering and URL state synchronization

**Independent Test**: Navigate to any collection page, apply date filter (Last 30 days), select tags, type search query, verify results update instantly and URL reflects filter state

### Client-Side Filtering Service

- [ ] T100 [US2] Create FilterStateService for Blazor in src/TechHub.Web/Services/FilterStateService.cs
- [ ] T101 [US2] Implement URL query parameter sync in FilterStateService
- [ ] T102 [US2] Add browser history integration (back/forward support) in FilterStateService

### Filter Components

- [ ] T103 [P] [US2] Create DateRangeFilter component in src/TechHub.Web/Components/Filters/DateRangeFilter.razor
- [ ] T104 [P] [US2] Create TagFilter component in src/TechHub.Web/Components/Filters/TagFilter.razor
- [ ] T105 [P] [US2] Create SearchFilter component (debounced input) in src/TechHub.Web/Components/Filters/SearchFilter.razor
- [ ] T106 [US2] Create FilterPanel component (aggregates all filters) in src/TechHub.Web/Components/Filters/FilterPanel.razor

### Filtering Logic

- [ ] T107 [US2] Implement infinite scroll logic with configurable batch sizes (30-50 items) in src/TechHub.Web/Services/ContentFilterService.cs
- [ ] T108 [P] [US2] Implement tag filtering logic (OR logic) in src/TechHub.Web/Services/ContentFilterService.cs
- [ ] T109 [P] [US2] Implement text search filtering (title, description, tags) in src/TechHub.Web/Services/ContentFilterService.cs
- [ ] T110 [US2] Integrate filtering with Section page component in src/TechHub.Web/Pages/Section.razor

### Component Tests for User Story 2

- [ ] T111 [P] [US2] bUnit tests for DateRangeFilter component in tests/TechHub.Web.Tests/Components/Filters/DateRangeFilterTests.cs
- [ ] T112 [P] [US2] bUnit tests for TagFilter component in tests/TechHub.Web.Tests/Components/Filters/TagFilterTests.cs
- [ ] T113 [P] [US2] bUnit tests for SearchFilter component in tests/TechHub.Web.Tests/Components/Filters/SearchFilterTests.cs
- [ ] T114 [US2] Unit tests for ContentFilterService in tests/TechHub.Web.Tests/Services/ContentFilterServiceTests.cs

### E2E Tests for User Story 2

- [ ] T115 [P] [US2] Playwright test for date filtering in tests/TechHub.E2E.Tests/Tests/FilteringTests.cs
- [ ] T116 [P] [US2] Playwright test for tag filtering in tests/TechHub.E2E.Tests/Tests/FilteringTests.cs
- [ ] T117 [P] [US2] Playwright test for text search in tests/TechHub.E2E.Tests/Tests/FilteringTests.cs
- [ ] T118 [US2] Playwright test for URL state synchronization (copy URL, paste in new tab) in tests/TechHub.E2E.Tests/Tests/FilteringTests.cs
- [ ] T119 [US2] Playwright test for browser back/forward navigation in tests/TechHub.E2E.Tests/Tests/FilteringTests.cs

**Checkpoint**: User Story 2 complete - users can filter content interactively with instant updates and shareable URLs

---

## Phase 5: User Story 6 - Performance and Responsiveness (P1)

**Goal**: Pages load instantly, interactions feel immediate, Lighthouse Performance >95, Core Web Vitals meet targets

**Independent Test**: Run Lighthouse on all page types, verify LCP <2.5s, FID <100ms, CLS <0.1, filter operations <100ms

### Performance Optimizations

- [ ] T108 [P] [US6] Enable response compression (Brotli/Gzip) in src/TechHub.Api/Program.cs
- [ ] T109 [P] [US6] Configure output caching for static content in src/TechHub.Api/Program.cs
- [ ] T110 [P] [US6] Add HTTP/2 and HTTP/3 support in src/TechHub.Api/Program.cs
- [ ] T111 [US6] Implement IMemoryCache with sliding/absolute expiration in src/TechHub.Api/Program.cs

### Blazor Performance

- [ ] T112 [US6] Configure Blazor SSR for initial render in src/TechHub.Web/Program.cs
- [ ] T113 [US6] Configure Blazor WASM auto-upgrade for interactivity in src/TechHub.Web/Program.cs
- [ ] T114 [P] [US6] Use @rendermode InteractiveAuto for filter components in src/TechHub.Web/Components/Filters/
- [ ] T115 [P] [US6] Use SSR-only rendering for static content components in src/TechHub.Web/Components/

### Image Optimization

- [ ] T116 [P] [US6] Implement lazy loading for images below the fold in src/TechHub.Web/Components/ContentItemCard.razor
- [ ] T117 [P] [US6] Add responsive image sizes with srcset in src/TechHub.Web/Components/ContentItemCard.razor

### Performance Tests

- [ ] T118 [P] [US6] Lighthouse performance test (score >95) in tests/TechHub.E2E.Tests/Tests/PerformanceTests.cs
- [ ] T119 [P] [US6] Core Web Vitals test (LCP, FID, CLS) in tests/TechHub.E2E.Tests/Tests/PerformanceTests.cs
- [ ] T120 [P] [US6] API response time test (<50ms p95 cached) in tests/TechHub.Api.Tests/Performance/ResponseTimeTests.cs
- [ ] T121 [P] [US6] Client-side filter performance test (<100ms) in tests/TechHub.Web.Tests/Performance/FilterPerformanceTests.cs

**Checkpoint**: User Story 6 complete - performance targets met, Lighthouse >95, Core Web Vitals pass

---

## Phase 6: Supporting Features (P2-P3)

### User Story 3 - RSS Feed Subscription (P2)

**Goal**: Users can subscribe to RSS feeds for sections and collections

**Independent Test**: Subscribe to /ai/feed.xml in RSS reader, verify feed is valid RSS 2.0 with recent items

- [X] T122 [P] [US3] Implement GET /api/rss/{sectionName} endpoint in src/TechHub.Api/Endpoints/RssEndpoints.cs
- [X] T123 [P] [US3] Add RSS feed link to section page header in src/TechHub.Web/Pages/Section.razor  
- [X] T124 [P] [US3] Integration test for RSS feed generation in tests/TechHub.Api.Tests/Endpoints/RssEndpointsTests.cs
- [X] T125 [P] [US3] Playwright test for RSS feed subscription in tests/TechHub.E2E.Tests/Tests/RssFeedTests.cs

### User Story 4 - Weekly Roundup Access (P2)

**Goal**: Users can view curated weekly roundups on home page and in dedicated section

**Independent Test**: Navigate to home page, verify 4 most recent roundups displayed, click a roundup, verify curated content

- [ ] T126 [US4] Implement roundups collection filtering in src/TechHub.Web/Pages/Index.razor
- [ ] T127 [P] [US4] Create RoundupCard component in src/TechHub.Web/Components/RoundupCard.razor
- [ ] T128 [P] [US4] Playwright test for roundup display on home page in tests/TechHub.E2E.Tests/Tests/RoundupTests.cs

### User Story 5 - SEO and Discoverability (P2)

**Goal**: Site is optimized for search engines with proper metadata, structured data, and sitemap

**Independent Test**: Run Google Rich Results Test, verify Schema.org markup, inspect meta tags, validate sitemap.xml

- [ ] T129 [P] [US5] Add meta tags (title, description, Open Graph, Twitter Card) in src/TechHub.Web/Pages/_Host.cshtml
- [ ] T130 [P] [US5] Implement Schema.org Article markup in src/TechHub.Web/Components/ContentDetail.razor
- [ ] T131 [P] [US5] Implement Schema.org WebSite markup in src/TechHub.Web/Pages/Index.razor
- [ ] T132 [US5] Generate sitemap.xml endpoint in src/TechHub.Api/Endpoints/SitemapEndpoints.cs
- [ ] T133 [P] [US5] Implement canonical URL meta tags in src/TechHub.Web/Components/ContentDetail.razor
- [ ] T134 [P] [US5] Lighthouse SEO test (score >95) in tests/TechHub.E2E.Tests/Tests/SeoTests.cs
- [ ] T135 [P] [US5] Structured data validation test in tests/TechHub.E2E.Tests/Tests/SeoTests.cs

### User Story 7 - Analytics and Monitoring (P3)

**Goal**: Track user behavior and performance metrics with Google Analytics and Application Insights

**Independent Test**: Navigate site, verify GA4 events in debug mode, check Application Insights for telemetry

- [ ] T136 [P] [US7] Integrate Google Analytics 4 in src/TechHub.Web/Pages/_Host.cshtml
- [ ] T137 [P] [US7] Add custom event tracking for filter interactions in src/TechHub.Web/Services/FilterStateService.cs
- [ ] T138 [P] [US7] Configure OpenTelemetry for Application Insights in src/TechHub.Api/Program.cs
- [ ] T139 [P] [US7] Add custom telemetry for repository operations in src/TechHub.Infrastructure/Repositories/
- [ ] T140 [US7] Implement cookie consent banner in src/TechHub.Web/Components/CookieConsent.razor

### Infinite Scroll Pagination

- [ ] T141 [US2] Implement infinite scroll component in src/TechHub.Web/Components/InfiniteScroll.razor
- [ ] T142 [US2] Integrate infinite scroll with Section page in src/TechHub.Web/Pages/Section.razor
- [ ] T143 [US2] Playwright test for infinite scroll in tests/TechHub.E2E.Tests/Tests/InfiniteScrollTests.cs

### Search Functionality

- [ ] T144 [US2] Implement GET /api/search endpoint in src/TechHub.Api/Endpoints/SearchEndpoints.cs
- [ ] T145 [P] [US2] Integration test for search endpoint in tests/TechHub.Api.Tests/Endpoints/SearchEndpointsTests.cs

### Available Tags Endpoint

- [ ] T146 [US2] Implement GET /api/tags endpoint in src/TechHub.Api/Endpoints/TagsEndpoints.cs
- [ ] T147 [P] [US2] Integration test for tags endpoint in tests/TechHub.Api.Tests/Endpoints/TagsEndpointsTests.cs

**Checkpoint**: All supporting features complete - RSS feeds, roundups, SEO, analytics, infinite scroll, search

---

## Phase 7: Infrastructure & Deployment

**Purpose**: Azure resources, CI/CD pipeline, monitoring, and production readiness

### Azure Resources

- [ ] T148 Create Bicep template for Azure Container Apps in infra/main.bicep
- [ ] T149 [P] Define Container Apps environment in infra/modules/container-apps-environment.bicep
- [ ] T150 [P] Define Container Apps for API in infra/modules/api-container-app.bicep
- [ ] T151 [P] Define Container Apps for Web in infra/modules/web-container-app.bicep
- [ ] T152 [P] Configure Application Insights in infra/modules/application-insights.bicep
- [ ] T153 [P] Configure Key Vault for secrets in infra/modules/key-vault.bicep

### Docker Containerization

- [ ] T154 [P] Create Dockerfile for TechHub.Api in src/TechHub.Api/Dockerfile
- [ ] T155 [P] Create Dockerfile for TechHub.Web in src/TechHub.Web/Dockerfile
- [ ] T156 Create docker-compose.yml for local development in docker-compose.yml

### CI/CD Pipeline

- [ ] T157 Create GitHub Actions workflow for CI in .github/workflows/dotnet-ci.yml
- [ ] T158 [P] Add build and test jobs in .github/workflows/dotnet-ci.yml
- [ ] T159 [P] Add code coverage reporting in .github/workflows/dotnet-ci.yml
- [ ] T160 Create GitHub Actions workflow for CD in .github/workflows/dotnet-cd.yml
- [ ] T161 [P] Add container build and push jobs in .github/workflows/dotnet-cd.yml
- [ ] T162 [P] Add Azure deployment job in .github/workflows/dotnet-cd.yml

### Monitoring & Alerts

- [ ] T163 [P] Configure Application Insights alerts for error rate >0.1% in infra/modules/alerts.bicep
- [ ] T164 [P] Configure Application Insights alerts for response time >200ms in infra/modules/alerts.bicep
- [ ] T165 Configure auto-scaling rules (1-10 instances, 80% CPU/memory threshold) in infra/modules/scaling.bicep

### Health Checks

- [ ] T166 Implement health check endpoint in src/TechHub.Api/Endpoints/HealthEndpoints.cs
- [ ] T167 [P] Add dependency health checks (cache, file system) in src/TechHub.Api/HealthChecks/

### Security

- [ ] T168 [P] Configure Content Security Policy headers in src/TechHub.Api/Middleware/SecurityHeadersMiddleware.cs
- [ ] T169 [P] Configure HSTS headers in src/TechHub.Api/Program.cs
- [ ] T170 [P] Configure CORS policy (whitelist only) in src/TechHub.Api/Program.cs
- [ ] T171 Implement input validation and sanitization in all API endpoints

**Checkpoint**: Infrastructure complete - ready for production deployment

---

## Phase 8: Polish & Cross-Cutting Concerns

**Purpose**: Documentation, final testing, code quality, and migration cutover

### Documentation

- [ ] T172 [P] Create README.md with setup instructions
- [ ] T173 [P] Update AGENTS.md with .NET development patterns
- [ ] T174 [P] Document API endpoints in docs/api-documentation.md
- [ ] T175 [P] Create deployment runbook in docs/deployment-runbook.md

### Code Quality

- [ ] T176 Run dotnet format on all projects
- [ ] T177 [P] Enable nullable reference types in all projects
- [ ] T178 [P] Fix all compiler warnings (treat warnings as errors)
- [ ] T179 Run security analysis (dotnet-sonarscanner or similar)
- [ ] T180 Verify code coverage >80% across all projects

### Final Testing

- [ ] T181 Run full E2E test suite against staging environment
- [ ] T182 [P] Run Lighthouse audits on all page types
- [ ] T183 [P] Run accessibility audits (Axe, WAVE)
- [ ] T184 [P] Run cross-browser testing (Chrome, Firefox, Safari, Edge)
- [ ] T185 Run performance load testing (Azure Load Testing)

### Migration Cutover

- [ ] T186 Freeze Jekyll content updates (2 hours before deployment)
- [ ] T187 Create final Jekyll site backup
- [ ] T188 Deploy .NET containers to Azure Container Apps staging
- [ ] T189 Verify staging environment health checks
- [ ] T190 Run smoke tests on staging
- [ ] T191 Update DNS records to point to Container Apps
- [ ] T192 Monitor production for 1 hour (error rate, response time)
- [ ] T193 Verify analytics tracking active
- [ ] T194 Announce migration complete

### Post-Migration

- [ ] T195 Monitor Application Insights for 24 hours
- [ ] T196 [P] Review error logs and fix critical issues
- [ ] T197 [P] Validate content parity (all Jekyll content migrated)
- [ ] T198 Plan Jekyll decommissioning (30 days retention)

**Checkpoint**: Migration complete - .NET site live in production

---

## Dependencies & Execution Order

### Phase Dependencies

1. **Phase 1: Foundation** (T001-T036) - No dependencies, MUST complete first
2. **Phase 2: Data Access & Core Services** (T037-T053) - Depends on Phase 1 completion
3. **Phase 3: User Story 1** (T054-T087) - Depends on Phase 2 completion (MVP!)
4. **Phase 4: User Story 2** (T088-T107) - Depends on Phase 3 completion
5. **Phase 5: User Story 6** (T108-T121) - Depends on Phase 3-4 completion
6. **Phase 6: Supporting Features** (T122-T147) - Depends on Phase 3-5 completion
7. **Phase 7: Infrastructure & Deployment** (T148-T171) - Depends on Phase 3-6 completion
8. **Phase 8: Polish & Cutover** (T172-T198) - Depends on all previous phases

### User Story Execution Order

**Sequential (Recommended for Single Developer)**:

1. Complete Foundation (Phase 1) + Data Access (Phase 2)
2. Implement User Story 1 (P1) → Test → MVP Ready!
3. Implement User Story 2 (P1) → Test
4. Implement User Story 6 (P1) → Test
5. Implement User Story 3 (P2) → Test
6. Implement User Story 4 (P2) → Test
7. Implement User Story 5 (P2) → Test
8. Implement User Story 7 (P3) → Test

**Parallel (If Multiple Developers)**:

After Phase 1-2 complete:

- Developer A: User Story 1 (T054-T087)
- Developer B: User Story 2 (T088-T107)
- Developer C: User Story 6 (T108-T121)

Then continue with supporting features in parallel.

### Within Each Phase

- Tasks marked [P] can run in parallel
- Tests should be written BEFORE or DURING implementation (not after)
- Integration tests verify API contracts
- Component tests verify Blazor UI
- E2E tests verify full user journeys
- All tests must pass before moving to next phase

---

## Parallel Opportunities

### Phase 1 - Foundation

- All project initialization tasks (T002-T006) can run in parallel
- All test project initialization tasks (T007-T011) can run in parallel
- All configuration tasks (T013-T016) can run in parallel
- All domain model creation tasks (T021-T030) can run in parallel
- All interface definition tasks (T031-T034) can run in parallel
- All extension method tasks (T035-T036) can run in parallel

### Phase 2 - Data Access

- Unit tests (T048-T053) can run in parallel

### Phase 3 - User Story 1

- API endpoint tasks (T054-T056) can run in parallel
- Blazor component tasks (T059-T063) can run in parallel
- Accessibility tasks (T070-T072) can run in parallel
- CSS tasks (T074-T076) can run in parallel
- Integration tests (T077-T079) can run in parallel
- Component tests (T080-T082) can run in parallel
- E2E tests (T083-T087) can run in parallel

### Phase 4 - User Story 2

- Filter component tasks (T091-T093) can run in parallel
- Filtering logic tasks (T096-T097) can run in parallel
- Component tests (T099-T101) can run in parallel
- E2E tests (T103-T105) can run in parallel

### Phase 5 - User Story 6

- Performance optimization tasks (T108-T110) can run in parallel
- Blazor rendering mode tasks (T114-T115) can run in parallel
- Image optimization tasks (T116-T117) can run in parallel
- Performance tests (T118-T121) can run in parallel

### Phase 6 - Supporting Features

- User Story 3 tasks (T122-T125) can run in parallel
- User Story 5 tasks (T129-T135) can run in parallel
- User Story 7 tasks (T136-T139) can run in parallel

### Phase 7 - Infrastructure

- Bicep module tasks (T149-T153) can run in parallel
- Dockerfile tasks (T154-T155) can run in parallel
- CI/CD workflow tasks (T158-T159, T161-T162) can run in parallel
- Alert configuration tasks (T163-T164) can run in parallel
- Security header tasks (T168-T170) can run in parallel

### Phase 8 - Polish

- Documentation tasks (T172-T175) can run in parallel
- Code quality tasks (T177-T179) can run in parallel
- Final testing tasks (T182-T184) can run in parallel
- Post-migration tasks (T196-T197) can run in parallel

---

## Implementation Strategy

### MVP First (Minimum Viable Product)

**Deliver Value Early**:

1. Complete **Phase 1: Foundation** (T001-T036)
2. Complete **Phase 2: Data Access** (T037-T053)
3. Complete **Phase 3: User Story 1** (T054-T087)
4. **STOP and VALIDATE**: Test User Story 1 independently
5. **Deploy to Staging**: Verify functionality
6. **Demo/Review**: Get feedback on core experience

**Result**: Users can browse sections and read content (core value!)

### Incremental Delivery

**Add Features Progressively**:

1. MVP (Phases 1-3) → **Deploy & Demo**
2. Add User Story 2 (Phase 4) → **Deploy & Demo** (filtering!)
3. Add User Story 6 (Phase 5) → **Deploy & Demo** (performance!)
4. Add Supporting Features (Phase 6) → **Deploy & Demo** (RSS, SEO, etc.)
5. Deploy Infrastructure (Phase 7) → **Production Ready**
6. Final Polish (Phase 8) → **Migration Complete**

Each increment adds value without breaking previous work.

### Parallel Team Strategy

**Multiple Developers After Foundation**:

1. **Team completes Phases 1-2 together** (Foundation + Data Access)
2. **Once Phase 2 done, split work**:
   - Developer A: User Story 1 (Phase 3)
   - Developer B: User Story 2 (Phase 4)
   - Developer C: User Story 6 (Phase 5)
3. **Merge and integrate** independently tested stories
4. **Continue with Phase 6-8** together or in parallel

---

## Task Complexity Estimates

### Simple (1-2 hours)

- Creating entity/DTO classes
- Writing unit tests
- Adding configuration files
- Documentation updates

### Medium (3-6 hours)

- Implementing repository patterns
- Creating Blazor components
- Writing integration tests
- API endpoint implementation

### Complex (1-2 days)

- Client-side filtering service with URL sync
- RSS feed generation with caching
- E2E test suites
- Performance optimization

### Very Complex (3-5 days)

- Full user story implementation (all tasks in a phase)
- Infrastructure setup with Bicep
- CI/CD pipeline configuration
- Migration cutover execution

---

## Total Task Count by Phase

- **Phase 1**: 36 tasks (Foundation)
- **Phase 2**: 17 tasks (Data Access)
- **Phase 3**: 34 tasks (User Story 1)
- **Phase 4**: 20 tasks (User Story 2)
- **Phase 5**: 14 tasks (User Story 6)
- **Phase 6**: 26 tasks (Supporting Features)
- **Phase 7**: 24 tasks (Infrastructure)
- **Phase 8**: 27 tasks (Polish & Cutover)

**Total**: 198 tasks

---

## MVP Scope (Phases 1-3)

**Recommended First Delivery**:

- Phase 1: Foundation (36 tasks)
- Phase 2: Data Access (17 tasks)
- Phase 3: User Story 1 (34 tasks)

**MVP Total**: 87 tasks

**MVP Delivers**:

- Users can browse sections
- Users can view collections
- Users can read full content
- Responsive design
- Accessibility (WCAG 2.1 AA)
- Server-side rendering
- Basic caching
- Comprehensive testing

**MVP Success Criteria**:

- Home page loads with section grid
- Section pages show collections and content
- Content detail pages render markdown
- YouTube videos embed correctly
- Site works on mobile
- Screen readers work correctly
- All tests pass

---

## Notes

- **[P] tasks**: Different files, no dependencies, can run in parallel
- **[Story] label**: Maps task to specific user story for traceability
- **Test-first workflow**: Write tests BEFORE or DURING implementation
- **Each user story independently testable**: Can deploy incrementally
- **Commit after each logical group**: Keep commits small and focused
- **Stop at checkpoints**: Validate story works before continuing
- **Avoid premature optimization**: Focus on working code first, optimize in Phase 5

---

**Status**: ✅ Tasks generated - Ready for implementation following 6-phase sequence from research.md
