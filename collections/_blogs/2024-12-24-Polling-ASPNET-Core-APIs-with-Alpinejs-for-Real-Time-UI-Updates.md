---
layout: "post"
title: "Polling ASP.NET Core APIs with Alpine.js for Real-Time UI Updates"
description: "This post by Khalid Abuhakmeh demonstrates integrating Alpine.js, a minimalist JavaScript framework, with ASP.NET Core APIs to create real-time updating UIs. It covers the basics of Alpine.js reactivity, step-by-step API creation in ASP.NET Core, and best practices for memory management."
author: "Khalid Abuhakmeh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://khalidabuhakmeh.com/alpinejs-polling-aspnet-core-apis-for-updates"
viewing_mode: "external"
feed_name: "Khalid Abuhakmeh's Blog"
feed_url: "https://khalidabuhakmeh.com/feed.xml"
date: 2024-12-24 00:00:00 +00:00
permalink: "/blogs/2024-12-24-Polling-ASPNET-Core-APIs-with-Alpinejs-for-Real-Time-UI-Updates.html"
categories: ["Coding"]
tags: ["Alpine.js", "API Integration", "ASP.NET Core", "C#", "Coding", "Front End Frameworks", "JavaScript", "Minimal JavaScript", "Posts", "Reactivity", "UI Updates", "Web Development"]
tags_normalized: ["alpinedotjs", "api integration", "aspdotnet core", "csharp", "coding", "front end frameworks", "javascript", "minimal javascript", "posts", "reactivity", "ui updates", "web development"]
---

In this post, Khalid Abuhakmeh guides readers through building a real-time updating UI by combining Alpine.js and ASP.NET Core APIs. The article demonstrates effective API design, data polling, and integration best practices.<!--excerpt_end-->

# Polling ASP.NET Core APIs with Alpine.js for Real-Time UI Updates

*By Khalid Abuhakmeh*

![Alpine.Js Polling ASP.NET Core APIs For Updates](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/alpinejs-aspnet-core-api-updates-polling.jpg)

_Photo by [Zinko Hein](https://unsplash.com/@zinkohein)_

## Introduction

Building dynamic JavaScript experiences has evolved significantly in the past two decades, yet updating the Document Object Model (DOM) remains challenging and verbose. Frameworks for single-page applications largely address this pain point. Alpine.js offers a declarative attribute-based approach, minimizing boilerplate and enabling real-time UI updates without directly handling DOM APIs.

## The Strength of Alpine.js

[Alpine.js](https://alpinejs.dev/) is a lightweight JavaScript framework that lets developers define interactive behavior directly within HTML markup. The framework operates with a small API surface — 15 attributes, six properties, and two methods — but provides powerful reactivity and convenience.

### Example: Reactive Count

```html
<div x-data="{ count: 0 }">
  <button x-on:click="count++">Increment</button>
  <span x-text="count"></span>
</div>
```

In the above example, the `count` value is reactive. When the button is clicked, both the state and the UI update seamlessly.

You can further abstract logic into reusable contexts via the `Alpine.data` method:

```html
<div x-data="count">
  <button x-on:click="increment()">Increment</button>
  <span x-text="value"></span>
</div>

<script>
document.addEventListener('alpine:init', () => {
  Alpine.data('count', () => ({
    increment() { this.value++; },
    value: 0
  }));
});
</script>
```

Here, the increment logic is encapsulated, and the value field is naturally accessible like a standard JavaScript property.

## ASP.NET Core and Alpine.js Integration

To demonstrate polling, the post details building an ASP.NET Core API that provides weather data, followed by a dynamic front-end using Alpine.js.

### C# Weather Data Model

```csharp
public class Weather {
  public string Location { get; set; } = "";
  public string Description { get; set; } = "";
  public string Temperature { get; set; } = "";

  public static ReadOnlySpan<string> Descriptions => new(["Sunny", "Cloudy", "Rainy", "Snowy"]);
  public static ReadOnlySpan<string> Locations => new(["Mountain", "Valley", "Desert", "Forest"]);
}
```

### API Endpoint in `Program.cs`

```csharp
using MountainWeather.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/weather", () => {
  return Results.Json(
    Enumerable .Range(1, 10)
      .Select(i => new Weather {
        Location = $"{Random.Shared.GetItems(Weather.Locations, 1)[0]} #{i}",
        Description = $"{Random.Shared.GetItems(Weather.Descriptions, 1)[0]}",
        Temperature = $"{Random.Shared.Next(32, 100)}℉"
      })
      .ToList()
  );
});

app.UseDefaultFiles();
app.UseStaticFiles();
app.Run();
```

### Example `index.html` for Polling and UI

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>Weather</title>
  <link rel="stylesheet" href="https://unpkg.com/@picocss/pico@latest/css/pico.min.css">
  <script src="https://cdn.jsdelivr.net/npm/alpinejs@3.x.x/dist/cdn.min.js" defer></script>
</head>
<body>
<main class="container-fluid">
  <table class="table table-striped" x-data="weather">
    <thead>
      <tr>
        <th>Location</th>
        <th>Description</th>
        <th>Temperature</th>
      </tr>
    </thead>
    <tbody>
      <tr x-show="locations.length === 0">
        <td colspan="3"> 0 Locations Found. </td>
      </tr>
      <template x-for="l in locations">
        <tr>
          <td x-text="l.location"></td>
          <td x-text="l.description"></td>
          <td x-text="l.temperature"></td>
        </tr>
      </template>
    </tbody>
    <tfoot>
      <tr>
        <td colspan="3" x-text="updated"></td>
      </tr>
    </tfoot>
  </table>
</main>
<script>
async function getWeather() {
  let result = {};
  const response = await fetch('/weather');
  result.locations = await response.json();
  result.updated = new Date();
  return result;
}
document.addEventListener('alpine:init', () => {
  Alpine.data('weather', () => ({
    async init() {
      this.timer = setInterval(async () => {
        const result = await getWeather();
        this.locations = result.locations;
        this.updated = result.updated;
      }, 3000);
      // Initial load
      const result = await getWeather();
      this.locations = result.locations;
      this.updated = result.updated;
    },
    destroy: () => { clearInterval(this.timer); },
    locations: [],
    updated: "n/a",
    timer: null
  }));
});
</script>
</body>
</html>
```

### Implementation Notes

- The `Alpine.data` method sets up a shared context accessible via the `x-data` attribute in the markup. Key state variables (`locations`, `updated`, `timer`) and lifecycle logic support automatic API polling.
- Alpine.js allows the use of the HTML `template` tag to bind and repeat elements for dynamic markup.
- Proper interval clearing with `destroy` prevents memory leaks when components are unmounted.

## Conclusion

This approach enables a simple, maintainable, and reactive UI with minimal JavaScript overhead. Alpine.js complements ASP.NET Core well for building polling-based or real-time updating interfaces.

---

![Khalid Abuhakmeh's Picture](/assets/images/authorimage.jpg)

### About the Author

Khalid is a developer advocate at JetBrains focusing on .NET technologies and tooling.

---

### Read Next

- [Building a Persistent Counter with Alpine.Js](/building-a-persistent-counter-with-alpinejs)
- [Great .NET Documentation with Astro, Starlight, and MarkdownSnippets](/great-dotnet-documentation-with-astro-starlight-and-markdownsnippets)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/alpinejs-polling-aspnet-core-apis-for-updates)
