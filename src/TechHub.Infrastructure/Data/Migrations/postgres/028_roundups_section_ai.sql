-- Migration 028: Roundup INSERTs for section 'ai'
-- Generated: 2026-05-21 from localhost database (AI-regenerated metadata).
-- Safe to re-run: ON CONFLICT DO UPDATE overwrites all fields with source-of-truth values.

-- ── content_items ─────────────────────────────────────────────────────────────────
-- weekly-ai-roundup-2026-05-11
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-05-11', 'roundups', 'Weekly AI Roundup: Production Agents, Governed Data, Safer Tooling',
    'This week focused on making AI agents more controllable in real workflows, from Copilot context improvements and tracing in VS Code to enterprise-managed CLI plugins, model deprecation deadlines, and clearer review hygiene for agent-authored PRs. Microsoft advanced production agent building and deployment with the Agent Framework and Azure AI Foundry, while Fabric and Databricks shipped practical operations features for discoverability, concurrency, monitoring, and recovery. Security and governance news emphasized token theft defense, passkey rollout realities, and shifting scanning earlier into agent tools with GitHub MCP Server and runtime-aware code-to-cloud visibility.
<!--excerpt_end-->
## Microsoft Agent Framework in .NET: agent building blocks, orchestration patterns, and a production deployment path
The Microsoft Agent Framework (v1.0) continued to take shape as a practical set of .NET building blocks for agentic apps, extending the same "from local to production" story we covered last week into more concrete .NET-first primitives. Jeremy Likness framed it as the third building block alongside the broader Microsoft.Extensions.AI story, focusing on what you need once you move past single-prompt chat: tool calling to let the model take actions, sessions (via `AgentSession`) to keep multi-turn state coherent, and memory via context providers (`AIContextProvider`) so the agent can retrieve and inject the right information at the right time. A key theme is that "agent" is not just a wrapper around an LLM call, its the combination of tools, state, and structured flow that makes behavior repeatable.
That emphasis on structured flow showed up again in Jacob Alber''s walkthrough of the Handoff orchestration pattern. It is a practical continuation of last week''s focus on multi-agent workflows that can be run and shipped, not just diagrammed. Instead of one agent trying to do everything, you define a bounded graph of agents and let them route control between each other using injected handoff tools, while maintaining a shared transcript so context does not get lost mid-transfer. The post contrasts Handoff with Sequential and explicit conditional workflows, and includes both .NET and Python examples, plus guidance on when to add Human-in-the-Loop (HITL) checkpoints so the system can escalate decisions rather than guess.
The other half of the story was what happens after your agent works locally. Following last week''s introduction of Foundry Hosted Agents as the "where does the code actually run?" answer, Tao Chen and Shawn Henry described how to deploy Agent Framework agents to Foundry Hosted Agents (Foundry Agent Service) in Azure AI Foundry, with concrete steps that mirror real delivery pipelines: container packaging with Azure Developer CLI (azd) and Azure Container Registry (ACR), choosing between protocol styles (`/responses` vs `invocations`) depending on how you want clients to interact, and wiring identity through Microsoft Entra ID instead of shipping keys around. They also call out operational basics that agent apps need just as much as APIs do, like versioned rollouts and built-in observability through Application Insights, so you can see tool calls, failures, and latency once the agent is under real load. Read together with last week''s OpenTelemetry-first messaging, the direction is consistent: Microsoft wants agent apps to inherit the same deployment and operations discipline teams already apply to services.
- [Microsoft Agent Framework – Building Blocks for AI Part 3](https://devblogs.microsoft.com/dotnet/microsoft-agent-framework-building-blocks-for-ai-part-3/)
- [A Tour of Handoff Orchestration Pattern](https://devblogs.microsoft.com/agent-framework/a-tour-of-handoff-orchestration-pattern/)
- [From Local to Production: Deploy Your Microsoft Agent Framework Agent with Foundry Hosted Agents](https://devblogs.microsoft.com/agent-framework/from-local-to-production-deploy-your-microsoft-agent-framework-agent-with-foundry-hosted-agents/)
## Azure AI Foundry + Fabric data: governed knowledge sources and real-world operations
Azure AI Foundry pushed further into "use your real data, but keep it governed" by making the OneLake catalog natively available in Foundry (now generally available). This is a direct continuation of last week''s MCP-through-Fabric storyline, where Fabric started to look like an AI tool surface and operational plane for agents. Here, the focus is more on the RAG setup path and discoverability: instead of switching contexts to hunt for datasets, you can browse OneLake assets directly while creating knowledge sources, which matters when teams want retrieval-augmented generation (RAG) that is both discoverable and compliant. The GA post walks through prerequisites and the setup path for creating a Foundry knowledge base backed by OneLake and Azure AI Search, a common pairing when you need governed storage (Fabric) plus an index optimized for retrieval (AI Search). It also matches the broader Foundry direction from last week, where governance and reuse move "up a layer" so teams can scale beyond one-off demos.
A Microsoft case study made that architecture feel less abstract by showing how Porsche Cup Brasil built AI-assisted race operations on the same stack. Their crash analysis workflow uses Azure AI Foundry with Azure AI Search and Microsoft Fabric, with apps running on Azure Kubernetes Service (AKS) and a human validation step to confirm conclusions before decisions get acted on. That HITL step echoes the practical safety thread running through both weeks: agentic systems need approval paths and auditability when outcomes matter. The broader operations loop pulls real-time telemetry into Fabric and visualizes it with Power BI for anomaly detection during races, illustrating a pattern many teams will recognize: stream data into an analytics layer, let AI help summarize or classify events, then keep a human in the approval path when the cost of a wrong call is high.
- [OneLake catalog is now natively available in Foundry (Generally Available)](https://community.fabric.microsoft.com/t5/Fabric-Updates-Blogs/OneLake-catalog-is-now-natively-available-in-Foundry-Generally/ba-p/5178376)
- [Inside Porsche Cup Brasil’s AI-powered race operations](https://news.microsoft.com/source/latam/features/ai/porsche-cup-brasil-ai-powered-crash-analysis/?lang=en)
## Agentic developer experience: IDE chat sessions, safer terminals, and editors that can opt out of AI
Tooling changes this week reflected a common friction point with agentic workflows: once you have multiple conversations, multiple models, and terminal access, the UI and safety defaults start to matter. That lines up with last week''s platform-side emphasis on "safe to run" agents (sandboxing, identity, observability), but viewed from the developer workstation where accidents (like leaking secrets) actually happen. Visual Studio Code 1.120 (Insiders) refined chat and agent workflows with session organization changes (helpful when you are juggling several tasks), a model context size picker (so you can trade off cost/latency vs how much context the model can read), and safer handling of terminal password prompts to reduce the chance of accidentally leaking secrets into an agent session. For extension authors, it also introduced a proposed `customDiffEditorProvider` API, alongside improvements across GitHub and Copilot CLI-related integrations.
On the editor side, Zed reached version 1.0 and positioned itself clearly as a "traditional editor and AI tool" rather than an AI-first environment. DevClass highlighted its Rust-based implementation, LSP-driven language support, and optional AI features that include Zeta LLM predictions and parallel agents, plus support for Agent Client Protocol (ACP). The practical detail many teams will care about is that AI can be disabled entirely, which complements the broader enterprise control story from last week (governed tools, identity boundaries, and hosted isolation): sometimes the safest AI feature is the one you can reliably turn off where policy demands it.
- [Visual Studio Code 1.120](https://code.visualstudio.com/updates/v1_120)
- [Zed team releases version 1.0 of Rust-built editor: Traditional editor and AI tool](https://www.devclass.com/software/2026/05/07/zed-team-releases-version-10-of-rust-built-editor/5234770)
## Applied agent patterns in teams: stand-ups, document pipelines, and production-minded tutorials
Several posts focused less on platforms and more on what teams are actually building with LLMs and agents, reinforcing the same "production reality" theme that ran through last week''s Agent Framework and Foundry updates. John Edward''s "Daily Stand-Up Agent" design connects to Jira and Azure DevOps, grounds responses on sprint and work item data, and uses LLM summarization to generate stand-up notes, blocker alerts, and sprint health reporting through a conversational interface. It highlights the integration work that tends to dominate these projects (Azure DevOps Work Item APIs, Jira REST API, OAuth, and RBAC), and why grounding plus prompt design matters when summaries must reflect real status rather than plausible text.
On the data extraction side, tanyabaranwal shared an event-driven pipeline for contract processing that starts with Blob Storage and an Azure Functions Blob trigger, uses Azure AI Document Intelligence to extract layout and tables, normalizes the output into a canonical JSON schema, and stores results in Cosmos DB for downstream use. The post rounds out the "production" story with monitoring in Application Insights and security practices like Key Vault, Managed Identity, and RBAC, which tracks closely with the lifecycle framing from last week (identity, governance, and observability as defaults rather than add-ons). Even when this is not presented as an "agent," it fits the same operational model: AI components become dependable services only when they ship with the same controls you expect for any other workload.
Rounding out the week, GitHub''s "Rubber Duck Thursdays" episode walked through building an AI agent app for a fictional company, including what changes when you plan for production deployment rather than a local prototype. That mirrors Microsoft''s recent messaging almost point-for-point: the throughline across these examples is that the agent behavior is only half the job, the rest is connectors, identity, monitoring, and a clear strategy for what the model can and cannot do in your workflow.
- [The Daily Stand-Up Agent: A Custom Copilot for Summarizing Jira & Azure DevOps Progress](https://dellenny.com/the-daily-stand-up-agent-a-custom-copilot-for-summarizing-jira-azure-devops-progress/)
- [Building a Scalable Contract Data Extraction Pipeline with Microsoft Foundry and Python](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-scalable-contract-data-extraction-pipeline-with/ba-p/4513681)
- [Rubber Duck Thursdays: Building an AI agent app](https://www.youtube.com/watch?v=zG6PJHVaUxs)',
    'This week focused on making AI agents more controllable in real workflows, from Copilot context improvements and tracing in VS Code to enterprise-managed CLI plugins, model deprecation deadlines, and clearer review hygiene for agent-authored PRs. Microsoft advanced production agent building and deployment with the Agent Framework and Azure AI Foundry, while Fabric and Databricks shipped practical operations features for discoverability, concurrency, monitoring, and recovery. Security and governance news emphasized token theft defense, passkey rollout realities, and shifting scanning earlier into agent tools with GitHub MCP Server and runtime-aware code-to-cloud visibility.',
    1778482800, 'ai', '/ai/roundups/weekly-ai-roundup-2026-05-11', 'TechHub',
    'TechHub', '0C51E6504AF3E0409B1DAA7D021992D4E73596F770F92C266661D2574846D5D5', ',AI Agents,Microsoft Agent Framework,.NET,Azure AI Foundry,Foundry Hosted Agents,Retrieval Augmented Generation,Microsoft Fabric,OneLake,Azure AI Search,Application Insights,Microsoft Entra ID,VS Code,GitHub Copilot,Azure Functions,Azure AI Document Intelligence,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-05-04
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-05-04', 'roundups', 'Weekly AI Roundup: Agent Interop, Foundry Ops, and RAG Governance',
    'Copilot moves toward more agentic workflows across IDEs and GitHub, while June 1 brings token-based billing, AI Credits, and new meters like Actions minutes for private-repo code review. In parallel, Microsoft and the broader ecosystem tightened the production story for agents with GPT-5.5 in Foundry, GA interoperability protocols (A2A and MCP), and more concrete guidance on observability, retrieval, and governance. Platform updates across Azure and Fabric focused on controlled operations: sovereign and disconnected deployments, least-privilege storage access, SLI/SLOs in Azure Monitor, and better real-time pipeline monitoring.
<!--excerpt_end-->
## Azure AI Foundry and Microsoft Foundry: GPT-5.5 lands, and the platform story gets clearer
Microsoft put GPT-5.5 into general availability inside Microsoft Foundry, positioning it as a more enterprise-ready way to consume frontier OpenAI models with the controls teams expect. The update calls out practical model-level gains like stronger agentic coding behavior, improved long-context reasoning, and better token efficiency, which matters when you are building multi-step agent flows that keep state, retrieve sources, and iterate. The more important shift is the surrounding platform: Foundry Agent Service is framed as the operational layer for hosted agents, with isolation boundaries, Microsoft Entra identity integration, and governance so teams can manage access and reduce the "who can make the agent do what" risk that shows up quickly in enterprise deployments.
On the developer experience side, Foundry Toolkit added an in-editor image generation loop in VS Code. The flow is designed to be end-to-end: browse the model catalog, deploy GPT-Image-2 into an Azure AI Foundry project, generate images in an Image Playground, then export code snippets you can drop into an app. It is a small update, but it reflects a pattern Foundry is pushing this month: keep model selection, deployment, experimentation, and integration in one place so teams do not treat "prompting" and "shipping" as two separate projects.
- [OpenAI’s GPT-5.5 in Microsoft Foundry: Frontier intelligence on an enterprise ready platform](https://azure.microsoft.com/en-us/blog/openais-gpt-5-5-in-microsoft-foundry-frontier-intelligence-on-an-enterprise-ready-platform/)
- [Streamline Image Generation Workflow in Foundry Toolkit](https://techcommunity.microsoft.com/t5/microsoft-developer-community/%EF%B8%8Fstreamline-image-generation-workflow-in-foundry-toolkit/ba-p/4516055)
## Microsoft Agent Framework 1.0 and A2A v1: interoperability becomes a first-class feature
Agent work this week was less about "how to build an agent" and more about "how agents talk to other agents and tools without custom glue." Microsoft Agent Framework 1.0 reached GA with a clearer split between agent architecture and traditional workflow orchestration, plus a set of runtime components meant to standardize how you host, route, and invoke tools across deployments. Two interoperability pieces stand out. First is A2A (agent-to-agent), which is positioned as a common way for agents to communicate across platforms. Second is MCP (Model Context Protocol), which is used as the tool invocation and connectivity layer so an agent can call capabilities without every team inventing its own tool schema.
For .NET teams, A2A Protocol v1.0 support landed in Microsoft Agent Framework for .NET with updated client and hosting packages and some concrete hosting details that matter in production. Discovery is handled via well-known agent cards (so clients can find capabilities without hardcoding), and streaming is supported over Server-Sent Events (SSE), which is a practical fit for incremental agent output and UI updates. The post also calls out migration notes from v0.3, which helps teams who started experimenting earlier move to the now-stable protocol and hosting APIs in ASP.NET Core.
- [The Future of Agentic AI: Inside Microsoft Agent Framework 1.0](https://techcommunity.microsoft.com/t5/microsoft-developer-community/the-future-of-agentic-ai-inside-microsoft-agent-framework-1-0/ba-p/4510698)
- [A2A v1 Is Here: Cross-Platform Agent Communication in Microsoft Agent Framework for .NET](https://devblogs.microsoft.com/agent-framework/a2a-v1-is-here-cross-platform-agent-communication-in-microsoft-agent-framework-for-net/)
## Foundry IQ and agentic retrieval on Azure AI Search: building knowledge copilots with fewer shortcuts
Enterprise RAG (retrieval-augmented generation) keeps running into the same hard problems: permissions, citations, query quality, and latency once you add multi-step reasoning. Two posts this month focused on Foundry IQ as the knowledge layer for that problem, backed by Azure AI Search. One walkthrough shows how to build an "enterprise knowledge copilot" using Foundry IQ knowledge bases with an agentic retrieval loop (plan, search, rank, reflect, synthesize). The point is not that agents can search, but that retrieval becomes iterative and self-correcting, which is often what it takes to get stable answers across messy internal content. It also leans on MCP-based connectivity so the agent can connect to tools and retrieval services in a standard way, while being candid about the real trade-offs teams need to measure (preview maturity, cost, latency, and security constraints).
A companion guide tries to reduce confusion about what Foundry IQ is and is not, when it fits, and how to use it without going through a full "copilot" layer. It highlights querying Foundry IQ knowledge bases directly with the Azure AI Search Python SDK (`azure-search-documents`), including returning citations and applying permission trimming so the retrieval layer respects user access (often enforced through Microsoft Entra ID patterns in enterprise environments). Together, the posts land on a pragmatic message: treat the knowledge base as a governed, queryable asset first, then decide how much agentic orchestration you actually need.
- [Building an Enterprise Knowledge Copilot with Foundry IQ and Agentic Retrieval on Azure AI](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-an-enterprise-knowledge-copilot-with-foundry-iq-and/ba-p/4512308)
- [Making Sense of Azure AI Foundry IQ](https://techcommunity.microsoft.com/t5/microsoft-developer-community/making-sense-of-azure-ai-foundry-iq/ba-p/4513131)
## Agent-driven UI and operations: streaming interfaces and real observability for agent workloads
As agents become long-running and multi-step, the UI and ops story matters as much as the prompt. AG-UI was introduced as a protocol for how agents communicate with frontends using streaming events, declarative UI proposals, and shared state updates, with explicit support for human-in-the-loop interrupts so a user can approve, correct, or stop actions mid-flight. The design lines up with the same streaming patterns showing up elsewhere (notably SSE), and the security notes point teams back to common Azure controls like Azure AD (Microsoft Entra) for securing endpoints and Key Vault for handling secrets.
On the operations side, another post focused on observability for agent workloads in Azure AI Foundry, specifically what it takes to get traces you can actually use in incident response and performance work. It compares tracing depth and setup effort across Microsoft Agent Framework, Semantic Kernel, LangChain/LangGraph, and the OpenAI Agent SDK, using OpenTelemetry pipelines into Azure Monitor and Application Insights. The practical takeaway is that "agent observability" is not a single feature toggle; it is a combination of consistent spans/attributes across tool calls and model invocations, plus an instrumentation story that does not collapse once you add multiple agents, external tools, and streaming responses.
- [AG-UI: The Future of Agent-Driven User Interfaces](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/ag-ui-the-future-of-agent-driven-user-interfaces/ba-p/4515769)
- [Operating AI Agents on Azure: Observability with Azure AI Foundry](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/operating-ai-agents-on-azure-observability-with-azure-ai-foundry/ba-p/4515975)
## Other Artificial Intelligence News
Microsoft highlighted a real multi-agent workflow in science with Microsoft Discovery, describing an automated computational chemistry screening pipeline that runs Gaussian 16 DFT simulations in parallel (using an MPI-style master-worker pattern), then feeds results into ML-based redox potential prediction with structured JSON outputs so downstream systems can consume the results reliably.
- [How Microsoft Discovery Is Empowering Scientists to Do More](https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-ms-discovery-is-empowering-scientists-to-do-more/ba-p/4516670)
A .NET-focused tutorial showed how to extend an agent built with the .NET Agent Framework by adding a class-based skill, using a locally hosted LLM through Ollama, which is a useful pattern when you want the "skills" abstraction without requiring a hosted model for every experiment.
- [Add class-based skill to an AI agent in .NET Agent Framework](https://www.youtube.com/watch?v=9B1WNIjKNDU)',
    'Copilot moves toward more agentic workflows across IDEs and GitHub, while June 1 brings token-based billing, AI Credits, and new meters like Actions minutes for private-repo code review. In parallel, Microsoft and the broader ecosystem tightened the production story for agents with GPT-5.5 in Foundry, GA interoperability protocols (A2A and MCP), and more concrete guidance on observability, retrieval, and governance. Platform updates across Azure and Fabric focused on controlled operations: sovereign and disconnected deployments, least-privilege storage access, SLI/SLOs in Azure Monitor, and better real-time pipeline monitoring.',
    1777878000, 'ai', '/ai/roundups/weekly-ai-roundup-2026-05-04', 'TechHub',
    'TechHub', '96DCE9B28DB9BF4668E8B2D745BEAE2FC96A7C671F55FBD908008CCC2C358E5D', ',AI Agents,GitHub Copilot,Microsoft Foundry,Azure AI Foundry,GPT 5.5,Microsoft Agent Framework,A2A Protocol,MCP,Foundry IQ,Azure AI Search,RAG,OpenTelemetry,Azure Monitor,Application Insights,Server Sent Events,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-04-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-04-27', 'roundups', 'Weekly AI Roundup: Agents in Production, MCP Tools, and Guardrails',
    'This week pushed AI assistants further into real workflows (IDE agents, azd deployments, and MCP-connected tools) while tightening the controls that keep costs and governance predictable, including Copilot individual plan limits and admin-gated access to GPT-5.5. Across Azure and Fabric, the focus stayed on secure-by-default operations (private networking, managed identities, outbound controls) and practical platform plumbing for MLOps, streaming, and telemetry. DevOps and security updates added more change-management work (TLS SHA-1 removal, longer GitHub App tokens), plus concrete improvements in scanning, dependency visibility, and Defender-guided incident disruption.
<!--excerpt_end-->
## Azure AI Foundry + Microsoft Agent Framework: shipping the agent platform (v1.0, hosted compute, reusable tools, and real deployment paths)
Microsoft Agent Framework hit v1.0 with a clear push to cover the full developer journey, not just local prototyping. The workflow described this week starts where most teams actually begin (VS Code) and carries through composition, tool access, managed memory, and hosted deployment in Azure AI Foundry, with azd positioned as the repeatable path from local to cloud. A key point here is that "agent app" concerns are getting treated like standard cloud app concerns: you get observability that is designed to be on by default (OpenTelemetry tracing), and you get security testing built into the lifecycle (a GA "AI Red Teaming Agent" capability called out as part of the story). That combination matters because agent behavior is often emergent, so teams need both trace-level visibility and a repeatable way to probe for risky behavior before and after release.
On the infrastructure side, Foundry Agent Service introduced Hosted Agents (public preview) to solve a recurring pain point: where code execution and tool calls actually run, and how you isolate them. Hosted Agents provide per-session VM-isolated sandboxes with a persistent filesystem state, plus scale-to-zero so you are not paying for always-on sandboxes. For enterprise deployments, the practical features are the ones that unblock real rollout discussions: VNet support for network control, integrated Microsoft Entra ID with on-behalf-of (OBO) flows for delegated access, and OpenTelemetry-based observability so operations teams can trace what the agent did and when.
Tooling and reuse got a big lift with Toolboxes in Azure AI Foundry (public preview). Instead of wiring the same set of tools (and auth) separately for every agent runtime, Toolboxes bundle tools behind a single MCP-compatible endpoint and centralize authentication and governance. The immediate developer benefit is fewer one-off integrations: multiple agent runtimes can reuse the same tool setup without copying secrets or duplicating policy logic, and teams can standardize how tools are exposed across Microsoft Agent Framework and other MCP-speaking runtimes. Azure AI Search shows up as an example of the kind of capability you would want to expose consistently as a governed tool.
Finally, the Agent Framework content this week included a concrete walkthrough of a multi-agent workflow that is meant to be runnable, not conceptual. The tutorial builds a Python-based multi-agent system using Microsoft Agent Framework, hosts it behind an OpenAI Responses API-compatible endpoint, deploys it to Azure AI Foundry using azd, and then optionally exposes it into Microsoft 365 (Teams/Copilot) via the Microsoft 365 Agents SDK. That is a useful pattern for teams that need one implementation to serve both "developer API" consumers and end-user surfaces inside Microsoft 365, without rewriting the whole agent stack.
- [From Local to Production: The Complete Developer Journey for Building, Composing, and Deploying AI Agents](https://devblogs.microsoft.com/foundry/from-local-to-production-the-complete-developer-journey-for-building-composing-and-deploying-ai-agents/)
- [Introducing the new hosted agents in Foundry Agent Service: secure, scalable compute built for agents](https://devblogs.microsoft.com/foundry/introducing-the-new-hosted-agents-in-foundry-agent-service-secure-scalable-compute-built-for-agents/)
- [Introducing Toolboxes in Foundry](https://devblogs.microsoft.com/foundry/introducing-toolboxes-in-foundry/)
- [Microsoft 365 multi-agent workflow with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/microsoft-365-multi-agent-workflow-with-microsoft-agent/ba-p/4514164)
## CodeAct + Hyperlight in Agent Framework: fewer model turns by collapsing tool loops into execute_code
Agent Framework also got an important performance and architecture knob with CodeAct support via the agent-framework-hyperlight (alpha) package. The problem it targets is familiar if you have built tool-using agents: the model often has to bounce through multiple "call_tool -> inspect output -> decide next step -> call_tool again" turns, and each turn adds latency and token cost. The CodeAct approach described here uses Hyperlight micro-VM sandboxes to safely execute code so the agent can perform multiple steps inside a single execute_code turn, instead of repeatedly round-tripping to the model for every tool call.
The practical takeaway is that this is not just "faster"; it changes how you design tools. If you can shift multi-step logic into sandboxed code execution, you can reduce the number of brittle intermediate prompts and tool schemas you need to maintain. The post also calls out the safety side explicitly: approvals (approval_mode) and careful tool design are still part of the contract, because "execute code" is powerful even when sandboxed. For teams building internal agents that need to interact with real systems, this pattern offers a clearer separation between (1) the model deciding what to do and (2) a tightly controlled runtime executing it.
- [CodeAct in Agent Framework: Faster Agents with Fewer Model Turns](https://devblogs.microsoft.com/agent-framework/codeact-with-hyperlight/)
## MCP keeps expanding: Fabric turns platform operations into agent tools, and Foundry standardizes tool access
Model Context Protocol (MCP) showed up repeatedly this week as the common interface for plugging agents into enterprise platforms, and Microsoft Fabric was the clearest example of what that looks like when taken seriously. With Fabric Local MCP now GA and Fabric Remote MCP in preview, Fabric is positioning MCP servers as a way for assistants and agents to generate API-grounded code and then carry out authenticated operations across core Fabric resources (workspaces, items, permissions, and OneLake). For developers, the important part is that this is not "screen-scraping automation"; it is a structured tool surface backed by Entra ID, RBAC, and audit logging, which makes it easier to justify agent-driven operations in environments that have governance requirements.
This connects directly to what Foundry is doing with Toolboxes: MCP-compatible endpoints become the stable contract, while authentication and governance move to a centralized layer. If you are building agents that need to query data (Fabric/OneLake), search content (Azure AI Search), and then act (create/update items, manage permissions), MCP is emerging as the way Microsoft expects those tool surfaces to be exposed and reused across runtimes.
- [Agentic Fabric: How MCP is turning your data platform into an AI-native operating system](https://blog.fabric.microsoft.com/en-US/blog/agentic-fabric-how-mcp-is-turning-your-data-platform-into-an-ai-native-operating-system/)
- [Introducing Toolboxes in Foundry](https://devblogs.microsoft.com/foundry/introducing-toolboxes-in-foundry/)
## Models and applied AI: MAI multimodal models, agentic R&D (Discovery), and an AI-for-nuclear collaboration
On the model side, Microsoft highlighted new in-house MAI models aimed at multimodal workloads: MAI-Transcribe-1, MAI-Voice-1, and MAI-Image-2. The developer-relevant angle in the write-up is less about raw benchmark claims and more about how these models fit into Azure AI Foundry with enterprise governance defaults: RBAC, Microsoft Entra ID integration, Microsoft Purview alignment, and Managed Identity support. That matters if you want to run speech, voice, and image pipelines inside the same governed environment as your agents, especially when those agents need to call models and tools under consistent identity and policy.
For research-heavy organizations, Microsoft also expanded preview access for Microsoft Discovery, described as an Azure-based agentic AI platform for R&D that combines agent orchestration, graph-based knowledge, and high performance computing (HPC). The early examples span materials science, oncology workflows, engineering simulation, and semiconductor design, which signals the kind of workloads where "agent + knowledge graph + HPC" is more than a chat interface: it is meant to coordinate long-running, compute-heavy experimentation while keeping context grounded in structured knowledge.
Separately, Microsoft described an AI-for-nuclear collaboration with NVIDIA that focuses on using generative AI and digital twins on Azure to streamline permitting, accelerate design and simulation, and improve planning and operations for nuclear plants. From a developer perspective, the interesting bit is the stack direction: digital twins plus simulation environments (including NVIDIA Omniverse) paired with Azure-hosted AI tooling, which hints at more end-to-end patterns for building AI systems that reason over physical-world models, not just documents and tickets.
- [Microsoft’s New In‑House AI Models (MAI‑Transcribe, MAI‑Voice, MAI‑Image)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/microsoft-s-new-in-house-ai-models-mai-transcribe-mai-voice-mai/ba-p/4512731)
- [Microsoft Discovery: Advancing agentic R&D at scale](https://azure.microsoft.com/en-us/blog/microsoft-discovery-advancing-agentic-rd-at-scale/)
- [AI for nuclear energy: Powering an intelligent, resilient future](https://www.microsoft.com/en-us/industry/blog/energy-and-resources/2026/03/24/ai-for-nuclear-energy-powering-an-intelligent-resilient-future/)
## AI security: hunting for "infiltrating IT workers" with Defender telemetry and KQL
Microsoft Defender Security Research published a practical guide for detecting the Jasper Sleet actor''s tactic of abusing remote hiring and onboarding to gain legitimate access as "IT workers." The guidance is valuable because it treats the recruitment pipeline as a security surface, not just an HR process: it maps suspicious patterns across recruiting systems and identity signals, then ties them to post-hire behavior once access exists.
On the implementation side, the post focuses on using Microsoft Defender for Cloud Apps and Microsoft Defender XDR, including Workday-focused telemetry and Advanced Hunting queries with KQL. The point is to give defenders something actionable to operationalize: hunt for suspicious recruiting and communications signals, correlate them with identity events, and then watch for post-hire access patterns that do not fit the expected onboarding path. For teams that build or secure agent-driven workflows around identity and HR systems, it is a reminder that "trusted business tools" (like Workday) produce the telemetry you may need when an attacker uses process abuse rather than malware.
- [Detection strategies across cloud and identities against infiltrating IT workers](https://www.microsoft.com/en-us/security/blog/2026/04/21/detection-strategies-cloud-identities-against-infiltrating-it-workers/)',
    'This week pushed AI assistants further into real workflows (IDE agents, azd deployments, and MCP-connected tools) while tightening the controls that keep costs and governance predictable, including Copilot individual plan limits and admin-gated access to GPT-5.5. Across Azure and Fabric, the focus stayed on secure-by-default operations (private networking, managed identities, outbound controls) and practical platform plumbing for MLOps, streaming, and telemetry. DevOps and security updates added more change-management work (TLS SHA-1 removal, longer GitHub App tokens), plus concrete improvements in scanning, dependency visibility, and Defender-guided incident disruption.',
    1777273200, 'ai', '/ai/roundups/weekly-ai-roundup-2026-04-27', 'TechHub',
    'TechHub', 'C797976F4A2946B5ADB8D308E12235CB85C30632FF8FEBB17D2EF90778083555', ',Azure AI Foundry,Microsoft Agent Framework,AI Agents,Foundry Agent Service,Hosted Agents,MCP,Toolboxes,OpenTelemetry,Azd,Microsoft Entra ID,Managed Identity,VNet Integration,Hyperlight,CodeAct,Microsoft Defender XDR,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-04-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-04-20', 'roundups', 'Weekly AI Roundup: IDE agents, governed tools, and hosting',
    'This week''s AI news leaned into making agent development look more like normal software engineering: tighter IDE loops for building, debugging, and evaluating; clearer production hosting and orchestration options; and concrete patterns for connecting agents to governed data and automation. This continues last week''s "run it like software" framing where stable runtimes, inspectable tool contracts, and day-two controls (identity, policy, cost, evaluation) become the default rather than add-ons. Microsoft Foundry and Fabric also expanded platform capabilities with new models, fine-tuning options, MCP toolchains, and agent experiences that are easier to monitor and audit.
<!--excerpt_end-->
## Microsoft Foundry in the IDE: end-to-end agent and model workflows
Building on last week''s Foundry standardization theme (Responses API compatibility, Agent Service GA, cloud+local deployment), Microsoft Foundry Toolkit for VS Code is now GA. It positions VS Code as the place where experimentation, agent engineering, evaluation-as-tests, deployment, and on-device model work (enabled by Foundry Local GA) can live in one loop.
For experimentation, the Model Catalog includes 100+ cloud and local models (OpenAI/Anthropic/Google plus local ONNX/Foundry Local/Ollama), along with a Model Playground for side-by-side comparisons, multimodal testing, optional web search, and streaming. "View Code" generates Python/JavaScript/C#/Java snippets that match what you tested, which mirrors last week''s focus on keeping a tight contract between what you test and what you run.
Agent building splits into low-code Agent Builder (prompt optimizer, tool catalog including local MCP servers, tool approvals, save to Foundry) and code-first scaffolding aligned to frameworks like Microsoft Agent Framework and LangGraph. The toolkit treats MCP servers as first-class tool sources inside the IDE, continuing last week''s MCP operationalization thread. Agent Inspector adds IDE-native debugging and observability: F5 debugging with breakpoints, stepping and variables, streaming tool-call visualization, workflow graphs, and local span tracing across tool calls. That brings last week''s "observability is design" message earlier into local development.
Evaluations show up as tests: pytest-style definitions run in VS Code Test Explorer, results can be analyzed in Data Wrangler, and reused for scaled runs in Foundry. This fits last week''s point that evaluation, monitoring, and cost controls are day-two expectations, not something you bolt on later.
The GA deep dive also treats Windows on-device AI as part of the same surface, extending last week''s "cloud + local" story with IDE controls. The local pipeline converts, quantizes, and evaluates Hugging Face models into ONNX for Windows ML, and targets execution providers across hardware (OpenVINO, TensorRT, Qualcomm QNN, AMD MIGraphX/VitisAI). Profiling includes CPU/GPU/NPU/memory views, Windows ML event breakdown (startup vs per-request), and operator-level tracing showing placement and timing. It also supports LoRA fine-tuning for Phi Silica via a cloud job on Azure Container Apps, then downloads the adapter for Phi Silica LoRA APIs. The intent is to reduce bespoke ML infrastructure when you only need an adapter.
- [Foundry Toolkit for VS Code: A Deep Dive on GA](https://techcommunity.microsoft.com/t5/microsoft-developer-community/foundry-toolkit-for-vs-code-a-deep-dive-on-ga/ba-p/4509510)
## Production agent architecture: security, governance, memory, and hosting choices
As agents move from prototypes to production, guidance converged on control: observability does not help much without enforceable policies (tool allowlists, least privilege, auditable decisions) and a hosting model that fits scale and operational constraints. This continues last week''s shift from "build an agent" to "run an agent," where tracing, identity, evaluation, and guarded automation are treated as core design inputs.
On governance, the Agent Governance Toolkit walkthrough shows deterministic runtime policy enforcement for a multi-agent ASP.NET Core app on Azure App Service (MAF 1.0) using Microsoft.AgentGovernance 3.0.2. The flow is middleware-like: default-deny governance-policies.yaml with allow rules, loaded into a GovernanceKernel at startup (audit + metrics), then builder.UseGovernance(kernel, AgentName) so tool calls are evaluated before execution. This complements last week''s "interruptible tools" and human approvals by making governance a runtime gate across agents. Decisions and reasons land in Application Insights alongside OpenTelemetry traces, with KQL to find violations (customDimensions["governance.decision"] != "ALLOWED") and track token budgets via customMetrics ("governance.tokens.consumed"). It also extends into reliability with YAML SLOs and circuit breakers that reduce autonomy as error budgets burn, which matches last week''s "guardrails before automation" sequencing.
The Foundry security architecture checklist maps agent risks (prompt injection, tool misuse, exfiltration, over-privilege, drift) into Azure patterns: managed identities and Entra RBAC, private endpoints/Private Link, Key Vault, API Management gateways, tool allowlists, and strict output validation (JSON schema). It also calls for CI/CD evaluation and red-teaming so prompt and model changes trigger regression tests that can block deploys. This reinforces last week''s MCP auth patterns (managed identity vs OAuth passthrough vs secrets) in an end-to-end posture: tool auth is not enough without least privilege, network boundaries, and validation.
Statefulness also advanced with Foundry managed memory (preview), positioned as built-in long-term persistence integrated with Microsoft Agent Framework and LangGraph. Hooks include per-user scoping, automatic extraction (the platform decides what to store), and CRUD APIs so apps can inspect and correct memory (including "forget this"). It reduces the need for custom memory stores and standardizes user controls.
For hosting, a guide compared Container Apps, AKS, Functions, App Service, Foundry Agents, and Foundry Hosted Agents, then focused on Hosted Agents as "containerized custom app + agent-native APIs." This ties to last week''s Agent Service GA by showing how runtime and code package together while keeping agent concepts like Responses protocols and telemetry export. The walkthrough is implementation-level: LangGraph calculator agent + adapter, agent.yaml (kind: hosted, protocols: responses), Python 3.11-slim container on port 8088, deployed via azd and azure.ai.agents extension (build/push to ACR, create/start Hosted Agent). It also calls out scale-to-zero vs min-replicas cold-start tradeoffs, automatic OpenTelemetry export to App Insights, and RBAC implications (publishing can create a dedicated agent identity that needs separate permissions from the project managed identity).
- [Architecting Secure and Trustworthy AI Agents with Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/architecting-secure-and-trustworthy-ai-agents-with-microsoft/ba-p/4506580)
- [Govern AI Agents on App Service with the Microsoft Agent Governance Toolkit](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/govern-ai-agents-on-app-service-with-the-microsoft-agent/ba-p/4510962)
- [Using Microsoft Agent Framework with Foundry managed memory](https://www.youtube.com/watch?v=DZn9bNDEs4U)
- [Choosing the Right Azure Hosting Option for Your AI Agents](https://devblogs.microsoft.com/all-things-azure/hostedagent/)
## MCP and data-connected agents: Fabric OneLake, Postgres, and Oracle Database@Azure
Tool calling and agent-to-data patterns continue consolidating around MCP, extending last week''s shift from "tool glue" to supportable infrastructure (hosting, auth, self-hosted Azure MCP Server 2.0). The emphasis is on explicit discovery (schemas, metadata, permissions) so agents behave predictably, and on identity and RBAC as the primary safety boundary.
In Fabric, OneLake MCP tools are GA: a 19-command toolset for discovering workspaces and items, inspecting schemas and metadata via Table APIs, and browsing, reading, writing, and mapping storage via OneLake File/Access APIs. All access is constrained by the caller''s Azure identity and Fabric permissions. This pairs with last week''s Fabric "intelligence platform" guidance: instead of copying data into agent pipelines, you expose governed surfaces (semantic models, Table APIs, OneLake files) as tools with enforceable permissions. The example is practical: an agent inventories a mirrored database ("House Price Open Mirror"), documents schemas, distinguishes Parquet landing zones from managed Delta outputs, and checks replication health and monitoring signals to generate docs and basic health reports without manual portal work.
For Postgres, a Foundry + PostgreSQL walkthrough emphasizes MCP as a controlled integration layer for exploring the database, pairing natural-language-to-SQL with vector search for RAG retrieval. This matches last week''s Entra-authenticated MCP guidance (pre-authorized clients, JWT validation, OBO): database access becomes a governable tool surface instead of a broad connection string. A related "PostgreSQL Like a Pro" announcement points to more demos on modernizing Postgres apps on Azure, including MCP agent patterns and AI-assisted Oracle-to-Azure Database for PostgreSQL migrations in VS Code, pointing toward IDE loops where the agent helps iterate on conversion issues.
For Oracle estates, an Oracle Database@Azure patterns article lays out options based on how deterministic you need behavior to be: Copilot Studio + Oracle connectors; ORDS + PL/SQL REST APIs for predictable behavior (with DB governance like RLS/VPD); and hybrid Oracle "Select AI" (DBMS_CLOUD_AI.GENERATE) using Azure OpenAI to generate and validate SQL inside Oracle. It includes code-first Azure Functions (JDBC/python-oracledb), Logic Apps/Power Automate orchestration, and an advanced direction where MCP and in-database runtimes participate in ReAct-style loops. That continues last week''s "MCP as governed automation interface" thread applied to database operations and policy controls.
- [Give your AI agent the keys to OneLake: OneLake MCP (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/give-your-ai-agent-the-keys-to-onelake-onelake-mcp-generally-available/)
- [Build smart and secure agents with PostgreSQL on Azure using Microsoft Foundry](https://www.youtube.com/watch?v=aMqrpk6ge9A)
- [Take your PostgreSQL-backed apps to the next level](https://devblogs.microsoft.com/blog/take-your-postgresql-backed-apps-to-the-next-level)
- [Six agent integration patterns for Oracle Database@Azure with Microsoft Foundry, Azure OpenAI, Copilot Studio, and Logic Apps](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/your-oracle-data-is-sitting-next-to-microsoft-ai-are-you-using/ba-p/4509438)
## Model updates and customization: text-to-image efficiency and reinforcement fine-tuning
Foundry expanded its model surface in two ways: a higher-throughput text-to-image option intended for production usage, and more workflow controls for reinforcement fine-tuning with graders you can tune over time. This continues last week''s "right model per step" and "model choice as an engineering setting" thread, expanding beyond text agents to image pipelines and training workflows.
MAI-Image-2-Efficient (MAI-Image-2e) is now available in Microsoft Foundry without a waitlist, positioned as the throughput-focused sibling to MAI-Image-2 for high-volume and interactive generation. The announcement is explicit about tradeoffs: MAI-Image-2e targets latency and per-image cost (pricing: $5 per 1M text input tokens and $19.50 per 1M image output tokens), while MAI-Image-2 remains positioned for higher fidelity (portraits/photorealism, stylized outputs, longer/complex in-image text). Benchmark context (NVIDIA H100 at 1024x1024 with normalization notes) helps set expectations for how batch size and concurrency affect real deployments.
The April 2026 Foundry fine-tuning update focused on reinforcement fine-tuning (RFT) for o4-mini with api-version=2025-04-01-preview. Key changes include "global training" (trainingType: "globalstandard") across 13+ regions to lower per-token training rates with consistent infrastructure and model quality, plus more grader options. RFT now supports model graders using GPT-4.1, GPT-4.1-mini, and GPT-4.1-nano, alongside deterministic graders (string checks, Python, endpoint-based). Guidance is operational: start deterministic for speed, cost, and reproducibility, use model graders for semantic or multi-dimensional partial credit, and iterate nano -> mini -> full as rubrics stabilize. It also calls out common pitfalls (role ordering, schema mismatches, missing structured response_format when graders reference output_json) and reinforces a best practice aligned with last week''s MCP thread: treat tools as part of the environment and consider MCP for production tools even if you also offer a function-calling-compatible interface for fine-tuning.
- [MAI-Image-2-Efficient in Microsoft Foundry: flagship-quality text-to-image with lower cost and faster latency](https://microsoft.ai/news/mai-image-2-efficient/)
- [What’s New in Microsoft Foundry Fine-Tuning | April 2026](https://devblogs.microsoft.com/foundry/whats-new-in-foundry-finetune-april-2026/)
## Copilot Studio orchestration: mixing agents with workflows
Copilot Studio added clearer orchestration primitives for business process automation scenarios where you want agent reasoning but still need deterministic execution and audit trails. This builds on last week''s focus on approvals, guarded automation, and explicit topology by formalizing two patterns inside a workflow engine. The update describes "agent-in-workflow" (agent nodes for interpretation/synthesis at specific steps, with request-for-information escalation) and "workflow-as-tool" (agents call workflows as reliable sub-process tools, authored in natural language or reused from a library). The practical benefit is keeping the workflow as the orchestrator (branching, handoffs, approvals stay explicit) while inserting LLM steps where ambiguity is unavoidable.
- [New capabilities help you automate business processes by mixing AI agents and workflows](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/automate-business-processes-with-agents-plus-workflows-in-microsoft-copilot-studio/)
## Other AI News
Visual Studio''s Copilot Chat gained a "Debugger Agent" workflow in Visual Studio 18.5 GA that ties an agent into a live debugging session. You provide a GitHub/ADO item (or description), it proposes a hypothesis, sets breakpoints (with approval), observes the repro, inspects runtime state (variables/call stack), then proposes or applies a fix and reruns. This complements the Foundry Toolkit "tight IDE loop" story: with agent inspectors, evaluation-as-tests, and debugger participation, agents can be tested and observed with familiar software engineering workflows.
- [Stop Hunting Bugs: Meet the New Visual Studio Debugger Agent](https://devblogs.microsoft.com/visualstudio/stop-hunting-bugs-meet-the-new-visual-studio-debugger-agent/)
.NET agent framework work introduced a concrete "skills" composition pattern: file-based skills on disk, class-based skills via NuGet, and inline skills in-app unified behind an AgentSkillsProvider so agents can select capabilities without custom routing. This matches last week''s tool-contract and governed-automation focus: standardized packaging helps you version and audit what agents can do, especially when combined with approvals and allowlists. The post also stresses safeguards: script runners need sandboxing, limits, and audit, and execution can be gated with human approval (.UseScriptApproval(true)) and filtered via allowlists when loading shared directories.
- [Agent Skills in .NET: Three Ways to Author, One Provider to Run Them](https://devblogs.microsoft.com/agent-framework/agent-skills-in-net-three-ways-to-author-one-provider-to-run-them/)
An architecture overview contrasted single-agent (for example, Semantic Kernel orchestrator) with multi-agent (for example, AutoGen specialization), mapping ops implications to Azure building blocks like Service Bus coordination, AKS scaling for specialized agents, and Azure Monitor tracing. The advice is to start single-agent for MVPs and move to multi-agent when specialization and scaling benefits outweigh token cost and observability complexity. That matches last week''s day-two complexity lesson.
- [Single Agent vs Multi-Agent Architectures: When Do You Need Each? (Microsoft Stack)](https://dellenny.com/single-agent-vs-multi-agent-architectures-when-do-you-need-each-with-microsoft-technologies-explained/)
A serverless GPU template shows deploying Gemma 4 via Ollama on Azure Container Apps behind Nginx HTTPS + Basic Auth, exposing an OpenAI-compatible /v1/chat/completions endpoint for OpenAI-API-compatible tools (including OpenCode). This matches last week''s "cloud + local" and API-compatibility theme: compatibility reduces integration churn across managed endpoints, your containers, and on-device endpoints. The guide includes GPU fit (T4 vs A100), region notes, `azd up` provisioning, and cost control via scale-to-zero.
- [Gemma 4 on Azure Container Apps Serverless GPU](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/gemma-4-on-azure-container-apps-serverless-gpu/ba-p/4511671)
Fabric Real-Time Intelligence previews outlined "agentic ops" productization, echoing last week''s operational sequencing (observe -> decide -> automate with guardrails). Operations Agent setup describes a Teams-delivered, LLM-generated "Agent Playbook" running scheduled checks against KQL/Eventhouse, then recommending actions that may require approval and trigger Power Automate flows (three-day recommendation timeout). An expanded Eventhouse preview positions it as a shared KQL landing and analysis surface with a SQL endpoint, notebooks (Python/Spark), built-in anomaly detection, and data-agent integration to keep investigation and automation close to event and time-series data.
- [Microsoft Fabric Operations Agent: Step-by-step setup and runtime behavior](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/microsoft-fabric-operations-agent-step-by-step-walkthrough/ba-p/4512572)
- [One platform, many insights: How Eventhouse brings analytics together (Preview)](https://blog.fabric.microsoft.com/en-US/blog/one-data-many-insights-how-eventhouse-brings-analytics-together-preview/)
A Foundry case study (Agents League winner) provided a multi-agent reference: CertPrep uses Foundry Agent Service with GPT-4o JSON mode and Pydantic-validated contracts between specialized agents, plus concurrency, Azure Content Safety guardrails, and human-in-the-loop gates. It follows last week''s inspectable execution thread by surfacing intermediate artifacts (profiles, plans, readiness scoring, assessments, traces) in the UI for review and debugging rather than only producing a final response.
- [Agents League Winner Spotlight: CertPrep Multi-Agent System on Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-winner-spotlight-reasoning-agents-track/ba-p/4511211)',
    'This week''s AI news leaned into making agent development look more like normal software engineering: tighter IDE loops for building, debugging, and evaluating; clearer production hosting and orchestration options; and concrete patterns for connecting agents to governed data and automation. This continues last week''s "run it like software" framing where stable runtimes, inspectable tool contracts, and day-two controls (identity, policy, cost, evaluation) become the default rather than add-ons. Microsoft Foundry and Fabric also expanded platform capabilities with new models, fine-tuning options, MCP toolchains, and agent experiences that are easier to monitor and audit.',
    1776668400, 'ai', '/ai/roundups/weekly-ai-roundup-2026-04-20', 'TechHub',
    'TechHub', '07803DEFEFD85323D0FDEBE64932E8A2802CBDAB0C5C5A62B638A3E3968ABAFB', ',Microsoft Foundry,VS Code,Foundry Toolkit,Agent Service,MCP,Agent Governance,OpenTelemetry,Application Insights,Azure Container Apps,Azure App Service,Microsoft Fabric,OneLake,PostgreSQL,Oracle Database@Azure,Copilot Studio,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-04-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-04-13', 'roundups', 'Weekly AI Roundup: Agent Runtimes, MCP Auth, and Day-Two Ops',
    'This week''s AI updates pushed in two directions: more "agent runtime + tools + governance" building blocks reaching GA, and clearer paths to operationalize them (local models, MCP tool wiring with real auth, and agent-specific observability/grounding patterns that can work in production). It continues last week''s "run it like software" framing: stable runtimes, inspectable tool contracts, and day-two controls (cost, identity, evaluation, safety) becoming the default.
<!--excerpt_end-->
## Microsoft Foundry: GA agent runtime, SDK 2.0 shifts, and the “cloud + local” story
Foundry''s March update reads like consolidation. Foundry Agent Service is now GA and built on the OpenAI Responses API, which keeps agents wire-compatible while adding enterprise needs like private networking (BYO VNet with no public egress, subnet/container injection), Entra RBAC, tracing, and production-focused evaluations/monitoring. It builds on last week''s Agent Framework/Copilot Studio direction: orchestration and governance are moving toward platform capabilities (versionable, observable, enforceable) instead of being rebuilt per application.
For Azure AI Projects SDK users, workflows are increasingly centered on `AIProjectClient`, and SDKs moved to 2.0 stable across Python/JS/TS/Java (with .NET 2.0 shortly after on April 1). That also means migration work: packages and namespaces changed (for example, `AIProjectClient.OpenAI` -> `ProjectOpenAIClient` in .NET, plus renames in JS/TS and Java). Teams should plan the upgrade rather than treating it as a last-minute patch. This matches last week''s theme that stable contracts matter, and now the "contract" is often the SDK surface and Responses API shape.
On models and routing, Foundry''s catalog continues to expand. GPT-5.4 (GA) is positioned for production agent reliability (tool calling, fewer mid-run failures, better multi-step stability) with up to 272K context and cached-input pricing. GPT-5.4 Mini (GA) targets high-volume classification/extraction and lightweight tool calls. Additions like Phi-4 Reasoning Vision 15B and Fireworks AI integration for open models/BYOW broaden options for "right model per step" architectures. This fits last week''s cost/ops thread: once you route by step, provider choice and model tier become routine engineering settings.
Foundry Local reaching GA completes the "cloud + local" story. It keeps an OpenAI-compatible API while moving inference to device/edge. It follows last week''s Foundry Local offline blueprints (RAG vs CAG) by pairing local-first patterns with a supported GA runtime: OpenAI-compatible schemas, local caching/streaming, and acceleration selection. The flow is designed to be straightforward: add a thin SDK wrapper (JS/Python/.NET/Rust), build pulls in Foundry Local Core and ONNX Runtime native bits, first run downloads a hardware-optimized model from the catalog, then runs fully offline with caching and token streaming. It chooses acceleration automatically (GPU/NPU with CPU fallback), supports OpenAI-compatible chat completions and audio transcription (plus Open Responses API format), and can run without a local HTTP server unless you enable an OpenAI-compatible endpoint. Platform specifics include WinML on Windows (with OS-managed execution provider/plugin acquisition), Metal on Apple Silicon, and support for Linux/macOS/Windows.
Overall, Foundry''s message is less about one new feature and more about "these are the runtimes and SDK shapes to build on": cloud-hosted agents with production controls plus a local runtime that preserves message schemas and tool calling across deployments. It follows the same continuity thread as last week: standardize runtimes, keep tools and governance enforceable, and make local vs cloud a deployment choice rather than a rewrite.
- [What’s new in Microsoft Foundry | March 2026](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-mar-2026/)
- [Foundry Local is now Generally Available](https://devblogs.microsoft.com/foundry/foundry-local-ga/)
## MCP on Azure: from tool hosting to authentication patterns and self-hosted automation
MCP continued to take shape as the connection layer between agents and real systems, with progress focused on making MCP deployments look like supportable services rather than one-off demos. This builds on last week''s MCP "tooling glue" theme (Functions hosting, VS Code loops, governance-aware access). The emphasis is shifting from "you can host a tool" to "here is how you secure it, version it, and run it as a shared capability."
For UI-capable MCP "Apps" on Azure Functions, there are now two paths. The TypeScript quickstart shows the serverless workflow: define tools via `app.mcpTool()` and UI resources via `app.mcpResource()` (`text/html;profile=mcp-app`), run locally with `func start`, validate with MCP Inspector, and deploy with `azd` plus Bicep to `/runtime/webhooks/mcp`. In .NET, the Functions MCP extension added a fluent API (preview) to hide brittle protocol wiring between tools and UI. `AsMcpApp(...)` generates the synthetic UI resource function, sets the MIME type, and aligns `_meta.ui` bindings. It also moves security into code: explicit permissions (for example, clipboard read/write), CSP allowlisting, static asset hosting with source maps excluded by default, and visibility controls to keep UI renderable while hiding tools from model tool selection. This matches last week''s "remote HTTPS endpoints plus identity" intent, but it pushes safer defaults up into the framework so teams need less custom glue.
Once tools are hosted, safe calling becomes the next focus. The Foundry integration walkthrough focuses on Functions-hosted MCP servers as reusable backends: one server can serve IDE clients (VS Code, Visual Studio, Cursor, etc.) and Foundry agents using the same tool schema. The main value is the auth decision tree, continuing last week''s identity options but with concrete fields: function keys for shared-secret access, Entra managed identity when agents act as themselves (agent identity preferred for production; project managed identity often OK for dev), OAuth passthrough for per-user permissions and auditing, and unauthenticated only for public or dev cases. It also lists the specific Foundry fields (audience/App ID URI, tenant endpoints, scopes like `user_impersonation`) to reduce portal guesswork.
Azure MCP Server 2.0 reaching stable is the other key MCP update. It is an open-source MCP server that exposes Azure operations as structured tools (276 tools across 57 services), with 2.0 focusing on remote/self-hosted deployment. This matches last week''s "governed tool surfaces" direction. Instead of each dev running local tool defaults, you can run Azure MCP as a centrally managed internal service with consistent tenant/subscription defaults, telemetry policy, and network boundaries. It supports managed identity (with guidance for Foundry-adjacent setups) and an OBO pattern via OpenID Connect delegation to operate in the signed-in user''s context. The release also highlights hardening (endpoint validation, injection protections for query-style tools), container image improvements, and sovereign cloud support. These are day-two details that fit treating tools as production infrastructure.
For Entra-authenticated MCP servers (especially with a pre-authorized client like VS Code), the Entra guide is explicit. Since Entra does not support MCP''s CIMD/DCR flows today, the recommendation is pre-registration and pre-authorization: register the MCP server as a protected resource (API), define a delegated scope (for example, `user_impersonation`), configure `requested_access_token_version=2`, and add VS Code as a `pre_authorized_application` to avoid extra consent prompts. FastMCP validates JWTs using Entra public keys (no client secret needed for validation), and middleware captures the user `oid` so tools can enforce per-user auth/storage. For downstream calls (like Microsoft Graph) in user context, it shows the OBO flow, including admin consent and a managed-identity plus federated identity credential setup for Azure Container Apps. This is the identity glue implied by last week''s hosting patterns, now written out as a workable recipe.
- [Announcing Azure MCP Server 2.0 Stable Release for Self-Hosted Agentic Cloud Automation](https://devblogs.microsoft.com/azure-sdk/announcing-azure-mcp-server-2-0-stable-release/)
- [Give your Foundry Agent Custom Tools with MCP Servers on Azure Functions](https://devblogs.microsoft.com/azure-sdk/give-your-foundry-agent-custom-tools-with-mcp-servers-on-azure-functions/)
- [''MCP Apps on Azure Functions: Quickstart with TypeScript''](https://devblogs.microsoft.com/azure-sdk/mcp-apps-on-azure-functions-quickstart-with-typescript/)
- [''MCP as Easy as 1-2-3: Introducing the Fluent API for MCP Apps''](https://devblogs.microsoft.com/azure-sdk/mcp-as-easy-as-1-2-3-introducing-the-fluent-api-for-mcp-apps/)
- [Building MCP servers with Entra ID and pre-authorized clients](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-mcp-servers-with-entra-id-and-pre-authorized-clients/ba-p/4508453)
## Agent operations in practice: observability, real-time UIs, and guarded automation
As teams move from "an agent that works" to "an agent we can run," two themes stood out: visibility into agent behavior and explicit control points for risky actions. It continues last week''s operational thread (Agent Framework 1.0 plus Copilot Studio GA hooks, plus SRE Agent prerequisites/billing). Once agents touch real systems, you need traceability, approval gates, and cost/usage visibility built into the loop.
For observability, Application Insights'' new Agents view (preview) shows the platform adapting to agent-focused telemetry. The walkthrough instruments a .NET multi-agent "travel planner" on Azure App Service with OpenTelemetry GenAI semantic conventions so App Insights can answer agent-shaped questions: token usage per agent, per-agent latency/error rate, and end-to-end traces across an API plus WebJob split. The Agents view requires the right GenAI attributes (for example, `gen_ai.agent.name`). It demonstrates two instrumentation layers: `Microsoft.Extensions.AI` for LLM-call spans (token/model/provider attrs) and Microsoft Agent Framework (MAF) for agent-identity spans used for per-agent grouping. It also calls out a tradeoff: double instrumentation can duplicate spans, so you may choose identity grouping vs token detail depending on what you need most. This matches last week''s "inspectable and governable" stance: instrumentation is part of system design, not an afterthought.
For making behavior visible and controllable, the AG-UI plus MAF demo streams multi-agent execution events to a live frontend over SSE so users can see which agent is active, what step is running, and why it is waiting. This echoes last week''s streaming patterns in Foundry Local samples. Production agents need explicit progress/state signaling. The backend uses an explicit handoff graph (declared routing edges) and interrupts for user-info requests and human approval of sensitive tools. Marking tools with `approval_mode="always_require"` forces an interrupt instead of execution, and the React UI renders an approval modal with tool name/args before resuming. This "declared topology plus interruptible tools plus real-time events" matches last week''s versioned orchestration and moderated tool integration, expressed as runtime plus UX.
For day-two ops, the Well-Architected operational excellence discussion reinforced a practical sequence: start with observability (OpenTelemetry), then use AI to summarize incidents and suggest next steps, and only move toward automation once guardrails, evaluation, and human-in-the-loop controls exist for high-impact actions. This aligns with last week''s SRE Agent framing: the constraint is not whether an agent can act, but whether you can prove what happened, limit blast radius, and sustain it on-call.
- [Monitor AI Agents on App Service with OpenTelemetry and the New Application Insights Agents View](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/monitor-ai-agents-on-app-service-with-opentelemetry-and-the-new/ba-p/4510023)
- [Building a Real-Time Multi-Agent UI with AG-UI and Microsoft Agent Framework Workflows](https://devblogs.microsoft.com/agent-framework/ag-ui-multi-agent-workflow-demo/)
- [Use AI to Achieve Operational Excellence with the Well-Architected Framework practices](https://www.youtube.com/watch?v=PRpYptDTe4o)
## Other AI News
Data and analytics teams got two options for making data easier to use with agents without adding a separate AI pipeline layer. Fabric Data Warehouse introduced built-in AI functions (preview) that run directly in T-SQL for JSON extraction (`ai_extract`), sentiment (`ai_analyze_sentiment`), classification (`ai_classify`), summarization/translation/grammar fixes, and a prompt-based escape hatch (`ai_generate_response`) that you can wrap in UDFs/stored procedures to standardize prompts. This extends last week''s Fabric "trusted data for AI" story by moving agent-enablement into the governed warehouse surface where permissions, lineage, and operational controls already exist.
A related "intelligence platforms" guide described an enterprise agent architecture: unify access with OneLake, enforce meaning/permissions with Fabric semantic models (measures/RLS), expose governed NL querying via Fabric Data Agents (preview), and connect that to Azure AI Foundry agents as a reusable tool, optionally enriched with Microsoft Graph context. It continues last week''s governance-and-metadata theme: grounding tends to be more reliable when it is a permission-aware tool call over curated semantics, rather than copied text in prompts.
- [Working with unstructured text in Fabric Data Warehouse with built-in AI functions (Preview)](https://blog.fabric.microsoft.com/en-US/blog/working-with-unstructured-text-in-fabric-data-warehouse-with-built-in-ai-functions-preview/)
- [Why data platforms must become intelligence platforms for AI agents (with Microsoft Fabric + Azure AI Foundry)](https://techcommunity.microsoft.com/t5/microsoft-developer-community/why-data-platforms-must-become-intelligence-platforms-for-ai/ba-p/4505653)
Legacy modernization got a concrete agent example. An IIS migration guide showed using an MCP server to orchestrate Microsoft''s IIS-to-App-Service migration scripts with human approvals, producing artifacts like `install.ps1`, adapter ARM templates, and `MigrationSettings.json`. It highlights Managed Instance on App Service specifics (PremiumV4 with `IsCustomMode=true`, plus OS dependencies like COM, MSMQ/SMTP, registry, drive-letter storage). This is a practical "MCP as governed automation interface" pattern: wrap existing scripts as typed tools, host them remotely, and require explicit approval before provisioning billable resources. That is similar to last week''s remote-only MCP endpoints and identity boundaries.
- [Agentic IIS Migration to Managed Instance on Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/agentic-iis-migration-to-managed-instance-on-azure-app-service/ba-p/4508969)
On developer workflow, a short Cozy AI Kitchen episode showed design-to-code MCP: wiring a Figma MCP server into VS Code so design artifacts become structured context. The practical goal is fewer handoff cycles by referencing the real source of truth (Figma components/layout metadata) instead of text descriptions, reinforcing the wider shift across these roundups toward tools over prompts.
- [Setting Up Figma MCP Server in VS Code](https://www.youtube.com/shorts/noehsI6cAEc)',
    'This week''s AI updates pushed in two directions: more "agent runtime + tools + governance" building blocks reaching GA, and clearer paths to operationalize them (local models, MCP tool wiring with real auth, and agent-specific observability/grounding patterns that can work in production). It continues last week''s "run it like software" framing: stable runtimes, inspectable tool contracts, and day-two controls (cost, identity, evaluation, safety) becoming the default.',
    1776063600, 'ai', '/ai/roundups/weekly-ai-roundup-2026-04-13', 'TechHub',
    'TechHub', '0B491EE941CEF0FA103B9D0DEA1879F2B3CAA9ED75204DB609E3ECA4EC8C02BF', ',Azure AI Foundry,Foundry Agent Service,Foundry Local,OpenAI Responses API,GPT 5.4,Phi 4,MCP,Azure Functions,Microsoft Entra ID,Managed Identity,OAuth 2.0,OpenTelemetry,Application Insights,Microsoft Fabric,Agent Observability,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-04-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-04-06', 'roundups', 'Weekly AI Roundup: Offline Agents, Orchestration, and Controls',
    'This week’s AI updates were less about new model behavior and more about making agent systems workable: running locally, standardizing orchestration across languages, and tightening operational controls (tools, governance, cost) so systems hold up in production. It continues last week’s "run it like software" direction (repeatable workflows, inspectable grounding, and day-two controls), with more emphasis on building blocks you can ship: offline templates, stable multi-agent runtimes, and governable tool-integration patterns.
<!--excerpt_end-->
## Offline, On-Device Agents with Foundry Local (RAG vs CAG)
Two local-first assistant blueprints built around Microsoft Foundry Local and `foundry-local-sdk` show how to run entirely on one machine with no API keys and no network after the initial model download. This builds on last week’s Foundry Local thread (OpenAI-compatible local endpoints, stable client code while endpoints swap, lightweight grounding) by showing two concrete app shapes that fit internal tools or offline/field use.
Both samples keep the app intentionally small: Node.js 20+ with Express, a single-page UI, and Server-Sent Events (SSE) used twice. First, SSE streams model download/load status until "Offline Ready." Then it streams tokens into chat. That "operate the loop" approach (status streaming, predictable startup, explicit offline readiness) lines up with last week’s idea that local runtimes should be operable systems, not just demos.
They differ mainly in how grounding works. The CAG version is startup-loaded and straightforward: preload Markdown docs from `docs/`, score documents with keyword scoring (no embeddings/chunking/vector DB), and inject the top docs into the prompt. The trade-offs are explicit: it is limited by the context window, best for "tens of documents," and KB updates require a restart. It also includes practical model selection: filter the local catalog by capability (chat-completion) and a RAM budget policy (for example, "60% of system RAM"), then pick models like `phi-4` on 32 GB or `phi-3.5-mini` on 8 GB, download if needed, load, and run completions in-process. This keeps last week’s "predictable endpoint swaps" idea but adds guidance for "what runs on this machine."
The RAG version adds more components for scale and hot updates, which echoes last week’s point that grounding should be reusable and testable. It chunks Markdown into ~200-token segments with overlap, stores chunks and TF-IDF vectors in a single-file SQLite DB (`better-sqlite3`), and retrieves using TF-IDF + cosine similarity, explicitly avoiding embeddings to stay offline and lightweight. Retrieval is optimized with an inverted index, prepared statements, and caching, and the author reports sub-millisecond retrieval for the target workload. The prompt contract is also strict: safety-first behavior, bans on guessing for procedures/tolerances, a required "This information is not available in the local knowledge base" response when grounding is insufficient, and a structured output format (summary, safety warnings, steps, references) with UI-visible citations and relevance scores. It also supports runtime doc upload (`.md`/`.txt`) with immediate chunk/vector/index updates without restart, which is where the extra RAG complexity pays off.
Both posts include setup details (`winget install Microsoft.FoundryLocal`, model sizes like ~2 GB for Phi-3.5 Mini, `npm test` via Node’s built-in test runner) and close with extension paths such as hybrid retrieval (TF-IDF + embeddings), persisted memory, multimodal input, and PWA packaging for offline install, which matches last week’s "start simple, stay inspectable" direction.
- [Building Your First Local RAG Application with Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-your-first-local-rag-application-with-foundry-local/ba-p/4501968)
- [Build a Fully Offline AI App with Foundry Local and CAG](https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-a-fully-offline-ai-app-with-foundry-local-and-cag/ba-p/4502124)
## Agent Orchestration Goes Production-Ready (Microsoft Agent Framework 1.0 + Copilot Studio Multi-Agent GA)
Microsoft moved multi-agent development toward more stable foundations in two places: Agent Framework 1.0 for developers and Copilot Studio multi-agent GA for makers/developers. This follows last week’s platform-choice framing (Copilot Studio vs Azure AI Agents vs Foundry) by translating "production-ready" into stable APIs across languages, reviewable configs, and evaluation/moderation hooks that fit CI/CD.
Microsoft Agent Framework 1.0 is out for .NET and Python with stable APIs and an LTS/backward-compatibility commitment, positioned as a convergence of Semantic Kernel foundations and AutoGen orchestration patterns. The core value is standardization: build single- or multi-agent systems with the same abstractions in both runtimes, and swap providers via connectors (Foundry, Azure OpenAI, OpenAI, Anthropic, Bedrock, Gemini, Ollama). That matches last week’s theme of keeping app/orchestration contracts stable while endpoints evolve (similar to Foundry Local’s endpoint swap story). It includes core building blocks teams need early: tools/functions, multi-turn session management, and streaming. For orchestration, it provides a graph workflow engine (branch/fan-out/converge), checkpointing/hydration for long-running flows, and patterns like sequential, concurrent, handoff, group chat, and Magentic-One, plus middleware hooks for policy, observability, and compliance logic. Memory is pluggable (history, persistent KV, vector retrieval) with backends like Foundry Agent Service memory, Mem0, Redis, Neo4j, and custom providers. It also introduces YAML-defined agents/workflows (instructions, tools, memory, topology) that can be version-controlled and promoted, which lines up with last week’s repo-first operating model.
Copilot Studio’s multi-agent orchestration is rolling into GA over the next few weeks (targeting full availability for eligible customers by April 2026). It extends last week’s "hybrid approach" framing (Copilot Studio for controlled experiences, programmable layers behind it) into multi-agent coordination. The GA scope emphasizes connected experiences: Fabric integration (Copilot Studio agents coordinate with Fabric agents), orchestration with the Microsoft 365 Agents SDK (reuse retrieval/actions across Microsoft 365 and Copilot Studio), and Agent-to-Agent (A2A) communication via an open protocol for delegating to other agents. Prompt Builder is now GA and integrated into the Tools tab for iterating instructions/models/inputs/knowledge in one place, and prompt-level content moderation controls are GA (supported regions) for managed models, which can help where default filters block legitimate regulated terms. Evaluation automation APIs are GA via Power Platform APIs/connectors for CI/CD gating against regression scores, and connectors like ServiceNow and Azure DevOps are called out as improved to better support operational grounding.
The shared direction is multi-agent work as engineering: stable runtimes (.NET/Python), checkpointed workflow graphs, versioned YAML orchestration, and platform features that make prompt iteration, moderation, and automated evaluation part of regular releases.
- [Microsoft Agent Framework Version 1.0](https://devblogs.microsoft.com/agent-framework/microsoft-agent-framework-version-1-0/)
- [What’s new in Copilot Studio: Updates to multi-agent systems](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/new-and-improved-multi-agent-orchestration-connected-experiences-and-faster-prompt-iteration/)
## MCP as the Tooling Glue (VS Code, Azure Functions, and Governed Data/Metadata Access)
MCP kept showing up as the tool layer that makes agents reusable across products: custom tools in Foundry agents, local development in VS Code, and governed metadata access for data copilots. This continues last week’s MCP storyline (maturity, hosted endpoints, identity-aware access, deterministic tool surfaces), with more emphasis on hosting, auth, and integration with governed systems.
In Azure AI Foundry, the practical pattern is to host an MCP server remotely on Azure Functions, then register that endpoint as a tool in Foundry so agents can discover/invoke it from the Agent Builder Playground. Azure Functions is positioned as the default host because it fits tool workloads (serverless scaling, consumption billing, multiple auth models). The post lays out identity choices teams need to decide early, following last week’s "tool calls need boundaries" theme: key-based auth (simple for dev), Entra ID + managed identity (recommended for production service-to-service calls using the Foundry project managed identity), OAuth identity passthrough (tool calls under each end user identity), and unauthenticated access (dev/public tools only). It also gives a concrete endpoint format for MCP extension-based Functions: `https://<FUNCTION_APP_NAME>.azurewebsites.net/runtime/webhooks/mcp`. The reuse point is explicit: MCP servers built for VS Code/Visual Studio/Cursor can be reused in Foundry without rebuilding integrations.
For local development, a VS Code video walkthrough shows end-to-end MCP server development with Python and FastMCP, including client/server responsibilities, tool discovery and invocation, and STDIO transport (server as a local process over stdin/stdout). This reinforces the schema discipline point: MCP tool schemas enable cross-client discovery, and transport can be local for dev even if production moves to remote HTTPS for governance and networking.
MCP also appears in the Fabric/Purview governance story as a way to expose metadata and governance-aware capabilities to AI agents without bypassing permissions. This aligns with last week’s Fabric direction (semantics, permission-aware context, MCP endpoints on the roadmap): instead of copying catalog/lineage/classification into prompts, you expose controlled tools that enforce Fabric/Purview rules. It is paired with API governance updates (OneLake Catalog Search API GA and Bulk Import/Export of Item Definitions preview) so teams can automate metadata operations instead of relying on UI-heavy workflows.
- [Give your Foundry Agent Custom Tools with MCP Servers on Azure Functions](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/give-your-foundry-agent-custom-tools-with-mcp-servers-on-azure/ba-p/4507828)
- [Create and install an F1 inspired MCP Server in VS Code](https://www.youtube.com/watch?v=ZPaF_6mSp8I)
- [Modern data governance in Fabric: How Purview and AI transform data governance](https://zure.com/blog/whats-new-in-microsoft-fabric-for-data-governance-and-metadata-management-march-2026)
## Azure SRE Agent: Provider Choice, Prerequisites, and a Shift to Token-Based Billing
Azure SRE Agent added operational details that shape safe, sustainable on-call usage: prerequisites, integrations, model provider choice, and billing. This builds on last week’s "external system -> managed identity bridge -> SRE Agent trigger" patterns and cost guardrails by adding rollout constraints, network realities, and cost units that map directly to usage.
One post focuses on preview onboarding prerequisites and infrastructure scenarios, framing the agent as an AI reliability operator that observes Azure telemetry (Azure Monitor, Log Analytics, Application Insights) and Azure service APIs, then helps with incident investigation, correlation, RCA, and optional controlled remediation. Teams can run in recommendation/review mode or enable autonomous execution for pre-approved steps with guardrails, approvals, and specialized subagents (VMs/databases/networking). The actionable content is the checklist: the preview control plane must be created in Sweden Central, Australia East, or US East 2 (monitored workloads can be elsewhere), subscriptions may need allow-listing, and identity/RBAC is the core dependency, often elevated for onboarding and then tightened to least privilege for the managed identity (read for investigation, scoped write for approved remediation). It also calls out integration edges: outbound HTTPS to Azure management endpoints and any third-party systems/MCP servers (custom MCP endpoints must be remote HTTPS, not local endpoints), no guaranteed static egress IPs for firewall allow lists, and allowing domains like `*.azuresre.ai`. Integrations include ServiceNow/PagerDuty, GitHub/Azure DevOps, Grafana, and Azure Data Explorer (Kusto). The "remote-only tool endpoints + identity boundaries" constraint matches the MCP hosting patterns showing up elsewhere.
Two updates moved the product toward more flexible operations and clearer cost planning. First, SRE Agent now supports multiple model providers, adding Anthropic with Claude Opus 4.6 as the baseline when selected, which fits this week’s provider-abstraction theme (Agent Framework). Second, active flow billing shifts from time-based to token-based metering effective April 15, 2026. The unit remains Azure Agent Units (AAUs): always-on flow stays 4 AAUs per agent-hour; active flow becomes "AAUs per million tokens" with rates varying by provider. This ties cost to investigation depth (conversation length, correlated telemetry breadth) and makes provider choice part of cost planning. Monthly AAU allocation limits (Settings -> Agent consumption) remain the key guardrail: when you hit the active flow limit, chat/autonomous actions pause until next month, while always-on continues, which matches last week’s cost-control approach.
- [From Toil to Trust: Azure SRE Agent prerequisites, integrations, and infra scenarios](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/from-toil-to-trust-how-azure-sre-agent-is-redefining-cloud/ba-p/4505875)
- [Azure SRE Agent now supports multiple model providers, including Anthropic Claude](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-now-supports-multiple-model-providers-including/ba-p/4508111)
- [An update to the active flow billing model for Azure SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/an-update-to-the-active-flow-billing-model-for-azure-sre-agent/ba-p/4507866)
## Other AI News
Foundry’s model catalog keeps expanding beyond chat into modality-specific building blocks. This follows last week’s "voice as an operational modality" thread by adding first-party primitives in Foundry so teams can build voice and image features without immediately using third-party hosting.
Microsoft announced MAI models in Azure AI Foundry: MAI-Transcribe-1 (speech-to-text, 25 languages), MAI-Voice-1 (text-to-speech), and MAI-Image-2 (image generation). The goal is first-party options in Foundry’s catalog, with details like parameters/pricing/regions expected in the linked build surfaces rather than the announcement.
- [We’re bringing our growing MAI model family to every developer in Foundry, including: MAI-Transcribe-1, most accurate transcription model in world across 25 languages; MAI-Voice-1, natural, expressive speech generation; MAI-Image-2, our most capable image model yet. Start building…](https://www.linkedin.com/posts/satyanadella_were-bringing-our-growing-mai-model-family-activity-7445475747680411650-T_tO)
Teams working on "custom model in production" got a concrete BYOM walkthrough using Azure Machine Learning as the hosting and governance boundary. It applies the same "treat it like software" posture (reproducible environments, managed endpoints, identity-first access) to teams running their own models instead of using a hosted catalog.
It covers registering a model (example: SmolLM-135M), defining a reproducible conda environment (Python 3.12, `azureml-inference-server-http`, PyTorch/Transformers), deploying to Managed Online Endpoints, and using Entra ID auth via `DefaultAzureCredential`/`MLClient` to avoid secrets. It also notes an alternative scoring shape for token-rank analysis when you need introspection rather than generation.
- [Bring Your Own Model (BYOM) for Azure AI Applications using Azure Machine Learning](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bring-your-own-model-byom-for-azure-ai-applications-using-azure/ba-p/4508211)
Database-centric AI patterns also got attention, with Azure SQL Managed Instance framed as an AI-enabled PaaS platform. This complements last week’s "governed grounding" and "data readiness" themes with an operational stance: keep retrieval and scoring close to the data boundary.
It highlights native vector types and distance functions for semantic search/RAG, in-database Python/R via Machine Learning Services for training/scoring near governed data, and Copilot-assisted diagnostics via Query Store/DMVs (plus Copilot in SSMS when connected to MI). For teams that want AI to stay inside operational data boundaries, it is a clear "do more where the data lives" path.
- [Azure SQL Managed Instance as an AI-Enabled PaaS Platform](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/azure-sql-managed-instance-as-an-ai-enabled-paas-platform/ba-p/4508380)
Two different takes on AI costs extended last week’s cost-guardrails storyline into broader engineering practice. One focuses on tactics such as token mechanics, caching, and routing, while the other covers the full cost of agentic systems, including human review and governance overhead once agents become part of delivery and operations.
- [Cost Optimization for Copilot and AI Agents on Azure](https://dellenny.com/cost-optimization-for-copilot-and-ai-agents-on-azure/)
- [On .NET Live - AI offers benefits, but at what cost?](https://www.youtube.com/watch?v=yO_TH3R8KMw)
Fabric’s "AI depends on trusted data" story continued with two governance-adjacent items. This matches last week’s Fabric focus on reusable business context (events, ontology, graph reasoning, de-identification) by showing more upstream pipeline work: master data curation and governance/metadata APIs that copilots rely on for permission-aware operation.
- [How Stibo Systems’ MDM powers trusted data for analytics and AI in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/how-stibo-systems-mdm-powers-trusted-data-for-analytics-and-ai-in-microsoft-fabric-preview/)
- [Modern data governance in Fabric: How Purview and AI transform data governance](https://zure.com/blog/whats-new-in-microsoft-fabric-for-data-governance-and-metadata-management-march-2026)
For reference architectures, the March 2026 Innovation Challenge recap points to hackathon projects that mirror production needs: NLQ-to-analytics flows, multi-agent analytics orchestration (including DuckDB pipelines and Microsoft 365 ingestion), and validation layers that enforce structure and block unsafe actions. That validation focus matches last week’s local-agent patterns and this week’s offline RAG prompt contracts: different stacks, similar engineering priorities.
- [The March 2026 Innovation Challenge Winners](https://techcommunity.microsoft.com/t5/azure/the-march-2026-innovation-challenge-winners/m-p/4508498#M22477)',
    'This week’s AI updates were less about new model behavior and more about making agent systems workable: running locally, standardizing orchestration across languages, and tightening operational controls (tools, governance, cost) so systems hold up in production. It continues last week’s "run it like software" direction (repeatable workflows, inspectable grounding, and day-two controls), with more emphasis on building blocks you can ship: offline templates, stable multi-agent runtimes, and governable tool-integration patterns.',
    1775458800, 'ai', '/ai/roundups/weekly-ai-roundup-2026-04-06', 'TechHub',
    'TechHub', 'BCAC0FB2F5D1850B4555D54F05EE24DE76D218DC4A58562A3EFC54D0C22A3A07', ',AI Agents,Microsoft Foundry Local,RAG,CAG,TF IDF,SQLite,Server Sent Events,Microsoft Agent Framework,Copilot Studio,Multi Agent Orchestration,MCP,Azure Functions,Governance,Azure SRE Agent,Cost Optimization,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-03-30
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-03-30', 'roundups', 'Weekly AI Roundup: IaC Agents, Local Endpoints, Governed Context',
    'This week''s AI updates tracked two parallel themes: shipping agents into production with repeatable workflows and governance, and adopting more local-first, inspectable patterns for building and operating AI systems. Across Azure AI Foundry, Foundry Local, and Microsoft Fabric, the common thread was making agent behavior easier to deploy, ground, observe, and control via IaC scaffolding, structured tool plans, ontology/graph grounding, and cost guardrails. This continues last week''s "run it like software" arc: last week delivered GA runtimes, private networking, managed identity, evaluation hooks, and MCP tooling glue; this week shows how teams ship and operate those ideas (IaC-first delivery, offline OpenAI-style endpoints, and more traceable retrieval/reasoning).
<!--excerpt_end-->
## Azure AI Foundry & Foundry Local: agent delivery from cloud endpoints to offline workflows
Foundry updates covered both ends of deployment: "code-to-cloud" publishing of agent endpoints and offline/on-device systems that still look like OpenAI-compatible apps. If last week was about the agent runtime being ready for production (GA runtime, private networking, evaluation, managed identities), this week focused on making that posture reproducible from a repo and extending "treat it like a service" to local endpoints.
On the cloud side, the Azure Developer CLI added a direct path from a Python agent repo to a live Azure AI Foundry agent endpoint via the `azd ai agent` extension. Building on last week''s `azd ai agent run`/`invoke`, it now covers deployment with infra/identity defaults that match the governance patterns we''ve been tracking (managed identity + RBAC, scripted flows for CI/CD). The workflow is intentionally opinionated: `azd ai agent init` scaffolds Bicep IaC (notably `infra/main.bicep`), `azure.yaml`, and `agent.yaml` for metadata/env vars. Then `azd up` provisions Foundry, deploys a model config (GPT-4o example), configures managed identity + RBAC, and publishes the endpoint with a portal link for playground validation. The inner loop includes `azd ai agent invoke` (multi-turn), `azd ai agent run` (local execution through the same flow), `azd ai agent monitor`/`--follow` for logs, and `azd down` for cost cleanup. The optional chat frontend wiring highlights using `azd env get-values`/`set` to keep app<->agent connections repeatable across environments and CI/CD (for example, GitHub Actions running `azd up` on `main`), complementing last week''s focus on repeatable evaluation/monitoring loops.
Foundry Local also matured as the offline counterpart, with examples that treat "local LLM runtime" as a dependency you operate rather than a demo shortcut. The OpenAI-compatible endpoint detail continues last week''s wire-compatibility thread: whether you use cloud Foundry or Foundry Local, client code can stay stable while you swap endpoints/environments.
One guide shows a multi-agent robotics automation pipeline that keeps the LLM away from direct simulator control using a constrained contract: "LLM -> strict JSON plan -> safety validation -> executor." This matches last week''s themes (structured outputs, approvals, least-privilege tools) but in a local control loop where safety/determinism matter more. Foundry Local exposes an OpenAI-compatible endpoint, so the main client change is the base URL; the example uses `FoundryLocalManager` and models like `qwen2.5-coder-0.5b` with automatic backend selection (CUDA GPU -> QNN NPU -> CPU). Agents are split cleanly: PlannerAgent emits JSON tool calls, SafetyAgent validates schema/bounds in sub-millisecond time, and ExecutorAgent runs PyBullet behaviors (IK movement, pick/place, scene description). It also includes offline voice commands (browser MediaRecorder, 16kHz mono WAV resampling, server-side ONNX Whisper with caching/chunking) feeding the same flow, which is useful for hands-free or low-latency local control. Compared with last week''s Foundry Voice Live, it''s a contrast between cloud real-time voice and local capture/transcription, with the same need for traceable plans and safety gates. Model timing comparisons (sub-5s on the smallest vs ~35-45s on larger ones) make the interactive tradeoffs concrete.
A second Foundry Local tutorial applies the OpenAI-compatible runtime to an offline RAG assistant ("Interview Doctor"), using a deliberately lightweight retrieval approach. Instead of embeddings, it chunks docs (~200 tokens + overlap), stores term-frequency vectors in SQLite (`sql.js`), and retrieves via cosine similarity, positioned as ~1ms for small corpora (CV + job descriptions) without running an embedding model alongside the LLM. This pairs with last week''s Foundry IQ direction (permission-aware retrieval as a standard tool surface): different stack, same goal of grounding as a reusable, testable component. The app is Node/Express with a single-file web UI streaming via SSE plus a CLI, and it notes an operational gotcha: Foundry Local uses a dynamic port, so use the SDK-discovered endpoint (`manager.endpoint`) instead of hardcoding `localhost`. It also demonstrates testability using `node:test` so core logic can be validated without the local runtime running, another "run it like software" signal applied to offline builds.
- [From code to cloud: Deploy an AI agent to Microsoft Foundry in minutes with azd](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-end-to-end/)
- [Building real-world AI automation with Foundry Local and the Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-real-world-ai-automation-with-foundry-local-and-the/ba-p/4501898)
- [Building an Offline AI Interview Coach with Foundry Local, RAG, and SQLite](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-an-offline-ai-interview-coach-with-foundry-local-rag/ba-p/4500614)
## Microsoft Fabric for AI agents: governed events, ontology context, and inspectable graph reasoning (Previews + GA)
Fabric''s AI updates focused on making real-time signals and business context reusable and governable, so agents, automation, and analytics can share definitions across notebooks, pipelines, and dashboards. It continues last week''s Fabric direction: put AI work into shared, governed surfaces (real-time intelligence + IQ context + semantics) instead of isolating it in one notebook or app.
Business Events in Fabric (preview) add a business-level event layer in Real-Time Hub. Instead of raw telemetry sent to tightly coupled consumers, teams define business event types with governed schemas via Schema Registry/Schema Sets, then emit events from Fabric compute (Notebooks and User Data Functions are called out). This extends last week''s "Observe -> Analyze -> Decide -> Act" loop by making "observe" a versioned contract: less ad-hoc plumbing, more reusable signals downstream systems (including agents) can trust. The value is decoupled fan-out: one Business Event can drive Activator actions, Power Automate, Notebooks, Spark jobs, Dataflows, and AI/ML enrichment without the publisher knowing consumers. The manufacturing example (anomaly -> "CriticalVibrationDetected" -> safe-mode + ticket + root-cause notebook) illustrates reducing glue code while keeping schemas consistent.
For agent context, Fabric IQ Ontology (preview) positions ontology items as operational context: mapping entities/relationships to OneLake data and events so agents do not rely on inconsistent definitions. This builds on last week''s Fabric IQ/ontology push: centralize semantics for humans and agents. The roadmap adds embedded rules/actions (via Activator), Fabric-aligned permissions/sharing (Read/Edit/Reshare), and tenant/workspace Azure Private Link hardening, mirroring last week''s move toward private networking and governed access across Foundry/Fabric. It also points to interoperability via upcoming "Ontology MCP endpoints," exposing ontology context through public MCP endpoints so external MCP-capable agents can retrieve the same grounded business context. Given last week''s MCP endpoint momentum, this is another step toward "business context as a standard tool surface," not copied into prompts.
Fabric also previewed "graph-powered AI reasoning," combining Fabric Data Agent with Fabric Graph for more inspectable answers via deterministic traversal ("graph RAG"). This matches last week''s evaluation/traceability emphasis but constrains the reasoning path: translate natural language to validated GQL via NL2GQL, run deterministic graph traversals, and expose a GQL trace so users can review which relationships produced the answer. The Adventure Works example highlights recommendation logic that can be awkward in SQL but explicit in a graph (including derived nodes like country) and queryable with traceable outputs, which is useful when you need an auditable reasoning path rather than only probabilistic text.
Finally, Fabric''s Workload Hub delivered an AI data-readiness GA: Tonic Textual is now generally available for scanning unstructured OneLake text for sensitive entities and applying transformations (redaction, masking, synthetic replacement, custom rules), writing results back to a separate OneLake location. This aligns with last week''s "production RAG is access control and repeatability" theme: data needs a standardized privacy/prep step before retrieval/eval pipelines become trustworthy. The practical benefit is OneLake-to-OneLake de-identification that preserves structure (dialogue, contracts) instead of exporting to external tools.
- [Business Events in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/business-events-in-microsoft-fabric-preview/)
- [What’s next for Fabric IQ Ontology: The operational context that powers your AI agents (Preview)](https://blog.fabric.microsoft.com/en-US/blog/whats-next-for-fabric-iq-ontology-the-operational-context-that-powers-your-ai-agents-preview/)
- [Graph-powered AI reasoning (Preview)](https://blog.fabric.microsoft.com/en-US/blog/graph-powered-ai-reasoning-preview/)
- [Preparing unstructured data in Microsoft Fabric with Tonic Textual (GA)](https://blog.fabric.microsoft.com/en-US/blog/from-restricted-to-ai-ready-preparing-unstructured-data-directly-in-microsoft-fabric-with-tonic-textual-generally-available/)
## MCP and agent platform choices: standardizing tool/context access and picking the right builder
A recurring architecture thread this week was how agents get tools and context, and how teams choose an agent-building surface as requirements grow. Last week framed MCP as increasingly operational (remote MCP servers in Foundry, managed Grafana MCP endpoints, and a .NET SDK). This week continued that theme with protocol maturity and tool-surface examples beyond typical enterprise apps.
Two MCP pieces reinforce the momentum. GitHub''s Universe video explains MCP as a standardized contract for exposing tools/data to agents, especially private or new information that is not in training data, and notes the official open-source MCP server was rewritten from TypeScript to Go, which changes deployment and contribution details. That kind of reference shift suggests MCP is moving from experimentation into maintainability (runtime footprint, deployment model, contributor workflow), matching last week''s shift toward hosted, identity-aware endpoints. Unity-MCP shows MCP-style structured calls in a game engine where "context" is editor/project state (scenes, GameObjects, assets, components), giving AI a more deterministic surface than text alone.
Two "which platform should I use?" pieces also landed, making explicit what tends to trigger migration. When governance, evaluation, observability, and custom tool/knowledge wiring matter, teams often move from simpler builders toward Foundry/Azure AI Agents-style surfaces (often keeping a separate interaction layer). One compares Copilot Studio vs Azure AI Agents: low-code connectors and predictable pricing for well-defined assistants vs developer-built, consumption-based systems with model choice, RAG, orchestration, evaluation, and observability. The other provides a broader framework across Agent Builder, Copilot Studio, and Azure AI Foundry, emphasizing criteria that drive migration: complexity, model flexibility, deployment targets, lifecycle ops (eval/observability), safety/guardrails, memory/state, tool/knowledge integration, and cost control. Together they reinforce a hybrid approach: Copilot Studio for UI/flows, backed by a programmable Foundry/Azure AI Agents layer for intelligence and governance, consistent with last week''s standardization on MCP and operational loops under multiple app surfaces.
- [What is MCP and how does it work with AI?](https://www.youtube.com/watch?v=lFQz1hugvHo)
- [Open Source Friday with Unity-MCP](https://www.youtube.com/watch?v=Ng5ltWrSG0M)
- [Copilot Studio vs Azure AI Agents: What Should You Use?](https://dellenny.com/copilot-studio-vs-azure-ai-agents-what-should-you-use/)
- [Picking the right Agent Builder solution](https://www.youtube.com/watch?v=WUgujz0y1K4)
## Other AI News
Cost and operations management showed up as platform guardrails and incident automation, connecting to last week''s "day-two" focus (evaluation, observability, private networking, identity). One guide shows an Azure-native spend control loop for Azure OpenAI: Cost Management Budgets trigger Action Groups, which run Azure Automation PowerShell to disable local auth (`Set-AzCognitiveServicesAccount -DisableLocalAuth $true`) when thresholds are hit, with a separate manual runbook to re-enable after review. Separately, Azure SRE Agent HTTP Triggers shows starting an automated investigation from Jira using an Azure Logic App as a Managed Identity auth bridge (the trigger endpoint is Entra-protected and uses SRE Agent data-plane RBAC). The pattern (external system -> Managed Identity bridge -> SRE Agent trigger) keeps credentials out of Jira while preserving audit history, and uses a Jira MCP connector (`mcp-atlassian` 2.0.0 over STDIO). In the context of last week''s MCP identity modes and managed endpoints, it''s another example of pairing agent actions with identity boundaries and auditability.
- [Automating Azure OpenAI Cost Control Using Budgets, Action Groups, and Automation Runbooks](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/automating-azure-openai-cost-control-using-budgets-action-groups/ba-p/4505164)
- [HTTP Triggers in Azure SRE Agent: From Jira Ticket to Automated Investigation](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/http-triggers-in-azure-sre-agent-from-jira-ticket-to-automated/ba-p/4504960)
.NET developers got an updated learning path: "Generative AI for Beginners .NET" v2 is rebuilt on .NET 10, switches foundational model calling from Semantic Kernel to Microsoft.Extensions.AI (`IChatClient` + middleware pipeline), and standardizes auth with `AzureCliCredential` so you can log in once via Azure CLI instead of distributing keys. This aligns with last week''s managed identity/consistent auth defaults and complements last week''s MCP/.NET tooling by clarifying the baseline stack before heavier orchestration. RAG content is reworked toward native SDKs, and the agent module uses Microsoft Agent Framework (RC), keeping orchestration as a dedicated topic rather than the default entry point.
- [Generative AI for Beginners .NET: Version 2 on .NET 10](https://devblogs.microsoft.com/dotnet/generative-ai-for-beginners-dotnet-version-2-on-dotnet-10/)
RAG patterns continued to diversify beyond embeddings, echoing last week''s themes of governed grounding and traceability. A "vectorless reasoning-based RAG" tutorial uses PageIndex to build a hierarchical document tree for long PDFs, then has an LLM select relevant nodes (pages/sections) via strict JSON before answering only from retrieved text. The goal is fewer moving parts than embeddings + vector DB and better traceability back to page indices and node IDs.
- [Vectorless Reasoning-Based RAG: A New Approach to Retrieval-Augmented Generation](https://techcommunity.microsoft.com/t5/microsoft-developer-community/vectorless-reasoning-based-rag-a-new-approach-to-retrieval/ba-p/4502238)
Foundry Labs introduced a "scout -> evaluate -> graduate" workflow: try early-stage model/agent experiments (30+ projects) with clear maturity expectations, then move promising work into Azure AI Foundry where evaluation, tracing, monitoring, and governance are first-class. This maps to the two-week story: prototype quickly, but capture telemetry early and graduate into a runtime where evaluation/observability are standard (last week''s Foundry Evaluations GA and agent runtime GA are the likely destinations). It also ties observability to Azure API Management''s genAI gateway controls (token metrics, prompt logging, quotas, safety policies) and suggests capturing telemetry from day one (even JSONL logs).
- [Microsoft Foundry Labs: A Practical Fast Lane from Research to Real Developer Work](https://techcommunity.microsoft.com/t5/microsoft-developer-community/microsoft-foundry-labs-a-practical-fast-lane-from-research-to/ba-p/4502127)
Fabric Real-Time Dashboards added a Copilot preview that generates and iterates KQL tiles from natural-language requests, suggesting a visualization, showing a preview table, and exposing editable KQL. It matches last week''s Fabric theme of speeding authoring where teams work while keeping the query layer inspectable, applied to real-time ops dashboards.
- [Use Copilot to create visuals in Real-Time Dashboards (Preview)](https://blog.fabric.microsoft.com/en-US/blog/use-copilot-to-create-visuals-in-real-time-dashboards-preview/)
Two "Budget Bytes" teaser posts pointed to a cost-constrained, hands-on AI app series around Azure SQL Database, linking to playlist/repo/free offer resources rather than going deep technically. In the context of last week''s cost/eval/ops focus, it''s another signal that budget-aware engineering is now common in AI guidance.
- [Budget Bytes: Azure Data Leaders on AI & Budget (Sneak Peek)](https://www.youtube.com/watch?v=BaGqdGds-eM)
- [What Would You Buy With $25? Answers from Execs](https://www.youtube.com/shorts/RIqLHwfg7oI)
A manufacturing case study highlighted a "copilot for operators" pattern: ARUM''s CNC assistant uses Azure AI Speech plus Azure OpenAI hosted in Microsoft Foundry (noted as GPT-5) to provide Japanese, step-by-step guidance for safety-critical setup tasks. It''s light on implementation details, but it connects threads we''ve been tracking: voice modalities (last week''s Voice Live vs this week''s offline voice pipeline) and production guardrails where procedures and human confirmation matter.
- [Japan’s ARUM turns craftsmanship into scalable AI for precision manufacturing](https://news.microsoft.com/source/asia/features/japans-arum-turns-craftsmanship-into-scalable-ai-for-precision-manufacturing/)',
    'This week''s AI updates tracked two parallel themes: shipping agents into production with repeatable workflows and governance, and adopting more local-first, inspectable patterns for building and operating AI systems. Across Azure AI Foundry, Foundry Local, and Microsoft Fabric, the common thread was making agent behavior easier to deploy, ground, observe, and control via IaC scaffolding, structured tool plans, ontology/graph grounding, and cost guardrails. This continues last week''s "run it like software" arc: last week delivered GA runtimes, private networking, managed identity, evaluation hooks, and MCP tooling glue; this week shows how teams ship and operate those ideas (IaC-first delivery, offline OpenAI-style endpoints, and more traceable retrieval/reasoning).',
    1774854000, 'ai', '/ai/roundups/weekly-ai-roundup-2026-03-30', 'TechHub',
    'TechHub', '880094102B29BEAC00EE1BD33E26A78A1260CE0FAB3B838282802EC8E761EFF6', ',Azure AI Foundry,Foundry Local,Azure Developer CLI,Bicep,Managed Identity,RBAC,MCP,Microsoft Fabric,Real Time Hub,Business Events,Fabric IQ Ontology,Graph RAG,Azure OpenAI,Retrieval Augmented Generation,.NET 10,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-03-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-03-23', 'roundups', 'Weekly AI Roundup: Production-ready agents, MCP tools, Fabric AI',
    'This week''s AI updates focused less on feature demos and more on making agent systems easier to run. Microsoft moved Azure AI Foundry''s agent runtime into GA with enterprise networking, identity, and evaluation hooks; MCP kept showing up as the tool-wiring layer; and Fabric continued to blend analytics and AI app building with more multimodal, real-time, and Copilot-driven workflows. Overall, it feels like a continuation of last week''s "run it like software" focus (approval gates, sandboxing, OpenTelemetry, structured outputs): more of those patterns are arriving as defaults (private networking, managed identity options, continuous eval, and tool connectivity without bespoke glue).
<!--excerpt_end-->
## Azure AI Foundry Agents move into production: GA runtime, private networking, voice, and built-in evaluation
Foundry Agent Service reached GA with a runtime built on the OpenAI Responses API, aiming to be wire-compatible for teams already aligned to Responses/Agents-style interfaces. Last week''s "treat agents like deployable software" theme shows up here as consolidation: rather than assembling orchestration, auth, and telemetry by hand (like last week''s Agent Framework + Foundry examples), Foundry is standardizing how agents are created, invoked, and governed. For Python, GA also tightens SDK guidance: agents become first-class operations on `AIProjectClient` in `azure-ai-projects`, with an explicit migration off `azure-ai-agents` (remove pin, use `azure-ai-projects`, call via `AIProjectClient.get_openai_client()` and `responses.create(..., extra_body={"agent_reference": ...})`).
The most practical production change is end-to-end private networking with a "Standard Setup" that keeps agent traffic off the public internet once agents start doing retrieval and tool calls. It extends last week''s "secure execution + governed gateways" theme: last week emphasized Foundry as a controlled model gateway plus approval and sandbox patterns at the tool layer, and this week adds the network boundary so models, retrieval, and tools can stay on private paths. Microsoft says this covers model runtime traffic and tool connectivity, including MCP servers, Azure AI Search indexes, and Fabric data agents, enabling a BYO VNet design without public egress for the workflow.
Identity and tool access also became more enterprise-shaped. Foundry expanded MCP authentication patterns: key-based access, Entra Agent Identity, Entra Foundry Project Managed Identity (project-level isolation), and OAuth identity passthrough for user-delegated access. This connects to last week''s least-privilege and approvals theme: when tools are real actuators (deployments, incidents, repo ops), these identity modes keep "agent can do things" from defaulting to shared secrets and broad access.
On interaction modes, Voice Live arrived as a managed, real-time speech-to-speech channel using the same agent runtime as text. Voice does not change the core problems (tool calling, tracing, approvals, compaction), but it does increase latency and reliability requirements. Having one runtime surface with shared tracing, evaluation, and cost accounting helps avoid building a separate voice stack that is hard to monitor.
Foundry Evaluations are now GA, with built-in evaluators (fluency/coherence, relevance, groundedness, retrieval quality, safety), custom evaluators, and continuous evaluation sampling of production traffic. This is the platform counterpart to last week''s operational loops (OpenTelemetry/Aspire dashboards, azd debugging, Fabric telemetry pipelines): instead of per-app eval harnesses, quality checks become continuous and viewable alongside latency, failures, and cost in Azure Monitor / Application Insights.
- [Foundry Agent Service is GA: private networking, Voice Live, and enterprise-grade evaluations](https://devblogs.microsoft.com/foundry/foundry-agent-service-ga/)
- [Microsoft announces new solutions, infrastructure at NVIDIA GTC](https://blogs.microsoft.com/blog/2026/03/16/microsoft-at-nvidia-gtc-new-solutions-for-microsoft-foundry-azure-ai-infrastructure-and-physical-ai/)
## MCP as the agent tool layer: remote servers in Foundry, managed observability gateways, and a C# SDK v1.0
MCP kept moving from "developer curiosity" to product integration, continuing last week''s story of MCP as a bridge between agent frameworks and external systems. This week''s tone is less "how to wire MCP" and more "here are MCP endpoints you can use," which is typically when protocols become operationally relevant.
Microsoft Foundry added a remote Azure DevOps MCP Server (public preview), letting Foundry agents connect to an Azure DevOps org via the tool catalog and call DevOps operations through MCP. It fits last week''s operational-agent direction (investigation, PR-shaped fixes): DevOps is where "agent does work" becomes risky without boundaries. A key control is restricting which DevOps tools the agent can use, which helps prevent early experiments from turning into "agent can access everything," echoing last week''s approval-gated tools and structured outputs.
Azure Managed Grafana MCP takes a different angle. Instead of deploying and securing a custom MCP server to expose telemetry, every Azure Managed Grafana instance can provide a built-in managed remote MCP endpoint. This connects to last week''s point that agents need day-two loops (telemetry, debugging, evaluation): MCP makes the observability estate queryable by agents while still using Azure RBAC and Grafana access controls. The approach is straightforward: authenticate with managed identity and let agents query Azure Monitor, Application Insights, and Kusto-backed sources without adding another hosted service.
For .NET teams, the MCP C# SDK v1.0 shipped with a community standup walkthrough focused on MCP as a vendor-neutral contract for exchanging context, requests, and responses. This matches last week''s polyglot MCP tool-server hint: Python ecosystems help, but a supported .NET SDK makes it easier to standardize tool wiring across enterprise services without tying everything to one runtime.
The ecosystem conversation remains noisy (GitHub''s "The Download" jokes about an "MCP funeral"), but Microsoft shipped multiple concrete MCP updates in the same week. Alongside last week''s repeated MCP appearances in Agent Framework + Foundry designs, the developer takeaway is that MCP is increasingly a practical option for tool connectivity, with identity, governance, and hosted endpoints becoming less DIY, while still keeping an eye on portability and cross-vendor compatibility.
- [Remote MCP Server preview in Microsoft Foundry](https://devblogs.microsoft.com/devops/remote-mcp-server-preview-in-microsoft-foundry/)
- [Introducing Azure Managed Grafana MCP: The Managed Data Gateway for AI Agents](https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-azure-managed-grafana-mcp-the-managed-data-gateway/ba-p/4503619)
- [.NET AI Community Standup: MCP (Model Context Protocol) C# SDK v1.0](https://www.youtube.com/watch?v=iMcowyYD5Q4)
- [The Download: MCP funeral, Perplexity computer, and Doom on a badge](https://www.youtube.com/watch?v=da8cSPcO7Lw)
## Knowledge-grounded agents with Foundry IQ: permission-aware retrieval via Azure AI Search + MCP
Foundry IQ aims to make "enterprise RAG" less bespoke by formalizing reusable knowledge bases over multiple sources (SharePoint, OneLake, Blob Storage, Azure AI Search, and more). It extends last week''s pattern of moving context out of prompts and into governed systems (Azure SRE Agent "Deep Context," Fabric telemetry-as-data-plane). Instead of each agent writing retrieval glue, IQ treats knowledge access as a platform component called through a standard tool surface.
The tutorial shows how this connects to Foundry Agent Service via MCP: the agent calls `knowledge_base_retrieve` exposed through an Azure AI Search endpoint, using a preview API path like `/knowledgebases/{kb-name}/mcp?api-version=2025-11-01-preview`.
The parts that matter most for developers are security and ops patterns. Retrieval is permission-aware: ACLs can sync into the index and be enforced at query time so the agent retrieves only what the current user is allowed to see, with citations generated as part of retrieval. This matches last week''s "structured outputs + approvals + least privilege" theme: production RAG is mostly access control, attribution, and repeatability. The sample also shows Entra ID + RBAC setup instead of keys: enable RBAC auth on Azure AI Search, grant project managed identity `Search Index Data Reader`, create an Azure AI Projects project connection as a RemoteTool target authenticated with Project Managed Identity, then attach an `MCPTool` and require tool usage plus citations in instructions.
Microsoft also announced a three-episode "IQ Series: Foundry IQ" starting March 18, 2026, with videos, notebooks, and cookbooks aimed at taking teams from concepts to multi-source knowledge bases and queries. The message is that retrieval is becoming a reusable platform surface (sources/bases + MCP endpoint + RBAC/ACL), not app-specific glue.
- [Building Knowledge-Grounded AI Agents with Foundry IQ](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-knowledge-grounded-ai-agents-with-foundry-iq/ba-p/4499683)
- [Announcing the IQ Series: Foundry IQ](https://techcommunity.microsoft.com/t5/microsoft-developer-community/announcing-the-iq-series-foundry-iq/ba-p/4501862)
## Building, testing, and operating agents: VS Code AI Toolkit, azd local run/invoke, and resilient long-running runs
Agent tooling posts kept shortening the path from prototype to repeatable testing, continuing last week''s day-two operability thread (azd status/logs, Aspire dashboards, production harnesses). The VS Code AI Toolkit + Foundry walkthrough shows an end-to-end workflow: start in an Agent Builder UI (assemble agent, attach tools, iterate in playground, ground on business data) then move to a code-first hosted template where you add custom Python functions, debug locally, deploy, run evals, do AI red teaming, and monitor quality/latency/cost across a fleet. It pulls last week''s production patterns into an IDE workflow so more teams can apply them consistently.
On CLI workflows, the Azure Developer CLI extension `azure.ai.agents` (v0.1.14-preview) adds `azd ai agent run` and `azd ai agent invoke`. This builds on last week''s hosted-agent visibility commands: `run` and `invoke` are what make regression testing and troubleshooting scriptable. `run` detects project type (Python/Node.js) and dependencies before launching. `invoke` supports streaming responses and persistent session IDs so multi-turn testing does not require manual state handling. It fits prompt testing scripts and CI-like agent checks, especially with Foundry Evaluations now GA.
For reliability, Microsoft Agent Framework added background responses for long-running operations without holding client connections open, especially for reasoning models that can take minutes. This extends last week''s long-session concerns (compaction, dynamic sessions, latency/cost control) into a resumable job model. In .NET, enable `AllowBackgroundResponses`; in Python, set `background=True`. If supported, you get a continuation token you can poll (non-streaming) or persist during streaming to resume after disconnects from the interruption point. The advice to persist continuation tokens (DB/cache) is a sign that agent runs increasingly behave like durable workflows with checkpoints, retries, and reattachment.
- [Build and Ship a Hosted Agent with VS Code AI Toolkit & Microsoft Foundry](https://www.youtube.com/watch?v=E9Hmk0PLMAQU)
- [Azure Developer CLI (azd): Run and test AI agents locally with azd](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-run-invoke/)
- [Handling Long-Running Operations with Background Responses](https://devblogs.microsoft.com/agent-framework/handling-long-running-operations-with-background-responses/)
## Agent patterns in practice: Python agent workflows and realtime voice multi-agent systems in .NET
Two longer pieces focused on architecture patterns, mapping closely to last week''s "multi-agent composition + observability + evaluation + approvals" blueprint.
Pamela Fox''s recap of the "Python + Agents" livestream series is a Microsoft Agent Framework curriculum: tool calling -> MCP servers -> supervisor/subagent patterns -> RAG with SQLite/PostgreSQL -> memory with Redis/Mem0 -> OpenTelemetry via Aspire dashboards -> evaluation with Azure AI Evaluation SDK -> branching/fan-out/fan-in workflows -> human approvals and checkpoint/resume for long-running work. It turns last week''s "real apps" examples into a repeatable build approach, including the same ops loops (OTel/Aspire, evaluation SDK) that both weeks keep reinforcing.
On the .NET side, RT.Assistant is a reference for low-latency voice assistants using OpenAI''s Realtime API over WebRTC, orchestrated in F#. It complements this week''s Foundry Voice Live: both treat voice as a first-class modality, but RT.Assistant focuses on runtime details (WebRTC/OPUS, message bus), while Voice Live emphasizes a managed channel with evaluation and tracing. RT.Assistant also makes multi-agent behavior more predictable with a deterministic state machine ("Flow") and a strongly typed message bus using F# discriminated unions, similar in spirit to last week''s structured schemas and explicit handoffs for automatable outputs. It argues for WebRTC + OPUS efficiency versus base64 PCM over WebSockets, and shows "structured RAG" where an LLM generates Prolog queries executed against a local KB (Tau Prolog) instead of embeddings/vector search.
- [Learn how to build agents and workflows in Python](https://techcommunity.microsoft.com/t5/microsoft-developer-community/learn-how-to-build-agents-and-workflows-in-python/ba-p/4502144)
- [RT.Assistant: A Multi-Agent Voice Bot Using .NET and OpenAI](https://devblogs.microsoft.com/dotnet/rt-assistant-a-realtime-multiagent-voice-bot-using-dotnet-and-open-ai-api/)
## Microsoft Fabric as an AI execution surface: multimodal functions, Copilot-in-notebooks, real-time intelligence, and pipeline automation
Fabric''s AI story kept moving from individual features to an integrated execution surface for analytics plus AI, extending last week''s Fabric thread (operational telemetry/eval loops, ontology automation, schema-controlled extraction). This week mostly deepens that direction with more modalities, more cost and operability surfaces, and more ways to push outputs into governed pipelines instead of one-off notebooks.
Fabric AI Functions added multimodal input (preview), so notebooks and Dataflows Gen2 can process images and PDFs (and common text formats) by passing file paths (including `column_type="path"`). It continues last week''s ExtractLabel theme: turn unstructured inputs into pipeline-friendly structured outputs, with schemas as the contract for downstream reliability. Helpers like `aifunc.load()` (folder-to-table with optional prompt/schema), `aifunc.list_file_paths()`, and `ai.infer_schema()` shorten the path from files to reproducible extraction via `ai.extract()`. Operability also improved: a progress bar estimating tokens and Capacity Units (CUs), and clearer capacity attribution in the Fabric Capacity Metrics App under "AI Functions." Evaluation notebooks for LLM-as-judge loops (executor + judge models, with precision/recall/F1/coherence) aim to reduce ad-hoc iteration and complement last week''s "reuse telemetry as eval data" guidance.
In notebooks, Fabric previewed an updated Copilot for data engineering/science with always-on context awareness (workspace, Lakehouse, notebook structure, runtime state), Spark performance recommendations based on observed behavior (joins, shuffles), and a "Fix with Copilot" loop that captures failure context, proposes patches, and applies them via diff review, plus a `/Fix` command for a cell or whole notebook. It is a notebook-native version of last week''s debugging and operability push: close the loop where people iterate.
Beyond notebooks, Fabric continued pushing real-time and operational AI via Real-Time Intelligence + Fabric IQ (ontology), building on last week''s Ontology Rules + Activator "Observe -> Analyze -> Decide -> Act" loop. OneLake ties to Eventstream/Eventhouse/Activator/real-time dashboards with a semantics layer so teams (and agents) interpret events consistently. Developer callouts include Maps GA, Business Events for semantic detection/triggers, Fabric Graph scaling and GQL updates (including shortest-path), and an Eventstream SQL Operator for SQL-based streaming transforms/routing (early April). Microsoft also announced a Microsoft-NVIDIA Omniverse direction (private preview planned in April) to embed 3D scenes into real-time dashboards for digital-twin/physical AI scenarios.
Fabric''s pipeline tooling also advanced: Data Factory pipelines added a Lakehouse Utility Suite (preview) with Lakehouse Maintenance activity and Refresh SQL Endpoint activity, while Copilot in the pipeline expression builder is GA for natural-language-to-expression authoring. It is a practical follow-on: once AI extraction and agent signals land in OneLake, you still need scheduled maintenance and automation to keep tables and endpoints healthy.
- [Multimodal support in Fabric AI functions (Preview): process images and PDFs, plus cost and eval updates](https://blog.fabric.microsoft.com/en-US/blog/unlock-insights-from-images-and-pdfs-with-multimodal-support-in-fabric-ai-functions-preview/)
- [Introducing the updated Copilot for data engineering and data science (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-the-updated-copilot-for-data-engineering-and-data-science-preview/)
- [Trusted AI starts with Microsoft Fabric: Unified real-time intelligence and IQ context](https://blog.fabric.microsoft.com/en-US/blog/trusted-ai-starts-with-microsoft-fabric-unified-real-time-intelligence-and-iq-context/)
- [Modernizing pipelines: New activities and innovations in Fabric Data Factory pipelines](https://blog.fabric.microsoft.com/en-US/blog/modernizing-pipelines-new-activities-and-innovations-in-fabric-data-factory-pipelines/)
## Other AI News
Fabric''s broader analytics/AI update included many GA vs preview details: Materialized Lake Views GA for incremental, quality-constrained lakehouse transforms; Runtime 2.0 preview moving toward Spark 4.0 / Delta Lake 4.0; new connectivity (JDBC GA, Spark ODBC and ADO.NET preview); and warehouse updates like compute isolation (Custom SQL Pools preview), freshness/stats automation, and AI functions callable from T-SQL. The roundup also introduced open-source "Agent Skills for Fabric" for GitHub Copilot CLI to scaffold and automate Fabric tasks from natural language, similar to last week''s Azure Skills direction but focused on Fabric operations.
Fabric Mirroring added opt-in paid "extended capabilities" (preview): Delta Change Data Feed into OneLake and Mirroring Views (Snowflake preview) to materialize source views as Delta tables, supporting incremental processing without custom change tracking. This matches last week''s governed data plane theme: keeping OneLake current helps downstream analytics, quality loops, and automation triggers stay aligned.
Fabric previewed Planning in Fabric IQ for budgeting/forecasting/scenarios on governed data and Power BI semantic models, with SQL writeback plus approval/audit/RBAC hooks. It is useful context for end-to-end operational analytics systems and another example of "semantic layer + governed actions," consistent with last week''s Ontology Rules theme.
Microsoft introduced MAI-Image-2, a text-to-image model focused on photorealism and more reliable in-image typography, with testing via MAI Playground and broader API access expected via Foundry.
For Foundry model optimization, Microsoft published videos on supervised fine-tuning, improving tool-calling accuracy (synthetic data and distillation), and post-training workflows (custom graders, evaluation, cost planning, deployment). The emphasis on graders, evaluation, and cost planning matches last week''s evaluation-loop focus and this week''s Foundry Evaluations GA.
For teams hosting inference stacks, AKS "inference at scale" guidance covered tensor/pipeline/data parallelism tradeoffs, quantization-first advice, Ray placement groups (Anyscale on Azure) for shard-aware scheduling, and production security posture (private clusters, Cilium policy, Entra ID + managed identities, Key Vault), plus core metrics (tokens/sec/GPU, tail latency, KV cache hit rate, tokens per GPU-hour). It fits the broader arc: private networking, managed identity, and measurable ops loops become baseline expectations when AI moves from prototypes to services.
- [From Lakehouse to boardroom: Analytics and AI for real insights](https://blog.fabric.microsoft.com/en-US/blog/from-lakehouse-to-boardroom-analytics-and-ai-for-real-insights/)
- [Extended Capabilities in Mirroring in Microsoft Fabric: Optional Enhancements to Core Mirroring](https://blog.fabric.microsoft.com/en-US/blog/extended-capabilities-in-mirroring-in-microsoft-fabric-optional-enhancements-to-core-mirroring/)
- [Introducing Planning in Microsoft Fabric IQ: From historical data to forecasting the future](https://blog.fabric.microsoft.com/en-US/blog/introducing-planning-in-microsoft-fabric-iq-from-historical-data-to-forecasting-the-future/)
- [Introducing MAI-Image-2: for limitless creativity](https://microsoft.ai/news/introducing-mai-image-2/)
- [Model Optimization in Microsoft Foundry: Supervised Fine-Tuning](https://www.youtube.com/watch?v=GyCLzGQwaEc)
- [Model Optimization in Microsoft Foundry: AI Agents Tool Calling Accuracy](https://www.youtube.com/watch?v=vKfG50oLSmk)
- [Model Optimization in Microsoft Foundry: Deployment and Evaluations](https://www.youtube.com/watch?v=6awzUxeG-b0)
- [Building an Enterprise Platform for Inference at Scale](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-an-enterprise-platform-for-inference-at-scale/ba-p/4498820)
- [Building Microsoft’s Sovereign AI on Azure Local with NVIDIA RTX PRO and Next Gen NVIDIA Rubin](https://techcommunity.microsoft.com/t5/azure-arc-blog/building-microsoft-s-sovereign-ai-on-azure-local-with-nvidia-rtx/ba-p/4502383)
- [The power of the Microsoft Fabric ecosystem: ISVs building natively on Fabric](https://blog.fabric.microsoft.com/en-US/blog/the-power-of-the-microsoft-fabric-ecosystem-isvs-building-natively-on-fabric/)',
    'This week''s AI updates focused less on feature demos and more on making agent systems easier to run. Microsoft moved Azure AI Foundry''s agent runtime into GA with enterprise networking, identity, and evaluation hooks; MCP kept showing up as the tool-wiring layer; and Fabric continued to blend analytics and AI app building with more multimodal, real-time, and Copilot-driven workflows. Overall, it feels like a continuation of last week''s "run it like software" focus (approval gates, sandboxing, OpenTelemetry, structured outputs): more of those patterns are arriving as defaults (private networking, managed identity options, continuous eval, and tool connectivity without bespoke glue).',
    1774252800, 'ai', '/ai/roundups/weekly-ai-roundup-2026-03-23', 'TechHub',
    'TechHub', '67A79DE9D1BB659D786770261B84746E8DE344BAD0F0B9F2850D5EB8A126E1B5', ',Azure AI Foundry,Agent Service,OpenAI Responses API,Private Networking,Microsoft Entra ID,Managed Identity,Foundry Evaluations,MCP,Azure DevOps,Azure Managed Grafana,Azure AI Search,Retrieval Augmented Generation,Microsoft Fabric,Copilot,Azure Developer CLI,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-03-16
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-03-16', 'roundups', 'Weekly AI Roundup: Multi-Agent Ops, Safe Tools, and Telemetry Loops',
    'AI coverage kept coming back to a practical question: how do you move from “an LLM that chats” to systems that can operate safely, repeatably, and at scale. This continues last week’s thread on production-ready agent tooling (skills, orchestration, sandboxing, MCP/OpenTelemetry), but with more “run it like software” patterns: multi-agent composition, approval gates, context compaction, and the operational plumbing (deploy automation, debugging loops, telemetry/evaluation, data platforms) needed for real deployments.
<!--excerpt_end-->
## Microsoft Agent Framework: production patterns for multi-agent apps (Python + .NET)
Microsoft Agent Framework content leaned more toward engineering details this week. It builds on last week’s reusable skills, orchestration patterns, and secure execution by showing how teams assemble multi-agent apps that can ship and run reliably.
One guide turns incident response into a multi-agent workflow by splitting “on-call copilot” into four narrow agents - Triage, Summary, Comms, and PIR - with strict JSON schemas so outputs can feed automation (tickets, updates, runbooks) without brittle parsing. The orchestrator uses `ConcurrentBuilder` and `asyncio.gather()` to run agents in parallel, replacing one large prompt with lower latency and more predictable structure. Deployment is set up for production use: a containerized Python orchestrator as a Foundry Hosted Agent, with model choice delegated to Azure OpenAI Model Router (one deployment like `model-router` routing between `gpt-4o` and `gpt-4o-mini`). Auth uses `DefaultAzureCredential` with the `https://cognitiveservices.azure.com/.default` scope (local via `az login`, prod via managed identity) so teams do not have to distribute API keys. It reads as a direct follow-on to last week’s guidance on boundaries and orchestration.
On the .NET side, the “Interview Coach” architecture uses the same multi-agent approach: receptionist/triage, behavioral interviewer, technical interviewer, and summarizer with explicit handoffs. It uses Agent Framework patterns (DI, type safety, OpenTelemetry), Microsoft Foundry as a governed model gateway (single endpoint, centralized identity/governance/moderation like PII detection), and external capabilities via MCP tool servers. The design is deliberately polyglot (for example, Python MarkItDown used by a .NET agent), which extends last week’s open-standards/MCP SDK coverage by showing cross-language tool servers in practice. .NET Aspire provides orchestration, service discovery, health checks, and a traces/health dashboard, with an end-to-end path from local `aspire run` to cloud `azd up`.
As agent apps start executing actions, the agent harness patterns post fills in safety and operability details that were previewed last week (dynamic sessions, secure execution). The theme is consistent: expose shell/filesystem tools with explicit approvals, move execution into hosted/container sandboxes, and keep long sessions from expanding tokens and latency. Python shows an approval-gated shell tool around `subprocess.run(...)` (timeouts, stdout/stderr capture), while .NET uses approval-required wrappers (for example, `ApprovalRequiredAIFunction`). For context compaction, Python shows sliding-window retention, while .NET combines strategies (tool result compaction + sliding windows + truncation) via `Microsoft.Agents.AI.Compaction` to tune responsiveness and cost. These are practical complements to last week’s “load skills only when needed” theme.
Python SDK Agent Skills updates also move skills closer to normal software development and extend last week’s skills SDK coverage. Skills can be defined in code (not only bundled files), resources can be dynamic via functions, scripts can be decorator-based in-process functions or file-based scripts via pluggable runners, and script execution can require human approval (`require_script_approval=True`). Across the posts, the pattern is consistent: multi-agent composition for clarity and latency, structured outputs for automation, tool execution behind approvals and sandboxing, and explicit strategies to keep long sessions reliable and affordable.
- [Building a Multi-Agent On-Call Copilot with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-multi-agent-on-call-copilot-with-microsoft-agent/ba-p/4499962)
- [Building an Interview Coach App with Microsoft Agent Framework, Foundry, MCP, and Aspire](https://devblogs.microsoft.com/blog/build-a-real-world-example-with-microsoft-agent-framework-microsoft-foundry-mcp-and-aspire)
- [''Agent Harness Patterns with Microsoft Agent Framework: Shell Execution and Context Compaction in Python and .NET''](https://devblogs.microsoft.com/agent-framework/agent-harness-in-agent-framework/)
- [''What’s New in Agent Skills: Code Skills, Script Execution, and Approval for Python''](https://devblogs.microsoft.com/agent-framework/whats-new-in-agent-skills-code-skills-script-execution-and-approval-for-python/)
## Azure automation and agent operations: Skills Plugin, SRE Agent “Deep Context,” and azd debugging
This week’s operational theme was making agents less advice-only and more able to execute real Azure work, while also improving how teams debug deployed agents. This continues last week’s secure execution and durable-tasks story (dynamic sessions, MCP SDKs), but with a day-two focus: environment context, tool-backed deployments, and CLI-first troubleshooting.
The Azure Skills Plugin is the clearest push in this area. It ships Azure skills (19+ guarded workflows), an Azure MCP Server with 200+ tools across 40+ services, and a Foundry MCP Server for model catalog/management/deployment. The goal is to turn prompts like “Deploy my Python Flask API to Azure” into a structured Prepare → Validate → Deploy flow: generate artifacts (for example, Dockerfiles), run preflight checks, generate/use IaC, then deploy via `azd`. It operationalizes last week’s reusable skills and tool discovery approach by shipping a ready-made Azure tool/skills surface. Requirements make it clear this is meant for execution: a compatible host (Copilot in VS Code, Copilot CLI, or Claude Code), Node.js 18+, `az`, `azd`, and an authenticated Azure account. Smoke tests include a guidance-only question and a live tool call (list resource groups) to confirm MCP servers and skills are active.
Azure SRE Agent also moved further from an incident assistant toward an operations agent that builds environment-specific expertise. Deep Context (described as available in GA) centers on continuous access to connected repositories and artifacts (auto-cloned/indexed), persistent memory across sessions (including capture via `#remember`), and background intelligence that discovers log schemas/KQL tables and generates reusable query templates. This extends last week’s boundaries theme: rather than stuffing context into prompts, the agent maintains a governed workspace and pulls evidence into the conversation when needed. The example (HTTP 5xx spike on a container app) shows the intent: start incidents with recent code/config and history already ingested. Another post describes “autonomous investigation” using a real cache-hit alert: parallel subagents tested hypotheses, filesystem workflows (`grep`, `find`, shell, reading files) tied telemetry to exact code versions, and the result was PR-shaped remediation (exclude uncacheable requests from alerting logic; restore prompt-prefix stability affecting caching). Across both, the pattern is consistent: treat the agent like a developer in a repository, layer context intentionally, keep evidence out of prompts until needed, and route changes through PR/CI gates.
To support hosted-agent operations, `azd` added debugging via the `azure.ai.agents` extension. `azd ai agent show` reports container status/health/replicas/errors, and `azd ai agent monitor` streams container logs, keeping troubleshooting in one CLI loop instead of bouncing between portals. This complements last week’s traceability focus (OpenTelemetry/OAuth): once agents are services, a status/log loop and consistent identity become part of basic supportability. Version details are explicit: `azure.ai.agents` `v0.1.12-preview`, included with `azd` `1.23.7+`, plus upgrade (`azd extension upgrade azure.ai.agents`) and bootstrap (`azd ai agent init`).
- [''Announcing the Azure Skills Plugin: AI Skills and Automation for Azure Deployment''](https://devblogs.microsoft.com/all-things-azure/announcing-the-azure-skills-plugin/)
- [How Azure SRE Agent’s Deep Context Builds Operational Expertise](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-now-builds-expertise-like-your-best-engineer/ba-p/4500754)
- [''The Agent that Investigates Itself: How Azure SRE Agent Enables Autonomous Incident Resolution''](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/the-agent-that-investigates-itself/ba-p/4500073)
- [Debugging Hosted AI Agents with Azure Developer CLI (azd)](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-logs-status/)
## Microsoft Foundry and Microsoft Fabric: model deployment choices and production telemetry/evaluation loops
Platform coverage connected two parts of production AI work: model deployment with controls, and telemetry/data for evaluating and governing what agent apps actually do. This extends last week’s Foundry theme (models + agent features + stable SDKs) by adding open-model deployment options, while Fabric positions observability and accountability as an end-to-end data plane.
Microsoft Foundry added a public preview integration with Fireworks AI for open-model inference hosted on Azure but managed through Foundry’s control plane. Teams can browse the catalog, evaluate models, deploy endpoints, monitor usage/quality, and apply governance without wiring together separate tools. Deployment supports serverless pay-per-token (“Data Zone Standard”) and provisioned throughput units (PTUs). It also adds BYOW (Bring Your Own Weights): upload/register custom (quantized or fine-tuned) weights and serve them through the same workflow. This extends last week’s “single control plane + stable SDKs” message to teams mixing frontier models and open weights. The post cites catalog models (for example, DeepSeek V3.2, OpenAI gpt-oss-120b, Kimi K2.5, MiniMax M2.5), signaling a consistent “try → deploy → govern” flow even as the open-model set changes.
Microsoft Fabric’s agentic guidance focuses on observability and operations. One post frames Fabric as the operational data plane for agents: land structured telemetry into a governed OneLake workspace so teams can monitor routing, tool calls, latency, safety blocks, and failures in near real time (Eventstream → Eventhouse with KQL), and also do historical/business correlation (Lakehouse + semantic model + Power BI). It builds on last week’s best practices (boundaries, compliance, observability) by describing what to emit and where to store it. A reference implementation (Agentic Banking App: React + Python/LangGraph) demonstrates the telemetry pipeline, and the quality loop uses notebooks plus Azure AI Evaluation SDK by reusing captured telemetry instead of rebuilding ad-hoc datasets.
Fabric also strengthened the link between business semantics and automation. Ontology Rules integrate with Fabric Activator so teams define real-time conditions/actions using Ontology entities/properties (Customer, Order, Device) rather than raw tables or stream-specific logic. The cold-chain example (“Freezer temperature exceeds safe limits for sustained period → trigger alert”) shows the goal: define thresholds in a governed semantic layer so analytics, agents, and automations reuse consistent definitions.
Fabric AI Functions added ExtractLabel for schema-driven extraction of structured fields from unstructured text in pandas and PySpark. The key is enforcing an explicit output contract (JSON Schema or Pydantic schema) with required/optional fields, enums, nested structures, and `additionalProperties=False` to prevent extra keys, making outputs predictable for downstream validation and pipelines. This mirrors the structured-output discipline in Agent Framework workflows: reliable machine-consumable AI outputs reduce brittle parsing. It also works in distributed PySpark via `synapse.ml.spark.aifunc`, supporting LLM extraction at data-engineering scale.
- [Fireworks AI Now Available in Microsoft Foundry for High-Performance Open Model Inference on Azure](https://azure.microsoft.com/en-us/blog/introducing-fireworks-ai-on-microsoft-foundry-bringing-high-performance-low-latency-open-model-inference-to-azure/)
- [Operationalizing Agentic Applications with Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/operationalizing-agentic-applications-with-microsoft-fabric/)
- [''From Insight to Action: Bringing Fabric Activator into Ontology with Rules''](https://blog.fabric.microsoft.com/en-US/blog/from-insight-to-action-bringing-fabric-activator-into-ontology-with-rules/)
- [''ExtractLabel: Schema-driven Unstructured Data Extraction with Microsoft Fabric AI Functions''](https://blog.fabric.microsoft.com/en-US/blog/extractlabel-schema-driven-unstructured-data-extraction-with-fabric-ai-functions/)
## Other AI News
Microsoft Research introduced AgentRx, a framework concept for systematic debugging of multimodal agents by centralizing traces across modalities and adding verifier-style checks to isolate failures (input interpretation, action selection, intermediate decisions, output validation). With this week’s production debugging focus (azd logs/status, Aspire dashboards, OpenTelemetry), AgentRx reads like the research-side version of the same idea: as tools and modalities expand, agents need failure modes that teams can observe and debug.
- [''Systematic Debugging for AI Agents: Introducing the AgentRx Framework''](https://www.microsoft.com/en-us/research/blog/systematic-debugging-for-ai-agents-introducing-the-agentrx-framework/)
VS Code added chat forking: branch a conversation at any point, explore alternatives in parallel, and keep the original thread for comparison. This aligns with last week’s VS Code agent UX work (including forking) and reinforces that chat is becoming a workflow control surface, not only a single linear thread.
- [Forking Chat Sessions in Visual Studio Code](https://www.youtube.com/shorts/mpqrdghoj_Y)
Several higher-level pieces reinforced common constraints around autonomy and security in agentic systems. One describes an IT loop (observe → detect → analyze → act → learn) using Azure Monitor, Automation/runbooks, AKS self-healing, CI/CD hooks, and security tooling. Another breaks down Copilot agent design (goals, memory, tools, autonomy) with guardrails like least privilege and human approval. A “computer use agents” overview highlights risk when agents can operate software environments, which puts least-privilege identity and authorization design at the center. This echoes last week’s secure execution focus once agents move from recommend to act.
- [''Agentic AI in IT: Self-Healing Systems and Smart Incident Response in the Microsoft Ecosystem''](https://dellenny.com/agentic-ai-in-it-self-healing-systems-and-smart-incident-response-microsoft-ecosystem-perspective/)
- [''How Copilot Agents Think: Goals, Memory, Tools, and Autonomy''](https://dellenny.com/how-copilot-agents-think-goals-memory-tools-and-autonomy/)
- [''Building Computer Use Agents: Types, Functionality, and Security Risks''](https://www.youtube.com/watch?v=zr_DuUzFEd4)
Low-code agent building showed up via a cost-focused walkthrough: Copilot Studio with Azure SQL Database as system of record, including how to keep an entry-level deployment around ~$10/month by using free/low-cost options and careful SKU choices, then iterating agent behavior in Copilot Studio. It complements last week’s Copilot/Fabric coverage by grounding adoption in budgeting, SKU selection, and incremental rollout.
- [Building Low-Code AI Agents with Copilot Studio and Azure SQL Database for Under $10/Month](https://www.youtube.com/watch?v=FzoXP4P8OAY)',
    'AI coverage kept coming back to a practical question: how do you move from “an LLM that chats” to systems that can operate safely, repeatably, and at scale. This continues last week’s thread on production-ready agent tooling (skills, orchestration, sandboxing, MCP/OpenTelemetry), but with more “run it like software” patterns: multi-agent composition, approval gates, context compaction, and the operational plumbing (deploy automation, debugging loops, telemetry/evaluation, data platforms) needed for real deployments.',
    1773648000, 'ai', '/ai/roundups/weekly-ai-roundup-2026-03-16', 'TechHub',
    'TechHub', 'CA90189034763E5D3682193401366C89E3655285F5A6DB2547F732A84D4751BC', ',AI Agents,LLMs,Microsoft Agent Framework,Multi Agent Systems,Structured Outputs,JSON Schema,Approval Gates,Sandboxed Execution,MCP,OpenTelemetry,Azure Developer CLI,Azure OpenAI,Microsoft Foundry,Microsoft Fabric,Evaluation And Observability,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-03-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-03-09', 'roundups', 'Weekly AI Roundup: Agent Skills, Foundry Updates, and Secure Runs',
    'The AI section outlines current innovations and tool adoption, especially among Microsoft and open-source developers. The focus is on new agent frameworks, skills SDKs, toolkits, and orchestration options for agentic applications, plus guidance on deploying secure and scalable AI solutions. This continues themes from last week, adding new practical toolkits for production-ready deployment.
<!--excerpt_end-->
## Microsoft Foundry Ecosystem: Model, Agent, and SDK Advancements
Microsoft Foundry’s February 2026 release brings new models and agent features, picking up from recent updates on edge AI and hybrid privacy. Anthropic''s Claude Opus/Sonnet 4.6 models enable deep reasoning, scalable deployment, and large context handling. Models like GPT-Realtime-1.5 and GPT-Audio-1.5 support internationalized and voice-driven use cases. The Grok 4.0 engine enhances agent workflows, and FLUX.2 Flex improves text-image generation for interface prototyping.
Model updates complement new features for privacy and on-prem deployments. Foundry Local allows for disconnected, compliance-aware hardware. The Agent Framework (Python) reaches API stability, with better credential management, session orchestration, and migration instructions. Durable agent orchestration via Azure Functions and SignalR enables agents to work around long delays or restarts—useful in public sector and telecom scenarios.
The new Foundry REST API v1 is stable, with SDKs for Python, .NET, JS/TS, and Java. It introduces consistent naming and credential handling. Migration is supported for previous versions. There are also improvements in the AI Toolkit for VS Code (v0.30.0), introducing debugging, a model inspector, and catalog tools for quick prototyping and release. Documentation now further supports onboarding and protocol usage.
- [What''s New in Microsoft Foundry: February 2026 Update](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-feb-2026/)
- [Introducing GPT-5.4 in Microsoft Foundry for Enterprise AI Production](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/introducing-gpt-5-4-in-microsoft-foundry/4499785)
- [Unlocking Document Understanding with Mistral Document AI in Microsoft Foundry](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/unlocking-document-understanding-with-mistral-document-ai-in-microsoft-foundry/4495664)
## Modular Skills and Agent Frameworks: Agent Skills SDK, Reusable Skills, and Dynamic Tool Discovery
The Agent Skills SDK, an open-source Python toolkit, helps developers package common agent knowledge as portable skills. These can be published or discovered across different storage providers (local, HTTP, Azure/S3, databases). Skills are loaded only when relevant to save resources. The SDK works with tools like LangChain and Microsoft Agent Framework, supporting skill reuse and modular agent composition. It is MIT-licensed and customizable for DevOps, incident response, and retrieval agents.
Microsoft Agent Framework also now supports reusable skills for .NET/Python agents, packaged with scripts and configuration to support on-demand discovery. Skills are loaded only as needed, reducing context and token requirements. Guidance is included for safely sharing and maintaining skills, with forward-looking features for creating new ones dynamically.
Developers can use `mcp-cli`, a Bun-powered CLI, for finding tools in a token-efficient way, letting agents only fetch what they need. This aligns with earlier updates on secure modular agent deployment.
- [Giving Your AI Agents Reliable Skills with the Agent Skills SDK](https://techcommunity.microsoft.com/t5/microsoft-developer-community/giving-your-ai-agents-reliable-skills-with-the-agent-skills-sdk/ba-p/4497074)
- [Equip Microsoft Agent Framework Agents with Reusable Agent Skills](https://devblogs.microsoft.com/semantic-kernel/give-your-agents-domain-expertise-with-agent-skills-in-microsoft-agent-framework/)
- [MCP vs mcp-cli: Dynamic Tool Discovery for Token-Efficient AI Agents](https://techcommunity.microsoft.com/t5/microsoft-developer-community/mcp-vs-mcp-cli-dynamic-tool-discovery-for-token-efficient-ai/ba-p/4494272)
## Secure Agent Execution and Durable Tasks: Azure Container Apps Dynamic Sessions, MCP C# SDK
Updates in this area give developers ways to run untrusted or agent-supplied code in Azure Container Apps with dynamic sessions, offering sandboxed runtimes for various workloads. Integration with Agent Framework and MCP is supported, while Azure AD and OpenTelemetry are used for authentication and traceability. Templates and deployment instructions make rollout easier, focusing on safe and repeatable ephemeral compute.
The MCP C# SDK v1.0 brings improved capabilities to .NET developers, providing durable AI operations, OAuth 2.0, client/server APIs, tool calling, SSE event streaming, and more. These changes enable secure, large-scale, and async tasks for agentic systems.
- [Safely Running AI-Generated Code with Azure Container Apps Dynamic Sessions](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/even-simpler-to-safely-execute-ai-generated-code-with-azure/ba-p/4499795)
- [Official MCP C# SDK v1.0 Released: Major Updates for Authorization, Tools, and Tasks](https://devblogs.microsoft.com/dotnet/release-v10-of-the-official-mcp-csharp-sdk/)
## Architectures and Best Practices: High-Performance Agentic Systems, Open Standards
A Microsoft guide details best practices for enterprise-scale agentic AI engineering with Foundry and Copilot Studio, emphasizing differences from traditional chatbots, and focusing on autonomy, orchestration, and clear boundaries. The architecture makes use of tools like Microsoft Graph, Logic Apps, and Power Automate. Memory, access control, observability, and compliance requirements are addressed, with case studies for contract analysis and customer support.
A separate panel review discusses how open standards (MCP, Agent2Agent, OpenTelemetry, OAuth) help with secure, interoperable agent deployments, as well as how to write requests-for-proposal and select technologies focused on compatibility.
- [Building High-Performance Agentic Systems: From Chatbots to Enterprise Operations](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-high-performance-agentic-systems/ba-p/4497391)
- [Open Standards for Enterprise Agents: Architecting Secure and Interoperable Agentic AI](/ai/videos/open-standards-for-enterprise-agents-architecting-secure-and-interoperable-agentic-ai)
## Other AI News
VS Code has added five new agent features: skills on demand, message steering, integrated browser, conversation forking, and lifecycle hooks. These updates are aimed at improving productivity and automation for developers.
- [Top 5 New VS Code Agent Features to Improve Your Workflow](/ai/videos/top-5-new-vs-code-agent-features-to-improve-your-workflow)
Copilot for Data Factory (Microsoft Fabric) enables low-code data engineering, including natural language SQL transformations and automated pipeline setup.
- [SQL to Insights in Minutes with Copilot for Data Factory](/ai/videos/sql-to-insights-in-minutes-with-copilot-for-data-factory)
Power Platform developers have new documentation for building Generative Pages with AI, supporting advanced UI features and Dataverse integration.
- [Building Generative Pages in Power Platform](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-generative-pages-in-power-platform/ba-p/4494062)
A new JavaScript AI Build-a-thon season is open, providing hands-on agentic AI and data pipeline challenges for JS/TS developers.
- [JavaScript AI Build-a-thon Season 2: Hands-On Program for AI Developers](https://devblogs.microsoft.com/blog/the-javascript-ai-build-a-thon-season-2-starts-today)
A GitHub interview with Anders Hejlsberg discusses the shift from AI assistants to more capable agents, including their effect on software tools and interoperability.
- [Anders Hejlsberg Discusses the Evolution from AI Assistant to AI Agent](/ai/videos/anders-hejlsberg-discusses-the-evolution-from-ai-assistant-to-ai-agent)',
    'The AI section outlines current innovations and tool adoption, especially among Microsoft and open-source developers. The focus is on new agent frameworks, skills SDKs, toolkits, and orchestration options for agentic applications, plus guidance on deploying secure and scalable AI solutions. This continues themes from last week, adding new practical toolkits for production-ready deployment.',
    1773043200, 'ai', '/ai/roundups/weekly-ai-roundup-2026-03-09', 'TechHub',
    'TechHub', 'C854DD7235426D356E1A611268AA4D2B2DC321DBF4AC9C8AC426F78819C7650C', ',Agentic AI,Microsoft Foundry,Azure AI Foundry,Microsoft Agent Framework,Agent Skills SDK,MCP,MCP CLI,Azure Container Apps,Dynamic Sessions,OpenTelemetry,OAuth 2.0,Azure Functions,SignalR,VS Code AI Toolkit,Copilot Studio,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-03-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-03-02', 'roundups', 'Weekly AI Roundup: Secure Agents, Hybrid AI, and Vector Data',
    'Microsoft’s AI ecosystem gets updates in agent automation, privacy-focused infrastructure, and developer tooling. Copilot Studio introduces new secure automation options, .NET simplifies vector data handling, and hybrid on-premises/cloud approaches are covered in depth. Recent updates show practical guidance for upskilling, prompt strategy, and measuring AI coding impact.
<!--excerpt_end-->
## Microsoft Copilot Studio Agentic Automation: Secure UI and Sales Workflows
Copilot Studio has new capabilities for computer-using agents (CUAs) that automate adaptive and secure UIs—beyond simple RPA scenarios. Developers can select Claude Sonnet 4.5 or OpenAI models for different use cases, managing credentials with the Power Platform or Azure Key Vault to reduce exposure risks. Monitoring now supports replay with audit logs, retention policies, and Purview/Dataverse compliance tracking. Cloud PC pools, Intune enrollment, and Entra ID simplify management at scale. Guided onboarding automates migration from scripted UI flows to agent-based systems for more resiliency. Updates reflect feedback from the community.
Integration improvements allow modular CRM and telecom automation using open standards (TMF ODA, eTOM), enabling easier agent ramp-up. Success stories detail positive alignment with business metrics, like conversions and retention rates supported by agent-centric processes.
- [Computer-Using Agents Deliver Enhanced Secure UI Automation at Scale in Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/computer-using-agents-now-deliver-more-secure-ui-automation-at-scale/)
- [Transforming Telecom Sales with Agentic AI and Copilot Studio](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/accelerating-revenue-in-telecommunications-through-agentic-sales/ba-p/4496523)
## Azure AI Infrastructure, Microsoft Foundry, and Edge AI Deployment
Azure’s AI infrastructure, shown at NVIDIA GTC 2026, features agentic models for large-scale training and deployment. Microsoft Foundry enables confidential and on-premises AI, real-time inference, and advanced robotics with NVIDIA hardware. Foundry tutorials cover running private AI in manufacturing and on hybrid edge/cloud systems, with integration guides for REST APIs and Node.js, supporting privacy and compliance use cases.
Local and hybrid AI extends privacy for regulated workflows, connecting with Azure OpenAI for compliant workload management. Guides show how to build privacy-focused, on-prem/cloud solutions for medical, manufacturing, and operational scenarios, all with sample code and API integration.
- [Microsoft Showcases Azure AI Infrastructure and Agentic AI at NVIDIA GTC 2026](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/microsoft-at-nvidia-gtc-2026/ba-p/4497670)
- [Building On-Premises AI-Powered Asset Monitoring with Microsoft Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/on-premises-manufacturing-intelligence/ba-p/4490771)
- [Building a Privacy-First Hybrid AI Briefing Tool with Foundry Local and Azure OpenAI](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-privacy-first-hybrid-ai-briefing-tool-with-foundry/ba-p/4490535)
## Advanced Vector Data and Retrieval-Augmented Generation in .NET and Azure
Developers benefit from Microsoft.Extensions.VectorData for .NET, which abstracts vector database access (Qdrant, Azure AI Search, Redis, Cosmos DB, SQL Server, SQLite, PostgreSQL). You can map C# models, use LINQ, and work with embeddings for semantic and RAG search. Vector storage is optional, letting teams manage project size and scale search capabilities for chatbots and other AI features.
Guides and repo samples support cost-effective architecture and backend integration, building on recent work around affordable, agent-powered .NET workflows. Developers can try full RAG implementations using Azure SQL, OpenAI, and Static Web Apps—with Data API Builder, LangChain support, and hands-on samples for quick prototyping and deployment.
- [Vector Data in .NET – Building Blocks for AI Part 2](https://devblogs.microsoft.com/dotnet/vector-data-in-dotnet-building-blocks-for-ai-part-2/)
- [Building an Agentic RAG Solution with Azure SQL, OpenAI, and Web Apps](/ai/videos/building-an-agentic-rag-solution-with-azure-sql-openai-and-web-apps)
## Agent Frameworks and Industry Adoption: Telco, Networking, and Public Sector
Microsoft’s NOA Framework advances telco automation with deterministic agent management, refined prompt workflows, and layered observability using Foundry and Azure. TM Forum API integration (TMF621) supports OSS/BSS stack compatibility, and features like managed identity, RBAC, and human review gates support best practices. Case studies with Vodafone and Far EasTone demonstrate improved NOC workflows and incident resolution, with links to blueprints and extensibility notes.
Recent posts show agentic platforms moving from pilots to enterprise adoption, including live GIS/CAD integration with Munich Fire Department. Lessons focus on governance and resilience for AI in operational environments.
- [Evolving the Network Operations Agent Framework: Microsoft’s Blueprint for Autonomous Telco Operations](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/evolving-the-network-operations-agent-framework-driving-the-next/ba-p/4496607)
- [AI-Powered Agent Platform Helps Munich Fire Department Speed Up Patient Transfers](https://www.linkedin.com/posts/satyanadella_how-the-munich-fire-departments-ai-operator-activity-7432465483335106560-x8Vj)
## Developer Tooling, Protocols, and AI Coding Impact
This week features technical guides on Model Context Protocol (MCP) for LLM integration, highlighting modular design, extensibility, and secure agent deployment. MCP allows safer interfaces between AI models and real-world data or tools, supporting enterprise integration and auditing.
Model Router for Azure OpenAI helps developers choose optimal models, balancing cost and output quality. Provided benchmarks, demos, and production notes help manage model selection, including BYOK options, with guides for secure deployment using Managed Identity.
Prompt engineering discussions introduce persistent context management, prompt chaining, and evaluation using tools like SOMA. Advice includes moving from manual chat prompts to structured agent workflows to improve reliability.
Microsoft leadership comments on AI’s impact: while senior developers are more productive, there is heightened risk of bugs and limited skill growth for new developers. Recommendations include mentoring, collaborative review, and updated training to maintain quality as code generation increases.
- [Understanding Model Context Protocol (MCP)](/ai/videos/understanding-model-context-protocol-mcp)
- [Optimising AI Costs with Microsoft Foundry Model Router](https://techcommunity.microsoft.com/t5/microsoft-developer-community/optimising-ai-costs-with-microsoft-foundry-model-router/ba-p/4494776)
- [Prompt Engineering That Actually Works](https://hiddedesmet.com/prompt-engineering-that-actually-works)
- [Microsoft Leaders Warn of AI''s Impact on Junior Developers and Engineering Skills](https://www.devclass.com/development/2026/02/26/top-microsoft-execs-fret-about-impact-of-ai-on-software-engineering-profession/4091789)
## Announcements and Skills Development in AI
Developers can participate in the JavaScript AI Build-a-thon, a four-week self-paced program using Microsoft Foundry to build AI agents, RAG workflows, and web applications. The program connects Python-based AI approaches with JavaScript-friendly stacks, promotes community projects, and includes mentorship, demos, and office hours for concrete skill-building.
This event reflects ongoing commitments to developer education and upskilling, following previous hackathons and innovation bootcamps.
- [The JavaScript AI Build-a-thon: Level Up AI Skills with JavaScript and Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/the-javascript-ai-build-a-thon-season-2-starts-march-2/ba-p/4496855)',
    'Microsoft’s AI ecosystem gets updates in agent automation, privacy-focused infrastructure, and developer tooling. Copilot Studio introduces new secure automation options, .NET simplifies vector data handling, and hybrid on-premises/cloud approaches are covered in depth. Recent updates show practical guidance for upskilling, prompt strategy, and measuring AI coding impact.',
    1772438400, 'ai', '/ai/roundups/weekly-ai-roundup-2026-03-02', 'TechHub',
    'TechHub', 'AD94D9A752DFAB1E68453ECE832423D56C52F007A6164383A115A07C0CDCC2A3', ',Microsoft Copilot Studio,Computer Using Agents,Agentic AI,Azure OpenAI,Microsoft Foundry,Confidential AI,Hybrid AI,Edge AI,Azure AI Infrastructure,NVIDIA GTC,.NET,Microsoft.Extensions.VectorData,Retrieval Augmented Generation,MCP,Model Router,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-02-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-02-23', 'roundups', 'Weekly AI Roundup: Agent Frameworks, Foundry Models, VS Code',
    'This week, AI updates include new tools, frameworks, and guidance for implementing agent-based systems, multimodal applications, and workflow integration across Microsoft Foundry, VS Code, and Azure. Progress continues on cross-language agent frameworks, enterprise modeling, and affordable deployment, all supporting faster, more flexible development spanning industries such as healthcare, telecom, and business software. Live events and guides make these advancements easier to adopt and use in practice.
<!--excerpt_end-->
## Microsoft Agent Framework: Cross-Language Orchestration and Migration
Microsoft Agent Framework is now a Release Candidate, providing cross-language orchestration for AI agents in .NET and Python. The framework delivers a unified way to build agents, type-safe APIs, and graph-based workflow support, working with major providers like Microsoft Foundry, Azure OpenAI, OpenAI, GitHub Copilot, Anthropic Claude, AWS Bedrock, and Ollama. Features include orchestration packages, migration guides, and support for standards like MCP, A2A, and AG-UI. Upgrading from Semantic Kernel or AutoGen is described in dedicated resources, with community support for adoption.
This milestone completes the journey from initial previews to a unified platform for managing .NET and Python agents, building on momentum toward more standardized agent management.
- [Microsoft Agent Framework Release Candidate: Cross-Language AI Agent Orchestration for .NET and Python](https://devblogs.microsoft.com/foundry/microsoft-agent-framework-reaches-release-candidate/)
- [Migrating to Microsoft Agent Framework Release Candidate: A Guide for Semantic Kernel and AutoGen Users](https://devblogs.microsoft.com/semantic-kernel/migrate-your-semantic-kernel-and-autogen-projects-to-microsoft-agent-framework-release-candidate/)
## Microsoft Foundry: Model, SDK, and Platform Updates
Microsoft Foundry brings in updated AI models and SDKs, including GPT-5.2, GPT-5.1 Codex Max, Mistral Large 3, DeepSeek V3.2, Kimi-K2 Thinking, Cohere Rerank 4, and enhanced image and audio generators. Serverless fine-tuning, persistent agent memory, and improved agent-to-agent communication support stronger workflow integration and visualization. Managed Foundry MCP Server now offers secure endpoints for running models, with a unified portal, improved VS Code integration, and updated SDKs for easier project standardization. Developers are encouraged to migrate from the AzureML SDK v1 in advance of retirement, with resources available for migration and deprecation. These features improve workflow flexibility and multimodal project support.
This release builds on earlier efforts to integrate advanced models and align SDK experiences across different programming languages, making it easier to develop and migrate projects.
- [What’s New in Microsoft Foundry: Models, SDKs, and Platform Updates (Dec 2025 – Jan 2026)](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-dec-2025-jan-2026/)
## Building Practical Solutions with Microsoft Foundry Local AI
Foundry Local AI documentation showcases privacy-focused workflows, such as a smart building HVAC digital twin that simulates real-world HVAC systems using modular design and KPIs with 3D visualization. Features include debugging, Copilot integration, and ready-to-run deployments with BACnet, Modbus, and MQTT connections. Another guide reviews privacy-aware medical transcription with OpenAI Whisper and ASP.NET Core 10, covering secure deployment, API design, compliance, electronic health record (EHR) integration, and production roll out for healthcare.
These examples provide deeper, enterprise-ready blueprints for using privacy-first, local and modular AI in regulated environments such as healthcare and facilities management.
- [Building a Smart Building HVAC Digital Twin with AI Copilot Using Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-smart-building-hvac-digital-twin-with-ai-copilot/ba-p/4490784)
- [Building HIPAA-Compliant Medical Transcription with Microsoft Foundry Local AI](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-hipaa-compliant-medical-transcription-with-local-ai/ba-p/4490777)
## Microsoft Foundry: Frontier Model Integration and Reasoning Agents Challenges
Claude Sonnet 4.6 is now in Foundry, featuring Opus-class reasoning with a beta 1 million-token context window and adaptive controls. New browser automation, search, summary, and compliance tools are available for enterprise AI. The Agents League challenge invites teams to build reasoning agent solutions, with starter kits, online sessions, and prize competitions for orchestration and integration. MCP and SDK-driven environments enable both programmatic and visual control, providing flexibility to experiment with practical agent-based designs.
Claude Sonnet’s introduction and the Agents League demonstrate continued growth in model diversity and hands-on adoption through challenges.
- [Claude Sonnet 4.6 Now Available in Microsoft Foundry: Frontier AI Performance for Enterprise Scale](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/claude-sonnet-4-6-in-microsoft-foundry-frontier-performance-for-scale/4494873)
- [Agents League: Build Reasoning Agents with Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-join-the-reasoning-agents-track/ba-p/4494394)
## Agentic AI Workflows and Integration in Visual Studio Code
VS Code introduces more agent-oriented features, supporting orchestration, team collaboration, and background work assignment, as well as real-time security and workflow metrics for team environments. Events highlight agent coordination and monitoring with actionable telemetry. Tutorials cover everything from prompt-based prototyping and live workflow demos to building AI-powered projects with little to no code.
These improvements follow recent enhancements to SDK support and workflow monitoring, providing secure and versatile agent-driven development tools for all experience levels.
- [VS Code Live: Agent Sessions Day - Keynote](/ai/videos/vs-code-live-agent-sessions-day-keynote)
- [How VS Code Builds with AI](/ai/videos/how-vs-code-builds-with-ai)
- [VS Code Live: Agent Sessions Day – AI and Agentic Development in Visual Studio Code](/ai/videos/vs-code-live-agent-sessions-day-ai-and-agentic-development-in-visual-studio-code)
- [Getting Started with Agents in VS Code](/ai/videos/getting-started-with-agents-in-vs-code)
## Affordable AI Development and Practical Tutorials
The Budget Bytes series provides step-by-step guides to build working AI applications on Azure for less than $25, covering topics like inventory control and insurance. Each tutorial comes with sample code (GitHub repos), deployment steps, and Azure SQL trials so that teams can manage costs and debug issues easily when starting or prototyping with large AI projects.
With a focus on hands-on learning and transparent costs, these resources help new teams build practical AI solutions.
- [Budget Bytes: Building Real AI Apps on Azure for Under $25](https://devblogs.microsoft.com/azure-sql/introducing-budget-bytes/)
## Telecom Infrastructure, Agentic BSS, and Intelligent Edge
Telecom solution updates analyze AI’s role in enhancing mobile networks and business support systems. AI improves radio access networks with spectrum management and anomaly detection—powered by Azure and Foundry. Open RAN projects and the Janus initiative offer orchestration and support for global testing. Guides for agent-driven business support use Copilot Studio and MCP to automate telecom billing, quoting, and other tasks through APIs and rapid prototype workflows. Mobile World Congress sessions walk developers through live agent-building.
Industry adoption and workflow integration remain in focus as telecom projects highlight AI and agent tools for practical transformation.
- [Microsoft’s Vision: AI-RAN and Intelligent Edge Transforming Telecom Networks](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/ai-powered-ran-and-the-intelligent-edge-microsoft-s-vision-for/ba-p/4495554)
- [The Rise of Agentic BSS in the IQ Era: From Systems of Record to Systems of Outcome](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/the-rise-of-agentic-bss-in-the-iq-era-from-systems-of-record-to/ba-p/4495499)
## Developer Workflow and Software Practices in the Age of AI Coding
Martin Fowler’s session reinforces the importance of test-driven development (TDD) in a world where AI now writes both code and tests. TDD still provides discipline, reduces risk, and ensures code quality when agent outputs can be unpredictable. With AI’s help, security and review cycles increase in importance, and teams may need to shift from manual coding bottlenecks to stronger coordination. Fowler advises keeping Agile principles and adapting them to fit AI-driven and agent-rich workflows.
This practical guidance underscores how long-standing engineering methods still matter as AI changes processes.
- [Test-Driven Development Remains Essential in the Age of AI Coding](https://www.devclass.com/development/2026/02/21/should-there-be-a-new-manifesto-for-ai-development/4091612)
## Other AI News
SQUAD, an open-source orchestration framework for .NET, provides design patterns and building blocks for .NET and Azure teams to create AI agent teams for cloud and hybrid projects.
The trend toward agent frameworks for .NET continues, making orchestration and team-based build patterns more approachable.
- [.NET AI Community Standup: SQUAD – AI Agent Teams for C# Projects](/ai/videos/net-ai-community-standup-squad-ai-agent-teams-for-c-projects)
The Imagine Cup semifinalists showcase AI-based solutions for accessibility, healthcare, education, and inventory, built by students on Azure using cognitive services, generative models, and cloud integration.
These projects show how agent workflows and AI apps are spreading across sectors like health, education, and enterprise, as highlighted in last week’s news.
- [AI in Action: Meet the 2026 Imagine Cup Semifinalists](https://techcommunity.microsoft.com/blog/studentdeveloperblog/ai-in-action-meet-the-2026-imagine-cup-semifinalists/4495567)
A March 5 webinar on cost-efficient scaling of Azure AI agents gives advice on practice, model selection, financial optimization, and governance frameworks. It includes examples and Q&A to address common enterprise concerns.
The financial guidance thread continues, with a focus this week on controlling costs for larger deployments as agent adoption grows.
- [Upcoming Webinar: Maximize the Cost Efficiency of AI Agents on Azure](https://techcommunity.microsoft.com/t5/microsoft-developer-community/upcoming-webinar-maximize-the-cost-efficiency-of-ai-agents-on/ba-p/4493923)
Enterprise architecture analysis shows that while AI can automate some documentation and basic cost estimation, solution architects are still needed for strategic design and client communication. Developers are advised to keep upskilling as AI complements their roles—not replaces them.
The conversation about AI as an augmentation tool (rather than a replacement) continues from last week, supporting blended teams working with AI/agent frameworks.
- [Can the Solution Architect Role Be Replaced by AI?](https://dellenny.com/can-the-solution-architect-role-be-replaced-by-ai/)',
    'This week, AI updates include new tools, frameworks, and guidance for implementing agent-based systems, multimodal applications, and workflow integration across Microsoft Foundry, VS Code, and Azure. Progress continues on cross-language agent frameworks, enterprise modeling, and affordable deployment, all supporting faster, more flexible development spanning industries such as healthcare, telecom, and business software. Live events and guides make these advancements easier to adopt and use in practice.',
    1771833600, 'ai', '/ai/roundups/weekly-ai-roundup-2026-02-23', 'TechHub',
    'TechHub', 'C22B8DF9E33CC19D296C09ECF2A3E7506374BFD88A4B92D084CE741CBA115EF1', ',Microsoft Agent Framework,AI Agents,.NET,Python,Microsoft Foundry,Azure OpenAI,MCP,Agent Orchestration,Multimodal AI,Fine Tuning,VS Code,Claude Sonnet,GPT 5.2,Edge AI,Cost Optimization,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-02-16
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-02-16', 'roundups', 'Weekly AI Roundup: Agent Tooling, Local Foundry, and Governance',
    'This week’s AI news highlights agent developer tooling, enterprise data integration, and continued expansion of Microsoft’s agent framework. You’ll find new updates for the AI Toolkit in VS Code, local agent development with Foundry, and broader coverage of governance and educational use.
<!--excerpt_end-->
## AI Agent Development and Orchestration in the Microsoft Ecosystem
AI Toolkit for VS Code (v0.30.0) now features a single Tool Catalog for easy agent tool management. The new Agent Inspector lets developers set breakpoints, examine variables, debug, and visualize workflow steps all in one place. Unit tests use pytest syntax and the Eval Runner SDK, running outputs through Data Wrangler and scaling up with Foundry. Model support is being extended (including gpt-5.2-codex), and productivity tools help bring agent workflows directly into standard development.
This continues last week’s focus on unifying the agent automation experience, from initial Copilot traces to full workflow management within VS Code. Greater support for agent coordination, strict policy controls, and human checkpoints is now easier for everyday developers to use.
For teams with privacy requirements, new guidance explores how Foundry Local and the Microsoft Agent Framework (MAF) enable local, research-grade agent workflows. Features include modular composition, OpenTelemetry for observability, built-in security practices (such as red teaming and privacy-by-design), and debugging with DevUI and .NET Aspire instrumentation. The articles provide practical instructions for building policy-compliant, cost-managed AI agents.
These improvements build on last week’s up-to-date model deployments with audit trails, continuing to prioritize flexibility and compliance.
An in-depth post walks through the Microsoft Learn MCP Server, released in 2025, which provides programmatic access to Microsoft Learn docs and code samples for agents. Architecture decisions like using Azure App Service, semantic and vector search, and distributed tool management support improved agent data retrieval and secure automation workflows. This extends last week’s trend of feeding current technical reference into Copilot, reinforcing Microsoft’s investment in agent awareness and developer experience.
- [AI Toolkit for VS Code: February 2026 Major Update (v0.30.0)](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-toolkit-for-vs-code-february-2026-update/ba-p/4493673)
- [Building Deep Research Agent Workflows with Microsoft Foundry Local and Agent Framework](https://devblogs.microsoft.com/semantic-kernel/from-local-models-to-agent-workflows-building-a-deep-research-solution-with-microsoft-agent-framework-on-microsoft-foundry-local/)
- [How We Built the Microsoft Learn MCP Server: Empowering AI Agents with Trusted Documentation](https://devblogs.microsoft.com/engineering-at-microsoft/how-we-built-the-microsoft-learn-mcp-server/)
## Local AI Integration, Gamification, and SDK Parity
Foundry Local is in use for privacy-focused, lightweight local AI integration—such as enabling in-game personality features in browser games, powered by models like phi-3.5-mini and event-driven JavaScript. The tools support asynchronous, pluggable functionality, offer offline support, and are customizable for low-cost development.
These efforts build on last week’s advancements in local evaluation and open infrastructure for building AI-powered features with privacy and cost control.
Gamified learning is now part of AI education. There’s a blueprint for creating browser and CLI learning environments on Foundry Local, featuring prompt engineering, workflow modules, and tool development in JavaScript, ES6 modules, and Node.js, offering a structured entry point for students and new developers.
Enterprise users of Azure AI Foundry should note that Python SDK supports agent memory out of the box, while C# does not have feature parity yet. .NET developers are encouraged to monitor the SDK roadmap and custom solutions until built-in support becomes available.
- [Adding AI Personality to Browser Games with Microsoft Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/adding-ai-personality-to-browser-games/ba/p/4490892)
- [Teaching AI Development Through Gamification: Building with Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/teaching-ai-development-through-gamification/ba-p/4490755)
- [Agent Memory Abstractions in Azure AI Foundry: Python vs C# SDKs](https://techcommunity.microsoft.com/t5/azure/missing-equivalent-for-python-memorysearchtool-and/m-p/4494284#M22429)
## AI-Driven Enterprise Data Intelligence and Agentic Workflows
Microsoft IQ and the Fabric IQ platforms support agent-driven integration for enterprise data. New materials show how modules like Work IQ, Foundry IQ, and Fabric IQ automate workflow management, model integration, and data discovery for Azure-based companies.
The Fabric IQ Agents platform includes both a semantic ontology and a Fabric Graph, allowing organizations to use natural language to explore and automate work with live data. Security and compliance are managed by Entra ID and Fabric policies. Integration hooks for analytics and operations mean teams can build custom and secure workflows, blending no-code and advanced developer controls.
This information builds on last week’s coverage of workflow automation and strict AI governance, showing how agent platforms now connect to knowledge and policy across larger organizations.
- [Microsoft IQ Overview: Exploring Work IQ, Foundry IQ, and Fabric IQ](/microsoft-iq-overview-exploring-work-iq-foundry-iq-and-fabric-iq)
- [Fabric IQ Agents: Bridging Enterprise Data and AI](https://zure.com/blog/fabric-iq-agents-operate-hand-to-hand-with-enterprise-data)
## Observability, Governance, and Security in Agent Deployments
At enterprise scale, deploying AI agents reveals ongoing challenges around observability, governance controls, and policy enforcement. Microsoft’s Cyber Pulse report encourages adopting Zero Trust: least privilege, real-time monitoring, and clearly defined roles. Agent management covers registry, identity, telemetry, policy compatibility, and security. Real-world scenarios include integrating Copilot Studio or Agent Builder with existing enterprise tooling, using registries for transparency and runtime checks for compliance. Technical guides help teams deliver solutions that are secure, observable, and policy aligned.
This mirrors last week’s emphasis on trackable and audit-ready agent deployments, moving from test environments to steady-state enterprise systems.
- [AI Agents in the Enterprise: Observability, Governance, and Security Insights from Microsoft''s Cyber Pulse Report](https://www.microsoft.com/en-us/security/blog/2026/02/10/80-of-fortune-500-use-active-ai-agents-observability-governance-and-security-shape-the-new-frontier/)
## Workflow Productivity, Semantic Search, and AI Evaluation
The TypeScript team continues to automate repetitive work (like porting PRs) with AI, promoting automation to help other teams free up cycles and improve workflow reliability.
GitHub’s public preview of semantic search for Issues offers context-based search results, helping projects track bugs and features more efficiently.
The GPT-5 pro Evaluation Challenge demonstrates how to build hands-on AI evaluation pipelines with Foundry and Azure AI, including guides and examples for setting up quality workflows.
Low-code and no-code tools for AI are gaining ground—recent programming sessions show designers and developers building apps directly in VS Code with AI agents, using agent steering, multi-agent management, and mobile integration for practical development.
- [How TypeScript''s Creator Uses AI for Team Productivity](/how-typescripts-creator-uses-ai-for-team-productivity)
- [GitHub Issues Semantic Search Public Preview](/github-issues-semantic-search-public-preview)
- [GPT-5 pro Evaluation Challenge – Evaluating AI Tools with Microsoft Foundry and Azure AI](/gpt-5-pro-evaluation-challenge-evaluating-ai-tools-with-microsoft-foundry-and-azure-ai)
- [Building an App in VS Code with AI Agents with Elijah King | Cozy AI Kitchen](/building-an-app-in-vs-code-with-ai-agents-with-elijah-king-cozy-ai-kitchen)',
    'This week’s AI news highlights agent developer tooling, enterprise data integration, and continued expansion of Microsoft’s agent framework. You’ll find new updates for the AI Toolkit in VS Code, local agent development with Foundry, and broader coverage of governance and educational use.',
    1771228800, 'ai', '/ai/roundups/weekly-ai-roundup-2026-02-16', 'TechHub',
    'TechHub', '3E2144E50911C62342D4444CBBEA04B3A168B8E9BCD10F647824F68DE30326D5', ',AI Agents,Microsoft Agent Framework,Azure AI Foundry,Foundry Local,VS Code,AI Toolkit,Agent Debugging,Observability,OpenTelemetry,Zero Trust Security,AI Governance,Semantic Search,Vector Search,Evaluation Pipelines,Microsoft Fabric,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-02-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-02-09', 'roundups', 'Weekly AI Roundup: Safe Agents, Foundry, and Measurable AI Ops',
    'This week’s AI highlights include new cloud automation resources, practical development guides, and accessible tools for deploying, auditing, and monitoring AI in the enterprise. Topics center on production implementation—safe agent workflows, integrated hardware, and balancing new features with transparency.
<!--excerpt_end-->
## Azure AI Services and Foundry Ecosystem
Azure now hosts Anthropic''s Claude Opus 4.6 in Foundry, giving developers access to agent workflows and embedded automation features. Claude Opus 4.6 supports complex reasoning, a large context window (beta), and detailed deployment controls useful for projects requiring compliance, refactoring, or secure document handling. Copilot Studio integration helps organizations scale agent use with proper review and oversight.
Building on last week’s protocol and workflow updates, Azure’s AI strategy emphasizes practical adoption—developer docs and new Maia 200 hardware events mark ongoing infrastructure support. Teams can deploy models on updated AI hardware and follow practices for secure, monitored automation.
Architecture guides describe how to create traceable Copilot workflows with strict permissions, audit trails, human-in-the-loop steps, safe API design, and Azure-based monitoring. With these best practices, teams can automate key tasks using Copilot Studio, Power Automate, and Graph APIs as covered in earlier governance news.
Maia 200, a new AI hardware accelerator optimized for Azure, offers scalable deployment for inference tasks. Technical content and demos, released at ISSCC 2026, as well as usage in both internal and public Foundry, support infrastructure and development teams with practical examples.
A reference on observability for generative AI details systematic evaluation strategies, including objective selection, dataset configuration, metrics, and regular risk checks. Developers receive step-by-step process advice for frequent audits—covering integration, cost, regional policy, and tech—reflecting last week’s evaluation baseline.
- [Claude Opus 4.6 Now Available in Microsoft Foundry: Advanced Agentic AI for Coding, Security, and Enterprise Workflows](https://azure.microsoft.com/en-us/blog/claude-opus-4-6-anthropics-powerful-model-for-coding-agents-and-enterprise-workflows-is-now-available-in-microsoft-foundry-on-azure/)
- [Designing Safe and Scalable Agentic Workflows with Microsoft Copilot](https://dellenny.com/designing-safe-agentic-workflows-with-microsoft-copilot/)
- [Introducing Maia 200: Microsoft’s Next-Generation AI Inference Accelerator](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/join-microsoft-as-we-share-more-on-maia-200-in-the-bay-area/ba-p/4492370)
- [Observability in Generative AI: Building Trust with Systematic Evaluation in Microsoft Foundry](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/observability-in-generative-ai-building-trust-with-systematic/ba-p/4492231)
## AI-Powered App Development with .NET and MCP
.NET app builders now have more guidance for adding AI features to web and enterprise projects. The current ASP.NET Community Standup covers using Progress Telerik AI controls with Blazor and MCP, showcasing generators for UI and automated scaffolding.
Expanding on last week''s new .NET AI features, advice is shifting from best practices to end-to-end deployment for all environments. The .NET ecosystem is emphasizing privacy, efficiency, and support for hybrid approaches, as outlined in the new "Foundry Local for C# devs" resource.
A comprehensive lesson describes how to build modular, reusable AI Skills Executors in .NET, using Azure OpenAI and MCP. This helps teams split skills (YAML-based prompts, toolsets) from orchestration code, improving flexibility, testability, and implementation for cases like code analysis or project tracking. The skills-first architecture also enables smooth rollout and ongoing monitoring.
- [Build AI‑Powered .NET Apps with Telerik](/build-aipowered-net-apps-with-telerik)
- [.NET AI Community Standup: Foundry Local for C# Developers](/net-ai-community-standup-foundry-local-for-c-developers)
- [Building an AI Skills Executor in .NET with Azure OpenAI and MCP](https://devblogs.microsoft.com/foundry/dotnet-ai-skills-executor-azure-openai-mcp/)
## Local AI Model Benchmarking and Scientific Evaluation
Developers can now test local AI models more reliably using tools built from the Foundry Local and FLPerformance SDK. These let teams measure speed, throughput, and resource usage, giving detailed comparison dashboards in React. Guidance includes hardware recommendations, how to design custom test suites, ways to reduce measurement noise, and links to open source projects for quick setup.
This reflects last week’s coverage of scalable measurement tools and evaluation frameworks within Foundry.
- [Scientific Benchmarking of Local AI Models with Foundry Local and FLPerformance](https://techcommunity.microsoft.com/t5/microsoft-developer-community/benchmarking-local-ai-models/ba-p/4490780)
## Practical AI: Continuous Automation and Safe API Workflows
End-to-end AI automation can now be adopted in CI/CD through generative agents—new guides show how agents can check code, write reports, and create documentation, converting plain-language instructions to CI tasks with GitHub Actions. Security and transparency are emphasized; teams can test these patterns incrementally using GitHub Next’s sample projects. This follows last week’s focus on managed workflow context for AI pipelines.
Another guide shares patterns for building reliable APIs using language models for intent and entity extraction, while ensuring business logic and validation stay deterministic. Libraries like LangGraph handle confidence thresholds, schema checking, and clarification, keeping APIs robust and fast despite using LLMs.
- [Continuous AI in Practice: Bringing Judgment-Based Automation to Code Repositories](https://github.blog/ai-and-ml/generative-ai/continuous-ai-in-practice-what-developers-can-automate-today-with-agentic-ci/)
- [How to Build Safe Natural Language-Driven APIs](https://techcommunity.microsoft.com/t5/microsoft-developer-community/how-to-build-safe-natural-language-driven-apis/ba-p/4488509)
## AI-Powered Image Generation with Serverless Azure Functions
A how-to tutorial explains using Stable Diffusion with Azure Functions on cloud GPUs to set up serverless image generation. The article outlines building and deploying a Python container, configuring scalable compute, and integrating with CLI automation for more consistent delivery. Troubleshooting tips, billing considerations, and next steps for UI integration or custom model use are included.
- [Build an AI Image Generator with Azure Functions and Serverless GPUs](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/serverless-gpu-tutorial-build-an-ai-image-generator-with-azure/ba-p/4492228)',
    'This week’s AI highlights include new cloud automation resources, practical development guides, and accessible tools for deploying, auditing, and monitoring AI in the enterprise. Topics center on production implementation—safe agent workflows, integrated hardware, and balancing new features with transparency.',
    1770624000, 'ai', '/ai/roundups/weekly-ai-roundup-2026-02-09', 'TechHub',
    'TechHub', 'D561DBE3442CFBD32998C7B321A9B0503EF664A55943AD87C4DB7ECD59CFA1DB', ',Azure AI Foundry,Anthropic Claude Opus 4.6,Microsoft Copilot Studio,Agentic Workflows,Human in The Loop,AI Governance,Observability,Generative AI Evaluation,Maia 200,Azure OpenAI,MCP,ASP.NET Blazor,Foundry Local,Benchmarking,GitHub Actions,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-02-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-02-02', 'roundups', 'Weekly AI Roundup: Maia 200, Agent Protocols, and .NET AI',
    'This week’s AI updates include new hardware, integration tools, and workflow enhancements, with a focus on agent protocols, orchestration, and stateful AI in developer tooling. Microsoft’s Maia 200 accelerator expands Azure’s AI hardware portfolio, and developers gain unified .NET AI integration tools, updated model orchestration, and sustainability practices.
<!--excerpt_end-->
## Maia 200 AI Accelerator: Launch, Architecture, and Developer Impact
Microsoft introduced the Maia 200, a new AI inference accelerator built for large Azure workloads. It offers roughly 30% better inference per dollar and clusters scale up to 6,144 chips. Key features include 140B transistors at TSMC 3nm, FP4/FP8/FP6 tensor cores, 216GB HBM3e at 7TB/s, and liquid cooling.
Maia SDK previews provide Triton compiler integration, PyTorch support, NPL programming, a simulator, and tools for model migration. Azure-native orchestration, monitoring, and benchmarking are also included. The Maia 200 sees active use in Microsoft’s infrastructure, powering large model deployments and supporting external frameworks and OpenAI models.
Resources for SDKs and model tuning support various inference requirements, and developers can now prepare for more optimized AI environments on Azure.
- [Maia 200: The AI Accelerator Built for Inference](https://blogs.microsoft.com/blog/2026/01/26/maia-200-the-ai-accelerator-built-for-inference/)
- [Deep Dive into the Maia 200 AI Inference Accelerator Architecture](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/deep-dive-into-the-maia-200-architecture/ba-p/4489312)
- [Introducing Maia 200: The AI Accelerator Built for Inference](https://news.microsoft.com/january-2026-news)
- [Microsoft Launches Maia 200 AI Accelerator on Azure for Enhanced Performance and Efficiency](https://www.linkedin.com/posts/satyanadella_our-newest-ai-accelerator-maia-200-is-now-activity-7421583368754110465-tXQM)
## Model Context Protocol, Agent UIs, and Agentic Workflows in VS Code
VS Code has added public preview support for Model Context Protocol (MCP) Apps, its first official extension supporting AI agents with interactive UI components in the chat panel. Integrations with partners such as Storybook enable richer, practical UIs for AI in the IDE.
This continues last week''s story on agent interoperability, now moving toward UI-driven agent flows inside VS Code. The Agent Sessions Day event showcased open source community extensions and AI-driven workflows that help streamline common development tasks.
- [Giving Agents a Visual Voice: MCP Apps Support in VS Code](https://code.visualstudio.com/blogs/2026/01/26/mcp-apps-support)
- [VS Code Live: Agent Sessions Day – Exploring AI and Agentic Development](/ai/videos/vs-code-live-agent-sessions-day-exploring-ai-and-agentic-development)
## Microsoft Agent Framework, Multi-Agent Orchestration, and UI Integration
Multi-agent support has expanded in Microsoft Agent Framework (MAF). A practical guide explains integrating the Claude Agent SDK with MAF, enabling combined workflows using Claude, Azure OpenAI, and Copilot (in Python). Useful features like permissions, function registration, async orchestration, and session management are described, including agent reviewer flows.
Updates include open event-driven protocol support via AG-UI, allowing Python/FastAPI/Azure OpenAI to handle streaming, observable workflows, and interoperable interfaces across agent architectures.
- [Integrating Claude Agent SDK with Microsoft Agent Framework for Advanced AI Agents](https://devblogs.microsoft.com/semantic-kernel/build-ai-agents-with-claude-agent-sdk-and-microsoft-agent-framework/)
- [Building Interactive Agent UIs with AG-UI and Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-interactive-agent-uis-with-ag-ui-and-microsoft-agent/ba-p/4488249)
## .NET AI Integration and Stateful Conversational Patterns
".NET AI Essentials" introduces the Microsoft.Extensions.AI (MEAI) library, which provides a unified interface for OpenAI, Azure OpenAI, and OllamaSharp, supporting adapters, output parsing, streaming, and multi-modal requests. Dependency injection and middleware are built in. This “building blocks” design helps you bring standard, provider-agnostic AI features into .NET apps, closely linked with Semantic Kernel and Aspire.
Detailed guides show how to move from stateless to stateful AI interactions within .NET, including examples for managing stateful assistants that adapt over multiple sessions.
- [.NET AI Essentials: Unified Building Blocks for Intelligent Apps](https://devblogs.microsoft.com/dotnet/dotnet-ai-essentials-the-core-building-blocks-explained/)
- [Beyond the Prompt: Designing Stateful AI Experiences in .NET](/ai/videos/beyond-the-prompt-designing-stateful-ai-experiences-in-net)
## Agentic Systems, Platform Governance, and Responsible Integration
Microsoft is focusing on balancing intelligence and trust across enterprise agent architectures (Agent 365, Foundry IQ, Fabric, and 365 Copilot). The strategy emphasizes unified observability, governance, and workflow integrations for secure, responsible deployment at scale.
Building from last week''s discussion of workflow specs and sector adoption, the story now includes examples from healthcare, manufacturing, and finance. Supporting tools like Agent Factory and Copilot integration improve deployment, governance, and automation.
- [How Microsoft is Empowering Frontier Transformation with Intelligence + Trust](https://blogs.microsoft.com/blog/2026/01/27/how-microsoft-is-empowering-frontier-transformation-with-intelligence-trust/)
## Other AI News
On AI-powered robotics, Microsoft Cozy AI Kitchen explores moving from scripted robots to generative systems by using action tokens and natural language for more adaptive machines. Integration with Azure AI and Teams shows how to build collaborative robotics.
Last week’s coverage of physical AI continues with guides and case studies on real-world agentic applications.
- [Inside the Future of AI‑Powered Robotics with Tim Chung | Cozy AI Kitchen](/ai/videos/inside-the-future-of-aipowered-robotics-with-tim-chung-cozy-ai-kitchen)
A step-by-step guide to building a free AI-powered inventory manager uses Azure Cognitive Services and .NET to help automate grocery tracking, showing accessible paths for AI automation.
- [Building a Free AI-Powered Inventory Manager with Azure](/ai/videos/building-a-free-ai-powered-inventory-manager-with-azure)
Sustainability is addressed this week in a practical guide for aligning AI with environmental goals. Concrete tips cover moving workloads to energy-efficient infrastructure, optimizing processes, and selecting AI models that match business and sustainability needs. Case studies demonstrate cost and energy reductions.
- [5 Essential Practices to Align AI Transformation with Sustainability](https://www.microsoft.com/en-us/microsoft-cloud/blog/2026/01/28/beyond-davos-2026-5-practices-to-align-ai-transformation-and-sustainability/)',
    'This week’s AI updates include new hardware, integration tools, and workflow enhancements, with a focus on agent protocols, orchestration, and stateful AI in developer tooling. Microsoft’s Maia 200 accelerator expands Azure’s AI hardware portfolio, and developers gain unified .NET AI integration tools, updated model orchestration, and sustainability practices.',
    1770019200, 'ai', '/ai/roundups/weekly-ai-roundup-2026-02-02', 'TechHub',
    'TechHub', '1C39924FCD326ACCECFC71BC735364979E1DA736B2E86A25C2CFE4AC0B826D14', ',AI,Azure,Maia 200,AI Inference,Accelerators,TSMC 3nm,HBM3e,MCP,MCP Apps,VS Code,AI Agents,Microsoft Agent Framework,Multi Agent Orchestration,AG UI,.NET,Microsoft.Extensions.AI,Azure OpenAI,Semantic Kernel,Stateful AI,Responsible AI,Observability,Governance,Sustainability,Robotics,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-01-26
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-01-26', 'roundups', 'Weekly AI Roundup: Agent Orchestration, Open APIs, Real Use Cases',
    'AI platform news this week highlights new agent orchestration tools and real-world automation scenarios in areas such as healthcare, law, and retail. Microsoft continues developing orchestration and security patterns. Best practices and open-source agent projects support teams looking to deliver practical AI solutions and reliable products.
<!--excerpt_end-->
## Microsoft Agent Framework, Foundry, and Agentic Orchestration
Microsoft’s Agent Framework (Python/.NET) reaches deeper into enterprise infrastructure, with Windows 365 for Agents providing secure, flexible cloud PCs for agent deployments beside human users. The new Agent 365 APIs and SDKs add robust and modular orchestration for scaling, policies, and automation. Integration with Azure, Entra ID, Intune, and added capabilities for credentials and observability all follow previous best-practices discussions. The Microsoft Foundry for VS Code introduces a workflow visualizer, resource tracking, and improved feedback loops for production-scale orchestration. Technical articles explore Foundry IQ for retrieval-augmented generation (RAG), context management, and troubleshooting memory, reinforcing the modular agent platform focus.
- [Windows 365 for Agents: Enabling Secure AI Cloud PCs](https://blogs.windows.com/windowsexperience/2026/01/22/windows-365-for-agents-the-cloud-pcs-next-chapter/)
- [The AI Agent Development Blueprint: From Design to Production with Microsoft Agent Framework](/ai/videos/the-ai-agent-development-blueprint-from-design-to-production-with-microsoft-agent-framework)
- [Microsoft Foundry for VS Code: January 2026 Update](https://techcommunity.microsoft.com/t5/microsoft-developer-community/microsoft-foundry-for-vs-code-january-2026-update/ba-p/4486132)
- [Deep Dive into Foundry IQ and Azure AI Search](/ai/videos/deep-dive-into-foundry-iq-and-azure-ai-search)
- [Context-Driven Development: Agent Skills for Microsoft Foundry and Azure](https://devblogs.microsoft.com/all-things-azure/context-driven-development-agent-skills-for-microsoft-foundry-and-azure/)
## Open Source Agent Interoperability and Best Practices
Open source agentic frameworks are moving forward, as discussed previously with the Model Context Protocol (MCP) and agent modularity. Angie Jones’s talk covers using Goose to build interoperable and trusted agents for developers and non-developers, supporting community and production adoption. Goose shows how Azure’s approach to open APIs and context supports broader agent patterns.
- [Angie Jones on AI Agents, Goose, and the Model Context Protocol (MCP) at GitHub Universe](/ai/videos/angie-jones-on-ai-agents-goose-and-the-model-context-protocol-mcp-at-github-universe)
## Specification-Driven and Contextual AI Development Workflows
The resource "From Vibe Coding to Spec-Driven Development" continues the discussion on test-driven and context-oriented agent workflows. This practical guide helps teams implement improved validation, automation, and error handling in agentic delivery. The content connects prior themes about prompt design and engineering rigor for reliable agent projects.
- [From Vibe Coding to Spec-Driven Development: Part 3 – Best Practices and Troubleshooting](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development-part3)
## Azure AI in Healthcare, Legal Practice, and Retail Scenarios
Azure collaborations reach practical use for healthcare (e.g., Parkinson’s care), legal technology (AI for India’s court system), and retail (autonomous robots). These examples extend previous themes of workflow modernization, while providing tips on compliance, privacy, cost efficiency, and deploying agents in context-sensitive environments.
- [How AI-Powered Collaborations Are Transforming Healthcare and Life Sciences](https://www.microsoft.com/en-us/startups/blog/how-ai-powered-collaborations-are-transforming-healthcare-and-life-sciences/)
- [How AI and Microsoft Azure Are Transforming Legal Practice in India](https://news.microsoft.com/source/asia/2026/01/21/code-of-law-how-ai-is-helping-indias-lawyers-work-faster/)
- [How Agentic AI Robots Are Transforming the Retail Store Experience](https://www.microsoft.com/en-us/microsoft-cloud/blog/2026/01/20/frontier-transformation-in-retail-how-agentic-ai-robots-are-redefining-store-experiences/)',
    'AI platform news this week highlights new agent orchestration tools and real-world automation scenarios in areas such as healthcare, law, and retail. Microsoft continues developing orchestration and security patterns. Best practices and open-source agent projects support teams looking to deliver practical AI solutions and reliable products.',
    1769414400, 'ai', '/ai/roundups/weekly-ai-roundup-2026-01-26', 'TechHub',
    'TechHub', '91513A0F9916858B8AF52EA2BF79425E196350DF0D1A99307B0216C57062B1CA', ',AI Agents,Agent Orchestration,Microsoft Agent Framework,Windows 365 For Agents,Agent 365 APIs,Microsoft Foundry,VS Code,Retrieval Augmented Generation,Azure AI Search,Context Management,Observability,MCP,Goose,Spec Driven Development,Azure AI,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-01-19
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-01-19', 'roundups', 'Weekly AI Roundup: Marketplace agents, security, and cost control',
    'Microsoft’s AI product ecosystem now offers enhanced integration, compliance controls, and cost management. Developers will find new tools for adoption at scale, stronger data security, cost-sensitive AI solutions, and modernized business workflows. The technical articles cover the ways Microsoft AI solutions meet business standards and compliance requirements—expanding on last week’s stories around modular and context-driven approaches and bringing more Marketplace-based deployment and lifecycle management resources.
<!--excerpt_end-->
## AI Adoption and Custom Agent Development on Microsoft Marketplace
The Microsoft Marketplace plays a main role in deploying AI at scale, bringing together models, code frameworks, and low-code solutions. Developers can now select from over 11,000 AI models and 4,000 agents/apps—including partner models from Anthropic, Cohere, Meta, OpenAI, NVIDIA—or make their own. The marketplace supports filtering on product, category, and business domain, letting users trial and adopt solutions under their Azure contract. Guides include best practices for integration with Azure/Microsoft 365, securing model links using Managed Identity, and tracking policy compliance and lifecycle. There are examples of embedding agents in Microsoft 365 Copilot, plus resources for managing the entire agent lifecycle at enterprise scale.
Compared to earlier coverage, organizations are now moving from trialing agent composition to using managed, secure solutions with compliance in mind.
- [Chart Your AI and Agent Strategy with Microsoft Marketplace](https://azure.microsoft.com/en-us/blog/design-your-ai-and-agent-strategy-with-microsoft-marketplace/)
## Securing and Streamlining Data with Azure AI: PII Redaction and Cost-Effective AI Apps
Azure AI Language PII Redaction provides solutions for protecting sensitive workflow data. Step-by-step guides show how to detect and mask different PII types to meet regulatory standards like GDPR and HIPAA. Video demos explain setup and tuning for use cases in finance, healthcare, and consumer applications.
For teams working on tight budgets, ‘Budget Bytes’ videos explain how to create powerful, Copilot-capable AI apps for less than $25 using Azure SQL Database, with examples including LLM data grounding, custom agent scenarios, RAG, and full stack development. Price tables and reusable code snippets help developers deploy quickly and stay on budget.
These topics build on earlier work around privacy in AI, giving developers ready-to-use options for work that’s both effective and cost-conscious.
- [Protect Sensitive Data with Azure AI Language PII Redaction](/ai/videos/protect-sensitive-data-with-azure-ai-language-pii-redaction)
- [Build Powerful AI Apps for Under $25 with Azure SQL Database](/ai/videos/build-powerful-ai-apps-for-under-25-with-azure-sql-database)
## Azure AI Model Integration: Troubleshooting, Prompt Fidelity, and Custom Workflows
Developers using new models like GPT-4o-mini in Azure AI Foundry have observed inconsistent output between the Playground UI and API calls, especially for classification jobs. The same settings and prompts sometimes produce different responses due to hidden prompts or preprocessing. This difference can affect reliability in deploying conversational agents, prompting teams to troubleshoot and document solutions for consistent behavior.
This ties directly into last week’s agent orchestration theme—underscoring the need for clear communication and transparency in production workflows.
- [Discrepancies Between Azure AI Foundry Playground and API Responses for GPT-4o-mini](https://techcommunity.microsoft.com/t5/azure/weird-problem-when-comparing-the-answers-from-chat-playground/m-p/4486090#M22407)
## Modernizing Industry Workflows: Azure AI in Healthcare Transcription and Analytics
A technical reference explains how healthcare providers can automate speech transcription and analytics with Azure AI. It combines Azure Speech Services for live and batch transcription (including speaker separation) with Azure Text Analytics for Health to extract clinical data. Advanced summarizations are handled by Azure OpenAI, producing FHIR JSON for Microsoft Fabric OneLake—helping with faster, more accurate clinical documentation and HIPAA-compliant data handling. Complete walk-throughs cover pipelines, automation with GitHub Actions, cloud resource tracking, and code samples, letting healthcare IT teams build and expand practical solutions quickly.
This continues the ongoing theme of adapting agent-driven automation for domains like retail logistics to core health information processing.
- [Modernizing Healthcare Transcription and Analytics with Azure AI](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/ai-transcription-text-analytics-for-health/ba-p/4486080)',
    'Microsoft’s AI product ecosystem now offers enhanced integration, compliance controls, and cost management. Developers will find new tools for adoption at scale, stronger data security, cost-sensitive AI solutions, and modernized business workflows. The technical articles cover the ways Microsoft AI solutions meet business standards and compliance requirements—expanding on last week’s stories around modular and context-driven approaches and bringing more Marketplace-based deployment and lifecycle management resources.',
    1768809600, 'ai', '/ai/roundups/weekly-ai-roundup-2026-01-19', 'TechHub',
    'TechHub', 'E2FD6B61BEFA92B09230F78DB8A20ABFE5B4F85BC425EBDAE1084EDA44176F65', ',Microsoft AI,Microsoft Marketplace,Azure AI Foundry,Azure OpenAI,GPT 4o Mini,Agents,Microsoft 365 Copilot,Managed Identity,Compliance,Policy Management,PII Redaction,GDPR,HIPAA,RAG,Azure SQL Database,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-01-12
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-01-12', 'roundups', 'Weekly AI Roundup: Local-First Agents and Multi-Agent Standards',
    'AI news continues the shift toward modular, production-ready agentic systems, with new resources for developers building context-aware, local-first agents and orchestration frameworks. This includes engineering guides for private studios, practical agent templates, open protocols, and multi-agent workflow practices in everyday coding and industry scenarios.
<!--excerpt_end-->
## Agentic AI on Microsoft Platforms: Frameworks, Protocols, and Implementation
Furthering past work with Agent Framework and context management, this week’s guide shows how to build local-first, privacy-focused podcast studios using Microsoft’s AI technology. Solutions use Python orchestration and edge deployments with Ollama, maintaining clear context boundaries and observability for agents.
For .NET developers, an updated guide details integration of Semantic Kernel, Microsoft AI Extensions, and OllamaSharp, outlining how interfaces like IChatClient support modular hybrid AI deployments locally and in the cloud.
The Agent-to-Agent Standard (A2AS) in .NET provides actionable steps for agent composability, including JSON-RPC 2.0, AgentCard metadata, and agent lifecycle management. These bring agent architectures closer to practical, standardized systems for reliable development.
- [Engineering a Local-First Agentic Podcast Studio with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/engineering-a-local-first-agentic-podcast-studio-a-deep-dive/ba-p/4482839)
- [Generative AI with Large Language Models in C# in 2026](https://devblogs.microsoft.com/dotnet/generative-ai-with-large-language-models-in-dotnet-and-csharp/)
- [Implementing the Agent-to-Agent (A2A) Protocol in .NET: A Practical Guide](https://techcommunity.microsoft.com/t5/microsoft-developer-community/implementing-a2a-protocol-in-net-a-practical-guide/ba-p/4480232)
## Patterns, Best Practices, and Orchestration for Multi-Agent AI
Multi-agent orchestration remains a priority, with new materials reviewing scalable architectures for collaborative agent automation. The Armchair Architects’ show analyzes Microsoft AutoGen, Semantic Kernel, and Agent Framework for resource scaling, security, and cost efficiency.
Topics include permissioning, security practices, and operational controls, building on last week''s focus on context management. Practical recommendations for phased experimentation parallel previous advice for deploying multi-agent solutions in business.
A review of application modernization further explores human-in-the-loop design for safe AI adoption, confirming the ongoing need for robust practices in updating existing technology.
- [Armchair Architects: Patterns and Best Practices for Multi-Agent AI Orchestration](/ai/videos/armchair-architects-patterns-and-best-practices-for-multi-agent-ai-orchestration)
- [The Realities of Application Modernization with Agentic AI: A 2026 Perspective](https://devblogs.microsoft.com/all-things-azure/the-realities-of-application-modernization-with-agentic-ai-early-2026/)
## Agentic AI Solutions in Retail and Supply Chain Automation
Microsoft launches updated agentic AI templates and retail automation tools, integrating Copilot Checkout and catalog enrichment agents in Azure and .NET environments. These fit a pattern of standardized automation and easy enterprise customization.
Workflow modularity supports mature enterprise tool practices and links internal engineering and customer-facing activities. Blue Yonder’s supply chain case study further exemplifies integration and transparency, building on previous themes around orchestration and business reliability for critical deployments.
- [Microsoft Launches Agentic AI Solutions to Transform Retail Automation and Personalization](https://news.microsoft.com/source/2026/01/08/microsoft-propels-retail-forward-with-agentic-ai-capabilities-that-power-intelligent-automation-for-every-retail-function/)
- [Store Operations That Scale: Turn Signals into Decisions](/ai/videos/store-operations-that-scale-turn-signals-into-decisions)
- [AI-Driven Agents Transforming Supply Chain Management at Blue Yonder](/ai/videos/ai-driven-agents-transforming-supply-chain-management-at-blue-yonder)
## Other AI News
Agent Skills in Visual Studio Code, previewed earlier in Copilot releases, now include expanded features. This allows more developers to author and test reusable automations within the IDE, highlighting the ongoing shift of customizable AI into daily development activities.
- [Introducing Agent Skills in VS Code](/ai/videos/introducing-agent-skills-in-vs-code)',
    'AI news continues the shift toward modular, production-ready agentic systems, with new resources for developers building context-aware, local-first agents and orchestration frameworks. This includes engineering guides for private studios, practical agent templates, open protocols, and multi-agent workflow practices in everyday coding and industry scenarios.',
    1768204800, 'ai', '/ai/roundups/weekly-ai-roundup-2026-01-12', 'TechHub',
    'TechHub', '6165170C36F6EDC978F53AFA72D9ED65BAE3DE37CD6B58B63D27B8BD723FF596', ',Agentic AI,Microsoft Agent Framework,Semantic Kernel,.NET,C#,Microsoft AI Extensions,Ollama,Edge AI,Local First AI,Multi Agent Orchestration,AutoGen,Agent To Agent Protocol,JSON RPC 2.0,Azure AI,VS Code,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2026-01-05
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2026-01-05', 'roundups', 'Weekly AI Roundup: Agent Context Engineering and Language Trends',
    'This AI section highlights continued momentum in deploying Microsoft Agent Framework, evolving agent skills for enterprise software reliability, and new context engineering practices for AI-powered SRE tools. The role of AI in language trends for developers is also examined.
<!--excerpt_end-->
## Context Engineering in Azure SRE Agent Development
Following last week’s overview of Microsoft Agent Framework for Azure SRE Agent automation, this technical review shares lessons in designing operational context for reliable AI agents. The Azure SRE Agent team describes methods for maintaining context boundaries, keeping track of state, and embedding guardrails for predictability—building on last week’s topics around production readiness and orchestration.
The article shifts the focus from diagrammatic design to real-world deployment. It explains hands-on guidance for structuring agent access, monitoring state, and ensuring operations within managed guardrails, situating these as practical steps to make enterprise agents dependable and supportable. The techniques serve organizations scaling agent use for critical workloads.
- [Context Engineering Insights from Building Azure SRE Agent](https://techcommunity.microsoft.com/t5/azure-sre-agent/yearinreview-insights-from-the-last-few-months-building-azure/m-p/4481823#M2)
## AI’s Impact on Programming Language Selection
This article offers a follow-up to recent discussions about workflows and language choice in AI development, with a focus on MCP and language-model compatibility. A new analysis from GitHub discusses how AI tooling is encouraging the use of statically typed languages, such as TypeScript, to improve reliability—a trend supported by recent Octoverse data showing increased adoption of those languages.
The video details the pattern: AI models are most effective with codebases that include clear type information, pushing developers to adopt languages that maximize AI’s benefit. It encourages teams to consider how language selection impacts tooling support and outcomes, especially with AI growing as a default part of the software development process.
- [How AI Influences Programming Language Selection](/ai/videos/how-ai-influences-programming-language-selection)',
    'This AI section highlights continued momentum in deploying Microsoft Agent Framework, evolving agent skills for enterprise software reliability, and new context engineering practices for AI-powered SRE tools. The role of AI in language trends for developers is also examined.',
    1767600000, 'ai', '/ai/roundups/weekly-ai-roundup-2026-01-05', 'TechHub',
    'TechHub', 'E0A61C116720CA74B541D4198740AD9567A07B365E2FEA1D6A9997E3F462D32C', ',AI,AI Agents,Microsoft Agent Framework,Azure SRE Agent,Site Reliability Engineering,Context Engineering,Operational Context,State Management,Guardrails,Enterprise Automation,Production Readiness,Agent Orchestration,AI Powered Developer Tools,Programming Languages,TypeScript,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-12-29
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-12-29', 'roundups', 'Weekly AI Roundup: Agent Frameworks, VS Code Skills, Azure Hosting',
    'The AI section highlights updated agent frameworks, enhanced developer experiences for conversational AI applications, and new tools for integrating semantic data. Key releases include Microsoft’s latest Agent Framework, new agent customization options in VS Code, simplified ChatGPT hosting on Azure, and the Fabric IQ semantic data layer. These updates expand agent features and provide better support for context-aware solutions.
<!--excerpt_end-->
## Microsoft Agent Framework: A Unified Approach to Enterprise AI Agents
Microsoft’s open-source Agent Framework is designed for building advanced AI agents and multi-agent systems, with support for both .NET and Python. The framework merges the stability and workflow features of Semantic Kernel with the modular design of AutoGen, making it straightforward to connect with LLMs, handle state, and integrate external tools. It focuses on maintainability, checkpointing, and human involvement, offering flexibility for long-running and complex agent operations in enterprise settings. Bringing together tools previously split across the ecosystem, Agent Framework streamlines agent development and operational support for both .NET and Python.
Continuing themes from Azure AI Foundry and its ecosystem, this framework brings orchestration, hosting, and model routing under a single solution. It combines methods like Durable Task Extension with orchestration models to support Microsoft’s efforts to unify agent development for cloud and enterprise use.
- [What Is Microsoft Agent Framework & Why Another Agent Framework?](/ai/videos/what-is-microsoft-agent-framework-and-why-another-agent-framework)
## Streamlining Agent Customization and Integration in Developer Tooling
Visual Studio Code now supports an Agent Skills feature, letting developers configure and manage agent abilities through markdown files directly in the editor. Skills can be switched on or off, providing flexible guidance to agents and simplifying context management. This streamlines the setup and tuning of agent capabilities in individual code projects.
Recent updates to Agent Skills and the Skills.md standard complement Copilot and Azure AI Foundry improvements introduced last week. The current focus is on integrated skills editing in VS Code, building on positive community feedback about consistent, shareable configuration. These enhancements further the effort to make agent customization accessible across toolsets.
- [Customizing AI Agents in VS Code with Agent Skills](/ai/videos/customizing-ai-agents-in-vs-code-with-agent-skills)
## Hosting and Scaling ChatGPT Apps with Azure Functions
Developers can now deploy ChatGPT applications more easily using Azure Functions, the MCP server model (FastMCP for Python), and the Azure Developer CLI for setup and authentication. The step-by-step guide walks through backend service configuration, custom endpoint integration (like weather data), and embedding chat metadata. Azure’s serverless hosting means automated scaling, pay-by-usage pricing, and simple connection to the ChatGPT App Directory. Included examples and developer-mode tools offer clear paths for Python users building and testing advanced chat apps without dedicated infrastructure. Planned improvements include stronger authentication and increased support for MCP extensions on Azure.
This matches recent trends around the Model Context Protocol and Azure AI Foundry’s model router, allowing developers to reliably link models, APIs, and backend resources for conversational agents. The walkthrough provides real code and approaches for running ChatGPT apps on Azure.
- [How to Host ChatGPT Apps on Azure Functions: Developer Guide](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-chatgpt-apps-on-azure-functions/ba-p/4480696)
## Enriching AI Agents with Semantic Data: Introducing Fabric IQ
Fabric IQ is a semantic modeling layer for Microsoft Fabric that helps agents understand business concepts and workflows. This abstraction layer maps raw data into entities and relationships, supports business logic with rules and validations, and enables monitoring through its ontology framework. The Fabric Graph allows for entity relationship mapping and supports both GQL and natural language queries, so agents can interact with business data more naturally. Full setup instructions and governance integration are included, making it easier for agents to incorporate organizational knowledge into their operation.
Building on prior discussions about Fabric’s AI-powered workflows, this update introduces direct semantic modeling to the process, which supports better analytics and agent operations based on real organizational data and rules.
- [Understanding Fabric IQ: The Semantic Layer in Microsoft Fabric](https://zure.com/blog/fabric-iq-the-new-semantic-layer-for-your-organizational-data)',
    'The AI section highlights updated agent frameworks, enhanced developer experiences for conversational AI applications, and new tools for integrating semantic data. Key releases include Microsoft’s latest Agent Framework, new agent customization options in VS Code, simplified ChatGPT hosting on Azure, and the Fabric IQ semantic data layer. These updates expand agent features and provide better support for context-aware solutions.',
    1766995200, 'ai', '/ai/roundups/weekly-ai-roundup-2025-12-29', 'TechHub',
    'TechHub', '192D3CC374D33F86939AFC2509D50B55C6044CE13D2E3A6AF8C03E83A7936FC9', ',AI,AI Agents,Microsoft Agent Framework,Semantic Kernel,AutoGen,.NET,Python,VS Code,Agent Skills,Copilot,Azure Functions,Azure Developer CLI,MCP,Microsoft Fabric,Fabric IQ,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-12-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-12-22', 'roundups', 'Weekly AI Roundup: Foundry Agents, Model Routing, and Fabric AI',
    'Azure AI Foundry expands with hosted agents, Model Router GA, and broader LLM support including Claude, Sora 2, and Mistral. Microsoft Fabric adds AI-powered data transformation capabilities in Dataflow Gen2.
<!--excerpt_end-->
## Azure AI Foundry and the Expanding Agentic AI Ecosystem
Azure AI Foundry brings new open-source components for orchestrating secure and flexible AI agents. Hosted Agents now provide persistent memory and simplified management, with deployment and onboarding tools supporting rapid development. The bring-your-own-model (BYO Model Gateway) now includes broader LLM support, such as Claude, Sora 2, and Mistral. Developers can use the Model Router, now generally available, to optimize AI model usage and manage costs. New Foundry Tools and security enhancements with Entra Agent ID centralize AI governance. Feedback from Discord and GitHub channels continues to shape feature planning and onboarding.
- [What''s New in Microsoft Foundry: Agents, Models, and Enterprise-Grade AI (October–November 2025)](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-oct-nov-2025/)
- [Getting Started with Azure AI Foundry: A Beginner’s Guide](/ai/videos/getting-started-with-azure-ai-foundry-a-beginners-guide)
- [Getting Started with Azure AI Foundry](/ai/videos/getting-started-with-azure-ai-foundry)
## Agent-Oriented Architecture: Durable Task Extension and Orchestration Guides
Durable Task Extension enhances agent orchestration, making it simpler to manage dependable workflows. Examples like the AI Travel Planner demonstrate how agents can operate in sequence or in parallel. Human oversight and rollback features are supported for easier troubleshooting. Guides using Azure Functions, Static Web Apps, and OpenTelemetry continue to encourage best practices for agent communication and security.
- [Building Reliable AI Travel Agents with the Durable Task Extension for Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-reliable-ai-travel-agents-with-the-durable-task/ba-p/4478913)
- [Best Practices for Architecting AI Agents in Enterprise Systems](/ai/videos/best-practices-for-architecting-ai-agents-in-enterprise-systems)
## Model Context Protocol (MCP) and Model Router in Developer Workflows
The Model Context Protocol (MCP) now features sessions that clarify how to link AI models, APIs, and data sources in daily work. Hands-on workshops and labs continue to build on this, while the new Model Router lets developers tune model use and integrate policies into agent workflows with better control.
- [Leveraging the Model Context Protocol (MCP) in Visual Studio Code for Enhanced Development](/ai/videos/leveraging-the-model-context-protocol-mcp-in-visual-studio-code-for-enhanced-development)
- [Using Foundry''s Model Router to Simplify Optimal AI Model Selection](/ai/videos/using-foundrys-model-router-to-simplify-optimal-ai-model-selection)
## Autonomous Agents and AI-Powered Data Transformation in Microsoft Fabric
Microsoft Fabric updates make agent consumption and billing reports more transparent, letting teams optimize usage and spending. Dataflow Gen2 gains AI-powered capabilities for prompt-based operations in Power Query, enabling summarization, classification, sentiment analysis, and translation directly—offering more advanced BI and analytics via easier-to-use interfaces and simple documentation.
- [Understanding Operations Agent Capacity Usage and Billing in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/understanding-operations-agent-capacity-consumption-usage-reporting-and-billing/)
- [AI-Powered Data Transformation with Dataflow Gen2 in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/from-simple-prompts-to-complex-insights-ai-expands-the-boundaries-of-data-transformation/)',
    'Azure AI Foundry expands with hosted agents, Model Router GA, and broader LLM support including Claude, Sora 2, and Mistral. Microsoft Fabric adds AI-powered data transformation capabilities in Dataflow Gen2.',
    1766390400, 'ai', '/ai/roundups/weekly-ai-roundup-2025-12-22', 'TechHub',
    'TechHub', '0171020F28F51740B2714C4CE1A6A147A854C6E0E1FC1E3643F3D62FD0DCE294', ',Azure AI Foundry,AI Agents,Hosted Agents,Agent Orchestration,Durable Task Extension,Model Router,MCP,Bring Your Own Model,Claude,Sora 2,Mistral,Microsoft Entra Agent ID,Microsoft Fabric,Dataflow Gen2,Power Query,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-12-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-12-15', 'roundups', 'Weekly AI Roundup: Foundry agents, GPT-5.2, and MCP standards',
    'The AI segment this week spotlights integration within Microsoft''s ecosystem, including improvements in agentic AI platforms, the broad GPT-5.2 rollout, and new open standards for agent orchestration. Developers received new resources covering best practices, productivity workflows, and team dynamics.
<!--excerpt_end-->
## Microsoft Foundry and Agentic AI Platforms
The Foundry platform, which now includes MCP Server and enhanced orchestration, supports modular agent architectures. Features include persistent agent memory, customization, security controls, and options for business process automation such as expense management and analytics.
Key updates at Ignite 2025 include access to Anthropic models, multi-model coordination, unified data pipelines, and dedicated hardware for AI training. Copilot Studio and Azure Copilot add tools for automation, analytics, and compliance via portal, CLI, and operations modules. Security improvements include ARM CPU, NVIDIA GPU, and hardware security module support for AI jobs. New guides demonstrate how to build document-processing pipelines and fine-tune agent orchestration, helping teams quickly adopt these capabilities.
- [Exploring the Future of AI Agents with Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/exploring-the-future-of-ai-agents-with-microsoft-foundry/ba-p/4476107)
- [Agentic AI and Cloud Innovation: Key Takeaways from Microsoft Ignite 2025](https://azure.microsoft.com/en-us/blog/actioning-agentic-ai-5-ways-to-build-with-news-from-microsoft-ignite-2025/)
- [Nasdaq Boardvantage: AI-Driven Governance Architecture with Azure PostgreSQL and Microsoft Foundry](/ai/videos/nasdaq-boardvantage-ai-driven-governance-architecture-with-azure-postgresql-and-microsoft-foundry)
- [From Large Semi-Structured Documents to Actionable Data: Azure-Powered Intelligent Document Processing Pipelines](https://techcommunity.microsoft.com/t5/azure-architecture-blog/from-large-semi-structured-docs-to-actionable-data-reusable/ba-p/4474054)
- [Ignite BRK197: AI Powered Automation & Multi-Agent Orchestration in Foundry](/ai/videos/ignite-brk197-ai-powered-automation-and-multi-agent-orchestration-in-foundry)
- [Build a Pizza Ordering Agent with Microsoft Foundry and MCP](/ai/videos/build-a-pizza-ordering-agent-with-microsoft-foundry-and-mcp)
- [AI Upskilling Framework Level 3: Building Agentic Workflows from Microsoft Ignite](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-upskilling-framework-level-3-building/ba-p/4477472)
- [Ignite BRK1706: Build and Manage AI Apps with Microsoft Foundry](/ai/videos/ignite-brk1706-build-and-manage-ai-apps-with-microsoft-foundry)
## GPT-5.2 Rollout and Integration
OpenAI’s GPT-5.2 is now rolled out to GitHub Copilot, Studio, Foundry, and Microsoft 365 Copilot, building on the previous preview release. GPT-5.2 adds improved logical reasoning, expanded automation and context length, and structured outputs for enterprise platforms, along with policy controls for regulated industries.
Developers can use new options to select GPT-5.2 models directly. Copilot and Studio gain improved reasoning and analytics features, while Microsoft 365 Copilot brings upgraded enterprise analytics. User feedback this week focused on refined experiences in model selection and analytics.
- [Introducing GPT-5.2 in Microsoft Foundry: The New Standard for Enterprise AI](https://azure.microsoft.com/en-us/blog/introducing-gpt-5-2-in-microsoft-foundry-the-new-standard-for-enterprise-ai/)
- [GPT-5.2 Integration Across Microsoft Tools: AI Model Choice and Developer Impact](https://www.linkedin.com/posts/satyanadella_super-excited-about-gpt-52-from-our-partners-activity-7404949433819463680-VyMD)
## Model Context Protocol (MCP) and Agentic Interoperability
The Model Context Protocol (MCP) has now been moved to the Linux Foundation, offering standards for authentication, tool invocation, and handling long-running jobs across agent systems. This move is designed to support interoperability and provide consistent APIs for AI agent developers.
The GitHub MCP Server added more granular configuration, context management, a Go SDK with auto-completion, and improved security with features like Lockdown mode. Additional resources this week demonstrate how to build and use MCP-enabled workflows and highlight the progress on open-source agentic tools.
- [GitHub MCP Server Adds Tool-Specific Configuration and Security Features](https://github.blog/changelog/2025-12-10-the-github-mcp-server-adds-support-for-tool-specific-configuration-and-more)
- [MCP Transitions to Linux Foundation: Impact on AI Tool and Agent Development](https://github.blog/open-source/maintainers/mcp-joins-the-linux-foundation-what-this-means-for-developers-building-the-next-era-of-ai-tools-and-agents/)
- [Unlock agentic workflows for your apps using MCP on Windows](/ai/videos/unlock-agentic-workflows-for-your-apps-using-mcp-on-windows)
- [Model Context Protocol: From Concept to Linux Foundation Agentic AI](/ai/videos/model-context-protocol-from-concept-to-linux-foundation-agentic-ai)
## AI Integration in Developer Workflows and Cloud Automation
Guides released this week detail best practices for integrating AI into developer workflows, focusing on authentication, agent orchestration, and end-to-end automation. Tutorials demonstrate secure app building with OpenAI libraries, using Entra ID tokens and MCP agents. Updates to the .NET agent framework add memory persistence and scalable orchestration, all supported by hands-on labs. Azure Redis now offers secure memory support for agents, and the Durable Task Extension streamlines orchestration for distributed workloads.
Azure Copilot Storage Migration Solutions Advisor and persistent memory in Azure SRE Agent continue the automation and troubleshooting focus for DevOps and cloud teams.
- [Supercharge Your Apps with OpenAI: Secure Authentication, Azure Integration, and MCP Agents](/ai/videos/supercharge-your-apps-with-openai-secure-authentication-azure-integration-and-mcp-agents)
- [Secure and Smart AI Agents with Azure Redis in .NET](/ai/videos/secure-and-smart-ai-agents-with-azure-redis-in-net)
- [Modernization Made Simple: Building Agentic Solutions in .NET](/ai/videos/modernization-made-simple-building-agentic-solutions-in-net)
- [Bulletproof Agents with the Durable Task Extension for Microsoft Agent Framework](/ai/videos/bulletproof-agents-with-the-durable-task-extension-for-microsoft-agent-framework)
- [Transforming Data Migration Decisions with Azure Copilot''s Storage Migration Solutions Advisor](https://techcommunity.microsoft.com/t5/azure-storage-blog/transforming-data-migration-using-azure-copilot/ba-p/4476610)
- [Never Explain Context Twice: Introducing Azure SRE Agent Memory](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/never-explain-context-twice-introducing-azure-sre-agent-memory/ba-p/4473059)
## Microsoft Fabric and Real-Time Intelligence for AI
New features in Microsoft Fabric this week enable more unified analytics and AI-driven modeling. Developers can coordinate data flows for ingestion, analytics, anomaly detection, and semantic modeling in a single platform, eliminating the need for separate tools. Updates include Eventstream, Eventhouse, anomaly detection, and Fabric IQ, supporting automated insight generation and dashboard creation.
- [Microsoft Fabric Real-Time Intelligence: Setting the Standard for Streaming Data Platforms](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-real-time-intelligence-a-leader-in-the-2025-forrester-streaming-data-wave/)
## Agentic Business Applications and Autonomous AI
Convergence 2025 confirms that business platforms such as Dynamics 365, Copilot Studio, and Microsoft Fabric now include AI-driven automation for ERP, CRM, and business operations. Copilot Studio allows for custom agent design and integration, and new articles outline how to define agent identity, enforce governance, and use open protocols for scale-out deployments.
- [Agentic Business Applications and AI Autonomy at Convergence 2025](https://www.microsoft.com/en-us/dynamics-365/blog/business-leader/2025/12/09/the-era-of-agentic-business-applications-arrives-at-convergence-2025/)
- [AI-Driven Product Change Management with Copilot Studio for Manufacturing](/ai/videos/ai-driven-product-change-management-with-copilot-studio-for-manufacturing)
## Advances in AI Procurement and Developer Productivity
Microsoft’s new Agent Pre-Purchase Plan (P3) gives organizations a way to purchase pooled agent credits for Foundry and Copilot Studio, which simplifies procurement across the AI portfolio and reflects last week’s preview. Developer tools like Aspire are now available to support monitoring AI applications and connecting to Azure, making it easier to adopt these in the enterprise.
- [Microsoft Agent Pre-Purchase Plan: Unified AI Procurement for Foundry and Copilot Studio](https://techcommunity.microsoft.com/t5/finops-blog/microsoft-agent-pre-purchase-plan-one-unified-path-to-scale-ai/ba-p/4476052)
- [Aspire for AI Applications](/ai/videos/aspire-for-ai-applications)
- [AI Dev Days 2025: Your Gateway to the Future of AI Development](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-dev-days-2025-your-gateway-to-the-future-of-ai-development/ba-p/4476113)
## Multi-Model Reasoning, Open Source, and AI Developer Research
A demonstration app showed how to run decisions across multiple AI models (GPT, Claude, Gemini) on Azure, with full CI/CD support. The 2025 Octoverse report analyzed the influence of AI on open-source work, continuing the thread of practical community practices from last week.
Research this week from Atlassian, Google DORA, and LaunchDarkly reinforces the importance of proven practices, trust, and discipline to maximize AI productivity gains, echoing prior coverage of the human side of developer work.
- [Multi-Model Reasoning App Demoed at Bengaluru Dev Event: Decision Frameworks, Azure, and Copilot Vision](https://www.linkedin.com/posts/satyanadella_was-fun-to-be-at-a-dev-event-in-bengaluru-activity-7404820776195043329-A5vB)
- [The Human Side of Octoverse 2025: Insights on Open Source, AI, and Collaboration](/ai/videos/the-human-side-of-octoverse-2025-insights-on-open-source-ai-and-collaboration)
- [Research: AI''s Impact on Developer Productivity Hinges on Best Practices](https://devclass.com/2025/12/11/research-ai-can-help-or-hinder-software-development-and-old-style-best-practices-make-the-difference/)',
    'The AI segment this week spotlights integration within Microsoft''s ecosystem, including improvements in agentic AI platforms, the broad GPT-5.2 rollout, and new open standards for agent orchestration. Developers received new resources covering best practices, productivity workflows, and team dynamics.',
    1765785600, 'ai', '/ai/roundups/weekly-ai-roundup-2025-12-15', 'TechHub',
    'TechHub', 'B978CDEB377481B031D6BD0CE0B679107D60330C51C34806B9980BE2C134E5F5', ',Microsoft Foundry,Agentic AI,GPT 5.2,MCP,Linux Foundation,GitHub Copilot,Copilot Studio,Microsoft 365 Copilot,Azure Copilot,Multi Agent Orchestration,Enterprise AI Governance,AI Security Controls,Azure Redis,.NET Agent Framework,Microsoft Fabric Real Time Intelligence,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-12-08
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-12-08', 'roundups', 'Weekly AI Roundup: Agent Platforms, Models, and Enterprise Ops',
    'This week features advances in agent platforms, new models, developer events, and enterprise integration tools.
<!--excerpt_end-->
## Microsoft Agentic Platforms: MCP, Foundry, and Agent Frameworks
The MCP platform enables agent interoperability, with a new livestream series showing how to run servers both locally and in hybrid setups. Foundry’s MCP Server preview adds deeper browser and IDE integration and features for compliance and monitoring. Agent Framework shared updated best practices and expanded integration feedback (e.g., CopilotKit, AG-UI), with guidance for troubleshooting and prototyping multi-agent systems.
- [Learn MCP from our Free Livestream Series in December](https://techcommunity.microsoft.com/t5/microsoft-developer-community/learn-mcp-from-our-free-livestream-series-in-december/ba-p/4474729)
- [Accelerating AI Agent Development: Foundry MCP Server Preview Announced](https://devblogs.microsoft.com/foundry/announcing-foundry-mcp-server-preview-speeding-up-ai-dev-with-microsoft-foundry/)
- [Agentic Development Best Practices with Microsoft Agent Framework: AG-UI, DevUI & OpenTelemetry](https://devblogs.microsoft.com/semantic-kernel/the-golden-triangle-of-agentic-development-with-microsoft-agent-framework-ag-ui-devui-opentelemetry-deep-dive/)
## AI Models and Data Workflows in the Microsoft Ecosystem
Mistral Large 3 is now part of Microsoft Foundry, building on previous support for multi-modal and long-context AI. Foundry also emphasizes sovereign cloud deployment and supports direct observability in the browser for live production workloads. .NET''s new ingestion tools further streamline integration with document and vector storage, while event-driven architectures like Drasi’s workflows expand possibilities for event-triggered agents.
- [Introducing Mistral Large 3 in Microsoft Foundry: Open Enterprise AI for Production Workloads](https://azure.microsoft.com/en-us/blog/introducing-mistral-large-3-in-microsoft-foundry-open-capable-and-ready-for-production-workloads/)
- [Introducing Data Ingestion Building Blocks in .NET for AI Applications](https://devblogs.microsoft.com/dotnet/introducing-data-ingestion-building-blocks-preview/)
- [Beyond the Chat Window: How Change-Driven Architecture Enables Ambient AI Agents](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/beyond-the-chat-window-how-change-driven-architecture-enables/ba-p/4475026)
## Building and Orchestrating AI Solutions with LangChain, Foundry, and Azure
The OSS AI Summit continued progress on multi-agent orchestration, featuring updates for LangChain v1, Foundry, and Azure OpenAI. Workshops showed secure agent orchestration and use of Azure project blueprints. Community challenges, like the November Innovation Challenge, provided real-world examples and highlighted community adoption.
- [Join the OSS AI Summit: Building with LangChain and Microsoft AI](https://devblogs.microsoft.com/blog/join-the-oss-ai-summit-building-with-langchain-event)
- [Innovation Challenge: Winning Azure AI Projects from November 2025 Hackathon](https://techcommunity.microsoft.com/t5/azure/the-november-innovation-challenge-winning-teams/m-p/4475579#M22360)
## AI Developer Events, Training, and Educational Programs
Developer education remained in focus, with AI Dev Days offering workshops on Copilot, Foundry, and Azure AI. Demonstrations covered agent code and deployment in Visual Studio 2026 and VS Code, with expanded coverage of new languages and governance APIs. The Hour of AI campaign brings AI literacy efforts to K-12 education, creating pathways for early learning.
- [AI Dev Days: Virtual Event for Developers on Azure, GitHub, and AI with Microsoft Reactor](https://devblogs.microsoft.com/foundry/ai-dev-days-december-2025/)
- [AI Dev Days 2025: Microsoft & GitHub Virtual Event for Developers](https://devblogs.microsoft.com/blog/join-us-for-ai-devdays)
- [AI Dev Days: Using AI to Enhance Developer Productivity](/ai/videos/ai-dev-days-using-ai-to-enhance-developer-productivity)
- [AI Dev Days: Building AI Applications with Azure and GitHub](/ai/videos/ai-dev-days-building-ai-applications-with-azure-and-github)
- [Hour of AI: Microsoft Launches Global AI Literacy Initiative for Computer Science Education Week 2025](https://www.microsoft.com/en-us/education/blog/2025/12/unlock-ai-learning-with-hour-of-ai-for-computer-science-education-week/)
## Enterprise AI Integration and Administration
Guides for using Claude Code, Foundry, and Spec Kit address requirement automation and code review for agent-based workflows. Monitoring and context management transitions to GitHub Actions pipelines, and OneLake connects with Foundry Knowledge for consolidated data analytics across Azure and AWS. The AB-900 exam resource helps teams understand Copilot administration and responsible AI practices.
- [Enterprise AI Coding Agent Setup: Claude Code, Microsoft Foundry, Spec Kit, and GitHub Actions](https://devblogs.microsoft.com/all-things-azure/claude-code-microsoft-foundry-enterprise-ai-coding-agent-setup/)
- [Unlocking Enterprise AI: Seamless Integration of OneLake Files in Microsoft Foundry Knowledge](https://blog.fabric.microsoft.com/en-US/blog/unlocking-enterprise-ai-seamless-integration-of-onelake-files-in-microsoft-foundry-knowledge/)
- [AB-900 Study Cram: Microsoft 365 Copilot & Agent Administration Fundamentals](/ai/videos/ab-900-study-cram-microsoft-365-copilot-and-agent-administration-fundamentals)
## AI Developer Workflows and Productivity Tools
VS Code’s Agent HQ integration supports session management and Copilot CLI, making agent deployment and monitoring more efficient. GitHub Models now allow real-time comparisons between model outputs for coding tasks, while Copilot Studio’s latest roadmap covers debugging improvements, Microsoft 365 integration, and better cost tracking.
- [Agent HQ Integration in Visual Studio Code](/ai/videos/agent-hq-integration-in-visual-studio-code)
- [GitHub Models: Test and Compare AI Code Models](/ai/videos/github-models-test-and-compare-ai-code-models)
- [What’s New in Copilot Studio and Roadmap](/ai/videos/whats-new-in-copilot-studio-and-roadmap)
## AI Agents in Production Workflows and Medical Automation
Guides describe how to embed AI voice agents into medical documentation for real-time productivity. Managed agentic apps in Foundry enable centralized control and cloud scaling, showcasing organizations moving their pilot projects to Azure for full production deployment.
- [How AI Voice Agents Transform Medical Documentation in Real Time](/ai/videos/how-ai-voice-agents-transform-medical-documentation-in-real-time)
- [Building Connected Managed Agentic Apps with Microsoft Foundry (Ignite BRK113)](/ai/videos/building-connected-managed-agentic-apps-with-microsoft-foundry-ignite-brk113)
## Responsible AI, Governance, and Engineering Collaboration
The Armchair Architects series provides guidance on governance for agent and microservice architectures. Microsoft’s collaborative AI engineering framework, with examples like Entra SDK migration, puts process improvements into focus—integrating agents that support documentation and escalation.
- [Armchair Architects: Governance Strategies for AI Agents](/ai/videos/armchair-architects-governance-strategies-for-ai-agents)
- [Collaborating with AI Agents: A Framework for Engineering Transformation at Microsoft](https://devblogs.microsoft.com/engineering-at-microsoft/the-interaction-changes-everything-treating-ai-agents-as-collaborators-not-automation/)',
    'This week features advances in agent platforms, new models, developer events, and enterprise integration tools.',
    1765180800, 'ai', '/ai/roundups/weekly-ai-roundup-2025-12-08', 'TechHub',
    'TechHub', 'E952696280E37E7AF1070B15D2C3E22F467C27024113B4FE86E89EF32F8987C0', ',AI Agents,MCP,Microsoft Foundry,Microsoft Agent Framework,Azure OpenAI,Mistral Large 3,LangChain,.NET,Vector Databases,Observability,OpenTelemetry,GitHub Actions,VS Code,GitHub Models,Copilot Studio,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-12-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-12-01', 'roundups', 'Weekly AI Roundup: Agents, Edge Deployment, and Azure AI Infra',
    'AI news this week includes applied updates, technical explorations, and confirmation of value in practical deployments, all centered on making intelligent solutions easier to roll out in cloud, edge, or enterprise situations. Ignite 2025 delivered new agent frameworks, infrastructure ready for AI, and reference architectures to address current business challenges. Developers now have access to updated guidance, stronger governance, and improved tools, with key session content on Microsoft Foundry, Azure, and automation.
<!--excerpt_end-->
## Microsoft Foundry and Edge/Enterprise AI Workflows
Continuing last week’s developments, Microsoft Foundry now further supports cloud and edge workflows by introducing deployment guides for Foundry Local and Android integration. This shift puts AI closer to where data lives, supporting the flexible, hybrid scenarios discussed previously.
Live sessions pick up on earlier coverage of agent coordination and tool-rich agent frameworks, expanding into GitOps and ORAS-based pipelines, as well as real-world demos on cluster management and staged deployments. Security, lifecycle controls, and collaboration with Microsoft Defender continue to progress, as does the use of multi-tenant solutions highlighted before.
Wider local AI support for Phi Silica and Copilot+ PCs is in place, including Windows AI API learning sessions. Tutorials on techniques like LoRA tuning, RAG, and admin controls further support the enterprise trend toward accessible, flexible AI. Use cases span industries like healthcare and analytics, underlining practical results from these developments.
- [Building and Shipping Edge AI Apps with Microsoft Foundry](/ai/videos/building-and-shipping-edge-ai-apps-with-microsoft-foundry)
- [Integrating Local AI in Enterprise Apps with Windows AI APIs and Microsoft Foundry](/ai/videos/integrating-local-ai-in-enterprise-apps-with-windows-ai-apis-and-microsoft-foundry)
- [From Concept to Code: Building Production-Ready Multi-Agent Systems with Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-concept-to-code-building-production-ready-multi-agent/ba-p/4472752)
- [Microsoft Foundry Workflows - Pt. 1: Creating a Sequential Multi-Agent Workflow](/ai/videos/microsoft-foundry-workflows-pt-1-creating-a-sequential-multi-agent-workflow)
- [Creating Multi-Agent Workflows in Microsoft Foundry: Adding Agents with Tools](/ai/videos/creating-multi-agent-workflows-in-microsoft-foundry-adding-agents-with-tools)
- [Migrating Microsoft Foundry Workflows to VS Code Web](/ai/videos/migrating-microsoft-foundry-workflows-to-vs-code-web)
- [Building Autonomous Enterprise Agents with Reasoning in Microsoft Foundry](/ai/videos/building-autonomous-enterprise-agents-with-reasoning-in-microsoft-foundry)
- [Building Microsoft Agent Framework Solutions for Microsoft 365 with AI Integration](/ai/videos/building-microsoft-agent-framework-solutions-for-microsoft-365-with-ai-integration)
- [Agentic AI for Creatives: Microsoft Foundry and Data Solutions at Ignite](/ai/videos/agentic-ai-for-creatives-microsoft-foundry-and-data-solutions-at-ignite)
- [Panel: Real-World Architectures and Lessons from Scaling AI Agents on Azure](/ai/videos/panel-real-world-architectures-and-lessons-from-scaling-ai-agents-on-azure)
- [AI Enterprise Value: Real-World Applications with Microsoft Foundry and MCP](/ai/videos/ai-enterprise-value-real-world-applications-with-microsoft-foundry-and-mcp)
- [Foundry Control Plane: Managing AI Agents at Scale](/ai/videos/foundry-control-plane-managing-ai-agents-at-scale)
## AI Infrastructure and Platform Integration on Azure
Expanding last week’s highlights of Azure’s Fairwater AI and core infrastructure, this week focuses on new NVIDIA Blackwell cluster deployments and progress in confidential computing. Detailed technical topics include advanced networking like NVLink and new RTX Pro 6000 features for robotics and simulation, following on from earlier discussions. Omniverse digital twin workflows are explored deeply, with examples for various industries.
Practical tutorials continue themes of real-time AI streaming, flexible model routing, and system reliability. Demonstrations show how open-source tools such as Ray, KAITO, and LangChain work together with Azure’s managed pipeline services, matching the earlier move toward hybrid, automated workflows. New Oracle Database@Azure integrations now include step-by-step guides for secure, unified AI search, and Copilot Studio and Purview play ongoing roles for governance and security.
- [Power Next-Generation AI Workloads with NVIDIA Blackwell on Azure](/ai/videos/power-next-generation-ai-workloads-with-nvidia-blackwell-on-azure)
- [Running AI on Azure Storage: Fast, Secure, and Scalable](/ai/videos/running-ai-on-azure-storage-fast-secure-and-scalable)
- [Embedding AI in Oracle Workloads with Oracle Database@Azure and Microsoft Fabric](/ai/videos/embedding-ai-in-oracle-workloads-with-oracle-databaseazure-and-microsoft-fabric)
- [AI-Driven Governance for Nasdaq Boardvantage with Azure PostgreSQL and Microsoft Foundry](/ai/videos/ai-driven-governance-for-nasdaq-boardvantage-with-azure-postgresql-and-microsoft-foundry)
## Agentic AI and Automation with Copilot Studio and Power Platform
Expanding on last week’s focus on automation, this week delivers concrete steps and best practices for lead qualification, sales automation, and workflow reliability in Copilot Studio—all closely linked with the Power Platform.
Guidance covers always-on agent deployment, failover, and monitoring for consistent operation. New features in Power Apps and Power Pages, as well as improvements to RPA and AI integration, reinforce the rapid implementation of low-code AI solutions for evolving business processes.
- [Microsoft Ignite: Agents at Work and Copilot Studio for Business Process Automation](/ai/videos/microsoft-ignite-agents-at-work-and-copilot-studio-for-business-process-automation)
- [Advancements in Power Platform: AI, Automation, and Secure Integrations](/ai/videos/advancements-in-power-platform-ai-automation-and-secure-integrations)
- [Best Practices for Always-On AI Agents Using Copilot Studio and Power Platform](/ai/videos/best-practices-for-always-on-ai-agents-using-copilot-studio-and-power-platform)
## Specialized AI Use Cases: Industry, Supply Chain, and Domain Solutions
At Ignite, multiple industry case studies continue the pattern of enterprise-scale agentic AI adoption. This includes projects at Kraft Heinz, Toyota, and Zurich, focused on real deployment of digital twins, and the use of SQL Server embeddings and ERP tools for process automation. These guides provide steps for applying advanced orchestration strategies in real settings, moving theory into successful practice.
- [Optimizing Manufacturing at Kraft Heinz with AI, Azure Arc, and Microsoft Foundry](/ai/videos/optimizing-manufacturing-at-kraft-heinz-with-ai-azure-arc-and-microsoft-foundry)
- [Reinventing Manufacturing with Digital Twin: AI-Powered Advances](/ai/videos/reinventing-manufacturing-with-digital-twin-ai-powered-advances)
- [Transforming Manufacturing with Digital Twins and Real-Time Simulation Featuring Azure and NVIDIA AI](/ai/videos/transforming-manufacturing-with-digital-twins-and-real-time-simulation-featuring-azure-and-nvidia-ai)
- [Drive Growth, Profitability and Resilience with Agentic Supply Chains](/ai/videos/drive-growth-profitability-and-resilience-with-agentic-supply-chains)
- [Zurich and Toyota’s Playbook for Enterprise AI Innovation](/ai/videos/zurich-and-toyotas-playbook-for-enterprise-ai-innovation)
- [ERP Transformation with AI: Building Autonomous Agents (Microsoft Ignite 2025 Session PBRK361)](/ai/videos/erp-transformation-with-ai-building-autonomous-agents-microsoft-ignite-2025-session-pbrk361)
- [Powering Financial Workflows with AI: Microsoft and LSEG Partnership at Ignite](/ai/videos/powering-financial-workflows-with-ai-microsoft-and-lseg-partnership-at-ignite)
- [Build Secure Agentic AI Apps with SQL Server 2025](/ai/videos/build-secure-agentic-ai-apps-with-sql-server-2025)
- [Partners and Agentic AI: Transforming Energy and Resources at Microsoft Ignite](/ai/videos/partners-and-agentic-ai-transforming-energy-and-resources-at-microsoft-ignite)
- [Transforming Enterprise Workflows with C3 AI Agentic Process Automation](/ai/videos/transforming-enterprise-workflows-with-c3-ai-agentic-process-automation)
- [Building Enterprise AI Apps with C3 Agentic AI Platform at Microsoft Ignite](/ai/videos/building-enterprise-ai-apps-with-c3-agentic-ai-platform-at-microsoft-ignite)
- [Real Stories of AI Transformation in Local Government](/ai/videos/real-stories-of-ai-transformation-in-local-government)
- [AI-Powered Industry Solutions at Microsoft Ignite 2025: Capgemini''s Multi-Channel Knowledge Innovations](/ai/videos/ai-powered-industry-solutions-at-microsoft-ignite-2025-capgeminis-multi-channel-knowledge-innovations)
- [Innovate with AI at Enterprise Scale: Microsoft Ignite Session BRK410](/ai/videos/innovate-with-ai-at-enterprise-scale-microsoft-ignite-session-brk410)
- [Impiger’s AI-First Enterprise Transformation at Microsoft Ignite](/ai/videos/impigers-ai-first-enterprise-transformation-at-microsoft-ignite)
## Azure Speech, Voice, and Conversational AI
Azure Speech and Voice Live API are now generally available. This extension, originating from recent updates, enables real-time, context-aware conversational AI with new features such as enhanced model selection and HD neural voice support that covers over 100 languages. These improvements support tasks like live translation in healthcare and customer service, following the roadmap originally presented in previous updates.
- [Building Agentic Apps with Azure Speech and Voice Live API](/ai/videos/building-agentic-apps-with-azure-speech-and-voice-live-api)
## AI Developer Workflow and Code Integration
Workflow integration continues to improve, with new resources covering GitHub, Foundry, and VS Code Web. The focus is on stepwise guides for migrating code, automating pipelines, and managing agent packaging for Azure deployments, following ongoing discussions around CI/CD and AI prototyping.
Configuration-as-code tutorials reinforce the code-first approach, equipping teams with reliable methods for traceable AI-powered automation.
- [Build AI Apps Fast with GitHub and Microsoft Foundry in Action (MS Ignite BRK110)](/ai/videos/build-ai-apps-fast-with-github-and-microsoft-foundry-in-action-ms-ignite-brk110)
## Other AI News
C3 AI Studio continues to evolve, offering practical guides for both low-code and code-first app development, focusing on deployment, ontology building, and monitoring. The community forums show strong collaboration, connecting GitHub and the Microsoft AI ecosystem for enterprise-ready knowledge management.
- [Building Enterprise AI Apps with C3 Agentic AI Platform at Microsoft Ignite](/ai/videos/building-enterprise-ai-apps-with-c3-agentic-ai-platform-at-microsoft-ignite)
- [AI-Powered Industry Solutions at Microsoft Ignite 2025: Capgemini''s Multi-Channel Knowledge Innovations](/ai/videos/ai-powered-industry-solutions-at-microsoft-ignite-2025-capgeminis-multi-channel-knowledge-innovations)
- [Transforming Enterprise Workflows with C3 AI Agentic Process Automation](/ai/videos/transforming-enterprise-workflows-with-c3-ai-agentic-process-automation)',
    'AI news this week includes applied updates, technical explorations, and confirmation of value in practical deployments, all centered on making intelligent solutions easier to roll out in cloud, edge, or enterprise situations. Ignite 2025 delivered new agent frameworks, infrastructure ready for AI, and reference architectures to address current business challenges. Developers now have access to updated guidance, stronger governance, and improved tools, with key session content on Microsoft Foundry, Azure, and automation.',
    1764576000, 'ai', '/ai/roundups/weekly-ai-roundup-2025-12-01', 'TechHub',
    'TechHub', '36E4990719786D212CA79C9B5A3E7CD11BA5B00E009780B7412E849B799588EB', ',Microsoft Foundry,Agentic AI,Multi Agent Systems,Azure AI,Edge AI,Foundry Local,Windows AI API,Copilot Studio,Power Platform,GitOps,ORAS,NVIDIA Blackwell,Confidential Computing,Digital Twins,Azure Speech,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-11-17
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-11-17', 'roundups', 'Weekly AI Roundup: Agents, Real-Time Apps, and Azure Scaling',
    'Recent AI developments focus on improved infrastructure, integration with real-time workflows, and expanded solutions across Microsoft environments. New tutorials and workflow guides underline the ongoing incorporation of AI into developer productivity and business operations. Surveys from GitHub’s Octoverse confirm AI’s influence on programming languages, team roles, and automation. This week’s articles also prioritize secure, compliant, and sustainable scaling.
<!--excerpt_end-->
## Azure AI: Infrastructure, Integration, and Operational Patterns
Building on earlier work with containerized and edge workloads, Azure’s Fairwater AI superfactory now brings more energy-efficient GPUs and faster networking for scalable and sustainable operations. Real-time capabilities are showcased through SignalR/Key Vault integrations in Angular and .NET chat, with Entra ID authentication. Durable Task Extension in the Microsoft Agent Framework adds reliability for agent applications. These updates support the cloud-native scaling improvements from previous coverage.
SleekFlow’s deployment example illustrates Azure’s support for secure and rapid integration of AI into enterprise workflows. New resources for agent construction, AI playgrounds, and adaptive model usage enable developers to route models and orchestrate operations with greater control and efficiency.
- [Infinite Scale: Architecture of the Azure AI Superfactory](https://aka.ms/AAyjgcy)
- [Real-Time AI Streaming with Azure OpenAI and SignalR](https://techcommunity.microsoft.com/t5/microsoft-developer-community/real-time-ai-streaming-with-azure-openai-and-signalr/ba-p/4468833)
- [Building Resilient AI Agents with the Durable Task Extension for Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bulletproof-agents-with-the-durable-task-extension-for-microsoft/ba-p/4467122)
- [How SleekFlow Uses Azure and AI to Orchestrate Enterprise Customer Conversations](https://techcommunity.microsoft.com/t5/azure-customer-innovation-blog/staying-in-the-flow-sleekflow-and-azure-turn-customer/ba-p/4467945)
- [Build Your First AI Agent with Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-your-first-ai-agent-with-azure-app-service/ba-p/4468725)
- [Introducing AI Playground on Azure App Service for Linux](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-ai-playground-on-azure-app-service-for-linux/ba-p/4469497)
- [Adaptive Model Selection with Azure AI Foundry Model Router in TypeScript](https://techcommunity.microsoft.com/t5/microsoft-developer-community/adaptive-model-selection-in-typescript-with-the-model-router/ba-p/4465192)
## .NET Ecosystem: AI Integration, Agentic Design, and Tooling
This week emphasizes .NET’s expanded support for AI, with updated abstractions, model management utilities, and design patterns provided by Semantic Kernel and Agent Framework. New releases in .NET 10, ASP.NET Core 10, MAUI 10, C# 14, and F# 10 showcase continued evolution in AI integration for language and tooling. Tutorials build on last week’s best practices, focusing on agentic structures, search, reasoning, and improved user experience in .NET. Visual Studio 2026 diagnostics and testing tools extend workflow validation paired with AI-enhanced feedback.
- [Building Intelligent Apps with .NET](/ai/videos/building-intelligent-apps-with-net)
- [AI Foundry for .NET Developers](/ai/videos/ai-foundry-for-net-developers)
- [Practical AI Applications for Improved User Experience with .NET](/ai/videos/practical-ai-applications-for-improved-user-experience-with-net)
- [.NET Diagnostic Tooling with AI](/ai/videos/net-diagnostic-tooling-with-ai)
- [AI-Powered Testing in Visual Studio](/ai/videos/ai-powered-testing-in-visual-studio)
- [Understanding Agentic Development for .NET Developers](/ai/videos/understanding-agentic-development-for-net-developers)
- [Overcoming the limitations when using AI](/ai/videos/overcoming-the-limitations-when-using-ai)
- [Architecting an AI-Powered Sales Dashboard with .NET MAUI and Azure OpenAI](/ai/videos/architecting-an-ai-powered-sales-dashboard-with-net-maui-and-azure-openai)
- [Designing Seamless AI Agents with C#: One Question, One Answer Approach](/ai/videos/designing-seamless-ai-agents-with-c-one-question-one-answer-approach)
- [Designing Seamless AI Agents with C#: One Question, One Answer Model](/ai/videos/designing-seamless-ai-agents-with-c-one-question-one-answer-model)
## Model Context Protocol (MCP) and Multimodal AI Agent Frameworks
Adoption of MCP frameworks for .NET, Java, and JetBrains continues to grow, with new resources confirming MCP’s importance for agent context-sharing and interoperability. The MMCTAgent’s Planner–Critic model further enhances multimodal AI agent reasoning—building on themes from earlier editions about plugin architectures and Azure AI Foundry.
- [Model Context Protocol (MCP) for .NET Developers](/ai/videos/model-context-protocol-mcp-for-net-developers)
- [5 Things You Need to Know About Model Context Protocol (MCP)](/ai/videos/5-things-you-need-to-know-about-model-context-protocol-mcp)
- [MMCTAgent: Microsoft’s Multimodal Critical Thinking Agent for Image and Video Reasoning](https://www.microsoft.com/en-us/research/blog/mmctagent-enabling-multimodal-reasoning-over-large-video-and-image-collections/)
## AI Agent Design and Automation Workflows
Agent and workflow design resources this week delve into practical comparisons between code-first, workflow-first, and hybrid solutions for enterprise automation. The expansion of no-code agent development through Azure Logic Apps brings AI capabilities to a wider audience. Knowledge-sharing continues through Mission Agent Possible and Ignite sessions.
- [Workflow-First, Code-First, and Hybrid AI Agent Design: Approaches for Enterprise Automation](https://techcommunity.microsoft.com/t5/azure-architecture-blog/building-ai-agents-workflow-first-vs-code-first-vs-hybrid/ba-p/4466788)
- [Build AI Agents with Zero Code Using Azure Logic Apps](/ai/videos/build-ai-agents-with-zero-code-using-azure-logic-apps)
- [Designing Effective AI Agents: Insights from Armchair Architects](/ai/videos/designing-effective-ai-agents-insights-from-armchair-architects)
- [Mission Agent Possible: Developer AI Agent Competition at Microsoft Ignite 2025](https://techcommunity.microsoft.com/t5/microsoft-developer-community/mission-agent-possible-your-chance-to-build-solve-and-win-at/ba-p/4467585)
- [Build Custom AI Copilots Using Microsoft Copilot Studio and Oracle Database@Azure](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/build-your-own-custom-copilots-with-microsoft-copilot-studio-and/ba-p/4468211)
- [Developer-Focused Azure AI Foundry Sessions at Microsoft Ignite 2025](https://devblogs.microsoft.com/foundry/ignitedevsessions/)
## AI-Driven Coding, Developer Workflows, and Trends
Building from data on programming languages and development trends, TypeScript’s increase in usage over Python and Java is attributed to the benefits of static typing, which supports safer and more automated workflows. Team surveys emphasize that relying solely on “vibe coding” introduces risks unless balanced with solid DevOps practices and engineering discipline, maintaining a regular theme of responsible and productive AI integration.
- [How AI and TypeScript Are Shaping the Future of Software Development: Insights from GitHub Octoverse 2025](/ai/videos/how-ai-and-typescript-are-shaping-the-future-of-software-development-insights-from-github-octoverse-2025)
- [How AI Is Shaping Language Trends in Software Development: Insights from GitHub Next](https://github.blog/news-insights/octoverse/typescript-python-and-the-ai-feedback-loop-changing-software-development/)
- [How AI Coding Is Shaping Software Engineering and DevOps Roles](https://devops.com/survey-sees-ai-coding-creating-need-for-more-software-engineers/)
- [Vibe Coding vs. Spec-Driven Development: Finding Balance in the AI Era](https://devops.com/vibe-coding-vs-spec-driven-development-finding-balance-in-the-ai-era/)
- [Vibe Coding Can Create Unseen Vulnerabilities](https://devops.com/vibe-coding-can-create-unseen-vulnerabilities/)
- [AI-Driven Performance Testing: Redefining Software Quality and Engineering Roles](https://devops.com/ai-driven-performance-testing-a-new-era-for-software-quality/)
## Other AI News
Developer tool news complements Copilot Studio and agent updates, with GPT-5.1 now enabling conversational AI for direct experimentation. Continued emphasis on model routing, session management, and security best practices reflect priorities of efficiency and compliance. Migration and troubleshooting guides bring practical solutions for adoption and feature expansion.
- [GPT-5.1 Experimental Model Now Available in Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/available-now-gpt-5-1-in-microsoft-copilot-studio/)
- [Build Custom AI Copilots Using Microsoft Copilot Studio and Oracle Database@Azure](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/build-your-own-custom-copilots-with-microsoft-copilot-studio-and/ba-p/4468211)
- [How SleekFlow Uses Azure and AI to Orchestrate Enterprise Customer Conversations](https://techcommunity.microsoft.com/t5/azure-customer-innovation-blog/staying-in-the-flow-sleekflow-and-azure-turn-customer/ba-p/4467945)
- [Build Your First AI Agent with Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-your-first-ai-agent-with-azure-app-service/ba-p/4468725)
- [Introducing AI Playground on Azure App Service for Linux](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-ai-playground-on-azure-app-service-for-linux/ba-p/4469497)
- [Learning From the Past: What Automation Mistakes Can Teach Us About AI](https://devops.com/learning-from-the-past-what-automation-mistakes-can-teach-us-about-ai/)',
    'Recent AI developments focus on improved infrastructure, integration with real-time workflows, and expanded solutions across Microsoft environments. New tutorials and workflow guides underline the ongoing incorporation of AI into developer productivity and business operations. Surveys from GitHub’s Octoverse confirm AI’s influence on programming languages, team roles, and automation. This week’s articles also prioritize secure, compliant, and sustainable scaling.',
    1763366400, 'ai', '/ai/roundups/weekly-ai-roundup-2025-11-17', 'TechHub',
    'TechHub', '266EE76AF16C2749ECC8506CF2C577D53B30BDBCB9BF52A96ECE765F738258B2', ',Azure AI,Azure OpenAI,SignalR,Azure Key Vault,Microsoft Entra ID,Microsoft Agent Framework,Durable Task,Azure AI Foundry,Model Router,Semantic Kernel,.NET 10,ASP.NET Core,MCP,Azure Logic Apps,Microsoft Copilot Studio,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-11-10
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-11-10', 'roundups', 'Weekly AI Roundup: Agents, Azure Containers, and Edge Inference',
    'AI updates this week extend recent trends in agent-based workflows, tighter Azure integration, and developer tool expansion. Resources focus on practical workflow patterns, actionable case studies, and new options for edge and containerized deployment, supporting teams building advanced intelligent apps with Microsoft services.
<!--excerpt_end-->
## Generative AI and Containerized Workflows on Azure
Technical comparisons for CPU and GPU containerized Stable Diffusion—using Spring Boot Java, ONNX Runtime, and CUDA—add to previous Azure GPU onboarding recommendations. ND GB200-v6 VMs and NVIDIA GB300 improvements show scalable deployment potential. Tutorials cover ONNX/CUDA version strategy and cloud deployment practices. Pipeline automation and session management with Copilot and Claude Sonnet 4.5 build on recent integration themes. The “Java and AI for Beginners” series continues, emphasizing modern Java app development and responsible GenAI use on Azure.
- [Scaling Generative AI with GPU-Powered Containers on Azure](/ai/videos/scaling-generative-ai-with-gpu-powered-containers-on-azure)
- [Running GenAI in Containers: Dynamic Sessions with Azure Container Apps and LangChain4j](/ai/videos/running-genai-in-containers-dynamic-sessions-with-azure-container-apps-and-langchain4j)
- [Java and AI for Beginners: Full Series on Building and Modernizing AI-Powered Java Apps](/ai/videos/java-and-ai-for-beginners-full-series-on-building-and-modernizing-ai-powered-java-apps)
## AI Agents: Orchestration, Orchestration Patterns, and Integration
Guides covering .NET 9 and the Microsoft Agent Framework describe approaches for architecting multi-agent systems, continuing last week’s progress on orchestration. The ChatClientAgent solution provides modular orchestration and repeatable DevOps deployment. LangChain4j continues as a primary tool for Java multi-agent orchestration, with new tutorials and workflow patterns. Recent analysis of agent vs. chatbot architectures supplies actionable insights for agent-enabled Azure development. AiGen adoption in .NET expands agent capabilities beyond traditional chat applications.
- [Client-Side Multi-Agent Orchestration with ChatClientAgent on Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-3-client-side-multi-agent-orchestration-on-azure-app/ba-p/4466728)
- [Building Multi-Agent AI Systems with LangChain4j and Java](/ai/videos/building-multi-agent-ai-systems-with-langchain4j-and-java)
- [Armchair Architects: Defining AI Agents and Their Core Traits](/ai/videos/armchair-architects-defining-ai-agents-and-their-core-traits)
- [Beyond Chat: Building Smarter AI Agents in .NET with AiGen](/ai/videos/beyond-chat-building-smarter-ai-agents-in-net-with-aigen)
## Enterprise AI and Real-World Case Studies
Case studies demonstrate offline, low-latency deployment of models in industries such as healthcare, education, and agriculture across Africa, following last week’s coverage of edge AI and PIKE-RAG frameworks. Technical articles explain PIKE-RAG’s customer service accuracy and describe new Azure AI Foundry and UiPath integrations for automating healthcare agent workflows, continuing integration topics from earlier SAP and ServiceNow updates.
- [Democratizing AI in Africa: Fastagger and Microsoft Enable On-Device AI for SMBs](https://news.microsoft.com/source/emea/features/connecting-africa-to-opportunities-by-closing-the-digital-divide/)
- [Signify Boosts Customer Service Accuracy with Microsoft PIKE-RAG on Azure](https://www.microsoft.com/en-us/research/blog/when-industry-knowledge-meets-pike-rag-the-innovation-behind-signifys-customer-service-boost/)
- [Driving ROI with Azure AI Foundry and UiPath: Intelligent Agents in Healthcare Workflows](https://azure.microsoft.com/en-us/blog/driving-roi-with-azure-ai-foundry-and-uipath-intelligent-agents-in-real-world-healthcare-workflows/)
## Language, Vision, and AI API Tooling
Recent AI development tools include Microsoft’s new image model, MAI-Image-1, which enhances image rendering options in Bing Image Creator and Copilot Audio Expressions. The Azure AI Translator API now offers tone, gender, and style options for multilingual app development in TypeScript, building on prior language tool updates. Mistral Document AI provides structured OCR in regulated environments through TypeScript workflow examples. Microsoft Fabric Data Agent SDK features debugging and multitasking updates for more reliable data agent creation.
- [Introducing MAI-Image-1: Microsoft’s In-House Image Generation Model in Bing Image Creator and Copilot Audio Expressions](https://microsoft.ai/news/introducing-mai-image-1-debuting-in-the-top-10-on-lmarena/)
- [Building Adaptive Multilingual Apps Using TypeScript and Azure AI Translator API](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-adaptive-multilingual-apps-using-typescript-and-azure/ba-p/4465267)
- [Unlock Structured OCR in TypeScript with Mistral Document AI on AI Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/unlock-structured-ocr-in-typescript-with-mistral-document-ai-on/ba-p/4466039)
- [Enhancements for Data Agent Creators in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/creator-improvements-in-the-data-agent/)
## Building Trust, Cost-Efficiency, and Edge/Offline AI
Guides emphasize practical steps for human-centered testing, maximizing cost-efficiency on Azure AI, and enabling hybrid inference with Windows AI Foundry. The human-centered testing guide provides actionable feedback methods; cost optimization and FinOps materials detail sustainable management practices. Windows AI Foundry enables rapid, private local inference with options for cloud fallback.
- [Building AI Apps That Earn User Trust: Human-Centered Testing and Continuous Feedback](https://devops.com/from-code-to-confidence-building-ai-apps-that-earn-user-trust/)
- [Maximize the Cost Efficiency of AI Agents on Azure: Comprehensive Learning Path](https://techcommunity.microsoft.com/t5/azure-governance-and-management/empower-smarter-ai-agent-investments/ba-p/4466010)
- [On-Device AI with Windows AI Foundry: Local Inference for Fast, Private Apps](https://techcommunity.microsoft.com/t5/microsoft-developer-community/on-device-ai-with-windows-ai-foundry/ba-p/4466236)
## Java AI Application Development and Unified Workflows
Extending last week''s Java resources, new guides cover dependency management for Java 24, Maven BOM usage, cloud integration, and vendor-neutral APIs for chat models. These materials support productivity improvements for Java developers working with Copilot.
- [Building Intelligent AI Applications with Java, Spring Boot, and LangChain4j](/ai/videos/building-intelligent-ai-applications-with-java-spring-boot-and-langchain4j)',
    'AI updates this week extend recent trends in agent-based workflows, tighter Azure integration, and developer tool expansion. Resources focus on practical workflow patterns, actionable case studies, and new options for edge and containerized deployment, supporting teams building advanced intelligent apps with Microsoft services.',
    1762761600, 'ai', '/ai/roundups/weekly-ai-roundup-2025-11-10', 'TechHub',
    'TechHub', '313D5457BB67302DAB71CBA6EF20564BFF7726DA186B23636A30636F5C6F4409', ',AI,Azure AI,AI Agents,Agent Orchestration,Microsoft Agent Framework,.NET 9,Java,Spring Boot,LangChain4j,Azure Container Apps,GPU Containers,ONNX Runtime,CUDA,Windows AI Foundry,Azure AI Translator,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-11-03
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-11-03', 'roundups', 'Weekly AI Roundup: Agentic Azure, GPUs, RAG, and Dev Tooling',
    'This week in AI brought updates in cloud infrastructure, new open-source tools, and practical tutorials for developers. Azure expanded its support for agent orchestration platforms and enterprise integrations, and developer-focused toolkits make it easier to build, test, and manage AI-driven solutions. Retrieval-Augmented Generation (RAG), enhanced agent tools, and improved Copilot Studio cost management give developers more robust options for cloud and local AI solutions. Microsoft and NVIDIA’s new partnership brings additional GPU resources and edge computing capabilities. Updates in Java and .NET continue to stress responsible AI development and best practices for enterprise apps.
<!--excerpt_end-->
## Azure Agentic Platforms and Integration Ecosystem
Following last week’s attention to the MCP standard and agent orchestration, Azure MCP Server 1.0.0 is now generally available as an open-source platform connecting over 47 Azure services through the Model Context Protocol. This moves the MCP registry and server to a ready-for-production solution for automated, cross-service management.
The Microsoft Agent Framework for .NET continues to replace Semantic Kernel and AutoGen, giving developers modular agent orchestration and support for workflows that span multiple memory stores. Tutorials walk through examples using Service Bus, Cosmos DB, Application Insights, VNet, and infrastructure-as-code (Bicep), building on the secure hosted agent instructions from recent weeks.
Copilot Studio now supports multi-agent orchestration with SAP, ServiceNow, and Salesforce integrations. Recent guides combine low-code and professional automation approaches, showing how to generate secure business process workflows through Azure Logic Apps and support hybrid automation.
- [Announcing Azure MCP Server 1.0.0 Stable Release – A New Era for Agentic Workflows](https://devblogs.microsoft.com/azure-sdk/announcing-azure-mcp-server-stable-release/)
- [Deep Dive into Microsoft Agent Framework for AutoGen Users](/ai/videos/deep-dive-into-microsoft-agent-framework-for-autogen-users)
- [Building Real-World AI Agents with Agent Framework on .NET Live](/ai/videos/building-real-world-ai-agents-with-agent-framework-on-net-live)
- [Building Multi-Agent AI Systems on Azure App Service with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-2-build-long-running-ai-agents-on-azure-app-service-with/ba-p/4465825)
- [Agentic Integration Patterns: Microsoft Copilot Studio with SAP, ServiceNow, and Salesforce](https://techcommunity.microsoft.com/t5/azure-architecture-blog/agentic-integration-with-sap-servicenow-and-salesforce/ba-p/4466049)
## Azure AI Foundry, GPU Innovations, and Edge AI
Improvements to cloud infrastructure this week include the deployment of the NVIDIA GB300 NVL72 GPU cluster on Azure, with operational reliability delivered by rack-scale cooling and detailed telemetry. The introduction of new SKUs (ND GB200-v6 VMs) supports large models and distributed inference, building on recent increases in available GPU resources.
Azure AI Foundry now integrates NVIDIA Nemotron and Cosmos models, making it easier to orchestrate generative and simulation applications. Run:ai orchestration provides improved GPU allocation and budget efficiency. Additional edge support through Azure Arc and RTX PRO 6000 Blackwell expands on previous local and hybrid cloud guides.
- [Reimagining AI at Scale: Deploying NVIDIA GB300 NVL72 on Azure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/reimagining-ai-at-scale-nvidia-gb300-nvl72-on-azure/ba-p/4464556)
- [Microsoft and NVIDIA Announce Major AI Advancements for Azure AI and Edge](https://azure.microsoft.com/en-us/blog/building-the-future-together-microsoft-and-nvidia-announce-ai-advancements-at-gtc-dc/)
## Retrieval-Augmented Generation and Storage Workflows
Advances in RAG pipelines continue from last week, now with the release of the langchain-azure-storage Python package. This tool adds OAuth2 document ingestion and metadata extraction, supporting secure RAG workflows for Python and customers using AI Foundry.
- [Introducing langchain-azure-storage: Azure Storage Integration with LangChain](https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-langchain-azure-storage-azure-storage-integrations/ba-p/4465268)
## AI-Powered Developer Tools and Agent Management Platforms
AI-powered developer tools gain new modular orchestration and management capabilities. Cursor 2.0 brings faster code completion with the Composer model, better long-context memory, and an interface supporting multi-agent workflows. This continues to build on last week’s progress with pluggable agents.
Anthropic’s Claude Agent Skills feature modular workflow skills for development and coding, reinforcing the move towards extensible frameworks. GitHub’s Agent HQ now consolidates agent management for both CLI and IDE use, marking a shift from registry/server-based management to unified control.
- [Cursor 2.0 Brings Faster AI Coding and Multi-Agent Workflows](https://devops.com/cursor-2-0-brings-faster-ai-coding-and-multi-agent-workflows/)
- [Claude Introduces Agent Skills for Custom AI Workflows](https://devops.com/claude-introduces-agent-skills-for-custom-ai-workflows/)
- [Introducing Agent HQ: Your Mission Control for AI Agents](/ai/videos/introducing-agent-hq-your-mission-control-for-ai-agents)
## .NET and Java: Responsible AI, Evaluation, and Application Patterns
.NET and Java updates maintain an emphasis on responsible AI development and practical integration. New Java samples demonstrate how to incorporate Azure AI Content Safety for filtering and blocking, complementing past coverage of safety guardrails. Further resources explore monitoring, abuse prevention, and enterprise best practices.
Microsoft.Extensions.AI.Evaluation adds support for automated AI testing using MSTest, xUnit, and Azure DevOps, helping to automate NLP and custom AI validation processes—building on last week’s test frameworks.
New Java tutorials extend the multi-week focus on RAG, generative apps, and hybrid cloud. They now include Codespaces workflows, multi-turn chat, LLM completions, and comparisons of MCP, browser LLMs, and Foundry Local.
- [Responsible AI for Java Developers: Building Safe and Trustworthy Applications](/ai/videos/responsible-ai-for-java-developers-building-safe-and-trustworthy-applications)
- [Put your AI to the Test with Microsoft.Extensions.AI.Evaluation](https://devblogs.microsoft.com/blog/put-your-ai-to-the-test-with-microsoft-extensions-ai-evaluation)
- [Getting Started with Generative AI for Java Developers Using GitHub Codespaces](/ai/videos/getting-started-with-generative-ai-for-java-developers-using-github-codespaces)
- [GenAI for Java Developers 2: Core Techniques Explained](/ai/videos/genai-for-java-developers-2-core-techniques-explained)
- [Building Three AI-Powered Applications: MCP, Browser LLMs, and Foundry Local](/ai/videos/building-three-ai-powered-applications-mcp-browser-llms-and-foundry-local)
- [Intro to Java and AI for Beginners](/ai/videos/intro-to-java-and-ai-for-beginners)
## Copilot Studio Cost Management and Migration
Copilot Studio cost management updates keep the recent focus on billing, model updates, and lifecycle planning. The Credit Pre-Purchase Plan (P3) introduces cost estimation, monitoring, and discounts, supporting budget management and migration best practices.
GitHub’s model deprecation notice provides up-to-date migration resources and documentation for improved CI/CD and compatibility.
- [Streamline Copilot Studio Costs with the Pre-Purchase (P3) Plan](https://techcommunity.microsoft.com/t5/finops-blog/unlock-savings-with-copilot-credit-pre-purchase-plan/ba-p/4464511)
- [Cost Optimization with Copilot Credit Pre-Purchase Plan for Microsoft Copilot Studio](https://techcommunity.microsoft.com/t5/azure-compute-blog/unlock-savings-with-copilot-credit-pre-purchase-plan/ba-p/4464517)
- [Deprecation Notice: Updates to GitHub Models and Migration Guidance](https://github.blog/changelog/2025-10-31-deprecated-models-in-github-models)
## AI-Enabled Application Patterns and Octoverse Trends
Application integration guides now offer workflow refinement and Power Automate integration for enterprise data modeling and management, building on previous Dataverse/Copilot Studio content. Emphasis on modular workflows provides a foundation for long-lasting, flexible AI solutions.
The Octoverse 2025 report summarizes ongoing trends in generative AI, adoption, and global architectural robustness.
- [Using Copilot Studio with Dataverse: A Developer’s Guide](https://dellenny.com/using-copilot-studio-with-dataverse-a-developers-guide/)
- [Octoverse 2025: AI, India, and TypeScript''s Rise](/ai/videos/octoverse-2025-ai-india-and-typescripts-rise)
## Other AI News
Guides on Small Language Models (SLMs) provide ongoing direction for device-centric intelligence in edge, healthcare, and robotics use cases.
The November 2025 Innovation Challenge highlights Microsoft’s commitment to skill-building in Azure AI, focusing on opportunities for underrepresented groups.
- [Understanding Small Language Models: The Role of SLMs and Microsoft Azure AI Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/understanding-small-language-modes/ba-p/4462827)
- [Announcing the November 2025 Innovation Challenge Hackathon](https://techcommunity.microsoft.com/t5/azure/announcing-the-november-2025-innovation-challenge-hackathon/m-p/4464518#M22287)',
    'This week in AI brought updates in cloud infrastructure, new open-source tools, and practical tutorials for developers. Azure expanded its support for agent orchestration platforms and enterprise integrations, and developer-focused toolkits make it easier to build, test, and manage AI-driven solutions. Retrieval-Augmented Generation (RAG), enhanced agent tools, and improved Copilot Studio cost management give developers more robust options for cloud and local AI solutions. Microsoft and NVIDIA’s new partnership brings additional GPU resources and edge computing capabilities. Updates in Java and .NET continue to stress responsible AI development and best practices for enterprise apps.',
    1762156800, 'ai', '/ai/roundups/weekly-ai-roundup-2025-11-03', 'TechHub',
    'TechHub', 'BC1C9CBBD69C0520A6FF7A5DCC6CC753081640315BA3A46DE8FA9B898BC0DA72', ',AI,Azure,AI Agents,MCP,Azure MCP Server,Microsoft Agent Framework,Copilot Studio,Azure AI Foundry,NVIDIA Blackwell,GPU Clusters,Edge AI,Retrieval Augmented Generation,LangChain,.NET,Java,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-10-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-10-27', 'roundups', 'Weekly AI Roundup: Agents, Signals Loops, and Private Edge AI',
    'Recent AI developments include growth in agent frameworks, better cloud and edge integrations, and updates in automation and security. Microsoft’s platforms—Agent Framework, MCP, Copilot Studio, and Azure AI Foundry—continue to put agents at the core of cloud-native, privacy-aware, and edge scenarios. Updates include new patterns in multi-agent orchestration, ongoing monitoring, signals-driven adjustments, and lifecycle management, with concrete progress on developer productivity and compliance.
<!--excerpt_end-->
## Microsoft Agent Framework: From Application Upgrades to Multi-Agent Enterprise Orchestration
Advancing last week’s discussion of MCP and agent-based workflows, Agent Framework adds more features and new integration options. Step-by-step guides and workshops help developers move from older Semantic Kernel and Blazor chat applications to orchestrated, multi-agent solutions in .NET 9. Support for enterprise orchestration—including sequential and concurrent workflow patterns—continues the focus on modularity. OpenTelemetry and DevUI integrations improve observability, while hosting agents on Azure App Service and using Cosmos DB for agent state expand guidance toward scalable, production deployments.
Multi-agent coordination with MCP and audit features carry forward the open-source registry approach for enterprise adoption. New SentinelStep and SentinelBench tools are maturing agent lifecycle and reliability standards for sustained workloads.
- [Upgrading to Microsoft Agent Framework in Your .NET AI Chat App](https://devblogs.microsoft.com/dotnet/upgrading-to-microsoft-agent-framework-in-your-dotnet-ai-chat-app/)
- [.NET AI Community Standup: Microsoft Agent Framework Workflows & Migration](/ai/videos/net-ai-community-standup-microsoft-agent-framework-workflows-and-migration)
- [Multi-Agent Orchestration in Enterprise AI with Microsoft Agent Framework](https://devblogs.microsoft.com/semantic-kernel/unlocking-enterprise-ai-complexity-multi-agent-orchestration-with-the-microsoft-agent-framework/)
- [Build Long-Running AI Agents on Azure App Service with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-long-running-ai-agents-on-azure-app-service-with-microsoft/ba-p/4463159)
- [Orchestrating Multi-Agent Intelligence with Microsoft Agent Framework and MCP](https://techcommunity.microsoft.com/t5/microsoft-developer-community/orchestrating-multi-agent-intelligence-mcp-driven-patterns-in/ba-p/4462150)
- [Building Persistent Monitoring Agents with SentinelStep in Magentic-UI](https://www.microsoft.com/en-us/research/blog/tell-me-when-building-agents-that-can-wait-monitor-and-act/)
- [Microsoft Agent Framework WorkFlows Explained](/ai/videos/microsoft-agent-framework-workflows-explained)
- [Microsoft Agent Framework Workflows: Deep Dive and Advanced Concepts](/ai/videos/microsoft-agent-framework-workflows-deep-dive-and-advanced-concepts)
## Adaptive AI: Signals Loops, Agentic Lifecycle, and Fine-Tuning in Azure AI Foundry
Building on last week''s coverage of model fine-tuning, this week brings more emphasis on signals loops and continuous improvement for agentic AI. Agentic Lifecycle Management (ALM) now formalizes agent development and governance, expanding feedback cycles.
More business-centric feedback processes extend previous GPT-4o-mini documentation automation, supporting quality checks and autonomous code review. Step-by-step resources for enterprise adoption now include operational details such as telemetry pipelines and MELT stack tools. The nine safety guardrails maintain a consistent focus on secure deployments and agent monitoring.
- [The Signals Loop: Fine-Tuning for World-Class AI Apps and Agents](https://azure.microsoft.com/en-us/blog/the-signals-loop-fine-tuning-for-world-class-ai-apps-and-agents/)
- [The Developer’s Guide to Agentic AI: The Five Stages of Agent Lifecycle Management](https://devops.com/the-developers-guide-to-agentic-ai-the-five-stages-of-agent-lifecycle-management/)
- [Rewriting the Rules of Software Quality: Why Agentic QA is the Future CIOs Must Champion](https://devops.com/rewriting-the-rules-of-software-quality-why-agentic-qa-is-the-future-cios-must-champion/)
- [The Agentic AI-Driven Future of Telemetry](https://devops.com/the-agentic-ai-driven-future-of-telemetry/)
- [Before You Deploy AI Agents in Observability: Nine Key Guardrails for Safety](https://devops.com/before-you-go-agentic-top-guardrails-to-safely-deploy-ai-agents-in-observability/)
## Edge and On-Premises AI: Azure AI Foundry Local Deepens Real-Time and Private Workloads
Azure AI Foundry Local advances last week''s toolkit discussion, offering local and edge deployment options to meet stricter privacy and compliance requirements found in healthcare, energy, and regulated settings.
SDKs designed for multiple frameworks, support for low-latency inference, and GPU acceleration tutorials continue last week’s focus on automation and container hosting. These features expand agentic AI intelligence to edge and on-prem infrastructure.
- [A Developer’s Guide to Edge AI with Microsoft Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/transform-your-ai-applications-with-local-llm-deployment/ba-p/4462829)
- [Understanding Azure AI Foundry Local: On-Premises AI for Privacy and Security](/ai/videos/understanding-azure-ai-foundry-local-on-premises-ai-for-privacy-and-security)
- [What is Azure AI Foundry Local?](/ai/videos/what-is-azure-ai-foundry-local)
## Retrieval-Augmented Generation and Hybrid Search: Building Smarter Enterprise Applications
RAG pipelines with Azure AI Search and OpenAI Service extend the scope of smart documentation and embedding search covered last week. New how-to articles, prompt methods, and troubleshooting tips build on distributed inference and dynamic routing with GPT-4o-mini. The introduction of BYOPI (Build Your Own Private Indexer) moves deployment resources toward secure, hybrid-compliant scenarios for regulated businesses.
- [Unlocking Smarter Search: Integrating Azure AI Search and Azure OpenAI Service](https://dellenny.com/unlocking-smarter-search-how-to-use-azure-ai-search-azure-openai-service-together/)
- [BYOPI: Building a Custom Private Azure AI Search Indexer with Azure Data Factory](https://techcommunity.microsoft.com/t5/azure/byopi-design-your-own-custom-private-ai-search-indexer-with-no/m-p/4464205#M22283)
## Model Context Protocol (MCP), Registry, and Serverless GenAI on Azure
Open-source MCP registries and related tools remain important for agentic collaboration. New articles detail server setup, security integration, and CLI adoption, marking increased use and operational stability. Serverless GenAI guides for LangChain.js v1 build on last week’s MCP preview, providing concrete deployment and observability directions for Azure.
Articles focus on authentication, CI/CD automation, and allowlist management, building on last week''s compliance coverage and strengthening secure and scalable AI deployments.
- [How to Find, Install, and Manage MCP Servers with the GitHub MCP Registry](https://github.blog/ai-and-ml/generative-ai/how-to-find-install-and-manage-mcp-servers-with-the-github-mcp-registry/)
- [Serverless MCP Agent with LangChain.js v1: Full-Stack GenAI Deployment on Azure](https://techcommunity.microsoft.com/t5/microsoft-developer-community/serverless-mcp-agent-with-langchain-js-v1-burgers-tools-and/ba-p/4463157)
## Developer AI Agents and Agentic Automation: From Productivity to Open Source Extension
Open source agent extensions and developer automation continue the series of updates related to Copilot Studio and developer-tier guides. The SRE Agent and community modules keep building out the ecosystem with flexible, modular AI developer tools—a shift begun last week. New extensions make it easier to increase productivity and adapt to workflow changes.
- [Advancements in Developer Agents: Task Automation and Open Source Extensions](/ai/videos/advancements-in-developer-agents-task-automation-and-open-source-extensions)
- [Agents for Developers: Latest Advancements in Developer Productivity](/ai/videos/agents-for-developers-latest-advancements-in-developer-productivity)
## Small Language Models and Local Agentic Workflows in .NET
New tutorials on small language models in .NET expand last week’s focus on agent orchestration and autonomous workflows built with Semantic Kernel. Step-by-step articles provide code samples for embedding retrieval-augmented generation and agentic patterns locally, transitioning from theory to actionable implementation.
- [Building Agentic AI Systems in .NET with Local SLMs](/ai/videos/building-agentic-ai-systems-in-net-with-local-slms)
## Multi-Agent Orchestration: Patterns and Practical Integration with Azure AI Foundry
Azure Essentials guides advance last week’s coverage of modular orchestration frameworks, now introducing reusable patterns and advanced integration options. The move from concepts to practical cloud-native multi-agent governance is supported with actionable resources.
- [Optimize Complex Workflows with Multi-Agent AI Patterns in Azure](/ai/videos/optimize-complex-workflows-with-multi-agent-ai-patterns-in-azure)
- [Optimize Complex Workflows Using Multi-Agent AI Patterns](https://www.thomasmaurer.ch/2025/10/optimize-complex-workflows-using-multi-agent-ai-patterns/)
## Microsoft Copilot Studio: Expanded Customization, Automation, Integration
September 2025 Copilot Studio updates continue last week’s coverage of customizable agents and prompt engineering. New features like hosted browser options and WhatsApp integration provide additional agent workflow choices, while evaluation tools and analytics dashboards connect to past topics in enterprise management. The Agent Academy adds new training resources, extending the focus on developer education.
- [September 2025 Updates: New Features and Enhancements in Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/whats-new-in-copilot-studio-september-2025/)
## AI-Driven Code Quality and Security: Analysis, Tools, and DevOps Integration
Progress in code security and validation for AI-generated output builds on last week’s SonarSweep coverage. New tools for detecting code anti-patterns reinforce previous themes of AI-powered quality assurance, supporting enterprise-level review and best practices.
- [10 AI Coding Tool Behaviors That Ignore Software Engineering Best Practices](https://devops.com/analysis-identifies-10-ai-coding-tool-behaviors-that-ignore-best-software-engineering-practices/)
- [SonarSweep: Improving AI-Generated Code Quality and Security](https://devops.com/sonar-previews-service-to-improve-quality-of-ai-generated-code/)
## AI Workflow Reusability, Developer Experience, and the Future of Web Development
Workflow modularity and reuse continue to stand out, following last week’s cataloging focus. New guides present strategies for versioning, repeatable design, and future agentic interfaces in web development—helping teams achieve operational efficiency and plan durable projects.
- [Don’t Reinvent the Wheel: A Developer’s Guide to AI Reusability](https://devops.com/dont-reinvent-the-wheel-a-developers-guide-to-ai-reusability/)
- [The Future of Web Development with AI Integration](/ai/videos/the-future-of-web-development-with-ai-integration)
## AI, Cloud, and Security for Startups: Ignite 2025 Conference
The Ignite 2025 agenda parallels last week’s discussions on ecosystem strategies, with tips for startups regarding Copilot, Azure AI Foundry, compliance, and agentic AI product design. Topics such as generative AI, SDK extension, and marketplace readiness remain at the forefront, keeping the material relevant for builders and the broader community.
- [Microsoft Ignite 2025: Shaping the Future of AI, Cloud, and Security for Startups](https://www.microsoft.com/en-us/startups/blog/why-startups-shouldnt-miss-microsoft-ignite-2025-a-front-row-seat-to-the-future-of-ai-innovation/)',
    'Recent AI developments include growth in agent frameworks, better cloud and edge integrations, and updates in automation and security. Microsoft’s platforms—Agent Framework, MCP, Copilot Studio, and Azure AI Foundry—continue to put agents at the core of cloud-native, privacy-aware, and edge scenarios. Updates include new patterns in multi-agent orchestration, ongoing monitoring, signals-driven adjustments, and lifecycle management, with concrete progress on developer productivity and compliance.',
    1761552000, 'ai', '/ai/roundups/weekly-ai-roundup-2025-10-27', 'TechHub',
    'TechHub', '473A5E1CF305A5E85DAC4CF947BCF3A95258F520517F20B6D4BE613E7E04015A', ',AI Agents,Multi Agent Orchestration,Microsoft Agent Framework,MCP,Azure AI Foundry,Azure AI Foundry Local,Copilot Studio,Retrieval Augmented Generation,Azure AI Search,Azure OpenAI Service,Observability,OpenTelemetry,DevOps,Security Guardrails,.NET 9,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-10-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-10-20', 'roundups', 'Weekly AI Roundup: Agents, Azure AI Foundry, and AI ops',
    'This week’s AI news centers around deeper integration of language models, agent frameworks, and cloud infrastructure, particularly on Azure. You’ll find updates to documentation automation, workload-focused model optimization, and infrastructure architecture, building a foundation for scalable and secure enterprise AI adoption.
<!--excerpt_end-->
## Azure AI Foundry: Model Development, Fine-Tuning, and Multimodal Expansion
Azure AI Foundry is positioned as the main hub for creating and managing AI applications. The latest release of LangChain v1 introduces initial Foundry support, improved agent APIs, and updated migration guides. Richer UI integration is now possible thanks to new structured content blocks, and expanded I/O supports text, images, files, and video for better functionality across real-world applications.
Highlights in fine-tuning include expanded support for Reinforcement Fine-Tuning (RFT), a more cost-effective Developer Tier, easier APIs for LLM customization (such as GPT-4o), and accelerated evaluation and deployment. Code samples are provided for RFT, distillation, and multi-region rollout.
Azure AI Foundry introduces Sora 2 in public preview, letting organizations use a REST API to generate detailed, physics-aware video with audio. This makes secure, scalable content generation for education and marketing simpler, following updates to Azure’s multimodal support.
- [LangChain v1 Launches with Azure AI Foundry Support and Streamlined Agent APIs](https://techcommunity.microsoft.com/t5/microsoft-developer-community/langchain-v1-is-now-generally-available/ba-p/4462159)
- [The Developer’s Guide to Smarter Fine-tuning with Azure AI Foundry](https://devblogs.microsoft.com/foundry/the-developers-guide-to-smarter-fine-tuning/)
- [Sora 2 Public Preview Now Available in Azure AI Foundry](https://azure.microsoft.com/en-us/blog/sora-2-now-available-in-azure-ai-foundry/)
## Intelligent Documentation and GPT-4o Optimization
Efforts to automate documentation now combine natural language processing, large language models, retrieval-augmented generation (RAG), and embedding search. Integrating with VS Code and JetBrains can save up to 80% of manual effort. These workflows use distributed inference, optimize real-time delivery, and scale with vector stores.
A case study with GPT-4o-mini on Azure OpenAI Service identifies strategies to handle throttling and timeouts—using token capping, streaming, and regional routing—to lower error rates and costs. Enhanced diagnostics and API management support stable large-scale deployments.
- [NLP Tools for Intelligent Documentation and Developer Enablement](https://devops.com/nlp-tools-for-intelligent-documentation-and-developer-enablement-2/)
- [From Timeouts to Triumph: Optimizing GPT-4o-mini for Speed, Efficiency, and Reliability](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-timeouts-to-triumph-optimizing-gpt-4o-mini-for-speed/ba-p/4461531)
## Building and Deploying AI Agents: Container Apps, Open Source Orchestration, and MCP
Hosting agentic AI is now easier, with agents like Goose scaling on Azure Container Apps, benefiting from managed GPUs and secure setups. Quickstart guides help teams deploy agents efficiently.
Archestra, featured in Open Source Friday, is built on the Microsoft Cloud Platform and allows secure, permissioned orchestration of agents and models. The MCP registry progresses, helping standardize context metadata and support effective collaboration among developers of open-source AI tools.
- [Building Agents on Azure Container Apps with Goose AI Agent, Ollama, and gpt-oss](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-agents-on-azure-container-apps-with-goose-ai-agent/ba-p/4460215)
- [Open Source Friday: Archestra – Secure Platform for Enterprise Agents with MCP](/ai/videos/open-source-friday-archestra-secure-platform-for-enterprise-agents-with-mcp)
- [Unlocking the Power of MCP: Model Context Protocol in Open Source AI Tools](/ai/videos/unlocking-the-power-of-mcp-model-context-protocol-in-open-source-ai-tools)
## AI Workflows and Developer Empowerment in the Enterprise
Enterprise teams continue to implement open standards like MCP, making AI solutions easier to integrate. Modular agent frameworks and tools such as Copilot Studio help create tailored “digital twin” AI agents for specific domains, with secure integration and custom prompt support.
Information shows that developers are rapidly automating tasks with AI, though project managers tend to adopt at a slower rate, pointing to ongoing needs for training and organizational involvement.
- [How Developers Are Leading AI Transformation Across the Enterprise](https://www.microsoft.com/en-us/microsoft-cloud/blog/2025/10/13/fyai-why-developers-will-lead-ai-transformation-across-the-enterprise/)
- [Digital Twin Employees: Hyper-Personalized AI Prompts with Copilot Studio](https://dellenny.com/the-digital-twin-employee-creating-hyper-personalized-copilot-prompts-with-copilot-studio/)
- [Survey Finds Developers Adopting AI More Rapidly Than Project Managers](https://devops.com/survey-sees-developers-embracing-ai-faster-than-project-leaders/)
## AI in Healthcare and Regulated Workflows
Microsoft has expanded Dragon Copilot’s AI features to nursing and clinical processes, letting partners create custom content and automate documentation. These improvements help reduce administrative work and support more efficient, ambient workflows.
A GenAI Solution Accelerator for energy permitting uses AI to automate approvals and compliance paperwork, highlighting new adoption in regulated industries.
- [Microsoft Expands Dragon Copilot AI Innovations for Nursing and Healthcare Partners](https://news.microsoft.com/source/2025/10/16/microsoft-extends-ai-advancements-in-dragon-copilot-to-nurses-and-partners-to-enhance-patient-care/)
- [Microsoft Introduces Dragon Copilot Ambient AI Experience for Nursing Workflows](https://www.linkedin.com/posts/satyanadella_no-question-that-nurses-are-the-heartbeat-activity-7384590573808234496-aaYh)
- [Microsoft GenAI for Energy Permitting Solution Accelerator](/ai/videos/microsoft-genai-for-energy-permitting-solution-accelerator)
## AI Infrastructure and Datacenter Operations
OpenAI, Microsoft, and Nvidia are using new methods to stabilize power for GPU-based AI training, including software controls, hardware management, and rack-level storage to support datacenter operations.
The Open Compute Project supports standardized APIs and onboarding processes, making it easier to manage infrastructure with mixed CPU and GPU resources, while promoting resilient AI clusters.
- [Power Stabilization Strategies for AI Training Datacenters](https://techcommunity.microsoft.com/t5/azure-compute-blog/power-stabilization-for-ai-training-datacenters/ba-p/4460937)
- [Operational Excellence in AI Infrastructure: Standardized Node Lifecycle Management](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/operational-excellence-in-ai-infrastructure-fleets-standardized/ba-p/4460754)
## Other AI News
The CX Observe Product Feedback Copilot converts support and survey feedback into actionable product insights, supporting data-driven workflows integrated with Azure and extending ongoing automation trends.
- [CX Observe Product Feedback Copilot: AI-Powered Insights for Azure Product Leaders](https://www.microsoft.com/en-us/garage/wall-of-fame/cx-observe-product-feedback/)',
    'This week’s AI news centers around deeper integration of language models, agent frameworks, and cloud infrastructure, particularly on Azure. You’ll find updates to documentation automation, workload-focused model optimization, and infrastructure architecture, building a foundation for scalable and secure enterprise AI adoption.',
    1760943600, 'ai', '/ai/roundups/weekly-ai-roundup-2025-10-20', 'TechHub',
    'TechHub', '11F2585286B3697C98534888A1F3163691A48BCD4DBF2CD7CB2BB5EFC26204D2', ',Azure AI Foundry,Azure OpenAI Service,LangChain,AI Agents,MCP,Retrieval Augmented Generation,Vector Search,Embeddings,Fine Tuning,Reinforcement Fine Tuning,GPT 4o,Azure Container Apps,Multimodal AI,Video Generation,AI Infrastructure,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-10-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-10-13', 'roundups', 'Weekly AI Roundup: Foundry Models, Agents, and Scaling on Azure',
    'Microsoft’s AI ecosystem made progress this week with Azure AI Foundry’s new releases, updated developer tools, and infrastructure enhancements. Advances include updated models, agent frameworks, workflow orchestration, and documentation for multimodal generation, voice solutions, and agent scaling. The highlights feature GPT-5-Codex and Sora in Azure AI Foundry, previews of multimodal models, new automation tools, and security/compliance resources. Investments in infrastructure and improved developer experience remain essential, supporting open source practices and accessible adoption paths for teams.
<!--excerpt_end-->
## Azure AI Foundry: Model Rollouts, Tools, Frameworks, and Security
Following last week’s Grok 4 preview and protocol adoption, Azure AI Foundry’s September update introduces the general availability of GPT-5-Codex for advanced code use-cases and migration tasks. Sora’s preview provides video-to-video editing with natural language and extends prior multimodal agent workflows.
Grok 4 Fast models are now in preview, supporting parallel calls and longer context sessions. Foundry Local v0.7 adds dynamic NPU discovery for hybrid deployments, continuing improvements in local/cloud AI. Microsoft Agent Framework has been open sourced for multi-agent orchestration and includes Semantic Kernel capabilities for enterprise applications.
Enhancements like browser automation, Azure AI Search improvements, and Key Vault integration support better workflow management and security needs. Updates to documentation and SDKs reflect ongoing developer feedback, echoing previous onboarding and integration efforts. Model documentation and pricing roll out beginning October 7.
- [What’s New in Azure AI Foundry: September 2025 Feature Roundup](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-september-2025/)
- [Azure AI Foundry Launches Multimodal AI Models for Developers](https://azure.microsoft.com/en-us/blog/unleash-your-creativity-at-scale-azure-ai-foundrys-multimodal-revolution/)
## Agentic AI Patterns, Enterprise Bots, and Multi-Agent Orchestration
New guides detail how to use agentic AI in enterprise scenarios by combining Azure tools, GPT models, and services for autonomous decision-making. Strategies for enhancing Copilot bots with Azure OpenAI Services employ RAG pipelines, vector databases, and the GPT Assistants API, building from recent tutorials.
The Microsoft Agent Framework, now open source, connects features from Semantic Kernel and AutoGen for broader .NET adoption. Tutorials help developers set up agent orchestration, app intelligence, and operational lifecycle management for new and migrating applications.
Enterprise scenarios—including bots and compliance agents—have updated guides for best practices and architecture expansion.
- [Enhancing Copilot Bots with Azure OpenAI Services](https://dellenny.com/enhancing-copilot-bots-with-azure-openai-services/)
- [How Agentic AI Works and How to Build It in Azure](https://dellenny.com/how-agentic-ai-works-and-how-to-build-it-in-azure/)
- [Semantic Kernel and Microsoft Agent Framework: Evolution and Future Support](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-agent-framework/)
- [Getting Started with the Microsoft Agent Framework in .NET](/ai/videos/getting-started-with-the-microsoft-agent-framework-in-net)
## Infrastructure Upgrades and Enterprise AI Adoption
Microsoft introduced a new supercomputing cluster with 4,600+ NVIDIA GB300 GPUs and InfiniBand networking, improving AI training and inference at large scales. This builds on infrastructure for Copilot, ChatGPT, and enterprise applications, aimed at enhanced reliability and development speed.
Enterprise AI resources expand last week’s Azure AI Landing Zone guidance, covering identity management, compliance, and modular deployments. Step-by-step guides support teams from pilot to production, continuing focus on secure scaling.
- [Microsoft Unveils Supercomputing Cluster with 4600+ NVIDIA GB300 GPUs for Next-Gen AI Workloads](https://www.linkedin.com/posts/satyanadella_another-first-for-our-ai-fleet-a-supercomputing-activity-7382086397152858113-BlSd)
- [Accelerating Enterprise AI Adoption with Azure AI Landing Zone](https://techcommunity.microsoft.com/t5/azure-architecture-blog/accelerating-enterprise-ai-adoption-with-azure-ai-landing-zone/ba-p/4460199)
- [How to Build Frontier AI Solutions with Azure AI Landing Zone](https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-to-build-frontier-ai-solutions-with-azure-ai-landing-zone/ba-p/4460199)
## Streaming, Concurrency, and App Patterns for LLMs
Developers aiming for better responsiveness in LLM-powered chat apps receive guidance on streaming, backend relays, NDJSON formats, and token management, continuing earlier focus on parallel processing and prompt engineering.
Concurrency recommendations build on last week’s asynchronous processing advice, highlighting frameworks such as Quart for non-blocking Azure LLM apps. Open repositories and performance advice contribute to continued improvement for scalable developer tools.
- [How to Implement Streaming in Azure LLM-Powered Chat Applications](https://techcommunity.microsoft.com/t5/microsoft-developer-community/the-importance-of-streaming-for-llm-powered-chat-applications/ba-p/4459574)
- [Concurrency Best Practices for LLM-Powered Apps with Azure OpenAI and Python](https://techcommunity.microsoft.com/t5/microsoft-developer-community/why-your-llm-powered-app-needs-concurrency/ba-p/4459584)
## Graph Data Management and Analytics in Microsoft Fabric
Microsoft Fabric adds graph data management and analytics features, supporting advanced relationship modeling and queries within OneLake. These tools expand on last week’s ETL and analytics developments and are designed for explainable AI, fraud detection, and real-time analysis.
- [Introducing Graph Data Management and Analytics in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/graph-in-microsoft-fabric/)
## AI Agents, Coding Teams, and Developer Experience
Agent platforms see continued growth with the general availability of Atlassian Rovo Dev, an automation agent for development planning and review integrated with Atlassian tools. MCP and Teamwork Graph expand workflow and collaboration features, reflecting previous progress in agentic automation.
Frameworks like AutoGen, MetaGPT, CrewAI, and Claudeflow become more common for modular, role-based orchestration. Advice to maintain oversight with automation continues from earlier best practices for trust and transparency.
Tool fragmentation gets attention with solutions for unified development and clearer transparency, as highlighted in earlier onboarding and productivity features.
- [Atlassian Rovo Dev: AI Coding Agent Now Generally Available](https://devops.com/atlassian-makes-ai-coding-agent-generally-available/)
- [Coding Agent Teams: The Next Frontier in AI-Assisted Software Development](https://devops.com/coding-agent-teams-the-next-frontier-in-ai-assisted-software-development/)
- [Developer Experience: Overcoming 6 AI-Induced Challenges](https://devops.com/developer-experience-overcoming-6-ai-induced-challenges/)
## Tutorials, Learning Resources, and Hands-On Guides
Developer resources continue to expand, featuring a nine-part video series on generative AI and Python, building on last week’s agentic Python guides and context management. Office hours and live sessions foster community learning.
Entry-level guides show how to build Azure AI Foundry agents using file search, advancing from last week’s no-code onboarding with easier setup. Additional tutorials for bots and speech cover enterprise NLP and voice solutions.
Copilot Studio’s use cases in financial services follow last week’s enterprise engagement stories.
- [Curso gratuito: IA Generativa y Python - Serie de 9 transmisiones en vivo](https://techcommunity.microsoft.com/t5/microsoft-developer-community/curso-oficial-y-gratuito-de-genai-y-python/ba-p/4459466)
- [Create an AI Agent with File Search in Azure AI Foundry (Portal)](/ai/videos/create-an-ai-agent-with-file-search-in-azure-ai-foundry-portal)
- [Unlocking the Power of Conversational AI with Azure Bot Service](https://dellenny.com/unlocking-the-power-of-conversational-ai-with-azure-bot-service/)
- [Noise-Free, Domain-Specific Voice Recognition with Azure Custom Speech](https://dellenny.com/when-words-matter-noise-free-domain-specific-voice-recognition-with-azure-custom-speech/)
- [AMA: Building Smarter Voice Agents with Azure AI Foundry Voice Live API](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ama-azure-ai-foundry-voice-live-api-build-smarter-faster-voice/ba-p/4460118)
- [How Copilot Studio Improves Customer Engagement in Financial Services](https://dellenny.com/how-copilot-studio-improves-customer-engagement-in-financial-services/)
## Other AI News
Grafana Labs upgraded its observability platform with AI-driven troubleshooting, root cause analysis, and adaptive telemetry. These changes mirror trends in AI-powered monitoring, adding new compliance, discoverability, and efficiency options for scalable operations.
- [Grafana Labs Extends AI Capabilities of Observability Platform](https://devops.com/grafana-labs-extends-ai-capabilities-of-observability-platform/)',
    'Microsoft’s AI ecosystem made progress this week with Azure AI Foundry’s new releases, updated developer tools, and infrastructure enhancements. Advances include updated models, agent frameworks, workflow orchestration, and documentation for multimodal generation, voice solutions, and agent scaling. The highlights feature GPT-5-Codex and Sora in Azure AI Foundry, previews of multimodal models, new automation tools, and security/compliance resources. Investments in infrastructure and improved developer experience remain essential, supporting open source practices and accessible adoption paths for teams.',
    1760338800, 'ai', '/ai/roundups/weekly-ai-roundup-2025-10-13', 'TechHub',
    'TechHub', '361E319425A44BB826B215BFD17118A876437A2D893D5456DE9DB236A636BB15', ',Azure AI Foundry,Azure OpenAI,GPT 5 Codex,Sora,Grok 4 Fast,Multimodal Models,AI Agents,Microsoft Agent Framework,Semantic Kernel,AutoGen,Retrieval Augmented Generation,Azure AI Search,Azure Key Vault,NVIDIA GB300 GPUs,Microsoft Fabric Graph Analytics,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-10-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-10-06', 'roundups', 'Weekly AI Roundup: Agent Framework, MCP, and Grok 4 on Foundry',
    'AI technology for developers saw new releases and expanded platform features, focusing on agentic models and enterprise automation. Microsoft Agent Framework now unifies orchestration experiences, integrating with Azure AI Foundry and Model Context Protocol (MCP). Grok 4 arrives on Azure AI Foundry with better reasoning and expanded context support. Updates center on improved automation, context retention, and data transformation, moving development teams from isolated models to modular, multi-agent workflows.
<!--excerpt_end-->
## Microsoft Agent Framework: Unified Agentic AI SDKs and Ecosystem Integrations
Microsoft Agent Framework is now in public preview, bringing a unified and open-source SDK for agentic AI to .NET and Python. This initiative, building on prior orchestration efforts like Semantic Kernel and AutoGen, simplifies multi-agent management. By supporting open protocols (MCP), developers gain modular context management, human-in-loop routing, thread-based state, and integrated Azure AI Foundry experiences.
Case studies such as automated audit workflows for KPMG Clara and voice-assisted services for Commerzbank show practical enterprise adoption. Community involvement in open-source development provides migration support from older agent SDKs. Observability via OpenTelemetry and security controls with Entra ID reinforce last week’s progress.
- [Introducing the Microsoft Agent Framework: Unified SDK for AI Agents and Workflows](https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-the-microsoft-agent-framework/ba/p/4458377)
- [Introducing Microsoft Agent Framework: The Open-Source Engine for Agentic AI Apps](https://devblogs.microsoft.com/foundry/introducing-microsoft-agent-framework-the-open-source-engine-for-agentic-ai-apps/)
- [Introducing Microsoft Agent Framework: Streamlining Multi-Agent AI Systems with Azure AI Foundry](https://azure.microsoft.com/en-us/blog/introducing-microsoft-agent-framework/)
- [Introducing Microsoft Agent Framework: Simplifying AI Agent Development for .NET Developers](https://devblogs.microsoft.com/dotnet/introducing-microsoft-agent-framework-preview/)
- [Agent Framework: Building Blocks for the Next Generation of AI Agents](/ai/videos/agent-framework-building-blocks-for-the-next-generation-of-ai-agents)
- [Microsoft Agent Framework Powers Multi-Agent Systems in Azure AI Foundry](https://www.linkedin.com/posts/satyanadella_introducing-microsoft-agent-framework-microsoft-activity-7379202146988318720-WDPf)
## Model Context Protocol (MCP) and Secure AI Integrations in Microsoft Fabric
Agentic AI open standards continue growing, with Fabric MCP’s API context streamlining developer experience across Microsoft Fabric. MCP’s standardization makes onboarding and automation quicker and safer, now reaching more data environments. The GitHub MCP Registry increases interoperability, and certified server discovery (including Figma, Postman, Terraform) demonstrates increased practical adoption.
Reviews of MCP’s effects show protocol-driven reduction of fragmentation and support for reusable workflows inside enterprise IDEs.
- [Introducing Fabric MCP (Preview): Developer-Focused AI Integration for Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/introducing-fabric-mcp-public-preview/)
- [How Model Context Protocol (MCP) Is Transforming AI-Driven Development Productivity](https://devops.com/how-model-context-protocol-mcp-is-fueling-the-next-era-of-developer-productivity/)
- [How to find the right MCP server in seconds with GitHub MCP registry](/ai/videos/how-to-find-the-right-mcp-server-in-seconds-with-github-mcp-registry)
## Grok 4: Advanced Reasoning Models in Azure AI Foundry
Grok 4 launches in Azure AI Foundry, expanding options after last week’s additions of new OpenAI and Anthropic models. Its multi-agent design and reinforcement learning take agentic automation beyond earlier Grok 3 updates.
Developers benefit from improved reasoning, larger context windows, and safer operations, while Foundry enables fast reasoning, summarization, and integrated code debugging for enterprise use—continuing a trend toward robust, compliant AI tools.
- [Grok 4 Now Available in Azure AI Foundry: Advanced Reasoning and Business-Ready AI](https://azure.microsoft.com/en-us/blog/grok-4-is-now-available-in-azure-ai-foundry-unlock-frontier-intelligence-and-business-ready-capabilities/)
## Copilot, Voice, and Intelligent Data Tools: Workflow Automation and NLP Integration
Copilot-powered automation now enables natural language workflow orchestration in Fabric Data Factory, continuing the momentum in analytics and user onboarding first seen last week.
Developers are adopting real-time voice-driven AI agents with Azure Voice Live API, applying conversational prompts and multimodal interactions for new process automation. Coverage of NLP tools explores documentation automation and references prior Copilot, VS Code, and JetBrains integrations.
- [AI-Powered Data Transformation and Insights with Copilot in Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/ai-powered-development-with-fabric-data-factory-ingest-transform-and-understand-your-data-with-copilot/)
- [Building a Real-Time Voice-Powered AI Sales Coach Using Azure Voice Live API](https://devblogs.microsoft.com/all-things-azure/from-lab-to-live-a-blueprint-for-a-voice-powered-ai-sales-coach/)
- [NLP Tools for Intelligent Documentation and Developer Enablement](https://devops.com/nlp-tools-for-intelligent-documentation-and-developer-enablement/)
## Practical Guides for Agentic AI in Python and Enterprise Workflows
This week’s guides share best practices for agentic AI in Python and enterprise systems. LangChain and CrewAI libraries address context retention challenges described in earlier tutorials.
Tutorials and documentation emphasize automation for diagrams and onboarding in CI/CD, supporting ongoing architecture compliance improvements. AIOps guidance merges traditional monitoring and anomaly detection with AI, following last week’s coverage of improved analytics integration.
- [Managing Context Retention in Agentic AI with Python and LangChain](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/managing-context-retention-in-agentic-ai/ba/p/4458586)
- [Documenting Architecture Using AI: From Painful Chore to Strategic Advantage](https://dellenny.com/documenting-architecture-using-ai-from-painful-chore-to-strategic-advantage/)
- [AIOps: Bringing Intelligence to IT Operations](https://dellenny.com/aiops-bringing-intelligence-to-it-operations/)
## AI Agents: Concepts, Architecture, and Developer Adoption
Discussions on agentic AI concepts and workflow adoption revisit last week’s conversations around code ownership and community trust. New guides and videos analyze the software development lifecycle in the context of agent use, with hands-on coverage explaining how Azure AI Foundry and agent architecture tools lower costs and facilitate deployment.
- [How AI Is Changing the SDLC: Trust, Ownership, and Community in Modern Software Development](https://www.arresteddevops.com/ai-sdlc/)
- [Why is everyone suddenly talking about AI agents?](/ai/videos/why-is-everyone-suddenly-talking-about-ai-agents)
- [Understanding AI Agents: Turning Plain Language into Code Execution](/ai/videos/understanding-ai-agents-turning-plain-language-into-code-execution)
- [What is an AI Agent?](/ai/videos/what-is-an-ai-agent)
## Retrieval Augmented Generation (RAG) and Document Processing Workflows
This week introduces a practical RAG workflow guide integrating OpenAI and Azure SQL based on last week’s enterprise data-to-chat tutorials. Logic Apps add features for metadata chunking, helping automate compliance in contract review and documentation workflows.
- [Azure SQL DB & OpenAI: Building Powerful RAG Applications](/ai/videos/azure-sql-db-and-openai-building-powerful-rag-applications)
- [Enhancing Logic Apps with Parse & Chunk with Metadata for AI-Powered Document Processing](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-parse-chunk-with-metadata-in-logic-apps-build-context/ba/p/4458438)
## Azure AI in Enterprise Workflows: Supply Chain & Scalable Cloud Infrastructure
Supply chain and forecasting solutions continue last week’s coverage of unified data workflows, showing business gains through resilient, AI-enhanced systems. Microsoft’s scalable AI infrastructure supports enterprise workloads for Copilot, ChatGPT, and other tools—underscoring reliability and developer productivity.
- [How Azure AI is Revolutionizing Supply Chain Forecasting and Inventory](https://dellenny.com/how-azure-ai-is-revolutionizing-supply-chain-forecasting-and-inventory/)
- [Microsoft''s Scalable AI Infrastructure for Copilot, ChatGPT, and Enterprise AI Workloads](https://www.linkedin.com/posts/satyanadella_our-approach-to-ai-infra-is-simple-build-activity-7379681735934083073-Scma)
## AI Ethics and Security in Scientific and Enterprise Environments
A Microsoft-led biosecurity report published in Science Magazine expands last week’s ethics coverage, focusing on safety measures and adversarial testing in generative models for biosciences. These conversations continue to emerge across scientific AI applications.
- [Microsoft-Led Study Unveils AI Protein Design Biosecurity Research in Science Magazine](https://www.linkedin.com/posts/satyanadella_strengthening-nucleic-acid-biosecurity-screening-activity-7379576230414753792-zB85)
## Other AI News
Visual Studio Code’s Bring Your Own Key (BYOK) support for model provider APIs follows last week’s workflow feature updates, enabling improved integration of third-party models in AI-driven coding.
Weekly Foundry Fridays AMA sessions grow community involvement, sharing technical best practices within Azure AI Foundry—videos are available for those seeking guidance on complex topics.
- [Enhancements to BYOK in Visual Studio Code: Model Provider Integration](/ai/videos/enhancements-to-byok-in-visual-studio-code-model-provider-integration)
- [Foundry Fridays: Weekly Azure AI Foundry AMA Series](https://techcommunity.microsoft.com/t5/microsoft-developer-community/foundry-fridays-your-front-row-seat-to-azure-ai-innovation/ba/p/4456956)',
    'AI technology for developers saw new releases and expanded platform features, focusing on agentic models and enterprise automation. Microsoft Agent Framework now unifies orchestration experiences, integrating with Azure AI Foundry and Model Context Protocol (MCP). Grok 4 arrives on Azure AI Foundry with better reasoning and expanded context support. Updates center on improved automation, context retention, and data transformation, moving development teams from isolated models to modular, multi-agent workflows.',
    1759734000, 'ai', '/ai/roundups/weekly-ai-roundup-2025-10-06', 'TechHub',
    'TechHub', '7449671BF3AE71E24F0C3090ED695018D07834FA4C29E343B3020E25C52087D5', ',AI Agents,Agentic AI,Microsoft Agent Framework,Azure AI Foundry,MCP,Microsoft Fabric,Copilot,Grok 4,LangChain,CrewAI,Retrieval Augmented Generation,Azure SQL,Logic Apps,OpenTelemetry,Entra ID,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-09-29
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-09-29', 'roundups', 'Weekly AI Roundup: Local Inference, Agents, and Multi-Model Tools',
    'AI technologies on Microsoft platforms continued to grow in hardware compatibility, agent reliability, model choice, and practical deployment, following the themes established in recent weeks. Guides and releases remain focused on bringing updated AI solutions into daily workflows, supporting best practices across both cloud and edge environments.
<!--excerpt_end-->
## Azure AI Foundry and Studio: Unified Generative AI Platform
Azure AI Studio (now Azure AI Foundry) establishes itself as a central workspace for developing generative AI and deploying LLM solutions, spanning model options including OpenAI, Meta, Mistral, and more. The platform supports prompt engineering, fine-tuning, retrieval-augmented generation (RAG), and offers both code-first and low-code interfaces. GPT-4o adds voice and multimodal features, and Phi-3 offers options for lightweight inference.
Security and governance improvements allow organizations to adopt responsible AI usage with integrated monitoring and compliance. Developers should remain aware of billing and vendor lock-in as they work with the platform.
Foundry Local v0.7 brings support for Intel/AMD NPUs on Windows 11 and simplifies local inference and AI runtime management. Installation with winget (Windows) and brew (Mac) reduces setup friction for multi-platform development.
Windows ML is now generally available, providing ONNX-based local inference for privacy and edge execution in Windows applications. Integrated with AMD Ryzen AI, Intel OpenVINO, NVIDIA TensorRT, and Snapdragon NPUs, Windows ML works closely with the App SDK and includes streamlined model conversion via the AI Toolkit for VS Code—highlighting edge AI’s readiness for production scenarios.
- [Azure AI Studio and AI Foundry: Microsoft’s Generative AI Platform Explained](https://dellenny.com/azure-ai-studio-azure-ai-foundry-a-powerful-platform-for-generative-ai/)
- [Foundry Local Meets More Silicon: Expanded AI Runtime and NPU Support](https://devblogs.microsoft.com/foundry/foundry-local-meets-more-silicon/)
- [Windows ML Now Generally Available: Empowering Developers to Deploy Local AI on Windows Devices](https://blogs.windows.com/windowsdeveloper/2025/09/23/windows-ml-is-generally-available-empowering-developers-to-scale-local-ai-across-windows-devices/)
## Secure and Reliable AI Agent Development with Azure and MCP
AI agent development is improving with integration methods for durable, reliable operations—building on previous agent orchestration and security content. Developers now have a step-by-step guide for using the OpenAI Agent SDK with Azure Durable Functions to support persistent state, retry logic, and distributed workflows, using decorators and orchestration functions to manage errors efficiently and reduce manual coding.
The final Agent Factory installment explains how to build a secure, standards-based agent ecosystem using the agentic web stack—covering identity, trust, and compliance via Entra ID alongside open protocols. Practical tips on integrating standards and secure orchestration are included, addressing both Microsoft and open-source tools.
- [Enhancing AI Agent Reliability with OpenAI Agent SDK and Azure Durable Functions](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/openai-agent-sdk-integration-with-azure-durable-functions/ba/p/4453402)
- [Agent Factory: Designing the Open Agentic Web Stack](https://azure.microsoft.com/en-us/blog/agent-factory-designing-the-open-agentic-web-stack/)
## Model Context Protocol (MCP) and Registry: Best Practices and Interoperability
Azure''s Model Context Protocol further embeds governance and security in AI workflows. Technical analysis highlights how MCP best practices in GitHub Copilot and VS Code enable automatic compliance and security enforcement, particularly for infrastructure-as-code scripts. Dynamic prompt instructions help teams maintain up-to-date policy compliance.
A video walkthrough introduces the GitHub MCP Registry, allowing developers to locate and connect MCP servers for agent development and modular design. Additional guidance outlines secure MCP server integration for Logic Apps and Copilot Studio, including authentication and deployment recommendations.
- [Teaching the LLM Good Habits: How Azure MCP Uses Best-Practice Tools](https://devblogs.microsoft.com/all-things-azure/teaching-the-llm-good-habits-how-azure-mcp-uses-best-practice-tools/)
- [A Deep Dive into the GitHub MCP Registry for AI Agents](/ai/videos/a-deep-dive-into-the-github-mcp-registry-for-ai-agents)
- [Connecting Azure Logic Apps MCP Server to Copilot Studio Securely](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/calling-logic-apps-mcp-server-from-copilot-studio/ba/p/4456277)
## Microsoft Copilot Studio and Model Selection
Copilot Studio adds model selection for Anthropic''s Claude Sonnet 4 and Opus 4.1, alongside OpenAI''s GPT models, enabling prompt- and logic-level model configuration. Admin options in Microsoft 365 and Power Platform allow for domain-specific assignments and automated fallback rules—providing more control for organizations pursuing consistent automation.
- [Anthropic Models Integrated with OpenAI in Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/anthropic-joins-the-multi-model-lineup-in-microsoft-copilot-studio/)
## Microsoft Fabric: LLM Analytics, Real-Time AI, and Workflow Automation
Fabric Data Agent now supports mirrored cloud databases, allowing natural language queries and multimodal analytics using Delta Parquet mirrors. Previewed anomaly detection in RTI streamlines streaming analytics with integration into Teams and email alerts. Agent Mart Studio’s expanded connections with Fabric and OneLake enhance low- and no-code workflow automation for data professionals and developers.
- [Unlocking LLM-Powered Analytics with Fabric Data Agent and Mirrored Databases](https://blog.fabric.microsoft.com/en-US/blog/unlocking-llm-powered-through-data-agent-from-your-mirrored-databases-in-microsoft-fabric/)
- [AI–Powered Real-Time Intelligence with Anomaly Detection in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/ai-powered-real-time-intelligence-with-anomaly-detection-preview/)
- [Building AI Agents for Enterprise Data with Agent Mart Studio and Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/27082/)
## .NET and Multimodal AI: Text-to-Image Capabilities
MEAI adds text-to-image generation in .NET, providing a consistent API that abstracts providers like Azure AI Foundry, OpenAI, and ONNX. This update prepares for future image-to-image and image-to-video support, making multimodal AI more accessible for .NET applications.
- [Exploring Text-to-Image Capabilities in .NET with Microsoft Extensions for AI](https://devblogs.microsoft.com/dotnet/explore-text-to-image-dotnet/)
## SharePoint and Microsoft 365: AI-Driven Content Intelligence
SharePoint''s Knowledge Agent (public preview) delivers AI-powered automation for content metadata, summaries, document comparison, and rule creation, with workflow integration into Copilot. Controlled pilot programs, governance, review cycles, and training are emphasized for effective enterprise use.
- [Introducing Knowledge Agent in SharePoint (Public Preview)](https://dellenny.com/introducing-knowledge-agent-in-sharepoint-public-preview/)
## Building and Operationalizing AI-Powered Agents
Developers continue to build practical agents, with a tutorial on creating a resilience coach using Azure OpenAI and Python. Additional resources show agent memory management with Semantic Kernel and Azure AI Search, alongside customization guides for LLMs and Cognitive Services. An operational workflow demonstrates post-call analytics using Azure OpenAI to process transcripts and feed CRM systems.
- [Building a Resilience Coach with AI on Cozy AI Kitchen](/ai/videos/building-a-resilience-coach-with-ai-on-cozy-ai-kitchen)
- [AI Agent Memory: Building Self-Improving Agents](/ai/videos/ai-agent-memory-building-self-improving-agents)
- [Generative AI in Azure: A Practical Guide to Getting Started](https://dellenny.com/generative-ai-in-azure-a-practical-guide-to-getting-started/)
- [From Call Transcripts to CRM Gold: AI-Powered Post-Call Intelligence](https://techcommunity.microsoft.com/t5/azure-communication-services/from-call-transcripts-to-crm-gold-ai-powered-post-call/ba/p/4456337)
## AI for Social Impact and Enterprise Architecture
UNHCR, Microsoft, and GitHub share new uses of drone data and open-source AI for sustainable planning in refugee settlements, showcasing adaptive open tools. Updated architecture frameworks now account for AI requirements, MLOps, and explainability. Sustainability remains a priority, with AI solutions for digital twins, forecasting, and energy-use reduction continuing the focus on practical environmental reliability.
- [Using AI and Open Source to Map Refugee Settlements: The UNHCR and GitHub Story](https://github.blog/open-source/social-impact/using-ai-to-map-hope-for-refugees-with-unhcr-the-un-refugee-agency/)
- [Software Architecture Frameworks and Artificial Intelligence: Building Smarter Systems](https://dellenny.com/software-architecture-frameworks-and-artificial-intelligence-building-smarter-systems/)
- [Accelerating Sustainability and Resilience with AI-Powered Innovation](https://www.microsoft.com/en-us/microsoft-cloud/blog/2025/09/23/accelerating-sustainability-and-resilience-with-ai-powered-innovation/)
## Other AI News
Research teams at Microsoft, Drexel University, and the Broad Institute present generative AI for rare disease diagnosis, utilizing Azure AI Foundry for evidence aggregation and collaborative genome review—a continuation of last week’s healthcare AI initiatives.
- [Using AI to Assist in Rare Disease Diagnosis: Insights from Microsoft Research](https://www.microsoft.com/en-us/research/blog/using-ai-to-assist-in-rare-disease-diagnosis/)',
    'AI technologies on Microsoft platforms continued to grow in hardware compatibility, agent reliability, model choice, and practical deployment, following the themes established in recent weeks. Guides and releases remain focused on bringing updated AI solutions into daily workflows, supporting best practices across both cloud and edge environments.',
    1759129200, 'ai', '/ai/roundups/weekly-ai-roundup-2025-09-29', 'TechHub',
    'TechHub', 'DA618BDE39C36939C68DA3F6AB5913DAFFCC87BD971D5ACBB7B46008D2622DC8', ',Azure AI Foundry,Azure AI Studio,Foundry Local,Windows ML,ONNX,NPU,Azure Durable Functions,OpenAI Agent SDK,MCP,GitHub MCP Registry,Copilot Studio,Anthropic Claude,Microsoft Fabric,Azure AI Search,Semantic Kernel,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-09-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-09-22', 'roundups', 'Weekly AI Roundup: Enterprise coding, agents, MCP, and security',
    'Recent AI updates highlight broad integration of advanced language models, maturing agent frameworks, improved developer tools, and practical guides for secure, automated workflows. OpenAI’s GPT-5-Codex is enterprise-ready, while Microsoft and its partners continue to expand Azure AI Foundry agent capabilities, security features, and tooling. MCP support grows, with more resources for multi-agent and scalable autonomous operations across industries.
<!--excerpt_end-->
## GPT-5-Codex and Enterprise AI Development
GPT-5-Codex is now available for enterprise software engineering beyond basic code generation, tackling deeper refactoring, debugging, and code review at scale. It allocates resources depending on workflow complexity, providing context-aware review, dependency checks, test management, and security feedback.
Codex works with VS Code, Cursor, CLI tools, APIs, and browser automation, retaining context across on-premises and cloud environments. It automates dependency checks and improves performance using container caching. Security measures include sandboxing, audit logs, custom controls, and protection against prompt injection.
Real-world cases, such as Cisco Meraki’s modernization, show Codex reducing manual review tasks and helping teams refocus on strategic work. These recent capabilities complement ongoing BYOK and model selection in Copilot, contributing to the wider adoption of context-driven coding assistants.
- [OpenAI’s GPT-5-Codex: Enterprise AI for Smarter Software Development](https://devops.com/openais-gpt-5-codex-a-smarter-approach-to-enterprise-development/)
- [OpenAI’s GPT-5-Codex: AI for Enterprise-Grade Development and Code Review](https://devops.com/openais-gpt-5-codex-a-smarter-approach-to-enterprise-development/?utm_source=rss&utm_medium=rss&utm_campaign=openais-gpt-5-codex-a-smarter-approach-to-enterprise-development)
## Azure AI Foundry: Agents, Orchestration, and Security
Azure AI Foundry has introduced the Computer Use Tool (preview) for agents to automate desktop and web interfaces using REST APIs or SDKs—even where native APIs are missing. It supports pixel-based reasoning, guardrails involving human review, risk monitoring, and sandboxed deployments.
Security updates feature Entra Agent IDs for lifecycle management, Purview-provided DLP, prompt injection defense, adversarial testing, and agent telemetry linked to Defender XDR for live incident monitoring.
A new engineering guide covers the design of multi-agent systems, integration of dynamic MCP tools, prompt best practices, RBAC, and approaches to scale. Azure AI Foundry now offers a complete platform for compliant agent development.
These updates build on previous Agent Factory and MCP standards coverage, showing the transition from reference architectures to preview features and governance models.
- [Announcing Computer Use Tool (Preview) in Azure AI Foundry Agent Service](https://devblogs.microsoft.com/foundry/announcing-computer-use-tool-preview-in-azure-ai-foundry-agent-service/)
- [Agent Factory: Blueprint for Safe and Secure Enterprise AI Agents Using Azure AI Foundry](https://azure.microsoft.com/en-us/blog/agent-factory-creating-a-blueprint-for-safe-and-secure-ai-agents/)
- [Building Multi-Agent AI Systems with Azure AI Foundry: Engineering, Orchestration, and Best Practices](https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-multi-agent-ai-systems-with-microsoft/ba/p/4454510)
## Model Context Protocol (MCP) in Microsoft’s AI Stack
Microsoft’s wide MCP standard adoption supports tool consistency and system integration. MCP offers a schema for agents, tools, and memory with support for HTTP, SSE, and WebSocket protocols. Developers benefit from cross-platform usage in Copilot Studio, Azure AI Foundry, and Dynamics 365, with SDKs for C# and Semantic Kernel integration.
Guides include MCP server setup, Dynamics 365/Dataverse deployments, and practical agent integration for regulated business environments, underlining Microsoft’s commitment to open frameworks and reusable tooling.
Recent MCP guides expand on earlier agent factory progress, providing more practical resources for interoperable workflows.
- [How MCP Works in Microsoft’s AI Ecosystem](https://dellenny.com/how-mcp-works-in-microsofts-ai-ecosystem/)
- [Unlocking MCP Server: AI Integration for Dataverse & Dynamics 365](/ai/videos/unlocking-mcp-server-ai-integration-for-dataverse-and-dynamics-365)
## Azure Agentic AI Solution Architecture and Best Practices
Developers receive updated migration advice for shifting from Azure OpenAI Assistants API (now deprecated) to Azure AI Agent Service, connecting to Azure AI Search, Fabric, containers, and SDKs for Python, C#, and Java. No-code automation with Logic Apps is featured for human-in-the-loop processes.
The resource includes open-source orchestrators, hosting choices for AKS/App Service, and security tips for agent orchestration deployment.
This extends last week’s focus on Logic Apps and Python/MCP agent previews, showing Azure’s movement to unified migration and orchestration strategies.
- [Selecting the Right Agentic Solution on Azure: A Guide to AI Agents and Orchestration](https://techcommunity.microsoft.com/t5/azure-architecture-blog/selecting-the-right-agentic-solution-on-azure/ba/p/4453955)
## Agentic AI for Platform Engineering and Infrastructure
Pulumi Neo now previews autonomous AI agents for Infrastructure-as-Code, managing diagnostics, compliance, policy automation, approvals, and audit logs, with MCP support for multi-tool workflows and recommendation context. Teams can reverse unsafe changes with improved traceability.
This continues the evolution from developer tools to advanced infrastructure automation, bridging AI agents, platform engineering, and DevOps.
- [Pulumi Unveils AI Agents for Autonomous Infrastructure Automation](https://devops.com/pulumi-previews-ai-agents-trained-to-automate-infrastructure-management/)
## Microsoft Fabric: Real-Time Intelligence and Developer Resources
Microsoft Fabric extends AI-driven event analytics and dashboarding with streaming tools (Eventstream/Eventhouse), reusable KQL queries, geospatial and graph analytics, Copilot NLP, Activator automation, and Digital Twin Builder. Additional monitoring, security, and connector features widen industry applicability.
There’s an announcement for a global AI/data hackathon including workshops and team challenges.
These updates continue last week’s event-driven agentic enhancements and integrations across pro-code and low-code environments.
- [AI-Driven Operations with Microsoft Fabric Real-Time Intelligence](https://blog.fabric.microsoft.com/en-US/blog/the-foundation-for-powering-ai-driven-operations-fabric-real-time-intelligence/)
- [Hack the Future of Data with Microsoft Fabric: Global AI & Data Hackathon](https://blog.fabric.microsoft.com/en-US/blog/announcement-hack-the-future-of-data-with-microsoft-fabric/)
## Industry-Specific AI Workflows & Communication Automation
A four-tier framework shows automation setups using Azure Communication Services and Copilot Studio for domains like healthcare, finance, recruiting, and retail. Technical guides share step-by-step instructions for HIPAA notifications, financial services onboarding, and omnichannel bots with secure integration.
A case study describes Copilot Studio bots reducing support tickets by 40% and increasing CSAT by 25%, with advice on flow design, prompt engineering, and API connections.
Building on last week’s templates and best practices, these resources offer direct methods for scaling communication automation.
- [How AI and Communication APIs Are Transforming Industry Workflows](https://techcommunity.microsoft.com/t5/azure-communication-services/how-ai-communication-apis-are-transforming-work-across/ba/p/4454224)
- [Case Study: Reducing Support Ticket Volume Using AI Bots Built in Copilot Studio](https://dellenny.com/case-study-reducing-support-ticket-volume-using-ai-bots-built-in-copilot-studio/)
## Microsoft Copilot Studio and Foundry Local Expansions
Copilot Studio’s computer use feature enters US public preview, enabling desktop and web automation even for systems lacking APIs. A hosted browser, templates, credential tools, and controlled allow-listing extend flexibility. Power Automate integration supports no-code scripting using natural language and UI interaction.
Upcoming technical AMAs cover Foundry Local’s on-device LLM customization and offline inference, supporting privacy and hybrid deployment.
These updates build on last week’s guides, expanding tool diversity for developers working with low-code and privacy-preserving workflows.
- [Computer Use Public Preview Launches in Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/computer-use-is-now-in-public-preview-in-microsoft-copilot-studio/)
- [Technical AMA: Foundry Local and On-Device LLMs with Azure AI Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/join-us-for-a-technical-deep-dive-and-q-a-on-foundry-local-llms/ba/p/4455060)
## Advances in AI Search, Indexing, and Knowledge Grounding
Developers can now create vector indexes from Azure storage in Azure AI Foundry, using Azure AI Search for both keyword and vector queries, RBAC, and network isolation. This supports faster Retrieval Augmented Generation (RAG) solution prototyping and agent deployment.
The Azure Essentials Show outlines improvements to RAG apps with better access control and quick deployment.
This builds on last week’s progress in agent-centric data integration, improving productivity.
- [Ground Your Agents Faster with Native Azure AI Search Indexing in Foundry](https://devblogs.microsoft.com/foundry/ground-your-agents-faster-native-azure-ai-search-indexing-foundry/)
- [Build Smarter Agents with Azure AI Search](/ai/videos/build-smarter-agents-with-azure-ai-search)
## Multi-Agent AI Solutions and Collaborative Microsoft Workflows
Guides show how Microsoft Fabric Data Agent, Azure AI Foundry, and Copilot Studio combine for full multi-agent solutions, supporting data synthesis, Lakehouse configuration, agent-centric workflow, and conversational setups in Teams. Documentation and workshops promote these collaborative patterns.
This continues last week’s practical tutorials for multi-agent deployment.
- [Building Multi-Agent Solutions with Microsoft Fabric Data Agent and Azure AI Foundry](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/explore-microsoft-fabric-data-agent-azure-ai-foundry-for-agentic/ba/p/4453709)
## Other AI News
Recent analysis covers general AI agents and how hallucinations emerge. “Sip and Sync with Azure” offers demonstrations of workflows, Warp CLI, and external protocol advice for agent-driven solution development.
A GitHub video explains AI model hallucinations, focusing on training data coverage and incentives, and offers suggestions for more reliable output—especially useful for LLM API developers and chatbot authors.
These resources complement last week’s coverage on developer education and practical risk management.
- [Building for General Purpose AI Agents | Sip and Sync with Azure](/ai/videos/building-for-general-purpose-ai-agents-sip-and-sync-with-azure)
- [The Real Reason AI Models Hallucinate](/ai/videos/the-real-reason-ai-models-hallucinate)',
    'Recent AI updates highlight broad integration of advanced language models, maturing agent frameworks, improved developer tools, and practical guides for secure, automated workflows. OpenAI’s GPT-5-Codex is enterprise-ready, while Microsoft and its partners continue to expand Azure AI Foundry agent capabilities, security features, and tooling. MCP support grows, with more resources for multi-agent and scalable autonomous operations across industries.',
    1758524400, 'ai', '/ai/roundups/weekly-ai-roundup-2025-09-22', 'TechHub',
    'TechHub', '998DF6046A95C723E491FE2CB81A7A580F6C43E0F4B1D66D197AD9FD62B207AE', ',GPT 5 Codex,OpenAI,Azure AI Foundry,AI Agents,Multi Agent Systems,MCP,Copilot Studio,Computer Use,Agent Orchestration,Agent Security,Prompt Injection Defense,Microsoft Entra,Azure AI Search,RAG,Microsoft Fabric,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-09-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-09-15', 'roundups', 'Weekly AI Roundup: Open agent standards, hybrid build flows',
    'Recent updates in AI center around hardware research, adoption of open standards, expanded low/no-code automation, and new developer integrations. Microsoft’s latest work enables more adaptable agent systems, hybrid developer experiences, and improved governance for enterprise AI. New research tackles infrastructure efficiency and scalable model deployments.
<!--excerpt_end-->
## AI Infrastructure: MOSAIC MicroLED Architecture
Microsoft Research introduced MOSAIC, an optical networking approach for data centers using bundles of slow microLED channels instead of a few fast ones. This increases throughput, reduces power use, and improves reliability—grouping thousands of channels can reach up to 3.2 Tbps while lowering costs and complexity for AI infrastructure. MicroLEDs also resist temperature problems and offer lower failure rates than lasers. MOSAIC is compatible with standard protocols, simplifying integration. Full details are available in technical papers.
- [MOSAIC: A Wide-and-Slow MicroLED Network Architecture for Next-Gen AI Infrastructure](https://www.microsoft.com/en-us/research/blog/breaking-the-networking-wall-in-ai-infrastructure/)
## Agent Interoperability and Azure AI Foundry
Azure AI Foundry added more open standards, enabling agents, apps, and enterprise data to interoperate via Model Context Protocol (MCP) and Agent2Agent (A2A). MCP lets agents share workflow context, while A2A models collaborative agent activity. Foundry supports thousands of connectors, unified monitoring, and comprehensive app integration—making multi-agent workflows possible across platforms and ensuring secure data access and compliance.
Continuing last week’s movement toward open standards and orchestration, this update shows how Foundry leverages MCP and A2A for flexible agent systems.
- [Agent Factory: Connecting Agents, Apps, and Data with Open Standards (MCP, A2A)](https://azure.microsoft.com/en-us/blog/agent-factory-connecting-agents-apps-and-data-with-new-open-standards-like-mcp-and-a2a/)
## Copilot Studio and Azure AI Foundry Integration
New resources explain how Copilot Studio’s low-code conversational agent builder connects with Azure AI Foundry, supporting custom model workflows and managed deployment. The workflow moves from quick agent creation to enterprise-grade deployment, bridging low-code tools and professional lifecycle management. Interviews and analysis highlight both current and planned integrations for a more unified Microsoft AI developer experience.
These updates continue coverage of orchestration and automation, now connecting low-code agent design to pro deployment.
- [Exploring the Connection Between Copilot Studio and Azure AI Foundry](/ai/videos/exploring-the-connection-between-copilot-studio-and-azure-ai-foundry)
- [Understanding the Connection Between Copilot Studio and Azure AI Foundry](/ai/videos/understanding-the-connection-between-copilot-studio-and-azure-ai-foundry)
## Copilot Studio: Document Automation and Hybrid Development
Copilot Studio demonstrated automation with no-code agents integrated into Power Platform and AI Builder, streamlining document-heavy tasks like vehicle permit processing. Resources cover setup, configuration, and monitoring, showing live metrics and error reduction. Additional analysis shows Copilot Studio connecting low-code and pro-code workflows—making it easier for both non-technical and experienced developers to collaborate and scale solutions. Teams can prototype quickly and migrate to professional-grade solutions as needed.
Continuing last week’s coverage of hybrid workflow automation, this set of articles explores Copilot Studio''s progress in team document workflows and solution scaling.
- [Build AI Agents for Fast, High-Volume Document Automation in Copilot Studio](/ai/videos/build-ai-agents-for-fast-high-volume-document-automation-in-copilot-studio)
- [Low-Code vs Pro-Code: How Copilot Studio Bridges the Gap](https://dellenny.com/low-code-vs-pro-code-how-copilot-studio-bridges-the-gap/)
## Language Model Updates and Migration
Microsoft has deprecated Phi-3 and Phi-3.5 models in GitHub Models, recommending that users migrate to Phi-4 and Phi-4-mini-instruct. Migration instructions and mapping are provided for updating workflows. Teams are encouraged to track changes for stability and current support.
This builds on last week’s efforts to keep teams informed about model life cycles and minimize disruption in workflows.
- [Microsoft Phi-3 Model Deprecation and Transition in GitHub Models](https://github.blog/changelog/2025-09-11-deprecated-microsoft-models-in-github-models)
## Developer Tooling for Model Integration and Workflow
Visual Studio Code introduces the Language Model Chat Provider API (BYOK), letting extension developers integrate models from any provider for privacy and policy flexibility. Technical resources detail API usage and implementation. Warp has added embedded AI agents to its CLI, offering prompt-driven scripting, code review, and editing for terminal-focused workflows.
These tools support easier daily AI integration in both IDE and CLI environments, following the trend of improving developer access to AI capabilities.
- [Extending VS Code with the Language Model Chat Provider (BYOK) API: Insights from Logan Ramos](/ai/videos/extending-vs-code-with-the-language-model-chat-provider-byok-api-insights-from-logan-ramos)
- [Warp Integrates AI Coding Agents into CLI for Enhanced Developer Feedback](https://devops.com/warp-embeds-ai-agents-into-a-cli-to-provide-better-feedback-loop/?utm_source=rss&utm_medium=rss&utm_campaign=warp-embeds-ai-agents-into-a-cli-to-provide-better-feedback-loop)
## Azure AI Foundry Translation and No-Code AI Workflows
Azure AI Foundry''s Translator API is now in public preview, enabling developers to add both neural and LLM translation features to apps with step-by-step multilingual integration. The Cozy AI Kitchen show explored CalcLM—a no-code option for bringing GPT-4.1 to spreadsheets via Azure OpenAI, allowing analysis and planning through natural language queries. Demos demonstrate agent customization and further integration possibilities.
These resources extend AI accessibility across technical and business user scenarios, complementing recent efforts to make AI more usable for non-developers.
- [Translating Conversations with Azure AI Foundry Translator API](/ai/videos/translating-conversations-with-azure-ai-foundry-translator-api)
- [Blending AI Agents and Spreadsheets: No-Code Solutions in the Cozy AI Kitchen](/ai/videos/blending-ai-agents-and-spreadsheets-no-code-solutions-in-the-cozy-ai-kitchen)
## Avatar-Powered Education and AI Content Creation
This guide covers building avatar-powered educational solutions with Azure, incorporating neural text-to-speech, avatars, and secure CI/CD, storage, and identity management. Developers receive templates and instructions for automating onboarding and training resources with compliance controls.
Supporting last week’s coverage on AI accessibility, Azure continues to offer tools for innovative and inclusive learning solutions.
- [Revolutionizing Learning with Immersive AI: Avatar-Powered Education on Azure](https://techcommunity.microsoft.com/t5/azure-architecture-blog/revolutionizing-learning-with-immersive-ai/ba-p/4453680)
## AI Coding Platforms for Enterprise Apps
Empromptu launched "vibecoding" for explainable, compliant enterprise AI apps. The platform uses retrieval-augmented generation, LLMOps tooling, and output scoring—plus DevOps integration with SOC 2 governance and credit-based pricing.
This matches last week''s interest in enterprise-ready AI, highlighting the benefit of transparency and production monitoring.
- [Empromptu Launches Vibecoding AI Coding Platform for Enterprise Apps](https://devops.com/empromptu-unveils-vibecoding-platform-for-building-enterprise-class-apps/?utm_source=rss&utm_medium=rss&utm_campaign=empromptu-unveils-vibecoding-platform-for-building-enterprise-class-apps)',
    'Recent updates in AI center around hardware research, adoption of open standards, expanded low/no-code automation, and new developer integrations. Microsoft’s latest work enables more adaptable agent systems, hybrid developer experiences, and improved governance for enterprise AI. New research tackles infrastructure efficiency and scalable model deployments.',
    1757919600, 'ai', '/ai/roundups/weekly-ai-roundup-2025-09-15', 'TechHub',
    'TechHub', '118AD7AA915853F476C8FA4C688511E5DF489CF47C75F7C6805DE32257F53630', ',Azure AI Foundry,Copilot Studio,AI Agents,MCP,Agent2Agent,Multi Agent Workflows,AI Governance,Enterprise AI,MicroLED Optical Networking,Data Center Networking,VS Code Extensions,Language Model Chat Provider API,Bring Your Own Model,GitHub Models,Phi 4,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-09-08
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-09-08', 'roundups', 'Weekly AI Roundup: Foundry Agents, Copilot Automation, Safer Deploys',
    'The AI section this week includes updates in Azure AI Foundry, enterprise automation with Copilot Studio, new tutorials, deployment security guides, and reflections on the challenges of AI coding tools. Azure AI Foundry released additional multimodal models and orchestration features, while Copilot Studio further enabled agent-to-agent automation for business use. Tutorials addressed context management and secure deployment, continuing to stress cost and accessibility.
<!--excerpt_end-->
## Azure AI Foundry and Agentic Workflows
Azure AI Foundry remains a mainstay for enterprise AI agent development. Its August 2025 release brings in GPT-5, extended context windows, advanced multimodality, and orchestration utilities. The Model Router helps pick the best GPT-5 version, the Sora API increases image-to-video features, and Mistral Document AI brings improved document layout recognition. The update also includes better SDKs, Terraform integration, and OpenTelemetry support for observability. Browser Automation preview supports RPA scenarios, combining natural language control with Playwright. The Agent Service is now available in 17 regions, with revamped onboarding and recovery documentation.
Building on previous coverage of multi-agent orchestration and RAG workflows, Foundry now moves several features to general availability. Open standards such as MCP/A2A facilitate migration and interoperability. Technical guides show how Foundry integrates with developer tools, allowing fast transitions from prototyping to production. Tutorials guide developers through setting up persistent-memory agents, orchestrating multi-agent scenarios, and ramping up quickly, while Q&A materials share strategies for robust design and troubleshooting.
- [What’s New in Azure AI Foundry: August 2025 Release Highlights](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-august-2025/)
- [Agent Factory: From Prototype to Production—Developer Tools and Rapid Agent Development](https://azure.microsoft.com/en-us/blog/agent-factory-from-prototype-to-production-developer-tools-and-rapid-agent-development/)
- [Build a Smart Shopping AI Agent with Memory Using Azure AI Foundry Agent Service](https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-a-smart-shopping-ai-agent-with-memory-using-the-azure-ai/ba-p/4450348)
- [Build Multi-Agent AI Systems on Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-multi-agent-ai-systems-on-azure-app-service/ba-p/4451373)
- [Context Window: 3 Azure AI Foundry Community Questions Answered](/ai/videos/context-window-3-azure-ai-foundry-community-questions-answered)
## Microsoft Copilot Studio: Workflow Automation and Generative Logic
Copilot Studio now supports agent-to-agent collaboration for modular HR and IT onboarding. Developers can build workflows where multiple agents manage different segments, coordinated using custom Canvas apps. New preview features support maintainable and extendable automation.
Building on enterprise automation and orchestration topics from last week, Copilot Studio now securely connects business logic with generative AI. Its architecture separates intent processing from compliance enforcement by using plugins, role-based access, and data loss prevention. Walkthroughs include CRM, ERP, and retail use cases, illustrating practical automation and strategies for scaling.
- [Agent-to-Agent Collaboration in Copilot Studio](/ai/videos/agent-to-agent-collaboration-in-copilot-studio)
- [Combining Generative AI and Business Logic with Copilot Studio](https://dellenny.com/combining-generative-ai-and-business-logic-with-copilot-studio/)
- [Automating Retail Customer Service with Copilot Studio](https://dellenny.com/how-retail-businesses-are-automating-customer-service-with-copilot-studio/)
## Azure AI OpenAI, Model Context Protocol, and Streamlit Deployments
This week’s tutorials describe deploying Azure OpenAI models securely using the Model Context Protocol (MCP), emphasizing transparent context management. Developers are shown how to build an image captioning system: users upload images via Streamlit, Azure AI Vision creates tags, and GPT-4o-mini writes captions. The workflow is hosted on Azure App Service and employs managed identities, azd, and Bicep.
These samples follow last week''s themes of MCP-based communication, persistent context, and serverless agent workflows. The current guides add hands-on code and step-by-step onboarding instructions.
A separate resource demonstrates deploying a Microsoft Docs AI assistant using RAG pipelines, MCP, Azure Container Apps, and OpenAI. The article covers containerization, environment configuration, and scaling—preparing teams for onboarding and future AI customizations, with a focus on modularity and security.
- [Build an AI Image-Caption Generator on Azure App Service with Streamlit and GPT-4o-mini](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-an-ai-image-caption-generator-on-azure-app-service-with/ba-p/4450313)
- [Deploying a Self-Hosted Microsoft Docs AI Assistant with Azure OpenAI and MCP](https://devblogs.microsoft.com/all-things-azure/build-your-own-microsoft-docs-ai-assistant-with-azure-container-apps-and-azure-openai/)
## AI Agent Design: Context Engineering and Developer Education
Several resources cover context engineering for AI agents, offering practical advice for finding and using relevant data—improving adaptability and reliability. The content features code samples, scaling tips, and fallback planning for building more robust agents.
Tying in with previous stories on GraphRAG and best practices, these materials highlight developer education. Tutorials, livestreams, and community events offer a variety of ways to apply concepts like MCP and agent lifecycle management.
An October livestream series (in Spanish) guides Python developers through generative AI, agent architectures, MCP workflows, and demos. Sessions include Q&A and access to a Discord community.
- [Context Engineering for AI Agents](/ai/videos/context-engineering-for-ai-agents)
- [Level Up Your Python Game with Generative AI: Free Livestream Series](https://techcommunity.microsoft.com/t5/microsoft-developer-community/level-up-your-python-game-with-generative-ai-free-livestream/ba-p/4450646)
## The Reality of AI-Augmented Coding
Companies find that LLM-based coding tools bring benefits but also introduce new costs, review needs, and security issues. Sonar’s report on ChatGPT-5 shows improvements in reasoning and code quality, but notes higher subscription costs and codebase demands. While some vulnerabilities decrease, concurrency issues become more common, highlighting the importance of thorough QA.
Following previous themes on cost and risk management, this section discusses budget implications of generative coding and the importance of active monitoring and quality assurance.
The concept of "vibe coding" (using LLMs for intent-driven development) is discussed with focus on enterprise risk. While productivity gains are possible, rigorous oversight, checks, and compliance are required to avoid leaks or errors in quickly built code.
- [Report: ChatGPT-5 Coding Gains Come at a Higher Cost](https://devops.com/report-chatgpt-5-coding-gains-come-at-a-higher-cost/?utm_source=rss&utm_medium=rss&utm_campaign=report-chatgpt-5-coding-gains-come-at-a-higher-cost)
- [Can Vibe Coding Work on an Enterprise Level?](https://devops.com/can-vibe-codingwork-on-an-enterprise-level/?utm_source=rss&utm_medium=rss&utm_campaign=can-vibe-codingwork-on-an-enterprise-level)
## Applied AI: Accessibility and Edge Workflows
The Argus Panoptes project, a Microsoft Imagine Cup winner, demonstrates how Azure AI and Wi-R wireless protocols are enabling modern accessibility devices. The system balances workloads between local devices and Azure Foundry, delivering reliable object recognition and leveraging Azure AI Speech for voice interaction.
This example continues last week''s focus on mainstream and startup innovation in AI infrastructure. It shows how Azure supports both enterprise uses and strategy for accessible technology.
Azure also fosters startup projects and innovations with an emphasis on accessibility.
- [Argus and Azure AI: Inclusive Assistive Tech Triumphs at Imagine Cup](https://www.microsoft.com/en-us/startups/blog/could-your-startup-be-the-next-imagine-cup-world-champion/)',
    'The AI section this week includes updates in Azure AI Foundry, enterprise automation with Copilot Studio, new tutorials, deployment security guides, and reflections on the challenges of AI coding tools. Azure AI Foundry released additional multimodal models and orchestration features, while Copilot Studio further enabled agent-to-agent automation for business use. Tutorials addressed context management and secure deployment, continuing to stress cost and accessibility.',
    1757314800, 'ai', '/ai/roundups/weekly-ai-roundup-2025-09-08', 'TechHub',
    'TechHub', 'E4038660466599016C6BB6AC2EEBF27DFE7EA9C1049DEB1262E8C0A74E8BDC48', ',Azure AI Foundry,Azure OpenAI,GPT 5,Multimodal Models,Agentic Workflows,Multi Agent Orchestration,MCP,Copilot Studio,Agent To Agent,Retrieval Augmented Generation,Azure App Service,Azure Container Apps,Terraform,OpenTelemetry,Streamlit,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-09-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-09-01', 'roundups', 'Weekly AI Roundup: Agents, RAG Upgrades, and Safer Workflows',
    'Recent AI developments emphasize the evolution of multi-agent frameworks, improved retrieval workflows, enhanced security, and better cost controls, particularly within Azure and the open source community. The updates include new APIs, orchestration models, guides for enterprise adoption, and real-world experiences dealing with shadow AI and developer upskilling.
<!--excerpt_end-->
## Azure AI Foundry: Multi-Agent Orchestration, RAG, and API Developments
Azure AI Foundry released upgraded tools for orchestrating multi-agent systems, building on its modular agent support. Enhanced retrieval, analytics, and policy integrations connect with previous guidance for real-world production deployments. New RAG walkthroughs and the public release of the Responses API help streamline agent orchestration, making large-scale deployments more approachable and integrating with platforms like Semantic Kernel and AutoGen. Freeform tool calling with GPT-5 enables flexible automation for generating developer artifacts.
- [Multi-Agent Orchestration with Azure AI Foundry: From Idea to Production](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/multi-agent-orchestration-with-azure-ai-foundry-from-idea-to/ba-p/4449925)
- [Retrieval-Augmented Generation (RAG) in Azure AI: A Step-by-Step Guide](https://dellenny.com/retrieval-augmented-generation-rag-in-azure-ai-a-step-by-step-guide/)
- [General Availability of the Responses API in Azure AI Foundry](https://techcommunity.microsoft.com/t5/ai-azure-ai-services-blog/the-responses-api-in-azure-ai-foundry-is-now-generally-available/ba-p/4446567)
- [Freeform Tool Calling with GPT-5 in Azure AI Foundry](/ai/videos/freeform-tool-calling-with-gpt-5-in-azure-ai-foundry)
## Semantic Kernel: Security, Template Updates, and Azure Integration Changes
Semantic Kernel Python 1.36.0 now requires explicit credential configuration for Azure authentication—a shift to stronger credential management for compliance and reliability. New encoding rules for template arguments bring added runtime protection, strengthening prompt engineering security and defending against injection risks.
- [Mandatory Explicit Azure Authentication in Semantic Kernel Python 1.36.0](https://devblogs.microsoft.com/semantic-kernel/azure-authentication-changes-in-semantic-kernel-python/)
- [Stricter Encoding Rules for Template Arguments in Semantic Kernel](https://devblogs.microsoft.com/semantic-kernel/encoding-changes-for-template-arguments-in-semantic-kernel/)
## Agentic Protocols and Communication: MCP, A2A, NLWeb
Tutorials explain how to use MCP, A2A, and NLWeb agentic communication for improved context management. Analysis of API limitations continues the discussion of context-aware, intent-driven automation and its impact on lifecycle, versioning, and security—in line with recent best practices.
- [Using Agentic Protocols (MCP, A2A, and NLWeb)](/ai/videos/using-agentic-protocols-mcp-a2a-and-nlweb)
- [Why APIs Alone Won’t Cut It in the AI Era](https://devops.com/why-apis-alone-wont-cut-it-in-the-ai-era/?utm_source=rss&utm_medium=rss&utm_campaign=why-apis-alone-wont-cut-it-in-the-ai-era)
## Advanced Search and Security: GraphRAG and Shadow AI Management
GraphRAG combines RAG and semantic graph search, supporting richer enterprise AI search and analytics and deepening security analysis. Guidance on managing shadow AI risk builds on compliance discussions, offering steps for monitoring and regulatory alignment.
- [Exploring GraphRAG: AI-Powered Graph Search for Security Data Analysis](/ai/videos/exploring-graphrag-ai-powered-graph-search-for-security-data-analysis)
- [Staying on Top of Shadow AI](https://devops.com/staying-on-top-of-shadow-ai/?utm_source=rss&utm_medium=rss&utm_campaign=staying-on-top-of-shadow-ai)
## Agent Observability, Cost Management, and HR Automation in the Enterprise
Agent observability and benchmarking resources provide practical recommendations for reliability, cost tracking, and compliance. Tutorials help teams manage AI project budgets and operational visibility. A case study details how Chemist Warehouse uses Azure AI Foundry to automate HR tasks, continuing documentation of AI adoption in specific business sectors.
- [Agent Factory: Top 5 Agent Observability Best Practices for Reliable AI](https://azure.microsoft.com/en-us/blog/agent-factory-top-5-agent-observability-best-practices-for-reliable-ai/)
- [Why Did the Cost of My AI Agent Exceed the Set Budget?](/ai/videos/why-did-the-cost-of-my-ai-agent-exceed-the-set-budget)
- [Context Window: Answering 3 Developer Questions to Save on AI Costs](/ai/videos/context-window-answering-3-developer-questions-to-save-on-ai-costs)
- [How Chemist Warehouse Uses Azure AI Foundry for HR Transformation](https://news.microsoft.com/source/asia/features/a-digital-colleague-how-chemist-warehouse-and-insurgence-ai-are-rewriting-the-hr-playbook/)
## Developer Workflows: Tutorials, Email Agents, and Open Source Trends
New tutorials build on recent agent setup guidance, demonstrating how to create production-ready designs including AI-powered email agents using Copilot Studio and Azure Communication Services. Discussions highlight the benefits of open source and project-based learning, emphasizing curiosity, skill development, and hands-on exploration for tech careers.
- [Build an AI Email Agent with Microsoft Copilot Studio and Azure Communication Services](https://techcommunity.microsoft.com/t5/azure-communication-services/build-your-ai-email-agent-with-microsoft-copilot-studio/ba-p/4448724)
- [Rediscovering Joy in Learning: Jason Lengstorf on AI, Open Source, and Developer Growth](https://github.blog/developer-skills/career-growth/rediscovering-joy-in-learning-jason-lengstorf-on-the-state-of-development/)
- [AI, Project-Based Learning, and the Future of Tech Careers](/ai/videos/ai-project-based-learning-and-the-future-of-tech-careers)',
    'Recent AI developments emphasize the evolution of multi-agent frameworks, improved retrieval workflows, enhanced security, and better cost controls, particularly within Azure and the open source community. The updates include new APIs, orchestration models, guides for enterprise adoption, and real-world experiences dealing with shadow AI and developer upskilling.',
    1756710000, 'ai', '/ai/roundups/weekly-ai-roundup-2025-09-01', 'TechHub',
    'TechHub', '5F95C51D4730357B4B5193ED17FB216A0C6D112629268ADF7512D0664A1FDDB3', ',Azure AI Foundry,Multi Agent Systems,Agent Orchestration,Retrieval Augmented Generation,Responses API,GPT 5,Tool Calling,Semantic Kernel,Prompt Injection Defense,Agentic Protocols,MCP,A2A,NLWeb,GraphRAG,Shadow AI,Agent Observability,Cost Management,Copilot Studio,Azure Communication Services,Open Source,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-08-25
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-08-25', 'roundups', 'Weekly AI Roundup: Agents, Local Models, and Tool Integration',
    'This week, the AI landscape included updated frameworks for agent-based development, additional options for local deployment, enhancements to developer tools, and resources for constructing agentic and generative AI systems. The focus continued to be on flexible integration—local or cloud—and supporting enterprise-scale agent architectures as well as individual productivity needs. Tutorials centered on adaptable model options, privacy, and streamlined AI workflow orchestration.
<!--excerpt_end-->
## Agentic AI Development on Azure AI Foundry
Building on previous content about Agent Factory and orchestration, this week presents new guides and technical references for deploying agents within Azure AI Foundry. Resources support enterprise use, including documentation for MCP integration and Logic Apps connectivity.
A multi-agent architecture reference, complete with industry case studies (such as cybersecurity and retail), extends earlier material about governance and versioning in production environments.
Guidance on selecting models is now broader and more practical for first-time deployment. New resources walk through the use of the Foundry Model Catalog and Model Router, continuing to address compliance and matching use cases to specific business needs—reiterating points made in earlier roundups.
- [Agent Factory: Building Your First AI Agent with Azure AI Foundry](https://azure.microsoft.com/en-us/blog/agent-factory-building-your-first-ai-agent-with-the-tools-to-deliver-real-world-outcomes/)
- [Designing Multi-Agent Intelligence: Microsoft Reference Architecture and Enterprise Case Studies](https://devblogs.microsoft.com/blog/designing-multi-agent-intelligence)
- [How to Choose the Right Model for Your AI Agent: A Developer’s Guide](https://techcommunity.microsoft.com/t5/microsoft-developer-community/how-do-i-choose-the-right-model-for-my-agent/ba-p/4445267)
## Advanced AI Features and Integration in Developer Tools
Extending earlier GPT-5 integration news, Visual Studio Code now allows developers to switch between GPT-5 and GPT-5 mini, giving them more direct control over price and speed. These adjustments are part of the Copilot/VS Code move toward greater convergence and personalization. Additional features such as ‘beast mode’ and task list automation give users new customized workflows.
Azure AI Foundry’s new GPT-5 freeform tool calling allows for flexible Python/SQL execution, moving beyond previous, more restricted function-call API patterns. These capabilities reflect the increasing sophistication of agentic and orchestrated workflows.
In addition, a new tutorial on Mistral Document AI provides hands-on steps for incorporating document parsing into developer environments, supporting conversion of unstructured PDFs and handwriting to structured, AI-ready data.
- [Hello GPT-5 & GPT-5 mini: New AI Features in VS Code Agent Mode](/ai/videos/hello-gpt-5-and-gpt-5-mini-new-ai-features-in-vs-code-agent-mode)
- [Unlocking GPT-5’s Freeform Tool Calling in Azure AI Foundry](https://devblogs.microsoft.com/foundry/unlocking-gpt-5s-freeform-tool-calling-a-new-era-of-seamless-integration/)
- [Mistral Document AI Integration with Azure AI Foundry](/ai/videos/mistral-document-ai-integration-with-azure-ai-foundry)
## Local Model Hosting and Deployment with .NET and Foundry Local
Following prior coverage of local model inference, this week’s resources include detailed instructions for running open-source models such as GPT-OSS within C# projects using Ollama. These methods align with recent developments for Foundry Local and ONNX integration—letting developers deploy streaming chatbots and retrieval-augmented generation solutions on local machines. Forthcoming enhancements in Windows and hardware acceleration reinforce the trend toward hybrid AI workloads.
“Beginner’s Guide” articles detail use of Foundry Local alongside Microsoft Olive, covering ONNX optimization, choosing formats, and troubleshooting—helping more developers move toward flexible AI deployments.
- [Running GPT-OSS Locally in C# Using Ollama and Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/gpt-oss-csharp-ollama/)
- [Beginner’s Guide: Using Custom AI Models with Foundry Local and Microsoft Olive](https://techcommunity.microsoft.com/t5/educator-developer-blog/how-to-use-custom-models-with-foundry-local-a-beginner-s-guide/ba-p/4428857)
## Workflow Automation and Copilot Studio Development
Continuing from last week’s focus on no-code tools, this week offers resources for deeper organizational integration in Copilot Studio. In-depth guides explain the creation of custom plugins and connectors, including advanced OpenAPI authentication, supporting more tailored organizational automation strategies.
Step-by-step resources cover integrating Copilot Studio with Power Automate for process automation involving platforms like SharePoint and Dynamics 365. The system’s library of over a thousand prebuilt connectors further supports broader enterprise workflow automation.
- [Creating Custom Plugins and Connectors in Copilot Studio](https://dellenny.com/creating-custom-plugins-and-connectors-in-copilot-studio/)
- [Integrating Copilot Studio with Power Automate for End-to-End Workflows](https://dellenny.com/integrating-copilot-studio-with-power-automate-for-end-to-end-workflows/)
## Specialty Agents and Agent-Centered Design
This week’s updates highlight the transition from traditional user experience (UX) to agent experience (AX). The Lacuna agent, created using Copilot Studio and AI Foundry, is designed to identify hidden assumptions in product design, expanding on last week’s discussion of agent-based collaboration. Agents focused on domains such as code review and risk assessment are also featured.
The discussion encourages a new focus on agent-centered design, emphasizing planning, orchestration, and domain expertise over simple chatbot-based systems. This builds on last week’s analysis of GPT-4-based planners and Semantic Kernel tools, demonstrating real-world adoption in risk and assumption management.
- [The Future of AI: Developing Lacuna – An Agent for Revealing Quiet Assumptions in Product Design](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/the-future-of-ai-developing-lacuna-an-agent-for-revealing-quiet/ba-p/4434633)
- [From UX to AX: Why Agent Experience is the Next Frontier in Business AI](/ai/videos/from-ux-to-ax-why-agent-experience-is-the-next-frontier-in-business-ai)',
    'This week, the AI landscape included updated frameworks for agent-based development, additional options for local deployment, enhancements to developer tools, and resources for constructing agentic and generative AI systems. The focus continued to be on flexible integration—local or cloud—and supporting enterprise-scale agent architectures as well as individual productivity needs. Tutorials centered on adaptable model options, privacy, and streamlined AI workflow orchestration.',
    1756105200, 'ai', '/ai/roundups/weekly-ai-roundup-2025-08-25', 'TechHub',
    'TechHub', '3334A917E23098CEDFF01623350951954833F3F72F4778B616DB9125A43E1E46', ',Agentic AI,Multi Agent Systems,Azure AI Foundry,Model Selection,Model Catalog,Model Router,MCP,Logic Apps,GitHub Copilot,VS Code,GPT 5,Tool Calling,Local Inference,Foundry Local,.NET,C#,Ollama,ONNX,Microsoft Olive,RAG,Copilot Studio,Power Automate,OpenAPI,Mistral Document AI,Document Parsing,Agent Experience,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-08-18
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-08-18', 'roundups', 'Weekly AI Roundup: GPT-5 Everywhere, Agents, MCP, and Trust Gaps',
    'AI development accelerates with strategic changes at Microsoft, broader model support, new orchestration frameworks, and evolving developer perspectives. GPT-5, Copilot Studio, and MCP are pushing enterprise innovation, security, and practical tool adoption forward. These updates show AI not just assisting work but actively transforming how software and systems are designed, deployed, and maintained—impacting skills, policies, DevOps, and open-source integration.
<!--excerpt_end-->
## Strategic Shifts and Leadership in Microsoft''s AI Ecosystem
GitHub CEO Thomas Dohmke has announced he’ll leave by late 2025 as Microsoft folds GitHub directly into its CoreAI engineering team—ending GitHub’s independent structure and speeding up the flow of AI features for developers. GitHub’s open-source focus will stay, but new features and improved onboarding will come faster under centralized leadership.
- [GitHub CEO Steps Down as Microsoft Integrates GitHub with CoreAI Team](https://devops.com/github-ceo-to-step-down-as-company-is-more-tightly-embraced-by-microsofts-coreai-team/?utm_source=rss&utm_medium=rss&utm_campaign=github-ceo-to-step-down-as-company-is-more-tightly-embraced-by-microsofts-coreai-team)
## GPT-5 and AI Model Integrations Across Developer Platforms
Following the rollout of GPT-5 in Azure and Microsoft’s platform last week, broader support is available now through GitHub Copilot, Azure AI Foundry, VS Code, and other SDKs. GPT-5—including the “mini” version—is now Copilot’s default and helps power agent orchestration in Copilot Studio. Developers benefit from secure access controls, advanced model routing, easier local/cloud inference, and clear setup guides. The transition from preview options to default status, plus modular integrations and more stability, all point to GPT-5 becoming the new norm for production AI.
- [GPT-5 Integrations for Microsoft Developers: GitHub Copilot, Azure AI, and VS Code](https://devblogs.microsoft.com/blog/gpt-5-for-microsoft-developers)
- [Using GPT-5 with Azure AI Foundry, GitHub Copilot, and Copilot Studio in the Microsoft Ecosystem](/ai/videos/using-gpt-5-with-azure-ai-foundry-github-copilot-and-copilot-studio-in-the-microsoft-ecosystem)
- [GPT-5 for Developers](/ai/videos/gpt-5-for-developers)
- [Evaluating GPT-5 Models for RAG on Azure AI Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/gpt-5-will-it-rag/ba-p/4442676)
## Advancements in Agentic AI and Enterprise Orchestration Patterns
Enterprise Agentic AI is moving forward with "Agent Factory," a new orchestration toolkit for agent design—spanning tool usage, workflow planning, and team coordination—built on Azure AI Foundry. The framework includes APIs, an agent catalog, a no-code designer, and Logic Apps integration, making it easier for organizations to deploy and govern agents. This builds on last week’s focus on multiple interacting agents and brings new patterns for teams looking to put agents in real production settings.
- [Agent Factory: Enterprise Patterns and Best Practices for Agentic AI with Azure AI Foundry](https://azure.microsoft.com/en-us/blog/agent-factory-the-new-era-of-agentic-ai-common-use-cases-and-design-patterns/)
- [Model Mondays S2E9: Models for AI Agents](https://techcommunity.microsoft.com/t5/educator-developer-blog/model-mondays-s2e9-models-for-ai-agents/ba-p/4443162)
- [AI Agent''s Toolbox: Building Intelligent Agents with Semantic Kernel, MCP Servers, and Python](/ai/videos/ai-agents-toolbox-building-intelligent-agents-with-semantic-kernel-mcp-servers-and-python)
- [Building AI Agents with Semantic Kernel, MCP Servers, and Python](/ai/videos/building-ai-agents-with-semantic-kernel-mcp-servers-and-python)
## The Rise and Evolution of Copilot Studio
Copilot Studio has matured into a no-code hub for building conversational automation, branching out from its Power Virtual Agents roots. Now you can use GPT-powered AI, deploy across multiple channels, and extend it with plugins for scenarios like customer support, HR, or lead management. There are step-by-step guides for non-developers, and direct deployment is more accessible. The ongoing improvements reflect Microsoft’s focus on making automation possible for everyone—from large enterprises to individuals just starting out.
- [Top 5 Use Cases for Copilot Studio in Your Business](https://dellenny.com/top-5-use-cases-for-copilot-studio-in-your-business/)
- [Copilot Studio vs. Power Virtual Agents: What’s Changed?](https://dellenny.com/copilot-studio-vs-power-virtual-agents-whats-changed/)
- [No-Code AI: Building Chatbots with Copilot Studio for Non-Developers](https://dellenny.com/no-code-ai-how-non-developers-can-build-smart-chatbots-with-copilot-studio/)
## Expanding the Model Context Protocol (MCP) Ecosystem
The Model Context Protocol (MCP) is gaining traction as an open standard—offering new integrations with VS Code, Foundry Agent, and Sentry for secure, consistent AI workflows at scale. MCP is being positioned as a modern replacement for SQL in database tasks and as a core orchestration layer for agents. Sentry’s direct monitoring helps teams observe agent operations in real time.
- [Exploring MCP Workflow for Database Management without SQL](/ai/videos/exploring-mcp-workflow-for-database-management-without-sql)
- [Boost Your Productivity with Visual Studio & Model Context Protocol (MCP) Servers](/ai/videos/boost-your-productivity-with-visual-studio-and-model-context-protocol-mcp-servers)
- [Introduction to Model Context Protocol (MCP) Servers: Building AI Integrations](/ai/videos/introduction-to-model-context-protocol-mcp-servers-building-ai-integrations)
- [Unlocking AI Interoperability with Model Context Protocol (MCP)](/ai/videos/unlocking-ai-interoperability-with-model-context-protocol-mcp)
- [Integrate Intelligent Agents with MCP and Azure AI Foundry on App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/supercharge-your-app-service-apps-with-ai-foundry-agents/ba-p/4444310)
- [Sentry Integrates MCP Server Monitoring into APM Platform for AI Workflows](https://devops.com/sentry-adds-tool-for-monitoring-mcp-servers-to-apm-platform/?utm_source=rss&utm_medium=rss&utm_campaign=sentry-adds-tool-for-monitoring-mcp-servers-to-apm-platform)
## AI Adoption, Trust, and Code Security in Practice
The Stack Overflow Developer Survey for 2025 reports nearly universal use of AI tools, but confidence in automated output has dropped—developers still rely on their own judgment, especially for autonomous systems. SonarSource’s study flagged persistent security and maintainability issues with LLM-generated code, highlighting the necessity for strict oversight. These patterns echo last week’s concerns about governance and code review.
- [Stack Overflow Survey Reveals Developer Attitudes Toward AI Tools in 2025](https://devops.com/stack-overflow-survey-shows-ai-adoption-for-devs/?utm_source=rss&utm_medium=rss&utm_campaign=stack-overflow-survey-shows-ai-adoption-for-devs)
- [SonarSource Highlights Security Risks and Code Quality Issues in LLM-Generated Code](https://devops.com/sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code)
## Innovations in Document Intelligence, Data Analytics, and Azure-powered AI
Developers working with unstructured data will find new options in Mistral Document AI on Azure AI Foundry—supporting complex, multilingual document analysis with faster table extraction and less latency. Updates in Microsoft Fabric and SharePoint Embedded enable real-time analytics and no-code extension options, continuing last week’s focus on bridging AI, analytics, and business systems.
- [Mistral Document AI Launches on Azure AI Foundry: Seamless Document Intelligence at Scale](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/deepening-our-partnership-with-mistral-ai-on-azure-ai-foundry/ba-p/4434656)
- [Advancements in Table Structure Recognition with Azure Document Intelligence](https://techcommunity.microsoft.com/t5/azure-ai-foundry-blog/unveiling-the-next-generation-of-table-structure-recognition/ba-p/4443684)
- [Data Intelligence at Your Fingertips: Fabric’s AI Functions & Data Agents](https://techcommunity.microsoft.com/t5/events/data-intelligence-at-your-fingertips-fabric-s-ai-functions-data/ec-p/4443431#M10)
- [Build the Future of AI-Driven Apps with SharePoint Embedded](https://techcommunity.microsoft.com/t5/microsoft-sharepoint-blog/build-the-future-of-ai-driven-apps-with-sharepoint-embedded/ba-p/4442595)
## Updates in Platform, Tooling, and AI Skills Development
Microsoft has open sourced the Windows Subsystem for Linux (WSL) and introduced Windows AI Foundry to let anybody build custom AI workflows and run models locally. In Australia, a nationwide AI skills program is reaching millions for hands-on upskilling. Azure Cognitive Services has published new resources outlining real-world value. Collectively, these advances push hybrid and on-device AI, as well as open up more possibilities for developers at every level.
- [MSBuild 2025 Highlights: Open Sourcing WSL and Windows AI Foundry](/ai/videos/msbuild-2025-highlights-open-sourcing-wsl-and-windows-ai-foundry)
- [Future Skills Organisation and Microsoft Launch Nationwide AI Skills Accelerator in Australia](https://news.microsoft.com/source/asia/features/fso-microsoft-skills-accelerator-ai/)
- [Unlocking the Power of AI with Azure Cognitive Services](https://dellenny.com/unlocking-the-power-of-ai-with-azure-cognitive-services/)
## Other AI News
AI-powered workflows are now common in Azure, Copilot, and OpenAI environments—including Microsoft Teams, document intelligence, agent-building in VS Code, Azure deployments, and more powerful function-calling for agents. Security and compliance for generative AI have grown with new red-teaming approaches, RAG security checks, PII redaction, and fresh monitoring tools. The Azure AI blog network has now merged for simpler access to technical content and practical guidance.
- [Building a Teams App with Azure Databricks Genie and Azure AI Agent Service](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/supercharge-data-intelligence-build-teams-app-with-azure/ba-p/4442653)
- [Extracting Page Numbers from PDFs with Azure AI Search and OCR](https://techcommunity.microsoft.com/t5/azure-paas-blog/finding-the-right-page-number-in-pdfs-with-ai-search/ba-p/4440758)
- [AI-powered appointment scheduling using Azure OpenAI and Communication Services](https://techcommunity.microsoft.com/t5/azure-communication-services/building-an-ai-receptionist-a-hands-on-demo-with-azure/ba-p/4442959)
- [Building Applications Locally with gpt-oss-20b and the AI Toolkit for VS Code](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-application-with-gpt-oss-20b-with-ai-toolkit/ba-p/4441486)
- [Deploying Lightweight AI Apps on Azure App Service Using GPT-OSS-20B and Flask](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-lightweight-ai-apps-on-azure-app-service-with-gpt-oss-20b/ba-p/4442885)
- [Building AI Agents with Ease: Function Calling in VS Code AI Toolkit](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-ai-agents-with-ease-function-calling-in-vs-code-ai/ba-p/4442637)
- [Red-teaming a RAG Application with Azure AI Evaluation SDK](https://techcommunity.microsoft.com/t5/microsoft-developer-community/red-teaming-a-rag-app-with-the-azure-ai-evaluation-sdk/ba-p/4442682)
- [Announcing the August Preview Model for PII Redaction in Azure AI Language](https://techcommunity.microsoft.com/t5/azure-ai-foundry-blog/announcing-the-text-pii-august-preview-model-release-in-azure-ai/ba-p/4441705)
- [Azure Logic App AI-Powered Monitoring Solution: Automate, Analyze, and Act on Your Azure Data](https://techcommunity.microsoft.com/t5/healthcare-and-life-sciences/azure-logic-app-ai-powered-monitoring-solution-automate-analyze/ba-p/4442665)
- [Azure AI Blogs Consolidate into New Azure AI Foundry Blog](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/exciting-news-azure-ai-blogs-have-come-together-in-the-new-azure/ba-p/4443002)
- [Build Next-Gen AI Apps with .NET and Azure](/ai/videos/build-next-gen-ai-apps-with-net-and-azure)
- [The Right Kind of AI for Infrastructure as Code](https://devops.com/the-right-kind-of-ai-for-infrastructure-as-code/?utm_source=rss&utm_medium=rss&utm_campaign=the-right-kind-of-ai-for-infrastructure-as-code)
- [Copado Enhances AI Tools to Uncover Salesforce Code Relationships](https://devops.com/copado-extends-ai-reach-to-surface-relationships-between-salesforce-code/?utm_source=rss&utm_medium=rss&utm_campaign=copado-extends-ai-reach-to-surface-relationships-between-salesforce-code)
- [Building Applications Locally with gpt-oss-20b and the AI Toolkit for VS Code](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-application-with-gpt-oss-20b-with-ai-toolkit/ba-p/4441486)
- [Deploying Lightweight AI Apps on Azure App Service Using GPT-OSS-20B and Flask](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-lightweight-ai-apps-on-azure-app-service-with-gpt-oss-20b/ba-p/4442885)
- [Designing Empathetic AI Experiences: Trish Winter-Hunt on Content Design and Azure AI Foundry](/ai/videos/designing-empathetic-ai-experiences-trish-winter-hunt-on-content-design-and-azure-ai-foundry)
- [Q1 2025 GitHub Innovation Graph Update: Trends in Data Visualization and AI Development](https://github.blog/news-insights/policy-news-and-insights/q1-2025-innovation-graph-update-bar-chart-races-data-visualization-on-the-rise-and-key-research/)
- [Generative AI for Permitting: Accelerating Clean Energy with Microsoft](https://www.microsoft.com/en-us/garage/wall-of-fame/generative-ai-for-permitting/)
- [Future Skills Organisation and Microsoft Launch Nationwide AI Skills Accelerator in Australia](https://news.microsoft.com/source/asia/features/fso-microsoft-skills-accelerator-ai/)
- [Futurum Signal: AI-Powered Market Intelligence for DevOps and Platform Engineering](https://devops.com/futurum-signal-ai-powered-market-intelligence-for-devops-and-platform-engineering/?utm_source=rss&utm_medium=rss&utm_campaign=futurum-signal-ai-powered-market-intelligence-for-devops-and-platform-engineering)
- [Implementing a Center of Excellence for Generative AI](https://www.thomasmaurer.ch/2025/08/implementing-a-center-of-excellence-for-generative-ai/)',
    'AI development accelerates with strategic changes at Microsoft, broader model support, new orchestration frameworks, and evolving developer perspectives. GPT-5, Copilot Studio, and MCP are pushing enterprise innovation, security, and practical tool adoption forward. These updates show AI not just assisting work but actively transforming how software and systems are designed, deployed, and maintained—impacting skills, policies, DevOps, and open-source integration.',
    1755500400, 'ai', '/ai/roundups/weekly-ai-roundup-2025-08-18', 'TechHub',
    'TechHub', '5B29B9FD215F12531791A434DA846E8235AFE6CD2F66122EED82B65312C8885C', ',GPT 5,GitHub Copilot,Copilot Studio,Azure AI Foundry,Agentic AI,Agent Orchestration,MCP,Semantic Kernel,Retrieval Augmented Generation,Azure AI Search,Document Intelligence,Microsoft Fabric,VS Code AI Toolkit,AI Governance,LLM Code Security,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-08-11
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-08-11', 'roundups', 'Weekly AI Roundup: GPT-5 Everywhere, Hybrid Models, and Agents',
    'This week’s AI landscape saw transformative updates strengthening model flexibility, hybrid deployment, agent orchestration, and reproducibility—signaling a practical shift toward governable AI-centric operations for both developers and enterprises.
<!--excerpt_end-->
## Universal Access to GPT-5 and gpt-oss: Hybrid AI Takes Center Stage
OpenAI’s GPT-5 family and new gpt-oss open-weight models are now fully supported in the Microsoft ecosystem, including Azure AI Foundry and VS Code’s AI Toolkit. Developers can test models like gpt-oss-120b locally or on Azure, benefit from chain-of-thought prompting, and use the unified catalog and code generation features, easing multi-cloud and edge deployments and reducing vendor lock-in.
- [GPT-5 and GPT OSS Models Now Integrated in AI Toolkit for VS Code](https://techcommunity.microsoft.com/t5/microsoft-developer-community/gpt-5-family-of-models-gpt-oss-are-now-available-in-ai-toolkit/ba-p/4441394)
- [OpenAI’s gpt‑oss Models Now Available on Azure AI Foundry and Windows AI Foundry](https://azure.microsoft.com/en-us/blog/openais-open%E2%80%91source-model-gpt%E2%80%91oss-on-azure-ai-foundry-and-windows-ai-foundry/)
## GPT-5 Arrives: New Standards for Coding, Agents, and Enterprise Security
Launch of GPT-5 and variants in Azure and GitHub Models boosts agentic automation, enabling dynamic multi-model workflows, task optimization, and transparency. Centralized observability and compliance—via Azure AI Content Safety and Purview integration—support secure deployment, driving broad industry adoption of agentic AI.
- [GPT-5 Launches in Azure AI Foundry: New Era for AI Apps, Agents, and Developers](https://azure.microsoft.com/en-us/blog/gpt-5-in-azure-ai-foundry-the-future-of-ai-apps-and-agents-starts-here/)
- [GPT-5 is now generally available in GitHub Models](https://github.blog/changelog/2025-08-07-gpt-5-is-now-generally-available-in-github-models)
## Autonomous Agents and Multi-Agent Patterns
Autonomous multi-agent systems are hitting real-world scale: Project Ire’s agentic malware classification is now in Defender, relieving analysts. “Async SWEs” shows AI fleets orchestrating full developer lifecycles. Composable multi-agent frameworks—like Dapr Durable AI Agents—simplify orchestration, error-handling, and monitoring, building on last week’s multi-agent maturation trend.
- [Project Ire: Autonomous AI Agent for Large-Scale Malware Detection](https://www.microsoft.com/en-us/research/blog/project-ire-autonomously-identifies-malware-at-scale/)
- [Compound Software Development with Async SWE Agents](/ai/videos/compound-software-development-with-async-swe-agents-orchestrating-ai-for-engineering-productivity)
## Next-Generation Reasoning, Transparency, and Evaluation
CLIO enables self-adaptive, user-steerable AI reasoning with explicit uncertainty controls for science and engineering. The Semantic Clinic toolkit and new .NET agent/NLP evaluators deliver rigorous AI debugging and systematic, reproducible evaluation—accelerating the push toward test-driven agent pipelines highlighted previously.
- [Introducing CLIO: Microsoft’s Self-Adaptive AI Reasoning System for Science](https://www.microsoft.com/en-us/research/blog/self-adaptive-reasoning-for-science/)
- [Semantic Clinic: A Math-First, Model-Agnostic Map for Diagnosing AI Failures](https://www.reddit.com/r/devops/comments/1mktxxc/semantic_clinic_a_reproducible_map_of_ai_failures/)
## AI-First Workflows: Automation, Data Quality, and Model Lifecycle
Best-practice guides detail integrating AI in Actions workflows, proactive data cleanup with VS Code Data Wrangler, and model management with “model operation agents.” AI powers new analytics, gold mapping, and SEO blog generation, tying into trends for practical, agent-managed automation.
- [How to Use AI Models in Your GitHub Actions Workflows](/ai/videos/how-to-use-ai-models-in-your-github-actions-workflows)
- [How to Quickly Catch and Clean Bad Data for AI Agents with VS Code Data Wrangler](https://techcommunity.microsoft.com/t5/microsoft-developer-community/how-do-i-catch-bad-data-before-it-derails-my-agent/ba-p/4440397)
## Developer Experience Evolves: MCP, Observability, and Accessibility
MCP is positioned as the “new browser”—enabling context-rich model/telemetry integration and root-cause analysis. AI accessibility takes a leap with Teams’ Sign Language Mode, while AI Shell Preview 6 and Copilot Studio democratize rapid bot and workflow deployment.
- ["MCP is the new browser?" — Kent C. Dodds x Burke Holland, Live](/ai/videos/mcp-is-the-new-browser-kent-c-dodds-x-burke-holland-live)
- [Inclusive communication with Sign Language Mode in Microsoft Teams](https://techcommunity.microsoft.com/t5/microsoft-teams-blog/inclusive-communication-with-sign-language-mode-in-microsoft/ba-p/4438299)
## Real-World Field Reporting: AI Agent Successes and Pitfalls
A six-month field study on AI agents in sales/support details best practices (e.g., strict tool typing, observability) and chronicled pitfalls (memory drift, loss, escalations), delivering a blueprint for safe, scalable workflow automation.
- [6-Month Field Report: AI Agents in SDR & L1 Support—What Worked and What Broke](https://www.reddit.com/r/AI_Agents/comments/1mktrgm/from_hype_to_headcount_6month_field_report_ai/)
## Economic and Organizational Impact: Productivity, Risk, and Governance
Survey data shows C-levels report big productivity and cost gains, but field research reveals perceived improvements often outpace realized efficiency. Organizations are setting up GenAI Centers of Excellence and maturing AI-powered operations to institutionalize responsible governance and resilience.
- [Survey Shows AI Drives Massive Economic Gains in Software Development](https://devops.com/survey-attributes-massive-economic-gains-to-rise-of-ai-in-software-development/?utm_source=rss&utm_medium=rss&utm_campaign=survey-attributes-massive-economic-gains-to-rise-of-ai-in-software-development)
## Community and Learning: Adoption and Guidance
Interactive learning and community events showcase security, workflow enablement, and rapid adoption of new AI/Foundry features.
- [Weekly Microsoft Learning Rooms Community Roundup (8/7)](https://techcommunity.microsoft.com/t5/microsoft-learn/microsoft-learning-rooms-weekly-round-up-8-7/m-p/4441646#M17145)
AI is now firmly embedded in software and enterprise workflows—practical, governable, and developer-first.',
    'This week’s AI landscape saw transformative updates strengthening model flexibility, hybrid deployment, agent orchestration, and reproducibility—signaling a practical shift toward governable AI-centric operations for both developers and enterprises.',
    1754895600, 'ai', '/ai/roundups/weekly-ai-roundup-2025-08-11', 'TechHub',
    'TechHub', '2A6485F2C318FBAF8BD2D3CC6048B508BEB3A3314838BEDC18C5149D5B8D60AB', ',GPT 5,Gpt Oss,Azure AI Foundry,Windows AI Foundry,VS Code AI Toolkit,GitHub Models,Agentic AI,Multi Agent Systems,Dapr Durable AI Agents,MCP,AI Observability,Azure AI Content Safety,Microsoft Purview,AI Evaluation,Responsible AI Governance,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-08-04
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-08-04', 'roundups', 'Weekly AI Roundup: Secure MCP, A2A Standards, and .NET Agents',
    'AI development this week was defined by major advancements from Microsoft and partners, focused on secure, interoperable agentic infrastructure, agent-to-agent standards, and practical, scalable tools for developers. From MCP and A2A protocols to deep .NET and Azure AI integration, this maturing ecosystem is enabling productive, developer-friendly, and robust AI deployments across industries.
<!--excerpt_end-->
## Building Secure and Scalable AI Agent Infrastructure
Microsoft’s enterprise-ready MCP blueprint equips developers to deploy multimodal agent systems on Azure, with best-in-class security and scaling (OAuth2/Entra ID integration, container-based deployment, real code patterns, latency optimization). This closes the gap between prototype and real-world production for advanced AI features.
- [Advanced MCP: Secure, Scalable, and Multi-Modal AI Agents](/ai/videos/advanced-mcp-secure-scalable-and-multi-modal-ai-agents)
## MCP and A2A: Foundations for Agentic Collaboration
Expanding on last week’s focus, open standards like MCP (with new OAuth 2.1 flows) and A2A SDK previews are now central for agent-to-agent communication and productivity. Workshops, bootcamps, and multi-language resources are boosting adoption and teaching schema-driven, robust orchestration from concept through production. Business and technical sessions highlight MCP’s compliance impact, and A2A’s message-based negotiation and capability discovery.
- [MCP Gets OAuth: Understanding the New Authorization Specification](/ai/videos/mcp-gets-oauth-understanding-the-new-authorization-specification)
- [Agents Talking to Agents: Harnessing MCP for Seamless Inter-Agent Collaboration](/ai/videos/agents-talking-to-agents-harnessing-mcp-for-seamless-inter-agent-collaboration)
- [Building Collaborative AI Agents with the A2A .NET SDK](https://devblogs.microsoft.com/foundry/building-ai-agents-a2a-dotnet-sdk/)
- [Build AI Agents in VS Code: 4 Hands-On Labs with MCP + AI Toolkit](/ai/videos/build-ai-agents-in-vs-code-4-hands-on-labs-with-mcp-ai-toolkit)
- [MCP Dev Days Day 2: From Concept to Code](/ai/videos/mcp-dev-days-day-2-from-concept-to-code)
- [Ctrl Shift - MCP & A2A: Why Business Leaders Should Care](/ai/videos/ctrl-shift-mcp-and-a2a-why-business-leaders-should-care)
- [Full Course: MCP for Beginners (Lessons 1-11) by Microsoft Developer](/ai/videos/full-course-mcp-for-beginners-lessons-1-11-by-microsoft-developer)
- [Let’s Learn MCP Series Recap: 8 Languages, 4 Code Bases, Full Resources](https://devblogs.microsoft.com/blog/lets-learn-mcp-series-recap-8-languages-4-code-bases-full-resources)
- [MCP Bootcamp: APAC, LATAM, and Brazil – Learn Model Context Protocol Integration, LLMs, Azure, and Copilot](https://techcommunity.microsoft.com/t5/microsoft-developer-community/mcp-bootcamp-apac-latam-and-brazil/ba-p/4435966)
## Intelligent Development Workflows with .NET and Azure AI
The .NET MCP SDK and Azure AI Foundry integrations make agent orchestration and rapid prototyping much more accessible. Developers now have privacy-first, offline local agent server options and ASP.NET Core + SignalR templates for real-time, scalable AI chat—demonstrating the practical boost in productivity, security, and debugging for local and cloud workflow development.
- [MCP C# SDK Deep Dive](/ai/videos/mcp-c-sdk-deep-dive)
- [Build Smarter LLMs with Local MCP Servers in .NET](https://www.reddit.com/r/dotnet/comments/1mgbojy/build_smarter_llms_with_local_mcp_servers_in_net/)
- [Blazing-fast AI Chat Apps with ASP.NET Core & SignalR: Insights from the T3 Chat Cloneathon](/ai/videos/blazing-fast-ai-chat-apps-with-aspnet-core-and-signalr-insights-from-the-t3-chat-cloneathon)
## Streamlining Agent-Based Automation and Enterprise Integration
Fresh case studies across health, finance, and data-centric enterprises, plus guides for modular code, remote MCP servers, and Azure-based scaling, reinforce MCP’s practicality for automating complex, compliant AI workflows across stacks.
- [MCP in Action: Real-World Case Studies](/ai/videos/mcp-in-action-real-world-case-studies)
- [Lessons from MCP Early Adopters](/ai/videos/lessons-from-mcp-early-adopters)
- [MCP Development Best Practices](/ai/videos/mcp-development-best-practices)
- [MCP Dev Days: Day 2 - Builders](/ai/videos/mcp-dev-days-day-2-builders)
- [Practical Introduction to Building Remote MCP Servers](/ai/videos/practical-introduction-to-building-remote-mcp-servers)
- [Build your first MCP server](/ai/videos/build-your-first-mcp-server)
- [MCP Core Concepts: Understanding the Architecture and Message Flow](/ai/videos/mcp-core-concepts-understanding-the-architecture-and-message-flow)
## Orchestrating AI Workflows and Prompt Engineering
Semantic Kernel-led orchestration patterns and a roundup of top prompt engineering tools point to practical strategies for chaining agents, modular workflow development, and boosting LLM-powered application efficiency.
- [Building AI Agent Workflows with Semantic Kernel](/ai/videos/building-ai-agent-workflows-with-semantic-kernel)
- [Best Prompt Engineering Tools (2025) for Building and Debugging LLM Agents](https://www.reddit.com/r/AI_Agents/comments/1mc4q9i/best_prompt_engineering_tools_2025_for_building/)
## Scaling AI: Microsoft’s Milestones, Industry Transformation, and Advanced Reasoning
Microsoft ended its fiscal year with record Azure revenue, announced over 100M monthly Copilot users, and spotlighted the rapid mainstreaming of Responsible AI and agentic platforms in sectors like energy and higher education. Research continues to push LLMs toward sharper multi-step reasoning and broader enterprise adoption.
- [Microsoft Fiscal Year Close: Azure and Copilot Milestones Announced](https://www.linkedin.com/posts/satyanadella_we-just-wrapped-our-earnings-call-it-was-activity-7356452669235875840-A-0W)
- [How Microsoft’s customers and partners accelerated AI Transformation in FY25](https://blogs.microsoft.com/blog/2025/07/28/how-microsofts-customers-and-partners-accelerated-ai-transformation-in-fy25-to-innovate-with-purpose-and-shape-their-future-success/)
- [How Microsoft and its partners are reenvisioning energy with AI](https://www.microsoft.com/en-us/industry/blog/energy-and-resources/2025/07/28/driving-the-grid-of-the-future-how-microsoft-and-our-partners-are-reenvisioning-energy-with-ai/)
- [Discover the Potential of Agentic AI in Higher Education](https://www.microsoft.com/en-us/education/blog/2025/07/discover-the-potential-of-agentic-ai-in-higher-education/)
- [How Microsoft Research is enhancing the ability of LLMs to handle more complex reasoning tasks](https://www.microsoft.com/en-us/research/articles/a-ladder-of-reasoning-testing-the-power-of-imagination-in-llms/)',
    'AI development this week was defined by major advancements from Microsoft and partners, focused on secure, interoperable agentic infrastructure, agent-to-agent standards, and practical, scalable tools for developers. From MCP and A2A protocols to deep .NET and Azure AI integration, this maturing ecosystem is enabling productive, developer-friendly, and robust AI deployments across industries.',
    1754290800, 'ai', '/ai/roundups/weekly-ai-roundup-2025-08-04', 'TechHub',
    'TechHub', '9F1B3950A1313CB8FEDC22792B0956752A719F92FEE94D3567A36699AC74230D', ',AI,AI Agents,Agentic AI,MCP,A2A Protocol,OAuth 2.1,Microsoft Entra ID,Azure AI,Azure AI Foundry,.NET,C#,ASP.NET Core,SignalR,Semantic Kernel,Responsible AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-07-28
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-07-28', 'roundups', 'Weekly AI Roundup: Interoperable Agents and Enterprise AI',
    'This week’s AI highlights centered on agent interoperability, open source access, and enterprise adoption, amplifying practical AI deployment across technical and vertical domains.
<!--excerpt_end-->
## Multi-Agent Orchestration and Protocol Standardization
Complex agent systems advanced via Microsoft’s Semantic Kernel and the Agent-to-Agent (A2A) protocol—an emerging standard for secure, discoverable agent communication. Together with the Model Context Protocol (MCP), developers can now compose resilient, orchestrated agent workflows that interoperate across systems like LangGraph and Azure AI Foundry. Open-source guides enable cloud-native agent orchestration, unified error handling, and plugin-based task routing, solidifying practical multi-agent automation (continuing the MCP workflows covered last week).
- [Building Multi-Agent AI Solutions Using Semantic Kernel and the A2A Protocol](https://devblogs.microsoft.com/semantic-kernel/guest-blog-building-multi-agent-solutions-with-semantic-kernel-and-a2a-protocol/)
- [Building Agent-to-Agent Communication with MCP: Capabilities, Patterns, and Implementation](https://devblogs.microsoft.com/blog/can-you-build-agent2agent-communication-on-mcp-yes)
## Open Source AI Access and Infrastructure
GitHub Models launched a free OpenAI-compatible inference API, letting open source projects easily embed AI features using GitHub token authentication—eliminating the need for self-hosting or extra secrets, and democratizing AI in development and CI/CD environments.
Microsoft also previewed Fabric data agents in Copilot Studio, enabling multi-agent automation inside enterprise data pipelines, reducing manual bottlenecks and boosting reliability. These steps expand the MCP and multi-agent ecosystems highlighted previously.
- [Solving the Inference Problem for Open Source AI Projects with GitHub Models](https://github.blog/ai-and-ml/llms/solving-the-inference-problem-for-open-source-ai-projects-with-github-models/)
- [Fabric Data Agents + Microsoft Copilot Studio: Multi-Agent Orchestration Preview Released](https://blog.fabric.microsoft.com/en-US/blog/fabric-data-agents-microsoft-copilot-studio-a-new-era-of-multi-agent-orchestration/)
## AI in Enterprise, Healthcare, and Business Transformation
AI integration in Windows 11, Power Apps, and industry platforms accelerates automation with new APIs and trusted generative features. Microsoft’s MAI-DxO outperformed human doctors, foreshadowing future transformative healthcare workflows. Power Apps now leverages generative AI for low-code logic and insights, spurring productivity in business, finance, healthcare, and retail sectors. These enterprise AI stories deepen last week’s focus on scalable, business-oriented deployments.
- [Windows 11 is the home for AI on the PC, with more experiences available today](https://blogs.windows.com/windowsexperience/2025/07/22/windows-11-is-the-home-for-ai-on-the-pc-with-even-more-experiences-available-today/)
- [Microsoft''s AI Doctor MAI-DxO has crushed human doctors](https://www.reddit.com/r/ArtificialInteligence/comments/1m5ig5j/microsofts_ai_doctor_maidxo_has_crushed_human/)
- [Introducing the new Power Apps - Generative power meets enterprise-grade trust](https://www.microsoft.com/en-us/power-platform/blog/power-apps/introducing-the-new-power-apps-generative-power-meets-enterprise-grade-trust/)
- [AI for business impact starts here - Proven AI use cases by industry](https://www.microsoft.com/en-us/microsoft-cloud/blog/2025/07/21/ai-for-business-impact-starts-here-proven-ai-use-cases-by-industry/)
## Advances in Developer Tooling and Ecosystem Growth
Visual Studio’s planned AI upgrades will embed code generation and debugging directly into the IDE, making AI foundational for developers. Free generative AI training courses, hands-on MCP protocol workshops, and community discussions guide developers on practical adoption strategies—often prioritizing backend automation over chatbots for real business value. These resources echo last week’s peer learning and platform education efforts.
- [Visual Studio might be getting its biggest upgrade in years, and it''ll include AI](https://www.reddit.com/r/VisualStudio/comments/1m81l7y/visual_studio_might_be_getting_its_biggest/)
- [My free AI Course on GitHub is now in Video Format](https://www.reddit.com/r/AI_Agents/comments/1m5ucwp/my_free_ai_course_on_github_is_now_in_video_format/)
- [Let''s Learn Model Context Protocol with JavaScript and TypeScript](/ai/videos/lets-learn-model-context-protocol-with-javascript-and-typescript)
- [Getting Started with MCP (Model Context Protocol)](/ai/videos/getting-started-with-mcp-model-context-protocol)
- [Should I Add a Chatbot to My App? AI Guidance from Steve Sanderson](/should-i-add-a-chatbot-to-my-app-ai-guidance-from-steve-sanderson)
- [Open Source and AI Special with @francescociulla](/ai/videos/open-source-and-ai-special-with-francescociulla)
- [Should I add a chatbot to my app?](/ai/videos/should-i-add-a-chatbot-to-my-app)
## AI Research, Regional Expansion, and Community-Driven Insights
Microsoft and Nvidia are pushing AI in biodiversity research, granting scientists new modeling capabilities. Microsoft Research Asia’s expansion into Singapore creates new APAC opportunities. Microsoft improved European language digital access, reflecting ongoing digital inclusivity efforts.
Community discussions surfaced: Azure Document Intelligence’s reliability with new layouts, the growing role of agent feedback in prototype evaluation, and choosing practical first AI projects. These complement last week’s emphasis on real-world AI integration and continuous community-driven learning.
- [How Microsoft and Nvidia are working together to unlock the secrets of biodiversity](https://www.microsoft.com/en-us/startups/blog/catalyst-basecamp-research-leverages-microsoft-and-nvidia-ai-to-unlock-secrets-of-biodiversity/)
- [Microsoft Research Asia launches Singapore lab](https://news.microsoft.com/source/asia/2025/07/24/microsoft-research-asia-launches-singapore-lab-to-drive-ai-innovation-industrial-transformation-and-talent-development/)
- [Microsoft launches first Southeast Asia AI research lab in Singapore](https://www.reddit.com/r/microsoft/comments/1m9q6o4/microsoft_launches_first_southeast_asia_ai/)
- [Microsoft supports making Europe’s languages and cultures more accessible in the digital realm](https://blogs.microsoft.com/on-the-issues/2025/07/20/eudigitalunlock/)
- [Confidence Score Decline in Document Intelligence Custom Extraction Models with New Layouts](https://techcommunity.microsoft.com/t5/ai-azure-ai-services/doc-intelligence-custom-extraction-model-confidence-score/m-p/4435860#M1270)
- [Agent feedback is the new User feedback](https://www.reddit.com/r/AI_Agents/comments/1m7ldl2/agent_feedback_is_the_new_user_feedback/)
- [Choosing Your First AI Application](/ai/videos/choosing-your-first-ai-application)',
    'This week’s AI highlights centered on agent interoperability, open source access, and enterprise adoption, amplifying practical AI deployment across technical and vertical domains.',
    1753686000, 'ai', '/ai/roundups/weekly-ai-roundup-2025-07-28', 'TechHub',
    'TechHub', '78E4DED661CD3F2470A500228E5820ABA332D5364621CDEB9735F1B3C0C7BCCB', ',AI Agents,Multi Agent Orchestration,Agent To Agent Protocol,MCP,Semantic Kernel,LangGraph,Azure AI Foundry,GitHub Models,OpenAI Compatible API,Copilot Studio,Microsoft Fabric,Windows 11 AI,Power Apps,VS,Azure Document Intelligence,AI,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-07-21
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-07-21', 'roundups', 'Weekly AI Roundup: Agents, MCP Tools, and Adoption Playbooks',
    'AI advancements this week underscore the convergence of automation, agentic architectures, and robust AI adoption frameworks. Key themes include strategic AI trends, agent ecosystems, enterprise enablement, collaborative frameworks, and accessible education.
<!--excerpt_end-->
## Strategic AI Trends and Developer Tools
2025’s software engineering centers on AI at every stage—from prompt-driven workflows and code orchestration, to composable architectures and standardized ethics. Trends highlight low-code democratization, DevSecOps, value-based engineering, and AI-powered multiplatform automation. Team success depends on blending architectural innovation with responsible governance.
- [Key Trends Driving Software Engineering in 2025](https://dellenny.com/key-trends-driving-software-engineering-in-2025/)
## Agentic AI and MCP Ecosystem Expansion
Agentic AI—autonomous, adaptive agents—powers workflow management and orchestration, fueled by a rapidly growing MCP ecosystem. Ten Microsoft MCP servers now connect IDEs to Azure, GitHub, Microsoft 365, SQL, and web testing, standardizing tool integration. Tutorials in Python and Java further expand MCP accessibility, powering real-time, cross-platform developer automation.
- [Agentic AI: The Next Evolution Beyond Generative AI for Solution Architects](https://dellenny.com/agentic-ai-the-next-evolution-beyond-generative-ai-for-solution-architects/)
- [10 Microsoft MCP Servers to Accelerate Your Development Workflow](https://devblogs.microsoft.com/blog/10-microsoft-mcp-servers-to-accelerate-your-development-workflow)
- [Let''s Learn MCP: Python](/ai/videos/lets-learn-mcp-python)
- [Let''s Learn MCP: Java](/ai/videos/lets-learn-mcp-java)
## Enterprise AI Adoption and Center of Excellence Strategies
Centers of Excellence are central to enterprise AI adoption: they align strategy, technical support, and governance, drive staff upskilling, and create scalable, repeatable frameworks. Real-world stories from Oracle, Deloitte, and DoD demonstrate success metrics and best practices, backed by the integration of AI tools like Copilot and VS Code.
- [Building a Center of Excellence for AI: A Strategic Roadmap for Enterprise Adoption](https://hiddedesmet.com/creating-ccoe-for-ai)
## Human-AI Collaboration and Real-World Applications
Microsoft’s collaboration framework aligns LLM outputs with user intention, emphasizing practical and easy human-AI integration. Adecco’s AI-first recruitment uses Azure to automate HR, embedding upskilling for developers and applicants. In education, Minecraft delivers gamified, interactive programming and AI lessons, making complex topics engaging for students and teachers.
- [Microsoft introduces new training framework that enhances human-LLM collaboration](https://www.microsoft.com/en-us/research/blog/collabllm-teaching-llms-to-collaborate-with-users/)
- [Adecco''s AI-First Approach to Recruitment with Microsoft Cloud](/ai/videos/adeccos-ai-first-approach-to-recruitment-with-microsoft-cloud)
- [Integrating interactivity with Minecraft Education - A fun approach to learning coding and AI](https://news.microsoft.com/source/asia/2025/07/15/integrating-interactive-and-collaborative-learning-solutions-with-minecraft-education-a-fun-approach-to-learn-coding-and-ai/)
## Foundational AI and Community Knowledge Exchange
Structured education and peer learning continue to grow. John Savill’s AI/ML primer and Xebia’s Knowledge Exchange events provide foundational training, professional networking, and practical application, supporting entry and progression in the AI field.
- [Artificial Intelligence (AI) and Machine Learning (ML) for Everyone!](/ai/videos/artificial-intelligence-ai-and-machine-learning-ml-for-everyone)
- [Xebia Knowledge Exchange](/xke-coming-soon)',
    'AI advancements this week underscore the convergence of automation, agentic architectures, and robust AI adoption frameworks. Key themes include strategic AI trends, agent ecosystems, enterprise enablement, collaborative frameworks, and accessible education.',
    1753081200, 'ai', '/ai/roundups/weekly-ai-roundup-2025-07-21', 'TechHub',
    'TechHub', 'BB2A91CCBCAD1AAF2D3F154FF0FDC637255BEDD6647CC0C6A00617A3601E9345', ',AI,Agentic AI,MCP,Microsoft MCP Servers,Developer Productivity,Workflow Automation,Prompt Engineering,DevSecOps,Responsible AI,Enterprise AI Adoption,AI Center Of Excellence,Human AI Collaboration,Azure AI,GitHub Copilot,AI Education,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-07-14
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-07-14', 'roundups', 'Weekly AI Roundup: Edge Reasoning, Agents, and People-First AI',
    'AI saw major progress in scalable reasoning models, agentic modernization, and human-centric frameworks. Microsoft and partners led with deployments that merge performance innovation, agent pipelines, and ethical rollout—reflecting AI’s rapid alignment with real-world systems and responsible integration.
<!--excerpt_end-->
## Efficient Edge Reasoning and Model Innovation
Microsoft’s Phi-4-mini-flash-reasoning marks a leap for edge AI with a hybrid SambaY architecture (self-decoding, sliding attention, gated memory) that achieves 10x throughput and 64K context windows with 3.8B parameters. It beats older Phi versions and larger models on logical/analytical tasks with far lower latency, now live in Azure AI Foundry, NVIDIA, and Hugging Face. The emphasis is on building blazing-fast, adaptable, and accurate models for mobile and embedded scenarios.
This continues last week’s theme of Microsoft prioritizing high-performance, accessible models for cloud and edge, with Azure AI Foundry at the core of mainstream deployment.
- [Reasoning Reimagined: Introducing Phi-4-mini-flash-reasoning for Efficient Edge AI](https://azure.microsoft.com/en-us/blog/reasoning-reimagined-introducing-phi-4-mini-flash-reasoning/)
## Agentic Mainframe Modernization
Microsoft''s COBOL Agentic Migration Factory (CAMF) automates legacy mainframe modernization with Semantic Kernel and AutoGen-powered agent pipelines. Agents analyze, document, and convert COBOL to Java/.NET, handling complex chaining and dependencies, while producing auditable transitions. Teams can leverage and customize CAMF pipelines, reducing manual tracing and boosting modernization reliability in mission-critical systems.
This continues the trend—seen in previous roundups—of moving multi-agent orchestration from new app dev to core enterprise refactoring.
- [How We Use AI Agents for COBOL Migration and Mainframe Modernization](https://devblogs.microsoft.com/all-things-azure/how-we-use-ai-agents-for-cobol-migration-and-mainframe-modernization/)
## AI Education, Responsible Transformation, and Human-Centric Initiatives
The National Academy for AI Instruction, supported by Microsoft, OpenAI, and Anthropic, brings structured AI literacy to teachers nationwide—mixing hands-on and ethical best practices. Microsoft Elevate pivots AI transformation toward human skill-building and transparent workflows, prioritizing augmentation and safety over automation. This signals a broader industry shift—spotlighted last week—toward inclusive, responsible AI standards.
- [AFT to launch National Academy for AI Instruction with Microsoft, OpenAI, Anthropic and United Federation of Teachers](https://news.microsoft.com/source/2025/07/08/aft-to-launch-national-academy-for-ai-instruction-with-microsoft-openai-anthropic-and-united-federation-of-teachers/)
- [Microsoft unveils Elevate, putting people first in AI transformation](https://blogs.microsoft.com/on-the-issues/2025/07/09/elevate/)
## Disciplined AI Workflows: Vibe Engineering and Multi-Agent Patterns
Encouraging a shift from informal ‘vibe coding’ to systematic ‘vibe engineering,’ the community adopts architectural constraints, automated tests, and reusable agent patterns with Semantic Kernel. Demonstrations show orchestrating planners, reviewers, and executors—plus human-in-the-loop approval—for maintainable, robust pipelines. Multi-agent best practices are now moving from innovation to mainstream.
This is a direct evolution from last week''s focus on agent orchestration and standardized, scalable AI engineering.
- [From Vibe Coding to Vibe Engineering: It''s Time to Stop Riffing with AI](https://thenewstack.io/from-vibe-coding-to-vibe-engineering-its-time-to-stop-riffing-with-ai/)
- [Building a multi-agent system with Semantic Kernel](https://www.reddit.com/r/dotnet/comments/1ltr8tf/building_a_multiagent_system_with_semantic_kernel/)
## AI for Healthcare, Social Impact, and Upskilling
AI’s reach in healthcare is expanding—startups innovate in clinical automation and engagement, while social impact projects (e.g., an AI chatbot for domestic violence support) win UN acclaim. Upskilling stories from Malaysia illustrate AI’s spillover into workforce growth in tech and retail, confirming the cross-sectoral influence of applied AI.
- [How startups are using AI to support healthcare providers and patients](https://www.microsoft.com/en-us/startups/blog/catalysts-revolutionizing-healthcare-with-pangaea-data-microsoft-azure-and-nvidia/)
- [AI chatbot supporting victims-survivors of domestic violence wins UN Global AI for Good Impact Award](https://news.microsoft.com/de-ch/2025/07/10/ai-chatbot-sophia-supporting-victim-survivors-of-domestic-violence-wins-the-united-nations-global-ai-for-good-impact-award-2025/)
- [From retail to cybersecurity, Malaysians are gaining skills and confidence to succeed with AI](https://news.microsoft.com/source/asia/features/from-retail-to-cybersecurity-malaysians-are-gaining-skills-and-confidence-to-succeed-with-ai/)
## Platform Architecture, MCP, and Deployment Choices
Choice of AI plumbing matters: MCP (Model Context Protocol) stands out for Azure workflow integration; A2A supports modular, agent-centric tasks. Workshops and technical guides are demystifying adoption, from C# training to executive playbooks—supporting smarter, lower-risk project deployment. The maturing MCP ecosystem, highlighted last week, is quickly broadening developer access.
- [Choosing Between MCP and A2A for AI Applications](/ai/videos/choosing-between-mcp-and-a2a-for-ai-applications)
- [Let''s Learn MCP: C#](/ai/videos/lets-learn-mcp-csharp)
- [Choosing the right AI path for your business - A practical guide for leaders](https://www.microsoft.com/en-us/microsoft-cloud/blog/2025/07/09/choosing-the-right-ai-path-for-your-business-a-practical-guide-for-business-leaders/)',
    'AI saw major progress in scalable reasoning models, agentic modernization, and human-centric frameworks. Microsoft and partners led with deployments that merge performance innovation, agent pipelines, and ethical rollout—reflecting AI’s rapid alignment with real-world systems and responsible integration.',
    1752476400, 'ai', '/ai/roundups/weekly-ai-roundup-2025-07-14', 'TechHub',
    'TechHub', '9D9EF2F139BF84C02D2151D8F441E9774C24F52E44DEEC032D4E67E465AF1657', ',AI,Phi 4,Edge AI,Reasoning Models,Azure AI Foundry,Model Optimization,Agentic AI,Multi Agent Systems,Semantic Kernel,AutoGen,Mainframe Modernization,COBOL Migration,Responsible AI,AI Literacy,MCP,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
-- weekly-ai-roundup-2025-07-07
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ai-roundup-2025-07-07', 'roundups', 'Weekly AI Roundup: Agent Orchestration, Local Models, .NET 9',
    'Recent developments highlight the growing accessibility, modularity, and social impact of AI in the .NET and Microsoft ecosystem. There’s a strong focus on developer-centric agent orchestration, cross-platform model integration, local AI workflows, and impactful applications in areas like education.
<!--excerpt_end-->
## Orchestrating and Standardizing AI Agent Workflows
A new demo shows .NET developers orchestrating multiple AI agents within a Blazor app via the Semantic Kernel Agent Framework. By combining specialized agents (e.g., “Movies” and “Food” assistants) within a concurrency-managed orchestration service, developers enable scalable recommendation/chatbot behaviors. The approach aligns with broader trends—context-driven, modular, and maintainable AI—offering immediately practical blueprints.
Microsoft’s upcoming ‘MCP Dev Days’ will dive deep into the Model Context Protocol, covering live IDE integrations, secure agent server construction, and rapid onboarding. MCP’s standardization will bring uniformity, secure onboarding, and easier model swapping, benefiting both enterprise and open-source projects.
- [Orchestrating AI Agents in Blazor Using Microsoft Semantic Kernel](/ai/videos/orchestrating-ai-agents-in-blazor-using-microsoft-semantic-kernel)
- [Join Us for MCP Dev Days – July 29-30: Deep Dive into the Model Context Protocol](https://devblogs.microsoft.com/blog/join-us-for-mcp-dev-days-july-29-30)
## Evolving .NET AI Support and Local AI Workflows
The .NET AI Community Standup covered new AI/ML features in .NET 9, especially native tensor support for C#, with more improvements previewed for .NET 10. These enhancements allow end-to-end ML workflows directly in .NET, simplifying deployments and unlocking advanced, high-performance data tasks for C# developers.
Bruno Capuano’s guide to generating AltText in C# using local Ollama models illustrates practical, privacy-preserving AI running off the dev’s own machine. Leveraging ‘dotnet run app.cs’ for lightweight scripting, this workflow enables fast, offline, and compliant accessibility automation.
- [.NET AI Community Standup - AI in .NET - What’s New, What’s Next](/ai/videos/net-ai-community-standup-ai-in-net-whats-new-whats-next)
- [Local AI + .NET: Generate AltText with C# Scripts and Ollama](https://devblogs.microsoft.com/dotnet/alttext-generator-csharp-local-models/)
## Global AI Initiatives and Impact
Microsoft and the Philippine Department of Education are scaling AI-fueled literacy solutions to more classrooms, personalizing learning, tracking student progress, and supporting teachers—especially in underserved areas. The partnership spotlights AI’s power in societal advancement, while presenting engineering challenges in scaling, localization, and security.
- [DepEd and Microsoft expand AI-powered literacy initiatives across the Philippines](https://news.microsoft.com/source/asia/2025/07/01/deped-and-microsoft-expand-ai-powered-literacy-initiatives-across-the-philippines/)',
    'Recent developments highlight the growing accessibility, modularity, and social impact of AI in the .NET and Microsoft ecosystem. There’s a strong focus on developer-centric agent orchestration, cross-platform model integration, local AI workflows, and impactful applications in areas like education.',
    1751871600, 'ai', '/ai/roundups/weekly-ai-roundup-2025-07-07', 'TechHub',
    'TechHub', '4E741F4FA83143CE66F2780A704E316C7687CEF6CB58AE36B750FA48A957FC41', ',AI,.NET,.NET 9,.NET 10,C#,Blazor,Semantic Kernel,AI Agents,Agent Orchestration,MCP,IDE Integration,Local AI,Ollama,Alt Text Generation,AI in Education,Roundups,',
    true, false, false, false, false,
    false, false, 1
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
