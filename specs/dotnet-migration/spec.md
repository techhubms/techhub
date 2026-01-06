# Feature Specification: Tech Hub .NET Migration

**Feature Branch**: `dotnet-migration`  
**Created**: 2026-01-02  
**Status**: Draft  
**Input**: Migrate Tech Hub content platform from Jekyll to modern .NET/Blazor architecture with optimal greenfield design

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Content Consumer Experience (Priority: P1)

As a Microsoft technology enthusiast, I want to browse and discover relevant content (news, blogs, videos, community posts) organized by topic (AI, GitHub Copilot, Azure, etc.), so I can stay up-to-date with the latest Microsoft technology developments.

**Why this priority**: Core value proposition - users must be able to find and consume content. Without this, the site has no purpose.

**Independent Test**: Can be fully tested by navigating to the site, browsing sections (e.g., /github-copilot), viewing content lists, and opening individual articles/videos. Delivers immediate value without any other features.

**Acceptance Scenarios**:

1. **Given** I visit the home page, **When** I view the page, **Then** I see all available sections (AI, GitHub Copilot, Azure, ML, .NET, DevOps, Security) with descriptions and content counts
2. **Given** I click on a section (e.g., "GitHub Copilot"), **When** the section page loads, **Then** I see all content collections (News, Blogs, Videos, Community) for that section
3. **Given** I am viewing a section page, **When** I select a collection (e.g., "News"), **Then** I see a list of all news items related to that section
4. **Given** I am viewing a content list, **When** I click on an item, **Then** I see the full article/blog post with rendered markdown, author, date, and tags
5. **Given** I am viewing a video content item, **When** the page loads, **Then** I see an embedded YouTube player with the video ready to play
6. **Given** I am viewing any page, **When** I use a screen reader, **Then** all content is accessible with proper semantic HTML and ARIA labels
7. **Given** I am on a mobile device, **When** I view any page, **Then** the layout adapts responsively and remains fully functional

---

### User Story 2 - Content Discovery via Filtering (Priority: P1)

As a content consumer, I want to filter content by date range, tags, and search text, so I can quickly find specific articles or topics I'm interested in without scrolling through everything.

**Why this priority**: Essential for usability - without filtering, large content libraries become overwhelming and unusable.

**Independent Test**: Can be tested by applying filters on any collection page and verifying results update instantly. Delivers value even if other features are incomplete.

**Acceptance Scenarios**:

1. **Given** I am viewing a content list with 100+ items, **When** I type text into the search box, **Then** results filter in real-time (debounced) to show only items matching the search text in title, description, or tags
2. **Given** I am viewing a content list, **When** I click a date filter (e.g., "Last 30 days"), **Then** only content published within that timeframe is displayed
3. **Given** I am viewing a content list, **When** I click multiple tag filters, **Then** content matching ANY of those tags is displayed (OR logic)
4. **Given** I have applied multiple filters, **When** I click "Clear All", **Then** all filters are removed and the full content list is restored
5. **Given** I am viewing filtered content, **When** I scroll to the bottom, **Then** the next batch of filtered results loads automatically (infinite scroll)
6. **Given** I apply filters, **When** I copy the page URL, **Then** the URL includes filter parameters so I can share the filtered view with others
7. **Given** I click the browser back button after applying filters, **When** the page reloads, **Then** my previous filter state is restored from URL parameters

---

### User Story 3 - RSS Feed Subscription (Priority: P2)

As a content consumer, I want to subscribe to RSS feeds for specific sections or collections, so I can receive updates in my preferred RSS reader without visiting the website.

**Why this priority**: Important for power users and automated content aggregation, but not required for basic site functionality.

**Independent Test**: Can be tested by subscribing to a feed URL in any RSS reader and verifying new content appears. Delivers value for RSS users regardless of other features.

**Acceptance Scenarios**:

1. **Given** I visit a section page, **When** I look for RSS options, **Then** I see a clearly labeled RSS feed link in the header or footer
2. **Given** I click an RSS feed link, **When** my RSS reader processes it, **Then** I see a valid RSS 2.0 feed with the most recent 50 items
3. **Given** I subscribe to a section feed (e.g., /ai/feed.xml), **When** new content is published in that section, **Then** it appears in my RSS reader within 1 hour
4. **Given** I am viewing an RSS feed, **When** I inspect the XML, **Then** each item includes title, description, link, author, publication date, and categories
5. **Given** content has a YouTube video, **When** I view it in my RSS reader, **Then** the video is embedded or linked appropriately

---

### User Story 4 - Weekly Roundup Access (Priority: P2)

As a content consumer, I want to view curated weekly roundups that highlight the best content across all sections, so I can catch up on important updates without reading everything.

**Why this priority**: Valuable for busy users, but not critical to core functionality. Can be delivered after basic content browsing.

**Independent Test**: Can be tested by viewing the home page and clicking on roundup links. Delivers value as a standalone feature.

**Acceptance Scenarios**:

1. **Given** I visit the home page, **When** the page loads, **Then** I see the 4 most recent weekly roundups prominently displayed
2. **Given** I click on a roundup, **When** the page loads, **Then** I see a curated collection of content organized by section with commentary
3. **Given** I am viewing a roundup, **When** I click on linked content, **Then** I navigate to the full article/video
4. **Given** I want to browse all roundups, **When** I navigate to the roundups section, **Then** I see a complete archive of all past roundups

---

### User Story 5 - SEO and Discoverability (Priority: P2)

As a content creator or marketer, I want the site to be optimized for search engines with proper metadata, structured data, and semantic HTML, so our content ranks well in Google and reaches a wider audience.

**Why this priority**: Important for growth and reach, but users can still consume content without perfect SEO.

**Independent Test**: Can be tested using Google Search Console, Lighthouse, and structured data testing tools. Delivers value for organic traffic growth.

**Acceptance Scenarios**:

1. **Given** any page is rendered, **When** I inspect the HTML, **Then** I see proper meta tags (title, description, Open Graph, Twitter Card)
2. **Given** I run Google's Rich Results Test on any article, **When** the test completes, **Then** Schema.org Article markup is detected with valid headline, author, datePublished, and description
3. **Given** I run Lighthouse on any page, **When** the test completes, **Then** SEO score is 95+ with no critical issues
4. **Given** I view the sitemap.xml, **When** I inspect its contents, **Then** all content pages are listed with proper priority and change frequency
5. **Given** a content item appears in multiple sections, **When** I inspect the HTML, **Then** a canonical URL meta tag is specified to prevent duplicate content penalties

---

## Support & Maintenance

**Issue Reporting**: GitHub Issues on techhubms/techhub repository

**Priority Levels**:

- **P0 (Site Down)**: Entire site unreachable → Notification sent immediately, fixed best effort
- **P1 (Critical Bug)**: Major functionality broken (e.g., all videos not loading) → Fixed best effort
- **P2 (High Bug)**: Specific feature broken (e.g., one section not loading) → Fixed when available
- **P3 (Low Bug)**: Minor issues, visual glitches → Backlog

**Important Note**: This is a hobby project maintained in spare time. There are **NO guaranteed SLAs** for issue resolution. Alerts provide awareness, not commitments.

**Bug Triage**: Site owner reviews and prioritizes GitHub Issues

**Monitoring**: Azure Monitor alerts configured to notify site owner immediately of critical issues

---

### User Story 6 - Performance and Responsiveness (Priority: P1)

As a content consumer, I want pages to load instantly and interactions to feel immediate, so I have a smooth browsing experience on any device or connection speed.

**Why this priority**: Critical for user retention - slow sites cause users to leave. Modern web performance is a hygiene factor.

**Independent Test**: Can be tested with Lighthouse, WebPageTest, and Core Web Vitals monitoring. Delivers value through superior UX.

**Acceptance Scenarios**:

1. **Given** I visit any page, **When** I measure time to first paint, **Then** content appears within 1 second
2. **Given** I am on a slow 3G connection, **When** I load a page, **Then** critical content is visible within 3 seconds
3. **Given** I run Lighthouse on any page, **When** the test completes, **Then** Performance score is 95+, LCP < 2.5s, FID < 100ms, CLS < 0.1
4. **Given** I apply client-side filters, **When** I interact with filter controls, **Then** results update within 100ms with no visible lag
5. **Given** I scroll through a long content list, **When** I reach the bottom, **Then** the next batch loads within 500ms
6. **Given** I navigate between pages, **When** I click a link, **Then** the browser back button works correctly and navigation feels instant

---

### User Story 7 - Analytics and Monitoring (Priority: P3)

As a site administrator, I want to track user behavior, performance metrics, and errors, so I can understand how users interact with the site and identify issues proactively.

**Why this priority**: Important for continuous improvement, but not required for users to consume content.

**Independent Test**: Can be tested by triggering events and verifying they appear in analytics dashboards. Delivers value for site operators.

**Acceptance Scenarios**:

1. **Given** a user visits any page, **When** the page loads, **Then** a Google Analytics 4 pageview event is recorded
2. **Given** a user applies filters, **When** they interact with filter controls, **Then** custom events are tracked (search queries, tag clicks, date filters)
3. **Given** an error occurs, **When** the error is logged, **Then** it appears in Application Insights with full stack trace and context
4. **Given** I am monitoring the site, **When** I view the dashboard, **Then** I see Core Web Vitals metrics, API response times, and error rates
5. **Given** the site is under load, **When** traffic spikes, **Then** telemetry data is collected without impacting user experience

---

### Edge Cases

- What happens when a content item has no tags or categories? (Display with "Uncategorized" label, still searchable by text)
- How does the system handle extremely long titles or descriptions? (Truncate with ellipsis, show full text on detail page)
- What happens when a YouTube video is deleted or unavailable? (Show fallback message with link to original URL)
- How does filtering work when no results match? (Display "No results found" with option to clear filters)
- What happens when RSS feed is accessed before content is loaded? (Return cached feed or generate on-demand)
- How does the system handle concurrent filter changes? (Debounce input, cancel in-flight requests, show latest results)
- What happens when a user has JavaScript disabled? (Server-side rendered content works, filters require JavaScript - progressive enhancement)
- How does the site handle very old browsers? (Modern browsers only - IE not supported, graceful degradation for older Chrome/Firefox)
- What happens when content is updated via Git push? (Container redeploy clears IMemoryCache automatically, fresh content loads on next request)
- How does telemetry sampling affect error debugging? (All errors traced at 100%, successful requests sampled at 10% to balance observability with cost)

## Requirements *(mandatory)*

### Functional Requirements

**Content Organization & Display**

- **FR-001**: System MUST organize content into 8 sections (Everything, AI, GitHub Copilot, Azure, ML, .NET, DevOps, Security)
- **FR-002**: System MUST support 5 content collections (News, Blogs, Videos, Community, Roundups)
- **FR-003**: System MUST read configuration from sections.json as single source of truth for site structure
- **FR-004**: System MUST render markdown content to semantic HTML with proper heading hierarchy
- **FR-005**: System MUST support YouTube video embeds via video_id frontmatter field
- **FR-006**: System MUST extract and display excerpts (content before `<!--excerpt_end-->` marker)
- **FR-007**: System MUST support alt-collection frontmatter for content organized in subfolders

**Filtering & Search**

- **FR-008**: System MUST provide client-side date filtering with preset ranges (Last 7 days, Last 30 days, Last 90 days, Last 365 days, All time)
- **FR-009**: System MUST implement infinite scroll pagination with configurable batch sizes (30-50 items per batch, no arbitrary date-based limits)
- **FR-010**: System MUST provide client-side tag filtering with multi-select capability (OR logic)
- **FR-011**: System MUST provide client-side text search across title, description, and tags with debounced input
- **FR-012**: System MUST synchronize filter state with URL query parameters for shareable filtered views
- **FR-013**: System MUST restore filter state when navigating back/forward in browser history

**RSS Feeds**

- **FR-014**: System MUST generate RSS 2.0 feeds for each section (e.g., /ai/feed.xml)
- **FR-015**: System MUST generate a combined RSS feed for all content (/feed.xml)
- **FR-016**: System MUST include RSS feed links in section page headers
- **FR-017**: RSS feeds MUST include title, description, link, author, pubDate, and category elements
- **FR-018**: RSS feeds MUST cache using IMemoryCache with 30-minute absolute expiration

**Performance & UX**

- **FR-019**: System MUST server-side render all initial page content (Blazor SSR) for SEO and fast first paint
- **FR-020**: System MUST use Blazor .NET 8+ unified model with auto-upgrade to WebAssembly for client-side interactivity (filtering, infinite scroll)
- **FR-021**: System MUST implement infinite scroll pagination with 20-item batches
- **FR-022**: System MUST lazy-load images below the fold
- **FR-023**: System MUST cache API responses using IMemoryCache with 1-hour absolute expiration
- **FR-024**: System MUST implement output caching for static content pages

**Accessibility & SEO**

- **FR-025**: System MUST use semantic HTML5 elements (article, section, nav, main, header, footer, time)
- **FR-026**: System MUST include Schema.org Article markup on all content pages
- **FR-027**: System MUST include Schema.org WebSite markup on home page
- **FR-028**: System MUST generate sitemap.xml with all content URLs
- **FR-029**: System MUST implement canonical URL meta tags for content appearing in multiple sections
- **FR-030**: System MUST meet WCAG 2.1 Level AA compliance (color contrast, keyboard navigation, screen reader support)

**Data Management**

- **FR-031**: System MUST read content from markdown files in collections/ directory
- **FR-032**: System MUST parse YAML frontmatter for metadata (title, author, date, categories, tags)
- **FR-033**: System MUST store dates as Unix epoch timestamps internally
- **FR-034**: System MUST handle Europe/Brussels timezone for all date operations
- **FR-035**: System MUST implement repository pattern with file-based implementation for MVP (database migration planned when content volume exceeds ~1000 items)

**Monitoring & Analytics**

- **FR-036**: System MUST integrate Google Analytics 4 with Core Web Vitals tracking
- **FR-037**: System MUST integrate OpenTelemetry for distributed tracing of HTTP requests, repository operations, and cache operations with adaptive sampling (100% errors, 10% success)
- **FR-038**: System MUST send telemetry to Application Insights
- **FR-039**: System MUST implement cookie consent banner for GDPR compliance

**Visual Design & Layout**

- **FR-040**: System MUST use full-width responsive grid layout for content listings (home page, section pages) with 1-3 columns based on viewport width
- **FR-041**: System MUST constrain article/content detail pages to 800px width (matching Jekyll $content-width) for optimal reading experience
- **FR-042**: System MUST implement two-column layout on article detail pages: left sidebar (25-30% width) and main content area (70-75% width)
- **FR-043**: System MUST include in left sidebar (in priority order): quick navigation (table of contents from headings), author information, article metadata (date, tags, category), related articles, social share/back-to-section links
- **FR-044**: System MUST support custom sidebar content for special pages (e.g., GenAI Basics, GenAI Advanced) with page-specific navigation and metadata
- **FR-045**: System MUST maintain visual parity with Jekyll site for top navigation bar (section links + subnavigation dropdowns)
- **FR-046**: System MUST keep section background images with hover highlight effects on home page section cards
- **FR-047**: System MUST use softer, more muted purple backgrounds (darker shades like #7f56d9, #6b4fb8) instead of bright purple (#bd93f9) to reduce visual strain
- **FR-048**: System MUST use soft dark gray text (#333) on off-white backgrounds (#fafafa) for better readability and reduced eye strain
- **FR-049**: System MUST maintain purple accents for interactive elements (links, buttons, hover states) but tone down background usage
- **FR-050**: System MUST implement responsive sidebar behavior on mobile: essential metadata and quick navigation above article, supplementary content (author, related articles) below article

### Key Entities *(include if feature involves data)*

- **Section**: Represents a topic area (AI, GitHub Copilot, etc.) with title, description, URL, category, background image, and collection references
- **Collection**: Represents a content type (News, Blogs, Videos, Community, Roundups) with title, URL, description, and isCustom flag
- **ContentItem**: Represents individual content with ID, title, description, author, date (epoch), collection, categories, tags, rendered HTML, excerpt, optional externalUrl, optional videoId
- **FilterState**: Client-side state tracking active filters (search text, date range, selected tags, selected collections) synchronized with URL parameters
- **RssFeed**: Generated XML document containing channel metadata and collection of items with required RSS 2.0 elements

## Success Criteria *(mandatory)*

### Measurable Outcomes

**Performance Metrics**

- **SC-001**: Pages load with Time to First Byte (TTFB) < 200ms (p95)
- **SC-002**: Largest Contentful Paint (LCP) < 2.5s on desktop, < 3.5s on mobile (p75)
- **SC-003**: First Input Delay (FID) < 100ms (p95)
- **SC-004**: Cumulative Layout Shift (CLS) < 0.1 (p75)
- **SC-005**: Lighthouse Performance score > 95 for all page types
- **SC-006**: Client-side filter operations complete in < 100ms (p95)
- **SC-007**: Infinite scroll batches load in < 500ms (p95)

**User Experience Metrics**

- **SC-008**: Users can find relevant content within 30 seconds of landing on the site
- **SC-009**: 90% of users successfully apply at least one filter on collection pages
- **SC-010**: Mobile users can navigate and consume content without horizontal scrolling
- **SC-011**: Keyboard-only users can access all interactive features

**Accessibility Metrics**

- **SC-012**: WCAG 2.1 Level AA compliance verified by automated testing (Axe, WAVE)
- **SC-013**: Color contrast ratios meet 4.5:1 minimum for normal text
- **SC-014**: All interactive elements have visible focus indicators
- **SC-015**: Screen reader users can navigate site structure and content logically

**SEO Metrics**

- **SC-016**: Lighthouse SEO score > 95 for all pages
- **SC-017**: All content pages indexed by Google within 48 hours of publication
- **SC-018**: Google Rich Results Test validates Schema.org markup on 100% of content pages
- **SC-019**: Sitemap includes all content URLs and updates within 1 hour of new content

**Technical Metrics**

- **SC-020**: API response time < 50ms (p95) for cached requests
- **SC-021**: Build time < 5 minutes for full site generation
- **SC-022**: Zero critical security vulnerabilities in dependencies
- **SC-023**: Code coverage > 80% for all .NET projects
- **SC-024**: No console errors or warnings in production builds

**Operational Metrics**

- **SC-025**: System auto-scales to handle 10,000 concurrent users without degradation (Azure Container Apps: min 1, max 10 instances, scale on HTTP requests + CPU/memory at 80% threshold)
- **SC-026**: Deployment completes in < 10 minutes with zero downtime
- **SC-027**: Error rate < 0.1% of all requests
- **SC-028**: Mean time to recovery (MTTR) < 5 minutes for any incidents

**Visual Design & Usability Metrics**

- **SC-029**: Article content width is exactly 800px on desktop viewports (matching Jekyll $content-width for reading comfort)
- **SC-030**: Sidebar occupies 25-30% of layout width on article pages with clear visual separation from main content
- **SC-031**: Color contrast ratios between purple backgrounds and text meet WCAG AA standards (4.5:1 minimum for normal text, 3:1 for large text)
- **SC-032**: Purple usage is primarily for accents and interactive elements, not dominant background color (measured by pixel count analysis showing <20% bright purple backgrounds)
- **SC-033**: Mobile article pages show metadata and quick navigation above content, supplementary sidebar content below content
- **SC-034**: Table of contents (quick navigation) links scroll smoothly to corresponding article sections with proper offset for fixed headers
- **SC-035**: Hover states on section cards produce visible highlight effect (brightness increase or overlay opacity change) within 100ms

## Assumptions

- Content will continue to be authored as markdown files with YAML frontmatter
- sections.json remains the authoritative configuration source for site structure
- YouTube remains the primary video hosting platform
- Google Analytics 4 is the approved analytics solution
- Azure Container Apps is the approved hosting platform
- No user authentication or authorization required for MVP
- All content is public and does not require access control
- Content publishing workflow remains file-based (no CMS required for MVP)
- Production domain is tech.hub.ms with Azure-managed SSL certificates
- Site owner performs UAT testing using production content
- Modern browsers only (no IE11 support)
- This is a hobby project with no formal SLAs for issue resolution

## Out of Scope

1. **Authentication**: No user authentication or admin login for MVP (future enhancement when needed)
2. **Content CMS**: No content management UI - maintain Git-based markdown workflow for MVP
3. **Comments System**: No commenting or user-generated content
4. **Multilingual Support**: English only (no i18n/l10n)
5. **Legacy URL Redirects**: Greenfield deployment, no backwards compatibility with Jekyll URLs required
6. **Advanced Search**: No full-text search with Azure AI Search or Elasticsearch for MVP (client-side filtering sufficient)
7. **Real-time Updates**: No WebSockets or SignalR for live content updates
8. **CDN**: No Azure Front Door or CDN initially (can add if traffic demands)
9. **Email Notifications**: No email alerts for new content
10. **Social Media Integration**: Beyond Open Graph meta tags
11. **A/B Testing**: No experimentation framework for MVP

### Future Enhancements (No Specific Timeline)

- **Authentication**: Design API auth-ready with Microsoft Entra ID (Azure AD), implement when use cases arise (admin editing, private content, etc.)
- **Headless CMS**: Consider Contentful/Strapi integration if Git workflow becomes limiting
- **Advanced Search**: Add Azure AI Search if content library grows significantly (1000+ items)
- **CDN**: Add Azure Front Door if global traffic demands improve latency

## Dependencies

- Existing Jekyll site serves as reference for content structure and behavior
- Markdown content files in collections/ directory
- sections.json configuration file
- YouTube API availability for video embeds
- Azure subscription for hosting and services
- Google Analytics 4 account (ID: G-95LLB67KJV)

## Related Specifications

This master spec is supported by detailed feature specifications in `/specs/`:

**Foundation** (Implement Phase 1-2, BEFORE major development):

- 001-solution-structure - .NET solution organization ✅
- 002-configuration-management - appsettings.json and environment config 📝
- 003-resilience-error-handling - Polly retry policies, circuit breakers, logging 📝

**Testing Strategy** (Implement Phase 2, alongside foundation):

- 004-unit-testing - xUnit for domain/services ✅
- 005-integration-testing - WebApplicationFactory for API ✅
- 006-component-testing - bUnit for Blazor components 📝
- 007-e2e-testing - Playwright end-to-end tests 📝
- 026-ci-cd-pipeline - GitHub Actions automation 📝

**Core Architecture**:

- 008-api-client - Typed HttpClient for Blazor frontend 📝
- 009-url-routing - URL structure and routing ✅
- 010-section-system - Section/collection architecture ✅
- 011-domain-models - DTOs and models ✅
- 012-repository-pattern - Data access ✅
- 013-api-endpoints - REST API definitions ✅

**User Interface**:

- 014-blazor-components - Reusable UI components ✅
- 015-nlweb-semantic-html - Semantic HTML and accessibility ✅
- 016-visual-design-system - Design tokens and styling ✅
- 017-page-components - Page-level Blazor components ✅

**Features**:

- 018-content-rendering - Markdown to HTML ✅
- 019-filtering-system - Client-side filtering ✅
- 020-infinite-scroll - Progressive loading ✅
- 021-rss-feeds - RSS generation ✅
- 022-search - Text search ✅
- 023-seo - SEO optimization ✅
- 024-google-analytics - GA4 integration ✅

**Infrastructure**:

- 025-azure-resources - Container Apps deployment ✅
- 026-ci-cd-pipeline - GitHub Actions automation 📝
- dotnet-migration - This overall migration spec ✅

**Legend**: ✅ Complete | 📝 Placeholder (needs detailed requirements)

## Clarifications

### Session 2026-01-02

- Q: Data persistence strategy for MVP - file-based vs. database? → A: Start with file-based storage for MVP, repository pattern allows seamless database swap when content volume demands it (e.g., >1000 items)
- Q: Blazor deployment model - SSR + WASM architecture? → A: Use Blazor .NET 8+ unified model (SSR for initial render, auto-upgrade to WebAssembly for interactivity) in a single project
- Q: Caching mechanism and invalidation strategy? → A: In-memory cache (IMemoryCache) with sliding/absolute expiration - cache cleared on container restart (acceptable for Git-based content updates)
- Q: Auto-scaling strategy and resource limits? → A: Azure Container Apps auto-scale: min 1, max 10 instances, scale on HTTP requests + CPU/memory (80% threshold)
- Q: OpenTelemetry tracing scope and sampling strategy? → A: Trace HTTP requests, repository operations, cache hits/misses with adaptive sampling (100% errors, 10% success in production)

## References

- [Tech Hub Constitution](/.specify/memory/constitution.md)
- [Blazor Documentation](https://learn.microsoft.com/aspnet/core/blazor/)
- [.NET Aspire Documentation](https://learn.microsoft.com/dotnet/aspire/)
- [Schema.org Article](https://schema.org/Article)
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
