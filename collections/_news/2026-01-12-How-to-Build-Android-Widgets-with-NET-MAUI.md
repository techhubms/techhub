---
external_url: https://devblogs.microsoft.com/dotnet/how-to-build-android-widgets-with-dotnet-maui/
title: How to Build Android Widgets with .NET MAUI
author: Toine de Boer
feed_name: Microsoft .NET Blog
date: 2026-01-12 18:05:00 +00:00
tags:
- .NET
- .NET For Android
- Android
- Android Studio
- AppWidgetProvider
- BroadcastReceiver
- C#
- Cross Platform Development
- Intent
- MAUI
- Mobile Development
- PendingIntent
- Preferences
- RemoteViews
- SharedPreferences
- VS
- Widget Configuration
- Widgets
- News
section_names:
- dotnet
primary_section: dotnet
---
Toine de Boer explains how to create interactive Android widgets using .NET MAUI, sharing detailed techniques and code examples for building, configuring, and communicating with widgets from your app.<!--excerpt_end-->

# How to Build Android Widgets with .NET MAUI

**Author:** Toine de Boer  

This guide explores how to add interactive Android widgets to your .NET MAUI applications, leveraging native Android capabilities directly in your C# codebase. You'll learn about widget architecture, data sharing between app and widget, communication via Intents, widget interactivity, configuration activities, and performance considerations.

## Prerequisites

- No special MAUI components for Android widgets exist, but you can use the native Android APIs via C#.
- Visual Studio suffices for coding, though Android Studio can be used for XML layouts.

## Integrating Widgets in a .NET MAUI Project

- Organize Android-specific resources in `./Platforms/Android` for code and `./Platforms/Resources` for assets.
- There is no need to touch your `.csproj` file; resource structure mimics a native Android app.

## Widget Basics

- Android widgets are implemented via `AppWidgetProvider`, which provides `RemoteViews` based on widget IDs.
- The widget's UI uses XML layouts (placed under `Resources/layout`).
- The provider is registered via meta-data in `Resources/xml/mywidget_provider_info.xml`.

**Sample AppWidgetProvider (C#):**

```csharp
[BroadcastReceiver(Label = "My Widget")]
[MetaData(AppWidgetManager.MetaDataAppwidgetProvider, Resource = "@xml/mywidget_provider_info")]
public class MyWidgetProvider : AppWidgetProvider {
    public override void OnUpdate(Context? context, AppWidgetManager? appWidgetManager, int[]? appWidgetIds) {
        if (context == null || appWidgetIds == null || appWidgetManager == null) {
            return;
        }
        foreach (var appWidgetId in appWidgetIds) {
            var views = new RemoteViews(context.PackageName, Resource.Layout.mywidget);
            views.SetTextViewText(Resource.Id.widgetText, "Count:5 (static)");
            appWidgetManager.UpdateAppWidget(appWidgetId, views);
        }
    }
}
```

## Widget Configuration XML

Example `mywidget_provider_info.xml`:

```xml
<appwidget-provider
  xmlns:android="http://schemas.android.com/apk/res/android"
  android:minWidth="120dp"
  android:minHeight="80dp"
  android:maxResizeWidth="140dp"
  android:updatePeriodMillis="0"
  android:initialLayout="@layout/mywidget"
  android:resizeMode="horizontal|vertical"
  android:widgetCategory="home_screen"
  android:configure="widgetexample.WidgetConfigurationActivity"
  android:widgetFeatures="reconfigurable"
  android:previewImage="@drawable/mywidget_preview_image" />
```

## Data Sharing Between App and Widget

- Use `.NET MAUI`'s `Preferences` for persistent data across app and widget.

  ```csharp
  Preferences.Set("MyDataKey", "my data to share", "group.com.enbyin.WidgetExample");
  ```

- Widgets can access shared values using Android's `SharedPreferences`.

## Updating Widgets from the App

- Send an `Intent` with action `ActionAppwidgetUpdate` to trigger refreshes.

  ```csharp
  var intent = new Android.Content.Intent(AppWidgetManager.ActionAppwidgetUpdate);
  Android.App.Application.Context.SendBroadcast(intent);
  ```

- Subscribe to intents using `[IntentFilter]` on your provider class.

## Scheduling and Manual Updates

- Use `updatePeriodMillis` in your XML for periodic updates (minimum 30 minutes).
- Use `AlarmManager` or `WorkManager` for more flexible scheduling.

## Interactivity with PendingIntents

- Interactivity (like button clicks) uses `PendingIntents` in the widget.

  ```csharp
  var incrementIntent = new Intent(context, typeof(MyWidgetProvider));
  incrementIntent.SetAction("com.enbyin.WidgetExample.INCREMENT_COUNTER");
  var incrementPendingIntent = PendingIntent.GetBroadcast(
      context, 101, incrementIntent, PendingIntentFlags.UpdateCurrent |
      (Build.VERSION.SdkInt >= BuildVersionCodes.S ? PendingIntentFlags.Mutable : 0));
  views.SetOnClickPendingIntent(Resource.Id.widgetIncrementButton, incrementPendingIntent);
  ```

- Handle actions in `OnReceive` of your provider by detecting the intent's action string.

## Communication from Widget to App

- Widgets launch the main app using deep links with `PendingIntent.GetActivity()` and attach structured data.

## Widget Configuration Activity

- Specify a configuration `Activity` in your XML using `android:configure`.
- Build your UI using standard Android views, then store changes and finish the activity.

## Using Android Context

- Always use the correct context (from provider/service/activity) to avoid runtime crashes.

## Performance Tips

- Minimize initialization work for widgets; you can use a light-weight variant of your MAUI app for widget contexts.

## Final Thoughts

- Widgets in .NET MAUI on Android allow deep integration with standard Android APIs and C# code.
- Test thoroughly across a range of devices and OS versions.
- Keep layouts simple and optimize for reliability and performance.

This article provides practical techniques and code for integrating Android widgets using .NET MAUI, offering a cross-platform approach for developers building advanced user experiences.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/how-to-build-android-widgets-with-dotnet-maui/)
