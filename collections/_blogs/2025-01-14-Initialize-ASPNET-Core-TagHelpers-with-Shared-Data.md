---
layout: post
title: Initialize ASP.NET Core TagHelpers with Shared Data
author: Khalid Abuhakmeh
canonical_url: https://khalidabuhakmeh.com/initialize-aspnet-core-taghelpers-with-shared-data
viewing_mode: external
feed_name: Khalid Abuhakmeh's Blog
feed_url: https://khalidabuhakmeh.com/feed.xml
date: 2025-01-14 00:00:00 +00:00
permalink: /coding/blogs/Initialize-ASPNET-Core-TagHelpers-with-Shared-Data
tags:
- ASP.NET
- ASP.NET Core
- Blogs
- C#
- Coding
- Dependency Injection
- HtmlAttributeNotBound
- ITagHelperInitializer
- Razor
- Singleton
- TagHelpers
- ViewContext
- Web Development
section_names:
- coding
---
In this blog post, Khalid Abuhakmeh explores how to leverage the ITagHelperInitializer interface in ASP.NET Core to provide shared data to TagHelpers, offering practical tips for efficient and maintainable Razor development.<!--excerpt_end-->

![Initialize ASP.NET Core TagHelpers with Shared Data](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/aspnet-core-shared-data-tag-helpers-initialize.jpg)

Photo by [David Clode](https://unsplash.com/@davidclode)

# Initialize ASP.NET Core TagHelpers with Shared Data

ASP.NET Core, powered by the Razor engine, provides a unique approach by allowing developers to blend HTML and C# seamlessly within views. While Razor syntax often focuses on HTML, its true power comes from how even the HTML portions are processed as C# and compiled into executable artifacts. This capability enables developers to craft sophisticated UI components and introduce powerful control and data management patterns.

This post demonstrates how to globally initialize TagHelpers across your application using the TagHelpers infrastructure, specifically the `ITagHelperInitializer` interface, in order to inject shared data or configuration.

## The TagHelper in Question

The article begins by showing how to define a simple TagHelper to target the `<span>` element and optionally replace its contents when a `text` attribute is present. The TagHelper code is as follows:

```csharp
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApplication2.Models;

[HtmlTargetElement("span")]
public class MyTagHelper : TagHelper
{
    [HtmlAttributeName("text")]
    public string Text { get; set; } = "";

    [HtmlAttributeNotBound]
    public string Version { get; set; } = "";

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Content.SetHtmlContent(Text);
        output.Attributes.Add("data-version", Version);
        return Task.CompletedTask;
    }
}
```

### Example Usage

In the Razor view you could use:

```html
<span class="fs-1 d-block" text="Hello, World!">...</span>
<span class="fs-1 d-block"></span>
```

Note the `Version` property in `MyTagHelper`, marked with `[HtmlAttributeNotBound]`, indicating that it is not set via HTML markup but will be initialized through the infrastructure.

## TagHelper Initializers

The core of the post focuses on `ITagHelperInitializer<TTagHelper>`, an interface that enables you to initialize all instances of a TagHelper with common data, potentially calculated once per application lifecycle. The implementation might look like:

```csharp
public class MyTagHelperInitializer(string defaultText, string version) : ITagHelperInitializer<MyTagHelper>
{
    public void Initialize(MyTagHelper helper, ViewContext context)
    {
        helper.Text = defaultText;
        helper.Version = version;
    }
}
```

**Key benefits of this approach:**

- Calculates expensive data once and applies it globally, saving resources and rendering time.
- Provides full access to `ViewContext`, allowing manipulation of request or response data.
- Direct access to the TagHelper instance for highly customizable initialization.
- `ViewContext` gives access to `HttpContext`, cookies, user information, and more.
- The initializer can resolve other registered services as needed.

## Registering the Initializer

To use the initializer, register it in the application's dependency injection container (typically in `Program.cs`):

```csharp
builder.Services.AddSingleton<ITagHelperInitializer<MyTagHelper>>(new MyTagHelperInitializer("Default Text", "1.0.0"));
```

Here, initial values are provided directly, but you could also read from configuration or other sources as necessary. Since the initializer is registered as a Singleton, the shared data persists for the application's lifetime.

## Resulting HTML

When the application runs, rendered HTML will look like:

```html
<span class="fs-1 d-block" data-version="1.0.0">Hello, World!</span>
<span class="fs-1 d-block" data-version="1.0.0">Default Text</span>
```

This demonstrates how the initializer can inject default or global data into generated HTML across your application.

## Conclusion

Using the TagHelpers and the global initialization pattern not only promotes code reusability and maintainability but can also optimize performance by centralizing logic and reducing redundant computation. This technique also benefits scenarios such as UI testing, since attributes and data can be conditionally managed based on build configurations or runtime flags.

**Try applying this pattern in your own ASP.NET Core applications to make managing cross-cutting concerns in your Razor views both easier and more robust.**

---

![Khalid Abuhakmeh's Picture](/assets/images/authorimage.jpg)

## About the Author

**Khalid Abuhakmeh** is a developer advocate at JetBrains focusing on .NET technologies and tooling.

---

## Read Next

- [Writing a String Numeric Comparer with .NET 9](/writing-a-string-numeric-comparer-with-dotnet-9)
- [Vogen and Value Objects with C# and .NET](/vogen-and-value-objects-with-csharp-and-dotnet)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/initialize-aspnet-core-taghelpers-with-shared-data)
