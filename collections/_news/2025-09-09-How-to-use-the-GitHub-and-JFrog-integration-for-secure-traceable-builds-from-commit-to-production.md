---
layout: post
title: How to use the GitHub and JFrog integration for secure, traceable builds from commit to production
author: April Yoho
canonical_url: https://github.blog/enterprise-software/devsecops/how-to-use-the-github-and-jfrog-integration-for-secure-traceable-builds-from-commit-to-production/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-09-09 22:00:00 +00:00
permalink: /devops/news/How-to-use-the-GitHub-and-JFrog-integration-for-secure-traceable-builds-from-commit-to-production
tags:
- Artifact Promotion
- Artifactory
- Attestation
- Compliance
- Continuous Delivery
- Continuous Integration
- Dependabot
- DevOps
- DevSecOps
- Enterprise Software
- GitHub
- GitHub Actions
- JFrog
- News
- OIDC
- Provenance
- Security
- Security Scanning
- Supply Chain Security
- Workflow Automation
section_names:
- devops
- security
---
April Yoho explains how developers can leverage the new GitHub and JFrog integration to create a secure, traceable build process, with unified security scanning and centralized artifact management from commit to production.<!--excerpt_end-->

# How to use the GitHub and JFrog integration for secure, traceable builds from commit to production

Developers can now connect GitHub and JFrog to build secure, traceable CI/CD pipelines, streamlining artifact management and security compliance.

## Why this integration matters

Modern software delivery is a supply chain comprised of source code, CI/CD pipelines, and production artifacts. Every link in this chain should be secure, traceable, and automated. Historically, developers faced friction such as losing traceability after builds leave GitHub, splitting security scanning across platforms, and stitched-together pipelines.

This integration helps by:

- Cryptographically linking every commit and its artifact.
- Running unified, in-context security scans (such as Dependabot alerts prioritized with JFrog production context).
- Publishing and promoting artifacts as part of the GitHub Actions workflow, not as a separate manual step.
- Automatically generating and centralizing provenance attestations and SBOMs in JFrog.

## How it works

1. **Push code to GitHub.**
2. **Build and test with GitHub Actions.**
3. **Link commits, builds, and artifacts for full traceability.**
4. **Publish artifacts to JFrog Artifactory automatically.**
5. **Run integrated security scans:**
    - Code is scanned with GitHub Advanced Security.
    - Built artifacts are scanned with JFrog Xray.
6. **All attestations (provenance, SBOM, custom) are collected and associated in JFrog Evidence.**
7. **Artifacts are promoted through environments (dev, staging, prod) with policy-based security gates.**

## Setup instructions

1. Enable the GitHub integration in JFrog Artifactory:
   - Go to Administration → General Management → Manage Integrations → GitHub.
   - Toggle "Enable GitHub Actions", authenticate your organization, and select your token.
   - Create a pull request.
2. Trigger a build with your GitHub Actions workflow to build the artifact and generate the attestation.
   - Use actions like `jfrog/setup-jfrog-cli` and `actions/attest-build-provenance`.
   - Example workflow snippet:

     ```yaml
     - name: Attest docker image
       uses: actions/attest-build-provenance@v2
       with:
         subject-name: oci://${{ env.JF_REGISTRY }}/${{ env.IMAGE_NAME }}
         subject-digest: ${{ steps.build-and-push.outputs.digest }}
     ```

3. Artifacts with attestations are pushed to the JFrog Artifactory staging repo for validation.
4. JFrog validates the artifact by matching GitHub-signed provenance against trusted criteria.
5. On policy pass, JFrog promotes artifacts to production.
6. Dependabot continues to scan source repositories; critical vulnerabilities result in security alerts.
7. Artifacts in production can be filtered with tags like `artifact-registry:jfrog-artifactory`.
8. Artifact lifecycle data is synced back to GitHub through the artifact metadata API.

## Best practices

- Prefer **OIDC** (OpenID Connect) to avoid storing long-lived credentials in workflows.
- Automate artifact promotion through environments in Artifactory.
- Set up early security gates to block unauthenticated or vulnerable builds.
- Rely on JFrog's Evidence for fast attestation and traceability.

## What’s next

You can enable this integration today to strengthen your software supply chain security. See more:

- [JFrog integration guide](https://jfrog.com/help/r/jfrog-and-github-integration-guide/jfrog-for-github-dependabot)
- [GitHub documentation](https://docs.github.com/en/code-security/securing-your-organization/understanding-your-organizations-exposure-to-vulnerabilities/prioritizing-dependabot-alerts-using-production-context)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/enterprise-software/devsecops/how-to-use-the-github-and-jfrog-integration-for-secure-traceable-builds-from-commit-to-production/)
