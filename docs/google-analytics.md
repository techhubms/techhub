# Google Analytics

Google Analytics 4 (GA4) provides user behavior tracking and traffic analytics for Tech Hub in production only.

## Configuration

GA4 is configured through the `GoogleAnalytics:MeasurementId` configuration key, which the Web container receives as the environment variable `GoogleAnalytics__MeasurementId`. No `appsettings*.json` file in the Web project contains it.

| Where | What |
|-------|------|
| [infra/parameters/prod-applications.bicepparam](../infra/parameters/prod-applications.bicepparam) | Production measurement ID value |
| [infra/applications.bicep](../infra/applications.bicep) | `googleAnalyticsMeasurementId` parameter (with a default), passed to the web module |
| [infra/modules/web.bicep](../infra/modules/web.bicep) | Sets the `GoogleAnalytics__MeasurementId` app setting on the Web app |
| [infra/pr-applications.bicep](../infra/pr-applications.bicep) | Sets it to an empty string so PR preview traffic never appears in GA reports |

The measurement ID is read from `IConfiguration` in [App.razor](../src/TechHub.Web/Components/App.razor).

## Environment Behavior

Whether GA4 loads depends on `ASPNETCORE_ENVIRONMENT` and on whether the deployment supplies a measurement ID:

| Environment | GA4 Active | Reason |
|-------------|-----------|--------|
| Development | No | First condition fails (`!= "Development"` is false) |
| PR preview | No | The Bicep deployment sets an empty measurement ID |
| Production | Yes | The Bicep deployment provides the measurement ID |
| Any other non-Development environment | Only if the deployment sets `GoogleAnalytics__MeasurementId` | Second condition |

Both conditions must be true for GA to load:

1. `ASPNETCORE_ENVIRONMENT` is **not** `Development`
2. `GoogleAnalytics:MeasurementId` configuration value is **non-empty**

## What Is Tracked

### Page Views

- **Initial load**: Tracked manually via the `enhancedload` handler (see below). The GA4 config uses `send_page_view: false` to suppress the automatic initial page view, avoiding double-counting.
- **Blazor enhanced navigation**: Tracked manually via `Blazor.addEventListener('enhancedload', ...)` which sends a `page_view` event with path, location, and title. A `window._gaEnhancedLoadRegistered` guard flag prevents that listener from being registered more than once in case the script tag is ever re-executed.

### Privacy Settings

GA4 is configured with privacy-friendly defaults:

- `anonymize_ip: true` — IP addresses are anonymized
- `allow_google_signals: false` — Remarketing signals disabled
- `allow_ad_personalization_signals: false` — Ad personalization disabled

## Script Loading

The GA4 script loads asynchronously (`<script async>`) and does not block page rendering. Two script blocks are injected:

1. **Head**: GA4 library + configuration (in `<head>`)
2. **Body**: Blazor enhanced navigation tracker (in `<body>`, after Blazor framework script)

## Ad Blocker Noise Suppression

Ad blockers — both browser extensions (uBlock Origin, etc.) and DNS-level blockers (Pi-hole, NextDNS) — block requests to `google-analytics.com` and `googletagmanager.com`. The Application Insights Browser SDK auto-instruments `fetch()` and records every blocked request as a failed dependency, which pollutes the Application Insights dependency failure view and masks real failures.

To prevent this, the Application Insights snippet is configured with an `onInit` callback that registers a telemetry initializer. The initializer drops any `RemoteDependencyData` envelope whose target contains `google-analytics.com` or `googletagmanager.com` before it is sent to Application Insights.

This suppression is client-side only and has no effect on actual GA4 data collection — it only prevents the noise from appearing in Application Insights.

## Implementation Reference

- [App.razor](../src/TechHub.Web/Components/App.razor) — Script injection and conditional rendering
- [web.bicep](../infra/modules/web.bicep) — Measurement ID app setting
- [prod-applications.bicepparam](../infra/parameters/prod-applications.bicepparam) — Production measurement ID
