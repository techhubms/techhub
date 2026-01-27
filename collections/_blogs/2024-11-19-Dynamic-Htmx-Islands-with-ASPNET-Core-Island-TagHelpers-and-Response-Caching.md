---
external_url: https://khalidabuhakmeh.com/dynamic-htmx-islands-with-aspnet-core
title: 'Dynamic Htmx Islands with ASP.NET Core: Island TagHelpers and Response Caching'
author: Khalid Abuhakmeh
feed_name: Khalid Abuhakmeh's Blog
date: 2024-11-19 00:00:00 +00:00
tags:
- ASP.NET
- ASP.NET Core
- Dynamic Content
- Htmx
- Island Architecture
- Output Caching
- Razor Pages
- Response Caching
- Server Islands
- Static Site Rendering
- TagHelper
- Web Development
section_names:
- coding
primary_section: coding
---
In this post, Khalid Abuhakmeh demonstrates how to build dynamic 'islands' in ASP.NET Core applications with Htmx, using TagHelpers and caching to combine static speed with dynamic server-rendered content.<!--excerpt_end-->

# Dynamic Htmx Islands with ASP.NET Core: Island TagHelpers and Response Caching

*Photo by [Denys Nevozhai](https://unsplash.com/@dnevozhai)*

## Introduction

Khalid Abuhakmeh shares his insights on the evolving landscape between static and dynamic content rendering in ASP.NET Core web applications. He explores techniques for integrating "island" architecture—patterned after client/server island techniques from frameworks like Astro—into ASP.NET Core apps by using Htmx for event-driven dynamic content loading, TagHelpers for component design, and output/response caching for improved performance.

---

### What Is an Island?

An "island" is a portion of a web page’s Document Object Model (DOM) that is loaded or refreshed after the initial static page load. This pattern enables the page to be rendered quickly (often benefiting from CDN or cache), while still allowing personalized or dynamic content to be loaded in small, targeted areas through asynchronous requests. Examples include showing a logged-in user’s profile picture, dashboard statistics, or personalized suggestions.

### Implementing an Island in ASP.NET Core

Islands follow a structure with three elements:

- **Initial content** (placeholder)
- **Triggering event** (what causes dynamic content to load)
- **Endpoint** (returns the dynamic content)

Khalid demonstrates a custom TagHelper for use in Razor pages, for example:

```razor
<island url="/profile/avatar">
  <div class="alert alert-info d-flex justify-content-center vertical-align-center">
    <div class="spinner-border" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
</island>
```

The placeholder is replaced by dynamic content fetched from `/profile/avatar`.

#### Example Endpoint

```csharp
app.MapGet("/profile/avatar", () => Results.Content($"""
  <div class="alert alert-info">
    <p class="fs-1 fw-bold">🌴 Welcome to the island Khalid!</p>
    <p class="fs-3">You arrived on ({DateTime.Now.ToLongTimeString()})</p>
  </div>
"""));
```

#### Island TagHelper Implementation

Khalid prefers TagHelpers over ViewComponents for this scenario due to increased flexibility. The TagHelper gives developers the ability to specify the loading behavior using an enum:

```csharp
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

public enum IslandEvents { Load, Revealed, Intersect }

[HtmlTargetElement("island")]
public class IslandTagHelper : TagHelper {
    [HtmlAttributeName("url"), Required]
    public string? Url { get; set; }

    [HtmlAttributeName("event")]
    public IslandEvents Event { get; set; } = IslandEvents.Load;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "div"; // Change the tag name to div

        var @event = Event switch {
            IslandEvents.Load => "load",
            IslandEvents.Revealed => "revealed",
            IslandEvents.Intersect => "intersect once",
            _ => "load"
        };
        output.Attributes.SetAttribute("hx-get", Url);
        output.Attributes.SetAttribute("hx-trigger", @event);
        output.Attributes.SetAttribute("hx-swap", "outerHTML");
        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
        output.TagMode = TagMode.StartTagAndEndTag;
    }
}
```

- **Load**: Content loads after initial page load
- **Revealed**: Content loads when the island scrolls into view
- **Intersect**: Content loads when element intersects visible viewport (e.g., in overflow)

**Register the TagHelper** in `_ViewImports.cshtml`, and add the [Htmx](https://htmx.org) script to your `_Layout.cshtml`:

```html
<script src="https://unpkg.com/htmx.org@2.0.3"></script>
```

#### Configurable Event Example

To change when the dynamic content loads, update the tag’s `event` attribute:

```razor
<div style="margin-top: 2000px">
  <island url="/profile/avatar" event="Revealed">
    <div class="alert alert-info d-flex justify-content-center vertical-align-center">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
  </island>
</div>
```

Now, dynamic content loads only when the user scrolls down.^

---

## Response and Output Caching in ASP.NET Core

Though not the focus, output and response caching are essential for efficiently sharing static content while only updating dynamic islands as needed.

**Service Registration:**

```csharp
builder.Services.AddOutputCache();
builder.Services.AddResponseCaching();
```

**Middleware in ASP.NET Core Pipeline:**

```csharp
app.UseResponseCaching();
app.UseOutputCache();
```

**Annotate Pages or Endpoints:**

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;

[OutputCache(Duration = 100), ResponseCache(
  Duration = 100, Location = ResponseCacheLocation.Any, NoStore = false)]
public class IndexModel(ILogger<IndexModel> logger) : PageModel {
    private readonly ILogger<IndexModel> logger = logger;
    public void OnGet() { }
}
```

**Cache Dynamic Endpoint Example:**

```csharp
app.MapGet("/profile/avatar", () => Results.Content($"""
  <div class="alert alert-info">
    <p class="fs-1 fw-bold">🌴 Welcome to the island Khalid!</p>
    <p class="fs-3">You arrived on ({DateTime.Now.ToLongTimeString()})</p>
  </div>
"""))
  .CacheOutput(policy => { /* custom caching policy */ });
```

Refer to the [official Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-8.0) for a thorough understanding of caching in ASP.NET Core.

---

## Conclusion and Further Considerations

Khalid discusses why this technique is preferable to older techniques like "donut caching":

- Dynamic content can come from any backend or be served from a CDN; it doesn’t have to be from your ASP.NET Core server directly.
- The technique encourages separation of static rendering (for performance) from dynamic, user-specific content.
- Enhances flexibility for web developers targeting high-performance with modern SPAs or hybrid applications.

He suggests there’s room for deeper integration with ASP.NET Core routing—potentially more advanced mapping in the future—and calls attention to the power of TagHelpers in modern .NET web development.

**Try It Out:**

- [GitHub repository with live demo](https://github.com/khalidabuhakmeh/aspnetcore-htmx-islands)

---

*About the Author:*

Khalid Abuhakmeh is a Developer Advocate at JetBrains focused on .NET technologies and tooling.

---

## Related Reading

- [Update HTML Elements with Htmx Triggers and ASP.NET Core](/update-html-elements-with-htmx-triggers-and-aspnet-core)
- [Building a Persistent Counter with Alpine.Js](/building-a-persistent-counter-with-alpinejs)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/dynamic-htmx-islands-with-aspnet-core)
