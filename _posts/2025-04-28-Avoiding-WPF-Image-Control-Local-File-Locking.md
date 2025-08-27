---
layout: "post"
title: "Avoiding WPF Image Control Local File Locking"
description: "Rick Strahl explores the problem of WPF's Image control locking local files and presents solutions using XAML techniques and a custom binding converter with built-in image caching, addressing performance and file update scenarios for desktop .NET applications."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/Apr/28/WPF-Image-Control-Local-File-Locking"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2025-04-28 22:01:46 +00:00
permalink: "/2025-04-28-Avoiding-WPF-Image-Control-Local-File-Locking.html"
categories: ["Coding"]
tags: [".NET", "Binding Converter", "BitmapImage", "Coding", "Desktop Applications", "File Locking", "Image Caching", "Image Control", "Posts", "TreeView", "Windows", "WPF", "WPF .NET Windows", "XAML"]
tags_normalized: ["dotnet", "binding converter", "bitmapimage", "coding", "desktop applications", "file locking", "image caching", "image control", "posts", "treeview", "windows", "wpf", "wpf dotnet windows", "xaml"]
---

In this technical post, Rick Strahl discusses how WPF’s Image control locks local image files, the issues it causes, and presents XAML and custom binding converter strategies—including image caching—to solve the problem efficiently for .NET Windows applications.<!--excerpt_end-->

# Avoiding WPF Image Control Local File Locking

*By Rick Strahl*

WPF applications often use images directly from disk, but the WPF Image control has an inconvenient side-effect: it locks the image file for as long as it’s displayed when you assign the image through the Source property. This can be problematic in scenarios where images are updated or replaced at runtime, especially in applications like file or template managers where users may update resources dynamically.

## The Problem: Locked Image Files

When you bind or assign an image file to a WPF Image control’s Source property, WPF keeps a file stream open internally. This typically isn’t an issue if your images are static and rarely updated. However, when frequent updates are involved—for example, applications that display user-updated icons in a TreeView—locked files can quickly become an obstacle. Locked files can’t be deleted, nor can their parent directories be removed, creating problems with update operations that involve replacing entire directories or refreshing template assets.

> "In several of my applications I have **tons of icons** that get loaded into a TreeView control...The locked files become a problem as these locked files can't be deleted."

## Native XAML Solution: BitmapSource with CacheOption

WPF offers a workaround using XAML by binding to a BitmapSource and setting its caching behavior:

```xml
<Image Height="16">
  <Image.Source>
    <BitmapSource UriSource="{Binding TopicState.DisplayTypeIconFile}" CacheOption="OnLoad" />
  </Image.Source>
</Image>
```

Setting `CacheOption="OnLoad"` ensures the whole image is loaded into memory immediately, and the file stream is closed, mitigating the lock. If the image must be refreshed on re-binding (such as after being updated on disk), you can add `CreateOptions="IgnoreImageCache"` to force reloads. Note that this can degrade performance if overused.

```xml
<BitmapSource UriSource="{Binding TopicState.DisplayTypeIconFile}" CacheOption="OnLoad" CreateOptions="IgnoreImageCache" />
```

## A General Approach: Using a Binding Converter

To make this process less verbose and more manageable across larger applications, you can implement a binding converter. The converter loads images into memory and, optionally, manages a cache of loaded images for performance with repeated image use (such as in tree views with many repeating icons):

### Sample XAML with Converter

```xml
<UserControl.Resources>
  <windows:LocalFileImageConverter x:Key="LocalFileImageConverter"/>
</UserControl.Resources>
...
<Image Height="16" Source="{Binding TopicState.OpenImageFilename, Converter={StaticResource LocalFileImageConverter}}" />
```

### The Converter Implementation

The converter uses a static dictionary for caching, loads files into memory streams with the proper cache options, freezes the BitmapImage for efficiency, and handles missing files by returning a default image. Clearing the cache allows for explicit refreshes of all or individual images.

```csharp
public class LocalFileImageConverter : IValueConverter
{
    public static Dictionary<string, BitmapImage> CachedBitmapImages = new Dictionary<string, BitmapImage>();

    public static void ClearCachedImages() {
        CachedBitmapImages = new Dictionary<string, BitmapImage>();
    }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        string val = value as string;
        if (!string.IsNullOrEmpty(val)) {
            val = val.ToLower();
            if (CachedBitmapImages.TryGetValue(val, out BitmapImage bi))
                return bi;
            try {
                using (var fstream = new FileStream(value.ToString(), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = fstream;
                    bi.StreamSource.Flush();
                    bi.EndInit();
                    bi.Freeze();
                }
                CachedBitmapImages.Add(val, bi);
                return bi;
            }
            catch {
                // Handle or log
            }
            return AssociatedIcons.DefaultIcon;
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException("LocalFileImageConverter: Two way conversion is not supported.");
    }
}
```

#### Key Implementation Details

- **File Loading/Locking:** The use of `CacheOption.OnLoad` ensures the file stream is closed after loading.
- **Flush and Freeze:** Flushing the stream and freezing the BitmapImage improves memory efficiency and ensures bindings are not kept alive in memory.
- **Caching:** A dictionary allows for quick repeated access to the same images. Clearing the cache allows for images to be refreshed from disk.

### When to Use Each Approach

- For simple cases or when only a few images are involved, the native XAML approach suffices.
- For applications with many dynamic images or performance concerns, a converter approach provides a scalable and maintainable solution with centralized logic and caching benefits.

## Summary

Using WPF’s direct file binding for images introduces file locking. Handling dynamic or mutable image content, as is common for applications with user-managed icons or assets, requires additional techniques: either verbose but effective XAML options, or a more universal converter-based approach that centralizes image management and prevents file locking while allowing for image caching and explicit refreshes. Rick Strahl’s methods are practical strategies for robust WPF application development where reusable, updatable images are essential.

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Apr/28/WPF-Image-Control-Local-File-Locking)
