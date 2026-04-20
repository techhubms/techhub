---
primary_section: devops
tags:
- Application Security
- Audit Logs
- Cloudflare
- Custom Patterns
- Detection Patterns
- DevOps
- DevSecOps
- Dismissal Requests API
- Enterprise Managed Users (emu)
- Figma
- Fork Inheritance
- GitHub Advanced Security
- GitHub Secret Scanning
- Google Cloud (gcp)
- Improvement
- LangChain
- News
- OpenVSX
- PostHog
- Provider Filtering
- Push Protection
- Scan History API
- Secret Protection
- Secret Scanning Alerts API
- Secret Scanning Campaigns
- Security
- Webhooks
title: Secret scanning pattern updates and product improvements
external_url: https://github.blog/changelog/2026-04-14-secret-scanning-pattern-updates-and-product-improvements
author: Allison
feed_name: The GitHub Blog
date: 2026-04-14 17:17:45 +00:00
section_names:
- devops
- security
---

Allison summarizes GitHub Secret Scanning updates that expand push protection defaults, improve enterprise fork coverage, and add new API capabilities for alert validity, provider filtering, scan history, and enterprise-wide dismissal request reporting.<!--excerpt_end-->

# Secret scanning pattern updates and product improvements

This update covers improvements to GitHub Secret Scanning detection coverage, push protection behavior, and APIs/workflows aimed at developer and security teams.

## Detection coverage

- **New Cloudflare detectors**: Cloudflare is now a secret scanning partner.

## Push protection

### Forks for enterprise-managed users (EMU)

- In **Enterprise Managed Users (EMU)** environments, developers often fork org repos into personal namespaces.
- Previously, **push protection** only applied to a fork if the fork itself had an active **GHAS** license, which could allow secrets blocked in the org repo to slip through in forks.
- **New behavior**: push protection now walks the **ancestor chain**.
  - If any repository in the fork hierarchy has push protection enabled, all forks beneath it inherit that protection.

### Push protection defaults expanded

Additional patterns now **block commits by default** when secret scanning is enabled (including free public repositories):

- Cloudflare: `cloudflare_account_api_token`
- Cloudflare: `cloudflare_global_user_api_key`
- Cloudflare: `cloudflare_user_api_token`
- Figma: `figma_scim_token`
- Google: `google_gcp_api_key_bound_service_account`
- Langchain: `langsmith_license_key`
- Langchain: `langsmith_scim_bearer_token`
- OpenVSX: `openvsx_access_token`
- PostHog: `posthog_personal_api_key`

Patterns not enabled by default remain configurable in push protection settings for GitHub Secret Protection and GitHub Advanced Security customers.

## Detectors added

Secret scanning now automatically detects these new secret types:

| Provider | Secret type | Partner | User | Push protection |
| --- | --- | --- | --- | --- |
| Cloudflare | `cloudflare_account_api_token` | ✓ | ✓ | ✓ (default) |
| Cloudflare | `cloudflare_global_user_api_key` | ✓ | ✓ | ✓ (default) |
| Cloudflare | `cloudflare_user_api_token` | ✓ | ✓ | ✓ (default) |

Notes:

- **Partner** secrets are automatically reported to the issuer when found in public repos via the secret scanning partnership program: https://docs.github.com/code-security/secret-scanning/secret-scanning-partnership-program/secret-scanning-partner-program
- **User** secrets generate secret scanning alerts when found in public or private repos.
- Background docs: https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning

## Secret metadata improvements

### Set validity on custom pattern alerts via API

Previously, validity was read-only in the API—you could dismiss/resolve, but not record whether a token was actually live or already rotated.

Now you can set the `validity` field on **custom pattern** secret scanning alerts via the existing PATCH endpoint:

```http
PATCH /repos/{owner}/{repo}/secret-scanning/alerts/{alert_number}
```

- Set to `active` (confirmed live secret)
- Set to `inactive` (confirmed not a threat)
- Clear override by sending `null`

The override is reflected immediately in:

- `GET` alert responses
- Alert detail UI (header and timeline entry)
- Webhook payloads
- Audit logs

Scope limitation:

- **Custom pattern alerts only**
- GitHub-supported patterns still use automatic validity via validators and can’t be overridden with this endpoint.

### Team and Topic filters for secret scanning campaigns

Secret scanning campaigns now support filtering by:

- **GitHub Team**
- **Topic**

These match filter options already available for code scanning campaigns and the standard secret scanning alert view.

### Provider field and filtering in the alerts API

If you’re building automation (routing/reporting by provider), alerts previously lacked provider filtering and didn’t return provider names.

Alert responses now include:

- `provider`: display name (e.g., "Stripe")
- `provider_slug`: machine-friendly identifier (e.g., "stripe") following the same slug convention as `secret_type`

All three list endpoints (`/repos`, `/orgs`, `/enterprises`) now accept:

- `providers`: include only alerts from these providers (comma-separated slugs)
- `exclude_providers`: exclude alerts from these providers

Constraints:

- `providers` and `exclude_providers` are mutually exclusive; using both returns **422**.
- The `provider` field is also included in webhook payloads.

## Scan history and delegation list improvements

### AI-detected secrets in scan history API

GitHub’s AI-powered generic secret detection runs backfill scans, but these scans were not previously visible in the scan history API.

Now the scan history API response includes:

- `generic_secrets_backfill_scans`

This follows the same structure as existing scan types such as `backfill_scans` and `pattern_update_scans`.

### Enterprise API: list secret scanning alert dismissal requests

Enterprise teams using delegated alert closures can now audit/report across the entire enterprise.

New endpoint:

```http
GET /enterprises/{enterprise}/dismissal-requests/secret-scanning
```

Features:

- Lists dismissal requests across all orgs in an enterprise
- Supports filters:
  - `organization_name`
  - `reviewer`
  - `requester`
  - `time_period`
  - `request_status`
- Pagination supported

Permissions:

- Requires **enterprise owner** or **enterprise security manager (ESM)** permissions

## Learn more

- Secret scanning docs: https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning
- Supported secrets list: https://docs.github.com/code-security/secret-scanning/introduction/supported-secret-scanning-patterns
- Community discussion: https://gh.io/community-secret-scanning


[Read the entire article](https://github.blog/changelog/2026-04-14-secret-scanning-pattern-updates-and-product-improvements)

