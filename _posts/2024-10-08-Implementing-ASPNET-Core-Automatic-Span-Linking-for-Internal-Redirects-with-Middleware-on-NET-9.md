---
layout: "post"
title: "Implementing ASP.NET Core Automatic Span Linking for Internal Redirects with Middleware on .NET 9"
description: "Steve Gordon explores how to implement automatic span (activity) linking between request traces in ASP.NET Core during internal redirects using OpenTelemetry and new .NET 9 features. The post details middleware setup, trace propagation, example code, and production considerations to enhance observability for .NET applications."
author: "Steve Gordon"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.stevejgordon.co.uk/implementing-aspnetcore-span-linking-for-redirects-with-middleware"
viewing_mode: "external"
feed_name: "Steve Gordon's Blog"
feed_url: "https://www.stevejgordon.co.uk/feed"
date: 2024-10-08 07:20:31 +00:00
permalink: "/2024-10-08-Implementing-ASPNET-Core-Automatic-Span-Linking-for-Internal-Redirects-with-Middleware-on-NET-9.html"
categories: ["Coding", "DevOps"]
tags: [".NET 9", "Activity", "ASP.NET Core", "Coding", "DevOps", "Distributed Tracing", "ITempDataDictionaryFactory", "Middleware", "Observability", "OpenTelemetry", "Posts", "Span Linking", "Tracing"]
tags_normalized: ["net 9", "activity", "asp dot net core", "coding", "devops", "distributed tracing", "itempdatadictionaryfactory", "middleware", "observability", "opentelemetry", "posts", "span linking", "tracing"]
---

In this detailed post by Steve Gordon, the author demonstrates a practical approach to implementing automatic span linking for internal redirects in ASP.NET Core apps with .NET 9, enhancing traceability and observability.<!--excerpt_end-->

# Implementing ASP.NET Core Automatic Span (Activity) Linking for Internal Redirects with Middleware on .NET 9

**Author:** Steve Gordon  

## Introduction

Steve Gordon continues his series based on hands-on experience with OpenTelemetry instrumentation in .NET applications. In this post, he focuses on a practical enhancement that enables automatic span (activity) links between request traces in ASP.NET Core during internal redirects using middleware and features introduced in .NET 9.

> **Note:** The implementation utilizes a new API in .NET 9, enabling links to be added to an existing `Activity` after creation. This is not natively supported in .NET 8 or earlier unless you explicitly depend on `System.Diagnostics.DiagnosticSource` version 9.0.0 or newer.

## Scenario

In many applications, internal redirects are common—for example, in OAuth authentication flows, where a request is redirected to a login page and then, via callback, redirected again to the originally requested endpoint. Tracking causality between such requests is crucial for end-to-end traceability but becomes challenging, as HTTP headers for trace propagation are not preserved through browser-followed redirects.

## Approach: TempData and Span Linking

Standard solutions may involve passing trace context via query strings or cookies. In this implementation, Steve leverages the `TempData` mechanism (which is cookie-backed by default) to preserve necessary span/trace information across redirects within ASP.NET Core.

> **Disclaimer:** The provided code is illustrative, not heavily production-tested, and may not handle all edge cases. Validate before deploying to production.

## Middleware Implementation Walkthrough

### Middleware Setup

```csharp
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Frozen;
using System.Diagnostics;

namespace MyApp.Middleware;

public class RedirectActivityLinkingMiddleware(
    RequestDelegate next,
    ITempDataDictionaryFactory tempDataDictionaryFactory)
{
    private readonly RequestDelegate _next = next;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory = tempDataDictionaryFactory;

    private const string RedirectParentActivityId = nameof(RedirectParentActivityId);
    private const string RedirectTimestamp = nameof(RedirectTimestamp);
    private const string RedirectTarget = nameof(RedirectTarget);

    private static readonly FrozenSet<int> RedirectStatusCodes = FrozenSet.ToFrozenSet([301, 302, 303, 307, 308]);

    public async Task Invoke(HttpContext context)
    {
        var tempData = _tempDataDictionaryFactory.GetTempData(context);
        // More code follows…
        await _next(context);
    }
}
```

- **Dependencies:** Uses `ITempDataDictionaryFactory` (available from the service provider in standard ASP.NET Core applications).
- **Constants:** Sets up keys and a frozen set of recognized redirect status codes.

### Storing Trace Info During Redirects

Within the middleware, register a `Response.OnStarting` delegate to capture redirect information before the response is sent:

```csharp
context.Response.OnStarting(static state => {
    var context = (HttpContext)state;
    var activity = context.Features.Get<IHttpActivityFeature>()?.Activity;
    if (activity is null || !activity.Recorded) return Task.CompletedTask;
    if (RedirectStatusCodes.Contains(context.Response.StatusCode)) {
        if (context.Response.Headers.Location.Count != 1) return Task.CompletedTask;
        var location = context.Response.Headers.Location[0];
        // Apply only to relative URLs
        if (string.IsNullOrEmpty(location) || location[0] != '/') return Task.CompletedTask;
        var factory = context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
        var tempData = factory.GetTempData(context);
        // Store current trace/activity info, timestamp, and target
        tempData[RedirectParentActivityId] = activity.Id;
        tempData[RedirectTimestamp] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        tempData[RedirectTarget] = location;
        tempData.Save();
    }
    return Task.CompletedTask;
}, context);
```

#### Notes

- Only attaches data for recognized redirect status codes (relative local URLs only).
- Uses TempData for temporary and secure storage of trace context.

### Linking the Next Request to the Previous Activity

On the target of the redirect, fetch stored info and add a span link if appropriate:

```csharp
var activity = Activity.Current;
if (
    activity?.IsAllDataRequested == true &&
    tempData.TryGetValue(RedirectParentActivityId, out var parentActivityIdObject) &&
    tempData.TryGetValue(RedirectTimestamp, out var tempDataTimestampObject) &&
    tempData.TryGetValue(RedirectTarget, out var redirectTargetObject) &&
    parentActivityIdObject is string parentId &&
    tempDataTimestampObject is string timestamp &&
    redirectTargetObject is string redirectTarget &&
    string.Equals(context.Request.GetEncodedPathAndQuery(), redirectTarget, StringComparison.Ordinal) &&
    ActivityContext.TryParse(parentId, activity.TraceStateString, isRemote: false, out var ctx) &&
    long.TryParse(timestamp, out var dateSet)
)
{
    var millisecondsDifference = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - dateSet;
    if (millisecondsDifference < (Debugger.IsAttached ? 60000 : 5000)) {
        activity.AddLink(new ActivityLink(ctx));
    }
}
await _next(context);
```

#### Key Checks

- Current activity must be sampled (`IsAllDataRequested`)
- Redirect information must be present and valid
- The request must land on the original redirect target (avoid cross-tab pollution)
- The redirect must be recent (default: 5 seconds)
- After use, clean up TempData keys

### Middleware Registration

Add the middleware early in the ASP.NET Core pipeline:

```csharp
var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseMiddleware<RedirectActivityLinkingMiddleware>();
// Other application-specific middleware
app.MapDefaultControllerRoute();
app.Run();
```

## Considerations

- The implementation is cautious to avoid inaccurate links but may still miss links in high latency scenarios.
- Timeout and safety checks can be adjusted for specific use cases.
- Extra metrics can be added to track missed link opportunities.

## Conclusion

This approach integrates with OpenTelemetry tracing and leverages .NET 9's new activity linking API, allowing more seamless tracking of the causal chain between internal redirects within ASP.NET Core applications. The result is richer, more navigable trace data for complex user flows.

---

Steve Gordon is a Pluralsight author, Microsoft MVP, and seasoned .NET engineer at Elastic. He actively shares his knowledge through blog posts, videos, OSS contributions, and community participation as founder of .NET South East.

This post appeared first on "Steve Gordon's Blog". [Read the entire article here](https://www.stevejgordon.co.uk/implementing-aspnetcore-span-linking-for-redirects-with-middleware)
