---
layout: "post"
title: "Ubuntu-Based Runtimes Coming to Azure App Service for Linux"
description: "This post from TulikaC provides details on the upcoming shift in Azure App Service for Linux, where new major versions of supported stacks (.NET 10, Python 3.14, Node 24, PHP 8.5, Java 25) will now be built on Ubuntu LTS images rather than Debian. The post highlights platform benefits, migration guidance, and operational impacts, with practical info for developers deploying code-based web applications on Azure."
author: "TulikaC"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/ubuntu-powered-runtimes-on-azure-app-service-for-linux-leaner/ba-p/4465414"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-30 04:00:28 +00:00
permalink: "/community/2025-10-30-Ubuntu-Based-Runtimes-Coming-to-Azure-App-Service-for-Linux.html"
categories: ["Azure", "Coding"]
tags: [".NET 10", "App Deployment", "Azure", "Azure App Service", "Azure CLI", "Coding", "Community", "Debian", "GitHub Actions", "Java 25", "Linux", "Node 24", "PHP 8.5", "Platform Updates", "Python 3.14", "Runtime Environments", "Ubuntu", "Web Applications"]
tags_normalized: ["dotnet 10", "app deployment", "azure", "azure app service", "azure cli", "coding", "community", "debian", "github actions", "java 25", "linux", "node 24", "php 8dot5", "platform updates", "python 3dot14", "runtime environments", "ubuntu", "web applications"]
---

TulikaC explains how Azure App Service for Linux is transitioning its new application runtimes to Ubuntu LTS, outlining the changes for developers and the operational benefits of this update.<!--excerpt_end-->

# Ubuntu-Powered Runtimes on Azure App Service for Linux: Leaner, Faster, Stronger

**Author:** TulikaC

Azure App Service for Linux is making an important OS foundation update: all new major versions of supported code-based stacks—including **.NET 10, Python 3.14, Node 24, PHP 8.5, and Java 25**—will now use Ubuntu-based images. Existing application deployments will remain on Debian, and there will be no forced migration.

## Why Ubuntu?

- **Debian Ecosystem Benefits:** Ubuntu builds on Debian, benefiting from its large library of packages while allowing faster adoption of upstream improvements. This enables more predictable adoption of new toolchains and libraries, which maximizes compatibility and supports modern dependencies.
- **LTS Stability:** Ubuntu LTS offers a five-year support cycle, providing a stable base for operating services at scale.

## What's Changing, What's Not

### What's Changing

- New code-based stacks for major frameworks (.NET, Python, Node, PHP, Java) will now default to **Ubuntu** images.

### What's Not Changing

- Existing apps deployed with earlier versions will stay on Debian. There’s no need for action unless you wish to upgrade.
- **Operational parity** remains: deployment methods (Oryx, GitHub Actions, Azure CLI), scaling, diagnostics, and networking all continue as before.

## Why This Matters

- When creating new apps or upgrading to the latest major stacks, you'll receive the Ubuntu runtime by default.
- For teams upgrading, it may be necessary to verify native dependencies, as package availability and naming can differ between Debian and Ubuntu. In most cases, Ubuntu offers equal or newer versions.

## FAQ Highlights

- **Immediate migration is not required.** Existing apps remain on Debian unless you opt in.
- **Build and runtime behaviors are expected to be neutral or improved**, with smaller images and refreshed toolchains meaning faster builds and potentially fewer cold starts.
- **No major breaking changes are anticipated** for supported frameworks, though verifying native package support is recommended if your deployment pins to specific distro versions.

By moving to Ubuntu LTS for new stacks, developers enjoy the reliability and scale of Debian foundations plus a faster update cadence and expanded security support—without extra effort.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/ubuntu-powered-runtimes-on-azure-app-service-for-linux-leaner/ba-p/4465414)
