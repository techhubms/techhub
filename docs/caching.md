# Browser Caching

Tech Hub uses aggressive caching strategies for static assets to maximize performance while ensuring cache invalidation works correctly when files change.

## Cache Strategy Overview

| Asset Type | Cache Duration | Cache-Control Header |
|------------|----------------|----------------------|
| **Fingerprinted assets** | Forever | `max-age=31536000,immutable` |
| **Images/Fonts** | 1 year | `max-age=31536000,immutable` |
| **CSS/JS (non-fingerprinted)** | 1 hour | `max-age=3600,must-revalidate` |
| **Other files** | Browser default | No headers set |

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

- Placed **FIRST** in pipeline (before MapStaticAssets) to override built-in headers
- Uses `OnStarting` callback to set cache headers after response is prepared
- Detects fingerprinted files for immutable caching

### File Type Detection

| Extensions | Caching |
|------------|---------|
| `.jpg`, `.jpeg`, `.png`, `.webp`, `.jxl`, `.svg`, `.ico` | Images - 1 year immutable |
| `.woff`, `.woff2`, `.ttf`, `.eot` | Fonts - 1 year immutable |
| `.css`, `.js` (non-fingerprinted) | Short cache with revalidation |
| Fingerprinted files (any type) | Forever immutable |

### CSS Bundle (WebOptimizer)

The CSS bundle (`css/bundle.css`) is generated at runtime by WebOptimizer, which sets its own aggressive cache headers (`max-age=10 years`). Since WebOptimizer middleware runs before `StaticFilesCacheMiddleware` and short-circuits the request, our middleware cannot override these headers.

**Cache busting**: `App.razor` appends the assembly MVID as a query parameter (`?v=...`) to the bundle URL. This value changes on every build, ensuring browsers fetch the new bundle after each deployment.

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
- **`<img>` tags**: Use ResponsiveImage component for automatic `<picture>` generation with all formats

### Compression Benefits

Modern formats (JPEG XL, WebP) provide 30-50% better compression than JPEG while maintaining quality.

## Image Path Convention

Use `/images/` prefix (NOT `/assets/`).

**Section Backgrounds**: `/images/section-backgrounds/{section-name}.{format}`

Examples:

- `/images/section-backgrounds/ai.jxl` (best)
- `/images/section-backgrounds/ai.webp` (fallback)
- `/images/section-backgrounds/ai.jpg` (universal)

## Implementation Reference

- Cache middleware: [src/TechHub.Web/Middleware/StaticFilesCacheMiddleware.cs](../src/TechHub.Web/Middleware/StaticFilesCacheMiddleware.cs)
- Configuration: [src/TechHub.Web/Program.cs](../src/TechHub.Web/Program.cs)
