---
primary_section: devops
date: 2026-03-26 18:08:13 +00:00
external_url: https://github.blog/changelog/2026-03-26-credential-revocation-api-now-supports-github-oauth-and-github-app-credentials
title: Credential revocation API now supports GitHub OAuth and GitHub app credentials
tags:
- API Rate Limits
- Application Security
- Bulk Operations
- Credential Exposure
- Credential Revocation API
- DevOps
- Fine Grained Personal Access Tokens
- GitHub App
- GitHub REST API
- GitHub.com
- Improvement
- Incident Response
- News
- OAuth App Tokens
- Personal Access Tokens
- Refresh Tokens
- Security
- Security Log
- Token Revocation
- User To Server Tokens
section_names:
- devops
- security
feed_name: The GitHub Blog
author: Allison
---

Allison announces an expansion of GitHub’s Credential revocation API so teams can programmatically revoke more token types after credential exposure, including OAuth app tokens and GitHub App credentials, with revocations logged and owners notified.<!--excerpt_end-->

# Credential revocation API now supports GitHub OAuth and GitHub app credentials

GitHub has extended the [Credential revocation API](https://docs.github.com/rest/credentials/revoke?apiVersion=2022-11-28#revoke-a-list-of-credentials) to support additional token types. The goal is to help you quickly limit the impact of exposed credentials by revoking them programmatically, including tokens found in repositories that you don’t own.

## What’s new

You can now submit a bulk revocation request to revoke compromised or exposed tokens you found (even if they aren’t yours):

- [Revoke compromised or exposed tokens](https://docs.github.com/authentication/keeping-your-account-and-data-secure/token-expiration-and-revocation#token-revoked-by-a-third-party)

This unauthenticated API is available to all users on GitHub.com.

## Supported credential types

The API supports these credential/token types:

- Personal access tokens (classic)
- Fine-grained personal access tokens
- OAuth app tokens
- GitHub App user-to-server tokens
- GitHub App refresh tokens

## What happens when a token is submitted

When the API receives a valid token:

- The token is automatically revoked.
- The revocation is logged in the token owner’s security log.
- If the exposed token had access to a GitHub organization, that access is immediately removed.
- Revoked credentials cannot be reactivated by GitHub; the token owner must generate new credentials.
- The token owner is notified via email sent to the primary email address associated with the GitHub user account.

![GitHub System alert showing a token has been revoked, with the explanation field indicating disclosure through the credential revocation API](https://github.com/user-attachments/assets/6532434c-e3a5-4db7-8579-622feb58a9ea)

## Abuse prevention limits

To reduce abuse risk, the API has limits:

- 60 unauthenticated requests per hour
- Maximum of 1,000 tokens per API request

## Discussion

Join the discussion on GitHub Community: GitHub Community announcements


[Read the entire article](https://github.blog/changelog/2026-03-26-credential-revocation-api-now-supports-github-oauth-and-github-app-credentials)

