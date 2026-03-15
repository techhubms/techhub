---
external_url: https://github.blog/changelog/2026-02-10-track-additional-dependabot-configuration-changes-in-audit-logs
title: Track Additional Dependabot Configuration Changes in Audit Logs
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-10 14:13:08 +00:00
tags:
- Audit Logs
- Compliance
- Configuration Management
- Dependabot
- DevOps
- Enterprise Security
- Forensic Investigation
- GitHub
- Improvement
- News
- Organization Security
- Security
- Security Monitoring
- Self Hosted Runners
- Supply Chain Security
- Vulnerability Updates
section_names:
- devops
- security
---
Allison outlines new GitHub audit log events that enhance visibility into Dependabot configuration changes, boosting compliance and supply chain security for organizations.<!--excerpt_end-->

# Track Additional Dependabot Configuration Changes in Audit Logs

GitHub has introduced two new types of events in organization and enterprise audit logs to help teams track changes to Dependabot configurations:

- **Dependabot vulnerability updates toggle**: Logs when users enable or disable Dependabot vulnerability updates on a repository. For details, see the [`dependabot_security_updates` documentation](https://docs.github.com/organizations/keeping-your-organization-secure/managing-security-settings-for-your-organization/audit-log-events-for-your-organization#dependabot_security_updates).
- **Self-hosted runner configuration**: Logs any enabling or disabling of Dependabot on self-hosted runners. See the [`repository_dependency_updates_self_hosted` documentation](https://docs.github.com/organizations/keeping-your-organization-secure/managing-security-settings-for-your-organization/audit-log-events-for-your-organization#repository_dependency_updates_self_hosted).

These audit events capture details such as the user who made the change and the exact timestamp. You can review them within your [organization audit log](https://docs.github.com/organizations/keeping-your-organization-secure/managing-security-settings-for-your-organization/reviewing-the-audit-log-for-your-organization) or [enterprise audit log](https://docs.github.com/enterprise-cloud@latest/admin/monitoring-activity-in-your-enterprise/reviewing-audit-logs-for-your-enterprise/about-the-audit-log-for-your-enterprise).

## Security and Compliance Benefits

- **Track configuration changes:** Maintain oversight for security and compliance by monitoring who alters Dependabot settings.
- **Detect unauthorized modifications:** Quickly identify and address tampering with security-related configurations.
- **Enable forensic investigations:** Leverage detailed logs when investigating possible security incidents.

For further information or to join the conversation, visit the [GitHub Community discussions](https://github.com/orgs/community/discussions/categories/announcements).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-10-track-additional-dependabot-configuration-changes-in-audit-logs)
