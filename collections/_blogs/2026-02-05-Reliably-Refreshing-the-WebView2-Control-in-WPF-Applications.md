---
layout: "post"
title: "Reliably Refreshing the WebView2 Control in WPF Applications"
description: "This article by Rick Strahl explores how to perform reliable refresh operations on the Microsoft WebView2 control in WPF desktop applications. It covers the challenges of reloading web content, especially images and cached resources, and presents practical coding solutions for performing soft and hard reloads using different techniques and APIs within the .NET/Microsoft ecosystem."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2026/Feb/04/Reliably-Refreshing-the-WebView2-Control"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2026-02-05 07:53:22 +00:00
permalink: "/2026-02-05-Reliably-Refreshing-the-WebView2-Control-in-WPF-Applications.html"
categories: ["Coding"]
tags: [".NET", "Blogs", "C#", "Cache Management", "Chromium", "ClearBrowsingDataAsync", "Coding", "CoreWebView2", "Desktop Development", "DevTools Protocol", "Microsoft Edge", "Reload", "UI Refresh", "WebView", "WebView2", "Windows", "Windows Applications", "WinUI", "WPF"]
tags_normalized: ["dotnet", "blogs", "csharp", "cache management", "chromium", "clearbrowsingdataasync", "coding", "corewebview2", "desktop development", "devtools protocol", "microsoft edge", "reload", "ui refresh", "webview", "webview2", "windows", "windows applications", "winui", "wpf"]
---

Rick Strahl explains several approaches to reliably refresh the Microsoft WebView2 control for WPF, focusing on solutions to reload web content and resources, and providing sample code for soft and hard cache clearing.<!--excerpt_end-->

# Reliably Refreshing the WebView2 Control in WPF Applications

When building WPF desktop applications with embedded web content via the Microsoft WebView2 control, programmatically refreshing the browser view is not always as simple as calling `Reload()`. In this guide, Rick Strahl explores the nuances of the WebView2 refresh process, especially for scenarios where dependent resources (like images or scripts) may still be served from cache, even after a page reload.

## Why Hard Refreshes Matter

Most WPF-based applications that preview HTML, such as Markdown editors, need to ensure that their content—including images—is up-to-date. However, updates to in-memory content do not always trigger a hard reload from disk, especially for dependent resources. For instance, changing an image file and refreshing the view might not display the new image unless appropriate steps are taken.

## Reload Options in WebView2

### 1. Soft Reload with `CoreWebView2.Reload()`

The simple approach is calling the built-in reload method:

```csharp
var url = WebView.Source?.ToString();
WebView.CoreWebView2.Reload();
```

However, even with this, cached resources like images or scripts might not update.

### 2. Forcing a Full Page Reload by Replacing the URL

Refresh by setting the source to `about:blank`, then back to the target URL to force a reload:

```csharp
public void Refresh(bool noCache)
{
    if (WebBrowser == null || string.IsNullOrEmpty(WebBrowser.Source?.ToString())) return;

    if (noCache)
    {
        WebBrowser.Dispatcher.InvokeAsync(async () => {
            var source = WebBrowser.Source?.ToString();
            WebBrowser.Source = new Uri("about:blank");
            Dispatcher.Invoke(() => WebBrowser.Source = new Uri(source));
        }).Task.FireAndForget();
        return;
    }
    WebBrowser.CoreWebView2.Reload();
}
```

This approach offers improved reliability, but not total cache invalidation.

### 3. Clearing the Browser Cache via WebView2 Profile

For reliable cache busting, you can use `ClearBrowsingDataAsync`:

```csharp
public void Refresh(bool noCache)
{
    if (WebBrowser == null || string.IsNullOrEmpty(WebBrowser.Source?.ToString())) return;
    if (noCache)
    {
        WebBrowser.Dispatcher.InvokeAsync(async () => {
            await WebBrowser.CoreWebView2.Profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.DiskCache);
            WebBrowser.CoreWebView2.Reload();
        }).Task.FireAndForget();
        return;
    }
    WebBrowser.CoreWebView2.Reload();
}
```

This clears the disk cache before reloading. Ensure the async operation completes prior to reloading.

### 4. Using DevTools Protocol for a True Hard Refresh

For a refresh equivalent to `Ctrl+Shift+R` (hard reload in browsers), make a DevTools call:

```csharp
await WebBrowser.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
WebBrowser.CoreWebView2.Reload();
```

This requires your UI thread to be STA—standard for desktop apps—but is the closest to a full hard refresh programmatically.

## Also Useful for Navigation

Similar cache issues can arise when navigating to new pages. The pattern of clearing the cache and then changing the URL helps ensure updated resources are loaded:

```csharp
public void Navigate(string url, bool noCache)
{
    if (noCache)
    {
        WebBrowser.Source = new Uri("about:blank");
        WebBrowser.Dispatcher.InvokeAsync(async () => {
            await WebBrowser.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
            WebBrowser.Source = new Uri(url);
        }).Task.FireAndForget();
    }
    else
    {
        WebBrowser.Source = new Uri(url);
    }
}
```

These methods are included in the [Westwind.WebView library](https://github.com/RickStrahl/Westwind.WebView), which provides reusable utilities for WPF developers.

## Summary

Refreshing embedded browser content in WPF with WebView2 is more nuanced than a simple reload. Rick Strahl demonstrates:

- The limitations of `CoreWebView2.Reload()`
- Workarounds for hard refreshes, including URL replacing, cache clearing, and DevTools protocol
- Practical WPF code samples for all approaches
- Associated trade-offs and practical advice

Choose the right level of refresh for your scenario, balancing reliability with the impact of full cache resets.

---

*This article was created and published using [Markdown Monster](https://markdownmonster.west-wind.com) by Rick Strahl.*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2026/Feb/04/Reliably-Refreshing-the-WebView2-Control)
