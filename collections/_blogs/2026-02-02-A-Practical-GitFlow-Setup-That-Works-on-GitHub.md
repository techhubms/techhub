---
title: A Practical GitFlow Setup That Works on GitHub
section_names:
- ai
- devops
- github-copilot
- security
author: Emanuele Bartolesi
feed_name: Emanuele Bartolesi's Blog
primary_section: github-copilot
tags:
- AI
- Blogs
- Branch Protection Rules
- Changelog Generation
- CI/CD
- Code Scanning
- CODEOWNERS
- Dependabot
- DevOps
- DevSecOps
- Environment Protection
- Environments
- Git
- Gitflow
- GitHub
- GitHub Actions
- GitHub Copilot
- GitHub Pull Requests
- Hotfix Workflow
- Least Privilege
- OIDC
- Production Deployments
- Protected Branches
- Release Management
- Reusable Workflows
- Security
- Semantic Versioning
- Staging Environment
- VS Code Extension
- VS Code Marketplace
date: 2026-02-02 12:06:43 +00:00
external_url: https://dev.to/playfulprogramming/a-practical-gitflow-setup-that-works-on-github-46lb
---

Emanuele Bartolesi shares the GitFlow setup he actually enforces on GitHub, including strict branch protection, PR habits, release/tag rules, and how he wires it to GitHub Actions, environments, and basic security checks so the workflow holds up under real release and hotfix pressure.<!--excerpt_end-->

# A Practical GitFlow Setup That Works on GitHub

I use GitFlow even for my side projects.

Not because it is elegant. Not because it is modern. I use it because it forces me to behave like the project matters, even when nobody else is watching.

When you work alone, shortcuts are tempting. You push directly to `main`. You skip reviews. You promise yourself you will clean it up later. Later never comes. GitFlow, with the right rules, removes that option. It makes discipline the default instead of a personal choice.

GitFlow is not light. It adds structure, friction, and process. If you deploy to production ten times per day, GitFlow will slow you down. In that case, trunk-based development with feature flags is usually the better choice.

But when you ship versioned releases, when you have a real staging phase, or when you want a clean separation between “work in progress” and “ready to release”, GitFlow still works well. Even for a solo developer.

Everything in this article is what I actually run on GitHub today.

If you are going to use GitFlow, use it properly. Otherwise, do not use it at all.

[![GitFlow Init diagram](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fy2mqvl0cfa7ax722i3g2.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fy2mqvl0cfa7ax722i3g2.png)

## When GitFlow makes sense (and when it does not)

GitFlow is a release model, not a productivity hack.

### When GitFlow makes sense

GitFlow works well when:

- You ship **versioned releases** (you care about `v1.2.3` meaning something).
- You have a **real staging phase** (release candidates can be rejected).
- You need **parallel workstreams** (features on `develop` while stabilizing a release or preparing a hotfix).
- You want a **hard separation** between work in progress and releasable code:
  - `main` is always deployable
  - `develop` can move faster and break occasionally

### When GitFlow is the wrong choice

Avoid GitFlow when:

- You deploy continuously multiple times per day with no staging or release windows.
- Your team cannot keep `develop` usable as an integration gate.
- You do not have environments to support the model (no staging / no clear separation).

### The most common mistake

Teams adopt GitFlow “by the book” but keep old habits:

- Direct pushes still sneak in.
- Hotfixes do not get merged back.
- Releases happen straight from `develop` because it is “almost ready”.

[![GitFlow workflow diagram](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Ffbj36d6v03epl9kxkdca.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Ffbj36d6v03epl9kxkdca.png)

## The GitFlow branch model (the “boring and strict” version)

These are the only branches used.

### `main`

- Always releasable.
- No direct pushes (not even for typos).
- If `main` is broken, the model failed earlier.

### `develop`

- Integration branch: features meet here.
- Protected even when working solo (no direct pushes; PRs only).
- Temporary instability is allowed, but not for long.

### `feature/*`

- All work starts here.
- Designed to be short-lived (aim: 3–5 days max; split bigger work).
- Deleted after merge; never reused.

Examples:

- `feature/auth-token-refresh`
- `feature/api-rate-limits`
- `feature/ui-settings-page`

### `release/*`

- Created only when you actually need stabilization.
- If there is no staging phase, skip release branches.
- No new features; only fixes, hardening, documentation, versioning.
- When ready: merge into `main`, tag, then merge back into `develop` (both directions).

Typical names:

- `release/1.4.0`
- `release/2026.02`

### `hotfix/*`

- Rare by design.
- Branch from `main` (never from `develop`).
- Merge into `main` and immediately back into `develop`.

Examples:

- `hotfix/login-nullref`
- `hotfix/payment-timeout`

## Rules that make GitFlow work (enforced, not optional)

### Protected branches are non-negotiable

Protect `main` and `develop`:

- No direct pushes
- No force pushes
- No branch deletion
- Pull requests only

### Pull requests are mandatory checkpoints

Before merging, answer:

- Why does this change exist?
- What exactly changed?
- How did I verify it works?
- What is the risk?

[![GitHub branch protection settings screenshot](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fe575r89phk5ubzjjiso0.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fe575r89phk5ubzjjiso0.png)

### Approvals still matter, even solo

- Require at least one approval.
- Even self-approving is useful as a deliberate pause (read the diff in GitHub).

### Conversation resolution

- Require all conversations to be resolved before merging.
- Every comment should result in a decision.

### Linear history vs merge commits

Pick and enforce one approach.

Author’s preference:

- Squash merge `feature/*` into `develop`
- Merge commits for `release/*` and `hotfix/*` into `main`

## Pull requests as the core unit of work

### A simple PR template

- **Why**: problem/reason
- **What**: high-level change
- **How tested**: tests/manual checks/environments
- **Risk**: what could break
- **Rollback**: how to undo

### Small PRs beat clever branching

- If a reviewer needs more than 30–45 minutes, the PR is too big.
- Slice work into vertical, mergeable steps.

### `CODEOWNERS` even for tiny repos

- Makes review routing explicit.
- GitHub treats CODEOWNERS reviews differently, adding another enforcement layer.

## Naming, traceability, and “future me”

### Branch names should explain intent

Prefer explicit names:

- Bad: `feature/login`
- Better: `feature/oauth-token-refresh`

If using an issue tracker, include the ID:

- `feature/ABC-123-oauth-token-refresh`

### PR titles are part of the changelog

Use titles that can ship as release notes:

- “Fix token refresh race condition”
- “Add rate limiting to public API”

### Link everything

- Always link PRs to issues (even if the issue only exists to track the PR).
- Example: `Closes #123`

### Traceability must be enforced

Enforce via:

- Branch naming patterns
- PR templates
- Required checks

[![GitFlow in GitKraken screenshot](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fwinolm9tuo3v1y0etxon.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fwinolm9tuoo3v1y0etxon.png)

## Releases, tags, and changelogs you can trust

### Tags exist only on `main`

- Tag releases only on `main`.
- Tagging on `develop` or `release/*` breaks the “what is actually running?” guarantee.

### Release branches are not releases

A release exists only when:

1. The release branch is merged into `main`
2. A tag is created on `main`

[![Semantic versioning diagram](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fx1qafc798dvm0gk17w5y.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fx1qafc798dvm0gk17w5y.png)

### Versioning decisions are made early

- Semantic versioning when public contracts matter
- Date-based versioning for internal tools or fast-moving products

### Changelogs are generated

Generated from:

- PR titles
- Labels
- Commit history on `main`

Note: the author mentions GitHub Copilot helps with PR titles and commit messages.

## Environments (or GitFlow is just ritual)

GitFlow assumes environments.

### Minimum environment setup

- **Dev / Integration**: auto-deploy from `develop`
- **Staging**: deploy from `release/*`
- **Production**: deploy from `main`

### Why this matters

- `develop` needs a shared place where integration issues show up.
- `release/*` needs a stable validation environment.
- `main` must map one-to-one to production.

## Handling hotfixes without breaking the model

- Hotfixes always start from `main`.
- Scope must stay minimal (no refactoring, no cleanup).
- CI and reviews still apply.
- Always back-merge into `develop`.

[![Merge conflicts illustration](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Ftf0yl4wzym0ihhfv04vl.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Ftf0yl4wzym0ihhfv04vl.png)

## Merge conflicts: keep them boring

- Painful conflicts usually come from long-lived branches.
- Keep feature branches short-lived.
- Split anything that can’t merge in ~3–5 days.

## GitHub Actions patterns reused everywhere

### Required checks are gates

- Protected branches have required checks.
- No override; fix flaky/slow checks instead of removing requirements.

### Reusable workflows

- Avoid copy/paste.
- Use reusable workflows so rules are consistent across repos.

### Environment protection

For production deployments, use GitHub environments with protection:

- Required reviewers
- Scoped secrets
- Explicit approval step

### Secrets and permissions

- Workflows get only the permissions they need.
- Avoid wildcard tokens.
- Avoid long-lived secrets when OIDC is available.

## Security and governance enabled by default

### Dependabot

- Enable Dependabot alerts and PRs.
- Dependency updates go through PR rules and checks like everything else.

### Code scanning

Run code scanning at minimum on:

- `develop`
- `main`

Goal: signal, not perfect scores.

## In a nutshell (copy this)

### Branches

- `main`: always releasable; production only
- `develop`: integration branch; must stay usable
- `feature/*`: short-lived; merged into `develop`
- `release/*`: only when a real stabilization phase exists
- `hotfix/*`: urgent fixes from `main`; always merged back

### Protection rules

Protected:

- `main`
- `develop`
- `release/*`
- `hotfix/*`

Rules:

- PRs required
- At least one approval
- Required status checks
- Conversation resolution required
- No direct pushes
- No force pushes
- No branch deletion

### Pull request rules

- PR template: Why / What / How tested / Risk / Rollback
- Small PRs by design
- `CODEOWNERS` enabled
- One merge strategy enforced

### Merge strategy

- Squash merge for `feature/*` into `develop`
- Merge commits for `release/*` and `hotfix/*`
- No rebasing of shared branches

### CI/CD

- `feature/*`: build, unit tests, lint
- `develop`: full test suite, security checks
- `release/*`: full suite, packaging, deploy to staging
- `main`: production deploy, tag, changelog

### Releases

- Tags on `main` only
- Semantic or date-based versioning decided early
- Changelogs generated from PR metadata

### Environments

- `develop` → dev/integration
- `release/*` → staging
- `main` → production

### Hotfix discipline

- Branch from `main`
- Minimal scope
- Merge to `main`
- Merge back to `develop`
- No skipped steps

This policy is intentionally boring. Boring scales.

## Appendix: GitHub Copilot quota visibility in VS Code

The author also built a VS Code extension called **Copilot Insights** for GitHub Copilot users to see plan and quota status in VS Code.

- Marketplace link: https://marketplace.visualstudio.com/items?itemName=emanuelebartolesi.vscode-copilot-insights

[![Copilot Insights extension screenshot](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fsqzk5hamyymcmuh515a4.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fsqzk5hamyymcmuh515a4.png)

![Additional extension image from the post](https://media2.dev.to/dynamic/image/width=256,height=,fit=scale-down,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F8j7kvp660rqzt99zui8e.png)


[Read the entire article](https://dev.to/playfulprogramming/a-practical-gitflow-setup-that-works-on-github-46lb)

