---
layout: post
title: High RAM Usage Troubleshooting in ASP.NET Core MVC 8 Application
author: scartus
canonical_url: https://www.reddit.com/r/dotnet/comments/1mk4hp1/high_ram_usage_aspnet_core_mvc/
viewing_mode: external
feed_name: Reddit DotNet
feed_url: https://www.reddit.com/r/dotnet/.rss
date: 2025-08-07 16:19:11 +00:00
permalink: /coding/community/High-RAM-Usage-Troubleshooting-in-ASPNET-Core-MVC-8-Application
tags:
- .NET
- .NET 8
- ASP.NET Core
- Caching
- EF Core
- IIS
- Memory Leaks
- Memory Usage
- MVC
- Performance Profiling
- Production Diagnostics
- Thumbnails
- VS
- WinDbg
- Windows Server
section_names:
- coding
---
scartus initiates a discussion about high memory usage in an ASP.NET Core MVC 8 application, seeking recommendations and insights from the developer community.<!--excerpt_end-->

# High RAM Usage Troubleshooting in ASP.NET Core MVC 8 Application

**Posted by scartus**

## Problem Statement

An ASP.NET Core MVC 8 app is running on IIS on Windows Server 2025 with 4GB RAM, supporting 50–70 users. The application frequently consumes 2.5GB RAM and experiences out-of-memory errors during business hours. Key workload: users perform database operations (using EF Core), and view case details with thumbnails (10–300KB each) retrieved and generated from a network folder.

### Author's Questions

- Is this level of memory usage normal given the scenario?
- Should the focus be on optimization or hardware upgrade?
- What tools are recommended for analyzing high memory usage, preferably capable of production diagnostics (VS, Rider, others)?

## Community Feedback Summary

- **Initial Step:** Take a memory dump during high memory usage, then analyze with Visual Studio or WinDbg to inspect object counts and sizes.
- **Questions to Investigate:**
  - How are images retrieved or generated? Unmanaged resources such as files or handles might not be disposed properly.
  - Is the IIS application pool running in 32-bit mode (limiting max memory)?
- **Potential Causes:**
  - Leaked unmanaged resources (file handles, memory streams not disposed)
  - Inefficient or unoptimized EF Core queries loading too much data
  - Ineffective caching strategy for thumbnails; suggest static caching or pre-generation
  - Site memory limit may be too low for modern web workloads
- **Tools for Analysis:**
  - Visual Studio and Rider for memory profiling
  - WinDbg for advanced memory dump inspection
  - Use production profilers when possible for most accurate picture
- **Best Practices:**
  - Profile memory allocations and check for leaks
  - Confirm app pool is 64-bit where possible
  - Optimize database logic, especially loading large collections
  - Consider hardware upgrades; 4GB is small by modern standards
- **Operational Tips:**
  - Taking a memory dump pauses the app; use fast local disks to reduce downtime
  - Memory issues may appear only in production—rarely reproducible locally

## Example Diagnostic Steps

1. Initiate a memory dump during high load via Task Manager or dedicated tools
2. Analyze dump to identify objects holding memory
3. Check code for proper disposal of images, file streams, or other unmanaged resources
4. Profile database queries for efficiency
5. Review environment (32vs64 bit, memory caps)
6. Implement caching strategies for thumbnails, if not already in place

## Conclusion

Out-of-memory exceptions indicate a serious issue and are not typical with this workload or user count. Community suggests profiling and code analysis before hardware upgrade, with a focus on identifying leaks, resource handling, and query optimization.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mk4hp1/high_ram_usage_aspnet_core_mvc/)
