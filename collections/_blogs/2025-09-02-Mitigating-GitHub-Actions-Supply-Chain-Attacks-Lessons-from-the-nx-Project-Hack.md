---
external_url: https://jessehouwing.net/github-actions-learnings-from-the-recent-nx-hack/
title: 'Mitigating GitHub Actions Supply Chain Attacks: Lessons from the nx Project Hack'
author: Jesse Houwing
feed_name: Jesse Houwing's Blog
date: 2025-09-02 21:05:46 +00:00
tags:
- Advanced Security Code Scanning
- Branch Policies
- CI/CD
- Dependabot
- Environment Secrets
- GitHub
- GitHub Actions
- GitHub Token
- npm
- Permission Management
- Repository Secrets
- Secret Management
- StepSecurity
- Supply Chain Security
- Workflow Security
- YAML
section_names:
- devops
- security
---
Jesse Houwing examines the nx supply chain attack in detail, explaining how GitHub Actions misconfigurations led to leaked secrets and how to secure CI/CD workflows with actionable security best practices.<!--excerpt_end-->

# Mitigating GitHub Actions Supply Chain Attacks: Lessons from the nx Project Hack

**Author:** Jesse Houwing

## Introduction

A recent targeted supply chain attack involved the `nx` project, where a malicious npm package version exfiltrated GitHub tokens, crypto wallets, and other sensitive materials. The breach exploited misconfigured GitHub Actions workflows, specifically unsafe use of the `pull_request_target` trigger and inadequate secret management. This article explains the full attack chain, key vulnerabilities, and provides practical remediation strategies.

## Attack Chain Walkthrough

### 1. Workflow Misconfiguration

- The dangerous [`pull_request_target`](https://docs.github.com/en/actions/using-workflows/events-that-trigger-workflows#pull_request_target) trigger gave workflow runs access to sensitive secrets, such as the built-in GitHub Actions token.
- Arbitrary code execution was possible by abusing unfiltered PR titles/bodies, allowing attackers to inject shell commands into scripts run by the CI pipeline.

### 2. Token and Secret Exfiltration

- With code execution in the CI blockchain and permissive token scoping, attackers could access and leak:
  - GitHub repository tokens (enabling further automation and credential theft)
  - npm tokens and environment credentials
- Exploited workflow patterns included:
  - Writing PR content containing shell payloads
  - Overwriting workflows and triggering sensitive jobs
  - Exploiting repository-level secrets accessible to any workflow/job

### 3. Human and Process Gaps

- Workflows were possibly AI-generated and unreviewed for security flaws.
- No automated scanning was in place for dangerous patterns or policy violations.
- No restrictions on third-party action references and missing secret scoping.

### 4. Similar Supply Chain Threats

- The incident is analogous to recent attacks like [CVE-2025-30066](https://www.cve.news/cve-2025-30066/?ref=jessehouwing.net), showing a widespread pattern of malicious commits and secrets leakage through workflow vulnerabilities.

## Remediation Best Practices

1. **Set Default Permissions to Read-Only:**
   - Proactively reduce GitHub Actions default permissions at the organization and repository level.
2. **Scoping Secrets and Permissions:**
   - Use job-level and environment-level secrets over repository-level, limiting access as tightly as possible.
   - Pass secrets only to individual steps that require them, not to entire jobs or workflows.
3. **Review Workflow Triggers:**
   - Avoid using `pull_request_target` and restrict use of `workflow_dispatch`/`repository_dispatch` for jobs with sensitive access.
4. **Require PR Approval for Contributors:**
   - Enforce manual review before running workflows for external contributions.
5. **Pin All Actions to Commit SHAs:**
   - Prevent supply chain attacks by referencing third-party actions by full SHA, not just tags.
   - Use tools like `gh-pin-actions` or Dependabot automation.
6. **Enable Advanced Security:**
   - Turn on GitHub Advanced Security Code Scanning for workflows and enforce successful scans with branch protection rules.
7. **Enable Dependabot for Workflow Updates:**
   - Automatically keep actions up to date and audit for vulnerabilities.
8. **Adopt Third-Party Security Hardening (e.g., StepSecurity):**
   - Implement monitoring for anomalous behavior and outbound egress traffic.

## Practical Examples

### Limiting Permissions

```yaml
permissions: {}

jobs:
  build:
    permissions:
      contents: read
  deploy:
    permissions:
      contents: read
      packages: write
```

### Job-Scoped Secrets

```yaml
jobs:
  deploy:
    environment: production
    steps:
      - name: Publish
        env:
          NODE_AUTH_TOKEN: ${{ secrets.NPM_TOKEN }}
        run: |
          pnpm nx-release --local=false $VERSION $DRY_RUN
```

### Enforcing Approval

Turn on "Require approval for all external contributors" in repository settings to block automatic workflow runs from untrusted sources.

### Pinning Actions

```yaml
- uses: actions/checkout@08c6903cd8c0fde910a37f88322edcfb5dd907a8 # v5
```

## Monitoring and Hardening

- Use security scanning to detect risky workflow code (script injection, unsafe job triggers, unpinned actions).
- Consider tools like StepSecurity's Harden Runner for monitoring, anomaly detection, and egress control.

## Conclusion

GitHub Actions is a power feature but poses real security risks if not carefully managed. Incidents like the nx supply chain attack highlight the urgent need for strong DevOps and security practices across all CI/CD pipelines.

Follow the outlined strategies to limit your exposure, enforce defense-in-depth, and protect sensitive secrets from increasingly sophisticated supply chain threats.

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/github-actions-learnings-from-the-recent-nx-hack/)
