---
layout: "post"
title: "WebView2: Simplifying Document Load Handling and Async Workflows in .NET Applications"
description: "Rick Strahl explores effective methods for managing document load events in the WebView2 control for .NET applications. The article introduces the Westwind.WebView library and its WebViewHandler, offering an async helper method to streamline content loading, simplify interactivity, and avoid common pitfalls with event-driven state management."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/May/06/WebView2-Waiting-for-Document-Loaded"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2025-05-06 21:50:16 +00:00
permalink: "/2025-05-06-WebView2-Simplifying-Document-Load-Handling-and-Async-Workflows-in-NET-Applications.html"
categories: ["Coding"]
tags: [".NET", "Async Programming", "Blogs", "C#", "Coding", "Document Loaded", "Event Handling", "JavaScript Interop", "Open Source", "Package Management", "Software Architecture", "UI Automation", "WebView", "WebView2", "Westwind.WebView", "Windows", "Windows Development", "WPF"]
tags_normalized: ["dotnet", "async programming", "blogs", "csharp", "coding", "document loaded", "event handling", "javascript interop", "open source", "package management", "software architecture", "ui automation", "webview", "webview2", "westwinddotwebview", "windows", "windows development", "wpf"]
---

In this detailed post, Rick Strahl discusses common challenges and streamlined solutions for handling document load events in WebView2 controls within .NET and WPF applications. He introduces the Westwind.WebView library, focusing on its WaitForDocumentLoaded() helper for more linear, maintainable workflows.<!--excerpt_end-->

# WebView2: Waiting for Document Loaded

**Author:** Rick Strahl

---

When building hybrid web applications where .NET code interacts with embedded HTML content using WebView2, one of the most persistent challenges is handling the asynchronous nature of document loading and subsequent interaction via script code. This article demonstrates practical methods for managing this complexity, including the use of event handlers and a streamlined async helper available in the [Westwind.WebView](https://github.com/RickStrahl/Westwind.WebView) library.

---

## Understanding the Challenge: Asynchronous Loading in WebView2

Modern UI frameworks like WPF offer the WebView2 control for rendering web content. Loading documents in WebView2 is inherently asynchronous, leading to possible timing issues when code tries to access the DOM or execute scripts *before* the document is fully loaded.

A basic example illustrates the problem:

```csharp
WebView.Source = "http://localhost:5200/MyPage";
// BOOM! await WebView.ExecuteAsync("alert('Document has loaded.')");
```

This sequence will fail—`ExecuteAsync` targets a document that might not be available yet.

### The Usual Fix: Event Handlers

The standard approach involves handling the `CoreWebView2.DOMContentLoaded` event:

```csharp
string envPath = Path.Combine(Path.GetTempPath(), "WpfSample_WebView");
var environment = await CoreWebView2Environment.CreateAsync(envPath);
await webBrowser.EnsureCoreWebView2Async(environment);

WebView.Source = "http://localhost:5200/MyPage";
WebBrowser.CoreWebView2.DOMContentLoaded += async (s, args) => {
    await WebView.ExecuteAsync("alert('Document has loaded.')");
};
```

> **Tip:** Always specify a WebView Environment Folder, preferably under a temp location. By default, some locations may be non-writable on user systems, resulting in subtle failures.

However, this pattern is:

- Verbose
- Requires explicit event handler registration/deregistration
- Difficult to manage for multiple WebViews or documents
- Tends to fragment application logic along awkward event boundaries

## A Cleaner Alternative: WebView2Handler.WaitForDocumentLoaded()

The [Westwind.WebView](https://github.com/RickStrahl/Westwind.WebView) library offers enhancements for WebView2, most notably the `WaitForDocumentLoaded()` async helper.

### Key Features

- Automatic (optional) WebView environment management
- Shared environments among multiple controls
- JavaScript interop helpers
- Easy navigation modes
- Simple linear async helper: `WaitForDocumentLoaded()`
- Detection for compatible runtime installs

#### Installing the Package

```bash
dotnet add package Westwind.WebView
```

#### Using WebViewHandler and WaitForDocumentLoaded

1. **Declare a property for the handler in your control/window:**

    ```csharp
    public WebViewHandler WebViewHandler { get; set; }
    WebViewHandler = new WebViewHandler(WebView); // inside your constructor
    ```

2. **Handling navigation and waiting for the document:**

    ```csharp
    private async void EmojiWindow_Loaded(object sender, RoutedEventArgs e) {
        WebViewHandler.Navigate("http://localhost:5200/MyPage");
        if(!await WebViewHandler.WaitForDocumentLoaded(5000))
            throw new ApplicationException("Webpage failed to load in time...");
        await WebBrowser.ExecuteScriptAsync("alert('You have arrived')");
    }
    ```

Alternatively, you may use the built-in JS interop:

```csharp
await WebViewHandler.JsInterop.Invoke("alert","You have arrived");
```

This approach ensures correct parameter encoding and easier maintenance for repeated JS interaction.

---

## How WaitForDocumentLoaded() Works

The helper leverages a `TaskCompletionSource` to bridge between the `CoreWebView2.DOMContentLoaded` event and async/await patterns. Here’s the core logic (abridged):

```csharp
public async Task<bool> WaitForDocumentLoaded(int msTimeout = 5000){
    if (IsLoaded) return true;
    if (IsLoadedTaskCompletionSource == null)
        IsLoadedTaskCompletionSource = new TaskCompletionSource();
    var task = IsLoadedTaskCompletionSource.Task;
    if (task == null) return false;
    if (task.IsCompleted) return true;
    var timeoutTask = Task.Delay(msTimeout);
    var completedTask = await Task.WhenAny(task, timeoutTask);
    return completedTask == task;
}
```

- The event handler for `CoreWebView2.DOMContentLoaded` marks the load as complete:

    ```csharp
    protected virtual async void OnDomContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e) {
        IsLoaded = true;
        if (IsLoadedTaskCompletionSource?.Task is { IsCompleted: false })
            IsLoadedTaskCompletionSource?.SetResult();
    }
    ```

- New navigations reset the TCS:

    ```csharp
    protected virtual void OnNavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e) {
        IsLoaded = false;
        IsLoadedTaskCompletionSource = new TaskCompletionSource();
    }
    ```

This pattern supports repeated navigations and tight coordination between UI actions and web content readiness.

---

## Real-World Scenarios

- **User Interactivity:**
    - In applications like Markdown Monster, dialogs using WebView might encounter user input before the window is fully initialized. Wrapping event handlers with `await WebViewHandler.WaitForDocumentLoaded(1000)` avoids referencing uninitialized objects, preventing crashes and null reference faults.

- **Multiple Coordinated Loads:**
    - When loading editors and previews simultaneously—such as in markdown authoring tools—it's critical to ensure each WebView is loaded in sequence before querying or updating their content, improving correctness and reliability.

Example:

```csharp
// Wait for the editor WebView to be loaded before accessing its content
if( !await editor.EditorHandler.WaitForLoaded(1000) ) {
    await Window.CloseTab(tab);
    WindowsNotifications.ShowInAppNotifications(...);
    return;
}
topic.Body = await editor.GetMarkdown();
```

---

## Summary

While setting up custom event handlers for every content load is possible, it fragments application logic and invites errors. The async helper `WaitForLoaded()` from the Westwind.WebView library makes it easier to coordinate UI actions, prevent null-reference races, and maintain consistency. It reduces guesswork, replacing brittle workarounds with clear, reusable patterns—and is especially effective in complex UI applications relying on dynamic web content.

---

## Resources

- [Westwind.WebView on GitHub (WPF)](https://github.com/rickstrahl/Westwind.WebView)
- [WebViewHandler.cs source](https://github.com/RickStrahl/Westwind.WebView/blob/master/Westwind.WebView/Wpf/WebViewHandler.cs)

**Related posts:**

- [Using SQL Server on Windows ARM](https://weblog.west-wind.com/posts/2024/Oct/24/Using-Sql-Server-on-Windows-ARM)
- [Fighting through Setting up Microsoft Trusted Signing](https://weblog.west-wind.com/posts/2025/Jul/20/Fighting-through-Setting-up-Microsoft-Trusted-Signing)
- [Programmatic Html to PDF Generation using the WebView2 Control with .NET](https://weblog.west-wind.com/posts/2024/Mar/26/Html-to-PDF-Generation-using-the-WebView2-Control)

---

*If you found this article useful, consider supporting the author with a small donation.*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/May/06/WebView2-Waiting-for-Document-Loaded)
