---
external_url: https://www.reddit.com/r/devops/comments/1mhho6n/best_practices_for_migrating_manually_created/
title: Best Practices for Migrating Manually Created Monitors to Terraform
author: JayDee2306
feed_name: Reddit DevOps
date: 2025-08-04 16:09:23 +00:00
tags:
- Automation
- Best Practices
- Configuration Management
- Datadog
- IaC
- Migration
- Monitoring
- Resource Drift
- Terraform
- Version Control
section_names:
- devops
---
Authored by JayDee2306, this article explores community-driven strategies for migrating Datadog monitors into Terraform, highlighting best practices and lessons learned.<!--excerpt_end-->

## Migrating Manually Created Monitors to Terraform: Community Best Practices

**Author: JayDee2306**

Transitioning from manually managed Datadog monitors to Terraform-based configuration is a common DevOps challenge. Doing so improves consistency, version control, and automation. This article compiles key community considerations and best practices for migrating these resources to Infrastructure as Code.

### 1. Exporting Existing Monitors

- **Tools and Scripts:**
  - Community and open-source tools can query the Datadog API and generate Terraform (HCL) code for existing monitors. Examples include `terraforming-datadog` or custom scripts utilizing Datadog’s API to output HCL files.
  - Review each tool for accuracy and active maintenance; limited or outdated tooling may require manual intervention for complex monitor logic.

### 2. Manual Migration Steps

- **Audit Existing Monitors:**
  - List all monitors and confirm which are in active use to avoid migrating obsolete configurations.
- **Map Configurations:**
  - Compare monitor settings in Datadog with corresponding Terraform resources to identify available features and potential gaps.
- **Import vs. Recreate:**
  - Where possible, use `terraform import` to link existing resources to Terraform state and then codify them in HCL. For more complex cases, manual re-creation may be necessary.
- **Review API Limits:**
  - Bulk importing or updating can hit API rate limits; stagger migrations if needed.

### 3. Common Migration Pitfalls

- **Duplication:**
  - If incorrectly imported, you may end up with duplicate monitors (both a manual and a Terraform-managed version). Audit Datadog before and after import.
- **Resource Drift:**
  - Ensure Terraform state accurately reflects the current Datadog monitor state to avoid unexpected replacements or deletions.
- **Downtime:**
  - If monitors are mistakenly deleted or disabled during migration, critical alerting gaps could occur. Plan for a phased cutover with tests.
- **Loss of History:**
  - Some monitor metadata (e.g., alert history) may not be migrated.

### 4. Enforcing Terraform Management

- **Change Management:**
  - Restrict permissions in Datadog to prevent users from editing monitors outside Terraform.
  - Implement peer-review processes for HCL changes (e.g., via GitHub Pull Requests).
- **Monitoring Drift:**
  - Use drift detection tools to identify changes made outside Terraform.
- **Documentation:**
  - Document the “Terraform only” policy and procedures for all team members.

### 5. Lessons Learned and Community Advice

- **Pilot First:**
  - Start with a subset of monitors to validate your export and import process before bulk migration.
- **Backups:**
  - Export current settings (even as JSON) to provide a rollback plan.
- **Communication:**
  - Communicate the migration plan to stakeholders to coordinate around any potential alerting disruptions.

### 6. Further Reading / Examples

- Explore examples and scripts on Datadog and Terraform community GitHub repositories.
- Review related experiences on forums such as Reddit’s /r/devops.

---

Migrating to Terraform enables repeatable, source-controlled monitor definitions while minimizing human error and configuration drift. Community experience confirms success depends on careful planning, stakeholder communication, and incremental adoption.

This post appeared first on "Reddit DevOps". [Read the entire article here](https://www.reddit.com/r/devops/comments/1mhho6n/best_practices_for_migrating_manually_created/)
