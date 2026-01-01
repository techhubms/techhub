---
layout: "post"
title: "Setting Environment Variables in IIS Without Triggering App Pool Restarts"
description: "Andrew Lock details several methods for configuring environment variables for IIS worker processes. The post covers approaches for system and service-level variable setting, editing applicationHost.config, and preventing unnecessary app pool restarts during configuration changes."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/setting-environment-variables-in-iis-and-avoiding-app-pool-restarts/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-02-18 09:00:00 +00:00
permalink: "/2025-02-18-Setting-Environment-Variables-in-IIS-Without-Triggering-App-Pool-Restarts.html"
categories: ["Coding"]
tags: [".NET", "Appcmd.exe", "Application Pool", "Applicationhost.config", "ASP.NET Core", "Blogs", "Coding", "Configuration", "Environment Variables", "Hosting", "IIS", "PowerShell", "Recycling", "W3SVC", "Windows", "Worker Process"]
tags_normalized: ["dotnet", "appcmddotexe", "application pool", "applicationhostdotconfig", "aspdotnet core", "blogs", "coding", "configuration", "environment variables", "hosting", "iis", "powershell", "recycling", "w3svc", "windows", "worker process"]
---

In this post, Andrew Lock provides practical strategies for setting environment variables in IIS application pools on Windows, covering both UI and command-line techniques, and explains how to prevent unwanted app pool restarts.<!--excerpt_end-->

# Setting Environment Variables in IIS Without Triggering App Pool Restarts

**By Andrew Lock**

## Introduction

This post provides a comprehensive guide on configuring environment variables for IIS (Internet Information Services) worker processes on Windows. Andrew Lock explores several approaches, including system-wide, service-specific, and application pool-level settings. The article also offers advice on how to avoid unnecessary IIS application pool recycles when changing environment-related configurations.

## How IIS Works: An Overview

Understanding how IIS routes HTTP requests is fundamental to effective configuration. The main components involved are:

- **HTTP.sys**: A kernel-mode driver handling HTTP requests, forwarding them to worker processes.
- **Windows Process Activation Service (WAS)**: Manages application pool configuration and worker process lifecycles.
- **World Wide Web Publishing Service (W3SVC)**: Bridges WAS and HTTP.sys, updating configurations and dispatching requests.
- **Application Pools**: Containers for worker processes, potentially hosting multiple sites or applications.
- **Worker Processes (w3wp.exe)**: Processes that run managed and native modules for request handling.

#### IIS Request Flow

1. Client sends a request, intercepted by HTTP.sys.
2. HTTP.sys consults W3SVC/WAS for configuration.
3. WAS retrieves config from *applicationHost.config* and site-level *web.config*.
4. WAS relays app pool and site config to W3SVC, which updates HTTP.sys.
5. WAS starts a worker process if necessary.
6. HTTP.sys passes details to the worker process, which generates a response.
7. HTTP.sys sends the response back to the client.

![How IIS processes an HTTP request](/content/images/2025/iis.svg)

## Methods for Setting Environment Variables

Lock explores three main methods for adjusting environment variables for IIS worker processes:

### 1. System-Wide Environment Variables

System variables affect all processes and can be set with PowerShell:

```powershell
[System.Environment]::SetEnvironmentVariable("MY_SPECIAL_KEY", "Some value", [System.EnvironmentVariableTarget]::Machine)
```

To apply these changes to running services:

```powershell
net stop /y was
net start w3svc
```

**Caveat**: This method applies the variable to every process and may have unintended consequences, especially for variables like the .NET profiling API.

### 2. Service-Specific Environment Variables (W3SVC and WAS)

Variables can be scoped to W3SVC and WAS through the Windows Registry:

```powershell
Set-ItemProperty HKLM:SYSTEM\CurrentControlSet\Services\W3SVC -Name Environment -Value 'MY_SPECIAL_KEY=Something' -Type MultiString
Set-ItemProperty HKLM:SYSTEM\CurrentControlSet\Services\WAS -Name Environment -Value 'MY_SPECIAL_KEY=Something' -Type MultiString
```

Alternatively, use the Registry Editor UI to add multi-string values.

Restart services for changes to take effect:

```powershell
net stop /y was
net start w3svc
```

This method ensures only processes started by these services (i.e., IIS worker processes) inherit the variable.

### 3. Application Pool-Level Variables via applicationHost.config

*applicationHost.config*, typically located at `C:\Windows\System32\inetsrv\config\applicationHost.config`, defines IIS app pools in the `<system.applicationHost>` section.

Sample configuration:

```xml
<applicationPools>
  <add name="DefaultAppPool" />
  <add name="dotnet7" autoStart="true" managedRuntimeVersion="">
    <environmentVariables>
      <add name="MY_VAL" value="1" />
      <add name="MY_SPECIAL_KEY" value="Something" />
    </environmentVariables>
  </add>
  <applicationPoolDefaults managedRuntimeVersion="v4.0">
    <processModel identityType="ApplicationPoolIdentity" loadUserProfile="true" setProfileEnvironment="false" />
    <environmentVariables>
      <add name="MY_SPECIAL_KEY" value="Something" />
    </environmentVariables>
  </applicationPoolDefaults>
</applicationPools>
```

#### Adding or Updating Variables

- **To apply a variable for all pools**: Add it to `<applicationPoolDefaults>`.
- **To override for a specific pool**: Add directly beneath the pool's configuration.

**Note**: If a pool defines its own `<environmentVariables>`, it will not inherit from the defaults.

#### Editing applicationHost.config

- **Via IIS UI**: Navigate Configuration Editor > `system.applicationHost/applicationPools`, expand and edit `environmentVariables` for pools and defaults using ellipsis buttons. Apply and restart as necessary.
- **Via appcmd.exe**: Use command-line for faster edits.

To add:

```powershell
C:\Windows\System32\inetsrv\appcmd.exe set config -section:system.applicationHost/applicationPools /+"[name='dotnet7'].environmentVariables.[name='MY_SPECIAL_KEY',value='Something']" /+"applicationPoolDefaults.environmentVariables.[name='MY_SPECIAL_KEY',value='Something']" /commit:apphost;
```

To update existing variables:

```powershell
C:\Windows\System32\inetsrv\appcmd.exe set config -section:system.applicationHost/applicationPools /"[name='dotnet7'].environmentVariables.[name='MY_SPECIAL_KEY'].value:'Something'" /"applicationPoolDefaults.environmentVariables.[name='MY_SPECIAL_KEY'].value:'Something'" /commit:apphost;
```

A helpful [guide](https://blogs.iis.net/eokim/understanding-appcmd-exe-list-set-config-configurationpath-section-name-parameter-name-value) explains the nuances of this syntax.

## Preventing Automatic App Pool Restarts on Config Changes

By default, changing *applicationHost.config* triggers immediate recycling of application pools, potentially causing short downtime. To delay this:

- Set the `disallowRotationOnConfigChange` attribute to `true` in the `<recycling>` element of each app pool or in `<applicationPoolDefaults>`.

Sample configuration:

```xml
<applicationPools>
  <add name="dotnet7">
    ...
    <recycling disallowRotationOnConfigChange="true" />
  </add>
  <applicationPoolDefaults>
    ...
    <recycling disallowRotationOnConfigChange="true" />
  </applicationPoolDefaults>
</applicationPools>
```

Corresponding appcmd.exe command:

```powershell
C:\Windows\System32\inetsrv\appcmd.exe set config -section:system.applicationHost/applicationPools /"[name='dotnet7'].recycling.disallowRotationOnConfigChange:true" /"applicationPoolDefaults.recycling.disallowRotationOnConfigChange:true" /+"[name='dotnet7'].environmentVariables.[name='MY_SPECIAL_KEY',value='Something']" /+"applicationPoolDefaults.environmentVariables.[name='MY_SPECIAL_KEY',value='Something']" /commit:apphost;
```

To revert to default recycling behavior:

```powershell
C:\Windows\System32\inetsrv\appcmd.exe set config -section:system.applicationHost/applicationPools /"[name='dotnet7'].recycling.disallowRotationOnConfigChange:false" /"applicationPoolDefaults.recycling.disallowRotationOnConfigChange:false" /commit:apphost;
```

Changing this attribute does not itself trigger recycling, providing a method to suppress unwanted restarts during configuration changes.

## Summary

- IIS worker process environment variables can be set globally, per service, or per application pool.
- Using the `disallowRotationOnConfigChange` attribute allows implementing changes without immediate app pool restarts, preventing unnecessary application downtime.
- Methods include Windows UI, Registry modification, command-line (appcmd.exe), and IIS Configuration Editor.

---

> **References and Links:**
>- [Introduction to IIS Architecture (Microsoft Learn)](https://learn.microsoft.com/en-us/iis/get-started/introduction-to-iis/introduction-to-iis-architecture)
>- [Managing environment variables in applicationHost.config](https://learn.microsoft.com/en-us/iis/configuration/system.applicationHost/applicationPools/add/environmentVariables/)
>- [IIS.Administration REST API (GitHub)](https://github.com/microsoft/IIS.Administration/)

---

*Andrew Lock | .Net Escapades*

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/setting-environment-variables-in-iis-and-avoiding-app-pool-restarts/)
