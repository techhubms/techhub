---
layout: post
title: Implementing SSSD with Microsoft Entra ID for Azure Linux HPC Clusters
author: trcooper
canonical_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/use-entra-ids-to-run-jobs-on-your-hpc-cluster/ba-p/4457932
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-29 17:50:46 +00:00
permalink: /azure/community/Implementing-SSSD-with-Microsoft-Entra-ID-for-Azure-Linux-HPC-Clusters
tags:
- Ansible
- App Registration
- Azure
- Azure Linux
- Cluster Configuration
- Community
- Graph API
- HPC
- Identity Management
- Linux Authentication
- Microsoft Entra ID
- MPI
- NSSwitch
- OAuth2
- OpenID Connect
- RPM
- Security
- Slurm
- SSSD
- User Provisioning
section_names:
- azure
- security
---
trcooper explains step-by-step how to integrate Microsoft Entra ID with SSSD on Azure Linux 3.0 HPC clusters, providing secure, unified user identities and covering deployment, configuration, and real-world usage details.<!--excerpt_end-->

# Implementing SSSD with Microsoft Entra ID for Azure Linux HPC Clusters

**Author:** trcooper  
**Last updated:** Sep 29, 2025

## Overview

This guide demonstrates the practical implementation of System Security Services Daemon (SSSD) with the new 'idp' provider for integrating Microsoft Entra ID (formerly Azure Active Directory) with Azure Linux 3.0 HPC clusters. The result is centrally managed, consistent usernames, UIDs, and GIDs across the cluster, all rooted in your Entra tenant.

## Why Integrate Entra ID with SSSD on HPC?

- Consistent user and group identities across nodes
- Centralized cloud-based identity provider using industry standards (OAuth2/OpenID Connect)
- Simplifies user management for high-performance computing (HPC) environments

## Implementation Steps

### 1. Build SSSD RPMs for Azure Linux 3.0

- Source SSSD 2.11.0 release from [SSSD GitHub releases](https://github.com/SSSD/sssd/releases).
- Install required build dependencies using `tdnf`, adding extra packages as needed.
- Use the generic SSSD spec file and resolve missing dependencies (some from Fedora 42 RPMs).
- Build and assemble the RPM set needed for Azure Linux 3.0 HPC.

```sh
# Example steps (executed on Azure Linux 3.0 VM)

sudo tdnf -y install [build dependencies]
wget https://github.com/SSSD/sssd/releases/download/2.11.0/sssd-2.11.0.tar.gz
tar -xvf sssd-2.11.0.tar.gz; cd sssd-2.11.0
autoreconf -if
./configure --enable-nsslibdir=/lib64 [other options]
make
sudo make rpms  # Handles most of the build; finalize with rpmbuild as needed
```

*Note*: Some dependencies must be fetched and installed manually from Fedora repos.

### 2. Deploy RPMs to the Cluster

- Use Ansible to copy and install the freshly built RPMs onto all cluster nodes.
- Integrate SSSD installation steps into your customized cluster images or deployment playbooks.

```yaml
- name: Copy SSSD RPMs
tasks:
  - ansible.builtin.copy:
      src: sssd-2.11.0/
      dest: /tmp/sssd/
  - ansible.builtin.shell: |
      tdnf -y install /tmp/sssd/*.rpm
```

### 3. Register an Application in Entra

- In your Microsoft Entra admin portal, create an App Registration with relevant API permissions.
- Grant the app Directory Readers role via the Graph API:

```http
POST https://graph.microsoft.com/v1.0/roleManagement/directory/roleAssignments
{
  "principalId": "<your app's objectId>",
  "roleDefinitionId": "<Directory Readers role definition id>",
  "directoryScopeId": "/"
}
```

- Record the Application (client) ID and secret for SSSD configuration.

### 4. Configure SSSD and nsswitch

- Use cloud-init or configuration management to distribute `sssd.conf` and update `nsswitch.conf`.
- Example `/etc/sssd/sssd.conf`:

```ini
[sssd]
config_file_version = 2
services = nss, pam
domains = mydomain.onmicrosoft.com

[domain/mydomain.onmicrosoft.com]
id_provider = idp
idp_type = entra_id
idp_client_id = <APP_CLIENT_ID>
idp_client_secret = <APP_SECRET>
idp_token_endpoint = https://login.microsoftonline.com/<TENANT_ID>/oauth2/v2.0/token
idp_userinfo_endpoint = https://graph.microsoft.com/v1.0/me
idp_device_auth_endpoint = https://login.microsoftonline.com/<TENANT_ID>/oauth2/v2.0/devicecode
idp_id_scope = https%3A%2F%2Fgraph.microsoft.com%2F.default
idp_auth_scope = openid profile email
auto_private_groups = true
use_fully_qualified_names = false
cache_credentials = true
entry_cache_timeout = 5400
entry_cache_nowait_percentage = 50
refresh_expired_interval = 4050
enumerate = false
debug_level = 2

[nss]
debug_level = 2
default_shell = /bin/bash
fallback_homedir = /shared/home/%u

[pam]
debug_level = 2
```

- Example `/etc/nsswitch.conf`:

```
passwd: files sss
group: files sss
shadow: files sss
hosts: files dns

# etc.
```

### 5. Create User Home Directories

- Device Auth isn't supported for SSH, so public key auth is currently required.
- Pre-create user home directories and set up keys using SSSD-provided POSIX info:

```bash
# !/bin/bash

# Usage: {{CONTENT}} username "ssh-rsa ..."

USER_NAME=$1
USER_PUBKEY=$2
entry=$(getent passwd "$USER_NAME")
USER_UID=$(echo "$entry" | awk -F: '{print $3}')
USER_HOME=$(echo "$entry" | awk -F: '{print $6}')
if [ ! -d "$USER_HOME" ]; then
  mkdir -p "$USER_HOME"
  chown $USER_UID:$USER_UID $USER_HOME
  chmod 700 $USER_HOME
  cp -r /etc/skel/. $USER_HOME
  mkdir -p $USER_HOME/.ssh
  chmod 700 $USER_HOME/.ssh
  touch $USER_HOME/.ssh/authorized_keys
  chmod 644 $USER_HOME/.ssh/authorized_keys
  echo "$USER_PUBKEY" >> $USER_HOME/.ssh/authorized_keys
  # (Optional) Setup passwordless SSH config, keypair, etc.
fi
```

### 6. Run Jobs as Entra Users

- After setup, users authenticated via Entra ID can run jobs in the cluster using their mapped POSIX accounts.
- Example usage:

```sh
john.doe@tst4-login-0 [ ~ ]$ id
uid=1137116670(john.doe) gid=1137116670(john.doe) groups=1137116670(john.doe)

# Running MPI job

sbatch -p hbv4 /cvmfs/az.pe/1.2.6/tests/imb/imb-env-intel-oneapi.sh
```

## Summary

Using the new SSSD 'idp' provider, you can now manage user identities in Azure Linux HPC clusters directly from Microsoft Entra ID. This approach streamlines authentication, improves user experience, and leverages native Azure and Microsoft 365 directory integration with open Linux infrastructure.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/use-entra-ids-to-run-jobs-on-your-hpc-cluster/ba-p/4457932)
