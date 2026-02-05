# SEO (Search Engine Optimization)

Tech Hub implements SEO best practices to ensure content is discoverable and properly indexed by search engines.

## Server-Side Rendering (SSR)

All content is server-side rendered by default. This ensures:

- Search engines see complete HTML content
- Fast initial page load (no JavaScript required for content)
- Proper meta tags in initial response
- Social media previews work correctly

See [render-modes.md](render-modes.md) for details on when to use SSR vs interactive modes.

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

Ensure each page has a canonical URL to prevent duplicate content issues:

```razor
<HeadContent>
    <link rel="canonical" href="https://tech.hub.ms@Item.GetPrimarySectionUrl()" />
</HeadContent>
```

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

## RSS Feeds

RSS feeds enable content syndication and feed readers.

See [rss-feeds.md](rss-feeds.md) for RSS endpoint documentation.

RSS `<link>` tags in `<head>`:

```html
<link rel="alternate" type="application/rss+xml" 
      title="Tech Hub - All Content" 
      href="/all/feed.xml" />
<link rel="alternate" type="application/rss+xml" 
      title="Tech Hub - AI" 
      href="/ai/feed.xml" />
```

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
