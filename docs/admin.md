# Admin Area

The admin area lets maintainers run and monitor the content pipelines, edit content, manage feeds and custom pages, and operate the newsletter. It consists of the `/api/admin/*` endpoints in the API and the `/admin/*` pages in the Web app. Authentication and authorization are covered in [admin-authentication.md](admin-authentication.md).

## Overview

- Every `/api/admin/*` endpoint requires the `AdminOnly` policy: an authenticated user whose token carries the `Admin.Access` scope. When `AzureAd:ClientId` is not configured, the policy allows all requests outside Production (local development, tests, PR previews) and denies all requests in Production, so a lost setting can never open the admin endpoints.
- The Web admin pages never call the API anonymously. They forward the signed-in user's access token.
- Inputs are sanitized before use, and mutations that change content invalidate the server-side content cache so changes show up immediately.
- Endpoints are defined in [AdminEndpoints.cs](../src/TechHub.Api/Endpoints/AdminEndpoints.cs), [NewsletterEndpoints.cs](../src/TechHub.Api/Endpoints/NewsletterEndpoints.cs) and [GhcFeaturesEndpoints.cs](../src/TechHub.Api/Endpoints/GhcFeaturesEndpoints.cs).

Content items are addressed by `collection` and `slug` query parameters (not route segments), because slugs are only unique within a collection.

## API Endpoints

### Background jobs

Trigger endpoints return `202 Accepted` immediately and run on the background services. See [background-services.md](background-services.md) for schedules, enable gates and job types.

| Endpoint | Purpose | Notes |
|----------|---------|-------|
| `POST /processing/trigger` | Run the RSS content processing pipeline now | `202` |
| `POST /roundup/trigger` | Generate the previous week's roundups now | `202` |
| `POST /roundup/regenerate?collection&slug` | Delete one roundup and regenerate that week | `collection` must be `roundups` and `slug` must match `weekly-{section}-roundup-YYYY-MM-DD`, otherwise `400`. `202` on success. |
| `POST /content-fixer/trigger` | Run the bulk content fixer (tags, authors, markdown) | `202` |
| `POST /processing/cancel` | Cancel the currently running processing, roundup or fixer run | `200` in both cases ("Cancellation requested" or "No running job to cancel"). Does not cancel the newsletter or ad-hoc URL runs. |
| `GET /processing/jobs?count` | Recent job history (all job types) | Default 20, clamped to 1-100 |
| `GET /processing/jobs/{id}` | One job with its log | `404` if unknown |
| `GET /job-settings` | List scheduled-job enable flags | |
| `PUT /job-settings/{jobName}` | Enable or disable a scheduled job | `204`, or `404` for an unknown job name |

### Feeds and processed URLs

| Endpoint | Purpose | Notes |
|----------|---------|-------|
| `GET /feeds`, `GET /feeds/{id}` | List or read RSS feed configurations | `404` for unknown id |
| `POST /feeds`, `PUT /feeds/{id}` | Create or update a feed | `name`, `url` and `outputDir` are required and `url` must be an absolute HTTP(S) URL, otherwise `400`. `201` on create, `404` on update of an unknown id. |
| `DELETE /feeds/{id}` | Delete a feed | `204` or `404` |
| `GET /processed-urls` | Paged list of processed URLs | `page`, `pageSize` (1-500, default 100), `status` (`succeeded`, `skipped`, `failed`, else `400`), `search`, `feedName`, `collectionName`, `jobId`, `sectionName`, `primarySectionOnly` |
| `DELETE /processed-urls?url` | Remove a processed URL and its content item so the next run retries it | `400` without `url`, `404` if unknown, `204` on success |
| `DELETE /processed-urls/failed` | Remove all failed records | `200` with the deleted count |
| `POST /urls/process` | Process a single URL outside the RSS pipeline | Body: `url`, `collectionName` (`blogs`, `news`, `videos`, `community`), optional `feedName`, `titleHint`, `transcript` (max 50,000 characters), `authorOverride`. `200` added or skipped, `400` invalid input, `409` URL already processed, `422` processing failed. |
| `GET /urls/title?url` | Fetch the page title for a URL (used by the add-content dialog) | `400` for non-HTTP(S) URLs, `200` with `title: null` when the fetch fails |

`/urls/process` and `/urls/title` make server-side requests to the HTTP(S) address a signed-in admin supplies. Outbound content fetching only connects to public internet addresses: loopback, private, link-local (cloud metadata) and other non-public destinations are blocked at connect time, including after redirects. See [input-validation-and-sanitization.md](input-validation-and-sanitization.md#outbound-request-protection-ssrf).

### Content items

| Endpoint | Purpose | Notes |
|----------|---------|-------|
| `GET /content-items` | Paged list | `page`, `pageSize` (1-500, default 100), `search`, `collectionName`, `feedName`, `sectionName`, `primarySectionOnly` |
| `GET /content-items/edit-data?collection&slug` | All editable fields of one item | `400` without parameters, `404` if unknown |
| `PUT /content-items/edit-data?collection&slug` | Save all editable fields | Requires title, author, primary section, at least one tag, at least one section and a positive date. `ai_metadata` must be valid JSON when present. `204`, `400` or `404`. |
| `POST /content-items/apply-transcript?collection&slug` | Apply a manually pasted transcript to a YouTube video and regenerate its AI content | Collection must be `videos`, transcript required and at most 50,000 characters. `400` invalid input, `404` unknown item, `422` AI excluded or failed to categorize, `502` AI service error, `200` with the updated item. |
| `POST /content-items/preview-markdown` | Render markdown to HTML for preview | `400` for empty markdown |
| `DELETE /content-items?collection&slug` | Delete an item (cascades to `processed_urls`) | `204`, `400` or `404` |

### Content reviews

The content fixer can propose changes as reviews instead of applying them (see [content-processing.md](content-processing.md)). These endpoints work through the queue.

Approval applies content changes, replaces the expanded tag index for tag reviews, and marks the selected reviews approved as one atomic operation. A failed approval leaves the content, tag index, and review statuses unchanged. Bulk approval only resolves the fetched batch; reviews created while it runs remain pending. Expanded tags retain the item's date and section metadata, and approving empty tags clears the old index entries.

| Endpoint | Purpose | Notes |
|----------|---------|-------|
| `GET /reviews?status&limit` | List reviews | `status` is `pending`, `approved` or `rejected` (else `400`). `limit` is 1-1000, default 100. |
| `GET /reviews/summary` | Counts per status | |
| `POST /reviews/{id}/approve`, `POST /reviews/{id}/reject` | Resolve one review | Approve applies the change. `404` if unknown. |
| `POST /reviews/approve-all`, `POST /reviews/reject-all` | Resolve all pending reviews | `200` with the count |
| `PUT /reviews/{id}` | Edit the proposed value of a pending review | `400` if `fixedValue` is missing, `404` if unknown |

### Custom pages, GitHub Copilot features and VS Code updates

| Endpoint | Purpose | Notes |
|----------|---------|-------|
| `GET /custom-pages`, `GET /custom-pages/{key}` | List entries or read raw JSON | `404` for unknown key |
| `PUT /custom-pages/{key}` | Replace the JSON of an existing entry | `400` for empty or invalid JSON, `404` if the key does not exist (entries cannot be created here). See [custom-pages.md](custom-pages.md). |
| `PUT /api/ghc-features/{slug}` | Create or update a GitHub Copilot feature | `400` for slug mismatch, empty title or null plans. `204`. |
| `DELETE /api/ghc-features/{slug}` | Delete a feature | `204` or `404` |
| `POST /api/ghc-features/{slug}/content-links` | Link a content item to a feature | Body: `collectionName`, `itemSlug`, `isThumbnail`, `sortOrder`. `400` or `404`, `204` on success. |
| `DELETE /api/ghc-features/{slug}/content-links?collection&itemSlug` | Remove a link | `204`, `400` or `404` |
| `PUT /api/ghc-features/{slug}/thumbnail?collection&itemSlug` | Choose the thumbnail link | `204`, `400` or `404` |
| `DELETE /ghc-features/{slug}` | Delete the `videos` content item with that slug | `204` or `404` |
| `GET /vscode-updates` | Paged list of VS Code update items | `page`, `pageSize` (1-500, default 100), `search` |
| `POST /vscode-updates` | Register an existing content item as a VS Code update | Body: `collectionName`, `slug`. `204` or `400`. |
| `DELETE /vscode-updates?collection&slug` | Remove from the list (the content item stays) | `204`, `400` or `404` |

The GitHub Copilot feature endpoints live under `/api/ghc-features` (not `/api/admin`). Their `GET` is public and the write operations require `AdminOnly`. The public `GET /api/vscode-updates` is capped at 100 items.

### Operations

| Endpoint | Purpose | Notes |
|----------|---------|-------|
| `POST /cache/invalidate` | Clear the API's entire in-memory cache | `200`. The settings page also refreshes the Web section and hero banner caches. |
| `GET /statistics` | Content counts, database sizes and processing metrics | |

### Newsletter

Subscriber management, send log and manual sends are under `/api/admin/newsletter`. They are documented in [newsletter.md](newsletter.md#api).

## Admin Pages

All pages render interactively on the server inside the admin layout. Unauthenticated visitors see the sign-in form instead of the page content, and the Web app re-challenges signed-in users whose cached token is gone (for example after a server restart).

| Page | Purpose |
|------|---------|
| `/admin` | Dashboard: job history with live logs, buttons to trigger processing, roundup generation and the content fixer, and to cancel the running job |
| `/admin/feeds` | Add, edit, enable or disable and delete RSS feeds |
| `/admin/processed-urls` | Search the URL ledger by status, feed, collection, section or job. Remove entries to retry them, clear all failed entries, and open the item editor. |
| `/admin/content-items` | Browse and filter content, edit or delete items, add a URL through the pipeline (the add-content dialog), and regenerate a roundup |
| `/admin/content-preview/{collection}/{slug}` | Render an item's markdown as it will appear on the site |
| `/admin/reviews` | Approve, reject or edit content fixer proposals, singly or in bulk, with a markdown preview |
| `/admin/custom-pages` | Edit the raw JSON behind the custom pages |
| `/admin/copilot-features` | Edit GitHub Copilot features (description, release date, plans, GHES support) and their linked content and thumbnail |
| `/admin/vscode-updates` | Add or remove items on the VS Code Updates page |
| `/admin/newsletter` | Subscribers, test sends and send log. See [newsletter.md](newsletter.md). |
| `/admin/settings` | Enable or disable scheduled jobs and invalidate all caches |
| `/admin/statistics` | Content and database statistics |
| `/admin/login` | Sign-in entry point |

The item editor (title, author, sections, tags, content, `ai_metadata`, and the transcript apply action for videos) is a shared dialog opened from the content items, processed URLs and reviews pages. AI metadata is read and saved together with the other editable fields through `GET` and `PUT /content-items/edit-data`.

## Related Documentation

- [admin-authentication.md](admin-authentication.md) - Entra ID sign-in, tokens and the `AdminOnly` policy
- [background-services.md](background-services.md) - What the trigger, cancel and setting endpoints control
- [content-processing.md](content-processing.md) - Pipeline, content fixer and review queue behavior
- [newsletter.md](newsletter.md) - Newsletter feature
- [custom-pages.md](custom-pages.md) - Custom page keys and data
