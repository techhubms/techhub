---
layout: "post"
title: "How dotnet.exe Resolves and Loads the hostfxr Library – Exploring the .NET Muxer"
description: "Steve Gordon provides a deep dive into the .NET muxer (dotnet.exe), focusing on the process by which it resolves and loads the hostfxr library. Detailing internal implementation steps, this post is ideal for those interested in .NET runtime internals and advanced diagnostics."
author: "Steve Gordon"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.stevejgordon.co.uk/how-dotnet-muxer-resolves-and-loads-the-hostfxr-library"
viewing_mode: "external"
feed_name: "Steve Gordon's Blog"
feed_url: "https://www.stevejgordon.co.uk/feed"
date: 2024-11-08 10:11:59 +00:00
permalink: "/posts/2024-11-08-How-dotnetexe-Resolves-and-Loads-the-hostfxr-Library-Exploring-the-NET-Muxer.html"
categories: ["Coding"]
tags: [".NET", "CLR", "Coding", "Corehost", "Diagnostics", "Dotnet.exe", "Framework Resolution", "Hostfxr", "Internals", "Linux", "Macos", "Muxer", "Platform Abstraction", "Posts", "Runtime", "Version Selection", "Windows"]
tags_normalized: ["dotnet", "clr", "coding", "corehost", "diagnostics", "dotnetdotexe", "framework resolution", "hostfxr", "internals", "linux", "macos", "muxer", "platform abstraction", "posts", "runtime", "version selection", "windows"]
---

In this article, Steve Gordon explores how dotnet.exe resolves and loads the hostfxr library, shedding light on the .NET muxer's internal steps. The discussion is a part of his deep dive series into the .NET hosting architecture.<!--excerpt_end-->

# How dotnet.exe Resolves and Loads the hostfxr Library – Exploring the .NET Muxer

*By Steve Gordon*

![Blog post header](https://www.stevejgordon.co.uk/wp-content/uploads/2024/11/How-dotnet.exe-resolves-and-loads-the-hostfxr-library-750x410.png)

In this post, we continue our journey into the functionality and implementation of `dotnet.exe`, specifically focusing on how the **hostfxr** library is resolved and loaded. This post follows part one of this series, [A Brief Introduction to the .NET Muxer (aka dotnet.exe)](https://www.stevejgordon.co.uk/a-brief-introduction-to-the-dotnet-muxer).

> **Note:** These posts are a deep dive into .NET internals and won’t typically apply to day-to-day development. While they might not have direct coding applications, understanding these concepts can aid in debugging complex issues or optimizing performance. This series primarily serves as a tool to expand my own understanding of .NET at a deeper level.

## Understanding the Role of hostfxr

The next phase for `dotnet.exe` is to hand control to the `hostfxr` library. This crucial component of the hosting mechanism is responsible for finding and resolving the runtime and the framework an app needs. The name "hostfxr" stands for ".NET Host Framework Resolver". It was introduced in .NET Core 2.0 (2017) to improve separation of concerns and to allow servicing of its logic without having to stop all running instances of the `dotnet.exe` host.

> **Note:** At the time of writing, `.NET 9 RC2` is the most current release.

## Step-by-Step: Resolving hostfxr

Before `dotnet.exe` can hand over control, it must resolve the correct version of `hostfxr`. This is achieved by the [`hostfxr_resolver_t` class](https://github.com/dotnet/runtime/blob/v9.0.0-rc.2.24473.5/src/native/corehost/hostfxr_resolver.h). The constructor receives the `app_root` variable, which symbolizes the path of the current executable.

### key Constructor Logic

- The constructor initializes the search for `hostfxr.dll` using a `search_location` enum to determine potential locations.
- Using the function `try_get_dotnet_search_options`, it tries to populate options regarding the dotnet search paths.
- A well-known placeholder (SHA-256 of "dotnet-search" in UTF-8, stored in `EMBED_DOTNET_SEARCH_FULL_UTF8`) can take up the value if the executable doesn’t provide an explicit path.
- The function extracts and interprets search flags. In most cases with the muxer, the app-relative flag isn't set, thus default paths are used.

### Directory Resolution

- If no app-relative path is set, the resolution process next invokes [`try_get_path`](https://github.com/dotnet/runtime/blob/v9.0.0-rc.2.24473.5/src/native/corehost/fxr_resolver.cpp#L64), which, for the muxer, uses the host executable’s directory as the root search path.
- This root directory is combined with the folder structure “host\fxr” (e.g., usually `C:\Program Files\dotnet\host\fxr` on Windows).

### Version Discovery and Selection

- Inside the `host\fxr` directory, the code [lists all subdirectories](https://github.com/dotnet/runtime/blob/v9.0.0-rc.2.24473.5/src/native/corehost/fxr_resolver.cpp#L12) corresponding to different `hostfxr` versions. Each folder's name is parsed as a semantic version.
- The highest version is selected using the version comparison logic (`fx_ver_t` struct and `std::max`).

### Platform Library Naming

- The actual file sought depends on the platform:
    - **Windows:** `hostfxr.dll`
    - **macOS:** `libhostfxr.dylib`
    - **Linux:** `libhostfxr.so`
- Macros in the code ensure the correct filename is built for the host OS.

### Loading the Library

- Having found the latest version, the presence of the file (e.g., `hostfxr.dll`) is verified (`file_exists_in_dir`).
- Once located, platform-specific logic loads the library: For example, Windows uses [`LoadLibraryExW`](https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-loadlibraryexw) to load the DLL into the host process.
- The module is ‘pinned’ to remain in memory, and `hostfxr_resolver_t`'s status is set to indicate success.
- If the status isn't success, control returns from the startup code with an appropriate error/exit code.
- When loading is successful, execution moves to the next phase, where control is passed fully to hostfxr.

## Real-World Examples: Diagnostics and Trace Output

You can observe the entire process by reviewing corehost trace logs when running `dotnet.exe --info`. Example output:

```
.NET root search location options: 0
Reading fx resolver directory=[C:\Program Files\dotnet\host\fxr]
Considering fxr version=[6.0.35]...
Considering fxr version=[7.0.20]...
Considering fxr version=[8.0.10]...
Considering fxr version=[9.0.0-rc.1.24431.7]...
Considering fxr version=[9.0.0-rc.2.24473.5]...
Detected latest fxr version=[C:\Program Files\dotnet\host\fxr\9.0.0-rc.2.24473.5]...
Resolved fxr [C:\Program Files\dotnet\host\fxr\9.0.0-rc.2.24473.5\hostfxr.dll]...
Loaded library from C:\Program Files\dotnet\host\fxr\9.0.0-rc.2.24473.5\hostfxr.dll
```

This step-by-step logging shows how .NET discovers every available version and loads the most recent one. Additional insights are available by tracing with [Process Explorer](https://learn.microsoft.com/en-us/sysinternals/downloads/process-explorer), which shows the precise file activity during this operation.

![Process Explorer screenshot](https://www.stevejgordon.co.uk/wp-content/uploads/2024/11/process-explorer-of-dotnet-muxer-loading-of-hostfxr-1024x391.png)

- Events in the red box indicate directory listing and version evaluation.
- The blue box captures the presence validation of the DLL in the version directory.
- The green box shows when `hostfxr.dll` is actually loaded.

## Summary

This post provided an in-depth look into how the .NET muxer (`dotnet.exe`) resolves and loads the latest `hostfxr` library into memory, a process crucial for hosting .NET applications. While not common knowledge for daily development, understanding this mechanism can be invaluable for diagnosing low-level runtime issues or when contributing to the runtime itself.

We’ll continue in part 3, exploring what happens once control is handed to hostfxr.

### Other Posts in This Series

- [A Brief Introduction to the .NET Muxer (aka dotnet.exe)](https://www.stevejgordon.co.uk/a-brief-introduction-to-the-dotnet-muxer)
- How dotnet.exe resolves and loads the hostfxr library – Exploring the .NET muxer  *(This post)*

---

*Steve Gordon is a Pluralsight author, 7x Microsoft MVP, and .NET engineer at Elastic, maintaining the .NET APM agent and participating actively in the .NET community. [Follow him on Twitter](https://twitter.com/stevejgordon) or visit his [blog](https://www.stevejgordon.co.uk) for more .NET insights.*

This post appeared first on "Steve Gordon's Blog". [Read the entire article here](https://www.stevejgordon.co.uk/how-dotnet-muxer-resolves-and-loads-the-hostfxr-library)
