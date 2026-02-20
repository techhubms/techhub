---
external_url: https://github.blog/changelog/2026-02-18-npm-bulk-trusted-publishing-config-and-script-security-now-generally-available
title: npm Bulk Trusted Publishing and Script Security Features Released
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-18 18:11:24 +00:00
tags:
- Configuration
- Dependency Management
- DevOps
- Git Dependencies
- GitHub
- News
- Node.js
- npm
- npm CLI
- OIDC
- Package Management
- Script Security
- Security
- Software Supply Chain
- Supply Chain Security
- Trusted Publishing
section_names:
- devops
- security
---
Allison presents new npm CLI features for secure trusted publishing and improved script security, with practical steps for maintainers and developers.<!--excerpt_end-->

# npm Bulk Trusted Publishing and Script Security Now Generally Available

*Authored by Allison*

Two major features have shipped in [npm CLI v11.10.0+](https://docs.npmjs.com/cli/v11/):

## 1. Bulk OIDC Trusted Publishing Configuration

- Maintainers can now add or update OpenID Connect (OIDC) trusted publishing configs across multiple npm packages at once using [`npm trust`](https://docs.npmjs.com/cli/v11/commands/npm-trust) instead of repeating manual steps per package.

**Key Advantage:**

- Simplifies setup for organizations managing many packages
- Streamlines configuration updates for trusted publishing

## 2. New `--allow-git` Flag for Script Security

- Git dependencies in npm could previously include `.npmrc` files overriding the `git` executable path, creating a risk of arbitrary code execution—sometimes even bypassing `--ignore-scripts`.
- The new `--allow-git` flag introduces explicit control. By default, it is set to `all` for backwards compatibility, but strict usage is recommended:

```shell
npm install --allow-git=none
```

- This disables git dependency execution unless explicitly allowed. It will become the default in npm v12.
- Maintainers and developers are urged to update automation and install processes accordingly for improved supply chain security.

For further reading and community feedback, see the [npm documentation](https://docs.npmjs.com/cli/v11/commands/npm-install#allow-git) and join the [GitHub Community discussion](https://github.com/orgs/community/discussions/187403).

---

**Summary:**
These enhancements reflect npm's focus on proactive package security and trusted automation. Bulk trusted publishing enables easier OIDC adoption across projects, while `--allow-git=none` fortifies the ecosystem against supply chain attacks.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-18-npm-bulk-trusted-publishing-config-and-script-security-now-generally-available)
