---
external_url: https://www.reddit.com/r/dotnet/comments/1mgewsc/just_built_a_tool_that_turns_any_app_into_a/
title: 'C# Tool to Run Any App as a Windows Service: Managed Alternative to NSSM'
author: AdUnhappy5308
feed_name: Reddit DotNet
date: 2025-08-03 09:21:26 +00:00
tags:
- .NET
- C#
- Custom Working Directory
- NSSM Alternative
- Open Source
- Process Wrapper
- Service Management
- Startup Type
- Windows App Deployment
- Windows Service
section_names:
- coding
---
AdUnhappy5308 shares details about a new C# tool that runs any app as a Windows service, highlighting its capabilities and differences from NSSM.<!--excerpt_end-->

## Overview

AdUnhappy5308 presents a new open-source tool written in C# that enables users to turn any application into a Windows service. This tool is positioned as a fully managed alternative to the widely used NSSM (Non-Sucking Service Manager), and it is designed to work across Windows 7–11 and Windows Server environments.

## Key Features

- **Service Customization:** Set the service name, description, and startup type (Automatic, Manual, Disabled).
- **Custom Executable Path:** Specify any executable to be run as a service.
- **Working Directory Support:** Unlike the default Windows service behavior, this tool allows you to define the working directory for the launched application, solving a long-standing limitation where Windows services default to `C:\Windows\System32`.
- **Custom Parameters:** Pass launch parameters to the underlying executable.

## Technical Approach

The main technical challenge addressed was setting the working directory for the service's process. By default, Windows services don't provide a way to adjust this. The solution involves a C# wrapper service that accepts the executable path, target working directory, and parameters, then launches the real process in the correct context as a child process. This method is similar to how NSSM operates but is implemented entirely in managed C# code.

## Compatibility

- **Supported OS:** Windows 7–11, Windows Server

## Repository and Feedback

- **Source Code:** [GitHub - aelassas/servy](https://github.com/aelassas/servy)
- **Feedback:** The author is open to comments and suggestions for improvements.

## Usage Example

Suppose you have a custom application that needs to run as a background service, but requires a specific working directory or custom startup arguments. With this tool, you can configure all relevant parameters through a managed interface, bypassing the need for third-party native utilities.

## Comparison to NSSM

- Both tools allow non-service executables to run as windows services.
- The main distinction is that this new tool is fully managed, making it easier to inspect, extend, or integrate into .NET-focused environments.

## Community

- Original Reddit discussion: [Reddit post](https://www.reddit.com/r/dotnet/comments/1mgewsc/just_built_a_tool_that_turns_any_app_into_a/)

---

This open-source C# utility may prove beneficial for developers or administrators looking to manage legacy or non-service applications as Windows services in a modern, .NET-aligned way.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mgewsc/just_built_a_tool_that_turns_any_app_into_a/)
