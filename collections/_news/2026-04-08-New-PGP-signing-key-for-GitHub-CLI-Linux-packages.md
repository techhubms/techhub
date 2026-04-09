---
primary_section: devops
tags:
- Apt
- Client Apps
- Conda
- DevOps
- Dnf
- Docker
- Gh
- GitHub CLI
- GPG
- Homebrew
- Improvement
- Key Rotation
- Linux Packages
- News
- Package Repository
- PGP Keyring
- PGP Signing Key
- Precompiled Binaries
- Repository Signing
- Security
- Supply Chain Security
- Yum
external_url: https://github.blog/changelog/2026-04-08-new-pgp-signing-key-for-github-cli-linux-packages
section_names:
- devops
- security
feed_name: The GitHub Blog
author: Allison
title: New PGP signing key for GitHub CLI Linux packages
date: 2026-04-08 16:13:36 +00:00
---

Allison announces an updated PGP keyring for GitHub CLI (gh) Linux package repositories, adding a replacement signing key and explaining who needs to rerun install steps before the current key expires on September 5, 2026.<!--excerpt_end-->

# New PGP signing key for GitHub CLI Linux packages

We’ve published an updated PGP keyring for GitHub CLI’s Linux package repositories. The keyring now includes both the current signing key and a new replacement key.

## Who is already covered

You don’t need to take any action if either of these is true:

- You already re-ran the Linux installation steps since the new keyring was published.
- You’re on Windows/macOS.
- You built GitHub CLI from source.
- You installed `gh` via:
  - Homebrew
  - Conda
  - Precompiled binaries

## What to do (Linux package installs)

The current signing key expires on **September 5, 2026**.

If you install or update `gh` via `apt`, `yum`, or `dnf`, rerun the installation steps for your distribution **before the expiry** to pick up the new keyring and avoid disruption.

- Linux installation steps: https://github.com/cli/cli/blob/trunk/docs/install_linux.md

## More details

For detailed instructions and Docker guidance, see the full announcement:

- https://github.com/cli/cli/issues/13118

![Social image related to the announcement](https://github.com/user-attachments/assets/223e61f7-aa7b-47f4-b7ea-e29178210a5b)


[Read the entire article](https://github.blog/changelog/2026-04-08-new-pgp-signing-key-for-github-cli-linux-packages)

