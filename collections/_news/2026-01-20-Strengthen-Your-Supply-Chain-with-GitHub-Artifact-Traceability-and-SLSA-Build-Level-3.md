---
external_url: https://github.blog/changelog/2026-01-20-strengthen-your-supply-chain-with-code-to-cloud-traceability-and-slsa-build-level-3-security
title: Strengthen Your Supply Chain with GitHub Artifact Traceability and SLSA Build Level 3
author: Allison
feed_name: The GitHub Blog
date: 2026-01-20 16:46:13 +00:00
tags:
- Actions
- API
- Application Security
- Artifact Attestation
- Artifact Metadata
- Build Provenance
- CI/CD
- Code Security
- Defender For Cloud
- Dependabot
- DevSecOps
- GitHub
- Improvement
- JFrog Artifactory
- Runtime Risk
- Security Alerts
- SLSA
- Supply Chain Security
section_names:
- devops
- security
primary_section: devops
---
Allison details new ways developers can use GitHub’s artifact metadata APIs to link and analyze build artifacts, improving supply chain security and enabling production-aware alerting—with built-in integrations for Microsoft Defender for Cloud.<!--excerpt_end-->

# Strengthen Your Supply Chain with GitHub Artifact Traceability and SLSA Build Level 3

GitHub now allows you to link build artifacts, such as containers and binaries, to your repositories by adding storage and deployment context—even for artifacts not stored on GitHub. These capabilities deliver code-to-cloud traceability and help teams focus their security efforts on the workloads that are actually running in production environments.

## What's New

### Artifact Metadata APIs

- **Storage records**: Use GitHub’s REST API to record where a build artifact is stored (for example, in a package registry or external location).
- **Deployment records**: Track where each artifact is deployed, plus runtime risks like exposure to the internet or processing of sensitive data.
- These APIs are accessible from CI/CD workflows, custom CD tooling, or cloud runtime monitors.
- **Native integrations**: Microsoft Defender for Cloud (in public preview) and JFrog Artifactory natively support these APIs for seamless deployment and storage data capture.

### Unified Artifact View

- The new linked artifacts view in the Packages tab offers a comprehensive display of all associated artifacts with their attestations, storage locations, and deployment histories.
- If you use [artifact attestations](https://docs.github.com/actions/how-tos/secure-your-work/use-artifact-attestations/use-artifact-attestations), each artifact is cryptographically tied to its repository and build pipeline, supporting [SLSA Build Level 3](https://github.blog/enterprise-software/devsecops/enhance-build-security-and-reach-slsa-level-3-with-github-artifact-attestations/) supply chain security.

### Security Alert Contextualization

- Adding storage and deployment records enables filtering of Dependabot alerts, code scanning alerts, and security campaigns based on actual deployment—such as what's running in production, what’s internet-exposed, or where runtime risk is highest.
- Use filters like `artifact-registry`, `has:deployment`, `runtime-risk`, and combine them with the existing EPSS and CVSS-based filtering.
- This approach prioritizes the most relevant vulnerabilities and improves response efficiency.

## How To Link Artifacts

- **Attestation actions**: GitHub’s [`attest-build-provenance` action](https://github.com/marketplace/actions/attest-build-provenance) can automatically generate storage records.
- **Partner integrations**: Enable direct integrations with Microsoft Defender for Cloud and JFrog Artifactory for automated deployment/storage record creation.
- **REST API**: Directly upload artifact metadata records through REST endpoints, supporting all artifact origins.

## Additional Resources

- [About linked artifacts](https://docs.github.com/code-security/concepts/supply-chain-security/linked-artifacts)
- [Artifact metadata API reference](https://docs.github.com/rest/orgs/artifact-metadata)
- [Prioritize security alerts based on production context](https://docs.github.com/code-security/tutorials/secure-your-organization/prioritize-alerts-in-production-code)
- [Microsoft Defender for Cloud integration overview](https://learn.microsoft.com/azure/defender-for-cloud/github-advanced-security-overview)
- [GitHub and JFrog integration for traceable builds](https://github.blog/enterprise-software/devsecops/how-to-use-the-github-and-jfrog-integration-for-secure-traceable-builds-from-commit-to-production/)

Join the discussion on the official [GitHub Community announcements](https://github.com/orgs/community/discussions/categories/announcements).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-20-strengthen-your-supply-chain-with-code-to-cloud-traceability-and-slsa-build-level-3-security)
