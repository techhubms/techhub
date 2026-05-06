# URL Routing and Redirects

Tech Hub processes every incoming HTTP request through an ordered middleware pipeline before it reaches Blazor. Each stage either redirects (301), short-circuits with an error, or passes the request to the next stage.

## Canonical URL Format

The canonical URL structure for content is:

```text
/{section}/{collection}/{slug}
```

Examples:

- `/ai/videos/my-article`
- `/github-copilot/roundups/june-2026-digest`
- `/all/roundups/{slug}` — roundups always use the `all` section

Special pages sit outside this structure: `/`, `/about`, `/admin`, `/not-found`.

## Pipeline Overview

Stages run in this order for every request:

| # | Stage | What it does |
|---|---|---|
| 1 | Subdomain redirects | Normalizes hostnames |
| 2 | URL normalization | Strips `.html`, date prefixes, resolves legacy slugs — at most one 301 |
| 3 | HTTPS redirect | Upgrades HTTP to HTTPS |
| 4 | Static files | Serves static assets, short-circuits |
| 5 | Invalid segment filter | Rejects probe and structurally invalid paths |
| 6 | Blazor routing | Renders the matching page |

## Stage 1 — Subdomain Redirects

Configured via `SubdomainShortcuts`, `PrimaryHosts`, and `PassthroughSubdomains` in `appsettings.json`.

| Hostname pattern | Outcome |
|---|---|
| Primary host (`tech.hub.ms`, `tech.xebia.ms`) | Pass through |
| No dots (e.g. `localhost`) | Pass through |
| Passthrough subdomain (e.g. `staging-tech.hub.ms`) | Pass through |
| `www.<primaryHost>` (e.g. `www.tech.hub.ms`) | **301 → `https://<primaryHost>/path`** |
| Known shortcut (e.g. `ghc.xebia.ms`) | **301 → `https://<primaryHost>/<section>/path`** |
| Unknown subdomain on known domain | **301 → `https://<primaryHost>/path`** |
| Completely unknown domain | Pass through |

All host comparisons are case-insensitive. Redirect URLs always use the lowercase canonical hostname. Path and query string are preserved in all cases.

## Stage 2 — URL Normalization

A single middleware handles all URL cleanup and legacy resolution in one pass, issuing **at most one 301 redirect per request**. Three concerns are handled together:

### 2a — Strip `.html` extension

Any path segment ending in `.html` has the extension removed.

```text
/github-copilot/features.html  →  /github-copilot/features
```

### 2b — Strip date-prefixed slugs

Any segment matching `YYYY-MM-DD-<slug>` has the date prefix removed.

```text
/ai/videos/2026-01-12-my-article  →  /ai/videos/my-article
```

The pattern applies at any depth in the path.

### 2c — Legacy slug lookup

Applies only to **single-segment paths** that remain unresolved after the normalizations above (i.e. the section and collection are still unknown). Skipped for:

- Known non-section pages: `not-found`, `about`, `error`, `admin`
- Known section names (from in-memory `SectionCache`)
- Static file extensions (any extension — `.html` was already stripped above)

For all other single-segment paths, calls `GET /api/legacy-redirect?slug={slug}&section={hint}`. The API normalizes the slug (lowercase, strips `.html`, strips `YYYY-MM-DD-` prefix) and queries the database. If multiple items match, the one whose `primary_section_name` matches the hint is preferred; ties go to the most recently dated item. Items in external collections (`news`, `blogs`, `community`) resolve to their `external_url`.

**One redirect covers everything**: `/2026-01-12-My-Article?section=ai` → strips date → normalizes to `my-article` → API lookup → **301 directly to `/ai/videos/my-article`** (no intermediate `/my-article` step).

**Case is not normalized in the middleware.** The infrastructure layer lowercases parameters before DB queries, so `/My-Article` and `/my-article` both resolve to the same content without a redirect.

**Not found**: if the API returns no match, the middleware returns a **404** immediately. There is no redirect to a cleaned path — the cleaned URL has already been applied upstream by normalizations, so no extra round-trip is needed.

**Transient API failure**: if the API is temporarily unavailable (network error or timeout after all retries), the middleware degrades gracefully instead of returning a cacheable 404 that could mislabel a valid legacy URL as permanently absent. When the path was cleaned (`pathChanged = true`), the middleware still issues a **301 redirect** to the normalized URL. When the path was already clean, the request is passed through to Blazor routing. In both cases the failure is logged as a warning.

## Stage 3 — HTTPS Redirect

Requests arriving over HTTP are **301-redirected** to the HTTPS equivalent. This stage runs after all URL-normalization redirects so that a single request never triggers two redirects in sequence.

## Stage 4 — Static Files

CSS, JS, images, fonts, `robots.txt`, and other static assets are served directly from `wwwroot/`. These requests short-circuit here and never reach the Blazor pipeline.

## Stage 5 — Invalid Route Segment Filter

Two categories of bad requests are rejected before the Blazor pipeline runs:

1. **Scanner/attacker probes** — paths matching well-known exploit probe patterns (e.g. `/wp-admin`, `/xmlrpc.php`, `/.env`). Detected via `ProbeDetector.IsProbeRequest()` (same shared logic as the OpenTelemetry filter). These receive a bare **404** immediately — no Activity span is created for them at all.

2. **Structurally invalid first segments** — segments that can never match a Blazor route (e.g. segments with digits, dots, or percent-encoding). Valid first segments match `[a-zA-Z][a-zA-Z-]*` (letters and hyphens, starting with a letter, case-insensitive).

**Always passed through:**

- Root `/`
- Paths with a file extension in the final segment (static assets)
- Paths starting with `_` (Blazor internals: `/_blazor`, `/_framework`, `/_content`)
- Paths starting with `MicrosoftIdentity` (OIDC callback routes)

Everything else with an invalid first segment receives an immediate **404**.

## Stage 6 — Blazor Routing

The Blazor router matches the path to a page component:

| Path pattern | Component |
|---|---|
| `/` | `Home.razor` |
| `/{section}` or `/{section}/{collection}` | `SectionCollection.razor` |
| `/{section}/{collection}/{slug}` | `ContentItem.razor` |
| `/about` | `About.razor` |
| `/not-found` | `NotFound.razor` |
| `/admin` | Admin pages |
| `/github-copilot/vscode-updates` | `GitHubCopilotVSCodeUpdates.razor` |
| No match | Fallback → `/not-found` |

When no route matches, the Blazor Router renders the `NotFound.razor` page (configured via `NotFoundPage` on the `<Router>` component) and returns HTTP 404.

Blazor pages additionally validate that the referenced section and collection exist (via `SectionCache`) in `OnInitializedAsync`. Invalid combinations redirect to `/not-found` before any content renders.

## robots.txt

The `robots.txt` file (`src/TechHub.Web/wwwroot/robots.txt`) disallows crawlers from accessing paths that have no indexable content:

| Disallowed path | Reason |
|---|---|
| `/_blazor` | Blazor SignalR negotiation traffic |
| `/_framework` | .NET framework assets |
| `/api/` | Internal API, not for crawlers |
| `/swagger/` | API documentation UI |
| `/health`, `/alive` | Health check endpoints |
| `/not-found` | 404 error page — not a real content destination |
| `/Error` | ASP.NET error handler page |
| `/admin` | Admin section, not for public indexing |

The file also points crawlers to `https://tech.hub.ms/sitemap.xml`.

## Implementation Reference

| Component | File |
|---|---|
| Subdomain redirect middleware | [src/TechHub.Web/Middleware/SubdomainRedirectMiddleware.cs](../src/TechHub.Web/Middleware/SubdomainRedirectMiddleware.cs) |
| URL normalization middleware | [src/TechHub.Web/Middleware/UrlNormalizationMiddleware.cs](../src/TechHub.Web/Middleware/UrlNormalizationMiddleware.cs) |
| Invalid segment filter middleware | [src/TechHub.Web/Middleware/InvalidRouteSegmentMiddleware.cs](../src/TechHub.Web/Middleware/InvalidRouteSegmentMiddleware.cs) |
| Pipeline registration | [src/TechHub.Web/Program.cs](../src/TechHub.Web/Program.cs) |
| Legacy redirect API endpoint | [src/TechHub.Api/Endpoints/ContentEndpoints.cs](../src/TechHub.Api/Endpoints/ContentEndpoints.cs) |
| Legacy redirect DB query | [src/TechHub.Infrastructure/Repositories/ContentRepository.cs](../src/TechHub.Infrastructure/Repositories/ContentRepository.cs) |
| `robots.txt` | [src/TechHub.Web/wwwroot/robots.txt](../src/TechHub.Web/wwwroot/robots.txt) |
