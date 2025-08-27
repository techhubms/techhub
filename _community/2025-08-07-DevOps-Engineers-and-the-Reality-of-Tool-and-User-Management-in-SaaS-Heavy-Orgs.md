---
layout: "post"
title: "DevOps Engineers and the Reality of Tool and User Management in SaaS-Heavy Orgs"
description: "This community discussion, led by Coffeebrain695, examines the blurred boundaries between DevOps, office IT, and user administration in organizations rapidly adopting multiple SaaS platforms. Contributors reflect on how tasks like SSO setup, Google Workspace group management, and user provisioning creep into DevOps roles, propose strategies for better boundaries, and share tips for automation to refocus on core platform engineering."
author: "Coffeebrain695"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/devops/comments/1mk0s1l/how_much_of_your_job_involves_administering_tools/"
viewing_mode: "external"
feed_name: "Reddit DevOps"
feed_url: "https://www.reddit.com/r/devops/.rss"
date: 2025-08-07 13:56:26 +00:00
permalink: "/2025-08-07-DevOps-Engineers-and-the-Reality-of-Tool-and-User-Management-in-SaaS-Heavy-Orgs.html"
categories: ["DevOps"]
tags: ["Automation", "CI/CD", "Community", "DevOps", "Google Workspace", "IAM", "IT Operations", "Platform Engineering", "RBAC", "Role Management", "SaaS Administration", "Scope Creep", "SSO", "Terraform", "User Provisioning", "Workload Management"]
tags_normalized: ["automation", "cislashcd", "community", "devops", "google workspace", "iam", "it operations", "platform engineering", "rbac", "role management", "saas administration", "scope creep", "sso", "terraform", "user provisioning", "workload management"]
---

Coffeebrain695 and other community members share real-world experiences of DevOps engineers handling SaaS user management and tool administration due to shifting team roles.<!--excerpt_end-->

# DevOps Engineers and SaaS User Administration – Where's the Line?

**Posted by Coffeebrain695**

> _"My company has really thrown the kitchen sink at SaaS products. Every week a new one seems to be coming up and I'm struggling to keep track of it. We have SSO enabled for most, but there are exceptions and user/group changes in Google Workspace still need manual work. It often feels like I'm doing Office IT rather than DevOps. Our security/IT guy had to step back, so these duties got dumped on us. Is managing tools and users just part of the platform/DevOps engineer job?"_

### Community Insights

- **Common Occurrence:** Many confirm that in organizations with fewer dedicated IT resources, platform, DevOps, or infrastructure engineers often inherit user admin and SaaS management tasks—especially after layoffs or role reductions.
- **Scope Creep:** Several members note this admin work isn't truly DevOps, but often gets 'lumped in' when others depart: _"User admin and SaaS wrangling isn't DevOps—it's what orgs shove into DevOps when they fire the person who was actually doing it."_
- **Defining Boundaries:** Advice is given to candidly track and report time spent on these activities to management, demonstrating impact on proper platform work:
  - Log time spent on non-core DevOps tasks
  - Show how it competes with platform engineering
  - Recommend splitting or delegating IT admin back to dedicated Ops/Sec roles
- **Automation Suggestions:** Some contributors describe success in reducing manual toil:
  - Automate user and SaaS provisioning (e.g., Google Workspace admin with Terraform)
  - Implement self-service or CI-led account changes, minimizing hands-on involvement
- **Mental Health and Professional Focus:** Reassurance that setting boundaries isn't being "precious"—it's key to protecting engineering focus and avoiding long-term burnout
- **Resources:** [NoFluffWisdom Newsletter](https://NoFluffWisdom.com/Subscribe) is suggested for deeper takes on scope management

### Takeaways for DevOps Teams

- User & SaaS admin work is a recurring 'side effect' when organizations cut IT staff
- Automation (Terraform, CI/CD) can reduce some of these burdens
- Clear communication and time tracking help make the case for restoring IT/ops support
- Not every task that involves infrastructure or user access should default to DevOps—it's healthy to draw lines and push for sustainable workflows

### Key Technical Points

- SSO, RBAC, user provisioning intersect with DevOps, but the routine admin should ideally be automated or delegated
- Tools mentioned: **Google Workspace**, **Terraform**, CI/CD pipelines for user management

This post appeared first on "Reddit DevOps". [Read the entire article here](https://www.reddit.com/r/devops/comments/1mk0s1l/how_much_of_your_job_involves_administering_tools/)
