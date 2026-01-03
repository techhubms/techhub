# Google Analytics Specification

> **Feature**: GA4 integration with privacy-compliant tracking, Core Web Vitals monitoring, and event analytics

## Overview

The Google Analytics system integrates GA4 (Google Analytics 4) for comprehensive user behavior tracking, performance monitoring, and business insights. Implementation follows privacy best practices (GDPR/CCPA compliance), includes cookie consent management, and tracks modern web metrics including Core Web Vitals.

## Requirements

### Functional Requirements

**FR-1**: The system MUST integrate Google Analytics 4 (GA4)  
**FR-2**: The system MUST track page views on initial load and navigation  
**FR-3**: The system MUST track custom events (search, filter, infinite scroll, content share)  
**FR-4**: The system MUST track Core Web Vitals (LCP, FID, CLS, TTFB)  
**FR-5**: The system MUST track user engagement metrics (time on page, scroll depth)  
**FR-6**: The system MUST implement cookie consent banner (GDPR/CCPA compliant)  
**FR-7**: The system MUST respect Do Not Track (DNT) browser settings  
**FR-8**: The system MUST support IP anonymization  
**FR-9**: The system MUST track content interactions (video plays, external link clicks)  
**FR-10**: The system MUST provide server-side tracking option for privacy  

### Non-Functional Requirements

**NFR-1**: Analytics script MUST NOT block page rendering (async loading)  
**NFR-2**: Analytics overhead MUST be < 50KB gzipped  
**NFR-3**: Event tracking MUST NOT impact user experience (< 10ms)  
**NFR-4**: Cookie consent MUST appear within 500ms of page load  
**NFR-5**: Data collection MUST comply with GDPR, CCPA, and privacy regulations  
**NFR-6**: Failed tracking MUST NOT cause JavaScript errors  

## Use Cases

### UC-1: Track Page View

**Actor**: User  
**Precondition**: User visits any page  
**Trigger**: Page loads  

**Flow**:

1. User navigates to `/github-copilot/`
2. Page renders (Blazor SSR)
3. GA4 script loads asynchronously
4. System checks cookie consent status
5. If consent granted:
   - System sends `page_view` event to GA4
   - System sends page_title, page_location, page_path
   - System sends custom dimensions (section, collection)
6. If consent not granted:
   - System shows cookie banner
   - System queues tracking until consent

**Postcondition**: Page view recorded in GA4

### UC-2: Track Custom Event (Search Query)

**Actor**: User  
**Precondition**: User performs search  
**Trigger**: User submits search query  

**Flow**:

1. User types "copilot features" and presses Enter
2. Search results display
3. System sends custom event to GA4:

   ```javascript
   gtag('event', 'search', {
     search_term: 'copilot features',
     search_results_count: 42,
     event_category: 'engagement',
     event_label: 'site_search'
   });
   ```

4. GA4 records search event with parameters

**Postcondition**: Search query tracked for analytics

### UC-3: Track Core Web Vitals

**Actor**: System  
**Precondition**: Page has loaded  
**Trigger**: Performance metrics available  

**Flow**:

1. Page finishes loading
2. Browser calculates Core Web Vitals:
   - LCP (Largest Contentful Paint)
   - FID (First Input Delay)
   - CLS (Cumulative Layout Shift)
   - TTFB (Time to First Byte)
3. System sends metrics to GA4:

   ```javascript
   gtag('event', 'web_vitals', {
     metric_name: 'LCP',
     metric_value: 1234, // milliseconds
     metric_rating: 'good' // good/needs-improvement/poor
   });
   ```

4. GA4 records performance metrics

**Postcondition**: Core Web Vitals data available for analysis

### UC-4: Handle Cookie Consent

**Actor**: User  
**Precondition**: First visit to site  
**Trigger**: Page loads  

**Flow**:

1. User visits site for first time
2. System checks for consent cookie
3. Cookie not found, system shows banner:
   "We use cookies to analyze site usage. [Accept] [Decline] [Settings]"
4. User clicks "Accept"
5. System sets consent cookie (1 year expiry)
6. System enables GA4 tracking
7. System sends queued events to GA4

**Alternative Flow** (User Declines):

1. User clicks "Decline"
2. System sets consent=false cookie
3. System disables GA4 tracking
4. System clears any tracking cookies
5. No data sent to GA4

**Postcondition**: User consent preferences saved

### UC-5: Track Infinite Scroll Interaction

**Actor**: User  
**Precondition**: User scrolls content list  
**Trigger**: Infinite scroll loads more items  

**Flow**:

1. User scrolls to 80% of content
2. System loads next 20 items via API
3. System sends custom event:

   ```javascript
   gtag('event', 'infinite_scroll', {
     event_category: 'engagement',
     page_number: 2,
     items_loaded: 20,
     section: 'github-copilot',
     collection: 'news'
   });
   ```

4. GA4 records scroll interaction

**Postcondition**: Infinite scroll usage tracked

## Event Tracking

### Standard Events

**Page View**:

```javascript
gtag('event', 'page_view', {
  page_title: 'GitHub Copilot - Tech Hub',
  page_location: 'https://tech.hub.ms/github-copilot/',
  page_path: '/github-copilot/',
  section: 'github-copilot',
  collection: null
});
```

**Search**:

```javascript
gtag('event', 'search', {
  search_term: 'azure deployment',
  search_results_count: 58,
  section: 'azure'
});
```

**Filter Application**:

```javascript
gtag('event', 'filter_applied', {
  event_category: 'engagement',
  filter_type: 'tag', // or 'date', 'text'
  filter_value: 'github-copilot',
  section: 'ai'
});
```

**Content Share**:

```javascript
gtag('event', 'share', {
  method: 'twitter', // or 'linkedin', 'copy_link'
  content_type: 'article',
  item_id: '2025-01-02-chat-in-ide'
});
```

**External Link Click**:

```javascript
gtag('event', 'click', {
  event_category: 'outbound',
  event_label: 'https://docs.microsoft.com/copilot',
  link_domain: 'docs.microsoft.com'
});
```

**Video Play**:

```javascript
gtag('event', 'video_start', {
  video_title: 'GitHub Copilot Chat Demo',
  video_url: 'https://youtube.com/watch?v=VIDEO_ID',
  video_duration: 330 // seconds
});
```

### Custom Dimensions

**User-Scoped**:

- `user_type`: "new" | "returning"
- `consent_status`: "granted" | "denied" | "pending"

**Session-Scoped**:

- `session_start_section`: Section where session began
- `device_category`: "desktop" | "mobile" | "tablet"

**Event-Scoped**:

- `section`: Current section (e.g., "github-copilot")
- `collection`: Current collection (e.g., "news")
- `content_category`: Content category from frontmatter
- `content_tags`: Tags array from frontmatter

## Core Web Vitals Tracking

### Implementation

**Using web-vitals Library**:

```javascript
import { onCLS, onFID, onLCP, onTTFB } from 'web-vitals';

function sendToGA4(metric) {
  // Rating based on thresholds
  let rating = 'good';
  if (metric.name === 'LCP') {
    rating = metric.value <= 2500 ? 'good' : metric.value <= 4000 ? 'needs-improvement' : 'poor';
  } else if (metric.name === 'FID') {
    rating = metric.value <= 100 ? 'good' : metric.value <= 300 ? 'needs-improvement' : 'poor';
  } else if (metric.name === 'CLS') {
    rating = metric.value <= 0.1 ? 'good' : metric.value <= 0.25 ? 'needs-improvement' : 'poor';
  }
  
  gtag('event', 'web_vitals', {
    metric_name: metric.name,
    metric_value: Math.round(metric.value),
    metric_rating: rating,
    metric_delta: metric.delta,
    metric_id: metric.id
  });
}

onCLS(sendToGA4);
onFID(sendToGA4);
onLCP(sendToGA4);
onTTFB(sendToGA4);
```

### Metrics Tracked

| Metric | Description | Good | Needs Improvement | Poor |
| -------- | ------------- | ------ | ------------------- | ------ |
| LCP | Largest Contentful Paint | ≤ 2.5s | 2.5s - 4.0s | > 4.0s |
| FID | First Input Delay | ≤ 100ms | 100ms - 300ms | > 300ms |
| CLS | Cumulative Layout Shift | ≤ 0.1 | 0.1 - 0.25 | > 0.25 |
| TTFB | Time to First Byte | ≤ 800ms | 800ms - 1800ms | > 1800ms |

## Privacy & Compliance

### Cookie Consent Banner

**Requirements**:

- Must appear on first visit
- Must be dismissible
- Must provide granular controls
- Must persist user choice
- Must respect Do Not Track (DNT)

**Cookie Banner UI**:

```html
<div class="cookie-banner" aria-live="polite" role="dialog">
  <p>
    We use cookies to understand how you use our site and improve your experience.
    This includes analytics cookies.
  </p>
  <div class="cookie-actions">
    <button onclick="acceptCookies()">Accept All</button>
    <button onclick="declineCookies()">Decline</button>
    <button onclick="showCookieSettings()">Cookie Settings</button>
  </div>
</div>
```

### Cookie Types

**Essential Cookies** (always enabled):

- `consent_preferences`: User's cookie consent choice (1 year)
- `session_id`: Server-side session identifier (session)

**Analytics Cookies** (require consent):

- `_ga`: GA4 client ID (2 years)
- `_ga_<container-id>`: GA4 session storage (2 years)
- `_gid`: GA4 session identifier (24 hours)

### GDPR/CCPA Compliance

**Data Collection Transparency**:

- Clear privacy policy link
- Explanation of data collection
- Purpose of tracking
- Data retention policy
- User rights (access, deletion, portability)

**User Rights**:

- Right to opt-out (cookie banner)
- Right to access data (GA4 data export)
- Right to deletion (data deletion request form)
- Right to portability (export personal data)

### IP Anonymization

**GA4 Configuration**:

```javascript
gtag('config', 'G-XXXXXXXXXX', {
  'anonymize_ip': true,
  'allow_google_signals': false, // Disable remarketing
  'allow_ad_personalization_signals': false
});
```

## Blazor Integration

### Analytics Service

**IAnalyticsService.cs**:

```csharp
public interface IAnalyticsService
{
    Task TrackPageViewAsync(string pageTitle, string pageUrl);
    Task TrackEventAsync(string eventName, Dictionary<string, object> parameters);
    Task TrackSearchAsync(string searchTerm, int resultsCount);
    Task TrackFilterAsync(string filterType, string filterValue);
    Task TrackWebVitalAsync(string metricName, double value, string rating);
    bool IsConsentGranted();
}
```

**AnalyticsService.cs**:

```csharp
public class AnalyticsService : IAnalyticsService
{
    private readonly IJSRuntime _js;
    private readonly ILogger<AnalyticsService> _logger;
    
    public async Task TrackPageViewAsync(string pageTitle, string pageUrl)
    {
        if (!IsConsentGranted()) return;
        
        try
        {
            await _js.InvokeVoidAsync("gtag", "event", "page_view", new
            {
                page_title = pageTitle,
                page_location = pageUrl
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to track page view");
        }
    }
    
    public bool IsConsentGranted()
    {
        // Check cookie consent status
        // Return false if user declined or DNT enabled
        return true; // Simplified
    }
}
```

### Component Integration

**ContentPage.razor**:

```razor
@inject IAnalyticsService Analytics
@implements IAsyncDisposable

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Analytics.TrackPageViewAsync(PageTitle, Navigation.Uri);
        }
    }
    
    private async Task HandleSearchAsync(string query)
    {
        var results = await SearchService.SearchAsync(query);
        await Analytics.TrackSearchAsync(query, results.TotalCount);
    }
}
```

## Server-Side Tracking (Optional)

### Measurement Protocol API

**For Privacy-Focused Alternative**:

```csharp
public class ServerSideAnalytics
{
    private readonly HttpClient _http;
    private readonly string _measurementId;
    private readonly string _apiSecret;
    
    public async Task TrackPageViewAsync(string clientId, string pageUrl)
    {
        var payload = new
        {
            client_id = clientId,
            events = new[]
            {
                new
                {
                    name = "page_view",
                    @params = new
                    {
                        page_location = pageUrl,
                        engagement_time_msec = "100"
                    }
                }
            }
        };
        
        var url = $"https://www.google-analytics.com/mp/collect?measurement_id={_measurementId}&api_secret={_apiSecret}";
        await _http.PostAsJsonAsync(url, payload);
    }
}
```

## Testing Strategy

### Unit Tests

- Test consent cookie logic
- Test event parameter generation
- Test DNT detection
- Test IP anonymization flag
- Test event queuing when consent pending

### Integration Tests

- Test GA4 script loading
- Test event firing on user actions
- Test Core Web Vitals measurement
- Test cookie banner display/dismiss

### E2E Tests (Playwright)

- Test cookie banner appears on first visit
- Test accepting/declining cookies
- Test GA4 script loads after consent
- Test tracking disabled when DNT enabled
- Test events fire on user interactions

## Analytics Dashboard

### Key Metrics to Monitor

**Traffic Metrics**:

- Page views
- Unique visitors
- Sessions
- Bounce rate
- Average session duration

**Content Metrics**:

- Most viewed articles
- Most searched terms
- Most applied filters
- Most shared content

**Performance Metrics**:

- Core Web Vitals (LCP, FID, CLS)
- Page load times
- API response times
- Infinite scroll engagement

**User Behavior**:

- Search-to-click rate
- Filter usage patterns
- Scroll depth
- Exit pages

## Acceptance Criteria

**AC-1**: Given user visits site, when page loads, then GA4 page_view event fires  
**AC-2**: Given user has not consented, when page loads, then cookie banner appears  
**AC-3**: Given user accepts cookies, when tracking events occur, then events sent to GA4  
**AC-4**: Given user declines cookies, when tracking events occur, then no data sent to GA4  
**AC-5**: Given user performs search, when results display, then search event tracked with query and count  
**AC-6**: Given page loads, when Core Web Vitals measured, then metrics sent to GA4  
**AC-7**: Given DNT enabled, when page loads, then no tracking occurs  
**AC-8**: Given user scrolls to load more content, when infinite scroll triggers, then event tracked  

## Performance Optimization

**Async Script Loading**:

```html
<!-- Load GA4 asynchronously -->
<script async src="https://www.googletagmanager.com/gtag/js?id=G-XXXXXXXXXX"></script>
<script>
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());
  gtag('config', 'G-XXXXXXXXXX', {
    'anonymize_ip': true,
    'cookie_flags': 'SameSite=None;Secure'
  });
</script>
```

**Event Batching**:

- Queue events when offline
- Batch send when connection restored
- Limit event frequency (debounce rapid events)

## Accessibility Considerations

- Cookie banner MUST be keyboard accessible
- Cookie banner MUST have ARIA labels
- Screen readers MUST announce banner appearance
- Banner MUST not trap focus
- Banner settings MUST be accessible via keyboard

## Migration Notes

**From Jekyll**:

- Jekyll used `google-analytics.html` include with UA (Universal Analytics)
- **UPGRADE**: Migrate from UA to GA4
- **ADDED**: Cookie consent banner (GDPR/CCPA compliance)
- **ADDED**: Core Web Vitals tracking
- **ADDED**: Custom event tracking (search, filters, infinite scroll)
- **ADDED**: Server-side tracking option
- **ADDED**: IP anonymization and privacy controls
- Maintain same GA property (migrate UA → GA4 in same property)

**Migration Steps**:

1. Create GA4 property in Google Analytics
2. Update tracking code from UA to GA4
3. Implement cookie consent banner
4. Add custom event tracking
5. Configure Core Web Vitals reporting
6. Test tracking in GA4 DebugView
7. Verify data collection compliance

## Open Questions

1. Should we implement Google Tag Manager for easier tag management?
2. Should we track video engagement (25%, 50%, 75%, 100% completion)?
3. Should we implement custom reports/dashboards?
4. Should we integrate with Microsoft Clarity for session recordings?
5. Should we track A/B test variants?

## References

- [GA4 Documentation](https://developers.google.com/analytics/devguides/collection/ga4)
- [web-vitals Library](https://github.com/GoogleChrome/web-vitals)
- [GDPR Compliance Guide](https://gdpr.eu/)
- [Cookie Consent Best Practices](https://www.cookiebot.com/en/gdpr-cookies/)
