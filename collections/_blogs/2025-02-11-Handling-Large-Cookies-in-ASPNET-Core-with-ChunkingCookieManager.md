---
layout: post
title: Handling Large Cookies in ASP.NET Core with ChunkingCookieManager
author: Khalid Abuhakmeh
canonical_url: https://khalidabuhakmeh.com/aspnet-core-and-chunking-http-cookies
viewing_mode: external
feed_name: Khalid Abuhakmeh's Blog
feed_url: https://khalidabuhakmeh.com/feed.xml
date: 2025-02-11 00:00:00 +00:00
permalink: /coding/blogs/Handling-Large-Cookies-in-ASPNET-Core-with-ChunkingCookieManager
tags:
- .NET
- ASP.NET
- ASP.NET Core
- Blogs
- ChunkingCookieManager
- Coding
- CookieOptions
- Cookies
- Cookies Size Limit
- HTTP
- HttpContext
- ICookieManager
- Minimal APIs
- Session Management
- Web Development
section_names:
- coding
---
Khalid Abuhakmeh discusses practical cookie management in ASP.NET Core, focusing on overcoming size limitations using the ChunkingCookieManager. This guide is ideal for developers confronted with storing substantial session data securely and efficiently.<!--excerpt_end-->

# ASP.NET Core and Chunking HTTP Cookies

*Photo by [LittleJakub](https://www.deviantart.com/littlejakub/art/Cookie-Monster-Wallpaper-194354451)*

If you’ve spent time around web development or your grocery store’s baked goods aisle, you’ve probably dealt with **cookies**—this post discusses web cookies specifically.

A web cookie is a header key-value pair set on the server using the [`Set-Cookie`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie) header in the format `<name>=<value>`. Cookies persist on the client, and the client sends them to the server with every subsequent request. This mechanism enables state persistence in a fundamentally stateless web environment and helps avoid complex session management infrastructure.

However, cookies do have limitations, notably the [**4kb size limit**](https://developer.mozilla.org/en-US/docs/Web/HTTP/Cookies#data_storage) for each unique cookie. This constraint can seriously impact ASP.NET Core applications, which often store encrypted and encoded user session data in cookies.

In this post, Khalid Abuhakmeh presents a sample solution using the same `ICookieManager` abstraction employed by ASP.NET Core to chunk large cookies, thereby overcoming these limitations.

## Why You Would and Wouldn’t Use Cookies

Cookies offer a straightforward method for maintaining state between HTTP requests as users interact with your application.

**Advantages:**

- Simplifies backend implementations, as relevant user state can be sent with each request.
- Useful for storing user information like IDs, names, and emails.
- The request/response cookie lifecycle is easy to debug since they appear in most HTTP logging mechanisms.

**Disadvantages:**

- Cookies are added to every request, including those that don’t need them (e.g., static file requests), causing unnecessary data transfer and resource use.
- Encrypted or chunked cookies require reassembling and decryption, adding to memory and CPU usage, depending on your request pipeline.
- A large number of big cookies can significantly increase request size (e.g., 10 cookies at maximum size lead to an extra 40KB per request).

**Conclusion:** Use cookies judiciously, being mindful of these trade-offs.

## Setting Cookies in ASP.NET Core

You can view the full sample at [Khalid's GitHub repository](https://github.com/khalidabuhakmeh/CookieMonster).

**Accessing cookies is straightforward:**

```csharp
ctx.Response.Cookies.Append("cookie_monster", "nom nom nom");
```

A sample demonstrating reading and updating a cookie value:

```csharp
app.MapGet("/nom-nom", async ctx => {
    ctx.Response.Cookies.Append(
        "cookie_monster",
        ctx.Request.Cookies.TryGetValue("cookie_monster", out var cookie)
            ? $"{cookie}, {RandomString()}"
            : $"{RandomString()}"
    );
    // more code
});
```

This works, but the cookie can keep growing until it hits the 4KB limit, which is problematic in real applications.

## Handling Large Cookies: Chunking with ChunkingCookieManager

To use cookie chunking, install the `Microsoft.AspNetCore.Authentication.Cookies` NuGet package. This is often already included transitively but can be referenced explicitly for clarity.

**Register the ChunkingCookieManager in your services:**

```csharp
builder.Services.AddScoped<ICookieManager>(svc => new ChunkingCookieManager {
    // characters, not bytes
    ChunkSize = 1000,
    ThrowForPartialCookies = true
});
```

- `ChunkSize` determines the size (in characters, not bytes) of each chunk. Defaults to 4050, but can be set lower to see the chunking effect sooner.
- `ThrowForPartialCookies` helps debug partial cookie issues.

**Sample implementation with minimal API endpoint:**

```csharp
app.MapGet("/chunks", async (HttpContext ctx, ICookieManager cookieManager) => {
    var value = cookieManager.GetRequestCookie(ctx, "chunky_monster") is { } cookie
        ? $"{cookie}, {RandomString()}"
        : $"{RandomString()}";

    cookieManager.AppendResponseCookie(ctx, "chunky_monster", value, new CookieOptions());
    // more code...
});
```

Key points for chunking:

- Only interact with cookies via the `ICookieManager` for logical grouping—to handle chunking automatically.
- The `ICookieManager` requires an `HttpContext` for its operations.
- `CookieOptions` can be used to set options like `Domain`, `Expires`, and `Secure` for each chunk.

**Browser DevTools display:**
| Name                | Value                             |
|---------------------|-----------------------------------|
| chunky_monster      | chunks-2                          |
| chunky_monsterC1    | *value chunk up to `ChunkSize`*   |
| chunky_monsterC2    | *rest of value*                   |
| chunky_monsterCN    | *further chunks if necessary*     |

You have now successfully enabled cookie chunking in ASP.NET Core using built-in abstractions.

## Conclusion

Cookies remain foundational in web development, but developers must recognize their strengths and weaknesses. Exceeding cookie size limits should prompt a review of what’s stored and possible optimizations. Chunking offers a viable workaround but introduces complexity and potential performance costs. For custom scenarios, consider implementing your own `ICookieManager` for tailored behavior.

To try this approach directly, visit the [CookieMonster GitHub repository](https://github.com/khalidabuhakmeh/CookieMonster).

---

*About the author:*  
Khalid Abuhakmeh is a developer advocate at JetBrains focusing on .NET technologies and tooling.

---

**Read Next:**

- [Vogen and Value Objects with C# and .NET](/vogen-and-value-objects-with-csharp-and-dotnet)
- [The Curious Case of .NET ConcurrentDictionary and Closures](/the-curious-case-of-dotnet-concurrentdictionary-and-closures)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/aspnet-core-and-chunking-http-cookies)
