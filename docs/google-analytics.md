# Google Analytics

Google Analytics 4 (GA4) provides user behavior tracking and traffic analytics for Tech Hub in staging and production environments.

## Configuration

GA4 is configured via `appsettings.Production.json` in the Web project:

```json
{
  "GoogleAnalytics": {
    "MeasurementId": "G-95LLB67KJV"
  }
}
```

The measurement ID is read from `IConfiguration` in [App.razor](../src/TechHub.Web/Components/App.razor).

## Environment Behavior

GA4 follows the same conditional pattern as Application Insights:

| Environment | GA4 Active | Reason |
|-------------|-----------|--------|
| Development | No | `ASPNETCORE_ENVIRONMENT` is `Development` (first condition fails) |
| Staging | Yes | `appsettings.Production.json` is loaded (both staging and production run with `ASPNETCORE_ENVIRONMENT=Production`) |
| Production | Yes | Same as staging |

Both conditions must be true for GA to load:

1. `ASPNETCORE_ENVIRONMENT` is **not** `Development`
2. `GoogleAnalytics:MeasurementId` configuration value is **non-empty**

## What Is Tracked

### Page Views

- **Initial load**: GA4 script handles automatically via `gtag('config', ...)`
- **Blazor enhanced navigation**: Tracked manually via `Blazor.addEventListener('enhancedload', ...)` which sends a `page_view` event with path, location, and title

### Privacy Settings

GA4 is configured with privacy-friendly defaults:

- `anonymize_ip: true` — IP addresses are anonymized
- `allow_google_signals: false` — Remarketing signals disabled
- `allow_ad_personalization_signals: false` — Ad personalization disabled

## Script Loading

The GA4 script loads asynchronously (`<script async>`) and does not block page rendering. Two script blocks are injected:

1. **Head**: GA4 library + configuration (in `<head>`)
2. **Body**: Blazor enhanced navigation tracker (in `<body>`, after Blazor framework script)

## Implementation Reference

- [App.razor](../src/TechHub.Web/Components/App.razor) — Script injection and conditional rendering
- [appsettings.Production.json](../src/TechHub.Web/appsettings.Production.json) — Measurement ID configuration
