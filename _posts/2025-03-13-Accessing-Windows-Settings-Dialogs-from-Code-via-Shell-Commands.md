---
layout: "post"
title: "Accessing Windows Settings Dialogs from Code via Shell Commands"
description: "Rick Strahl explains how to use the Windows `ms-settings:` Protocol Handler/URI Scheme to open specific Windows settings directly from shell commands or code, especially in .NET or Windows applications. Practical code samples and activation methods are provided for developers."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/Mar/13/Accessing-Windows-Settings-Dialogs-from-Code-via-Shell-Commands"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2025-03-13 22:40:22 +00:00
permalink: "/2025-03-13-Accessing-Windows-Settings-Dialogs-from-Code-via-Shell-Commands.html"
categories: ["Coding"]
tags: [".NET", "Application Permissions", "C#", "Coding", "Ms Settings", "Posts", "PowerShell", "Process.Start", "Protocol Handler", "Settings Dialogs", "ShellExecute", "URI Scheme", "Windows"]
tags_normalized: ["dotnet", "application permissions", "csharp", "coding", "ms settings", "posts", "powershell", "processdotstart", "protocol handler", "settings dialogs", "shellexecute", "uri scheme", "windows"]
---

Rick Strahl discusses how developers can open Windows Settings dialogs directly from code or shell commands using the `ms-settings:` protocol handler. The article provides practical usage examples for applications built with .NET and Windows technologies.<!--excerpt_end-->

## Accessing Windows Settings Dialogs from Code via Shell Commands

**Author:** Rick Strahl  
**Posted:** March 13, 2025 • from Maui, Hawaii

---

### Overview

Windows provides a built-in protocol handler—`ms-settings:`—which allows developers and users to open specific Windows Settings dialogs directly, bypassing the usual navigation. This can be invoked via shell commands, from within applications, or even from the browser, streamlining user experience when your app needs users to adjust specific system settings.

### Key Use Cases

- Prompting users to change system settings required for app features (e.g., microphone access for speech recognition)
- Rapid navigation to configuration pages from apps or scripts
- Automating diagnostics or support workflows

---

### Example Command Usage

**Shell Commands in Applications:**

- Using `.NET` methods: `ShellExecute()`, `Process.Start()`
- Terminal commands: `start` (DOS), `start-process` (PowerShell)
- Directly in browsers

**Screenshot: Windows Settings Through Shell Commands**  
![Windows Settings Through Shell Commands](https://weblog.west-wind.com/images/2025/Accessing-Windows-Settings-Dialogs-from-Code-via-Shell-Commands/WindowsSettingsThroughShellCommands.png)

> You can directly access specific Windows Settings dialogs using `ms-settings:` URIs in your applications or scripts.

#### Real-World Application Example

In the Markdown Monster app, if speech recognition fails due to missing permissions, the app can open the exact relevant Settings dialog:

```csharp
try  
{  
    await EnsureCompiledAsync();  
    _ = _recognizer.ContinuousRecognitionSession.StartAsync();  
    _isDictating = true;  
}  
catch (Exception ex) when (ex.Message.Contains("privacy"))  
{  
    // Open the settings page for speech recognition
    Westwind.Utilities.ShellUtils.ShellExecute("ms-settings:privacy-speech");
}
```

---

## How to Activate ms-settings Handlers

#### Terminal Activation

**PowerShell/DOS:**

```ps
# DOS/Command Prompt

start "ms-settings:privacy-speech"

# PowerShell

start-process "ms-settings:privacy-speech"
```

**WSL Terminal:**

```bash
explorer.exe ms-settings:privacy-speech
```

#### Code Activation (.NET/C#)

Using code with `.NET` platforms, you can launch these dialogs via:

```csharp
Process.Start("ms-settings:privacy-speech");
// or your own helper
ShellUtils.ShellExecute("ms-settings:privacy-speech");
ShellUtils.GoUrl("ms-settings:privacy-speech");
```

All leverage the Windows `ShellExecute` API, which runs these commands.

---

## Reference for ms-settings Commands

You can jump to many Windows configuration pages; see the full reference:

- [ms-settings Command Reference (Microsoft Docs)](https://learn.microsoft.com/en-us/windows/apps/develop/launch/launch-settings-app#ms-settings-uri-scheme-reference)

Other protocol handlers exist that let you trigger different built-in Windows features from code or URLs (see MS docs for exploration).

---

## What Are Protocol Handlers/URI Schemes?

- **Protocol Handler**: Globally registered command invoked via a URL prefix (e.g., `ms-settings:`).
- Allows apps or browsers to trigger built-in operating system dialogs or app-specific features.
- Used for system features (like settings) or custom integrations (Markdown Monster, Teams, Zoom, etc.).
- Example: open a Markdown document in Markdown Monster from the browser via its own handler.

---

## Why This Is Useful

While not something you might use daily, directly opening configuration dialogs improves user guidance and troubleshooting, especially around complex features like security and permissions where normal navigation is cumbersome.

---

## Resources

- [ms-settings Command Reference (Microsoft Docs)](https://learn.microsoft.com/en-us/windows/apps/develop/launch/launch-settings-app#ms-settings-uri-scheme-reference)

## Related Reading

- [Adding minimal OWIN Identity Authentication to an Existing ASP.NET MVC Application](https://weblog.west-wind.com/posts/2015/Apr/29/Adding-minimal-OWIN-Identity-Authentication-to-an-Existing-ASPNET-MVC-Application)
- [Keeping Content Out of the Publish Folder for WebDeploy](https://weblog.west-wind.com/posts/2022/Aug/24/Keeping-Content-Out-of-the-Publish-Folder-for-WebDeploy)
- [Using SQL Server on Windows ARM](https://weblog.west-wind.com/posts/2024/Oct/24/Using-Sql-Server-on-Windows-ARM)
- [Map Physical Paths with an HttpContext.MapPath() Extension Method in ASP.NET](https://weblog.west-wind.com/posts/2023/Aug/15/Map-Physical-Paths-with-an-HttpContextMapPath-Extension-Method-in-ASPNET)

---

*Original post by Rick Strahl. If you found this content helpful, consider making a small donation.*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Mar/13/Accessing-Windows-Settings-Dialogs-from-Code-via-Shell-Commands)
