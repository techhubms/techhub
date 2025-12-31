---
layout: "post"
title: "Integrating Windows.Media SpeechRecognition in WPF Applications Using .NET"
description: "This blog post by Rick Strahl explores how to integrate the Windows.Media SpeechRecognition engine into WPF applications using .NET and the Windows SDK. It covers setup, implementation strategies, handling dependencies, pitfalls, and practical code examples, especially around managing WinSDK/WinRT complexities."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/Mar/24/Using-WindowsMedia-SpeechRecognition-in-WPF"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2025-03-25 04:33:00 +00:00
permalink: "/blogs/2025-03-25-Integrating-WindowsMedia-SpeechRecognition-in-WPF-Applications-Using-NET.html"
categories: ["Coding"]
tags: [".NET", "Asynchronous Programming", "Coding", "Dependency Management", "Input Handling", "Language Support", "Markdown Monster", "NuGet", "Blogs", "Reflection", "Speech Recognition", "Windows", "Windows SDK", "Windows.media.speechrecognition", "Winrt", "Winsdk", "WPF"]
tags_normalized: ["dotnet", "asynchronous programming", "coding", "dependency management", "input handling", "language support", "markdown monster", "nuget", "blogs", "reflection", "speech recognition", "windows", "windows sdk", "windowsdotmediadotspeechrecognition", "winrt", "winsdk", "wpf"]
---

Rick Strahl discusses the practical integration of Windows.Media SpeechRecognition within WPF applications using .NET. The post highlights code samples, SDK dependencies, workarounds for WinRT/.NET issues, and language management.<!--excerpt_end-->

# Integrating Windows.Media SpeechRecognition in WPF Applications Using .NET

**By Rick Strahl**

---

Windows includes a built-in SpeechRecognition engine available through Windows Media services. In .NET, you can access these features via the Windows SDK (WinSdk), exposing them to .NET applications. This article details the integration of these speech capabilities into a WPF application, highlights the necessary dependencies, discusses challenges around SDK integration, and offers practical solutions.

## Introduction

The Windows.Media SpeechRecognition features replace the older System.Speech functionality, providing a much-improved recognition engine with a familiar but more capable API. This guide focuses on:

- Integrating Windows Media SpeechRecognition in WPF
- Building a simple wrapper class for speech dictation
- Handling the required SDK and WinRT dependencies
- Dealing with SDK/WinRT integration quirks
- Working through relevant pitfalls and workarounds

## Required Dependencies

Because the SpeechRecognizer class is part of the Windows SDK and originated from the WinRT/UWP development period, you need two NuGet packages:

```xml
<PackageReference Include="Microsoft.Windows.SDK.Contracts" Version="10.0.22621.2" />
<PackageReference Include="Microsoft.Windows.CsWinRt" Version="*" />
```

- **SDK.Contracts** must match your intended minimum Windows version (10.0.22621 covers all Win10 and Win11 versions commonly).
- Ensure the matching Windows SDK is installed on your development machine, typically via Visual Studio Installer > Individual Components or Windows Desktop Development.

## Creating a VoiceDictation Class

This reusable class handles:

- Starting listening for dictation
- Stopping the listener
- Shutting down on idle
- Handling and fixing up dictation results

Typical usage involves wiring commands to hotkeys or menu options:

```csharp
// Initialization
if (mmApp.Configuration.EnableVoiceDictation)
    VoiceDictation = new VoiceDictation();

// Command example
public CommandBase StartListeningCommand { get; set; }
void Command_StartListening() {
    StartListeningCommand = new CommandBase((parameter, command) => {
        Model.Window.VoiceDictation?.StartAsync().FireAndForget();
    }, (p, c) => true);
}

public CommandBase StopListeningCommand { get; set; }
void Command_StopListening() {
    StopListeningCommand = new CommandBase((parameter, command) => {
        Model.Window.VoiceDictation?.Stop();
    }, (p, c) => true);
}
```

## VoiceDictation Class: Key Implementation Details

- **Singleton Engine**: You should have only one SpeechRecognizer instance at any time due to resource constraints and async tracking. Initialization isn't fast, and overlapping engines can cause instability.
- **Language**: The engine uses the current system UI language by default but can be set via an IETF language code (e.g., `en-US`, `de-DE`).
- **Event Hookup**: Crucial events include `ResultGenerated` (for generated text results) and `Completed` (for idle/timeouts/end of session).

Sample snippet:

```csharp
public class VoiceDictation {
    private SpeechRecognizer _recognizer;
    private bool _isCompiled;
    private bool _isDisposed;
    public bool IsDictating { get; private set; }

    public VoiceDictation() {
        // Language selection
        if (string.IsNullOrEmpty(mmApp.Configuration.VoiceDictationLanguage))
            _recognizer = new SpeechRecognizer();
        else
            _recognizer = new SpeechRecognizer(new Language(mmApp.Configuration.VoiceDictationLanguage));
        _recognizer.Constraints.Add(dictation);
        _recognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
        _recognizer.ContinuousRecognitionSession.AutoStopSilenceTimeout = TimeSpan.FromMinutes(1);
        _recognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
        Keyboard.AddKeyDownHandler(mmApp.Window, KeydownHandler);
    }
    // ...
}
```

**Commands**: Example hotkeys include F4 to start and ESC to stop dictation. Language changes require recreating the engine.

## Starting and Stopping Dictation

- **CompileConstraintsAsync**: The recognizer must be compiled (async op) before listening.
- **Session Start/Stop**: All major ops (start, stop, compile) on SpeechRecognizer are async, but WinRT uses IAsync patterns incompatible with .NET Tasks. Reflection-based `.AsTask()` wrappers are used as a workaround.

```csharp
public async Task StartAsync(DictationListenModes listenMode = DictationListenModes.EscPressed) {
    if (IsDictating) return;
    try {
        await EnsureCompiledAsync();
        var action = _recognizer.ContinuousRecognitionSession.StartAsync();
        await AsTask(action);
        IsDictating = true;
        // Show progress/status
    } catch (Exception ex) when (ex.Message.Contains("privacy")) {
        // Open privacy settings
        ShellUtils.GoUrl("ms-settings:privacy-speech");
    }
}
```

**Speech Features**: Windows Speech must be enabled, or the async start will fail. You can open relevant config dialogs programmatically via `Process.Start("ms-settings:privacy-speech")`.

## Handling Asynchronous Pattern and Dueling References

- WinRT methods return `IAsyncAction` and `IAsyncOperation<>`, not `Task`.
- The `.AsTask()` extension isn't available due to ambiguous/duplicated type signatures between SDK and WinRT. Reflection is used to access these at runtime.

Custom Reflection-based workaround (simplified):

```csharp
Task AsTask(object action) { /* ...Reflection logic... */ }
Task AsTask<T>(object action) { /* ...Generic version with reflection... */ }
```

This is a cautionary tale—if you need to await these ops in standard .NET, Reflection may be unavoidable.

## Capturing and Processing Recognized Speech

- **ResultGenerated Event**: Receives dictated text and inserts it into the editor or UI control.
- **UI Thread**: Use Dispatcher to access UI elements safely.
- **Text Fix-Up**: Punctuation, commands (like "stop recording"), and spacing are handled via custom methods to improve context and accuracy.

```csharp
private async void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args) {
    if (args.Result.Status != SpeechRecognitionResultStatus.Success || !mmApp.Configuration.EnableVoiceDictation) return;
    var text = args.Result?.Text;
    if (string.IsNullOrEmpty(text)) return;
    await mmApp.Window.Dispatcher.InvokeAsync(async () => {
        var ctrl = Keyboard.FocusedElement;
        if (ctrl is TextBox tb) {
            // Insert text logic here
        }
        // Editor logic, fix-up, etc.
    });
}
```

## Language Keywords and Switching

- **Keyword Issues**: Command words like "space" and "return" are checked in English, but may fail in other languages, requiring further handling.
- **Language Switching**: Only possible by recreating the speech engine with the desired language code.

## Deployment and SDK/WinRT Issues

- **Large Dependency Size**: Using the SDK and WinRT adds ~30MB to deployables.
- **Negotiating Ambiguous Types**: Overlapping APIs result in complicated workarounds, mainly through Reflection.

## Alternative: Win-H Shortcut

Windows provides a native speech-to-text overlay (`Win+H`) for basic dictation. It's less integrated but doesn't require code.

## Summary

Windows.Media SpeechRecognition delivers robust dictation capabilities compared to the legacy System.Speech engine. Despite some integration challenges (large dependencies, API overlap, and async integration issues), it's a practical option for advanced voice features in desktop applications. Direct programmatic integration delivers a much better user experience than global dictation shortcuts.

## Resources

- [Complete VoiceDictation Class Code (gist)](https://gist.github.com/RickStrahl/9b250c8bff67edd26b79e614b16955eb)
- [ms-settings URI Scheme Reference](https://learn.microsoft.com/en-us/windows/apps/develop/launch/launch-settings-app#ms-settings-uri-scheme-reference)

---

**Related Posts:**

- [Adding minimal OWIN Identity Authentication to an Existing ASP.NET MVC Application](https://weblog.west-wind.com/posts/2015/Apr/29/Adding-minimal-OWIN-Identity-Authentication-to-an-Existing-ASPNET-MVC-Application)
- [Using SQL Server on Windows ARM](https://weblog.west-wind.com/posts/2024/Oct/24/Using-Sql-Server-on-Windows-ARM)

*If you found this content useful, consider making a small donation to support further work.*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Mar/24/Using-WindowsMedia-SpeechRecognition-in-WPF)
