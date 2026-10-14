# Caching

Tech Hub caches at two levels. Browser caching uses aggressive static asset headers while ensuring cache invalidation works correctly when files change. [Server-side caching](#server-side-caching) keeps content queries and navigation data in memory so most requests never reach the database. The sections up to [Image Path Convention](#image-path-convention) cover browser caching.

## Cache Strategy Overview

| Asset Type | Cache Duration | Cache-Control Header |
|------------|----------------|----------------------|
| **Fingerprinted assets** | Forever | `max-age=31536000,immutable` |
| **Images/Fonts** | 1 year | `max-age=31536000,immutable` |
| **CSS/JS/JSON/XML/HTML (non-fingerprinted)** | 1 hour | `max-age=3600,must-revalidate` |
| **Other files** | Framework default | Not changed by the middleware |

## Fingerprinting

Fingerprinted files have a content hash in their filename (e.g., `styles.abc123.css`). When the file content changes, the URL changes, forcing browsers to download the new version.

### How Fingerprinting Works

1. Build system generates hash from file content
2. Hash is embedded in filename or query string
3. References to file use fingerprinted URL
4. URL change = automatic cache invalidation

### Fingerprinted Asset Detection

The caching middleware detects fingerprinted files by pattern matching on the URL. Files with version hashes in their names receive `immutable` caching.

## Cache Headers Explained

### `max-age`

Specifies how long (in seconds) the browser can use the cached version without checking with the server.

- `max-age=31536000` = 1 year (365 days × 24 hours × 60 minutes × 60 seconds)
- `max-age=3600` = 1 hour

### `immutable`

Tells the browser the file will NEVER change at this URL. The browser won't even send a conditional request to check if the file changed - it just uses the cached version.

**Why This Matters**: Without `immutable`, browsers may still send "If-Modified-Since" requests even with long `max-age`. With `immutable`, zero HTTP requests are made for cached assets.

### `must-revalidate`

Tells the browser to revalidate with the server when the cached version becomes stale. Used for non-fingerprinted files where we need to ensure updates are picked up.

## Static Files Middleware

The `StaticFilesCacheMiddleware` provides centralized cache control for all static files.

### Implementation Architecture

- Registered before `UseStaticFiles` and `MapStaticAssets`, after the security and URL correction stages (see [url-routing.md](url-routing.md#cache-header-strategy)). Its `OnStarting` callback runs last and overrides their headers
- Uses `OnStarting` callback to set cache headers after response is prepared
- Detects fingerprinted files for immutable caching

### File Type Detection

| Extensions | Caching |
|------------|---------|
| `.jpg`, `.jpeg`, `.png`, `.gif`, `.bmp`, `.webp`, `.jxl`, `.svg`, `.ico` | Images - 1 year immutable |
| `.woff`, `.woff2`, `.ttf`, `.otf`, `.eot` | Fonts - 1 year immutable |
| `.css`, `.js`, `.json`, `.xml`, `.html`, `.htm` (non-fingerprinted) | Short cache with revalidation |
| Fingerprinted files (any type) | Forever immutable |

Only successful (`200`) responses for paths with a file extension are touched.

### CSS Files

All global CSS files use `@Assets["path"]` in App.razor, which produces fingerprinted URLs (content hash in filename, e.g., `base.e3opgp91n1.css`). When CSS content changes, the URL changes, forcing browsers to fetch the new version automatically. `MapStaticAssets()` serves these with proper cache headers, and `StaticFilesCacheMiddleware` sets `immutable` caching for fingerprinted files.

## Performance Benefits

- **Banner images** load instantly on subsequent page views (from disk cache)
- **Zero HTTP requests** for cached assets = faster page loads
- **Reduced server bandwidth** - server never serves cached files again
- **Better user experience** on slow connections
- **Automatic cache busting** for fingerprinted assets when content changes

## Image Multi-Format Support

All images are provided in three formats for optimal performance:

| Format | Browser Support | Use Case |
|--------|----------------|----------|
| **JPEG XL** (`.jxl`) | Chrome 109+, Edge 109+ | Best compression, modern browsers |
| **WebP** (`.webp`) | Wide support | Good compression, fallback |
| **JPEG** (`.jpg`) | Universal | All browsers |

### Format Selection

- **CSS backgrounds**: Reference `.webp` (browsers use native format if supported)
- **`<img>` tags**: Reference the `.webp` file, or use a `<picture>` element listing all formats

### Compression Benefits

Modern formats (JPEG XL, WebP) provide 30-50% better compression than JPEG while maintaining quality.

## Image Path Convention

Use `/images/` prefix (NOT `/assets/`).

**Section Backgrounds**: `/images/section-backgrounds/{section-name}.{format}`

Examples:

- `/images/section-backgrounds/ai.jxl` (best)
- `/images/section-backgrounds/ai.webp` (fallback)
- `/images/section-backgrounds/ai.jpg` (universal)

## Server-side Caching

Server-side caches are in-process `IMemoryCache` instances. They are not shared between processes, so the API, the Web app and each deployed instance keep their own copies. Tech Hub runs a single instance per app, so there is no cross-instance invalidation.

### API Content Cache

`ContentRepository` caches database results in memory. Every entry uses an absolute TTL:

| Cache | TTL | Notes |
|---|---|---|
| Search results and facets | 5 minutes | Key is built from the request (filters, paging, search text) |
| Tag cloud counts | 10 minutes | Key is built from the tag cloud request |
| Content item by slug | 15 minutes | Cached separately with and without rendered HTML |
| Sitemap items | 30 minutes | Single entry |
| Author list | 30 minutes | Single entry |
| Sections and tag-cloud exclusion list | 1 hour | Derived from configuration, so they only change on deployment |

TTLs are a safety net. Freshness comes from invalidation: `InvalidateCachedData()` clears the whole API memory cache. It runs:

- After a content processing run, a successful roundup generation and a content fixer run that changed items
- After admin writes that change content: editing or deleting a content item, applying a transcript, editing AI metadata, adding a URL, approving reviews, removing a processed URL, regenerating a roundup, and deleting a GitHub Copilot feature
- On demand through `POST /api/admin/cache/invalidate` (AdminOnly), which the **Invalidate caches** button on `/admin/settings` calls

### Web Caches

The Web app keeps two singleton caches so pages render without calling the API per request:

| Cache | Content | Refresh |
|---|---|---|
| `SectionCache` | Sections and collections used for navigation and route validation | Loaded at startup, then every 5 minutes by `SectionCacheRefreshService` |
| `HeroBannerCache` | Hero banner data | Loaded at startup, then every 5 minutes by `HeroBannerCacheRefreshService` |

A failed refresh logs a warning and keeps the previous data, and an empty section list never replaces a populated one. `SectionCache` reports ready after its first successful load, which feeds the Web health check (see [health-checks.md](health-checks.md)). The **Invalidate caches** button on `/admin/settings` also reloads both caches immediately instead of waiting for the next refresh.

### MCP Cache

The MCP latest-content tools cache the API response per section and page for 5 minutes in the Web app's memory cache. Only unfiltered requests are cached so callers cannot create unlimited cache keys. See [content-api.md](content-api.md#mcp-server).

## Implementation Reference

- Cache middleware: [src/TechHub.Web/Middleware/StaticFilesCacheMiddleware.cs](../src/TechHub.Web/Middleware/StaticFilesCacheMiddleware.cs)
- Configuration: [src/TechHub.Web/Program.cs](../src/TechHub.Web/Program.cs)
- Content cache: [src/TechHub.Infrastructure/Repositories/ContentRepository.cs](../src/TechHub.Infrastructure/Repositories/ContentRepository.cs)
- Web caches: [src/TechHub.Web/Services/SectionCacheRefreshService.cs](../src/TechHub.Web/Services/SectionCacheRefreshService.cs) and [HeroBannerCacheRefreshService.cs](../src/TechHub.Web/Services/HeroBannerCacheRefreshService.cs)
- MCP cache: [src/TechHub.Web/MCP/LatestContentProvider.cs](../src/TechHub.Web/MCP/LatestContentProvider.cs)
