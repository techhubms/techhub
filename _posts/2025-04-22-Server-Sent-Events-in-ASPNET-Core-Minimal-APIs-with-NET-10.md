---
layout: "post"
title: "Server-Sent Events in ASP.NET Core Minimal APIs with .NET 10"
description: "Khalid Abuhakmeh explores the new support for Server-Sent Events (SSE) in .NET 10's ASP.NET Core Minimal APIs. The post compares SSE to SignalR, demonstrates a full working example of a real-time food ordering feed, and provides key C# and JavaScript implementations."
author: "Khalid Abuhakmeh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://khalidabuhakmeh.com/server-sent-events-in-aspnet-core-and-dotnet-10"
viewing_mode: "external"
feed_name: "Khalid Abuhakmeh's Blog"
feed_url: "https://khalidabuhakmeh.com/feed.xml"
date: 2025-04-22 00:00:00 +00:00
permalink: "/2025-04-22-Server-Sent-Events-in-ASPNET-Core-Minimal-APIs-with-NET-10.html"
categories: ["Coding"]
tags: [".NET", ".NET 10", "ASP.NET", "ASP.NET Core", "BackgroundService", "C# 14", "Coding", "EventSource", "IAsyncEnumerable", "INotifyPropertyChanged", "JavaScript", "Minimal APIs", "Posts", "Real Time", "Server Sent Events", "SignalR", "SSE", "WebSockets"]
tags_normalized: ["net", "net 10", "asp dot net", "asp dot net core", "backgroundservice", "c sharp 14", "coding", "eventsource", "iasyncenumerable", "inotifypropertychanged", "javascript", "minimal apis", "posts", "real time", "server sent events", "signalr", "sse", "websockets"]
---

In this post, Khalid Abuhakmeh showcases how to implement Server-Sent Events with ASP.NET Core Minimal APIs in .NET 10, highlighting practical differences with SignalR and providing sample C# and JavaScript code for real-time feeds.<!--excerpt_end-->

# Server-Sent Events in ASP.NET Core and .NET 10

*Author: Khalid Abuhakmeh*

![Server-Sent Events in ASP.NET Core and .NET 10](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/server-sent-events-dotnet-10-aspnetcore-minimal-apis.jpg)

_Photo by [Allen Rad](https://unsplash.com/photos/cheese-burger-on-a-wooden-surface-9G_oJBKwi1c)_

## Introduction

As .NET 10 and C# 14 approach, new features are coming to ASP.NET Core—one major addition is support for Server-Sent Events (SSE) within Minimal APIs. SSE allows a server to push events to clients over HTTP, making it suitable for applications that require unidirectional, real-time updates such as news feeds or stock tickers.

## SSE vs SignalR

A common question concerns the core differences between SSE and SignalR. SSE is lighter than WebSockets, as it uses the HTTP protocol and provides one-way communication from server to client. SignalR, by default, uses WebSockets to provide bidirectional communication, which can be unnecessary overhead for scenarios where clients simply listen for server updates. SSE is best suited for cases where real-time, one-way communication suffices.

## SSE Implementation Example with ASP.NET Core Minimal APIs

This section demonstrates a straightforward example of implementing SSE using Minimal APIs in .NET 10. The approach utilizes `TypedResults.ServerSentEvents`, a new response type, which can stream items from an `IAsyncEnumerable` to any connected client.

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FoodService>();
builder.Services.AddHostedService<FoodServiceWorker>();

var app = builder.Build();
app.UseDefaultFiles().UseStaticFiles();

app.MapGet("/orders", (FoodService foods, CancellationToken token) =>
    TypedResults.ServerSentEvents(
        foods.GetCurrent(token),
        eventType: "order")
);

app.Run();
```

**How it works:**

- The `/orders` endpoint streams food emoji updates to clients using the SSE protocol.
- The data is streamed by returning an `IAsyncEnumerable<string>` from `FoodService.GetCurrent`.
- A cancellation token allows clean client disconnection/stopping of the enumeration.

## Building the IAsyncEnumerable Food Service

To keep all clients in sync with the same "current" food emoji, the implementation leverages .NET’s observable and asynchronous patterns. Two core classes are involved:

### FoodService

The `FoodService` class manages the current food and implements `INotifyPropertyChanged` to notify all listeners of updates.

```csharp
public class FoodService : INotifyPropertyChanged
{
    public FoodService()
    {
        Current = Foods[Random.Shared.Next(Foods.Length)];
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private static readonly string[] Foods =
        ["🍔", "🍟", "🥤", "🍤", "🍕", "🌮", "🥙"];

    private string Current { get; set { field = value; OnPropertyChanged(); } }

    public async IAsyncEnumerable<string> GetCurrent([
        EnumeratorCancellation] CancellationToken ct)
    {
        while (ct is not { IsCancellationRequested: true })
        {
            yield return Current;
            var tcs = new TaskCompletionSource();
            PropertyChangedEventHandler handler = (_, _) => tcs.SetResult();
            PropertyChanged += handler;
            try
            {
                await tcs.Task.WaitAsync(ct);
            }
            finally
            {
                PropertyChanged -= handler;
            }
        }
    }

    public void Set()
    {
        Current = Foods[Random.Shared.Next(Foods.Length)];
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

### FoodServiceWorker

`FoodServiceWorker` updates the food emoji at timed intervals, simulating real-time changes.

```csharp
public class FoodServiceWorker(FoodService foodService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foodService.Set();
            await Task.Delay(1000, stoppingToken);
        }
    }
}
```

## JavaScript Client: Subscribing to the SSE Endpoint

A simple HTML page can take advantage of the newly-created SSE endpoint by connecting an `EventSource` instance and responding to incoming events.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Khalid's Fast-Food Fair</title>
    <style>
        ul { display: flex; flex-direction: row; list-style: none; flex-wrap: wrap; width: 90%; gap: 1rem; padding: 0; }
        li { font-size: 2rem; }
    </style>
</head>
<body>
    <h1>Khalid's Fast-Food Fair</h1>
    <ul id="orders"></ul>
    <script>
        const eventSource = new EventSource('/orders');
        const ordersList = document.getElementById('orders');
        eventSource.addEventListener('order', event => {
            const li = document.createElement('li');
            li.textContent = event.data;
            ordersList.appendChild(li);
        });
        eventSource.onerror = error => {
            console.error('EventSource failed:', error);
            eventSource.close();
        };
    </script>
</body>
</html>
```

Whenever the browser loads the page, it automatically subscribes to `/orders`, and each time the `FoodServiceWorker` updates the food, all subscribers receive the new emoji. Opening multiple pages will show the updated emoji in sync across all clients.

## Conclusion

This demonstration shows how .NET 10’s Minimal APIs can leverage built-in support for Server-Sent Events to create simple, real-time server-to-client update channels with minimal complexity. This can be preferable to SignalR when only unidirectional communication is required. The code provided can serve as a starting point for building real-world applications that require live data feeds.

---

![Khalid Abuhakmeh's Picture](/assets/images/authorimage.jpg)

**About the Author:**
Khalid Abuhakmeh is a developer advocate at JetBrains, specializing in .NET technologies and tooling.

---

**Read Next:**
[Generic C# Methods with Enum Constraints for .NET](https://khalidabuhakmeh.com/generic-csharp-methods-with-enum-constraints-for-dotnet)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/server-sent-events-in-aspnet-core-and-dotnet-10)
