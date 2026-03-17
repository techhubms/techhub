---
primary_section: devops
author: Allison
date: 2026-03-17 12:44:48 +00:00
feed_name: The GitHub Blog
external_url: https://github.blog/changelog/2026-03-17-code-quality-permissions-removed-from-security-manager-role
section_names:
- devops
- security
title: Code Quality permissions removed from security manager role
tags:
- Application Security
- Code Security
- DevOps
- GitHub
- GitHub Code Quality
- Governance
- Least Privilege
- News
- RBAC
- Repository Administration
- Repository Permissions
- Retired
- Role Based Access Control
- Security
- Security Manager Role
---

Allison reports that GitHub changed repository permissions so the security manager role can no longer enable or disable GitHub Code Quality unless they’re also a repository administrator, aligning the role more closely with least-privilege security responsibilities.<!--excerpt_end-->

## Summary

The **security manager** role in GitHub can **no longer enable or disable GitHub Code Quality** for a repository unless the user is also a **repository administrator**.

Only **repository administrators** can now enable or disable GitHub Code Quality in a repository.

## What changed

- **Before**: A user with the **security manager** role could enable/disable **GitHub Code Quality** for a repository.
- **Now**: The **security manager** role alone is not sufficient.
  - You must be a **repository administrator** (or also hold admin permissions) to enable/disable GitHub Code Quality.

## Rationale

This change keeps the **security manager** role focused on **security-related products** and follows the **principle of least privilege**.

## Documentation

- Managing Code Quality settings: [Managing GitHub Code Quality settings](https://docs.github.com/enterprise-cloud@latest/code-security/how-tos/maintain-quality-code/enable-code-quality)

## Image

![Social preview image from the GitHub changelog post](https://github.com/user-attachments/assets/648752f4-ce5e-4526-83f0-c7cb9a422bd7)


[Read the entire article](https://github.blog/changelog/2026-03-17-code-quality-permissions-removed-from-security-manager-role)

