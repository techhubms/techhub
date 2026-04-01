---
title: GitHub secret scanning — coverage update
tags:
- Application Security
- DevOps
- Figma SCIM Token
- GitHub Code Security
- GitHub Secret Scanning
- Google GCP API Key
- Improvement
- Langchain
- News
- npm Access Token
- Observation Mode Detectors
- OpenVSX Access Token
- PostHog OAuth Token
- Push Protection
- Salesforce Marketing Cloud
- Secret Detectors
- Secret Scanning Alerts
- Secret Scanning Partnership Program
- Secret Scanning Patterns
- Security
- Validity Checks
external_url: https://github.blog/changelog/2026-03-31-github-secret-scanning-nine-new-types-and-more
section_names:
- devops
- security
author: Allison
date: 2026-03-31 17:01:01 +00:00
primary_section: devops
feed_name: The GitHub Blog
---

Allison summarizes the latest GitHub secret scanning coverage updates, including new secret detectors, new validity checks for npm tokens, and expanded push protection defaults to help prevent committing credentials.<!--excerpt_end-->

## Summary

GitHub secret scanning has new and updated coverage across detectors, validators, and push protection defaults.

Key changes:

- Added **nine new secret detectors** from seven providers (including Langchain, Salesforce, and Figma).
- Added/expanded **push protection by default** for secrets from **Figma, Google, OpenVSX, and PostHog**.
- Added **validity checks** support for **npm** secrets (`npm_access_token`).

Related references:

- Recently added detectors (previous update): https://github.blog/changelog/2026-03-10-secret-scanning-pattern-updates-march-2026/
- Full list of supported secrets: https://docs.github.com/code-security/secret-scanning/introduction/supported-secret-scanning-patterns

## Detectors added

Secret scanning now automatically detects these new secret types:

| Provider | Secret type | Partner | User | Push protection |
| --- | --- | --- | --- | --- |
| Fieldguide | `fieldguide_api_token` | ✓ | ✓ | (configurable) |
| Figma | `figma_scim_token` | ✓ | ✓ | ✓ (default) |
| Flickr | `flickr_api_key` |  | ✓ | (configurable) |
| Hack Club | `hackclub_ai_api_key` |  | ✓ | (configurable) |
| Langchain | `langsmith_license_key` |  | ✓ | ✓ (default) |
| Langchain | `langsmith_scim_bearer_token` |  | ✓ | ✓ (default) |
| PostHog | `posthog_oauth_access_token` | ✓ | ✓ | (configurable) |
| PostHog | `posthog_oauth_refresh_token` | ✓ | ✓ | (configurable) |
| Salesforce | `salesforce_marketing_cloud_api_oauth2_token` |  | ✓ | ✓ (default) |

Notes:

- Detectors for **Drone CI, Netlify, Pydantic, and Twitch** are currently in **observation mode** and will be promoted to GA after validation.
- **Partner** secrets: when found in public repos, they’re automatically reported to the issuer via the partnership program.
  - Partnership program details: https://docs.github.com/code-security/secret-scanning/secret-scanning-partnership-program/secret-scanning-partner-program
- **User** secrets: generate secret scanning alerts in public or private repos.
  - About secret scanning: https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning

## Validators added

These secret types now support **validity checks** (automatic verification whether a detected secret is still active):

| Provider | Secret type |
| --- | --- |
| npm | `npm_access_token` |

Validity checks documentation:

- https://docs.github.com/code-security/secret-scanning/managing-alerts-from-secret-scanning/evaluating-alerts#checking-a-secrets-validity

## Push protection defaults

These existing detectors are now included in **push protection by default**:

| Provider | Secret type |
| --- | --- |
| Figma | `figma_scim_token` |
| Google | `google_gcp_api_key_bound_service_account` |
| OpenVSX | `openvsx_access_token` |
| PostHog | `posthog_personal_api_key` |

Additional details:

- Default push protection applies to all repos with secret scanning enabled, including **free public repositories**.
- Patterns marked as **configurable** can be enabled in push protection settings for GitHub secret scanning customers.
- Push protection documentation: https://docs.github.com/enterprise-cloud@latest/code-security/concepts/secret-security/about-push-protection

## Push protection configurability (UI improvement)

Starting today, **pattern type names** in the push protection pattern configurations UI will **link back to a filtered alert list view** for that type.

Feedback channel:

- GitHub Community (Code Security): https://github.com/orgs/community/discussions/categories/code-security

## Learn more

- Secret scanning overview: https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning
- Supported secret scanning patterns: https://docs.github.com/code-security/secret-scanning/introduction/supported-secret-scanning-patterns


[Read the entire article](https://github.blog/changelog/2026-03-31-github-secret-scanning-nine-new-types-and-more)

