---
external_url: https://github.blog/changelog/2025-11-04-github-copilot-policy-update-for-unconfigured-policies
title: GitHub Copilot Policy Update for Unconfigured Enterprise Policies
author: Allison
feed_name: The GitHub Blog
date: 2025-11-04 17:59:50 +00:00
tags:
- Access Control
- Admin Tools
- Configuration
- Copilot
- Developer Tools
- Enterprise Administration
- Enterprise Management Tools
- Enterprise Policy
- Feature Update
- Improvement
- Organization Policy
- Policy Management
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Allison explains recent GitHub Copilot policy changes that refine how unconfigured enterprise policies cascade to organizations, giving enterprise admins more explicit control.<!--excerpt_end-->

# GitHub Copilot Policy Update for Unconfigured Enterprise Policies

GitHub has addressed an issue affecting Copilot policy options for organizations when the corresponding enterprise policy was set to `Unconfigured`.

## What’s Changed

- When an enterprise-level Copilot policy is set to `Unconfigured`, the organizational policy now defaults to `Disabled`.
- Organizations can no longer enable Copilot for their users unless the enterprise admin enables or delegates the policy.
- To prevent disruption, previously enabled organization policies remain unchanged until manually set to `Disabled`, after which further changes cannot be made at the organization level unless the enterprise policy allows it.
- Future releases will automatically apply this corrected delegation behavior, ensuring organizational policies inherit the intended setting.

## Impact for Administrators

- **Admins now have more explicit control** over how Copilot is enabled or restricted across organizations.
- **No automatic disabling** for already enabled org policies, but changes are locked until the enterprise setting is updated.
- This update strengthens policy enforcement and streamlines policy management flow.

## Learn More

For more details, refer to [GitHub’s documentation on managing Copilot policies](https://docs.github.com/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-enterprise-policies).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-04-github-copilot-policy-update-for-unconfigured-policies)
