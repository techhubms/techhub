---
external_url: https://www.devclass.com/development/2025/12/17/github-to-charge-for-self-hosted-runners-from-march-2026/1734518
title: GitHub to Charge for Self-Hosted Runners from March 2026
author: DevClass.com
primary_section: devops
feed_name: DevClass
date: 2025-12-17 11:55:59 +00:00
tags:
- Actions Runner Controller
- Automation
- Blogs
- CI/CD
- Cloud Development
- Continuous Deployment
- Continuous Integration
- DevOps
- GitHub
- GitHub Actions
- Infrastructure
- Observability
- Open Source
- Pricing Model
- Self Hosted Runners
- Workflow Automation
section_names:
- devops
---
DevClass.com reports on GitHub’s plan to introduce per-minute pricing for self-hosted GitHub Actions runners in March 2026. The article, authored by Tim Anderson, reviews developer reactions, rationale behind the move, and new infrastructure features in response.<!--excerpt_end-->

# GitHub to Charge for Self-Hosted Runners from March 2026

GitHub has announced that starting March 1, 2026, it will charge {{CONTENT}}.002 per minute for the use of self-hosted runners with GitHub Actions—a major change, as these runners have previously been free. GitHub Actions is widely used for automating CI/CD workflows, and this move will affect many development teams running their own automation infrastructure.

## Key Points

- **Pricing Change:**
  - Self-hosted runner usage will be billed at {{CONTENT}}.002 per minute.
  - This only applies to hosted runners; GitHub-hosted runner prices will decrease by 20-39% starting January 1, 2026.
  - GitHub says 96% of customers will not see any billing change; public repositories remain unaffected.

- **Rationale:**
  - GitHub states that revenue from GitHub-hosted runners was subsidizing the free self-hosted runner offering.
  - Maintaining and evolving GitHub Actions infrastructure (now processing 71 million jobs daily) comes with costs.

- **Community Reaction:**
  - Users expressed frustration, particularly over paying for compute running on their own infrastructure.
  - Some cited more complex configuration, issues with the open source runner not accepting contributions, and problems like cancellation and reliability concerns.

- **Competitor Context:**
  - Competitors such as Depot offer optimized runners with per-second billing at lower prices, intensifying the debate.
  - Some users pointed out significant cost increases for high-usage workflows.

- **Performance and Popularity:**
  - Developers often use self-hosted runners for superior performance (builds can be over 10x faster than cloud runners).

- **Planned Improvements:**
  - GitHub promises new features inspired by feedback:
    - A new scale-set client for easier infrastructure scaling.
    - Multi-label support for Actions Runner Controller for more flexible runner management.
    - Introduction of Actions Data Stream, delivering real-time observability for GitHub workflows.

- **Exclusions:**
  - GitHub Enterprise Server (on-premises) users are not affected.

- **Industry Implications:**
  - This change prompts organizations to review their CI/CD infrastructure and potential costs.
  - Some projects, like the Zig programming language, have moved away from GitHub due to perceived neglect of Actions quality alongside these business shifts.

## References

- [Full announcement and pricing details](https://resources.github.com/actions/2026-pricing-changes-for-github-actions/)
- [Reddit developer discussion](https://old.reddit.com/r/programming/comments/1po996g/starting_march_1_2026_github_will_introduce_a_new/)
- [Actions Runner Controller roadmap](https://github.com/github/roadmap/issues/1195)
- [Depot competitor pricing](https://depot.dev/)

## Conclusion

GitHub’s new pricing model for self-hosted runners introduces both financial and technical considerations for DevOps teams. While the company is promising enhanced features and investment, many developers are watching to see if improvements sufficiently offset the changes and justify the ongoing cost of integrating self-hosted workflows.

This post appeared first on "DevClass". [Read the entire article here](https://www.devclass.com/development/2025/12/17/github-to-charge-for-self-hosted-runners-from-march-2026/1734518)
