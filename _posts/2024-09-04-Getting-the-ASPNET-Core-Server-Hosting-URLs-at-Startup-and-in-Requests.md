---
layout: "post"
title: "Getting the ASP.NET Core Server Hosting URLs at Startup and in Requests"
description: "Rick Strahl discusses reliable strategies for programmatically retrieving the hosting URLs in ASP.NET Core applications, both during startup and in requests. He examines various approaches, explains their pros and cons, provides recommended code patterns, and offers additional context about logging-based methods."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2024/Sep/03/Getting-the-ASPNET-Core-Server-Hosting-Urls-at-Startup-and-in-Requests"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2024-09-04 09:31:35 +00:00
permalink: "/2024-09-04-Getting-the-ASPNET-Core-Server-Hosting-URLs-at-Startup-and-in-Requests.html"
categories: ["Coding"]
tags: [".NET", "Application Development", "Asp.net", "Asp.net Core", "Coding", "Dependency Injection", "Host Configuration", "Iservaddressesfeature", "Iservice", "Logging", "Posts", "Program.cs", "Startup", "Urls"]
tags_normalized: ["net", "application development", "asp dot net", "asp dot net core", "coding", "dependency injection", "host configuration", "iservaddressesfeature", "iservice", "logging", "posts", "program dot cs", "startup", "urls"]
---

In this post, Rick Strahl explores different techniques to retrieve ASP.NET Core application hosting URLs at startup and during requests, providing code samples and practical advice.<!--excerpt_end-->

# Getting the ASP.NET Core Server Hosting URLs at Startup and in Requests

**Author:** Rick Strahl
**Published:** September 3, 2024 — Hood River, Oregon

---

Retrieving the hosting URLs for an ASP.NET Core application is a deceptively tricky task. There are numerous ways to set the addresses, but not a single dedicated location to query them reliably. Depending on whether you need the URLs on startup or inside a request, you'll need to follow different approaches.

## Introduction

Rick recounts a client scenario where an ASP.NET Core application running in IIS wouldn’t start on a known port. The client had altered the default port programmatically, and the hosting URL was not easily discoverable. This challenge underpins the necessity for a robust approach to extract the actual startup URLs.

### Common Setup Methods

- **Default Port:** Sometimes defaults to 5000
- **Command Line/Env Variables:** `--urls "<urlList>"` or `--https_ports` can override, unless explicit overrides exist in code
- **Programmatic Assignment:** URLs set in code can supersede other settings

[See Andrew Lock's post for 8 ways to set hosting URLs](https://andrewlock.net/8-ways-to-set-the-urls-for-an-aspnetcore-app/)

## The Challenge: Getting URLs Reliably at Runtime

While setting the hosting address is well documented, **retrieving the actual addresses in use is less straightforward**. An ideal scenario is displaying these addresses in a startup banner, useful for both debugging and development purposes.

Typical (but unreliable) approach:

```csharp
// DON'T USE THIS - works only if URLs assigned via config or command line
var urls = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey)?.Replace(";", " ");
```

This method doesn't account for programmatic URL assignments or defaults, and is *not always reliable* unless the URLs are set via config, environment variables, or command line switches.

## The Reliable Way: After the Server has Started

The correct strategy is to query `app.Urls`, but only after the server has started using `app.Start()`. Avoid `app.Run()` if you wish to capture the URLs programmatically.

### Example Startup Banner in `program.cs`

```csharp
// At the bottom of program.cs
// Replace app.Run() with:
app.Start();

Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine($@"--------------------------------- West Wind Web Store v{wsApp.Version} ---------------------------------");
Console.ResetColor();

var urlList = app.Urls;
string urls = string.Join(" ", urlList);

Console.Write(" Urls: ");
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine(urls, ConsoleColor.DarkCyan);
Console.ResetColor();

Console.WriteLine($" Runtime: {RuntimeInformation.FrameworkDescription} - {builder.Environment.EnvironmentName}");
Console.WriteLine($"Platform: {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})");
Console.WriteLine();

app.WaitForShutdown();
```

**Key points:**

- Use `app.Urls` *after* `app.Start()`.
- Present information as a startup banner for clarity during development or debugging.

## Retrieving Host URLs Within an HTTP Request

URLs can also be obtained at runtime within a request by leveraging dependency injection to access `IServer` and `IServerAddressesFeature`:

```csharp
public object SystemInfo([FromServices] IServer server) {
    var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
    // process addresses...
}
```

Alternatively, this pattern also works in startup code after `app.Start()`:

```csharp
var server = app.Services.GetService(typeof(IServer)) as IServer;
var addrs = server.Features.Get<IServerAddressesFeature>()?.Addresses;
```

While both `app.Urls` and `IServerAddressesFeature` can be used, `app.Urls` is simpler if accessed post-startup.

## Using .NET Logging to Reveal Hosting URLs

ASP.NET Core conveniently outputs listening ports in the debug log, given that:

- Logging is enabled to `Information` level on the `Microsoft.Hosting.Lifetime` category
- You're running in the `Development` environment, or have the right log level in production via `appsettings.json`:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run
```

Or configure explicitly in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Error",
      "Microsoft.Hosting.Lifetime": "Information" // Enables port log
    }
  }
}
```

**Note:** This method is often disabled in production for performance/security reasons.

## Summary

- Multiple mechanisms exist to set hosting URLs; reliably retrieving the ones in use requires care
- Using `app.Urls` after `app.Start()`, or extracting `IServerAddressesFeature` via DI, are robust methods
- Logging can provide this info during development but is less reliable in production

## References

- [8 ways to set the URLs for an ASP.NET Core app - Andrew Lock](https://andrewlock.net/8-ways-to-set-the-urls-for-an-aspnetcore-app/)
- [Add an ASP.NET Runtime Information Startup Banner](https://weblog.west-wind.com/posts/2021/Nov/09/Add-an-ASPNET-Runtime-Information-Startup-Banner)

---

**Comments/Discussion Highlights:**

- The role of structured logging and configuration is highlighted in the comments, with pros and cons mentioned regarding production use.

---

*This post was created and published with the [Markdown Monster Editor](https://markdownmonster.west-wind.com)*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2024/Sep/03/Getting-the-ASPNET-Core-Server-Hosting-Urls-at-Startup-and-in-Requests)
