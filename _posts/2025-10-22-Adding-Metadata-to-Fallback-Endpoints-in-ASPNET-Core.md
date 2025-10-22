---
layout: "post"
title: "Adding Metadata to Fallback Endpoints in ASP.NET Core"
description: "This post by Andrew Lock explores the use of metadata with fallback endpoints in ASP.NET Core. It explains the routing infrastructure, why fallback endpoints exist, and highlights key differences in how metadata like authorization is handled for minimal APIs versus MVC and Razor Pages. Readers will learn common mistakes and best practices for ensuring proper authorization behavior."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/adding-metadata-to-fallback-endpoints-in-aspnetcore/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-10-22 10:00:00 +00:00
permalink: "/2025-10-22-Adding-Metadata-to-Fallback-Endpoints-in-ASPNET-Core.html"
categories: ["Coding"]
tags: [".NET 6", ".NET Core", "AllowAnonymous", "ASP.NET Core", "Authorization", "Coding", "Endpoint Metadata", "Fallback Endpoints", "MapFallback", "MapFallbackToController", "MapFallbackToPage", "Middleware", "Minimal APIs", "MVC", "Posts", "Razor Pages", "RequireAuthorization", "Routing"]
tags_normalized: ["dotnet 6", "dotnet core", "allowanonymous", "aspdotnet core", "authorization", "coding", "endpoint metadata", "fallback endpoints", "mapfallback", "mapfallbacktocontroller", "mapfallbacktopage", "middleware", "minimal apis", "mvc", "posts", "razor pages", "requireauthorization", "routing"]
---

Andrew Lock delves into the quirks of applying metadata to fallback endpoints in ASP.NET Core, providing guidance on best practices for minimal APIs, MVC, and Razor Pages.<!--excerpt_end-->

# Adding Metadata to Fallback Endpoints in ASP.NET Core

Andrew Lock discusses how routing and metadata interact in ASP.NET Core, especially when configuring fallback endpoints. He emphasizes how middleware uses metadata to apply features like authorization and CORS on a per-endpoint basis.

## Routing Infrastructure in ASP.NET Core

- The routing system in ASP.NET Core uses two main middleware components: `EndpointRoutingMiddleware` (responsible for endpoint selection) and `EndpointMiddleware` (executes the selected endpoint).
- Metadata added to endpoints enables middleware before execution (e.g., for authorization or CORS) to alter behavior based on endpoint-specific settings.
- There are two main methods to add metadata:
  - Attribute-based (e.g., `[AllowAnonymous]`, `[Authorize]` for MVC or Razor Pages)
  - Fluent/extension method-based (e.g., `.AllowAnonymous()`, `.RequireAuthorization()` for minimal APIs).

## Fallback Endpoints

- A fallback endpoint is one that matches when no other endpoint does. Typical usage includes SPA applications where client-side routing needs to be supported.
- Minimal APIs can add fallback endpoints using `MapFallback()`, which registers a new endpoint that can have metadata attached via methods like `.AllowAnonymous()`.
- Other overloads include `MapFallbackToFile`, `MapFallbackToPage`, and `MapFallbackToController`.

## Fallback Routing and Metadata Nuances

- For minimal APIs and `MapFallbackToFile()`, attaching metadata (e.g., `.AllowAnonymous()`) directly works as expected. Authorization is evaluated using metadata on the fallback endpoint.
- For `MapFallbackToPage()` and `MapFallbackToController()`:
  - These methods add an extra "dynamic" endpoint that points to the actual destination endpoint (Razor Page or MVC Controller).
  - Metadata attached to the fallback endpoint is **not** transferred to the ultimate destination. Authorization evaluates metadata on the destination, so attributes or policy methods need to be applied directly on the Razor Page/Controller.

**Takeaway:** To ensure fallback pages in MVC or Razor Pages allow (or restrict) access, place your authorization-related metadata (such as `[AllowAnonymous]`) on the destination endpoint, not just on the fallback mapping.

## Code Examples

### Minimal API Fallback

```csharp
app.MapFallback(() => "Fallback").AllowAnonymous();
```

### Razor Pages Fallback

```csharp
// Map fallback to page, but .AllowAnonymous() here is ineffective
app.MapFallbackToPage("/Fallback").AllowAnonymous();

// Add the attribute directly to the Razor Page
// /Fallback.cshtml:
@page
@attribute [Microsoft.AspNetCore.Authorization.AllowAnonymous]
<h1>Fallback</h1>
```

## Summary

- Fallback metadata handling differs based on the type of endpoint.
- For minimal APIs, add metadata directly on the mapping.
- For MVC and Razor Pages, add metadata (like `[AllowAnonymous]`) to the actual destination action or page, not on the fallback mapping itself.
- This distinction avoids common pitfalls with authorization logic in real-world ASP.NET Core applications.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/adding-metadata-to-fallback-endpoints-in-aspnetcore/)
