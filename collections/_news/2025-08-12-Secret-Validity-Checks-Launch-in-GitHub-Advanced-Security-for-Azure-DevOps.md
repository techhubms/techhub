---
layout: post
title: Secret Validity Checks Launch in GitHub Advanced Security for Azure DevOps
author: Michael Omokoh
canonical_url: https://devblogs.microsoft.com/devops/hunting-living-secrets-secret-validity-checks-arrive-in-github-advanced-security-for-azure-devops/
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/devops/feed/
date: 2025-08-12 16:08:45 +00:00
permalink: /azure/news/Secret-Validity-Checks-Launch-in-GitHub-Advanced-Security-for-Azure-DevOps
tags:
- Active Secrets
- Alert Prioritization
- Azure
- Azure & Cloud
- Azure DevOps
- Cloud Security
- Credential Management
- DevOps
- DevOps Security
- GitHub Advanced Security
- Microsoft Azure
- News
- Open Source
- Provider Patterns
- Remediation
- Secret Protection
- Secret Scanning
- Security
- Security Alerts
- Unknown Secrets
- YAML
section_names:
- azure
- devops
- security
---
Michael Omokoh introduces the secret validity checks feature in GitHub Advanced Security for Azure DevOps, showing how developers and security teams can now prioritize remediation of live secrets and stay ahead of credential exposures.<!--excerpt_end-->

# Secret Validity Checks Launch in GitHub Advanced Security for Azure DevOps

If you've ever felt overwhelmed by a flood of secret scanning alerts in your repositories, the new secret validity checks in GitHub Advanced Security for Azure DevOps are designed for you.

## What Are Secret Validity Checks?

With this enhancement, each secret scanning alert is now labeled with a high-signal field: either **Active** (the credential is still usable) or **Unknown** (validity could not be determined). This lets you separate alerts that represent a real, live risk from those that do not—cutting down on wasted effort and letting you focus on what matters most.

### How Does It Work?

- **Secret scanning** matches your code against known partner/provider patterns.
- For supported patterns, the platform automatically contacts the provider to check if the secret is still active.
- Alerts are labeled `Active` or `Unknown` depending on the result.
- After you remediate a secret, you can run on-demand verification to check that it's no longer usable.

For unsupported provider patterns, the status may stay 'Unknown'—that's normal. You can consult the [list of supported providers](https://learn.microsoft.com/azure/devops/repos/security/github-advanced-security-secret-scan-patterns?view=azure-devops#partner-provider-patterns) for more information.

## Typical Remediation Workflow

1. **Filter for Active secrets** to focus on the most urgent risks.
2. **Review alert details**, including the last time the secret was verified as active.
3. **Remediate**: rotate or revoke the secret, remove references in code, and follow recommended actions.
4. **Verify the secret** on demand to confirm it's been neutralized.
5. **Handle Unknown secrets** prudently, applying remediation if the blast radius or sensitivity is significant.
6. **Close resolved alerts** according to your team's policy.

![Validity checking list image](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2025/08/Validity-checking-list.png)

## Dealing With 'Unknown' Secrets

Treat 'Unknown' as potentially risky unless you have other mitigating information:

- Consider the blast radius (prod infrastructure vs. sandbox).
- Evaluate data sensitivity.
- Judge rotation cost—if it's low, rotate anyway.

If in doubt, act as if the secret is still active.

## FAQ Highlights

- **Does this feature revoke secrets automatically?** No. Validity checks only inform your prioritization—remediation actions remain manual (or automated by your workflows).
- **Will all secret types support validation?** Support will expand; check the [provider patterns list](https://learn.microsoft.com/azure/devops/repos/security/github-advanced-security-secret-scan-patterns?view=azure-devops#partner-provider-patterns) for updates.

## Getting Started

- Ensure that GitHub Advanced Security for Azure DevOps or the standalone Secret Protection experience is enabled for your repos.
- Make sure secret scanning is turned on—validity checks build on this foundation.
- No extra configuration is needed; validity checks start automatically for new, supported secrets.

## Reference Links

- [Explore secret scanning in greater depth](https://aka.ms/ghazdo-secret-validation)
- [Configure GitHub Advanced Security for Azure DevOps features](https://learn.microsoft.com/en-us/azure/devops/repos/security/configure-github-advanced-security-features?view=azure-devops&tabs=yaml)
- [Release notes for GitHub Advanced Security for Azure DevOps](https://learn.microsoft.com/azure/devops/release-notes/2025/sprint-260-update#github-advanced-security-for-azure-devops-1)

By integrating these new validity checks, teams can reduce time spent on false alarms and focus remediation where it matters—making for a stronger, more efficient security posture.

**Happy hunting.**

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/hunting-living-secrets-secret-validity-checks-arrive-in-github-advanced-security-for-azure-devops/)
