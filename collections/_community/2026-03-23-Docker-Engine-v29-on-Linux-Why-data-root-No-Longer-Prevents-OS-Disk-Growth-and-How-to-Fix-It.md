---
author: vdivizinschi
tags:
- /var/lib/containerd
- /var/lib/docker
- Azure
- Azure Batch
- Azure Virtual Machines
- Cloud Infrastructure
- Community
- Containerd
- Containerd Image Store
- Content Store
- Daemon.json
- Data Root
- DevOps
- Disk Usage
- Docker Engine 29
- Docker Engine V29
- Linux
- Node Reimage
- OS Disk Growth
- Snapshotters
- Symbolic Links
- Systemctl
- Systemd
- VM Scale Sets
external_url: https://techcommunity.microsoft.com/t5/azure/docker-engine-v29-on-linux-why-data-root-no-longer-prevents-os/m-p/4504862#M22466
section_names:
- azure
- devops
primary_section: azure
date: 2026-03-23 18:17:37 +00:00
title: 'Docker Engine v29 on Linux: Why data-root No Longer Prevents OS Disk Growth (and How to Fix It)'
feed_name: Microsoft Tech Community
---

vdivizinschi explains why Docker Engine v29 on Linux can still fill the OS disk even when Docker’s data-root points to a mounted data disk, and shows a practical workaround to relocate containerd storage so images and snapshots stop landing under /var/lib/containerd.<!--excerpt_end-->

## Scope

- Applies to **Linux hosts only**
- Does **not** apply to Windows or Docker Desktop

## Problem summary

After upgrading to **Docker Engine v29** or reimaging Linux nodes with this version, you may observe **unexpected growth on the OS disk**, even when Docker is configured with a custom `data-root` pointing to a mounted data disk.

This often shows up in cloud environments where the OS disk is intentionally small and container data is expected to live on a separate disk, such as:

- VM Scale Sets (VMSS)
- Azure Batch
- Self-managed Linux VMs

## What changed in Docker Engine v29 (Linux)

Starting with **Docker Engine 29.0**, **containerd’s image store is the default storage backend on fresh installations**.

Docker documents this explicitly:

> “The containerd image store is the default storage backend for Docker Engine 29.0 and later on fresh installations.”

Reference: https://docs.docker.com/engine/storage/containerd/

Key points on Linux:

- Docker delegates **image and snapshot storage** to **containerd**
- containerd uses its own content store and snapshotters
- Docker’s traditional `data-root` setting **no longer controls all container storage**

Docker Engine v29 was released on **11 November 2025**, and this behavior is **by design**, not a regression.

## Where disk usage goes on Linux

Docker’s daemon documentation describes the split:

- **Legacy storage (pre-v29 or upgraded installs):**
  - All data under `/var/lib/docker`
- **Docker Engine v29 (containerd image store enabled):**
  - Images & snapshots → `/var/lib/containerd`
  - Other Docker data (volumes, configs, metadata) → `/var/lib/docker`

Crucial note from Docker:

> “The data-root option does not affect image and container data stored in /var/lib/containerd when using the containerd image store.”

Reference: https://docs.docker.com/engine/daemon/

This explains why OS disk usage can keep growing even when `data-root` is set to a data disk.

## Why the old configuration worked before

On earlier Docker versions, Docker fully managed image and snapshot storage. Setting:

```json
{
  "data-root": "/mnt/docker-data"
}
```

was sufficient to redirect **all container storage** off the OS disk.

With Docker Engine v29:

- containerd owns image and snapshot storage
- `data-root` only affects Docker-managed data
- OS disk growth after upgrades or reimages is **expected behavior**

## Linux workaround: redirect containerd storage

To keep **both Docker and containerd storage** on the mounted data disk, containerd’s storage path must also be redirected.

One practical workaround is to relocate `/var/lib/containerd` using a symbolic link.

### Example (Linux)

```bash
sudo systemctl stop docker.socket docker containerd || true;
sudo mkdir -p /mnt/docker-data /mnt/containerd;
sudo rm -rf /var/lib/containerd;
sudo ln -s /mnt/containerd /var/lib/containerd;
echo "{\"data-root\": \"/mnt/docker-data\"}" | sudo tee /etc/docker/daemon.json;
sudo systemctl daemon-reload;
sudo systemctl start containerd docker'
```

## What this does

- Stops Docker and containerd
- Creates container storage directories on the mounted data disk
- Redirects `/var/lib/containerd` → `/mnt/containerd`
- Keeps Docker’s `data-root` at `/mnt/docker-data`
- Restarts services with a unified storage layout

## Key takeaways

- Docker Engine v29 introduces a **fundamental storage architecture change on Linux**
- `data-root` alone is no longer sufficient
- OS disk growth after upgrades or reimages is **expected**
- containerd storage must also be redirected
- The workaround matches Docker’s documented design

## References

- Docker daemon data directory
  - https://docs.docker.com/engine/daemon/
- containerd image store (Docker Engine v29)
  - https://docs.docker.com/engine/storage/containerd/
- Docker Engine v29 release notes
  - https://docs.docker.com/engine/release-notes/29/


[Read the entire article](https://techcommunity.microsoft.com/t5/azure/docker-engine-v29-on-linux-why-data-root-no-longer-prevents-os/m-p/4504862#M22466)

