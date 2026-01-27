---
external_url: https://www.reddit.com/r/csharp/comments/1mjszox/need_help_automating_windows_forms_inside_remote/
title: 'Automating Legacy WinForms Apps in RDP: UI Automation vs. Computer Vision (C# Solutions)'
author: Grifone87
feed_name: Reddit CSharp
date: 2025-08-07 06:50:03 +00:00
tags:
- .NET 6
- .NET 7
- API Monitor
- AutoHotkey
- C#
- Computer Vision
- Database Access
- FlaUI
- Microsoft Access
- ML.NET
- OCR
- OpenCV
- PE Explorer
- PInvoke
- PowerShell Remoting
- Process Monitor
- RDP Automation
- Remote Desktop
- Reverse Engineering
- SendKeys
- Template Matching
- UI Automation API
- WinAutomation
- Windows Forms
- Windows Server
- YOLO
section_names:
- coding
primary_section: coding
---
Grifone87 and the r/csharp community discuss complexities of automating a WinForms insurance system inside an RDP session with C#, weighing UI automation versus computer vision approaches with useful insights and reverse engineering tips.<!--excerpt_end-->

# Automating Windows Forms Apps in Remote Desktop: UI Automation vs. Computer Vision

**Author:** Grifone87

## Scenario Overview

A legacy Windows Forms insurance management system, running inside a Remote Desktop (RDP) session on Windows Server 2019, needs automation for repetitive tasks (searching, clicking, data extraction). There is no API, no source code, and no software installation allowed on the remote.

### Key Challenges

- **RDP barrier:** UI automation frameworks (FlaUI, Windows UI Automation API) can't enumerate controls inside RDP – the remote window is just a bitmap.
- **Coordinate-based actions:** Fragile against screen resolution/scaling changes.
- **No remote software installation:** Can't run agents or install helper tooling inside RDP session.

## Approaches Tried

- **FlaUI/UI Automation:** Limited to the 'mstsc.exe' container, can't see inside.
- **SendKeys/Coordinate Clicking:** Highly brittle due to environmental variability.
- **AutoHotkey:** Runs into same coordinate/location issues; C# preferable.

## Potential Solutions & Community Insights

### 1. Computer Vision Techniques

- **OpenCV/ML.NET:** Use template matching or ML models (like YOLO) for control detection.
  - Extract controls or buttons visually within the RDP bitmap.
  - OCR (including non-English support; the app is in Italian) for reading UI text.
- **Windows OCR API:** For extracting on-screen text.
- **Tools like WinAutomation:** Can click based on image matching.
- **Limitations:** CPU intensive, can be less reliable due to UI changes, requires model/template maintenance.

### 2. Reverse Engineering Strategies

- **DLL Hunting:** Investigate application directory for DLLs doing the real work—possible to PInvoke/export to interface directly, *if* allowed.
- **Database Access:** Old WinForms apps may use local databases (e.g., MS Access MDB/ACCDB or SQLite). If accessible, you may automate at the data layer (read/write directly to DB).
  - Use Process Monitor to identify file access patterns.
- **API Monitor/PE Explorer/IDA Pro/Ghidra:** For static/dynamic analysis to discover hidden integration points.
- **Limitation:** Most options off-limits because you can't install/run extra tools remotely.

### 3. Remote Script/Automation

- **PowerShell Remoting:** If remoting is allowed, script actions directly on the server; avoids UI fragility.
- **File Sync:** Copy the database (if feasible) to a local or cloud location for easier script-based access.
- **CLI Automation:** If the app has a CLI or exposed batch jobs, leverage those for process integration rather than the UI.

### 4. Constraints & Pragmatic Considerations

- **Security and permissions:** Cannot install or execute things with admin permissions on remote.
- **Interactive video barrier:** RDP is essentially streaming video; UI automation sees a single windowed image.
- **Resolution mismatch:** Always account for unpredictable differences in display setup.

## Tools Mentioned

- PE Explorer, Process Monitor, Api Monitor, Microsoft Detours, PInvoke.net, Ollydbg, IDA Pro, Ghidra, WinAutomation.

## Recommendations Summary

- *If no install possible and no database access*: Computer Vision techniques (OpenCV, OCR) are most viable, though maintenance-heavy.
- *If you can access backend files/database*: Reverse engineer for DB/file access.
- *Explore remote scripting (PowerShell) or CLI access if permissions allow*.

## Example (Computer Vision Button Detection with OpenCV in C#)

```csharp
// Sample: Finding a button template inside RDP screenshot
using OpenCvSharp;
Mat screenshot = Cv2.ImRead("rdp_capture.png");
Mat template = Cv2.ImRead("button_template.png");
Mat result = new Mat();
Cv2.MatchTemplate(screenshot, template, result, TemplateMatchModes.CCoeffNormed);
double minVal, maxVal; Point minLoc, maxLoc;
Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);
if (maxVal > 0.8) { /* simulate click at maxLoc via SendInput */ }
```

---

## Conclusion

When dealing with legacy WinForms inside RDP, pure UI automation isn't possible. Computer vision can work with enough engineering investment. Always explore backend/database access, PowerShell remoting, or file syncing for more robust solutions if at all feasible.

---

### Further Reading & Tools

- [OpenCV](https://opencv.org/)
- [ML.NET](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet)
- [Windows OCR API](https://learn.microsoft.com/en-us/windows/uwp/input-and-devices/optical-character-recognition)
- [PowerShell Remoting](https://learn.microsoft.com/en-us/powershell/scripting/learn/remoting)
- [Process Monitor](https://learn.microsoft.com/en-us/sysinternals/downloads/procmon)

*Author: Grifone87 and r/csharp community contributors*

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mjszox/need_help_automating_windows_forms_inside_remote/)
