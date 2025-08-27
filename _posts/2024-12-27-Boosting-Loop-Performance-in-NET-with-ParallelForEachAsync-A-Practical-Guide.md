---
layout: "post"
title: "Boosting Loop Performance in .NET with Parallel.ForEachAsync: A Practical Guide"
description: "Rick Strahl demonstrates how switching from a sequential foreach loop to Parallel.ForEachAsync drastically optimizes loop performance in a .NET Markdown link checker. The article covers practical implementation details, performance benefits, thread safety considerations, and alternatives for parallelizing IO-heavy operations."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2024/Dec/27/Back-to-Basics-Using-the-Parallel-Library-to-Massively-Boost-Loop-Performance"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2024-12-27 23:37:19 +00:00
permalink: "/2024-12-27-Boosting-Loop-Performance-in-NET-with-ParallelForEachAsync-A-Practical-Guide.html"
categories: ["Coding"]
tags: [".NET", "Asynchronous Programming", "C#", "Coding", "HTTP Requests", "IO Bound Operations", "Markdown Monster", "Parallel.ForEachAsync", "Performance Optimization", "Posts", "Task.WhenAll", "Thread Safety", "TPL Dataflow", "WPF"]
tags_normalized: ["dotnet", "asynchronous programming", "csharp", "coding", "http requests", "io bound operations", "markdown monster", "paralleldotforeachasync", "performance optimization", "posts", "taskdotwhenall", "thread safety", "tpl dataflow", "wpf"]
---

Rick Strahl explores using Parallel.ForEachAsync in .NET to significantly improve the performance of link validation in Markdown Monster by parallelizing HTTP operations. This article walks through before-and-after implementations, practical code, performance data, and key caveats.<!--excerpt_end-->

# Boosting Loop Performance in .NET with Parallel.ForEachAsync: A Practical Guide

_By Rick Strahl_

---

## Introduction

Rick Strahl discusses how replacing a standard asynchronous foreach loop with `Parallel.ForEachAsync` in a .NET application led to dramatic performance improvements. Using the real-world case of link validation in the Markdown Monster editor, this article details how parallelism massively boosts throughput in IO-bound operations such as HTTP link checking.

---

## Scenario: Link Checking in Markdown Documents

Markdown Monster, a popular Markdown editor, needs to validate all links—local and online—in a document. Validating links sequentially is slow, especially as the number increases. Rick found that using parallel processing, notably through `Parallel.ForEachAsync`, drastically sped up the operation.

- **Context**: Checking 52 links cut processing time from 25+ seconds to about 2.3 seconds after parallelizing.
- **Minimal Code Change**: Switching from a standard foreach to Parallel.ForEachAsync took just a few lines, providing outsized performance gains.

---

## Parallelization Overview

### Sequential Approach (Original Code)

The application passes a list of links and processes them in sequence. Even though each HTTP check is asynchronous, the loop executes one after another, resulting in sluggish aggregate performance for large sets.

```csharp
public async Task ValidateLinks(List<LinkInfo> links, bool onlyChangedLinks = false, string docHtml = null) {
  foreach(var link in links) {
    if (CancelLinkChecking) return;
    if (onlyChangedLinks && !link.HasLinkChanged && !link.IsLinkValid != null) continue;
    var url = link.Url;
    if (url.StartsWith("http://") || url.StartsWith("https://")) {
      var settings = new HttpClientRequestSettings { Url = url, HttpVerb = "HEAD", Timeout = 2000 };
      bool isSuccessStatus = false;
      using (var message = await HttpClientUtils.DownloadResponseMessageAsync(settings)) {
        if(message != null) isSuccessStatus = message.IsSuccessStatusCode;
      }
      link.isValid = isSuccessStatus;
      if (!link isValid) link.ErrorMessage = $"{(int)settings.ResponseStatusCode} {settings.ResponseStatusCode}";
    }
    // ...other link types
  }
}
```

- **Result**: Each request is awaited before the next begins, creating a performance bottleneck.

### Parallelized Approach

Using `Parallel.ForEachAsync` enables running many link checks simultaneously:

```csharp
public async Task ValidateLinks(List<LinkInfo> links, bool onlyChangedLinks = false, string docHtml = null) {
  await Parallel.ForEachAsync<LinkInfo>(links, new ParallelOptions { MaxDegreeOfParallelism = 50 }, async (link, cancel) => {
    if (CancelLinkChecking || (onlyChangedLinks && !link.HasLinkChanged && !link.IsLinkValid != null)) return;
    var url = link.Url;
    if (url.StartsWith("http://") || url.StartsWith("https://")) {
      var settings = new HttpClientRequestSettings { Url = url, HttpVerb = "HEAD", Timeout = 2000 };
      link.IsLinkValid = null;
      bool isSuccessStatus = false;
      using (var message = await HttpClientUtils.DownloadResponseMessageAsync(settings)) {
        if(message != null) isSuccessStatus = message.IsSuccessStatusCode;
      }
      link.isValid = isSuccessStatus;
      if (!link isValid) link.ErrorMessage = $"{(int)settings.ResponseStatusCode} {settings.ResponseStatusCode}";
    }
    // ...other link types
  });
}
```

- **Benefit**: Link checks happen concurrently, cutting total processing time to that of the slowest request, rather than the sum.
- **Degree of Parallelism**: Set to 50 to avoid overwhelming the system with excessive simultaneous requests.

---

## Key Considerations and Best Practices

### Parallelization Pitfalls

- **Thread Safety**: Only modify state unique to each iteration (i.e., the current `link`), not the list itself or shared objects, to avoid multi-threading bugs.
- **UI Updates**: Avoid updating the UI or other application-wide state inside the parallel loop.

### Limiting Parallelism

- Unlimited parallelism can exhaust system resources. Always specify `MaxDegreeOfParallelism` to keep usage in check, especially for IO operations like HTTP requests.

### HTTP Request Strategy

- **HEAD then GET**: Start with a HEAD request for quick validation. If it fails, fall back to GET for a more definitive check, as some servers don't support HEAD.

### Alternative Approaches

- **Task.WhenAll**: Gathers tasks in a collection and awaits their completion. Less convenient for loop-to-parallel-loop conversions than Parallel.ForEachAsync.
- **TPL Dataflow**: Useful for more complex processing pipelines or when the job list is dynamic. Can offer fine-grained control over parallelism but adds complexity.

---

## Full Example Code

Below is a representative structure implementing the full logic, including handling HTTP, hash, email, and file links, respecting thread safety, and rebinding data to the UI after processing.

```csharp
public async Task ValidateLinks(List<LinkInfo> links, bool onlyChangedLinks = false, string docHtml = null) {
  if (string.IsNullOrEmpty(docHtml)) docHtml = mmApp.Model.ActiveDocument?.RenderHtml();

  await Parallel.ForEachAsync<LinkInfo>(links, new ParallelOptions { MaxDegreeOfParallelism = 50 }, async (link, cancel) => {
    if (CancelLinkChecking) return;
    if (onlyChangedLinks && !link.HasLinkChanged && !link.IsLinkValid != null) return;
    var url = link.Url;
    if (url.StartsWith("http://") || url.StartsWith("https://")) {
      var settings = new HttpClientRequestSettings { UserAgent = mmApp.Constants.WebBrowserUserAgent, Url = url, HttpVerb = "HEAD", Timeout = 2000 };
      link.IsLinkValid = null;
      bool isSuccessStatus = false;
      using (var message = await HttpClientUtils.DownloadResponseMessageAsync(settings)) {
        if(message != null) isSuccessStatus = message.IsSuccessStatusCode;
      }
      if (settings.HasErrors || !isSuccessStatus) {
        settings = new HttpClientRequestSettings { UserAgent = mmApp.Constants.WebBrowserUserAgent, Url = url, HttpVerb = "GET", Timeout = 2000 };
        using (var message = await HttpClientUtils.DownloadResponseMessageAsync(settings)) {
          isSuccessStatus = false;
          if (message != null) isSuccessStatus = message.IsSuccessStatusCode;
        }
        if (settings.HasErrors || !isSuccessStatus) {
          link.IsLinkValid = false;
          var status = string.Empty;
          if (!isSuccessStatus) status = $"{(int)settings.ResponseStatusCode} {settings.ResponseStatusCode}";
          if (settings.ResponseStatusCode == System.Net.HttpStatusCode.Unused) status = "Server Request failed.";
          var errorMessage = settings.ErrorMessage;
          if (!string.IsNullOrEmpty(errorMessage)) errorMessage = "\n" + errorMessage;
          link.ErrorMessage = $"{status}{errorMessage}";
        } else link.IsLinkValid = true;
      } else link.IsLinkValid = true;
    }
    else if (url.StartsWith('#')) {
      link.IsLinkValid = docHtml.Contains("id=\"" + url?.TrimStart('#') + "\"");
    }
    else if (url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) || url.Contains("ftp://", StringComparison.OrdinalIgnoreCase)) {
      // ignore
    }
    else {
      link.IsLinkValid = false;
      try {
        string path = null;
        if (!string.IsNullOrEmpty(BasePath)) {
          var uri = new Uri(url, UriKind.RelativeOrAbsolute);
          if (!uri.IsAbsoluteUri) path = System.IO.Path.Combine(BasePath, url);
          else path = uri.LocalPath;
        } else path = url;
        link.IsLinkValid = System.IO.File.Exists(path);
      } catch { /* ignore */ }
    }
  });
}
```

---

## Practical Results & Use Cases

- Checking 52 links: reduced time from >25 seconds to ~2.3 seconds.
- This approach generalizes to any IO-heavy, per-item processing in collections.
- Also applicable to background Addin Manager tasks and AI requests in the Markdown Monster app.

---

## Community Discussions & Comments

The article concludes with feedback from the developer community, discussing:

- Nuances in MaxDegreeOfParallelism defaults.
- New .NET methods like Task.WhenEach.
- Nullable boolean usage for state tracking.
- Deeper alternatives, such as TPL Dataflow.

---

## Summary & Resources

Parallel.ForEachAsync provides a simple yet powerful way to accelerate IO-bound operations in .NET, especially when loops are otherwise bottlenecked by sequential awaits. Limit parallelism, respect thread safety, and consider your application's specific needs for optimal results.

**References:**

- [Parallel Class Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.parallel?view=net-9.0)
- [Task.WhenAll() Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.whenall?view=net-9.0)
- [Westwind.Utilities.HttpClientUtils Http Helper (Github)](https://github.com/RickStrahl/Westwind.Utilities/blob/master/Westwind.Utilities/Utilities/HttpClientUtils.cs)

---

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2024/Dec/27/Back-to-Basics-Using-the-Parallel-Library-to-Massively-Boost-Loop-Performance)
