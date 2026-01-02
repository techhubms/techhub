# Feature Specification: URL Routing & Structure

**Feature Branch**: `002-url-routing`  
**Created**: 2026-01-02  
**Status**: Draft  
**Input**: User description: "Define complete URL structure for Tech Hub .NET migration using hierarchical pattern /section/collection/YYYY-MM-DD-slug without .html extensions. Support multi-category items accessible from multiple section URLs with canonical URL determined by category priority order (GitHub-Copilot > AI > ML > Azure > .NET > Security > DevOps). Include 'all' section in URL paths. Breadcrumbs deferred for future implementation. Define Blazor routing configuration, URL generation patterns, and Schema.org canonical link handling."

## User Scenarios & Testing

### User Story 1 - Direct URL Access (Priority: P1)

Users access content via direct URLs shared on social media, email, or bookmarks, reaching correct content immediately with clean, readable URLs that clearly indicate content hierarchy.

**Why this priority**: URLs are shared constantly. Clean URLs improve click-through rates and user trust.

**Independent Test**: Can be fully tested by navigating to any URL directly in browser and verifying correct content loads with proper routing.

**Acceptance Scenarios**:

1. **Given** user has URL `/github-copilot/videos/2026-01-02-chat-in-ide`, **When** they navigate to it, **Then** they see the correct video content with section and collection context
2. **Given** user bookmarks `/ai/news/2025-12-15-azure-openai-update`, **When** they return via bookmark, **Then** they see same content with AI section navigation active
3. **Given** user shares `/all/roundups/2026-01-01-weekly-summary`, **When** recipient clicks link, **Then** they see roundup in "All" section context

---

### User Story 2 - Multi-Category Content Discovery (Priority: P1)

Users discover same content item through different category entry points based on their interests, with each URL providing appropriate section context while linking to canonical version for SEO.

**Why this priority**: Content is tagged with multiple categories. Users should find content regardless of which section they browse.

**Independent Test**: Can be tested by navigating to same item via different category URLs and verifying content loads with appropriate section context.

**Acceptance Scenarios**:

1. **Given** video tagged `["GitHub Copilot", "Azure"]`, **When** user browses GitHub Copilot videos, **Then** they see item at `/github-copilot/videos/2026-01-02-copilot-azure`
2. **Given** same video, **When** user browses Azure videos, **Then** they also find it at `/azure/videos/2026-01-02-copilot-azure`
3. **Given** item viewed from any category URL, **When** page HTML is generated, **Then** canonical link points to highest priority category URL (GitHub Copilot in this case)

---

### User Story 3 - URL Readability and Sharing (Priority: P2)

Users understand content topic from URL alone before clicking, improving decision-making about whether to click shared links and increasing engagement.

**Why this priority**: URL readability affects click-through rates on social media and search results. Clean URLs build trust.

**Independent Test**: Can be tested by showing URLs to users and verifying they can accurately predict content type and topic.

**Acceptance Scenarios**:

1. **Given** user sees URL `/github-copilot/news/2026-01-15-new-features`, **When** they read it, **Then** they understand it's GitHub Copilot news from January 15, 2026 about new features
2. **Given** developer sees `/dotnet/blogs/2026-01-20-aspire-patterns`, **When** they evaluate whether to click, **Then** URL clearly indicates .NET/coding section, blog content, date, and topic
3. **Given** URL without `.html` extension, **When** users share it, **Then** it appears modern and professional (not legacy)

---

### User Story 4 - SEO and Search Engine Indexing (Priority: P1)

Search engines index content with proper URL hierarchy, understanding section and collection relationships while respecting canonical URLs to avoid duplicate content penalties.

**Why this priority**: SEO drives majority of traffic. Proper URL structure and canonical links critical for search rankings.

**Independent Test**: Can be tested using Google Search Console URL Inspection tool and verifying canonical URL recognition.

**Acceptance Scenarios**:

1. **Given** item exists at multiple category URLs, **When** Google crawls all versions, **Then** it recognizes canonical URL via `<link rel="canonical">` tag
2. **Given** hierarchical URL structure, **When** search engine analyzes site, **Then** it understands section > collection > item relationship
3. **Given** URL includes date prefix, **When** search results display, **Then** date in URL reinforces content freshness for ranking

---

### User Story 5 - Navigation Context Preservation (Priority: P2)

Users navigate between sections and items while maintaining clear context of where they are in site hierarchy through URL structure and active navigation states.

**Why this priority**: Users need to understand their location in site. URL structure helps maintain mental model.

**Independent Test**: Can be tested by navigating through site and verifying URL always reflects current section/collection context.

**Acceptance Scenarios**:

1. **Given** user on `/ai/videos` collection page, **When** they click on item, **Then** URL becomes `/ai/videos/2026-01-02-item-slug` maintaining AI context
2. **Given** user browses `/github-copilot/news`, **When** they view item details, **Then** section navigation shows GitHub Copilot as active
3. **Given** user on item accessible from multiple sections, **When** they arrived via `/azure/videos/item`, **Then** Azure navigation is active despite canonical URL being different section

---

### Edge Cases

- What happens when item has no categories assigned? **Answer**: Default to "all" section canonical URL
- How to handle item title changes (slug changes)? **Answer**: Slug is immutable once published, tied to date-slug identifier
- What if two items published same day have identical slugs? **Answer**: Append incrementing number: `2026-01-02-title`, `2026-01-02-title-2`
- How to handle removed categories from item? **Answer**: Old category URLs return 404, only current category URLs work
- What if section name changes in sections.json? **Answer**: URLs break (sections.json is stable, changes require site rebuild and consideration)

## Requirements

### Functional Requirements

**URL Structure Patterns**:

- **FR-001**: Home page MUST use root URL `/` (no trailing slash)
- **FR-002**: Section index pages MUST use pattern `/{section}` (e.g., `/github-copilot`, `/ai`, `/all`)
- **FR-003**: Collection pages MUST use pattern `/{section}/{collection}` (e.g., `/github-copilot/videos`, `/all/news`)
- **FR-004**: Item detail pages MUST use pattern `/{section}/{collection}/{YYYY-MM-DD-slug}` (e.g., `/github-copilot/videos/2026-01-02-chat-in-ide`)
- **FR-005**: URLs MUST NOT include `.html` file extensions (modern clean URLs)
- **FR-006**: Roundup detail pages MUST use pattern `/all/roundups/{YYYY-MM-DD-slug}` (roundups only in "all" section)
- **FR-007**: Custom pages MUST use pattern `/{section}/{page-name}` (e.g., `/github-copilot/features`, `/about`)

**Date and Slug Format**:

- **FR-008**: Date prefix in item URLs MUST use format `YYYY-MM-DD` (ISO 8601 date)
- **FR-009**: Slug MUST be lowercase with hyphens separating words (no underscores, spaces, or special characters)
- **FR-010**: Slug MUST be derived from item title using kebab-case conversion (e.g., "Chat in IDE" → "chat-in-ide")
- **FR-011**: Date-slug combination MUST be unique within collection (enforce at content creation time)
- **FR-012**: If slug collision occurs, MUST append incrementing number: `-2`, `-3`, etc.

**Multi-Category URL Handling**:

- **FR-013**: Items with multiple categories MUST be accessible from URL for EACH category
- **FR-014**: Item tagged `["GitHub Copilot", "Azure"]` MUST be accessible from both `/github-copilot/videos/2026-01-02-slug` AND `/azure/videos/2026-01-02-slug`
- **FR-015**: All category URLs for same item MUST return HTTP 200 (not redirects)
- **FR-016**: System MUST determine canonical URL based on category priority order
- **FR-017**: Category priority order MUST be: GitHub-Copilot > AI > ML > Azure > Coding (Dotnet) > Security > DevOps > All

**Canonical URL Rules**:

- **FR-018**: Every page MUST include `<link rel="canonical" href="...">` in HTML `<head>`
- **FR-019**: For items with multiple categories, canonical URL MUST use highest priority category from item's category list
- **FR-020**: If item tagged `["Azure", "GitHub Copilot", "AI"]`, canonical URL MUST be `/github-copilot/...` (highest priority)
- **FR-021**: Canonical URL MUST be absolute URL including protocol and domain (e.g., `https://tech.hub.ms/github-copilot/videos/2026-01-02-slug`)
- **FR-022**: Pages accessed via non-canonical URL MUST still include canonical link pointing to highest-priority URL

**RSS Feed URLs**:

- **FR-023**: Site-wide RSS feed MUST be at `/feed.xml`
- **FR-024**: Section-specific RSS feeds MUST use pattern `/{section}/feed.xml`
- **FR-025**: Roundups-only RSS feed MUST be at `/roundups/feed.xml`
- **FR-026**: RSS feed URLs MUST keep `.xml` extension (standard RSS convention)

**Special Pages**:

- **FR-027**: About page MUST be at `/about` (no section prefix)
- **FR-028**: 404 error page MUST be at `/404` with appropriate HTTP status
- **FR-029**: Sitemap MUST be at `/sitemap.xml`
- **FR-030**: Robots.txt MUST be at `/robots.txt`

**Blazor Routing Configuration**:

- **FR-031**: System MUST use Blazor SSR routing for initial page load
- **FR-032**: System MUST support client-side routing for navigation within site (Blazor WASM)
- **FR-033**: System MUST preserve URL structure when switching between SSR and WASM rendering
- **FR-034**: System MUST handle trailing slashes consistently (redirect `/section/` to `/section`)

**URL Generation**:

- **FR-035**: System MUST provide UrlHelper service for generating URLs from item metadata
- **FR-036**: Components MUST NOT hardcode URLs - use UrlHelper for all link generation
- **FR-037**: UrlHelper MUST accept item and optional section parameter to generate category-specific URL
- **FR-038**: UrlHelper MUST provide method to get canonical URL for any item

### Key Entities

**URL Pattern** represents URL structure definition:

- Pattern template (e.g., `/{section}/{collection}/{date-slug}`)
- Route parameters (section, collection, date, slug)
- HTTP status (200, 301, 404)
- Canonical rules

**Category Priority** represents section priority for canonical URL determination:

- Section key (github-copilot, ai, ml, etc.)
- Priority rank (1-8, lower is higher priority)
- Display name

**Item URL** represents specific URL for content item:

- Full URL path
- Section context
- Collection context
- Date-slug identifier
- Is canonical (boolean)
- Canonical URL reference

## Success Criteria

### Measurable Outcomes

- **SC-001**: 100% of content items accessible via direct URL without errors
- **SC-002**: Items with N categories are accessible from N different section URLs (all return HTTP 200)
- **SC-003**: 100% of pages include valid canonical URL in HTML head
- **SC-004**: Canonical URLs follow priority order with zero errors (automated validation)
- **SC-005**: URL format matches specifications with zero deviations (no .html, correct date format, kebab-case slugs)
- **SC-006**: Google Search Console recognizes canonical URLs correctly (zero duplicate content warnings)
- **SC-007**: Users can determine content type from URL alone (user testing: 90%+ accuracy)
- **SC-008**: Zero hardcoded URLs in component code (code review verification)

## Acceptance Criteria

### URL Structure Compliance

- [ ] Home page accessible at `/` (root URL, no trailing slash)
- [ ] Section index pages use `/{section}` pattern (e.g., `/github-copilot`, `/ai`, `/all`)
- [ ] Collection pages use `/{section}/{collection}` pattern (e.g., `/github-copilot/videos`)
- [ ] Item detail pages use `/{section}/{collection}/{YYYY-MM-DD-slug}` pattern
- [ ] URLs contain NO `.html` file extensions
- [ ] Roundup detail pages accessible at `/all/roundups/{YYYY-MM-DD-slug}`
- [ ] Custom pages accessible at `/{section}/{page-name}` (e.g., `/github-copilot/features`)

### Date and Slug Format Validation

- [ ] Date prefix in item URLs uses `YYYY-MM-DD` format (ISO 8601)
- [ ] Slug is lowercase with hyphens separating words (no underscores, spaces, special characters)
- [ ] Slug auto-generated from title using kebab-case conversion
- [ ] Date-slug combination unique within collection (enforced at creation)
- [ ] Slug collisions resolved with incrementing numbers (`-2`, `-3`)
- [ ] Slug generation algorithm removes non-alphanumeric characters except hyphens
- [ ] Multiple consecutive hyphens collapsed to single hyphen

### Multi-Category URL Handling

- [ ] Items with multiple categories accessible from URL for EACH category
- [ ] Item tagged `["GitHub Copilot", "Azure"]` accessible from `/github-copilot/videos/...` AND `/azure/videos/...`
- [ ] All category URLs for same item return HTTP 200 (not 301/302 redirects)
- [ ] System determines canonical URL based on category priority order
- [ ] Category priority order: GitHub-Copilot > AI > ML > Azure > Coding > Security > DevOps > All
- [ ] Item with `["Azure", "GitHub Copilot", "AI"]` has canonical URL using GitHub Copilot section
- [ ] Item accessible from non-canonical category URL still loads correctly with appropriate section context

### Canonical URL Implementation

- [ ] Every page includes `<link rel="canonical" href="...">` in HTML `<head>`
- [ ] For items with multiple categories, canonical URL uses highest priority category
- [ ] Canonical URL is absolute URL including protocol and domain (e.g., `https://tech.hub.ms/...`)
- [ ] Pages accessed via non-canonical URL include canonical link pointing to highest-priority URL
- [ ] Canonical URL algorithm correctly maps categories to priority ranks
- [ ] Items with no categories use `/all/...` as canonical URL
- [ ] Home page canonical URL is `https://tech.hub.ms/`

### RSS Feed URLs

- [ ] Site-wide RSS feed accessible at `/feed.xml`
- [ ] Section-specific RSS feeds accessible at `/{section}/feed.xml`
- [ ] Roundups-only RSS feed accessible at `/roundups/feed.xml`
- [ ] RSS feed URLs retain `.xml` extension (standard convention)
- [ ] RSS feed Content-Type header set to `application/xml` or `application/rss+xml`

### Special Page URLs

- [ ] About page accessible at `/about` (no section prefix)
- [ ] 404 error page accessible at `/404` with HTTP status 404
- [ ] Sitemap accessible at `/sitemap.xml`
- [ ] Robots.txt accessible at `/robots.txt`
- [ ] Custom section pages load correctly (features, levels-of-enlightenment, ai-to-z)

### Blazor Routing Configuration

- [ ] Blazor page components have `@page` directives with correct route templates
- [ ] Route parameters defined for section, collection, date-slug
- [ ] Optional parameters used where appropriate (e.g., section parameter on collection pages)
- [ ] Route constraints applied (e.g., date format validation)
- [ ] Fallback route handles 404 cases
- [ ] Trailing slashes removed via middleware or routing configuration

### URL Generation and Helpers

- [ ] UrlHelper service provides GetItemUrl(item, section) method
- [ ] UrlHelper service provides GetCanonicalUrl(item) method
- [ ] UrlHelper service provides GetSectionUrl(sectionKey) method
- [ ] UrlHelper service provides GetCollectionUrl(sectionKey, collectionKey) method
- [ ] UrlHelper service provides GetRssUrl(sectionKey) method
- [ ] UrlHelper service provides GetAbsoluteUrl(relativePath) method
- [ ] URL generation results cached for performance
- [ ] No hardcoded URLs in Razor components (all use UrlHelper)

### Navigation Context

- [ ] Current section highlighted in navigation based on URL
- [ ] Items viewed via non-canonical URL show correct section as active
- [ ] Breadcrumbs deferred (not required in this spec)
- [ ] URL structure provides clear hierarchy (section > collection > item)

### Error Handling and Edge Cases

- [ ] Nonexistent section URLs return 404
- [ ] Nonexistent collection URLs return 404
- [ ] Nonexistent item URLs return 404
- [ ] Invalid date formats in URLs return 404
- [ ] Malformed slugs handled gracefully (404)
- [ ] Items with removed categories return 404 for old category URLs
- [ ] Items with no categories fallback to "all" section

### SEO and Schema.org

- [ ] Canonical link tag present on all pages
- [ ] Google Search Console URL Inspection recognizes canonical URLs
- [ ] No duplicate content warnings in Search Console
- [ ] Sitemap includes all URL variations (not just canonical)
- [ ] Absolute URLs used in sitemap and RSS feeds
- [ ] URLs are case-sensitive (lowercase enforced)

### Performance and Caching

- [ ] URL generation cached (items don't change URLs once published)
- [ ] Route matching optimized for common patterns
- [ ] No database queries required for URL generation (uses in-memory data)
- [ ] Static URLs pre-generated during build/startup when possible

## Assumptions

1. **Sections.json Stable**: Section keys in sections.json are stable and won't change frequently
2. **Category Assignment**: Content items have at least one category assigned (fallback to "all" if none)
3. **Slug Uniqueness**: Content creation process ensures slug uniqueness within date+collection
4. **Domain Name**: Production domain is `tech.hub.ms` for canonical URLs
5. **HTTPS Only**: All canonical URLs use HTTPS protocol
6. **Immutable Slugs**: Once published, item slug and date never change
7. **Section Key to URL**: Section keys in sections.json directly map to URL segments (github-copilot → /github-copilot)

## Out of Scope

1. **Breadcrumbs**: Deferred to future implementation (not in scope now)
2. **URL Redirects**: No 301 redirects from old Jekyll URLs (greenfield, no backwards compatibility)
3. **Custom Slugs**: Not supporting manual slug override (always auto-generated from title)
4. **Version URLs**: Not supporting multiple versions of same content at different URLs
5. **Query Parameters**: Filtering uses query params but not part of core URL routing spec (covered in filtering spec)
6. **Localization**: No language-specific URLs (e.g., `/en/`, `/es/`) - English only
7. **Pagination URLs**: Infinite scroll means no `/page/2` style URLs

## Dependencies

1. **Sections Configuration**: sections.json defines available sections and collections
2. **Content Metadata**: Item frontmatter provides categories for multi-URL generation
3. **NLWeb Semantic HTML Spec**: Canonical link tag implementation details
4. **Domain Configuration**: Production domain name for absolute canonical URLs
5. **Filtering System Spec**: Query parameter handling for filters (separate concern)

## Appendix: URL Mapping Examples

### Home and Sections

| Page Type      | URL              | Notes         |
| -------------- | ---------------- | ------------- |
| Home           | `/`              | Root page     |
| All Section    | `/all`           | Section index |
| GitHub Copilot | `/github-copilot`| Section index |
| AI Section     | `/ai`            | Section index |
| ML Section     | `/ml`            | Section index |
| Azure Section  | `/azure`         | Section index |
| .NET Section   | `/dotnet`        | Section index |
| Security       | `/security`      | Section index |
| DevOps         | `/devops`        | Section index |

### Collections

| Collection | Section        | URL                        |
| ---------- | -------------- | -------------------------- |
| News       | All            | `/all/news`                |
| News       | GitHub Copilot | `/github-copilot/news`     |
| Videos     | GitHub Copilot | `/github-copilot/videos`   |
| Blogs      | AI             | `/ai/blogs`                |
| Community  | Azure          | `/azure/community`         |
| Roundups   | All            | `/all/roundups`            |

### Item Examples

| Item | Categories | URL | Canonical |
| ---- | ---------- | --- | --------- |
| Chat in IDE video | `["GitHub Copilot"]` | `/github-copilot/videos/2026-01-02-chat-in-ide` | `/github-copilot/videos/2026-01-02-chat-in-ide` |
| Azure OpenAI news (Azure path) | `["Azure", "AI"]` | `/azure/news/2025-12-15-openai-update` | `/ai/news/2025-12-15-openai-update` |
| Azure OpenAI news (AI path) | `["Azure", "AI"]` | `/ai/news/2025-12-15-openai-update` | `/ai/news/2025-12-15-openai-update` (canonical, AI priority) |
| Copilot + Azure (GitHub Copilot path) | `["GitHub Copilot", "Azure", "AI"]` | `/github-copilot/blogs/2026-01-10-copilot-azure` | `/github-copilot/blogs/2026-01-10-copilot-azure` (canonical, highest priority) |
| Copilot + Azure (Azure path) | `["GitHub Copilot", "Azure", "AI"]` | `/azure/blogs/2026-01-10-copilot-azure` | `/github-copilot/blogs/2026-01-10-copilot-azure` |
| Copilot + Azure (AI path) | `["GitHub Copilot", "Azure", "AI"]` | `/ai/blogs/2026-01-10-copilot-azure` | `/github-copilot/blogs/2026-01-10-copilot-azure` |
| Weekly roundup | `[]` (no categories) | `/all/roundups/2026-01-01-weekly-summary` | `/all/roundups/2026-01-01-weekly-summary` |

### RSS Feeds

| Feed           | URL                        |
| -------------- | -------------------------- |
| Everything     | `/feed.xml`                |
| GitHub Copilot | `/github-copilot/feed.xml` |
| AI             | `/ai/feed.xml`             |
| Azure          | `/azure/feed.xml`          |
| Roundups       | `/roundups/feed.xml`       |

### Special Pages

| Page                           | URL                                      |
| ------------------------------ | ---------------------------------------- |
| About                          | `/about`                                 |
| GitHub Copilot Features        | `/github-copilot/features`               |
| Levels of Enlightenment        | `/github-copilot/levels-of-enlightenment`|
| AI A-to-Z                      | `/ai/ai-to-z`                            |
| Sitemap                        | `/sitemap.xml`                           |
| Robots                         | `/robots.txt`                            |
| 404                            | `/404`                                   |

## Appendix: Category Priority Reference

**Priority Order** (for canonical URL determination):

1. **GitHub-Copilot** (github-copilot)
2. **AI** (ai)
3. **ML** (ml)
4. **Azure** (azure)
5. **Coding** (dotnet)
6. **Security** (security)
7. **DevOps** (devops)
8. **All** (all)

**Algorithm**: Given item with categories `["Azure", "GitHub Copilot", "AI"]`:

1. Extract category keys: `["azure", "github-copilot", "ai"]`
2. Map to priority ranks: `[4, 1, 2]`
3. Select minimum rank: `1` (GitHub Copilot)
4. Canonical URL uses: `/github-copilot/{collection}/{date-slug}`

## Appendix: URL Generation Patterns

### UrlHelper Service Interface

```text
GetItemUrl(item, section = null)
  → Returns: section-specific URL or canonical if section not provided

GetCanonicalUrl(item)
  → Returns: Absolute canonical URL based on highest priority category

GetSectionUrl(sectionKey)
  → Returns: /section

GetCollectionUrl(sectionKey, collectionKey)
  → Returns: /section/collection

GetRssUrl(sectionKey = null)
  → Returns: /feed.xml or /section/feed.xml

GetAbsoluteUrl(relativePath)
  → Returns: https://tech.hub.ms{relativePath}
```

### Slug Generation Algorithm

```text
GenerateSlug(title, date, collection):
  1. Convert title to lowercase
  2. Replace spaces with hyphens
  3. Remove non-alphanumeric except hyphens
  4. Collapse multiple hyphens to single
  5. Trim hyphens from start/end
  6. Check uniqueness in collection for date
  7. If collision, append -2, -3, etc.
  8. Return slug
```

### Example Component Usage

```text
// In Razor component
<a href="@UrlHelper.GetItemUrl(item, CurrentSection)">
  @item.Title
</a>

// Canonical link tag
<link rel="canonical" href="@UrlHelper.GetCanonicalUrl(item)" />
```

## Appendix: Implementation Notes

1. **Route Templates**: Define Blazor `@page` directives with route parameters
2. **URL Consistency**: Always remove trailing slashes via middleware
3. **Case Sensitivity**: URLs are case-sensitive, always use lowercase
4. **Canonical Enforcement**: Every page component must include canonical link
5. **404 Handling**: Non-existent category combinations return 404
6. **Performance**: Cache URL generation results (items don't change URLs)
7. **Testing**: Generate all possible URLs for all items, verify HTTP 200
8. **Sitemap Generation**: Include all URL variations in sitemap (not just canonical)

