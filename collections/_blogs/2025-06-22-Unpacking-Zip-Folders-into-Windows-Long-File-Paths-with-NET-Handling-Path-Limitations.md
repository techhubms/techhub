---
external_url: https://weblog.west-wind.com/posts/2025/Jun/22/Unpacking-Zip-Folders-into-Windows-Long-File-Paths
title: 'Unpacking Zip Folders into Windows Long File Paths with .NET: Handling Path Limitations'
author: Rick Strahl
feed_name: Rick Strahl's Blog
date: 2025-06-22 11:00:48 +00:00
tags:
- .NET
- Application Manifest
- C#
- ExtractToDirectory
- File System
- Group Policy
- Long Paths
- MAX PATH
- Path Length
- Unpacking
- Utility Development
- Windows
- Windows Registry
- ZipFile
section_names:
- coding
---
In this detailed blog post, Rick Strahl examines .NET's handling of long Windows file paths during zip extraction. He discusses the limitations of ZipFile.ExtractToDirectory, reviews Windows long path support options, and presents alternative coding approaches to overcome path length restrictions in C# projects.<!--excerpt_end-->

# Unpacking Zip Folders into Windows Long File Paths with .NET

**Author:** Rick Strahl

![Zip Archive Banner](https://weblog.west-wind.com/images/2025/Unpacking-Zip-Folders-into-Windows-Long-File-Paths/ZipArchiveBanner.jpg)

## Introduction

Long file paths on Windows have been a notorious challenge for developers, especially when working with .NET and file system operations. In this article, Rick Strahl demonstrates a real-world issue with using the classic `\\?\` long path prefix in combination with .NET’s `ZipFile.ExtractToDirectory()` method. The author shares troubleshooting insights, a review of the causes, and practical workarounds for handling long paths when unpacking archive files.

---

## The Problem: Failing with Long Paths and .NET ZipFile

If you attempt to extract a zip file whose entries will create file paths longer than the default `MAX_PATH` limit (260 characters) using code like:

```csharp
// THIS DOES NOT WORK
outputFolder = @"\\?\" + outputFolder;
ZipFile.ExtractToDirectory(zipFile, outputFolder);
```

you will quickly hit a wall: the function throws an exception when it encounters a very long path, leaving you with a **partially unpacked directory**. The error isn’t immediate; it only occurs once an entry violating the maximum path constraint is hit.

![Unzipping With Long Path Syntax Fails](https://weblog.west-wind.com/images/2025/Unpacking-Zip-Folders-into-Windows-Long-File-Paths/UnzippingWithLongPathSyntaxFails.png)

### Key takeaway

- `\\?\` long path syntax **does not work** with `ZipFile.ExtractToDirectory()`.
- You must handle exceptions and clean up leftover files if the extraction fails.

---

## Reviewing Long File Path Options in Windows and .NET

There are two main approaches for handling long paths on Windows:

1. **Enabling Long Path Support Globally and for Your Application**
   - Use a global Windows registry or group policy setting.
   - Add an application manifest entry to enable long paths in your .NET app.
   - For details and how-to, see: [Integrating Long Path Names in Windows Applications](https://weblog.west-wind.com/posts/2022/Jan/03/Integrating-Long-Path-Names-in-Windows-Applications)
2. **Using `\\?\` Syntax (Long Path Prefixing)**
   - Prefix file paths with `\\?\` (local) or `\\?\UNC\` (network shares).
   - Only works for fully qualified or UNC paths; not relative paths.

#### Example: Registry and Manifest Entries

**Registry Setting:**

```reg
Windows Registry Editor Version 5.00
[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem]
"LongPathsEnabled"=dword:00000001
```

**Group Policy:**

- Go to `Computer Configuration -> Administrative Templates -> System -> Enable Win32 Long Paths`.

**Application Manifest Snippet:**

```xml
<windowsSettings xmlns:ws2="http://schemas.microsoft.com/SMI/2016/WindowsSettings">
  <ws2:longPathAware>true</ws2:longPathAware>
</windowsSettings>
```

#### Challenge

Enabling global long path support **requires admin privilege** and isn’t possible directly from a regular application. Most users and small utilities simply can’t assume these preconditions will be met.

---

## Path Prefix Solution and Its Limitations

To sidestep system-wide settings, you may add the `\\?\` or `\\?\UNC\` prefix to fully-resolved file paths. However:

- All paths must be absolute, not relative.
- Some .NET APIs and external tools (like the built-in Zip handling code) may not respect the prefix.
- It complicates file access logic if your application manipulates many files or paths.

**Helper Function Example:**
Rick demonstrates adding a utility to generate long path-aware paths:

```csharp
public static string GetWindowsLongFilename(string path) {
    if (string.IsNullOrEmpty(path)) return path;
    string fullPath = System.IO.Path.GetFullPath(path);
    if (fullPath.Length < 260) return fullPath; // No need to convert
    // Fully qualified path
    if (fullPath.Length > 1 && fullPath[1] == ':')
        fullPath = @"\\?\" + fullPath;
    // UNC Path
    else if (fullPath.Length > 2 && fullPath.StartsWith(@"\\"))
        fullPath = @"\\?\UNC\" + fullPath.Substring(2);
    return fullPath;
}
```

---

## Custom Workaround: Manually Extracting Zip Files

Since the high-level `ExtractToDirectory` API fails, Rick suggests extracting entries one by one, applying the long path workaround per file.

**Sample Extraction Routine:**

```csharp
public bool ExtractZipFileToFolder(string zipFile, string outputFolder = null) {
    if (string.IsNullOrEmpty(outputFolder)) {
        SetError("Output folder is not specified.");
        return false;
    }
    zipFile = FileUtils.GetWindowsLongFilename(zipFile);
    using (var archive = ZipFile.OpenRead(zipFile)) {
        foreach (var entry in archive.Entries) {
            string targetPath = Path.Combine(outputFolder, entry.FullName);
            targetPath = targetPath.Replace('/', '\\'); // Windows separator
            targetPath = Path.GetFullPath(targetPath);
            var fullPath = FileUtils.GetWindowsLongFilename(targetPath);
            if (entry.FullName.EndsWith("/"))
                Directory.CreateDirectory(fullPath);
            else {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                entry.ExtractToFile(fullPath, overwrite: true);
            }
        }
    }
    return true;
}
```

This approach allows even deeply nested files and paths to be unpacked, even if the global long path policy is not enabled.

---

## Considerations and Real-world Insights

- **Avoiding long file paths is ideal, but can be difficult.** User profile nesting and application structures quickly push you past 260 characters, especially for user-generated or deeply nested structures (such as npm dependencies, documentation trees).
- **Minimizing path length is a partial fix:** Users can create deeply nested folders or long file/topical names you can’t control.

Example temp path:

```text
c:\users\MyUserName\Temp\tmpfiles\
```

- **Just because your app writes files doesn’t mean every tool can read them** unless they’re also long-path-aware.
- **Self-contained apps (like a bundled web server and viewer) are easier to manage** because both read and write with the same path logic.
- **Third-party controls (e.g., WebView2, Chromium) may support long paths**—test your setup.

---

## Example: Unpacking An Embedded Zip from an EXE

The final tool described unpacks an EXE, extracts an embedded zip, handles long paths, and cleans up as needed. Key approach:

- Verify file/folder existence.
- Extract EXE/ZIP content by offsets appropriately.
- Extract the zipped content to a folder using the manual method with long path handling per file.
- Clean up broken extractions on errors.

---

## Summary

Windows still does not enable long path support by default, which causes headaches for developers working with files and directories beyond 260 characters in path length. While system-wide enabling of long paths is possible, it’s not always under the developer’s or user’s control. As shown, handling long paths in .NET often requires low-level workaround logic, particularly for zip extraction.

Rick Strahl’s article breaks down the problems encountered, reviews enabling strategies, and suggests reusable code to work around these platform constraints in C# and .NET environments.

---

## References & Further Reading

- [ZipFile class (Microsoft Docs)](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.zipfile?view=net-9.0)
- [Integrating Long Path Names in Windows Applications](https://weblog.west-wind.com/posts/2022/Jan/03/Integrating-Long-Path-Names-in-Windows-Applications)

© Rick Strahl, West Wind Technologies, 2005–2025

---

Posted in **.NET, Windows**

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Jun/22/Unpacking-Zip-Folders-into-Windows-Long-File-Paths)
