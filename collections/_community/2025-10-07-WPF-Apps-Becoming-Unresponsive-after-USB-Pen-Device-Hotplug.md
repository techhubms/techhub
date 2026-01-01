---
layout: "post"
title: "WPF Apps Becoming Unresponsive after USB Pen Device Hotplug"
description: "This post discusses a persistent issue where WPF applications on Windows 10 and 11 become unresponsive when USB input devices supporting pen or stylus events are plugged or unplugged. It covers debugging observations, possible causes involving the PenThreadWorker, and workarounds using the Stylus and Touch support switch, along with their limitations."
author: "shepburn"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/app-development/wpf-application-becomes-unresponsive-after-plugging-unplugging/m-p/4459751#M1274"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=dotnet"
date: 2025-10-07 18:38:42 +00:00
permalink: "/2025-10-07-WPF-Apps-Becoming-Unresponsive-after-USB-Pen-Device-Hotplug.html"
categories: ["Coding"]
tags: [".NET Framework", "AppContext Switch", "Coding", "Community", "Input Devices", "PenThreadWorker", "Stylus", "Tablet Events", "Unresponsive Application", "USB", "VS", "Windows 10", "Windows 11", "WM DEVICECHANGE", "WM TABLET ADDED", "WM TABLET DELETED", "WPF", "WPF Performance"]
tags_normalized: ["dotnet framework", "appcontext switch", "coding", "community", "input devices", "penthreadworker", "stylus", "tablet events", "unresponsive application", "usb", "vs", "windows 10", "windows 11", "wm devicechange", "wm tablet added", "wm tablet deleted", "wpf", "wpf performance"]
---

shepburn explores a recurring problem where WPF applications freeze or become unresponsive when plugging or unplugging USB devices that provide touch or stylus functionality, sharing observations, a workaround, and its limitations.<!--excerpt_end-->

# WPF Apps Becoming Unresponsive after USB Pen Device Hotplug

**Author:** shepburn

## Overview

This community post describes an issue with Windows Presentation Foundation (WPF) applications on Windows 10 and 11 that become unresponsive when a USB input device (supporting pen or touch events) is plugged in or removed. The behavior was consistently observed across different .NET Framework versions and even in Visual Studio.

## Reproduction Steps

- Create a basic WPF application in Visual Studio.
- Run the app.
- Plug in or unplug any USB device that registers as an input device for stylus/touch events.
- Observe that the app can freeze for several minutes or sometimes crash due to excessive memory usage.

## Technical Investigation

- Using Spy++, observed the app received expected messages such as `WM_DEVICECHANGE`, `WM_POINTERDEVICECHANGE`, `WM_TABLET_ADDED` (0x02C8), and `WM_TABLET_DELETED` (0x02C9).
- Following the device event, a rapid stream of thousands or tens of thousands of `WM_USER` (0x0400) messages (with `wParam` 0x0000BABE) is posted to a window named "OLEChannelWnd" (located at 0x0, 0x0 and sized 0x0).
- During this flood of messages, the app becomes unresponsive, sometimes for 5-10 minutes, risking crashes from memory exhaustion.

## Reference to Related Issues

- Online research points to problems with the `PenThreadWorker`, which may deadlock during stylus/touch device (re)initialization.
- Disabling stylus and touch support via the AppContext switch (`Switch.System.Windows.Input.Stylus.DisableStylusAndTouchSupport`) can resolve the issue for apps that do not require stylus processing:
  - App config setting:

    ```xml
    <runtime>
      <AppContextSwitchOverrides value="Switch.System.Windows.Input.Stylus.DisableStylusAndTouchSupport=true" />
    </runtime>
    ```

  - Or in startup code:

    ```csharp
    AppContext.SetSwitch("Switch.System.Windows.Input.Stylus.DisableStylusAndTouchSupport", true);
    ```

  - Note: This must occur at startup and cannot be toggled dynamically.
- The workaround cannot be used for apps that need tablet/stylus input.

## Attempts and Limitations

- Tried multiple .NET Framework versions (4.5.1, 4.6, 4.8.1): the issue persisted.
- Applied Microsoft's guidance from [official documentation](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/disable-the-realtimestylus-for-wpf-applications), but it did not resolve the issue in all cases.
- The behavior is reproducible in simple test apps and also in various WPF-based third-party and Microsoft-built applications, including Visual Studio 2017.

## Summary

- Workaround: Disabling stylus/touch support at startup resolves the hang for applications that do not rely on pen/tablet events.
- Limitation: No known fix for applications that require stylus/touch events support; the workaround is not suitable.
- The issue affects a broad spectrum of WPF applications across several frameworks and development environments.

## Further Reading

- Microsoft's documentation: [Disable RealTimeStylus for WPF applications](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/disable-the-realtimestylus-for-wpf-applications)

## Author's Experience

shepburn has validated this issue across multiple setups, .NET versions, and even within Visual Studio itself, supporting that it's a platform-level problem with WPF's handling of tablet input device hotplug events.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/app-development/wpf-application-becomes-unresponsive-after-plugging-unplugging/m-p/4459751#M1274)
