---
layout: "post"
title: "Dependabot Adds Support for Conda Environment Files"
description: "This news update announces that Dependabot now supports parsing and updating `environment.yml` Conda files for Python projects. It highlights automated security and version updates, the importance for teams managing dependencies with Conda, and how the feature complements supply chain security on GitHub. The release applies to users of github.com and is coming to GitHub Enterprise Server soon."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-16-conda-ecosystem-support-for-dependabot-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-16 18:01:54 +00:00
permalink: "/news/2025-09-16-Dependabot-Adds-Support-for-Conda-Environment-Files.html"
categories: ["DevOps", "Security"]
tags: ["Conda", "Dependabot", "Dependency Graph", "Dependency Management", "Dependency Submission API", "DevOps", "Environment.yml", "GitHub", "GitHub Enterprise Server", "Improvement", "News", "pip", "Python", "Security", "Security Updates", "Software Supply Chain", "Supply Chain Security", "Vulnerability Management"]
tags_normalized: ["conda", "dependabot", "dependency graph", "dependency management", "dependency submission api", "devops", "environmentdotyml", "github", "github enterprise server", "improvement", "news", "pip", "python", "security", "security updates", "software supply chain", "supply chain security", "vulnerability management"]
---

Allison announces that Dependabot now supports Conda environment files, offering automated security and version updates for Conda-based Python projects and enhancing supply chain security on GitHub.<!--excerpt_end-->

# Dependabot Adds Support for Conda Environment Files

Dependabot can now parse and update `environment.yml` Conda environment files, adding both security and version update capabilities for Python projects using Conda. This update enables automated dependency management and vulnerability detection for teams relying on Conda-managed environments.

## Why It Matters

Many Python teams use Conda to manage project dependencies and isolated environments. With Dependabot's new support, developers using Conda benefit from:

- **Automated Security Alerts:** Get notified about vulnerabilities in Python packages from the pip [GitHub Advisory database](https://github.com/advisories).
- **Automated Dependency Updates:** Receive pull requests with the latest dependency versions based on your `environment.yml`.
- **Greater Supply Chain Security:** Reduce risk by ensuring environments are kept up-to-date and patched.

## How It Works

- Dependabot detects Conda environment files (`environment.yml`) in repositories and creates automated pull requests for updates.
- Security alerts require submission of a dependency graph using the [Dependency Submission API](https://docs.github.com/code-security/supply-chain-security/understanding-your-software-supply-chain/using-the-dependency-submission-api).
- Only Python packages dependencies are supported. Security advisories are sourced from GitHub's pip advisory database.

## Availability

- **Available Today:** GitHub.com users can leverage this feature now.
- **Coming Soon:** GitHub Enterprise Server (GHES) support begins with version 3.20.

For further information, documentation and examples are available in the [Dependabot documentation](https://docs.github.com/code-security/dependabot/ecosystems-supported-by-dependabot/supported-ecosystems-and-repositories#supported-ecosystems-and-repositories). Join the ongoing conversation in the [Dependabot Community](https://github.com/dependabot/dependabot-core/issues/2227).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-16-conda-ecosystem-support-for-dependabot-now-generally-available)
