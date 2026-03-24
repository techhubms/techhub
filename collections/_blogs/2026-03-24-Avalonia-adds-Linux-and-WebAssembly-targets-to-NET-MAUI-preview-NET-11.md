---
primary_section: dotnet
author: DevClass.com
feed_name: DevClass
title: Avalonia adds Linux and WebAssembly targets to .NET MAUI (preview, .NET 11)
date: 2026-03-24 17:02:00 +00:00
section_names:
- dotnet
external_url: https://www.devclass.com/development/2026/03/24/avaloniaui-enhances-net-maui-with-linux-and-webassembly-support/5209515
tags:
- .NET
- .NET 11
- Android
- Avalonia 12
- AvaloniaUI
- Blogs
- Cross Platform UI
- GUI Framework
- Ios
- Linux
- Macos
- MAUI
- Wayland
- WebAssembly
- WinUI
- WPF
- X11
- XWayland
---

DevClass.com (Tim Anderson) reports on AvaloniaUI’s preview backend for .NET MAUI using .NET 11, which aims to add Linux and WebAssembly browser targets by letting developers use Avalonia-drawn controls alongside or instead of MAUI’s native controls.<!--excerpt_end-->

# Avalonia adds Linux and WebAssembly targets to .NET MAUI (preview, .NET 11)

AvaloniaUI has previewed MAUI support for Linux and WebAssembly browser applications—platforms Microsoft’s own cross-platform .NET UI framework does not currently cover. The preview is a backend for .NET MAUI (Multi-platform App UI) based on **.NET 11 (preview)**.

## What Avalonia is adding to MAUI

- A **MAUI backend** that enables developers to target:
  - **Linux**
  - **WebAssembly (browser apps)**
- The approach is to allow using **Avalonia controls**:
  - **Alongside** MAUI controls, or
  - **In place of** MAUI controls

## Why this is more than “just another target”

Cross-platform UI frameworks typically render controls in one of two ways:

- **Native controls**: call the OS platform APIs to render buttons, switches, etc.
  - Pros: more “correct” look-and-feel on each OS
- **Custom-drawn controls**: render their own controls for consistency across platforms
  - Pros: more uniform UI across targets

In this article’s framing:

- **.NET MAUI** uses the **native** approach.
- **Avalonia** uses **custom drawing**.
- The preview enables MAUI apps to use Avalonia’s custom-drawn controls, unlocking Avalonia’s platform reach.

![Software panel with color gradients, shapes, and text alignment previews](https://image.devclass.com/5211351.webp?imageId=5211351&width=960&height=548&format=jpg)

## Background: Avalonia’s role in the .NET UI ecosystem

- Avalonia is an **open source .NET GUI framework** inspired by **WPF (Windows Presentation Foundation)**.
- It was created in 2013 by developer **Steven Kirk** when WPF was seen by some as neglected.
- It became popular as a way for developers with WPF experience to target **macOS and Linux**, and now also targets:
  - **Windows**
  - **macOS**
  - **Linux**
  - **iOS**
  - **Android**
  - **WebAssembly**

## Preview status and key limitations called out

The preview referenced in the article:

- Is based on **.NET 11**, expected to be generally available in November.
- Will therefore likely remain in **preview until .NET 11 ships**.

Software engineer **Tim Miller** said “there are still many areas to address,” including:

- A version of the **Microsoft MAUI APIs** for “essential platform features” such as:
  - **Storage**
  - **Media access**

Other limitations mentioned:

- **No Wayland support yet** on Linux; it relies on:
  - **X11**, or
  - **XWayland** compatibility layer
- It’s **not yet possible** to host Avalonia controls within **WinUI** (the Windows GUI API MAUI targets when running on Windows).

## Spillover benefits for Avalonia itself

Miller also said the MAUI work benefited Avalonia directly via:

- New controls and APIs planned for **Avalonia 12**
- Efforts to “close the gap” between the control sets available in **.NET MAUI** and **Avalonia**

## Adoption concerns around MAUI

The article argues the main challenge is MAUI uptake:

- Developers reportedly struggle with **bugs** and **slow updates**.
- Microsoft is described as not using MAUI much internally; examples given:
  - **Microsoft Teams** uses **TypeScript + Electron**, not MAUI.
  - **React Native** is described as popular inside Microsoft (used in Microsoft Office and elsewhere).

A linked GitHub discussion includes comments such as:

- Problems during the **.NET 9 → .NET 10** transition, with some Android/iOS features not working as expected, leading one developer to revert back to .NET 9.
- Another comment claiming Q1 2026 had “constant regressions and other bugs” making MAUI difficult to use in production.

The article notes that frequent **Android and iOS updates** create a moving target for cross-platform frameworks like MAUI.

## Links referenced

- Avalonia MAUI preview announcement: https://avaloniaui.net/blog/maui-avalonia-preview-1
- Background on Avalonia (The Register): https://www.theregister.com/2021/03/30/crossplatform_wpf_anyone_unpacking_avolonia/
- GitHub discussion “how is MAUI doing in 2026?”: https://github.com/dotnet/maui/discussions/34171#discussion-9513663
- Comment about regressions: https://github.com/dotnet/maui/discussions/34171#discussioncomment-15961081


[Read the entire article](https://www.devclass.com/development/2026/03/24/avaloniaui-enhances-net-maui-with-linux-and-webassembly-support/5209515)

