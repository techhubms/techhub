---
external_url: https://weblog.west-wind.com/posts/2024/Oct/24/Using-Sql-Server-on-Windows-ARM
title: 'How to Run SQL Server on Windows ARM Devices: Solutions and Workarounds'
author: Rick Strahl
feed_name: Rick Strahl's Blog
date: 2024-10-24 21:59:54 +00:00
tags:
- .NET
- ARM64
- Azure Data Studio
- Azure SQL Edge
- Database Development
- Docker
- Emulation
- LocalDb
- Named Pipes
- SnapDragon X Elite
- SQL Server
- SQL Server Express
- VS
- Windows
- Windows 11
- Windows ARM
section_names:
- azure
- coding
primary_section: coding
---
In this post, Rick Strahl shares his journey and solutions for running SQL Server on a Windows ARM device, providing practical techniques, troubleshooting steps, and community-sourced insight relevant for .NET and SQL Server developers.<!--excerpt_end-->

# Using SQL Server on Windows ARM Devices: Solutions and Workarounds

**Author:** Rick Strahl

---

## Introduction

Recently, I acquired a Samsung Galaxy Book 4 with a SnapDragon X Elite ARM processor to evaluate Windows application compatibility and performance on ARM hardware. While most Windows applications—including .NET-based desktop and web applications—ran natively or under emulation without issue, I encountered significant obstacles running SQL Server locally on the ARM device.

This article details my hands-on experience, covering successful and unsuccessful approaches to getting SQL Server operational for local development scenarios on Windows ARM. If you rely on SQL Server for development, particularly for offline use on ARM laptops, this guide walks you through the current landscape, caveats, community tips, and step-by-step workarounds.

---

## Table of Contents

1. [The Windows ARM Experience for Development](#the-windows-arm-experience-for-development)
2. [The SQL Server Challenge](#the-sql-server-challenge)
3. [SQL Server on Docker](#sql-server-on-docker)
4. [What Works: SQL Server LocalDb](#what-works-sql-server-localdb)
    - [Installing LocalDb v16 (2022)](#installing-localdb-v16-2022)
    - [Starting LocalDb and Finding Connection Info](#starting-localdb-and-finding-connection-info)
    - [Connecting with Named Pipes](#connecting-with-named-pipes)
    - [LocalDb Connection String Example](#localdb-connection-string-example)
5. [Unofficial SQL Server Installer Builds for ARM](#unofficial-sql-server-installer-builds-for-arm)
6. [Summary & Recommendations](#summary--recommendations)
7. [Resources](#resources)

---

## The Windows ARM Experience for Development

The Samsung Galaxy Book 4 with its SnapDragon X Elite was purchased mainly for experimentation. Overall, most applications—including legacy Visual Basic and FoxPro 32-bit apps—ran smoothly either as native ARM applications or under x64 emulation. Notably, all my .NET desktop and web applications worked out-of-the-box, even with heavy P/Invoke and native code usage.

**Minor Issues Encountered:**

- Some legacy third-party tools (e.g., older SnagIt releases, certain audio hardware) had compatibility problems.
- Networking was occasionally unreliable, particularly concerning slow or delayed Wi-Fi and DNS connectivity.
- Performance, while respectable, did not match high-end x64 laptops (e.g., noticeable lag launching heavy .NET/WPF applications).

> *Most tasks just worked and the ARM machine was a solid portable option for development, except when it came to SQL Server.*

---

## The SQL Server Challenge

Unlike most of my dev tools, setting up SQL Server for offline local use was a hurdle:

- **No ARM-native build**: SQL Server is not available in an ARM64 build.
- **x64 builds do not run**: Standard x64 SQL Server installers refuse to run on ARM hardware.
- **Docker images**: Most official Docker SQL Server images are for amd64 and do not run out of the box on Windows ARM.

I explored three main approaches:

1. **Running SQL Server via Linux-based Docker Images**
2. **Using LocalDb in emulation**
3. **Applying unofficial installer hacks**

---

## SQL Server on Docker

I attempted running SQL Server inside a Docker container, as Microsoft's official SQL Server on Linux images target AMD64 (x64) only. Docker for Windows ARM could not start these containers directly.

### Azure SQL Edge

Microsoft's [Azure SQL Edge](https://learn.microsoft.com/en-us/azure/azure-sql-edge/disconnected-deployment) image does support arm64 and can be run in Docker. I attempted:

```powershell
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=superseekrit1' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
```

However, the container failed to expose TCP/IP ports for direct connections (errors on port-mapping with `-p 1433:1433`) making it impractical for my needs. Community members suggested that with adjustments for WSL2 and Docker for Windows ARM, it can work, though reliability may be in question as Azure SQL Edge is being retired September 2025 and is losing ARM64 support.

---

## What Works: SQL Server LocalDb

After troubleshooting with help from community suggestions, I succeeded in running **SQL Server LocalDb**.

- LocalDb is a minimal, user-mode SQL Server more suited for development and testing.
- LocalDb is an x64 binary, but can run inside ARM64 emulation.

### Key Limitations

- TCP/IP connections typically do not work on ARM; only Named Pipes do.
- Visual Studio installs an older LocalDb (v15 for SQL Server 2019). If your data files are for SQL Server 2022, you need **LocalDb v16**.

### Installing LocalDb v16 (2022)

1. Download the latest LocalDb installer here: [Microsoft Docs: SQL Server LocalDb](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16#installation-media)
2. The installer is for SQL Server Express, but you can select only the LocalDb component (uncheck all others during feature selection).

![Installation](https://weblog.west-wind.com/images/2024/Using-Sql-Server-on-Windows-ARM/LocalDbInstallationFeatureSelection.png)

### Starting LocalDb and Finding Connection Info

Because TCP/IP is not available, start LocalDb manually and retrieve the Named Pipe connection:

```powershell
# List available LocalDb instances

SqlLocalDb info

# Show details for a specific instance

SqlLocalDb info "MsSqlLocalDb"

# Create a new instance with a specific version

SqlLocalDb create "MsSqlLocalDb2022" 16.0.1000.6

# Start the LocalDb instance

SqlLocalDb start "MsSqlLocalDb"

# Get connection info (Instance Pipe Name)

SqlLocalDb info "MsSqlLocalDb"
```

Copy the Named Pipe address from the output to use as your connection string.

![Start LocalDb](https://weblog.west-wind.com/images/2024/Using-Sql-Server-on-Windows-ARM/StartLocalDb.png)

> **Note:** The Named Pipe address changes per session, so you'll need to refresh it each time you restart the server.

### Connecting with Named Pipes

You can connect to LocalDb via tools like **Azure Data Studio**, **SSMS**, or from code. In Azure Data Studio, use the Named Pipe as the server and set encryption to optional (`encrypt=false`). Use the "Attach DB Filename" field for local `.mdf` files.

![Azure Data Studio Connection](https://weblog.west-wind.com/images/2024/Using-Sql-Server-on-Windows-ARM/AzureDataStudioConnection2.png)

### LocalDb Connection String Example

```csharp
var connStr = @"server=np:\\.\pipe\LOCALDB#270C6261\\tsql\query;" +
              @"AttachDbFilename=c:\SqlServerDataFolder\WestwindWebStore.mdf;" +
              "integrated security=true;encrypt=false";

var sql = new SqlDataAccess(connStr);
sql.OpenConnection();
```

> *Auto-starting LocalDb via code with the short server notation (`server=(localdb)\MsSqlLocalDb`) did not work on ARM in my case, likely due to lack of TCP/IP.*

---

## Unofficial SQL Server Installer Builds for ARM

As an alternative, some GitHub projects have published modified SQL Server Express/Developer installers to bypass ARM platform restrictions:

- [MSSQLEXPRESS-M1-Install (jimm98y on GitHub)](https://github.com/jimm98y/MSSQLEXPRESS-M1-Install)

The repository offers `.bat` scripts for custom installs or `.msi` bundles. In some cases, installation required multiple attempts (e.g., service not starting initially, then resolving after a failed uninstall attempt). Use "Run As Administrator" in tools like Azure Data Studio to establish connections using Windows Authentication.

Compared to LocalDb:

- Full SQL Server installs run as a service, always available, work with TCP/IP, and can use Windows Auth easily.
- LocalDb is less intrusive and lighter but requires manual management and only works with Named Pipes.

---

## Summary & Recommendations

Getting SQL Server running locally on Windows ARM is far from seamless:

- **No native or officially supported solution** exists yet.
- **LocalDb (v16+)** is currently the most viable option for most development use cases. It requires some manual management and connects via Named Pipes.
- **Unofficial custom installers** can enable full SQL Server installs, but come with risk and may require extra steps.
- **Docker with Azure SQL Edge** is an option but is losing ARM64 support and will be retired.

#### What Microsoft Could Improve

- Better documentation and tooling for SQL Server on ARM.
- Direct, updated LocalDb downloads for the latest SQL Server versions.
- Official ARM installers for SQL Server.

For most .NET developers needing SQL Server on Windows ARM: use LocalDb v16, connect via Named Pipes, and configure connection strings and data paths as described above.

---

## Resources

- [SQL Server Express LocalDb Download (Microsoft)](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16#installation-media)
- [Azure SQL Edge Docker Image](https://hub.docker.com/r/microsoft/azure-sql-edge)
- [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio/)
- [MSSQLEXPRESS-M1-Install Github Repo](https://github.com/jimm98y/MSSQLEXPRESS-M1-Install)
- [Direct download for SqlLocalDb.msi (v16)](https://download.microsoft.com/download/3/8/d/38de7036-2433-4207-8eae-06e247e17b25/SqlLocalDB.msi)

---

## Comments & Community Insights

The extensive comments section includes valuable community-sourced clarifications, alternative approaches, and direct feedback from developers successfully applying or extending these techniques, plus discussion about issues with Docker, automation, installer versions, and connection methods. For more, see the original article's comment thread.

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2024/Oct/24/Using-Sql-Server-on-Windows-ARM)
