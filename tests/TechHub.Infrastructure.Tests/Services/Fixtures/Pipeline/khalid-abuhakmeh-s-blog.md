Like many .NET developers, I’m starting to look at the features coming in .NET 10, C# 14, and specifically ASP.NET Core. To my surprise, ASP.NET Core Minimal APIs now support Server-Sent Events (SSE). For folks who don’t know what Server-Sent Events are, they are a unidirectional channel from the server to a client where a client can subscribe to events. SSE is handy for building live news feeds, stock ticker applications, or any system that has real-time information.

Inevitably, folks will ask, what’s the difference between SSE and SignalR? The difference is that SSE is lighter than WebSockets, and you can implement an SSE solution using the HTTP protocol. Whereas WebSockets, SignalR’s default operating mode, is a different protocol entirely. WebSockets are great, but the bidirectional communication between server and client adds additional costs that are typically unnecessary for the systems I mentioned previously.

In this post, I’ll show you how to implement a straightforward SSE example using ASP.NET Core Minimal APIs, a background service, and some basic JavaScript.

## The Anatomy of a SSE Endpoint

Starting in .NET 10, you can now use the `TypedResults` class to return a `ServerSentEventsResult`, which takes an `IAsyncEnumerable<>` instance and an event type.

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton();
builder.Services.AddHostedService();

var app = builder.Build();
app.UseDefaultFiles().UseStaticFiles();

app.MapGet("/orders", (FoodService foods, CancellationToken token) =>
 TypedResults.ServerSentEvents(
 foods.GetCurrent(token),
 eventType: "order")
);

app.Run();
```

In this example code, the `foods.GetCurrent` method call returns an `IAsyncEnumerable` of food-based emojis. The cancellation token allows the client to unsubscribe, stopping the enumeration and server-side computation.

That’s all you need; let’s see our `IAsyncEnumerable` implementation.

## Implementing an IAsyncEnumerable Food Service

While implementing an `IAsyncEnumerable` is straightforward, I wanted to write an implementation that synced all subscribers to a single source of truth. I accomplish this task in two classes: `FoodService` and `FoodServiceWorker`.

The `FoodService` implements an `INotifyPropertyChanged` and allows all subscribers to sync to get a single food item’s `Current` value.

```csharp
public class FoodService : INotifyPropertyChanged
{
 public FoodService()
 {
 Current = Foods[Random.Shared.Next(Foods.Length)];
 }

 public event PropertyChangedEventHandler? PropertyChanged;
 private static readonly string[] Foods = ["🍔", "🍟", "🥤", "🍤", "🍕", "🌮", "🥙"];

 private string Current
 {
 get;
 set
 {
 field = value;
 OnPropertyChanged();
 }
 }

 public async IAsyncEnumerable GetCurrent(
 [EnumeratorCancellation] CancellationToken ct)
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

Now, I need a background service that updates the food choices at a timed interval.

```csharp
public class FoodServiceWorker(FoodService foodService)
 : BackgroundService
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

Now, let’s write the HTML subscribing to the SSE endpoint defined in my simple sample.

## Subscribing to SSE using JavaScript

In a new `index.html` file in `wwwroot`, I’ll need to create a new EventSource object, listen for my `order` events to come through, and handle them appropriately.

```html

 
 Title

# Khalid's Fast-Food Fair

```

It’s that simple. When the browser loads the page, we’ll immediately subscribe to the SSE endpoint and start receiving food emojis. If you open two pages, you’ll see the emojis synced between the pages.

Try this quick sample, and let me know if you have any questions. Cheers.