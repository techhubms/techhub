---
external_url: https://www.stevejgordon.co.uk/a-brief-introduction-to-the-dotnet-muxer
title: A Brief Introduction to the .NET Muxer (aka dotnet.exe)
author: Steve Gordon
viewing_mode: external
feed_name: Steve Gordon's Blog
date: 2024-10-25 08:57:43 +00:00
tags:
- .NET
- Apphost
- CLI
- CoreHost
- Corehost.cpp
- Debugging
- Dotnet.exe
- Execution Model
- Internals
- Microsoft
- Muxer
- Native Code
- Performance
- Runtime
- SDK
- Tracing
section_names:
- coding
---
In this introduction by Steve Gordon, readers gain foundational knowledge of the .NET muxer (dotnet.exe), an essential component of the .NET ecosystem. Gordon's thorough exploration unveils the inner workings and key mechanisms underpinning the .NET command-line interface.<!--excerpt_end-->

# A Brief Introduction to the .NET Muxer (aka dotnet.exe)

*By Steve Gordon*

![Blog post header image with the title "A Brief Introduction to the .NET Muxer (aka dotnet.exe)"](https://www.stevejgordon.co.uk/wp-content/uploads/2024/10/A-Brief-Introduction-to-the-.NET-Muxer-aka-dotnet.exe_-750x410.png)

---

This post marks the start of what I expect will be a long-term effort to explore the inner workings of .NET, expose the “magic” behind the scenes, and explain the mechanisms and underlying components of the .NET execution model. Today, we begin with a brief introduction to the .NET muxer (dotnet.exe).

> **Note:** These posts are a deep dive into .NET internals and won’t typically apply to day-to-day development. While they might not have direct coding applications, understanding these concepts can aid in debugging complex issues or optimizing performance. This series primarily serves as a tool to expand my own understanding of .NET at a deeper level.

## What is the .NET Muxer?

As a .NET developer, you've almost certainly used the .NET muxer—more commonly known as **dotnet.exe**—even if you haven’t considered its mechanics closely. Integrated Development Environments (IDEs) often automate interactions with it, but developers working with .NET Core or 5+ have inevitably engaged with the muxer directly.

Sometimes referred to in older documentation as **CoreHost**, the muxer functions as the primary executable that manages .NET command-line operations. The name "muxer" stands for *multiplexer*, reflecting its role as a single entry point to manage multiple versions of the .NET SDK and runtime. It directs commands for building, running, and administering .NET applications, streamlining version management for developers.

## Where Is It Installed?

After installing a .NET runtime or SDK, you’ll find a single instance of dotnet.exe in your installation directory (typically `C:\Program Files\dotnet` on Windows). Even with several versions of the SDK or runtime installed side-by-side, there’s generally only one dotnet executable present. This version usually matches the latest SDK or runtime on your machine and determines which version to use based on the application configuration and global settings.

## The Codebase and Core Logic

The muxer’s code resides within the [runtime repository](https://github.com/dotnet/runtime) as native code—specifically in `corehost.cpp`. Here, you’ll find the [main entry point](https://github.com/dotnet/runtime/blob/release/9.0-rc2/src/native/corehost/corehost.cpp#L298), which interacts closely with both the .NET runtime and application hosts.

> **Reference:** This deep dive is based on the .NET 9 RC2 release, the latest available at this writing. You can follow along with the code in a browser tab; while some code snippets are included here for illustrative purposes, full source sections are omitted for brevity.

## The Main Function and Tracing

Within the main function, you’ll encounter preprocessor directives like `FEATURE_APPHOST`. Parts of the code are designed to output the muxer itself (dotnet.exe), while significant sections are reused by **apphost**—a native wrapper used to run .NET applications as stand-alone executables. Although the post does not delve into the apphost today, it notes that version resolution logic is shared between it and dotnet.exe.

A simplified flow from `corehost.cpp` illustrates the startup process:

```cpp
trace::setup();

if (trace::is_enabled()) {
    trace::info(_X("--- Invoked %s [version: %s] main = {"), CURHOST_TYPE, get_host_version_description().c_str());
    for (int i = 0; i < argc; ++i) {
        trace::info(_X("%s"), argv[i]);
    }
    trace::info(_X("}"));
}
...
int exit_code = exe_start(argc, argv);
trace::flush();
...
return exit_code;
```

Before invoking the main operational logic (inside `exe_start`), the above code sets up tracing if it is enabled. By default, tracing is off, but can be activated by setting the `COREHOST_TRACE` environment variable. Traced information helps debug and troubleshoot .NET applications by logging muxer activity. Output goes to `stderr` by default but can be redirected with `COREHOST_TRACEFILE`. The detail level is set by `COREHOST_TRACE_VERBOSITY`, defaulting to 4 (most verbose).

**Enabling Tracing in PowerShell:**

```powershell
$Env:COREHOST_TRACE = 1
```

When running dotnet.exe with tracing enabled (and no arguments), you’ll see output similar to the following (truncated for clarity):

```
Tracing enabled @ Wed Oct 23 06:44:22 2024 GMT
--- Invoked dotnet [version: 9.0.0-rc.2.24473.5 @Commit: 990ebf52fc408ca45929fd176d2740675a67fab8] main = { C:\Program Files\dotnet\dotnet.exe }
Usage: dotnet [options] Usage: dotnet [path-to-application] …
```

The initial tracing confirms successful activation and invocation of dotnet.exe, followed by usage instructions if no further commands are provided.

## Inside exe_start: Path Resolution and Security

Turning to the [exe_start function](https://github.com/dotnet/runtime/blob/release/9.0-rc2/src/native/corehost/corehost.cpp#L108):

- The first steps resolve the executable’s path, including potential symbolic links.
- A [security check](https://github.com/dotnet/runtime/blob/release/9.0-rc2/src/native/corehost/corehost.cpp#L162) ensures the executable name (pre-extension) is "dotnet". This is to prevent tampering—if renamed, dotnet.exe will fail to execute to avoid misleading code signatures:

**If you rename dotnet.exe and run it:**

```
Tracing enabled @ Wed Oct 23 07:25:33 2024 GMT
--- Invoked dotnet [version: 9.0.0-rc.2.24473.5 @Commit: 990ebf52fc408ca45929fd176d2740675a67fab8] main = { C:\Program Files\dotnet2.exe }
Error: cannot execute dotnet when renamed to dotnet2.
```

The process then fails with a core host entry point failure and exit code `0x80008084`.

- The next argument parsing checks (
  [link](https://github.com/dotnet/runtime/blob/release/9.0-rc2/src/native/corehost/corehost.cpp#L174)
) verify that at least one argument is provided. With too few arguments, the program prints usage instructions and exits with an error code (`0x80008081`).

## Looking Ahead

This post concludes with the muxer preparing to hand off to deeper logic, which determines how .NET applications and SDKs are discovered and invoked. In the [next post in the series](https://www.stevejgordon.co.uk/how-dotnet-muxer-resolves-and-loads-the-hostfxr-library), Steve will focus on the `host_fxr` logic: how `hostfxr.dll` is located and loaded.

### Series Table of Contents

- A Brief Introduction to the .NET Muxer (aka dotnet.exe) [This post]
- [How dotnet.exe resolves and loads the hostfxr library – Exploring the .NET muxer](https://www.stevejgordon.co.uk/how-dotnet-muxer-resolves-and-loads-the-hostfxr-library)

---

If you've found this post helpful, Steve welcomes your support:

[![](https://cdn.buymeacoffee.com/buttons/bmc-new-btn-logo.svg) Buy me a coffee](https://www.buymeacoffee.com/stevejgordon) [![](https://www.stevejgordon.co.uk/wp-content/uploads/2020/07/PaypalLogo.png)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=WV4JPPV9FS34L&source=url)

---

#### About Steve Gordon

Steve Gordon is a Pluralsight author, 7x Microsoft MVP, and a .NET engineer at [Elastic](https://www.elastic.co) where he maintains the .NET APM agent and related libraries. He's passionate about .NET, community engagement, and open source. Steve blogs, creates videos, and delivers talks globally. You can find him online as [@stevejgordon](https://twitter.com/stevejgordon).

This post appeared first on "Steve Gordon's Blog". [Read the entire article here](https://www.stevejgordon.co.uk/a-brief-introduction-to-the-dotnet-muxer)
