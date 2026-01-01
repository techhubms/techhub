---
layout: "post"
title: "GitHub Releases Introduce Immutability for Enhanced Supply Chain Security"
description: "This news update announces GitHub's new public preview feature enabling immutable releases. The feature improves supply chain security by locking assets and tags after publication, introducing cryptographic release attestations, and supporting Sigstore for verification, ultimately protecting software distributions from tampering and supply chain attacks."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-26-releases-now-support-immutability-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-26 20:33:53 +00:00
permalink: "/2025-08-26-GitHub-Releases-Introduce-Immutability-for-Enhanced-Supply-Chain-Security.html"
categories: ["DevOps", "Security"]
tags: ["Artifact Verification", "CI/CD", "DevOps", "DevOps Security", "GitHub", "Immutable Releases", "News", "Release Attestation", "Release Automation", "Repository Management", "Security", "Sigstore", "Software Distribution", "Supply Chain Security", "Tag Protection"]
tags_normalized: ["artifact verification", "cislashcd", "devops", "devops security", "github", "immutable releases", "news", "release attestation", "release automation", "repository management", "security", "sigstore", "software distribution", "supply chain security", "tag protection"]
---

Allison reports on GitHub's rollout of immutable releases in public preview, highlighting new supply chain security measures, tag and asset protection, and cryptographic attestations for artifact verification.<!--excerpt_end-->

# GitHub Releases Introduce Immutability for Enhanced Supply Chain Security

GitHub has announced that releases now support immutability in public preview, marking a significant advancement in supply chain security for developers and organizations.

## Key Features of Immutable Releases

- **Immutable Assets:** Once a release is published as immutable, its assets cannot be added, altered, or removed. This safeguards distributed artifacts from supply chain tampering.
- **Tag Protection:** Tags associated with new immutable releases are locked down and cannot be deleted or changed, reinforcing the trustworthiness of software versions.
- **Release Attestations:** Every immutable release is accompanied by a signed attestation, enabling users and automated systems to verify authenticity and integrity even outside GitHub.

## How to Enable Immutable Releases

- Enable at either the repository or organization level within your GitHub settings.
- Once enabled, all new releases are immutable by default. Existing releases remain mutable unless republished.
- If immutability is later disabled, any releases made during the enabled period remain immutable and protected.

## Verifying Releases and Assets

- Release attestations are based on the [Sigstore bundle format](https://docs.sigstore.dev/about/bundle/), supporting integration with Sigstore-compatible tools and policy automation in CI/CD pipelines.
- Developers can use the GitHub CLI to check the validity of releases and assets:

  ```sh
  gh release verify <tag>
  gh release verify-asset <tag> <asset>
  ```

## Additional Resources

- [Immutable releases documentation](https://docs.github.com/code-security/supply-chain-security/understanding-your-software-supply-chain/immutable-releases)
- [Discuss or leave feedback in the GitHub Community](https://gh.io/AAxpck6)

> **Note:** Immutable releases are being gradually deployed across all repositories and organizations.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-26-releases-now-support-immutability-in-public-preview)
