---
layout: "post"
title: "Copilot Code Review: Now Available in Xcode and Enterprise Admin Controls"
description: "This news update covers the general availability of Copilot code review in Xcode, along with enhanced enterprise and organization settings for managing Copilot code review. The post explains how developers can use Copilot code review in Xcode and details new admin controls that help enterprises and organizations manage Copilot code review access across platforms such as GitHub.com and Visual Studio Code."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-27-copilot-code-review-generally-available-in-xcode-and-new-admin-control"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-27 18:03:54 +00:00
permalink: "/2025-08-27-Copilot-Code-Review-Now-Available-in-Xcode-and-Enterprise-Admin-Controls.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Admin Control", "AI", "CCR", "Code Review Automation", "Copilot Code Review", "Developer Tools", "Eclipse", "Enterprise Administration", "Enterprise Policy", "GitHub Copilot", "JetBrains", "News", "Organization Settings", "Pull Requests", "Security Review", "VS Code", "Xcode"]
tags_normalized: ["admin control", "ai", "ccr", "code review automation", "copilot code review", "developer tools", "eclipse", "enterprise administration", "enterprise policy", "github copilot", "jetbrains", "news", "organization settings", "pull requests", "security review", "vs code", "xcode"]
---

Allison reports on the general release of Copilot code review in Xcode and the introduction of new enterprise and organization-level controls, giving admins more direct management of the Copilot review experience.<!--excerpt_end-->

# Copilot Code Review: General Availability in Xcode and Enterprise Controls

GitHub has announced that Copilot code review (CCR) is now generally available in Xcode, giving macOS and iOS developers access to AI-powered code review directly within the Xcode editor. Developers can use Copilot code review in Xcode to quickly self-review open files, specific selections, or staged changes before opening a pull request. This helps identify logic, security, performance, or testing issues earlier in the development cycle, without leaving the coding environment.

To request a code review in Xcode, you can use the Copilot chat panel and receive AI-driven feedback on selected code.

## New Enterprise and Organization-Level Controls

A new Copilot code review setting provides enterprises and organizations with granular control over the CCR feature. Key capabilities include:

- **Independent Feature Toggle:** Admins can enable or disable Copilot code review separately from other Copilot features, such as Copilot chat.
- **Seamless Transition:** By default, existing Copilot policies continue to apply, ensuring no disruption as organizations adopt the new feature.
- **Public Preview Participation:** Organizations can opt-in to try new CCR features before general release.
- **Multi-Platform Management:** The new controls apply to Xcode, Visual Studio Code, and soon-to-be-supported editors like Visual Studio, JetBrains IDEs, and Eclipse.
- **Enterprise-Wide Block Policy:** Admins can enforce a policy to disable Copilot code review across all enterprise repositories, overriding individual user licenses if needed.

If your enterprise uses the `Copilot on GitHub.com` policy or preview sub-policies, CCR remains enabled by default but now is managed through its own setting.

For further discussion or questions about these changes and rollout plans, developers and admins are encouraged to visit the [GitHub Community discussion](https://github.com/orgs/community/discussions/141896?sort=new).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-27-copilot-code-review-generally-available-in-xcode-and-new-admin-control)
