---
primary_section: devops
section_names:
- devops
- security
author: Allison
title: Secret scanning improvements to alert APIs, webhooks, and delegated workflows
external_url: https://github.blog/changelog/2026-04-08-secret-scanning-improvements-to-alert-apis-webhooks-and-delegated-workflows
feed_name: The GitHub Blog
tags:
- Alert Locations
- Alert Payloads
- API Filters
- Application Security
- Auditing
- Automation
- Bug Fix
- Delegated Closures
- Delegated Workflows
- Developer Experience
- DevOps
- Email Notifications
- Exclude Secret Types
- GitHub REST API
- GitHub Secret Scanning
- HTML Url
- Improvement
- News
- Push Protection Bypass
- Query Parameters
- Resolution Comment
- Secret Scanning Alerts API
- Security
- Ticketing Systems Integration
- Webhooks
date: 2026-04-08 19:56:31 +00:00
---

Allison summarizes GitHub Secret Scanning updates that make security automation easier: a new REST API exclusion filter, richer webhook payloads (including user-facing URLs), clearer delegated-bypass emails, additional closure-request fields, and a fix for a delegated-closure resolution comment bug.<!--excerpt_end-->

# Secret scanning improvements to alert APIs, webhooks, and delegated workflows

This update rolls out several improvements across GitHub Secret Scanning APIs, webhooks, and delegated alert workflows, aimed at making security automation and reporting simpler.

## What’s changing

Secret scanning now includes:

- **New `exclude_secret_types` filter** in the REST API (previously only available in the GitHub UI via `-secret-type:`)
- **Richer webhook payloads** and REST API data for alert locations via a new `html_url`
- **Delegated bypass email improvements** (expiry deadlines visible to reviewers; confirmation emails for developers)
- **New closure request comment fields** surfaced in API/webhook payloads
- **Bug fix** so `resolution_comment` is no longer `null` for delegated-closure approvals

## New `exclude_secret_types` API filter

New secret types are added regularly, which can make inclusion-based filtering brittle for reporting/remediation scripts (you have to keep updating your allowed list). With exclusion filtering, you can filter out the few secret types you *don’t* care about and automatically include newly-added types.

The secret scanning alerts API now supports an `exclude_secret_types` query parameter on all three list endpoints:

- `GET /repos/{owner}/{repo}/secret-scanning/alerts`
- `GET /orgs/{org}/secret-scanning/alerts`
- `GET /enterprises/{enterprise}/secret-scanning/alerts`

Behavior notes:

- Pass a **comma-separated list** of secret type slugs to exclude.
- Using `secret_type` and `exclude_secret_types` together in the same request returns a **422** error.

## Alert location `html_url` field

Secret scanning alert locations in both the REST API and webhook payloads now include an `html_url` field under the `details` object.

Previously, location details returned only API URLs (for example: `/repos/{owner}/{repo}/issues/comments/{id}`), which forced extra API calls to obtain a user-facing link for notifications or dashboards. Now the clickable URL is included directly.

### Covered location types

The `html_url` field covers commit, issue, pull request, and review locations:

| Location type | Example `html_url` |
| --- | --- |
| `commit` | `/{owner}/{repo}/blob/{sha}/{path}#L5-L5` |
| `issue_title` / `issue_body` | `/{owner}/{repo}/issues/{number}` |
| `issue_comment` | `/{owner}/{repo}/issues/{number}#issuecomment-{id}` |
| `pull_request_title` / `pull_request_body` | `/{owner}/{repo}/pull/{number}` |
| `pull_request_comment` | `/{owner}/{repo}/pull/{number}#issuecomment-{id}` |
| `pull_request_review` | `/{owner}/{repo}/pull/{number}#pullrequestreview-{id}` |
| `pull_request_review_comment` | `/{owner}/{repo}/pull/{number}#discussion_r{id}` |

Notes:

- Existing URL fields are unchanged; **`html_url` is additive**.
- Available in:
  - REST API: `GET /repos/{owner}/{repo}/secret-scanning/alerts/{number}/locations`
  - Webhook payloads

## Email notifications for delegated bypass requests

Two updates were made to emails sent during delegated bypass workflows:

- **Expiry notice**: Bypass and dismissal request emails now include the request’s expiration period:
  - 7 days, 30 days, or 1 year depending on request type
  - Example: push protection bypasses expire in 7 days; code scanning dismissals last a year
- **Developer confirmation email**: Developers now receive an email when they submit an alert dismissal request (so they know it’s pending review). Previously only reviewers were notified.

## Closure request comments in alert API and webhooks

For secret scanning alerts that have a delegated closure (dismissal) request, three new fields are now surfaced on alert payloads:

- `closure_request_comment`: requester’s comment explaining why they want to close the alert
- `closure_request_reviewer_comment`: reviewer’s response comment
- `closure_request_reviewer`: reviewer user object

These follow the same model as existing push protection bypass fields (for example, `push_protection_bypass_request_comment` and `push_protection_bypass_request_reviewer_comment`).

This is especially useful if you automate delegated alert closures, such as:

- Tracking why alerts were dismissed
- Auditing reviewer decisions
- Syncing alerts and decisions to external ticketing systems

## Bug fix: `resolution_comment` for delegated closures

A bug was fixed where `resolution_comment` was `null` on alerts closed through the delegated alert closure approval flow.

- The requester’s comment was stored on the exemption request but wasn’t forwarded to the alert when approved.
- The comment is now correctly passed through to the alert’s `resolution_comment`, matching the behavior of direct dismissals via the REST API.

## Learn more

- Secret scanning docs: https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning
- Secret scanning REST API docs: https://docs.github.com/rest/secret-scanning
- Community discussion: https://gh.io/community-secret-scanning


[Read the entire article](https://github.blog/changelog/2026-04-08-secret-scanning-improvements-to-alert-apis-webhooks-and-delegated-workflows)

