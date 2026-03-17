# SEO (Search Engine Optimization)

Tech Hub implements SEO best practices to ensure content is discoverable and properly indexed by search engines.

## Server-Side Rendering (SSR)

All content is server-side rendered via Blazor's global InteractiveServer mode with prerendering enabled. This ensures:

- Search engines see complete HTML content
- Fast initial page load (prerendered HTML delivered before SignalR connects)
- Proper meta tags in initial response
- Social media previews work correctly

See [render-modes.md](render-modes.md) for the render mode architecture and PersistentComponentState pattern.

## Schema.org Structured Data

Content pages include JSON-LD structured data for rich search results.

### Article Schema

```html
<script type="application/ld+json">
{
    "@context": "https://schema.org",
    "@type": "Article",
    "headline": "Article Title",
    "description": "Article description from excerpt",
    "author": {
        "@type": "Person",
        "name": "Author Name"
    },
    "datePublished": "2025-01-15T10:00:00+01:00",
    "publisher": {
        "@type": "Organization",
        "name": "Microsoft Tech Hub",
        "logo": {
            "@type": "ImageObject",
            "url": "https://tech.hub.ms/images/logo.png"
        }
    },
    "keywords": ["tag1", "tag2", "tag3"]
}
</script>
```

### Implementation Pattern

```razor
@page "/{sectionName}/{collectionName}/{slug}"

<HeadContent>
    <script type="application/ld+json">
    {
        "@context": "https://schema.org",
        "@type": "Article",
        "headline": "@Item.Title",
        "description": "@Item.Excerpt",
        "author": {
            "@type": "Person",
            "name": "@Item.Author"
        },
        "datePublished": "@GetIsoDate(Item.DateEpoch)",
        "publisher": {
            "@type": "Organization",
            "name": "Microsoft Tech Hub",
            "logo": {
                "@type": "ImageObject",
                "url": "https://tech.hub.ms/images/logo.png"
            }
        },
        "keywords": [@string.Join(", ", Item.Tags.Select(t => $"\"{t}\""))]
    }
    </script>
</HeadContent>

@code {
    private string GetIsoDate(long epochSeconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(epochSeconds)
            .ToString("yyyy-MM-ddTHH:mm:sszzz");
    }
}
```

## Page Titles

Each page should have a descriptive title using `<PageTitle>`:

```razor
<PageTitle>@Item.Title - Tech Hub</PageTitle>
```

Title patterns:

- **Homepage**: `Tech Hub - Microsoft Developer Resources`
- **Section**: `AI - Tech Hub`
- **Collection**: `News - AI - Tech Hub`
- **Content Item**: `Article Title - Tech Hub`

## Meta Description

Use the excerpt as the meta description:

```razor
<HeadContent>
    <meta name="description" content="@Item.Excerpt" />
</HeadContent>
```

## Canonical URLs

Every page automatically receives a canonical URL via `MainLayout.razor`. The layout computes the canonical URL by normalizing the current path (lowercased, query parameters stripped) and combining it with the base URI. Individual pages do not need to add their own canonical tags.

## Open Graph Tags

For social media sharing (Facebook, LinkedIn):

```razor
<HeadContent>
    <meta property="og:title" content="@Item.Title" />
    <meta property="og:description" content="@Item.Excerpt" />
    <meta property="og:type" content="article" />
    <meta property="og:url" content="https://tech.hub.ms@Item.GetPrimarySectionUrl()" />
    <meta property="og:image" content="https://tech.hub.ms/images/og-image.png" />
</HeadContent>
```

## Twitter Cards

For Twitter/X sharing:

```razor
<HeadContent>
    <meta name="twitter:card" content="summary_large_image" />
    <meta name="twitter:title" content="@Item.Title" />
    <meta name="twitter:description" content="@Item.Excerpt" />
    <meta name="twitter:image" content="https://tech.hub.ms/images/twitter-card.png" />
</HeadContent>
```

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

RSS feeds enable content syndication and feed readers. RSS `<link>` tags are automatically rendered by `MainLayout.razor` for pages that have an associated feed (section pages and the homepage). The layout determines the correct feed URL from the current URL and SectionCache.

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

- Structured data: Content detail page components
- Meta tags: [src/TechHub.Web/Components/App.razor](../src/TechHub.Web/Components/App.razor)
- RSS feeds: [rss-feeds.md](rss-feeds.md)
