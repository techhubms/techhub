# SEO (Search Engine Optimization)

Tech Hub implements SEO best practices to ensure content is discoverable and properly indexed by search engines.

## Server-Side Rendering (SSR)

All content is server-side rendered via Blazor's global InteractiveServer mode with prerendering enabled. This ensures:

- Search engines see complete HTML content
- Fast initial page load (prerendered HTML delivered before SignalR connects)
- Proper meta tags in initial response
- Social media previews work correctly

See [render-modes.md](render-modes.md) for the render mode architecture and PersistentComponentState pattern.

## SeoMetaTags Component

All SEO head tags are rendered by the `SeoMetaTags` shared component (`src/TechHub.Web/Components/SeoMetaTags.razor`). Pages use this component instead of writing `<HeadContent>` blocks directly.

The component emits:

- `<meta name="description">` — truncated to 160 characters, HTML stripped
- Open Graph tags (`og:type`, `og:title`, `og:description`, `og:url`, `og:site_name`, `og:locale`)
- Article-specific Open Graph properties (`article:published_time`, `article:author`, `article:section`, `article:tag`) for Article and Video content types
- Twitter Card tags (`twitter:card`, `twitter:title`, `twitter:description`, `twitter:site`)
- JSON-LD structured data (schema type depends on `ContentType` parameter)
- A second `BreadcrumbList` JSON-LD block when the `Breadcrumbs` parameter contains at least two items
- `<link rel="canonical">` — normalized (lowercased, query-stripped) canonical URL
- `<link rel="alternate" type="application/rss+xml">` — RSS autodiscovery (when `RssFeedUrl` parameter is set)

**Important**: All SEO `<HeadContent>` MUST be rendered from page components, not from the layout. Blazor's `HeadOutlet @rendermode="InteractiveServer"` does not render layout `HeadContent` during SSR prerendering. Only `HeadContent` from page-level components (inside `@Body`) is included in the prerendered HTML that search engines see.

### Content Types

The `SeoContentType` enum controls which JSON-LD schema is generated and the Open Graph `og:type` value:

| Content type | JSON-LD schema | `og:type` | Used by |
|---|---|---|---|
| `Website` | `WebSite` | `website` | Homepage |
| `Article` | `Article` | `article` | Content detail pages (non-video) |
| `Video` | `VideoObject` | `article` | Video content items |
| `Collection` | `CollectionPage` | `website` | Section and collection listing pages |

### JSON-LD Schemas

**Article**: includes `headline`, `description`, `url`, `publisher`, `author` (when present), `datePublished`, `dateModified`, `keywords` (up to 20), and `articleSection` (when present).

**VideoObject**: includes `name`, `description`, `publisher`, `uploadDate`, `contentUrl` (external video URL), and `embedUrl` (YouTube embed URL derived automatically from watch/youtu.be URLs).

**CollectionPage**: includes `name`, `description`, `url`, and `publisher`.

**WebSite**: includes `name`, `description`, and `url`.

**BreadcrumbList**: generated alongside the main schema when at least two `BreadcrumbItem` values are passed. Each item carries `position`, `name`, and an absolute `item` URL.

## Page Titles

Each page has a descriptive title using `<PageTitle>`. Title patterns:

- **Homepage**: `Tech Hub - Microsoft Developer Resources`
- **Section**: `AI - Tech Hub`
- **Collection**: `News - AI - Tech Hub`
- **Content Item**: `Article Title - Tech Hub`

## Meta Description

The `SeoMetaTags` component generates the meta description from the `Description` parameter. Descriptions are automatically stripped of HTML tags, whitespace-normalized, and truncated to 160 characters at a word boundary with an ellipsis appended when truncated.

## Canonical URLs

Every page that uses `SeoMetaTags` automatically receives a canonical URL. The component computes the canonical URL by normalizing the current path (lowercased, query parameters stripped) and combining it with the base URI. Individual pages do not need to add their own canonical tags.

## Open Graph Tags

Open Graph tags are emitted by the `SeoMetaTags` component for every page. Always present: `og:type`, `og:title`, `og:description`, `og:url`, `og:site_name`, `og:locale`. Content items and videos additionally receive `article:published_time`, `article:author`, `article:section`, and per-tag `article:tag` properties.

Note: `og:image` is not currently set. See the image SEO spec for the planned approach.

## Twitter Cards

Twitter Card tags are emitted by the `SeoMetaTags` component. All pages use `summary` card type. Always present: `twitter:card`, `twitter:title`, `twitter:description`, `twitter:site` (`@Microsoft`).

Note: `twitter:image` is not currently set. See the image SEO spec for the planned approach.

## robots.txt

The `robots.txt` file is served as a static file from `src/TechHub.Web/wwwroot/robots.txt`. It allows all crawlers while blocking internal framework paths, the API, and health endpoints:

- `/_blazor` — Blazor SignalR negotiation traffic
- `/_framework` — .NET framework assets
- `/api/` — Internal API (not meant for crawlers)
- `/swagger/` — API documentation UI
- `/health` and `/alive` — Health check endpoints

The file also points crawlers to `https://tech.hub.ms/sitemap.xml`.

## XML Sitemap

The sitemap is available at `GET /sitemap.xml` on the web domain (proxied from `GET /api/sitemap`). It returns `application/xml` and includes:

**Static entries:**

| URL | Priority |
|-----|----------|
| Homepage | 1.0 |
| Section pages (e.g. `/ai`, `/devops`) — "all" section excluded | 0.9 |
| Collection pages (e.g. `/ai/videos`) | 0.8 |

**Dynamic entries (priority 0.6):**

Content items from the database where `collection NOT IN ('news', 'blogs', 'community')`. These collections link externally and have no detail page on the site. Only collections with real detail pages (`videos`, `roundups`, custom collections) are included. Each entry includes a `<lastmod>` date derived from the item's `DateEpoch`.

URL routing for dynamic entries follows the same rules as the site navigation: `roundups` items always route to `/all/roundups/{slug}`; all other items use `/{primarySection}/{collection}/{slug}`.

The dynamic items are cached in memory under the key `"sitemap:items"` with `CacheItemPriority.NeverRemove`.

**Implementation files:**

- `src/TechHub.Api/Endpoints/SitemapEndpoints.cs` — XML generation
- `src/TechHub.Infrastructure/Repositories/ContentRepository.cs` — `GetSitemapItemsAsync()`
- `src/TechHub.Core/Models/Core/SitemapItem.cs` — Lightweight record
- `src/TechHub.Web/Program.cs` — `/sitemap.xml` proxy route

## RSS Feeds

RSS feeds enable content syndication and feed readers. RSS autodiscovery `<link>` tags are rendered by `SeoMetaTags` when the `RssFeedUrl` parameter is provided. The homepage uses `/all/feed.xml` and section pages use `/{sectionName}/feed.xml`. Content detail pages do not have an RSS feed link.

See [rss-feeds.md](rss-feeds.md) for RSS endpoint documentation.

## Semantic HTML

Proper semantic HTML structure improves SEO by helping search engines understand content hierarchy.

See [page-structure.md](page-structure.md) for semantic HTML requirements:

- Use `<main>` for primary content
- Use `<article>` for self-contained content
- Use `<section>` for thematic groupings
- Use `<nav>` for navigation
- Use `<aside>` for sidebars
- Use proper heading hierarchy (`<h1>` → `<h2>` → `<h3>`)

## URL Structure

URLs follow a hierarchical pattern:

- **Sections**: `/{sectionName}` (e.g., `/ai`, `/github-copilot`)
- **Collections**: `/{sectionName}/{collectionName}` (e.g., `/ai/news`)
- **Content**: `/{sectionName}/{collectionName}/{slug}` (e.g., `/ai/news/2025-01-15-article`)

This structure:

- Reflects content hierarchy
- Uses human-readable slugs
- Includes keywords relevant to content
- Enables breadcrumb navigation

## Implementation Reference

- SEO meta tags: [src/TechHub.Web/Components/SeoMetaTags.razor](../src/TechHub.Web/Components/SeoMetaTags.razor)
- Homepage: [src/TechHub.Web/Components/Pages/Home.razor](../src/TechHub.Web/Components/Pages/Home.razor)
- Content detail pages: [src/TechHub.Web/Components/Pages/ContentItem.razor](../src/TechHub.Web/Components/Pages/ContentItem.razor)
- Section and collection listing pages: [src/TechHub.Web/Components/Pages/SectionCollection.razor](../src/TechHub.Web/Components/Pages/SectionCollection.razor)
- Canonical URL: [src/TechHub.Web/Components/Layout/MainLayout.razor](../src/TechHub.Web/Components/Layout/MainLayout.razor)
- RSS feeds: [rss-feeds.md](rss-feeds.md)
