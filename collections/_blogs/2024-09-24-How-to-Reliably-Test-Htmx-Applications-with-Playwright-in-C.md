---
layout: post
title: How to Reliably Test Htmx Applications with Playwright in C#
author: Khalid Abuhakmeh
canonical_url: https://khalidabuhakmeh.com/htmx-and-playwright-tests-in-csharp
viewing_mode: external
feed_name: Khalid Abuhakmeh's Blog
feed_url: https://khalidabuhakmeh.com/feed.xml
date: 2024-09-24 00:00:00 +00:00
permalink: /coding/blogs/How-to-Reliably-Test-Htmx-Applications-with-Playwright-in-C
tags:
- ASP.NET Core
- C#
- Frontend Testing
- Htmx
- Integration Testing
- JavaScript Events
- Playwright
- Test Automation
- Test Synchronization
- Web Testing
section_names:
- coding
- devops
---
In this post, Khalid Abuhakmeh introduces a seamless technique for automating tests in Htmx-powered ASP.NET Core applications using Playwright, ensuring stable and reliable results.<!--excerpt_end-->

# How to Reliably Test Htmx Applications with Playwright in C#

**By Khalid Abuhakmeh**

![Htmx and Playwright Tests in C#](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/htmx-playwright-csharp-tests.jpg)

*Photo by [Ricardio de Penning](https://unsplash.com/@ricardio)*

Testing frontend applications that use Htmx can be challenging due to the asynchronous nature of server requests and DOM updates. Community member Jonathan Channon asked about the best way to test such Htmx-powered applications using the Playwright testing framework. Correctly timing assertions after Htmx-driven requests is essential to avoid flaky tests.

This post details a method for reliably waiting for Htmx to finish its work before verifying page state, resulting in faster and more dependable automated tests.

## The Htmx Counter Application

The example under test is a simple Counter component built using ASP.NET Core and Htmx. The counter increases its displayed value whenever a user clicks the "Increment" button.

### Razor Counter Component

```razor
@model HtmxPlaywrightIntegration.ViewModels.CounterViewModel

<div id="counter" class="card">
  <div id="value" class="card-body">
    @Model.Count
  </div>
  <div class="card-footer">
    <form asp-page="Index" method="post" hx-post hx-target="#counter" hx-swap="outerHTML">
      <button class="btn btn-primary">Increment</button>
    </form>
  </div>
</div>
```

### ASP.NET Core Endpoint

```csharp
using HtmxPlaywrightIntegration.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HtmxPlaywrightIntegration.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    public static CounterViewModel Value { get; set; } = new();

    public void OnGet() {}

    public IActionResult OnPost()
    {
        Value.Count += 1;
        return Partial("_Counter", Value);
    }
}
```

When the button is clicked, an Htmx-triggered POST request is sent to the server, which increments the count and responds with the updated snippet. Htmx then applies the change to the DOM.

## Htmx Request Lifecycle Events

Htmx offers many lifecycle events that provide insight into its progress when handling requests. The most relevant lifecycle event here is `htmx:afterSettle`, which signals that all DOM updates related to an Htmx request are complete and the page is stable.

We can add a JavaScript event listener for this event to log a message to the browser console whenever the DOM settles. This snippet can be placed in your application's JavaScript:

```javascript
document.body.addEventListener('htmx:afterSettle', function(evt) {
  console.log('playwright:continue');
});
```

This setup writes a specific message ('playwright:continue') to the console whenever Htmx finishes applying updates. Playwright can watch for this message to know exactly when it's safe to execute assertions in your tests.

## Testing with Playwright & Htmx Extensions

Let's look at how to write stable Playwright tests that interact with Htmx-powered pages.

### Sample Playwright Test

```csharp
namespace HtmxPlaywrightIntegration.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest {
    [Test]
    public async Task CanIncrementCountUsingHtmx() {
        await Page.GotoAsync("http://localhost:5170");

        await Page.RegisterHtmxLifecycleListener();

        var button = Page.Locator("text=Increment");
        var body = Page.Locator("#value");

        var currentCount = int.Parse(await body.TextContentAsync() ?? "-1");

        await button.ClickAsync();
        await Page.WaitForHtmx();

        await Expect(body).ToHaveTextAsync($"{currentCount+1}");
    }
}
```

**How It Works:**

- The test navigates to the application.
- It registers the Htmx lifecycle listener (the earlier JavaScript snippet).
- The button is clicked, triggering an Htmx request.
- The test then waits for the 'playwright:continue' console message, ensuring the DOM update is complete before asserting the new count.

### Reusable Playwright Extensions

These extension methods encapsulate the logic for listening for the Htmx lifecycle and waiting for the correct event:

```csharp
using Microsoft.Playwright;

namespace HtmxPlaywrightIntegration.Tests;

public static class HtmxExtensions {
    private const string Continue = "playwright:continue";

    public static Task WaitForHtmx(this IPage page) {
        return page.WaitForConsoleMessageAsync(new() {
            Predicate = message => message.Text == Continue
        });
    }

    public static Task RegisterHtmxLifecycleListener(this IPage page) {
        return page.AddScriptTagAsync(new() {
            Content = $"""
                document.body.addEventListener('htmx:afterSettle', function(evt) {
                  console.log('{{Continue}}');
                });
            """
        });
    }
}
```

With this setup, your Playwright tests robustly synchronize with Htmx activity, eliminating the need for arbitrary timeouts and reducing flakiness even as your front-end or back-end logic evolves.

---

## Conclusion

By leveraging Htmx's lifecycle events and Playwright's ability to listen for console messages, you can write fast, reliable end-to-end tests for dynamic applications. This approach streamlines frontend integration testing and is easy to adapt to other asynchronous workflows.

---

*About the Author*:  
Khalid Abuhakmeh is a developer advocate at JetBrains focusing on .NET technologies and tooling.

---

**Read Next:**

- [Intersperse Values for Enumerable Collections](/intersperse-values-for-enumerable-collections)
- [Add EF Core Migrations to .NET Aspire Solutions](/add-ef-core-migrations-to-dotnet-aspire-solutions)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/htmx-and-playwright-tests-in-csharp)
