---
layout: "post"
title: "Running an ASP.NET Core App Inside IIS in a Windows Docker Container"
description: "Andrew Lock explores running ASP.NET Core applications within IIS inside Windows containers. The post details differences between Linux and Windows containers, preparing Dockerfiles, configuring IIS, managing app pools with PowerShell, entrypoint strategies, and troubleshooting environment variable conflicts."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/running-an-aspnetcore-app-behind-iis-in-a-windows-container/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-03-11 09:00:00 +00:00
permalink: "/blogs/2025-03-11-Running-an-ASPNET-Core-App-Inside-IIS-in-a-Windows-Docker-Container.html"
categories: ["Coding", "DevOps"]
tags: ["App Pools", "APPCMD", "ASP.NET Core", "Coding", "Configuration", "Containerization", "DevOps", "Docker", "Environment Variables", "Front End", "Hosting Bundle", "IIS", "Blogs", "PowerShell", "ServiceMonitor.exe", "Troubleshooting", "Windows Containers"]
tags_normalized: ["app pools", "appcmd", "aspdotnet core", "coding", "configuration", "containerization", "devops", "docker", "environment variables", "front end", "hosting bundle", "iis", "blogs", "powershell", "servicemonitordotexe", "troubleshooting", "windows containers"]
---

Andrew Lock explains the process of hosting an ASP.NET Core application in IIS on a Windows Docker container. Learn about Dockerfile creation, configuring IIS, and resolving common issues.<!--excerpt_end-->

# Running an ASP.NET Core App Inside IIS in a Windows Docker Container

## Introduction

Andrew Lock examines how to run an ASP.NET Core application in IIS hosted within a Windows container. The post compares Linux and Windows containers, details the creation of Dockerfiles for the Windows environment, illustrates key scripts for IIS integration, and provides solutions for encountered issues.

---

## Containers: Linux vs Windows

- **Linux containers** are widespread, well-supported, and offer reliable, lightweight environments for ASP.NET Core applications. They're supported on Windows, Linux, and macOS hosts, and base images are typically small and fast.
- **Windows containers** are needed primarily for applications depending on Windows-specific APIs or requiring IIS. However, Windows base images are significantly larger (on the order of gigabytes), are more resource-intensive, and only support matching Windows OS versions between container and host.

**Key Differences:**

- Windows images are much larger than Linux counterparts.
- Authoring Dockerfiles for Windows images can feel more cumbersome compared to Linux.
- Windows containers are slower due to their size and limitations on which hosts can run which containers.

**Why Use Windows Containers?**

- Support for legacy applications or those depending on Windows-only features/APIs.
- IIS hosting requirements for ASP.NET (non-Core) or ASP.NET Core apps requiring IIS.

---

## Building and Hosting ASP.NET Core Apps in IIS Using Windows Containers

### Multi-Stage Dockerfile Structure

A sample Dockerfile approach is outlined:

- **Stage 1**: Build and publish the ASP.NET Core application using the latest SDK.
- **Stage 2**: Use a Windows Server Core image with IIS and ASP.NET, install the ASP.NET Core hosting bundle, copy published files, configure IIS (websites and app pools), and set the entrypoint.

```dockerfile
# Build the ASP.NET Core app using the latest SDK

FROM mcr.microsoft.com/dotnet/sdk:9.0-windowsservercore-ltsc2022 AS builder

# Build the test app

WORKDIR /src
RUN dotnet new web --name AspNetCoreTest --output .
RUN dotnet publish "AspNetCoreTest.csproj" -c Release -o /src/publish

# IIS/ASP.NET base image

FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2022 AS publish
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
WORKDIR /app

# Install ASP.NET Core hosting bundle

RUN $url='https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/9.0.0/dotnet-hosting-9.0.0-win.exe'; \
    echo "Fetching " + $url; \
    Invoke-WebRequest $url -OutFile c:/hosting.exe; \
    Start-Process -Wait -PassThru -FilePath "c:/hosting.exe" -ArgumentList @('/install', '/q', '/norestart'); \
    rm c:/hosting.exe;

# Copy app files

COPY --from=builder /src/publish /app/.

# Configure IIS: remove default site, create new app pool and site

RUN Remove-WebSite -Name 'Default Web Site'; \
    c:\Windows\System32\inetsrv\appcmd add apppool /name:AspNetCorePool /managedRuntimeVersion:""; \
    New-Website -Name 'SmokeTest' -Port 5000 -PhysicalPath 'c:\app' -ApplicationPool 'AspNetCorePool';

ENTRYPOINT ["C:\\ServiceMonitor.exe", "w3svc", "AspNetCorePool"]
```

#### Key Steps and Tools

- **ASP.NET Core hosting bundle**: Installs the ASP.NET Core runtime and the IIS integration module (ANCM). Required for serving ASP.NET Core apps via IIS.
- **IIS Management**: Uses `APPCMD.EXE` and PowerShell cmdlets (available since IIS 10/Windows Server 2016) to control websites and app pools.
- **ServiceMonitor.exe**: The designated entrypoint, monitors the specified IIS app pool (e.g., `AspNetCorePool`) and exits when the app pool stops.

---

## Entrypoint Scripting: Custom Container Shutdown

In scenarios requiring more precise control (such as automated smoke testing), a custom entrypoint can be used instead of ServiceMonitor’s default approach. This script does the following:

1. Runs ServiceMonitor.exe to propagate environment variables and monitor IIS.
2. Explicitly starts the app pool.
3. Makes an HTTP request to the app.
4. Stops the app pool and exits the container.

**Sample PowerShell Entrypoint:**

```dockerfile
# Entrypoint PowerShell script for custom container shutdown logic

RUN echo 'Write-Host "Running servicemonitor to copy environment variables"; Start-Process -NoNewWindow -PassThru -FilePath "c:/ServiceMonitor.exe" -ArgumentList ("w3svc", "AspNetCorePool");' > C:\app\entrypoint.ps1; \
    echo 'Write-Host "Starting AspNetCorePool app pool"; Start-WebAppPool -Name "AspNetCorePool" -PassThru;' >> C:\app\entrypoint.ps1; \
    echo 'Write-Host "Making 404 request"; curl http://localhost:5000;' >> C:\app\entrypoint.ps1; \
    echo 'Write-Host "Stopping pool"; Stop-WebAppPool "AspNetCorePool" -PassThru;' >> C:\app\entrypoint.ps1; \
    echo 'Write-Host "Shutting down"' >> C:\app\entrypoint.ps1;

ENTRYPOINT ["powershell", "-File", "C:\\app\\entrypoint.ps1"]
```

---

## Controlling App Pool Startup

Certain IIS app pool properties impact startup and worker process creation:

- **autoStart**: When set to false, requires manual activation.
- **startMode**: 'AlwaysRunning' immediately launches the worker process, 'OnDemand' waits for an incoming request.
- **preloadEnabled**: Triggers IIS to simulate a request at startup (must combine with AlwaysRunning).

In practice, even with these settings, the desired startup/shutdown control can be finicky due to IIS’s internal behavior and initialization mechanisms. As a workaround, the author uses scripts that explicitly handle the pool lifecycle.

---

## Troubleshooting: APPCMD Error 183

**Symptom:**

```
Service 'w3svc' has been stopped
APPCMD failed with error code 183
Failed to update IIS configuration
```

**Root Cause:**

- This error occurs if the same environment variable is both in IIS’s `<environmentVariables>` and set in the container's environment, leading ServiceMonitor.exe to attempt a duplicate creation, which fails.

**Solution:**

- Remove duplicate environment variable definitions either from the app pool or the Dockerfile to avoid collision.

---

## Summary

- Detailed walkthrough for running ASP.NET Core apps under IIS within Windows containers.
- Necessity of the ASP.NET Core hosting bundle for IIS integration.
- Dockerfile strategies and IIS/PowerShell automation techniques.
- Workflow for handling non-trivial container/app pool lifecycle requirements.
- Guidance for resolving common environment configuration errors.

---

**Author:** Andrew Lock

Stay up-to-date with posts from Andrew Lock and .Net Escapades.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/running-an-aspnetcore-app-behind-iis-in-a-windows-container/)
