---
layout: "post"
title: "Preparing Your .NET MAUI Apps for Google Play’s 16 KB Page Size Requirement"
description: "This guide covers Google's upcoming requirement for Android apps to support 16 KB memory page sizes starting November 1, 2025. It explains what the 16 KB requirement is, why it matters, how .NET MAUI 9 supports it, and what .NET developers need to do. Steps include upgrading to .NET 9, auditing dependencies for compliance, and testing on 16 KB environments to ensure app compatibility and continued publishing on Google Play."
author: "Gerald Versluis"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/maui-google-play-16-kb-page-size-support/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-09-26 17:05:00 +00:00
permalink: "/2025-09-26-Preparing-Your-NET-MAUI-Apps-for-Google-Plays-16-KB-Page-Size-Requirement.html"
categories: ["Coding"]
tags: [".NET", ".NET 9", ".NET For Android", "16 KB Page Size", "Android 15", "Android Development", "App Compatibility", "Coding", "Dependencies", "Developer Guidance", "Emulator Testing", "Google Play", "MAUI", "Memory Management", "Mobile Apps", "News", "NuGet", "Page Size Requirement"]
tags_normalized: ["dotnet", "dotnet 9", "dotnet for android", "16 kb page size", "android 15", "android development", "app compatibility", "coding", "dependencies", "developer guidance", "emulator testing", "google play", "maui", "memory management", "mobile apps", "news", "nuget", "page size requirement"]
---

Gerald Versluis explains the upcoming Google Play 16 KB page size mandate for Android apps and how .NET MAUI developers can prepare by upgrading to .NET 9 and ensuring all dependencies are compliant.<!--excerpt_end-->

# Preparing Your .NET MAUI Apps for Google Play’s 16 KB Page Size Requirement

Starting **November 1st, 2025**, Google Play will require all new and updated apps targeting Android 15+ to support 16 KB memory page sizes on 64-bit devices. This guide explains what the requirement means for .NET MAUI developers and the steps you need to take to keep your apps compliant.

## What is the 16 KB Page Size Requirement?

Android is moving from 4 KB to 16 KB memory page sizes to boost performance, especially on devices with more RAM. Google has reported:

- **Lower app launch times** (average 3.16% improvement, up to 30% for some apps)
- **Reduced power draw** during app launch (average 4.56% decrease)
- **Improved system performance**, such as the camera app launching 4.48% faster (hot starts) and 6.60% faster (cold starts)

## .NET MAUI Support

- **.NET MAUI 9** supports 16 KB page sizes by default. Upgrade your app to .NET 9 to ensure compatibility.
- **.NET MAUI 8** will be out of support after May 14, 2025. Reference: [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

## Check Your Dependencies

All dependencies in your .NET MAUI project must also support 16 KB page sizes. When you build, you may see warnings like:

```
Android 16 will require 16 KB page sizes, shared library '{library_name}' does not have a 16 KB page size. Please inform the authors of the NuGet package '{package_name}' version '{version}' which contains '{file_path}'.
See https://developer.android.com/guide/practices/page-sizes for more details.
```

**Action Steps:**

1. **Update dependencies** to versions supporting 16 KB page size
2. **Contact package authors** if updates aren’t available
3. **Replace dependencies** that are no longer maintained

Manual compliance checks can be performed using scripts from [Google’s documentation](https://developer.android.com/guide/practices/page-sizes#elf-alignment).

## What You Need to Do

- **Upgrade to .NET 9** if you haven’t already
- **Audit your dependencies** for 16 KB support
- **Update or replace non-compliant dependencies**
- **Test your app** in 16 KB environments via Android emulators or device developer options

**Don’t wait until the deadline.** Start these steps early to avoid interruptions in releasing updates on Google Play.

## Summary

The 16 KB page size requirement delivers system and app performance benefits, but demands preparation. With .NET MAUI 9, your framework is ready; ensure the same for your dependencies.

### References

- [Android 16 KB Page Size Support Guide](https://developer.android.com/guide/practices/page-sizes)
- [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui)
- [Google Play 16 KB Page Size Blog Post](https://android-developers.googleblog.com/2025/05/prepare-play-apps-for-devices-with-16kb-page-size.html)
- [.NET for Android repository issue](https://github.com/dotnet/android/issues/10477)

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/maui-google-play-16-kb-page-size-support/)
