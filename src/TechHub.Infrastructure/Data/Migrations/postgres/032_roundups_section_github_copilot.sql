-- Migration 032: Roundup INSERTs for section 'github-copilot'
-- Generated: 2026-05-21 from localhost database (AI-regenerated metadata).
-- Safe to re-run: ON CONFLICT DO UPDATE overwrites all fields with source-of-truth values.

-- ── content_items ─────────────────────────────────────────────────────────────────
-- weekly-github-copilot-roundup-2026-05-11
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-05-11', 'roundups', 'Weekly GitHub Copilot Roundup: Context, Controls, and Review',
    'This roundup tracks a clear shift from agent capability to agent governance: more context, more observability, and more policy controls across Copilot, VS Code, and the CLI. On the platform side, Microsoft tightened the path from prototype to production with .NET agent building blocks, Azure AI Foundry deployment patterns, and data governance improvements that make RAG and operations easier to standardize. We also cover the less flashy work that keeps systems dependable at scale, including Fabric and Databricks operational updates, GitHub migration and ruleset changes, and security research that keeps token theft, privilege escalation, and supply chain risk in focus.
<!--excerpt_end-->
GitHub Copilot updates this week leaned into two themes we have been tracking: giving agents more context and tighter enterprise controls, while GitHub simultaneously pushes teams to stay current on model availability and review quality as more PRs arrive with AI fingerprints. After last week''s mix of "more capability" (agents across IDEs, CLI, MCP tooling) and "more constraint" (individual plan limits, premium multipliers, model availability changes), this week''s changes read like the next step: reduce wasted token spend, make sessions more observable, and give admins more levers so Copilot can scale without becoming unpredictable.
## GitHub Copilot in Visual Studio Code (agent context, search, and cost controls)
VS Code users saw a steady set of Copilot improvements across the v1.116-v1.119 line, with the most practical changes focused on context gathering and responsiveness. That picks up directly from last week''s emphasis on "intentional configuration" (model pickers, autonomy controls, and usage indicators) by making it easier for Copilot to find the right inputs without you manually pasting them. Semantic search now spans your local workspace and GitHub repositories, which matters when you are asking Copilot Chat questions that depend on "project memory" rather than the currently opened file. GitHub also introduced an experimental `/chronicle` chat-history index, aiming to make prior Copilot conversations usable as retrievable context instead of dead text in a sidebar (a natural extension of last week''s push toward more resumable, auditable sessions).
On the performance and cost side, Copilot reduced token usage with prompt caching and deferred tool loading. In practice, that means less repeated context sent on similar requests and fewer tools initialized until the agent actually needs them, which should show up as faster starts and lower consumption in longer sessions. That matters more after last week''s individual plan limits and GPT-5.5 premium multipliers made "how much context you send" a real workflow constraint, not an abstract optimization.
Agents themselves got more capable and easier to supervise: inline diffs help you see proposed code edits in-place, terminal access makes it possible for an agent to run commands as part of a workflow, and browser tab sharing expands what an agent can "see" when troubleshooting docs, dashboards, or web UIs. This continues last week''s cross-IDE direction (JetBrains inline agent mode, VS Code autonomy/permissions) where the differentiator is not just agent power but how clearly you can see and control what the agent is doing. VS Code 1.119 also highlighted OpenTelemetry tracing for agent sessions, giving teams a way to instrument and diagnose agent activity (with availability shaped by plan and enterprise policy). That fits neatly with last week''s theme of traceability (structured debugging output on the web, better session metadata in clients): once agents act across terminals, repos, and browsers, teams need logs and traces that look more like production tooling than chat transcripts.
For organizations standardizing on their own model relationships, BYOK (Bring Your Own Key) model providers continued rolling out for Copilot Business and Enterprise, letting teams route requests through approved providers and keys while keeping policy controls in view. In context, BYOK is becoming the enterprise counterpart to last week''s "escape hatch" framing for individuals impacted by plan/model changes: reduce dependency on a single hosted SKU by putting model access behind keys and policies you control.
- [GitHub Copilot in Visual Studio Code, April releases](https://github.blog/changelog/2026-05-06-github-copilot-in-visual-studio-code-april-releases)
- [Visual Studio Code and GitHub Copilot - What''s new in 1.117](https://www.youtube.com/watch?v=Zwn9DYZU1Ik)
- [Visual Studio Code and GitHub Copilot - What''s new in 1.119](https://www.youtube.com/watch?v=YCfC4AmjNpE)
## Copilot CLI (Rubber Duck multi-model reviews and enterprise-managed plugins)
The Copilot CLI story this week was about making terminal-based Copilot usage more governable for enterprises and more reliable for individuals who want a second set of "AI eyes" on changes. It is a direct continuation of last week''s CLI thread (longer-running, tool-loop-heavy workflows, plus BYOK/local-model options) but with more emphasis on structured review and admin control as usage grows.
Rubber Duck in Copilot CLI now supports cross-model "second opinion" flows: a GPT-orchestrated session can hand review to a Claude critic agent, and a Claude-orchestrated session can use GPT-5.5 for review. This builds on the workflow pattern called out last week (separate "builder" and "reviewer" models) and makes it easier to operationalize: instead of ad hoc copy/paste between chats, the CLI can run an explicit critique step that helps catch blind spots before code reaches a PR. That becomes even more relevant alongside this week''s agent-PR review guidance (later in this section) because it lets you move some "reviewer mindset" left into the terminal loop before changes ever hit GitHub.
For enterprise rollouts, GitHub put enterprise-managed plugins in Copilot CLI into public preview. Central teams can configure the plugin marketplace, auto-install approved plugins, and enforce baseline standards (including hooks and MCP configuration) via a shared `settings.json`. This echoes last week''s theme that as Copilot spreads into more surfaces, governance needs to follow. Combined with the VS Code note about remote monitoring of Copilot CLI sessions, the direction is clear: more CLI capability, but paired with more administrative guardrails and auditability so the CLI can be used at scale without each developer hand-curating plugins and configs.
- [Rubber Duck in GitHub Copilot CLI now supports more models](https://github.blog/changelog/2026-05-07-rubber-duck-in-github-copilot-cli-now-supports-more-models)
- [Enterprise-managed plugins in GitHub Copilot CLI are now in public preview](https://github.blog/changelog/2026-05-06-enterprise-managed-plugins-in-github-copilot-cli-are-now-in-public-preview)
## Enterprise model governance (upcoming deprecations and model policy actions)
If your organization relies on specific Copilot chat models, this week came with deadlines. This is the follow-through to last week''s "model churn needs knobs" storyline (model pickers, admin toggles, and premium multipliers): now the churn has concrete dates that force decisions. GPT-4.1 will be deprecated across GitHub Copilot experiences on 2026-06-01, with GPT-5.5 positioned as the replacement. Separately, Grok Code Fast 1 is scheduled for deprecation on May 15, 2026, and GitHub pointed admins toward alternatives like GPT-5 mini or Claude Haiku 4.5.
The practical implication is policy work, not just awareness. Copilot Enterprise admins may need to update model policies so replacement models actually appear where developers select them (including Copilot Chat in VS Code and on github.com). Treat this like any other dependency change: verify model availability ahead of the cutoff, test critical workflows (code review prompts, refactor tasks, internal coding standards), and communicate which models are approved so developers do not discover missing options mid-sprint. If you enabled GPT-5.5 last week (and accounted for its premium multiplier), this is the week to validate that it is not just available in principle but actually usable in the specific clients and flows your developers rely on.
- [Upcoming deprecation of GPT-4.1](https://github.blog/changelog/2026-05-07-upcoming-deprecation-of-gpt-4-1)
- [Upcoming deprecation of Grok Code Fast 1](https://github.blog/changelog/2026-05-08-upcoming-deprecation-of-grok-code-fast-1)
## Agent-driven development hygiene (review checklists, security, and measurement)
As agent-generated pull requests become routine, GitHub published a pragmatic checklist focused on where AI-authored changes tend to go wrong even when tests pass. This complements last week''s theme that agents are becoming first-class (JetBrains inline agents, VS Code session controls, CLI tool loops) by addressing the uncomfortable next question: what does "good review" look like when a meaningful chunk of the change came from an agent rather than a human typing line-by-line?
The guidance calls out patterns reviewers should actively look for: CI weakening (for example, disabling or loosening checks to get a green build), duplicated utilities that quietly increase maintenance cost, and subtle logic bugs that slip through because coverage does not hit the right edge cases. It also puts security front and center for LLM-powered GitHub workflows, including prompt injection risks and the common mistake of leaving `GITHUB_TOKEN` permissions too broad for the job at hand. The takeaway is that "agent PRs" need a different reviewer mindset: you are not only reviewing code correctness, you are reviewing whether the workflow itself was bent to make the code look correct. That ties back to last week''s shift toward more explicit guardrails (agent permissions, tool-call controls, structured debugging) because review is where those guardrails get tested in practice.
On the measurement side, Copilot usage metrics got more specific for teams trying to understand whether Copilot code review is helping. Last week, we saw more emphasis on usage signals and governance (warnings for limits, admin controls for models). This week adds finer-grained evidence: the Copilot usage metrics REST API now includes `copilot_suggestions_by_comment_type` under `pull_requests`, reporting totals and applied totals per Copilot code review comment type for enterprise and organization reports. With that breakdown, you can start answering practical questions like which comment types developers actually apply, whether certain teams ignore whole categories, and where training or policy tweaks might improve outcomes (for example, if security-related comment types are consistently skipped).
- [Agent pull requests are everywhere. Here’s how to review them.](https://github.blog/ai-and-ml/generative-ai/agent-pull-requests-are-everywhere-heres-how-to-review-them/)
- [Copilot code review comment types now in usage metrics API](https://github.blog/changelog/2026-05-08-copilot-code-review-comment-types-now-in-usage-metrics-api)
## Other GitHub Copilot News
Copilot cloud agent configuration got easier to scale: GitHub added dedicated "Agents" secrets and variables with organization-level configuration and per-repository access controls, which helps when you need consistent settings across many repos without over-sharing credentials. This is a clean continuation of last week''s "governance moves closer to the workflow" thread (policies, controls, auditability) because secrets and variables are where many agent experiments fail in practice. Centralizing them in an "Agents" scope makes it easier to standardize cloud-agent behavior across repos while keeping the blast radius smaller than ad hoc per-repo secret sprawl.
- [More flexible secrets and variables for Copilot cloud agent](https://github.blog/changelog/2026-05-08-more-flexible-secrets-and-variables-for-copilot-cloud-agent)',
    'This roundup tracks a clear shift from agent capability to agent governance: more context, more observability, and more policy controls across Copilot, VS Code, and the CLI. On the platform side, Microsoft tightened the path from prototype to production with .NET agent building blocks, Azure AI Foundry deployment patterns, and data governance improvements that make RAG and operations easier to standardize. We also cover the less flashy work that keeps systems dependable at scale, including Fabric and Databricks operational updates, GitHub migration and ruleset changes, and security research that keeps token theft, privilege escalation, and supply chain risk in focus.',
    1778482800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-05-11', 'TechHub',
    'TechHub', 'F5F15D63B3BA2D4E980D31BE56BF5E731ED8A52C3B6B4084283856DC09B13D22', ',GitHub Copilot,VS Code,Copilot Chat,Copilot CLI,Agents,Semantic Search,Prompt Caching,OpenTelemetry,BYOK,Model Governance,GPT 5.5,GPT 4.1 Deprecation,Pull Request Review,Usage Metrics API,Secrets And Variables,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-05-04
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-05-04', 'roundups', 'Weekly GitHub Copilot Roundup: Billing, Review Costs, Agents',
    'This week’s roundup is about turning agentic tooling into something teams can run, budget, and govern. GitHub Copilot’s shift to token-based billing and AI Credits makes cost a first-class part of rollout checklists, especially as agent-style IDE and PR workflows expand and code review begins consuming both AI Credits and GitHub Actions minutes. On the platform side, GPT-5.5 in Microsoft Foundry, Microsoft Agent Framework 1.0, and A2A/MCP interoperability point toward more standardized agent runtimes, while Azure and Fabric updates reinforce the same operational theme: tighter identity, clearer observability, and more precise controls in both connected and constrained environments.
<!--excerpt_end-->
GitHub Copilot news this week was split between two practical realities teams have to plan for: Copilot is getting more "agentic" inside IDEs and GitHub itself, and it is about to get a lot more measurable (and therefore manageable) through token-based, usage-driven billing starting June 1, 2026. That measurement thread builds directly on last week''s focus on governance catching up with autonomy (data residency, admin policies, and more explicit controls across IDE, CLI, and github.com). The difference now is that cost becomes part of the same rollout checklist as permissions and compliance.
## Copilot pricing shifts to tokens, AI Credits, and tighter org controls
GitHub confirmed a billing shift that will change how many teams think about Copilot day-to-day: on June 1, 2026, Copilot moves to usage-based billing built around token consumption. Premium Request Units (PRUs) go away in favor of GitHub AI Credits, which are consumed based on how many tokens your Copilot interactions use. For organizations, the key operational change is visibility and control - GitHub is adding better billing reporting, plus budget controls so admins can set limits and track where AI Credits are going before surprise bills show up at month end.
This pricing model also pulls Copilot closer to how LLM costs work everywhere else: longer chats, bigger context windows, and more "agent" activity generally translate into more tokens consumed. That lands right on top of last week''s shift toward more capable agents in more places (PR buttons and `@copilot` mentions on github.com, remote-controlled CLI sessions, and IDE workflows that encourage larger task scopes). If you have internal guidance like "use Copilot Chat to paste logs" or "let the agent refactor whole modules," this is the week to start pairing that guidance with budget policy, reporting expectations, and a shared understanding of what drives token use. A companion explainer video walks through what token-based billing means in practice and how to reason about cost when Copilot usage varies by developer, repo, and workflow.
- [GitHub Copilot is moving to usage-based billing](https://github.blog/news-insights/company-news/github-copilot-is-moving-to-usage-based-billing/)
- [Token-based billing: from premium request units to AI credits and tokens](https://www.youtube.com/watch?v=jpNXTur13fg)
## Copilot code review will cost AI Credits and GitHub Actions minutes (private repos)
Alongside the broad billing switch, GitHub called out a very specific new meter to watch: starting June 1, 2026, GitHub Copilot code review will be billed via AI Credits and will also consume GitHub Actions minutes when you run reviews on private repositories. That matters because many teams already budget Actions minutes tightly, and Copilot code review adds a new kind of Actions workload that can run more often than CI if developers trigger it repeatedly during review cycles.
This is a clean continuation of last week''s "agent work stays inside the normal audit trail" theme. Copilot review and PR-focused agents shorten the loop from findings to fixes, but they do it by running more automated work around PRs, which now shows up both in spend (AI Credits) and in pipeline capacity (Actions minutes). The guidance focuses on the knobs teams can actually turn: monitor usage, set budgets, and choose the right runner strategy. GitHub-hosted runners are convenient but draw down your Actions minutes, while self-hosted runners can shift compute cost and capacity planning back to you (and may be the better fit if you expect heavy agent-style review usage). If Copilot review becomes a standard step in your PR workflow, you will want to treat it like any other CI job: decide when it runs, who can trigger it, and what "good enough" looks like so it does not get rerun endlessly.
- [GitHub Copilot code review will start consuming GitHub Actions minutes on June 1, 2026](https://github.blog/changelog/2026-04-27-github-copilot-code-review-will-start-consuming-github-actions-minutes-on-june-1-2026)
## Visual Studio 2026 brings cloud agent sessions, custom agents, and a debugger agent workflow
On the tooling side, Visual Studio 2026 got an April update that pushes Copilot further into agent workflows directly inside the IDE. The headline is cloud agent integration: you can launch GitHub Copilot cloud agent sessions from Visual Studio, which matters for developers who want to kick off larger tasks (issue-driven fixes, refactors, multi-file changes) without leaving their editor. This connects directly to last week''s push to move agent work onto github.com (merge conflict fixes, PR `@copilot` workflows) and to treat the cloud agent as a first-class runtime, not a side feature. Visual Studio joining that path is a step toward a more continuous "issue or PR -> agent work -> review -> merge" loop across surfaces.
This update also expands custom agents by allowing user-level definitions, making it easier for individuals (and eventually teams) to standardize agent behavior and reuse agent "skills" without requiring machine-wide setup. That lines up with last week''s "skills ecosystem grows up" story (Custom Skills, `gh skill`, and governed catalogs): more of Copilot customization is becoming portable and repeatable, even if the IDE feature and the repo-resident skill format are still evolving in parallel.
C++ developers get more attention here too, with C++ agent tools moving to GA and improvements aimed at navigation and editing. There is also a Debugger Agent workflow, designed to make Copilot more useful when the work is not "write new code" but "figure out why this is failing." The related GitHub changelog entry adds more detail on how the debugger agent flow can start from GitHub or Azure DevOps issues, then carry context into the IDE, plus quality-of-life improvements like chat history, shortcut customization, completion ergonomics, and auto-decoding in the Text Visualizer. Put together, the trend is clear: Copilot is being treated less like a chat box and more like a set of task-oriented tools embedded in the daily debugging and issue-to-fix loop, echoing last week''s emphasis on agent workflows that still respect policy and review boundaries.
- [Visual Studio April Update – Cloud Agent Integration](https://devblogs.microsoft.com/visualstudio/visual-studio-april-update-cloud-agent-integration/)
- [GitHub Copilot in Visual Studio — April update](https://github.blog/changelog/2026-04-30-github-copilot-in-visual-studio-april-update)
## Model availability changes: upcoming GPT-5.2 deprecation and Student picker adjustments
Admins and educators got a reminder that "which model are we using" is now an operational concern, not trivia. GitHub announced that GPT-5.2 and GPT-5.2-Codex will be deprecated across Copilot experiences on June 1, 2026, with GPT-5.5 and GPT-5.3-Codex positioned as replacements. For Copilot Enterprise, this can require actual admin action: model policies may need updates so the replacement models appear as selectable options in Copilot Chat on github.com and in VS Code. If your org standardizes on a specific model for consistency, testing, or compliance, you should validate those policies before June.
This is the same management lesson as last week, just from the opposite direction: last week''s Opus 4.7 rollout (and replacement of older Opus pickers) showed how quickly model menus change even when capability is "GA." Now deprecations make that churn explicit, and the upcoming token billing makes it more consequential because model choice and context size can translate directly into spend. It also pairs with last week''s data residency reality that some models are not available in some regions, so "standardize on model X" may need a fallback story per tenant and geography.
Separately, GitHub adjusted the Copilot Student model picker by removing GPT-5.3-Codex as a manual selection option (it remains available via auto model selection). GitHub framed this as a temporary reliability and performance measure ahead of the broader usage-based billing transition, which is a good signal that model availability may continue to shift as GitHub tunes capacity and cost controls. The practical takeaway is to avoid teaching workflows or internal docs that depend on students (or any users) always seeing a specific model in a picker, which matches last week''s advice to document intent (speed/cost/reasoning) rather than pinning to exact versions.
- [Upcoming deprecation of GPT-5.2 and GPT-5.2-Codex](https://github.blog/changelog/2026-05-01-upcoming-deprecation-of-gpt-5-2-and-gpt-5-2-codex)
- [Copilot Student: GPT-5.3-Codex removal from model picker](https://github.blog/changelog/2026-04-27-copilot-student-gpt-5-3-codex-removal-from-model-picker)
## Copilot agents and protocols: faster cloud startup and ACP for CLI-driven workflows
GitHub continued tightening the mechanics behind agent workflows, starting with a performance win: Copilot cloud agent now starts more than 20% faster by using optimized runner environments built with GitHub Actions custom images. In practice, that reduces the "waiting for environment" overhead when an agent begins work from an issue, a PR, or the Agents tab, which is the kind of latency that determines whether teams keep using agent workflows or abandon them after the novelty wears off. It also follows naturally from last week''s expansion of cloud-agent work inside github.com (merge conflict fixes and PR maintenance via `@copilot`): as more workflows depend on the cloud agent, startup time becomes a reliability feature, not a nice-to-have.
On the extensibility side, a community deep dive explained how GitHub Copilot CLI can run as an Agent Client Protocol (ACP) server. ACP is presented as a standard way for IDEs, CI/CD pipelines, and custom tools to connect to an agent over a streaming interface (NDJSON over stdin/stdout), with session handling and permissions as first-class concerns. This is a close cousin to last week''s theme of turning Copilot into a configurable runtime with portable integrations (remote-controlled CLI sessions, MCP tooling, skills and plugin catalogs). For teams experimenting with "Copilot in CI" or building internal developer tools, the interesting part is the protocol shape: it suggests a path to swapping clients in and out while keeping the agent integration consistent, rather than building one-off glue for every environment.
- [Copilot cloud agent starts 20% faster with Actions custom images](https://github.blog/changelog/2026-04-27-copilot-cloud-agent-starts-20-faster-with-actions-custom-images)
- [From Terminal to Autonomous Coding: Mastering GitHub Copilot CLI ACP Server](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-terminal-to-autonomous-coding-mastering-github-copilot-cli/ba-p/4508604)
## Other GitHub Copilot News
Copilot''s role in education got another careful look, focusing less on whether students "should" use AI assistants and more on how educators can design assignments and guidelines that reduce over-reliance and integrity problems while still letting Copilot lower friction for experimentation, debugging, and iteration. This fits with last week''s training and adoption coverage that focused on safe, repeatable usage (instructions, skills, and workflow structure) rather than only "better prompts," and it becomes more relevant as token billing makes usage patterns (and incentives) more visible.
- [GitHub Copilot in the Classroom: Help or Hindrance?](https://dellenny.com/github-copilot-in-the-classroom-help-or-hindrance/)',
    'This week’s roundup is about turning agentic tooling into something teams can run, budget, and govern. GitHub Copilot’s shift to token-based billing and AI Credits makes cost a first-class part of rollout checklists, especially as agent-style IDE and PR workflows expand and code review begins consuming both AI Credits and GitHub Actions minutes. On the platform side, GPT-5.5 in Microsoft Foundry, Microsoft Agent Framework 1.0, and A2A/MCP interoperability point toward more standardized agent runtimes, while Azure and Fabric updates reinforce the same operational theme: tighter identity, clearer observability, and more precise controls in both connected and constrained environments.',
    1777878000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-05-04', 'TechHub',
    'TechHub', '5B7D2AAC826ACDB2122643ADF2F5C6F22E1599417E5DF5A921526801FFE67BDF', ',GitHub Copilot,Usage Based Billing,Tokens,GitHub AI Credits,Cost Governance,Copilot Code Review,GitHub Actions Minutes,Visual Studio 2026,Copilot Cloud Agent,Custom Agents,Debugger Agent,Model Deprecation,GPT 5.5,Copilot CLI,Agent Client Protocol,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-04-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-04-27', 'roundups', 'Weekly GitHub Copilot Roundup: Agents Grow Up, Limits Move In',
    'This week’s roundup is about the trade-offs that show up when agents move from demos to daily work: more surfaces, more automation, and more reasons to enforce limits and policies. GitHub Copilot expanded agent experiences and model options (including GPT-5.5 GA), but it also introduced tighter individual usage controls and shifting access to premium Claude Opus models. On the Microsoft side, Azure AI Foundry, Agent Framework, and Fabric leaned into governed tool execution through MCP, with secure networking, managed identity, and outbound restrictions becoming default expectations. We close with the less glamorous but essential work of reliability and security: upcoming GitHub protocol and token changes, DevSecOps tuning via CodeQL and dependency graphs, and Defender research that turns real intrusion chains into actionable hunts and containment steps.
<!--excerpt_end-->
GitHub Copilot had a split week of "more capability" and "more constraint": new models and agent experiences kept expanding where Copilot can help, while plan changes for individuals introduced tighter usage controls and reduced access to some high-end Claude Opus options. That tension builds directly on last week''s themes: as Copilot spreads into more surfaces (VS Code agents, Copilot CLI with BYOK/offline, GitHub Mobile, and MCP-backed tooling), GitHub is simultaneously tightening governance and predictability through policies, usage reporting, and now individual plan limits.
## Copilot Individual plans: paused signups, tighter usage limits, and Opus model changes
GitHub is pausing new signups for Copilot Individual plans (including Pro, Pro+, and Student) and tightening token-based usage limits for existing users, with in-product warnings designed to help you understand when you are approaching limits before features are restricted. After last week''s note about enforced limits and Opus 4.6 Fast retirements, this is the more formal follow-through: the "limits are real" message is now paired with clearer UX signals and plan-level changes that will affect day-to-day workflows for heavy users.
The changes put more emphasis on how much you use Copilot over time, not just whether you have a paid plan, so heavy usage in long chat sessions, large context windows, or frequent premium model calls is more likely to trip limits. This matters more now that last week''s Copilot CLI updates made longer-running terminal agent workflows (including cross-model review and MCP tool loops) more common. Those workflows can be token-hungry by design, so the practical impact is that individuals will need to be more deliberate about when they run "agentic" sessions versus smaller, scoped prompts.
The updated docs and announcements also clarify how Copilot tracks usage across "session" limits and "weekly" limits, which matters if your workflow mixes short completions with longer chat-based debugging or refactoring sessions. Practically, the guidance is to watch the usage indicators inside VS Code and Copilot CLI, then reduce consumption by shortening prompts, trimming context, and avoiding unnecessary retries when you are iterating on the same answer. That advice aligns with last week''s push toward more explicit per-session controls (like VS Code agent autonomy and "thinking effort") because teams and individuals are being nudged toward intentional configuration, not defaulting to the most expensive mode for everything.
Alongside usage limits, GitHub is adjusting model availability: some Claude Opus models are being removed from Copilot Pro now, with additional removals planned for Pro+. If you rely on Opus for specific kinds of reasoning or writing quality, this is the week to double-check what your plan still exposes in the model picker and to plan for fallbacks (both in day-to-day chat and in any saved workflows or team guidance that assumes Opus is always available). For some users, last week''s Copilot CLI BYOK/local-model support becomes a more practical escape hatch here, since you can route workloads to models you control (where policy allows) rather than depending on a specific Copilot-hosted Opus SKU staying available.
- [Changes to GitHub Copilot plans for individuals](https://github.blog/changelog/2026-04-20-changes-to-github-copilot-plans-for-individuals)
- [Changes to GitHub Copilot Individual plans](https://github.blog/news-insights/company-news/changes-to-github-copilot-individual-plans/)
## Model updates: GPT-5.5 reaches GA with premium multipliers and admin controls
GPT-5.5 is now generally available in GitHub Copilot for Pro+, Business, and Enterprise, rolling out gradually. This continues last week''s "model churn needs knobs" storyline (persistent per-session settings like "thinking effort", plus explicit model pickers) but adds a stronger economic signal: the model choice is not only about quality, it is directly tied to premium consumption.
For developers, that means a newer default option in the model picker for tasks that benefit from stronger reasoning or more consistent long-form output, but it also comes with a cost-control lever: premium requests to GPT-5.5 apply a 7.5x multiplier. In practice, teams that standardize on GPT-5.5 for everything may hit premium usage ceilings sooner than expected, so it is worth being intentional about when you choose it (complex debugging, multi-file refactors, architecture decisions) versus when a lighter model is sufficient (small edits, quick explanations, straightforward unit tests). This pairs neatly with last week''s cross-model "Rubber Duck" review idea in Copilot CLI: separate "builder" and "reviewer" models, and reserve the expensive calls for the checkpoints that benefit most.
For Business and Enterprise, GPT-5.5 availability is not only a user choice - it is governed by policy. Admins need to enable it via an organization policy toggle, so if the model does not show up for your users, the fix is likely in Copilot policy configuration rather than client updates. That matches last week''s arc where governance moved closer to the workflow (agent permissions, sandboxing for MCP servers, runner/firewall controls, and expanded usage metrics) and now extends into model access itself.
- [GPT-5.5 is generally available for GitHub Copilot](https://github.blog/changelog/2026-04-24-gpt-5-5-is-generally-available-for-github-copilot)
## Copilot + Azure Developer CLI (azd): AI-assisted scaffolding and deployment troubleshooting
Microsoft is pushing Copilot deeper into the "build and ship" loop for Azure apps by integrating it into the Azure Developer CLI (azd). This is a direct continuation of last week''s MCP + Azure story, where MCP Apps and Azure MCP Server examples showed how to connect tools and guardrails. Here, instead of a standalone tutorial project, the integration lands in a tool many teams already use to scaffold and deploy.
The new flow brings Copilot into `azd init` so you can scaffold a project with a Copilot-assisted setup that produces the core IaC and configuration artifacts (including `azure.yaml` and Bicep). This is especially useful when you have a destination architecture in mind but do not want to hand-assemble the initial wiring for services, deployment steps, and environment configuration. It also complements last week''s "assessment as source of truth" modernization framing: both approaches produce repo artifacts that you can review, version, and feed into repeatable delivery.
The other half of the integration targets the most time-consuming part of cloud delivery: diagnosing failed deployments. In-terminal troubleshooting can explain errors, guide you through likely causes, diagnose common Azure deployment failures, and in some cases optionally apply fixes. That "optionally apply" detail matters because it mirrors the guardrail pattern from last week (explicitly allowed actions through constrained tool surfaces): Copilot can help execute, but the workflow is designed so you can see what will change and decide when to accept it.
Under the hood, the integration is built around the Model Context Protocol (MCP), which is becoming a common way to connect agents to tools in a structured, auditable manner. For teams experimenting with agent-driven workflows, this is a concrete example of how to attach Copilot to a real operational surface area (Azure deployments) rather than keeping it confined to IDE chat. It also reinforces last week''s theme that MCP is shifting from "tool connectivity" to "operationalized tooling" with clearer boundaries and more predictable execution.
- [GitHub Copilot meets Azure Developer CLI: AI-assisted project setup and error troubleshooting](https://devblogs.microsoft.com/azure-sdk/azd-copilot-integration/)
## IDE and client experiences: JetBrains agent mode preview, VS Code Insiders CLI updates, and SSMS context gains
GitHub Copilot for JetBrains IDEs added inline agent mode in public preview, bringing more agent-style interactions directly into the editor rather than pushing everything into chat panels. Coming right after last week''s VS Code focus on agent permissions and autonomy modes, JetBrains moving toward inline agent flows reinforces the cross-IDE pattern: agents are becoming a first-class interaction model, and the key product work is increasingly about how you control and audit them, not just whether they exist.
The same update improves Next Edit Suggestions by showing in-editor previews and adding navigation for edits that land far from your current cursor, which is important when Copilot proposes multi-file or non-local changes. It also introduces new global and granular controls for auto-approving tool calls, giving you more control over when an agent can take actions without prompting (useful for trust-and-verify workflows, and critical for teams that need tighter guardrails). That connects cleanly to last week''s "explicit autonomy choice up front" in VS Code sessions: different clients are converging on the same question, "what is the agent allowed to do without asking?"
On the VS Code side, the 1.118 Insiders release notes include multiple Copilot CLI and Agents app changes: adoption of a session-title API, keybindings for switching sessions in the Agents app, auto model selection support, and model badges in chat so you can see what is being used without digging through settings. Those are small, practical steps toward the "resumable and auditable sessions" direction highlighted last week (titles and badges make logs and handoffs easier), and they matter more now that model availability and premium multipliers can change the "cost profile" of a session without you noticing.
There is also a Copilot CLI SDK change to resolve `node-pty` via `hostRequire`, which is the kind of plumbing fix that tends to matter most in enterprise setups or custom VS Code distributions where native modules and terminal integration can be brittle. Last week''s Copilot CLI story emphasized configuration correctness (offline/BYOK behavior and avoiding accidental fallback). This week adds reliability at the integration layer, which is the other half of "predictable agent runtimes."
For data-focused workflows, Copilot in SQL Server Management Studio (SSMS) gained a practical context improvement: Copilot Chat can now reference the Results pane, letting you ask questions about execution plans, the Messages tab output, and the results grid directly. That reduces the back-and-forth of copying query results or plan snippets into chat, and it aligns Copilot more closely with the way DBAs and backend developers actually debug performance regressions and query behavior. It also extends last week''s "verify what context was sent" theme (via debug panels and skills/instructions) by making a high-signal context source (results and plans) directly available to the assistant.
- [Inline agent mode in preview and more in GitHub Copilot for JetBrains IDEs](https://github.blog/changelog/2026-04-24-inline-agent-mode-in-preview-and-more-in-github-copilot-for-jetbrains-ides)
- [Visual Studio Code 1.118 (Insiders) release notes](https://code.visualstudio.com/updates/v1_118)
- [GitHub Copilot in SSMS: Interacting with the Results Pane | Data Exposed](https://www.youtube.com/watch?v=Fkxhv7xG_2A)
## Copilot on the web: improved stack trace debugging and structured root-cause analysis
Copilot Chat on github.com now does a better job recognizing stack traces and turning them into a more structured debugging output. This feels like the web counterpart to last week''s "shorter safe-to-merge loop" work (faster validations, more security workflows routed to agents) because it targets the same bottleneck: taking raw failure output (stack traces, logs) and converting it into an actionable plan you can verify.
Instead of returning a generic explanation, the updated experience aims to produce a root-cause analysis that includes code evidence, an explicit confidence level, suggested fixes, and follow-up checks you can perform to validate the diagnosis. This format is useful when you are debugging in a browser context (issues, PRs, Actions logs) because it encourages a repeatable workflow: identify the failure point, assess confidence, apply a fix, and confirm with targeted verification steps. It also echoes last week''s push toward traceability and reviewable artifacts (plans, validations, logs): the output is structured in a way that is easier to paste into an issue comment or PR discussion and revisit later.
- [Better debugging with GitHub Copilot on the web](https://github.blog/changelog/2026-04-23-better-debugging-with-github-copilot-on-the-web)
## Other GitHub Copilot News
VS Code Agent Skills got a clearer, more hands-on explanation this week, showing how to package instructions, scripts, and resources into reusable, multi-step workflows you can share and iterate on, which helps teams standardize "how we do this task" beyond a single prompt. That lands right on top of last week''s thread about scaling agent behavior through repo/org instructions, skills files, and repeatable loops (like Spec-Kit extensions): the story is moving from "you can customize Copilot" to "you can package a workflow and maintain it like code."
The C# Dev Kit team''s deep dive on building Node.js N-API addons with .NET Native AOT is not a Copilot feature update, but it is relevant to Copilot-heavy teams working across JS and .NET stacks because it shows a practical pattern for bridging ecosystems (exporting `napi_register_module_v1`, calling Node-API via `LibraryImport`, and handling UTF-8 string marshalling) that Copilot can assist with during implementation and code review. In the context of this week''s plan and model constraints, it is a good reminder that high-leverage Copilot use is often about narrowing the problem (clear API boundaries, concrete interop steps) so you spend fewer tokens getting to a correct implementation.
- [Agent Skills Explained in 5 Minutes | Ep 3 of 8](https://www.youtube.com/watch?v=mPjTZviv23s)
- [Writing Node.js addons with .NET Native AOT](https://devblogs.microsoft.com/dotnet/writing-nodejs-addons-with-dotnet-native-aot/)',
    'This week’s roundup is about the trade-offs that show up when agents move from demos to daily work: more surfaces, more automation, and more reasons to enforce limits and policies. GitHub Copilot expanded agent experiences and model options (including GPT-5.5 GA), but it also introduced tighter individual usage controls and shifting access to premium Claude Opus models. On the Microsoft side, Azure AI Foundry, Agent Framework, and Fabric leaned into governed tool execution through MCP, with secure networking, managed identity, and outbound restrictions becoming default expectations. We close with the less glamorous but essential work of reliability and security: upcoming GitHub protocol and token changes, DevSecOps tuning via CodeQL and dependency graphs, and Defender research that turns real intrusion chains into actionable hunts and containment steps.',
    1777273200, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-04-27', 'TechHub',
    'TechHub', '4D84702F2E51B45EB9095F0ACF5A977D8F72F52C93478540AD0BC30BECE3CD0A', ',GitHub Copilot,Copilot Individual,Usage Limits,Token Based Billing,Claude Opus,GPT 5.5,Premium Multipliers,Copilot Policies,MCP,VS Code Agents,Copilot CLI,JetBrains IDEs,Azure Developer CLI,Bicep,SQL Server Management Studio,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-04-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-04-20', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, Skills, and Governance',
    'This week''s Copilot updates were less about new chat features and more about making Copilot usable in operational workflows: agents that work in PRs and terminals, stronger admin controls (including data location), and portable "skills" and tool catalogs that keep behavior consistent. This continues last week''s thread: as Copilot expands from IDE chat and autocomplete into PR and branch agents, CLI orchestration, and MCP tooling, GitHub is filling in the gaps around control, traceability, and rollout management.
<!--excerpt_end-->
## Copilot agents move deeper into PRs and github.com workflows
After last week''s work to make cloud-agent workflows faster and easier (parallel validations, Dependabot-to-agent remediation, and Mobile flows), GitHub is shifting more agent work onto github.com. A new **"Fix with Copilot"** button resolves PR merge conflicts using the **Copilot cloud agent**: click the button, post a prefilled PR comment, and the agent resolves conflicts, runs builds and tests, and pushes updates back to the PR. The goal matches last week''s "shorter path to merge-ready" idea: fewer local context switches, while normal commits and repo validations still apply.
The cloud-agent path also expands through `@copilot` mentions in PR conversations for common maintenance tasks such as fixing failing GitHub Actions tests, addressing review comments, or adding unit tests for edge cases. This carries last week''s "agent-in-the-workflow" approach beyond security remediation into everyday PR work, while keeping changes inside the standard PR audit trail. Admin control remains a core requirement: Copilot cloud agent is available on paid plans, but **Copilot Business/Enterprise admins must enable cloud agent** before developers can use these PR commands and buttons.
GitHub also added **per-run model selection for the third-party Claude and Codex agents on github.com**, so you choose the model at kickoff instead of relying on a single default. This matches last week''s theme of making choices explicit (permission modes, thinking effort, BYOK/provider controls): per-run selection makes it clearer what you ran and why outputs differ. Enterprise enablement remains policy-driven: admins must enable Anthropic Claude and/or OpenAI Codex policies, and repo owners must enable the agent under **Settings -> Copilot -> Cloud agent**.
- [Fix merge conflicts in three clicks with Copilot cloud agent](https://github.blog/changelog/2026-04-13-fix-merge-conflicts-in-three-clicks-with-copilot-cloud-agent)
- [Enable Copilot cloud agent via custom properties](https://github.blog/changelog/2026-04-15-enable-copilot-cloud-agent-via-custom-properties)
- [Model selection for Claude and Codex agents on github.com](https://github.blog/changelog/2026-04-14-model-selection-for-claude-and-codex-agents-on-github-com)
## Governance and compliance: data residency, FedRAMP routing, and controlled rollouts
This week''s governance updates continue last week''s pattern of controls catching up with autonomy (stronger VS Code agent permissions, offline/BYOK in CLI, more reporting). Copilot now supports **data residency for US and EU regions**, plus routing through **FedRAMP Moderate-authorized model hosts and infrastructure** for US government compliance. GitHub says all GA Copilot capabilities are included (Agent mode, inline suggestions, Copilot Chat, cloud agent, Code Review, PR summaries, Copilot CLI), and requests route to compliant endpoints in the selected geography. In other words, the "where does it go?" control is aligning with the "what can it do?" controls across IDE, CLI, and github.com.
Supported models span OpenAI and Anthropic (including **GPT-5.4**, **Claude Sonnet 4.6**, **Claude Opus 4.6**) with a model-by-region matrix. Teams should expect constraints, especially if they used last week''s model configurability: **Gemini isn''t supported** (there are no suitable data-resident inference endpoints in this setup), and new models may show up later in resident regions. There is also a cost change: data residency/FedRAMP requests apply a **10% increase to the model multiplier** for premium request accounting, which ties back to last week''s budgeting and limits planning (quotas, deprecations, plan standardization).
For customers on **Data Resident GitHub Enterprise Cloud** (ghe.com / "Proxima"), the practical next step is enabling enforcement policies like **"Restrict Copilot to Data Residency Models"** so inference and Copilot data stay in-region. Plan for previews and newly released features to land later in data-residency tenants, so enablement guidance may need to differ between standard Enterprise Cloud and ghe.com. That mirrors last week''s split between "the feature exists" and "the feature is manageable at org scale."
- [GitHub Copilot data residency in US + EU and FedRAMP compliance now available](https://github.blog/changelog/2026-04-13-copilot-data-residency-in-us-eu-and-fedramp-compliance-now-available)
- [GitHub Copilot now available with Data Residency in the EU](https://jessehouwing.net/github-copilot-now-with-data-residency-in-europe/)
## The “skills” ecosystem grows up: Custom Skills, `gh skill`, and enterprise plugin catalogs
Copilot customization is moving from one-off prompting to **versioned, shareable runbooks**, which matches last week''s IDE and CLI direction around instruction management, better agent hooks, and more repeatable sessions with artifacts. GitHub Copilot **Custom Skills** are the runbook unit for Agent mode, typically a folder under `.github/skills/<name>/` with required **SKILL.md** plus optional scripts, templates, and resources. The guidance is specific: Copilot first reads only the skill''s **name + description (~100 tokens)** to decide whether to load it, so descriptions should lead with trigger keywords and phrasing, and the skill should load referenced files only when needed. The "deployment-health" example encodes a real check: a Python script reads endpoints, measures latency and thresholds, generates an HTML report, and Copilot runs it and summarizes results. That aligns with last week''s focus on inspectable, repo-resident artifacts.
Managing skills is the next step, and GitHub addresses it with **`gh skill`** in **GitHub CLI v2.90.0+** (public preview). It reads like last week''s governance follow-through, but applied to the customization supply chain: install, update, and publish workflows with provenance controls. You can pin installs to a **git tag** or **commit SHA**, detect upstream changes via **git tree SHA**, and store provenance in SKILL.md frontmatter so the origin remains even if copied. For authors, `gh skill publish` validates against the **agentskills.io** spec and pushes security practices (tag protection, secret scanning, code scanning), with optional immutable releases tied to tags. GitHub also calls out the risk model: skills can contain malicious scripts or prompt injection, so `gh skill preview` is recommended before installing. This matches last week''s safety framing (sandboxing MCP servers, safer execution, better debugging).
Separately, Azure introduced an **Azure API Center plugin marketplace endpoint** (public preview) to help platform teams distribute approved tool extensions. This connects directly to last week''s MCP operationalization theme: API Center provisions a Git-based **`marketplace.git`** endpoint that tools can consume. The post names **Claude Code** and **GitHub Copilot CLI** as consumers: developers add the URL and browse or install plugins from a governed, authenticated source instead of relying on internal docs. The practical result is that skills, MCP servers, and plugins can behave more like managed internal packages with central inventory and consistent installs. That matches what we''re also seeing with PR cloud agents and the CLI as a formal runtime.
- [Supercharge Your Dev Workflows with GitHub Copilot Custom Skills](https://techcommunity.microsoft.com/t5/microsoft-developer-community/supercharge-your-dev-workflows-with-github-copilot-custom-skills/ba-p/4510012)
- [Manage agent skills with GitHub CLI](https://github.blog/changelog/2026-04-16-manage-agent-skills-with-github-cli)
- [Introducing the plugin marketplace for Azure API Center](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-the-plugin-marketplace-for-azure-api-center/ba-p/4512231)
## Copilot CLI: auto model routing, remote-controlled sessions, and “plan → implement” workflows
This week''s CLI changes extend last week''s arc: from "Copilot in the terminal" to a configurable agent runtime (BYOK/local models, offline mode, MCP tools, plugins) with clearer controls. First, **auto model selection** is now GA in Copilot CLI across all plans: set the model to "auto" and each request routes to an appropriate model (currently including **GPT-5.4**, **GPT-5.3-Codex**, **Sonnet 4.6**, **Haiku 4.5**) to improve availability and reduce rate limits. The CLI shows which model handled each request, continuing last week''s theme that traceability matters. Billing follows the chosen model multiplier; auto is limited to 0x-1x models and includes a **10% multiplier discount** for paid subscribers (for example, 1x billed as 0.9 under auto), which ties back to last week''s cost and ops planning.
Second, GitHub is previewing **remote control for Copilot CLI sessions** via `copilot --remote`. The session streams to GitHub and provides a link or QR code to view or control from the web or GitHub Mobile (with prerelease mobile builds). This continues last week''s "Copilot in more places" direction by turning a long-running CLI session into a shared artifact, rather than a separate mobile or web copilot. You can send steering messages (including queued "keep going"), review and edit the plan before execution, switch modes (plan/interactive/autopilot), stop the session, and approve or deny permission prompts, while still respecting existing CLI permissions and Business/Enterprise policies. It is the same "more autonomy with explicit approvals and governance hooks" idea, applied to where supervision happens.
Hands-on build stories show how the modes fit together. One uses **plan mode** to generate `plan.md`, then switches models (for example, Sonnet for planning and **Claude Opus 4.7** for implementation) to build a Node.js terminal UI "emoji list generator" using **@github/copilot-sdk**, **@opentui/core**, and **clipboardy**. That kind of model switching matches last week''s cross-model review idea: planning, implementation, and review as separate phases with checkpoints. Another example (a personal "command center" desktop app) shows a split between supervised changes in VS Code interactive agent mode and background work delegated to Copilot Cloud Agent, along with the practical reminder that agents often add code faster than they remove it, so cleanup still benefits from human refactoring. A short onboarding demo shows Copilot CLI generating a plain-English repo overview from the terminal instead of manual file browsing.
- [GitHub Copilot CLI now supports Copilot auto model selection](https://github.blog/changelog/2026-04-17-github-copilot-cli-now-supports-copilot-auto-model-selection)
- [Remote control Copilot CLI sessions on web and mobile (public preview)](https://github.blog/changelog/2026-04-13-remote-control-cli-sessions-on-web-and-mobile-in-public-preview)
- [Remote control GitHub Copilot CLI sessions on web and mobile (public preview)](https://www.youtube.com/watch?v=dRSHSZ53c1g)
- [Building an emoji list generator with the GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/building-an-emoji-list-generator-with-the-github-copilot-cli/)
- [Build a personal organization command center with GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/build-a-personal-organization-command-center-with-github-copilot-cli/)
- [How a GitHub engineer built an AI productivity hub with Copilot CLI](https://www.youtube.com/watch?v=BDZKubrUO1M)
- [How to understand any codebase in seconds](https://www.youtube.com/shorts/CwpmuUh9izg)
## Models and IDE integrations: Opus 4.7 rollout, Visual Studio changes, and Azure MCP built-in
Model availability and IDE ergonomics shifted this week, connecting to last week''s themes around model churn, explicit settings, and MCP parity across surfaces. GitHub Copilot is rolling out **Claude Opus 4.7** as a GA model across IDEs, CLI, github.com, GitHub Mobile, and Copilot Coding Agent. The picker is also being simplified: **Opus 4.7 will replace Opus 4.5 and 4.6** in the Pro+ model picker over the coming weeks, which reinforces last week''s reminder to document intent (speed/cost/reasoning) rather than hard-coding versions. On cost, Opus 4.7 launches with a **7.5x premium request multiplier** under promo pricing through **April 30**, and Business/Enterprise still require admins to enable the Opus 4.7 policy. That means more model choice, but still behind org policy.
In Visual Studio, two Copilot-adjacent changes stood out. Visual Studio 2022 now ships **Azure MCP tools built in** (via the "Azure development" workload) starting with **VS 17.14.30+**, so you no longer need a separate "Copilot for Azure" extension. This follows last week''s trajectory where MCP becomes part of the default IDE surface rather than an add-on. Once enabled in Copilot Chat''s tool picker, the Azure MCP Server exposes **230+ tools across 45 Azure services** for tasks like listing resources, deploying via **azd** to App Service, running AppLens and health checks, and generating and running **KQL** queries in Log Analytics. Updates now follow the Visual Studio Installer cadence, but tools are off by default and gated by Azure permissions plus sign-in to GitHub and Azure. That matches last week''s framing around explicitly allowed surfaces.
Visual Studio 2026 **18.5** changes completion behavior by prioritizing IntelliSense list completions so Copilot inline suggestions do not compete for the same accept gesture. It also adds a Copilot-driven **debugger agent** workflow: from an issue link or prompt, Copilot forms hypotheses, sets conditional breakpoints, runs under the debugger, and proposes fixes based on what it observes. This matches last week''s theme of richer debugging and more autonomy, but with a loop driven by runtime state rather than only static edits. Coverage also notes ongoing friction (reduced theme token granularity impacting readability, more forceful update prompts), which matters because Copilot usage depends on IDE stability.
- [Claude Opus 4.7 is generally available in GitHub Copilot](https://github.blog/changelog/2026-04-16-claude-opus-4-7-is-generally-available)
- [Azure MCP tools now ship built into Visual Studio 2022 — no extension required](https://devblogs.microsoft.com/visualstudio/azure-mcp-tools-now-ship-built-into-visual-studio-2022-no-extension-required/)
- [Visual Studio 18.5 adds Copilot debugger agent workflow, but devs complain about theme contrast and forced updates](https://www.devclass.com/ai-ml/2026/04/17/visual-studio-185-lands-with-ai-debugging-at-a-price-devs-still-feeling-blue/5218068)
## Other GitHub Copilot News
Security and compliance are showing up more often in day-to-day agent adoption guidance, not only as admin settings. This matches last week''s shift toward governance that you can measure (usage metrics, security entry points, constrained capabilities with audit trails). A DevOps playbook argues that agents amplify existing engineering discipline, recommends treating repos as explicit agent interfaces (via `.github/copilot-instructions.md` and spec and constraint files), and using CI/CD as an active verifier with structural, semantic, and provenance checks plus defenses against prompt injection and supply-chain abuse.
- [DevOps Playbook for the Agentic Era](https://devblogs.microsoft.com/all-things-azure/agentic-devops-practices-principles-strategic-direction/)
GitHub Code Quality public preview added small but practical UI improvements. Standard findings now support file-path search, bulk dismiss and reopen, and richer diagnostic context, with Copilot Autofix still attached to each finding to shorten the triage-to-fix loop. This continues last week''s "shorter remediation loop" story (Ask Copilot in assessments, agent-assigned Dependabot alerts) by moving Copilot closer to fix queues.
- [''GitHub Code Quality: Improvements to standard findings in public preview''](https://github.blog/changelog/2026-04-14-github-code-quality-improvements-to-standard-findings-in-public-preview)
A .NET-focused comparison walks through tradeoffs between a single chat agent, an MCP/function-calling tool-using agent, and a multi-agent "team," using a **.NET Aspire + ASP.NET Core Minimal API** rate-limiting implementation (with integration tests). It complements last week''s multi-agent and checkpoint direction (CLI `/fleet`, cross-model review patterns) by helping decide when extra agent structure is worth the added complexity.
- [Single-agent, tools, or a team? A practical comparison of AI coding setups](https://hiddedesmet.com/single-agent-tools-or-a-team)
Docker Desktop''s Docker Sandbox microVMs are presented as a way to let Copilot-driven refactoring run `docker build`, Compose, and tests without mounting the host Docker socket (and without giving root-equivalent access to the host daemon). This matches last week''s containment theme (sandboxing MCP servers, offline/BYOK controls, runner/firewall placement): as agents do more real work, teams are putting safer execution boundaries around builds and infrastructure access.
- [''Best of Both Worlds for Agentic Refactoring: GitHub Copilot + MicroVMs via Docker Sandbox''](https://devblogs.microsoft.com/all-things-azure/best-of-both-worlds-for-agentic-refactoring-github-copilot-microvms-via-docker-sandbox/)
A GitHub video roundup is mainly useful for two Copilot-adjacent milestones: Copilot SDK public preview and Copilot CLI adding BYOK/local-model support (plus a reminder to tighten dependency hygiene after recent npm incidents). It is effectively a recap of last week''s two main threads: SDK-standardized sessions, tools, and tracing, and the CLI as a configurable runtime. It also shows how quickly these topics are turning into baseline knowledge.
- [''The Download: Copilot SDK, Claude Mythos, AI models are protecting each other & more''](https://www.youtube.com/watch?v=0vvY2AGg4sM)
Training content continues to address adoption gaps around customization and safe usage. This includes a beginner CLI tutorial on composing instructions, skills, and custom agents; a migration video showing Copilot assisting Oracle-to-PostgreSQL in VS Code while moving to Azure Database for PostgreSQL; and a VS Live playlist with AI/Copilot workflow talks alongside .NET, ASP.NET Core, and GitHub Actions. Alongside last week''s focus on instruction management and troubleshooting (editors, debug logs, verifying sent context), the learning trend is "safe and repeatable," not only "how to prompt."
- [How to use agents, skills, and instructions in Copilot CLI (beginner tutorial)](https://www.youtube.com/watch?v=-yKALFS5ewY)
- [''PostgreSQL Like a Pro: Migrate to managed PostgreSQL on Azure (incl. AI-assisted Oracle-to-Postgres demo)''](https://www.youtube.com/watch?v=Suvakz3yJgM)
- [''From AI to .NET: 20 VS Live! Las Vegas Sessions You Can Watch Now''](https://devblogs.microsoft.com/visualstudio/from-ai-to-net-20-vs-live-las-vegas-sessions-you-can-watch-now/)',
    'This week''s Copilot updates were less about new chat features and more about making Copilot usable in operational workflows: agents that work in PRs and terminals, stronger admin controls (including data location), and portable "skills" and tool catalogs that keep behavior consistent. This continues last week''s thread: as Copilot expands from IDE chat and autocomplete into PR and branch agents, CLI orchestration, and MCP tooling, GitHub is filling in the gaps around control, traceability, and rollout management.',
    1776668400, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-04-20', 'TechHub',
    'TechHub', 'C19E72BB63513977E4D4D23A56F705C28136048CB559FB4B0813CB2FEED65311', ',GitHub Copilot,Copilot Cloud Agent,Pull Requests,GitHub Actions,GitHub CLI,Copilot CLI,Custom Skills,Agent Mode,Data Residency,FedRAMP,Model Selection,Claude Opus 4.7,GPT 5.4,MCP,VS,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-04-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-04-13', 'roundups', 'Weekly GitHub Copilot Roundup: Agents Expand, Controls Catch Up',
    'This week''s Copilot story was less about one headline and more about Copilot being available in more places: stronger agent controls in VS Code and GitHub Mobile, deeper terminal workflows through Copilot CLI (including offline/BYOK), and more admin/reporting to track adoption and outcomes. It follows last week''s theme that as Copilot grows from chat/autocomplete into branch/PR agents, multi-agent CLI orchestration, and MCP-backed tooling, GitHub is closing gaps in control (permissions, firewall/runner placement), traceability (sessions/logs/telemetry), and administration (instructions and usage reporting).
<!--excerpt_end-->
## Copilot in VS Code: more autonomous agents, richer debugging, and better customization
Building on last week''s VS Code work (session restore with diffs/undo, background terminal completion notices, integrated browser context, and SSH agent host mode), the March-early April releases (v1.111-v1.115) expanded agent sessions, starting with per-session **Agent permissions** to control autonomy. Sessions can run as **Default**, **Bypass Approvals**, or **Autopilot** (public preview). Autopilot lets the agent approve its own actions, retry on errors, and continue until completion, so you can run longer multi-step work with fewer prompts when that fits the task. It continues the "resumable and auditable sessions" direction from last week, but with an explicit autonomy choice up front.
VS Code workflow changes also go beyond agents. The editor now supports **integrated browser debugging** through a new `editor-browser` debug type that works with many existing Chrome/Edge configurations. With self-signed cert support for local HTTPS and improved tab management (Quick Open, Close All, title-bar shortcuts), web debugging can happen with fewer context switches. This connects to last week''s "integrated browser as session context" idea: debugging context and "agent saw these tabs" are starting to land in the same editor-native loop.
On models, reasoning models now have a persistent **"thinking effort"** control in the model picker (for example, Claude Sonnet 4.6 and GPT-5.4). That makes it easier to trade speed/cost for deeper reasoning depending on the task. It mirrors last week''s model churn point: explicit, repeatable per-session settings (and now "effort") help teams keep outcomes steadier as model options change. Copilot Chat also gained more multimodal support: you can attach **screenshots and videos**, and agents can return images/recordings in a carousel (zoom/pan/thumbnails). That is useful for UI bugs and visual diffs, and it extends last week''s "blob attachments" work in the Copilot SDK preview.
Extensibility and team-scale management also moved forward. **Nested subagents** can invoke other subagents (`chat.subagents.allowInvocationsFromSubagents`), MCP integration has better parity (MCP servers configured in VS Code are available to Copilot CLI and Claude agent sessions), and **session forking** makes it easier to try alternatives without losing the original thread. Customization and diagnostics improved as well: a **Chat customizations editor** (preview) centralizes instructions/agents/skills/plugins plus marketplace browsing, the `#codebase` tool moved to one auto-managed semantic index, and `/troubleshoot` can analyze agent debug logs (including past sessions) to explain issues like ignored instructions or slow responses. Security and governance also improved with **sandboxing for local MCP servers** (macOS/Linux), better monorepo discovery for shared instructions/agents/hooks, and **agent-scoped hooks** (preview) via YAML frontmatter in `.agent.md`. Overall, this reads like the IDE-side follow-through on last week''s scaling story: last week expanded repo/org instruction patterns, and this week makes them easier to manage, safer to run, and easier to debug in VS Code.
- [GitHub Copilot in Visual Studio Code, March Releases](https://github.blog/changelog/2026-04-08-github-copilot-in-visual-studio-code-march-releases)
## Copilot CLI: BYOK/local models, cross-model review, plugins, and MCP tool connections
This week''s Copilot CLI updates extend last week''s "CLI as an agent runtime" direction, where `/fleet` introduced multi-agent orchestration and the Copilot SDK standardized sessions, tool invocation, and tracing. The CLI is moving from "Copilot in the terminal" to a configurable, extensible agent platform with review flows. The main change is that Copilot CLI now supports **BYOK providers and fully offline local-model workflows**. You can route to providers you already use (Azure OpenAI, Anthropic, OpenAI-compatible endpoints) or run locally (Ollama, vLLM, Foundry Local). For regulated or restricted environments, `COPILOT_OFFLINE=true` prevents any GitHub server contact (telemetry disabled) and forces the CLI to use only the configured provider, which helps avoid accidental cloud fallback. GitHub sign-in is optional under BYOK, though GitHub-backed features (like `/delegate`, Code Search, and the GitHub MCP server) still require GitHub connectivity and identity. The CLI also will not silently fall back on invalid configuration, which helps keep CI and locked-down machines more predictable. In practice, last week''s "agent sessions need controls" now includes provider/connectivity control, not only model choice.
For developers testing BYOK, the LM Studio walkthrough makes the "local OpenAI-compatible endpoint" path concrete and calls out a common pitfall: if you do not set `COPILOT_OFFLINE=true`, you may still allow cloud routing. The Azure AI Foundry (Azure OpenAI) setup shows the hosted alternative with its own details: you must use the **deployment-scoped URL** (`.../openai/deployments/<deployment>/v1`) and set `COPILOT_MODEL` to the *deployment name*, not the catalog model name. This "details matter" theme matches last week''s `/fleet` guidance (separate contexts, shared filesystem, collision avoidance). As CLI use becomes more agent-driven, configuration edge cases matter as much as prompting.
For agent quality, Copilot CLI added an experimental "Rubber Duck" workflow that **reviews an orchestrator model''s output using a different model family** at checkpoints (plan review, post-implementation review, test review, or when stuck). In the current experiment, using a Claude model as orchestrator triggers GPT-5.4 as reviewer. The goal is to catch plan mistakes and cross-file edge cases earlier in longer runs without constant interruption. This pairs with last week''s "plan first" additions (cloud agent drafting plans, SDK demo emphasizing iterative planning). Rubber Duck turns the "builder vs critic" idea into a structured flow by separating models.
The "GitHub Checkout" episode added more context on the CLI''s experimental direction: **plugins and a plugin marketplace**, `/chronicle` for capturing "self-healing" task context across longer runs, and background execution like `/fleet` and "autopilot" for wider refactors. For tool integration, the beginner MCP episode focuses on using `/mcp` with local/remote MCP servers (for example, Playwright and Svelte) so Copilot CLI can invoke tools in an iterative loop rather than only suggesting commands. This continues last week''s MCP thread (tools + enforcement + audit) but shifts emphasis from "connect tools" to "make the terminal agent reliably use them."
Finally, the "Copilot CLI for Beginners" post reiterates the core workflow: install (`npm install -g @github/copilot`), `/login`, folder permissions, and `/delegate` to hand a task to the cloud agent to work in a branch and open a draft PR. That sets up the deeper BYOK/MCP/agent features described above. The tie to last week''s branch-first cloud agent story is explicit: the CLI is increasingly the "remote control" for branch/PR agent workflows, now with more options for models and tools.
- [GitHub Copilot CLI now supports BYOK and local models](https://github.blog/changelog/2026-04-07-copilot-cli-now-supports-byok-and-local-models)
- [Using GitHub Copilot CLI with Local Models (LM Studio)](https://dev.to/playfulprogramming/using-github-copilot-cli-with-local-models-lm-studio-5e3b)
- [Using GitHub Copilot CLI with Azure AI Foundry (BYOK Models) – Part 2](https://dev.to/playfulprogramming/using-github-copilot-cli-with-azure-ai-foundry-byok-models-part-2-4e5n)
- [GitHub Copilot CLI adds Rubber Duck for cross-model plan and code review (experimental)](https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-combines-model-families-for-a-second-opinion/)
- [Copilot CLI update: chronicle, plugins, and fleet mode | GitHub Checkout](https://www.youtube.com/watch?v=9oAcwmrUE44)
- [How to use MCP servers with GitHub Copilot CLI (Beginner Tutorial)](https://www.youtube.com/watch?v=DtQjVIRRszM)
- [GitHub Copilot CLI for Beginners: Getting started with GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-for-beginners-getting-started-with-github-copilot-cli/)
- [Turning a codebase into an 80s dungeon crawler with Copilot CLI](https://www.youtube.com/watch?v=WekOupjeM6E)
## MCP and agent UI inside Copilot: interactive apps, dashboards, and Azure execution guardrails
This month''s MCP work moved in three directions: interactive UI inside Copilot Chat, MCP as a deployment surface, and MCP as a governance boundary for infrastructure actions. It continues last week''s MCP narrative (connect tools, then operationalize with versioned config and constrained capabilities), with more attention on what users can see in the client and how ops teams can bound what agents are allowed to do.
For "UI inside chat," the Azure App Service tutorial walks through building an **MCP App** that pairs tools with UI resources so clients like **VS Code Copilot Chat** can render an interactive widget inline (also compatible with Claude Desktop and ChatGPT). The sample is an ASP.NET Core/.NET 9 MCP server with a weather tool. UI is delivered via a `ui://` resource and returned via MCP UI metadata (`_meta.ui.resourceUri`) plus `text/html;profile=mcp-app`. It covers packaging tool + UI, running locally through `.vscode/mcp.json`, and deploying with `azd up` to App Service using Bicep templates, including App Service settings that matter for interactive calls (Always On, Easy Auth with Entra ID, deployment slots, Application Insights). It also links back to last week''s azd "Set up with GitHub Copilot" work: azd is becoming a common path for scaffolding and shipping MCP-backed agent apps.
For "visual output," Visuals MCP added a **chart tool** that can render charts or multi-card dashboards in Copilot Chat in VS Code, with Storybook examples for payload validation. With support for line/bar/area/pie/scatter/composed charts, layouts, legends/tooltips/gridlines, dual-axis, theme awareness, and CSV/JSON plus JPG/SVG exports, teams can keep analysis and visualization inside the editor. This complements last week''s "deep research" and "cited Markdown report" patterns: once research outputs become artifacts, charts and dashboards are a natural next step for reviewable results without leaving the session.
For "platform actions with guardrails," the platform engineering walkthrough shows Copilot agents packaged in a repo (Azure''s `git-ape`) and wired to the **Azure MCP Server** so agents execute Azure workflows through an explicitly enabled surface. The example runs Azure MCP in namespace mode, allows writes (`azureMcp.readOnly: false`), and whitelists services like `deploy`, `group`, `subscription`, `functionapp`, `storage`, `sql`, and `monitor`. That reinforces a pattern where natural-language infra work flows through constrained, reviewable capabilities. This matches last week''s governance framing (versioned playbooks and enforcement) and mirrors GitHub-side guardrails (cloud agent firewall and runner controls): agents can act, but only through explicitly allowed surfaces.
- [Build and Host MCP Apps on Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-and-host-mcp-apps-on-azure-app-service/ba-p/4509705)
- [Visuals MCP Update: Charts and Dashboards in VS Code](https://harrybin.de/posts/visuals-mcp-charts-dashboard-update/)
- [Putting Agentic Platform Engineering to the test](https://devblogs.microsoft.com/all-things-azure/putting-agentic-platform-engineering-to-the-test/)
## Copilot agents in GitHub workflows: mobile coding, faster validation, and security remediation loops
This week''s workflow updates follow directly from last week''s GitHub.com cloud agent changes (branch-first work without opening a PR, plan-first and deep research sessions, commit signing, plus org firewall/runner controls). The agent is quicker at completing safety checks, fits better into security remediation, and is accessible from more places, especially mobile.
GitHub tightened the "agent wrote code" to "safe to merge" loop. Copilot cloud agent validations now run about **20% faster** by running checks in parallel: CodeQL, secret scanning, GitHub Advisory Database checks, and Copilot code review. That shortens iteration time without changing what is enforced, and repo owners can still tune validations in Copilot -> Cloud agent settings. Combined with last week''s session logs/validation traceability and signed commits, this helps agent work fit more naturally into normal CI timing.
For remediation, Dependabot alerts can now be **assigned to AI coding agents** from an alert detail page. The agent reviews the advisory and repo usage, opens a **draft PR** with a fix, and tries to resolve test failures. Teams can assign multiple agents in parallel to compare approaches (upgrade paths, breaking-change refactors, or downgrades when no patch exists). The intent stays the same: the output is a PR for humans to review. This complements last week''s in-IDE remediation note (Visual Studio fixing vulnerable NuGet packages) by moving the loop directly into the security product.
Copilot also expanded into mobile. GitHub Mobile now supports a cloud agent flow to research a repo, draft a plan, apply changes on a branch, review diffs, and optionally open a PR from the app. It extends last week''s branch-first workflow and makes small fixes and follow-ups more realistic away from a laptop, especially with last week''s mobile session logs and issue-based agent assignment.
- [Copilot cloud agent’s validation tools are now 20% faster](https://github.blog/changelog/2026-04-10-copilot-cloud-agents-validation-tools-are-now-20-faster)
- [Dependabot alerts can now be assigned to AI coding agents for remediation](https://github.blog/changelog/2026-04-07-dependabot-alerts-are-now-assignable-to-ai-agents-for-remediation)
- [GitHub Mobile: Research and code with Copilot cloud agent anywhere](https://github.blog/changelog/2026-04-08-github-mobile-research-and-code-with-copilot-cloud-agent-anywhere)
## Other GitHub Copilot News
Copilot adoption and governance became more measurable for admins through several usage metrics API updates. This continues last week''s admin thread (org-wide custom instructions GA; per-user Copilot CLI activity in org reports) but shifts from "who used Copilot?" to "which workflows changed, and what happened?" Copilot CLI activity is now included in totals and feature breakdowns (`feature=copilot_cli`), which simplifies reporting but means dashboards that assumed totals were IDE-only may need updates to avoid double counting. The API also adds **merged PRs that were Copilot-reviewed** and **median time-to-merge for Copilot-reviewed PRs**, which moves measurement beyond "Copilot created PRs" to "Copilot reviewed PRs." For maturity tracking, user-level reports now split **active vs passive Copilot Code Review**, and enterprise/org reports add daily/weekly/monthly active user counts for the renamed **Copilot cloud agent** (with null-vs-0 behavior ETL needs to handle). Together with last week''s CLI activity reporting, it is easier to separate chat usage, terminal automation, and agent execution in rollups.
- [Copilot CLI activity now included in usage metrics totals and feature breakdowns](https://github.blog/changelog/2026-04-10-copilot-cli-activity-now-included-in-usage-metrics-totals-and-feature-breakdowns)
- [Copilot-reviewed pull request merge metrics now in the usage metrics API](https://github.blog/changelog/2026-04-08-copilot-reviewed-pull-request-merge-metrics-now-in-the-usage-metrics-api)
- [Copilot usage metrics now identify active and passive Copilot code review users](https://github.blog/changelog/2026-04-06-copilot-usage-metrics-now-identify-active-and-passive-copilot-code-review-users)
- [Copilot usage metrics now aggregate Copilot cloud agent active user counts](https://github.blog/changelog/2026-04-10-copilot-usage-metrics-now-aggregate-copilot-cloud-agent-active-user-counts)
Security teams also got a more direct workflow: **Ask Copilot** is now available inside Code Security''s risk assessment and secret risk assessment results, so admins can open Copilot from a finding to get explanation and remediation guidance without leaving the UI. This fits the "shorter remediation loop" pattern from last week''s Copilot Autofix positioning and this week''s agent-assigned Dependabot alerts. Copilot is being placed closer to the security queue, not only the coding surface.
- [Ask Copilot in security assessments now available](https://github.blog/changelog/2026-04-09-ask-copilot-in-security-assessments-now-available)
Modernization and long-lived codebases also came up this week. The Copilot Modernization assessment workflow is positioned as the repo''s "source of truth" for Assess -> Plan -> Execute migrations for .NET and Java to Azure. It stores reports in `.github/modernize/assessment/` and feeds IaC generation (Bicep/Terraform), containerization, and deployment manifests via the VS Code extension (GA) or the Modernize CLI (public preview) with multi-repo assessments. This builds on last week''s modernization content by turning plans into versioned repo artifacts that tools and agents can act on. Microsoft also announced **ASP.NET Core 2.3 end of support on April 7, 2027**, which is a cue to schedule upgrades (for example, toward .NET 10 LTS) and points to Copilot App Modernization as one assessment option.
- [Your Migration’s Source of Truth: The Modernization Assessment](https://devblogs.microsoft.com/dotnet/your-migrations-source-of-truth-the-modernization-assessment/)
- [ASP.NET Core 2.3 end of support announcement](https://devblogs.microsoft.com/dotnet/aspnet-core-2-3-end-of-support/)
Teams building repeatable agent workflows got some useful patterns. Spec-Kit''s extensions system shows how to extend spec-driven development with commands and automation, including the "Ralph Loop" that repeatedly runs a Copilot-based agent against `tasks.md`, produces reviewable commits, and tracks context in `progress.md`. It matches last week''s "configuration scales" and "repo hygiene" guidance by turning agent work into a predictable loop with reviewable artifacts. On the data/dev side, the Data Exposed episode showed how to make Copilot-generated T-SQL more consistent using **custom instructions**, **Plan Mode** (PRD -> schema plan), **Agent Mode** with the MSSQL extension''s Schema Designer against a real DB, and a **skills file** exposed as a slash command, plus checking what context was sent through the Copilot debug panel. That ties back to last week''s observability focus: once teams rely on instructions and skills, they need a way to verify what the model received.
- [Spec-Kit Extensions: Making spec-driven development your own](https://hiddedesmet.com/speckit-extensions)
- [Next-Level SQL in VS Code: GitHub Copilot Custom Instructions, Plan Mode & Skills | Data Exposed](https://www.youtube.com/watch?v=noEj1AhZUwE)
A couple of access and cost/ops notes may affect rollout planning. GitHub is enforcing updated Copilot limits with separate "service reliability" vs "model capacity" behaviors (often mitigated by switching models or using Auto mode), and it is retiring **Opus 4.6 Fast** from Copilot Pro+. This reinforces last week''s reminder that model availability can change, and deprecations and quota behavior can affect standardized model plans and agent throughput. GitHub also paused new Copilot Pro trials due to trial abuse, pointing evaluators to Copilot Free or paid subscriptions for now. For budgeting context, Microsoft''s "Budget Bites" finale discussed AI, cost constraints, and database choices (Azure SQL and Hyperscale), and framed Copilot/agents as a way to reduce repetitive operational work.
- [Enforcing new limits and retiring Opus 4.6 Fast from Copilot Pro+](https://github.blog/changelog/2026-04-10-enforcing-new-limits-and-retiring-opus-4-6-fast-from-copilot-pro)
- [Pausing new GitHub Copilot Pro trials](https://github.blog/changelog/2026-04-10-pausing-new-github-copilot-pro-trials)
- [AI, Budget, and the Future of Databases](https://www.youtube.com/watch?v=vc2kcT-75J8)',
    'This week''s Copilot story was less about one headline and more about Copilot being available in more places: stronger agent controls in VS Code and GitHub Mobile, deeper terminal workflows through Copilot CLI (including offline/BYOK), and more admin/reporting to track adoption and outcomes. It follows last week''s theme that as Copilot grows from chat/autocomplete into branch/PR agents, multi-agent CLI orchestration, and MCP-backed tooling, GitHub is closing gaps in control (permissions, firewall/runner placement), traceability (sessions/logs/telemetry), and administration (instructions and usage reporting).',
    1776063600, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-04-13', 'TechHub',
    'TechHub', '6770AC04FF5825320C1DD8C3C82E4774EB0D3278244D9CDCC093F434DD1EC63B', ',GitHub Copilot,VS Code,Copilot CLI,Copilot Cloud Agent,GitHub Mobile,Agent Permissions,MCP,BYOK,Offline Local Models,Ollama,Azure OpenAI,Dependabot,CodeQL,Secret Scanning,Usage Metrics API,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-04-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-04-06', 'roundups', 'Weekly GitHub Copilot Roundup: Agents Everywhere, Guardrails Tighten',
    'This week’s Copilot updates kept moving past the "chat + autocomplete" baseline toward agents that work across the web, IDE, CLI, and mobile, with more governance and observability as usage scales. Building on last week’s shift toward agent work inside PRs/Issues/Projects and better operability (logs, validations, admin controls, reporting), this week extends that direction in two ways: more entry points for agent work (branch-first, mobile/Slack) and tighter enterprise guardrails (runner and firewall controls, signed commits, org-wide instructions). Model availability is also changing quickly, so teams that pin models or enforce policies should plan regular housekeeping to avoid surprises.
<!--excerpt_end-->
## Copilot cloud agent on GitHub.com: branch-first workflows, planning, research, and tighter org controls
After last week’s PR-native improvements (comment `@copilot` on an existing PR, update the current PR by default, and resolve merge conflicts with logs and validations), Copilot cloud agent (formerly the Copilot coding agent) can now work directly on a branch without automatically opening a PR. Teams can iterate privately, review the evolving diff, and open a PR only when they are ready, while still requesting auto-created PRs when that is preferred. This pairs last week’s "iterate within the same PR thread" flow with a pre-PR staging mode.
The agent also added two changes aimed at reducing unexpected diffs. First, it can draft an implementation plan before coding so you can approve or adjust the approach up front. Second, "deep research" sessions let it answer broader repo-grounded questions, which can help with impact analysis and design trade-offs, especially when combined with last week’s expanded agent visibility in Issues/Projects.
As more teams adopt it, GitHub added org-level controls for where the cloud agent runs and what it can reach. Platform teams can set (and optionally lock) which Actions runner executes agent tasks (GitHub-hosted, Large, or self-hosted) without managing runner choice repo-by-repo via `copilot-setup-steps.yml`. Org admins can also centrally manage the agent firewall (on/off, recommended allowlist, custom allowlist, and whether repos can add entries), which affects workflows that fetch dependencies, call external services, or reach internal registries. Together, these continue last week’s "automatable rollout" direction at two early control points: network egress and compute placement.
Supply-chain and compliance workflows got easier as well: the cloud agent now signs every commit it creates. Agent commits show as "Verified" and work in repos that enforce "Require signed commits" via branch protection/rulesets. Combined with last week’s traceability (session logs, issue sidebar status, PR-thread invocation), commit provenance becomes part of the default setup as agents become regular commit authors.
- [Research, plan, and code with Copilot cloud agent](https://github.blog/changelog/2026-04-01-research-plan-and-code-with-copilot-cloud-agent)
- [Put GitHub Copilot cloud agent to work: research, plan, and code on github.com](https://www.youtube.com/watch?v=pn5x1CamKVY)
- [Organization runner controls for Copilot cloud agent](https://github.blog/changelog/2026-04-03-organization-runner-controls-for-copilot-cloud-agent)
- [Organization firewall settings for Copilot cloud agent](https://github.blog/changelog/2026-04-03-organization-firewall-settings-for-copilot-cloud-agent)
- [Copilot cloud agent signs its commits](https://github.blog/changelog/2026-04-03-copilot-cloud-agent-signs-its-commits)
## Copilot in Visual Studio and VS Code: custom agents, agent sessions, and more IDE-native workflows
In Visual Studio, Copilot’s March update focused on making agent behavior portable and repeatable across repos, which mirrors last week’s repo-visible instructions/skills and this week’s GitHub.com plan/research work. Teams can define custom agents as `*.agent.md` files under `*.github/agents/*` and expose them in Visual Studio’s agent picker, with settings such as workspace awareness, tool access, preferred model, and MCP connections. Visual Studio also added "agent skills" as reusable instruction sets stored in the repo (shared defaults) or the user profile (personal defaults), which are auto-discovered during workflows. This fits the "configuration scales" theme and helps keep IDE and GitHub.com agents aligned per repo.
Agent mode also gained a navigation tool that relies on language services rather than text search: `find_symbol` locates and reasons about symbols across the project. It supports C++, C#, Razor, TypeScript, and LSP-backed languages, which helps refactors avoid missed call sites or scope/type mistakes, especially as last week’s PR/issue workflows encourage larger agent changes.
The update also brought Copilot into IDE performance and security loops. "Profile with Copilot" in Test Explorer runs a specific test via a Profiling Agent and analyzes CPU/instrumentation data (called out for .NET tests). During debugging, PerfTips now use live profiling, and a Profiler Agent captures elapsed time/CPU/memory signals so Copilot can suggest optimizations when you hit a slowdown. For dependency hygiene, Visual Studio can remediate vulnerable NuGet packages from Solution Explorer via "Fix with GitHub Copilot," which turns detection into an in-IDE update loop. This continues last week’s "give the agent real signals" thread, but shifts it from external telemetry to built-in diagnostics and security findings.
In VS Code, Copilot kept becoming more "session-aware," matching last week’s traceable sessions/logs (issue sidebar status, PR-linked logs, CLI `/context` and `/resume`). VS Code 1.114 highlights centered on Copilot Chat usability, including richer media in the chat carousel, copy as Markdown, improved troubleshooting, and updates to the `#codebase` grounding command. VS Code 1.115 Insiders added more session state (restore agent edits with diffs and undo/redo), exposed entitlements/usage inside Sessions, and expanded session context to include the integrated browser (tracking tabs the agent used). Terminal automation also improved: background terminals can notify the agent on completion with exit codes/output, input prompts are surfaced to avoid silent stalls, and `send_to_terminal` supports confirmed command dispatch to background terminals. Remote workflows improved with an SSH path that installs the VS Code CLI and starts agent host mode on remote machines. Overall, last week made agents more present in GitHub collaboration surfaces, and this week makes editor/terminal work easier to resume, audit, and drive with more explicit control.
- [GitHub Copilot in Visual Studio — March update](https://github.blog/changelog/2026-04-02-github-copilot-in-visual-studio-march-update)
- [Visual Studio March Update – Build Your Own Custom Agents](https://devblogs.microsoft.com/visualstudio/visual-studio-march-update-build-your-own-custom-agents/)
- [What''s hot in VS Code v1.114? 🔥](https://www.youtube.com/shorts/LrQBmrRV_Ro)
- [Visual Studio Code 1.115 (Insiders): agent sessions, Copilot entitlements, SSH agent host mode](https://code.visualstudio.com/updates/v1_115)
## Copilot CLI and Copilot SDK: multi-agent orchestration and a reusable agent runtime
Copilot CLI added multi-agent execution via the `/fleet` command. `/fleet` breaks a goal into work items, runs sub-agents in parallel, then validates and synthesizes results into your working tree. In real repos, the details matter: each sub-agent has its own context window, they share the filesystem, and they do not coordinate directly, so prompts should specify file ownership to avoid collisions (or stage outputs in temp paths and merge). It builds on last week’s CLI focus on session controls (`/model`, `/context`, `/resume`) by adding an orchestration pattern that does not depend on one long context thread.
GitHub also released the Copilot SDK in public preview (the same agent runtime used by Copilot cloud agent and Copilot CLI), so teams can embed agent interactions in internal apps without building orchestration from scratch. This extends last week’s Copilot SDK "IssueCrush" walkthrough into a shared runtime rather than a one-off pattern. The SDK includes tool invocation, stateful multi-turn sessions, streaming, built-in file ops, and a permissions/approval framework (including read-only tools that can bypass approvals). It supports blob attachments (images/screenshots without temp files) and OpenTelemetry tracing with W3C Trace Context propagation, extending last week’s "traceable logs" theme into standard telemetry pipelines. BYOK (OpenAI, Azure AI Foundry, Anthropic) keeps model/provider flexible while standardizing the runtime. The preview ships for Node/TypeScript, Python, Go, .NET, and Java.
GitHub also published an SDK demo that adds planning flows to a Node.js app (meal plans and weekend schedules), with an emphasis on regeneration as constraints change. The takeaway is how to wire an app to SDK sessions and support iterative refinement without restarting, which lines up with this week’s "plan first" in Copilot cloud agent and last week’s repeatable session lifecycles.
- [Run multiple agents at once with /fleet in Copilot CLI](https://github.blog/ai-and-ml/github-copilot/run-multiple-agents-at-once-with-fleet-in-copilot-cli/)
- [Copilot SDK in public preview](https://github.blog/changelog/2026-04-02-copilot-sdk-in-public-preview)
- [Build a Planning App with the GitHub Copilot SDK | demo](https://www.youtube.com/watch?v=ylPjZVaLJYI)
- [Not sure where to start with the GitHub Copilot SDK?](https://www.youtube.com/shorts/R5ouLL_UA3o)
## MCP + Azure workflows: Copilot-assisted scaffolding, deployments, and repo-grounded research pipelines
Copilot’s "agent + tools" story showed up across Azure workflows, extending last week’s MCP thread: connect Copilot to tools, then operationalize access with versioned config, enforcement, and audit. Azure Developer CLI (azd) shipped a preview "Set up with GitHub Copilot" path in `azd init`, using a Copilot agent to scaffold a project and align it to azd conventions (templates, `azure.yaml`, service detection), with guardrails like dirty-directory checks and consent before enabling MCP server tooling. The same release added AI-assisted troubleshooting for failed commands (explain/guidance/troubleshoot/skip), with an option to apply a fix and retry from the terminal. azd’s extension SDK also added MCP utilities and a new `CopilotService` gRPC service so extensions can use agent capabilities (sessions/messages/usage metrics), which matches last week’s "turn playbooks into tool-driven flows" pattern.
An "Azure Skills Plugin" cookbook also published many copy/paste prompts for Copilot Chat (Agent mode) and Copilot CLI to automate Azure tasks end-to-end: repo analysis and infra generation (`azure-prepare`), validation (`azure-validate`), deployment via azd (`azure-deploy`), plus diagnostics via KQL, RBAC/compliance checks, Entra app registrations, and AI service setup (Azure AI Search, API Management as an AI gateway, Foundry tasks). The theme is chaining skills into prepare -> validate -> deploy pipelines while keeping explicit per-skill calls for tighter control, similar to last week’s "standardized prompt files in CI," expressed here as skill chains.
Project Nighthawk provided a concrete pattern for repo-grounded research in VS Code: a multi-agent pipeline that searches locally cloned repos (kept current via `git pull`), consults Microsoft Learn via an MCP server, synthesizes a cited Markdown report, and runs a fact-checker that validates claims and flags unverified statements. For deep technical investigations, this extends last week’s "observability + verification" mindset into reviewable research artifacts, especially now that this week adds "deep research" sessions to Copilot cloud agent.
- [Azure Developer CLI (azd) – March 2026: Run and Debug AI Agents Locally, GitHub Copilot Integration, & Container App Jobs](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-march-2026/)
- [Building with Azure Skills: a prompt cookbook for the Azure plugin (MCP server)](https://devblogs.microsoft.com/all-things-azure/building-with-azure-skills/)
- [Project Nighthawk: A Research Agent Built for Field Engineering](https://devblogs.microsoft.com/all-things-azure/project-nighthawk-a-research-agent-built-for-field-engineering/)
## Other GitHub Copilot News
Org-level governance and reporting kept closing admin gaps, building on last week’s repo allowlisting and usage metrics flagging `used_copilot_coding_agent`. Organization custom instructions are now GA for Copilot Business and Enterprise, so admins can set defaults across Copilot Chat on github.com, Copilot code review, and Copilot cloud agent. Usage reporting also expanded: org reports now include per-user Copilot CLI activity (session/request counts, tokens, average tokens per request, last known CLI version) via the Copilot usage metrics REST API (`apiVersion=2026-03-10`). Together with last week’s agent-user metric, this helps distinguish "used Copilot" from "ran agents" from "drove terminal automation," as entry points expand beyond IDEs.
- [Copilot organization custom instructions are generally available](https://github.blog/changelog/2026-04-02-copilot-organization-custom-instructions-are-generally-available)
- [Copilot usage metrics now includes per-user GitHub Copilot CLI activity in organization reports](https://github.blog/changelog/2026-04-02-copilot-usage-metrics-now-includes-per-user-github-copilot-cli-activity-in-organization-reports)
Model availability shifted again, with two deprecations that matter for teams pinning models or enforcing policies, which continues last week’s reminder that model lifecycles change quickly (for example, Gemini 3 Pro exiting for Gemini 3.1 Pro). GPT-5.1 Codex variants (GPT-5.1-Codex, -Mini, -Max) were deprecated April 1, 2026, with GPT-5.3-Codex as the replacement. Claude Sonnet 4 is scheduled for deprecation on 2026-05-01 with Claude Sonnet 4.6 suggested. On the adoption side, GPT-5.4 mini is now GA for Copilot Student via auto model selection in Copilot Chat across major IDEs.
- [GPT-5.1 Codex, GPT-5.1-Codex-Max, and GPT-5.1-Codex-Mini deprecated](https://github.blog/changelog/2026-04-03-gpt-5-1-codex-gpt-5-1-codex-max-and-gpt-5-1-codex-mini-deprecated)
- [Upcoming deprecation of Claude Sonnet 4 in GitHub Copilot](https://github.blog/changelog/2026-03-31-upcoming-deprecation-of-claude-sonnet-4-in-github-copilot)
- [GPT-5.4 mini is now available in Copilot Student auto model selection](https://github.blog/changelog/2026-04-01-gpt-5-4-mini-is-now-available-in-copilot-student-auto-model-selection)
Copilot’s "where work starts" expanded beyond IDEs into Slack and mobile, extending last week’s "agents show up where teams collaborate" shift into chat and on-the-go triage. The GitHub app for Slack can now create Issues from natural-language prompts (including sub-issues), supports thread-based refinement of metadata, and offers an in-Slack flex pane to view the issue. GitHub Mobile also made sessions and delegation easier: a dedicated Copilot tab (notably on Android), native session logs, stop sessions and create PRs from completed sessions, plus a faster "Assign an Agent" flow from an issue (custom instructions and optional repo selection). The pattern across two weeks is consistent: as agents act in more places, GitHub adds workflow hygiene (logs, status visibility, and mobile-native controls) so activity stays reviewable.
- [Create issues from Slack with Copilot](https://github.blog/changelog/2026-03-30-create-issues-from-slack-with-copilot)
- [GitHub Mobile: Stay in flow with a refreshed Copilot tab and native session logs](https://github.blog/changelog/2026-04-01-github-mobile-stay-in-flow-with-a-refreshed-copilot-tab-and-native-session-logs)
- [GitHub Mobile: Faster, more flexible agent assignment from issues](https://github.blog/changelog/2026-04-01-github-mobile-faster-more-flexible-agent-assignment-from-issues)
Several items focused on practical guidance for shaping Copilot behavior, from agent-first repo practices to prompt patterns and repo-level instruction files. This mirrors last week’s dotnet/runtime lessons ("prepare the repo, document commands/conventions, pair with humans") and this week’s move toward org-wide defaults (custom instructions GA). GitHub’s Applied Science team described an agent-driven loop around Copilot CLI (planning, autopilot-style execution, iterative Copilot Code Review, then human review), with an emphasis on repo navigability (structure, naming, docs, tests, dead-code removal) and CI guardrails (typing, linters, layered tests) so agents can self-correct. Other posts covered prompting Copilot to ask clarifying questions before implementing, and adding a repo "tone of voice"/instructions file to make Copilot review more strict.
- [Agent-driven development in Copilot Applied Science](https://github.blog/ai-and-ml/github-copilot/agent-driven-development-in-copilot-applied-science/)
- [Let GitHub Copilot Ask First](https://www.cooknwithcopilot.com/blog/let-github-copilot-ask-first.html)
- [GitHub Copilot Is Too Nice. Fix It With a Tone of Voice File.](https://dev.to/playfulprogramming/github-copilot-is-too-nice-fix-it-with-a-tone-of-voice-file-39ij)
Copilot also showed up in modernization and security enablement content. Videos covered using Copilot to assess legacy .NET apps, draft modernization plans, and convert them into task lists (including Copilot Modernization in VS Code). A GitHub Security intro positioned Copilot Autofix alongside Dependabot, secret scanning, and code scanning to shorten the initial remediation loop. This follows last week’s thread of turning assistant output into workflow steps, but framed around modernization and remediation rather than feature work.
- [How can AI help me modernize my app?](https://www.youtube.com/watch?v=DgIweLo3-Oo)
- [Using GitHub Copilot in VS Code to plan a .NET Framework to modern .NET migration](https://www.youtube.com/shorts/ppHWnhw2BUM)
- [VS Code Live: Modernizing legacy .NET apps with GitHub Copilot Modernization in VS Code](https://www.youtube.com/watch?v=JQ3x362nc6c)
- [Getting started with GitHub security (secret scanning, Dependabot, code scanning, Copilot Autofix)](https://www.youtube.com/watch?v=zhxXaFzzJYA)
Infrastructure-as-code teams got more "Copilot + guardrails" examples in VS Code, continuing last week’s "agentic platform engineering" message: version playbooks, use deterministic checks, keep humans in approvals. One walkthrough showed a repo-scoped VS Code custom agent (1.99+) for Terraform security scanning with structured findings (severity + stable IDs, file/line, remediation, mappings to CIS/ASB v3/NIST), paired with TFLint and SARIF output. Another tutorial described a VS Code extension that scaffolds Terraform from centrally managed module templates and uses Copilot to draft configuration, while keeping deterministic validation/compliance enforcement in extension code (explicitly avoiding MCP) so AI output stays draftable but constrained.
- [VS Code Custom Agents: AI-Powered Terraform Security Scanning in the IDE](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/vs-code-custom-agents-ai-powered-terraform-security-scanning-in/ba-p/4507903)
- [Building a VS Code extension to scaffold Terraform with guardrails and GitHub Copilot](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/vs-code-extension/ba-p/4500803)
A governance-focused analysis flagged an upcoming Copilot data policy change, following last week’s official policy update and guardrails guidance. Copilot Free/Pro/Pro+ will allow training on "interaction data" unless users opt out, while Copilot Business and Enterprise are not affected. It reviews enforcement options (central licensing, network controls, IDE policy packs, managed identities/data residency) for orgs that want to avoid relying on opt-out behavior and keep usage on commercial tiers.
- [GitHub Copilot will start training on your interactions](https://jessehouwing.net/github-copilot-will-start-training-on-your-interactions/)
A few community and onboarding items fit the two-week trend of agents as normal workflow participants. These include quick-start content for installing Copilot CLI, a beginner workflow using CLI "plan" and "delegate" to hand work to a background cloud agent then review PRs from the terminal, and an event announcement for Copilot Dev Days.
- [How to install GitHub Copilot CLI in seconds](https://www.youtube.com/shorts/9-zk6RNIS6w)
- [Copilot CLI for beginners: Plan, delegate, and review](https://www.youtube.com/watch?v=v8dr7QcIiLU)
- [GitHub Copilot Dev Days are Here! We''re in Chennai on 4/11](https://www.youtube.com/shorts/Pl7wYzQzQ-w)',
    'This week’s Copilot updates kept moving past the "chat + autocomplete" baseline toward agents that work across the web, IDE, CLI, and mobile, with more governance and observability as usage scales. Building on last week’s shift toward agent work inside PRs/Issues/Projects and better operability (logs, validations, admin controls, reporting), this week extends that direction in two ways: more entry points for agent work (branch-first, mobile/Slack) and tighter enterprise guardrails (runner and firewall controls, signed commits, org-wide instructions). Model availability is also changing quickly, so teams that pin models or enforce policies should plan regular housekeeping to avoid surprises.',
    1775458800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-04-06', 'TechHub',
    'TechHub', 'CA1CB18F3ED2262C459AF02697767C8832F215AF671C0ADEA9559839AC6E376E', ',GitHub Copilot,Copilot Cloud Agent,GitHub Actions Runners,Agent Firewall,Signed Commits,Organization Custom Instructions,Copilot Usage Metrics API,VS,VS Code,Copilot CLI,Copilot SDK,Multi Agent Orchestration,Model Deprecations,OpenTelemetry,MCP,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-03-30
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-03-30', 'roundups', 'Weekly GitHub Copilot Roundup: Agents in PRs, Models, Controls',
    'This week''s Copilot updates continued the shift from "help me write code" to "help me run the workflow," with more agent work inside pull requests, issues, and project boards. Building on last week''s focus on "agents you can operate at scale" (faster starts, configurable validations, traceable logs, and better reporting), this week''s changes bring that thread into core GitHub surfaces teams already use: PR comments, issue sidebars, and Projects views. GitHub also expanded model choice (while retiring older models), and Microsoft integrations (SSMS, Azure App Service tooling, Fabric in VS Code) kept positioning Copilot as an embedded assistant where developers already work.
<!--excerpt_end-->
## Copilot coding agent in pull requests, issues, and project tracking
After last week''s improvements to execution quality (repo-tunable validations and commit-to-session traceability), Copilot''s coding agent now fits PR workflows more naturally. You can invoke it on any existing pull request by commenting **@copilot** with the change you want, and it now updates the current PR by default instead of opening a stacked PR (unless you ask). That keeps common review iterations (fix failing Actions runs, address feedback, add tests) inside the same PR thread and history. It also builds on last week''s "Agent-Logs-Url trailer" and clearer session logs: when more work happens in PRs, reviewers need direct links to what ran and why changes were made.
Copilot can also handle a frequent review task: **merge conflict resolution**. You trigger it via comments (for example, "merge in main and resolve the conflicts"), and it works in an isolated cloud dev environment, validates builds, runs tests, and pushes updates back to the PR. Together, "make changes on this PR" plus "resolve conflicts on this PR" shortens the loop for teams that rebase/merge often or maintain long-running branches, especially with last week''s validation tuning so "prove it passes" matches the repo''s real feedback loop.
GitHub also improved planning visibility so agent work is easier to track. When an agent is assigned to an issue, an **agent session** now appears in the issue sidebar with status (queued/working/waiting for review/completed) plus a link to logs. That signal can also show in GitHub Projects table/board views via "Show agent sessions," letting teams scan what''s in progress vs. waiting on humans. This continues last week''s observability push (more actionable logs, live streaming in Raycast): as agents run longer and touch more steps, "where is it and where are the logs?" becomes part of normal workflow hygiene.
- [Ask @copilot to make changes to any pull request](https://github.blog/changelog/2026-03-24-ask-copilot-to-make-changes-to-any-pull-request)
- [Ask @copilot to resolve merge conflicts on pull requests](https://github.blog/changelog/2026-03-26-ask-copilot-to-resolve-merge-conflicts-on-pull-requests)
- [Agent activity in GitHub Issues and Projects](https://github.blog/changelog/2026-03-26-agent-activity-in-github-issues-and-projects)
## Enterprise admin controls and adoption reporting for the coding agent
As more teams enable PR- and issue-driven agents (and now see agent status in Issues/Projects), GitHub is adding admin controls for where agents can operate. In public preview, org owners can manage coding agent repository access using new REST endpoints (apiVersion **2026-03-10**), setting it to: enabled for no repos, enabled for all repos, or enabled only for an allowlist (with add/remove endpoints). This matches last week''s operational direction (validation "speed vs safety" controls and longer-lived defaults): the coding agent is becoming something you roll out repo-by-repo with automatable, auditable settings.
Usage reporting is also more specific, extending last week''s push to close reporting gaps (org-level CLI activity and resolving "Auto" into the actual model). Copilot usage metrics now include **used_copilot_coding_agent** at the user level, so enterprises can separate "used Copilot in an IDE / agent mode" from "triggered the coding agent" (assigning Copilot to an issue or tagging @copilot in a PR comment). For internal dashboards and adoption analysis, this reduces ambiguity and aligns with this week''s PR/issue-native invocation patterns.
- [Manage Copilot coding agent repository access via the API](https://github.blog/changelog/2026-03-24-manage-copilot-coding-agent-repository-access-via-the-api)
- [Copilot usage metrics now identify active Copilot coding agent users](https://github.blog/changelog/2026-03-25-copilot-usage-metrics-now-identify-active-copilot-coding-agent-users)
## Model lifecycle and selection: Gemini 3.1 Pro arrives, Gemini 3 Pro exits
Copilot''s model picker keeps expanding across clients, following last week''s theme of model choice as an admin-governed surface (rollouts, LTS options, clearer attribution). **Gemini 3.1 Pro** is now in public preview as a selectable chat model across github.com, GitHub Mobile, VS Code, Visual Studio, JetBrains IDEs, Xcode, and Eclipse. For Copilot Business and Enterprise, admins must enable the Gemini 3.1 Pro policy before users see it, which helps teams standardize (or restrict) model choice centrally.
At the same time, GitHub deprecated **Gemini 3 Pro** across Copilot experiences (Chat, inline edits, Ask/Agent/Edit modes, and completions) and points users to Gemini 3.1 Pro. This complements last week''s "stable defaults" story: even with LTS models reducing churn, the wider lineup still changes, so teams should review internal docs, defaults, and enterprise policies, especially where policy enablement controls whether the replacement is selectable.
- [Gemini 3.1 Pro is now available in JetBrains IDEs, Xcode, and Eclipse](https://github.blog/changelog/2026-03-23-gemini-3-1-pro-is-now-available-in-jetbrains-ides-xcode-and-eclipse)
- [Gemini 3 Pro deprecated](https://github.blog/changelog/2026-03-26-gemini-3-pro-deprecated)
## Copilot SDK and Copilot CLI: building blocks for agentic apps and terminal workflows
On the "build with Copilot" side, this week''s Copilot SDK coverage moved from an intro to a full end-to-end app example. GitHub published a walkthrough for an AI-backed issue triage app ("IssueCrush"). The key architecture point is that the Copilot SDK depends on Node and manages a local **Copilot CLI** process via JSON-RPC, so the SDK runs server-side (Node/Express) while a React Native client uses GitHub OAuth + REST APIs to fetch issues. The tutorial emphasizes production-oriented patterns: reuse a long-lived SDK client, manage session lifecycle (`start()`, `createSession()`, `sendAndWait()` with timeouts, `disconnect()`, `stop()` in `finally`), and degrade gracefully when Copilot auth/subscription is missing (HTTP 403 with `requiresCopilot`, plus a metadata-based fallback summary). This echoes last week''s "session forensics" mindset: bound, resume, and reason about agent work instead of treating it as chat output.
For terminal workflows, GitHub also highlighted Copilot CLI slash commands: **/model** to switch models mid-session (useful alongside last week''s model attribution and this week''s Gemini changes), **/context** to view token usage and avoid bloated sessions, and **/resume** to continue a previous session. These fit last week''s push for more traceable, inspectable sessions.
- [Building AI-powered GitHub issue triage with the Copilot SDK](https://github.blog/ai-and-ml/github-copilot/building-ai-powered-github-issue-triage-with-the-copilot-sdk/)
- [A beginner''s guide to Copilot CLI slash commands](https://www.youtube.com/watch?v=-Yavis20B4Q)
## MCP-powered “agentic platform engineering”: Copilot beyond coding into CI/CD enforcement and AKS ops
This week''s "agentic platform engineering" pattern continues last week''s MCP/repo-native workflow progression: moving from "connect Copilot to tools" to "operationalize tool access with repeatable config, enforcement, and audit." The approach starts by capturing operational knowledge in repos so Copilot can answer "how do I...?" grounded in conventions, IaC modules, and real environments (including reverse-engineering portal-built Azure into Terraform/Bicep). It then moves into enforcement by wiring Copilot-driven checks into GitHub Actions using standardized prompt files (`.prompt.md` under `.github/prompts/`) so teams can update rules without rewriting pipeline logic. This matches last week''s "repo-visible instructions/skills" and "governed workflow participant" themes.
The final stage connects AKS health signals to agent triage. Argo CD detects unhealthy deployments, Argo CD Notifications posts a payload via `repository_dispatch`, an Actions workflow creates/dedupes structured issues, and a second workflow triggers a custom Copilot agent ("Cluster Doctor") when a `cluster-doctor` label is applied. "Cluster Doctor" lives in `.github/agents/cluster-doctor.agent.md` with a collect->verify->diagnose->triage->remediate flow and safety constraints (verify cluster identity; avoid destructive actions without authorization), and it proposes fixes as PRs with humans approving. That lines up with this week''s PR-native agent updates: PR-based remediation is more useful when agents can be invoked and iterated directly in the PR thread.
A key enabler is MCP: the repo includes `.copilot/mcp-config.json` to connect Copilot to a GitHub MCP server (issues/PRs) and an in-cluster **AKS MCP server** for telemetry and kubectl-based diagnostics, authenticated via Azure Workload Identity Federation. The emphasis is less "autonomous agent" and more "version operational playbooks as prompts/agents, enforce in CI, then turn GitOps signals into human-reviewed remediation PRs," extending last week''s MCP examples (Azure DevOps remote MCP, GitHub secret scanning via MCP, Fabric MCP) into end-to-end platform ops.
- [Agentic Platform Engineering with GitHub Copilot](https://devblogs.microsoft.com/all-things-azure/agentic-platform-engineering-with-github-copilot/)
## Copilot embedded in Microsoft tooling: SSMS GA, Azure App Service profiling fixes, and Fabric + MCP in VS Code
Copilot continued to show up as a first-class feature in Microsoft developer tools, reinforcing last week''s theme that customization and tool wiring are becoming portable configuration rather than per-app prompt habits. **SSMS 22.4.1** moved "GitHub Copilot in SSMS" from preview to **GA**, and added support for user-scoped custom instructions at `%USERPROFILE%\\copilot-instructions.md` so Copilot can stay consistent even for ad-hoc scripts outside a repo/solution. This complements last week''s repo-visible instructions/skills: when there is no repo context (or you do not want one), user-level instructions can act as a set of defaults you control. The update also added more export formats (Excel, JSON, XML, Markdown), "Save with Encoding...", broader "group objects by schema" support beyond Fabric SQL databases, plus a new "SQL Projects (Preview)" entry and a roadmap note toward future "Agent mode."
In VS Code, the Azure App Service extension added a "Code Optimizations" workflow that converts **Application Insights Profiler** findings into "Fix with Copilot" prompts. Items (CPU/memory, impact %, stack traces) appear under "Code Optimizations"; the extension maps stack traces to local methods, opens the code, and seeds Copilot Chat with the recommendation, stack trace, and method source to drive concrete refactors. It targets .NET web apps on App Service, with Windows/Linux differences in profiling enablement. Conceptually it matches last week''s "give the agent real signals and context" thread, but here the signals come from production profiling rather than repo scanning.
On the data side, Microsoft Fabric updated its VS Code extension for workspace artifact management (browse folders, open item definitions; editing behind an "allow editing" setting), and introduced a pre-release **Fabric MCP server** extension for Copilot Chat. With MCP tools enabled, Copilot can interpret item definitions, work with Fabric REST APIs/docs, and run tenant/workspace operations (CRUD items, OneLake file operations, notebook creation/run) inside the editor. This continues last week''s Fabric MCP storyline (local MCP GA/open source and remote MCP in preview): "Copilot acts through governed tools," now packaged for day-to-day editor workflows.
- [''SSMS 22.4.1: GitHub Copilot in SSMS GA, custom instructions, and new export formats''](https://blog.fabric.microsoft.com/en-US/blog/sql-server-management-studio-ssms-22-4-1-and-github-copilot-in-ssms-generally-available/)
- [Code Optimizations for Azure App Service Now Available in VS Code](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/code-optimizations-for-azure-app-service-now-available-in-vs/ba-p/4504252)
- [Explore, edit, and organize Item definitions and Fabric MCP server in VS Code](https://blog.fabric.microsoft.com/en-US/blog/explore-edit-and-organize-item-definitions-and-fabric-mcp-server-in-vs-code/)
## Other GitHub Copilot News
Long-running usage data from a large OSS repo added practical "what works at scale?" guidance for coding agents. After last week''s focus on operability (validation tuning, traceability, reporting), the .NET team''s ten-month review of Copilot Coding Agent in **dotnet/runtime** provides concrete maintenance outcomes: 878 agent-authored PRs (14% of PRs) with ~68% merged rate for decided PRs. Results improved when the repo was prepared (documented build/test commands, conventions in `.github/copilot-instructions.md`, agent access to needed feeds). They also showed "pairing" matters: agent PRs with human commits merged far more often than fully hands-off ones, and strict expectations (like requiring benchmark evidence for performance PRs) still applied. This aligns with this week''s PR-native invocation: pairing is easier when the agent works in the same PR thread reviewers already use, not in a separate stacked PR.
- [Ten Months with Copilot Coding Agent in dotnet/runtime](https://devblogs.microsoft.com/dotnet/ten-months-with-cca-in-dotnet-runtime/)
Work tracking integrations kept improving the "agent in the workflow" story. In GitHub Copilot for Jira public preview, setup guidance and errors improved, users can select a supported model directly from Jira comments, and traceability tightened with Jira keys in branch names and PR titles plus links back to tickets. For Confluence-heavy teams, it can pull Confluence context via MCP by configuring an Atlassian MCP server with a Confluence PAT, which is useful but should be evaluated within an org''s permissions and data-access model. This parallels GitHub''s own shift: as MCP becomes the common way to bring systems into the agent loop, integrations increasingly look like repeatable MCP configuration plus clear policy boundaries.
- [GitHub Copilot for Jira — Public preview enhancements](https://github.blog/changelog/2026-03-25-github-copilot-for-jira-public-preview-enhancements)
Policy and governance stayed central, continuing last week''s "defaults, attribution, reporting" direction with a privacy angle. GitHub announced a Copilot interaction data policy change effective **April 24, 2026**: Copilot Free/Pro/Pro+ will use interaction data for model training by default unless users opt out, while Copilot Business/Enterprise are unaffected. Developers using personal plans on proprietary code should review settings at `github.com/settings/copilot`, and teams may want to document which account types are approved for which repos. Related guardrails guidance connected repo controls (like `.github/copilot-instructions.md` and duplicate detection) with CI/CD enforcement and Azure-side governance (Content Safety, Azure AI Foundry, DLP for Copilot Studio, Entra/RBAC, audit/monitoring). Together with last week''s model governance/traceability work, the message is consistent: if agents act in real workflows, teams need controls over both capabilities and data flows.
- [Updates to GitHub Copilot interaction data usage policy](https://github.blog/news-insights/company-news/updates-to-github-copilot-interaction-data-usage-policy/)
- [''Guardrails for Generative AI: Securing Developer Workflows''](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/guardrails-for-generative-ai-securing-developer-workflows/ba-p/4505801)
Teams trying multi-model setups got a practical resilience pattern that echoes last week''s multi-model review and "skills as shared assets" themes. Run Claude Code and Copilot CLI side-by-side, share as much repo configuration as possible (instructions, skills, MCP servers), and keep only tool-specific glue separate. The guidance includes concrete layouts (for example, `CLAUDE.md` as a single instructions source, syncing agent defs into `.github/agents/*.agent.md`, registering MCP servers in each tool''s config) so tool switching during latency/errors does not require rebuilding your setup. It matches last week''s lesson: reliability often comes from explicit, versioned configuration, not UI state.
- [Using Claude Code and GitHub Copilot CLI together for resilient agentic coding](https://devblogs.microsoft.com/all-things-azure/your-entire-engineering-floor-just-stopped-coding/)
A DevX Summit talk republished this month used Copilot CLI to frame how agent-heavy development shifts daily work toward intent specification, decomposition, and verification. The practical skill is less prompting and more repeatable review/validation habits that keep humans accountable for what ships. This fits the last two weeks'' arc: more agent autonomy and surfaces, paired with more traceability, governance, and "prove it" hooks.
- [The Transformation of the Developer Role with AI Agents](https://www.youtube.com/watch?v=0HI3OIi-YJY)',
    'This week''s Copilot updates continued the shift from "help me write code" to "help me run the workflow," with more agent work inside pull requests, issues, and project boards. Building on last week''s focus on "agents you can operate at scale" (faster starts, configurable validations, traceable logs, and better reporting), this week''s changes bring that thread into core GitHub surfaces teams already use: PR comments, issue sidebars, and Projects views. GitHub also expanded model choice (while retiring older models), and Microsoft integrations (SSMS, Azure App Service tooling, Fabric in VS Code) kept positioning Copilot as an embedded assistant where developers already work.',
    1774854000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-03-30', 'TechHub',
    'TechHub', 'DBBEE45C0CDBB89D52EE999EFB2062A3AB0EAAB73D2931454F58111172D92598', ',GitHub Copilot,Copilot Coding Agent,Pull Requests,GitHub Issues,GitHub Projects,GitHub Actions,REST API,Usage Metrics,Gemini 3.1 Pro,Model Governance,Copilot SDK,Copilot CLI,MCP,AKS,SQL Server Management Studio,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-03-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-03-23', 'roundups', 'Weekly GitHub Copilot Roundup: Models, Agents, and MCP Ops',
    'This week''s Copilot story is less about one headline feature and more about Copilot settling into three practical layers teams run every day: (1) clearer model choice and governance, (2) agent workflows with the observability and safety controls teams expect, and (3) broader MCP tool access so Copilot can act with real platform context (Azure DevOps, GitHub scanners, Azure resources, Fabric) instead of relying on chat history guesses. Building on last week''s themes (auto model selection across IDEs, repo-visible instruction files and hooks, and enterprise observability), this week adds more of the operational layer needed for scale: stable model windows, adjustable validations, and more traceable agent execution.
<!--excerpt_end-->
## Models, defaults, and enterprise governance
Copilot''s model lineup changed in two ways that matter for operations, and both extend last week''s model-management thread (JetBrains Auto Model Selection GA, model attribution, and plan/policy routing).
First, OpenAI''s **GPT-5.4 mini** is rolling out as generally available as a faster, agentic coding option aimed at repo exploration and "grep-style" tool workflows. It appears in the model picker across VS Code (chat/ask/edit/agent), Visual Studio, JetBrains, Xcode, Eclipse, github.com, GitHub Mobile, and GitHub CLI; availability depends on paid plans and Business/Enterprise admin policy. GitHub also notes a **0.33x premium request multiplier** (subject to change), which can help keep exploration costs lower. That matters more now that auto-routed requests can carry pricing modifiers and teams increasingly treat model choice as a policy and cost-control surface, not just personal preference.
Second, GitHub added an enterprise stability option: **long-term support (LTS) for GPT-5.3-Codex** on Copilot Business and Enterprise. LTS models remain available for **12 months**, and GPT-5.3-Codex is the first, available through **2027-02-04**. GPT-5.3-Codex will also become the **default base model** (replacing GPT-4.1) for orgs that have not explicitly approved or selected alternatives. GitHub notes automatic enablement within 60 days and calls out a **base-model date of 2026-05-17**, so even teams that never touch the model picker should plan a validation window if they care about style, security patterns, or dependency choices. Together with last week''s push to show which model responded, this is about fewer surprises: clearer defaults, longer-lived enterprise options, and better attribution for governance and audit.
- [GPT-5.4 mini is now generally available for GitHub Copilot](https://github.blog/changelog/2026-03-17-gpt-5-4-mini-is-now-generally-available-for-github-copilot)
- [GPT-5.3-Codex long-term support in GitHub Copilot](https://github.blog/changelog/2026-03-18-gpt-5-3-codex-long-term-support-in-github-copilot)
## Copilot coding agent: faster starts, smarter search, tighter quality gates, and better traceability
Several updates landed around the Copilot **coding agent** (the hosted/background agent assigned issues, run from Agents, or driven via `@copilot` on PRs). Last week''s focus on autonomy trade-offs (auto-approval switches, IDE autopilot modes) and "run agents safely in real repos" continues here, with speed improvements plus more reviewable, repo-tunable controls and audit hooks.
On performance, GitHub says the agent now **starts work ~50% faster**, reducing "time to first action" before it begins changing code. On effectiveness, it now uses **semantic code search** alongside text matching, helping it find code by intent; GitHub says this cut **~2%** off task time in internal tests. This pairs with last week''s parallelism story (CLI `/fleet`, multi-agent editor workflows): faster orientation and better code location make delegated tasks easier to use in practice.
The most practical change this week is control and inspectability. Repos can now **configure which validation tools the coding agent runs** when it changes code. By default it runs tests and lint plus GitHub security/quality checks (CodeQL, Advisory Database, secret scanning, Copilot code review) and tries to fix findings before returning results. Admins can now tune that set in **Repository Settings -> Copilot -> Coding agent**, including disabling heavier checks (like slow CodeQL) when they do not match the repo''s feedback loop, even though GitHub says these validations are free and on by default. This continues last week''s governance theme: as agents act more autonomously, teams want visible "speed vs safety" controls aligned with CI, not buried in prompt habits.
Two related improvements also help with audit and review. Session logs are now clearer and more actionable (explicit setup steps like clone + "agent firewall" start; inline output from `copilot-setup-steps.yml`; subagent work collapsed by default with an "in progress" indicator). And agent commits now include an **`Agent-Logs-Url` trailer** so reviewers can jump from a commit to the exact session logs that produced it, which helps both review context and later traceability. This extends last week''s observability push (VS Code OpenTelemetry, hook troubleshooting, debug snapshots) into Git history.
- [Copilot coding agent now starts work 50% faster](https://github.blog/changelog/2026-03-19-copilot-coding-agent-now-starts-work-50-faster)
- [Copilot coding agent works faster with semantic code search](https://github.blog/changelog/2026-03-17-copilot-coding-agent-works-faster-with-semantic-code-search)
- [Configure Copilot coding agent’s validation tools](https://github.blog/changelog/2026-03-18-configure-copilot-coding-agents-validation-tools)
- [More visibility into Copilot coding agent sessions](https://github.blog/changelog/2026-03-19-more-visibility-into-copilot-coding-agent-sessions)
- [Trace any Copilot coding agent commit to its session logs](https://github.blog/changelog/2026-03-20-trace-any-copilot-coding-agent-commit-to-its-session-logs)
## VS Code Copilot agents: Autopilot, multi-agent workflows, troubleshooting, and customization management
VS Code''s Copilot agent experience kept moving toward longer-running, more autonomous workflows, while adding guardrails and diagnostics for when agents go off track. This continues last week''s 1.111 autonomy modes discussion (default approvals, bypass approvals, autopilot) and the parallel effort to make customization debuggable (instruction discovery conventions, `/troubleshoot`, hook inspection).
In an Insiders walkthrough, VS Code previewed **Autopilot mode** and chat UX updates for multi-step sessions: "shimmers" to clarify generation state, **collapsed containers** to keep long runs readable, and input UI changes that surface agent controls. Autopilot reduces constant confirmations, but adds explicit **approval modes**, a **permissions picker**, max-retry limits, and a "task_complete" stop condition to keep unattended runs bounded. That matches last week''s framing that autonomy is a posture you choose, not a single switch.
The VS Code **1.112** update video covered details that show up in CLI/background sessions: you can now steer an in-progress run (steer after current tool call, add to queue, or stop-and-send). Startup is also safer with uncommitted changes: the session prompts what to do and shows an **expandable file list** so you know what will be copied, moved, or skipped. Autopilot extends to **CLI sessions** via `chat.autopilot.enabled`, with guidance to use isolated environments like dev containers or Codespaces when bypassing approvals. That echoes last week''s "safer automation" and sandboxing focus.
Troubleshooting also got more concrete: a new **`/troubleshoot`** command analyzes **agent debug logs** (with JSONL logging enabled) to explain why instructions or skills are not applied. Logs can now be **exported/imported** as JSON for team debugging, with a reminder they may contain sensitive content. Version 1.112 also added image analysis for workspace images, symbol paste that preserves location context (`sym:` references), better monorepo customization discovery (including parent `.github` customizations via `chat.useCustomizationsInParentRepositories`), and a unified "Open Customizations" view to manage agents, skills, instructions, and **MCP servers** (including disabling servers per session or workspace). This continues last week''s "move customization to files" direction: fewer implicit behaviors, more repo-visible configuration, and fewer mysteries when a skill or instruction does not apply.
For teams testing parallel agents, another video showed **multiple agent sessions side-by-side** as separate workstreams (feature, storage wiring, docs) so you are not blocked on one tool run. It complements last week''s CLI `/fleet` story by showing parallelization in-editor as a practical way to deal with tool latency.
- [Autopilot Mode with Justin Chen](https://www.youtube.com/watch?v=ne9l0S-JNE8)
- [Visual Studio Code and GitHub Copilot - What''s new in 1.112](https://www.youtube.com/watch?v=BC35VXggNDc)
- [Multi-agent workflows in VS Code](https://www.youtube.com/watch?v=J5KTpq7hVn4)
## MCP and plugins: giving Copilot real tools (Azure DevOps, GitHub secret scanning, Azure resources, Fabric)
MCP keeps becoming the plumbing that connects Copilot to real systems in a way teams can deploy and govern. After last week''s MCP momentum (bridging chat to permissioned tool calls) and the reminder that auto-approve is a policy choice, this week''s emphasis is moving from local experiments to operational deployments: hosted endpoints, security scanning in the inner loop, and platform skills that need repeatable setup.
For Azure DevOps, Microsoft announced a **public preview Remote Azure DevOps MCP Server**: a hosted MCP endpoint in Azure DevOps that uses streamable HTTP transport and removes the need to run a local server process. Setup is essentially an org-scoped URL in MCP config (for example, `https://mcp.dev.azure.com/{organization}`), but the prerequisite matters: the org must be **Entra-backed** (not MSA-only). Today, it works without extra onboarding in **Visual Studio + Copilot** and **VS Code + Copilot**; other clients (Copilot CLI, Claude Desktop/Code, ChatGPT) depend on upcoming Entra OAuth dynamic registration. Microsoft also signaled that local-server support remains "for now," but the repo is expected to be archived once remote reaches GA. In practice, this is the step that makes MCP easier to run in an enterprise: fewer local daemons, more centrally managed auth and endpoints.
On GitHub''s MCP side, a **public preview** lets AI coding agents trigger **GitHub secret scanning via the GitHub MCP Server** to catch leaked credentials in uncommitted changes (pre-commit/pre-PR). It requires **GitHub Secret Protection**. Setup differs by host: in Copilot CLI you install the Advanced Security plugin and add the MCP tool (`run_secret_scanning`); in VS Code you install the advanced-security agent plugin and run `/secret-scanning` from Copilot Chat. The key shift is bringing secret detection into the inner loop while agents generate or refactor config, scripts, and sample code, which is where secrets often slip in. That reinforces last week''s defense-in-depth guidance: wire scanners in as tools instead of assuming the model will remember.
For Azure, a guide walked through installing and verifying the **Azure Skills Plugin** across **Copilot CLI**, **VS Code**, and **Claude Code**, with an emphasis on proving tools are actually called. It covers Node.js 18+ (MCP servers via `npx`), `az login`, `azd auth login`, and smoke tests like "list my resource groups." It also calls out a common operational issue: Copilot CLI needs **`/mcp reload`** after install, and token or skill budgets can silently prevent skills from activating when multiple plugins are loaded. This matches last week''s "prompt less, context more" point: many "Copilot ignored me" cases are tool/config visibility problems.
Fabric also showed up via the FabCon/SQLCon recap: **Fabric MCP** (local MCP GA as open source; remote MCP in public preview) and "Agent Skills for Fabric" plugins so GitHub Copilot in the terminal can perform Fabric tasks via MCP tooling, alongside Git integration/CI/CD improvements and agent-enabled operational workflows. It follows the same arc as the Azure DevOps remote MCP announcement: move from "connect a tool" to "repeatable platform capability."
- [Azure DevOps Remote MCP Server (public preview)](https://devblogs.microsoft.com/devops/azure-devops-remote-mcp-server-public-preview/)
- [Secret scanning in AI coding agents via the GitHub MCP Server](https://github.blog/changelog/2026-03-17-secret-scanning-in-ai-coding-agents-via-the-github-mcp-server)
- [Azure Skills Plugin – Let’s Get Started!](https://devblogs.microsoft.com/all-things-azure/azure-skills-plugin-lets-get-started/)
- [''FabCon and SQLCon 2026: Unifying databases and Fabric on a single platform''](https://azure.microsoft.com/en-us/blog/fabcon-and-sqlcon-2026_unifying-databases-and-fabric-on-a-single-data-platform/)
## Copilot CLI and repo-native agent workflows: multi-model review, session forensics, and reproducible “agent memory”
Copilot CLI and "agents in the repo" content lined up on one theme: treat agent output as inspectable, repeatable, and standardizable, not one-off chat. This continues last week''s CLI direction (requesting Copilot code review from `gh`, `/fleet` parallelism, terminal-first workflows) and pushes further into repeatability and post-hoc analysis.
A Copilot CLI tip showed using `/review` as a local pre-PR gate scoped to **bugs, security, and performance**, then doing multi-agent review by using multiple model backends (for example, Gemini, Codex, Opus) and combining results. That can help surface different failure modes before CI, and it fits last week''s point that model choice is becoming explicit and policy-shaped. Here, the goal is coverage rather than debating a single "best model."
Another advanced video focused on forensics: Copilot CLI stores session history in a local **SQLite database**, which you can query to retrieve prior prompts and outputs, reconstruct past work, or feed history back into Copilot to critique prompting patterns. It complements last week''s "memory default-on" and "debug snapshots" story: even when memory sharing is not desired, teams still want reproducible records of what happened.
GitHub also highlighted **Squad**, an open-source, Copilot-based multi-agent orchestration approach that lives in the repo. It is repo-native: install a CLI, run `squad init`, and it generates an AI "team" (coordinator plus specialists) that works via branches and PRs, uses repo files for shared memory (for example, a committed "versioned decisions.md"), and keeps an audit trail under `.squad/`. Version-controlled memory (instead of an external vector DB) keeps it inspectable and portable. This matches last week''s instruction-file and hook movement: put agent behavior in on-disk artifacts you can review, diff, and govern. The post also describes safety patterns like independent review, where rejected output cannot be self-corrected by the same agent and fixes are pushed to a different persona. That is an organizational analogue to last week''s separate approvals and defense-in-depth posture.
- [How to get a multi-agent code review in Copilot CLI](https://www.youtube.com/watch?v=qRXztN1hi1M)
- [How to query your Copilot CLI session history | Advanced tips & tricks](https://www.youtube.com/watch?v=XHTc6XF0gdk)
- [How Squad runs coordinated AI agents inside your repository](https://github.blog/ai-and-ml/github-copilot/how-squad-runs-coordinated-ai-agents-inside-your-repository/)
## Copilot customizations and “skills” as shared team assets
Copilot customization is increasingly treated as reusable tooling, not personal prompt snippets. This builds on last week''s shift to repo-discoverable instruction sources (AGENTS.md/CLAUDE.md, `.github/instructions`, hooks) and the emerging skills ecosystem (`dotnet/skills`, skill specs, evaluation loops). The common thread is that "how Copilot behaves" is becoming versioned project configuration, not chat lore.
The community-driven **Awesome GitHub Copilot Customizations** project shipped a website and Learning Hub, plus a plugin marketplace-style flow to discover and adopt agents, skills, instructions, and plugins via PR-reviewed, traceable GitHub workflows. The catalog is now large (hundreds of items), and the site adds search, previews, and one-click installs for VS Code/Insiders, plus CLI install patterns like `copilot plugin install <plugin>@awesome-copilot`. It also reflects an ongoing simplification effort: consolidating formats so skills become the common reusable unit over time, which helps teams package and maintain standard behaviors more easily. That lines up with last week''s point that reusable skills tend to produce more reproducible behavior than ad-hoc prompting.
A short VS Code intro reinforced the same product direction: **Agent Skills** bundle instructions and resources into a named capability invoked on demand (for example, `/skill-name`) instead of expanding system prompts or rewriting checklists. The aim is predictability: teams codify conventions, navigation routines, and run/test steps as repeatable skills discoverable in chat. Paired with last week''s instruction discovery and troubleshooting, the pattern is explicit, discoverable, and debuggable customization.
- [Awesome GitHub Copilot gets a website, Learning Hub, and plugin marketplace](https://devblogs.microsoft.com/blog/awesome-github-copilot-just-got-a-website-and-a-learning-hub-and-plugins)
- [Want to give your coding agent new capabilities? Use Agent Skills in VS Code](https://www.youtube.com/shorts/7fzCsefkgKk)
## Copilot in real workflows: MAUI repo agents, cross-arch C++ migrations, designers prototyping in code, and hosted agent production paths
A few deeper pieces showed what "Copilot as workflow" looks like when embedded in real contribution pipelines. This extends last week''s "portfolio-scale modernization" and "agents as workflow participants" theme, but with concrete layouts, gates, and deployment paths teams can reuse.
The .NET MAUI team described repo-native **Copilot CLI agents and composable skills** under `.github/agents` and `.github/skills`, including a structured `pr-review` skill with phases that enforce tests-first and multi-platform validation. The "Gate" step blocks progress until tests exist and are shown to fail on the unfixed code; "Try-Fix" runs multiple fix attempts (including multi-model sequences) and records outcomes; a final PR-ready report is produced for humans. The post also details test generation (Appium UI tests, XAML runtime/XamlC/source-gen coverage) and CI debugging (Helix logs), anchoring the flow to real build/test infrastructure rather than pure code generation. It aligns with last week''s governance direction by making guardrails and phase gates repo-visible and repeatable.
A migration guide walked through using Copilot to refactor C++ during IBM Power/IBM i big-endian to Azure x86 little-endian ports, where apps can "work" while silently corrupting numeric values. Copilot helps with mechanical refactors (finding unsafe `memcpy` into structs, generating byte-swap helpers with C++20 `std::endian` and intrinsics), while the article stresses the parts you cannot outsource (ABI/padding checks via `static_assert`, EBCDIC conversions, packed decimals). It is the modernization arc from last week, with a reminder that correctness comes from platform constraints, tests, and review, not prompting.
A design-focused discussion described designers using VS Code + Copilot (Chat and CLI) for code-based prototyping in repos, using plan-mode-like structure and parallel Copilot sessions paired with Git worktrees to test UI variants without branch friction. It mirrors last week''s multi-surface Copilot story (IDE + CLI + web) and shows parallel sessions as a broader collaboration pattern.
A tutorial also showed moving from prompt prototype to hosted agent using **VS Code AI Toolkit + Microsoft Foundry**, with Copilot used for model comparison and scaffolding Agent Framework templates (Python/C#), then using Foundry evaluation, LLM-judge scoring, red teaming, and monitoring in production. This continues last week''s "Beyond Prompts" message (execution loops, tools, observability), and it ends in deployment monitoring rather than an interactive session.
- [Accelerating .NET MAUI Development with AI Agents](https://devblogs.microsoft.com/dotnet/accelerating-dotnet-maui-with-ai-agents/)
- [''Porting C++ from IBM Power to Azure x86: fixing silent endianness corruption with GitHub Copilot''](https://techcommunity.microsoft.com/t5/azure-migration-and/porting-c-from-ibm-power-to-x86-solving-silent-endianness/ba-p/4501666)
- [''Why Design Feels Different: Designers using VS Code and GitHub Copilot for code-based prototyping''](https://www.youtube.com/watch?v=CMvnRYgB5Ac)
- [''From Prototype to Production: Building a Hosted Agent with AI Toolkit & Microsoft Foundry''](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-prototype-to-production-building-a-hosted-agent-with-ai/ba-p/4501969)
## Other GitHub Copilot News
Usage reporting and governance kept catching up to how Copilot is actually used across terminals and model pickers. Building on last week''s enterprise-management angle (usage metrics, routing/session controls, and transparency into which model responded), this week closes two reporting gaps: org-wide CLI adoption and the "Auto" model bucket.
Org admins can now see **Copilot CLI activity at org scope** (daily active users, sessions/requests, token totals, average tokens/request) in 1-day and 28-day windows, and via the usage metrics REST API. Separately, usage metrics now **resolve "Auto" model selection** into the actual model used in dashboard and API breakdowns, removing an ambiguous bucket that made cost and policy analysis harder (though GitHub says it still does not split auto-selected vs manual). This ties back to last week''s JetBrains auto-routing story: once routing is automated, reporting needs to support cost and compliance governance.
- [Copilot usage metrics now includes organization-level GitHub Copilot CLI activity](https://github.blog/changelog/2026-03-17-copilot-usage-metrics-now-includes-organization-level-github-copilot-cli-activity)
- [Copilot usage metrics now resolve auto model selection to actual models](https://github.blog/changelog/2026-03-20-copilot-usage-metrics-now-resolve-auto-model-selection-to-actual-models)
GitHub''s planning surface also added workflow changes related to agent delegation. **Projects Hierarchy view** is GA and enabled by default for new views, which makes sub-issue trees easier to manage. Issue templates can also auto-assign **@copilot** via YAML (`assignees: ["@copilot"]`) to route new issues to the coding agent (where permitted). GitHub also adjusted blank-issue behavior to preserve structured intake for contributors while still letting maintainers create blank issues. After last week''s "trigger agentic review from CLI" and "agents doing routine triage," this is a practical intake step: route work into an agent-friendly pipeline up front, not after manual rerouting.
- [Hierarchy view in GitHub Projects is now generally available](https://github.blog/changelog/2026-03-19-hierarchy-view-in-github-projects-is-now-generally-available)
Agent monitoring also showed up in Raycast: the GitHub Copilot Raycast extension now lets you **stream agent logs live** from "View Tasks," which mainly reduces context switches and helps spot stalled sessions without living in the web UI. It fits last week''s observability trend (debug snapshots, OpenTelemetry traces): as agents run longer, "where do I see what it''s doing?" becomes a normal workflow question.
- [Monitor Copilot coding agent logs live in Raycast](https://github.blog/changelog/2026-03-20-monitor-copilot-coding-agent-logs-live-in-raycast)
A couple of getting-started items focused on standardizing Copilot''s environment. One video covered installing Copilot CLI on Windows (winget) and adding the **Work IQ** plugin (including EULA acceptance), with notes on interactive vs non-interactive usage, continuing last week''s CLI onboarding. Another short reminded maintainers that `.github` is the repo''s workflow control plane (Actions, templates, and a home for Copilot instructions and related guidance), echoing last week''s move to repo-visible instruction sources.
- [Getting Started with GitHub Copilot CLI and Work IQ](https://www.youtube.com/watch?v=tQlNq8bH674)
- [The powerful GitHub folder most developers ignore](https://www.youtube.com/shorts/X4XiMKott2E)
Microsoft Fabric''s extensibility tooling also reached a new baseline with the **Fabric Extensibility Toolkit GA**, including a "Copilot-optimized" starter kit (repo context under `.ai/` plus DevContainer/Codespaces setup) and React/Fluent UI building blocks for Fabric workload items, plus OneLake-backed storage patterns and Entra OBO auth flows. It connects to last week''s "structured context + reusable skills" story and this week''s Fabric MCP mention: platforms are shipping repo scaffolding that assumes agents are part of the workflow.
- [Microsoft Fabric Extensibility Toolkit is now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-extensibility-toolkit-generally-available/)
For developers building Copilot extensions, a VS Code episode walked through creating a project with the **GitHub Copilot SDK** from scratch, pointing to the canonical repo for templates and APIs. This continues last week''s "Copilot SDK = execution loops and tools" message, but as a more direct starter path for teams productizing internal agents.
- [''Let it Cook - GitHub Copilot SDK: Fresh from Scratch''](https://www.youtube.com/watch?v=uzmnpGmR2tg)',
    'This week''s Copilot story is less about one headline feature and more about Copilot settling into three practical layers teams run every day: (1) clearer model choice and governance, (2) agent workflows with the observability and safety controls teams expect, and (3) broader MCP tool access so Copilot can act with real platform context (Azure DevOps, GitHub scanners, Azure resources, Fabric) instead of relying on chat history guesses. Building on last week''s themes (auto model selection across IDEs, repo-visible instruction files and hooks, and enterprise observability), this week adds more of the operational layer needed for scale: stable model windows, adjustable validations, and more traceable agent execution.',
    1774252800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-03-23', 'TechHub',
    'TechHub', 'D6AFB6E3DBAB279B55BF72C8A3DD25427C8F34D87206072E885685ED231D1BD8', ',GitHub Copilot,Copilot Business,Copilot Enterprise,GPT 5.4 Mini,GPT 5.3 Codex LTS,Model Governance,Coding Agent,VS Code,Autopilot Mode,MCP,Azure DevOps MCP Server,GitHub Advanced Security,Secret Scanning,Copilot CLI,Agent Skills,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-03-16
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-03-16', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, Hooks, and Safer Autonomy',
    'This week’s Copilot updates continued the move toward agentic workflows: more autonomy in editors and the CLI, more customization through instruction files and hooks, and more attention on running agents safely in real repositories. Building on last week’s VS Code lifecycle hooks, default memory, and MCP tool integrations, these ideas are now showing up across IDEs (notably JetBrains) with more reviewable on-disk configuration plus better observability, troubleshooting, and governance. Copilot also kept improving everyday workflows, including faster terminal-based code review requests, better web repo exploration, and portfolio-scale modernization that ties code changes to migration planning.
<!--excerpt_end-->
## Agentic Copilot in IDEs: customization, autonomy, and observability
GitHub Copilot’s JetBrains plugin moved key in-IDE agent building blocks to GA - Custom Agents, Sub-agents, and a Plan Agent - so teams can define specialized roles inside IntelliJ workflows. This matches last week’s “configurable teammate” theme from VS Code and helps teams using multiple IDEs get more consistent behavior. Customization is also shifting toward files: Copilot can discover AGENTS.md and CLAUDE.md in the workspace (or apply them globally), and a new `/memory` command jumps to settings for those instruction sources. With Copilot Memory now default for Pro/Pro+, the direction is clear: manage agent behavior with explicit, reviewable instruction artifacts instead of relying on transient chat context.
In preview, JetBrains adds Agent Hooks to run automation at lifecycle events (userPromptSubmitted, preToolUse, postToolUse, errorOccurred) via `.github/hooks/hooks.json`, enabling policy checks and integrations during agent execution. This continues last week’s VS Code lifecycle-hook work (auto-linting, output restrictions, safer automation) and brings similar tool-usage hook points to JetBrains in a repo-visible way. For MCP tool usage, Auto-Approve for MCP reduces repeated confirmations by letting you set auto-approval per MCP server/tool in JetBrains settings, but teams should treat it as a policy choice, especially given last week’s emphasis on terminal sandboxing and safety constraints.
JetBrains also has Auto Model Selection GA across all plans. Copilot can route completions among models (including GPT‑5.4, GPT‑5.3‑Codex, Sonnet 4.6, Haiku 4.5) based on availability/performance while respecting entitlements and admin policies, and it shows which model responded. This connects to last week’s model-management thread (explicit model selection for `@copilot` in PR comments, plus deprecations/migrations) by adding policy-bounded routing with transparency. Billing adds a detail: auto-routed requests get a 10% discount on the model’s premium multiplier (and currently routes only to 0x–1x multiplier models).
VS Code’s Copilot surface area evolved in parallel. Insiders 1.111 aligns instruction handling by recursively discovering `*.instructions.md` under `.github/instructions/`, which reduces “why was my instruction ignored?” differences across CLI, agents, and editor. This extends last week’s “shared memory/context” theme toward a clearer repository convention. Customization is also easier to debug and more modular: custom agent frontmatter supports agent-scoped chat hooks, and `/troubleshoot` helps inspect hook/customization behavior from chat. After last week’s conversation forking, live steering, and memory controls, the emphasis this week is diagnosing how hooks, instructions, and tool policies interact.
Insiders also added OpenTelemetry instrumentation for Copilot Chat (subject to telemetry settings), giving teams traces to diagnose latency, failures, or regressions during controlled rollouts. This complements last week’s enterprise management angle (session filtering, network routing, usage metrics) by expanding visibility from “what users did” to “what happened in the agent loop, and why.”
VS Code 1.111 (and discussion of the weekly stable cadence) also put more focus on autonomy modes: default approvals, bypass approvals, and an autopilot mode where Copilot continues end-to-end with auto-approved tool calls and retries. This builds on last week’s autonomy trend (more capable agents, terminal sandboxing, output restrictions) while making trade-offs more explicit: fewer interruptions for multi-step tasks, but a different security posture on developer machines. The same release also expanded hook control (scoping hooks to custom agents behind `chat.useCustomAgentHooks`) and improved debugging with “debug events snapshot” attachments you can carry into a new chat (including references like `#debugEventsSnapshot`) to diagnose issues such as unsupported model config values.
- [Major Agentic Capabilities Improvements in GitHub Copilot for JetBrains IDEs](https://github.blog/changelog/2026-03-11-major-agentic-capabilities-improvements-in-github-copilot-for-jetbrains-ides)
- [GitHub Copilot Auto Model Selection Launches for JetBrains IDEs](https://github.blog/changelog/2026-03-12-copilot-auto-model-selection-is-generally-available-in-jetbrains-ides)
- [What''s New in Visual Studio Code Insiders 1.111](https://code.visualstudio.com/updates/v1_111)
- [Visual Studio Code and GitHub Copilot - What''s new in 1.111](https://www.youtube.com/watch?v=Z-psHv_W5Yc)
- [Microsoft Accelerates VS Code Releases, Introduces Autopilot AI Mode](https://www.devclass.com/development/2026/03/12/microsoft-ships-vs-code-weekly-adds-autopilot-mode-so-ai-can-wreak-havoc-without-bothering-you/5208978)
## GitHub Copilot CLI and PR workflows: multi-agent parallelism and terminal-first review
Copilot’s terminal workflows kept reducing the “do it without leaving the shell” gaps. After last week’s Copilot CLI GA push (plus tutorials, metrics, dashboards), this week connects terminal work more directly to PR review, including a faster way to request agentic review. GitHub CLI v2.88.0 can request Copilot Code Review from the command line by adding Copilot as a reviewer (`gh pr edit --add-reviewer @copilot`) or selecting it during interactive PR creation. With last week’s focus on Copilot Code Review’s agent architecture and scale, this is a simple way to trigger the same review automation without switching to the web UI.
In the same release, reviewer/assignee selection moved to search-as-you-type instead of preloading large collaborator lists, which improves performance in large orgs and improves accessibility (screen readers avoid massive lists). It also supports the enterprise adoption theme from last week: as Copilot expands, supporting tooling is adjusting to large repositories and workflows.
On the Copilot CLI side, the new `/fleet` command goes beyond single-agent help by running multiple Copilot-powered tasks in parallel from one prompt, targeting maintenance work like dependency upgrades, documentation updates, and batching issues. This builds on last week’s “agent workflows beyond the IDE” direction (CLI automation, repo bots, MCP servers) by moving from sequential help to parallel orchestration, then returning combined output for review before merging.
For onboarding, beginner tutorials covered setup (npm install, auth, folder permissions) and two prompting styles: interactive mode for iterative sessions versus non-interactive `-p` for quick one-offs. This continues last week’s hands-on learning angle and reinforces that terminal workflows are a first-class Copilot surface, not just an IDE add-on.
- [Request Copilot Code Review Directly from GitHub CLI](https://github.blog/changelog/2026-03-11-request-copilot-code-review-from-github-cli)
- [Accelerate Issue Resolution with GitHub Copilot CLI Fleet Command](https://www.youtube.com/shorts/lHtQPImRJSc)
- [How to Use the /fleet Command in GitHub Copilot CLI](https://www.youtube.com/watch?v=BviR6Me36gI)
- [Getting Started with GitHub Copilot CLI: Beginner''s Tutorial](https://www.youtube.com/watch?v=BDxRhhs36ns)
- [Interactive vs Non-Interactive Modes in GitHub Copilot CLI](https://www.youtube.com/watch?v=bdIJkGr2NV0)
- [Streamlining Dependency Updates with GitHub Copilot CLI](https://www.youtube.com/shorts/1npISWbvle4)
- [Learn to use an AI agent in your terminal | Copilot CLI for beginners](https://www.youtube.com/shorts/yXZVNfnAoCk)
## Modernization agents and migration integration: from repo changes to portfolio orchestration
Modernization came up repeatedly, with Copilot positioned less as “write code” and more as “upgrade estates.” This fits the progression from last week’s agentic repo automation and enterprise controls (session filtering, routing changes, user metrics): as Copilot automates across many repositories, teams need plans, governance, and standardized skills. GitHub Copilot’s modernization agent entered Public Preview with a portfolio workflow: run assessments, generate plans, and convert output into GitHub Issues and PRs so work stays in normal review/CI. It also emphasizes cross-repo coordination (async execution), centralized monitoring/governance via Agent HQ, and custom skills to enforce org rules across upgrades, especially for .NET and Java.
Microsoft also introduced `modernize-dotnet`, a Copilot coding agent with an “Assess → Plan → Execute” flow callable from Visual Studio (Solution Explorer), VS Code (modernization extension), Copilot CLI (marketplace plugin), and directly in GitHub repos. This continues last week’s “Copilot across surfaces” thread (IDE + web + CLI + MCP), but applied to long-running, review-heavy work. The focus is on producing structured, versionable artifacts (assessment and plan) alongside code changes so teams can review intent, not only diffs. It covers many workloads (ASP.NET Core, Blazor, Azure Functions, WPF, libraries, console apps) and supports .NET Framework migrations (Windows required).
At the program level, Azure Copilot’s Migration Agent and GitHub Copilot are described as complementary. Azure Copilot handles discovery/assessment (including agentless VMware discovery and offline inventory via Azure Migrate Collector), produces 6R recommendations, wave plans, and landing-zone setup aligned to the Cloud Adoption Framework, then links developer modernization work via GitHub Copilot integration. The shared theme is tighter coupling between “what to modernize/migrate” and “make code/config changes,” reducing silos between infrastructure planning and repo work. It also extends last week’s point that agents are becoming workflow participants that need traceability and controls.
- [How GitHub Copilot Agents Enable Application Modernization at Scale](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-single-apps-to-scale-solutions-how-ai-agents-scale/ba-p/4500059)
- [Modernize .NET Projects Anywhere Using GitHub Copilot and modernize-dotnet](https://devblogs.microsoft.com/dotnet/modernize-dotnet-anywhere-with-ghcp/)
- [Scaling Modernization on Azure with Agentic AI Tools](https://azure.microsoft.com/en-us/blog/many-agents-one-team-scaling-modernization-on-azure/)
- [Accelerating Cloud Modernization with Azure Copilot Migration Agent and GitHub Copilot Integration](https://techcommunity.microsoft.com/t5/azure-migration-and/azure-copilot-migration-agent/ba-p/4501292)
## Security, governance, and “auto-approval” controls for agentic workflows
As Copilot shifts toward autonomy (auto-approve, autopilot, multi-agent execution), security guidance is becoming more specific. This follows last week’s lifecycle hooks, terminal sandboxing, output restrictions, enterprise session filters, and outbound routing changes: as agents gain permissions, teams need consistent guardrails across IDEs, Actions, and the CLI. GitHub’s deep dive on “GitHub Agentic Workflows” security (agents via GitHub Actions) describes a defense-in-depth model that treats the agent as untrusted and designs for prompt injection, privilege escalation, secret exfiltration, and unsafe repo changes. It uses isolation (Actions runner VMs, hardened containers), declarative permission scoping, firewalled egress, and “zero secrets for agents” (secrets outside agent runtime in isolated proxies/containers). Repository writes go through a “safe outputs” pipeline so changes can be buffered and vetted before applying, backed by policy controls (limits on PR/issue creation, moderation/sanitization) and logging/observability for audit and incident response.
GitHub also added a repository-level Actions control: admins can optionally skip the “Approve and run workflows” gate for workflows triggered by the Copilot coding agent. That speeds CI while Copilot iterates on PRs, but it moves the speed/safety tradeoff onto workflow hardening (token permissions, secret exposure, environment protections, and exfiltration risk under malicious inputs). It lines up with this week’s autonomy levers in IDEs (JetBrains MCP auto-approve, VS Code autopilot/bypass approvals) with an equivalent CI-side switch. It can work well when hardening is in place, and it can be risky when it is not.
- [''Security Architecture of GitHub Agentic Workflows: Deep Dive''](https://github.blog/ai-and-ml/generative-ai/under-the-hood-security-architecture-of-github-agentic-workflows/)
- [Optionally Skip Approval for GitHub Copilot Coding Agent Actions Workflows](https://github.blog/changelog/2026-03-13-optionally-skip-approval-for-copilot-coding-agent-actions-workflows)
## Other GitHub Copilot News
Copilot’s web experience added a repository-exploration preview that brings file tree browsing into Copilot Chat on GitHub.com. You can ask Copilot to open files, navigate the tree, and attach files as temporary references. To keep context, you can pin a file by converting it into a permanent reference from chat tokens or the file preview. This matches last week’s push for persistent context (Copilot Memory default-on) and faster repo exploration in IDE agents, and it gives the web UI more explicit, user-controlled context attachment and retention.
- [Explore a Repository Using GitHub Copilot on the Web](https://github.blog/changelog/2026-03-11-explore-a-repository-using-copilot-on-the-web)
Copilot extensibility continued shifting toward building agents into products and internal tools. The Copilot SDK post emphasizes execution loops (plan → act → recover → continue) instead of one-off prompts, using structured context plus first-class tools/skills, and using MCP to connect to internal APIs so workflows stay permissioned and observable, even outside IDEs in services, SaaS, or event-driven/serverless setups. This extends last week’s MCP momentum (including Figma MCP): MCP is increasingly the bridge to real systems without relying on prompt-only approaches.
- [''Beyond Prompts: Agentic AI Workflows with GitHub Copilot SDK''](https://github.blog/ai-and-ml/github-copilot/the-era-of-ai-as-text-is-over-execution-is-the-new-interface/)
In the skills ecosystem, Microsoft’s .NET team introduced `dotnet/skills` to package reusable agent skills using the Agent Skills specification. It targets marketplace distribution (Copilot CLI and VS Code Copilot) and includes an evaluation/validation loop comparing outputs with and without a skill before merging, with the goal of more reproducible behavior instead of ad-hoc prompting. This continues last week’s custom plugins and “build agents/skills from chat” thread, and it connects to this week’s modernization theme: standardized skills help keep many agents aligned with internal rules.
- [''Enhance Coding Agents with .NET Skills: Introducing dotnet/skills''](https://devblogs.microsoft.com/dotnet/extend-your-coding-agent-with-dotnet-skills/)
GitHub also shared a “Copilot + Actions” pattern for accessibility feedback: structured reports become consistently triaged issues using prompt files/versioned instructions, Actions orchestration, and GitHub Models API analysis, while keeping humans responsible for validation and decisions. The implementation details (templates, .github instructions, PR-reviewed prompts, re-runs) are a useful reference for operationalizing Copilot in issue workflows without losing governance. It also matches this week’s instruction files + hooks + observability direction and last week’s repository automation bot theme.
- [''Continuous AI for Accessibility: How GitHub Automates and Accelerates Inclusive Feedback''](https://github.blog/ai-and-ml/github-copilot/continuous-ai-for-accessibility-how-github-transforms-feedback-into-inclusion/)
The VS Code team published a playbook-style case study using Copilot (CLI + SDK) and GitHub Actions for issue triage, release notes, PR validation, and “Copilot-first” review (Copilot as first reviewer on every PR). They describe custom agents with Playwright MCP for UI flow validation and screenshot evaluation, plus running multiple agent tasks in parallel across worktrees and sessions. It reads like a field report tying together CLI-first workflows, agentic review at scale, MCP tools, and the need to standardize instructions and guardrails as agents take on more routine work.
- [How VS Code Team Uses AI and GitHub Copilot to Accelerate Development](https://code.visualstudio.com/blogs/2026/03/13/how-VS-Code-Builds-with-AI)
Design-to-code got another MCP example: Figma Dev Mode context into VS Code so Copilot can generate UI code from structured design data (not screenshots), and guidance to encode design-system rules as reusable skills for consistent iteration. This continues last week’s Figma MCP story, with more focus on structured metadata and reusable skills to keep output consistent across teams and repositories.
- [''Figma MCP for VS Code: AI-Driven Design-to-Code Collaboration''](https://www.youtube.com/watch?v=KCu3g2_xqzM)
A best-practices post argued “prompt less, context more”: select relevant code, add short intent when needed, and expand workspace context rather than writing longer prompts, especially for review/refactor work like readability, error handling, and validation. This pairs with last week’s persistent memory and this week’s expanded instruction discovery: as teams invest in durable context (memory, instructions, pinned references), they can rely less on elaborate prompting and more on repeatable, reviewable repository context.
- [''Prompt Less, Context More: How to Get Better Results with GitHub Copilot''](https://www.cooknwithcopilot.com/blog/prompt-less-context-more.html)
Copilot plan changes reached education users: GitHub announced a new Copilot Student plan for GitHub Education beneficiaries and noted an updated model lineup for student accounts (details expected via the linked community thread). With last week’s model rollouts (GPT‑5.4) and deprecations/migrations, it’s another reminder that model access is increasingly shaped by plan and policy across segments.
- [''Updates to GitHub Copilot for Students: New Student Plan Announced''](https://github.blog/changelog/2026-03-13-updates-to-github-copilot-for-students)
A demo showed Copilot applied to hardware-adjacent scripting: using a structured “servo movement vocabulary” file (servos.md) so Copilot generates constrained movement sequences for a Raspberry Pi robot dog greeting routine triggered by face detection. It’s the same pattern as instruction files and skills, applied to a non-traditional codebase: constrain the agent with explicit, reviewable context so outputs stay within safe/valid bounds.
- [Giving a robot dog a personality using GitHub Copilot](https://www.youtube.com/watch?v=rUxB9M69e_Y)',
    'This week’s Copilot updates continued the move toward agentic workflows: more autonomy in editors and the CLI, more customization through instruction files and hooks, and more attention on running agents safely in real repositories. Building on last week’s VS Code lifecycle hooks, default memory, and MCP tool integrations, these ideas are now showing up across IDEs (notably JetBrains) with more reviewable on-disk configuration plus better observability, troubleshooting, and governance. Copilot also kept improving everyday workflows, including faster terminal-based code review requests, better web repo exploration, and portfolio-scale modernization that ties code changes to migration planning.',
    1773648000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-03-16', 'TechHub',
    'TechHub', '4E453B47DAB8E22DC23B66CF06B12E129AC6A8EA654616264D4282B1BF82F7E3', ',GitHub Copilot,Agentic Workflows,VS Code,JetBrains,GitHub CLI,Copilot Code Review,Instruction Files,Copilot Memory,Lifecycle Hooks,MCP,Auto Model Selection,OpenTelemetry,GitHub Actions,Modernization Agent,.NET Modernization,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-03-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-03-09', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, CLI, GPT-5.4, and Metrics',
    'The Copilot section this week covers updates in AI tooling for developers, including improved agent functions in VS Code, Microsoft platform integrations, recent model rollouts, and broader workflow and analytics options. The new features reinforce Copilot''s growing use in regular coding, CLI automation, and review tasks, supporting both organizational and individual needs.
<!--excerpt_end-->
## Copilot Agent Automation and Extensibility in Visual Studio Code
The February 2026 update to VS Code v1.110 includes automation and extensibility features for Copilot. Building on past agent automation and custom agent creation, it introduces lifecycle hooks for automated activity, like auto-linting and output restrictions that help maintain safe workflows. You can now fork conversations to explore different code paths within agent chats. Slash commands in chat streamline approvals and actions, and terminal sandboxing adds safety for prototyping. Live steering lets you shift task focus during execution. There''s experimental support for custom third-party plugins. Developers can access browser tools, and build agents, prompts, or skills from chat. Integration with the CLI is more flexible, with improved diff views and folder syncing. Persistent memory helps agents retain context and information, making recall across sessions easier. Tools like Explore Subagent speed up codebase searching, and memory compaction gives manual control over session data. Predictive edits now use complete file context for more relevant suggestions. Terminal images and interface updates (including accessibility changes) lead to a more productive and collaborative environment in VS Code.
- [GitHub Copilot Agents and Extensions in Visual Studio Code February 2026 Release](https://github.blog/changelog/2026-03-06-github-copilot-in-visual-studio-code-v1-110-february-release)
## GitHub Copilot CLI: General Availability and Practical Tutorials
Copilot CLI is now available for all users, letting developers work with Copilot in their terminal. After last week’s general availability announcement, developers can access code suggestions, reviews, and simple automation workflows outside the IDE. CLI users benefit from diff navigation, trusted sync, and snippet-based actions. There are telemetry and user-metrics options for teams to track usage. A free eight-part open source course helps you set up CLI, build custom agents, and run your own MCP server with hands-on labs. User-level metrics help with resource planning. Official dashboards and how-to guides demonstrate how Copilot CLI can help in day-to-day command-line work.
- [GitHub Copilot CLI General Availability and New Repository Dashboard](/github-copilot/videos/github-copilot-cli-general-availability-and-new-repository-dashboard)
- [Get started with GitHub Copilot CLI: A free, hands-on course](https://devblogs.microsoft.com/blog/get-started-with-github-copilot-cli-a-free-hands-on-course)
- [Copilot Usage Metrics Expanded to User-Level GitHub Copilot CLI Activity](https://github.blog/changelog/2026-03-05-copilot-usage-metrics-now-includes-user-level-github-copilot-cli-activity)
## GPT-5.4 Integration: New Coding Model for Copilot
With the release of GPT-5.4, Copilot Pro, Pro+, Business, and Enterprise users in supported IDEs can now access better code generation and improved agent workflow support. It’s available on VS Code (v1.104.1+), Visual Studio (v17.14.19+), JetBrains (v1.5.66+), Xcode, Eclipse, the web, and in the CLI. These updates bring improvements to handling multi-step coding tasks and agent interactions. Enterprise admins must enable GPT-5.4, with guides provided for migration. Tutorials explain changes that improve reasoning and task management in VS Code Copilot.
- [GPT-5.4 is Generally Available in GitHub Copilot](https://github.blog/changelog/2026-03-05-gpt-5-4-is-generally-available-in-github-copilot)
- [GPT-5.4 Now Available in VS Code with GitHub Copilot](/github-copilot/videos/gpt-54-now-available-in-vs-code-with-github-copilot)
## Copilot Code Review: Agentic Architecture and Workflow Evolution
Copilot Code Review uses an agent-driven architecture to support more than 60 million automated reviews, now accounting for over 20% of all reviews on GitHub. Continuing last week''s agent-oriented code review discussion, Copilot supplies detailed context, recognizes recurring code issues, and makes targeted suggestions on correctness and design. Batch review comments and automated multi-line fixes are designed to make feedback clearer. Self-hosted runner setup is needed for these features, while GitHub-hosted runners work automatically. Continuous improvements are based on telemetry and team reactions. Future plans include adaptive personalization and more interactive team review features.
- [Copilot Code Review Now Runs on Agentic Architecture](https://github.blog/changelog/2026-03-05-copilot-code-review-now-runs-on-an-agentic-architecture)
- [60 Million GitHub Copilot Code Reviews: Enhancing Developer Workflows with AI-Powered Review](https://github.blog/ai-and-ml/github-copilot/60-million-copilot-code-reviews-and-counting/)
## Copilot Memory: Persistent Context Now On by Default
For Pro and Pro+ users, Copilot Memory is now enabled by default. This feature stores repository-level conventions, patterns, and dependencies, refreshing every 28 days to keep the context relevant. This capability supports better code completion, reviewing, and CLI interaction. Users may opt out, and organization admins control settings. The documentation provides more about operation and updates, reflecting last week''s move from opt-in to default for persistent memory.
- [Copilot Memory Now Enabled by Default for GitHub Copilot Pro and Pro+ Users](https://github.blog/changelog/2026-03-04-copilot-memory-now-on-by-default-for-pro-and-pro-users-in-public-preview)
## Model Management: Selection and Deprecation
Now, you can select specific Copilot AI models such as GPT-5.4 when you use @copilot in pull request comments. This lets you tailor model selection for each review, with the picker available on github.com, and plans to support GitHub Mobile. Gemini 3 Pro and GPT-5.1 are being deprecated; admins are advised to migrate to Gemini 3.1 Pro and GPT-5.3-Codex with official support to guide the transition.
- [Pick a Model for @copilot in Pull Request Comments](https://github.blog/changelog/2026-03-05-pick-a-model-for-copilot-in-pull-request-comments)
- [Deprecation Notice: Gemini 3 Pro and GPT-5.1 Models in GitHub Copilot](https://github.blog/changelog/2026-03-02-upcoming-deprecation-of-gemini-3-pro-and-gpt-5-1-models)
## Figma MCP Server Integration: Design-to-Code Workflows
Copilot now connects VS Code with the Figma MCP server, allowing developers to import UX/UI content for code or export components as editable frames in Figma. This helps bridge design and engineering tasks. Setup guides are available for continuous integration, with CLI support planned. These updates provide deeper automation across tools, picking up from last week''s discussion on design-engineering workflows.
- [Figma MCP Server Integration with GitHub Copilot in VS Code](https://github.blog/changelog/2026-03-06-figma-mcp-server-can-now-generate-design-layers-from-vs-code)
- [VS Code Live: Code to Canvas with Figma MCP and GitHub Copilot](/github-copilot/videos/vs-code-live-code-to-canvas-with-figma-mcp-and-github-copilot)
## Agentic Workflows: Repository Automation and Open Source Bots
Copilot’s agentic workflows allow developers to automate repository tasks using markdown scripts. Tutorials cover safety precautions, scenario-based automation, and best practices for bots that manage issues or maintain projects. Home Assistant and similar open source projects use these workflows to reduce manual work and enable community support. Guidance is provided for setting up safe, effective automations.
- [How to use Agentic Workflows for Your Repos with GitHub Copilot](/github-copilot/videos/how-to-use-agentic-workflows-for-your-repos-with-github-copilot)
## Jira Integration: Coding Agent for Issue-Driven PR Automation
Copilot Coding Agent’s new link to Jira Cloud lets teams assign Jira tasks to the agent, which then drafts pull requests based on details and status, asking clarifying questions when necessary. It helps automate repetitive issue-resolution steps and keeps manual reviews intact. Marketplace app installation and repository-level access are required, and data residency controls are included. Documentation is available for setup, and feedback is invited for new features. This addition complements last week''s focus on agent-powered task automation.
- [GitHub Copilot Coding Agent Integration with Jira Now in Public Preview](https://github.blog/changelog/2026-03-05-github-copilot-coding-agent-for-jira-is-now-in-public-preview)
## Copilot Usage Metrics: Expanded Telemetry, Plan Mode, and Username Consistency
The Copilot metrics API now provides plan mode telemetry across IDEs and detailed CLI usage at a per-user level, helping organizations understand adoption and support users effectively. The dashboard reflects these changes, and Enterprise Managed Users (EMUs) now have consistent usernames to simplify large-scale tracking.
- [GitHub Copilot Usage Metrics Now Track Plan Mode Telemetry](https://github.blog/changelog/2026-03-02-copilot-metrics-now-includes-plan-mode)
- [GitHub Copilot Metrics Reports Now Return Consistent Usernames for EMUs](https://github.blog/changelog/2026-03-02-copilot-metrics-reports-now-return-consistent-usernames-for-enterprise-managed-users)
- [Copilot Usage Metrics Expanded to User-Level GitHub Copilot CLI Activity](https://github.blog/changelog/2026-03-05-copilot-usage-metrics-now-includes-user-level-github-copilot-cli-activity)
## Agent Management and Network Configuration
Enterprise admins gain session filtering by agent status, repository, and user, simplifying oversight. Outbound network routing for Copilot agents now depends on specific plans; allowlists need updating for self-managed and Azure runners, though GitHub-hosted runners remain unchanged. Documentation explains the settings, updating last week''s content on enterprise management and registry.
- [Discover and Manage GitHub Copilot Agent Activity with New Session Filters](https://github.blog/changelog/2026-03-05-discover-and-manage-agent-activity-with-new-session-filters)
- [Network Configuration Update for Copilot Coding Agent Now Active](https://github.blog/changelog/2026-03-02-network-configuration-changes-for-copilot-coding-agent-now-in-effect)
## Global Developer Events and Skill Expansion
GitHub Copilot Dev Days are rolling out worldwide, offering workshops, demos, and hands-on sessions for VS Code and Visual Studio users starting March 15, 2026. GitHub is working with Andela to grow AI skills, offering training and mentorship in Africa and Latin America. This focus on training supports faster onboarding and more complex project management, reflecting Copilot’s dedication to developer education.
- [GitHub Copilot Dev Days: Accelerate Coding with AI in the Microsoft Developer Stack](https://devblogs.microsoft.com/blog/github-copilot-dev-days)
- [Join or Host a GitHub Copilot Dev Days Event Near You](https://github.blog/ai-and-ml/github-copilot/join-or-host-a-github-copilot-dev-days-event-near-you/)
- [How GitHub Copilot and Andela Are Expanding AI Skills Globally](https://github.blog/developer-skills/career-growth/scaling-ai-opportunity-across-the-globe-learnings-from-github-and-andela/)
## Other GitHub Copilot News
The Grok Code Fast 1 AI model is now included for automatic model selection in Copilot Free, making updated suggestions accessible in leading IDEs. Developers using .NET MAUI can try a demonstration of Copilot’s effect on their mobile workflow, covering scaffolding, troubleshooting, and deployment.
- [Grok Code Fast 1 is now available in Copilot Free auto model selection](https://github.blog/changelog/2026-03-04-grok-code-fast-1-is-now-available-in-copilot-free-auto-model-selection)
- [AI-First Mobile Development: Live Coding with Copilot and .NET MAUI](/github-copilot/videos/ai-first-mobile-development-live-coding-with-copilot-and-net-maui)',
    'The Copilot section this week covers updates in AI tooling for developers, including improved agent functions in VS Code, Microsoft platform integrations, recent model rollouts, and broader workflow and analytics options. The new features reinforce Copilot''s growing use in regular coding, CLI automation, and review tasks, supporting both organizational and individual needs.',
    1773043200, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-03-09', 'TechHub',
    'TechHub', '119113014E7BA4AA3DE5B49D98635F817FA9BAA02D6A2CB9063757D82FC717DA', ',GitHub Copilot,VS Code,Copilot Agents,Copilot CLI,GPT 5.4,Copilot Code Review,Copilot Memory,Model Selection,Pull Requests,Telemetry,Usage Metrics API,Enterprise Managed Users,MCP,Figma,Jira Cloud,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-03-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-03-02', 'roundups', 'Weekly GitHub Copilot Roundup: CLI Agents, Models, and Metrics',
    'This section highlights current updates for GitHub Copilot, including extended agent-based workflows, CLI automation, model selection, and improved analytics for enterprise users. The platform introduces the Copilot CLI with public availability, enhanced chat models, richer metrics APIs, and deeper integration for both developers and organizational teams. New guides break down recent architectural changes and provide directions for automation, governance, and productivity improvements.
<!--excerpt_end-->
## GitHub Copilot CLI: Agentic Terminal Workflows and Guides
GitHub Copilot CLI is now generally available for paid subscribers, offering an agent-based terminal for macOS, Linux, and Windows. Users access built-in agents (Explore, Task, Code Review, Plan) in both interactive and autonomous modes, and can select local or cloud sessions. Plan mode supports structured planning, while Autopilot automates repetitive or background commands, including batch processing with `&`. Developers can choose from Claude Opus/Sonnet, GPT-5.3-Codex, or Gemini 3 Pro models, and tune reasoning behavior for their work.
Recent CLI updates add support for MCP servers, Agent Skills in markdown for customized automation, plugin workflows, and new options to create custom agents or policy hooks. Code review is streamlined using `/diff` and `/review` features, file analysis, persistent memory, auto-compaction, session recall, and undo capabilities. Terminal integration supports full-screen UI, custom themes, UNIX shortcuts, mouse and keyboard navigation, and improved accessibility.
CLI installation is available through npm, Homebrew, WinGet, or shell scripts, while Dev Container setups provide advanced DevOps integration. Enterprises get model control, authentication choices (OAuth, GitHub CLI tokens), proxy compatibility, and compliance hooks. Documentation and onboarding resources walk through best practices for productivity and automation.
Demonstration articles show step-by-step installation, setting up agent workflows, using automated code reviews, and combining terminal and GitHub features with natural language commands. Best-practices guides show project scaffolding, test debugging, batch changes, and seamless CLI/IDE experiences. New enterprise telemetry supports tracking CLI usage, sessions, and tokens for broader organizational metrics. Video guides and documentation are available.
- [GitHub Copilot CLI Now Generally Available for Paid Subscribers](https://github.blog/changelog/2026-02-25-github-copilot-cli-is-now-generally-available)
- [Exploring GitHub Copilot CLI: Features and Live Demonstration](/github-copilot/videos/exploring-github-copilot-cli-features-and-live-demonstration)
- [From Idea to Pull Request: Practical GitHub Copilot CLI Workflow](https://github.blog/ai-and-ml/github-copilot/from-idea-to-pull-request-a-practical-guide-to-building-with-github-copilot-cli/)
- [Intro to GitHub Copilot CLI: Boosting Terminal Productivity](/github-copilot/videos/intro-to-github-copilot-cli-boosting-terminal-productivity)
- [Enterprise Metrics for GitHub Copilot CLI Now Available](https://github.blog/changelog/2026-02-27-copilot-usage-metrics-now-includes-enterprise-level-github-copilot-cli-activity)
## GitHub Copilot Chat: Upgrades, Web Search, and Pull Request Enhancements
The GPT-5.3-Codex model is now part of Copilot Chat on the web, GitHub Mobile, VS Code, and Visual Studio, enabling more responsive and accurate conversation for all paid users. Admins can restrict model access through policies, and developers have the ability to switch models in real time, supporting consistent workflows across devices and environments.
The new release expands on earlier model options, bringing the Codex experience to both web-based and IDE Copilot Chat.
Web search is now model-native in Copilot Chat for github.com, so users can pull up-to-date context directly in chat for supported AI models. Paid accounts can enable the feature for current documentation and real-world information, reducing external search time. Enterprises and personal users can choose to opt in.
Additionally, Copilot now generates pull request titles, auto-suggesting clear PR names based on commit messages. Teams are encouraged to group changes logically and provide descriptive commit messages to streamline code reviews.
- [GPT-5.3-Codex Now Available in GitHub Copilot Chat on Web and IDEs](https://github.blog/changelog/2026-02-25-gpt-5-3-codex-is-now-available-in-github-com-github-mobile-and-visual-studio)
- [Improved Web Search Capability in GitHub Copilot Chat](https://github.blog/changelog/2026-02-25-improved-web-search-in-copilot-on-github-com)
- [Generate Pull Request Titles Using GitHub Copilot on the Web](https://github.blog/changelog/2026-02-25-generate-pull-request-titles-with-copilot-on-the-web)
## Copilot Coding Agents: New Models, Parallel Agents, and Mobile Integration
Copilot now allows developers to select models and run agents in parallel, letting users choose suitable models for their coding tasks. The system includes automated self-review, security reviews (for code, secrets, and dependencies), and supports the creation of compliance-based agent tasks using ".github/agents/". The CLI enables flexible switching between cloud and local agent sessions.
This update builds on previous model expansion, giving developers more control over workflow execution. Parallel agents in VS Code make it possible to compare Copilot, Claude, and Codex results at once.
Claude and Codex agents are now also available to Copilot Business and Pro users on the web, mobile, and VS Code. Developers can assign agents to pull requests, mention them in comments, or use them directly within IDEs. Parallel execution helps teams compare multiple approaches or validate code quality before merging. GitHub Mobile users receive live notifications for agent status, pull request workflows, and completed tasks on both iOS and Android.
- [What’s New in GitHub Copilot Coding Agent: Model Selection, Self-Review, Security Scanning, and More](https://github.blog/ai-and-ml/github-copilot/whats-new-with-github-copilot-coding-agent/)
- [Claude and Codex Now Available as Coding Agents for Copilot Business and Pro Users](https://github.blog/changelog/2026-02-26-claude-and-codex-now-available-for-copilot-business-pro-users)
- [VS Code Live: Exploring Coding Agents and GitHub Copilot Integration](/github-copilot/videos/vs-code-live-exploring-coding-agents-and-github-copilot-integration)
- [GitHub Mobile: Real-Time Notifications for Copilot Coding Agents](https://github.blog/changelog/2026-02-26-github-mobile-track-coding-agent-progress-in-real-time-with-live-notifications)
## Copilot Usage Metrics APIs: Enterprise and Org-Level Enhancements
New metrics dashboards and APIs for Copilot adoption, code completion, and usage insight are now generally available. Teams can track output by language, IDE, and user group, making it easier to link AI contributions to engineering outcomes. This update enables custom reporting, helps with governance, and supports compliance.
Following last week''s organization-level dashboard preview, this public release provides feature parity for both enterprises and organizations. The updated APIs now report PR activity, merge times, Copilot engagement (authored/reviewed PRs, suggestion stats), and distinguishes between suggestion creation and application, including activity outside the IDE.
APIs now use updated domain endpoints, so organizations should review firewall access lists. New enterprise telemetry includes CLI tracking to support data-driven resourcing and analytics, continuing last week''s telemetry coverage.
- [GitHub Copilot Metrics Now Generally Available for Enterprise Usage Insights](https://github.blog/changelog/2026-02-27-copilot-metrics-is-now-generally-available)
- [Org-level Metrics API Adds Pull Request Throughput and Copilot Activity Metrics](https://github.blog/changelog/2026-02-25-org-level-metrics-api-now-includes-pull-request-throughput-metric-parity)
- [Copilot Metrics Report URLs Update](https://github.blog/changelog/2026-02-26-copilot-metrics-report-urls-update)
- [Enterprise Metrics for GitHub Copilot CLI Now Available](https://github.blog/changelog/2026-02-27-copilot-usage-metrics-now-includes-enterprise-level-github-copilot-cli-activity)
## Copilot Agentic Workflows and Reliable Multi-Agent Architectures
GitHub now supports agent-driven workflows for GitHub Actions, where developers can define CI/CD and automation tasks in markdown, executed by agents like Copilot, Claude, or Codex. The approach offers flexibility over standard YAML scripting and enables use of AI for more dynamic automation, supporting agent fallback and collaboration.
This week''s guides provide best practices for building robust multi-agent systems, covering typing schemas, action contracts, and MCP-based validation for workflow reliability. MCP remains central to coordination and policy enforcement. Documentation clarifies schema validation and common debugging scenarios, supporting long-term agent scalability.
- [GitHub Introduces Agentic Workflows: Integrating AI Agents with GitHub Actions](/github-copilot/videos/github-introduces-agentic-workflows-integrating-ai-agents-with-github-actions)
- [Engineering Reliable Multi-Agent Workflows: Patterns for Success](https://github.blog/ai-and-ml/generative-ai/multi-agent-workflows-often-fail-heres-how-to-engineer-ones-that-dont/)
## Developer Insights, Case Studies, and Copilot SDK Applications
Case studies and resources outline real Copilot adoption patterns in both startups and large organizations. The Octoverse 2025 report explores Copilot’s effect on multitasking and system maintenance, with best practices for security and process improvement. Measurement guides describe using DORA metrics and Apache DevLake to quantify improvements in delivery and recovery cycles.
These resources follow last week''s workflow improvement stories, including prompt engineering trends, WinForms modernization, and real-world Copilot applications. Example case studies show AI helping rebuild business systems after critical failure, demonstrating tools for agent creation and integration with SDKs—including Python AI tutors and Kubernetes sidecar designs for agent and skill server interaction.
- [Tim Rogers on the Future of GitHub Copilot and AI Agents: Octoverse 2025 Insights](/github-copilot/videos/tim-rogers-on-the-future-of-github-copilot-and-ai-agents-octoverse-2025-insights)
- [Measuring Actual AI Impact for Engineering with Apache DevLake and GitHub Copilot](https://devblogs.microsoft.com/all-things-azure/measuring-actual-ai-impact-for-engineering-with-apache-devlake/)
- [How AI-Driven WinForms Development Saved a Business in Crisis](https://devblogs.microsoft.com/dotnet/the-dongle-died-at-midnight/)
- [How I built an AI Python tutor with the GitHub Copilot SDK](/github-copilot/videos/how-i-built-an-ai-python-tutor-with-the-github-copilot-sdk)
- [Building a Dual Sidecar Pod: Integrating GitHub Copilot SDK and Skill Server on Kubernetes](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-dual-sidecar-pod-combining-github-copilot-sdk-with/ba-p/4497080)
## Other GitHub Copilot News
Enterprise AI Controls and the Agent Control Plane are now available for managing Copilot and custom agents at scale. Features include role-based access, detailed logs, enforceable policy, versioned agent standards, and configuration APIs. Improved UIs and registry previews enhance governance for larger deployments.
These features extend work from last week, providing tools for secure, centrally managed agent ecosystems.
- [Enterprise AI Controls and Agent Control Plane Now Generally Available](https://github.blog/changelog/2026-02-26-enterprise-ai-controls-agent-control-plane-now-generally-available)
The Copilot Content Exclusion REST API enters public preview, giving admins tools to automate content exclusion for compliance and data protection.
- [Copilot Content Exclusion REST API in Public Preview](https://github.blog/changelog/2026-02-26-copilot-content-exclusion-rest-api-in-public-preview)
Visual Studio’s February 2026 update delivers expanded Copilot integration for C# and C++, test generation, slash command customization, and support for AI-powered debugging directly in the editor. This continues ongoing improvements for IDE productivity and legacy code upgrades.
- [Visual Studio February 2026 Update: AI Assistance, GitHub Copilot, and Developer Productivity](https://devblogs.microsoft.com/visualstudio/visual-studio-february-update/)
Copilot’s Next Edit Suggestions for VS Code adds support for complex multi-location changes with a multi-model pipeline and RLVR. The new diff preview widget lets users review refactorings before applying changes, and upcoming changes will add cross-file and unified modeling.
- [Building Long-Distance Next Edit Suggestions in GitHub Copilot](https://code.visualstudio.com/blogs/2026/02/26/long-distance-nes)
VS Code 1.110 improves Copilot integration, actionable AI suggestions, UI workflows, and delivers product walkthroughs and live discussions on the latest changes.
- [VS Code v1.110 Release: Highlights and GitHub Copilot Updates](/github-copilot/videos/vs-code-v1110-release-highlights-and-github-copilot-updates)',
    'This section highlights current updates for GitHub Copilot, including extended agent-based workflows, CLI automation, model selection, and improved analytics for enterprise users. The platform introduces the Copilot CLI with public availability, enhanced chat models, richer metrics APIs, and deeper integration for both developers and organizational teams. New guides break down recent architectural changes and provide directions for automation, governance, and productivity improvements.',
    1772438400, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-03-02', 'TechHub',
    'TechHub', 'E0D8C4EFD77F9D606D06AF08121591FC2ECAF976075065EA2AA14A689ECF8092', ',GitHub Copilot,Copilot CLI,Copilot Chat,Coding Agents,Agentic Workflows,GitHub Actions,Model Selection,GPT 5.3 Codex,Claude,Gemini,MCP,Enterprise Metrics API,Governance And Compliance,VS,VS Code,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-02-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-02-23', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, MCP, and Model Choice',
    'GitHub Copilot continues to expand its feature set, integrations, and workflows. New agent workflows, model support, and interactive capabilities are providing more ways for developers to automate coding, documentation, and review. The range of updates covers cloud-based agent automation, deeper editor integrations, custom workflow flexibility, enhanced model selection, and new security features—focused on creating a more responsive and productive development environment.
<!--excerpt_end-->
## Copilot Coding Agents: Autonomous Workflows and Expanded Integration
Copilot Coding Agents now support more environments, allowing developers to delegate tasks from Visual Studio, VS Code, GitHub, the CLI, and Raycast. Visual Studio 2026 and newer versions support sending Copilot Chat work to agents for asynchronous execution, improving agent-driven pull request creation and notifications that were part of recent IDE and Actions updates. Raycast-based assignment and monitoring add support for cross-platform task delegation, complementing last week’s improvements to GitHub and VS Code workflows. Business and Enterprise users get advanced model picker options for Claude, GPT-5 Codex, and additional model choices. These updates reflect a continued shift toward enhanced, flexible, and secure agent-powered coding.
- [Running Code Generation in the Background with GitHub Copilot Coding Agents](https://github.blog/changelog/2026-02-18-Running-Code-Generation-in-the-Background-with-GitHub-Copilot-Coding-Agents.html)
- [Delegate Tasks to GitHub Copilot Coding Agent from Visual Studio](https://github.blog/changelog/2026-02-17-delegate-tasks-to-copilot-coding-agent-from-visual-studio)
- [Assigning Issues to GitHub Copilot Coding Agent from Raycast](https://github.blog/changelog/2026-02-17-assign-issues-to-copilot-coding-agent-from-raycast)
- [Copilot Coding Agent Supports Code Referencing](https://github.blog/changelog/2026-02-18-copilot-coding-agent-supports-code-referencing)
- [Copilot Coding Agent Model Picker for Business and Enterprise Users](https://github.blog/changelog/2026-02-19-model-picker-for-copilot-coding-agent-for-copilot-business-and-enterprise-users)
- [How to Use Copilot Coding Agent with Windows Projects](https://github.blog/changelog/2026-02-18-use-copilot-coding-agent-with-windows-projects)
## Model Context Protocol (MCP) and Interactive Agent Experiences
Model Context Protocol (MCP) enhancements offer better agent coordination and extensibility for VS Code and open agent frameworks. Visuals MCP brings interactive UI components into open source, allowing agents to display dashboards, tables, and previews during development tasks. The MCP Registry for Eclipse unifies agent toolchains, while updated preference and task management options let users personalize agent interactions. By supporting markdown-based agents on Azure Functions with Copilot SDK and AGENTS.md integration, cloud distribution and event-driven agent automation become easier.
- [Building Interactive UI Components for AI Agents with Visuals MCP](https://harrybin.de/posts/visuals-mcp-server/)
- [Introducing MCP Apps: Interactive UI Components in VS Code Chat](/ai/videos/introducing-mcp-apps-interactive-ui-components-in-vs-code-chat)
- [MCP Registry and New GitHub Copilot Features in Eclipse](https://github.blog/changelog/2026-02-17-mcp-registry-and-more-improvements-in-copilot-in-eclipse)
- [Hosting Declarative Markdown-Based Agents on Azure Functions with GitHub Copilot SDK](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-declarative-markdown-based-agents-on-azure-functions/ba-p/4496038)
## Expanded AI Model Support: Claude, Gemini, and BYOK
GitHub Copilot now supports more models. Claude Opus 4.6 and Claude Sonnet 4.6 are generally available, while Gemini 3.1 Pro is offered in public preview. The BYOK (Bring Your Own Key) feature in VS Code allows teams to use their own API keys or self-hosted and open source models. These changes reflect ongoing attention to flexible model selection. Some older Anthropic and OpenAI models are being deprecated in line with the focus on performance and updated support.
- [Claude Sonnet 4.6 Now Available in GitHub Copilot](https://github.blog/changelog/2026-02-17-claude-sonnet-4-6-is-now-generally-available-in-github-copilot)
- [Claude Opus 4.6 Integration with GitHub Copilot Available in Popular IDEs](https://github.blog/changelog/2026-02-18-claude-opus-4-6-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Gemini 3.1 Pro Public Preview in GitHub Copilot](https://github.blog/changelog/2026-02-19-gemini-3-1-pro-is-now-in-public-preview-in-github-copilot)
- [Bring Your Own AI Models to Visual Studio Code](/github-copilot/videos/bring-your-own-ai-models-to-visual-studio-code)
- [GitHub Copilot Deprecates Selected Anthropic and OpenAI Models](https://github.blog/changelog/2026-02-19-selected-anthropic-and-openai-models-are-now-deprecated)
## Custom Agents and Unified Agent Management
With Visual Studio support for both built-in and custom agents, developers now have access to new debugging and modernization workflows, matching recent rollouts of agent skills and .agent.md standards for JetBrains and VS Code. Custom agent roles and project context tools encourage modular and team-based automation. VS Code brings together management for local, cloud, and partner agents, building toward the vision of unified agent handoff and coordination—sometimes called “Agent HQ.”
- [Custom Agents in Visual Studio: Built-in and Custom Workflows with GitHub Copilot](https://devblogs.microsoft.com/visualstudio/custom-agents-in-visual-studio-built-in-and-build-your-own-agents/)
- [A Unified Agent Experience in Visual Studio Code](/a-unified-agent-experience-in-visual-studio-code)
- [Customize GitHub Copilot Agents in Visual Studio Code](/github-copilot/videos/customize-github-copilot-agents-in-visual-studio-code)
## Agentic Workflows, Continuous AI, and Automation Enhancements
GitHub’s preview of agent-driven repository automation builds on recent markdown/YAML-based automation and Actions improvements. Copilot, OpenAI agents, and secure container features offer new options for fine-grained controls, making complex bot orchestration possible. Agent HQ is moving toward improved security, fleet management, and workflow reproducibility, supporting safer automation for non-critical pull requests while reproducibility practices develop further.
- [GitHub Previews Agentic Workflows as Part of Continuous AI Concept](https://www.devclass.com/ci-cd/2026/02/17/github-previews-agentic-workflows-as-part-of-continuous-ai-concept/4091356)
- [The Download: Agentic Workflows, New AI Models, and GitHub Updates](/github-copilot/videos/the-download-agentic-workflows-new-ai-models-and-github-updates)
## Copilot Integration: VS Code, Zed, and SQL Tools
Copilot now works in the Zed Editor—offering chat, completions, and code suggestions in more places. VS Code adds CLI integration and session tracking to strengthen its feature set. SQL toolkit improvements provide Copilot-powered support inside SSMS, VS Code MSSQL, and Microsoft Fabric Query Editor, simplifying query generation and automation for teams moving from Azure Data Studio.
- [GitHub Copilot Now Supported in Zed Editor](https://github.blog/changelog/2026-02-19-github-copilot-support-in-zed-generally-available)
- [Integrating Copilot CLI with Visual Studio Code](/github-copilot/videos/integrating-copilot-cli-with-visual-studio-code)
- [Let it Cook: Latest Updates in VS Code and GitHub Copilot](/github-copilot/videos/let-it-cook-latest-updates-in-vs-code-and-github-copilot)
- [AI-Powered Assistants in SSMS, VS Code, and Fabric: GitHub Copilot for SQL Developers](https://blog.fabric.microsoft.com/en-US/blog/no-more-excuses-ai-powered-assistants-are-in-ssms-vs-code-and-fabric/)
## Developer Workflow Optimization: App Modernization and Prompts
New Copilot migration guides provide support for legacy .NET upgrades—covering containerization, managed identities, and app restructuring. These guides are based on feedback about common .NET and Azure migration scenarios and highlight the practical impact of Copilot automation. The prompts.chat resource offers growing collections of reusable prompt templates to help teams accelerate processes by sharing best practices.
- [From "Maybe Next Quarter" to "Running Before Lunch" on Container Apps - Modernizing Legacy .NET App](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-quot-maybe-next-quarter-quot-to-quot-running-before-lunch/ba-p/4495736)
- [Open Source Friday: Practical Prompt Patterns with prompts.chat](/github-copilot/videos/open-source-friday-practical-prompt-patterns-with-promptschat)
## Copilot Usage Metrics and Analytics
Copilot provides organization-wide dashboards and APIs for real-time monitoring of usage and developer productivity. These tools help teams meet compliance policies and give engineering leads better data for analyzing PR cycle times, review speed, and DevOps performance. The new metrics go beyond past status reports or outage analytics, giving actionable insights for daily process improvement.
- [Organization-Level GitHub Copilot Usage Metrics Dashboard Public Preview](https://github.blog/changelog/2026-02-20-organization-level-copilot-usage-metrics-dashboard-available-in-public-preview)
- [Pull Request Throughput and Time to Merge in GitHub Copilot Usage Metrics API](https://github.blog/changelog/2026-02-19-pull-request-throughput-and-time-to-merge-available-in-copilot-usage-metrics-api)
## Developer Trends and Security Tools
Octoverse data highlights how Copilot affects the technologies and languages developers prefer, including growth in TypeScript and more adoption of strongly-typed AI integrations. The GitHub Secure Open Source Fund continues to drive improved security, including autofix, scanning, and Copilot-based workflows for open source projects—maintaining last week’s emphasis on audit logs and automated governance.
- [How AI is Reshaping Developer Choice: Insights from Octoverse 2025](https://github.blog/ai-and-ml/generative-ai/how-ai-is-reshaping-developer-choice-and-octoverse-data-proves-it/)
- [Open Source Friday: GitHub Secure Open Source Fund and the Future of Supply Chain Security](/github-copilot/videos/open-source-friday-github-secure-open-source-fund-and-the-future-of-supply-chain-security)
## Other GitHub Copilot News
Visual Studio 2026 introduces built-in support for Mermaid diagramming, integrated with Copilot to help plan architectures and document workflows more easily. These features add to past architectural tooling in the IDE, with stronger Copilot connectivity. Additional updates show steady progress in agent interoperability and model partnerships, moving toward a consistent agentic ecosystem.
- [Visualizing Workflows and Architecture with Mermaid Charts in Visual Studio 2026](https://techcommunity.microsoft.com/t5/tools/visualize-workflows-and-architecture-with-mermaid-charts-in/m-p/4495253#M190)
- [Leveraging Claude Opus 4.6 and Agents in GitHub Copilot for Advanced Coding Tasks](https://techcommunity.microsoft.com/t5/apps-on-azure/using-claude-opus-4-6-in-github-copilot/m/p/4495127#M1393)',
    'GitHub Copilot continues to expand its feature set, integrations, and workflows. New agent workflows, model support, and interactive capabilities are providing more ways for developers to automate coding, documentation, and review. The range of updates covers cloud-based agent automation, deeper editor integrations, custom workflow flexibility, enhanced model selection, and new security features—focused on creating a more responsive and productive development environment.',
    1771833600, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-02-23', 'TechHub',
    'TechHub', 'E5E0215ECCE84D5A1BB07027A49034CCD4361F29779085BB49B29A19D0F551C8', ',GitHub Copilot,Copilot Coding Agent,Agentic Workflows,MCP,VS Code,Visual Studio 2026,GitHub Actions,Claude Opus 4.6,Claude Sonnet 4.6,Gemini 3.1 Pro,BYOK,Raycast,Eclipse,Copilot Usage Metrics,Software Supply Chain Security,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-02-16
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-02-16', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, Models, and Automation',
    'This week’s Copilot release centers on new developer assistant features and Copilot’s evolution as a platform. The focus is on more flexible agent workflows, expanded automation, and productivity controls, alongside easier model selection and improved support for teams throughout the development lifecycle.
<!--excerpt_end-->
## GitHub Agentic Workflows: AI-Powered Automation Within GitHub Actions
Agentic Workflows are now available as an intent-based way to automate tasks on GitHub. Instead of managing manual scripts and event triggers, developers describe their objectives in natural language or Markdown, and AI agents such as Copilot, Claude, and Codex implement tasks while respecting security and compliance policies. There’s a technical overview showing setup info and how workflows tie in with GitHub Actions pipelines for safe automation, from PR reviews to CI fixes.
This draws from last week’s work on agent collaboration and VS Code automation—now extending those features to pipelines and repo-level management. Agent coordination, previously in IDE tools and Agent HQ, is now supporting automation tasks in Actions too. With Copilot, Claude, and Codex available together, the workflow moves toward declarative repo automation instead of script-heavy processes.
Another new capability is the open-source `gh aw` CLI extension, which lets you create Markdown-based workflows—without learning YAML. Debugging tools are available in VS Code, on the web, and in the CLI. Security is a core part of the design: all actions are sandboxed, read-only by default, and explicit writes are audited. Over 50 workflow templates help teams get started fast. Community cooperation among GitHub Next, Microsoft Research, and Azure specialists is driving the open-source roadmap. Early users are invited to provide input.
- [Introducing GitHub Agentic Workflows: Intent-Driven Repository Automation](/introducing-github-agentic-workflows-intent-driven-repository-automation)
- [GitHub Agentic Workflows: Automate Repository Tasks with AI Agents](https://github.blog/changelog/2026-02-13-github-agentic-workflows-are-now-in-technical-preview)
- [Automate Repository Tasks with GitHub Agentic Workflows](https://github.blog/ai-and-ml/automate-repository-tasks-with-github-agentic-workflows/)
## Expanded Model Selection and AI Integration Across Copilot Platforms
Model selection is now built into core Copilot workflows. New guides explain Copilot model choices—like GPT‑4.1, GPT‑5, Claude Sonnet, Haiku, Codex, and more—offering details on task suitability, cost, resource management, and enterprise policies. An “Auto” mode lets Copilot select a model automatically. Model selection is now on mobile: GitHub Mobile introduces a model picker for Pro users, including Anthropic and Codex models, on iOS and Android.
Last week Copilot added speed and more choices with Claude Opus 4.6 and faster selection, and those controls are now unified across web, IDEs, and mobile. Ongoing infrastructure updates for enterprise and self-hosted runners reflect adjustments to Copilot’s security and network patterns.
- [Choosing the Right Model in GitHub Copilot: A Practical Guide for Developers](https://techcommunity.microsoft.com/t5/microsoft-developer-community/choosing-the-right-model-in-github-copilot-a-practical-guide-for/ba-p/4491623)
- [Network Configuration Updates for Copilot Coding Agent with GitHub Actions and Azure](https://github.blog/changelog/2026-02-13-network-configuration-changes-for-copilot-coding-agent)
- [GitHub Mobile Adds Model Picker for Copilot Coding Agent](https://github.blog/changelog/2026-02-11-github-mobile-model-picker-for-copilot-coding-agent)
## New Features and Productivity Tools in Visual Studio, JetBrains, and Copilot Ecosystem
Visual Studio’s Copilot integration gets a new find_symbol tool in Chat Agent Mode—supporting in-depth symbol navigation, refactoring, and code review across C++, C#, Razor, TypeScript, and LSP-based languages. The tool offers smarter search and safer refactorings, and expands for user testing via the Insiders program.
As noted last week, Copilot’s unit test generation for .NET has advanced and is now generally available in Visual Studio 2026. Copilot automates test writing, coverage checks, and validation, building on pilot customer feedback.
For JetBrains IDE users, the Copilot plugin now has Agent Skills public preview, better onboarding, and inline chat improvements. Projects built with IntelliJ IDEA, PyCharm, or WebStorm gain improved automation, better UI, and new controls for using agent skills within team environments.
- [Unlock Language-Specific Rich Symbol Context with the New find_symbol Tool in Visual Studio Copilot Chat](https://devblogs.microsoft.com/visualstudio/unlock-language-specific-rich-symbol-context-using-new-find_symbol-tool/)
- [GitHub Copilot Testing for .NET: AI-Powered Unit Tests in Visual Studio 2026](https://devblogs.microsoft.com/dotnet/github-copilot-testing-for-dotnet-available-in-visual-studio/)
- [New Features and Improvements in GitHub Copilot for JetBrains IDEs](https://github.blog/changelog/2026-02-13-new-features-and-improvements-in-github-copilot-in-jetbrains-ides-2)
- [Visualize Workflows and Architecture with Mermaid Charts in Visual Studio 2026](https://dellenny.com/visualize-workflows-and-architecture-with-mermaid-charts-in-visual-studio-2026/)
## Copilot Models and Agent Framework Updates
OpenAI GPT-5.3-Codex is now generally available in Copilot Pro, Pro+, Business, and Enterprise plans, providing improved code generation, benchmarks, and better support for complex tasks. Business and Enterprise users can enable access through their organization, with support in VS Code, CLI, web, and mobile. The update supports more advanced automation, especially for larger teams and those with custom infrastructure.
Following last week’s Fast Mode rollout for Claude Opus 4.6, GPT-5.3-Codex continues to expand model and automation choices. There’s ongoing work in VS Code around “subagents,” which can now run in parallel and keep context separate, building on last week’s updates around multi-agent reviews and more sophisticated automation.
- [GPT-5.3-Codex Now Available in GitHub Copilot](https://github.blog/changelog/2026-02-09-gpt-5-3-codex-is-now-generally-available-for-github-copilot)
- [Subagents in VS Code: Parallel Execution and Context Isolation Explained](/subagents-in-vs-code-parallel-execution-and-context-isolation-explained)
## AI-Powered Workflows in Practice: Modernization, Case Studies, and Community Upskilling
Microsoft showcases Copilot, Azure Migrate, and Azure Copilot as tools that remove legacy blockers for moving and modernizing applications, complementing last week’s focus on .NET upgrade assistance and agent-driven modernization paths.
A featured case study explores building a university clinic web app for less than $10 using Copilot Pro, Azure, and VS Code. The coverage emphasizes technical feasibility, cost control, secure builds, and how Copilot-generated scripts streamline automation.
Agents League has launched a two-week AI agent development challenge using Copilot SDK, Foundry, and Copilot Studio. Events include coding battles, templates, and badges, following the recently expanded community event schedule. This effort is designed to grow best practices and enable more developers to contribute agent workflows. GitHub’s partnership with Andela highlights Copilot certification and structured training as a way for global tech talent to learn and compete.
- [Modernizing for the AI Era: Accelerating Application Transformation with Agentic Tools](https://techcommunity.microsoft.com/t5/azure-migration-and/modernizing-for-the-ai-era-accelerating-application/ba-p/4490596)
- [Building a Professional Clinic Web App with GitHub Copilot and Azure SQL for Under $10](/building-a-professional-clinic-web-app-with-github-copilot-and-azure-sql-for-under-10)
- [Agents League: Build AI Agents with Microsoft Tools in a Two-Week Challenge](https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-two-weeks-three-tracks-one-challenge/ba-p/4492102)
- [How Global Tech Talent Is Advancing with GitHub Copilot](/how-global-tech-talent-is-advancing-with-github-copilot)
## Other GitHub Copilot News
Operational reliability remains a priority for the Copilot team. The January Availability Report outlines recent Copilot incidents, root causes, and fixes. Status Page improvements add a 90-day incident record, detailed outage information, and better regional view. New CI/CD reports now clearly list the impact to runners and models, providing more transparency.
These observations carry over from last week’s emphasis on improved workflow validation, governance practices, and enhanced incident insight for both enterprises and open source Copilot teams.
- [GitHub Availability Report: January 2026](https://github.blog/news-insights/company-news/github-availability-report-january-2026/)
- [Improved GitHub Status Page with Enhanced Incident Visibility](https://github.blog/changelog/2026-02-13-updated-status-experience)',
    'This week’s Copilot release centers on new developer assistant features and Copilot’s evolution as a platform. The focus is on more flexible agent workflows, expanded automation, and productivity controls, alongside easier model selection and improved support for teams throughout the development lifecycle.',
    1771228800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-02-16', 'TechHub',
    'TechHub', '9F04181268D2A0E8C692442AF1BA2E42FC865186D04DC66430F02362F72C5570', ',GitHub Copilot,Agentic Workflows,GitHub Actions,Gh CLI,Markdown Workflows,Model Selection,GPT 5.3 Codex,Claude Sonnet,Anthropic,Codex,Visual Studio 2026,JetBrains IDEs,VS Code Subagents,Enterprise Security,Reliability And Status Reporting,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-02-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-02-09', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, Governance, and SDKs',
    'This week, GitHub Copilot added enhanced multi-agent coordination, a unified management environment, new models, and additional developer controls with the Copilot SDK. These changes enable more robust workflows, faster feedback, and customizable AI support, giving organizations improved governance and an easier path to extend Copilot.
<!--excerpt_end-->
## Multi-Agent Development and Integration in VS Code
Visual Studio Code v1.109 now supports routine multi-agent work, letting users manage Copilot, Claude, and Codex agents together. The Agent Sessions view brings all agent management into one place with simple session switching and monitoring. Codex runs locally via extension, while Claude can run either locally or in the cloud for Copilot Pro+ subscribers. The platform adopts open standards (MCP Apps, Anthropic Agent Skills) to let agents use skills and extend their features. New agent protocol and organizational views improve monitoring and assignment. Developers can run subagents in parallel for specialized reviews, security tasks, or subject matter expertise, supporting team workflows. Ongoing documentation and feedback threads help teams create collaborative environments and maintain clear, efficient, AI-organized workflows in VS Code.
- [VS Code 1.109: Multi-Agent Development with Copilot, Claude, and Codex](https://code.visualstudio.com/blogs/2026/02/05/multi-agent-development)
## The Expanding Copilot Ecosystem: Agent HQ, SDK, and Governance
GitHub improves AI tool coordination using Agent HQ, improvements to the Copilot CLI, the Copilot SDK, and Agent Mode. Agent HQ works as a central interface for organizations to set up, manage, and track agents like Claude and Codex both in GitHub and VS Code. Enhancing repository-based controls, Agent HQ offers organization-wide governance including branch controls and change logs, making automation safer and more adaptable. Copilot CLI''s Plan Mode audits terminal commands, and Agentic Memory helps capture policies and standards. The Copilot SDK lets engineers use Copilot with their Node.js or Python tools and enforces compliance and secure access. PR times are reduced and reliability is improved, giving teams flexibility and protection for AI adoption.
- [How GitHub Bridges the Fragmented AI Development Landscape](https://devblogs.microsoft.com/all-things-azure/the-os-for-intelligence-how-github-bridges-the-fragmented-ai-landscape/)
## GitHub Copilot SDK: Cloud-Native Agents, Community Projects, and Demos
New step-by-step guides and demos show developers how to use the Copilot SDK for smart agent applications. These practical resources, expanding on last week’s hybrid app patterns, show how to blend the Copilot SDK, Agent-to-Agent Protocol, and Azure Container Apps to develop and securely deploy agent systems in the cloud. Tutorials explain skill files, agent lookup, security, and scaling. Projects include educational apps like “Flight School” and showcase top community work in automation or browser agents. The SDK enables quick app creation, GitHub API use, and workflow automation, with sample code and deployment steps encouraging developers to try cloud agent system design.
- [The Perfect Fusion of GitHub Copilot SDK, Agent Protocol, and Cloud Native Deployment](https://techcommunity.microsoft.com/t5/microsoft-developer-community/the-perfect-fusion-of-github-copilot-sdk-and-cloud-native/ba-p/4491199)
- [GitHub Copilot SDK Demo: Building ''Flight School'' Personalized Coding Challenges](/github-copilot-sdk-demo-building-flight-school-personalized-coding-challenges)
- [Top 10 Community Projects Built with GitHub Copilot SDK](/top-10-community-projects-built-with-github-copilot-sdk)
## Multi-Agent Strategies: Claude, Codex, and GitHub Copilot in Practice
Copilot Pro+ and Enterprise subscriptions now provide access to Claude and Codex agents in Agent HQ for web, mobile, and VS Code. Extending new org-level features, teams can use several agents for code reviews, assign reviewers, and automate parts of the review cycle. Dashboards help manage reviewer assignments, see access levels, and review audit logs. Tutorials show how to use multiple agents together by handling sessions and artifacts that affect pull requests or issues. Planned support for agents from Google, Cognition, and xAI will offer expanded choice in a central location. The documentation covers best practices and setup.
- [Using Claude and Codex AI Agents in GitHub Agent HQ](https://github.blog/news-insights/company-news/pick-your-agent-use-claude-and-codex-on-agent-hq/)
- [Claude and Codex Agents Now Available for GitHub Copilot Pro+ and Enterprise](https://github.blog/changelog/2026-02-04-claude-and-codex-are-now-available-in-public-preview-on-github)
- [How to Use Claude, Codex, and GitHub Copilot Together in GitHub and VS Code](/how-to-use-claude-codex-and-github-copilot-together-in-github-and-vs-code)
## Advances in Copilot Model Management and AI Model Options
GitHub now enables newer Copilot models by default to reduce friction. Claude Opus 4.6 is rolling out to all Pro+, Pro, and Free users (manual selection remains possible). This model provides advanced planning abilities and tool integration. Fast Mode for Claude Opus 4.6 can speed up output by up to 2.5x for Enterprise users, when configured by admins. Last week''s changes for easier model access and command-line switching now extend to web and IDEs. Documentation makes model selection clear and accessible.
- [Simplified Copilot Model Enablement for GitHub Users](https://github.blog/changelog/2026-02-03-simplified-copilot-model-enablement-experience-for-individual-users)
- [Claude Opus 4.6 Now Available in GitHub Copilot](https://github.blog/changelog/2026-02-05-claude-opus-4-6-is-now-generally-available-for-github-copilot)
- [Fast Mode for Claude Opus 4.6 Rolls Out in GitHub Copilot Preview](https://github.blog/changelog/2026-02-06-claude-opus-4-6-fast-is-now-in-public-preview-for-github-copilot)
- [Fast Mode for Claude Opus 4.6 Now Available in GitHub Copilot Preview](https://github.blog/changelog/2026-02-07-claude-opus-4-6-fast-is-now-in-public-preview-for-github-copilot)
## Copilot Experience Updates in Visual Studio, VS Code, and the Web
Copilot for Visual Studio and VS Code received various updates. Visual Studio 2026 (January) brings colorized completions and an option to accept part of an AI suggestion, leading to clearer, more integrated results. This builds on recent GPT-5.2-Codex features and context-based suggestions. Editor updates refine scrolling, selection, and markdown experiences, including support for Mermaid and interactive previews. VS Code’s January update improves agent tasking, multi-agent collaboration, token chat, and introduces deeper session controls, enhanced terminal safety, and in-editor browsers for testing. Copilot for the web now lists tool actions, provides export (JSON/Markdown), and attaches chats to repositories—making browser usage clearer and more manageable.
- [Roadmap for AI and GitHub Copilot in Visual Studio: February Update](https://devblogs.microsoft.com/visualstudio/roadmap-for-ai-in-visual-studio-february/)
- [GitHub Copilot in Visual Studio Code v1.109: Major Agent-Driven Improvements (January 2026 Release)](https://github.blog/changelog/2026-02-04-github-copilot-in-visual-studio-code-v1-109-january-release)
- [GitHub Copilot in Visual Studio 2026 — January Update](https://github.blog/changelog/2026-02-04-github-copilot-in-visual-studio-january-update)
- [Enhancements to GitHub Copilot Chat on the Web: Tool Calls, Exports, and More](https://github.blog/changelog/2026-02-04-showing-tool-calls-and-other-improvements-to-copilot-chat-on-the-web)
## Custom Agents, Prompts, and Quality Engineering
Teams using Copilot with Azure DevOps can now design custom Azure Boards agents (.agent.md) for automating and standardizing pull request handling, helping review quality and consistency. These changes extend earlier support for logic app/data map profiles and deployment walkthroughs. Prompt engineering guides focused on Copilot in QA/staging highlight usable roles, prompt examples, and anti-patterns, making it easier to automate testing, regression checks, and coverage improvements. Teams can use prompt context providers (such as MCP) to adapt Copilot for specialized or routine work.
- [Azure Boards Now Supports Custom Agents in GitHub Copilot Integration](https://devblogs.microsoft.com/devops/azure-boards-integration-with-github-copilot-includes-custom-agent-support/)
- [Writing Effective Prompts for Testing Scenarios: AI-Assisted Quality Engineering](https://techcommunity.microsoft.com/t5/microsoft-developer-community/writing-effective-prompts-for-testing-scenarios-ai-assisted/ba-p/4488001)
## Practical Tutorials: CLI, Agents, and Documentation Context
Recent tutorials emphasize hands-on Copilot use, including updated CLI automation and model management. A video walkthrough describes how Copilot CLI’s ''/share'' command sends logs and diagrams to gists, making collaboration easier—especially with agents like Claude Opus and Next.js diagrams. Another guide covers custom agent creation through the CLI with details on setup, using the MCP server, and automating tasks. Scott Hanselman’s workflow shows how MCP feeds live documentation into Copilot for richer, up-to-date suggestions and visualizations. These examples help developers fine-tune Copilot for their specific needs.
- [How to Use the /share Command in GitHub Copilot CLI](/how-to-use-the-share-command-in-github-copilot-cli)
- [Getting Started with GitHub Copilot CLI and Custom Agents](/getting-started-with-github-copilot-cli-and-custom-agents)
- [Configuring Model Context Protocol in the GitHub Copilot CLI](/configuring-model-context-protocol-in-the-github-copilot-cli)
## Deep Agentic Workflows: Skills, Integration, and Observability
Agent Skills (SKILL.md files) let Copilot repeat standardized actions across environments. VS Code is piloting support for loading and managing these skill modules, helping developers reuse scripts vetted for reliability and security. This expands on last week’s discussions of agent skill architectures and new MCP links. MCP can now forward Azure App Service logs to Copilot, letting users troubleshoot via plain language, saving DevOps time. With end-to-end pipelines, full automation now spans requirements, coding, pull requests, CI/CD, and incident handling—demonstrated in new workflow analyses.
- [Integrating Agent Skills with GitHub Copilot and Visual Studio Code](https://techcommunity.microsoft.com/t5/apps-on-azure/integrate-agents-with-skills-in-github-copilot/m-p/4492020#M1391)
- [Chat with Your App Service Logs Using GitHub Copilot](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/chat-with-your-app-service-logs-using-github-copilot/ba-p/4491573)
- [An AI-Led SDLC: Building an Agentic E2E Software Lifecycle with Azure and GitHub](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/an-ai-led-sdlc-building-an-end-to-end-agentic-software/ba-p/4491896)
## Real-World Use Cases: Modernization, Kafka, and Community Coding
Modernizing older .NET applications is more straightforward, thanks to a Copilot Visual Studio agent that walks through code evaluation, migration planning, and execution with analysis and dependency tracking. This connects with earlier coverage on code migration and SQL changes. Developing with Kafka is now easier in VS Code with the Confluent extension, Copilot, and MCP, giving smart code hints and project bootstrapping. Community competitions like Agents League and Battle encourage creative agent development and group interaction, positioning Copilot as a tool for both productivity and collaborative coding.
- [Modernizing Legacy .NET Apps with GitHub Copilot: Step-by-Step Upgrade](/modernizing-legacy-net-apps-with-github-copilot-step-by-step-upgrade)
- [AI-Powered Kafka Development with Confluent VS Code Extension, GitHub Copilot, and MCP](/ai-powered-kafka-development-with-confluent-vs-code-extension-github-copilot-and-mcp)
- [Agents League: 2-Week AI Developer Challenge Featuring GitHub Copilot and Microsoft Foundry](/agents-league-2-week-ai-developer-challenge-featuring-github-copilot-and-microsoft-foundry)
- [Agents League Battle 1: Building Creative AI Apps with GitHub Copilot](/agents-league-battle-1-building-creative-ai-apps-with-github-copilot)
## Other GitHub Copilot News
GitHub Copilot supports open source growth and ongoing integration. In a live session, Martin Woodward discussed creative Copilot uses—like orchestrating a Furby music hack—and GitHub''s open Agent HQ plans. New tools such as ''actions-semver-checker'' use Copilot Agents and Claude for improved workflow validation, helping maintainers automate version checks and release tasks. Copilot''s use is broadening from simple suggestions to more automated and community-aware processes.
- [Open Source Friday: Martin Woodward on GitHub Copilot, Furby Hack, and Agent HQ](/open-source-friday-martin-woodward-on-github-copilot-furby-hack-and-agent-hq)
- [Improved Versioning and Release Automation for GitHub Actions Maintainers](https://jessehouwing.net/github-actions-automatic-versioning-for-actions/)',
    'This week, GitHub Copilot added enhanced multi-agent coordination, a unified management environment, new models, and additional developer controls with the Copilot SDK. These changes enable more robust workflows, faster feedback, and customizable AI support, giving organizations improved governance and an easier path to extend Copilot.',
    1770624000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-02-09', 'TechHub',
    'TechHub', '87ECA815A9E3095DE0EA5D368A05B53B90A95660B9C09CF53337A9C248636437', ',GitHub Copilot,Agent HQ,Copilot SDK,Multi Agent Workflows,VS Code,VS,Copilot CLI,Model Selection,Claude Opus 4.6,Codex,MCP,Agent To Agent Protocol,Azure Container Apps,Azure DevOps,Agent Skills,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-02-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-02-02', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, CLI Automation, SDKs',
    'GitHub Copilot continues to grow as an AI-powered development platform by adding automation options, agent-based workflows, and support for enterprise integrations. This week, the platform introduced additional SDKs, updates to the CLI, new agent features, and targeted resources for developers and architects. Key releases include expanded support for the GPT-5.2-Codex model, agent protocol integration in the CLI, improvements in SDK orchestration, and new resources for validating Copilot-powered tools at scale. Building on last week''s coverage of agentic workflows, Copilot is now more tightly integrated across developer tools and automation workflows.
<!--excerpt_end-->
## Copilot Model and IDE Integration Updates
GPT-5.2-Codex is now live in Copilot for multiple IDEs, including Visual Studio, JetBrains, Xcode, Eclipse, VS Code, GitHub.com, and GitHub Mobile. This change adds more AI coding options, builds on last week''s CLI model selection features, and enables subscribers across all business tiers to use a chat-based model picker for agent, ask, and edit modes. Make sure you meet the minimum version requirements, and check your Copilot settings for rollouts. The updates improve code review, conversational interactions, and AI-generated code in multi-language, multi-project environments. Expanding organizational context support, the rollout provides more relevant code suggestions for distributed teams.
- [GPT-5.2-Codex is now available in GitHub Copilot for Multiple IDEs](https://github.blog/changelog/2026-01-26-gpt-5-2-codex-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
## GitHub Copilot CLI: Automation, Model Switching, and Agent Protocols
The Copilot CLI continues to improve for terminal automation. Following last week''s introduction of "Plan" mode, this week adds seamless switching between 14 AI models—including Claude Opus 4.5 and GPT-5.2 Codex—directly in the CLI. This gives finer-grained control over model selection and reasoning for code changes and analysis tasks. Voice dictation speeds up command input, and installation is now easier with Homebrew and Winget for Windows and macOS.
A public preview of the Agent Client Protocol (ACP) brings standardized integration for Copilot CLI with other tools, IDEs, and CI/CD pipelines, making Copilot automation easier to embed. New tutorials and demos show practical CLI use for onboarding, code review, debugging, and accessibility workflows. Enhanced command approval, permissions, and automation based on context further improve secure development.
- [What’s New in the GitHub Copilot CLI?](/github-copilot/videos/whats-new-in-the-github-copilot-cli)
- [Switching Models in GitHub Copilot CLI: Demo by @shanselman](/github-copilot/videos/switching-models-in-github-copilot-cli-demo-by-shanselman)
- [ACP Protocol Support Now Available in GitHub Copilot CLI](https://github.blog/changelog/2026-01-28-acp-support-in-copilot-cli-is-now-in-public-preview)
- [Power Agentic Workflows in Your Terminal with GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/power-agentic-workflows-in-your-terminal-with-github-copilot-cli/)
- [How a Designer Built an ASCII Animation Tool with GitHub Copilot](/github-copilot/videos/how-a-designer-built-an-ascii-animation-tool-with-github-copilot)
- [Engineering Accessibility and Animation for GitHub Copilot CLI’s ASCII Banner](https://github.blog/engineering/from-pixels-to-characters-the-engineering-behind-github-copilot-clis-animated-ascii-banner/)
## GitHub Copilot SDK and Agent Framework
The Copilot SDK and agent features have been enhanced, continuing last week''s focus on cross-language SDKs, agentic workflows, and integrations for Node.js, Python, Go, and .NET. The SDK works with the Microsoft Agent Framework so you can use Copilot, Azure OpenAI, and other models together. This improves modular workflows with features for session management, streaming, and permissions.
Guides cover tasks like image extraction, agent memory for update tracking, multi-model routing, and hybrid AI that combines Foundry Local SLMs for private data with cloud LLMs for richer content. The tutorials support rapid prototyping and stronger automation for teams using AI at scale.
- [Building a Color Palette App Using GitHub Copilot SDK](/github-copilot/videos/building-a-color-palette-app-using-github-copilot-sdk)
- [Building Agents with GitHub Copilot SDK: A Practical Guide to Automated Tech Update Tracking](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-agents-with-github-copilot-sdk-a-practical-guide-to/ba-p/4488948)
- [Hybrid AI with GitHub Copilot SDK: Automating README to PowerPoint Generation Using Microsoft Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/github-copilot-sdk-and-hybrid-ai-in-practice-automating-readme/ba-p/4489694)
- [Build AI Agents with GitHub Copilot SDK and Microsoft Agent Framework](https://devblogs.microsoft.com/semantic-kernel/build-ai-agents-with-github-copilot-sdk-and-microsoft-agent-framework/)
- [Integrating GitHub Copilot SDK: Live Coding on Rubber Duck Thursdays](/github-copilot/videos/integrating-github-copilot-sdk-live-coding-on-rubber-duck-thursdays)
## GitHub Copilot Agent Ecosystem and Extension Mechanisms
Copilot now offers an Agents tab in repositories for centralized task management, giving teams a single point to view, switch, and track coding agent activity in the desktop, browser, or terminal interface. This update builds on recent efforts for team-based agent infusions and smoother modular integration.
Comparisons between Custom Agents, Agent Skills, and MCP Tools help guide users in building the right automation mix for their workflows. Updates to the GitHub MCP Server add features including OAuth filtering, Copilot integration with pull requests, and project tracking. These build on recent work in credential management and platform tools. Also, unit test agent profiles for Logic Apps and Data Maps extend cloud-based test automation.
- [Introducing the Agents Tab for Copilot Coding Agents in GitHub Repositories](https://github.blog/changelog/2026-01-26-introducing-the-agents-tab-in-your-repository)
- [Comparing Custom Agents, Skills, and MCP Tools in GitHub Copilot](https://harrybin.de/posts/github-copilot-context-extensions-compared/)
- [GitHub MCP Server: New Tools for Project Management, OAuth Filtering, and Copilot Integration](https://github.blog/changelog/2026-01-28-github-mcp-server-new-projects-tools-oauth-scope-filtering-and-new-features)
- [Introducing Unit Test Agent Profiles for Logic Apps & Data Maps](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-unit-test-agent-profiles-for-logic-apps-data-maps/ba-p/4490216)
## Copilot for Cloud, DevOps, and App Deployment
Copilot continues to play a bigger part in DevOps by adding more features for CI/CD pipeline automation, cloud migration, and app deployment. The Copilot Agent Skill for Azure Static Web Apps simplifies deployment for JavaScript frameworks, automating configuration checks and troubleshooting. This adds to previous updates supporting Java, Spring, and cloud modernization tasks. Integration with GitHub Actions and Copilot Chat now provides real-time troubleshooting and error resolution for teams adopting or migrating cloud resources.
- [Introducing the Azure Static Web Apps Skill for GitHub Copilot](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-the-azure-static-web-apps-skill-for-github-copilot/ba-p/4487920)
## Copilot Metrics, Compliance, and API Evolution
This week brings public preview dashboards and reporting APIs for Copilot Enterprise Cloud customers with specific data residency requirements. New reporting capabilities help organizations monitor adoption, activity, and features for compliance and data governance. As legacy Copilot metrics APIs are being replaced with more detailed endpoints, users will need to migrate to maintain up-to-date analytics.
- [Copilot Usage Metrics in GitHub Enterprise Cloud with Data Residency: Public Preview](https://github.blog/changelog/2026-01-29-copilot-metrics-in-github-enterprise-cloud-with-data-residency-in-public-preview)
- [Legacy GitHub Copilot Metrics APIs to Sunset in 2026](https://github.blog/changelog/2026-01-29-closing-down-notice-of-legacy-copilot-metrics-apis)
## Other GitHub Copilot News
Best practices for Copilot usage continue to be highlighted, focusing on using Copilot Chat, building effective trust models, and validating outputs in real workflows. This aligns with Copilot''s documented approach to onboarding and code review.
Copilot’s integration into SQL Server Migration Assistant for Oracle offers new AI-powered support for large codebase migrations, supporting ongoing modernization efforts detailed last week for Java and Spring environments.
Community workshops and demos remain a central channel for hands-on learning. These resources illustrate how to use SDKs and CLI integrations, write effective prompts, and modernize applications with Copilot.
- [Trust, but Verify: Building Confidence in GitHub Copilot Output](https://www.cooknwithcopilot.com/blog/trust-but-verify-building-confidence-in-github-copilot-output.html)
- [Accelerating Oracle to SQL Server Migrations with AI and Copilot in SSMA](/ai/videos/accelerating-oracle-to-sql-server-migrations-with-ai-and-copilot-in-ssma)
- [Modernizing Applications with GitHub Copilot: Workshop Overview](/github-copilot/videos/modernizing-applications-with-github-copilot-workshop-overview)',
    'GitHub Copilot continues to grow as an AI-powered development platform by adding automation options, agent-based workflows, and support for enterprise integrations. This week, the platform introduced additional SDKs, updates to the CLI, new agent features, and targeted resources for developers and architects. Key releases include expanded support for the GPT-5.2-Codex model, agent protocol integration in the CLI, improvements in SDK orchestration, and new resources for validating Copilot-powered tools at scale. Building on last week''s coverage of agentic workflows, Copilot is now more tightly integrated across developer tools and automation workflows.',
    1770019200, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-02-02', 'TechHub',
    'TechHub', 'F79ACF1A5DF8BDBD265675DE469DE577712AD7463C8CB46403DB530F32B407DE', ',GitHub Copilot,GPT 5.2 Codex,Copilot Chat,Copilot CLI,Agent Client Protocol,AI Agents,Copilot SDK,Microsoft Agent Framework,Model Switching,MCP Server,GitHub Actions,CI/CD Automation,Azure Static Web Apps,Copilot Metrics API,Data Residency,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-01-26
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-01-26', 'roundups', 'Weekly GitHub Copilot Roundup: SDK Agents, CLI Plan Mode, Modernization',
    'GitHub Copilot continues to add new agent-based SDK capabilities, as well as improvements for its command-line interface and tools for automating modernization. Developers can now use Copilot in different development environments, including IDEs, terminals, automated pipelines, and custom agent-based apps. The latest enhancements to the CLI, better integration with organizational information, and tools for Java modernization are all designed to provide more concrete benefits for programming teams.
<!--excerpt_end-->
## GitHub Copilot SDK: Powering Custom Agentic Development
The GitHub Copilot SDK is now in technical preview, following a recent rollout for platforms including Node.js, Python, Go, and .NET. Step-by-step guides now demonstrate how to use Copilot’s code completions inside purpose-built agent applications and automation tasks. The SDK expands on CLI capabilities with features such as agentic workflows, memory, tool orchestration, routing for multiple models, and streaming. Microsoft and contributors are offering in-depth examples for embedding agents using Python, enabling .NET in Visual Studio, and combining Copilot with external model authentication. The platform’s modular approach is suitable for organizations creating custom AI-enabled workflows and tools, and user feedback notes broader integration with internal processes.
- [Bringing Work Context to Your Code with GitHub Copilot SDK](https://devblogs.microsoft.com/blog/bringing-work-context-to-your-code-in-github-copilot)
- [Building Agentic Apps with the GitHub Copilot SDK: New Developer Paradigms](https://www.linkedin.com/posts/satyanadella_build-an-agent-into-any-app-with-the-github-activity-7420126187286568961-TdW7)
- [Build an Agent into Any App with the GitHub Copilot SDK](https://github.blog/news-insights/company-news/build-an-agent-into-any-app-with-the-github-copilot-sdk/)
- [Using the GitHub Copilot SDK with Python](/github-copilot/videos/using-the-github-copilot-sdk-with-python)
- [The GitHub Copilot SDK is here! - Rubber Duck Thursdays](/github-copilot/videos/the-github-copilot-sdk-is-here-rubber-duck-thursdays)
- [Open Source Friday: Exploring the GitHub Copilot SDK](/github-copilot/videos/open-source-friday-exploring-the-github-copilot-sdk)
- [Add an AI Agent to Your Application with GitHub Copilot SDK](/github-copilot/videos/add-an-ai-agent-to-your-application-with-github-copilot-sdk)
## GitHub Copilot CLI: Enhanced Terminal Workflows and Integration
GitHub Copilot CLI now offers new features for automatically using AI within the terminal, following updates such as support for different models (including GPT-5 mini, GPT-4.1), better installation, and agent-driven command-line tools. The updated "Plan" mode introduces guided, stepwise code planning before code is generated, which aligns with the broader move toward agentic workflows. GPT-5.2-Codex support provides improved prompt handling and context control (`/context`). The CLI now integrates with `gh copilot` for quick setup and unified onboarding for teams. Features for background task delivery (`/delegate`), persistent repository memory, review functionality, and audit/history tools all support better security practices. The YOLO mode and CI/CD automation are designed to cut repeated tasks.
- [Install and Use GitHub Copilot CLI Directly from the GitHub CLI](https://github.blog/changelog/2026-01-21-install-and-use-github-copilot-cli-directly-from-the-github-cli)
- [GitHub Copilot CLI: Plan Mode, Advanced Reasoning, and Terminal Workflow Enhancements](https://github.blog/changelog/2026-01-21-github-copilot-cli-plan-before-you-build-steer-as-you-go)
- [A Cheat Sheet to Slash Commands in GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/a-cheat-sheet-to-slash-commands-in-github-copilot-cli/)
- [Demo: Using /delegate in the GitHub Copilot CLI](/github-copilot/videos/demo-using-delegate-in-the-github-copilot-cli)
- [Demo: Using GitHub Copilot CLI and YOLO Mode](/github-copilot/videos/demo-using-github-copilot-cli-and-yolo-mode)
- [Building with GitHub Copilot CLI: Rubber Duck Thursdays Live Coding Stream](/github-copilot/videos/building-with-github-copilot-cli-rubber-duck-thursdays-live-coding-stream)
## Organizational and Work Context Integration
For distributed and enterprise development teams, Copilot now adds greater organizational context into AI features. Following last week’s update on context engineering and memory, these integrations allow Copilot to pull from Microsoft 365, SharePoint, and call transcripts via the Work IQ MCP server. You can now search organization documents and previous work history directly from the IDE or command line, which helps with requirement matching and audit needs. Persistent and auditable context handling strengthens code reviews and onboarding, which continues the trend of team-wide curation and verifiable memory functions. Teams report higher productivity and more accurate requirement handling.
- [Bringing Work Context to Your Code with GitHub Copilot SDK](https://devblogs.microsoft.com/blog/bringing-work-context-to-your-code-in-github-copilot)
- [Bringing Organizational Context to GitHub Copilot CLI with Work IQ](https://www.linkedin.com/posts/satyanadella_so-much-of-dev-work-happens-in-the-context-activity-7420485585376620544-vudJ)
- [Bringing Work Context to Your Code in GitHub Copilot](/github-copilot/videos/bringing-work-context-to-your-code-in-github-copilot)
## App Modernization with GitHub Copilot
Copilot’s modernization tools support more automation for Java and Spring upgrades. Last week’s Java EE to Jakarta EE migration is now followed by tools to automate updates for Spring Boot and Spring Framework, with step-by-step guides for both JDK upgrades and secure identity changes. These tools combine security, dependency management, and automated refactoring—including support for OpenRewrite, JDK/build planning, and Microsoft’s ID and Key Vault. This reduces friction for enterprise migrations, covering tasks like namespace changes, dependency and CVE analysis, fixing security vulnerabilities, and deeper IDE integration. Focus remains on simplifying post-migration review, automated code improvements, and cloud adoption.
- [Modernizing Spring Boot Applications with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-spring-boot-applications-with-github-copilot-app/ba-p/4486466)
- [Modernizing Applications by Migrating Code to Managed Identity with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-applications-by-migrating-code-to-use-managed/ba-p/4486481)
- [Migrating Application Credentials to Azure Key Vault with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/migrating-application-credentials-to-azure-key-vault-with-github/ba-p/4486482)
- [Modernizing Spring Framework Applications with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-spring-framework-applications-with-github-copilot/ba-p/4486469)
- [Upgrade Your Java JDK (8, 11, 17, 21, or 25) with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/upgrade-your-java-jdk-8-11-17-21-or-25-with-github-copilot-app/ba-p/4486468)
## Integrating and Expanding Copilot Use Across Platforms
GitHub Copilot continues to integrate with additional workflows. After last week’s news about Copilot support for OpenCode (including login and credential management across desktop, terminal, and IDE), current guides show practical setup steps for connecting Copilot to OpenCode and CI/CD tools. An in-depth look at Copilot’s Arm Cloud Migration Agent discusses how Copilot helps with container migration, indicating expanded applicability in infrastructure and cloud migrations. Community updates show more widespread adoption.
- [How to use GitHub Copilot with OpenCode](/github-copilot/videos/how-to-use-github-copilot-with-opencode)
- [GitHub Copilot Arm Cloud Migration Agent Deep Dive](/github-copilot/videos/github-copilot-arm-cloud-migration-agent-deep-dive)
- [The Download: GitHub Copilot SDK Updates, Copilot for OpenCode, and Cloudflare Buys Astro](/github-copilot/videos/the-download-github-copilot-sdk-updates-copilot-for-opencode-and-cloudflare-buys-astro)
## Advanced Workflow Guidance and Tutorials
Recent resources offer strategies for using Copilot throughout the development process. Tutorials cover test-driven development with agents, Copilot review practices, and ways Copilot functions as a collaborator rather than just a code assistant. KQL support in Microsoft Fabric continues to show how Copilot can power analytics and engineering. Community events like the .NET AI Community Standup discuss new SDK features and integration, highlighting practical implementation advice.
- [Applying Context Windows, Plan Agent, and TDD with GitHub Copilot to Build a Countdown App](https://github.blog/developer-skills/application-development/context-windows-plan-agent-and-tdd-what-i-learned-building-a-countdown-app-with-github-copilot/)
- [Introducing Copilot for Real-Time Dashboards: Write KQL with Natural Language](https://blog.fabric.microsoft.com/en-US/blog/introducing-copilot-for-real-time-dashboards-write-kql-with-natural-language/)
- [How to Review GitHub Copilot’s Work Like a Senior Developer](https://www.cooknwithcopilot.com/blog/how-to-review-github-copilots-work-like-a-senior-developer.html)
- [.NET AI Community Standup - Using the GitHub Copilot SDK in .NET Apps](/github-copilot/videos/net-ai-community-standup-using-the-github-copilot-sdk-in-net-apps)',
    'GitHub Copilot continues to add new agent-based SDK capabilities, as well as improvements for its command-line interface and tools for automating modernization. Developers can now use Copilot in different development environments, including IDEs, terminals, automated pipelines, and custom agent-based apps. The latest enhancements to the CLI, better integration with organizational information, and tools for Java modernization are all designed to provide more concrete benefits for programming teams.',
    1769414400, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-01-26', 'TechHub',
    'TechHub', 'ED712365698509C1F810ED62750F88B024826489F903BEFECBE9F41F20BCA312', ',GitHub Copilot,Copilot SDK,Agentic Workflows,Copilot CLI,GitHub CLI,Slash Commands,CI/CD,Model Routing,Memory And Context,Microsoft 365,SharePoint,Java Modernization,Spring Boot,OpenRewrite,Azure Key Vault,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-01-19
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-01-19', 'roundups', 'Weekly GitHub Copilot Roundup: Models, BYOK, and Agent Memory',
    'Extending from last week’s progress, GitHub Copilot adds further AI-based upgrades, new workflow integration, and developer tutorials. New capabilities include additional model options, more ways to customize, better context-awareness through memory and agent-based features, plus collaboration improvements for individual and group coding. You’ll find practical guides and analysis on Copilot’s growing role in meeting business needs, managing feedback, and developing more agent-like coding tools.
<!--excerpt_end-->
## Enhanced AI Models and Model Management in Copilot
After recent attention on multiple models, Copilot now includes GPT-5.2-Codex for all paid tiers, giving access to code suggestions, chat, and agent features in tools like VS Code, GitHub.com, Copilot CLI, and GitHub Mobile. Organization admins enable it through settings, and Pro users can turn it on via prompts. The distribution is rolling out gradually, and Bring Your Own Key (BYOK) allows developers to use their OpenAI API keys in VS Code.
GitHub has officially announced the retirement of older models (including Claude Opus 4.1, GPT-5, and GPT-5-Codex) set for February 17, 2026. These will be replaced by Claude Opus 4.5, GPT-5.2, and GPT-5.1-Codex for chat, code completion, and agent features. Update instructions are included for managing model transitions.
- [GPT-5.2-Codex Now Available in GitHub Copilot](https://github.blog/changelog/2026-01-14-gpt-5-2-codex-is-now-generally-available-in-github-copilot)
- [Upcoming Deprecation of Select GitHub Copilot Models from Claude and OpenAI](https://github.blog/changelog/2026-01-13-upcoming-deprecation-of-select-github-copilot-models-from-claude-and-openai)
## Enterprise-Grade Copilot: Integration, BYOK, and Modernization
Building on recent discussions around workflows and organizational adoption, Copilot’s BYOK function now supports AWS Bedrock, Google AI Studio, and any OpenAI API-compatible provider (including Anthropic and others). Enterprises can define their own context size, use the Responses API for multimodal work, and enable streamed results within the IDE. All of these options are in public preview for Enterprise and Business editions, and are designed for better security, cost flexibility, and fine-tuned performance.
Copilot is now officially integrated with OpenCode, so teams can log in across terminals, desktops, or IDEs using GitHub credentials—streamlining authentication for varied coding environments.
There’s also new support for upgrading older Java EE applications to Jakarta EE using Copilot’s modernization tools, which feature automated code analysis, migration planning, refactoring, and code security checks. Integration with OpenRewrite and plugins for VS Code and IntelliJ IDEA simplify upgrades, handle library dependency changes, and highlight known security issues.
- [Enhancements to GitHub Copilot Bring Your Own Key (BYOK) Capabilities](https://github.blog/changelog/2026-01-15-github-copilot-bring-your-own-key-byok-enhancements)
- [GitHub Copilot Now Officially Supports OpenCode Integration](https://github.blog/changelog/2026-01-16-github-copilot-now-supports-opencode)
- [Modernizing Java EE Applications to Jakarta EE with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-java-ee-applications-to-jakarta-ee-with-github/ba-p/4486471)
## Context Awareness, Agentic Workflows, and Memory Systems
Extending the Agent Skills and playbooks introduced last week, GitHub Copilot Memory is now in public preview for all paid plans, letting models retain repository-specific context for improved coding suggestions and code reviews. Memories are verified, expire automatically after 28 days, and are managed through GitHub settings.
A new agentic memory system offers citation-backed memory objects across code, CLI, and code review agents, recording team standards and choices with code links for tracing and verification. Early tests show improved effectiveness in reviews, developer onboarding, and onboarding of best practices.
Visual Studio now curates Copilot Memories covering standards, rules, and personal preferences, automatically generating reference documentation and helping with consistency. Instruction file support helps new developers learn team practices quickly.
- [Agentic Memory Now in Public Preview for GitHub Copilot](https://github.blog/changelog/2026-01-15-agentic-memory-for-github-copilot-is-in-public-preview)
- [Building an Agentic Memory System for GitHub Copilot](https://github.blog/ai-and-ml/github-copilot/building-an-agentic-memory-system-for-github-copilot/)
- [Copilot Memories: Streamlining Team Coding with Visual Studio](https://devblogs.microsoft.com/visualstudio/copilot-memories/)
## CLI, SDK, and AI Toolkit Updates
The Copilot CLI now offers more model choices, including GPT-5 mini and GPT-4.1, available from the terminal for all subscribers without extra API charges. Recent improvements added automated exploration, planning, and review agents, with new skills and better session organization. Installation is now unified (covering WinGet, Homebrew, Dev Containers, and standalone executables), and command-line scripting and token handling have minimal friction.
The Copilot SDK is now in technical preview for Node.js, Python, Go, and .NET, making it possible to embed Copilot features and agents into CI/CD scripts, IDE plugins, and workflow automation tools.
The latest AI Toolkit for VS Code (version 0.28.1) introduces Copilot Skills for agent programming, tighter integration with Microsoft Foundry, new support for Anthropic models, and better profiling tools. It also includes various improvements in sign-in, the user interface, and performance.
- [GitHub Copilot CLI: Enhanced Agents, Context Management, and Installation Methods](https://github.blog/changelog/2026-01-14-github-copilot-cli-enhanced-agents-context-management-and-new-ways-to-install)
- [Copilot SDK Technical Preview: Multi-Language Access to GitHub Copilot CLI](https://github.blog/changelog/2026-01-14-copilot-sdk-in-technical-preview)
- [AI Toolkit for VS Code: January 2026 Update — Copilot Skills, Foundry Integration, and Dev Enhancements](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-toolkit-for-vs-code-january-2026-update/ba-p/4485205)
## Context Engineering, Collaboration Patterns, and Practical AI Usage
This section continues last week’s look at context engineering for Copilot. There are guides on using custom instructions, prompt files, and agents to get more reliable coding support, including examples for security and documentation automation. Setup instructions include markdown-based context files for VS Code and GitHub workflows.
Another article explores moving from ad-hoc programming (“vibe coding”) to a more structured, spec-driven workflow using Spec-Kit, Copilot, .NET 9, and Blazor. The approach shows how teams can use specifications to guide code review and architecture.
Developers are advised on when to assign repetitive work to Copilot and when to use their own judgment. There are also tutorials for running several agents together in VS Code for linked tasks, linting, and handling tasks in parallel, reflecting more complex, real-world development needs.
- [Want Better AI Outputs? Try Context Engineering with GitHub Copilot](https://github.blog/ai-and-ml/generative-ai/want-better-ai-outputs-try-context-engineering/)
- [From Vibe Coding to Spec-Driven Development: Practical Spec-Kit Workflow](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development-part2)
- [When to Lead, When to Delegate to GitHub Copilot](https://www.cooknwithcopilot.com/blog/when-to-lead-when-to-delegate-to-github-copilot.html)
- [Orchestrating Multiple AI Agents in VS Code: Insights from Ben & Peng](/github-copilot/videos/orchestrating-multiple-ai-agents-in-vs-code-insights-from-ben-and-peng)
## Real-World Impact and Developer-Centric Analysis
Adding to last week’s coverage of open source and feedback, the latest analysis and the Octoverse 2025 report show that developers use Copilot mainly for transparent, oversight-enabled automation. Most developers value customizable suggestions and in-context options for code, documentation, and refactoring tasks, but expect to stay in control of important architecture decisions. Teams also iterate on product design based on this ongoing feedback.
Octoverse 2025 includes topics like language popularity, agent-driven workflows (“vibe coding”), default security strategies, open source adoption, and renewal of legacy expertise (for instance, COBOL).
- [Inside Octoverse 2025: Vibe Coding, Agentic AI, and Shifting Developer Trends](/github-copilot/videos/inside-octoverse-2025-vibe-coding-agentic-ai-and-shifting-developer-trends)
- [What AI coding tools are actually good for, according to developers](https://github.blog/ai-and-ml/generative-ai/what-ai-is-actually-good-for-according-to-developers/)
## Other GitHub Copilot News
A practical case illustrates Copilot’s role in rapidly building docfind, a client-only search engine for VS Code documentation using Rust and WebAssembly, demonstrating Copilot’s adaptability for different technical problems.
- [Building docfind: Fast Client-Side Search for VS Code Docs with Rust, WASM, and Copilot](https://code.visualstudio.com/blogs/2026/01/15/docfind)',
    'Extending from last week’s progress, GitHub Copilot adds further AI-based upgrades, new workflow integration, and developer tutorials. New capabilities include additional model options, more ways to customize, better context-awareness through memory and agent-based features, plus collaboration improvements for individual and group coding. You’ll find practical guides and analysis on Copilot’s growing role in meeting business needs, managing feedback, and developing more agent-like coding tools.',
    1768809600, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-01-19', 'TechHub',
    'TechHub', '51B168493C89C12C48A6B82331F09C32B8F84A8F8ADB7A32A3F40ADA84013FEB', ',GitHub Copilot,GPT 5.2 Codex,Model Deprecation,BYOK,OpenAI API,AWS Bedrock,Google AI Studio,Anthropic,Copilot Memory,Agentic Workflows,Copilot CLI,Copilot SDK,VS Code,Java EE To Jakarta EE,AI Toolkit For VS Code,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-01-12
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-01-12', 'roundups', 'Weekly GitHub Copilot Roundup: Multi-Model IDEs and Agent Skills',
    'This week, GitHub Copilot introduced broader model access in leading IDEs and enabled more tailored workflow customization. These updates span both real-world developer scenarios and educational resources, highlighting Copilot''s wider support in conventional programming environments and creative use cases.
<!--excerpt_end-->
## GitHub Copilot Expands Capabilities in Popular IDEs
With continued support for multiple agent types and improved model integration, the Gemini 3 Flash model for Copilot Chat is now available in public preview. Visual Studio (17.14.12+ and 18.1.0+), JetBrains IDEs (1.5.62+), Xcode (0.46.0+), Eclipse (0.14.0+), github.com, and GitHub Mobile now let users select Gemini 3 Flash via the chat model picker. This makes Copilot’s multi-model support more accessible in standard developer environments. Modes including chat, ask, edit, and agent are available in Visual Studio and JetBrains IDEs, while Xcode and Eclipse provide ask and agent options for custom workflows.
- [Gemini 3 Flash Model Now Accessible in GitHub Copilot Across Major IDEs](https://github.blog/changelog/2026-01-06-gemini-3-flash-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
## Copilot Customization and New Workflows in Visual Studio Code
Extending the focus on project-specific workflows in VS Code, the December 2025 (v1.108) release adds Agent Skills for GitHub Copilot. Developers can save specialized knowledge in SKILL.md files within .github/skills and load them during Copilot chat sessions, supporting individualized guidance and project context.
Updates also bring improved navigation, session management, keyboard and git workflow support, and better debugging and accessibility. Copilot plugin development and issue handling gains new tooling for code quality and maintainability in both enterprise and open source environments.
- [Visual Studio Code December 2025 Release (v1.108): New Features and Improvements](https://code.visualstudio.com/updates/v1_108)
## Tutorials: Real-World, Agentic, and Spec-Driven Copilot Workflows
New tutorials demonstrate Copilot usage across hands-on projects and agent-driven approaches. The Space Invaders walkthrough shows Copilot integrating with Figma, implementing advanced gameplay mechanics, Playwright code reviews, and structured feature specs, supporting consistent team practices and automation with MCP. Azure MCP and CLI deployment strategies build on recent teamwork themes using intelligent agents.
A Copilot lab for Visual Studio Code explores agent mode, covering MCP server connections, custom agent development, and automated background tasks. These learning resources help developers move from basic completion to orchestrated multi-agent workflows, continuing the shift toward scalable, enterprise-grade Copilot implementations.
- [Building Space Invaders from Scratch with GitHub Copilot, AI Toolkit, and Azure Deployment](/ai/videos/building-space-invaders-from-scratch-with-github-copilot-ai-toolkit-and-azure-deployment)
- [Hands-On Lab: The Power of GitHub Copilot in Visual Studio Code](/ai/videos/hands-on-lab-the-power-of-github-copilot-in-visual-studio-code)
## Analyses: Copilot’s Impact on Frameworks, Open Source, and Coding Standards
Analysis this week looks at how Copilot influences technology choices and engineering approaches. Articles discuss Copilot’s role in supporting React’s ongoing use, building on past themes about AI and statically typed languages and developer efficiency.
An interview with Homebrew maintainer Mike McQuaid spotlights open source automation and scaling strategies involving Copilot. Emphasis is placed on bug and documentation management through automated AI processes previously discussed in agentic and MCP concepts.
A case for moving beyond informal code styles to systematic, specification-led workflow development supports a disciplined approach to software engineering as AI becomes more integral to project delivery.
- [Will AI Make React Popular Forever?](/ai/videos/will-ai-make-react-popular-forever)
- [Sustaining Homebrew: Leadership, Automation, and AI with Mike McQuaid](/ai/videos/sustaining-homebrew-leadership-automation-and-ai-with-mike-mcquaid)
- [From Vibe Coding to Spec-Driven Development: Why AI-Generated Code Needs Structure](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development)
## Other GitHub Copilot News
This week’s case study highlights Microsoft Copilot helping Deaf creators. Min-ji So’s Webtoon Storyboard Assistant uses conversational AI for grammar suggestions and creative support, further demonstrating Copilot''s accessibility and flexibility across industry and creative fields. These examples reinforce Copilot''s growing role in enabling new workflows and fostering broad user impact.
- [Empowering Deaf Creators: Min-ji So''s Journey with Microsoft Copilot and AI](https://news.microsoft.com/source/asia/features/a-deaf-writers-journey-with-ai-discovering-new-creative-paths/?lang=ko)',
    'This week, GitHub Copilot introduced broader model access in leading IDEs and enabled more tailored workflow customization. These updates span both real-world developer scenarios and educational resources, highlighting Copilot''s wider support in conventional programming environments and creative use cases.',
    1768204800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-01-12', 'TechHub',
    'TechHub', '8D700542C7D3738E9D07FF1CF6696EE68F4E6E3775D06B682C16526D304BFC3D', ',GitHub Copilot,Copilot Chat,Gemini 3 Flash,VS,VS Code,JetBrains IDEs,Xcode,Eclipse IDE,Agent Mode,Agent Skills,SKILL.md,MCP,Playwright,Spec Driven Development,Open Source Automation,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2026-01-05
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2026-01-05', 'roundups', 'Weekly GitHub Copilot Roundup: MCP, Agents, and Spec-Driven Work',
    'GitHub Copilot continues to play a larger role in development workflows, especially in Visual Studio Code and agent-driven automation. Current topics include using Copilot and Model Context Protocol (MCP) for agents that better understand workspace context, improved collaboration, and coding processes structured around clear specifications.
<!--excerpt_end-->
## Context Engineering and Team Workflows in VS Code
Building on last week’s guidance for Copilot as a context-aware assistant in Visual Studio Code, this update covers detailed steps for setting up Copilot to behave consistently across teams. Previous discussions highlighted Agent Skills, structured prompts, and reusable instructions; now, the focus shifts to practical implementation using templates, custom guidance, and Copilot Plugins (MCPs) tailored for enterprise environments.
The article describes processes for encoding your project’s coding standards, architecture, and workflow guidance directly into your Copilot agent. It expands earlier updates on managing sessions and setup at the repository level, with hands-on templates and reference repositories that help teams achieve consistent Copilot behavior. These reusable practices support automation and peer review, giving teams a way to guide AI toward their established standards.
This content moves ahead from last week’s coverage of agent and workflow sharing—such as repository-wide settings and Skills.md—by providing tools that enforce norms and support larger collaborative engineering teams.
- [Scaling Context-Aware Workflows with GitHub Copilot in VS Code](/ai/videos/scaling-context-aware-workflows-with-github-copilot-in-vs-code)
## Agentic AI, MCP Integration, and Spec-Driven Development
After recent deep-dives into Copilot’s agent-driven modes and use of MCP, the latest resources demonstrate more cross-agent project work and improved model integration. Last week covered Agent Mode, the Cloud Agent, and support for BYOK (Bring Your Own Key) automation in VS Code; this week examines the new features in Copilot Agent Mode and stronger IDE integration.
The article highlights how automation such as repository scanning, code editing, and pull request support now benefits from better MCP integration. Copilot coordinates multiple agent types, including Anthropic, OpenAI, and Google models, all managed under one subscription—a natural extension of previous collaborative updates.
Agent HQ, introduced at GitHub Universe, supports community agent sharing and mult-agent workflows. The linked article explains how Spec Kit—mentioned previously as the base for specification-driven development—empowers robust, repeatable, and maintainable automation across teams.
Altogether, these articles show how Copilot is moving from simple code completion towards a platform supporting customizable, team-managed agents. The introduction of the MCP Registry and more case studies demonstrates how these new approaches are being put to use. Teams can now move beyond suggestions to structured, collaborative AI tools suited to company standards.
- [Agentic AI, MCP, and Spec-Driven Development: The Biggest GitHub Dev Topics of 2025](https://github.blog/developer-skills/agentic-ai-mcp-and-spec-driven-development-top-blog-posts-of-2025/)',
    'GitHub Copilot continues to play a larger role in development workflows, especially in Visual Studio Code and agent-driven automation. Current topics include using Copilot and Model Context Protocol (MCP) for agents that better understand workspace context, improved collaboration, and coding processes structured around clear specifications.',
    1767600000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2026-01-05', 'TechHub',
    'TechHub', 'D24CF760802C080FA56EA1F7EF725555475C81471C7CD91C402BD8493908F2E6', ',GitHub Copilot,VS Code,Copilot Agent Mode,Agentic AI,MCP,MCP Registry,Copilot Plugins,Context Engineering,Prompt Templates,Repository Level Configuration,Spec Driven Development,Spec Kit,Agent HQ,Multi Model Integration,Enterprise Developer Workflows,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-12-29
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-12-29', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, MCP, BYOK, and Safer Automation',
    'This week’s updates for GitHub Copilot focus on improving workflow capabilities and strengthening integration with common tools. Visual Studio Code adds new features for agent automation and customization, and practical guides show how to use Copilot to support daily work. Security enhancements, more flexible agent models, and ongoing community contributions continue to make Copilot a valuable tool for AI-driven development.
<!--excerpt_end-->
## GitHub Copilot Integration and Agentic Automation in Visual Studio Code
Building on last week''s coverage of agent skills and Mission Control, Visual Studio Code now offers more seamless Copilot integration for daily work, with new tools for customizing and automating agents. In the VS Code 2025 retrospective by Burke Holland and Pierce Boggan, these changes are highlighted, including continued advancements in Agent Skills, Mission Control, and the availability of Copilot Universe and Free features to make AI coding support accessible, as referenced in earlier updates.
Agent Mode and Auto Mode—released after last week’s reusable workflow instructions—make it easier for developers to automate tasks and background activities. The inclusion of Model Choice Platform (MCP) and Bring Your Own Key (BYOK) allows teams to choose and configure AI models as needed, matching earlier discussions around governance and transparency.
Recent improvements to prompt engineering, including updated prompt file support and multi-window chat, give developers a more responsive interactive workflow. Support for Cloud Agent, originally launched in May, continues the trend toward parallel task execution and integrates with pull request processes to help reduce development waits.
Security remains an ongoing effort; terminal sandboxing and new safety measures address concerns about agent execution, continuing the platform protections discussed in relation to earlier vulnerabilities like React2Shell. Visual Studio Code 1.107 introduces agent session management in Copilot Chat, adding enterprise-level tracking of Copilot activities, consistent with progress on team metrics and review features.
Asynchronous workflow delegation now extends through Copilot CLI and cloud agents, improving on last week’s methods for running isolated agent workflows and addressing merge conflict management through separate branches. The latest updates provide more options for full review in source control, balancing automation with necessary human checks.
Custom agents can now be shared at the organization level, with repository-based enterprise controls supporting community-driven sharing through Skills.md and the Agent Skills forum. This positions Copilot as a platform for reusable automation. New MCP server bundling and infrastructure adjustments make setup and integration easier, reflecting last week’s focus on agent persistence and operational context.
Security processes, project-specific skills, and review controls build on last week''s coverage of compliance, review assignments, and tailored AI integration, keeping Copilot headed toward a broad-use productivity solution that addresses enterprise needs.
- [VS Code 2025: Year in Review with Burke Holland & Pierce Boggan](/ai/videos/vs-code-2025-year-in-review-with-burke-holland-and-pierce-boggan)
- [Visual Studio Code and GitHub Copilot - What''s new in 1.107](/ai/videos/visual-studio-code-and-github-copilot-whats-new-in-1107)
## Agentic Copilot Workflows in Visual Studio and Project Management
Agent-based workflows in Visual Studio are getting wider adoption, as detailed in Mads Kristensen’s recent guide. Projects like converting books to static websites and writing custom language extensions illustrate how Copilot supports automating repetitive tasks, while still allowing for careful oversight—a direction consistent with earlier features focused on context-specific applications.
These workflows use Visual Studio’s Cloud Agent, Copilot Chat, and extension APIs, aligning with recent coverage of Azure Boards and automation integrations. Further integration with GitHub Actions for CI/CD and NuGet for package management highlights Copilot’s role through the entire delivery pipeline, from code to deployment.
- [How AI Fixed My Procrastination: Using Copilot and AI Agents in Visual Studio Projects](https://devblogs.microsoft.com/visualstudio/how-ai-fixed-my-procrastination/)
## Copilot Coding Agent and Backlog Management Techniques
This week introduces the WRAP method for backlog management, which extends recent themes on context-driven workflows and structured project organization. WRAP blends task scoping, context management, and agent pairing, demonstrating how teams can use Copilot and agent automation to streamline task assignment and completion.
The approach builds directly on improvements in code review, project metrics, and agent integration discussed in prior roundups. The WRAP method details how Copilot can manage focused migrations or repetitive coding assignments, echoing examples of parallel workflows and pull request workflow integration from last week’s feature set.
Together, these resources offer step-by-step advice for using Copilot and agent automation in team environments, supporting extensibility and standardization in line with recent best-practice recommendations.
- [Maximize Your Backlog Productivity with the WRAP Method and GitHub Copilot Coding Agent](https://github.blog/ai-and-ml/github-copilot/wrap-up-your-backlog-with-github-copilot-coding-agent/)',
    'This week’s updates for GitHub Copilot focus on improving workflow capabilities and strengthening integration with common tools. Visual Studio Code adds new features for agent automation and customization, and practical guides show how to use Copilot to support daily work. Security enhancements, more flexible agent models, and ongoing community contributions continue to make Copilot a valuable tool for AI-driven development.',
    1766995200, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-12-29', 'TechHub',
    'TechHub', 'B577C0C2E973F40EC5491DCFB0F51FB4CCE36FC2BCAEF1B378576C29FE438C4C', ',GitHub Copilot,VS Code,VS Code 1.107,VS,Copilot Chat,Copilot CLI,Agent Mode,Auto Mode,Cloud Agent,Agent Skills,Mission Control,Model Choice Platform,Bring Your Own Key,Terminal Sandboxing,GitHub Actions,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-12-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-12-22', 'roundups', 'Weekly GitHub Copilot Roundup: Models, Agents, and Governance',
    'GitHub Copilot expands with new AI models (GPT-5.2, Claude Opus 4.5, Gemini 3 Flash), Agent Skills for reusable workflows, and Mission Control for managing automated agents—continuing its evolution into a complete developer productivity platform.
<!--excerpt_end-->
## AI Model Releases and Chat Model Availability
GitHub Copilot’s list of supported AI models continues to grow. GPT-5.2 is now available on all plans, building on earlier GPT-5.1, GPT-5.1-Codex, and Codex-Max releases. Developers can access these models directly in supported tools, including VS Code, Visual Studio, JetBrains IDEs, Xcode, Eclipse, GitHub Mobile, and the GitHub website. The recent addition of Claude Opus 4.5 offers another model for users, while Gemini 3 Flash has entered public preview—demonstrating Copilot''s ongoing focus on giving organizations flexibility and more administrative control. These tools grant transparency and allow real-time model selection, making it easier to match team needs and governance policies.
- [GPT-5.2 is now generally available in GitHub Copilot](https://github.blog/changelog/2025-12-17-gpt-5-2-is-now-generally-available-in-github-copilot)
- [GPT-5.1 and GPT-5.1-Codex Now Available in GitHub Copilot](https://github.blog/changelog/2025-12-17-gpt-5-1-and-gpt-5-1-codex-are-now-generally-available-in-github-copilot)
- [GPT-5.1-Codex-Max Now Available in GitHub Copilot Across Platforms](https://github.blog/changelog/2025-12-17-gpt-5-1-codex-max-is-now-generally-available-in-github-copilot)
- [Claude Opus 4.5 Now Available in GitHub Copilot](https://github.blog/changelog/2025-12-18-claude-opus-4-5-is-now-generally-available-in-github-copilot)
- [Gemini 3 Flash Public Preview Released for GitHub Copilot Users](https://github.blog/changelog/2025-12-17-gemini-3-flash-is-now-in-public-preview-for-github-copilot)
## Agent Skills and Automation Ecosystem
Developers can now use Agent Skills to organize and share reusable instructions following the open Skills.md standard. This helps standardize workflows and encourages sharing across teams. Community-curated repositories—such as ''github/awesome-copilot''—enable faster onboarding and foster workflow automation. These additions support earlier work on creating repeatable, integrated DevOps flows.
- [GitHub Copilot Now Supports Agent Skills](https://github.blog/changelog/2025-12-18-github-copilot-now-supports-agent-skills)
- [Agent Skills (Skills.md) in GitHub Copilot for Visual Studio Code](/ai/videos/agent-skills-skillsmd-in-github-copilot-for-visual-studio-code)
## Integrated Workflows and Mission Control
Mission Control is now available as a central tool for managing and observing agents, promoting better visibility into automated workflows. With real-time monitoring and tools for ensuring workflow consistency between local and cloud setups, Mission Control improves process management. The integration of Azure Boards with Copilot connects DevOps tracking directly with AI-aided code generation. New workflow options like manual branch selection and feedback tracking through Kanban boards also add to operational flexibility.
- [Manage All GitHub Copilot Agents with Mission Control](/ai/videos/manage-all-github-copilot-agents-with-mission-control)
- [Azure Boards Now Integrates Directly with GitHub Copilot Coding Agent](https://devblogs.microsoft.com/devops/github-copilot-for-azure-boards/)
- [GitHub Copilot Coding Agent: Practical Automation Examples](https://devopsjournal.io/blog/2025/12/20/Copilot-Agent-example)
## Code Review, Security, and Metrics
With the latest update, organizations can enable Copilot code review feedback for all contributors—even those without licenses—which broadens accessibility and usage across teams. New features have been added for tracking review actions, managing usage metrics, and monitoring team-level activity. Mission Control integrates security scanning tools like CodeQL and ESLint, further supporting compliance needs. Administrators now have access to detailed metrics for pull request activity and Copilot usage, making tracking and reporting more straightforward.
- [Copilot Code Review Now Available for Organization Members Without a License](https://github.blog/changelog/2025-12-17-copilot-code-review-now-available-for-organization-members-without-a-license)
- [GitHub Copilot Code Review Preview Features Now Available in Enterprise Cloud with Data Residency](https://github.blog/changelog/2025-12-18-copilot-code-review-preview-features-now-supported-in-github-enterprise-cloud-with-data-residency)
- [Enhanced Copilot Autofix Metrics for CodeQL Security Overview on GitHub](https://github.blog/changelog/2025-12-16-more-accurate-copilot-autofix-usage-metrics-on-security-overview)
- [Enterprise Pull Request Metrics Now Included in GitHub Copilot Usage Metrics API](https://github.blog/changelog/2025-12-18-enterprise-level-pull-request-activity-metrics-now-in-public-preview)
- [Track Organization Copilot Usage with Copilot Usage APIs](https://github.blog/changelog/2025-12-16-track-organization-copilot-usage)
## IDE Integration, Debugging, and Language Support
Visual Studio 2026 delivers a new debugging experience powered by Copilot for faster startup and improved diagnostics. The Debugger Agent is now integrated with test tools, creating a more consistent workflow from coding through to solution diagnosis. New C++ code editing tools for VS 2026 Insiders enhance symbol support and refactoring in multilingual environments. Copilot-driven SQL features for VS Code are now generally available, with capabilities for traditional and vector database projects.
- [Debugging, but Without the Drama: Visual Studio 2026’s Copilot-Powered Experience](https://devblogs.microsoft.com/visualstudio/visual-studio-2026-debugging-with-copilot/)
- [C++ Code Editing Tools for GitHub Copilot in Visual Studio 2026 Insiders Public Preview](https://github.blog/changelog/2025-12-16-c-code-editing-tools-for-github-copilot-in-public-preview)
- [Next-Level SQL in VS Code: GitHub Copilot GA and AI-Ready SQL](/ai/videos/next-level-sql-in-vs-code-github-copilot-ga-and-ai-ready-sql)
## Prompt Engineering and Context Management
Recent guidance helps developers apply prompt patterns—like Persona and Reflection—to refine Copilot’s support for specific use cases. Copilot can now generate dynamic prompts linked directly from GitHub documentation, bridging learning resources and AI code suggestions for a smoother workflow.
- [Cook’n with GitHub Copilot: Recap of Context Engineering Prompt Patterns](https://www.cooknwithcopilot.com/blog/context-engineering-recipes-recap.html)
- [Dynamic Copilot Prompts Now Available on GitHub Docs](https://github.blog/changelog/2025-12-17-dynamic-copilot-prompts-on-github-docs)
## Advanced Agent Capabilities and Memory
Copilot introduces early access for repository-specific memory to Pro and Pro+ users, allowing agents to retain project knowledge and reduce repeated prompts. This addition follows ongoing improvements for agent persistence and more focused support on recurring automation.
- [Early Access Release: Copilot Memory for GitHub Copilot Pro and Pro+](https://github.blog/changelog/2025-12-19-copilot-memory-early-access-for-pro-and-pro)
## Real-World Usage and Ecosystem News
Developer onboarding metrics show strong Copilot adoption. This week’s discussions reinforce the importance of AI fluency, prompt design, and open governance through standards like the Model Context Protocol. Security news—including the React2Shell vulnerability—connects to wider conversations about platform resilience and best practices.
- [The Download: React2Shell Vulnerability, GPT 5.2 in GitHub Copilot, and Open Source News](/ai/videos/the-download-react2shell-vulnerability-gpt-52-in-github-copilot-and-open-source-news)
- [A New Kind of Developer is Emerging on GitHub](/ai/videos/a-new-kind-of-developer-is-emerging-on-github)
## Effective Use of Copilot in Domain-Specific Contexts
A new resource explains how to configure Copilot for highly specialized languages and workflows. It covers using copilot-instructions.md, maintaining up-to-date reference files, and incorporating compiler validation to improve Copilot’s performance in unique environments. This pairs with community-led initiatives like DSL-Copilot and custom repository templates for real-world situations.
- [AI Coding Agents and Domain-Specific Languages: Challenges and Practical Mitigation Strategies](https://devblogs.microsoft.com/all-things-azure/ai-coding-agents-domain-specific-languages/)
## Other GitHub Copilot News
When users assign GitHub Copilot to an issue, they are now added as an assignee, further improving team transparency and workflow clarity.
- [Assigning GitHub Copilot to an Issue Now Also Assigns You](https://github.blog/changelog/2025-12-18-assigning-github-copilot-to-an-issue-now-adds-you-as-an-assignee)',
    'GitHub Copilot expands with new AI models (GPT-5.2, Claude Opus 4.5, Gemini 3 Flash), Agent Skills for reusable workflows, and Mission Control for managing automated agents—continuing its evolution into a complete developer productivity platform.',
    1766390400, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-12-22', 'TechHub',
    'TechHub', '691401457998F43753BF9F984069BC1B75259359AD8662C3AF2AFD410E624796', ',GitHub Copilot,GPT 5.2,Claude Opus 4.5,Gemini 3 Flash,AI Coding Agents,Agent Skills,Skills.md,Mission Control,Copilot Memory,Copilot Code Review,Copilot Usage API,CodeQL,ESLint,Visual Studio 2026,VS Code,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-12-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-12-15', 'roundups', 'Weekly GitHub Copilot Roundup: Models, Agents, and IDE + CLI',
    'GitHub Copilot received updates this week that improve model selection, support for agent workflows, and integration with IDEs and command line tools. The ecosystem expanded with new management features and educational resources for tasks like legacy app updates and streamlined Git workflows.
<!--excerpt_end-->
## GitHub Copilot: AI Model Expansion and Selection
Model choices continue to expand, with OpenAI’s GPT-5.2 now in public preview for Pro, Business, and Enterprise users, following last week’s release of GPT-5.1-Codex-Max and Claude Opus 4.5. Developers can benefit from better long-context and UI generation, reinforcing Copilot’s strategy to provide flexible AI model options.
User-controlled options for selecting models are now available to more users. Bring-Your-Own-Key (BYOK) builds on earlier guides, supporting both organizational control and security. Automated model selection, now available in Visual Studio Code, transitions from manual settings to routing based on workload requirements, offering transparency on which models are in use.
In addition, Copilot Coding Agent’s model switcher is now available for Pro and Pro+ subscribers, extending agent management options from last week by enabling users to select between Claude and GPT-based agents. This provides further flexibility for personalized AI assistance in development workflows.
- [OpenAI’s GPT-5.2 Now Available in GitHub Copilot Public Preview](https://github.blog/changelog/2025-12-11-openais-gpt-5-2-is-now-in-public-preview-for-github-copilot)
- [Auto Model Selection in GitHub Copilot Now Available in Visual Studio Code](https://github.blog/changelog/2025-12-10-auto-model-selection-is-generally-available-in-github-copilot-in-visual-studio-code)
- [Model Picker Now Available for Copilot Coding Agent (Pro and Pro+ Subscribers)](https://github.blog/changelog/2025-12-08-model-picker-for-copilot-coding-agent-for-copilot-pro-and-pro-subscribers)
- [Exploring AI Models and GitHub Copilot''s Raptor Mini in VS Code](/ai/videos/exploring-ai-models-and-github-copilots-raptor-mini-in-vs-code)
## Deep Copilot Integration Across Developer Workflows
With the release of Visual Studio Code November 2025 Insiders, CLI and agent session management improved, continuing support for features introduced in earlier rounds. Copilot CLI now launches from the palette or toolbar, demonstrating registry and agent integration in action. The Agent HQ panel consolidates agent monitoring for both local and cloud environments and incorporates the recent Mission Control features for easier oversight.
Automated code review and chat capabilities in Visual Studio 2026 now help analyze code for quality and security concerns, making AI-based feedback more useful during pull requests and team collaboration. This continues progress from last week’s previews. New tutorials cover automation and agent workflows in DevOps, expanding on recent cases with detailed steps for migration, standards enforcement, and deployment automation.
- [What''s New in Visual Studio Code November 2025 Insiders Release (v1.107)](https://code.visualstudio.com/updates/v1_107)
- [Agent HQ for VS Code: Unified Agent Experience with GitHub Copilot](/ai/videos/agent-hq-for-vs-code-unified-agent-experience-with-github-copilot)
- [Streamlining Your Git Workflow with Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/streamlining-your-git-workflow-with-visual-studio-2026/)
- [Continuous Efficiency: AI-Driven Software Optimization with GitHub Agentic Workflows](https://github.blog/news-insights/policy-news-and-insights/the-future-of-ai-powered-software-optimization-and-how-it-can-help-your-team/)
- [Tackling Tech Debt with the GitHub Copilot Cloud Agent](/tackling-tech-dev-with-the-github-copilot-cloud-agent)
- [Rubber Duck Thursdays: Using GitHub Copilot Custom Agents to Address Tech Debt](/ai/videos/rubber-duck-thursdays-using-github-copilot-custom-agents-to-address-tech-debt)
- [Ship Faster: End-to-End Azure App Development with GitHub Copilot and Intelligent Agents](/ai/videos/ship-faster-end-to-end-azure-app-development-with-github-copilot-and-intelligent-agents)
## Copilot in Modernization and App Transformation
Content on application modernization builds on topics from the previous week, including refactoring, review, and migration strategies. The in-depth guide for migrating Microsoft Access illustrates how Copilot can help script and automate database migrations, reflecting agent orchestration concepts discussed earlier.
Visual Studio Code Insiders now offers a JavaScript/TypeScript App Modernizer preview, supporting interactive upgrades and extending recent automation coverage. Additional .NET modernization articles provide guidance on improving code quality, managing dependencies, and enhancing security in cloud transitions, helping reduce risk and improve modernization planning.
- [Modernizing Microsoft Access: Migrating to Node.js, OpenAPI, SQL Server, and MongoDB with GitHub Copilot](https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-to-modernise-a-microsoft-access-database-forms-vba-to-node/ba-p/4473504)
- [AI-Assisted JavaScript/TypeScript Modernizer Preview in VS Code Insiders](https://devblogs.microsoft.com/blog/jsts-modernizer-preview)
- [Modernizing Legacy Apps with GitHub Copilot and Azure](/ai/videos/modernizing-legacy-apps-with-github-copilot-and-azure)
## Enhancing CLI and Terminal Developer Environments
Copilot CLI improvements continue the progress toward more productive terminal use. The updated Windows Terminal guide complements earlier scripting resources, covering prompt customization and persistent tracking for increased efficiency.
New scripting patterns, automation for scaffolding, and tighter integration with Visual Studio Code and Azure illustrate how the CLI is evolving into a core productivity platform, continuing themes of seamless scripting and automation.
- [Making Windows Terminal Awesome with GitHub Copilot CLI](https://devblogs.microsoft.com/blog/making-windows-terminal-awesome-with-github-copilot-cli)
- [Scripting the GitHub Copilot CLI - Deep Dive](/ai/videos/scripting-the-github-copilot-cli-deep-dive)
## Copilot Ecosystem: Administration, Education, and Workflow Guides
The Copilot ecosystem now offers improved management, reporting, and educational resources. Updates include new dashboards, metrics tracking for code generation, and expanded governance. The GitHub Spark SKU changes and DPA features enhance organizational management, while Spec Kit now integrates spec-driven workflows as discussed last week.
Educational initiatives such as quantum computing learning with Copilot agents, Octoverse reports, and enablement stories extend Copilot’s reach for teams experimenting with advanced scenarios. These also reinforce onboarding and standards automation priorities introduced previously.
- [GitHub Spark: Improvements, Dedicated SKU, and DPA Coverage](https://github.blog/changelog/2025-12-10-github-spark-improvements-dpa-coverage-dedicated-sku)
- [Plan, Specify, and Implement with Spec Kit and GitHub Copilot](/ai/videos/plan-specify-and-implement-with-spec-kit-and-github-copilot)
- [Showcasing a Quantum Computing Educational Platform with Custom Copilot](/ai/videos/showcasing-a-quantum-computing-educational-platform-with-custom-copilot)
- [Balancing Speed and Quality with AI and GitHub Copilot in Development](https://github.blog/ai-and-ml/generative-ai/speed-is-nothing-without-control-how-to-keep-quality-high-in-the-ai-era/)
- [Eirini Kalliamvakou Discusses Copilot and AI Trends in the 2025 Octoverse Report](/ai/videos/eirini-kalliamvakou-discusses-copilot-and-ai-trends-in-the-2025-octoverse-report)
- [The New Identity of a Developer in the AI Era](https://github.blog/news-insights/octoverse/the-new-identity-of-a-developer-what-changes-and-what-doesnt-in-the-ai-era/)
- [How I shipped more code and products than ever before with GitHub Copilot](/ai/videos/how-i-shipped-more-code-and-products-than-ever-before-with-github-copilot)
- [Building AI Agents with VS Code, GitHub Copilot, and Azure](/ai/videos/building-ai-agents-with-vs-code-github-copilot-and-azure)
- [Gemini 3 Pro Model Launches for GitHub Copilot in Popular IDEs](https://github.blog/changelog/2025-12-12-gemini-3-pro-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Microsoft Learn MCP Server: Next-Level Copilot Integration for Developers](https://devblogs.microsoft.com/dotnet/microsoft-learn-mcp-server-elevates-development/)',
    'GitHub Copilot received updates this week that improve model selection, support for agent workflows, and integration with IDEs and command line tools. The ecosystem expanded with new management features and educational resources for tasks like legacy app updates and streamlined Git workflows.',
    1765785600, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-12-15', 'TechHub',
    'TechHub', '65A93BFA037246C99D31573527A9FA360AF4B87AEAAB1F2A3AA7036808CB3371', ',GitHub Copilot,VS Code,Visual Studio 2026,Copilot Coding Agent,Copilot CLI,Agent Workflows,Auto Model Selection,BYOK,GPT 5.2,Claude Opus 4.5,Gemini 3 Pro,Windows Terminal,App Modernization,JavaScript,TypeScript,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-12-08
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-12-08', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, Models, and Governance',
    'This week''s updates bring new custom agents, enhanced models, deeper IDE integration, and improved governance tools for developers and teams.
<!--excerpt_end-->
## GitHub Copilot Custom Agents
Custom agents are now available in Copilot, extending beyond standard code completion to streamline DevOps, security, and automation workflows. Teams can define agents in markdown and manage them inside repositories. Integration examples include PagerDuty, JFrog, and Neon. These agents, which run in the terminal, VS Code, and on GitHub.com, provide automation for specific domains and support organization-wide policies or coding standards. Tutorials such as Rubber Duck Thursdays show how to build and set up agents tailored to the needs of your team. Strong vendor integrations and accessible setup options enable flexible AI-driven automation for software pipelines.
- [Introducing Custom Agents in GitHub Copilot for Developer Workflows](https://github.blog/news-insights/product-news/your-stack-your-rules-introducing-custom-agents-in-github-copilot-for-observability-iac-and-security/)
- [Rubber Duck Thursdays: Building with Copilot Custom Agents](/ai/videos/rubber-duck-thursdays-building-with-copilot-custom-agents)
## GitHub Copilot Spaces
GitHub Copilot Spaces now support sharing via view-only links, making it easier to share documentation, reusable code, and learning materials for open-source projects. These Spaces include role-specific controls and only host user-generated content, focusing on a balance between access and security. Another feature supports adding files directly from GitHub’s code viewer, streamlining workspace creation for AI-powered changes. Updated documentation explains how Copilot Spaces can assist with debugging, planning, and onboarding while maintaining privacy and focusing on team efficiency. The features support collaborative, context-rich work for teams.
- [Accelerate Debugging with GitHub Copilot Spaces and Copilot Coding Agent](https://github.blog/ai-and-ml/github-copilot/how-to-use-github-copilot-spaces-to-debug-issues-faster/)
- [Major Updates to Copilot Spaces: Public Spaces and Code View Integration](https://github.blog/changelog/2025-12-01-copilot-spaces-public-spaces-and-code-view-support)
## Copilot Model and Chat Enhancements
Recent Copilot updates provide public preview access for OpenAI’s GPT-5.1-Codex-Max and add support for Claude Opus 4.5 in Copilot Chat, delivering expanded model choices. Copilot Pro, Business, and Enterprise users have flexible options for code generation and model selection. Copilot Chat in Visual Studio now includes web URL context, letting users reference and query current online content—helpful for questions beyond the model’s existing data. These features equip teams with stronger models, targeted responses, and more manageable AI interactions.
- [OpenAI’s GPT-5.1-Codex-Max Public Preview Release for GitHub Copilot](https://github.blog/changelog/2025-12-04-openais-gpt-5-1-codex-max-is-now-in-public-preview-for-github-copilot)
- [Claude Opus 4.5 Preview Available in GitHub Copilot Chat and IDEs](https://github.blog/changelog/2025-12-03-claude-opus-4-5-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Unlocking Developer Productivity with Copilot Chat’s New URL Context](https://devblogs.microsoft.com/visualstudio/unlocking-the-power-of-web-with-copilot-chats-new-url-context/)
## GitHub Copilot in Visual Studio 2026
The Visual Studio 2026 update boosts Copilot’s integration by adding a GitHub Cloud Agent and expanded contextual actions. Developers can now handle documentation, refactoring, and batch editing directly through Copilot’s interface. New UI features include one-click actions, smarter code search suggestions ("Did You Mean"), and improved code refactoring and hierarchy visualization for C++ projects. C++ preview enrollment is open to more users, extending Copilot’s toolkit for Visual Studio.
- [Visual Studio 2026 Released: GitHub Cloud Agent Preview and Copilot Features](https://devblogs.microsoft.com/visualstudio/visual-studio-november-update-visual-studio-2026-cloud-agent-preview-and-more/)
- [GitHub Copilot and Visual Studio 2026: November Update Highlights](https://github.blog/changelog/2025-12-03-github-copilot-in-visual-studio-november-update)
- [Enhancing C++ Development in Visual Studio 2026 with GitHub Copilot](https://devblogs.microsoft.com/visualstudio/upgrade-msvc-improve-c-build-performance-and-refactor-c-code-with-github-copilot/)
## Copilot CLI and MCP Enhancements
Building on recent work around registry and deployment, new tutorials walk through setting up a private registry on Azure API Center, so only trusted models are accessible in Copilot and VS Code. There are demonstrations for the kit-dev MCP Server CLI, including code symbol extraction, abstract syntax tree searching, and inline documentation. The guides help teams securely automate Copilot and MCP tasks using compliant workflows.
- [Locking Down MCP: Create a Private Registry on Azure API Center for GitHub Copilot and VS Code](https://devblogs.microsoft.com/all-things-azure/locking-down-mcp-create-a-private-registry-on-azure-api-center-and-enforce-it-in-github-copilot-and-vs-code/)
- [Supercharging GitHub Copilot CLI with MCP Server](/ai/videos/supercharging-github-copilot-cli-with-mcp-server)
## Copilot Agent Automation, Orchestration, and Evaluation
Step-by-step guides continue from last week’s agent orchestration materials, showing how to use Mission Control for Copilot agent assignment, prompt creation, and parallel execution. The ongoing AI Toolkit + Copilot Pet Planner series now covers agent setup, code output generation, iterative tracing, and results evaluation. Tutorials focus on reviewing trace data, comparing agents side by side, and scoring output, making agent development easier to manage.
- [How to Orchestrate Multiple GitHub Copilot Agents Using Mission Control](https://github.blog/ai-and-ml/github-copilot/how-to-orchestrate-agents-using-mission-control/)
- [Setting Up AI Toolkit and GitHub Copilot for Microsoft Foundry Projects](/ai/videos/setting-up-ai-toolkit-and-github-copilot-for-microsoft-foundry-projects)
- [Generating Agent Code Using AI Toolkit and GitHub Copilot](/ai/videos/generating-agent-code-using-ai-toolkit-and-github-copilot)
- [Creating an Agent with AI Toolkit and GitHub Copilot: Pet Planner Workshop Part 3](/ai/videos/creating-an-agent-with-ai-toolkit-and-github-copilot-pet-planner-workshop-part-3)
- [Adding Tracing to an Agent with AI Toolkit and GitHub Copilot](/ai/videos/adding-tracing-to-an-agent-with-ai-toolkit-and-github-copilot)
- [Evaluating AI Agent Output with GitHub Copilot and AI Toolkit (Pet Planner Workshop, Part 6)](/ai/videos/evaluating-ai-agent-output-with-github-copilot-and-ai-toolkit-pet-planner-workshop-part-6)
- [AI Toolkit and GitHub Copilot: Model Recommendations Workshop](/ai/videos/ai-toolkit-and-github-copilot-model-recommendations-workshop)
- [Evaluating AI Models for Coding with GitHub Models](/ai/videos/evaluating-ai-models-for-coding-with-github-models)
## Issue Assignment and Project Management Integrations
Now, issues can be assigned directly to Copilot using GraphQL/REST APIs, streamlining automation for code review, triage, and routing CI/CD workflows. Teams can set up custom agent directions and use Copilot with Linear’s issue tracker for automatic code or pull request generation, expanding integrations with other tools.
- [Assign Issues to GitHub Copilot Using the API](https://github.blog/changelog/2025-12-03-assign-issues-to-copilot-using-the-api)
- [Assigning Linear Issues to GitHub Copilot Coding Agent](/ai/videos/assigning-linear-issues-to-github-copilot-coding-agent)
## Administration, Auditing, and Code Generation Metrics
New governance features allow organizations to see more code generation metrics with Copilot Insights Dashboard, breaking down activity by model, user, trigger, and language. Metrics can now be exported, and the Control Panel now provides a unified location for managing agent access, permissions, and audit logs. Better audit trails support secure deployments and help organizations meet compliance requirements.
- [Track Copilot Code Generation Metrics in GitHub Insights Dashboard](https://github.blog/changelog/2025-12-05-track-copilot-code-generation-metrics-in-a-dashboard)
- [Managing and Auditing GitHub Copilot Agents: Insights and Governance Tools](/ai/videos/managing-and-auditing-github-copilot-agents-insights-and-governance-tools)
## Advanced Copilot Use Cases: Code Review, Performance Profiling, HPC Automation
Pull request review integration now includes automated and custom review features, with CodeQL static analysis. Visual Studio 2026’s Profiler Agent enables natural-language performance analysis using BenchmarkDotNet for .NET projects. For high-performance computing, Copilot helps automate Slurm jobs via GPT-5-based models, reducing manual scripting in scientific workflows.
- [Accelerating Pull Request Reviews with GitHub Copilot Code Review](/ai/videos/accelerating-pull-request-reviews-with-github-copilot-code-review)
- [Optimizing .NET Performance with Copilot Profiler Agent in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/delegate-the-analysis-not-the-performance/)
- [Automating HPC Workflows with Copilot Agents](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/automating-hpc-workflows-with-copilot-agents/ba-p/4472610)
## Copilot Studio Intelligent Agent Development
A Microsoft Ignite session highlighted ways to develop advanced Copilot Studio agents using Microsoft Graph, Azure AI Search, and Active Directory. Teams can use connectors and business logic to filter documents and analyze information, supporting enterprise automation and aligning with Microsoft security standards.
- [Building Intelligent Agents with Copilot Studio and Advanced Knowledge Sources](/ai/videos/building-intelligent-agents-with-copilot-studio-and-advanced-knowledge-sources)',
    'This week''s updates bring new custom agents, enhanced models, deeper IDE integration, and improved governance tools for developers and teams.',
    1765180800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-12-08', 'TechHub',
    'TechHub', '5EE284428F3ED310A7AF251716398073B72B0F00D7EAF56F65C32D916C17C3F9', ',GitHub Copilot,Custom Agents,Copilot Spaces,Copilot Chat,Visual Studio 2026,GPT 5.1 Codex Max,Claude Opus 4.5,Copilot Insights Dashboard,Governance,Audit Logs,MCP,Copilot CLI,GitHub API,GraphQL,Code Review,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-12-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-12-01', 'roundups', 'Weekly GitHub Copilot Roundup: New Models, Agents, Governance',
    'This week, GitHub Copilot introduces updated AI models, workflow enhancements, and more automation tools for developers. These developments expand on recent improvements around model management, Visual Studio 2026 integration, and responsible practices for agent-based development. The latest features include support for Anthropic’s newest model, enhanced low-code controls in Copilot Studio, and strategies to ensure responsible use of agentic AI in software projects. Community events and step-by-step tutorials provide developers with more ways to tailor Copilot inside tools like VS Code. These changes highlight Copilot’s adaptability for enterprise-scale environments, local code projects, and specific automation needs.
<!--excerpt_end-->
## GitHub Copilot AI Models and Integration
Continuing last week’s launch of new model controls and previews, GitHub Copilot now offers a public preview of Anthropic’s Claude Opus 4.5 for users on Pro, Pro+, Business, and Enterprise subscriptions. Users can pick Claude Opus 4.5 in Copilot Chat across supported platforms, giving more model flexibility, better coding suggestions, and reduced token costs. Admins for enterprises get new policy settings to manage access, building on last week’s updates for organizational oversight. The release is rolling out in stages and user feedback is being collected to shape future features and model support.
- [Claude Opus 4.5 Public Preview Launches for GitHub Copilot](https://github.blog/changelog/2025-11-24-claude-opus-4-5-is-in-public-preview-for-github-copilot)
## Copilot in the Visual Studio Family
After last week’s updates on Visual Studio 2026 and Copilot-driven workflows, Ignite 2025 featured deeper Copilot integration for smarter code suggestions and improved review experiences. The new Profiler Agent for diagnostics continues the trend of embedding Copilot features into daily development, helping individuals and teams boost productivity. Ongoing monthly updates and interface improvements make sure Copilot aligns with the latest .NET and business requirements, keeping pace with prior Visual Studio releases.
- [First Look at Visual Studio 2026: Fast, Modern, and AI-Powered](/ai/videos/first-look-at-visual-studio-2026-fast-modern-and-ai-powered)
## Copilot in Microsoft Foundry and Code-First Workflows
Building on last week’s spotlight on Agent Mode in VS Code, this week provides a detailed guide for moving Microsoft Foundry workflows into a code-first editing environment. The Model Mondays tutorial series continues, with stepwise instructions for configuring, editing, and testing workflows powered by AI. This underlines Copilot’s local agent features, continuing its commitment to code-first, self-managed AI development—an approach highlighted in prior news and tutorials on workflow improvement.
- [Microsoft Foundry Workflows: Migrating to Code-First Development in VS Code](/ai/videos/microsoft-foundry-workflows-migrating-to-code-first-development-in-vs-code)
## Low-Code Agent Building and Automation with Copilot Studio
Copilot Studio’s progress, highlighted last week with expanded no-code options, continued at Ignite 2025 with the announcement of an embedded builder and a more flexible enterprise platform. These updates add advanced knowledge base management, automated workflow testing, and deeper links to Microsoft 365 Copilot. Earlier case studies on automation are still useful as new sessions focus on deploying agent flows in larger environments with an emphasis on security, credential management, and transparent operations—supporting broader business automation.
Ignite sessions also explored automation and workflow expansion using Copilot Studio, emphasizing security and scalability for both individual and organizational use. These resources outline best practices for integrating Copilot Studio across platforms, meeting ongoing needs for governance and real-world deployment of complex agent flows.
- [Copilot Studio Innovations and Roadmap: Building Low-Code AI Agents (BRK313)](/ai/videos/copilot-studio-innovations-and-roadmap-building-low-code-ai-agents-brk313)
- [Automation in Copilot Studio: Agent Flows and Computer Use](/ai/videos/automation-in-copilot-studio-agent-flows-and-computer-use)
## Advanced AI-Assisted Coding Models and Custom Copilot Agents
Furthering recent discussions on extensibility, this week’s live sessions focus on orchestrating Copilot models and setting up advanced agent workflows in VS Code. Using new standards like agents.md and tools such as Ruler, developers now have clearer options for delegating tasks like coding and evaluation. Recent tutorials on prompt engineering, command line tools, and voice integrations build on past guides, keeping Copilot customizable and shaped by the development community. Showcases for building custom agents highlight Copilot’s flexibility for specific workflows in various areas.
- [Taming AI Assisted Coding Models with Eleanor Berger](/ai/videos/taming-ai-assisted-coding-models-with-eleanor-berger)
- [Building Custom Agents for Copilot on Rubber Duck Thursdays](/ai/videos/building-custom-agents-for-copilot-on-rubber-duck-thursdays)
## Responsible and Sustainable Software with Copilot and Agentic DevOps
Guidance around responsible AI remains a steady theme, as Ignite sessions this week featured experts from Cognizant and Microsoft on managing agentic AI and scaling Copilot responsibly within organizations. Discussions covered how to build more sustainable software, connecting with previous conversations about measurable green workflows. Developers can find new tools and resources for driving ethical and sustainability-focused Copilot adoption in their projects.
- [Building and Deploying Responsible Agentic AI with Microsoft Copilot](/ai/videos/building-and-deploying-responsible-agentic-ai-with-microsoft-copilot)
- [Building Sustainable Software with Agentic DevOps and GitHub Copilot](/ai/videos/building-sustainable-software-with-agentic-devops-and-github-copilot)',
    'This week, GitHub Copilot introduces updated AI models, workflow enhancements, and more automation tools for developers. These developments expand on recent improvements around model management, Visual Studio 2026 integration, and responsible practices for agent-based development. The latest features include support for Anthropic’s newest model, enhanced low-code controls in Copilot Studio, and strategies to ensure responsible use of agentic AI in software projects. Community events and step-by-step tutorials provide developers with more ways to tailor Copilot inside tools like VS Code. These changes highlight Copilot’s adaptability for enterprise-scale environments, local code projects, and specific automation needs.',
    1764576000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-12-01', 'TechHub',
    'TechHub', '860600B008CEF27B7218783C23AE9855079FC9438B786F4ADAFEBA95F06C899B', ',GitHub Copilot,Copilot Chat,Anthropic Claude Opus 4.5,Model Selection,Enterprise Policy Controls,Visual Studio 2026,.NET,VS Code,Agent Mode,Microsoft Foundry,Copilot Studio,Low Code Agents,Automation Testing,Custom Agents,Responsible AI,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-11-24
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-11-24', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, Controls, and More IDEs',
    'Building on the previous week’s updates in agent design, model selection, workflow automation, and IDE compatibility, GitHub Copilot has progressed with new features and integrations for IDEs, cloud platforms, enterprise controls, and agent workflows. These enhancements add support for more developer environments and management tools, bringing practical gains in productivity, security, and code quality as Copilot’s features become more policy-driven and context aware.
<!--excerpt_end-->
## Agentic Automation and IDE/Cloud Integrations
Copilot’s agent capabilities have grown, with recent improvements in Mission Control and updated experiences in Visual Studio and VS Code now joined by Ignite’s announcements. App modernization powered by Copilot agents is now available for JetBrains, Eclipse, and Xcode, adding to existing support in Visual Studio and VS Code. For developers using Visual Studio, .NET, and Azure, Copilot now provides automation for migration and containerization tasks, expanding from basic workflow help to actual application modernization.
The Copilot CLI now supports the latest models (like OpenAI GPT-5.1 and Gemini 3.5 Pro), building on last week’s features in code search and context. Eclipse users can now use Copilot’s coding agents, a continuation of the rollout seen for VS Code and JetBrains. Migration assessment is now connected to Copilot’s agent features, reinforcing prior improvements in policy enforcement and organizational controls.
- [AI Agents Accelerate App Modernization with GitHub Copilot and Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/ai-agents-are-rewriting-the-app-modernization-playbook/ba-p/4470162)
- [GitHub Copilot CLI Introduces New AI Models, Enhanced Code Search, and Improved Image Support](https://github.blog/changelog/2025-11-18-github-copilot-cli-new-models-enhanced-code-search-and-better-image-support)
- [GitHub Copilot Coding Agent for Eclipse Now in Public Preview](https://github.blog/changelog/2025-11-18-github-copilot-coding-agent-for-eclipse-now-in-public-preview)
- [GitHub Copilot Isolated Subagents Now in Public Preview for JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-11-18-isolated-subagents-for-jetbrains-eclipse-and-xcode-now-in-public-preview)
- [How to Assign and Manage Copilot Agent Tasks from Anywhere](/ai/videos/how-to-assign-and-manage-copilot-agent-tasks-from-anywhere)
## Intelligent Code Suggestion, Planning, and Test Automation
Copilot has enhanced its predictive editing and planning with new features that build on last week’s inline chat and session management in VS Code. Next Edit Suggestions (NES) are now in public preview for Xcode and Eclipse, expanding coverage beyond VS Code and Visual Studio and moving toward similar functionality across all environments. NES adapts suggestions to align better with user intent, moving beyond basic code completion.
Test automation with Copilot is now available for .NET in Visual Studio 2026 Insiders, marking progress from purely manual reviews to integrated test generation and automation. Agent-based planning features are now available in JetBrains, VS Code, Xcode, and Eclipse, following the recent addition of organizational instruction and review tools.
- [Enhancing GitHub Copilot’s Next Edit Suggestions with Custom Model Training](https://github.blog/ai-and-ml/github-copilot/evolving-github-copilots-next-edit-suggestions-through-custom-model-training/)
- [GitHub Copilot Next Edit Suggestions (NES) Public Preview for Xcode and Eclipse](https://github.blog/changelog/2025-11-18-github-copilot-next-edit-suggestions-nes-now-in-public-preview-for-xcode-and-eclipse)
- [Supercharge Your Test Coverage with GitHub Copilot Testing for .NET](https://devblogs.microsoft.com/dotnet/github-copilot-testing-for-dotnet/)
- [Plan Mode in GitHub Copilot Now Available in Public Preview for JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-11-18-plan-mode-in-github-copilot-now-in-public-preview-in-jetbrains-eclipse-and-xcode)
- [Using the Plan Agent in VS Code for Step-by-Step Task Planning](/ai/videos/using-the-plan-agent-in-vs-code-for-step-by-step-task-planning)
## Enterprise Controls, Model Flexibility, and Security
Continuing from new administrative options last week, this update introduces BYOK (Bring Your Own Key) and broader MCP allowlisting. These features allow enterprises to use their own LLM API keys and define which backend servers developers may connect to, increasing Copilot’s suitability for regulated environments.
Enhanced usage metrics permissions support better tracking of Copilot use and investment. Updated security guides now cover SIEM integration and advanced anomaly detection to provide clear ways for organizations to baseline and review Copilot activity. Authentication improvements across JetBrains, Eclipse, and Xcode further streamline onboarding in managed setups.
- [Internal MCP Registry and Allowlist Controls for Copilot in VS Code and Visual Studio](https://github.blog/changelog/2025-11-18-internal-mcp-registry-and-allowlist-controls-for-vs-code-stable-in-public-preview)
- [Enterprise BYOK for GitHub Copilot Now in Public Preview](https://github.blog/changelog/2025-11-20-enterprise-bring-your-own-key-byok-for-github-copilot-is-now-in-public-preview)
- [Fine-Grain Permissions for GitHub Copilot Usage Metrics Released](https://github.blog/changelog/2025-11-17-fine-grain-permissions-for-copilot-usage-metrics-now-available)
- [Setting Up Security Alerts for Unusual GitHub Copilot Activity](https://dellenny.com/setting-up-alerts-for-unusual-github-copilot-activity/)
- [Enhanced MCP OAuth Support for GitHub Copilot Plugins in JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-11-18-enhanced-mcp-oauth-support-for-github-copilot-in-jetbrains-eclipse-and-xcode)
## Customization and Agent Management Across Development Teams
Applying what was learned about custom instructions and team workflows, Copilot’s agent customization and isolated subagent features are now public for JetBrains, Eclipse, and Xcode, enabling clearer workflow division. The agents.md guide has added input from 2,500 repositories, building on prior documentation and guidance for multi-agent setups.
New video tutorials cover assigning and monitoring agent tasks across multiple platforms, addressing practical workflow management as highlighted previously. These additions make Copilot’s automation easier to adopt for teams with diverse technology stacks.
- [Custom Agents in GitHub Copilot for JetBrains, Eclipse, and Xcode Now in Public Preview](https://github.blog/changelog/2025-11-18-custom-agents-available-in-github-copilot-for-jetbrains-eclipse-and-xcode-now-in-public-preview)
- [GitHub Copilot Isolated Subagents Now in Public Preview for JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-11-18-isolated-subagents-for-jetbrains-eclipse-and-xcode-now-in-public-preview)
- [How to Write a Great agents.md: Lessons from 2,500 GitHub Repositories](https://github.blog/ai-and-ml/github-copilot/how-to-write-a-great-agents-md-lessons-from-over-2500-repositories/)
- [How to Assign and Manage Copilot Agent Tasks from Anywhere](/ai/videos/how-to-assign-and-manage-copilot-agent-tasks-from-anywhere)
## Modernization, Migration, and DevOps Integration
Building on prior coverage of Visual Studio 2026 and .NET Aspire, Copilot’s Agent Mode now automates many aspects of .NET app migration and legacy modernization. The shift from free tools to subscription models has prompted discussion of costs and continuity for developers and organizations.
Integration between Azure DevOps and Copilot strengthens automation for project management and security across coding workflows. Agent-driven DevOps guides and dashboards help reinforce the practical approach detailed previously.
- [Modernizing .NET Applications with GitHub Copilot Agent Mode: A Step-by-Step Guide](https://devblogs.microsoft.com/dotnet/modernizing-dotnet-with-github-copilot-agent-mode/)
- [Migrating .NET Framework Apps with GitHub Copilot in Visual Studio: Developer Feedback and Licensing Changes](https://devclass.com/2025/11/20/copilot-net-modernization-tool-a-huge-downgrade-devs-say-and-no-longer-free/)
- [Azure DevOps and GitHub Repositories: Unlocking Agentic AI for Developer Teams](https://devblogs.microsoft.com/devops/azure-devops-and-github-repositories-next-steps-in-the-path-to-agentic-ai/)
- [AI-Powered Hybrid DevOps with GitHub Copilot and Azure DevOps](/ai/videos/ai-powered-hybrid-devops-with-github-copilot-and-azure-devops)
- [Modernize Your Apps in Days with AI Agents in GitHub Copilot](/ai/videos/modernize-your-apps-in-days-with-ai-agents-in-github-copilot)
- [From Legacy to Modern .NET on Azure with Visual Studio 2026, Azure App Service, and GitHub Copilot](/ai/videos/from-legacy-to-modern-net-on-azure-with-visual-studio-2026-azure-app-service-and-github-copilot)
## Productivity, Code Quality, and Workflow Best Practices
Updates to Copilot dashboards, analytics, and guides on prompt engineering build on earlier productivity themes. Keyboard shortcut and command reference tutorials help streamline Copilot Chat in practical contexts. Tips and best practices for integrating Copilot into test-driven development, code reviews, and static analysis reinforce proven approaches for reliable automation.
Security and workflow recommendations point to Copilot’s role alongside linters and other guards, a repeat point from recent discussions on building automation that still requires human oversight for safety.
- [Top GitHub Copilot Shortcuts and Productivity Tips for VS Code](https://dellenny.com/turbocharge-your-coding-top-github-copilot-shortcuts-and-productivity-tips-for-vs-code/)
- [Your Guide to Debugging and Reviewing Copilot-Generated Code](https://dellenny.com/your-guide-to-debugging-and-reviewing-copilot-generated-code/)
- [Your Guide to Debugging and Reviewing Copilot-Generated Code](https://techcommunity.microsoft.com/t5/tools/your-guide-to-debugging-and-reviewing-copilot-generated-code/m-p/4472116#M182)
- [Best Practices for Coding with GitHub Copilot in .NET](/ai/videos/best-practices-for-coding-with-github-copilot-in-net)
- [Using the Cognitive Verifier Pattern with GitHub Copilot](https://www.cooknwithcopilot.com/blog/context-engineering-recipes-the-cognitive-verifier-pattern.html)
## AI Model Choice, Embedding-Guided Tooling, and Collaborative Development
Auto model selection and controls, previewed last week, are now available in JetBrains, Xcode, and Eclipse, supporting smarter project-specific automation. Gemini 3 Pro is now in public preview, joining GPT-5.1 and Codex, broadening choice for developers.
Updates to embedding-guided tooling and routing in VS Code further extend Copilot’s ability to select the right tool for context. Copilot Spaces now aggregates context from multiple files and repositories, improving overall automation.
- [Auto Model Selection for GitHub Copilot in JetBrains, Xcode, and Eclipse](https://github.blog/changelog/2025-11-18-auto-model-selection-for-copilot-in-jetbrains-ides-xcode-and-eclipse-in-public-preview)
- [Gemini 3 Pro Model Now Available in GitHub Copilot Public Preview](https://github.blog/changelog/2025-11-18-gemini-3-pro-is-in-public-preview-for-github-copilot)
- [How GitHub Copilot Uses Embedding-Guided Tool Routing in VS Code](https://github.blog/ai-and-ml/github-copilot/how-were-making-github-copilot-smarter-with-fewer-tools/)
- [How Copilot Spaces gives your AI the right project context](/ai/videos/how-copilot-spaces-gives-your-ai-the-right-project-context)
## AI-Enhanced Code Quality, Review Workflows, and Developer Collaboration
Linter support in Copilot’s code review toolkit continues earlier efforts around CodeQL, agent review features, and better control for team leads. Language-aware analysis builds on efforts for robust organization-level quality review.
Recent sessions at GitHub Universe and Ignite add case studies and guidance focused on developer productivity and automation across the SDLC. Coverage on MCP-backed policy and context management links to previous enterprise-level updates.
- [Linter Integration Arrives in Copilot Code Review Public Preview](https://github.blog/changelog/2025-11-20-linter-integration-with-copilot-code-review-now-in-public-preview)
- [Scaling Code Quality in the Age of AI](/ai/videos/scaling-code-quality-in-the-age-of-ai)
- [Redefining the SDLC with GitHub Copilot and Context-Driven AI](/ai/videos/redefining-the-sdlc-with-github-copilot-and-context-driven-ai)
- [Reimagining Software Development with GitHub Copilot and AI Agents](/ai/videos/reimagining-software-development-with-github-copilot-and-ai-agents)
## Copilot for Data, Natural Language Automation, and Operations
Building on recent automation coverage, Copilot now brings automation to data work. Copilot and Query Editor for SQL Database on Microsoft Fabric move to general availability, expanding Copilot''s reach into database tasks. Natural language pipeline authoring in Fabric Data Factory continues the drive for context-powered automation from app development into data engineering.
Integration with Azure DevOps, including PagerDuty and Datadog, keeps the focus on practical end-to-end DevOps automation.
- [Copilot and Query Editor Now Generally Available in SQL Database on Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/copilot-and-query-editor-in-sql-database-in-fabric-ga-update/)
- [Natural Language to Generate and Explain Pipeline Expressions with Copilot (Preview)](https://blog.fabric.microsoft.com/en-US/blog/preview-natural-language-to-generate-and-explain-pipeline-expressions-with-copilot/)
- [Copilot-Assisted Real-Time Data Exploration in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/copilot-assisted-real-time-data-exploration-preview/)
- [Ship Faster with Azure and GitHub Copilot: End-to-End DevOps with AI Agents](/ai/videos/ship-faster-with-azure-and-github-copilot-end-to-end-devops-with-ai-agents)
## Other GitHub Copilot News
Further updates to developer tools follow last week’s introduction of the Raptor Mini Model and improved session management. The Download video transitions tools like Gemini 3 Pro to general release and presents demonstrations of Git 2.52 jetpack and Agent 365, highlighting Copilot’s growing ecosystem.
Additional resources support Copilot adoption, code review, and debugging, ensuring developers remain current as Copilot evolves. These tools help teams maintain quality and productivity as they bring automation and AI deeper into daily development.
- [The Download: Git 2.52, Gemini 3, GitHub Copilot Updates & Agent 365](/ai/videos/the-download-git-252-gemini-3-github-copilot-updates-and-agent-365)
Additional resources have been shared to help teams adjust to Copilot’s expanding features and agent-based automation, from debugging guides to new feedback channels. These tools will be key for organizations standardizing AI-powered workflows.',
    'Building on the previous week’s updates in agent design, model selection, workflow automation, and IDE compatibility, GitHub Copilot has progressed with new features and integrations for IDEs, cloud platforms, enterprise controls, and agent workflows. These enhancements add support for more developer environments and management tools, bringing practical gains in productivity, security, and code quality as Copilot’s features become more policy-driven and context aware.',
    1763971200, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-11-24', 'TechHub',
    'TechHub', '466B1566648EF4F5F6AF42F85F64BF4B310ED4D9C6DFDDD722555D0C58AA48BB', ',GitHub Copilot,AI Agents,Copilot Agent Mode,Copilot CLI,VS Code,VS,JetBrains,Eclipse IDE,Xcode,.NET,Azure DevOps,MCP,BYOK,SIEM,Microsoft Fabric,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-11-17
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-11-17', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, IDE Upgrades, Model Choice',
    'This week’s GitHub Copilot coverage highlights expanded IDE features, maturing agent systems, new AI model options, improved enterprise features, and hands-on guidance for workflow optimization. As Copilot integrates deeper into Visual Studio, VS Code, and cloud environments, AI-based automation becomes a continuous part of daily developer tasks. Updates this week address infrastructure needs—such as security, model control, and agent configuration—while providing practical improvements for code editors, review workflows, and .NET automation. The summary below breaks down the news by technology area.
<!--excerpt_end-->
## GitHub Copilot Integrations in Visual Studio and VS Code
Building from recent advances in unified AI and agent processes, Visual Studio 2026 now features Copilot embedded for .NET 10, C#, and C++. Enhanced speed, an upgraded interface, and Copilot-powered suggestions streamline coding tasks. Regular monthly updates, continued extension compatibility, and simple upgrades from VS2022 remain consistent with trends since last November.
VS Code 1.106 continues to serve as an “AI-integrated” editor, featuring Copilot Chat with a centralized Agent HQ, more robust chats, and greater safety controls. Features such as inline chat v2 and updated session views further develop the multi-agent user experience, solidifying VS Code as a primary choice for AI-centered workflows.
Guides this week address Godot prototyping, Avalonia UI test automation, playlist-based debugging, and .NET workflow automation. Updates to C++ editing and build analysis showcase Copilot’s multi-agent capabilities—responding to the progress seen with JetBrains and Visual Studio agents last week.
- [Visual Studio 2026: Faster Performance and AI-Driven Developer Tools Now Available](https://devblogs.microsoft.com/visualstudio/visual-studio-2026-is-here-faster-smarter-and-a-hit-with-early-adopters/)
- [AI-Powered Development with GitHub Copilot in Visual Studio](/ai/videos/ai-powered-development-with-github-copilot-in-visual-studio)
- [Visual Studio Code October 2025 Release (v1.106): New AI Agents, Copilot Chat, Terminal IntelliSense & More](https://code.visualstudio.com/updates/v1_106)
- [Visual Studio Code and GitHub Copilot - What''s new in 1.106](/ai/videos/visual-studio-code-and-github-copilot-whats-new-in-1106)
- [Godot for C# Developers: Leveraging AI and GitHub Copilot for Prototyping](/ai/videos/godot-for-c-developers-leveraging-ai-and-github-copilot-for-prototyping)
- [Building Rock-Solid Avalonia Apps: A Guide to Headless Testing with AI Assistance](/ai/videos/building-rock-solid-avalonia-apps-a-guide-to-headless-testing-with-ai-assistance)
- [Transforming CI Failures into Focused Debugging with Visual Studio Playlists and AI](/ai/videos/transforming-ci-failures-into-focused-debugging-with-visual-studio-playlists-and-ai)
- [AI-Powered Development with GitHub Copilot in Visual Studio](/ai/videos/ai-powered-development-with-github-copilot-in-visual-studio)
- [Refactor C++ Code with C++ Code Editing Tools for GitHub Copilot in Visual Studio](/ai/videos/refactor-c-code-with-c-code-editing-tools-for-github-copilot-in-visual-studio)
- [GitHub Copilot Build Performance Analysis in Visual Studio 2026](/ai/videos/github-copilot-build-performance-analysis-in-visual-studio-2026)
## GitHub Copilot Agent Architecture and Workflow Automation
Recent updates in Copilot agents follow last week’s advances in agent modes, CLI updates, and organizational control options. The introduction of Mission Control and Agent Sessions in VS Code allows developers to manage both automated and human-guided workflows, with features like complete logging and rationale tracking for smoother transitions between manual and automated work. Assigning responsibilities such as tech debt reduction to Copilot agents extends prior work on organization-wide instructions and pull request templates.
Setting up Copilot agents as bypass actors for GitHub rulesets brings stricter policy enforcement, while updates to `.instructions.md` and the new `excludeAgent` setting allow for more detailed customization and enhanced compliance.
- [VS Code as an AI-Native Editor: Insights from GitHub Universe 2025](/ai/videos/vs-code-as-an-ai-native-editor-insights-from-github-universe-2025)
- [Tackling Your Tech Debt with Copilot Coding Agent](/ai/videos/tackling-your-tech-debt-with-copilot-coding-agent)
- [Manage Copilot Coding Agent Tasks in Visual Studio Code](https://github.blog/changelog/2025-11-13-manage-copilot-coding-agent-tasks-in-visual-studio-code)
- [How to Configure Copilot Coding Agent as a Bypass Actor for GitHub Rulesets](https://github.blog/changelog/2025-11-13-configure-copilot-coding-agent-as-a-bypass-actor-for-rulesets)
- [Agent-Specific Custom Instructions Now Supported in Copilot Code Review and Coding Agent](https://github.blog/changelog/2025-11-12-copilot-code-review-and-coding-agent-now-support-agent-specific-instructions)
## Advanced AI Model Selection and Management
Building on last week’s additions to admin controls over AI models, Copilot now introduces public preview support for OpenAI GPT-5.1 and updated Codex models. Users can choose models manually or automatically for better visibility and improved organizational policy management. Claude Sonnet 3.5 is being replaced by Claude Haiku 4.5 for Copilot Free users, and Raptor mini is in public preview, offering diverse choices to fit varying developer needs.
- [OpenAI’s GPT-5.1 Family Now Available for GitHub Copilot Users](https://github.blog/changelog/2025-11-13-openais-gpt-5-1-gpt-5-1-codex-and-gpt-5-1-codex-mini-are-now-in-public-preview-for-github-copilot)
- [Introducing Copilot Auto Model Selection (Preview)](https://devblogs.microsoft.com/visualstudio/introducing-copilot-auto-model-selection-preview/)
- [Auto Model Selection for GitHub Copilot in Visual Studio: Public Preview](https://github.blog/changelog/2025-11-11-auto-model-selection-for-copilot-in-visual-studio-in-public-preview)
- [Claude Sonnet 3.5 Deprecated, Claude Haiku 4.5 Now Available in GitHub Copilot Free](https://github.blog/changelog/2025-11-10-claude-sonnet-3-5-deprecated-claude-haiku-4-5-available-in-copilot-free)
- [Raptor Mini Model Public Preview Available in GitHub Copilot](https://github.blog/changelog/2025-11-10-raptor-mini-is-rolling-out-in-public-preview-for-github-copilot)
## Copilot-Powered Code Review and Quality Automation
Extending the recent improvements in code review automation, Copilot now features integration with CodeQL for smarter pull request suggestions, combining AI-driven insights with manual instructions. Agent handoff, GitHub Actions automation, and expanded organization-level review settings represent ongoing progress in code quality management. Support for agent-specific review instructions supports customized workflows.
- [What''s New with GitHub Copilot Code Review: CodeQL, Agents & More](/ai/videos/whats-new-with-github-copilot-code-review-codeql-agents-and-more)
- [Mastering Copilot Code Review: Writing Effective Instructions Files](https://github.blog/ai-and-ml/unlocking-the-full-power-of-copilot-code-review-master-your-instructions-files/)
- [Agent-Specific Custom Instructions Now Supported in Copilot Code Review and Coding Agent](https://github.blog/changelog/2025-11-12-copilot-code-review-and-coding-agent-now-support-agent-specific-instructions)
## Copilot for .NET, C#, and Modern Desktop
Copilot now extends its automation skills to .NET, C#, and desktop environments. Agents support platforms like Windows Forms and .NET Aspire, continuing progress from Java and alluding to the wider role of multi-agent orchestration in modern workflows. New guides show how Copilot assists with architectural documentation and .NET modernization, supporting the wider move towards “living documentation” and cloud-native approaches for .NET teams.
- [What''s New in Windows Forms: Modern Enhancements and AI Integration](/ai/videos/whats-new-in-windows-forms-modern-enhancements-and-ai-integration)
- [Automating .NET Aspire Architecture Documentation with GitHub Copilot](/ai/videos/automating-net-aspire-architecture-documentation-with-github-copilot)
## Enterprise, Security, and Administration
Continuing the expansion of enterprise controls and onboarding flexibility, recent updates help organizations deploy Copilot more transparently. New privacy, intellectual property, and compliance guidance complements organizational policies and budget features, now with support for `.copilotignore` and enhanced terminal protection. Expanded video tutorials provide clear steps for onboarding, SSO configuration, billing setup, and permissions management, supporting larger team requirements.
- [Demystifying GitHub Copilot Security Controls for Organizations](https://techcommunity.microsoft.com/t5/microsoft-developer-community/demystifying-github-copilot-security-controls-easing-concerns/ba/p/4468193)
- [Tutorial Videos: Setting up GitHub Copilot for Your Company](https://devblogs.microsoft.com/all-things-azure/tutorial-videos-setting-up-github-copilot-for-your-company/)
## Developer Productivity, Guidance, and Practical Workflows
Developers gain new prompt engineering guides, detailed tutorials for context prompts, and more workflow customization strategies. Improved tools now support better writing, refactoring, testing, and modernization workflows. The updated Copilot Metrics dashboard introduces advanced analytics for adoption and ROI, replacing older approaches and supporting data-driven deployment.
Case studies on internal Copilot usage at GitHub demonstrate productivity improvements in review, configuration management, and rollout processes. This highlights the practical benefits of Copilot when paired with clear, intentional routines.
- [Prompt Engineering Techniques for Developers Using GitHub Copilot](https://dellenny.com/prompt-engineering-for-developers-getting-the-best-out-of-copilot/)
- [Practical Use Cases: Writing, Refactoring, and Testing Code with GitHub Copilot](https://dellenny.com/practical-use-cases-writing-refactoring-and-testing-code-with-github-copilot/)
- [Transforming GitHub Copilot Metrics into Business Value](/ai/videos/transforming-github-copilot-metrics-into-business-value)
- [How Copilot Helps Build the GitHub Platform](https://github.blog/ai-and-ml/github-copilot/how-copilot-helps-build-the-github-platform/)
- [Your codebase, your rules: Customizing Copilot with context engineering](/ai/videos/your-codebase-your-rules-customizing-copilot-with-context-engineering)',
    'This week’s GitHub Copilot coverage highlights expanded IDE features, maturing agent systems, new AI model options, improved enterprise features, and hands-on guidance for workflow optimization. As Copilot integrates deeper into Visual Studio, VS Code, and cloud environments, AI-based automation becomes a continuous part of daily developer tasks. Updates this week address infrastructure needs—such as security, model control, and agent configuration—while providing practical improvements for code editors, review workflows, and .NET automation. The summary below breaks down the news by technology area.',
    1763366400, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-11-17', 'TechHub',
    'TechHub', 'EB243CBA4EFC96D0C48B4D0B0566DCB0DA64CA51AC29524F03CACE7A4B0178B4', ',GitHub Copilot,Visual Studio 2026,VS Code,Copilot Chat,AI Agents,Agent Sessions,Mission Control,Model Selection,GPT 5.1,Codex,Claude Haiku 4.5,Code Review,CodeQL,.NET 10,Enterprise Security,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-11-10
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-11-10', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, IDE Unification, Governance',
    'This week, GitHub Copilot introduces new agent workflow tools, a unified extension for popular IDEs, and expanded enterprise policy and budgeting controls. Developers benefit from enhanced orchestration in VS Code and Visual Studio, increased automation options, collaboration improvements, and updated certification guides. With more granular enterprise administration, organizations can better align Copilot usage with internal requirements. These additions continue to advance Copilot as a key resource for streamlined, AI-supported development.
<!--excerpt_end-->
## GitHub Copilot in Editors: Unified Extension, JetBrains Integration, and Visual Studio Advancements
Copilot now consolidates all AI features—including inline suggestions, chat, and agent mode—into the open source Copilot Chat extension for VS Code 1.105+. The approach builds on centralized agent management, providing a more integrated developer experience and encouraging community contributions. VS Code improvements continue with new platform plans outlined at Universe.
Support for Copilot in JetBrains IDEs grows with better Model Context Protocol (MCP) integration. Demos highlight agent mode capabilities for automating planning, troubleshooting, and dialog, resulting in improved IDE context awareness and agent assistance.
Visual Studio''s November AI roadmap brings extensions for automated testing, debugging agents, and advanced governance, evolving its planning mode. Multi-agent and chat enhancements help move Copilot towards thorough agent-driven development within both cloud and local IDE environments.
- [VS Code Unifies Copilot AI Features in Open Source Extension](https://code.visualstudio.com/blogs/2025/11/04/openSourceAIEditorSecondMilestone)
- [GitHub Copilot in JetBrains: Demo of MCP and Agent Mode](/ai/videos/github-copilot-in-jetbrains-demo-of-mcp-and-agent-mode)
- [Visual Studio AI Roadmap: Copilot Chat, Agents, and Model Integrations (November)](https://devblogs.microsoft.com/visualstudio/roadmap-for-ai-in-visual-studio-november/)
- [A Unified Experience for All Coding Agents in VS Code](https://code.visualstudio.com/blogs/2025/11/03/unified-agent-experience)
- [Behind the Scenes of VS Code’s Planning Agent](/ai/videos/behind-the-scenes-of-vs-codes-planning-agent)
## Copilot Agent Mode, CLI, and Workflow Automation
Copilot''s automation capabilities advance with updates to the CLI and workflow tools. A recently published guide provides step-by-step usage for Mission Control and Agent Mode in VS Code and GitHub, simplifying tasks such as testing, refactoring, and documentation.
Enhancements to the CLI facilitate secure, flexible agent workflows, covering installation, trust configuration, and interactive automation. These updates align with Microsoft Learn MCP servers, improved batch editing, and team automation. Copilot Coding Agent now supports pull request templates and organization-wide custom instructions, building on last week''s customizable agent workflows.
- [GitHub Copilot: Modern AI Coding Workflows, Mission Control, and Best Practices](https://github.blog/ai-and-ml/github-copilot/a-developers-guide-to-writing-debugging-reviewing-and-shipping-code-faster-with-github-copilot/)
- [GitHub Copilot CLI 101: How to use GitHub Copilot from the command line](https://github.blog/ai-and-ml/github-copilot-cli-101-how-to-use-github-copilot-from-the-command-line/)
- [Copilot Coding Agent Now Supports Pull Request Templates](https://github.blog/changelog/2025-11-05-copilot-coding-agent-now-supports-pull-request-templates)
- [Copilot Coding Agent Adds Organization-Wide Custom Instructions](https://github.blog/changelog/2025-11-05-copilot-coding-agent-supports-organization-custom-instructions)
## Enhancing Pull Request Reviews and Collaboration
Recent features in Copilot strengthen collaboration by enabling batch commits, collapsible CI annotations, and grouped pull request suggestions. These tools advance progress in automated reviews and multi-agent teamwork. Improvements in merge interfaces and accessibility further support AI-assisted code reviews for enterprise and Pro+ users.
- [GitHub Pull Request Files Changed Page Update with Copilot Grouping and Merge Experience Improvements](https://github.blog/changelog/2025-11-06-pull-request-files-changed-public-preview-and-merge-experience-november-6-updates)
## Enterprise-Grade Controls: Policy, Delegation, and Budget Management
Copilot now provides more granular enterprise controls for managing access and budgets. The default ''Unconfigured'' policy enhances governance by increasing administrator monitoring and workflow security. Agent controls and delegated policy management in IDEs offer greater compliance flexibility, while budget tracking for Copilot and Spark builds on cost management tools from earlier releases.
- [GitHub Copilot Policy Update for Unconfigured Enterprise Policies](https://github.blog/changelog/2025-11-04-github-copilot-policy-update-for-unconfigured-policies)
- [GitHub Copilot Policy Adds Agent Mode Controls for IDE](https://github.blog/changelog/2025-11-03-github-copilot-policy-now-supports-agent-mode-in-the-ide)
- [Delegating AI and Copilot Controls in GitHub Enterprises](https://github.blog/changelog/2025-11-03-delegate-ai-controls-management-to-members-of-your-enterprise)
- [Control AI Spending with Budget Tracking for GitHub AI Tools](https://github.blog/changelog/2025-11-03-control-ai-spending-with-budget-tracking-for-github-ai-tools)
## Certification, Exam Resources, and Developer Guides
Expanded Copilot certification resources are now available, featuring a detailed exam blueprint and official study materials. These materials supplement previous exam preparation, providing structured paths that emphasize responsible AI development, privacy, and workflow integration.
- [Understanding the GitHub Copilot Exam: Blueprint, Skills, and Key Domains](https://dellenny.com/understanding-the-github-copilot-exam-blueprint-skills-measured-topics-covered/)
- [Free & Official Learning Resources for the GitHub Copilot Certification Exam](https://dellenny.com/free-official-learning-resources-for-the-github-copilot-certification-exam/)
## Building AI-Driven and Modernized Applications with Copilot
Guides for application modernization and AI-based workflows continue from last week, focusing on Java upgrades, CI/CD automation, and review tools. The Copilot App Modernization tool and Azure Developer CLI now offer easier provisioning and deployment. Fresh resources for creating multi-agent AI applications in VS Code carry forward improvements in orchestrating cloud-native agents for scalable and observable solutions.
- [Modernize Java Apps with AI: Deploy Your Applications to Azure](/ai/videos/modernize-java-apps-with-ai-deploy-your-applications-to-azure)
- [Building Scalable AI Apps and Agents with VS Code, GitHub Copilot, and Agent Framework](/ai/videos/building-scalable-ai-apps-and-agents-with-vs-code-github-copilot-and-agent-framework)
## Copilot Studio: Performance, Debugging, and Bot Reliability
Copilot Studio benefits from the recent move to .NET 8 and WebAssembly, cutting bot load times and build cycles. Coverage on debugging and telemetry continues, supporting teams in building and maintaining stable, automated bots—a continuation of reliability themes discussed in previous weeks.
- [How Copilot Studio Uses .NET and WebAssembly for Performance and Innovation](https://devblogs.microsoft.com/dotnet/copilot-studio-dotnet-wasm/)
- [Debugging and Testing Your Copilot Studio Bots Efficiently](https://dellenny.com/debugging-and-testing-your-copilot-studio-bots-efficiently/)
## Best Practices in Prompt Engineering and Workflow Customization
A new tutorial introduces the "Refusal Breaker" prompt pattern, offering teams actionable techniques for boosting Copilot output while staying within compliance and responsible AI guidelines.
- [Context Engineering Recipes: The Refusal Breaker Pattern for GitHub Copilot](https://www.cooknwithcopilot.com/blog/context-engineering-recipes-the-refusal-breaker-pattern.html)
## Developer Impact, Workflow Trends, and Usage Reporting
Recent studies further demonstrate Copilot’s improvements in development time and workflow quality, reflecting previous Octoverse reporting. Enhanced activity and analytics reports now replace legacy usage CSVs for enterprise management, maintaining continuity in activity tracking from earlier previews.
- [How AI Code Assistants Save Developers Thousands of Hours](https://devops.com/how-ai-code-assistants-can-save-1000-years-of-developer-time/)
- [The AI-Powered Evolution of Software Development](https://devops.com/the-ai-powered-evolution-of-software-development/)
- [The Legacy Copilot Usage Report CSV Is No Longer Available](https://github.blog/changelog/2025-11-05-the-legacy-copilot-usage-report-csv-is-no-longer-available)',
    'This week, GitHub Copilot introduces new agent workflow tools, a unified extension for popular IDEs, and expanded enterprise policy and budgeting controls. Developers benefit from enhanced orchestration in VS Code and Visual Studio, increased automation options, collaboration improvements, and updated certification guides. With more granular enterprise administration, organizations can better align Copilot usage with internal requirements. These additions continue to advance Copilot as a key resource for streamlined, AI-supported development.',
    1762761600, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-11-10', 'TechHub',
    'TechHub', '07AFAB0CA72716021E4CC0556BE4CF026DC12216540707EF13E448A6B0D77A30', ',GitHub Copilot,Agent Mode,VS Code,VS,JetBrains,Copilot Chat Extension,MCP,Copilot CLI,Workflow Automation,Pull Requests,Code Review,Enterprise Policies,Budget Tracking,Copilot Studio,.NET 8,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-11-03
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-11-03', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, Control Planes, Code Quality',
    'GitHub Copilot and its broader ecosystem received attention this week with new features, integrations, enterprise management capabilities, and practical use cases. Copilot now emphasizes coding agents, better agent management interfaces, and updated AI-driven code quality, automation, and developer productivity. Announcements from GitHub Universe 2025 and ongoing platform updates mark a transition from code suggestions to the adoption of integrated development agents across IDEs, cloud environments, and collaboration tools. This supports the development of agent-enabled workflows, agent orchestration, and code automation for both individuals and organizations.
<!--excerpt_end-->
## Copilot Coding Agent and Agent Management
GitHub Copilot extends its purpose beyond line-by-line code suggestions, functioning as a coding agent with centralized management and integration options. The new Mission Control interface is a unified resource for assigning and monitoring Copilot agent tasks across github.com, VS Code Insiders, Codespaces, CLI, and mobile. This setup enables improved oversight and operational transparency for agent activities.
Agent HQ and Mission Control further the orchestration capabilities, offering support for both GitHub-native and third-party agents (OpenAI, Google, Anthropic, xAI), bringing together different AI systems. Features like @copilot PR mentions and support for self-hosted runners with ARC focus on secure agent workflows and improving integration with organizational infrastructure.
Copilot’s expanding collaboration with platforms such as Linear and Slack demonstrates ongoing efforts to enable workflow automation and issue resolution outside the core coding process. The azd extension’s managed identity and MCP configuration delivers enhancements in Azure authentication and integration for development teams.
Enterprise AI Controls and the public preview of Agent Control Plane give administrators new tools to manage agents, control policy, and monitor usage—supporting wider adoption of agent-centric features in large organizations.
- [A Mission Control for Managing Copilot Coding Agent Tasks on GitHub](https://github.blog/changelog/2025-10-28-a-mission-control-to-assign-steer-and-track-copilot-coding-agent-tasks)
- [Introducing Agent HQ Mission Control for GitHub Copilot](/ai/videos/introducing-agent-hq-mission-control-for-github-copilot)
- [Ask Copilot Coding Agent to Make Changes in Any Pull Request with @copilot](https://github.blog/changelog/2025-10-28-ask-copilot-coding-agent-to-make-changes-in-any-pull-request-with-copilot)
- [Copilot Coding Agent Now Supports Self-Hosted Runners Using ARC](https://github.blog/changelog/2025-10-28-copilot-coding-agent-now-supports-self-hosted-runners)
- [GitHub Copilot Coding Agent for Linear Enters Public Preview](https://github.blog/changelog/2025-10-28-github-copilot-for-linear-available-in-public-preview)
- [Use GitHub Copilot Coding Agent with Slack to Generate Pull Requests](https://github.blog/changelog/2025-10-28-work-with-copilot-coding-agent-in-slack)
- [Integrating GitHub Copilot Coding Agent with Azure Using the azd Extension](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-copilot-coding-agent-config/)
- [Enterprise AI Controls and Agent Control Plane Public Preview for GitHub Copilot](https://github.blog/changelog/2025-10-28-enterprise-ai-controls-the-agent-control-plane-are-in-public-preview)
## Feature Expansions: Planning, Code Review, and Custom Agents
Building on last week’s addition of planning modes and code review capabilities, these features are now available in public preview for Visual Studio and VS Code. The planning mode helps teams break down complex, multi-step engineering tasks, especially for larger projects, supported by new models such as GPT-5 and Claude Haiku 4.5. These tools offer a smooth shift from guided workflow management to more advanced, AI-driven planning.
Copilot Code Review now incorporates LLM feedback along with traditional static analysis tools (CodeQL, ESLint), continuing the effort to combine AI insights with deterministic analysis to support secure, maintainable code. The use of @copilot mentions for PR changes supports collaborative workflows and teamwork across agent-driven reviews.
The release of custom agents for .NET, including C# Expert and WinForms Expert, delivers platform-specific agents for code upgrades, recommended practices, and reducing repetitive setup tasks.
Workflow customization using copilot-instructions.md and the introduction of Visual Studio memory features build on recent improvements to agent contextualization, helping teams create consistent and efficient workflows.
- [Visual Studio Copilot Gets Planning Mode for Complex Tasks](https://devops.com/visual-studio-copilot-gets-planning-mode-for-complex-tasks/)
- [Introducing Plan Mode: Build Better Plans with GitHub Copilot](/ai/videos/introducing-plan-mode-build-better-plans-with-github-copilot)
- [New Public Preview Features in Copilot Code Review: Smarter, AI-Driven Code Reviews](https://github.blog/changelog/2025-10-28-new-public-preview-features-in-copilot-code-review-ai-reviews-that-see-the-full-picture)
- [Custom Agents for .NET Developers: C# Expert and WinForms Expert](https://devblogs.microsoft.com/dotnet/introducing-custom-agents-for-dotnet-developers-csharp-expert-winforms-expert/)
- [Visual Studio October 2025 Update: Copilot Memories, Custom Instructions, and Azure Foundry Integration](https://devblogs.microsoft.com/visualstudio/visual-studio-october-update/)
## GitHub Copilot Ecosystem at GitHub Universe
Announcements from GitHub Universe 2025 reinforce the move toward a connected agent platform. The confirmation of Agent HQ builds on growing themes of modular agent management and third-party integration. Mission Control and Plan Mode, now officially released, anchor the platform''s agent collaboration and workflow tracking features.
The AI Toolkit for VS Code (v4.0 preview) adds prompt-first agent development, orchestration, tracing, and evaluation for both single- and multi-agent systems. These functions expand the toolkit’s utility for diverse developer tasks, while integration with Microsoft Agent Framework continues the focus on agent orchestration.
Universe sessions featured the use of Copilot and the Agent Framework in building intelligent, cloud-native applications within VS Code. Updates in domain-specific model selection and workflow automation add useful tools for daily developer use.
Advances in MCP integration, cloud operations, and isolated sub-agents for processes like TDD and code research expand on previous technical deep dives.
- [GitHub Universe 2025 Day 1 Recap: Announcements and New Features](/devops/videos/github-universe-2025-day-1-recap-announcements-and-new-features)
- [GitHub Universe Day 1 Keynote Recap: Agent HQ, Mission Control, and Custom Agents](/devops/videos/github-universe-day-1-keynote-recap-agent-hq-mission-control-and-custom-agents)
- [GitHub Universe 2025: AI-Driven Developer Innovation Takes Center Stage](https://azure.microsoft.com/en-us/blog/github-universe-2025-where-developer-innovation-took-center-stage/)
- [Public Preview: AI Toolkit for GitHub Copilot Brings Prompt-First Agent Development to VS Code](https://techcommunity.microsoft.com/t5/microsoft-developer-community/announcing-public-preview-ai-toolkit-for-github-copilot-prompt/ba-p/4465069)
- [From Idea to Production: Building Intelligent Cloud-Native Apps with VS Code, GitHub Copilot, and Microsoft Agent Framework](https://devblogs.microsoft.com/blog/behind-the-universe-demo-with-vs-code-copilot-and-agent-framework)
- [GitHub Copilot in Visual Studio Code Upgraded with OpenAI Codex and New Agent Features](https://github.blog/changelog/2025-10-28-github-copilot-in-visual-studio-code-gets-upgraded)
- [OpenAI Codex Now Available in VS Code with GitHub Copilot Pro+](/ai/videos/openai-codex-now-available-in-vs-code-with-github-copilot-pro)
## AI-Driven Code Quality, Review, and Modernization
GitHub Code Quality is now in public preview, delivering instant PR feedback and autofix in enterprise repositories using CodeQL-based rules. Direct feedback helps reduce technical debt and provides actionable insights. Copilot’s autofix feature drives automated code improvements and helps standardize the review process.
Updates for app modernization bring new tools for Java upgrades, AWS-to-Azure migration, dependency management, and secure C++ transitions with MSVC migration tools. These updates support a continuous shift from maintaining legacy compatibility to developing with current, secure standards.
Smarter code review, integrating AI-driven suggestions and static analysis, automates more of the review process and reduces manual work.
- [GitHub Code Quality Public Preview: Inline Findings and Copilot Fixes](https://github.blog/changelog/2025-10-28-github-code-quality-in-public-preview)
- [AI-Assisted Modernization and Cloud Migration of Legacy Java Applications with GitHub Copilot](/ai/videos/ai-assisted-modernization-and-cloud-migration-of-legacy-java-applications-with-github-copilot)
- [Upgrade MSVC with GitHub Copilot App Modernization for C++](/ai/videos/upgrade-msvc-with-github-copilot-app-modernization-for-c)
## Copilot Coding Agent: Expanding Roles and Use Cases
The Copilot Coding Agent is now more deeply integrated with GitHub workflows, independently handling issues, triage, and solution proposals. This automation streamlines routine maintenance and project management, following a growing pattern of more connected workflow tools.
New guides and demos illustrate the agent’s daily use, sharing practical benefits and productivity data.
- [Introduction to GitHub Copilot Coding Agent](/ai/videos/introduction-to-github-copilot-coding-agent)
- [Exploring GitHub Copilot Coding Agent: Beyond Code Suggestions](/ai/videos/exploring-github-copilot-coding-agent-beyond-code-suggestions)
## Copilot in Context: Language Trends, Ecosystem Growth, and Analytics
The Octoverse 2025 report offers more analytics on the rise of TypeScript and Copilot use, extending the latest coverage on usage and adoption metrics. TypeScript continues to lead, with Copilot used by 80% of new developers in their first week. Growth in AI repositories and dashboard activity underscores a trend toward data-driven development and optimization in organizations.
- [Octoverse 2025: AI Adoption and TypeScript Rise Drive Unprecedented Developer Growth on GitHub](https://github.blog/news-insights/octoverse/octoverse-a-new-developer-joins-github-every-second-as-ai-leads-typescript-to-1/)
- [GitHub Octoverse 2025: AI, Copilot, and the Rise of TypeScript in Software Development](https://www.linkedin.com/posts/satyanadella_i-love-how-easy-its-becoming-to-learn-on-activity-7389085159972593664-d87n)
- [Copilot Usage Metrics Dashboard and API in Public Preview for GitHub Enterprise](https://github.blog/changelog/2025-10-28-copilot-usage-metrics-dashboard-and-api-in-public-preview)
## Copilot Agent Technical Deep Dives: MCP Integration and Evaluation
Expanded best practices cover Copilot integration with the Model Context Protocol (MCP), building on recent technical articles. Tutorials focus on setting up MCP in Java projects, automating API scaffolding, and validating applications, moving toward more advanced use.
Offline MCP Server evaluation pipelines now provide a way to benchmark Copilot’s reliability and performance, reflecting ongoing interest in robust offline validation and iterative dataset evaluation.
- [MCP and Java Apps: Building a Server](/ai/videos/mcp-and-java-apps-building-a-server)
- [Building MCP Clients: Java Integration and GitHub Copilot Use Cases](/ai/videos/building-mcp-clients-java-integration-and-github-copilot-use-cases)
- [Measuring What Matters: Offline Evaluation of GitHub MCP Server](https://github.blog/ai-and-ml/generative-ai/measuring-what-matters-how-offline-evaluation-of-github-mcp-server-works/)
## Promoting Code Quality and Workflow Best Practices
Continued guidance focuses on code quality, prompt engineering, and effective Copilot use. Articles on reflection pattern, context engineering, and chaining prompts provide new approaches to prompt strategy and optimization.
AI-driven game design and hardware hack projects show how Copilot can be used for creative learning as well as engineering work. Resources include preparation guides for the Copilot certification exam and highlights from university events, promoting skill building and learning verification.
- [Writing Cleaner Code with GitHub Copilot Suggestions](https://dellenny.com/writing-cleaner-code-with-github-copilot-suggestions/)
- [Context Engineering for Java Ecosystem](/ai/videos/context-engineering-for-java-ecosystem)
- [Understanding AI Prompt Engineering: Writing Better Requests for GitHub Copilot](https://dellenny.com/understanding-ai-prompts-writing-better-requests-for-copilot/)
- [Context Engineering Recipes: The Reflection Pattern for GitHub Copilot](https://www.cooknwithcopilot.com/blog/context-engineering-recipes-the-reflection-pattern.html)
- [Building a 2D Platformer with Spec Kit, VS Code, and GitHub Copilot](/ai/videos/building-a-2d-platformer-with-spec-kit-vs-code-and-github-copilot)
- [How GitHub Copilot Hacked a Furby](/ai/videos/how-github-copilot-hacked-a-furby)
- [How to Register and Prepare for the GitHub Copilot Certification Exam](https://dellenny.com/how-to-register-and-prepare-for-the-github-copilot-exam-step-by-step-guide/)
- [Sprint to Imagine Cup: Igniting Innovation on Campus](https://techcommunity.microsoft.com/blog/studentdeveloperblog/sprint-to-imagine-cup-igniting-innovation-on-campus/4463230)
- [Collapsing the Distance from Idea to Impact with GitHub Copilot and AI-Powered Development](/ai/videos/collapsing-the-distance-from-idea-to-impact-with-github-copilot-and-ai-powered-development)',
    'GitHub Copilot and its broader ecosystem received attention this week with new features, integrations, enterprise management capabilities, and practical use cases. Copilot now emphasizes coding agents, better agent management interfaces, and updated AI-driven code quality, automation, and developer productivity. Announcements from GitHub Universe 2025 and ongoing platform updates mark a transition from code suggestions to the adoption of integrated development agents across IDEs, cloud environments, and collaboration tools. This supports the development of agent-enabled workflows, agent orchestration, and code automation for both individuals and organizations.',
    1762156800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-11-03', 'TechHub',
    'TechHub', '11B0AE036B4DE9422A6B8A199C2C50BB62DC223D38A0AB61A969458DF5B2879D', ',GitHub Copilot,Copilot Coding Agent,Agent HQ,Mission Control,Plan Mode,Copilot Code Review,GitHub Code Quality,Enterprise AI Controls,Agent Control Plane,VS Code,VS,Codespaces,MCP,CodeQL,OpenAI Codex,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-10-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-10-27', 'roundups', 'Weekly GitHub Copilot Roundup: Faster Models and Agentic Workflows',
    'This week’s GitHub Copilot updates focus on increased speed, better model accuracy, model management features, and new workflow improvements. The developer community saw technical advances and practical guides for deploying tailored AI models, greater extension support in VS Code, and deeper analysis of Copilot’s place in real-world development. Tutorials and feature highlights show how Copilot is used both as a coding assistant and a link for agentic workflows, cloud migration, documentation help, and creative development.
<!--excerpt_end-->
## Speed, Model Accuracy, and Copilot Integration Updates
Building on the recent rollout of GPT-4.1 and Claude Sonnet 4.5, GitHub made further advances in model training and benchmarking. Fine-tuning and custom reinforcement learning for fill-in-the-middle tasks enhance context-sensitive results. These improvements led to a 20% increase in accepted code, 12% higher suggestion acceptance, and higher speed, positioning Copilot as a multi-model platform for intelligent automation. The updates are now available in Copilot-compatible IDEs, offering developers faster, more automated workflows.
The October VS Code AI Toolkit update (v0.24.0) tightens Copilot''s editor integration, allowing Copilot Tools to be used directly within VS Code. New features like the Agent Evaluation Planner and Runner simplify analysis of metrics and results, reducing tool-switching and supporting context-driven tasks as mentioned in previous roundups.
- [Building a Faster, Smarter GitHub Copilot with Custom Models](https://github.blog/ai-and-ml/github-copilot/the-road-to-better-completions-building-a-faster-smarter-github-copilot-with-a-new-custom-model/)
- [AI Toolkit for VS Code October Update: GitHub Copilot Tools Integration](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-toolkit-for-vs-code-october-update/ba-p/4463365)
## Model Choice and Agentic AI Platform Extensions
Increasing options for multi-model support and agentic workspaces, VS Code now provides Bring Your Own Key (BYOK) and the Language Model Chat Provider API. This opens up new choices for enterprise and open-source models such as Hugging Face, Ollama, and Cerebras, in addition to existing support for OpenAI, Claude Sonnet 4.5, and Grok Code Fast 1. Support for modular AI assistance keeps growing; note that code completion is not yet available for BYOK.
Copilot’s Planning mode preview in Visual Studio 2022 offers hierarchical, editable prompt plans with models including GPT-5 and Claude Sonnet 4. User feedback on live editing continues to drive improvements in agentic workflows.
Copilot is retiring older Claude, OpenAI, and Gemini models, continuing the changes discussed in previous updates. Teams should update to newer models to ensure continued compatibility, and broader Claude Haiku 4.5 support (now in all Copilot plans and IDEs) helps tailor workflow fit.
- [Expanding Language Model Choice in VS Code with Bring Your Own Key and New API](https://code.visualstudio.com/blogs/2025/10/22/bring-your-own-key)
- [Introducing Planning in Visual Studio: Copilot Agent Mode Public Preview](https://devblogs.microsoft.com/visualstudio/introducing-planning-in-visual-studio-public-preview/)
- [GitHub Copilot Deprecates Legacy Claude, OpenAI, and Gemini Models](https://github.blog/changelog/2025-10-23-selected-claude-openai-and-gemini-copilot-models-are-now-deprecated)
- [Claude Haiku 4.5 Now Available in GitHub Copilot Across All Supported IDEs](https://github.blog/changelog/2025-10-20-claude-haiku-4-5-is-generally-available-in-all-supported-ides)
## Copilot Workflow Guides, Advanced Usage, and MCP Integration
New guides on prompt engineering and context management, such as the Persona Pattern guide, build on last week’s information regarding prompt versioning and .prompt.md usage. The focus remains on accuracy and productivity, and aligns with recent best practices.
Copilot’s integration into agentic AI workflows for updating legacy Java and .NET applications continues the shift toward system upgrades and PowerShell automation. Real-world examples, automated upgrades, and Infrastructure-as-Code template generation mark Copilot’s increasing role in hybrid cloud migration, consistent with previous orchestration discussion.
MCP server extensions for VS Code provide faster documentation and quick database deployments, demonstrating better support for external data and APIs. This continues the evolution of secure, domain-specific suggestions featured in last week’s roundup.
A new GitHub Copilot Certification article reinforces responsible use, privacy, and test automation, extending earlier discussions on training, community education, and actual feature usage in development.
- [Context Engineering Recipes: Using the Persona Pattern with GitHub Copilot](https://www.cooknwithcopilot.com/blog/context-engineering-recipes-persona-pattern.html)
- [AI & GitHub Copilot: Modernizing Legacy Apps for the Cloud](/ai/videos/ai-and-github-copilot-modernizing-legacy-apps-for-the-cloud)
- [Rapid Database Integration in VS Code with Microsoft Learn MCP Server and GitHub Copilot](/ai/videos/rapid-database-integration-in-vs-code-with-microsoft-learn-mcp-server-and-github-copilot)
- [Adding a Database to Your App with GitHub Copilot and Microsoft Docs MCP Server](/ai/videos/adding-a-database-to-your-app-with-github-copilot-and-microsoft-docs-mcp-server)
- [What is Model Context Protocol (MCP) in GitHub Copilot?](/ai/videos/what-is-model-context-protocol-mcp-in-github-copilot)
- [What Is the GitHub Copilot Certification and Why It Matters for Developers](https://dellenny.com/what-is-the-github-copilot-certification-and-why-it-matters-for-developers/)
## Copilot in Live Coding, Community, and Creative Use Cases
GitHub Universe and the ‘For the Love of Code’ hackathon highlighted Copilot’s integration into practical events and creative open-source development, continuing the story from previous weeks. Live coding in VS Code provided chances to interact directly with tool creators and get actionable advice, further building on MCP community involvement.
Showcases of open-source projects using Copilot demonstrated both accessibility and unique deployment scenarios, following last week’s ‘No Bark’ case study. Tutorials on building a DJ app illustrated Copilot’s value for creative coding and prompt-driven experiments.
- [VS Code Live Coding with GitHub Copilot at GitHub Universe](/ai/videos/vs-code-live-coding-with-github-copilot-at-github-universe)
- [VS Code Live at GitHub Universe: Live Coding with Copilot](/ai/videos/vs-code-live-at-github-universe-live-coding-with-copilot)
- [For the Love of Code: GitHub Hackathon Winners Showcase Creative Projects with Copilot](https://github.blog/open-source/from-karaoke-terminals-to-ai-resumes-the-winners-of-githubs-for-the-love-of-code-challenge/)
- [Live-Coding a DJ App with VS Code and GitHub Copilot](/ai/videos/live-coding-a-dj-app-with-vs-code-and-github-copilot)
## Contextual AI and Copilot Highlights in Documentation
Copilot Highlights in Microsoft Learn documentation are refining delivery methods. Building on last week’s introduction of AI-powered guidance and step reasoning, this update supports technical writers and developers with more practical, example-oriented workflows.
Tutorials for maintaining community health files now include Copilot-supported automation for ongoing compliance and iterative updating. These enhancements help to reduce manual maintenance and improve prompt strategies for better context assistance.
- [AI-Powered Enhancements in Microsoft Learn Developer Docs with Copilot Highlights](/ai/videos/ai-powered-enhancements-in-microsoft-learn-developer-docs-with-copilot-highlights)
- [Streamline Community Health Files with AI and GitHub Copilot](https://github.blog/ai-and-ml/github-copilot/how-to-update-community-health-files-with-ai/)
## Advanced Usage, Education, and Developer Adaptation
New education and advanced usage guides build on last week''s podcast discussions and agent memory content. Revisions now add details for chat agent configuration and GPT-4.1 setup in VS Code. Fresh podcasts and articles continue the series on optimizing Copilot workflows.
Reviews of how Copilot affects software engineer and student workflows extend last week’s focus on AI agents, productivity, and project speed using MCP and Copilot CLI. Automatic documentation, test creation, and code review for large codebases continue to be central, promoting updates in curriculum and organizational adoption as Copilot gains traction.
- [Mastering Chat Modes in VS Code with Burke Holland](/ai/videos/mastering-chat-modes-in-vs-code-with-burke-holland)
- [How Software Engineers and Students Use AI to Move Faster than Ever (without breaking things)](https://devops.com/how-software-engineers-and-students-use-ai-to-move-faster-than-ever-without-breaking-things/)
- [Your 6-Step Guide to Deploying a Website with GitHub Codespaces and Copilot Agent Mode](https://devblogs.microsoft.com/all-things-azure/your-6-step-guide-to-deploying-a-website-with-github-codespaces-and-copilot-agent-mode/)',
    'This week’s GitHub Copilot updates focus on increased speed, better model accuracy, model management features, and new workflow improvements. The developer community saw technical advances and practical guides for deploying tailored AI models, greater extension support in VS Code, and deeper analysis of Copilot’s place in real-world development. Tutorials and feature highlights show how Copilot is used both as a coding assistant and a link for agentic workflows, cloud migration, documentation help, and creative development.',
    1761552000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-10-27', 'TechHub',
    'TechHub', '4A0028A65A24FDE3BA10CF6E83AE23CAD564F08D317B2A84D03B2F345EC0B470', ',GitHub Copilot,VS Code,Visual Studio 2022,AI Toolkit,GPT 4.1,GPT 5,Claude Sonnet 4.5,Claude Haiku 4.5,Model Benchmarking,Custom Models,Fine Tuning,Reinforcement Learning,BYOK,Language Model Chat Provider API,MCP,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-10-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-10-20', 'roundups', 'Weekly GitHub Copilot Roundup: Multi-Model, Agents, and Automation',
    'GitHub Copilot rolled out additional integrations and improvements this week, furthering its shift from a code completion tool to an AI platform designed for developers. Enhancements reinforce recent releases (such as Grok Code Fast 1 and Claude Sonnet 4.5), strengthening Copilot’s use of multiple models, support for more IDEs, and automation features. Copilot now contributes to a broader range of developer workflows, including Pull Request creation, PowerShell scripting, legacy system updates, and Model Context Protocol (MCP) agent support.
<!--excerpt_end-->
## Core GitHub Copilot Developments and Integrations
With last week’s Grok Code Fast 1 release and IDE model selector updates, Copilot’s multi-model system now better supports switching between OpenAI, Claude Sonnet 4.5, and Grok Code Fast 1. Claude Sonnet 4.5 is generally available across all Copilot plans, and Grok Code Fast 1 has expanded from preview to full support in GitHub.com, mobile apps, VS Code, JetBrains, Xcode, and Eclipse.
A new preview brings Copilot into SQL Server Management Studio 22, supporting T-SQL code suggestions and troubleshooting in line with earlier workflow additions. Commit message generation, previously in beta, is now broadly available, adding to Copilot’s growing automation features. Security functions like Autofix and AI-driven code review continue the focus on addressing vulnerabilities. Guidance is available for those migrating from deprecated Copilot knowledge bases to Copilot Spaces, detailing the migration timeline to support organizational planning.
- [GitHub Copilot: From Autocomplete to Multi-Model AI Coding Assistant](https://github.blog/ai-and-ml/github-copilot/copilot-faster-smarter-and-built-for-how-you-work-now/)
- [Claude Sonnet 4.5 Now Available in GitHub Copilot](https://github.blog/changelog/2025-10-13-anthropics-claude-sonnet-4-5-is-now-generally-available-in-github-copilot)
- [Grok Code Fast 1 Now Available in GitHub Copilot](https://github.blog/changelog/2025-10-16-grok-code-fast-1-is-now-generally-available-in-github-copilot)
- [Introducing GitHub Copilot Integration in SQL Server Management Studio 22](/ai/videos/introducing-github-copilot-integration-in-sql-server-management-studio-22)
- [Copilot-Generated Commit Messages Now Generally Available on GitHub.com](https://github.blog/changelog/2025-10-15-copilot-generated-commit-messages-on-github-com-are-generally-available)
- [Migrating Copilot Knowledge Bases to Copilot Spaces](https://github.blog/changelog/2025-10-17-copilot-knowledge-bases-can-now-be-converted-to-copilot-spaces)
## GitHub Copilot CLI and Agentic Workflows
Copilot CLI now includes updates aimed at terminal and AI-native workflows, resulting in easier onboarding and improved support for Git operations using global installation and clear permissions. The multiline input feature, introduced earlier, now offers more flexible interaction for developers.
Integration with Claude Haiku 4.5 and MCP server updates provides richer command handling and stable session management, contributing to better context management. PowerShell scripting capability has reached stable status in response to requests for effective cross-platform support.
Interest in open-source MCP projects is growing, with new frameworks and workflow automation gaining traction as more developers adopt AI-driven strategies through CLI and VS Code.
- [Getting Started with GitHub Copilot CLI in the Terminal](https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-how-to-get-started/)
- [GitHub Copilot CLI: Multiline Input, MCP Enhancements, and Haiku 4.5 Release](https://github.blog/changelog/2025-10-17-copilot-cli-multiline-input-new-mcp-enhancements-and-haiku-4-5)
- [9 Open Source MCP Projects Advancing AI-Native Developer Workflows](https://github.blog/open-source/accelerate-developer-productivity-with-these-9-open-source-ai-and-mcp-projects/)
## GitHub Copilot Integration in Visual Studio Code
Copilot’s integration with Visual Studio Code has expanded, building on recent agent features. Merge conflict resolution is now assisted by Copilot, providing input on both code branches.
Agent mode now enforces use of fully qualified tool names, aligning with the MCP protocol and registry improvements. The new Extensions Marketplace preview makes it easier to find MCP server backends. Copilot features display step-by-step reasoning and improved tooling for managing workflow approvals and navigation.
VS Code updates include enhanced keyboard shortcuts, system-aware profile detection, and stronger integration with test suites, all based on developer feedback. Collaboration continues to make agentic workflows smoother within the editor.
- [Visual Studio Code and GitHub Copilot - What''s new in 1.105](/ai/videos/visual-studio-code-and-github-copilot-whats-new-in-1105)
## Copilot Coding Agent and Automation Features
The Copilot coding agent now features web search for gathering error details and documentation, extending prior troubleshooting updates. Asynchronous features allow for draft pull requests and review requests that do not require constant oversight from developers.
Naming conventions for branches and pull requests have been refined, improving workflow clarity. Policy settings are thoroughly documented so organizations can control integration and meet compliance goals.
- [Copilot Coding Agent Can Now Search the Web](https://github.blog/changelog/2025-10-16-copilot-coding-agent-can-now-search-the-web)
- [Copilot Coding Agent Improves Branch Naming and Pull Request Titles](https://github.blog/changelog/2025-10-16-copilot-coding-agent-uses-better-branch-names-and-pull-request-titles)
## Model Updates and Prompt Engineering Best Practices
GitHub Copilot now uses the GPT-4.1 code completion model, improving suggestion context and accuracy. This continues the trend of upgrading models by phasing out older versions like Claude Sonnet 3.5.
Prompt engineering guidance encourages version control and team review, with prompts stored in .prompt.md and copilot-instructions.md files. Treating prompts as maintainable components integrates AI assistance directly into existing development practices.
- [GPT-4.1 Copilot Code Completion Model – October Update](https://github.blog/changelog/2025-10-17-gpt-4-1-copilot-code-completion-model-october-update)
- [Treat Your AI Prompts Like Code](https://www.cooknwithcopilot.com/blog/treat-your-ai-prompts-like-code.html)
## Specialized Workflows: Testing, PowerShell, and Mainframe Modernization
Copilot’s ability to generate test suites in VS Code using prompts improves on previous automated testing features for Playwright and Jupyter. PowerShell automation now leverages Copilot Chat for Microsoft 365 and Azure, building on advances in CLI and agent workflows.
For mainframe modernization, Copilot and agent frameworks work alongside Azure orchestration to support updating legacy COBOL systems.
- [Generate a Test Suite with GitHub Copilot and Prompt-Driven Development](/ai/videos/generate-a-test-suite-with-github-copilot-and-prompt-driven-development)
- [Automating PowerShell Scripts with GitHub Copilot Chat for SysAdmins](https://dellenny.com/copilot-for-sysadmins-automating-powershell-script-generation-from-plain-english-prompts/)
- [How GitHub Copilot and AI Agents Modernize Legacy COBOL Systems](https://github.blog/ai-and-ml/github-copilot/how-github-copilot-and-ai-agents-are-saving-legacy-systems/)
## Copilot Customization and Advanced Agent Workflows
Developers can now use Agent Package Manager in conjunction with GitHub Actions to orchestrate and maintain AI agents, supporting version control and auditing.
A technical podcast with Harald Kirschner offers insight into customizing chat agents within VS Code, covering the new Agent Memory extension for context management. These updates add new customization options to Copilot’s agent-based features.
- [How to Build Reliable AI Workflows with Agentic Primitives and Context Engineering](https://github.blog/ai-and-ml/github-copilot/how-to-build-reliable-ai-workflows-with-agentic-primitives-and-context-engineering/)
- [Building Agent Memory for VS Code with Harald Kirschner](/ai/videos/building-agent-memory-for-vs-code-with-harald-kirschner)
## Copilot in Real-World and Open Source Projects
Case studies such as the ‘No Bark’ open-source project illustrate how Copilot supports accessibility and deployment for those without a coding background. Developers are also encouraged to join the open source MCP community and contribute to agentic workflow innovation.
- [How GitHub Copilot and Azure AI Apps Fueled a Real-World Project: The ''No Bark'' Solution](/ai/videos/how-github-copilot-and-azure-ai-apps-fueled-a-real-world-project-the-no-bark-solution)',
    'GitHub Copilot rolled out additional integrations and improvements this week, furthering its shift from a code completion tool to an AI platform designed for developers. Enhancements reinforce recent releases (such as Grok Code Fast 1 and Claude Sonnet 4.5), strengthening Copilot’s use of multiple models, support for more IDEs, and automation features. Copilot now contributes to a broader range of developer workflows, including Pull Request creation, PowerShell scripting, legacy system updates, and Model Context Protocol (MCP) agent support.',
    1760943600, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-10-20', 'TechHub',
    'TechHub', 'B2F67C72608D4ADD1B1D3AA8D6B7EC19C82AEA49B88EAE4D4FD9291915CD1551', ',GitHub Copilot,AI Coding Assistant,Multi Model AI,GPT 4.1,Claude Sonnet 4.5,Grok Code Fast 1,MCP,Copilot CLI,VS Code,JetBrains,SQL Server Management Studio,Pull Requests,Commit Messages,Code Review,Autofix,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-10-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-10-13', 'roundups', 'Weekly GitHub Copilot Roundup: New Models, CLI Gains, and Agents',
    'GitHub Copilot delivered a variety of updates this week, increasing AI-assisted coding support in developer workflows—including IDEs, command-line tools, and mobile apps. Improvements include new model selection features, updated productivity tools, expanded automation options, and guidance for reviewing and customizing AI-generated code. Tutorials and case studies show Copilot’s practical uses, while CLI enhancements, educational features, and bug-fix resources help developers wherever they choose to work. New agent models and command-line/chat capabilities make interaction with Copilot more flexible, and community analysis highlights how these changes impact developer experience. Copilot is steadily growing into a toolset for supporting modern development flows.
<!--excerpt_end-->
## GitHub Copilot: New Models, Agent Integrations, and IDE Features
In continuation of the previous week’s model management and context tools (such as Sonnet 4.5’s rollout), GitHub released the Grok Code Fast 1 public preview for Copilot Pro, Pro+, Business, and Enterprise plans. This expands available models and further supports agent workflows. Grok Code Fast 1 is accessible via model selectors on GitHub.com, GitHub Mobile, VS Code, Visual Studio, JetBrains IDEs, Xcode, and Eclipse, strengthening integration across development environments.
Features for model selection, enterprise policy controls, and extended feedback build on last week’s analytics and scaling support, helping organizations shape Copilot’s AI adoption to their needs. Documentation updates and request feedback maintain a focus on user input, similar to earlier community releases.
Visual Studio Code continues with persistent chat and agentic automation. New capabilities—support for Claude Sonnet 4.5, GPT-5, Grok Code Fast 1—add persistent chat, Plan Mode, and the Copilot Coding Agent. Tutorials now cover MCP server integration and remote context management, showing the expanding registry/protocol ecosystem in ongoing developer resources.
- [Grok Code Fast 1 Public Preview Launched for GitHub Copilot Plans](https://github.blog/changelog/2025-10-06-grok-code-fast-1-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [The Latest AI Features in Visual Studio Code](/ai/videos/the-latest-ai-features-in-visual-studio-code)
- [Enhancing Developer Workflows with MCP Servers and GitHub Copilot in VS Code](/ai/videos/enhancing-developer-workflows-with-mcp-servers-and-github-copilot-in-vs-code)
## GitHub Copilot CLI: Terminal Improvements and Workflow Expansion
Expanding on last week’s improvements in model switching, image support, and permissions, Copilot CLI now delivers additional speed and efficiency. Response times are 45% faster, completion steps are reduced, token usage is optimized, and the terminal experience is improved. This continues the shift from legacy workflow tools to a streamlined command-line approach.
Additional interface features—edit diffs, colored markdown, compact rendering—support new workflows (tab cycling, multiline input, argument hints) built on previous additions like command forwarding, context alerts, and accessibility options. PowerShell parity supports reliability across platforms. Daily npm updates allow ongoing feedback, aligning with continued community-driven feature releases.
- [An Introduction to the New GitHub Copilot CLI](/ai/videos/an-introduction-to-the-new-github-copilot-cli)
- [GitHub Copilot CLI: Faster, More Concise, Streamlined Developer Experience](https://github.blog/changelog/2025-10-10-github-copilot-cli-faster-more-concise-and-prettier)
## Copilot-Aided Case Studies and Tutorials: Apps, Automations, and Real-World Workflows
Recent case studies continue previous themes of fast onboarding, agent automation, and project kickstart. The Buzzword Bingo app, created with GitHub Spark and Copilot Coding Agent, uses prompt-driven development for project scaffolding—providing a direct comparison to manual and chat-based coding approaches.
Tutorials on Playwright test generation, Jupyter Notebook improvements, and automated accessibility expand last week’s coverage on documentation and developer workflows. CI/CD session examples demonstrate mobile bug fixes and build on prior onboarding, session management, and code review processes.
An article on upgrading Blazor apps in .NET 10 highlights ongoing focus on modernization, workflow analytics, and enterprise migration, positioning Copilot as a tool for compliance and iterative code improvements.
- [Building Buzzword Bingo with GitHub Spark, Copilot, and Modern Dev Tools](https://harrybin.de/posts/github-spark-buzzword-bingo/)
- [Vibe Coding a Podcast Analytics Dashboard with GitHub Copilot and AI](https://devblogs.microsoft.com/blog/complete-beginners-guide-to-vibe-coding-an-app-in-5-minutes)
- [Generating Reliable Tests with AI and Copilot in Playwright](/ai/videos/generating-reliable-tests-with-ai-and-copilot-in-playwright)
- [VS Code Live: Leveraging GitHub Copilot and Agents in Jupyter Notebooks](/ai/videos/vs-code-live-leveraging-github-copilot-and-agents-in-jupyter-notebooks)
- [Upgrading eShop with Blazor in .NET 10 Using GitHub Copilot](/ai/videos/upgrading-eshop-with-blazor-in-net-10-using-github-copilot)
- [How GitHub Copilot Automated Accessibility Governance at GitHub](https://github.blog/ai-and-ml/github-copilot/how-we-automated-accessibility-compliance-in-five-hours-with-github-copilot/)
- [Completing Urgent Fixes Remotely Using GitHub Copilot Coding Agent and Mobile](https://github.blog/developer-skills/github/completing-urgent-fixes-anywhere-with-github-copilot-coding-agent-and-mobile/)
## Copilot in Review and Ongoing Role Evolution
Reflections on code review and developer roles continue last week’s focus on best practices, documentation automation, and increased agent-driven workflows. A guide for reviewing AI-generated .NET code and an analysis of developer roles emphasize effective review cycles, team collaboration, and portfolio management, shaped by Copilot’s AI support for code orchestration. Copilot Spaces and Code Review features illustrate the ongoing move to flexible, iterative developer workflows.
- [Reviewing AI-Generated Code in .NET: Best Practices for Developers](https://devblogs.microsoft.com/dotnet/developer-and-ai-code-reviewer-reviewing-ai-generated-code-in-dotnet/)
- [The Developer Role Is Evolving: How to Stay Ahead with AI and GitHub Copilot](https://github.blog/ai-and-ml/the-developer-role-is-evolving-heres-how-to-stay-ahead/)
## Supporting Education, Neurodiversity, and Sustainability with Copilot
Education with Copilot expands through curriculum integration and feedback tools, building on last week’s Study Buddy Agent and sustainability topics. New material on neurodiversity extends previous personal stories and workflow evaluations, offering advice for developers with diverse learning requirements.
Examples from City Energy Analyst and MapYourGrid show Copilot supporting social impact and open source collaboration, continuing last week’s climate and community features. Podcast discussions highlight how AI is changing experimental workflows and automation skills, following ongoing learning and adaptation strategies.
- [Study Buddy: Learning Data Science and Machine Learning with an AI Sidekick](https://techcommunity.microsoft.com/t5/microsoft-developer-community/study-buddy-learning-data-science-and-machine-learning-with-an/ba/p/4460144)
- [ADHD, GitHub Copilot, and Neurodiversity: Real Talk with Mads Torgersen and Klaus Loffelmann](/ai/videos/adhd-github-copilot-and-neurodiversity-real-talk-with-mads-torgersen-and-klaus-loffelmann)
- [Using GitHub Copilot to Advance Sustainable City Design with Open Source](/ai/videos/using-github-copilot-to-advance-sustainable-city-design-with-open-source)
- [Using GitHub Copilot to Map Electricity Gaps in Zambia with MapYourGrid](/ai/videos/using-github-copilot-to-map-electricity-gaps-in-zambia-with-mapyourgrid)
- [How AI Is Changing the Way Developers Learn to Code: A Conversation with Scott Tolinski](/ai/videos/how-ai-is-changing-the-way-developers-learn-to-code-a-conversation-with-scott-tolinski)
## Other GitHub Copilot News
New tutorials on prompt-driven code generation in VS Code expand last week’s work with documentation and chat workflows, improving Copilot’s conversational automation approach through constant feedback.
A key update announces that Claude Sonnet 3.5 will be deprecated from Copilot, starting a new round of model standardization and upgrades—continuing the process established by previous migration and selector guides.
- [Build a Responsive UI through Prompt Driven Development with GitHub Copilot](/ai/videos/build-a-responsive-ui-through-prompt-driven-development-with-github-copilot)
- [Deprecation of Claude Sonnet 3.5 in GitHub Copilot: What You Need to Know](https://github.blog/changelog/2025-10-07-upcoming-deprecation-of-claude-sonnet-3-5)',
    'GitHub Copilot delivered a variety of updates this week, increasing AI-assisted coding support in developer workflows—including IDEs, command-line tools, and mobile apps. Improvements include new model selection features, updated productivity tools, expanded automation options, and guidance for reviewing and customizing AI-generated code. Tutorials and case studies show Copilot’s practical uses, while CLI enhancements, educational features, and bug-fix resources help developers wherever they choose to work. New agent models and command-line/chat capabilities make interaction with Copilot more flexible, and community analysis highlights how these changes impact developer experience. Copilot is steadily growing into a toolset for supporting modern development flows.',
    1760338800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-10-13', 'TechHub',
    'TechHub', 'AA864E1CAAEF9654BE9C91D9FB6D284A5F0ED1C99EE6C9CB8FE86027C19A7D36', ',GitHub Copilot,Copilot Coding Agent,Agentic Workflows,Model Selection,Grok Code Fast 1,GPT 5,Claude Sonnet 4.5,VS Code,VS,JetBrains,GitHub Copilot CLI,MCP Servers,Code Review,Enterprise Policy Controls,GitHub Mobile,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-10-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-10-06', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, Models, and Analytics',
    'This week, GitHub Copilot introduced new and updated features: deeper integration with Visual Studio and VS Code, expanded access to AI models such as Anthropic Claude Sonnet 4.5 for chat and CLI use, and improvements to agent workflows. .NET and Azure users benefit from better performance analysis and modernization tools. Copilot’s improvements include enhanced analytics, prompt engineering, and rapid prototyping for enterprise teams, supporting the move to AI-assisted coding across desktop, web, terminal, and cloud for increased developer flexibility and efficiency.
<!--excerpt_end-->
## GitHub Copilot Integration in Visual Studio
Visual Studio 2022’s September 2025 release (v17.14) adds new capabilities for Copilot, pairing tools like the Profiler Agent and .NET Modernization Agent as part of ongoing agentic workflow automation. The Profiler Agent, accessible via Copilot Chat or the `@Profiler` command, delivers diagnostics and benchmarking, adding context-driven automation and performance review. Integrating BenchmarkDotNet simplifies modernization and Azure migration for .NET workloads.
Agent Mode produces faster responses and stronger context management, with better Model Context Protocol (MCP) support for extensibility and structured outputs. Features such as Mermaid diagram generation and code review promote collaborative development. The October roadmap highlights progress toward remote agents, MCP governance, group policy, and wider model support (Claude Sonnet 4.5, future GPT-5 Codex) as Visual Studio continues evolving as an AI-ready developer platform.
- [GitHub Copilot Updates in Visual Studio September 2025 Release](https://github.blog/changelog/2025-09-30-github-copilot-in-visual-studio-september-update)
- [Visual Studio September 2025 Update: Profiler Agent, App Modernization, GitHub Copilot Enhancements](https://devblogs.microsoft.com/visualstudio/visual-studio-september-update/)
- [Visual Studio AI-Powered Roadmap: October Updates with GitHub Copilot](https://devblogs.microsoft.com/visualstudio/roadmap-for-ai-in-visual-studio-october/)
## GitHub Copilot CLI and Terminal Workflows
CLI releases reinforce the transition from preview to general availability, replacing older extensions with a unified npm CLI. Anthropic Claude Sonnet 4.5 is now active in the terminal using the `/model` command for flexible model switching, continuing the shift toward cross-model agentic experiences. New CLI features support image handling, improved input processing, and refined context management for multimodal interaction.
Security and analytics improvements add detailed permission controls and dashboards for usage tracking, helping teams manage resources and promote transparency. Context truncation alerts and improved command forwarding strengthen developer tool reliability. Tutorials and practical guides support smooth onboarding for agentic workflows.
- [GitHub Copilot CLI: Enhanced Model Selection, Image Recognition, and Streamlined UI Improvements](https://github.blog/changelog/2025-10-03-github-copilot-cli-enhanced-model-selection-image-support-and-streamlined-ui)
- [Full Demo: Mastering GitHub Copilot CLI for Terminal-Based Development](/ai/videos/full-demo-mastering-github-copilot-cli-for-terminal-based-development)
- [The Coding Buddy That Lives in Your Command Line: GitHub Copilot CLI](/ai/videos/the-coding-buddy-that-lives-in-your-command-line-github-copilot-cli)
## GitHub Copilot Coding Agent
With last week’s Coding Agent general availability and updated session controls, Copilot now includes a repository kickstart option to automate project scaffolding, making agent workflows more approachable for teams. The agent maintains pull request history to streamline code review rounds and onboarding. Claude Sonnet 4.5 integration builds on past model updates like GPT-5-Codex and Copilot-SWE, supporting better code generation and handling with practical SWE-bench feedback.
Recent admin guides help teams configure policies and manage models, providing governance for expanding AI-powered automation.
- [Kickstart Repositories Using Copilot Coding Agent](https://github.blog/changelog/2025-09-30-start-your-new-repository-with-copilot-coding-agent)
- [Copilot Coding Agent Now Remembers Context Within Pull Requests](https://github.blog/changelog/2025-09-30-copilot-coding-agent-remembers-context-within-the-same-pull-request)
- [Anthropic Claude Sonnet 4.5 Now Available for GitHub Copilot Coding Agent](https://github.blog/changelog/2025-09-30-anthropic-claude-sonnet-4-5-is-in-public-preview-for-copilot-coding-agent)
## AI Model Selection and Integration in Copilot
Claude Sonnet 4.5 is now generally available in Copilot Chat and CLI, with unified policy controls and a public preview for automatic model selection. This transition marks full deployment from initial previews to availability across major IDEs (Visual Studio, VS Code, JetBrains, Xcode, Eclipse, GitHub.com, Mobile).
Automatic model selection in VS Code for Copilot Business/Enterprise automatically chooses between GPT-5, GPT-4.1, Sonnet 4, and Sonnet 3.5 to balance user experience and operational costs. Analytic transparency and billing features allow teams to monitor resource use. Bring Your Own Key (BYOK) lets organizations manage custom API credentials, and staged rollout plus policy controls help teams use AI responsibly.
- [Claude Sonnet 4.5 Now Available to GitHub Copilot Users in Visual Studio and Other IDEs](https://github.blog/changelog/2025-10-02-claude-sonnet-4-5-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Anthropic Claude Sonnet 4.5 Launches for GitHub Copilot Chat and CLI](https://github.blog/changelog/2025-09-29-anthropic-claude-sonnet-4-5-is-in-public-preview-for-github-copilot)
- [Auto Model Selection Now Available in VS Code for GitHub Copilot Business and Enterprise](https://github.blog/changelog/2025-09-30-auto-model-selection-is-now-in-vs-code-for-copilot-business-and-enterprise)
## GitHub Spark and Rapid Application Prototyping
GitHub Spark’s public preview for prototyping in Codespaces now enrolls Copilot Enterprise users, featuring better reliability and reduced setup effort. The update automates initial configuration and simplifies foundational tasks—a continuation of previous workflow orchestration changes.
Recent bug fixes, improved iteration history, and smoother workbench interfaces reflect Copilot’s ongoing push for productivity and onboarding improvements.
- [GitHub Spark Public Preview Now Available for Copilot Enterprise Subscribers](https://github.blog/changelog/2025-09-30-github-spark-in-public-preview-for-copilot-enterprise-subscribers)
- [Spark Public Preview Released for Copilot Enterprise: Expanded Access and Enhanced Reliability](https://github.blog/changelog/2025-10-01-spark-%f0%9f%9a%80-expanded-access-enhanced-reliability-and-faster-iteration-history)
## GitHub Copilot Advanced Documentation, Prompt Engineering, and Parallel Workflows
Documentation and prompt engineering updates build on last week’s adoption of standardized `.prompt.md` files and agentic parallel workflows. Tutorials outline methods for auto-generating README files, API documentation, and inline comments, supporting reusable prompts and modular documents that scale with team needs. Expanded parallel workflow techniques now let developers orchestrate tasks across Copilot Chat, CLI, and Coding Agent for faster delivery.
Spec-driven development merges Markdown for code and documentation, continuing from previous experimental workflow showcases. Tutorials and videos highlight Copilot’s broader utility for planning, brainstorming, and creative work.
- [Advanced Techniques for Documenting Code with GitHub Copilot](/ai/videos/advanced-techniques-for-documenting-code-with-github-copilot)
- [Supercharge Your Prompts with .prompt.md](https://www.cooknwithcopilot.com/blog/supercharge-your-prompts-with-prompt-md.html)
- [Using GitHub Copilot for Multiple Tasks in Parallel](https://harrybin.de/posts/parallel-github-copilot-workflow/)
- [Spec-driven Development with Markdown and GitHub Copilot: An Experimental AI Coding Workflow](https://github.blog/ai-and-ml/generative-ai/spec-driven-development-using-markdown-as-a-programming-language-when-building-with-ai/)
- [Prompting for More Than Code with GitHub Copilot](/ai/videos/prompting-for-more-than-code-with-github-copilot)
## GitHub Copilot Workflow Analytics and Registry Support
Copilot’s Premium Requests Analytics Dashboard, now generally available, provides granular usage tracking by user, model, and cost center, increasing admin oversight and supporting enterprise-level automation and transparency.
Model Context Protocol (MCP) Registry use continues to expand, building on last week’s guides concerning protocol development and integration. Teams increasingly experiment with open protocol customization for reusable agentic workflows.
- [Premium Requests Analytics Dashboard for GitHub Copilot: General Availability](https://github.blog/changelog/2025-09-30-premium-requests-analytics-page-is-now-generally-available)
- [The Origins & Evolution of the GitHub MCP Registry with Toby Padilla](/ai/videos/the-origins-and-evolution-of-the-github-mcp-registry-with-toby-padilla)
## Other GitHub Copilot News
Visual Studio Code introduces updates for background coding agents, building on previously reported advances in agentic automation. Feedback features and more customizable agent behaviors support context-aware development.
Workshops like "How to Master GitHub Copilot" offer hands-on training for MCP integration, modernization, and cloud deployment, supporting community learning as Copilot’s feature set expands.
- [VS Code: Background Coding Agents and GitHub Copilot Enhancements](/ai/videos/vs-code-background-coding-agents-and-github-copilot-enhancements)
- [How to Master GitHub Copilot: Build, Prompt, Deploy Smarter](https://techcommunity.microsoft.com/t5/microsoft-developer-community/how-to-master-github-copilot-build-prompt-deploy-smarter/ba/p/4456660)',
    'This week, GitHub Copilot introduced new and updated features: deeper integration with Visual Studio and VS Code, expanded access to AI models such as Anthropic Claude Sonnet 4.5 for chat and CLI use, and improvements to agent workflows. .NET and Azure users benefit from better performance analysis and modernization tools. Copilot’s improvements include enhanced analytics, prompt engineering, and rapid prototyping for enterprise teams, supporting the move to AI-assisted coding across desktop, web, terminal, and cloud for increased developer flexibility and efficiency.',
    1759734000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-10-06', 'TechHub',
    'TechHub', '1475416D6FD0220716F318275532DBB9AA8F0878A198B58CA62DBEBD383171B1', ',GitHub Copilot,Visual Studio 2022,VS Code,Copilot Chat,Copilot CLI,Copilot Coding Agent,Agent Mode,Anthropic Claude Sonnet 4.5,Auto Model Selection,MCP,MCP Registry,.NET Modernization,BenchmarkDotNet,Premium Requests Analytics,GitHub Spark,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-09-29
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-09-29', 'roundups', 'Weekly GitHub Copilot Roundup: MCP, Agents, and Modernization',
    'GitHub Copilot continues its transition from basic code completion toward agent-powered automation, cloud modernization, and context-driven software development. Developers now have more choice with new models, such as GPT-5-Codex and Claude Opus 4.1, alongside further Copilot improvements and extended IDE support. The platform is aligning with open standards focused on the Model Context Protocol (MCP) to improve automation, compatibility, and extensibility. These upgrades include updates for command-line tools, VS Code, enterprise tracking, SQL development, and cloud migration workflows for Java and .NET. As legacy models and extensions get phased out, developers are encouraged to move to the latest approaches and integrate Copilot more deeply into team-oriented, automated workflows.
<!--excerpt_end-->
## Model Context Protocol (MCP): Standardization and Ecosystem Transition
GitHub will remove support for Copilot Extensions as GitHub Apps by November 2025 and recommends full migration to MCP servers. This transition takes the ecosystem from initial registry features to a protocol-focused structure, allowing developers to reuse MCP integrations more easily across agents for increased interoperability. Recent registry changes reinforce this approach. The latest MCP IDE guides encourage developers to review migration documentation and shift to MCP standards—for scalable, maintainable workflows and future agentic development. The MCP registry is now positioned as a foundation for Copilot''s ongoing development.
- [Deprecation of GitHub Copilot Extensions in Favor of Model Context Protocol (MCP) Servers](https://github.blog/changelog/2025-09-24-deprecate-github-copilot-extensions-github-apps)
- [What is Model Context Protocol (MCP)?](/ai/videos/what-is-model-context-protocol-mcp)
- [Understanding Model Context Protocol (MCP) for Developers](/ai/videos/understanding-model-context-protocol-mcp-for-developers)
- [How to Use GitHub Copilot Agent Mode and MCP to Query Microsoft Learn Docs in VS Code](https://techcommunity.microsoft.com/t5/microsoft-developer-community/use-copilot-and-mcp-to-query-microsoft-learn-docs/ba/p/4455835)
- [Use Copilot and MCP to Query Microsoft Learn Docs](/ai/videos/use-copilot-and-mcp-to-query-microsoft-learn-docs)
- [GitHub MCP Registry Integration with Playwright in VS Code Insiders](/ai/videos/github-mcp-registry-integration-with-playwright-in-vs-code-insiders)
## Copilot Coding Agent: From Workflow Automation to IDE and CLI Integration
Copilot Coding Agent is now available to all paid users, progressing from workflow previews to broad automation across GitHub, IDEs, and direct CLI/Mobile app usage. GitHub Actions continue to orchestrate agent tasks, with new CLI and mobile features providing more flexibility for developers. Recent added controls for issue assignment and repository selection give teams enhanced management over agent-driven tasks—making cross-platform delegation simpler for both individuals and groups.
- [GitHub Copilot Coding Agent Now Generally Available](https://github.blog/changelog/2025-09-25-copilot-coding-agent-is-now-generally-available)
- [Kick off and Track Copilot Coding Agent Sessions from the GitHub CLI](https://github.blog/changelog/2025-09-25-kick-off-and-track-copilot-coding-agent-sessions-from-the-github-cli)
- [Start and Track GitHub Copilot Coding Agent Tasks in GitHub Mobile](https://github.blog/changelog/2025-09-24-start-and-track-copilot-coding-agent-tasks-in-github-mobile)
- [Enhanced Copilot Issue Assignment: Pick Repository and Base Branch](https://github.blog/changelog/2025-09-23-pick-the-repository-and-base-branch-when-assigning-issues-to-copilot)
- [Copilot Can Create GitHub Issues with Code Snippets: Public Preview](https://github.blog/changelog/2025-09-25-copilot-can-create-issues-with-code-snippets-in-public-preview)
## AI Model Options: Next-Generation Model Rollouts, Integrations, and Deprecation
OpenAI''s GPT-5-Codex and Claude Opus 4.1 are now available to additional Copilot subscribers and IDE users, broadening model selection following last week''s previews. The Copilot-SWE model launches for focused software engineering in VS Code Insiders, supporting ongoing context-driven workflows. Admin controls and Pro+ plan updates deliver more robust model management. GitHub continues to phase out older models, with organizations guided through updates using actionable adoption tutorials.
- [OpenAI GPT-5-Codex Now Available in GitHub Copilot Public Preview](https://github.blog/changelog/2025-09-23-openai-gpt-5-codex-is-rolling-out-in-public-preview-for-github-copilot)
- [Claude Opus 4.1 Now Generally Available in GitHub Copilot](https://github.blog/changelog/2025-09-23-claude-opus-4-1-is-now-generally-available-in-github-copilot)
- [GitHub Copilot-SWE Model Launches in Visual Studio Code Insiders](https://github.blog/changelog/2025-09-22-copilot-swe-model-rolling-out-to-visual-studio-code-insiders)
- [Upcoming Deprecation of Select Copilot Models from Claude, OpenAI, and Gemini](https://github.blog/changelog/2025-09-23-upcoming-deprecation-of-select-copilot-models-from-claude-openai-and-gemini)
- [VS Code and GitHub Copilot: Exploring GPT-5-Codex and Copilot-SWE](/ai/videos/vs-code-and-github-copilot-exploring-gpt-5-codex-and-copilot-swe)
- [What''s New: GitHub MCP Registry, Copilot CLI Public Preview, and Beast Mode for Copilot](/ai/videos/whats-new-github-mcp-registry-copilot-cli-public-preview-and-beast-mode-for-copilot)
## Copilot CLI and Extension Deprecations: Streamlining for the Future
GitHub is discontinuing the `gh-copilot` CLI extension, moving to the npm-distributed Copilot CLI (now in public preview). This is part of the ongoing shift away from historic Marketplace extensions and supports recent MCP registry changes. The improved CLI simplifies agentic code generation, code review, and MCP-based extensibility, reinforcing the platform''s commitment to standardization and modern developer tooling. Admins should review timelines and update workflows before October 2025 as MCP adoption grows.
- [GitHub Copilot CLI Extension Deprecation Announcement](https://github.blog/changelog/2025-09-25-upcoming-deprecation-of-gh-copilot-cli-extension)
- [GitHub Copilot CLI Now Available in Public Preview](https://github.blog/changelog/2025-09-25-github-copilot-cli-is-now-in-public-preview)
## Copilot Spaces, Embedding Model Updates, and Open Ecosystem Enhancements
Copilot Spaces is now available to all users, providing a platform for managing files, documentation, and project context—building on recent embedding and workflow advancements. A new embedding model for code search in VS Code enhances daily code retrieval and multi-language support. The Hugging Face VS Code extension now allows Copilot Chat to interact with open-source models, enabling experimental and domain-specific development and broadening pay-as-you-go model choices for teams seeking customized workflows and improved context support.
- [Copilot Spaces: General Availability Announcement](https://github.blog/changelog/2025-09-24-copilot-spaces-is-now-generally-available)
- [GitHub Copilot’s New Embedding Model Improves Code Search in VS Code](https://github.blog/news-insights/product-news/copilot-new-embedding-model-vs-code/)
- [Hugging Face Opens GitHub Copilot Chat to Open-Source Models](https://devops.com/hugging-face-opens-github-copilot-chat-to-open-source-models/)
## Copilot-Powered Modernization: Java, .NET, and SQL Workflows
The Copilot App Modernization toolkit is now generally available for Java and .NET projects, expanding on last week''s agent-guided refactoring tutorials. The toolkit automates dependency analysis, code transformation, containerization, and incorporates security scanning—supporting recent compliance initiatives. Updated SQL workflows leverage AI for smarter query generation, analytics, and automation for both MSSQL and PostgreSQL, continuing the trend from last week''s context-driven database improvements. Tutorials help teams upgrade legacy systems to modern infrastructure while prioritizing productivity and maintainability.
- [GitHub Copilot App Modernization Now Available for Java and .NET Projects](https://github.blog/changelog/2025-09-22-github-copilot-app-modernization-is-now-generally-available-for-java-and-net)
- [Modernize .NET Apps in Days with GitHub Copilot](/ai/videos/modernize-net-apps-in-days-with-github-copilot)
- [Modernizing Java Applications with GitHub Copilot and Azure Deployment](/ai/videos/modernizing-java-applications-with-github-copilot-and-azure-deployment)
- [Modernizing Java Projects with GitHub Copilot Agent Mode: Step-by-Step Guide](https://github.blog/ai-and-ml/github-copilot/a-step-by-step-guide-to-modernizing-java-projects-with-github-copilot-agent-mode/)
- [Quickly Modernize and Deploy Java Apps with AI and GitHub Copilot in VS Code](/ai/videos/quickly-modernize-and-deploy-java-apps-with-ai-and-github-copilot-in-vs-code)
- [Modernize Java Apps in Days with GitHub Copilot](/ai/videos/modernize-java-apps-in-days-with-github-copilot)
- [Microsoft’s AI Agents Target Technical Debt Crisis](https://devops.com/microsofts-ai-agents-target-technical-debt-crisis/)
- [Enhancing SQL Development in VS Code with GitHub Copilot and Microsoft Fabric](/ai/videos/enhancing-sql-development-in-vs-code-with-github-copilot-and-microsoft-fabric)
- [Boost Productivity with the PostgreSQL Extension and GitHub Copilot in VS Code](https://techcommunity.microsoft.com/t5/microsoft-developer-community/talk-to-your-data-postgresql-gets-a-voice-in-vs-code/ba/p/4453695)
## Agentic Workflows, Prompt-Driven Development, and IDE Innovations
New guidance covers converting web apps to mobile apps using Copilot prompts, leveraging plan mode, voice input, and improved model management—continuing previous advances in MCP-powered IDE workflows and Spec Kit sessions. The VS Code Insiders podcast features the latest on IDE improvements, focusing on AI’s shift from pure code assistance to orchestrated workflow support. Ongoing updates in live preview, documentation access, and activity tracking build on former releases in XAML, collaborative coding, and agentic automation—marking positive change for developer workflows.
- [Converting a Web App to Mobile Using GitHub Copilot Prompts](/ai/videos/converting-a-web-app-to-mobile-using-github-copilot-prompts)
- [The Future of Coding Agents in VS Code: Insights from VS Code Insiders Podcast](/ai/videos/the-future-of-coding-agents-in-vs-code-insights-from-vs-code-insiders-podcast)
- [Enhancements to XAML Live Preview in Visual Studio for .NET MAUI](https://devblogs.microsoft.com/visualstudio/enhancements-to-xaml-live-preview-in-visual-studio-for-net-maui/)
- [GitHub Copilot: The Influence of Generative AI Assistants and Agents on Software Development - Netherlands](/github-copilot-the-influence-of-generative-ai-assistants-and-agents-on-software-development-netherlands)
- [GitHub Copilot: The Influence of Generative AI Assistants and Agents on Software Development - Belgium](/github-copilot-the-influence-of-generative-ai-assistants-and-agents-on-software-development-belgium)',
    'GitHub Copilot continues its transition from basic code completion toward agent-powered automation, cloud modernization, and context-driven software development. Developers now have more choice with new models, such as GPT-5-Codex and Claude Opus 4.1, alongside further Copilot improvements and extended IDE support. The platform is aligning with open standards focused on the Model Context Protocol (MCP) to improve automation, compatibility, and extensibility. These upgrades include updates for command-line tools, VS Code, enterprise tracking, SQL development, and cloud migration workflows for Java and .NET. As legacy models and extensions get phased out, developers are encouraged to move to the latest approaches and integrate Copilot more deeply into team-oriented, automated workflows.',
    1759129200, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-09-29', 'TechHub',
    'TechHub', 'A18FAF4BB0976C12EE70BF6F199938F34CCB98184239D39BE7420C7982ACA1F1', ',GitHub Copilot,Copilot Coding Agent,MCP,MCP Servers,VS Code,VS Code Insiders,GitHub Actions,GitHub CLI,GitHub Mobile,GPT 5 Codex,Claude Opus 4.1,Copilot SWE,Copilot Spaces,Copilot CLI,App Modernization,Java,.NET,MAUI,SQL,PostgreSQL,Microsoft Fabric,Code Search,Embedding Models,Hugging Face,Open Source Models,Enterprise Administration,Model Deprecations,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-09-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-09-22', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, MCP, Models, Governance',
    'Recent GitHub Copilot updates reinforce the ecosystem’s growth in enterprise integrations, broader MCP standard usage, increased model options, and better context and privacy features. Integrations with Teams, Azure DevOps, Visual Studio Code, and third-party services highlight practical automation and a more effective developer experience.
<!--excerpt_end-->
## GitHub Copilot Coding Agent and Team Integration
The new public preview for Copilot allows Azure Boards work items to be assigned to Copilot’s coding agent, linking Azure DevOps and GitHub for automated handling of issues, bug fixes, and feature delivery. This builds on earlier agentic workflows, focusing on safe code changes with continuous integration.
The GitHub app for Microsoft Teams enables conversational pull request automation directly within chat, expanding Copilot’s reach across platforms. These features allow organizations to use Copilot’s agent capabilities throughout their toolchains, with administration supported by refined workflow controls—a natural progression from previous enterprise management features.
Guides illustrate Copilot agent flexibility with Playwright MCP, Notion, Hugging Face, and IDE features like the Agents panel. The focus is now on workflow optimization and practical automation rather than just feature announcements.
- [Assign Azure Boards Work Items to GitHub Copilot Coding Agent in Public Preview](https://github.blog/changelog/2025-09-18-assign-azure-boards-work-items-to-copilot-coding-agent-in-public-preview)
- [Azure Boards Integration with GitHub Copilot: Private Preview Announced](https://devblogs.microsoft.com/devops/azure-boards-integration-with-github-copilot-private-preview/)
- [Using GitHub Copilot Coding Agent with Microsoft Teams for Automated PRs](https://github.blog/changelog/2025-09-19-work-with-copilot-coding-agent-in-microsoft-teams)
- [AI-Powered GitHub App for Teams Now in Public Preview](https://devblogs.microsoft.com/microsoft365dev/copilot-powered-github-app-for-teams-preview/)
- [5 Powerful Ways to Integrate GitHub Copilot Coding Agent into Your Workflow](https://github.blog/ai-and-ml/github-copilot/5-ways-to-integrate-github-copilot-coding-agent-into-your-workflow/)
## GitHub MCP Registry and Agentic Ecosystem
The GitHub MCP Registry further extends last week’s MCP server and registry controls, providing a unified hub for discovering agents, servers, and partner extensions. Features such as allowlists and registry setup now combine within a secure, centralized registry.
This registry supports a variety of IDEs and adopts an open-source model, continuing recent community engagement. Tutorials highlight MCP prompt extensions, code search for Azure DevOps, and streamlined MCP-powered IDE sessions, all aimed at simplifying deployment and management.
- [GitHub MCP Registry Launches as Central Hub for AI Development Tools](https://devops.com/github-mcp-registry-launches-as-central-hub-for-ai-development-tools/)
- [Meet the GitHub MCP Registry: The Fastest Way to Discover MCP Servers](https://github.blog/ai-and-ml/github-copilot/meet-the-github-mcp-registry-the-fastest-way-to-discover-mcp-servers/)
- [GitHub MCP Registry: Centralizing AI Agent Tool Discovery](https://github.blog/changelog/2025-09-16-github-mcp-registry-the-fastest-way-to-discover-ai-tools)
- [Search Less, Build More: Inner Sourcing with GitHub Copilot and Azure DevOps MCP Server](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/search-less-build-more-inner-sourcing-with-github-copilot-and/ba-p/4454560)
- [Enhance Your Copilot Experience in Visual Studio with MCP Prompts, Resources, and Sampling](https://devblogs.microsoft.com/visualstudio/mcp-prompts-resources-sampling/)
## Expanding Model Choice, Context Management, and Privacy
Copilot’s model system now features automatic selection for Copilot Chat, improving on previous manual options and BYOK functionality. It supports automated, context-sensitive switching for both paid and free users, resulting in improved responsiveness and quota use.
Claude Opus 4.1 is now available for enterprise and Pro+ users in leading IDEs, and Foundry Local integration highlights privacy features, following the move to more context-driven deployments. Guides focus on practical benefits like choosing suitable models and running private, on-device inference for sensitive projects.
- [Auto Model Selection Preview for GitHub Copilot Chat in VS Code](https://code.visualstudio.com/blogs/2025/09/15/autoModelSelection)
- [Auto Model Selection for GitHub Copilot in VS Code Public Preview](https://github.blog/changelog/2025-09-14-auto-model-selection-for-copilot-in-vs-code-in-public-preview)
- [Claude Opus 4.1 Rolls Out for GitHub Copilot Users in Popular IDEs](https://github.blog/changelog/2025-09-16-anthropic-claude-opus-4-1-is-now-available-in-public-preview-in-visual-studio-jetbrains-xcode-and-eclipse)
- [Picking the Right AI Model for Your Task in GitHub Copilot](https://cooknwithcopilot.com/blog/picking-the-right-ai-model-for-your-task.html)
- [Integrating Foundry Local with GitHub Copilot in Visual Studio Code](https://devblogs.microsoft.com/foundry/ai-assisted-development-powered-by-local-models/)
## GitHub Copilot with Azure AI Foundry and Enterprise Model Integration
This week’s tutorials show Copilot connecting to Azure AI Foundry models with the AI Toolkit in Visual Studio Code, continuing last week’s MCP-powered workflow coverage. Guides cover steps from set-up to using custom, compliance-oriented large language models in local and cloud environments.
Instructions include authentication, extension configuration, and enterprise deployment, illustrating a shift from general news toward step-by-step deployments.
- [Integrating Azure AI Foundry Models with GitHub Copilot via AI Toolkit](/ai/videos/integrating-azure-ai-foundry-models-with-github-copilot-via-ai-toolkit)
- [Integrating Azure AI Foundry Models with GitHub Copilot Using the AI Toolkit](/ai/videos/integrating-azure-ai-foundry-models-with-github-copilot-using-the-ai-toolkit)
## Improving Code Quality, Refactoring, and Application Modernization
Application Insights for .NET now features Copilot agent integration for automated performance analysis, following last week’s prompt-based code review and modernization efforts. Tutorials provide practical examples of Copilot instructions, prompt files, and refactoring with VS Code, extending previous resources on prompt engineering and moving toward full codebase modernization.
- [Application Insights Code Optimizations: AI-Driven Performance Tuning for .NET Apps](https://devblogs.microsoft.com/dotnet/application-insights-code-optimizations/)
- [Refactor an Existing Codebase Using Prompt-Driven Development with GitHub Copilot](/ai/videos/refactor-an-existing-codebase-using-prompt-driven-development-with-github-copilot)
- [Understanding Instruction and Prompt Files for GitHub Copilot in .NET Development](https://devblogs.microsoft.com/dotnet/prompt-files-and-instructions-files-explained/)
- [Unlocking Application Modernisation with GitHub Copilot](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unlocking-application-modernisation-with-github-copilot/ba/p/4454121)
## GitHub Copilot in IDEs and Developer Experience Enhancements
Agent-based code review and workflow features for Copilot now reach JetBrains and Visual Studio IDEs, carrying forward last week’s progress. Tutorials cover advanced Copilot Chat for full-stack debugging, custom chat linked to MCP servers, and switching between Agent and Ask modes for dynamic code edits.
Spec Kit tools connect development standards to code delivery for improved collaboration—directly evolving earlier workflow standardization efforts. VS Code Dev Days continues with installation tutorials, extension building, and live demonstrations, providing hands-on training in Copilot features.
- [Copilot Code Review Now Available in JetBrains IDEs and Visual Studio](https://github.blog/changelog/2025-09-18-copilot-code-review-now-in-jetbrains-ides-and-visual-studio)
- [VS Code Dev Days: Unlocking AI-Powered Coding with GitHub Copilot](/ai/videos/vs-code-dev-days-unlocking-ai-powered-coding-with-github-copilot)
- [Debugging a Full-Stack Chat App with GitHub Copilot Chat in VS Code](/ai/videos/debugging-a-full-stack-chat-app-with-github-copilot-chat-in-vs-code)
- [Spec Kit and GitHub Copilot: Spec-Driven Development in VS Code with Den](/ai/videos/spec-kit-and-github-copilot-spec-driven-development-in-vs-code-with-den)
- [Spec-Driven Development with GitHub Spec Kit: Streamlining AI-Assisted Coding Workflows](https://devblogs.microsoft.com/blog/spec-driven-development-spec-kit)
- [VS Code Live: Enhancing Presentations and Live Coding with Demo Time](/ai/videos/vs-code-live-enhancing-presentations-and-live-coding-with-demo-time)
## Guardrails, Workflow Governance, and Enterprise Standards
Enterprise guidance expands to workflow governance and AGENTS.md support, sharing practical advice for code review guardrails, versioned instruction files, and project knowledge curation. The trend moves from building basic configurations to defining organization-wide standards and syncing documentation.
- [Disciplined Guardrail Development in Enterprise Applications with GitHub Copilot](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/disciplined-guardrail-development-in-enterprise-application-with/ba/p/4455321)
## Other GitHub Copilot News
GitHub updated Copilot premium request budgets for enterprise and team accounts, improving last week’s quota management for SKUs and giving more admin control. GitHub Sparks updates help teams collaborate securely on read-only workflows with managed data sharing.
New guides explain multi-turn conversational workflows in Copilot Studio, expanding last week’s resources for prompt and context management in large-scale assistant design.
- [Removal of {{TITLE}} Copilot Premium Request Budgets for Enterprise and Team Accounts](https://github.blog/changelog/2025-09-17-upcoming-removal-of-copilot-premium-request-0-budgets-for-enterprise-and-team-accounts)
- [Share Read-Only Sparks with Controlled Data Access](https://github.blog/changelog/2025-09-17-share-read-only-sparks-with-controlled-data-access)
- [Multi-Turn Conversations and Context Management in Copilot Studio](https://dellenny.com/multi-turn-conversations-and-context-management-in-copilot-studio/)',
    'Recent GitHub Copilot updates reinforce the ecosystem’s growth in enterprise integrations, broader MCP standard usage, increased model options, and better context and privacy features. Integrations with Teams, Azure DevOps, Visual Studio Code, and third-party services highlight practical automation and a more effective developer experience.',
    1758524400, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-09-22', 'TechHub',
    'TechHub', '8B86C450FAB3985D3F1AD0BAA2CCE52201E9B6266B54204F91DB813F84E2EACE', ',GitHub Copilot,Copilot Coding Agent,MCP,GitHub MCP Registry,Azure DevOps,Azure Boards,Microsoft Teams,VS Code,VS,JetBrains,Copilot Chat,Claude Opus 4.1,Azure AI Foundry,Foundry Local,.NET,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-09-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-09-15', 'roundups', 'Weekly GitHub Copilot Roundup: Open VS Code, More Models',
    'GitHub Copilot received new updates, including open sourcing of its VS Code extension, expanded control over AI models, and integrations focused on transparency for developers. Automation, security, and configuration options now better support individual and enterprise needs. Tutorials highlight Copilot''s role in telecom APIs, certification study, and open source development. Copilot continues to evolve as a flexible AI coding platform, assisted by community contributions and support for additional models.
<!--excerpt_end-->
## VS Code Copilot Extension: Open Source, Custom Modes, and Deep Customization
Following last week’s focus on MCP integration and workflow automation, the VS Code Copilot Extension is now open source, discussed in detail by Microsoft’s Burke Holland. Community access allows developers to contribute new features and refine daily Copilot usage in VS Code. Custom Modes support tailored AI responses and performance for specific developer workflows. Advanced users can enable ''Beast Mode'' in VS Code Insiders to experiment with prompt engineering and agent tuning.
These improvements build on last week’s MCP-driven automation. Guides and documentation on Custom Modes offer hands-on prompt engineering instructions. Open sourcing welcomes contributions such as code fixes and feature suggestions, letting users directly affect Copilot’s development. Initial coverage explains the extensibility available for customizing Copilot behavior.
- [VS Code Copilot Extension Goes Open Source: Deep Dive with Burke Holland](/ai/videos/vs-code-copilot-extension-goes-open-source-deep-dive-with-burke-holland)
- [Deep Dive into the Open-Sourced VS Code Copilot Extension with Custom Modes](/ai/videos/deep-dive-into-the-open-sourced-vs-code-copilot-extension-with-custom-modes)
## GitHub Copilot in Visual Studio Code: New Features, AI Models, Security, and Workflow Enhancements
After agent and prompt file improvements last week, Visual Studio Code August 2025 (v1.104) introduces more Copilot upgrades for streamlined coding and improved security. Copilot Chat now automatically selects among AI models (GPT-5, Claude Sonnet 4, Gemini Pro 2.5), enhancing suggestion accuracy and dealing with rate limits; manual model selection is also possible. Team collaboration gets refinements such as an advanced agent system, reusable prompts, device-wide toggles, and chat enhancements including KaTeX math rendering and custom fonts. Extension authors benefit from finalized LanguageModelChatProvider APIs and improved authentication.
For security, agent mode prompts users before modifying protected files, and organizations can adjust approvals for terminal commands. Changed files are now easier to review with collapsible lists, and expanded AGENTS.md support allows detailed Copilot usage standards. These additions align with productivity and high-control needs for large and security-sensitive teams.
- [What''s New in Visual Studio Code August 2025 (v1.104)](https://code.visualstudio.com/updates/v1_104)
- [GitHub Copilot in Visual Studio Code August 2025 Update (v1.104): New Features and Security Enhancements](https://github.blog/changelog/2025-09-12-github-copilot-in-vs-code-august-release-v1-104)
## Expanded AI Model Support and BYOK Flexibility
Extending last week’s updates for organizational customization, Copilot now supports OpenAI GPT-5 and GPT-5 mini—GPT-5 mini for Free users, GPT-5 for paid subscriptions. A new model picker UI lets developers and organizations choose models on GitHub.com, VS Code, Visual Studio, JetBrains IDEs, Xcode, and GitHub Mobile. IT administrators can set defaults centrally. This lets users and teams adjust Copilot for anything from basic scripting to complex code generation.
Copilot now supports Bring Your Own Key (BYOK), available in public preview for JetBrains IDEs and Xcode. Developers can use their own API keys for Azure, OpenAI, Anthropic, Gemini, Groq, and OpenRouter—enabling model experimentation and better quota management. Teams can customize Copilot Chat and try out new models before they’re fully supported, with ongoing community feedback shaping future IDE integrations.
- [GitHub Copilot Adds Support for OpenAI GPT-5 and GPT-5 Mini Models](https://github.blog/changelog/2025-09-09-openai-gpt-5-and-gpt-5-mini-are-now-generally-available-in-github-copilot)
- [Public Preview: Bring Your Own Key (BYOK) for GitHub Copilot Chat in JetBrains and Xcode](https://github.blog/changelog/2025-09-11-bring-your-own-key-byok-support-for-jetbrains-ides-and-xcode-in-public-preview)
## GitHub Copilot Coding Agent and Agentic Workflows
Building on agent-driven automation and MCP connections from last week, Copilot’s agent now acts as a coding teammate—running code fixes, refactoring, and other pull request tasks. Developers assign agent tasks via Issues, VS Code, or a special panel, using ephemeral GitHub Actions runners for safe, isolated work. Agents’ changes are reviewed and run through CI before approval, maintaining secure standards. The Model Context Protocol (MCP) provides expanded context for improved results. Teams get management over assignments, audits, and branch protection, with guides outlining setup and scaling.
- [Getting Started with GitHub Copilot Coding Agent and Agentic Workflows](https://github.blog/ai-and-ml/github-copilot/github-copilot-coding-agent-101-getting-started-with-agentic-workflows-on-github/)
## Automatic Code Review and Administration with Copilot and MCP
Copilot’s new repository rule allows separate automatic code review from merge requirements. Repository admins can get early feedback and frequent code quality checks tailored to different rule sets, supporting productivity and flexible coverage—useful for regulated projects.
The MCP remote server (now generally available) links Copilot agents, GitHub workflows, and large language models. Security features include secret scanning, code scanning, and automated advisories. Enterprises can set internal registry and allowlist controls for MCP servers via Copilot in VS Code Insiders, regulating AI endpoint access. These changes make configuration easier and anticipate broader tool support, showing Copilot’s focus on balancing useful AI and governance.
- [Automatic Copilot Code Review: Standalone Repository Rule Now Available](https://github.blog/changelog/2025-09-10-copilot-code-review-independent-repository-rule-for-automatic-reviews)
- [GitHub''s Remote MCP Update: General Availability and Key Integrations](/ai/videos/githubs-remote-mcp-update-general-availability-and-key-integrations)
- [Configuring Internal MCP Registries and Allowlists for Copilot in VS Code Insiders](https://github.blog/changelog/2025-09-12-internal-mcp-registry-and-allowlist-controls-for-vs-code-insiders)
## Copilot for TM Forum Open API and Telco Workflows
Copilot’s application in TM Forum Open API development boosts productivity and standards compliance for telecom APIs—especially with Node.js/Express TMF-compliant endpoints. Copilot streamlines boilerplate creation, testing, and validation. Companies like Proximus, NOS, Orange, and Vodafone report faster development and improved API matching. Best practices stress ongoing validation and keeping features current.
At TM Forum Innovate Americas 2025, Microsoft demonstrated Copilot, Azure AI Foundry, and MCP integration for modular telecom architectures and open standards. Agentic AI and Copilot accelerate API delivery, reduce repetitive work, and support orchestration, showing impact in telecom engineering.
- [Supercharge TM Forum Open API Development with GitHub Copilot](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/supercharge-your-tm-forum-open-api-development-with-github/ba-p/4451366)
- [Reimagining Telco with Microsoft: AI, TM Forum ODA, and Developer Innovation](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/reimagining-telco-with-microsoft-ai-tm-forum-oda-and-developer/ba-p/4451724)
## Tutorials and Practical Guides: Prompt-Driven Development, Building with AI, and Certification Support
A range of resources are now available. Tutorials in VS Code detail Copilot prompt engineering, Copilot Vision, voice interactions, and agent workflows—helping users automate coding and boost test coverage from within the IDE.
Building on last week’s beginner guides and prompt best practices, other guides cover Copilot’s use in open source app prototyping and Microsoft certification study, offering step-by-step automation for documentation, scripting, and custom workflows (like Markdown or PowerShell tasks for cloud automation). Copilot helps developers build skills, onboard to new stacks, and launch open source projects with community support.
- [Introduction to Prompt-Driven Development in VS Code](/ai/videos/introduction-to-prompt-driven-development-in-vs-code)
- [Building Personal Apps with Open Source and AI](https://github.blog/open-source/maintainers/building-personal-apps-with-open-source-and-ai/)
- [How GitHub Copilot Can Boost Your Microsoft Certification Prep](https://dellenny.com/supercharge-your-it-certification-prep-how-github-copilot-can-be-your-study-buddy/)
## Other GitHub Copilot News
GitHub Universe 2025 will feature over 100 sessions about Copilot workflows, automation, and current advances in AI and security. Attendees can view demos, take remote certifications (including new Copilot exams), and meet technical experts, showing Copilot’s growing role in development and automation.
- [Your Guide to GitHub Universe 2025: Event Schedule, Learning, and Certifications Announced](https://github.blog/news-insights/company-news/your-guide-to-github-universe-2025-the-schedule-just-launched/)',
    'GitHub Copilot received new updates, including open sourcing of its VS Code extension, expanded control over AI models, and integrations focused on transparency for developers. Automation, security, and configuration options now better support individual and enterprise needs. Tutorials highlight Copilot''s role in telecom APIs, certification study, and open source development. Copilot continues to evolve as a flexible AI coding platform, assisted by community contributions and support for additional models.',
    1757919600, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-09-15', 'TechHub',
    'TechHub', 'A25FD360411113171C9C907350C41CAB3684510A620E2370BFFDB956793F27C7', ',GitHub Copilot,VS Code,VS Code Extension,Open Source,Custom Modes,Prompt Engineering,AI Model Picker,GPT 5,Claude Sonnet 4,Gemini Pro 2.5,Bring Your Own Key,Copilot Coding Agent,MCP,Automatic Code Review,Enterprise Governance,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-09-08
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-09-08', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, MCP, and Prompt Control',
    'GitHub Copilot introduced updates across its platform, further integrating AI into everyday development activities. These enhancements include better support in IDEs, new ways to manage prompts, autonomous agents, and streamlined enterprise administration. Copilot is evolving toward a multi-modal assistant that supports coding, collaboration, and codebase insights. Notable updates involve the Copilot Agents Panel, the general availability of the remote MCP Server, new agent-driven workflows, and greater adoption of Copilot for team collaboration and modernizing legacy code.
<!--excerpt_end-->
## Copilot Agents and MCP: Expanding Context and Automation
Building on recent improvements in MCP integration and agent workflows, Copilot now features its Agents Panel within the GitHub web interface. This centralizes development and review tasks, reducing context switching and enabling context-aware workflows that were previously available only in desktop environments.
The remote GitHub MCP Server has moved to general availability, featuring standardized OAuth 2.1 with PKCE for secure authentication across IDEs and browsers. Copilot continues to strengthen security with secret scanning and built-in code scanning alerts, further reducing operational risks by extending last week''s added security features.
Centralized automation, robust authentication, and policy-based team collaboration enhance resource management, following the ongoing move toward scalable coding and agent workflows.
- [A First Look at the New Copilot Agents Panel on GitHub](/ai/videos/a-first-look-at-the-new-copilot-agents-panel-on-github)
- [Remote GitHub MCP Server Now Generally Available](https://github.blog/changelog/2025-09-04-remote-github-mcp-server-is-now-generally-available)
- [How to Debug a Web App Using Playwright MCP and GitHub Copilot](https://github.blog/ai-and-ml/github-copilot/how-to-debug-a-web-app-with-playwright-mcp-and-github-copilot/)
- [Building Smarter AI Tool Interactions with MCP Elicitation](https://github.blog/ai-and-ml/github-copilot/building-smarter-interactions-with-mcp-elicitation-from-clunky-tool-calls-to-seamless-user-experiences/)
## Copilot Coding Agent in Practice
The features in Copilot Coding Agent build upon previous automation resources, extending agent-driven workflows for .NET automation and backlog management. This enhances Copilot’s usefulness on both desktop and cloud platforms.
New documentation covers tasks like identifying gaps in unit test coverage, automating pull requests, and reviewing agent logs—improving on earlier approaches to team collaboration and sub-task management. Integrating the Playwright MCP Server further broadens the agent''s use in debugging and extensibility.
By adopting agent-driven processes, teams can minimize repetitive tasks, address legacy code, and coordinate remote updates—mirroring recent trends in robust automation.
- [Automating .NET Development with GitHub Copilot Coding Agent](https://devblogs.microsoft.com/dotnet/copilot-coding-agent-dotnet/)
- [What''s New with the GitHub Copilot Coding Agent](/ai/videos/whats-new-with-the-github-copilot-coding-agent)
- [From Issue to PR: Asynchronously Develop with GitHub Copilot Coding Agent](/ai/videos/from-issue-to-pr-asynchronously-develop-with-github-copilot-coding-agent)
## Copilot in IDEs: Visual Studio and Eclipse Enhancements
Recent improvements in prompt handling and model selection in Visual Studio and JetBrains IDEs continue with updates to Visual Studio 17.14 and Eclipse. The new Output Window integration allows developers to query and understand logs directly. Reusable prompt files make prompt management more efficient for teams.
Eclipse has added support for custom instructions, enhanced APIs, and image context, strengthening its multi-model backend and agent scripting. These updates broaden Copilot’s compatibility across various IDEs and plugin systems.
- [Make Sense of Your Output Window with Copilot in Visual Studio](https://devblogs.microsoft.com/visualstudio/make-sense-of-your-output-window-with-copilot/)
- [Boost Your Copilot Collaboration with Reusable Prompt Files](https://devblogs.microsoft.com/visualstudio/boost-your-copilot-collaboration-with-reusable-prompt-files/)
- [Turning GitHub Copilot Prompts into Executable Files in VS Code](/ai/videos/turning-github-copilot-prompts-into-executable-files-in-vs-code)
- [New Features in GitHub Copilot for Eclipse Empower Developer Experience](https://github.blog/changelog/2025-09-05-new-features-in-github-copilot-in-eclipse)
## Code Review and Customization: Instructions at Scale
Copilot now allows path-scoped instruction files in code reviews, providing more targeted feedback for larger codebases. This shift from ad-hoc to modular settings builds on previous support for project and organization-level customization.
Guides recommend offering detailed instructions about project context, technology stack, and coding conventions, reinforcing the trend toward thorough, actionable reviews.
- [Path-Scoped Custom Instructions for Copilot Code Review](https://github.blog/changelog/2025-09-03-copilot-code-review-path-scoped-custom-instruction-file-support)
- [5 Tips for Crafting Better Custom Instructions for GitHub Copilot](https://github.blog/ai-and-ml/github-copilot/5-tips-for-writing-better-custom-instructions-for-copilot/)
## Enterprise Teams and Business License Management
Copilot''s Enterprise Teams feature on GitHub Enterprise Cloud (now in public preview) builds on prior advances in data residency and licensing. It provides more detailed access controls, automated license assignment, and onboarding improvements, all reflecting the evolution of enterprise identity and workflow management.
These updates help organizations allocate licenses, control permissions, and track Copilot use—supporting earlier improvements in preparing for enterprise adoption.
- [Managing GitHub Copilot Business Licenses with Enterprise Teams (Public Preview)](https://github.blog/changelog/2025-09-04-manage-copilot-and-users-via-enterprise-teams-in-public-preview)
## Prompt and Spec-Driven Workflows
The Spec Kit enables a more structured approach to development, shifting from prompt-driven to spec-driven workflows. This continues last week''s focus on Spec Kit and automation. Spec Kit organizes projects by specification and modular tasks, building on past structured coding guidance.
CLI and IDE integration keep the focus on reliable, validated development and support translating requirements into reliable code for new and legacy projects.
- [Adopting Spec-Driven Development with AI: Introducing Spec Kit](https://github.blog/ai-and-ml/generative-ai/spec-driven-development-with-ai-get-started-with-a-new-open-source-toolkit/)
## Copilot-Assisted Migration and Legacy Modernization
Copilot is being used more often for migrating legacy and enterprise systems, with new guides covering reverse engineering, generating documentation, and automated testing. These resources help teams approach incremental migrations safely and clarify Copilot’s value for large codebase transitions.
Copilot’s suggestions support planning and executing modernization projects, in line with earlier updates.
- [Modernizing Legacy COBOL to Cloud with GitHub Copilot](/ai/videos/modernizing-legacy-cobol-to-cloud-with-github-copilot)
- [How to Migrate Legacy Applications Using GitHub Copilot](https://dellenny.com/how-to-migrate-legacy-applications-using-github-copilot/)
## Developer Education and Onboarding
This week’s focus on education builds on existing learning resources, offering clear "top 10" Copilot workflows and guidance on requirement gathering and prompt writing. The goal is to provide structured, actionable onboarding for developers.
Additional topics such as RegEx validation and conducting Agent Mode code reviews support the growing role of Copilot for automation and efficient onboarding.
- [Top 10 Things You Can Do with GitHub Copilot as a New Developer](https://dellenny.com/top-10-things-you-can-do-with-github-copilot-as-a-new-developer-2/)
- [Using GitHub Copilot Chat to Write Better Regex Patterns](https://cooknwithcopilot.com/blog/draft-smarter-regex-without-the-headaches.html)
- [How to Get the Most Out of Your AI with Vibe Coding](/ai/videos/how-to-get-the-most-out-of-your-ai-with-vibe-coding)',
    'GitHub Copilot introduced updates across its platform, further integrating AI into everyday development activities. These enhancements include better support in IDEs, new ways to manage prompts, autonomous agents, and streamlined enterprise administration. Copilot is evolving toward a multi-modal assistant that supports coding, collaboration, and codebase insights. Notable updates involve the Copilot Agents Panel, the general availability of the remote MCP Server, new agent-driven workflows, and greater adoption of Copilot for team collaboration and modernizing legacy code.',
    1757314800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-09-08', 'TechHub',
    'TechHub', '0ECE7E4C46D892886764F54C7ECCCBAC9D726CC7A3F9571BC157B8BF294C11BC', ',GitHub Copilot,Copilot Agents Panel,MCP,Remote MCP Server,OAuth 2.1,PKCE,Copilot Coding Agent,.NET,Visual Studio 17.14,VS Code,Eclipse IDE,Copilot Code Review,Custom Instruction Files,Enterprise Teams,Spec Kit,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-09-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-09-01', 'roundups', 'Weekly GitHub Copilot Roundup: Models, MCP, Agents, Security',
    'This week, GitHub Copilot received a series of updates, expanding how developers can use AI throughout their workflow. The enhancements include more options for model selection, agent management, automation tools for enterprise workflows, and new security features. These releases cover Visual Studio 2022, VS Code, JetBrains IDEs, and macOS/web environments—and bring additional AI models like GPT-5 and Grok Code Fast 1. A focus on security introduces prompt injection checks and improved defense strategies. Documentation and new walkthroughs provide clear steps for developers adopting these tools.
<!--excerpt_end-->
## GitHub Copilot in Visual Studio: Updates, Models, and Agentic Workflows
Visual Studio 2022 received its August 2025 update with Copilot positioned for deeper integration with Microsoft’s developer platforms. MCP is now a standard feature, supporting secure, custom AI workflows via `.mcp.json` files. Users have access to one-click MCP server installations, expanded model selection, and more API choices (OpenAI, Google, Anthropic). Copilot Chat adds snippet retrieval, history-based context, and smarter search—all improving debugging and collaborative workflows. Upcoming features previewed for September include AI Profiler and Debugger Agents, faster model switching, and updated policy controls. These changes help Visual Studio serve as a central platform for building customized AI-powered workflows.
- [August 2025 Update: GitHub Copilot Advancements in Visual Studio 2022](https://github.blog/changelog/2025-08-28-github-copilot-in-visual-studio-august-update)
- [Visual Studio 2022 August 2025 Update: GPT-5, MCP Integration, Copilot Enhancements, and Improved Debugging](https://devblogs.microsoft.com/visualstudio/the-visual-studio-august-update-is-here-smarter-ai-better-debugging-and-more-control/)
- [Roadmap for AI and GitHub Copilot in Visual Studio: September Update](https://devblogs.microsoft.com/visualstudio/roadmap-for-ai-in-visual-studio-september/)
- [GitHub Copilot for Azure Public Preview Launched in Visual Studio 2022 with MCP](https://devblogs.microsoft.com/visualstudio/github-copilot-for-azure-preview-launches-in-visual-studio-2022-with-azure-mcp-support/)
## Model Flexibility and Agentic Infrastructure: New AI Models, MCP, and Customization
Building on previous multi-model support, Copilot’s latest preview supports GPT-5 Mini and Grok Code Fast 1, giving users more backend options—including choices like Gemini 2.5 Pro. Organizations can now set enterprise-wide model defaults, and new MCP features such as AGENTS.md enable per-project instructions. Tutorials offer guidance on adapting workflows for monorepos or microservices, simplifying the setup of specialized AI agents and supporting more robust development environments.
- [OpenAI GPT-5 Mini Available in GitHub Copilot for Visual Studio and More](https://github.blog/changelog/2025-08-28-openai-gpt-5-mini-is-now-available-in-public-preview-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Public Preview: Grok Code Fast 1 Model Now Available in GitHub Copilot](https://github.blog/changelog/2025-08-26-grok-code-fast-1-is-rolling-out-in-public-preview-for-github-copilot)
- [Copilot Coding Agent Adds AGENTS.md Custom Instruction Support](https://github.blog/changelog/2025-08-28-copilot-coding-agent-now-supports-agents-md-custom-instructions)
- [Announcing Awesome Copilot MCP Server: Customizing GitHub Copilot Like a Pro](https://devblogs.microsoft.com/blog/announcing-awesome-copilot-mcp-server)
- [MCP Servers in VS Code and GitHub Copilot](/ai/videos/mcp-servers-in-vs-code-and-github-copilot)
- [Installing the GitHub MCP Server in Visual Studio Code](/ai/videos/installing-the-github-mcp-server-in-visual-studio-code)
## Copilot as an Agent: Automated Coding, Code Review, and Collaborative Features
Copilot’s functionality as a coding agent extends to GitHub Enterprise Cloud, now supporting data residency policies and improved security controls for business automation. The Agents Panel is expanding, Raycast integration adds desktop productivity options, and code review enhancements in Xcode give administrators new controls. Sub-issue management in Copilot Chat introduces a structured framework for conversation-based agile planning, supporting recent improvements in collaborative work.
- [Copilot Coding Agent Now in GitHub Enterprise Cloud with Data Residency](https://github.blog/changelog/2025-08-27-copilot-coding-agent-is-now-available-in-github-enterprise-cloud-with-data-residency)
- [The Download: GitHub Copilot Agents, Actions Security, and Vite''s npm Milestone](/ai/videos/the-download-github-copilot-agents-actions-security-and-vites-npm-milestone)
- [Start and Track GitHub Copilot Coding Agent Tasks from Raycast](https://github.blog/changelog/2025-08-28-start-and-track-copilot-coding-agent-tasks-from-raycast)
- [Copilot Code Review: Now Available in Xcode and Enterprise Admin Controls](https://github.blog/changelog/2025-08-27-copilot-code-review-generally-available-in-xcode-and-new-admin-control)
- [Managing Sub-Issues with GitHub Copilot: Public Preview Update](https://github.blog/changelog/2025-08-27-create-sub-issues-with-copilot-in-public-preview)
## Security, Reliability, and Safe Adoption
Security improvements include prompt injection threat assessments for Copilot Chat in VS Code, mitigations based on workspace trust, agent transparency, and domain controls. Building on earlier improvements in secret scanning and container development, the coverage illustrates a layered approach to security. One engineering study reports Copilot accelerating secret token validation and pull request creation, improving coverage and offering tangible results for secure code practices.
- [Safeguarding VS Code Against Prompt Injections: Securing GitHub Copilot Chat](https://github.blog/security/vulnerability-research/safeguarding-vs-code-against-prompt-injections/)
- [How GitHub Copilot Accelerated Secret Protection Engineering](https://github.blog/ai-and-ml/github-copilot/how-we-accelerated-secret-protection-engineering-with-copilot/)
## Copilot’s Model Architecture, Developer Experience, and Practical Guides
Copilot now gives Pro+, Business, and Enterprise users the ability to choose between models such as GPT-4.1, Claude, and Gemini. The update includes new features for code completion, quick instruction generation, and expanded shortcuts, all supporting everyday development tasks. Practical guides and resources help teams integrate Copilot responsibly, with examples for onboarding, day-to-day prompts, and scaling best practices.
- [Inside GitHub Copilot: The AI Models Behind Your Coding Assistant](https://github.blog/ai-and-ml/github-copilot/under-the-hood-exploring-the-ai-models-powering-github-copilot/)
- [GitHub Copilot Code Completion Now Powered by GPT-4.1 Model](https://github.blog/changelog/2025-08-27-copilot-code-completion-now-uses-the-gpt-4-1-copilot-model)
- [Create GitHub Copilot Instructions in Just One Click](/ai/videos/create-github-copilot-instructions-in-just-one-click)
- [Mastering GitHub Copilot: Tips, Shortcuts, and Prompts That Work](https://dellenny.com/mastering-github-copilot-tips-shortcuts-and-prompts-that-work/)
- [How to Use GitHub Copilot on github.com: A Power User’s Guide](https://github.blog/ai-and-ml/github-copilot/how-to-use-github-copilot-on-github-com-a-power-users-guide/)
- [Boost Code Quality Fast with GitHub Copilot Edit Mode](https://cooknwithcopilot.com/blog/use-edit-mode-for-quick-targeted-improvements.html)
- [How to Automate Code Reviews and Testing with GitHub Copilot](/ai/videos/how-to-automate-code-reviews-and-testing-with-github-copilot)
- [Managing Your GitHub Enterprise: Policies, Copilot, and Security Settings](/ai/videos/managing-your-github-enterprise-policies-copilot-and-security-settings)
- [Using GitHub Copilot to Teach Programming: A New Approach for Educators](https://dellenny.com/using-github-copilot-to-teach-programming-a-new-approach-for-educators/)
## Copilot Integrations and Developer Event Highlights
Broader public previews rolled out for Copilot integrations in Visual Studio 2022, JetBrains IDEs, and expanded MCP support in VS Code. JetBrains gets Next Edit Suggestions (NES); VS Code launches Spec Kit for specification-driven development, continuing efforts in workflow automation and collaboration. VS Code Dev Days offer educational events for practitioners, building on previous GitHub Universe gatherings to promote skill sharing and community learning.
- [Next Edit Suggestions (NES) with GitHub Copilot for JetBrains IDEs: Public Preview](https://github.blog/changelog/2025-08-29-copilots-next-edit-suggestion-nes-in-public-preview-in-jetbrains)
- [Introducing Spec Kit for Spec-Driven Development in VS Code](/ai/videos/introducing-spec-kit-for-spec-driven-development-in-vs-code)
- [VS Code Dev Days: Explore AI-Assisted Development with GitHub Copilot](https://code.visualstudio.com/blogs/2025/08/27/vscode-dev-days)
## Other GitHub Copilot News
Workflows remain a main focus, with fresh guides showing how to enable full agentic cycles and use advanced debugging features—expanding on coverage for agent scripting and MCP customization. The .NET AI Community Standup and a review of MCP comparisons provide additional insights for hybrid and enterprise Copilot implementation.
- [End-to-End Agentic Development with GitHub Copilot: A Developer Workflow Demo](/ai/videos/end-to-end-agentic-development-with-github-copilot-a-developer-workflow-demo)
- [Automate Debugging with the Playwright MCP Server and GitHub Copilot](/ai/videos/automate-debugging-with-the-playwright-mcp-server-and-github-copilot)
- [.NET AI Community Standup: AI Tools Every .NET Dev Needs](/ai/videos/net-ai-community-standup-ai-tools-every-net-dev-needs)',
    'This week, GitHub Copilot received a series of updates, expanding how developers can use AI throughout their workflow. The enhancements include more options for model selection, agent management, automation tools for enterprise workflows, and new security features. These releases cover Visual Studio 2022, VS Code, JetBrains IDEs, and macOS/web environments—and bring additional AI models like GPT-5 and Grok Code Fast 1. A focus on security introduces prompt injection checks and improved defense strategies. Documentation and new walkthroughs provide clear steps for developers adopting these tools.',
    1756710000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-09-01', 'TechHub',
    'TechHub', '800C064E5866CA969298522FE992BFDA55CA55A56743EEDD738BA558EE768BFC', ',GitHub Copilot,Visual Studio 2022,VS Code,JetBrains IDEs,Xcode,MCP,Agents,GPT 5 Mini,Grok Code Fast 1,GPT 4.1,Claude,Gemini,Enterprise Policies,Prompt Injection Defense,Data Residency,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-08-25
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-08-25', 'roundups', 'Weekly GitHub Copilot Roundup: Models, MCP, and Agents',
    'GitHub Copilot delivered updated features this week, introducing premium model options, customizable workflows with Model Context Protocol (MCP), and new code editing capabilities to support more personalized AI-assisted coding. GitHub refined controls, extended automation, and consolidated context management in response to user feedback. Copilot’s transition from legacy environments to new collaboration solutions is ongoing, with organizations gaining more oversight for policy and spending, as illustrated by new case studies.
<!--excerpt_end-->
## GitHub Copilot: New Models, Automation Workflows, and Core Feature Updates
Gemini 2.5 Pro is now available to all Copilot Pro/Pro+, Business, and Enterprise users. GitHub continues to support flexible, multi-model work with Gemini, OpenAI’s latest models, and more, enabling users to choose the AI that fits their work best. Gemini is selectable in Copilot Chat across major IDEs including VS Code, Visual Studio, JetBrains, Xcode, and Eclipse, and organization administrators can set Gemini as the default, following similar GPT-5 organization rollouts.
Copilot provides commit message suggestions in the GitHub web editor for all users, moving automation beyond code writing to workflow documentation. Copilot Spaces improvements add precision in linking to repositories, directories, files, and branches, further supporting collaboration. These updates expand on earlier context management updates, making Spaces more practical for day-to-day team work.
Workflow automation receives an upgrade with the Copilot coding agent and the Agents panel, available to all Copilot paid users. Developers can now assign tasks, track status, and manage pull request drafts directly on GitHub.com, shifting Copilot’s capability from code suggestion toward comprehensive project automation.
- [Gemini 2.5 Pro Model Now Available for GitHub Copilot Users](https://github.blog/changelog/2025-08-19-gemini-2-5-pro-is-generally-available-in-copilot)
- [GitHub Copilot Introduces Commit Message Suggestions and Spaces Enhancements](https://github.blog/changelog/2025-08-21-copilot-generated-commit-messages-on-github-com-is-in-public-preview)
- [Agents Panel: Easily Delegate Tasks to Copilot Coding Agent Across GitHub](https://github.blog/changelog/2025-08-19-agents-panel-launch-copilot-coding-agent-tasks-anywhere-on-github-com)
- [Agents Panel: Delegate Copilot Coding Agent Tasks Anywhere on GitHub](https://github.blog/news-insights/product-news/agents-panel-launch-copilot-coding-agent-tasks-anywhere-on-github/)
## Copilot in Visual Studio and .NET Ecosystem: Diagnostics, Controls, and Integration
Visual Studio continues to improve Copilot for .NET with expanded debugging and smarter context support. The Copilot Diagnostics tool now brings AI-powered debugging—with automated breakpoints, LINQ analysis, and focused profiling for CPU and memory. Aspire 9.3 now includes Copilot for live troubleshooting and log review, following last week''s integration of Copilot into cloud-native .NET applications.
Refined Copilot controls in Visual Studio respond directly to developer feedback. Updated delay settings for completions, selective suggestion handling, and keyboard shortcuts help developers maintain control over automated code suggestions.
Model Context Protocol (MCP) integration in Visual Studio has reached general availability. Using `.mcp.json` files, developers can now connect to custom or community MCP servers, simplifying adoption of secure context automation.
- [Copilot Diagnostics Toolset Enhances .NET Debugging in Visual Studio](https://devblogs.microsoft.com/dotnet/github-copilot-diagnostics-toolset-for-dotnet-in-visual-studio/)
- [GitHub Copilot Integration in .NET Aspire 9.3 Dashboard](/ai/videos/github-copilot-integration-in-net-aspire-93-dashboard)
- [GitHub Copilot Now Integrated Into .NET Aspire Dashboard](/ai/videos/github-copilot-now-integrated-into-net-aspire-dashboard)
- [Better Control Over GitHub Copilot Code Suggestions in Visual Studio](https://devblogs.microsoft.com/visualstudio/better-control-over-your-copilot-code-suggestions/)
- [Model Context Protocol (MCP) Is Now Generally Available in Visual Studio](https://devblogs.microsoft.com/visualstudio/mcp-is-now-generally-available-in-visualstudio/)
## Model Context Protocol (MCP): Advanced Extensibility and Custom AI Workflows
Building on prior enterprise automation news, this week introduces detailed guides and general availability for custom MCP servers. With MCP, Copilot can connect to internal APIs and business applications, supporting tailored automation. Real examples—such as a TypeScript server for turn-based games or automated class naming—demonstrate how teams can use MCP to apply domain-specific automation within their preferred IDEs.
With full support for MCP in Visual Studio, organizations can better manage secure orchestration and contextual workflows, especially where process control is essential.
- [Building Your First MCP Server: Extending GitHub Copilot with Custom Tools](https://github.blog/ai-and-ml/github-copilot/building-your-first-mcp-server-how-to-extend-ai-tools-with-custom-capabilities/)
- [Generating Classes with Custom Naming Conventions Using GitHub Copilot and a Custom MCP Server](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/generating-classes-with-custom-naming-conventions-using-github/ba-p/4444837)
- [Model Context Protocol (MCP) Is Now Generally Available in Visual Studio](https://devblogs.microsoft.com/visualstudio/mcp-is-now-generally-available-in-visualstudio/)
## Copilot Spaces and Context Management: Transitioning from Knowledge Bases to Spaces
Copilot Spaces is scheduled to fully replace the former knowledge base system by September 2025. This change consolidates Copilot’s context features into a unified platform. Spaces now support linking to code, documentation, images, issues, pull requests, and more, allowing for comprehensive context and information sharing. Users should move their content using migration tools before retirement. This change makes Spaces the central location for team-oriented, context-driven AI workflows.
- [GitHub Copilot Knowledge Bases Retiring: Transition to Copilot Spaces](https://github.blog/changelog/2025-08-20-sunset-notice-copilot-knowledge-bases)
## Everyday Productivity, Workflow Automation, and Developer Experience
New resources this week emphasize Copilot’s impact on productivity within .NET and workflows involving multiple files and TDD. Guides show how Copilot aids in tracing dependencies, maintaining cross-file consistency, and organizing large projects. The system now provides support for TDD, prototyping, and code scaffolding by improving its understanding of file relationships.
Recent case studies, such as Bank Galicia, demonstrate that Copilot is increasingly essential for teams seeking more reliable and faster delivery, evolving from small pilots to standard practice.
- [Work Smarter Across Multiple Files with GitHub Copilot](https://cooknwithcopilot.com/blog/work-smarter-across-multiple-files.html)
- [Boosting Productivity with GitHub Copilot: Real-World .NET Coding Examples](https://dellenny.com/boosting-productivity-with-github-copilot-real-world-net-coding-examples/)
- [How GitHub Copilot Helps with Test-Driven Development (TDD)](https://dellenny.com/how-github-copilot-helps-with-test-driven-development-tdd/)
- [How to Test Nonexistent Code with GitHub Copilot](/ai/videos/how-to-test-nonexistent-code-with-github-copilot)
- [Software Developers in Argentina’s Financial Sector Boost Innovation with GitHub Copilot and AI](https://news.microsoft.com/source/latam/features/ai/galicia-naranja-x-github-copilot/?lang=en)
## Copilot Customization: Bring Your Own Models and VS Code Personalization
Visual Studio Chat now lets developers integrate third-party models (such as OpenAI, Anthropic, or Gemini) by providing their API keys and selecting from available models, currently for chat-based use only. This builds on Copilot’s existing support for a growing set of models, including GPT-5, Claude, and Gemini. Copilot plans further flexibility for model selection.
Recent guides for VS Code Joyride and Copilot demonstrate workflow personalization. Developers use Copilot within scripts and personalized tools, reflecting a wider trend of individualized automation routines.
- [Bring Your Own Language Model to Visual Studio Chat](https://devblogs.microsoft.com/visualstudio/bring-your-own-model-visual-studio-chat/)
- [VS Code Live: Scripting with Joyride and GitHub Copilot](/ai/videos/vs-code-live-scripting-with-joyride-and-github-copilot)
## Other GitHub Copilot News
New for Business and Enterprise customers, Copilot now features a request overage policy. This lets administrators set usage boundaries or enable overage pricing to control expenses—a capability needed as Copilot adoption grows in organizations.
The announcement of GitHub Universe 2025 highlights technical sessions on Copilot, Actions, security, and community learning. As with past conferences, these sessions aim to help teams optimize their use of GitHub’s AI and collaboration tools at scale.
- [GitHub Copilot Business and Enterprise: Premium Request Overage Policy Now Available](https://github.blog/changelog/2025-08-22-premium-request-overage-policy-is-generally-available-for-copilot-business-and-enterprise)
- [Explore GitHub Universe 2025: Dev Tools, Community Spaces, and More](https://github.blog/news-insights/company-news/explore-the-best-of-github-universe-9-spaces-built-to-spark-creativity-connection-and-joy/)',
    'GitHub Copilot delivered updated features this week, introducing premium model options, customizable workflows with Model Context Protocol (MCP), and new code editing capabilities to support more personalized AI-assisted coding. GitHub refined controls, extended automation, and consolidated context management in response to user feedback. Copilot’s transition from legacy environments to new collaboration solutions is ongoing, with organizations gaining more oversight for policy and spending, as illustrated by new case studies.',
    1756105200, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-08-25', 'TechHub',
    'TechHub', 'FFA6A25E8738F176EDC24EA3F6E0A22ED7F451DF1967C9C00466132673CFA15E', ',GitHub Copilot,Copilot Chat,Gemini 2.5 Pro,GPT 5,Claude,MCP,MCP Servers,Agents Panel,Copilot Coding Agent,Copilot Spaces,VS,VS Code,.NET Aspire,Copilot Diagnostics,Enterprise Policy And Spend Controls,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-08-18
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-08-18', 'roundups', 'Weekly GitHub Copilot Roundup: GPT-5, MCP, and Repo-Wide Context',
    'GitHub Copilot reached a new level of integration this week, thanks to GPT-5 and the leaner “Mini” model now powering Copilot in all major IDEs—including Visual Studio, VS Code, JetBrains, Xcode, and Eclipse—through the Model Context Protocol (MCP). Developers get context-aware AI for writing code, refactoring, and automating projects, plus easier onboarding, API integration, and modernization. Copilot is moving beyond code suggestions to become a real platform for automation, secure collaboration, and better admin controls—raising the standard for quick, high-quality, AI-supported development.
<!--excerpt_end-->
## GPT-5 Arrives Across GitHub Copilot and Major IDEs
GPT-5 is now available across GitHub Copilot, enhancing code completion, context handling, and automation in Visual Studio, VS Code, JetBrains IDEs, Xcode, Eclipse, and the GitHub apps. Visual Studio users will notice better reasoning in complex code, debugging help, faster suggestions, and stronger explanations. The rollout is phased—paid users will see GPT-5 rolled out first, with enterprise admin controls for adoption. Upgrades mean smoother transitions from older models, improved code quality, better onboarding, more effective code reviews, and easier maintenance. GitHub is providing clear changelogs and guides to help users through changes.
- [GPT-5 Comes to GitHub Copilot in Visual Studio](https://devblogs.microsoft.com/visualstudio/gpt-5-now-available-in-visual-studio/)
- [OpenAI GPT-5 Now Available to GitHub Copilot Users in Major IDEs](https://github.blog/changelog/2025-08-12-openai-gpt-5-is-now-available-in-public-preview-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [GPT-5 Now Available in GitHub Copilot: Advanced Features and How to Enable](/ai/videos/gpt-5-now-available-in-github-copilot-advanced-features-and-how-to-enable)
- [GPT-5 and Claude 4.1 Arrive in GitHub Copilot, TypeScript 5.9 Updates, and Community News](/ai/videos/gpt-5-and-claude-41-arrive-in-github-copilot-typescript-59-updates-and-community-news)
## Automation and Developer Workflows: From Natural Language to Real Code
Copilot combines GPT-5 and MCP for closer DevOps alignment. As shown this week, Copilot can now generate full games from natural language prompts in under a minute. MCP lets Copilot fire off real GitHub actions—handling repository management, issue triage, tool integrations—straight from the IDE, so developers avoid context switching. New guides help teams set up MCP securely and expand Copilot’s role from code generation to full automation, boosting project best practices.
- [Building a Game in 60 Seconds with GPT-5 in GitHub Copilot and MCP Server](https://github.blog/ai-and-ml/generative-ai/gpt-5-in-github-copilot-how-i-built-a-game-in-60-seconds/)
- [Announcing the NuGet MCP Server Preview: Real-Time NuGet Package Management with AI Integration](https://devblogs.microsoft.com/dotnet/nuget-mcp-server-preview/)
- [Why We Open Sourced Our MCP Server and What It Means for Developers](https://github.blog/open-source/maintainers/why-we-open-sourced-our-mcp-server-and-what-it-means-for-you/)
## Contextual Collaboration and Code Understanding Expands
New collaboration features in GitHub Copilot now allow repo chat, contextual Q&A, and Copilot Spaces with repository imports. Developers can interact with full repositories via chat, open pull requests and issues, and handle projects using AI suggestions—streamlining both onboarding and maintenance. These features come directly from community feedback wanting easier integration and more context-aware development.
- [How to Chat with Your Repo & Create PRs with Copilot on GitHub](/ai/videos/how-to-chat-with-your-repo-and-create-prs-with-copilot-on-github)
- [Copilot Spaces Now Support Adding Entire Repositories](https://github.blog/changelog/2025-08-13-add-repositories-to-spaces)
## Advanced Protocol and IDE Integration: MCP and Semantic Search
MCP support now extends to JetBrains, Eclipse, and Xcode, enabling organizations to manage secure, policy-controlled, multi-context workflows. Visual Studio Copilot Chat introduces semantic search, moving past keyword search to give meaning-based code results—improving navigation and making code review and summarization more effective as features continue to grow.
- [Model Context Protocol (MCP) Support for GitHub Copilot Now Available in JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-08-13-model-context-protocol-mcp-support-for-jetbrains-eclipse-and-xcode-is-now-generally-available)
- [Enhancing Visual Studio Copilot Chat with Semantic Code Search](https://devblogs.microsoft.com/visualstudio/improving-codebase-awareness-in-visual-studio-chat/)
## Specialized AI Tools and Automation Modes
Copilot now includes a “Do Epic Shit” chat mode (“Beast Mode”), organizing automation with step-by-step checklists that round out the original agent workflows. AI coding assistants built for platforms like Telerik and KendoUI now provide tailored completions for users working in those ecosystems.
- [Do Epic Shit Chat Mode: Beast Mode for GitHub Copilot](https://harrybin.de/posts/do-epic-shit-chat-mode/)
- [VS Code Live: Telerik & KendoUI AI Coding Assistants and Contextual AI Integration](/ai/videos/vs-code-live-telerik-and-kendoui-ai-coding-assistants-and-contextual-ai-integration)
## Modernization and Migration: AI-Driven Refactoring for Enterprise Stacks
Copilot is now automating modernization for enterprise Java and .NET codebases. The App Modernization Extension, using OpenRewrite AI, plans migration, checks dependencies, scaffolds test suites, and confirms compliance automatically. This removes some pain from upgrading legacy applications, following last week’s in-depth guides and ongoing enterprise feedback.
- [Modernizing Legacy Java Applications with GitHub Copilot App Modernization Upgrade](https://techcommunity.microsoft.com/t5/microsoft-developer-community/modernizing-legacy-java-project-using-github-copilot-app/ba-p/4440777)
- [Modernizing and Upgrading Your .NET Apps with Visual Studio and Copilot-Powered AI Tools](/ai/videos/modernizing-and-upgrading-your-net-apps-with-visual-studio-and-copilot-powered-ai-tools)
## Streamlined and Secure: API, Secrets, and Admin Experience
Copilot has upgraded its user management APIs to include a `last_authenticated_at` field, providing real-time compliance and licensing checks instead of slow CSV exports. AI secret scanning is now more accurate, identifying a wider variety of secret types—including custom tokens—and suggesting faster fixes, making pipelines more secure by default.
- [GitHub Copilot User Management API Adds last_authenticated_at Field](https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api)
- [What is GitHub Secret Protection?](/ai/videos/what-is-github-secret-protection-github-explained)
## Other GitHub Copilot News
The GPT-5 Mini version is now available for every Copilot plan, including free ones. This lightweight, quick model helps reduce quota usage for paid tiers while giving everyone easier access to AI features. Ongoing feedback will inform future improvements.
- [GPT-5 Mini Launches in Public Preview for GitHub Copilot Users](https://github.blog/changelog/2025-08-13-gpt-5-mini-now-available-in-github-copilot-in-public-preview)
Copilot’s platform is growing—Claude 4.1 joins GPT-5 to support code intelligence, along with more support for different runtimes and open models. Project management is tied into Microsoft’s Community Project (MCP). Copilot’s role in .NET and Visual Studio was a key focus at events like VS Live!, highlighting its growing reach.
- [GPT-5 and Claude 4.1 Arrive in GitHub Copilot, TypeScript 5.9 Updates, and Community News](/ai/videos/gpt-5-and-claude-41-arrive-in-github-copilot-typescript-59-updates-and-community-news)
- [VS Live! Recap: Visual Studio, GitHub Copilot, and Azure AI Session Highlights](https://devblogs.microsoft.com/visualstudio/from-redmond-to-san-diego-vs-live-highlights-session-examples-and-whats-next/)
Practical tutorials this week included streamlining API integration with Copilot and tackling broken migrations using Copilot’s AI debugging tools—two useful areas for boosting day-to-day productivity.
- [Speed Up API Integration with GitHub Copilot](https://cooknwithcopilot.com/blog/speed-up-api-integration.html)
- [Fix Broken Migrations with AI Debugging in VS Code Using GitHub Copilot](https://techcommunity.microsoft.com/t5/educator-developer-blog/fix-broken-migrations-with-ai-powered-debugging-in-vs-code-using/ba-p/4439418)
As new pull request summarization features arrive, GitHub is deprecating text completion for pull request descriptions. Organizations should keep an eye on update channels to catch the latest changes.
- [GitHub Copilot Text Completion for Pull Request Descriptions to Be Deprecated](https://github.blog/changelog/2025-08-15-deprecating-copilot-text-completion-for-pull-request-descriptions)',
    'GitHub Copilot reached a new level of integration this week, thanks to GPT-5 and the leaner “Mini” model now powering Copilot in all major IDEs—including Visual Studio, VS Code, JetBrains, Xcode, and Eclipse—through the Model Context Protocol (MCP). Developers get context-aware AI for writing code, refactoring, and automating projects, plus easier onboarding, API integration, and modernization. Copilot is moving beyond code suggestions to become a real platform for automation, secure collaboration, and better admin controls—raising the standard for quick, high-quality, AI-supported development.',
    1755500400, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-08-18', 'TechHub',
    'TechHub', '93E44FD51EDE7CFC3DCA2CE6CDC99BBB761B259A44A1EBB3707459A99177C4FD', ',GitHub Copilot,GPT 5,GPT 5 Mini,MCP,VS,VS Code,JetBrains,Xcode,Eclipse,Copilot Chat,Semantic Search,Copilot Spaces,GitHub Actions,Secret Scanning,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-08-11
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-08-11', 'roundups', 'Weekly GitHub Copilot Roundup: GPT-5, Opus 4.1, and Agent Workflows',
    'Building on last week’s momentum in agent workflows and persistent memory, GitHub Copilot rolled out major updates, broadening its lead as an AI-powered developer tool. This week, public preview integrations for OpenAI GPT-5 and Anthropic Claude Opus 4.1 brought richer context-aware assistance and deepened enterprise and user controls. Enhanced VS Code workflows, improved pull request automation, and advances in customization and security continue to position Copilot as a standard for modern coding, while ongoing debates focus on transparency, cost, and workflow integration.
<!--excerpt_end-->
## Powerful AI Model Integrations Reshape Copilot
The public preview of GPT-5 and Claude Opus 4.1 into Copilot marks a significant upgrade, providing more nuanced code reasoning and advanced summarization. Paid Copilot tiers now access GPT-5—across github.com, VS Code, and GitHub Mobile—with administrators controlling model rollout for compliance. Anthropic’s Opus 4.1 boosts logic and summarization, and configurable organizational controls help teams adapt incrementally. Community feedback spotlights GPT-5’s value for analytic reviews and complex onboarding, but notes verbosity and inconsistent rollout, echoing ongoing discussions around quotas, transparency, and real-world deployment.
- [AMA: GPT-5 Launch and GitHub Copilot – Community Questions Answered](https://www.reddit.com/r/GithubCopilot/comments/1mk5xc3/gpt5_is_here_ama_on_thursday_august_13th/)
- [OpenAI GPT-5 Public Preview Launches for GitHub Copilot Users](https://github.blog/changelog/2025-08-07-openai-gpt-5-is-now-in-public-preview-for-github-copilot)
- [Anthropic Claude Opus 4.1 Now Publicly Previewed in GitHub Copilot](https://github.blog/changelog/2025-08-05-anthropic-claude-opus-4-1-is-now-in-public-preview-in-github-copilot)
- [Community Experiences with GPT-5 in GitHub Copilot and Coding Workflows](https://www.reddit.com/r/GithubCopilot/comments/1mkse98/how_is_gpt5_experience_for_everyone/)
- [Comparing GPT-5 and Opus 4.1 Model Capabilities and Economics in GitHub Copilot](https://www.reddit.com/r/GithubCopilot/comments/1mk8f03/gpt5_only_matches_opus_41/)
## Coding Agent Capabilities and Automated Workflow Improvements
New Copilot Coding Agent features automate drafting repo-specific instructions for tasks like building and testing, reducing manual efforts. Pull request workflows now require explicit @copilot mentions by write-access collaborators, clarifying authority and minimizing accidental changes. General availability of copilot-instructions.md supports encoding project standards in natural language for best practice enforcement. VS Code users benefit from chat checkpoints, improved tool selection, model customization, and safer command line automation, streamlining agent-assisted coding and integrating deeply into daily workflows.
- [Copilot Coding Agent: Automatically Generate Custom Instructions](https://github.blog/changelog/2025-08-06-copilot-coding-agent-automatically-generate-custom-instructions)
- [Copilot Coding Agent: Enhanced Pull Request Review Workflow](https://github.blog/changelog/2025-08-05-copilot-coding-agent-improved-pull-request-review-experience)
- [GitHub Copilot in VS Code July 2025 Release (v1.103)](https://github.blog/changelog/2025-08-07-github-copilot-in-vs-code-july-release-v1-103)
## Practical Guides for Code Review, Automation, and Daily Workflows
New guides illustrate advanced Copilot prompts for code review, PR summaries, typo detection, and onboarding—reinforcing Copilot’s daily value and learning utility, especially for students and early-career developers. Resources detail project automation using GitHub Models and Actions, AI-assisted bug triage, and changelog generation, extending agent workflows. Educational advice balances Copilot’s benefits with fundamentals, offering options for educators on tool enablement and responsible use.
- [How to Use GitHub Copilot to Level Up Your Code Reviews and Pull Requests](https://github.blog/ai-and-ml/github-copilot/how-to-use-github-copilot-to-level-up-your-code-reviews-and-pull-requests/)
- [Top 10 Ways New Developers Can Benefit from GitHub Copilot](https://dellenny.com/top-10-things-you-can-do-with-github-copilot-as-a-new-developer/)
- [Automate Your Project with GitHub Models in Actions: AI Integration for Workflows](https://github.blog/ai-and-ml/generative-ai/automate-your-project-with-github-models-in-actions/)
## Enterprise, Security, and Admin-Focused Enhancements
Copilot Studio’s July update introduced NLU+, Microsoft OneLake integration, workspace search, and enhanced governance. Large orgs now enjoy asynchronous report generation, sector-focused Copilot Pak365, and stronger integration and cost controls. Discussions cover deployment in both small consultancies and enterprise environments, focusing on scaling secure, compliant AI.
- [What’s New in Copilot Studio: July 2025 Feature Roundup](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/whats-new-in-copilot-studio-july-2025/)
- [Copilot Pak365™: Empowering Frontier Companies with Secure AI for Microsoft 365](https://techcommunity.microsoft.com/t5/partner-news/copilot-pak365-empowering-frontier-companies-with-secure-ai-for/ba-p/4439925)
## Challenges and Community Reflections: Quota, Credit Use, Context, and Model Choice
Ongoing debates focus on quotas—uncertainty around premium requests, chat vs. code credit usage, and rapid exhaustion persist. Context limits (capped at 128k tokens) remain a pain point for large codebases, encouraging hybrid analysis approaches. Model choice is nuanced: teams compare GPT-5, Opus 4.1, Gemini 2.5 Pro, and user experiences with verbose or inaccurate outputs, reinforcing best practices for workflow-specific model selection and human oversight.
- [Understanding GitHub Copilot Usage Quotas and Agent Mode Requests](https://www.reddit.com/r/GithubCopilot/comments/1mk63f2/understanding_usage_quotas_what_about_copilot/)
- [Discussion: Capped Context Length in GitHub Copilot Models](https://www.reddit.com/r/github/comments/1mkne30/capped_context_length_issues_in_copilot_anyone/)
- [Comparing Copilot AI Models for C# Bug Fixing: GPT-5, Gemini 2.5 Pro, and Others](https://www.reddit.com/r/GithubCopilot/comments/1mkqvpn/vibe_debugging_gpt5_is_worse_than_o3gemini25_pro/)
## Copilot’s Role in Modern Development: Survey Insights and Forward-Looking Discussions
Now recognized as developers’ top AI tool, Copilot’s deep IDE integration propels both productivity and a new culture of code review and learning. Yet, developers want more alignment with personal coding styles and stable platform integration, as discussed in live demos and team case studies. Ongoing product deprecations, platform-specific bugs, and consolidation of models (like GPT-4o’s retirement) highlight the rapid cycle of Copilot’s innovation and stabilization.
- [GitHub Copilot Surpasses ChatGPT as Top Developer AI Tool](/ai/videos/github-copilot-surpasses-chatgpt-as-top-developer-ai-tool)
- [Deprecation of GPT-4o in Copilot Chat](https://github.blog/changelog/2025-08-06-deprecation-of-gpt-4o-in-copilot-chat)
In sum, Copilot continues to evolve rapidly, maturing transparency, model choice, security, and workflow-native automation—driven by both technical advancements and persistent community feedback.',
    'Building on last week’s momentum in agent workflows and persistent memory, GitHub Copilot rolled out major updates, broadening its lead as an AI-powered developer tool. This week, public preview integrations for OpenAI GPT-5 and Anthropic Claude Opus 4.1 brought richer context-aware assistance and deepened enterprise and user controls. Enhanced VS Code workflows, improved pull request automation, and advances in customization and security continue to position Copilot as a standard for modern coding, while ongoing debates focus on transparency, cost, and workflow integration.',
    1754895600, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-08-11', 'TechHub',
    'TechHub', 'A5C7A359F6D5DC724C2427AAAB350C18FC6E606ED87A1BC1C507D0BE48432F9C', ',GitHub Copilot,GPT 5,Claude Opus 4.1,Copilot Chat,VS Code,Copilot Coding Agent,Pull Requests,Code Review,Copilot Instructions.md,GitHub Actions,GitHub Models,Enterprise Governance,Admin Controls,Security Compliance,Usage Quotas,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-08-04
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-08-04', 'roundups', 'Weekly GitHub Copilot Roundup: Memory, Agents, and Repo Control',
    'This week, GitHub Copilot further entrenched itself as a vital tool in the developer workflow through increased adoption, feature enhancements, and active community input. Its advancements—persistent memory, enhanced in-chat repo management, far-reaching automation, and evolving prompt and workflow strategies—are not just technical gains; they’re directly shaping how teams manage codebases, onboard, and address cost and privacy concerns. This narrative of steady progress is bolstered by enterprise adoption, practical team case studies, and ongoing, candid discussions about the challenges of model reliability and transparency.
<!--excerpt_end-->
## Personalized AI: Copilot''s Memory and Context Awareness
Copilot’s new ''memory'' feature marks a notable leap: it now persists details like your coding style, naming conventions, and framework/library preferences across sessions—making its suggestions more accurate and contextually aware. The memory is private, user-controllable, and can be reviewed, edited, or reset at any time. This offers meaningful efficiency for those juggling multiple projects or teams, and is paired with clear privacy controls and transparent notifications.
- [How GitHub Copilot''s New Memory Feature Changes Your Coding Experience](https://dellenny.com/copilot-now-remembers-you-heres-why-that-matters/)
## Copilot Overtakes ChatGPT: Leading the AI Developer Toolchain
GitHub Copilot’s overtaking of ChatGPT as developers’ top AI tool reflects the shift toward deep, workflow-native AI integration. Copilot now powers seamless code suggestions, automated refactoring, and richer IDE automation. This growth is further visible through initiatives like the ‘For the love of code’ hackathon and GitHub’s new developer-focused podcast—signaling the momentum of a fast-growing Copilot ecosystem.
- [The Download: GitHub Copilot Overtakes ChatGPT as Top Developer AI Tool](/ai/videos/the-download-github-copilot-overtakes-chatgpt-as-top-developer-ai-tool)
## Enhanced AI-Powered Workflows in Visual Studio Code
Building on last week’s GitHub Spark debut, its integration with Copilot in VS Code advanced: developers now saw seamless, in-session natural language-to-app generation, accelerated code automation, and improved extension/workflow management. Community feedback continued to refine these features, cementing Copilot and Spark as drivers for rapid prototyping and modernization.
- [VS Code Live – Exploring GitHub Spark and Copilot in Visual Studio Code](/ai/videos/vs-code-live-exploring-github-spark-and-copilot-in-visual-studio-code)
## Repository Management Directly Through Copilot Chat
Copilot Chat now enables core repo management—file creation, updates, pushes, branch actions, and PR merges—by simple conversational prompts. This tightens the workflow loop for developers and teams, minimizing context-switching and integrating code and project management into a single interface.
- [Copilot Chat Unlocks Powerful Repository Management Skills in GitHub](https://github.blog/changelog/2025-07-31-copilot-chat-unlocks-new-repository-management-skills)
## The Rise of AI-Powered Agents in Software Development
Agent-based automation continued to evolve, with Copilot Coding Agent now automating code reviews, branch management, and PR detail sync. Enhanced setup reliability and “agent vs. agent mode” options provide flexible levels of task delegation and collaboration. The MCP server ecosystem and guides for YAML/instruction management demonstrate maturing best practices that smooth onboarding and boost adoption.
- [Onboarding Your AI Peer Programmer: Success with GitHub Copilot Coding Agent](https://github.blog/ai-and-ml/github-copilot/onboarding-your-ai-peer-programmer-setting-up-github-copilot-coding-agent-for-success/)
- [Copilot Coding Agent: Enhanced Reliability and Debugging for Custom Setup Steps](https://github.blog/changelog/2025-07-30-copilot-coding-agent-custom-setup-steps-are-more-reliable-and-easier-to-debug)
- [GitHub Copilot Coding Agent: Automatic Updates for Pull Request Titles and Descriptions](https://github.blog/changelog/2025-07-30-copilot-coding-agent-keeps-pull-request-titles-and-bodies-up-to-date)
- [A Practical Guide to Using the GitHub MCP Server for Automated AI Workflows](https://github.blog/ai-and-ml/generative-ai/a-practical-guide-on-how-to-use-the-github-mcp-server/)
- [When to Use GitHub Copilot Coding Agent Versus Agent Mode](/ai/videos/when-to-use-github-copilot-coding-agent-versus-agent-mode)
## Workflow Strategies: Prompt Engineering and Customization
Community strategies continued to optimize prompt engineering, including ‘Extensive Mode’ for cost control, JSON-based prompts, and context engineering to steer LLMs. Discussion covered distributed `.instructions.md` files, chain-of-thought prompting, and methods for structured project context—all aimed at reproducible, consistent AI guidance and cost efficiency.
- [Save on GitHub Copilot Premium Requests with Extensive Mode Inspired by Beast Mode](https://www.reddit.com/r/GithubCopilot/comments/1mfja7z/want_to_save_on_your_premium_request_well/)
- [How I Levelled Up My GitHub Copilot Prompts with Instruction Files and Context Engineering](https://www.reddit.com/r/GithubCopilot/comments/1mbebfh/how_i_levelled_up_my_github_copilot_prompts_with/)
- [Anyone using JSON Prompting with LLMs?](https://www.reddit.com/r/GithubCopilot/comments/1mb7lpn/anyone_using_json_prompting_with_llms/)
- [Forcing Chain-of-Thought to Non-Thinking Models in AI IDEs like VS Code & Copilot](https://www.reddit.com/r/GithubCopilot/comments/1mcbkb8/forcing_cot_to_nonthinking_models_within_an_ai/)
- [Never lose your GitHub Copilot session history again](/ai/videos/never-lose-your-github-copilot-session-history-again)
## Real-World Impact: Productivity Gains in Teams and Nonprofits
Real-world adoption stories, such as at One Acre Fund, showed Copilot can triple software delivery speed, echoing earlier themes around rapid MVPs and modernization. Best practices—agent onboarding, prompt-driven docs, using Copilot for both infra/app layers—are being widely adopted from startups to nonprofits.
- [Scaling for Impact: How GitHub Copilot Accelerates One Acre Fund’s Mission for African Farmers](https://github.blog/open-source/social-impact/scaling-for-impact-how-github-copilot-supercharges-smallholder-farmers/)
- [GitHub Copilot Helps One Acre Fund Scale Farming Impact](/ai/videos/github-copilot-helps-one-acre-fund-scale-farming-impact)
- [Complete functional MVP using Copilot.](https://www.reddit.com/r/GithubCopilot/comments/1mfw2yf/complete_functional_mvp_using_copilot/)
## Productivity Modes, Extensions, and Collaboration in VS Code
Copilot’s three core modes—Agent, Edit, and Ask—now fully span the software lifecycle. SQL developers benefit from agent task delegation, local-containerized DBs, and AI-powered code review, with custom chat modes and competitions expanding AI use beyond just code generation.
- [Are You Using All 3 GitHub Copilot Modes?](/ai/videos/are-you-using-all-3-github-copilot-modes)
- [Boost Your SQL Development in VS Code: GitHub Copilot, Containers, and More](/ai/videos/boost-your-sql-development-in-vs-code-github-copilot-containers-and-more)
- [Creating a Custom Chat Mode in VS Code for Smarter AI Assistance](/ai/videos/creating-a-custom-chat-mode-in-vs-code-for-smarter-ai-assistance)
- [1st GitHub Copilot Custom Chat Competition](https://www.reddit.com/r/GithubCopilot/comments/1mfjlie/1st_github_copilot_custom_chat_competition/)
## Enhanced Debugging and Code Review
Ongoing improvements now allow Copilot Chat to leverage more contextual input for debugging, while Copilot Coding Agent automates PR title/description sync. These changes further last week’s push toward actionable, automated reviews and richer documentation for teams.
- [Debugging Faster with GitHub Copilot Chat: Tips from GitHub](/ai/videos/debugging-faster-with-github-copilot-chat-tips-from-github)
- [GitHub Copilot Coding Agent: Automatic Updates for Pull Request Titles and Descriptions](https://github.blog/changelog/2025-07-30-copilot-coding-agent-keeps-pull-request-titles-and-bodies-up-to-date)
- [Write Cleaner Code Comments with GitHub Copilot](https://cooknwithcopilot.com/blog/write-cleaner-code-comments-with-github-copilot.html)
## Real-World Guidance and Best Practices for New Users
Onboarding guides have expanded—offering step-by-step help for VS Code, Docker, privacy management, and troubleshooting. The sustained growth of structured documentation reflects a user-driven drive to reduce friction and boost Copilot reliability.
- [A Comprehensive Guide to Getting Started with GitHub Copilot](https://dellenny.com/a-comprehensive-guide-to-getting-started-with-github-copilot-for-end-users/)
- [How to Use GitHub Copilot: The Complete Beginner''s Guide](/ai/videos/how-to-use-github-copilot-the-complete-beginners-guide)
- [Seeking Guidance: Effectively Using GitHub Copilot with VS Code and Docker](https://www.reddit.com/r/GithubCopilot/comments/1mg6uu8/am_i_using_it_wrong/)
- [Vibe Code Your First MCP Server with GitHub Copilot](/ai/videos/vibe-code-your-first-mcp-server-with-github-copilot)
## AI Support for Agile Teams and Technical Writing
The Scrum Assistant automates daily Agile rituals and sprint planning, while Copilot’s prompt-based document generation aids in drafting RFPs and technical content—consistently saving time and ensuring clarity with human review.
- [How to Activate and Use the Scrum Assistant Agent in GitHub Copilot](https://dellenny.com/how-to-activate-and-use-the-scrum-assistant-agent-in-github-copilot/)
- [How to Use GitHub Copilot to Write RFP Responses Faster](https://dellenny.com/win-more-bids-how-to-use-github-copilot-to-write-winning-rfp-responses-faster/)
## Enterprise, Billing, and API Enhancements
Enterprises benefit from Copilot’s new billing models (per-user premium quotas, overage management) and improved User Management API durability—facilitating compliance requirements and more reliable team activity tracking.
- [Update: GitHub Copilot Consumptive Billing for Enterprise Cloud with Data Residency](https://github.blog/changelog/2025-08-01-update-on-github-copilot-consumptive-billing-for-github-enterprise-cloud-with-data-residency)
- [Enhancements to last_activity_at in the Copilot User Management API](https://github.blog/changelog/2025-07-29-enhancements-to-last_activity_at-in-the-user-management-api)
## Transparency, Reliability, and Areas for Caution
Developers continued to debate Copilot’s reliability and transparency, discussing rate limits, quota resets, the accuracy of session memory, and AI hallucinations. Practical recommendations included regular manual review and monitoring privacy boundaries as feature sets grow.
- [Copilot is Lying About Seeing My Code](https://www.reddit.com/r/GithubCopilot/comments/1mc7cof/copilot_is_lying_about_seeing_my_code/)
- [Being rate limited on VSCode on a single GitHub Copilot chat thread](https://www.reddit.com/r/GithubCopilot/comments/1mgh2oo/being_rate_limited_on_vscode_on_a_single_chat/)
- [Gemini Pro Fails More Often Than Not](https://www.reddit.com/r/GithubCopilot/comments/1mbfeiw/gemini_pro_fails_more_often_than_not/)
- [Wait… Premium requests reset on the 1st of every month??!](https://www.reddit.com/r/GithubCopilot/comments/1mfg0ta/wait_premium_requests_reset_on_the_1st_of_every/)
- [A new problem - I didn''t use all my GitHub Copilot premium requests last month](https://www.reddit.com/r/GithubCopilot/comments/1mev008/a_new_problem_i_didnt_use_all_my_github_copilot/)
- [Agent Can''t Memorize the Full Session?](https://www.reddit.com/r/GithubCopilot/comments/1mfu87w/agent_cant_memorize_the_full_session/)
- [Cleaning Up Projects with GitHub Copilot: Seeking Reliable Code Cleanup Methods](https://www.reddit.com/r/GithubCopilot/comments/1mgx5uj/cleaning_up_a_project/)
- [Oopsie doopsie copilot made a little hallucination 🤣](https://www.reddit.com/r/GithubCopilot/comments/1mg5oty/oopsie_doopsie_copilot_made_a_little_hallucination/)
## Community, Competitions, and Lighter Moments
Developer camaraderie and fun were evident through changelog discussions, competitions, and light-hearted takes on day-to-day Copilot quirks. These ongoing community interactions remain central to Copilot’s rapid evolution and user-driven vibe.
- [GitHub Copilot Changelog Thread](https://www.reddit.com/r/GithubCopilot/comments/1mezre9/github_copilot_changelog_thread/)
- [LMAO Did I Do Something Wrong?](https://www.reddit.com/r/GithubCopilot/comments/1mbdd6h/lmao_did_i_do_something_wrong/)
- [Made me chuckle - trying to stop artifact files being added](https://www.reddit.com/r/GithubCopilot/comments/1mgipey/made_me_chuckle_trying_to_stop_artifact_files/)',
    'This week, GitHub Copilot further entrenched itself as a vital tool in the developer workflow through increased adoption, feature enhancements, and active community input. Its advancements—persistent memory, enhanced in-chat repo management, far-reaching automation, and evolving prompt and workflow strategies—are not just technical gains; they’re directly shaping how teams manage codebases, onboard, and address cost and privacy concerns. This narrative of steady progress is bolstered by enterprise adoption, practical team case studies, and ongoing, candid discussions about the challenges of model reliability and transparency.',
    1754290800, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-08-04', 'TechHub',
    'TechHub', '43985FEFF4C321CC7C874423D0FD70A2E0732447C51EAB5156C9779E16A5B4C9', ',GitHub Copilot,Copilot Chat,Copilot Coding Agent,VS Code,GitHub Spark,AI Agents,Repository Management,Pull Requests,Prompt Engineering,Instruction Files,MCP Server,Enterprise Billing,User Management API,Data Residency,Privacy Controls,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-07-28
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-07-28', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, Instructions, and Spark',
    'This week marked major leaps in Copilot’s automation, customization, and workflow integration—positioning it as a vital productivity platform. Developers gained smarter Git automation, context-driven code review, and new tools for rapid prototyping and modernization, while staying alert to upcoming configuration changes.
<!--excerpt_end-->
## Coding Agent Capabilities and Workflow Automation
Copilot’s coding agent advanced with enhanced Git workflow automation and tighter Visual Studio Code integration. The new PowerShell workflow prompt introduces AI-generated commit messages, advanced branch protections, and seamless server automation—streamlining daily repository management with hands-free, multi-step execution and robust error handling. Upcoming Visual Studio Code v1.103 will require a shift from legacy allow/deny-lists to a unified `autoApprove` configuration, prompting early migration to maintain workflow continuity.
Copilot agents now support custom base branch selection in automated PRs, accommodating complex CI/CD and branching requirements. Expanded session tracking, Playwright MCP server integration, and improved remote support boost scalability and transparency for large teams.
- [Enhanced Git Workflow Prompt and Upcoming VS Code Terminal Auto-Approval Changes](https://r-vm.com/improved-git-workflow-custom-prompt-upcoming-vscode-change-warning.html)
- [Agents Page Update: Choose Base Branch for GitHub Copilot Coding Agent Tasks](https://github.blog/changelog/2025-07-23-agents-page-set-the-base-branch-for-github-copilot-coding-agent-tasks)
- [What''s new with the GitHub Copilot coding agent: A look at the updates](/ai/videos/whats-new-with-the-github-copilot-coding-agent-a-look-at-the-updates)
## Custom Instructions and Contextual Guidance
Copilot now features distributed `.instructions.md` support, letting teams issue granular, context-sensitive directives at any folder or file level. This upgrades the prior monolithic instructions approach, enabling nuanced control for builds, lints, and tests—resulting in higher quality automated PRs and less manual intervention. Accessible in public preview across interfaces, this boosts applicability in varied workflows.
- [GitHub Copilot Coding Agent Now Supports .instructions.md Custom Instructions](https://github.blog/changelog/2025-07-23-github-copilot-coding-agent-now-supports-instructions-md-custom-instructions)
- [Debugging UI with AI: GitHub Copilot Agent Mode Meets MCP Servers](https://github.blog/ai-and-ml/github-copilot/debugging-ui-with-ai-github-copilot-agent-mode-meets-mcp-servers/)
## Rapid Prototyping and App Modernization
The debut of GitHub Spark (public preview) allows Copilot Pro+ users to convert natural language ideas into fully deployed full-stack apps—eliminating much of the planning and configuration overhead with AI-driven code and infra generation. Early adopters highlight its versatility for experimentation and MVP workflows.
Simultaneously, Copilot’s .NET app modernization tool, also in public preview, leverages AI to automate assessment and remediation for moving legacy .NET apps to Azure—minimizing manual labor and risk in cloud migrations. These innovations further Copilot’s role in enabling safe, rapid modernization for enterprise workloads.
- [GitHub Spark in Public Preview for Copilot Pro+ Subscribers](https://github.blog/changelog/2025-07-23-github-spark-in-public-preview-for-copilot-pro-subscribers)
- [Today we''re releasing GitHub Spark — a new tool in Copilot that turns your ideas into full-stack apps, entirely in natural language](https://www.linkedin.com/posts/satyanadella_today-were-releasing-github-spark-a-new-activity-7353868825320214529-o3C5)
- [GitHub Copilot app modernization for .NET enters public preview](https://github.blog/changelog/2025-07-21-github-copilot-app-modernization-for-net-enters-public-preview)
## AI-Driven Test Automation and Review
Test automation received a boost through Copilot’s integration with Azure DevOps MCP and Playwright, turning manual QA cases into AI-generated automation scripts—accelerating regression coverage and CI/CD reliability. The quality of prompt engineering and test assets is crucial for effective automation.
Copilot’s expanded code review capabilities deliver pre-commit feedback in VS Code and often automate actionable suggestions on GitHub.com, ensuring fast, quality-centric review cycles. This deep integration shortens feedback loops for teams without increasing manual review demands.
- [From Manual Testing to AI-Generated Automation: Azure DevOps MCP & Playwright with GitHub Copilot](https://devblogs.microsoft.com/devops/from-manual-testing-to-ai-generated-automation-our-azure-devops-mcp-playwright-success-story/)
- [Using GitHub Copilot for Code Reviews - From VS Code to GitHub.com](/ai/videos/using-github-copilot-for-code-reviews-from-vs-code-to-githubcom)
## Agent Mode, Ask Mode, and Integration Patterns
Developers can now toggle between Ask Mode (Q&A) and Agent Mode (automation, code edits) to maximize productivity for differing .NET tasks. Ongoing hands-on sessions show how Agent Mode streamlines building, debugging, and collaboration in both competitions and business settings.
Copilot''s coding agent continues to unify assignment, pull request, and hands-free code delegation within VS Code, pushing automation boundaries for everyday development.
- [Ask Mode vs Agent Mode: Choosing the Right GitHub Copilot Experience for .NET Developers](https://devblogs.microsoft.com/dotnet/ask-mode-vs-agent-mode/)
- [Coding Agent Integration in Visual Studio Code](/ai/videos/coding-agent-integration-in-visual-studio-code)
- [Rubber Duck Thursday - Building an App with GitHub Copilot Agent Mode for the Competition](/ai/videos/rubber-duck-thursday-building-an-app-with-github-copilot-agent-mode-for-the-competition)
## Productivity Extensions and Platform-Wide Features
Copilot Spaces now ground AI suggestions in company standards, automating compliant code review at scale. Legacy modernization is further streamlined through COBOL-to-cloud migration demonstrations, accelerating documentation and reducing manual intervention.
Workarounds for Copilot quota limits, prompt engineering best practices, improved model selection in the GitHub Mobile app, and stronger Eclipse support all demonstrate Copilot’s ongoing platform and device integration—making Copilot available to more workflows and environments each week.
- [Turn Copilot into a subject matter expert with GitHub Copilot Spaces](/ai/videos/turn-copilot-into-a-subject-matter-expert-with-github-copilot-spaces)
- [Modernizing Legacy: COBOL to Cloud with GitHub Copilot](/ai/videos/modernizing-legacy-cobol-to-cloud-with-github-copilot)
- [Limited to 300 Free Premium Requests by Your Org? Here’s an Expensive GitHub Copilot Workaround](https://r-vm.com/limited-to-300-free-premium-requests-by-your-org-heres-an-expensive-workaround.html)
- [Vibe Coding PromptBoost.dev with GitHub Copilot in VS Code](/ai/videos/vibe-coding-promptboostdev-with-github-copilot-in-vs-code)
- [GitHub Copilot in Eclipse—smarter, faster, and more integrated](https://github.blog/changelog/2025-07-22-github-copilot-in-eclipse-smarter-faster-and-more-integrated)
- [Enhanced Model Selection in Copilot Chat on GitHub Mobile](https://github.blog/changelog/2025-07-25-enhanced-model-selection-experience-in-copilot-chat-on-github-mobile)
- [Missing ''Enable Copilot'' Checkbox in Word and PowerPoint Despite M365 Copilot License](https://www.reddit.com/r/microsoft/comments/1m9a3lz/do_you_have_the_enable_copilot_checkbox_in_your/)',
    'This week marked major leaps in Copilot’s automation, customization, and workflow integration—positioning it as a vital productivity platform. Developers gained smarter Git automation, context-driven code review, and new tools for rapid prototyping and modernization, while staying alert to upcoming configuration changes.',
    1753686000, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-07-28', 'TechHub',
    'TechHub', '239738BC6EC7CA2C0571B8BF1A04EF9E8C22EC2BF07E4FEA623B9579779BAF7B', ',GitHub Copilot,Copilot Coding Agent,Agent Mode,Ask Mode,VS Code,VS Code Settings,Git Automation,Pull Requests,Code Review,.instructions.md,GitHub Spark,.NET Modernization,Azure,Playwright,MCP,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-07-21
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-07-21', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, MCP, and AI Code Review',
    'This week, GitHub Copilot received significant enhancements, reinforcing its drive toward streamlined automation, extensibility, and practical productivity for both enterprise and individual developers. Updates advanced code review automation, cross-IDE agent support, robust contextual intelligence with MCP, reporting capabilities, and operational transparency.
<!--excerpt_end-->
## AI-Assisted Code Review and Workflow Automation
Copilot’s AI-powered code review, after internal and early previews, is now automating over 600,000 PRs monthly at Microsoft. It surfaces issues, summarizes PRs, and supports interactive queries, speeding up reviews by up to 20% and supporting compliance through customizable instructions. Guidance is consolidated in `copilot-instructions.md`, with new UI enhancements, keeping developer ownership over merges. These features build on Copilot code review’s rollout in GitHub Mobile and support for structured, scalable human-AI collaboration in code reviews.
- [Enhancing Code Quality at Scale with AI-Powered Code Reviews](https://devblogs.microsoft.com/engineering-at-microsoft/enhancing-code-quality-at-scale-with-ai-powered-code-reviews/)
- [Upcoming deprecations and changes to Copilot code review](https://github.blog/changelog/2025-07-18-upcoming-deprecations-and-changes-to-copilot-code-review)
- [Code review in the age of AI - Why developers will always own the merge button](https://github.blog/ai-and-ml/generative-ai/code-review-in-the-age-of-ai-why-developers-will-always-own-the-merge-button/)
## Coding Agent Capabilities Expand Across IDEs
Copilot coding agents are now generally available not only in VS Code but also JetBrains, Eclipse, and Xcode. Developers initiate and monitor autonomous code tasks within any major IDE, reducing context switching and bottlenecks. Enhanced security defaults restrict agent internet access, with detailed configuration options meeting enterprise policies. Broad agent availability and secure, cross-platform automation signal GitHub’s commitment to robust, organization-ready deployment.
- [Start and track GitHub Copilot coding agent sessions from Visual Studio Code](https://github.blog/changelog/2025-07-14-start-and-track-github-copilot-coding-agent-sessions-from-visual-studio-code)
- [Agent mode for JetBrains, Eclipse, and Xcode is now generally available](https://github.blog/changelog/2025-07-16-agent-mode-for-jetbrains-eclipse-and-xcode-is-now-generally-available)
- [Configure internet access for Copilot coding agent](https://github.blog/changelog/2025-07-15-configure-internet-access-for-copilot-coding-agent)
- [From chaos to clarity - Using GitHub Copilot agents to improve developer workflows](https://github.blog/ai-and-ml/github-copilot/from-chaos-to-clarity-using-github-copilot-agents-to-improve-developer-workflows/)
- [Finish your work without touching any code](/ai/videos/finish-your-work-without-touching-any-code)
## Model Context Protocol (MCP) and Extensibility
Copilot’s Model Context Protocol (MCP) is now fully live in VS Code, enabling real-time integration with external tools and data sources. This brings contextually aware AI support, automating tasks using live project data, APIs, and databases, and supporting hands-on workflow automation. VS Code’s v1.102 release leverages MCP for cross-environment collaboration, with tutorials guiding effective team adoption.
- [Model Context Protocol (MCP) support in VS Code is generally available](https://github.blog/changelog/2025-07-14-model-context-protocol-mcp-support-in-vs-code-is-generally-available)
- [VS Code Live - Exploring v1.102 Release Features—AI Chat, MCP, Coding Agent & More](/ai/videos/vs-code-live-exploring-v1102-release-featuresai-chat-mcp-coding-agent-more)
- [Model selection](/github-copilot-features-model-selection)
- [Automating Developer Tasks with GitHub Copilot Agent Mode and MCP Servers in VS Code](/ai/videos/automating-developer-tasks-with-github-copilot-agent-mode-and-mcp-servers-in-vs-code)
## Productivity and Reporting Enhancements
Copilot adds support for interactive issue forms on github.com and enhances administrative oversight with improved activity reports, offering granular insights into usage and authentication for license management and targeted support. Copilot Chat supports early software planning, helping teams clarify requirements and reduce rework.
- [Support for issue forms in chat and file uploads in spaces](https://github.blog/changelog/2025-07-16-support-for-issue-forms-in-chat-and-file-uploads-in-spaces)
- [New GitHub Copilot activity report with enhanced authentication and usage insights](https://github.blog/changelog/2025-07-18-new-github-copilot-activity-report-with-enhanced-authentication-and-usage-insights)
- [Use GitHub Copilot Chat to Plan Your Software Before Coding](https://cooknwithcopilot.com/blog/use-github-copilot-chat-to-plan-before-you-code.html)
## Availability, Incident Management, and Community Engagement
The June GitHub Availability Report reviews resilience efforts, transparency in incident management, and plans for improved monitoring. The "For the Love of Code" summer hackathon encourages creative use of Copilot and open source, sustaining community engagement and informing future product directions.
- [GitHub Availability Report: June 2025](https://github.blog/news-insights/company-news/github-availability-report-june-2025/)
- [For the Love of Code - A Summer Hackathon for Joyful and Creative Projects](https://github.blog/open-source/for-the-love-of-code-2025/)',
    'This week, GitHub Copilot received significant enhancements, reinforcing its drive toward streamlined automation, extensibility, and practical productivity for both enterprise and individual developers. Updates advanced code review automation, cross-IDE agent support, robust contextual intelligence with MCP, reporting capabilities, and operational transparency.',
    1753081200, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-07-21', 'TechHub',
    'TechHub', 'E460237C41DC2A7F05611F7E6285CBA5594B38F0CB56521D5EC36AA37946E091', ',GitHub Copilot,Copilot Code Review,Pull Requests,Agent Mode,Coding Agents,VS Code,JetBrains,Eclipse IDE,Xcode,MCP,Copilot Chat,Issue Forms,Activity Reports,Enterprise Security,GitHub Availability,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-07-14
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-07-14', 'roundups', 'Weekly GitHub Copilot Roundup: Agents, MCP, and CI Pipelines',
    'This week’s Copilot news revolves around upgrades that sharpen its integration into developer workflows, enhance its platform capabilities, and spark reflection on AI’s role in coding productivity. Developers and teams report gains and friction as Copilot evolves from suggestion engine to agent platform, fueling rich discussion on where real value lies and what best practices must emerge.
<!--excerpt_end-->
## Real-World Integration, CI/CD, and Security Practices
The .NET MAUI team provided a glimpse into real, enterprise-scale Copilot adoption. By embedding Copilot Coding Agent directly into CI pipelines and using a dedicated `copilot-instructions.md`, they boost productivity on repeatable tasks while managing security via domain restrictions and role segregation. Integrations with MCP servers and explicit team guidelines mitigate risk. Their experience echoes a maturing playbook: scope Copilot tightly, enforce contextual instructions, and drive usage through structured workflow integration. Gaps remain—especially for advanced PR handling—but adoption is expanding as feature breadth grows.
This builds on last week’s focus on agentic workflows and growing industry consensus around codified prompt management. Patterns like persistent chat mode configs and domain-specific instructions are now proven in production.
- [How the .NET MAUI Team Uses GitHub Copilot for Productivity](https://devblogs.microsoft.com/dotnet/maui-team-copilot-tips/)
## Copilot Features and Platform Availability Expand
Copilot Chat has reached general availability, now offering code change previews, streamlined Issue integration, model selection, and file attachments natively inside GitHub. Its VS Code extension is open source (v1.102), allowing for deep customization; developers can define instruction sets, automate terminal approvals, and add multi-agent support through Model Context Protocol. Features like terminal auto-approval create nearly hands-free Git workflows, with controls for safety and repeatability.
Open-sourcing the Chat extension and fully supporting MCP signal Copilot’s move toward an extensible automation platform—mirroring the expected progression from last week’s cross-IDE and agentic groundwork to feature-complete GA releases.
- [New Copilot Chat features now generally available on GitHub](https://github.blog/changelog/2025-07-09-new-copilot-chat-features-now-generally-available-on-github)
- [Automating My Git Workflow in VS Code with Copilot Chat, Custom Prompts, and Terminal Auto-Approval](https://r-vm.com/automating-my-git-workflow-vscode-copilot-chat-terminal-auto-approval.html)
- [VS Code June 2025 (version 1.102)](https://www.reddit.com/r/GithubCopilot/comments/1lwk6ba/vs_code_june_2025_version_1102/)
- [MCP Support Now Generally Available in Visual Studio Code](/ai/videos/mcp-support-now-generally-available-in-visual-studio-code)
## Mobile Review, Agentic Tasks, and Cost Predictability
Copilot code review lands on GitHub Mobile, bringing AI-assisted PR feedback to any device and supporting asynchronous, sustained code review. Copilot agents now support remote MCP servers, broadening multi-team and distributed codebase use. Background automation—such as task delegation from MCP servers—allows Copilot to handle refactoring or code generation at scale, augmenting team output. Importantly, Copilot’s updated billing is now session-based, giving teams better budget predictability and enabling more confident workflow integration.
These moves, following last week’s browser expansion and agentic emphasis, further Copilot’s goal: seamless productivity and automation across environments, architectures, and devices.
- [Copilot code review now generally available on GitHub Mobile](https://github.blog/changelog/2025-07-08-copilot-code-review-now-generally-available-on-github-mobile)
- [Copilot coding agent now supports remote MCP servers](https://github.blog/changelog/2025-07-09-copilot-coding-agent-now-supports-remote-mcp-servers)
- [Delegate tasks to Copilot coding agent from the GitHub MCP server](https://github.blog/changelog/2025-07-09-delegate-tasks-to-copilot-coding-agent-from-the-github-mcp-server)
- [GitHub Copilot coding agent now uses one premium request per session](https://github.blog/changelog/2025-07-10-github-copilot-coding-agent-now-uses-one-premium-request-per-session)
## Custom Instructions, Context, and Workflow Tips
Custom instructions, like copilot-instructions.md, are essential to aligning Copilot output with organizational standards. Teams embed best practices, versions, and conventions in project roots for more predictable and maintainable code review—even mapping out guidance as standards evolve (e.g., C# 13 exception handling). The community is converging on a toolset: codified context, custom prompts, and collaborative adjustment underpin stable Copilot-scale rollouts.
This picks up last week’s growing adoption of `.chatmode.md` and prompt methodology—resolutely proven as critical for robust, enterprise Copilot use.
- [Customize AI Responses from GitHub Copilot with Custom Instructions](https://devblogs.microsoft.com/dotnet/customize-ai-responses-from-github-copilot/)
- [Search any GitHub repo from agent](https://www.reddit.com/r/GithubCopilot/comments/1lvx08d/search_any_github_repo_from_agent/)
## Community Advice: Copilot Use, Planning, and Alternatives
Veteran devs offer blunt advice: Copilot, used well, supercharges repetitive work but cannot replace architectural judgment or disciplined planning. Vague prompts or unchecked outputs create tech debt; explicit briefs extract value. Comparisons between Copilot and Cursor highlight that reliability and billing transparency strongly influence user loyalty—even when alternatives might edge ahead in some features.
These evolving best practices echo last week’s feedback theme, reinforcing that Copilot works best as a disciplined co-pilot, not a surrogate engineer.
- [Hot take - Copilot is amazing! You''re probably just lazy.](https://www.reddit.com/r/GithubCopilot/comments/1lvt549/hot_take_copilot_is_amazing_youre_probably_just/)
- [Copilot vs. Chat: Sidekick Showdown – When to Use Each Coding Sidekick](https://cooknwithcopilot.com/blog/copilot-vs-chat-sidekick-showdown.html)
- [Why I changed Cursor to Copilot and it turned out to be the best choice](https://www.reddit.com/r/GithubCopilot/comments/1lwosq7/why_i_changed_cursor_to_copilot_and_it_turned_out/)
- [You''re probably using Copilot the wrong way](https://www.reddit.com/r/GithubCopilot/comments/1lwg11b/youre_probably_using_copilot_the_wrong_way/)
- [Beyond prompt crafting - How to be a better partner for your AI pair programmer](https://github.blog/ai-and-ml/github-copilot/beyond-prompt-crafting-how-to-be-a-better-partner-for-your-ai-pair-programmer/)
- [A follow-up to ''Goodbye Copilot!''...](https://www.reddit.com/r/GithubCopilot/comments/1lwecec/a_followup_to_goodbye_copilot/)
## Bugs, Usability Feedback, and Developer Culture
Developers are reporting UI regressions—like missing sidebar controls in VS Code 1.102—and recurring agent mode bugs (e.g., checkbox glitches), reflecting a broader reliance on fast user feedback and iterative patching. Community voices stress that strong feedback loops, adaptability, and a collaborative, critical mindset are vital as Copilot and AI agents become routine in daily dev and review workflows.
This culture-first emphasis mirrors last week’s attention on usability and collaborative adaptation, reinforcing that Copilot’s success hinges as much on team process as on technical advance.
- [Missing feature in VS Code version 1.102.0 - option to close copilot changed/modified files](https://www.reddit.com/r/GithubCopilot/comments/1lwvfgs/missing_feature_in_vs_code_version_11020_option/)
- [Checkbox Symbols Bug in VS Code Copilot Agent Mode](https://www.reddit.com/r/GithubCopilot/comments/1lvekgv/visual_studio_code_github_copilot_agent_mode/)
- [Your New Rubber Duck is an AI](https://roadtoalm.com/2025/07/11/your-new-rubber-duck-is-an-ai/)
- [GitHub Copilot coding agent now uses one premium request per session](https://www.reddit.com/r/GithubCopilot/comments/1lwlp9r/github_copilot_coding_agent_now_uses_one_premium/)',
    'This week’s Copilot news revolves around upgrades that sharpen its integration into developer workflows, enhance its platform capabilities, and spark reflection on AI’s role in coding productivity. Developers and teams report gains and friction as Copilot evolves from suggestion engine to agent platform, fueling rich discussion on where real value lies and what best practices must emerge.',
    1752476400, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-07-14', 'TechHub',
    'TechHub', '803DEF97A748E655C3F55DF53A09C8A76B16451E24379F33B82D80528AC60141', ',GitHub Copilot,Copilot Chat,Copilot Coding Agent,MCP,VS Code,GitHub Mobile,CI/CD,Code Review,Custom Instructions,Copilot Instructions.md,Terminal Auto Approval,Remote MCP Servers,Session Based Billing,MAUI,Security Controls,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-github-copilot-roundup-2025-07-07
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-github-copilot-roundup-2025-07-07', 'roundups', 'Weekly GitHub Copilot Roundup: Custom modes, agents, and reviews',
    'This week’s GitHub Copilot updates showcase growing maturity in AI tooling for developers, spotlighting productivity improvements through advanced workflows, customizable automation, and cross-IDE feature growth. VS Code, Xcode, and other environments now offer powerful configuration avenues so teams can tune Copilot to match specific project requirements, with a focus on scalable task automation, end-to-end workflow support, community sharing, and prompt optimization for larger codebases.
<!--excerpt_end-->
## Advanced Customizations and Team Alignment
New custom chat modes in Visual Studio Code, as detailed by Harald Binkle, empower developers to instantly toggle AI behaviors for reviewing code, writing tests, designing APIs, or documenting. Storing `.chatmode.md` files in `.github/chatmodes/` ensures precise enablement and team-wide AI behavior standardization, driving consistency on complex projects. In parallel, Microsoft introduced the ‘Awesome GitHub Copilot Customizations’ repo—making it far easier for teams to define, share, and integrate purpose-built instructions, prompts, and chat modes. Teams now centralize standards while maintaining adaptability for unique workflows, aided by practical tips to blend global and domain-specific instructions.
- [GitHub Copilot Custom Chat Modes for Large, Complex Projects in VS Code](https://harrybin.de/posts/github-copilot-custom-chat-modes/)
- [Introducing the Awesome GitHub Copilot Customizations Repo](https://devblogs.microsoft.com/blog/introducing-awesome-github-copilot-customizations-repo)
- [How to Improve GitHub Copilot Results with Instruction Files and Custom Chat Modes](https://harrybin.de/posts/improve-github-copilot-results/)
## Expanding Agentic Workflows and Task Delegation
Copilot’s transition to a true agent is reflected in the new Agents page (public preview), which allows teams to delegate specific coding tasks—like technical debt reduction or bug fixing—to Copilot and track progress within a unified dashboard. Chris Reddington’s guide demonstrates how agentic workflows let developers convert issues to actionable steps, clarify requirements through chat modes, and securely offload code generation with MCP servers. Offloading routine work dramatically improves overall productivity and innovation capacity.
- [Agents page for GitHub Copilot coding agent](https://github.blog/changelog/2025-07-02-agents-page-for-copilot-coding-agent-in-public-preview)
- [From idea to PR - A guide to GitHub Copilot’s agentic workflows](https://github.blog/ai-and-ml/github-copilot/from-idea-to-pr-a-guide-to-github-copilots-agentic-workflows/)
- [5 ways to transform your workflow using GitHub Copilot and MCP](https://github.blog/ai-and-ml/github-copilot/5-ways-to-transform-your-workflow-using-github-copilot-and-mcp/)
## Enhanced Review, Research, and Web Capabilities
Copilot now significantly eases large pull request reviews, offering more intelligent, prioritizing feedback in PRs with 20+ files—thus lowering reviewer burden. Copilot search is now integrated into GitHub Docs, turning natural questions into instant, contextual answers and streamlining documentation lookup. Additionally, coding agents now feature web browser capabilities via Playwright MCP, facilitating browser automation, data extraction, and live testing—all within Copilot, supporting comprehensive end-to-end automation and research.
- [Copilot code review - Better handling of large pull requests](https://github.blog/changelog/2025-07-02-copilot-code-review-better-handling-of-large-pull-requests)
- [Copilot search now on GitHub Docs](https://github.blog/changelog/2025-06-30-copilot-search-now-on-github-docs)
- [Copilot coding agent now has its own web browser](https://github.blog/changelog/2025-07-02-copilot-coding-agent-now-has-its-own-web-browser)
## Cross-IDE Support and Prompt Optimization
Copilot Vision extends key features to Xcode, including image upload/discussion in chat—crucial for UI reviews in macOS development—alongside enhanced custom instructions and locale-aware support. Prompt engineering best practices are further emphasized: specificity (languages, frameworks, intent) in prompts is proving essential for quality output. Moving from generic to highly tailored prompting is accelerating accurate code generation and faster bug fixes.
- [GitHub Copilot in Xcode - Explore with Copilot Vision, custom instructions, and locale response support](https://github.blog/changelog/2025-06-29-github-copilot-in-xcode-explore-with-copilot-vision-custom-instructions-and-locale-response-support)
- [Avoid These Common Copilot Prompts: How to Get Better Results with Specific Guidance](https://cooknwithcopilot.com/blog/avoid-these-common-prompts.html)
---',
    'This week’s GitHub Copilot updates showcase growing maturity in AI tooling for developers, spotlighting productivity improvements through advanced workflows, customizable automation, and cross-IDE feature growth. VS Code, Xcode, and other environments now offer powerful configuration avenues so teams can tune Copilot to match specific project requirements, with a focus on scalable task automation, end-to-end workflow support, community sharing, and prompt optimization for larger codebases.',
    1751871600, 'github-copilot', '/github-copilot/roundups/weekly-github-copilot-roundup-2025-07-07', 'TechHub',
    'TechHub', 'DD7BFF653BD0338E1FF6D3106CA51B879F0AE394CE5DFD11B06E44F7BF78FF11', ',GitHub Copilot,VS Code,Xcode,Copilot Vision,Custom Chat Modes,Instruction Files,Prompt Engineering,Copilot Coding Agent,Agents Page,MCP,Playwright,Pull Request Review,GitHub Docs,Developer Productivity,Automation,AI,Roundups,',
    false, false, false, false, true,
    false, false, 16
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
