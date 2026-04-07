---
date: 2026-04-07 14:00:39 +00:00
section_names:
- dotnet
title: Take full control of your floating windows in Visual Studio
feed_name: Microsoft VisualStudio Blog
external_url: https://devblogs.microsoft.com/visualstudio/take-full-control-of-your-floating-windows-in-visual-studio/
primary_section: dotnet
tags:
- .NET
- Always On Top
- Ctrl + Double Click
- Docking
- Document Windows
- Environment Settings
- FancyZones
- Floating Windows
- IDE
- Layout Switching
- Microsoft PowerToys
- Minimize Behavior
- Multi Monitor Setup
- News
- Tool Windows
- Tools Options
- VS
- Window Management
- Window Ownership
- Window Snapping
- Windows Taskbar
author: Mads Kristensen
---

Mads Kristensen explains a lesser-known Visual Studio option that controls whether floating tool windows and document windows are owned by the main IDE window, and how setting it to “None” pairs well with PowerToys FancyZones for a cleaner multi-monitor workflow.<!--excerpt_end-->

# Take full control of your floating windows in Visual Studio

If you work with multiple monitors, floating tool windows and documents in Visual Studio can be a big productivity boost. You can pull out things like Solution Explorer, the debugger, or code files onto a second (or third) screen.

The friction: by default, floating windows are *owned* by the main Visual Studio window.

## What “owned by the main window” means

When a floating window is owned by the main Visual Studio window:

- It **doesn’t show up as a separate button** in the Windows taskbar.
- It **disappears when you minimize** the main IDE window.
- It **stays on top** of everything else, even when you don’t want it to.

That behavior can be great for some workflows and annoying for others.

## The setting to change

Go to:

**Tools > Options > Environment > Windows > Floating Windows**

Look for the dropdown labeled:

**“These floating windows are owned by the main window”**

It offers three choices:

- **None**
- **Tool Windows** (default)
- **Documents and Tool Windows**

Changing this setting can significantly change how floating windows behave.

## Favorite scenario: PowerToys FancyZones

This setting works especially well alongside Microsoft PowerToys and its FancyZones feature:

- PowerToys install: https://aka.ms/installpowertoys
- FancyZones docs: https://learn.microsoft.com/windows/powertoys/fancyzones

A useful setup is choosing **None** and then using FancyZones to create custom layouts across monitors.

With **None**, floating tool windows and documents behave more like normal application windows:

- They **appear in the taskbar**.
- They **stay visible** even if you minimize the main Visual Studio window.
- You can **snap** them cleanly into FancyZones layouts.
- They don’t constantly **force themselves to the front**.

## When to choose each option

- **None**: Maximum independence. Everything gets its own taskbar entry and normal window behavior. Works well for heavy multi-monitor use and FancyZones.
- **Tool Windows**: Middle ground—documents can float more freely, while tool windows stay tied to the IDE.
- **Documents and Tool Windows**: Classic Visual Studio behavior.

## Pro tip: fast dock/float switching

You can also combine this with a shortcut:

- **Ctrl + double-click** on any tool window title bar for quick layout switching.

Related post: https://devblogs.microsoft.com/visualstudio/easily-dock-and-float-tool-windows/

## Discussion prompt

The author asks what you prefer:

- None
- Tool Windows
- Default behavior


[Read the entire article](https://devblogs.microsoft.com/visualstudio/take-full-control-of-your-floating-windows-in-visual-studio/)

