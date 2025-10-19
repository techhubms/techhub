# Tech Hub Migration Plan: Jekyll to .NET Web Application

## Overview

This plan outlines the step-by-step migration of the Tech Hub from a Jekyll-based static site to a modern .NET 9 web application with Blazor frontend and ASP.NET Core Web API backend. The new application will maintain identical functionality and visual appearance while introducing NLWeb/MCP compatibility and a more maintainable architecture.

## Key Requirements

- ✅ Maintain identical visual appearance and functionality
- ✅ Implement all current Jekyll features (excluding PowerShell preprocessing)
- ✅ Use semantic HTML for NLWeb compatibility
- ✅ API doubles as MCP server
- ✅ Full unit and integration test coverage
- ✅ Modular, agent-friendly architecture with AGENTS.md files
- ✅ No authentication initially (will be added later)
- ✅ Performance optimizations (7-day content limiting, client-side filtering)

## Current Site Features to Migrate

### Core Features
1. **Section-based organization**: All, AI, GitHub Copilot, ML, Azure, .NET, DevOps, Security
2. **Collection types**: News, Blogs, Videos, Community, Events, Roundups
3. **Content filtering system**:
   - Date filters (Last 3 days, Last 30 days, etc.)
   - Tag-based filters (unified system for sections, collections, and content tags)
   - Real-time text search with debouncing
   - "20 + Same-Day" content limiting rule with 7-day recency filter
4. **RSS feeds**: Per-section and global feeds
5. **Custom pages**: Features page, Levels of Enlightenment, VS Code Updates
6. **Content management**: Markdown-based content with YAML frontmatter

---

## Phase 1: Foundation & Data Layer (API Backend)

### Step 1.1: Data Models & Domain Layer
**Goal**: Create core domain models and interfaces

**Tasks**:
1. Define content models:
   - `ContentItem` (base class)
   - `NewsArticle`, `BlogPost`, `Video`, `CommunityPost`, `Event`, `Roundup`
   - `Section`, `Collection`, `Tag`
2. Define filtering models:
   - `DateFilter`, `TagFilter`, `SearchCriteria`
   - `FilterResult<T>`
3. Create interfaces:
   - `IContentRepository`
   - `IFilteringService`
   - `ISearchService`

**Files to create**:
```
src/techhub.webapp/techhub.webapp.ApiService/
├── Domain/
│   ├── Models/
│   │   ├── ContentItem.cs
│   │   ├── NewsArticle.cs
│   │   ├── BlogPost.cs
│   │   ├── Video.cs
│   │   ├── Section.cs
│   │   ├── Collection.cs
│   │   ├── Tag.cs
│   │   └── AGENTS.md
│   ├── Interfaces/
│   │   ├── IContentRepository.cs
│   │   ├── IFilteringService.cs
│   │   ├── ISearchService.cs
│   │   └── AGENTS.md
│   └── AGENTS.md
```

**Tests**:
- Unit tests for model validation
- Tests for model serialization/deserialization

### Step 1.2: Content Repository Implementation
**Goal**: Implement markdown file reading and parsing

**Tasks**:
1. Create `MarkdownContentRepository`:
   - Parse markdown files from existing collections
   - Extract YAML frontmatter
   - Parse content and excerpts
2. Implement caching strategy:
   - In-memory cache with file system watcher
   - Cache invalidation on file changes
3. Create content indexing service:
   - Build tag relationships
   - Calculate tag hierarchies (subset matching)

**Files to create**:
```
src/techhub.webapp/techhub.webapp.ApiService/
├── Infrastructure/
│   ├── Repositories/
│   │   ├── MarkdownContentRepository.cs
│   │   ├── ContentCache.cs
│   │   └── AGENTS.md
│   ├── Parsers/
│   │   ├── YamlFrontmatterParser.cs
│   │   ├── MarkdownParser.cs
│   │   └── AGENTS.md
│   └── AGENTS.md
```

**Tests**:
- Unit tests for frontmatter parsing
- Integration tests for file reading
- Cache invalidation tests

### Step 1.3: Filtering & Search Services
**Goal**: Implement filtering logic matching Jekyll behavior

**Tasks**:
1. Create `FilteringService`:
   - Implement "20 + Same-Day" limiting rule
   - Implement 7-day recency filter
   - Date range filtering (Europe/Brussels timezone)
   - Tag-based filtering with subset matching
2. Create `SearchService`:
   - Full-text search across titles, descriptions, authors, tags
   - Case-insensitive, partial word matching
3. Create `TagRelationshipService`:
   - Pre-calculate tag relationships
   - Implement subset matching logic

**Files to create**:
```
src/techhub.webapp/techhub.webapp.ApiService/
├── Services/
│   ├── FilteringService.cs
│   ├── SearchService.cs
│   ├── TagRelationshipService.cs
│   ├── DateFilterService.cs
│   └── AGENTS.md
```

**Tests**:
- Unit tests for each filter type
- Integration tests for combined filters
- Performance tests for large datasets
- Edge case tests (timezone boundaries, same-day logic)

### Step 1.4: API Endpoints
**Goal**: Create RESTful API with MCP compatibility

**Tasks**:
1. Create REST endpoints:
   - `GET /api/sections` - Get all sections
   - `GET /api/sections/{section}` - Get section details
   - `GET /api/sections/{section}/collections/{collection}` - Get collection items
   - `GET /api/content/{id}` - Get single content item
   - `POST /api/content/filter` - Filter content with criteria
   - `GET /api/tags` - Get all tags with relationships
   - `GET /api/search` - Search endpoint
2. Add MCP endpoint:
   - `POST /mcp` - MCP server endpoint
   - Implement `ask` function for natural language queries
3. Add RSS feed endpoints:
   - `GET /feed.xml` - Global feed
   - `GET /{section}.xml` - Section-specific feeds
   - `GET /roundups.xml` - Roundups feed

**Files to create**:
```
src/techhub.webapp/techhub.webapp.ApiService/
├── Controllers/
│   ├── SectionsController.cs
│   ├── ContentController.cs
│   ├── FilterController.cs
│   ├── TagsController.cs
│   ├── SearchController.cs
│   ├── McpController.cs
│   ├── FeedController.cs
│   └── AGENTS.md
├── Mcp/
│   ├── McpServer.cs
│   ├── McpFunctionHandler.cs
│   └── AGENTS.md
└── AGENTS.md
```

**Tests**:
- API endpoint integration tests
- MCP server functionality tests
- RSS feed generation tests
- API contract tests

---

## Phase 2: Frontend Foundation (Blazor Web)

### Step 2.1: Blazor Component Architecture
**Goal**: Set up Blazor component structure with semantic HTML

**Tasks**:
1. Create layout components:
   - `MainLayout.razor` - Overall layout
   - `Header.razor` - Site header with navigation
   - `Footer.razor` - Site footer
2. Create shared components:
   - `SectionCard.razor` - Section display card
   - `ContentCard.razor` - Content item card
   - `FilterPanel.razor` - Filter controls
   - `SearchBox.razor` - Search input
3. Use semantic HTML throughout:
   - `<header>`, `<nav>`, `<main>`, `<article>`, `<section>`, `<footer>`
   - Proper heading hierarchy
   - ARIA labels where needed

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   ├── Header.razor
│   │   ├── Footer.razor
│   │   └── AGENTS.md
│   ├── Shared/
│   │   ├── SectionCard.razor
│   │   ├── ContentCard.razor
│   │   ├── FilterPanel.razor
│   │   ├── SearchBox.razor
│   │   └── AGENTS.md
│   └── AGENTS.md
```

**Tests**:
- Blazor component unit tests (bUnit)
- Semantic HTML validation tests
- Accessibility tests

### Step 2.2: API Client Services
**Goal**: Create typed HTTP clients for API communication

**Tasks**:
1. Create API client services:
   - `SectionsApiClient` - Section data
   - `ContentApiClient` - Content items
   - `FilterApiClient` - Filtering operations
   - `SearchApiClient` - Search operations
2. Implement response caching
3. Add error handling and retry logic

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── Services/
│   ├── ApiClients/
│   │   ├── SectionsApiClient.cs
│   │   ├── ContentApiClient.cs
│   │   ├── FilterApiClient.cs
│   │   ├── SearchApiClient.cs
│   │   └── AGENTS.md
│   └── AGENTS.md
```

**Tests**:
- Mock HTTP client tests
- Client error handling tests

### Step 2.3: State Management
**Goal**: Implement client-side state management

**Tasks**:
1. Create state containers:
   - `FilterState` - Current filter selections
   - `SearchState` - Search query state
   - `NavigationState` - Section/collection navigation
2. Implement URL synchronization:
   - Update URL on filter changes
   - Restore state from URL parameters
3. Add browser history support

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── State/
│   ├── FilterState.cs
│   ├── SearchState.cs
│   ├── NavigationState.cs
│   └── AGENTS.md
```

**Tests**:
- State management unit tests
- URL sync integration tests

---

## Phase 3: Core Pages Implementation

### Step 3.1: Home Page
**Goal**: Implement main landing page

**Tasks**:
1. Create `Index.razor`:
   - Section grid display
   - Latest roundups (last 4)
   - Welcome message
2. Implement section card rendering
3. Add responsive design

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── Components/
│   ├── Pages/
│   │   ├── Index.razor
│   │   └── AGENTS.md
```

**Tests**:
- Page rendering tests
- Responsive layout tests

### Step 3.2: Section Index Pages
**Goal**: Implement section overview pages

**Tasks**:
1. Create `SectionIndex.razor`:
   - Section header with RSS link
   - Collection cards grid
   - Latest content from all collections
   - Filter panel (date, collection, tags)
2. Implement client-side filtering:
   - Date filter selection
   - Collection filter (tag-based)
   - Text search
   - Filter count updates
3. Add URL parameter handling

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── Components/
│   ├── Pages/
│   │   ├── SectionIndex.razor
│   │   └── AGENTS.md
```

**Tests**:
- Section page rendering tests
- Filter interaction tests
- URL parameter tests

### Step 3.3: Collection Pages
**Goal**: Implement collection listing pages

**Tasks**:
1. Create `CollectionPage.razor`:
   - Content item list
   - Filter panel (date, content tags)
   - Text search
2. Implement content card rendering:
   - Title, description, author
   - Publication date
   - Tags display
   - Link to full content
3. Add pagination/infinite scroll

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── Components/
│   ├── Pages/
│   │   ├── CollectionPage.razor
│   │   └── AGENTS.md
```

**Tests**:
- Collection page tests
- Content rendering tests

### Step 3.4: Content Detail Pages
**Goal**: Implement individual content pages

**Tasks**:
1. Create `ContentDetail.razor`:
   - Full content rendering
   - Markdown to HTML conversion
   - Author and metadata display
   - Link to original source
2. Support different content types:
   - News articles
   - Blog posts
   - Videos (with YouTube embed)
   - Community posts
   - Events

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── Components/
│   ├── Pages/
│   │   ├── ContentDetail.razor
│   │   ├── VideoPlayer.razor
│   │   └── AGENTS.md
```

**Tests**:
- Content rendering tests
- YouTube embed tests

### Step 3.5: Custom Pages
**Goal**: Implement special pages

**Tasks**:
1. Create GitHub Copilot Features page:
   - Feature grid organized by subscription tier
   - Video integration
   - GHES support indicator
2. Create Levels of Enlightenment page
3. Create VS Code Updates page

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── Components/
│   ├── Pages/
│   │   ├── GitHubCopilot/
│   │   │   ├── Features.razor
│   │   │   ├── LevelsOfEnlightenment.razor
│   │   │   ├── VsCodeUpdates.razor
│   │   │   └── AGENTS.md
│   │   └── AGENTS.md
```

**Tests**:
- Custom page rendering tests

---

## Phase 4: Client-Side Filtering & Interactivity

### Step 4.1: Filter System Implementation
**Goal**: Implement JavaScript-free client-side filtering using Blazor

**Tasks**:
1. Create filter components:
   - `DateFilterButtons.razor` - Date range buttons
   - `TagFilterButtons.razor` - Tag filter buttons
   - `SearchInput.razor` - Text search input
   - `ClearAllButton.razor` - Clear filters button
2. Implement filtering logic in C#:
   - Date range filtering (visitor's local timezone)
   - Tag subset matching
   - Text search across indexed content
   - Combined filter logic (AND)
3. Implement dynamic filter count updates
4. Handle zero-count filter states

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── Components/
│   ├── Filters/
│   │   ├── DateFilterButtons.razor
│   │   ├── TagFilterButtons.razor
│   │   ├── SearchInput.razor
│   │   ├── ClearAllButton.razor
│   │   └── AGENTS.md
│   ├── Services/
│   │   ├── ClientFilterService.cs
│   │   └── AGENTS.md
```

**Tests**:
- Filter component interaction tests
- Filter logic unit tests
- Combined filter scenario tests

### Step 4.2: URL State Synchronization
**Goal**: Sync filters with browser URL

**Tasks**:
1. Implement URL parameter management:
   - Serialize filter state to URL
   - Deserialize URL to filter state
   - Handle browser back/forward
2. Add debounced URL updates (300ms for search)
3. Ensure bookmarkable URLs

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── Services/
│   ├── UrlStateService.cs
│   └── AGENTS.md
```

**Tests**:
- URL sync tests
- Browser navigation tests

### Step 4.3: Performance Optimizations
**Goal**: Ensure responsive filtering

**Tasks**:
1. Implement virtual scrolling for large lists
2. Add debouncing for text search
3. Optimize re-render performance
4. Cache filtered results

**Tests**:
- Performance benchmarks
- Large dataset tests

---

## Phase 5: Styling & Visual Parity

### Step 5.1: CSS Migration
**Goal**: Migrate existing CSS to new structure

**Tasks**:
1. Copy and adapt existing SASS files:
   - Migrate `_sass/` directory structure
   - Update selectors for Blazor components
   - Ensure responsive design
2. Maintain visual appearance:
   - Section background images
   - Color scheme
   - Typography
   - Filter button styles
   - Card layouts

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── wwwroot/
│   ├── css/
│   │   ├── site.scss
│   │   ├── _variables.scss
│   │   ├── _layout.scss
│   │   ├── _filters.scss
│   │   ├── _cards.scss
│   │   └── AGENTS.md
│   ├── images/
│   │   └── section-backgrounds/
│   └── AGENTS.md
```

**Tests**:
- Visual regression tests (Playwright)
- Responsive design tests

### Step 5.2: JavaScript Interop (Minimal)
**Goal**: Add only essential JavaScript for features Blazor can't handle

**Tasks**:
1. Create interop services only if needed:
   - Smooth scrolling (if required)
   - Focus management (if required)
2. Keep JavaScript minimal and isolated

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Web/
├── wwwroot/
│   ├── js/
│   │   ├── interop.js (only if needed)
│   │   └── AGENTS.md
```

**Tests**:
- Interop functionality tests

---

## Phase 6: RSS Feed Generation

### Step 6.1: RSS Feed Service
**Goal**: Generate RSS/Atom feeds

**Tasks**:
1. Create RSS feed generator:
   - Global feed (all content)
   - Section-specific feeds
   - Roundups feed
2. Implement feed caching
3. Add feed autodiscovery links

**Files to create**:
```
src/techhub.webapp/techhub.webapp.ApiService/
├── Services/
│   ├── RssFeedService.cs
│   └── AGENTS.md
```

**Tests**:
- RSS feed generation tests
- Feed validation tests

---

## Phase 7: Testing & Quality Assurance

### Step 7.1: Unit Tests
**Goal**: Comprehensive unit test coverage

**Tasks**:
1. Backend unit tests:
   - Domain model tests
   - Service logic tests
   - Filter logic tests
   - Parser tests
2. Frontend unit tests:
   - Component tests (bUnit)
   - State management tests
   - API client tests

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Tests/
├── ApiService/
│   ├── Domain/
│   ├── Services/
│   ├── Infrastructure/
│   └── AGENTS.md
├── Web/
│   ├── Components/
│   ├── Services/
│   ├── State/
│   └── AGENTS.md
└── AGENTS.md
```

**Coverage targets**:
- >80% code coverage
- All critical paths covered
- Edge cases tested

### Step 7.2: Integration Tests
**Goal**: Test API and database integration

**Tasks**:
1. API integration tests:
   - Endpoint behavior
   - Request/response validation
   - Error handling
2. Repository integration tests:
   - File system operations
   - Cache behavior
3. End-to-end filter tests:
   - API → Service → Repository flow

**Files to create**:
```
src/techhub.webapp/techhub.webapp.Tests/
├── Integration/
│   ├── Api/
│   ├── Repositories/
│   ├── Filters/
│   └── AGENTS.md
```

### Step 7.3: E2E Tests with Playwright
**Goal**: Browser-based functional testing

**Tasks**:
1. Create E2E test scenarios:
   - Homepage navigation
   - Section browsing
   - Filter interactions
   - Search functionality
   - Content detail views
2. Visual regression tests
3. Cross-browser testing (Chrome, Firefox, Safari)
4. Responsive design tests

**Files to create**:
```
spec/e2e-webapp/
├── tests/
│   ├── home.spec.ts
│   ├── sections.spec.ts
│   ├── filters.spec.ts
│   ├── search.spec.ts
│   ├── content.spec.ts
│   └── AGENTS.md
├── playwright.config.ts
└── AGENTS.md
```

---

## Phase 8: Documentation & Deployment

### Step 8.1: AGENTS.md Files
**Goal**: Document architecture for AI agents

**Tasks**:
1. Create AGENTS.md in each major folder:
   - Explain folder purpose
   - Document key classes/interfaces
   - Describe relationships
   - Provide usage examples
   - List testing requirements

**Template structure**:
```markdown
# [Folder Name] - Agent Context

## Purpose
What this folder contains and why it exists.

## Key Components
- **ClassName**: Brief description
- **AnotherClass**: Brief description

## Relationships
How these components interact with other parts of the system.

## Usage Examples
Code snippets showing typical usage.

## Testing
How to test code in this folder.

## Important Notes
Any gotchas or special considerations.
```

### Step 8.2: Migration Documentation
**Goal**: Document the migration process

**Tasks**:
1. Create migration guide:
   - Architecture comparison (Jekyll vs .NET)
   - Feature parity checklist
   - Deployment guide
   - Configuration guide
2. Update README with new structure

**Files to create**:
```
docs/
├── migration/
│   ├── architecture-comparison.md
│   ├── feature-parity.md
│   ├── deployment-guide.md
│   └── AGENTS.md
```

### Step 8.3: Deployment Setup
**Goal**: Configure Azure deployment

**Tasks**:
1. Update Azure Static Web Apps config:
   - Configure for Blazor WebAssembly/Server
   - Set up API routes
   - Configure custom domain
2. Set up CI/CD:
   - Build pipeline
   - Test execution
   - Deployment automation

**Files to create**:
```
.github/workflows/
├── webapp-deploy.yml
└── webapp-tests.yml
```

---

## Phase 9: Performance & Optimization

### Step 9.1: Caching Strategy
**Goal**: Implement comprehensive caching

**Tasks**:
1. API response caching:
   - Output caching for read-heavy endpoints
   - Cache tag relationships
   - Cache filter results
2. Frontend caching:
   - Component-level caching
   - HTTP caching headers

### Step 9.2: Content Loading Optimization
**Goal**: Implement progressive loading

**Tasks**:
1. Server-side "20 + Same-Day" limiting
2. Lazy loading for images
3. Virtual scrolling for long lists
4. Code splitting for Blazor

### Step 9.3: Performance Monitoring
**Goal**: Track performance metrics

**Tasks**:
1. Add Application Insights
2. Monitor API response times
3. Track client-side rendering performance
4. Set up alerts for performance degradation

---

## Phase 10: NLWeb/MCP Integration

### Step 10.1: MCP Server Implementation
**Goal**: Make API function as MCP server

**Tasks**:
1. Implement MCP protocol:
   - Function discovery
   - `ask` function implementation
   - Streaming responses
2. Add natural language query processing:
   - Parse user queries
   - Map to filter criteria
   - Return semantic results

**Files to create**:
```
src/techhub.webapp/techhub.webapp.ApiService/
├── Mcp/
│   ├── McpProtocolHandler.cs
│   ├── NaturalLanguageProcessor.cs
│   ├── QueryMapper.cs
│   └── AGENTS.md
```

**Tests**:
- MCP protocol tests
- Natural language query tests
- Claude Desktop integration tests

### Step 10.2: Semantic HTML Validation
**Goal**: Ensure full NLWeb compatibility

**Tasks**:
1. Validate semantic HTML structure:
   - Proper heading hierarchy
   - Semantic elements usage
   - ARIA labels
2. Test with NLWeb tools:
   - Verify Schema.org markup (if needed)
   - Test MCP connectivity
   - Validate conversational interface

---

## Success Criteria

### Functional Parity
- ✅ All sections and collections accessible
- ✅ Filtering works identically to Jekyll site
- ✅ Search returns correct results
- ✅ RSS feeds generate correctly
- ✅ Custom pages function properly

### Visual Parity
- ✅ Pixel-perfect match to production site
- ✅ Responsive design works on all devices
- ✅ All interactive elements function smoothly
- ✅ Loading states and animations present

### Technical Requirements
- ✅ >80% test coverage
- ✅ All E2E tests passing
- ✅ API response time <200ms (p95)
- ✅ Page load time <2s (p95)
- ✅ MCP server functional with Claude Desktop

### Code Quality
- ✅ All AGENTS.md files present
- ✅ Clear separation of concerns
- ✅ Consistent coding standards
- ✅ Comprehensive documentation

---

## Timeline Estimate

Based on complexity and dependencies:

- **Phase 1** (API Backend): 2-3 weeks
- **Phase 2** (Frontend Foundation): 1-2 weeks
- **Phase 3** (Core Pages): 2-3 weeks
- **Phase 4** (Filtering): 1-2 weeks
- **Phase 5** (Styling): 1 week
- **Phase 6** (RSS): 1 week
- **Phase 7** (Testing): 2 weeks
- **Phase 8** (Documentation): 1 week
- **Phase 9** (Optimization): 1 week
- **Phase 10** (NLWeb/MCP): 1-2 weeks

**Total**: 13-19 weeks (3-5 months)

---

## Risk Mitigation

### Technical Risks
1. **Performance**: Content limiting and caching mitigate
2. **Filter complexity**: Pre-calculated relationships simplify
3. **Visual parity**: Systematic CSS migration and visual tests

### Schedule Risks
1. **Scope creep**: Strict feature parity focus
2. **Blocking dependencies**: Parallel work where possible
3. **Testing time**: Continuous testing throughout

---

## Next Steps

1. **Review and approve this plan**
2. **Set up development environment**
3. **Begin Phase 1: Data models and domain layer**
4. **Establish CI/CD pipeline early**
5. **Create first AGENTS.md file as template**

---

## Notes

- PowerShell preprocessing (RSS processing, tag enhancement) is excluded as requested
- Authentication will be added in a future phase
- The plan prioritizes API development first, then UI on top (as suggested)
- NLWeb compatibility is built in from the start via semantic HTML
- MCP server functionality is a natural extension of the REST API
