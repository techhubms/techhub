---
external_url: https://github.blog/changelog/2025-09-10-copilot-code-review-independent-repository-rule-for-automatic-reviews
title: 'Automatic Copilot Code Review: Standalone Repository Rule Now Available'
author: Allison
feed_name: The GitHub Blog
date: 2025-09-10 20:08:07 +00:00
tags:
- Automatic Code Review
- Automation
- Code Quality
- Continuous Review
- Copilot
- Developer Tools
- GitHub
- Improvement
- Pull Requests
- Repository Rules
- Rule Configuration
section_names:
- ai
- devops
- github-copilot
---
Allison reports on GitHub Copilot's new standalone repository rule for automatic code reviews, providing teams with granular control over review triggers and workflow integration.<!--excerpt_end-->

# Automatic Copilot Code Review: Standalone Repository Rule Now Available

GitHub Copilot users can now enable automatic code review with a dedicated repository rule, expanding the flexibility and control available to development teams. Previously, automatic Copilot code reviews could only be triggered as part of the **Require a pull request before merging** policy. This update decouples Copilot reviews from merge gating, allowing automatic reviews to be applied more broadly and granularly.

## What’s New

- **Standalone Copilot Review Rule:** Admins can add or edit rulesets to include Copilot code review as an independent automation.
- **Subsettings for Fine-Tuning:**
  - **Run on each push:** Automatically reruns reviews when new commits are pushed.
  - **Run on drafts:** Enables Copilot to review draft pull requests, allowing authors to iterate before a human review is requested.
- **Flexible Integration:** Teams can start with code reviews on creation only and extend to on-push or on-draft scenarios for added coverage and signal control.

For example, teams working with critical branches can enforce tighter review loops with on-push reviews. Alternatively, enabling on-draft reviews helps iterate code quality before pulling in additional reviewers.

### How to Enable

1. Open your repository’s **Settings** and navigate to **Rules** → **Rulesets**.
2. Add or modify a ruleset to include the new automatic Copilot code review option.
3. Choose from the available subsettings (on-push, on-draft) to tailor the review flow to your team's needs.
4. Save the ruleset and test by opening a new pull request.

For more details and technical instructions, refer to the [GitHub documentation](https://docs.github.com/copilot/how-tos/use-copilot-agents/request-a-code-review/configure-automatic-review).

Join the community conversation in [GitHub Community: Copilot Conversations](https://github.com/orgs/community/discussions/categories/copilot-conversations).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-10-copilot-code-review-independent-repository-rule-for-automatic-reviews)
