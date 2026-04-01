---
author: Kedasha Kerr
section_names:
- ai
- devops
- github-copilot
- security
date: 2026-03-30 16:00:00 +00:00
tags:
- AI
- Code Scanning
- CodeQL
- Copilot Autofix
- Dependabot
- Dependabot Alerts
- Dependabot Security Updates
- Developer Skills
- DevOps
- GHAS
- GitHub
- GitHub Advanced Security
- GitHub Advisory Database
- GitHub Copilot
- News
- Pull Requests
- Repository Settings
- Secret Scanning
- Security
- Security Alerts
- Supply Chain Security
- Vulnerability Management
primary_section: github-copilot
feed_name: The GitHub Blog
external_url: https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-security/
title: 'GitHub for Beginners: Getting started with GitHub security'
---

Kedasha Kerr walks through enabling and using GitHub Advanced Security features—secret scanning, Dependabot, CodeQL code scanning, and Copilot Autofix—to help you detect and fix common vulnerabilities in a GitHub repository.<!--excerpt_end-->

# GitHub for Beginners: Getting started with GitHub security

Welcome back to GitHub for Beginners, season three! So far this year, we’ve covered [GitHub Issues and Projects](https://youtu.be/c67GaAkf1BE?si=gQm38vCKp0S8y66C), as well as [GitHub Actions](https://www.youtube.com/watch?v=BQrohJ3PT7I). This time around, we’re going to be talking a little bit about security, and what tools GitHub provides to help you keep your code secure.

By the end of this post, you’ll understand how to fix vulnerabilities in your repository using built-in tools like secret scanning, Dependabot, code scanning, and Copilot Autofix.

## Why security matters

Vulnerabilities are weaknesses in your code or the libraries you use that attackers can exploit.

A key point: you inherit risk from a library the moment you import it into your project, even though you didn’t write the vulnerable code yourself. This is why even small or brand-new projects can have vulnerabilities—almost all software relies on third-party packages.

GitHub provides [GitHub Advanced Security (GHAS)](https://github.com/security/advanced-security?utm_source=blog-episode-3&utm_medium=blog&utm_campaign=gfb-s3-2026), a suite of products that helps you improve and maintain the quality of your code.

- On public repositories, you have access to:
  - Dependabot
  - Code scanning
  - Secret scanning
  - Copilot Autofix
- For private repositories, you’ll need a GHAS license.

If you want a broad overview, see the docs: [About GitHub Advanced Security](https://docs.github.com/get-started/learning-about-github/about-github-advanced-security?utm_source=blog-episode-3&utm_medium=blog&utm_campaign=gfb-s3-2026).

## Enabling security features

First, ensure GHAS is turned on for your repository:

1. Navigate to your repository.
2. Click the **Settings** tab at the top of the page.
3. In the left-hand bar, under the “Security” section, select **Advanced Security**.
4. Under “Dependabot,” enable:
   - “Dependabot alerts”
   - “Dependabot security updates”
5. Scroll down to the “Code scanning” section.
6. For “CodeQL analysis,” select **Set up** and then select **Default** from the context menu.
7. In the new window, select **Enable CodeQL** without changing any settings.
8. Scroll down to “Secret Protection” and enable it.

After enabling, select the **Security** tab at the top of the repository UI to open the security home page. This is where you’ll see alerts for:

- Exposed secrets
- Vulnerable dependencies
- Risky code paths

## Using secret scanning

Secret scanning helps protect sensitive information you accidentally commit (for example, API keys or tokens).

- When a secret is detected, you’ll see an alert under **Secret scanning** in the repository’s **Security** area.
- Open an alert to see what was detected and where it was found.

A common response is to **revoke** the secret (disable the old key so it can’t be used anymore). You typically do this at the source platform that issued the credential (for example, Azure or Stripe).

GitHub can’t revoke the secret automatically, but secret scanning provides early warning so a leaked secret doesn’t become an exploited secret.

To close a secret scanning alert after revoking:

1. Select **Close as** in the top-right of the window.
2. Select **Revoked** from the context menu.
3. Click **Close alert**.

## What is Dependabot?

Dependabot helps keep dependencies up to date and alerts you when libraries your project depends on have known vulnerabilities.

How to use Dependabot alerts:

- Go to the repository **Security** tab and open a Dependabot alert.
- The alert takes you to a pull request that updates the dependency.
- In that PR, you can view the advisory that triggered the alert via **See advisory in GitHub Advisory Database**.

When reviewing the update:

1. Select **Review security update**.
2. Review the proposed version bump.
3. If it looks good, merge the pull request.

Dependabot automates turning GitHub security advisories into pull requests so you don’t have to manually track CVEs.

## Responding to CodeQL alerts

CodeQL is the engine that scans your code and produces code scanning alerts.

- CodeQL is not a linter.
- It can analyze data flow (where input starts and where it ends up), which enables a broader set of security findings.

When you open a code scanning alert, GitHub can provide:

- An explanation of the issue
- Recommendations for fixes
- Examples showing the problem and possible solution

### Using Copilot Autofix for CodeQL alerts

You can use Copilot Autofix to generate a patch for certain code scanning alerts:

1. Select **Generate fix** at the top of the alert.
2. Review the suggested patch and verify it addresses your needs.
3. Click **Commit to new branch**.
4. In the pop-up, select **Open a pull request**, then click **Commit change**.
5. Review and merge the generated pull request like any other change.

Even when Copilot accelerates security fixes, you stay in control: you still need to review and decide what gets merged.

## What’s next?

Public repositories have access to these GHAS tools for free, so you can start using them early. You can practice using GitHub Skills or the `vulnerable-node` repository.

Further reading:

- [About secret scanning](https://docs.github.com/code-security/concepts/secret-security/about-secret-scanning?utm_source=blog-episode-3&utm_medium=blog&utm_campaign=gfb-s3-2026)
- [About Dependabot alerts](https://docs.github.com/code-security/concepts/supply-chain-security/about-dependabot-alerts?utm_source=blog-episode-3&utm_medium=blog&utm_campaign=gfb-s3-2026)
- [About code scanning alerts](https://docs.github.com/code-security/concepts/code-scanning/about-code-scanning-alerts?utm_source=blog-episode-3&utm_medium=blog&utm_campaign=gfb-s3-2026)


[Read the entire article](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-security/)

