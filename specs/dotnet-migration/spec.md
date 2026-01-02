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
5. **Given** a content item appears in multiple sections, **When** I inspect the HTML, **Then** a canonical URL is specified to prevent duplicate content penalties

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

## Requirements *(mandatory)*

### Functional Requirements

**Content Organization & Display**

- **FR-001**: System MUST organize content into 7 sections (AI, GitHub Copilot, Azure, ML, .NET, DevOps, Security)
- **FR-002**: System MUST support 5 content collections (News, Blogs, Videos, Community, Roundups)
- **FR-003**: System MUST read configuration from sections.json as single source of truth for site structure
- **FR-004**: System MUST render markdown content to semantic HTML with proper heading hierarchy
- **FR-005**: System MUST support YouTube video embeds via video_id frontmatter field
- **FR-006**: System MUST extract and display excerpts (content before `<!--excerpt_end-->` marker)
- **FR-007**: System MUST support alt-collection frontmatter for content organized in subfolders

**Filtering & Search**

- **FR-008**: System MUST provide client-side date filtering with preset ranges (Last 7 days, Last 30 days, Last 90 days, Last 365 days, All time)
- **FR-009**: System MUST implement "20 + Same-Day" rule for date filtering (show 20 most recent + any others from same day as 20th item)
- **FR-010**: System MUST provide client-side tag filtering with multi-select capability (OR logic)
- **FR-011**: System MUST provide client-side text search across title, description, and tags with debounced input
- **FR-012**: System MUST synchronize filter state with URL query parameters for shareable filtered views
- **FR-013**: System MUST restore filter state when navigating back/forward in browser history

**RSS Feeds**

- **FR-014**: System MUST generate RSS 2.0 feeds for each section (e.g., /ai/feed.xml)
- **FR-015**: System MUST generate a combined RSS feed for all content (/feed.xml)
- **FR-016**: System MUST include RSS feed links in section page headers
- **FR-017**: RSS feeds MUST include title, description, link, author, pubDate, and category elements
- **FR-018**: RSS feeds MUST cache for 30 minutes to reduce server load

**Performance & UX**

- **FR-019**: System MUST server-side render all initial page content (Blazor SSR) for SEO and fast first paint
- **FR-020**: System MUST use Blazor WebAssembly for client-side interactivity (filtering, infinite scroll)
- **FR-021**: System MUST implement infinite scroll pagination with 20-item batches
- **FR-022**: System MUST lazy-load images below the fold
- **FR-023**: System MUST cache API responses in-memory for 1 hour
- **FR-024**: System MUST implement output caching for static content pages

**Accessibility & SEO**

- **FR-025**: System MUST use semantic HTML5 elements (article, section, nav, main, header, footer, time)
- **FR-026**: System MUST include Schema.org Article markup on all content pages
- **FR-027**: System MUST include Schema.org WebSite markup on home page
- **FR-028**: System MUST generate sitemap.xml with all content URLs
- **FR-029**: System MUST implement canonical URLs for content appearing in multiple sections
- **FR-030**: System MUST meet WCAG 2.1 Level AA compliance (color contrast, keyboard navigation, screen reader support)

**Data Management**

- **FR-031**: System MUST read content from markdown files in collections/ directory
- **FR-032**: System MUST parse YAML frontmatter for metadata (title, author, date, categories, tags)
- **FR-033**: System MUST store dates as Unix epoch timestamps internally
- **FR-034**: System MUST handle Europe/Brussels timezone for all date operations
- **FR-035**: System MUST support repository pattern with file-based implementation (database-ready architecture)

**Monitoring & Analytics**

- **FR-036**: System MUST integrate Google Analytics 4 with Core Web Vitals tracking
- **FR-037**: System MUST integrate OpenTelemetry for distributed tracing
- **FR-038**: System MUST send telemetry to Application Insights
- **FR-039**: System MUST implement cookie consent banner for GDPR compliance

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

- **SC-025**: System auto-scales to handle 10,000 concurrent users without degradation
- **SC-026**: Deployment completes in < 10 minutes with zero downtime
- **SC-027**: Error rate < 0.1% of all requests
- **SC-028**: Mean time to recovery (MTTR) < 5 minutes for any incidents

## Assumptions

- Content will continue to be authored as markdown files with YAML frontmatter
- sections.json remains the authoritative configuration source for site structure
- YouTube remains the primary video hosting platform
- Google Analytics 4 is the approved analytics solution
- Azure Container Apps is the approved hosting platform
- No user authentication or authorization required for MVP
- All content is public and does not require access control
- Content publishing workflow remains file-based (no CMS required for MVP)

## Out of Scope

- User authentication and authorization
- Content management UI (content remains file-based)
- Comments or user-generated content
- Multi-language support (English only)
- Email notifications for new content
- Social media integration beyond Open Graph meta tags
- Advanced search features (fuzzy matching, autocomplete, search suggestions)
- A/B testing or experimentation framework
- Real-time content updates (webhook-based cache invalidation)

## Dependencies

- Existing Jekyll site serves as reference for content structure and behavior
- Markdown content files in collections/ directory
- sections.json configuration file
- YouTube API availability for video embeds
- Azure subscription for hosting and services
- Google Analytics 4 account (ID: G-95LLB67KJV)

## Related Specifications

This master spec is supported by detailed feature specifications in `/specs/`:

**Core Architecture**:
- api-endpoints - REST API endpoint definitions
- blazor-components - Reusable UI components
- content-rendering - Markdown to HTML rendering
- dependency-injection - Service registration patterns
- domain-models - Data structures and entities
- markdown-processing - Markdig configuration
- repository-pattern - Data access layer

**Features**:
- filtering-system - Client-side filtering logic
- google-analytics - GA4 integration
- rss-feeds - RSS generation
- search - Text search implementation
- section-system - Section/collection architecture
- seo - SEO optimization features

**Infrastructure**:
- azure-resources - Azure Container Apps deployment
- solution-structure - .NET project organization

**Testing**:
- integration-testing - API integration tests
- unit-testing - xUnit test strategies

## References

- [Tech Hub Constitution](/.specify/memory/constitution.md)
- [.NET Migration Plan](/docs/migration/dotnet-migration-plan.md)
- [Current Site Analysis](/specs/current-site-analysis.md)
- [Clarifications Needed](/docs/migration/clarification-needed.md)
- [Blazor Documentation](https://learn.microsoft.com/aspnet/core/blazor/)
- [.NET Aspire Documentation](https://learn.microsoft.com/dotnet/aspire/)
- [Schema.org Article](https://schema.org/Article)
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)

