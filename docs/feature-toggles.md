# Feature Toggles

A feature toggle lets an admin switch a feature off, show it to admins only, or open it to everyone, without a deployment. It is how a new feature is tried on the live site before the public sees it.

## Modes

| Mode | Who can use the feature |
|------|-------------------------|
| `Disabled` | Nobody |
| `Admins` | Visitors that are signed in as admin |
| `Everyone` | All visitors |

A feature that has no row in the database is disabled.

## What a Visitor Sees

The public pages never mention signing in. Only an admin knows the sign-in at `/admin` exists, and signing in there sets the normal authentication cookie (path `/`), which the browser sends to every page. The Web app reads it on the public pages to decide what to show:

- **Buttons.** A collection that has a `Feature` in `appsettings.json` is left out of the navigation, the sub navigation and the section cards unless the toggle lets the visitor in.
- **Direct URLs.** The page of a feature that is not available to the visitor answers with the normal `404` and the "Page Not Found" content, not a redirect or a login prompt.
- **Sitemap.** A collection with a feature is only in `/api/sitemap` when the feature is open to `Everyone`.

`/admin` itself still shows its sign-in form to anyone who opens it.

An admin is any signed-in user (see [admin-authentication.md](admin-authentication.md)). A visitor who signs in after opening a page sees the change after a reload, because Blazor decides at the start of a page or circuit.

## Defining a Feature

1. Add a constant to `FeatureKeys` ([FeatureKeys.cs](../src/TechHub.Core/Models/FeatureToggles/FeatureKeys.cs)).
2. Add a row to `feature_toggles` in a migration, with the mode it starts in and a description for the admin page. Currently `timeline` (migration `051_feature_toggles.sql`), which starts as `admins`.
3. Gate the feature:
   - For navigation buttons: set `"Feature": "<key>"` on the collection in `appsettings.json`. `NavHeader`, `SubNav` and `SectionCard` already filter on it.
   - For a page: inject `IFeatureGate` and render `NotFoundContent` when `await FeatureGate.IsEnabledAsync(key)` is false, as `SectionTimeline.razor` does.

Do not name a feature in the code that decides who sees what: the toggle row and the `Feature` property carry that.

## Architecture

- **Table `feature_toggles`**: `feature_key` (primary key), `mode` (`disabled`, `admins` or `everyone`, enforced by a check constraint), `description` and `updated_at`.
- **API**: `GET /api/feature-toggles` is public and returns every toggle with its mode as text. `PUT /api/admin/feature-toggles/{featureKey}` with `{"mode": "Disabled|Admins|Everyone"}` is under the `AdminOnly` policy and answers `204`, `400` for an invalid key or mode, or `404` for an unknown feature. The API is only reachable from the Web app (see [network-architecture.md](network-architecture.md)), so the Web app is where a visitor is evaluated.
- **Web**: `FeatureToggleCache` is a singleton copy of the modes, refreshed every 30 seconds by `FeatureToggleCacheRefreshService` and immediately after an admin saves a change. A failed refresh keeps the previous modes; before the first successful load every feature is disabled. `IFeatureGate` combines the cache with the signed-in state of the visitor.
- **Admin page**: `/admin/feature-toggles` (Settings group) has one select per feature.

## Related Documentation

- [admin.md](admin.md) - The admin area
- [admin-authentication.md](admin-authentication.md) - Signing in
- [caching.md](caching.md) - The other Web caches
