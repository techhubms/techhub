# SEO Images Specification

> **Feature**: Dynamic Open Graph and Twitter Card image generation for content items

## Overview

Tech Hub currently serves `og:image` and `twitter:image` meta tags without values, resulting in no rich preview image when content is shared on social media. This spec covers generating and serving page-specific social sharing images for internally-rendered content pages (videos, roundups, custom collection pages).

External-link content (news, blogs, community items) is excluded: Tech Hub does not own that content, and a generic fallback image would add no meaningful value.

This work builds on [spec 006](../006-seo/spec.md), which implemented the full SEO meta tag and JSON-LD infrastructure. The `SeoMetaTags` component already renders `og:image`, `twitter:image`, `og:image:width`, and `og:image:height` when an image URL is provided — this spec defines how to produce that URL.

## Scope

### In scope

- `og:image` and `twitter:image` for video detail pages
- `og:image` and `twitter:image` for roundup detail pages
- `og:image` and `twitter:image` for custom collection detail pages
- `og:image` and `twitter:image` for section and collection listing pages (homepage optional)
- `thumbnailUrl` on `VideoObject` JSON-LD schema (currently omitted)
- Publisher `logo` on all JSON-LD schemas (currently omitted — no logo asset exists yet)

### Out of scope

- News and blog content items (externally owned — no og:image)
- Community content items
- Automated image crawling / mirroring of external thumbnails
- Video duration metadata (`duration` on `VideoObject`) — YouTube API required

## Requirements

### Functional Requirements

**FR-1**: The system MUST serve an `og:image` URL for video, roundup, and custom content item pages  
**FR-2**: The system MUST serve an `og:image` URL for section and collection listing pages  
**FR-3**: Social sharing images MUST be 1200 × 630 pixels (standard Open Graph dimensions)  
**FR-4**: The `SeoMetaTags` component MUST emit `og:image:width` (1200) and `og:image:height` (630) alongside `og:image`  
**FR-5**: Video items MUST include `thumbnailUrl` in their `VideoObject` JSON-LD schema  
**FR-6**: All JSON-LD schemas MUST include a `publisher.logo` `ImageObject` once a logo asset exists  

### Non-Functional Requirements

**NFR-1**: og:image generation MUST complete in < 2 seconds (per FR-9 / NFR-5 from spec 006)  
**NFR-2**: Generated images MUST be served over HTTPS  
**NFR-3**: Images MUST be cached to avoid regenerating on every social media scrape  

## Approach Options

Three implementation approaches are viable. A decision is needed before work begins.

### Option A — Pre-generate at build time

A build step generates PNG images for every content item and places them in `wwwroot/og-images/`. The `og:image` URL is the static file path.

**Pros**: Zero runtime cost, works in any hosting environment, no external dependencies.  
**Cons**: Adds time to the build/deploy pipeline, requires regenerating all images when the template changes, storage grows with content volume.

### Option B — On-demand generation with caching

An API endpoint (`GET /api/og-image/{slug}`) generates the image on first request using a .NET image library (e.g., SkiaSharp or ImageSharp), then caches it (in-memory or blob storage).

**Pros**: No build-time overhead, fresh images immediately available for new content.  
**Cons**: First request has latency spike; social media crawlers may time out before the image is ready.

### Option C — External image service

Delegate to an external API (e.g., Vercel OG, Cloudinary, or a custom Azure Function) that generates an image from query parameters.

**Pros**: No .NET image library dependency, scalable, preview URL can be tested by pasting in a browser.  
**Cons**: External dependency, potential cost, additional network call.

## Design Notes

### Image template

Regardless of approach, each image should display:

- Site/section name or branding
- Content title (truncated if needed, using the same `TruncateDescription` logic as meta descriptions)
- Collection or section label
- Publication date (for content items)

### YouTube video thumbnails

For video items, YouTube provides thumbnail images at predictable URLs (`https://img.youtube.com/vi/{videoId}/maxresdefault.jpg`). If the `VideoUrl` is a YouTube link, this thumbnail URL can be used directly as `og:image` and `thumbnailUrl` without any image generation infrastructure.

This is likely the lowest-effort starting point.

### `SeoMetaTags` component changes needed

The `SeoMetaTags` component already accepts all parameters needed. The only change required is:

1. Add an `ImageUrl` parameter (`string?`)
2. When `ImageUrl` is non-null, emit:
   - `<meta property="og:image" content="@ImageUrl" />`
   - `<meta property="og:image:width" content="1200" />`
   - `<meta property="og:image:height" content="630" />`
   - `<meta name="twitter:image" content="@ImageUrl" />`
   - Change `twitter:card` from `summary` to `summary_large_image`
3. When `ImageUrl` is non-null and `ContentType` is `Video`, pass it as `thumbnailUrl` in `BuildVideoSchema()`

### Publisher logo

All JSON-LD schemas include a `publisher` object. Once a logo asset is available at a stable URL, add `logo` as an `ImageObject` to `BuildArticleSchema()`, `BuildVideoSchema()`, `BuildCollectionSchema()`, and `BuildWebSiteSchema()`. The logo URL should come from configuration (not hardcoded).

## Acceptance Criteria

- Sharing a video item URL on LinkedIn shows a valid 1200×630 image preview
- Sharing a roundup item URL on Twitter shows the `summary_large_image` card type
- Google's Rich Results Test validates `VideoObject` with `thumbnailUrl` present
- `og:image:width` and `og:image:height` are present alongside every `og:image`
- Pages without an image URL continue to render without `og:image` tags (no regression)

## Implementation Reference

- SEO meta tags component: [src/TechHub.Web/Components/SeoMetaTags.razor](../../src/TechHub.Web/Components/SeoMetaTags.razor)
- Content detail page: [src/TechHub.Web/Components/Pages/ContentItem.razor](../../src/TechHub.Web/Components/Pages/ContentItem.razor)
- Related spec: [specs/006-seo/spec.md](../006-seo/spec.md)
- SEO documentation: [docs/seo.md](../../docs/seo.md)
