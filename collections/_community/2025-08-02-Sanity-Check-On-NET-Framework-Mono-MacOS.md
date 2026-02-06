---
external_url: https://www.reddit.com/r/dotnet/comments/1mfy7yk/sanity_check_on_net_framework_mono_macos/
title: Sanity Check On .NET Framework / Mono / MacOS
author: tparikka
feed_name: Reddit DotNet
date: 2025-08-02 18:53:34 +00:00
tags:
- .NET
- .NET Framework
- Assembly Versioning
- Build Toolchain
- Cross Platform Development
- Developer Workflow
- MacOS
- Mono
- Rider IDE
- System.Net.Http
- Unit Testing
- Community
section_names:
- dotnet
primary_section: dotnet
---
Author tparikka discusses the challenges of building and testing .NET Framework 4.8 services on MacOS using Mono, highlighting assembly compatibility issues.<!--excerpt_end-->

## Summary

In this detailed post, tparikka explores the challenges faced when attempting to build and unit test several .NET Framework 4.8 worker services on MacOS. The motivation arises from a shift in their development team's hardware from Windows laptops to M3 MacBooks, aiming for improved stability, battery life, and performance.

## The Problem

- **Objective:** Enable building and running unit tests (not full runtime) for .NET Framework 4.8 worker services on MacOS.
- **Tools Used:** Mono was selected as the cross-platform toolchain to facilitate this process.
- **Obstacle Encountered:** The build process failed with an error stemming from a version mismatch in the `System.Net.Http` assembly—a required version was 4.2.0.0, but Mono supplied version 4.1.1.3 (which targets up to .NET Framework 4.6).
- **Investigation:** Using the Rider IDE, the author confirmed that Mono's bundled `System.Net.Http` does not match the version needed for successful builds against .NET Framework 4.8 targets.
- **Documentation Check:** Mono’s official documentation indicates only up to .NET Framework 4.7 compatibility, further signalling limitations for targeting 4.8.

## Possible Workarounds Considered

- **Binding Redirects:** Implementing assembly binding redirects was considered but deemed undesirable as it would introduce complexity and undermine the goal of a seamless, low-maintenance developer experience.

## Author's Conclusion

The author concludes this is likely a significant roadblock for supporting legacy .NET Framework development and testing on MacOS. They ponder if developers working on these older projects should either use solutions like DevBox (cloud-hosted Windows environments) or remain on dedicated Windows machines for full compatibility.

## Key Takeaways

- Mono may not be sufficient for building or testing projects targeting .NET Framework 4.8 on MacOS due to assembly compatibility reasons.
- Transitioning legacy .NET Framework workflows to non-Windows systems remains difficult, and teams may need specialized setups (such as remote dev environments or Windows hardware) for successful builds.
- This scenario highlights broader cross-platform limitations legacy Microsoft technology stacks can present.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mfy7yk/sanity_check_on_net_framework_mono_macos/)
