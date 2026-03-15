# SEO & Structured Data Specification

> **Feature**: Modern SEO with server-side rendering, structured data, and social media optimization

## Overview

The SEO system ensures optimal search engine discoverability through server-side rendering (Blazor SSR), comprehensive structured data (JSON-LD), Open Graph tags, Twitter Cards, and automated sitemap generation. All SEO elements are rendered server-side for maximum crawlability.

## Requirements

### Functional Requirements

**FR-1**: The system MUST render all content server-side (Blazor SSR) for search engine crawlers  
**FR-2**: The system MUST generate JSON-LD structured data for all content types  
**FR-3**: The system MUST include Open Graph meta tags for social media sharing  
**FR-4**: The system MUST include Twitter Card meta tags  
**FR-5**: The system MUST generate dynamic meta descriptions (max 160 characters)  
**FR-6**: The system MUST generate XML sitemap (`/sitemap.xml`)  
**FR-7**: The system MUST generate `robots.txt` with sitemap reference  
**FR-8**: The system MUST set canonical URLs for all pages  
**FR-9**: The system MUST generate dynamic og:image for content items  
**FR-10**: The system MUST implement breadcrumb structured data  

### Non-Functional Requirements

**NFR-1**: All pages MUST achieve Lighthouse SEO score > 95  
**NFR-2**: Structured data MUST validate with Google's Rich Results Test  
**NFR-3**: Sitemap generation MUST complete in < 5 seconds  
**NFR-4**: Meta tag generation MUST add < 10ms to page render time  
**NFR-5**: og:image generation MUST complete in < 2 seconds  
**NFR-6**: Canonical URLs MUST always use HTTPS and primary domain  

## Use Cases

### UC-1: Crawl Content Page

**Actor**: Search Engine Bot  
**Precondition**: Content page exists  
**Trigger**: Bot visits content URL  

**Flow**:

1. Bot requests `/2025-01-02-chat-in-ide.html`
2. Server renders page via Blazor SSR
3. Server includes meta tags, JSON-LD, Open Graph
4. Bot receives fully rendered HTML with:
   - Title, meta description
   - Canonical URL
   - JSON-LD Article structured data
   - Open Graph tags
   - Breadcrumb navigation
5. Bot indexes content

**Postcondition**: Content indexed by search engine

### UC-2: Share Content on Social Media

**Actor**: User  
**Precondition**: Content page exists  
**Trigger**: User shares URL on Twitter/LinkedIn  

**Flow**:

1. User copies content URL
2. User pastes URL on Twitter
3. Twitter fetches URL and parses Open Graph tags
4. Twitter displays rich preview with:
   - Article title (og:title)
   - Description (og:description)
   - Featured image (og:image)
   - Site name (og:site_name)
5. User's followers see rich preview in timeline

**Postcondition**: Content shared with rich preview

### UC-3: Generate Sitemap

**Actor**: Build System  
**Precondition**: Content exists  
**Trigger**: Site build or scheduled job  

**Flow**:

1. System queries all published content items
2. System queries all static pages
3. System generates XML sitemap with:
   - URL (absolute HTTPS)
   - Last modified date
   - Change frequency (daily for news, weekly for blogs)
   - Priority (1.0 for home, 0.8 for sections, 0.6 for content)
4. System writes `/sitemap.xml`
5. System updates `robots.txt` with sitemap reference

**Postcondition**: Sitemap available for search engines

## Structured Data (JSON-LD)

### Article Schema

**For Content Items** (news, blogs, videos, community):

```json
{
  "@context": "https://schema.org",
  "@type": "Article",
  "headline": "Chat in IDE - GitHub Copilot Feature",
  "description": "Use GitHub Copilot chat directly in your IDE for coding assistance",
  "author": {
    "@type": "Organization",
    "name": "Microsoft"
  },
  "publisher": {
    "@type": "Organization",
    "name": "Tech Hub",
    "logo": {
      "@type": "ImageObject",
      "url": "https://tech.hub.ms/assets/logo.png"
    }
  },
  "datePublished": "2025-01-02T10:00:00+01:00",
  "dateModified": "2025-01-02T10:00:00+01:00",
  "image": "https://tech.hub.ms/assets/og-images/2025-01-02-chat-in-ide.png",
  "url": "https://tech.hub.ms/2025-01-02-chat-in-ide.html",
  "mainEntityOfPage": "https://tech.hub.ms/2025-01-02-chat-in-ide.html",
  "keywords": ["GitHub Copilot", "AI", "IDE", "Coding Assistant"],
  "articleSection": "GitHub Copilot"
}
```

### VideoObject Schema

**For Video Content**:

```json
{
  "@context": "https://schema.org",
  "@type": "VideoObject",
  "name": "GitHub Copilot Chat Demo",
  "description": "Learn how to use GitHub Copilot chat in VS Code",
  "thumbnailUrl": "https://tech.hub.ms/assets/thumbnails/video-123",
  "uploadDate": "2025-01-02T10:00:00+01:00",
  "duration": "PT5M30S",
  "contentUrl": "https://www.youtube.com/watch?v=VIDEO_ID",
  "embedUrl": "https://www.youtube.com/embed/VIDEO_ID",
  "publisher": {
    "@type": "Organization",
    "name": "Tech Hub"
  }
}
```

### BreadcrumbList Schema

**For All Pages**:

```json
{
  "@context": "https://schema.org",
  "@type": "BreadcrumbList",
  "itemListElement": [
    {
      "@type": "ListItem",
      "position": 1,
      "name": "Home",
      "item": "https://tech.hub.ms/"
    },
    {
      "@type": "ListItem",
      "position": 2,
      "name": "GitHub Copilot",
      "item": "https://tech.hub.ms/github-copilot/"
    },
    {
      "@type": "ListItem",
      "position": 3,
      "name": "Chat in IDE",
      "item": "https://tech.hub.ms/2025-01-02-chat-in-ide.html"
    }
  ]
}
```

### CollectionPage Schema

**For Section Index Pages**:

```json
{
  "@context": "https://schema.org",
  "@type": "CollectionPage",
  "name": "GitHub Copilot Resources",
  "description": "Latest news, videos, and blogs about GitHub Copilot",
  "url": "https://tech.hub.ms/github-copilot/",
  "publisher": {
    "@type": "Organization",
    "name": "Tech Hub"
  }
}
```

## Meta Tags

### Basic Meta Tags

**For All Pages**:

```html
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<meta name="description" content="@description">
<meta name="keywords" content="@string.Join(", ", tags)">
<meta name="author" content="@author">
<link rel="canonical" href="@canonicalUrl">
```

### Open Graph Tags

**For Content Pages**:

```html
<meta property="og:type" content="article">
<meta property="og:title" content="@title">
<meta property="og:description" content="@description">
<meta property="og:url" content="@canonicalUrl">
<meta property="og:image" content="@ogImageUrl">
<meta property="og:image:width" content="1200">
<meta property="og:image:height" content="630">
<meta property="og:site_name" content="Tech Hub">
<meta property="og:locale" content="en_US">
<meta property="article:published_time" content="@publishDate">
<meta property="article:modified_time" content="@modifiedDate">
<meta property="article:author" content="@author">
<meta property="article:section" content="@category">
<meta property="article:tag" content="@tag">
```

### Twitter Cards

**For Content Pages**:

```html
<meta name="twitter:card" content="summary_large_image">
<meta name="twitter:title" content="@title">
<meta name="twitter:description" content="@description">
<meta name="twitter:image" content="@ogImageUrl">
<meta name="twitter:site" content="@Microsoft">
```

## Sitemap Generation

### XML Sitemap Structure

**`/sitemap.xml`**:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <!-- Home Page -->
  <url>
    <loc>https://tech.hub.ms/</loc>
    <lastmod>2025-01-23T00:00:00+01:00</lastmod>
    <changefreq>daily</changefreq>
    <priority>1.0</priority>
  </url>
  
  <!-- Section Index Pages -->
  <url>
    <loc>https://tech.hub.ms/github-copilot/</loc>
    <lastmod>2025-01-23T00:00:00+01:00</lastmod>
    <changefreq>daily</changefreq>
    <priority>0.8</priority>
  </url>
  
  <!-- Content Items -->
  <url>
    <loc>https://tech.hub.ms/2025-01-02-chat-in-ide.html</loc>
    <lastmod>2025-01-02T10:00:00+01:00</lastmod>
    <changefreq>weekly</changefreq>
    <priority>0.6</priority>
  </url>
</urlset>
```

### Robots.txt

**`/robots.txt`**:

```text
User-agent: *
Allow: /

Sitemap: https://tech.hub.ms/sitemap.xml
```

## Dynamic og:image Generation

### og:image Requirements

- Generate unique og:image for each content item
- Include article title, category, and logo
- Use consistent branding (colors, fonts)
- Optimize for social media (1200x630px)

### Implementation Approaches

#### Option 1: Pre-Generate During Build

- Generate images at build time using ImageSharp
- Store in `/assets/og-images/` directory
- Reference in meta tags

#### Option 2: Dynamic Generation on First Request

- Generate on-demand when og:image URL requested
- Cache generated image (CDN, Azure Blob Storage)
- Return cached image on subsequent requests

#### Option 3: External Service

- Use service like Cloudinary or og-image.vercel.app
- Pass title, category as URL parameters
- Service generates and caches image

## Blazor Components

### MetaTags.razor

```razor
@inject NavigationManager Navigation

<HeadContent>
    <!-- Basic Meta -->
    <meta name="description" content="@Description">
    <meta name="keywords" content="@string.Join(", ", Keywords)">
    <link rel="canonical" href="@CanonicalUrl">
    
    <!-- Open Graph -->
    <meta property="og:type" content="@OgType">
    <meta property="og:title" content="@Title">
    <meta property="og:description" content="@Description">
    <meta property="og:url" content="@CanonicalUrl">
    <meta property="og:image" content="@OgImage">
    <meta property="og:site_name" content="Tech Hub">
    
    <!-- Twitter Card -->
    <meta name="twitter:card" content="summary_large_image">
    <meta name="twitter:title" content="@Title">
    <meta name="twitter:description" content="@Description">
    <meta name="twitter:image" content="@OgImage">
    
    <!-- JSON-LD -->
    <script type="application/ld+json">
        @((MarkupString)StructuredDataJson)
    </script>
</HeadContent>

@code {
    [Parameter, EditorRequired]
    public string Title { get; set; } = string.Empty;
    
    [Parameter, EditorRequired]
    public string Description { get; set; } = string.Empty;
    
    [Parameter]
    public string[] Keywords { get; set; } = Array.Empty<string>();
    
    [Parameter]
    public string OgType { get; set; } = "website";
    
    [Parameter]
    public string OgImage { get; set; } = "/assets/default-og-image.png";
    
    [Parameter]
    public string StructuredDataJson { get; set; } = string.Empty;
    
    private string CanonicalUrl => Navigation.ToAbsoluteUri(Navigation.Uri).ToString();
}
```

## Implementation Patterns

### Schema.org JSON-LD in Blazor

**HeadContent with JSON-LD Structured Data**:

```razor
@* ContentDetail.razor *@
@inject IContentRepository ContentRepository

<HeadContent>
    <!-- JSON-LD Article Schema -->
    <script type="application/ld+json">
    {
        "@context": "https://schema.org",
        "@type": "Article",
        "headline": "@Item.Title",
        "description": "@Item.Excerpt",
        "author": {
            "@type": "Organization",
            "name": "Microsoft"
        },
        "publisher": {
            "@type": "Organization",
            "name": "Tech Hub",
            "logo": {
                "@type": "ImageObject",
                "url": "https://tech.hub.ms/assets/logo.png"
            }
        },
        "datePublished": "@Item.Date.ToString("yyyy-MM-ddTHH:mm:sszzz")",
        "dateModified": "@Item.Date.ToString("yyyy-MM-ddTHH:mm:sszzz")",
        "image": "https://tech.hub.ms/assets/og-images/@Item.Slug.png",
        "url": "https://tech.hub.ms/@Item.GetUrlInSection()",
        "mainEntityOfPage": "https://tech.hub.ms/@Item.GetUrlInSection()",
        "keywords": [@string.Join(", ", Item.Tags.Select(t => $"\"{t}\""))],
        "articleSection": "@string.Join(", ", Item.SectionNames)"
    }
    </script>
    
    <!-- Breadcrumb Schema -->
    <script type="application/ld+json">
    {
        "@context": "https://schema.org",
        "@type": "BreadcrumbList",
        "itemListElement": [
            {
                "@type": "ListItem",
                "position": 1,
                "name": "Home",
                "item": "https://tech.hub.ms/"
            },
            {
                "@type": "ListItem",
                "position": 2,
                "name": "@SectionTitle",
                "item": "https://tech.hub.ms/@SectionName/"
            },
            {
                "@type": "ListItem",
                "position": 3,
                "name": "@Item.Title",
                "item": "https://tech.hub.ms/@Item.GetUrlInSection()"
            }
        ]
    }
    </script>
</HeadContent>

@code {
    [Parameter, EditorRequired]
    public ContentItem Item { get; set; } = default!;
    
    [Parameter, EditorRequired]
    public string SectionName { get; set; } = string.Empty;
    
    [Parameter, EditorRequired]
    public string SectionTitle { get; set; } = string.Empty;
}
```

**VideoObject Schema for Video Content**:

```razor
@* Use this pattern when Item.VideoId is present *@

<script type="application/ld+json">
{
    "@context": "https://schema.org",
    "@type": "VideoObject",
    "name": "@Item.Title",
    "description": "@Item.Excerpt",
    "thumbnailUrl": "https://img.youtube.com/vi/@Item.VideoId/maxresdefault",
    "uploadDate": "@Item.Date.ToString("yyyy-MM-ddTHH:mm:sszzz")",
    "contentUrl": "https://www.youtube.com/watch?v=@Item.VideoId",
    "embedUrl": "https://www.youtube.com/embed/@Item.VideoId",
    "publisher": {
        "@type": "Organization",
        "name": "Tech Hub"
    }
}
</script>
```

**CollectionPage Schema for Section Pages**:

```razor
@* SectionIndex.razor *@

<HeadContent>
    <script type="application/ld+json">
    {
        "@context": "https://schema.org",
        "@type": "CollectionPage",
        "name": "@Section.DisplayTitle",
        "description": "@Section.Description",
        "url": "https://tech.hub.ms/@Section.Name/",
        "publisher": {
            "@type": "Organization",
            "name": "Tech Hub"
        }
    }
    </script>
</HeadContent>
```

**Key Implementation Notes**:

- ✅ Use `<HeadContent>` component to inject `<script type="application/ld+json">` into `<head>`
- ✅ Escape JSON strings properly using Razor syntax (`@Item.Title` auto-escapes)
- ✅ Use absolute URLs for all `url`, `image`, `item` properties
- ✅ Format dates as ISO 8601 with timezone (`yyyy-MM-ddTHH:mm:sszzz`)
- ✅ Include both Article/VideoObject AND BreadcrumbList schemas on content pages
- ✅ Test structured data with [Google Rich Results Test](https://search.google.com/test/rich-results)

## Testing Strategy

### Unit Tests

- Test structured data generation (valid JSON-LD)
- Test meta tag generation (correct format)
- Test sitemap URL generation
- Test redirect mapping lookup
- Test og:image URL generation

### Integration Tests

- Test full page render includes all SEO elements
- Test sitemap generation with real content
- Test redirect middleware behavior
- Validate structured data with Google validator

### SEO Validation Tests

- Lighthouse SEO audit (score > 95)
- Google Rich Results Test
- Facebook Sharing Debugger
- Twitter Card Validator
- Sitemap XML validation

### E2E Tests (Playwright)

- Test meta tags present in page source
- Test canonical URL correctness
- Test Open Graph tags for sharing
- Test structured data exists in page
- Test sitemap accessibility

## Acceptance Criteria

**AC-1**: Given any content page, when viewed by search bot, then all meta tags are present  
**AC-2**: Given any content page, when tested with Google Rich Results, then structured data is valid  
**AC-3**: Given sitemap URL, when requested, then XML contains all published pages  
**AC-4**: Given any page, when run through Lighthouse, then SEO score is > 95  
**AC-5**: Given content URL, when shared on Twitter, then rich preview appears  
**AC-6**: Given any content, when og:image requested, then image loads within 2 seconds  

## Performance Optimization

**Caching**:

- Cache generated structured data in memory
- Cache sitemap XML (regenerate daily or on content update)
- Cache og:images with CDN (1 year TTL)
- Use Output Caching for meta tag rendering

**Optimization**:

- Minify JSON-LD (remove whitespace)
- Lazy-generate og:images (only when requested)
- Batch sitemap generation during off-peak hours
- Use HTTP/2 for faster meta resource loading

## Accessibility Considerations

- Meta descriptions help screen readers understand page content
- Structured data improves voice assistant responses
- Breadcrumbs aid navigation for all users
- Canonical URLs prevent duplicate content confusion

## Implementation Notes

**SEO Foundation**:

- Implement comprehensive JSON-LD structured data for all content types
- Generate dynamic og:image for social sharing
- Add breadcrumb structured data for navigation
- Include enhanced Open Graph and Twitter Card support
- Enforce meta description format (160 char limit)
- Use Europe/Brussels timezone for all date handling

**URL Strategy**:

- Use clean, semantic URLs (e.g., `/ai/content-title`)
- Monitor Google Search Console for crawl errors
- Ensure internal links use consistent URL structure

## Open Questions

1. Should we implement hreflang tags for internationalization?
2. Should we generate AMP versions of pages?
3. Should we implement RSS autodiscovery link tags?
4. Should we add Dublin Core metadata?
5. Should we implement news sitemap for Google News?

## References

- [Content Rendering Spec](/specs/018-content-rendering/spec.md) (for Markdown to HTML)
- [Section System Spec](/specs/010-section-system/spec.md) (for navigation structure)
- [Google Structured Data Guidelines](https://developers.google.com/search/docs/guides/intro-structured-data)
- [Open Graph Protocol](https://ogp.me/)
- [Twitter Card Documentation](https://developer.twitter.com/en/docs/twitter-for-websites/cards/overview/abouts-cards)
