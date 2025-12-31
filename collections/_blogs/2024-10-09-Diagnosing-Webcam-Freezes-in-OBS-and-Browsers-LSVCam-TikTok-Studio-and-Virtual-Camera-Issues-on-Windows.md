---
layout: "post"
title: "Diagnosing Webcam Freezes in OBS and Browsers: LSVCam, TikTok Studio, and Virtual Camera Issues on Windows"
description: "Scott Hanselman shares a detailed troubleshooting journey into mysterious webcam pauses affecting OBS Studio and browser-based video apps, tracing the issue to faulty virtual camera drivers from TikTok Live Studio (LSVCam). The article offers tools, registry hacks, and technical insights for identifying and resolving virtual camera-related hangs on Windows systems."
author: "Scott Hanselman"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.hanselman.com/blog/webcam-randomly-pausing-in-obs-discord-and-websites-lsvcam-and-tiktok-studio"
viewing_mode: "external"
feed_name: "Scott Hanselman's Blog"
feed_url: "https://www.hanselman.com/blog/SyndicationService.asmx/GetRss"
date: 2024-10-09 19:32:28 +00:00
permalink: "/blogs/2024-10-09-Diagnosing-Webcam-Freezes-in-OBS-and-Browsers-LSVCam-TikTok-Studio-and-Virtual-Camera-Issues-on-Windows.html"
categories: ["Coding"]
tags: ["Browser APIs", "Bugs", "Coding", "Device Drivers", "DirectShow", "LSVCam", "Media Device Enumeration", "OBS Studio", "Blogs", "PowerShell", "Registry", "TikTok Live Studio", "Troubleshooting", "Video Streaming", "Virtual Camera", "Webcam", "Windows", "Windows Media Foundation"]
tags_normalized: ["browser apis", "bugs", "coding", "device drivers", "directshow", "lsvcam", "media device enumeration", "obs studio", "blogs", "powershell", "registry", "tiktok live studio", "troubleshooting", "video streaming", "virtual camera", "webcam", "windows", "windows media foundation"]
---

Scott Hanselman documents his investigation into persistent webcam hangs affecting OBS Studio and browser apps on Windows, identifying the LSVCam virtual camera from TikTok Live Studio as the culprit. He details detection methods, registry fixes, and practical observations.<!--excerpt_end-->

# Diagnosing Webcam Freezes in OBS and Browsers: LSVCam, TikTok Studio, and Virtual Camera Issues on Windows

Author: Scott Hanselman

## Overview

Scott Hanselman describes a frustrating, technically in-depth problem where his webcam feed would regularly pause for 10-15 seconds when accessed from browsers or when loading properties in OBS Studio. His long experience with video streaming tools, including extensive use of virtual cameras (especially with OBS Studio), provided the basis for a systematic troubleshooting approach.

## The Problem

- **Symptoms**: Webcam streams intermittently froze every 90 seconds in browser apps (e.g., Zencastr, StreamYard, Riverside) and caused long hangs inside OBS Studio.
- **Initial Suspects**: USB issues or hardware reliability. Both the USB bus and reliable El Gato capture device were ruled out.

## Investigation

Scott discovered that neither Windows settings nor the main UI offer a comprehensive list of connected or virtual cameras. To get a full device list, he used:

### PowerShell Script

```powershell
Get-CimInstance -Namespace root\cimv2 -ClassName Win32_PnPEntity | Where-Object { ---
layout: "post"
title: "Diagnosing Webcam Freezes in OBS and Browsers: LSVCam, TikTok Studio, and Virtual Camera Issues on Windows"
description: "Scott Hanselman shares a detailed troubleshooting journey into mysterious webcam pauses affecting OBS Studio and browser-based video apps, tracing the issue to faulty virtual camera drivers from TikTok Live Studio (LSVCam). The article offers tools, registry hacks, and technical insights for identifying and resolving virtual camera-related hangs on Windows systems."
author: "Scott Hanselman"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.hanselman.com/blog/webcam-randomly-pausing-in-obs-discord-and-websites-lsvcam-and-tiktok-studio"
viewing_mode: "external"
feed_name: "Scott Hanselman's Blog"
feed_url: https://www.hanselman.com/blog/SyndicationService.asmx/GetRss
date: 2024-10-09 19:32:28 +00:00
permalink: "2024-10-09-Diagnosing-Webcam-Freezes-in-OBS-and-Browsers-LSVCam-TikTok-Studio-and-Virtual-Camera-Issues-on-Windows.html"
categories: ["Coding"]
tags: ["Browser APIs", "Bugs", "Coding", "Device Drivers", "DirectShow", "LSVCam", "Media Device Enumeration", "OBS Studio", "Blogs", "PowerShell", "Registry", "TikTok Live Studio", "Troubleshooting", "Video Streaming", "Virtual Camera", "Webcam", "Windows", "Windows Media Foundation"]
tags_normalized: [["browser apis", "bugs", "coding", "device drivers", "directshow", "lsvcam", "media device enumeration", "obs studio", "blogs", "powershell", "registry", "tiktok live studio", "troubleshooting", "video streaming", "virtual camera", "webcam", "windows", "windows media foundation"]]
---

Scott Hanselman documents his investigation into persistent webcam hangs affecting OBS Studio and browser apps on Windows, identifying the LSVCam virtual camera from TikTok Live Studio as the culprit. He details detection methods, registry fixes, and practical observations.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Scott Hanselman's Blog". [Read the entire article here](https://www.hanselman.com/blog/webcam-randomly-pausing-in-obs-discord-and-websites-lsvcam-and-tiktok-studio)
.Name -match 'Cam' } | Select-Object Name, Manufacturer, PNPDeviceID
```

This lists both physical and virtual webcams from the device manager level.

### JavaScript Enumeration

In browser environments, the following JavaScript gets a list of video input devices:

```javascript
async function listWebcams() {
  try {
    const devices = await navigator.mediaDevices.enumerateDevices();
    const webcams = devices.filter(device => device.kind === 'videoinput');
    webcams.forEach((webcam, index) => {
      console.log(`${index + 1}. ${webcam.label || `Camera ${index + 1}`}`);
    });
  } catch (error) {
    console.error("Error accessing media devices:", error);
  }
}
listWebcams();
```

Typical output included legitimate physical devices and several virtual cameras—but also a mysterious “LSVCam”.

## Root Cause: The LSVCam Mystery

- **LSVCam** appeared inconsistently across apps and caused device enumeration to hang or throw NotReadableError in the browser API.
- Device properties for LSVCam were visible in the Windows registry at:

  ```
  HKEY_CLASSES_ROOT\CLSID\{860BB310-5D01-11d0-BD3B-00A0C911CE86}\Instance\LSVCam
  ```

- Research revealed that LSVCam is installed by **TikTok Live Studio** as part of its game streaming/virtual camera stack.
- The virtual camera appeared to be incompletely installed or misbehaving, creating system-wide video device enumeration hangs and errors.

## Resolution

- **Registry Fix**: Safely deleting the registry key specific to LSVCam removed the device from enumeration—but TikTok Live Studio would recreate it on each launch.
- **Caution**: Only delete the LSVCam key, not the entire Instance registry tree, to avoid damaging other system components.
- **Source File**: LSVCam.dll is located in the TikTok Live Studio program files.

## Technical Notes

- Virtual camera enumeration on Windows differs according to whether devices are implemented using DirectShow or Windows Media Foundation. Device visibility and stability varies accordingly.
- PowerShell and JavaScript methods provide a comprehensive technical approach for device auditing.

## Additional Observations & Community Input

- Other users in the comments reported mixed results, depending on the underlying virtual camera technology and software used.
- Some cameras, like XSplit VCam or Camo, appear depending on whether they use DirectShow or Media Foundation.

## Takeaways

- If virtual cameras freeze enumeration or disrupt streaming apps in Windows, third-party programs (like TikTok Live Studio) may be to blame.
- Developers and advanced users can audit virtual camera devices via PowerShell and JavaScript.
- Registry editing is an effective workaround pending official fixes from software vendors.

---

*Disclaimer: Modifying the registry may adversely affect your system. Proceed with caution and back up your registry beforehand. These steps are provided for informational purposes only.*

© 2025 Scott Hanselman. All rights reserved.

This post appeared first on "Scott Hanselman's Blog". [Read the entire article here](https://www.hanselman.com/blog/webcam-randomly-pausing-in-obs-discord-and-websites-lsvcam-and-tiktok-studio)
