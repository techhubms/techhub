---
external_url: https://devopsjournal.io/blog/2025/03/16/Really-keepingyour-GitHub-Actions-usage-secure
title: Really Keeping Your GitHub Actions Usage Secure
author: Rob Bos
feed_name: Rob Bos' Blog
date: 2025-03-16 00:00:00 +00:00
tags:
- Action Backups
- CVE 30066
- Dependency Monitoring
- GitHub Actions
- GitHub Advanced Security
- Internal Marketplace
- Security Alerts
- Slsa.dev
- StepSecurity
- Supply Chain Security
- Workflow Automation
section_names:
- devops
- security
---
In this post, Rob Bos details a recent security incident involving a compromised GitHub Action and offers guidance on securing your CI/CD pipelines with robust processes and tooling.<!--excerpt_end-->

## Really Keeping Your GitHub Actions Usage Secure

**By Rob Bos**

### Incident Recap

A widely-used GitHub Action—utilized in over 23,000 public repositories—was recently compromised. Malicious code was injected to steal repository secrets, and version tags were updated to propagate the change. Many organizations could have had sensitive data exposed, especially those not following security best practices. The incident is catalogued as CVE-2025-30066.

This breach occurred just before the weekend, underlining the importance of monitoring workflow executions and promptly checking logs following security advisories.

### Remediation Measures

StepSecurity identified the compromise and quickly published a secure version of the affected Action at [step-security/changed-files](https://github.com/step-security/changed-files). StepSecurity provides point-in-time backups for key actions, updating only after approval and validation, thereby catching malicious modifications early. Such processes provide greater assurance and minimize the risk of unnoticed data leakage.

The original compromised repository has now been purged of malicious code and restored. However, some users may not even realize sensitive information could have leaked.

### Awareness and Communications

Many affected workflows encountered errors when the original compromised Action was removed. While GitHub Advisories Database published the security alert, only repositories with Dependency Security Alerts enabled received notifications. For robust protection, activating Dependency Security Alerts across all repositories is recommended.

### Preventive Strategies

The best protection against such attacks involves:

- **Using a service** like StepSecurity to validate, secure, and back up your Actions, ensuring workflow continuity if an Action is taken offline.
- **Maintaining your own backups** by forking Actions into an organization you control. This lets you dictate when updates occur and facilitates extra validations and security reviews before deploying updates to your workflows.
- **Supply chain awareness:** Enable GitHub’s Dependency Graph and Security Alerts on every repository (public or private). Advanced features for organizational views are available on paid plans.
- **Periodic inventory:** Use tools like [devops-actions/load-used-actions](https://github.com/devops-actions/load-used-actions) to audit dependencies and track Actions usage.

### Implementing Backup Processes

Rob Bos highlights the need to proactively manage Actions reliance:

- **Fork** required Actions into a dedicated organization for clarity and logistical separation.
- **Update management:** Use tools like GitHub Fork Updater to control and review all incoming updates, building an approval and security evaluation step into your release process.
- **Internal marketplace:** Create an inventory for tracking Action usage and facilitate requests for new Actions. This streamlines security checks and supports organizational compliance.
- **Secure dependencies:** Take responsibility not just for the primary Action, but also for all dependencies—binaries, shell scripts, Docker images—that are fetched at runtime.

These recommendations align closely with supply chain security frameworks such as [slsa.dev](https://slsa.dev). Ensuring complete visibility and control is essential, as your obligations extend to all components and artifacts accessed during CI/CD execution.

### Further Resources and Learning

- **Presentations:** Watch "Using GitHub Actions with Security in Mind" from GitHub Universe 2021 ([YouTube link](https://www.youtube.com/watch?v=Ers-LcA7Nmc)).
- **Books:** Refer to "GitHub Actions in Action" co-authored by Rob Bos, available at [Manning.com](https://www.manning.com/books/github-actions-in-action).
- **Courses:** Explore "GitHub Advanced Security" on LinkedIn Learning for an in-depth look at available security tools.

By following these strategies, organizations can significantly enhance their resilience against supply chain attacks in their CI/CD pipelines.

This post appeared first on "Rob Bos' Blog". [Read the entire article here](https://devopsjournal.io/blog/2025/03/16/Really-keepingyour-GitHub-Actions-usage-secure)
