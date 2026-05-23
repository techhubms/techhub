-- Section-specific roundup content: devops
-- Generated: 2026-05-19 11:07
-- Heading aliases matched: DevOps
-- Updates backfilled duplicates so each roundup only contains devops content.
-- Usage: psql -U techhub -d techhub -f section-devops.sql

-- SKIP 2025-07-07 : no 'DevOps' block found in source (weekly-ai-roundup-2025-07-07)

-- weekly-devops-roundup-2025-07-14  (2025-07-14)
UPDATE content_items
SET  content      = 'Workflow improvements center on Azure DevOps documentation, CI/CD reliability, and onboarding standardization—targeting daily developer efficiency.

<!--excerpt_end-->

## DevOps

### Markdown-Enabled Work Item Documentation and Editor Preferences

Azure DevOps introduces Markdown support for work item fields, finally offering real-time preview, sticky editor preferences, and REST API compatibility. The change brings modern, portable docs to dev teams, and the opt-in transition is carefully staged to preserve stability.

- [Markdown Support Now Available for Azure DevOps Work Items](https://devblogs.microsoft.com/devops/markdown-support-arrives-for-work-items/)

### Security Patch, Installation Standardization, and Maintenance Reliability

Critical July patches for Azure DevOps Server resolve multi-repo trigger bugs in YAML pipelines; updates ship as standalone executables with clear validation paths for legacy environments. To ease onboarding, the community proposes a universal MCP install link for VS Code and docs—promising one-click, platform-agnostic setup.

Momentum on extensibility and easier onboarding—highlighted last week—remains, with CLI and MCP tools driving productivity and security.

- [July Patches for Azure DevOps Server Now Available](https://devblogs.microsoft.com/devops/july-patches-for-azure-devops-server-2/)
- [VS Code Live - Let it Cook: Building a Universal MCP Install Link](/devops/videos/vs-code-live-let-it-cook-building-a-universal-mcp-install-link)',
     excerpt      = 'Workflow improvements center on Azure DevOps documentation, CI/CD reliability, and onboarding standardization—targeting daily developer efficiency.',
     content_hash = md5('Workflow improvements center on Azure DevOps documentation, CI/CD reliability, and onboarding standardization—targeting daily developer efficiency.

<!--excerpt_end-->

## DevOps

### Markdown-Enabled Work Item Documentation and Editor Preferences

Azure DevOps introduces Markdown support for work item fields, finally offering real-time preview, sticky editor preferences, and REST API compatibility. The change brings modern, portable docs to dev teams, and the opt-in transition is carefully staged to preserve stability.

- [Markdown Support Now Available for Azure DevOps Work Items](https://devblogs.microsoft.com/devops/markdown-support-arrives-for-work-items/)

### Security Patch, Installation Standardization, and Maintenance Reliability

Critical July patches for Azure DevOps Server resolve multi-repo trigger bugs in YAML pipelines; updates ship as standalone executables with clear validation paths for legacy environments. To ease onboarding, the community proposes a universal MCP install link for VS Code and docs—promising one-click, platform-agnostic setup.

Momentum on extensibility and easier onboarding—highlighted last week—remains, with CLI and MCP tools driving productivity and security.

- [July Patches for Azure DevOps Server Now Available](https://devblogs.microsoft.com/devops/july-patches-for-azure-devops-server-2/)
- [VS Code Live - Let it Cook: Building a Universal MCP Install Link](/devops/videos/vs-code-live-let-it-cook-building-a-universal-mcp-install-link)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-07-14';

-- weekly-devops-roundup-2025-07-21  (2025-07-21)
UPDATE content_items
SET  content      = 'Community discussions this week emphasized workflow automation, flexible pipeline design, and toolchain modernization to meet the needs of evolving cloud and enterprise environments.

<!--excerpt_end-->

## DevOps

### Automating and Streamlining Merge Conflict Resolution

Developers automate large-scale merge conflict resolution in Visual Studio using git commands or scripts to accept incoming changes, calling for IDE extensions and improved tooling as projects grow.

- [How to Auto-Resolve 100+ Merge Conflicts by Accepting Incoming Version for All Files?](https://www.reddit.com/r/azuredevops/comments/1m1xrde/how_to_autoresolve_100_merge_conflicts_by/)

### Designing Flexible Deployment Pipelines for Complex Environments

Developers architect cleaner Azure DevOps pipelines for multi-environment, role-based deployments, favoring dynamic inventories, centralized configs, and orchestration tools over hardcoded logic to better manage complexity and troubleshooting.

- [Seeking Advice on Deployment Pipeline Design for Multi-Environment, Role-Based Server Setups](https://www.reddit.com/r/azuredevops/comments/1m2vtfz/looking_for_advice_on_architecting_a_deployment/)

### Transitioning Outlook Add-ins for Modern DevOps Integration

As Outlook add-in models modernize, integrating DevOps/TFS extensions for compatibility and productivity remains a niche need, requiring adaptive marketing and ongoing innovation to serve enterprise users in the cloud era.

- [Should I Build for Azure DevOps? Exploring Market Potential for Outlook Add-ins](https://www.reddit.com/r/azuredevops/comments/1m4jkch/should_i_build_for_ado/)',
     excerpt      = 'Community discussions this week emphasized workflow automation, flexible pipeline design, and toolchain modernization to meet the needs of evolving cloud and enterprise environments.',
     content_hash = md5('Community discussions this week emphasized workflow automation, flexible pipeline design, and toolchain modernization to meet the needs of evolving cloud and enterprise environments.

<!--excerpt_end-->

## DevOps

### Automating and Streamlining Merge Conflict Resolution

Developers automate large-scale merge conflict resolution in Visual Studio using git commands or scripts to accept incoming changes, calling for IDE extensions and improved tooling as projects grow.

- [How to Auto-Resolve 100+ Merge Conflicts by Accepting Incoming Version for All Files?](https://www.reddit.com/r/azuredevops/comments/1m1xrde/how_to_autoresolve_100_merge_conflicts_by/)

### Designing Flexible Deployment Pipelines for Complex Environments

Developers architect cleaner Azure DevOps pipelines for multi-environment, role-based deployments, favoring dynamic inventories, centralized configs, and orchestration tools over hardcoded logic to better manage complexity and troubleshooting.

- [Seeking Advice on Deployment Pipeline Design for Multi-Environment, Role-Based Server Setups](https://www.reddit.com/r/azuredevops/comments/1m2vtfz/looking_for_advice_on_architecting_a_deployment/)

### Transitioning Outlook Add-ins for Modern DevOps Integration

As Outlook add-in models modernize, integrating DevOps/TFS extensions for compatibility and productivity remains a niche need, requiring adaptive marketing and ongoing innovation to serve enterprise users in the cloud era.

- [Should I Build for Azure DevOps? Exploring Market Potential for Outlook Add-ins](https://www.reddit.com/r/azuredevops/comments/1m4jkch/should_i_build_for_ado/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-07-21';

-- weekly-devops-roundup-2025-07-28  (2025-07-28)
UPDATE content_items
SET  content      = 'DevOps advancements focused on automation, secure workflows, and scale-friendly best practices, establishing a foundation for robust, reproducible deployments and open source platform sustainability.

<!--excerpt_end-->

## DevOps

### Infrastructure as Code and Fabric Automation

The Terraform Provider for Microsoft Fabric, with Fabric CLI and MCP server integration, now enables end-to-end declarative Fabric environment automation—minimizing manual errors and aligning with IaC standards. Hands-on guides walk through these automations, extending last week’s momentum on MCP-driven deployment.

- [Terraform Provider for Microsoft Fabric: Using MCP Servers and Fabric CLI](https://blog.fabric.microsoft.com/en-US/blog/terraform-provider-for-microsoft-fabric-2-using-the-terraform-mcp-server-and-fabric-cli-to-help-define-your-fabric-resources/)

### Secure Automation, API Integration, and DevOps Productivity

A GitHub App + JWT auth extension for Azure Pipelines centralizes API access, eliminates manual secrets, and raises security for status, checks, and releases. MCP server improvements enable secure remote deployment and OAuth, unlocking multi-tenant and project-specific automation while continuing last week’s automation theme.

- [How to Streamline GitHub API Calls in Azure Pipelines Using a Custom DevOps Extension](https://github.blog/enterprise-software/ci-cd/how-to-streamline-github-api-calls-in-azure-pipelines/)
- [Upgrading to GitHub''s Remote MCP Server: From Docker Setup to OAuth Simplicity](/devops/videos/upgrading-to-githubs-remote-mcp-server-from-docker-setup-to-oauth-simplicity)

### Advanced Context Switching, Multi-Org Workflows, and Pipeline Modernization

Context switching between Azure DevOps orgs is now instant with dynamic MCP servers, saving consultants significant time and error risk. Community guidance on YAML migration aids multi-repo and multi-env pipelines, regression test coordination, and modern, scalable CI/CD.

- [Dynamic Azure DevOps MCP Server for Seamless Context Switching in Claude Code](https://www.reddit.com/r/azuredevops/comments/1m91urt/i_built_a_dynamic_azure_devops_mcp_server_for/)
- [Migrating Classic Azure DevOps Pipelines to YAML for Multi-Repo Apps](https://www.reddit.com/r/azuredevops/comments/1malfn3/best_practice_for_yaml_pipelines/)

### Practical Configuration, Troubleshooting, and Secret Management

Operational tips covered managing Key Vault URIs and secret rotatability between GitHub and Azure DevOps, as well as real-world fixes for deployment blockers like Node.js version mismatches in Azure containers.

- [Managing Key Vault URIs Across Environments in GitHub Actions and Azure DevOps Pipelines](https://www.reddit.com/r/azuredevops/comments/1m886qz/pipeline_parameters/)
- [Fixing Node.js Version Mismatch in Azure Web App Service Deployment](https://www.reddit.com/r/azuredevops/comments/1m6btbu/web_app_service_wrong_version/)

### Environments, Deployments, and Open Source Community

A practical guide to GitHub Environments clarifies deployment strategies and secret management. Community sessions, like Open Source Friday Brasil, continue to lower contribution barriers—carrying on last week’s collaborative, open DevOps thread.

- [Understanding GitHub Environments and Deployments: A Practical Overview](https://www.reddit.com/r/github/comments/1madm8p/i_finally_understand_what_are_github_environments/)
- [Open Source Friday Brasil with Ana Carolyne from Codaqui](/devops/videos/open-source-friday-brasil-with-ana-carolyne-from-codaqui)

### Sustaining Open Source and European Tech Funding

Calls for a European Sovereign Tech Fund push for government-supported, secure, and sustainable open source infrastructure—responding to high-impact security incidents and advocating stable OSS funding models.

- [We Need a European Sovereign Tech Fund to Sustain Open Source Software](https://github.blog/open-source/maintainers/we-need-a-european-sovereign-tech-fund/)',
     excerpt      = 'DevOps advancements focused on automation, secure workflows, and scale-friendly best practices, establishing a foundation for robust, reproducible deployments and open source platform sustainability.',
     content_hash = md5('DevOps advancements focused on automation, secure workflows, and scale-friendly best practices, establishing a foundation for robust, reproducible deployments and open source platform sustainability.

<!--excerpt_end-->

## DevOps

### Infrastructure as Code and Fabric Automation

The Terraform Provider for Microsoft Fabric, with Fabric CLI and MCP server integration, now enables end-to-end declarative Fabric environment automation—minimizing manual errors and aligning with IaC standards. Hands-on guides walk through these automations, extending last week’s momentum on MCP-driven deployment.

- [Terraform Provider for Microsoft Fabric: Using MCP Servers and Fabric CLI](https://blog.fabric.microsoft.com/en-US/blog/terraform-provider-for-microsoft-fabric-2-using-the-terraform-mcp-server-and-fabric-cli-to-help-define-your-fabric-resources/)

### Secure Automation, API Integration, and DevOps Productivity

A GitHub App + JWT auth extension for Azure Pipelines centralizes API access, eliminates manual secrets, and raises security for status, checks, and releases. MCP server improvements enable secure remote deployment and OAuth, unlocking multi-tenant and project-specific automation while continuing last week’s automation theme.

- [How to Streamline GitHub API Calls in Azure Pipelines Using a Custom DevOps Extension](https://github.blog/enterprise-software/ci-cd/how-to-streamline-github-api-calls-in-azure-pipelines/)
- [Upgrading to GitHub''s Remote MCP Server: From Docker Setup to OAuth Simplicity](/devops/videos/upgrading-to-githubs-remote-mcp-server-from-docker-setup-to-oauth-simplicity)

### Advanced Context Switching, Multi-Org Workflows, and Pipeline Modernization

Context switching between Azure DevOps orgs is now instant with dynamic MCP servers, saving consultants significant time and error risk. Community guidance on YAML migration aids multi-repo and multi-env pipelines, regression test coordination, and modern, scalable CI/CD.

- [Dynamic Azure DevOps MCP Server for Seamless Context Switching in Claude Code](https://www.reddit.com/r/azuredevops/comments/1m91urt/i_built_a_dynamic_azure_devops_mcp_server_for/)
- [Migrating Classic Azure DevOps Pipelines to YAML for Multi-Repo Apps](https://www.reddit.com/r/azuredevops/comments/1malfn3/best_practice_for_yaml_pipelines/)

### Practical Configuration, Troubleshooting, and Secret Management

Operational tips covered managing Key Vault URIs and secret rotatability between GitHub and Azure DevOps, as well as real-world fixes for deployment blockers like Node.js version mismatches in Azure containers.

- [Managing Key Vault URIs Across Environments in GitHub Actions and Azure DevOps Pipelines](https://www.reddit.com/r/azuredevops/comments/1m886qz/pipeline_parameters/)
- [Fixing Node.js Version Mismatch in Azure Web App Service Deployment](https://www.reddit.com/r/azuredevops/comments/1m6btbu/web_app_service_wrong_version/)

### Environments, Deployments, and Open Source Community

A practical guide to GitHub Environments clarifies deployment strategies and secret management. Community sessions, like Open Source Friday Brasil, continue to lower contribution barriers—carrying on last week’s collaborative, open DevOps thread.

- [Understanding GitHub Environments and Deployments: A Practical Overview](https://www.reddit.com/r/github/comments/1madm8p/i_finally_understand_what_are_github_environments/)
- [Open Source Friday Brasil with Ana Carolyne from Codaqui](/devops/videos/open-source-friday-brasil-with-ana-carolyne-from-codaqui)

### Sustaining Open Source and European Tech Funding

Calls for a European Sovereign Tech Fund push for government-supported, secure, and sustainable open source infrastructure—responding to high-impact security incidents and advocating stable OSS funding models.

- [We Need a European Sovereign Tech Fund to Sustain Open Source Software](https://github.blog/open-source/maintainers/we-need-a-european-sovereign-tech-fund/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-07-28';

-- weekly-devops-roundup-2025-08-04  (2025-08-04)
UPDATE content_items
SET  content      = 'DevOps saw deepening observability, AI automation, secure config management, maturing toolchains, and best-practices for policy, IaC, and real-world deployments—reflecting a domain balancing technical change and organizational growth.

<!--excerpt_end-->

## DevOps

### Observability Matures in Hybrid Environments

Organizations are moving beyond firewall-centric monitoring to full-stack observability—combining Internet Performance Monitoring, Real User Monitoring, and Synthetic Monitoring for comprehensive insight and rapid, DevSecOps-aligned incident response.

- [Beyond the Firewall - Achieving True Observability in Hybrid Infrastructure](https://devops.com/beyond-the-firewall-achieving-true-observability-in-the-era-of-hybrid-infrastructure/?utm_source=rss&utm_medium=rss&utm_campaign=beyond-the-firewall-achieving-true-observability-in-the-era-of-hybrid-infrastructure)
- [Observability in Retail: How to Monitor and Manage Interactive Kiosk Fleets](https://devops.com/observability-in-retail-how-to-monitor-and-manage-interactive-kiosk-fleets/?utm_source=rss&utm_medium=rss&utm_campaign=observability-in-retail-how-to-monitor-and-manage-interactive-kiosk-fleets)
- [Why Observability Isn’t Just for SREs (and How Devs Can Get Started)](https://www.reddit.com/r/devops/comments/1mfsvq8/why_observability_isnt_just_for_sres_and_how_devs/)

### AI and Automation Expand Productivity

AI extensions for Azure DevOps automate pull request reviews, cut review time, and surface security issues, while BMC brings AI-driven insight to mainframe DevOps. The trend is toward freeing humans for higher-value work, with strong privacy and data control options.

- [Building an AI Extension to Enhance Azure DevOps Pull Request Reviews](https://www.reddit.com/r/azuredevops/comments/1mdoa94/built_an_ai_extension_that_actually_makes_azure/)
- [BMC Extends Scope and Reach of DevOps Mainframe Workflows](https://devops.com/bmc-extends-scope-and-reach-of-devops-mainframe-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=bmc-extends-scope-and-reach-of-devops-mainframe-workflows)
- [Redefining Engineering Excellence: Amplifying Impact with Product Skills in the AI Era](https://devops.com/redefining-engineering-excellence-how-product-skills-amplify-your-impact-in-the-era-of-ai/?utm_source=rss&utm_medium=rss&utm_campaign=redefining-engineering-excellence-how-product-skills-amplify-your-impact-in-the-era-of-ai)

### Managing Secrets and Config at Scale

Centralizing secrets via AWS Parameter Store and automating Kubernetes sealed-secrets are now best practice for scaling microservices and delivery pipelines securely. Teams are reminded to avoid storing sensitive data in public source and lean on runtime secret injection.

- [How we solved environment variable chaos for 40+ microservices on ECS/Lambda/Batch with AWS Parameter Store](https://www.reddit.com/r/devops/comments/1mgl9tl/how_we_solved_environment_variable_chaos_for_40/)
- [[kubeseal] Built a small tool to make bitnami''s sealed-secrets less painful in GitOps](https://www.reddit.com/r/devops/comments/1mfshis/kubeseal_built_a_small_tool_to_make_bitnamis/)
- [How to Keep key.properties Private in a Public GitHub Repository](https://www.reddit.com/r/github/comments/1mgrtc3/private_file_in_github_repo/)

### Modern Toolchains and Deployment Orchestration

Microsoft Aspire positions itself as a multi-language DevOps “IDE” for managing distributed deployments, joined by SchemaNest for schema management and actionable guidance on CI/CD pipeline structuring and service connection automation.

- [Aspire: A Modern DevOps Toolchain](https://medium.com/@davidfowl/aspire-a-modern-devops-toolchain-fa5aac019d64)
- [SchemaNest: A Fast, Team-Friendly CI/CD-Ready JSON Schema Registry](https://www.reddit.com/r/devops/comments/1mg1fl8/schemanest_where_schemas_grow_thrive_and_scale/)
- [Structuring CI/CD Pipelines Across Code and Helm Chart Repositories in Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1mfmw05/how_to_structure_cicd_pipelines_across_two_repos/)
- [Automating Azure DevOps Service Connection Creation via Release Pipelines](https://www.reddit.com/r/azuredevops/comments/1mgg9wy/release_pipeline_for_creating_serviceconnections/)

### IaC and Compliance Best Practices

Terraform provider guides for Microsoft Fabric and Terraform Associate exam tips reflect ongoing organizational focus on codifying and securing infrastructure, with real-world experience emphasizing compliance and practical deployment.

- [Terraform Provider for Microsoft Fabric: #3 Creating a Workload Identity with Fabric Permissions](https://blog.fabric.microsoft.com/en-US/blog/terraform-provider-for-microsoft-fabric-3-creating-a-workload-identity-with-fabric-permissions/)
- [Terraform Associate (003) Exam – Sharing Study Resources That Helped Me Pass](https://www.reddit.com/r/devops/comments/1mgm77r/terraform_associate_003_exam_sharing_study/)

### Release, Handoff, and Deployment Versioning

Agency teams are tackling versioning and client hand-off with checklists and dedicated tool discussions; practical pain points often center on mapping independently versioned components for diverse customers.

- [Order of Operations for Web Agency: Building, Deploying, and Transferring Client Websites](https://www.reddit.com/r/github/comments/1mgba7n/need_help_web_ai_agency/)
- [Deployment versioning challenges across customers and components](https://www.reddit.com/r/devops/comments/1mfy3o0/deployment_versioning_problems/)

### Workflow, Shift Left, and DevOps Careers

The “shift left” vs. “shove left” distinction is emphasized—empowering devs with tools/process is key, not just dumping more work. Step-by-step roadmaps help backend engineers transition to fully skilled DevOps practitioners.

- [“Shove Left” – Dumping Downstream Tasks Onto Developers – A Recipe for Failure](https://devops.com/shove-left-dumping-downstream-tasks-onto-developers-a-recipe-for-failure/?utm_source=rss&utm_medium=rss&utm_campaign=shove-left-dumping-downstream-tasks-onto-developers-a-recipe-for-failure)
- [Transitioning from Backend Developer to DevOps](https://www.reddit.com/r/devops/comments/1mgx0a4/transitioning_from_backend_developer_to_devops/)

### Azure DevOps Workflows and Policy

Teams reviewed backlog and PR merging policies, service connection scripting, and repo-split CI/CD pipeline management, as workflow reliability and productivity remain major themes.

- [Questions About Azure DevOps Backlogs: Closed Work Items & Iteration Filtering](https://www.reddit.com/r/azuredevops/comments/1me1ts0/devops_backlog_questions/)
- [Enforcing PR Branch Policies with Multiple Required Pipelines in Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1mbguxq/how_to_only_allow_prs_if_pipelines_x_y_both_run/)
- [Automating Azure DevOps Service Connection Creation via Release Pipelines](https://www.reddit.com/r/azuredevops/comments/1mgg9wy/release_pipeline_for_creating_serviceconnections/)
- [Structuring CI/CD Pipelines Across Code and Helm Chart Repositories in Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1mfmw05/how_to_structure_cicd_pipelines_across_two_repos/)

### Blazor, Web Delivery, and Code Coverage

Blazor’s streamlined .NET delivery is gaining traction, while teams address CI coverage limitations with creative open-source and Makefile/CMake practices.

- [DevOps Meets Blazor in 2025 - Streamlining .NET Web App Delivery for the Future](https://devops.com/devops-meets-blazor-in-2025-streamlining-net-web-app-delivery-for-the-future/?utm_source=rss&utm_medium=rss&utm_campaign=devops-meets-blazor-in-2025-streamlining-net-web-app-delivery-for-the-future)
- [Unit Test Code Coverage Options in VS 2022 Pro for C Projects](https://www.reddit.com/r/VisualStudio/comments/1md4xq8/how_to_get_unit_test_code_coverage_using_vs_2022/)

### Emerging Trends: DevSecOps and Sustainability

Trends point to embedded security, scalability, and environmentally conscious DevOps as critical next frontiers.

- [Emerging DevOps Trends: Security, Scalability and Sustainability](https://devops.com/emerging-devops-trends-security-scalability-and-sustainability/?utm_source=rss&utm_medium=rss&utm_campaign=emerging-devops-trends-security-scalability-and-sustainability)

### Community and Ecosystem Updates

Ecosystem chatter addressed GitHub UI bugs, access friction, static hosting tradeoffs, and new podcasts, highlighting ongoing community adaptation and platform evolution.

- [[Bug] “Commit changes” button remains active during GitHub file upload — causes incomplete commits](https://www.reddit.com/r/github/comments/1mblpfk/bug_commit_changes_button_remains_active_during/)
- [Login Prompts and Access Restrictions on GitHub: Privacy Concerns From a User''s Perspective](https://www.reddit.com/r/github/comments/1mgvust/promting_to_login_circumventing_that_leaves_me/)
- [Are There Perks to Using GitHub Pages for Web Tool Hosting Over Amateur Hosting Sites?](https://www.reddit.com/r/github/comments/1mgjz65/are_there_perks_to_using_github_pages_for_web/)
- [How viable is it to use Github Codespaces on an iPad 11inch with BT Keyboard/Mouse combo for college?](https://www.reddit.com/r/github/comments/1mgg60v/how_viable_is_it_to_use_github_codespaces_on_an/)
- [From First Commits to Big Ships: Announcing the GitHub Open Source Podcast](https://github.blog/open-source/maintainers/from-first-commits-to-big-ships-tune-into-our-new-open-source-podcast/)
- [Releases and Tags Disappearing: Troubleshooting GitHub Branch and Tag Issues](https://www.reddit.com/r/github/comments/1mfuewy/releases_and_tags_disappearing/)

### Zero-Downtime Deployments

Strategies for zero-downtime updates in Celery and other distributed job processors emphasize staggered rollouts and worker draining, foundational for critical workloads.

- [Long Running Celery Tasks with Zero Downtime Updates](https://www.reddit.com/r/devops/comments/1mfq8ri/long_running_celery_tasks_with_zero_downtime/)',
     excerpt      = 'DevOps saw deepening observability, AI automation, secure config management, maturing toolchains, and best-practices for policy, IaC, and real-world deployments—reflecting a domain balancing technical change and organizational growth.',
     content_hash = md5('DevOps saw deepening observability, AI automation, secure config management, maturing toolchains, and best-practices for policy, IaC, and real-world deployments—reflecting a domain balancing technical change and organizational growth.

<!--excerpt_end-->

## DevOps

### Observability Matures in Hybrid Environments

Organizations are moving beyond firewall-centric monitoring to full-stack observability—combining Internet Performance Monitoring, Real User Monitoring, and Synthetic Monitoring for comprehensive insight and rapid, DevSecOps-aligned incident response.

- [Beyond the Firewall - Achieving True Observability in Hybrid Infrastructure](https://devops.com/beyond-the-firewall-achieving-true-observability-in-the-era-of-hybrid-infrastructure/?utm_source=rss&utm_medium=rss&utm_campaign=beyond-the-firewall-achieving-true-observability-in-the-era-of-hybrid-infrastructure)
- [Observability in Retail: How to Monitor and Manage Interactive Kiosk Fleets](https://devops.com/observability-in-retail-how-to-monitor-and-manage-interactive-kiosk-fleets/?utm_source=rss&utm_medium=rss&utm_campaign=observability-in-retail-how-to-monitor-and-manage-interactive-kiosk-fleets)
- [Why Observability Isn’t Just for SREs (and How Devs Can Get Started)](https://www.reddit.com/r/devops/comments/1mfsvq8/why_observability_isnt_just_for_sres_and_how_devs/)

### AI and Automation Expand Productivity

AI extensions for Azure DevOps automate pull request reviews, cut review time, and surface security issues, while BMC brings AI-driven insight to mainframe DevOps. The trend is toward freeing humans for higher-value work, with strong privacy and data control options.

- [Building an AI Extension to Enhance Azure DevOps Pull Request Reviews](https://www.reddit.com/r/azuredevops/comments/1mdoa94/built_an_ai_extension_that_actually_makes_azure/)
- [BMC Extends Scope and Reach of DevOps Mainframe Workflows](https://devops.com/bmc-extends-scope-and-reach-of-devops-mainframe-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=bmc-extends-scope-and-reach-of-devops-mainframe-workflows)
- [Redefining Engineering Excellence: Amplifying Impact with Product Skills in the AI Era](https://devops.com/redefining-engineering-excellence-how-product-skills-amplify-your-impact-in-the-era-of-ai/?utm_source=rss&utm_medium=rss&utm_campaign=redefining-engineering-excellence-how-product-skills-amplify-your-impact-in-the-era-of-ai)

### Managing Secrets and Config at Scale

Centralizing secrets via AWS Parameter Store and automating Kubernetes sealed-secrets are now best practice for scaling microservices and delivery pipelines securely. Teams are reminded to avoid storing sensitive data in public source and lean on runtime secret injection.

- [How we solved environment variable chaos for 40+ microservices on ECS/Lambda/Batch with AWS Parameter Store](https://www.reddit.com/r/devops/comments/1mgl9tl/how_we_solved_environment_variable_chaos_for_40/)
- [[kubeseal] Built a small tool to make bitnami''s sealed-secrets less painful in GitOps](https://www.reddit.com/r/devops/comments/1mfshis/kubeseal_built_a_small_tool_to_make_bitnamis/)
- [How to Keep key.properties Private in a Public GitHub Repository](https://www.reddit.com/r/github/comments/1mgrtc3/private_file_in_github_repo/)

### Modern Toolchains and Deployment Orchestration

Microsoft Aspire positions itself as a multi-language DevOps “IDE” for managing distributed deployments, joined by SchemaNest for schema management and actionable guidance on CI/CD pipeline structuring and service connection automation.

- [Aspire: A Modern DevOps Toolchain](https://medium.com/@davidfowl/aspire-a-modern-devops-toolchain-fa5aac019d64)
- [SchemaNest: A Fast, Team-Friendly CI/CD-Ready JSON Schema Registry](https://www.reddit.com/r/devops/comments/1mg1fl8/schemanest_where_schemas_grow_thrive_and_scale/)
- [Structuring CI/CD Pipelines Across Code and Helm Chart Repositories in Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1mfmw05/how_to_structure_cicd_pipelines_across_two_repos/)
- [Automating Azure DevOps Service Connection Creation via Release Pipelines](https://www.reddit.com/r/azuredevops/comments/1mgg9wy/release_pipeline_for_creating_serviceconnections/)

### IaC and Compliance Best Practices

Terraform provider guides for Microsoft Fabric and Terraform Associate exam tips reflect ongoing organizational focus on codifying and securing infrastructure, with real-world experience emphasizing compliance and practical deployment.

- [Terraform Provider for Microsoft Fabric: #3 Creating a Workload Identity with Fabric Permissions](https://blog.fabric.microsoft.com/en-US/blog/terraform-provider-for-microsoft-fabric-3-creating-a-workload-identity-with-fabric-permissions/)
- [Terraform Associate (003) Exam – Sharing Study Resources That Helped Me Pass](https://www.reddit.com/r/devops/comments/1mgm77r/terraform_associate_003_exam_sharing_study/)

### Release, Handoff, and Deployment Versioning

Agency teams are tackling versioning and client hand-off with checklists and dedicated tool discussions; practical pain points often center on mapping independently versioned components for diverse customers.

- [Order of Operations for Web Agency: Building, Deploying, and Transferring Client Websites](https://www.reddit.com/r/github/comments/1mgba7n/need_help_web_ai_agency/)
- [Deployment versioning challenges across customers and components](https://www.reddit.com/r/devops/comments/1mfy3o0/deployment_versioning_problems/)

### Workflow, Shift Left, and DevOps Careers

The “shift left” vs. “shove left” distinction is emphasized—empowering devs with tools/process is key, not just dumping more work. Step-by-step roadmaps help backend engineers transition to fully skilled DevOps practitioners.

- [“Shove Left” – Dumping Downstream Tasks Onto Developers – A Recipe for Failure](https://devops.com/shove-left-dumping-downstream-tasks-onto-developers-a-recipe-for-failure/?utm_source=rss&utm_medium=rss&utm_campaign=shove-left-dumping-downstream-tasks-onto-developers-a-recipe-for-failure)
- [Transitioning from Backend Developer to DevOps](https://www.reddit.com/r/devops/comments/1mgx0a4/transitioning_from_backend_developer_to_devops/)

### Azure DevOps Workflows and Policy

Teams reviewed backlog and PR merging policies, service connection scripting, and repo-split CI/CD pipeline management, as workflow reliability and productivity remain major themes.

- [Questions About Azure DevOps Backlogs: Closed Work Items & Iteration Filtering](https://www.reddit.com/r/azuredevops/comments/1me1ts0/devops_backlog_questions/)
- [Enforcing PR Branch Policies with Multiple Required Pipelines in Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1mbguxq/how_to_only_allow_prs_if_pipelines_x_y_both_run/)
- [Automating Azure DevOps Service Connection Creation via Release Pipelines](https://www.reddit.com/r/azuredevops/comments/1mgg9wy/release_pipeline_for_creating_serviceconnections/)
- [Structuring CI/CD Pipelines Across Code and Helm Chart Repositories in Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1mfmw05/how_to_structure_cicd_pipelines_across_two_repos/)

### Blazor, Web Delivery, and Code Coverage

Blazor’s streamlined .NET delivery is gaining traction, while teams address CI coverage limitations with creative open-source and Makefile/CMake practices.

- [DevOps Meets Blazor in 2025 - Streamlining .NET Web App Delivery for the Future](https://devops.com/devops-meets-blazor-in-2025-streamlining-net-web-app-delivery-for-the-future/?utm_source=rss&utm_medium=rss&utm_campaign=devops-meets-blazor-in-2025-streamlining-net-web-app-delivery-for-the-future)
- [Unit Test Code Coverage Options in VS 2022 Pro for C Projects](https://www.reddit.com/r/VisualStudio/comments/1md4xq8/how_to_get_unit_test_code_coverage_using_vs_2022/)

### Emerging Trends: DevSecOps and Sustainability

Trends point to embedded security, scalability, and environmentally conscious DevOps as critical next frontiers.

- [Emerging DevOps Trends: Security, Scalability and Sustainability](https://devops.com/emerging-devops-trends-security-scalability-and-sustainability/?utm_source=rss&utm_medium=rss&utm_campaign=emerging-devops-trends-security-scalability-and-sustainability)

### Community and Ecosystem Updates

Ecosystem chatter addressed GitHub UI bugs, access friction, static hosting tradeoffs, and new podcasts, highlighting ongoing community adaptation and platform evolution.

- [[Bug] “Commit changes” button remains active during GitHub file upload — causes incomplete commits](https://www.reddit.com/r/github/comments/1mblpfk/bug_commit_changes_button_remains_active_during/)
- [Login Prompts and Access Restrictions on GitHub: Privacy Concerns From a User''s Perspective](https://www.reddit.com/r/github/comments/1mgvust/promting_to_login_circumventing_that_leaves_me/)
- [Are There Perks to Using GitHub Pages for Web Tool Hosting Over Amateur Hosting Sites?](https://www.reddit.com/r/github/comments/1mgjz65/are_there_perks_to_using_github_pages_for_web/)
- [How viable is it to use Github Codespaces on an iPad 11inch with BT Keyboard/Mouse combo for college?](https://www.reddit.com/r/github/comments/1mgg60v/how_viable_is_it_to_use_github_codespaces_on_an/)
- [From First Commits to Big Ships: Announcing the GitHub Open Source Podcast](https://github.blog/open-source/maintainers/from-first-commits-to-big-ships-tune-into-our-new-open-source-podcast/)
- [Releases and Tags Disappearing: Troubleshooting GitHub Branch and Tag Issues](https://www.reddit.com/r/github/comments/1mfuewy/releases_and_tags_disappearing/)

### Zero-Downtime Deployments

Strategies for zero-downtime updates in Celery and other distributed job processors emphasize staggered rollouts and worker draining, foundational for critical workloads.

- [Long Running Celery Tasks with Zero Downtime Updates](https://www.reddit.com/r/devops/comments/1mfq8ri/long_running_celery_tasks_with_zero_downtime/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-08-04';

-- weekly-devops-roundup-2025-08-11  (2025-08-11)
UPDATE content_items
SET  content      = 'DevOps this week centered on AI acceleration, rapid tool improvements, secure automation, and pragmatic guidance for cloud-scale workflows.

<!--excerpt_end-->

## DevOps

### AI Integration Accelerates and Enriches DevOps Automation

Microsoft’s stack now tightly combines Copilot, Azure DevOps, and Fabric for real-time code generation, automated CI/CD, risk-aware observability, and responsible ML delivery. These advances—building on last week’s trends—reinforce resilience and adaptability.

- [DevOps Meets Microsoft AI: Accelerating Innovation in the Cloud Era](https://dellenny.com/devops-meets-microsoft-ai-accelerating-innovation-in-the-cloud-era/)

### Blazing Fast AI Code Generation

AI code gen at 2,000 tokens/sec, powered by WSE-3 hardware and Qwen3, ushers in “flow state” programming and opens rapid, democratized DevOps acceleration, especially for junior enablement and open-source model deployment.

- [The Evolution of DevOps: Impact of 2,000 Token-Per-Second AI Code Generation](https://devops.com/the-evolution-of-devops-continues-how-2000-token-per-second-ai-code-generation-changes-everything/?utm_source=rss&utm_medium=rss&utm_campaign=the-evolution-of-devops-continues-how-2000-token-per-second-ai-code-generation-changes-everything)

### Upgrades to Essential Tooling: Dependabot & Playwright

Native .NET Dependabot NuGet updater cuts update time 65%, improves PRs, and handles complex dependencies without configuration changes. Playwright now integrates with AI agents, supports multi-language automation, and adds deep observability—majorly reducing test maintenance.

- [The new Dependabot NuGet updater: 65% faster with native .NET](https://devblogs.microsoft.com/dotnet/the-new-dependabot-nuget-updater/)

### Agent Workflows and Secure Context

MCP servers and agent-to-agent protocols mature as context brokers for AI-driven pipelines, improving modular, secure automation. Artifact scaling, retention, and supply chain security are now central, as build frequencies soar.

- [Context on Tap: How MCP Servers Bridge AI Agents and DevOps Pipelines](https://devops.com/context-on-tap-how-mcp-servers-bridge-ai-agents-and-devops-pipelines/?utm_source=rss&utm_medium=rss&utm_campaign=context-on-tap-how-mcp-servers-bridge-ai-agents-and-devops-pipelines)

### Streamlining Cloud Deployments

Best practices for Azure deployments now emphasize separating build/deploy, using environment variables, and immutable infrastructure. Microsoft Fabric now allows 20 schedulers per pipeline/job, improving CI/CD for enterprise data and ML.

- [Tokenization Task Alternatives for Cross-Platform Azure App Service Deployments](https://www.reddit.com/r/azuredevops/comments/1mk7u14/better_solidify_tokenization_task/)

### Secretless Deployments and Infrastructure as Code

Microsoft Fabric’s guidance enables secretless GitHub Action deployments via OIDC and RBAC, with YAML and Terraform bringing compliance-ready automation.

- [Terraform Provider for Microsoft Fabric: Deploying Fabric Configs with GitHub Actions](https://blog.fabric.microsoft.com/en-US/blog/terraform-provider-for-microsoft-fabric-4-deploying-a-fabric-config-with-terraform-in-github-actions/)

### Advanced Simulation and AI-Stabilized Pipelines

AI-driven simulation (PlayerZero’s CodeSim) and semantic health checks help DevOps teams catch hidden errors and speed incident triage in hybrid and generative AI pipelines.

- [PlayerZero Introduces AI-Driven Code Simulation with CodeSim](https://devops.com/playerzero-adds-ability-to-simulate-code-to-ai-platform/?utm_source=rss&utm_medium=rss&utm_campaign=playerzero-adds-ability-to-simulate-code-to-ai-platform)

### CI/CD: Persistent Friction and Best Practices

CI maturity provides rapid testing and early security, yet real-world frictions—especially in scaling and organizational adoption—persist. True DevOps demands organizational investment, not just CI tools.

- [Why Continuous Integration Matters More Than Ever](https://devops.com/why-continuous-integration-matters-more-than-ever/?utm_source=rss&utm_medium=rss&utm_campaign=why-continuous-integration-matters-more-than-ever)

### Tooling Evolution: GitHub, Jira, SaaS Administration

GitHub platform refinements (e.g., tab size change, SSO banners) reduce distraction and align code. Jira debates highlight usability, over-customization risks, and IaC for tooling/admin scale.

- [GitHub Changes Default Tab Size from Eight to Four Spaces](https://github.blog/changelog/2025-08-07-default-tab-size-changed-from-eight-to-four)

### Managing Workflows in Changing Ecosystems

Updates like Dependabot reviewer retirement and Events API payload changes drive teams to centralize PR controls and update automation. Legal and feature adoption barriers are dropping as GitHub improves indemnity/pre-release processes.

- [Dependabot Reviewers Option Removed in Favor of GitHub Code Owners](https://github.blog/changelog/2025-08-08-dependabot-reviewers-configuration-option-is-replaced-by-code-owners)

### From Incidents to IaC Migrations

Mature incident management emphasizes blameless postmortems, actionable followups, and declarative IaC. Open-source Datadog-to-Terraform migrations and Git-based reviews enforce healthy change management.

- [From Incidents to Insights: The Power of Blameless Postmortems](https://devops.com/from-incidents-to-insights-the-power-of-blameless-postmortems/?utm_source=rss&utm_medium=rss&utm_campaign=from-incidents-to-insights-the-power-of-blameless-postmortems)

DevOps is now defined by AI-infused automation, consistent practice, and resilient, well-governed workflows.',
     excerpt      = 'DevOps this week centered on AI acceleration, rapid tool improvements, secure automation, and pragmatic guidance for cloud-scale workflows.',
     content_hash = md5('DevOps this week centered on AI acceleration, rapid tool improvements, secure automation, and pragmatic guidance for cloud-scale workflows.

<!--excerpt_end-->

## DevOps

### AI Integration Accelerates and Enriches DevOps Automation

Microsoft’s stack now tightly combines Copilot, Azure DevOps, and Fabric for real-time code generation, automated CI/CD, risk-aware observability, and responsible ML delivery. These advances—building on last week’s trends—reinforce resilience and adaptability.

- [DevOps Meets Microsoft AI: Accelerating Innovation in the Cloud Era](https://dellenny.com/devops-meets-microsoft-ai-accelerating-innovation-in-the-cloud-era/)

### Blazing Fast AI Code Generation

AI code gen at 2,000 tokens/sec, powered by WSE-3 hardware and Qwen3, ushers in “flow state” programming and opens rapid, democratized DevOps acceleration, especially for junior enablement and open-source model deployment.

- [The Evolution of DevOps: Impact of 2,000 Token-Per-Second AI Code Generation](https://devops.com/the-evolution-of-devops-continues-how-2000-token-per-second-ai-code-generation-changes-everything/?utm_source=rss&utm_medium=rss&utm_campaign=the-evolution-of-devops-continues-how-2000-token-per-second-ai-code-generation-changes-everything)

### Upgrades to Essential Tooling: Dependabot & Playwright

Native .NET Dependabot NuGet updater cuts update time 65%, improves PRs, and handles complex dependencies without configuration changes. Playwright now integrates with AI agents, supports multi-language automation, and adds deep observability—majorly reducing test maintenance.

- [The new Dependabot NuGet updater: 65% faster with native .NET](https://devblogs.microsoft.com/dotnet/the-new-dependabot-nuget-updater/)

### Agent Workflows and Secure Context

MCP servers and agent-to-agent protocols mature as context brokers for AI-driven pipelines, improving modular, secure automation. Artifact scaling, retention, and supply chain security are now central, as build frequencies soar.

- [Context on Tap: How MCP Servers Bridge AI Agents and DevOps Pipelines](https://devops.com/context-on-tap-how-mcp-servers-bridge-ai-agents-and-devops-pipelines/?utm_source=rss&utm_medium=rss&utm_campaign=context-on-tap-how-mcp-servers-bridge-ai-agents-and-devops-pipelines)

### Streamlining Cloud Deployments

Best practices for Azure deployments now emphasize separating build/deploy, using environment variables, and immutable infrastructure. Microsoft Fabric now allows 20 schedulers per pipeline/job, improving CI/CD for enterprise data and ML.

- [Tokenization Task Alternatives for Cross-Platform Azure App Service Deployments](https://www.reddit.com/r/azuredevops/comments/1mk7u14/better_solidify_tokenization_task/)

### Secretless Deployments and Infrastructure as Code

Microsoft Fabric’s guidance enables secretless GitHub Action deployments via OIDC and RBAC, with YAML and Terraform bringing compliance-ready automation.

- [Terraform Provider for Microsoft Fabric: Deploying Fabric Configs with GitHub Actions](https://blog.fabric.microsoft.com/en-US/blog/terraform-provider-for-microsoft-fabric-4-deploying-a-fabric-config-with-terraform-in-github-actions/)

### Advanced Simulation and AI-Stabilized Pipelines

AI-driven simulation (PlayerZero’s CodeSim) and semantic health checks help DevOps teams catch hidden errors and speed incident triage in hybrid and generative AI pipelines.

- [PlayerZero Introduces AI-Driven Code Simulation with CodeSim](https://devops.com/playerzero-adds-ability-to-simulate-code-to-ai-platform/?utm_source=rss&utm_medium=rss&utm_campaign=playerzero-adds-ability-to-simulate-code-to-ai-platform)

### CI/CD: Persistent Friction and Best Practices

CI maturity provides rapid testing and early security, yet real-world frictions—especially in scaling and organizational adoption—persist. True DevOps demands organizational investment, not just CI tools.

- [Why Continuous Integration Matters More Than Ever](https://devops.com/why-continuous-integration-matters-more-than-ever/?utm_source=rss&utm_medium=rss&utm_campaign=why-continuous-integration-matters-more-than-ever)

### Tooling Evolution: GitHub, Jira, SaaS Administration

GitHub platform refinements (e.g., tab size change, SSO banners) reduce distraction and align code. Jira debates highlight usability, over-customization risks, and IaC for tooling/admin scale.

- [GitHub Changes Default Tab Size from Eight to Four Spaces](https://github.blog/changelog/2025-08-07-default-tab-size-changed-from-eight-to-four)

### Managing Workflows in Changing Ecosystems

Updates like Dependabot reviewer retirement and Events API payload changes drive teams to centralize PR controls and update automation. Legal and feature adoption barriers are dropping as GitHub improves indemnity/pre-release processes.

- [Dependabot Reviewers Option Removed in Favor of GitHub Code Owners](https://github.blog/changelog/2025-08-08-dependabot-reviewers-configuration-option-is-replaced-by-code-owners)

### From Incidents to IaC Migrations

Mature incident management emphasizes blameless postmortems, actionable followups, and declarative IaC. Open-source Datadog-to-Terraform migrations and Git-based reviews enforce healthy change management.

- [From Incidents to Insights: The Power of Blameless Postmortems](https://devops.com/from-incidents-to-insights-the-power-of-blameless-postmortems/?utm_source=rss&utm_medium=rss&utm_campaign=from-incidents-to-insights-the-power-of-blameless-postmortems)

DevOps is now defined by AI-infused automation, consistent practice, and resilient, well-governed workflows.'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-08-11';

-- weekly-devops-roundup-2025-08-18  (2025-08-18)
UPDATE content_items
SET  content      = 'DevOps is seeing another round of automation, improved workflow features, and more ways to manage releases and access securely. New AI agents, stricter policies, and improved collaboration reflect a steady shift toward streamlined, well-governed developer operations.

<!--excerpt_end-->

## DevOps

### The Rise of AI Agents and Automation in DevOps

Google’s Gemini CLI GitHub Actions (beta) bring “AI teammate” capabilities for issue triage, reviews, and more—complete with allowlisting, Workload Identity integration, OpenTelemetry monitoring, and customizable workflows. Free quotas help lower the cost of getting started.

Shadow, a secure, open-source AI coding agent, is designed for production pipelines with semantic search and automatic documentation—helping handle technical debt and supporting both collaborative and automated DevOps patterns.

- [How Gemini CLI GitHub Actions is Changing Developer Workflows](https://devops.com/how-gemini-cli-github-actions-is-changing-developer-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=how-gemini-cli-github-actions-is-changing-developer-workflows)
- [Shadow: How AI Coding Agents are Transforming DevOps Workflows](https://devops.com/shadow-how-ai-coding-agents-are-transforming-devops-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=shadow-how-ai-coding-agents-are-transforming-devops-workflows)

### Security and Policy Enhancements: Supply Chain and Workflow Hardening

GitHub Actions now supports blocking/versioning and SHA pinning, making it possible to harden CI/CD supply chains and guarantee artifact integrity. Fast incident response and automated governance help address new security threats as the platform evolves.

- [GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions)

### Streamlined Dev to Production Workflows with Modern CI/CD and IaC

A “Dev to Prod” guide outlines how to use Azure Developer CLI with DevOps YAML pipelines for efficient builds, artifact handling, and Copilot-driven diagnostics. This matches ongoing trends toward better, faster development-to-production workflows.

- [Azure Developer CLI: Dev to Production with Azure DevOps Pipelines](https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-azure-devops-pipelines/)

### Workflow Improvements for Visibility, Notifications, and Collaboration

GitHub has enhanced reviewer visibility in pull requests, improved email filters, and expanded supported file types for attachments—further smoothing team workflow and onboarding processes.

- [Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub](https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering)
- [Expanded File Type Support for GitHub Attachments](https://github.blog/changelog/2025-08-13-expanded-file-type-support-for-attachments-across-issues-pull-requests-and-discussions)

### DevOps Release Management: Bottlenecks and Opportunities

A recent survey of mobile app release practices finds high manual effort and frequent interruptions, highlighting opportunities for better automation and more reliable CI/CD pipelines.

- [Survey Reveals Major Challenges in Mobile Application Release Management](https://devops.com/survey-surfaces-multiple-mobile-application-release-management-headaches/?utm_source=rss&utm_medium=rss&utm_campaign=survey-surfaces-multiple-mobile-application-release-management-headaches)

### Enhancements in Application Monitoring and Dependency Management

AppSignal now offers zero-config OpenTelemetry monitoring for mainstream languages, while Dependabot adds vcpkg update automation for C/C++—making security and dependency management easier in native codebases.

- [AppSignal Adds Native OpenTelemetry Support for Enhanced Application Monitoring](https://devops.com/appsignal-adds-opentelemetry-support-to-monitoring-platform/?utm_source=rss&utm_medium=rss&utm_campaign=appsignal-adds-opentelemetry-support-to-monitoring-platform)
- [Dependabot Adds Version Update Support for vcpkg](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)

### Migration, Incident, and Access Management in Complex Environments

After a GitHub Enterprise Importer outage, stronger testing and firewall management were put in place. Visual Studio subscribers can now access metered enterprise billing. An ITU open-source migration guide provides a four-step model, supporting teams moving from private to public projects.

- [GitHub Enterprise Importer Incident and IP Range Update: July 2025 Availability Report](https://github.blog/news-insights/company-news/github-availability-report-july-2025/)
- [Metered GitHub Enterprise Billing Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-08-14-introducing-metered-github-enterprise-billing-for-visual-studio-subscriptions-with-github-enterprise)
- [How the International Telecommunication Union Open Sourced Its Tech: A Four-Step Guide](https://github.blog/open-source/social-impact/from-private-to-public-how-a-united-nations-organization-open-sourced-its-tech-in-four-steps/)

### Other DevOps News

Dev tools continue to receive attention, with new OpenTelemetry features in AppSignal, simplified dependency updates via Dependabot, and more collaborative GitHub features. Security advances include improved Actions policy controls and user management APIs. There’s also updated guidance on migration, incident handling, and real-world DevOps lessons from practitioners.

- [AppSignal Adds Native OpenTelemetry Support for Enhanced Application Monitoring](https://devops.com/appsignal-adds-opentelemetry-support-to-monitoring-platform/?utm_source=rss&utm_medium=rss&utm_campaign=appsignal-adds-opentelemetry-support-to-monitoring-platform)
- [Dependabot Adds Version Update Support for vcpkg](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)
- [Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub](https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering)
- [Expanded File Type Support for GitHub Attachments](https://github.blog/changelog/2025-08-13-expanded-file-type-support-for-attachments-across-issues-pull-requests-and-discussions)
- [GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions)
- [Metered GitHub Enterprise Billing Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-08-14-introducing-metered-github-enterprise-billing-for-visual-studio-subscriptions-with-github-enterprise)
- [How the International Telecommunication Union Open Sourced Its Tech: A Four-Step Guide](https://github.blog/open-source/social-impact/from-private-to-public-how-a-united-nations-organization-open-sourced-its-tech-in-four-steps/)
- [GitHub Enterprise Importer Incident and IP Range Update: July 2025 Availability Report](https://github.blog/news-insights/company-news/github-availability-report-july-2025/)
- [Persistent Visual Studio Enterprise Access Level in Azure DevOps After License Removal](https://techcommunity.microsoft.com/t5/azure/unable-to-revert-azure-devops-user-access-level/m-p/4442871#M22102)
- [Troubleshooting MCC Phantom Install Issues on Windows Server 2022 with WSL](https://techcommunity.microsoft.com/t5/microsoft-connected-cache-for/mcc-phantom-install/m-p/4444201#M108)
- [From Firefighting to Forward-Thinking: Real-World Lessons in DevOps and Cloud Engineering](https://devops.com/from-firefighting-to-forward-thinking-my-real-world-lessons-in-devops-and-cloud-engineering/?utm_source=rss&utm_medium=rss&utm_campaign=from-firefighting-to-forward-thinking-my-real-world-lessons-in-devops-and-cloud-engineering)',
     excerpt      = 'DevOps is seeing another round of automation, improved workflow features, and more ways to manage releases and access securely. New AI agents, stricter policies, and improved collaboration reflect a steady shift toward streamlined, well-governed developer operations.',
     content_hash = md5('DevOps is seeing another round of automation, improved workflow features, and more ways to manage releases and access securely. New AI agents, stricter policies, and improved collaboration reflect a steady shift toward streamlined, well-governed developer operations.

<!--excerpt_end-->

## DevOps

### The Rise of AI Agents and Automation in DevOps

Google’s Gemini CLI GitHub Actions (beta) bring “AI teammate” capabilities for issue triage, reviews, and more—complete with allowlisting, Workload Identity integration, OpenTelemetry monitoring, and customizable workflows. Free quotas help lower the cost of getting started.

Shadow, a secure, open-source AI coding agent, is designed for production pipelines with semantic search and automatic documentation—helping handle technical debt and supporting both collaborative and automated DevOps patterns.

- [How Gemini CLI GitHub Actions is Changing Developer Workflows](https://devops.com/how-gemini-cli-github-actions-is-changing-developer-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=how-gemini-cli-github-actions-is-changing-developer-workflows)
- [Shadow: How AI Coding Agents are Transforming DevOps Workflows](https://devops.com/shadow-how-ai-coding-agents-are-transforming-devops-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=shadow-how-ai-coding-agents-are-transforming-devops-workflows)

### Security and Policy Enhancements: Supply Chain and Workflow Hardening

GitHub Actions now supports blocking/versioning and SHA pinning, making it possible to harden CI/CD supply chains and guarantee artifact integrity. Fast incident response and automated governance help address new security threats as the platform evolves.

- [GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions)

### Streamlined Dev to Production Workflows with Modern CI/CD and IaC

A “Dev to Prod” guide outlines how to use Azure Developer CLI with DevOps YAML pipelines for efficient builds, artifact handling, and Copilot-driven diagnostics. This matches ongoing trends toward better, faster development-to-production workflows.

- [Azure Developer CLI: Dev to Production with Azure DevOps Pipelines](https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-azure-devops-pipelines/)

### Workflow Improvements for Visibility, Notifications, and Collaboration

GitHub has enhanced reviewer visibility in pull requests, improved email filters, and expanded supported file types for attachments—further smoothing team workflow and onboarding processes.

- [Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub](https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering)
- [Expanded File Type Support for GitHub Attachments](https://github.blog/changelog/2025-08-13-expanded-file-type-support-for-attachments-across-issues-pull-requests-and-discussions)

### DevOps Release Management: Bottlenecks and Opportunities

A recent survey of mobile app release practices finds high manual effort and frequent interruptions, highlighting opportunities for better automation and more reliable CI/CD pipelines.

- [Survey Reveals Major Challenges in Mobile Application Release Management](https://devops.com/survey-surfaces-multiple-mobile-application-release-management-headaches/?utm_source=rss&utm_medium=rss&utm_campaign=survey-surfaces-multiple-mobile-application-release-management-headaches)

### Enhancements in Application Monitoring and Dependency Management

AppSignal now offers zero-config OpenTelemetry monitoring for mainstream languages, while Dependabot adds vcpkg update automation for C/C++—making security and dependency management easier in native codebases.

- [AppSignal Adds Native OpenTelemetry Support for Enhanced Application Monitoring](https://devops.com/appsignal-adds-opentelemetry-support-to-monitoring-platform/?utm_source=rss&utm_medium=rss&utm_campaign=appsignal-adds-opentelemetry-support-to-monitoring-platform)
- [Dependabot Adds Version Update Support for vcpkg](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)

### Migration, Incident, and Access Management in Complex Environments

After a GitHub Enterprise Importer outage, stronger testing and firewall management were put in place. Visual Studio subscribers can now access metered enterprise billing. An ITU open-source migration guide provides a four-step model, supporting teams moving from private to public projects.

- [GitHub Enterprise Importer Incident and IP Range Update: July 2025 Availability Report](https://github.blog/news-insights/company-news/github-availability-report-july-2025/)
- [Metered GitHub Enterprise Billing Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-08-14-introducing-metered-github-enterprise-billing-for-visual-studio-subscriptions-with-github-enterprise)
- [How the International Telecommunication Union Open Sourced Its Tech: A Four-Step Guide](https://github.blog/open-source/social-impact/from-private-to-public-how-a-united-nations-organization-open-sourced-its-tech-in-four-steps/)

### Other DevOps News

Dev tools continue to receive attention, with new OpenTelemetry features in AppSignal, simplified dependency updates via Dependabot, and more collaborative GitHub features. Security advances include improved Actions policy controls and user management APIs. There’s also updated guidance on migration, incident handling, and real-world DevOps lessons from practitioners.

- [AppSignal Adds Native OpenTelemetry Support for Enhanced Application Monitoring](https://devops.com/appsignal-adds-opentelemetry-support-to-monitoring-platform/?utm_source=rss&utm_medium=rss&utm_campaign=appsignal-adds-opentelemetry-support-to-monitoring-platform)
- [Dependabot Adds Version Update Support for vcpkg](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)
- [Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub](https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering)
- [Expanded File Type Support for GitHub Attachments](https://github.blog/changelog/2025-08-13-expanded-file-type-support-for-attachments-across-issues-pull-requests-and-discussions)
- [GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions)
- [Metered GitHub Enterprise Billing Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-08-14-introducing-metered-github-enterprise-billing-for-visual-studio-subscriptions-with-github-enterprise)
- [How the International Telecommunication Union Open Sourced Its Tech: A Four-Step Guide](https://github.blog/open-source/social-impact/from-private-to-public-how-a-united-nations-organization-open-sourced-its-tech-in-four-steps/)
- [GitHub Enterprise Importer Incident and IP Range Update: July 2025 Availability Report](https://github.blog/news-insights/company-news/github-availability-report-july-2025/)
- [Persistent Visual Studio Enterprise Access Level in Azure DevOps After License Removal](https://techcommunity.microsoft.com/t5/azure/unable-to-revert-azure-devops-user-access-level/m-p/4442871#M22102)
- [Troubleshooting MCC Phantom Install Issues on Windows Server 2022 with WSL](https://techcommunity.microsoft.com/t5/microsoft-connected-cache-for/mcc-phantom-install/m-p/4444201#M108)
- [From Firefighting to Forward-Thinking: Real-World Lessons in DevOps and Cloud Engineering](https://devops.com/from-firefighting-to-forward-thinking-my-real-world-lessons-in-devops-and-cloud-engineering/?utm_source=rss&utm_medium=rss&utm_campaign=from-firefighting-to-forward-thinking-my-real-world-lessons-in-devops-and-cloud-engineering)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-08-18';

-- weekly-devops-roundup-2025-08-25  (2025-08-25)
UPDATE content_items
SET  content      = 'This week’s DevOps updates include new features and integrations to support more reliable workflows, stronger observability, and increased use of AI for automation, all while emphasizing oversight and collaboration. GitHub adds improved permissions, dependency management, and UI features. AI is being integrated into CI/CD, combining productivity with careful governance. Practical guides help troubleshoot Kubernetes, automate Angular CI, and link Azure DevOps with Jira. The ecosystem maintains a focus on cost, quality, and productivity.

<!--excerpt_end-->

## DevOps

### GitHub Platform Enhancements and Developer Workflow Updates

GitHub’s recent updates include general availability for issue dependency management, support for enterprise-level custom organization roles and increased limits, pull request improvements, and Dependabot Rust toolchain automation. New features for cost attribution and repository migration align with GitHub’s focus on usability and admin features, and the retirement of GraphQL Explorer reflects ongoing documentation and API enhancements.

- [Managing Issue Dependencies in GitHub Now Generally Available](https://github.blog/changelog/2025-08-21-dependencies-on-issues)
- [Enterprise-Wide Custom Organization Roles and Increased Role Limits in GitHub](https://github.blog/changelog/2025-08-21-enterprises-can-create-organization-roles-for-use-across-their-enterprise-and-custom-role-limits-have-been-increased)
- [GitHub Pull Request ''Files Changed'' Public Preview: August 21 Updates](https://github.blog/changelog/2025-08-21-pull-request-files-changed-public-preview-experience-august-21-updates)
- [Dependabot Adds Support for Automated Rust Toolchain Updates](https://github.blog/changelog/2025-08-19-dependabot-now-supports-rust-toolchain-updates)
- [Manage Cost Center Users in GitHub Enterprise Cloud via Billing UI and API](https://github.blog/changelog/2025-08-18-customers-can-now-add-users-to-a-cost-center-from-both-the-ui-and-api-2)
- [Migrate Repositories Using GitHub-Owned Blob Storage](https://github.blog/changelog/2025-08-18-migrate-repositories-with-github-owned-blob-storage)
- [GraphQL Explorer Removal from GitHub API Documentation in 2025](https://github.blog/changelog/2025-08-21-graphql-explorer-removal-from-api-documentation-on-november-1-2025)

### Advancing Observability and Kubernetes Troubleshooting

New tools such as Retina and eBPF for Kubernetes support deeper inspection and debugging for cloud workloads. These resources extend earlier distributed tracing and monitoring improvements, helping teams trace issues in modern networking environments.

- [Troubleshooting Kubernetes Network Issues with Retina and eBPF](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/troubleshooting-network-issues-with-retina/ba-p/4446071)

### AI-Driven Automation and the Evolution of DevOps Pipelines

Recent developments in AI integration for DevOps build on previous releases for agents, pipelines, and MCP tools. Discussions cover the potential and challenges of AI-driven orchestration in CI/CD, with articles emphasizing platform engineering, robust oversight, and the role of humans in overseeing automated, agent-based pipelines.

Contextual engineering is also stressed as necessary for safe and practical automation. Case studies illustrate the stepwise adoption of smarter, more context-rich automation practices.

- [How MCP Is Shaping the Future of DevOps Processes](https://devops.com/mcp-emerges-as-a-catalyst-for-modern-devops-processes/?utm_source=rss&utm_medium=rss&utm_campaign=mcp-emerges-as-a-catalyst-for-modern-devops-processes)
- [How AI-Created Code Will Strain DevOps Workflows](https://devops.com/how-ai-created-code-will-strain-devops-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=how-ai-created-code-will-strain-devops-workflows)
- [Unlocking DevOps-Ready AI Agents Through Context Engineering](https://devops.com/context-engineering-is-the-key-to-unlocking-ai-agents-in-devops/?utm_source=rss&utm_medium=rss&utm_campaign=context-engineering-is-the-key-to-unlocking-ai-agents-in-devops)
- [Why Human Oversight Remains Essential in an AI-Driven DevOps Landscape](https://devops.com/keeping-humans-in-the-loop-why-human-oversight-still-matters-in-an-ai-driven-devops-future/?utm_source=rss&utm_medium=rss&utm_campaign=keeping-humans-in-the-loop-why-human-oversight-still-matters-in-an-ai-driven-devops-future)
- [The Future of DevSecOps in Fully Autonomous CI/CD Pipelines](https://devops.com/white-paper-the-future-of-devsecops-in-a-fully-autonomous-ci-cd-pipeline/?utm_source=rss&utm_medium=rss&utm_campaign=white-paper-the-future-of-devsecops-in-a-fully-autonomous-ci-cd-pipeline)

### CI/CD Workflows, Testing, and Seamless Integrations

Workflow automation guides this week reflect ongoing trends toward secure, frictionless CI/CD pipelines. Articles cover Angular coverage enforcement in Azure DevOps and practical synchronization between Azure DevOps and Jira, supporting smoother testing, deployment, and coordination across development suites.

- [Enforcing Angular Unit Test Coverage in Azure DevOps Pipelines: A Step-by-Step Guide](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/enforcing-angular-unit-test-coverage-in-azure-devops-pipelines-a/ba-p/4446485)
- [Optimizing Azure DevOps and Jira Integration: 5 Real-World Use Cases for DevOps Teams](https://techcommunity.microsoft.com/t5/azure/optimizing-azure-devops-jira-integration-5-practical-use-cases/m-p/4445837#M22123)

### Observability, Debugging, and Production Reliability

Teams can further improve production operations with guidance on structured logging, metrics, and alerting. These resources are designed to help debug live systems and maintain high reliability, building on last week’s monitoring and incident response coverage.

- [Debugging in Production: Leveraging Logs, Metrics and Traces](https://devops.com/debugging-in-production-leveraging-logs-metrics-and-traces/?utm_source=rss&utm_medium=rss&utm_campaign=debugging-in-production-leveraging-logs-metrics-and-traces)

### The Expanding Ecosystem: AI-Powered Content, Fusion Development, and Cost Optimization

Ecosystem-wide updates include the introduction of tech.hub.ms, a platform for curated Microsoft technical content. Fusion development stories show increased adoption of blended business and engineering workflows; articles on FinOps as Code and SRE.ai explore automation and cost-conscious practices across SaaS and DevOps teams.

- [Announcing tech.hub.ms: Curated Microsoft Tech Content Platform](https://r-vm.com/new-website-tech-hub-ms.html)
- [Microsoft Morphs Fusion Developers To Full Stack Builders](https://devops.com/microsoft-morphs-fusion-developers-to-full-stack-builders/?utm_source=rss&utm_medium=rss&utm_campaign=microsoft-morphs-fusion-developers-to-full-stack-builders)
- [FinOps as Code – Unlocking Cloud Cost Optimization](https://devops.com/finops-as-code-unlocking-cloud-cost-optimization/?utm_source=rss&utm_medium=rss&utm_campaign=finops-as-code-unlocking-cloud-cost-optimization)
- [SRE.ai Aims to Streamline DevOps for SaaS with AI Automation](https://devops.com/sre-ai-looks-to-unify-devops-workflows-across-multiple-saas-applications/?utm_source=rss&utm_medium=rss&utm_campaign=sre-ai-looks-to-unify-devops-workflows-across-multiple-saas-applications)',
     excerpt      = 'This week’s DevOps updates include new features and integrations to support more reliable workflows, stronger observability, and increased use of AI for automation, all while emphasizing oversight and collaboration. GitHub adds improved permissions, dependency management, and UI features. AI is being integrated into CI/CD, combining productivity with careful governance. Practical guides help troubleshoot Kubernetes, automate Angular CI, and link Azure DevOps with Jira. The ecosystem maintains a focus on cost, quality, and productivity.',
     content_hash = md5('This week’s DevOps updates include new features and integrations to support more reliable workflows, stronger observability, and increased use of AI for automation, all while emphasizing oversight and collaboration. GitHub adds improved permissions, dependency management, and UI features. AI is being integrated into CI/CD, combining productivity with careful governance. Practical guides help troubleshoot Kubernetes, automate Angular CI, and link Azure DevOps with Jira. The ecosystem maintains a focus on cost, quality, and productivity.

<!--excerpt_end-->

## DevOps

### GitHub Platform Enhancements and Developer Workflow Updates

GitHub’s recent updates include general availability for issue dependency management, support for enterprise-level custom organization roles and increased limits, pull request improvements, and Dependabot Rust toolchain automation. New features for cost attribution and repository migration align with GitHub’s focus on usability and admin features, and the retirement of GraphQL Explorer reflects ongoing documentation and API enhancements.

- [Managing Issue Dependencies in GitHub Now Generally Available](https://github.blog/changelog/2025-08-21-dependencies-on-issues)
- [Enterprise-Wide Custom Organization Roles and Increased Role Limits in GitHub](https://github.blog/changelog/2025-08-21-enterprises-can-create-organization-roles-for-use-across-their-enterprise-and-custom-role-limits-have-been-increased)
- [GitHub Pull Request ''Files Changed'' Public Preview: August 21 Updates](https://github.blog/changelog/2025-08-21-pull-request-files-changed-public-preview-experience-august-21-updates)
- [Dependabot Adds Support for Automated Rust Toolchain Updates](https://github.blog/changelog/2025-08-19-dependabot-now-supports-rust-toolchain-updates)
- [Manage Cost Center Users in GitHub Enterprise Cloud via Billing UI and API](https://github.blog/changelog/2025-08-18-customers-can-now-add-users-to-a-cost-center-from-both-the-ui-and-api-2)
- [Migrate Repositories Using GitHub-Owned Blob Storage](https://github.blog/changelog/2025-08-18-migrate-repositories-with-github-owned-blob-storage)
- [GraphQL Explorer Removal from GitHub API Documentation in 2025](https://github.blog/changelog/2025-08-21-graphql-explorer-removal-from-api-documentation-on-november-1-2025)

### Advancing Observability and Kubernetes Troubleshooting

New tools such as Retina and eBPF for Kubernetes support deeper inspection and debugging for cloud workloads. These resources extend earlier distributed tracing and monitoring improvements, helping teams trace issues in modern networking environments.

- [Troubleshooting Kubernetes Network Issues with Retina and eBPF](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/troubleshooting-network-issues-with-retina/ba-p/4446071)

### AI-Driven Automation and the Evolution of DevOps Pipelines

Recent developments in AI integration for DevOps build on previous releases for agents, pipelines, and MCP tools. Discussions cover the potential and challenges of AI-driven orchestration in CI/CD, with articles emphasizing platform engineering, robust oversight, and the role of humans in overseeing automated, agent-based pipelines.

Contextual engineering is also stressed as necessary for safe and practical automation. Case studies illustrate the stepwise adoption of smarter, more context-rich automation practices.

- [How MCP Is Shaping the Future of DevOps Processes](https://devops.com/mcp-emerges-as-a-catalyst-for-modern-devops-processes/?utm_source=rss&utm_medium=rss&utm_campaign=mcp-emerges-as-a-catalyst-for-modern-devops-processes)
- [How AI-Created Code Will Strain DevOps Workflows](https://devops.com/how-ai-created-code-will-strain-devops-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=how-ai-created-code-will-strain-devops-workflows)
- [Unlocking DevOps-Ready AI Agents Through Context Engineering](https://devops.com/context-engineering-is-the-key-to-unlocking-ai-agents-in-devops/?utm_source=rss&utm_medium=rss&utm_campaign=context-engineering-is-the-key-to-unlocking-ai-agents-in-devops)
- [Why Human Oversight Remains Essential in an AI-Driven DevOps Landscape](https://devops.com/keeping-humans-in-the-loop-why-human-oversight-still-matters-in-an-ai-driven-devops-future/?utm_source=rss&utm_medium=rss&utm_campaign=keeping-humans-in-the-loop-why-human-oversight-still-matters-in-an-ai-driven-devops-future)
- [The Future of DevSecOps in Fully Autonomous CI/CD Pipelines](https://devops.com/white-paper-the-future-of-devsecops-in-a-fully-autonomous-ci-cd-pipeline/?utm_source=rss&utm_medium=rss&utm_campaign=white-paper-the-future-of-devsecops-in-a-fully-autonomous-ci-cd-pipeline)

### CI/CD Workflows, Testing, and Seamless Integrations

Workflow automation guides this week reflect ongoing trends toward secure, frictionless CI/CD pipelines. Articles cover Angular coverage enforcement in Azure DevOps and practical synchronization between Azure DevOps and Jira, supporting smoother testing, deployment, and coordination across development suites.

- [Enforcing Angular Unit Test Coverage in Azure DevOps Pipelines: A Step-by-Step Guide](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/enforcing-angular-unit-test-coverage-in-azure-devops-pipelines-a/ba-p/4446485)
- [Optimizing Azure DevOps and Jira Integration: 5 Real-World Use Cases for DevOps Teams](https://techcommunity.microsoft.com/t5/azure/optimizing-azure-devops-jira-integration-5-practical-use-cases/m-p/4445837#M22123)

### Observability, Debugging, and Production Reliability

Teams can further improve production operations with guidance on structured logging, metrics, and alerting. These resources are designed to help debug live systems and maintain high reliability, building on last week’s monitoring and incident response coverage.

- [Debugging in Production: Leveraging Logs, Metrics and Traces](https://devops.com/debugging-in-production-leveraging-logs-metrics-and-traces/?utm_source=rss&utm_medium=rss&utm_campaign=debugging-in-production-leveraging-logs-metrics-and-traces)

### The Expanding Ecosystem: AI-Powered Content, Fusion Development, and Cost Optimization

Ecosystem-wide updates include the introduction of tech.hub.ms, a platform for curated Microsoft technical content. Fusion development stories show increased adoption of blended business and engineering workflows; articles on FinOps as Code and SRE.ai explore automation and cost-conscious practices across SaaS and DevOps teams.

- [Announcing tech.hub.ms: Curated Microsoft Tech Content Platform](https://r-vm.com/new-website-tech-hub-ms.html)
- [Microsoft Morphs Fusion Developers To Full Stack Builders](https://devops.com/microsoft-morphs-fusion-developers-to-full-stack-builders/?utm_source=rss&utm_medium=rss&utm_campaign=microsoft-morphs-fusion-developers-to-full-stack-builders)
- [FinOps as Code – Unlocking Cloud Cost Optimization](https://devops.com/finops-as-code-unlocking-cloud-cost-optimization/?utm_source=rss&utm_medium=rss&utm_campaign=finops-as-code-unlocking-cloud-cost-optimization)
- [SRE.ai Aims to Streamline DevOps for SaaS with AI Automation](https://devops.com/sre-ai-looks-to-unify-devops-workflows-across-multiple-saas-applications/?utm_source=rss&utm_medium=rss&utm_campaign=sre-ai-looks-to-unify-devops-workflows-across-multiple-saas-applications)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-08-25';

-- weekly-devops-roundup-2025-09-01  (2025-09-01)
UPDATE content_items
SET  content      = 'This week in DevOps, teams focus on advanced automation powered by AI, improvements in open-source governance, updated platform features, and practical insights on reliability and workflow management.

<!--excerpt_end-->

## DevOps

### AI-Powered Automation and Autonomous Agents

Harness’s new AI DevOps platform automates pipeline creation, deployment, root-cause detection, and testing with natural language prompts and built-in privacy controls. System Initiative introduces autonomous agents that manage infrastructure via digital twins and natural language change proposals. These features build on recent progress in onboarding, permission management, and observability, emphasizing hands-on oversight by DevOps teams and confirming that AI is a complement rather than a replacement for engineers.

- [Harness AI-Powered DevOps Platform Launches to Automate Software Delivery](https://devops.com/harness-delivers-on-ai-promise-for-devops/?utm_source=rss&utm_medium=rss&utm_campaign=harness-delivers-on-ai-promise-for-devops)
- [System Initiative Introduces Autonomous AI Agents for Infrastructure Automation](https://devops.com/system-initiative-adds-ai-agents-to-infrastructure-automation-platform/?utm_source=rss&utm_medium=rss&utm_campaign=system-initiative-adds-ai-agents-to-infrastructure-automation-platform)
- [AI Agent Onboarding as a Core DevOps Responsibility](https://devops.com/ai-agent-onboarding-is-now-a-critical-devops-function/?utm_source=rss&utm_medium=rss&utm_campaign=ai-agent-onboarding-is-now-a-critical-devops-function)
- [Can AI Replace DevOps Engineers?](https://devops.com/can-ai-replace-devops-engineers-3/?utm_source=rss&utm_medium=rss&utm_campaign=can-ai-replace-devops-engineers-3)

### Architectural Governance, Patterns, and Compliance

Morgan Stanley’s open source CALM tools automate enterprise architecture governance with meta schemas, templates, and command-line utilities, which integrate CI/CD compliance checks. Broadcom’s VMware Cloud Foundation adds Argo CD, Ubuntu container support, and GPU/AI workload capabilities, simplifying orchestration and enterprise-grade compliance for cloud workloads.

- [Morgan Stanley Open Sources CALM: Architecture as Code for Enterprise DevOps](https://devops.com/morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops/?utm_source=rss&utm_medium=rss&utm_campaign=morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops)
- [Broadcom Expands VMware Cloud Foundation with Argo CD and Ubuntu Support](https://devops.com/broadcom-adds-argo-and-ubuntu-support-to-vmware-cloud-foundation/?utm_source=rss&utm_medium=rss&utm_campaign=broadcom-adds-argo-and-ubuntu-support-to-vmware-cloud-foundation)

### Developer Platform Updates and Workflow Automation

GitHub’s new Dependabot exclude-paths option provides finer control over automated pull request noise, plus improvements for template URLs and fine-grained Personal Access Token management. Walkthroughs support maintainers in scaling open source projects via models and Actions. Added repository management features (rulesets, dashboard, export options) and accessibility upgrades help teams simplify administration and improve accessibility.

- [Suppress Dependabot PRs in Specific Subdirectories with `exclude-paths`](https://github.blog/changelog/2025-08-26-dependabot-can-now-exclude-automatic-pull-requests-for-manifests-in-selected-subdirectories)
- [Template URLs for Fine-Grained PATs and Updated Permissions UI](https://github.blog/changelog/2025-08-26-template-urls-for-fine-grained-pats-and-updated-permissions-ui)
- [Automating Open Source Maintenance with GitHub Models and AI Workflows](https://github.blog/open-source/maintainers/how-github-models-can-help-open-source-maintainers-focus-on-what-matters/)
- [Improved GitHub Repository Creation, Ruleset, and Insights Features Released](https://github.blog/changelog/2025-08-26-improved-repository-creation-generally-available-plus-ruleset-insights-improvements)
- [Public Preview: Enhanced Home Dashboard with My Work and Feed Tabs](https://github.blog/changelog/2025-08-28-improvements-to-the-home-dashboard-available-in-public-preview)

### DevOps Tooling, Trends, and Team Practices

Growing use of modular automation frameworks such as GitHub Actions, Dagger, and Temporal enables developers to build efficient, event-driven workflows. Articles emphasize practices like improving team visibility, capacity management, and combining AI workflow automation with strong peer review and security. John Willis highlights the importance of building resilience and security into ongoing engineering work.

- [How Engineers are Automating More with Less: Trends in DevOps Tooling](https://devops.com/how-engineers-are-automating-more-with-less-trends-in-devops-tooling/?utm_source=rss&utm_medium=rss&utm_campaign=how-engineers-are-automating-more-with-less-trends-in-devops-tooling)
- [Bringing Order to Chaotic Software Engineering Workflows](https://devops.com/bringing-order-to-chaotic-software-engineering-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=bringing-order-to-chaotic-software-engineering-workflows)
- [John Willis: The True North of DevOps and DevSecOps](https://devops.com/john-willis-the-true-north-of-devops-and-devsecops/?utm_source=rss&utm_medium=rss&utm_campaign=john-willis-the-true-north-of-devops-and-devsecops)

### DevOps Platform Reliability and Security Incidents

A mid-year report finds a rise in service interruptions and outages for platforms including GitHub, Azure DevOps, GitLab, Bitbucket, and Jira, with Azure DevOps reporting 74 incidents and GitHub up by 58%. Ongoing security concerns on platforms such as GitLab and Jira show how CI/CD environments remain key targets and reinforce the importance of observability and backup strategies.

- [Surge in DevOps Platform Incidents: 2025 Mid-Year Analysis of GitHub, Azure DevOps, and Jira Disruptions](https://devops.com/devops-platforms-show-cracks-github-incidents-surge-58-azure-gitlab-and-jira-also-under-pressure/?utm_source=rss&utm_medium=rss&utm_campaign=devops-platforms-show-cracks-github-incidents-surge-58-azure-gitlab-and-jira-also-under-pressure)',
     excerpt      = 'This week in DevOps, teams focus on advanced automation powered by AI, improvements in open-source governance, updated platform features, and practical insights on reliability and workflow management.',
     content_hash = md5('This week in DevOps, teams focus on advanced automation powered by AI, improvements in open-source governance, updated platform features, and practical insights on reliability and workflow management.

<!--excerpt_end-->

## DevOps

### AI-Powered Automation and Autonomous Agents

Harness’s new AI DevOps platform automates pipeline creation, deployment, root-cause detection, and testing with natural language prompts and built-in privacy controls. System Initiative introduces autonomous agents that manage infrastructure via digital twins and natural language change proposals. These features build on recent progress in onboarding, permission management, and observability, emphasizing hands-on oversight by DevOps teams and confirming that AI is a complement rather than a replacement for engineers.

- [Harness AI-Powered DevOps Platform Launches to Automate Software Delivery](https://devops.com/harness-delivers-on-ai-promise-for-devops/?utm_source=rss&utm_medium=rss&utm_campaign=harness-delivers-on-ai-promise-for-devops)
- [System Initiative Introduces Autonomous AI Agents for Infrastructure Automation](https://devops.com/system-initiative-adds-ai-agents-to-infrastructure-automation-platform/?utm_source=rss&utm_medium=rss&utm_campaign=system-initiative-adds-ai-agents-to-infrastructure-automation-platform)
- [AI Agent Onboarding as a Core DevOps Responsibility](https://devops.com/ai-agent-onboarding-is-now-a-critical-devops-function/?utm_source=rss&utm_medium=rss&utm_campaign=ai-agent-onboarding-is-now-a-critical-devops-function)
- [Can AI Replace DevOps Engineers?](https://devops.com/can-ai-replace-devops-engineers-3/?utm_source=rss&utm_medium=rss&utm_campaign=can-ai-replace-devops-engineers-3)

### Architectural Governance, Patterns, and Compliance

Morgan Stanley’s open source CALM tools automate enterprise architecture governance with meta schemas, templates, and command-line utilities, which integrate CI/CD compliance checks. Broadcom’s VMware Cloud Foundation adds Argo CD, Ubuntu container support, and GPU/AI workload capabilities, simplifying orchestration and enterprise-grade compliance for cloud workloads.

- [Morgan Stanley Open Sources CALM: Architecture as Code for Enterprise DevOps](https://devops.com/morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops/?utm_source=rss&utm_medium=rss&utm_campaign=morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops)
- [Broadcom Expands VMware Cloud Foundation with Argo CD and Ubuntu Support](https://devops.com/broadcom-adds-argo-and-ubuntu-support-to-vmware-cloud-foundation/?utm_source=rss&utm_medium=rss&utm_campaign=broadcom-adds-argo-and-ubuntu-support-to-vmware-cloud-foundation)

### Developer Platform Updates and Workflow Automation

GitHub’s new Dependabot exclude-paths option provides finer control over automated pull request noise, plus improvements for template URLs and fine-grained Personal Access Token management. Walkthroughs support maintainers in scaling open source projects via models and Actions. Added repository management features (rulesets, dashboard, export options) and accessibility upgrades help teams simplify administration and improve accessibility.

- [Suppress Dependabot PRs in Specific Subdirectories with `exclude-paths`](https://github.blog/changelog/2025-08-26-dependabot-can-now-exclude-automatic-pull-requests-for-manifests-in-selected-subdirectories)
- [Template URLs for Fine-Grained PATs and Updated Permissions UI](https://github.blog/changelog/2025-08-26-template-urls-for-fine-grained-pats-and-updated-permissions-ui)
- [Automating Open Source Maintenance with GitHub Models and AI Workflows](https://github.blog/open-source/maintainers/how-github-models-can-help-open-source-maintainers-focus-on-what-matters/)
- [Improved GitHub Repository Creation, Ruleset, and Insights Features Released](https://github.blog/changelog/2025-08-26-improved-repository-creation-generally-available-plus-ruleset-insights-improvements)
- [Public Preview: Enhanced Home Dashboard with My Work and Feed Tabs](https://github.blog/changelog/2025-08-28-improvements-to-the-home-dashboard-available-in-public-preview)

### DevOps Tooling, Trends, and Team Practices

Growing use of modular automation frameworks such as GitHub Actions, Dagger, and Temporal enables developers to build efficient, event-driven workflows. Articles emphasize practices like improving team visibility, capacity management, and combining AI workflow automation with strong peer review and security. John Willis highlights the importance of building resilience and security into ongoing engineering work.

- [How Engineers are Automating More with Less: Trends in DevOps Tooling](https://devops.com/how-engineers-are-automating-more-with-less-trends-in-devops-tooling/?utm_source=rss&utm_medium=rss&utm_campaign=how-engineers-are-automating-more-with-less-trends-in-devops-tooling)
- [Bringing Order to Chaotic Software Engineering Workflows](https://devops.com/bringing-order-to-chaotic-software-engineering-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=bringing-order-to-chaotic-software-engineering-workflows)
- [John Willis: The True North of DevOps and DevSecOps](https://devops.com/john-willis-the-true-north-of-devops-and-devsecops/?utm_source=rss&utm_medium=rss&utm_campaign=john-willis-the-true-north-of-devops-and-devsecops)

### DevOps Platform Reliability and Security Incidents

A mid-year report finds a rise in service interruptions and outages for platforms including GitHub, Azure DevOps, GitLab, Bitbucket, and Jira, with Azure DevOps reporting 74 incidents and GitHub up by 58%. Ongoing security concerns on platforms such as GitLab and Jira show how CI/CD environments remain key targets and reinforce the importance of observability and backup strategies.

- [Surge in DevOps Platform Incidents: 2025 Mid-Year Analysis of GitHub, Azure DevOps, and Jira Disruptions](https://devops.com/devops-platforms-show-cracks-github-incidents-surge-58-azure-gitlab-and-jira-also-under-pressure/?utm_source=rss&utm_medium=rss&utm_campaign=devops-platforms-show-cracks-github-incidents-surge-58-azure-gitlab-and-jira-also-under-pressure)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-09-01';

-- weekly-devops-roundup-2025-09-08  (2025-09-08)
UPDATE content_items
SET  content      = 'This week’s DevOps news focuses on workflow automation, more actionable observability, and team collaboration in GitHub, Azure DevOps, and AI tooling. The theme is to help teams streamline productivity and adapt efficiently with smarter interfaces and continuous improvements in testing and code quality.

<!--excerpt_end-->

## DevOps

### GitHub Ecosystem Updates

GitHub’s recent releases continue the push for better workflow management. Two AI-powered GitHub Actions—AI Labeler and Moderator—extend automation by using the Models inference API to assist with issue classification and moderation. Maintainers can automate these steps directly in Actions workflows.

The GraphQL API adds new resource limits to streamline performance and reduce deep nesting in queries. Developers are encouraged to check and update their queries for efficiency.

Improved file navigation in GitHub’s web interface now includes editing files from search, clearer branch context, and onboarding improvements for new contributors. These changes support the goal of simplifying navigation and boosting workflow clarity.

GitHub Spark updates add enhanced sharing, smoother Codespaces integration, and an updated activity page—combining to provide a faster, more consistent collaboration experience.

- [New GitHub Actions for AI-Based Issue Labeling and Moderation](https://github.blog/changelog/2025-09-05-github-actions-ai-labeler-and-moderator-with-the-github-models-inference-api)
- [GitHub GraphQL API Resource Limits Introduced](https://github.blog/changelog/2025-09-01-graphql-api-resource-limits)
- [Improved File Navigation and Editing in the GitHub Web UI](https://github.blog/changelog/2025-09-04-improved-file-navigation-and-editing-in-the-web-ui)
- [New Organization Sharing and Local Development Improvements in GitHub Spark](https://github.blog/changelog/2025-09-05-new-spark-sharing-option-and-improved-local-dev-experience)
- [GitHub Dashboard Feed Page Updated for Better Performance and Consistency](https://github.blog/changelog/2025-09-04-the-dashboard-feed-page-gets-a-refreshed-faster-experience)

### AI and Observability in DevOps

AI is becoming more central in DevOps, expanding on recent automation and agent management coverage. The idea of "vibe coding" is reframed as using generative AI to assist with CI/CD, support coding standards, and manage technical debt.

Observability is also becoming more actionable. There is a shift toward reporting metrics and logs that tie directly to business outcomes and faster incident response—echoing earlier discussions on reliability and transparency.

- [Vibe Coding: Transforming DevOps and CI/CD Teams with AI-Powered Collaboration](https://devops.com/vibing-with-the-future-why-vibe-coding-is-the-next-big-wave-for-devops-and-ci-cd-teams/?utm_source=rss&utm_medium=rss&utm_campaign=vibing-with-the-future-why-vibe-coding-is-the-next-big-wave-for-devops-and-ci-cd-teams)
- [Making Observability Actionable: Turning Metrics, Logs, and Traces into Better Business Outcomes](https://devops.com/making-observability-actionable-turning-metrics-logs-and-traces-into-better-business-outcomes/?utm_source=rss&utm_medium=rss&utm_campaign=making-observability-actionable-turning-metrics-logs-and-traces-into-better-business-outcomes)

### Azure DevOps and Quality Management Enhancements

Azure Test Plans now features the new Test Run Hub (public preview), giving teams improved management of quality processes. This includes advanced analytics, filtering, API integrations for report automation, markdown-based commenting, and work item linking.

Combining manual and automated testing in one place, these updates help teams consistently improve software quality and respond more effectively—supporting broader DevOps and engineering goals.

- [Introducing the New Test Run Hub in Azure Test Plans](https://devblogs.microsoft.com/devops/new-test-run-hub/)',
     excerpt      = 'This week’s DevOps news focuses on workflow automation, more actionable observability, and team collaboration in GitHub, Azure DevOps, and AI tooling. The theme is to help teams streamline productivity and adapt efficiently with smarter interfaces and continuous improvements in testing and code quality.',
     content_hash = md5('This week’s DevOps news focuses on workflow automation, more actionable observability, and team collaboration in GitHub, Azure DevOps, and AI tooling. The theme is to help teams streamline productivity and adapt efficiently with smarter interfaces and continuous improvements in testing and code quality.

<!--excerpt_end-->

## DevOps

### GitHub Ecosystem Updates

GitHub’s recent releases continue the push for better workflow management. Two AI-powered GitHub Actions—AI Labeler and Moderator—extend automation by using the Models inference API to assist with issue classification and moderation. Maintainers can automate these steps directly in Actions workflows.

The GraphQL API adds new resource limits to streamline performance and reduce deep nesting in queries. Developers are encouraged to check and update their queries for efficiency.

Improved file navigation in GitHub’s web interface now includes editing files from search, clearer branch context, and onboarding improvements for new contributors. These changes support the goal of simplifying navigation and boosting workflow clarity.

GitHub Spark updates add enhanced sharing, smoother Codespaces integration, and an updated activity page—combining to provide a faster, more consistent collaboration experience.

- [New GitHub Actions for AI-Based Issue Labeling and Moderation](https://github.blog/changelog/2025-09-05-github-actions-ai-labeler-and-moderator-with-the-github-models-inference-api)
- [GitHub GraphQL API Resource Limits Introduced](https://github.blog/changelog/2025-09-01-graphql-api-resource-limits)
- [Improved File Navigation and Editing in the GitHub Web UI](https://github.blog/changelog/2025-09-04-improved-file-navigation-and-editing-in-the-web-ui)
- [New Organization Sharing and Local Development Improvements in GitHub Spark](https://github.blog/changelog/2025-09-05-new-spark-sharing-option-and-improved-local-dev-experience)
- [GitHub Dashboard Feed Page Updated for Better Performance and Consistency](https://github.blog/changelog/2025-09-04-the-dashboard-feed-page-gets-a-refreshed-faster-experience)

### AI and Observability in DevOps

AI is becoming more central in DevOps, expanding on recent automation and agent management coverage. The idea of "vibe coding" is reframed as using generative AI to assist with CI/CD, support coding standards, and manage technical debt.

Observability is also becoming more actionable. There is a shift toward reporting metrics and logs that tie directly to business outcomes and faster incident response—echoing earlier discussions on reliability and transparency.

- [Vibe Coding: Transforming DevOps and CI/CD Teams with AI-Powered Collaboration](https://devops.com/vibing-with-the-future-why-vibe-coding-is-the-next-big-wave-for-devops-and-ci-cd-teams/?utm_source=rss&utm_medium=rss&utm_campaign=vibing-with-the-future-why-vibe-coding-is-the-next-big-wave-for-devops-and-ci-cd-teams)
- [Making Observability Actionable: Turning Metrics, Logs, and Traces into Better Business Outcomes](https://devops.com/making-observability-actionable-turning-metrics-logs-and-traces-into-better-business-outcomes/?utm_source=rss&utm_medium=rss&utm_campaign=making-observability-actionable-turning-metrics-logs-and-traces-into-better-business-outcomes)

### Azure DevOps and Quality Management Enhancements

Azure Test Plans now features the new Test Run Hub (public preview), giving teams improved management of quality processes. This includes advanced analytics, filtering, API integrations for report automation, markdown-based commenting, and work item linking.

Combining manual and automated testing in one place, these updates help teams consistently improve software quality and respond more effectively—supporting broader DevOps and engineering goals.

- [Introducing the New Test Run Hub in Azure Test Plans](https://devblogs.microsoft.com/devops/new-test-run-hub/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-09-08';

-- weekly-devops-roundup-2025-09-15  (2025-09-15)
UPDATE content_items
SET  content      = 'DevOps coverage this week features updated automation, end-to-end traceability, improved collaboration, unified observability, and embedded AI throughout software delivery. Security and workflow resilience remain central, with infrastructure enhancements supporting secure, agentic automations.

<!--excerpt_end-->

## DevOps

### GitHub and JFrog: Secure, Traceable AI-Driven DevOps Pipelines

GitHub and JFrog expanded their integration for secure CI/CD traceability—developers can link commits to artifacts, automate SBOM policies, and use GitHub Actions for artifact management. Security combines GitHub Advanced Security and JFrog Xray; OIDC-based authentication removes secrets from CI, continuing secretless automation. JFrog previewed JFry, an agentic AI platform for artifact control and governance with semantic metadata.

Integration with GitHub Copilot, ServiceNow, and SonarQube extends efficient developer experiences, and new tools automate compliance and evidence management for audit-ready DevOps.

- [How to use the GitHub and JFrog integration for secure, traceable builds from commit to production](https://github.blog/enterprise-software/devsecops/how-to-use-the-github-and-jfrog-integration-for-secure-traceable-builds-from-commit-to-production/)
- [JFrog Unveils DevOps Platform for the Agentic AI Era](https://devops.com/jfrog-unveils-devops-platform-for-the-agentic-ai-era/?utm_source=rss&utm_medium=rss&utm_campaign=jfrog-unveils-devops-platform-for-the-agentic-ai-era)
- [JFrog SwampUP 2025 Highlights: AI-Driven DevOps, Governance, and Secure Software Supply Chains](https://devops.com/jfrog-continues-leaping-at-swampup/?utm_source=rss&utm_medium=rss&utm_campaign=jfrog-continues-leaping-at-swampup)
- [JFrog CEO: AI Agents Require Practices Beyond Security, Traceability](https://devops.com/jfrog-ceo-ai-agents-require-practices-beyond-security-traceability/?utm_source=rss&utm_medium=rss&utm_campaign=jfrog-ceo-ai-agents-require-practices-beyond-security-traceability)

### AI, Governance, and Observability in Modern DevOps

DevOps teams now integrate governance into every workflow phase to manage AI expansion and meet compliance needs. DevGovOps adds real-time verification and risk detection in CI/CD, continuing last week’s DevSecOps evolution.

Surveys show IT teams expect greater AI usage but need further automation and skills to ensure reliability, confirming last week’s findings about workflow challenges. Infrastructure as Code and platform engineering remain priorities. SwampUP 2025 panels focus on supply chain verification and team-wide compliance. Perforce’s Delphix update now uses AI to generate synthetic data for application testing, supporting better test coverage in secure settings.

- [DevGovOps: Embedding Governance into DevOps for the Age of AI](https://devops.com/devgovops-a-new-play-in-devops-or-is-it/?utm_source=rss&utm_medium=rss&utm_campaign=devgovops-a-new-play-in-devops-or-is-it)
- [Survey Reveals IT Teams Struggle to Scale AI Workloads Due to Automation Gaps](https://devops.com/survey-most-it-teams-not-prepared-to-manage-ai-workloads/?utm_source=rss&utm_medium=rss&utm_campaign=survey-most-it-teams-not-prepared-to-manage-ai-workloads)
- [Perforce Adds AI-Driven Synthetic Data Generation to Delphix Platform for DevOps Testing](https://devops.com/perforce-adds-small-language-model-to-create-synthetic-data-for-app-testing/?utm_source=rss&utm_medium=rss&utm_campaign=perforce-adds-small-language-model-to-create-synthetic-data-for-app-testing)
- [Bringing Trust and Governance to AI-Driven DevOps](https://devops.com/bringing-trust-and-governance-to-ai-driven-devops/?utm_source=rss&utm_medium=rss&utm_campaign=bringing-trust-and-governance-to-ai-driven-devops)
- [Survey Reveals Software Engineering Hurdles After AI Adoption](https://devops.com/survey-surfaces-software-engineering-challenges-following-adoption-of-ai/?utm_source=rss&utm_medium=rss&utm_campaign=survey-surfaces-software-engineering-challenges-following-adoption-of-ai)

### Cisco & Splunk: Unified Observability and AI Agent Automation

Cisco Splunk .Conf25 launched agentic AI and a data fabric for automated observability. Splunk now uses OpenTelemetry for streamlined agent integration in monitoring, supporting automated data collection, incident management, and remediation for distributed workloads.

Cisco Data Fabric offers analytics on machine data using AI/ML, supporting operational insights. Integrations provide APM, DEA, user and network monitoring, and Cisco AI Canvas delivers a single platform for DevOps and security teams. OpenTelemetry semantic extensions allow unified monitoring for legacy and AI services, improving automated incident response.

- [Cisco Integrates AI Agents and Data Fabric into Splunk Observability Platform](https://devops.com/cisco-to-add-ai-agents-to-splunk-observability-platforms/?utm_source=rss&utm_medium=rss&utm_campaign=cisco-to-add-ai-agents-to-splunk-observability-platforms)
- [OpenTelemetry Extensions Enhance Observability for AI Agents](https://devops.com/opentelemetry-extensions-to-enable-observability-of-ai-agents/?utm_source=rss&utm_medium=rss&utm_campaign=opentelemetry-extensions-to-enable-observability-of-ai-agents)

### GitHub Actions, Collaboration, and Repository Management Updates

GitHub has released several workflow improvements. The macOS 26 image for Actions is now in preview, streamlining infrastructure for iOS developers. An improved Projects REST API enables more refined management. Pull request file review is faster, with increased limits and better usability. Ruleset exemptions let trusted users or bots bypass code checks. Repository insights now have expanded accessibility and export options.

Multiple assignees for issues and pull requests are available in all repositories for improved teamwork. Verified Answers in GitHub Discussions provide formal confirmation, helping both human and AI-powered support.

- [Using the macOS 26 Image in GitHub Actions Workflows](https://github.blog/changelog/2025-09-11-actions-macos-26-image-now-in-public-preview)
- [REST API Enhancements for GitHub Projects and Sub-Issues Improvements](https://github.blog/changelog/2025-09-11-a-rest-api-for-github-projects-sub-issues-improvements-and-more)
- [GitHub Pull Request ‘Files Changed’ Public Preview Updates – Increased File Limits and Performance Improvements](https://github.blog/changelog/2025-09-11-pull-request-files-changed-public-preview-experience-september-11-updates)
- [GitHub Ruleset Exemptions and New Repository Insights Features](https://github.blog/changelog/2025-09-10-github-ruleset-exemptions-and-repository-insights-updates)
- [Multiple Assignees for Issues and Pull Requests Now Available in All GitHub Repositories](https://github.blog/changelog/2025-09-11-multiple-assignees-for-issues-and-pull-requests-now-available-in-all-repositories)
- [Verified Answers Launched in GitHub Discussions for Reliable Community Solutions](https://github.blog/changelog/2025-09-11-verified-answers-generally-available-in-github-discussions)

### Other DevOps News

Teams migrating from Azure DevOps to GitHub now have guidance covering planning, repository and pipeline migration, and hybrid deployment approaches for secure, agile development.

- [From Azure DevOps to GitHub: Migrate, Integrate, Accelerate](/from-azure-devops-to-github-migrate-integrate-accelerate)

A short GitHub video tutorial explains basic Git concepts for new developers, supporting best practices for onboarding and collaboration.

- [7 Essential Git Concepts Every Beginner Needs to Know](/devops/videos/7-essential-git-concepts-every-beginner-needs-to-know)',
     excerpt      = 'DevOps coverage this week features updated automation, end-to-end traceability, improved collaboration, unified observability, and embedded AI throughout software delivery. Security and workflow resilience remain central, with infrastructure enhancements supporting secure, agentic automations.',
     content_hash = md5('DevOps coverage this week features updated automation, end-to-end traceability, improved collaboration, unified observability, and embedded AI throughout software delivery. Security and workflow resilience remain central, with infrastructure enhancements supporting secure, agentic automations.

<!--excerpt_end-->

## DevOps

### GitHub and JFrog: Secure, Traceable AI-Driven DevOps Pipelines

GitHub and JFrog expanded their integration for secure CI/CD traceability—developers can link commits to artifacts, automate SBOM policies, and use GitHub Actions for artifact management. Security combines GitHub Advanced Security and JFrog Xray; OIDC-based authentication removes secrets from CI, continuing secretless automation. JFrog previewed JFry, an agentic AI platform for artifact control and governance with semantic metadata.

Integration with GitHub Copilot, ServiceNow, and SonarQube extends efficient developer experiences, and new tools automate compliance and evidence management for audit-ready DevOps.

- [How to use the GitHub and JFrog integration for secure, traceable builds from commit to production](https://github.blog/enterprise-software/devsecops/how-to-use-the-github-and-jfrog-integration-for-secure-traceable-builds-from-commit-to-production/)
- [JFrog Unveils DevOps Platform for the Agentic AI Era](https://devops.com/jfrog-unveils-devops-platform-for-the-agentic-ai-era/?utm_source=rss&utm_medium=rss&utm_campaign=jfrog-unveils-devops-platform-for-the-agentic-ai-era)
- [JFrog SwampUP 2025 Highlights: AI-Driven DevOps, Governance, and Secure Software Supply Chains](https://devops.com/jfrog-continues-leaping-at-swampup/?utm_source=rss&utm_medium=rss&utm_campaign=jfrog-continues-leaping-at-swampup)
- [JFrog CEO: AI Agents Require Practices Beyond Security, Traceability](https://devops.com/jfrog-ceo-ai-agents-require-practices-beyond-security-traceability/?utm_source=rss&utm_medium=rss&utm_campaign=jfrog-ceo-ai-agents-require-practices-beyond-security-traceability)

### AI, Governance, and Observability in Modern DevOps

DevOps teams now integrate governance into every workflow phase to manage AI expansion and meet compliance needs. DevGovOps adds real-time verification and risk detection in CI/CD, continuing last week’s DevSecOps evolution.

Surveys show IT teams expect greater AI usage but need further automation and skills to ensure reliability, confirming last week’s findings about workflow challenges. Infrastructure as Code and platform engineering remain priorities. SwampUP 2025 panels focus on supply chain verification and team-wide compliance. Perforce’s Delphix update now uses AI to generate synthetic data for application testing, supporting better test coverage in secure settings.

- [DevGovOps: Embedding Governance into DevOps for the Age of AI](https://devops.com/devgovops-a-new-play-in-devops-or-is-it/?utm_source=rss&utm_medium=rss&utm_campaign=devgovops-a-new-play-in-devops-or-is-it)
- [Survey Reveals IT Teams Struggle to Scale AI Workloads Due to Automation Gaps](https://devops.com/survey-most-it-teams-not-prepared-to-manage-ai-workloads/?utm_source=rss&utm_medium=rss&utm_campaign=survey-most-it-teams-not-prepared-to-manage-ai-workloads)
- [Perforce Adds AI-Driven Synthetic Data Generation to Delphix Platform for DevOps Testing](https://devops.com/perforce-adds-small-language-model-to-create-synthetic-data-for-app-testing/?utm_source=rss&utm_medium=rss&utm_campaign=perforce-adds-small-language-model-to-create-synthetic-data-for-app-testing)
- [Bringing Trust and Governance to AI-Driven DevOps](https://devops.com/bringing-trust-and-governance-to-ai-driven-devops/?utm_source=rss&utm_medium=rss&utm_campaign=bringing-trust-and-governance-to-ai-driven-devops)
- [Survey Reveals Software Engineering Hurdles After AI Adoption](https://devops.com/survey-surfaces-software-engineering-challenges-following-adoption-of-ai/?utm_source=rss&utm_medium=rss&utm_campaign=survey-surfaces-software-engineering-challenges-following-adoption-of-ai)

### Cisco & Splunk: Unified Observability and AI Agent Automation

Cisco Splunk .Conf25 launched agentic AI and a data fabric for automated observability. Splunk now uses OpenTelemetry for streamlined agent integration in monitoring, supporting automated data collection, incident management, and remediation for distributed workloads.

Cisco Data Fabric offers analytics on machine data using AI/ML, supporting operational insights. Integrations provide APM, DEA, user and network monitoring, and Cisco AI Canvas delivers a single platform for DevOps and security teams. OpenTelemetry semantic extensions allow unified monitoring for legacy and AI services, improving automated incident response.

- [Cisco Integrates AI Agents and Data Fabric into Splunk Observability Platform](https://devops.com/cisco-to-add-ai-agents-to-splunk-observability-platforms/?utm_source=rss&utm_medium=rss&utm_campaign=cisco-to-add-ai-agents-to-splunk-observability-platforms)
- [OpenTelemetry Extensions Enhance Observability for AI Agents](https://devops.com/opentelemetry-extensions-to-enable-observability-of-ai-agents/?utm_source=rss&utm_medium=rss&utm_campaign=opentelemetry-extensions-to-enable-observability-of-ai-agents)

### GitHub Actions, Collaboration, and Repository Management Updates

GitHub has released several workflow improvements. The macOS 26 image for Actions is now in preview, streamlining infrastructure for iOS developers. An improved Projects REST API enables more refined management. Pull request file review is faster, with increased limits and better usability. Ruleset exemptions let trusted users or bots bypass code checks. Repository insights now have expanded accessibility and export options.

Multiple assignees for issues and pull requests are available in all repositories for improved teamwork. Verified Answers in GitHub Discussions provide formal confirmation, helping both human and AI-powered support.

- [Using the macOS 26 Image in GitHub Actions Workflows](https://github.blog/changelog/2025-09-11-actions-macos-26-image-now-in-public-preview)
- [REST API Enhancements for GitHub Projects and Sub-Issues Improvements](https://github.blog/changelog/2025-09-11-a-rest-api-for-github-projects-sub-issues-improvements-and-more)
- [GitHub Pull Request ‘Files Changed’ Public Preview Updates – Increased File Limits and Performance Improvements](https://github.blog/changelog/2025-09-11-pull-request-files-changed-public-preview-experience-september-11-updates)
- [GitHub Ruleset Exemptions and New Repository Insights Features](https://github.blog/changelog/2025-09-10-github-ruleset-exemptions-and-repository-insights-updates)
- [Multiple Assignees for Issues and Pull Requests Now Available in All GitHub Repositories](https://github.blog/changelog/2025-09-11-multiple-assignees-for-issues-and-pull-requests-now-available-in-all-repositories)
- [Verified Answers Launched in GitHub Discussions for Reliable Community Solutions](https://github.blog/changelog/2025-09-11-verified-answers-generally-available-in-github-discussions)

### Other DevOps News

Teams migrating from Azure DevOps to GitHub now have guidance covering planning, repository and pipeline migration, and hybrid deployment approaches for secure, agile development.

- [From Azure DevOps to GitHub: Migrate, Integrate, Accelerate](/from-azure-devops-to-github-migrate-integrate-accelerate)

A short GitHub video tutorial explains basic Git concepts for new developers, supporting best practices for onboarding and collaboration.

- [7 Essential Git Concepts Every Beginner Needs to Know](/devops/videos/7-essential-git-concepts-every-beginner-needs-to-know)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-09-15';

-- weekly-devops-roundup-2025-09-22  (2025-09-22)
UPDATE content_items
SET  content      = 'Recent DevOps coverage highlights workflow automation, higher code quality, and improved security. Updates include GitHub Actions and Dependabot features, migration advice, advancements in AI-powered automation, modern workflow strategies, and enhanced platform security.

<!--excerpt_end-->

## DevOps

### GitHub Actions and Dependabot Enhancements

GitHub Actions now features YAML anchor support and can use workflow templates from private repositories, streamlining complex CI/CD automation and internal sharing. A new job context variable (`check_run_id`) helps target build artifacts and notifications, reducing setup complexity. Dependabot adds support for Conda `environment.yml` files so Python/science projects now automate vulnerability and version updates, enhancing supply chain protections.

These changes build on last week’s automation transparency and secure workflow improvements.

- [GitHub Actions Adds YAML Anchors and Workflow Templates from Non-Public Repositories](https://github.blog/changelog/2025-09-18-actions-yaml-anchors-and-non-public-workflow-templates)
- [Dependabot Adds Support for Conda Environment Files](https://github.blog/changelog/2025-09-16-conda-ecosystem-support-for-dependabot-now-generally-available)

### Platform Deprecations and Migration Guidance

GitHub Actions will phase out Node 20 for JavaScript actions, switching to Node 24 default by March 2026. Developers should start updating workflows and testing affected architectures now. The macOS 13 runner image retires on December 4, 2025, with interim service interruptions and guidance to migrate to ARM64 runners.

These transitions align with last week’s migration tips to ensure stable CI pipelines.

- [Deprecation of Node 20 on GitHub Actions Runners](https://github.blog/changelog/2025-09-19-deprecation-of-node-20-on-github-actions-runners)
- [Retirement of GitHub Actions macOS 13 Runner Image Announced](https://github.blog/changelog/2025-09-19-github-actions-macos-13-runner-image-is-closing-down)

### AI-Powered Automation: Code Review and Infrastructure

CodeRabbit now includes CLI options, auto-generated tests, custom merge checks, and MCP integration for scalable, AI-driven code reviews. Teams automate review outside IDEs, increase test coverage, and link feedback to documentation for safer handling of AI-generated code.

Pulumi Neo previews AI agents for infrastructure tasks, automating diagnostics, deployments, compliance, and audit reporting. MCP support enables broader pipeline integration with balanced manual oversight and automation.

These features extend last week’s agentic improvement in CI/CD traceability and workflow governance.

- [CodeRabbit Adds CLI and AI-Powered Enhancements to Code Review Platform](https://devops.com/coderabbit-adds-cli-support-to-code-review-platform-based-on-ai/)
- [CodeRabbit Expands AI Code Review Platform with CLI and MCP Support](https://devops.com/coderabbit-adds-cli-support-to-code-review-platform-based-on-ai/?utm_source=rss&utm_medium=rss&utm_campaign=coderabbit-adds-cli-support-to-code-review-platform-based-on-ai)
- [Pulumi Introduces AI Agents for Automated Infrastructure Management](https://devops.com/pulumi-previews-ai-agents-trained-to-automate-infrastructure-management/?utm_source=rss&utm_medium=rss&utm_campaign=pulumi-previews-ai-agents-trained-to-automate-infrastructure-management)

### Workflow Automation Trends: AI Agents and GitOps for Next-Gen DevOps

Guides provide actionable approaches for employing AI agents in Jira ticketing, code review, and deployment with LangChain, OpenAI, and event hooks. Recommendations highlight increased autonomy in ticket creation, PR handling, and deployments, as developers shift toward oversight.

A migration roadmap for enterprise GitOps details phases—asset review, repository restructuring, Argo CD/Flux automation, policy enforcement, and audit-ready service transitions. The focus remains on compliance and ongoing improvement.

These resources build on last week’s secure automation and agent-based pipeline content.

- [Automating Jira, PR Reviews, and Deployment with AI Agents](https://dellenny.com/supercharging-your-workflow-using-an-ai-agent-to-automate-jira-updates-pr-reviews-and-code-deployment/)
- [From Legacy to GitOps: A Roadmap for Enterprise Modernization](https://devops.com/from-legacy-to-gitops-a-roadmap-for-enterprise-modernization/)

### DevOps Security, Platform Resilience, and Industry Analysis

Recent analysis reviews platform outages and security incidents with GitHub, Jira, and Bitbucket, recommending improved redundancy, secrets management, observability, and disaster planning.

JFrog’s swampUP 2025 event introduces agent-aware artifact validation (AppTrust, JFrog Fly, MCP integration) for traceability and governance. Harness advocates AI-enabled CI/CD with automated remediation, contextual security, and developer portal access.

DevOps-as-a-Service solutions now target modular, AI-augmented management for organizations exploring new orchestration tools.

These topics extend last week’s unified observability, compliance, and platform automation content.

- [Outages and Security Threats in DevOps Tooling: Cracks in the Foundation](https://devops.com/outages-and-security-threats-in-devops-tooling-cracks-in-the-foundation/)
- [AI-Driven Security and Automation in Modern DevOps: Insights from JFrog swampUP 2025](https://devops.com/empowering-secure-agentic-software-delivery/)
- [Harness CEO Advocates AI-Driven Transformation of CI/CD Workflows](https://devops.com/harness-ceo-calls-for-reimagining-of-ci-cd-workflows-in-the-ai-era/)
- [Is the Future of DevOps DevOps-as-a-Service (DaaS)?](https://devops.com/is-the-future-of-devops-daas/)

### Other DevOps News

GitHub Enterprise now offers public preview of daily license history tracking, supporting compliance, billing, and audit processes, following last week’s repository management improvement coverage.

- [GitHub Enterprise License History Tracking Public Preview](https://github.blog/changelog/2025-09-15-github-enterprise-license-history-tracking-now-available-in-public-preview)

Advisory articles underscore that successful automation depends on early QA focus and combining scripts with human insight for specialized and regulated workflows.

- [Why Automation Fails Without the Right QA Mindset](https://devops.com/why-automation-fails-without-the-right-qa-mindset/)',
     excerpt      = 'Recent DevOps coverage highlights workflow automation, higher code quality, and improved security. Updates include GitHub Actions and Dependabot features, migration advice, advancements in AI-powered automation, modern workflow strategies, and enhanced platform security.',
     content_hash = md5('Recent DevOps coverage highlights workflow automation, higher code quality, and improved security. Updates include GitHub Actions and Dependabot features, migration advice, advancements in AI-powered automation, modern workflow strategies, and enhanced platform security.

<!--excerpt_end-->

## DevOps

### GitHub Actions and Dependabot Enhancements

GitHub Actions now features YAML anchor support and can use workflow templates from private repositories, streamlining complex CI/CD automation and internal sharing. A new job context variable (`check_run_id`) helps target build artifacts and notifications, reducing setup complexity. Dependabot adds support for Conda `environment.yml` files so Python/science projects now automate vulnerability and version updates, enhancing supply chain protections.

These changes build on last week’s automation transparency and secure workflow improvements.

- [GitHub Actions Adds YAML Anchors and Workflow Templates from Non-Public Repositories](https://github.blog/changelog/2025-09-18-actions-yaml-anchors-and-non-public-workflow-templates)
- [Dependabot Adds Support for Conda Environment Files](https://github.blog/changelog/2025-09-16-conda-ecosystem-support-for-dependabot-now-generally-available)

### Platform Deprecations and Migration Guidance

GitHub Actions will phase out Node 20 for JavaScript actions, switching to Node 24 default by March 2026. Developers should start updating workflows and testing affected architectures now. The macOS 13 runner image retires on December 4, 2025, with interim service interruptions and guidance to migrate to ARM64 runners.

These transitions align with last week’s migration tips to ensure stable CI pipelines.

- [Deprecation of Node 20 on GitHub Actions Runners](https://github.blog/changelog/2025-09-19-deprecation-of-node-20-on-github-actions-runners)
- [Retirement of GitHub Actions macOS 13 Runner Image Announced](https://github.blog/changelog/2025-09-19-github-actions-macos-13-runner-image-is-closing-down)

### AI-Powered Automation: Code Review and Infrastructure

CodeRabbit now includes CLI options, auto-generated tests, custom merge checks, and MCP integration for scalable, AI-driven code reviews. Teams automate review outside IDEs, increase test coverage, and link feedback to documentation for safer handling of AI-generated code.

Pulumi Neo previews AI agents for infrastructure tasks, automating diagnostics, deployments, compliance, and audit reporting. MCP support enables broader pipeline integration with balanced manual oversight and automation.

These features extend last week’s agentic improvement in CI/CD traceability and workflow governance.

- [CodeRabbit Adds CLI and AI-Powered Enhancements to Code Review Platform](https://devops.com/coderabbit-adds-cli-support-to-code-review-platform-based-on-ai/)
- [CodeRabbit Expands AI Code Review Platform with CLI and MCP Support](https://devops.com/coderabbit-adds-cli-support-to-code-review-platform-based-on-ai/?utm_source=rss&utm_medium=rss&utm_campaign=coderabbit-adds-cli-support-to-code-review-platform-based-on-ai)
- [Pulumi Introduces AI Agents for Automated Infrastructure Management](https://devops.com/pulumi-previews-ai-agents-trained-to-automate-infrastructure-management/?utm_source=rss&utm_medium=rss&utm_campaign=pulumi-previews-ai-agents-trained-to-automate-infrastructure-management)

### Workflow Automation Trends: AI Agents and GitOps for Next-Gen DevOps

Guides provide actionable approaches for employing AI agents in Jira ticketing, code review, and deployment with LangChain, OpenAI, and event hooks. Recommendations highlight increased autonomy in ticket creation, PR handling, and deployments, as developers shift toward oversight.

A migration roadmap for enterprise GitOps details phases—asset review, repository restructuring, Argo CD/Flux automation, policy enforcement, and audit-ready service transitions. The focus remains on compliance and ongoing improvement.

These resources build on last week’s secure automation and agent-based pipeline content.

- [Automating Jira, PR Reviews, and Deployment with AI Agents](https://dellenny.com/supercharging-your-workflow-using-an-ai-agent-to-automate-jira-updates-pr-reviews-and-code-deployment/)
- [From Legacy to GitOps: A Roadmap for Enterprise Modernization](https://devops.com/from-legacy-to-gitops-a-roadmap-for-enterprise-modernization/)

### DevOps Security, Platform Resilience, and Industry Analysis

Recent analysis reviews platform outages and security incidents with GitHub, Jira, and Bitbucket, recommending improved redundancy, secrets management, observability, and disaster planning.

JFrog’s swampUP 2025 event introduces agent-aware artifact validation (AppTrust, JFrog Fly, MCP integration) for traceability and governance. Harness advocates AI-enabled CI/CD with automated remediation, contextual security, and developer portal access.

DevOps-as-a-Service solutions now target modular, AI-augmented management for organizations exploring new orchestration tools.

These topics extend last week’s unified observability, compliance, and platform automation content.

- [Outages and Security Threats in DevOps Tooling: Cracks in the Foundation](https://devops.com/outages-and-security-threats-in-devops-tooling-cracks-in-the-foundation/)
- [AI-Driven Security and Automation in Modern DevOps: Insights from JFrog swampUP 2025](https://devops.com/empowering-secure-agentic-software-delivery/)
- [Harness CEO Advocates AI-Driven Transformation of CI/CD Workflows](https://devops.com/harness-ceo-calls-for-reimagining-of-ci-cd-workflows-in-the-ai-era/)
- [Is the Future of DevOps DevOps-as-a-Service (DaaS)?](https://devops.com/is-the-future-of-devops-daas/)

### Other DevOps News

GitHub Enterprise now offers public preview of daily license history tracking, supporting compliance, billing, and audit processes, following last week’s repository management improvement coverage.

- [GitHub Enterprise License History Tracking Public Preview](https://github.blog/changelog/2025-09-15-github-enterprise-license-history-tracking-now-available-in-public-preview)

Advisory articles underscore that successful automation depends on early QA focus and combining scripts with human insight for specialized and regulated workflows.

- [Why Automation Fails Without the Right QA Mindset](https://devops.com/why-automation-fails-without-the-right-qa-mindset/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-09-22';

-- weekly-devops-roundup-2025-09-29  (2025-09-29)
UPDATE content_items
SET  content      = 'DevOps this week highlights new automation features, updates to API lifecycles, and improved onboarding, with emphasis on collaboration and clear operational processes.

<!--excerpt_end-->

## DevOps

### GitHub Platform and API Lifecycle Updates

GitHub’s pull request ''Files changed'' page now supports comments on any changed line, improving code review flexibility for teams and supporting enhanced navigation and API/webhook integration. This update continues previous efforts to refine workflow transparency.

Dependabot alert pagination via offsets is being retired on the REST API—teams should transition to cursor-based pagination for easier handling of larger alert sets. Billing API endpoints now provide aggregate metered usage, streamlining integration and reporting. Enterprise Cloud accounts gain new organizational usage views for better cost management.

- [Enhanced GitHub Pull Request Files Changed Page: Comment Anywhere in Changed Files](https://github.blog/changelog/2025-09-25-pull-request-files-changed-public-preview-now-supports-commenting-on-unchanged-lines)
- [Upcoming Changes to GitHub Dependabot Alerts REST API Pagination](https://github.blog/changelog/2025-09-23-upcoming-changes-to-github-dependabot-alerts-rest-api-offset-based-pagination-parameters-page-first-and-last)
- [GitHub Retires Product-Specific Billing APIs for Actions, Packages, and Storage](https://github.blog/changelog/2025-09-26-product-specific-billing-apis-are-closing-down)
- [Product-specific Billing APIs for GitHub Actions and Packages Are Closing Down](https://github.blog/changelog/2025-09-25-product-specific-billing-apis-are-closing-down)
- [Visualizing GitHub Enterprise Cloud Metered Usage by Organization](https://github.blog/changelog/2025-09-22-visualize-metered-usage-by-organization-in-github-enterprise-cloud)

### AI and Automation in DevOps: Harness and HashiCorp Advances

Harness adds modules for autonomous DevOps tasks, including code maintenance, build troubleshooting, feature flag management, and policy enforcement, all powered by AI. Verification and rollback modules work with observability platforms to improve deployment reliability, and natural language YAML generation supports automated pipeline configuration.

HashiCorp brings agentic AI for infrastructure automation, compatible with Microsoft, AWS, and Red Hat Ansible environments. HCP Terraform Stacks reaches general availability, delivering dependent config management, and new search/action tools (in beta) improve resource management. Vault security updates offer automated cryptography and enhanced credential workflows.

- [Harness Adds New AI Modules to Automate DevOps Pipelines and Maintenance](https://devops.com/harness-extends-scope-and-reach-of-ai-platform-for-automating-devops-workflows/)
- [HashiCorp Introduces Agentic AI and Enhanced Automation for IT Infrastructure](https://devops.com/hashicorp-embraces-agentic-ai-to-streamline-management-of-it/)

### Testing and Developer Onboarding Tools

Playwright Testing now runs on all major browsers and languages, offering managed parallel sessions on Azure and close CI/CD integration. Guides cover advanced debugging, reporting, and DevOps pipeline integration to help teams scale automated testing.

GitHub’s beginner guide delivers video resources for repository management, pull requests, commands, licensing, and profile setup, providing a standardized approach to DevOps onboarding.

- [Getting Started with Microsoft Playwright Testing Features and How to Use It](https://dellenny.com/getting-started-with-microsoft-playwright-testing-features-and-how-to-use-it/)
- [The Ultimate Beginner''s Guide to GitHub in 2025](/devops/videos/the-ultimate-beginners-guide-to-github-in-2025)

### Other DevOps News

GitHub refreshes its DMCA takedown policy, Acceptable Use Policy, and moderation practices, clarifying boundaries around developer feedback, synthetic media, and content safety. Teams managing public and open-source projects should review these updates.

- [How GitHub Protects Developers from Copyright Enforcement Overreach](https://github.blog/news-insights/policy-news-and-insights/how-github-protects-developers-from-copyright-enforcement-overreach/)',
     excerpt      = 'DevOps this week highlights new automation features, updates to API lifecycles, and improved onboarding, with emphasis on collaboration and clear operational processes.',
     content_hash = md5('DevOps this week highlights new automation features, updates to API lifecycles, and improved onboarding, with emphasis on collaboration and clear operational processes.

<!--excerpt_end-->

## DevOps

### GitHub Platform and API Lifecycle Updates

GitHub’s pull request ''Files changed'' page now supports comments on any changed line, improving code review flexibility for teams and supporting enhanced navigation and API/webhook integration. This update continues previous efforts to refine workflow transparency.

Dependabot alert pagination via offsets is being retired on the REST API—teams should transition to cursor-based pagination for easier handling of larger alert sets. Billing API endpoints now provide aggregate metered usage, streamlining integration and reporting. Enterprise Cloud accounts gain new organizational usage views for better cost management.

- [Enhanced GitHub Pull Request Files Changed Page: Comment Anywhere in Changed Files](https://github.blog/changelog/2025-09-25-pull-request-files-changed-public-preview-now-supports-commenting-on-unchanged-lines)
- [Upcoming Changes to GitHub Dependabot Alerts REST API Pagination](https://github.blog/changelog/2025-09-23-upcoming-changes-to-github-dependabot-alerts-rest-api-offset-based-pagination-parameters-page-first-and-last)
- [GitHub Retires Product-Specific Billing APIs for Actions, Packages, and Storage](https://github.blog/changelog/2025-09-26-product-specific-billing-apis-are-closing-down)
- [Product-specific Billing APIs for GitHub Actions and Packages Are Closing Down](https://github.blog/changelog/2025-09-25-product-specific-billing-apis-are-closing-down)
- [Visualizing GitHub Enterprise Cloud Metered Usage by Organization](https://github.blog/changelog/2025-09-22-visualize-metered-usage-by-organization-in-github-enterprise-cloud)

### AI and Automation in DevOps: Harness and HashiCorp Advances

Harness adds modules for autonomous DevOps tasks, including code maintenance, build troubleshooting, feature flag management, and policy enforcement, all powered by AI. Verification and rollback modules work with observability platforms to improve deployment reliability, and natural language YAML generation supports automated pipeline configuration.

HashiCorp brings agentic AI for infrastructure automation, compatible with Microsoft, AWS, and Red Hat Ansible environments. HCP Terraform Stacks reaches general availability, delivering dependent config management, and new search/action tools (in beta) improve resource management. Vault security updates offer automated cryptography and enhanced credential workflows.

- [Harness Adds New AI Modules to Automate DevOps Pipelines and Maintenance](https://devops.com/harness-extends-scope-and-reach-of-ai-platform-for-automating-devops-workflows/)
- [HashiCorp Introduces Agentic AI and Enhanced Automation for IT Infrastructure](https://devops.com/hashicorp-embraces-agentic-ai-to-streamline-management-of-it/)

### Testing and Developer Onboarding Tools

Playwright Testing now runs on all major browsers and languages, offering managed parallel sessions on Azure and close CI/CD integration. Guides cover advanced debugging, reporting, and DevOps pipeline integration to help teams scale automated testing.

GitHub’s beginner guide delivers video resources for repository management, pull requests, commands, licensing, and profile setup, providing a standardized approach to DevOps onboarding.

- [Getting Started with Microsoft Playwright Testing Features and How to Use It](https://dellenny.com/getting-started-with-microsoft-playwright-testing-features-and-how-to-use-it/)
- [The Ultimate Beginner''s Guide to GitHub in 2025](/devops/videos/the-ultimate-beginners-guide-to-github-in-2025)

### Other DevOps News

GitHub refreshes its DMCA takedown policy, Acceptable Use Policy, and moderation practices, clarifying boundaries around developer feedback, synthetic media, and content safety. Teams managing public and open-source projects should review these updates.

- [How GitHub Protects Developers from Copyright Enforcement Overreach](https://github.blog/news-insights/policy-news-and-insights/how-github-protects-developers-from-copyright-enforcement-overreach/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-09-29';

-- weekly-devops-roundup-2025-10-06  (2025-10-06)
UPDATE content_items
SET  content      = 'DevOps news showcases updated automation tools, AI integration, and security-focused platform practices. Teams work toward secure CI/CD, efficient AI-powered infrastructure management, improved governance, and practical connections between code, infrastructure, and incident response—all geared for reliable delivery and balanced development speed.

<!--excerpt_end-->

## DevOps

### Secure NuGet Publishing and Trusted CI/CD Workflows

Andrew Lock’s step-by-step guide explains Secure NuGet Package Publishing using ephemeral credentials with GitHub Actions, replacing static API keys for direct nuget.org authentication. Developers specify workflow permissions and deploy the NuGet/login@v1 YAML action for flexible access, supporting secure collaboration and compliance. This builds on last week’s Trusted Publishing and security enhancements for CI/CD automation.

- [Secure NuGet Package Publishing from GitHub Actions with Trusted Publishing](https://andrewlock.net/easily-publishing-nuget-packages-from-github-actions-with-trusted-publishing/)

### AI-driven DevOps Automation and Infrastructure Management

ControlMonkey introduces KoMo AI agents for automated Terraform provisioning, policy enforcement, and code analysis. These trace dependencies, review modules, and detect misconfigurations. env zero’s Cloud Governance Platform features a Static Code Analyzer Agent for policy-compliant fixes via pull requests, using Anthropic’s MCP to orchestrate multi-cloud resources. These platforms streamline manual work and standardize policy automation, continuing last week’s expansion of MCP-driven DevOps.

- [ControlMonkey Introduces AI Agents for Infrastructure Automation](https://devops.com/controlmonkey-adds-ai-agents-to-infrastructure-automation-platform/)
- [env zero Revamps Infrastructure Automation Platform for AI Era](https://devops.com/env-zero-revamps-infrastructure-automation-platform-for-ai-era/)

### Azure SRE Agent Expands: Automation, Diagnostics, and Incident Response

Azure SRE Agent v2.0 moves into wider public preview, adding role-based access control, approval workflows, and secure automation for Azure resources. Diagnostics cover both core and specialized workloads, with incident response linking Monitor, PagerDuty, ServiceNow, GitHub, and Azure DevOps. SREs benefit from unified orchestration and Copilot-powered PR automation, transparent billing via Agent Units, and improved documentation—reinforcing recent agent-driven modernization efforts.

- [Azure SRE Agent Public Preview Expands: New Features for DevOps and Automation](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/expanding-the-public-preview-of-the-azure-sre-agent/ba/p/4458514)

### AI Adoption in DevOps: Trust, Velocity, and Pipeline Bottlenecks

Industry analysis shares that while AI use is nearly universal, mistrust and pipeline instability persist. Internal platforms report stronger results and faster delivery (63%) with AI, but incidents and deployment errors remain frequent. Less than 10% of teams have fully automated pipelines; cost and security drive modernization. This data continues last week’s discussion on balancing performance and stability, illustrating current strengths and challenges.

- [DORA 2025: AI’s Impact on DevOps — Speed, Trust, and the Platform Effect](https://devops.com/dora-2025-faster-but-are-we-any-better/)
- [Survey Reveals DevOps Bottlenecks Created by AI-Generated Code](https://devops.com/survey-surfaces-downstream-devops-challenges-created-by-ai-code/)

### AI-driven Project Management for DevOps Teams

Shortcut’s Korey AI agent automates project management by turning natural language into actionable plans, tracking dependencies, and orchestrating work based on code comments, GitHub changes, and team workflows. Direct communication helps organize sprints and monitor blockers, reducing manual work and freeing engineers for coding. Korey expands AI’s project management role, building on last week’s coverage of Copilot Coding Agent and orchestration tools.

- [Shortcut Introduces Korey AI Agent for Software Project Management](https://devops.com/shortcut-adds-ai-agent-to-orchestrate-management-of-software-development-projects/)

### AI in the DevOps Lifecycle and Quality Assurance Mindset

DevOps guides recommend integrating AI-powered monitoring, testing, and pipeline automation to improve event correlation and vulnerability management—while highlighting the importance of data quality and integration planning. Teams are urged to prioritize automation for the highest-impact tasks. Another feature underlines that combining automation with exploratory QA creates stronger pipelines—engage QA early and build resilient scripts for meaningful business value. These articles reinforce recent best practices for maintaining robust, collaborative delivery pipelines with AI integration.

- [How AI is Transforming DevOps: Smarter, Faster, and More Reliable Software Delivery](https://dellenny.com/how-ai-is-transforming-devops-smarter-faster-and-more-reliable-software-delivery/)
- [Why Automation Fails Without the Right QA Mindset](https://devops.com/why-automation-fails-without-the-right-qa-mindset-2/)

### Platform, Tooling, and Workflow Updates

VS Live! Orlando 2025 previews hands-on training in Visual Studio, .NET, DevOps, AI, ML, cloud engineering, and security. Attendees will learn about tools ranging from AI debugging and Copilot-driven DevOps to .NET Aspire microservices, continuing previous attention to productivity and agentic workflows.

- [VS Live! Orlando 2025: Deep Dive into Visual Studio, AI, DevOps, and Beyond](https://devblogs.microsoft.com/visualstudio/visual-studio-live-orlando-2025/)

### Other DevOps News

GitHub Actions cache eviction enforcement is postponed, giving teams more time for workflow optimization and aligning with earlier roundups about lifecycle management.

- [New Enforcement Date for GitHub Actions Cache Eviction Policy](https://github.blog/changelog/2025-09-29-new-date-for-enforcement-of-cache-eviction-policy)

GitHub’s web interface adds interactive, one-click merge conflict resolution, streamlining collaboration and context management—building on recent onboarding and workflow enhancements.

- [One-Click Merge Conflict Resolution in GitHub Web Interface](https://github.blog/changelog/2025-10-02-one-click-merge-conflict-resolution-now-in-the-web-interface)

MapYourGrid continues to invite users to contribute to an open energy infrastructure map using GitHub, offering opportunities for climate action through technical collaboration.

- [MapYourGrid: Contributing to an Open Map of the World''s Energy Grid](/devops/videos/mapyourgrid-contributing-to-an-open-map-of-the-worlds-energy-grid)',
     excerpt      = 'DevOps news showcases updated automation tools, AI integration, and security-focused platform practices. Teams work toward secure CI/CD, efficient AI-powered infrastructure management, improved governance, and practical connections between code, infrastructure, and incident response—all geared for reliable delivery and balanced development speed.',
     content_hash = md5('DevOps news showcases updated automation tools, AI integration, and security-focused platform practices. Teams work toward secure CI/CD, efficient AI-powered infrastructure management, improved governance, and practical connections between code, infrastructure, and incident response—all geared for reliable delivery and balanced development speed.

<!--excerpt_end-->

## DevOps

### Secure NuGet Publishing and Trusted CI/CD Workflows

Andrew Lock’s step-by-step guide explains Secure NuGet Package Publishing using ephemeral credentials with GitHub Actions, replacing static API keys for direct nuget.org authentication. Developers specify workflow permissions and deploy the NuGet/login@v1 YAML action for flexible access, supporting secure collaboration and compliance. This builds on last week’s Trusted Publishing and security enhancements for CI/CD automation.

- [Secure NuGet Package Publishing from GitHub Actions with Trusted Publishing](https://andrewlock.net/easily-publishing-nuget-packages-from-github-actions-with-trusted-publishing/)

### AI-driven DevOps Automation and Infrastructure Management

ControlMonkey introduces KoMo AI agents for automated Terraform provisioning, policy enforcement, and code analysis. These trace dependencies, review modules, and detect misconfigurations. env zero’s Cloud Governance Platform features a Static Code Analyzer Agent for policy-compliant fixes via pull requests, using Anthropic’s MCP to orchestrate multi-cloud resources. These platforms streamline manual work and standardize policy automation, continuing last week’s expansion of MCP-driven DevOps.

- [ControlMonkey Introduces AI Agents for Infrastructure Automation](https://devops.com/controlmonkey-adds-ai-agents-to-infrastructure-automation-platform/)
- [env zero Revamps Infrastructure Automation Platform for AI Era](https://devops.com/env-zero-revamps-infrastructure-automation-platform-for-ai-era/)

### Azure SRE Agent Expands: Automation, Diagnostics, and Incident Response

Azure SRE Agent v2.0 moves into wider public preview, adding role-based access control, approval workflows, and secure automation for Azure resources. Diagnostics cover both core and specialized workloads, with incident response linking Monitor, PagerDuty, ServiceNow, GitHub, and Azure DevOps. SREs benefit from unified orchestration and Copilot-powered PR automation, transparent billing via Agent Units, and improved documentation—reinforcing recent agent-driven modernization efforts.

- [Azure SRE Agent Public Preview Expands: New Features for DevOps and Automation](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/expanding-the-public-preview-of-the-azure-sre-agent/ba/p/4458514)

### AI Adoption in DevOps: Trust, Velocity, and Pipeline Bottlenecks

Industry analysis shares that while AI use is nearly universal, mistrust and pipeline instability persist. Internal platforms report stronger results and faster delivery (63%) with AI, but incidents and deployment errors remain frequent. Less than 10% of teams have fully automated pipelines; cost and security drive modernization. This data continues last week’s discussion on balancing performance and stability, illustrating current strengths and challenges.

- [DORA 2025: AI’s Impact on DevOps — Speed, Trust, and the Platform Effect](https://devops.com/dora-2025-faster-but-are-we-any-better/)
- [Survey Reveals DevOps Bottlenecks Created by AI-Generated Code](https://devops.com/survey-surfaces-downstream-devops-challenges-created-by-ai-code/)

### AI-driven Project Management for DevOps Teams

Shortcut’s Korey AI agent automates project management by turning natural language into actionable plans, tracking dependencies, and orchestrating work based on code comments, GitHub changes, and team workflows. Direct communication helps organize sprints and monitor blockers, reducing manual work and freeing engineers for coding. Korey expands AI’s project management role, building on last week’s coverage of Copilot Coding Agent and orchestration tools.

- [Shortcut Introduces Korey AI Agent for Software Project Management](https://devops.com/shortcut-adds-ai-agent-to-orchestrate-management-of-software-development-projects/)

### AI in the DevOps Lifecycle and Quality Assurance Mindset

DevOps guides recommend integrating AI-powered monitoring, testing, and pipeline automation to improve event correlation and vulnerability management—while highlighting the importance of data quality and integration planning. Teams are urged to prioritize automation for the highest-impact tasks. Another feature underlines that combining automation with exploratory QA creates stronger pipelines—engage QA early and build resilient scripts for meaningful business value. These articles reinforce recent best practices for maintaining robust, collaborative delivery pipelines with AI integration.

- [How AI is Transforming DevOps: Smarter, Faster, and More Reliable Software Delivery](https://dellenny.com/how-ai-is-transforming-devops-smarter-faster-and-more-reliable-software-delivery/)
- [Why Automation Fails Without the Right QA Mindset](https://devops.com/why-automation-fails-without-the-right-qa-mindset-2/)

### Platform, Tooling, and Workflow Updates

VS Live! Orlando 2025 previews hands-on training in Visual Studio, .NET, DevOps, AI, ML, cloud engineering, and security. Attendees will learn about tools ranging from AI debugging and Copilot-driven DevOps to .NET Aspire microservices, continuing previous attention to productivity and agentic workflows.

- [VS Live! Orlando 2025: Deep Dive into Visual Studio, AI, DevOps, and Beyond](https://devblogs.microsoft.com/visualstudio/visual-studio-live-orlando-2025/)

### Other DevOps News

GitHub Actions cache eviction enforcement is postponed, giving teams more time for workflow optimization and aligning with earlier roundups about lifecycle management.

- [New Enforcement Date for GitHub Actions Cache Eviction Policy](https://github.blog/changelog/2025-09-29-new-date-for-enforcement-of-cache-eviction-policy)

GitHub’s web interface adds interactive, one-click merge conflict resolution, streamlining collaboration and context management—building on recent onboarding and workflow enhancements.

- [One-Click Merge Conflict Resolution in GitHub Web Interface](https://github.blog/changelog/2025-10-02-one-click-merge-conflict-resolution-now-in-the-web-interface)

MapYourGrid continues to invite users to contribute to an open energy infrastructure map using GitHub, offering opportunities for climate action through technical collaboration.

- [MapYourGrid: Contributing to an Open Map of the World''s Energy Grid](/devops/videos/mapyourgrid-contributing-to-an-open-map-of-the-worlds-energy-grid)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-10-06';

-- weekly-devops-roundup-2025-10-13  (2025-10-13)
UPDATE content_items
SET  content      = 'DevOps updates featured expanded automation capabilities, new lifecycle management, observability tools, and strategy resources. Platform changes, workflow migration guides, and studies on technical debt and supply chain improvements focus on reliability, developer productivity, and streamlined processes.

<!--excerpt_end-->

## DevOps

### Azure DevOps Server: Modern Lifecycle and Feature Parity

Azure DevOps Server’s release candidate closes the gap with the cloud service by adding TFX validation, new REST APIs for test recovery, and a supported upgrade path from TFS 2015+. Microsoft transitions to a Modern Lifecycle Policy—favoring ongoing updates over scheduled releases—and updates branding for easier support. Hybrid and on-premises teams receive improved upgrade, support, and feature access.

- [Azure DevOps Server RC Release: New Features, Lifecycle Policy, and Branding Updates](https://devblogs.microsoft.com/devops/announcing-the-new-azure-devops-server-rc-release/)

### GitHub, Dependabot, and Pull Request Workflow Migration

GitHub’s Dependabot will deprecate pull request comment commands (such as `@dependabot merge/close`) in Nov 2025/Jan 2026, requiring migration to web UI, CLI, and REST APIs. Automatic warnings and step-by-step migration guides help teams update CI/CD scripts, prioritizing security and reliability. These changes continue last week’s workflow migration agenda aimed at standardizing automation features.

- [Dependabot Pull Request Comment Command Deprecations and Migration to GitHub Native Features](https://github.blog/changelog/2025-10-06-upcoming-changes-to-github-dependabot-pull-request-comment-commands)
- [Deprecation of Dependabot Pull Request Comment Commands in Favor of GitHub Native Features](https://github.blog/changelog/2025-10-07-upcoming-changes-to-github-dependabot-pull-request-comment-commands)

### Fabric Data Agent: Integrated CI/CD and Git-Driven Workflows

Fabric’s data agents now offer integrated CI/CD, ALM, and Git for Lakehouse, Warehouse, Power BI Models, and KQL databases. Version control, rollback, staging, and reviews are managed in standard repos, making changes traceable and release processes structured for modern DevOps. Onboarding links guide teams through new source-controlled flows.

- [Fabric Data Agent: CI/CD, ALM Flow, and Git Integration Enhancements](https://blog.fabric.microsoft.com/en-US/blog/fabric-data-agent-now-supports-ci-cd-alm-flow-and-git-integration/)

### Git Merge 2025: Community, Innovation, and Future Roadmaps

Git Merge 2025 celebrated Git’s 20th anniversary at GitHub HQ, where contributors discussed branches, SHA-256 compatibility, visualization tools, and roadmap features. Community management and optimization remain key, impacting daily developer activities.

- [Git Merge 2025: Celebrating 20 Years of Git at GitHub HQ](https://github.blog/open-source/git/20-years-of-git-2-days-at-github-hq-git-merge-2025-highlights-%f0%9f%8e%89/)

### Infrastructure as Code with Pulumi and .NET

A new On .NET Live session explains how Pulumi brings .NET languages (C#/F#) to infrastructure automation across Azure, AWS, and Google Cloud. The episode covers configuration, CLI integration, and onboarding, providing a simple introduction to code-based infrastructure management.

- [Infrastructure as Code with Pulumi Using .NET Languages](/coding/videos/infrastructure-as-code-with-pulumi-using-net-languages)

### AI, Observability, and Supply Chain Intelligence in DevOps

Recent articles examine how AI and graph intelligence improve DevOps visibility and supply chain mapping. ‘DevGraphIntelOps’ was featured at swampUP 2025, highlighting graph analytics for dependency tracking and risk management. AI-driven pipelines support error detection, monitoring, and reliability. New Relic’s survey shows better observability reduces outages and incidents; expanded supply chain intelligence and smart CI/CD promote effective troubleshooting and automation.

These trends build on last week’s move toward agent-powered observability.

- [How Graph Intelligence Is Transforming Software Supply Chain Visibility](https://devops.com/how-graph-intelligence-is-transforming-software-supply-chain-visibility/)
- [Full-Stack Observability and AI: Mitigating IT Outage Costs](https://devops.com/report-full-stack-observability-cuts-downtime-costs/)
- [How AI Enhances DevOps Pipelines for Smarter Automation](https://devops.com/how-can-the-usage-of-ai-help-boost-devops-pipelines/)

### AI-Powered Automation Platforms: PagerDuty and Spacelift

PagerDuty’s new AI agents automate incident response, runbooks, transcription, scheduling, and integrate with external data protocols (MCP servers). Spacelift introduced Spacelift Intent, an open source AI tool using Anthropic’s MCP, which translates requests into API calls for infrastructure setups. These platforms simplify recurring tasks and speed up engineering, adding to current agent automation efforts in DevOps.

- [PagerDuty Introduces Suite of AI Agents to Enhance IT Management and DevOps Workflows](https://devops.com/pagerduty-adds-suite-of-ai-agents-to-it-management-platform/)
- [Spacelift Unveils Open Source AI Vibecoding Tool for Infrastructure Provisioning](https://devops.com/spacelift-adds-open-source-ai-vibecoding-tool-to-provision-infrastructure/)

### DevOps Culture: Internal Developer Platforms, Security, and AIOps

Analysis explores the advantages of Internal Developer Platforms (IDPs), AIOps, and DevSecOps for efficiency and autonomy. IDPs automate routine work, freeing developers for project goals. DevSecOps and AIOps require new skills and shift roles. GitOps, plug-ins, and unified tools continue to support speed and transparency. MLOps fosters collaboration between operations and data teams, guiding lifecycle and governance efforts.

- [How IDPs, AI, and Security Are Evolving DevOps Culture](https://devops.com/whose-ops-is-it-anyway-how-idps-ai-and-security-are-evolving-developer-culture/)

### Other DevOps News

A guide explains reducing repetitive DevOps work with Bash, Python, or PowerShell scripts—starting small and growing best practices. Advice on technical debt covers ways to improve developer satisfaction through roles like DX champion or using AI agents. A Chainguard survey highlights the challenges of tool sprawl and switching, underscoring the need for unified workflows and tools for developer well-being.

- [How to Eliminate DevOps Toil Using Automation Scripts](https://devops.com/how-to-eliminate-devops-toil-using-automation-scripts/)
- [Technical Debt: Make Developers Happier Now or Pay More Later](https://devops.com/technical-debt-make-developers-happier-now-or-pay-more-later/)
- [Survey Highlights Challenges and Opportunities in Software Engineering and DevOps](https://devops.com/survey-surfaces-lots-of-room-for-software-engineering-improvement/)',
     excerpt      = 'DevOps updates featured expanded automation capabilities, new lifecycle management, observability tools, and strategy resources. Platform changes, workflow migration guides, and studies on technical debt and supply chain improvements focus on reliability, developer productivity, and streamlined processes.',
     content_hash = md5('DevOps updates featured expanded automation capabilities, new lifecycle management, observability tools, and strategy resources. Platform changes, workflow migration guides, and studies on technical debt and supply chain improvements focus on reliability, developer productivity, and streamlined processes.

<!--excerpt_end-->

## DevOps

### Azure DevOps Server: Modern Lifecycle and Feature Parity

Azure DevOps Server’s release candidate closes the gap with the cloud service by adding TFX validation, new REST APIs for test recovery, and a supported upgrade path from TFS 2015+. Microsoft transitions to a Modern Lifecycle Policy—favoring ongoing updates over scheduled releases—and updates branding for easier support. Hybrid and on-premises teams receive improved upgrade, support, and feature access.

- [Azure DevOps Server RC Release: New Features, Lifecycle Policy, and Branding Updates](https://devblogs.microsoft.com/devops/announcing-the-new-azure-devops-server-rc-release/)

### GitHub, Dependabot, and Pull Request Workflow Migration

GitHub’s Dependabot will deprecate pull request comment commands (such as `@dependabot merge/close`) in Nov 2025/Jan 2026, requiring migration to web UI, CLI, and REST APIs. Automatic warnings and step-by-step migration guides help teams update CI/CD scripts, prioritizing security and reliability. These changes continue last week’s workflow migration agenda aimed at standardizing automation features.

- [Dependabot Pull Request Comment Command Deprecations and Migration to GitHub Native Features](https://github.blog/changelog/2025-10-06-upcoming-changes-to-github-dependabot-pull-request-comment-commands)
- [Deprecation of Dependabot Pull Request Comment Commands in Favor of GitHub Native Features](https://github.blog/changelog/2025-10-07-upcoming-changes-to-github-dependabot-pull-request-comment-commands)

### Fabric Data Agent: Integrated CI/CD and Git-Driven Workflows

Fabric’s data agents now offer integrated CI/CD, ALM, and Git for Lakehouse, Warehouse, Power BI Models, and KQL databases. Version control, rollback, staging, and reviews are managed in standard repos, making changes traceable and release processes structured for modern DevOps. Onboarding links guide teams through new source-controlled flows.

- [Fabric Data Agent: CI/CD, ALM Flow, and Git Integration Enhancements](https://blog.fabric.microsoft.com/en-US/blog/fabric-data-agent-now-supports-ci-cd-alm-flow-and-git-integration/)

### Git Merge 2025: Community, Innovation, and Future Roadmaps

Git Merge 2025 celebrated Git’s 20th anniversary at GitHub HQ, where contributors discussed branches, SHA-256 compatibility, visualization tools, and roadmap features. Community management and optimization remain key, impacting daily developer activities.

- [Git Merge 2025: Celebrating 20 Years of Git at GitHub HQ](https://github.blog/open-source/git/20-years-of-git-2-days-at-github-hq-git-merge-2025-highlights-%f0%9f%8e%89/)

### Infrastructure as Code with Pulumi and .NET

A new On .NET Live session explains how Pulumi brings .NET languages (C#/F#) to infrastructure automation across Azure, AWS, and Google Cloud. The episode covers configuration, CLI integration, and onboarding, providing a simple introduction to code-based infrastructure management.

- [Infrastructure as Code with Pulumi Using .NET Languages](/coding/videos/infrastructure-as-code-with-pulumi-using-net-languages)

### AI, Observability, and Supply Chain Intelligence in DevOps

Recent articles examine how AI and graph intelligence improve DevOps visibility and supply chain mapping. ‘DevGraphIntelOps’ was featured at swampUP 2025, highlighting graph analytics for dependency tracking and risk management. AI-driven pipelines support error detection, monitoring, and reliability. New Relic’s survey shows better observability reduces outages and incidents; expanded supply chain intelligence and smart CI/CD promote effective troubleshooting and automation.

These trends build on last week’s move toward agent-powered observability.

- [How Graph Intelligence Is Transforming Software Supply Chain Visibility](https://devops.com/how-graph-intelligence-is-transforming-software-supply-chain-visibility/)
- [Full-Stack Observability and AI: Mitigating IT Outage Costs](https://devops.com/report-full-stack-observability-cuts-downtime-costs/)
- [How AI Enhances DevOps Pipelines for Smarter Automation](https://devops.com/how-can-the-usage-of-ai-help-boost-devops-pipelines/)

### AI-Powered Automation Platforms: PagerDuty and Spacelift

PagerDuty’s new AI agents automate incident response, runbooks, transcription, scheduling, and integrate with external data protocols (MCP servers). Spacelift introduced Spacelift Intent, an open source AI tool using Anthropic’s MCP, which translates requests into API calls for infrastructure setups. These platforms simplify recurring tasks and speed up engineering, adding to current agent automation efforts in DevOps.

- [PagerDuty Introduces Suite of AI Agents to Enhance IT Management and DevOps Workflows](https://devops.com/pagerduty-adds-suite-of-ai-agents-to-it-management-platform/)
- [Spacelift Unveils Open Source AI Vibecoding Tool for Infrastructure Provisioning](https://devops.com/spacelift-adds-open-source-ai-vibecoding-tool-to-provision-infrastructure/)

### DevOps Culture: Internal Developer Platforms, Security, and AIOps

Analysis explores the advantages of Internal Developer Platforms (IDPs), AIOps, and DevSecOps for efficiency and autonomy. IDPs automate routine work, freeing developers for project goals. DevSecOps and AIOps require new skills and shift roles. GitOps, plug-ins, and unified tools continue to support speed and transparency. MLOps fosters collaboration between operations and data teams, guiding lifecycle and governance efforts.

- [How IDPs, AI, and Security Are Evolving DevOps Culture](https://devops.com/whose-ops-is-it-anyway-how-idps-ai-and-security-are-evolving-developer-culture/)

### Other DevOps News

A guide explains reducing repetitive DevOps work with Bash, Python, or PowerShell scripts—starting small and growing best practices. Advice on technical debt covers ways to improve developer satisfaction through roles like DX champion or using AI agents. A Chainguard survey highlights the challenges of tool sprawl and switching, underscoring the need for unified workflows and tools for developer well-being.

- [How to Eliminate DevOps Toil Using Automation Scripts](https://devops.com/how-to-eliminate-devops-toil-using-automation-scripts/)
- [Technical Debt: Make Developers Happier Now or Pay More Later](https://devops.com/technical-debt-make-developers-happier-now-or-pay-more-later/)
- [Survey Highlights Challenges and Opportunities in Software Engineering and DevOps](https://devops.com/survey-surfaces-lots-of-room-for-software-engineering-improvement/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-10-13';

-- weekly-devops-roundup-2025-10-20  (2025-10-20)
UPDATE content_items
SET  content      = 'DevOps news includes improvements to secure automation, tool unification, and observability. Major updates were released for Azure DevOps, GitHub Enterprise Server, and other open-source components. Industry insights explore the ongoing challenges of complexity and security, with a focus on the intersection of automation and human review.

<!--excerpt_end-->

## DevOps

### Azure DevOps, GitHub Enterprise Server, and Self-Hosted Runner Innovations

Azure DevOps Local MCP Server is now generally available, supporting in-house automation and AI features in controlled environments. Improvements include enhanced authorization, broader object coverage, domain scoping, and per-project configuration, providing flexibility and secure deployment. Open-source access maintains a strong user community.

GitHub Enterprise Server 3.18 delivers extra security and scaling, with custom properties, merge rules, scalable project issues, code scanning notifications, and OpenTelemetry instrumentation. Dependabot access is easier for teams managing large codebases.

Actions Runner Controller 0.13.0 now offers container lifecycle hooks, improved dual-stack networking, and finalized Azure Key Vault integration for deployment, networking, and secrets management.

- [Azure DevOps Local MCP Server Now Generally Available](https://devblogs.microsoft.com/devops/azure-devops-local-mcp-server-generally-available/)
- [GitHub Enterprise Server 3.18 Release Overview](https://github.blog/changelog/2025-10-14-github-enterprise-server-3-18-is-now-generally-available)
- [Actions Runner Controller 0.13.0: Storage, Networking, and Azure Key Vault Updates](https://github.blog/changelog/2025-10-16-actions-runner-controller-release-0-13-0)

### GitHub Automation and Specification-Driven Development

GitHub MCP Server introduces management for Projects, centralizing configuration-driven workflow automation and building on efficiency improvements from earlier versions.

Spec Kit rolls out a specification-first approach, using commands like `/specify`, `/plan`, and `/implement` to convert requirements into actionable code plans and scaffolding, supporting more systematic automation of software projects.

- [GitHub MCP Server Adds GitHub Projects Management and Improves Toolset Efficiency](https://github.blog/changelog/2025-10-14-github-mcp-server-now-supports-github-projects-and-more)
- [Introducing GitHub Spec Kit: A New Approach to Software Development](/devops/videos/introducing-github-spec-kit-a-new-approach-to-software-development)

### Observability, Toolchain Unification, and Real-World Security Practices

Site reliability engineers (SREs) address alert volume by prioritizing actionable metrics and post-incident reviews. OpenTelemetry unifies telemetry and continues to improve monitoring, as seen with GitHub Enterprise Server.

Governance solutions like CloudBees Unify bring artifact tracking across platforms, helping organizations gradually adopt more automated and AI‑ready DevOps without abrupt migration.

Despite advancement in infrastructure as code and DevOps security, implementation sometimes falls short. Recommendations focus on automating routine tasks, implementing policy-as-code, and pairing AI with direct engineering oversight.

A Fastly survey notes rapid but measured AI uptake in DevSecOps, emphasizing that automation needs human review for best results.

- [When Metrics Overwhelm: How SREs Help Engineers Reclaim Focus](https://devops.com/when-metrics-overwhelm-how-sres-help-engineers-reclaim-focus/)
- [Beyond the Platform: How Enterprises Can Unify Their DevOps Toolchains for Better Governance and AI Readiness](https://devops.com/beyond-the-platform-how-enterprises-can-unify-their-devops-toolchains-for-better-governance-and-ai-readiness/)
- [Infrastructure as Code, Security Blind Spots, and the Messy Reality of DevOps](https://devops.com/infrastructure-as-code-security-blind-spots-and-the-messy-reality-of-devops/)
- [Survey Reveals Rapid AI Adoption to Strengthen DevSecOps Practices](https://devops.com/survey-surfaces-widespread-adoption-of-ai-to-improve-devsecops/)',
     excerpt      = 'DevOps news includes improvements to secure automation, tool unification, and observability. Major updates were released for Azure DevOps, GitHub Enterprise Server, and other open-source components. Industry insights explore the ongoing challenges of complexity and security, with a focus on the intersection of automation and human review.',
     content_hash = md5('DevOps news includes improvements to secure automation, tool unification, and observability. Major updates were released for Azure DevOps, GitHub Enterprise Server, and other open-source components. Industry insights explore the ongoing challenges of complexity and security, with a focus on the intersection of automation and human review.

<!--excerpt_end-->

## DevOps

### Azure DevOps, GitHub Enterprise Server, and Self-Hosted Runner Innovations

Azure DevOps Local MCP Server is now generally available, supporting in-house automation and AI features in controlled environments. Improvements include enhanced authorization, broader object coverage, domain scoping, and per-project configuration, providing flexibility and secure deployment. Open-source access maintains a strong user community.

GitHub Enterprise Server 3.18 delivers extra security and scaling, with custom properties, merge rules, scalable project issues, code scanning notifications, and OpenTelemetry instrumentation. Dependabot access is easier for teams managing large codebases.

Actions Runner Controller 0.13.0 now offers container lifecycle hooks, improved dual-stack networking, and finalized Azure Key Vault integration for deployment, networking, and secrets management.

- [Azure DevOps Local MCP Server Now Generally Available](https://devblogs.microsoft.com/devops/azure-devops-local-mcp-server-generally-available/)
- [GitHub Enterprise Server 3.18 Release Overview](https://github.blog/changelog/2025-10-14-github-enterprise-server-3-18-is-now-generally-available)
- [Actions Runner Controller 0.13.0: Storage, Networking, and Azure Key Vault Updates](https://github.blog/changelog/2025-10-16-actions-runner-controller-release-0-13-0)

### GitHub Automation and Specification-Driven Development

GitHub MCP Server introduces management for Projects, centralizing configuration-driven workflow automation and building on efficiency improvements from earlier versions.

Spec Kit rolls out a specification-first approach, using commands like `/specify`, `/plan`, and `/implement` to convert requirements into actionable code plans and scaffolding, supporting more systematic automation of software projects.

- [GitHub MCP Server Adds GitHub Projects Management and Improves Toolset Efficiency](https://github.blog/changelog/2025-10-14-github-mcp-server-now-supports-github-projects-and-more)
- [Introducing GitHub Spec Kit: A New Approach to Software Development](/devops/videos/introducing-github-spec-kit-a-new-approach-to-software-development)

### Observability, Toolchain Unification, and Real-World Security Practices

Site reliability engineers (SREs) address alert volume by prioritizing actionable metrics and post-incident reviews. OpenTelemetry unifies telemetry and continues to improve monitoring, as seen with GitHub Enterprise Server.

Governance solutions like CloudBees Unify bring artifact tracking across platforms, helping organizations gradually adopt more automated and AI‑ready DevOps without abrupt migration.

Despite advancement in infrastructure as code and DevOps security, implementation sometimes falls short. Recommendations focus on automating routine tasks, implementing policy-as-code, and pairing AI with direct engineering oversight.

A Fastly survey notes rapid but measured AI uptake in DevSecOps, emphasizing that automation needs human review for best results.

- [When Metrics Overwhelm: How SREs Help Engineers Reclaim Focus](https://devops.com/when-metrics-overwhelm-how-sres-help-engineers-reclaim-focus/)
- [Beyond the Platform: How Enterprises Can Unify Their DevOps Toolchains for Better Governance and AI Readiness](https://devops.com/beyond-the-platform-how-enterprises-can-unify-their-devops-toolchains-for-better-governance-and-ai-readiness/)
- [Infrastructure as Code, Security Blind Spots, and the Messy Reality of DevOps](https://devops.com/infrastructure-as-code-security-blind-spots-and-the-messy-reality-of-devops/)
- [Survey Reveals Rapid AI Adoption to Strengthen DevSecOps Practices](https://devops.com/survey-surfaces-widespread-adoption-of-ai-to-improve-devsecops/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-10-20';

-- weekly-devops-roundup-2025-10-27  (2025-10-27)
UPDATE content_items
SET  content      = 'DevOps topics this week center on AI-enhanced automation for observability, security, infrastructure management, and improved productivity. Key articles discuss cloud-native tools paired with AI features for log handling, automatic remediation, agile governance, and platform updates from GitHub for enterprise-scale workflows. swamUP 2025 sessions revisit changing DevOps metrics, encouraging responsible deployments and teamwork in hybrid infrastructure.

<!--excerpt_end-->

## DevOps

### OpenTelemetry, AI, and Cost-Efficient Observability Architectures

Building on last week’s OpenTelemetry and observability coverage, AI’s role increases in log management, with better support for file, Kubernetes, and system journal logs in the OpenTelemetry Collector. New deployment options like Helm help teams standardize logging and adopt open architectures, moving away from vendor lock-in and toward scalable in-house databases.

Features like AI-driven log enrichment contribute real-time event grouping, anomaly detection, and improved root cause analysis. The Elastic Streams extension integrates AI normalization, supporting cross-signal visualization. The move to open standards and developer-friendly tools allows organizations to build cost-effective, flexible observability.

- [OpenTelemetry and AI are Unlocking Logs as the Essential Signal for ''Why''](https://devops.com/opentelemetry-and-ai-are-unlocking-logs-as-the-essential-signal-for-why/)
- [Open Cost-Efficient Architectures for Observability: Escaping Vendor Lock-In and Ballooning Costs](https://devops.com/breaking-free-from-rising-observability-costs-with-open-cost-efficient-architectures/)

### DevOps, AI Adoption, and Metrics Evolution

New DevOps metrics shift from speed alone to include trust and transparency. swamUP 2025 talks feature pipelines incorporating explainability and continuous safeguards, blending automation with solid governance and policy enforcement for responsible software delivery.

- [The New Metrics of DevOps: Speed, Trust and Transparency](https://devops.com/the-new-metrics-of-devops-speed-trust-and-transparency/)
- [How DevOps Is Evolving for the Age of Intelligent Automation](https://devops.com/how-devops-is-evolving-for-the-age-of-intelligent-automation/)

### Automated Vulnerability Remediation and Nano Updates in DevOps

Security and remediation stay in focus with ActiveState’s vulnerability report, showing high costs of manual patching for open source. Automated patch platforms for smart CI/CD are recommended to lower risk and developer effort.

Small, targeted updates ("nano updates") allow for secure system maintenance with minimal disruption, especially when paired with solid dependency management and container rebasing. Collaboration across security and engineering teams supports ongoing vulnerability response and continuous improvement.

- [The Silent Technical Debt: Why Manual Remediation Is Costing You More Than You Think](https://devops.com/the-silent-technical-debt-why-manual-remediation-is-costing-you-more-than-you-think/)
- [Why Nano Updates Only Work if You Begin with the Latest and Greatest Software](https://devops.com/why-nano-updates-only-work-if-you-begin-with-the-latest-and-greatest-software/)

### GitHub Enterprise Cloud: Governance, Security, and Organization Management

GitHub Enterprise Cloud adds new previews for centralized team and role management, introducing roles like Enterprise Security Manager and controls for code/secret scanning and Dependabot alerts. Admins gain abilities to manage permissions and compliance exceptions using APIs and the UI.

A preview for organization custom properties lets admins attach metadata and automate rules across organizations, improving compliance and reducing manual mistakes.

- [Enterprise Teams Public Preview: Centralized Role and Security Management in GitHub Enterprise Cloud](https://github.blog/changelog/2025-10-23-managing-roles-and-governance-via-enterprise-teams-is-in-public-preview)
- [Streamlining Enterprise Governance with GitHub Organization Custom Properties](https://github.blog/changelog/2025-10-23-organization-custom-properties-are-now-available-in-public-preview)

### AI-Powered Code Quality and Analysis in DevOps

Building on MCP and prior code analysis, Opsera’s Hummingbird AI reviews code in CI/CD, surfacing insights into quality and productivity. Its recommendations, natural-language search, and system compatibility address compliance requirements for teams using several AI solutions.

- [Opsera Introduces AI Agent for Analyzing Code Quality from AI Coding Tools](https://devops.com/opsera-adds-ai-agent-capable-of-analyzing-quality-of-code-generated-by-ai-tools/)

### SRE for AI and Hybrid Infrastructure

Site Reliability Engineering (SRE) now includes operations for GPU clusters, hybrid pipelines, and AI inference metrics. Automation and incident prediction tools support the changing reliability landscape for AI-specific workloads, including cost management and deployment across multiple clouds.

- [From Cloud to Cognitive Infrastructure: How AI is Redefining the Next Frontier of SRE](https://devops.com/from-cloud-to-cognitive-infrastructure-how-ai-is-redefining-the-next-frontier-of-sre/)

### Hybrid Cloud and DevOps Workflows

More DevOps teams are moving workloads to private/hybrid clouds for greater control, data security, and cost oversight. Modern private clouds now support container orchestration, Infrastructure-as-Code, and microservices for easier automation and planning—improving budget management and reducing hidden IT risks.

- [The Cloud Reset: Why DevOps Teams Are Returning Workloads to Private Clouds](https://devops.com/the-cloud-reset-devops-brings-workloads-back-to-private-clouds/)',
     excerpt      = 'DevOps topics this week center on AI-enhanced automation for observability, security, infrastructure management, and improved productivity. Key articles discuss cloud-native tools paired with AI features for log handling, automatic remediation, agile governance, and platform updates from GitHub for enterprise-scale workflows. swamUP 2025 sessions revisit changing DevOps metrics, encouraging responsible deployments and teamwork in hybrid infrastructure.',
     content_hash = md5('DevOps topics this week center on AI-enhanced automation for observability, security, infrastructure management, and improved productivity. Key articles discuss cloud-native tools paired with AI features for log handling, automatic remediation, agile governance, and platform updates from GitHub for enterprise-scale workflows. swamUP 2025 sessions revisit changing DevOps metrics, encouraging responsible deployments and teamwork in hybrid infrastructure.

<!--excerpt_end-->

## DevOps

### OpenTelemetry, AI, and Cost-Efficient Observability Architectures

Building on last week’s OpenTelemetry and observability coverage, AI’s role increases in log management, with better support for file, Kubernetes, and system journal logs in the OpenTelemetry Collector. New deployment options like Helm help teams standardize logging and adopt open architectures, moving away from vendor lock-in and toward scalable in-house databases.

Features like AI-driven log enrichment contribute real-time event grouping, anomaly detection, and improved root cause analysis. The Elastic Streams extension integrates AI normalization, supporting cross-signal visualization. The move to open standards and developer-friendly tools allows organizations to build cost-effective, flexible observability.

- [OpenTelemetry and AI are Unlocking Logs as the Essential Signal for ''Why''](https://devops.com/opentelemetry-and-ai-are-unlocking-logs-as-the-essential-signal-for-why/)
- [Open Cost-Efficient Architectures for Observability: Escaping Vendor Lock-In and Ballooning Costs](https://devops.com/breaking-free-from-rising-observability-costs-with-open-cost-efficient-architectures/)

### DevOps, AI Adoption, and Metrics Evolution

New DevOps metrics shift from speed alone to include trust and transparency. swamUP 2025 talks feature pipelines incorporating explainability and continuous safeguards, blending automation with solid governance and policy enforcement for responsible software delivery.

- [The New Metrics of DevOps: Speed, Trust and Transparency](https://devops.com/the-new-metrics-of-devops-speed-trust-and-transparency/)
- [How DevOps Is Evolving for the Age of Intelligent Automation](https://devops.com/how-devops-is-evolving-for-the-age-of-intelligent-automation/)

### Automated Vulnerability Remediation and Nano Updates in DevOps

Security and remediation stay in focus with ActiveState’s vulnerability report, showing high costs of manual patching for open source. Automated patch platforms for smart CI/CD are recommended to lower risk and developer effort.

Small, targeted updates ("nano updates") allow for secure system maintenance with minimal disruption, especially when paired with solid dependency management and container rebasing. Collaboration across security and engineering teams supports ongoing vulnerability response and continuous improvement.

- [The Silent Technical Debt: Why Manual Remediation Is Costing You More Than You Think](https://devops.com/the-silent-technical-debt-why-manual-remediation-is-costing-you-more-than-you-think/)
- [Why Nano Updates Only Work if You Begin with the Latest and Greatest Software](https://devops.com/why-nano-updates-only-work-if-you-begin-with-the-latest-and-greatest-software/)

### GitHub Enterprise Cloud: Governance, Security, and Organization Management

GitHub Enterprise Cloud adds new previews for centralized team and role management, introducing roles like Enterprise Security Manager and controls for code/secret scanning and Dependabot alerts. Admins gain abilities to manage permissions and compliance exceptions using APIs and the UI.

A preview for organization custom properties lets admins attach metadata and automate rules across organizations, improving compliance and reducing manual mistakes.

- [Enterprise Teams Public Preview: Centralized Role and Security Management in GitHub Enterprise Cloud](https://github.blog/changelog/2025-10-23-managing-roles-and-governance-via-enterprise-teams-is-in-public-preview)
- [Streamlining Enterprise Governance with GitHub Organization Custom Properties](https://github.blog/changelog/2025-10-23-organization-custom-properties-are-now-available-in-public-preview)

### AI-Powered Code Quality and Analysis in DevOps

Building on MCP and prior code analysis, Opsera’s Hummingbird AI reviews code in CI/CD, surfacing insights into quality and productivity. Its recommendations, natural-language search, and system compatibility address compliance requirements for teams using several AI solutions.

- [Opsera Introduces AI Agent for Analyzing Code Quality from AI Coding Tools](https://devops.com/opsera-adds-ai-agent-capable-of-analyzing-quality-of-code-generated-by-ai-tools/)

### SRE for AI and Hybrid Infrastructure

Site Reliability Engineering (SRE) now includes operations for GPU clusters, hybrid pipelines, and AI inference metrics. Automation and incident prediction tools support the changing reliability landscape for AI-specific workloads, including cost management and deployment across multiple clouds.

- [From Cloud to Cognitive Infrastructure: How AI is Redefining the Next Frontier of SRE](https://devops.com/from-cloud-to-cognitive-infrastructure-how-ai-is-redefining-the-next-frontier-of-sre/)

### Hybrid Cloud and DevOps Workflows

More DevOps teams are moving workloads to private/hybrid clouds for greater control, data security, and cost oversight. Modern private clouds now support container orchestration, Infrastructure-as-Code, and microservices for easier automation and planning—improving budget management and reducing hidden IT risks.

- [The Cloud Reset: Why DevOps Teams Are Returning Workloads to Private Clouds](https://devops.com/the-cloud-reset-devops-brings-workloads-back-to-private-clouds/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-10-27';

-- weekly-devops-roundup-2025-11-03  (2025-11-03)
UPDATE content_items
SET  content      = 'DevOps updates focus on the use of AI-enabled automation and enhanced security. New solutions improve agent management, smooth security processes, and address the intersection of technical and organizational challenges. Topics include GitHub Universe highlights, observability techniques, quantum readiness, and advances in cloud-native pipeline management.

<!--excerpt_end-->

## DevOps

### AI Agents, Automation, and GitHub DevOps Platform

GitHub Universe introduced Agent HQ, providing a more unified platform for managing and orchestrating AI agents, including support for multiple frameworks and audit logs. This matches previous discussions on multi-agent orchestration.

Copilot-driven tooling and code reviews continue to streamline automated development practices, moving teams from manual coding to orchestrating and refining agent outputs.

Guides for integrating agents with Azure AI Foundry and managed identities extend multi-agent scenarios and security for authentication. GitHub MCP Server updates standardize automation and prompts, supporting expanded enterprise adoption.

VS Code integrations from Universe emphasize collaboration, faster CI/CD feedback, and more practical DevOps workflows.

- [GitHub Launches Agent HQ for Unified AI Agent Management in DevOps Workflows](https://devops.com/github-adds-platform-for-managing-ai-agents-embedded-in-devops-workflows/)
- [Authentic DevOps with AI Foundry and GitHub: Enhancing Security Automation](/ai/videos/authentic-devops-with-ai-foundry-and-github-enhancing-security-automation)
- [Enhancements to GitHub MCP Server: Server Instructions and Multifunctional Tools](https://github.blog/changelog/2025-10-29-github-mcp-server-now-comes-with-server-instructions-better-tools-and-more)
- [VS Code Live: Highlights from GitHub Universe Announcements](/devops/videos/vs-code-live-highlights-from-github-universe-announcements)

### Modern Approaches to DevOps Observability

The ongoing focus on observability shows multi-signal telemetry strategies and the practical benefits of cloud monitoring for teams. New frameworks for categorizing telemetry support better incident response and operational understanding.

Outcome-focused metrics, such as p99 latency and deployment success rates, help teams distinguish valuable signals and make informed engineering decisions. Case studies from Airbnb, Spotify, and Riot Games demonstrate the benefits of proactive monitoring, shift-left validation, and improved user-centric debugging. Integrations with popular tools like Grafana and Clepher support effective, cross-platform monitoring.

- [A Modern Approach to Multi-Signal Optimization](https://devops.com/a-modern-approach-to-multi-signal-optimization/)
- [How Observability Improves User Experience and Digital Performance](https://devops.com/how-observability-improves-user-experience-and-digital-performance/)

### Securing DevOps: Patch Management and AI Tooling Risks

Security coverage includes new resources for automated patch management, vulnerability remediation, and CI/CD validation, continuing recent trends in proactive security. Teams are encouraged to practice collaborative, metric-driven patch deployment.

New analysis of AI tool adoption highlights onboarding gains but also notes the importance of compliance checks for verbose or AI-generated code.

- [Patch Management is Essential for Securing DevOps](https://devops.com/patch-management-is-essential-for-securing-devops/)
- [Impact of AI Coding Tools on DevOps Workflows: Analysis of EMA/Perforce Survey](https://devops.com/survey-surfaces-impact-ai-coding-tools-are-having-on-devops-workflows/)

### Cloud-Native Pipeline Innovation and Quantum Readiness

Pipeline automation grows with Dalec, an open-source CNCF project that supports declarative, multi-distribution packaging and secure builds. Dalec’s features—support for Azure Linux, Ubuntu, SBOM, and signature validation—expand on prior work in reproducible builds and security audits.

Guidance continues to support quantum readiness through updated migration, simulation, and SDK integration for hybrid cloud environments.

- [Dalec: Declarative Linux Package and Container Builds with Azure Linux and Docker BuildKit](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/dalec-declarative-package-and-container-builds/ba-p/4465290)
- [Quantum-Ready Cloud DevOps: Preparing for Quantum Computing Integration](https://devops.com/quantum%e2%80%91ready-cloud-devops-getting-ready-for-quantum-computing-integration/)

### Other DevOps News

Broader resources include:

- Analysis of business and technical benefits from DevOps implementation, such as improved delivery speed, resource usage, and reliability.
- Perspectives on building sustainable DevOps through automation, visibility, and feedback.
- A step-by-step guide to choosing DevOps vendors, addressing technical, QA, and risk needs.

These continue last week’s coverage on automation and DevOps maturity.

- [7 Proven Benefits of DevOps Implementation in Modern Software Development](https://devops.com/7-proven-benefits-of-devops-implementation-in-modern-software-development/)
- [How Data, Empathy and Visibility Are Redefining DevOps Maturity](https://devops.com/how-data-empathy-and-visibility-are-redefining-devops-maturity/)
- [An Experience-Based Guide to Choosing the Right DevOps Provider in 2026](https://devops.com/an-experience-based-guide-to-choosing-the-right-devops-provider-in-2026/)',
     excerpt      = 'DevOps updates focus on the use of AI-enabled automation and enhanced security. New solutions improve agent management, smooth security processes, and address the intersection of technical and organizational challenges. Topics include GitHub Universe highlights, observability techniques, quantum readiness, and advances in cloud-native pipeline management.',
     content_hash = md5('DevOps updates focus on the use of AI-enabled automation and enhanced security. New solutions improve agent management, smooth security processes, and address the intersection of technical and organizational challenges. Topics include GitHub Universe highlights, observability techniques, quantum readiness, and advances in cloud-native pipeline management.

<!--excerpt_end-->

## DevOps

### AI Agents, Automation, and GitHub DevOps Platform

GitHub Universe introduced Agent HQ, providing a more unified platform for managing and orchestrating AI agents, including support for multiple frameworks and audit logs. This matches previous discussions on multi-agent orchestration.

Copilot-driven tooling and code reviews continue to streamline automated development practices, moving teams from manual coding to orchestrating and refining agent outputs.

Guides for integrating agents with Azure AI Foundry and managed identities extend multi-agent scenarios and security for authentication. GitHub MCP Server updates standardize automation and prompts, supporting expanded enterprise adoption.

VS Code integrations from Universe emphasize collaboration, faster CI/CD feedback, and more practical DevOps workflows.

- [GitHub Launches Agent HQ for Unified AI Agent Management in DevOps Workflows](https://devops.com/github-adds-platform-for-managing-ai-agents-embedded-in-devops-workflows/)
- [Authentic DevOps with AI Foundry and GitHub: Enhancing Security Automation](/ai/videos/authentic-devops-with-ai-foundry-and-github-enhancing-security-automation)
- [Enhancements to GitHub MCP Server: Server Instructions and Multifunctional Tools](https://github.blog/changelog/2025-10-29-github-mcp-server-now-comes-with-server-instructions-better-tools-and-more)
- [VS Code Live: Highlights from GitHub Universe Announcements](/devops/videos/vs-code-live-highlights-from-github-universe-announcements)

### Modern Approaches to DevOps Observability

The ongoing focus on observability shows multi-signal telemetry strategies and the practical benefits of cloud monitoring for teams. New frameworks for categorizing telemetry support better incident response and operational understanding.

Outcome-focused metrics, such as p99 latency and deployment success rates, help teams distinguish valuable signals and make informed engineering decisions. Case studies from Airbnb, Spotify, and Riot Games demonstrate the benefits of proactive monitoring, shift-left validation, and improved user-centric debugging. Integrations with popular tools like Grafana and Clepher support effective, cross-platform monitoring.

- [A Modern Approach to Multi-Signal Optimization](https://devops.com/a-modern-approach-to-multi-signal-optimization/)
- [How Observability Improves User Experience and Digital Performance](https://devops.com/how-observability-improves-user-experience-and-digital-performance/)

### Securing DevOps: Patch Management and AI Tooling Risks

Security coverage includes new resources for automated patch management, vulnerability remediation, and CI/CD validation, continuing recent trends in proactive security. Teams are encouraged to practice collaborative, metric-driven patch deployment.

New analysis of AI tool adoption highlights onboarding gains but also notes the importance of compliance checks for verbose or AI-generated code.

- [Patch Management is Essential for Securing DevOps](https://devops.com/patch-management-is-essential-for-securing-devops/)
- [Impact of AI Coding Tools on DevOps Workflows: Analysis of EMA/Perforce Survey](https://devops.com/survey-surfaces-impact-ai-coding-tools-are-having-on-devops-workflows/)

### Cloud-Native Pipeline Innovation and Quantum Readiness

Pipeline automation grows with Dalec, an open-source CNCF project that supports declarative, multi-distribution packaging and secure builds. Dalec’s features—support for Azure Linux, Ubuntu, SBOM, and signature validation—expand on prior work in reproducible builds and security audits.

Guidance continues to support quantum readiness through updated migration, simulation, and SDK integration for hybrid cloud environments.

- [Dalec: Declarative Linux Package and Container Builds with Azure Linux and Docker BuildKit](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/dalec-declarative-package-and-container-builds/ba-p/4465290)
- [Quantum-Ready Cloud DevOps: Preparing for Quantum Computing Integration](https://devops.com/quantum%e2%80%91ready-cloud-devops-getting-ready-for-quantum-computing-integration/)

### Other DevOps News

Broader resources include:

- Analysis of business and technical benefits from DevOps implementation, such as improved delivery speed, resource usage, and reliability.
- Perspectives on building sustainable DevOps through automation, visibility, and feedback.
- A step-by-step guide to choosing DevOps vendors, addressing technical, QA, and risk needs.

These continue last week’s coverage on automation and DevOps maturity.

- [7 Proven Benefits of DevOps Implementation in Modern Software Development](https://devops.com/7-proven-benefits-of-devops-implementation-in-modern-software-development/)
- [How Data, Empathy and Visibility Are Redefining DevOps Maturity](https://devops.com/how-data-empathy-and-visibility-are-redefining-devops-maturity/)
- [An Experience-Based Guide to Choosing the Right DevOps Provider in 2026](https://devops.com/an-experience-based-guide-to-choosing-the-right-devops-provider-in-2026/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-11-03';

-- weekly-devops-roundup-2025-11-10  (2025-11-10)
UPDATE content_items
SET  content      = 'Recent DevOps news features more automation, expanded use of AI, and improved collaboration tools. Updates prioritize secure, resilient workflows, offering new CI/CD features, security integration, and cost management. Efforts continue to make automation accessible and operations more reliable in enterprise, cloud-native, and AI-powered settings.

<!--excerpt_end-->

## DevOps

### GitHub Platform and Developer Workflow Updates

GitHub Actions now allows up to 10 nested workflows and 50 calls per run, supporting complex CI/CD automation. M2 macOS runners with GPU support and changes to Copilot Agent enablement in Actions demonstrate advances in integration. Security policies require `pull_request_target` events to run only on default branches after December 8, and environment protection rules now cover actual execution branches. Additional updates include billing APIs, notifications, onboarding, and API documentation. Code search receives an `enterprise:` qualifier, and rulesets for team-based branch approvals improve control. The retirement of GraphQL Explorer further streamlines API documentation.

- [GitHub Actions November 2025 Releases: Increased Limits, M2 Runners, and Copilot Agent Update](https://github.blog/changelog/2025-11-06-new-releases-for-github-actions-november-2025)
- [Important Changes to GitHub Actions: pull_request_target and Environment Branch Protection Rules](https://github.blog/changelog/2025-11-07-actions-pull_request_target-and-environment-branch-protections-changes)
- [GitHub Billing API Updates: Programmatic Budget Management and Usage Tracking](https://github.blog/changelog/2025-11-03-manage-budgets-and-track-usage-with-new-billing-api-updates)
- [Removing Notifications for @mentions in Commit Messages](https://github.blog/changelog/2025-11-07-removing-notifications-for-mentions-in-commit-messages)
- [Improved Onboarding Flow for GitHub Projects](https://github.blog/changelog/2025-11-06-improved-onboarding-flow-for-github-projects)
- [GitHub Introduces ''enterprise:'' Qualifier for Enhanced Code Search](https://github.blog/changelog/2025-11-05-enterprise-qualifier-is-now-generally-available-in-github-code-search)
- [Require Team Approvals for Protected Branches in GitHub Rulesets](https://github.blog/changelog/2025-11-03-required-review-by-specific-teams-now-available-in-rulesets)
- [GitHub Retires GraphQL Explorer from API Documentation](https://github.blog/changelog/2025-11-07-graphql-explorer-removal-from-api-documentation-on-november-7-2025)

### AI-Driven DevOps and Observability Tools

AI agent integration increases with Qovery’s Copilot agents (Anthropic Claude LLM) automating environment setup and governance, including secure credential management. Tabnine introduces agentic refactoring, compliance workflows, and a context engine with greater flexibility. Observe Inc. links SRE and o11y.ai agents for automating incident analysis and telemetry, now supporting MCP servers. Kong Insomnia 12 provides MCP for API testing, prototyping, and compliance with RBAC.

- [Qovery Introduces AI Agents to Enhance DevOps Automation](https://devops.com/qovery-adds-multiple-ai-agents-to-devops-automation-platform/)
- [Tabnine Launches AI Agents for Automated DevOps Workflows](https://devops.com/tabnine-adds-agents-capable-of-automating-workflows-to-ai-coding-platform/)
- [Observe Integrates AI Agents to Enhance Observability for DevOps Teams](https://devops.com/observe-adds-two-ai-agents-to-improve-observability/)
- [Kong Adds Model Context Protocol Support to Insomnia API Tool](https://devops.com/kong-adds-mcp-support-to-tool-for-designing-and-testing-apis/)

### AIOps and the Evolution of DevOps Monitoring

AIOps achieves new maturity, as guides show SREs using AI for reducing on-call fatigue and faster incident management. Case studies examine event anomaly detection, correlation, and automated remediation. Debate over DevOps versus AIOps clarifies how analytics and AI-based automation are shaping contemporary DevOps pipelines.

- [AIOps for SRE: Leveraging AI to Combat On-Call Fatigue and Boost Reliability](https://devops.com/aiops-for-sre-using-ai-to-reduce-on-call-fatigue-and-improve-reliability/)
- [Is There Still a Difference Between DevOps and AIOps?](https://devops.com/is-there-still-a-difference-between-devops-and-aiops/)
- [How AIOps is Revolutionizing DevOps Monitoring in the Cloud Era](https://devops.com/how-aiops-is-revolutionizing-devops-monitoring-in-the-cloud-era/)

### Workflow Design, Optimization, and Collaboration

Analysis from Octoverse highlights the adoption of frequent commits, CI/CD, and feature flags. Guides reinforce the importance of automated tests, infrastructure as code, and continuous monitoring. Cost management content recommends optimizing workloads before seeking provider discounts. Security tips emphasize the value of automated tools and joint improvements between development and security teams.

- [Developer Workflows in 2025: Insights from 986 Million Code Pushes](https://github.blog/news-insights/octoverse/what-986-million-code-pushes-say-about-the-developer-workflow-in-2025/)
- [DevOps Workflow: The Key Elements and Tools Involved](https://devops.com/devops-workflow-the-key-elements-and-tools-involved/)
- [Avoiding Cloud Cost Traps: Optimize Workloads Before Negotiating Discounts](https://devops.com/the-most-destructive-cloud-cost-pitfall-discounts-before-optimization/)
- [How Cybersecurity Teams Can Work Better with DevOps](https://devops.com/how-cybersecurity-teams-can-work-better-with-devops/)

### DevSecOps Integration and Security Automation

Security updates focus on integrating analysis tools (SonarQube, Semgrep), Dependabot, Snyk, and Trivy into DevOps pipelines. Guides cover remediation gates and "security champion" roles, aiming to balance velocity and assurance—furthering approaches covered in previous roundups.

- [DevSecOps in Practice: Closing the Gap Between Development Speed and Security Assurance](https://devops.com/devsecops-in-practice-closing-the-gap-between-development-speed-and-security-assurance/)

### Other DevOps News

Highlights from GitHub Universe 2025—including Agent HQ, Octoverse, and Game Off—are reviewed in "The Download." The episode examines open source security, project onboarding, and developer tooling, continuing community development coverage from prior roundups.

- [The Download: Highlights from GitHub Universe 2025, Octoverse, and Game Off](/devops/videos/the-download-highlights-from-github-universe-2025-octoverse-and-game-off)',
     excerpt      = 'Recent DevOps news features more automation, expanded use of AI, and improved collaboration tools. Updates prioritize secure, resilient workflows, offering new CI/CD features, security integration, and cost management. Efforts continue to make automation accessible and operations more reliable in enterprise, cloud-native, and AI-powered settings.',
     content_hash = md5('Recent DevOps news features more automation, expanded use of AI, and improved collaboration tools. Updates prioritize secure, resilient workflows, offering new CI/CD features, security integration, and cost management. Efforts continue to make automation accessible and operations more reliable in enterprise, cloud-native, and AI-powered settings.

<!--excerpt_end-->

## DevOps

### GitHub Platform and Developer Workflow Updates

GitHub Actions now allows up to 10 nested workflows and 50 calls per run, supporting complex CI/CD automation. M2 macOS runners with GPU support and changes to Copilot Agent enablement in Actions demonstrate advances in integration. Security policies require `pull_request_target` events to run only on default branches after December 8, and environment protection rules now cover actual execution branches. Additional updates include billing APIs, notifications, onboarding, and API documentation. Code search receives an `enterprise:` qualifier, and rulesets for team-based branch approvals improve control. The retirement of GraphQL Explorer further streamlines API documentation.

- [GitHub Actions November 2025 Releases: Increased Limits, M2 Runners, and Copilot Agent Update](https://github.blog/changelog/2025-11-06-new-releases-for-github-actions-november-2025)
- [Important Changes to GitHub Actions: pull_request_target and Environment Branch Protection Rules](https://github.blog/changelog/2025-11-07-actions-pull_request_target-and-environment-branch-protections-changes)
- [GitHub Billing API Updates: Programmatic Budget Management and Usage Tracking](https://github.blog/changelog/2025-11-03-manage-budgets-and-track-usage-with-new-billing-api-updates)
- [Removing Notifications for @mentions in Commit Messages](https://github.blog/changelog/2025-11-07-removing-notifications-for-mentions-in-commit-messages)
- [Improved Onboarding Flow for GitHub Projects](https://github.blog/changelog/2025-11-06-improved-onboarding-flow-for-github-projects)
- [GitHub Introduces ''enterprise:'' Qualifier for Enhanced Code Search](https://github.blog/changelog/2025-11-05-enterprise-qualifier-is-now-generally-available-in-github-code-search)
- [Require Team Approvals for Protected Branches in GitHub Rulesets](https://github.blog/changelog/2025-11-03-required-review-by-specific-teams-now-available-in-rulesets)
- [GitHub Retires GraphQL Explorer from API Documentation](https://github.blog/changelog/2025-11-07-graphql-explorer-removal-from-api-documentation-on-november-7-2025)

### AI-Driven DevOps and Observability Tools

AI agent integration increases with Qovery’s Copilot agents (Anthropic Claude LLM) automating environment setup and governance, including secure credential management. Tabnine introduces agentic refactoring, compliance workflows, and a context engine with greater flexibility. Observe Inc. links SRE and o11y.ai agents for automating incident analysis and telemetry, now supporting MCP servers. Kong Insomnia 12 provides MCP for API testing, prototyping, and compliance with RBAC.

- [Qovery Introduces AI Agents to Enhance DevOps Automation](https://devops.com/qovery-adds-multiple-ai-agents-to-devops-automation-platform/)
- [Tabnine Launches AI Agents for Automated DevOps Workflows](https://devops.com/tabnine-adds-agents-capable-of-automating-workflows-to-ai-coding-platform/)
- [Observe Integrates AI Agents to Enhance Observability for DevOps Teams](https://devops.com/observe-adds-two-ai-agents-to-improve-observability/)
- [Kong Adds Model Context Protocol Support to Insomnia API Tool](https://devops.com/kong-adds-mcp-support-to-tool-for-designing-and-testing-apis/)

### AIOps and the Evolution of DevOps Monitoring

AIOps achieves new maturity, as guides show SREs using AI for reducing on-call fatigue and faster incident management. Case studies examine event anomaly detection, correlation, and automated remediation. Debate over DevOps versus AIOps clarifies how analytics and AI-based automation are shaping contemporary DevOps pipelines.

- [AIOps for SRE: Leveraging AI to Combat On-Call Fatigue and Boost Reliability](https://devops.com/aiops-for-sre-using-ai-to-reduce-on-call-fatigue-and-improve-reliability/)
- [Is There Still a Difference Between DevOps and AIOps?](https://devops.com/is-there-still-a-difference-between-devops-and-aiops/)
- [How AIOps is Revolutionizing DevOps Monitoring in the Cloud Era](https://devops.com/how-aiops-is-revolutionizing-devops-monitoring-in-the-cloud-era/)

### Workflow Design, Optimization, and Collaboration

Analysis from Octoverse highlights the adoption of frequent commits, CI/CD, and feature flags. Guides reinforce the importance of automated tests, infrastructure as code, and continuous monitoring. Cost management content recommends optimizing workloads before seeking provider discounts. Security tips emphasize the value of automated tools and joint improvements between development and security teams.

- [Developer Workflows in 2025: Insights from 986 Million Code Pushes](https://github.blog/news-insights/octoverse/what-986-million-code-pushes-say-about-the-developer-workflow-in-2025/)
- [DevOps Workflow: The Key Elements and Tools Involved](https://devops.com/devops-workflow-the-key-elements-and-tools-involved/)
- [Avoiding Cloud Cost Traps: Optimize Workloads Before Negotiating Discounts](https://devops.com/the-most-destructive-cloud-cost-pitfall-discounts-before-optimization/)
- [How Cybersecurity Teams Can Work Better with DevOps](https://devops.com/how-cybersecurity-teams-can-work-better-with-devops/)

### DevSecOps Integration and Security Automation

Security updates focus on integrating analysis tools (SonarQube, Semgrep), Dependabot, Snyk, and Trivy into DevOps pipelines. Guides cover remediation gates and "security champion" roles, aiming to balance velocity and assurance—furthering approaches covered in previous roundups.

- [DevSecOps in Practice: Closing the Gap Between Development Speed and Security Assurance](https://devops.com/devsecops-in-practice-closing-the-gap-between-development-speed-and-security-assurance/)

### Other DevOps News

Highlights from GitHub Universe 2025—including Agent HQ, Octoverse, and Game Off—are reviewed in "The Download." The episode examines open source security, project onboarding, and developer tooling, continuing community development coverage from prior roundups.

- [The Download: Highlights from GitHub Universe 2025, Octoverse, and Game Off](/devops/videos/the-download-highlights-from-github-universe-2025-octoverse-and-game-off)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-11-10';

-- weekly-devops-roundup-2025-11-17  (2025-11-17)
UPDATE content_items
SET  content      = 'This week, DevOps teams addressed complex automation requirements by focusing on business impact, improved reliability, and distributed cloud environments. New methods link SLOs to actual business value, support scalable pipeline orchestration, streamline onboarding, and expand observability—all moving towards continuous improvement and reduced hands-on effort.

<!--excerpt_end-->

## DevOps

### Business-Aligned SLOs and Advanced Reliability Engineering

Guidance moves beyond legacy SLO metrics, encouraging dashboards that tie performance indicators to end-user tiers and financial goals. For large-scale deployments, context-aware reliability models adjust resource allocation by customer segment, helping lower costs and improve outcomes. Practical blueprints and case studies show step-by-step ways to implement these methods.

- [Why Your SLO Dashboard Is Lying: Building Business-Aligned Service Level Objectives](https://devops.com/why-your-slo-dashboard-is-lying-moving-beyond-vanity-metrics-in-production/)
- [Context-Aware Reliability Contracts: Rethinking SLOs for Hyperscale DevOps](https://devops.com/why-traditional-slos-are-failing-at-hyperscale-building-context-aware-reliability-contracts/)

### GitOps, Pipeline Orchestration, and Scalable Multi-Cloud Delivery

Articles highlight multi-cloud adoption with advanced GitOps—featuring tools like ArgoCD, Flux, and policy-as-code—to meet regulatory requirements. Developers can now use Cake SDK to orchestrate GitHub Actions pipelines in C#, offering alternatives to YAML/JSON. Additions to OIDC token claims for GitHub Actions further strengthen security by enabling more granular identity controls.

- [Scaling GitOps for Continuous Delivery in Hybrid and Multi-Cloud Environments](https://devops.com/gitops-in-the-wild-scaling-continuous-delivery-in-hybrid-cloud-environments/)
- [Orchestrating GitHub Actions Pipelines in C# with Cake SDK](/coding/videos/orchestrating-github-actions-pipelines-in-c-with-cake-sdk)
- [GitHub Actions OIDC Token Claims Now Include check_run_id](https://github.blog/changelog/2025-11-13-github-actions-oidc-token-claims-now-include-check_run_id)

### Hybrid Cloud Load Balancing and Modernization with Microsoft Technologies

Practical advice covers load balancing across Kubernetes, containers, and virtual machines, including detailed YARP setup for authentication and multicluster routing. Steps for safely refactoring legacy environments are shared, and new onboarding automation with Aspire and Visual Studio aims to streamline compliance and reduce ramp-up time.

- [Multicluster Load Balancing with YARP: From Kubernetes to Legacy Containers](/coding/videos/multicluster-load-balancing-with-yarp-from-kubernetes-to-legacy-containers)
- [How to Refactor Legacy Solution Architectures Without Breaking Everything](https://dellenny.com/how-to-refactor-legacy-solution-architectures-without-breaking-everything/)
- [Automating Engineer Onboarding with .NET Aspire and Visual Studio](/coding/videos/automating-engineer-onboarding-with-net-aspire-and-visual-studio)

### Observability and Telemetry Automation

Bindplane’s Blueprints and Fleets automate deployment of OpenTelemetry collectors, promoting standardized and scalable observability for modern architectures, while increasing integration with AI-readiness.

- [Bindplane Adds Ability to Automate Deployment of OpenTelemetry Collectors](https://devops.com/bindplane-adds-ability-to-automate-deployment-of-opentelemetry-collectors/)

### API-First Development and AI in the DevOps Workflow

API governance is highlighted, with recommendations for integrating best practices into the CI/CD workflow. New research explores the organizational challenges of migrating to new platforms and combining AI tools, stressing the need for continuous adaptation and analytical insight.

- [The DevOps Impact of API-First Development](https://devops.com/the-devops-impact-of-api-first-development/)
- [Survey Reveals Challenges in DevOps Platform Migrations and AI Tool Integration](https://devops.com/survey-surfaces-multiple-devops-platform-migration-challenges/)

### Architecture as Code and Lightweight Governance

Updated perspectives on “Architecture as Code” support automated, version-controlled, and decentralized governance models, which help maintain clear and traceable architectural decisions.

- [Architecture as Code: Practical Approaches and Benefits](https://dellenny.com/architecture-as-code-what-it-means-and-how-to-apply-it/)
- [Architecture Decision Records (ADRs): A Lightweight Governance Model for Modern Teams](https://dellenny.com/architecture-decision-records-adrs-a-lightweight-governance-model-for-software-architecture/)

### Bridging ClickOps, IaC, and AI-Driven Infrastructure

Combined approaches to ClickOps, Infrastructure-as-Code (IaC), and AI-powered interfaces help organizations achieve fast prototyping without sacrificing policy compliance. Session locking, drift detection, and enhanced integrations maintain the momentum on compliant DevOps orchestration.

- [Bridging the Gap Between ClickOps and Infrastructure-as-Code in DevOps](https://devops.com/clickops-iac-and-the-excluded-avocado-middle/)

### Other DevOps News

GitHub Enterprise Cloud introduces new data residency options for Visual Studio subscribers. The October 2025 Availability Report provides transparency on service incidents, detailing mitigation responses and lessons for platform improvements.

- [GitHub Enterprise Cloud Data Residency Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-11-10-github-enterprise-cloud-with-data-residency-now-supports-visual-studio-subscriptions)
- [GitHub Availability Report: October 2025 – Analysis of Service Incidents](https://github.blog/news-insights/company-news/github-availability-report-october-2025/)',
     excerpt      = 'This week, DevOps teams addressed complex automation requirements by focusing on business impact, improved reliability, and distributed cloud environments. New methods link SLOs to actual business value, support scalable pipeline orchestration, streamline onboarding, and expand observability—all moving towards continuous improvement and reduced hands-on effort.',
     content_hash = md5('This week, DevOps teams addressed complex automation requirements by focusing on business impact, improved reliability, and distributed cloud environments. New methods link SLOs to actual business value, support scalable pipeline orchestration, streamline onboarding, and expand observability—all moving towards continuous improvement and reduced hands-on effort.

<!--excerpt_end-->

## DevOps

### Business-Aligned SLOs and Advanced Reliability Engineering

Guidance moves beyond legacy SLO metrics, encouraging dashboards that tie performance indicators to end-user tiers and financial goals. For large-scale deployments, context-aware reliability models adjust resource allocation by customer segment, helping lower costs and improve outcomes. Practical blueprints and case studies show step-by-step ways to implement these methods.

- [Why Your SLO Dashboard Is Lying: Building Business-Aligned Service Level Objectives](https://devops.com/why-your-slo-dashboard-is-lying-moving-beyond-vanity-metrics-in-production/)
- [Context-Aware Reliability Contracts: Rethinking SLOs for Hyperscale DevOps](https://devops.com/why-traditional-slos-are-failing-at-hyperscale-building-context-aware-reliability-contracts/)

### GitOps, Pipeline Orchestration, and Scalable Multi-Cloud Delivery

Articles highlight multi-cloud adoption with advanced GitOps—featuring tools like ArgoCD, Flux, and policy-as-code—to meet regulatory requirements. Developers can now use Cake SDK to orchestrate GitHub Actions pipelines in C#, offering alternatives to YAML/JSON. Additions to OIDC token claims for GitHub Actions further strengthen security by enabling more granular identity controls.

- [Scaling GitOps for Continuous Delivery in Hybrid and Multi-Cloud Environments](https://devops.com/gitops-in-the-wild-scaling-continuous-delivery-in-hybrid-cloud-environments/)
- [Orchestrating GitHub Actions Pipelines in C# with Cake SDK](/coding/videos/orchestrating-github-actions-pipelines-in-c-with-cake-sdk)
- [GitHub Actions OIDC Token Claims Now Include check_run_id](https://github.blog/changelog/2025-11-13-github-actions-oidc-token-claims-now-include-check_run_id)

### Hybrid Cloud Load Balancing and Modernization with Microsoft Technologies

Practical advice covers load balancing across Kubernetes, containers, and virtual machines, including detailed YARP setup for authentication and multicluster routing. Steps for safely refactoring legacy environments are shared, and new onboarding automation with Aspire and Visual Studio aims to streamline compliance and reduce ramp-up time.

- [Multicluster Load Balancing with YARP: From Kubernetes to Legacy Containers](/coding/videos/multicluster-load-balancing-with-yarp-from-kubernetes-to-legacy-containers)
- [How to Refactor Legacy Solution Architectures Without Breaking Everything](https://dellenny.com/how-to-refactor-legacy-solution-architectures-without-breaking-everything/)
- [Automating Engineer Onboarding with .NET Aspire and Visual Studio](/coding/videos/automating-engineer-onboarding-with-net-aspire-and-visual-studio)

### Observability and Telemetry Automation

Bindplane’s Blueprints and Fleets automate deployment of OpenTelemetry collectors, promoting standardized and scalable observability for modern architectures, while increasing integration with AI-readiness.

- [Bindplane Adds Ability to Automate Deployment of OpenTelemetry Collectors](https://devops.com/bindplane-adds-ability-to-automate-deployment-of-opentelemetry-collectors/)

### API-First Development and AI in the DevOps Workflow

API governance is highlighted, with recommendations for integrating best practices into the CI/CD workflow. New research explores the organizational challenges of migrating to new platforms and combining AI tools, stressing the need for continuous adaptation and analytical insight.

- [The DevOps Impact of API-First Development](https://devops.com/the-devops-impact-of-api-first-development/)
- [Survey Reveals Challenges in DevOps Platform Migrations and AI Tool Integration](https://devops.com/survey-surfaces-multiple-devops-platform-migration-challenges/)

### Architecture as Code and Lightweight Governance

Updated perspectives on “Architecture as Code” support automated, version-controlled, and decentralized governance models, which help maintain clear and traceable architectural decisions.

- [Architecture as Code: Practical Approaches and Benefits](https://dellenny.com/architecture-as-code-what-it-means-and-how-to-apply-it/)
- [Architecture Decision Records (ADRs): A Lightweight Governance Model for Modern Teams](https://dellenny.com/architecture-decision-records-adrs-a-lightweight-governance-model-for-software-architecture/)

### Bridging ClickOps, IaC, and AI-Driven Infrastructure

Combined approaches to ClickOps, Infrastructure-as-Code (IaC), and AI-powered interfaces help organizations achieve fast prototyping without sacrificing policy compliance. Session locking, drift detection, and enhanced integrations maintain the momentum on compliant DevOps orchestration.

- [Bridging the Gap Between ClickOps and Infrastructure-as-Code in DevOps](https://devops.com/clickops-iac-and-the-excluded-avocado-middle/)

### Other DevOps News

GitHub Enterprise Cloud introduces new data residency options for Visual Studio subscribers. The October 2025 Availability Report provides transparency on service incidents, detailing mitigation responses and lessons for platform improvements.

- [GitHub Enterprise Cloud Data Residency Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-11-10-github-enterprise-cloud-with-data-residency-now-supports-visual-studio-subscriptions)
- [GitHub Availability Report: October 2025 – Analysis of Service Incidents](https://github.blog/news-insights/company-news/github-availability-report-october-2025/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-11-17';

-- weekly-devops-roundup-2025-11-24  (2025-11-24)
UPDATE content_items
SET  content      = 'This collection covers practical automation in DevOps workflows, improvements to build and release processes, updates for GitHub Actions and VS Code, and enhancements around governance and migration support.

<!--excerpt_end-->

## DevOps

### GitHub and GitHub Actions: Migrations, Workflow Enhancements, and Platform Governance

GitHub continues supporting enterprise migrations with features like GitHub-owned blob storage, reducing setup complexity. New controls for managing App installations give organizations more say in integration security. Public preview updates to Pull Request “Files changed” help developers review large codebases. The Actions cache size limit increase expands support for larger monorepos and dependency sets.

- [Migrating Repositories with GitHub-Owned Blob Storage on GitHub Enterprise Cloud](https://github.blog/changelog/2025-11-17-migrating-repositories-with-github-owned-blob-storage-is-now-generally-available)
- [Controlling GitHub App Installations by Organization Owners](https://github.blog/changelog/2025-11-17-block-repository-administrators-from-installing-github-apps-on-their-own-now-in-public-preview)
- [Pull Request “Files Changed” Public Preview: November 20 Updates](https://github.blog/changelog/2025-11-20-pull-request-files-changed-public-preview-november-20-updates)
- [Expanded GitHub Actions Cache Limits Exceed 10 GB per Repository](https://github.blog/changelog/2025-11-20-github-actions-cache-size-can-now-exceed-10-gb-per-repository)

### CI/CD Automation, Migration, and Unified Build Approaches

New guides detail CI/CD automation in Microsoft Fabric and how to unify .NET build processes, streamlining deployment using virtual monorepos. Stories about CVS Health’s migration to GitHub Actions and guidance for migrating from Azure DevOps offer practical insight for teams moving to agent-based DevOps setups.

- [Automating Microsoft Fabric Deployments with Azure DevOps and Python](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/from-code-to-cloud-python-driven-microsoft-fabric-deployments/ba-p/4470447)
- [Reinventing .NET Build and Release: Unified Build Approach](https://devblogs.microsoft.com/dotnet/reinventing-how-dotnet-builds-and-ships-again/)
- [CVS Health’s Migration: Transforming Developer Experience with GitHub Actions](/devops/videos/cvs-healths-migration-transforming-developer-experience-with-github-actions)
- [Azure DevOps to GitHub Migration Playbook: A Step-by-Step Guide for Agentic DevOps](https://devblogs.microsoft.com/all-things-azure/azure-devops-to-github-migration-playbook-unlocking-agentic-devops/)

### Visual Studio Code: Private Marketplace and IT Governance

The VS Code Private Marketplace provides better governance for organizations. Sessions on deployment and AI oversight reinforce responsible adoption and management, echoing previous efforts to streamline onboarding while maintaining control.

- [VS Code Private Marketplace: Enterprise Control Meets Developer Speed](/azure/videos/vs-code-private-marketplace-enterprise-control-meets-developer-speed)
- [Visual Studio Code Deployment and AI Governance for IT Pros](/ai/videos/visual-studio-code-deployment-and-ai-governance-for-it-pros)

### Observability, Monitoring, and Security in DevOps Pipelines

Updated observability tools focus on proactive monitoring, with dashboards that help teams quickly identify incidents. MyDecisive’s Smart Telemetry Hub for Kubernetes and insights on deterministic guardrails reinforce a shift to actionable, policy-driven monitoring and code verification.

- [Observability and Security: Evolving DevOps Across Cloud-Native Environments](https://devops.com/observability-is-the-next-frontier-of-devops-and-cloud-security/)
- [MyDecisive Open Sources Smart Telemetry Hub for OpenTelemetry Data Processing](https://devops.com/mydecisive-open-sources-platform-for-processing-opentelemetry-data/)
- [Deterministic Guardrails for AI-Generated Code: Why Observability and Smarter Linters Matter](https://devops.com/the-deterministic-future-of-ai-generated-code/)

### DevOps for Data, GenAI, and MLOps

Coverage includes GenAI hackathons, the use of MLflow and Kubeflow, and observability across MLOps pipelines—a continuation of focus on explainability and security in enterprise automation.

- [DevOps for GenAI Toronto Hackathon: Lifecycle Automation, MLOps, and Enterprise AI Security](https://devops.com/devops-for-genai-toronto-edition-hackathon-unlocking-new-ai-market-opportunities/)

### Azure DevOps Integrations and Outage Readiness

New Azure DevOps integrations with Jira Service Management create connected, transparent lifecycle management, while coverage of outage response emphasizes best practices for reliability.

- [Integrating Azure DevOps with Jira Service Management: Practical Approaches and Real-World Scenarios](https://techcommunity.microsoft.com/t5/azure/integrating-azure-devops-with-jira-service-management-real-world/m-p/4471605#M22340)
- [Anatomy of an Outage: Evolving Transparency in Microsoft Engineering Teams](/azure/videos/anatomy-of-an-outage-evolving-transparency-in-microsoft-engineering-teams)

### Other DevOps News

Golazo, an engineering workflow framework, addresses open team governance and knowledge management issues. Better GitHub license reporting helps with compliance and resource visibility in complex organizations.

- [Introducing Golazo: Open-Source Framework for Transparent Engineering Teams](https://techcommunity.microsoft.com/t5/azure-compute-blog/golazo-a-framework-for-streamlined-engineering/ba-p/4471142)
- [Improved Enterprise License Consumption Reporting for Outside Collaborators](https://github.blog/changelog/2025-11-17-improved-enterprise-license-consumption-reporting-for-outside-collaborators-now-generally-available)',
     excerpt      = 'This collection covers practical automation in DevOps workflows, improvements to build and release processes, updates for GitHub Actions and VS Code, and enhancements around governance and migration support.',
     content_hash = md5('This collection covers practical automation in DevOps workflows, improvements to build and release processes, updates for GitHub Actions and VS Code, and enhancements around governance and migration support.

<!--excerpt_end-->

## DevOps

### GitHub and GitHub Actions: Migrations, Workflow Enhancements, and Platform Governance

GitHub continues supporting enterprise migrations with features like GitHub-owned blob storage, reducing setup complexity. New controls for managing App installations give organizations more say in integration security. Public preview updates to Pull Request “Files changed” help developers review large codebases. The Actions cache size limit increase expands support for larger monorepos and dependency sets.

- [Migrating Repositories with GitHub-Owned Blob Storage on GitHub Enterprise Cloud](https://github.blog/changelog/2025-11-17-migrating-repositories-with-github-owned-blob-storage-is-now-generally-available)
- [Controlling GitHub App Installations by Organization Owners](https://github.blog/changelog/2025-11-17-block-repository-administrators-from-installing-github-apps-on-their-own-now-in-public-preview)
- [Pull Request “Files Changed” Public Preview: November 20 Updates](https://github.blog/changelog/2025-11-20-pull-request-files-changed-public-preview-november-20-updates)
- [Expanded GitHub Actions Cache Limits Exceed 10 GB per Repository](https://github.blog/changelog/2025-11-20-github-actions-cache-size-can-now-exceed-10-gb-per-repository)

### CI/CD Automation, Migration, and Unified Build Approaches

New guides detail CI/CD automation in Microsoft Fabric and how to unify .NET build processes, streamlining deployment using virtual monorepos. Stories about CVS Health’s migration to GitHub Actions and guidance for migrating from Azure DevOps offer practical insight for teams moving to agent-based DevOps setups.

- [Automating Microsoft Fabric Deployments with Azure DevOps and Python](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/from-code-to-cloud-python-driven-microsoft-fabric-deployments/ba-p/4470447)
- [Reinventing .NET Build and Release: Unified Build Approach](https://devblogs.microsoft.com/dotnet/reinventing-how-dotnet-builds-and-ships-again/)
- [CVS Health’s Migration: Transforming Developer Experience with GitHub Actions](/devops/videos/cvs-healths-migration-transforming-developer-experience-with-github-actions)
- [Azure DevOps to GitHub Migration Playbook: A Step-by-Step Guide for Agentic DevOps](https://devblogs.microsoft.com/all-things-azure/azure-devops-to-github-migration-playbook-unlocking-agentic-devops/)

### Visual Studio Code: Private Marketplace and IT Governance

The VS Code Private Marketplace provides better governance for organizations. Sessions on deployment and AI oversight reinforce responsible adoption and management, echoing previous efforts to streamline onboarding while maintaining control.

- [VS Code Private Marketplace: Enterprise Control Meets Developer Speed](/azure/videos/vs-code-private-marketplace-enterprise-control-meets-developer-speed)
- [Visual Studio Code Deployment and AI Governance for IT Pros](/ai/videos/visual-studio-code-deployment-and-ai-governance-for-it-pros)

### Observability, Monitoring, and Security in DevOps Pipelines

Updated observability tools focus on proactive monitoring, with dashboards that help teams quickly identify incidents. MyDecisive’s Smart Telemetry Hub for Kubernetes and insights on deterministic guardrails reinforce a shift to actionable, policy-driven monitoring and code verification.

- [Observability and Security: Evolving DevOps Across Cloud-Native Environments](https://devops.com/observability-is-the-next-frontier-of-devops-and-cloud-security/)
- [MyDecisive Open Sources Smart Telemetry Hub for OpenTelemetry Data Processing](https://devops.com/mydecisive-open-sources-platform-for-processing-opentelemetry-data/)
- [Deterministic Guardrails for AI-Generated Code: Why Observability and Smarter Linters Matter](https://devops.com/the-deterministic-future-of-ai-generated-code/)

### DevOps for Data, GenAI, and MLOps

Coverage includes GenAI hackathons, the use of MLflow and Kubeflow, and observability across MLOps pipelines—a continuation of focus on explainability and security in enterprise automation.

- [DevOps for GenAI Toronto Hackathon: Lifecycle Automation, MLOps, and Enterprise AI Security](https://devops.com/devops-for-genai-toronto-edition-hackathon-unlocking-new-ai-market-opportunities/)

### Azure DevOps Integrations and Outage Readiness

New Azure DevOps integrations with Jira Service Management create connected, transparent lifecycle management, while coverage of outage response emphasizes best practices for reliability.

- [Integrating Azure DevOps with Jira Service Management: Practical Approaches and Real-World Scenarios](https://techcommunity.microsoft.com/t5/azure/integrating-azure-devops-with-jira-service-management-real-world/m-p/4471605#M22340)
- [Anatomy of an Outage: Evolving Transparency in Microsoft Engineering Teams](/azure/videos/anatomy-of-an-outage-evolving-transparency-in-microsoft-engineering-teams)

### Other DevOps News

Golazo, an engineering workflow framework, addresses open team governance and knowledge management issues. Better GitHub license reporting helps with compliance and resource visibility in complex organizations.

- [Introducing Golazo: Open-Source Framework for Transparent Engineering Teams](https://techcommunity.microsoft.com/t5/azure-compute-blog/golazo-a-framework-for-streamlined-engineering/ba-p/4471142)
- [Improved Enterprise License Consumption Reporting for Outside Collaborators](https://github.blog/changelog/2025-11-17-improved-enterprise-license-consumption-reporting-for-outside-collaborators-now-generally-available)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-11-24';

-- weekly-devops-roundup-2025-12-01  (2025-12-01)
UPDATE content_items
SET  content      = 'DevOps continues to enhance configuration management and automated setup for Windows, building on last week’s emphasis on code-based infrastructure for smoother developer workflows. The goal remains consistency, simple onboarding, and flexible extension across environments.

<!--excerpt_end-->

## DevOps

### WinGet and Desired State Configuration Integration

This week’s milestone announcement is the integration of Desired State Configuration (DSC) with WinGet, featuring code-driven setup for Windows machines. Teams can now automate app configuration and policy enforcement, reducing repetitive tasks and ensuring consistent environments for developers and production teams. This matches the trend toward policy-driven automation described before.

Guides now show how to export and reuse configuration templates, making standard set-up easy. The updates fit with ongoing patterns for onboarding, compliance, and policy-driven automation now built directly into Windows.

Additional improvements include interface updates, advanced font handling, new command line features, and PowerShell integration. WinGet Studio, an open community portal, now assists with plugin sharing and customization—expanding community input and evolution found in previous DevOps news.

These updates provide faster onboarding and reliable setup for both development and IT teams, strengthening the open, adaptable nature of modern DevOps on Windows.

- [Fast and Easy Windows Setup & Configuration with WinGet and Desired State Configuration](/coding/videos/fast-and-easy-windows-setup-and-configuration-with-winget-and-desired-state-configuration)',
     excerpt      = 'DevOps continues to enhance configuration management and automated setup for Windows, building on last week’s emphasis on code-based infrastructure for smoother developer workflows. The goal remains consistency, simple onboarding, and flexible extension across environments.',
     content_hash = md5('DevOps continues to enhance configuration management and automated setup for Windows, building on last week’s emphasis on code-based infrastructure for smoother developer workflows. The goal remains consistency, simple onboarding, and flexible extension across environments.

<!--excerpt_end-->

## DevOps

### WinGet and Desired State Configuration Integration

This week’s milestone announcement is the integration of Desired State Configuration (DSC) with WinGet, featuring code-driven setup for Windows machines. Teams can now automate app configuration and policy enforcement, reducing repetitive tasks and ensuring consistent environments for developers and production teams. This matches the trend toward policy-driven automation described before.

Guides now show how to export and reuse configuration templates, making standard set-up easy. The updates fit with ongoing patterns for onboarding, compliance, and policy-driven automation now built directly into Windows.

Additional improvements include interface updates, advanced font handling, new command line features, and PowerShell integration. WinGet Studio, an open community portal, now assists with plugin sharing and customization—expanding community input and evolution found in previous DevOps news.

These updates provide faster onboarding and reliable setup for both development and IT teams, strengthening the open, adaptable nature of modern DevOps on Windows.

- [Fast and Easy Windows Setup & Configuration with WinGet and Desired State Configuration](/coding/videos/fast-and-easy-windows-setup-and-configuration-with-winget-and-desired-state-configuration)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-12-01';

-- weekly-devops-roundup-2025-12-08  (2025-12-08)
UPDATE content_items
SET  content      = 'This week features new governance controls, automation tools, and insights on AI-driven delivery practices.

<!--excerpt_end-->

## DevOps

### GitHub Platform Updates for Governance, Security, and Automation

GitHub Enterprise Server 3.19 Release Candidate brings added controls, better repository setup and automation, extra rules enforcement, and improved CI/CD. GitHub Actions updates allow blocking individual workflows and enforcing SHA pinning. Organization owners can now manage app installations directly, improving security and simplicity. Workflows now support up to 25 inputs, increasing CI/CD pipeline flexibility.

- [GitHub Enterprise Server 3.19: New Release Candidate Features and Enhancements](https://github.blog/changelog/2025-12-02-github-enterprise-server-3-19-release-candidate-is-now-available)
- [GitHub Actions workflow_dispatch Now Supports 25 Inputs](https://github.blog/changelog/2025-12-04-actions-workflow-dispatch-workflows-now-support-25-inputs)
- [GitHub Organization Owners Gain Control Over App Installations](https://github.blog/changelog/2025-12-01-block-repository-admins-from-installing-github-apps-now-generally-available)

### Improving Code Health and Automation with GitHub Code Quality

GitHub Code Quality pulls together automated code insights, supporting maintainable code and fast feedback as part of the standard DevOps pipeline.

- [How to Improve Code Health with GitHub Code Quality](/devops/videos/how-to-improve-code-health-with-github-code-quality)

### DevOps Practices and the AI-Driven Evolution of Software Delivery

A retrospective discusses the history and ongoing evolution of DevOps—tracing early software challenges to today’s use of policy-driven and intelligent automation.

- [The Software Crisis Never Ended, It Just Evolved](https://roadtoalm.com/2025/12/01/the-software-crisis-never-ended-it-just-evolved/)',
     excerpt      = 'This week features new governance controls, automation tools, and insights on AI-driven delivery practices.',
     content_hash = md5('This week features new governance controls, automation tools, and insights on AI-driven delivery practices.

<!--excerpt_end-->

## DevOps

### GitHub Platform Updates for Governance, Security, and Automation

GitHub Enterprise Server 3.19 Release Candidate brings added controls, better repository setup and automation, extra rules enforcement, and improved CI/CD. GitHub Actions updates allow blocking individual workflows and enforcing SHA pinning. Organization owners can now manage app installations directly, improving security and simplicity. Workflows now support up to 25 inputs, increasing CI/CD pipeline flexibility.

- [GitHub Enterprise Server 3.19: New Release Candidate Features and Enhancements](https://github.blog/changelog/2025-12-02-github-enterprise-server-3-19-release-candidate-is-now-available)
- [GitHub Actions workflow_dispatch Now Supports 25 Inputs](https://github.blog/changelog/2025-12-04-actions-workflow-dispatch-workflows-now-support-25-inputs)
- [GitHub Organization Owners Gain Control Over App Installations](https://github.blog/changelog/2025-12-01-block-repository-admins-from-installing-github-apps-now-generally-available)

### Improving Code Health and Automation with GitHub Code Quality

GitHub Code Quality pulls together automated code insights, supporting maintainable code and fast feedback as part of the standard DevOps pipeline.

- [How to Improve Code Health with GitHub Code Quality](/devops/videos/how-to-improve-code-health-with-github-code-quality)

### DevOps Practices and the AI-Driven Evolution of Software Delivery

A retrospective discusses the history and ongoing evolution of DevOps—tracing early software challenges to today’s use of policy-driven and intelligent automation.

- [The Software Crisis Never Ended, It Just Evolved](https://roadtoalm.com/2025/12/01/the-software-crisis-never-ended-it-just-evolved/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-12-08';

-- weekly-devops-roundup-2025-12-15  (2025-12-15)
UPDATE content_items
SET  content      = 'DevOps updates this week emphasize secure and scalable environments with new tools from GitHub, Azure, and Visual Studio Code to manage complex automation and infrastructure for large teams.

<!--excerpt_end-->

## DevOps

### GitHub Platform and Workflow Enhancements

GitHub Enterprise Server 3.19 is out, offering new repository metadata options, reusable ruleset templates, SHA pinning for Actions, and SSH/TLS management to address governance and compliance. OpenTelemetry integration and improved admin tools are available, including a central repository dashboard, updated PR reviews, and performance improvements for enterprise teams. Supply chain security for Go projects is advanced with improved dependency graphs and expanded API support.

Admins also have new capabilities for official communication with ‘Post as Admin’ in GitHub Discussions.

- [GitHub Enterprise Server 3.19 Release Highlights](https://github.blog/changelog/2025-12-10-github-enterprise-server-3-19-is-now-generally-available)
- [Repository Dashboard Preview: Centralized Search, Filtering, and Saved Queries on GitHub](https://github.blog/changelog/2025-12-11-repository-dashboard-find-search-and-save-queries-in-preview)
- [Commit-by-Commit Review and Enhanced Filtering in GitHub Pull Request Files Changed Experience](https://github.blog/changelog/2025-12-11-review-commit-by-commit-improved-filtering-and-more-in-the-pull-request-files-changed-public-preview)
- [Enterprise Teams Product Limits Increased by Over 10x](https://github.blog/changelog/2025-12-08-enterprise-teams-product-limits-increased-by-over-10x)
- [Dependabot-Based Dependency Graphs Enhance Supply Chain Security for Go Projects](https://github.blog/changelog/2025-12-09-dependabot-dgs-for-go)
- [GitHub Custom Repository Properties: GraphQL API and URL Type Enhancements](https://github.blog/changelog/2025-12-09-repository-custom-properties-graphql-api-and-url-type)
- [Post as Admin Feature in GitHub Discussions](https://github.blog/changelog/2025-12-11-post-as-admin-now-available-in-github-discussions)

### GitHub Actions Evolution and Runner Management

GitHub Actions is now capable of handling up to 71 million jobs daily, following architectural changes and better scheduling. YAML anchors, nested workflows, and larger caches make CI/CD processes more flexible. All self-hosted runners must be upgraded to v2.329.0 by January 2026 to remain supported, reflecting the new security guidelines.

For Azure VNET-injected runners, network diagnostics are improved. Forthcoming features expand flexibility around job scheduling and parallel execution.

- [How GitHub Actions Evolved: Architecture, Key Upgrades & What’s Next](https://github.blog/news-insights/product-news/lets-talk-about-github-actions/)
- [Improved Network Diagnostics and Required Self-Hosted Runner Upgrades for GitHub Actions with Azure VNET Injection](https://github.blog/changelog/2025-12-12-better-diagnostics-for-vnet-injected-runners-and-required-self-hosted-runner-upgrades)

### Azure DevOps Server and Enterprise Migration

Azure DevOps Server is generally available, maintaining support for companies with self-managed environments. Migration from legacy TFS systems is also facilitated, and real-world stories provide guidance for phased migrations and enterprise upgrades.

- [Azure DevOps Server Now Generally Available for Self-Hosted Enterprises](https://devblogs.microsoft.com/devops/announcing-azure-devops-server-general-availability/)
- [Applying DevOps Principles on Lean Infrastructure: Lessons from Scaling to 102,000 Users and Planning Azure Migration](https://techcommunity.microsoft.com/t5/azure/applying-devops-principles-on-lean-infrastructure-lessons-from/m-p/4476015#M22362)

### Cloud-Native DevOps with Azure MCP and Agent Management

Recent posts and guides highlight best practices for orchestrating pipelines with .NET, Visual Studio, Azure MCP, SQL, and Azure Storage. Visual Studio Code’s new unified agent tools make it easier for developers to oversee multiple agents and improve orchestration, consistent with earlier updates to cloud-native automation.

- [Agentic DevOps: Enhancing .NET Web Apps with Azure MCP](/ai/videos/agentic-devops-enhancing-net-web-apps-with-azure-mcp)
- [A Unified Agent Experience in Visual Studio Code](/azure/videos/a-unified-agent-experience-in-visual-studio-code)',
     excerpt      = 'DevOps updates this week emphasize secure and scalable environments with new tools from GitHub, Azure, and Visual Studio Code to manage complex automation and infrastructure for large teams.',
     content_hash = md5('DevOps updates this week emphasize secure and scalable environments with new tools from GitHub, Azure, and Visual Studio Code to manage complex automation and infrastructure for large teams.

<!--excerpt_end-->

## DevOps

### GitHub Platform and Workflow Enhancements

GitHub Enterprise Server 3.19 is out, offering new repository metadata options, reusable ruleset templates, SHA pinning for Actions, and SSH/TLS management to address governance and compliance. OpenTelemetry integration and improved admin tools are available, including a central repository dashboard, updated PR reviews, and performance improvements for enterprise teams. Supply chain security for Go projects is advanced with improved dependency graphs and expanded API support.

Admins also have new capabilities for official communication with ‘Post as Admin’ in GitHub Discussions.

- [GitHub Enterprise Server 3.19 Release Highlights](https://github.blog/changelog/2025-12-10-github-enterprise-server-3-19-is-now-generally-available)
- [Repository Dashboard Preview: Centralized Search, Filtering, and Saved Queries on GitHub](https://github.blog/changelog/2025-12-11-repository-dashboard-find-search-and-save-queries-in-preview)
- [Commit-by-Commit Review and Enhanced Filtering in GitHub Pull Request Files Changed Experience](https://github.blog/changelog/2025-12-11-review-commit-by-commit-improved-filtering-and-more-in-the-pull-request-files-changed-public-preview)
- [Enterprise Teams Product Limits Increased by Over 10x](https://github.blog/changelog/2025-12-08-enterprise-teams-product-limits-increased-by-over-10x)
- [Dependabot-Based Dependency Graphs Enhance Supply Chain Security for Go Projects](https://github.blog/changelog/2025-12-09-dependabot-dgs-for-go)
- [GitHub Custom Repository Properties: GraphQL API and URL Type Enhancements](https://github.blog/changelog/2025-12-09-repository-custom-properties-graphql-api-and-url-type)
- [Post as Admin Feature in GitHub Discussions](https://github.blog/changelog/2025-12-11-post-as-admin-now-available-in-github-discussions)

### GitHub Actions Evolution and Runner Management

GitHub Actions is now capable of handling up to 71 million jobs daily, following architectural changes and better scheduling. YAML anchors, nested workflows, and larger caches make CI/CD processes more flexible. All self-hosted runners must be upgraded to v2.329.0 by January 2026 to remain supported, reflecting the new security guidelines.

For Azure VNET-injected runners, network diagnostics are improved. Forthcoming features expand flexibility around job scheduling and parallel execution.

- [How GitHub Actions Evolved: Architecture, Key Upgrades & What’s Next](https://github.blog/news-insights/product-news/lets-talk-about-github-actions/)
- [Improved Network Diagnostics and Required Self-Hosted Runner Upgrades for GitHub Actions with Azure VNET Injection](https://github.blog/changelog/2025-12-12-better-diagnostics-for-vnet-injected-runners-and-required-self-hosted-runner-upgrades)

### Azure DevOps Server and Enterprise Migration

Azure DevOps Server is generally available, maintaining support for companies with self-managed environments. Migration from legacy TFS systems is also facilitated, and real-world stories provide guidance for phased migrations and enterprise upgrades.

- [Azure DevOps Server Now Generally Available for Self-Hosted Enterprises](https://devblogs.microsoft.com/devops/announcing-azure-devops-server-general-availability/)
- [Applying DevOps Principles on Lean Infrastructure: Lessons from Scaling to 102,000 Users and Planning Azure Migration](https://techcommunity.microsoft.com/t5/azure/applying-devops-principles-on-lean-infrastructure-lessons-from/m-p/4476015#M22362)

### Cloud-Native DevOps with Azure MCP and Agent Management

Recent posts and guides highlight best practices for orchestrating pipelines with .NET, Visual Studio, Azure MCP, SQL, and Azure Storage. Visual Studio Code’s new unified agent tools make it easier for developers to oversee multiple agents and improve orchestration, consistent with earlier updates to cloud-native automation.

- [Agentic DevOps: Enhancing .NET Web Apps with Azure MCP](/ai/videos/agentic-devops-enhancing-net-web-apps-with-azure-mcp)
- [A Unified Agent Experience in Visual Studio Code](/azure/videos/a-unified-agent-experience-in-visual-studio-code)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-12-15';

-- weekly-devops-roundup-2025-12-22  (2025-12-22)
UPDATE content_items
SET  content      = 'GitHub Actions will charge for self-hosted runners starting March 2026 ($0.002/min on private repos). Dependabot adds support for Conda, OpenTofu, Julia, and Bazel ecosystems.

<!--excerpt_end-->

## DevOps

### GitHub Actions Self-Hosted Runners: Pricing Changes, Roadmap, and Community Impact

Starting in March 2026, GitHub will charge for self-hosted Actions runners on private repositories ($0.002/min), while maintaining free access on public repositories and Enterprise Server. Lower costs for hosted runners offset the change for most users, but planning and migration resources are central to the transition. Competitive alternatives (e.g., Depot), coming platform support, better workflow metrics, and ongoing documentation updates aim to ease migration.

- [GitHub to Charge for Self-Hosted Runners on GitHub Actions Starting March 2026](https://devclass.com/2025/12/17/github-to-charge-for-self-hosted-runners-from-march-2026/)
- [GitHub Actions Pricing Update: Lower Runner Costs, Platform Enhancements Coming in 2026](https://github.blog/changelog/2025-12-16-coming-soon-simpler-pricing-and-a-better-experience-for-github-actions)

### Dependabot Expands Ecosystem: Conda, OpenTofu, Julia, Bazel Automated Updates

Dependabot now offers automatic updates for Conda (popular with Python/data science), OpenTofu (IaC), Julia (scientific), and Bazel (build systems), broadening security and maintenance coverage. Documentation and troubleshooting resources help support this expanded ecosystem.

- [Dependabot Adds Conda Ecosystem Support for Automated Version Updates](https://github.blog/changelog/2025-12-16-conda-ecosystem-support-for-dependabot-now-generally-available)
- [Dependabot Adds Support for OpenTofu Dependency Updates](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-opentofu)
- [Dependabot Adds Version Update Support for Julia](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-julia)
- [Dependabot Version Updates Now Support Bazel](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-bazel)

### Azure DevOps Productivity: Microsoft.Testing.Platform Integration and Security Workflows

Azure DevOps now fully supports Microsoft.Testing.Platform—streamlining .NET test commands, automatic retries, and publishing of results. Vulnerability management links GitHub Advanced Security alerts to Azure DevOps Boards, streamlining issue triage and resolution.

- [Azure DevOps Gains Full Support for Microsoft.Testing.Platform](https://devblogs.microsoft.com/dotnet/microsoft-testing-platform-azure-retry/)
- [Work Item Linking for GitHub Advanced Security Alerts in Azure DevOps Now Available](https://devblogs.microsoft.com/devops/work-item-linking-for-advanced-security-alerts-now-available/)

### Microsoft Fabric and Azure DevOps: CI/CD Automation and Integration

Microsoft Fabric now features direct guides for automating SQL database deployment with Azure DevOps. Secure connection support (service principal, OAuth 2.0) enables better CI/CD integration and supports complex automation scenarios across cloud environments.

- [Performing CI/CD for SQL Databases in Fabric Using Azure DevOps](/azure/videos/performing-cicd-for-sql-databases-in-fabric-using-azure-devops)
- [How to Connect Microsoft Fabric to Azure DevOps Using Service Principal](https://blog.fabric.microsoft.com/en-US/blog/how-to-connect-microsoft-fabric-to-azure-devops-using-service-principal/)

### Other DevOps News

GitHub Teams administration is now consolidated under the ''Settings → Teams'' menu, promoting easier management. The Azure SRE Agent automates incident response by running playbooks and collecting evidence when triggered by alerts in PagerDuty, ServiceNow, or Azure Monitor—removing the need for manual intervention in multi-cloud environments.

- [Teams Management Relocated to Settings in GitHub](https://github.blog/changelog/2025-12-18-teams-management-now-moved-to-settings)
- [Automating On-Call Runbooks with Azure SRE Agent for Incident Response](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/stop-running-runbooks-at-3-am-let-azure-sre-agent-do-your-on/ba-p/4479811)',
     excerpt      = 'GitHub Actions will charge for self-hosted runners starting March 2026 ($0.002/min on private repos). Dependabot adds support for Conda, OpenTofu, Julia, and Bazel ecosystems.',
     content_hash = md5('GitHub Actions will charge for self-hosted runners starting March 2026 ($0.002/min on private repos). Dependabot adds support for Conda, OpenTofu, Julia, and Bazel ecosystems.

<!--excerpt_end-->

## DevOps

### GitHub Actions Self-Hosted Runners: Pricing Changes, Roadmap, and Community Impact

Starting in March 2026, GitHub will charge for self-hosted Actions runners on private repositories ($0.002/min), while maintaining free access on public repositories and Enterprise Server. Lower costs for hosted runners offset the change for most users, but planning and migration resources are central to the transition. Competitive alternatives (e.g., Depot), coming platform support, better workflow metrics, and ongoing documentation updates aim to ease migration.

- [GitHub to Charge for Self-Hosted Runners on GitHub Actions Starting March 2026](https://devclass.com/2025/12/17/github-to-charge-for-self-hosted-runners-from-march-2026/)
- [GitHub Actions Pricing Update: Lower Runner Costs, Platform Enhancements Coming in 2026](https://github.blog/changelog/2025-12-16-coming-soon-simpler-pricing-and-a-better-experience-for-github-actions)

### Dependabot Expands Ecosystem: Conda, OpenTofu, Julia, Bazel Automated Updates

Dependabot now offers automatic updates for Conda (popular with Python/data science), OpenTofu (IaC), Julia (scientific), and Bazel (build systems), broadening security and maintenance coverage. Documentation and troubleshooting resources help support this expanded ecosystem.

- [Dependabot Adds Conda Ecosystem Support for Automated Version Updates](https://github.blog/changelog/2025-12-16-conda-ecosystem-support-for-dependabot-now-generally-available)
- [Dependabot Adds Support for OpenTofu Dependency Updates](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-opentofu)
- [Dependabot Adds Version Update Support for Julia](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-julia)
- [Dependabot Version Updates Now Support Bazel](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-bazel)

### Azure DevOps Productivity: Microsoft.Testing.Platform Integration and Security Workflows

Azure DevOps now fully supports Microsoft.Testing.Platform—streamlining .NET test commands, automatic retries, and publishing of results. Vulnerability management links GitHub Advanced Security alerts to Azure DevOps Boards, streamlining issue triage and resolution.

- [Azure DevOps Gains Full Support for Microsoft.Testing.Platform](https://devblogs.microsoft.com/dotnet/microsoft-testing-platform-azure-retry/)
- [Work Item Linking for GitHub Advanced Security Alerts in Azure DevOps Now Available](https://devblogs.microsoft.com/devops/work-item-linking-for-advanced-security-alerts-now-available/)

### Microsoft Fabric and Azure DevOps: CI/CD Automation and Integration

Microsoft Fabric now features direct guides for automating SQL database deployment with Azure DevOps. Secure connection support (service principal, OAuth 2.0) enables better CI/CD integration and supports complex automation scenarios across cloud environments.

- [Performing CI/CD for SQL Databases in Fabric Using Azure DevOps](/azure/videos/performing-cicd-for-sql-databases-in-fabric-using-azure-devops)
- [How to Connect Microsoft Fabric to Azure DevOps Using Service Principal](https://blog.fabric.microsoft.com/en-US/blog/how-to-connect-microsoft-fabric-to-azure-devops-using-service-principal/)

### Other DevOps News

GitHub Teams administration is now consolidated under the ''Settings → Teams'' menu, promoting easier management. The Azure SRE Agent automates incident response by running playbooks and collecting evidence when triggered by alerts in PagerDuty, ServiceNow, or Azure Monitor—removing the need for manual intervention in multi-cloud environments.

- [Teams Management Relocated to Settings in GitHub](https://github.blog/changelog/2025-12-18-teams-management-now-moved-to-settings)
- [Automating On-Call Runbooks with Azure SRE Agent for Incident Response](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/stop-running-runbooks-at-3-am-let-azure-sre-agent-do-your-on/ba-p/4479811)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-12-22';

-- weekly-devops-roundup-2025-12-29  (2025-12-29)
UPDATE content_items
SET  content      = 'This week’s DevOps section spotlights automation enhancements and workflow updates across Azure, GitHub, and CI/CD environments. New processes for triage, app management, and testing help teams work more efficiently, while additional security improvements reinforce safe, reliable automation at scale.

<!--excerpt_end-->

## DevOps

### Azure SRE Agent Automation and Advanced DevOps Workflows

Expanding on last week’s SRE Agent automation, new instructions provide guidance for integrating the agent with the Model Context Protocol (MCP). Beyond automated runbook handling, examples show how to use MCP for complex tasks like automated customer issue triage. Developers can set up SRE Agent to work with GitHub and PagerDuty APIs, automate ticket scanning, classify issues with markdown, and auto-assign urgent cases to PagerDuty. Subagents are given clear, limited permissions, matching coverage of secure, flexible incident automation from last week’s updates. MCP adapters extend the platform for larger-scale process automation with lower friction.

- [Automating Customer Issue Triage with Azure SRE Agent and MCP](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/extend-sre-agent-with-mcp-build-an-agentic-workflow-to-triage/ba-p/4480710)

### GitHub Actions Updates: Workflow Productivity and Security

GitHub Actions announced changes that enhance productivity and reliability, complementing last week''s coverage of pricing and ecosystem changes. The workflows page now loads faster for repositories with many jobs, reducing timeouts and rendering issues in large pipelines. New job status filters make navigation more manageable, addressing prior requests from the community. Jesse Houwing’s “actions-dependency-submission” custom action improves security by allowing Dependency Graph and Dependabot to function properly even with SHA-pinned or forked actions, supporting recent advances in automating and securing CI/CD. These incremental updates move Actions toward greater transparency and reliability for DevOps teams.

- [Improved Performance for GitHub Actions Workflows Page](https://github.blog/changelog/2025-12-22-improved-performance-for-github-actions-workflows-page)
- [Improved Dependency Submission for GitHub Actions](https://jessehouwing.net/github-actions-improved-dependency-submission/)

### GitHub Governance: Granular App Access Controls Now in Preview

GitHub has started a public preview for improved app request controls, expanding options for organization admins to restrict which members can request installation of GitHub/OAuth apps. These controls can be set for all members, a specific group, or disabled entirely. This change addresses compliance needs, reduces third-party risk, and continues the focus on operational security controls for larger organizations discussed last week.

- [Granular Controls for GitHub App Requests Now in Public Preview](https://github.blog/changelog/2025-12-22-control-who-can-request-apps-for-your-organization)

### Azure DevOps: Test Run Hub Reaches General Availability

The new Test Run Hub in Azure DevOps is now available for all users, providing a single location for manual and automated test runs. Integration with the Azure DevOps REST API supports automation, while new filters (for outcome, priority, and failure type) and improved artifact sharing with markdown support make collaboration simpler. Stronger search and tracking features connect test results to work items, addressing feedback from the developer community. Organizations will transition to the Test Run Hub starting January 2026, retiring legacy test management methods.

- [The New Test Run Hub in Azure Test Plans Reaches General Availability](https://devblogs.microsoft.com/devops/the-new-test-run-hub-is-going-generally-available/)',
     excerpt      = 'This week’s DevOps section spotlights automation enhancements and workflow updates across Azure, GitHub, and CI/CD environments. New processes for triage, app management, and testing help teams work more efficiently, while additional security improvements reinforce safe, reliable automation at scale.',
     content_hash = md5('This week’s DevOps section spotlights automation enhancements and workflow updates across Azure, GitHub, and CI/CD environments. New processes for triage, app management, and testing help teams work more efficiently, while additional security improvements reinforce safe, reliable automation at scale.

<!--excerpt_end-->

## DevOps

### Azure SRE Agent Automation and Advanced DevOps Workflows

Expanding on last week’s SRE Agent automation, new instructions provide guidance for integrating the agent with the Model Context Protocol (MCP). Beyond automated runbook handling, examples show how to use MCP for complex tasks like automated customer issue triage. Developers can set up SRE Agent to work with GitHub and PagerDuty APIs, automate ticket scanning, classify issues with markdown, and auto-assign urgent cases to PagerDuty. Subagents are given clear, limited permissions, matching coverage of secure, flexible incident automation from last week’s updates. MCP adapters extend the platform for larger-scale process automation with lower friction.

- [Automating Customer Issue Triage with Azure SRE Agent and MCP](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/extend-sre-agent-with-mcp-build-an-agentic-workflow-to-triage/ba-p/4480710)

### GitHub Actions Updates: Workflow Productivity and Security

GitHub Actions announced changes that enhance productivity and reliability, complementing last week''s coverage of pricing and ecosystem changes. The workflows page now loads faster for repositories with many jobs, reducing timeouts and rendering issues in large pipelines. New job status filters make navigation more manageable, addressing prior requests from the community. Jesse Houwing’s “actions-dependency-submission” custom action improves security by allowing Dependency Graph and Dependabot to function properly even with SHA-pinned or forked actions, supporting recent advances in automating and securing CI/CD. These incremental updates move Actions toward greater transparency and reliability for DevOps teams.

- [Improved Performance for GitHub Actions Workflows Page](https://github.blog/changelog/2025-12-22-improved-performance-for-github-actions-workflows-page)
- [Improved Dependency Submission for GitHub Actions](https://jessehouwing.net/github-actions-improved-dependency-submission/)

### GitHub Governance: Granular App Access Controls Now in Preview

GitHub has started a public preview for improved app request controls, expanding options for organization admins to restrict which members can request installation of GitHub/OAuth apps. These controls can be set for all members, a specific group, or disabled entirely. This change addresses compliance needs, reduces third-party risk, and continues the focus on operational security controls for larger organizations discussed last week.

- [Granular Controls for GitHub App Requests Now in Public Preview](https://github.blog/changelog/2025-12-22-control-who-can-request-apps-for-your-organization)

### Azure DevOps: Test Run Hub Reaches General Availability

The new Test Run Hub in Azure DevOps is now available for all users, providing a single location for manual and automated test runs. Integration with the Azure DevOps REST API supports automation, while new filters (for outcome, priority, and failure type) and improved artifact sharing with markdown support make collaboration simpler. Stronger search and tracking features connect test results to work items, addressing feedback from the developer community. Organizations will transition to the Test Run Hub starting January 2026, retiring legacy test management methods.

- [The New Test Run Hub in Azure Test Plans Reaches General Availability](https://devblogs.microsoft.com/devops/the-new-test-run-hub-is-going-generally-available/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2025-12-29';

-- weekly-devops-roundup-2026-01-05  (2026-01-05)
UPDATE content_items
SET  content      = 'This week in DevOps, Azure added tools for AI-guided debugging and repair, while GitHub made it more affordable to run hosted CI/CD runners. Both changes support smoother operations and accessibility in cloud engineering.

<!--excerpt_end-->

## DevOps

### AI-Powered Debugging and Remediation with Azure SRE Agent

Extending coverage of SRE Agent automation and incident workflows powered by MCP, the Azure SRE Agent now includes developer-focused support for debugging and automated remediation. This week’s workflow goes beyond detection, connecting infrastructure monitoring and health analysis to agent-driven fixes.

The latest advances combine VS Code Copilot and Claude Opus for more effective debugging. The GitHub Coding Agent structures pull requests for all types of changes, unifying app, infrastructure, and SQL repairs. This parallels recent articles arguing for source-controlled change automation as a safer choice than ad-hoc fixes in cloud portals.

The automation tools benefit from improvements in orchestration and permission models introduced earlier, and reinforce a shift toward open source agent engagement. Developers are encouraged to contribute and test agent features as the model for custom MCP adapters and integrated alerts expands.

- [How SRE Agent Closes the Developer Loop: Debugging and Fixing Azure Cloud App Failures with AI](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-vibe-coding-to-working-app-how-sre-agent-completes-the/ba-p/4482000)

### Lower Costs for GitHub-Hosted CI/CD Runners

GitHub Actions pricing for hosted runners has been reduced as of December 16, 2025, supporting the continued push for easy and affordable CI/CD pipelines. Private repository users can now automate builds and releases at a lower cost, with public repositories retaining free runner access. New calculators help teams predict costs and plan for automation at different scales.

This change follows last week’s news about workflow reliability and aligns with the ongoing goal of making GitHub Actions practical and accessible for development teams. Cost savings should help further automate builds and expand testing in private repos, fitting the recurring story of broader CI/CD adoption.

- [Reduced Pricing for GitHub-Hosted Runners Usage](https://github.blog/changelog/2026-01-01-reduced-pricing-for-github-hosted-runners-usage)',
     excerpt      = 'This week in DevOps, Azure added tools for AI-guided debugging and repair, while GitHub made it more affordable to run hosted CI/CD runners. Both changes support smoother operations and accessibility in cloud engineering.',
     content_hash = md5('This week in DevOps, Azure added tools for AI-guided debugging and repair, while GitHub made it more affordable to run hosted CI/CD runners. Both changes support smoother operations and accessibility in cloud engineering.

<!--excerpt_end-->

## DevOps

### AI-Powered Debugging and Remediation with Azure SRE Agent

Extending coverage of SRE Agent automation and incident workflows powered by MCP, the Azure SRE Agent now includes developer-focused support for debugging and automated remediation. This week’s workflow goes beyond detection, connecting infrastructure monitoring and health analysis to agent-driven fixes.

The latest advances combine VS Code Copilot and Claude Opus for more effective debugging. The GitHub Coding Agent structures pull requests for all types of changes, unifying app, infrastructure, and SQL repairs. This parallels recent articles arguing for source-controlled change automation as a safer choice than ad-hoc fixes in cloud portals.

The automation tools benefit from improvements in orchestration and permission models introduced earlier, and reinforce a shift toward open source agent engagement. Developers are encouraged to contribute and test agent features as the model for custom MCP adapters and integrated alerts expands.

- [How SRE Agent Closes the Developer Loop: Debugging and Fixing Azure Cloud App Failures with AI](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-vibe-coding-to-working-app-how-sre-agent-completes-the/ba-p/4482000)

### Lower Costs for GitHub-Hosted CI/CD Runners

GitHub Actions pricing for hosted runners has been reduced as of December 16, 2025, supporting the continued push for easy and affordable CI/CD pipelines. Private repository users can now automate builds and releases at a lower cost, with public repositories retaining free runner access. New calculators help teams predict costs and plan for automation at different scales.

This change follows last week’s news about workflow reliability and aligns with the ongoing goal of making GitHub Actions practical and accessible for development teams. Cost savings should help further automate builds and expand testing in private repos, fitting the recurring story of broader CI/CD adoption.

- [Reduced Pricing for GitHub-Hosted Runners Usage](https://github.blog/changelog/2026-01-01-reduced-pricing-for-github-hosted-runners-usage)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-01-05';

-- weekly-devops-roundup-2026-01-12  (2026-01-12)
UPDATE content_items
SET  content      = 'DevOps topics this week highlight monitoring complexity in cloud-native systems and the adoption of automated detection for missed alerts, supporting more reliable production engineering.

<!--excerpt_end-->

## DevOps

### Automated Detection of Alert Gaps in Azure Kubernetes Deployments

A new case study addresses alerting gaps in AKS deployments. Last week’s SRE Agent worked on debugging and remediation with MCP; now it expands to audit for missed alert coverage.

The report analyzes a Redis credential rotation incident where essential failures went unreported due to narrow alert criteria focused on resource usage. Blocked Redis connections caused outages not evident in current alerting rules.

Deploying the Azure SRE Agent with GitHub MCP improves alert coverage by discovering missing synthetic probes, weak ingress alerts, and absent pod-specific checks. The agent, using broad permissions, correlates AKS diagnostics, Log Analytics, and infrastructure code.

Building on previous AI-driven incident response, SRE Agent now suggests improved KQL and Bicep configurations and automatically creates actionable GitHub issues.

Subagent setup instructions and directions for Log Analytics/CLI integration are provided for teams looking to strengthen monitoring and resilience.

- [Identifying Missed Alerts in Azure Kubernetes Deployments with SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/find-the-alerts-you-didn-t-know-you-were-missing-with-azure-sre/ba-p/4483494)',
     excerpt      = 'DevOps topics this week highlight monitoring complexity in cloud-native systems and the adoption of automated detection for missed alerts, supporting more reliable production engineering.',
     content_hash = md5('DevOps topics this week highlight monitoring complexity in cloud-native systems and the adoption of automated detection for missed alerts, supporting more reliable production engineering.

<!--excerpt_end-->

## DevOps

### Automated Detection of Alert Gaps in Azure Kubernetes Deployments

A new case study addresses alerting gaps in AKS deployments. Last week’s SRE Agent worked on debugging and remediation with MCP; now it expands to audit for missed alert coverage.

The report analyzes a Redis credential rotation incident where essential failures went unreported due to narrow alert criteria focused on resource usage. Blocked Redis connections caused outages not evident in current alerting rules.

Deploying the Azure SRE Agent with GitHub MCP improves alert coverage by discovering missing synthetic probes, weak ingress alerts, and absent pod-specific checks. The agent, using broad permissions, correlates AKS diagnostics, Log Analytics, and infrastructure code.

Building on previous AI-driven incident response, SRE Agent now suggests improved KQL and Bicep configurations and automatically creates actionable GitHub issues.

Subagent setup instructions and directions for Log Analytics/CLI integration are provided for teams looking to strengthen monitoring and resilience.

- [Identifying Missed Alerts in Azure Kubernetes Deployments with SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/find-the-alerts-you-didn-t-know-you-were-missing-with-azure-sre/ba-p/4483494)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-01-12';

-- weekly-devops-roundup-2026-01-19  (2026-01-19)
UPDATE content_items
SET  content      = 'DevOps improvements this week introduce new GitHub and .NET features for safer automation, project management, and strong, integrated workflows. Highlights include repository synchronization tools, access management options, workflow visualization, and CI/CD process upgrades.

<!--excerpt_end-->

## DevOps

### GitHub Platform Feature Advancements

To improve project organization and oversight, GitHub has introduced a hierarchy view for Projects, letting teams visualize and sort issues up to eight levels deep. Issue load times are faster, and you can add custom organization properties to tag projects, which supports policy management and helps teams coordinate.

App access rules have become more detailed, letting administrators better define request limits. Consent page warnings for app permissions are also improved for added transparency.

- [Hierarchy View for GitHub Projects Launches in Public Preview](https://github.blog/changelog/2026-01-15-hierarchy-view-now-available-in-github-projects)
- [GitHub Organization Custom Properties: General Availability](https://github.blog/changelog/2026-01-13-organization-custom-properties-now-generally-available)
- [Granular Controls for App Access Requests in GitHub Organizations Now Available](https://github.blog/changelog/2026-01-12-controlling-who-can-request-apps-for-your-organization-is-now-generally-available)
- [Improved Consent Page Warnings for GitHub Apps in Public Preview](https://github.blog/changelog/2026-01-12-selectively-showing-act-on-your-behalf-warning-for-github-apps-is-in-public-preview)

### CI/CD Security and Workflow Controls

GitHub Actions now offers an ''artifact_metadata'' permission for more targeted API access, supporting best practices around least-privilege use. Pipeline authors should review workflows for compatibility. Upload rates for Actions caches have strict limits for better pipeline reliability and clear guidance on how to optimize.

- [GitHub Introduces Fine-Grained artifact_metadata Permission for Enhanced API Access Control](https://github.blog/changelog/2026-01-13-new-fine-grained-permission-for-artifact-metadata-is-now-generally-available)
- [Rate limiting for GitHub Actions cache entries](https://github.blog/changelog/2026-01-16-rate-limiting-for-actions-cache-entries)

### Large-Scale Repository and Build Workflow Engineering

.NET’s new Virtual Monorepo Synchronization uses two-way, patch-based syncing for multi-repo setups, allowing automation and continuous delivery across many teams. Details are available for handling metadata and branches.

- [How .NET’s Virtual Monorepo Synchronization Works: Technical Challenges and Solutions](https://devblogs.microsoft.com/dotnet/how-we-synchronize-dotnets-virtual-monorepo/)

### Other DevOps News

December’s GitHub Availability Report details pipeline and infrastructure events, with lessons for monitoring and resilience. Tutorials are also available for developers working with Dev Containers/Codespaces and a practical introduction to managing git branches for newcomers.

- [GitHub Availability Report: December 2025](https://github.blog/news-insights/company-news/github-availability-report-december-2025/)

- [Running AI Coding Agents with Dev Containers and GitHub Codespaces](/ai/videos/running-ai-coding-agents-with-dev-containers-and-github-codespaces)
- [How to Use Git Branches for Beginners](/devops/videos/how-to-use-git-branches-for-beginners)',
     excerpt      = 'DevOps improvements this week introduce new GitHub and .NET features for safer automation, project management, and strong, integrated workflows. Highlights include repository synchronization tools, access management options, workflow visualization, and CI/CD process upgrades.',
     content_hash = md5('DevOps improvements this week introduce new GitHub and .NET features for safer automation, project management, and strong, integrated workflows. Highlights include repository synchronization tools, access management options, workflow visualization, and CI/CD process upgrades.

<!--excerpt_end-->

## DevOps

### GitHub Platform Feature Advancements

To improve project organization and oversight, GitHub has introduced a hierarchy view for Projects, letting teams visualize and sort issues up to eight levels deep. Issue load times are faster, and you can add custom organization properties to tag projects, which supports policy management and helps teams coordinate.

App access rules have become more detailed, letting administrators better define request limits. Consent page warnings for app permissions are also improved for added transparency.

- [Hierarchy View for GitHub Projects Launches in Public Preview](https://github.blog/changelog/2026-01-15-hierarchy-view-now-available-in-github-projects)
- [GitHub Organization Custom Properties: General Availability](https://github.blog/changelog/2026-01-13-organization-custom-properties-now-generally-available)
- [Granular Controls for App Access Requests in GitHub Organizations Now Available](https://github.blog/changelog/2026-01-12-controlling-who-can-request-apps-for-your-organization-is-now-generally-available)
- [Improved Consent Page Warnings for GitHub Apps in Public Preview](https://github.blog/changelog/2026-01-12-selectively-showing-act-on-your-behalf-warning-for-github-apps-is-in-public-preview)

### CI/CD Security and Workflow Controls

GitHub Actions now offers an ''artifact_metadata'' permission for more targeted API access, supporting best practices around least-privilege use. Pipeline authors should review workflows for compatibility. Upload rates for Actions caches have strict limits for better pipeline reliability and clear guidance on how to optimize.

- [GitHub Introduces Fine-Grained artifact_metadata Permission for Enhanced API Access Control](https://github.blog/changelog/2026-01-13-new-fine-grained-permission-for-artifact-metadata-is-now-generally-available)
- [Rate limiting for GitHub Actions cache entries](https://github.blog/changelog/2026-01-16-rate-limiting-for-actions-cache-entries)

### Large-Scale Repository and Build Workflow Engineering

.NET’s new Virtual Monorepo Synchronization uses two-way, patch-based syncing for multi-repo setups, allowing automation and continuous delivery across many teams. Details are available for handling metadata and branches.

- [How .NET’s Virtual Monorepo Synchronization Works: Technical Challenges and Solutions](https://devblogs.microsoft.com/dotnet/how-we-synchronize-dotnets-virtual-monorepo/)

### Other DevOps News

December’s GitHub Availability Report details pipeline and infrastructure events, with lessons for monitoring and resilience. Tutorials are also available for developers working with Dev Containers/Codespaces and a practical introduction to managing git branches for newcomers.

- [GitHub Availability Report: December 2025](https://github.blog/news-insights/company-news/github-availability-report-december-2025/)

- [Running AI Coding Agents with Dev Containers and GitHub Codespaces](/ai/videos/running-ai-coding-agents-with-dev-containers-and-github-codespaces)
- [How to Use Git Branches for Beginners](/devops/videos/how-to-use-git-branches-for-beginners)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-01-19';

-- weekly-devops-roundup-2026-01-26  (2026-01-26)
UPDATE content_items
SET  content      = 'DevOps news includes new GitHub Actions runners for cost-effective CI, improved issue and pull request workflows, and advances in artifact traceability and security features. Beginner-friendly content covers common workflows.

<!--excerpt_end-->

## DevOps

### GitHub Platform and Workflow Improvements

GitHub Actions announces general availability of 1 vCPU Linux runners for shorter, lightweight jobs such as linting and scripting—designed to help teams lower CI/CD costs. Updates expand on earlier changes for reliability and artifact management. GitHub Issues are now faster to load, reducing project dashboard wait times, and the modern "Files Changed" pull request interface with improved commenting is now the default—bringing better accessibility and review features.

- [GitHub Actions 1 vCPU Linux Runners Now Generally Available](https://github.blog/changelog/2026-01-22-1-vcpu-linux-runner-now-generally-available-in-github-actions)
- [Faster Loading for GitHub Issues: 35% of Views Now Under 200ms](https://github.blog/changelog/2026-01-22-faster-loading-for-github-issues)
- [GitHub''s Improved Pull Request “Files Changed” Experience Now Default](https://github.blog/changelog/2026-01-22-improved-pull-request-files-changed-page-on-by-default)

### Expanding Supply Chain Security and Artifact Traceability

GitHub adds artifact metadata APIs and Unified Artifact Views to help with code/build tracking and verification. REST APIs and Defender/JFrog integrations allow improved workflows. The new Packages tab displays build details, SLSA Level 3 proofs, and event histories. GitHub Actions now streamline attestation creation. Docs and guides offer step-by-step help for DevSecOps adoption.

- [Strengthen Your Supply Chain with GitHub Artifact Traceability and SLSA Build Level 3](https://github.blog/changelog/2026-01-20-strengthen-your-supply-chain-with-code-to-cloud-traceability-and-slsa-build-level-3-security)

### Other DevOps News

Enterprise admins can now use more precise budget controls that exclude cost center usage—supported by REST APIs for automation. User feedback feeds directly into policy and cost management improvements. New tutorials for beginners walk through `git switch`, `git add`, and `git commit`, highlighting the value of isolating changes and working with tracked project histories.

- [Enterprise-Scoped Budgets Feature Update for GitHub Enterprise Customers](https://github.blog/changelog/2026-01-19-enterprise-scoped-budgets-that-exclude-cost-center-usage-in-public-preview)
- [How to Switch Branches and Commit Changes in Git](/devops/videos/how-to-switch-branches-and-commit-changes-in-git)',
     excerpt      = 'DevOps news includes new GitHub Actions runners for cost-effective CI, improved issue and pull request workflows, and advances in artifact traceability and security features. Beginner-friendly content covers common workflows.',
     content_hash = md5('DevOps news includes new GitHub Actions runners for cost-effective CI, improved issue and pull request workflows, and advances in artifact traceability and security features. Beginner-friendly content covers common workflows.

<!--excerpt_end-->

## DevOps

### GitHub Platform and Workflow Improvements

GitHub Actions announces general availability of 1 vCPU Linux runners for shorter, lightweight jobs such as linting and scripting—designed to help teams lower CI/CD costs. Updates expand on earlier changes for reliability and artifact management. GitHub Issues are now faster to load, reducing project dashboard wait times, and the modern "Files Changed" pull request interface with improved commenting is now the default—bringing better accessibility and review features.

- [GitHub Actions 1 vCPU Linux Runners Now Generally Available](https://github.blog/changelog/2026-01-22-1-vcpu-linux-runner-now-generally-available-in-github-actions)
- [Faster Loading for GitHub Issues: 35% of Views Now Under 200ms](https://github.blog/changelog/2026-01-22-faster-loading-for-github-issues)
- [GitHub''s Improved Pull Request “Files Changed” Experience Now Default](https://github.blog/changelog/2026-01-22-improved-pull-request-files-changed-page-on-by-default)

### Expanding Supply Chain Security and Artifact Traceability

GitHub adds artifact metadata APIs and Unified Artifact Views to help with code/build tracking and verification. REST APIs and Defender/JFrog integrations allow improved workflows. The new Packages tab displays build details, SLSA Level 3 proofs, and event histories. GitHub Actions now streamline attestation creation. Docs and guides offer step-by-step help for DevSecOps adoption.

- [Strengthen Your Supply Chain with GitHub Artifact Traceability and SLSA Build Level 3](https://github.blog/changelog/2026-01-20-strengthen-your-supply-chain-with-code-to-cloud-traceability-and-slsa-build-level-3-security)

### Other DevOps News

Enterprise admins can now use more precise budget controls that exclude cost center usage—supported by REST APIs for automation. User feedback feeds directly into policy and cost management improvements. New tutorials for beginners walk through `git switch`, `git add`, and `git commit`, highlighting the value of isolating changes and working with tracked project histories.

- [Enterprise-Scoped Budgets Feature Update for GitHub Enterprise Customers](https://github.blog/changelog/2026-01-19-enterprise-scoped-budgets-that-exclude-cost-center-usage-in-public-preview)
- [How to Switch Branches and Commit Changes in Git](/devops/videos/how-to-switch-branches-and-commit-changes-in-git)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-01-26';

-- weekly-devops-roundup-2026-02-02  (2026-02-02)
UPDATE content_items
SET  content      = 'This week''s DevOps updates highlight actions for critical tool deprecation, new ARM CI/CD automation, and improvements in workflow editing and collaboration. Teams should plan for deprecations and migrations, update dependency management processes, and utilize workflow enhancements for compliance and incident response.

<!--excerpt_end-->

## DevOps

### GitHub Platform Updates: Actions, Codespaces, and Dependency Management

GitHub is deprecating Dependabot pull request comment commands, and recommends managing PRs with the web UI, CLI, or REST APIs. Guides are provided for migrations.

Scheduled upgrades for Docker/Docker Compose in GitHub runners (Feb 9) may require updates to existing automation. Actions now include a `case` function for conditional logic, improved debug logs, and major editor workflow enhancements. Metadata support and validation are improved, and Arm64 runners are available for private Linux/Windows repos. Codespaces brings enterprise data residency options (public preview).

- [Deprecation of GitHub Dependabot Pull Request Comment Commands](https://github.blog/changelog/2026-01-27-changes-to-github-dependabot-pull-request-comment-commands)
- [Docker and Docker Compose Upgrades on GitHub Hosted Runners](https://github.blog/changelog/2026-01-30-docker-and-docker-compose-version-upgrades-on-hosted-runners)
- [GitHub Actions: Enhanced Editing, Debugging, and New Case Function](https://github.blog/changelog/2026-01-29-github-actions-smarter-editing-clearer-debugging-and-a-new-case-function)
- [GitHub Actions: Linux and Windows arm64 Runners Now Supported in Private Repositories](https://github.blog/changelog/2026-01-29-arm64-standard-runners-are-now-available-in-private-repositories)
- [GitHub Codespaces Public Preview for Enterprise Cloud with Data Residency](https://github.blog/changelog/2026-01-29-codespaces-is-now-in-public-preview-for-github-enterprise-with-data-residency)

### Kubernetes Ingress Controller Deprecation and Migration

The Kubernetes community has announced the deprecation of the Ingress NGINX controller (support ends March 2026). Teams are encouraged to migrate early, exploring Gateway API, Envoy, Traefik, Cilium, or F5 NGINX as alternatives. Feature sets differ and automatic migration is unsupported. Extensive guidance is available for this transition.

- [Kubernetes Leadership Urges Migration from Ingress NGINX Due to Security Risks and Deprecation](https://devclass.com/2026/01/29/kubernetes-leadership-warns-of-ingress-nginx-risks-but-has-also-hastened-its-deprecation/)

### Automation, Observability, and SRE Agent Integration on Azure

This week, guides show how to link Dynatrace logs and Azure deployment data by using the Azure SRE Agent with Model Context Protocol. Subagents for log analysis can reduce incident response times, and the guide covers both configuration and extensibility for on-prem or remote management with tools like Grafana and Jira Cloud.

- [Unifying Scattered Observability Data from Dynatrace and Azure for Self-Healing Deployments with SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unifying-scattered-observability-data-from-dynatrace-azure-for/ba-p/4489547)
- [How SRE Agent Bridges Grafana and Jira for Incident Automation on Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-sre-agent-pulls-logs-from-grafana-and-creates-jira-tickets/ba-p/4489527)

### Evolving DevOps Workflows: Specification-Driven and Collaborative Development

A step-by-step approach for teams looking to transition to spec-driven, collaborative workflows is provided, including templates for team contracts, CI/CD validation, artifact traceability, and architecture discussions. The guide focuses on ensuring clarity, accountability, and robust DevOps for both monolith and distributed service patterns.

- [From Vibe Coding to Spec-Driven Development: Team Collaboration, CI/CD, and Advanced Patterns](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development-part4)

### Other DevOps News

GitHub Issues now supports semantic search for more relevant results, with an opt-out option. The latest GitHub podcast covers review management and Model Context Protocol improvements, and the Innovation Graph now provides expanded data for open source adoption and research.

- [Semantic Search Enhancement for GitHub Issues Now in Public Preview](https://github.blog/changelog/2026-01-29-improved-search-for-github-issues-in-public-preview)
- [GitHub’s Year in Review: Accessibility, Model Context Protocol, and Developer Wins](/devops/videos/githubs-year-in-review-accessibility-model-context-protocol-and-developer-wins)
- [GitHub Innovation Graph: 2025 Recap and Future Plans](https://github.blog/news-insights/policy-news-and-insights/year-recap-and-future-goals-for-the-github-innovation-graph/)',
     excerpt      = 'This week''s DevOps updates highlight actions for critical tool deprecation, new ARM CI/CD automation, and improvements in workflow editing and collaboration. Teams should plan for deprecations and migrations, update dependency management processes, and utilize workflow enhancements for compliance and incident response.',
     content_hash = md5('This week''s DevOps updates highlight actions for critical tool deprecation, new ARM CI/CD automation, and improvements in workflow editing and collaboration. Teams should plan for deprecations and migrations, update dependency management processes, and utilize workflow enhancements for compliance and incident response.

<!--excerpt_end-->

## DevOps

### GitHub Platform Updates: Actions, Codespaces, and Dependency Management

GitHub is deprecating Dependabot pull request comment commands, and recommends managing PRs with the web UI, CLI, or REST APIs. Guides are provided for migrations.

Scheduled upgrades for Docker/Docker Compose in GitHub runners (Feb 9) may require updates to existing automation. Actions now include a `case` function for conditional logic, improved debug logs, and major editor workflow enhancements. Metadata support and validation are improved, and Arm64 runners are available for private Linux/Windows repos. Codespaces brings enterprise data residency options (public preview).

- [Deprecation of GitHub Dependabot Pull Request Comment Commands](https://github.blog/changelog/2026-01-27-changes-to-github-dependabot-pull-request-comment-commands)
- [Docker and Docker Compose Upgrades on GitHub Hosted Runners](https://github.blog/changelog/2026-01-30-docker-and-docker-compose-version-upgrades-on-hosted-runners)
- [GitHub Actions: Enhanced Editing, Debugging, and New Case Function](https://github.blog/changelog/2026-01-29-github-actions-smarter-editing-clearer-debugging-and-a-new-case-function)
- [GitHub Actions: Linux and Windows arm64 Runners Now Supported in Private Repositories](https://github.blog/changelog/2026-01-29-arm64-standard-runners-are-now-available-in-private-repositories)
- [GitHub Codespaces Public Preview for Enterprise Cloud with Data Residency](https://github.blog/changelog/2026-01-29-codespaces-is-now-in-public-preview-for-github-enterprise-with-data-residency)

### Kubernetes Ingress Controller Deprecation and Migration

The Kubernetes community has announced the deprecation of the Ingress NGINX controller (support ends March 2026). Teams are encouraged to migrate early, exploring Gateway API, Envoy, Traefik, Cilium, or F5 NGINX as alternatives. Feature sets differ and automatic migration is unsupported. Extensive guidance is available for this transition.

- [Kubernetes Leadership Urges Migration from Ingress NGINX Due to Security Risks and Deprecation](https://devclass.com/2026/01/29/kubernetes-leadership-warns-of-ingress-nginx-risks-but-has-also-hastened-its-deprecation/)

### Automation, Observability, and SRE Agent Integration on Azure

This week, guides show how to link Dynatrace logs and Azure deployment data by using the Azure SRE Agent with Model Context Protocol. Subagents for log analysis can reduce incident response times, and the guide covers both configuration and extensibility for on-prem or remote management with tools like Grafana and Jira Cloud.

- [Unifying Scattered Observability Data from Dynatrace and Azure for Self-Healing Deployments with SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unifying-scattered-observability-data-from-dynatrace-azure-for/ba-p/4489547)
- [How SRE Agent Bridges Grafana and Jira for Incident Automation on Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-sre-agent-pulls-logs-from-grafana-and-creates-jira-tickets/ba-p/4489527)

### Evolving DevOps Workflows: Specification-Driven and Collaborative Development

A step-by-step approach for teams looking to transition to spec-driven, collaborative workflows is provided, including templates for team contracts, CI/CD validation, artifact traceability, and architecture discussions. The guide focuses on ensuring clarity, accountability, and robust DevOps for both monolith and distributed service patterns.

- [From Vibe Coding to Spec-Driven Development: Team Collaboration, CI/CD, and Advanced Patterns](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development-part4)

### Other DevOps News

GitHub Issues now supports semantic search for more relevant results, with an opt-out option. The latest GitHub podcast covers review management and Model Context Protocol improvements, and the Innovation Graph now provides expanded data for open source adoption and research.

- [Semantic Search Enhancement for GitHub Issues Now in Public Preview](https://github.blog/changelog/2026-01-29-improved-search-for-github-issues-in-public-preview)
- [GitHub’s Year in Review: Accessibility, Model Context Protocol, and Developer Wins](/devops/videos/githubs-year-in-review-accessibility-model-context-protocol-and-developer-wins)
- [GitHub Innovation Graph: 2025 Recap and Future Plans](https://github.blog/news-insights/policy-news-and-insights/year-recap-and-future-goals-for-the-github-innovation-graph/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-02-02';

-- weekly-devops-roundup-2026-02-09  (2026-02-09)
UPDATE content_items
SET  content      = 'Current DevOps improvements include faster environment setup, scalable CI/CD features, better supply chain controls, and enhanced review experience in GitHub and Windows workflows.

<!--excerpt_end-->

## DevOps

### GitHub Actions: Runner Updates, Scale Sets, and Enhanced Workflow Features

The deadline for upgrading self-hosted runners is extended to March 16, 2026, allowing more teams to keep pipelines secure while updating. Runner Scale Set Client (Go-based) is now in public preview and supports autoscaling on more platforms beyond Kubernetes. Action allow-listing works across all repo types, giving teams more control. New runner images for Server 2025 and macOS 26 Intel are available, and updates to editing, debug, and YAML features add further workflow flexibility.

- [GitHub Actions: Enforcement of Minimum Self-hosted Runner Version Extended to March 16, 2026](https://github.blog/changelog/2026-02-05-github-actions-self-hosted-runner-minimum-version-enforcement-extended)
- [GitHub Actions February 2026 Updates: Scale Sets, Security, and New Runner Images](https://github.blog/changelog/2026-02-05-github-actions-early-february-2026-updates)
- [The Download: OpenClaw AI Agents, Babel 7 Farewell, and GitHub Actions Updates](/the-download-openclaw-ai-agents-babel-7-farewell-and-github-actions-updates)

### WinGet Configuration: One-command Developer Environments

WinGet Configuration lets Windows users automate installation, settings, and toolchains from YAML files with a single command. It checks and applies only changes needed, supports developer mode, Visual Studio add-ons, logic for versioning, OS targeting, and infra setup, and integrates with PowerShell DSC. Version-controlling configs helps with setup and disaster recovery. CLI integration with Copilot speeds template creation, and Kayla Cinnamon’s walkthrough shows how to quickly set up reproducible developer environments.

- [WinGet Configuration: Set up your dev machine in one command](https://devblogs.microsoft.com/blog/winget-configuration-set-up-your-dev-machine-in-one-command)

### Open Source Ecosystem: Dependabot Proxy and Documentation Translation as Versioned Assets

Dependabot Proxy is now open sourced (MIT) and can update dependencies that require authentication for npm, Maven, Docker, NuGet, and Terraform. This adds visibility and auditability to supply chain automation for regulated teams.

Co-op Translator now stores documentation translations as versioned language files (JSON), making tracking changes against software releases easier. It helps contributors spot and fix out-of-date translations quickly.

- [Dependabot Proxy Now Open Source with MIT License](https://github.blog/changelog/2026-02-03-the-dependabot-proxy-is-now-open-source-with-an-mit-license)
- [Rethinking Documentation Translation: Treating Translations as Versioned Software Assets](https://techcommunity.microsoft.com/t5/microsoft-developer-community/rethinking-documentation-translation-treating-translations-as/ba-p/4491755)

### GitHub Review and Collaboration: Pull Request and Issue Experience

GitHub Mobile now lets users comment on any line of a file (even unchanged lines) in PRs, matching web review features. On desktop, UI improvements to the "Files Changed" view (including CODEOWNERS highlighting and faster navigation) assist with large or complicated reviews. GitHub Issues introduces pinned comments and a system that encourages reactions to keep discussions relevant and actionable.

- [GitHub Mobile: Comment on Unchanged Lines in Pull Request Files](https://github.blog/changelog/2026-02-03-github-mobile-comment-on-unchanged-lines-in-pull-request-files)
- [Improved Pull Request “Files Changed” – February 5 GitHub Updates](https://github.blog/changelog/2026-02-05-improved-pull-request-files-changed-february-5-updates)
- [Pinned Comments and Improved Comment Quality in GitHub Issues](https://github.blog/changelog/2026-02-05-pinned-comments-on-github-issues)',
     excerpt      = 'Current DevOps improvements include faster environment setup, scalable CI/CD features, better supply chain controls, and enhanced review experience in GitHub and Windows workflows.',
     content_hash = md5('Current DevOps improvements include faster environment setup, scalable CI/CD features, better supply chain controls, and enhanced review experience in GitHub and Windows workflows.

<!--excerpt_end-->

## DevOps

### GitHub Actions: Runner Updates, Scale Sets, and Enhanced Workflow Features

The deadline for upgrading self-hosted runners is extended to March 16, 2026, allowing more teams to keep pipelines secure while updating. Runner Scale Set Client (Go-based) is now in public preview and supports autoscaling on more platforms beyond Kubernetes. Action allow-listing works across all repo types, giving teams more control. New runner images for Server 2025 and macOS 26 Intel are available, and updates to editing, debug, and YAML features add further workflow flexibility.

- [GitHub Actions: Enforcement of Minimum Self-hosted Runner Version Extended to March 16, 2026](https://github.blog/changelog/2026-02-05-github-actions-self-hosted-runner-minimum-version-enforcement-extended)
- [GitHub Actions February 2026 Updates: Scale Sets, Security, and New Runner Images](https://github.blog/changelog/2026-02-05-github-actions-early-february-2026-updates)
- [The Download: OpenClaw AI Agents, Babel 7 Farewell, and GitHub Actions Updates](/the-download-openclaw-ai-agents-babel-7-farewell-and-github-actions-updates)

### WinGet Configuration: One-command Developer Environments

WinGet Configuration lets Windows users automate installation, settings, and toolchains from YAML files with a single command. It checks and applies only changes needed, supports developer mode, Visual Studio add-ons, logic for versioning, OS targeting, and infra setup, and integrates with PowerShell DSC. Version-controlling configs helps with setup and disaster recovery. CLI integration with Copilot speeds template creation, and Kayla Cinnamon’s walkthrough shows how to quickly set up reproducible developer environments.

- [WinGet Configuration: Set up your dev machine in one command](https://devblogs.microsoft.com/blog/winget-configuration-set-up-your-dev-machine-in-one-command)

### Open Source Ecosystem: Dependabot Proxy and Documentation Translation as Versioned Assets

Dependabot Proxy is now open sourced (MIT) and can update dependencies that require authentication for npm, Maven, Docker, NuGet, and Terraform. This adds visibility and auditability to supply chain automation for regulated teams.

Co-op Translator now stores documentation translations as versioned language files (JSON), making tracking changes against software releases easier. It helps contributors spot and fix out-of-date translations quickly.

- [Dependabot Proxy Now Open Source with MIT License](https://github.blog/changelog/2026-02-03-the-dependabot-proxy-is-now-open-source-with-an-mit-license)
- [Rethinking Documentation Translation: Treating Translations as Versioned Software Assets](https://techcommunity.microsoft.com/t5/microsoft-developer-community/rethinking-documentation-translation-treating-translations-as/ba-p/4491755)

### GitHub Review and Collaboration: Pull Request and Issue Experience

GitHub Mobile now lets users comment on any line of a file (even unchanged lines) in PRs, matching web review features. On desktop, UI improvements to the "Files Changed" view (including CODEOWNERS highlighting and faster navigation) assist with large or complicated reviews. GitHub Issues introduces pinned comments and a system that encourages reactions to keep discussions relevant and actionable.

- [GitHub Mobile: Comment on Unchanged Lines in Pull Request Files](https://github.blog/changelog/2026-02-03-github-mobile-comment-on-unchanged-lines-in-pull-request-files)
- [Improved Pull Request “Files Changed” – February 5 GitHub Updates](https://github.blog/changelog/2026-02-05-improved-pull-request-files-changed-february-5-updates)
- [Pinned Comments and Improved Comment Quality in GitHub Issues](https://github.blog/changelog/2026-02-05-pinned-comments-on-github-issues)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-02-09';

-- weekly-devops-roundup-2026-02-16  (2026-02-16)
UPDATE content_items
SET  content      = 'In DevOps, teams benefit from updated Azure DevOps Server patches, finer-grained GitHub permissions, and governance tools that help keep collaborative workflows secure as automation and AI-driven contribution increase.

<!--excerpt_end-->

## DevOps

### Azure DevOps Server: February Patch Release

The February 2026 Azure DevOps Server patches provide updates for 2022.2, 2020.1.2, and 2019.1.2. Each release contains patch notes, install scripts, and instructions for validation. Teams should run ''<patch-installer>.exe CheckInstall'' when updating. Staying current with these releases helps organizations keep systems secure and minimize downtime.

- [February Patches for Azure DevOps Server](https://devblogs.microsoft.com/devops/february-patches-for-azure-devops-server-5/)

### GitHub Platform Updates: Permissions, PR Controls, and Open Source Governance

GitHub Apps now support fine-grained permissions for Enterprise Teams APIs, allowing more secure automation and audit tracking. Repository settings can now restrict or disable PRs, providing new controls as AI-generated pull requests grow more common.

Open source maintainers now have new contributor controls: better navigation for large diffs, UI upgrades, banners, reputation-based gating, and the "vouch" trust management system. These changes help automate standards and build safe, scalable CI/CD pipelines.

This is a continuation of last week’s trend toward enhanced workflow controls, permission systems, and automated community management—vital as global contributor numbers, both human and AI, continue to grow.

- [GitHub Apps Gain Fine-Grained Permissions for Enterprise Teams APIs](https://github.blog/changelog/2026-02-09-github-apps-can-now-utilize-public-preview-enterprise-teams-apis-via-fine-grained-permissions)
- [New GitHub Settings to Configure Pull Request Access](https://github.blog/changelog/2026-02-13-new-repository-settings-for-configuring-pull-request-access)
- [Open Source’s Eternal September: Supporting Maintainers Amid the Scaling Contributor Wave](https://github.blog/open-source/maintainers/welcome-to-the-eternal-september-of-open-source-heres-what-we-plan-to-do-for-maintainers/)
- [Open Source Friday: Trust Management with Vouch and GitHub](/open-source-friday-trust-management-with-vouch-and-github)',
     excerpt      = 'In DevOps, teams benefit from updated Azure DevOps Server patches, finer-grained GitHub permissions, and governance tools that help keep collaborative workflows secure as automation and AI-driven contribution increase.',
     content_hash = md5('In DevOps, teams benefit from updated Azure DevOps Server patches, finer-grained GitHub permissions, and governance tools that help keep collaborative workflows secure as automation and AI-driven contribution increase.

<!--excerpt_end-->

## DevOps

### Azure DevOps Server: February Patch Release

The February 2026 Azure DevOps Server patches provide updates for 2022.2, 2020.1.2, and 2019.1.2. Each release contains patch notes, install scripts, and instructions for validation. Teams should run ''<patch-installer>.exe CheckInstall'' when updating. Staying current with these releases helps organizations keep systems secure and minimize downtime.

- [February Patches for Azure DevOps Server](https://devblogs.microsoft.com/devops/february-patches-for-azure-devops-server-5/)

### GitHub Platform Updates: Permissions, PR Controls, and Open Source Governance

GitHub Apps now support fine-grained permissions for Enterprise Teams APIs, allowing more secure automation and audit tracking. Repository settings can now restrict or disable PRs, providing new controls as AI-generated pull requests grow more common.

Open source maintainers now have new contributor controls: better navigation for large diffs, UI upgrades, banners, reputation-based gating, and the "vouch" trust management system. These changes help automate standards and build safe, scalable CI/CD pipelines.

This is a continuation of last week’s trend toward enhanced workflow controls, permission systems, and automated community management—vital as global contributor numbers, both human and AI, continue to grow.

- [GitHub Apps Gain Fine-Grained Permissions for Enterprise Teams APIs](https://github.blog/changelog/2026-02-09-github-apps-can-now-utilize-public-preview-enterprise-teams-apis-via-fine-grained-permissions)
- [New GitHub Settings to Configure Pull Request Access](https://github.blog/changelog/2026-02-13-new-repository-settings-for-configuring-pull-request-access)
- [Open Source’s Eternal September: Supporting Maintainers Amid the Scaling Contributor Wave](https://github.blog/open-source/maintainers/welcome-to-the-eternal-september-of-open-source-heres-what-we-plan-to-do-for-maintainers/)
- [Open Source Friday: Trust Management with Vouch and GitHub](/open-source-friday-trust-management-with-vouch-and-github)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-02-16';

-- weekly-devops-roundup-2026-02-23  (2026-02-23)
UPDATE content_items
SET  content      = 'DevOps updates this week include new automation and governance features for versioning, pipelines, and team management. GitHub, Azure, and Microsoft Fabric all deliver enhancements for customization and compliance in global developer workflows.

<!--excerpt_end-->

## DevOps

### GitHub Platform and Workflow Enhancements

GitHub Desktop 3.5.5 adds standard Git hook support for Windows and Unix, with management tools for commit automation and real-time feedback on errors. Commits can reference Copilot as an author for better workflow visibility.

APIs and workflow triggers bring new details, such as run IDs for monitoring. Project management tools receive updates for search imports and hierarchical views, while a redesigned comments panel simplifies pull request reviews. Test merge commits now use resources more efficiently in CI, and improved reviewer rules reinforce repository compliance.

These updates reinforce GitHub’s direction from last week—adding automation, control, and enhanced workflow structure.

- [GitHub Desktop 3.5.5 Adds Native Git Hooks Support](https://github.blog/changelog/2026-02-18-github-desktop-3-5-5-adds-git-hooks-support)
- [API Access to GitHub Billing Usage Reports in Public Preview](https://github.blog/changelog/2026-02-17-api-access-to-billing-usage-reports-in-public-preview)
- [GitHub Actions Workflow Dispatch API Now Returns Run IDs and Details](https://github.blog/changelog/2026-02-19-workflow-dispatch-api-now-returns-run-ids)
- [Enhancements to GitHub Projects: Search Import and Hierarchy View Updates](https://github.blog/changelog/2026-02-19-github-projects-import-items-based-on-a-query-and-hierarchy-view-improvements)
- [Improved Pull Request Commenting Experience on GitHub''s Files Changed Page](https://github.blog/changelog/2026-02-19-access-all-pull-request-comments-without-leaving-the-new-files-changed-page)
- [Changes to Test Merge Commit Generation for GitHub Pull Requests](https://github.blog/changelog/2026-02-19-changes-to-test-merge-commit-generation-for-pull-requests)
- [Custom Properties and Rule Insights Improvements in GitHub Organizations](https://github.blog/changelog/2026-02-17-custom-properties-and-rule-insights-improvements)
- [Required Reviewer Rule for Repository Rulesets Now Generally Available](https://github.blog/changelog/2026-02-17-required-reviewer-rule-is-now-generally-available)

### Azure DevOps and SRE Automation

Azure DevOps Boards now offer condensed Kanban and Sprint views for improved clarity and focused automation. Azure SRE Agent receives new documentation for managing incidents, and TFVC announces the deprecation of older check-in policies, signaling a move toward modern version control and governance. These updates align with recent changes supporting permissions and automated workflows.

- [Introducing Condensed Views for Kanban and Sprint Boards in Azure DevOps](https://devblogs.microsoft.com/devops/condensed-views-on-kanban-and-sprint-boards/)
- [Reactive Incident Response with Azure SRE Agent: From Alert to Resolution in Minutes](https://techcommunity.microsoft.com/t5/azure-architecture-blog/reactive-incident-response-with-azure-sre-agent-from-alert-to/ba-p/4492938)
- [TFVC Remove Existing Obsolete Policies ASAP](https://devblogs.microsoft.com/devops/tfvc-remove-existing-obsolete-policies-asap/)

### Microsoft Fabric CI/CD Automation

Microsoft fabric-cicd now has official support, moving the tool from the community into Microsoft’s main DevOps roadmap. Development teams have a maintained, first-party CI/CD automation option for managing artifacts and dependencies.

This aligns with a broader trend toward robust automation and best practices in Microsoft Fabric environments.

- [Announcing Official Support for Microsoft fabric-cicd Tool](https://blog.fabric.microsoft.com/en-US/blog/announcing-official-support-for-microsoft-fabric-cicd-tool/)

### Open Source Ecosystem Developments

Open source community growth and best practices for scaling project onboarding and automation are summarized in the Octoverse 2026 report findings. With more contributors and increased automation, sustainable project management is vital.

These findings reinforce the observations shared last week on healthy open source ecosystem evolution.

- [Open Source Trends for 2026: Insights from GitHub’s Octoverse Report](https://github.blog/open-source/maintainers/what-to-expect-for-open-source-in-2026/)',
     excerpt      = 'DevOps updates this week include new automation and governance features for versioning, pipelines, and team management. GitHub, Azure, and Microsoft Fabric all deliver enhancements for customization and compliance in global developer workflows.',
     content_hash = md5('DevOps updates this week include new automation and governance features for versioning, pipelines, and team management. GitHub, Azure, and Microsoft Fabric all deliver enhancements for customization and compliance in global developer workflows.

<!--excerpt_end-->

## DevOps

### GitHub Platform and Workflow Enhancements

GitHub Desktop 3.5.5 adds standard Git hook support for Windows and Unix, with management tools for commit automation and real-time feedback on errors. Commits can reference Copilot as an author for better workflow visibility.

APIs and workflow triggers bring new details, such as run IDs for monitoring. Project management tools receive updates for search imports and hierarchical views, while a redesigned comments panel simplifies pull request reviews. Test merge commits now use resources more efficiently in CI, and improved reviewer rules reinforce repository compliance.

These updates reinforce GitHub’s direction from last week—adding automation, control, and enhanced workflow structure.

- [GitHub Desktop 3.5.5 Adds Native Git Hooks Support](https://github.blog/changelog/2026-02-18-github-desktop-3-5-5-adds-git-hooks-support)
- [API Access to GitHub Billing Usage Reports in Public Preview](https://github.blog/changelog/2026-02-17-api-access-to-billing-usage-reports-in-public-preview)
- [GitHub Actions Workflow Dispatch API Now Returns Run IDs and Details](https://github.blog/changelog/2026-02-19-workflow-dispatch-api-now-returns-run-ids)
- [Enhancements to GitHub Projects: Search Import and Hierarchy View Updates](https://github.blog/changelog/2026-02-19-github-projects-import-items-based-on-a-query-and-hierarchy-view-improvements)
- [Improved Pull Request Commenting Experience on GitHub''s Files Changed Page](https://github.blog/changelog/2026-02-19-access-all-pull-request-comments-without-leaving-the-new-files-changed-page)
- [Changes to Test Merge Commit Generation for GitHub Pull Requests](https://github.blog/changelog/2026-02-19-changes-to-test-merge-commit-generation-for-pull-requests)
- [Custom Properties and Rule Insights Improvements in GitHub Organizations](https://github.blog/changelog/2026-02-17-custom-properties-and-rule-insights-improvements)
- [Required Reviewer Rule for Repository Rulesets Now Generally Available](https://github.blog/changelog/2026-02-17-required-reviewer-rule-is-now-generally-available)

### Azure DevOps and SRE Automation

Azure DevOps Boards now offer condensed Kanban and Sprint views for improved clarity and focused automation. Azure SRE Agent receives new documentation for managing incidents, and TFVC announces the deprecation of older check-in policies, signaling a move toward modern version control and governance. These updates align with recent changes supporting permissions and automated workflows.

- [Introducing Condensed Views for Kanban and Sprint Boards in Azure DevOps](https://devblogs.microsoft.com/devops/condensed-views-on-kanban-and-sprint-boards/)
- [Reactive Incident Response with Azure SRE Agent: From Alert to Resolution in Minutes](https://techcommunity.microsoft.com/t5/azure-architecture-blog/reactive-incident-response-with-azure-sre-agent-from-alert-to/ba-p/4492938)
- [TFVC Remove Existing Obsolete Policies ASAP](https://devblogs.microsoft.com/devops/tfvc-remove-existing-obsolete-policies-asap/)

### Microsoft Fabric CI/CD Automation

Microsoft fabric-cicd now has official support, moving the tool from the community into Microsoft’s main DevOps roadmap. Development teams have a maintained, first-party CI/CD automation option for managing artifacts and dependencies.

This aligns with a broader trend toward robust automation and best practices in Microsoft Fabric environments.

- [Announcing Official Support for Microsoft fabric-cicd Tool](https://blog.fabric.microsoft.com/en-US/blog/announcing-official-support-for-microsoft-fabric-cicd-tool/)

### Open Source Ecosystem Developments

Open source community growth and best practices for scaling project onboarding and automation are summarized in the Octoverse 2026 report findings. With more contributors and increased automation, sustainable project management is vital.

These findings reinforce the observations shared last week on healthy open source ecosystem evolution.

- [Open Source Trends for 2026: Insights from GitHub’s Octoverse Report](https://github.blog/open-source/maintainers/what-to-expect-for-open-source-in-2026/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-02-23';

-- SKIP 2026-03-02 : no DB record for section 'devops'

-- SKIP 2026-03-09 : no DB record for section 'devops'

-- weekly-devops-roundup-2026-03-16  (2026-03-16)
UPDATE content_items
SET  content      = 'DevOps coverage split into two practical tracks: AI-assisted operations moving into governed production workflows, and keeping CI/CD platforms stable as providers add security controls, version requirements, and reliability fixes. Pipeline hygiene also improved with updates to dependency automation, issue metadata, and database/testing workflows that fit Git-based delivery. Building on last week’s reliability work (GitHub’s HA search architecture) and GitHub Issues/Projects workflow improvements, this week adds more “operate safely at scale” pieces: governance controls for agentic incident response, clearer runner compliance expectations, and more structured issue metadata to replace ad-hoc labels.

<!--excerpt_end-->

## DevOps

### Azure SRE Agent reaches GA—and adds production governance for agentic ops

Azure SRE Agent reached GA with an emphasis on making incident response automation easier to adopt and safer in real environments. GA highlights include a redesigned onboarding flow that collects needed context - source, telemetry/logs, incidents, Azure resources, knowledge files - so teams can set up end-to-end investigations without stitching steps together. With context attached, deep context and persistent memory retain operational history (incidents, deployments, known failure modes) so investigations become less prompt-driven and more proactive.

GA also emphasizes integrations and orchestration: ingestion and workflows via ICM, PagerDuty, and ServiceNow; RCA linking telemetry to code paths; and automation via MCP connectors and generic HTTP integrations. Extensibility remains central - custom Python scripts, skills/plugins, subagents, and a Plugin Marketplace - so teams can turn runbooks into repeatable actions. This matches last week’s microservices guidance around tracing and CI/CD: distributed systems benefit from repeatable investigation and remediation steps that do not rely on one on-call engineer’s memory.

Governance is the other GA pillar, and Agent Hooks guidance describes the production controls teams need before letting an agent execute changes. Hooks intercept runtime behavior (agent/org/thread scopes) to enforce policy-as-code guardrails. A Stop Hook can block vague output and require a retry unless the agent provides structured, evidence-backed diagnosis (for example, Root Cause, Evidence with real metric values, and remediation steps). A PostToolUse Hook can enforce allowlists (for example, allowing `az postgres flexible-server restart`) while blocking destructive commands (`DROP`, `rm -rf`). A Global Hook can log tool usage (turn, tool, success/failure) with optional enablement to manage volume. The PostgreSQL Flexible Server latency scenario ties it together: allow investigation via metrics/logs, but only permit remediation when evidence meets policy and actions match approved patterns.

- [Announcing General Availability for the Azure SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-for-the-azure-sre-agent/ba-p/4500682)
- [What''s New in Azure SRE Agent: GA Release Highlights](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-in-azure-sre-agent-in-the-ga-release/ba-p/4500779)
- [''Azure SRE Agent Is Now Generally Available: New Features and Capabilities''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-is-generally-available-what-s-new/ba-p/4500779)
- [''Agent Hooks: Production-Grade Governance for Azure SRE Agent''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/agent-hooks-production-grade-governance-for-azure-sre-agent/ba-p/4500292)

### GitHub Actions and GitHub platform operations: runner compliance, richer OIDC claims, and reliability learnings

GitHub Actions updates focused on avoiding future breakage while giving teams time to adapt. GitHub paused enforcement of the minimum self-hosted runner version requirement (still v2.329.0) after previously targeting March 16, 2026. Older runners can still be registered/configured during the pause, which prevents immediate disruption for orgs still upgrading fleets or images. GitHub also says enforcement will return, so teams should still upgrade VMs, containers, autoscaling images, and provisioning automation to avoid sudden failures later. Like last week’s Actions Example Checker theme, it’s the same hygiene issue: keep tooling, images, and automation current so platform changes do not break pipelines.

GitHub also expanded OIDC workload identity in Actions by allowing repository custom properties to appear as token claims (prefixed `repo_property_`). This supports attribute-based access control: set repository metadata (environment, classification, cost center, compliance tier) at repo/org/enterprise level and let cloud trust policies use those claims instead of hardcoding repository names or duplicating workflow logic. A public preview settings page for configuring token claims via UI or API signals this is meant to be a managed governance surface, which aligns with last week’s admin-controlled policy direction.

GitHub’s reliability narrative continued with the February 2026 availability report and a remediation write-up on February/March incidents. They highlight failure modes CI/CD teams should plan for: backend policy changes affecting hosted runner lifecycles, cache/database scaling issues degrading auth and API automation, and failover gaps (for example, Redis failover leaving no writable primary). This complements last week’s GHE search reliability story: simplify topology, validate failover, and reduce hidden coupling. GitHub’s mitigations - cache segmentation, load protection/shedding, isolation, capacity audits, stronger failover validation, and continued Azure migration - map to customer practices: retries/backoff and idempotency around API calls, documented fallbacks for development environments, and hybrid CI approaches where self-hosted runners cover critical workloads when hosted capacity is impaired.

- [''GitHub Actions: Minimum Self-Hosted Runner Version Enforcement Paused''](https://github.blog/changelog/2026-03-13-self-hosted-runner-minimum-version-enforcement-paused)
- [Actions OIDC Tokens Now Support Repository Custom Properties](https://github.blog/changelog/2026-03-12-actions-oidc-tokens-now-support-repository-custom-properties)
- [''GitHub Availability Report: Service Outages and Performance Incidents in February 2026''](https://github.blog/news-insights/company-news/github-availability-report-february-2026/)
- [Addressing GitHub''s Recent Availability and Reliability Incidents](https://github.blog/news-insights/company-news/addressing-githubs-recent-availability-issues-2/)

### Azure DevOps operations: urgent patching and a deadline to migrate Advanced Security automation

Azure DevOps updates were time-sensitive and focused on preventing access and security automation from breaking in active environments. For Azure DevOps Server, Microsoft released Patch 2 (March 13, 2026) to fix an issue where group memberships could be deactivated under certain conditions. This is an access-control problem that can cascade into repository permissions, pipelines, and service accounts. Guidance is specific: install Patch 2 if you installed before the re-published release (March 13, 2026). If you ran the mitigation script, Patch 2 completes remediation. If you did not, Patch 2 alone is enough. Admins can verify via the installer’s `CheckInstall` argument.

In Azure DevOps Services, Microsoft temporarily rolled back Sprint 269 restrictions so build service identities can again call Advanced Security APIs after the restriction broke automation. The rollback has a deadline: build identities keep access only until April 15, 2026, then restrictions return. The recommended fix is migrating automation to a service principal with “Advanced Security: Read alerts,” narrowly scoped. For licensing concerns, service principals that do not commit code will not consume an Advanced Security committer license. Sprint 272 is also expected to add status checks that gate PR merges based on high/critical alerts, which may replace custom “call API and decide” pipelines. This lines up with the GitHub trend from last week: governance and quality move into platform controls and merge gates, not only custom pipeline scripts.

- [March Patches for Azure DevOps Server](https://devblogs.microsoft.com/devops/march-patches-for-azure-devops-server-4/)
- [''Temporary Rollback: Build Identities Can Access Advanced Security APIs Again''](https://devblogs.microsoft.com/devops/temporary-rollback-build-identities-can-access-advanced-security-read-alerts-again/)

### Other DevOps News

GitHub released a new REST API version (“2026-03-10”), the first calendar-based version with breaking changes. Integration owners should review breaking changes, then opt in explicitly using `X-GitHub-Api-Version: 2026-03-10` while validating response-shape assumptions and error handling. The default remains `2022-11-28` for at least 24 months if you do not set the header. GitHub also launched issue fields in public preview for select orgs: typed org-level metadata (up to 25 fields; single select/text/number/date) searchable across repositories, usable in Projects views, automatable via REST/GraphQL, and emitting webhooks (`field_added`, `field_removed`). If last week’s Projects features structured boards, issue fields structure the issue data itself for consistent queries and automation across repositories.

- [GitHub REST API Version 2026-03-10 Now Available](https://github.blog/changelog/2026-03-12-rest-api-version-2026-03-10-is-now-available)
- [''GitHub Issue Fields Public Preview: Structured Metadata for Issues''](https://github.blog/changelog/2026-03-12-issue-fields-structured-issue-metadata-is-in-public-preview)

Dependabot can now open PRs updating `.pre-commit-config.yaml` hook revisions when you set ecosystem `pre-commit` in `dependabot.yml`, supporting tag pins and commit SHA pins (preserving YAML formatting and skipping `local`/`meta` hooks). In JavaScript, an alpha “npmx” package browser launched to help evaluate npm packages (module format, install size, outdated dependency signals), which may help dependency due diligence despite being early.

- [Dependabot Now Supports Automatic Updates for pre-commit Hooks](https://github.blog/changelog/2026-03-10-dependabot-now-supports-pre-commit-hooks)
- [npmx Package Browser Released as Alpha to Improve npmjs Experience](https://www.devclass.com/devops/2026/03/09/npmx-package-browser-released-as-alpha-to-fix-pain-of-using-npmjs/4093605)

Microsoft Fabric added publishing SQL database schema changes from VS Code via SQL Database Projects, including a Publish dialog that browses Fabric workspaces/databases, previews the deployment script, and exposes options (including deletion behavior). It also adds templates for common objects and optional validation using a local SQL Server 2025 container. A Harness tutorial showed building a CI pipeline on AKS using Delegates and Connectors, with Secrets Manager (optionally Azure Key Vault-backed) storing service principal creds so Azure access stays within a governed connector and network boundary.

- [Deploy SQL Databases in Microsoft Fabric Directly from VS Code](https://blog.fabric.microsoft.com/en-US/blog/deploy-sql-databases-in-fabric-from-vs-code-no-more-context-switching/)
- [How to Create a Harness Pipeline and Integrate with Azure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/how-to-create-a-harness-pipeline-and-integrate-with-azure/ba-p/4499862)

A Behave tutorial showed structuring Python BDD suites in VS Code (feature files, steps, `environment.py` hooks, `behave.ini`, tagging) in a way that maps cleanly to CI. A platform engineering essay argued “human scale” coordination is the constraint. Tool sprawl across Kubernetes, observability, and CI/CD creates overhead, and the post recommends evolving platform interfaces while using AI assistants to surface institutional knowledge without building overly rigid platforms. It also connects to this week’s agent governance theme: adoption tends to hinge less on capability and more on standard interfaces, guardrails, and shared context for safe collaboration across complex stacks.

- [''Getting Started with Behave: Writing Cucumber Tests in VS Code''](https://techcommunity.microsoft.com/t5/microsoft-developer-community/getting-started-with-behave-writing-cucumber-tests-in-vs-code/ba-p/4496865)
- [The Human Scale Problem in Platform Engineering](https://devblogs.microsoft.com/all-things-azure/the-human-scale-problem-in-platform-engineering/)',
     excerpt      = 'DevOps coverage split into two practical tracks: AI-assisted operations moving into governed production workflows, and keeping CI/CD platforms stable as providers add security controls, version requirements, and reliability fixes. Pipeline hygiene also improved with updates to dependency automation, issue metadata, and database/testing workflows that fit Git-based delivery. Building on last week’s reliability work (GitHub’s HA search architecture) and GitHub Issues/Projects workflow improvements, this week adds more “operate safely at scale” pieces: governance controls for agentic incident response, clearer runner compliance expectations, and more structured issue metadata to replace ad-hoc labels.',
     content_hash = md5('DevOps coverage split into two practical tracks: AI-assisted operations moving into governed production workflows, and keeping CI/CD platforms stable as providers add security controls, version requirements, and reliability fixes. Pipeline hygiene also improved with updates to dependency automation, issue metadata, and database/testing workflows that fit Git-based delivery. Building on last week’s reliability work (GitHub’s HA search architecture) and GitHub Issues/Projects workflow improvements, this week adds more “operate safely at scale” pieces: governance controls for agentic incident response, clearer runner compliance expectations, and more structured issue metadata to replace ad-hoc labels.

<!--excerpt_end-->

## DevOps

### Azure SRE Agent reaches GA—and adds production governance for agentic ops

Azure SRE Agent reached GA with an emphasis on making incident response automation easier to adopt and safer in real environments. GA highlights include a redesigned onboarding flow that collects needed context - source, telemetry/logs, incidents, Azure resources, knowledge files - so teams can set up end-to-end investigations without stitching steps together. With context attached, deep context and persistent memory retain operational history (incidents, deployments, known failure modes) so investigations become less prompt-driven and more proactive.

GA also emphasizes integrations and orchestration: ingestion and workflows via ICM, PagerDuty, and ServiceNow; RCA linking telemetry to code paths; and automation via MCP connectors and generic HTTP integrations. Extensibility remains central - custom Python scripts, skills/plugins, subagents, and a Plugin Marketplace - so teams can turn runbooks into repeatable actions. This matches last week’s microservices guidance around tracing and CI/CD: distributed systems benefit from repeatable investigation and remediation steps that do not rely on one on-call engineer’s memory.

Governance is the other GA pillar, and Agent Hooks guidance describes the production controls teams need before letting an agent execute changes. Hooks intercept runtime behavior (agent/org/thread scopes) to enforce policy-as-code guardrails. A Stop Hook can block vague output and require a retry unless the agent provides structured, evidence-backed diagnosis (for example, Root Cause, Evidence with real metric values, and remediation steps). A PostToolUse Hook can enforce allowlists (for example, allowing `az postgres flexible-server restart`) while blocking destructive commands (`DROP`, `rm -rf`). A Global Hook can log tool usage (turn, tool, success/failure) with optional enablement to manage volume. The PostgreSQL Flexible Server latency scenario ties it together: allow investigation via metrics/logs, but only permit remediation when evidence meets policy and actions match approved patterns.

- [Announcing General Availability for the Azure SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-for-the-azure-sre-agent/ba-p/4500682)
- [What''s New in Azure SRE Agent: GA Release Highlights](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-in-azure-sre-agent-in-the-ga-release/ba-p/4500779)
- [''Azure SRE Agent Is Now Generally Available: New Features and Capabilities''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-is-generally-available-what-s-new/ba-p/4500779)
- [''Agent Hooks: Production-Grade Governance for Azure SRE Agent''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/agent-hooks-production-grade-governance-for-azure-sre-agent/ba-p/4500292)

### GitHub Actions and GitHub platform operations: runner compliance, richer OIDC claims, and reliability learnings

GitHub Actions updates focused on avoiding future breakage while giving teams time to adapt. GitHub paused enforcement of the minimum self-hosted runner version requirement (still v2.329.0) after previously targeting March 16, 2026. Older runners can still be registered/configured during the pause, which prevents immediate disruption for orgs still upgrading fleets or images. GitHub also says enforcement will return, so teams should still upgrade VMs, containers, autoscaling images, and provisioning automation to avoid sudden failures later. Like last week’s Actions Example Checker theme, it’s the same hygiene issue: keep tooling, images, and automation current so platform changes do not break pipelines.

GitHub also expanded OIDC workload identity in Actions by allowing repository custom properties to appear as token claims (prefixed `repo_property_`). This supports attribute-based access control: set repository metadata (environment, classification, cost center, compliance tier) at repo/org/enterprise level and let cloud trust policies use those claims instead of hardcoding repository names or duplicating workflow logic. A public preview settings page for configuring token claims via UI or API signals this is meant to be a managed governance surface, which aligns with last week’s admin-controlled policy direction.

GitHub’s reliability narrative continued with the February 2026 availability report and a remediation write-up on February/March incidents. They highlight failure modes CI/CD teams should plan for: backend policy changes affecting hosted runner lifecycles, cache/database scaling issues degrading auth and API automation, and failover gaps (for example, Redis failover leaving no writable primary). This complements last week’s GHE search reliability story: simplify topology, validate failover, and reduce hidden coupling. GitHub’s mitigations - cache segmentation, load protection/shedding, isolation, capacity audits, stronger failover validation, and continued Azure migration - map to customer practices: retries/backoff and idempotency around API calls, documented fallbacks for development environments, and hybrid CI approaches where self-hosted runners cover critical workloads when hosted capacity is impaired.

- [''GitHub Actions: Minimum Self-Hosted Runner Version Enforcement Paused''](https://github.blog/changelog/2026-03-13-self-hosted-runner-minimum-version-enforcement-paused)
- [Actions OIDC Tokens Now Support Repository Custom Properties](https://github.blog/changelog/2026-03-12-actions-oidc-tokens-now-support-repository-custom-properties)
- [''GitHub Availability Report: Service Outages and Performance Incidents in February 2026''](https://github.blog/news-insights/company-news/github-availability-report-february-2026/)
- [Addressing GitHub''s Recent Availability and Reliability Incidents](https://github.blog/news-insights/company-news/addressing-githubs-recent-availability-issues-2/)

### Azure DevOps operations: urgent patching and a deadline to migrate Advanced Security automation

Azure DevOps updates were time-sensitive and focused on preventing access and security automation from breaking in active environments. For Azure DevOps Server, Microsoft released Patch 2 (March 13, 2026) to fix an issue where group memberships could be deactivated under certain conditions. This is an access-control problem that can cascade into repository permissions, pipelines, and service accounts. Guidance is specific: install Patch 2 if you installed before the re-published release (March 13, 2026). If you ran the mitigation script, Patch 2 completes remediation. If you did not, Patch 2 alone is enough. Admins can verify via the installer’s `CheckInstall` argument.

In Azure DevOps Services, Microsoft temporarily rolled back Sprint 269 restrictions so build service identities can again call Advanced Security APIs after the restriction broke automation. The rollback has a deadline: build identities keep access only until April 15, 2026, then restrictions return. The recommended fix is migrating automation to a service principal with “Advanced Security: Read alerts,” narrowly scoped. For licensing concerns, service principals that do not commit code will not consume an Advanced Security committer license. Sprint 272 is also expected to add status checks that gate PR merges based on high/critical alerts, which may replace custom “call API and decide” pipelines. This lines up with the GitHub trend from last week: governance and quality move into platform controls and merge gates, not only custom pipeline scripts.

- [March Patches for Azure DevOps Server](https://devblogs.microsoft.com/devops/march-patches-for-azure-devops-server-4/)
- [''Temporary Rollback: Build Identities Can Access Advanced Security APIs Again''](https://devblogs.microsoft.com/devops/temporary-rollback-build-identities-can-access-advanced-security-read-alerts-again/)

### Other DevOps News

GitHub released a new REST API version (“2026-03-10”), the first calendar-based version with breaking changes. Integration owners should review breaking changes, then opt in explicitly using `X-GitHub-Api-Version: 2026-03-10` while validating response-shape assumptions and error handling. The default remains `2022-11-28` for at least 24 months if you do not set the header. GitHub also launched issue fields in public preview for select orgs: typed org-level metadata (up to 25 fields; single select/text/number/date) searchable across repositories, usable in Projects views, automatable via REST/GraphQL, and emitting webhooks (`field_added`, `field_removed`). If last week’s Projects features structured boards, issue fields structure the issue data itself for consistent queries and automation across repositories.

- [GitHub REST API Version 2026-03-10 Now Available](https://github.blog/changelog/2026-03-12-rest-api-version-2026-03-10-is-now-available)
- [''GitHub Issue Fields Public Preview: Structured Metadata for Issues''](https://github.blog/changelog/2026-03-12-issue-fields-structured-issue-metadata-is-in-public-preview)

Dependabot can now open PRs updating `.pre-commit-config.yaml` hook revisions when you set ecosystem `pre-commit` in `dependabot.yml`, supporting tag pins and commit SHA pins (preserving YAML formatting and skipping `local`/`meta` hooks). In JavaScript, an alpha “npmx” package browser launched to help evaluate npm packages (module format, install size, outdated dependency signals), which may help dependency due diligence despite being early.

- [Dependabot Now Supports Automatic Updates for pre-commit Hooks](https://github.blog/changelog/2026-03-10-dependabot-now-supports-pre-commit-hooks)
- [npmx Package Browser Released as Alpha to Improve npmjs Experience](https://www.devclass.com/devops/2026/03/09/npmx-package-browser-released-as-alpha-to-fix-pain-of-using-npmjs/4093605)

Microsoft Fabric added publishing SQL database schema changes from VS Code via SQL Database Projects, including a Publish dialog that browses Fabric workspaces/databases, previews the deployment script, and exposes options (including deletion behavior). It also adds templates for common objects and optional validation using a local SQL Server 2025 container. A Harness tutorial showed building a CI pipeline on AKS using Delegates and Connectors, with Secrets Manager (optionally Azure Key Vault-backed) storing service principal creds so Azure access stays within a governed connector and network boundary.

- [Deploy SQL Databases in Microsoft Fabric Directly from VS Code](https://blog.fabric.microsoft.com/en-US/blog/deploy-sql-databases-in-fabric-from-vs-code-no-more-context-switching/)
- [How to Create a Harness Pipeline and Integrate with Azure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/how-to-create-a-harness-pipeline-and-integrate-with-azure/ba-p/4499862)

A Behave tutorial showed structuring Python BDD suites in VS Code (feature files, steps, `environment.py` hooks, `behave.ini`, tagging) in a way that maps cleanly to CI. A platform engineering essay argued “human scale” coordination is the constraint. Tool sprawl across Kubernetes, observability, and CI/CD creates overhead, and the post recommends evolving platform interfaces while using AI assistants to surface institutional knowledge without building overly rigid platforms. It also connects to this week’s agent governance theme: adoption tends to hinge less on capability and more on standard interfaces, guardrails, and shared context for safe collaboration across complex stacks.

- [''Getting Started with Behave: Writing Cucumber Tests in VS Code''](https://techcommunity.microsoft.com/t5/microsoft-developer-community/getting-started-with-behave-writing-cucumber-tests-in-vs-code/ba-p/4496865)
- [The Human Scale Problem in Platform Engineering](https://devblogs.microsoft.com/all-things-azure/the-human-scale-problem-in-platform-engineering/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-03-16';

-- weekly-devops-roundup-2026-03-23  (2026-03-23)
UPDATE content_items
SET  content      = 'This week''s DevOps story split into two threads. GitHub tightened daily shipping and review mechanics (self-hosted runners, scheduling, review ergonomics, GHES governance), while Microsoft Fabric pushed "artifacts as code" with more Git-native workflows and REST APIs for repeatable promotion. Building on last week''s "operate safely at scale" theme (runner compliance, OIDC governance signals, reliability learnings), this week focuses on reducing friction once controls exist: clearer GHES merge feedback, more predictable runner targeting on Kubernetes, and more flexible scheduling and environment usage in Actions. On the Microsoft side, Fabric extends last week''s "deploy from VS Code / database projects" direction into bulk promotion, event-driven lifecycle automation, and Git-style review loops inside Fabric.

<!--excerpt_end-->

## DevOps

### GitHub Enterprise Server 3.20: tighter governance, safer releases, and backup planning

GitHub Enterprise Server 3.20 GA brings changes teams will notice in merge readiness, release integrity, and admin workflows. After last week''s push to move governance into platform controls instead of bespoke scripts, GHES 3.20 makes merge-time policy outcomes easier to see. The PR merge area is tuned for faster triage: required status checks are grouped with failures shown first, ordering is more predictable via natural sorting, and commit metadata rule failures show clearer merge-time errors so developers know what to fix. GitHub also notes accessibility improvements (keyboard navigation, focus handling, landmarks), which matter in keyboard-heavy review flows.

Release management adds immutable releases: after publishing, assets cannot be added, modified, or deleted and the tag cannot be moved or deleted, which reduces post-release tampering risk. A gap remains: release attestations still are not supported on GHES (GitHub.com only), so on-prem teams depending on attestations need compensating controls, which is another example of uneven platform capability across surfaces.

Secret scanning gained enterprise-scale improvements: validity checks can indicate whether secrets are still active; enterprise admins can manage exposure via the Management Console; bypass controls for push protection can be governed centrally; alert assignment supports collaborative triage; and default push protection expands to more secret types with new or improved detectors. It continues last week''s direction: security automation runs under governed roles and policies, not "whatever the pipeline can call." "Enterprise teams" also entered public preview as an enterprise-wide grouping model (API/UI managed, assign across orgs, use with custom enterprise roles, add to ruleset bypass lists). It is useful but worth staging due to preview limits and bypass-list implications. For Advanced Security, GHES adds an Enterprise Security Manager role (public preview) to centralize policy and alerts, with an enterprise size limit of 15,000 orgs.

The built-in backup service moved from public preview to GA as a managed alternative to older backup utilities, without requiring a separate backup-software host. It matches last week''s reliability posture: "operate safely at scale" includes customer-side DR readiness and simpler runbooks. GitHub also set a timeline: `backup-utils` begins retirement starting in GHES 3.22, giving admins time to evaluate the built-in service and update DR automation.

- [GitHub Enterprise Server 3.20 is now generally available](https://github.blog/changelog/2026-03-17-github-enterprise-server-3-20-is-now-generally-available)

### GitHub Actions on Kubernetes and in workflow YAML: runner scale sets and fewer scheduling papercuts

GitHub Actions self-hosted runners on Kubernetes got a practical update with Actions Runner Controller (ARC) 0.14.0 GA. It continues last week''s runner compliance and fleet hygiene narrative: once upgrades and image refreshes are routine, the next pain is operational sprawl from too many pools and labels. Multilabel Runner Scale Sets reduce that: one scale set can advertise multiple labels (OS, hardware tier, compliance zone, network placement), and workflows can target runners via combined `runs-on` attributes. ARC also standardized its API integration around the public `actions/scaleset` Go library, which is useful if you build autoscaling or automation and want to align with a supported client.

ARC 0.14.0 also adds metadata and policy knobs for orgs enforcing cluster hygiene, echoing last week''s "governance moves into primitives" trend. Helm charts expose a `resource` interface for applying custom labels and annotations to ARC-managed resources (Roles, RoleBindings, ServiceAccounts, listener pods), with global defaults via `resource.all.metadata` and per-resource overrides. Experimental chart rewrites ship alongside existing charts, aiming for cleaner templates, unified metadata config, and better Docker-based runner configuration.

Two runtime behaviors address common failure modes. The listener pod defaults to `nodeSelector: kubernetes.io/os: linux` to prevent accidental scheduling onto Windows nodes in mixed clusters (overrideable via `listenerTemplate`). ARC can also stop autoscaling for a runner set when runner configuration is outdated (when a runner exits code 7), avoiding repeatedly provisioning stale runners during rollouts; GitHub notes this depends on an upcoming runner change and will not be fully effective until a couple of runner releases after.

At the workflow YAML level, GitHub Actions addressed two recurring design issues. You can now reference an environment for environment-scoped secrets and variables without creating a deployment record by setting `deployment: false`, which is useful for tests, maintenance, and validations where you want environment governance without deployment bookkeeping. Limitation: repos with custom deployment protection rules cannot use `deployment: false`. The other update is cron timezone support: schedules can specify an IANA timezone instead of being fixed to UTC, which reduces DST-related confusion.

- [''Actions Runner Controller (ARC) 0.14.0: multilabel runner scale sets, new scaleset client, Helm and scheduling updates''](https://github.blog/changelog/2026-03-19-actions-runner-controller-release-0-14-0)
- [''GitHub Actions: Late March 2026 updates''](https://github.blog/changelog/2026-03-19-github-actions-late-march-2026-updates)

### Microsoft Fabric CI/CD: “definitions as code” gets more Git-native and more automatable

Fabric''s CI/CD surface expanded in preview with a consistent theme: treat workspace artifacts as code and make automation less special-case. Building on last week''s push to deploy SQL schema changes from VS Code, this extends from single-artifact publishing to more standardized promotion patterns across many artifact types, using Git-shaped delivery workflows.

For custom workload partners, the Fabric Extensibility Toolkit (GA) adds preview CI/CD support so custom workload items can use the same Git and promotion lifecycle as first-party items. Workload items are now included in Fabric Git integration commits (serialized with metadata and definitions), can be promoted through Deployment Pipelines across dev/test/prod, and can run stage-specific behavior via an optional deployment hook before applying the next-stage definition. Variable Library support reduces environment-specific rewiring (IDs, connections) by providing per-stage workspace values, so definitions reference variables instead of hard-coded IDs.

The same toolkit update adds automation integration points. The Remote Lifecycle Notification API (preview) provides webhooks for Created/Updated/Deleted events regardless of origin (UI, REST, CI/CD promotions, admin cleanups). That helps partner backends provision infrastructure, enforce licensing, or sync catalogs without polling. Fabric Scheduler support for "Remote Jobs" (preview) lets workload items define job types executed on schedules via backend endpoints, using delegated Entra OBO tokens so backends can act as the scheduling user when accessing Fabric APIs, OneLake, or other Entra-protected services, with status visible in Fabric job history.

For core automation, Fabric introduced preview REST APIs for Bulk Export and Bulk Import of item definitions. The intent is to export definitions at scale, version them in Git, validate via PR and policy, then bulk import into target workspaces as repeatable deployments. Bulk Import uses dependency handling to deploy in the correct order, and the APIs support enterprise patterns like async long-running operations and non-interactive auth (service principals/managed identities), which helps with CI/CD, DR, and large-scale promotion.

Fabric Git integration also gained preview developer experiences that make feature-branch workflows less awkward when "workspace is the branch." Branched Workspaces makes source/branch relationships explicit. Selective Branching lets you branch only intended items (pulling required related items automatically). Compare Code Changes adds diff-style review inside Fabric for outgoing changes pre-commit, incoming updates pre-sync, and conflict resolution with side-by-side context. Together, these features support focused workspaces, earlier review before repo commits, and promotion through standard Git and pipeline practices, which mirrors this week''s GHES merge UI improvements and review ergonomics.

- [''Fabric Extensibility Toolkit: CI/CD, Remote Lifecycle Notifications, and Fabric Scheduler (Preview)''](https://blog.fabric.microsoft.com/en-US/blog/fabric-extensibility-toolkit-ci-cd-remote-lifecycle-notifications-and-fabric-scheduler-preview/)
- [Introducing Bulk Export and Import APIs for CI/CD in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-bulk-export-and-import-apis-for-ci-cd-in-microsoft-fabric-preview/)
- [Introducing new Git developer experiences in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-new-git-developer-experiences-in-microsoft-fabric-preview/)

### Other DevOps News

GitHub''s PR review surface got a workflow update: docked side-by-side panels in "Files changed" keep PR overview, comment threads, merge status, and code scanning alerts visible next to the diff. That reduces tab switching when juggling merge readiness and security findings. Paired with GHES 3.20 merge-area improvements, it continues the practical theme of reducing UI friction around expanded checks and controls.

- [View code and comments side-by-side in pull request Files changed page](https://github.blog/changelog/2026-03-19-view-code-and-comments-side-by-side-in-pull-request-files-changed-page)

Data residency and mobile ergonomics also got incremental updates for distributed teams. Codespaces with data residency expanded public preview to Japan (joining EU and Australia), helping enterprises keep Codespaces data in-region and aligning with last week''s identity and governance controls. GitHub Mobile for Android updated navigation to keep bottom tabs consistent and preserve state better between Home/Inbox and related contexts, which helps on-the-go PR and notification triage.

- [Codespaces with data residency now available in Japan](https://github.blog/changelog/2026-03-19-codespaces-with-data-residency-now-available-in-japan)
- [A smoother navigation experience in GitHub Mobile for Android](https://github.blog/changelog/2026-03-20-a-smoother-navigation-experience-in-github-mobile-for-android)

A practical Git migration guide clarified syncing branches/tags (`git push --all` plus `--tags`) versus true mirroring (`git push --mirror`) that pushes all `refs/*` and deletes destination-only refs. It is useful during phased cutovers where CI and hosting systems may create extra refs you do not want wiped, and it complements last week''s theme that provider behaviors can create migration gotchas.

- [''Git Mirroring During Migrations: `--all` vs `--mirror`''](https://dev.to/playfulprogramming/git-mirroring-during-migrations-all-vs-mirror-2i4h)

Two GitHub Actions beginner resources aimed at helping teams reach first automation using issue-labeling workflows to teach events, jobs/steps, runners, permissions, and troubleshooting via logs. The written walkthrough reinforces least privilege (`issues: write`, `contents: read`), uses `actions/checkout@v6`, labels via `gh` using `GH_TOKEN` from `secrets.GITHUB_TOKEN`, and reminds you the target label must exist. It counterbalances last week''s enterprise operations focus by teaching permission and scope habits early.

- [How to use GitHub Actions | GitHub for Beginners](https://www.youtube.com/watch?v=BQrohJ3PT7I)
- [''GitHub for Beginners: Getting started with GitHub Actions''](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-actions/)

Microsoft shipped an operations-focused Fabric update: on-premises data gateway auto-update (admin triggered) is GA. After upgrading to November 2025 (or later), admins can trigger upgrades on demand for maintenance windows/change control and script cluster member updates via PowerShell (for example, `Update-DataGatewayClusterMember ...`) to standardize rollout. Alongside Fabric bulk import/export and Git-based promotion, it reinforces that repeatable deployments also depend on gateway infrastructure staying current in controlled windows.

- [On-premises data gateway auto-update (admin triggered) (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-auto-update-admin-triggered-generally-available/)

GitHub published maintainer guidance on how AI-assisted coding is shifting open source contributions, proposing a framework around "Comprehension, Context, and Continuity," plus process levers like issue-first gates, AI-use disclosure expectations, and repo-level agent guidance (AGENTS.md) to reduce review load while keeping mentorship sustainable. It connects to last week''s agent governance theme from a different angle: whether agents respond to incidents or submit code, teams need boundaries, expectations, and structured context for efficient human review.

- [Rethinking open source mentorship in the AI era](https://github.blog/open-source/maintainers/rethinking-open-source-mentorship-in-the-ai-era/)

VS Code highlighted an experimental "Agentic Browser Tools" capability that lets agent chat interact with the integrated browser (open pages, click UI, verify changes) to keep edit-run-verify loops in-editor. It continues last week''s point that assistants are easier to use when they fit existing verification patterns, reducing reliance on undocumented manual checks.

- [Agentic Browser Tools (Experimental) in VS Code](https://www.youtube.com/shorts/DWh7Izwu3wQ)',
     excerpt      = 'This week''s DevOps story split into two threads. GitHub tightened daily shipping and review mechanics (self-hosted runners, scheduling, review ergonomics, GHES governance), while Microsoft Fabric pushed "artifacts as code" with more Git-native workflows and REST APIs for repeatable promotion. Building on last week''s "operate safely at scale" theme (runner compliance, OIDC governance signals, reliability learnings), this week focuses on reducing friction once controls exist: clearer GHES merge feedback, more predictable runner targeting on Kubernetes, and more flexible scheduling and environment usage in Actions. On the Microsoft side, Fabric extends last week''s "deploy from VS Code / database projects" direction into bulk promotion, event-driven lifecycle automation, and Git-style review loops inside Fabric.',
     content_hash = md5('This week''s DevOps story split into two threads. GitHub tightened daily shipping and review mechanics (self-hosted runners, scheduling, review ergonomics, GHES governance), while Microsoft Fabric pushed "artifacts as code" with more Git-native workflows and REST APIs for repeatable promotion. Building on last week''s "operate safely at scale" theme (runner compliance, OIDC governance signals, reliability learnings), this week focuses on reducing friction once controls exist: clearer GHES merge feedback, more predictable runner targeting on Kubernetes, and more flexible scheduling and environment usage in Actions. On the Microsoft side, Fabric extends last week''s "deploy from VS Code / database projects" direction into bulk promotion, event-driven lifecycle automation, and Git-style review loops inside Fabric.

<!--excerpt_end-->

## DevOps

### GitHub Enterprise Server 3.20: tighter governance, safer releases, and backup planning

GitHub Enterprise Server 3.20 GA brings changes teams will notice in merge readiness, release integrity, and admin workflows. After last week''s push to move governance into platform controls instead of bespoke scripts, GHES 3.20 makes merge-time policy outcomes easier to see. The PR merge area is tuned for faster triage: required status checks are grouped with failures shown first, ordering is more predictable via natural sorting, and commit metadata rule failures show clearer merge-time errors so developers know what to fix. GitHub also notes accessibility improvements (keyboard navigation, focus handling, landmarks), which matter in keyboard-heavy review flows.

Release management adds immutable releases: after publishing, assets cannot be added, modified, or deleted and the tag cannot be moved or deleted, which reduces post-release tampering risk. A gap remains: release attestations still are not supported on GHES (GitHub.com only), so on-prem teams depending on attestations need compensating controls, which is another example of uneven platform capability across surfaces.

Secret scanning gained enterprise-scale improvements: validity checks can indicate whether secrets are still active; enterprise admins can manage exposure via the Management Console; bypass controls for push protection can be governed centrally; alert assignment supports collaborative triage; and default push protection expands to more secret types with new or improved detectors. It continues last week''s direction: security automation runs under governed roles and policies, not "whatever the pipeline can call." "Enterprise teams" also entered public preview as an enterprise-wide grouping model (API/UI managed, assign across orgs, use with custom enterprise roles, add to ruleset bypass lists). It is useful but worth staging due to preview limits and bypass-list implications. For Advanced Security, GHES adds an Enterprise Security Manager role (public preview) to centralize policy and alerts, with an enterprise size limit of 15,000 orgs.

The built-in backup service moved from public preview to GA as a managed alternative to older backup utilities, without requiring a separate backup-software host. It matches last week''s reliability posture: "operate safely at scale" includes customer-side DR readiness and simpler runbooks. GitHub also set a timeline: `backup-utils` begins retirement starting in GHES 3.22, giving admins time to evaluate the built-in service and update DR automation.

- [GitHub Enterprise Server 3.20 is now generally available](https://github.blog/changelog/2026-03-17-github-enterprise-server-3-20-is-now-generally-available)

### GitHub Actions on Kubernetes and in workflow YAML: runner scale sets and fewer scheduling papercuts

GitHub Actions self-hosted runners on Kubernetes got a practical update with Actions Runner Controller (ARC) 0.14.0 GA. It continues last week''s runner compliance and fleet hygiene narrative: once upgrades and image refreshes are routine, the next pain is operational sprawl from too many pools and labels. Multilabel Runner Scale Sets reduce that: one scale set can advertise multiple labels (OS, hardware tier, compliance zone, network placement), and workflows can target runners via combined `runs-on` attributes. ARC also standardized its API integration around the public `actions/scaleset` Go library, which is useful if you build autoscaling or automation and want to align with a supported client.

ARC 0.14.0 also adds metadata and policy knobs for orgs enforcing cluster hygiene, echoing last week''s "governance moves into primitives" trend. Helm charts expose a `resource` interface for applying custom labels and annotations to ARC-managed resources (Roles, RoleBindings, ServiceAccounts, listener pods), with global defaults via `resource.all.metadata` and per-resource overrides. Experimental chart rewrites ship alongside existing charts, aiming for cleaner templates, unified metadata config, and better Docker-based runner configuration.

Two runtime behaviors address common failure modes. The listener pod defaults to `nodeSelector: kubernetes.io/os: linux` to prevent accidental scheduling onto Windows nodes in mixed clusters (overrideable via `listenerTemplate`). ARC can also stop autoscaling for a runner set when runner configuration is outdated (when a runner exits code 7), avoiding repeatedly provisioning stale runners during rollouts; GitHub notes this depends on an upcoming runner change and will not be fully effective until a couple of runner releases after.

At the workflow YAML level, GitHub Actions addressed two recurring design issues. You can now reference an environment for environment-scoped secrets and variables without creating a deployment record by setting `deployment: false`, which is useful for tests, maintenance, and validations where you want environment governance without deployment bookkeeping. Limitation: repos with custom deployment protection rules cannot use `deployment: false`. The other update is cron timezone support: schedules can specify an IANA timezone instead of being fixed to UTC, which reduces DST-related confusion.

- [''Actions Runner Controller (ARC) 0.14.0: multilabel runner scale sets, new scaleset client, Helm and scheduling updates''](https://github.blog/changelog/2026-03-19-actions-runner-controller-release-0-14-0)
- [''GitHub Actions: Late March 2026 updates''](https://github.blog/changelog/2026-03-19-github-actions-late-march-2026-updates)

### Microsoft Fabric CI/CD: “definitions as code” gets more Git-native and more automatable

Fabric''s CI/CD surface expanded in preview with a consistent theme: treat workspace artifacts as code and make automation less special-case. Building on last week''s push to deploy SQL schema changes from VS Code, this extends from single-artifact publishing to more standardized promotion patterns across many artifact types, using Git-shaped delivery workflows.

For custom workload partners, the Fabric Extensibility Toolkit (GA) adds preview CI/CD support so custom workload items can use the same Git and promotion lifecycle as first-party items. Workload items are now included in Fabric Git integration commits (serialized with metadata and definitions), can be promoted through Deployment Pipelines across dev/test/prod, and can run stage-specific behavior via an optional deployment hook before applying the next-stage definition. Variable Library support reduces environment-specific rewiring (IDs, connections) by providing per-stage workspace values, so definitions reference variables instead of hard-coded IDs.

The same toolkit update adds automation integration points. The Remote Lifecycle Notification API (preview) provides webhooks for Created/Updated/Deleted events regardless of origin (UI, REST, CI/CD promotions, admin cleanups). That helps partner backends provision infrastructure, enforce licensing, or sync catalogs without polling. Fabric Scheduler support for "Remote Jobs" (preview) lets workload items define job types executed on schedules via backend endpoints, using delegated Entra OBO tokens so backends can act as the scheduling user when accessing Fabric APIs, OneLake, or other Entra-protected services, with status visible in Fabric job history.

For core automation, Fabric introduced preview REST APIs for Bulk Export and Bulk Import of item definitions. The intent is to export definitions at scale, version them in Git, validate via PR and policy, then bulk import into target workspaces as repeatable deployments. Bulk Import uses dependency handling to deploy in the correct order, and the APIs support enterprise patterns like async long-running operations and non-interactive auth (service principals/managed identities), which helps with CI/CD, DR, and large-scale promotion.

Fabric Git integration also gained preview developer experiences that make feature-branch workflows less awkward when "workspace is the branch." Branched Workspaces makes source/branch relationships explicit. Selective Branching lets you branch only intended items (pulling required related items automatically). Compare Code Changes adds diff-style review inside Fabric for outgoing changes pre-commit, incoming updates pre-sync, and conflict resolution with side-by-side context. Together, these features support focused workspaces, earlier review before repo commits, and promotion through standard Git and pipeline practices, which mirrors this week''s GHES merge UI improvements and review ergonomics.

- [''Fabric Extensibility Toolkit: CI/CD, Remote Lifecycle Notifications, and Fabric Scheduler (Preview)''](https://blog.fabric.microsoft.com/en-US/blog/fabric-extensibility-toolkit-ci-cd-remote-lifecycle-notifications-and-fabric-scheduler-preview/)
- [Introducing Bulk Export and Import APIs for CI/CD in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-bulk-export-and-import-apis-for-ci-cd-in-microsoft-fabric-preview/)
- [Introducing new Git developer experiences in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-new-git-developer-experiences-in-microsoft-fabric-preview/)

### Other DevOps News

GitHub''s PR review surface got a workflow update: docked side-by-side panels in "Files changed" keep PR overview, comment threads, merge status, and code scanning alerts visible next to the diff. That reduces tab switching when juggling merge readiness and security findings. Paired with GHES 3.20 merge-area improvements, it continues the practical theme of reducing UI friction around expanded checks and controls.

- [View code and comments side-by-side in pull request Files changed page](https://github.blog/changelog/2026-03-19-view-code-and-comments-side-by-side-in-pull-request-files-changed-page)

Data residency and mobile ergonomics also got incremental updates for distributed teams. Codespaces with data residency expanded public preview to Japan (joining EU and Australia), helping enterprises keep Codespaces data in-region and aligning with last week''s identity and governance controls. GitHub Mobile for Android updated navigation to keep bottom tabs consistent and preserve state better between Home/Inbox and related contexts, which helps on-the-go PR and notification triage.

- [Codespaces with data residency now available in Japan](https://github.blog/changelog/2026-03-19-codespaces-with-data-residency-now-available-in-japan)
- [A smoother navigation experience in GitHub Mobile for Android](https://github.blog/changelog/2026-03-20-a-smoother-navigation-experience-in-github-mobile-for-android)

A practical Git migration guide clarified syncing branches/tags (`git push --all` plus `--tags`) versus true mirroring (`git push --mirror`) that pushes all `refs/*` and deletes destination-only refs. It is useful during phased cutovers where CI and hosting systems may create extra refs you do not want wiped, and it complements last week''s theme that provider behaviors can create migration gotchas.

- [''Git Mirroring During Migrations: `--all` vs `--mirror`''](https://dev.to/playfulprogramming/git-mirroring-during-migrations-all-vs-mirror-2i4h)

Two GitHub Actions beginner resources aimed at helping teams reach first automation using issue-labeling workflows to teach events, jobs/steps, runners, permissions, and troubleshooting via logs. The written walkthrough reinforces least privilege (`issues: write`, `contents: read`), uses `actions/checkout@v6`, labels via `gh` using `GH_TOKEN` from `secrets.GITHUB_TOKEN`, and reminds you the target label must exist. It counterbalances last week''s enterprise operations focus by teaching permission and scope habits early.

- [How to use GitHub Actions | GitHub for Beginners](https://www.youtube.com/watch?v=BQrohJ3PT7I)
- [''GitHub for Beginners: Getting started with GitHub Actions''](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-actions/)

Microsoft shipped an operations-focused Fabric update: on-premises data gateway auto-update (admin triggered) is GA. After upgrading to November 2025 (or later), admins can trigger upgrades on demand for maintenance windows/change control and script cluster member updates via PowerShell (for example, `Update-DataGatewayClusterMember ...`) to standardize rollout. Alongside Fabric bulk import/export and Git-based promotion, it reinforces that repeatable deployments also depend on gateway infrastructure staying current in controlled windows.

- [On-premises data gateway auto-update (admin triggered) (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-auto-update-admin-triggered-generally-available/)

GitHub published maintainer guidance on how AI-assisted coding is shifting open source contributions, proposing a framework around "Comprehension, Context, and Continuity," plus process levers like issue-first gates, AI-use disclosure expectations, and repo-level agent guidance (AGENTS.md) to reduce review load while keeping mentorship sustainable. It connects to last week''s agent governance theme from a different angle: whether agents respond to incidents or submit code, teams need boundaries, expectations, and structured context for efficient human review.

- [Rethinking open source mentorship in the AI era](https://github.blog/open-source/maintainers/rethinking-open-source-mentorship-in-the-ai-era/)

VS Code highlighted an experimental "Agentic Browser Tools" capability that lets agent chat interact with the integrated browser (open pages, click UI, verify changes) to keep edit-run-verify loops in-editor. It continues last week''s point that assistants are easier to use when they fit existing verification patterns, reducing reliance on undocumented manual checks.

- [Agentic Browser Tools (Experimental) in VS Code](https://www.youtube.com/shorts/DWh7Izwu3wQ)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-03-23';

-- weekly-devops-roundup-2026-03-30  (2026-03-30)
UPDATE content_items
SET  content      = 'This week''s DevOps updates focused on making automation more repeatable and less fragile. Fabric kept closing "treat artifacts like code" gaps (Git, pipelines, environment promotion), while GitHub and VS Code shipped workflow improvements that reduce triage overhead and tighten feedback loops. Infrastructure teams also got a heads-up on Docker storage behavior changes and a pattern for turning Helm chart expectations into CI-enforced tests.

<!--excerpt_end-->

## DevOps

### Microsoft Fabric CI/CD and “everything as code” (CLI, Git, pipelines, and environment promotion)

Fabric updates focused on turning UI-heavy operations into versioned, promotable artifacts across dev/test/prod. Building on last week''s Git-shaped delivery push (bulk export/import, branched workspaces, selective branching, diff-style review), the Fabric Data Engineering API for GraphQL now supports source control and CI/CD as GA. GraphQL artifacts can now be committed, reviewed via PRs, and promoted using Azure DevOps or Fabric Deployment pipelines. The result is that API definition/config changes become diffable and rollback-friendly instead of portal-only state.

Fabric''s database DevOps tooling also moved toward an end-to-end workflow. Extending last week''s "deploy from VS Code / database projects" direction, updated guidance positions a SQL project (.sqlproj with Microsoft.Build.Sql) as the unit of versioning and deployment. Builds validate dependencies and produce a DACPAC; deployments generate a plan/script to align the target database. Fabric''s portal can connect a SQL database in Fabric to GitHub/Azure DevOps, generate a SQL project from the current schema, and commit it so changes can flow through PRs/pipelines. Two pipeline-relevant details: (1) pre/post-deployment scripts are supported in SQL database in Fabric (authored as Shared Queries and marked), and (2) tooling is converging. SSMS 22.4 adds a "Database DevOps" workload (preview) using the same project system as VS Code, while the VS Code MSSQL extension publish dialog is now GA and can show the equivalent SqlPackage command for CI reuse.

Automation gained more building blocks that fit last week''s "reduce special-case automation" theme. Fabric CLI v1.5 is GA with a new `deploy` command wrapping `fabric-cicd`, enabling full workspace deploys with one command (for example, `fab deploy --config deployment-config.yaml`) suitable for GitHub Actions/Azure DevOps. It also expands Power BI automation (rebinding, semantic model refresh scheduling/triggering), improves notebook execution (including running `fab` inside Fabric notebooks and parsing JSON), and adds agent-facing guardrails like `.ai-assets` and `AGENTS.md` to reduce invented flags in generated commands. A separate preview Azure DevOps Marketplace extension targets pipeline boilerplate by provisioning Fabric CLI via a `FabricCLI@1` task (PowerShell/PowerShell Core/Bash, version pinning), avoiding per-pipeline install scripting.

Notebook Git workflows also improved for promotion. Notebooks can now optionally commit/restore the Resources folder (Python modules, config files, small assets), with Fabric-side exclusion rules plus `.gitignore` support inside Resources. And Lakehouse auto-binding (preview) reduces manual rebinding of lakehouses across Git-connected workspaces by capturing config in a Fabric-managed `notebook-settings.json` (visible for auditing, not intended for manual editing).

- [API for GraphQL source control and CI/CD support (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/api-for-graphql-source-control-and-ci-cd-support-generally-available/)
- [''Batteries included: Database DevOps with SQL projects''](https://blog.fabric.microsoft.com/en-US/blog/batteries-included-database-devops-with-sql-projects/)
- [''Fabric CLI in Azure DevOps: automation without friction (Preview)''](https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-in-azure-devops-automation-without-friction-preview/)
- [Fabric CLI v1.5 is here (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-v1-5-is-here-generally-available/)
- [''Fabric Notebooks: Resources Folder Support in Git''](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-resources-folder-support-in-git/)
- [Fabric notebooks support Lakehouse auto-binding in Git (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-support-lakehouse-auto-binding-in-git-preview/)

### GitHub Actions and PR workflow ergonomics (runner images, agentic automation, review triage)

GitHub shipped changes targeting two friction sources: inconsistent CI environments and review/maintenance overhead. Building on last week''s runner fleet hygiene and Kubernetes scheduling improvements (ARC multilabel scale sets, safer listener defaults, controls to avoid stale runner reprovisioning), custom images for GitHub-hosted runners are now GA. Orgs can start from GitHub-curated images and bake in toolchains, dependencies, cert roots, and hardened config so workflows install less and fail less due to drift. The gains show up in performance (fewer setup steps) and governance (standardized approved versions). GitHub notes October 2025 preview users should continue without changes.

Agentic workflows are also being previewed as a way to define intent-driven automation in Markdown that runs in Actions via coding agents, for tasks like scheduled maintainer reports or proposing fixes for CI failures. Continuing last week''s theme of adding boundaries/context for AI contributions, Actions run summaries now show the exact agentic workflow Markdown used for a run, making auditing/debugging easier without jumping between pages.

For human review flow, GitHub''s pull requests dashboard is in public preview with an Inbox-style view (needs your review, needs fixing, ready to merge), saved views built from common queries, and richer search syntax (AND/OR, nested expressions) for cross-repo review queues. It follows last week''s PR ergonomics updates: as controls/checks expand, GitHub is also reducing "what do I review next?" overhead.

- [Custom images for GitHub-hosted runners are now generally available](https://github.blog/changelog/2026-03-26-custom-images-for-github-hosted-runners-are-now-generally-available)
- [Automate your repo with GitHub agentic workflows](https://www.youtube.com/shorts/XH8oKA-ZYbU)
- [View Agentic Workflow configs in the Actions run summary](https://github.blog/changelog/2026-03-26-view-agentic-workflow-configs-in-the-actions-run-summary)
- [New pull requests dashboard is in public preview](https://github.blog/changelog/2026-03-26-new-pull-requests-dashboard-is-in-public-preview)

### Other DevOps News

Fabric''s environment configuration story advanced for data integration, continuing last week''s Variable Library theme for environment rewiring during promotions. Dataflow Gen2 Variable Library integration is now GA, letting teams externalize environment-specific values (endpoints, IDs, paths, thresholds, flags) and resolve them at runtime instead of editing Power Query/M per environment. This supports promoting the same logic across dev/test/prod under CI/CD with centrally governed configuration.

- [''Dataflow Gen2: Variable Library integration in Microsoft Fabric (Generally Available)''](https://blog.fabric.microsoft.com/en-US/blog/dataflow-gen2-variable-library-integration-in-microsoft-fabric-generally-available/)

Gateway lifecycle automation became more pipeline-friendly with the Gateway PowerShell module reaching GA for on-premises and VNet data gateways. Following last week''s note on admin-triggered auto-update GA, this moves from "upgrade on demand" to "script upgrades and recovery as runbooks." New/updated cmdlets cover upgrade/recovery tasks like checking versions, pinning upgrade targets, polling update status, and restoring a cluster member, reducing reliance on portal workflows.

- [Gateway PowerShell module is now generally available, with new update and recovery commands](https://blog.fabric.microsoft.com/en-US/blog/powershell-module-for-gateways-with-expanded-automation-capabilities-generally-available/)

Container operators got a troubleshooting note for Docker Engine v29 on Linux. With containerd image store enabled by default on fresh installs, Docker''s `data-root` no longer prevents OS disk growth because images/snapshots go under `/var/lib/containerd`. The workaround is relocating/symlinking containerd storage to a data disk in addition to setting Docker''s `data-root`. This matters for build agents, VMSS workers, and batch nodes where OS disk saturation impacts availability, especially when standardizing CI environments (like GitHub runner custom images) and needing worker disk behavior to match assumptions.

- [''Docker Engine v29 on Linux: Why data-root No Longer Prevents OS Disk Growth (and How to Fix It)''](https://techcommunity.microsoft.com/t5/azure/docker-engine-v29-on-linux-why-data-root-no-longer-prevents-os/m-p/4504862#M22466)

A Helm chart testing pattern showed how to unit test charts with Terratest by rendering (`helm template`) with base + environment override values, unmarshalling into typed Kubernetes API objects in Go, and asserting on fields (labels/selectors, securityContext hardening, ingress/TLS, HPA bounds). Using typed structs reduces brittle YAML-path assertions, and the post includes an Azure DevOps pipeline pattern for running tests and publishing JUnit results. This aligns with last week''s "policy as enforceable primitives" direction: chart invariants can be enforced as tests rather than review-only expectations.

- [''Unit Testing Helm Charts with Terratest: A Pattern Guide for Type-Safe Validation''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unit-testing-helm-charts-with-terratest-a-pattern-guide-for-type/ba-p/4506165)

Two smaller workflow updates may still affect automation. VS Code Insiders 1.114 adds `${taskVar:name}` so task output (captured via problem matchers) can feed launch/debug configs, helping with dynamic ports/URLs. GitHub also added a repo setting to disable comments on individual commits (via REST and GraphQL), which may require adjustments if bots/CI currently post commit-level comments instead of PR comments, especially as teams reduce review noise with improved PR dashboards and review surfaces.

- [''Visual Studio Code 1.114 (Insiders): pinned chat sessions and new task variables''](https://code.visualstudio.com/updates/v1_114)
- [Disable comments on individual commits (GitHub repository setting)](https://github.blog/changelog/2026-03-25-disable-comments-on-individual-commits)',
     excerpt      = 'This week''s DevOps updates focused on making automation more repeatable and less fragile. Fabric kept closing "treat artifacts like code" gaps (Git, pipelines, environment promotion), while GitHub and VS Code shipped workflow improvements that reduce triage overhead and tighten feedback loops. Infrastructure teams also got a heads-up on Docker storage behavior changes and a pattern for turning Helm chart expectations into CI-enforced tests.',
     content_hash = md5('This week''s DevOps updates focused on making automation more repeatable and less fragile. Fabric kept closing "treat artifacts like code" gaps (Git, pipelines, environment promotion), while GitHub and VS Code shipped workflow improvements that reduce triage overhead and tighten feedback loops. Infrastructure teams also got a heads-up on Docker storage behavior changes and a pattern for turning Helm chart expectations into CI-enforced tests.

<!--excerpt_end-->

## DevOps

### Microsoft Fabric CI/CD and “everything as code” (CLI, Git, pipelines, and environment promotion)

Fabric updates focused on turning UI-heavy operations into versioned, promotable artifacts across dev/test/prod. Building on last week''s Git-shaped delivery push (bulk export/import, branched workspaces, selective branching, diff-style review), the Fabric Data Engineering API for GraphQL now supports source control and CI/CD as GA. GraphQL artifacts can now be committed, reviewed via PRs, and promoted using Azure DevOps or Fabric Deployment pipelines. The result is that API definition/config changes become diffable and rollback-friendly instead of portal-only state.

Fabric''s database DevOps tooling also moved toward an end-to-end workflow. Extending last week''s "deploy from VS Code / database projects" direction, updated guidance positions a SQL project (.sqlproj with Microsoft.Build.Sql) as the unit of versioning and deployment. Builds validate dependencies and produce a DACPAC; deployments generate a plan/script to align the target database. Fabric''s portal can connect a SQL database in Fabric to GitHub/Azure DevOps, generate a SQL project from the current schema, and commit it so changes can flow through PRs/pipelines. Two pipeline-relevant details: (1) pre/post-deployment scripts are supported in SQL database in Fabric (authored as Shared Queries and marked), and (2) tooling is converging. SSMS 22.4 adds a "Database DevOps" workload (preview) using the same project system as VS Code, while the VS Code MSSQL extension publish dialog is now GA and can show the equivalent SqlPackage command for CI reuse.

Automation gained more building blocks that fit last week''s "reduce special-case automation" theme. Fabric CLI v1.5 is GA with a new `deploy` command wrapping `fabric-cicd`, enabling full workspace deploys with one command (for example, `fab deploy --config deployment-config.yaml`) suitable for GitHub Actions/Azure DevOps. It also expands Power BI automation (rebinding, semantic model refresh scheduling/triggering), improves notebook execution (including running `fab` inside Fabric notebooks and parsing JSON), and adds agent-facing guardrails like `.ai-assets` and `AGENTS.md` to reduce invented flags in generated commands. A separate preview Azure DevOps Marketplace extension targets pipeline boilerplate by provisioning Fabric CLI via a `FabricCLI@1` task (PowerShell/PowerShell Core/Bash, version pinning), avoiding per-pipeline install scripting.

Notebook Git workflows also improved for promotion. Notebooks can now optionally commit/restore the Resources folder (Python modules, config files, small assets), with Fabric-side exclusion rules plus `.gitignore` support inside Resources. And Lakehouse auto-binding (preview) reduces manual rebinding of lakehouses across Git-connected workspaces by capturing config in a Fabric-managed `notebook-settings.json` (visible for auditing, not intended for manual editing).

- [API for GraphQL source control and CI/CD support (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/api-for-graphql-source-control-and-ci-cd-support-generally-available/)
- [''Batteries included: Database DevOps with SQL projects''](https://blog.fabric.microsoft.com/en-US/blog/batteries-included-database-devops-with-sql-projects/)
- [''Fabric CLI in Azure DevOps: automation without friction (Preview)''](https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-in-azure-devops-automation-without-friction-preview/)
- [Fabric CLI v1.5 is here (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-v1-5-is-here-generally-available/)
- [''Fabric Notebooks: Resources Folder Support in Git''](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-resources-folder-support-in-git/)
- [Fabric notebooks support Lakehouse auto-binding in Git (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-support-lakehouse-auto-binding-in-git-preview/)

### GitHub Actions and PR workflow ergonomics (runner images, agentic automation, review triage)

GitHub shipped changes targeting two friction sources: inconsistent CI environments and review/maintenance overhead. Building on last week''s runner fleet hygiene and Kubernetes scheduling improvements (ARC multilabel scale sets, safer listener defaults, controls to avoid stale runner reprovisioning), custom images for GitHub-hosted runners are now GA. Orgs can start from GitHub-curated images and bake in toolchains, dependencies, cert roots, and hardened config so workflows install less and fail less due to drift. The gains show up in performance (fewer setup steps) and governance (standardized approved versions). GitHub notes October 2025 preview users should continue without changes.

Agentic workflows are also being previewed as a way to define intent-driven automation in Markdown that runs in Actions via coding agents, for tasks like scheduled maintainer reports or proposing fixes for CI failures. Continuing last week''s theme of adding boundaries/context for AI contributions, Actions run summaries now show the exact agentic workflow Markdown used for a run, making auditing/debugging easier without jumping between pages.

For human review flow, GitHub''s pull requests dashboard is in public preview with an Inbox-style view (needs your review, needs fixing, ready to merge), saved views built from common queries, and richer search syntax (AND/OR, nested expressions) for cross-repo review queues. It follows last week''s PR ergonomics updates: as controls/checks expand, GitHub is also reducing "what do I review next?" overhead.

- [Custom images for GitHub-hosted runners are now generally available](https://github.blog/changelog/2026-03-26-custom-images-for-github-hosted-runners-are-now-generally-available)
- [Automate your repo with GitHub agentic workflows](https://www.youtube.com/shorts/XH8oKA-ZYbU)
- [View Agentic Workflow configs in the Actions run summary](https://github.blog/changelog/2026-03-26-view-agentic-workflow-configs-in-the-actions-run-summary)
- [New pull requests dashboard is in public preview](https://github.blog/changelog/2026-03-26-new-pull-requests-dashboard-is-in-public-preview)

### Other DevOps News

Fabric''s environment configuration story advanced for data integration, continuing last week''s Variable Library theme for environment rewiring during promotions. Dataflow Gen2 Variable Library integration is now GA, letting teams externalize environment-specific values (endpoints, IDs, paths, thresholds, flags) and resolve them at runtime instead of editing Power Query/M per environment. This supports promoting the same logic across dev/test/prod under CI/CD with centrally governed configuration.

- [''Dataflow Gen2: Variable Library integration in Microsoft Fabric (Generally Available)''](https://blog.fabric.microsoft.com/en-US/blog/dataflow-gen2-variable-library-integration-in-microsoft-fabric-generally-available/)

Gateway lifecycle automation became more pipeline-friendly with the Gateway PowerShell module reaching GA for on-premises and VNet data gateways. Following last week''s note on admin-triggered auto-update GA, this moves from "upgrade on demand" to "script upgrades and recovery as runbooks." New/updated cmdlets cover upgrade/recovery tasks like checking versions, pinning upgrade targets, polling update status, and restoring a cluster member, reducing reliance on portal workflows.

- [Gateway PowerShell module is now generally available, with new update and recovery commands](https://blog.fabric.microsoft.com/en-US/blog/powershell-module-for-gateways-with-expanded-automation-capabilities-generally-available/)

Container operators got a troubleshooting note for Docker Engine v29 on Linux. With containerd image store enabled by default on fresh installs, Docker''s `data-root` no longer prevents OS disk growth because images/snapshots go under `/var/lib/containerd`. The workaround is relocating/symlinking containerd storage to a data disk in addition to setting Docker''s `data-root`. This matters for build agents, VMSS workers, and batch nodes where OS disk saturation impacts availability, especially when standardizing CI environments (like GitHub runner custom images) and needing worker disk behavior to match assumptions.

- [''Docker Engine v29 on Linux: Why data-root No Longer Prevents OS Disk Growth (and How to Fix It)''](https://techcommunity.microsoft.com/t5/azure/docker-engine-v29-on-linux-why-data-root-no-longer-prevents-os/m-p/4504862#M22466)

A Helm chart testing pattern showed how to unit test charts with Terratest by rendering (`helm template`) with base + environment override values, unmarshalling into typed Kubernetes API objects in Go, and asserting on fields (labels/selectors, securityContext hardening, ingress/TLS, HPA bounds). Using typed structs reduces brittle YAML-path assertions, and the post includes an Azure DevOps pipeline pattern for running tests and publishing JUnit results. This aligns with last week''s "policy as enforceable primitives" direction: chart invariants can be enforced as tests rather than review-only expectations.

- [''Unit Testing Helm Charts with Terratest: A Pattern Guide for Type-Safe Validation''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unit-testing-helm-charts-with-terratest-a-pattern-guide-for-type/ba-p/4506165)

Two smaller workflow updates may still affect automation. VS Code Insiders 1.114 adds `${taskVar:name}` so task output (captured via problem matchers) can feed launch/debug configs, helping with dynamic ports/URLs. GitHub also added a repo setting to disable comments on individual commits (via REST and GraphQL), which may require adjustments if bots/CI currently post commit-level comments instead of PR comments, especially as teams reduce review noise with improved PR dashboards and review surfaces.

- [''Visual Studio Code 1.114 (Insiders): pinned chat sessions and new task variables''](https://code.visualstudio.com/updates/v1_114)
- [Disable comments on individual commits (GitHub repository setting)](https://github.blog/changelog/2026-03-25-disable-comments-on-individual-commits)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-03-30';

-- weekly-devops-roundup-2026-04-06  (2026-04-06)
UPDATE content_items
SET  content      = 'This week’s DevOps items covered familiar platform concerns: securing CI/CD without extra secrets, making dev environments workable in regulated orgs, and tightening everyday feedback loops. Longer write-ups also looked at operational scale, including cross-cloud incident investigation with agent tooling, release pipeline reliability, and the realities of rendering very large diffs.

<!--excerpt_end-->

## DevOps

### GitHub Actions and GitHub platform updates for CI/CD and governance

GitHub Actions’ early April updates reduced friction in common workflows while tightening cloud-access security controls. Workflow authors can now override a service container’s defaults in YAML using new `entrypoint` and `command` keys under `jobs.<job_id>.services`, similar to Docker Compose, which avoids forking images just to change startup flags. OIDC tokens now include repository custom properties as claims (GA), which enables cloud trust policies tied to governance metadata like `environment`, `owning_team`, or `compliance_tier` instead of long repo allowlists. This builds on last week’s CI-hardening theme: once runner environments are more consistent, identity assumption becomes the next control. For orgs using Azure private networking with GitHub-hosted runners, VNet failover networks (public preview) add resilience by letting you configure a secondary subnet (optionally cross-region) and fail over manually (UI/REST API) or automatically, with audit log and email notifications.

Across the GitHub platform, improved Issues search is now GA with semantic and hybrid modes for titles/bodies. You can use natural-language queries in the UI (repo-scoped or across the Issues dashboard), while tools can call REST `/search/issues` with `search_type=semantic|hybrid` or GraphQL `searchType`. This fits last week’s "review triage and queue management" direction: better discovery helps keep operational work searchable as CI events and bot signals grow. Operationally, semantic/hybrid is rate-limited to 10 requests/minute, so bots and dashboards need to budget; filter-only and quoted searches remain lexical.

- [GitHub Actions: Early April 2026 updates](https://github.blog/changelog/2026-04-02-github-actions-early-april-2026-updates)
- [Improved search for GitHub Issues is now generally available](https://github.blog/changelog/2026-04-02-improved-search-for-github-issues-is-now-generally-available)

### GitHub enterprise developer environments and supply chain automation

Two GitHub releases landed in platform-team territory: enterprise dev environments and dependency hygiene automation. Codespaces is now GA for GitHub Enterprise Cloud with data residency (Australia, EU, Japan, US) and feature parity with standard Codespaces. The key constraint is ownership: only enterprise- or organization-owned codespaces are supported (no user-owned), so admins must set Codespaces policies for compliant provisioning/billing while preserving "devcontainer in minutes" workflows. This continues last week’s push for repeatable environments (runner images in CI) with a "standardize dev" path where workstation variance and data locality have blocked adoption.

Dependabot also added support for SwiftPM dependencies managed inside Xcode bundles, for repos storing config in `.xcodeproj`/`.xcworkspace` rather than top-level `Package.swift`. It can discover `Package.resolved` inside Xcode bundle layouts, read SwiftPM rules from `project.pbxproj`, and open PRs updating the right resolved files. It keeps the existing `dependabot.yml` model (schedules, grouping, ignores). GHES support is planned for 3.22.

- [Codespaces is now generally available for GitHub Enterprise with data residency](https://github.blog/changelog/2026-04-01-codespaces-is-now-generally-available-for-github-enterprise-with-data-residency)
- [Dependabot now supports Xcode projects using SwiftPM with .xcodeproj manifests](https://github.blog/changelog/2026-03-31-dependabot-now-supports-xcode-projects-using-swiftpm-with-xcodeproj-manifests)

### Azure DevOps and Azure operations: publishing automation, work tracking UX, and cross-cloud investigations via MCP

Azure DevOps extension publishing automation got a refresh with the azdo-marketplace v6 rebuild. v6 consolidates multiple tasks into one task/action using an `operation` parameter (`package`, `publish`, `install`, `share`, `unpublish`, various `wait-*` gates), aligns behavior across Azure Pipelines and GitHub Actions, and reduces distribution size (to ~20 MB from ~300 MB) while adding extensive tests and cross-platform CI. A key security improvement is first-class GitHub Actions OIDC support (workload identity federation) to Azure DevOps, which reduces reliance on PATs. PAT/basic auth remain for compatibility, but the direction favors federated identity and service connections, continuing last week’s "reduce secret sprawl" theme for extension publishing supply chains.

Azure Boards is also rolling out a Markdown editor UX change aimed at reducing accidental edits in large text fields. Fields default to preview mode, editing is explicit via an edit icon, and "done" returns to preview. It targets triage flows where double-clicking to read/select text used to create unintended edits, which fits last week’s "reduce review noise" thread (PR dashboards, comment controls).

A deep operations guide showed cross-cloud investigations from one Azure SRE Agent chat by connecting Azure SRE Agent (Azure portal) to AWS via the AWS MCP Server and MCP Proxy for AWS. The setup is lightweight: Azure SRE Agent launches a local stdio connector via `uvx` (Astral `uv`), and the proxy forwards HTTPS to an AWS MCP endpoint (for example, `https://aws-mcp.us-east-1.api.aws/mcp`) with SigV4 signing using IAM creds from environment variables, with no container and no additional hosted infrastructure. This matches the "make ops repeatable and auditable" theme: turn investigations into tool calls rather than portal clicks. Once connected, AWS MCP Server exposes 23 MCP tools (docs lookups, authenticated AWS API execution with validation/error handling, guided Agent SOPs aligned to Well-Architected, and AWS DevOps Agent operations). The guide covers IAM setup (`aws-mcp:InvokeMcp`, `aws-mcp:CallReadOnlyTool`, optionally `aws-mcp:CallReadWriteTool`, plus service permissions), Azure SRE Agent skill configuration, and troubleshooting (403s from missing permissions, 401s from rotated keys, and restart/redeploy needs because MCP connections initialize at startup). It also highlights using AWS DevOps Agent tools (AgentSpace management, investigation/task lifecycle, journal/recommendations, evaluations, chat) alongside Azure telemetry for a unified RCA and remediation plan.

- [Introducing Azure DevOps Marketplace tasks and actions: A Complete Rebuild for Speed, Stability, and Security](https://jessehouwing.net/azure-devops-marketplace-tasks-and-actions/)
- [Improving the Markdown Editor for Work Items](https://devblogs.microsoft.com/devops/improving-the-markdown-editor-for-work-items/)
- [Announcing AWS with Azure SRE Agent: Cross-Cloud Investigation using the brand new AWS DevOps Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-aws-with-azure-sre-agent-cross-cloud-investigation/ba-p/4507413)

### Other DevOps News

IaC workflows got a CI-friendly drift-detection recipe designed for human governance: generate deterministic plan artifacts (`terraform plan -out=tfplan`, `terraform apply tfplan`), add a drift gate with `terraform plan -refresh-only -detailed-exitcode` (0 no drift, 2 drift, 1 error), and use Azure Resource Graph and Azure Policy queries to understand changes and compliance slips. Copilot is framed as helpful for summarizing/triaging noisy outputs (plans, KQL, policy states) without replacing RBAC or approvals. This matches last week’s "repeatable primitives enforce expectations" theme: drift checks and deterministic plans turn "we should notice changes" into a predictable gate.

- [AI‑Assisted Azure Infrastructure Validation and Drift Detection](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-assisted-azure-infrastructure-validation-and-drift-detection/ba-p/4508346)

Release engineering reliability showed up in the PowerShell 7.6 postmortem, explaining why the March 2026 release slipped: late packaging/tooling changes, weaker preview signal, cross-platform validation gaps, and distro constraints (Alpine packaging, RHEL 8 glibc 2.28). For teams depending on distribution channels, the concrete scope is useful: 29 packages across 8 formats, 4 architectures, 8 OSes, published via GitHub, PMC, winget, Store, NuGet. Planned investments include clearer release ownership, better tracking/communication, more consistent previews, simplified packaging, and fewer manual publishing steps, which matches last week’s theme of reducing fragile automation in high-coordination processes.

- [PowerShell 7.6 release postmortem and investments](https://devblogs.microsoft.com/powershell/powershell-7-6-release-postmortem/)

GitHub’s diff rendering performance deep dive is a reminder that developer experience often comes down to systems work. By reducing per-line DOM/component/handler overhead, using event delegation, hiding heavy state behind conditional rendering, enforcing O(1) indexed state access, and adding TanStack Virtual windowing, GitHub improved responsiveness for PRs with 10,000+ lines. It is the engineering counterpart to last week’s workflow UX updates: as triage UX expands, pages still need to stay fast under real repo diff sizes. These patterns translate to many React list/table UIs: simplify per-row structure, avoid per-item effects/handlers, and virtualize when full rendering becomes the bottleneck.

- [The uphill climb of making diff lines performant](https://github.blog/engineering/architecture-optimization/the-uphill-climb-of-making-diff-lines-performant/)',
     excerpt      = 'This week’s DevOps items covered familiar platform concerns: securing CI/CD without extra secrets, making dev environments workable in regulated orgs, and tightening everyday feedback loops. Longer write-ups also looked at operational scale, including cross-cloud incident investigation with agent tooling, release pipeline reliability, and the realities of rendering very large diffs.',
     content_hash = md5('This week’s DevOps items covered familiar platform concerns: securing CI/CD without extra secrets, making dev environments workable in regulated orgs, and tightening everyday feedback loops. Longer write-ups also looked at operational scale, including cross-cloud incident investigation with agent tooling, release pipeline reliability, and the realities of rendering very large diffs.

<!--excerpt_end-->

## DevOps

### GitHub Actions and GitHub platform updates for CI/CD and governance

GitHub Actions’ early April updates reduced friction in common workflows while tightening cloud-access security controls. Workflow authors can now override a service container’s defaults in YAML using new `entrypoint` and `command` keys under `jobs.<job_id>.services`, similar to Docker Compose, which avoids forking images just to change startup flags. OIDC tokens now include repository custom properties as claims (GA), which enables cloud trust policies tied to governance metadata like `environment`, `owning_team`, or `compliance_tier` instead of long repo allowlists. This builds on last week’s CI-hardening theme: once runner environments are more consistent, identity assumption becomes the next control. For orgs using Azure private networking with GitHub-hosted runners, VNet failover networks (public preview) add resilience by letting you configure a secondary subnet (optionally cross-region) and fail over manually (UI/REST API) or automatically, with audit log and email notifications.

Across the GitHub platform, improved Issues search is now GA with semantic and hybrid modes for titles/bodies. You can use natural-language queries in the UI (repo-scoped or across the Issues dashboard), while tools can call REST `/search/issues` with `search_type=semantic|hybrid` or GraphQL `searchType`. This fits last week’s "review triage and queue management" direction: better discovery helps keep operational work searchable as CI events and bot signals grow. Operationally, semantic/hybrid is rate-limited to 10 requests/minute, so bots and dashboards need to budget; filter-only and quoted searches remain lexical.

- [GitHub Actions: Early April 2026 updates](https://github.blog/changelog/2026-04-02-github-actions-early-april-2026-updates)
- [Improved search for GitHub Issues is now generally available](https://github.blog/changelog/2026-04-02-improved-search-for-github-issues-is-now-generally-available)

### GitHub enterprise developer environments and supply chain automation

Two GitHub releases landed in platform-team territory: enterprise dev environments and dependency hygiene automation. Codespaces is now GA for GitHub Enterprise Cloud with data residency (Australia, EU, Japan, US) and feature parity with standard Codespaces. The key constraint is ownership: only enterprise- or organization-owned codespaces are supported (no user-owned), so admins must set Codespaces policies for compliant provisioning/billing while preserving "devcontainer in minutes" workflows. This continues last week’s push for repeatable environments (runner images in CI) with a "standardize dev" path where workstation variance and data locality have blocked adoption.

Dependabot also added support for SwiftPM dependencies managed inside Xcode bundles, for repos storing config in `.xcodeproj`/`.xcworkspace` rather than top-level `Package.swift`. It can discover `Package.resolved` inside Xcode bundle layouts, read SwiftPM rules from `project.pbxproj`, and open PRs updating the right resolved files. It keeps the existing `dependabot.yml` model (schedules, grouping, ignores). GHES support is planned for 3.22.

- [Codespaces is now generally available for GitHub Enterprise with data residency](https://github.blog/changelog/2026-04-01-codespaces-is-now-generally-available-for-github-enterprise-with-data-residency)
- [Dependabot now supports Xcode projects using SwiftPM with .xcodeproj manifests](https://github.blog/changelog/2026-03-31-dependabot-now-supports-xcode-projects-using-swiftpm-with-xcodeproj-manifests)

### Azure DevOps and Azure operations: publishing automation, work tracking UX, and cross-cloud investigations via MCP

Azure DevOps extension publishing automation got a refresh with the azdo-marketplace v6 rebuild. v6 consolidates multiple tasks into one task/action using an `operation` parameter (`package`, `publish`, `install`, `share`, `unpublish`, various `wait-*` gates), aligns behavior across Azure Pipelines and GitHub Actions, and reduces distribution size (to ~20 MB from ~300 MB) while adding extensive tests and cross-platform CI. A key security improvement is first-class GitHub Actions OIDC support (workload identity federation) to Azure DevOps, which reduces reliance on PATs. PAT/basic auth remain for compatibility, but the direction favors federated identity and service connections, continuing last week’s "reduce secret sprawl" theme for extension publishing supply chains.

Azure Boards is also rolling out a Markdown editor UX change aimed at reducing accidental edits in large text fields. Fields default to preview mode, editing is explicit via an edit icon, and "done" returns to preview. It targets triage flows where double-clicking to read/select text used to create unintended edits, which fits last week’s "reduce review noise" thread (PR dashboards, comment controls).

A deep operations guide showed cross-cloud investigations from one Azure SRE Agent chat by connecting Azure SRE Agent (Azure portal) to AWS via the AWS MCP Server and MCP Proxy for AWS. The setup is lightweight: Azure SRE Agent launches a local stdio connector via `uvx` (Astral `uv`), and the proxy forwards HTTPS to an AWS MCP endpoint (for example, `https://aws-mcp.us-east-1.api.aws/mcp`) with SigV4 signing using IAM creds from environment variables, with no container and no additional hosted infrastructure. This matches the "make ops repeatable and auditable" theme: turn investigations into tool calls rather than portal clicks. Once connected, AWS MCP Server exposes 23 MCP tools (docs lookups, authenticated AWS API execution with validation/error handling, guided Agent SOPs aligned to Well-Architected, and AWS DevOps Agent operations). The guide covers IAM setup (`aws-mcp:InvokeMcp`, `aws-mcp:CallReadOnlyTool`, optionally `aws-mcp:CallReadWriteTool`, plus service permissions), Azure SRE Agent skill configuration, and troubleshooting (403s from missing permissions, 401s from rotated keys, and restart/redeploy needs because MCP connections initialize at startup). It also highlights using AWS DevOps Agent tools (AgentSpace management, investigation/task lifecycle, journal/recommendations, evaluations, chat) alongside Azure telemetry for a unified RCA and remediation plan.

- [Introducing Azure DevOps Marketplace tasks and actions: A Complete Rebuild for Speed, Stability, and Security](https://jessehouwing.net/azure-devops-marketplace-tasks-and-actions/)
- [Improving the Markdown Editor for Work Items](https://devblogs.microsoft.com/devops/improving-the-markdown-editor-for-work-items/)
- [Announcing AWS with Azure SRE Agent: Cross-Cloud Investigation using the brand new AWS DevOps Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-aws-with-azure-sre-agent-cross-cloud-investigation/ba-p/4507413)

### Other DevOps News

IaC workflows got a CI-friendly drift-detection recipe designed for human governance: generate deterministic plan artifacts (`terraform plan -out=tfplan`, `terraform apply tfplan`), add a drift gate with `terraform plan -refresh-only -detailed-exitcode` (0 no drift, 2 drift, 1 error), and use Azure Resource Graph and Azure Policy queries to understand changes and compliance slips. Copilot is framed as helpful for summarizing/triaging noisy outputs (plans, KQL, policy states) without replacing RBAC or approvals. This matches last week’s "repeatable primitives enforce expectations" theme: drift checks and deterministic plans turn "we should notice changes" into a predictable gate.

- [AI‑Assisted Azure Infrastructure Validation and Drift Detection](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-assisted-azure-infrastructure-validation-and-drift-detection/ba-p/4508346)

Release engineering reliability showed up in the PowerShell 7.6 postmortem, explaining why the March 2026 release slipped: late packaging/tooling changes, weaker preview signal, cross-platform validation gaps, and distro constraints (Alpine packaging, RHEL 8 glibc 2.28). For teams depending on distribution channels, the concrete scope is useful: 29 packages across 8 formats, 4 architectures, 8 OSes, published via GitHub, PMC, winget, Store, NuGet. Planned investments include clearer release ownership, better tracking/communication, more consistent previews, simplified packaging, and fewer manual publishing steps, which matches last week’s theme of reducing fragile automation in high-coordination processes.

- [PowerShell 7.6 release postmortem and investments](https://devblogs.microsoft.com/powershell/powershell-7-6-release-postmortem/)

GitHub’s diff rendering performance deep dive is a reminder that developer experience often comes down to systems work. By reducing per-line DOM/component/handler overhead, using event delegation, hiding heavy state behind conditional rendering, enforcing O(1) indexed state access, and adding TanStack Virtual windowing, GitHub improved responsiveness for PRs with 10,000+ lines. It is the engineering counterpart to last week’s workflow UX updates: as triage UX expands, pages still need to stay fast under real repo diff sizes. These patterns translate to many React list/table UIs: simplify per-row structure, avoid per-item effects/handlers, and virtualize when full rendering becomes the bottleneck.

- [The uphill climb of making diff lines performant](https://github.blog/engineering/architecture-optimization/the-uphill-climb-of-making-diff-lines-performant/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-04-06';

-- weekly-devops-roundup-2026-04-13  (2026-04-13)
UPDATE content_items
SET  content      = 'This week''s DevOps updates centered on practical CI/CD and dependency-maintenance mechanics on GitHub, plus more shift-left thinking for cost control and incident response that often involves agents. Alongside platform changes, guides also focused on making agent workflows safer on laptops and more accountable in IaC pull requests.

<!--excerpt_end-->

## DevOps

### GitHub Actions, Dependabot, and platform reliability: tighter guardrails and broader ecosystem support

Building on last week''s Actions work (less CI friction, tighter security via OIDC claims), GitHub added a limit that affects retry-heavy pipelines: a workflow run can be rerun at most 50 times, whether rerunning all jobs or selected jobs. After the 50th rerun, GitHub returns a failed check suite with an annotation that the limit was reached. If bots or scripts auto-rerun until green, update logic to stop before the cap and consider alternatives like backoff/jitter, narrowing retries to specific steps, or starting a fresh run. This supports last week''s reliability theme by nudging teams to engineer reliability rather than relying on unlimited reruns.

Dependabot continued last week''s ecosystem expansion with support for Nix flakes in version updates. By adding `nix` in `.github/dependabot.yml`, Dependabot can monitor `flake.lock` inputs and open one PR per outdated flake input as upstream Git refs advance (GitHub, GitLab, SourceHut, or generic `git` URLs). The key caveat remains that this is version updates only. Dependabot security updates still do not apply to Nix flakes, so vulnerability-driven automation needs a separate approach for Nix setups.

GitHub''s March 2026 availability report reinforced why fallbacks matter, complementing last week''s "keep platform usable at scale" theme. It covers incidents affecting github.com and the API (including a cache-write bug causing widespread expiry and cascading load), Actions scheduling delays and infra errors (Redis load balancer misconfig during resiliency updates), Copilot Coding Agent session failures (auth issues to backing datastore, mitigated by credential rotation, then recurring due to incomplete remediation), and Teams integration delivery failures due to an upstream outage. The actionable DevOps takeaway is to treat platform delays as a distinct failure mode: monitor pipeline SLAs, adjust expectations during incidents, and keep alternate notification paths when integrations break.

- [GitHub Actions workflows are limited to 50 reruns](https://github.blog/changelog/2026-04-10-actions-workflows-are-limited-to-50-reruns)
- [Dependabot version updates now support the Nix ecosystem](https://github.blog/changelog/2026-04-07-dependabot-version-updates-now-support-the-nix-ecosystem)
- [GitHub availability report: March 2026](https://github.blog/news-insights/company-news/github-availability-report-march-2026/)

### Azure cost-aware IaC pipelines and agentic operations: shifting governance earlier and into runtime

Last week emphasized more repeatable infrastructure operations (deterministic Terraform plans, drift gates, cross-cloud investigation via SRE Agent plus MCP). This week extends "intent into enforceable gates" by bringing cost into pull request feedback alongside tests and drift. One guide estimates monthly cost delta for Bicep changes in PRs by running `az deployment group what-if` for a structured change set, then mapping changes to prices via the Azure Retail Prices API. It is implemented in GitHub Actions: trigger on PRs touching `infra/**`, authenticate via OIDC (`azure/login@v2` with `id-token: write`), output what-if JSON, run a Python 3.12 script querying `https://prices.azure.com/api/retail/prices` with OData filters, compute monthly cost as `rate * 730`, and post a sticky PR comment with before/after/delta totals. The gate can fail the workflow if `delta_value` exceeds a threshold (for example, 500), making cost regressions enforceable like failing tests. If you added last week''s drift gates, this is the adjacent control: not only "did reality drift?" but "will this PR exceed budget boundaries?"

Microsoft also shared more detail on operationalizing Azure SRE Agent for on-call, continuing last week''s MCP-based investigation storyline. The focus is on keeping the system workable over time: explicit autonomy levels (assistive investigation, remediation proposals for review, autonomous resolution for selected classes), RBAC constraints, approval checkpoints, and escalation paths. It also frames agentic workflows across SDLC phases (agents for spec drafting/prototyping in Plan & Code, and evaluation loops in Verify/Test/Deploy) so ops is not the only integration point. On extensibility, it calls out Python tools and MCP to connect external systems/context while keeping humans accountable at boundaries. Together with last week''s AWS connectivity guide, the storyline is clearer: MCP is the integration mechanism, while autonomy/RBAC/approvals are what make it safe to run.

- [''Building Cost-Aware Azure Infrastructure Pipelines: Estimate Costs Before You Deploy''](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-cost-aware-azure-infrastructure-pipelines-estimate/ba-p/4508776)
- [How we build and use Azure SRE Agent with agentic workflows](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-we-build-and-use-azure-sre-agent-with-agentic-workflows/ba-p/4508753)

### Other DevOps News

Local AI coding agents got a more ops-focused safety pattern using Docker Sandboxes through the `sbx` CLI. Each sandbox runs inside a microVM with its own kernel and separate Docker engine, instead of giving an agent broad host permissions or access to the host Docker socket. This fits last week''s theme that as agents spread beyond ops consoles, isolation and auditable boundaries should become a baseline on laptops too. The guide covers Windows 11 setup (enable `HypervisorPlatform`, install `Docker.sbx` via WinGet, log in, choose egress policy Open/Balanced/Locked Down), then `sbx run` to start agents like Claude Code with network controlled via a host proxy. It also covers practical workflow details: using `--branch` to work in a git worktree under `.sbx/...` to reduce risk to the main tree, adding `.sbx/` to gitignore, and handling constraints like performance overhead, restrictive allowlists, and commit signing friction.

- [Running AI agents safely in a microVM using Docker Sandboxes (sbx)](https://andrewlock.net/running-ai-agents-safely-in-a-microvm-using-docker-sandbox/)

GitHub collaboration UX got several triage improvements that reduce friction, continuing last week''s pattern of making queues easier to navigate and less noisy. Issues now show release info in the sidebar Development section when linked by a PR, including the first release tag that shipped it and whether it is Latest or Pre-release. Projects added default values for Text, Number, and Single select fields so new items have consistent baseline metadata. PR lists in public repos can now show contributor role labels ("First-time contributor," "Contributor," "Member") with privacy rules to avoid exposing private org membership. Moderation also improved with a "Low Quality" hide option (separate from Spam/Abuse) across Issues, Discussions, PRs, and commit comments. Combined with last week''s search improvements, these changes help keep queues usable as repos and bot/AI activity increase.

- [Release information in issue sidebar and default values for project fields](https://github.blog/changelog/2026-04-09-release-info-in-issue-sidebar-and-project-defaults)
- [Repository member role labels now in pull request list view](https://github.blog/changelog/2026-04-09-repository-member-role-labels-now-in-pull-request-list-view)
- [New Low Quality option in the Hide comment menu](https://github.blog/changelog/2026-04-09-new-low-quality-option-in-the-hide-comment-menu)

VS Code Insiders 1.116 continued improving the "Agents app" experience: better keyboard navigation (focus commands for Changes view, its file tree, and Chat Customizations), screen reader help in chat input (including verbosity controls), and "#"-triggered file-context completions scoped to the selected workspace. It also improves CSS `@import` navigation by resolving into `node_modules`, which reduces friction in bundler-heavy repos. GitHub also highlighted GitCity, an open-source Next.js 15 plus Three.js project that turns profile activity into an explorable 3D pixel-art city. It is more inspiration than operations, but it is a creative use of GitHub signals.

- [Visual Studio Code 1.116 (Insiders) release notes (April 2026)](https://code.visualstudio.com/updates/v1_116)
- [Turn your GitHub profile into a 3D city](https://www.youtube.com/shorts/34nBtYNWm4c)',
     excerpt      = 'This week''s DevOps updates centered on practical CI/CD and dependency-maintenance mechanics on GitHub, plus more shift-left thinking for cost control and incident response that often involves agents. Alongside platform changes, guides also focused on making agent workflows safer on laptops and more accountable in IaC pull requests.',
     content_hash = md5('This week''s DevOps updates centered on practical CI/CD and dependency-maintenance mechanics on GitHub, plus more shift-left thinking for cost control and incident response that often involves agents. Alongside platform changes, guides also focused on making agent workflows safer on laptops and more accountable in IaC pull requests.

<!--excerpt_end-->

## DevOps

### GitHub Actions, Dependabot, and platform reliability: tighter guardrails and broader ecosystem support

Building on last week''s Actions work (less CI friction, tighter security via OIDC claims), GitHub added a limit that affects retry-heavy pipelines: a workflow run can be rerun at most 50 times, whether rerunning all jobs or selected jobs. After the 50th rerun, GitHub returns a failed check suite with an annotation that the limit was reached. If bots or scripts auto-rerun until green, update logic to stop before the cap and consider alternatives like backoff/jitter, narrowing retries to specific steps, or starting a fresh run. This supports last week''s reliability theme by nudging teams to engineer reliability rather than relying on unlimited reruns.

Dependabot continued last week''s ecosystem expansion with support for Nix flakes in version updates. By adding `nix` in `.github/dependabot.yml`, Dependabot can monitor `flake.lock` inputs and open one PR per outdated flake input as upstream Git refs advance (GitHub, GitLab, SourceHut, or generic `git` URLs). The key caveat remains that this is version updates only. Dependabot security updates still do not apply to Nix flakes, so vulnerability-driven automation needs a separate approach for Nix setups.

GitHub''s March 2026 availability report reinforced why fallbacks matter, complementing last week''s "keep platform usable at scale" theme. It covers incidents affecting github.com and the API (including a cache-write bug causing widespread expiry and cascading load), Actions scheduling delays and infra errors (Redis load balancer misconfig during resiliency updates), Copilot Coding Agent session failures (auth issues to backing datastore, mitigated by credential rotation, then recurring due to incomplete remediation), and Teams integration delivery failures due to an upstream outage. The actionable DevOps takeaway is to treat platform delays as a distinct failure mode: monitor pipeline SLAs, adjust expectations during incidents, and keep alternate notification paths when integrations break.

- [GitHub Actions workflows are limited to 50 reruns](https://github.blog/changelog/2026-04-10-actions-workflows-are-limited-to-50-reruns)
- [Dependabot version updates now support the Nix ecosystem](https://github.blog/changelog/2026-04-07-dependabot-version-updates-now-support-the-nix-ecosystem)
- [GitHub availability report: March 2026](https://github.blog/news-insights/company-news/github-availability-report-march-2026/)

### Azure cost-aware IaC pipelines and agentic operations: shifting governance earlier and into runtime

Last week emphasized more repeatable infrastructure operations (deterministic Terraform plans, drift gates, cross-cloud investigation via SRE Agent plus MCP). This week extends "intent into enforceable gates" by bringing cost into pull request feedback alongside tests and drift. One guide estimates monthly cost delta for Bicep changes in PRs by running `az deployment group what-if` for a structured change set, then mapping changes to prices via the Azure Retail Prices API. It is implemented in GitHub Actions: trigger on PRs touching `infra/**`, authenticate via OIDC (`azure/login@v2` with `id-token: write`), output what-if JSON, run a Python 3.12 script querying `https://prices.azure.com/api/retail/prices` with OData filters, compute monthly cost as `rate * 730`, and post a sticky PR comment with before/after/delta totals. The gate can fail the workflow if `delta_value` exceeds a threshold (for example, 500), making cost regressions enforceable like failing tests. If you added last week''s drift gates, this is the adjacent control: not only "did reality drift?" but "will this PR exceed budget boundaries?"

Microsoft also shared more detail on operationalizing Azure SRE Agent for on-call, continuing last week''s MCP-based investigation storyline. The focus is on keeping the system workable over time: explicit autonomy levels (assistive investigation, remediation proposals for review, autonomous resolution for selected classes), RBAC constraints, approval checkpoints, and escalation paths. It also frames agentic workflows across SDLC phases (agents for spec drafting/prototyping in Plan & Code, and evaluation loops in Verify/Test/Deploy) so ops is not the only integration point. On extensibility, it calls out Python tools and MCP to connect external systems/context while keeping humans accountable at boundaries. Together with last week''s AWS connectivity guide, the storyline is clearer: MCP is the integration mechanism, while autonomy/RBAC/approvals are what make it safe to run.

- [''Building Cost-Aware Azure Infrastructure Pipelines: Estimate Costs Before You Deploy''](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-cost-aware-azure-infrastructure-pipelines-estimate/ba-p/4508776)
- [How we build and use Azure SRE Agent with agentic workflows](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-we-build-and-use-azure-sre-agent-with-agentic-workflows/ba-p/4508753)

### Other DevOps News

Local AI coding agents got a more ops-focused safety pattern using Docker Sandboxes through the `sbx` CLI. Each sandbox runs inside a microVM with its own kernel and separate Docker engine, instead of giving an agent broad host permissions or access to the host Docker socket. This fits last week''s theme that as agents spread beyond ops consoles, isolation and auditable boundaries should become a baseline on laptops too. The guide covers Windows 11 setup (enable `HypervisorPlatform`, install `Docker.sbx` via WinGet, log in, choose egress policy Open/Balanced/Locked Down), then `sbx run` to start agents like Claude Code with network controlled via a host proxy. It also covers practical workflow details: using `--branch` to work in a git worktree under `.sbx/...` to reduce risk to the main tree, adding `.sbx/` to gitignore, and handling constraints like performance overhead, restrictive allowlists, and commit signing friction.

- [Running AI agents safely in a microVM using Docker Sandboxes (sbx)](https://andrewlock.net/running-ai-agents-safely-in-a-microvm-using-docker-sandbox/)

GitHub collaboration UX got several triage improvements that reduce friction, continuing last week''s pattern of making queues easier to navigate and less noisy. Issues now show release info in the sidebar Development section when linked by a PR, including the first release tag that shipped it and whether it is Latest or Pre-release. Projects added default values for Text, Number, and Single select fields so new items have consistent baseline metadata. PR lists in public repos can now show contributor role labels ("First-time contributor," "Contributor," "Member") with privacy rules to avoid exposing private org membership. Moderation also improved with a "Low Quality" hide option (separate from Spam/Abuse) across Issues, Discussions, PRs, and commit comments. Combined with last week''s search improvements, these changes help keep queues usable as repos and bot/AI activity increase.

- [Release information in issue sidebar and default values for project fields](https://github.blog/changelog/2026-04-09-release-info-in-issue-sidebar-and-project-defaults)
- [Repository member role labels now in pull request list view](https://github.blog/changelog/2026-04-09-repository-member-role-labels-now-in-pull-request-list-view)
- [New Low Quality option in the Hide comment menu](https://github.blog/changelog/2026-04-09-new-low-quality-option-in-the-hide-comment-menu)

VS Code Insiders 1.116 continued improving the "Agents app" experience: better keyboard navigation (focus commands for Changes view, its file tree, and Chat Customizations), screen reader help in chat input (including verbosity controls), and "#"-triggered file-context completions scoped to the selected workspace. It also improves CSS `@import` navigation by resolving into `node_modules`, which reduces friction in bundler-heavy repos. GitHub also highlighted GitCity, an open-source Next.js 15 plus Three.js project that turns profile activity into an explorable 3D pixel-art city. It is more inspiration than operations, but it is a creative use of GitHub signals.

- [Visual Studio Code 1.116 (Insiders) release notes (April 2026)](https://code.visualstudio.com/updates/v1_116)
- [Turn your GitHub profile into a 3D city](https://www.youtube.com/shorts/34nBtYNWm4c)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-04-13';

-- weekly-devops-roundup-2026-04-20  (2026-04-20)
UPDATE content_items
SET  content      = 'This week''s DevOps updates clustered around tighter delivery mechanics (review, shipping, remote work) and more guardrails as automation and agents approach production workflows. GitHub and Azure DevOps shipped reliability and governance updates, while VS Code and Docker continued turning agent-driven work into something more isolated, auditable, and less disruptive to your main working copy.

<!--excerpt_end-->

## DevOps

### AI agents in developer and ops workflows (VS Code + Docker)

Running agents safely is becoming more practical day to day, with focus on isolation and repeatability rather than only chat. After last week''s Docker Sandbox introduction, this week''s follow-on focused on avoiding repeated setup. Andrew Lock dug into Docker Sandboxes (microVM environments launched via `sbx`) and how to avoid reinstalling toolchains for each agent session. The key is publishing your own sandbox *template images* (OCI images) to a registry (for example, Docker Hub) and referencing them by full name (for example, `sbx run -t docker.io/my-org/my-template:v1 claude`). Because sandboxes do not share your local Docker image store, pushing to a registry is required so sandboxes can pull and cache templates.

The guide stays operational: extending `docker/sandbox-templates:claude-code-docker` (Ubuntu-based) with extra tools, including a .NET example that installs OS packages as `root` but installs user-scoped tooling as non-root `agent` (for example, `dotnet-install.sh --channel 10.0 --no-path`, then `DOTNET_ROOT` and `PATH` to `/home/agent/.dotnet` and `/home/agent/.dotnet/tools`). It also covers minimal variants, starting Docker inside the sandbox via `LABEL com.docker.sandboxes.start-docker=true`, and multi-stage builds so updating Claude Code does not force a full toolchain rebuild (for example, `--no-cache-filter claude`).

VS Code''s agent story continued the arc from last week, moving from UI polish to controls that keep agent work contained and accountable. VS Code 1.117 (Insiders) refined session behavior: Autopilot permission mode can persist across sessions, and `chat.permissions.default` lets teams set default permission levels. Agent Host added configurable auto-approvals (including "Bypass Approvals" and "Autopilot (Preview)"), and Agent Host Protocol added support for "subagents" and "agent teams," which signals preparation for multi-agent patterns. For DevOps hygiene, Agent Host sessions can use worktree/Git isolation so agent work does not pollute your main working directory, turning last week''s manual safety pattern into an editor workflow.

Terminal execution also tightened: when an agent sends input to a terminal, VS Code now captures terminal output automatically after a short delay, removing extra back-and-forth. Shell recognition now includes Copilot CLI, Claude Code, and Gemini CLI, and Copilot CLI worktrees get more meaningful branch names based on the user prompt. A companion video focused on terminal tools: foreground terminal support (visible/interruptible), better interactive prompt handling, clearer progress for long commands, and smarter notifications so you do not miss prompts while multitasking.

- [Running AI agents with customized templates using docker sandbox](https://andrewlock.net/running-ai-agents-with-customized-templates-in-docker-sandbox/)
- [Visual Studio Code 1.117 (April 2026) release notes](https://code.visualstudio.com/updates/v1_117)
- [VS Code Terminal Agent Tools](https://www.youtube.com/watch?v=0Eq8m63Z5J0)

### GitHub workflow and governance updates (Stacked PRs, rulesets, status transparency)

GitHub changes landed across review workflow, governance monitoring, and outage interpretation. The thread from last week remains consistent: as automation volume rises, GitHub is adding guardrails, visibility, and reliability signals.

GitHub entered private preview for **Stacked PRs**, bringing stacked-diffs workflows into pull requests. The goal is to make "keep PRs small" workable without blocking progress: PRs can be based on other PRs, forming a stack where review stays granular and merge order is enforced (a PR cannot merge until those below merge). GitHub also supports merging an approved stack at once. An optional CLI extension, **`gh stack`** (https://github.github.com/gh-stack/), helps manage stacks and supports AI-agent-friendly workflows that generate and update chains of dependent PRs. This matches last week''s theme of keeping queues usable as bot and agent activity increases: smaller diffs reduce review load, and tooling reduces fragility when automation authors changes.

GitHub also added a **Rule insights dashboard** under *Repository Settings -> Rules* for repos using rulesets. It summarizes evaluation activity over time (successes, failures, bypasses) and shows "most active bypassers," with charts linking to filtered detailed views for incident and audit workflows. GitHub also replaced multiple bespoke filtering UIs with a **unified filter bar** across code scanning alert dismissal requests, Dependabot alert dismissal requests, secret scanning alert dismissals, and secret scanning push protection bypass requests at enterprise/org/repo scopes. It supports filtering via custom properties. This continues last week''s "tighter guardrails, better triage" theme as policy enforcement and exception handling become daily operations.

GitHub updated its status page to support clearer incident interpretation: a new "Degraded Performance" state, per-service 90-day uptime percentages, and a dedicated Copilot component ("Copilot AI Model Providers"). This matches last week''s availability report takeaway where delays can be a distinct failure mode, and it adds vocabulary for "up but slow" while mapping to pipeline SLAs. The uptime math details matter for SLO/vendor-risk discussions: "Major Outage" counts as 100% downtime, "Partial Outage" as 30%, and "Degraded Performance" as 0% downtime (service considered functional), which changes how published uptime compares to internal telemetry.

- [GitHub invokes spirit of Phabricator with preview of Stacked PRs](https://www.devclass.com/development/2026/04/16/github-invokes-spirit-of-phabricator-with-preview-of-stacked-prs/5217921)
- [Rule insights dashboard and unified filter bar](https://github.blog/changelog/2026-04-16-rule-insights-dashboard-and-unified-filter-bar)
- [Bringing more transparency to GitHub’s status page](https://github.blog/news-insights/company-news/bringing-more-transparency-to-githubs-status-page/)

### Azure SRE Agent automation for AKS incidents and IaC drift

Azure SRE Agent guidance leaned into closed-loop ops: trigger from alerts or drift, investigate under governance, apply scoped fixes (optionally autonomously), verify recovery, and leave durable follow-up in GitHub/Teams. Building on last week''s safety framing (autonomy levels, RBAC constraints, approval checkpoints, MCP/Python extensibility), these walkthroughs show governance wired end-to-end from real triggers into Azure remediation and back into source control.

In the AKS incident-response walkthrough, safety comes from Azure RBAC + scoped identities + execution modes (Review vs Privileged vs Autonomous), not prompt wording. An Azure Monitor alert (Action Group webhook) triggers the agent, which uses Log Analytics/Kusto, Azure Resource Graph, Azure CLI/ARM, and `kubectl` to diagnose, remediate, and verify.

Two failure modes make it concrete. For CPU starvation, workloads are deployed with very low CPU/memory (requests `cpu: 1m`, limits `cpu: 5m`; memory `6Mi/20Mi`), causing startup probe failures because the process cannot bind in time. The agent uses pod status and exit codes (exit code `1`, not `137`, to rule out OOMKill), finds CPU-throttled pods via `kubectl top`, patches CPU across workloads, and verifies recovery (healthy pods, zero restarts). For OOMKilled, it uses exit code `137`, empty logs, and baseline memory (~50Mi) to justify raising limits from `20Mi` to `128Mi` (and requests `10Mi` to `50Mi`), then verifies stabilization via utilization and restarts. Aftercare is built in: Teams gets milestone updates, and the agent can open GitHub issues and draft PRs so hotfixes are reconciled into source-controlled manifests. That matches last week''s emphasis on leaving an artifact trail for post-incident review.

The drift-detection walkthrough applies the same model to Terraform. Terraform Cloud (or another drift system) sends a webhook to an Azure Logic App, which uses Managed Identity to get an Entra ID token and forwards an authenticated request to an Azure SRE Agent HTTP Trigger endpoint. The agent correlates drift diffs with Azure Activity Log and Application Insights, classifies drift as Benign/Risky/Critical, and can recommend not reverting drift if it is mitigating an incident. This continues last week''s drift-gates mindset: detect mismatch, but turn it into a governed decision with context (who changed what, why, and what it is doing now).

The demo scenario shows why correlation matters. An App Service on B1 has latency spikes and 502s from a blocking `/api/data`; during mitigation, an engineer changes infra in the portal by adding tags (benign), downgrading TLS 1.2 to TLS 1.0 (risky), and scaling B1 -> S1 (critical cost). Drift triggers later, and the agent recommends reverting TLS immediately, reverting tags anytime, and delaying SKU revert until the performance issue is fixed because scaling is mitigating the incident. It also captures actor context from Activity Log and posts a severity-coded drift table and ordered plan into Teams, with optional GitHub PR follow-up.

- [''Autonomous AKS Incident Response with Azure SRE Agent: From Alert to Verified Recovery in Minutes''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/autonomous-aks-incident-response-with-azure-sre-agent-from-alert/ba-p/4511343)
- [''Event-Driven IaC Operations with Azure SRE Agent: Terraform Drift Detection via HTTP Triggers''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/event-driven-iac-operations-with-azure-sre-agent-terraform-drift/ba-p/4512233)

### Other DevOps News

Azure DevOps Server Patch 3 (April 14, 2026) included fixes that affect everyday repo/integration reliability: a null reference that could break PR completion during work item auto-completion, improved sign-out redirect validation to reduce open redirect risk, and a fix for PAT-based connections to GitHub Enterprise Server. Microsoft also included a way to verify install via the patch installer''s `CheckInstall` argument.

- [April Patches for Azure DevOps Server](https://devblogs.microsoft.com/devops/april-patches-for-azure-devops-server/)

VS Code Remote Tunnels were highlighted as an alternative to RDP for locked-down customer VMs, matching last week''s "safer dev workflows" theme. The guide walks through running `code tunnel` on the remote VM (installing VS Code Server components and creating an outbound tunnel via Microsoft Dev Tunnels), then attaching from local VS Code or `vscode.dev` without inbound SSH. It also notes constraints (single-user, customer policy limits on GitHub/Microsoft auth) and keeping tunnels alive via service mode (`code tunnel service install`).

- [''Stop Coding Through Remote Desktop: Use VS Code Remote Tunnels Instead''](https://dev.to/playfulprogramming/stop-coding-through-remote-desktop-use-vs-code-remote-tunnels-instead-37oc)

GitHub Pages onboarding content (blog + video) emphasized a key operational choice: deploy from a branch for simple static sites, or use GitHub Actions when you need a build (for example, Next.js). In the context of last week''s reliability/fallback theme, it is a reminder to treat Pages pipelines like any delivery surface: know whether you depend on Actions and what "degraded" modes mean. Both also cover common production steps (custom domain, DNS verification, "Enforce HTTPS") and note Pages sites are public even if the repo is private.

- [''GitHub for Beginners: Getting started with GitHub Pages''](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-pages/)
- [Getting started with GitHub Pages for beginners | Tutorial](https://www.youtube.com/watch?v=b2r9Cdvssi0)

SSMS 22.5 added SQL projects support, aiming to make schema-as-code more accessible starting from an existing database. The workflow is importing a database into a SQL project, editing and validating changes, and publishing in a controlled way, reusing the same project artifact across SSMS, VS Code, GitHub Actions, and Azure DevOps pipelines.

- [Introducing SQL projects in SSMS (SSMS 22.5) | Data Exposed](https://www.youtube.com/watch?v=HE0t7IpOsuM)

A GitHub-based architecture-as-code workflow outlined a repo structure for ADRs, diagram-as-code (Mermaid/PlantUML/C4), standards, reference architectures, and roadmaps, governed via PRs and CI checks to reduce doc drift. With this week''s Stacked PRs preview and last week''s GitHub UX/triage refinements, the direction is consistent: more "non-code" work is moving into PR-governed, policy-enforced workflows.

- [''From Diagrams to Decisions: Using GitHub to Power Modern Solution Architecture''](https://dellenny.com/from-diagrams-to-decisions-using-github-to-power-modern-solution-architecture/)

GitHub shared a deployment-safety pattern using eBPF + cgroups to apply per-process network controls to deployment tooling so rollouts do not accidentally depend on github.com during outages. This extends last week''s lesson about availability and fallbacks: engineer deployment systems to avoid circular dependencies, not only monitor them. The write-up covers CGROUP_SKB egress enforcement and a domain-centric approach intercepting DNS via CGROUP_SOCK_ADDR routed through a local proxy, plus attribution to map blocked lookups back to PID/command line for actionable logs.

- [How GitHub uses eBPF to improve deployment safety](https://github.blog/engineering/infrastructure/how-github-uses-ebpf-to-improve-deployment-safety/)',
     excerpt      = 'This week''s DevOps updates clustered around tighter delivery mechanics (review, shipping, remote work) and more guardrails as automation and agents approach production workflows. GitHub and Azure DevOps shipped reliability and governance updates, while VS Code and Docker continued turning agent-driven work into something more isolated, auditable, and less disruptive to your main working copy.',
     content_hash = md5('This week''s DevOps updates clustered around tighter delivery mechanics (review, shipping, remote work) and more guardrails as automation and agents approach production workflows. GitHub and Azure DevOps shipped reliability and governance updates, while VS Code and Docker continued turning agent-driven work into something more isolated, auditable, and less disruptive to your main working copy.

<!--excerpt_end-->

## DevOps

### AI agents in developer and ops workflows (VS Code + Docker)

Running agents safely is becoming more practical day to day, with focus on isolation and repeatability rather than only chat. After last week''s Docker Sandbox introduction, this week''s follow-on focused on avoiding repeated setup. Andrew Lock dug into Docker Sandboxes (microVM environments launched via `sbx`) and how to avoid reinstalling toolchains for each agent session. The key is publishing your own sandbox *template images* (OCI images) to a registry (for example, Docker Hub) and referencing them by full name (for example, `sbx run -t docker.io/my-org/my-template:v1 claude`). Because sandboxes do not share your local Docker image store, pushing to a registry is required so sandboxes can pull and cache templates.

The guide stays operational: extending `docker/sandbox-templates:claude-code-docker` (Ubuntu-based) with extra tools, including a .NET example that installs OS packages as `root` but installs user-scoped tooling as non-root `agent` (for example, `dotnet-install.sh --channel 10.0 --no-path`, then `DOTNET_ROOT` and `PATH` to `/home/agent/.dotnet` and `/home/agent/.dotnet/tools`). It also covers minimal variants, starting Docker inside the sandbox via `LABEL com.docker.sandboxes.start-docker=true`, and multi-stage builds so updating Claude Code does not force a full toolchain rebuild (for example, `--no-cache-filter claude`).

VS Code''s agent story continued the arc from last week, moving from UI polish to controls that keep agent work contained and accountable. VS Code 1.117 (Insiders) refined session behavior: Autopilot permission mode can persist across sessions, and `chat.permissions.default` lets teams set default permission levels. Agent Host added configurable auto-approvals (including "Bypass Approvals" and "Autopilot (Preview)"), and Agent Host Protocol added support for "subagents" and "agent teams," which signals preparation for multi-agent patterns. For DevOps hygiene, Agent Host sessions can use worktree/Git isolation so agent work does not pollute your main working directory, turning last week''s manual safety pattern into an editor workflow.

Terminal execution also tightened: when an agent sends input to a terminal, VS Code now captures terminal output automatically after a short delay, removing extra back-and-forth. Shell recognition now includes Copilot CLI, Claude Code, and Gemini CLI, and Copilot CLI worktrees get more meaningful branch names based on the user prompt. A companion video focused on terminal tools: foreground terminal support (visible/interruptible), better interactive prompt handling, clearer progress for long commands, and smarter notifications so you do not miss prompts while multitasking.

- [Running AI agents with customized templates using docker sandbox](https://andrewlock.net/running-ai-agents-with-customized-templates-in-docker-sandbox/)
- [Visual Studio Code 1.117 (April 2026) release notes](https://code.visualstudio.com/updates/v1_117)
- [VS Code Terminal Agent Tools](https://www.youtube.com/watch?v=0Eq8m63Z5J0)

### GitHub workflow and governance updates (Stacked PRs, rulesets, status transparency)

GitHub changes landed across review workflow, governance monitoring, and outage interpretation. The thread from last week remains consistent: as automation volume rises, GitHub is adding guardrails, visibility, and reliability signals.

GitHub entered private preview for **Stacked PRs**, bringing stacked-diffs workflows into pull requests. The goal is to make "keep PRs small" workable without blocking progress: PRs can be based on other PRs, forming a stack where review stays granular and merge order is enforced (a PR cannot merge until those below merge). GitHub also supports merging an approved stack at once. An optional CLI extension, **`gh stack`** (https://github.github.com/gh-stack/), helps manage stacks and supports AI-agent-friendly workflows that generate and update chains of dependent PRs. This matches last week''s theme of keeping queues usable as bot and agent activity increases: smaller diffs reduce review load, and tooling reduces fragility when automation authors changes.

GitHub also added a **Rule insights dashboard** under *Repository Settings -> Rules* for repos using rulesets. It summarizes evaluation activity over time (successes, failures, bypasses) and shows "most active bypassers," with charts linking to filtered detailed views for incident and audit workflows. GitHub also replaced multiple bespoke filtering UIs with a **unified filter bar** across code scanning alert dismissal requests, Dependabot alert dismissal requests, secret scanning alert dismissals, and secret scanning push protection bypass requests at enterprise/org/repo scopes. It supports filtering via custom properties. This continues last week''s "tighter guardrails, better triage" theme as policy enforcement and exception handling become daily operations.

GitHub updated its status page to support clearer incident interpretation: a new "Degraded Performance" state, per-service 90-day uptime percentages, and a dedicated Copilot component ("Copilot AI Model Providers"). This matches last week''s availability report takeaway where delays can be a distinct failure mode, and it adds vocabulary for "up but slow" while mapping to pipeline SLAs. The uptime math details matter for SLO/vendor-risk discussions: "Major Outage" counts as 100% downtime, "Partial Outage" as 30%, and "Degraded Performance" as 0% downtime (service considered functional), which changes how published uptime compares to internal telemetry.

- [GitHub invokes spirit of Phabricator with preview of Stacked PRs](https://www.devclass.com/development/2026/04/16/github-invokes-spirit-of-phabricator-with-preview-of-stacked-prs/5217921)
- [Rule insights dashboard and unified filter bar](https://github.blog/changelog/2026-04-16-rule-insights-dashboard-and-unified-filter-bar)
- [Bringing more transparency to GitHub’s status page](https://github.blog/news-insights/company-news/bringing-more-transparency-to-githubs-status-page/)

### Azure SRE Agent automation for AKS incidents and IaC drift

Azure SRE Agent guidance leaned into closed-loop ops: trigger from alerts or drift, investigate under governance, apply scoped fixes (optionally autonomously), verify recovery, and leave durable follow-up in GitHub/Teams. Building on last week''s safety framing (autonomy levels, RBAC constraints, approval checkpoints, MCP/Python extensibility), these walkthroughs show governance wired end-to-end from real triggers into Azure remediation and back into source control.

In the AKS incident-response walkthrough, safety comes from Azure RBAC + scoped identities + execution modes (Review vs Privileged vs Autonomous), not prompt wording. An Azure Monitor alert (Action Group webhook) triggers the agent, which uses Log Analytics/Kusto, Azure Resource Graph, Azure CLI/ARM, and `kubectl` to diagnose, remediate, and verify.

Two failure modes make it concrete. For CPU starvation, workloads are deployed with very low CPU/memory (requests `cpu: 1m`, limits `cpu: 5m`; memory `6Mi/20Mi`), causing startup probe failures because the process cannot bind in time. The agent uses pod status and exit codes (exit code `1`, not `137`, to rule out OOMKill), finds CPU-throttled pods via `kubectl top`, patches CPU across workloads, and verifies recovery (healthy pods, zero restarts). For OOMKilled, it uses exit code `137`, empty logs, and baseline memory (~50Mi) to justify raising limits from `20Mi` to `128Mi` (and requests `10Mi` to `50Mi`), then verifies stabilization via utilization and restarts. Aftercare is built in: Teams gets milestone updates, and the agent can open GitHub issues and draft PRs so hotfixes are reconciled into source-controlled manifests. That matches last week''s emphasis on leaving an artifact trail for post-incident review.

The drift-detection walkthrough applies the same model to Terraform. Terraform Cloud (or another drift system) sends a webhook to an Azure Logic App, which uses Managed Identity to get an Entra ID token and forwards an authenticated request to an Azure SRE Agent HTTP Trigger endpoint. The agent correlates drift diffs with Azure Activity Log and Application Insights, classifies drift as Benign/Risky/Critical, and can recommend not reverting drift if it is mitigating an incident. This continues last week''s drift-gates mindset: detect mismatch, but turn it into a governed decision with context (who changed what, why, and what it is doing now).

The demo scenario shows why correlation matters. An App Service on B1 has latency spikes and 502s from a blocking `/api/data`; during mitigation, an engineer changes infra in the portal by adding tags (benign), downgrading TLS 1.2 to TLS 1.0 (risky), and scaling B1 -> S1 (critical cost). Drift triggers later, and the agent recommends reverting TLS immediately, reverting tags anytime, and delaying SKU revert until the performance issue is fixed because scaling is mitigating the incident. It also captures actor context from Activity Log and posts a severity-coded drift table and ordered plan into Teams, with optional GitHub PR follow-up.

- [''Autonomous AKS Incident Response with Azure SRE Agent: From Alert to Verified Recovery in Minutes''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/autonomous-aks-incident-response-with-azure-sre-agent-from-alert/ba-p/4511343)
- [''Event-Driven IaC Operations with Azure SRE Agent: Terraform Drift Detection via HTTP Triggers''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/event-driven-iac-operations-with-azure-sre-agent-terraform-drift/ba-p/4512233)

### Other DevOps News

Azure DevOps Server Patch 3 (April 14, 2026) included fixes that affect everyday repo/integration reliability: a null reference that could break PR completion during work item auto-completion, improved sign-out redirect validation to reduce open redirect risk, and a fix for PAT-based connections to GitHub Enterprise Server. Microsoft also included a way to verify install via the patch installer''s `CheckInstall` argument.

- [April Patches for Azure DevOps Server](https://devblogs.microsoft.com/devops/april-patches-for-azure-devops-server/)

VS Code Remote Tunnels were highlighted as an alternative to RDP for locked-down customer VMs, matching last week''s "safer dev workflows" theme. The guide walks through running `code tunnel` on the remote VM (installing VS Code Server components and creating an outbound tunnel via Microsoft Dev Tunnels), then attaching from local VS Code or `vscode.dev` without inbound SSH. It also notes constraints (single-user, customer policy limits on GitHub/Microsoft auth) and keeping tunnels alive via service mode (`code tunnel service install`).

- [''Stop Coding Through Remote Desktop: Use VS Code Remote Tunnels Instead''](https://dev.to/playfulprogramming/stop-coding-through-remote-desktop-use-vs-code-remote-tunnels-instead-37oc)

GitHub Pages onboarding content (blog + video) emphasized a key operational choice: deploy from a branch for simple static sites, or use GitHub Actions when you need a build (for example, Next.js). In the context of last week''s reliability/fallback theme, it is a reminder to treat Pages pipelines like any delivery surface: know whether you depend on Actions and what "degraded" modes mean. Both also cover common production steps (custom domain, DNS verification, "Enforce HTTPS") and note Pages sites are public even if the repo is private.

- [''GitHub for Beginners: Getting started with GitHub Pages''](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-pages/)
- [Getting started with GitHub Pages for beginners | Tutorial](https://www.youtube.com/watch?v=b2r9Cdvssi0)

SSMS 22.5 added SQL projects support, aiming to make schema-as-code more accessible starting from an existing database. The workflow is importing a database into a SQL project, editing and validating changes, and publishing in a controlled way, reusing the same project artifact across SSMS, VS Code, GitHub Actions, and Azure DevOps pipelines.

- [Introducing SQL projects in SSMS (SSMS 22.5) | Data Exposed](https://www.youtube.com/watch?v=HE0t7IpOsuM)

A GitHub-based architecture-as-code workflow outlined a repo structure for ADRs, diagram-as-code (Mermaid/PlantUML/C4), standards, reference architectures, and roadmaps, governed via PRs and CI checks to reduce doc drift. With this week''s Stacked PRs preview and last week''s GitHub UX/triage refinements, the direction is consistent: more "non-code" work is moving into PR-governed, policy-enforced workflows.

- [''From Diagrams to Decisions: Using GitHub to Power Modern Solution Architecture''](https://dellenny.com/from-diagrams-to-decisions-using-github-to-power-modern-solution-architecture/)

GitHub shared a deployment-safety pattern using eBPF + cgroups to apply per-process network controls to deployment tooling so rollouts do not accidentally depend on github.com during outages. This extends last week''s lesson about availability and fallbacks: engineer deployment systems to avoid circular dependencies, not only monitor them. The write-up covers CGROUP_SKB egress enforcement and a domain-centric approach intercepting DNS via CGROUP_SOCK_ADDR routed through a local proxy, plus attribution to map blocked lookups back to PID/command line for actionable logs.

- [How GitHub uses eBPF to improve deployment safety](https://github.blog/engineering/infrastructure/how-github-uses-ebpf-to-improve-deployment-safety/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'devops'
  AND slug                 = 'weekly-devops-roundup-2026-04-20';

-- SKIP 2026-04-27 : no 'DevOps' block found in source (weekly-ai-roundup-2026-04-27)

-- SKIP 2026-05-04 : no 'DevOps' block found in source (weekly-ai-roundup-2026-05-04)

-- SKIP 2026-05-11 : no 'DevOps' block found in source (weekly-ai-roundup-2026-05-11)

