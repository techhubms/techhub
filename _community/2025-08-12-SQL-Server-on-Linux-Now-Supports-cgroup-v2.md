---
layout: "post"
title: "SQL Server on Linux Now Supports cgroup v2"
description: "This post details the new support for cgroup v2 in SQL Server 2025 preview and SQL Server 2022 CU 20, enabling improved memory and CPU resource management for Linux-based containerized deployments. Covering implementation steps, problem analysis, and real-world effects on Kubernetes, this content is valuable for DBAs and platform engineers."
author: "Attinder_Pal_Singh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/sql-server-blog/sql-server-on-linux-now-supports-cgroup-v2/ba-p/4433523"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-12 12:30:37 +00:00
permalink: "/2025-08-12-SQL-Server-on-Linux-Now-Supports-cgroup-v2.html"
categories: ["Azure", "DevOps"]
tags: ["AKS", "Azure", "Cgroup", "Cgroup V2", "Community", "Container Orchestration", "Containerization", "DevOps", "Docker", "Errorlog", "Kubernetes", "Linux", "Memory Limits", "OpenShift", "Platform Engineering", "Quality Of Service", "Resource Enforcement", "Resource Management", "SQL Server", "SQL Server CU 20", "SQL Server On Linux"]
tags_normalized: ["aks", "azure", "cgroup", "cgroup v2", "community", "container orchestration", "containerization", "devops", "docker", "errorlog", "kubernetes", "linux", "memory limits", "openshift", "platform engineering", "quality of service", "resource enforcement", "resource management", "sql server", "sql server cu 20", "sql server on linux"]
---

Attinder Pal Singh gives a practical look at cgroup v2 support in SQL Server 2025 preview and SQL Server 2022 CU 20 on Linux, helping DBAs and engineers adopt better resource controls and avoid OOM errors on Kubernetes and other container platforms.<!--excerpt_end-->

# Smarter Resource Management for Containers and Beyond

**Authors:** Engineering: Andrew Carter (Lead), Nicolas Blais-Miko. Product Manager: Attinder Pal Singh and Amit Khandelwal

SQL Server now supports **cgroup v2** on Linux with SQL Server 2025 preview and SQL Server 2022 CU 20. This improvement means more accurate, reliable resource management for SQL Server instances—especially those running inside containers on platforms like Docker, Kubernetes, Azure Kubernetes Service (AKS), and OpenShift.

## Why Does cgroup v2 Matter?

Linux control groups (cgroups) let you set, enforce, and monitor resource usage (CPU, memory) for processes and containers. Version 2 of cgroup provides:

- Unified hierarchy
- Stricter resource isolation
- Reliable enforcement and better metrics

[Read more on cgroup v2 in the Linux Kernel docs.](https://www.kernel.org/doc/html/latest/admin-guide/cgroup-v2.html)

## How to Check Your cgroup Version

Run this command:

```sh
stat -fc %T /sys/fs/cgroup/
```

- If the result is `cgroup2fs`, you're running cgroup v2
- If it is `cgroup`, you're running cgroup v1

## Switching to cgroup v2

- Use a current Linux distro with cgroup v2 enabled by default
- To enable manually:
  1. Add `systemd.unified_cgroup_hierarchy=1` to GRUB
  2. Run `sudo update-grub`

## Real-World Scenario: SQL Server in Kubernetes

**Before SQL Server 2022 CU 20:** SQL Server containers sometimes exceeded memory limits, leading to out-of-memory (OOM) errors, despite those limits being set. Example:

- Node: 4 CPUs, 16 GB RAM
- Pod config: memory limited to 3Gi
- SQL Server still saw and used up to ~12.8GB (80% of node total), risking OOM

**Errorlog snippet:**
> Microsoft SQL Server 2022 (RTM-CU19)...
> Detected 12792 MB of RAM, 12313 MB of available memory...

## Why Only 80% Memory?

By default, SQL Server's `memory.memorylimit` parameter is set to 80% physical memory to help prevent host OOM. [See official docs.](https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-configure-mssql-conf?view=sql-server-ver17#set-the-memory-limit)

## What Changes With cgroup v2 Support?

Starting in SQL Server 2022 CU 20 and SQL Server 2025 preview, the memory limits set by the orchestration platform (Kubernetes, Docker, etc.) are now respected reliably.

- SQL Server now sees the correct per-container memory, based on cgroup v2 limits.
- Example errorlog with 3Gi limit:

> Microsoft SQL Server 2022 (RTM-CU20)...
> Detected 2458 MB of RAM, 1932 MB of available memory...

This reflects 80% of 3Gi—so SQL Server fully honors resource control in containerized environments on Linux.

## How to Validate Your Deployment

- Check your cgroup version as above
- Confirm SQL Server errorlog detects the correct RAM
- Container resource config: `kubectl get pod ... -o jsonpath="{.status.qosClass}{.spec.containers[*].resources.limits.memory}"`

## Learn More

- [SQL Server on Linux Overview](https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-overview?view=sql-server-ver17)
- [SQL Server 2025 Release Notes](https://learn.microsoft.com/en-us/sql/sql-server/sql-server-2025-release-notes?view=sql-server-ver17)
- [Deploy SQL Server Linux container to Kubernetes](https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-containers-deploy-helm-charts-kubernetes?view=sql-server-ver17)
- [Deploy SQL Server on OpenShift or Kubernetes](https://learn.microsoft.com/en-us/sql/linux/quickstart-sql-server-containers-azure?view=sql-server-ver17&tabs=kubectl)
- [Kubernetes cgroup v2 docs](https://kubernetes.io/docs/concepts/architecture/cgroups/)

## Summary

The cgroup v2 support in SQL Server for Linux enables smarter, predictable Quality of Service enforcement, especially for containerized solutions. DBAs and DevOps engineers deploying SQL Server on AKS or on-premises now have greater confidence in resource isolation and platform resilience.

---

**Authors:** Andrew Carter, Nicolas Blais-Miko, Attinder Pal Singh, Amit Khandelwal

_Last updated: Aug 12, 2025_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/sql-server-blog/sql-server-on-linux-now-supports-cgroup-v2/ba-p/4433523)
