---
layout: "post"
title: "npm Bulk Trusted Publishing and Script Security Features Released"
description: "This news highlights the release of two significant security and configuration features in npm CLI v11.10.0+: bulk configuration for OIDC trusted publishing and new controls for git dependencies using the --allow-git flag. These updates help package maintainers securely publish and manage scripts at scale within the npm ecosystem, addressing supply chain security concerns."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-18-npm-bulk-trusted-publishing-config-and-script-security-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-18 18:11:24 +00:00
permalink: "/2026-02-18-npm-Bulk-Trusted-Publishing-and-Script-Security-Features-Released.html"
categories: ["DevOps", "Security"]
tags: ["Configuration", "Dependency Management", "DevOps", "Git Dependencies", "GitHub", "News", "Node.js", "npm", "npm CLI", "OIDC", "Package Management", "Script Security", "Security", "Software Supply Chain", "Supply Chain Security", "Trusted Publishing"]
tags_normalized: ["configuration", "dependency management", "devops", "git dependencies", "github", "news", "nodedotjs", "npm", "npm cli", "oidc", "package management", "script security", "security", "software supply chain", "supply chain security", "trusted publishing"]
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
