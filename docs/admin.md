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
| `POST /timeline-backfill/trigger` | Scan stored articles for timeline events (see [content-processing.md](content-processing.md#timeline-events)). Optional JSON body: `force` (scan scanned articles again), `publishedAfter` (`yyyy-MM-dd`), `collection` (lowercase letters, digits, hyphens) and `limit` (> 0) | `202`. `400` for an invalid option. `409` when a run is already queued |
| `POST /processing/cancel` | Cancel the currently running processing, roundup, fixer or timeline backfill run | `200` in both cases ("Cancellation requested" or "No running job to cancel"). Does not cancel the newsletter or ad-hoc URL runs. |
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

### Custom pages and VS Code updates

| Endpoint | Purpose | Notes |
|----------|---------|-------|
| `GET /custom-pages`, `GET /custom-pages/{key}` | List entries or read raw JSON | `404` for unknown key |
| `PUT /custom-pages/{key}` | Replace the JSON of an existing entry | `400` for empty or invalid JSON, `404` if the key does not exist (entries cannot be created here). See [custom-pages.md](custom-pages.md). |
| `GET /vscode-updates` | Paged list of VS Code update items | `page`, `pageSize` (1-500, default 100), `search` |
| `POST /vscode-updates` | Register an existing content item as a VS Code update | Body: `collectionName`, `slug`. `204` on success, `400` for invalid input, `404` for an unknown item. |
| `DELETE /vscode-updates?collection&slug` | Remove from the list (the content item stays) | `204`, `400` or `404` |

The public `GET /api/vscode-updates` is capped at 100 items. The GitHub Copilot features are edited under `/api/admin/products/{productId}/features` (see [Product Catalog](#product-catalog)); the public `GET /api/ghc-features` only reads them.

### Operations

| Endpoint | Purpose | Notes |
|----------|---------|-------|
| `POST /cache/invalidate` | Clear the API's entire in-memory cache | `200`. The settings page also refreshes the Web section and hero banner caches. |
| `GET /statistics` | Content counts, database sizes and processing metrics | |

### Newsletter

Subscriber management, send log and manual sends are under `/api/admin/newsletter`. They are documented in [newsletter.md](newsletter.md#api).

### Product Catalog

The catalog links the product names the AI returns to curated products (see [content-processing.md](content-processing.md#product-catalog)). Endpoints under `/api/admin/products`:

| Endpoint | Purpose | Result |
|---|---|---|
| `GET /products` | Products with aliases and the number of linked articles and dates. Optional `search` on name or alias | `200`, `400` for a search over 200 characters |
| `GET /products/{id}` | A product with its aliases, linked articles and linked dates | `200`, `404` |
| `POST /products` | Add a product (`name`) | `201` with the product, `400` for an empty or too long name, `409` when the name is already a product or an alias |
| `PUT /products/{id}` | Rename (`name`). The old name becomes an alias | `200`, `400`, `404`, `409` |
| `PUT /products/{id}/parent` | Make the product a child of another product (`parentId`), or top-level when `parentId` is null | `200` with the product, `400` for its own parent or a descendant, `404` |
| `DELETE /products/{id}` | Delete a product and its aliases. Its articles and dates become unmatched again | `204`, `404` |
| `POST /products/{id}/aliases` | Add an alias (`alias`), optionally implying a plan or surface (`qualifierId`) | `200` with the product, `400`, `404`, `409` |
| `PUT /products/{id}/aliases/{aliasId}/qualifier` | Set or clear the plan or surface an alias implies (`qualifierId`) | `200` with the product, `404` |
| `DELETE /products/{id}/aliases/{aliasId}` | Remove an alias | `204`, `404` |
| `POST /products/{id}/merge` | Merge into another product (`targetId`). The name and aliases become aliases of the target | `200` with the target, `400` when merging into itself, `404` |
| `GET /products/unmatched` | The review queue: names that are not in the catalog and not ignored, with the number of articles and dates, most used first | `200` |
| `GET /products/ignored` | The names that are marked as not being a product | `200` |
| `POST /products/ignored` | Mark a name as not being a product (`name`) | `204`, `400`, `409` when it is a product or an alias |
| `DELETE /products/ignored?name=` | Put an ignored name back in the review queue | `204`, `404` |

Error responses carry a plain message that the admin pages show.

The vocabulary of plans and surfaces (see [Plans and Surfaces](content-processing.md#plans-and-surfaces)) is managed under `/api/admin/qualifiers`:

| Endpoint | Purpose | Result |
|---|---|---|
| `GET /qualifiers` | Plans and surfaces with aliases and the number of linked dates and articles | `200` |
| `GET /qualifiers/unmatched` | Plan and surface names the AI returned that are not in the vocabulary, most used first | `200` |
| `POST /qualifiers` | Add a plan or surface (`kind` is `plan` or `surface`, `name`) | `201` with the qualifier, `400`, `409` |
| `PUT /qualifiers/{id}` | Rename (`name`). The old name becomes an alias | `200`, `400`, `404`, `409` |
| `DELETE /qualifiers/{id}` | Delete a qualifier and its aliases. Dates and product aliases lose it | `204`, `404` |
| `POST /qualifiers/{id}/aliases` | Add an alias (`alias`) | `200` with the qualifier, `400`, `404`, `409` |
| `DELETE /qualifiers/{id}/aliases/{aliasId}` | Remove an alias | `204`, `404` |

The curated features of a product (see [Features](content-processing.md#features)) are managed under `/api/admin/products/{productId}/features`. Every call addresses a feature together with its product, so a feature of another product is never touched:

| Endpoint | Purpose | Result |
|---|---|---|
| `GET /products/features/unmatched` | The products that have dates with a feature name that matches no feature, with the number of different names and dates, most dates first. Feeds the review overview | `200` |
| `GET /products/{productId}/features` | The features with aliases and number of dates, and the feature names of the dates of the product (and its children) that match no feature | `200` |
| `POST /products/{productId}/features` | Add a feature (`name`) | `201` with the overview, `400`, `404`, `409` |
| `PUT /products/{productId}/features/{id}` | Rename (`name`). The old name becomes an alias | `200` with the overview, `400`, `404`, `409` |
| `DELETE /products/{productId}/features/{id}` | Delete a feature and its aliases. Its dates keep their text and become unmatched | `204`, `404` |
| `POST /products/{productId}/features/{id}/aliases` | Add an alias (`alias`) | `200` with the overview, `400`, `404`, `409` |
| `DELETE /products/{productId}/features/{id}/aliases/{aliasId}` | Remove an alias | `204`, `404` |
| `PUT /products/{productId}/features/{id}/details` | Set the description (at most 4000 characters) and the release date (`yyyy-MM-dd`, empty clears it: the features page then uses the first launch, preview, GA or release date of the dates) | `200` with the overview, `400`, `404` |
| `POST /products/{productId}/features/{id}/content` | Link a content item (video, post) to a feature (`collectionName`, `itemSlug`, `isThumbnail`). Setting `isThumbnail` clears the previous thumbnail | `200` with the overview, `400`, `404` (feature or item) |
| `DELETE /products/{productId}/features/{id}/content?collection&itemSlug` | Remove a link; the content item stays | `204`, `400`, `404` |
| `PUT /products/{productId}/features/{id}/thumbnail?collection&itemSlug` | Make a linked item the thumbnail | `204`, `400`, `404` |

## Admin Pages

All pages render interactively on the server inside the admin layout. Unauthenticated visitors see the sign-in form instead of the page content, and the Web app re-challenges signed-in users whose cached token is gone (for example after a server restart).

### Navigation

The menu has two levels. The top bar lists the groups; a bar below it is always shown and lists the pages of the active group, centred (a group with one page shows that page). The groups and pages are defined in `AdminNavigation` (`Components/Layout/AdminNavigation.cs`), which the layout renders and the layout tests cover.

The right side of the second bar holds the actions of the current page, which is its **Refresh** button. Each page decides what refreshing does, so the button belongs to the page: it is wrapped in `AdminSubbarAction`, which the admin layout renders in the bar (a layout section) and which renders inline when a page is used outside the layout, for example in a component test.

| Group | Pages |
|---|---|
| Automatic Processing | Dashboard, Feeds, Processed |
| Content Management | Content, VSC Updates, Pages |
| Product Management | Products, Plans & surfaces |
| Reviews | Reviews |
| Newsletter | Subscribers, Send log |
| Stats | Stats |
| Settings | Settings |

### Pages

| Page | Purpose |
|------|---------|
| `/admin/reviews` | Everything that needs a decision, on one page: cards with the number of product names, plans and surfaces, features, content fixes and failed URLs to review, each linking to its queue. Below them: the product names to review (add as a product, add as an alias, or mark as not a product, with the list of names that are not products), the plan and surface names to review and the products with features to review (both link to the page where they are handled), and the content fixer proposals (approve, reject or edit them, singly or in bulk, with a markdown preview). **Run Cleanup** starts the content fixer from here |
| `/admin` | Dashboard: job history with live logs, and a button to cancel the running job. Jobs started from Settings (processing, roundup, timeline backfill) and from Reviews (content cleanup) show up here while they run |
| `/admin/feeds` | Add, edit, enable or disable and delete RSS feeds |
| `/admin/processed-urls` | Search the URL ledger by status, feed, collection, section or job. Remove entries to retry them, clear all failed entries, and open the item editor. |
| `/admin/content-items` | Browse and filter content, edit or delete items, add a URL through the pipeline (the add-content dialog), and regenerate a roundup |
| `/admin/content-preview/{collection}/{slug}` | Render an item's markdown as it will appear on the site |
| `/admin/vscode-updates` | Add or remove items on the VS Code Updates page |
| `/admin/custom-pages` | Edit the raw JSON behind the custom pages |
| `/admin/products` | Product list with search (name or alias; at most three aliases are shown, the rest on hover) and adding a product (see [Product Catalog](#product-catalog)) |
| `/admin/products/{id}` | One product, in three tabs. **General** (`/admin/products/{id}`): rename, aliases (each can imply a plan or surface), parent and children, merge into another product and delete. **Features** (`/admin/products/{id}/features`): curated features with the description, release date and linked content (videos and posts, one of them the thumbnail) of each feature, the feature names to review and the features by surface overview. **Dates and articles** (`/admin/products/{id}/activity`): the linked dates and articles, including those of its child products |
| `/admin/products/qualifiers` | Plans and surfaces: the review queue of unmatched names and the vocabulary with its aliases |
| `/admin/newsletter` | Subscribers with search and confirmed filter, edit, delete and test sends. Deleting requires inline confirmation. Test sends require an entered recipient email; none is prefilled. Refresh, delete, edit and test-send failures show generic errors, and a failed refresh after a deletion or edit does not show a success message. See [newsletter.md](newsletter.md). |
| `/admin/newsletter/send-log` | The last 100 newsletter sends with their recipient and failure counts |
| `/admin/statistics` | Content and database statistics |
| `/admin/settings` | Enable or disable scheduled jobs, start processing or roundup generation now, invalidate all caches and start the timeline backfill |
| `/admin/login` | Sign-in entry point |

The item editor (title, author, sections, tags, content, `ai_metadata`, and the transcript apply action for videos) is a shared dialog opened from the content items, processed URLs and reviews pages. AI metadata is read and saved together with the other editable fields through `GET` and `PUT /content-items/edit-data`.

## Related Documentation

- [admin-authentication.md](admin-authentication.md) - Entra ID sign-in, tokens and the `AdminOnly` policy
- [background-services.md](background-services.md) - What the trigger, cancel and setting endpoints control
- [content-processing.md](content-processing.md) - Pipeline, content fixer and review queue behavior
- [newsletter.md](newsletter.md) - Newsletter feature
- [custom-pages.md](custom-pages.md) - Custom page keys and data
