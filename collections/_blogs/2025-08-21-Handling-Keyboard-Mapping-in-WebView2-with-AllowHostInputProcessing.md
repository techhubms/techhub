---
layout: post
title: Handling Keyboard Mapping in WebView2 with AllowHostInputProcessing
author: Rick Strahl
canonical_url: https://weblog.west-wind.com/posts/2025/Aug/20/Using-the-new-WebView2-AllowHostInputProcessing-Keyboard-Mapping-Feature
viewing_mode: external
feed_name: Rick Strahl's Blog
feed_url: https://feeds.feedburner.com/rickstrahl
date: 2025-08-21 03:17:34 +00:00
permalink: /coding/blogs/Handling-Keyboard-Mapping-in-WebView2-with-AllowHostInputProcessing
tags:
- .NET
- AllowHostInputProcessing
- Blogs
- C# Code Samples
- Coding
- CoreWebView2ControllerOptions
- Hybrid Apps
- JavaScript Interop
- Key Bindings
- Keyboard Mapping
- Markdown Monster
- UI Automation
- WebView
- WebView2
- Windows Desktop Development
- WinForms
- WPF
section_names:
- coding
---
Rick Strahl examines the implementation of keyboard mapping and menu accelerator key support in Microsoft WebView2 for .NET and WPF applications, using practical examples from Markdown Monster.<!--excerpt_end-->

# Handling Keyboard Mapping in WebView2 with AllowHostInputProcessing

**By Rick Strahl**

Hybrid desktop/web applications often use the [WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/) control to embed web content inside .NET or WPF interfaces. This integration can create complex keyboard and focus challenges, especially when trying to ensure a natural user experience across both the web and desktop parts of the app.

## The Challenge: Main Menu Accelerators and Keyboard Focus

One major issue in apps like Markdown Monster—an advanced Markdown editor built on WPF and JavaScript—is that main window menu accelerator keys (like the `Alt` key in Windows) don't work intuitively out of the box. While normal Windows apps activate the main menu with `Alt`, WebView2 historically would swallow keyboard events, requiring awkward workarounds to pass them to the host app.

![Accelerator keys with Alt](https://weblog.west-wind.com/images/2025/Using-the-new-WebView2-AllowHostInputProcessing-Keyboard-Mapping-Feature/PressingAltActivatesMenuMnemonics.png)

## Previous Workarounds

Before the arrival of dedicated options in WebView2, developers had to intercept Alt-key presses, infer the intent via timing code, and manually forward or manipulate focus in WPF. This was complex, error-prone, and prone to inconsistent results.

## Understanding the WebView2 Focus Model

WebView2, by default, keeps focus and handles key events within itself. For complex editors and hybrid apps, keyboard shortcuts have to be strategically split between JavaScript (for performance and customization) and WPF (for system-level or UI-heavy actions). Markdown Monster uses a configurable key binding manager to determine where each key should be handled.

## The Solution: AllowHostInputProcessing

Recent WebView2 versions (`.NET SDK 1.0.3351` with `runtime 1.0.1901.177` and above) introduce the `CoreWebView2ControllerOptions.AllowHostInputProcessing` setting. When enabled, special keyboard and mouse events like `Alt` are forwarded to the WPF host before being handled in the WebView2, allowing menu accelerators and other system keys to behave more naturally.

### Enabling AllowHostInputProcessing

To enable this, configure the controller options during environment setup:

```csharp
if (allowHostInputProcessing) {
  var opts = Environment.CreateCoreWebView2ControllerOptions();
  opts.AllowHostInputProcessing = true;
  await webBrowser.EnsureCoreWebView2Async(environment, opts);
}
```

Developers should create a helper function to set up shared environments properly, especially for advanced scenarios with multiple controls:

```csharp
public async Task InitializeWebViewEnvironment(WebView2 webBrowser, CoreWebView2Environment environment = null, ... , bool allowHostInputProcessing = false) {
  // [snip] setup logic
  if (allowHostInputProcessing) {
    var opts = Environment.CreateCoreWebView2ControllerOptions();
    opts.AllowHostInputProcessing = true;
    await webBrowser.EnsureCoreWebView2Async(environment, opts);
  } else {
    await webBrowser.EnsureCoreWebView2Async(environment);
  }
  // [snip]
}
```

**Important Note:**
`EnsureCoreWebView2Async` will not complete if the WebView is not visible during initialization.

## Caveats: Navigation Keys and Other Side-Effects

When AllowHostInputProcessing is enabled, some keys (like `Tab` and `Shift-Tab`) now move focus at the WPF level by default, even if your intent is to keep those keys inside an embedded editor. This requires further custom handling:

- Use WPF key bindings to intercept Tab/Shift-Tab
- Cancel the WPF event and explicitly trigger the relevant command in your JavaScript-based editor

**Sample solution:**

```csharp
// Custom WPF key binding to intercept Tab
new AppKeyBinding {
  Id = "EditorCommand_TabKey",
  Key = "Tab",
  Command = model.Commands.KeyBoardOperationCommand,
  CommandParameter = "TabKey",
  HasJavaScriptHandler = false,
  BindingControl = mmApp.Window.TabControl
},
// ...and for Shift+Tab
```

And then in your command logic, invoke a JavaScript key event as needed:

```javascript
fireTabKey: function (e) {
  const event = new KeyboardEvent("keydown", { key: 'Tab', keyCode: 9, code: 'Tab', ... });
  te.editor.textInput.getElement().dispatchEvent(event);
}
```

This ensures the editor sees the Tab as input, not just a focus change in WPF.

## Summary

- Keyboard handling across WebView2 and desktop hosts requires careful planning.
- The new `AllowHostInputProcessing` option improves compatibility with Windows keyboard conventions, especially menu accelerators.
- Developers must explicitly handle side effects for keys like Tab to maintain expected editor behavior.
- The article offers well-documented sample code suitable for adapting to other complex hybrid apps.

---
This post was created and published with [Markdown Monster Editor](https://markdownmonster.west-wind.com)

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Aug/20/Using-the-new-WebView2-AllowHostInputProcessing-Keyboard-Mapping-Feature)
