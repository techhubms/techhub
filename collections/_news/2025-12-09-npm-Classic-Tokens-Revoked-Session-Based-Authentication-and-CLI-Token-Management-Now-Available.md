---
layout: post
title: 'npm Classic Tokens Revoked: Session-Based Authentication and CLI Token Management Now Available'
author: Allison
canonical_url: https://github.blog/changelog/2025-12-09-npm-classic-tokens-revoked-session-based-auth-and-cli-token-management-now-available
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-12-09 16:21:59 +00:00
permalink: /devops/news/npm-Classic-Tokens-Revoked-Session-Based-Authentication-and-CLI-Token-Management-Now-Available
tags:
- Authentication
- CI/CD
- CLI
- DevOps
- Granular Tokens
- Legacy API Endpoint
- News
- npm
- OIDC Trusted Publishing
- Retired
- Secure Publishing
- Security
- Security Hardening
- Session Tokens
- Supply Chain Security
- Token Management
- Yarn V1
- Yarn V2
section_names:
- devops
- security
---
Allison announces the permanent revocation of npm classic tokens, the shift to session-based authentication, and new CLI tools for granular token management, emphasizing heightened security in development and deployment workflows.<!--excerpt_end-->

# npm Classic Tokens Revoked: Session-Based Authentication and CLI Token Management Now Available

**Author:** Allison

As part of npm's ongoing security hardening initiative, several major changes to authentication and token management have now taken effect.

## What's Changed (December 9, 2025)

### Permanent Revocation of Classic Tokens

- All previously issued npm classic tokens have been revoked and can no longer be used, recreated, or recovered.

### Session-Based Authentication

- Running `npm login` now provides a temporary two-hour session token.
- Session tokens expire automatically after two hours, requiring users to reauthenticate.
- These tokens are not listed in the UI or CLI token lists but operate in the background to maintain authenticated sessions.
- Two-factor authentication (2FA) is now enforced for all publishing operations during these sessions.

### New CLI Tool for Granular Token Management

- npm introduces CLI commands to create, list, and revoke granular tokens directly from the terminal.
- Users no longer need to visit the npm website for token management.
- The CLI experience mirrors what was previously available for classic tokens.
- Documentation for these features is available in the [npm CLI documentation](https://docs.npmjs.com/cli/).

### Secure-by-Default 2FA for New Packages

- New package publications now have 2FA enabled by default, enhancing protection immediately upon creation.
- Existing packages retain their current 2FA configurations.

### Legacy API Endpoint Temporarily Restored

- Support for the legacy `/user/org.couchdb.user:` endpoint is temporarily reinstated to address compatibility issues, particularly for Yarn v1 and v2 users.
- This endpoint issues two-hour session tokens and will require reauthentication.
- Removal of this endpoint is imminent; users are urged to migrate to modern authentication methods.

## Guidance for Developers and CI/CD

**If you were using classic tokens:**

- Your tokens stopped working as of today. Restore access by:
  - Running `npm login` locally to obtain a session token.
  - For automation and CI/CD, create granular tokens using CLI (`npm token create`) or via [npmjs.com/settings/~/tokens](https://www.npmjs.com/settings/~/tokens).
  - For maximum security, consider OIDC trusted publishing, which removes the need for manual token management.
- Automated workflows may use the 'Bypass 2FA' setting for noninteractive usage, with limited write token expiration (up to 90 days).

## Next Steps & Further Information

- These enhancements safeguard both individual and organizational workflows across the JavaScript ecosystem.
- For more on npm's security roadmap, refer to the [community discussion](https://github.com/orgs/community/discussions/179562).

---
**References:**

- [npm security update: classic token creation disabled](https://github.blog/changelog/2025-11-05-npm-security-update-classic-token-creation-disabled-and-granular-token-changes/)
- [npm CLI documentation](https://docs.npmjs.com/cli/)
- [OIDC trusted publishing](https://docs.npmjs.com/trusted-publishers)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-09-npm-classic-tokens-revoked-session-based-auth-and-cli-token-management-now-available)
