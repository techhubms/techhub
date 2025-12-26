---
layout: "post"
title: "Finding Large Directories and Recovering Lost Disk Space with PowerShell"
description: "This practical guide by LainRobertson demonstrates how to identify large directories and analyze disk space usage using a custom PowerShell module. The post explains the reasoning behind the approach, shares complete code examples, and walks through real-world use cases for filesystem analysis and troubleshooting in Windows environments."
author: "LainRobertson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-13 12:30:08 +00:00
permalink: "/community/2025-08-13-Finding-Large-Directories-and-Recovering-Lost-Disk-Space-with-PowerShell.html"
categories: ["Coding"]
tags: [".NET", "Automation", "Coding", "Community", "Directory Size", "Disk Space Management", "Filesystem Analysis", "Get DirectorySize", "Get DirectoryTreeSize", "Module Development", "PowerShell", "Reparse Points", "Scripting", "System.IO.Directory", "Windows"]
tags_normalized: ["dotnet", "automation", "coding", "community", "directory size", "disk space management", "filesystem analysis", "get directorysize", "get directorytreesize", "module development", "powershell", "reparse points", "scripting", "systemdotiodotdirectory", "windows"]
---

LainRobertson shares a PowerShell module and examples for analyzing large directories and recovering lost disk space, offering practical insights for Windows administrators and developers.<!--excerpt_end-->

# Finding Large Directories and Recovering Lost Disk Space with PowerShell

Author: LainRobertson

## Overview

When disk space mysteriously vanishes on Windows, GUI tools exist to help—but for automation and repeatable troubleshooting, PowerShell shines. This guide walks through using a simple PowerShell module to determine which directories are consuming the most space, leveraging .NET classes for efficiency and resilience.

## XTree.psm1 PowerShell Module

**Key Functions:**

- `Get-DirectorySize`: Calculates the number and total size of files in a single directory.
- `Get-DirectoryTreeSize`: Recursively assesses directories, skipping reparse points to avoid double-counting or misattribution of files.

### Module Code

```powershell
function Get-DirectorySize {
    [cmdletbinding()]
    param(
        [parameter(Mandatory = $true)][ValidateNotNull()][string] $Path
    )

    Write-Verbose -Message "Parsing $Path";
    $Summary = [PSCustomObject] @{ Path = $Path.ToLowerInvariant(); Count = 0; Size = 0; }
    [System.IO.Directory]::EnumerateFiles($Path) | ForEach-Object {
        [System.IO.FileInfo]::new(---
layout: "post"
title: "Finding Large Directories and Recovering Lost Disk Space with PowerShell"
description: "This practical guide by LainRobertson demonstrates how to identify large directories and analyze disk space usage using a custom PowerShell module. The post explains the reasoning behind the approach, shares complete code examples, and walks through real-world use cases for filesystem analysis and troubleshooting in Windows environments."
author: "LainRobertson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-13 12:30:08 +00:00
permalink: "2025-08-13-Finding-Large-Directories-and-Recovering-Lost-Disk-Space-with-PowerShell.html"
categories: ["Coding"]
tags: [".NET", "Automation", "Coding", "Community", "Directory Size", "Disk Space Management", "Filesystem Analysis", "Get DirectorySize", "Get DirectoryTreeSize", "Module Development", "PowerShell", "Reparse Points", "Scripting", "System.IO.Directory", "Windows"]
tags_normalized: [["net", "automation", "coding", "community", "directory size", "disk space management", "filesystem analysis", "get directorysize", "get directorytreesize", "module development", "powershell", "reparse points", "scripting", "system dot io dot directory", "windows"]]
---

LainRobertson shares a PowerShell module and examples for analyzing large directories and recovering lost disk space, offering practical insights for Windows administrators and developers.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
) | ForEach-Object {
            $Summary.Count++;
            $Summary.Size += ---
layout: "post"
title: "Finding Large Directories and Recovering Lost Disk Space with PowerShell"
description: "This practical guide by LainRobertson demonstrates how to identify large directories and analyze disk space usage using a custom PowerShell module. The post explains the reasoning behind the approach, shares complete code examples, and walks through real-world use cases for filesystem analysis and troubleshooting in Windows environments."
author: "LainRobertson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-13 12:30:08 +00:00
permalink: "2025-08-13-Finding-Large-Directories-and-Recovering-Lost-Disk-Space-with-PowerShell.html"
categories: ["Coding"]
tags: [".NET", "Automation", "Coding", "Community", "Directory Size", "Disk Space Management", "Filesystem Analysis", "Get DirectorySize", "Get DirectoryTreeSize", "Module Development", "PowerShell", "Reparse Points", "Scripting", "System.IO.Directory", "Windows"]
tags_normalized: [["net", "automation", "coding", "community", "directory size", "disk space management", "filesystem analysis", "get directorysize", "get directorytreesize", "module development", "powershell", "reparse points", "scripting", "system dot io dot directory", "windows"]]
---

LainRobertson shares a PowerShell module and examples for analyzing large directories and recovering lost disk space, offering practical insights for Windows administrators and developers.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
.Length;
        }
    }
    $Summary;
}

function Get-DirectoryTreeSize {
    [cmdletbinding()]
    param(
        [parameter(Mandatory = $true)][ValidateNotNull()][string] $Path
    )

    # Reference: https://learn.microsoft.com/en-us/dotnet/api/system.io.fileattributes?view=netframework-4.8.1
    New-Variable -Name "ReparsePoint" -Value ([System.IO.FileAttributes]::ReparsePoint.value__) -Option Constant;
    $Summary = [PSCustomObject] @{ Path = $Path.ToLowerInvariant(); Count = 0; Size = 0; TotalCount = 0; TotalSize = 0; }
    [System.IO.Directory]::EnumerateDirectories($Path) | ForEach-Object {
        # Avoid processing reparse points.
        if (0 -eq (([System.IO.DirectoryInfo]::new(---
layout: "post"
title: "Finding Large Directories and Recovering Lost Disk Space with PowerShell"
description: "This practical guide by LainRobertson demonstrates how to identify large directories and analyze disk space usage using a custom PowerShell module. The post explains the reasoning behind the approach, shares complete code examples, and walks through real-world use cases for filesystem analysis and troubleshooting in Windows environments."
author: "LainRobertson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-13 12:30:08 +00:00
permalink: "2025-08-13-Finding-Large-Directories-and-Recovering-Lost-Disk-Space-with-PowerShell.html"
categories: ["Coding"]
tags: [".NET", "Automation", "Coding", "Community", "Directory Size", "Disk Space Management", "Filesystem Analysis", "Get DirectorySize", "Get DirectoryTreeSize", "Module Development", "PowerShell", "Reparse Points", "Scripting", "System.IO.Directory", "Windows"]
tags_normalized: [["net", "automation", "coding", "community", "directory size", "disk space management", "filesystem analysis", "get directorysize", "get directorytreesize", "module development", "powershell", "reparse points", "scripting", "system dot io dot directory", "windows"]]
---

LainRobertson shares a PowerShell module and examples for analyzing large directories and recovering lost disk space, offering practical insights for Windows administrators and developers.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
).Attributes.value__ -band $ReparsePoint))) {
            Get-DirectoryTreeSize -Path ---
layout: "post"
title: "Finding Large Directories and Recovering Lost Disk Space with PowerShell"
description: "This practical guide by LainRobertson demonstrates how to identify large directories and analyze disk space usage using a custom PowerShell module. The post explains the reasoning behind the approach, shares complete code examples, and walks through real-world use cases for filesystem analysis and troubleshooting in Windows environments."
author: "LainRobertson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-13 12:30:08 +00:00
permalink: "2025-08-13-Finding-Large-Directories-and-Recovering-Lost-Disk-Space-with-PowerShell.html"
categories: ["Coding"]
tags: [".NET", "Automation", "Coding", "Community", "Directory Size", "Disk Space Management", "Filesystem Analysis", "Get DirectorySize", "Get DirectoryTreeSize", "Module Development", "PowerShell", "Reparse Points", "Scripting", "System.IO.Directory", "Windows"]
tags_normalized: [["net", "automation", "coding", "community", "directory size", "disk space management", "filesystem analysis", "get directorysize", "get directorytreesize", "module development", "powershell", "reparse points", "scripting", "system dot io dot directory", "windows"]]
---

LainRobertson shares a PowerShell module and examples for analyzing large directories and recovering lost disk space, offering practical insights for Windows administrators and developers.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
 | ForEach-Object {
                $Summary.TotalCount += ---
layout: "post"
title: "Finding Large Directories and Recovering Lost Disk Space with PowerShell"
description: "This practical guide by LainRobertson demonstrates how to identify large directories and analyze disk space usage using a custom PowerShell module. The post explains the reasoning behind the approach, shares complete code examples, and walks through real-world use cases for filesystem analysis and troubleshooting in Windows environments."
author: "LainRobertson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-13 12:30:08 +00:00
permalink: "2025-08-13-Finding-Large-Directories-and-Recovering-Lost-Disk-Space-with-PowerShell.html"
categories: ["Coding"]
tags: [".NET", "Automation", "Coding", "Community", "Directory Size", "Disk Space Management", "Filesystem Analysis", "Get DirectorySize", "Get DirectoryTreeSize", "Module Development", "PowerShell", "Reparse Points", "Scripting", "System.IO.Directory", "Windows"]
tags_normalized: [["net", "automation", "coding", "community", "directory size", "disk space management", "filesystem analysis", "get directorysize", "get directorytreesize", "module development", "powershell", "reparse points", "scripting", "system dot io dot directory", "windows"]]
---

LainRobertson shares a PowerShell module and examples for analyzing large directories and recovering lost disk space, offering practical insights for Windows administrators and developers.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
.Count;
                $Summary.TotalSize += ---
layout: "post"
title: "Finding Large Directories and Recovering Lost Disk Space with PowerShell"
description: "This practical guide by LainRobertson demonstrates how to identify large directories and analyze disk space usage using a custom PowerShell module. The post explains the reasoning behind the approach, shares complete code examples, and walks through real-world use cases for filesystem analysis and troubleshooting in Windows environments."
author: "LainRobertson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-13 12:30:08 +00:00
permalink: "2025-08-13-Finding-Large-Directories-and-Recovering-Lost-Disk-Space-with-PowerShell.html"
categories: ["Coding"]
tags: [".NET", "Automation", "Coding", "Community", "Directory Size", "Disk Space Management", "Filesystem Analysis", "Get DirectorySize", "Get DirectoryTreeSize", "Module Development", "PowerShell", "Reparse Points", "Scripting", "System.IO.Directory", "Windows"]
tags_normalized: [["net", "automation", "coding", "community", "directory size", "disk space management", "filesystem analysis", "get directorysize", "get directorytreesize", "module development", "powershell", "reparse points", "scripting", "system dot io dot directory", "windows"]]
---

LainRobertson shares a PowerShell module and examples for analyzing large directories and recovering lost disk space, offering practical insights for Windows administrators and developers.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
.Size;
                ---
layout: "post"
title: "Finding Large Directories and Recovering Lost Disk Space with PowerShell"
description: "This practical guide by LainRobertson demonstrates how to identify large directories and analyze disk space usage using a custom PowerShell module. The post explains the reasoning behind the approach, shares complete code examples, and walks through real-world use cases for filesystem analysis and troubleshooting in Windows environments."
author: "LainRobertson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-13 12:30:08 +00:00
permalink: "2025-08-13-Finding-Large-Directories-and-Recovering-Lost-Disk-Space-with-PowerShell.html"
categories: ["Coding"]
tags: [".NET", "Automation", "Coding", "Community", "Directory Size", "Disk Space Management", "Filesystem Analysis", "Get DirectorySize", "Get DirectoryTreeSize", "Module Development", "PowerShell", "Reparse Points", "Scripting", "System.IO.Directory", "Windows"]
tags_normalized: [["net", "automation", "coding", "community", "directory size", "disk space management", "filesystem analysis", "get directorysize", "get directorytreesize", "module development", "powershell", "reparse points", "scripting", "system dot io dot directory", "windows"]]
---

LainRobertson shares a PowerShell module and examples for analyzing large directories and recovering lost disk space, offering practical insights for Windows administrators and developers.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
;
            }
        }
    }
    $Stats = Get-DirectorySize -Path $Path;
    $Summary.Count = $Stats.Count;
    $Summary.Size = $Stats.Size;
    $Summary.TotalCount += $Stats.Count;
    $Summary.TotalSize += $Stats.Size;
    $Summary;
}

Export-ModuleMember -Function @("Get-DirectorySize", "Get-DirectoryTreeSize");
```

## Usage Examples

### 1. Top Five Consumers by `TotalSize`

Useful for understanding the largest overall subdirectories in a tree:

```powershell
Get-DirectoryTreeSize -Path "D:\Data\Temp\Edge\windows" |
    Sort-Object -Property TotalSize -Descending |
    Select-Object -First 5 |
    Format-Table -AutoSize -Property TotalSize, TotalCount, Path
```

### 2. Top Five Consumers by `Size`

Helps target the largest single directories (excluding nested items):

```powershell
Get-DirectoryTreeSize -Path "D:\Data\Temp\Edge\windows" |
    Sort-Object -Property Size -Descending |
    Select-Object -First 5 |
    Format-Table -AutoSize -Property Size, Count, Path
```

## Why Avoid Reparse Points?

- Avoids double-counting files if they are referenced multiple times.
- Prevents including data from outside the target tree, ensuring accurate metrics.

## Implementation Notes

- Uses `[System.IO.Directory]` for better performance versus `Get-ChildItem`.
- `Get-ChildItem` can throw errors on reparse points; this approach dodges that.
- For illustration: `Get-ChildItem -Directory -Force -Path "$env:USERPROFILE\Application Data\";`

## Summary

This approach provides a pragmatic, replicable method for diagnosing disk space issues on Windows systems using PowerShell. By adopting built-in .NET functionality and careful logic around reparse points, it's robust and especially useful for IT pros and developers seeking scriptable answers.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
